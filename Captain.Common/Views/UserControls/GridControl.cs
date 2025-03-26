#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Data;
using System.Drawing;
using System.Text;
using Captain.Common.EventArg;
using Captain.Common.Handlers;
using Captain.Common.Utilities;
using Captain.Common.Views.Forms;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Menus;
using Captain.Common.Model.Objects;
using Captain.Common.Model.Data;
using Captain.Common.Views.UserControls.Base;
using Wisej.Web;
#endregion

namespace Captain.Common.Views.UserControls
{
    public partial class GridControl : UserControl
    {
        private List<HierarchyEntity> _selectedHierarchies = new List<HierarchyEntity>();
        private CaptainModel _model = null;

        public GridControl(BaseForm baseForm, string hieType, UserEntity userProfile)
        {
            InitializeComponent();
            _model = new CaptainModel();
            BaseForm = baseForm;
            UserProfile = userProfile;
            //ListcaseSiteEntity = new List<CaseSiteEntity>();
            //DataSet dsAgency = Captain.DatabaseLayer.ADMNB001DB.ADMNB001_Browse_AGCYCNTL("00", null, null, null, null, null, null);
            //if (dsAgency != null && dsAgency.Tables[0].Rows.Count > 0)
            //{
            //    AGYSiteSec = dsAgency.Tables[0].Rows[0]["ACR_SITE_SEC"].ToString().Trim();
            //}
            //FillSites();
            HieType = hieType;
            GridViewControl = gvwControl;
            olblRequired = lblRequired;
        }


        public GridControl(BaseForm baseForm, string hieType, UserEntity userProfile, string strMode)
        {
            InitializeComponent();
            _model = new CaptainModel();
            BaseForm = baseForm;
            UserProfile = userProfile;
            Mode = strMode;
            ListcaseSiteEntity = new List<CaseSiteEntity>();
            DataSet dsAgency = Captain.DatabaseLayer.ADMNB001DB.ADMNB001_Browse_AGCYCNTL("00", null, null, null, null, null, null);
            if (dsAgency != null && dsAgency.Tables[0].Rows.Count > 0)
            {
                AGYSiteSec = dsAgency.Tables[0].Rows[0]["ACR_SITE_SEC"].ToString().Trim();
            }
            //FillSites();
            HieType = hieType;
            GridViewControl = gvwControl;
            olblRequired = lblRequired;
            if (AGYSiteSec == "1")
            {
                this.Size = new System.Drawing.Size(604, 172);
                this.picEdit.Location = new System.Drawing.Point(576, 9);
                this.gvwControl.Size = new System.Drawing.Size(572, 169);
                this.cellSites.Visible = true;
            }
            else
            {
                this.Size = new System.Drawing.Size(454, 172);
                this.picEdit.Location = new System.Drawing.Point(426, 9);
                this.gvwControl.Size = new System.Drawing.Size(422, 169);
                this.cellSites.Visible = false;
            }

        }

        #region Public Properties

        public UserEntity UserProfile { get; set; }

        public BaseForm BaseForm { get; set; }

        public string Mode { get; set; }
        public string HieType { get; set; }
        public string AGYSiteSec { get; set; }

        public bool SetEditable
        {
            set
            {
                picAdd.Visible = !value;
                picEdit.Visible = value;
            }
        }

        public bool SetVisible
        {
            set
            {
                picAdd.Visible = value;
                picEdit.Visible = value;
            }
        }

        public List<CaseSiteEntity> ListcaseSiteEntity { get; set; }

        public List<HierarchyEntity> UserHierarchy { get; set; }

