#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using Wisej.Web;
using Wisej.Design;
using Captain.Common.Utilities;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Menus;
using Captain.Common.Views.Forms;
using System.Data.SqlClient;
using Captain.Common.Views.Controls;
using Captain.Common.Model.Objects;
using Captain.Common.Model.Data;
using System.Text.RegularExpressions;
using Captain.Common.Views.UserControls;
using Captain.Common.Views.Controls.Compatibility;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class ADMN0015Form : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        //private bool boolChangeStatus = false;

        public int strIndex = 0;
        public int strCrIndex = 0;
        public int strPageIndex = 1;

        #endregion

        public ADMN0015Form(BaseForm baseform, string mode, string Site_Type, string agency, string dept, string prog, string year, string code, string hier_Desc, string hierar, PrivilegeEntity privileges)
        {
            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            _model = new CaptainModel();

            BaseForm = baseform;
            Mode = mode;
            Agency = agency;
            Dept = dept; Prog = prog;
            Year = year; Code = code;
            strHiearchy = hierar; Type = Site_Type;
            strHierarcy_Desc = hier_Desc;
            Privileges = privileges;

            chkHomebaseSite.Visible = false;
            hierarchyEntity = _model.lookupDataAccess.GetHierarchyByUserID(null, "I", string.Empty);
            //this.Text = Privileges.Program + " - " + Mode;
            this.Text = Type + " Maintenance" + " - " + Mode;
            //LblHeader.Text = Type + " Maintenance";
            if (string.IsNullOrEmpty(Year) || string.IsNullOrWhiteSpace(Year))
                Year = "    ";
            if (Type == "Site")
            {
                if (!string.IsNullOrEmpty(Year.Trim()))
                {
                    pnlSiteTxt.Visible = pnlSiteNameTxt.Visible = false;
                    pnlSiteNameCmbx.Visible = true;

                    pnlSiteDetails.Size = new Size(this.Width,185);
                    pnlSite.Size = new Size(this.Width, 287);
                    this.Size = new System.Drawing.Size(730, 344);//(727, 387);

                    fillSiteNameCombo();
                }
                else
                {
                    pnlSiteTxt.Visible = pnlSiteNameTxt.Visible = true;
                    pnlSiteNameCmbx.Visible = false;

                    pnlSiteDetails.Size = new Size(this.Width, 216);
                    pnlSite.Size = new Size(this.Width, 320);
                    this.Size = new System.Drawing.Size(730, 377);
                }


                pnlSite.Visible = true;   /*panel1.Visible = panel2.Visible = panel3.Visible = panel4.Visible = */
                room_Panel1.Visible = room_Panel2.Visible = room_Panel3.Visible = false;
                room_Panel7.Visible = false;

                txtSite.Focus();

                if (Mode == "Edit")
                {
                    txtSite.Enabled = false;
                    txtHierarchy.Enabled = false;
                    txtHierachydesc.Enabled = false;
                    PbHierarchies.Visible = false;
                    FillControls();
                    pnlSiteNameCmbx.Enabled = false;
                }
                else if (Mode == "View")
                {
                    txtSite.Enabled = false;
                    txtHierarchy.Enabled = false;
                    txtHierachydesc.Enabled = false;
                    PbHierarchies.Visible = false;
                    FillControls();
                    EnableDisableSite();
                }

                txtHierarchy.Text = strHiearchy;
                txtHierachydesc.Text = strHierarcy_Desc.Trim();
                if (Year == "    ")
                {
                    if (!string.IsNullOrEmpty(strHiearchy))
                        txtHierarchy.Text = strHiearchy.Substring(0, 2).Trim();
                    lblYear.Visible = false;
                    lblTxtYear.Visible = false;
                }
                else
                {
                    lblYear.Visible = true; lblTxtYear.Visible = true;
                    lblTxtYear.Text = Year;
                }
                txtZip.Validator = TextBoxValidation.IntegerValidator;
                txtZipPlus.Validator = TextBoxValidation.IntegerValidator;
                txtSite_Slots.Validator = TextBoxValidation.IntegerValidator;
                txtRentCost.Validator = TextBoxValidation.FloatValidator;
                txtUtilitycost.Validator = TextBoxValidation.FloatValidator;
            }
            else
            {
             

                pnlSite.Visible = false;  //panel1.Visible = panel2.Visible = panel3.Visible = panel4.Visible = false;
                CompleteRoom_panel.Visible = true;   //room_Panel1.Visible = room_Panel2.Visible = room_Panel3.Visible = true = room_Panel7.Visible = true; 

                this.Size = new System.Drawing.Size(772, 566);

                chkHomebaseSite.Visible = true;
                //chkHomebaseSite.Location = new System.Drawing.Point(387, 12);

                //Size = new Size(702, 566);
                //this.room_Panel1.Location = new System.Drawing.Point(0, 0);
                //this.room_Panel2.Location = new System.Drawing.Point(0,0);
                //this.room_Panel3.Location = new System.Drawing.Point(366, 0);
                //this.room_Panel7.Location = new System.Drawing.Point(0,820);


                //LblHeader.Text = "Room Maintenance";
                txtRoomSite.Text = Code;
                fillDropdowns();
                fillgvMeal();
                FillEntity();
                if (Mode == "Edit")
                {
                    txtRoom.Enabled = false;
                    cmbAmPm.Enabled = false;
                    fillRoomControls();
                    fillEditGvMeal();
                    txtRoom.Enabled = false;
                }
                else if (Mode == "View")
                {
                    txtRoom.Enabled = false;
                    cmbAmPm.Enabled = false;
                    fillRoomControls();
                    fillEditGvMeal();
                    txtRoom.Enabled = false;
                    EnabledisableRoom();
                }
                else
                {
                    maskRoom_PhnNo.Text = Sitelist[0].SitePHONE.Replace("-", "");
                    maskRoom_Fax.Text = Sitelist[0].SiteFAX.Replace("-", "");
                    maskRoom_OthPhn.Text = Sitelist[0].SiteOTHER_PHONE.Replace("-", "");
                }
                txtSlots.Validator = TextBoxValidation.IntegerValidator;
                txtF1Slots.Validator = TextBoxValidation.IntegerValidator;
                txtF2Slots.Validator = TextBoxValidation.IntegerValidator;
                txtF3Slots.Validator = TextBoxValidation.IntegerValidator;
                txtF4Slots.Validator = TextBoxValidation.IntegerValidator;
                txtF5Slots.Validator = TextBoxValidation.IntegerValidator;
            }

        }

        //Added by Vikash on 08/16/2024 as per 2024 Enhancement Doucument
        private void fillSiteNameCombo()
        {
            cmbSiteName.Items.Clear();

            CaseSiteEntity Search_Entity = new CaseSiteEntity(true);
            Site_List = _model.CaseMstData.Browse_CASESITE(Search_Entity, "Browse");

            foreach (CaseSiteEntity Entity in Site_List)
            {
                cmbSiteName.Items.Add(new ListItem(Entity.SiteNAME, Entity.SiteNUMBER));
            }

            if (cmbSiteName.Items.Count > 0)
                cmbSiteName.SelectedIndex = 0;
        }

        #region properties

        public BaseForm BaseForm { get; set; }

        public string Mode { get; set; }

        public string Code { get; set; }

        public string Agency { get; set; }

        public string Dept { get; set; }

        public string Prog { get; set; }

        public string Year { get; set; }

        public string Type { get; set; }

        public string strHierarcy_Desc { get; set; }

        public string HiearchyCode { get; set; }

        public string strHiearchy { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public List<HierarchyEntity> hierarchyEntity { get; set; }

        public bool IsSaveValid { get; set; }

        #endregion

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "ADMN0015");
        }


        List<CaseSiteEntity> Sitelist = new List<CaseSiteEntity>();
        private void FillEntity()
        {
            CaseSiteEntity Search_Entity = new CaseSiteEntity(true);
            Search_Entity.SiteAGENCY = Agency; Search_Entity.SiteDEPT = Dept;
            Search_Entity.SitePROG = Prog; Search_Entity.SiteYEAR = Year;
            Search_Entity.SiteNUMBER = Code; Search_Entity.SiteROOM = "0000";
            Sitelist = _model.CaseMstData.Browse_CASESITE(Search_Entity, "Browse");
        }

        private void fillDropdowns()
        {
            cmbAmPm.Items.Clear();
            List<ListItem> ampmlist = new List<ListItem>();
            ampmlist.Add(new ListItem("A - AM Class", "A"));
            ampmlist.Add(new ListItem("P - PM Class", "P"));
            ampmlist.Add(new ListItem("E - Extended Day", "E"));
            ampmlist.Add(new ListItem("F - Full Day", "F"));
            cmbAmPm.Items.AddRange(ampmlist.ToArray());
            cmbAmPm.SelectedIndex = 0;


            DataSet dsLang = DatabaseLayer.Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.LANGUAGECODES);
            DataTable dtLang = dsLang.Tables[0];
            List<ListItem> listlang = new List<ListItem>();
            cmbLang.Items.Insert(0, new ListItem("---None---", "NA"));
            foreach (DataRow drLang in dtLang.Rows)
            {
                cmbLang.Items.Add(new ListItem(drLang["LookUpDesc"].ToString().Trim(), drLang["Code"].ToString().Trim()));
            }
            cmbLang.SelectedIndex = 0;

            DataSet dsFund = DatabaseLayer.Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.CASEMNGMTFUNDSRC, "A");
            DataTable dtFund = dsFund.Tables[0];
            List<ListItem> listFund = new List<ListItem>();
            cmbFund1.Items.Insert(0, new ListItem("None", "None"));
            cmbFund1.Items.Insert(1, new ListItem("All", "All"));

            cmbFund2.Items.Insert(0, new ListItem("None", "None"));
            cmbFund2.Items.Insert(1, new ListItem("All", "All"));
            cmbFund3.Items.Insert(0, new ListItem("None", "None"));
            cmbFund3.Items.Insert(1, new ListItem("All", "All"));
            cmbFund4.Items.Insert(0, new ListItem("None", "None"));
            cmbFund4.Items.Insert(1, new ListItem("All", "All"));
            cmbFund5.Items.Insert(0, new ListItem("None", "None"));
            cmbFund5.Items.Insert(1, new ListItem("All", "All"));
            foreach (DataRow drFund in dtFund.Rows)
            {
                cmbFund1.Items.Add(new ListItem(drFund["LookUpDesc"].ToString().Trim(), drFund["Code"].ToString().Trim()));
                cmbFund2.Items.Add(new ListItem(drFund["LookUpDesc"].ToString().Trim(), drFund["Code"].ToString().Trim()));
                cmbFund3.Items.Add(new ListItem(drFund["LookUpDesc"].ToString().Trim(), drFund["Code"].ToString().Trim()));
                cmbFund4.Items.Add(new ListItem(drFund["LookUpDesc"].ToString().Trim(), drFund["Code"].ToString().Trim()));
                cmbFund5.Items.Add(new ListItem(drFund["LookUpDesc"].ToString().Trim(), drFund["Code"].ToString().Trim()));
            }
            cmbFund1.SelectedIndex = 0; cmbFund2.SelectedIndex = 0; cmbFund3.SelectedIndex = 0; cmbFund4.SelectedIndex = 0;
            cmbFund5.SelectedIndex = 0;


            STAFFMSTEntity Search_STAFFMST = new STAFFMSTEntity(true);
            Search_STAFFMST.Agency = BaseForm.BaseAgency;
            Search_STAFFMST.Year= Year;
            List<STAFFMSTEntity> propSTAFFMST_List = _model.STAFFData.Browse_STAFFMST(Search_STAFFMST, "Browse");
            cmbHeadTeacher.ColorMember = "FavoriteColor";
            cmbAss1.ColorMember = "FavoriteColor";
            cmbAss2.ColorMember = "FavoriteColor";
            if (propSTAFFMST_List.Count > 0)
            {
                propSTAFFMST_List = propSTAFFMST_List.OrderBy(u => u.Active.Trim()).ThenBy(u => u.First_Name.Trim()).ThenBy(u => u.Last_Name.Trim()).ToList();
                foreach (STAFFMSTEntity staffdetails in propSTAFFMST_List)
                {
                    ListItem li = new ListItem(staffdetails.First_Name.Trim() + " " + staffdetails.Last_Name.Trim(), staffdetails.Staff_Code.Trim(), staffdetails.Active, staffdetails.Active.Equals("A") ? Color.Black : Color.Red);
                    cmbHeadTeacher.Items.Add(li);
                    cmbAss1.Items.Add(li);
                    cmbAss2.Items.Add(li);
                    //cmbHeadTeacher.Items.Add(new ListItem(staffdetails.First_Name.Trim() + " " + staffdetails.Last_Name.Trim(), staffdetails.Staff_Code.Trim()));
                    //cmbAss1.Items.Add(new ListItem(staffdetails.First_Name.Trim() + " " + staffdetails.Last_Name.Trim(), staffdetails.Staff_Code.Trim()));
                    //cmbAss2.Items.Add(new ListItem(staffdetails.First_Name.Trim() + " " + staffdetails.Last_Name.Trim(), staffdetails.Staff_Code.Trim()));
                }
            }

            //DataSet ds = Captain.DatabaseLayer.Lookups.GetStaff();
            //DataTable dt = ds.Tables[0];
            //DataView dv = new DataView(dt);
            //dv.RowFilter = "STF_AGENCY=" + Agency.Trim();
            ////dv.Sort = "STF_CODE";
            //dt = dv.ToTable();
            //string Priv_Name = string.Empty;
            //foreach (DataRow dr in dt.Rows)
            //{
            //    if (dr["STF_CODE"].ToString().Trim() != Priv_Name.Trim())
            //    {
            //        cmbHeadTeacher.Items.Add(new ListItem(dr["STF_NAME"].ToString().Replace(",",""), dr["STF_CODE"].ToString()));
            //        cmbAss1.Items.Add(new ListItem(dr["STF_NAME"].ToString(), dr["STF_CODE"].ToString()));//dr["STF_Agency"].ToString() + " - " + 
            //        cmbAss2.Items.Add(new ListItem(dr["STF_NAME"].ToString(), dr["STF_CODE"].ToString()));

            //        Priv_Name = dr["STF_CODE"].ToString().Trim();
            //    }
            //}
            cmbHeadTeacher.Items.Insert(0, new ListItem("None", "0"));
            cmbHeadTeacher.SelectedIndex = 0;
            cmbAss1.Items.Insert(0, new ListItem("None", "0"));
            cmbAss1.SelectedIndex = 0;
            cmbAss2.Items.Insert(0, new ListItem("None", "0"));
            cmbAss2.SelectedIndex = 0;

        }
        //string Img_Cross = new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("cross.ico");
        //string Img_Tick = new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("tick.ico");

        string Img_Tick = "icon-gridtick"; // "Resources/Images/tick.ico";
        string Img_Cross = "icon-close?color=Red"; // "Resources/Images/cross.ico";

        private void fillgvMeal()
        {
            int rowIndex = 0;
            //string[] Meals = new string[5];
            string[] Meals = { "Breakfast", "AM Snack", "Lunch", "PM Snack", "Supper" };
            for (int i = 0; i < Meals.Length; i++)
            {
                rowIndex = gvMeal.Rows.Add(Meals[i], Img_Cross, Img_Cross, Img_Cross, Img_Cross, Img_Cross, Img_Cross, Img_Cross, "N", "N", "N", "N", "N", "N", "N");
            }
        }
        private void btnZipSearch_Click(object sender, EventArgs e)
        {
            ZipCodeSearchForm zipCodeSearchForm = new ZipCodeSearchForm(Privileges, txtZip.Text);
            zipCodeSearchForm.FormClosed += new FormClosedEventHandler(OnZipCodeFormClosed);
            zipCodeSearchForm.StartPosition = FormStartPosition.CenterScreen;
            zipCodeSearchForm.ShowDialog();
        }

        private void OnZipCodeFormClosed(object sender, FormClosedEventArgs e)
        {
            btnZipSearch.Enabled = true;
            //btnCitySearch.Enabled = true;
            ZipCodeSearchForm form = sender as ZipCodeSearchForm;
            if (form.DialogResult == DialogResult.OK)
            {
                ZipCodeEntity zipcodedetais = form.GetSelectedZipCodedetails();
                if (zipcodedetais != null)
                {
                    string zipPlus = zipcodedetais.Zcrplus4;
                    txtZipPlus.Text = "0000".Substring(0, 4 - zipPlus.Length) + zipPlus;
                    txtZip.Text = "00000".Substring(0, 5 - zipcodedetais.Zcrzip.Length) + zipcodedetais.Zcrzip;
                    txtState.Text = zipcodedetais.Zcrstate;
                    txtCity.Text = zipcodedetais.Zcrcity;
                    // SetComboBoxValue(cmbCounty, zipcodedetais.Zcrcountry);
                    //SetComboBoxValue(cmbTownship, zipcodedetais.Zcrcitycode);

                }
            }
            // btnCitySearch.Focus();
        }

        private void txtZip_Leave(object sender, EventArgs e)
        {

            if (txtZip.Text.Length == 5)
            {
                List<ZipCodeEntity> zipcodeEntity = _model.ZipCodeAndAgency.GetZipCodeSearch(txtZip.Text, string.Empty, string.Empty, string.Empty);
                if (zipcodeEntity.Count > 0)
                    zipcodeEntity = zipcodeEntity.FindAll(u => u.InActive.Equals("N") || u.InActive.Trim().Equals(""));

                if (zipcodeEntity.Count > 0)
                {
                    if (zipcodeEntity.Count == 1)
                    {
                        btnZipSearch.Enabled = true;
                    }
                    foreach (ZipCodeEntity zipcodedetais in zipcodeEntity)
                    {
                        if (zipcodedetais != null)
                        {
                            string zipPlus = zipcodedetais.Zcrplus4;
                            txtZipPlus.Text = "0000".Substring(0, 4 - zipPlus.Length) + zipPlus;
                            txtZip.Text = "00000".Substring(0, 5 - zipcodedetais.Zcrzip.Length) + zipcodedetais.Zcrzip;
                            txtState.Text = zipcodedetais.Zcrstate;
                            txtCity.Text = zipcodedetais.Zcrcity;
                            //SetComboBoxValue(cmbCounty, zipcodedetais.Zcrcountry);
                            //SetComboBoxValue(cmbTownship, zipcodedetais.Zcrcitycode);
                        }

                    }
                }
                else
                {
                    string Zip = txtZip.Text.Trim();
                    txtZip.Text = "00000".Substring(0, 5 - Zip.Length) + Zip.Trim();
                    //ZipCodeSearchForm zipCodeSearchForm = new ZipCodeSearchForm(Privileges, txtZip.Text);
                    //zipCodeSearchForm.FormClosed += new Form.FormClosedEventHandler(OnZipCodeFormClosed);
                    //zipCodeSearchForm.ShowDialog();
                }
            }
            else
            {
                string Zip = txtZip.Text.Trim();
                txtZip.Text = "00000".Substring(0, 5 - Zip.Length) + Zip.Trim();
                //ZipCodeSearchForm zipCodeSearchForm = new ZipCodeSearchForm(Privileges, txtZip.Text);
                //zipCodeSearchForm.FormClosed += new Form.FormClosedEventHandler(OnZipCodeFormClosed);
                //zipCodeSearchForm.ShowDialog();
            }
        }

        private void txtZipPlus_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtZipPlus.Text) || (!string.IsNullOrWhiteSpace(txtZipPlus.Text)))
            {
                string ZipPlus = txtZipPlus.Text.Trim();
                txtZipPlus.Text = "0000".Substring(0, 4 - ZipPlus.Length) + ZipPlus.Trim();
            }
            else
            {
                if (!string.IsNullOrEmpty(txtZip.Text) || (!string.IsNullOrWhiteSpace(txtZip.Text)))
                    txtZipPlus.Text = "0000";
            }
        }
        List<CaseSiteEntity> Site_List;
        private void FillControls()
        {
            CaseSiteEntity Search_Entity = new CaseSiteEntity(true);
            Search_Entity.SiteAGENCY = Agency; Search_Entity.SiteDEPT = Dept;
            Search_Entity.SitePROG = Prog; Search_Entity.SiteYEAR = Year;
            Search_Entity.SiteNUMBER = Code.Trim();
            Site_List = _model.CaseMstData.Browse_CASESITE(Search_Entity, "Browse");

            CaseSiteEntity Entity = Site_List[0];
            if (Entity.SiteACTIVE.Equals("Y")) chkbActive.Checked = true;
            else chkbActive.Checked = false;
            txtSite.Text = Entity.SiteNUMBER.Trim();
            txtSiteName.Text = Entity.SiteNAME.Trim();

            SetComboBoxValue(cmbSiteName, Entity.SiteNUMBER);

            txtStreet.Text = Entity.SiteSTREET.Trim();
            txtCity.Text = Entity.SiteCITY.Trim();
            txtState.Text = Entity.SiteSTATE.Trim();
            txtZip.Text = "00000".Substring(0, 5 - Entity.SiteZIP.Length) + Entity.SiteZIP.Trim();
            txtZipPlus.Text = "0000".Substring(0, 4 - Entity.SiteZIP_PLUS.Length) + Entity.SiteZIP_PLUS.Trim();
            txtCd.Text = Entity.SiteC_DIRECTOR.Trim();
            maskPhone.Text = Entity.SitePHONE.Trim().Replace("-", "");
            maskFax.Text = Entity.SiteFAX.Trim().Replace("-", "");
            maskOther.Text = Entity.SiteOTHER_PHONE.Trim().Replace("-", "");
            txtProject.Text = Entity.SitePROJECT.Trim();
            txtRentCost.Text = Entity.SiteRENTAL_FEE.Trim();
            txtUtilitycost.Text = Entity.SiteUTILITY_FEE.Trim();
            txtSite_Slots.Text = Entity.SITE_FUNDED_SLOTS.Trim();
            if (!string.IsNullOrEmpty(Entity.SiteOPEN_DATE.Trim()))
            {
                dtpSiteOpen.Text = CommonFunctions.ChangeDateFormat(Entity.SiteOPEN_DATE, Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat); //Convert.ToDateTime(LookupDataAccess.Getdate(Entity.SiteOPEN_DATE.Trim()));
                dtpSiteOpen.Checked = true;
            }
            if (!string.IsNullOrEmpty(Entity.SiteCLOSE_DATE.Trim()))
            {
                dtpSiteClose.Text = CommonFunctions.ChangeDateFormat(Entity.SiteCLOSE_DATE, Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat); //Convert.ToDateTime(LookupDataAccess.Getdate(Entity.SiteCLOSE_DATE.Trim()));
                dtpSiteClose.Checked = true;
            }
            if (Entity.SiteOUTSIDE_PLAY.Equals("Y")) chkbPlayArea.Checked = true;
            else chkbPlayArea.Checked = false;

        }

        private void EnableDisableSite()
        {
            chkbActive.Enabled = false;
            txtSite.Enabled = false;
            txtSiteName.Enabled = false;
            pnlSiteNameCmbx.Enabled = false;
            txtStreet.Enabled = false;
            txtCity.Enabled = false;
            txtState.Enabled = false;
            txtZip.Enabled = false;
            txtZipPlus.Enabled = false;
            txtCd.Enabled = false;
            maskPhone.Enabled = false;
            maskFax.Enabled = false;
            maskOther.Enabled = false;
            txtProject.Enabled = false;
            txtRentCost.Enabled = false;
            txtUtilitycost.Enabled = false;
            txtSite_Slots.Enabled = false;
            dtpSiteOpen.Enabled = false;
            dtpSiteClose.Enabled = false;
            chkbPlayArea.Enabled = false;
            btnSave.Visible = false; btnCancel.Visible = false;
            btnZipSearch.Visible = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateForm())
                {
                    CaseSiteEntity Entity = new CaseSiteEntity();
                    string SqlMsg = string.Empty;
                    string msg = string.Empty;
                    CaptainModel Model = new CaptainModel();
                    if (Mode == "Edit")
                        Entity.Row_Type = "U";
                    else
                        Entity.Row_Type = "I";
                    string Hierarchy_Code = txtHierarchy.Text.Trim();
                    if (Year == "    ")
                    {
                        Agency = Hierarchy_Code.Trim();
                        Dept = "  ";
                        Prog = "  ";
                    }
                    else
                    {
                        Agency = Hierarchy_Code.Substring(0, 2).Trim();
                        if (Hierarchy_Code.Substring(3, 2).Trim() == "**")
                            Dept = "  ";
                        else Dept = Hierarchy_Code.Substring(3, 2).Trim();
                        if (Hierarchy_Code.Substring(6, 2).Trim() == "**")
                            Prog = "  ";
                        else Prog = Hierarchy_Code.Substring(6, 2).Trim();
                    }
                    Entity.SiteAGENCY = Agency;
                    Entity.SiteDEPT = Dept;
                    Entity.SitePROG = Prog;

                    Entity.SiteYEAR = Year;
                    Entity.SiteROOM = "0000";
                    Entity.SiteAM_PM = " ";
                    Entity.SiteNUMBER = txtSite.Text.Trim();
                    Entity.SiteTRAN_AREA = txtSite.Text.Trim();
                    Entity.SiteNAME = txtSiteName.Text.Trim();
                    Entity.SiteSTREET = txtStreet.Text.Trim();
                    Entity.SiteCITY = txtCity.Text.Trim();
                    Entity.SiteSTATE = txtState.Text.Trim();
                    Entity.SiteZIP = txtZip.Text.Trim();
                    Entity.SiteZIP_PLUS = txtZipPlus.Text.Trim();
                    Entity.SiteC_DIRECTOR = txtCd.Text.Trim();
                    Entity.SitePHONE = maskPhone.Text.Trim().Replace("-", "");
                    Entity.SiteFAX = maskFax.Text.Trim().Replace("-", "");
                    Entity.SiteOTHER_PHONE = maskOther.Text.Trim().Replace("-", "");
                    Entity.SitePROJECT = txtProject.Text.Trim();
                    Entity.SiteACTIVE = chkbActive.Checked ? "Y" : "N";
                    if (!string.IsNullOrEmpty(txtRentCost.Text.Trim()))
                        Entity.SiteRENTAL_FEE = decimal.Parse(txtRentCost.Text.Trim()).ToString();
                    else Entity.SiteRENTAL_FEE = txtRentCost.Text.Trim();
                    if (!string.IsNullOrEmpty(txtUtilitycost.Text.Trim()))
                        Entity.SiteUTILITY_FEE = decimal.Parse(txtUtilitycost.Text.Trim()).ToString();
                    else Entity.SiteUTILITY_FEE = txtUtilitycost.Text.Trim();
                    Entity.SiteOUTSIDE_PLAY = chkbPlayArea.Checked ? "Y" : "N";
                    if (dtpSiteOpen.Checked.Equals(true))
                        Entity.SiteOPEN_DATE = dtpSiteOpen.Text.Trim();
                    if (dtpSiteClose.Checked.Equals(true))
                        Entity.SiteCLOSE_DATE = dtpSiteClose.Text.Trim();
                    Entity.SITE_FUNDED_SLOTS = txtSite_Slots.Text.Trim();
                    Entity.SiteADD_OPERATOR = BaseForm.UserID;
                    Entity.SiteLSTC_OPERATOR = BaseForm.UserID;

                    _model.CaseMstData.UpdateCASESITE(Entity, "Update", out msg, out SqlMsg);

                    this.DialogResult = DialogResult.OK;
                    //AlertBox.Show("Saved Successfully", MessageBoxIcon.Information, null, ContentAlignment.BottomRight);
                    this.Close();

                }

            }
            catch (Exception ex)
            {

            }
        }


        private bool ValidateForm()
        {
            bool isValid = true;

            if (string.IsNullOrEmpty(txtHierarchy.Text.Trim()))
            {
                _errorProvider.SetError(PbHierarchies, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblHierarchy.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtHierachydesc, null);
            }

            if (!string.IsNullOrEmpty(Year.Trim()))
            {
                if (isCodeExists(((Captain.Common.Utilities.ListItem)cmbSiteName.SelectedItem).Value.ToString().Trim()))
                {
                    _errorProvider.SetError(cmbSiteName, "Site already on file".Replace(Consts.Common.Colon, string.Empty));
                    isValid = false;
                }
                else
                    _errorProvider.SetError(cmbSiteName, null);
            }
            else
            {
                if (string.IsNullOrEmpty(txtSite.Text) || string.IsNullOrWhiteSpace(txtSite.Text))
                {
                    _errorProvider.SetError(txtSite, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblSite.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    if (isCodeExists(txtSite.Text))
                    {
                        _errorProvider.SetError(txtSite, "Site already on file".Replace(Consts.Common.Colon, string.Empty));
                        //_errorProvider.SetError(txtSite, string.Format(Consts.Messages.AlreadyExists.GetMessage(), lblSite.Text.Replace(Consts.Common.Colon, string.Empty)));
                        isValid = false;
                    }
                    else
                        _errorProvider.SetError(txtSite, null);
                }


                if (string.IsNullOrEmpty(txtSiteName.Text) || string.IsNullOrWhiteSpace(txtSiteName.Text))
                {
                    _errorProvider.SetError(txtSiteName, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblSiteName.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                    _errorProvider.SetError(txtSiteName, null);
            }

            if (string.IsNullOrEmpty(txtStreet.Text) || string.IsNullOrWhiteSpace(txtStreet.Text))
            {
                _errorProvider.SetError(txtStreet, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblStreet.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(txtStreet, null);

            if (string.IsNullOrEmpty(txtCity.Text) || string.IsNullOrWhiteSpace(txtCity.Text))
            {
                _errorProvider.SetError(txtCity, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblCity.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(txtCity, null);

            if (string.IsNullOrEmpty(txtState.Text) || string.IsNullOrWhiteSpace(txtState.Text))
            {
                _errorProvider.SetError(txtState, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblState.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(txtState, null);

            if (dtpSiteOpen.Checked.Equals(false) && dtpSiteClose.Checked.Equals(true))
            {
                _errorProvider.SetError(dtpSiteOpen, "Please select Site Open Date");
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtpSiteOpen, null);
            }

            //if (dtpSiteOpen.Checked.Equals(true) && dtpSiteClose.Checked.Equals(false))
            //{
            //    _errorProvider.SetError(dtpSiteClose, "Please select Site Close Date");
            //    isValid = false;
            //}
            //else
            //{
            //    _errorProvider.SetError(dtpSiteClose, null);
            //}

            if (dtpSiteOpen.Checked.Equals(true) && dtpSiteClose.Checked.Equals(true))
            {
                if (string.IsNullOrWhiteSpace(dtpSiteOpen.Text))
                {
                    _errorProvider.SetError(dtpSiteOpen, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Site Open Date".Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtpSiteOpen, null);
                }
                if (string.IsNullOrWhiteSpace(dtpSiteClose.Text))
                {
                    _errorProvider.SetError(dtpSiteClose, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Site Close Date".Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtpSiteClose, null);
                }
            }

            if (dtpSiteOpen.Checked.Equals(true) && dtpSiteClose.Checked.Equals(true))
            {
                if (Convert.ToDateTime(dtpSiteOpen.Text/*.ToShortDateString()*/) > Convert.ToDateTime(dtpSiteClose.Text/*.ToShortDateString()*/))
                {
                    _errorProvider.SetError(dtpSiteClose, "Invalid Site Close Date.It should be greater than start date ".Replace(Consts.Common.Colon, string.Empty));
                    isValid = false;
                }
                else 
                {
                    _errorProvider.SetError(dtpSiteClose, null);
                }
            }

            if (txtRentCost.Text.Length > 6)
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtRentCost.Text, Consts.StaticVars.TwoDecimalRange6String))
                {
                    _errorProvider.SetError(txtRentCost, Consts.Messages.PleaseEnterDecimals6Range);
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtRentCost, null);
                }
            }

            if (txtUtilitycost.Text.Length > 6)
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtUtilitycost.Text, Consts.StaticVars.TwoDecimalRange6String))
                {
                    _errorProvider.SetError(txtUtilitycost, Consts.Messages.PleaseEnterDecimals6Range);
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtUtilitycost, null);
                }
            }


            //if (string.IsNullOrEmpty(maskPhone.Text.Trim()))
            //{
            //    _errorProvider.SetError(maskPhone, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), maskPhone.Text));
            //    isValid = false;
            //}
            //else
            //{
            //    string strhome = maskPhone.Text.Replace(" ", string.Empty);
            //    if (strhome.Length < 10 && strhome.Length > 0)
            //    {
            //        _errorProvider.SetError(maskPhone, "Please enter Cell in correct format");
            //        isValid = false;

            //    }
            //    else
            //    {
            //        _errorProvider.SetError(maskPhone, null);
            //    }

            //}

            if (maskPhone.Text != "" && maskPhone.Text != "   -   -")
            {
                if (maskPhone.Text.Trim().Replace("-", "").Replace(" ", "").Length < 10)
                {
                    _errorProvider.SetError(maskPhone, "Please enter valid Phone Number");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(maskPhone, null);
                }
            }
            if (maskFax.Text != "" && maskFax.Text != "   -   -")
            {
                if (maskFax.Text.Trim().Replace("-", "").Replace(" ", "").Length < 10)
                {
                    _errorProvider.SetError(maskFax, "Please enter valid Fax Number");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(maskFax, null);
                }
            }
            if (maskOther.Text != "" && maskOther.Text != "   -   -")
            {
                if (maskOther.Text.Trim().Replace("-", "").Replace(" ", "").Length < 10)
                {
                    _errorProvider.SetError(maskOther, "Please enter valid Other Number");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(maskOther, null);
                }
            }

            IsSaveValid = isValid;
            return (isValid);
        }

        public string[] GetSelected_Site_Code()
        {
            string[] Added_Edited_SiteCode = new string[3];

            Added_Edited_SiteCode[0] = txtSite.Text;
            Added_Edited_SiteCode[1] = Mode;
            //Added_Edited_SiteCode[2] = Agency + '-' + "**" + '-' + "**";
            Added_Edited_SiteCode[2] = txtHierarchy.Text;
            //Added_Edited_SiteCode[2] = ;

            return Added_Edited_SiteCode;
        }

        public string[] GetSelected_Room_Code()
        {
            string[] Added_Edited_RoomCode = new string[4];
            Added_Edited_RoomCode[0] = txtRoom.Text;
            Added_Edited_RoomCode[1] = Mode;
            Added_Edited_RoomCode[2] = txtRoomSite.Text;
            Added_Edited_RoomCode[3] = Agency + '-' + Dept + '-' + Prog;

            return Added_Edited_RoomCode;
        }

        private bool isCodeExists(string TempCode)
        {
            bool isExists = false;
            if (Type == "Site")
            {
                if (Mode.Equals("Add"))
                {
                    CaseSiteEntity Search_Entity = new CaseSiteEntity(true);
                    string Hierarchy_Code = txtHierarchy.Text.Trim();
                    if (Year == "    ")
                    {
                        Agency = Hierarchy_Code.Trim();
                        Dept = "  ";
                        Prog = "  ";
                    }
                    else
                    {
                        Agency = Hierarchy_Code.Substring(0, 2).Trim();
                        if (Hierarchy_Code.Substring(3, 2).Trim() == "**")
                            Dept = "  ";
                        if (Hierarchy_Code.Substring(6, 2).Trim() == "**")
                            Prog = "  ";
                    }
                    Search_Entity.SiteAGENCY = Agency; Search_Entity.SiteDEPT = Dept;
                    Search_Entity.SitePROG = Prog; Search_Entity.SiteYEAR = Year;
                    Search_Entity.SiteNUMBER = TempCode.Trim();
                    Site_List = _model.CaseMstData.Browse_CASESITE(Search_Entity, "Browse");
                    foreach (CaseSiteEntity Entity in Site_List)
                    {
                        if (Entity.SiteNUMBER.Trim() == TempCode.Trim())
                        {
                            isExists = true;
                        }
                    }
                }
            }
            else
            {
                if (Mode.Equals("Add"))
                {
                    List<CaseSiteEntity> Site_Entity = new List<CaseSiteEntity>();
                    CaseSiteEntity Search_Entity = new CaseSiteEntity(true);
                    Search_Entity.SiteAGENCY = Agency; Search_Entity.SiteDEPT = Dept;
                    Search_Entity.SitePROG = Prog; Search_Entity.SiteYEAR = Year; Search_Entity.SiteROOM = TempCode.Trim();
                    Search_Entity.SiteNUMBER = txtRoomSite.Text; Search_Entity.SiteAM_PM = ((ListItem)cmbAmPm.SelectedItem).Value.ToString().Trim();
                    Site_Entity = _model.CaseMstData.Browse_CASESITE(Search_Entity, "Browse");
                    if (Site_Entity.Count > 0)
                    {
                        //MessageBox.Show("Room Already Exists", "CAP Systems");
                        //txtRoom.Text = string.Empty;
                        isExists = true;

                    }
                }

            }
            //UserEntity userProfile = _model.UserProfileAccess.GetUserProfileByID(userID);

            return isExists;
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PbHierarchies_Click(object sender, EventArgs e)
        {
            //HierarchieSelectionForm addForm = new HierarchieSelectionForm(BaseForm, "Master", string.Empty, string.Empty, "Casedep", string.Empty);
            //addForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
            //addForm.ShowDialog();
            if (Year == "    ")
            {
                AgencySelectionForm addAgencyForm = new AgencySelectionForm(BaseForm, "Agy", "A", SelAgency, SelDept, SelProg);
                addAgencyForm.FormClosed += new FormClosedEventHandler(OnAgencySelectionFormClosed);
                addAgencyForm.StartPosition = FormStartPosition.CenterScreen;
                addAgencyForm.ShowDialog();
            }
            else
            {
                AgencySelectionForm addAgencyForm = new AgencySelectionForm(BaseForm, "Agy", "P", SelAgency, SelDept, SelProg);
                addAgencyForm.FormClosed += new FormClosedEventHandler(OnAgencySelectionFormClosed);
                addAgencyForm.StartPosition = FormStartPosition.CenterScreen;
                addAgencyForm.ShowDialog();
                //HierarchieSelectionFormNew addForm = new HierarchieSelectionFormNew(BaseForm, string.Empty, "Master", string.Empty, "D", string.Empty);
                //addForm.FormClosed += new Form.FormClosedEventHandler(OnHierarchieFormClosed);
                //addForm.ShowDialog();
            }
        }


        private void OnAgencySelectionFormClosed(object sender, FormClosedEventArgs e)
        {
            AgencySelectionForm form = sender as AgencySelectionForm;
            if (form.DialogResult == DialogResult.OK)
            {
                string[] Agency_det = new string[2];
                Agency_det = form.GetAgency();
                txtHierarchy.Text = Agency_det[0];
                txtHierachydesc.Text = Agency_det[1];
                if (Year == "    ")
                    SelAgency = Agency_det[0];
                else
                {
                    SelAgency = txtHierarchy.Text.Substring(0, 2).Trim();
                    SelDept = txtHierarchy.Text.Substring(3, 2).Trim();
                    SelProg = txtHierarchy.Text.Substring(6, 2).Trim();
                }
            }
        }


        string SelAgency = string.Empty, SelDept = string.Empty, SelProg = string.Empty;
        private void OnHierarchieFormClosed(object sender, FormClosedEventArgs e)
        {
            HierarchieSelectionFormNew form = sender as HierarchieSelectionFormNew;

            if (form.DialogResult == DialogResult.OK)
            {
                List<HierarchyEntity> selectedHierarchies = form.SelectedHierarchies;
                string hierarchy = string.Empty;

                foreach (HierarchyEntity row in selectedHierarchies)
                {
                    hierarchy += row.Agency + row.Dept + row.Prog;
                    if (Year == "    ")
                    {
                        txtHierarchy.Text = row.Agency + "-" + "**" + "-" + "**";
                        HierarchyEntity hierachysubEntity = hierarchyEntity.Find(u => u.Code.Equals(txtHierarchy.Text.Trim()));
                        if (hierachysubEntity != null)
                        {
                            txtHierachydesc.Text = hierachysubEntity.HirarchyName;
                        }
                    }
                    else
                    {
                        txtHierarchy.Text = row.Code;
                        txtHierachydesc.Text = row.HirarchyName;
                    }
                    //string strCode = CaseSum.GETCASEELIGGCode(row.Agency + row.Dept + row.Prog);
                    //txtGroupCode.Text = "0000".Substring(0, 4 - strCode.Length) + strCode;
                    HiearchyCode = row.Agency + row.Dept + row.Prog;
                }
            }

        }

        private void btnroom_Save_Click(object sender, EventArgs e)
        {
            FillEntity();
            try
            {
                if (Room_ValidateForm())
                {

                    CaseSiteEntity Entity = new CaseSiteEntity();
                    string SqlMsg = string.Empty;
                    string msg = string.Empty;
                    CaptainModel Model = new CaptainModel();
                    if (Mode == "Edit")
                        Entity.Row_Type = "U";
                    else
                        Entity.Row_Type = "I";
                    Entity.SiteAGENCY = Sitelist[0].SiteAGENCY;
                    if (!string.IsNullOrEmpty(Sitelist[0].SiteDEPT))
                        Entity.SiteDEPT = Sitelist[0].SiteDEPT;
                    else
                        Entity.SiteDEPT = "  ";
                    if (!string.IsNullOrEmpty(Sitelist[0].SitePROG))
                        Entity.SitePROG = Sitelist[0].SitePROG;
                    else
                        Entity.SitePROG = "  ";
                    if (!string.IsNullOrEmpty(Sitelist[0].SiteYEAR))
                        Entity.SiteYEAR = Sitelist[0].SiteYEAR;
                    else
                        Entity.SiteYEAR = "    ";
                    Entity.SiteNUMBER = Sitelist[0].SiteNUMBER.Trim();
                    Entity.SiteTRAN_AREA = Sitelist[0].SiteNUMBER.Trim();
                    Entity.SiteROOM = txtRoom.Text.Trim();
                    Entity.SiteAM_PM = ((ListItem)cmbAmPm.SelectedItem).Value.ToString().Trim();
                    Entity.SiteNAME = Sitelist[0].SiteNAME.Trim();
                    Entity.SiteSTREET = Sitelist[0].SiteSTREET.Trim();
                    Entity.SiteCITY = Sitelist[0].SiteCITY.Trim();
                    Entity.SiteSTATE = Sitelist[0].SiteSTATE.Trim();
                    Entity.SiteZIP = Sitelist[0].SiteZIP.Trim();
                    Entity.SiteZIP_PLUS = Sitelist[0].SiteZIP_PLUS.Trim();
                    Entity.SitePHONE = maskRoom_PhnNo.Text.Trim().Replace("-", "");
                    Entity.SiteFAX = maskRoom_Fax.Text.Trim().Replace("-", "");
                    Entity.SiteOTHER_PHONE = maskRoom_OthPhn.Text.Trim().Replace("-", "");
                    Entity.SITE_FUNDED_SLOTS = txtSlots.Text.Trim();
                    Entity.SiteLANGUAGE = ((ListItem)cmbLang.SelectedItem).Value.ToString().Trim();
                    if (((ListItem)cmbLang.SelectedItem).Text.ToString().Trim().ToUpper() == "OTHER")
                        Entity.SiteLANGUAGE_OTHER = txtOtherLang.Text.Trim();
                    //else
                    //    Entity.SiteLANGUAGE_OTHER = ((ListItem)cmbLang.SelectedItem).Text.ToString().Trim();
                    Entity.SiteCOMMENT = txtComments.Text.Trim();
                    Entity.SiteFUND1 = ((ListItem)cmbFund1.SelectedItem).Value.ToString().Trim();
                    Entity.SiteFUND2 = ((ListItem)cmbFund2.SelectedItem).Value.ToString().Trim();
                    Entity.SiteFUND3 = ((ListItem)cmbFund3.SelectedItem).Value.ToString().Trim();
                    Entity.SiteFUND4 = ((ListItem)cmbFund4.SelectedItem).Value.ToString().Trim();
                    Entity.SiteFUND5 = ((ListItem)cmbFund5.SelectedItem).Value.ToString().Trim();
                    Entity.SiteHEDTEACHER = ((ListItem)cmbHeadTeacher.SelectedItem).Value.ToString().Trim();
                    Entity.SiteASSISTANT1 = ((ListItem)cmbAss1.SelectedItem).Value.ToString().Trim();
                    Entity.SiteASSISTANT2 = ((ListItem)cmbAss2.SelectedItem).Value.ToString().Trim();
                    Entity.SiteCLASS_START = LookupDataAccess.Getdate(dtpClsStrts.Value.ToString().Trim());
                    Entity.SiteCLASS_END = LookupDataAccess.Getdate(dtpClsEnds.Value.ToString().Trim());
                    Entity.SiteSTART_TIME = dtptimeStarts.Value.ToString("HH:mm:ss");
                    Entity.SiteEND_TIME = dtpTimeEnds.Value.ToString("HH:mm:ss");
                    Entity.SiteACTIVE = Sitelist[0].SiteACTIVE;
                    Entity.SiteADD_OPERATOR = BaseForm.UserID;
                    Entity.SiteLSTC_OPERATOR = BaseForm.UserID;
                    Entity.SITE_FUND_SLOT1 = txtF1Slots.Text.Trim();
                    Entity.SITE_FUND_SLOT2 = txtF2Slots.Text.Trim();
                    Entity.SITE_FUND_SLOT3 = txtF3Slots.Text.Trim();
                    Entity.SITE_FUND_SLOT4 = txtF4Slots.Text.Trim();
                    Entity.SITE_FUND_SLOT5 = txtF5Slots.Text.Trim();
                    Entity.SITE_HOMEBASE = chkHomebaseSite.Checked ? "Y" : "N";


                    int rowIndex = 0;
                    foreach (DataGridViewRow dr in gvMeal.Rows)
                    {

                        Entity.SiteMEAL_AREA += gvMeal.Rows[rowIndex].Cells["Mon"].Value.ToString().Trim() + gvMeal.Rows[rowIndex].Cells["Tue"].Value.ToString().Trim() +
                            gvMeal.Rows[rowIndex].Cells["Wed"].Value.ToString().Trim() + gvMeal.Rows[rowIndex].Cells["Thu"].Value.ToString().Trim() + gvMeal.Rows[rowIndex].Cells["Fri"].Value.ToString().Trim() +
                            gvMeal.Rows[rowIndex].Cells["Sat"].Value.ToString().Trim() + gvMeal.Rows[rowIndex].Cells["Sun"].Value.ToString().Trim();
                        rowIndex++;
                    }


                    if (_model.CaseMstData.UpdateCASESITE(Entity, "Update", out msg, out SqlMsg))
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        AlertBox.Show("Error, Please try again", MessageBoxIcon.Warning);
                    }

                }
            }
            catch (Exception ex)
            {
            }
        }

        private bool RoomChecking()
        {
            CaseSiteEntity Search_Entity = new CaseSiteEntity(true);
            Search_Entity.SiteAGENCY = Agency; Search_Entity.SiteDEPT = Dept;
            Search_Entity.SitePROG = Prog; Search_Entity.SiteYEAR = Year;
            Search_Entity.SiteNUMBER = Code; Search_Entity.SiteROOM = txtRoom.Text;
            Search_Entity.SiteROOM = ((ListItem)cmbAmPm.SelectedItem).Value.ToString().Trim();
            List<CaseSiteEntity> SiteChecklist = _model.CaseMstData.Browse_CASESITE(Search_Entity, "Browse");
            if (SiteChecklist.Count > 0)
                return false;
            else
                return true;
        }

        private bool Room_ValidateForm()
        {
            bool Room_isValid = true;
            if (!string.IsNullOrEmpty(txtRoom.Text.Trim()))
            {
                if ((txtRoom.Text == "0000") || (txtRoom.Text == "000") || (txtRoom.Text == "00") || (txtRoom.Text == "0"))
                {
                    _errorProvider.SetError(txtRoom, "Room can be greater than 0".Replace(Consts.Common.Colon, string.Empty));
                    Room_isValid = false;
                }
                else if (isCodeExists(txtRoom.Text))
                {
                    _errorProvider.SetError(txtRoom, string.Format(Consts.Messages.AlreadyExists.GetMessage(), lblRoom.Text.Replace(Consts.Common.Colon, string.Empty)));
                    Room_isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtRoom, null);
                }
            }
            else if (string.IsNullOrEmpty(txtRoom.Text.Trim()))
            {
                _errorProvider.SetError(txtRoom, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblRoom.Text.Replace(Consts.Common.Colon, string.Empty)));
                Room_isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtRoom, null);
            }



            if (dtpClsStrts.Checked.Equals(false))
            {
                _errorProvider.SetError(dtpClsStrts, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblDtClsStarts.Text.Replace(Consts.Common.Colon, string.Empty)));
                Room_isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtpClsStrts, null);
            }

            if (dtpClsEnds.Checked.Equals(false))
            {
                _errorProvider.SetError(dtpClsEnds, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblDtClsEnds.Text.Replace(Consts.Common.Colon, string.Empty)));
                Room_isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtpClsEnds, null);
            }

            if (dtpClsStrts.Checked.Equals(true) && dtpClsEnds.Checked.Equals(true))
            {
                if (string.IsNullOrWhiteSpace(dtpClsStrts.Text))
                {
                    _errorProvider.SetError(dtpClsStrts, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblDtClsStarts.Text.Replace(Consts.Common.Colon, string.Empty)));
                    Room_isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtpClsStrts, null);
                }
                if (string.IsNullOrWhiteSpace(dtpClsEnds.Text))
                {
                    _errorProvider.SetError(dtpClsEnds, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblDtClsEnds.Text.Replace(Consts.Common.Colon, string.Empty)));
                    Room_isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtpClsEnds, null);
                }
            }

            if (dtpClsStrts.Checked.Equals(true) && dtpClsEnds.Checked.Equals(true))
            {
                if (!string.IsNullOrEmpty(dtpClsStrts.Text) && (!string.IsNullOrEmpty(dtpClsEnds.Text)))
                {
                    if (Convert.ToDateTime(dtpClsStrts.Value.ToShortDateString()) > Convert.ToDateTime(dtpClsEnds.Value.ToShortDateString()))
                    {
                        _errorProvider.SetError(dtpClsEnds, "Class End Date should be equal or greater than Class Start Date ".Replace(Consts.Common.Colon, string.Empty));
                        Room_isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(dtpClsEnds, null);
                    }
                }
            }

            if (dtptimeStarts.Checked.Equals(false))
            {
                _errorProvider.SetError(dtptimeStarts, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblTimeStarts.Text.Replace(Consts.Common.Colon, string.Empty)));
                Room_isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtptimeStarts, null);
            }

            if (dtpTimeEnds.Checked.Equals(false))
            {
                _errorProvider.SetError(dtpTimeEnds, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblTimeEnds.Text.Replace(Consts.Common.Colon, string.Empty)));
                Room_isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtpTimeEnds, null);
            }

            if (dtptimeStarts.Checked.Equals(true) && dtpTimeEnds.Checked.Equals(true))
            {
                if (string.IsNullOrWhiteSpace(dtptimeStarts.Text))
                {
                    _errorProvider.SetError(dtptimeStarts, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblTimeStarts.Text.Replace(Consts.Common.Colon, string.Empty)));
                    Room_isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtptimeStarts, null);
                }
                if (string.IsNullOrWhiteSpace(dtpTimeEnds.Text))
                {
                    _errorProvider.SetError(dtpTimeEnds, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblTimeEnds.Text.Replace(Consts.Common.Colon, string.Empty)));
                    Room_isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtpTimeEnds, null);
                }
            }

            if (dtptimeStarts.Checked.Equals(true) && dtpTimeEnds.Checked.Equals(true))
            {
                if (!string.IsNullOrEmpty(dtptimeStarts.Text) && (!string.IsNullOrEmpty(dtpTimeEnds.Text)))
                {
                    if (Convert.ToDateTime(dtptimeStarts.Text) > Convert.ToDateTime(dtpTimeEnds.Text))
                    {
                        //_errorProvider.SetError(dtptimeStarts, "Start time shouldn't be greater or equal to End time ".Replace(Consts.Common.Colon, string.Empty));
                        _errorProvider.SetError(dtpTimeEnds, "Class End Time should be equal or greater than Class Start Time".Replace(Consts.Common.Colon, string.Empty));
                        Room_isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(dtpTimeEnds, null);
                    }
                }
            }

            if (((ListItem)cmbLang.SelectedItem).Text.ToString().Trim() == "OTHER")
            {
                if (string.IsNullOrEmpty(txtOtherLang.Text.Trim()))
                {
                    _errorProvider.SetError(txtOtherLang, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Other " + lblClsLang.Text.Replace(Consts.Common.Colon, string.Empty)));
                    Room_isValid = false;
                }
                else
                    _errorProvider.SetError(txtOtherLang, null);
            }
            else
                _errorProvider.SetError(txtOtherLang, null);



            if (maskRoom_PhnNo.Text != "" && maskRoom_PhnNo.Text != "   -   -")
            {
                if (maskRoom_PhnNo.Text.Trim().Replace("-", "").Replace(" ", "").Length < 10)
                {
                    _errorProvider.SetError(maskRoom_PhnNo, "Please enter valid Phone Number");
                    Room_isValid = false;
                }
                else
                {
                    _errorProvider.SetError(maskRoom_PhnNo, null);
                }
            }
            if (maskRoom_Fax.Text != "" && maskRoom_Fax.Text != "   -   -")
            {
                if (maskRoom_Fax.Text.Trim().Replace("-", "").Replace(" ", "").Length < 10)
                {
                    _errorProvider.SetError(maskRoom_Fax, "Please enter valid Fax Number");
                    Room_isValid = false;
                }
                else
                {
                    _errorProvider.SetError(maskRoom_Fax, null);
                }
            }
            if (maskRoom_OthPhn.Text != "" && maskRoom_OthPhn.Text != "   -   -")
            {
                if (maskRoom_OthPhn.Text.Trim().Replace("-", "").Replace(" ", "").Length < 10)
                {
                    _errorProvider.SetError(maskRoom_OthPhn, "Please enter valid Other Number");
                    Room_isValid = false;
                }
                else
                {
                    _errorProvider.SetError(maskOther, null);
                }
            }

            return Room_isValid;
        }



        //private bool isCodeExists(string RmCode)
        //{
        //    bool isExists = false;
        //    List<CaseSiteEntity> Site_Entity = new List<CaseSiteEntity>();
        //    CaseSiteEntity Search_Entity = new CaseSiteEntity(true);
        //    Search_Entity.SiteAGENCY = Agency; Search_Entity.SiteDEPT = Dept;
        //    Search_Entity.SitePROG = Prog; Search_Entity.SiteYEAR = Year;
        //    Search_Entity.SiteNUMBER = RmCode.Trim(); Search_Entity.SiteAM_PM = ((ListItem)cmbAmPm.SelectedItem).Value.ToString().Trim();
        //    Site_Entity = _model.CaseMstData.Browse_CASESITE(Search_Entity, "Browse");
        //    if (Site_Entity.Count > 0)
        //    {
        //        //MessageBox.Show("Room Already Exists", "CAP Systems");
        //        //txtRoom.Text = string.Empty;
        //        isExists = true;

        //    }

        //    return isExists;
        //}

        private void gvMeal_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (gvMeal.Rows.Count > 0)
            {
                if (e.ColumnIndex == 1 && e.RowIndex != -1 && (Mode.Equals("Add") || Mode.Equals("Edit")))
                {
                    if (gvMeal.CurrentRow.Cells["Mon"].Value.ToString() == "Y")
                    {
                        gvMeal.CurrentRow.Cells["Image_Mon"].Value = Img_Cross;
                        gvMeal.CurrentRow.Cells["Mon"].Value = "N";
                    }
                    else
                    {
                        gvMeal.CurrentRow.Cells["Image_Mon"].Value = Img_Tick;
                        gvMeal.CurrentRow.Cells["Mon"].Value = "Y";
                    }
                }
                if (e.ColumnIndex == 2 && e.RowIndex != -1 && (Mode.Equals("Add") || Mode.Equals("Edit")))
                {
                    if (gvMeal.CurrentRow.Cells["Tue"].Value.ToString() == "Y")
                    {
                        gvMeal.CurrentRow.Cells["Image_Tue"].Value = Img_Cross;
                        gvMeal.CurrentRow.Cells["Tue"].Value = "N";
                    }
                    else
                    {
                        gvMeal.CurrentRow.Cells["Image_Tue"].Value = Img_Tick;
                        gvMeal.CurrentRow.Cells["Tue"].Value = "Y";
                    }
                }
                if (e.ColumnIndex == 3 && e.RowIndex != -1 && (Mode.Equals("Add") || Mode.Equals("Edit")))
                {
                    if (gvMeal.CurrentRow.Cells["Wed"].Value.ToString() == "Y")
                    {
                        gvMeal.CurrentRow.Cells["Image_Wed"].Value = Img_Cross;
                        gvMeal.CurrentRow.Cells["Wed"].Value = "N";
                    }
                    else
                    {
                        gvMeal.CurrentRow.Cells["Image_Wed"].Value = Img_Tick;
                        gvMeal.CurrentRow.Cells["Wed"].Value = "Y";
                    }
                }
                if (e.ColumnIndex == 4 && e.RowIndex != -1 && (Mode.Equals("Add") || Mode.Equals("Edit")))
                {
                    if (gvMeal.CurrentRow.Cells["Thu"].Value.ToString() == "Y")
                    {
                        gvMeal.CurrentRow.Cells["Image_Thu"].Value = Img_Cross;
                        gvMeal.CurrentRow.Cells["Thu"].Value = "N";
                    }
                    else
                    {
                        gvMeal.CurrentRow.Cells["Image_Thu"].Value = Img_Tick;
                        gvMeal.CurrentRow.Cells["Thu"].Value = "Y";
                    }
                }
                if (e.ColumnIndex == 5 && e.RowIndex != -1 && (Mode.Equals("Add") || Mode.Equals("Edit")))
                {
                    if (gvMeal.CurrentRow.Cells["Fri"].Value.ToString() == "Y")
                    {
                        gvMeal.CurrentRow.Cells["Image_Fri"].Value = Img_Cross;
                        gvMeal.CurrentRow.Cells["Fri"].Value = "N";
                    }
                    else
                    {
                        gvMeal.CurrentRow.Cells["Image_Fri"].Value = Img_Tick;
                        gvMeal.CurrentRow.Cells["Fri"].Value = "Y";
                    }
                }
                if (e.ColumnIndex == 6 && e.RowIndex != -1 && (Mode.Equals("Add") || Mode.Equals("Edit")))
                {
                    if (gvMeal.CurrentRow.Cells["Sat"].Value.ToString() == "Y")
                    {
                        gvMeal.CurrentRow.Cells["Image_Sat"].Value = Img_Cross;
                        gvMeal.CurrentRow.Cells["Sat"].Value = "N";
                    }
                    else
                    {
                        gvMeal.CurrentRow.Cells["Image_Sat"].Value = Img_Tick;
                        gvMeal.CurrentRow.Cells["Sat"].Value = "Y";
                    }
                }
                if (e.ColumnIndex == 7 && e.RowIndex != -1 && (Mode.Equals("Add") || Mode.Equals("Edit")))
                {
                    if (gvMeal.CurrentRow.Cells["Sun"].Value.ToString() == "Y")
                    {
                        gvMeal.CurrentRow.Cells["Image_Sun"].Value = Img_Cross;
                        gvMeal.CurrentRow.Cells["Sun"].Value = "N";
                    }
                    else
                    {
                        gvMeal.CurrentRow.Cells["Image_Sun"].Value = Img_Tick;
                        gvMeal.CurrentRow.Cells["Sun"].Value = "Y";
                    }
                }
            }
        }

        private void fillRoomControls()
        {
            CaseSiteEntity Search_Entity = new CaseSiteEntity(true);
            Search_Entity.SiteAGENCY = Agency; Search_Entity.SiteDEPT = Dept;
            Search_Entity.SitePROG = Prog; Search_Entity.SiteYEAR = Year;
            Search_Entity.SiteNUMBER = Code.Trim(); Search_Entity.SiteROOM = strHierarcy_Desc.Trim();
            Search_Entity.SiteAM_PM = strHiearchy;
            Site_List = _model.CaseMstData.Browse_CASESITE(Search_Entity, "Browse");

            CaseSiteEntity Entity = Site_List[0];
            txtRoom.Text = Entity.SiteROOM.Trim();
            SetComboBoxValue(cmbAmPm, Entity.SiteAM_PM.Trim());
            SetComboBoxValue(cmbLang, Entity.SiteLANGUAGE.Trim());
            txtOtherLang.Text = Entity.SiteLANGUAGE_OTHER.Trim();
            SetComboBoxValue(cmbFund1, Entity.SiteFUND1.Trim());
            SetComboBoxValue(cmbFund2, Entity.SiteFUND2.Trim());
            SetComboBoxValue(cmbFund3, Entity.SiteFUND3.Trim());
            SetComboBoxValue(cmbFund4, Entity.SiteFUND4.Trim());
            SetComboBoxValue(cmbFund5, Entity.SiteFUND5.Trim());
            SetComboBoxValue(cmbHeadTeacher, Entity.SiteHEDTEACHER.Trim());
            SetComboBoxValue(cmbAss1, Entity.SiteASSISTANT1.Trim());
            SetComboBoxValue(cmbAss2, Entity.SiteASSISTANT2.Trim());
            maskRoom_PhnNo.Text = Entity.SitePHONE.Trim().Replace("-", "");
            maskRoom_Fax.Text = Entity.SiteFAX.Trim().Replace("-", "");
            maskRoom_OthPhn.Text = Entity.SiteOTHER_PHONE.Trim().Replace("-", "");
            txtSlots.Text = Entity.SITE_FUNDED_SLOTS.Trim();
            txtF1Slots.Text = Entity.SITE_FUND_SLOT1.Trim();
            txtF2Slots.Text = Entity.SITE_FUND_SLOT2.Trim();
            txtF3Slots.Text = Entity.SITE_FUND_SLOT3.Trim();
            txtF4Slots.Text = Entity.SITE_FUND_SLOT4.Trim();
            txtF5Slots.Text = Entity.SITE_FUND_SLOT5.Trim();

            if (Entity.SITE_HOMEBASE == "Y")
                chkHomebaseSite.Checked = true;
            else
                chkHomebaseSite.Checked = false;

            if (!string.IsNullOrEmpty(Entity.SiteCLASS_START))
            {
                dtpClsStrts.Text = CommonFunctions.ChangeDateFormat(Entity.SiteCLASS_START, Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);
                //Convert.ToDateTime(LookupDataAccess.Getdate(Entity.SiteCLASS_START.Trim()));
                dtpClsStrts.Checked = true;
            }
            if (!string.IsNullOrEmpty(Entity.SiteCLASS_END))
            {
                dtpClsEnds.Text = CommonFunctions.ChangeDateFormat(Entity.SiteCLASS_END, Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);
                //Convert.ToDateTime(LookupDataAccess.Getdate(Entity.SiteCLASS_END.Trim()));
                dtpClsEnds.Checked = true;
            }
            if (!string.IsNullOrEmpty(Entity.SiteSTART_TIME))
            {
                dtptimeStarts.Value = Convert.ToDateTime(LookupDataAccess.GetTime(Entity.SiteSTART_TIME.Trim()));
                dtptimeStarts.Checked = true;
            }
            if (!string.IsNullOrEmpty(Entity.SiteEND_TIME))
            {
                dtpTimeEnds.Value = Convert.ToDateTime(LookupDataAccess.GetTime(Entity.SiteEND_TIME.Trim()));
                dtpTimeEnds.Checked = true;
            }
            txtComments.Text = Entity.SiteCOMMENT.Trim();
            Meals = Entity.SiteMEAL_AREA.Trim();
            fillEditGvMeal();
        }

        private void EnabledisableRoom()
        {
            txtRoom.Enabled = false;
            cmbAmPm.Enabled = false;
            cmbLang.Enabled = false;
            txtOtherLang.Enabled = false;
            cmbFund1.Enabled = false;
            cmbFund2.Enabled = false;
            cmbFund3.Enabled = false;
            cmbFund4.Enabled = false;
            cmbFund5.Enabled = false;
            cmbHeadTeacher.Enabled = false;
            cmbAss1.Enabled = false;
            cmbAss2.Enabled = false;
            maskRoom_PhnNo.Enabled = false;
            maskRoom_Fax.Enabled = false;
            maskRoom_OthPhn.Enabled = false;
            txtSlots.Enabled = false;
            txtF1Slots.Enabled = false;
            txtF2Slots.Enabled = false;
            txtF3Slots.Enabled = false;
            txtF4Slots.Enabled = false;
            txtF5Slots.Enabled = false;

            dtpClsStrts.Enabled = false;
            dtpClsEnds.Enabled = false;
            dtptimeStarts.Enabled = false;
            dtpTimeEnds.Enabled = false;
            txtComments.Enabled = false;
            gvMeal.Enabled = false;
            btnroom_Save.Visible = false; btnRoomCancel.Visible = false;
            btnZipSearch.Visible = false;
        }

        string Meals = string.Empty;
        private void fillEditGvMeal()
        {
            int i = 0; int rowIndex = 0;
            foreach (DataGridViewRow dr in gvMeal.Rows)
            {
                //int rowIndex=0;
                string TempMeal = Meals.Substring(i, 7).ToString().Trim();
                for (int j = 0; j < TempMeal.Length; j++)
                {
                    string meal_Temp = TempMeal.Substring(j, 1).ToString().Trim();
                    gvMeal.Rows[rowIndex].Cells[j + 8].Value = meal_Temp;
                    if (meal_Temp == "Y")
                        gvMeal.Rows[rowIndex].Cells[j + 1].Value = Img_Tick;
                    else
                        gvMeal.Rows[rowIndex].Cells[j + 1].Value = Img_Cross;
                }
                i += 7; rowIndex++;
            }
        }

        private void SetComboBoxValue(ComboBox comboBox, string value)
        {
            if (comboBox != null && comboBox.Items.Count > 0)
            {
                foreach (ListItem li in comboBox.Items)
                {
                    if (li.Value.Equals(value) || li.Text.Equals(value))
                    {
                        comboBox.SelectedItem = li;
                        break;
                    }
                }
            }
        }

        private void cmbAmPm_SelectedIndexChanged(object sender, EventArgs e)
        {
            //List<CaseSiteEntity> Site_Entity = new List<CaseSiteEntity>();
            //CaseSiteEntity Search_Entity = new CaseSiteEntity(true);
            //Search_Entity.SiteAGENCY = Agency; Search_Entity.SiteDEPT = Dept;
            //Search_Entity.SitePROG = Prog; Search_Entity.SiteYEAR = Year;
            //Search_Entity.SiteNUMBER = Code; Search_Entity.SiteAM_PM = ((ListItem)cmbAmPm.SelectedItem).Value.ToString().Trim();
            //Site_Entity = _model.CaseMstData.Browse_CASESITE(Search_Entity, "Browse");

            //foreach (CaseSiteEntity Entity in Site_Entity)
            //{
            //    if (Entity.SiteROOM.Trim() == txtRoom.Text.Trim())
            //    {
            //        MessageBox.Show("Room Already Exists", "CAP Systems");
            //        txtRoom.Text = string.Empty;
            //    }
            //}

        }

        private void cmbLang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ListItem)cmbLang.SelectedItem).Text.ToString().Trim() == "OTHER" || ((ListItem)cmbLang.SelectedItem).Text.ToString().Trim() == "Other")
            {
                txtOtherLang.Visible = true;
                Size = new Size(772, 566);
                this.room_Panel2.Size = new Size(366, 455);
               // this.lblFundedSlots.Location = new Point(678, 12);
            }
            else
            {
                txtOtherLang.Visible = false;
                Size = new Size(702, 566);
                this.room_Panel2.Size = new Size(294, 455);
                //this.lblFundedSlots.Location = new Point(608, 12);
            }
        }

        private void txtF1Slots_Leave(object sender, EventArgs e)
        {
            //if (!string.IsNullOrEmpty(txtF1Slots.Text.Trim()))
            //{
            //    if (((ListItem)cmbFund1.SelectedItem).Value.ToString() == "None")
            //        txtF1Slots.Text = "";
            //}
        }

        private void ADMN0015Form_ToolClick(object sender, ToolClickEventArgs e)
        {
            Application.Navigate(CommonFunctions.CreateZenHelps(Privileges.Program, 0, BaseForm.BusinessModuleID.ToString(), "", ""), target: "_blank");
        }

        private void cmbSiteName_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSite.Text = ((Captain.Common.Utilities.ListItem)cmbSiteName.SelectedItem).Value.ToString().Trim();
            txtSiteName.Text = ((Captain.Common.Utilities.ListItem)cmbSiteName.SelectedItem).Text.ToString().Trim();
        }

        private void txtRentCost_Leave(object sender, EventArgs e)
        {
            if (txtRentCost.Text.Trim().Length > 6)
            {
                txtRentCost.Text = txtRentCost.Text.Substring(1, 6).Trim();
            }
        }

        private void txtUtilitycost_Leave(object sender, EventArgs e)
        {
            if (txtUtilitycost.Text.Trim().Length > 6)
            {
                txtUtilitycost.Text = txtUtilitycost.Text.Substring(1, 6).Trim();
            }
        }



    }
}