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
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using XLSExportFile;
using CarlosAg.ExcelXmlWriter;
using DevExpress.CodeParser;
using System.Threading.Tasks;
using static DevExpress.Xpo.Helpers.AssociatedCollectionCriteriaHelper;
using Aspose.Cells.Drawing;
using Captain.Common.Exceptions;
#endregion

namespace Captain.Common.Views.Forms
{
    public partial class HSSB2114ReportForm : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;


        #endregion
        public HSSB2114ReportForm(BaseForm baseForm, PrivilegeEntity privileges)
        {
            InitializeComponent(); _model = new CaptainModel();
            BaseForm = baseForm;
            Privileges = privileges;
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            this.Text = /*privileges.Program + " - " +*/ privileges.PrivilegeName;
            Set_Report_Hierarchy(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear);
            Agency = BaseForm.BaseAgency;
            Dept = BaseForm.BaseDept;
            Prog = BaseForm.BaseProg;
            Program_Year = BaseForm.BaseYear;
            txtFrom.Validator = TextBoxValidation.IntegerValidator;
            txtTo.Validator = TextBoxValidation.IntegerValidator;
            List<CommonEntity> listSequence = _model.lookupDataAccess.GetHssb2108FormActiveDetails();
            List<CommonEntity> listFundings = _model.lookupDataAccess.GetHssb2114FormFundings();
            propReportPath = _model.lookupDataAccess.GetReportPath();
            propfundingSource = _model.lookupDataAccess.GetAgyTabRecordsByCodefilter(Consts.AgyTab.CASEMNGMTFUNDSRC, "H");
            propcustQuestions = _model.FieldControls.GetCustomQuestions("HSS00133", "*", string.Empty, string.Empty, string.Empty, string.Empty);

            foreach (CommonEntity item in listSequence)
            {
                cmbActive.Items.Add(new Captain.Common.Utilities.ListItem(item.Desc, item.Code));
            }
            cmbActive.SelectedIndex = 2;
            /*cmbFundingSource.ColorMember = "FavoriteColor";
            listFundings = listFundings.OrderByDescending(u =>u.Active).ToList();*/
            foreach (CommonEntity item in listFundings)
            {
                cmbFundingSource.Items.Add(new Captain.Common.Utilities.ListItem(item.Desc, item.Code)); //, item.Active, item.Active.Equals("Y") ? Color.Black : Color.Red) ;
            }
            cmbFundingSource.SelectedIndex = 2;
            FillComponents();

        }

        List<SPCommonEntity> AgyCommon_List = new List<SPCommonEntity>();
        DataSet dsComp = DatabaseLayer.Lookups.GetLookUpFromAGYTAB("00356");
        private void FillComponents()
        {
            string components = BaseForm.UserProfile.Components.ToString();
            string Tmp_Desc = string.Empty;
            List<string> incomeList = new List<string>();
            if (components != null)
            {
                string[] incomeTypes = components.Split(' ');
                for (int i = 0; i < incomeTypes.Length; i++)
                {
                    incomeList.Add(incomeTypes.GetValue(i).ToString());
                }
            }


            CmbTrackComp.Items.Clear();
            DataTable dtcomp = dsComp.Tables[0];
            CmbTrackComp.ColorMember = "FavoriteColor";

            if (dtcomp != null && dtcomp.Rows.Count > 0)
            {
                foreach (DataRow row in dtcomp.Rows)
                {
                    AgyCommon_List.Add(new SPCommonEntity(row));
                }
            }
            AgyCommon_List = AgyCommon_List.OrderByDescending(x => x.Active).ToList();
            foreach (SPCommonEntity Entity in AgyCommon_List)
            {
                if (components.Contains("****"))
                {
                    CmbTrackComp.Items.Add(new Captain.Common.Utilities.ListItem(Entity.Desc.ToString().Trim(), Entity.Code, Entity.Active, Entity.Active.Equals("Y") ? Color.Black : Color.Red));
                }
                else
                {
                    if (components != null && incomeList.Contains(Entity.Code.ToString()))
                    {
                        CmbTrackComp.Items.Add(new Captain.Common.Utilities.ListItem(Entity.Desc.ToString().Trim(), Entity.Code, Entity.Active, Entity.Active.Equals("Y") ? Color.Black : Color.Red));
                    }
                }
            }
            if (components != string.Empty)
                CmbTrackComp.SelectedIndex = 0;
            else
            {
                CmbTrackComp.Items.Add(new Captain.Common.Utilities.ListItem(" ", "0"));
                CmbTrackComp.SelectedIndex = 0;
            }

        }


