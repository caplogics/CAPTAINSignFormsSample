#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;


using Wisej.Web;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Objects;
using Captain.Common.Model.Data;
using Captain.Common.Utilities;

using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using Microsoft.Ajax.Utilities;
#endregion

namespace Captain.Common.Views.Forms
{
    public partial class CaseNotesPrintPreview : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;

        #endregion

        public CaseNotesPrintPreview(BaseForm baseForm, PrivilegeEntity privilegeEntity, string Year, string strApp, string strName)
        {
            InitializeComponent();
            BaseForm = baseForm;
            _model = new CaptainModel();
            sizecombo();
            propScreenType = string.Empty;
            strYear = Year;
            propAppNo = strApp;
            propAppName = strName;
            Privilege = privilegeEntity;
            this.Text = privilegeEntity.PrivilegeName + " - " + "Print/Preview Case Notes ";
            if (Privilege.Program.Trim() == "CASE0012" || Privilege.Program.Trim() == "ADMN0011")
            {
                lblApplicationNo.Text = "Code";
                lblClientName.Text = "Name:";
            }
            lblApplicationNon.Text = propAppNo;
            lblClientNameD.Text = propAppName;
            propReportPath = _model.lookupDataAccess.GetReportPath();
            DataSet AgencyData = Captain.DatabaseLayer.ZipCodePlusAgency.GetAgencyControlDetails("00");
            string StrAgency = string.Empty;
            chkPrintName.Checked = true;

            // Added by Vikash on 05/16/2023 for displaying Hierarchy on tob of the form as per May 2023 Enhancements doc.
            string Name = _model.lookupDataAccess.GetHierachyDescription("3", BaseForm.BaseAdminAgency, BaseForm.BaseDept, BaseForm.BaseProg);
            lblHierarchy.Text = BaseForm.BaseAgency + "-" + BaseForm.BaseDept + "-" + BaseForm.BaseProg + " [" + Name + "]";

            fillDetails();

        }

        public CaseNotesPrintPreview(BaseForm baseForm, PrivilegeEntity privilegeEntity, string Year, string strApp, string strName, string FieldName)
        {
            InitializeComponent();
            BaseForm = baseForm;
            _model = new CaptainModel();
            sizecombo();
            propScreenType = string.Empty;
            strYear = Year;
            propAppNo = strApp;
            propAppName = strName;
            strfieldName = FieldName;
            Privilege = privilegeEntity;
            this.Text = privilegeEntity.PrivilegeName + " - " + "Print/Preview Case Notes ";
            lblApplicationNon.Text = propAppNo;
            lblClientNameD.Text = propAppName;
            propReportPath = _model.lookupDataAccess.GetReportPath();
            DataSet AgencyData = Captain.DatabaseLayer.ZipCodePlusAgency.GetAgencyControlDetails("00");
            chkPrintName.Checked = true;
            //string StrAgency = string.Empty;
            //if (AgencyData != null && AgencyData.Tables[0].Rows.Count > 0)
            //{
            //    StrAgency = Consts.Common.ServerLocation + AgencyData.Tables[0].Rows[0]["ACR_SHORT_NAME"].ToString().Trim();
            //}
            //if (StrAgency == "UETHDA") chkPrintName.Checked = true; else chkPrintName.Checked = false;

            // Added by Vikash on 05/16/2023 for displaying Hierarchy on tob of the form as per May 2023 Enhancements doc.
            string Name = _model.lookupDataAccess.GetHierachyDescription("3", BaseForm.BaseAdminAgency, BaseForm.BaseDept, BaseForm.BaseProg);
            //lblHierarchy.Text = BaseForm.BaseAgency + "-" + BaseForm.BaseDept + "-" + BaseForm.BaseProg + " [" + Name + "]";
            lblHierarchy.Text = strfieldName.Substring(0, 2) + "-" + strfieldName.Substring(2, 2) + "-" + strfieldName.Substring(4, 2) + " [" + Name + "]";

            fillClientInqDetails();


        }

        public CaseNotesPrintPreview(BaseForm baseForm, string strClientMsg, string strVerMsg, bool boolintake, bool boolcustom, bool boolincomeVer, string strClientDisMsg, string strCustomDisMsg, string strVerDisMsg, string strincompletMsg, bool boolcaseincome, string strincomemsg)
        {
            InitializeComponent();
            BaseForm = baseForm;
            _model = new CaptainModel();
            sizecombo();
            lblSize.Visible = false;
            cmbsize.Visible = false;
            propAppNo = BaseForm.BaseApplicationNo;
            propClientIntakeMsg = strClientMsg;
            propVerMsg = strVerMsg;
            this.Text = "Incomplete Intake Controls";
            lblApplicationNon.Text = propAppNo;
            lblClientNameD.Text = BaseForm.BaseApplicationName;
            propScreenType = "Admin Control";
            categorychk.Visible = false; categorychk.ShowInVisibilityMenu = false;
            ReceiveDate.Visible = true;
            dataGridViewTextBoxColumn1.Visible = false; dataGridViewTextBoxColumn1.ShowInVisibilityMenu = false;
            ScreenName.Width = 200;
            ReceiveDate.Width = 380;
            pnlRdbBtns.Visible = false;
            pnlRdbBtns.Enabled = false;
            chkPrintName.Visible = false;
            btnPrint.Visible = false;
            lblCaseNotes.Text = "Required Field Details";
            boolIntake = boolintake;
            boolCustom = boolcustom;
            boolIncomeVer = boolincomeVer;
            propClientDisMsg = strClientDisMsg;
            propCustomDisMsg = strCustomDisMsg;
            propVerDisMsg = strVerDisMsg;
            propIncompleteMsg = strincompletMsg;
            boolCaseIncome = boolcaseincome;
            propCaseIncomeMsg = strincomemsg;

            // Added by Vikash on 05/16/2023 for displaying Hierarchy on tob of the form as per May 2023 Enhancements doc.
            string Name = _model.lookupDataAccess.GetHierachyDescription("3", BaseForm.BaseAdminAgency, BaseForm.BaseDept, BaseForm.BaseProg);
            lblHierarchy.Text = BaseForm.BaseAgency + "-" + BaseForm.BaseDept + "-" + BaseForm.BaseProg + " [" + Name + "]";

            fillClientInqDetails(); //Vikash Uncommenteed for Incomplete Intake Controls Issue(12/26/2022)

        }

        public string propClientIntakeMsg { get; set; }
        public string propVerMsg { get; set; }
        public BaseForm BaseForm { get; set; }
        public string propAppNo { get; set; }
        public string strfieldName { get; set; }
        public string propAppName { get; set; }
        public PrivilegeEntity Privilege { get; set; }
        public string propReportPath { get; set; }
        string strYear = "    ";
        public string propScreenType { get; set; }
        public bool boolIntake { get; set; }
        public bool boolCustom { get; set; }
        public bool boolIncomeVer { get; set; }
        public bool boolCaseIncome { get; set; }
        string propClientDisMsg { get; set; }
        string propCustomDisMsg { get; set; }
        string propVerDisMsg { get; set; }
        string propIncompleteMsg { get; set; }
        string propCaseIncomeMsg { get; set; }
        public string PropAgency { get; set; }
        public string PropDept { get; set; }
        public string PropProg { get; set; }
        public string PropYear { get; set; }

