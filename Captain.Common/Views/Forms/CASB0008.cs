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
using XLSExportFile;
using CarlosAg.ExcelXmlWriter;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class CASB0008 : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        //private GridControl _intakeHierarchy = null;
        private bool boolChangeStatus = false;
        private CaptainModel _model = null;
        private List<HierarchyEntity> _selectedHierarchies = null;

        #endregion
        public CASB0008(BaseForm baseForm, PrivilegeEntity privileges)
        {
            InitializeComponent();

            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            BaseForm = baseForm; Privileges = privileges;
            Agency = BaseForm.BaseAgency; Depart = BaseForm.BaseDept; Program = BaseForm.BaseProg;
            strYear = BaseForm.BaseYear;
            Set_Report_Hierarchy(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear);
            propReportPath = _model.lookupDataAccess.GetReportPath();

            //dtpFrmDate.Focused = true;
            propCentralHie = string.Empty;
            DataSet dsAgency = Captain.DatabaseLayer.ADMNB001DB.ADMNB001_Browse_AGCYCNTL("00", null, null, null, null, null, null);
            if (dsAgency != null && dsAgency.Tables[0].Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(dsAgency.Tables[0].Rows[0]["ACR_CENT_HIE"].ToString().Trim()))
                    propCentralHie = dsAgency.Tables[0].Rows[0]["ACR_CENT_HIE"].ToString().Trim();
            }
            //if (!string.IsNullOrEmpty(BaseForm.BaseAgencyControlDetails.CentralHierarchy.Trim()))
            //    propCentralHie = BaseForm.BaseAgencyControlDetails.CentralHierarchy.Trim();

            this.Text = Privileges.PrivilegeName.Trim();/*Privileges.Program + " - " + Privileges.PrivilegeName.Trim();*/
            fillcombo(); //cmbCaseType.SelectedIndex = 0;  //Vikash added

        }
        #region Vikash added
        void fillcombo()
        {
            cmbCaseType.Items.Clear();
            cmbCaseType.ColorMember = "FavoriteColor";
            List<CommonEntity> CaseType = _model.lookupDataAccess.GetCaseType();
            CaseType = CaseType.OrderByDescending(u => u.Active.Trim()).ToList();
            foreach (CommonEntity casetype in CaseType)
            {
                cmbCaseType.Items.Add(new Captain.Common.Utilities.ListItem(casetype.Desc, casetype.Code, casetype.Active, casetype.Active.Equals("Y") ? Color.Black : Color.Red));
            }
            cmbCaseType.Items.Insert(0, new Captain.Common.Utilities.ListItem("All", "**"));
            cmbCaseType.SelectedIndex = 0;
        }

        private void cmbCaseType_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvwApplicants.Rows.Clear();
        }

        #endregion

        string Agency = string.Empty, Depart = string.Empty, Program = string.Empty, strYear = string.Empty;

        #region properties

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileges { get; set; }

      
        public string propReportPath { get; set; }

        public string propCentralHie { get; set; }


        public List<CaseSiteEntity> propCaseSiteEntity { get; set; }

        public List<CommonEntity> propfundingSource { get; set; }

        #endregion

        string Img_Blank = "blank";
        string Img_Tick = "icon-gridtick"; //tick-icon10.png";

        //string Img_Blank = Consts.Icons.ico_Blank;
        //string Img_Tick = new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("tick-icon10.png");

        public bool IsSaveValid { get; set; }
        private bool ValidateForm()
        {
            bool isValid = true;
            if (string.IsNullOrWhiteSpace(dtpFrmDate.Text))
            {
                _errorProvider.SetError(dtpFrmDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblDate.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtpFrmDate, null);
            }

            if (string.IsNullOrWhiteSpace(dtpToDt.Text))
            {
                _errorProvider.SetError(dtpToDt, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Intake " + lblToDt.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtpToDt, null);
            }

            if (Convert.ToDateTime(dtpFrmDate.Value.ToShortDateString()) > Convert.ToDateTime(dtpToDt.Value.ToShortDateString()))
            {
                _errorProvider.SetError(dtpToDt, "Intake End Date should be greater than Start Date ".Replace(Consts.Common.Colon, string.Empty));
                isValid = false;
            }
            IsSaveValid = isValid;
            return isValid;
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
                FillYearCombo(Agy, Dept, Prog, Year);
            else
            {
                this.pnlHie.Size = new Size(778, 25);//this.Size = new Size(778, 450) ;  //this.Txt_HieDesc.Size = new System.Drawing.Size(758, 33);
                Agency = Agy; Depart = Dept; Program = Prog; //strYear = Year;
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
            {
                //this.Size = new Size(699, 450);
                this.pnlHie.Size = new Size(699, 25);//this.Txt_HieDesc.Size = new System.Drawing.Size(758, 33);
            }
            else
            {
               // this.Size = new Size(770, 450);
                this.pnlHie.Size = new Size(778, 25);//this.Txt_HieDesc.Size = new System.Drawing.Size(699, 33);
            }
        }

        private void CmbYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program_Year = "    ";
            if (!(string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString())))
                Program_Year = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString();
        }

        private void btnSaveParameters_Click(object sender, EventArgs e)
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
            //string Report = ((Captain.Common.Utilities.ListItem)cmbReport.SelectedItem).Value.ToString();
            //string Seq = ((Captain.Common.Utilities.ListItem)cmbSeq.SelectedItem).Value.ToString();

            //string strSite = rdoAllSite.Checked == true ? "A" : "M";
            //string strFundSource = string.Empty;
            //string strenrl = string.Empty; string Pick = txtDesc.Text;
            //strenrl = ((Captain.Common.Utilities.ListItem)cmbEntlStat.SelectedItem).Value.ToString();

            //string strBaseAge = rdoTodayDate.Checked ? "T" : "K";
            //string Repeaters = string.Empty;
            //if (rbBoth.Checked) Repeaters = "B"; else if (rbRepeaters.Checked) Repeaters = "R"; else if (rbNonRepeat.Checked) Repeaters = "N";
            //string labels = rbNoLbl.Checked ? "N" : "M";
            //string PrintLbls = rbParent.Checked ? "P" : "C";

            //if (rbFundAll.Checked) strFundSource = "A"; else if (rbFundSel.Checked) strFundSource = "P";
            //string strsiteRoomNames = string.Empty;
            //if (rdoMultipleSites.Checked == true)
            //{
            //    foreach (CaseSiteEntity siteRoom in Sel_REFS_List)
            //    {
            //        if (!strsiteRoomNames.Equals(string.Empty)) strsiteRoomNames += ",";
            //        strsiteRoomNames += siteRoom.SiteNUMBER + siteRoom.SiteROOM + siteRoom.SiteAM_PM;
            //    }
            //}


            //string strFundingCodes = string.Empty;
            //if (rbFundSel.Checked == true)
            //{
            //    foreach (CommonEntity FundingCode in SelFundingList)
            //    {
            //        if (!strFundingCodes.Equals(string.Empty)) strFundingCodes += ",";
            //        strFundingCodes += FundingCode.Code;
            //    }
            //}

            StringBuilder str = new StringBuilder();
            str.Append("<Rows>");
            str.Append("<Row AGENCY = \"" + Agency + "\" DEPT = \"" + Depart + "\" PROG = \"" + Program +
                            "\" YEAR = \"" + Program_Year + "\" FromDate = \"" + dtpFrmDate.Value.ToShortDateString() + "\" ToDate = \"" + dtpToDt.Value.ToShortDateString() +
                            "\" CMBCASETYPE = \"" + ((Captain.Common.Utilities.ListItem)cmbCaseType.SelectedItem).Value.ToString() + 
                            "\"  />");
            str.Append("</Rows>");

            return str.ToString();
        }

        private void Set_Report_Controls(DataTable Tmp_Table)
        {
            if (Tmp_Table != null && Tmp_Table.Rows.Count > 0)
            {
                DataRow dr = Tmp_Table.Rows[0];

                Set_Report_Hierarchy(dr["AGENCY"].ToString(), dr["DEPT"].ToString(), dr["PROG"].ToString(), dr["YEAR"].ToString());

                dtpFrmDate.Text = dr["FromDate"].ToString();
                dtpToDt.Text = dr["ToDate"].ToString();
                CommonFunctions.SetComboBoxValue(cmbCaseType, dr["CMBCASETYPE"].ToString().Trim());
                //CommonFunctions.SetComboBoxValue(cmbReport, dr["ReportType"].ToString());
                //CommonFunctions.SetComboBoxValue(cmbSeq, dr["Sequence"].ToString());
                //CommonFunctions.SetComboBoxValue(cmbEntlStat, dr["Enroll"].ToString());
                //txtDesc.Text = dr["Desc"].ToString().Trim();

                //if (dr["Site"].ToString() == "A")
                //    rdoAllSite.Checked = true;
                //else
                //{
                //    rdoMultipleSites.Checked = true;
                //    CaseSiteEntity Search_Entity = new CaseSiteEntity(true);
                //    Search_Entity.SiteAGENCY = Agency; Search_Entity.SiteDEPT = Depart;
                //    Search_Entity.SitePROG = Program; Search_Entity.SiteYEAR = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString().Trim();
                //    propCaseSiteEntity = _model.CaseMstData.Browse_CASESITE(Search_Entity, "Browse");
                //    propCaseSiteEntity = propCaseSiteEntity.FindAll(u => u.SiteROOM.Trim() != "0000");
                //    Sel_REFS_List.Clear();
                //    string strSites = dr["SiteNames"].ToString();
                //    List<string> siteList = new List<string>();
                //    if (strSites != null)
                //    {
                //        string[] sitesRooms = strSites.Split(',');
                //        for (int i = 0; i < sitesRooms.Length; i++)
                //        {
                //            CaseSiteEntity siteDetails = propCaseSiteEntity.Find(u => u.SiteNUMBER + u.SiteROOM + u.SiteAM_PM == sitesRooms.GetValue(i).ToString());
                //            Sel_REFS_List.Add(siteDetails);
                //        }
                //    }
                //}
                //Sel_REFS_List = Sel_REFS_List;

                //if (dr["FundedSource"].ToString() == "A")
                //    rbFundAll.Checked = true;
                //else
                //{
                //    rbFundSel.Checked = true;
                //    SelFundingList.Clear();
                //    string strFunds = dr["FundingCode"].ToString();
                //    List<string> siteList1 = new List<string>();
                //    if (strFunds != null)
                //    {
                //        string[] FundCodes = strFunds.Split(',');
                //        for (int i = 0; i < FundCodes.Length; i++)
                //        {
                //            CommonEntity fundDetails = propfundingSource.Find(u => u.Code == FundCodes.GetValue(i).ToString());
                //            SelFundingList.Add(fundDetails);
                //        }
                //    }
                //    SelFundingList = SelFundingList;
                //}

                //if (dr["BaseAge"].ToString().Trim() == "T")
                //    rdoTodayDate.Checked = true;
                //else
                //    rdoKindergartenDate.Checked = true;

                //if (dr["Repeat"].ToString().Trim() == "R")
                //    rbRepeaters.Checked = true;
                //else if (dr["Repeat"].ToString().Trim() == "N")
                //    rbNonRepeat.Checked = true;
                //else if (dr["Repeat"].ToString().Trim() == "B")
                //    rbBoth.Checked = true;

                //if (dr["Labels"].ToString().Trim() == "N")
                //    rbNoLbl.Checked = true;
                //else
                //    rbMailLbl.Checked = true;

                //if (dr["PrintLabels"].ToString().Trim() == "C")
                //    rbChild.Checked = true;
                //else
                //    rbParent.Checked = true;

            }
        }


        private void Pb_Search_Hie_Click(object sender, EventArgs e)
        {
            //HierarchieSelectionFormNew hierarchieSelectionForm = new HierarchieSelectionFormNew(BaseForm, Current_Hierarchy_DB, "Master", " ", "D", "Reports");
            //hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
            //hierarchieSelectionForm.StartPosition = FormStartPosition.CenterScreen;
            //hierarchieSelectionForm.ShowDialog();


            HierarchieSelection hierarchieSelectionForm = new HierarchieSelection(BaseForm, Current_Hierarchy_DB, "Master", " ", "D", "Reports", BaseForm.UserID);
            hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
            hierarchieSelectionForm.StartPosition = FormStartPosition.CenterScreen;
            hierarchieSelectionForm.ShowDialog();
        }

        string Current_Hierarchy = "******", Current_Hierarchy_DB = "**-**-**";
        private void OnHierarchieFormClosed(object sender, FormClosedEventArgs e)
        {
            //HierarchieSelectionFormNew form = sender as HierarchieSelectionFormNew;

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
                    propReportPath = _model.lookupDataAccess.GetReportPath();
                }
            }
        }

        string Member_NameFormat = "1", CAseWorkerr_NameFormat = "1";

        private void btnProcess_Click(object sender, EventArgs e)
        {
            _errorProvider.SetError(dtpToDt,string.Empty);
            try
            {
                if (ValidateForm())
                    fillGrid();
            }
            catch(Exception ex) { };
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            gvwApplicants.Rows.Clear();
            dtpFrmDate.Value = dtpToDt.Value = DateTime.Now.Date;
            cmbCaseType.SelectedIndex = 0;
        }

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

        private void btnGetParameters_Click(object sender, EventArgs e)
        {
            ControlCard_Entity Save_Entity = new ControlCard_Entity(true);
            Save_Entity.Scr_Code = Privileges.Program;
            Save_Entity.UserID = BaseForm.UserID;
            Save_Entity.Module = BaseForm.BusinessModuleID;
            Report_Get_SaveParams_Form Save_Form = new Report_Get_SaveParams_Form(Save_Entity, "Get");
            Save_Form.FormClosed += new FormClosedEventHandler(Get_Saved_Parameters);
            Save_Form.StartPosition = FormStartPosition.CenterScreen;
            Save_Form.ShowDialog();
        }

        DataTable dtSummary = new DataTable();
        DataTable dtDet = new DataTable();
        private void BtnGenPdf_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateForm())
                {
                    string Year = string.Empty;string casetype = string.Empty;
                    if (CmbYear.Visible == true)
                        Year = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString().Trim();

                    //string Curr_Prog = "N";
                    //if (chkbCurrProg.Checked) Curr_Prog = "Y";
                    if (((Captain.Common.Utilities.ListItem)cmbCaseType.SelectedItem).Value.ToString() != "**")
                        casetype = ((Captain.Common.Utilities.ListItem)cmbCaseType.SelectedItem).Value.ToString();
                    DataSet ds = DatabaseLayer.CaseSnpData.GetCase0008_REPORT(Agency, Depart, Program, Year, dtpFrmDate.Value.ToShortDateString(), dtpToDt.Value.ToShortDateString(), casetype);
                        dtSummary = ds.Tables[0];
                        dtDet = ds.Tables[1];

                    if (dtDet.Rows.Count > 0)
                    {
                        PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, false, propReportPath,"XLS");
                        pdfListForm.FormClosed += new FormClosedEventHandler(OnExcel_Report4);
                        pdfListForm.StartPosition = FormStartPosition.CenterScreen;
                        pdfListForm.ShowDialog();
                    }
                    else
                        AlertBox.Show("No Data Found",MessageBoxIcon.Warning);
                    }            
            }
            catch(Exception ex) { };
        }

        private void BtnPdfPrev_Click(object sender, EventArgs e)
        {
            PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, true, propReportPath);
            pdfListForm.StartPosition = FormStartPosition.CenterScreen;
            pdfListForm.ShowDialog();
        }

        private void fillGrid()
        {
            int rowindex = 0; string casetype = string.Empty;
            string Year = string.Empty;
            if (CmbYear.Visible == true)
                Year = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString().Trim();

            string Curr_Prog = "N";
            //if (chkbCurrProg.Checked) Curr_Prog = "Y";
            if (((Captain.Common.Utilities.ListItem)cmbCaseType.SelectedItem).Value.ToString() != "**")
                casetype = ((Captain.Common.Utilities.ListItem)cmbCaseType.SelectedItem).Value.ToString();

            DataSet ds = DatabaseLayer.CaseSnpData.GetCase0008_REPORT(Agency, Depart, Program, Year, dtpFrmDate.Value.ToShortDateString(), dtpToDt.Value.ToShortDateString(), casetype);
            DataTable dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                gvwApplicants.Rows.Clear();
                int i = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    i++;
                    rowindex = gvwApplicants.Rows.Add(true, dr["DESCRIPTON"].ToString(), dr["YES_COUNT"].ToString(), dr["NO_COUNT"].ToString(), "Y", i.ToString());//Img_Tick
                    rowindex++;
                }

            }

            if (gvwApplicants.SelectedRows.Count > 0)
            { gvwApplicants.Rows[rowindex].Selected = true; }
            else
            { gvwApplicants.Rows[0].Selected = true; }
        }

        private void gvwApplicants_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (gvwApplicants.Rows.Count > 0)
            {
                if (e.RowIndex > -1)
                {
                    if (e.ColumnIndex == 0)
                    {
                        if (gvwApplicants.CurrentRow.Cells["Selected"].Value.ToString() == "Y")
                        {
                            gvwApplicants.CurrentRow.Cells["SCheck1"].Value = false;//Img_Blank;
                            gvwApplicants.CurrentRow.Cells["Selected"].Value = "N";
                        }
                        else
                        {
                            gvwApplicants.CurrentRow.Cells["SCheck1"].Value = true;//Img_Tick;
                            gvwApplicants.CurrentRow.Cells["Selected"].Value = "Y";
                        }
                    }
                }
            }
        }

        #region ExcelReportFormat

        string strFolderPath = string.Empty;
        string Random_Filename = null; string PdfName = "Pdf File";

        private void CASB0008_ToolClick(object sender, ToolClickEventArgs e)
        {
            Application.Navigate(CommonFunctions.CreateZenHelps(Privileges.Program, 0, BaseForm.BusinessModuleID.ToString(), "", ""), target: "_blank");
        }

        private void OnExcel_Report(object sender, FormClosedEventArgs e)
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
                    {
                        DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim());
                    }
                }
                catch (Exception ex)
                {
                    AlertBox.Show("Error", MessageBoxIcon.Error);
                }


                try
                {
                    string Tmpstr = PdfName + ".xls";
                    if (File.Exists(Tmpstr))
                        File.Delete(Tmpstr);
                }
                catch (Exception ex)
                {
                    int length = 8;
                    string newFileName = System.Guid.NewGuid().ToString();
                    newFileName = newFileName.Replace("-", string.Empty);

                    Random_Filename = PdfName + newFileName.Substring(0, length) + ".xls";
                }


                if (!string.IsNullOrEmpty(Random_Filename))
                    PdfName = Random_Filename;
                else
                    PdfName += ".xls";

                //Workbook book = new Workbook();

                //this.GenerateStyles(book.Styles);

                ExcelDocument xlWorkSheet = new ExcelDocument();

                if (dtSummary.Rows.Count == 12)
                {
                    xlWorkSheet.ColumnWidth(0, 200);
                    xlWorkSheet.ColumnWidth(1, 100);
                    xlWorkSheet.ColumnWidth(2, 80);
                    xlWorkSheet.ColumnWidth(3, 0);
                    xlWorkSheet.ColumnWidth(4, 120);
                    xlWorkSheet.ColumnWidth(5, 120);
                    xlWorkSheet.ColumnWidth(6, 210);
                    xlWorkSheet.ColumnWidth(7, 210);
                    xlWorkSheet.ColumnWidth(8, 220);
                    xlWorkSheet.ColumnWidth(9, 120);
                    xlWorkSheet.ColumnWidth(10, 120);
                    xlWorkSheet.ColumnWidth(11, 150);
                    xlWorkSheet.ColumnWidth(12, 150);
                    xlWorkSheet.ColumnWidth(13, 150);
                    xlWorkSheet.ColumnWidth(14, 150);
                    xlWorkSheet.ColumnWidth(15, 150);
                    xlWorkSheet.ColumnWidth(16, 150);
                    xlWorkSheet.ColumnWidth(17, 120);
                    xlWorkSheet.ColumnWidth(18, 120);
                    xlWorkSheet.ColumnWidth(19, 180);
                    //xlWorkSheet.ColumnWidth(20, 120);
                    //xlWorkSheet.ColumnWidth(21, 120);
                    //xlWorkSheet.ColumnWidth(21, 120);
                }
                else if (dtSummary.Rows.Count >12)
                {
                    xlWorkSheet.ColumnWidth(0, 200);
                    xlWorkSheet.ColumnWidth(1, 100);
                    xlWorkSheet.ColumnWidth(2, 80);
                    xlWorkSheet.ColumnWidth(3, 80);
                    xlWorkSheet.ColumnWidth(4, 120);
                    xlWorkSheet.ColumnWidth(5, 120);
                    xlWorkSheet.ColumnWidth(6, 210);
                    xlWorkSheet.ColumnWidth(7, 210);
                    xlWorkSheet.ColumnWidth(8, 220);
                    xlWorkSheet.ColumnWidth(9, 120);
                    xlWorkSheet.ColumnWidth(10, 120);
                    xlWorkSheet.ColumnWidth(11, 150);
                    xlWorkSheet.ColumnWidth(12, 150);
                    xlWorkSheet.ColumnWidth(13, 150);
                    xlWorkSheet.ColumnWidth(14, 150);
                    xlWorkSheet.ColumnWidth(15, 150);
                    xlWorkSheet.ColumnWidth(16, 150);
                    xlWorkSheet.ColumnWidth(17, 120);
                    xlWorkSheet.ColumnWidth(18, 120);
                    xlWorkSheet.ColumnWidth(19, 180);
                    //xlWorkSheet.ColumnWidth(20, 120);
                    //xlWorkSheet.ColumnWidth(21, 120);
                    //xlWorkSheet.ColumnWidth(22, 120);

                }


                try
                {
                    int excelcolumn = 0;

                    xlWorkSheet[excelcolumn, 5].Font = new System.Drawing.Font("Tahoma", 12, System.Drawing.FontStyle.Bold);
                    xlWorkSheet[excelcolumn, 5].Alignment = Alignment.Centered;
                    xlWorkSheet[excelcolumn, 5].ForeColor = ExcelColor.Blue;
                    xlWorkSheet.WriteCell(excelcolumn, 5, "Customer Intake Quality Control Report");

                    excelcolumn = excelcolumn + 2;


                    xlWorkSheet[excelcolumn, 0].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    xlWorkSheet[excelcolumn, 0].Alignment = Alignment.Centered;
                    xlWorkSheet.WriteCell(excelcolumn, 0, "Name (first and last)");

                    xlWorkSheet[excelcolumn, 1].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    xlWorkSheet[excelcolumn, 1].Alignment = Alignment.Centered;
                    xlWorkSheet.WriteCell(excelcolumn, 1, "Program App #");

                    xlWorkSheet[excelcolumn, 2].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    xlWorkSheet[excelcolumn, 2].Alignment = Alignment.Centered;
                    xlWorkSheet.WriteCell(excelcolumn, 2, "Central");

                    if (dtSummary.Rows.Count > 12)
                    {
                        xlWorkSheet[excelcolumn, 3].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                        xlWorkSheet[excelcolumn, 3].Alignment = Alignment.Centered;
                        xlWorkSheet.WriteCell(excelcolumn, 3, "SIM");
                    }
                    else if (dtSummary.Rows.Count == 12)
                    {
                        xlWorkSheet[excelcolumn, 3].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                        xlWorkSheet[excelcolumn, 3].Alignment = Alignment.Centered;
                        xlWorkSheet.WriteCell(excelcolumn, 3, "");
                    }

                    xlWorkSheet[excelcolumn, 4].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    xlWorkSheet[excelcolumn, 4].Alignment = Alignment.Centered;
                    xlWorkSheet.WriteCell(excelcolumn, 4, "Program Matrix");

                    xlWorkSheet[excelcolumn, 5].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    xlWorkSheet[excelcolumn, 5].Alignment = Alignment.Centered;
                    xlWorkSheet.WriteCell(excelcolumn, 5, "Central Matrix");

                    xlWorkSheet[excelcolumn, 6].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    xlWorkSheet[excelcolumn, 6].Alignment = Alignment.Centered;
                    xlWorkSheet.WriteCell(excelcolumn, 6, "Income Verification - Program");

                    xlWorkSheet[excelcolumn, 7].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    xlWorkSheet[excelcolumn, 7].Alignment = Alignment.Centered;
                    xlWorkSheet.WriteCell(excelcolumn, 7, "Income Verification - Central");

                    xlWorkSheet[excelcolumn, 8].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    xlWorkSheet[excelcolumn, 8].Alignment = Alignment.Centered;
                    xlWorkSheet.WriteCell(excelcolumn, 8, "Required Intake Fields - Program");

                    xlWorkSheet[excelcolumn, 9].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    xlWorkSheet[excelcolumn, 9].Alignment = Alignment.Centered;
                    xlWorkSheet.WriteCell(excelcolumn, 9, "SP In Program");

                    xlWorkSheet[excelcolumn, 10].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    xlWorkSheet[excelcolumn, 10].Alignment = Alignment.Centered;
                    xlWorkSheet.WriteCell(excelcolumn, 10, "SP in Central");

                    xlWorkSheet[excelcolumn, 11].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    xlWorkSheet[excelcolumn, 11].Alignment = Alignment.Centered;
                    xlWorkSheet.WriteCell(excelcolumn, 11, "Service in Program");

                    xlWorkSheet[excelcolumn, 12].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    xlWorkSheet[excelcolumn, 12].Alignment = Alignment.Centered;
                    xlWorkSheet.WriteCell(excelcolumn, 12, "Service in Central");

                    xlWorkSheet[excelcolumn, 13].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    xlWorkSheet[excelcolumn, 13].Alignment = Alignment.Centered;
                    xlWorkSheet.WriteCell(excelcolumn, 13, "Outcome in Program");

                    xlWorkSheet[excelcolumn, 14].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    xlWorkSheet[excelcolumn, 14].Alignment = Alignment.Centered;
                    xlWorkSheet.WriteCell(excelcolumn, 14, "Outcome in Central");

                    xlWorkSheet[excelcolumn, 15].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    xlWorkSheet[excelcolumn, 15].Alignment = Alignment.Centered;
                    xlWorkSheet.WriteCell(excelcolumn, 15, "Add Operator in Program");

                    xlWorkSheet[excelcolumn, 16].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    xlWorkSheet[excelcolumn, 16].Alignment = Alignment.Centered;
                    xlWorkSheet.WriteCell(excelcolumn, 16, "Add Operator in Central");

                    xlWorkSheet[excelcolumn, 17].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    xlWorkSheet[excelcolumn, 17].Alignment = Alignment.Centered;
                    xlWorkSheet.WriteCell(excelcolumn, 17, "Intake Date – Central");

                    xlWorkSheet[excelcolumn, 18].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    xlWorkSheet[excelcolumn, 18].Alignment = Alignment.Centered;
                    xlWorkSheet.WriteCell(excelcolumn, 18, "Intake Date – Program");

                    xlWorkSheet[excelcolumn, 19].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    xlWorkSheet[excelcolumn, 19].Alignment = Alignment.Centered;
                    xlWorkSheet.WriteCell(excelcolumn, 19, "Program Enroll Status");


                    if (dtDet.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtDet.Rows)
                        {
                            excelcolumn = excelcolumn + 1;
                            xlWorkSheet.WriteCell(excelcolumn, 0, dr["FIRST_NAME"].ToString() + " " + dr["LAST_NAME"].ToString());
                            xlWorkSheet.WriteCell(excelcolumn, 1, dr["MST_APP_NO"].ToString());
                            xlWorkSheet.WriteCell(excelcolumn, 2, dr["CENTRAL_INTAKE"].ToString());
                            if (dtSummary.Rows.Count > 12)
                                xlWorkSheet.WriteCell(excelcolumn, 3, dr["CENTRAL_SIM"].ToString());
                            else if (dtSummary.Rows.Count == 12)
                                xlWorkSheet.WriteCell(excelcolumn, 3, dr["CENTRAL_SIM"].ToString());

                            xlWorkSheet.WriteCell(excelcolumn, 4, dr["PROG_MATRIX"].ToString());
                            xlWorkSheet.WriteCell(excelcolumn, 5, dr["CENTRAL_MATRIX"].ToString());

                            xlWorkSheet.WriteCell(excelcolumn, 6, dr["PROG_VERIFICATION"].ToString());
                            xlWorkSheet.WriteCell(excelcolumn, 7, dr["CENTRAL_VERIFICATION"].ToString());

                            xlWorkSheet.WriteCell(excelcolumn, 8, dr["REQ_FLDS"].ToString());

                            xlWorkSheet.WriteCell(excelcolumn, 9, dr["PROG_SPM"].ToString());
                            xlWorkSheet.WriteCell(excelcolumn, 10, dr["CENTRAL_SPM"].ToString());

                            xlWorkSheet.WriteCell(excelcolumn, 11, dr["PROG_CASEACT"].ToString());
                            xlWorkSheet.WriteCell(excelcolumn, 12, dr["CENTRAL_CASEACT"].ToString());

                            xlWorkSheet.WriteCell(excelcolumn, 13, dr["PROG_CASEMS"].ToString());
                            xlWorkSheet.WriteCell(excelcolumn, 14, dr["CENTRAL_CASEMS"].ToString());

                            xlWorkSheet.WriteCell(excelcolumn, 15, dr["ADD_OPERATOR"].ToString());
                            xlWorkSheet.WriteCell(excelcolumn, 16, dr["CENTRAL_ADD_OPERATOR"].ToString());

                            xlWorkSheet.WriteCell(excelcolumn, 17, LookupDataAccess.Getdate(dr["INTAKE_DATE"].ToString()));
                            xlWorkSheet.WriteCell(excelcolumn, 18, LookupDataAccess.Getdate(dr["CENTRAL_INTAKE_DATE"].ToString()));

                            string Status_Desc = string.Empty;

                            switch(dr["PROG_ENRL_STATUS"].ToString().Trim())
                            {
                                case "L": Status_Desc = "Wait List"; break;
                                case "P": Status_Desc = "Pending"; break;
                                //case "R": Status_Desc = "Denied"; break;
                                case "E": Status_Desc = "Enrolled"; break;
                                case "W": Status_Desc = "Withdrawn"; break;
                                case "I": Status_Desc = "Post Intake"; break;
                                case "N": Status_Desc = "Inactive"; break;
                                case "C": Status_Desc = "Accepted"; break;
                            }

                            xlWorkSheet.WriteCell(excelcolumn, 19, Status_Desc);

                        }

                    }

                    FileStream stream = new FileStream(PdfName, FileMode.Create);

                    xlWorkSheet.Save(stream);
                    stream.Close();


                }
                catch (Exception ex) { }

            }
        }


        private void OnExcel_Report1(object sender, FormClosedEventArgs e)
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
                    {
                        DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim());
                    }
                }
                catch (Exception ex)
                {
                    AlertBox.Show("Error", MessageBoxIcon.Error);
                }


                try
                {
                    string Tmpstr = PdfName + ".xls";
                    if (File.Exists(Tmpstr))
                        File.Delete(Tmpstr);
                }
                catch (Exception ex)
                {
                    int length = 8;
                    string newFileName = System.Guid.NewGuid().ToString();
                    newFileName = newFileName.Replace("-", string.Empty);

                    Random_Filename = PdfName + newFileName.Substring(0, length) + ".xls";
                }


                if (!string.IsNullOrEmpty(Random_Filename))
                    PdfName = Random_Filename;
                else
                    PdfName += ".xls";

                Workbook book = new Workbook();

                Worksheet sheet = book.Worksheets.Add("Sheet1");
                //this.GenerateStyles(book.Styles);

                //ExcelDocument xlWorkSheet = new ExcelDocument();

                if (dtSummary.Rows.Count == 12)
                {
                    sheet.Table.Columns.Add(new WorksheetColumn(200));
                    sheet.Table.Columns.Add(new WorksheetColumn(100));
                    sheet.Table.Columns.Add(new WorksheetColumn(100));
                    sheet.Table.Columns.Add(new WorksheetColumn(80));
                    sheet.Table.Columns.Add(new WorksheetColumn(0));
                    sheet.Table.Columns.Add(new WorksheetColumn(120));
                    sheet.Table.Columns.Add(new WorksheetColumn(120));
                    sheet.Table.Columns.Add(new WorksheetColumn(210));
                    sheet.Table.Columns.Add(new WorksheetColumn(210));
                    sheet.Table.Columns.Add(new WorksheetColumn(220));
                    sheet.Table.Columns.Add(new WorksheetColumn(120));
                    sheet.Table.Columns.Add(new WorksheetColumn(120));
                    sheet.Table.Columns.Add(new WorksheetColumn(150));
                    sheet.Table.Columns.Add(new WorksheetColumn(150));
                    sheet.Table.Columns.Add(new WorksheetColumn(150));
                    sheet.Table.Columns.Add(new WorksheetColumn(150));
                    sheet.Table.Columns.Add(new WorksheetColumn(150));
                    sheet.Table.Columns.Add(new WorksheetColumn(150));
                    sheet.Table.Columns.Add(new WorksheetColumn(120));
                    sheet.Table.Columns.Add(new WorksheetColumn(120));
                    sheet.Table.Columns.Add(new WorksheetColumn(180));
                    //xlWorkSheet.ColumnWidth(20, 120);
                    //xlWorkSheet.ColumnWidth(21, 120);
                    //xlWorkSheet.ColumnWidth(21, 120);
                }
                else if (dtSummary.Rows.Count > 12)
                {
                    sheet.Table.Columns.Add(new WorksheetColumn(200));
                    sheet.Table.Columns.Add(new WorksheetColumn(100));
                    sheet.Table.Columns.Add(new WorksheetColumn(100));
                    sheet.Table.Columns.Add(new WorksheetColumn(80));
                    sheet.Table.Columns.Add(new WorksheetColumn(80));
                    sheet.Table.Columns.Add(new WorksheetColumn(120));
                    sheet.Table.Columns.Add(new WorksheetColumn(120));
                    sheet.Table.Columns.Add(new WorksheetColumn(210));
                    sheet.Table.Columns.Add(new WorksheetColumn(210));
                    sheet.Table.Columns.Add(new WorksheetColumn(220));
                    sheet.Table.Columns.Add(new WorksheetColumn(120));
                    sheet.Table.Columns.Add(new WorksheetColumn(120));
                    sheet.Table.Columns.Add(new WorksheetColumn(150));
                    sheet.Table.Columns.Add(new WorksheetColumn(150));
                    sheet.Table.Columns.Add(new WorksheetColumn(150));
                    sheet.Table.Columns.Add(new WorksheetColumn(150));
                    sheet.Table.Columns.Add(new WorksheetColumn(150));
                    sheet.Table.Columns.Add(new WorksheetColumn(150));
                    sheet.Table.Columns.Add(new WorksheetColumn(120));
                    sheet.Table.Columns.Add(new WorksheetColumn(120));
                    sheet.Table.Columns.Add(new WorksheetColumn(180));
                    
                    //xlWorkSheet.ColumnWidth(20, 120);
                    //xlWorkSheet.ColumnWidth(21, 120);
                    //xlWorkSheet.ColumnWidth(22, 120);

                }

                WorksheetStyle styleb = book.Styles.Add("HeaderStyleBlue");
                styleb.Font.FontName = "Tahoma";
                styleb.Font.Size = 12;
                styleb.Font.Bold = true;
                styleb.Font.Color = "#0000FF";
                styleb.Alignment.Horizontal = StyleHorizontalAlignment.Center;


                WorksheetStyle style = book.Styles.Add("HeaderStyle");
                style.Font.FontName = "Tahoma";
                style.Font.Size = 12;
                style.Font.Bold = true;
                style.Alignment.Horizontal = StyleHorizontalAlignment.Center;

                WorksheetStyle style1 = book.Styles.Add("HeaderStyle1");
                style1.Font.FontName = "Tahoma";
                style1.Font.Size = 12;
                style1.Font.Bold = true;
                style1.Alignment.Horizontal = StyleHorizontalAlignment.Left;

                WorksheetStyle style2 = book.Styles.Add("CellStyle");
                style2.Font.FontName = "Tahoma";
                style2.Font.Size = 10;
                style2.Font.Color = "Blue";
                style2.Alignment.Horizontal = StyleHorizontalAlignment.Left;


                style = book.Styles.Add("Default");
                style.Font.FontName = "Tahoma";
                style.Font.Size = 10;

                try
                {
                    WorksheetRow row = sheet.Table.Rows.Add();


                    WorksheetCell cell = row.Cells.Add("Customer Intake Quality Control Report", DataType.String, "HeaderStyleBlue");
                    cell.MergeAcross = 20;

                    //int excelcolumn = 0;

                    //xlWorkSheet[excelcolumn, 5].Font = new System.Drawing.Font("Tahoma", 12, System.Drawing.FontStyle.Bold);
                    //xlWorkSheet[excelcolumn, 5].Alignment = Alignment.Centered;
                    //xlWorkSheet[excelcolumn, 5].ForeColor = ExcelColor.Blue;
                    //xlWorkSheet.WriteCell(excelcolumn, 5, "Customer Intake Quality Control Report");

                    //excelcolumn = excelcolumn + 2;
                    row = sheet.Table.Rows.Add();
                    row.Cells.Add(new WorksheetCell("Name(last and first)", "HeaderStyle"));//Name (first and last)
                    row.Cells.Add(new WorksheetCell("Program App #", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Central App #", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Central", "HeaderStyle"));
                    if (dtSummary.Rows.Count > 12)
                    {
                        row.Cells.Add(new WorksheetCell("SIM", "HeaderStyle"));
                    }
                    else if (dtSummary.Rows.Count == 12)
                    {
                        row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                    }

                    row.Cells.Add(new WorksheetCell("Program Matrix", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Central Matrix", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Income Verification - Program", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Income Verification - Central", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Required Intake Fields - Program", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("SP In Program", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("SP in Central", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Service in Program", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Service in Central", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Outcome in Program", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Outcome in Central", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Add Operator in Program", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Add Operator in Central", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Intake Date – Central", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Intake Date – Program", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Program Enroll Status", "HeaderStyle"));
                    
                    
                    if (dtDet.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtDet.Rows)
                        {
                            row = sheet.Table.Rows.Add();

                            row.Cells.Add(new WorksheetCell(dr["LAST_NAME"].ToString() + ", " + dr["FIRST_NAME"].ToString(), "Default"));//(dr["FIRST_NAME"].ToString() + " " + dr["LAST_NAME"].ToString(), "Default"));
                            row.Cells.Add(new WorksheetCell(dr["MST_APP_NO"].ToString(), "Default"));
                            row.Cells.Add(new WorksheetCell(dr["CENTRAL_APP"].ToString(), "Default"));
                            row.Cells.Add(new WorksheetCell(dr["CENTRAL_INTAKE"].ToString(), "Default"));
                            if (dtSummary.Rows.Count > 12)
                            {
                                row.Cells.Add(new WorksheetCell(dr["CENTRAL_SIM"].ToString(), "Default"));
                            }
                            else if (dtSummary.Rows.Count == 12)
                            {
                                row.Cells.Add(new WorksheetCell(dr["CENTRAL_SIM"].ToString(), "Default"));
                            }

                            row.Cells.Add(new WorksheetCell(dr["PROG_MATRIX"].ToString(), "Default"));
                            row.Cells.Add(new WorksheetCell(dr["CENTRAL_MATRIX"].ToString(), "Default"));
                            row.Cells.Add(new WorksheetCell(dr["PROG_VERIFICATION"].ToString(), "Default"));
                            row.Cells.Add(new WorksheetCell(dr["CENTRAL_VERIFICATION"].ToString(), "Default"));
                            row.Cells.Add(new WorksheetCell(dr["REQ_FLDS"].ToString(), "Default"));
                            row.Cells.Add(new WorksheetCell(dr["PROG_SPM"].ToString(), "Default"));
                            row.Cells.Add(new WorksheetCell(dr["CENTRAL_SPM"].ToString(), "Default"));
                            row.Cells.Add(new WorksheetCell(dr["PROG_CASEACT"].ToString(), "Default"));
                            row.Cells.Add(new WorksheetCell(dr["CENTRAL_CASEACT"].ToString(), "Default"));
                            row.Cells.Add(new WorksheetCell(dr["PROG_CASEMS"].ToString(), "Default"));
                            row.Cells.Add(new WorksheetCell(dr["CENTRAL_CASEMS"].ToString(), "Default"));
                            row.Cells.Add(new WorksheetCell(dr["ADD_OPERATOR"].ToString(), "Default"));
                            row.Cells.Add(new WorksheetCell(dr["CENTRAL_ADD_OPERATOR"].ToString(), "Default"));
                            row.Cells.Add(new WorksheetCell(LookupDataAccess.Getdate(dr["CENTRAL_INTAKE_DATE"].ToString()).ToString(), "Default"));
                            row.Cells.Add(new WorksheetCell(LookupDataAccess.Getdate(dr["INTAKE_DATE"].ToString()).ToString(), "Default"));

                            string Status_Desc = string.Empty;

                            switch (dr["PROG_ENRL_STATUS"].ToString().Trim())
                            {
                                case "L": Status_Desc = "Wait List"; break;
                                case "P": Status_Desc = "Pending"; break;
                                //case "R": Status_Desc = "Denied"; break;
                                case "E": Status_Desc = "Enrolled"; break;
                                case "W": Status_Desc = "Withdrawn"; break;
                                case "I": Status_Desc = "Post Intake"; break;
                                case "N": Status_Desc = "Inactive"; break;
                                case "C": Status_Desc = "Accepted"; break;
                            }

                            row.Cells.Add(new WorksheetCell(Status_Desc, "Default"));
                            

                        }

                    }

                    FileStream stream = new FileStream(PdfName, FileMode.Create);

                    book.Save(stream);
                    stream.Close();


                }
                catch (Exception ex) { }

            }
        }

        private void OnExcel_Report2(object sender, FormClosedEventArgs e)
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
                    {
                        DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim());
                    }
                }
                catch (Exception ex)
                {
                    AlertBox.Show("Error", MessageBoxIcon.Error);
                }


                try
                {
                    string Tmpstr = PdfName + ".xls";
                    if (File.Exists(Tmpstr))
                        File.Delete(Tmpstr);
                }
                catch (Exception ex)
                {
                    int length = 8;
                    string newFileName = System.Guid.NewGuid().ToString();
                    newFileName = newFileName.Replace("-", string.Empty);

                    Random_Filename = PdfName + newFileName.Substring(0, length) + ".xls";
                }


                if (!string.IsNullOrEmpty(Random_Filename))
                    PdfName = Random_Filename;
                else
                    PdfName += ".xls";

                Workbook book = new Workbook();

                Worksheet sheet = book.Worksheets.Add("Sheet1");
                //this.GenerateStyles(book.Styles);

                //ExcelDocument xlWorkSheet = new ExcelDocument();
                string strCodes = string.Empty;
                if (dtSummary.Rows.Count == 12)
                {
                    sheet.Table.Columns.Add(new WorksheetColumn(200));
                    sheet.Table.Columns.Add(new WorksheetColumn(100));
                    sheet.Table.Columns.Add(new WorksheetColumn(100));
                    //sheet.Table.Columns.Add(new WorksheetColumn(80));

                    foreach (DataGridViewRow drSps in gvwApplicants.Rows)
                    {
                        if (drSps.Cells["Selected"].Value.ToString().Trim() == "Y")
                        {
                            sheet.Table.Columns.Add(new WorksheetColumn(180));
                            if (drSps.Cells["Col_Code"].Value.ToString().Trim() == "1")
                                sheet.Table.Columns.Add(new WorksheetColumn(150));
                            if (!strCodes.Equals(string.Empty)) strCodes += ",";
                                strCodes += drSps.Cells["Col_Code"].Value.ToString().Trim();
                        }
                    }

                    //sheet.Table.Columns.Add(new WorksheetColumn(0));
                    //sheet.Table.Columns.Add(new WorksheetColumn(120));
                    //sheet.Table.Columns.Add(new WorksheetColumn(120));
                    //sheet.Table.Columns.Add(new WorksheetColumn(210));
                    //sheet.Table.Columns.Add(new WorksheetColumn(210));
                    //sheet.Table.Columns.Add(new WorksheetColumn(220));
                    //sheet.Table.Columns.Add(new WorksheetColumn(120));
                    //sheet.Table.Columns.Add(new WorksheetColumn(120));
                    //sheet.Table.Columns.Add(new WorksheetColumn(150));
                    //sheet.Table.Columns.Add(new WorksheetColumn(150));
                    //sheet.Table.Columns.Add(new WorksheetColumn(150));
                    //sheet.Table.Columns.Add(new WorksheetColumn(150));


                    sheet.Table.Columns.Add(new WorksheetColumn(150));
                    sheet.Table.Columns.Add(new WorksheetColumn(150));
                    sheet.Table.Columns.Add(new WorksheetColumn(120));
                    sheet.Table.Columns.Add(new WorksheetColumn(120));
                    sheet.Table.Columns.Add(new WorksheetColumn(180));
                    //xlWorkSheet.ColumnWidth(20, 120);
                    //xlWorkSheet.ColumnWidth(21, 120);
                    //xlWorkSheet.ColumnWidth(21, 120);
                }
                else if (dtSummary.Rows.Count > 12)
                {
                    sheet.Table.Columns.Add(new WorksheetColumn(200));
                    sheet.Table.Columns.Add(new WorksheetColumn(100));
                    sheet.Table.Columns.Add(new WorksheetColumn(100));
                    //sheet.Table.Columns.Add(new WorksheetColumn(80));

                    foreach (DataGridViewRow drSps in gvwApplicants.Rows)
                    {
                        if (drSps.Cells["Selected"].Value.ToString().Trim() == "Y")
                        {
                            sheet.Table.Columns.Add(new WorksheetColumn(180));
                            if (drSps.Cells["Col_Code"].Value.ToString().Trim() == "1")
                                sheet.Table.Columns.Add(new WorksheetColumn(180));
                            if (!strCodes.Equals(string.Empty)) strCodes += ",";
                                strCodes += drSps.Cells["Col_Code"].Value.ToString().Trim();
                        }
                    }

                    //sheet.Table.Columns.Add(new WorksheetColumn(80));
                    //sheet.Table.Columns.Add(new WorksheetColumn(120));
                    //sheet.Table.Columns.Add(new WorksheetColumn(120));
                    //sheet.Table.Columns.Add(new WorksheetColumn(210));
                    //sheet.Table.Columns.Add(new WorksheetColumn(210));
                    //sheet.Table.Columns.Add(new WorksheetColumn(220));
                    //sheet.Table.Columns.Add(new WorksheetColumn(120));
                    //sheet.Table.Columns.Add(new WorksheetColumn(120));
                    //sheet.Table.Columns.Add(new WorksheetColumn(150));
                    //sheet.Table.Columns.Add(new WorksheetColumn(150));
                    //sheet.Table.Columns.Add(new WorksheetColumn(150));
                    //sheet.Table.Columns.Add(new WorksheetColumn(150));


                    sheet.Table.Columns.Add(new WorksheetColumn(150));
                    sheet.Table.Columns.Add(new WorksheetColumn(150));
                    sheet.Table.Columns.Add(new WorksheetColumn(120));
                    sheet.Table.Columns.Add(new WorksheetColumn(120));
                    sheet.Table.Columns.Add(new WorksheetColumn(180));

                    //xlWorkSheet.ColumnWidth(20, 120);
                    //xlWorkSheet.ColumnWidth(21, 120);
                    //xlWorkSheet.ColumnWidth(22, 120);

                }

                WorksheetStyle styleb = book.Styles.Add("HeaderStyleBlue");
                styleb.Font.FontName = "Tahoma";
                styleb.Font.Size = 12;
                styleb.Font.Bold = true;
                styleb.Font.Color = "#0000FF";
                styleb.Alignment.Horizontal = StyleHorizontalAlignment.Center;


                WorksheetStyle style = book.Styles.Add("HeaderStyle");
                style.Font.FontName = "Tahoma";
                style.Font.Size = 12;
                style.Font.Bold = true;
                style.Alignment.Horizontal = StyleHorizontalAlignment.Center;

                WorksheetStyle style1 = book.Styles.Add("HeaderStyle1");
                style1.Font.FontName = "Tahoma";
                style1.Font.Size = 12;
                style1.Font.Bold = true;
                style1.Alignment.Horizontal = StyleHorizontalAlignment.Left;

                WorksheetStyle style2 = book.Styles.Add("CellStyle");
                style2.Font.FontName = "Tahoma";
                style2.Font.Size = 10;
                style2.Font.Color = "Blue";
                style2.Alignment.Horizontal = StyleHorizontalAlignment.Left;

                WorksheetStyle CellFill = book.Styles.Add("CellFill");
                CellFill.Font.FontName = "Tahoma";
                CellFill.Font.Size = 10;
                CellFill.Interior.Color = "Yellow";
                CellFill.Interior.Pattern= StyleInteriorPattern.Solid;
                CellFill.Alignment.Horizontal = StyleHorizontalAlignment.Left;


                style = book.Styles.Add("Default");
                style.Font.FontName = "Tahoma";
                style.Font.Size = 10;

                AGYTABSEntity searchAgytabs = new AGYTABSEntity(true);
                searchAgytabs.Tabs_Type = "S0023";
                List<AGYTABSEntity> AgyTabs_List = _model.AdhocData.Browse_AGYTABS(searchAgytabs);

                try
                {
                    WorksheetRow row = sheet.Table.Rows.Add();

                   

                    WorksheetCell cell = row.Cells.Add("Customer Intake Quality Control Report", DataType.String, "HeaderStyleBlue");
                    cell.MergeAcross = 20;

                    //int excelcolumn = 0;

                    //xlWorkSheet[excelcolumn, 5].Font = new System.Drawing.Font("Tahoma", 12, System.Drawing.FontStyle.Bold);
                    //xlWorkSheet[excelcolumn, 5].Alignment = Alignment.Centered;
                    //xlWorkSheet[excelcolumn, 5].ForeColor = ExcelColor.Blue;
                    //xlWorkSheet.WriteCell(excelcolumn, 5, "Customer Intake Quality Control Report");

                    //excelcolumn = excelcolumn + 2;
                    row = sheet.Table.Rows.Add();
                    row.Cells.Add(new WorksheetCell("Name(last and first)", "HeaderStyle"));//Name (first and last)
                    row.Cells.Add(new WorksheetCell("Program App #", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Central App #", "HeaderStyle"));

                    if (strCodes.Length > 0)
                    {
                        string[] values = strCodes.Split(',');
                        for (int i = 0; i < values.Length; i++)
                        {
                            values[i] = values[i].Trim();

                            switch (values[i])
                            {
                                case "1": row.Cells.Add(new WorksheetCell("Central Applicant", "HeaderStyle"));
                                        row.Cells.Add(new WorksheetCell("Central Member", "HeaderStyle")); break;
                                case "2":
                                    if (dtSummary.Rows.Count > 12)
                                        row.Cells.Add(new WorksheetCell("SIM", "HeaderStyle"));
                                    else row.Cells.Add(new WorksheetCell("Program Matrix", "HeaderStyle")); break;
                                case "3":
                                    if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell("Program Matrix", "HeaderStyle"));
                                    else row.Cells.Add(new WorksheetCell("Central Matrix", "HeaderStyle")); break;
                                case "4":
                                    if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell("Central Matrix", "HeaderStyle"));
                                    else row.Cells.Add(new WorksheetCell("Income Verification - Program", "HeaderStyle")); break;
                                case "5":
                                    if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell("Income Verification - Program", "HeaderStyle"));
                                    else row.Cells.Add(new WorksheetCell("Income Verification - Central", "HeaderStyle")); break;
                                case "6":
                                    if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell("Income Verification - Central", "HeaderStyle"));
                                    else row.Cells.Add(new WorksheetCell("Required Intake Fields - Program", "HeaderStyle")); break;
                                case "7":
                                    if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell("Required Intake Fields - Program", "HeaderStyle"));
                                    else row.Cells.Add(new WorksheetCell("SP In Program", "HeaderStyle")); break;
                                case "8":
                                    if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell("SP In Program", "HeaderStyle"));
                                    else row.Cells.Add(new WorksheetCell("SP in Central", "HeaderStyle")); break;
                                case "9":
                                    if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell("SP in Central", "HeaderStyle"));
                                    else row.Cells.Add(new WorksheetCell("Service in Program", "HeaderStyle")); break;
                                case "10":
                                    if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell("Service in Program", "HeaderStyle"));
                                    else row.Cells.Add(new WorksheetCell("Service in Central", "HeaderStyle")); break;
                                case "11":
                                    if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell("Service in Central", "HeaderStyle"));
                                    else row.Cells.Add(new WorksheetCell("Outcome in Program", "HeaderStyle")); break;
                                case "12":
                                    if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell("Outcome in Program", "HeaderStyle"));
                                    else row.Cells.Add(new WorksheetCell("Outcome in Central", "HeaderStyle")); break;
                                case "13":
                                    if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell("Outcome in Central", "HeaderStyle"));
                                    break;
                            }


                        }
                    }

                    //row.Cells.Add(new WorksheetCell("Central", "HeaderStyle"));
                    //if (dtSummary.Rows.Count > 12)
                    //{
                    //    row.Cells.Add(new WorksheetCell("SIM", "HeaderStyle"));
                    //}
                    //else if (dtSummary.Rows.Count == 12)
                    //{
                    //    row.Cells.Add(new WorksheetCell("", "HeaderStyle"));
                    //}

                    //row.Cells.Add(new WorksheetCell("Program Matrix", "HeaderStyle"));
                    //row.Cells.Add(new WorksheetCell("Central Matrix", "HeaderStyle"));
                    //row.Cells.Add(new WorksheetCell("Income Verification - Program", "HeaderStyle"));
                    //row.Cells.Add(new WorksheetCell("Income Verification - Central", "HeaderStyle"));
                    //row.Cells.Add(new WorksheetCell("Required Intake Fields - Program", "HeaderStyle"));
                    //row.Cells.Add(new WorksheetCell("SP In Program", "HeaderStyle"));
                    //row.Cells.Add(new WorksheetCell("SP in Central", "HeaderStyle"));
                    //row.Cells.Add(new WorksheetCell("CA in Program", "HeaderStyle"));
                    //row.Cells.Add(new WorksheetCell("CA in Central", "HeaderStyle"));
                    //row.Cells.Add(new WorksheetCell("MS in Program", "HeaderStyle"));
                    //row.Cells.Add(new WorksheetCell("MS in Central", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Add Operator in Program", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Add Operator in Central", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Intake Date – Central", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Intake Date – Program", "HeaderStyle"));
                    row.Cells.Add(new WorksheetCell("Program Enroll Status", "HeaderStyle"));


                    if (dtDet.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtDet.Rows)
                        {
                            row = sheet.Table.Rows.Add();

                            row.Cells.Add(new WorksheetCell(dr["LAST_NAME"].ToString() + ", " + dr["FIRST_NAME"].ToString(), "Default"));//(dr["FIRST_NAME"].ToString() + " " + dr["LAST_NAME"].ToString(), "Default"));
                            row.Cells.Add(new WorksheetCell(dr["MST_APP_NO"].ToString(), "Default"));
                            row.Cells.Add(new WorksheetCell(dr["CENTRAL_APP"].ToString(), "Default"));

                            if (strCodes.Length > 0)
                            {
                                string[] values = strCodes.Split(',');
                                for (int i = 0; i < values.Length; i++)
                                {
                                    values[i] = values[i].Trim();

                                    switch (values[i])
                                    {
                                        case "1": row.Cells.Add(new WorksheetCell(dr["CENTRAL_INTAKE"].ToString(), "Default"));
                                                    row.Cells.Add(new WorksheetCell(dr["CENTRAL_MEMBER"].ToString(), "Default")); break;
                                        case "2":
                                            if (dtSummary.Rows.Count > 12)
                                            {
                                                if (dr["CENTRAL_SIM"].ToString().Trim() == "N")
                                                    row.Cells.Add(new WorksheetCell(dr["CENTRAL_SIM"].ToString(), "CellFill"));
                                                else
                                                    row.Cells.Add(new WorksheetCell(dr["CENTRAL_SIM"].ToString(), "Default"));
                                            }
                                            else
                                            {
                                                if (dr["PROG_MATRIX"].ToString().Trim() == "N")
                                                    row.Cells.Add(new WorksheetCell(dr["PROG_MATRIX"].ToString(), "CellFill"));
                                                else
                                                    row.Cells.Add(new WorksheetCell(dr["PROG_MATRIX"].ToString(), "Default"));
                                            }
                                            break;
                                        case "3":
                                            if (dtSummary.Rows.Count > 12)
                                            {
                                                if (dr["PROG_MATRIX"].ToString().Trim() == "N")
                                                    row.Cells.Add(new WorksheetCell(dr["PROG_MATRIX"].ToString(), "CellFill"));
                                                else
                                                    row.Cells.Add(new WorksheetCell(dr["PROG_MATRIX"].ToString(), "Default"));
                                            }
                                            else
                                            {
                                                if (dr["CENTRAL_MATRIX"].ToString().Trim() == "N")
                                                    row.Cells.Add(new WorksheetCell(dr["CENTRAL_MATRIX"].ToString(), "CellFill"));
                                                else
                                                    row.Cells.Add(new WorksheetCell(dr["CENTRAL_MATRIX"].ToString(), "Default"));
                                            }
                                            break;
                                        case "4":
                                            if (dtSummary.Rows.Count > 12)
                                            {
                                                if (dr["CENTRAL_MATRIX"].ToString().Trim() == "N")
                                                    row.Cells.Add(new WorksheetCell(dr["CENTRAL_MATRIX"].ToString(), "CellFill"));
                                                else
                                                    row.Cells.Add(new WorksheetCell(dr["CENTRAL_MATRIX"].ToString(), "Default"));
                                            }
                                            else
                                            {
                                                if (dr["PROG_VERIFICATION"].ToString().Trim() == "N")
                                                    row.Cells.Add(new WorksheetCell(dr["PROG_VERIFICATION"].ToString(), "CellFill"));
                                                else
                                                    row.Cells.Add(new WorksheetCell(dr["PROG_VERIFICATION"].ToString(), "Default"));
                                            }
                                            //if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell(dr["CENTRAL_MATRIX"].ToString(), "Default"));
                                            //else row.Cells.Add(new WorksheetCell(dr["PROG_VERIFICATION"].ToString(), "Default")); 
                                            break;
                                        case "5":
                                            if (dtSummary.Rows.Count > 12)
                                            {
                                                if (dr["PROG_VERIFICATION"].ToString().Trim() == "N")
                                                    row.Cells.Add(new WorksheetCell(dr["PROG_VERIFICATION"].ToString(), "CellFill"));
                                                else
                                                    row.Cells.Add(new WorksheetCell(dr["PROG_VERIFICATION"].ToString(), "Default"));
                                            }
                                            else
                                            {
                                                if (dr["CENTRAL_VERIFICATION"].ToString().Trim() == "N")
                                                    row.Cells.Add(new WorksheetCell(dr["CENTRAL_VERIFICATION"].ToString(), "CellFill"));
                                                else
                                                    row.Cells.Add(new WorksheetCell(dr["CENTRAL_VERIFICATION"].ToString(), "Default"));
                                            }
                                            //if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell(dr["PROG_VERIFICATION"].ToString(), "Default"));
                                            //else row.Cells.Add(new WorksheetCell(dr["CENTRAL_VERIFICATION"].ToString(), "Default"));
                                            break;
                                        case "6":
                                            if (dtSummary.Rows.Count > 12)
                                            {
                                                if (dr["CENTRAL_VERIFICATION"].ToString().Trim() == "N")
                                                    row.Cells.Add(new WorksheetCell(dr["CENTRAL_VERIFICATION"].ToString(), "CellFill"));
                                                else
                                                    row.Cells.Add(new WorksheetCell(dr["CENTRAL_VERIFICATION"].ToString(), "Default"));
                                            }
                                            else
                                            {
                                                if (dr["REQ_FLDS"].ToString().Trim() == "N")
                                                    row.Cells.Add(new WorksheetCell(dr["REQ_FLDS"].ToString(), "CellFill"));
                                                else
                                                    row.Cells.Add(new WorksheetCell(dr["REQ_FLDS"].ToString(), "Default"));
                                            }
                                            //if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell(dr["CENTRAL_VERIFICATION"].ToString(), "Default"));
                                            //else row.Cells.Add(new WorksheetCell(dr["REQ_FLDS"].ToString(), "Default"));
                                            break;
                                        case "7":
                                            if (dtSummary.Rows.Count > 12)
                                            {
                                                if (dr["REQ_FLDS"].ToString().Trim() == "N")
                                                    row.Cells.Add(new WorksheetCell(dr["REQ_FLDS"].ToString(), "CellFill"));
                                                else
                                                    row.Cells.Add(new WorksheetCell(dr["REQ_FLDS"].ToString(), "Default"));
                                            }
                                            else
                                            {
                                                if (dr["PROG_SPM"].ToString().Trim() == "N")
                                                    row.Cells.Add(new WorksheetCell(dr["PROG_SPM"].ToString(), "CellFill"));
                                                else
                                                    row.Cells.Add(new WorksheetCell(dr["PROG_SPM"].ToString(), "Default"));
                                            }
                                            //if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell(dr["REQ_FLDS"].ToString(), "Default"));
                                            //else row.Cells.Add(new WorksheetCell(dr["PROG_SPM"].ToString(), "Default"));
                                            break;
                                        case "8":
                                            if (dtSummary.Rows.Count > 12)
                                            {
                                                if (dr["PROG_SPM"].ToString().Trim() == "N")
                                                    row.Cells.Add(new WorksheetCell(dr["PROG_SPM"].ToString(), "CellFill"));
                                                else
                                                    row.Cells.Add(new WorksheetCell(dr["PROG_SPM"].ToString(), "Default"));
                                            }
                                            else
                                            {
                                                if (dr["CENTRAL_SPM"].ToString().Trim() == "N")
                                                    row.Cells.Add(new WorksheetCell(dr["CENTRAL_SPM"].ToString(), "CellFill"));
                                                else
                                                    row.Cells.Add(new WorksheetCell(dr["CENTRAL_SPM"].ToString(), "Default"));
                                            }
                                            //if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell(dr["PROG_SPM"].ToString(), "Default"));
                                            //else row.Cells.Add(new WorksheetCell(dr["CENTRAL_SPM"].ToString(), "Default"));
                                            break;
                                        case "9":
                                            if (dtSummary.Rows.Count > 12)
                                            {
                                                if (dr["CENTRAL_SPM"].ToString().Trim() == "N")
                                                    row.Cells.Add(new WorksheetCell(dr["CENTRAL_SPM"].ToString(), "CellFill"));
                                                else
                                                    row.Cells.Add(new WorksheetCell(dr["CENTRAL_SPM"].ToString(), "Default"));
                                            }
                                            else
                                            {
                                                if (dr["PROG_CASEACT"].ToString().Trim() == "N")
                                                    row.Cells.Add(new WorksheetCell(dr["PROG_CASEACT"].ToString(), "CellFill"));
                                                else
                                                    row.Cells.Add(new WorksheetCell(dr["PROG_CASEACT"].ToString(), "Default"));
                                            }
                                            //if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell(dr["CENTRAL_SPM"].ToString(), "Default"));
                                            //else row.Cells.Add(new WorksheetCell(dr["PROG_CASEACT"].ToString(), "Default"));
                                            break;
                                        case "10":
                                            if (dtSummary.Rows.Count > 12)
                                            {
                                                if (dr["PROG_CASEACT"].ToString().Trim() == "N")
                                                    row.Cells.Add(new WorksheetCell(dr["PROG_CASEACT"].ToString(), "CellFill"));
                                                else
                                                    row.Cells.Add(new WorksheetCell(dr["PROG_CASEACT"].ToString(), "Default"));
                                            }
                                            else
                                            {
                                                if (dr["CENTRAL_CASEACT"].ToString().Trim() == "N")
                                                    row.Cells.Add(new WorksheetCell(dr["CENTRAL_CASEACT"].ToString(), "CellFill"));
                                                else
                                                    row.Cells.Add(new WorksheetCell(dr["CENTRAL_CASEACT"].ToString(), "Default"));
                                            }
                                            //if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell(dr["PROG_CASEACT"].ToString(), "Default"));
                                            //else row.Cells.Add(new WorksheetCell(dr["CENTRAL_CASEACT"].ToString(), "Default"));
                                            break;
                                        case "11":
                                            if (dtSummary.Rows.Count > 12)
                                            {
                                                if (dr["CENTRAL_CASEACT"].ToString().Trim() == "N")
                                                    row.Cells.Add(new WorksheetCell(dr["CENTRAL_CASEACT"].ToString(), "CellFill"));
                                                else
                                                    row.Cells.Add(new WorksheetCell(dr["CENTRAL_CASEACT"].ToString(), "Default"));
                                            }
                                            else
                                            {
                                                if (dr["PROG_CASEMS"].ToString().Trim() == "N")
                                                    row.Cells.Add(new WorksheetCell(dr["PROG_CASEMS"].ToString(), "CellFill"));
                                                else
                                                    row.Cells.Add(new WorksheetCell(dr["PROG_CASEMS"].ToString(), "Default"));
                                            }
                                            //if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell(dr["CENTRAL_CASEACT"].ToString(), "Default"));
                                            //else row.Cells.Add(new WorksheetCell(dr["PROG_CASEMS"].ToString(), "Default"));
                                            break;
                                        case "12":
                                            if (dtSummary.Rows.Count > 12)
                                            {
                                                if (dr["PROG_CASEMS"].ToString().Trim() == "N")
                                                    row.Cells.Add(new WorksheetCell(dr["PROG_CASEMS"].ToString(), "CellFill"));
                                                else
                                                    row.Cells.Add(new WorksheetCell(dr["PROG_CASEMS"].ToString(), "Default"));
                                            }
                                            else
                                            {
                                                if (dr["CENTRAL_CASEMS"].ToString().Trim() == "N")
                                                    row.Cells.Add(new WorksheetCell(dr["CENTRAL_CASEMS"].ToString(), "CellFill"));
                                                else
                                                    row.Cells.Add(new WorksheetCell(dr["CENTRAL_CASEMS"].ToString(), "Default"));
                                            }
                                            //if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell(dr["PROG_CASEMS"].ToString(), "Default"));
                                            //else row.Cells.Add(new WorksheetCell(dr["CENTRAL_CASEMS"].ToString(), "Default"));
                                            break;
                                        case "13":
                                            if (dtSummary.Rows.Count > 12)
                                            {
                                                if (dr["CENTRAL_CASEMS"].ToString().Trim() == "N")
                                                    row.Cells.Add(new WorksheetCell(dr["CENTRAL_CASEMS"].ToString(), "CellFill"));
                                                else
                                                    row.Cells.Add(new WorksheetCell(dr["CENTRAL_CASEMS"].ToString(), "Default"));
                                            }
                                                //row.Cells.Add(new WorksheetCell(dr["CENTRAL_CASEMS"].ToString(), "Default"));
                                            break;
                                    }

                                }
                            }

                            //    row.Cells.Add(new WorksheetCell(dr["CENTRAL_INTAKE"].ToString(), "Default"));
                            //if (dtSummary.Rows.Count > 12)
                            //{
                            //    row.Cells.Add(new WorksheetCell(dr["CENTRAL_SIM"].ToString(), "Default"));
                            //}
                            //else if (dtSummary.Rows.Count == 12)
                            //{
                            //    row.Cells.Add(new WorksheetCell(dr["CENTRAL_SIM"].ToString(), "Default"));
                            //}

                            //row.Cells.Add(new WorksheetCell(dr["PROG_MATRIX"].ToString(), "Default"));
                            //row.Cells.Add(new WorksheetCell(dr["CENTRAL_MATRIX"].ToString(), "Default"));
                            //row.Cells.Add(new WorksheetCell(dr["PROG_VERIFICATION"].ToString(), "Default"));
                            //row.Cells.Add(new WorksheetCell(dr["CENTRAL_VERIFICATION"].ToString(), "Default"));
                            //row.Cells.Add(new WorksheetCell(dr["REQ_FLDS"].ToString(), "Default"));
                            //row.Cells.Add(new WorksheetCell(dr["PROG_SPM"].ToString(), "Default"));
                            //row.Cells.Add(new WorksheetCell(dr["CENTRAL_SPM"].ToString(), "Default"));
                            //row.Cells.Add(new WorksheetCell(dr["PROG_CASEACT"].ToString(), "Default"));
                            //row.Cells.Add(new WorksheetCell(dr["CENTRAL_CASEACT"].ToString(), "Default"));
                            //row.Cells.Add(new WorksheetCell(dr["PROG_CASEMS"].ToString(), "Default"));
                            //row.Cells.Add(new WorksheetCell(dr["CENTRAL_CASEMS"].ToString(), "Default"));
                            row.Cells.Add(new WorksheetCell(dr["ADD_OPERATOR"].ToString(), "Default"));
                            row.Cells.Add(new WorksheetCell(dr["CENTRAL_ADD_OPERATOR"].ToString(), "Default"));
                            row.Cells.Add(new WorksheetCell(LookupDataAccess.Getdate(dr["CENTRAL_INTAKE_DATE"].ToString()).ToString(), "Default"));
                            row.Cells.Add(new WorksheetCell(LookupDataAccess.Getdate(dr["INTAKE_DATE"].ToString()).ToString(), "Default"));

                            string Status_Desc = string.Empty;

                            if(AgyTabs_List.Count>0)
                            {
                                AGYTABSEntity SelEntity = AgyTabs_List.Find(u => u.Table_Code.Equals(dr["PROG_ENRL_STATUS"].ToString().Trim()));
                                if (SelEntity != null)
                                    Status_Desc = SelEntity.Code_Desc.Trim();
                            }

                            //switch (dr["PROG_ENRL_STATUS"].ToString().Trim())
                            //{
                            //    case "L": Status_Desc = "Wait List"; break;
                            //    case "P": Status_Desc = "Pending"; break;
                            //    //case "R": Status_Desc = "Denied"; break;
                            //    case "E": Status_Desc = "Enrolled"; break;
                            //    case "W": Status_Desc = "Withdrawn"; break;
                            //    case "I": Status_Desc = "Post Intake"; break;
                            //    case "N": Status_Desc = "Inactive"; break;
                            //    case "C": Status_Desc = "Accepted"; break;
                            //}

                            row.Cells.Add(new WorksheetCell(Status_Desc, "Default"));


                        }

                    }

                    FileStream stream = new FileStream(PdfName, FileMode.Create);

                    book.Save(stream);
                    stream.Close();


                }
                catch (Exception ex) { }

            }
        }

        private void OnExcel_Report3(object sender, FormClosedEventArgs e)
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
                    {
                        DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim());
                    }
                }
                catch (Exception ex)
                {
                    AlertBox.Show("Error", MessageBoxIcon.Error);
                }


                try
                {
                    string Tmpstr = PdfName + ".xls";
                    if (File.Exists(Tmpstr))
                        File.Delete(Tmpstr);
                }
                catch (Exception ex)
                {
                    int length = 8;
                    string newFileName = System.Guid.NewGuid().ToString();
                    newFileName = newFileName.Replace("-", string.Empty);

                    Random_Filename = PdfName + newFileName.Substring(0, length) + ".xls";
                }


                if (!string.IsNullOrEmpty(Random_Filename))
                    PdfName = Random_Filename;
                else
                    PdfName += ".xls";

                Workbook book = new Workbook();

                Worksheet sheet = book.Worksheets.Add("Sheet1");
                //this.GenerateStyles(book.Styles);

                //ExcelDocument xlWorkSheet = new ExcelDocument();
                string strCodes = string.Empty;


                if(string.IsNullOrEmpty(propCentralHie.Trim()))
                {
                    sheet.Table.Columns.Add(new WorksheetColumn(200));
                    sheet.Table.Columns.Add(new WorksheetColumn(100));

                    sheet.Table.Columns.Add(new WorksheetColumn(180));
                    sheet.Table.Columns.Add(new WorksheetColumn(180));
                    sheet.Table.Columns.Add(new WorksheetColumn(180));
                    sheet.Table.Columns.Add(new WorksheetColumn(180));
                    sheet.Table.Columns.Add(new WorksheetColumn(180));
                    sheet.Table.Columns.Add(new WorksheetColumn(180));

                    sheet.Table.Columns.Add(new WorksheetColumn(150));
                    //sheet.Table.Columns.Add(new WorksheetColumn(150));
                    sheet.Table.Columns.Add(new WorksheetColumn(120));
                    //sheet.Table.Columns.Add(new WorksheetColumn(120));
                    sheet.Table.Columns.Add(new WorksheetColumn(180));


                }
                else
                {
                    if (dtSummary.Rows.Count == 12)
                    {
                        sheet.Table.Columns.Add(new WorksheetColumn(200));
                        sheet.Table.Columns.Add(new WorksheetColumn(100));
                        sheet.Table.Columns.Add(new WorksheetColumn(100));
                        //sheet.Table.Columns.Add(new WorksheetColumn(80));

                        foreach (DataGridViewRow drSps in gvwApplicants.Rows)
                        {
                            if (drSps.Cells["Selected"].Value.ToString().Trim() == "Y")
                            {
                                sheet.Table.Columns.Add(new WorksheetColumn(180));
                                if (drSps.Cells["Col_Code"].Value.ToString().Trim() == "1")
                                    sheet.Table.Columns.Add(new WorksheetColumn(150));
                                if (!strCodes.Equals(string.Empty)) strCodes += ",";
                                strCodes += drSps.Cells["Col_Code"].Value.ToString().Trim();
                            }
                        }

                        //sheet.Table.Columns.Add(new WorksheetColumn(0));
                        //sheet.Table.Columns.Add(new WorksheetColumn(120));
                        //sheet.Table.Columns.Add(new WorksheetColumn(120));
                        //sheet.Table.Columns.Add(new WorksheetColumn(210));
                        //sheet.Table.Columns.Add(new WorksheetColumn(210));
                        //sheet.Table.Columns.Add(new WorksheetColumn(220));
                        //sheet.Table.Columns.Add(new WorksheetColumn(120));
                        //sheet.Table.Columns.Add(new WorksheetColumn(120));
                        //sheet.Table.Columns.Add(new WorksheetColumn(150));
                        //sheet.Table.Columns.Add(new WorksheetColumn(150));
                        //sheet.Table.Columns.Add(new WorksheetColumn(150));
                        //sheet.Table.Columns.Add(new WorksheetColumn(150));


                        sheet.Table.Columns.Add(new WorksheetColumn(150));
                        sheet.Table.Columns.Add(new WorksheetColumn(150));
                        sheet.Table.Columns.Add(new WorksheetColumn(120));
                        sheet.Table.Columns.Add(new WorksheetColumn(120));
                        sheet.Table.Columns.Add(new WorksheetColumn(180));
                        //xlWorkSheet.ColumnWidth(20, 120);
                        //xlWorkSheet.ColumnWidth(21, 120);
                        //xlWorkSheet.ColumnWidth(21, 120);
                    }
                    else if (dtSummary.Rows.Count > 12)
                    {
                        sheet.Table.Columns.Add(new WorksheetColumn(200));
                        sheet.Table.Columns.Add(new WorksheetColumn(100));
                        sheet.Table.Columns.Add(new WorksheetColumn(100));
                        //sheet.Table.Columns.Add(new WorksheetColumn(80));

                        foreach (DataGridViewRow drSps in gvwApplicants.Rows)
                        {
                            if (drSps.Cells["Selected"].Value.ToString().Trim() == "Y")
                            {
                                sheet.Table.Columns.Add(new WorksheetColumn(180));
                                if (drSps.Cells["Col_Code"].Value.ToString().Trim() == "1")
                                    sheet.Table.Columns.Add(new WorksheetColumn(180));
                                if (!strCodes.Equals(string.Empty)) strCodes += ",";
                                strCodes += drSps.Cells["Col_Code"].Value.ToString().Trim();
                            }
                        }

                        //sheet.Table.Columns.Add(new WorksheetColumn(80));
                        //sheet.Table.Columns.Add(new WorksheetColumn(120));
                        //sheet.Table.Columns.Add(new WorksheetColumn(120));
                        //sheet.Table.Columns.Add(new WorksheetColumn(210));
                        //sheet.Table.Columns.Add(new WorksheetColumn(210));
                        //sheet.Table.Columns.Add(new WorksheetColumn(220));
                        //sheet.Table.Columns.Add(new WorksheetColumn(120));
                        //sheet.Table.Columns.Add(new WorksheetColumn(120));
                        //sheet.Table.Columns.Add(new WorksheetColumn(150));
                        //sheet.Table.Columns.Add(new WorksheetColumn(150));
                        //sheet.Table.Columns.Add(new WorksheetColumn(150));
                        //sheet.Table.Columns.Add(new WorksheetColumn(150));


                        sheet.Table.Columns.Add(new WorksheetColumn(150));
                        sheet.Table.Columns.Add(new WorksheetColumn(150));
                        sheet.Table.Columns.Add(new WorksheetColumn(120));
                        sheet.Table.Columns.Add(new WorksheetColumn(120));
                        sheet.Table.Columns.Add(new WorksheetColumn(180));

                        //xlWorkSheet.ColumnWidth(20, 120);
                        //xlWorkSheet.ColumnWidth(21, 120);
                        //xlWorkSheet.ColumnWidth(22, 120);

                    }
                }


                

                WorksheetStyle styleb = book.Styles.Add("HeaderStyleBlue");
                styleb.Font.FontName = "Tahoma";
                styleb.Font.Size = 12;
                styleb.Font.Bold = true;
                styleb.Font.Color = "#0000FF";
                styleb.Alignment.Horizontal = StyleHorizontalAlignment.Center;


                WorksheetStyle style = book.Styles.Add("HeaderStyle");
                style.Font.FontName = "Tahoma";
                style.Font.Size = 12;
                style.Font.Bold = true;
                style.Alignment.Horizontal = StyleHorizontalAlignment.Center;

                WorksheetStyle style1 = book.Styles.Add("HeaderStyle1");
                style1.Font.FontName = "Tahoma";
                style1.Font.Size = 12;
                style1.Font.Bold = true;
                style1.Alignment.Horizontal = StyleHorizontalAlignment.Left;

                WorksheetStyle style2 = book.Styles.Add("CellStyle");
                style2.Font.FontName = "Tahoma";
                style2.Font.Size = 10;
                style2.Font.Color = "Blue";
                style2.Alignment.Horizontal = StyleHorizontalAlignment.Left;

                WorksheetStyle CellFill = book.Styles.Add("CellFill");
                CellFill.Font.FontName = "Tahoma";
                CellFill.Font.Size = 10;
                CellFill.Interior.Color = "Yellow";
                CellFill.Interior.Pattern = StyleInteriorPattern.Solid;
                CellFill.Alignment.Horizontal = StyleHorizontalAlignment.Left;


                style = book.Styles.Add("Default");
                style.Font.FontName = "Tahoma";
                style.Font.Size = 10;

                AGYTABSEntity searchAgytabs = new AGYTABSEntity(true);
                searchAgytabs.Tabs_Type = "S0023";
                List<AGYTABSEntity> AgyTabs_List = _model.AdhocData.Browse_AGYTABS(searchAgytabs);

                try
                {
                    WorksheetRow row = sheet.Table.Rows.Add();



                    WorksheetCell cell = row.Cells.Add("Customer Intake Quality Control Report", DataType.String, "HeaderStyleBlue");
                    if (string.IsNullOrEmpty(propCentralHie.Trim()))
                        cell.MergeAcross = 20;
                    else
                        cell.MergeAcross = 10;
                    //int excelcolumn = 0;

                    //xlWorkSheet[excelcolumn, 5].Font = new System.Drawing.Font("Tahoma", 12, System.Drawing.FontStyle.Bold);
                    //xlWorkSheet[excelcolumn, 5].Alignment = Alignment.Centered;
                    //xlWorkSheet[excelcolumn, 5].ForeColor = ExcelColor.Blue;
                    //xlWorkSheet.WriteCell(excelcolumn, 5, "Customer Intake Quality Control Report");

                    //excelcolumn = excelcolumn + 2;
                    row = sheet.Table.Rows.Add();
                    row.Cells.Add(new WorksheetCell("Name(last and first)", "HeaderStyle"));//Name (first and last)
                    row.Cells.Add(new WorksheetCell("Program App #", "HeaderStyle"));

                    if (string.IsNullOrEmpty(propCentralHie.Trim()))
                    {
                        row.Cells.Add(new WorksheetCell("Program Matrix", "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell("Income Verification - Program", "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell("Required Intake Fields - Program", "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell("SP In Program", "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell("Service in Program", "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell("Outcome in Program", "HeaderStyle"));

                        row.Cells.Add(new WorksheetCell("Add Operator in Program", "HeaderStyle"));
                        //row.Cells.Add(new WorksheetCell("Add Operator in Central", "HeaderStyle"));
                        //row.Cells.Add(new WorksheetCell("Intake Date – Central", "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell("Intake Date – Program", "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell("Program Enroll Status", "HeaderStyle"));

                    }
                    else
                    {
                        row.Cells.Add(new WorksheetCell("Central App #", "HeaderStyle"));

                        row.Cells.Add(new WorksheetCell("Central Applicant", "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell("Central Member", "HeaderStyle"));

                        foreach (DataGridViewRow drSps in gvwApplicants.Rows)
                        {
                            if (drSps.Cells["Selected"].Value.ToString().Trim() == "Y")
                            {
                                switch(drSps.Cells["Col_Code"].Value.ToString().Trim())
                                {
                                    case "1":
                                        if (dtSummary.Rows.Count > 12)
                                            row.Cells.Add(new WorksheetCell("SIM", "HeaderStyle"));
                                        else row.Cells.Add(new WorksheetCell("Program Matrix", "HeaderStyle")); break;
                                    case "2":
                                        if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell("Program Matrix", "HeaderStyle"));
                                        else row.Cells.Add(new WorksheetCell("Central Matrix", "HeaderStyle"));break;
                                    case "3":
                                        if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell("Central Matrix", "HeaderStyle"));
                                        else row.Cells.Add(new WorksheetCell("Income Verification - Program", "HeaderStyle")); break;
                                    case "4":
                                        if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell("Income Verification - Program", "HeaderStyle"));
                                        else row.Cells.Add(new WorksheetCell("Income Verification - Central", "HeaderStyle")); break;
                                    case "5":
                                        if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell("Income Verification - Central", "HeaderStyle"));
                                        else row.Cells.Add(new WorksheetCell("Required Intake Fields - Program", "HeaderStyle")); break;
                                    case "6":
                                        if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell("Required Intake Fields - Program", "HeaderStyle"));
                                        else row.Cells.Add(new WorksheetCell("SP In Program", "HeaderStyle")); break;
                                    case "7":
                                        if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell("SP In Program", "HeaderStyle"));
                                        else row.Cells.Add(new WorksheetCell("SP in Central", "HeaderStyle")); break;
                                    case "8":
                                        if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell("SP in Central", "HeaderStyle"));
                                        else row.Cells.Add(new WorksheetCell("Service in Program", "HeaderStyle")); break;
                                    case "9":
                                        if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell("Service in Program", "HeaderStyle"));
                                        else row.Cells.Add(new WorksheetCell("Service in Central", "HeaderStyle")); break;
                                    case "10":
                                        if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell("Service in Central", "HeaderStyle"));
                                        else row.Cells.Add(new WorksheetCell("Outcome in Program", "HeaderStyle")); break;
                                    case "11":
                                        if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell("Outcome in Program", "HeaderStyle"));
                                        else row.Cells.Add(new WorksheetCell("Outcome in Central", "HeaderStyle")); break;
                                    case "12": if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell("Outcome in Central", "HeaderStyle")); break;
                                }
                                
                            }
                        }

                        

                        //if (dtSummary.Rows.Count > 12)
                        //    row.Cells.Add(new WorksheetCell("SIM", "HeaderStyle"));
                        //else row.Cells.Add(new WorksheetCell("Program Matrix", "HeaderStyle"));

                        //if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell("Program Matrix", "HeaderStyle"));
                        //else row.Cells.Add(new WorksheetCell("Central Matrix", "HeaderStyle"));

                        //if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell("Central Matrix", "HeaderStyle"));
                        //else row.Cells.Add(new WorksheetCell("Income Verification - Program", "HeaderStyle"));

                        //if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell("Income Verification - Program", "HeaderStyle"));
                        //else row.Cells.Add(new WorksheetCell("Income Verification - Central", "HeaderStyle"));

                        //if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell("Income Verification - Central", "HeaderStyle"));
                        //else row.Cells.Add(new WorksheetCell("Required Intake Fields - Program", "HeaderStyle"));

                        //if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell("Required Intake Fields - Program", "HeaderStyle"));
                        //else row.Cells.Add(new WorksheetCell("SP In Program", "HeaderStyle"));

                        //if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell("SP In Program", "HeaderStyle"));
                        //else row.Cells.Add(new WorksheetCell("SP in Central", "HeaderStyle"));


                        //if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell("SP in Central", "HeaderStyle"));
                        //else row.Cells.Add(new WorksheetCell("CA in Program", "HeaderStyle"));

                        //if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell("CA in Program", "HeaderStyle"));
                        //else row.Cells.Add(new WorksheetCell("CA in Central", "HeaderStyle"));

                        //if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell("CA in Central", "HeaderStyle"));
                        //else row.Cells.Add(new WorksheetCell("MS in Program", "HeaderStyle"));

                        //if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell("MS in Program", "HeaderStyle"));
                        //else row.Cells.Add(new WorksheetCell("MS in Central", "HeaderStyle"));

                        //if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell("MS in Central", "HeaderStyle"));


                        row.Cells.Add(new WorksheetCell("Add Operator in Program", "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell("Add Operator in Central", "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell("Intake Date – Central", "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell("Intake Date – Program", "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell("Program Enroll Status", "HeaderStyle"));

                    }

                     
                    
                    
                    


                    if (dtDet.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtDet.Rows)
                        {
                            row = sheet.Table.Rows.Add();

                            row.Cells.Add(new WorksheetCell(dr["LAST_NAME"].ToString() + ", " + dr["FIRST_NAME"].ToString(), "Default"));//(dr["FIRST_NAME"].ToString() + " " + dr["LAST_NAME"].ToString(), "Default"));
                            row.Cells.Add(new WorksheetCell(dr["MST_APP_NO"].ToString(), "Default"));

                            if (string.IsNullOrEmpty(propCentralHie.Trim()))
                            {
                                if (dr["PROG_MATRIX"].ToString().Trim() == "N")
                                    row.Cells.Add(new WorksheetCell(dr["PROG_MATRIX"].ToString(), "CellFill"));
                                else
                                    row.Cells.Add(new WorksheetCell(dr["PROG_MATRIX"].ToString(), "Default"));

                                if (dr["PROG_VERIFICATION"].ToString().Trim() == "N")
                                    row.Cells.Add(new WorksheetCell(dr["PROG_VERIFICATION"].ToString(), "CellFill"));
                                else
                                    row.Cells.Add(new WorksheetCell(dr["PROG_VERIFICATION"].ToString(), "Default"));

                                if (dr["REQ_FLDS"].ToString().Trim() == "N")
                                    row.Cells.Add(new WorksheetCell(dr["REQ_FLDS"].ToString(), "CellFill"));
                                else
                                    row.Cells.Add(new WorksheetCell(dr["REQ_FLDS"].ToString(), "Default"));

                                if (dr["PROG_SPM"].ToString().Trim() == "N")
                                    row.Cells.Add(new WorksheetCell(dr["PROG_SPM"].ToString(), "CellFill"));
                                else
                                    row.Cells.Add(new WorksheetCell(dr["PROG_SPM"].ToString(), "Default"));

                                if (dr["PROG_CASEACT"].ToString().Trim() == "N")
                                    row.Cells.Add(new WorksheetCell(dr["PROG_CASEACT"].ToString(), "CellFill"));
                                else
                                    row.Cells.Add(new WorksheetCell(dr["PROG_CASEACT"].ToString(), "Default"));

                                if (dr["PROG_CASEMS"].ToString().Trim() == "N")
                                    row.Cells.Add(new WorksheetCell(dr["PROG_CASEMS"].ToString(), "CellFill"));
                                else
                                    row.Cells.Add(new WorksheetCell(dr["PROG_CASEMS"].ToString(), "Default"));

                                row.Cells.Add(new WorksheetCell(dr["ADD_OPERATOR"].ToString(), "Default"));
                                //row.Cells.Add(new WorksheetCell(dr["CENTRAL_ADD_OPERATOR"].ToString(), "Default"));
                                //row.Cells.Add(new WorksheetCell(LookupDataAccess.Getdate(dr["CENTRAL_INTAKE_DATE"].ToString()).ToString(), "Default"));
                                row.Cells.Add(new WorksheetCell(LookupDataAccess.Getdate(dr["INTAKE_DATE"].ToString()).ToString(), "Default"));

                            }
                            else
                            {
                                row.Cells.Add(new WorksheetCell(dr["CENTRAL_APP"].ToString(), "Default"));

                                row.Cells.Add(new WorksheetCell(dr["CENTRAL_INTAKE"].ToString(), "Default"));
                                row.Cells.Add(new WorksheetCell(dr["CENTRAL_MEMBER"].ToString(), "Default"));

                                foreach (DataGridViewRow drSps in gvwApplicants.Rows)
                                {
                                    if (drSps.Cells["Selected"].Value.ToString().Trim() == "Y")
                                    {
                                        switch (drSps.Cells["Col_Code"].Value.ToString().Trim())
                                        {
                                            case "1":
                                                if (dtSummary.Rows.Count > 12)
                                                {
                                                    if (dr["CENTRAL_SIM"].ToString().Trim() == "N")
                                                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_SIM"].ToString(), "CellFill"));
                                                    else
                                                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_SIM"].ToString(), "Default"));
                                                }
                                                else
                                                {
                                                    if (dr["PROG_MATRIX"].ToString().Trim() == "N")
                                                        row.Cells.Add(new WorksheetCell(dr["PROG_MATRIX"].ToString(), "CellFill"));
                                                    else
                                                        row.Cells.Add(new WorksheetCell(dr["PROG_MATRIX"].ToString(), "Default"));
                                                }
                                                break;
                                            case "2":
                                                if (dtSummary.Rows.Count > 12)
                                                {
                                                    if (dr["PROG_MATRIX"].ToString().Trim() == "N")
                                                        row.Cells.Add(new WorksheetCell(dr["PROG_MATRIX"].ToString(), "CellFill"));
                                                    else
                                                        row.Cells.Add(new WorksheetCell(dr["PROG_MATRIX"].ToString(), "Default"));
                                                }
                                                else
                                                {
                                                    if (dr["CENTRAL_MATRIX"].ToString().Trim() == "N")
                                                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_MATRIX"].ToString(), "CellFill"));
                                                    else
                                                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_MATRIX"].ToString(), "Default"));
                                                }
                                                break;
                                            case "3":
                                                if (dtSummary.Rows.Count > 12)
                                                {
                                                    if (dr["CENTRAL_MATRIX"].ToString().Trim() == "N")
                                                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_MATRIX"].ToString(), "CellFill"));
                                                    else
                                                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_MATRIX"].ToString(), "Default"));
                                                }
                                                else
                                                {
                                                    if (dr["PROG_VERIFICATION"].ToString().Trim() == "N")
                                                        row.Cells.Add(new WorksheetCell(dr["PROG_VERIFICATION"].ToString(), "CellFill"));
                                                    else
                                                        row.Cells.Add(new WorksheetCell(dr["PROG_VERIFICATION"].ToString(), "Default"));
                                                }
                                                break;
                                            case "4":
                                                if (dtSummary.Rows.Count > 12)
                                                {
                                                    if (dr["PROG_VERIFICATION"].ToString().Trim() == "N")
                                                        row.Cells.Add(new WorksheetCell(dr["PROG_VERIFICATION"].ToString(), "CellFill"));
                                                    else
                                                        row.Cells.Add(new WorksheetCell(dr["PROG_VERIFICATION"].ToString(), "Default"));
                                                }
                                                else
                                                {
                                                    if (dr["CENTRAL_VERIFICATION"].ToString().Trim() == "N")
                                                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_VERIFICATION"].ToString(), "CellFill"));
                                                    else
                                                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_VERIFICATION"].ToString(), "Default"));
                                                }
                                                break;
                                            case "5":
                                                if (dtSummary.Rows.Count > 12)
                                                {
                                                    if (dr["CENTRAL_VERIFICATION"].ToString().Trim() == "N")
                                                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_VERIFICATION"].ToString(), "CellFill"));
                                                    else
                                                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_VERIFICATION"].ToString(), "Default"));
                                                }
                                                else
                                                {
                                                    if (dr["REQ_FLDS"].ToString().Trim() == "N")
                                                        row.Cells.Add(new WorksheetCell(dr["REQ_FLDS"].ToString(), "CellFill"));
                                                    else
                                                        row.Cells.Add(new WorksheetCell(dr["REQ_FLDS"].ToString(), "Default"));
                                                }
                                                break;
                                            case "6":
                                                if (dtSummary.Rows.Count > 12)
                                                {
                                                    if (dr["REQ_FLDS"].ToString().Trim() == "N")
                                                        row.Cells.Add(new WorksheetCell(dr["REQ_FLDS"].ToString(), "CellFill"));
                                                    else
                                                        row.Cells.Add(new WorksheetCell(dr["REQ_FLDS"].ToString(), "Default"));
                                                }
                                                else
                                                {
                                                    if (dr["PROG_SPM"].ToString().Trim() == "N")
                                                        row.Cells.Add(new WorksheetCell(dr["PROG_SPM"].ToString(), "CellFill"));
                                                    else
                                                        row.Cells.Add(new WorksheetCell(dr["PROG_SPM"].ToString(), "Default"));
                                                }
                                                break;
                                            case "7":
                                                if (dtSummary.Rows.Count > 12)
                                                {
                                                    if (dr["PROG_SPM"].ToString().Trim() == "N")
                                                        row.Cells.Add(new WorksheetCell(dr["PROG_SPM"].ToString(), "CellFill"));
                                                    else
                                                        row.Cells.Add(new WorksheetCell(dr["PROG_SPM"].ToString(), "Default"));
                                                }
                                                else
                                                {
                                                    if (dr["CENTRAL_SPM"].ToString().Trim() == "N")
                                                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_SPM"].ToString(), "CellFill"));
                                                    else
                                                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_SPM"].ToString(), "Default"));
                                                }
                                                break;
                                            case "8":
                                                if (dtSummary.Rows.Count > 12)
                                                {
                                                    if (dr["CENTRAL_SPM"].ToString().Trim() == "N")
                                                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_SPM"].ToString(), "CellFill"));
                                                    else
                                                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_SPM"].ToString(), "Default"));
                                                }
                                                else
                                                {
                                                    if (dr["PROG_CASEACT"].ToString().Trim() == "N")
                                                        row.Cells.Add(new WorksheetCell(dr["PROG_CASEACT"].ToString(), "CellFill"));
                                                    else
                                                        row.Cells.Add(new WorksheetCell(dr["PROG_CASEACT"].ToString(), "Default"));
                                                }
                                                break;
                                            case "9":
                                                if (dtSummary.Rows.Count > 12)
                                                {
                                                    if (dr["PROG_CASEACT"].ToString().Trim() == "N")
                                                        row.Cells.Add(new WorksheetCell(dr["PROG_CASEACT"].ToString(), "CellFill"));
                                                    else
                                                        row.Cells.Add(new WorksheetCell(dr["PROG_CASEACT"].ToString(), "Default"));
                                                }
                                                else
                                                {
                                                    if (dr["CENTRAL_CASEACT"].ToString().Trim() == "N")
                                                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_CASEACT"].ToString(), "CellFill"));
                                                    else
                                                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_CASEACT"].ToString(), "Default"));
                                                }
                                                break;
                                            case "10":
                                                if (dtSummary.Rows.Count > 12)
                                                {
                                                    if (dr["CENTRAL_CASEACT"].ToString().Trim() == "N")
                                                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_CASEACT"].ToString(), "CellFill"));
                                                    else
                                                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_CASEACT"].ToString(), "Default"));
                                                }
                                                else
                                                {
                                                    if (dr["PROG_CASEMS"].ToString().Trim() == "N")
                                                        row.Cells.Add(new WorksheetCell(dr["PROG_CASEMS"].ToString(), "CellFill"));
                                                    else
                                                        row.Cells.Add(new WorksheetCell(dr["PROG_CASEMS"].ToString(), "Default"));
                                                }
                                                break;
                                            case "11":
                                                if (dtSummary.Rows.Count > 12)
                                                {
                                                    if (dr["PROG_CASEMS"].ToString().Trim() == "N")
                                                        row.Cells.Add(new WorksheetCell(dr["PROG_CASEMS"].ToString(), "CellFill"));
                                                    else
                                                        row.Cells.Add(new WorksheetCell(dr["PROG_CASEMS"].ToString(), "Default"));
                                                }
                                                else
                                                {
                                                    if (dr["CENTRAL_CASEMS"].ToString().Trim() == "N")
                                                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_CASEMS"].ToString(), "CellFill"));
                                                    else
                                                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_CASEMS"].ToString(), "Default"));
                                                }
                                                break;
                                            case "12":
                                                if (dtSummary.Rows.Count > 12)
                                                {
                                                    if (dr["CENTRAL_CASEMS"].ToString().Trim() == "N")
                                                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_CASEMS"].ToString(), "CellFill"));
                                                    else
                                                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_CASEMS"].ToString(), "Default"));
                                                }
                                                break;
                                        }
                                    }
                                }


                                    //if (dtSummary.Rows.Count > 12)
                                    //{
                                    //    if (dr["CENTRAL_SIM"].ToString().Trim() == "N")
                                    //        row.Cells.Add(new WorksheetCell(dr["CENTRAL_SIM"].ToString(), "CellFill"));
                                    //    else
                                    //        row.Cells.Add(new WorksheetCell(dr["CENTRAL_SIM"].ToString(), "Default"));
                                    //}
                                    //else
                                    //{
                                    //    if (dr["PROG_MATRIX"].ToString().Trim() == "N")
                                    //        row.Cells.Add(new WorksheetCell(dr["PROG_MATRIX"].ToString(), "CellFill"));
                                    //    else
                                    //        row.Cells.Add(new WorksheetCell(dr["PROG_MATRIX"].ToString(), "Default"));
                                    //}


                                    //if (dtSummary.Rows.Count > 12)
                                    //{
                                    //    if (dr["PROG_MATRIX"].ToString().Trim() == "N")
                                    //        row.Cells.Add(new WorksheetCell(dr["PROG_MATRIX"].ToString(), "CellFill"));
                                    //    else
                                    //        row.Cells.Add(new WorksheetCell(dr["PROG_MATRIX"].ToString(), "Default"));
                                    //}
                                    //else
                                    //{
                                    //    if (dr["CENTRAL_MATRIX"].ToString().Trim() == "N")
                                    //        row.Cells.Add(new WorksheetCell(dr["CENTRAL_MATRIX"].ToString(), "CellFill"));
                                    //    else
                                    //        row.Cells.Add(new WorksheetCell(dr["CENTRAL_MATRIX"].ToString(), "Default"));
                                    //}


                                    //if (dtSummary.Rows.Count > 12)
                                    //{
                                    //    if (dr["CENTRAL_MATRIX"].ToString().Trim() == "N")
                                    //        row.Cells.Add(new WorksheetCell(dr["CENTRAL_MATRIX"].ToString(), "CellFill"));
                                    //    else
                                    //        row.Cells.Add(new WorksheetCell(dr["CENTRAL_MATRIX"].ToString(), "Default"));
                                    //}
                                    //else
                                    //{
                                    //    if (dr["PROG_VERIFICATION"].ToString().Trim() == "N")
                                    //        row.Cells.Add(new WorksheetCell(dr["PROG_VERIFICATION"].ToString(), "CellFill"));
                                    //    else
                                    //        row.Cells.Add(new WorksheetCell(dr["PROG_VERIFICATION"].ToString(), "Default"));
                                    //}

                                    //if (dtSummary.Rows.Count > 12)
                                    //{
                                    //    if (dr["PROG_VERIFICATION"].ToString().Trim() == "N")
                                    //        row.Cells.Add(new WorksheetCell(dr["PROG_VERIFICATION"].ToString(), "CellFill"));
                                    //    else
                                    //        row.Cells.Add(new WorksheetCell(dr["PROG_VERIFICATION"].ToString(), "Default"));
                                    //}
                                    //else
                                    //{
                                    //    if (dr["CENTRAL_VERIFICATION"].ToString().Trim() == "N")
                                    //        row.Cells.Add(new WorksheetCell(dr["CENTRAL_VERIFICATION"].ToString(), "CellFill"));
                                    //    else
                                    //        row.Cells.Add(new WorksheetCell(dr["CENTRAL_VERIFICATION"].ToString(), "Default"));
                                    //}

                                    //if (dtSummary.Rows.Count > 12)
                                    //{
                                    //    if (dr["CENTRAL_VERIFICATION"].ToString().Trim() == "N")
                                    //        row.Cells.Add(new WorksheetCell(dr["CENTRAL_VERIFICATION"].ToString(), "CellFill"));
                                    //    else
                                    //        row.Cells.Add(new WorksheetCell(dr["CENTRAL_VERIFICATION"].ToString(), "Default"));
                                    //}
                                    //else
                                    //{
                                    //    if (dr["REQ_FLDS"].ToString().Trim() == "N")
                                    //        row.Cells.Add(new WorksheetCell(dr["REQ_FLDS"].ToString(), "CellFill"));
                                    //    else
                                    //        row.Cells.Add(new WorksheetCell(dr["REQ_FLDS"].ToString(), "Default"));
                                    //}

                                    //if (dtSummary.Rows.Count > 12)
                                    //{
                                    //    if (dr["REQ_FLDS"].ToString().Trim() == "N")
                                    //        row.Cells.Add(new WorksheetCell(dr["REQ_FLDS"].ToString(), "CellFill"));
                                    //    else
                                    //        row.Cells.Add(new WorksheetCell(dr["REQ_FLDS"].ToString(), "Default"));
                                    //}
                                    //else
                                    //{
                                    //    if (dr["PROG_SPM"].ToString().Trim() == "N")
                                    //        row.Cells.Add(new WorksheetCell(dr["PROG_SPM"].ToString(), "CellFill"));
                                    //    else
                                    //        row.Cells.Add(new WorksheetCell(dr["PROG_SPM"].ToString(), "Default"));
                                    //}

                                    //if (dtSummary.Rows.Count > 12)
                                    //{
                                    //    if (dr["PROG_SPM"].ToString().Trim() == "N")
                                    //        row.Cells.Add(new WorksheetCell(dr["PROG_SPM"].ToString(), "CellFill"));
                                    //    else
                                    //        row.Cells.Add(new WorksheetCell(dr["PROG_SPM"].ToString(), "Default"));
                                    //}
                                    //else
                                    //{
                                    //    if (dr["CENTRAL_SPM"].ToString().Trim() == "N")
                                    //        row.Cells.Add(new WorksheetCell(dr["CENTRAL_SPM"].ToString(), "CellFill"));
                                    //    else
                                    //        row.Cells.Add(new WorksheetCell(dr["CENTRAL_SPM"].ToString(), "Default"));
                                    //}

                                    //if (dtSummary.Rows.Count > 12)
                                    //{
                                    //    if (dr["CENTRAL_SPM"].ToString().Trim() == "N")
                                    //        row.Cells.Add(new WorksheetCell(dr["CENTRAL_SPM"].ToString(), "CellFill"));
                                    //    else
                                    //        row.Cells.Add(new WorksheetCell(dr["CENTRAL_SPM"].ToString(), "Default"));
                                    //}
                                    //else
                                    //{
                                    //    if (dr["PROG_CASEACT"].ToString().Trim() == "N")
                                    //        row.Cells.Add(new WorksheetCell(dr["PROG_CASEACT"].ToString(), "CellFill"));
                                    //    else
                                    //        row.Cells.Add(new WorksheetCell(dr["PROG_CASEACT"].ToString(), "Default"));
                                    //}

                                    //if (dtSummary.Rows.Count > 12)
                                    //{
                                    //    if (dr["PROG_CASEACT"].ToString().Trim() == "N")
                                    //        row.Cells.Add(new WorksheetCell(dr["PROG_CASEACT"].ToString(), "CellFill"));
                                    //    else
                                    //        row.Cells.Add(new WorksheetCell(dr["PROG_CASEACT"].ToString(), "Default"));
                                    //}
                                    //else
                                    //{
                                    //    if (dr["CENTRAL_CASEACT"].ToString().Trim() == "N")
                                    //        row.Cells.Add(new WorksheetCell(dr["CENTRAL_CASEACT"].ToString(), "CellFill"));
                                    //    else
                                    //        row.Cells.Add(new WorksheetCell(dr["CENTRAL_CASEACT"].ToString(), "Default"));
                                    //}

                                    //if (dtSummary.Rows.Count > 12)
                                    //{
                                    //    if (dr["CENTRAL_CASEACT"].ToString().Trim() == "N")
                                    //        row.Cells.Add(new WorksheetCell(dr["CENTRAL_CASEACT"].ToString(), "CellFill"));
                                    //    else
                                    //        row.Cells.Add(new WorksheetCell(dr["CENTRAL_CASEACT"].ToString(), "Default"));
                                    //}
                                    //else
                                    //{
                                    //    if (dr["PROG_CASEMS"].ToString().Trim() == "N")
                                    //        row.Cells.Add(new WorksheetCell(dr["PROG_CASEMS"].ToString(), "CellFill"));
                                    //    else
                                    //        row.Cells.Add(new WorksheetCell(dr["PROG_CASEMS"].ToString(), "Default"));
                                    //}

                                    //if (dtSummary.Rows.Count > 12)
                                    //{
                                    //    if (dr["PROG_CASEMS"].ToString().Trim() == "N")
                                    //        row.Cells.Add(new WorksheetCell(dr["PROG_CASEMS"].ToString(), "CellFill"));
                                    //    else
                                    //        row.Cells.Add(new WorksheetCell(dr["PROG_CASEMS"].ToString(), "Default"));
                                    //}
                                    //else
                                    //{
                                    //    if (dr["CENTRAL_CASEMS"].ToString().Trim() == "N")
                                    //        row.Cells.Add(new WorksheetCell(dr["CENTRAL_CASEMS"].ToString(), "CellFill"));
                                    //    else
                                    //        row.Cells.Add(new WorksheetCell(dr["CENTRAL_CASEMS"].ToString(), "Default"));
                                    //}

                                    //if (dtSummary.Rows.Count > 12)
                                    //{
                                    //    if (dr["CENTRAL_CASEMS"].ToString().Trim() == "N")
                                    //        row.Cells.Add(new WorksheetCell(dr["CENTRAL_CASEMS"].ToString(), "CellFill"));
                                    //    else
                                    //        row.Cells.Add(new WorksheetCell(dr["CENTRAL_CASEMS"].ToString(), "Default"));
                                    //}

                                    row.Cells.Add(new WorksheetCell(dr["ADD_OPERATOR"].ToString(), "Default"));
                                    row.Cells.Add(new WorksheetCell(dr["CENTRAL_ADD_OPERATOR"].ToString(), "Default"));
                                    row.Cells.Add(new WorksheetCell(LookupDataAccess.Getdate(dr["CENTRAL_INTAKE_DATE"].ToString()).ToString(), "Default"));
                                    row.Cells.Add(new WorksheetCell(LookupDataAccess.Getdate(dr["INTAKE_DATE"].ToString()).ToString(), "Default"));

                                }


                                //row.Cells.Add(new WorksheetCell(dr["CENTRAL_APP"].ToString(), "Default"));

                                //if (strCodes.Length > 0)
                                //{
                                //    string[] values = strCodes.Split(',');
                                //    for (int i = 0; i < values.Length; i++)
                                //    {
                                //        values[i] = values[i].Trim();

                                //        switch (values[i])
                                //        {
                                //            case "1":
                                //                row.Cells.Add(new WorksheetCell(dr["CENTRAL_INTAKE"].ToString(), "Default"));
                                //                row.Cells.Add(new WorksheetCell(dr["CENTRAL_MEMBER"].ToString(), "Default")); break;
                                //            case "2":
                                //                if (dtSummary.Rows.Count > 12)
                                //                {
                                //                    if (dr["CENTRAL_SIM"].ToString().Trim() == "N")
                                //                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_SIM"].ToString(), "CellFill"));
                                //                    else
                                //                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_SIM"].ToString(), "Default"));
                                //                }
                                //                else
                                //                {
                                //                    if (dr["PROG_MATRIX"].ToString().Trim() == "N")
                                //                        row.Cells.Add(new WorksheetCell(dr["PROG_MATRIX"].ToString(), "CellFill"));
                                //                    else
                                //                        row.Cells.Add(new WorksheetCell(dr["PROG_MATRIX"].ToString(), "Default"));
                                //                }
                                //                break;
                                //            case "3":
                                //                if (dtSummary.Rows.Count > 12)
                                //                {
                                //                    if (dr["PROG_MATRIX"].ToString().Trim() == "N")
                                //                        row.Cells.Add(new WorksheetCell(dr["PROG_MATRIX"].ToString(), "CellFill"));
                                //                    else
                                //                        row.Cells.Add(new WorksheetCell(dr["PROG_MATRIX"].ToString(), "Default"));
                                //                }
                                //                else
                                //                {
                                //                    if (dr["CENTRAL_MATRIX"].ToString().Trim() == "N")
                                //                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_MATRIX"].ToString(), "CellFill"));
                                //                    else
                                //                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_MATRIX"].ToString(), "Default"));
                                //                }
                                //                break;
                                //            case "4":
                                //                if (dtSummary.Rows.Count > 12)
                                //                {
                                //                    if (dr["CENTRAL_MATRIX"].ToString().Trim() == "N")
                                //                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_MATRIX"].ToString(), "CellFill"));
                                //                    else
                                //                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_MATRIX"].ToString(), "Default"));
                                //                }
                                //                else
                                //                {
                                //                    if (dr["PROG_VERIFICATION"].ToString().Trim() == "N")
                                //                        row.Cells.Add(new WorksheetCell(dr["PROG_VERIFICATION"].ToString(), "CellFill"));
                                //                    else
                                //                        row.Cells.Add(new WorksheetCell(dr["PROG_VERIFICATION"].ToString(), "Default"));
                                //                }
                                //                //if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell(dr["CENTRAL_MATRIX"].ToString(), "Default"));
                                //                //else row.Cells.Add(new WorksheetCell(dr["PROG_VERIFICATION"].ToString(), "Default")); 
                                //                break;
                                //            case "5":
                                //                if (dtSummary.Rows.Count > 12)
                                //                {
                                //                    if (dr["PROG_VERIFICATION"].ToString().Trim() == "N")
                                //                        row.Cells.Add(new WorksheetCell(dr["PROG_VERIFICATION"].ToString(), "CellFill"));
                                //                    else
                                //                        row.Cells.Add(new WorksheetCell(dr["PROG_VERIFICATION"].ToString(), "Default"));
                                //                }
                                //                else
                                //                {
                                //                    if (dr["CENTRAL_VERIFICATION"].ToString().Trim() == "N")
                                //                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_VERIFICATION"].ToString(), "CellFill"));
                                //                    else
                                //                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_VERIFICATION"].ToString(), "Default"));
                                //                }
                                //                //if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell(dr["PROG_VERIFICATION"].ToString(), "Default"));
                                //                //else row.Cells.Add(new WorksheetCell(dr["CENTRAL_VERIFICATION"].ToString(), "Default"));
                                //                break;
                                //            case "6":
                                //                if (dtSummary.Rows.Count > 12)
                                //                {
                                //                    if (dr["CENTRAL_VERIFICATION"].ToString().Trim() == "N")
                                //                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_VERIFICATION"].ToString(), "CellFill"));
                                //                    else
                                //                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_VERIFICATION"].ToString(), "Default"));
                                //                }
                                //                else
                                //                {
                                //                    if (dr["REQ_FLDS"].ToString().Trim() == "N")
                                //                        row.Cells.Add(new WorksheetCell(dr["REQ_FLDS"].ToString(), "CellFill"));
                                //                    else
                                //                        row.Cells.Add(new WorksheetCell(dr["REQ_FLDS"].ToString(), "Default"));
                                //                }
                                //                //if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell(dr["CENTRAL_VERIFICATION"].ToString(), "Default"));
                                //                //else row.Cells.Add(new WorksheetCell(dr["REQ_FLDS"].ToString(), "Default"));
                                //                break;
                                //            case "7":
                                //                if (dtSummary.Rows.Count > 12)
                                //                {
                                //                    if (dr["REQ_FLDS"].ToString().Trim() == "N")
                                //                        row.Cells.Add(new WorksheetCell(dr["REQ_FLDS"].ToString(), "CellFill"));
                                //                    else
                                //                        row.Cells.Add(new WorksheetCell(dr["REQ_FLDS"].ToString(), "Default"));
                                //                }
                                //                else
                                //                {
                                //                    if (dr["PROG_SPM"].ToString().Trim() == "N")
                                //                        row.Cells.Add(new WorksheetCell(dr["PROG_SPM"].ToString(), "CellFill"));
                                //                    else
                                //                        row.Cells.Add(new WorksheetCell(dr["PROG_SPM"].ToString(), "Default"));
                                //                }
                                //                //if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell(dr["REQ_FLDS"].ToString(), "Default"));
                                //                //else row.Cells.Add(new WorksheetCell(dr["PROG_SPM"].ToString(), "Default"));
                                //                break;
                                //            case "8":
                                //                if (dtSummary.Rows.Count > 12)
                                //                {
                                //                    if (dr["PROG_SPM"].ToString().Trim() == "N")
                                //                        row.Cells.Add(new WorksheetCell(dr["PROG_SPM"].ToString(), "CellFill"));
                                //                    else
                                //                        row.Cells.Add(new WorksheetCell(dr["PROG_SPM"].ToString(), "Default"));
                                //                }
                                //                else
                                //                {
                                //                    if (dr["CENTRAL_SPM"].ToString().Trim() == "N")
                                //                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_SPM"].ToString(), "CellFill"));
                                //                    else
                                //                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_SPM"].ToString(), "Default"));
                                //                }
                                //                //if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell(dr["PROG_SPM"].ToString(), "Default"));
                                //                //else row.Cells.Add(new WorksheetCell(dr["CENTRAL_SPM"].ToString(), "Default"));
                                //                break;
                                //            case "9":
                                //                if (dtSummary.Rows.Count > 12)
                                //                {
                                //                    if (dr["CENTRAL_SPM"].ToString().Trim() == "N")
                                //                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_SPM"].ToString(), "CellFill"));
                                //                    else
                                //                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_SPM"].ToString(), "Default"));
                                //                }
                                //                else
                                //                {
                                //                    if (dr["PROG_CASEACT"].ToString().Trim() == "N")
                                //                        row.Cells.Add(new WorksheetCell(dr["PROG_CASEACT"].ToString(), "CellFill"));
                                //                    else
                                //                        row.Cells.Add(new WorksheetCell(dr["PROG_CASEACT"].ToString(), "Default"));
                                //                }
                                //                //if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell(dr["CENTRAL_SPM"].ToString(), "Default"));
                                //                //else row.Cells.Add(new WorksheetCell(dr["PROG_CASEACT"].ToString(), "Default"));
                                //                break;
                                //            case "10":
                                //                if (dtSummary.Rows.Count > 12)
                                //                {
                                //                    if (dr["PROG_CASEACT"].ToString().Trim() == "N")
                                //                        row.Cells.Add(new WorksheetCell(dr["PROG_CASEACT"].ToString(), "CellFill"));
                                //                    else
                                //                        row.Cells.Add(new WorksheetCell(dr["PROG_CASEACT"].ToString(), "Default"));
                                //                }
                                //                else
                                //                {
                                //                    if (dr["CENTRAL_CASEACT"].ToString().Trim() == "N")
                                //                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_CASEACT"].ToString(), "CellFill"));
                                //                    else
                                //                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_CASEACT"].ToString(), "Default"));
                                //                }
                                //                //if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell(dr["PROG_CASEACT"].ToString(), "Default"));
                                //                //else row.Cells.Add(new WorksheetCell(dr["CENTRAL_CASEACT"].ToString(), "Default"));
                                //                break;
                                //            case "11":
                                //                if (dtSummary.Rows.Count > 12)
                                //                {
                                //                    if (dr["CENTRAL_CASEACT"].ToString().Trim() == "N")
                                //                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_CASEACT"].ToString(), "CellFill"));
                                //                    else
                                //                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_CASEACT"].ToString(), "Default"));
                                //                }
                                //                else
                                //                {
                                //                    if (dr["PROG_CASEMS"].ToString().Trim() == "N")
                                //                        row.Cells.Add(new WorksheetCell(dr["PROG_CASEMS"].ToString(), "CellFill"));
                                //                    else
                                //                        row.Cells.Add(new WorksheetCell(dr["PROG_CASEMS"].ToString(), "Default"));
                                //                }
                                //                //if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell(dr["CENTRAL_CASEACT"].ToString(), "Default"));
                                //                //else row.Cells.Add(new WorksheetCell(dr["PROG_CASEMS"].ToString(), "Default"));
                                //                break;
                                //            case "12":
                                //                if (dtSummary.Rows.Count > 12)
                                //                {
                                //                    if (dr["PROG_CASEMS"].ToString().Trim() == "N")
                                //                        row.Cells.Add(new WorksheetCell(dr["PROG_CASEMS"].ToString(), "CellFill"));
                                //                    else
                                //                        row.Cells.Add(new WorksheetCell(dr["PROG_CASEMS"].ToString(), "Default"));
                                //                }
                                //                else
                                //                {
                                //                    if (dr["CENTRAL_CASEMS"].ToString().Trim() == "N")
                                //                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_CASEMS"].ToString(), "CellFill"));
                                //                    else
                                //                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_CASEMS"].ToString(), "Default"));
                                //                }
                                //                //if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell(dr["PROG_CASEMS"].ToString(), "Default"));
                                //                //else row.Cells.Add(new WorksheetCell(dr["CENTRAL_CASEMS"].ToString(), "Default"));
                                //                break;
                                //            case "13":
                                //                if (dtSummary.Rows.Count > 12)
                                //                {
                                //                    if (dr["CENTRAL_CASEMS"].ToString().Trim() == "N")
                                //                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_CASEMS"].ToString(), "CellFill"));
                                //                    else
                                //                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_CASEMS"].ToString(), "Default"));
                                //                }
                                //                //row.Cells.Add(new WorksheetCell(dr["CENTRAL_CASEMS"].ToString(), "Default"));
                                //                break;
                                //        }

                                //    }
                                //}




                                string Status_Desc = string.Empty;

                                if (AgyTabs_List.Count > 0)
                                {
                                    AGYTABSEntity SelEntity = AgyTabs_List.Find(u => u.Table_Code.Equals(dr["PROG_ENRL_STATUS"].ToString().Trim()));
                                    if (SelEntity != null)
                                        Status_Desc = SelEntity.Code_Desc.Trim();
                                }


                                row.Cells.Add(new WorksheetCell(Status_Desc, "Default"));


                            }
                        

                    }

                    FileStream stream = new FileStream(PdfName, FileMode.Create);

                    book.Save(stream);
                    stream.Close();


                }
                catch (Exception ex) { }

            }
        }


        private void OnExcel_Report4(object sender, FormClosedEventArgs e)
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
                    {
                        DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim());
                    }
                }
                catch (Exception ex)
                {
                    AlertBox.Show("Error", MessageBoxIcon.Error);
                }


                try
                {
                    string Tmpstr = PdfName + ".xls";
                    if (File.Exists(Tmpstr))
                        File.Delete(Tmpstr);
                }
                catch (Exception ex)
                {
                    int length = 8;
                    string newFileName = System.Guid.NewGuid().ToString();
                    newFileName = newFileName.Replace("-", string.Empty);

                    Random_Filename = PdfName + newFileName.Substring(0, length) + ".xls";
                }


                if (!string.IsNullOrEmpty(Random_Filename))
                    PdfName = Random_Filename;
                else
                    PdfName += ".xls";

                Workbook book = new Workbook();

                Worksheet sheet = book.Worksheets.Add("Sheet1");
                //this.GenerateStyles(book.Styles);

                //ExcelDocument xlWorkSheet = new ExcelDocument();
                string strCodes = string.Empty;
                if (string.IsNullOrEmpty(propCentralHie.Trim()))
                {
                    sheet.Table.Columns.Add(new WorksheetColumn(200));
                    sheet.Table.Columns.Add(new WorksheetColumn(100));
                    sheet.Table.Columns.Add(new WorksheetColumn(100));
                    //sheet.Table.Columns.Add(new WorksheetColumn(80));

                    foreach (DataGridViewRow drSps in gvwApplicants.Rows)
                    {
                        if (drSps.Cells["Selected"].Value.ToString().Trim() == "Y")
                        {
                            sheet.Table.Columns.Add(new WorksheetColumn(180));
                            if (drSps.Cells["Col_Code"].Value.ToString().Trim() == "1")
                                sheet.Table.Columns.Add(new WorksheetColumn(150));
                            if (!strCodes.Equals(string.Empty)) strCodes += ",";
                            strCodes += drSps.Cells["Col_Code"].Value.ToString().Trim();
                        }
                    }

                    sheet.Table.Columns.Add(new WorksheetColumn(150));
                    //sheet.Table.Columns.Add(new WorksheetColumn(150));
                    //sheet.Table.Columns.Add(new WorksheetColumn(120));
                    sheet.Table.Columns.Add(new WorksheetColumn(120));
                    sheet.Table.Columns.Add(new WorksheetColumn(180));
                }
                else if (dtSummary.Rows.Count == 12)
                {
                    sheet.Table.Columns.Add(new WorksheetColumn(200));
                    sheet.Table.Columns.Add(new WorksheetColumn(100));
                    sheet.Table.Columns.Add(new WorksheetColumn(100));
                    //sheet.Table.Columns.Add(new WorksheetColumn(80));

                    foreach (DataGridViewRow drSps in gvwApplicants.Rows)
                    {
                        if (drSps.Cells["Selected"].Value.ToString().Trim() == "Y")
                        {
                            sheet.Table.Columns.Add(new WorksheetColumn(180));
                            if (drSps.Cells["Col_Code"].Value.ToString().Trim() == "1")
                                sheet.Table.Columns.Add(new WorksheetColumn(150));
                            if (!strCodes.Equals(string.Empty)) strCodes += ",";
                            strCodes += drSps.Cells["Col_Code"].Value.ToString().Trim();
                        }
                    }
                    
                    sheet.Table.Columns.Add(new WorksheetColumn(150));
                    sheet.Table.Columns.Add(new WorksheetColumn(150));
                    sheet.Table.Columns.Add(new WorksheetColumn(120));
                    sheet.Table.Columns.Add(new WorksheetColumn(120));
                    sheet.Table.Columns.Add(new WorksheetColumn(180));
                    
                }
                else if (dtSummary.Rows.Count > 12)
                {
                    sheet.Table.Columns.Add(new WorksheetColumn(200));
                    sheet.Table.Columns.Add(new WorksheetColumn(100));
                    sheet.Table.Columns.Add(new WorksheetColumn(100));
                    //sheet.Table.Columns.Add(new WorksheetColumn(80));

                    foreach (DataGridViewRow drSps in gvwApplicants.Rows)
                    {
                        if (drSps.Cells["Selected"].Value.ToString().Trim() == "Y")
                        {
                            sheet.Table.Columns.Add(new WorksheetColumn(180));
                            if (drSps.Cells["Col_Code"].Value.ToString().Trim() == "1")
                                sheet.Table.Columns.Add(new WorksheetColumn(180));
                            if (!strCodes.Equals(string.Empty)) strCodes += ",";
                            strCodes += drSps.Cells["Col_Code"].Value.ToString().Trim();
                        }
                    }

                    
                    sheet.Table.Columns.Add(new WorksheetColumn(150));
                    sheet.Table.Columns.Add(new WorksheetColumn(150));
                    sheet.Table.Columns.Add(new WorksheetColumn(120));
                    sheet.Table.Columns.Add(new WorksheetColumn(120));
                    sheet.Table.Columns.Add(new WorksheetColumn(180));
                    
                }

                WorksheetStyle styleb = book.Styles.Add("HeaderStyleBlue");
                styleb.Font.FontName = "Tahoma";
                styleb.Font.Size = 12;
                styleb.Font.Bold = true;
                styleb.Font.Color = "#0000FF";
                styleb.Alignment.Horizontal = StyleHorizontalAlignment.Center;


                WorksheetStyle style = book.Styles.Add("HeaderStyle");
                style.Font.FontName = "Tahoma";
                style.Font.Size = 12;
                style.Font.Bold = true;
                style.Alignment.Horizontal = StyleHorizontalAlignment.Center;

                WorksheetStyle style1 = book.Styles.Add("HeaderStyle1");
                style1.Font.FontName = "Tahoma";
                style1.Font.Size = 12;
                style1.Font.Bold = true;
                style1.Alignment.Horizontal = StyleHorizontalAlignment.Left;

                WorksheetStyle style2 = book.Styles.Add("CellStyle");
                style2.Font.FontName = "Tahoma";
                style2.Font.Size = 10;
                style2.Font.Color = "Blue";
                style2.Alignment.Horizontal = StyleHorizontalAlignment.Left;

                WorksheetStyle CellFill = book.Styles.Add("CellFill");
                CellFill.Font.FontName = "Tahoma";
                CellFill.Font.Size = 10;
                CellFill.Interior.Color = "Yellow";
                CellFill.Interior.Pattern = StyleInteriorPattern.Solid;
                CellFill.Alignment.Horizontal = StyleHorizontalAlignment.Left;


                style = book.Styles.Add("Default");
                style.Font.FontName = "Tahoma";
                style.Font.Size = 10;

                AGYTABSEntity searchAgytabs = new AGYTABSEntity(true);
                searchAgytabs.Tabs_Type = "S0023";
                List<AGYTABSEntity> AgyTabs_List = _model.AdhocData.Browse_AGYTABS(searchAgytabs);

                try
                {
                    WorksheetRow row = sheet.Table.Rows.Add();



                    WorksheetCell cell = row.Cells.Add("Customer Intake Quality Control Report", DataType.String, "HeaderStyleBlue");
                    cell.MergeAcross = 20;

                    //int excelcolumn = 0;

                   
                    //excelcolumn = excelcolumn + 2;
                    row = sheet.Table.Rows.Add();
                    row.Cells.Add(new WorksheetCell("Name(last and first)", "HeaderStyle"));//Name (first and last)
                    row.Cells.Add(new WorksheetCell("Program App #", "HeaderStyle"));
                    //row.Cells.Add(new WorksheetCell("Central App #", "HeaderStyle"));

                    if (string.IsNullOrEmpty(propCentralHie.Trim()))
                    {
                        string[] values = strCodes.Split(',');
                        for (int i = 0; i < values.Length; i++)
                        {
                            values[i] = values[i].Trim();

                            switch (values[i])
                            {
                                case "2": row.Cells.Add(new WorksheetCell("Program Matrix", "HeaderStyle"));break;
                                case "3": row.Cells.Add(new WorksheetCell("Income Verification - Program", "HeaderStyle")); break;
                                case "4": row.Cells.Add(new WorksheetCell("Required Intake Fields - Program", "HeaderStyle")); break;
                                case "5": row.Cells.Add(new WorksheetCell("SP In Program", "HeaderStyle")); break;
                                case "6": row.Cells.Add(new WorksheetCell("Service in Program", "HeaderStyle")); break;
                                case "7": row.Cells.Add(new WorksheetCell("Outcome in Program", "HeaderStyle")); break;
                            }
                        }
                        row.Cells.Add(new WorksheetCell("Add Operator in Program", "HeaderStyle"));
                        //row.Cells.Add(new WorksheetCell("Add Operator in Central", "HeaderStyle"));
                        //row.Cells.Add(new WorksheetCell("Intake Date – Central", "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell("Intake Date – Program", "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell("Program Enroll Status", "HeaderStyle"));
                    }
                    else
                    {
                        row.Cells.Add(new WorksheetCell("Central App #", "HeaderStyle"));
                        if (strCodes.Length > 0)
                        {
                            string[] values = strCodes.Split(',');
                            for (int i = 0; i < values.Length; i++)
                            {
                                values[i] = values[i].Trim();

                                switch (values[i])
                                {
                                    case "1":
                                        row.Cells.Add(new WorksheetCell("Central Applicant", "HeaderStyle"));
                                        row.Cells.Add(new WorksheetCell("Central Member", "HeaderStyle")); break;
                                    case "2":
                                        if (dtSummary.Rows.Count > 12)
                                            row.Cells.Add(new WorksheetCell("SIM", "HeaderStyle"));
                                        else row.Cells.Add(new WorksheetCell("Program Matrix", "HeaderStyle")); break;
                                    case "3":
                                        if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell("Program Matrix", "HeaderStyle"));
                                        else row.Cells.Add(new WorksheetCell("Central Matrix", "HeaderStyle")); break;
                                    case "4":
                                        if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell("Central Matrix", "HeaderStyle"));
                                        else row.Cells.Add(new WorksheetCell("Income Verification - Program", "HeaderStyle")); break;
                                    case "5":
                                        if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell("Income Verification - Program", "HeaderStyle"));
                                        else row.Cells.Add(new WorksheetCell("Income Verification - Central", "HeaderStyle")); break;
                                    case "6":
                                        if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell("Income Verification - Central", "HeaderStyle"));
                                        else row.Cells.Add(new WorksheetCell("Required Intake Fields - Program", "HeaderStyle")); break;
                                    case "7":
                                        if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell("Required Intake Fields - Program", "HeaderStyle"));
                                        else row.Cells.Add(new WorksheetCell("SP In Program", "HeaderStyle")); break;
                                    case "8":
                                        if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell("SP In Program", "HeaderStyle"));
                                        else row.Cells.Add(new WorksheetCell("SP in Central", "HeaderStyle")); break;
                                    case "9":
                                        if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell("SP in Central", "HeaderStyle"));
                                        else row.Cells.Add(new WorksheetCell("Service in Program", "HeaderStyle")); break;
                                    case "10":
                                        if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell("Service in Program", "HeaderStyle"));
                                        else row.Cells.Add(new WorksheetCell("Service in Central", "HeaderStyle")); break;
                                    case "11":
                                        if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell("Service in Central", "HeaderStyle"));
                                        else row.Cells.Add(new WorksheetCell("Outcome in Program", "HeaderStyle")); break;
                                    case "12":
                                        if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell("Outcome in Program", "HeaderStyle"));
                                        else row.Cells.Add(new WorksheetCell("Outcome in Central", "HeaderStyle")); break;
                                    case "13":
                                        if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell("Outcome in Central", "HeaderStyle"));
                                        break;
                                }


                            }
                        }


                        row.Cells.Add(new WorksheetCell("Add Operator in Program", "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell("Add Operator in Central", "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell("Intake Date – Central", "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell("Intake Date – Program", "HeaderStyle"));
                        row.Cells.Add(new WorksheetCell("Program Enroll Status", "HeaderStyle"));
                    }

                   
                    if (dtDet.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtDet.Rows)
                        {
                            row = sheet.Table.Rows.Add();

                            row.Cells.Add(new WorksheetCell(dr["LAST_NAME"].ToString() + ", " + dr["FIRST_NAME"].ToString(), "Default"));//(dr["FIRST_NAME"].ToString() + " " + dr["LAST_NAME"].ToString(), "Default"));
                            row.Cells.Add(new WorksheetCell(dr["MST_APP_NO"].ToString(), "Default"));
                            

                            if (string.IsNullOrEmpty(propCentralHie.Trim()))
                            {
                                if (strCodes.Length > 0)
                                {
                                    string[] values = strCodes.Split(',');
                                    for (int i = 0; i < values.Length; i++)
                                    {
                                        values[i] = values[i].Trim();

                                        switch (values[i])
                                        {
                                            case "2":
                                                if (dr["PROG_MATRIX"].ToString().Trim() == "N")
                                                    row.Cells.Add(new WorksheetCell(dr["PROG_MATRIX"].ToString(), "CellFill"));
                                                else
                                                    row.Cells.Add(new WorksheetCell(dr["PROG_MATRIX"].ToString(), "Default")); break;
                                            case "3":
                                                if (dr["PROG_VERIFICATION"].ToString().Trim() == "N")
                                                    row.Cells.Add(new WorksheetCell(dr["PROG_VERIFICATION"].ToString(), "CellFill"));
                                                else
                                                    row.Cells.Add(new WorksheetCell(dr["PROG_VERIFICATION"].ToString(), "Default")); break;
                                            case "4":
                                                if (dr["REQ_FLDS"].ToString().Trim() == "N")
                                                    row.Cells.Add(new WorksheetCell(dr["REQ_FLDS"].ToString(), "CellFill"));
                                                else
                                                    row.Cells.Add(new WorksheetCell(dr["REQ_FLDS"].ToString(), "Default")); break;
                                            case "5":
                                                if (dr["PROG_SPM"].ToString().Trim() == "N")
                                                    row.Cells.Add(new WorksheetCell(dr["PROG_SPM"].ToString(), "CellFill"));
                                                else
                                                    row.Cells.Add(new WorksheetCell(dr["PROG_SPM"].ToString(), "Default")); break;
                                            case "6":
                                                if (dr["PROG_CASEACT"].ToString().Trim() == "N")
                                                    row.Cells.Add(new WorksheetCell(dr["PROG_CASEACT"].ToString(), "CellFill"));
                                                else
                                                    row.Cells.Add(new WorksheetCell(dr["PROG_CASEACT"].ToString(), "Default")); break;
                                            case "7":
                                                if (dr["PROG_CASEMS"].ToString().Trim() == "N")
                                                    row.Cells.Add(new WorksheetCell(dr["PROG_CASEMS"].ToString(), "CellFill"));
                                                else
                                                    row.Cells.Add(new WorksheetCell(dr["PROG_CASEMS"].ToString(), "Default")); break;
                                        }
                                    }
                                }
                                row.Cells.Add(new WorksheetCell(dr["ADD_OPERATOR"].ToString(), "Default"));
                                row.Cells.Add(new WorksheetCell(LookupDataAccess.Getdate(dr["INTAKE_DATE"].ToString()).ToString(), "Default"));
                            }
                            else
                            {
                                row.Cells.Add(new WorksheetCell(dr["CENTRAL_APP"].ToString(), "Default"));
                                if (strCodes.Length > 0)
                                {
                                    string[] values = strCodes.Split(',');
                                    for (int i = 0; i < values.Length; i++)
                                    {
                                        values[i] = values[i].Trim();

                                        switch (values[i])
                                        {
                                            case "1":
                                                row.Cells.Add(new WorksheetCell(dr["CENTRAL_INTAKE"].ToString(), "Default"));
                                                row.Cells.Add(new WorksheetCell(dr["CENTRAL_MEMBER"].ToString(), "Default")); break;
                                            case "2":
                                                if (dtSummary.Rows.Count > 12)
                                                {
                                                    if (dr["CENTRAL_SIM"].ToString().Trim() == "N")
                                                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_SIM"].ToString(), "CellFill"));
                                                    else
                                                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_SIM"].ToString(), "Default"));
                                                }
                                                else
                                                {
                                                    if (dr["PROG_MATRIX"].ToString().Trim() == "N")
                                                        row.Cells.Add(new WorksheetCell(dr["PROG_MATRIX"].ToString(), "CellFill"));
                                                    else
                                                        row.Cells.Add(new WorksheetCell(dr["PROG_MATRIX"].ToString(), "Default"));
                                                }
                                                break;
                                            case "3":
                                                if (dtSummary.Rows.Count > 12)
                                                {
                                                    if (dr["PROG_MATRIX"].ToString().Trim() == "N")
                                                        row.Cells.Add(new WorksheetCell(dr["PROG_MATRIX"].ToString(), "CellFill"));
                                                    else
                                                        row.Cells.Add(new WorksheetCell(dr["PROG_MATRIX"].ToString(), "Default"));
                                                }
                                                else
                                                {
                                                    if (dr["CENTRAL_MATRIX"].ToString().Trim() == "N")
                                                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_MATRIX"].ToString(), "CellFill"));
                                                    else
                                                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_MATRIX"].ToString(), "Default"));
                                                }
                                                break;
                                            case "4":
                                                if (dtSummary.Rows.Count > 12)
                                                {
                                                    if (dr["CENTRAL_MATRIX"].ToString().Trim() == "N")
                                                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_MATRIX"].ToString(), "CellFill"));
                                                    else
                                                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_MATRIX"].ToString(), "Default"));
                                                }
                                                else
                                                {
                                                    if (dr["PROG_VERIFICATION"].ToString().Trim() == "N")
                                                        row.Cells.Add(new WorksheetCell(dr["PROG_VERIFICATION"].ToString(), "CellFill"));
                                                    else
                                                        row.Cells.Add(new WorksheetCell(dr["PROG_VERIFICATION"].ToString(), "Default"));
                                                }
                                                //if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell(dr["CENTRAL_MATRIX"].ToString(), "Default"));
                                                //else row.Cells.Add(new WorksheetCell(dr["PROG_VERIFICATION"].ToString(), "Default")); 
                                                break;
                                            case "5":
                                                if (dtSummary.Rows.Count > 12)
                                                {
                                                    if (dr["PROG_VERIFICATION"].ToString().Trim() == "N")
                                                        row.Cells.Add(new WorksheetCell(dr["PROG_VERIFICATION"].ToString(), "CellFill"));
                                                    else
                                                        row.Cells.Add(new WorksheetCell(dr["PROG_VERIFICATION"].ToString(), "Default"));
                                                }
                                                else
                                                {
                                                    if (dr["CENTRAL_VERIFICATION"].ToString().Trim() == "N")
                                                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_VERIFICATION"].ToString(), "CellFill"));
                                                    else
                                                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_VERIFICATION"].ToString(), "Default"));
                                                }
                                                //if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell(dr["PROG_VERIFICATION"].ToString(), "Default"));
                                                //else row.Cells.Add(new WorksheetCell(dr["CENTRAL_VERIFICATION"].ToString(), "Default"));
                                                break;
                                            case "6":
                                                if (dtSummary.Rows.Count > 12)
                                                {
                                                    if (dr["CENTRAL_VERIFICATION"].ToString().Trim() == "N")
                                                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_VERIFICATION"].ToString(), "CellFill"));
                                                    else
                                                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_VERIFICATION"].ToString(), "Default"));
                                                }
                                                else
                                                {
                                                    if (dr["REQ_FLDS"].ToString().Trim() == "N")
                                                        row.Cells.Add(new WorksheetCell(dr["REQ_FLDS"].ToString(), "CellFill"));
                                                    else
                                                        row.Cells.Add(new WorksheetCell(dr["REQ_FLDS"].ToString(), "Default"));
                                                }
                                                //if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell(dr["CENTRAL_VERIFICATION"].ToString(), "Default"));
                                                //else row.Cells.Add(new WorksheetCell(dr["REQ_FLDS"].ToString(), "Default"));
                                                break;
                                            case "7":
                                                if (dtSummary.Rows.Count > 12)
                                                {
                                                    if (dr["REQ_FLDS"].ToString().Trim() == "N")
                                                        row.Cells.Add(new WorksheetCell(dr["REQ_FLDS"].ToString(), "CellFill"));
                                                    else
                                                        row.Cells.Add(new WorksheetCell(dr["REQ_FLDS"].ToString(), "Default"));
                                                }
                                                else
                                                {
                                                    if (dr["PROG_SPM"].ToString().Trim() == "N")
                                                        row.Cells.Add(new WorksheetCell(dr["PROG_SPM"].ToString(), "CellFill"));
                                                    else
                                                        row.Cells.Add(new WorksheetCell(dr["PROG_SPM"].ToString(), "Default"));
                                                }
                                                //if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell(dr["REQ_FLDS"].ToString(), "Default"));
                                                //else row.Cells.Add(new WorksheetCell(dr["PROG_SPM"].ToString(), "Default"));
                                                break;
                                            case "8":
                                                if (dtSummary.Rows.Count > 12)
                                                {
                                                    if (dr["PROG_SPM"].ToString().Trim() == "N")
                                                        row.Cells.Add(new WorksheetCell(dr["PROG_SPM"].ToString(), "CellFill"));
                                                    else
                                                        row.Cells.Add(new WorksheetCell(dr["PROG_SPM"].ToString(), "Default"));
                                                }
                                                else
                                                {
                                                    if (dr["CENTRAL_SPM"].ToString().Trim() == "N")
                                                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_SPM"].ToString(), "CellFill"));
                                                    else
                                                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_SPM"].ToString(), "Default"));
                                                }
                                                //if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell(dr["PROG_SPM"].ToString(), "Default"));
                                                //else row.Cells.Add(new WorksheetCell(dr["CENTRAL_SPM"].ToString(), "Default"));
                                                break;
                                            case "9":
                                                if (dtSummary.Rows.Count > 12)
                                                {
                                                    if (dr["CENTRAL_SPM"].ToString().Trim() == "N")
                                                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_SPM"].ToString(), "CellFill"));
                                                    else
                                                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_SPM"].ToString(), "Default"));
                                                }
                                                else
                                                {
                                                    if (dr["PROG_CASEACT"].ToString().Trim() == "N")
                                                        row.Cells.Add(new WorksheetCell(dr["PROG_CASEACT"].ToString(), "CellFill"));
                                                    else
                                                        row.Cells.Add(new WorksheetCell(dr["PROG_CASEACT"].ToString(), "Default"));
                                                }
                                                //if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell(dr["CENTRAL_SPM"].ToString(), "Default"));
                                                //else row.Cells.Add(new WorksheetCell(dr["PROG_CASEACT"].ToString(), "Default"));
                                                break;
                                            case "10":
                                                if (dtSummary.Rows.Count > 12)
                                                {
                                                    if (dr["PROG_CASEACT"].ToString().Trim() == "N")
                                                        row.Cells.Add(new WorksheetCell(dr["PROG_CASEACT"].ToString(), "CellFill"));
                                                    else
                                                        row.Cells.Add(new WorksheetCell(dr["PROG_CASEACT"].ToString(), "Default"));
                                                }
                                                else
                                                {
                                                    if (dr["CENTRAL_CASEACT"].ToString().Trim() == "N")
                                                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_CASEACT"].ToString(), "CellFill"));
                                                    else
                                                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_CASEACT"].ToString(), "Default"));
                                                }
                                                //if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell(dr["PROG_CASEACT"].ToString(), "Default"));
                                                //else row.Cells.Add(new WorksheetCell(dr["CENTRAL_CASEACT"].ToString(), "Default"));
                                                break;
                                            case "11":
                                                if (dtSummary.Rows.Count > 12)
                                                {
                                                    if (dr["CENTRAL_CASEACT"].ToString().Trim() == "N")
                                                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_CASEACT"].ToString(), "CellFill"));
                                                    else
                                                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_CASEACT"].ToString(), "Default"));
                                                }
                                                else
                                                {
                                                    if (dr["PROG_CASEMS"].ToString().Trim() == "N")
                                                        row.Cells.Add(new WorksheetCell(dr["PROG_CASEMS"].ToString(), "CellFill"));
                                                    else
                                                        row.Cells.Add(new WorksheetCell(dr["PROG_CASEMS"].ToString(), "Default"));
                                                }
                                                //if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell(dr["CENTRAL_CASEACT"].ToString(), "Default"));
                                                //else row.Cells.Add(new WorksheetCell(dr["PROG_CASEMS"].ToString(), "Default"));
                                                break;
                                            case "12":
                                                if (dtSummary.Rows.Count > 12)
                                                {
                                                    if (dr["PROG_CASEMS"].ToString().Trim() == "N")
                                                        row.Cells.Add(new WorksheetCell(dr["PROG_CASEMS"].ToString(), "CellFill"));
                                                    else
                                                        row.Cells.Add(new WorksheetCell(dr["PROG_CASEMS"].ToString(), "Default"));
                                                }
                                                else
                                                {
                                                    if (dr["CENTRAL_CASEMS"].ToString().Trim() == "N")
                                                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_CASEMS"].ToString(), "CellFill"));
                                                    else
                                                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_CASEMS"].ToString(), "Default"));
                                                }
                                                //if (dtSummary.Rows.Count > 12) row.Cells.Add(new WorksheetCell(dr["PROG_CASEMS"].ToString(), "Default"));
                                                //else row.Cells.Add(new WorksheetCell(dr["CENTRAL_CASEMS"].ToString(), "Default"));
                                                break;
                                            case "13":
                                                if (dtSummary.Rows.Count > 12)
                                                {
                                                    if (dr["CENTRAL_CASEMS"].ToString().Trim() == "N")
                                                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_CASEMS"].ToString(), "CellFill"));
                                                    else
                                                        row.Cells.Add(new WorksheetCell(dr["CENTRAL_CASEMS"].ToString(), "Default"));
                                                }
                                                //row.Cells.Add(new WorksheetCell(dr["CENTRAL_CASEMS"].ToString(), "Default"));
                                                break;
                                        }

                                    }
                                }


                                row.Cells.Add(new WorksheetCell(dr["ADD_OPERATOR"].ToString(), "Default"));
                                row.Cells.Add(new WorksheetCell(dr["CENTRAL_ADD_OPERATOR"].ToString(), "Default"));
                                row.Cells.Add(new WorksheetCell(LookupDataAccess.Getdate(dr["CENTRAL_INTAKE_DATE"].ToString()).ToString(), "Default"));
                                row.Cells.Add(new WorksheetCell(LookupDataAccess.Getdate(dr["INTAKE_DATE"].ToString()).ToString(), "Default"));

                                string Status_Desc = string.Empty;

                                if (AgyTabs_List.Count > 0)
                                {
                                    AGYTABSEntity SelEntity = AgyTabs_List.Find(u => u.Table_Code.Equals(dr["PROG_ENRL_STATUS"].ToString().Trim()));
                                    if (SelEntity != null)
                                        Status_Desc = SelEntity.Code_Desc.Trim();
                                }



                                row.Cells.Add(new WorksheetCell(Status_Desc, "Default"));


                            }
                        }

                    }

                    FileStream stream = new FileStream(PdfName, FileMode.Create);

                    book.Save(stream);
                    stream.Close();
                    AlertBox.Show("Generated Report Successfully");

                }
                catch (Exception ex) { }

            }
        }


        #endregion



    }
}