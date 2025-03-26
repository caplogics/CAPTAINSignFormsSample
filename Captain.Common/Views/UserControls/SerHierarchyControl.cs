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
using mshtml;
#endregion

namespace Captain.Common.Views.UserControls
{
    public partial class SerHierarchyControl : UserControl
    {
        private List<HierarchyEntity> _selectedHierarchies = new List<HierarchyEntity>();
        private CaptainModel _model = null;

        public SerHierarchyControl(BaseForm baseForm, string hieType, UserEntity userProfile)
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
            GridViewControl = gvSerHie;
            SerGridViewControl = gvSerHie;
        }


        public SerHierarchyControl(BaseForm baseForm, string hieType, UserEntity userProfile, string strMode)
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
            GridViewControl = gvSerHie; // gvwControl;
                                        // SerGridViewControl = gvSerHie;
            if (AGYSiteSec == "1")
            {

                //this.cellSites.Visible = true;
            }
            else
            {

                // this.cellSites.Visible = false;
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

        public DataGridView SerGridViewControl
        {
            get;
            set;
        }

        public List<HierarchyEntity> SelectedHierarchies
        {
            get
            {
                _selectedHierarchies = new List<HierarchyEntity>();
                List<string> lstServices = new List<string>();

                foreach (DataGridViewRow row in gvSerHie.Rows)
                {
                    if (row[2].Value.ToString() != "" && row[3].Value.ToString() != "")
                    {
                        string SearchCode = row[2].Value.ToString();
                        //if (!lstServices.Contains(SearchCode))
                        //{
                        lstServices.Add(SearchCode);
                        HierarchyEntity hierarchy = row.Tag as HierarchyEntity;
                        if (!hierarchy.UsedFlag.Equals("Y"))
                        {
                            _selectedHierarchies.Add(hierarchy);
                        }
                        // }
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

                List<HierarchyEntity> usedHierarchies = (from c in gvSerHie.Rows.Cast<DataGridViewRow>().ToList()
                                                         select ((DataGridViewRow)c).Tag as HierarchyEntity).ToList();
                //usedHierarchies = usedHierarchies.FindAll(u => u.UsedFlag.Equals("Y"));
                gvSerHie.Rows.Clear();
                foreach (HierarchyEntity row in usedHierarchies)
                {
                    string code = row.Agency + "-" + (row.Dept == string.Empty ? "**" : row.Dept) + "-" + (row.Prog == string.Empty ? "**" : row.Prog);
                    int rowIndex = gvSerHie.Rows.Add(code, row.HirarchyName.ToString(), row.Sites);
                    HierarchyEntity hieEntity = selectedHierarchies.Find(u => u.Code.Replace("-", string.Empty).Equals(row.Code.Replace("-", string.Empty)));
                    // HierarchyEntity hieEntity = selectedHierarchies.Find(u => u.Agency+u.Dept+u.Prog == row.Agency+row.Dept+row.Prog);
                    if (hieEntity != null)
                    {
                        row.UsedFlag = "N";
                        gvSerHie.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Black;
                    }
                    else
                    {
                        row.UsedFlag = "Y";
                        gvSerHie.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    }
                    gvSerHie.Rows[rowIndex].Tag = row;
                }
                selectedHierarchies = selectedHierarchies.FindAll(u => !u.UsedFlag.Equals("Y"));
                foreach (HierarchyEntity row in selectedHierarchies)
                {
                    HierarchyEntity hieEntity = usedHierarchies.Find(u => u.Code.Replace("-", string.Empty).Equals(row.Code.Replace("-", string.Empty)));
                    if (hieEntity == null)
                    {
                        int rowIndex = gvSerHie.Rows.Add(row.Code, row.HirarchyName.ToString(), row.Sites);
                        gvSerHie.Rows[rowIndex].Tag = row;
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
                    HierarchieSelectionFormNew addForm = new HierarchieSelectionFormNew(BaseForm, SelectedHierarchies, "Add", "I", "*", "I", UserProfile);
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
                    HierarchieSelectionFormNew hsForm = new HierarchieSelectionFormNew(BaseForm, SelectedHierarchies, "Edit", "I", "*", "I", UserProfile);
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
        private void gvSerHie_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                string Hierar = gvSerHie.CurrentRow.Cells["cellCode"].Value.ToString();
                if (AGYSiteSec == "1" && Mode == "Edit" && HieType == "InTake" && Hierar != "**-**-**")
                {
                    if (PrivHier != Hierar) ListcaseSiteEntity.Clear();

                    string Agency = gvSerHie.CurrentRow.Cells["cellCode"].Value.ToString().Substring(0, 2);
                    string Dept = gvSerHie.CurrentRow.Cells["cellCode"].Value.ToString().Substring(3, 2);
                    string Prog = gvSerHie.CurrentRow.Cells["cellCode"].Value.ToString().Substring(6, 2);

                    PrivHier = Hierar;

                    Site_List = _model.CaseMstData.GetCaseSite(Agency, Dept, Prog, "SiteHie");
                    if (!string.IsNullOrEmpty(gvSerHie.CurrentRow.Cells["cellSites"].Value.ToString().Trim()))
                    {
                        string Sites = gvSerHie.CurrentRow.Cells["cellSites"].Value.ToString();
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

                if (gvSerHie.Rows.Count > 0)
                {
                    foreach (DataGridViewRow dr in gvSerHie.Rows)
                    {
                        if (dr.Cells["cellCode"].Value.ToString() == gvSerHie.CurrentRow.Cells["cellCode"].Value.ToString())
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

        public void RefreshGrid()
        {
            //SERHIESEL_FORM form = sender as SERHIESEL_FORM;
            //TagClass selectedTabTagClass = BaseForm.ContentTabs.SelectedTab.Tag as TagClass;

            List<HierarchyEntity> ointakeHeadHierarchy = proppUserIntakeHierarchy;
            List<HierarchyEntity> oSPHierarchieslst = SelectedHierarchies;// form.ListOfSelectedServiceHierarchies;
            List<HierarchyEntity> usedHierarchies = (from c in gvSerHie.Rows.Cast<DataGridViewRow>().ToList()
                                                     where (c.Cells[2].Value.ToString() != "" && c.Cells[3].Value.ToString() != "")
                                                     select ((DataGridViewRow)c).Tag as HierarchyEntity).ToList();
            if (ointakeHeadHierarchy.Count > 0)
            {
                //ointakeHeadHierarchy = ointakeHeadHierarchy.OrderBy(u => u.UsedFlag).ThenBy(u => u.Agency).ThenBy(u => u.Dept).ThenBy(u => u.Prog).ToList();
                if (oSPHierarchieslst.Count > 0)
                {
                    foreach (HierarchyEntity selEnti in oSPHierarchieslst)
                    {
                        selEnti.Agency = (selEnti.Agency == "" ? "**" : selEnti.Agency);
                        selEnti.Dept = (selEnti.Dept == "" ? "**" : selEnti.Dept);
                        selEnti.Prog = (selEnti.Prog == "" ? "**" : selEnti.Prog);
                    }
                }

                foreach (HierarchyEntity hierarchy in ointakeHeadHierarchy)
                {
                    List<HierarchyEntity> SelusedHierarchies = usedHierarchies.FindAll(x => x.SerAgency == hierarchy.Agency && x.SerDept == hierarchy.Dept && x.SerProg == hierarchy.Prog);
                    if (SelusedHierarchies.Count > 0)
                    {
                        foreach (HierarchyEntity lstHIEenty in SelusedHierarchies)
                        {
                            lstHIEenty.Agency = (lstHIEenty.Agency == "" ? "**" : lstHIEenty.Agency);
                            lstHIEenty.Dept = (lstHIEenty.Dept == "" ? "**" : lstHIEenty.Dept);
                            lstHIEenty.Prog = (lstHIEenty.Prog == "" ? "**" : lstHIEenty.Prog);

                            HierarchyEntity hieEntity = oSPHierarchieslst.Find(x => x.Agency == lstHIEenty.Agency && x.Dept == lstHIEenty.Dept && (x.Prog == lstHIEenty.Prog));
                            if (hieEntity == null)
                            {
                                lstHIEenty.UsedFlag = "Y";
                                oSPHierarchieslst.Add(lstHIEenty);
                            }
                            else
                            {
                                HierarchyEntity unushieEntity = SelusedHierarchies.Find(x => x.Agency == lstHIEenty.Agency && x.Dept == lstHIEenty.Dept && (x.Prog == lstHIEenty.Prog) && (x.UsedFlag == "Y"));
                                if (unushieEntity != null)
                                {


                                    // lstHIEenty.UsedFlag = "Y";
                                    // oSPHierarchieslst.Add(lstHIEenty);

                                    List<HierarchyEntity> selfCheck = oSPHierarchieslst.Where(x => (x.Agency == lstHIEenty.Agency && x.Dept == lstHIEenty.Dept && x.Prog == lstHIEenty.Prog)
                                    && (x.SerAgency == hierarchy.Agency && x.SerDept == hierarchy.Dept && x.SerProg == hierarchy.Prog)
                                    ).ToList();
                                    if (selfCheck.Count == 0)
                                    {
                                        lstHIEenty.UsedFlag = "Y";
                                        oSPHierarchieslst.Add(lstHIEenty);
                                    }
                                }
                            }

                        }
                    }
                }
            }

            List<HierarchyEntity> oSPHeadHierarchy = (from s in oSPHierarchieslst orderby s.Agency, s.Dept, s.Prog select s).ToList();
   
            

            if (ointakeHeadHierarchy.Count > 0)
            {
                ointakeHeadHierarchy = ointakeHeadHierarchy.OrderBy(u => u.UsedFlag).ThenBy(u => u.Agency).ThenBy(u => u.Dept).ThenBy(u => u.Prog).ToList();

                gvSerHie.Rows.Clear();
                List<HierarchyEntity> oSPHierachies = new List<HierarchyEntity>();
                if (oSPHierarchieslst.Count > 0)
                    oSPHierachies = (from s in oSPHierarchieslst orderby s.Agency, s.Dept, s.Prog select s).ToList();

                List<string> lstServices = new List<string>();
                int rowIndex = 0;

                foreach (HierarchyEntity hierarchy in ointakeHeadHierarchy)
                {
                    string Agy = (hierarchy.Agency == "" ? "**" : hierarchy.Agency);
                    string Dept = (hierarchy.Dept == "" ? "**" : hierarchy.Dept);
                    string Prog = (hierarchy.Prog == "" ? "**" : hierarchy.Prog);


                    string SearchIntakCode = Agy + "-" + Dept + "-" + Prog;
                    string hierarchyName = hierarchy.HirarchyName.ToString();

                    rowIndex = GridViewControl.Rows.Add((SearchIntakCode), hierarchyName, "", "");
                    GridViewControl.Rows[rowIndex].Tag = hierarchy;
                    lstServices.Add(SearchIntakCode);
                    if (hierarchy.UsedFlag.Equals("Y"))
                    {
                        GridViewControl.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    }

                    List<HierarchyEntity> SPFilterHier = oSPHierachies.FindAll(x => (x.SerAgency == hierarchy.Agency) && (x.SerDept == hierarchy.Dept) && (x.SerProg == hierarchy.Prog));
                    if (SPFilterHier.Count > 0)
                    {
                        rowIndex = Captain.Common.Utilities.CommonFunctions.BuildSerHIEGrid(SPFilterHier, GridViewControl);
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
                /*
                List<HierarchyEntity> selectedHierarchies = form.ListOfSelectedServiceHierarchies;

                List<HierarchyEntity> usedHierarchies = (from c in gvSerHie.Rows.Cast<DataGridViewRow>().ToList()
                                                         select ((DataGridViewRow)c).Tag as HierarchyEntity).ToList();
                //usedHierarchies = usedHierarchies.FindAll(u => u.UsedFlag.Equals("Y"));
               
                foreach (HierarchyEntity row in usedHierarchies)
                {
                    string code = (row.Agency == string.Empty ? "**" : row.Agency) + "-" + (row.Dept == string.Empty ? "**" : row.Dept) + "-" + (row.Prog == string.Empty ? "**" : row.Prog);
                    int rowIndex = gvSerHie.Rows.Add(code, row.HirarchyName.ToString(), row.Sites);
                    HierarchyEntity hieEntity = selectedHierarchies.Find(u => u.Code.Replace("-", string.Empty).Equals(row.Code.Replace("-", string.Empty)));
                    // HierarchyEntity hieEntity = selectedHierarchies.Find(u => u.Agency+u.Dept+u.Prog == row.Agency+row.Dept+row.Prog);
                    if (hieEntity != null)
                    {
                        row.UsedFlag = "N";
                        gvSerHie.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Black;
                    }
                    else
                    {
                        row.UsedFlag = "Y";
                        gvSerHie.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    }
                    gvSerHie.Rows[rowIndex].Tag = row;
                }
                selectedHierarchies = selectedHierarchies.FindAll(u => !u.UsedFlag.Equals("Y"));
                foreach (HierarchyEntity row in selectedHierarchies)
                {
                    HierarchyEntity hieEntity = usedHierarchies.Find(u => u.Code.Replace("-", string.Empty).Equals(row.Code.Replace("-", string.Empty)));
                    if (hieEntity == null)
                    {
                        int rowIndex = gvSerHie.Rows.Add(row.Code, row.HirarchyName.ToString(), row.Sites);
                        gvSerHie.Rows[rowIndex].Tag = row;
                    }
                }

                ////RefreshGrid();
                ///
                */

                List<HierarchyEntity> ointakeHeadHierarchy = proppUserIntakeHierarchy;
                List<HierarchyEntity> oSPHierarchieslst = form.ListOfSelectedServiceHierarchies;
                List<HierarchyEntity> usedHierarchies = (from c in gvSerHie.Rows.Cast<DataGridViewRow>().ToList()
                                                         where (c.Cells[2].Value.ToString() != "" && c.Cells[3].Value.ToString() != "")
                                                         select ((DataGridViewRow)c).Tag as HierarchyEntity).ToList();
                if (ointakeHeadHierarchy.Count > 0)
                {
                    //ointakeHeadHierarchy = ointakeHeadHierarchy.OrderBy(u => u.UsedFlag).ThenBy(u => u.Agency).ThenBy(u => u.Dept).ThenBy(u => u.Prog).ToList();
                    if (oSPHierarchieslst.Count > 0)
                    {
                        foreach (HierarchyEntity selEnti in oSPHierarchieslst)
                        {
                            selEnti.Agency = (selEnti.Agency == "" ? "**" : selEnti.Agency);
                            selEnti.Dept = (selEnti.Dept == "" ? "**" : selEnti.Dept);
                            selEnti.Prog = (selEnti.Prog == "" ? "**" : selEnti.Prog);
                        }
                    }

                    foreach (HierarchyEntity hierarchy in ointakeHeadHierarchy)
                    {
                        List<HierarchyEntity> SelusedHierarchies = usedHierarchies.FindAll(x => x.SerAgency == hierarchy.Agency && x.SerDept == hierarchy.Dept && x.SerProg == hierarchy.Prog);
                        if (SelusedHierarchies.Count > 0)
                        {
                            foreach (HierarchyEntity lstHIEenty in SelusedHierarchies)
                            {
                                lstHIEenty.Agency = (lstHIEenty.Agency == "" ? "**" : lstHIEenty.Agency);
                                lstHIEenty.Dept = (lstHIEenty.Dept == "" ? "**" : lstHIEenty.Dept);
                                lstHIEenty.Prog = (lstHIEenty.Prog == "" ? "**" : lstHIEenty.Prog);

                                HierarchyEntity hieEntity = oSPHierarchieslst.Find(x => x.Agency == lstHIEenty.Agency && x.Dept == lstHIEenty.Dept && (x.Prog == lstHIEenty.Prog));
                                if (hieEntity == null)
                                {
                                    lstHIEenty.UsedFlag = "Y";
                                    oSPHierarchieslst.Add(lstHIEenty);
                                }
                                else
                                {
                                    HierarchyEntity unushieEntity = SelusedHierarchies.Find(x => x.Agency == lstHIEenty.Agency && x.Dept == lstHIEenty.Dept && (x.Prog == lstHIEenty.Prog) && (x.UsedFlag == "Y"));
                                    if (unushieEntity != null)
                                    {


                                        // lstHIEenty.UsedFlag = "Y";
                                        // oSPHierarchieslst.Add(lstHIEenty);

                                        List<HierarchyEntity> selfCheck = oSPHierarchieslst.Where(x => (x.Agency == lstHIEenty.Agency && x.Dept == lstHIEenty.Dept && x.Prog == lstHIEenty.Prog)
                                        && (x.SerAgency == hierarchy.Agency && x.SerDept == hierarchy.Dept && x.SerProg == hierarchy.Prog)
                                        ).ToList();
                                        if (selfCheck.Count == 0)
                                        {
                                            lstHIEenty.UsedFlag = "Y";
                                            oSPHierarchieslst.Add(lstHIEenty);
                                        }
                                    }
                                }

                            }
                        }
                    }
                }


                //if (usedHierarchies.Count > 0)
                //{
                //    foreach (HierarchyEntity lstHIEenty in usedHierarchies)
                //    {
                //        HierarchyEntity hieEntity = oSPHierarchieslst.Find(x => x.Agency == lstHIEenty.Agency && x.Dept == lstHIEenty.Dept && x.Prog == lstHIEenty.Prog);
                //        if (hieEntity == null)
                //        {
                //            lstHIEenty.UsedFlag = "Y";
                //            oSPHierarchieslst.Add(lstHIEenty);
                //        }
                //        //else
                //        //{
                //        //   if(hieEntity.UsedFlag!=lstHIEenty.UsedFlag)
                //        //    {
                //        //        oSPHierarchieslst.Remove(lstHIEenty);
                //        //        oSPHierarchieslst.Add(hieEntity);
                //        //    }
                //        //}
                //    }
                //}

                List<HierarchyEntity> oSPHeadHierarchy = (from s in oSPHierarchieslst orderby s.Agency, s.Dept, s.Prog select s).ToList();
                //List<HierarchyEntity> oSPHeadHierarchy2 = oSPHeadHierarchy1.OrderBy(x => new { x.Agency, x.Dept, x.Prog }).ToList();
                #region Hierarchy sel Sudheer Code
                /*
                if (ointakeHeadHierarchy.Count>0)
                {
                    gvSerHie.Rows.Clear();

                    List<string> lstServices = new List<string>();
                    int rowIndex = 0;

                    foreach (HierarchyEntity hierarchy in ointakeHeadHierarchy)
                    {
                        string Agy = (hierarchy.Agency == "" ? "**" : hierarchy.Agency);
                        string Dept = (hierarchy.Dept == "" ? "**" : hierarchy.Dept);
                        string Prog = (hierarchy.Prog == "" ? "**" : hierarchy.Prog);


                        string code = Agy + "-" + Dept + "-" + Prog;
                        string hierarchyName = hierarchy.HirarchyName.ToString();

                        string searchHeadCode = hierarchy.Agency + "-**-**";
                        string searchCode = hierarchy.Agency + "-" + hierarchy.Dept + "-**";

                        rowIndex = GridViewControl.Rows.Add((hierarchy.Code), hierarchyName, "", "");

                        GridViewControl.Rows[rowIndex].Tag = hierarchy;
                        lstServices.Add(searchHeadCode);
                        if (hierarchy.UsedFlag.Equals("Y"))
                        {
                            GridViewControl.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;

                        }


                       
                        
                        HierarchyEntity ofilterSerHIE = new HierarchyEntity(null);
                        if (hierarchy.Code=="******")
                        {
                            ofilterSerHIE = oSPHeadHierarchy.Find(x => ((x.Agency == "**" || x.Agency == "") && (x.Dept == "**" || x.Dept == "") && (x.Prog == "**" || x.Prog == "")));
                            if (ofilterSerHIE != null)
                            {
                                if (hierarchy.Code == ofilterSerHIE.Code)
                                {
                                    rowIndex = GridViewControl.Rows.Add("", "", ofilterSerHIE.Code, ofilterSerHIE.HirarchyName);
                                    GridViewControl.Rows[rowIndex].Tag = hierarchy;
                                    lstServices.Add(searchHeadCode);
                                    if (ofilterSerHIE.UsedFlag.Equals("Y"))
                                    {
                                        GridViewControl.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                                    }
                                }

                            }
                        }
                        //List<HierarchyEntity> ofilterSerAgyHIE = oSPHeadHierarchy.FindAll(x => ((x.Agency == hierarchy.Agency && x.Agency!="**")  && (x.Dept == "**" || x.Dept == "") && (x.Prog == "**" || x.Prog == "")));
                        List<HierarchyEntity> ofilterSerAgyHIE = oSPHeadHierarchy.FindAll(x => ((x.Agency == hierarchy.Agency && x.Agency != "**") ));
                        if (ofilterSerAgyHIE.Count>0)
                        {
                            foreach(HierarchyEntity AgyHie in ofilterSerAgyHIE) 
                            {
                                if (hierarchy.Agency == AgyHie.Agency && hierarchy.Dept=="**")
                                {
                                    rowIndex = GridViewControl.Rows.Add("", "", AgyHie.Code, AgyHie.HirarchyName);
                                    GridViewControl.Rows[rowIndex].Tag = AgyHie;
                                    lstServices.Add(searchHeadCode);
                                    if (AgyHie.UsedFlag.Equals("Y"))
                                    {
                                        GridViewControl.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                                    }
                                }
                            }
                        }
                        //List<HierarchyEntity> ofilterSerDepHIE = oSPHeadHierarchy.FindAll(x => ((x.Agency == hierarchy.Agency) && (x.Dept == hierarchy.Dept && x.Dept!="**") && (x.Prog == "**" || x.Prog == "")));
                        List<HierarchyEntity> ofilterSerDepHIE = oSPHeadHierarchy.FindAll(x => ((x.Agency == hierarchy.Agency) && (x.Dept == hierarchy.Dept && x.Dept != "**") ));
                        if (ofilterSerDepHIE.Count>0)
                        {
                            foreach (HierarchyEntity DeptHie in ofilterSerDepHIE)
                            {
                                if (hierarchy.Agency == DeptHie.Agency && hierarchy.Dept==DeptHie.Dept && hierarchy.Prog=="**")
                                {
                                    rowIndex = GridViewControl.Rows.Add("", "", DeptHie.Code, DeptHie.HirarchyName);
                                    GridViewControl.Rows[rowIndex].Tag = DeptHie;
                                    lstServices.Add(searchHeadCode);
                                    if (DeptHie.UsedFlag.Equals("Y"))
                                    {
                                        GridViewControl.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                                    }
                                }
                                    
                            }

                               
                        }
                        //List<HierarchyEntity> ofilterSerProgHIE = oSPHeadHierarchy.FindAll(x => ((x.Agency == hierarchy.Agency) && (x.Dept == hierarchy.Dept) && (x.Prog != "**" && x.Prog == hierarchy.Prog)));
                        List<HierarchyEntity> ofilterSerProgHIE = oSPHeadHierarchy.FindAll(x => ((x.Agency == hierarchy.Agency) && (x.Dept == hierarchy.Dept) && (x.Prog != "**" && x.Prog == hierarchy.Prog)));
                        if (ofilterSerProgHIE.Count>0)
                        {
                            foreach (HierarchyEntity ProgHie in ofilterSerProgHIE)
                            {
                                if (hierarchy.Agency == ProgHie.Agency && hierarchy.Dept == ProgHie.Dept && hierarchy.Prog==ProgHie.Prog)
                                {
                                    rowIndex = GridViewControl.Rows.Add("", "", ProgHie.Code, ProgHie.HirarchyName);
                                    GridViewControl.Rows[rowIndex].Tag = ProgHie;
                                    lstServices.Add(searchHeadCode);
                                    if (ProgHie.UsedFlag.Equals("Y"))
                                    {
                                        GridViewControl.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                                    }
                                }
                                    
                            }

                                
                        }

                        


            }
        }
                */
                #endregion

                if (ointakeHeadHierarchy.Count > 0)
                {
                    ointakeHeadHierarchy = ointakeHeadHierarchy.OrderBy(u => u.UsedFlag).ThenBy(u => u.Agency).ThenBy(u => u.Dept).ThenBy(u => u.Prog).ToList();

                    gvSerHie.Rows.Clear();
                    List<HierarchyEntity> oSPHierachies = new List<HierarchyEntity>();
                    if (oSPHierarchieslst.Count > 0)
                        oSPHierachies = (from s in oSPHierarchieslst orderby s.Agency, s.Dept, s.Prog select s).ToList();

                    List<string> lstServices = new List<string>();
                    int rowIndex = 0;

                    foreach (HierarchyEntity hierarchy in ointakeHeadHierarchy)
                    {
                        string Agy = (hierarchy.Agency == "" ? "**" : hierarchy.Agency);
                        string Dept = (hierarchy.Dept == "" ? "**" : hierarchy.Dept);
                        string Prog = (hierarchy.Prog == "" ? "**" : hierarchy.Prog);


                        string SearchIntakCode = Agy + "-" + Dept + "-" + Prog;
                        string hierarchyName = hierarchy.HirarchyName.ToString();

                        rowIndex = GridViewControl.Rows.Add((SearchIntakCode), hierarchyName, "", "");
                        GridViewControl.Rows[rowIndex].Tag = hierarchy;
                        lstServices.Add(SearchIntakCode);
                        if (hierarchy.UsedFlag.Equals("Y"))
                        {
                            GridViewControl.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                        }

                        List<HierarchyEntity> SPFilterHier = oSPHierachies.FindAll(x => (x.SerAgency == hierarchy.Agency) && (x.SerDept == hierarchy.Dept) && (x.SerProg == hierarchy.Prog));
                        if (SPFilterHier.Count > 0)
                        {
                            rowIndex = Captain.Common.Utilities.CommonFunctions.BuildSerHIEGrid(SPFilterHier, GridViewControl);
                        }


                        ////CASE 1 :: **-**-**    || If the intake hierarchy is **-**-**
                        //if (Agy == "**" && Dept == "**" && Prog == "**")
                        //{

                        //    if (oSPHierachies.Count > 0)
                        //    {
                        //        rowIndex = Captain.Common.Utilities.CommonFunctions.BuildHIEGrid(oSPHierachies, GridViewControl);
                        //    }
                        //}

                        ////CASE 2 :: AGY-DEPT-**    || If the intake hierarchy has Agency-Depratment-**
                        //if (Agy != "**" && Dept != "**" && Prog == "**")
                        //{
                        //    //Added by Sudheer on 04/14/23
                        //    List<HierarchyEntity> ofilterSerAgyDepHIE = new List<HierarchyEntity>();
                        //    if (BaseForm.BaseAgencyControlDetails.ServicePlanHiecontrol == "Y")
                        //    {
                        //        if (BaseForm.BaseAgencyControlDetails.SerPlanAllow == "D")
                        //            ofilterSerAgyDepHIE = oSPHierachies.FindAll(x => x.Agency == hierarchy.Agency && x.Dept == hierarchy.Dept);
                        //        else
                        //            ofilterSerAgyDepHIE = oSPHierachies.FindAll(x => x.Agency == hierarchy.Agency);
                        //    }
                        //    else
                        //        ofilterSerAgyDepHIE = oSPHierachies.FindAll(x => x.Agency == hierarchy.Agency && x.Dept == hierarchy.Dept);

                        //    if (ofilterSerAgyDepHIE.Count > 0)
                        //    {
                        //        rowIndex = Captain.Common.Utilities.CommonFunctions.BuildHIEGrid(ofilterSerAgyDepHIE, GridViewControl);
                        //    }
                        //}

                        ////CASE 3 :: AGY-**-**    || If the intake hierarchy has only Agency-**-**
                        //if (Agy != "**" && Dept == "**" && Prog == "**")
                        //{
                        //    List<HierarchyEntity> ofilterSerAgyHIE = oSPHierachies.FindAll(x => x.Agency == hierarchy.Agency);
                        //    if (ofilterSerAgyHIE.Count > 0)
                        //    {
                        //        rowIndex = Captain.Common.Utilities.CommonFunctions.BuildHIEGrid(ofilterSerAgyHIE, GridViewControl);
                        //    }
                        //}

                        ////CASE 4 :: AGY-DEPT-PROG    || If the intake hierarchy has Agency-Department-Program
                        //if (Agy != "**" && Dept != "**" && Prog != "**")
                        //{
                        //    //Added by Sudheer on 04/13/2023
                        //    List<HierarchyEntity> ofilterSerHIE = new List<HierarchyEntity>();
                        //    if(BaseForm.BaseAgencyControlDetails.ServicePlanHiecontrol=="Y")
                        //    {
                        //        if(BaseForm.BaseAgencyControlDetails.SerPlanAllow=="D")
                        //            ofilterSerHIE = oSPHierachies.FindAll(x => x.Agency == hierarchy.Agency && x.Dept == hierarchy.Dept );
                        //        else
                        //            ofilterSerHIE = oSPHierachies.FindAll(x => x.Agency == hierarchy.Agency);
                        //    }
                        //    else
                        //       ofilterSerHIE = oSPHierachies.FindAll(x => x.Agency == hierarchy.Agency && x.Dept == hierarchy.Dept && x.Prog == hierarchy.Prog);

                        //    if (ofilterSerHIE.Count > 0)
                        //    {
                        //        rowIndex = Captain.Common.Utilities.CommonFunctions.BuildHIEGrid(ofilterSerHIE, GridViewControl);
                        //    }
                        //}

                    }
                }

                #region sel hierarchies Kranthi Code
                //if (oSPHeadHierarchy.Count > 0)
                //{
                //    gvSerHie.Rows.Clear();
                //    /*************** Kranthi :: NEW CONCEPT on 02/02/2023 :: Keep in mind document *********************/
                //    List<string> lstServices = new List<string>();
                //    int rowIndex = 0;
                //    foreach (HierarchyEntity hierarchy in oSPHeadHierarchy)
                //    {
                //        string Agy = (hierarchy.Agency == "" ? "**" : hierarchy.Agency);
                //        string Dept = (hierarchy.Dept == "" ? "**" : hierarchy.Dept);
                //        string Prog = (hierarchy.Prog == "" ? "**" : hierarchy.Prog);


                //        string code = Agy + "-" + Dept + "-" + Prog;
                //        string hierarchyName = hierarchy.HirarchyName.ToString();

                //        string searchHeadCode = hierarchy.Agency + "-**-**";
                //        string searchCode = hierarchy.Agency + "-" + hierarchy.Dept + "-**";

                //        HierarchyEntity ofilterintkHeadHIE = ointakeHeadHierarchy.Find(x => (x.Agency == hierarchy.Agency && (x.Dept == "**" || x.Dept == "") && (x.Prog == "**" || x.Prog == "")));
                //        if (ofilterintkHeadHIE != null)
                //        {
                //            if (!lstServices.Contains(searchHeadCode))
                //            {
                //                rowIndex = GridViewControl.Rows.Add((searchHeadCode), ofilterintkHeadHIE.HirarchyName, "", "");
                //                GridViewControl.Rows[rowIndex].Tag = hierarchy;
                //                lstServices.Add(searchHeadCode);
                //                if (ofilterintkHeadHIE.UsedFlag.Equals("Y"))
                //                {
                //                    GridViewControl.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                //                }
                //            }

                //        }
                //        //  else
                //        // {
                //        HierarchyEntity ofilterintkHIE = ointakeHeadHierarchy.Find(x => (x.Agency == hierarchy.Agency && x.Dept == hierarchy.Dept && (x.Prog == "**" || x.Prog == "")));
                //        if (ofilterintkHIE != null)
                //        {
                //            if (!lstServices.Contains(searchCode))
                //            {
                //                rowIndex = GridViewControl.Rows.Add((searchCode), ofilterintkHIE.HirarchyName, "", "");
                //                GridViewControl.Rows[rowIndex].Tag = hierarchy;
                //                lstServices.Add(searchCode);
                //                if (ofilterintkHIE.UsedFlag.Equals("Y"))
                //                {
                //                    GridViewControl.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                //                }
                //            }
                //        }
                //        else
                //        {
                //            List<HierarchyEntity> ofilterintkHIE2 = ointakeHeadHierarchy.FindAll(x => (x.Agency + "-" + x.Dept + "-" + x.Prog) == code);
                //            if (ofilterintkHIE2.Count > 0)
                //            {
                //                if (!lstServices.Contains(code))
                //                {
                //                    rowIndex = GridViewControl.Rows.Add((ofilterintkHIE2[0].Agency + "-" + ofilterintkHIE2[0].Dept + "-" + ofilterintkHIE2[0].Prog), ofilterintkHIE2[0].HirarchyName, "", "");
                //                    GridViewControl.Rows[rowIndex].Tag = hierarchy;
                //                    lstServices.Add(code);
                //                    if (ofilterintkHIE2[0].UsedFlag.Equals("Y"))
                //                    {
                //                        GridViewControl.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                //                    }
                //                }
                //            }
                //            else
                //            {
                //                HierarchyEntity ostarfilterintkHIE = ointakeHeadHierarchy.Find(x => (x.Agency == "**" && x.Dept == "**" && x.Prog == "**"));
                //                if (ostarfilterintkHIE != null)
                //                {
                //                    string strSearchstarcCode = "**-**-**";
                //                    if (!lstServices.Contains(strSearchstarcCode))
                //                    {
                //                        rowIndex = GridViewControl.Rows.Add((strSearchstarcCode), ostarfilterintkHIE.HirarchyName, "", "");
                //                        GridViewControl.Rows[rowIndex].Tag = hierarchy;
                //                        lstServices.Add(strSearchstarcCode);
                //                        if (ostarfilterintkHIE.UsedFlag.Equals("Y"))
                //                        {
                //                            GridViewControl.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //        //}

                //        rowIndex = GridViewControl.Rows.Add("", "", code, hierarchyName);
                //        GridViewControl.Rows[rowIndex].Tag = hierarchy;
                //        if (hierarchy.UsedFlag.Equals("Y"))
                //        {
                //            GridViewControl.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                //        }
                //    }
                //    /************************************************************************/
                //}
                #endregion
            }
        }
    }


}
