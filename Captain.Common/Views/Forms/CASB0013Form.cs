#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using Wisej.Web;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using XLSExportFile;
using Captain.DatabaseLayer;
using CarlosAg.ExcelXmlWriter;
using DevExpress.CodeParser;
using System.Security.Cryptography;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class CASB0013Form : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;


        #endregion
        public CASB0013Form(BaseForm baseForm, PrivilegeEntity privileges)
        {
            InitializeComponent();
            _model = new CaptainModel();
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
            propReportPath = _model.lookupDataAccess.GetReportPath();
            Rep_From_Date.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            Rep_To_Date.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Month, DateTime.Today.Month));
            int intmonth = DateTime.Today.Month + 1;
            int intyear = DateTime.Today.Year - 1;
            Rb_MS_Date.Focus();
            if (DateTime.Today.Month == 12)
            {
                intmonth = 1;
                intyear = DateTime.Today.Year;
            }
            Ref_From_Date.Value = new DateTime(intyear, intmonth, 1);
            Ref_To_Date.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Month, DateTime.Today.Month));


        }

        #region properties

        public BaseForm BaseForm { get; set; }
        public PrivilegeEntity Privileges { get; set; }

        public string propReportPath { get; set; }
        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Prog { get; set; }



        #endregion

        private void CmbYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program_Year = "    ";
            if (!(string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString())))
            {
                Program_Year = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString();
            }
        }

        private void btnGeneratePdf_Click(object sender, EventArgs e)
        {
            _errorProvider.SetError(Ref_To_Date, null); _errorProvider.SetError(Rep_To_Date, null); _errorProvider.SetError(Ref_From_Date, null); _errorProvider.SetError(Rep_From_Date, null);
            if (gvwProgramData.Rows.Count > 0)
            {
                if (chkbExcel.Checked == true)
                {
                    PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, false, propReportPath,"XLS");
                    pdfListForm.FormClosed += new FormClosedEventHandler(On_SaveFormClosed);
                    pdfListForm.StartPosition= FormStartPosition.CenterScreen;
                    pdfListForm.ShowDialog();
                }
                else
                {
                    PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, false, propReportPath,"PDF");
                    pdfListForm.FormClosed += new FormClosedEventHandler(On_SaveFormClosed);
                    pdfListForm.StartPosition = FormStartPosition.CenterScreen;
                    pdfListForm.ShowDialog();
                }
            }
            else
            {
                AlertBox.Show("Please Click on Process button first", MessageBoxIcon.Warning);
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

        private string Get_XML_Format_for_Report_Controls()
        {

            return "".ToString();
        }

        private void Set_Report_Controls(DataTable Tmp_Table)
        {
            if (Tmp_Table != null && Tmp_Table.Rows.Count > 0)
            {
                DataRow dr = Tmp_Table.Rows[0];


            }
        }



        private void Pb_Search_Hie_Click(object sender, EventArgs e)
        {
            //HierarchieSelectionFormNew hierarchieSelectionForm = new HierarchieSelectionFormNew(BaseForm, Current_Hierarchy_DB, "Master", "A", "*", "Reports");
            //hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
            //hierarchieSelectionForm.StartPosition = FormStartPosition.CenterScreen;
            //hierarchieSelectionForm.ShowDialog();

            HierarchieSelection hierarchieSelectionForm = new HierarchieSelection(BaseForm, Current_Hierarchy_DB, "Master", "A", "*", "Reports", BaseForm.UserID);
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
        DataSet dsresult;
        private void btnProcess_Click(object sender, EventArgs e)
        {
            try
            {
                _errorProvider.SetError(Ref_To_Date, null); _errorProvider.SetError(Rep_To_Date, null); _errorProvider.SetError(Ref_From_Date, null); _errorProvider.SetError(Rep_From_Date, null);

                if (ValidReport())
                {
                    gvwProgramData.Rows.Clear();
                    if (rdoServiceCount.Checked)
                    {
                        gvtServicecode.Visible = true;
                        gvtProgramDesc.Width = 200;
                        gvtReportcount.ShowInVisibilityMenu= true;
                        gvtRefcount.ShowInVisibilityMenu = true;
                        gvtServicecode.ShowInVisibilityMenu = true;
                    }
                    else
                    {
                        gvtServicecode.Visible = false;
                        gvtProgramDesc.Width = 400;
                        gvtReportcount.ShowInVisibilityMenu = true;
                        gvtRefcount.ShowInVisibilityMenu = true;
                        gvtServicecode.ShowInVisibilityMenu = false;
                    }
                    string strcoltype = string.Empty;
                    if (Rb_MS_Date.Checked)
                        strcoltype = "CADATE";

                    dsresult = SPAdminDB.CAPS_CASB0013_GET(strcoltype, Ref_From_Date.Value.ToShortDateString(), Ref_To_Date.Value.ToShortDateString(), Rep_From_Date.Value.ToShortDateString(), Rep_To_Date.Value.ToShortDateString(), Agency, Dept, Prog, Program_Year, rdoServiceCount.Checked == true ? "S" : "P", chkbUndupTable.Checked == true ? "Y" : "N");
                    gvtReportcount.HeaderText = LookupDataAccess.Getdate(Rep_From_Date.Value.ToString()) + " - " + LookupDataAccess.Getdate(Rep_To_Date.Value.ToString());
                    gvtRefcount.HeaderText = LookupDataAccess.Getdate(Ref_From_Date.Value.ToString()) + " - " + LookupDataAccess.Getdate(Ref_To_Date.Value.ToString());

                    int intindex;
                    //gvwProgramData.Rows.Add("Program", "Service/Activity", LookupDataAccess.Getdate(Rep_From_Date.Value.ToString()) + " - " + LookupDataAccess.Getdate(Rep_To_Date.Value.ToString()), LookupDataAccess.Getdate(Ref_From_Date.Value.ToString()) + " - " + LookupDataAccess.Getdate(Ref_To_Date.Value.ToString()));
                    if (dsresult.Tables.Count > 0)
                    {
                        if (dsresult.Tables[1].Rows.Count > 0)
                        {
                            foreach (DataRow item in dsresult.Tables[1].Rows)
                            {
                                if (item["RowType"].ToString().Trim() == "Main")
                                {
                                    intindex = gvwProgramData.Rows.Add(item["ProgramDesc"].ToString(), string.Empty, item["PeriodCount"].ToString(), item["Referencecount"].ToString());
                                    if (rdoServiceCount.Checked)
                                    {
                                        gvwProgramData.Rows[intindex].Cells["gvtProgramDesc"].Style.Font = new System.Drawing.Font(DataGridView.DefaultFont, FontStyle.Bold);
                                        gvwProgramData.Rows[intindex].Cells["gvtServicecode"].Style.Font = new System.Drawing.Font(DataGridView.DefaultFont, FontStyle.Bold);
                                        gvwProgramData.Rows[intindex].Cells["gvtReportcount"].Style.Font = new System.Drawing.Font(DataGridView.DefaultFont, FontStyle.Bold);
                                        gvwProgramData.Rows[intindex].Cells["gvtRefcount"].Style.Font = new System.Drawing.Font(DataGridView.DefaultFont, FontStyle.Bold);

                                    }
                                }
                                else
                                {
                                    string PeriodCount = string.Empty,RefCount=string.Empty;
                                    if (!string.IsNullOrEmpty(item["SerP_Cost"].ToString().Trim()))
                                    {
                                        if (decimal.Parse(item["SerP_Cost"].ToString().Trim()) > 0)
                                            PeriodCount = item["SerP_Cost"].ToString().Trim();
                                        else
                                            PeriodCount = item["PeriodCount"].ToString().Trim();
                                    }
                                    else
                                        PeriodCount = item["PeriodCount"].ToString().Trim();

                                    if (!string.IsNullOrEmpty(item["SerREF_Cost"].ToString().Trim()))
                                    {
                                        if (decimal.Parse(item["SerREF_Cost"].ToString().Trim()) > 0)
                                            RefCount = item["SerREF_Cost"].ToString().Trim();
                                        else
                                            RefCount = item["Referencecount"].ToString().Trim();
                                    }

                                    //intindex = gvwProgramData.Rows.Add(string.Empty, item["ServiceDesc"].ToString(), item["PeriodCount"].ToString(), item["Referencecount"].ToString());
                                    intindex = gvwProgramData.Rows.Add(string.Empty, item["ServiceDesc"].ToString(), PeriodCount, RefCount);

                                }
                            }
                        }
                        else
                        {
                            AlertBox.Show("Records not found", MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AlertBox.Show(ex.Message, MessageBoxIcon.Error);
            }
        }

        private bool ValidateForm()
        {
            bool isValid = true;


            return (isValid);
        }

        private bool ValidReport()
        {
            bool Can_Generate = true;

            if (!Ref_From_Date.Checked)
            {
                _errorProvider.SetError(Ref_From_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Reference Period 'From' Date".Replace(Consts.Common.Colon, string.Empty)));
                Can_Generate = false;
            }
            else
                _errorProvider.SetError(Ref_From_Date, null);

            if (!Ref_To_Date.Checked)
            {
                _errorProvider.SetError(Ref_To_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Reference Period 'To' Date".Replace(Consts.Common.Colon, string.Empty)));
                Can_Generate = false;
            }
            else
            {
                if (Ref_From_Date.Checked)
                    _errorProvider.SetError(Ref_To_Date, null);
            }

            if (!Rep_From_Date.Checked)
            {
                _errorProvider.SetError(Rep_From_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Report Period 'From' Date".Replace(Consts.Common.Colon, string.Empty)));
                Can_Generate = false;
            }
            else
                _errorProvider.SetError(Rep_From_Date, null);

            if (!Rep_To_Date.Checked)
            {
                _errorProvider.SetError(Rep_To_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Report Period 'To' Date".Replace(Consts.Common.Colon, string.Empty)));
                Can_Generate = false;
            }
            else
            {
                if (Rep_From_Date.Checked)
                    _errorProvider.SetError(Rep_To_Date, null);
            }

            if (Ref_From_Date.Checked.Equals(true) && Ref_To_Date.Checked.Equals(true))
            {
                if (string.IsNullOrWhiteSpace(Ref_From_Date.Text))
                {
                    _errorProvider.SetError(Ref_From_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Reference Period 'From' Date".Replace(Consts.Common.Colon, string.Empty)));
                    Can_Generate = false;
                }
                else
                {
                    _errorProvider.SetError(Ref_From_Date, null);
                }
                if (string.IsNullOrWhiteSpace(Ref_To_Date.Text))
                {
                    _errorProvider.SetError(Ref_To_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Reference Period 'To' Date".Replace(Consts.Common.Colon, string.Empty)));
                    Can_Generate = false;
                }
                else
                {
                    _errorProvider.SetError(Ref_To_Date, null);
                }
            }

            if (Rep_From_Date.Checked.Equals(true) && Rep_To_Date.Checked.Equals(true))
            {
                if (string.IsNullOrWhiteSpace(Rep_From_Date.Text))
                {
                    _errorProvider.SetError(Rep_From_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Report Period 'From' Date".Replace(Consts.Common.Colon, string.Empty)));
                    Can_Generate = false;
                }
                else
                {
                    _errorProvider.SetError(Rep_From_Date, null);
                }
                if (string.IsNullOrWhiteSpace(Rep_To_Date.Text))
                {
                    _errorProvider.SetError(Rep_To_Date, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Report Period 'To' Date".Replace(Consts.Common.Colon, string.Empty)));
                    Can_Generate = false;
                }
                else
                {
                    _errorProvider.SetError(Rep_To_Date, null);
                }
            }

            if (Ref_To_Date.Checked && Ref_From_Date.Checked)
            {
                if (!string.IsNullOrEmpty(Ref_From_Date.Text) && (!string.IsNullOrEmpty(Ref_To_Date.Text)))
                {
                    if (Convert.ToDateTime(Ref_From_Date.Text) > Convert.ToDateTime(Ref_To_Date.Text))
                    {
                        _errorProvider.SetError(Ref_From_Date, string.Format("Reference Period 'From Date' should be prior or equal to 'To Date'".Replace(Consts.Common.Colon, string.Empty)));
                        Can_Generate = false;
                    }
                    else
                    {
                        if (Ref_To_Date.Checked && Ref_From_Date.Checked)
                            _errorProvider.SetError(Ref_From_Date, null);
                    }
                }
            }

            if (Rep_To_Date.Checked && Rep_From_Date.Checked)
            {
                if (!string.IsNullOrEmpty(Rep_From_Date.Text) && (!string.IsNullOrEmpty(Rep_To_Date.Text)))
                {
                    if (Convert.ToDateTime(Rep_From_Date.Text) > Convert.ToDateTime(Rep_To_Date.Text))
                    {
                        _errorProvider.SetError(Rep_From_Date, string.Format("Report Period 'From Date' should be prior or equal to 'To Date'".Replace(Consts.Common.Colon, string.Empty)));
                        Can_Generate = false;
                    }
                    else
                    {
                        if (Rep_To_Date.Checked && Rep_From_Date.Checked)
                            _errorProvider.SetError(Rep_From_Date, null);
                    }
                }
            }

            if (Ref_To_Date.Checked && Ref_From_Date.Checked && Rep_To_Date.Checked && Rep_From_Date.Checked)
            {
                if (!string.IsNullOrEmpty(Ref_From_Date.Text) && !string.IsNullOrEmpty(Ref_To_Date.Text) && !string.IsNullOrEmpty(Rep_From_Date.Text) && !string.IsNullOrEmpty(Rep_To_Date.Text))
                {
                    if (Convert.ToDateTime(Ref_From_Date.Text) > Convert.ToDateTime(Rep_From_Date.Text) || Convert.ToDateTime(Ref_To_Date.Text) < Convert.ToDateTime(Rep_To_Date.Text))
                    {
                        _errorProvider.SetError(Rep_To_Date, string.Format("'Report Period' Should be in 'Reference Period' Date Range".Replace(Consts.Common.Colon, string.Empty)));
                        Can_Generate = false;
                    }
                    else
                        _errorProvider.SetError(Rep_To_Date, null);
                }
            }

            return Can_Generate;
        }


        PdfContentByte cb;
        string strFolderPath = string.Empty;
        string Random_Filename = null;
        string PdfName = "Pdf File";
        BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
        int Y_Pos;
        BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
        private void On_SaveFormClosed(object sender, FormClosedEventArgs e)
        {
            PdfListForm form = sender as PdfListForm;
            if (form.DialogResult == DialogResult.OK)
            {
                StringBuilder strMstApplUpdate = new StringBuilder();
                string PdfName = "Pdf File";
                PdfName = form.GetFileName();

                // hierarchyEntity = _model.CaseMstData.GetCaseWorker(BaseForm.BaseHierarchyCwFormat.ToString(), BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);

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

                string strPdfHeader = PdfName;
                if (!string.IsNullOrEmpty(Random_Filename))
                {
                    PdfName = Random_Filename;
                    strPdfHeader = Random_Filename;
                }
                else
                    PdfName += ".pdf";



                List<Utilities.ListItem> _listPdffiles = new List<Utilities.ListItem>();
                DataSet ds = dsresult; // Captain.DatabaseLayer.EMSBDCDB.GetEMSB1009(Agency, Dept, Prog, Year, Fund, Site, Worker, Fromdate, Todate, Selection, strreportupload);


                FileStream fs = new FileStream(PdfName, FileMode.Create);
                Document document = new Document(PageSize.A4, 30, 30, 30, 50);




                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                document.Open();



                //BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
                //iTextSharp.text.Font helvetica = new iTextSharp.text.Font(bf_times, 12, 1);
                //BaseFont bf_helv = helvetica.GetCalculatedBaseFont(false);
                //iTextSharp.text.Font TimesUnderline = new iTextSharp.text.Font(1, 9, 4);
                //BaseFont bf_TimesUnderline = TimesUnderline.GetCalculatedBaseFont(true);
                //BaseFont bf_Times = BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1250, false);

                //iTextSharp.text.Font TableFontBold = new iTextSharp.text.Font(bf_times, 14, 1);
                //iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 10);
                //iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 8);
                //iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 9, 3);
                //iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(1, 9, 1);
                //iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 8, 2);
                //iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 9, 4);
                //cb = writer.DirectContent;

                BaseFont bf_Calibri = BaseFont.CreateFont("c:/windows/fonts/calibri.ttf", BaseFont.WINANSI, BaseFont.EMBEDDED);
                iTextSharp.text.Font Calibri = new iTextSharp.text.Font(bf_Calibri, 10);
                iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_Calibri, 8);
                iTextSharp.text.Font TableFont1 = new iTextSharp.text.Font(bf_Calibri, 8, iTextSharp.text.Font.BOLD);
                iTextSharp.text.Font SubHeadFont = new iTextSharp.text.Font(bf_Calibri, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_Calibri, 9, 3);
                iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_Calibri, 8, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
                iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_Calibri, 9, 2);
                iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_Calibri, 9, 4);
                iTextSharp.text.Font MainHeaderLine = new iTextSharp.text.Font(bf_Calibri, 12, 1, BaseColor.BLACK);


                try
                {
                    int PageNo = 0;

                    PrintHeaderPage(document,writer);

                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        document.NewPage();

                        PdfPTable Pdf_SumTable = new PdfPTable(4);
                        Pdf_SumTable.TotalWidth = 550f;
                        Pdf_SumTable.WidthPercentage = 100;
                        Pdf_SumTable.LockedWidth = true;
                        float[] widthssum = new float[] { 150f, 150f, 120f, 120f };
                        Pdf_SumTable.SetWidths(widthssum);
                        Pdf_SumTable.HorizontalAlignment = Element.ALIGN_CENTER;

                        PdfPCell row1 = new PdfPCell(new Phrase("Summary Report", MainHeaderLine));
                        row1.HorizontalAlignment = Element.ALIGN_CENTER;
                        //row1.FixedHeight = 15f;
                        row1.Colspan = 4;
                        row1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        row1.PaddingBottom = 5f;
                        Pdf_SumTable.AddCell(row1);

                        row1 = new PdfPCell(new Phrase("", MainHeaderLine));
                        row1.HorizontalAlignment = Element.ALIGN_CENTER;
                        row1.Colspan = 4;
                        row1.MinimumHeight = 10;
                        row1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        Pdf_SumTable.AddCell(row1);


                        if (rdoServiceCount.Checked)
                        {
                            row1 = new PdfPCell(new Phrase("Program", TblFontBold));
                            row1.HorizontalAlignment = Element.ALIGN_LEFT;
                            row1.FixedHeight = 15f;
                            row1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                            row1.BorderColor = BaseColor.WHITE;
                            //row1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            Pdf_SumTable.AddCell(row1);

                            row1 = new PdfPCell(new Phrase("Service", TblFontBold));
                            row1.HorizontalAlignment = Element.ALIGN_LEFT;
                            row1.FixedHeight = 15f;
                            row1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                            row1.BorderColor = BaseColor.WHITE;
                            //row1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            Pdf_SumTable.AddCell(row1);
                        }
                        else
                        {
                            row1 = new PdfPCell(new Phrase("Program", TblFontBold));
                            row1.HorizontalAlignment = Element.ALIGN_LEFT;
                            row1.Colspan = 2;
                            row1.FixedHeight = 15f;
                            row1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                            row1.BorderColor = BaseColor.WHITE;
                            //row1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            Pdf_SumTable.AddCell(row1);
                        }

                        //gvtReportcount.HeaderText = LookupDataAccess.Getdate(Rep_From_Date.Value.ToString()) + " - " + LookupDataAccess.Getdate(Rep_To_Date.Value.ToString());
                        //gvtRefcount.HeaderText = LookupDataAccess.Getdate(Ref_From_Date.Value.ToString()) + " - " + LookupDataAccess.Getdate(Ref_To_Date.Value.ToString());

                        row1 = new PdfPCell(new Phrase(LookupDataAccess.Getdate(Rep_From_Date.Value.ToString()) + " - " + LookupDataAccess.Getdate(Rep_To_Date.Value.ToString()), TblFontBold));
                        row1.HorizontalAlignment = Element.ALIGN_CENTER;
                        row1.FixedHeight = 15f;
                        row1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                        row1.BorderColor = BaseColor.WHITE;
                        //row1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        Pdf_SumTable.AddCell(row1);

                        row1 = new PdfPCell(new Phrase(LookupDataAccess.Getdate(Ref_From_Date.Value.ToString()) + " - " + LookupDataAccess.Getdate(Ref_To_Date.Value.ToString()), TblFontBold));
                        row1.HorizontalAlignment = Element.ALIGN_CENTER;
                        row1.FixedHeight = 15f;
                        row1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                        row1.BorderColor = BaseColor.WHITE;
                        //row1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        Pdf_SumTable.AddCell(row1);



                        foreach (DataGridViewRow item in gvwProgramData.Rows)
                        {

                            if (rdoServiceCount.Checked)
                            {
                                row1 = new PdfPCell(new Phrase(item.Cells["gvtProgramDesc"].Value.ToString(), TableFont));
                                row1.HorizontalAlignment = Element.ALIGN_LEFT;
                                row1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                row1.BorderColor = BaseColor.WHITE;
                                //row1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                Pdf_SumTable.AddCell(row1);

                                row1 = new PdfPCell(new Phrase(item.Cells["gvtServicecode"].Value.ToString(), TableFont));
                                row1.HorizontalAlignment = Element.ALIGN_LEFT;
                                row1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                                row1.BorderColor = BaseColor.WHITE;
                                //row1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                Pdf_SumTable.AddCell(row1);
                            }

                            else
                            {
                                row1 = new PdfPCell(new Phrase(item.Cells["gvtProgramDesc"].Value.ToString(), TableFont));
                                row1.HorizontalAlignment = Element.ALIGN_LEFT;
                                row1.Colspan = 2;
                                row1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                row1.BorderColor = BaseColor.WHITE;
                                //row1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                Pdf_SumTable.AddCell(row1);
                            }
                                row1 = new PdfPCell(new Phrase(item.Cells["gvtReportcount"].Value.ToString(), TableFont));
                                row1.HorizontalAlignment = Element.ALIGN_CENTER;
                                row1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                row1.BorderColor = BaseColor.WHITE;
                                //row1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                Pdf_SumTable.AddCell(row1);

                                row1 = new PdfPCell(new Phrase(item.Cells["gvtRefcount"].Value.ToString(), TableFont));
                                row1.HorizontalAlignment = Element.ALIGN_CENTER;
                                row1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                row1.BorderColor = BaseColor.WHITE;
                                //row1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                Pdf_SumTable.AddCell(row1);



                        }
                        document.Add(Pdf_SumTable);

                        if (chkPrintAuditreport.Checked)
                        {
                            document.SetPageSize(iTextSharp.text.PageSize.LETTER.Rotate());
                            document.NewPage();


                            PdfPTable Pdf_Table = new PdfPTable(12);
                            Pdf_Table.TotalWidth = 750f;
                            Pdf_Table.WidthPercentage = 100;
                            Pdf_Table.LockedWidth = true;
                            float[] widths = new float[] { 150f, 120f, 55f, 50f, 50f, 50f, 50f, 50f, 100f, 45f, 70f,50f };
                            Pdf_Table.SetWidths(widths);
                            Pdf_Table.HorizontalAlignment = Element.ALIGN_CENTER;


                            if (chkbUndupTable.Checked && rdoProgcount.Checked)
                            {

                                PdfPCell Header3 = new PdfPCell(new Phrase("Program", TblFontBold));
                                Header3.HorizontalAlignment = Element.ALIGN_LEFT;
                                Header3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                                Header3.BorderColor = BaseColor.WHITE;
                                //Header3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                Header3.Colspan = 2;
                                Pdf_Table.AddCell(Header3);


                            }
                            else
                            {
                                PdfPCell Header3 = new PdfPCell(new Phrase("Program", TblFontBold));
                                Header3.HorizontalAlignment = Element.ALIGN_LEFT;
                                Header3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                                Header3.BorderColor = BaseColor.WHITE;
                                //Header3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                Pdf_Table.AddCell(Header3);

                                PdfPCell Header2 = new PdfPCell(new Phrase("Service", TblFontBold));
                                Header2.HorizontalAlignment = Element.ALIGN_LEFT;
                                Header2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                                Header2.BorderColor = BaseColor.WHITE;
                                //Header2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                Pdf_Table.AddCell(Header2);


                            }



                            if (Rb_MS_Date.Checked)
                            {

                                PdfPCell Header41 = new PdfPCell(new Phrase("Add Date", TblFontBold));
                                Header41.HorizontalAlignment = Element.ALIGN_LEFT;
                                Header41.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                                Header41.BorderColor = BaseColor.WHITE;
                                //Header41.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                Pdf_Table.AddCell(Header41);
                            }
                            else
                            {

                                PdfPCell Header42 = new PdfPCell(new Phrase("Service Date", TblFontBold));
                                Header42.HorizontalAlignment = Element.ALIGN_LEFT;
                                Header42.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                                Header42.BorderColor = BaseColor.WHITE;
                                //Header42.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                Pdf_Table.AddCell(Header42);
                            }

                            PdfPCell Header31 = new PdfPCell(new Phrase("Reference Period Count", TblFontBold));
                            Header31.HorizontalAlignment = Element.ALIGN_LEFT;
                            Header31.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                            Header31.BorderColor = BaseColor.WHITE;
                            //Header31.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            Pdf_Table.AddCell(Header31);



                            if (Rb_MS_Date.Checked)
                            {
                                Header31 = new PdfPCell(new Phrase("Service Date", TblFontBold));
                                Header31.HorizontalAlignment = Element.ALIGN_LEFT;
                                Header31.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                                Header31.BorderColor = BaseColor.WHITE;
                                //Header31.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                Pdf_Table.AddCell(Header31);

                            }
                            else
                            {
                                Header31 = new PdfPCell(new Phrase("Add Date", TblFontBold));
                                Header31.HorizontalAlignment = Element.ALIGN_LEFT;
                                Header31.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                                Header31.BorderColor = BaseColor.WHITE;
                                //Header31.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                Pdf_Table.AddCell(Header31);
                            }
                            Header31 = new PdfPCell(new Phrase("Report Period Count", TblFontBold));
                            Header31.HorizontalAlignment = Element.ALIGN_LEFT;
                            Header31.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                            Header31.BorderColor = BaseColor.WHITE;
                            //Header31.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            Pdf_Table.AddCell(Header31);




                            PdfPCell Header_1 = new PdfPCell(new Phrase("Intake Hierarchy", TblFontBold));
                            Header_1.HorizontalAlignment = Element.ALIGN_LEFT;
                            Header_1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                            Header_1.BorderColor = BaseColor.WHITE;
                            //Header_1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            Pdf_Table.AddCell(Header_1);

                            PdfPCell Header = new PdfPCell(new Phrase("App No", TblFontBold));
                            Header.HorizontalAlignment = Element.ALIGN_LEFT;
                            Header.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                            Header.BorderColor = BaseColor.WHITE;
                            //Header.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            Pdf_Table.AddCell(Header);

                            PdfPCell HeaderTop1 = new PdfPCell(new Phrase("Name", TblFontBold));
                            HeaderTop1.HorizontalAlignment = Element.ALIGN_LEFT;
                            HeaderTop1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                            HeaderTop1.BorderColor = BaseColor.WHITE;
                            //HeaderTop1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            Pdf_Table.AddCell(HeaderTop1);

                            PdfPCell Header4 = new PdfPCell(new Phrase("Client ID", TblFontBold));
                            Header4.HorizontalAlignment = Element.ALIGN_LEFT;
                            Header4.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                            Header4.BorderColor = BaseColor.WHITE;
                            //Header4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            Pdf_Table.AddCell(Header4);

                            Header4 = new PdfPCell(new Phrase("Add Operator", TblFontBold));
                            Header4.HorizontalAlignment = Element.ALIGN_LEFT;
                            Header4.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                            Header4.BorderColor = BaseColor.WHITE;
                            //Header4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            Pdf_Table.AddCell(Header4);

                            Header4 = new PdfPCell(new Phrase("Amount", TblFontBold));
                            Header4.HorizontalAlignment = Element.ALIGN_LEFT;
                            Header4.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                            Header4.BorderColor = BaseColor.WHITE;
                            //Header4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            Pdf_Table.AddCell(Header4);

                            bool boolfileexist = false;
                            foreach (DataRow SelRow in ds.Tables[0].Rows)
                            {

                                if (chkbUndupTable.Checked && rdoProgcount.Checked)
                                {

                                    PdfPCell A91 = new PdfPCell(new Phrase(SelRow["Act_ProgramDesc"].ToString().Trim(), TableFont));
                                    A91.HorizontalAlignment = Element.ALIGN_LEFT;
                                    A91.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                    A91.BorderColor = BaseColor.WHITE;
                                    //A91.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    A91.Colspan = 2;
                                    Pdf_Table.AddCell(A91);

                                }
                                else
                                {
                                    PdfPCell A92 = new PdfPCell(new Phrase(SelRow["Act_ProgramDesc"].ToString().Trim(), TableFont));
                                    A92.HorizontalAlignment = Element.ALIGN_LEFT;
                                    A92.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                    A92.BorderColor = BaseColor.WHITE;
                                    //A92.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    Pdf_Table.AddCell(A92);

                                    A92 = new PdfPCell(new Phrase(SelRow["ACT_DESC"].ToString().Trim(), TableFont));
                                    A92.HorizontalAlignment = Element.ALIGN_LEFT;
                                    A92.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                    A92.BorderColor = BaseColor.WHITE;
                                    //A92.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    Pdf_Table.AddCell(A92);
                                }

                                if (Rb_MS_Date.Checked)
                                {

                                    PdfPCell A2 = new PdfPCell(new Phrase(LookupDataAccess.Getdate(SelRow["Act_DateAdd"].ToString().Trim()), TableFont));
                                    A2.HorizontalAlignment = Element.ALIGN_LEFT;
                                    A2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                                    A2.BorderColor = BaseColor.WHITE;
                                    //A2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    Pdf_Table.AddCell(A2);

                                }
                                else
                                {
                                    PdfPCell A2 = new PdfPCell(new Phrase(LookupDataAccess.Getdate(SelRow["Act_Date"].ToString().Trim()), TableFont));
                                    A2.HorizontalAlignment = Element.ALIGN_LEFT;
                                    A2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                                    A2.BorderColor = BaseColor.WHITE;
                                    //A2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    Pdf_Table.AddCell(A2);

                                }
                                PdfPCell A21 = new PdfPCell(new Phrase(SelRow["Act_RefCount"].ToString().Trim(), TableFont));
                                A21.HorizontalAlignment = Element.ALIGN_LEFT;
                                A21.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                                A21.BorderColor = BaseColor.WHITE;
                                //A21.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                Pdf_Table.AddCell(A21);

                                if (Rb_MS_Date.Checked)
                                {
                                    PdfPCell A2 = new PdfPCell(new Phrase(LookupDataAccess.Getdate(SelRow["Act_Date"].ToString().Trim()), TableFont));
                                    A2.HorizontalAlignment = Element.ALIGN_LEFT;
                                    A2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                                    A2.BorderColor = BaseColor.WHITE;
                                    //A2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    Pdf_Table.AddCell(A2);

                                }
                                else
                                {
                                    PdfPCell A2 = new PdfPCell(new Phrase(LookupDataAccess.Getdate(SelRow["Act_DateAdd"].ToString().Trim()), TableFont));
                                    A2.HorizontalAlignment = Element.ALIGN_LEFT;
                                    A2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                                    A2.BorderColor = BaseColor.WHITE;
                                    //A2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    Pdf_Table.AddCell(A2);

                                }
                                A21 = new PdfPCell(new Phrase(SelRow["Act_RepCount"].ToString().Trim(), TableFont));
                                A21.HorizontalAlignment = Element.ALIGN_LEFT;
                                A21.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                                A21.BorderColor = BaseColor.WHITE;
                                //A21.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                Pdf_Table.AddCell(A21);


                                PdfPCell A1 = new PdfPCell(new Phrase(SelRow["Act_Agy"].ToString().Trim() + SelRow["Act_Dept"].ToString().Trim() + SelRow["Act_Prog"].ToString().Trim() + SelRow["Act_Year"].ToString().Trim(), TableFont));
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                                A1.BorderColor = BaseColor.WHITE;
                                //A1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                Pdf_Table.AddCell(A1);

                                PdfPCell A22 = new PdfPCell(new Phrase(SelRow["Act_App"].ToString().Trim(), TableFont));
                                A22.HorizontalAlignment = Element.ALIGN_LEFT;
                                A22.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                A22.BorderColor = BaseColor.WHITE;
                                //A22.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                Pdf_Table.AddCell(A22);


                                PdfPCell A4 = new PdfPCell(new Phrase(SelRow["Act_LName"].ToString().Trim() + ", " + SelRow["Act_FName"].ToString().Trim() + " " + SelRow["Act_MName"].ToString().Trim(), TableFont));
                                A4.HorizontalAlignment = Element.ALIGN_LEFT;
                                A4.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                A4.BorderColor = BaseColor.WHITE;
                                // A4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                Pdf_Table.AddCell(A4);

                                PdfPCell A9 = new PdfPCell(new Phrase(SelRow["Act_ClientID"].ToString().Trim(), TableFont));
                                A9.HorizontalAlignment = Element.ALIGN_LEFT;
                                A9.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                A9.BorderColor = BaseColor.WHITE;
                                //A9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                Pdf_Table.AddCell(A9);

                                A9 = new PdfPCell(new Phrase(SelRow["Act_AddOperator"].ToString().Trim(), TableFont));
                                A9.HorizontalAlignment = Element.ALIGN_LEFT;
                                A9.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                A9.BorderColor = BaseColor.WHITE;
                                //A9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                Pdf_Table.AddCell(A9);

                                A9 = new PdfPCell(new Phrase(SelRow["Act_Cost"].ToString().Trim(), TableFont));
                                A9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                A9.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                A9.BorderColor = BaseColor.WHITE;
                                //A9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                Pdf_Table.AddCell(A9);

                            }

                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                document.Add(Pdf_Table);
                            }

                        }

                        if (chkbExcel.Checked) OnSaveExcel_Report(ds.Tables[0], PdfName);
                    }

                }
                catch (Exception ex)
                {
                    document.Add(new Paragraph("Aborted due to Exception............................................... "));
                }
                document.Close();
                fs.Close();
                fs.Dispose();
                AlertBox.Show("Report Generated Successfullly");


            }

        }


        private void PrintHeaderPage(Document document, PdfWriter writer)
        {

            //BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/calibrib.ttf", BaseFont.WINANSI, BaseFont.EMBEDDED);
            //BaseFont bfTimes = BaseFont.CreateFont("c:/windows/fonts/calibri.ttf", BaseFont.WINANSI, BaseFont.EMBEDDED);
            ////BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, false);
            //BaseFont bf_Times = BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1250, false);
            //iTextSharp.text.Font fc = new iTextSharp.text.Font(bfTimes, 10, 2);
            //iTextSharp.text.Font fc1 = new iTextSharp.text.Font(bf_Times, 10, 2, BaseColor.BLUE);
            //iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bfTimes, 10);
            //iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_Times, 13);
            //iTextSharp.text.Font TblFont = new iTextSharp.text.Font(bf_Times, 11);

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
            string PageForm = "P";
            //if (rbLandScape.Checked == true) PageForm = "L"; else PageForm = "P";


            PdfPTable Headertable = new PdfPTable(7);
            Headertable.TotalWidth = 510f;
            Headertable.WidthPercentage = 100;
            Headertable.LockedWidth = true;
            float[] Headerwidths = new float[] { 23f, 13f, 13f, 13f, 13f, 13f, 13f };
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

            PdfPCell cellspace = new PdfPCell(new Phrase(""));
            cellspace.HorizontalAlignment = Element.ALIGN_CENTER;
            cellspace.Colspan = 7;
            cellspace.PaddingBottom = 5;
            cellspace.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Headertable.AddCell(cellspace);

            if (_strImageFolderPath != "")
            {
                iTextSharp.text.Image imgLogo = iTextSharp.text.Image.GetInstance(_strImageFolderPath);
                PdfPCell cellLogo = new PdfPCell(imgLogo);
                cellLogo.HorizontalAlignment = Element.ALIGN_CENTER;
                cellLogo.Colspan = 7;
                cellLogo.Padding = 5;
                cellLogo.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Headertable.AddCell(cellLogo);
            }

            PdfPCell row1 = new PdfPCell(new Phrase(Privileges.Program + " - " + Privileges.PrivilegeName, TblHeaderTitleFont));
            row1.HorizontalAlignment = Element.ALIGN_CENTER;
            row1.Colspan = 7;
            row1.PaddingBottom = 15;
            row1.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Headertable.AddCell(row1);

            //PdfPCell row2 = new PdfPCell(new Phrase("Run By : " + LookupDataAccess.GetMemberName(BaseForm.UserProfile.FirstName.Trim(), BaseForm.UserProfile.MI.Trim(), BaseForm.UserProfile.LastName.Trim(), "3"), TableFont));
            //row2.HorizontalAlignment = Element.ALIGN_LEFT;
            ////row2.Colspan = 2;
            //row2.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //Headertable.AddCell(row2);

            //PdfPCell row21 = new PdfPCell(new Phrase("Date : " + DateTime.Now.ToString(), TableFont));
            //row21.HorizontalAlignment = Element.ALIGN_RIGHT;
            ////row2.Colspan = 2;
            //row21.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //Headertable.AddCell(row21);

            PdfPCell row3 = new PdfPCell(new Phrase("Selected Report Parameters", TblParamsHeaderFont));
            row3.HorizontalAlignment = Element.ALIGN_CENTER;
            row3.VerticalAlignment = PdfPCell.ALIGN_TOP;
            row3.MinimumHeight = 6;
            row3.PaddingBottom = 5;
            row3.Colspan = 7;
            row3.Border = iTextSharp.text.Rectangle.NO_BORDER;
            row3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#b8e9fb"));
            //row3.BackgroundColor = BaseColor.LIGHT_GRAY;
            Headertable.AddCell(row3);

            string Agy = /*Agency: */"All"; string Deprt = /*Dept : */"All"; string Prg = /*Program : */"All"; string Header_year = string.Empty;
            if (Agency != "**") Agy = /*"Agency : " + */Agency;
            if (Dept != "**") Deprt = /*"Dept : " + */Dept;
            if (Prog != "**") Prg = /*"Program : " + */Prog;
            if (CmbYear.Visible == true)
                Header_year = /*"Year : " + */((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString();

            if (CmbYear.Visible == true)
            {
                PdfPCell Hie = new PdfPCell(new Phrase("  " + "Hierarchy", TableFont));
                Hie.HorizontalAlignment = Element.ALIGN_LEFT;
                Hie.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                Hie.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                Hie.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                Hie.PaddingBottom = 5;
                Headertable.AddCell(Hie);

                PdfPCell Hierarchy1 = new PdfPCell(new Phrase("Agency: " + Agy + "," + "Department: " + Dept + ", " + "Program: " + Prg + ", " + "Year: " + Header_year, TableFont));
                Hierarchy1.HorizontalAlignment = Element.ALIGN_LEFT;
                Hierarchy1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                Hierarchy1.Colspan = 6;
                Hierarchy1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                Hierarchy1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                Hierarchy1.PaddingBottom = 5;
                Headertable.AddCell(Hierarchy1);
            }
            else
            {
                PdfPCell Hie = new PdfPCell(new Phrase("  " + "Hierarchy", TableFont));
                Hie.HorizontalAlignment = Element.ALIGN_LEFT;
                Hie.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                Hie.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                Hie.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                Hie.PaddingBottom = 5;
                Headertable.AddCell(Hie);

                PdfPCell Hierarchy1 = new PdfPCell(new Phrase("Agency: " + Agy + ", " + "Department: " + Dept + ", " + "Program: " + Prg, TableFont));
                Hierarchy1.HorizontalAlignment = Element.ALIGN_LEFT;
                Hierarchy1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                Hierarchy1.Colspan = 6;
                Hierarchy1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                Hierarchy1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                Hierarchy1.PaddingBottom = 5;
                Headertable.AddCell(Hierarchy1);
            }


            //PdfPCell Hierarchy = new PdfPCell(new Phrase(Agy + "  " + Dept + "  " + Prg + "  " + Header_year, TableFont));Txt_HieDesc
            //PdfPCell Hierarchy = new PdfPCell(new Phrase(Txt_HieDesc.Text.Trim() + "     " + Header_year, TableFont));
            //Hierarchy.HorizontalAlignment = Element.ALIGN_LEFT;
            //Hierarchy.Colspan = 2;
            //Hierarchy.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //Headertable.AddCell(Hierarchy);


            PdfPCell R1 = new PdfPCell(new Phrase("  " + lblDateSelection.Text.Trim()/* + " : " + (Rb_MS_Date.Checked == true ? Rb_MS_Date.Text : Rb_MS_AddDate.Text)*/, TableFont)); //+ (rdoSiteName.Checked == true ? rdoSiteName.Text : rdoRank.Text)
            R1.HorizontalAlignment = Element.ALIGN_LEFT;
            R1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R1.PaddingBottom = 5;
            //R1.Colspan = 2;
            //R1.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Headertable.AddCell(R1);

            PdfPCell R11 = new PdfPCell(new Phrase((Rb_MS_Date.Checked == true ? Rb_MS_Date.Text : Rb_MS_AddDate.Text), TableFont));
            R11.HorizontalAlignment = Element.ALIGN_LEFT;
            R11.Colspan = 6;
            R11.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R11.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R11.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R11.PaddingBottom = 5;
            Headertable.AddCell(R11);

            PdfPCell R4 = new PdfPCell(new Phrase("  " + "Reference Period"/*      Start Date: " + Ref_From_Date.Text + "    End Date: " + Ref_To_Date.Text*/, TableFont));
            R4.HorizontalAlignment = Element.ALIGN_LEFT;
            R4.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R4.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R4.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R4.PaddingBottom = 5;
            //R4.Colspan = 2;
            //R4.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Headertable.AddCell(R4);

            PdfPCell R41 = new PdfPCell(new Phrase("Start Date: " + Ref_From_Date.Text + "      End Date: " + Ref_To_Date.Text, TableFont));
            R41.HorizontalAlignment = Element.ALIGN_LEFT;
            R41.Colspan = 6;
            R41.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R41.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R41.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R41.PaddingBottom = 5;
            Headertable.AddCell(R41);

            PdfPCell R5 = new PdfPCell(new Phrase("  " + "Report Period"/*     Start Date: " + Rep_From_Date.Text + "    End Date: " + Rep_To_Date.Text*/, TableFont));
            R5.HorizontalAlignment = Element.ALIGN_LEFT;
            R5.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R5.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R5.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R5.PaddingBottom = 5;
            //R5.Colspan = 2;
            //R5.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Headertable.AddCell(R5);

            PdfPCell R51 = new PdfPCell(new Phrase("Start Date: " + Rep_From_Date.Text + "      End Date: " + Rep_To_Date.Text, TableFont));
            R51.HorizontalAlignment = Element.ALIGN_LEFT;
            R51.Colspan = 6;
            R51.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R51.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R51.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R51.PaddingBottom = 5;
            Headertable.AddCell(R51);

            PdfPCell R6 = new PdfPCell(new Phrase("  " + "Report Type" /*+ (rdoProgcount.Checked == true ? rdoProgcount.Text : rdoServiceCount.Text)*/, TableFont));
            R6.HorizontalAlignment = Element.ALIGN_LEFT;
            R6.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R6.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R6.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R6.PaddingBottom = 5;
            //R6.Colspan = 2;
            //R6.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Headertable.AddCell(R6);

            PdfPCell R61 = new PdfPCell(new Phrase((rdoProgcount.Checked == true ? rdoProgcount.Text : rdoServiceCount.Text), TableFont));
            R61.HorizontalAlignment = Element.ALIGN_LEFT;
            R61.Colspan = 6;
            R61.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R61.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R61.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R61.PaddingBottom = 5;
            Headertable.AddCell(R61);

            PdfPCell R7 = new PdfPCell(new Phrase("  " + "Unduplicated Program Count" /*+ (chkbUndupTable.Checked == true ? "Yes" : "No")*/, TableFont));
            R7.HorizontalAlignment = Element.ALIGN_LEFT;
            R7.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R7.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R7.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R7.PaddingBottom = 5;
            //R7.Colspan = 2;
            //R7.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Headertable.AddCell(R7);

            PdfPCell R71 = new PdfPCell(new Phrase((chkbUndupTable.Checked == true ? "Yes" : "No"), TableFont));
            R71.HorizontalAlignment = Element.ALIGN_LEFT;
            R71.Colspan = 6;
            R71.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R71.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R71.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R71.PaddingBottom = 5;
            Headertable.AddCell(R71);

            PdfPCell R8 = new PdfPCell(new Phrase("  " + "Print Audit Report" /*+ (chkPrintAuditreport.Checked == true ? "Yes" : "No")*/, TableFont));
            R8.HorizontalAlignment = Element.ALIGN_LEFT;
            R8.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R8.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R8.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R8.PaddingBottom = 5;
            //R8.Colspan = 2;
            //R8.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Headertable.AddCell(R8);

            PdfPCell R81 = new PdfPCell(new Phrase((chkPrintAuditreport.Checked == true ? "Yes" : "No"), TableFont));
            R81.HorizontalAlignment = Element.ALIGN_LEFT;
            R81.Colspan = 6;
            R81.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R81.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R81.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R81.PaddingBottom = 5;
            Headertable.AddCell(R81);


            //if (ds.Tables[0].Rows.Count == 0)
            //{
            //R7 = new PdfPCell(new Phrase("", TblFontBold));
            //R7.HorizontalAlignment = Element.ALIGN_CENTER;
            //R7.MinimumHeight = 50;
            //R7.Colspan = 2;
            //R7.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //Headertable.AddCell(R7);

            //R7 = new PdfPCell(new Phrase("", TblFontBold));
            //R7.HorizontalAlignment = Element.ALIGN_CENTER;
            //R7.MinimumHeight = 50;
            //R7.Colspan = 2;
            //R7.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //Headertable.AddCell(R7);

            //R7 = new PdfPCell(new Phrase("", TblFontBold));
            //R7.HorizontalAlignment = Element.ALIGN_CENTER;
            //R7.MinimumHeight = 50;
            //R7.Colspan = 2;
            //R7.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //Headertable.AddCell(R7);


            //R7 = new PdfPCell(new Phrase("No Uploaded Invoices found in the selected criteria", TblFontBold));
            //R7.HorizontalAlignment = Element.ALIGN_CENTER;
            //R7.MinimumHeight = 50;
            //R7.Colspan = 2;
            //R7.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //Headertable.AddCell(R7);
            // }

            document.Add(Headertable);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase("Generated By : ", fnttimesRoman_Italic), 33, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.GetMemberName(BaseForm.UserProfile.FirstName.Trim(), BaseForm.UserProfile.MI.Trim(), BaseForm.UserProfile.LastName.Trim(), "3"), fnttimesRoman_Italic), 90, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase("Generated On : ", fnttimesRoman_Italic), 410, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase(DateTime.Now.ToString(), fnttimesRoman_Italic), 468, 40, 0);

        }



        private void GenerateStyles(WorksheetStyleCollection styles)
        {
            // -----------------------------------------------
            //  Default
            // -----------------------------------------------
            WorksheetStyle Default = styles.Add("Default");
            Default.Name = "Normal";
            Default.Font.FontName = "Calibri";
            Default.Font.Size = 11;
            Default.Font.Color = "#000000";
            Default.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s15
            // -----------------------------------------------
            WorksheetStyle s15 = styles.Add("s15");
            s15.Font.FontName = "Calibri";
            s15.Font.Size = 11;
            // -----------------------------------------------
            //  s16
            // -----------------------------------------------
            WorksheetStyle s16 = styles.Add("s16");
            s16.Font.FontName = "Calibri";
            s16.Font.Size = 11;
            s16.Interior.Color = "#FFFFFF";
            s16.Interior.Pattern = StyleInteriorPattern.Solid;
            s16.Alignment.Vertical = StyleVerticalAlignment.Top;
            s16.Alignment.WrapText = true;
            // -----------------------------------------------
            //  s17
            // -----------------------------------------------
            WorksheetStyle s17 = styles.Add("s17");
            s17.Font.FontName = "Calibri";
            s17.Font.Size = 11;
            s17.Interior.Color = "#FFFFFF";
            s17.Interior.Pattern = StyleInteriorPattern.Solid;
            s17.Alignment.Vertical = StyleVerticalAlignment.Top;
            s17.Alignment.WrapText = true;
            s17.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            s17.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s18
            // -----------------------------------------------
            WorksheetStyle s18 = styles.Add("s18");
            s18.Font.FontName = "Calibri";
            s18.Font.Size = 11;
            s18.Interior.Color = "#FFFFFF";
            s18.Interior.Pattern = StyleInteriorPattern.Solid;
            s18.Alignment.Vertical = StyleVerticalAlignment.Top;
            s18.Alignment.WrapText = true;
            s18.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s19
            // -----------------------------------------------
            WorksheetStyle s19 = styles.Add("s19");
            s19.Font.FontName = "Calibri";
            s19.Font.Size = 11;
            s19.Interior.Color = "#FFFFFF";
            s19.Interior.Pattern = StyleInteriorPattern.Solid;
            s19.Alignment.Vertical = StyleVerticalAlignment.Top;
            s19.Alignment.WrapText = true;
            s19.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            s19.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s20
            // -----------------------------------------------
            WorksheetStyle s20 = styles.Add("s20");
            s20.Font.FontName = "Calibri";
            s20.Font.Size = 11;
            s20.Interior.Color = "#FFFFFF";
            s20.Interior.Pattern = StyleInteriorPattern.Solid;
            s20.Alignment.Vertical = StyleVerticalAlignment.Top;
            s20.Alignment.WrapText = true;
            s20.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s21
            // -----------------------------------------------
            WorksheetStyle s21 = styles.Add("s21");
            s21.Font.FontName = "Arial";
            s21.Font.Color = "#9400D3";
            s21.Interior.Color = "#FFFFFF";
            s21.Interior.Pattern = StyleInteriorPattern.Solid;
            s21.Alignment.Vertical = StyleVerticalAlignment.Top;
            s21.Alignment.WrapText = true;
            s21.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            // -----------------------------------------------
            //  s22
            // -----------------------------------------------
            WorksheetStyle s22 = styles.Add("s22");
            s22.Font.FontName = "Calibri";
            s22.Font.Size = 11;
            s22.Interior.Color = "#FFFFFF";
            s22.Interior.Pattern = StyleInteriorPattern.Solid;
            s22.Alignment.Vertical = StyleVerticalAlignment.Top;
            s22.Alignment.WrapText = true;
            s22.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s23
            // -----------------------------------------------
            WorksheetStyle s23 = styles.Add("s23");
            s23.Font.FontName = "Arial";
            s23.Font.Color = "#000000";
            s23.Interior.Color = "#FFFFFF";
            s23.Interior.Pattern = StyleInteriorPattern.Solid;
            s23.Alignment.Vertical = StyleVerticalAlignment.Top;
            s23.Alignment.WrapText = true;
            s23.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;


            WorksheetStyle s23center = styles.Add("s23center");
            s23center.Font.FontName = "Arial";
            s23center.Font.Color = "#000000";
            s23center.Interior.Color = "#FFFFFF";
            s23center.Interior.Pattern = StyleInteriorPattern.Solid;
            s23center.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s23center.Alignment.Vertical = StyleVerticalAlignment.Top;
            s23center.Alignment.WrapText = true;
            s23center.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            // -----------------------------------------------
            //  s24
            // -----------------------------------------------
            WorksheetStyle s24 = styles.Add("s24");
            s24.Font.FontName = "Calibri";
            s24.Font.Size = 11;
            s24.Interior.Color = "#FFFFFF";
            s24.Interior.Pattern = StyleInteriorPattern.Solid;
            s24.Alignment.Vertical = StyleVerticalAlignment.Top;
            s24.Alignment.WrapText = true;
            s24.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            s24.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s25
            // -----------------------------------------------
            WorksheetStyle s25 = styles.Add("s25");
            s25.Font.FontName = "Arial";
            s25.Font.Color = "#9400D3";
            s25.Interior.Color = "#FFFFFF";
            s25.Interior.Pattern = StyleInteriorPattern.Solid;
            s25.Alignment.Vertical = StyleVerticalAlignment.Top;
            s25.Alignment.WrapText = true;
            s25.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s25.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s26
            // -----------------------------------------------
            WorksheetStyle s26 = styles.Add("s26");
            s26.Font.FontName = "Calibri";
            s26.Font.Size = 11;
            s26.Interior.Color = "#FFFFFF";
            s26.Interior.Pattern = StyleInteriorPattern.Solid;
            s26.Alignment.Vertical = StyleVerticalAlignment.Top;
            s26.Alignment.WrapText = true;
            s26.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s27
            // -----------------------------------------------
            WorksheetStyle s27 = styles.Add("s27");
            s27.Font.FontName = "Calibri";
            s27.Font.Size = 11;
            s27.Interior.Color = "#FFFFFF";
            s27.Interior.Pattern = StyleInteriorPattern.Solid;
            s27.Alignment.Vertical = StyleVerticalAlignment.Top;
            s27.Alignment.WrapText = true;
            s27.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            s27.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s28
            // -----------------------------------------------
            WorksheetStyle s28 = styles.Add("s28");
            s28.Font.Bold = true;
            s28.Font.FontName = "Arial";
            s28.Font.Color = "#000000";
            s28.Interior.Color = "#B0C4DE";
            s28.Interior.Pattern = StyleInteriorPattern.Solid;
            s28.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s28.Alignment.Vertical = StyleVerticalAlignment.Top;
            s28.Alignment.WrapText = true;
            s28.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s28.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            s28.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            s28.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            s28.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s30
            // -----------------------------------------------
            WorksheetStyle s30 = styles.Add("s30");
            s30.Font.FontName = "Arial";
            s30.Font.Color = "#000000";
            s30.Interior.Color = "#FFFFFF";
            s30.Interior.Pattern = StyleInteriorPattern.Solid;
            s30.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            s30.Alignment.Vertical = StyleVerticalAlignment.Top;
            s30.Alignment.WrapText = true;
            s30.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            // -----------------------------------------------
            //  s32
            // -----------------------------------------------
            WorksheetStyle s32 = styles.Add("s32");
            s32.Font.FontName = "Arial";
            s32.Font.Color = "#000000";
            s32.Alignment.Vertical = StyleVerticalAlignment.Top;
            s32.Alignment.WrapText = true;
            s32.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            // -----------------------------------------------
            //  s33
            // -----------------------------------------------
            WorksheetStyle s33 = styles.Add("s33");
            s33.Font.FontName = "Arial";
            s33.Font.Color = "#000000";
            s33.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            s33.Alignment.Vertical = StyleVerticalAlignment.Top;
            s33.Alignment.WrapText = true;
            s33.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            // -----------------------------------------------
            //  s34
            // -----------------------------------------------
            WorksheetStyle s34 = styles.Add("s34");
            s34.Font.FontName = "Arial";
            s34.Font.Color = "#000000";
            s34.Interior.Color = "#FFFF00";
            s34.Interior.Pattern = StyleInteriorPattern.Solid;
            s34.Alignment.Vertical = StyleVerticalAlignment.Top;
            s34.Alignment.WrapText = true;
            s34.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            // -----------------------------------------------
            //  s35
            // -----------------------------------------------
            WorksheetStyle s35 = styles.Add("s35");
            s35.Font.FontName = "Arial";
            s35.Font.Color = "#FF0000";
            s35.Interior.Color = "#FFFF00";
            s35.Interior.Pattern = StyleInteriorPattern.Solid;
            s35.Alignment.Vertical = StyleVerticalAlignment.Top;
            s35.Alignment.WrapText = true;
            s35.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            // -----------------------------------------------
            //  s36
            // -----------------------------------------------
            WorksheetStyle s36 = styles.Add("s36");
            s36.Font.FontName = "Arial";
            s36.Font.Color = "#000000";
            s36.Interior.Color = "#FFC000";
            s36.Interior.Pattern = StyleInteriorPattern.Solid;
            s36.Alignment.Vertical = StyleVerticalAlignment.Top;
            s36.Alignment.WrapText = true;
            s36.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            // -----------------------------------------------
            //  s37
            // -----------------------------------------------
            WorksheetStyle s37 = styles.Add("s37");
            s37.Font.FontName = "Arial";
            s37.Font.Color = "#FF0000";
            s37.Interior.Color = "#FFC000";
            s37.Interior.Pattern = StyleInteriorPattern.Solid;
            s37.Alignment.Vertical = StyleVerticalAlignment.Top;
            s37.Alignment.WrapText = true;
            s37.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;


            // -----------------------------------------------
            //  s111
            // -----------------------------------------------
            WorksheetStyle s111 = styles.Add("s111");
            s111.Font.Bold = true;
            s111.Font.FontName = "Arial";
            s111.Font.Color = "#000000";
            s111.Interior.Color = "#FF5050";
            s111.Interior.Pattern = StyleInteriorPattern.Solid;
            s111.Alignment.Vertical = StyleVerticalAlignment.Top;
            s111.Alignment.WrapText = true;
            s111.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s111.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 2);
            //s111.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 2);


            // -----------------------------------------------
            //  s113
            // -----------------------------------------------
            WorksheetStyle s113 = styles.Add("s113");
            s113.Font.Bold = true;
            s113.Font.FontName = "Arial";
            s113.Font.Color = "#000000";
            s113.Interior.Color = "#FF5050";
            s113.Interior.Pattern = StyleInteriorPattern.Solid;
            s113.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s113.Alignment.Vertical = StyleVerticalAlignment.Top;
            s113.Alignment.WrapText = true;
            s113.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s113.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 2);


            // -----------------------------------------------
            //  s111
            // -----------------------------------------------
            WorksheetStyle s111blue = styles.Add("s111blue");
            s111blue.Font.Bold = true;
            s111blue.Font.FontName = "Arial";
            s111blue.Font.Color = "#000000";
            s111blue.Interior.Color = "#00B0F0";
            s111blue.Interior.Pattern = StyleInteriorPattern.Solid;
            s111blue.Alignment.Vertical = StyleVerticalAlignment.Top;
            s111blue.Alignment.WrapText = true;
            s111blue.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s111blue.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 2);
            //s111.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 2);


            // -----------------------------------------------
            //  s113
            // -----------------------------------------------
            WorksheetStyle s113blue = styles.Add("s113blue");
            s113blue.Font.Bold = true;
            s113blue.Font.FontName = "Arial";
            s113blue.Font.Color = "#000000";
            s113blue.Interior.Color = "#00B0F0";
            s113blue.Interior.Pattern = StyleInteriorPattern.Solid;
            s113blue.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s113blue.Alignment.Vertical = StyleVerticalAlignment.Top;
            s113blue.Alignment.WrapText = true;
            s113blue.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s113blue.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 2);



            // -----------------------------------------------
            //  s115
            // -----------------------------------------------
            WorksheetStyle s115 = styles.Add("s115");
            s115.Font.Bold = true;
            s115.Font.FontName = "Arial";
            s115.Font.Color = "#000000";
            s115.Interior.Color = "#00B0F0";
            s115.Interior.Pattern = StyleInteriorPattern.Solid;
            s115.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s115.Alignment.Vertical = StyleVerticalAlignment.Top;
            s115.Alignment.WrapText = true;
            s115.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s115.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 2);

            // -----------------------------------------------
            //  s117
            // -----------------------------------------------
            WorksheetStyle s117 = styles.Add("s117");
            s117.Font.FontName = "Calibri";
            s117.Font.Size = 11;
            s117.Interior.Color = "#FF5050";
            s117.Interior.Pattern = StyleInteriorPattern.Solid;
            s117.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 2);
            s117.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 2);
        }

        private void GenerateWorksheetParameters(Workbook book)
        {
            Worksheet sheet = book.Worksheets.Add("Parameters");
            sheet.Table.DefaultRowHeight = 14.4F;
            sheet.Table.ExpandedColumnCount = 6;
            sheet.Table.ExpandedRowCount = 25;
            sheet.Table.FullColumns = 1;
            sheet.Table.FullRows = 1;
            sheet.Table.StyleID = "s15";
            WorksheetColumn column0 = sheet.Table.Columns.Add();
            column0.Width = 15;
            column0.StyleID = "s15";
            WorksheetColumn column1 = sheet.Table.Columns.Add();
            column1.Width = 5;
            column1.StyleID = "s15";
            WorksheetColumn column2 = sheet.Table.Columns.Add();
            column2.Width = 162;
            column2.StyleID = "s15";
            WorksheetColumn column3 = sheet.Table.Columns.Add();
            column3.Width = 332;
            column3.StyleID = "s15";
            WorksheetColumn column4 = sheet.Table.Columns.Add();
            column4.Width = 59;
            column4.StyleID = "s15";
            WorksheetColumn column5 = sheet.Table.Columns.Add();
            column5.Width = 180;
            column5.StyleID = "s15";
            // -----------------------------------------------
            WorksheetRow Row0 = sheet.Table.Rows.Add();
            Row0.Height = 17;
            Row0.AutoFitHeight = false;
            WorksheetCell cell;
            cell = Row0.Cells.Add();
            cell.StyleID = "s16";
            cell = Row0.Cells.Add();
            cell.StyleID = "s16";
            cell = Row0.Cells.Add();
            cell.StyleID = "s16";
            cell = Row0.Cells.Add();
            cell.StyleID = "s16";
            cell = Row0.Cells.Add();
            cell.StyleID = "s16";
            cell = Row0.Cells.Add();
            cell.StyleID = "s16";
            // -----------------------------------------------
            WorksheetRow Row1 = sheet.Table.Rows.Add();
            Row1.Height = 12;
            Row1.AutoFitHeight = false;
            cell = Row1.Cells.Add();
            cell.StyleID = "s16";
            cell = Row1.Cells.Add();
            cell.StyleID = "s17";
            cell = Row1.Cells.Add();
            cell.StyleID = "s18";
            cell = Row1.Cells.Add();
            cell.StyleID = "s18";
            cell = Row1.Cells.Add();
            cell.StyleID = "s18";
            cell = Row1.Cells.Add();
            cell.StyleID = "s19";
            // -----------------------------------------------
            WorksheetRow Row2 = sheet.Table.Rows.Add();
            Row2.Height = 15;
            Row2.AutoFitHeight = false;
            cell = Row2.Cells.Add();
            cell.StyleID = "s16";
            cell = Row2.Cells.Add();
            cell.StyleID = "s20";
            cell = Row2.Cells.Add();
            cell.StyleID = "s21";
            cell.Data.Type = DataType.String;
            cell.MergeAcross = 2;
            cell = Row2.Cells.Add();
            cell.StyleID = "s22";
            // -----------------------------------------------
            //WorksheetRow Row3 = sheet.Table.Rows.Add();
            //Row3.Height = 15;
            //Row3.AutoFitHeight = false;
            //cell = Row3.Cells.Add();
            //cell.StyleID = "s16";
            //cell = Row3.Cells.Add();
            //cell.StyleID = "s20";
            //cell = Row3.Cells.Add();
            //cell.StyleID = "s23";
            //cell.Data.Type = DataType.String;
            //cell.Data.Text = "Run By : " + BaseForm.UserID + "                                                        Date : " + String.Format("{0:G}", DateTime.Now.Date);

            //cell.MergeAcross = 2;
            //cell = Row3.Cells.Add();
            //cell.StyleID = "s22";
            //// -----------------------------------------------
            //WorksheetRow Row4 = sheet.Table.Rows.Add();
            //Row4.Height = 15;
            //Row4.AutoFitHeight = false;
            //cell = Row4.Cells.Add();
            //cell.StyleID = "s16";
            //cell = Row4.Cells.Add();
            //cell.StyleID = "s20";
            //cell = Row4.Cells.Add();
            //cell.StyleID = "s23";
            //cell.Data.Type = DataType.String;
            //cell.Data.Text = "---------------------------------------------------------------------------------" +
            //    "-----------------------------------------------------------------------------";
            //cell.MergeAcross = 2;
            //cell = Row4.Cells.Add();
            //cell.StyleID = "s22";
            //// -----------------------------------------------
            WorksheetRow Row5 = sheet.Table.Rows.Add();
            Row5.Height = 15;
            Row5.AutoFitHeight = false;
            cell = Row5.Cells.Add();
            cell.StyleID = "s16";
            cell = Row5.Cells.Add();
            cell.StyleID = "s20";
            cell = Row5.Cells.Add();
            cell.StyleID = "s21";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "Selected Report Parameters";
            cell.MergeAcross = 2;
            cell = Row5.Cells.Add();
            cell.StyleID = "s22";
            // -----------------------------------------------
            WorksheetRow Row6 = sheet.Table.Rows.Add();
            Row6.Height = 15;
            Row6.AutoFitHeight = false;
            cell = Row6.Cells.Add();
            cell.StyleID = "s16";
            cell = Row6.Cells.Add();
            cell.StyleID = "s20";
            cell = Row6.Cells.Add();
            cell.StyleID = "s21";
            cell.Data.Type = DataType.String;
            cell.MergeAcross = 2;
            cell = Row6.Cells.Add();
            cell.StyleID = "s22";
            // -----------------------------------------------
            string Header_year = string.Empty;
            if (CmbYear.Visible == true)
                Header_year = "Year : " + ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString();
            WorksheetRow Row7 = sheet.Table.Rows.Add();
            Row7.Height = 15;
            Row7.AutoFitHeight = false;
            cell = Row7.Cells.Add();
            cell.StyleID = "s16";
            cell = Row7.Cells.Add();
            cell.StyleID = "s20";
            cell = Row7.Cells.Add();
            cell.StyleID = "s21";
            cell.Data.Type = DataType.String;
            cell.Data.Text = Txt_HieDesc.Text.Trim() + "     " + Header_year;
            cell.MergeAcross = 2;
            cell = Row7.Cells.Add();
            cell.StyleID = "s22";
            // -----------------------------------------------
            WorksheetRow Row8 = sheet.Table.Rows.Add();
            Row8.Height = 15;
            Row8.AutoFitHeight = false;
            cell = Row8.Cells.Add();
            cell.StyleID = "s16";
            cell = Row8.Cells.Add();
            cell.StyleID = "s20";
            cell = Row8.Cells.Add();
            cell.StyleID = "s21";
            cell.Data.Type = DataType.String;
            cell.MergeAcross = 2;
            cell = Row8.Cells.Add();
            cell.StyleID = "s22";
            // -----------------------------------------------
            WorksheetRow Row9 = sheet.Table.Rows.Add();
            Row9.Height = 15;
            Row9.AutoFitHeight = false;
            cell = Row9.Cells.Add();
            cell.StyleID = "s16";
            cell = Row9.Cells.Add();
            cell.StyleID = "s20";
            cell = Row9.Cells.Add();
            cell.StyleID = "s21";
            cell.Data.Type = DataType.String;
            cell.MergeAcross = 2;
            cell = Row9.Cells.Add();
            cell.StyleID = "s22";
            // -----------------------------------------------
            WorksheetRow Row10 = sheet.Table.Rows.Add();
            cell = Row10.Cells.Add();
            cell.StyleID = "s16";
            cell = Row10.Cells.Add();
            cell.StyleID = "s20";
            Row10.Cells.Add("Date Selection", DataType.String, "s21");
            Row10.Cells.Add(" : " + (Rb_MS_Date.Checked == true ? Rb_MS_Date.Text : Rb_MS_AddDate.Text), DataType.String, "s21");
            cell = Row10.Cells.Add();
            cell.StyleID = "s16";
            cell = Row10.Cells.Add();
            cell.StyleID = "s22";


            // -----------------------------------------------
            WorksheetRow Row11 = sheet.Table.Rows.Add();
            cell = Row11.Cells.Add();
            cell.StyleID = "s16";
            cell = Row11.Cells.Add();
            cell.StyleID = "s20";
            Row11.Cells.Add("Reference Period      Start Date", DataType.String, "s21");
            Row11.Cells.Add(": " + Ref_From_Date.Text + "    End Date: " + Ref_To_Date.Text, DataType.String, "s21");
            cell = Row11.Cells.Add();
            cell.StyleID = "s16";
            cell = Row11.Cells.Add();
            cell.StyleID = "s22";
            // -----------------------------------------------
            WorksheetRow Row12 = sheet.Table.Rows.Add();
            cell = Row12.Cells.Add();
            cell.StyleID = "s16";
            cell = Row12.Cells.Add();
            cell.StyleID = "s20";
            Row12.Cells.Add("Report Period     Start Date", DataType.String, "s21");
            Row12.Cells.Add(" : " + Rep_From_Date.Text + "    End Date: " + Rep_To_Date.Text, DataType.String, "s21");
            cell = Row12.Cells.Add();
            cell.StyleID = "s16";
            cell = Row12.Cells.Add();
            cell.StyleID = "s22";
            // -----------------------------------------------
            WorksheetRow Row13 = sheet.Table.Rows.Add();
            cell = Row13.Cells.Add();
            cell.StyleID = "s16";
            cell = Row13.Cells.Add();
            cell.StyleID = "s20";
            Row13.Cells.Add("Report Type", DataType.String, "s21");
            Row13.Cells.Add(" : " + (rdoProgcount.Checked == true ? rdoProgcount.Text : rdoServiceCount.Text), DataType.String, "s21");
            cell = Row13.Cells.Add();
            cell.StyleID = "s16";
            cell = Row13.Cells.Add();
            cell.StyleID = "s22";
            // -----------------------------------------------
            WorksheetRow Row14 = sheet.Table.Rows.Add();
            cell = Row14.Cells.Add();
            cell.StyleID = "s16";
            cell = Row14.Cells.Add();
            cell.StyleID = "s20";
            Row14.Cells.Add("Unduplicated Program count", DataType.String, "s21");
            Row14.Cells.Add(" : " + (chkbUndupTable.Checked == true ? "Yes" : "No"), DataType.String, "s21");
            cell = Row14.Cells.Add();
            cell.StyleID = "s16";
            cell = Row14.Cells.Add();
            cell.StyleID = "s22";


            Row14 = sheet.Table.Rows.Add();
            cell = Row14.Cells.Add();
            cell.StyleID = "s16";
            cell = Row14.Cells.Add();
            cell.StyleID = "s20";
            Row14.Cells.Add("Print Audit Report", DataType.String, "s21");
            Row14.Cells.Add(" : " + (chkPrintAuditreport.Checked == true ? "Yes" : "No"), DataType.String, "s21");
            cell = Row14.Cells.Add();
            cell.StyleID = "s16";
            cell = Row14.Cells.Add();
            cell.StyleID = "s22";

            //// -----------------------------------------------
            WorksheetRow Row24 = sheet.Table.Rows.Add();
            cell = Row24.Cells.Add();
            cell.StyleID = "s16";
            cell = Row24.Cells.Add();
            cell.StyleID = "s24";
            Row24.Cells.Add("", DataType.String, "s25");
            cell = Row24.Cells.Add();
            cell.StyleID = "s25";
            cell.Data.Type = DataType.String;
            cell = Row24.Cells.Add();
            cell.StyleID = "s26";
            cell = Row24.Cells.Add();
            cell.StyleID = "s27";
            // -----------------------------------------------
            //  Options
            // -----------------------------------------------
            sheet.Options.ProtectObjects = false;
            sheet.Options.ProtectScenarios = false;
            sheet.Options.PageSetup.Layout.Orientation = CarlosAg.ExcelXmlWriter.Orientation.Landscape;
            sheet.Options.PageSetup.Header.Margin = 0.2F;
            sheet.Options.PageSetup.Footer.Data = "&R&\"Arial,Regular\"&10&P";
            sheet.Options.PageSetup.Footer.Margin = 0.2F;
            sheet.Options.PageSetup.PageMargins.Bottom = 0.5508299F;
            sheet.Options.PageSetup.PageMargins.Left = 0.2F;
            sheet.Options.PageSetup.PageMargins.Right = 0.2F;
            sheet.Options.PageSetup.PageMargins.Top = 0.2F;
            sheet.Options.Print.HorizontalResolution = 300;
            sheet.Options.Print.VerticalResolution = 300;
            sheet.Options.Print.ValidPrinterInfo = true;
        }

        private void GenerateWorksheetDetails(Workbook book, DataTable dt)
        {
            Worksheet sheet = book.Worksheets.Add("Details");
            sheet.Table.DefaultRowHeight = 14.4F;
            //sheet.Table.ExpandedColumnCount = 11;
            sheet.Table.FullColumns = 1;
            sheet.Table.FullRows = 1;
            sheet.Table.StyleID = "s15";

            if (chkbUndupTable.Checked && rdoProgcount.Checked)
            {
                WorksheetColumn column5 = sheet.Table.Columns.Add();
                column5.Width = 180;
                column5.StyleID = "s15";
            }

            else
            {
                WorksheetColumn column4 = sheet.Table.Columns.Add();
                column4.Width = 180;
                column4.StyleID = "s15";

                WorksheetColumn column5 = sheet.Table.Columns.Add();
                column5.Width = 220;
                column5.StyleID = "s15";
            }

            WorksheetColumn column0 = sheet.Table.Columns.Add();
            column0.Width = 70;
            column0.StyleID = "s15";
            WorksheetColumn column1 = sheet.Table.Columns.Add();
            column1.Width = 70;
            column1.StyleID = "s15";
            WorksheetColumn column2 = sheet.Table.Columns.Add();
            column2.Width = 70;
            column2.StyleID = "s15";

            WorksheetColumn column6 = sheet.Table.Columns.Add();
            column6.Width = 70;
            column6.StyleID = "s15";
            //WorksheetColumn column3 = sheet.Table.Columns.Add();
            //column3.Width = 70;
            //column3.StyleID = "s15";



            WorksheetColumn column7 = sheet.Table.Columns.Add();
            column7.Width = 70;
            column7.StyleID = "s15";

            column7 = sheet.Table.Columns.Add();
            column7.Width = 70;
            column7.StyleID = "s15";

            column7 = sheet.Table.Columns.Add();
            column7.Width = 150;
            column7.StyleID = "s15";

            column7 = sheet.Table.Columns.Add();
            column7.Width = 70;
            column7.StyleID = "s15";

            column7 = sheet.Table.Columns.Add();
            column7.Width = 70;
            column7.StyleID = "s15";

            column7 = sheet.Table.Columns.Add();
            column7.Width = 70;
            column7.StyleID = "s15";

            //column7 = sheet.Table.Columns.Add();
            //column7.Width = 70;
            //column7.StyleID = "s15";
            // -----------------------------------------------
            WorksheetRow Row0 = sheet.Table.Rows.Add();
            Row0.Height = 39;
            if (chkbUndupTable.Checked && rdoProgcount.Checked)
            {
                Row0.Cells.Add("Program", DataType.String, "s28");
            }
            else
            {
                Row0.Cells.Add("Program", DataType.String, "s28");
                Row0.Cells.Add("Service/Activity", DataType.String, "s28");
            }
            if (Rb_MS_Date.Checked)
            {
                Row0.Cells.Add("Add Date", DataType.String, "s28");
            }
            else
            {
                Row0.Cells.Add("Service Date", DataType.String, "s28");
            }
            Row0.Cells.Add("Reference Period Count", DataType.String, "s28");
            if (Rb_MS_Date.Checked)
            {
                Row0.Cells.Add("Service Date", DataType.String, "s28");

            }
            else
            {
                Row0.Cells.Add("Add Date", DataType.String, "s28");
            }
            Row0.Cells.Add("Report Period Count", DataType.String, "s28");
            Row0.Cells.Add("Intake Hierarchy", DataType.String, "s28");
            Row0.Cells.Add("App No", DataType.String, "s28");
            Row0.Cells.Add("Name", DataType.String, "s28");
            Row0.Cells.Add("Client ID", DataType.String, "s28");



            Row0.Cells.Add("Add Operator", DataType.String, "s28");

            Row0.Cells.Add("Amount", DataType.String, "s28");


            // -----------------------------------------------

            string strProgramDesc = string.Empty;
            string strServiceDesc = string.Empty;
            int intTotalrefcount = 0; int intTotalrepcount = 0;
            int intTotalserrefcount = 0; int intTotalserrepcount = 0;
            foreach (DataRow SelRow in dt.Rows)
            {

                if (strServiceDesc != string.Empty)
                {
                    if (strServiceDesc != SelRow["Act_Desc"].ToString().Trim())
                    {
                        WorksheetRow RowTot = sheet.Table.Rows.Add();
                        RowTot.Cells.Add("SERVICE TOTALS", DataType.String, "s111blue");
                        if (chkbUndupTable.Checked && rdoProgcount.Checked)
                        {
                            RowTot.Cells.Add("", DataType.String, "s113blue");
                        }
                        else
                        {
                            RowTot.Cells.Add("", DataType.String, "s113blue");
                            RowTot.Cells.Add("", DataType.String, "s113blue");
                        }
                        RowTot.Cells.Add(intTotalserrefcount.ToString(), DataType.String, "s113blue");
                        RowTot.Cells.Add("", DataType.String, "s113blue");
                        RowTot.Cells.Add(intTotalserrepcount.ToString(), DataType.String, "s113blue");
                        RowTot.Cells.Add("", DataType.String, "s113blue");
                        RowTot.Cells.Add("", DataType.String, "s113blue");
                        RowTot.Cells.Add("", DataType.String, "s113blue");
                        RowTot.Cells.Add("", DataType.String, "s113blue");
                        RowTot.Cells.Add("", DataType.String, "s113blue");
                        RowTot.Cells.Add("", DataType.String, "s113blue");
                        intTotalserrefcount = 0; intTotalserrepcount = 0;

                    }
                }

                if (strProgramDesc != string.Empty)
                {
                    if (strProgramDesc != SelRow["Act_ProgramDesc"].ToString().Trim())
                    {
                        WorksheetRow RowTot = sheet.Table.Rows.Add();
                        RowTot.Cells.Add("PROGRAM TOTALS", DataType.String, "s111");
                        if (chkbUndupTable.Checked && rdoProgcount.Checked)
                        {
                            RowTot.Cells.Add("", DataType.String, "s113");
                        }
                        else
                        {
                            RowTot.Cells.Add("", DataType.String, "s113");
                            RowTot.Cells.Add("", DataType.String, "s113");
                        }
                        RowTot.Cells.Add(intTotalrefcount.ToString(), DataType.String, "s113");
                        RowTot.Cells.Add("", DataType.String, "s113");
                        RowTot.Cells.Add(intTotalrepcount.ToString(), DataType.String, "s113");
                        RowTot.Cells.Add("", DataType.String, "s113");
                        RowTot.Cells.Add("", DataType.String, "s113");
                        RowTot.Cells.Add("", DataType.String, "s113");
                        RowTot.Cells.Add("", DataType.String, "s113");
                        RowTot.Cells.Add("", DataType.String, "s113");
                        RowTot.Cells.Add("", DataType.String, "s113");
                        intTotalrefcount = intTotalrepcount = 0;
                    }
                }
                strServiceDesc = SelRow["Act_Desc"].ToString().Trim();
                strProgramDesc = SelRow["Act_ProgramDesc"].ToString().Trim();
                intTotalrefcount = intTotalrefcount + 1;
                intTotalserrefcount = intTotalserrefcount + 1;
                if (SelRow["Act_RepDate"].ToString().Trim() != string.Empty)
                {
                    intTotalrepcount = intTotalrepcount + 1;
                    intTotalserrepcount = intTotalserrepcount + 1;
                }
                WorksheetRow Row2 = sheet.Table.Rows.Add();
                if (chkbUndupTable.Checked && rdoProgcount.Checked)
                {
                    Row2.Cells.Add(SelRow["Act_ProgramDesc"].ToString().Trim(), DataType.String, "s23");
                }
                else
                {
                    Row2.Cells.Add(SelRow["Act_ProgramDesc"].ToString().Trim(), DataType.String, "s23");
                    Row2.Cells.Add(SelRow["Act_Desc"].ToString().Trim(), DataType.String, "s23");

                }
                if (Rb_MS_Date.Checked)
                {
                    Row2.Cells.Add(LookupDataAccess.Getdate(SelRow["Act_DateAdd"].ToString().Trim()), DataType.String, "s23");

                }
                else
                {
                    Row2.Cells.Add(LookupDataAccess.Getdate(SelRow["Act_Date"].ToString().Trim()), DataType.String, "s23");

                }
                Row2.Cells.Add(intTotalrefcount.ToString(), DataType.String, "s23center");
                if (Rb_MS_Date.Checked)
                {
                    Row2.Cells.Add(LookupDataAccess.Getdate(SelRow["Act_Date"].ToString().Trim()), DataType.String, "s23");

                }
                else
                {
                    Row2.Cells.Add(LookupDataAccess.Getdate(SelRow["Act_DateAdd"].ToString().Trim()), DataType.String, "s23");

                }
                if (SelRow["Act_RepCount"].ToString().Trim() != string.Empty)
                    Row2.Cells.Add(intTotalrepcount.ToString(), DataType.String, "s23center");
                else
                    Row2.Cells.Add("", DataType.String, "s23");
                Row2.Cells.Add(SelRow["Act_Agy"].ToString().Trim() + SelRow["Act_Dept"].ToString().Trim() + SelRow["Act_Prog"].ToString().Trim() + SelRow["Act_Year"].ToString().Trim(), DataType.String, "s23");

                Row2.Cells.Add(SelRow["Act_App"].ToString().Trim(), DataType.String, "s23");
                Row2.Cells.Add(SelRow["Act_LName"].ToString().Trim() + ", " + SelRow["Act_FName"].ToString().Trim() + " " + SelRow["Act_MName"].ToString().Trim(), DataType.String, "s23");
                Row2.Cells.Add(SelRow["Act_ClientID"].ToString().Trim(), DataType.String, "s23");
                Row2.Cells.Add(SelRow["Act_AddOperator"].ToString().Trim(), DataType.String, "s23");

                Row2.Cells.Add(SelRow["Act_Cost"].ToString().Trim(), DataType.String, "s23");



            }
            if (dt.Rows.Count > 0)
            {

                WorksheetRow RowTot = sheet.Table.Rows.Add();
                RowTot.Cells.Add("SERVICE TOTALS", DataType.String, "s111blue");
                if (chkbUndupTable.Checked && rdoProgcount.Checked)
                {
                    RowTot.Cells.Add("", DataType.String, "s113blue");
                }
                else
                {
                    RowTot.Cells.Add("", DataType.String, "s113blue");
                    RowTot.Cells.Add("", DataType.String, "s113blue");
                }
                RowTot.Cells.Add(intTotalserrefcount.ToString(), DataType.String, "s113blue");
                RowTot.Cells.Add("", DataType.String, "s113blue");
                RowTot.Cells.Add(intTotalserrepcount.ToString(), DataType.String, "s113blue");
                RowTot.Cells.Add("", DataType.String, "s113blue");
                RowTot.Cells.Add("", DataType.String, "s113blue");
                RowTot.Cells.Add("", DataType.String, "s113blue");
                RowTot.Cells.Add("", DataType.String, "s113blue");
                RowTot.Cells.Add("", DataType.String, "s113blue");
                RowTot.Cells.Add("", DataType.String, "s113blue");

                RowTot = sheet.Table.Rows.Add();
                RowTot.Cells.Add("PROGRAM TOTALS", DataType.String, "s111");
                if (chkbUndupTable.Checked && rdoProgcount.Checked)
                {
                    RowTot.Cells.Add("", DataType.String, "s113");
                }
                else
                {
                    RowTot.Cells.Add("", DataType.String, "s113");
                    RowTot.Cells.Add("", DataType.String, "s113");
                }
                RowTot.Cells.Add(intTotalrefcount.ToString(), DataType.String, "s113");
                RowTot.Cells.Add("", DataType.String, "s113");
                RowTot.Cells.Add(intTotalrepcount.ToString(), DataType.String, "s113");
                RowTot.Cells.Add("", DataType.String, "s113");
                RowTot.Cells.Add("", DataType.String, "s113");
                RowTot.Cells.Add("", DataType.String, "s113");
                RowTot.Cells.Add("", DataType.String, "s113");
                RowTot.Cells.Add("", DataType.String, "s113");
                RowTot.Cells.Add("", DataType.String, "s113");
            }


        }

        public void Generate(string filename)
        {
            Workbook book = new Workbook();
            // -----------------------------------------------
            //  Properties
            // -----------------------------------------------
            book.Properties.Author = "Lenovo";
            book.Properties.LastAuthor = "Lenovo";
            book.Properties.Created = new System.DateTime(2021, 9, 20, 18, 11, 44, 0);
            book.Properties.LastSaved = new System.DateTime(2021, 9, 20, 18, 11, 44, 0);
            book.Properties.Version = "12.00";
            book.ExcelWorkbook.WindowHeight = 13176;
            book.ExcelWorkbook.WindowWidth = 23256;
            book.ExcelWorkbook.WindowTopX = -108;
            book.ExcelWorkbook.WindowTopY = -108;
            book.ExcelWorkbook.ActiveSheetIndex = 1;
            book.ExcelWorkbook.ProtectWindows = false;
            book.ExcelWorkbook.ProtectStructure = false;
            // -----------------------------------------------
            //  Generate Styles
            // -----------------------------------------------
            this.GenerateStyles(book.Styles);
            // -----------------------------------------------
            //  Generate Parameters Worksheet
            // -----------------------------------------------
            // this.GenerateWorksheetParameters(book.Worksheets);
            // -----------------------------------------------
            //  Generate Details Worksheet
            // -----------------------------------------------
            // this.GenerateWorksheetDetails(book.Worksheets);
            book.Save(filename);
        }

        private void rdoServiceCount_Click(object sender, EventArgs e)
        {
            if (rdoServiceCount.Checked)
            {
                chkbUndupTable.Text = "Unduplicated Service Count";
            }
            else
            {
                chkbUndupTable.Text = "Unduplicated Program Count";
            }
        }

        private void CASB0013Form_ToolClick(object sender, ToolClickEventArgs e)
        {
            Application.Navigate(CommonFunctions.CreateZenHelps(Privileges.Program, 0, BaseForm.BusinessModuleID.ToString(), "", ""), target: "_blank");
        }

        private void OnSaveExcel_Report(DataTable dt, string pdfname)
        {
            PdfName = pdfname.Trim().Remove(pdfname.Trim().Length - 4);
            //PdfName = strFolderPath + PdfName;
            PdfName = PdfName + ".xls";

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


            Workbook book = new Workbook();

            this.GenerateStyles(book.Styles);



            try
            {
                GenerateWorksheetParameters(book);

                Worksheet sheet = book.Worksheets.Add("Summary");
                sheet.Table.DefaultRowHeight = 14.4F;
                // sheet.Table.ExpandedColumnCount = 5;
                sheet.Table.FullColumns = 1;
                sheet.Table.FullRows = 1;
                sheet.Table.StyleID = "s15";
                WorksheetColumn column0 = sheet.Table.Columns.Add();
                column0.Width = 300;
                column0.StyleID = "s15";
                if (rdoServiceCount.Checked)
                {

                    WorksheetColumn column1 = sheet.Table.Columns.Add();
                    column1.Width = 200;

                    column1.StyleID = "s15";
                }
                WorksheetColumn column2 = sheet.Table.Columns.Add();
                column2.Width = 200;
                column2.StyleID = "s15";
                WorksheetColumn column3 = sheet.Table.Columns.Add();
                column3.Width = 200;

                WorksheetRow Row0 = sheet.Table.Rows.Add();
                Row0.Height = 25;
                Row0.Cells.Add("Program", DataType.String, "s28");
                if (rdoServiceCount.Checked)
                {
                    Row0.Cells.Add("Service", DataType.String, "s28");
                }
                Row0.Cells.Add(LookupDataAccess.Getdate(Rep_From_Date.Value.ToString()) + " - " + LookupDataAccess.Getdate(Rep_To_Date.Value.ToString()), DataType.String, "s28");
                Row0.Cells.Add(LookupDataAccess.Getdate(Ref_From_Date.Value.ToString()) + " - " + LookupDataAccess.Getdate(Ref_To_Date.Value.ToString()), DataType.String, "s28");

                // -----------------------------------------------

                foreach (DataGridViewRow item in gvwProgramData.Rows)
                {
                    WorksheetRow Row2 = sheet.Table.Rows.Add();
                    Row2.Cells.Add(item.Cells["gvtProgramDesc"].Value.ToString(), DataType.String, "s23");
                    if (rdoServiceCount.Checked)
                    {
                        Row2.Cells.Add(item.Cells["gvtServicecode"].Value.ToString(), DataType.String, "s23");
                    }
                    Row2.Cells.Add(item.Cells["gvtReportcount"].Value.ToString(), DataType.String, "s23center");
                    Row2.Cells.Add(item.Cells["gvtRefcount"].Value.ToString(), DataType.String, "s23center");

                }



                if (chkPrintAuditreport.Checked == true)
                    GenerateWorksheetDetails(book, dt);
                book.Save(PdfName);


                FileStream stream = new FileStream(PdfName, FileMode.Create);

                book.Save(stream);
                stream.Close();


            }
            catch (Exception ex) { }


        }

    }
}