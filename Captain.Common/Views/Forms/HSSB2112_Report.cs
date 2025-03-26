#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using Wisej.Web;
using Captain.Common.Views.Controls.Compatibility;
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
using System.Xml;
using System.IO;

using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Globalization;
using DevExpress.CodeParser;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class HSSB2112_Report : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        //private GridControl _intakeHierarchy = null;
        private bool boolChangeStatus = false;
        private CaptainModel _model = null;
        private List<HierarchyEntity> _selectedHierarchies = null;

        #endregion

        public HSSB2112_Report(BaseForm baseForm, PrivilegeEntity privileges)
        {
            InitializeComponent();
            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            BaseForm = baseForm;
            Privileges = privileges;
            Agency = BaseForm.BaseAgency; Depart = BaseForm.BaseDept; Program = BaseForm.BaseProg;
            strYear = BaseForm.BaseYear;
            propfundingSource = _model.lookupDataAccess.GetAgyTabRecordsByCodefilter(Consts.AgyTab.CASEMNGMTFUNDSRC, "H");
            Set_Report_Hierarchy(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear);

            propReportPath = _model.lookupDataAccess.GetReportPath();
            this.Text = /*Privileges.Program + " - " +*/ Privileges.PrivilegeName.Trim();
        }

        string Agency = string.Empty, Depart = string.Empty, Program = string.Empty, strYear = string.Empty;
        #region properties

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public string Calling_ID { get; set; }

        public string Calling_UserID { get; set; }

        public string propReportPath { get; set; }
        public List<CaseSiteEntity> propCaseSiteEntity { get; set; }
        public List<CommonEntity> propfundingSource { get; set; }
        public List<ChldTrckEntity> propTasks { get; set; }
        public List<ChldTrckEntity> TrackList { get; set; }
        

        #endregion

        private void CmbYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program_Year = "    ";
            if (!(string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString())))
                Program_Year = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString();
        }

        string Program_Year;
        private void Set_Report_Hierarchy(string Agy, string Dept, string Prog, string Year)
        {
            Txt_HieDesc.Clear();
            CmbYear.Visible = false;
            Program_Year = "    ";
            Current_Hierarchy = Agy + Dept + Prog;
            Current_Hierarchy_DB = Agy + "-" + Dept + "-" + Prog;

            if (Agy != "**")
            {
                DataSet ds_AGY = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(Agy, "**", "**");
                if (ds_AGY.Tables.Count > 0)
                {
                    if (ds_AGY.Tables[0].Rows.Count > 0)
                        Txt_HieDesc.Text += "AGY : " + Agy + " - " + (ds_AGY.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim() + "      ";
                }
            }
            else
                Txt_HieDesc.Text += "AGY : ** - All Agencies      ";

            if (Dept != "**")
            {
                DataSet ds_DEPT = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(Agy, Dept, "**");
                if (ds_DEPT.Tables.Count > 0)
                {
                    if (ds_DEPT.Tables[0].Rows.Count > 0)
                        Txt_HieDesc.Text += "DEPT : " + Dept + " - " + (ds_DEPT.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim() + "      ";
                }
            }
            else
                Txt_HieDesc.Text += "DEPT : ** - All Departments      ";

            if (Prog != "**")
            {
                DataSet ds_PROG = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(Agy, Dept, Prog);
                if (ds_PROG.Tables.Count > 0)
                {
                    if (ds_PROG.Tables[0].Rows.Count > 0)
                        Txt_HieDesc.Text += "PROG : " + Prog + " - " + (ds_PROG.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim();
                }
            }
            else
                Txt_HieDesc.Text += "PROG : ** - All Programs ";


            if (Agy != "**")
                Get_NameFormat_For_Agencirs(Agy);
            else
                Member_NameFormat = CAseWorkerr_NameFormat = "1";

            if (Agy != "**" && Dept != "**" && Prog != "**")
            {
                FillYearCombo(Agy, Dept, Prog, Year);
                rdoMultipleSites.Enabled = true;
            }
            else
            {
                this.Txt_HieDesc.Size = new System.Drawing.Size(630, 25);
                rdoMultipleSites.Enabled = false;
            }
        }

        string DepYear;
        bool DefHieExist = false;
        private void FillYearCombo(string Agy, string Dept, string Prog, string Year)
        {
            CmbYear.Visible = DefHieExist = false;
            Program_Year = "    ";
            if (!string.IsNullOrEmpty(Year.Trim()))
                DefHieExist = true;

            DataSet ds = Captain.DatabaseLayer.MainMenu.GetCaseDepForHierarchy(Agy, Dept, Prog);
            if (ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                int YearIndex = 0;

                if (dt.Rows.Count > 0)
                {
                    Program_Year = DepYear = dt.Rows[0]["DEP_YEAR"].ToString();
                    if (!(String.IsNullOrEmpty(DepYear)) && DepYear != null && DepYear != "    ")
                    {
                        int TmpYear = int.Parse(DepYear);
                        int TempCompareYear = 0;
                        string TmpYearStr = null;
                        if (!(String.IsNullOrEmpty(Year)) && Year != null && Year != " " && DefHieExist)
                            TempCompareYear = int.Parse(Year);
                        List<Captain.Common.Utilities.ListItem> listItem = new List<Captain.Common.Utilities.ListItem>();
                        for (int i = 0; i < 10; i++)
                        {
                            TmpYearStr = (TmpYear - i).ToString();
                            listItem.Add(new Captain.Common.Utilities.ListItem(TmpYearStr, i));
                            if (TempCompareYear == (TmpYear - i) && TmpYear != 0 && TempCompareYear != 0)
                                YearIndex = i;
                        }

                        CmbYear.Items.AddRange(listItem.ToArray());

                        CmbYear.Visible = true;

                        if (DefHieExist)
                            CmbYear.SelectedIndex = YearIndex;
                        else
                            CmbYear.SelectedIndex = 0;
                    }
                }
            }

            Agency = Agy; Depart = Dept; Program = Prog; strYear = Year;
           
            //fillBusCombo(Agency, Depart, Program, strYear);
            if (!string.IsNullOrEmpty(Program_Year.Trim()))
                this.Txt_HieDesc.Size = new System.Drawing.Size(550, 25);
            else
                this.Txt_HieDesc.Size = new System.Drawing.Size(630, 25);
        }

        string Member_NameFormat = "1", CAseWorkerr_NameFormat = "1";
        private void Get_NameFormat_For_Agencirs(string Agency)
        {
            DataSet ds = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(Agency, "**", "**");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Member_NameFormat = ds.Tables[0].Rows[0]["HIE_CN_FORMAT"].ToString();
                    CAseWorkerr_NameFormat = ds.Tables[0].Rows[0]["HIE_CW_FORMAT"].ToString();
                }
            }

        }

        List<CommonEntity> SelFundingList = new List<CommonEntity>();
        private void FundingForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            HSSB2111FundForm form = sender as HSSB2111FundForm;
            if (form.DialogResult == DialogResult.OK)
            {
                SelFundingList = form.GetSelectedFundings();
            }
        }

        private void rdoFundingSelect_Click(object sender, EventArgs e)
        {
            if (rdoFundingSelect.Checked == true)
            {
                HSSB2111FundForm FundingForm = new HSSB2111FundForm(BaseForm, SelFundingList, Privileges);
                FundingForm.FormClosed += new FormClosedEventHandler(FundingForm_FormClosed);
                FundingForm.StartPosition = FormStartPosition.CenterScreen;
                FundingForm.ShowDialog();
            }
        }

        List<CaseSiteEntity> Sel_REFS_List = new List<CaseSiteEntity>();
        private void On_Room_Select_Closed(object sender, FormClosedEventArgs e)
        {
            Site_SelectionForm form = sender as Site_SelectionForm;
            if (form.DialogResult == DialogResult.OK)
            {
                Sel_REFS_List = form.GetSelected_Sites();
            }
        }

        private void rdoMultipleSites_Click(object sender, EventArgs e)
        {
            if (rdoMultipleSites.Checked == true)
            {
                Site_SelectionForm SiteSelection = new Site_SelectionForm(BaseForm, "Room", Sel_REFS_List, "Report", Agency.Trim(), Depart.Trim(), Program.Trim(), Program_Year, Privileges);
                SiteSelection.FormClosed += new FormClosedEventHandler(On_Room_Select_Closed);
                SiteSelection.StartPosition = FormStartPosition.CenterScreen;
                SiteSelection.ShowDialog();
            }
        }

        string strsiteRoomNames = string.Empty; string strFundingCodes = string.Empty;
        private void btnGeneratePdf_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                strsiteRoomNames = string.Empty;
                if (rdoMultipleSites.Checked == true)
                {
                    foreach (CaseSiteEntity siteRoom in Sel_REFS_List)
                    {
                        if (!strsiteRoomNames.Equals(string.Empty)) strsiteRoomNames += ",";
                        strsiteRoomNames += siteRoom.SiteNUMBER + siteRoom.SiteROOM + siteRoom.SiteAM_PM;
                    }
                }
                else
                {
                    string Year_value = string.Empty; strsiteRoomNames = "A";
                    if (CmbYear.Visible == true)
                        Year_value = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString().Trim();
                    else Year_value = null;
                    CaseSiteEntity searchEntity = new CaseSiteEntity(true);
                    searchEntity.SiteAGENCY = Agency; searchEntity.SiteDEPT = Depart; searchEntity.SitePROG = Program;
                    searchEntity.SiteYEAR = Year_value;
                    List<CaseSiteEntity> AllSites = _model.CaseMstData.Browse_CASESITE(searchEntity, "Browse");
                    if (BaseForm.BaseAgencyControlDetails.SiteSecurity == "1")
                    {
                        List<HierarchyEntity> userHierarchy = _model.UserProfileAccess.GetUserHierarchyByID(BaseForm.UserID);
                        HierarchyEntity hierarchyEntity = new HierarchyEntity();
                        foreach (HierarchyEntity Entity in userHierarchy)
                        {
                            if (Entity.InActiveFlag == "N")
                            {
                                if (Entity.Agency == Agency && Entity.Dept == Depart && Entity.Prog == Program)
                                    hierarchyEntity = Entity;
                                else if (Entity.Agency == Agency && Entity.Dept == Depart && Entity.Prog == "**")
                                    hierarchyEntity = Entity;
                                else if (Entity.Agency == Agency && Entity.Dept == "**" && Entity.Prog == "**")
                                    hierarchyEntity = Entity;
                                else if (Entity.Agency == "**" && Entity.Dept == "**" && Entity.Prog == "**")
                                { hierarchyEntity = null; strsiteRoomNames = "A"; }
                            }
                        }

                        if (hierarchyEntity != null)
                        {
                            if (hierarchyEntity.Sites.Length > 0)
                            {
                                string[] Sites = hierarchyEntity.Sites.Split(',');

                                for (int i = 0; i < Sites.Length; i++)
                                {
                                    if (!string.IsNullOrEmpty(Sites[i].ToString().Trim()))
                                    {
                                        foreach (CaseSiteEntity casesite in AllSites) //Site_List)//ListcaseSiteEntity)
                                        {
                                            if (Sites[i].ToString() == casesite.SiteNUMBER && casesite.SiteROOM != "0000")
                                            {
                                                if (!strsiteRoomNames.Equals(string.Empty)) strsiteRoomNames += ",";
                                                strsiteRoomNames += casesite.SiteNUMBER + casesite.SiteROOM + casesite.SiteAM_PM;
                                                //break;
                                            }
                                            // Sel_Site_Codes += "'" + casesite.SiteNUMBER + "' ,";
                                        }
                                    }
                                }
                                //strsiteRoomNames = hierarchyEntity.Sites;
                            }
                        }


                    }
                    else
                        strsiteRoomNames = "A";
                }


                strFundingCodes = string.Empty;
                if (rdoFundingSelect.Checked == true)
                {
                    foreach (CommonEntity FundingCode in SelFundingList)
                    {
                        if (!strFundingCodes.Equals(string.Empty)) strFundingCodes += ",";
                        strFundingCodes += FundingCode.Code;
                    }
                }
                else
                {
                    strFundingCodes = "A";
                }

                PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, false, propReportPath, "PDF");
                pdfListForm.FormClosed += new FormClosedEventHandler(On_SaveFormClosed);
                pdfListForm.StartPosition = FormStartPosition.CenterScreen;
                pdfListForm.ShowDialog();
            }
        }

        private bool ValidateForm()
        {
            bool isValid = true;
            _errorProvider.SetError(dtFrom, null);
            _errorProvider.SetError(dtTodate, null);
            _errorProvider.SetError(rdoMultipleSites, null);

            if (dtFrom.Checked == false && dtTodate.Checked == false)
            {
                _errorProvider.SetError(dtTodate, string.Format("Please select Report From and To Dates"));
                isValid = false;
            }
            else
                _errorProvider.SetError(dtTodate, null);

            if (dtFrom.Checked == true || dtTodate.Checked == true)
            {
                if (dtFrom.Checked == false && dtTodate.Checked == true)
                {
                    _errorProvider.SetError(dtFrom, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Report From Date".Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtFrom, null);
                }
            }

            if (dtFrom.Checked == true || dtTodate.Checked == true)
            {
                if (dtTodate.Checked == false && dtFrom.Checked == true)
                {
                    _errorProvider.SetError(dtTodate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Report To Date".Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtTodate, null);
                }
            }
            if (dtFrom.Checked.Equals(true) && dtTodate.Checked.Equals(true))
            {
                if (string.IsNullOrWhiteSpace(dtFrom.Text))
                {
                    _errorProvider.SetError(dtFrom, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Report From Date".Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtFrom, null);
                }
                if (string.IsNullOrWhiteSpace(dtTodate.Text))
                {
                    _errorProvider.SetError(dtTodate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Report To Date".Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtTodate, null);
                }
            }

            if (dtFrom.Checked && dtTodate.Checked == true)
            {
                if (!string.IsNullOrWhiteSpace(dtFrom.Text) && !string.IsNullOrWhiteSpace(dtTodate.Text))
                {
                    if (dtFrom.Value.Date > dtTodate.Value.Date)
                    {
                        _errorProvider.SetError(dtTodate, "Report 'To Date' should be equal to or greater than 'From Date'");
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(dtTodate, null);
                    }
                }
            }

            /*if (dtForm.Checked == false)
            {
                _errorProvider.SetError(dtForm, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblReportFormdt.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtForm, null);
            }
            if (dtTodate.Checked == false)
            {
                _errorProvider.SetError(dtTodate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblToDate.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtTodate, null);
            }

            if ((dtForm.Checked == true) && (dtTodate.Checked == true))
            {

                if (dtForm.Value.Date > dtTodate.Value.Date)
                {
                    _errorProvider.SetError(dtTodate, "From Date Greater than To date");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtTodate, null);
                }
            }*/

            if (rdoMultipleSites.Checked == true)
            {
                if (Sel_REFS_List.Count == 0)
                {
                    _errorProvider.SetError(rdoMultipleSites, "Please Select at least One Site");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(rdoMultipleSites, null);
                }
            }

            if (rdoFundingSelect.Checked == true)
            {
                if (SelFundingList.Count == 0)
                {
                    _errorProvider.SetError(rdoFundingSelect, "Please Select at least One Fund");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(rdoFundingSelect, null);
                }
            }

            return (isValid);
        }


        private void btnPdfPreview_Click(object sender, EventArgs e)
        {
            PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, true, propReportPath);
            pdfListForm.StartPosition = FormStartPosition.CenterScreen;
            pdfListForm.ShowDialog();
        }

        private void btnSaveParameters_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {

                ControlCard_Entity Save_Entity = new ControlCard_Entity(true);
                Save_Entity.Scr_Code = Privileges.Program;
                Save_Entity.UserID = BaseForm.UserID;
                Save_Entity.Card_1 = Get_XML_Format_for_Report_Controls();
                Save_Entity.Card_2 = string.Empty;
                Save_Entity.Card_3 = string.Empty;
                Save_Entity.Module = BaseForm.BusinessModuleID;

                Report_Get_SaveParams_Form Save_Form = new Report_Get_SaveParams_Form(Save_Entity, "Save", BaseForm, Privileges);
                Save_Form.StartPosition = FormStartPosition.CenterScreen;
                Save_Form.ShowDialog();
            }
        }

        private void btnGetParameters_Click(object sender, EventArgs e)
        {
            //_errorProvider.SetError(dtpSBCB, null);
            _errorProvider.SetError(rdoMultipleSites, null);
            ControlCard_Entity Save_Entity = new ControlCard_Entity(true);
            Save_Entity.Scr_Code = Privileges.Program;
            Save_Entity.UserID = BaseForm.UserID;
            Save_Entity.Module = BaseForm.BusinessModuleID;
            Report_Get_SaveParams_Form Save_Form = new Report_Get_SaveParams_Form(Save_Entity, "Get");
            Save_Form.FormClosed += new FormClosedEventHandler(Get_Saved_Parameters);
            Save_Form.StartPosition = FormStartPosition.CenterScreen;
            Save_Form.ShowDialog();
        }

        private void Get_Saved_Parameters(object sender, FormClosedEventArgs e)
        {
            Report_Get_SaveParams_Form form = sender as Report_Get_SaveParams_Form;
            string[] Saved_Parameters = new string[2];
            Saved_Parameters[0] = Saved_Parameters[1] = string.Empty;

            if (form.DialogResult == DialogResult.OK)
            {
                DataTable RepCntl_Table = new DataTable();
                Saved_Parameters = form.Get_Adhoc_Saved_Parameters();

                RepCntl_Table = CommonFunctions.Convert_XMLstring_To_Datatable(Saved_Parameters[0]);
                Set_Report_Controls(RepCntl_Table);


            }
        }

        private string Get_XML_Format_for_Report_Controls()
        {
            //string Active = ((Captain.Common.Utilities.ListItem)cmbActive.SelectedItem).Value.ToString();
            //string RepSeq = ((Captain.Common.Utilities.ListItem)CmbRepSeq.SelectedItem).Value.ToString();
            //string EnrlStat = ((Captain.Common.Utilities.ListItem)cmbEnrlStatus.SelectedItem).Value.ToString();
            string strSite = rdoAllSite.Checked == true ? "A" : "M";
            //string strTaskAll = rbAllTasks.Checked == true ? "1" : "2";
            string strReportType = string.Empty;
            string strShowTask = string.Empty;


            if (rbAllmeals.Checked == true)
                strReportType = "A";
            else if (rbFree.Checked == true)
                strReportType = "F";
            else if (rbPaid.Checked == true)
                strReportType = "P";
            else if (rbReduced.Checked == true)
                strReportType = "R";

            string strFundSource = rdoFundingAll.Checked ? "Y" : "N";
            string strsiteRoomNames = string.Empty;
            if (rdoMultipleSites.Checked == true)
            {
                foreach (CaseSiteEntity siteRoom in Sel_REFS_List)
                {
                    if (!strsiteRoomNames.Equals(string.Empty)) strsiteRoomNames += ",";
                    strsiteRoomNames += siteRoom.SiteNUMBER + siteRoom.SiteROOM + siteRoom.SiteAM_PM;
                }
            }

            
            string strFundingCodes = string.Empty;
            if (rdoFundingSelect.Checked == true)
            {
                foreach (CommonEntity FundingCode in SelFundingList)
                {
                    if (!strFundingCodes.Equals(string.Empty)) strFundingCodes += ",";
                    strFundingCodes += FundingCode.Code;
                }
            }



            StringBuilder str = new StringBuilder();
            str.Append("<Rows>");
            str.Append("<Row AGENCY = \"" + Agency + "\" DEPT = \"" + Depart + "\" PROG = \"" + Program +
                            "\" YEAR = \"" + Program_Year + "\" Site = \"" + strSite + "\" MealSeq = \"" + strReportType  +
                             "\" FundedSource = \"" + strFundSource + "\" ShowTask = \"" + strShowTask +
                            "\"  FromDate = \"" + dtFrom.Value.Date + "\" ToDate = \"" + dtTodate.Value.Date + "\" SiteNames = \"" + strsiteRoomNames + "\" FundingCode = \"" + strFundingCodes + "\" />");
            str.Append("</Rows>");

            return str.ToString();
        }

        private void Set_Report_Controls(DataTable Tmp_Table)
        {
            if (Tmp_Table != null && Tmp_Table.Rows.Count > 0)
            {
                DataRow dr = Tmp_Table.Rows[0];

                Set_Report_Hierarchy(dr["AGENCY"].ToString(), dr["DEPT"].ToString(), dr["PROG"].ToString(), dr["YEAR"].ToString());

                
                if (dr["Site"].ToString() == "A")
                    rdoAllSite.Checked = true;
                else
                {
                    rdoMultipleSites.Checked = true;
                    CaseSiteEntity Search_Entity = new CaseSiteEntity(true);
                    Search_Entity.SiteAGENCY = Agency; Search_Entity.SiteDEPT = Depart;
                    Search_Entity.SitePROG = Program; Search_Entity.SiteYEAR = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString().Trim();
                    propCaseSiteEntity = _model.CaseMstData.Browse_CASESITE(Search_Entity, "Browse");
                    propCaseSiteEntity = propCaseSiteEntity.FindAll(u => u.SiteROOM.Trim() != "0000");
                    Sel_REFS_List.Clear();
                    
                    string strSites = dr["SiteNames"].ToString();
                    List<string> siteList = new List<string>();
                    if (strSites != null)
                    {
                        string[] sitesRooms = strSites.Split(',');
                        for (int i = 0; i < sitesRooms.Length; i++)
                        {
                            CaseSiteEntity siteDetails = propCaseSiteEntity.Find(u => u.SiteNUMBER + u.SiteROOM + u.SiteAM_PM == sitesRooms.GetValue(i).ToString());
                            Sel_REFS_List.Add(siteDetails);
                        }
                    }

                }
                Sel_REFS_List = Sel_REFS_List;


                if (dr["MealSeq"].ToString().Trim() == "A")
                    rbAllmeals.Checked = true;
                else if (dr["MealSeq"].ToString().Trim() == "F")
                    rbFree.Checked = true;
                else if (dr["MealSeq"].ToString().Trim() == "R")
                    rbReduced.Checked = true;
                else if (dr["MealSeq"].ToString().Trim() == "P")
                    rbPaid.Checked = true;


                //if (dr["FundedDays"].ToString() == "Y")
                //    rdoAllFundYes.Checked = true;
                //else
                //    rdoAllFundNo.Checked = true;
                if (dr["FundedSource"].ToString() == "Y")
                    rdoFundingAll.Checked = true;
                else
                {
                    rdoFundingSelect.Checked = true;
                    SelFundingList.Clear();
                    string strFunds = dr["FundingCode"].ToString();
                    List<string> siteList = new List<string>();
                    if (strFunds != null)
                    {
                        string[] FundCodes = strFunds.Split(',');
                        for (int i = 0; i < FundCodes.Length; i++)
                        {
                            CommonEntity fundDetails = propfundingSource.Find(u => u.Code == FundCodes.GetValue(i).ToString());
                            SelFundingList.Add(fundDetails);
                        }
                    }
                    SelFundingList = SelFundingList;
                }

                
                dtFrom.Value = Convert.ToDateTime(dr["FromDate"]);
                dtFrom.Checked = true;
                dtTodate.Value = Convert.ToDateTime(dr["ToDate"]);
                dtTodate.Checked = true;

            }
        }


        private void Pb_Search_Hie_Click(object sender, EventArgs e)
        {
           /* HierarchieSelectionFormNew hierarchieSelectionForm = new HierarchieSelectionFormNew(BaseForm, Current_Hierarchy_DB, "Master", "", "D", "Reports");
            hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
            hierarchieSelectionForm.ShowDialog();*/

            HierarchieSelection hierarchieSelectionForm = new HierarchieSelection(BaseForm, Current_Hierarchy_DB, "Master", "", "D", "Reports", BaseForm.UserID);
            hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
            hierarchieSelectionForm.StartPosition = FormStartPosition.CenterScreen;
            hierarchieSelectionForm.ShowDialog();
        }

        string Current_Hierarchy = "******", Current_Hierarchy_DB = "**-**-**";
        private void OnHierarchieFormClosed(object sender, FormClosedEventArgs e)
        {
            HierarchieSelection form = sender as HierarchieSelection;
            if (form.DialogResult == DialogResult.OK)
            {
                List<HierarchyEntity> selectedHierarchies = form.SelectedHierarchies;
                string hierarchy = string.Empty;

                if (selectedHierarchies.Count > 0)
                {
                    foreach (HierarchyEntity row in selectedHierarchies)
                    {
                        hierarchy += (string.IsNullOrEmpty(row.Agency) ? "**" : row.Agency) + (string.IsNullOrEmpty(row.Dept) ? "**" : row.Dept) + (string.IsNullOrEmpty(row.Prog) ? "**" : row.Prog);
                    }
                    //Current_Hierarchy = hierarchy.Substring(0, 2) + "-" + hierarchy.Substring(2, 2) + "-" + hierarchy.Substring(4, 2);

                    Set_Report_Hierarchy(hierarchy.Substring(0, 2), hierarchy.Substring(2, 2), hierarchy.Substring(4, 2), string.Empty);
                    
                }
                //if (hierarchy == "******")
                //    rdoMultipleSites.Enabled = false;
                //else rdoMultipleSites.Enabled = true;
            }
        }

        PdfContentByte cb;
        int X_Pos, Y_Pos;
        string strFolderPath = string.Empty;
        string Random_Filename = null; string PdfName = "Pdf File";

        private void rdoAllSite_CheckedChanged(object sender, EventArgs e)
        {
            Sel_REFS_List.Clear();
            _errorProvider.SetError(rdoMultipleSites, null);
        }

        private void rdoFundingAll_CheckedChanged(object sender, EventArgs e)
        {
            SelFundingList.Clear();
            _errorProvider.SetError(rdoFundingSelect, null);
        }

        private void HSSB2112_Report_ToolClick(object sender, ToolClickEventArgs e)
        {
            //Application.Navigate(CommonFunctions.BuildHelpURLS(Privileges.Program, 0, BaseForm.BusinessModuleID.ToString()), target: "_blank");
            Application.Navigate(CommonFunctions.CreateZenHelps(Privileges.Program, 0, BaseForm.BusinessModuleID.ToString(), "", ""), target: "_blank");
        }

        BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
        DateTime end = DateTime.Now.Date;
        private void On_SaveFormClosed(object sender, FormClosedEventArgs e)
        {
            PdfListForm form = sender as PdfListForm;
            if (form.DialogResult == DialogResult.OK)
            {
                string PdfName = "Pdf File";
                PdfName = form.GetFileName();

                PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
                try
                {
                    if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
                    { DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim()); }
                }
                catch (Exception ex)
                {
                    AlertBox.Show("Error", MessageBoxIcon.Error);
                }


                try
                {
                    string Tmpstr = PdfName + ".pdf";
                    if (File.Exists(Tmpstr))
                        File.Delete(Tmpstr);
                }
                catch (Exception ex)
                {
                    int length = 8;
                    string newFileName = System.Guid.NewGuid().ToString();
                    newFileName = newFileName.Replace("-", string.Empty);

                    Random_Filename = PdfName + newFileName.Substring(0, length) + ".pdf";
                }


                if (!string.IsNullOrEmpty(Random_Filename))
                    PdfName = Random_Filename;
                else
                    PdfName += ".pdf";

                FileStream fs = new FileStream(PdfName, FileMode.Create);

                //Document document = new Document();
                Document document = new Document(PageSize.A4, 30f, 30f, 30f, 50f);
                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                document.Open();
                cb = writer.DirectContent;

                BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
                iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 10);
                iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 8);
                iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 9, 3);
                iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_times, 9, 1);
                iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 8, 2);
                iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 9, 4);

                string Year_value=string.Empty;
                if (CmbYear.Visible == true)
                    Year_value = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString().Trim();
                else Year_value = null;

                string Meal="A"; if(rbFree.Checked==true) Meal="F";else if(rbPaid.Checked==true) Meal="P"; else if(rbReduced.Checked==true) Meal="R";

                List<HSSB2112ReportEntity> Report_list = new List<HSSB2112ReportEntity>();
                List<HSSB2112ReportEntity> App_list = new List<HSSB2112ReportEntity>();

                DataSet ds = DatabaseLayer.ChldAttnDB.GetMealsReport_HSSB2112(Agency, Depart, Program, Year_value, strsiteRoomNames, strFundingCodes, dtFrom.Text.Trim(), dtTodate.Text.Trim(), Meal);
                if (ds != null)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        Report_list.Add(new HSSB2112ReportEntity(row));
                    }
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        App_list.Add(new HSSB2112ReportEntity(dr,string.Empty));
                    }
                }
                //List<HSSB2112ReportEntity> Report_list = _model.ChldAttnData.GetReportHSSB2112(Agency, Depart, Program, Year_value, strsiteRoomNames, strFundingCodes, dtForm.Text.Trim(), dtTodate.Text.Trim(), Meal);

                ProgramDefinitionEntity Program_Entity = _model.HierarchyAndPrograms.GetCaseDepadp(Agency, Depart, Program);

                try
                {
                    PrintHeaderPage(document, writer);
                    document.NewPage();

                    PdfPTable table = new PdfPTable(18);
                    table.TotalWidth = 560f;
                    table.WidthPercentage = 100;
                    table.LockedWidth = true;
                    float[] widths = new float[] { 15f, 15f, 15f, 12f, 12f, 12f, 12f, 12f, 12f, 12f, 12f, 12f, 12f, 12f, 12f, 12f, 12f, 15f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                    table.SetWidths(widths);
                    table.HorizontalAlignment = Element.ALIGN_CENTER;

                    if (Report_list.Count > 0)
                    {
                        string[] Header = { "SITE", "CLASS", "AM/PM", "BRK", "AM", "LUN", "PM", "SUP", "BRK", "AM", "LUN", "PM", "SUP", "BRK", "AM", "LUN", "PM", "SUP" };
                        for (int i = 0; i < Header.Length; ++i)
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(Header[i], TblFontBold));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            //cell.FixedHeight = 15f;
                            cell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                            table.AddCell(cell);
                        }
                        table.HeaderRows = 2;

                        PdfPCell Space = new PdfPCell(new Phrase("", TblFontBold));
                        Space.HorizontalAlignment = Element.ALIGN_CENTER;
                        Space.Colspan = 18;
                        Space.FixedHeight = 15f;
                        Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        table.AddCell(Space);
                        int cnt = 0;
                        string PrivSite = string.Empty; string privSiteRoomFree = string.Empty; string privSiteRoomR = string.Empty; string privSiteRoomP = string.Empty;
                        int FBr = 0, FAM = 0, FL = 0, FPM = 0, FS = 0; int RBr = 0, RAM = 0, RL = 0, RPM = 0, RS = 0; int PBr = 0, PAM = 0, PL = 0, PPM = 0, PS = 0;
                        decimal FBr_tot = 0, FAM_tot = 0, FL_tot = 0, FPM_tot = 0, FS_tot = 0; decimal RBr_tot = 0, RAM_tot = 0, RL_tot = 0, RPM_tot = 0, RS_tot = 0; decimal PBr_tot = 0, PAM_tot = 0, PL_tot = 0, PPM_tot = 0, PS_tot = 0;
                        decimal Tot_FBr = 0, Tot_FAM = 0, Tot_FL = 0, Tot_FPM = 0, Tot_FS = 0; decimal Tot_RBr = 0, Tot_RAM = 0, Tot_RL = 0, Tot_RPM = 0, Tot_RS = 0; decimal Tot_PBr = 0, Tot_PAM = 0, Tot_PL = 0, Tot_PPM = 0, Tot_PS = 0;
                        decimal Tot_FCounts = 0; decimal Tot_RCounts = 0; decimal Tot_PCounts = 0;
                        foreach (HSSB2112ReportEntity Entity in Report_list)
                        {
                            cnt++;
                            if (Entity.Col_type.Trim() == "FREE")//Entity.SITE.Trim() + Entity.ROOM.Trim() + Entity.AMPM.Trim() != privSiteRoomFree &&
                            {
                                PdfPCell site = new PdfPCell(new Phrase(Entity.SITE.Trim(), TableFont));
                                site.HorizontalAlignment = Element.ALIGN_CENTER;
                                site.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(site);

                                PdfPCell Room = new PdfPCell(new Phrase(Entity.ROOM.Trim(), TableFont));
                                Room.HorizontalAlignment = Element.ALIGN_CENTER;
                                Room.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(Room);

                                PdfPCell AmPm = new PdfPCell(new Phrase(Entity.AMPM.Trim(), TableFont));
                                AmPm.HorizontalAlignment = Element.ALIGN_CENTER;
                                AmPm.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(AmPm);

                                if (Entity.Breakfast.Trim() != "0")
                                {
                                    string Break = Convert.ToInt32(Entity.Breakfast.Trim()).ToString("N", CultureInfo.InvariantCulture);
                                    string SBreakfast = Break.Substring(0, (Break.Length - 3));
                                    PdfPCell FBrk = new PdfPCell(new Phrase(SBreakfast, TableFont));
                                    FBrk.HorizontalAlignment = Element.ALIGN_CENTER;
                                    FBrk.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(FBrk);
                                    FBr = FBr + int.Parse(Entity.Breakfast.Trim());
                                }
                                else
                                {
                                    PdfPCell FBrk = new PdfPCell(new Phrase("", TableFont));
                                    FBrk.HorizontalAlignment = Element.ALIGN_CENTER;
                                    FBrk.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(FBrk);
                                }

                                if (Entity.AM_Snacks.Trim() != "0")
                                {
                                    string AM_Snacks = Convert.ToInt32(Entity.AM_Snacks.Trim()).ToString("N", CultureInfo.InvariantCulture);
                                    string SAM_Snacks = AM_Snacks.Substring(0, (AM_Snacks.Length - 3));
                                    PdfPCell FAM1 = new PdfPCell(new Phrase(SAM_Snacks, TableFont));
                                    FAM1.HorizontalAlignment = Element.ALIGN_CENTER;
                                    FAM1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(FAM1);
                                    FAM = FAM + int.Parse(Entity.AM_Snacks.Trim());
                                }
                                else
                                {
                                    PdfPCell FAM1 = new PdfPCell(new Phrase("", TableFont));
                                    FAM1.HorizontalAlignment = Element.ALIGN_CENTER;
                                    FAM1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(FAM1);
                                }

                                if (Entity.Lunch.Trim() != "0")
                                {
                                    string Lunch = Convert.ToInt32(Entity.Lunch.Trim()).ToString("N", CultureInfo.InvariantCulture);
                                    string SLunch = Lunch.Substring(0, (Lunch.Length - 3));
                                    PdfPCell FLun = new PdfPCell(new Phrase(SLunch, TableFont));
                                    FLun.HorizontalAlignment = Element.ALIGN_CENTER;
                                    FLun.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(FLun);
                                    FL = FL + int.Parse(Entity.Lunch.Trim());
                                }
                                else
                                {
                                    PdfPCell FLun = new PdfPCell(new Phrase("", TableFont));
                                    FLun.HorizontalAlignment = Element.ALIGN_CENTER;
                                    FLun.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(FLun);
                                }

                                if (Entity.PM_Snacks.Trim() != "0")
                                {
                                    string PM_Snacks = Convert.ToInt32(Entity.PM_Snacks.Trim()).ToString("N", CultureInfo.InvariantCulture);
                                    string SPM_Snacks = PM_Snacks.Substring(0, (PM_Snacks.Length - 3));
                                    PdfPCell FPM1 = new PdfPCell(new Phrase(SPM_Snacks, TableFont));
                                    FPM1.HorizontalAlignment = Element.ALIGN_CENTER;
                                    FPM1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(FPM1);
                                    FPM = FPM + int.Parse(Entity.PM_Snacks.Trim());
                                }
                                else
                                {
                                    PdfPCell FPM1 = new PdfPCell(new Phrase("", TableFont));
                                    FPM1.HorizontalAlignment = Element.ALIGN_CENTER;
                                    FPM1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(FPM1);
                                }

                                if (Entity.Supper.Trim() != "0")
                                {
                                    string Supper = Convert.ToInt32(Entity.Supper.Trim()).ToString("N", CultureInfo.InvariantCulture);
                                    string SSupper = Supper.Substring(0, (Supper.Length - 3));
                                    PdfPCell Fsup = new PdfPCell(new Phrase(SSupper, TableFont));
                                    Fsup.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Fsup.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(Fsup);
                                    FS = FS + int.Parse(Entity.Supper.Trim());
                                }
                                else
                                {
                                    PdfPCell Fsup = new PdfPCell(new Phrase("", TableFont));
                                    Fsup.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Fsup.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(Fsup);
                                }


                                if (rbFree.Checked == true)
                                {
                                    PdfPCell BrkSpace = new PdfPCell(new Phrase("", TableFont));
                                    BrkSpace.HorizontalAlignment = Element.ALIGN_CENTER;
                                    BrkSpace.Colspan = 10;
                                    BrkSpace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(BrkSpace);

                                }
                            }
                            else if (Entity.Col_type.Trim() == "REDUCED") //Entity.SITE.Trim() + Entity.ROOM.Trim() + Entity.AMPM.Trim() != privSiteRoomFree &&
                            {
                                if (rbReduced.Checked == true)
                                {
                                    PdfPCell site = new PdfPCell(new Phrase(Entity.SITE.Trim(), TableFont));
                                    site.HorizontalAlignment = Element.ALIGN_CENTER;
                                    site.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(site);

                                    PdfPCell Room = new PdfPCell(new Phrase(Entity.ROOM.Trim(), TableFont));
                                    Room.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Room.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(Room);

                                    PdfPCell AmPm = new PdfPCell(new Phrase(Entity.AMPM.Trim(), TableFont));
                                    AmPm.HorizontalAlignment = Element.ALIGN_CENTER;
                                    AmPm.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(AmPm);

                                    PdfPCell BrkSpace = new PdfPCell(new Phrase("", TableFont));
                                    BrkSpace.HorizontalAlignment = Element.ALIGN_CENTER;
                                    BrkSpace.Colspan = 5;
                                    BrkSpace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(BrkSpace);

                                }
                                if (Entity.Breakfast.Trim() != "0")
                                {
                                    string Break = Convert.ToInt32(Entity.Breakfast.Trim()).ToString("N", CultureInfo.InvariantCulture);
                                    string SBreakfast = Break.Substring(0, (Break.Length - 3));
                                    PdfPCell RBrk = new PdfPCell(new Phrase(SBreakfast, TableFont));
                                    RBrk.HorizontalAlignment = Element.ALIGN_CENTER;
                                    RBrk.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(RBrk);
                                    RBr = RBr + int.Parse(Entity.Breakfast.Trim());
                                }
                                else
                                {
                                    PdfPCell RBrk = new PdfPCell(new Phrase("", TableFont));
                                    RBrk.HorizontalAlignment = Element.ALIGN_CENTER;
                                    RBrk.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(RBrk);
                                }

                                if (Entity.AM_Snacks.Trim() != "0")
                                {
                                    string AM_Snacks = Convert.ToInt32(Entity.AM_Snacks.Trim()).ToString("N", CultureInfo.InvariantCulture);
                                    string SAM_Snacks = AM_Snacks.Substring(0, (AM_Snacks.Length - 3));
                                    PdfPCell RAM1 = new PdfPCell(new Phrase(SAM_Snacks, TableFont));
                                    RAM1.HorizontalAlignment = Element.ALIGN_CENTER;
                                    RAM1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(RAM1);
                                    RAM = RAM + int.Parse(Entity.AM_Snacks.Trim());
                                }
                                else
                                {
                                    PdfPCell RAM1 = new PdfPCell(new Phrase("", TableFont));
                                    RAM1.HorizontalAlignment = Element.ALIGN_CENTER;
                                    RAM1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(RAM1);
                                }

                                if (Entity.Lunch.Trim() != "0")
                                {
                                    string Lunch = Convert.ToInt32(Entity.Lunch.Trim()).ToString("N", CultureInfo.InvariantCulture);
                                    string SLunch = Lunch.Substring(0, (Lunch.Length - 3));
                                    PdfPCell RLun = new PdfPCell(new Phrase(SLunch, TableFont));
                                    RLun.HorizontalAlignment = Element.ALIGN_CENTER;
                                    RLun.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(RLun);
                                    RL = RL + int.Parse(Entity.Lunch.Trim());
                                }
                                else
                                {
                                    PdfPCell RLun = new PdfPCell(new Phrase("", TableFont));
                                    RLun.HorizontalAlignment = Element.ALIGN_CENTER;
                                    RLun.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(RLun);
                                }

                                if (Entity.PM_Snacks.Trim() != "0")
                                {
                                    string PM_Snacks = Convert.ToInt32(Entity.PM_Snacks.Trim()).ToString("N", CultureInfo.InvariantCulture);
                                    string SPM_Snacks = PM_Snacks.Substring(0, (PM_Snacks.Length - 3));
                                    PdfPCell RPM1 = new PdfPCell(new Phrase(SPM_Snacks, TableFont));
                                    RPM1.HorizontalAlignment = Element.ALIGN_CENTER;
                                    RPM1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(RPM1);
                                    RPM = RPM + int.Parse(Entity.PM_Snacks.Trim());
                                }
                                else
                                {
                                    PdfPCell RPM1 = new PdfPCell(new Phrase("", TableFont));
                                    RPM1.HorizontalAlignment = Element.ALIGN_CENTER;
                                    RPM1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(RPM1);
                                }

                                if (Entity.Supper.Trim() != "0")
                                {
                                    string Supper = Convert.ToInt32(Entity.Supper.Trim()).ToString("N", CultureInfo.InvariantCulture);
                                    string SSupper = Supper.Substring(0, (Supper.Length - 3));
                                    PdfPCell Rsup = new PdfPCell(new Phrase(Entity.Supper.Trim(), TableFont));
                                    Rsup.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Rsup.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(Rsup);
                                    RS = RS + int.Parse(Entity.Supper.Trim());
                                }
                                else
                                {
                                    PdfPCell Rsup = new PdfPCell(new Phrase("", TableFont));
                                    Rsup.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Rsup.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(Rsup);
                                }

                                if (rbReduced.Checked == true)
                                {
                                    PdfPCell BrkSpace1 = new PdfPCell(new Phrase("", TableFont));
                                    BrkSpace1.HorizontalAlignment = Element.ALIGN_CENTER;
                                    BrkSpace1.Colspan = 5;
                                    BrkSpace1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(BrkSpace1);

                                }
                            }
                            else if (Entity.Col_type.Trim() == "PAID") //Entity.SITE.Trim() + Entity.ROOM.Trim() + Entity.AMPM.Trim() != privSiteRoomFree &&
                            {
                                if (rbPaid.Checked == true)
                                {
                                    PdfPCell site = new PdfPCell(new Phrase(Entity.SITE.Trim(), TableFont));
                                    site.HorizontalAlignment = Element.ALIGN_CENTER;
                                    site.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(site);

                                    PdfPCell Room = new PdfPCell(new Phrase(Entity.ROOM.Trim(), TableFont));
                                    Room.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Room.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(Room);

                                    PdfPCell AmPm = new PdfPCell(new Phrase(Entity.AMPM.Trim(), TableFont));
                                    AmPm.HorizontalAlignment = Element.ALIGN_CENTER;
                                    AmPm.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(AmPm);
                                }
                                if (Entity.Breakfast.Trim() != "0")
                                {
                                    string Breakfast = Convert.ToInt32(Entity.Breakfast.Trim()).ToString("N", CultureInfo.InvariantCulture);
                                    string SBreakfast = Breakfast.Substring(0, (Breakfast.Length - 3));
                                    PdfPCell PBrk = new PdfPCell(new Phrase(SBreakfast, TableFont));
                                    PBrk.HorizontalAlignment = Element.ALIGN_CENTER;
                                    PBrk.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(PBrk);
                                    PBr = PBr + int.Parse(Entity.Breakfast.Trim());
                                }
                                else
                                {
                                    PdfPCell PBrk = new PdfPCell(new Phrase("", TableFont));
                                    PBrk.HorizontalAlignment = Element.ALIGN_CENTER;
                                    PBrk.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(PBrk);
                                }

                                if (Entity.AM_Snacks.Trim() != "0")
                                {
                                    string AM_Snacks = Convert.ToInt32(Entity.AM_Snacks.Trim()).ToString("N", CultureInfo.InvariantCulture);
                                    string SAM_Snacks = AM_Snacks.Substring(0, (AM_Snacks.Length - 3));
                                    PdfPCell PAM1 = new PdfPCell(new Phrase(SAM_Snacks, TableFont));
                                    PAM1.HorizontalAlignment = Element.ALIGN_CENTER;
                                    PAM1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(PAM1);
                                    PAM = PAM + int.Parse(Entity.AM_Snacks.Trim());
                                }
                                else
                                {
                                    PdfPCell PAM1 = new PdfPCell(new Phrase("", TableFont));
                                    PAM1.HorizontalAlignment = Element.ALIGN_CENTER;
                                    PAM1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(PAM1);
                                }

                                if (Entity.Lunch.Trim() != "0")
                                {
                                    string Lunch = Convert.ToInt32(Entity.Lunch.Trim()).ToString("N", CultureInfo.InvariantCulture);
                                    string SLunch = Lunch.Substring(0, (Lunch.Length - 3));
                                    PdfPCell PLun = new PdfPCell(new Phrase(SLunch, TableFont));
                                    PLun.HorizontalAlignment = Element.ALIGN_CENTER;
                                    PLun.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(PLun);
                                    PL = PL + int.Parse(Entity.Lunch.Trim());
                                }
                                else
                                {
                                    PdfPCell PLun = new PdfPCell(new Phrase("", TableFont));
                                    PLun.HorizontalAlignment = Element.ALIGN_CENTER;
                                    PLun.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(PLun);
                                }

                                if (Entity.PM_Snacks.Trim() != "0")
                                {
                                    string PM_Snacks = Convert.ToInt32(Entity.PM_Snacks.Trim()).ToString("N", CultureInfo.InvariantCulture);
                                    string SPM_Snacks = PM_Snacks.Substring(0, (PM_Snacks.Length - 3));
                                    PdfPCell PPM1 = new PdfPCell(new Phrase(SPM_Snacks, TableFont));
                                    PPM1.HorizontalAlignment = Element.ALIGN_CENTER;
                                    PPM1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(PPM1);
                                    PPM = PPM + int.Parse(Entity.PM_Snacks.Trim());
                                }
                                else
                                {
                                    PdfPCell PPM1 = new PdfPCell(new Phrase("", TableFont));
                                    PPM1.HorizontalAlignment = Element.ALIGN_CENTER;
                                    PPM1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(PPM1);
                                }

                                if (Entity.Supper.Trim() != "0")
                                {
                                    string Supper = Convert.ToInt32(Entity.Supper.Trim()).ToString("N", CultureInfo.InvariantCulture);
                                    string SSupper = Supper.Substring(0, (Supper.Length - 3));
                                    PdfPCell Psup = new PdfPCell(new Phrase(SSupper, TableFont));
                                    Psup.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Psup.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(Psup);
                                    PS = PS + int.Parse(Entity.Supper.Trim());
                                }
                                else
                                {
                                    PdfPCell Psup = new PdfPCell(new Phrase("", TableFont));
                                    Psup.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Psup.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(Psup);
                                }

                                if (rbPaid.Checked == true)
                                {
                                    PdfPCell BrkSpace = new PdfPCell(new Phrase("", TableFont));
                                    BrkSpace.HorizontalAlignment = Element.ALIGN_CENTER;
                                    BrkSpace.Colspan = 10;
                                    BrkSpace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(BrkSpace);

                                }
                            }
                            else if (Entity.Col_type.Trim() == "COMPLETED")
                            {

                                PdfPCell sapce1_emoty = new PdfPCell(new Phrase("", TableFont));
                                sapce1_emoty.HorizontalAlignment = Element.ALIGN_CENTER;
                                sapce1_emoty.Colspan = 18;
                                sapce1_emoty.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(sapce1_emoty);

                                PdfPCell sapce1 = new PdfPCell(new Phrase("", TableFont));
                                sapce1.HorizontalAlignment = Element.ALIGN_CENTER;
                                sapce1.Colspan = 3;
                                sapce1.Border = iTextSharp.text.Rectangle.BOX;
                                table.AddCell(sapce1);

                                string[] SubHeader = { " F R E E ", " R E D U C E D ", " P A I D " };
                                for (int i = 0; i < SubHeader.Length; ++i)
                                {
                                    PdfPCell SubHeader1 = new PdfPCell(new Phrase(SubHeader[i], TableFont));
                                    SubHeader1.HorizontalAlignment = Element.ALIGN_CENTER;
                                    SubHeader1.Colspan = 5;
                                    SubHeader1.Border = iTextSharp.text.Rectangle.BOX;
                                    table.AddCell(SubHeader1);
                                }
                                //PdfPCell space2 = new PdfPCell(new Phrase(" F R E E ", TableFont));
                                //space2.HorizontalAlignment = Element.ALIGN_CENTER;
                                //space2.Colspan = 5;
                                //space2.Border = iTextSharp.text.Rectangle.BOX;
                                //table.AddCell(space2);

                                //PdfPCell space3 = new PdfPCell(new Phrase(" R E D U C E D ", TableFont));
                                //space3.HorizontalAlignment = Element.ALIGN_CENTER;
                                //space3.Colspan = 5;
                                //space3.Border = iTextSharp.text.Rectangle.BOX;
                                //table.AddCell(space3);

                                //PdfPCell space4 = new PdfPCell(new Phrase(" P A I D ", TableFont));
                                //space4.HorizontalAlignment = Element.ALIGN_CENTER;
                                //space4.Colspan = 5;
                                //space4.Border = iTextSharp.text.Rectangle.BOX;
                                //table.AddCell(space4);

                                PdfPCell data1 = new PdfPCell(new Phrase("Breakfasts", TableFont));
                                data1.HorizontalAlignment = Element.ALIGN_LEFT;
                                data1.Colspan = 3;
                                data1.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                table.AddCell(data1);
                                Tot_FBr = Tot_FBr + FBr; Tot_FAM = Tot_FAM + FAM; Tot_FL = Tot_FL + FL; Tot_FPM = Tot_FPM + FPM; Tot_FS = Tot_FS + FS;
                                Tot_RBr = Tot_RBr + RBr; Tot_RAM = Tot_RAM + RAM; Tot_RL = Tot_RL + RL; Tot_RPM = Tot_RPM + RPM; Tot_RS = Tot_RS + RS;
                                Tot_PBr = Tot_PBr + PBr; Tot_PAM = Tot_PAM + PAM; Tot_PL = Tot_PL + PL; Tot_PPM = Tot_PPM + PPM; Tot_PS = Tot_PS + PS;
                                if (Program_Entity != null)
                                {
                                    FBr_tot = FBr * decimal.Parse(Program_Entity.Free1.Trim());
                                    FAM_tot = FAM * decimal.Parse(Program_Entity.Free2.Trim());
                                    FL_tot = FL * decimal.Parse(Program_Entity.Free3.Trim());
                                    FPM_tot = FPM * decimal.Parse(Program_Entity.Free4.Trim());
                                    FS_tot = FS * decimal.Parse(Program_Entity.Free5.Trim());

                                    RBr_tot = RBr * decimal.Parse(Program_Entity.Reduced1.Trim());
                                    RAM_tot = RAM * decimal.Parse(Program_Entity.Reduced2.Trim());
                                    RL_tot = RL * decimal.Parse(Program_Entity.Reduced3.Trim());
                                    RPM_tot = RPM * decimal.Parse(Program_Entity.Reduced4.Trim());
                                    RS_tot = RS * decimal.Parse(Program_Entity.Reduced5.Trim());

                                    PBr_tot = PBr * decimal.Parse(Program_Entity.Paid1.Trim());
                                    PAM_tot = PAM * decimal.Parse(Program_Entity.Paid2.Trim());
                                    PL_tot = PL * decimal.Parse(Program_Entity.Paid3.Trim());
                                    PPM_tot = PPM * decimal.Parse(Program_Entity.Paid4.Trim());
                                    PS_tot = PS * decimal.Parse(Program_Entity.Paid5.Trim());
                                }
                                Tot_FCounts = FBr_tot + FAM_tot + FL_tot + FPM_tot + FS_tot;
                                Tot_RCounts = RBr_tot + RAM_tot + RL_tot + RPM_tot + RS_tot;
                                Tot_PCounts = PBr_tot + PAM_tot + PL_tot + PPM_tot + PS_tot;

                                string[] Breakfast = { FBr.ToString(), RBr.ToString(), PBr.ToString() };
                                for (int i = 0; i < Breakfast.Length; ++i)
                                {
                                    


                                    if (Breakfast[i] != "0")
                                    {
                                        string TBreak = Convert.ToInt32(Breakfast[i].ToString().Trim()).ToString("N", CultureInfo.InvariantCulture);
                                        string STBreak = TBreak.Substring(0, (TBreak.Length - 3));
                                        PdfPCell cell = new PdfPCell(new Phrase(STBreak, TableFont));
                                        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        //cell.Colspan = 2;
                                        cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(cell);
                                    }
                                    else
                                    {
                                        PdfPCell cell = new PdfPCell(new Phrase("", TableFont));
                                        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        //cell.Colspan = 2;
                                        cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(cell);
                                    }

                                    if (i == 0)
                                    {
                                        if (Program_Entity != null)
                                        {
                                            PdfPCell Cost = new PdfPCell(new Phrase(decimal.Parse(Program_Entity.Free1.Trim()).ToString(), TableFont));
                                            Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            Cost.Colspan = 2;
                                            Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(Cost);
                                        }
                                        else
                                        {
                                            PdfPCell Cost = new PdfPCell(new Phrase("", TableFont));
                                            Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            Cost.Colspan = 2;
                                            Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(Cost);
                                        }
                                        PdfPCell cell1 = new PdfPCell(new Phrase("$" + decimal.Parse(FBr_tot.ToString()).ToString("N", CultureInfo.InvariantCulture), TableFont));
                                        cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        cell1.Colspan = 2;
                                        cell1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(cell1);

                                    }
                                    else if (i == 1)
                                    {
                                        if (Program_Entity != null)
                                        {
                                            PdfPCell Cost = new PdfPCell(new Phrase(decimal.Parse(Program_Entity.Reduced1.Trim()).ToString(), TableFont));
                                            Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            Cost.Colspan = 2;
                                            Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(Cost);
                                        }
                                        else
                                        {
                                            PdfPCell Cost = new PdfPCell(new Phrase("", TableFont));
                                            Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            Cost.Colspan = 2;
                                            Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(Cost);
                                        }

                                        PdfPCell cell1 = new PdfPCell(new Phrase("$" + decimal.Parse(RBr_tot.ToString()).ToString("N", CultureInfo.InvariantCulture), TableFont));
                                        cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        cell1.Colspan = 2;
                                        cell1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(cell1);
                                    }
                                    else if (i == 2)
                                    {
                                        if (Program_Entity != null)
                                        {
                                            PdfPCell Cost = new PdfPCell(new Phrase(decimal.Parse(Program_Entity.Paid1.Trim()).ToString(), TableFont));
                                            Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            Cost.Colspan = 2;
                                            Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(Cost);
                                        }
                                        else
                                        {
                                            PdfPCell Cost = new PdfPCell(new Phrase("", TableFont));
                                            Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            Cost.Colspan = 2;
                                            Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(Cost);
                                        }

                                        PdfPCell cell1 = new PdfPCell(new Phrase("$" + decimal.Parse(PBr_tot.ToString()).ToString("N", CultureInfo.InvariantCulture), TableFont));
                                        cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        cell1.Colspan = 2;
                                        cell1.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                        table.AddCell(cell1);
                                    }

                                    //PdfPCell Spacecell = new PdfPCell(new Phrase("", TableFont));
                                    //Spacecell.HorizontalAlignment = Element.ALIGN_CENTER;
                                    //if (i == 2)
                                    //    Spacecell.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                    //else
                                    //    Spacecell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //table.AddCell(Spacecell);
                                }

                                //PdfPCell data2 = new PdfPCell(new Phrase(FBr.ToString(), TableFont));
                                //data2.HorizontalAlignment = Element.ALIGN_CENTER;
                                //data2.Colspan = 5;
                                //data2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //table.AddCell(data2);

                                //PdfPCell data3 = new PdfPCell(new Phrase(RBr.ToString(), TableFont));
                                //data3.HorizontalAlignment = Element.ALIGN_CENTER;
                                //data3.Colspan = 5;
                                //data3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //table.AddCell(data3);

                                //PdfPCell data4 = new PdfPCell(new Phrase(PBr.ToString(), TableFont));
                                //data4.HorizontalAlignment = Element.ALIGN_CENTER;
                                //data4.Colspan = 5;
                                //data4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //table.AddCell(data4);

                                PdfPCell data5 = new PdfPCell(new Phrase("AM Snacks", TableFont));
                                data5.HorizontalAlignment = Element.ALIGN_LEFT;
                                data5.Colspan = 3;
                                data5.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                table.AddCell(data5);

                                string[] AM_Snacks = { FAM.ToString(), RAM.ToString(), PAM.ToString() };
                                for (int i = 0; i < AM_Snacks.Length; ++i)
                                {
                                    if (AM_Snacks[i] != "0")
                                    {
                                        string TAM_Snacks = Convert.ToInt32(AM_Snacks[i].ToString().Trim()).ToString("N", CultureInfo.InvariantCulture);
                                        string SAM_Snacks = TAM_Snacks.Substring(0, (TAM_Snacks.Length - 3));
                                        PdfPCell cell = new PdfPCell(new Phrase(SAM_Snacks, TableFont));
                                        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        //cell.Colspan = 2;
                                        cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(cell);
                                    }
                                    else
                                    {
                                        PdfPCell cell = new PdfPCell(new Phrase("", TableFont));
                                        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        //cell.Colspan = 2;
                                        cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(cell);
                                    }
                                    if (i == 0)
                                    {
                                        if (Program_Entity != null)
                                        {
                                            PdfPCell Cost = new PdfPCell(new Phrase(decimal.Parse(Program_Entity.Free2.Trim()).ToString(), TableFont));
                                            Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            Cost.Colspan = 2;
                                            Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(Cost);
                                        }
                                        else
                                        {
                                            PdfPCell Cost = new PdfPCell(new Phrase("", TableFont));
                                            Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            Cost.Colspan = 2;
                                            Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(Cost);
                                        }

                                        PdfPCell cell1 = new PdfPCell(new Phrase("$" + decimal.Parse(FAM_tot.ToString()).ToString("N", CultureInfo.InvariantCulture), TableFont));
                                        cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        cell1.Colspan = 2;
                                        cell1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(cell1);
                                    }
                                    else if (i == 1)
                                    {
                                        if (Program_Entity != null)
                                        {
                                            PdfPCell Cost = new PdfPCell(new Phrase(decimal.Parse(Program_Entity.Reduced2.Trim()).ToString(), TableFont));
                                            Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            Cost.Colspan = 2;
                                            Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(Cost);
                                        }
                                        else
                                        {
                                            PdfPCell Cost = new PdfPCell(new Phrase("", TableFont));
                                            Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            Cost.Colspan = 2;
                                            Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(Cost);
                                        }


                                        PdfPCell cell1 = new PdfPCell(new Phrase("$" + decimal.Parse(RAM_tot.ToString()).ToString("N", CultureInfo.InvariantCulture), TableFont));
                                        cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        cell1.Colspan = 2;
                                        cell1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(cell1);
                                    }
                                    else if (i == 2)
                                    {

                                        if (Program_Entity != null)
                                        {
                                            PdfPCell Cost = new PdfPCell(new Phrase(decimal.Parse(Program_Entity.Paid2.Trim()).ToString(), TableFont));
                                            Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            Cost.Colspan = 2;
                                            Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(Cost);
                                        }
                                        else
                                        {
                                            PdfPCell Cost = new PdfPCell(new Phrase("", TableFont));
                                            Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            Cost.Colspan = 2;
                                            Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(Cost);
                                        }

                                        PdfPCell cell1 = new PdfPCell(new Phrase("$" + decimal.Parse(PAM_tot.ToString()).ToString("N", CultureInfo.InvariantCulture), TableFont));
                                        cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        cell1.Colspan = 2;
                                        cell1.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                        table.AddCell(cell1);
                                    }
                                    //PdfPCell Spacecell = new PdfPCell(new Phrase("", TableFont));
                                    //Spacecell.HorizontalAlignment = Element.ALIGN_CENTER;
                                    //if (i == 2)
                                    //    Spacecell.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                    //else
                                    //    Spacecell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //table.AddCell(Spacecell);
                                }

                                //PdfPCell data6 = new PdfPCell(new Phrase(FAM.ToString(), TableFont));
                                //data6.HorizontalAlignment = Element.ALIGN_CENTER;
                                //data6.Colspan = 5;
                                //data6.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //table.AddCell(data6);

                                //PdfPCell data7 = new PdfPCell(new Phrase(RAM.ToString(), TableFont));
                                //data7.HorizontalAlignment = Element.ALIGN_CENTER;
                                //data7.Colspan = 5;
                                //data7.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //table.AddCell(data7);

                                //PdfPCell data8 = new PdfPCell(new Phrase(PAM.ToString(), TableFont));
                                //data8.HorizontalAlignment = Element.ALIGN_CENTER;
                                //data8.Colspan = 5;
                                //data8.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //table.AddCell(data8);

                                PdfPCell data9 = new PdfPCell(new Phrase("Lunchs", TableFont));
                                data9.HorizontalAlignment = Element.ALIGN_LEFT;
                                data9.Colspan = 3;
                                data9.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                table.AddCell(data9);

                                string[] Lunch = { FL.ToString(), RL.ToString(), PL.ToString() };
                                for (int i = 0; i < Lunch.Length; ++i)
                                {
                                    if (Lunch[i] != "0")
                                    {
                                        string TLunch = Convert.ToInt32(Lunch[i].ToString().Trim()).ToString("N", CultureInfo.InvariantCulture);
                                        string STLunch = TLunch.Substring(0, (TLunch.Length - 3));
                                        PdfPCell cell = new PdfPCell(new Phrase(STLunch, TableFont));
                                        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        //cell.Colspan = 2;
                                        cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(cell);
                                    }
                                    else
                                    {
                                        PdfPCell cell = new PdfPCell(new Phrase("", TableFont));
                                        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        //cell.Colspan = 2;
                                        cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(cell);
                                    }

                                    if (i == 0)
                                    {
                                        if (Program_Entity != null)
                                        {
                                            PdfPCell Cost = new PdfPCell(new Phrase(decimal.Parse(Program_Entity.Free3.Trim()).ToString(), TableFont));
                                            Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            Cost.Colspan = 2;
                                            Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(Cost);
                                        }
                                        else
                                        {
                                            PdfPCell Cost = new PdfPCell(new Phrase("", TableFont));
                                            Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            Cost.Colspan = 2;
                                            Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(Cost);
                                        }

                                        PdfPCell cell1 = new PdfPCell(new Phrase("$" + decimal.Parse(FL_tot.ToString()).ToString("N", CultureInfo.InvariantCulture), TableFont));
                                        cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        cell1.Colspan = 2;
                                        cell1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(cell1);
                                    }
                                    else if (i == 1)
                                    {
                                        if (Program_Entity != null)
                                        {
                                            PdfPCell Cost = new PdfPCell(new Phrase(decimal.Parse(Program_Entity.Reduced3.Trim()).ToString(), TableFont));
                                            Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            Cost.Colspan = 2;
                                            Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(Cost);
                                        }
                                        else
                                        {
                                            PdfPCell Cost = new PdfPCell(new Phrase("", TableFont));
                                            Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            Cost.Colspan = 2;
                                            Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(Cost);
                                        }

                                        PdfPCell cell1 = new PdfPCell(new Phrase("$" + decimal.Parse(RL_tot.ToString()).ToString("N", CultureInfo.InvariantCulture), TableFont));
                                        cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        cell1.Colspan = 2;
                                        cell1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(cell1);
                                    }
                                    else if (i == 2)
                                    {
                                        if (Program_Entity != null)
                                        {
                                            PdfPCell Cost = new PdfPCell(new Phrase(decimal.Parse(Program_Entity.Paid3.Trim()).ToString(), TableFont));
                                            Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            Cost.Colspan = 2;
                                            Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(Cost);
                                        }
                                        else
                                        {
                                            PdfPCell Cost = new PdfPCell(new Phrase("", TableFont));
                                            Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            Cost.Colspan = 2;
                                            Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(Cost);
                                        }

                                        PdfPCell cell1 = new PdfPCell(new Phrase("$" + decimal.Parse(PL_tot.ToString()).ToString("N", CultureInfo.InvariantCulture), TableFont));
                                        cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        cell1.Colspan = 2;
                                        cell1.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                        table.AddCell(cell1);
                                    }
                                    //PdfPCell Spacecell = new PdfPCell(new Phrase("", TableFont));
                                    //Spacecell.HorizontalAlignment = Element.ALIGN_CENTER;
                                    //if (i == 2)
                                    //    Spacecell.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                    //else
                                    //    Spacecell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //table.AddCell(Spacecell);
                                }

                                //PdfPCell data10 = new PdfPCell(new Phrase(FL.ToString(), TableFont));
                                //data10.HorizontalAlignment = Element.ALIGN_CENTER;
                                //data10.Colspan = 5;
                                //data10.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //table.AddCell(data10);

                                //PdfPCell data11 = new PdfPCell(new Phrase(RL.ToString(), TableFont));
                                //data11.HorizontalAlignment = Element.ALIGN_CENTER;
                                //data11.Colspan = 5;
                                //data11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //table.AddCell(data11);

                                //PdfPCell data12 = new PdfPCell(new Phrase(PL.ToString(), TableFont));
                                //data12.HorizontalAlignment = Element.ALIGN_CENTER;
                                //data12.Colspan = 5;
                                //data12.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //table.AddCell(data12);

                                PdfPCell data13 = new PdfPCell(new Phrase("PM Snacks", TableFont));
                                data13.HorizontalAlignment = Element.ALIGN_LEFT;
                                data13.Colspan = 3;
                                data13.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                table.AddCell(data13);

                                string[] PM_Snacks = { FPM.ToString(), RPM.ToString(), PPM.ToString() };
                                for (int i = 0; i < PM_Snacks.Length; ++i)
                                {
                                    if (PM_Snacks[i] != "0")
                                    {
                                        string TPM_Snacks = Convert.ToInt32(PM_Snacks[i].ToString().Trim()).ToString("N", CultureInfo.InvariantCulture);
                                        string STPM_Snacks = TPM_Snacks.Substring(0, (TPM_Snacks.Length - 3));
                                        PdfPCell cell = new PdfPCell(new Phrase(STPM_Snacks, TableFont));
                                        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        //cell.Colspan = 2;
                                        cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(cell);
                                    }
                                    else
                                    {
                                        PdfPCell cell = new PdfPCell(new Phrase("", TableFont));
                                        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        //cell.Colspan = 2;
                                        cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(cell);
                                    }

                                    if (i == 0)
                                    {
                                        if (Program_Entity != null)
                                        {
                                            PdfPCell Cost = new PdfPCell(new Phrase(decimal.Parse(Program_Entity.Free4.Trim()).ToString(), TableFont));
                                            Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            Cost.Colspan = 2;
                                            Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(Cost);
                                        }
                                        else
                                        {
                                            PdfPCell Cost = new PdfPCell(new Phrase("", TableFont));
                                            Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            Cost.Colspan = 2;
                                            Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(Cost);
                                        }

                                        PdfPCell cell1 = new PdfPCell(new Phrase("$" + decimal.Parse(FPM_tot.ToString()).ToString("N", CultureInfo.InvariantCulture), TableFont));
                                        cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        cell1.Colspan = 2;
                                        cell1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(cell1);
                                    }
                                    else if (i == 1)
                                    {
                                        if (Program_Entity != null)
                                        {
                                            PdfPCell Cost = new PdfPCell(new Phrase(decimal.Parse(Program_Entity.Reduced4.Trim()).ToString(), TableFont));
                                            Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            Cost.Colspan = 2;
                                            Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(Cost);
                                        }
                                        else
                                        {
                                            PdfPCell Cost = new PdfPCell(new Phrase("", TableFont));
                                            Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            Cost.Colspan = 2;
                                            Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(Cost);
                                        }

                                        PdfPCell cell1 = new PdfPCell(new Phrase("$" + decimal.Parse(RPM_tot.ToString()).ToString("N", CultureInfo.InvariantCulture), TableFont));
                                        cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        cell1.Colspan = 2;
                                        cell1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(cell1);
                                    }
                                    else if (i == 2)
                                    {
                                        if (Program_Entity != null)
                                        {
                                            PdfPCell Cost = new PdfPCell(new Phrase(decimal.Parse(Program_Entity.Paid4.Trim()).ToString(), TableFont));
                                            Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            Cost.Colspan = 2;
                                            Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(Cost);
                                        }
                                        else
                                        {
                                            PdfPCell Cost = new PdfPCell(new Phrase("", TableFont));
                                            Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            Cost.Colspan = 2;
                                            Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(Cost);
                                        }

                                        PdfPCell cell1 = new PdfPCell(new Phrase("$" + decimal.Parse(PPM_tot.ToString()).ToString("N", CultureInfo.InvariantCulture), TableFont));
                                        cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        cell1.Colspan = 2;
                                        cell1.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                        table.AddCell(cell1);
                                    }
                                    //PdfPCell Spacecell = new PdfPCell(new Phrase("", TableFont));
                                    //Spacecell.HorizontalAlignment = Element.ALIGN_CENTER;
                                    //if (i == 2)
                                    //    Spacecell.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                    //else
                                    //    Spacecell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //table.AddCell(Spacecell);
                                }

                                PdfPCell data14 = new PdfPCell(new Phrase("Suppers", TableFont));
                                data14.HorizontalAlignment = Element.ALIGN_LEFT;
                                data14.Colspan = 3;
                                data14.Border = iTextSharp.text.Rectangle.LEFT_BORDER + iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                table.AddCell(data14);

                                string[] Supper = { FS.ToString(), RS.ToString(), PS.ToString() };
                                for (int i = 0; i < Supper.Length; ++i)
                                {
                                    if (Supper[i] != "0")
                                    {
                                        string TSupper = Convert.ToInt32(Supper[i].ToString().Trim()).ToString("N", CultureInfo.InvariantCulture);
                                        string STSupper = TSupper.Substring(0, (TSupper.Length - 3));
                                        PdfPCell cell = new PdfPCell(new Phrase(STSupper, TableFont));
                                        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        //cell.Colspan = 2;
                                        cell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                        table.AddCell(cell);
                                    }
                                    else
                                    {
                                        PdfPCell cell = new PdfPCell(new Phrase("", TableFont));
                                        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        //cell.Colspan = 2;
                                        cell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                        table.AddCell(cell);
                                    }

                                    if (i == 0)
                                    {
                                        if (Program_Entity != null)
                                        {
                                            PdfPCell Cost = new PdfPCell(new Phrase(decimal.Parse(Program_Entity.Free5.Trim()).ToString(), TableFont));
                                            Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            Cost.Colspan = 2;
                                            Cost.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                            table.AddCell(Cost);
                                        }
                                        else
                                        {
                                            PdfPCell Cost = new PdfPCell(new Phrase("", TableFont));
                                            Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            Cost.Colspan = 2;
                                            Cost.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                            table.AddCell(Cost);
                                        }

                                        PdfPCell cell1 = new PdfPCell(new Phrase("$" + decimal.Parse(FS_tot.ToString()).ToString("N", CultureInfo.InvariantCulture), TableFont));
                                        cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        cell1.Colspan = 2;
                                        cell1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                        table.AddCell(cell1);
                                    }
                                    else if (i == 1)
                                    {
                                        if (Program_Entity != null)
                                        {
                                            PdfPCell Cost = new PdfPCell(new Phrase(decimal.Parse(Program_Entity.Reduced5.Trim()).ToString(), TableFont));
                                            Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            Cost.Colspan = 2;
                                            Cost.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                            table.AddCell(Cost);
                                        }
                                        else
                                        {
                                            PdfPCell Cost = new PdfPCell(new Phrase("", TableFont));
                                            Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            Cost.Colspan = 2;
                                            Cost.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                            table.AddCell(Cost);
                                        }

                                        PdfPCell cell1 = new PdfPCell(new Phrase("$" + decimal.Parse(RS_tot.ToString()).ToString("N", CultureInfo.InvariantCulture), TableFont));
                                        cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        cell1.Colspan = 2;
                                        cell1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                        table.AddCell(cell1);
                                    }
                                    else if (i == 2)
                                    {
                                        if (Program_Entity != null)
                                        {
                                            PdfPCell Cost = new PdfPCell(new Phrase( decimal.Parse(Program_Entity.Paid5.Trim()).ToString(), TableFont));
                                            Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            Cost.Colspan = 2;
                                            Cost.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                            table.AddCell(Cost);
                                        }
                                        else
                                        {
                                            PdfPCell Cost = new PdfPCell(new Phrase("", TableFont));
                                            Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            Cost.Colspan = 2;
                                            Cost.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                            table.AddCell(Cost);
                                        }

                                        PdfPCell cell1 = new PdfPCell(new Phrase("$" + decimal.Parse(PS_tot.ToString()).ToString("N", CultureInfo.InvariantCulture), TableFont));
                                        cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        cell1.Colspan = 2;
                                        cell1.Border = iTextSharp.text.Rectangle.RIGHT_BORDER + iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                        table.AddCell(cell1);
                                    }
                                    //PdfPCell Spacecell = new PdfPCell(new Phrase("", TableFont));
                                    //Spacecell.HorizontalAlignment = Element.ALIGN_CENTER;
                                    //if (i == 2)
                                    //    Spacecell.Border = iTextSharp.text.Rectangle.RIGHT_BORDER + iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                    //else
                                    //    Spacecell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                    //table.AddCell(Spacecell);
                                }

                                PdfPCell totalSpace = new PdfPCell(new Phrase("", TableFont));
                                totalSpace.HorizontalAlignment = Element.ALIGN_RIGHT;
                                totalSpace.Colspan = 6;
                                totalSpace.FixedHeight = 15f;
                                totalSpace.Border = iTextSharp.text.Rectangle.LEFT_BORDER + iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                table.AddCell(totalSpace);

                                PdfPCell total = new PdfPCell(new Phrase("$" + decimal.Parse(Tot_FCounts.ToString()).ToString("N", CultureInfo.InvariantCulture), TableFont));
                                total.HorizontalAlignment = Element.ALIGN_RIGHT;
                                total.FixedHeight = 15f;
                                total.Colspan = 2;
                                total.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER;
                                table.AddCell(total);

                                PdfPCell totalRSpace = new PdfPCell(new Phrase("", TableFont));
                                totalRSpace.HorizontalAlignment = Element.ALIGN_RIGHT;
                                totalRSpace.FixedHeight = 15f;
                                totalRSpace.Colspan = 3;
                                totalRSpace.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER;
                                table.AddCell(totalRSpace);

                                PdfPCell totalR = new PdfPCell(new Phrase("$" + decimal.Parse(Tot_RCounts.ToString()).ToString("N", CultureInfo.InvariantCulture), TableFont));
                                totalR.HorizontalAlignment = Element.ALIGN_RIGHT;
                                totalR.FixedHeight = 15f;
                                totalR.Colspan = 2;
                                totalR.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER;
                                table.AddCell(totalR);

                                PdfPCell totalPSpace = new PdfPCell(new Phrase("", TableFont));
                                totalPSpace.HorizontalAlignment = Element.ALIGN_RIGHT;
                                totalPSpace.FixedHeight = 15f;
                                totalPSpace.Colspan = 3;
                                totalPSpace.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER;
                                table.AddCell(totalPSpace);

                                PdfPCell totalP = new PdfPCell(new Phrase("$" + decimal.Parse(Tot_PCounts.ToString()).ToString("N", CultureInfo.InvariantCulture), TableFont));
                                totalP.HorizontalAlignment = Element.ALIGN_RIGHT;
                                totalP.FixedHeight = 15f;
                                totalP.Colspan = 2;
                                totalP.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER;
                                table.AddCell(totalP);

                                PdfPCell totalT = new PdfPCell(new Phrase("$" + decimal.Parse((Tot_PCounts + Tot_RCounts + Tot_FCounts).ToString()).ToString("N", CultureInfo.InvariantCulture), TableFont));
                                totalT.HorizontalAlignment = Element.ALIGN_RIGHT;
                                totalT.FixedHeight = 15f;
                                totalT.Colspan = 18;
                                totalT.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER + iTextSharp.text.Rectangle.LEFT_BORDER;
                                table.AddCell(totalT);

                                Tot_FCounts = 0; Tot_RCounts = 0; Tot_PCounts = 0;
                                FBr = FAM = FL = FPM = FS = 0; RBr = RAM = RL = RPM = RS = 0; PBr = PAM = PL = PPM = PS = 0;
                                FBr_tot = FAM_tot = FL_tot = FPM_tot = FS_tot = 0; RBr_tot = RAM_tot = RL_tot = RPM_tot = RS_tot = 0; PBr_tot = PAM_tot = PL_tot = PPM_tot = PS_tot = 0;
                                if (table.Rows.Count > 0)
                                {
                                    document.Add(table);
                                    table.DeleteBodyRows();
                                    document.NewPage();
                                }
                            }

                            if (Report_list.Count == cnt)
                            {
                                PdfPCell sapce1_empty = new PdfPCell(new Phrase("", TableFont));
                                sapce1_empty.HorizontalAlignment = Element.ALIGN_CENTER;
                                sapce1_empty.Colspan = 18;
                                sapce1_empty.FixedHeight = 18f;
                                sapce1_empty.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(sapce1_empty);

                                PdfPCell sapce1 = new PdfPCell(new Phrase("", TableFont));
                                sapce1.HorizontalAlignment = Element.ALIGN_CENTER;
                                sapce1.Colspan = 3;
                                sapce1.Border = iTextSharp.text.Rectangle.BOX;
                                table.AddCell(sapce1);

                                string[] SubHeader = { " F R E E ", " R E D U C E D ", " P A I D " };
                                for (int i = 0; i < SubHeader.Length; ++i)
                                {
                                    PdfPCell SubHeader1 = new PdfPCell(new Phrase(SubHeader[i], TableFont));
                                    SubHeader1.HorizontalAlignment = Element.ALIGN_CENTER;
                                    SubHeader1.Colspan = 5;
                                    SubHeader1.Border = iTextSharp.text.Rectangle.BOX;
                                    table.AddCell(SubHeader1);
                                }

                                PdfPCell data1 = new PdfPCell(new Phrase("Breakfasts", TableFont));
                                data1.HorizontalAlignment = Element.ALIGN_LEFT;
                                data1.Colspan = 3;
                                data1.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                table.AddCell(data1);

                                Tot_FBr = Tot_FBr + FBr; Tot_FAM = Tot_FAM + FAM; Tot_FL = Tot_FL + FL; Tot_FPM = Tot_FPM + FPM; Tot_FS = Tot_FS + FS;
                                Tot_RBr = Tot_RBr + RBr; Tot_RAM = Tot_RAM + RAM; Tot_RL = Tot_RL + RL; Tot_RPM = Tot_RPM + RPM; Tot_RS = Tot_RS + RS;
                                Tot_PBr = Tot_PBr + PBr; Tot_PAM = Tot_PAM + PAM; Tot_PL = Tot_PL + PL; Tot_PPM = Tot_PPM + PPM; Tot_PS = Tot_PS + PS;
                                if (Program_Entity != null)
                                {
                                    FBr_tot = FBr * decimal.Parse(Program_Entity.Free1.Trim());
                                    FAM_tot = FAM * decimal.Parse(Program_Entity.Free2.Trim());
                                    FL_tot = FL * decimal.Parse(Program_Entity.Free3.Trim());
                                    FPM_tot = FPM * decimal.Parse(Program_Entity.Free4.Trim());
                                    FS_tot = FS * decimal.Parse(Program_Entity.Free5.Trim());

                                    RBr_tot = RBr * decimal.Parse(Program_Entity.Reduced1.Trim());
                                    RAM_tot = RAM * decimal.Parse(Program_Entity.Reduced2.Trim());
                                    RL_tot = RL * decimal.Parse(Program_Entity.Reduced3.Trim());
                                    RPM_tot = RPM * decimal.Parse(Program_Entity.Reduced4.Trim());
                                    RS_tot = RS * decimal.Parse(Program_Entity.Reduced5.Trim());

                                    PBr_tot = PBr * decimal.Parse(Program_Entity.Paid1.Trim());
                                    PAM_tot = PAM * decimal.Parse(Program_Entity.Paid2.Trim());
                                    PL_tot = PL * decimal.Parse(Program_Entity.Paid3.Trim());
                                    PPM_tot = PPM * decimal.Parse(Program_Entity.Paid4.Trim());
                                    PS_tot = PS * decimal.Parse(Program_Entity.Paid5.Trim());
                                }
                                Tot_FCounts = FBr_tot + FAM_tot + FL_tot + FPM_tot + FS_tot;
                                Tot_RCounts = RBr_tot + RAM_tot + RL_tot + RPM_tot + RS_tot;
                                Tot_PCounts = PBr_tot + PAM_tot + PL_tot + PPM_tot + PS_tot;

                                string[] Breakfast = { FBr.ToString(), RBr.ToString(), PBr.ToString() };
                                for (int i = 0; i < Breakfast.Length; ++i)
                                {
                                    if (Breakfast[i] != "0")
                                    {
                                        string STALL = Convert.ToInt32(Breakfast[i].ToString().Trim()).ToString("N", CultureInfo.InvariantCulture);
                                        string STPrint = STALL.Substring(0, (STALL.Length - 3));
                                        PdfPCell cell = new PdfPCell(new Phrase(STPrint, TableFont));
                                        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        //cell.Colspan = 2;
                                        cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(cell);
                                    }
                                    else
                                    {
                                        PdfPCell cell = new PdfPCell(new Phrase("", TableFont));
                                        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        //cell.Colspan = 2;
                                        cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(cell);
                                    }

                                    if (i == 0)
                                    {
                                        if (Program_Entity != null)
                                        {
                                            PdfPCell Cost = new PdfPCell(new Phrase( decimal.Parse(Program_Entity.Free1.Trim()).ToString(), TableFont));
                                            Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            Cost.Colspan = 2;
                                            Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(Cost);
                                        }
                                        else
                                        {
                                            PdfPCell Cost = new PdfPCell(new Phrase("", TableFont));
                                            Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            Cost.Colspan = 2;
                                            Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(Cost);
                                        }

                                        PdfPCell cell1 = new PdfPCell(new Phrase("$" + decimal.Parse(FBr_tot.ToString()).ToString("N", CultureInfo.InvariantCulture), TableFont));
                                        cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        cell1.Colspan = 2;
                                        cell1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(cell1);

                                    }
                                    else if (i == 1)
                                    {
                                        if (Program_Entity != null)
                                        {
                                            PdfPCell Cost = new PdfPCell(new Phrase( decimal.Parse(Program_Entity.Reduced1.Trim()).ToString(), TableFont));
                                            Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            Cost.Colspan = 2;
                                            Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(Cost);
                                        }
                                        else
                                        {
                                            PdfPCell Cost = new PdfPCell(new Phrase("", TableFont));
                                            Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            Cost.Colspan = 2;
                                            Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(Cost);
                                        }

                                        PdfPCell cell1 = new PdfPCell(new Phrase("$" + decimal.Parse(RBr_tot.ToString()).ToString("N", CultureInfo.InvariantCulture), TableFont));
                                        cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        cell1.Colspan = 2;
                                        cell1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(cell1);
                                    }
                                    else if (i == 2)
                                    {
                                        if (Program_Entity != null)
                                        {
                                            PdfPCell Cost = new PdfPCell(new Phrase(decimal.Parse(Program_Entity.Paid1.Trim()).ToString(), TableFont));
                                            Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            Cost.Colspan = 2;
                                            Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(Cost);
                                        }
                                        else
                                        {
                                            PdfPCell Cost = new PdfPCell(new Phrase("", TableFont));
                                            Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            Cost.Colspan = 2;
                                            Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(Cost);
                                        }

                                        PdfPCell cell1 = new PdfPCell(new Phrase("$" + decimal.Parse(PBr_tot.ToString()).ToString("N", CultureInfo.InvariantCulture), TableFont));
                                        cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        cell1.Colspan = 2;
                                        cell1.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                        table.AddCell(cell1);
                                    }

                                    //PdfPCell Spacecell = new PdfPCell(new Phrase("", TableFont));
                                    //Spacecell.HorizontalAlignment = Element.ALIGN_CENTER;
                                    //if (i == 2)
                                    //    Spacecell.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                    //else
                                    //    Spacecell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //table.AddCell(Spacecell);
                                }

                                //PdfPCell data2 = new PdfPCell(new Phrase(FBr.ToString(), TableFont));
                                //data2.HorizontalAlignment = Element.ALIGN_CENTER;
                                //data2.Colspan = 5;
                                //data2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //table.AddCell(data2);

                                //PdfPCell data3 = new PdfPCell(new Phrase(RBr.ToString(), TableFont));
                                //data3.HorizontalAlignment = Element.ALIGN_CENTER;
                                //data3.Colspan = 5;
                                //data3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //table.AddCell(data3);

                                //PdfPCell data4 = new PdfPCell(new Phrase(PBr.ToString(), TableFont));
                                //data4.HorizontalAlignment = Element.ALIGN_CENTER;
                                //data4.Colspan = 5;
                                //data4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //table.AddCell(data4);

                                PdfPCell data5 = new PdfPCell(new Phrase("AM Snacks", TableFont));
                                data5.HorizontalAlignment = Element.ALIGN_LEFT;
                                data5.Colspan = 3;
                                data5.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                table.AddCell(data5);

                                string[] AM_Snacks = { FAM.ToString(), RAM.ToString(), PAM.ToString() };
                                for (int i = 0; i < AM_Snacks.Length; ++i)
                                {
                                    if (AM_Snacks[i] != "0")
                                    {
                                        string STALL = Convert.ToInt32(AM_Snacks[i].ToString().Trim()).ToString("N", CultureInfo.InvariantCulture);
                                        string STPrint = STALL.Substring(0, (STALL.Length - 3));
                                        PdfPCell cell = new PdfPCell(new Phrase(STPrint, TableFont));
                                        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        //cell.Colspan = 2;
                                        cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(cell);
                                    }
                                    else
                                    {
                                        PdfPCell cell = new PdfPCell(new Phrase("", TableFont));
                                        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        //cell.Colspan = 2;
                                        cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(cell);
                                    }
                                    if (i == 0)
                                    {
                                        if (Program_Entity != null)
                                        {
                                            PdfPCell Cost = new PdfPCell(new Phrase(decimal.Parse(Program_Entity.Free2.Trim()).ToString(), TableFont));
                                            Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            Cost.Colspan = 2;
                                            Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(Cost);
                                        }
                                        else
                                        {
                                            PdfPCell Cost = new PdfPCell(new Phrase("", TableFont));
                                            Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            Cost.Colspan = 2;
                                            Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(Cost);
                                        }

                                        PdfPCell cell1 = new PdfPCell(new Phrase("$" + decimal.Parse(FAM_tot.ToString()).ToString("N", CultureInfo.InvariantCulture), TableFont));
                                        cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        cell1.Colspan = 2;
                                        cell1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(cell1);
                                    }
                                    else if (i == 1)
                                    {
                                        if (Program_Entity != null)
                                        {
                                            PdfPCell Cost = new PdfPCell(new Phrase(decimal.Parse(Program_Entity.Reduced2.Trim()).ToString(), TableFont));
                                            Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            Cost.Colspan = 2;
                                            Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(Cost);
                                        }
                                        else
                                        {
                                            PdfPCell Cost = new PdfPCell(new Phrase("", TableFont));
                                            Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            Cost.Colspan = 2;
                                            Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(Cost);
                                        }

                                        PdfPCell cell1 = new PdfPCell(new Phrase("$" + decimal.Parse(RAM_tot.ToString()).ToString("N", CultureInfo.InvariantCulture), TableFont));
                                        cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        cell1.Colspan = 2;
                                        cell1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(cell1);
                                    }
                                    else if (i == 2)
                                    {
                                        if (Program_Entity != null)
                                        {
                                            PdfPCell Cost = new PdfPCell(new Phrase(decimal.Parse(Program_Entity.Paid2.Trim()).ToString(), TableFont));
                                            Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            Cost.Colspan = 2;
                                            Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(Cost);
                                        }
                                        else
                                        {
                                            PdfPCell Cost = new PdfPCell(new Phrase("", TableFont));
                                            Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            Cost.Colspan = 2;
                                            Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(Cost);
                                        }

                                        PdfPCell cell1 = new PdfPCell(new Phrase("$" + decimal.Parse(PAM_tot.ToString()).ToString("N", CultureInfo.InvariantCulture), TableFont));
                                        cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        cell1.Colspan = 2;
                                        cell1.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                        table.AddCell(cell1);
                                    }
                                    //PdfPCell Spacecell = new PdfPCell(new Phrase("", TableFont));
                                    //Spacecell.HorizontalAlignment = Element.ALIGN_CENTER;
                                    //if (i == 2)
                                    //    Spacecell.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                    //else
                                    //    Spacecell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //table.AddCell(Spacecell);
                                }

                                //PdfPCell data6 = new PdfPCell(new Phrase(FAM.ToString(), TableFont));
                                //data6.HorizontalAlignment = Element.ALIGN_CENTER;
                                //data6.Colspan = 5;
                                //data6.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //table.AddCell(data6);

                                //PdfPCell data7 = new PdfPCell(new Phrase(RAM.ToString(), TableFont));
                                //data7.HorizontalAlignment = Element.ALIGN_CENTER;
                                //data7.Colspan = 5;
                                //data7.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //table.AddCell(data7);

                                //PdfPCell data8 = new PdfPCell(new Phrase(PAM.ToString(), TableFont));
                                //data8.HorizontalAlignment = Element.ALIGN_CENTER;
                                //data8.Colspan = 5;
                                //data8.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //table.AddCell(data8);

                                PdfPCell data9 = new PdfPCell(new Phrase("Lunchs", TableFont));
                                data9.HorizontalAlignment = Element.ALIGN_LEFT;
                                data9.Colspan = 3;
                                data9.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                table.AddCell(data9);

                                string[] Lunch = { FL.ToString(), RL.ToString(), PL.ToString() };
                                for (int i = 0; i < Lunch.Length; ++i)
                                {
                                    if (Lunch[i] != "0")
                                    {
                                        string STALL = Convert.ToInt32(Lunch[i].ToString().Trim()).ToString("N", CultureInfo.InvariantCulture);
                                        string STPrint = STALL.Substring(0, (STALL.Length - 3));
                                        PdfPCell cell = new PdfPCell(new Phrase(STPrint, TableFont));
                                        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        //cell.Colspan = 2;
                                        cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(cell);
                                    }
                                    else
                                    {
                                        PdfPCell cell = new PdfPCell(new Phrase("", TableFont));
                                        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        //cell.Colspan = 2;
                                        cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(cell);
                                    }

                                    if (i == 0)
                                    {
                                        if (Program_Entity != null)
                                        {
                                            PdfPCell Cost = new PdfPCell(new Phrase(decimal.Parse(Program_Entity.Free3.Trim()).ToString(), TableFont));
                                            Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            Cost.Colspan = 2;
                                            Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(Cost);
                                        }
                                        else
                                        {
                                            PdfPCell Cost = new PdfPCell(new Phrase("", TableFont));
                                            Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            Cost.Colspan = 2;
                                            Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(Cost);
                                        }

                                        PdfPCell cell1 = new PdfPCell(new Phrase("$" + decimal.Parse(FL_tot.ToString()).ToString("N", CultureInfo.InvariantCulture), TableFont));
                                        cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        cell1.Colspan = 2;
                                        cell1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(cell1);
                                    }
                                    else if (i == 1)
                                    {
                                        if (Program_Entity != null)
                                        {
                                            PdfPCell Cost = new PdfPCell(new Phrase( decimal.Parse(Program_Entity.Reduced3.Trim()).ToString(), TableFont));
                                            Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            Cost.Colspan = 2;
                                            Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(Cost);
                                        }
                                        else
                                        {
                                            PdfPCell Cost = new PdfPCell(new Phrase("", TableFont));
                                            Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            Cost.Colspan = 2;
                                            Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(Cost);
                                        }

                                        PdfPCell cell1 = new PdfPCell(new Phrase("$" + decimal.Parse(RL_tot.ToString()).ToString("N", CultureInfo.InvariantCulture), TableFont));
                                        cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        cell1.Colspan = 2;
                                        cell1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(cell1);
                                    }
                                    else if (i == 2)
                                    {
                                        if (Program_Entity != null)
                                        {
                                            PdfPCell Cost = new PdfPCell(new Phrase( decimal.Parse(Program_Entity.Paid3.Trim()).ToString(), TableFont));
                                            Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            Cost.Colspan = 2;
                                            Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(Cost);
                                        }
                                        else
                                        {
                                            PdfPCell Cost = new PdfPCell(new Phrase("", TableFont));
                                            Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            Cost.Colspan = 2;
                                            Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(Cost);
                                        }

                                        PdfPCell cell1 = new PdfPCell(new Phrase("$" + decimal.Parse(PL_tot.ToString()).ToString("N", CultureInfo.InvariantCulture), TableFont));
                                        cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        cell1.Colspan = 2;
                                        cell1.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                        table.AddCell(cell1);
                                    }
                                    //PdfPCell Spacecell = new PdfPCell(new Phrase("", TableFont));
                                    //Spacecell.HorizontalAlignment = Element.ALIGN_CENTER;
                                    //if (i == 2)
                                    //    Spacecell.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                    //else
                                    //    Spacecell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //table.AddCell(Spacecell);
                                }

                                //PdfPCell data10 = new PdfPCell(new Phrase(FL.ToString(), TableFont));
                                //data10.HorizontalAlignment = Element.ALIGN_CENTER;
                                //data10.Colspan = 5;
                                //data10.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //table.AddCell(data10);

                                //PdfPCell data11 = new PdfPCell(new Phrase(RL.ToString(), TableFont));
                                //data11.HorizontalAlignment = Element.ALIGN_CENTER;
                                //data11.Colspan = 5;
                                //data11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //table.AddCell(data11);

                                //PdfPCell data12 = new PdfPCell(new Phrase(PL.ToString(), TableFont));
                                //data12.HorizontalAlignment = Element.ALIGN_CENTER;
                                //data12.Colspan = 5;
                                //data12.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //table.AddCell(data12);

                                PdfPCell data13 = new PdfPCell(new Phrase("PM Snacks", TableFont));
                                data13.HorizontalAlignment = Element.ALIGN_LEFT;
                                data13.Colspan = 3;
                                data13.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                table.AddCell(data13);

                                string[] PM_Snacks = { FPM.ToString(), RPM.ToString(), PPM.ToString() };
                                for (int i = 0; i < PM_Snacks.Length; ++i)
                                {
                                    if (PM_Snacks[i] != "0")
                                    {
                                        string STALL = Convert.ToInt32(PM_Snacks[i].ToString().Trim()).ToString("N", CultureInfo.InvariantCulture);
                                        string STPrint = STALL.Substring(0, (STALL.Length - 3));
                                        PdfPCell cell = new PdfPCell(new Phrase(STPrint, TableFont));
                                        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        //cell.Colspan = 2;
                                        cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(cell);
                                    }
                                    else
                                    {
                                        PdfPCell cell = new PdfPCell(new Phrase("", TableFont));
                                        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        //cell.Colspan = 2;
                                        cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(cell);
                                    }

                                    if (i == 0)
                                    {
                                        if (Program_Entity != null)
                                        {
                                            PdfPCell Cost = new PdfPCell(new Phrase(decimal.Parse(Program_Entity.Free4.Trim()).ToString(), TableFont));
                                            Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            Cost.Colspan = 2;
                                            Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(Cost);
                                        }
                                        else
                                        {
                                            PdfPCell Cost = new PdfPCell(new Phrase("", TableFont));
                                            Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            Cost.Colspan = 2;
                                            Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(Cost);
                                        }

                                        PdfPCell cell1 = new PdfPCell(new Phrase("$" + decimal.Parse(FPM_tot.ToString()).ToString("N", CultureInfo.InvariantCulture), TableFont));
                                        cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        cell1.Colspan = 2;
                                        cell1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(cell1);
                                    }
                                    else if (i == 1)
                                    {
                                        if (Program_Entity != null)
                                        {
                                            PdfPCell Cost = new PdfPCell(new Phrase( decimal.Parse(Program_Entity.Reduced4.Trim()).ToString(), TableFont));
                                            Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            Cost.Colspan = 2;
                                            Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(Cost);
                                        }
                                        else
                                        {
                                            PdfPCell Cost = new PdfPCell(new Phrase("", TableFont));
                                            Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            Cost.Colspan = 2;
                                            Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(Cost);
                                        }

                                        PdfPCell cell1 = new PdfPCell(new Phrase("$" + decimal.Parse(RPM_tot.ToString()).ToString("N", CultureInfo.InvariantCulture), TableFont));
                                        cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        cell1.Colspan = 2;
                                        cell1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(cell1);
                                    }
                                    else if (i == 2)
                                    {
                                        if (Program_Entity != null)
                                        {
                                            PdfPCell Cost = new PdfPCell(new Phrase( decimal.Parse(Program_Entity.Paid4.Trim()).ToString(), TableFont));
                                            Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            Cost.Colspan = 2;
                                            Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(Cost);
                                        }
                                        else
                                        {
                                            PdfPCell Cost = new PdfPCell(new Phrase("", TableFont));
                                            Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            Cost.Colspan = 2;
                                            Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(Cost);
                                        }

                                        PdfPCell cell1 = new PdfPCell(new Phrase("$" + decimal.Parse(PPM_tot.ToString()).ToString("N", CultureInfo.InvariantCulture), TableFont));
                                        cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        cell1.Colspan = 2;
                                        cell1.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                        table.AddCell(cell1);
                                    }
                                    //PdfPCell Spacecell = new PdfPCell(new Phrase("", TableFont));
                                    //Spacecell.HorizontalAlignment = Element.ALIGN_CENTER;
                                    //if (i == 2)
                                    //    Spacecell.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                    //else
                                    //    Spacecell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //table.AddCell(Spacecell);
                                }

                                PdfPCell data14 = new PdfPCell(new Phrase("Suppers", TableFont));
                                data14.HorizontalAlignment = Element.ALIGN_LEFT;
                                data14.Colspan = 3;
                                data14.Border = iTextSharp.text.Rectangle.LEFT_BORDER + iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                table.AddCell(data14);

                                string[] Supper = { FS.ToString(), RS.ToString(), PS.ToString() };
                                for (int i = 0; i < Supper.Length; ++i)
                                {
                                    if (Supper[i] != "0")
                                    {
                                        string STALL = Convert.ToInt32(Supper[i].ToString().Trim()).ToString("N", CultureInfo.InvariantCulture);
                                        string STPrint = STALL.Substring(0, (STALL.Length - 3));
                                        PdfPCell cell = new PdfPCell(new Phrase(STPrint, TableFont));
                                        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        //cell.Colspan = 2;
                                        cell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                        table.AddCell(cell);
                                    }
                                    else
                                    {
                                        PdfPCell cell = new PdfPCell(new Phrase("", TableFont));
                                        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        //cell.Colspan = 2;
                                        cell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                        table.AddCell(cell);
                                    }

                                    if (i == 0)
                                    {
                                        if (Program_Entity != null)
                                        {
                                            PdfPCell Cost = new PdfPCell(new Phrase( decimal.Parse(Program_Entity.Free5.Trim()).ToString(), TableFont));
                                            Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            Cost.Colspan = 2;
                                            Cost.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                            table.AddCell(Cost);
                                        }
                                        else
                                        {
                                            PdfPCell Cost = new PdfPCell(new Phrase("", TableFont));
                                            Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            Cost.Colspan = 2;
                                            Cost.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                            table.AddCell(Cost);
                                        }

                                        PdfPCell cell1 = new PdfPCell(new Phrase("$" + decimal.Parse(FS_tot.ToString()).ToString("N", CultureInfo.InvariantCulture), TableFont));
                                        cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        cell1.Colspan = 2;
                                        cell1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                        table.AddCell(cell1);
                                    }
                                    else if (i == 1)
                                    {
                                        if (Program_Entity != null)
                                        {
                                            PdfPCell Cost = new PdfPCell(new Phrase( decimal.Parse(Program_Entity.Reduced5.Trim()).ToString(), TableFont));
                                            Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            Cost.Colspan = 2;
                                            Cost.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                            table.AddCell(Cost);
                                        }
                                        else
                                        {
                                            PdfPCell Cost = new PdfPCell(new Phrase("", TableFont));
                                            Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            Cost.Colspan = 2;
                                            Cost.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                            table.AddCell(Cost);
                                        }

                                        PdfPCell cell1 = new PdfPCell(new Phrase("$" + decimal.Parse(RS_tot.ToString()).ToString("N", CultureInfo.InvariantCulture), TableFont));
                                        cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        cell1.Colspan = 2;
                                        cell1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                        table.AddCell(cell1);
                                    }
                                    else if (i == 2)
                                    {
                                        if (Program_Entity != null)
                                        {
                                            PdfPCell Cost = new PdfPCell(new Phrase(decimal.Parse(Program_Entity.Paid5.Trim()).ToString(), TableFont));
                                            Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            Cost.Colspan = 2;
                                            Cost.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                            table.AddCell(Cost);
                                        }
                                        else
                                        {
                                            PdfPCell Cost = new PdfPCell(new Phrase("", TableFont));
                                            Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            Cost.Colspan = 2;
                                            Cost.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                            table.AddCell(Cost);
                                        }

                                        PdfPCell cell1 = new PdfPCell(new Phrase("$" + decimal.Parse(PS_tot.ToString()).ToString("N", CultureInfo.InvariantCulture), TableFont));
                                        cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        cell1.Colspan = 2;
                                        cell1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER+iTextSharp.text.Rectangle.RIGHT_BORDER;
                                        table.AddCell(cell1);
                                    }
                                    //PdfPCell Spacecell = new PdfPCell(new Phrase("", TableFont));
                                    //Spacecell.HorizontalAlignment = Element.ALIGN_CENTER;
                                    //if (i == 2)
                                    //    Spacecell.Border = iTextSharp.text.Rectangle.RIGHT_BORDER + iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                    //else
                                    //    Spacecell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                    //table.AddCell(Spacecell);
                                }

                                PdfPCell totalSpace = new PdfPCell(new Phrase("", TableFont));
                                totalSpace.HorizontalAlignment = Element.ALIGN_RIGHT;
                                totalSpace.Colspan = 6;
                                totalSpace.FixedHeight = 15f;
                                totalSpace.Border = iTextSharp.text.Rectangle.LEFT_BORDER + iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                table.AddCell(totalSpace);

                                PdfPCell total = new PdfPCell(new Phrase("$" + decimal.Parse(Tot_FCounts.ToString()).ToString("N", CultureInfo.InvariantCulture), TableFont));
                                total.HorizontalAlignment = Element.ALIGN_RIGHT;
                                total.FixedHeight = 15f;
                                total.Colspan = 2;
                                total.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER;
                                table.AddCell(total);

                                PdfPCell totalRSpace = new PdfPCell(new Phrase("", TableFont));
                                totalRSpace.HorizontalAlignment = Element.ALIGN_RIGHT;
                                totalRSpace.FixedHeight = 15f;
                                totalRSpace.Colspan = 3;
                                totalRSpace.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER;
                                table.AddCell(totalRSpace);

                                PdfPCell totalR = new PdfPCell(new Phrase("$" + decimal.Parse(Tot_RCounts.ToString()).ToString("N", CultureInfo.InvariantCulture), TableFont));
                                totalR.HorizontalAlignment = Element.ALIGN_RIGHT;
                                totalR.FixedHeight = 15f;
                                totalR.Colspan = 2;
                                totalR.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER;
                                table.AddCell(totalR);

                                PdfPCell totalPSpace = new PdfPCell(new Phrase("", TableFont));
                                totalPSpace.HorizontalAlignment = Element.ALIGN_RIGHT;
                                totalPSpace.FixedHeight = 15f;
                                totalPSpace.Colspan = 3;
                                totalPSpace.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER;
                                table.AddCell(totalPSpace);

                                PdfPCell totalP = new PdfPCell(new Phrase("$" + decimal.Parse(Tot_PCounts.ToString()).ToString("N", CultureInfo.InvariantCulture), TableFont));
                                totalP.HorizontalAlignment = Element.ALIGN_RIGHT;
                                totalP.FixedHeight = 15f;
                                totalP.Colspan = 2;
                                totalP.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER;
                                table.AddCell(totalP);

                                PdfPCell totalT = new PdfPCell(new Phrase("$" + decimal.Parse((Tot_PCounts + Tot_RCounts + Tot_FCounts).ToString()).ToString("N", CultureInfo.InvariantCulture), TableFont));
                                totalT.HorizontalAlignment = Element.ALIGN_RIGHT;
                                totalT.FixedHeight = 15f;
                                totalT.Colspan = 18;
                                totalT.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER + iTextSharp.text.Rectangle.LEFT_BORDER;
                                table.AddCell(totalT);

                                Tot_RCounts = 0; Tot_PCounts = 0; Tot_FCounts = 0;
                                if (table.Rows.Count > 0)
                                {
                                    document.Add(table);
                                    table.DeleteBodyRows();
                                    document.NewPage();
                                }
                            }

                        }

                        PdfPTable table1 = new PdfPTable(18);
                        table1.TotalWidth = 560f;
                        table1.WidthPercentage = 100;
                        table1.LockedWidth = true;
                        float[] table1widths = new float[] { 15f, 15f, 15f, 12f, 12f, 12f, 12f, 12f, 12f, 12f, 12f, 12f, 12f, 12f, 12f, 12f, 12f, 12f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                        table1.SetWidths(table1widths);
                        table1.HorizontalAlignment = Element.ALIGN_CENTER;

                        PdfPCell Tot_Head = new PdfPCell(new Phrase("Summary", TblFontBold));
                        Tot_Head.HorizontalAlignment = Element.ALIGN_CENTER;
                        Tot_Head.Colspan = 18;
                        Tot_Head.Border = iTextSharp.text.Rectangle.BOX;
                        table1.AddCell(Tot_Head);

                        PdfPCell Tot_sapce1 = new PdfPCell(new Phrase("", TableFont));
                        Tot_sapce1.HorizontalAlignment = Element.ALIGN_CENTER;
                        Tot_sapce1.Colspan = 3;
                        Tot_sapce1.Border = iTextSharp.text.Rectangle.BOX;
                        table1.AddCell(Tot_sapce1);

                        string[] SubHeader_last = { " F R E E ", " R E D U C E D ", " P A I D " };
                        for (int i = 0; i < SubHeader_last.Length; ++i)
                        {
                            PdfPCell SubHeader1 = new PdfPCell(new Phrase(SubHeader_last[i], TableFont));
                            SubHeader1.HorizontalAlignment = Element.ALIGN_CENTER;
                            SubHeader1.Colspan = 5;
                            SubHeader1.Border = iTextSharp.text.Rectangle.BOX;
                            table1.AddCell(SubHeader1);
                        }

                        PdfPCell Totdata1 = new PdfPCell(new Phrase("Breakfasts", TableFont));
                        Totdata1.HorizontalAlignment = Element.ALIGN_LEFT;
                        Totdata1.Colspan = 3;
                        Totdata1.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                        table1.AddCell(Totdata1);

                        if (Program_Entity != null)
                        {
                            FBr_tot = Tot_FBr * decimal.Parse(Program_Entity.Free1.Trim());
                            FAM_tot = Tot_FAM * decimal.Parse(Program_Entity.Free2.Trim());
                            FL_tot = Tot_FL * decimal.Parse(Program_Entity.Free3.Trim());
                            FPM_tot = Tot_FPM * decimal.Parse(Program_Entity.Free4.Trim());
                            FS_tot = Tot_FS * decimal.Parse(Program_Entity.Free5.Trim());

                            RBr_tot = Tot_RBr * decimal.Parse(Program_Entity.Reduced1.Trim());
                            RAM_tot = Tot_RAM * decimal.Parse(Program_Entity.Reduced2.Trim());
                            RL_tot = Tot_RL * decimal.Parse(Program_Entity.Reduced3.Trim());
                            RPM_tot = Tot_RPM * decimal.Parse(Program_Entity.Reduced4.Trim());
                            RS_tot = Tot_RS * decimal.Parse(Program_Entity.Reduced5.Trim());

                            PBr_tot = Tot_PBr * decimal.Parse(Program_Entity.Paid1.Trim());
                            PAM_tot = Tot_PAM * decimal.Parse(Program_Entity.Paid2.Trim());
                            PL_tot = Tot_PL * decimal.Parse(Program_Entity.Paid3.Trim());
                            PPM_tot = Tot_PPM * decimal.Parse(Program_Entity.Paid4.Trim());
                            PS_tot = Tot_PS * decimal.Parse(Program_Entity.Paid5.Trim());
                        }
                        Tot_FCounts = FBr_tot + FAM_tot + FL_tot + FPM_tot + FS_tot;
                        Tot_RCounts = RBr_tot + RAM_tot + RL_tot + RPM_tot + RS_tot;
                        Tot_PCounts = PBr_tot + PAM_tot + PL_tot + PPM_tot + PS_tot;

                        string[] TotBreakfast = { Tot_FBr.ToString(), Tot_RBr.ToString(), Tot_PBr.ToString() };
                        for (int i = 0; i < TotBreakfast.Length; ++i)
                        {
                            if (TotBreakfast[i] != "0")
                            {
                                string STALL = Convert.ToInt32(TotBreakfast[i].ToString().Trim()).ToString("N", CultureInfo.InvariantCulture);
                                string STPrint = STALL.Substring(0, (STALL.Length - 3));
                                PdfPCell cell = new PdfPCell(new Phrase(STPrint, TableFont));
                                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //cell.Colspan = 2;
                                cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table1.AddCell(cell);
                            }
                            else
                            {
                                PdfPCell cell = new PdfPCell(new Phrase("", TableFont));
                                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //cell.Colspan = 2;
                                cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table1.AddCell(cell);
                            }

                            if (i == 0)
                            {
                                if (Program_Entity != null)
                                {
                                    PdfPCell Cost = new PdfPCell(new Phrase(decimal.Parse(Program_Entity.Free1.Trim()).ToString(), TableFont));
                                    Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    Cost.Colspan = 2;
                                    Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table1.AddCell(Cost);
                                }
                                else
                                {
                                    PdfPCell Cost = new PdfPCell(new Phrase("", TableFont));
                                    Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    Cost.Colspan = 2;
                                    Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table1.AddCell(Cost);
                                }

                                PdfPCell cell1 = new PdfPCell(new Phrase("$" + decimal.Parse(FBr_tot.ToString()).ToString("N", CultureInfo.InvariantCulture), TableFont));
                                cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cell1.Colspan = 2;
                                cell1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table1.AddCell(cell1);
                            }
                            else if (i == 1)
                            {
                                if (Program_Entity != null)
                                {
                                    PdfPCell Cost = new PdfPCell(new Phrase(decimal.Parse(Program_Entity.Reduced1.Trim()).ToString(), TableFont));
                                    Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    Cost.Colspan = 2;
                                    Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table1.AddCell(Cost);
                                }
                                else
                                {
                                    PdfPCell Cost = new PdfPCell(new Phrase("", TableFont));
                                    Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    Cost.Colspan = 2;
                                    Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table1.AddCell(Cost);
                                }

                                PdfPCell cell1 = new PdfPCell(new Phrase("$" + decimal.Parse(RBr_tot.ToString()).ToString("N", CultureInfo.InvariantCulture), TableFont));
                                cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cell1.Colspan = 2;
                                cell1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table1.AddCell(cell1);
                            }
                            else if (i == 2)
                            {
                                if (Program_Entity != null)
                                {
                                    PdfPCell Cost = new PdfPCell(new Phrase(decimal.Parse(Program_Entity.Paid1.Trim()).ToString(), TableFont));
                                    Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    Cost.Colspan = 2;
                                    Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table1.AddCell(Cost);
                                }
                                else
                                {
                                    PdfPCell Cost = new PdfPCell(new Phrase("", TableFont));
                                    Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    Cost.Colspan = 2;
                                    Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table1.AddCell(Cost);
                                }

                                PdfPCell cell1 = new PdfPCell(new Phrase("$" + decimal.Parse(PBr_tot.ToString()).ToString("N", CultureInfo.InvariantCulture), TableFont));
                                cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cell1.Colspan = 2;
                                cell1.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                table1.AddCell(cell1);
                            }
                            //PdfPCell Spacecell = new PdfPCell(new Phrase("", TableFont));
                            //Spacecell.HorizontalAlignment = Element.ALIGN_CENTER;
                            //if (i == 2)
                            //    Spacecell.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                            //else
                            //    Spacecell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            //table1.AddCell(Spacecell);

                        }

                        PdfPCell totdata5 = new PdfPCell(new Phrase("AM Snacks", TableFont));
                        totdata5.HorizontalAlignment = Element.ALIGN_LEFT;
                        totdata5.Colspan = 3;
                        totdata5.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                        table1.AddCell(totdata5);

                        string[] totAM_Snacks = { Tot_FAM.ToString(), Tot_RAM.ToString(), Tot_PAM.ToString() };
                        for (int i = 0; i < totAM_Snacks.Length; ++i)
                        {
                            if (totAM_Snacks[i] != "0")
                            {
                                string STALL = Convert.ToInt32(totAM_Snacks[i].ToString().Trim()).ToString("N", CultureInfo.InvariantCulture);
                                string STPrint = STALL.Substring(0, (STALL.Length - 3));
                                PdfPCell cell = new PdfPCell(new Phrase(STPrint, TableFont));
                                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //cell.Colspan = 2;
                                cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table1.AddCell(cell);
                            }
                            else
                            {
                                PdfPCell cell = new PdfPCell(new Phrase("", TableFont));
                                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //cell.Colspan = 2;
                                cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table1.AddCell(cell);
                            }

                            if (i == 0)
                            {
                                if (Program_Entity != null)
                                {
                                    PdfPCell Cost = new PdfPCell(new Phrase(decimal.Parse(Program_Entity.Free2.Trim()).ToString(), TableFont));
                                    Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    Cost.Colspan = 2;
                                    Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table1.AddCell(Cost);
                                }
                                else
                                {
                                    PdfPCell Cost = new PdfPCell(new Phrase("", TableFont));
                                    Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    Cost.Colspan = 2;
                                    Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table1.AddCell(Cost);
                                }

                                PdfPCell cell1 = new PdfPCell(new Phrase("$" + decimal.Parse(FAM_tot.ToString()).ToString("N", CultureInfo.InvariantCulture), TableFont));
                                cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cell1.Colspan = 2;
                                cell1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table1.AddCell(cell1);
                            }
                            else if (i == 1)
                            {
                                if (Program_Entity != null)
                                {
                                    PdfPCell Cost = new PdfPCell(new Phrase(decimal.Parse(Program_Entity.Reduced2.Trim()).ToString(), TableFont));
                                    Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    Cost.Colspan = 2;
                                    Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table1.AddCell(Cost);
                                }
                                else
                                {
                                    PdfPCell Cost = new PdfPCell(new Phrase("", TableFont));
                                    Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    Cost.Colspan = 2;
                                    Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table1.AddCell(Cost);
                                }

                                PdfPCell cell1 = new PdfPCell(new Phrase("$" + decimal.Parse(RAM_tot.ToString()).ToString("N", CultureInfo.InvariantCulture), TableFont));
                                cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cell1.Colspan = 2;
                                cell1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table1.AddCell(cell1);
                            }
                            else if (i == 2)
                            {
                                if (Program_Entity != null)
                                {
                                    PdfPCell Cost = new PdfPCell(new Phrase(decimal.Parse(Program_Entity.Paid2.Trim()).ToString(), TableFont));
                                    Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    Cost.Colspan = 2;
                                    Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table1.AddCell(Cost);
                                }
                                else
                                {
                                    PdfPCell Cost = new PdfPCell(new Phrase("", TableFont));
                                    Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    Cost.Colspan = 2;
                                    Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table1.AddCell(Cost);
                                }

                                PdfPCell cell1 = new PdfPCell(new Phrase("$" + decimal.Parse(PAM_tot.ToString()).ToString("N", CultureInfo.InvariantCulture), TableFont));
                                cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cell1.Colspan = 2;
                                cell1.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                table1.AddCell(cell1);
                            }
                            //PdfPCell Spacecell = new PdfPCell(new Phrase("", TableFont));
                            //Spacecell.HorizontalAlignment = Element.ALIGN_CENTER;
                            //if (i == 2)
                            //    Spacecell.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                            //else
                            //    Spacecell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            //table1.AddCell(Spacecell);
                        }

                        PdfPCell totdata9 = new PdfPCell(new Phrase("Lunchs", TableFont));
                        totdata9.HorizontalAlignment = Element.ALIGN_LEFT;
                        totdata9.Colspan = 3;
                        totdata9.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                        table1.AddCell(totdata9);

                        string[] totLunch = { Tot_FL.ToString(), Tot_RL.ToString(), Tot_PL.ToString() };
                        for (int i = 0; i < totLunch.Length; ++i)
                        {
                            if (totLunch[i] != "0")
                            {
                                string STALL = Convert.ToInt32(totLunch[i].ToString().Trim()).ToString("N", CultureInfo.InvariantCulture);
                                string STPrint = STALL.Substring(0, (STALL.Length - 3));
                                PdfPCell cell = new PdfPCell(new Phrase(STPrint, TableFont));
                                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //cell.Colspan = 2;
                                cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table1.AddCell(cell);
                            }
                            else
                            {
                                PdfPCell cell = new PdfPCell(new Phrase("", TableFont));
                                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //cell.Colspan = 2;
                                cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table1.AddCell(cell);
                            }

                            if (i == 0)
                            {
                                if (Program_Entity != null)
                                {
                                    PdfPCell Cost = new PdfPCell(new Phrase( decimal.Parse(Program_Entity.Free3.Trim()).ToString(), TableFont));
                                    Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    Cost.Colspan = 2;
                                    Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table1.AddCell(Cost);
                                }
                                else
                                {
                                    PdfPCell Cost = new PdfPCell(new Phrase("", TableFont));
                                    Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    Cost.Colspan = 2;
                                    Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table1.AddCell(Cost);
                                }

                                PdfPCell cell1 = new PdfPCell(new Phrase("$" + decimal.Parse(FL_tot.ToString()).ToString("N", CultureInfo.InvariantCulture), TableFont));
                                cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cell1.Colspan = 2;
                                cell1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table1.AddCell(cell1);
                            }
                            else if (i == 1)
                            {
                                if (Program_Entity != null)
                                {
                                    PdfPCell Cost = new PdfPCell(new Phrase( decimal.Parse(Program_Entity.Reduced3.Trim()).ToString(), TableFont));
                                    Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    Cost.Colspan = 2;
                                    Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table1.AddCell(Cost);
                                }
                                else
                                {
                                    PdfPCell Cost = new PdfPCell(new Phrase("", TableFont));
                                    Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    Cost.Colspan = 2;
                                    Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table1.AddCell(Cost);
                                }

                                PdfPCell cell1 = new PdfPCell(new Phrase("$" + decimal.Parse(RL_tot.ToString()).ToString("N", CultureInfo.InvariantCulture), TableFont));
                                cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cell1.Colspan = 2;
                                cell1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table1.AddCell(cell1);
                            }
                            else if (i == 2)
                            {
                                if (Program_Entity != null)
                                {
                                    PdfPCell Cost = new PdfPCell(new Phrase(decimal.Parse(Program_Entity.Paid3.Trim()).ToString(), TableFont));
                                    Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    Cost.Colspan = 2;
                                    Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table1.AddCell(Cost);
                                }
                                else
                                {
                                    PdfPCell Cost = new PdfPCell(new Phrase("", TableFont));
                                    Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    Cost.Colspan = 2;
                                    Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table1.AddCell(Cost);
                                }

                                PdfPCell cell1 = new PdfPCell(new Phrase("$" + decimal.Parse(PL_tot.ToString()).ToString("N", CultureInfo.InvariantCulture), TableFont));
                                cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cell1.Colspan = 2;
                                cell1.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                table1.AddCell(cell1);
                            }
                            //PdfPCell Spacecell = new PdfPCell(new Phrase("", TableFont));
                            //Spacecell.HorizontalAlignment = Element.ALIGN_CENTER;
                            //if (i == 2)
                            //    Spacecell.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                            //else
                            //    Spacecell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            //table1.AddCell(Spacecell);
                        }

                        PdfPCell totdata13 = new PdfPCell(new Phrase("PM Snacks", TableFont));
                        totdata13.HorizontalAlignment = Element.ALIGN_LEFT;
                        totdata13.Colspan = 3;
                        totdata13.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                        table1.AddCell(totdata13);

                        string[] totPM_Snacks = { Tot_FPM.ToString(), Tot_RPM.ToString(), Tot_PPM.ToString() };
                        for (int i = 0; i < totPM_Snacks.Length; ++i)
                        {
                            if (totPM_Snacks[i] != "0")
                            {
                                string STALL = Convert.ToInt32(totPM_Snacks[i].ToString().Trim()).ToString("N", CultureInfo.InvariantCulture);
                                string STPrint = STALL.Substring(0, (STALL.Length - 3));
                                PdfPCell cell = new PdfPCell(new Phrase(STPrint, TableFont));
                                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //cell.Colspan = 2;
                                cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table1.AddCell(cell);
                            }
                            else
                            {
                                PdfPCell cell = new PdfPCell(new Phrase("", TableFont));
                                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //cell.Colspan = 2;
                                cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table1.AddCell(cell);
                            }

                            if (i == 0)
                            {
                                if (Program_Entity != null)
                                {
                                    PdfPCell Cost = new PdfPCell(new Phrase(decimal.Parse(Program_Entity.Free4.Trim()).ToString(), TableFont));
                                    Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    Cost.Colspan = 2;
                                    Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table1.AddCell(Cost);
                                }
                                else
                                {
                                    PdfPCell Cost = new PdfPCell(new Phrase("", TableFont));
                                    Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    Cost.Colspan = 2;
                                    Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table1.AddCell(Cost);
                                }

                                PdfPCell cell1 = new PdfPCell(new Phrase("$" + decimal.Parse(FPM_tot.ToString()).ToString("N", CultureInfo.InvariantCulture), TableFont));
                                cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cell1.Colspan = 2;
                                cell1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table1.AddCell(cell1);
                            }
                            else if (i == 1)
                            {
                                if (Program_Entity != null)
                                {
                                    PdfPCell Cost = new PdfPCell(new Phrase(decimal.Parse(Program_Entity.Reduced4.Trim()).ToString(), TableFont));
                                    Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    Cost.Colspan = 2;
                                    Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table1.AddCell(Cost);
                                }
                                else
                                {
                                    PdfPCell Cost = new PdfPCell(new Phrase("", TableFont));
                                    Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    Cost.Colspan = 2;
                                    Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table1.AddCell(Cost);
                                }

                                PdfPCell cell1 = new PdfPCell(new Phrase("$" + decimal.Parse(RPM_tot.ToString()).ToString("N", CultureInfo.InvariantCulture), TableFont));
                                cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cell1.Colspan = 2;
                                cell1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table1.AddCell(cell1);
                            }
                            else if (i == 2)
                            {
                                if (Program_Entity != null)
                                {
                                    PdfPCell Cost = new PdfPCell(new Phrase( decimal.Parse(Program_Entity.Paid4.Trim()).ToString(), TableFont));
                                    Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    Cost.Colspan = 2;
                                    Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table1.AddCell(Cost);
                                }
                                else
                                {
                                    PdfPCell Cost = new PdfPCell(new Phrase("", TableFont));
                                    Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    Cost.Colspan = 2;
                                    Cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table1.AddCell(Cost);
                                }

                                PdfPCell cell1 = new PdfPCell(new Phrase("$" + decimal.Parse(PPM_tot.ToString()).ToString("N", CultureInfo.InvariantCulture), TableFont));
                                cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cell1.Colspan = 2;
                                cell1.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                table1.AddCell(cell1);
                            }
                            //PdfPCell Spacecell = new PdfPCell(new Phrase("", TableFont));
                            //Spacecell.HorizontalAlignment = Element.ALIGN_CENTER;
                            //if (i == 2)
                            //    Spacecell.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                            //else
                            //    Spacecell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            //table1.AddCell(Spacecell);
                        }

                        PdfPCell Tot_data14 = new PdfPCell(new Phrase("Suppers", TableFont));
                        Tot_data14.HorizontalAlignment = Element.ALIGN_LEFT;
                        Tot_data14.Colspan = 3;
                        Tot_data14.Border = iTextSharp.text.Rectangle.LEFT_BORDER + iTextSharp.text.Rectangle.BOTTOM_BORDER;
                        table1.AddCell(Tot_data14);

                        string[] Tot_Supper = { Tot_FS.ToString(), Tot_RS.ToString(), Tot_PS.ToString() };
                        for (int i = 0; i < Tot_Supper.Length; ++i)
                        {
                            if (Tot_Supper[i] != "0")
                            {
                                string STALL = Convert.ToInt32(Tot_Supper[i].ToString().Trim()).ToString("N", CultureInfo.InvariantCulture);
                                string STPrint = STALL.Substring(0, (STALL.Length - 3));
                                PdfPCell cell = new PdfPCell(new Phrase(STPrint, TableFont));
                                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //cell.Colspan = 2;
                                cell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                table1.AddCell(cell);
                            }
                            else
                            {
                                PdfPCell cell = new PdfPCell(new Phrase("", TableFont));
                                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //cell.Colspan = 2;
                                cell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                table1.AddCell(cell);
                            }

                            if (i == 0)
                            {
                                if (Program_Entity != null)
                                {
                                    PdfPCell Cost = new PdfPCell(new Phrase( decimal.Parse(Program_Entity.Free5.Trim()).ToString(), TableFont));
                                    Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    Cost.Colspan = 2;
                                    Cost.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                    table1.AddCell(Cost);
                                }
                                else
                                {
                                    PdfPCell Cost = new PdfPCell(new Phrase("", TableFont));
                                    Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    Cost.Colspan = 2;
                                    Cost.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                    table1.AddCell(Cost);
                                }

                                PdfPCell cell1 = new PdfPCell(new Phrase("$" + decimal.Parse(FS_tot.ToString()).ToString("N", CultureInfo.InvariantCulture), TableFont));
                                cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cell1.Colspan = 2;
                                cell1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                table1.AddCell(cell1);
                            }
                            else if (i == 1)
                            {
                                if (Program_Entity != null)
                                {
                                    PdfPCell Cost = new PdfPCell(new Phrase(decimal.Parse(Program_Entity.Reduced5.Trim()).ToString(), TableFont));
                                    Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    Cost.Colspan = 2;
                                    Cost.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                    table1.AddCell(Cost);
                                }
                                else
                                {
                                    PdfPCell Cost = new PdfPCell(new Phrase("", TableFont));
                                    Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    Cost.Colspan = 2;
                                    Cost.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                    table1.AddCell(Cost);
                                }

                                PdfPCell cell1 = new PdfPCell(new Phrase("$" + decimal.Parse(RS_tot.ToString()).ToString("N", CultureInfo.InvariantCulture), TableFont));
                                cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cell1.Colspan = 2;
                                cell1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                table1.AddCell(cell1);
                            }
                            else if (i == 2)
                            {
                                if (Program_Entity != null)
                                {
                                    PdfPCell Cost = new PdfPCell(new Phrase( decimal.Parse(Program_Entity.Paid5.Trim()).ToString(), TableFont));
                                    Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    Cost.Colspan = 2;
                                    Cost.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                    table1.AddCell(Cost);
                                }
                                else
                                {
                                    PdfPCell Cost = new PdfPCell(new Phrase("", TableFont));
                                    Cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    Cost.Colspan = 2;
                                    Cost.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                    table1.AddCell(Cost);
                                }

                                PdfPCell cell1 = new PdfPCell(new Phrase("$" + decimal.Parse(PS_tot.ToString()).ToString("N", CultureInfo.InvariantCulture), TableFont));
                                cell1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                cell1.Colspan = 2;
                                cell1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER+iTextSharp.text.Rectangle.RIGHT_BORDER;
                                table1.AddCell(cell1);
                            }
                            //PdfPCell Spacecell = new PdfPCell(new Phrase("", TableFont));
                            //Spacecell.HorizontalAlignment = Element.ALIGN_CENTER;
                            //if (i == 2)
                            //    Spacecell.Border = iTextSharp.text.Rectangle.RIGHT_BORDER + iTextSharp.text.Rectangle.BOTTOM_BORDER;
                            //else
                            //    Spacecell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                            //table1.AddCell(Spacecell);
                        }

                        PdfPCell total1Space = new PdfPCell(new Phrase("", TableFont));
                        total1Space.HorizontalAlignment = Element.ALIGN_RIGHT;
                        total1Space.Colspan = 6;
                        total1Space.FixedHeight = 15f;
                        total1Space.Border = iTextSharp.text.Rectangle.BOX;
                        table1.AddCell(total1Space);


                        PdfPCell total1 = new PdfPCell(new Phrase("$" + decimal.Parse(Tot_FCounts.ToString()).ToString("N", CultureInfo.InvariantCulture), TableFont));
                        total1.HorizontalAlignment = Element.ALIGN_RIGHT;
                        total1.FixedHeight = 15f;
                        total1.Colspan = 2;
                        total1.Border = iTextSharp.text.Rectangle.LEFT_BORDER + iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER;
                        table1.AddCell(total1);

                        PdfPCell total1RSpace = new PdfPCell(new Phrase("", TableFont));
                        total1RSpace.HorizontalAlignment = Element.ALIGN_RIGHT;
                        total1RSpace.FixedHeight = 15f;
                        total1RSpace.Colspan = 3;
                        total1RSpace.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER;
                        table1.AddCell(total1RSpace);

                        PdfPCell totalR1 = new PdfPCell(new Phrase("$" + decimal.Parse(Tot_RCounts.ToString()).ToString("N", CultureInfo.InvariantCulture), TableFont));
                        totalR1.HorizontalAlignment = Element.ALIGN_RIGHT;
                        totalR1.FixedHeight = 15f;
                        totalR1.Colspan = 2;
                        totalR1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER;
                        table1.AddCell(totalR1);

                        PdfPCell total1PSpace = new PdfPCell(new Phrase("", TableFont));
                        total1PSpace.HorizontalAlignment = Element.ALIGN_RIGHT;
                        total1PSpace.FixedHeight = 15f;
                        total1PSpace.Colspan = 3;
                        total1PSpace.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER;
                        table1.AddCell(total1PSpace);

                        PdfPCell totalP1 = new PdfPCell(new Phrase("$" + decimal.Parse(Tot_PCounts.ToString()).ToString("N", CultureInfo.InvariantCulture), TableFont));
                        totalP1.HorizontalAlignment = Element.ALIGN_RIGHT;
                        totalP1.FixedHeight = 15f;
                        totalP1.Colspan = 2;
                        totalP1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER;
                        table1.AddCell(totalP1);

                        PdfPCell totalT1 = new PdfPCell(new Phrase("$" + decimal.Parse((Tot_PCounts + Tot_RCounts + Tot_FCounts).ToString()).ToString("N", CultureInfo.InvariantCulture), TableFont));
                        totalT1.HorizontalAlignment = Element.ALIGN_RIGHT;
                        totalT1.FixedHeight = 15f;
                        totalT1.Colspan = 18;
                        totalT1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER + iTextSharp.text.Rectangle.LEFT_BORDER;
                        table1.AddCell(totalT1);

                        if (table1.Rows.Count > 0)
                        {
                            document.Add(table1);
                            document.NewPage();
                        }

                        PdfPTable Last = new PdfPTable(6);
                        Last.TotalWidth = 200f;
                        Last.WidthPercentage = 100;
                        Last.LockedWidth = true;
                        float[] Lastwidths = new float[] { 15f, 18f, 15f, 15f, 15f, 15f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                        Last.SetWidths(Lastwidths);
                        Last.HorizontalAlignment = Element.ALIGN_LEFT;
                        if (App_list.Count > 0)
                        {
                            string[] app_header = { "SITE", "CLASS", "AMPM", "Free", "Red.", "Paid" };
                            for (int i = 0; i < app_header.Length; i++)
                            {
                                PdfPCell lastHeader = new PdfPCell(new Phrase(app_header[i], TblFontBold));
                                lastHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                                lastHeader.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                Last.AddCell(lastHeader);
                            }

                            foreach (HSSB2112ReportEntity RepEntity in App_list)
                            {
                                string[] data = { RepEntity.SITE.Trim(), RepEntity.ROOM.Trim(), RepEntity.AMPM.Trim() };
                                for (int i = 0; i < data.Length; i++)
                                {
                                    PdfPCell lastHeader1 = new PdfPCell(new Phrase(data[i], TableFont));
                                    lastHeader1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    lastHeader1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    Last.AddCell(lastHeader1);
                                }

                                if (rbFree.Checked == true)
                                {
                                    string[] datacount = { RepEntity.FREE.Trim(), "0", "0" };
                                    for (int i = 0; i < datacount.Length; i++)
                                    {
                                        PdfPCell lastHeader2 = new PdfPCell(new Phrase(datacount[i], TableFont));
                                        lastHeader2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        lastHeader2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        Last.AddCell(lastHeader2);
                                    }
                                }
                                else if (rbPaid.Checked == true)
                                {
                                    string[] datacount = { "0", "0", RepEntity.PAID.Trim() };
                                    for (int i = 0; i < datacount.Length; i++)
                                    {
                                        PdfPCell lastHeader2 = new PdfPCell(new Phrase(datacount[i], TableFont));
                                        lastHeader2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        lastHeader2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        Last.AddCell(lastHeader2);
                                    }
                                }
                                else if (rbReduced.Checked == true)
                                {
                                    string[] datacount = { "0", RepEntity.REDUCED.Trim(), "0" };
                                    for (int i = 0; i < datacount.Length; i++)
                                    {
                                        PdfPCell lastHeader2 = new PdfPCell(new Phrase(datacount[i], TableFont));
                                        lastHeader2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        lastHeader2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        Last.AddCell(lastHeader2);
                                    }
                                }
                                else
                                {
                                    string[] datacount = { RepEntity.FREE.Trim(), RepEntity.REDUCED.Trim(), RepEntity.PAID.Trim() };
                                    for (int i = 0; i < datacount.Length; i++)
                                    {
                                        PdfPCell lastHeader2 = new PdfPCell(new Phrase(datacount[i], TableFont));
                                        lastHeader2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        lastHeader2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        Last.AddCell(lastHeader2);
                                    }
                                }
                            }

                            if (Last.Rows.Count > 0)
                                document.Add(Last);
                        }

                    }
                }
                catch (Exception ex) { document.Add(new Paragraph("Aborted due to Exception............................................... ")); }

                document.Close();
                fs.Close();
                fs.Dispose();

                AlertBox.Show("Report Generated Successfully");
            }
        }

        private void PrintHeaderPage(Document document, PdfWriter writer)
        {

            /*BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/calibrib.ttf", BaseFont.WINANSI, BaseFont.EMBEDDED);
            BaseFont bfTimes = BaseFont.CreateFont("c:/windows/fonts/calibri.ttf", BaseFont.WINANSI, BaseFont.EMBEDDED);
            //BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, false);
            BaseFont bf_Times = BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1250, false);
            iTextSharp.text.Font fc = new iTextSharp.text.Font(bfTimes, 10, 2);
            iTextSharp.text.Font fc1 = new iTextSharp.text.Font(bf_Times, 10, 2, BaseColor.BLUE);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bfTimes, 10);
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_Times, 13);
            iTextSharp.text.Font TblFont = new iTextSharp.text.Font(bf_Times, 11);*/


            BaseFont bf_Calibri = BaseFont.CreateFont("c:/windows/fonts/calibri.ttf", BaseFont.WINANSI, BaseFont.EMBEDDED);
            BaseFont bf_TimesRomanI = BaseFont.CreateFont(BaseFont.TIMES_ITALIC, BaseFont.CP1250, false);
            BaseFont bf_Times = BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1250, false);
            iTextSharp.text.Font fc = new iTextSharp.text.Font(bf_Calibri, 10, 2);
            iTextSharp.text.Font fc1 = new iTextSharp.text.Font(bf_Times, 10, 2, BaseColor.BLUE);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_Calibri, 8);
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_Times, 8);
            iTextSharp.text.Font TblFont = new iTextSharp.text.Font(bf_Times, 8);
            iTextSharp.text.Font TblParamsHeaderFont = new iTextSharp.text.Font(bf_Calibri, 11, 1, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#2e5f71")));
            iTextSharp.text.Font TblHeaderTitleFont = new iTextSharp.text.Font(bf_Calibri, 14, 1, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#806000")));
            iTextSharp.text.Font fnttimesRoman_Italic = new iTextSharp.text.Font(bf_TimesRomanI, 9, iTextSharp.text.Font.NORMAL, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000")));

            HierarchyEntity hierarchyDetails = _model.HierarchyAndPrograms.GetCaseHierarchy("AGENCY", BaseForm.BaseAdminAgency, string.Empty, string.Empty, string.Empty, string.Empty);
            string _strImageFolderPath = "";
            if (hierarchyDetails != null)
            {
                string LogoName = hierarchyDetails.Logo.ToString();
                _strImageFolderPath = _model.lookupDataAccess.GetReportPath() + "\\AgencyLogos\\";
                FileInfo info = new FileInfo(_strImageFolderPath + LogoName);
                if (info.Exists)
                    _strImageFolderPath = _model.lookupDataAccess.GetReportPath() + "\\AgencyLogos\\" + LogoName;
                else
                    _strImageFolderPath = "";

            }

            PdfPTable Headertable = new PdfPTable(2);
            Headertable.TotalWidth = 510f;
            Headertable.WidthPercentage = 100;
            Headertable.LockedWidth = true;
            float[] Headerwidths = new float[] { 25f, 80f };
            Headertable.SetWidths(Headerwidths);
            Headertable.HorizontalAlignment = Element.ALIGN_CENTER;

            //border trails
            PdfContentByte content = writer.DirectContent;
            iTextSharp.text.Rectangle rectangle = new iTextSharp.text.Rectangle(document.PageSize);
            rectangle.Left += document.LeftMargin;
            rectangle.Right -= document.RightMargin;
            rectangle.Top -= document.TopMargin;
            rectangle.Bottom += document.BottomMargin;
            content.SetColorStroke(BaseColor.BLACK);
            content.Rectangle(rectangle.Left, rectangle.Bottom, rectangle.Width, rectangle.Height);
            content.Stroke();

            if (_strImageFolderPath != "")
            {
                iTextSharp.text.Image imgLogo = iTextSharp.text.Image.GetInstance(_strImageFolderPath);
                PdfPCell cellLogo = new PdfPCell(imgLogo);
                cellLogo.HorizontalAlignment = Element.ALIGN_CENTER;
                cellLogo.Colspan = 2;
                cellLogo.Padding = 5;
                cellLogo.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Headertable.AddCell(cellLogo);
            }

            PdfPCell row1 = new PdfPCell(new Phrase(Privileges.Program + " - " + Privileges.PrivilegeName, TblParamsHeaderFont));
            row1.HorizontalAlignment = Element.ALIGN_CENTER;
            row1.Colspan = 2;
            row1.PaddingBottom = 15;
            row1.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Headertable.AddCell(row1);

            /*PdfPCell row2 = new PdfPCell(new Phrase("Run By : " + LookupDataAccess.GetMemberName(BaseForm.UserProfile.FirstName.Trim(), BaseForm.UserProfile.MI.Trim(), BaseForm.UserProfile.LastName.Trim(), "3"), TableFont));
            row2.HorizontalAlignment = Element.ALIGN_LEFT;
            //row2.Colspan = 2;
            row2.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Headertable.AddCell(row2);

            PdfPCell row21 = new PdfPCell(new Phrase("Date : " + DateTime.Now.ToString(), TableFont));
            row21.HorizontalAlignment = Element.ALIGN_RIGHT;
            //row2.Colspan = 2;
            row21.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Headertable.AddCell(row21);*/

            PdfPCell row3 = new PdfPCell(new Phrase("Selected Report Parameters", TblHeaderTitleFont));
            row3.HorizontalAlignment = Element.ALIGN_CENTER;
            row3.VerticalAlignment = PdfPCell.ALIGN_TOP;
            row3.PaddingBottom = 5;
            row3.MinimumHeight = 6;
            row3.Colspan = 2;
            row3.Border = iTextSharp.text.Rectangle.NO_BORDER;
            row3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#b8e9fb"));
            Headertable.AddCell(row3);

            /*string Agy = "Agency : All"; string Dept = "Dept : All"; string Prg = "Program : All"; string Header_year = string.Empty;
            if (Agency != "**") Agy = "Agency : " + Agency;
            if (Depart != "**") Dept = "Dept : " + Depart;
            if (Program != "**") Prg = "Program : " + Program;
            if (CmbYear.Visible == true)
                Header_year = "Year : " + ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString();


            PdfPCell Hierarchy = new PdfPCell(new Phrase(Txt_HieDesc.Text.Trim() + "     " + Header_year, TableFont));
            Hierarchy.HorizontalAlignment = Element.ALIGN_LEFT;
            Hierarchy.Colspan = 2;
            Hierarchy.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Headertable.AddCell(Hierarchy);*/

            string Agy = /*"Agency : */"All"; string Dept = /*"Dept : */"All"; string Prg = /*"Program : */"All"; string Header_year = string.Empty;
            if (Agency != "**") Agy = /*"Agency : " +*/ Agency;
            if (Depart != "**") Dept = /*"Dept : " + */Depart;
            if (Program != "**") Prg = /*"Program : " +*/ Program;
            if (CmbYear.Visible == true)
                Header_year = "Year: " + ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString();

            PdfPCell Hierarchy = new PdfPCell(new Phrase("  " + "Hierarchy", TableFont));
            Hierarchy.HorizontalAlignment = Element.ALIGN_LEFT;
            Hierarchy.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            Hierarchy.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            Hierarchy.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            Hierarchy.PaddingBottom = 5;
            Headertable.AddCell(Hierarchy);

            if (CmbYear.Visible == true)
            {
                PdfPCell Hierarchy1 = new PdfPCell(new Phrase(Txt_HieDesc.Text.Trim() + "   " + Header_year, TableFont));
                Hierarchy1.HorizontalAlignment = Element.ALIGN_LEFT;
                Hierarchy1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                Hierarchy1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                Hierarchy1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                Hierarchy1.PaddingBottom = 5;
                Headertable.AddCell(Hierarchy1);
            }
            else
            {
                PdfPCell Hierarchy1 = new PdfPCell(new Phrase(Txt_HieDesc.Text.Trim(), TableFont));
                Hierarchy1.HorizontalAlignment = Element.ALIGN_LEFT;
                Hierarchy1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                Hierarchy1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                Hierarchy1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                Hierarchy1.PaddingBottom = 5;
                Headertable.AddCell(Hierarchy1);
            }

            string Site = string.Empty;
            if (rdoAllSite.Checked == true) Site = "All Sites";
            else
            {
                string Selsites = string.Empty;
                if (Sel_REFS_List.Count > 0)
                {
                    foreach (CaseSiteEntity Entity in Sel_REFS_List)
                    {
                        Selsites += Entity.SiteNUMBER + "/" + Entity.SiteROOM + "/" + Entity.SiteAM_PM + "  ";
                    }
                }
                Site = rdoMultipleSites.Text.Trim()+": ( " +Selsites+" )"; //"Site/Class/APF : "
            }

            PdfPCell R3 = new PdfPCell(new Phrase("  " + "Site", TableFont));
            R3.HorizontalAlignment = Element.ALIGN_LEFT;
            R3.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R3.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R3.PaddingBottom = 5;
            Headertable.AddCell(R3);

            PdfPCell R33 = new PdfPCell(new Phrase(Site, TableFont));
            R33.HorizontalAlignment = Element.ALIGN_LEFT;
            R33.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R33.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R33.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R33.PaddingBottom = 5;
            Headertable.AddCell(R33);

            string Meals = rbAllmeals.Text;
            if (rbFree.Checked == true) Meals = rbFree.Text.Trim(); else if (rbPaid.Checked == true) Meals = rbPaid.Text.Trim(); else if (rbReduced.Checked == true) Meals = rbReduced.Text;

            PdfPCell R1 = new PdfPCell(new Phrase("  " + lblMeals.Text.Trim()/*+" : "+Meals*/, TableFont));
            R1.HorizontalAlignment = Element.ALIGN_LEFT;
            R1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R1.PaddingBottom = 5;
            Headertable.AddCell(R1);

            PdfPCell R11 = new PdfPCell(new Phrase(Meals, TableFont));
            R11.HorizontalAlignment = Element.ALIGN_LEFT;
            R11.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R11.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R11.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R11.PaddingBottom = 5;
            Headertable.AddCell(R11);

            string Fund = string.Empty;
            if (rdoFundingAll.Checked == true)
                Fund = rdoFundingAll.Text.Trim();
            else
                Fund = rdoFundingSelect.Text.Trim()+": ( " +strFundingCodes+" )";

            PdfPCell R2 = new PdfPCell(new Phrase("  " + "Funding Source", TableFont));
            R2.HorizontalAlignment = Element.ALIGN_LEFT;
            R2.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R2.PaddingBottom = 5;
            Headertable.AddCell(R2);

            PdfPCell R22 = new PdfPCell(new Phrase(Fund, TableFont));
            R22.HorizontalAlignment = Element.ALIGN_LEFT;
            R22.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R22.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R22.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R22.PaddingBottom = 5;
            Headertable.AddCell(R22);

            PdfPCell R4 = new PdfPCell(new Phrase("  " + lblReportFormdt.Text.Trim()/*+" : " + dtForm.Text.Trim()+ "   "+lblToDate.Text.Trim()+" : "+dtTodate.Text.Trim()*/, TableFont));
            R4.HorizontalAlignment = Element.ALIGN_LEFT;
            R4.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R4.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R4.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R4.PaddingBottom = 5;
            Headertable.AddCell(R4);

            PdfPCell R44 = new PdfPCell(new Phrase(dtFrom.Text.Trim() + "   " + lblToDate.Text.Trim() + ": " + dtTodate.Text.Trim(), TableFont));
            R44.HorizontalAlignment = Element.ALIGN_LEFT;
            R44.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R44.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R44.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R44.PaddingBottom = 5;
            Headertable.AddCell(R44);

            document.Add(Headertable);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase("Generated By : ", fnttimesRoman_Italic), 33, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.GetMemberName(BaseForm.UserProfile.FirstName.Trim(), BaseForm.UserProfile.MI.Trim(), BaseForm.UserProfile.LastName.Trim(), "3"), fnttimesRoman_Italic), 90, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase("Generated On : ", fnttimesRoman_Italic), 410, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase(DateTime.Now.ToString(), fnttimesRoman_Italic), 468, 40, 0);

            //cb.BeginText();
            //cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ROMAN).BaseFont, 12);
            //cb.ShowTextAligned(100, "Program Name:", 30, 725, 0);
            //cb.ShowTextAligned(100, Privileges.Program + " - " + Privileges.PrivilegeName, 110, 725, 0);
            //cb.ShowTextAligned(100, "Run By : " + Privileges.UserID, 30, 705, 0);
            //cb.ShowTextAligned(100, "Module Desc : " + GetModuleDesc(), 30, 685, 0);
            //cb.ShowTextAligned(100, "Started : " + DateTime.Now.ToString(), 30, 665, 0);
            //cb.ShowTextAligned(100, "Report  Selection Criterion", 40, 635, 0);
            //string Header_year = string.Empty;
            //if (CmbYear.Visible == true)
            //    Header_year = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString();
            //cb.ShowTextAligned(100, "Hierarchy : " + Agency + "-" + Depart + "-" + Program + "   " + Header_year, 120, 610, 0);
            //string Meals = rbAllmeals.Text;
            ////cb.ShowTextAligned(100, "Report Type : " + Report, 120, 590, 0);
            //cb.ShowTextAligned(100, "Site : " + (rdoAllSite.Checked == true ? rdoAllSite.Text : rdoMultipleSites.Text), 120, 590, 0);
            //cb.ShowTextAligned(100, "Funding Sources : " + (rdoFundingAll.Checked == true ? rdoFundingAll.Text : rdoFundingSelect.Text), 300, 590, 0);
            //cb.ShowTextAligned(100, "Report From Date : " + dtForm.Text.Trim() + "  To Date: " + dtTodate.Text.Trim(), 120, 570, 0);
            //if (rbFree.Checked == true) Meals = rbFree.Text.Trim(); else if (rbPaid.Checked == true) Meals = rbPaid.Text.Trim(); else if (rbReduced.Checked == true) Meals = rbReduced.Text;
            //cb.ShowTextAligned(100, "Meals : " + Meals, 120, 550, 0);
            //string strReportType = string.Empty;
            //cb.SetFontAndSize(bfTimes, 12);
            //Y_Pos = 480;    // Y =  350

            //cb.EndText();

            //cb.SetLineWidth(0.7f);
            //cb.MoveTo(30f, 638f);
            //cb.LineTo(40f, 638f);

            //cb.LineTo(30f, 638f);
            //cb.LineTo(30f, 530);

            //cb.LineTo(30f, 530f);
            //cb.LineTo(530f, 530f);

            //cb.LineTo(530f, 638f);
            //cb.LineTo(530f, 530f);

            //cb.MoveTo(170f, 638f);
            //cb.LineTo(530f, 638f);


            //cb.Stroke();
        }

        private string GetModuleDesc()
        {
            string ModuleDesc = null;
            DataSet ds = Captain.DatabaseLayer.Lookups.GetModules();
            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["APPL_CODE"].ToString() == Privileges.ModuleCode)
                {
                    ModuleDesc = dr["APPL_DESCRIPTION"].ToString(); break;
                }
            }
            return ModuleDesc;
        }

        private void Pb_Help_Click(object sender, EventArgs e)
        {
            //Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "HSSB2112");
        }

    }
}