#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Wisej.Web;
using Captain.Common.Utilities;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Objects;
using Captain.Common.Model.Data;
using Captain.Common.Views.UserControls;

#endregion


namespace Captain.Common.Views.Forms
{
    public partial class PIR20001HardcodedQues : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;

        #endregion
        public PIR20001HardcodedQues(BaseForm baseForm, PrivilegeEntity privileges, string Mode_from, PIRMISCEntity MiscEntity)
        {
            InitializeComponent();
            _model = new CaptainModel();
            BaseForm = baseForm;
            Privileges = privileges;
            Mode = Mode_from;
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            this.Text = /*privileges.Program + " - " + */"Hardcoded Question Options";
            FillAgeCombo();
            if (Mode == "Add")
            {
                FillGrids();
            }
            else
            {
                FillEntity();
                //PropPirMiscEntity = MiscEntity;
                CommonFunctions.SetComboBoxValue(cmbAgetype, PropPirMiscEntity.C9_Agy_Type);
                if (PropPirMiscEntity.Sites_Flag.Trim() == "A") chkbSites.Checked = true;
                else chkbSites.Checked = false;
                if (PropPirMiscEntity.Taskfund.Trim() == "Y")
                    chkdisgred.Checked = true;
                else
                    chkdisgred.Checked = false;
                FillGrids();
            }
        }

        #region Properties
        public BaseForm BaseForm { get; set; }
        public PrivilegeEntity Privileges { get; set; }
        public PIRMISCEntity PropPirMiscEntity { get; set; }
        public string Mode { get; set; }
        #endregion

        public void FillEntity()
        {
            List<PIRMISCEntity> misc_entity = new List<PIRMISCEntity>();
            PIRMISCEntity Search_Entity = new PIRMISCEntity(true);
            misc_entity = _model.ChldAttnData.Browse_PIRMISC(Search_Entity, "Browse");
            PropPirMiscEntity = misc_entity.Find(u => u.Agency.Equals(BaseForm.BaseAgency) && u.Dept.Equals(BaseForm.BaseDept) && u.Prog.Equals(BaseForm.BaseProg) && u.Year.Equals(BaseForm.BaseYear));
        }

