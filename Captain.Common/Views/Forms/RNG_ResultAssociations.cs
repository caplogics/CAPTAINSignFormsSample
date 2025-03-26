#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using Wisej.Web;
using Wisej.Design;
using System.Web.Configuration;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Utilities;
using Captain.Common.Menus;
using System.Data.SqlClient;
using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Views.UserControls.Base;
using Captain.Common.Exceptions;
using System.Diagnostics;
using Captain.Common.Views.Forms;
using Captain.Common.Views.UserControls;
#endregion

namespace Captain.Common.Views.Forms
{
    public partial class RNG_ResultAssociations : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        private bool boolChangeStatus = false;

        public int strIndex = 0;
        public int strCrIndex = 0;
        public int strPageIndex = 1;

        #endregion

        public RNG_ResultAssociations(BaseForm baseform, string Mode, string Reffdate, string RefTdate, string gridType, string Grpcd, string tblcd, string referDesc, PrivilegeEntity priviliages)
        {
            InitializeComponent();

            BaseForm = baseform;
            Privileage = priviliages;
            _model = new CaptainModel();
            mode = Mode;
            GridType = gridType;
            refFdate = Reffdate;
            refTdate = RefTdate;
            ReferDesc = referDesc;
            groupCd = Grpcd;
            tablecd = tblcd;
            this.Text = "Outcome Indicators - Result Associations";
            lblRefDt.Text = refFdate.ToString() + "    " + ReferDesc.ToString();
            FillCntrols();
            fillGvResults();
            mode = "Edit";
        }

        #region properties

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileage { get; set; }

        public string mode { get; set; }

        public string GridType { get; set; }

        public string refFdate { get; set; }

        public string refTdate { get; set; }

        public string ReferDesc { get; set; }

        public string groupCd { get; set; }

        public string tablecd { get; set; }

        public bool IsSaveValid { get; set; }

        #endregion

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {

        }

        //string Img_Blank = Consts.Icons.ico_Blank;
        //string Img_Tick = new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("tick.ico");

        string Img_Tick = "icon-gridtick";//"icon-done?color=#01a601";
        string Img_Blank = "blank";

        private void FillCntrols()
        {
            List<RepListItem> list;
            List<RCsb14GroupEntity> csbcntrls;
            csbcntrls = _model.SPAdminData.Browse_RNGGrp(null, null, null, null, null, BaseForm.UserID, string.Empty);
            string UserAgency = string.Empty;
            if (BaseForm.BaseAgencyuserHierarchys.Count > 0)
            {
                HierarchyEntity SelHie = BaseForm.BaseAgencyuserHierarchys.Find(u => u.Code == "******");
                if (SelHie != null)
                    UserAgency = "**";
            }

            if (csbcntrls.Count > 0)
            {
                csbcntrls = csbcntrls.FindAll(u => u.Agency == refTdate || u.Agency == UserAgency);
            }

            if (csbcntrls.Count > 0)
            {
                foreach (RCsb14GroupEntity drg in csbcntrls)
                {
                    if (drg.Code.ToString() == refFdate && drg.Agency.ToString() == refTdate && drg.GrpCode.ToString() == groupCd && string.IsNullOrWhiteSpace(drg.TblCode.ToString()))
                    {
                        txtCode.Text = drg.GrpCode.ToString();
                        txtDesc.Text = drg.GrpDesc.ToString();
                        if (drg.Hrd1.ToString() != string.Empty)
                            CmbRsltHead.Items.Add(new RepListItem(drg.Hrd1.ToString(), "1"));
                        if (drg.Hrd2.ToString() != string.Empty)
                            CmbRsltHead.Items.Add(new RepListItem(drg.Hrd2.ToString(), "2"));
                        if (drg.Hrd3.ToString() != string.Empty)
                            CmbRsltHead.Items.Add(new RepListItem(drg.Hrd3.ToString(), "3"));
                        if (drg.Hrd4.ToString() != string.Empty)
                            CmbRsltHead.Items.Add(new RepListItem(drg.Hrd4.ToString(), "4"));
                        if (drg.Hrd5.ToString() != string.Empty)
                            CmbRsltHead.Items.Add(new RepListItem(drg.Hrd5.ToString(), "5"));
                    }

                }
                CmbRsltHead.SelectedIndex = 0;
            }
        }

