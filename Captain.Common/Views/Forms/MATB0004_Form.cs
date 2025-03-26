using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;
using Captain.Common.Views.Forms.Base;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using Wisej.Web;
using System.Text;
using Syncfusion.XlsIO.Implementation.XmlSerialization;

namespace Captain.Common.Views.Forms
{
    public partial class MATB0004_Form : Form
    {
        #region Properties & Private Variables

        private ErrorProvider _errorProvider = null;
        CaptainModel _model = null;


        public BaseForm _baseForm
        {
            get;
            set;
        }
        public PrivilegeEntity _privilegeEntity
        {
            get;
            set;
        }
        public string Agency
        {
            get; set;
        }
        public string Depart
        {
            get; set;
        }
        public string Program
        {
            get; set;
        }
        public string ReportPath
        {
            get; set;
        }
        public string propReportPath
        {
            get; set;
        }
        public string MatCode
        {
            get; set;
        }
        public string ScrCode
        {
            get; set;
        }
        public string ScoreSheet
        {
            get; set;
        }
        public string propCopy_PreAssement
        {
            get; set;
        }
        public List<CommonEntity> ListcommonEntity
        {
            get; set;
        }

        List<CommonEntity> County_List = new List<CommonEntity>();
        #endregion

        public MATB0004_Form(BaseForm baseForm, PrivilegeEntity privilegeEntity)
        {
            InitializeComponent();

            _model = new CaptainModel();

            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 1;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            _baseForm = baseForm;
            _privilegeEntity = privilegeEntity;

            this.Text = _privilegeEntity.PrivilegeName.Trim();

            Agency = _baseForm.BaseAgency;
            Depart = _baseForm.BaseDept;
            Program = _baseForm.BaseProg;
            Set_Report_Hierarchy(_baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, _baseForm.BaseYear);
            ReportPath = _model.lookupDataAccess.GetReportPath();
            propReportPath = _model.lookupDataAccess.GetReportPath();

            ListcommonEntity = new List<CommonEntity>();
            
            County_List = _model.ZipCodeAndAgency.GetCounty();

            Fill_Matrix_Combo();
        }

        #region Hierarchy Code

        string Program_Year;

        private void Pb_Search_Hie_Click(object sender, EventArgs e)
        {
            HierarchieSelection hierarchieSelectionForm = new HierarchieSelection(_baseForm, Current_Hierarchy_DB, "Master", "", "A", "Reports", _baseForm.UserID);
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
                    Depart = hierarchy.Substring(2, 2);
                    Program = hierarchy.Substring(4, 2);
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
                this.Txt_HieDesc.Size = new System.Drawing.Size(625, 25);
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
                this.Txt_HieDesc.Size = new System.Drawing.Size(550, 25);
            else
                this.Txt_HieDesc.Size = new System.Drawing.Size(625, 25);
        }

        #endregion

        #region Save and Get Parameters
        DG_Browse_Entity Search_Entity = new DG_Browse_Entity(true);
        private void btnSaveParameters_Click(object sender, EventArgs e)
        {
            if (ValidateReport())
            {
                ControlCard_Entity Save_Entity = new ControlCard_Entity(true);
                Save_Entity.Scr_Code = _privilegeEntity.Program;
                Save_Entity.UserID = _baseForm.UserID;
                Save_Entity.Card_1 = Get_XML_Format_for_Report_Controls();
                Save_Entity.Card_2 = string.Empty;
                Save_Entity.Card_3 = string.Empty;
                Save_Entity.Module = _baseForm.BusinessModuleID;

                Report_Get_SaveParams_Form Save_Form = new Report_Get_SaveParams_Form(Save_Entity, "Save", _baseForm, _privilegeEntity);
                Save_Form.StartPosition = FormStartPosition.CenterScreen;
                Save_Form.ShowDialog();
            }
        }

        private void btnGetParameters_Click(object sender, EventArgs e)
        {
            ControlCard_Entity Save_Entity = new ControlCard_Entity(true);
            Save_Entity.Scr_Code = _privilegeEntity.Program;
            Save_Entity.UserID = _baseForm.UserID;
            Save_Entity.Module = _baseForm.BusinessModuleID;
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
            string datechk = string.Empty;
            string RepFormat = string.Empty;
            string Catg = string.Empty;
            string RepDup = string.Empty;

            string matrix = ((Captain.Common.Utilities.ListItem)cmbMatrix.SelectedItem).Value.ToString();
            string scale = ((Captain.Common.Utilities.ListItem)cmbScale.SelectedItem).Value.ToString();

            string Year = string.Empty;
            if (CmbYear.Visible == true)
                Year = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString();

            string county = string.Empty;
            if (rdbCounty_All.Checked)
                county = "**";
            else
                county = Get_Sel_County();

            StringBuilder str = new StringBuilder();
            str.Append("<Rows>");

            str.Append("<Row AGENCY = \"" + Agency + "\" DEPT = \"" + Depart + "\" PROG = \"" + Program + "\" YEAR = \"" + Year + "\" FromDate = \"" + dtpFrom.Value.Date + "\" ToDate = \"" + dtpTo.Value.Date + "\" MATRIX = \"" + matrix + "\" SCALE = \"" + scale + "\" COUNTY = \"" + county + "\" />");

            str.Append("</Rows>");

            return str.ToString();
        }

        private void Set_Report_Controls(DataTable Tmp_Table)
        {
            if (Tmp_Table != null && Tmp_Table.Rows.Count > 0)
            {
                DataRow dr = Tmp_Table.Rows[0];
                DataColumnCollection columns = Tmp_Table.Columns;

                Set_Report_Hierarchy(dr["AGENCY"].ToString(), dr["DEPT"].ToString(), dr["PROG"].ToString(), dr["YEAR"].ToString());

                CommonFunctions.SetComboBoxValue(cmbMatrix, dr["MATRIX"].ToString());
                CommonFunctions.SetComboBoxValue(cmbScale, dr["SCALE"].ToString());

                dtpFrom.Value = Convert.ToDateTime(dr["FromDate"]);
                dtpTo.Value = Convert.ToDateTime(dr["ToDate"]);

                if (columns.Contains("COUNTY"))
                {
                    if ((dr["COUNTY"].ToString() == "**"))
                        rdbCounty_All.Checked = true;
                    else
                    {
                        rdbCounty_Sel.Checked = true;
                        Fill_County_Selected_List(dr["COUNTY"].ToString().Trim());
                    }
                }
                else
                    rdbCounty_All.Checked = true;
            }
        }