        private void fillCustQuestions(string custQues, PdfPTable hssb2103_Table2)
        {
            List<string> QuesList = new List<string>();
            if (custQues != null)
            {
                string[] relationTypes = custQues.Split(' ');
                for (int i = 0; i < relationTypes.Length; i++)
                {
                    QuesList.Add(relationTypes.GetValue(i).ToString());
                }
            }
            List<CustomQuestionsEntity> custQuestionsList = propcustQuestions;
            bool boolQuestionDisplay = true;
            foreach (CustomQuestionsEntity entityQuestion in custQuestionsList)
            {

                if (custQues != null && QuesList.Contains(entityQuestion.CUSTCODE))
                {
                    boolQuestionDisplay = false;
                    BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
                    iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 8);
                    PdfPCell pdfData2 = new PdfPCell(new Phrase((entityQuestion.CUSTDESC), TableFont));
                    pdfData2.HorizontalAlignment = Element.ALIGN_LEFT;
                    pdfData2.Colspan = 8;
                    pdfData2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    hssb2103_Table2.AddCell(pdfData2);


                    PdfPCell pdfSpace4 = new PdfPCell(new Phrase("", TableFont));
                    pdfSpace4.HorizontalAlignment = Element.ALIGN_LEFT;
                    pdfSpace4.Colspan = 8;
                    pdfSpace4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    hssb2103_Table2.AddCell(pdfSpace4);

                    PdfPCell pdfSpace5 = new PdfPCell(new Phrase("", TableFont));
                    pdfSpace5.HorizontalAlignment = Element.ALIGN_LEFT;
                    pdfSpace5.Colspan = 8;
                    pdfSpace5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    hssb2103_Table2.AddCell(pdfSpace5);


                    PdfPCell pdfSpace6 = new PdfPCell(new Phrase("", TableFont));
                    pdfSpace6.HorizontalAlignment = Element.ALIGN_LEFT;
                    pdfSpace6.Colspan = 8;
                    pdfSpace6.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    hssb2103_Table2.AddCell(pdfSpace6);


                    PdfPCell pdfData3 = new PdfPCell(new Phrase("Result: (" + entityQuestion.CUSTRESPTYPE.Trim() + ") _________________  Date_________________________________", TableFont));
                    pdfData3.HorizontalAlignment = Element.ALIGN_LEFT;
                    pdfData3.Colspan = 4;
                    pdfData3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    hssb2103_Table2.AddCell(pdfData3);

                    PdfPCell pdfData4 = new PdfPCell(new Phrase("Comments______________________________________________________", TableFont));
                    pdfData4.HorizontalAlignment = Element.ALIGN_LEFT;
                    pdfData4.Colspan = 4;
                    pdfData4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    hssb2103_Table2.AddCell(pdfData4);

                    PdfPCell pdfSpace7 = new PdfPCell(new Phrase("", TableFont));
                    pdfSpace7.HorizontalAlignment = Element.ALIGN_LEFT;
                    pdfSpace7.Colspan = 8;
                    pdfSpace7.FixedHeight = 10f;
                    pdfSpace7.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    hssb2103_Table2.AddCell(pdfSpace7);

                }
            }
            if (boolQuestionDisplay)
            {
                PdfPCell pdfSpace7 = new PdfPCell(new Phrase(""));
                pdfSpace7.HorizontalAlignment = Element.ALIGN_LEFT;
                pdfSpace7.Colspan = 8;
                pdfSpace7.FixedHeight = 10f;
                pdfSpace7.Border = iTextSharp.text.Rectangle.NO_BORDER;
                hssb2103_Table2.AddCell(pdfSpace7);
            }

        }


        #region properties

        public BaseForm BaseForm { get; set; }
        public PrivilegeEntity Privileges { get; set; }
        public List<RankCatgEntity> propRankscategory { get; set; }
        public string propReportPath { get; set; }
        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Prog { get; set; }
        public List<CaseEnrlEntity> propCaseEnrlList { get; set; }
        public string propMealTypes { get; set; }
        public List<CaseSiteEntity> propCaseSiteEntity { get; set; }
        public List<CommonEntity> propfundingSource { get; set; }
        public List<ChldAttnEntity> propChldAttnList { get; set; }
        public List<ChldAttnEntity> propChldAttnCountList { get; set; }
        public List<ZipCodeEntity> propZipCode { get; set; }
        public List<ChldMediEntity> propChldMediEntity { get; set; }
        public List<CustomQuestionsEntity> propcustQuestions { get; set; }
        public List<ChldTrckREntity> propchldtrackRList { get; set; }
        

        #endregion
        string strsiteRoomNames = string.Empty;


        private void btnBrowse_Click(object sender, EventArgs e)
        {
            BrowseApplicantForm BrowseApplcantForm = new BrowseApplicantForm(BaseForm, string.Empty, Privileges, Agency, Dept, Prog, Program_Year);
            BrowseApplcantForm.FormClosed += new FormClosedEventHandler(BrowseApplcantForm_FormClosed);
            BrowseApplcantForm.StartPosition = FormStartPosition.CenterScreen;
            BrowseApplcantForm.ShowDialog();
        }

        void BrowseApplcantForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            BrowseApplicantForm BrowseApplication = sender as BrowseApplicantForm;
            if (BrowseApplication.DialogResult == DialogResult.OK)
            {

                CaseMstEntity caseMstData = BrowseApplication.MstData;
                if (caseMstData != null)
                {
                    txtApplicant.Text = caseMstData.ApplNo;
                }
            }
        }

        private void txtApplicant_Leave(object sender, EventArgs e)
        {
            //if (txtApplicant.Text.Trim() != string.Empty)
            //{
            //    txtApplicant.Text = SetLeadingZeros(txtApplicant.Text);

            //    CaseEnrlEntity Search_Entity = new CaseEnrlEntity(true);
            //    Search_Entity.Join_Mst_Snp = "N";
            //    Search_Entity.Agy = Agency;
            //    Search_Entity.Dept = Dept;
            //    Search_Entity.Prog = Prog;
            //    Search_Entity.Year = Program_Year;
            //    Search_Entity.App = txtApplicant.Text;
            //    Search_Entity.Rec_Type = "H";
            //    // Search_Entity.Status = "E";
            //    List<CaseEnrlEntity> Transfer_Enroll_List = new List<CaseEnrlEntity>();
            //    Transfer_Enroll_List = _model.EnrollData.Browse_CASEENRL(Search_Entity, "Browse");
            //    if (Transfer_Enroll_List.Count == 0)
            //    {
            //        txtApplicant.Text = string.Empty;
            //        CommonFunctions.MessageBoxDisplay("Applicant does not exist");
            //    }


            //}
            if (txtApplicant.Text.Trim() != string.Empty)
            {
                txtApplicant.Text = SetLeadingZeros(txtApplicant.Text);
                List<HSSB2106Report_Entity> SeaarchReport_list = _model.ChldTrckData.GetChldTrck_Report(Agency, Dept, Prog, Program_Year, txtApplicant.Text, null, null, null, null, null, null, null, null);

                if (SeaarchReport_list.Count == 0)
                {
                    txtApplicant.Text = string.Empty;
                   AlertBox.Show("Applicant does not exist", MessageBoxIcon.Warning);
                }


            }
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

        private void btnPdfPreview_Click(object sender, EventArgs e)
        {
            PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, true, propReportPath);
            pdfListForm.StartPosition = FormStartPosition.CenterScreen;
            pdfListForm.ShowDialog();
        }

        private void rdoSelectApplicant_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void CmbYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program_Year = "    ";
            if (!(string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString())))
            {
                Program_Year = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString();
            }
        }

        private void Pb_Search_Hie_Click(object sender, EventArgs e)
        {
            /*HierarchieSelectionFormNew hierarchieSelectionForm = new HierarchieSelectionFormNew(BaseForm, Current_Hierarchy_DB, "Master", "", "A", "Reports");
            hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
            hierarchieSelectionForm.ShowDialog();*/

            HierarchieSelection hierarchieSelectionForm = new HierarchieSelection(BaseForm, Current_Hierarchy_DB, "Master", "", "A", "Reports", BaseForm.UserID);
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
                    Dept = hierarchy.Substring(2, 2);
                    Prog = hierarchy.Substring(4, 2);

                }

            }
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
                this.Txt_HieDesc.Size = new System.Drawing.Size(725, 25);
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
                this.Txt_HieDesc.Size = new System.Drawing.Size(645, 25);
            else
                this.Txt_HieDesc.Size = new System.Drawing.Size(725, 25);
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

        private bool ValidateForm()
        {
            bool isValid = true;

            _errorProvider.SetError(cmbFundingSource, null);
            _errorProvider.SetError(rdoMultipleTask, null);
            _errorProvider.SetError(rdoMultipleSites, null);
            _errorProvider.SetError(btnBrowse, null);
            _errorProvider.SetError(dtSBCB, null);
            _errorProvider.SetError(txtTo, null);

            if (dtSBCB.Checked)
            {
                if (string.IsNullOrWhiteSpace(dtSBCB.Text))
                {
                    _errorProvider.SetError(dtSBCB, "SBCB Date is Required");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtSBCB, null);
                }
            }

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
            if (rdoMultipleTask.Checked == true)
            {
                if (SeltrackList.Count == 0)
                {
                    _errorProvider.SetError(rdoMultipleTask, "Please Select at least One Task");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(rdoMultipleTask, null);
                }
            }

            if (((Captain.Common.Utilities.ListItem)CmbTrackComp.SelectedItem).Value.ToString() == "0")
            {
                _errorProvider.SetError(CmbTrackComp, "Please Select Track Component");
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(CmbTrackComp, null);
            }

            if (((Captain.Common.Utilities.ListItem)cmbFundingSource.SelectedItem).Value.ToString().Trim() == "D")
            {
                if (SelFundingList.Count == 0)
                {
                    _errorProvider.SetError(btnFunding, "Please Select at least One Daycare Funding");
                    isValid = false;
                }
                else
                    _errorProvider.SetError(btnFunding, null);
            }
            else
            {
                _errorProvider.SetError(cmbFundingSource, null);
            }

            if (rdoSelectedApplicantSeq.Checked == true)
            {

                if (txtApplicant.Text.Trim() == string.Empty)
                {
                    _errorProvider.SetError(btnBrowse, "Please Select Applicant");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(btnBrowse, null);
                }
            }

            if (string.IsNullOrEmpty(txtFrom.Text.Trim()) || string.IsNullOrEmpty(txtTo.Text.Trim()))
            {
                if (string.IsNullOrWhiteSpace(txtFrom.Text) || string.IsNullOrWhiteSpace(txtTo.Text))
                {
                    _errorProvider.SetError(txtTo, "Please enter the missing From/To Children Age");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtTo, null);
                }
            }
            else
            {
                if (Convert.ToInt32(txtFrom.Text) > -1 && Convert.ToInt32(txtTo.Text) > -1)
                {
                    if (Convert.ToInt32(txtFrom.Text) > Convert.ToInt32(txtTo.Text))
                    {
                        _errorProvider.SetError(txtTo, "'To Age' should be Greater than or Equal to 'From Age'");
                        isValid = false;
                    }
                }
                else
                {
                    _errorProvider.SetError(txtTo, "Values of Children Age cannot be Negative");
                    isValid = false;
                }
            }

            return (isValid);
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
            string Active = ((Captain.Common.Utilities.ListItem)cmbActive.SelectedItem).Value.ToString();
            string MaxTask = ((Captain.Common.Utilities.ListItem)CmbTrackComp.SelectedItem).Value.ToString();
            string strFundSource = ((Captain.Common.Utilities.ListItem)cmbFundingSource.SelectedItem).Value.ToString(); 
            string strSite = rdoAllSite.Checked == true ? "A" : "M";
            string strReportSequence = string.Empty;
            string strEnrollType = string.Empty;
            string SbcbDt = string.Empty;
            if (dtSBCB.Checked)
                SbcbDt = dtSBCB.Value.ToShortDateString();

            if (rdoApplicantNumseq.Checked == true)
                strReportSequence = "NUM";
            else if (rdoApplicantNameSeq.Checked == true)
                strReportSequence = "NAM";
            else if (rdoClassSeq.Checked == true)
                strReportSequence = "CLA";
            else if (rdoSelectedApplicantSeq.Checked == true)
                strReportSequence = "SEL";

            string strBaseAge = rdoTodayDate.Checked ? "T" : "K";

            if (rdoEnrolled.Checked == true)
                strEnrollType = "E";
            else if (rdounEnrolled.Checked == true)
                strEnrollType = "U";
            else if (rdoEnrollBoth.Checked == true)
                strEnrollType = "A";

            string strTaskSource = rdoAllTask.Checked ? "Y" : "N";




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

            if (strFundSource == "D")
            {
                foreach (CommonEntity FundingCode in SelFundingList)
                {
                    if (!strFundingCodes.Equals(string.Empty)) strFundingCodes += ",";
                    strFundingCodes += FundingCode.Code;
                }
            }


            string strTaskCodes = string.Empty;
            foreach (ChldTrckEntity Entity in SeltrackList)
            {
                if (!strTaskCodes.Equals(string.Empty)) strTaskCodes += ",";
                strTaskCodes += Entity.TASK;
            }

            string Excel = "N";
            if (chkbExcel.Checked)
                Excel = "Y";

            StringBuilder str = new StringBuilder();
            str.Append("<Rows>");
            str.Append("<Row AGENCY = \"" + Agency + "\" DEPT = \"" + Dept + "\" PROG = \"" + Prog +
                            "\" YEAR = \"" + Program_Year + "\" Site = \"" + strSite + "\"  MaxofTask = \"" + MaxTask + "\" Active = \"" + Active + "\" ReportSequence = \"" + strReportSequence +
                            "\" AgeFrom = \"" + txtFrom.Text + "\" AgeTo = \"" + txtTo.Text + "\" BaseAge = \"" + strBaseAge + "\"  EnrollType = \"" + strEnrollType + "\" FundedSource = \"" + strFundSource + "\"  SiteNames = \"" + strsiteRoomNames + "\" FundingCode = \"" + strFundingCodes + "\" TaskNames = \"" + strTaskCodes + "\"  SBCBDt = \"" + SbcbDt + "\" TaskForAll = \"" + strTaskSource + "\" ApplicantNo = \"" + txtApplicant.Text + "\" EXCEL = \"" + Excel + "\" />");

            str.Append("</Rows>");

            return str.ToString();
        }

        private void Set_Report_Controls(DataTable Tmp_Table)
        {
            if (Tmp_Table != null && Tmp_Table.Rows.Count > 0)
            {
                DataRow dr = Tmp_Table.Rows[0];

                Set_Report_Hierarchy(dr["AGENCY"].ToString(), dr["DEPT"].ToString(), dr["PROG"].ToString(), dr["YEAR"].ToString());

                CommonFunctions.SetComboBoxValue(CmbTrackComp, dr["MaxofTask"].ToString());
                CommonFunctions.SetComboBoxValue(cmbActive, dr["Active"].ToString());
                

                if (dr["SBCBDt"] != string.Empty)
                {
                    dtSBCB.Value = Convert.ToDateTime(dr["SBCBDt"].ToString());
                    dtSBCB.Checked = true;
                }

                if (dr["Site"].ToString() == "A")
                    rdoAllSite.Checked = true;
                else
                {
                    rdoMultipleSites.Checked = true;
                    CaseSiteEntity Search_Entity = new CaseSiteEntity(true);
                    Search_Entity.SiteAGENCY = Agency; Search_Entity.SiteDEPT = Dept;
                    Search_Entity.SitePROG = Prog; Search_Entity.SiteYEAR = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString().Trim();
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
                            if (siteDetails != null)
                                Sel_REFS_List.Add(siteDetails);
                        }
                    }

                }
                Sel_REFS_List = Sel_REFS_List;


                if (dr["ReportSequence"].ToString() == "NUM")
                    rdoApplicantNumseq.Checked = true;
                else if (dr["ReportSequence"].ToString() == "NAM")
                    rdoApplicantNameSeq.Checked = true;
                else if (dr["ReportSequence"].ToString() == "CLS")
                    rdoClassSeq.Checked = true;
                else if (dr["ReportSequence"].ToString() == "SEL")
                    rdoSelectedApplicantSeq.Checked = true;

                txtApplicant.Text = dr["ApplicantNo"].ToString();

                

                List<ChldTrckEntity> chldTrckList = _model.ChldTrckData.GetCasetrckDetails(string.Empty, string.Empty, string.Empty, "0000", string.Empty, string.Empty);

                string strTasks = dr["TaskNames"].ToString();
                List<string> siteTaskList = new List<string>();
                if (strTasks != null)
                {
                    string[] TaskCodes = strTasks.Split(',');
                    for (int i = 0; i < TaskCodes.Length; i++)
                    {
                        ChldTrckEntity TaskDetails = chldTrckList.Find(u => u.TASK.Trim() == TaskCodes.GetValue(i).ToString());
                        if (TaskDetails != null)
                            SeltrackList.Add(TaskDetails);
                    }
                }
                SeltrackList = SeltrackList;

                //txtApplicant.Text = dr["ApplicantNo"].ToString();
                txtFrom.Text = dr["AgeFrom"].ToString();
                txtTo.Text = dr["AgeTo"].ToString();


                if (dr["BaseAge"].ToString() == "T")
                    rdoTodayDate.Checked = true;
                else
                    rdoKindergartenDate.Checked = true;

                if (dr["TaskForAll"].ToString() == "Y")
                    rdoAllTask.Checked = true;
                else
                    rdoMultipleTask.Checked = true;

                if (dr["EnrollType"].ToString() == "E")
                    rdoEnrolled.Checked = true;
                else if (dr["EnrollType"].ToString() == "U")
                    rdounEnrolled.Checked = true;
                else if (dr["EnrollType"].ToString() == "A")
                    rdoEnrollBoth.Checked = true;

                chkbExcel.Checked = false;
                if (dr["EXCEL"].ToString() == "Y")

                    CommonFunctions.SetComboBoxValue(cmbFundingSource, dr["FundedSource"].ToString());

                if (dr["EnrollType"].ToString() == "E")
                {
                    if (dr["FundedSource"].ToString() == "Y")
                    {
                        // rdoFundingAll.Checked = true;
                    }
                    else if (dr["FundedSource"].ToString() == "D")
                    {
                        //rdoFundingSelect.Checked = true;
                        SelFundingList.Clear();
                        string strFunds = dr["FundingCode"].ToString();
                        List<string> siteList = new List<string>();
                        if (strFunds != null)
                        {
                            string[] FundCodes = strFunds.Split(',');
                            for (int i = 0; i < FundCodes.Length; i++)
                            {
                                CommonEntity fundDetails = propfundingSource.Find(u => u.Code == FundCodes.GetValue(i).ToString());
                                if (fundDetails != null)
                                    SelFundingList.Add(fundDetails);
                            }
                        }
                        SelFundingList = SelFundingList;
                    }
                }

            }
        }

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
                    strsiteRoomNames = "A";
                    CaseSiteEntity searchEntity = new CaseSiteEntity(true);
                    searchEntity.SiteAGENCY = Agency; searchEntity.SiteDEPT = Dept; searchEntity.SitePROG = Prog;
                    searchEntity.SiteYEAR = DepYear;
                    List<CaseSiteEntity> AllSites = _model.CaseMstData.Browse_CASESITE(searchEntity, "Browse");
                    if (BaseForm.BaseAgencyControlDetails.SiteSecurity == "1")
                    {
                        List<HierarchyEntity> userHierarchy = _model.UserProfileAccess.GetUserHierarchyByID(BaseForm.UserID);
                        HierarchyEntity hierarchyEntity = new HierarchyEntity();
                        foreach (HierarchyEntity Entity in userHierarchy)
                        {
                            if (Entity.InActiveFlag == "N")
                            {
                                if (Entity.Agency == Agency && Entity.Dept == Dept && Entity.Prog == Prog)
                                    hierarchyEntity = Entity;
                                else if (Entity.Agency == Agency && Entity.Dept == Dept && Entity.Prog == "**")
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


                string strFundingCodes = string.Empty;
                if (((Captain.Common.Utilities.ListItem)cmbFundingSource.SelectedItem).Value.ToString().Trim() == "D")
                {
                    foreach (CommonEntity FundingCode in SelFundingList)
                    {
                        if (!strFundingCodes.Equals(string.Empty)) strFundingCodes += ",";
                        strFundingCodes += FundingCode.Code;
                    }
                }
                else if (((Captain.Common.Utilities.ListItem)cmbFundingSource.SelectedItem).Value.ToString().Trim() == "H")
                {
                    strFundingCodes = "HS";
                }
                else
                {
                    strFundingCodes = "A";
                }


                string strSequence = string.Empty;
                if (rdoApplicantNumseq.Checked == true)
                    strSequence = "NUMBER";
                else if (rdoApplicantNameSeq.Checked == true)
                    strSequence = "NAME";
                else if (rdoClassSeq.Checked == true)
                    strSequence = "CLASS";
                else if (rdoSelectedApplicantSeq.Checked == true)
                    strSequence = "APPLICANT";


                string Task = "A";
                string Chidrenwith = string.Empty;
                propCaseEnrlList = _model.EnrollData.GetEnrollDetails2114(Agency, Dept, Prog, Program_Year, txtApplicant.Text.Trim(), (rdoTodayDate.Checked == true ? "T" : "K"), strsiteRoomNames, strFundingCodes, Task, Chidrenwith, strSequence);
                propchldtrackRList = _model.ChldTrckData.GetCasetrckrDetails(string.Empty, string.Empty, string.Empty, ((Captain.Common.Utilities.ListItem)CmbTrackComp.SelectedItem).Value.ToString(), string.Empty, string.Empty);
                //PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, false, propReportPath);
                //pdfListForm.FormClosed += new Form.FormClosedEventHandler(On_SaveFormClosed);
                //pdfListForm.ShowDialog();

                if (chkbExcel.Checked == true)
                {
                    PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, false, propReportPath, "XLS");
                    pdfListForm.FormClosed += new FormClosedEventHandler(On_SaveFormClosed);
                    pdfListForm.FormClosed += new FormClosedEventHandler(On_SaveExcelClosed);
                    pdfListForm.StartPosition = FormStartPosition.CenterScreen;
                    pdfListForm.ShowDialog();
                }
                else
                {
                    PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, false, propReportPath, "PDF");
                    pdfListForm.FormClosed += new FormClosedEventHandler(On_SaveFormClosed);
                    pdfListForm.StartPosition = FormStartPosition.CenterScreen;
                    pdfListForm.ShowDialog();
                }
            }
        }


        #region PrintingReportDetails

        PdfContentByte cb;
        int X_Pos, Y_Pos;
        string Random_Filename = null; string PdfName = "Pdf File";
        BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);

        private void On_SaveFormClosed(object sender, FormClosedEventArgs e)
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
                //document.SetPageSize(iTextSharp.text.PageSize.LETTER.Rotate());
                //PdfWriter writer = PdfWriter.GetInstance(document, fs);
                document.Open();
                BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
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

                try
                {

                    PrintHeaderPage(document, writer);


                    document.NewPage();
                    Y_Pos = 795;




                    PdfPTable hssb2103_Table2 = new PdfPTable(8);
                    hssb2103_Table2.TotalWidth = 550f;
                    hssb2103_Table2.WidthPercentage = 100;
                    hssb2103_Table2.LockedWidth = true;
                    float[] widths2 = new float[] { 30f, 65f, 30f, 30f, 45f, 30f, 30f, 45f, };
                    hssb2103_Table2.SetWidths(widths2);
                    hssb2103_Table2.HorizontalAlignment = Element.ALIGN_CENTER;
                    hssb2103_Table2.HeaderRows = 3;

                    string strTaskCodes = string.Empty;
                    List<ChldTrckEntity> Track_allEntity;
                    if (rdoAllTask.Checked)
                    {

                        List<ChldTrckEntity> TrackList = _model.ChldTrckData.Browse_CasetrckDetails("01");
                        List<ChldTrckEntity> Track_Det = new List<ChldTrckEntity>();
                        if (((Captain.Common.Utilities.ListItem)CmbTrackComp.SelectedItem).Value.ToString().Trim() == "0000")
                        {
                            Track_Det = TrackList.FindAll(u => u.COMPONENT.Trim().Equals("0000"));
                        }
                        else
                            Track_Det = TrackList.FindAll(u => u.COMPONENT.Trim().Equals(((Captain.Common.Utilities.ListItem)CmbTrackComp.SelectedItem).Value.ToString().Trim()) && u.Agency.Trim().Equals(BaseForm.BaseAgency.Trim()) && u.Dept.Equals(BaseForm.BaseDept.Trim()) && u.Prog.Equals(BaseForm.BaseProg.Trim()));

                        foreach (ChldTrckEntity Entity in Track_Det)
                        {
                            if (!strTaskCodes.Equals(string.Empty)) strTaskCodes += ",";
                            strTaskCodes += Entity.TASK;
                        }
                        Track_allEntity = Track_Det;

                    }
                    else
                    {
                        foreach (ChldTrckEntity Entity in SeltrackList)
                        {
                            if (!strTaskCodes.Equals(string.Empty)) strTaskCodes += ",";
                            strTaskCodes += Entity.TASK;
                        }
                        Track_allEntity = SeltrackList;
                    }

                    List<CaseEnrlEntity> caseEnrlList;
                    if (((Captain.Common.Utilities.ListItem)cmbActive.SelectedItem).Value.ToString() == "B")
                    {
                        caseEnrlList = propCaseEnrlList.FindAll(u => Convert.ToInt32(u.Snp_Age) >= Convert.ToInt32(txtFrom.Text) && Convert.ToInt32(u.Snp_Age) <= Convert.ToInt32(txtTo.Text));
                    }
                    else
                    {
                        caseEnrlList = propCaseEnrlList.FindAll(u => u.MST_ActiveStatus == ((Captain.Common.Utilities.ListItem)cmbActive.SelectedItem).Value.ToString() && Convert.ToInt32(u.Snp_Age) >= Convert.ToInt32(txtFrom.Text) && Convert.ToInt32(u.Snp_Age) <= Convert.ToInt32(txtTo.Text));
                    }

                    if (rdoEnrolled.Checked)
                        caseEnrlList = caseEnrlList.FindAll(u => u.Status.Trim().Equals("E"));
                    if (rdounEnrolled.Checked)
                        caseEnrlList = caseEnrlList.FindAll(u => u.Status.Trim() != "E");
                    if (caseEnrlList.Count > 0)
                    {
                        propChldMediEntity = _model.ChldTrckData.GetChldMedi_Report(Agency, Dept, Prog, "****", (txtApplicant.Text == string.Empty ? string.Empty : txtApplicant.Text), (dtSBCB.Checked == true ? dtSBCB.Value.ToShortDateString() : string.Empty), strTaskCodes, "Task", "TaskDesc");

                        //propChldMediEntity = propChldMediEntity.FindAll(u=>u.SBCB_DATE.Trim()!=string.Empty);


                        PdfPCell Header1 = new PdfPCell(new Phrase("Applicant", TblFontBold));
                        Header1.HorizontalAlignment = Element.ALIGN_CENTER;
                        Header1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        Header1.Colspan = 2;
                        hssb2103_Table2.AddCell(Header1);


                        PdfPCell Header2 = new PdfPCell(new Phrase("Enrollment", TblFontBold));
                        Header2.HorizontalAlignment = Element.ALIGN_CENTER;
                        Header2.Colspan = 6;
                        Header2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        hssb2103_Table2.AddCell(Header2);

                        string[] Headers = { "Number", "Name   ", "Date", "Class", "Funding", "Date", "Class", "Funding" };

                        for (int i = 0; i < Headers.Length; i++)
                        {

                            PdfPCell Header21 = new PdfPCell(new Phrase(Headers[i], TblFontBold));
                            Header21.HorizontalAlignment = Element.ALIGN_CENTER;
                            Header21.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            hssb2103_Table2.AddCell(Header21);
                        }

                        PdfPCell Header3 = new PdfPCell(new Phrase("", TblFontBold));
                        Header3.HorizontalAlignment = Element.ALIGN_CENTER;
                        Header3.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                        Header3.FixedHeight = 10f;
                        Header3.Colspan = 8;
                        hssb2103_Table2.AddCell(Header3);

                        foreach (CaseEnrlEntity enrlrow in caseEnrlList)
                        {



                            List<ChldMediEntity> chldMediEntity = propChldMediEntity.FindAll(u => (u.AGENCY == enrlrow.Agy) && (u.DEPT == enrlrow.Dept) && (u.PROG == enrlrow.Prog) && (u.APP_NO == enrlrow.App));//&& (u.YEAR == enrlrow.Year)

                            DataTable dtMediTask = CreateTable();
                            DataRow dtrow;
                            bool boolprintstatus = true;
                            string strAddresDt = string.Empty;
                            string strFollowDt = string.Empty;
                            string strCompledt = string.Empty;
                            string strSbcbdt = string.Empty;
                            List<ChldTrckEntity> chldTrckNewList = new List<ChldTrckEntity>();
                            foreach (ChldTrckEntity trackitem in Track_allEntity)
                            {
                                try
                                {
                                    strAddresDt = string.Empty;
                                    strFollowDt = string.Empty;
                                    strCompledt = string.Empty;
                                    List<ChldTrckREntity> chldtrackRList = propchldtrackRList.FindAll(u => u.TASK == trackitem.TASK);
                                    ChldTrckREntity chldtrackRListItem = propchldtrackRList.Find(u => u.TASK == trackitem.TASK && u.FUND.Trim().ToString() == enrlrow.FundHie.Trim().ToString());



                                    //if (trackitem.TASK.ToString() == "0320")
                                    //{

                                    //}




                                    //foreach (ChldMediEntity chlditem in chldMediEntity)
                                    //{

                                    List<ChldMediEntity> chlditem = chldMediEntity.FindAll(u => u.TASK.Trim() == trackitem.TASK);
                                    if (chlditem.Count > 0)
                                    {
                                        chlditem = chlditem.OrderByDescending(u => Convert.ToInt32(u.YEAR)).ThenByDescending(u => Convert.ToInt32(u.SEQ)).ToList();//(u => Convert.ToInt32(u.SEQ)).ThenByDescending(u => u.YEAR).ToList();
                                        //if (chlditem.Count == 1)
                                        //{
                                        if (chlditem[0].COMPLETED_DATE == string.Empty)
                                        {
                                            strAddresDt = chlditem[0].ADDRESSED_DATE;
                                            strFollowDt = chlditem[0].FOLLOWUP_DATE;
                                            strCompledt = string.Empty;
                                            string strAddresFollwdt = "    ";
                                            string strSBCBDate = string.Empty;
                                            if (chlditem[0].ADDRESSED_DATE != string.Empty)
                                                strAddresFollwdt = strAddresFollwdt + "ADDRESSED: " + LookupDataAccess.Getdate(chlditem[0].ADDRESSED_DATE) + "    ";
                                            if (chlditem[0].FOLLOWUP_DATE != string.Empty)
                                                strAddresFollwdt = strAddresFollwdt + "FOLLOW UP: " + LookupDataAccess.Getdate(chlditem[0].FOLLOWUP_DATE);
                                            //bool boolTrackRecord = true;
                                            //foreach (DataRow item in dtMediTask.Rows)
                                            //{
                                            //    if (item["TaskId"] == trackitem.TASK.ToString())
                                            //    {
                                            //        strSBCBDate = LookupDataAccess.Getdate(FillSBCDDetails(trackitem.TASK, enrlrow, chldtrackRList));
                                            //        if (strSBCBDate != string.Empty)
                                            //        {
                                            //            item["TaskDesc"] = chlditem[0].Task_Desc + "  : SBCB: " + LookupDataAccess.Getdate(strSBCBDate) + strAddresFollwdt;
                                            //            item["SBCBdt"] = LookupDataAccess.Getdate(strSBCBDate);
                                            //        }
                                            //        boolTrackRecord = false;
                                            //        break;
                                            //    }


                                            //}
                                            //if (boolTrackRecord)
                                            //{
                                            //    dtrow = dtMediTask.NewRow();
                                            //    dtrow["CustQues"] = chlditem[0].CustQuestions.Trim();
                                            //    dtrow["TaskId"] = trackitem.TASK.ToString();
                                            //    strSBCBDate = LookupDataAccess.Getdate(FillSBCDDetails(trackitem.TASK, enrlrow, chldtrackRList));
                                            //    if (strSBCBDate != string.Empty)
                                            //    {
                                            //        dtrow["TaskDesc"] = chlditem[0].Task_Desc + "  : SBCB: " + LookupDataAccess.Getdate(strSBCBDate) + strAddresFollwdt;
                                            //        dtrow["SBCBdt"] = LookupDataAccess.Getdate(strSBCBDate);
                                            //    }

                                            //    if (strSBCBDate != string.Empty)
                                            //        dtMediTask.Rows.Add(dtrow);
                                            //}

                                        }
                                        else
                                        {

                                            strAddresDt = string.Empty; //chlditem[0].ADDRESSED_DATE;
                                            strFollowDt = chlditem[0].FOLLOWUP_DATE;
                                            strCompledt = chlditem[0].COMPLETED_DATE;
                                            ChldTrckREntity chldtrckrNextyear = chldtrackRList.Find(u => u.FUND.Trim().ToString() == enrlrow.FundHie.ToString());

                                            string strDate = string.Empty;
                                            if (chldtrckrNextyear != null)
                                            {


                                                if (chldtrckrNextyear.NXTACTION.Trim() == "Y")
                                                {
                                                    if (!string.IsNullOrEmpty(chldtrckrNextyear.NEXTDAYS))
                                                    {
                                                        if (Convert.ToInt32(chldtrckrNextyear.NEXTDAYS) >= 0)
                                                        {
                                                            if (chldtrckrNextyear.INTAKEENTRY.Trim().Equals("A"))
                                                            {
                                                                if (!string.IsNullOrEmpty(enrlrow.MST_INTAKE_DT))
                                                                    strDate = Convert.ToDateTime(enrlrow.MST_INTAKE_DT).AddDays(Convert.ToInt32(chldtrckrNextyear.NEXTDAYS)).ToString();
                                                                // break;
                                                            }
                                                            else if (chldtrckrNextyear.INTAKEENTRY.Trim().Equals("E"))
                                                            {
                                                                //  strAttnDate = ChldAttnDB.GetChldAttnDate(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, strApplicationNo, caseEnrlCheckRecord.Site, caseEnrlCheckRecord.Room, caseEnrlCheckRecord.AMPM, caseEnrlCheckRecord.FundHie, string.Empty);
                                                                if (!string.IsNullOrEmpty(enrlrow.AttendedDt))
                                                                    strDate = Convert.ToDateTime(enrlrow.AttendedDt).AddDays(Convert.ToInt32(chldtrckrNextyear.NEXTDAYS)).ToString();
                                                                if (!string.IsNullOrEmpty(enrlrow.AttendedDt))
                                                                    strDate = Convert.ToDateTime(enrlrow.AttendedDt).AddDays(Convert.ToInt32(chldtrckrNextyear.NEXTDAYS)).ToString();
                                                            }
                                                            else if (chldtrckrNextyear.INTAKEENTRY.Trim().Equals("D"))
                                                            {

                                                                strDate = Convert.ToDateTime(enrlrow.Snp_DOB).AddDays(Convert.ToInt32(chldtrckrNextyear.NEXTDAYS)).ToString();
                                                                // break;
                                                            }
                                                        }
                                                        // break;
                                                    }

                                                    //ChldTrckEntity chldNextTaskEntity = Track_allEntity.Find(u => u.TASK == chldtrckrNextyear.NEXTTASK);
                                                    //if (chldNextTaskEntity != null)
                                                    //{
                                                    //    bool boolTrackRecord = true;
                                                    //    foreach (DataRow item in dtMediTask.Rows)
                                                    //    {
                                                    //        if (item["TaskId"] == trackitem.TASK.ToString())
                                                    //        {
                                                    //            string strSBCBDate = LookupDataAccess.Getdate(FillSBCDDetails(trackitem.TASK, enrlrow, chldtrackRList));
                                                    //            if (strSBCBDate != string.Empty)
                                                    //            {
                                                    //                item["TaskDesc"] = chlditem[0].Task_Desc + "  : SBCB: " + LookupDataAccess.Getdate(strSBCBDate);
                                                    //                item["SBCBdt"] = LookupDataAccess.Getdate(strSBCBDate);
                                                    //            }
                                                    //            boolTrackRecord = false;
                                                    //            break;
                                                    //        }


                                                    //    }
                                                    //    if (boolTrackRecord)
                                                    //    {
                                                    //        dtrow = dtMediTask.NewRow();
                                                    //        dtrow["TaskId"] = chldNextTaskEntity.TASK.ToString();
                                                    //        dtrow["CustQues"] = chldNextTaskEntity.CustQCodes.Trim();
                                                    //        // dtrow["TaskDesc"] = chldNextTaskEntity.TASKDESCRIPTION + "  : SBCB: " + strSBCBDate;

                                                    //        // strSBCBDate = LookupDataAccess.Getdate(FillSBCDDetails(trackitem.TASK, enrlrow, chldtrackRList));
                                                    //        if (strDate != string.Empty)
                                                    //        {
                                                    //            dtrow["TaskDesc"] = chldNextTaskEntity.TASKDESCRIPTION + "  : SBCB: " + LookupDataAccess.Getdate(strDate);
                                                    //            dtrow["SBCBdt"] = LookupDataAccess.Getdate(strDate);
                                                    //        }

                                                    //        if (strDate != string.Empty)
                                                    //            dtMediTask.Rows.Add(dtrow);
                                                    //    }
                                                    //}

                                                }
                                            }
                                        }
                                        // }
                                        //else
                                        //{
                                        //    List<ChldMediEntity> chldorderbyitem = chlditem.OrderByDescending(u => u.YEAR).ThenBy(u=>u.SEQ).ToList();
                                        //    //chldorderbyitem = chlditem.OrderByDescending(u => u.YEAR).ToList(); 
                                        //}
                                    }
                                    //else
                                    //{
                                    //     bool boolTrackRecord = true;
                                    //        foreach (DataRow item in dtMediTask.Rows)
                                    //        {
                                    //            if (item["TaskId"] == trackitem.TASK.ToString())
                                    //            {
                                    //              string strSBCBDate = LookupDataAccess.Getdate(FillSBCDDetails(trackitem.TASK, enrlrow, chldtrackRList));
                                    //                if (strSBCBDate != string.Empty)
                                    //                {
                                    //                    item["TaskDesc"] = chlditem[0].Task_Desc + "  : SBCB: " + LookupDataAccess.Getdate(strSBCBDate) ;
                                    //                    item["SBCBdt"] = LookupDataAccess.Getdate(strSBCBDate);
                                    //                }
                                    //                boolTrackRecord = false;
                                    //                break;
                                    //            }


                                    //        }
                                    //        if (boolTrackRecord)
                                    //        {
                                    //            string strSBCBDate = LookupDataAccess.Getdate(FillSBCDDetails(trackitem.TASK, enrlrow, chldtrackRList));
                                    //            if (strSBCBDate != string.Empty)
                                    //            {
                                    //                dtrow = dtMediTask.NewRow();

                                    //                dtrow["CustQues"] = trackitem.CustQCodes.Trim();
                                    //                dtrow["TaskId"] = trackitem.TASK.ToString();
                                    //                dtrow["TaskDesc"] = trackitem.TASKDESCRIPTION + "  : SBCB: " + strSBCBDate;

                                    //                dtrow["SBCBdt"] = LookupDataAccess.Getdate(strSBCBDate);

                                    //                dtMediTask.Rows.Add(dtrow);

                                    //                //PdfPCell pdfData1 = new PdfPCell(new Phrase((trackitem.TASKDESCRIPTION + "  : SBCB: " + strSBCBDate), TableFont));
                                    //                //pdfData1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    //                //pdfData1.Colspan = 8;
                                    //                //pdfData1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //                //hssb2103_Table2.AddCell(pdfData1);

                                    //                //fillCustQuestions(trackitem.CustQCodes.Trim(), hssb2103_Table2);


                                    //            }
                                    //        }
                                    //}

                                    //if (trackitem.NEXTYN == "Y")
                                    //{ 

                                    //}

                                    if (chldtrackRListItem != null)
                                    {
                                        chldtrackRListItem.CompletDt = strCompledt;
                                        strSbcbdt = LookupDataAccess.Getdate(FillSBCDDetails(trackitem.TASK, enrlrow, chldtrackRListItem));
                                        //if (chldtrackRListItem.TASK.ToString() == "0320")
                                        //{

                                        //}

                                        chldTrckNewList.Add(new ChldTrckEntity(trackitem.TASK, trackitem.TASKDESCRIPTION, chldtrackRListItem.REQUIREYEAR, chldtrackRListItem.ENTERDAYS, chldtrackRListItem.NXTACTION, chldtrackRListItem.NEXTTASK, chldtrackRListItem.NEXTDAYS, strSbcbdt, strCompledt, strFollowDt, strAddresDt, chldtrackRListItem.INTAKEENTRY, trackitem.CustQCodes));
                                    }
                                    else
                                    {
                                        chldTrckNewList.Add(new ChldTrckEntity(trackitem.TASK, trackitem.TASKDESCRIPTION, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, trackitem.CustQCodes));
                                    }

                                    //PdfPCell pdfSpace4 = new PdfPCell(new Phrase("", TableFont));
                                    //pdfSpace4.HorizontalAlignment = Element.ALIGN_LEFT;
                                    //pdfSpace4.Colspan = 8;
                                    //pdfSpace4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //hssb2103_Table2.AddCell(pdfSpace4);

                                    //PdfPCell pdfSpace5 = new PdfPCell(new Phrase("", TableFont));
                                    //pdfSpace5.HorizontalAlignment = Element.ALIGN_LEFT;
                                    //pdfSpace5.Colspan = 8;
                                    //pdfSpace5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //hssb2103_Table2.AddCell(pdfSpace5);


                                    //PdfPCell pdfSpace6 = new PdfPCell(new Phrase("", TableFont));
                                    //pdfSpace6.HorizontalAlignment = Element.ALIGN_LEFT;
                                    //pdfSpace6.Colspan = 8;
                                    //pdfSpace6.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //hssb2103_Table2.AddCell(pdfSpace6);


                                    //PdfPCell pdfData3 = new PdfPCell(new Phrase("Result: (OT) _________________  Date_________________________________", TableFont));
                                    //pdfData3.HorizontalAlignment = Element.ALIGN_LEFT;
                                    //pdfData3.Colspan = 4;
                                    //pdfData3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //hssb2103_Table2.AddCell(pdfData3);

                                    //PdfPCell pdfData4 = new PdfPCell(new Phrase("Comments______________________________________________________", TableFont));
                                    //pdfData4.HorizontalAlignment = Element.ALIGN_LEFT;
                                    //pdfData4.Colspan = 4;
                                    //pdfData4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //hssb2103_Table2.AddCell(pdfData4);

                                    //PdfPCell pdfSpace7 = new PdfPCell(new Phrase("", TableFont));
                                    //pdfSpace7.HorizontalAlignment = Element.ALIGN_LEFT;
                                    //pdfSpace7.Colspan = 8;
                                    //pdfSpace7.FixedHeight = 10f;
                                    //pdfSpace7.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //hssb2103_Table2.AddCell(pdfSpace7);


                                }
                                catch (Exception ex)
                                {

                                    // throw;
                                }

                                //}
                            }


                            foreach (ChldTrckEntity item in chldTrckNewList)
                            {

                                //if (item.TASK.ToString() == "0320")
                                //{

                                //}


                                if (item.NEXTYN.Trim().ToString() == "Y" && item.CompletDT.ToString() != string.Empty)
                                {
                                    if (!string.IsNullOrEmpty(item.NEXTDAYS))
                                    {
                                        string strSbcbdate = string.Empty;
                                        if (Convert.ToInt32(item.NEXTDAYS) >= 0)
                                        {
                                            if (item.IntakeType.Trim().Equals("A"))
                                            {
                                                if (!string.IsNullOrEmpty(enrlrow.MST_INTAKE_DT))
                                                    strSbcbdate = Convert.ToDateTime(enrlrow.MST_INTAKE_DT).AddDays(Convert.ToInt32(item.NEXTDAYS)).ToString();
                                                // break;
                                            }
                                            else if (item.IntakeType.Trim().Equals("E"))
                                            {
                                                //  strAttnDate = ChldAttnDB.GetChldAttnDate(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, strApplicationNo, caseEnrlCheckRecord.Site, caseEnrlCheckRecord.Room, caseEnrlCheckRecord.AMPM, caseEnrlCheckRecord.FundHie, string.Empty);
                                                if (!string.IsNullOrEmpty(enrlrow.AttendedDt))
                                                    strSbcbdate = Convert.ToDateTime(enrlrow.AttendedDt).AddDays(Convert.ToInt32(item.NEXTDAYS)).ToString();
                                                if (!string.IsNullOrEmpty(enrlrow.AttendedDt))
                                                    strSbcbdate = Convert.ToDateTime(enrlrow.AttendedDt).AddDays(Convert.ToInt32(item.NEXTDAYS)).ToString();
                                            }
                                            else if (item.IntakeType.Trim().Equals("D"))
                                            {

                                                strSbcbdate = Convert.ToDateTime(enrlrow.Snp_DOB).AddDays(Convert.ToInt32(item.NEXTDAYS)).ToString();
                                                // break;
                                            }
                                        }

                                        if (strSbcbdate != string.Empty)
                                        {
                                            if (item.TASK.ToString() == item.NEXTTASK.ToString())
                                            {
                                                if (item.CompletDT.ToString() != string.Empty)
                                                {
                                                    DateTime dtcomple = Convert.ToDateTime(item.CompletDT).Date;
                                                    DateTime dtSbcb = Convert.ToDateTime(strSbcbdate).Date;
                                                    if (dtcomple < dtSbcb)
                                                    {
                                                        item.CompletDT = string.Empty;
                                                        item.SBCBDT = dtSbcb.Date.ToShortDateString();
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                ChldTrckEntity chldtrckNewTaskItem = chldTrckNewList.Find(u => u.TASK == item.NEXTTASK);
                                                if (chldtrckNewTaskItem != null)
                                                {
                                                    if (chldtrckNewTaskItem.CompletDT.ToString() != string.Empty)
                                                    {
                                                        DateTime dtcomple = Convert.ToDateTime(item.CompletDT).Date;
                                                        DateTime dtSbcb = Convert.ToDateTime(strSbcbdate).Date;
                                                        if (dtcomple < dtSbcb)
                                                        {
                                                            chldTrckNewList.Find(u => u.TASK == item.NEXTTASK).CompletDT = string.Empty;
                                                            chldTrckNewList.Find(u => u.TASK == item.NEXTTASK).SBCBDT = dtSbcb.Date.ToShortDateString();
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        // break;
                                    }
                                }


                            }

                            List<ChldTrckEntity> chldtrcknewcount = chldTrckNewList.FindAll(u => u.SBCBDT.ToString() != string.Empty && u.CompletDT.ToString() == string.Empty);


                            chldtrcknewcount = chldtrcknewcount.OrderBy(u => Convert.ToDateTime(u.SBCBDT)).ToList();

                            //DataView dv = dtMediTask.DefaultView;
                            //dv.Sort = "SBCBDt asc";
                            //DataTable dtnew = dv.ToTable();

                            if (dtSBCB.Checked == true)
                            {
                                chldtrcknewcount = chldtrcknewcount.FindAll(u => Convert.ToDateTime(u.SBCBDT) <= Convert.ToDateTime(dtSBCB.Value));
                                //if (Convert.ToDateTime(chldtrcknewcount[0].SBCBDT) <= Convert.ToDateTime(dtSBCB.Value))
                                //{
                                //    boolprintstatus = false;
                                //}
                            }


                            if (chldtrcknewcount.Count > 0)
                            {

                                if (boolprintstatus)
                                {
                                    PdfPCell pdfAppldata = new PdfPCell(new Phrase(enrlrow.App, TableFont));
                                    pdfAppldata.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfAppldata.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    hssb2103_Table2.AddCell(pdfAppldata);


                                    PdfPCell pdfChildName = new PdfPCell(new Phrase(LookupDataAccess.GetMemberName(enrlrow.Snp_F_Name, enrlrow.Snp_M_Name, enrlrow.Snp_L_Name, BaseForm.BaseHierarchyCnFormat), TableFont));
                                    pdfChildName.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfChildName.Colspan = 1;
                                    pdfChildName.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    hssb2103_Table2.AddCell(pdfChildName);

                                    PdfPCell pdfDob = new PdfPCell(new Phrase(LookupDataAccess.Getdate(enrlrow.Enrl_Date), TableFont));
                                    pdfDob.HorizontalAlignment = Element.ALIGN_CENTER;
                                    pdfDob.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    hssb2103_Table2.AddCell(pdfDob);

                                    PdfPCell pdfSiteRoom = new PdfPCell(new Phrase(enrlrow.Site + enrlrow.Room + enrlrow.AMPM, TableFont));
                                    pdfSiteRoom.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfSiteRoom.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    hssb2103_Table2.AddCell(pdfSiteRoom);

                                    PdfPCell pdfFundSource = new PdfPCell(new Phrase((propfundingSource.Find(u => u.Code == enrlrow.FundHie).Desc.ToString()), TableFont));
                                    pdfFundSource.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfFundSource.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    hssb2103_Table2.AddCell(pdfFundSource);

                                    PdfPCell pdfDob2 = new PdfPCell(new Phrase("", TableFont));
                                    pdfDob2.HorizontalAlignment = Element.ALIGN_CENTER;
                                    pdfDob2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    hssb2103_Table2.AddCell(pdfDob2);

                                    PdfPCell pdfSiteRoom2 = new PdfPCell(new Phrase("", TableFont));
                                    pdfSiteRoom2.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfSiteRoom2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    hssb2103_Table2.AddCell(pdfSiteRoom2);

                                    PdfPCell pdfFundSource2 = new PdfPCell(new Phrase("", TableFont));
                                    pdfFundSource2.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfFundSource2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    hssb2103_Table2.AddCell(pdfFundSource2);

                                    PdfPCell pdfSpace = new PdfPCell(new Phrase((""), TableFont));
                                    pdfSpace.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfSpace.Colspan = 8;
                                    pdfSpace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    hssb2103_Table2.AddCell(pdfSpace);

                                    PdfPCell pdfSpace2 = new PdfPCell(new Phrase((""), TableFont));
                                    pdfSpace2.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfSpace2.Colspan = 8;
                                    pdfSpace2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    hssb2103_Table2.AddCell(pdfSpace2);

                                    PdfPCell pdfSpace3 = new PdfPCell(new Phrase("", TableFont));
                                    pdfSpace3.HorizontalAlignment = Element.ALIGN_LEFT;
                                    pdfSpace3.Colspan = 8;
                                    pdfSpace3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    hssb2103_Table2.AddCell(pdfSpace3);
                                }
                            }

                            foreach (ChldTrckEntity dtrowitem in chldtrcknewcount)
                            {
                                strAddresDt = dtrowitem.AddressDT;
                                strFollowDt = dtrowitem.FollowDT;
                                strCompledt = string.Empty;
                                string strAddresFollwdt = "    ";
                                string strSBCBDate = string.Empty;
                                if (dtrowitem.AddressDT != string.Empty)
                                    strAddresFollwdt = strAddresFollwdt + "ADDRESSED: " + LookupDataAccess.Getdate(dtrowitem.AddressDT) + "    ";
                                if (dtrowitem.FollowDT != string.Empty)
                                    strAddresFollwdt = strAddresFollwdt + "FOLLOW UP: " + LookupDataAccess.Getdate(dtrowitem.FollowDT);
                                ;
                                PdfPCell pdfData1 = new PdfPCell(new Phrase((dtrowitem.TASKDESCRIPTION + "  : SBCB: " + dtrowitem.SBCBDT + strAddresFollwdt.ToString()), TableFont));
                                pdfData1.HorizontalAlignment = Element.ALIGN_LEFT;
                                pdfData1.Colspan = 8;
                                pdfData1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                hssb2103_Table2.AddCell(pdfData1);

                                fillCustQuestions(dtrowitem.CustQCodes.ToString(), hssb2103_Table2);
                            }

                            //foreach (DataRow dtrowitem in dtnew.Rows)
                            //{

                            //    PdfPCell pdfData1 = new PdfPCell(new Phrase((dtrowitem["TaskDesc"].ToString()), TableFont));
                            //    pdfData1.HorizontalAlignment = Element.ALIGN_LEFT;
                            //    pdfData1.Colspan = 8;
                            //    pdfData1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            //    hssb2103_Table2.AddCell(pdfData1);

                            //    fillCustQuestions(dtrowitem["CustQues"].ToString(), hssb2103_Table2);
                            //}



                            if (chldtrcknewcount.Count > 0)
                            {
                                PdfPCell pdfChildName2 = new PdfPCell(new Phrase(LookupDataAccess.GetMemberName(enrlrow.Snp_F_Name, enrlrow.Snp_M_Name, enrlrow.Snp_L_Name, BaseForm.BaseHierarchyCnFormat), TableFont));
                                pdfChildName2.HorizontalAlignment = Element.ALIGN_CENTER;
                                pdfChildName2.Colspan = 2;
                                pdfChildName2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                hssb2103_Table2.AddCell(pdfChildName2);

                                PdfPCell pdfTaskCount = new PdfPCell(new Phrase(chldtrcknewcount.Count + "  Tasks   Age : 0" + enrlrow.Snp_Age + "  BD  " + LookupDataAccess.Getdate(enrlrow.Snp_DOB) + "  AS  " + LookupDataAccess.Getdate(enrlrow.MSTZIPDATE), TableFont));
                                pdfTaskCount.HorizontalAlignment = Element.ALIGN_LEFT;
                                pdfTaskCount.Colspan = 6;
                                pdfTaskCount.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                hssb2103_Table2.AddCell(pdfTaskCount);

                                PdfPCell pdfSpace7 = new PdfPCell(new Phrase("", TableFont));
                                pdfSpace7.HorizontalAlignment = Element.ALIGN_LEFT;
                                pdfSpace7.Colspan = 8;
                                pdfSpace7.FixedHeight = 10f;
                                pdfSpace7.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                hssb2103_Table2.AddCell(pdfSpace7);
                            }
                        }
                    }
                    if (hssb2103_Table2.Rows.Count > 0)
                    {

                        document.Add(hssb2103_Table2);
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
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_Times, 13);*/

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

           /* PdfPCell row2 = new PdfPCell(new Phrase("Run By :" + LookupDataAccess.GetMemberName(BaseForm.UserProfile.FirstName.Trim(), BaseForm.UserProfile.MI.Trim(), BaseForm.UserProfile.LastName.Trim(), "3"), TableFont));
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

            /* string Agy = "Agency : All"; string Det = "Dept : All"; string Prg = "Program : All"; string Header_year = string.Empty;
             if (Agency != "**") Agy = "Agency : " + Agency;
             if (Dept != "**") Det = "Dept : " + Dept;
             if (Prog != "**") Prg = "Program : " + Prog;
             if (CmbYear.Visible == true)
                 Header_year = "Year : " + ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString();


             PdfPCell Hierarchy = new PdfPCell(new Phrase(Txt_HieDesc.Text.Trim() + "     " + Header_year, TableFont));
             Hierarchy.HorizontalAlignment = Element.ALIGN_LEFT;
             Hierarchy.Colspan = 2;
             Hierarchy.Border = iTextSharp.text.Rectangle.NO_BORDER;
             Headertable.AddCell(Hierarchy);*/

            string Agy = /*"Agency : */"All"; string Dept = /*"Dept : */"All"; string Prg = /*"Program : */"All"; string Header_year = string.Empty;
            if (Agency != "**") Agy = /*"Agency : " +*/ Agency;
            if (Dept != "**") Dept = /*"Dept : " + */Dept;
            if (Prog != "**") Prg = /*"Program : " +*/ Prog;
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

            string strReportType = string.Empty;

            string strReportSequence = string.Empty;

            if (rdoApplicantNumseq.Checked)
                strReportSequence = "Applicant Number";
            else if (rdoApplicantNameSeq.Checked)
                strReportSequence = "Applicant Name";
            else if (rdoClassSeq.Checked)
                strReportSequence = "Class";
            else if (rdoSelectedApplicantSeq.Checked)
                strReportSequence = "Selected Applicant: " + txtApplicant.Text + "";
            string strSbcbDate = string.Empty;
            if (dtSBCB.Checked)
                strSbcbDate = dtSBCB.Value.ToShortDateString();
            else
                strSbcbDate = "All SBCB dates";


            string strEnrolledstatus = string.Empty;
            if (rdoEnrolled.Checked)
                strEnrolledstatus = "Enrolled Only";
            else if (rdounEnrolled.Checked)
                strEnrolledstatus = "Unenrolled Only";
            else if (rdoEnrollBoth.Checked)
                strEnrolledstatus = "Both Enrolled & Unenrolled";

            PdfPCell R2 = new PdfPCell(new Phrase("  " + "Report Sequence" /*+ strReportSequence*/, TableFont));
            R2.HorizontalAlignment = Element.ALIGN_LEFT;
            R2.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R2.PaddingBottom = 5;
            Headertable.AddCell(R2);

            PdfPCell R22 = new PdfPCell(new Phrase(strReportSequence, TableFont));
            R22.HorizontalAlignment = Element.ALIGN_LEFT;
            R22.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R22.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R22.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R22.PaddingBottom = 5;
            Headertable.AddCell(R22);

            string Site = string.Empty;
            if (rdoAllSite.Checked == true) Site = "For All Sites";
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
                Site =  rdoMultipleSites.Text + " ( " + Selsites + " ) ";
            }

            PdfPCell Rsite = new PdfPCell(new Phrase("  " + "Site", TableFont));
            Rsite.HorizontalAlignment = Element.ALIGN_LEFT;
            Rsite.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            Rsite.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            Rsite.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            Rsite.PaddingBottom = 5;
            Headertable.AddCell(Rsite);

            PdfPCell Rsite1 = new PdfPCell(new Phrase(Site, TableFont));
            Rsite1.HorizontalAlignment = Element.ALIGN_LEFT;
            Rsite1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            Rsite1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            Rsite1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            Rsite1.PaddingBottom = 5;
            Headertable.AddCell(Rsite1);

            PdfPCell Rtrckcomp = new PdfPCell(new Phrase("  " + "Tracking Component" /*+ ((Captain.Common.Utilities.ListItem)CmbTrackComp.SelectedItem).Text.ToString()*/, TableFont));
            Rtrckcomp.HorizontalAlignment = Element.ALIGN_LEFT;
            Rtrckcomp.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            Rtrckcomp.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            Rtrckcomp.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            Rtrckcomp.PaddingBottom = 5;
            Headertable.AddCell(Rtrckcomp);

            PdfPCell Rtrckcomp1 = new PdfPCell(new Phrase(((Captain.Common.Utilities.ListItem)CmbTrackComp.SelectedItem).Text.ToString(), TableFont));
            Rtrckcomp1.HorizontalAlignment = Element.ALIGN_LEFT;
            Rtrckcomp1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            Rtrckcomp1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            Rtrckcomp1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            Rtrckcomp1.PaddingBottom = 5;
            Headertable.AddCell(Rtrckcomp1);


            string strTaskList = string.Empty;
            if (rdoAllTask.Checked)
                strTaskList = "All Tasks";
            else
            {
                string strTaskCodes = string.Empty;
                foreach (ChldTrckEntity Entity in SeltrackList)
                {
                    if (!strTaskCodes.Equals(string.Empty)) strTaskCodes += ",";
                    strTaskCodes += Entity.TASK;
                }
                strTaskList = "Selected Tasks: " + strTaskCodes;
            }


            PdfPCell RTasks = new PdfPCell(new Phrase("  " + "Tasks", TableFont));
            RTasks.HorizontalAlignment = Element.ALIGN_LEFT;
            RTasks.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            RTasks.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            RTasks.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            RTasks.PaddingBottom = 5;
            Headertable.AddCell(RTasks);

            PdfPCell RTasks1 = new PdfPCell(new Phrase(strTaskList, TableFont));
            RTasks1.HorizontalAlignment = Element.ALIGN_LEFT;
            RTasks1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            RTasks1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            RTasks1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            RTasks1.PaddingBottom = 5;
            Headertable.AddCell(RTasks1);

            PdfPCell R4 = new PdfPCell(new Phrase("  " + lblReportFormdt.Text.Trim(), TableFont));
            R4.HorizontalAlignment = Element.ALIGN_LEFT;
            R4.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R4.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R4.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R4.PaddingBottom = 5;
            Headertable.AddCell(R4);

            PdfPCell R44 = new PdfPCell(new Phrase(strSbcbDate, TableFont));
            R44.HorizontalAlignment = Element.ALIGN_LEFT;
            R44.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R44.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R44.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R44.PaddingBottom = 5;
            Headertable.AddCell(R44);

            PdfPCell R5 = new PdfPCell(new Phrase("  " + "Active/Inactive" /*+ ((Captain.Common.Utilities.ListItem)cmbActive.SelectedItem).Text.ToString()*/, TableFont));
            R5.HorizontalAlignment = Element.ALIGN_LEFT;
            R5.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R5.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R5.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R5.PaddingBottom = 5;
            Headertable.AddCell(R5);

            PdfPCell R55 = new PdfPCell(new Phrase(((Captain.Common.Utilities.ListItem)cmbActive.SelectedItem).Text.ToString(), TableFont));
            R55.HorizontalAlignment = Element.ALIGN_LEFT;
            R55.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R55.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R55.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R55.PaddingBottom = 5;
            Headertable.AddCell(R55);

            PdfPCell R6 = new PdfPCell(new Phrase("  " + "Enroll Status" /*+ strEnrolledstatus*/, TableFont));
            R6.HorizontalAlignment = Element.ALIGN_LEFT;
            R6.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R6.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R6.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R6.PaddingBottom = 5;
            Headertable.AddCell(R6);

            PdfPCell R66 = new PdfPCell(new Phrase(strEnrolledstatus, TableFont));
            R66.HorizontalAlignment = Element.ALIGN_LEFT;
            R66.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R66.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R66.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R66.PaddingBottom = 5;
            Headertable.AddCell(R66);

            PdfPCell R8 = new PdfPCell(new Phrase("  " + "Funding Sources" /*+ ((Captain.Common.Utilities.ListItem)cmbFundingSource.SelectedItem).Text.ToString()*/, TableFont));
            R8.HorizontalAlignment = Element.ALIGN_LEFT;
            R8.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R8.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R8.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R8.PaddingBottom = 5;
            Headertable.AddCell(R8);

            PdfPCell R88 = new PdfPCell(new Phrase(((Captain.Common.Utilities.ListItem)cmbFundingSource.SelectedItem).Text.ToString(), TableFont));
            R88.HorizontalAlignment = Element.ALIGN_LEFT;
            R88.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R88.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R88.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R88.PaddingBottom = 5;
            Headertable.AddCell(R88);

            if (((Captain.Common.Utilities.ListItem)cmbFundingSource.SelectedItem).Value.ToString() == "D")
            {
                string strFundingCodes = string.Empty;

                foreach (CommonEntity FundingCode in SelFundingList)
                {
                    if (!strFundingCodes.Equals(string.Empty)) strFundingCodes += ",";
                    strFundingCodes += FundingCode.Code;
                }

                PdfPCell RFunds1 = new PdfPCell(new Phrase("Selected Funds: " + strFundingCodes, TableFont));
                RFunds1.HorizontalAlignment = Element.ALIGN_LEFT;
                RFunds1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                RFunds1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                RFunds1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                RFunds1.PaddingBottom = 5;
                Headertable.AddCell(RFunds1);
            }

            PdfPCell R7 = new PdfPCell(new Phrase("  " + "Children Age" /*+ txtFrom.Text + " to " + txtTo.Text + ", Base On " + (rdoTodayDate.Checked == true ? rdoTodayDate.Text : rdoKindergartenDate.Text)*/, TableFont));
            R7.HorizontalAlignment = Element.ALIGN_LEFT;
            R7.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R7.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R7.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R7.PaddingBottom = 5;
            Headertable.AddCell(R7);

            PdfPCell R77 = new PdfPCell(new Phrase("From: " + txtFrom.Text + "   To: " + txtTo.Text + ",  Base Ages on: " + (rdoTodayDate.Checked == true ? rdoTodayDate.Text : rdoKindergartenDate.Text), TableFont));
            R77.HorizontalAlignment = Element.ALIGN_LEFT;
            R77.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R77.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R77.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R77.PaddingBottom = 5;
            Headertable.AddCell(R77);

            document.Add(Headertable);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase("Generated By : ", fnttimesRoman_Italic), 33, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.GetMemberName(BaseForm.UserProfile.FirstName.Trim(), BaseForm.UserProfile.MI.Trim(), BaseForm.UserProfile.LastName.Trim(), "3"), fnttimesRoman_Italic), 90, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase("Generated On : ", fnttimesRoman_Italic), 410, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase(DateTime.Now.ToString(), fnttimesRoman_Italic), 468, 40, 0);
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
        #region Excel Report

        #region Converted to Carlos by Vikash on 06/21/2023 for adding Params page
        private void GenerateStyles(WorksheetStyleCollection styles)
        {
            // -----------------------------------------------
            //  Default
            // -----------------------------------------------
            WorksheetStyle Default = styles.Add("Default");
            Default.Name = "Normal";
            Default.Font.FontName = "Arial";
            Default.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s16
            // -----------------------------------------------
            WorksheetStyle s16 = styles.Add("s16");
            s16.Font.Bold = true;
            s16.Font.FontName = "Calibri Light";
            s16.Font.Size = 16;
            s16.Interior.Color = "#DDEBF7";
            s16.Interior.Pattern = StyleInteriorPattern.Solid;
            s16.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s16.Alignment.Vertical = StyleVerticalAlignment.Center;
            // -----------------------------------------------
            //  s17
            // -----------------------------------------------
            WorksheetStyle s17 = styles.Add("s17");
            s17.Font.Bold = true;
            s17.Font.FontName = "Calibri Light";
            s17.Font.Size = 16;
            s17.Interior.Color = "#DDEBF7";
            s17.Interior.Pattern = StyleInteriorPattern.Solid;
            s17.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s17.Alignment.Vertical = StyleVerticalAlignment.Center;
            // -----------------------------------------------
            //  s18
            // -----------------------------------------------
            WorksheetStyle s18 = styles.Add("s18");
            s18.Interior.Color = "#DDEBF7";
            s18.Interior.Pattern = StyleInteriorPattern.Solid;
            s18.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s18.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s19
            // -----------------------------------------------
            WorksheetStyle s19 = styles.Add("s19");
            s19.Interior.Color = "#DDEBF7";
            s19.Interior.Pattern = StyleInteriorPattern.Solid;
            // -----------------------------------------------
            //  s20
            // -----------------------------------------------
            WorksheetStyle s20 = styles.Add("s20");
            s20.Interior.Color = "#DDEBF7";
            s20.Interior.Pattern = StyleInteriorPattern.Solid;
            s20.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s20.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s21
            // -----------------------------------------------
            WorksheetStyle s21 = styles.Add("s21");
            s21.Font.Bold = true;
            s21.Font.FontName = "Calibri";
            s21.Font.Size = 11;
            s21.Font.Color = "#000000";
            s21.Interior.Color = "#FCE4D6";
            s21.Interior.Pattern = StyleInteriorPattern.Solid;
            s21.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s21.Alignment.Vertical = StyleVerticalAlignment.Center;
            s21.Alignment.WrapText = true;
            s21.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s21.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s21.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s21.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s21.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s22
            // -----------------------------------------------
            WorksheetStyle s22 = styles.Add("s22");
            s22.Font.Bold = true;
            s22.Font.FontName = "Calibri";
            s22.Font.Size = 11;
            s22.Font.Color = "#000000";
            s22.Interior.Color = "#FCE4D6";
            s22.Interior.Pattern = StyleInteriorPattern.Solid;
            s22.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s22.Alignment.Vertical = StyleVerticalAlignment.Center;
            s22.Alignment.WrapText = true;
            s22.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s22.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s22.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s22.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s22.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s23
            // -----------------------------------------------
            WorksheetStyle s23 = styles.Add("s23");
            s23.Font.Bold = true;
            s23.Font.FontName = "Calibri";
            s23.Font.Size = 11;
            // -----------------------------------------------
            //  s24
            // -----------------------------------------------
            WorksheetStyle s24 = styles.Add("s24");
            s24.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s24.Alignment.Vertical = StyleVerticalAlignment.Center;
            s24.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s24.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s24.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s24.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s25
            // -----------------------------------------------
            WorksheetStyle s25 = styles.Add("s25");
            s25.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s25.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s25.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s25.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s25.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s25.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s26
            // -----------------------------------------------
            WorksheetStyle s26 = styles.Add("s26");
            s26.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s26.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s26.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s26.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s27
            // -----------------------------------------------
            WorksheetStyle s27 = styles.Add("s27");
            s27.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s27.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s27.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s27.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s27.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s27.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            s27.NumberFormat = "General Date";
            // -----------------------------------------------
            //  s28
            // -----------------------------------------------
            WorksheetStyle s28 = styles.Add("s28");
            s28.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s28.Alignment.Vertical = StyleVerticalAlignment.Center;
            s28.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s28.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s28.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s28.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            s28.NumberFormat = "General Date";
            // -----------------------------------------------
            //  s29
            // -----------------------------------------------
            WorksheetStyle s29 = styles.Add("s29");
            s29.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s29.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s29.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s29.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s29.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s29.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s30
            // -----------------------------------------------
            WorksheetStyle s30 = styles.Add("s30");
            s30.Font.FontName = "Arial";
            s30.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s30.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s30.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s30.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s30.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s30.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s31
            // -----------------------------------------------
            WorksheetStyle s31 = styles.Add("s31");
            s31.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s31.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s32
            // -----------------------------------------------
            WorksheetStyle s32 = styles.Add("s32");
            s32.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s32.Alignment.Vertical = StyleVerticalAlignment.Center;
            // -----------------------------------------------
            //  s33
            // -----------------------------------------------
            WorksheetStyle s33 = styles.Add("s33");
            s33.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s33.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s34
            // -----------------------------------------------
            WorksheetStyle s34 = styles.Add("s34");
            s34.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s34.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s35
            // -----------------------------------------------
            WorksheetStyle s35 = styles.Add("s35");
            // -----------------------------------------------
            //  s36
            // -----------------------------------------------
            WorksheetStyle s36 = styles.Add("s36");
            s36.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s36.Alignment.Vertical = StyleVerticalAlignment.Center;
            // -----------------------------------------------
            //  s37
            // -----------------------------------------------
            WorksheetStyle s37 = styles.Add("s37");
            s37.Font.FontName = "Arial";
            s37.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s37.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s38
            // -----------------------------------------------
            WorksheetStyle s38 = styles.Add("s38");
            s38.Font.FontName = "Arial";
            s38.Font.Color = "#000000";
            s38.Interior.Color = "#FFFFFF";
            s38.Interior.Pattern = StyleInteriorPattern.Solid;
            s38.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s38.Alignment.Vertical = StyleVerticalAlignment.Center;
            s38.Alignment.WrapText = true;
            s38.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            // -----------------------------------------------
            //  s39
            // -----------------------------------------------
            WorksheetStyle s39 = styles.Add("s39");
            s39.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s39.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s40
            // -----------------------------------------------
            WorksheetStyle s40 = styles.Add("s40");
            s40.Font.FontName = "Arial";
            s40.Font.Color = "#000000";
            s40.Interior.Color = "#FFFFFF";
            s40.Interior.Pattern = StyleInteriorPattern.Solid;
            s40.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s40.Alignment.Vertical = StyleVerticalAlignment.Center;
            s40.Alignment.WrapText = true;
            s40.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s40.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s40.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s40.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s40.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s41
            // -----------------------------------------------
            WorksheetStyle s41 = styles.Add("s41");
            s41.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            s41.Alignment.Vertical = StyleVerticalAlignment.Center;
            s41.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s41.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s41.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s41.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

            // -----------------------------------------------
            //  m386123900
            // -----------------------------------------------
            WorksheetStyle m386123900 = styles.Add("m386123900");
            m386123900.Font.FontName = "Calibri";
            m386123900.Font.Size = 12;
            m386123900.Font.Color = "#000000";
            m386123900.Interior.Color = "#F2DCDB";
            m386123900.Interior.Pattern = StyleInteriorPattern.Solid;
            m386123900.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            m386123900.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            m386123900.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 2);
            m386123900.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 2);
            m386123900.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 2);
            m386123900.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 2);
            // -----------------------------------------------
            //  m386123920
            // -----------------------------------------------
            WorksheetStyle m386123920 = styles.Add("m386123920");
            m386123920.Font.Bold = true;
            m386123920.Font.FontName = "Calibri";
            m386123920.Font.Size = 11;
            m386123920.Font.Color = "#000000";
            m386123920.Interior.Color = "#DCE6F1";
            m386123920.Interior.Pattern = StyleInteriorPattern.Solid;
            m386123920.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            m386123920.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            m386123920.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 2);
            m386123920.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 2);
            m386123920.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 2);
            m386123920.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 2);
            // -----------------------------------------------
            //  m386123940
            // -----------------------------------------------
            WorksheetStyle m386123940 = styles.Add("m386123940");
            m386123940.Font.Bold = true;
            m386123940.Font.FontName = "Calibri";
            m386123940.Font.Size = 12;
            m386123940.Font.Color = "#000000";
            m386123940.Interior.Color = "#FCD5B4";
            m386123940.Interior.Pattern = StyleInteriorPattern.Solid;
            m386123940.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            m386123940.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            m386123940.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 2);
            m386123940.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 2);
            m386123940.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 2);
            // -----------------------------------------------
            //  s45
            // -----------------------------------------------
            WorksheetStyle s45 = styles.Add("s45");
            s45.Font.Bold = true;
            s45.Font.FontName = "Calibri";
            s45.Font.Size = 11;
            s45.Font.Color = "#000000";
            s45.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s45.Alignment.Vertical = StyleVerticalAlignment.Top;
            s45.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s45.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s45.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s46
            // -----------------------------------------------
            WorksheetStyle s46 = styles.Add("s46");
            s46.Font.Bold = true;
            s46.Font.FontName = "Calibri";
            s46.Font.Size = 11;
            s46.Font.Color = "#000000";
            s46.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s46.Alignment.Vertical = StyleVerticalAlignment.Center;
            s46.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s46.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s46.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 2);
            // -----------------------------------------------
            //  s53
            // -----------------------------------------------
            WorksheetStyle s53 = styles.Add("s53");
            s53.Font.Bold = true;
            s53.Font.FontName = "Calibri";
            s53.Font.Size = 11;
            s53.Font.Color = "#000000";
            s53.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s53.Alignment.Vertical = StyleVerticalAlignment.Top;
            s53.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s53.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 2);
            s53.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s53.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 2);
            // -----------------------------------------------
            //  s54
            // -----------------------------------------------
            WorksheetStyle s54 = styles.Add("s54");
            s54.Font.Bold = true;
            s54.Font.FontName = "Calibri";
            s54.Font.Size = 11;
            s54.Font.Color = "#000000";
            s54.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s54.Alignment.Vertical = StyleVerticalAlignment.Top;
            s54.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s54.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s54.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s54.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 2);
            // -----------------------------------------------
            //  s55
            // -----------------------------------------------
            WorksheetStyle s55 = styles.Add("s55");
            s55.Font.Bold = true;
            s55.Font.FontName = "Calibri";
            s55.Font.Size = 11;
            s55.Font.Color = "#000000";
            s55.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s55.Alignment.Vertical = StyleVerticalAlignment.Top;
            s55.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s55.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s55.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 2);
            s55.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 2);
            // -----------------------------------------------
            //  s56
            // -----------------------------------------------
            WorksheetStyle s56 = styles.Add("s56");
            s56.Alignment.Vertical = StyleVerticalAlignment.Center;
            s56.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s56.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 2);
            s56.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s56.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s57
            // -----------------------------------------------
            WorksheetStyle s57 = styles.Add("s57");
            s57.Alignment.Vertical = StyleVerticalAlignment.Center;
            s57.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s57.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s57.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 2);
            s57.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s58
            // -----------------------------------------------
            WorksheetStyle s58 = styles.Add("s58");
            s58.Alignment.Vertical = StyleVerticalAlignment.Center;
            s58.Alignment.WrapText = true;
            s58.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s58.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s58.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 2);
            s58.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s59
            // -----------------------------------------------
            WorksheetStyle s59 = styles.Add("s59");
            s59.Alignment.Vertical = StyleVerticalAlignment.Center;
            s59.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 2);
            s59.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 2);
            s59.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s59.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s60
            // -----------------------------------------------
            WorksheetStyle s60 = styles.Add("s60");
            s60.Alignment.Vertical = StyleVerticalAlignment.Center;
            s60.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 2);
            s60.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s60.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 2);
            s60.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s61
            // -----------------------------------------------
            WorksheetStyle s61 = styles.Add("s61");
            s61.Font.Bold = true;
            s61.Font.FontName = "Calibri";
            s61.Font.Size = 11;
            s61.Font.Color = "#000000";
            s61.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s61.Alignment.Vertical = StyleVerticalAlignment.Center;
            s61.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s61.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 2);
            s61.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s62
            // -----------------------------------------------
            WorksheetStyle s62 = styles.Add("s62");
            s62.Font.Bold = true;
            s62.Font.FontName = "Calibri";
            s62.Font.Size = 11;
            s62.Font.Color = "#000000";
            s62.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s62.Alignment.Vertical = StyleVerticalAlignment.Center;
            s62.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s62.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s62.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s63
            // -----------------------------------------------
            WorksheetStyle s63 = styles.Add("s63");
            s63.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s63.Alignment.Vertical = StyleVerticalAlignment.Center;
            s63.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s63.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 2);
            s63.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s63.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s64
            // -----------------------------------------------
            WorksheetStyle s64 = styles.Add("s64");
            s64.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s64.Alignment.Vertical = StyleVerticalAlignment.Center;
            s64.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s64.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s64.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s64.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s65
            // -----------------------------------------------
            WorksheetStyle s65 = styles.Add("s65");
            s65.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s65.Alignment.Vertical = StyleVerticalAlignment.Center;
            s65.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 2);
            s65.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 2);
            s65.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s65.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s66
            // -----------------------------------------------
            WorksheetStyle s66 = styles.Add("s66");
            s66.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s66.Alignment.Vertical = StyleVerticalAlignment.Center;
            s66.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 2);
            s66.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s66.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s66.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s67
            // -----------------------------------------------
            WorksheetStyle s67 = styles.Add("s67");
            s67.Font.Bold = true;
            s67.Font.FontName = "Calibri";
            s67.Font.Size = 16;
            s67.Font.Color = "#000000";
            s67.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s67.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s137
            // -----------------------------------------------
            WorksheetStyle s137 = styles.Add("s137");
            s137.Name = "Normal 3";
            s137.Font.FontName = "Calibri";
            s137.Font.Size = 11;
            s137.Font.Color = "#000000";
            s137.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  m2611536909264
            // -----------------------------------------------
            WorksheetStyle m2611536909264 = styles.Add("m2611536909264");
            m2611536909264.Parent = "s137";
            m2611536909264.Font.FontName = "Arial";
            m2611536909264.Font.Color = "#9400D3";
            m2611536909264.Interior.Color = "#FFFFFF";
            m2611536909264.Interior.Pattern = StyleInteriorPattern.Solid;
            m2611536909264.Alignment.Vertical = StyleVerticalAlignment.Top;
            m2611536909264.Alignment.WrapText = true;
            m2611536909264.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m2611536909264.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  m2611536909284
            // -----------------------------------------------
            WorksheetStyle m2611536909284 = styles.Add("m2611536909284");
            m2611536909284.Parent = "s137";
            m2611536909284.Font.FontName = "Arial";
            m2611536909284.Font.Color = "#9400D3";
            m2611536909284.Interior.Color = "#FFFFFF";
            m2611536909284.Interior.Pattern = StyleInteriorPattern.Solid;
            m2611536909284.Alignment.Vertical = StyleVerticalAlignment.Top;
            m2611536909284.Alignment.WrapText = true;
            m2611536909284.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m2611536909284.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  m2611536909304
            // -----------------------------------------------
            WorksheetStyle m2611536909304 = styles.Add("m2611536909304");
            m2611536909304.Parent = "s137";
            m2611536909304.Font.FontName = "Arial";
            m2611536909304.Font.Color = "#9400D3";
            m2611536909304.Interior.Color = "#FFFFFF";
            m2611536909304.Interior.Pattern = StyleInteriorPattern.Solid;
            m2611536909304.Alignment.Vertical = StyleVerticalAlignment.Top;
            m2611536909304.Alignment.WrapText = true;
            m2611536909304.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m2611536909304.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  m2611536909324
            // -----------------------------------------------
            WorksheetStyle m2611536909324 = styles.Add("m2611536909324");
            m2611536909324.Parent = "s137";
            m2611536909324.Font.FontName = "Arial";
            m2611536909324.Font.Color = "#9400D3";
            m2611536909324.Interior.Color = "#FFFFFF";
            m2611536909324.Interior.Pattern = StyleInteriorPattern.Solid;
            m2611536909324.Alignment.Vertical = StyleVerticalAlignment.Top;
            m2611536909324.Alignment.WrapText = true;
            m2611536909324.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m2611536909324.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  m2611536909344
            // -----------------------------------------------
            WorksheetStyle m2611536909344 = styles.Add("m2611536909344");
            m2611536909344.Parent = "s137";
            m2611536909344.Font.FontName = "Arial";
            m2611536909344.Font.Color = "#9400D3";
            m2611536909344.Interior.Color = "#FFFFFF";
            m2611536909344.Interior.Pattern = StyleInteriorPattern.Solid;
            m2611536909344.Alignment.Vertical = StyleVerticalAlignment.Top;
            m2611536909344.Alignment.WrapText = true;
            m2611536909344.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m2611536909344.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  m2611540549552
            // -----------------------------------------------
            WorksheetStyle m2611540549552 = styles.Add("m2611540549552");
            m2611540549552.Parent = "s137";
            m2611540549552.Font.FontName = "Arial";
            m2611540549552.Font.Color = "#9400D3";
            m2611540549552.Interior.Color = "#FFFFFF";
            m2611540549552.Interior.Pattern = StyleInteriorPattern.Solid;
            m2611540549552.Alignment.Vertical = StyleVerticalAlignment.Top;
            m2611540549552.Alignment.WrapText = true;
            m2611540549552.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m2611540549552.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  m2611540549572
            // -----------------------------------------------
            WorksheetStyle m2611540549572 = styles.Add("m2611540549572");
            m2611540549572.Parent = "s137";
            m2611540549572.Font.FontName = "Arial";
            m2611540549572.Font.Color = "#9400D3";
            m2611540549572.Interior.Color = "#FFFFFF";
            m2611540549572.Interior.Pattern = StyleInteriorPattern.Solid;
            m2611540549572.Alignment.Vertical = StyleVerticalAlignment.Top;
            m2611540549572.Alignment.WrapText = true;
            m2611540549572.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m2611540549572.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            m2611540549572.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  m2611540549592
            // -----------------------------------------------
            WorksheetStyle m2611540549592 = styles.Add("m2611540549592");
            m2611540549592.Parent = "s137";
            m2611540549592.Font.FontName = "Arial";
            m2611540549592.Font.Color = "#9400D3";
            m2611540549592.Interior.Color = "#FFFFFF";
            m2611540549592.Interior.Pattern = StyleInteriorPattern.Solid;
            m2611540549592.Alignment.Vertical = StyleVerticalAlignment.Top;
            m2611540549592.Alignment.WrapText = true;
            m2611540549592.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m2611540549592.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  m2611540549612
            // -----------------------------------------------
            WorksheetStyle m2611540549612 = styles.Add("m2611540549612");
            m2611540549612.Parent = "s137";
            m2611540549612.Font.FontName = "Arial";
            m2611540549612.Font.Color = "#9400D3";
            m2611540549612.Interior.Color = "#FFFFFF";
            m2611540549612.Interior.Pattern = StyleInteriorPattern.Solid;
            m2611540549612.Alignment.Vertical = StyleVerticalAlignment.Top;
            m2611540549612.Alignment.WrapText = true;
            m2611540549612.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m2611540549612.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  m2611540549632
            // -----------------------------------------------
            WorksheetStyle m2611540549632 = styles.Add("m2611540549632");
            m2611540549632.Parent = "s137";
            m2611540549632.Font.FontName = "Arial";
            m2611540549632.Font.Color = "#9400D3";
            m2611540549632.Interior.Color = "#FFFFFF";
            m2611540549632.Interior.Pattern = StyleInteriorPattern.Solid;
            m2611540549632.Alignment.Vertical = StyleVerticalAlignment.Top;
            m2611540549632.Alignment.WrapText = true;
            m2611540549632.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m2611540549632.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  m2611540549652
            // -----------------------------------------------
            WorksheetStyle m2611540549652 = styles.Add("m2611540549652");
            m2611540549652.Parent = "s137";
            m2611540549652.Font.FontName = "Arial";
            m2611540549652.Font.Color = "#9400D3";
            m2611540549652.Interior.Color = "#FFFFFF";
            m2611540549652.Interior.Pattern = StyleInteriorPattern.Solid;
            m2611540549652.Alignment.Vertical = StyleVerticalAlignment.Top;
            m2611540549652.Alignment.WrapText = true;
            m2611540549652.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m2611540549652.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  m2611540549672
            // -----------------------------------------------
            WorksheetStyle m2611540549672 = styles.Add("m2611540549672");
            m2611540549672.Parent = "s137";
            m2611540549672.Font.FontName = "Arial";
            m2611540549672.Font.Color = "#9400D3";
            m2611540549672.Interior.Color = "#FFFFFF";
            m2611540549672.Interior.Pattern = StyleInteriorPattern.Solid;
            m2611540549672.Alignment.Vertical = StyleVerticalAlignment.Top;
            m2611540549672.Alignment.WrapText = true;
            m2611540549672.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m2611540549672.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");

            // -----------------------------------------------
            //  s139
            // -----------------------------------------------
            WorksheetStyle s139 = styles.Add("s139");
            s139.Parent = "s137";
            s139.Font.FontName = "Calibri";
            s139.Font.Size = 11;
            s139.Interior.Color = "#FFFFFF";
            s139.Interior.Pattern = StyleInteriorPattern.Solid;
            s139.Alignment.Vertical = StyleVerticalAlignment.Top;
            s139.Alignment.WrapText = true;
            // -----------------------------------------------
            //  s140
            // -----------------------------------------------
            WorksheetStyle s140 = styles.Add("s140");
            s140.Parent = "s137";
            s140.Font.FontName = "Calibri";
            s140.Font.Size = 11;
            s140.Interior.Color = "#FFFFFF";
            s140.Interior.Pattern = StyleInteriorPattern.Solid;
            s140.Alignment.Vertical = StyleVerticalAlignment.Top;
            s140.Alignment.WrapText = true;
            s140.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            s140.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s141
            // -----------------------------------------------
            WorksheetStyle s141 = styles.Add("s141");
            s141.Parent = "s137";
            s141.Font.FontName = "Calibri";
            s141.Font.Size = 11;
            s141.Interior.Color = "#FFFFFF";
            s141.Interior.Pattern = StyleInteriorPattern.Solid;
            s141.Alignment.Vertical = StyleVerticalAlignment.Top;
            s141.Alignment.WrapText = true;
            s141.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s142
            // -----------------------------------------------
            WorksheetStyle s142 = styles.Add("s142");
            s142.Parent = "s137";
            s142.Font.FontName = "Calibri";
            s142.Font.Size = 11;
            s142.Interior.Color = "#FFFFFF";
            s142.Interior.Pattern = StyleInteriorPattern.Solid;
            s142.Alignment.Vertical = StyleVerticalAlignment.Top;
            s142.Alignment.WrapText = true;
            s142.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            s142.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s143
            // -----------------------------------------------
            WorksheetStyle s143 = styles.Add("s143");
            s143.Parent = "s137";
            s143.Font.FontName = "Calibri";
            s143.Font.Size = 11;
            s143.Interior.Color = "#FFFFFF";
            s143.Interior.Pattern = StyleInteriorPattern.Solid;
            s143.Alignment.Vertical = StyleVerticalAlignment.Top;
            s143.Alignment.WrapText = true;
            s143.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s144
            // -----------------------------------------------
            WorksheetStyle s144 = styles.Add("s144");
            s144.Parent = "s137";
            s144.Font.FontName = "Arial";
            s144.Font.Color = "#9400D3";
            s144.Interior.Color = "#FFFFFF";
            s144.Interior.Pattern = StyleInteriorPattern.Solid;
            s144.Alignment.Vertical = StyleVerticalAlignment.Top;
            s144.Alignment.WrapText = true;
            s144.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            // -----------------------------------------------
            //  s145
            // -----------------------------------------------
            WorksheetStyle s145 = styles.Add("s145");
            s145.Parent = "s137";
            s145.Font.FontName = "Calibri";
            s145.Font.Size = 11;
            s145.Interior.Color = "#FFFFFF";
            s145.Interior.Pattern = StyleInteriorPattern.Solid;
            s145.Alignment.Vertical = StyleVerticalAlignment.Top;
            s145.Alignment.WrapText = true;
            s145.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s146
            // -----------------------------------------------
            WorksheetStyle s146 = styles.Add("s146");
            s146.Parent = "s137";
            s146.Font.FontName = "Calibri";
            s146.Font.Size = 11;
            s146.Interior.Color = "#FFFFFF";
            s146.Interior.Pattern = StyleInteriorPattern.Solid;
            s146.Alignment.Vertical = StyleVerticalAlignment.Top;
            s146.Alignment.WrapText = true;
            s146.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            s146.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s169
            // -----------------------------------------------
            WorksheetStyle s169 = styles.Add("s169");
            s169.Parent = "s137";
            s169.Font.FontName = "Arial";
            s169.Font.Color = "#9400D3";
            s169.Interior.Color = "#FFFFFF";
            s169.Interior.Pattern = StyleInteriorPattern.Solid;
            s169.Alignment.Vertical = StyleVerticalAlignment.Top;
            s169.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            // -----------------------------------------------
            //  s170
            // -----------------------------------------------
            WorksheetStyle s170 = styles.Add("s170");
            s170.Parent = "s137";
            s170.Font.FontName = "Calibri";
            s170.Font.Size = 11;
            s170.Interior.Color = "#FFFFFF";
            s170.Interior.Pattern = StyleInteriorPattern.Solid;
            s170.Alignment.Vertical = StyleVerticalAlignment.Top;
            // -----------------------------------------------
            //  s171
            // -----------------------------------------------
            WorksheetStyle s171 = styles.Add("s171");
            s171.Parent = "s137";
            s171.Font.FontName = "Calibri";
            s171.Font.Size = 11;
            s171.Interior.Color = "#FFFFFF";
            s171.Interior.Pattern = StyleInteriorPattern.Solid;
            s171.Alignment.Vertical = StyleVerticalAlignment.Top;
            s171.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s172
            // -----------------------------------------------
            WorksheetStyle s172 = styles.Add("s172");
            s172.Alignment.Vertical = StyleVerticalAlignment.Bottom;
        }

        private void On_SaveExcelClosed(object sender, FormClosedEventArgs e)
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

                WorksheetRow excelrowHeader; WorksheetRow excelrowSpace; WorksheetRow excelrowSpace1; WorksheetRow excelrow;
                WorksheetRow excelrow1; WorksheetRow excelrow2; WorksheetRow excelrow3; WorksheetRow excelrow4;

                WorksheetCell cell;

                Workbook book = new Workbook();

                Worksheet sheet = book.Worksheets.Add("Parameters");

                #region Header Page
                WorksheetColumn columnHead = sheet.Table.Columns.Add();
                columnHead.Index = 2;
                columnHead.Width = 5;
                sheet.Table.Columns.Add(163);
                WorksheetColumn column2Head = sheet.Table.Columns.Add();
                column2Head.Width = 370;//332;
                column2Head.StyleID = "s172";
                sheet.Table.Columns.Add(100);//(59);
                //  s137
                // -----------------------------------------------
                WorksheetStyle s137 = book.Styles.Add("s137");
                s137.Name = "Normal 3";
                s137.Font.FontName = "Calibri";
                s137.Font.Size = 11;
                s137.Font.Color = "#000000";
                s137.Alignment.Vertical = StyleVerticalAlignment.Bottom;
                // -----------------------------------------------
                //  m2611536909264
                // -----------------------------------------------
                WorksheetStyle m2611536909264 = book.Styles.Add("m2611536909264");
                m2611536909264.Parent = "s137";
                m2611536909264.Font.FontName = "Arial";
                m2611536909264.Font.Color = "#9400D3";
                m2611536909264.Interior.Color = "#FFFFFF";
                m2611536909264.Interior.Pattern = StyleInteriorPattern.Solid;
                m2611536909264.Alignment.Vertical = StyleVerticalAlignment.Top;
                m2611536909264.Alignment.WrapText = true;
                m2611536909264.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
                m2611536909264.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
                // -----------------------------------------------
                //  m2611536909284
                // -----------------------------------------------
                WorksheetStyle m2611536909284 = book.Styles.Add("m2611536909284");
                m2611536909284.Parent = "s137";
                m2611536909284.Font.FontName = "Arial";
                m2611536909284.Font.Color = "#9400D3";
                m2611536909284.Interior.Color = "#FFFFFF";
                m2611536909284.Interior.Pattern = StyleInteriorPattern.Solid;
                m2611536909284.Alignment.Vertical = StyleVerticalAlignment.Top;
                m2611536909284.Alignment.WrapText = true;
                m2611536909284.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
                m2611536909284.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
                // -----------------------------------------------
                //  m2611536909304
                // -----------------------------------------------
                WorksheetStyle m2611536909304 = book.Styles.Add("m2611536909304");
                m2611536909304.Parent = "s137";
                m2611536909304.Font.FontName = "Arial";
                m2611536909304.Font.Color = "#9400D3";
                m2611536909304.Interior.Color = "#FFFFFF";
                m2611536909304.Interior.Pattern = StyleInteriorPattern.Solid;
                m2611536909304.Alignment.Vertical = StyleVerticalAlignment.Top;
                m2611536909304.Alignment.WrapText = true;
                m2611536909304.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
                m2611536909304.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
                // -----------------------------------------------
                //  m2611536909324
                // -----------------------------------------------
                WorksheetStyle m2611536909324 = book.Styles.Add("m2611536909324");
                m2611536909324.Parent = "s137";
                m2611536909324.Font.FontName = "Arial";
                m2611536909324.Font.Color = "#9400D3";
                m2611536909324.Interior.Color = "#FFFFFF";
                m2611536909324.Interior.Pattern = StyleInteriorPattern.Solid;
                m2611536909324.Alignment.Vertical = StyleVerticalAlignment.Top;
                m2611536909324.Alignment.WrapText = true;
                m2611536909324.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
                m2611536909324.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
                // -----------------------------------------------
                //  m2611536909344
                // -----------------------------------------------
                WorksheetStyle m2611536909344 = book.Styles.Add("m2611536909344");
                m2611536909344.Parent = "s137";
                m2611536909344.Font.FontName = "Arial";
                m2611536909344.Font.Color = "#9400D3";
                m2611536909344.Interior.Color = "#FFFFFF";
                m2611536909344.Interior.Pattern = StyleInteriorPattern.Solid;
                m2611536909344.Alignment.Vertical = StyleVerticalAlignment.Top;
                m2611536909344.Alignment.WrapText = true;
                m2611536909344.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
                m2611536909344.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
                // -----------------------------------------------
                //  m2611540549552
                // -----------------------------------------------
                WorksheetStyle m2611540549552 = book.Styles.Add("m2611540549552");
                m2611540549552.Parent = "s137";
                m2611540549552.Font.FontName = "Arial";
                m2611540549552.Font.Color = "#9400D3";
                m2611540549552.Interior.Color = "#FFFFFF";
                m2611540549552.Interior.Pattern = StyleInteriorPattern.Solid;
                m2611540549552.Alignment.Vertical = StyleVerticalAlignment.Top;
                m2611540549552.Alignment.WrapText = true;
                m2611540549552.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
                m2611540549552.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
                // -----------------------------------------------
                //  m2611540549572
                // -----------------------------------------------
                WorksheetStyle m2611540549572 = book.Styles.Add("m2611540549572");
                m2611540549572.Parent = "s137";
                m2611540549572.Font.FontName = "Arial";
                m2611540549572.Font.Color = "#9400D3";
                m2611540549572.Interior.Color = "#FFFFFF";
                m2611540549572.Interior.Pattern = StyleInteriorPattern.Solid;
                m2611540549572.Alignment.Vertical = StyleVerticalAlignment.Top;
                m2611540549572.Alignment.WrapText = true;
                m2611540549572.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
                m2611540549572.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
                m2611540549572.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
                // -----------------------------------------------
                //  m2611540549592
                // -----------------------------------------------
                WorksheetStyle m2611540549592 = book.Styles.Add("m2611540549592");
                m2611540549592.Parent = "s137";
                m2611540549592.Font.FontName = "Arial";
                m2611540549592.Font.Color = "#9400D3";
                m2611540549592.Interior.Color = "#FFFFFF";
                m2611540549592.Interior.Pattern = StyleInteriorPattern.Solid;
                m2611540549592.Alignment.Vertical = StyleVerticalAlignment.Top;
                m2611540549592.Alignment.WrapText = true;
                m2611540549592.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
                m2611540549592.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
                // -----------------------------------------------
                //  m2611540549612
                // -----------------------------------------------
                WorksheetStyle m2611540549612 = book.Styles.Add("m2611540549612");
                m2611540549612.Parent = "s137";
                m2611540549612.Font.FontName = "Arial";
                m2611540549612.Font.Color = "#9400D3";
                m2611540549612.Interior.Color = "#FFFFFF";
                m2611540549612.Interior.Pattern = StyleInteriorPattern.Solid;
                m2611540549612.Alignment.Vertical = StyleVerticalAlignment.Top;
                m2611540549612.Alignment.WrapText = true;
                m2611540549612.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
                m2611540549612.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
                // -----------------------------------------------
                //  m2611540549632
                // -----------------------------------------------
                WorksheetStyle m2611540549632 = book.Styles.Add("m2611540549632");
                m2611540549632.Parent = "s137";
                m2611540549632.Font.FontName = "Arial";
                m2611540549632.Font.Color = "#9400D3";
                m2611540549632.Interior.Color = "#FFFFFF";
                m2611540549632.Interior.Pattern = StyleInteriorPattern.Solid;
                m2611540549632.Alignment.Vertical = StyleVerticalAlignment.Top;
                m2611540549632.Alignment.WrapText = true;
                m2611540549632.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
                m2611540549632.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
                // -----------------------------------------------
                //  m2611540549652
                // -----------------------------------------------
                WorksheetStyle m2611540549652 = book.Styles.Add("m2611540549652");
                m2611540549652.Parent = "s137";
                m2611540549652.Font.FontName = "Arial";
                m2611540549652.Font.Color = "#9400D3";
                m2611540549652.Interior.Color = "#FFFFFF";
                m2611540549652.Interior.Pattern = StyleInteriorPattern.Solid;
                m2611540549652.Alignment.Vertical = StyleVerticalAlignment.Top;
                m2611540549652.Alignment.WrapText = true;
                m2611540549652.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
                m2611540549652.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
                // -----------------------------------------------
                //  m2611540549672
                // -----------------------------------------------
                WorksheetStyle m2611540549672 = book.Styles.Add("m2611540549672");
                m2611540549672.Parent = "s137";
                m2611540549672.Font.FontName = "Arial";
                m2611540549672.Font.Color = "#9400D3";
                m2611540549672.Interior.Color = "#FFFFFF";
                m2611540549672.Interior.Pattern = StyleInteriorPattern.Solid;
                m2611540549672.Alignment.Vertical = StyleVerticalAlignment.Top;
                m2611540549672.Alignment.WrapText = true;
                m2611540549672.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
                m2611540549672.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
                // -----------------------------------------------
                //  s139
                // -----------------------------------------------
                WorksheetStyle s139 = book.Styles.Add("s139");
                s139.Parent = "s137";
                s139.Font.FontName = "Calibri";
                s139.Font.Size = 11;
                s139.Interior.Color = "#FFFFFF";
                s139.Interior.Pattern = StyleInteriorPattern.Solid;
                s139.Alignment.Vertical = StyleVerticalAlignment.Top;
                s139.Alignment.WrapText = true;
                // -----------------------------------------------
                //  s140
                // -----------------------------------------------
                WorksheetStyle s140 = book.Styles.Add("s140");
                s140.Parent = "s137";
                s140.Font.FontName = "Calibri";
                s140.Font.Size = 11;
                s140.Interior.Color = "#FFFFFF";
                s140.Interior.Pattern = StyleInteriorPattern.Solid;
                s140.Alignment.Vertical = StyleVerticalAlignment.Top;
                s140.Alignment.WrapText = true;
                s140.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
                s140.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
                // -----------------------------------------------
                //  s141
                // -----------------------------------------------
                WorksheetStyle s141 = book.Styles.Add("s141");
                s141.Parent = "s137";
                s141.Font.FontName = "Calibri";
                s141.Font.Size = 11;
                s141.Interior.Color = "#FFFFFF";
                s141.Interior.Pattern = StyleInteriorPattern.Solid;
                s141.Alignment.Vertical = StyleVerticalAlignment.Top;
                s141.Alignment.WrapText = true;
                s141.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
                // -----------------------------------------------
                //  s142
                // -----------------------------------------------
                WorksheetStyle s142 = book.Styles.Add("s142");
                s142.Parent = "s137";
                s142.Font.FontName = "Calibri";
                s142.Font.Size = 11;
                s142.Interior.Color = "#FFFFFF";
                s142.Interior.Pattern = StyleInteriorPattern.Solid;
                s142.Alignment.Vertical = StyleVerticalAlignment.Top;
                s142.Alignment.WrapText = true;
                s142.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
                s142.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
                // -----------------------------------------------
                //  s143
                // -----------------------------------------------
                WorksheetStyle s143 = book.Styles.Add("s143");
                s143.Parent = "s137";
                s143.Font.FontName = "Calibri";
                s143.Font.Size = 11;
                s143.Interior.Color = "#FFFFFF";
                s143.Interior.Pattern = StyleInteriorPattern.Solid;
                s143.Alignment.Vertical = StyleVerticalAlignment.Top;
                s143.Alignment.WrapText = true;
                s143.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
                // -----------------------------------------------
                //  s144
                // -----------------------------------------------
                WorksheetStyle s144 = book.Styles.Add("s144");
                s144.Parent = "s137";
                s144.Font.FontName = "Arial";
                s144.Font.Color = "#9400D3";
                s144.Interior.Color = "#FFFFFF";
                s144.Interior.Pattern = StyleInteriorPattern.Solid;
                s144.Alignment.Vertical = StyleVerticalAlignment.Top;
                s144.Alignment.WrapText = true;
                s144.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
                // -----------------------------------------------
                //  s145
                // -----------------------------------------------
                WorksheetStyle s145 = book.Styles.Add("s145");
                s145.Parent = "s137";
                s145.Font.FontName = "Calibri";
                s145.Font.Size = 11;
                s145.Interior.Color = "#FFFFFF";
                s145.Interior.Pattern = StyleInteriorPattern.Solid;
                s145.Alignment.Vertical = StyleVerticalAlignment.Top;
                s145.Alignment.WrapText = true;
                s145.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
                // -----------------------------------------------
                //  s146
                // -----------------------------------------------
                WorksheetStyle s146 = book.Styles.Add("s146");
                s146.Parent = "s137";
                s146.Font.FontName = "Calibri";
                s146.Font.Size = 11;
                s146.Interior.Color = "#FFFFFF";
                s146.Interior.Pattern = StyleInteriorPattern.Solid;
                s146.Alignment.Vertical = StyleVerticalAlignment.Top;
                s146.Alignment.WrapText = true;
                s146.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
                s146.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
                // -----------------------------------------------
                //  s169
                // -----------------------------------------------
                WorksheetStyle s169 = book.Styles.Add("s169");
                s169.Parent = "s137";
                s169.Font.FontName = "Arial";
                s169.Font.Color = "#9400D3";
                s169.Interior.Color = "#FFFFFF";
                s169.Interior.Pattern = StyleInteriorPattern.Solid;
                s169.Alignment.Vertical = StyleVerticalAlignment.Top;
                s169.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
                // -----------------------------------------------
                //  s170
                // -----------------------------------------------
                WorksheetStyle s170 = book.Styles.Add("s170");
                s170.Parent = "s137";
                s170.Font.FontName = "Calibri";
                s170.Font.Size = 11;
                s170.Interior.Color = "#FFFFFF";
                s170.Interior.Pattern = StyleInteriorPattern.Solid;
                s170.Alignment.Vertical = StyleVerticalAlignment.Top;
                // -----------------------------------------------
                //  s171
                // -----------------------------------------------
                WorksheetStyle s171 = book.Styles.Add("s171");
                s171.Parent = "s137";
                s171.Font.FontName = "Calibri";
                s171.Font.Size = 11;
                s171.Interior.Color = "#FFFFFF";
                s171.Interior.Pattern = StyleInteriorPattern.Solid;
                s171.Alignment.Vertical = StyleVerticalAlignment.Top;
                s171.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
                // -----------------------------------------------
                //  s172
                // -----------------------------------------------
                WorksheetStyle s172 = book.Styles.Add("s172");
                s172.Alignment.Vertical = StyleVerticalAlignment.Bottom;

                // -----------------------------------------------
                WorksheetRow RowHead = sheet.Table.Rows.Add();
                //WorksheetCell cell;
                cell = RowHead.Cells.Add();
                cell.StyleID = "s139";
                cell = RowHead.Cells.Add();
                cell.StyleID = "s139";
                cell = RowHead.Cells.Add();
                cell.StyleID = "s139";
                cell = RowHead.Cells.Add();
                cell.StyleID = "s170";
                cell = RowHead.Cells.Add();
                cell.StyleID = "s139";
                // -----------------------------------------------
                WorksheetRow Row1Head = sheet.Table.Rows.Add();
                Row1Head.Height = 14;
                Row1Head.AutoFitHeight = false;
                cell = Row1Head.Cells.Add();
                cell.StyleID = "s139";
                cell = Row1Head.Cells.Add();
                cell.StyleID = "s140";
                cell = Row1Head.Cells.Add();
                cell.StyleID = "s141";
                cell = Row1Head.Cells.Add();
                cell.StyleID = "s171";
                cell = Row1Head.Cells.Add();
                cell.StyleID = "s142";
                // -----------------------------------------------
                WorksheetRow Row2Head = sheet.Table.Rows.Add();
                Row2Head.Height = 14;
                Row2Head.AutoFitHeight = false;
                cell = Row2Head.Cells.Add();
                cell.StyleID = "s139";
                cell = Row2Head.Cells.Add();
                cell.StyleID = "s143";
                cell = Row2Head.Cells.Add();
                cell.StyleID = "m2611536909264";
                cell.Data.Type = DataType.String;
                cell.MergeAcross = 2;
                // -----------------------------------------------
                WorksheetRow Row7Head = sheet.Table.Rows.Add();
                Row7Head.Height = 14;
                Row7Head.AutoFitHeight = false;
                cell = Row7Head.Cells.Add();
                cell.StyleID = "s139";
                cell = Row7Head.Cells.Add();
                cell.StyleID = "s143";
                cell = Row7Head.Cells.Add();
                cell.StyleID = "m2611536909324";
                cell.Data.Type = DataType.String;
                cell.Data.Text = Privileges.Program + " - " + Privileges.PrivilegeName;
                cell.MergeAcross = 2;
                // -----------------------------------------------
                WorksheetRow Row101 = sheet.Table.Rows.Add();
                Row101.Height = 14;
                Row101.AutoFitHeight = false;
                cell = Row101.Cells.Add();
                cell.StyleID = "s139";
                cell = Row101.Cells.Add();
                cell.StyleID = "s143";
                cell = Row101.Cells.Add();
                cell.StyleID = "m2611540549592";
                cell.Data.Type = DataType.String;
                cell.MergeAcross = 2;
                // -----------------------------------------------
                WorksheetRow Row88Head = sheet.Table.Rows.Add();
                Row88Head.Height = 14;
                Row88Head.AutoFitHeight = false;
                cell = Row88Head.Cells.Add();
                cell.StyleID = "s139";
                cell = Row88Head.Cells.Add();
                cell.StyleID = "s143";
                cell = Row88Head.Cells.Add();
                cell.StyleID = "s139";
                cell = Row88Head.Cells.Add();
                cell.StyleID = "s170";
                cell = Row88Head.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                WorksheetRow Row3Head = sheet.Table.Rows.Add();
                Row3Head.Height = 14;
                Row3Head.AutoFitHeight = false;
                cell = Row3Head.Cells.Add();
                cell.StyleID = "s139";
                cell = Row3Head.Cells.Add();
                cell.StyleID = "s143";
                cell = Row3Head.Cells.Add();
                cell.StyleID = "m2611536909284";
                cell.Data.Type = DataType.String;
                cell.Data.Text = "Selected Report Parameters";
                cell.MergeAcross = 2;
                // -----------------------------------------------
                WorksheetRow Row4Head = sheet.Table.Rows.Add();
                Row4Head.Height = 14;
                Row4Head.AutoFitHeight = false;
                cell = Row4Head.Cells.Add();
                cell.StyleID = "s139";
                cell = Row4Head.Cells.Add();
                cell.StyleID = "s143";
                cell = Row4Head.Cells.Add();
                cell.StyleID = "s139";
                cell = Row4Head.Cells.Add();
                cell.StyleID = "s170";
                cell = Row4Head.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                WorksheetRow Row5Head = sheet.Table.Rows.Add();
                Row5Head.Height = 14;
                Row5Head.AutoFitHeight = false;
                cell = Row5Head.Cells.Add();
                cell.StyleID = "s139";
                cell = Row5Head.Cells.Add();
                cell.StyleID = "s143";
                cell = Row5Head.Cells.Add();
                cell.StyleID = "m2611536909304";
                cell.Data.Type = DataType.String;
                cell.MergeAcross = 2;
                // -----------------------------------------------
                WorksheetRow Row6Head = sheet.Table.Rows.Add();
                Row6Head.Height = 14;
                Row6Head.AutoFitHeight = false;
                cell = Row6Head.Cells.Add();
                cell.StyleID = "s139";
                cell = Row6Head.Cells.Add();
                cell.StyleID = "s143";
                cell = Row6Head.Cells.Add();
                cell.StyleID = "s139";
                cell = Row6Head.Cells.Add();
                cell.StyleID = "s170";
                cell = Row6Head.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                string Header_year = string.Empty;

                if (CmbYear.Visible == true)
                    Header_year = "Year: " + ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString();

                WorksheetRow Row78Head = sheet.Table.Rows.Add();
                Row78Head.Height = 14;
                Row78Head.AutoFitHeight = false;
                cell = Row78Head.Cells.Add();
                cell.StyleID = "s139";
                cell = Row78Head.Cells.Add();
                cell.StyleID = "s143";
                cell = Row78Head.Cells.Add();
                cell.StyleID = "m2611536909324";
                cell.Data.Type = DataType.String;
                cell.Data.Text = "            Hierarchy: " + Txt_HieDesc.Text.Trim() + "   " + Header_year;
                cell.MergeAcross = 2;
                // -----------------------------------------------
                WorksheetRow Row10 = sheet.Table.Rows.Add();
                Row10.Height = 14;
                Row10.AutoFitHeight = false;
                cell = Row10.Cells.Add();
                cell.StyleID = "s139";
                cell = Row10.Cells.Add();
                cell.StyleID = "s143";
                cell = Row10.Cells.Add();
                cell.StyleID = "m2611540549592";
                cell.Data.Type = DataType.String;
                cell.MergeAcross = 2;
                // -----------------------------------------------
                WorksheetRow Row11 = sheet.Table.Rows.Add();
                Row11.Height = 14;
                Row11.AutoFitHeight = false;
                cell = Row11.Cells.Add();
                cell.StyleID = "s139";
                cell = Row11.Cells.Add();
                cell.StyleID = "s143";
                cell = Row11.Cells.Add();
                cell.StyleID = "s139";
                cell = Row11.Cells.Add();
                cell.StyleID = "s170";
                cell = Row11.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                string strReportType = string.Empty;

                string strReportSequence = string.Empty;

                if (rdoApplicantNumseq.Checked)
                    strReportSequence = "Applicant Number";
                else if (rdoApplicantNameSeq.Checked)
                    strReportSequence = "Applicant Name";
                else if (rdoClassSeq.Checked)
                    strReportSequence = "Class";
                else if (rdoSelectedApplicantSeq.Checked)
                    strReportSequence = "Selected Applicant: " + txtApplicant.Text + "";
                string strSbcbDate = string.Empty;
                if (dtSBCB.Checked)
                    strSbcbDate = dtSBCB.Value.ToShortDateString();
                else
                    strSbcbDate = "All SBCB dates";


                string strEnrolledstatus = string.Empty;
                if (rdoEnrolled.Checked)
                    strEnrolledstatus = "Enrolled Only";
                else if (rdounEnrolled.Checked)
                    strEnrolledstatus = "Unenrolled Only";
                else if (rdoEnrollBoth.Checked)
                    strEnrolledstatus = "Both Enrolled & Unenrolled";


                WorksheetRow DateRange = sheet.Table.Rows.Add();
                DateRange.Height = 14;
                DateRange.AutoFitHeight = false;
                cell = DateRange.Cells.Add();
                cell.StyleID = "s139";
                cell = DateRange.Cells.Add();
                cell.StyleID = "s143";
                DateRange.Cells.Add("           Report Sequence", DataType.String, "s144");
                DateRange.Cells.Add(": " + strReportSequence, DataType.String, "s169");
                cell = DateRange.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                WorksheetRow Row111 = sheet.Table.Rows.Add();
                Row111.Height = 14;
                Row111.AutoFitHeight = false;
                cell = Row111.Cells.Add();
                cell.StyleID = "s139";
                cell = Row111.Cells.Add();
                cell.StyleID = "s143";
                cell = Row111.Cells.Add();
                cell.StyleID = "s139";
                cell = Row111.Cells.Add();
                cell.StyleID = "s170";
                cell = Row111.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                string Site = string.Empty;
                if (rdoAllSite.Checked == true) Site = "For All Sites";
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
                    Site = rdoMultipleSites.Text + " ( " + Selsites + " ) ";
                }

                WorksheetRow PrintDet = sheet.Table.Rows.Add();
                PrintDet.Height = 14;
                PrintDet.AutoFitHeight = false;
                cell = PrintDet.Cells.Add();
                cell.StyleID = "s139";
                cell = PrintDet.Cells.Add();
                cell.StyleID = "s143";
                PrintDet.Cells.Add("           Site", DataType.String, "s144");
                PrintDet.Cells.Add(": " + Site, DataType.String, "s169");
                cell = PrintDet.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                WorksheetRow Row121 = sheet.Table.Rows.Add();
                Row121.Height = 14;
                Row121.AutoFitHeight = false;
                cell = Row121.Cells.Add();
                cell.StyleID = "s139";
                cell = Row121.Cells.Add();
                cell.StyleID = "s143";
                cell = Row121.Cells.Add();
                cell.StyleID = "s139";
                cell = Row121.Cells.Add();
                cell.StyleID = "s170";
                cell = Row121.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                WorksheetRow SortBy = sheet.Table.Rows.Add();
                SortBy.Height = 14;
                SortBy.AutoFitHeight = false;
                cell = SortBy.Cells.Add();
                cell.StyleID = "s139";
                cell = SortBy.Cells.Add();
                cell.StyleID = "s143";
                SortBy.Cells.Add("           Tracking Component", DataType.String, "s144");
                SortBy.Cells.Add(": " + ((Captain.Common.Utilities.ListItem)CmbTrackComp.SelectedItem).Text.ToString(), DataType.String, "s169");
                cell = SortBy.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                WorksheetRow Row131 = sheet.Table.Rows.Add();
                Row131.Height = 14;
                Row131.AutoFitHeight = false;
                cell = Row131.Cells.Add();
                cell.StyleID = "s139";
                cell = Row131.Cells.Add();
                cell.StyleID = "s143";
                cell = Row131.Cells.Add();
                cell.StyleID = "s139";
                cell = Row131.Cells.Add();
                cell.StyleID = "s170";
                cell = Row131.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                string strTaskList = string.Empty;
                if (rdoAllTask.Checked)
                    strTaskList = "All Tasks";
                else
                {
                    string strTaskCodes = string.Empty;
                    foreach (ChldTrckEntity Entity in SeltrackList)
                    {
                        if (!strTaskCodes.Equals(string.Empty)) strTaskCodes += ",";
                        strTaskCodes += Entity.TASK;
                    }
                    strTaskList = "Selected Tasks: " + strTaskCodes;
                }

                WorksheetRow SelSite = sheet.Table.Rows.Add();
                SelSite.Height = 14;
                SelSite.AutoFitHeight = false;
                cell = SelSite.Cells.Add();
                cell.StyleID = "s139";
                cell = SelSite.Cells.Add();
                cell.StyleID = "s143";
                SelSite.Cells.Add("            Tasks", DataType.String, "s144");
                SelSite.Cells.Add(": " + strTaskList, DataType.String, "s169");
                cell = SelSite.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                WorksheetRow Row141 = sheet.Table.Rows.Add();
                Row141.Height = 14;
                Row141.AutoFitHeight = false;
                cell = Row141.Cells.Add();
                cell.StyleID = "s139";
                cell = Row141.Cells.Add();
                cell.StyleID = "s143";
                cell = Row141.Cells.Add();
                cell.StyleID = "s139";
                cell = Row141.Cells.Add();
                cell.StyleID = "s170";
                cell = Row141.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                WorksheetRow SelTown = sheet.Table.Rows.Add();
                SelTown.Height = 14;
                SelTown.AutoFitHeight = false;
                cell = SelTown.Cells.Add();
                cell.StyleID = "s139";
                cell = SelTown.Cells.Add();
                cell.StyleID = "s143";
                SelTown.Cells.Add("            " + lblReportFormdt.Text.Trim(), DataType.String, "s144");
                SelTown.Cells.Add(": " + strSbcbDate, DataType.String, "s169");
                cell = SelTown.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                WorksheetRow Row151 = sheet.Table.Rows.Add();
                Row151.Height = 14;
                Row151.AutoFitHeight = false;
                cell = Row151.Cells.Add();
                cell.StyleID = "s139";
                cell = Row151.Cells.Add();
                cell.StyleID = "s143";
                cell = Row151.Cells.Add();
                cell.StyleID = "s139";
                cell = Row151.Cells.Add();
                cell.StyleID = "s170";
                cell = Row151.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                WorksheetRow RepElder = sheet.Table.Rows.Add();
                RepElder.Height = 14;
                RepElder.AutoFitHeight = false;
                cell = RepElder.Cells.Add();
                cell.StyleID = "s139";
                cell = RepElder.Cells.Add();
                cell.StyleID = "s143";
                RepElder.Cells.Add("           Active/Inactive", DataType.String, "s144");
                RepElder.Cells.Add(": " + ((Captain.Common.Utilities.ListItem)cmbActive.SelectedItem).Text.ToString(), DataType.String, "s169");
                cell = RepElder.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                WorksheetRow Row152 = sheet.Table.Rows.Add();
                Row152.Height = 14;
                Row152.AutoFitHeight = false;
                cell = Row152.Cells.Add();
                cell.StyleID = "s139";
                cell = Row152.Cells.Add();
                cell.StyleID = "s143";
                cell = Row152.Cells.Add();
                cell.StyleID = "s139";
                cell = Row152.Cells.Add();
                cell.StyleID = "s170";
                cell = Row152.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                WorksheetRow RepDisb = sheet.Table.Rows.Add();
                RepDisb.Height = 14;
                RepDisb.AutoFitHeight = false;
                cell = RepDisb.Cells.Add();
                cell.StyleID = "s139";
                cell = RepDisb.Cells.Add();
                cell.StyleID = "s143";
                RepDisb.Cells.Add("           Enroll Status", DataType.String, "s144");
                RepDisb.Cells.Add(": " + strEnrolledstatus, DataType.String, "s169");
                cell = RepDisb.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                WorksheetRow Row153 = sheet.Table.Rows.Add();
                Row153.Height = 14;
                Row153.AutoFitHeight = false;
                cell = Row153.Cells.Add();
                cell.StyleID = "s139";
                cell = Row153.Cells.Add();
                cell.StyleID = "s143";
                cell = Row153.Cells.Add();
                cell.StyleID = "s139";
                cell = Row153.Cells.Add();
                cell.StyleID = "s170";
                cell = Row153.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                WorksheetRow Report = sheet.Table.Rows.Add();
                Report.Height = 14;
                Report.AutoFitHeight = false;
                cell = Report.Cells.Add();
                cell.StyleID = "s139";
                cell = Report.Cells.Add();
                cell.StyleID = "s143";
                Report.Cells.Add("           Funding Sources", DataType.String, "s144");
                Report.Cells.Add(": " + ((Captain.Common.Utilities.ListItem)cmbFundingSource.SelectedItem).Text.ToString(), DataType.String, "s169");
                cell = Report.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                if (((Captain.Common.Utilities.ListItem)cmbFundingSource.SelectedItem).Value.ToString() == "D")
                {
                    string strFundingCodes = string.Empty;

                    foreach (CommonEntity FundingCode in SelFundingList)
                    {
                        if (!strFundingCodes.Equals(string.Empty)) strFundingCodes += ",";
                        strFundingCodes += FundingCode.Code;
                    }

                    WorksheetRow Row15 = sheet.Table.Rows.Add();
                    Row15.Height = 14;
                    Row15.AutoFitHeight = false;
                    cell = Row15.Cells.Add();
                    cell.StyleID = "s139";
                    cell = Row15.Cells.Add();
                    cell.StyleID = "s143";
                    cell = Row15.Cells.Add();
                    cell.StyleID = "s139";
                    cell = Row15.Cells.Add();
                    cell.StyleID = "s170";
                    cell = Row15.Cells.Add();
                    cell.StyleID = "s145";
                    // -----------------------------------------------
                    WorksheetRow Report1 = sheet.Table.Rows.Add();
                    Report1.Height = 14;
                    Report1.AutoFitHeight = false;
                    cell = Report1.Cells.Add();
                    cell.StyleID = "s139";
                    cell = Report1.Cells.Add();
                    cell.StyleID = "s143";
                    Report1.Cells.Add("           Selected Funds", DataType.String, "s144");
                    Report1.Cells.Add(": " + strFundingCodes, DataType.String, "s169");
                    cell = Report1.Cells.Add();
                    cell.StyleID = "s145";
                }
                // -----------------------------------------------
                WorksheetRow Child = sheet.Table.Rows.Add();
                Child.Height = 14;
                Child.AutoFitHeight = false;
                cell = Child.Cells.Add();
                cell.StyleID = "s139";
                cell = Child.Cells.Add();
                cell.StyleID = "s143";
                cell = Child.Cells.Add();
                cell.StyleID = "s139";
                cell = Child.Cells.Add();
                cell.StyleID = "s170";
                cell = Child.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                WorksheetRow Children = sheet.Table.Rows.Add();
                Children.Height = 14;
                Children.AutoFitHeight = false;
                cell = Children.Cells.Add();
                cell.StyleID = "s139";
                cell = Children.Cells.Add();
                cell.StyleID = "s143";
                Children.Cells.Add("           Children Age ", DataType.String, "s144");
                Children.Cells.Add("From: " + txtFrom.Text + "   To: " + txtTo.Text + ",  Base Ages on: " + (rdoTodayDate.Checked == true ? rdoTodayDate.Text : rdoKindergartenDate.Text), DataType.String, "s169");
                cell = Children.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                WorksheetRow Row13 = sheet.Table.Rows.Add();
                Row13.Height = 14;
                Row13.AutoFitHeight = false;
                cell = Row13.Cells.Add();
                cell.StyleID = "s139";
                cell = Row13.Cells.Add();
                cell.StyleID = "s143";
                cell = Row13.Cells.Add();
                cell.StyleID = "s139";
                cell = Row13.Cells.Add();
                cell.StyleID = "s170";
                cell = Row13.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                WorksheetRow Row24 = sheet.Table.Rows.Add();
                Row24.Height = 14;
                Row24.AutoFitHeight = false;
                cell = Row24.Cells.Add();
                cell.StyleID = "s139";
                cell = Row24.Cells.Add();
                cell.StyleID = "s143";
                cell = Row24.Cells.Add();
                cell.StyleID = "m2611540549632";
                cell.Data.Type = DataType.String;
                cell.MergeAcross = 2;
                // -----------------------------------------------
                WorksheetRow Row25 = sheet.Table.Rows.Add();
                Row25.Height = 14;
                Row25.AutoFitHeight = false;
                cell = Row25.Cells.Add();
                cell.StyleID = "s139";
                cell = Row25.Cells.Add();
                cell.StyleID = "s143";
                cell = Row25.Cells.Add();
                cell.StyleID = "m2611540549652";
                cell.Data.Type = DataType.String;
                cell.MergeAcross = 2;
                // -----------------------------------------------
                WorksheetRow Row26Head = sheet.Table.Rows.Add();
                Row26Head.Height = 14;
                Row26Head.AutoFitHeight = false;
                cell = Row26Head.Cells.Add();
                cell.StyleID = "s139";
                cell = Row26Head.Cells.Add();
                cell.StyleID = "s143";
                cell = Row26Head.Cells.Add();
                cell.StyleID = "s139";
                cell = Row26Head.Cells.Add();
                cell.StyleID = "s170";
                cell = Row26Head.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                WorksheetRow Row27Head = sheet.Table.Rows.Add();
                Row27Head.Height = 14;
                Row27Head.AutoFitHeight = false;
                cell = Row27Head.Cells.Add();
                cell.StyleID = "s139";
                cell = Row27Head.Cells.Add();
                cell.StyleID = "s143";
                cell = Row27Head.Cells.Add();
                cell.StyleID = "m2611540549672";
                cell.Data.Type = DataType.String;
                cell.MergeAcross = 2;
                // -----------------------------------------------
                WorksheetRow Row28 = sheet.Table.Rows.Add();
                Row28.Height = 14;
                Row28.AutoFitHeight = false;
                cell = Row28.Cells.Add();
                cell.StyleID = "s139";
                cell = Row28.Cells.Add();
                cell.StyleID = "s143";
                cell = Row28.Cells.Add();
                cell.StyleID = "s139";
                cell = Row28.Cells.Add();
                cell.StyleID = "s170";
                cell = Row28.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                WorksheetRow Row29 = sheet.Table.Rows.Add();
                Row29.Height = 14;
                Row29.AutoFitHeight = false;
                cell = Row29.Cells.Add();
                cell.StyleID = "s139";
                cell = Row29.Cells.Add();
                cell.StyleID = "s143";
                cell = Row29.Cells.Add();
                cell.StyleID = "m2611540549552";
                cell.Data.Type = DataType.String;
                cell.MergeAcross = 2;
                // -----------------------------------------------
                WorksheetRow Row30 = sheet.Table.Rows.Add();
                Row30.Height = 14;
                Row30.AutoFitHeight = false;
                cell = Row30.Cells.Add();
                cell.StyleID = "s139";
                cell = Row30.Cells.Add();
                cell.StyleID = "s143";
                cell = Row30.Cells.Add();
                cell.StyleID = "s139";
                cell = Row30.Cells.Add();
                cell.StyleID = "s170";
                cell = Row30.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                WorksheetRow Row31 = sheet.Table.Rows.Add();
                Row31.Height = 14;
                Row31.AutoFitHeight = false;
                cell = Row31.Cells.Add();
                cell.StyleID = "s139";
                cell = Row31.Cells.Add();
                cell.StyleID = "s146";
                cell = Row31.Cells.Add();
                cell.StyleID = "m2611540549572";
                cell.Data.Type = DataType.String;
                cell.MergeAcross = 2;
                // -----------------------------------------------
                //  Options
                // -----------------------------------------------
                sheet.Options.Selected = true;
                sheet.Options.ProtectObjects = false;
                sheet.Options.ProtectScenarios = false;
                sheet.Options.PageSetup.Header.Margin = 0.3F;
                sheet.Options.PageSetup.Footer.Margin = 0.3F;
                sheet.Options.PageSetup.PageMargins.Bottom = 0.75F;
                sheet.Options.PageSetup.PageMargins.Left = 0.7F;
                sheet.Options.PageSetup.PageMargins.Right = 0.7F;
                sheet.Options.PageSetup.PageMargins.Top = 0.75F;

                #endregion
                #region Styles
                WorksheetStyle mainstyle = book.Styles.Add("MainHeaderStyles");
                mainstyle.Font.FontName = "Tahoma";
                mainstyle.Font.Size = 12;
                mainstyle.Font.Bold = true;
                mainstyle.Font.Color = "#FFFFFF";
                mainstyle.Interior.Color = "#0070c0";
                mainstyle.Interior.Pattern = StyleInteriorPattern.Solid;
                mainstyle.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                mainstyle.Alignment.Vertical = StyleVerticalAlignment.Center;


                WorksheetStyle style1 = book.Styles.Add("Normal");
                style1.Font.FontName = "Tahoma";
                style1.Font.Size = 10;
                style1.Alignment.Horizontal = StyleHorizontalAlignment.Right;
                style1.Alignment.Vertical = StyleVerticalAlignment.Center;

                WorksheetStyle stylecenter = book.Styles.Add("Normalcenter");
                stylecenter.Font.FontName = "Tahoma";
                stylecenter.Font.Bold = true;
                stylecenter.Font.Size = 11;
                stylecenter.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                stylecenter.Alignment.Vertical = StyleVerticalAlignment.Center;

                WorksheetStyle style3 = book.Styles.Add("NormalLeft");
                style3.Font.FontName = "Tahoma";
                style3.Font.Size = 10;
                style3.Interior.Color = "#f2f2f2";
                style3.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                style3.Alignment.Vertical = StyleVerticalAlignment.Center;

                WorksheetStyle style4 = book.Styles.Add("NormalRight");
                style4.Font.FontName = "Tahoma";
                style4.Font.Size = 10;
                style4.Interior.Color = "#f2f2f2";
                style4.Alignment.Horizontal = StyleHorizontalAlignment.Right;
                style4.Alignment.Vertical = StyleVerticalAlignment.Center;

                WorksheetStyle style44 = book.Styles.Add("NormalRightBold");
                style44.Font.FontName = "Tahoma";
                style44.Font.Size = 10;
                style44.Font.Bold = true;
                style44.Interior.Color = "#f2f2f2";
                style44.Alignment.Horizontal = StyleHorizontalAlignment.Right;
                style44.Alignment.Vertical = StyleVerticalAlignment.Center;

                WorksheetStyle style15 = book.Styles.Add("NormalBold");
                style15.Font.FontName = "Tahoma";
                style15.Font.Size = 11;
                style15.Font.Bold = true;
                style15.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                style15.Alignment.Vertical = StyleVerticalAlignment.Center;
                style15.Alignment.WrapText = true;

                WorksheetStyle style16 = book.Styles.Add("NormalRedBold");
                style16.Font.FontName = "Tahoma";
                style16.Font.Size = 10;
                style16.Font.Bold = true;
                style16.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                style16.Alignment.Vertical = StyleVerticalAlignment.Center;
                style16.Alignment.WrapText = true;

                #endregion

                /*Worksheet*/
                sheet = book.Worksheets.Add("Data");
                sheet.Table.DefaultRowHeight = 14.25F;

                try
                {

                    string strTaskCodes = string.Empty;
                    List<ChldTrckEntity> Track_allEntity;

                    if (rdoAllTask.Checked)
                    {
                        List<ChldTrckEntity> TrackList = _model.ChldTrckData.Browse_CasetrckDetails("01");
                        List<ChldTrckEntity> Track_Det = new List<ChldTrckEntity>();
                        if (((Captain.Common.Utilities.ListItem)CmbTrackComp.SelectedItem).Value.ToString().Trim() == "0000")
                        {
                            Track_Det = TrackList.FindAll(u => u.COMPONENT.Trim().Equals("0000"));
                        }
                        else
                            Track_Det = TrackList.FindAll(u => u.COMPONENT.Trim().Equals(((Captain.Common.Utilities.ListItem)CmbTrackComp.SelectedItem).Value.ToString().Trim()) && u.Agency.Trim().Equals(BaseForm.BaseAgency.Trim()) && u.Dept.Equals(BaseForm.BaseDept.Trim()) && u.Prog.Equals(BaseForm.BaseProg.Trim()));

                        foreach (ChldTrckEntity Entity in Track_Det)
                        {
                            if (!strTaskCodes.Equals(string.Empty)) strTaskCodes += ",";
                            strTaskCodes += Entity.TASK;
                        }
                        Track_allEntity = Track_Det;

                    }
                    else
                    {
                        foreach (ChldTrckEntity Entity in SeltrackList)
                        {
                            if (!strTaskCodes.Equals(string.Empty)) strTaskCodes += ",";
                            strTaskCodes += Entity.TASK;
                        }
                        Track_allEntity = SeltrackList;
                    }

                    List<CaseEnrlEntity> caseEnrlList;
                    if (((Captain.Common.Utilities.ListItem)cmbActive.SelectedItem).Value.ToString() == "B")
                    {
                        caseEnrlList = propCaseEnrlList.FindAll(u => Convert.ToInt32(u.Snp_Age) >= Convert.ToInt32(txtFrom.Text) && Convert.ToInt32(u.Snp_Age) <= Convert.ToInt32(txtTo.Text));
                    }
                    else
                    {
                        caseEnrlList = propCaseEnrlList.FindAll(u => u.MST_ActiveStatus == ((Captain.Common.Utilities.ListItem)cmbActive.SelectedItem).Value.ToString() && Convert.ToInt32(u.Snp_Age) >= Convert.ToInt32(txtFrom.Text) && Convert.ToInt32(u.Snp_Age) <= Convert.ToInt32(txtTo.Text));
                    }

                    if (rdoEnrolled.Checked)
                        caseEnrlList = caseEnrlList.FindAll(u => u.Status.Trim().Equals("E"));
                    if (rdounEnrolled.Checked)
                        caseEnrlList = caseEnrlList.FindAll(u => u.Status.Trim() != "E");

                    if (caseEnrlList.Count > 0)
                    {
                        sheet.Table.DefaultColumnWidth = 220.5F;
                        sheet.Table.Columns.Add(100);
                        sheet.Table.Columns.Add(200);
                        sheet.Table.Columns.Add(100);
                        sheet.Table.Columns.Add(100);
                        sheet.Table.Columns.Add(100);
                        sheet.Table.Columns.Add(100);
                        sheet.Table.Columns.Add(100);
                        sheet.Table.Columns.Add(100);
                        sheet.Table.Columns.Add(300);
                        sheet.Table.Columns.Add(100);
                        sheet.Table.Columns.Add(100);
                        sheet.Table.Columns.Add(100);
                        sheet.Table.Columns.Add(100);
                        sheet.Table.Columns.Add(100);
                        sheet.Table.Columns.Add(80);
                        sheet.Table.Columns.Add(50);
                        sheet.Table.Columns.Add(80);

                        excelrowHeader = sheet.Table.Rows.Add();
                        cell = excelrowHeader.Cells.Add(Privileges.PrivilegeName, DataType.String, "MainHeaderStyles");
                        cell.MergeAcross = 16;

                        excelrowSpace = sheet.Table.Rows.Add();
                        cell = excelrowSpace.Cells.Add("", DataType.String, "NormalLeft");
                        cell.MergeAcross = 16;

                        excelrowSpace1 = sheet.Table.Rows.Add();
                        cell = excelrowSpace1.Cells.Add("", DataType.String, "NormalLeft");
                        cell.MergeAcross = 16;

                        excelrow1 = sheet.Table.Rows.Add();
                        cell = excelrow1.Cells.Add("Number", DataType.String, "NormalBold");
                        cell = excelrow1.Cells.Add("Name", DataType.String, "NormalBold");
                        cell = excelrow1.Cells.Add("Date", DataType.String, "NormalBold");
                        cell = excelrow1.Cells.Add("Class", DataType.String, "NormalBold");
                        cell = excelrow1.Cells.Add("Funding", DataType.String, "NormalBold");
                        cell = excelrow1.Cells.Add("Date", DataType.String, "NormalBold");
                        cell = excelrow1.Cells.Add("Class", DataType.String, "NormalBold");
                        cell = excelrow1.Cells.Add("Funding", DataType.String, "NormalBold");
                        cell = excelrow1.Cells.Add("Task", DataType.String, "NormalBold");
                        cell = excelrow1.Cells.Add("SBCD", DataType.String, "NormalBold");
                        cell = excelrow1.Cells.Add("Result:(D)", DataType.String, "NormalBold");
                        cell = excelrow1.Cells.Add("Date", DataType.String, "NormalBold");
                        cell = excelrow1.Cells.Add("Comments", DataType.String, "NormalBold");
                        cell = excelrow1.Cells.Add("Addressed Date", DataType.String, "NormalBold");
                        cell = excelrow1.Cells.Add("Follow up Date", DataType.String, "NormalBold");
                        cell = excelrow1.Cells.Add("Age", DataType.String, "NormalBold");
                        cell = excelrow1.Cells.Add("DOB", DataType.String, "NormalBold");

                        excelrow2 = sheet.Table.Rows.Add();
                        cell = excelrow2.Cells.Add("", DataType.String, "NormalBold");

                        propChldMediEntity = _model.ChldTrckData.GetChldMedi_Report(Agency, Dept, Prog, "****", (txtApplicant.Text == string.Empty ? string.Empty : txtApplicant.Text), (dtSBCB.Checked == true ? dtSBCB.Value.ToShortDateString() : string.Empty), strTaskCodes, "Task", "TaskDesc");

                        foreach (CaseEnrlEntity enrlrow in caseEnrlList)
                        {

                            List<ChldMediEntity> chldMediEntity = propChldMediEntity.FindAll(u => (u.AGENCY == enrlrow.Agy) && (u.DEPT == enrlrow.Dept) && (u.PROG == enrlrow.Prog) && (u.APP_NO == enrlrow.App));//&& (u.YEAR == enrlrow.Year)

                            DataTable dtMediTask = CreateTable();
                            DataRow dtrow;
                            bool boolprintstatus = true;
                            string strAddresDt = string.Empty;
                            string strFollowDt = string.Empty;
                            string strCompledt = string.Empty;
                            string strSbcbdt = string.Empty;
                            List<ChldTrckEntity> chldTrckNewList = new List<ChldTrckEntity>();
                            foreach (ChldTrckEntity trackitem in Track_allEntity)
                            {
                                try
                                {
                                    strAddresDt = string.Empty;
                                    strFollowDt = string.Empty;
                                    strCompledt = string.Empty;
                                    List<ChldTrckREntity> chldtrackRList = propchldtrackRList.FindAll(u => u.TASK == trackitem.TASK);
                                    ChldTrckREntity chldtrackRListItem = propchldtrackRList.Find(u => u.TASK == trackitem.TASK && u.FUND.Trim().ToString() == enrlrow.FundHie.Trim().ToString());


                                    List<ChldMediEntity> chlditem = chldMediEntity.FindAll(u => u.TASK.Trim() == trackitem.TASK);
                                    if (chlditem.Count > 0)
                                    {
                                        chlditem = chlditem.OrderByDescending(u => Convert.ToInt32(u.YEAR)).ThenByDescending(u => Convert.ToInt32(u.SEQ)).ToList();//(u => Convert.ToInt32(u.SEQ)).ThenByDescending(u => u.YEAR).ToList();

                                        if (chlditem[0].COMPLETED_DATE == string.Empty)
                                        {
                                            strAddresDt = chlditem[0].ADDRESSED_DATE;
                                            strFollowDt = chlditem[0].FOLLOWUP_DATE;
                                            strCompledt = string.Empty;
                                            string strAddresFollwdt = "    ";
                                            string strSBCBDate = string.Empty;
                                            if (chlditem[0].ADDRESSED_DATE != string.Empty)
                                                strAddresFollwdt = strAddresFollwdt + "ADDRESSED: " + LookupDataAccess.Getdate(chlditem[0].ADDRESSED_DATE) + "    ";
                                            if (chlditem[0].FOLLOWUP_DATE != string.Empty)
                                                strAddresFollwdt = strAddresFollwdt + "FOLLOW UP: " + LookupDataAccess.Getdate(chlditem[0].FOLLOWUP_DATE);

                                        }
                                        else
                                        {
                                            strFollowDt = chlditem[0].FOLLOWUP_DATE;
                                            strCompledt = chlditem[0].COMPLETED_DATE;
                                            ChldTrckREntity chldtrckrNextyear = chldtrackRList.Find(u => u.FUND.Trim().ToString() == enrlrow.FundHie.ToString());

                                            string strDate = string.Empty;
                                            if (chldtrckrNextyear != null)
                                            {
                                                if (chldtrckrNextyear.NXTACTION.Trim() == "Y")
                                                {
                                                    if (!string.IsNullOrEmpty(chldtrckrNextyear.NEXTDAYS))
                                                    {
                                                        if (Convert.ToInt32(chldtrckrNextyear.NEXTDAYS) >= 0)
                                                        {
                                                            if (chldtrckrNextyear.INTAKEENTRY.Trim().Equals("A"))
                                                            {
                                                                if (!string.IsNullOrEmpty(enrlrow.MST_INTAKE_DT))
                                                                    strDate = Convert.ToDateTime(enrlrow.MST_INTAKE_DT).AddDays(Convert.ToInt32(chldtrckrNextyear.NEXTDAYS)).ToString();
                                                                // break;
                                                            }
                                                            else if (chldtrckrNextyear.INTAKEENTRY.Trim().Equals("E"))
                                                            {
                                                                //  strAttnDate = ChldAttnDB.GetChldAttnDate(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, strApplicationNo, caseEnrlCheckRecord.Site, caseEnrlCheckRecord.Room, caseEnrlCheckRecord.AMPM, caseEnrlCheckRecord.FundHie, string.Empty);
                                                                if (!string.IsNullOrEmpty(enrlrow.AttendedDt))
                                                                    strDate = Convert.ToDateTime(enrlrow.AttendedDt).AddDays(Convert.ToInt32(chldtrckrNextyear.NEXTDAYS)).ToString();
                                                                if (!string.IsNullOrEmpty(enrlrow.AttendedDt))
                                                                    strDate = Convert.ToDateTime(enrlrow.AttendedDt).AddDays(Convert.ToInt32(chldtrckrNextyear.NEXTDAYS)).ToString();
                                                            }
                                                            else if (chldtrckrNextyear.INTAKEENTRY.Trim().Equals("D"))
                                                            {

                                                                strDate = Convert.ToDateTime(enrlrow.Snp_DOB).AddDays(Convert.ToInt32(chldtrckrNextyear.NEXTDAYS)).ToString();
                                                                // break;
                                                            }
                                                        }
                                                        // break;
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    if (chldtrackRListItem != null)
                                    {
                                        chldtrackRListItem.CompletDt = strCompledt;
                                        strSbcbdt = LookupDataAccess.Getdate(FillSBCDDetails(trackitem.TASK, enrlrow, chldtrackRListItem));

                                        chldTrckNewList.Add(new ChldTrckEntity(trackitem.TASK, trackitem.TASKDESCRIPTION, chldtrackRListItem.REQUIREYEAR, chldtrackRListItem.ENTERDAYS, chldtrackRListItem.NXTACTION, chldtrackRListItem.NEXTTASK, chldtrackRListItem.NEXTDAYS, strSbcbdt, strCompledt, strFollowDt, strAddresDt, chldtrackRListItem.INTAKEENTRY, trackitem.CustQCodes));
                                    }
                                    else
                                    {
                                        chldTrckNewList.Add(new ChldTrckEntity(trackitem.TASK, trackitem.TASKDESCRIPTION, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, trackitem.CustQCodes));
                                    }
                                }
                                catch (Exception ex)
                                {

                                    // throw;
                                }

                                //}
                            }

                            foreach (ChldTrckEntity item in chldTrckNewList)
                            {
                                if (item.NEXTYN.Trim().ToString() == "Y" && item.CompletDT.ToString() != string.Empty)
                                {
                                    if (!string.IsNullOrEmpty(item.NEXTDAYS))
                                    {
                                        string strSbcbdate = string.Empty;
                                        if (Convert.ToInt32(item.NEXTDAYS) >= 0)
                                        {
                                            if (item.IntakeType.Trim().Equals("A"))
                                            {
                                                if (!string.IsNullOrEmpty(enrlrow.MST_INTAKE_DT))
                                                    strSbcbdate = Convert.ToDateTime(enrlrow.MST_INTAKE_DT).AddDays(Convert.ToInt32(item.NEXTDAYS)).ToString();
                                                // break;
                                            }
                                            else if (item.IntakeType.Trim().Equals("E"))
                                            {
                                                //  strAttnDate = ChldAttnDB.GetChldAttnDate(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, strApplicationNo, caseEnrlCheckRecord.Site, caseEnrlCheckRecord.Room, caseEnrlCheckRecord.AMPM, caseEnrlCheckRecord.FundHie, string.Empty);
                                                if (!string.IsNullOrEmpty(enrlrow.AttendedDt))
                                                    strSbcbdate = Convert.ToDateTime(enrlrow.AttendedDt).AddDays(Convert.ToInt32(item.NEXTDAYS)).ToString();
                                                if (!string.IsNullOrEmpty(enrlrow.AttendedDt))
                                                    strSbcbdate = Convert.ToDateTime(enrlrow.AttendedDt).AddDays(Convert.ToInt32(item.NEXTDAYS)).ToString();
                                            }
                                            else if (item.IntakeType.Trim().Equals("D"))
                                            {

                                                strSbcbdate = Convert.ToDateTime(enrlrow.Snp_DOB).AddDays(Convert.ToInt32(item.NEXTDAYS)).ToString();
                                                // break;
                                            }
                                        }
                                        if (strSbcbdate != string.Empty)
                                        {
                                            if (item.TASK.ToString() == item.NEXTTASK.ToString())
                                            {
                                                if (item.CompletDT.ToString() != string.Empty)
                                                {
                                                    DateTime dtcomple = Convert.ToDateTime(item.CompletDT).Date;
                                                    DateTime dtSbcb = Convert.ToDateTime(strSbcbdate).Date;
                                                    if (dtcomple < dtSbcb)
                                                    {
                                                        item.CompletDT = string.Empty;
                                                        item.SBCBDT = dtSbcb.Date.ToShortDateString();
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                ChldTrckEntity chldtrckNewTaskItem = chldTrckNewList.Find(u => u.TASK == item.NEXTTASK);
                                                if (chldtrckNewTaskItem != null)
                                                {
                                                    if (chldtrckNewTaskItem.CompletDT.ToString() != string.Empty)
                                                    {
                                                        DateTime dtcomple = Convert.ToDateTime(item.CompletDT).Date;
                                                        DateTime dtSbcb = Convert.ToDateTime(strSbcbdate).Date;
                                                        if (dtcomple < dtSbcb)
                                                        {
                                                            chldTrckNewList.Find(u => u.TASK == item.NEXTTASK).CompletDT = string.Empty;
                                                            chldTrckNewList.Find(u => u.TASK == item.NEXTTASK).SBCBDT = dtSbcb.Date.ToShortDateString();
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        // break;
                                    }
                                }
                            }

                            List<ChldTrckEntity> chldtrcknewcount = chldTrckNewList.FindAll(u => u.SBCBDT.ToString() != string.Empty && u.CompletDT.ToString() == string.Empty);


                            chldtrcknewcount = chldtrcknewcount.OrderBy(u => Convert.ToDateTime(u.SBCBDT)).ToList();



                            if (dtSBCB.Checked == true)
                            {
                                chldtrcknewcount = chldtrcknewcount.FindAll(u => Convert.ToDateTime(u.SBCBDT) <= Convert.ToDateTime(dtSBCB.Value));

                            }

                            if (chldtrcknewcount.Count > 0)
                            {
                                if (boolprintstatus)
                                {
                                    foreach (ChldTrckEntity dtrowitem in chldtrcknewcount)
                                    {
                                        excelrow3 = sheet.Table.Rows.Add();
                                        cell = excelrow3.Cells.Add(enrlrow.App, DataType.String, "NormalLeft");
                                        cell = excelrow3.Cells.Add(LookupDataAccess.GetMemberName(enrlrow.Snp_F_Name, enrlrow.Snp_M_Name, enrlrow.Snp_L_Name, BaseForm.BaseHierarchyCnFormat), DataType.String, "NormalLeft");
                                        cell = excelrow3.Cells.Add(LookupDataAccess.Getdate(enrlrow.Enrl_Date), DataType.String, "NormalLeft");
                                        cell = excelrow3.Cells.Add(enrlrow.Site + enrlrow.Room + enrlrow.AMPM, DataType.String, "NormalLeft");
                                        cell = excelrow3.Cells.Add((propfundingSource.Find(u => u.Code == enrlrow.FundHie).Desc.ToString()), DataType.String, "NormalLeft");
                                        cell = excelrow3.Cells.Add("", DataType.String, "NormalLeft");
                                        cell = excelrow3.Cells.Add("", DataType.String, "NormalLeft");
                                        cell = excelrow3.Cells.Add("", DataType.String, "NormalLeft");

                                        strAddresDt = dtrowitem.AddressDT;
                                        strFollowDt = dtrowitem.FollowDT;
                                        strCompledt = string.Empty;
                                        string strAddresFollwdt = "    ";
                                        string strSBCBDate = string.Empty;

                                        cell = excelrow3.Cells.Add(dtrowitem.TASKDESCRIPTION, DataType.String, "NormalLeft");
                                        cell = excelrow3.Cells.Add(dtrowitem.SBCBDT, DataType.String, "NormalLeft");
                                        cell = excelrow3.Cells.Add("", DataType.String, "NormalLeft");
                                        cell = excelrow3.Cells.Add("", DataType.String, "NormalLeft");
                                        cell = excelrow3.Cells.Add("", DataType.String, "NormalLeft");
                                        if (dtrowitem.AddressDT != string.Empty)
                                         cell = excelrow3.Cells.Add(LookupDataAccess.Getdate(dtrowitem.AddressDT), DataType.String, "NormalLeft"); 
                                        else
                                            cell = excelrow3.Cells.Add("", DataType.String, "NormalLeft");
                                        if (dtrowitem.FollowDT != string.Empty)
                                         cell = excelrow3.Cells.Add(LookupDataAccess.Getdate(dtrowitem.FollowDT), DataType.String, "NormalLeft");
                                        else
                                            cell = excelrow3.Cells.Add("", DataType.String, "NormalLeft");
                                        cell = excelrow3.Cells.Add("0" + enrlrow.Snp_Age, DataType.String, "NormalLeft");
                                        cell = excelrow3.Cells.Add(LookupDataAccess.Getdate(enrlrow.Snp_DOB), DataType.String, "NormalLeft");
                                    }
                                }
                            }

                            if (chldtrcknewcount.Count > 0)
                            {

                            }
                        }
                    }
                    FileStream stream1 = new FileStream(PdfName, FileMode.Create);
                    book.Save(stream1);
                    stream1.Close();
                }
                catch (Exception ex) { }
            }
        }

        #endregion

        /* private void On_SaveExcelClosed(object sender, FormClosedEventArgs e)
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
                     string Tmpstr = PdfName + ".xls";
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
                     PdfName += ".xls";

                 try
                 {

                     ExcelDocument xlWorkSheet = new ExcelDocument();
                     string strTaskCodes = string.Empty;
                     List<ChldTrckEntity> Track_allEntity;
                     if (rdoAllTask.Checked)
                     {

                         List<ChldTrckEntity> TrackList = _model.ChldTrckData.Browse_CasetrckDetails("01");
                         List<ChldTrckEntity> Track_Det = new List<ChldTrckEntity>();
                         if (((Captain.Common.Utilities.ListItem)CmbTrackComp.SelectedItem).Value.ToString().Trim() == "0000")
                         {
                             Track_Det = TrackList.FindAll(u => u.COMPONENT.Trim().Equals("0000"));
                         }
                         else
                             Track_Det = TrackList.FindAll(u => u.COMPONENT.Trim().Equals(((Captain.Common.Utilities.ListItem)CmbTrackComp.SelectedItem).Value.ToString().Trim()) && u.Agency.Trim().Equals(BaseForm.BaseAgency.Trim()) && u.Dept.Equals(BaseForm.BaseDept.Trim()) && u.Prog.Equals(BaseForm.BaseProg.Trim()));

                         foreach (ChldTrckEntity Entity in Track_Det)
                         {
                             if (!strTaskCodes.Equals(string.Empty)) strTaskCodes += ",";
                             strTaskCodes += Entity.TASK;
                         }
                         Track_allEntity = Track_Det;

                     }
                     else
                     {
                         foreach (ChldTrckEntity Entity in SeltrackList)
                         {
                             if (!strTaskCodes.Equals(string.Empty)) strTaskCodes += ",";
                             strTaskCodes += Entity.TASK;
                         }
                         Track_allEntity = SeltrackList;
                     }

                     List<CaseEnrlEntity> caseEnrlList;
                     if (((Captain.Common.Utilities.ListItem)cmbActive.SelectedItem).Value.ToString() == "B")
                     {
                         caseEnrlList = propCaseEnrlList.FindAll(u => Convert.ToInt32(u.Snp_Age) >= Convert.ToInt32(txtFrom.Text) && Convert.ToInt32(u.Snp_Age) <= Convert.ToInt32(txtTo.Text));
                     }
                     else
                     {
                         caseEnrlList = propCaseEnrlList.FindAll(u => u.MST_ActiveStatus == ((Captain.Common.Utilities.ListItem)cmbActive.SelectedItem).Value.ToString() && Convert.ToInt32(u.Snp_Age) >= Convert.ToInt32(txtFrom.Text) && Convert.ToInt32(u.Snp_Age) <= Convert.ToInt32(txtTo.Text));
                     }

                     if (rdoEnrolled.Checked)
                         caseEnrlList = caseEnrlList.FindAll(u => u.Status.Trim().Equals("E"));
                     if (rdounEnrolled.Checked)
                         caseEnrlList = caseEnrlList.FindAll(u => u.Status.Trim() != "E");
                     if (caseEnrlList.Count > 0)
                     {



                         xlWorkSheet.ColumnWidth(0, 0);
                         xlWorkSheet.ColumnWidth(1, 100);
                         xlWorkSheet.ColumnWidth(2, 200); 
                         xlWorkSheet.ColumnWidth(3, 100); 
                         xlWorkSheet.ColumnWidth(4, 100); 
                         xlWorkSheet.ColumnWidth(5, 100); 
                         xlWorkSheet.ColumnWidth(6, 100); 
                         xlWorkSheet.ColumnWidth(7, 100); 
                         xlWorkSheet.ColumnWidth(8, 100); 
                         xlWorkSheet.ColumnWidth(9, 300); 
                         xlWorkSheet.ColumnWidth(10, 100); 
                         xlWorkSheet.ColumnWidth(11, 100); 
                         xlWorkSheet.ColumnWidth(12, 100); 
                         xlWorkSheet.ColumnWidth(13, 100); 
                         xlWorkSheet.ColumnWidth(14, 100); 
                         xlWorkSheet.ColumnWidth(15, 80); 
                         xlWorkSheet.ColumnWidth(16, 50); 
                         xlWorkSheet.ColumnWidth(17, 80);

                         int excelcolumn = 0;

                         ExcelHeaderData(excelcolumn, xlWorkSheet);
                         excelcolumn = excelcolumn + 1;

                         // PdfPTable hssb2103_Table2 = new PdfPTable(8);


                         propChldMediEntity = _model.ChldTrckData.GetChldMedi_Report(Agency, Dept, Prog, "****", (txtApplicant.Text == string.Empty ? string.Empty : txtApplicant.Text), (dtSBCB.Checked == true ? dtSBCB.Value.ToShortDateString() : string.Empty), strTaskCodes, "Task", "TaskDesc");

                         //propChldMediEntity = propChldMediEntity.FindAll(u=>u.SBCB_DATE.Trim()!=string.Empty);


                         //PdfPCell Header1 = new PdfPCell(new Phrase("Applicant", TblFontBold));
                         //Header1.HorizontalAlignment = Element.ALIGN_CENTER;
                         //Header1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                         //Header1.Colspan = 2;
                         //hssb2103_Table2.AddCell(Header1);


                         //PdfPCell Header2 = new PdfPCell(new Phrase("Enrollment", TblFontBold));
                         //Header2.HorizontalAlignment = Element.ALIGN_CENTER;
                         //Header2.Colspan = 6;
                         //Header2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                         //hssb2103_Table2.AddCell(Header2);

                         string[] Headers = { "Number", "Name   ", "Date", "Class", "Funding", "Date", "Class", "Funding" };


                         foreach (CaseEnrlEntity enrlrow in caseEnrlList)
                         {



                             List<ChldMediEntity> chldMediEntity = propChldMediEntity.FindAll(u => (u.AGENCY == enrlrow.Agy) && (u.DEPT == enrlrow.Dept) && (u.PROG == enrlrow.Prog) && (u.APP_NO == enrlrow.App));//&& (u.YEAR == enrlrow.Year)

                             DataTable dtMediTask = CreateTable();
                             DataRow dtrow;
                             bool boolprintstatus = true;
                             string strAddresDt = string.Empty;
                             string strFollowDt = string.Empty;
                             string strCompledt = string.Empty;
                             string strSbcbdt = string.Empty;
                             List<ChldTrckEntity> chldTrckNewList = new List<ChldTrckEntity>();
                             foreach (ChldTrckEntity trackitem in Track_allEntity)
                             {
                                 try
                                 {
                                     strAddresDt = string.Empty;
                                     strFollowDt = string.Empty;
                                     strCompledt = string.Empty;
                                     List<ChldTrckREntity> chldtrackRList = propchldtrackRList.FindAll(u => u.TASK == trackitem.TASK);
                                     ChldTrckREntity chldtrackRListItem = propchldtrackRList.Find(u => u.TASK == trackitem.TASK && u.FUND.Trim().ToString() == enrlrow.FundHie.Trim().ToString());


                                     List<ChldMediEntity> chlditem = chldMediEntity.FindAll(u => u.TASK.Trim() == trackitem.TASK);
                                     if (chlditem.Count > 0)
                                     {
                                         chlditem = chlditem.OrderByDescending(u => Convert.ToInt32(u.YEAR)).ThenByDescending(u => Convert.ToInt32(u.SEQ)).ToList();//(u => Convert.ToInt32(u.SEQ)).ThenByDescending(u => u.YEAR).ToList();

                                         if (chlditem[0].COMPLETED_DATE == string.Empty)
                                         {
                                             strAddresDt = chlditem[0].ADDRESSED_DATE;
                                             strFollowDt = chlditem[0].FOLLOWUP_DATE;
                                             strCompledt = string.Empty;
                                             string strAddresFollwdt = "    ";
                                             string strSBCBDate = string.Empty;
                                             if (chlditem[0].ADDRESSED_DATE != string.Empty)
                                                 strAddresFollwdt = strAddresFollwdt + "ADDRESSED: " + LookupDataAccess.Getdate(chlditem[0].ADDRESSED_DATE) + "    ";
                                             if (chlditem[0].FOLLOWUP_DATE != string.Empty)
                                                 strAddresFollwdt = strAddresFollwdt + "FOLLOW UP: " + LookupDataAccess.Getdate(chlditem[0].FOLLOWUP_DATE);

                                         }
                                         else
                                         {

                                             strAddresDt = string.Empty; //chlditem[0].ADDRESSED_DATE;
                                             strFollowDt = chlditem[0].FOLLOWUP_DATE;
                                             strCompledt = chlditem[0].COMPLETED_DATE;
                                             ChldTrckREntity chldtrckrNextyear = chldtrackRList.Find(u => u.FUND.Trim().ToString() == enrlrow.FundHie.ToString());

                                             string strDate = string.Empty;
                                             if (chldtrckrNextyear != null)
                                             {


                                                 if (chldtrckrNextyear.NXTACTION.Trim() == "Y")
                                                 {
                                                     if (!string.IsNullOrEmpty(chldtrckrNextyear.NEXTDAYS))
                                                     {
                                                         if (Convert.ToInt32(chldtrckrNextyear.NEXTDAYS) >= 0)
                                                         {
                                                             if (chldtrckrNextyear.INTAKEENTRY.Trim().Equals("A"))
                                                             {
                                                                 if (!string.IsNullOrEmpty(enrlrow.MST_INTAKE_DT))
                                                                     strDate = Convert.ToDateTime(enrlrow.MST_INTAKE_DT).AddDays(Convert.ToInt32(chldtrckrNextyear.NEXTDAYS)).ToString();
                                                                 // break;
                                                             }
                                                             else if (chldtrckrNextyear.INTAKEENTRY.Trim().Equals("E"))
                                                             {
                                                                 //  strAttnDate = ChldAttnDB.GetChldAttnDate(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, strApplicationNo, caseEnrlCheckRecord.Site, caseEnrlCheckRecord.Room, caseEnrlCheckRecord.AMPM, caseEnrlCheckRecord.FundHie, string.Empty);
                                                                 if (!string.IsNullOrEmpty(enrlrow.AttendedDt))
                                                                     strDate = Convert.ToDateTime(enrlrow.AttendedDt).AddDays(Convert.ToInt32(chldtrckrNextyear.NEXTDAYS)).ToString();
                                                                 if (!string.IsNullOrEmpty(enrlrow.AttendedDt))
                                                                     strDate = Convert.ToDateTime(enrlrow.AttendedDt).AddDays(Convert.ToInt32(chldtrckrNextyear.NEXTDAYS)).ToString();
                                                             }
                                                             else if (chldtrckrNextyear.INTAKEENTRY.Trim().Equals("D"))
                                                             {

                                                                 strDate = Convert.ToDateTime(enrlrow.Snp_DOB).AddDays(Convert.ToInt32(chldtrckrNextyear.NEXTDAYS)).ToString();
                                                                 // break;
                                                             }
                                                         }
                                                         // break;
                                                     }

                                                 }
                                             }
                                         }
                                     }

                                     if (chldtrackRListItem != null)
                                     {
                                         chldtrackRListItem.CompletDt = strCompledt;
                                         strSbcbdt = LookupDataAccess.Getdate(FillSBCDDetails(trackitem.TASK, enrlrow, chldtrackRListItem));

                                         chldTrckNewList.Add(new ChldTrckEntity(trackitem.TASK, trackitem.TASKDESCRIPTION, chldtrackRListItem.REQUIREYEAR, chldtrackRListItem.ENTERDAYS, chldtrackRListItem.NXTACTION, chldtrackRListItem.NEXTTASK, chldtrackRListItem.NEXTDAYS, strSbcbdt, strCompledt, strFollowDt, strAddresDt, chldtrackRListItem.INTAKEENTRY, trackitem.CustQCodes));
                                     }
                                     else
                                     {
                                         chldTrckNewList.Add(new ChldTrckEntity(trackitem.TASK, trackitem.TASKDESCRIPTION, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, trackitem.CustQCodes));
                                     }


                                 }
                                 catch (Exception ex)
                                 {

                                     // throw;
                                 }

                                 //}
                             }


                             foreach (ChldTrckEntity item in chldTrckNewList)
                             {



                                 if (item.NEXTYN.Trim().ToString() == "Y" && item.CompletDT.ToString() != string.Empty)
                                 {
                                     if (!string.IsNullOrEmpty(item.NEXTDAYS))
                                     {
                                         string strSbcbdate = string.Empty;
                                         if (Convert.ToInt32(item.NEXTDAYS) >= 0)
                                         {
                                             if (item.IntakeType.Trim().Equals("A"))
                                             {
                                                 if (!string.IsNullOrEmpty(enrlrow.MST_INTAKE_DT))
                                                     strSbcbdate = Convert.ToDateTime(enrlrow.MST_INTAKE_DT).AddDays(Convert.ToInt32(item.NEXTDAYS)).ToString();
                                                 // break;
                                             }
                                             else if (item.IntakeType.Trim().Equals("E"))
                                             {
                                                 //  strAttnDate = ChldAttnDB.GetChldAttnDate(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, strApplicationNo, caseEnrlCheckRecord.Site, caseEnrlCheckRecord.Room, caseEnrlCheckRecord.AMPM, caseEnrlCheckRecord.FundHie, string.Empty);
                                                 if (!string.IsNullOrEmpty(enrlrow.AttendedDt))
                                                     strSbcbdate = Convert.ToDateTime(enrlrow.AttendedDt).AddDays(Convert.ToInt32(item.NEXTDAYS)).ToString();
                                                 if (!string.IsNullOrEmpty(enrlrow.AttendedDt))
                                                     strSbcbdate = Convert.ToDateTime(enrlrow.AttendedDt).AddDays(Convert.ToInt32(item.NEXTDAYS)).ToString();
                                             }
                                             else if (item.IntakeType.Trim().Equals("D"))
                                             {

                                                 strSbcbdate = Convert.ToDateTime(enrlrow.Snp_DOB).AddDays(Convert.ToInt32(item.NEXTDAYS)).ToString();
                                                 // break;
                                             }
                                         }

                                         if (strSbcbdate != string.Empty)
                                         {
                                             if (item.TASK.ToString() == item.NEXTTASK.ToString())
                                             {
                                                 if (item.CompletDT.ToString() != string.Empty)
                                                 {
                                                     DateTime dtcomple = Convert.ToDateTime(item.CompletDT).Date;
                                                     DateTime dtSbcb = Convert.ToDateTime(strSbcbdate).Date;
                                                     if (dtcomple < dtSbcb)
                                                     {
                                                         item.CompletDT = string.Empty;
                                                         item.SBCBDT = dtSbcb.Date.ToShortDateString();
                                                     }
                                                 }
                                             }
                                             else
                                             {
                                                 ChldTrckEntity chldtrckNewTaskItem = chldTrckNewList.Find(u => u.TASK == item.NEXTTASK);
                                                 if (chldtrckNewTaskItem != null)
                                                 {
                                                     if (chldtrckNewTaskItem.CompletDT.ToString() != string.Empty)
                                                     {
                                                         DateTime dtcomple = Convert.ToDateTime(item.CompletDT).Date;
                                                         DateTime dtSbcb = Convert.ToDateTime(strSbcbdate).Date;
                                                         if (dtcomple < dtSbcb)
                                                         {
                                                             chldTrckNewList.Find(u => u.TASK == item.NEXTTASK).CompletDT = string.Empty;
                                                             chldTrckNewList.Find(u => u.TASK == item.NEXTTASK).SBCBDT = dtSbcb.Date.ToShortDateString();
                                                         }
                                                     }
                                                 }
                                             }
                                         }

                                         // break;
                                     }
                                 }


                             }

                             List<ChldTrckEntity> chldtrcknewcount = chldTrckNewList.FindAll(u => u.SBCBDT.ToString() != string.Empty && u.CompletDT.ToString() == string.Empty);


                             chldtrcknewcount = chldtrcknewcount.OrderBy(u => Convert.ToDateTime(u.SBCBDT)).ToList();



                             if (dtSBCB.Checked == true)
                             {
                                 chldtrcknewcount = chldtrcknewcount.FindAll(u => Convert.ToDateTime(u.SBCBDT) <= Convert.ToDateTime(dtSBCB.Value));

                             }


                             if (chldtrcknewcount.Count > 0)
                             {

                                 if (boolprintstatus)
                                 {

                                     foreach (ChldTrckEntity dtrowitem in chldtrcknewcount)
                                     {
                                         excelcolumn = excelcolumn + 1;

                                         xlWorkSheet.WriteCell(excelcolumn, 1, enrlrow.App);

                                         //PdfPCell pdfAppldata = new PdfPCell(new Phrase(enrlrow.App, TableFont));
                                         //pdfAppldata.HorizontalAlignment = Element.ALIGN_LEFT;
                                         //pdfAppldata.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                         //hssb2103_Table2.AddCell(pdfAppldata);

                                         xlWorkSheet.WriteCell(excelcolumn, 2, LookupDataAccess.GetMemberName(enrlrow.Snp_F_Name, enrlrow.Snp_M_Name, enrlrow.Snp_L_Name, BaseForm.BaseHierarchyCnFormat));
                                         //PdfPCell pdfChildName = new PdfPCell(new Phrase(LookupDataAccess.GetMemberName(enrlrow.Snp_F_Name, enrlrow.Snp_M_Name, enrlrow.Snp_L_Name, BaseForm.BaseHierarchyCnFormat), TableFont));
                                         //pdfChildName.HorizontalAlignment = Element.ALIGN_LEFT;
                                         //pdfChildName.Colspan = 1;
                                         //pdfChildName.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                         //hssb2103_Table2.AddCell(pdfChildName);

                                         xlWorkSheet.WriteCell(excelcolumn, 3, LookupDataAccess.Getdate(enrlrow.Enrl_Date));
                                         //PdfPCell pdfDob = new PdfPCell(new Phrase(LookupDataAccess.Getdate(enrlrow.Enrl_Date), TableFont));
                                         //pdfDob.HorizontalAlignment = Element.ALIGN_CENTER;
                                         //pdfDob.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                         //hssb2103_Table2.AddCell(pdfDob);

                                         xlWorkSheet.WriteCell(excelcolumn, 4, enrlrow.Site + enrlrow.Room + enrlrow.AMPM);
                                         //PdfPCell pdfSiteRoom = new PdfPCell(new Phrase(enrlrow.Site + enrlrow.Room + enrlrow.AMPM, TableFont));
                                         //pdfSiteRoom.HorizontalAlignment = Element.ALIGN_LEFT;
                                         //pdfSiteRoom.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                         //hssb2103_Table2.AddCell(pdfSiteRoom);

                                         xlWorkSheet.WriteCell(excelcolumn, 5, (propfundingSource.Find(u => u.Code == enrlrow.FundHie).Desc.ToString()));
                                         //PdfPCell pdfFundSource = new PdfPCell(new Phrase((propfundingSource.Find(u => u.Code == enrlrow.FundHie).Desc.ToString()), TableFont));
                                         //pdfFundSource.HorizontalAlignment = Element.ALIGN_LEFT;
                                         //pdfFundSource.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                         //hssb2103_Table2.AddCell(pdfFundSource);

                                         xlWorkSheet.WriteCell(excelcolumn, 6, "");
                                         //PdfPCell pdfDob2 = new PdfPCell(new Phrase("", TableFont));
                                         //pdfDob2.HorizontalAlignment = Element.ALIGN_CENTER;
                                         //pdfDob2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                         //hssb2103_Table2.AddCell(pdfDob2);

                                         xlWorkSheet.WriteCell(excelcolumn, 7, "");
                                         //PdfPCell pdfSiteRoom2 = new PdfPCell(new Phrase("", TableFont));
                                         //pdfSiteRoom2.HorizontalAlignment = Element.ALIGN_LEFT;
                                         //pdfSiteRoom2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                         //hssb2103_Table2.AddCell(pdfSiteRoom2);

                                         xlWorkSheet.WriteCell(excelcolumn, 8, "");
                                         //PdfPCell pdfFundSource2 = new PdfPCell(new Phrase("", TableFont));
                                         //pdfFundSource2.HorizontalAlignment = Element.ALIGN_LEFT;
                                         //pdfFundSource2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                         //hssb2103_Table2.AddCell(pdfFundSource2);

                                         // xlWorkSheet.WriteCell(excelcolumn, 9, "");
                                         //PdfPCell pdfSpace = new PdfPCell(new Phrase((""), TableFont));
                                         //pdfSpace.HorizontalAlignment = Element.ALIGN_LEFT;
                                         //pdfSpace.Colspan = 8;
                                         //pdfSpace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                         //hssb2103_Table2.AddCell(pdfSpace);

                                         //xlWorkSheet.WriteCell(excelcolumn, 2, enrlrow.);
                                         //PdfPCell pdfSpace2 = new PdfPCell(new Phrase((""), TableFont));
                                         //pdfSpace2.HorizontalAlignment = Element.ALIGN_LEFT;
                                         //pdfSpace2.Colspan = 8;
                                         //pdfSpace2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                         //hssb2103_Table2.AddCell(pdfSpace2);

                                         //PdfPCell pdfSpace3 = new PdfPCell(new Phrase("", TableFont));
                                         //pdfSpace3.HorizontalAlignment = Element.ALIGN_LEFT;
                                         //pdfSpace3.Colspan = 8;
                                         //pdfSpace3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                         //hssb2103_Table2.AddCell(pdfSpace3);

                                         strAddresDt = dtrowitem.AddressDT;
                                         strFollowDt = dtrowitem.FollowDT;
                                         strCompledt = string.Empty;
                                         string strAddresFollwdt = "    ";
                                         string strSBCBDate = string.Empty;
                                         //if (dtrowitem.AddressDT != string.Empty)
                                         //    strAddresFollwdt = strAddresFollwdt + "ADDRESSED: " + LookupDataAccess.Getdate(dtrowitem.AddressDT) + "    ";
                                         //if (dtrowitem.FollowDT != string.Empty)
                                         //    strAddresFollwdt = strAddresFollwdt + "FOLLOW UP: " + LookupDataAccess.Getdate(dtrowitem.FollowDT);


                                         xlWorkSheet.WriteCell(excelcolumn, 9, dtrowitem.TASKDESCRIPTION);

                                         xlWorkSheet.WriteCell(excelcolumn, 10, dtrowitem.SBCBDT);

                                         if (dtrowitem.AddressDT != string.Empty)
                                             xlWorkSheet.WriteCell(excelcolumn, 14, LookupDataAccess.Getdate(dtrowitem.AddressDT));

                                         if (dtrowitem.FollowDT != string.Empty)
                                             xlWorkSheet.WriteCell(excelcolumn, 15, LookupDataAccess.Getdate(dtrowitem.FollowDT));

                                         xlWorkSheet.WriteCell(excelcolumn, 16, "0" + enrlrow.Snp_Age);

                                         xlWorkSheet.WriteCell(excelcolumn, 17, LookupDataAccess.Getdate(enrlrow.Snp_DOB));

                                         //PdfPCell pdfData1 = new PdfPCell(new Phrase((dtrowitem.TASKDESCRIPTION + "  : SBCB: " + dtrowitem.SBCBDT + strAddresFollwdt.ToString()), TableFont));
                                         //pdfData1.HorizontalAlignment = Element.ALIGN_LEFT;
                                         //pdfData1.Colspan = 8;
                                         //pdfData1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                         //hssb2103_Table2.AddCell(pdfData1);

                                         // fillCustQuestions(dtrowitem.CustQCodes.ToString(), hssb2103_Table2);
                                     }
                                 }
                             }
                             if (chldtrcknewcount.Count > 0)
                             {
                                 //PdfPCell pdfChildName2 = new PdfPCell(new Phrase(LookupDataAccess.GetMemberName(enrlrow.Snp_F_Name, enrlrow.Snp_M_Name, enrlrow.Snp_L_Name, BaseForm.BaseHierarchyCnFormat), TableFont));
                                 //pdfChildName2.HorizontalAlignment = Element.ALIGN_CENTER;
                                 //pdfChildName2.Colspan = 2;
                                 //pdfChildName2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                 //hssb2103_Table2.AddCell(pdfChildName2);

                                 //PdfPCell pdfTaskCount = new PdfPCell(new Phrase(chldtrcknewcount.Count + "  Tasks   Age : 0" + enrlrow.Snp_Age + "  BD  " + LookupDataAccess.Getdate(enrlrow.Snp_DOB) + "  AS  " + LookupDataAccess.Getdate(enrlrow.MSTZIPDATE), TableFont));
                                 //pdfTaskCount.HorizontalAlignment = Element.ALIGN_LEFT;
                                 //pdfTaskCount.Colspan = 6;
                                 //pdfTaskCount.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                 //hssb2103_Table2.AddCell(pdfTaskCount);

                                 //PdfPCell pdfSpace7 = new PdfPCell(new Phrase("", TableFont));
                                 //pdfSpace7.HorizontalAlignment = Element.ALIGN_LEFT;
                                 //pdfSpace7.Colspan = 8;
                                 //pdfSpace7.FixedHeight = 10f;
                                 //pdfSpace7.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                 //hssb2103_Table2.AddCell(pdfSpace7);
                             }
                         }

                     }

                     FileStream stream = new FileStream(PdfName, FileMode.Create);

                     xlWorkSheet.Save(stream);
                     stream.Close();
                 }
                 catch (Exception ex)
                 {

                 }

             }
         }*/

        private void ExcelHeaderData(int intcolumn, ExcelDocument xlWorkSheet)
        {
            xlWorkSheet[intcolumn, 1].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
            xlWorkSheet[intcolumn, 1].Alignment = Alignment.Centered;
            xlWorkSheet.WriteCell(intcolumn, 1, "Number");

            xlWorkSheet[intcolumn, 2].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
            xlWorkSheet[intcolumn, 2].Alignment = Alignment.Centered;
            xlWorkSheet.WriteCell(intcolumn, 2, "Name");

            xlWorkSheet[intcolumn, 3].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
            xlWorkSheet[intcolumn, 3].Alignment = Alignment.Centered;
            xlWorkSheet.WriteCell(intcolumn, 3, "Date");

            xlWorkSheet[intcolumn, 4].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
            xlWorkSheet[intcolumn, 4].Alignment = Alignment.Centered;
            xlWorkSheet.WriteCell(intcolumn, 4, "Class");

            xlWorkSheet[intcolumn, 5].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
            xlWorkSheet[intcolumn, 5].Alignment = Alignment.Centered;
            xlWorkSheet.WriteCell(intcolumn, 5, "Funding");

            xlWorkSheet[intcolumn, 6].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
            xlWorkSheet[intcolumn, 6].Alignment = Alignment.Centered;
            xlWorkSheet.WriteCell(intcolumn, 6, "Date");

            xlWorkSheet[intcolumn, 7].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
            xlWorkSheet[intcolumn, 7].Alignment = Alignment.Centered;
            xlWorkSheet.WriteCell(intcolumn, 7, "Class");

            xlWorkSheet[intcolumn, 8].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
            xlWorkSheet[intcolumn, 8].Alignment = Alignment.Centered;
            xlWorkSheet.WriteCell(intcolumn, 8, "Funding");

            xlWorkSheet[intcolumn, 9].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
            xlWorkSheet[intcolumn, 9].Alignment = Alignment.Centered;
            xlWorkSheet.WriteCell(intcolumn, 9, "Task");

            xlWorkSheet[intcolumn, 10].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
            xlWorkSheet[intcolumn, 10].Alignment = Alignment.Centered;
            xlWorkSheet.WriteCell(intcolumn, 10, "SBCD");

            xlWorkSheet[intcolumn, 11].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
            xlWorkSheet[intcolumn, 11].Alignment = Alignment.Centered;
            xlWorkSheet.WriteCell(intcolumn, 11, "Result:(D)");

            xlWorkSheet[intcolumn, 12].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
            xlWorkSheet[intcolumn, 12].Alignment = Alignment.Centered;
            xlWorkSheet.WriteCell(intcolumn, 12, "Date");

            xlWorkSheet[intcolumn, 13].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
            xlWorkSheet[intcolumn, 13].Alignment = Alignment.Centered;
            xlWorkSheet.WriteCell(intcolumn, 13, "Comments");

            xlWorkSheet[intcolumn, 14].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
            xlWorkSheet[intcolumn, 14].Alignment = Alignment.Centered;
            xlWorkSheet.WriteCell(intcolumn, 14, "Addressed Date");

            xlWorkSheet[intcolumn, 15].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
            xlWorkSheet[intcolumn, 15].Alignment = Alignment.Centered;
            xlWorkSheet.WriteCell(intcolumn, 15, "Follow up Date");


            xlWorkSheet[intcolumn, 16].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
            xlWorkSheet[intcolumn, 16].Alignment = Alignment.Centered;
            xlWorkSheet.WriteCell(intcolumn, 16, "Age");

            xlWorkSheet[intcolumn, 17].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
            xlWorkSheet[intcolumn, 17].Alignment = Alignment.Centered;
            xlWorkSheet.WriteCell(intcolumn, 17, "DOB");

        }
        #endregion

        #endregion

        private void rdoMultipleSites_Click(object sender, EventArgs e)
        {
            if (rdoMultipleSites.Checked == true)
            {
                Site_SelectionForm SiteSelection = new Site_SelectionForm(BaseForm, "Room", Sel_REFS_List, "Report", Agency.Trim(), Dept.Trim(), Prog.Trim(), Program_Year, Privileges);
                SiteSelection.FormClosed += new FormClosedEventHandler(On_Room_Select_Closed);
                SiteSelection.StartPosition = FormStartPosition.CenterScreen;
                SiteSelection.ShowDialog();
            }
        }

        List<CaseSiteEntity> Sel_REFS_List = new List<CaseSiteEntity>();
        private void On_Room_Select_Closed(object sender, FormClosedEventArgs e)
        {
            //string SelRef_Name = null;
            string Sql_MSg = string.Empty;
            Site_SelectionForm form = sender as Site_SelectionForm;
            if (form.DialogResult == DialogResult.OK)
            {
                Sel_REFS_List = form.GetSelected_Sites();
            }
        }

        private void rdoMultipleTask_Click(object sender, EventArgs e)
        {
            if (rdoMultipleTask.Checked == true)
            {
                HSSB2111FundForm TaskForm = new HSSB2111FundForm(BaseForm, SeltrackList, Privileges, ((Captain.Common.Utilities.ListItem)CmbTrackComp.SelectedItem).Value.ToString().Trim(), string.Empty, string.Empty);
                TaskForm.FormClosed += new FormClosedEventHandler(TaskForm_FormClosed);
                TaskForm.StartPosition = FormStartPosition.CenterScreen;
                TaskForm.ShowDialog();
            }
        }

        List<ChldTrckEntity> SeltrackList = new List<ChldTrckEntity>();
        private void TaskForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            HSSB2111FundForm form = sender as HSSB2111FundForm;
            if (form.DialogResult == DialogResult.OK)
            {
                SeltrackList = form.GetSelectedTracks();
            }
        }

        private void rdoSelectedApplicantSeq_CheckedChanged(object sender, EventArgs e)
        {
            _errorProvider.SetError(btnBrowse, null);
            if (rdoSelectedApplicantSeq.Checked == true)
            {
                txtApplicant.Enabled = true;
                btnBrowse.Visible = true;
                lblApplicantReq.Visible = true;
                rdoMultipleSites.Enabled = false;
                rdoAllSite.Checked = true;
            }
            else
            {
                txtApplicant.Clear();
                txtApplicant.Enabled = false;
                btnBrowse.Visible = false;
                lblApplicantReq.Visible = false;
                rdoMultipleSites.Enabled = true;
            }
        }

        private void cmbFundingSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((Captain.Common.Utilities.ListItem)cmbFundingSource.SelectedItem).Value.ToString() == "D")
                btnFunding.Visible = true;
            else
            { 
                btnFunding.Visible = false; _errorProvider.SetError(btnFunding, null);
                SelFundingList.Clear();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //if (rdoFundingSelect.Checked == true)
            //{
            HSSB2111FundForm FundingForm = new HSSB2111FundForm(BaseForm, SelFundingList, Privileges, "DayCare");
            FundingForm.FormClosed += new FormClosedEventHandler(FundingForm_FormClosed);
            FundingForm.StartPosition = FormStartPosition.CenterScreen;
            FundingForm.ShowDialog();
            //}
        }

        List<CommonEntity> SelFundingList = new List<CommonEntity>();
        void FundingForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            HSSB2111FundForm form = sender as HSSB2111FundForm;
            if (form.DialogResult == DialogResult.OK)
            {
                SelFundingList = form.GetSelectedFundings();
            }
        }

        private void rdoEnrolled_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoEnrolled.Checked)
                cmbFundingSource.Enabled = true;
            else
                cmbFundingSource.Enabled = false;
            cmbFundingSource.SelectedIndex = 2;
        }

        private void Pb_Help_Click(object sender, EventArgs e)
        {
           // Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "HSSB2114");
        }


        private string FillSBCDDetails(string strTask, CaseEnrlEntity caseEnrlCheckRecord, List<ChldTrckREntity> chldtrackRList)
        {
            string strDate = string.Empty;
            // List<ChldTrckREntity> chldtrackRList = propchldtrackRList.FindAll(u => u.TASK == strTask); // _model.ChldTrckData.GetCasetrckrDetails(string.Empty, string.Empty, string.Empty, "0000", propTask, string.Empty);
            if (chldtrackRList.Count > 0)
            {
                if (chldtrackRList.Count > 0)
                {
                    foreach (ChldTrckREntity chldtrackitem in chldtrackRList)
                    {
                        if (chldtrackitem.FUND.Trim() == caseEnrlCheckRecord.FundHie.ToString())
                        {
                            if (!string.IsNullOrEmpty(chldtrackitem.ENTERDAYS))
                            {
                                if (Convert.ToInt32(chldtrackitem.ENTERDAYS) >= 0)
                                {
                                    if (chldtrackitem.INTAKEENTRY.Trim().Equals("A"))
                                    {
                                        if (!string.IsNullOrEmpty(caseEnrlCheckRecord.MST_INTAKE_DT))
                                            strDate = Convert.ToDateTime(caseEnrlCheckRecord.MST_INTAKE_DT).AddDays(Convert.ToInt32(chldtrackitem.ENTERDAYS)).ToString();
                                        break;
                                    }
                                    else if (chldtrackitem.INTAKEENTRY.Trim().Equals("E"))
                                    {
                                        //  strAttnDate = ChldAttnDB.GetChldAttnDate(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, strApplicationNo, caseEnrlCheckRecord.Site, caseEnrlCheckRecord.Room, caseEnrlCheckRecord.AMPM, caseEnrlCheckRecord.FundHie, string.Empty);
                                        if (!string.IsNullOrEmpty(caseEnrlCheckRecord.AttendedDt) && chldtrackitem.REQUIREYEAR.Trim() != "Y")
                                            strDate = Convert.ToDateTime(caseEnrlCheckRecord.AttendedDt).AddDays(Convert.ToInt32(chldtrackitem.ENTERDAYS)).ToString();
                                        if (!string.IsNullOrEmpty(caseEnrlCheckRecord.AttendedDt) && chldtrackitem.REQUIREYEAR.Trim() == "Y")
                                            strDate = Convert.ToDateTime(caseEnrlCheckRecord.AttendedDt).AddDays(Convert.ToInt32(chldtrackitem.ENTERDAYS)).ToString();
                                    }
                                    else if (chldtrackitem.INTAKEENTRY.Trim().Equals("D"))
                                    {

                                        strDate = Convert.ToDateTime(caseEnrlCheckRecord.Snp_DOB).AddDays(Convert.ToInt32(chldtrackitem.ENTERDAYS)).ToString();
                                        break;
                                    }
                                }
                                break;
                            }
                            break;
                        }

                    }
                }
            }
            return strDate;
        }


        private string FillSBCDDetails(string strTask, CaseEnrlEntity caseEnrlCheckRecord, ChldTrckREntity chldtrackRList)
        {
            string strDate = string.Empty;
            // List<ChldTrckREntity> chldtrackRList = propchldtrackRList.FindAll(u => u.TASK == strTask); // _model.ChldTrckData.GetCasetrckrDetails(string.Empty, string.Empty, string.Empty, "0000", propTask, string.Empty);
            if (chldtrackRList != null)
            {
                if (chldtrackRList != null)
                {
                    if (chldtrackRList.FUND.Trim() == caseEnrlCheckRecord.FundHie.ToString())
                    {
                        if (!string.IsNullOrEmpty(chldtrackRList.ENTERDAYS))
                        {
                            if (Convert.ToInt32(chldtrackRList.ENTERDAYS) >= 0)
                            {
                                if (chldtrackRList.INTAKEENTRY.Trim().Equals("A"))
                                {
                                    if (!string.IsNullOrEmpty(caseEnrlCheckRecord.MST_INTAKE_DT))
                                        strDate = Convert.ToDateTime(caseEnrlCheckRecord.MST_INTAKE_DT).AddDays(Convert.ToInt32(chldtrackRList.ENTERDAYS)).ToString();

                                }
                                else if (chldtrackRList.INTAKEENTRY.Trim().Equals("E"))
                                {
                                    string strYear = string.Empty;
                                    if (chldtrackRList.CompletDt != string.Empty)
                                        strYear = Convert.ToDateTime(chldtrackRList.CompletDt).Year.ToString();
                                    //  strAttnDate = ChldAttnDB.GetChldAttnDate(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, strApplicationNo, caseEnrlCheckRecord.Site, caseEnrlCheckRecord.Room, caseEnrlCheckRecord.AMPM, caseEnrlCheckRecord.FundHie, string.Empty);
                                    if (!string.IsNullOrEmpty(caseEnrlCheckRecord.AttendedDt) && chldtrackRList.REQUIREYEAR.Trim() != "Y" && chldtrackRList.CompletDt.Trim().ToString() == string.Empty)
                                        strDate = Convert.ToDateTime(caseEnrlCheckRecord.AttendedDt).AddDays(Convert.ToInt32(chldtrackRList.ENTERDAYS)).ToString();
                                    if (!string.IsNullOrEmpty(caseEnrlCheckRecord.AttendedDt) && chldtrackRList.REQUIREYEAR.Trim() == "Y" && strYear != ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString().Trim())
                                    {
                                        strDate = Convert.ToDateTime(caseEnrlCheckRecord.AttendedDt).AddDays(Convert.ToInt32(chldtrackRList.ENTERDAYS)).ToString();
                                        chldtrackRList.CompletDt = string.Empty;
                                    }
                                }
                                else if (chldtrackRList.INTAKEENTRY.Trim().Equals("D"))
                                {

                                    strDate = Convert.ToDateTime(caseEnrlCheckRecord.Snp_DOB).AddDays(Convert.ToInt32(chldtrackRList.ENTERDAYS)).ToString();

                                }
                            }
                        }

                    }
                }
            }
            return strDate;
        }

        private void HSSB2114ReportForm_ToolClick(object sender, ToolClickEventArgs e)
        {
            //Application.Navigate(CommonFunctions.BuildHelpURLS(Privileges.Program, 0, BaseForm.BusinessModuleID.ToString()), target: "_blank");
            Application.Navigate(CommonFunctions.CreateZenHelps(Privileges.Program, 0, BaseForm.BusinessModuleID.ToString(), "", ""), target: "_blank");
        }

        private DataTable CreateTable()
        {
            try
            {
                DataTable table = new DataTable();

                // Declare DataColumn and DataRow variables.
                DataColumn column;

                // Create new DataColumn, set DataType, ColumnName
                // and add to DataTable.    
                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "CustQues";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "TaskId";
                table.Columns.Add(column);

                // Create second column.
                column = new DataColumn();
                column.DataType = Type.GetType("System.String");
                column.ColumnName = "TaskDesc";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = typeof(DateTime); ;
                column.ColumnName = "SBCBDt";
                table.Columns.Add(column);


                return table;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void rdoAllSite_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoAllSite.Checked)
            { 
                _errorProvider.SetError(rdoMultipleSites, null);
                Sel_REFS_List.Clear();
            }
        }

        private void rdoAllTask_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoAllTask.Checked)
            { 
                _errorProvider.SetError(rdoMultipleTask, null);
                SeltrackList.Clear();
            }
        }

    }
}