        List<SPCommonEntity> ResultList;
        List<RNGRAEntity> Sel_resultEntity;
        private void fillGvResults()
        {
            if (CmbRsltHead.Items.Count > 0)
            {
                Sel_resultEntity = _model.SPAdminData.Browse_RNGRA(refFdate, groupCd, null);
                ResultList = _model.SPAdminData.Get_AgyRecs("Results");
                GvResults.Rows.Clear();

                foreach (SPCommonEntity Entity in ResultList)
                {
                    string Result = Entity.Code;
                    string ResultDesc = Entity.Desc;
                    bool Ststus_Exists = false; bool Already_Exsists = false;
                    foreach (RNGRAEntity dr in Sel_resultEntity)
                    {
                        string rescode = dr.ResCode.ToString();
                        string resdesc = dr.Desc.ToString();
                        if (dr.ResCode.ToString().Trim() == Result.ToString().Trim() && dr.CntCode.ToString() == ((RepListItem)CmbRsltHead.SelectedItem).Value.ToString())
                        {
                            dr.Sel_Switch = true;
                            Ststus_Exists = dr.Sel_Switch;
                        }
                        else if (dr.ResCode.ToString().Trim() == Result.ToString().Trim())
                            Already_Exsists = true;
                    }
                    if (Ststus_Exists)
                        GvResults.Rows.Add(Img_Tick, Entity.Desc, Entity.Code, "Y");
                    else
                    {
                        if (!Already_Exsists)
                            GvResults.Rows.Add(Img_Blank, Entity.Desc, Entity.Code, "N");
                    }

                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CmbRsltHead_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillGvResults();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            //GvResults.Enabled = true;
            GvResults.ReadOnly = true;
            if (btnSave.Text == "&Save")
            {
                if (GvResults.Rows.Count > 0)
                {
                    try
                    {
                        Captain.DatabaseLayer.SPAdminDB.DeleteRNGRA(refFdate, refTdate, groupCd, ((RepListItem)CmbRsltHead.SelectedItem).Value.ToString());

                        CaptainModel model = new CaptainModel();
                        RNGRAEntity resultEntity = new RNGRAEntity();

                        resultEntity.Code = refFdate.ToString();
                        resultEntity.Agency = refTdate.ToString();
                        resultEntity.GrpCode = groupCd.ToString();
                        resultEntity.CntCode = ((RepListItem)CmbRsltHead.SelectedItem).Value.ToString();
                        resultEntity.LSTCOperator = BaseForm.UserID;
                        foreach (DataGridViewRow dr in GvResults.Rows)
                        {
                            if (dr.Cells["Img_Cd"].Value.ToString() == "Y")
                            {
                                resultEntity.ResCode = dr.Cells["Agy_Code"].Value.ToString();
                                resultEntity.Desc = dr.Cells["Results"].Value.ToString();
                                _model.SPAdminData.InsertUpdateRNGRA(resultEntity);
                            }

                        }
                        if (strCrIndex != 0)
                            strCrIndex = strCrIndex - 1;

                        GvResults.Enabled = true;
                        GvResults.ReadOnly = false;
                        //btnSave.Text = "Ch&ange";

                        fillGvResults();
                        AlertBox.Show("Details Saved Successfully");
                        if (GvResults.Rows.Count != 0)
                        {
                            GvResults.Rows[strCrIndex].Selected = true;
                        }
                        //mode = "View";
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            else
            {
                //btnSave.Text = "S&ave";
                mode = "Edit";
            }


        }

        private void GvResults_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (GvResults.Rows.Count > 0)
            {
                if (e.RowIndex > -1)
                {
                    if (e.ColumnIndex == 0 && (mode.Equals("Add") || mode.Equals("Edit")))
                    {
                        if (GvResults.CurrentRow.Cells["Img_Cd"].Value.ToString() == "Y")
                        {
                            GvResults.CurrentRow.Cells["Grid_Img"].Value = Img_Blank;
                            GvResults.CurrentRow.Cells["Img_Cd"].Value = "N";
                        }
                        else
                        {
                            GvResults.CurrentRow.Cells["Grid_Img"].Value = Img_Tick;
                            GvResults.CurrentRow.Cells["Img_Cd"].Value = "Y";
                        }
                    }
                }
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            //Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "Performance");
        }

    }
}