        #endregion

        #region Combo Box Filling

        private void Fill_Matrix_Combo()
        {
            MATDEFEntity Search_Entity = new MATDEFEntity(true);
            Search_Entity.Scale_Code = "0";
            List<MATDEFEntity> matdefEntity = _model.MatrixScalesData.Browse_MATDEF(Search_Entity, "Browse");

            cmbMatrix.Items.Clear();
            cmbMatrix.ColorMember = "FavoriteColor";
            matdefEntity = matdefEntity.OrderBy(u => u.Active).ToList();
            if (matdefEntity.Count > 0)
            {
                foreach (MATDEFEntity matdef in matdefEntity)
                    cmbMatrix.Items.Add(new Captain.Common.Utilities.ListItem(matdef.Desc, matdef.Mat_Code, matdef.Interval, matdef.Show_BA, matdef.Date_option, matdef.Active.ToString(), matdef.Active.Equals("A") ? Color.Black : Color.Red));
            }
            else
                cmbMatrix.Items.Insert(0, new Captain.Common.Utilities.ListItem("    ", "0"));

            if (cmbMatrix.Items.Count > 0)
                cmbMatrix.SelectedIndex = 0;
        }

        private void cmbMatrix_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbMatrix.SelectedItem.ToString().Trim() != string.Empty)
            {
                cmbScale.Items.Clear();

                MatCode = ((Captain.Common.Utilities.ListItem)cmbMatrix.SelectedItem).Value.ToString();
                ScoreSheet = ((Captain.Common.Utilities.ListItem)cmbMatrix.SelectedItem).ID.ToString();
                propCopy_PreAssement = ((Captain.Common.Utilities.ListItem)cmbMatrix.SelectedItem).ValueDisplayCode.ToString();

                List<MATDEFEntity> matdefEntity = _model.MatrixScalesData.Get_Matdef_MatCode(((Captain.Common.Utilities.ListItem)cmbMatrix.SelectedItem).Value.ToString());
                matdefEntity = matdefEntity.FindAll(u => u.Sequence != string.Empty);
                matdefEntity = matdefEntity.OrderBy(u => Convert.ToInt32(u.Sequence)).ToList();

                foreach (MATDEFEntity matdef in matdefEntity)
                {
                    cmbScale.Items.Add(new Captain.Common.Utilities.ListItem(matdef.Desc, matdef.Scale_Code, matdef.Assessment_Type, matdef.Score));
                }

                if (cmbScale.Items.Count > 0)
                    cmbScale.SelectedIndex = 0;
            }
            else
            {
                cmbScale.Items.Clear();
                cmbScale.Items.Insert(0, new Captain.Common.Utilities.ListItem("    ", "0"));
                cmbScale.SelectedIndex = 0;
            }
        }

        #endregion

        DataSet dsCounts = new DataSet();
        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (ValidateReport())
            {
                try
                {
                    string Year = string.Empty;
                    if (CmbYear.Visible == true)
                        Year = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString().Trim();

                    string Mat_Code = string.Empty;
                    if (cmbMatrix.Items.Count > 0)
                        Mat_Code = ((Captain.Common.Utilities.ListItem)cmbMatrix.SelectedItem).Value.ToString().Trim();

                    string Scale_Code = string.Empty;
                    if (cmbScale.Items.Count > 0)
                        Scale_Code = ((Captain.Common.Utilities.ListItem)cmbScale.SelectedItem).Value.ToString().Trim();

                    string county = string.Empty;
                    if (rdbCounty_All.Checked)
                        county = "**";
                    else
                        county = Get_Sel_County();

                    dsCounts = DatabaseLayer.MatrixDB.MATB0004_REPORT(Current_Hierarchy.Substring(0, 2), Current_Hierarchy.Substring(2, 2), Current_Hierarchy.Substring(4, 2), Year, Mat_Code, Scale_Code, dtpFrom.Text, dtpTo.Text, county);


                    PdfListForm pdfListForm = new PdfListForm(_baseForm, _privilegeEntity, false, propReportPath, "PDF");
                    pdfListForm.FormClosed += new FormClosedEventHandler(PDF_Report);
                    pdfListForm.FormClosed += new FormClosedEventHandler(Excel_Report);
                    pdfListForm.StartPosition = FormStartPosition.CenterScreen;
                    pdfListForm.ShowDialog();
                }
                catch(Exception ex) { }
            }
        }

        private void PrintPDFHeaderPage(Document document, PdfWriter writer)
        {
            #region Font styles

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

            #endregion

            PdfPTable Headertable = new PdfPTable(5);
            Headertable.TotalWidth = 510f;
            Headertable.WidthPercentage = 100;
            Headertable.LockedWidth = true;
            float[] Headerwidths = new float[] { 30f, 30f, 50f, 30f, 70f };
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
            if (_baseForm.BaseAgencyControlDetails.AgyShortName == "NEOCAA")
            {
                ImgName = "NEOCAA_" + strAgy + "_LOGO.png";
            }
            else
                ImgName = _baseForm.BaseAgencyControlDetails.AgyShortName + "_00_LOGO.png";

            string imagesPath = "https://capsysdev.capsystems.com/images/PIPlogos/" + ImgName;

            if (imagesPath != "")
            {
                try
                {
                    iTextSharp.text.Image imgLogo = iTextSharp.text.Image.GetInstance(imagesPath);
                    imgLogo.ScaleAbsolute(120f, 50f);
                    PdfPCell cellLogo = new PdfPCell(imgLogo);
                    cellLogo.HorizontalAlignment = Element.ALIGN_CENTER;
                    cellLogo.Rowspan = 2;
                    cellLogo.Padding = 3;
                    cellLogo.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Headertable.AddCell(cellLogo);


                    AgencyControlEntity _agycntrldets = new AgencyControlEntity();
                    _agycntrldets = _baseForm.BaseAgencyControlDetails;

                    if (_baseForm.BaseAgencyControlDetails.AgyShortName == "NEOCAA")
                        _agycntrldets = BAgyControlDetails;
                    else
                        _agycntrldets = _baseForm.BaseAgencyControlDetails;

                    string street = _agycntrldets.Street == "" ? "" : (_agycntrldets.Street + ", ");
                    string city = _agycntrldets.City == "" ? "" : (_agycntrldets.City + ", ");
                    string state = _agycntrldets.State == "" ? "" : (_agycntrldets.State + ", ");
                    string zip1 = _agycntrldets.Zip1 == "" ? "" : _agycntrldets.Zip1.PadLeft(5, '0');
                    string strAddress = street + city + state + zip1;

                    PdfPCell rowH = new PdfPCell(new Phrase(BAgyControlDetails.AgyName, TblParamsHeaderFont));
                    rowH.HorizontalAlignment = Element.ALIGN_CENTER;
                    rowH.Colspan = 5;
                    rowH.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Headertable.AddCell(rowH);

                    PdfPCell row1 = new PdfPCell(new Phrase(strAddress, TblParamsHeaderFont));
                    row1.HorizontalAlignment = Element.ALIGN_CENTER;
                    row1.Colspan = 4;
                    row1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Headertable.AddCell(row1);
                }
                catch (Exception ex)
                {
                    AgencyControlEntity _agycntrldets = new AgencyControlEntity();
                    _agycntrldets = _baseForm.BaseAgencyControlDetails;

                    if (_baseForm.BaseAgencyControlDetails.AgyShortName == "NEOCAA")
                        _agycntrldets = BAgyControlDetails;
                    else
                        _agycntrldets = _baseForm.BaseAgencyControlDetails;

                    string street = _agycntrldets.Street == "" ? "" : (_agycntrldets.Street + ", ");
                    string city = _agycntrldets.City == "" ? "" : (_agycntrldets.City + ", ");
                    string state = _agycntrldets.State == "" ? "" : (_agycntrldets.State + ", ");
                    string zip1 = _agycntrldets.Zip1 == "" ? "" : _agycntrldets.Zip1.PadLeft(5, '0');
                    string strAddress = street + city + state + zip1;

                    PdfPCell rowH = new PdfPCell(new Phrase(BAgyControlDetails.AgyName, TblParamsHeaderFont));
                    rowH.HorizontalAlignment = Element.ALIGN_CENTER;
                    rowH.Colspan = 5;
                    rowH.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Headertable.AddCell(rowH);

                    PdfPCell row1 = new PdfPCell(new Phrase(strAddress, TblParamsHeaderFont));
                    row1.HorizontalAlignment = Element.ALIGN_CENTER;
                    row1.Colspan = 5;
                    row1.FixedHeight = 25f;
                    row1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Headertable.AddCell(row1);
                }

            }

            PdfPCell row2 = new PdfPCell(new Phrase(_privilegeEntity.PrivilegeName, reportNameStyle));
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

            //Row 1
            PdfPCell CH1 = new PdfPCell(new Phrase(lblMatrix.Text, paramsCellStyle));
            CH1.HorizontalAlignment = Element.ALIGN_LEFT;
            CH1.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CH1.Border = iTextSharp.text.Rectangle.BOX;
            CH1.Colspan = 2;
            CH1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEF5FC"));
            Headertable.AddCell(CH1);

            PdfPCell CB1 = new PdfPCell(new Phrase(((Captain.Common.Utilities.ListItem)cmbMatrix.SelectedItem).Text.ToString().Trim(), paramsCellStyle));
            CB1.HorizontalAlignment = Element.ALIGN_LEFT;
            CB1.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CB1.Border = iTextSharp.text.Rectangle.BOX;
            CB1.Colspan = 3;
            CB1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            Headertable.AddCell(CB1);

            //Row 2
            PdfPCell CH2 = new PdfPCell(new Phrase(lblScale.Text, paramsCellStyle));
            CH2.HorizontalAlignment = Element.ALIGN_LEFT;
            CH2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CH2.Border = iTextSharp.text.Rectangle.BOX;
            CH2.Colspan = 2;
            CH2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEF5FC"));
            Headertable.AddCell(CH2);

            PdfPCell CB2 = new PdfPCell(new Phrase(((Captain.Common.Utilities.ListItem)cmbScale.SelectedItem).Text.ToString().Trim(), paramsCellStyle));
            CB2.HorizontalAlignment = Element.ALIGN_LEFT;
            CB2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CB2.Border = iTextSharp.text.Rectangle.BOX;
            CB2.Colspan = 3;
            CB2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            Headertable.AddCell(CB2);

            //Row 3
            PdfPCell CH3 = new PdfPCell(new Phrase(lblDateRangeFrm.Text.Trim(), paramsCellStyle));
            CH3.HorizontalAlignment = Element.ALIGN_LEFT;
            CH3.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CH3.Border = iTextSharp.text.Rectangle.BOX;
            CH3.Colspan = 2;
            CH3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEF5FC"));
            Headertable.AddCell(CH3);

            PdfPCell CB3 = new PdfPCell(new Phrase(dtpFrom.Text.Trim() + "  To: " + dtpTo.Text.Trim(), paramsCellStyle));
            CB3.HorizontalAlignment = Element.ALIGN_LEFT;
            CB3.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CB3.Border = iTextSharp.text.Rectangle.BOX;
            CB3.Colspan = 3;
            CB3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            Headertable.AddCell(CB3);

            //Row 4
            PdfPCell CH4 = new PdfPCell(new Phrase(rdbCounty_All.Checked ? lblCounty.Text.Trim() : "Selected County(ies)", paramsCellStyle));
            CH4.HorizontalAlignment = Element.ALIGN_LEFT;
            CH4.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CH4.Border = iTextSharp.text.Rectangle.BOX;
            CH4.Colspan = 2;
            CH4.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEF5FC"));
            Headertable.AddCell(CH4);

            PdfPCell CB4 = new PdfPCell(new Phrase(rdbCounty_All.Checked ? "All Counties" : Get_Sel_County_Desc(), paramsCellStyle));
            CB4.HorizontalAlignment = Element.ALIGN_LEFT;
            CB4.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CB4.Border = iTextSharp.text.Rectangle.BOX;
            CB4.Colspan = 3;
            CB4.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            Headertable.AddCell(CB4);

            document.Add(Headertable);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase("Generated By: ", fnttimesRoman_Italic), 33, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.GetMemberName(_baseForm.UserProfile.FirstName.Trim(), _baseForm.UserProfile.MI.Trim(), _baseForm.UserProfile.LastName.Trim(), "3"), fnttimesRoman_Italic), 90, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase("Generated On: ", fnttimesRoman_Italic), 410, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase(DateTime.Now.ToString(), fnttimesRoman_Italic), 468, 40, 0);

        }

        string Random_Filename = null; PdfContentByte cb;
        private void PDF_Report(object sender, FormClosedEventArgs e)
        {
            PdfListForm form = sender as PdfListForm;

            if (form.DialogResult == DialogResult.OK)
            {
                #region FileNameBuild

                string PdfName = "Pdf File";
                PdfName = form.GetFileName();
                string NamePdf = PdfName;
                PdfName = propReportPath + _baseForm.UserID + "\\" + PdfName;
                try
                {
                    if (!Directory.Exists(propReportPath + _baseForm.UserID.Trim()))
                    {
                        DirectoryInfo di = Directory.CreateDirectory(propReportPath + _baseForm.UserID.Trim());
                    }
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

                #endregion

                FileStream fs = new FileStream(PdfName, FileMode.Create);

                Document document = new Document(PageSize.A4, 30f, 30f, 30f, 50f);
                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                document.Open();

                #region Font styles

                BaseFont bf_calibri = BaseFont.CreateFont("c:/windows/fonts/Calibri.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
                iTextSharp.text.Font HeaderTextfnt = new iTextSharp.text.Font(bf_calibri, 12, 2, BaseColor.WHITE);
                iTextSharp.text.Font SubHeadTextfnt = new iTextSharp.text.Font(bf_calibri, 10, 2, new BaseColor(26, 71, 119));
                iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_calibri, 8);
                iTextSharp.text.Font TableFonterr = new iTextSharp.text.Font(bf_calibri, 8, 0, BaseColor.RED);
                iTextSharp.text.Font TableFontWhite = new iTextSharp.text.Font(bf_calibri, 8, 2, BaseColor.WHITE);
                iTextSharp.text.Font BoldFont = new iTextSharp.text.Font(bf_calibri, 8, 1);
                iTextSharp.text.Font BoldUnderlineFont = new iTextSharp.text.Font(bf_calibri, 8, 3);
                iTextSharp.text.Font TableFontPrgNotes = new iTextSharp.text.Font(bf_calibri, 8, 0, new BaseColor(111, 48, 160));
                iTextSharp.text.Font TableFontPrgNotesBold = new iTextSharp.text.Font(bf_calibri, 8, 1, new BaseColor(111, 48, 160));
                iTextSharp.text.Font TableFontPrgNotesBoldUndline = new iTextSharp.text.Font(bf_calibri, 8, 5, new BaseColor(111, 48, 160));
                iTextSharp.text.Font fcRed = new iTextSharp.text.Font(bf_calibri, 10, 1, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#806000")));
                iTextSharp.text.Font FTableFont = new iTextSharp.text.Font(bf_calibri, 8);

                #endregion

                #region Cell Color Define

                BaseColor DarkBlue = new BaseColor(26, 71, 119);
                BaseColor SecondBlue = new BaseColor(184, 217, 255);
                BaseColor TblHeaderBlue = new BaseColor(155, 194, 230);

                #endregion

                cb = writer.DirectContent;

                try
                {
                    PrintPDFHeaderPage(document, writer);

                    document.NewPage();

                    DataTable dtSummary_Counts = dsCounts.Tables[0];

                    if (dtSummary_Counts.Rows.Count > 0)
                    {
                        PdfPTable table = new PdfPTable(2);
                        table.TotalWidth = 400f;
                        table.WidthPercentage = 100;
                        table.LockedWidth = true;
                        float[] widths = new float[] { 450f, 50f };
                        table.SetWidths(widths);
                        table.HorizontalAlignment = Element.ALIGN_CENTER;

                        PdfPCell Screen_Name = new PdfPCell(new Phrase(_privilegeEntity.PrivilegeName.Trim(), HeaderTextfnt));
                        Screen_Name.HorizontalAlignment = Element.ALIGN_LEFT;
                        Screen_Name.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        Screen_Name.BackgroundColor = DarkBlue;
                        Screen_Name.Colspan = 2;
                        table.AddCell(Screen_Name);

                        string MatrixDesc = string.Empty;
                        if (cmbMatrix.Items.Count > 0)
                            MatrixDesc = ((Captain.Common.Utilities.ListItem)cmbMatrix.SelectedItem).Text.ToString().Trim();

                        PdfPCell Matrix_Desc = new PdfPCell(new Phrase("Matrix: " + MatrixDesc, SubHeadTextfnt));
                        Matrix_Desc.HorizontalAlignment = Element.ALIGN_LEFT;

                        Matrix_Desc.BorderWidthBottom = 0.3f;
                        Matrix_Desc.BorderWidthLeft = 0.5f;
                        Matrix_Desc.BorderWidthTop = 0.5f;
                        Matrix_Desc.BorderWidthRight = 0.5f;

                        Matrix_Desc.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"));
                        Matrix_Desc.BorderColorRight = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"));
                        Matrix_Desc.BorderColorTop = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"));
                        Matrix_Desc.BorderColorBottom = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"));

                        Matrix_Desc.BackgroundColor = SecondBlue;
                        Matrix_Desc.Colspan = 2;
                        table.AddCell(Matrix_Desc);

                        string ScaleDesc = string.Empty;
                        if (cmbScale.Items.Count > 0)
                            ScaleDesc = ((Captain.Common.Utilities.ListItem)cmbScale.SelectedItem).Text.ToString().Trim();

                        PdfPCell Scale_Desc = new PdfPCell(new Phrase("Scale: " + ScaleDesc, SubHeadTextfnt));
                        Scale_Desc.HorizontalAlignment = Element.ALIGN_LEFT;

                        Scale_Desc.BorderWidthTop = 0f;
                        Scale_Desc.BorderWidthBottom = 0.5f;
                        Scale_Desc.BorderWidthLeft = 0.5f;
                        Scale_Desc.BorderWidthRight = 0.5f;

                        Scale_Desc.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"));
                        Scale_Desc.BorderColorRight = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"));
                        Scale_Desc.BorderColorBottom = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"));   

                        Scale_Desc.BackgroundColor = SecondBlue;
                        Scale_Desc.Colspan = 2;
                        table.AddCell(Scale_Desc);

                        var distinctQues = (from DataRow dRow in dtSummary_Counts.Rows select dRow["MATQUESR_CODE"]).Distinct();

                        foreach (var drRow in distinctQues)
                        {
                            DataTable dt = new DataTable();
                            DataView dv = new DataView(dtSummary_Counts);
                            dv.RowFilter = "MATQUESR_CODE='" + drRow + "'";
                            dt = dv.ToTable();

                            document.NewPage();

                            PdfPCell Space = new PdfPCell(new Phrase("", BoldFont));
                            Space.HorizontalAlignment = Element.ALIGN_LEFT;
                            Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            Space.Colspan = 2;
                            Space.FixedHeight = 10f;
                            table.AddCell(Space);

                            PdfPCell Ques_Desc = new PdfPCell(new Phrase(dt.Rows[0]["QUES_DESC"].ToString().Trim(), BoldFont));
                            Ques_Desc.HorizontalAlignment = Element.ALIGN_LEFT;
                            Ques_Desc.BackgroundColor = SecondBlue;

                            Ques_Desc.BorderWidthBottom = 0.5f;
                            Ques_Desc.BorderWidthLeft = 0.5f;
                            Ques_Desc.BorderWidthTop = 0.5f;
                            Ques_Desc.BorderWidthRight = 0.5f;

                            Ques_Desc.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"));
                            Ques_Desc.BorderColorRight = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"));
                            Ques_Desc.BorderColorTop = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"));
                            Ques_Desc.BorderColorBottom = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"));
                            Ques_Desc.Colspan = 2;
                            table.AddCell(Ques_Desc);

                            #region Column Headers

                            PdfPCell Resp_Desc_Header = new PdfPCell(new Phrase("Response", BoldFont));
                            Resp_Desc_Header.HorizontalAlignment = Element.ALIGN_LEFT;
                            Resp_Desc_Header.BackgroundColor = SecondBlue;
                            Resp_Desc_Header.BorderWidthBottom = 0.5f;
                            Resp_Desc_Header.BorderWidthTop = 0f;
                            Resp_Desc_Header.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"));
                            Resp_Desc_Header.BorderColorRight = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                            Resp_Desc_Header.BorderColorBottom = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"));
                            table.AddCell(Resp_Desc_Header);

                            PdfPCell Counts_Header = new PdfPCell(new Phrase("Counts", BoldFont));
                            Counts_Header.HorizontalAlignment = Element.ALIGN_RIGHT;
                            Counts_Header.BorderWidthBottom = 0.5f;
                            Counts_Header.BorderWidthTop = 0f;
                            Counts_Header.BackgroundColor = SecondBlue;
                            Counts_Header.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                            Counts_Header.BorderColorRight = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"));
                            Counts_Header.BorderColorBottom = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"));
                            table.AddCell(Counts_Header);

                            #endregion

                            int RespTotals = 0;

                            foreach (DataRow dr in dt.Rows)
                            {
                                PdfPCell Resp_Desc = new PdfPCell(new Phrase(dr["MATQUESR_RESP_DESC"].ToString().Trim(), TableFont));
                                Resp_Desc.HorizontalAlignment = Element.ALIGN_LEFT;

                                Resp_Desc.BorderWidthBottom = 0.5f;
                                Resp_Desc.BorderWidthRight = 0.5f;

                                Resp_Desc.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"));
                                Resp_Desc.BorderColorRight = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                Resp_Desc.BorderColorTop = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                Resp_Desc.BorderColorBottom = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                table.AddCell(Resp_Desc);

                                PdfPCell Counts = new PdfPCell(new Phrase(dr["COUNTS"].ToString().Trim(), TableFont));
                                Counts.HorizontalAlignment = Element.ALIGN_RIGHT;

                                Counts.BorderWidthBottom = 0.5f;
                                Counts.BorderWidthLeft = 0.5f;

                                Counts.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                Counts.BorderColorRight = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"));
                                Counts.BorderColorTop = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                Counts.BorderColorBottom = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                table.AddCell(Counts);

                                RespTotals += Convert.ToInt32(dr["COUNTS"].ToString().Trim());
                            }

                            PdfPCell Totals = new PdfPCell(new Phrase("Total", BoldFont));
                            Totals.HorizontalAlignment = Element.ALIGN_LEFT;

                            Totals.BorderWidthBottom = 0.5f;
                            Totals.BorderWidthLeft = 0.5f;
                            Totals.BorderWidthTop = 0.5f;
                            Totals.BorderWidthRight = 0.5f;

                            Totals.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"));
                            Totals.BorderColorRight = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                            Totals.BorderColorTop = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"));
                            Totals.BorderColorBottom = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"));
                            table.AddCell(Totals);

                            Totals = new PdfPCell(new Phrase(RespTotals.ToString(), BoldFont));
                            Totals.HorizontalAlignment = Element.ALIGN_RIGHT;

                            Totals.BorderWidthBottom = 0.5f;
                            Totals.BorderWidthLeft = 0.5f;
                            Totals.BorderWidthTop = 0.5f;
                            Totals.BorderWidthRight = 0.5f;

                            Totals.BorderColorLeft = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                            Totals.BorderColorRight = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"));
                            Totals.BorderColorTop = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000")); 
                            Totals.BorderColorBottom = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"));
                            table.AddCell(Totals);
                        }
                        document.Add(table);
                    }

                    AlertBox.Show("Report Generated Successfully");
                }
                catch(Exception ex) { document.Add(new Paragraph("Aborted due to Exception!!!")); }

                document.Close();
                fs.Close();
                fs.Dispose();

                PdfViewerNewForm objfrm = new PdfViewerNewForm(PdfName);
                objfrm.StartPosition = FormStartPosition.CenterScreen;
                objfrm.ShowDialog();
            }
        }

        private void Excel_Report(object sender, FormClosedEventArgs e)
        {
            Random_Filename = null;
            PdfListForm form = sender as PdfListForm;

            if (form.DialogResult == DialogResult.OK)
            {
                #region FileNameBuild

                Random_Filename = null;
                string xlFileName = form.GetFileName(); 

                xlFileName = propReportPath + _baseForm.UserID + "\\" + xlFileName + "_Audit";

                try
                {
                    if (!Directory.Exists(propReportPath + _baseForm.UserID.Trim()))
                    {
                        DirectoryInfo di = Directory.CreateDirectory(propReportPath + _baseForm.UserID.Trim());
                    }
                }
                catch (Exception ex)
                {
                    CommonFunctions.MessageBoxDisplay("Error");
                }

                try
                {
                    string Tmpstr = xlFileName + ".xlsx";
                    if (File.Exists(Tmpstr))
                        File.Delete(Tmpstr);
                }
                catch (Exception ex)
                {
                    int length = 8;
                    string newFileName = System.Guid.NewGuid().ToString();
                    newFileName = newFileName.Replace("-", string.Empty);

                    Random_Filename = xlFileName + newFileName.Substring(0, length) + ".xlsx";
                }

                if (!string.IsNullOrEmpty(Random_Filename))
                    xlFileName = Random_Filename;
                else
                    xlFileName += ".xlsx";

                string _excelPath = xlFileName;

                #endregion

                try
                {
                    List<CommonEntity> COUNTY = CommonFunctions.AgyTabsFilterCode( _baseForm.BaseAgyTabsEntity, "00525",_baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, string.Empty);
                    DataTable dtAudit_Counts = dsCounts.Tables[1];

                    if (dtAudit_Counts.Rows.Count > 0)
                    {
                        using (DevExpress.Spreadsheet.Workbook wb = new DevExpress.Spreadsheet.Workbook())
                        {
                            DevExpress_Excel_Properties oDevExpress_Excel_Properties = new DevExpress_Excel_Properties();
                            oDevExpress_Excel_Properties.sxlbook = wb;
                            oDevExpress_Excel_Properties.sxlTitleFont = "calibri";
                            oDevExpress_Excel_Properties.sxlbodyFont = "calibri";

                            oDevExpress_Excel_Properties.getDevexpress_Excel_Properties();

                            #region Data Printing


                            #region Parameters Page

                            DevExpress.Spreadsheet.Worksheet paramSheet = wb.Worksheets[0];
                            paramSheet.Name = "Params";
                            paramSheet.ActiveView.TabColor = Color.ForestGreen;
                            paramSheet.ActiveView.ShowGridlines = false;
                            wb.Unit = DevExpress.Office.DocumentUnit.Point;

                            paramSheet.Columns[1].Width = 80;
                            paramSheet.Columns[2].Width = 80;
                            paramSheet.Columns[3].Width = 50;
                            paramSheet.Columns[4].Width = 80;
                            paramSheet.Columns[5].Width = 200;

                            int _Rowindex = 0;
                            int _Columnindex = 0;

                            string strAgy = Current_Hierarchy_DB.Split('-')[0];

                            AgencyControlEntity BAgyControlDetails = _model.ZipCodeAndAgency.GetAgencyControlFile(strAgy);

                            string ImgName = "";
                            if (_baseForm.BaseAgencyControlDetails.AgyShortName == "NEOCAA")
                            {
                                ImgName = "NEOCAA_" + strAgy + "_LOGO.png";
                            }
                            else
                                ImgName = _baseForm.BaseAgencyControlDetails.AgyShortName + "_00_LOGO.png";

                            _Rowindex = 1;
                            _Columnindex = 1;
                            paramSheet.Rows[_Rowindex][_Columnindex].Value = BAgyControlDetails.AgyName;
                            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.xTitleCellstyle;
                            oDevExpress_Excel_Properties.xlRowsMerge(paramSheet, _Rowindex, _Columnindex, 5);
                            _Rowindex++;

                            string imagesPath = "https://capsysdev.capsystems.com/images/PIPlogos/" + ImgName;
                            DevExpress.Spreadsheet.SpreadsheetImageSource imgsrc = DevExpress.Spreadsheet.SpreadsheetImageSource.FromUri(imagesPath, wb);
                            //DevExpress.Spreadsheet.Picture pic = paramSheet.Pictures.AddPicture(imgsrc, 200, 80, 630, 280);
                            DevExpress.Spreadsheet.Picture pic = paramSheet.Pictures.AddPicture(imgsrc, 20, 0, 120, 70);


                            AgencyControlEntity _agycntrldets = new AgencyControlEntity();
                            _agycntrldets = _baseForm.BaseAgencyControlDetails;

                            if (_baseForm.BaseAgencyControlDetails.AgyShortName == "NEOCAA")
                                _agycntrldets = BAgyControlDetails;
                            else
                                _agycntrldets = _baseForm.BaseAgencyControlDetails;

                            string street = _agycntrldets.Street == "" ? "" : (_agycntrldets.Street + ", ");
                            string city = _agycntrldets.City == "" ? "" : (_agycntrldets.City + ", ");
                            string state = _agycntrldets.State == "" ? "" : (_agycntrldets.State + ", ");
                            string zip1 = _agycntrldets.Zip1 == "" ? "" : _agycntrldets.Zip1.PadLeft(5, '0');

                            string strAddress = street + city + state + zip1;
                            paramSheet.Rows[_Rowindex][_Columnindex].Value = strAddress;
                            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlEMPTC;
                            oDevExpress_Excel_Properties.xlRowsMerge(paramSheet, _Rowindex, _Columnindex, 5);

                            _Rowindex++;
                            paramSheet.Rows[_Rowindex][_Columnindex].Value = "";
                            oDevExpress_Excel_Properties.xlRowsMerge(paramSheet, _Rowindex, _Columnindex, 5);

                            _Rowindex++;
                            paramSheet.Rows[_Rowindex][_Columnindex].Value = _privilegeEntity.PrivilegeName;
                            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.reportNameStyle;
                            oDevExpress_Excel_Properties.xlRowsMerge(paramSheet, _Rowindex, _Columnindex, 5);

                            _Rowindex++;
                            paramSheet.Rows[_Rowindex][_Columnindex].Value = "Report Parameters";
                            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.xTitleCellstyle2;
                            oDevExpress_Excel_Properties.xlRowsMerge(paramSheet, _Rowindex, _Columnindex, 5);

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
                                _Rowindex++;
                                paramSheet.Rows[_Rowindex][_Columnindex].Value = Txt_HieDesc.Text.Trim() + "   " + program_year;
                                paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.xsubTitleintakeCellstyle;
                                paramSheet.Rows[_Rowindex][_Columnindex].RowHeight = 25;
                                oDevExpress_Excel_Properties.xlRowsMerge(paramSheet, _Rowindex, _Columnindex, 5);
                            }
                            else
                            {
                                _Rowindex++;
                                paramSheet.Rows[_Rowindex][_Columnindex].Value = Txt_HieDesc.Text.Trim();
                                paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.xsubTitleintakeCellstyle;
                                paramSheet.Rows[_Rowindex][_Columnindex].RowHeight = 25;
                                oDevExpress_Excel_Properties.xlRowsMerge(paramSheet, _Rowindex, _Columnindex, 5);
                            }

                            _Rowindex++;
                            paramSheet.Rows[_Rowindex][_Columnindex].Value = "";
                            oDevExpress_Excel_Properties.xlRowsMerge(paramSheet, _Rowindex, _Columnindex, 5);

                            _Rowindex++;
                            _Columnindex = 1;

                            paramSheet.Rows[_Rowindex][_Columnindex].Value = lblMatrix.Text.Trim();
                            oDevExpress_Excel_Properties.xlRowsMerge(paramSheet, _Rowindex, _Columnindex, 2, oDevExpress_Excel_Properties.paramsCellStyle);
                            _Columnindex = _Columnindex + 2;

                            paramSheet.Rows[_Rowindex][_Columnindex].Value = ((Captain.Common.Utilities.ListItem)cmbMatrix.SelectedItem).Text.ToString();
                            oDevExpress_Excel_Properties.xlRowsMerge(paramSheet, _Rowindex, _Columnindex, 3, oDevExpress_Excel_Properties.gxlNLC);
                            _Columnindex = _Columnindex + 3;

                            _Rowindex++;
                            _Columnindex = 1;

                            paramSheet.Rows[_Rowindex][_Columnindex].Value = lblScale.Text;
                            oDevExpress_Excel_Properties.xlRowsMerge(paramSheet, _Rowindex, _Columnindex, 2, oDevExpress_Excel_Properties.paramsCellStyle);
                            _Columnindex = _Columnindex + 2;

                            paramSheet.Rows[_Rowindex][_Columnindex].Value = ((Captain.Common.Utilities.ListItem)cmbScale.SelectedItem).Text.ToString();
                            oDevExpress_Excel_Properties.xlRowsMerge(paramSheet, _Rowindex, _Columnindex, 3, oDevExpress_Excel_Properties.gxlNLC);
                            _Columnindex = _Columnindex + 3;

                            _Rowindex++;
                            _Columnindex = 1;

                            paramSheet.Rows[_Rowindex][_Columnindex].Value = lblDateRangeFrm.Text;
                            oDevExpress_Excel_Properties.xlRowsMerge(paramSheet, _Rowindex, _Columnindex, 2, oDevExpress_Excel_Properties.paramsCellStyle);
                            _Columnindex = _Columnindex + 2;

                            paramSheet.Rows[_Rowindex][_Columnindex].Value = dtpFrom.Text + "  To: " + dtpTo.Text;
                            oDevExpress_Excel_Properties.xlRowsMerge(paramSheet, _Rowindex, _Columnindex, 3, oDevExpress_Excel_Properties.gxlNLC);
                            _Columnindex = _Columnindex + 3;

                            _Rowindex++;
                            _Columnindex = 1;

                            paramSheet.Rows[_Rowindex][_Columnindex].Value = rdbCounty_All.Checked ? lblCounty.Text.Trim() : "Selected County(ies)";
                            oDevExpress_Excel_Properties.xlRowsMerge(paramSheet, _Rowindex, _Columnindex, 2, oDevExpress_Excel_Properties.paramsCellStyle);
                            _Columnindex = _Columnindex + 2;

                            paramSheet.Rows[_Rowindex][_Columnindex].Value = rdbCounty_All.Checked ? "All Counties" : Get_Sel_County_Desc();
                            oDevExpress_Excel_Properties.xlRowsMerge(paramSheet, _Rowindex, _Columnindex, 3, oDevExpress_Excel_Properties.gxlNLC);
                            _Columnindex = _Columnindex + 3;

                            paramSheet.Rows[_Rowindex][_Columnindex].Value = "";
                            _Columnindex = _Columnindex + 5;

                            _Rowindex = _Rowindex + 3;
                            _Columnindex = 5;
                            paramSheet.Rows[_Rowindex][_Columnindex].Value = "Generated By: " + _baseForm.UserID;
                            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlGenerate_lr;

                            _Rowindex++;
                            paramSheet.Rows[_Rowindex][_Columnindex].Value = "Generated On: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");
                            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlGenerate_lr;

                            paramSheet.Rows[_Rowindex].AutoFit();
                            paramSheet.IgnoredErrors.Add(paramSheet.GetDataRange(), DevExpress.Spreadsheet.IgnoredErrorType.NumberAsText);

                            #endregion

                            DevExpress.Spreadsheet.Worksheet SheetDetails = wb.Worksheets.Add("Audit Report");
                            SheetDetails.ActiveView.ShowGridlines = false;
                            wb.Unit = DevExpress.Office.DocumentUnit.Point;

                            #region Column Widths

                            SheetDetails.Columns[0].Width = 60;//Agency
                            SheetDetails.Columns[1].Width = 60;//Department
                            SheetDetails.Columns[2].Width = 60;//Program
                            SheetDetails.Columns[3].Width = 30;//Year
                            SheetDetails.Columns[4].Width = 50;//Applicant No
                            SheetDetails.Columns[5].Width = 150;//Applicant Name
                            SheetDetails.Columns[6].Width = 120;//Response Desc
                            SheetDetails.Columns[7].Width = 80;//County

                            #endregion

                            _Rowindex = 0;
                            _Columnindex = 0;

                            DataTable dtQues = new DataTable();
                            DataView dvQues = new DataView(dtAudit_Counts);
                            dvQues.Sort = "MATQUESR_CODE";
                            dtQues = dvQues.ToTable();

                            var distinctQues = (from DataRow dRow in dtQues.Rows select dRow["MATQUESR_CODE"]).Distinct();

                            int firstSpaces = 0;

                            foreach (var drRow in distinctQues)
                            {
                                DataTable dt = new DataTable();
                                DataView dv = new DataView(dtAudit_Counts);
                                dv.RowFilter = "MATQUESR_CODE='" + drRow + "'";
                                dv.Sort = "MATASMT_APP";
                                dt = dv.ToTable();

                                if (firstSpaces != 0)
                                    _Rowindex = _Rowindex + 2;
                                _Columnindex = 0;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = dt.Rows[0]["QUES_DESC"].ToString().Trim();
                                oDevExpress_Excel_Properties.xlRowsMerge(SheetDetails, _Rowindex, _Columnindex, 8, oDevExpress_Excel_Properties.gxlFrameStyleL);
                                SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 11;

                                _Rowindex++;
                                _Columnindex = 0;

                                #region Column Headers

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Agency";
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                                SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 11;
                                _Columnindex++;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Department";
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                                SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 11;
                                _Columnindex++;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Program";
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                                SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 11;
                                _Columnindex++;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Year";
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleC;
                                SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 11;
                                _Columnindex++;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "App#";
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                                SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 11;
                                _Columnindex++;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Applicant Name";
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                                SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 11;
                                _Columnindex++;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Response Description";
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                                SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 11;
                                _Columnindex++;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "County";
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlFrameBlockStyleL;
                                SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 11;

                                #endregion

                                #region Data Loop

                                foreach (DataRow dr in dt.Rows)
                                {
                                    _Rowindex++;
                                    _Columnindex = 0;

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["MATASMT_AGENCY"].ToString().Trim();
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                    _Columnindex++;

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["MATASMT_DEPT"].ToString().Trim();
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                    _Columnindex++;

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["MATASMT_PROGRAM"].ToString().Trim();
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                    _Columnindex++;

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["MATASMT_YEAR"].ToString().Trim();
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                    _Columnindex++;

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["MATASMT_APP"].ToString().Trim();
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                    _Columnindex++;

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["SNP_NAME_IX_LAST"].ToString().Trim() + ", " + dr["SNP_NAME_IX_FI"].ToString().Trim();
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                    _Columnindex++;

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = dr["MATQUESR_RESP_DESC"].ToString().Trim();
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                    _Columnindex++;

                                    string countyName = string.Empty;
                                    if (!string.IsNullOrEmpty(dr["MST_COUNTY"].ToString().Trim()))
                                    {
                                        countyName = COUNTY.Find(x => x.Code == dr["MST_COUNTY"].ToString().Trim()).Desc;
                                    }

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = countyName;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                }

                                SheetDetails.IgnoredErrors.Add(SheetDetails.GetDataRange(), DevExpress.Spreadsheet.IgnoredErrorType.NumberAsText);

                                firstSpaces++;

                                #endregion

                            }

                            #endregion

                            #region File Saving and Downloading

                            wb.Sheets.ActiveSheet = wb.Sheets[0];

                            wb.SaveDocument(_excelPath, DevExpress.Spreadsheet.DocumentFormat.OpenXml);

                            //try
                            //{
                            //    string localFilePath = xlFileName;

                            //    FileInfo fiDownload = new FileInfo(localFilePath);

                            //    string name = fiDownload.Name;
                            //    using (FileStream fileStream = fiDownload.OpenRead())
                            //    {
                            //        Application.Download(fileStream, name);
                            //    }
                            //}
                            //catch (Exception ex)
                            //{

                            //}

                            #endregion
                        }
                    }
                }
                catch (Exception ex) { }

            }

        }

        private void rdbCounty_Sel_Click(object sender, EventArgs e)
        {
            if (rdbCounty_Sel.Checked == true)
            {
                SelectZipSiteCountyForm countyform = new SelectZipSiteCountyForm(_baseForm, ListcommonEntity);
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
                if (form.FormType == "COUNTY")
                {
                    ListcommonEntity = form.SelectedCountyEntity;
                }
            }
        }

        private void rdbCounty_All_Click(object sender, EventArgs e)
        {
            ListcommonEntity.Clear();
        }
        private void Fill_County_Selected_List(string County_Str)
        {
            foreach (CommonEntity Ent in County_List)
            {
                if (County_Str.Contains(Ent.Code))
                    ListcommonEntity.Add(new CommonEntity(Ent));
            }
        }
        private string Get_Sel_County()
        {
            string Sel_County_Codes = null;
            foreach (CommonEntity Entity in ListcommonEntity)
            {
                Sel_County_Codes += Entity.Code + ",";
            }

            if (Sel_County_Codes.Length > 0)
                Sel_County_Codes = Sel_County_Codes.Substring(0, (Sel_County_Codes.Length - 1));

            return Sel_County_Codes;
        }
        private string Get_Sel_County_Desc()
        {
            string Sel_County_Desc = null;
            foreach (CommonEntity Entity in ListcommonEntity)
            {
                Sel_County_Desc += Entity.Desc + ",";
            }

            if (Sel_County_Desc.Length > 0)
                Sel_County_Desc = Sel_County_Desc.Substring(0, (Sel_County_Desc.Length - 1));

            return Sel_County_Desc;
        }
        private bool ValidateReport()
        {
            bool isValid = true;

            if (dtpFrom.Checked == false && dtpTo.Checked == false)
            {
                _errorProvider.SetError(dtpTo, string.Format("Please Select From and To Dates"));
                isValid = false;
            }
            else
                _errorProvider.SetError(dtpTo, null);

            if (dtpFrom.Checked == true || dtpTo.Checked == true)
            {
                if (dtpFrom.Checked == false && dtpTo.Checked == true)
                {
                    _errorProvider.SetError(dtpFrom, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "From Date".Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtpFrom, null);
                }
            }

            if (dtpFrom.Checked == true || dtpTo.Checked == true)
            {
                if (dtpTo.Checked == false && dtpFrom.Checked == true)
                {
                    _errorProvider.SetError(dtpTo, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "To Date".Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtpTo, null);
                }
            }
            if (dtpFrom.Checked.Equals(true) && dtpTo.Checked.Equals(true))
            {
                if (string.IsNullOrWhiteSpace(dtpFrom.Text))
                {
                    _errorProvider.SetError(dtpFrom, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "From Date".Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtpFrom, null);
                }
                if (string.IsNullOrWhiteSpace(dtpTo.Text))
                {
                    _errorProvider.SetError(dtpTo, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "To Date".Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtpTo, null);
                }
            }

            if (dtpFrom.Checked && dtpTo.Checked == true)
            {
                if (!string.IsNullOrWhiteSpace(dtpFrom.Text) && !string.IsNullOrWhiteSpace(dtpTo.Text))
                {
                    if (dtpFrom.Value.Date > dtpTo.Value.Date)
                    {
                        _errorProvider.SetError(dtpTo, "From Date should be prior or equal to To Date");
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(dtpTo, null);
                    }
                }
            }
            if (rdbCounty_Sel.Checked && ListcommonEntity.Count <= 0)
            {
                _errorProvider.SetError(rdbCounty_Sel, string.Format("Please Select at least One County".Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(rdbCounty_Sel, null);

            return isValid;
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            PdfListForm pdfListForm = new PdfListForm(_baseForm, _privilegeEntity, true, propReportPath);
            pdfListForm.StartPosition = FormStartPosition.CenterScreen;
            pdfListForm.ShowDialog();
        }
    }
}
