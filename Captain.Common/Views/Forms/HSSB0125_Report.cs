using Captain.Common.Interfaces;
using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;
using Captain.Common.Views.Forms.Base;
using DevExpress.DashboardWin.Native;
using iTextSharp.text.pdf;
using iTextSharp.text;
using NPOI.OpenXmlFormats.Spreadsheet;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using Wisej.Web;
using static DevExpress.Xpo.DB.DataStoreLongrunnersWatch;
using System.IO;
using System.Text.RegularExpressions;
using DevExpress.Utils.Extensions;
using System.Linq;

namespace Captain.Common.Views.Forms
{
    public partial class HSSB0125_Report : Form
    {

        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;

        #endregion

        public HSSB0125_Report(BaseForm baseform, PrivilegeEntity privilegeEntity)
        {
            InitializeComponent();

            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            _baseform = baseform;
            _priviliges = privilegeEntity;

            propReportPath = _model.lookupDataAccess.GetReportPath();
            Set_Report_Hierarchy(_baseform.BaseAgency, _baseform.BaseDept, _baseform.BaseProg, _baseform.BaseYear);
            Agency = _baseform.BaseAgency;
            Dept = _baseform.BaseDept;
            Prog = _baseform.BaseProg;
            Program_Year = _baseform.BaseYear;

            STAFFMSTEntity Search_STAFFMST = new STAFFMSTEntity(true);
            Search_STAFFMST.Agency = _baseform.BaseAgency;
            Search_STAFFMST.Year = _baseform.BaseYear;

            if (!_baseform.UserProfile.StaffCode.Contains("**"))
                Search_STAFFMST.Staff_Code = _baseform.UserProfile.StaffCode;
            //Search_STAFFMST.Active = "A";
            STAFFMST_List = _model.STAFFData.Browse_STAFFMST(Search_STAFFMST, "Browse");

            staffTrainEntity = _model.STAFFData.GetSTFTRAIN(_baseform.BaseAgency, _baseform.BaseDept, _baseform.BaseProg, _baseform.BaseYear, "");

            SPDBPostEntity = _model.STAFFData.GetStaffBulkPost(_baseform.BaseAgency, _baseform.BaseDept, _baseform.BaseProg, _baseform.BaseYear, "");

            Topic = _model.lookupDataAccess.GetLookkupFronAGYTAB("00450");
            Location = _model.lookupDataAccess.GetLookkupFronAGYTAB("00452");
            Format = _model.lookupDataAccess.GetLookkupFronAGYTAB("00453");
            CredType = _model.lookupDataAccess.GetLookkupFronAGYTAB("00454");

            ServAreaListBox.Height = 200;

            ServiceArea = CommonFunctions.AgyTabsFilterCode(_baseform.BaseAgyTabsEntity, "S0220", _baseform.BaseAgency, _baseform.BaseDept, _baseform.BaseProg, string.Empty);

            foreach (CommonEntity entity in ServiceArea)
            {
                this.ServAreaListBox.Items.Add(new Captain.Common.Utilities.ListItem(entity.Desc, entity.Code));
            }

            //string[] ServAreaValues = staffTrainEntity[0].Serv_Area.Split(',');

            //foreach (string SerArea in ServAreaValues)
            //{
            //    for (int x = 0; x < ServAreaListBox.Items.Count; x++)
            //    {
            //        if (((Captain.Common.Utilities.ListItem)ServAreaListBox.Items[x]).Value.ToString() == SerArea)
            //            ServAreaListBox.SetItemChecked(x, true);
            //    }
            //}
        }

        #region Properties

        public BaseForm _baseform
        {
            get;
            set;
        }
        public PrivilegeEntity _priviliges
        {
            get;
            set;
        }
        public string Agency
        {
            get; set;
        }
        public string Dept
        {
            get; set;
        }
        public string Prog
        {
            get; set;
        }
        public string propReportPath
        {
            get; set;
        }
        public List<STAFFMSTEntity> STAFFMST_List
        {
            get; set;
        }
        public List<STAFFBULKPOSTEntity> SPDBPostEntity
        {
            get; set;
        }
        public List<STAFFTRAINEntity> staffTrainEntity
        {
            get; set;
        }

        #endregion

        List<CommonEntity> Topic = new List<CommonEntity>();
        List<CommonEntity> CredType = new List<CommonEntity>();
        List<CommonEntity> Format = new List<CommonEntity>();
        List<CommonEntity> Location = new List<CommonEntity>();
        List<CommonEntity> Level = new List<CommonEntity>();
        List<CommonEntity> ServiceArea = new List<CommonEntity>();

        #region Hierarchy Code

        string Program_Year;
        private void Pb_Search_Hie_Click(object sender, EventArgs e)
        {
            HierarchieSelection hierarchieSelectionForm = new HierarchieSelection(_baseform, Current_Hierarchy_DB, "Master", "A", "D", "Reports", _baseform.UserID);
            hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
            hierarchieSelectionForm.StartPosition = FormStartPosition.CenterScreen;
            hierarchieSelectionForm.ShowDialog();
        }

