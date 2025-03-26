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
using XLSExportFile;
using DevExpress.XtraEditors;

#endregion
namespace Captain.Common.Views.Forms
{
    public partial class HSSB0124_Report : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;

        #endregion
        public HSSB0124_Report(BaseForm baseForm, PrivilegeEntity privileges)
        {
            InitializeComponent();

            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            BaseForm = baseForm; Privileges = privileges;
            Agency = BaseForm.BaseAgency; Depart = BaseForm.BaseDept;
            Program = BaseForm.BaseProg; Program_Year = BaseForm.BaseYear;
            Set_Report_Hierarchy(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear);
            propfundingSource = _model.lookupDataAccess.GetAgyTabRecordsByCodefilter(Consts.AgyTab.CASEMNGMTFUNDSRC, "H");
            propReportPath = _model.lookupDataAccess.GetReportPath();
            this.Text = /*Privileges.Program + " - " +*/ Privileges.PrivilegeName.Trim();
            txtAppNo.Validator = TextBoxValidation.IntegerValidator;
            txtDayCareCuttOff.Validator = TextBoxValidation.IntegerValidator;
            txtHDCutoff.Validator = TextBoxValidation.IntegerValidator;
            txtHeadstart.Validator = TextBoxValidation.IntegerValidator;
            ListcommonEnrollEntity = new List<CommonEntity>();


            // FillEnrollStatCombo();
            FillSequencecombo();
        }
        #region properties

        public BaseForm BaseForm { get; set; }
        public PrivilegeEntity Privileges { get; set; }
        public string propReportPath { get; set; }
        public string Agency { get; set; }
        public string Depart { get; set; }
        public string Program { get; set; }
        public List<MembersEntity> mem_List { get; set; }
        List<CommonEntity> commonfundingsource { get; set; }
        List<CommonEntity> commonCasetypes { get; set; }
        public List<CommonEntity> propfundingSource { get; set; }
        public List<CaseSiteEntity> propCaseAllSiteEntity { get; set; }
        public List<HierarchyEntity> hierarchyEntity { get; set; }
        public List<CaseMstEntity> propcasemstAlllist { get; set; }
        List<CaseMstEntity> PropCaseMST { get; set; }
        List<CaseSnpEntity> PropCaseSnp { get; set; }
        public List<CaseEnrlEntity> propCaseENrl { get; set; }
        List<CaseCondEntitty> propcaseconddet { get; set; }
        List<AddCustEntity> propADDCUST { get; set; }
        List<CaseVerEntity> propcaseVer { get; set; }
        List<ChldMstEntity> propchldmst { get; set; }

        public List<CommonEntity> ListcommonEnrollEntity { get; set; }

        #endregion

        private void FillSequencecombo()
        {
            cmbRepSeq.Items.Clear();

            List<Captain.Common.Utilities.ListItem> listItem = new List<Captain.Common.Utilities.ListItem>();
            listItem.Add(new Captain.Common.Utilities.ListItem("1  - Child Name", "1"));
            listItem.Add(new Captain.Common.Utilities.ListItem("2  - Child App. #", "2"));
            listItem.Add(new Captain.Common.Utilities.ListItem("3  - Site/Child Name", "3"));
            listItem.Add(new Captain.Common.Utilities.ListItem("4  - Selected App #", "4"));
           
            cmbRepSeq.Items.AddRange(listItem.ToArray());
            cmbRepSeq.SelectedIndex = 0;
        }

        private void FillEnrollStatCombo()
        {
            cmbEnrlStat.Items.Clear();
            List<Captain.Common.Utilities.ListItem> listItem = new List<Captain.Common.Utilities.ListItem>();
            listItem.Add(new Captain.Common.Utilities.ListItem("Both", "B"));
            listItem.Add(new Captain.Common.Utilities.ListItem("Enrolled", "E"));
            listItem.Add(new Captain.Common.Utilities.ListItem("Not Enrolled", "N"));

            cmbEnrlStat.Items.AddRange(listItem.ToArray());
            cmbEnrlStat.SelectedIndex = 0;
        }


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
                FillYearCombo(Agy, Dept, Prog, Year);
            else
                this.Txt_HieDesc.Size = new System.Drawing.Size(740, 25);
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
                    if (!(String.IsNullOrEmpty(DepYear.Trim())) && DepYear != null && DepYear != "    ")
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