        public List<HierarchyEntity> proppUserIntakeHierarchy { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataGridView GridViewControl
        {
            get;
            set;
        }
        public Label olblRequired
        {
            get;
            set;
        }

        public List<HierarchyEntity> SelectedHierarchies
        {
            get
            {
                _selectedHierarchies = new List<HierarchyEntity>();
                foreach (DataGridViewRow row in gvwControl.Rows)
                {
                    HierarchyEntity hierarchy = row.Tag as HierarchyEntity;
                    if (!hierarchy.UsedFlag.Equals("Y"))
                    {
                        _selectedHierarchies.Add(hierarchy);
                    }
                }
                return _selectedHierarchies;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnHierarchieFormClosed(object sender, FormClosedEventArgs e)
        {
            // HierarchieSelectionForm form = sender as HierarchieSelectionForm;
            HierarchieSelectionFormNew form = sender as HierarchieSelectionFormNew;
            TagClass selectedTabTagClass = BaseForm.ContentTabs.SelectedTab.Tag as TagClass;

            if (form.DialogResult == DialogResult.OK)
            {
                List<HierarchyEntity> selectedHierarchies = form.SelectedHierarchies;

                List<HierarchyEntity> usedHierarchies = (from c in gvwControl.Rows.Cast<DataGridViewRow>().ToList()
                                                         select ((DataGridViewRow)c).Tag as HierarchyEntity).ToList();
                //usedHierarchies = usedHierarchies.FindAll(u => u.UsedFlag.Equals("Y"));
                gvwControl.Rows.Clear();
                foreach (HierarchyEntity row in usedHierarchies)
                {
                    string code = row.Agency + "-" + (row.Dept == string.Empty ? "**" : row.Dept) + "-" + (row.Prog == string.Empty ? "**" : row.Prog);
                    int rowIndex = gvwControl.Rows.Add(code, row.HirarchyName.ToString(), row.Sites);
                    HierarchyEntity hieEntity = selectedHierarchies.Find(u => u.Code.Replace("-", string.Empty).Equals(row.Code.Replace("-", string.Empty)));
                    // HierarchyEntity hieEntity = selectedHierarchies.Find(u => u.Agency+u.Dept+u.Prog == row.Agency+row.Dept+row.Prog);
                    if (hieEntity != null)
                    {
                        row.UsedFlag = "N";
                        gvwControl.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Black;
                    }
                    else
                    {
                        row.UsedFlag = "Y";
                        gvwControl.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    }
                    gvwControl.Rows[rowIndex].Tag = row;
                }
                selectedHierarchies = selectedHierarchies.FindAll(u => !u.UsedFlag.Equals("Y"));
                foreach (HierarchyEntity row in selectedHierarchies)
                {
                    HierarchyEntity hieEntity = usedHierarchies.Find(u => u.Code.Replace("-", string.Empty).Equals(row.Code.Replace("-", string.Empty)));
                    if (hieEntity == null)
                    {
                        int rowIndex = gvwControl.Rows.Add(row.Code, row.HirarchyName.ToString(), row.Sites);
                        gvwControl.Rows[rowIndex].Tag = row;
                    }
                }
                ////RefreshGrid();
            }
        }

        private void OnAddClick(object sender, EventArgs e)
        {
            try
            {
                if (HieType == "Service")
                {
                    SERHIESEL_FORM addForm = new SERHIESEL_FORM(BaseForm, proppUserIntakeHierarchy, "Add", "I", "*", "I", UserProfile, SelectedHierarchies);
                    addForm.StartPosition = FormStartPosition.CenterScreen;
                    addForm.FormClosed += new FormClosedEventHandler(OnSERHIESELFormClosed);
                    addForm.ShowDialog();
                }
                else
                {
                    HierarchieSelectionFormNew addForm = new HierarchieSelectionFormNew(BaseForm, SelectedHierarchies, "Add", "I", "*", "I", UserProfile,"Y");
                    addForm.StartPosition = FormStartPosition.CenterScreen;
                    addForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
                    addForm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                //
            }
        }

        private void OnEditClick(object sender, EventArgs e)
        {
            try
            {
                if (HieType == "Service")
                {
                    SERHIESEL_FORM hsForm = new SERHIESEL_FORM(BaseForm, proppUserIntakeHierarchy, "Edit", "I", "*", "I", UserProfile, SelectedHierarchies);
                    hsForm.StartPosition = FormStartPosition.CenterScreen;
                    hsForm.FormClosed += new FormClosedEventHandler(OnSERHIESELFormClosed);
                    hsForm.ShowDialog();

                }
                else
                {
                    //HierarchieSelectionForm hsForm = new HierarchieSelectionForm(BaseForm, SelectedHierarchies, HieType, UserProfile, "Edit");  
                    HierarchieSelectionFormNew hsForm = new HierarchieSelectionFormNew(BaseForm, SelectedHierarchies, "Edit", "I", "*", "I", UserProfile,"Y");
                    hsForm.StartPosition = FormStartPosition.CenterScreen;
                    hsForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
                    hsForm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                //
            }
        }

        #endregion

        string PrivHier = string.Empty;
        private void gvwControl_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                string Hierar = gvwControl.CurrentRow.Cells["cellCode"].Value.ToString();
                if (AGYSiteSec == "1" && Mode == "Edit" && HieType == "InTake" && Hierar != "**-**-**")
                {
                    if (PrivHier != Hierar) ListcaseSiteEntity.Clear();

                    string Agency = gvwControl.CurrentRow.Cells["cellCode"].Value.ToString().Substring(0, 2);
                    string Dept = gvwControl.CurrentRow.Cells["cellCode"].Value.ToString().Substring(3, 2);
                    string Prog = gvwControl.CurrentRow.Cells["cellCode"].Value.ToString().Substring(6, 2);

                    PrivHier = Hierar;

                    Site_List = _model.CaseMstData.GetCaseSite(Agency, Dept, Prog, "SiteHie");
                    if (!string.IsNullOrEmpty(gvwControl.CurrentRow.Cells["cellSites"].Value.ToString().Trim()))
                    {
                        string Sites = gvwControl.CurrentRow.Cells["cellSites"].Value.ToString();
                        if (Sites.Length > 0)
                        {
                            string[] SelSites = Sites.Split(',');
                            FillSites(SelSites);
                        }
                    }


                    SelectZipSiteCountyForm siteform = new SelectZipSiteCountyForm(BaseForm, ListcaseSiteEntity, Agency, Dept, Prog, string.Empty);
                    siteform.StartPosition = FormStartPosition.CenterScreen;
                    siteform.FormClosed += new FormClosedEventHandler(SelectZipSiteCountyFormClosed);
                    siteform.ShowDialog();
                }
            }
        }

        private void SelectZipSiteCountyFormClosed(object sender, FormClosedEventArgs e)
        {

            SelectZipSiteCountyForm form = sender as SelectZipSiteCountyForm;
            if (form.DialogResult == DialogResult.OK)
            {
                //if (form.FormType == "CASESITE")
                //{
                ListcaseSiteEntity = form.SelectedCaseSiteEntity;
                string Sites = string.Empty;
                if (ListcaseSiteEntity.Count > 0)
                {
                    foreach (CaseSiteEntity Entity in ListcaseSiteEntity)
                    {
                        if (!string.IsNullOrEmpty(Entity.SiteNUMBER.Trim()))
                            Sites += Entity.SiteNUMBER.Trim() + ",";
                    }
                }

                if (Sites.Length > 0) Sites = Sites.Substring(0, Sites.Length - 1).ToString();

                if (gvwControl.Rows.Count > 0)
                {
                    foreach (DataGridViewRow dr in gvwControl.Rows)
                    {
                        if (dr.Cells["cellCode"].Value.ToString() == gvwControl.CurrentRow.Cells["cellCode"].Value.ToString())
                            dr.Cells["cellSites"].Value = Sites;
                    }

                }

                //if (Rb_Site_Sel.Checked == true && ListcaseSiteEntity.Count > 0)
                //    Txt_Sel_Site.Text = ListcaseSiteEntity[0].SiteNUMBER.ToString();
                //else
                //    Txt_Sel_Site.Clear();
                //}


                //ZipCodeEntity zipcodedetais = form.ListOfSelectedCaseSite;
                //if (zipcodedetais != null)
                //{
                //    string zipPlus = zipcodedetais.Zcrplus4;
                //    txtZipPlus.Text = "0000".Substring(0, 4 - zipPlus.Length) + zipPlus;
                //    txtZipCode.Text = "00000".Substring(0, 5 - zipcodedetais.Zcrzip.Length) + zipcodedetais.Zcrzip;
                //    txtState.Text = zipcodedetais.Zcrstate;
                //    txtCity.Text = zipcodedetais.Zcrcity;
                //    SetComboBoxValue(cmbCounty, zipcodedetais.Zcrcountry);
                //    SetComboBoxValue(cmbTownship, zipcodedetais.Zcrcitycode);

                //}
            }
        }

        List<CaseSiteEntity> Site_List = new List<CaseSiteEntity>();

        private void FillSites(string[] Selsites)
        {
            //Site_List = _model.CaseMstData.GetCaseSite(_intakeHierarchy.GridViewControl.CurrentRow.Cells["code"].Value.ToString().Substring(0, 2), _intakeHierarchy.GridViewControl.CurrentRow.Cells["code"].Value.ToString().Substring(3, 2), _intakeHierarchy.GridViewControl.CurrentRow.Cells["code"].Value.ToString().Substring(6, 2), "SiteHie");
            ListcaseSiteEntity.Clear();
            if (Selsites.Length > 0)
            {
                for (int i = 0; i < Selsites.Length; i++)
                {
                    if (!string.IsNullOrEmpty(Selsites[i].ToString().Trim()))
                    {
                        foreach (CaseSiteEntity casesite in Site_List) //Site_List)//ListcaseSiteEntity)
                        {
                            if (Selsites[i].ToString() == casesite.SiteNUMBER)
                            {
                                ListcaseSiteEntity.Add(casesite);
                                break;
                            }
                            // Sel_Site_Codes += "'" + casesite.SiteNUMBER + "' ,";
                        }
                    }
                }
            }
        }


        private void OnSERHIESELFormClosed(object sender, FormClosedEventArgs e)
        {
            // HierarchieSelectionForm form = sender as HierarchieSelectionForm;
            SERHIESEL_FORM form = sender as SERHIESEL_FORM;
            TagClass selectedTabTagClass = BaseForm.ContentTabs.SelectedTab.Tag as TagClass;

            if (form.DialogResult == DialogResult.OK)
            {
                List<HierarchyEntity> selectedHierarchies = form.ListOfSelectedServiceHierarchies;

                List<HierarchyEntity> usedHierarchies = (from c in gvwControl.Rows.Cast<DataGridViewRow>().ToList()
                                                         select ((DataGridViewRow)c).Tag as HierarchyEntity).ToList();
                //usedHierarchies = usedHierarchies.FindAll(u => u.UsedFlag.Equals("Y"));
                gvwControl.Rows.Clear();
                foreach (HierarchyEntity row in usedHierarchies)
                {
                    string code = (row.Agency == string.Empty ? "**" : row.Agency) + "-" + (row.Dept == string.Empty ? "**" : row.Dept) + "-" + (row.Prog == string.Empty ? "**" : row.Prog);
                    int rowIndex = gvwControl.Rows.Add(code, row.HirarchyName.ToString(), row.Sites);
                    HierarchyEntity hieEntity = selectedHierarchies.Find(u => u.Code.Replace("-", string.Empty).Equals(row.Code.Replace("-", string.Empty)));
                    // HierarchyEntity hieEntity = selectedHierarchies.Find(u => u.Agency+u.Dept+u.Prog == row.Agency+row.Dept+row.Prog);
                    if (hieEntity != null)
                    {
                        row.UsedFlag = "N";
                        gvwControl.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Black;
                    }
                    else
                    {
                        row.UsedFlag = "Y";
                        gvwControl.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    }
                    gvwControl.Rows[rowIndex].Tag = row;
                }
                selectedHierarchies = selectedHierarchies.FindAll(u => !u.UsedFlag.Equals("Y"));
                foreach (HierarchyEntity row in selectedHierarchies)
                {
                    HierarchyEntity hieEntity = usedHierarchies.Find(u => u.Code.Replace("-", string.Empty).Equals(row.Code.Replace("-", string.Empty)));
                    if (hieEntity == null)
                    {
                        int rowIndex = gvwControl.Rows.Add(row.Code, row.HirarchyName.ToString(), row.Sites);
                        gvwControl.Rows[rowIndex].Tag = row;
                    }
                }
                ////RefreshGrid();
                ///


            }
        }


    }
}