        public List<CASEACTEntity> SP_Activity_Details { get; set; }
        public List<CASEMSEntity> SP_MS_Details { get; set; }
        public List<CASECONTEntity> CASECONT_List { get; set; }
        public List<CAMASTEntity> CA_List { get; set; }
        public List<MSMASTEntity> MS_List { get; set; }
        public List<CommonEntity> commonHowwhere { get; set; }

        private void GetData()
        {
            CASEACTEntity CA_Pass_Entity = new CASEACTEntity();
            CA_Pass_Entity.Agency = PropAgency;
            CA_Pass_Entity.Dept = PropDept;
            CA_Pass_Entity.Program = PropProg;


            //CA_Pass_Entity.Year = BaseYear;                        
            CA_Pass_Entity.Year = strYear;                             // Year will be always Four-Spaces in CASEACT
            CA_Pass_Entity.App_no = propAppNo;
            CA_Pass_Entity.ACT_Code = null;
            CA_Pass_Entity.Service_plan = null;//Entity.service_plan;
            CA_Pass_Entity.Branch = null; CA_Pass_Entity.Group = null;
            CA_Pass_Entity.ACT_Date = CA_Pass_Entity.ACT_Seq = CA_Pass_Entity.Site = CA_Pass_Entity.Fund1 = null;
            CA_Pass_Entity.Fund2 = CA_Pass_Entity.Fund3 = CA_Pass_Entity.Caseworker = CA_Pass_Entity.Vendor_No = null;
            CA_Pass_Entity.Check_Date = CA_Pass_Entity.Check_No = CA_Pass_Entity.Cost = CA_Pass_Entity.Followup_On = null;
            CA_Pass_Entity.Followup_Comp = CA_Pass_Entity.Followup_By = CA_Pass_Entity.Refer_Data = CA_Pass_Entity.Cust_Code1 = null;
            CA_Pass_Entity.Cust_Value1 = CA_Pass_Entity.Cust_Code2 = CA_Pass_Entity.Cust_Value2 = CA_Pass_Entity.Cust_Code3 = null;
            CA_Pass_Entity.Cust_Value3 = CA_Pass_Entity.Lstc_Date = CA_Pass_Entity.Lsct_Operator = CA_Pass_Entity.Add_Date = null;
            CA_Pass_Entity.Add_Operator = CA_Pass_Entity.ACT_ID = null; CA_Pass_Entity.Bulk = CA_Pass_Entity.Act_PROG = null;
            CA_Pass_Entity.Cust_Code4 = CA_Pass_Entity.Cust_Value4 = CA_Pass_Entity.Cust_Code5 = CA_Pass_Entity.Cust_Value5 = null;
            CA_Pass_Entity.Units = CA_Pass_Entity.UOM = CA_Pass_Entity.Curr_Grp = null;
            CA_Pass_Entity.SPM_Seq = null;

            SP_Activity_Details = _model.SPAdminData.Browse_CASEACT(CA_Pass_Entity, "Browse");

            CASEMSEntity Search_MS_Details = new CASEMSEntity();
            Search_MS_Details.Agency = BaseForm.BaseAgency;
            Search_MS_Details.Dept = BaseForm.BaseDept;
            Search_MS_Details.Program = BaseForm.BaseProg;
            //Search_MS_Details.Year = BaseYear; 
            Search_MS_Details.Year = strYear;                              // Year will be always Four-Spaces in CASEMS
            Search_MS_Details.App_no = propAppNo;
            Search_MS_Details.MS_Code = null;//dr["sp2_cams_code"].ToString().Trim();
            Search_MS_Details.SPM_Seq = null;//Entity.Seq;

            Search_MS_Details.Service_plan = null;//Entity.service_plan;
            Search_MS_Details.Branch = null; Search_MS_Details.Group = null;//dr["sp2_orig_grp"].ToString().Trim();
            Search_MS_Details.ID = Search_MS_Details.Date = Search_MS_Details.CaseWorker = Search_MS_Details.Site = null;
            Search_MS_Details.Result = Search_MS_Details.OBF = Search_MS_Details.Add_Operator = null;
            Search_MS_Details.Lstc_Date = Search_MS_Details.Lsct_Operator = Search_MS_Details.Add_Date = Search_MS_Details.Bulk =
            Search_MS_Details.Acty_PROG = Search_MS_Details.Curr_Grp = null;

            SP_MS_Details = _model.SPAdminData.Browse_CASEMS(Search_MS_Details, "Browse");

            CASECONTEntity Cont_Search_Entity = new CASECONTEntity();
            Cont_Search_Entity.Agency = BaseForm.BaseAgency;
            Cont_Search_Entity.Dept = BaseForm.BaseDept;
            Cont_Search_Entity.Program = BaseForm.BaseProg;
            Cont_Search_Entity.App_no = propAppNo;

            Cont_Search_Entity.Year = "    ";
            if (!string.IsNullOrEmpty(BaseForm.BaseYear))
                Cont_Search_Entity.Year = BaseForm.BaseYear;

            Cont_Search_Entity.Contact_No = Cont_Search_Entity.Contact_Name = Cont_Search_Entity.CaseWorker = Cont_Search_Entity.Cont_Date = null;
            Cont_Search_Entity.Duration_Type = Cont_Search_Entity.Time = Cont_Search_Entity.Time_Starts = Cont_Search_Entity.Time_Ends = null;
            Cont_Search_Entity.Duration = Cont_Search_Entity.How_Where = null;
            Cont_Search_Entity.Language = Cont_Search_Entity.Interpreter = Cont_Search_Entity.Refer_From = Cont_Search_Entity.BillTO = Cont_Search_Entity.BillTo_UOM = null;
            Cont_Search_Entity.Cust1_Code = Cont_Search_Entity.Cust1_Value = Cont_Search_Entity.Cust2_Code = Cont_Search_Entity.Cust2_Value = Cont_Search_Entity.Cust3_Code = null;

            Cont_Search_Entity.Cust3_Value = Cont_Search_Entity.Bridge = Cont_Search_Entity.Lsct_Operator = Cont_Search_Entity.Lstc_Date = Cont_Search_Entity.Add_Date = null;
            Cont_Search_Entity.Add_Operator = null;

            int TmpCount = 0, Cont_Sel_Index = 0;
            CASECONT_List = _model.SPAdminData.Browse_CASECONT(Cont_Search_Entity, "Browse");

            CA_List = _model.SPAdminData.Browse_CAMAST("Code", null, null, null);
            MS_List = _model.SPAdminData.Browse_MSMAST("Code", null, null, null, null);

            commonHowwhere = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.CONTACTHOWHERE, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty);

        }

        private void fillDetails()
        {

            //if (BaseForm.BaseYear != string.Empty && BaseForm.BaseYear != null)
            //{
            //    strYear = BaseForm.BaseYear;
            //}

            //strYear = "    ";
            //if (!string.IsNullOrEmpty(BaseForm.BaseYear))
            //    strYear = BaseForm.BaseYear;

            PropAgency = BaseForm.BaseAgency; PropDept = BaseForm.BaseDept; PropProg = BaseForm.BaseProg; PropYear = BaseForm.BaseYear;

            string components = BaseForm.UserProfile.Components.ToString();

            List<ChldTrckEntity> TrackList = _model.ChldTrckData.Browse_CasetrckDetails("01");

            List<string> incomeList = new List<string>();
            if (components != null)
            {
                string[] incomeTypes = components.Split(' ');
                for (int i = 0; i < incomeTypes.Length; i++)
                {
                    incomeList.Add(incomeTypes.GetValue(i).ToString());
                }
            }
            int rowindex;
            string strScreenDesc = string.Empty;
            string strFieldName = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg + strYear + propAppNo;
            if (Privilege.Program.Trim() == "CASE0012")
            {
                strFieldName = BaseForm.BaseAgency + "    " + propAppNo;
            }
            if (Privilege.Program.Trim() == "ADMN0011")
            {
                strFieldName = propAppNo;
            }

            //Added by Sudheer on 01/28/2021 for to print Activity or Contact Information
            GetData();

            // List<EMPLFUNCEntity> empfuncEntity = _model.TmsApcndata.GetCaseNotesDesc(BaseForm.UserID, Privilege.ModuleCode, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg, strFieldName);
            //empfuncEntity = empfuncEntity.OrderByDescending(u => Convert.ToDateTime(u.CaseNotesLstcDate)).ThenBy(u => u.ScrCode).ToList();
            List<EMPLFUNCEntity> empfuncEntity = new List<EMPLFUNCEntity>();

            if (Privilege.Program.ToUpper() == "ADMN0011")
                empfuncEntity = _model.TmsApcndata.GetCaseNotesDesc(BaseForm.UserID, Privilege.ModuleCode, "******", strFieldName);
            else
            {
                empfuncEntity = _model.TmsApcndata.GetCaseNotesDesc(BaseForm.UserID, Privilege.ModuleCode, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg, strFieldName);
                empfuncEntity = empfuncEntity.OrderBy(e => DateTime.TryParse(e.CaseNotesDate, out _))
                    .ThenByDescending(e => DateTime.TryParse(e.CaseNotesDate, out var dt) ? dt : DateTime.MinValue).ToList();
            }

            foreach (var empfunen in empfuncEntity)
            {
                if (empfunen.ScrCode.Trim() == "CASE2007" || empfunen.ScrCode.Trim() == "HOU00002" || empfunen.ScrCode.Trim() == "HOU00012")
                { }
                else
                {
                    if (empfunen.CaseNotesScreenName.ToUpper().Trim() == "CASE00064")
                    {
                        //strScreenDesc = "Service/Activity(Milestone) Posting";
                        strScreenDesc = "Outcome";
                        empfunen.ScrDesc = strScreenDesc;
                    }
                    else if (empfunen.CaseNotesScreenName.ToUpper().Trim() == "CASE00063")
                    {
                        //strScreenDesc = "Service/Activity(CA) Posting";
                        strScreenDesc = "Service";
                        empfunen.ScrDesc = strScreenDesc;
                    }
                    else if (empfunen.CaseNotesScreenName.ToUpper().Trim() == "CASE00061" || empfunen.CaseNotesScreenName.ToUpper().Trim() == "CASE10061")
                    {
                        //strScreenDesc = "Service/Activity(Contact) Posting";
                        strScreenDesc = "Contact";
                        empfunen.ScrDesc = strScreenDesc;
                    }
                    else if (empfunen.CaseNotesScreenName.ToUpper().Trim() == "CASE00065")
                    {
                        //strScreenDesc = "Service/Activity(Referred) Posting";
                        strScreenDesc = "Referred From/To";
                        empfunen.ScrDesc = strScreenDesc;
                    }
                    else if (empfunen.CaseNotesScreenName.ToUpper().Trim() == "MAT00003")
                    {
                        strScreenDesc = "Assessment";
                        empfunen.ScrDesc = strScreenDesc;
                    }
                    else if (empfunen.CaseNotesScreenName.ToUpper().Trim() == "CASE2001")
                    {
                        strScreenDesc = "Intake";
                        empfunen.ScrDesc = strScreenDesc;
                    }
                    else if (empfunen.CaseNotesScreenName.ToUpper().Trim() == "CASE2004")
                    {
                        strScreenDesc = "SIM";
                        empfunen.ScrDesc = strScreenDesc;
                    }
                    else if (empfunen.CaseNotesScreenName.ToUpper().Trim() == "CASE0016")
                    {
                        strScreenDesc = "Benefit Maintenance and Usage";
                        empfunen.ScrDesc = strScreenDesc;
                    }
                    else
                        strScreenDesc = empfunen.ScrDesc;
                    if (empfunen.ScrCode == "TMS00081")
                    {
                        if (empfunen.CaseNotesFieldName.Length > 18)
                        {
                            if (empfunen.CaseNotesFieldName.Substring(18, 1) == "0")
                            {
                                strScreenDesc = "Control Card – Incomplete Letter";
                                empfunen.ScrDesc = strScreenDesc;
                            }
                        }
                    }

                    if (empfunen.ScrCode == "HSS00134")
                    {
                        if (components.Contains("****"))
                        {
                            //** rowindex = dataGridCaseNotes.Rows.Add(false, strScreenDesc, LookupDataAccess.Getdate(empfunen.CaseNotesLstcDate), empfunen.CaseNotesLstcOpr);
                            rowindex = dataGridCaseNotes.Rows.Add(false, strScreenDesc, empfunen.CaseNotesScreenDesc, LookupDataAccess.Getdate(empfunen.CaseNotesDate), empfunen.CaseNotesLstcOpr);
                            dataGridCaseNotes.Rows[rowindex].Tag = empfunen;
                            CommonFunctions.setTooltip(rowindex, empfunen.CaseNotesAddOpr, empfunen.CaseNotesAddDate, empfunen.CaseNotesLstcOpr, empfunen.CaseNotesLstcDate, dataGridCaseNotes);
                        }
                        else
                        {
                            if (empfunen.CaseNotesFieldName.Length > 18)
                            {
                                string strtask = empfunen.CaseNotesFieldName.Substring(18, 4).ToString();
                                if (components != null)
                                {
                                    List<ChldTrckEntity> Track_Det;
                                    foreach (string item in incomeList)
                                    {
                                        if (item == "0000")
                                            Track_Det = TrackList.FindAll(u => u.COMPONENT.Trim().Equals(item));
                                        else
                                            Track_Det = TrackList.FindAll(u => u.COMPONENT.Trim().Equals(item) && u.Agency.Trim().Equals(BaseForm.BaseAgency.Trim()) && u.Dept.Equals(BaseForm.BaseDept.Trim()) && u.Prog.Equals(BaseForm.BaseProg.Trim()));
                                        if (Track_Det.Count > 0)
                                        {
                                            if (Track_Det.FindAll(u => u.TASK.Contains(strtask)).ToList().Count > 0)
                                            {
                                                //**rowindex = dataGridCaseNotes.Rows.Add(false, strScreenDesc, LookupDataAccess.Getdate(empfunen.CaseNotesLstcDate), empfunen.CaseNotesLstcOpr);
                                                rowindex = dataGridCaseNotes.Rows.Add(false, strScreenDesc, empfunen.CaseNotesScreenDesc, LookupDataAccess.Getdate(empfunen.CaseNotesDate), empfunen.CaseNotesLstcOpr);
                                                dataGridCaseNotes.Rows[rowindex].Tag = empfunen;
                                                CommonFunctions.setTooltip(rowindex, empfunen.CaseNotesAddOpr, empfunen.CaseNotesAddDate, empfunen.CaseNotesLstcOpr, empfunen.CaseNotesLstcDate, dataGridCaseNotes);
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                    }
                    else
                    {

                        //**rowindex = dataGridCaseNotes.Rows.Add(false, strScreenDesc, LookupDataAccess.Getdate(empfunen.CaseNotesLstcDate), empfunen.CaseNotesLstcOpr);
                        rowindex = dataGridCaseNotes.Rows.Add(false, strScreenDesc, empfunen.CaseNotesScreenDesc, LookupDataAccess.Getdate(empfunen.CaseNotesDate), empfunen.CaseNotesLstcOpr);
                        dataGridCaseNotes.Rows[rowindex].Tag = empfunen;
                        CommonFunctions.setTooltip(rowindex, empfunen.CaseNotesAddOpr, empfunen.CaseNotesAddDate, empfunen.CaseNotesLstcOpr, empfunen.CaseNotesLstcDate, dataGridCaseNotes);
                    }
                }
            }
            if (dataGridCaseNotes.Rows.Count > 0) dataGridCaseNotes.Rows[0].Selected = true;
            // dataGridCaseNotes.Sort(dataGridCaseNotes.Columns["ScreenName"], ListSortDirection.Ascending);
        }


        private void fillClientInqDetails()
        {
            if (propScreenType == string.Empty)
            {

                //if (BaseForm.BaseYear != string.Empty && BaseForm.BaseYear != null)
                //{
                //    strYear = BaseForm.BaseYear;
                //}

                //strYear = "    ";
                //if (!string.IsNullOrEmpty(BaseForm.BaseYear))
                //    strYear = BaseForm.BaseYear;


                string components = BaseForm.UserProfile.Components.ToString();

                List<ChldTrckEntity> TrackList = _model.ChldTrckData.Browse_CasetrckDetails("01");



                List<string> incomeList = new List<string>();
                if (components != null)
                {
                    string[] incomeTypes = components.Split(' ');
                    for (int i = 0; i < incomeTypes.Length; i++)
                    {
                        incomeList.Add(incomeTypes.GetValue(i).ToString());
                    }
                }
                int rowindex;
                string strScreenDesc = string.Empty;
                string strFieldName = strfieldName + strYear + propAppNo;
                List<EMPLFUNCEntity> empfuncEntity = _model.TmsApcndata.GetCaseNotesDesc(BaseForm.UserID, Privilege.ModuleCode, BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg, strFieldName);

                if (!string.IsNullOrEmpty(strfieldName.Trim()))
                {
                    PropAgency = strfieldName.Substring(0, 2); PropDept = strfieldName.Substring(2, 2); PropProg = strfieldName.Substring(4, 2); PropYear = strFieldName.Substring(6, 4);
                }
                //Added by Sudheer on 02/13/2021 for to print Activity or Contact Information
                GetData();

                foreach (var empfunen in empfuncEntity)
                {
                    if (empfunen.ScrCode.Trim() == "CASE2007" || empfunen.ScrCode.Trim() == "HOU00002" || empfunen.ScrCode.Trim() == "HOU00012")
                    { }
                    else
                    {
                        if (empfunen.CaseNotesScreenName.ToUpper().Trim() == "CASE00064")
                        {
                            //**strScreenDesc = "Service/Activity(Milestone) Posting";
                            strScreenDesc = "Outcome";
                            empfunen.ScrDesc = strScreenDesc;
                        }
                        else if (empfunen.CaseNotesScreenName.ToUpper().Trim() == "CASE00063")
                        {
                            //**strScreenDesc = "Service/Activity(CA) Posting";
                            strScreenDesc = "Service";
                            empfunen.ScrDesc = strScreenDesc;
                        }
                        else if (empfunen.CaseNotesScreenName.ToUpper().Trim() == "CASE00061")
                        {
                            //**strScreenDesc = "Service/Activity(Contact) Posting";
                            strScreenDesc = "Contact";
                            empfunen.ScrDesc = strScreenDesc;
                        }
                        else if (empfunen.CaseNotesScreenName.ToUpper().Trim() == "CASE00065")
                        {
                            //**strScreenDesc = "Service/Activity(Referred) Posting";
                            strScreenDesc = "Referred From/To";
                            empfunen.ScrDesc = strScreenDesc;
                        }
                        else if (empfunen.CaseNotesScreenName.ToUpper().Trim() == "MAT00003")
                        {
                            strScreenDesc = "Assessment";
                            empfunen.ScrDesc = strScreenDesc;
                        }
                        else if (empfunen.CaseNotesScreenName.ToUpper().Trim() == "CASE2001")
                        {
                            strScreenDesc = "Intake";
                            empfunen.ScrDesc = strScreenDesc;
                        }
                        else if (empfunen.CaseNotesScreenName.ToUpper().Trim() == "CASE2004")
                        {
                            strScreenDesc = "SIM";
                            empfunen.ScrDesc = strScreenDesc;
                        }
                        else
                            strScreenDesc = empfunen.ScrDesc;
                        if (empfunen.ScrCode == "TMS00081")
                        {
                            if (empfunen.CaseNotesFieldName.Length > 18)
                            {
                                if (empfunen.CaseNotesFieldName.Substring(18, 1) == "0")
                                {
                                    strScreenDesc = "Control Card – Incomplete Letter";
                                    empfunen.ScrDesc = strScreenDesc;
                                }
                            }
                        }

                        if (empfunen.ScrCode == "HSS00134")
                        {
                            if (components.Contains("****"))
                            {
                                //**rowindex = dataGridCaseNotes.Rows.Add(false, strScreenDesc, LookupDataAccess.Getdate(empfunen.CaseNotesLstcDate), empfunen.CaseNotesLstcOpr);
                                rowindex = dataGridCaseNotes.Rows.Add(false, strScreenDesc, empfunen.CaseNotesScreenDesc, LookupDataAccess.Getdate(empfunen.CaseNotesDate), empfunen.CaseNotesLstcOpr);
                                dataGridCaseNotes.Rows[rowindex].Tag = empfunen;
                                CommonFunctions.setTooltip(rowindex, empfunen.CaseNotesAddOpr, empfunen.CaseNotesAddDate, empfunen.CaseNotesLstcOpr, empfunen.CaseNotesLstcDate, dataGridCaseNotes);
                            }
                            else
                            {
                                if (empfunen.CaseNotesFieldName.Length > 18)
                                {
                                    string strtask = empfunen.CaseNotesFieldName.Substring(18, 4).ToString();
                                    if (components != null)
                                    {
                                        List<ChldTrckEntity> Track_Det;
                                        foreach (string item in incomeList)
                                        {
                                            if (item == "0000")
                                                Track_Det = TrackList.FindAll(u => u.COMPONENT.Trim().Equals(item));
                                            else
                                                Track_Det = TrackList.FindAll(u => u.COMPONENT.Trim().Equals(item) && u.Agency.Trim().Equals(BaseForm.BaseAgency.Trim()) && u.Dept.Equals(BaseForm.BaseDept.Trim()) && u.Prog.Equals(BaseForm.BaseProg.Trim()));
                                            if (Track_Det.Count > 0)
                                            {
                                                if (Track_Det.FindAll(u => u.TASK.Contains(strtask)).ToList().Count > 0)
                                                {
                                                    //** rowindex = dataGridCaseNotes.Rows.Add(false, strScreenDesc, LookupDataAccess.Getdate(empfunen.CaseNotesLstcDate), empfunen.CaseNotesLstcOpr);
                                                    rowindex = dataGridCaseNotes.Rows.Add(false, strScreenDesc, empfunen.CaseNotesScreenDesc, LookupDataAccess.Getdate(empfunen.CaseNotesDate), empfunen.CaseNotesLstcOpr);
                                                    dataGridCaseNotes.Rows[rowindex].Tag = empfunen;
                                                    CommonFunctions.setTooltip(rowindex, empfunen.CaseNotesAddOpr, empfunen.CaseNotesAddDate, empfunen.CaseNotesLstcOpr, empfunen.CaseNotesLstcDate, dataGridCaseNotes);
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        else
                        {

                            //**rowindex = dataGridCaseNotes.Rows.Add(false, strScreenDesc, LookupDataAccess.Getdate(empfunen.CaseNotesLstcDate), empfunen.CaseNotesLstcOpr);
                            rowindex = dataGridCaseNotes.Rows.Add(false, strScreenDesc, empfunen.CaseNotesScreenDesc, LookupDataAccess.Getdate(empfunen.CaseNotesDate), empfunen.CaseNotesLstcOpr);
                            dataGridCaseNotes.Rows[rowindex].Tag = empfunen;
                            CommonFunctions.setTooltip(rowindex, empfunen.CaseNotesAddOpr, empfunen.CaseNotesAddDate, empfunen.CaseNotesLstcOpr, empfunen.CaseNotesLstcDate, dataGridCaseNotes);
                        }
                    }
                }
                //dataGridCaseNotes.Sort(dataGridCaseNotes.Columns["ScreenName"], ListSortDirection.Ascending);
            }
            else
            {
                if (boolIntake == false)
                    dataGridCaseNotes.Rows.Add(false, "Client Intake", propClientDisMsg, string.Empty);
                if (boolCustom == false)
                    dataGridCaseNotes.Rows.Add(false, "Client Intake Custom Questions", propCustomDisMsg, string.Empty);
                if (boolIncomeVer == false)
                    dataGridCaseNotes.Rows.Add(false, "Income Verification", propVerDisMsg, string.Empty);
                if (boolCaseIncome == false)
                    dataGridCaseNotes.Rows.Add(false, "Case Income", propCaseIncomeMsg, string.Empty);
                if (!string.IsNullOrEmpty(propIncompleteMsg))
                    dataGridCaseNotes.Rows.Add(false, "Incomplete Income", propIncompleteMsg, string.Empty);
            }
            if (dataGridCaseNotes.Rows.Count > 0) dataGridCaseNotes.Rows[0].Selected = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void dataGridCaseNotes_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridCaseNotes.SelectedRows.Count > 0)
            {
                if (propScreenType == string.Empty)
                {
                    txtDesc.Text = string.Empty;
                    EMPLFUNCEntity row = dataGridCaseNotes.SelectedRows[0].Tag as EMPLFUNCEntity;
                    if (row != null)
                    {
                        txtDesc.Text = row.CaseNotesData;
                    }
                }
                else
                {
                    txtDesc.Text = string.Empty;
                    if (dataGridCaseNotes.SelectedRows[0].Cells["ScreenName"].Value == "Client Intake")
                    {
                        txtDesc.Text = propClientIntakeMsg;
                    }
                    if (dataGridCaseNotes.SelectedRows[0].Cells["ScreenName"].Value == "Client Intake Custom Questions")
                    {
                        CaseSnpEntity snpdata = BaseForm.BaseCaseSnpEntity.Find(u => u.FamilySeq == BaseForm.BaseCaseMstListEntity[0].FamilySeq);
                        List<CustomQuestionsEntity> custQuestions = _model.FieldControls.GetCustomQuestions("CASE2001", "A", BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg, "Sequence", "ACTIVE", "P");
                        custQuestions = custQuestions.FindAll(u => u.CUSTREQUIRED.ToUpper() == "Y").ToList();
                        List<CustomQuestionsEntity> custResponses = _model.CaseMstData.GetCustomQuestionAnswers(snpdata);
                        foreach (CustomQuestionsEntity dr in custQuestions)
                        {
                            string custCode = dr.CUSTCODE.ToString();
                            List<CustomQuestionsEntity> response = custResponses.FindAll(u => u.ACTCODE.Equals(custCode)).ToList();
                            if (response.Count == 0)
                            {
                                txtDesc.Text = txtDesc.Text + dr.CUSTDESC + "\n";
                            }
                        }

                        List<CustomQuestionsEntity> custQuestionshouse = _model.FieldControls.GetCustomQuestions("CASE2001", "H", BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg, "Sequence", "ACTIVE", string.Empty);
                        custQuestionshouse = custQuestionshouse.FindAll(u => u.CUSTREQUIRED.ToUpper() == "Y").ToList();
                        foreach (CustomQuestionsEntity dr in custQuestionshouse)
                        {
                            string custCode = dr.CUSTCODE.ToString();
                            List<CustomQuestionsEntity> response = custResponses.FindAll(u => u.ACTCODE.Equals(custCode)).ToList();
                            if (response.Count == 0)
                            {
                                txtDesc.Text = txtDesc.Text + dr.CUSTDESC + "\n";
                            }
                        }

                    }
                    if (dataGridCaseNotes.SelectedRows[0].Cells["ScreenName"].Value == "Income Verification")
                    {
                        txtDesc.Text = propVerMsg;
                    }
                    if (dataGridCaseNotes.SelectedRows[0].Cells["ScreenName"].Value == "Incomplete Income")
                    {
                        txtDesc.Text = propIncompleteMsg;
                    }
                    if (dataGridCaseNotes.SelectedRows[0].Cells["ScreenName"].Value == "Case Income")
                    {
                        txtDesc.Text = propCaseIncomeMsg;
                    }


                }
            }
        }

        private void CaseNotesPrintPreview_Load(object sender, EventArgs e)
        {
            dataGridCaseNotes_SelectionChanged(sender, e);
            radioButton1_CheckedChanged(sender, e);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dataGridrow in dataGridCaseNotes.Rows)
            {
                dataGridrow.Cells["categorychk"].ReadOnly = true;
                dataGridrow.Cells["categorychk"].Value = true;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dataGridrow in dataGridCaseNotes.Rows)
            {
                dataGridrow.Cells["categorychk"].ReadOnly = false;
                dataGridrow.Cells["categorychk"].Value = false;
            }
        }

        // ................Begin Report Section......

        private void btnPrint_Click_1(object sender, EventArgs e)
        {
            if (dataGridCaseNotes.Rows.Count > 0)
            {
                bool boolselectedprint = true;
                if (rdbSel.Checked == true)
                {
                    boolselectedprint = false;
                    foreach (DataGridViewRow dr in dataGridCaseNotes.Rows)
                    {
                        if (dr.Cells["categorychk"].Value.ToString() == true.ToString())
                        {
                            boolselectedprint = true; break;
                        }
                    }
                }
                if (boolselectedprint)
                {
                    //**On_SaveForm_Closed(PdfName, EventArgs.Empty);
                    On_SaveForm_ClosedNew(PdfName, EventArgs.Empty);

                    if (BaseForm.BaseAgencyControlDetails.ReportSwitch.ToUpper() == "Y")
                    {
                        PdfViewerNewForm objfrm = new PdfViewerNewForm(PdfName);
                        objfrm.StartPosition = FormStartPosition.CenterScreen;

                        objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                        objfrm.ShowDialog();
                    }
                    else
                    {
                        FrmViewer objfrm = new FrmViewer(PdfName);
                        objfrm.StartPosition = FormStartPosition.CenterScreen;

                        objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                        objfrm.ShowDialog();
                    }
                }
                else
                {
                    CommonFunctions.MessageBoxDisplay("Please select at least one Case Note");
                }
            }
            else
                MessageBox.Show("No CaseNotes for this Applicant", "CAPTAIN");
        }

        private void On_Delete_PDF_File(object sender, FormClosedEventArgs e)
        {
            System.IO.File.Delete(PdfName);
        }

        PdfContentByte cb;
        int X_Pos, Y_Pos;
        string Random_Filename = null;
        string PdfName = "Pdf File";
        private void On_SaveForm_Closed(object sender, EventArgs e)
        {
            Random_Filename = null;

            PdfName = "AllCaseNotes_" + propAppNo;
            PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
            try
            {
                if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
                { DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim()); }
            }
            catch (Exception ex)
            {
                CommonFunctions.MessageBoxDisplay("Error");
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

            Document document = new Document();
            document.SetPageSize(iTextSharp.text.PageSize.LETTER.Rotate());
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();
            BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            iTextSharp.text.Font helvetica = new iTextSharp.text.Font(bf_times, 16, 3, new iTextSharp.text.BaseColor(0, 0, 102));
            BaseFont bf_helv = helvetica.GetCalculatedBaseFont(false);
            iTextSharp.text.Font TimesUnderline = new iTextSharp.text.Font(bf_times, 9, 4);
            BaseFont bf_TimesUnderline = TimesUnderline.GetCalculatedBaseFont(true);


            float floatvalue = float.Parse(((Captain.Common.Utilities.ListItem)cmbsize.SelectedItem).Value.ToString());

            iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, floatvalue);
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_times, floatvalue, 1);

            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 8);
            iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 8, 3);

            iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 8, 2);
            iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 9, 4);
            cb = writer.DirectContent;

            PdfPTable APP_details = new PdfPTable(2);
            APP_details.TotalWidth = 750f;
            APP_details.WidthPercentage = 100;
            APP_details.LockedWidth = true;
            float[] APP_details_Widths = new float[] { 30f, 70f };
            APP_details.SetWidths(APP_details_Widths);
            APP_details.HorizontalAlignment = Element.ALIGN_CENTER;
            APP_details.HeaderRows = 3;
            APP_details.SpacingBefore = 9f;

            if (rdbAll.Checked == true)
            {
                PdfPCell Header_Row = new PdfPCell(new Phrase("All CaseNotes", TblFontBold));
                Header_Row.Colspan = 2;
                Header_Row.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                Header_Row.Border = iTextSharp.text.Rectangle.NO_BORDER;
                APP_details.AddCell(Header_Row);
            }
            else
            {
                PdfPCell Header_Row = new PdfPCell(new Phrase("Selected CaseNotes", TblFontBold));
                Header_Row.Colspan = 2;
                Header_Row.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                Header_Row.Border = iTextSharp.text.Rectangle.NO_BORDER;
                APP_details.AddCell(Header_Row);
            }

            if (Privilege.Program.Trim() == "CASE0012")
            {
                PdfPCell Appl_No = new PdfPCell(new Phrase("Code :" + propAppNo.Trim(), TblFontBold));
                Appl_No.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                Appl_No.Border = iTextSharp.text.Rectangle.NO_BORDER;
                APP_details.AddCell(Appl_No);
                string strClientName = string.Empty;
                if (chkPrintName.Checked)
                {
                    strClientName = "Volunteer/Donor Name :" + propAppName.Trim();
                }
                PdfPCell App_Name = new PdfPCell(new Phrase(strClientName, TblFontBold));
                App_Name.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                App_Name.Border = iTextSharp.text.Rectangle.NO_BORDER;
                APP_details.AddCell(App_Name);
            }
            else
            {
                PdfPCell Appl_No = new PdfPCell(new Phrase("App # :" + propAppNo.Trim(), TblFontBold));
                Appl_No.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                Appl_No.Border = iTextSharp.text.Rectangle.NO_BORDER;
                APP_details.AddCell(Appl_No);
                string strClientName = string.Empty;
                if (chkPrintName.Checked)
                {
                    strClientName = "Client Name :" + propAppName.Trim();
                }
                PdfPCell App_Name = new PdfPCell(new Phrase(strClientName, TblFontBold));
                App_Name.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                App_Name.Border = iTextSharp.text.Rectangle.NO_BORDER;
                APP_details.AddCell(App_Name);
            }

            PdfPCell Date = new PdfPCell(new Phrase("", TblFontBold));
            Date.Colspan = 4;
            Date.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
            Date.Border = iTextSharp.text.Rectangle.NO_BORDER;
            APP_details.AddCell(Date);

            string Screen_Desc = null, Priv_ScreenDesc = null;
            PdfPCell Screen = new PdfPCell(new Phrase("Screen Name", TblFontBold));
            Screen.Colspan = 2;
            Screen.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            Screen.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER;
            APP_details.AddCell(Screen);
            foreach (DataGridViewRow dr in dataGridCaseNotes.Rows)
            {
                if (dr.Cells["categorychk"].Value.ToString() == true.ToString())
                {
                    EMPLFUNCEntity row = dr.Tag as EMPLFUNCEntity;

                    //PdfPCell Space = new PdfPCell(new Phrase("" , TblFontBold));
                    //Space.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    //Space.Border = iTextSharp.text.Rectangle.BOX;
                    //APP_details.AddCell(Space);

                    string CaseNotes_Desc = row.CaseNotesData.Trim();
                    Screen_Desc = row.ScrDesc.Trim();
                    //if (Screen_Desc != Priv_ScreenDesc)
                    //{
                    PdfPCell Screen_desc = new PdfPCell(new Phrase(Screen_Desc, Times));
                    Screen_desc.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    Screen_desc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    APP_details.AddCell(Screen_desc);
                    //Priv_ScreenDesc = Screen_Desc;
                    //}
                    //else
                    //{
                    //    PdfPCell Screen_space = new PdfPCell(new Phrase("", Times));
                    //    Screen_space.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    //    Screen_space.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    //    APP_details.AddCell(Screen_space);
                    //}

                    PdfPCell Notes = new PdfPCell(new Phrase(CaseNotes_Desc, Times));
                    Notes.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    Notes.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    APP_details.AddCell(Notes);

                    if (row.CaseNotesScreenName == "CASE00063")
                    {
                        if (SP_Activity_Details.Count > 0)
                        {
                            CASEACTEntity SelAct = SP_Activity_Details.Find(u => (u.Agency + u.Dept + u.Program + u.Year + u.App_no + u.Service_plan.Trim() + u.SPM_Seq + u.Branch.Trim() + u.Group.Trim() + "CA" + u.ACT_Code.Trim() + u.ACT_Seq.Trim()) == row.CaseNotesFieldName.Trim());
                            if (SelAct != null)
                            {
                                PdfPCell H1 = new PdfPCell(new Phrase("Service Description", TblFontBold));
                                H1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                H1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                APP_details.AddCell(H1);

                                PdfPCell H2 = new PdfPCell(new Phrase("Comp Date", TblFontBold));
                                H2.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                H2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                APP_details.AddCell(H2);

                                string CADesc = string.Empty;
                                if (CA_List.Count > 0)
                                {
                                    CAMASTEntity SelCA = CA_List.Find(u => u.Code.Trim() == SelAct.ACT_Code.Trim());
                                    if (SelCA != null) CADesc = SelCA.Desc.Trim();
                                }

                                PdfPCell N1 = new PdfPCell(new Phrase(CADesc, Times));
                                N1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                N1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                APP_details.AddCell(N1);

                                PdfPCell N2 = new PdfPCell(new Phrase(LookupDataAccess.Getdate(SelAct.ACT_Date.Trim()), Times));
                                N2.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                N2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                APP_details.AddCell(N2);

                            }
                        }
                    }
                    else if (row.CaseNotesScreenName == "CASE00064")
                    {
                        if (SP_MS_Details.Count > 0)
                        {
                            CASEMSEntity SelMS = SP_MS_Details.Find(u => (u.Agency + u.Dept + u.Program + u.Year + u.App_no + u.Service_plan.Trim() + u.SPM_Seq + u.Branch.Trim() + u.Group.Trim() + "CA" + u.MS_Code.Trim() + CommonFunctions.ChangeDateFormat(Convert.ToDateTime(u.Date.ToString()).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat)) == row.CaseNotesFieldName.Trim());
                            if (SelMS != null)
                            {
                                PdfPCell H1 = new PdfPCell(new Phrase("Outcome Description", TblFontBold));
                                H1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                H1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                APP_details.AddCell(H1);

                                PdfPCell H2 = new PdfPCell(new Phrase("Comp Date", TblFontBold));
                                H2.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                H2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                APP_details.AddCell(H2);

                                string CADesc = string.Empty;
                                if (MS_List.Count > 0)
                                {
                                    MSMASTEntity SelCA = MS_List.Find(u => u.Code.Trim() == SelMS.MS_Code.Trim());
                                    if (SelCA != null) CADesc = SelCA.Desc.Trim();
                                }

                                PdfPCell N1 = new PdfPCell(new Phrase(CADesc, Times));
                                N1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                N1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                APP_details.AddCell(N1);

                                PdfPCell N2 = new PdfPCell(new Phrase(LookupDataAccess.Getdate(SelMS.Date.Trim()), Times));
                                N2.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                N2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                APP_details.AddCell(N2);

                            }
                        }
                    }
                    else if (row.CaseNotesScreenName == "CASE00061" || row.CaseNotesScreenName == "CASE10061")
                    {
                        if (CASECONT_List.Count > 0)
                        {
                            CASECONTEntity SelCont = CASECONT_List.Find(u => (u.Agency + u.Dept + u.Program + u.Year + u.App_no + "0000".Substring(0, (4 - u.Seq.Trim().Length)) + u.Seq.Trim()) == row.CaseNotesFieldName.Trim());
                            if (SelCont != null)
                            {
                                PdfPCell H1 = new PdfPCell(new Phrase("Contact How/Where", TblFontBold));
                                H1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                H1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                APP_details.AddCell(H1);

                                PdfPCell H2 = new PdfPCell(new Phrase("Contact Date", TblFontBold));
                                H2.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                H2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                APP_details.AddCell(H2);

                                string CADesc = string.Empty;
                                if (commonHowwhere.Count > 0)
                                {
                                    CommonEntity SelCA = commonHowwhere.Find(u => u.Code.Trim() == SelCont.How_Where.Trim());
                                    if (SelCA != null) CADesc = SelCA.Desc.Trim();
                                }

                                PdfPCell N1 = new PdfPCell(new Phrase(CADesc, Times));
                                N1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                N1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                APP_details.AddCell(N1);

                                PdfPCell N2 = new PdfPCell(new Phrase(LookupDataAccess.Getdate(SelCont.Cont_Date.Trim()), Times));
                                N2.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                N2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                APP_details.AddCell(N2);

                            }
                        }
                    }
                    PdfPCell Notes_Space1 = new PdfPCell(new Phrase("", Times));
                    Notes_Space1.Colspan = 2;
                    Notes_Space1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    Notes_Space1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    APP_details.AddCell(Notes_Space1);

                    PdfPCell Notes_Space2 = new PdfPCell(new Phrase("", Times));
                    Notes_Space2.Colspan = 2;
                    Notes_Space2.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    Notes_Space2.Border = iTextSharp.text.Rectangle.TOP_BORDER;
                    APP_details.AddCell(Notes_Space2);

                }
            }

            //PdfPCell Space = new PdfPCell(new Phrase("", TblFontBold));
            //Space.Colspan = 2;
            //Space.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            //Space.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            //APP_details.AddCell(Space);

            document.Add(APP_details);
            document.Close();
            fs.Close();
            fs.Dispose();

        }

        #region Vikash added for new PDF design conversion on 05/12/2023 as per May 2023 enhancement document
        private void On_SaveForm_ClosedNew(object sender, EventArgs e)
        {
            Random_Filename = null;

            PdfName = "AllCaseNotes_" + propAppNo;
            PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
            try
            {
                if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
                { DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim()); }
            }
            catch (Exception ex)
            {
                CommonFunctions.MessageBoxDisplay("Error");
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

            Document document = new Document(PageSize.A4, 30f, 30f, 60f, 30f);
            document.SetPageSize(iTextSharp.text.PageSize.LETTER.Rotate());
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();
            BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            iTextSharp.text.Font helvetica = new iTextSharp.text.Font(bf_times, 16, 3, new iTextSharp.text.BaseColor(0, 0, 102));
            BaseFont bf_helv = helvetica.GetCalculatedBaseFont(false);
            iTextSharp.text.Font TimesUnderline = new iTextSharp.text.Font(bf_times, 9, 4);
            BaseFont bf_TimesUnderline = TimesUnderline.GetCalculatedBaseFont(true);


            float floatvalue = float.Parse(((Captain.Common.Utilities.ListItem)cmbsize.SelectedItem).Value.ToString());

            iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, floatvalue);
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_times, floatvalue, 1);

            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 8);
            iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 8, 3);

            iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 8, 2);
            iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 9, 4);
            cb = writer.DirectContent;

            PdfPTable APP_details = new PdfPTable(4);
            APP_details.TotalWidth = 730f;//750f;
            APP_details.WidthPercentage = 100;
            APP_details.LockedWidth = true;
            float[] APP_details_Widths = new float[] { 50f, 100f, 40f, 40f };
            APP_details.SetWidths(APP_details_Widths);
            APP_details.HorizontalAlignment = Element.ALIGN_CENTER;
            APP_details.HeaderRows = 3;
            APP_details.SpacingBefore = 9f;

            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase(lblHierarchy.Text.Trim(), TblFontBold), 33, 570, 0);

            if (Privilege.Program.Trim() == "CASE0012")
            {
                string strClientName = string.Empty;
                if (chkPrintName.Checked)
                {
                    strClientName = /*"Volunteer/Donor Name :" + */propAppName.Trim();
                }
                ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase("Code: " + propAppNo.Trim(), TblFontBold), 410, 570, 0);
                ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase(strClientName, TblFontBold), 570, 570, 0);
            }
            else
            {
                string strClientName = string.Empty;
                if (chkPrintName.Checked)
                {
                    strClientName = /*"Client Name :" +*/ propAppName.Trim();
                }
                ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase("App#: " + propAppNo.Trim(), TblFontBold), 410, 570, 0);
                ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase(strClientName, TblFontBold), 570, 570, 0);
            }

            PdfPCell Date = new PdfPCell(new Phrase("", TblFontBold));
            Date.Colspan = 4;
            Date.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
            Date.Border = iTextSharp.text.Rectangle.NO_BORDER;
            APP_details.AddCell(Date);

            /*//border trails
            PdfContentByte content = writer.DirectContent;
            iTextSharp.text.Rectangle rectangle = new iTextSharp.text.Rectangle(document.PageSize);
            rectangle.Left += document.LeftMargin;
            rectangle.Right -= document.RightMargin;
            rectangle.Top -= document.TopMargin;
            rectangle.Bottom += document.BottomMargin;
            content.SetColorStroke(BaseColor.BLACK);
            content.Rectangle(rectangle.Left, rectangle.Bottom, rectangle.Width, rectangle.Height);
            content.Stroke();*/

            string Screen_Desc = null, Priv_ScreenDesc = null;

            PdfPCell Screen = new PdfPCell(new Phrase("Screen Name", TblFontBold));
            Screen.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            Screen.Border = iTextSharp.text.Rectangle.TOP_BORDER;
            APP_details.AddCell(Screen);

            PdfPCell Description = new PdfPCell(new Phrase("Description", TblFontBold));
            Description.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            Description.Border = iTextSharp.text.Rectangle.TOP_BORDER;
            APP_details.AddCell(Description);

            PdfPCell DateHeader = new PdfPCell(new Phrase("Date", TblFontBold));
            DateHeader.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            DateHeader.Border = iTextSharp.text.Rectangle.TOP_BORDER;
            APP_details.AddCell(DateHeader);

            PdfPCell UserHeader = new PdfPCell(new Phrase("User", TblFontBold));
            UserHeader.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            UserHeader.Border = iTextSharp.text.Rectangle.TOP_BORDER;
            APP_details.AddCell(UserHeader);

            PdfPCell HSpace = new PdfPCell(new Phrase(" ", Times));
            HSpace.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            HSpace.Colspan = 4;
            HSpace.Border = iTextSharp.text.Rectangle.NO_BORDER;
            APP_details.AddCell(HSpace);

            foreach (DataGridViewRow dr in dataGridCaseNotes.Rows)
            {
                if (dr.Cells["categorychk"].Value.ToString() == true.ToString())
                {
                    EMPLFUNCEntity row = dr.Tag as EMPLFUNCEntity;

                    string CaseNotes_Desc = row.CaseNotesData.Trim();
                    Screen_Desc = row.ScrDesc.Trim();

                    PdfPCell Screen_desc = new PdfPCell(new Phrase(Screen_Desc, Times));
                    Screen_desc.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    Screen_desc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    APP_details.AddCell(Screen_desc);

                    if (Screen_Desc == "Contact")
                    {
                        string _strContName = "";
                        if (chkPrintName.Checked)
                            _strContName = row.CaseNotesScreenDesc.Trim();
                        else
                        {
                            string strShortName = GetShortName(row.CaseNotesScreenDesc.Trim().ToUpper());

                            if (BaseForm.BaseApplicationName.ToUpper() == row.CaseNotesScreenDesc.Trim().ToUpper())
                                _strContName = "Applicant " + strShortName;
                            else
                                _strContName = "Member " + strShortName;
                        }

                        PdfPCell Desc = new PdfPCell(new Phrase(_strContName, Times));
                        Desc.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        Desc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        APP_details.AddCell(Desc);
                    }
                    else
                    {
                        PdfPCell Desc = new PdfPCell(new Phrase(row.CaseNotesScreenDesc.Trim(), Times));
                        Desc.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        Desc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        APP_details.AddCell(Desc);
                    }

                    PdfPCell DateV = new PdfPCell(new Phrase(LookupDataAccess.Getdate(row.CaseNotesDate), Times));
                    DateV.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    DateV.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    APP_details.AddCell(DateV);

                    PdfPCell User = new PdfPCell(new Phrase(BaseForm.UserID, Times));
                    User.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    User.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    APP_details.AddCell(User);

                    PdfPCell spce = new PdfPCell(new Phrase("  ", Times));
                    spce.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    spce.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    spce.Colspan = 4;
                    APP_details.AddCell(spce);

                    PdfPCell Notes = new PdfPCell(new Phrase(CaseNotes_Desc, Times));
                    Notes.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    Notes.Colspan = 4;
                    Notes.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    APP_details.AddCell(Notes);

                    PdfPCell Space = new PdfPCell(new Phrase(" ", Times));
                    Space.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    Space.Colspan = 4;
                    Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    APP_details.AddCell(Space);

                    Space = new PdfPCell(new Phrase(" ", Times));
                    Space.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    Space.Colspan = 4;
                    Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    APP_details.AddCell(Space);

                }

            }
            document.Add(APP_details);
            document.Close();
            fs.Close();
            fs.Dispose();
        }
        #endregion

        public static string GetShortName(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            string[] words = input.Split(' ');
            if (words.Length < 2)
            {
                string strShrtname = words[0].Substring(0, 2);
                return strShrtname;
            };


            string firstName = words[0];
            string lastName = words[words.Length - 1]; // Get the last word

            string shortName = string.Concat(firstName[0], lastName[0]);


            return shortName.ToUpper();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //Help.ShowHelp(this, Application.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "casenotes");
        }

        //End Report Section...................



        private void sizecombo()
        {
            cmbsize.Items.Add(new Captain.Common.Utilities.ListItem("10", "10"));
            //cmbsize.Items.Add(new Captain.Common.Utilities.ListItem("11", "11"));
            cmbsize.Items.Add(new Captain.Common.Utilities.ListItem("12", "12"));
            //  cmbsize.Items.Add(new Captain.Common.Utilities.ListItem("13", "13"));
            cmbsize.Items.Add(new Captain.Common.Utilities.ListItem("14", "14"));
            //   cmbsize.Items.Add(new Captain.Common.Utilities.ListItem("15", "15"));
            cmbsize.Items.Add(new Captain.Common.Utilities.ListItem("16", "16"));
            //  cmbsize.Items.Add(new Captain.Common.Utilities.ListItem("17", "17"));
            cmbsize.Items.Add(new Captain.Common.Utilities.ListItem("18", "18"));
            //  cmbsize.Items.Add(new Captain.Common.Utilities.ListItem("19", "19"));
            cmbsize.Items.Add(new Captain.Common.Utilities.ListItem("20", "20"));
            cmbsize.SelectedIndex = 0;
        }



    }
}