        private void FillAgeCombo()
        {
            cmbAgetype.Items.Insert(0, new ListItem("As of Today", "T"));
            cmbAgetype.Items.Insert(1, new ListItem("As of 1 day of Attn", "F"));
            cmbAgetype.Items.Insert(2, new ListItem("As of last day of Attn", "L"));
            cmbAgetype.SelectedIndex = 0;

        }
        List<string> Site_list = new List<string>();
        private void FillGrids()
        {
            int rowIndex = 0;
            DataSet dsEducation = Captain.DatabaseLayer.AgyTab.GetAgyTabDetails("00007");
            DataTable dtEducation = dsEducation.Tables[0];
            if (dtEducation.Rows.Count > 0)
            {
                foreach (DataRow dr in dtEducation.Rows)
                {
                    rowIndex = gvEducation.Rows.Add(dr["AGY_1"].ToString().Trim(), dr["AGY_7"].ToString().Trim(), dr["AGY_2"].ToString().Trim(), dr["AGY_2"].ToString().Trim());
                }
            }


            CaseSiteEntity Search_Site = new CaseSiteEntity(true);
            Search_Site.SiteAGENCY = BaseForm.BaseAgency; Search_Site.SiteDEPT = BaseForm.BaseDept; Search_Site.SitePROG = BaseForm.BaseProg;
            Search_Site.SiteYEAR = BaseForm.BaseYear;
            List<CaseSiteEntity> Sites = _model.CaseMstData.Browse_CASESITE_PIR(Search_Site, "Browse", "PIRMISC");
            int Row_site_index = 0;
            if (Sites.Count > 0)
            {
                if (PropPirMiscEntity != null)
                {
                    string siteCodes = PropPirMiscEntity.Sites.Trim();
                    if (!string.IsNullOrEmpty(siteCodes))
                    {
                        for (int i = 0; i < siteCodes.Length; )
                        {
                            Site_list.Add(siteCodes.Substring(i, 9));
                            i += 9;
                        }
                    }
                }
                foreach (CaseSiteEntity item in Sites)
                {
                    if (item.SiteROOM != "0000")
                    {
                        string strSiteRoom = item.SiteROOM;
                        if (strSiteRoom.Length == 1)
                            strSiteRoom = item.SiteROOM + "   ";
                        if (strSiteRoom.Length == 2)
                            strSiteRoom = item.SiteROOM + "  ";
                        if (strSiteRoom.Length == 3)
                            strSiteRoom = item.SiteROOM + " ";


                        Row_site_index = gvSites.Rows.Add(false, item.SiteNUMBER.Trim(), item.SiteNAME.Trim(), strSiteRoom, item.SiteAM_PM, item.SiteNUMBER + strSiteRoom + item.SiteAM_PM);

                        if (Site_list.Contains(item.SiteNUMBER + strSiteRoom + item.SiteAM_PM))
                            gvSites.Rows[Row_site_index].Cells["Site_Check"].Value = true;
                    }
                }

            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            PIRMISCEntity Entity = new PIRMISCEntity();
            Entity.Agency = BaseForm.BaseAgency;
            Entity.Dept = BaseForm.BaseDept;
            Entity.Prog = BaseForm.BaseProg;
            Entity.Year = BaseForm.BaseYear;
            Entity.C9_Agy_Type = ((ListItem)cmbAgetype.SelectedItem).Value.ToString().Trim();
            if (chkbSites.Checked == true) Entity.Sites_Flag = "A"; else Entity.Sites_Flag = "S";
            string Sites_collection = string.Empty;
            Entity.Taskfund = chkdisgred.Checked == true ? "Y" : "N";
            foreach (DataGridViewRow dr in gvSites.Rows)
            {
                if (chkbSites.Checked == false)
                {
                    if (dr.Cells["Site_Check"].Value != null && Convert.ToBoolean(dr.Cells["Site_Check"].Value) == true)
                        Sites_collection += dr.Cells["Sites"].Value.ToString(); //+ dr.Cells["Room"].Value.ToString() + dr.Cells["AMPM"].Value.ToString();
                }
                else
                {
                    Sites_collection += dr.Cells["Sites"].Value.ToString();
                }
            }
            Entity.Sites = Sites_collection;
            Entity.Mode = Mode;

            if (_model.ChldAttnData.InsertUpdatePIRMISC(Entity, "Update"))
            {
                foreach (DataGridViewRow gvEdRow in gvEducation.Rows)
                {
                    string Temp = string.Empty;
                    if (!string.IsNullOrEmpty(Convert.ToString(gvEdRow.Cells["Priorty"].Value))) Temp = gvEdRow.Cells["Priorty"].Value.ToString();
                    _model.ChldAttnData.UpdateAgyTab_pir(gvEdRow.Cells["Code"].Value.ToString(), Temp);
                }

                this.Close();
            }

        }

        private void chkbSites_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbSites.Checked == true)
            {
                gvSites.Enabled = false;
                foreach (DataGridViewRow drsite in gvSites.Rows)
                {
                    drsite.Cells["Site_Check"].Value = false;
                }
            }
            else
            {
                gvSites.Enabled = true;
                foreach (DataGridViewRow drsite in gvSites.Rows)
                {
                    if (Site_list.Contains(drsite.Cells["Sites"].Value.ToString()))
                        drsite.Cells["Site_Check"].Value = true;
                }
            }

        }

        private void gvEducation_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (gvEducation.Rows.Count > 0)
            {
                gvEducation.CellValueChanged -= new DataGridViewCellEventHandler(gvEducation_CellValueChanged);
                if (e.ColumnIndex == Priorty.Index)
                {
                    int introwindex = gvEducation.CurrentCell.RowIndex;
                    int intcolumnindex = gvEducation.CurrentCell.ColumnIndex;
                    string strAmtValue = Convert.ToString(gvEducation.Rows[introwindex].Cells[intcolumnindex].Value);
                    if (!string.IsNullOrEmpty(strAmtValue))
                    {
                        if (!System.Text.RegularExpressions.Regex.IsMatch(strAmtValue, Consts.StaticVars.NumericString))
                        {
                            gvEducation.Rows[introwindex].Cells[intcolumnindex].Value = gvEducation.Rows[introwindex].Cells[intcolumnindex + 1].Value;
                        }
                    }
                }
            }
        }
    }
}