        string strYear = string.Empty;
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
                    Set_Report_Hierarchy(hierarchy.Substring(0, 2), hierarchy.Substring(2, 2), hierarchy.Substring(4, 2), string.Empty);
                    Agency = hierarchy.Substring(0, 2);
                    Dept = hierarchy.Substring(2, 2);
                    Prog = hierarchy.Substring(4, 2);
                }
            }
        }

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
                this.Txt_HieDesc.Size = new System.Drawing.Size(675, 25);
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
                this.Txt_HieDesc.Size = new System.Drawing.Size(595, 25);
            else
                this.Txt_HieDesc.Size = new System.Drawing.Size(675, 25);
        }

        private void CmbYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            //DataSet dsAwards = DatabaseLayer.FuelControlDB.Browse_FuelCntl(string.Empty);
            //DataTable dtAwards = new DataTable();
            //dtAwards = dsAwards.Tables[0];

            //DataRow[] dr = dtAwards.Select("FCNTL_YEAR = '" + ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString() + "' AND FCNTL_Award = 'B'");
            //DataTable dtnew = new DataTable();

            //if (rdbAnnual.Checked)
            //{
            //    if (dr.Length > 0)
            //    {
            //        dtnew = dr.CopyToDataTable();

            //        if (dtnew.Rows[0]["FCNTL_SDate"].ToString() != "")
            //            dtpFrom.Text = Convert.ToDateTime(dtnew.Rows[0]["FCNTL_SDate"]).ToString().Trim();
            //        else
            //            dtpFrom.Text = DateTime.Now.ToString();

            //        DateTime dateFrom = DateTime.Parse(dtpFrom.Text);

            //        dtpTo.Text = dateFrom.AddMonths(12).AddDays(-1).ToString();
            //    }
            //}
            //else
            //{
            //    if (dr.Length > 0)
            //    {
            //        dtnew = dr.CopyToDataTable();

            //        if (dtnew.Rows[0]["FCNTL_SDate"].ToString() != "")
            //        {
            //            dtpProgStartDte.Text = Convert.ToDateTime(dtnew.Rows[0]["FCNTL_SDate"]).ToString().Trim();
            //        }
            //        else
            //        {
            //            dtpProgStartDte.Text = DateTime.Now.ToString();
            //        }

            //        dtpQ1F.Text = dtpProgStartDte.Text;

            //        DateTime dateQ1 = DateTime.Parse(dtpQ1F.Text);

            //        dtpQ1T.Text = dateQ1.AddMonths(3).AddDays(-1).ToString();
            //        dtpQ2F.Text = dateQ1.AddMonths(3).ToString();
            //        dtpQ2T.Text = dateQ1.AddMonths(6).AddDays(-1).ToString();
            //        dtpQ3F.Text = dateQ1.AddMonths(6).ToString();
            //        dtpQ3T.Text = dateQ1.AddMonths(9).AddDays(-1).ToString();
            //        dtpQ4F.Text = dateQ1.AddMonths(9).ToString();
            //        dtpQ4T.Text = dateQ1.AddMonths(12).AddDays(-1).ToString();
            //    }
            //}
        }

        #endregion

        #region Save and Get Parameters

        private void btnGetParameters_Click(object sender, EventArgs e)
        {
            ControlCard_Entity Save_Entity = new ControlCard_Entity(true);
            Save_Entity.Scr_Code = _priviliges.Program;
            Save_Entity.UserID = _baseform.UserID;
            Save_Entity.Module = _baseform.BusinessModuleID;
            Report_Get_SaveParams_Form Save_Form = new Report_Get_SaveParams_Form(Save_Entity, "Get");
            Save_Form.FormClosed += new FormClosedEventHandler(Get_Saved_Parameters);
            Save_Form.StartPosition = FormStartPosition.CenterScreen;
            Save_Form.ShowDialog();
        }
        private void btnSaveParameters_Click(object sender, EventArgs e)
        {
            if (ValidateReport())
            {

                ControlCard_Entity Save_Entity = new ControlCard_Entity(true);
                Save_Entity.Scr_Code = _priviliges.Program;
                Save_Entity.UserID = _baseform.UserID;
                Save_Entity.Card_1 = Get_XML_Format_for_Report_Controls();
                Save_Entity.Card_2 = string.Empty;
                Save_Entity.Card_3 = string.Empty;
                Save_Entity.Module = _baseform.BusinessModuleID;

                Report_Get_SaveParams_Form Save_Form = new Report_Get_SaveParams_Form(Save_Entity, "Save", _baseform, _priviliges);
                Save_Form.StartPosition = FormStartPosition.CenterScreen;
                Save_Form.ShowDialog();
            }
        }

        private bool ValidateReport()
        {
            bool isValid = true;

            if (dtpFromDte.Checked == false)
            {
                _errorProvider.SetError(dtpFromDte, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblFromDte.Text.Trim().Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtpFromDte, null);
            }
            if (dtpToDte.Checked == false)
            {
                _errorProvider.SetError(dtpToDte, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblToDate.Text.Trim().Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtpToDte, null);
            }

            if (dtpFromDte.Checked == true || dtpToDte.Checked == true)
            {
                if (dtpFromDte.Checked == false && dtpToDte.Checked == true)
                {
                    _errorProvider.SetError(dtpFromDte, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblFromDte.Text.Trim().Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtpFromDte, null);
                }
            }

            if (dtpFromDte.Checked == true || dtpToDte.Checked == true)
            {
                if (dtpToDte.Checked == false && dtpFromDte.Checked == true)
                {
                    _errorProvider.SetError(dtpToDte, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblToDate.Text.Trim().Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtpToDte, null);
                }
            }
            if (dtpFromDte.Checked.Equals(true) && dtpToDte.Checked.Equals(true))
            {
                if (string.IsNullOrWhiteSpace(dtpFromDte.Text))
                {
                    _errorProvider.SetError(dtpFromDte, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblFromDte.Text.Trim().Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtpFromDte, null);
                }
                if (string.IsNullOrWhiteSpace(dtpToDte.Text))
                {
                    _errorProvider.SetError(dtpToDte, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblToDate.Text.Trim().Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtpToDte, null);
                }
            }

            if (dtpFromDte.Checked && dtpToDte.Checked == true)
            {
                if (!string.IsNullOrWhiteSpace(dtpFromDte.Text) && !string.IsNullOrWhiteSpace(dtpToDte.Text))
                {
                    if (dtpFromDte.Value.Date > dtpToDte.Value.Date)
                    {
                        _errorProvider.SetError(dtpToDte, "'To Date' should be equal to or greater than 'From Date'");
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(dtpToDte, null);
                    }
                }
            }

            return isValid;
        }

        private string Get_XML_Format_for_Report_Controls()
        {

            string FromDate = string.Empty, ToDate = string.Empty, Name = string.Empty, Topic = string.Empty, ServArea = string.Empty, Format = string.Empty, Location = string.Empty, CredType = string.Empty;

            FromDate = dtpFromDte.Text;
            ToDate = dtpToDte.Text;

            if (rdbAllTNames.Checked)
                Name = "A";
            else
                Name = "S";

            if (rdbAllTopic.Checked)
                Topic = "A";
            else
                Topic = "S";







            if (rdbAllFormat.Checked)
                Format = "A";
            else
                Format = "S";

            if (rdbAllLocation.Checked)
                Location = "A";
            else
                Location = "S";

            if (rdbAllCredType.Checked)
                CredType = "A";
            else
                CredType = "S";


            StringBuilder str = new StringBuilder();
            str.Append("<Rows>");
            str.Append("<Row AGENCY = \"" + Agency + "\" DEPT = \"" + Dept + "\" PROG = \"" + Prog +
                            "\" YEAR = \"" + ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString() + "\" FROMDATE = \"" + FromDate +
                            "\" TODATE = \"" + ToDate + "\" NAME = \"" + Name + "\" TOPIC = \"" + Topic + "\" SERVAREA = \"" + ServArea + "\" FORMAT = \"" + Format + "\" LOCATION = \"" + Location + "\" CREDTYPE = \"" + CredType + "\" />");

            str.Append("</Rows>");

            return str.ToString();
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
        private void Set_Report_Controls(DataTable Tmp_Table)
        {
            if (Tmp_Table != null && Tmp_Table.Rows.Count > 0)
            {
                DataRow dr = Tmp_Table.Rows[0];

                Set_Report_Hierarchy(dr["AGENCY"].ToString(), dr["DEPT"].ToString(), dr["PROG"].ToString(), dr["YEAR"].ToString());

                dtpFromDte.Text = dr["FROMDATE"].ToString();
                dtpToDte.Text = dr["TODATE"].ToString();

                if (dr["NAME"].ToString() == "A")
                    rdbAllTNames.Checked = true;
                else if (dr["NAME"].ToString() == "S")
                    rdbSelTNames.Checked = true;

                if (dr["TOPIC"].ToString() == "A")
                    rdbAllTopic.Checked = true;
                else if (dr["TOPIC"].ToString() == "S")
                    rdbSelTopic.Checked = true;





                if (dr["FORMAT"].ToString() == "A")
                    rdbAllFormat.Checked = true;
                else if (dr["FORMAT"].ToString() == "S")
                    rdbSelFormat.Checked = true;

                if (dr["LOCATION"].ToString() == "A")
                    rdbAllLocation.Checked = true;
                else if (dr["LOCATION"].ToString() == "S")
                    rdbSelLocation.Checked = true;

                if (dr["CREDTYPE"].ToString() == "A")
                    rdbAllCredType.Checked = true;
                else if (dr["CREDTYPE"].ToString() == "S")
                    rdbSelCredType.Checked = true;

            }
        }

        #endregion

        private void rdbAllSName_Click(object sender, EventArgs e)
        {
            Sel_Staff = string.Empty;
        }
        private void rdbSelSName_Click(object sender, EventArgs e)
        {
            STFBLK_Search_Sel_Form staffForm = new STFBLK_Search_Sel_Form(_baseform, _priviliges, STAFFMST_List, "Staff", Sel_Staff);
            staffForm.StartPosition = FormStartPosition.CenterScreen;
            staffForm.FormClosed += new FormClosedEventHandler(objform_FormClosed);
            staffForm.ShowDialog();
        }

        private void rdbAllTNames_Click(object sender, EventArgs e)
        {
            Sel_Training = string.Empty;
        }

        private void rdbSelTNames_Click(object sender, EventArgs e)
        {
            STFBLK_Search_Sel_Form trainForm = new STFBLK_Search_Sel_Form(_baseform, _priviliges, staffTrainEntity, "Training", Sel_Training);
            trainForm.StartPosition = FormStartPosition.CenterScreen;
            trainForm.FormClosed += new FormClosedEventHandler(objform_FormClosed);
            trainForm.ShowDialog();
        }
        private void rdbAllTopic_CheckedChanged(object sender, EventArgs e)
        {
            Sel_Topics = string.Empty;
        }

        private void rdbSelTopic_Click(object sender, EventArgs e)
        {
            STFBLK_Search_Sel_Form topicForm = new STFBLK_Search_Sel_Form(_baseform, _priviliges, Topic, "Topic", Sel_Topics);
            topicForm.StartPosition = FormStartPosition.CenterScreen;
            topicForm.FormClosed += new FormClosedEventHandler(objform_FormClosed);
            topicForm.ShowDialog();
        }

        private void rdbAllFormat_Click(object sender, EventArgs e)
        {
            Sel_Formats = string.Empty;
        }

        private void rdbSelFormat_Click(object sender, EventArgs e)
        {
            STFBLK_Search_Sel_Form formatForm = new STFBLK_Search_Sel_Form(_baseform, _priviliges, Format, "Format", Sel_Formats);
            formatForm.StartPosition = FormStartPosition.CenterScreen;
            formatForm.FormClosed += new FormClosedEventHandler(objform_FormClosed);
            formatForm.ShowDialog();
        }

        private void rdbAllLocation_Click(object sender, EventArgs e)
        {
            Sel_Loc = string.Empty;
        }

        private void rdbSelLocation_Click(object sender, EventArgs e)
        {
            STFBLK_Search_Sel_Form locForm = new STFBLK_Search_Sel_Form(_baseform, _priviliges, Location, "Loc", Sel_Loc);
            locForm.StartPosition = FormStartPosition.CenterScreen;
            locForm.FormClosed += new FormClosedEventHandler(objform_FormClosed);
            locForm.ShowDialog();
        }

        private void rdbAllCredType_Click(object sender, EventArgs e)
        {
            Sel_CredType = string.Empty;
        }

        private void rdbSelCredType_Click(object sender, EventArgs e)
        {
            STFBLK_Search_Sel_Form credTypeForm = new STFBLK_Search_Sel_Form(_baseform, _priviliges, CredType, "CredType", Sel_CredType);
            credTypeForm.StartPosition = FormStartPosition.CenterScreen;
            credTypeForm.FormClosed += new FormClosedEventHandler(objform_FormClosed);
            credTypeForm.ShowDialog();
        }

        string Sel_Staff = string.Empty;

        string Sel_Training = string.Empty;

        string Sel_Topics = string.Empty;

        string Sel_Formats = string.Empty;

        string Sel_Loc = string.Empty;

        string Sel_CredType = string.Empty;


        private void objform_FormClosed(object sender, FormClosedEventArgs e)
        {
            STFBLK_Search_Sel_Form form = sender as STFBLK_Search_Sel_Form;
            if (form.DialogResult == DialogResult.OK)
            {
                if (form.formType == "Staff")
                    Sel_Staff = form.fillCodesforReport();
                else if (form.formType == "Training")
                    Sel_Training = form.fillCodesforReport();
                else if (form.formType == "Topic")
                    Sel_Topics = form.fillCodesforReport();
                else if (form.formType == "Format")
                    Sel_Formats = form.fillCodesforReport();
                else if (form.formType == "Loc")
                    Sel_Loc = form.fillCodesforReport();
                else if (form.formType == "CredType")
                    Sel_CredType = form.fillCodesforReport();
            }
            else
                form.Close();
        }

        string ServAreaValues = ""; string pdfServText = string.Empty; string pdfAreaPrint = string.Empty;
        private void ServAreaListBox_AfterItemCheck(object sender, ItemCheckEventArgs e)
        {
            string ServAreaText = "";
            ServAreaValues = "";
            pdfAreaPrint = "";
            foreach (Captain.Common.Utilities.ListItem checkedItem in ServAreaListBox.CheckedItems)
            {
                ServAreaValues += $"{checkedItem.Value},";
                ServAreaText += $"{checkedItem.Text},";

                pdfAreaPrint +=  $"{checkedItem.Value}_{checkedItem.Text}:";
            }
            pdfServText = this.cmbUserServArea.Text = ServAreaText.Trim().TrimEnd(',');
        }

        private void btnGeneratePdf_Click(object sender, EventArgs e)
        {
            PdfListForm pdfListForm = new PdfListForm(_baseform, _priviliges, false, propReportPath, "PDF");
            pdfListForm.FormClosed += new FormClosedEventHandler(On_Form_Closed);
            pdfListForm.StartPosition = FormStartPosition.CenterScreen;
            pdfListForm.ShowDialog();
        }

        string Random_Filename = null; string PdfName = null; PdfContentByte cb;
        private void On_Form_Closed(object sender, FormClosedEventArgs e)
        {
            PdfListForm form = sender as PdfListForm;

            if (form.DialogResult == DialogResult.OK)
            {
                #region FileNameBuild
                /*************************************************************/
                Random_Filename = null;
                string xlFileName = form.GetFileName();
                xlFileName = propReportPath + _baseform.UserID + "\\" + xlFileName;
                try
                {
                    if (!Directory.Exists(propReportPath + _baseform.UserID.Trim()))
                    {
                        DirectoryInfo di = Directory.CreateDirectory(propReportPath + _baseform.UserID.Trim());
                    }
                }
                catch (Exception ex)
                {
                    CommonFunctions.MessageBoxDisplay("Error");
                }

                try
                {
                    string Tmpstr = xlFileName + ".pdf";
                    if (File.Exists(Tmpstr))
                        File.Delete(Tmpstr);
                }
                catch (Exception ex)
                {
                    int length = 8;
                    string newFileName = System.Guid.NewGuid().ToString();
                    newFileName = newFileName.Replace("-", string.Empty);

                    Random_Filename = xlFileName + newFileName.Substring(0, length) + ".pdf";
                }

                if (!string.IsNullOrEmpty(Random_Filename))
                    xlFileName = Random_Filename;
                else
                    xlFileName += ".pdf";

                /*************************************************************/
                string _excelPath = xlFileName;
                #endregion

                #region Font styles

                BaseFont bfTimes = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
                BaseFont bf_Times = BaseFont.CreateFont("c:/windows/fonts/TIMESBD.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
                iTextSharp.text.Font fc = new iTextSharp.text.Font(bfTimes, 10, 2);
                iTextSharp.text.Font fc1 = new iTextSharp.text.Font(bf_Times, 10, 2, BaseColor.BLUE);
                iTextSharp.text.Font fcGray = new iTextSharp.text.Font(bf_Times, 10, 2, BaseColor.DARK_GRAY);

                iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_Times, 10);

                BaseFont bf_calibri = BaseFont.CreateFont("c:/windows/fonts/Calibri.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
                iTextSharp.text.Font HeaderTextfnt = new iTextSharp.text.Font(bf_calibri, 12, 1, BaseColor.WHITE);
                iTextSharp.text.Font SubHeadTextfnt = new iTextSharp.text.Font(bf_calibri, 10, 1, new BaseColor(26, 71, 119));
                iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_calibri, 8);
                iTextSharp.text.Font TableFonterr = new iTextSharp.text.Font(bf_calibri, 8, 0, BaseColor.RED);
                iTextSharp.text.Font TableFontWhite = new iTextSharp.text.Font(bf_calibri, 8, 2, BaseColor.WHITE);
                iTextSharp.text.Font BoldFont = new iTextSharp.text.Font(bf_calibri, 8, 1);
                iTextSharp.text.Font BoldUnderlineFont = new iTextSharp.text.Font(bf_calibri, 8, 3);
                iTextSharp.text.Font TableFontPrgNotes = new iTextSharp.text.Font(bf_calibri, 8, 0, new BaseColor(111, 48, 160));
                iTextSharp.text.Font TableFontPrgNotesBold = new iTextSharp.text.Font(bf_calibri, 8, 1, new BaseColor(111, 48, 160));
                iTextSharp.text.Font TableFontPrgNotesBoldUndline = new iTextSharp.text.Font(bf_calibri, 8, 5, new BaseColor(111, 48, 160));
                iTextSharp.text.Font fcRed = new iTextSharp.text.Font(bf_calibri, 10, 1, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#806000")));
                iTextSharp.text.Font FTblFontBold = new iTextSharp.text.Font(bfTimes, 7, 1);
                iTextSharp.text.Font FTableFont = new iTextSharp.text.Font(bf_calibri, 8);

                #endregion

                #region Cell Color Define

                BaseColor DarkBlue = new BaseColor(26, 71, 119);
                BaseColor SecondBlue = new BaseColor(184, 217, 255);
                BaseColor TblHeaderBlue = new BaseColor(155, 194, 230);

                #endregion

                FileStream fs = new FileStream(xlFileName, FileMode.Create);
                Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                document.Open();
                cb = writer.DirectContent;

                try
                {
                    PrintHeaderPage(document, writer);

                    document.NewPage();



                    PdfPTable table = new PdfPTable(8);
                    table.TotalWidth = 550f;
                    table.WidthPercentage = 100;
                    table.LockedWidth = true;
                    float[] widths = new float[] { 2f, 60f, 40f, 40f, 100f, 100f, 40f, 2f };
                    table.SetWidths(widths);
                    table.HorizontalAlignment = Element.ALIGN_CENTER;

                    PdfPCell Cell = new PdfPCell(new Phrase(" ", TableFont));
                    Cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Cell.BackgroundColor = DarkBlue;
                    table.AddCell(Cell);

                    PdfPCell Cell1 = new PdfPCell(new Phrase(" Individual Traing (Grid) Report", HeaderTextfnt));
                    Cell1.HorizontalAlignment = Element.ALIGN_LEFT;
                    Cell1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Cell1.BackgroundColor = DarkBlue;
                    Cell1.Colspan = 6;
                    table.AddCell(Cell1);

                    PdfPCell Cell2 = new PdfPCell(new Phrase(" ", TableFont));
                    Cell2.HorizontalAlignment = Element.ALIGN_LEFT;
                    Cell2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Cell2.BackgroundColor = DarkBlue;
                    table.AddCell(Cell2);

                    string[] HeaderSeq = { "", "Name", "Initial Hire", "Event Date", "Training Name", "Service Area", "Hours", "" };
                    for (int i = 0; i < HeaderSeq.Length; ++i)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(HeaderSeq[i], SubHeadTextfnt));
                        if (i == 6)
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        else
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        if (i == 0 || i == 7)
                            cell.BackgroundColor = DarkBlue;
                        else
                            cell.BackgroundColor = SecondBlue;
                        table.AddCell(cell);
                    }

                    List<STAFFBULKPOSTEntity> filterSPDB = new List<STAFFBULKPOSTEntity>();

                    //Staff Member Filter
                    if (rdbSelSName.Checked)
                        filterSPDB = SPDBPostEntity.FindAll(x => Sel_Staff.Contains(x.Staff_Code));
                    else
                        filterSPDB = SPDBPostEntity;

                    //Training Name Filter
                    if (rdbSelTNames.Checked)
                        filterSPDB = filterSPDB.FindAll(X => Sel_Training.Contains(X.Train_ID));
                    else
                        filterSPDB = filterSPDB;

                    //Topic Filter
                    if (rdbSelTopic.Checked)
                        filterSPDB = filterSPDB.FindAll(X => Sel_Topics.Contains(X.Topic));
                    else
                        filterSPDB = filterSPDB;

                    //Service Area Filter
                    if (!string.IsNullOrEmpty(ServAreaValues))
                        filterSPDB = filterSPDB.FindAll(X => (ServAreaValues.Trim().TrimEnd(',').Contains(X.Area)) && !string.IsNullOrEmpty(X.Area));
                    else
                        filterSPDB = filterSPDB;

                    //Format Filter
                    if (rdbSelFormat.Checked)
                        filterSPDB = filterSPDB.FindAll(X => Sel_Formats.Contains(X.Format));
                    else
                        filterSPDB = filterSPDB;

                    //Location Filter
                    if (rdbSelLocation.Checked)
                        filterSPDB = filterSPDB.FindAll(X => Sel_Loc.Contains(X.Loc));
                    else
                        filterSPDB = filterSPDB;

                    //Credit Type Filter
                    if (rdbSelCredType.Checked)
                        filterSPDB = filterSPDB.FindAll(X => Sel_CredType.Contains(X.Cred_type));
                    else
                        filterSPDB = filterSPDB;

                    decimal HrsTotal = 0;

                    foreach (STAFFBULKPOSTEntity Entity in filterSPDB)
                    {
                        if (Convert.ToDateTime(dtpFromDte.Text) <= Convert.ToDateTime(Entity.Comp_Date) && Convert.ToDateTime(Entity.Comp_Date) <= Convert.ToDateTime(dtpToDte.Text))
                        {
                            string staffFName = STAFFMST_List.Find(x => x.Staff_Code == Entity.Staff_Code).First_Name;
                            string staffLName = STAFFMST_List.Find(x => x.Staff_Code == Entity.Staff_Code).Last_Name;

                            string hireDate = STAFFMST_List.Find(x => x.Staff_Code == Entity.Staff_Code).Date_Hired;

                            string trainName = staffTrainEntity.Find(x => x.ID == Entity.Train_ID).Name;

                            Cell = new PdfPCell(new Phrase("", TableFont));
                            Cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            Cell.BackgroundColor = DarkBlue;
                            table.AddCell(Cell);

                            PdfPCell StaffName = new PdfPCell(new Phrase(staffFName + " " + staffLName, TableFont));
                            StaffName.HorizontalAlignment = Element.ALIGN_LEFT;
                            StaffName.Border = iTextSharp.text.Rectangle.BOX;
                            table.AddCell(StaffName);

                            PdfPCell dateHired = new PdfPCell(new Phrase(hireDate == "" ? "" : LookupDataAccess.Getdate(hireDate), TableFont));
                            dateHired.HorizontalAlignment = Element.ALIGN_LEFT;
                            dateHired.Border = iTextSharp.text.Rectangle.BOX;
                            table.AddCell(dateHired);

                            PdfPCell eventDate = new PdfPCell(new Phrase(Entity.Comp_Date == "" ? "" : LookupDataAccess.Getdate(Entity.Comp_Date), TableFont));
                            eventDate.HorizontalAlignment = Element.ALIGN_LEFT;
                            eventDate.Border = iTextSharp.text.Rectangle.BOX;
                            table.AddCell(eventDate);

                            PdfPCell trainingName = new PdfPCell(new Phrase(trainName, TableFont));
                            trainingName.HorizontalAlignment = Element.ALIGN_LEFT;
                            trainingName.Border = iTextSharp.text.Rectangle.BOX;
                            table.AddCell(trainingName);

                            string AreaText = string.Empty;
                            List<CommonEntity> multiAreaText = null;
                            if (!string.IsNullOrEmpty(ServAreaValues))
                            {
                                if (ServAreaValues.Length > 2)
                                {
                                    if (!string.IsNullOrEmpty(Entity.Area))
                                    {
                                        multiAreaText = ServiceArea.FindAll(x => Entity.Area.Contains(x.Code)).ToList();

                                        foreach (CommonEntity entity in multiAreaText)
                                        {
                                            AreaText += entity.Desc.Trim() + ", ";
                                        }
                                    }
                                }
                                else
                                {
                                    string[] strAreaValues = ServAreaValues.Trim().TrimEnd(',').Split(',');
                                    string[] strDesc = pdfAreaPrint.Trim().TrimEnd(':').Split(':');

                                    string strDesc1 = strDesc[0];//.Trim().Split('_').ToString();


                                    if (!string.IsNullOrEmpty(Entity.Area))
                                    {
                                        foreach (string strValues in strAreaValues)
                                        {
                                            if (Entity.Area == strValues)
                                            {
                                                foreach (string desc in strDesc)
                                                {
                                                    if (desc.Contains(Entity.Area))
                                                    {
                                                        int index = desc.IndexOf('_');
                                                        if (index >= 0 && index < desc.Length - 1)
                                                        {
                                                            string result = desc.Substring(index + 1);
                                                            AreaText = result;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(Entity.Area))
                                {
                                    multiAreaText = ServiceArea.FindAll(x => Entity.Area.Contains(x.Code)).ToList();

                                    foreach (CommonEntity entity in multiAreaText)
                                    {
                                        AreaText += entity.Desc.Trim() + ", ";
                                    }
                                }
                            }

                            AreaText = AreaText.Trim().TrimEnd(',');
                            PdfPCell servArea = new PdfPCell(new Phrase(AreaText, TableFont));
                            servArea.HorizontalAlignment = Element.ALIGN_LEFT;
                            servArea.Border = iTextSharp.text.Rectangle.BOX;
                            table.AddCell(servArea);

                            PdfPCell hours = new PdfPCell(new Phrase(Entity.Hrs, TableFont));
                            hours.HorizontalAlignment = Element.ALIGN_CENTER;
                            hours.Border = iTextSharp.text.Rectangle.BOX;
                            table.AddCell(hours);

                            if (!string.IsNullOrEmpty(Entity.Hrs))
                            {
                                HrsTotal += Convert.ToDecimal(Entity.Hrs);
                            }

                            Cell = new PdfPCell(new Phrase("", TableFont));
                            Cell.HorizontalAlignment = Element.ALIGN_LEFT;
                            Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            Cell.BackgroundColor = DarkBlue;
                            table.AddCell(Cell);
                        }
                    }

                    //Hours Total
                    Cell = new PdfPCell(new Phrase(" ", TableFont));
                    Cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Cell.BackgroundColor = DarkBlue;
                    table.AddCell(Cell);

                    PdfPCell Total = new PdfPCell(new Phrase("Total Hours", TblFontBold));
                    Total.HorizontalAlignment = Element.ALIGN_RIGHT;
                    Total.Border = iTextSharp.text.Rectangle.BOX;
                    Total.BackgroundColor = SecondBlue;
                    Total.Colspan = 5;
                    table.AddCell(Total);

                    Total = new PdfPCell(new Phrase(HrsTotal.ToString(), TblFontBold));
                    Total.HorizontalAlignment = Element.ALIGN_CENTER;
                    Total.Border = iTextSharp.text.Rectangle.BOX;
                    Total.BackgroundColor = SecondBlue;
                    table.AddCell(Total);

                    Cell = new PdfPCell(new Phrase(" ", TableFont));
                    Cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Cell.BackgroundColor = DarkBlue;
                    table.AddCell(Cell);

                    PdfPCell CelllL = new PdfPCell(new Phrase(" ", TableFont));
                    CelllL.HorizontalAlignment = Element.ALIGN_LEFT;
                    CelllL.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    CelllL.BackgroundColor = DarkBlue;
                    table.AddCell(CelllL);

                    PdfPCell CellML = new PdfPCell(new Phrase("©2024 CAPSystems Inc", TableFontWhite));
                    CellML.HorizontalAlignment = Element.ALIGN_RIGHT;
                    CellML.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    CellML.BackgroundColor = DarkBlue;
                    CellML.Colspan = 6;
                    table.AddCell(CellML);

                    PdfPCell CellRL = new PdfPCell(new Phrase(" ", TableFont));
                    CellRL.HorizontalAlignment = Element.ALIGN_LEFT;
                    CellRL.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    CellRL.BackgroundColor = DarkBlue;
                    table.AddCell(CellRL);

                    PdfPCell cellSpace = new PdfPCell(new Phrase("", TableFont));
                    cellSpace.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellSpace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    cellSpace.FixedHeight = 20f;
                    cellSpace.Colspan = 8;
                    table.AddCell(cellSpace);

                    document.Add(table);

                    document.NewPage();

                    decimal ServAreaHrs = 0;
                    string ServAreaCode = string.Empty;

                    PdfPTable areaTable = new PdfPTable(2);
                    areaTable.TotalWidth = 550f;
                    areaTable.WidthPercentage = 100;
                    areaTable.LockedWidth = true;
                    float[] areawidths = new float[] { 200f, 40f };
                    areaTable.SetWidths(areawidths);
                    areaTable.HorizontalAlignment = Element.ALIGN_CENTER;

                    string[] AreaSeq = { "Service Area", "Hours" };

                    for (int i = 0; i < AreaSeq.Length; ++i)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(AreaSeq[i], SubHeadTextfnt));
                        if (i == 0)
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        else
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        cell.BackgroundColor = SecondBlue;
                        areaTable.AddCell(cell);
                    }

                    List<STAFFBULKPOSTEntity> databtwdtes = filterSPDB.FindAll(x => (Convert.ToDateTime(x.Comp_Date) >= Convert.ToDateTime(dtpFromDte.Text)) && (Convert.ToDateTime(x.Comp_Date) <= Convert.ToDateTime(dtpToDte.Text))).ToList();

                    foreach (CommonEntity entity in ServiceArea)
                    {
                        ServAreaHrs = 0;
                        PdfPCell servArea = new PdfPCell(new Phrase(entity.Desc, TableFont));
                        servArea.HorizontalAlignment = Element.ALIGN_LEFT;
                        servArea.Border = iTextSharp.text.Rectangle.BOX;
                        areaTable.AddCell(servArea);

                        List<STAFFBULKPOSTEntity> hrstotal = databtwdtes.FindAll(x => x.Area.Contains(entity.Code)).ToList();

                        if (hrstotal.Count > 0)
                        {
                            foreach (STAFFBULKPOSTEntity hrs in hrstotal)
                            {
                                if (hrs.Area.Contains(entity.Code))
                                    ServAreaHrs = ServAreaHrs + (hrs.Hrs == "" ? 0 : Convert.ToDecimal(hrs.Hrs));
                                else
                                    ServAreaHrs = 0;
                            }
                        }

                        PdfPCell hours = new PdfPCell(new Phrase(ServAreaHrs.ToString(), TableFont));
                        hours.HorizontalAlignment = Element.ALIGN_CENTER;
                        hours.Border = iTextSharp.text.Rectangle.BOX;
                        areaTable.AddCell(hours);

                    }

                    document.Add(areaTable);
                }
                catch (Exception ex) { document.Add(new Paragraph("Aborted due to Exception!!!")); }

                AlertBox.Show("Report Generated Successfully");
                document.Close();
                fs.Close();
                fs.Dispose();

                PdfViewerNewForm objfrm = new PdfViewerNewForm(xlFileName);
                objfrm.StartPosition = FormStartPosition.CenterScreen;
                objfrm.ShowDialog();
            }
        }

        private void PrintHeaderPage(Document document, PdfWriter writer)
        {
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


            iTextSharp.text.Font reportNameStyle = new iTextSharp.text.Font(bf_Calibri, 12, 1, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#002060")));
            iTextSharp.text.Font xTitleCellstyle2 = new iTextSharp.text.Font(bf_Calibri, 10, 1, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0070C0")));
            iTextSharp.text.Font xsubTitleintakeCellstyle = new iTextSharp.text.Font(bf_Calibri, 9, 1, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0070C0")));
            iTextSharp.text.Font paramsCellStyle = new iTextSharp.text.Font(bf_Calibri, 8);

            PdfPTable Headertable = new PdfPTable(5);
            Headertable.TotalWidth = 510f;
            Headertable.WidthPercentage = 100;
            Headertable.LockedWidth = true;
            float[] Headerwidths = new float[] { 30f, 70f, 10f, 30f, 70f };
            Headertable.SetWidths(Headerwidths);
            Headertable.HorizontalAlignment = Element.ALIGN_CENTER;

            //***************** border trails *******************************//
            PdfContentByte content = writer.DirectContent;
            iTextSharp.text.Rectangle rectangle = new iTextSharp.text.Rectangle(document.PageSize);
            rectangle.Left += document.LeftMargin;
            rectangle.Right -= document.RightMargin;
            rectangle.Top -= document.TopMargin;
            rectangle.Bottom += document.BottomMargin;
            content.SetColorStroke(BaseColor.BLACK);
            content.Rectangle(rectangle.Left, rectangle.Bottom, rectangle.Width, rectangle.Height);
            content.Stroke();

            string strAgy = Current_Hierarchy_DB.Split('-')[0];
            AgencyControlEntity BAgyControlDetails = _model.ZipCodeAndAgency.GetAgencyControlFile(strAgy);
            string ImgName = "";
            if (_baseform.BaseAgencyControlDetails.AgyShortName == "NEOCAA")
            {
                ImgName = "NEOCAA_" + strAgy + "_LOGO.png";
            }
            else
                ImgName = _baseform.BaseAgencyControlDetails.AgyShortName + "_00_LOGO.png";

            string imagesPath = "https://capsysdev.capsystems.com/images/PIPlogos/" + ImgName;

            if (imagesPath != "")
            {
                iTextSharp.text.Image imgLogo = iTextSharp.text.Image.GetInstance(imagesPath);
                imgLogo.ScaleAbsolute(120f, 50f);
                PdfPCell cellLogo = new PdfPCell(imgLogo);
                cellLogo.HorizontalAlignment = Element.ALIGN_CENTER;
                cellLogo.Rowspan = 2;
                cellLogo.Padding = 3;
                cellLogo.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Headertable.AddCell(cellLogo);
            }


            AgencyControlEntity _agycntrldets = new AgencyControlEntity();
            _agycntrldets = _baseform.BaseAgencyControlDetails;

            if (_baseform.BaseAgencyControlDetails.AgyShortName == "NEOCAA")
                _agycntrldets = BAgyControlDetails;
            else
                _agycntrldets = _baseform.BaseAgencyControlDetails;

            string street = _agycntrldets.Street == "" ? "" : (_agycntrldets.Street + ", ");
            string city = _agycntrldets.City == "" ? "" : (_agycntrldets.City + ", ");
            string state = _agycntrldets.State == "" ? "" : (_agycntrldets.State + ", ");
            string zip1 = _agycntrldets.Zip1 == "" ? "" : _agycntrldets.Zip1.PadLeft(5, '0');
            string strAddress = street + city + state + zip1;


            PdfPCell rowH = new PdfPCell(new Phrase(BAgyControlDetails.AgyName, TblParamsHeaderFont));
            rowH.HorizontalAlignment = Element.ALIGN_CENTER;
            rowH.Colspan = 5;
            //rowH.PaddingBottom = 15;
            rowH.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Headertable.AddCell(rowH);

            PdfPCell row1 = new PdfPCell(new Phrase(strAddress, TblParamsHeaderFont));
            row1.HorizontalAlignment = Element.ALIGN_CENTER;
            row1.Colspan = 4;
            //row1.PaddingBottom = 15;
            row1.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Headertable.AddCell(row1);

            PdfPCell row2 = new PdfPCell(new Phrase(_priviliges.PrivilegeName, reportNameStyle));
            row2.HorizontalAlignment = Element.ALIGN_CENTER;
            row2.Colspan = 5;
            row2.PaddingBottom = 8;
            row2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#CFE6F9"));
            row2.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Headertable.AddCell(row2);


            PdfPCell row3 = new PdfPCell(new Phrase("Report Parameters", xTitleCellstyle2));
            row3.HorizontalAlignment = Element.ALIGN_CENTER;
            row3.Colspan = 5;
            row3.PaddingBottom = 8;
            row3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEF5FC"));
            row3.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Headertable.AddCell(row3);


            string Agy = "All";
            string Dept = "All";
            string Prg = "All";
            string program_year = string.Empty;
            if (Current_Hierarchy.Substring(0, 2) != "**")
                Agy = Current_Hierarchy.Substring(0, 2);
            if (Current_Hierarchy.Substring(2, 2) != "**")
                Dept = Current_Hierarchy.Substring(2, 2);
            if (Current_Hierarchy.Substring(4, 2) != "**")
                Prg = Current_Hierarchy.Substring(4, 2);
            if (CmbYear.Visible == true)
                program_year = "Year: " + ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString();

            if (CmbYear.Visible == true)
            {
                PdfPCell row4 = new PdfPCell(new Phrase(Txt_HieDesc.Text.Trim() + "   " + program_year, xsubTitleintakeCellstyle));
                row4.HorizontalAlignment = Element.ALIGN_CENTER;
                row4.Colspan = 5;
                row4.PaddingBottom = 8;
                row4.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F8FBFE"));
                row4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Headertable.AddCell(row4);
            }
            else
            {
                PdfPCell row4 = new PdfPCell(new Phrase(Txt_HieDesc.Text.Trim(), xsubTitleintakeCellstyle));
                row4.HorizontalAlignment = Element.ALIGN_CENTER;
                row4.Colspan = 5;
                row4.PaddingBottom = 8;
                row4.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F8FBFE"));
                row4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Headertable.AddCell(row4);
            }


            PdfPCell rSpace = new PdfPCell(new Phrase(" ", TblHeaderTitleFont));
            rSpace.HorizontalAlignment = Element.ALIGN_CENTER;
            rSpace.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            rSpace.MinimumHeight = 6;
            rSpace.Colspan = 5;
            rSpace.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Headertable.AddCell(rSpace);


            /*** Prameters Start ***/
            //Row 1
            PdfPCell CH1 = new PdfPCell(new Phrase(lblFromDte.Text.Trim(), paramsCellStyle));
            CH1.HorizontalAlignment = Element.ALIGN_LEFT;
            CH1.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CH1.Border = iTextSharp.text.Rectangle.BOX;
            CH1.PaddingBottom = 5;
            CH1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEF5FC"));
            Headertable.AddCell(CH1);

            PdfPCell CB1 = new PdfPCell(new Phrase(dtpFromDte.Text.Trim(), paramsCellStyle));
            CB1.HorizontalAlignment = Element.ALIGN_LEFT;
            CB1.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CB1.Border = iTextSharp.text.Rectangle.BOX;
            CB1.PaddingBottom = 5;
            CB1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            Headertable.AddCell(CB1);

            PdfPCell CS1 = new PdfPCell(new Phrase("", paramsCellStyle));
            CS1.HorizontalAlignment = Element.ALIGN_LEFT;
            CS1.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CS1.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Headertable.AddCell(CS1);

            PdfPCell CH1_2 = new PdfPCell(new Phrase(lblToDate.Text, paramsCellStyle));
            CH1_2.HorizontalAlignment = Element.ALIGN_LEFT;
            CH1_2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CH1_2.Border = iTextSharp.text.Rectangle.BOX;
            CH1_2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEF5FC"));
            Headertable.AddCell(CH1_2);

            PdfPCell CB1_2 = new PdfPCell(new Phrase(dtpToDte.Text, paramsCellStyle));
            CB1_2.HorizontalAlignment = Element.ALIGN_LEFT;
            CB1_2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CB1_2.Border = iTextSharp.text.Rectangle.BOX;
            CB1_2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            Headertable.AddCell(CB1_2);


            //Row 2

            string staffNames = rdbAllSName.Checked ? lblStaffName.Text : "Selected Staff Member(s)";

            PdfPCell CH2 = new PdfPCell(new Phrase(staffNames, paramsCellStyle));
            CH2.HorizontalAlignment = Element.ALIGN_LEFT;
            CH2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CH2.Border = iTextSharp.text.Rectangle.BOX;
            CH2.PaddingBottom = 5;
            CH2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEF5FC"));
            Headertable.AddCell(CH2);

            List<STAFFMSTEntity> selstaff = STAFFMST_List.FindAll(x => Sel_Staff.Contains(x.Staff_Code)).ToList();
            string printStaff = string.Empty;
            foreach (STAFFMSTEntity entity in selstaff)
            {
                printStaff = printStaff + (entity.First_Name + " " +entity.Last_Name) + ", ";
            }

            staffNames = rdbAllSName.Checked ? "All Staff Members" : printStaff.Trim().TrimEnd(',');

            PdfPCell CB2 = new PdfPCell(new Phrase(staffNames, paramsCellStyle));
            CB2.HorizontalAlignment = Element.ALIGN_LEFT;
            CB2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CB2.Border = iTextSharp.text.Rectangle.BOX;
            CB2.PaddingBottom = 5;
            CB2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            Headertable.AddCell(CB2);

            PdfPCell CS2 = new PdfPCell(new Phrase("", paramsCellStyle));
            CS2.HorizontalAlignment = Element.ALIGN_LEFT;
            CS2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CS2.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Headertable.AddCell(CS2);

            string trainingNames = rdbAllTNames.Checked ? lblTrainName.Text : "Selected Training(s)";

            PdfPCell CH2_2 = new PdfPCell(new Phrase(trainingNames, paramsCellStyle));
            CH2_2.HorizontalAlignment = Element.ALIGN_LEFT;
            CH2_2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CH2_2.Border = iTextSharp.text.Rectangle.BOX;
            CH2_2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEF5FC"));
            Headertable.AddCell(CH2_2);

            List<STAFFTRAINEntity> selTrainings = staffTrainEntity.FindAll(x => Sel_Training.Contains(x.ID)).ToList();
            string printTraining = string.Empty;
            foreach (STAFFTRAINEntity entity in selTrainings)
            {
                printTraining = printTraining + entity.Name + ", ";
            }

            trainingNames = rdbAllTNames.Checked ? "All Trainings" : printTraining.Trim().TrimEnd(',');

            PdfPCell CB2_2 = new PdfPCell(new Phrase(trainingNames, paramsCellStyle));
            CB2_2.HorizontalAlignment = Element.ALIGN_LEFT;
            CB2_2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CB2_2.Border = iTextSharp.text.Rectangle.BOX;
            CB2_2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            Headertable.AddCell(CB2_2);


            //Row 3

            string topics = rdbAllTopic.Checked ? lblTopic.Text : "Selected Topic(s)";

            PdfPCell CH3 = new PdfPCell(new Phrase(topics, paramsCellStyle));
            CH3.HorizontalAlignment = Element.ALIGN_LEFT;
            CH3.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CH3.Border = iTextSharp.text.Rectangle.BOX;
            CH3.PaddingBottom = 5;
            CH3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEF5FC"));
            Headertable.AddCell(CH3);

            List<CommonEntity> selTopics = Topic.FindAll(x => Sel_Topics.Contains(x.Code)).ToList();
            string printTopic = string.Empty;
            foreach (CommonEntity entity in selTopics)
            {
                printTopic = printTopic + entity.Desc + ", ";
            }

            topics = rdbAllTopic.Checked ? "All Topics" : printTopic.Trim().TrimEnd(',');

            PdfPCell CB3 = new PdfPCell(new Phrase(topics, paramsCellStyle));
            CB3.HorizontalAlignment = Element.ALIGN_LEFT;
            CB3.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CB3.Border = iTextSharp.text.Rectangle.BOX;
            CB3.PaddingBottom = 5;
            CB3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            Headertable.AddCell(CB3);

            PdfPCell CS3 = new PdfPCell(new Phrase("", paramsCellStyle));
            CS3.HorizontalAlignment = Element.ALIGN_LEFT;
            CS3.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CS3.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Headertable.AddCell(CS3);

            PdfPCell CH3_2 = new PdfPCell(new Phrase(lblServArea.Text, paramsCellStyle));
            CH3_2.HorizontalAlignment = Element.ALIGN_LEFT;
            CH3_2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CH3_2.Border = iTextSharp.text.Rectangle.BOX;
            CH3_2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEF5FC"));
            Headertable.AddCell(CH3_2);

            PdfPCell CB3_2 = new PdfPCell(new Phrase(pdfServText == "" ? "All Service Areas" : pdfServText, paramsCellStyle));
            CB3_2.HorizontalAlignment = Element.ALIGN_LEFT;
            CB3_2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CB3_2.Border = iTextSharp.text.Rectangle.BOX;
            CB3_2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            Headertable.AddCell(CB3_2);


            //Row 4

            string formats = rdbAllFormat.Checked ? lblFormat.Text : "Selected Format(s)";

            PdfPCell CH4 = new PdfPCell(new Phrase(formats, paramsCellStyle));
            CH4.HorizontalAlignment = Element.ALIGN_LEFT;
            CH4.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CH4.Border = iTextSharp.text.Rectangle.BOX;
            CH4.PaddingBottom = 5;
            CH4.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEF5FC"));
            Headertable.AddCell(CH4);

            List<CommonEntity> selFormats = Format.FindAll(x => Sel_Formats.Contains(x.Code)).ToList();
            string printFormat = string.Empty;
            foreach (CommonEntity entity in selFormats)
            {
                printFormat = printFormat + entity.Desc + ", ";
            }

            formats = rdbAllFormat.Checked ? "All Formats" : printFormat.Trim().TrimEnd(',');

            PdfPCell CB4 = new PdfPCell(new Phrase(formats, paramsCellStyle));
            CB4.HorizontalAlignment = Element.ALIGN_LEFT;
            CB4.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CB4.Border = iTextSharp.text.Rectangle.BOX;
            CB4.PaddingBottom = 5;
            CB4.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            Headertable.AddCell(CB4);

            PdfPCell CS4 = new PdfPCell(new Phrase("", paramsCellStyle));
            CS4.HorizontalAlignment = Element.ALIGN_LEFT;
            CS4.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CS4.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Headertable.AddCell(CS4);

            string locs = rdbAllLocation.Checked ? lblLoc.Text : "Selected Location(s)";

            PdfPCell CH4_2 = new PdfPCell(new Phrase(locs, paramsCellStyle));
            CH4_2.HorizontalAlignment = Element.ALIGN_LEFT;
            CH4_2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CH4_2.Border = iTextSharp.text.Rectangle.BOX;
            CH4_2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEF5FC"));
            Headertable.AddCell(CH4_2);

            List<CommonEntity> selLoc = Location.FindAll(x => Sel_Loc.Contains(x.Code)).ToList();
            string printLoc = string.Empty;
            foreach (CommonEntity entity in selLoc)
            {
                printLoc = printLoc + entity.Desc + ", ";
            }

            locs = rdbAllLocation.Checked ? "All Locations" : printLoc.Trim().TrimEnd(',');

            PdfPCell CB4_2 = new PdfPCell(new Phrase(locs, paramsCellStyle));
            CB4_2.HorizontalAlignment = Element.ALIGN_LEFT;
            CB4_2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CB4_2.Border = iTextSharp.text.Rectangle.BOX;
            CB4_2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            Headertable.AddCell(CB4_2);

            //Row 5

            string credTypes = rdbAllCredType.Checked ? lblCredType.Text : "Selected Credit Type(s)";

            PdfPCell CH5 = new PdfPCell(new Phrase(credTypes, paramsCellStyle));
            CH5.HorizontalAlignment = Element.ALIGN_LEFT;
            CH5.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CH5.Border = iTextSharp.text.Rectangle.BOX;
            CH5.PaddingBottom = 5;
            CH5.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEF5FC"));
            Headertable.AddCell(CH5);

            List<CommonEntity> selCredType = CredType.FindAll(x => Sel_CredType.Contains(x.Code)).ToList();
            string printCredType = string.Empty;
            foreach (CommonEntity entity in selCredType)
            {
                printCredType = printCredType + entity.Desc + ", ";
            }

            credTypes = rdbAllCredType.Checked ? "All Credit Types" : printCredType.Trim().TrimEnd(',');

            PdfPCell CB5 = new PdfPCell(new Phrase(credTypes, paramsCellStyle));
            CB5.HorizontalAlignment = Element.ALIGN_LEFT;
            CB5.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CB5.Border = iTextSharp.text.Rectangle.BOX;
            CB5.PaddingBottom = 5;
            CB5.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            Headertable.AddCell(CB5);

            CS2 = new PdfPCell(new Phrase("", paramsCellStyle));
            CS2.HorizontalAlignment = Element.ALIGN_LEFT;
            CS2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CS2.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Headertable.AddCell(CS2);

            CS2 = new PdfPCell(new Phrase("", paramsCellStyle));
            CS2.HorizontalAlignment = Element.ALIGN_LEFT;
            CS2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CS2.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Headertable.AddCell(CS2);

            CS2 = new PdfPCell(new Phrase("", paramsCellStyle));
            CS2.HorizontalAlignment = Element.ALIGN_LEFT;
            CS2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CS2.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Headertable.AddCell(CS2);

            document.Add(Headertable);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase("Generated By : ", fnttimesRoman_Italic), 33, 18, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.GetMemberName(_baseform.UserProfile.FirstName.Trim(), _baseform.UserProfile.MI.Trim(), _baseform.UserProfile.LastName.Trim(), "3"), fnttimesRoman_Italic), 90, 18, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase("Generated On : ", fnttimesRoman_Italic), 410, 18, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase(DateTime.Now.ToString(), fnttimesRoman_Italic), 468, 18, 0);

        }

        private void btnPdfPreview_Click(object sender, EventArgs e)
        {
            PdfListForm pdfListForm = new PdfListForm(_baseform, _priviliges, true, propReportPath);
            pdfListForm.StartPosition = FormStartPosition.CenterScreen;
            pdfListForm.ShowDialog();
        }
    }
}