            if (!string.IsNullOrEmpty(Program_Year.Trim()))
                this.Txt_HieDesc.Size = new System.Drawing.Size(660, 25);
            else
                this.Txt_HieDesc.Size = new System.Drawing.Size(740, 25);
        }

        private bool ValidateForm()
        {
            bool isValid = true;
            _errorProvider.SetError(dtpFrmDate, null);
            _errorProvider.SetError(btnFunds, null);
            _errorProvider.SetError(txtHeadstart, null);
            _errorProvider.SetError(Rb_Enroll_Status, null);
            _errorProvider.SetError(txtAppNo, null);

            if (chkbRollOvrTrans.Checked == true)
            {
                if (SelFundingList.Count == 0)
                {
                    _errorProvider.SetError(btnFunds, "Please Select at least One Fund");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(btnFunds, null);
                }
            }

            if (((Captain.Common.Utilities.ListItem)cmbRepSeq.SelectedItem).Value.ToString().Trim() != "4")
            {
                if (string.IsNullOrEmpty(txtHeadstart.Text.Trim()))
                {
                    _errorProvider.SetError(txtHeadstart, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblHeadstart.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                    _errorProvider.SetError(txtHeadstart, null);
            }
            else
                _errorProvider.SetError(txtHeadstart, null);

            if (Rb_Enroll_Status.Checked && ListcommonEnrollEntity.Count <= 0)
            {
                _errorProvider.SetError(Rb_Enroll_Status, string.Format("Please Select at least One Enroll Status".Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(Rb_Enroll_Status, null);

            if(rbEnrlDate.Checked)
            {
                if (string.IsNullOrWhiteSpace(dtpFrmDate.Text))
                {
                    _errorProvider.SetError(dtpFrmDate, string.Format("Enrollment Date is required".Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                    _errorProvider.SetError(dtpFrmDate, null);
            }

            if(cmbRepSeq.SelectedIndex == 3)
            {
                if (string.IsNullOrEmpty(txtAppNo.Text.Trim()))
                {
                    _errorProvider.SetError(txtAppNo, "Please enter Applicant Number".Replace(Consts.Common.Colon, string.Empty));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtAppNo, null);
                }
            }

            //if (dtpFromDate.Checked == false)
            //{
            //    _errorProvider.SetError(dtpFromDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblDateRange.Text.Replace(Consts.Common.Colon, string.Empty)));
            //    isValid = false;
            //}
            //else
            //{
            //    _errorProvider.SetError(dtpFromDate, null);
            //}

            //if (dtpToDate.Checked == false)
            //{
            //    _errorProvider.SetError(dtpToDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblDateRange.Text.Replace(Consts.Common.Colon, string.Empty)));
            //    isValid = false;
            //}
            //else
            //{
            //    _errorProvider.SetError(dtpToDate, null);
            //}
            //if (dtpToDate.Value < dtpFromDate.Value)
            //{
            //    isValid = false;
            //    MessageBox.Show("End Date May Prior To Start Date", "TMSB0002", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //}


            return (isValid);
        }


        private void btnGeneratePdf_Click(object sender, EventArgs e)
        {
            bool ISSelect = false;
            if (gvApp.Rows.Count > 0)
            {
                foreach (DataGridViewRow dr in gvApp.Rows)
                {
                    if (dr.Cells["Check"].Value != null && Convert.ToBoolean(dr.Cells["Check"].Value) == true)
                    {
                        ISSelect = true; break;
                    }
                }

                if (((Captain.Common.Utilities.ListItem)cmbRepSeq.SelectedItem).Value.ToString() == "4")
                {
                    if (ISSelect)
                    {
                        if (StatActive != "Y")
                            MessageBox.Show("You are rolling over an INACTIVE application do you want to continue", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question,onclose: On_SelectedAppActiveStat);
                        else
                        {
                            PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, false, propReportPath, "PDF");
                            pdfListForm.FormClosed += new FormClosedEventHandler(On_ProduceClosed);
                            pdfListForm.StartPosition = FormStartPosition.CenterScreen;
                            pdfListForm.ShowDialog();
                        }
                    }
                }
                else if (((Captain.Common.Utilities.ListItem)cmbRepSeq.SelectedItem).Value.ToString() != "4")
                {
                    if (ISSelect) //&& rbProduct.Checked == true
                    {
                        PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, false, propReportPath, "PDF");
                        pdfListForm.FormClosed += new FormClosedEventHandler(On_ProduceClosed);
                        pdfListForm.StartPosition = FormStartPosition.CenterScreen;
                        pdfListForm.ShowDialog();
                    }
                }
                //else if (ISSelect && rbRollOver.Checked == true)
                //{
                //}
                else
                    AlertBox.Show("No Children Selected", MessageBoxIcon.Warning);
            }
            else
            {
                if (((Captain.Common.Utilities.ListItem)cmbRepSeq.SelectedItem).Value.ToString().Trim() == "4")
                    AlertBox.Show("Please Select App#", MessageBoxIcon.Warning);
                else
                    AlertBox.Show("Please Click Generate Apps Button", MessageBoxIcon.Warning);
            }
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
            //_errorProvider.SetError(rdoMultipleSites, null);
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
            string RepSeq = ((Captain.Common.Utilities.ListItem)cmbRepSeq.SelectedItem).Value.ToString();
            string Status = string.Empty;
            if (Rb_Enroll_All.Checked == true) Status = "A";
            else if (Rb_Enroll_Status.Checked == true) Status = "S";

            string EnrlStat = string.Empty; //((Captain.Common.Utilities.ListItem)cmbEnrlStat.SelectedItem).Value.ToString();
            if(Status=="S")
                EnrlStat= Get_Sel_StatusCodes();

            //string strSite = rdoAllSite.Checked == true ? "A" : "M";
            //string strTaskAll = rbAllTasks.Checked == true ? "1" : "2";
            string strRepeaters = string.Empty;
            string strEnroll = string.Empty;
            string date = string.Empty;

            if (rbRepeaters.Checked == true)
                strRepeaters = "R";
            else if (rbNonRepeaters.Checked == true)
                strRepeaters = "N";
            else if (rbBoth.Checked == true)
                strRepeaters = "B";
            
            string strRoll = rbRollOver.Checked ? "R" : "P";
            string strchkRoll = "N";
            if (chkbRollOvrTrans.Checked == true) strchkRoll = "Y"; 
            
            string strFundingCodes = string.Empty;
            if (chkbRollOvrTrans.Checked == true)
            {
                foreach (CommonEntity FundingCode in SelFundingList)
                {
                    if (!strFundingCodes.Equals(string.Empty)) strFundingCodes += ",";
                    strFundingCodes += FundingCode.Code;
                }

                strEnroll = rbEnrlDate.Checked == true ? "E" : "K";
                if (rbEnrlDate.Checked == true)
                    date = dtpFrmDate.Value.Date.ToShortDateString();
            }

            string ExcldIntake = "N";
            if (chkbExcludIntake.Checked) ExcldIntake = "Y";



            StringBuilder str = new StringBuilder();
            str.Append("<Rows>");
            str.Append("<Row AGENCY = \"" + Agency + "\" DEPT = \"" + Depart + "\" PROG = \"" + Program +
                            "\" YEAR = \"" + Program_Year + "\" ReportSequence = \"" + RepSeq +
                            "\" HCut = \"" + txtHeadstart.Text + "\" HDCut = \"" + txtHDCutoff.Text + "\" DCut = \"" + txtDayCareCuttOff.Text + "\" CheckRoll = \"" + strchkRoll + "\" Status = \"" + Status + "\" EnrollStatus = \"" + EnrlStat + "\" IntakeExcld = \"" + ExcldIntake+
                            "\" Repeaters = \"" + strRepeaters + "\" RollProduce = \"" + strRoll + "\" FundingCode = \"" + strFundingCodes + "\" Enroll = \"" + strEnroll + "\"  EnrlDate = \"" + date + "\" />");
            str.Append("</Rows>");

            return str.ToString();
        }

        private void Set_Report_Controls(DataTable Tmp_Table)
        {
            if (Tmp_Table != null && Tmp_Table.Rows.Count > 0)
            {
                DataRow dr = Tmp_Table.Rows[0];

                Set_Report_Hierarchy(dr["AGENCY"].ToString(), dr["DEPT"].ToString(), dr["PROG"].ToString(), dr["YEAR"].ToString());

                //CommonFunctions.SetComboBoxValue(cmbEnrlStat, dr["EnrollStatus"].ToString());
                CommonFunctions.SetComboBoxValue(cmbRepSeq, dr["ReportSequence"].ToString());

                if (dr["Status"].ToString().Trim() == "A")
                    Rb_Enroll_All.Checked = true;
                else if (dr["Status"].ToString().Trim() == "S")
                {
                    Rb_Enroll_Status.Checked = true;
                    ListcommonEnrollEntity = new List<CommonEntity>();//ListcommonEnrollEntity.Clear();


                    Fill_EnrlStatus_Selected_List(dr["EnrollStatus"].ToString().Trim());
                }

                if (dr["IntakeExcld"].ToString().Trim() == "Y")
                    chkbExcludIntake.Checked = true;
                else chkbExcludIntake.Checked = false;

                if (dr["Repeaters"].ToString().Trim() == "R")
                    rbRepeaters.Checked = true;
                else if (dr["Repeaters"].ToString().Trim() == "N")
                    rbNonRepeaters.Checked = true;
                else if (dr["Repeaters"].ToString().Trim() == "B")
                    rbBoth.Checked = true;


                if (dr["RollProduce"].ToString().Trim() == "R")
                    rbRollOver.Checked = true;
                else if (dr["RollProduce"].ToString().Trim() == "P")
                    rbProduct.Checked = true;
                

                //if (dr["FundedDays"].ToString() == "Y")
                //    rdoAllFundYes.Checked = true;
                //else
                //    rdoAllFundNo.Checked = true;

                if (dr["CheckRoll"].ToString().Trim() == "Y")
                {
                    chkbRollOvrTrans.Checked = true;
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

                    if (dr["Enroll"].ToString().Trim() == "E")
                    {
                        rbEnrlDate.Checked = true;
                        dtpFrmDate.Value = Convert.ToDateTime(dr["EnrlDate"]);
                    }
                    else
                        rbKeepExisEnrlDate.Checked = true;

                    

                }


                txtDayCareCuttOff.Text = dr["DCut"].ToString();
                txtHDCutoff.Text = dr["HDCut"].ToString();
                txtHeadstart.Text = dr["HCut"].ToString();
                
                
                
            }
        }

        List<CommonEntity> Status_List = new List<CommonEntity>();
        private void Fill_EnrlStatus_Selected_List(string County_Str)
        {
            List<CommonEntity> Status_List = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "S0023", "**", "**", "**", string.Empty);

            foreach (CommonEntity Ent in Status_List)
            {
                if (County_Str.Contains(/*"'" +*/ Ent.Code /*+ "'"*/))
                    ListcommonEnrollEntity.Add(new CommonEntity(Ent));
            }
        }

        private void Pb_Search_Hie_Click(object sender, EventArgs e)
        {
            /*HierarchieSelectionFormNew hierarchieSelectionForm = new HierarchieSelectionFormNew(BaseForm, Current_Hierarchy_DB, "Master", "A", "A", "Reports");
            hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
            hierarchieSelectionForm.ShowDialog();*/

            HierarchieSelection hierarchieSelectionForm = new HierarchieSelection(BaseForm, Current_Hierarchy_DB, "Master", "A", "A", "Reports", BaseForm.UserID);
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
                    Agency = hierarchy.Substring(0, 2);
                    Depart = hierarchy.Substring(2, 2);
                    Program = hierarchy.Substring(4, 2);
                    
                }

            }
        }

        private void btnFunds_Click(object sender, EventArgs e)
        {
            if (chkbRollOvrTrans.Checked == true)
            {
                HSSB2111FundForm FundingForm = new HSSB2111FundForm(BaseForm, SelFundingList, Privileges);
                FundingForm.FormClosed += new FormClosedEventHandler(FundingForm_FormClosed);
                FundingForm.StartPosition = FormStartPosition.CenterScreen;
                FundingForm.ShowDialog();
            }
            else
                SelFundingList.Clear();
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

        private void chkbRollOvrTrans_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbRollOvrTrans.Checked == true)
            {
                btnFunds.Visible = true; pnlDte.Visible = true;
                //rbEnrlDate.Enabled = true; rbKeepExisEnrlDate.Enabled = true;
                dtpFrmDate.Enabled = false; rbKeepExisEnrlDate.Checked = true;
                
                HSSB2111FundForm FundingForm = new HSSB2111FundForm(BaseForm, SelFundingList, Privileges);
                FundingForm.FormClosed += new FormClosedEventHandler(FundingForm_FormClosed);
                FundingForm.StartPosition = FormStartPosition.CenterScreen;
                FundingForm.ShowDialog();

            }
            else
            {
                btnFunds.Visible = false; pnlDte.Visible = false ;
                //rbEnrlDate.Enabled = false; rbKeepExisEnrlDate.Enabled = false;
                //dtpFrmDate.Enabled = true; rbEnrlDate.Checked = false;
                SelFundingList = new List<CommonEntity>();
            }
        }

        private void rbKeepExisEnrlDate_CheckedChanged(object sender, EventArgs e)
        {
            if (rbEnrlDate.Checked == true)
            {
                dtpFrmDate.Enabled = true;
            }
            else dtpFrmDate.Enabled = false;
        }

        private void cmbRepSeq_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((Captain.Common.Utilities.ListItem)cmbRepSeq.SelectedItem).Value.ToString().Trim() == "4")
            {
                lblApp.Visible = true; label1.Visible = true; label2.Visible = false;
                txtAppNo.Visible = true; btnProcess.Visible = false;
            }
            else
            {
                lblApp.Visible = false; label1.Visible = false; label2.Visible = true;
                txtAppNo.Visible = false; btnProcess.Visible = true; txtAppNo.Text = string.Empty;
                _errorProvider.SetError(txtAppNo, null);
            }
        }

        List<HSSB2106Report_Entity> Report_list = new List<HSSB2106Report_Entity>();
        string strFundingCodes = string.Empty;string strStatusCodes = string.Empty;
        private void btnProcess_Click(object sender, EventArgs e)
        {
            fillgrid();
        }

        private string Get_Sel_StatusCodes()
        {
            string Sel_Enrl_Codes = null;
            foreach (CommonEntity Entity in ListcommonEnrollEntity)
            {
                Sel_Enrl_Codes += "" + Entity.Code + ",";
            }

            if (Sel_Enrl_Codes.Length > 0)
                Sel_Enrl_Codes = Sel_Enrl_Codes.Substring(0, (Sel_Enrl_Codes.Length - 1));

            return Sel_Enrl_Codes;
        }

        string StatActive = string.Empty;
        private void fillgrid()
        {
            if (ValidateForm())
            {

                if (Agency == "**") Agency = null; if (Depart == "**") Depart = null; if (Program == "**") Program = null;
                string Year = string.Empty; string AppNo = string.Empty; string BaseAge = string.Empty;
                if (CmbYear.Visible == true)
                    Year = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString().Trim();
                string Active_stat = string.Empty;
                if (((Captain.Common.Utilities.ListItem)cmbRepSeq.SelectedItem).Value.ToString().Trim() == "4")
                {
                    AppNo = txtAppNo.Text.Trim();
                    if (!string.IsNullOrEmpty(Year.Trim()))
                        Year = (int.Parse(Year) - 1).ToString();
                }
                if (rbRepeaters.Checked == true)
                    Active_stat = "R";
                else if (rbNonRepeaters.Checked == true)
                    Active_stat = "N";
                string Sbcb_date = string.Empty;

                StatActive = string.Empty;
                CaseEnrlEntity EnrlList = new CaseEnrlEntity(true);
                EnrlList.Agy = Agency;
                EnrlList.Dept = Depart;
                EnrlList.Prog = Program;
                EnrlList.Year = Year;
                if (((Captain.Common.Utilities.ListItem)cmbRepSeq.SelectedItem).Value.ToString().Trim() == "4")
                    EnrlList.App = txtAppNo.Text;
                EnrlList.Rec_Type = "H";
                EnrlList.Status = "E";
                propCaseENrl = _model.EnrollData.Browse_CASEENRL(EnrlList, "Browse");

                //if(propCaseENrl.Count>0)
                //{
                //    propCaseENrl= propCaseENrl.FindAll(u=>u.)
                //}

                strFundingCodes = string.Empty;
                if (chkbRollOvrTrans.Checked == true)
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

                strStatusCodes = string.Empty;
                if (Rb_Enroll_Status.Checked)
                {
                    strStatusCodes = Get_Sel_StatusCodes();
                }
                else strStatusCodes = "A";

                string ExcludeIntakes = "N";
                if (chkbExcludIntake.Checked) ExcludeIntakes = "Y";


                gvApp.Rows.Clear();

                int NextYear = int.Parse(Year) + 1;
                bool Istrue = false;
                propcasemstAlllist = _model.CaseMstData.GetCaseMstAll(Agency, Depart, Program, NextYear.ToString(), AppNo, string.Empty, string.Empty, string.Empty, string.Empty, "MSTALLSNP");
                if (propcasemstAlllist.Count > 0)
                    Istrue = true;


                //Report_list = _model.ChldTrckData.GetHSSB2124_Report(Agency, Depart, Program, Year, AppNo, "K", string.Empty, strFundingCodes, Active_stat, ((Captain.Common.Utilities.ListItem)cmbEnrlStat.SelectedItem).Value.ToString().Trim(), string.Empty, string.Empty, ((Captain.Common.Utilities.ListItem)cmbRepSeq.SelectedItem).Value.ToString().Trim());
                Report_list = _model.ChldTrckData.GetHSSB2124_Report(Agency, Depart, Program, Year, AppNo, "K", string.Empty, strFundingCodes, Active_stat, strStatusCodes, string.Empty, string.Empty, ((Captain.Common.Utilities.ListItem)cmbRepSeq.SelectedItem).Value.ToString().Trim(), ExcludeIntakes);

                if (Report_list.Count > 0)
                {
                    if (((Captain.Common.Utilities.ListItem)cmbRepSeq.SelectedItem).Value.ToString().Trim() == "1")
                        Report_list = Report_list.OrderBy(u => u.Lname.ToString().Trim()).ThenBy(u => u.Fname.Trim()).ThenBy(u => u.Mname.Trim()).ToList();
                    else if (((Captain.Common.Utilities.ListItem)cmbRepSeq.SelectedItem).Value.ToString().Trim() == "2")
                        Report_list = Report_list.OrderBy(u => u.AppNo.ToString().Trim()).ThenBy(u => u.Enrl_Site.Trim()).ThenBy(u => u.Enrl_Room.Trim()).ThenBy(u => u.Enrl_AMPM.Trim()).ToList();
                    else if (((Captain.Common.Utilities.ListItem)cmbRepSeq.SelectedItem).Value.ToString().Trim() == "3")
                        Report_list = Report_list.OrderBy(u => u.Site.ToString().Trim()).ThenBy(u => u.Lname.Trim()).ThenBy(u => u.Fname.Trim()).ToList();
                    else
                        Report_list = Report_list.OrderBy(u => u.Site.ToString().Trim()).ThenBy(u => u.Lname.Trim()).ThenBy(u => u.Fname.Trim()).ToList();

                    int rowIndex = 0; bool IsApp = true;
                    string Funding_HS = "N"; string Funding_DC = "N"; string Funding_Both = "N";
                    bool IsFund = true;
                    foreach (HSSB2106Report_Entity Entity in Report_list)
                    {
                        Funding_HS = "N"; Funding_DC = "N"; Funding_Both = "N";
                        string Status = "Unenrolled"; IsFund = true;
                        if (!string.IsNullOrEmpty(Entity.Enrl_Status.Trim()))
                        {
                            if (chkbRollOvrTrans.Checked)
                            {
                                if (!string.IsNullOrEmpty(Entity.Funds.Trim()))
                                {
                                    if (Entity.Funds.Contains(','))
                                    {
                                        String[] SFunds = Entity.Funds.Split(',');
                                        if (SFunds.Length > 0)
                                        {
                                            for (int i = 0; i < SFunds.Length; i++)
                                            {
                                                if (strFundingCodes.Contains(SFunds[i]))
                                                {
                                                    IsFund = true; break;
                                                }
                                                else IsFund = false;
                                            }
                                        }
                                    }
                                }
                            }

                            switch(Entity.Enrl_Status.Trim())
                            {
                                case "A": Status = "Parent declined";break;
                                case "B": Status = "No Longer Interested"; break;
                                case "C": Status = "Accepted"; break;
                                case "E": Status = "Enrolled"; break;
                                case "F": Status = "Deferred"; break;
                                case "I": Status = "Post Intake"; break;
                                case "L": Status = "Waitlist"; break;
                                case "N": Status = "Inactive"; break;
                                case "P": Status = "Pending"; break;
                                case "R": Status = "Denied"; break;
                                case "T": Status = "Transferred"; break;
                                case "W": Status = "Withdrawn"; break;
                                case "": Status = "Unenrolled"; break;
                            }

                            //if (Entity.Enrl_Status.Trim() == "E") Status = "Enrolled";else if (Entity.Enrl_Status.Trim() == "L") Status = "Wait List";
                            
                            if (propCaseENrl.Count > 0)
                            {
                                List<CaseEnrlEntity> SelEnrlApps = propCaseENrl.FindAll(u => u.Agy.Equals(Entity.Agy) && u.Dept.Equals(Entity.Dept) && u.Prog.Equals(Entity.Prog) && u.Year.Equals(Entity.Year) && u.App.Equals(Entity.AppNo));
                                if (SelEnrlApps.Count > 0)
                                {
                                    SelEnrlApps = SelEnrlApps.OrderBy(u => u.FundHie.Trim()).ToList();

                                    foreach (CaseEnrlEntity Drent in SelEnrlApps)
                                    {
                                        if (Drent.FundHie.Trim() == "HS") Funding_HS = "Y";
                                        if (Drent.FundHie.Trim() != "HS") Funding_DC = "Y";
                                        if (Funding_DC == "Y" && Funding_HS == "Y") Funding_Both = "Y";
                                    }
                                }
                            }
                        }

                        if (((Captain.Common.Utilities.ListItem)cmbRepSeq.SelectedItem).Value.ToString().Trim() != "4" && IsFund)
                        {
                            if (!string.IsNullOrEmpty(Entity.Age.Trim()))
                            {
                                if (Funding_Both == "Y")
                                {
                                    if (!string.IsNullOrEmpty(txtHDCutoff.Text.Trim()))
                                    {
                                        if (int.Parse(Entity.Age.Trim()) >= int.Parse(txtHDCutoff.Text.Trim())) IsApp = false;
                                        else IsApp = true;
                                    }
                                    else IsApp = false;
                                }
                                else if (Funding_HS == "Y")
                                {
                                    if (!string.IsNullOrEmpty(txtHeadstart.Text.Trim()))
                                    {
                                        if (int.Parse(Entity.Age.Trim()) >= int.Parse(txtHeadstart.Text.Trim())) IsApp = false;
                                        else IsApp = true;
                                    }
                                    else IsApp = false;
                                }
                                else if (Funding_DC == "Y")
                                {
                                    if (!string.IsNullOrEmpty(txtDayCareCuttOff.Text.Trim()))
                                    {
                                        if (int.Parse(Entity.Age.Trim()) >= int.Parse(txtDayCareCuttOff.Text.Trim())) IsApp = false;
                                        else IsApp = true;
                                    }
                                    else IsApp = false;
                                }
                                else if (!string.IsNullOrEmpty(txtHeadstart.Text.Trim()))
                                {
                                    //if (!string.IsNullOrEmpty(Entity.Age.Trim()))
                                    //{
                                    if (int.Parse(Entity.Age.Trim()) >= int.Parse(txtHeadstart.Text.Trim())) IsApp = false;
                                    else IsApp = true;
                                    //}
                                }
                            }
                            else IsApp = true;
                        }
                        //string DOB = string.Empty;
                        //if(!string.IsNullOrEmpty(Entity.DOB.Trim()))
                        //    DOB=LookupDataAccess.Getdate(Entity.DOB.Trim());
                        if (IsApp && IsFund)
                        {
                            rowIndex = gvApp.Rows.Add(false, Entity.AppNo.Trim(), Entity.Lname.Trim() + " " + Entity.Fname.Trim() + "  " + Entity.Mname.Trim(), LookupDataAccess.Getdate(Entity.DOB.Trim()), Entity.Age, Entity.Site.Trim(), Entity.Funds.Trim(), Status, Entity.Fname.Trim(), Entity.Lname.Trim(), Entity.Mname.Trim(), "N",Entity.Enrl_Status);

                            if (((Captain.Common.Utilities.ListItem)cmbRepSeq.SelectedItem).Value.ToString() == "4")
                                StatActive = Entity.ActiveStatus.Trim();
                            
                            if (Istrue)
                            {
                                CaseMstEntity SelApp = propcasemstAlllist.Find(u => u.ApplAgency.Trim().Equals(Entity.Agy) && u.ApplDept.Equals(Entity.Dept) && u.ApplProgram.Equals(Entity.Prog) && u.ApplNo.Equals(Entity.AppNo.Trim()));
                                if (SelApp != null)
                                {
                                    gvApp.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                                    gvApp.Rows[rowIndex].Cells["Check"].ReadOnly = true;
                                    //gvApp.Rows[rowIndex].Cells["Selected"].Value = "Y";
                                }
                            }
                        }

                    }
                    gvApp.Rows[0].Tag = 0;
                }
                if (gvApp.Rows.Count > 0)
                {
                    gvApp.Rows[0].Selected = true;
                    btnSelectAll.Visible = btnUnSelect.Visible = true;
                }
            }
        }

        private void gvApp_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (gvApp.Rows.Count > 0)
            //{
            //    if (e.ColumnIndex == 0)
            //    {
            //        //if (gvwApplicants.CurrentRow.Cells["Selected"].Value.ToString() == "Y")
            //        //if (dr.Cells["dgvChk"].Value != null && Convert.ToBoolean(dr.Cells["dgvChk"].Value) == true)
            //        if (gvApp.CurrentRow.Cells["Selected"].Value.ToString() == "Y")
            //            //gvApp.CurrentRow.Cells["Check"].Value = false;
            //            gvApp.CurrentRow.Cells["Check"].ReadOnly = true;
            //        else
            //        {

            //        }
            //    }
            //}
        }

        string WS_Next_Year = string.Empty;
        private void Gettable()
        {
            if (Agency == "**") Agency = null; if (Depart == "**") Depart = null; if (Program == "**") Program = null;
            string Year = string.Empty; string AppNo = string.Empty; string BaseAge = string.Empty;
            if (CmbYear.Visible == true)
                Year = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString().Trim();

            if (rbRollOver.Checked == true)
            {
                WS_Next_Year = int.Parse(Year + 1).ToString();
                PropCaseMST = _model.CaseMstData.GetCaseMstAll(Agency, Depart, Program, Program_Year, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, "MSTALLSNP");
                                    
                PropCaseSnp = _model.CaseMstData.GetCaseSnpadpyn(Agency, Depart, Program, Year, string.Empty);
                CaseEnrlEntity EnrlList = new CaseEnrlEntity(true);
                EnrlList.Agy = Agency;
                EnrlList.Dept = Depart;
                EnrlList.Prog = Program;
                EnrlList.Year = Year;
                EnrlList.Rec_Type = "H";
                EnrlList.Status = "E";
                //public List<CaseEnrlEntity> propCaseENrl { get; set; }
                propCaseENrl = _model.EnrollData.Browse_CASEENRL(EnrlList, "Browse");
                propcaseconddet = _model.ChldMstData.GetCaseCondDetailsList(Agency, Depart, Program, Year, string.Empty, string.Empty);
                CaseDiffEntity caseDiffDetails = _model.CaseMstData.GetCaseDiffadpya(Agency, Depart, Program, Year, string.Empty, string.Empty);
                //List<AddCustEntity> propADDCUST = new List<AddCustEntity>();
                AddCustEntity Search_AddCust = new AddCustEntity(true);
                Search_AddCust.ACTAGENCY = BaseForm.BaseAgency; Search_AddCust.ACTDEPT = BaseForm.BaseDept; Search_AddCust.ACTPROGRAM = BaseForm.BaseProg;
                Search_AddCust.ACTYEAR = BaseForm.BaseYear; //Search_AddCust.ACTAPPNO = BaseForm.BaseApplicationNo;
                propADDCUST = _model.CaseMstData.Browse_ADDCUST(Search_AddCust, "Browse");
                propcaseVer = _model.CaseMstData.GetCASEVeradpyalst(Agency, Depart, Program, Year, string.Empty, string.Empty, string.Empty);
                propchldmst = _model.ChldMstData.GetChldMstDetailsList(Agency, Depart, Program, Year, string.Empty, string.Empty);
            }
        }

        PdfContentByte cb;
        string strFolderPath = string.Empty;
        string Random_Filename = null;
        string PdfName = "Pdf File";
        BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
        int Y_Pos;
        BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
        private void On_ProduceClosed(object sender, FormClosedEventArgs e)
        {
            PdfListForm form = sender as PdfListForm;
            if (form.DialogResult == DialogResult.OK)
            {
                StringBuilder strMstApplUpdate = new StringBuilder();
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
                    ex.ToString();
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

                Document document = new Document(PageSize.A4, 30f, 30f, 30f, 50f);
                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                document.Open();
                //BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
                iTextSharp.text.Font helvetica = new iTextSharp.text.Font(bf_times, 12, 1);
                BaseFont bf_helv = helvetica.GetCalculatedBaseFont(false);
                iTextSharp.text.Font TimesUnderline = new iTextSharp.text.Font(bf_times, 9, 4);
                BaseFont bf_TimesUnderline = TimesUnderline.GetCalculatedBaseFont(true);

                iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 10);
                iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 8);
                iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 9, 3);
                iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_times, 9, 1);
                iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 8, 2);
                iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 9, 4);
                cb = writer.DirectContent;

                if (Agency == "**") Agency = null; if (Depart == "**") Depart = null; if (Program == "**") Program = null;
                string Year = string.Empty;
                if (CmbYear.Visible == true)
                    Year = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString().Trim();

                if (((Captain.Common.Utilities.ListItem)cmbRepSeq.SelectedItem).Value.ToString() == "4")
                {
                    if (!string.IsNullOrEmpty(Year.Trim()))
                        Year = (int.Parse(Year) - 1).ToString();
                }

                strFundingCodes = string.Empty;
                if (chkbRollOvrTrans.Checked == true)
                {
                    foreach (CommonEntity FundingCode in SelFundingList)
                    {
                        if (!strFundingCodes.Equals(string.Empty)) strFundingCodes += ",";
                        strFundingCodes += FundingCode.Code;
                    }
                }
               
                try
                {
                    PrintHeaderPage(document, writer);
                    document.NewPage();

                    if (gvApp.Rows.Count > 0)
                    {
                        string HeadStrt = string.Empty;
                        DataSet dsFund = DatabaseLayer.Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.CASEMNGMTFUNDSRC, "H");
                        DataTable dtFund = dsFund.Tables[0];
                        if (dtFund.Rows.Count > 0)
                        {
                            DataView dv = new DataView(dtFund);
                            dv.RowFilter = "EXT='Y'";
                            dtFund = dv.ToTable();

                            if (dtFund.Rows.Count > 0)
                            {
                                foreach (DataRow dr in dtFund.Rows)
                                {
                                    HeadStrt += dr["Code"].ToString().Trim() + " ";
                                }
                            }
                        }

                        string KeepOld = "N";
                        string RollOverEnrl_Sw = "N"; string Enrldate_sw = "N"; //string Enrlstat = "B";
                        if (chkbRollOvrTrans.Checked) RollOverEnrl_Sw = "Y";
                        if (rbEnrlDate.Checked) Enrldate_sw = "Y";
                        if (chkbKeep.Checked) KeepOld = "Y";
                        //if (((Captain.Common.Utilities.ListItem)cmbEnrlStat.SelectedItem).Value.ToString() == "E") Enrlstat = "E";
                        //else if (((Captain.Common.Utilities.ListItem)cmbEnrlStat.SelectedItem).Value.ToString() == "N") Enrlstat = "N";
                        //else Enrlstat = "B";

                        

                        PdfPTable table = new PdfPTable(10);
                        table.TotalWidth = 550f;
                        table.WidthPercentage = 100;
                        table.LockedWidth = true;
                        float[] widths = new float[] { 18f, 45f, 10f,16f, 18f,20f,28f,30f, 25f,30f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                        table.SetWidths(widths);
                        table.HorizontalAlignment = Element.ALIGN_LEFT;

                        string[] HeaderSeq4 = { "APP #", "Child's Name", "REP", "Site", "DOB", "Cert", "Inc/#InHouse", "", "Child's Age" ,""};
                        for (int i = 0; i < HeaderSeq4.Length; ++i)
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(HeaderSeq4[i], TblFontBold));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            //cell.FixedHeight = 15f;
                            cell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                            //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                            table.AddCell(cell);
                        }

                        int Appcnt = 0;
                        foreach (DataGridViewRow dr in gvApp.Rows)
                        {
                            if (dr.Cells["Check"].Value != null && Convert.ToBoolean(dr.Cells["Check"].Value) == true)
                            {
                                HSSB2106Report_Entity Entity = Report_list.Find(u => u.AppNo.Equals(dr.Cells["APPNO"].Value.ToString().Trim()));
                                if (Entity != null)
                                {
                                    PdfPCell App = new PdfPCell(new Phrase(Entity.AppNo.Trim(), TableFont));
                                    App.HorizontalAlignment = Element.ALIGN_LEFT;
                                    App.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(App);

                                    PdfPCell App_Name = new PdfPCell(new Phrase(dr.Cells["Name"].Value.ToString().Trim(), TableFont));
                                    App_Name.HorizontalAlignment = Element.ALIGN_LEFT;
                                    App_Name.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(App_Name);

                                    if (Entity.Repeater == "True")
                                    {
                                        PdfPCell Rpe = new PdfPCell(new Phrase("Y", TableFont));
                                        Rpe.HorizontalAlignment = Element.ALIGN_LEFT;
                                        Rpe.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(Rpe);
                                    }
                                    else
                                    {
                                        PdfPCell Rpe = new PdfPCell(new Phrase("", TableFont));
                                        Rpe.HorizontalAlignment = Element.ALIGN_LEFT;
                                        Rpe.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(Rpe);
                                    }

                                    PdfPCell AppSite = new PdfPCell(new Phrase(dr.Cells["Site"].Value.ToString().Trim(), TableFont));
                                    AppSite.HorizontalAlignment = Element.ALIGN_LEFT;
                                    AppSite.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(AppSite);

                                    PdfPCell Dob = new PdfPCell(new Phrase(LookupDataAccess.Getdate(dr.Cells["gvDOB"].Value.ToString().Trim()), TableFont));
                                    Dob.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Dob.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(Dob);

                                    string Certify = string.Empty;
                                    if (string.IsNullOrEmpty(Entity.Elig_Date.Trim())) Entity.Classfication = "00";
                                    if (Entity.Classfication == "00") Certify = "In Cert";
                                    else if (Entity.Classfication == "98") Certify = "Denied";
                                    else if (Entity.Classfication == "99") Certify = "Certified";

                                    PdfPCell cert = new PdfPCell(new Phrase(Certify, TableFont));
                                    cert.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cert.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(cert);

                                    PdfPCell Inc = new PdfPCell(new Phrase(Entity.FamIncome.Trim()+"   "+Entity.NoInHH.Trim(), TableFont));
                                    Inc.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    Inc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(Inc);

                                    PdfPCell ChFund = new PdfPCell(new Phrase("Expected Fund: " + Entity.ChldMstFund.Trim(), TableFont));
                                    ChFund.HorizontalAlignment = Element.ALIGN_LEFT;
                                    ChFund.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(ChFund);

                                    PdfPCell Ages = new PdfPCell(new Phrase("Yr: "+Entity.Age.Trim()+" Mo: "+Entity.AMonths.Trim(), TableFont));
                                    Ages.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    Ages.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(Ages);

                                    //if (chkbRollOvrTrans.Checked == false)
                                    //{
                                    //     if (Entity.ChldMstFund.Trim() == "1") //if (HeadStrt.Trim().Contains(Entity.ChldMstFund.Trim()))
                                    //        Entity.WaitList = "Y";
                                    //    else
                                    //    {
                                    //        if (string.IsNullOrEmpty(Entity.Enrl_Status.Trim()))
                                    //            Entity.WaitList = "Y";
                                    //        else Entity.WaitList = "N";
                                    //    }
                                    //}

                                    //if (Entity.WaitList == "Y")
                                    //{
                                    //    PdfPCell Wait = new PdfPCell(new Phrase("On Waiting List", TableFont));
                                    //    Wait.HorizontalAlignment = Element.ALIGN_LEFT;
                                    //    Wait.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //    table.AddCell(Wait);
                                    //}
                                    //else
                                    //{
                                        PdfPCell Wait = new PdfPCell(new Phrase("", TableFont));
                                        Wait.HorizontalAlignment = Element.ALIGN_LEFT;
                                        Wait.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(Wait);
                                    //}

                                    PdfPCell Space = new PdfPCell(new Phrase("", TableFont));
                                    Space.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Space.Colspan = 5;
                                    Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(Space);

                                    

                                    if (Entity.PreClient == "Y")
                                    {
                                        PdfPCell Cleint = new PdfPCell(new Phrase("PRE - ENROLLED", TableFont));
                                        Cleint.HorizontalAlignment = Element.ALIGN_LEFT;
                                        Cleint.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(Cleint);
                                    }
                                    else
                                    {
                                        PdfPCell EStatus = new PdfPCell(new Phrase(dr.Cells["EnrlStatus"].Value.ToString().Trim(), TableFont));
                                        EStatus.HorizontalAlignment = Element.ALIGN_LEFT;
                                        EStatus.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(EStatus);

                                        //PdfPCell Cleint = new PdfPCell(new Phrase("", TableFont));
                                        //Cleint.HorizontalAlignment = Element.ALIGN_LEFT;
                                        //Cleint.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        //table.AddCell(Cleint);
                                    }

                                    List<CaseEnrlEntity> SelEnrlApps = new List<CaseEnrlEntity>();
                                    if (Entity.Enrl_Status == "E")
                                    {
                                        if (propCaseENrl.Count > 0)
                                            SelEnrlApps = propCaseENrl.FindAll(u => u.Agy.Equals(Entity.Agy) && u.Dept.Equals(Entity.Dept) && u.Prog.Equals(Entity.Prog) && u.Year.Equals(Entity.Year) && u.App.Equals(Entity.AppNo));
                                    }

                                    string Message = string.Empty;
                                    if (string.IsNullOrEmpty(Entity.Age.Trim()))
                                    {
                                        if (string.IsNullOrEmpty(Entity.DOB.Trim())) Message = "*** No Age Calc. (Missing DOB) ***";
                                        else if (string.IsNullOrEmpty(Entity.Zip.Trim())) Message = "*** No Age Calc. (Missing Zip Code) ***";
                                    }


                                    if (Entity.Enrl_Status.Trim() != "E")
                                    {
                                        PdfPCell Enrlstat = new PdfPCell(new Phrase("", TableFont));
                                        Enrlstat.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        Enrlstat.Colspan = 2;
                                        Enrlstat.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(Enrlstat);

                                        PdfPCell mesg = new PdfPCell(new Phrase(Message, TableFont));
                                        mesg.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        mesg.Colspan = 2;
                                        mesg.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(mesg);
                                    }
                                    else
                                    {
                                        if (SelEnrlApps.Count > 0)
                                        {
                                            int i = 0;
                                            foreach (CaseEnrlEntity drenrl in SelEnrlApps)
                                            {
                                                if (i > 0)
                                                {
                                                    PdfPCell Spaceenrl = new PdfPCell(new Phrase("", TableFont));
                                                    Spaceenrl.HorizontalAlignment = Element.ALIGN_LEFT;
                                                    Spaceenrl.Colspan = 5;
                                                    Spaceenrl.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                    table.AddCell(Spaceenrl);

                                                    PdfPCell EStatus = new PdfPCell(new Phrase(dr.Cells["EnrlStatus"].Value.ToString().Trim(), TableFont));
                                                    EStatus.HorizontalAlignment = Element.ALIGN_LEFT;
                                                    EStatus.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                    table.AddCell(EStatus);

                                                }

                                                PdfPCell Enrlstat = new PdfPCell(new Phrase(drenrl.Site.Trim() + " " + drenrl.Room.Trim() + " " + drenrl.AMPM.Trim() + "  " + LookupDataAccess.Getdate(drenrl.Enrl_Date.Trim()) + "  " + drenrl.FundHie.Trim(), TableFont));
                                                Enrlstat.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                Enrlstat.Colspan = 2;
                                                Enrlstat.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                table.AddCell(Enrlstat);

                                                if (i == 0)
                                                {
                                                    PdfPCell mesg = new PdfPCell(new Phrase(Message, TableFont));
                                                    mesg.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                    mesg.Colspan = 2;
                                                    mesg.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                    table.AddCell(mesg);
                                                }
                                                else
                                                {
                                                    PdfPCell mesg = new PdfPCell(new Phrase("", TableFont));
                                                    mesg.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                    mesg.Colspan = 2;
                                                    mesg.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                    table.AddCell(mesg);
                                                }

                                                i++;
                                            }
                                        }
                                    }



                                    //if (!string.IsNullOrEmpty(Entity.Enrl_Status.Trim()))
                                    //{
                                    //    PdfPCell Enrlstat = new PdfPCell(new Phrase(Entity.Enrl_Site.Trim() + " " + Entity.Enrl_Room.Trim() + " " + Entity.Enrl_AMPM.Trim() + "  " + LookupDataAccess.Getdate(Entity.Enrl_Date.Trim()) + "  " + Entity.Enrl_Fund.Trim(), TableFont));
                                    //    Enrlstat.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    //    Enrlstat.Colspan = 2;
                                    //    Enrlstat.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //    table.AddCell(Enrlstat);
                                    //}
                                    //else
                                    //{
                                    //    PdfPCell Enrlstat = new PdfPCell(new Phrase("", TableFont));
                                    //    Enrlstat.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    //    Enrlstat.Colspan = 2;
                                    //    Enrlstat.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //    table.AddCell(Enrlstat);
                                    //}

                                    //if (string.IsNullOrEmpty(Entity.Age.Trim()))
                                    //{
                                    //    string Message = string.Empty;
                                    //    if (string.IsNullOrEmpty(Entity.DOB.Trim())) Message = "*** No Age Calc. (Missing DOB) ***";
                                    //    else if (string.IsNullOrEmpty(Entity.Zip.Trim())) Message = "*** No Age Calc. (Missing Zip Code) ***";

                                    //    PdfPCell mesg = new PdfPCell(new Phrase(Message, TableFont));
                                    //    mesg.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    //    mesg.Colspan = 2;
                                    //    mesg.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //    table.AddCell(mesg);
                                    //}
                                    //else
                                    //{
                                    //    PdfPCell Space1 = new PdfPCell(new Phrase("", TableFont));
                                    //    Space1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    //    Space1.Colspan = 2;
                                    //    Space1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //    table.AddCell(Space1);
                                    //}

                                    if (rbRollOver.Checked == true)
                                    {
                                        string Rept = string.Empty;

                                        if (chkbRollOvrTrans.Checked == false)
                                        {
                                             if (Entity.ChldMstFund.Trim() == "HS") //if (HeadStrt.Trim().Contains(Entity.ChldMstFund.Trim()))
                                                Entity.WaitList = "Y";
                                            else
                                            {
                                                if (string.IsNullOrEmpty(Entity.Enrl_Status.Trim()))
                                                    Entity.WaitList = "Y";
                                                else Entity.WaitList = "N";
                                            }
                                        }

                                        if (Entity.Repeater.Trim() == "True") Rept = "1"; else Rept = "0";
                                        if (Entity.Enrl_Status == "E") Rept = "1"; //else Entity.Repeater = "0";

                                        string Status = string.Empty;
                                        if (Rb_Enroll_All.Checked) Status = "*";
                                        else if(Rb_Enroll_Status.Checked)
                                        {
                                            Status = Entity.Enrl_Status;
                                        }

                                        Captain.DatabaseLayer.CaseMst.updateHSSB0124(Entity.Agy, Entity.Dept, Entity.Prog, Entity.Year, Entity.AppNo, Entity.WaitList, Rept, BaseForm.UserID, RollOverEnrl_Sw, Enrldate_sw, dtpFrmDate.Value.ToShortDateString(), Status, ((Captain.Common.Utilities.ListItem)cmbRepSeq.SelectedItem).Value.ToString(),strFundingCodes,KeepOld);
                                    }

                                    Appcnt++;
                                }

                            }
                        }
                        if (table.Rows.Count > 0)
                        {
                            PdfPCell cnt = new PdfPCell(new Phrase("Total Applicants:           " + Appcnt.ToString(), TblFontBold));
                            cnt.HorizontalAlignment = Element.ALIGN_LEFT;
                            cnt.Colspan = 10;
                            cnt.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            table.AddCell(cnt);

                            document.Add(table);
                            document.NewPage();

                            CaseSiteEntity Search_Entity = new CaseSiteEntity(true);
                            Search_Entity.SiteAGENCY = Agency; Search_Entity.SiteDEPT = Depart; Search_Entity.SitePROG = Program;
                            Search_Entity.SiteYEAR = Year;
                            //Search_Entity.SiteROOM = "0000";

                            propCaseAllSiteEntity = _model.CaseMstData.Browse_CASESITE(Search_Entity, "Browse");
                            if (propCaseAllSiteEntity.Count > 0)
                            {
                                PdfPTable Sitetable = new PdfPTable(2);
                                Sitetable.TotalWidth = 300f;
                                Sitetable.WidthPercentage = 100;
                                Sitetable.LockedWidth = true;
                                float[] Sitetablewidths = new float[] { 25f, 45f};// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                                Sitetable.SetWidths(Sitetablewidths);
                                Sitetable.HorizontalAlignment = Element.ALIGN_LEFT;

                                string[] HeaderSeq = { "Site/Room", "Site Name" };
                                for (int i = 0; i < HeaderSeq.Length; ++i)
                                {
                                    PdfPCell cell = new PdfPCell(new Phrase(HeaderSeq[i], TblFontBold));
                                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    //cell.FixedHeight = 15f;
                                    cell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                    //cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                                    Sitetable.AddCell(cell);
                                }

                                int TotSites = 0;
                                foreach (CaseSiteEntity SiteR in propCaseAllSiteEntity)
                                {
                                    PdfPCell S1 = new PdfPCell(new Phrase(SiteR.SiteNUMBER.Trim()+"  "+SiteR.SiteROOM.Trim()+"  "+SiteR.SiteAM_PM.Trim(), TableFont));
                                    S1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    //S1.Colspan = 10;
                                    S1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    Sitetable.AddCell(S1);

                                    PdfPCell S2 = new PdfPCell(new Phrase(SiteR.SiteNAME.Trim() , TableFont));
                                    S2.HorizontalAlignment = Element.ALIGN_LEFT;
                                    //S1.Colspan = 10;
                                    S2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    Sitetable.AddCell(S2);

                                    TotSites++;
                                }
                                PdfPCell S3 = new PdfPCell(new Phrase("Total Sites:     " + TotSites.ToString(), TblFontBold));
                                S3.HorizontalAlignment = Element.ALIGN_LEFT;
                                S3.Colspan = 2;
                                S3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                Sitetable.AddCell(S3);

                                document.Add(Sitetable);

                            }

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


        private void On_SelectedAppActiveStat(DialogResult dialogResult)
        {
           // Gizmox.WebGUI.Forms.Form senderform=(Gizmox.WebGUI.Forms.Form)sender;

            //if (senderform != null)
            //{
                if (DialogResult.Yes == dialogResult)
                {
                    PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, false, propReportPath);
                    pdfListForm.FormClosed += new FormClosedEventHandler(On_ProduceClosed);
                    pdfListForm.StartPosition = FormStartPosition.CenterScreen;
                    pdfListForm.ShowDialog();
                }
            //}
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

           /* PdfPCell row2 = new PdfPCell(new Phrase("Run By : " + LookupDataAccess.GetMemberName(BaseForm.UserProfile.FirstName.Trim(), BaseForm.UserProfile.MI.Trim(), BaseForm.UserProfile.LastName.Trim(), "3"), TableFont));
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


            //PdfPCell Hierarchy = new PdfPCell(new Phrase(Agy + "  " + Dept + "  " + Prg + "  " + Header_year, TableFont));Txt_HieDesc
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

            PdfPCell R1 = new PdfPCell(new Phrase("  " + lblRepSeq.Text.Trim() /*+ " :" + ((Captain.Common.Utilities.ListItem)cmbRepSeq.SelectedItem).Text.ToString()*/, TableFont)); //+ (rdoSiteName.Checked == true ? rdoSiteName.Text : rdoRank.Text)
            R1.HorizontalAlignment = Element.ALIGN_LEFT;
            R1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R1.PaddingBottom = 5;
            Headertable.AddCell(R1);

            PdfPCell R11 = new PdfPCell(new Phrase(((Captain.Common.Utilities.ListItem)cmbRepSeq.SelectedItem).Text.ToString().Trim(), TableFont));
            R11.HorizontalAlignment = Element.ALIGN_LEFT;
            R11.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R11.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R11.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R11.PaddingBottom = 5;
            Headertable.AddCell(R11);

            //PdfPCell R2 = new PdfPCell(new Phrase(lblEnrlStat.Text.Trim() + " : " + ((Captain.Common.Utilities.ListItem)cmbEnrlStat.SelectedItem).Text.ToString(), TableFont));
            //R2.HorizontalAlignment = Element.ALIGN_LEFT;
            //R2.Colspan = 2;
            //R2.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //Headertable.AddCell(R2);

            PdfPCell R2 = new PdfPCell(new Phrase("  " + lblEnrlStat.Text.Trim() /*+ " : " + strStatusCodes*/, TableFont));
            R2.HorizontalAlignment = Element.ALIGN_LEFT;
            R2.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R2.PaddingBottom = 5;
            Headertable.AddCell(R2);

            PdfPCell R22 = new PdfPCell(new Phrase(strStatusCodes, TableFont));
            R22.HorizontalAlignment = Element.ALIGN_LEFT;
            R22.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R22.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R22.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R22.PaddingBottom = 5;
            Headertable.AddCell(R22);

            string Repeaters = "Both";
            if (rbRepeaters.Checked == true) Repeaters = rbRepeaters.Text.Trim();
            else if (rbNonRepeaters.Checked == true) Repeaters = rbNonRepeaters.Text.Trim();

            PdfPCell R3 = new PdfPCell(new Phrase("  " + lblChild.Text.Trim() /*+ " : " + Repeaters*/, TableFont));
            R3.HorizontalAlignment = Element.ALIGN_LEFT;
            R3.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R3.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R3.PaddingBottom = 5;
            Headertable.AddCell(R3);

            PdfPCell R33 = new PdfPCell(new Phrase(Repeaters, TableFont));
            R33.HorizontalAlignment = Element.ALIGN_LEFT;
            R33.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R33.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R33.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R33.PaddingBottom = 5;
            Headertable.AddCell(R33);

            //(rbSiteCode.Checked == true ? rbSiteCode.Text : rbTownCode.Text)
            PdfPCell R5 = new PdfPCell(new Phrase("  " + "Roll Over" /*+ (rbRollOver.Checked == true ? rbRollOver.Text : rbProduct.Text)*/, TableFont));
            R5.HorizontalAlignment = Element.ALIGN_LEFT;
            R5.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R5.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R5.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R5.PaddingBottom = 5;
            Headertable.AddCell(R5);

            PdfPCell R55 = new PdfPCell(new Phrase((rbRollOver.Checked == true ? rbRollOver.Text : rbProduct.Text), TableFont));
            R55.HorizontalAlignment = Element.ALIGN_LEFT;
            R55.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R55.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R55.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R55.PaddingBottom = 5;
            Headertable.AddCell(R55);

            PdfPCell R6 = new PdfPCell(new Phrase("  " + lblHeadstart.Text.Trim() /*+ " : " + txtHeadstart.Text.Trim()+"        "+lblHDCutOff.Text.Trim()+"  : "+txtHDCutoff.Text.Trim()+"       "+lblDayCare.Text.Trim()+"  : "+txtDayCareCuttOff.Text.Trim()*/, TableFont));
            R6.HorizontalAlignment = Element.ALIGN_LEFT;
            R6.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R6.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R6.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R6.PaddingBottom = 5;
            Headertable.AddCell(R6);

            PdfPCell R66 = new PdfPCell(new Phrase(txtHeadstart.Text.Trim() + "   " + lblHDCutOff.Text.Trim() + ": " + txtHDCutoff.Text.Trim() + "   " + lblDayCare.Text.Trim() + ": " + txtDayCareCuttOff.Text.Trim(), TableFont));
            R66.HorizontalAlignment = Element.ALIGN_LEFT;
            R66.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R66.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R66.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R66.PaddingBottom = 5;
            Headertable.AddCell(R66);

            if (chkbRollOvrTrans.Checked == true)
            {
                PdfPCell R7 = new PdfPCell(new Phrase("  " + chkbRollOvrTrans.Text.Trim() /*+ "       Selected Funds : " + strFundingCodes*/, TableFont));
                R7.HorizontalAlignment = Element.ALIGN_LEFT;
                R7.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                R7.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                R7.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                R7.PaddingBottom = 5;
                Headertable.AddCell(R7);

                PdfPCell R77 = new PdfPCell(new Phrase("Selected Funds: " + strFundingCodes, TableFont));
                R77.HorizontalAlignment = Element.ALIGN_LEFT;
                R77.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                R77.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                R77.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                R77.PaddingBottom = 5;
                Headertable.AddCell(R77);

                PdfPCell R8 = new PdfPCell(new Phrase("  " + "Enrollment Dates", TableFont));
                R8.HorizontalAlignment = Element.ALIGN_LEFT;
                R8.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                R8.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                R8.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                R8.PaddingBottom = 5;
                Headertable.AddCell(R8);

                PdfPCell R88 = new PdfPCell(new Phrase(rbEnrlDate.Checked == true ? rbEnrlDate.Text : rbKeepExisEnrlDate.Text, TableFont));
                R88.HorizontalAlignment = Element.ALIGN_LEFT;
                R88.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                R88.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                R88.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                R88.PaddingBottom = 5;
                Headertable.AddCell(R88);

                if (rbEnrlDate.Checked == true)
                {
                    PdfPCell R9 = new PdfPCell(new Phrase("  " + "Enrollment Date" /*+ dtpFrmDate.Text.Trim()*/, TableFont));
                    R9.HorizontalAlignment = Element.ALIGN_LEFT;
                    R9.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    R9.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    R9.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                    R9.PaddingBottom = 5;
                    Headertable.AddCell(R9);

                    PdfPCell R99 = new PdfPCell(new Phrase(dtpFrmDate.Text.Trim(), TableFont));
                    R99.HorizontalAlignment = Element.ALIGN_LEFT;
                    R99.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    R99.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    R99.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                    R99.PaddingBottom = 5;
                    Headertable.AddCell(R99);
                }

            }
            

            document.Add(Headertable);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase("Generated By : ", fnttimesRoman_Italic), 33, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.GetMemberName(BaseForm.UserProfile.FirstName.Trim(), BaseForm.UserProfile.MI.Trim(), BaseForm.UserProfile.LastName.Trim(), "3"), fnttimesRoman_Italic), 90, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase("Generated On : ", fnttimesRoman_Italic), 410, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase(DateTime.Now.ToString(), fnttimesRoman_Italic), 468, 40, 0);
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in gvApp.Rows)
            {
                if (item.Cells["Check"].ReadOnly == false)
                    item.Cells["Check"].Value = true;
            }
        }

        private void btnUnSelect_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in gvApp.Rows)
            {
                //if (item.Cells["Check"].ReadOnly == false)
                    item.Cells["Check"].Value = false;
            }
        }

        private void Pb_Help_Click(object sender, EventArgs e)
        {
            //Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "HSSB0124");
        }

        private void txtAppNo_Leave(object sender, EventArgs e)
        {
            if (txtAppNo.Text.Trim() != string.Empty)
            {
                string Year1 = string.Empty;
                if (CmbYear.Visible == true)
                    Year1 = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString().Trim();
                if (!string.IsNullOrEmpty(Year1.Trim()))
                    Year1 = (int.Parse(Year1) - 1).ToString();

                txtAppNo.Text = SetLeadingZeros(txtAppNo.Text);
                string Active_stat = string.Empty;
                if (rbRepeaters.Checked == true)
                    Active_stat = "R";
                else if (rbNonRepeaters.Checked == true)
                    Active_stat = "N";
                strStatusCodes = string.Empty;
                if (Rb_Enroll_Status.Checked)
                {
                    strStatusCodes = Get_Sel_StatusCodes();
                }
                else strStatusCodes = "A";

                string ExcludeIntakes = "N";
                if (chkbExcludIntake.Checked) ExcludeIntakes = "Y";


                List<HSSB2106Report_Entity> Report_list = _model.ChldTrckData.GetHSSB2124_Report(Agency, Depart, Program, Year1, txtAppNo.Text, "K", string.Empty, strFundingCodes, Active_stat, strStatusCodes, string.Empty, string.Empty, ((Captain.Common.Utilities.ListItem)cmbRepSeq.SelectedItem).Value.ToString().Trim(),ExcludeIntakes);
                //List<HSSB2106Report_Entity> SeaarchReport_list = _model.ChldTrckData.GetChldTrck_Report(Agency, Depart, Program, Program_Year, txtAppNo.Text, null, null, null, null, null, null, null, null);
                if (Report_list.Count == 0)
                {
                    txtAppNo.Text = string.Empty;
                    gvApp.Rows.Clear();
                    btnUnSelect.Visible = btnSelectAll.Visible = false;
                    AlertBox.Show("Applicant Record was not found for prior Program Year: " + Year1, MessageBoxIcon.Warning);
                }
                else
                {
                    if (Report_list[0].ActiveStatus != "Y")
                        MessageBox.Show("You are rolling over an INACTIVE application, do you want to continue", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question,onclose: OnInactveApp);
                    else
                        fillgrid();
                }
            }
        }

        private void Rb_Enroll_Status_Click(object sender, EventArgs e)
        {
            if (Rb_Enroll_Status.Checked == true)
            {
                SelectZipSiteCountyForm countyform = new SelectZipSiteCountyForm(BaseForm, ListcommonEnrollEntity, BaseForm.BaseAgencyControlDetails.AgyShortName.Trim(),"EnrollStatus");
                countyform.FormClosed += new FormClosedEventHandler(SelectZipSiteCountyFormClosed);
                countyform.StartPosition = FormStartPosition.CenterScreen;
                countyform.ShowDialog();
            }
        }

        private void SelectZipSiteCountyFormClosed(object sender, FormClosedEventArgs e)
        {
            SelectZipSiteCountyForm form = sender as SelectZipSiteCountyForm;
            if (form.DialogResult == DialogResult.OK)
            {
                if (form.FormType == "EnrollStatus")
                {
                    ListcommonEnrollEntity = form.SelectedCountyEntity;
                }
            }
        }

        private void Rb_Enroll_All_Click(object sender, EventArgs e)
        {
            //if (ListcommonEnrollEntity.Count > 0)
                ListcommonEnrollEntity = new List<CommonEntity>();
        }

        private void HSSB0124_Report_ToolClick(object sender, ToolClickEventArgs e)
        {
            Application.Navigate(CommonFunctions.CreateZenHelps(Privileges.Program, 0, BaseForm.BusinessModuleID.ToString(), "", ""), target: "_blank");
        }

        private void OnInactveApp(DialogResult dialogResult)
        {
            //Gizmox.WebGUI.Forms.Form senderform = (Gizmox.WebGUI.Forms.Form)sender;

            //if (senderform != null)
            //{
                if (DialogResult.Yes == dialogResult)
                {
                    fillgrid();
                }
            //}
        }

        private string SetLeadingZeros(string TmpSeq)
        {
            int Seq_len = TmpSeq.Trim().Length;
            string TmpCode = null;
            TmpCode = TmpSeq.ToString().Trim();
            switch (Seq_len)
            {
                case 7: TmpCode = "0" + TmpCode; break;
                case 6: TmpCode = "00" + TmpCode; break;
                case 5: TmpCode = "000" + TmpCode; break;
                case 4: TmpCode = "0000" + TmpCode; break;
                case 3: TmpCode = "00000" + TmpCode; break;
                case 2: TmpCode = "000000" + TmpCode; break;
                case 1: TmpCode = "0000000" + TmpCode; break;
                //default: MessageBox.Show("Table Code should not be blank", "CAP Systems", MessageBoxButtons.OK);  TxtCode.Focus();
                //    break;
            }
            return (TmpCode);
        }


    }
}