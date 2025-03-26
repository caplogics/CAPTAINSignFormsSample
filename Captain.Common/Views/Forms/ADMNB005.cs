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
using Captain.Common.Model.Objects;
using Captain.Common.Model.Data;
using CarlosAg.ExcelXmlWriter;
using System.IO;
using Captain.Common.Utilities;
using Captain.DatabaseLayer;
using System.Data.SqlClient;
using NPOI.SS.Formula.Functions;
using Captain.Common.Interfaces;
using iTextSharp.text.pdf;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class ADMNB005 : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        DataTable dtDSSXMLData = new DataTable();
        #endregion
        public ADMNB005(BaseForm baseform, PrivilegeEntity privileges)
        {
            _model = new CaptainModel();
            BaseForm = baseform;
            Privileges = privileges;
            InitializeComponent();

            propReportPath = _model.lookupDataAccess.GetReportPath();

            userHierarchy = _model.UserProfileAccess.GetUserHierarchyByID(BaseForm.UserID);
            if (userHierarchy.Count > 0)
                userHierarchy = userHierarchy.FindAll(u => u.Agency == "**" && u.Dept == "**" && u.Prog == "**" && u.UsedFlag == "N");

            fillcombo();
            this.Text = Privileges.PrivilegeName;

            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            //Added by Vikash on 09/28/2023 as a part of enhancement in "Brian work for DSS feed for week of sept 19.doc - 9/26/2023 - Image Types Report" point

            Set_Report_Hierarchy(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear);

            if (BaseForm.BusinessModuleID == "08" || BaseForm.BusinessModuleID == "03")
            {
                pnlHieFilter.Visible = true;
                //this.Size = new System.Drawing.Size(this.Width, this.Height);

                this.Size = new System.Drawing.Size(this.Width, this.Height);

                //if (lblAgy.Visible == true && cmbAgy.Visible == true)
                //    this.Size = new System.Drawing.Size(this.Width, this.Height);
                //else
                //    this.Size = new System.Drawing.Size(this.Width, 205);
            }
            else
            {
                pnlHieFilter.Visible = false;
                this.Size = new System.Drawing.Size(this.Width, this.Height - pnlHieFilter.Height);


                //this.Size = new System.Drawing.Size(600, this.Height - this.pnlHieFilter.Height);

                //if (lblAgy.Visible == true && cmbAgy.Visible == true)
                //    this.Size = new System.Drawing.Size(600, this.Height);
                //else
                //    this.Size = new System.Drawing.Size(600, 163);
            }

        }
        #region properties

        public List<HierarchyEntity> userHierarchy { get; set; }

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileges { get; set; }
        public string propReportPath { get; set; }

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

        #endregion

        private void BtnPdfPrev_Click(object sender, EventArgs e)
        {
            PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, true, propReportPath);
            pdfListForm.StartPosition = FormStartPosition.CenterScreen;
            pdfListForm.ShowDialog();
        }

        private void BtnGenFile_Click(object sender, EventArgs e)
        {

            if ((BaseForm.BusinessModuleID == "08" || BaseForm.BusinessModuleID == "03") && BaseForm.BaseAgencyControlDetails.State=="CT")  //Short Name logic added by Sudheer on 08/14/24 as per the IHCDA document
            {
                if (((ListItem)cmbReport.SelectedItem).Value.ToString() == "2")
                {
                    if (Validateform())
                    {
                        GenerateExcelfileUploadCaptain_CT();
                    }
                }
            }
            else
            { 
                if (((ListItem)cmbReport.SelectedItem).Value.ToString() == "2")
                {
                    if (Validateform())
                    {
                        GenerateExcelfileUploadCaptain();
                    }
                }
                else if (((ListItem)cmbReport.SelectedItem).Value.ToString() == "3")
                {
                    if (Validateform())
                    {
                        GenerateExcelfilePipupload();
                    }
                }
                else
                {
                    GenerateExcelFileforPIP();
                }
            }

        }

        private bool Validateform()
        {
            bool isValid = true;
            if (dtpFrmDte.Checked && !dtpToDte.Checked)
            {
                _errorProvider.SetError(dtpToDte, "Must select 'To Date'");
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtpToDte, null);
            }

            if (dtpToDte.Checked && !dtpFrmDte.Checked)
            {
                _errorProvider.SetError(dtpFrmDte, "Must select 'From Date'");
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtpFrmDte, null);
            }

            if (dtpFrmDte.Checked.Equals(true) && dtpToDte.Checked.Equals(true))
            {
                if (string.IsNullOrWhiteSpace(dtpFrmDte.Text))
                {
                    _errorProvider.SetError(dtpFrmDte, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "From Date".Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtpFrmDte, null);
                }
                if (string.IsNullOrWhiteSpace(dtpToDte.Text))
                {
                    _errorProvider.SetError(dtpToDte, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "To Date".Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtpToDte, null);
                }
            }

            if (dtpFrmDte.Checked && dtpToDte.Checked)
            {
                if (dtpFrmDte.Value.Date > dtpToDte.Value.Date)
                {
                    _errorProvider.SetError(dtpToDte, "End Date should be greater than Start Date");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtpToDte, null);
                }
            }

            return (isValid);
        }


        #region Excelfile
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
            //  s57
            // -----------------------------------------------
            WorksheetStyle s57 = styles.Add("s57");
            s57.Name = "Comma";
            s57.NumberFormat = "_ * #,##0.00_ ;_ * \\-#,##0.00_ ;_ * \"-\"??_ ;_ @_ ";
            // -----------------------------------------------
            //  s58
            // -----------------------------------------------
            WorksheetStyle s58 = styles.Add("s58");
            s58.Name = "Normal 2";
            s58.Font.FontName = "Arial";
            s58.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s78
            // -----------------------------------------------
            WorksheetStyle s78 = styles.Add("s78");
            s78.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s78.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s79
            // -----------------------------------------------
            WorksheetStyle s79 = styles.Add("s79");
            s79.Font.FontName = "Arial";
            s79.Font.Color = "#000000";
            s79.Interior.Color = "#FFFFFF";
            s79.Interior.Pattern = StyleInteriorPattern.Solid;
            s79.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s79.Alignment.Vertical = StyleVerticalAlignment.Center;
            s79.Alignment.WrapText = true;
            s79.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s79.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            s79.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            s79.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s80
            // -----------------------------------------------
            WorksheetStyle s80 = styles.Add("s80");
            s80.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            s80.Alignment.Vertical = StyleVerticalAlignment.Center;
            s80.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s80.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s80.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s80.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s81
            // -----------------------------------------------
            WorksheetStyle s81 = styles.Add("s81");
            s81.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s81.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s81.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s81.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s81.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s81.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s82
            // -----------------------------------------------
            WorksheetStyle s82 = styles.Add("s82");
            s82.Interior.Color = "#DEEBF6";
            s82.Interior.Pattern = StyleInteriorPattern.Solid;
            // -----------------------------------------------
            //  s83
            // -----------------------------------------------
            WorksheetStyle s83 = styles.Add("s83");
            s83.Interior.Color = "#DEEBF6";
            s83.Interior.Pattern = StyleInteriorPattern.Solid;
            s83.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s83.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s84
            // -----------------------------------------------
            WorksheetStyle s84 = styles.Add("s84");
            s84.Font.Bold = true;
            s84.Font.FontName = "Calibri";
            s84.Font.Color = "#000000";
            s84.Interior.Color = "#FBE4D5";
            s84.Interior.Pattern = StyleInteriorPattern.Solid;
            s84.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s84.Alignment.Vertical = StyleVerticalAlignment.Center;
            s84.Alignment.WrapText = true;
            s84.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s84.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            s84.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            s84.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            s84.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s85
            // -----------------------------------------------
            WorksheetStyle s85 = styles.Add("s85");
            s85.Font.Bold = true;
            s85.Font.FontName = "Calibri";
            s85.Font.Color = "#000000";
            s85.Interior.Color = "#FBE4D5";
            s85.Interior.Pattern = StyleInteriorPattern.Solid;
            s85.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            s85.Alignment.Vertical = StyleVerticalAlignment.Center;
            s85.Alignment.WrapText = true;
            s85.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s85.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s85.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s85.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s85.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s86
            // -----------------------------------------------
            WorksheetStyle s86 = styles.Add("s86");
            s86.Font.Bold = true;
            s86.Font.FontName = "Calibri";
            s86.Font.Color = "#000000";
            s86.Interior.Color = "#FBE4D5";
            s86.Interior.Pattern = StyleInteriorPattern.Solid;
            s86.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s86.Alignment.Vertical = StyleVerticalAlignment.Center;
            s86.Alignment.WrapText = true;
            s86.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s86.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s86.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s86.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s86.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s87
            // -----------------------------------------------
            WorksheetStyle s87 = styles.Add("s87");
            s87.Font.Bold = true;
            s87.Font.FontName = "Calibri";
            s87.Font.Color = "#000000";
            s87.Interior.Color = "#FBE4D5";
            s87.Interior.Pattern = StyleInteriorPattern.Solid;
            s87.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s87.Alignment.Vertical = StyleVerticalAlignment.Center;
            s87.Alignment.WrapText = true;
            s87.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s87.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s87.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s87.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s87.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s88
            // -----------------------------------------------
            WorksheetStyle s88 = styles.Add("s88");
            s88.Font.FontName = "Calibri";
            // -----------------------------------------------
            //  s89
            // -----------------------------------------------
            WorksheetStyle s89 = styles.Add("s89");
            s89.Font.FontName = "Arial";
            s89.Font.Color = "#000000";
            s89.Interior.Color = "#FFFFFF";
            s89.Interior.Pattern = StyleInteriorPattern.Solid;
            s89.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s89.Alignment.Vertical = StyleVerticalAlignment.Center;
            s89.Alignment.WrapText = true;
            s89.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s89.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            s89.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            s89.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s90
            // -----------------------------------------------
            WorksheetStyle s90 = styles.Add("s90");
            s90.Font.Bold = true;
            s90.Font.FontName = "Calibri";
            s90.Font.Color = "#000000";
            s90.Interior.Color = "#FBE4D5";
            s90.Interior.Pattern = StyleInteriorPattern.Solid;
            s90.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s90.Alignment.Vertical = StyleVerticalAlignment.Center;
            s90.Alignment.WrapText = true;
            s90.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s90.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            s90.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            s90.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s91
            // -----------------------------------------------
            WorksheetStyle s91 = styles.Add("s91");
            s91.Font.Bold = true;
            s91.Font.FontName = "Calibri Light";
            s91.Font.Size = 16;
            s91.Interior.Color = "#DEEBF6";
            s91.Interior.Pattern = StyleInteriorPattern.Solid;
            s91.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s91.Alignment.Vertical = StyleVerticalAlignment.Center;
            // -----------------------------------------------
            //  s92
            // -----------------------------------------------
            WorksheetStyle s92 = styles.Add("s92");
            s92.Parent = "s57";
            s92.Font.FontName = "Arial";
            s92.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s92.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s92.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s92.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            s92.NumberFormat = "_ * #,##0_ ;_ * \\-#,##0_ ;_ * \"-\"??_ ;_ @_ ";
            // -----------------------------------------------
            //  s93
            // -----------------------------------------------
            WorksheetStyle s93 = styles.Add("s93");
            s93.Font.Bold = true;
            s93.Font.FontName = "Calibri";
            s93.Font.Color = "#000000";
            s93.Interior.Color = "#FBE4D5";
            s93.Interior.Pattern = StyleInteriorPattern.Solid;
            s93.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s93.Alignment.Vertical = StyleVerticalAlignment.Center;
            s93.Alignment.WrapText = true;
            s93.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s93.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            s93.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            s93.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s94
            // -----------------------------------------------
            WorksheetStyle s94 = styles.Add("s94");
            s94.Font.Bold = true;
            s94.Font.FontName = "Calibri Light";
            s94.Font.Size = 16;
            s94.Interior.Color = "#DEEBF6";
            s94.Interior.Pattern = StyleInteriorPattern.Solid;
            s94.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s94.Alignment.Vertical = StyleVerticalAlignment.Center;
            // -----------------------------------------------
            //  s95
            // -----------------------------------------------
            WorksheetStyle s95 = styles.Add("s95");
            s95.Font.FontName = "Arial";
            s95.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s95.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s95.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s95.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s95.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s95.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s96
            // -----------------------------------------------
            WorksheetStyle s96 = styles.Add("s96");
            s96.Parent = "s58";
            s96.Font.FontName = "Arial";
            s96.Font.Color = "#000000";
            s96.Interior.Color = "#FFFFFF";
            s96.Interior.Pattern = StyleInteriorPattern.Solid;
            s96.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s96.Alignment.Vertical = StyleVerticalAlignment.Center;
            s96.Alignment.WrapText = true;
            s96.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s96.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            s96.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            s96.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            s96.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s97
            // -----------------------------------------------
            WorksheetStyle s97 = styles.Add("s97");
            s97.Parent = "s58";
            s97.Font.FontName = "Arial";
            s97.Font.Color = "#000000";
            s97.Interior.Color = "#FFFFFF";
            s97.Interior.Pattern = StyleInteriorPattern.Solid;
            s97.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s97.Alignment.Vertical = StyleVerticalAlignment.Center;
            s97.Alignment.WrapText = true;
            s97.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s97.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            s97.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            s97.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            s97.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s98
            // -----------------------------------------------
            WorksheetStyle s98 = styles.Add("s98");
            s98.Font.Bold = true;
            s98.Font.FontName = "Calibri";
            s98.Font.Color = "#000000";
            s98.Interior.Color = "#FBE4D5";
            s98.Interior.Pattern = StyleInteriorPattern.Solid;
            s98.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s98.Alignment.Vertical = StyleVerticalAlignment.Center;
            s98.Alignment.WrapText = true;
            s98.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            // -----------------------------------------------
            //  s99
            // -----------------------------------------------
            WorksheetStyle s99 = styles.Add("s99");
            s99.Font.Bold = true;
            s99.Font.FontName = "Calibri";
            s99.Font.Color = "#000000";
            s99.Interior.Color = "#FBE4D5";
            s99.Interior.Pattern = StyleInteriorPattern.Solid;
            s99.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s99.Alignment.Vertical = StyleVerticalAlignment.Center;
            s99.Alignment.WrapText = true;
            s99.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            // -----------------------------------------------
            //  s100
            // -----------------------------------------------
            WorksheetStyle s100 = styles.Add("s100");
            s100.Font.Bold = true;
            s100.Font.FontName = "Calibri";
            s100.Font.Color = "#000000";
            s100.Interior.Color = "#FBE4D5";
            s100.Interior.Pattern = StyleInteriorPattern.Solid;
            s100.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            s100.Alignment.Vertical = StyleVerticalAlignment.Center;
            s100.Alignment.WrapText = true;
            s100.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            // -----------------------------------------------
            //  s101
            // -----------------------------------------------
            WorksheetStyle s101 = styles.Add("s101");
            s101.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s101.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s102
            // -----------------------------------------------
            WorksheetStyle s102 = styles.Add("s102");
            s102.Font.FontName = "Arial";
            s102.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s102.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s103
            // -----------------------------------------------
            WorksheetStyle s103 = styles.Add("s103");
        }

        string Random_Filename = null; string PdfName = null;
        private void GenerateExcelFileforPIP()
        {


            PdfName = "IMG_TYPES_USAGE_" + BaseForm.UserID.ToString() + "_" + DateTime.Now.ToString("yyyyMMdd") + "_" + DateTime.Now.ToString("HHmmss");

            Random_Filename = null;

            string strFileName = string.Empty;

            strFileName = PdfName;

            strFileName = strFileName + ".xls";

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

            string strAgency = string.Empty;

            DataSet ds = SPAdminDB.ADMNB005_GET(strAgency, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

            DataTable dt = ds.Tables[0];

            List<ListItem> pipImageTypes = GetPipImageTypes();

            List<AgyTabEntity> _listIncomeTypes = _model.lookupDataAccess.GetIncomeTypes();



            Workbook book = new Workbook();

            this.GenerateStyles(book.Styles);

            Worksheet sheet; WorksheetCell cell;

            sheet = book.Worksheets.Add("Parameters");
            sheet.Table.DefaultRowHeight = 13.2F;
            //sheet.Table.ExpandedColumnCount = 10;
            //sheet.Table.ExpandedRowCount = 45;
            sheet.Table.FullColumns = 1;
            sheet.Table.FullRows = 1;
            // -----------------------------------------------
            WorksheetRow Row0 = sheet.Table.Rows.Add();
            Row0.Height = 21;
            cell = Row0.Cells.Add();
            cell.StyleID = "s91";
            cell = Row0.Cells.Add();
            cell.StyleID = "s94";
            cell = Row0.Cells.Add();
            cell.StyleID = "s94";
            cell = Row0.Cells.Add();
            cell.StyleID = "s82";
            cell = Row0.Cells.Add();
            cell.StyleID = "s82";
            cell = Row0.Cells.Add();
            cell.StyleID = "s82";
            cell = Row0.Cells.Add();
            cell.StyleID = "s82";
            cell = Row0.Cells.Add();
            cell.StyleID = "s82";
            cell = Row0.Cells.Add();
            cell.StyleID = "s83";
            cell = Row0.Cells.Add();
            cell.StyleID = "s82";
            // -----------------------------------------------
            WorksheetRow Row1 = sheet.Table.Rows.Add();
            Row1.Height = 18;
            Row1.AutoFitHeight = false;
            cell = Row1.Cells.Add();
            cell.StyleID = "s98";
            cell = Row1.Cells.Add();
            cell.StyleID = "s98";
            cell = Row1.Cells.Add();
            cell.StyleID = "s98";
            cell = Row1.Cells.Add();
            cell.StyleID = "s99";
            cell = Row1.Cells.Add();
            cell.StyleID = "s100";
            cell = Row1.Cells.Add();
            cell.StyleID = "s100";
            cell = Row1.Cells.Add();
            cell.StyleID = "s100";
            cell = Row1.Cells.Add();
            cell.StyleID = "s100";
            cell = Row1.Cells.Add();
            cell.StyleID = "s98";
            cell = Row1.Cells.Add();
            cell.StyleID = "s99";
            // -----------------------------------------------
            WorksheetRow Row2 = sheet.Table.Rows.Add();
            Row2.Height = 18;
            Row2.AutoFitHeight = false;
            cell = Row2.Cells.Add();
            cell.StyleID = "s101";
            cell = Row2.Cells.Add();
            cell.StyleID = "s102";
            cell = Row2.Cells.Add();
            cell.StyleID = "s101";
            cell = Row2.Cells.Add();
            cell.StyleID = "s101";
            cell = Row2.Cells.Add();
            cell.StyleID = "s101";
            cell = Row2.Cells.Add();
            cell.StyleID = "s103";
            cell = Row2.Cells.Add();
            cell.StyleID = "s103";
            cell = Row2.Cells.Add();
            cell.StyleID = "s103";
            cell = Row2.Cells.Add();
            cell.StyleID = "s101";
            cell = Row2.Cells.Add();
            cell.StyleID = "s103";


            sheet = book.Worksheets.Add("Details");


            sheet.Table.DefaultRowHeight = 14.25F;
            sheet.Table.DefaultColumnWidth = 50.4F;
            //sheet.Table.ExpandedColumnCount = 11;
            //sheet.Table.ExpandedRowCount = 33;
            //sheet.Table.FullColumns = 1;
            //sheet.Table.FullRows = 1;
            WorksheetColumn column0 = sheet.Table.Columns.Add();
            column0.Width = 47;
            column0.StyleID = "s78";
            column0.Span = 2;
            WorksheetColumn column1 = sheet.Table.Columns.Add();
            column1.Index = 4;
            column1.Width = 220;
            WorksheetColumn column2 = sheet.Table.Columns.Add();
            column2.Width = 63;
            column2.Span = 3;
            WorksheetColumn column3 = sheet.Table.Columns.Add();
            column3.Index = 9;
            column3.Width = 63;
            column3.StyleID = "s78";
            sheet.Table.Columns.Add(436);
            sheet.Table.Columns.Add(274);
            // -----------------------------------------------
            Row0 = sheet.Table.Rows.Add();
            Row0.Height = 23;
            Row0.AutoFitHeight = false;
            Row0.Cells.Add("Image Types Usage in CAPTAIN and PIP", DataType.String, "s91");

            cell = Row0.Cells.Add();
            cell.StyleID = "s94";
            cell = Row0.Cells.Add();
            cell.StyleID = "s94";
            cell = Row0.Cells.Add();
            cell.StyleID = "s82";
            cell = Row0.Cells.Add();
            cell.StyleID = "s82";
            cell = Row0.Cells.Add();
            cell.StyleID = "s82";
            cell = Row0.Cells.Add();
            cell.StyleID = "s82";
            cell = Row0.Cells.Add();
            cell.StyleID = "s82";
            cell = Row0.Cells.Add();
            cell.StyleID = "s83";
            cell = Row0.Cells.Add();
            cell.StyleID = "s82";
            cell = Row0.Cells.Add();
            cell.StyleID = "s82";
            // -----------------------------------------------
            Row1 = sheet.Table.Rows.Add();
            Row1.Height = 19;
            Row1.AutoFitHeight = false;
            Row1.Cells.Add("Sno", DataType.String, "s90");
            Row1.Cells.Add("Security", DataType.String, "s90");
            Row1.Cells.Add("Code", DataType.String, "s84");
            Row1.Cells.Add("Image Type", DataType.String, "s93");
            Row1.Cells.Add("Total Images", DataType.String, "s85");
            Row1.Cells.Add("last 30 days", DataType.String, "s85");
            Row1.Cells.Add("last 6 months", DataType.String, "s85");
            Row1.Cells.Add("Last 1 Year", DataType.String, "s85");
            Row1.Cells.Add("Internal/PIP", DataType.String, "s86");
            Row1.Cells.Add("Long description of image type for PIP", DataType.String, "s87");
            Row1.Cells.Add("Income Types", DataType.String, "s87");
            // -----------------------------------------------
            int introws = 0;

            foreach (DataRow dritem in dt.Rows)
            {

                introws = introws + 1;


                Row2 = sheet.Table.Rows.Add();
                Row2.Height = 16;
                Row2.AutoFitHeight = false;
                Row2.AutoFitHeight = false;
                Row2.Cells.Add(introws.ToString(), DataType.Number, "s81");
                Row2.Cells.Add(dritem["Security"].ToString(), DataType.String, "s95");
                Row2.Cells.Add(dritem["AGYS_CODE"].ToString(), DataType.String, "s89");
                Row2.Cells.Add(dritem["AGYS_DESC"].ToString(), DataType.String, "s79");

                Row2.Cells.Add(dritem["TotalImages"].ToString() == "0" ? "" : dritem["TotalImages"].ToString(), DataType.String, "s80");
                Row2.Cells.Add(dritem["Month1"].ToString() == "0" ? "" : dritem["Month1"].ToString(), DataType.String, "s80");
                Row2.Cells.Add(dritem["Month6"].ToString() == "0" ? "" : dritem["Month6"].ToString(), DataType.String, "s80");
                Row2.Cells.Add(dritem["Lastyear"].ToString() == "0" ? "" : dritem["Lastyear"].ToString(), DataType.String, "s80");
                ListItem listpipimg = pipImageTypes.Find(u => u.Value.ToString().Trim() == dritem["AGYS_CODE"].ToString().Trim());
                if (listpipimg != null)
                {
                    Row2.Cells.Add("PIP", DataType.String, "s81");
                    Row2.Cells.Add(listpipimg.ID, DataType.String, "s79");
                    Row2.Cells.Add(fillIncomesTypesDesc(_listIncomeTypes, listpipimg.ScreenCode), DataType.String, "s79");
                }
                else
                {
                    Row2.Cells.Add("", DataType.String, "s81");
                    Row2.Cells.Add("", DataType.String, "s79");
                    Row2.Cells.Add("", DataType.String, "s79");


                }

            }

            sheet = book.Worksheets.Add("IncomeTypes");
            sheet.Table.DefaultRowHeight = 14.25F;
            sheet.Table.DefaultColumnWidth = 50.4F;
            sheet.Table.FullColumns = 1;
            sheet.Table.FullRows = 1;
            column0 = sheet.Table.Columns.Add();
            column0.Width = 47;
            column0.StyleID = "s78";
            column0.Span = 1;
            column1 = sheet.Table.Columns.Add();
            column1.Index = 3;
            column1.Width = 268;
            // -----------------------------------------------
            Row0 = sheet.Table.Rows.Add();
            Row0.Height = 23;
            Row0.AutoFitHeight = false;
            Row0.Cells.Add("Income Types in CAPTAIN", DataType.String, "s91");
            cell = Row0.Cells.Add();
            cell.StyleID = "s94";
            cell = Row0.Cells.Add();
            cell.StyleID = "s82";
            // -----------------------------------------------
            Row1 = sheet.Table.Rows.Add();
            Row1.Height = 19;
            Row1.AutoFitHeight = false;
            Row1.Cells.Add("Sno", DataType.String, "s86");
            Row1.Cells.Add("Code", DataType.String, "s86");
            Row1.Cells.Add("Image Type", DataType.String, "s87");
            // -----------------------------------------------
            introws = 0;
            foreach (AgyTabEntity item in _listIncomeTypes)
            {

                introws = introws + 1;
                Row2 = sheet.Table.Rows.Add();
                Row2.Height = 18;
                Row2.AutoFitHeight = false;
                Row2.Cells.Add(introws.ToString(), DataType.Number, "s81");
                Row2.Cells.Add(item.agycode, DataType.String, "s96");
                Row2.Cells.Add(item.agydesc, DataType.String, "s97");
            }
            FileStream stream = new FileStream(PdfName, FileMode.Create);

            book.Save(stream);
            stream.Close();

            AlertBox.Show(strFileName + " File Generated Successfully");

            //FileDownloadGateway downloadGateway = new FileDownloadGateway();
            //downloadGateway.Filename = strFileName;

            //downloadGateway.SetContentType(DownloadContentType.OctetStream);

            //downloadGateway.StartFileDownload(new ContainerControl(), PdfName);


        }
        List<ListItem> GetPipImageTypes()
        {

            List<ListItem> listpipimageTypes = new List<ListItem>();
            try
            {
                string strselectImgAgency = "00";
                if (BaseForm.BaseAgencyControlDetails.PIPSwitch == "I")
                {
                    if (cmbAgy.Items.Count > 0)
                    {
                        strselectImgAgency = ((ListItem)cmbAgy.SelectedItem).Value.ToString();
                    }
                }

                SqlConnection con = new SqlConnection(BaseForm.BaseLeanDataBaseConnectionString);

                con.Open();

                using (SqlCommand cmd = new SqlCommand("SELECT * FROM PIPIMGTYPES WHERE PIPIMGT_AGENCY ='" + BaseForm.BaseAgencyControlDetails.AgyShortName + "' AND PIPIMGT_AGY ='" + strselectImgAgency + "'", con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drrow in ds.Tables[0].Rows)
                        {
                            listpipimageTypes.Add(new ListItem(drrow["PIPIMGT_ID"].ToString(), drrow["PIPIMGT_IMG_TYPE"].ToString(), drrow["PIPIMGT_DESCRIPTION"].ToString(), drrow["PIPIMGT_AMH"].ToString(), drrow["PIPIMGT_IncTypes"].ToString(), drrow["PIPIMGT_AGENCY"].ToString(), drrow["PIPIMGT_REQUIRED"].ToString(), string.Empty));
                        }
                    }
                }

                con.Close();

            }
            catch (Exception ex)
            {


            }
            return listpipimageTypes;

        }

        public DataSet dsPIPAgencies { get; set; }
        void fillcombo()
        {
            if (BaseForm.BaseAgencyControlDetails.PIPSwitch != "N" && BaseForm.BaseAgencyControlDetails.PIPSwitch != string.Empty)
            {
                cmbReport.Items.Insert(0, new ListItem("Image Types for PIP", "1"));
                cmbReport.Items.Insert(1, new ListItem("Images Uploaded in CAPTAIN", "2"));
                cmbReport.Items.Insert(2, new ListItem("Images Uploaded in PIP", "3"));
                cmbReport.SelectedIndex = 0;
            }
            else
            {
                cmbReport.Items.Insert(0, new ListItem("Images Uploaded in CAPTAIN", "2"));
                cmbReport.SelectedIndex = 0;
            }
            if (BaseForm.BaseAgencyControlDetails.PIPSwitch == "I")
            {
                pnlAgy.Visible = true;

                lblAgy.Visible = true;

                cmbAgy.Visible = true;

                DataSet dsEmail = new DataSet();

                DataSet ds = Captain.DatabaseLayer.MainMenu.GetGlobalHierarchies_Latest(BaseForm.UserID, "1", " ", " ", " ");  // Verify it Once
                dsPIPAgencies = ds;
                //DataSet dsHierches = Captain.DatabaseLayer.MainMenu.GetGlobalHierarchies("1", string.Empty, " ");
                if (dsPIPAgencies.Tables.Count > 0)
                {
                    foreach (DataRow drhiechy in dsPIPAgencies.Tables[0].Rows)
                    {
                        cmbAgy.Items.Add(new ListItem(drhiechy["Agy"].ToString().Trim() + " - " + drhiechy["Name"].ToString(), drhiechy["Agy"].ToString()));
                    }
                }
                if (userHierarchy.Count > 0)
                {

                    cmbAgy.Items.Insert(0, new ListItem("** - All Agencies", "00"));


                }

                cmbAgy.SelectedIndex = 0;



            }

        }

        string fillIncomesTypesDesc(List<AgyTabEntity> lookUpIncomeTypes, string strIncomeTypes)
        {
            string strDesc = string.Empty;

            if (strIncomeTypes.Trim() != string.Empty)
            {
                string response = strIncomeTypes;
                string[] arrResponse = null;
                if (response.IndexOf(',') > 0)
                {
                    arrResponse = response.Split(',');
                }
                else if (!response.Equals(string.Empty))
                {
                    arrResponse = new string[] { response };
                }


                foreach (AgyTabEntity agyEntity in lookUpIncomeTypes)
                {

                    string resDesc = agyEntity.agycode.ToString().Trim();


                    if (arrResponse != null && arrResponse.ToList().Exists(u => u.Equals(resDesc)))
                    {
                        strDesc = strDesc + agyEntity.agydesc + ",";
                    }

                }
            }
            return strDesc;

        }

        private void GenerateStylesUpload(WorksheetStyleCollection styles)
        {
            // -----------------------------------------------
            //  Default
            // -----------------------------------------------
            WorksheetStyle Default = styles.Add("Default");
            Default.Name = "Normal";
            Default.Font.FontName = "Arial";
            Default.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s77
            // -----------------------------------------------
            WorksheetStyle s77 = styles.Add("s77");
            s77.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s77.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s77.Alignment.WrapText = true;
            // -----------------------------------------------
            //  s78
            // -----------------------------------------------
            WorksheetStyle s78 = styles.Add("s78");
            s78.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s78.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s78.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s78.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s79
            // -----------------------------------------------
            WorksheetStyle s79 = styles.Add("s79");
            s79.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s79.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s79.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s79.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s79.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s79.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s80
            // -----------------------------------------------
            WorksheetStyle s80 = styles.Add("s80");
            s80.Interior.Color = "#DEEBF6";
            s80.Interior.Pattern = StyleInteriorPattern.Solid;
            // -----------------------------------------------
            //  s81
            // -----------------------------------------------
            WorksheetStyle s81 = styles.Add("s81");
            s81.Interior.Color = "#DEEBF6";
            s81.Interior.Pattern = StyleInteriorPattern.Solid;
            s81.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s81.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s82
            // -----------------------------------------------
            WorksheetStyle s82 = styles.Add("s82");
            s82.Font.FontName = "Arial";
            s82.Font.Color = "#000000";
            s82.Interior.Color = "#FFFFFF";
            s82.Interior.Pattern = StyleInteriorPattern.Solid;
            s82.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s82.Alignment.Vertical = StyleVerticalAlignment.Center;
            s82.Alignment.WrapText = true;
            s82.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s82.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            s82.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            s82.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s83
            // -----------------------------------------------
            WorksheetStyle s83 = styles.Add("s83");
            s83.Font.Bold = true;
            s83.Font.FontName = "Calibri Light";
            s83.Font.Size = 16;
            s83.Interior.Color = "#DEEBF6";
            s83.Interior.Pattern = StyleInteriorPattern.Solid;
            s83.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s83.Alignment.Vertical = StyleVerticalAlignment.Center;
            // -----------------------------------------------
            //  s84
            // -----------------------------------------------
            WorksheetStyle s84 = styles.Add("s84");
            s84.Font.Bold = true;
            s84.Font.FontName = "Calibri Light";
            s84.Font.Size = 16;
            s84.Interior.Color = "#DEEBF6";
            s84.Interior.Pattern = StyleInteriorPattern.Solid;
            s84.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s84.Alignment.Vertical = StyleVerticalAlignment.Center;
            // -----------------------------------------------
            //  s85
            // -----------------------------------------------
            WorksheetStyle s85 = styles.Add("s85");
            s85.Font.FontName = "Arial";
            s85.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s85.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s85.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s85.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s85.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s85.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s86
            // -----------------------------------------------
            WorksheetStyle s86 = styles.Add("s86");
            s86.Font.Bold = true;
            s86.Font.FontName = "Calibri";
            s86.Font.Color = "#000000";
            s86.Interior.Color = "#FBE4D5";
            s86.Interior.Pattern = StyleInteriorPattern.Solid;
            s86.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s86.Alignment.Vertical = StyleVerticalAlignment.Center;
            s86.Alignment.WrapText = true;
            s86.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            // -----------------------------------------------
            //  s87
            // -----------------------------------------------
            WorksheetStyle s87 = styles.Add("s87");
            s87.Font.Bold = true;
            s87.Font.FontName = "Calibri";
            s87.Font.Color = "#000000";
            s87.Interior.Color = "#FBE4D5";
            s87.Interior.Pattern = StyleInteriorPattern.Solid;
            s87.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s87.Alignment.Vertical = StyleVerticalAlignment.Center;
            s87.Alignment.WrapText = true;
            s87.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            // -----------------------------------------------
            //  s88
            // -----------------------------------------------
            WorksheetStyle s88 = styles.Add("s88");
            s88.Font.Bold = true;
            s88.Font.FontName = "Calibri";
            s88.Font.Color = "#000000";
            s88.Interior.Color = "#FBE4D5";
            s88.Interior.Pattern = StyleInteriorPattern.Solid;
            s88.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            s88.Alignment.Vertical = StyleVerticalAlignment.Center;
            s88.Alignment.WrapText = true;
            s88.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            // -----------------------------------------------
            //  s89
            // -----------------------------------------------
            WorksheetStyle s89 = styles.Add("s89");
            s89.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s89.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s90
            // -----------------------------------------------
            WorksheetStyle s90 = styles.Add("s90");
            s90.Font.FontName = "Arial";
            s90.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s90.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s91
            // -----------------------------------------------
            WorksheetStyle s91 = styles.Add("s91");
            // -----------------------------------------------
            //  s92
            // -----------------------------------------------
            WorksheetStyle s92 = styles.Add("s92");
            s92.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s92.Alignment.Vertical = StyleVerticalAlignment.Center;
            s92.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s92.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s92.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s92.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s93
            // -----------------------------------------------
            WorksheetStyle s93 = styles.Add("s93");
            s93.Font.FontName = "Arial";
            s93.Font.Color = "#000000";
            s93.Interior.Color = "#FFFFFF";
            s93.Interior.Pattern = StyleInteriorPattern.Solid;
            s93.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s93.Alignment.Vertical = StyleVerticalAlignment.Center;
            s93.Alignment.WrapText = true;
            s93.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s93.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s93.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s93.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s93.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s95
            // -----------------------------------------------
            WorksheetStyle s95 = styles.Add("s95");
            s95.Font.FontName = "Arial";
            s95.Font.Color = "#000000";
            s95.Interior.Color = "#FFFFFF";
            s95.Interior.Pattern = StyleInteriorPattern.Solid;
            s95.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s95.Alignment.Vertical = StyleVerticalAlignment.Center;
            s95.Alignment.WrapText = true;
            s95.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s95.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            s95.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            s95.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s96
            // -----------------------------------------------
            WorksheetStyle s96 = styles.Add("s96");
            s96.Interior.Color = "#DEEBF6";
            s96.Interior.Pattern = StyleInteriorPattern.Solid;
            s96.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s96.Alignment.Vertical = StyleVerticalAlignment.Center;
            // -----------------------------------------------
            //  s97
            // -----------------------------------------------
            WorksheetStyle s97 = styles.Add("s97");
            s97.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s97.Alignment.Vertical = StyleVerticalAlignment.Center;
            s97.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s97.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s97.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s97.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            s97.NumberFormat = "General Date";
            // -----------------------------------------------
            //  s98
            // -----------------------------------------------
            WorksheetStyle s98 = styles.Add("s98");
            s98.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s98.Alignment.Vertical = StyleVerticalAlignment.Center;
            // -----------------------------------------------
            //  s99
            // -----------------------------------------------
            WorksheetStyle s99 = styles.Add("s99");
            s99.Font.Bold = true;
            s99.Font.FontName = "Calibri";
            s99.Font.Size = 11;
            s99.Font.Color = "#000000";
            s99.Interior.Color = "#FBE4D5";
            s99.Interior.Pattern = StyleInteriorPattern.Solid;
            s99.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s99.Alignment.Vertical = StyleVerticalAlignment.Center;
            s99.Alignment.WrapText = true;
            s99.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s99.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            s99.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            s99.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s100
            // -----------------------------------------------
            WorksheetStyle s100 = styles.Add("s100");
            s100.Font.Bold = true;
            s100.Font.FontName = "Calibri";
            s100.Font.Size = 11;
            s100.Font.Color = "#000000";
            s100.Interior.Color = "#FBE4D5";
            s100.Interior.Pattern = StyleInteriorPattern.Solid;
            s100.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s100.Alignment.Vertical = StyleVerticalAlignment.Center;
            s100.Alignment.WrapText = true;
            s100.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s100.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            s100.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            s100.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s101
            // -----------------------------------------------
            WorksheetStyle s101 = styles.Add("s101");
            s101.Font.Bold = true;
            s101.Font.FontName = "Calibri";
            s101.Font.Size = 11;
            s101.Font.Color = "#000000";
            s101.Interior.Color = "#FBE4D5";
            s101.Interior.Pattern = StyleInteriorPattern.Solid;
            s101.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s101.Alignment.Vertical = StyleVerticalAlignment.Center;
            s101.Alignment.WrapText = true;
            s101.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s101.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            s101.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            s101.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            s101.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s102
            // -----------------------------------------------
            WorksheetStyle s102 = styles.Add("s102");
            s102.Font.Bold = true;
            s102.Font.FontName = "Calibri";
            s102.Font.Size = 11;
            s102.Font.Color = "#000000";
            s102.Interior.Color = "#FBE4D5";
            s102.Interior.Pattern = StyleInteriorPattern.Solid;
            s102.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s102.Alignment.Vertical = StyleVerticalAlignment.Center;
            s102.Alignment.WrapText = true;
            s102.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s102.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            s102.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            s102.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s103
            // -----------------------------------------------
            WorksheetStyle s103 = styles.Add("s103");
            s103.Font.Bold = true;
            s103.Font.FontName = "Calibri";
            s103.Font.Size = 11;
            s103.Font.Color = "#000000";
            s103.Interior.Color = "#FBE4D5";
            s103.Interior.Pattern = StyleInteriorPattern.Solid;
            s103.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s103.Alignment.Vertical = StyleVerticalAlignment.Center;
            s103.Alignment.WrapText = true;
            s103.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s103.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s103.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s103.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s103.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s104
            // -----------------------------------------------
            WorksheetStyle s104 = styles.Add("s104");
            s104.Font.Bold = true;
            s104.Font.FontName = "Calibri";
            s104.Font.Size = 11;
            s104.Font.Color = "#000000";
            s104.Interior.Color = "#FBE4D5";
            s104.Interior.Pattern = StyleInteriorPattern.Solid;
            s104.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s104.Alignment.Vertical = StyleVerticalAlignment.Center;
            s104.Alignment.WrapText = true;
            s104.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s104.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s104.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s104.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s104.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s105
            // -----------------------------------------------
            WorksheetStyle s105 = styles.Add("s105");
            s105.Font.Bold = true;
            s105.Font.FontName = "Calibri";
            s105.Font.Size = 11;
            // -----------------------------------------------
            //  s106
            // -----------------------------------------------
            WorksheetStyle s106 = styles.Add("s106");
            s106.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s106.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s106.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s106.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s106.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s106.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            s106.NumberFormat = "Short Date";

        }

        private void GenerateExcelfileUploadCaptain()
        {

            PdfName = "IMGS_UPLOADED_CAP_" + BaseForm.UserID.ToString() + "_" + DateTime.Now.ToString("yyyyMMdd") + "_" + DateTime.Now.ToString("HHmmss");

            Random_Filename = null;

            string strFileName = string.Empty;

            strFileName = PdfName;

            strFileName = strFileName + ".xls";

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



            string strselectImgAgency = string.Empty;
            if (BaseForm.BaseAgencyControlDetails.PIPSwitch == "I")
            {
                if (cmbAgy.Items.Count > 0)
                {
                    strselectImgAgency = ((ListItem)cmbAgy.SelectedItem).Value.ToString();
                }
                if (strselectImgAgency == "00")
                    strselectImgAgency = string.Empty;
            }

            DataSet ds = SPAdminDB.ADMNB005_GET(strselectImgAgency, dtpFrmDte.Value.ToString(), dtpToDte.Value.ToString(), "UPDCAPTAIN", string.Empty, string.Empty, string.Empty);

            DataTable dt = ds.Tables[0];




            Workbook book = new Workbook();

            this.GenerateStylesUpload(book.Styles);

            Worksheet sheet; WorksheetCell cell;

            //sheet = book.Worksheets.Add("Parameters");
            //sheet.Table.DefaultRowHeight = 13.2F;
            //sheet.Table.ExpandedColumnCount = 10;
            //sheet.Table.ExpandedRowCount = 45;
            //sheet.Table.FullColumns = 1;
            //sheet.Table.FullRows = 1;
            //// -----------------------------------------------
            WorksheetRow Row0;//= sheet.Table.Rows.Add();
            //Row0.Height = 21;

            //cell = Row0.Cells.Add();
            //cell.StyleID = "s83";
            //cell = Row0.Cells.Add();
            //cell.StyleID = "s84";
            //cell = Row0.Cells.Add();
            //cell.StyleID = "s84";
            //cell = Row0.Cells.Add();
            //cell.StyleID = "s80";
            //cell = Row0.Cells.Add();
            //cell.StyleID = "s80";
            //cell = Row0.Cells.Add();
            //cell.StyleID = "s80";
            //cell = Row0.Cells.Add();
            //cell.StyleID = "s80";
            //cell = Row0.Cells.Add();
            //cell.StyleID = "s80";
            //cell = Row0.Cells.Add();
            //cell.StyleID = "s81";
            //cell = Row0.Cells.Add();
            //cell.StyleID = "s80";
            //// -----------------------------------------------
            WorksheetRow Row1;// = sheet.Table.Rows.Add();
            //Row1.Height = 18;
            //Row1.AutoFitHeight = false;
            //cell = Row1.Cells.Add();
            //cell.StyleID = "s86";
            //cell = Row1.Cells.Add();
            //cell.StyleID = "s86";
            //cell = Row1.Cells.Add();
            //cell.StyleID = "s86";
            //cell = Row1.Cells.Add();
            //cell.StyleID = "s87";
            //cell = Row1.Cells.Add();
            //cell.StyleID = "s88";
            //cell = Row1.Cells.Add();
            //cell.StyleID = "s88";
            //cell = Row1.Cells.Add();
            //cell.StyleID = "s88";
            //cell = Row1.Cells.Add();
            //cell.StyleID = "s88";
            //cell = Row1.Cells.Add();
            //cell.StyleID = "s86";
            //cell = Row1.Cells.Add();
            //cell.StyleID = "s87";
            //// -----------------------------------------------
            //WorksheetRow Row2 = sheet.Table.Rows.Add();
            //Row2.Height = 18;
            //Row2.AutoFitHeight = false;
            //cell = Row2.Cells.Add();
            //cell.StyleID = "s89";
            //cell = Row2.Cells.Add();
            //cell.StyleID = "s90";
            //cell = Row2.Cells.Add();
            //cell.StyleID = "s89";
            //cell = Row2.Cells.Add();
            //cell.StyleID = "s89";
            //cell = Row2.Cells.Add();
            //cell.StyleID = "s89";
            //cell = Row2.Cells.Add();
            //cell.StyleID = "s91";
            //cell = Row2.Cells.Add();
            //cell.StyleID = "s91";
            //cell = Row2.Cells.Add();
            //cell.StyleID = "s91";
            //cell = Row2.Cells.Add();
            //cell.StyleID = "s89";
            //cell = Row2.Cells.Add();
            //cell.StyleID = "s91";



            sheet = book.Worksheets.Add("Details");
            sheet.Table.DefaultRowHeight = 14.25F;
            sheet.Table.DefaultColumnWidth = 50.4F;
            sheet.Table.FullColumns = 1;
            sheet.Table.FullRows = 1;
            WorksheetColumn column0 = sheet.Table.Columns.Add();
            column0.Width = 43;
            column0.StyleID = "s77";
            WorksheetColumn column1 = sheet.Table.Columns.Add();
            column1.Width = 115;
            column1.StyleID = "s77";
            WorksheetColumn column2 = sheet.Table.Columns.Add();
            column2.Width = 70;
            column2.StyleID = "s77";
            WorksheetColumn column3 = sheet.Table.Columns.Add();
            column3.Width = 61;
            column3.StyleID = "s77";
            sheet.Table.Columns.Add(142);
            sheet.Table.Columns.Add(175);
            sheet.Table.Columns.Add(112);
            WorksheetColumn column7 = sheet.Table.Columns.Add();
            column7.Width = 69;
            column7.StyleID = "s77";
            WorksheetColumn column8 = sheet.Table.Columns.Add();
            column8.Width = 85;
            column8.StyleID = "s98";
            WorksheetColumn column9 = sheet.Table.Columns.Add();
            column9.Width = 112;
            column9.StyleID = "s77";
            WorksheetColumn column10 = sheet.Table.Columns.Add();
            column10.Width = 69;
            column10.StyleID = "s77";
            sheet.Table.Columns.Add(112);
            WorksheetColumn column12 = sheet.Table.Columns.Add();
            column12.Width = 69;
            column12.StyleID = "s77";
            // -----------------------------------------------
            Row0 = sheet.Table.Rows.Add();
            Row0.Height = 23;
            Row0.AutoFitHeight = false;
            Row0.Cells.Add("Images Uploaded to CAPTAIN between " + dtpFrmDte.Value.ToShortDateString() + " - " + dtpToDte.Value.ToShortDateString() + "", DataType.String, "s83");

            cell = Row0.Cells.Add();
            cell.StyleID = "s84";
            cell = Row0.Cells.Add();
            cell.StyleID = "s84";
            cell = Row0.Cells.Add();
            cell.StyleID = "s81";
            cell = Row0.Cells.Add();
            cell.StyleID = "s80";
            cell = Row0.Cells.Add();
            cell.StyleID = "s80";
            cell = Row0.Cells.Add();
            cell.StyleID = "s80";
            cell = Row0.Cells.Add();
            cell.StyleID = "s81";
            cell = Row0.Cells.Add();
            cell.StyleID = "s96";
            cell = Row0.Cells.Add();
            cell.StyleID = "s81";
            cell = Row0.Cells.Add();
            cell.StyleID = "s81";
            cell = Row0.Cells.Add();
            cell.StyleID = "s80";
            cell = Row0.Cells.Add();
            cell.StyleID = "s81";
            // -----------------------------------------------
            Row1 = sheet.Table.Rows.Add();
            Row1.Height = 19;
            Row1.AutoFitHeight = false;
            Row1.Cells.Add("Sno", DataType.String, "s99");
            Row1.Cells.Add("Applicant #", DataType.String, "s100");
            Row1.Cells.Add("Fam Seq", DataType.String, "s101");
            Row1.Cells.Add("Security", DataType.String, "s99");
            Row1.Cells.Add("Image Type", DataType.String, "s102");
            Row1.Cells.Add("Image/document Name", DataType.String, "s103");
            Row1.Cells.Add("Uploaded By", DataType.String, "s103");
            Row1.Cells.Add("Uploaded On", DataType.String, "s104");
            if (BaseForm.BaseAgencyControlDetails.PIPSwitch != "N" && BaseForm.BaseAgencyControlDetails.PIPSwitch != string.Empty)
            {
                Row1.Cells.Add("Copied from PIP", DataType.String, "s104");
            }
            Row1.Cells.Add("Changed By", DataType.String, "s104");
            Row1.Cells.Add("Changed On", DataType.String, "s104");
            Row1.Cells.Add("Deleted By", DataType.String, "s103");
            Row1.Cells.Add("Deleted On", DataType.String, "s104");
            // -----------------------------------------------

            int intseq = 0;
            foreach (DataRow dritem in dt.Rows)
            {
                intseq = intseq + 1;
                Row1 = sheet.Table.Rows.Add();
                Row1.Height = 16;
                Row1.AutoFitHeight = false;
                Row1.Cells.Add(intseq.ToString(), DataType.Number, "s92");
                Row1.Cells.Add(dritem["APPlNO"].ToString(), DataType.String, "s93");
                Row1.Cells.Add(dritem["IMGLOG_FAMILY_SEQ"].ToString() == string.Empty ? "House Hold" : dritem["IMGLOG_FAMILY_SEQ"].ToString(), DataType.String, "s92");

                Row1.Cells.Add(dritem["IMGLOG_SECURITY"].ToString(), DataType.String, "s79");
                Row1.Cells.Add(dritem["IMGTYPE"].ToString(), DataType.String, "s78");
                Row1.Cells.Add(dritem["IMGLOG_UPLoadAs"].ToString(), DataType.String, "s78");
                Row1.Cells.Add(dritem["IMGLOG_UPLOAD_BY"].ToString(), DataType.String, "s78");
                Row1.Cells.Add(dritem["IMGLOG_DATE_UPLOAD"].ToString(), DataType.String, "s78");
                if (BaseForm.BaseAgencyControlDetails.PIPSwitch != "N" && BaseForm.BaseAgencyControlDetails.PIPSwitch != string.Empty)
                {
                    Row1.Cells.Add((dritem["IMGLOG_SCREEN"].ToString().ToUpper() == "PIP00000" ? "Yes" : string.Empty), DataType.String, "s79");
                }
                Row1.Cells.Add(dritem["IMGLOG_LSTC_OPERATOR"].ToString(), DataType.String, "s78");
                Row1.Cells.Add(dritem["IMGLOG_DATE_LSTC"].ToString(), DataType.String, "s78");
                Row1.Cells.Add(dritem["IMGLOG_DELETED_BY"].ToString(), DataType.String, "s78");
                Row1.Cells.Add(dritem["IMGLOG_DATE_DELETED"].ToString(), DataType.String, "s78");

                // -----------------------------------------------
            }
            FileStream stream = new FileStream(PdfName, FileMode.Create);

            book.Save(stream);
            stream.Close();
            AlertBox.Show(strFileName + " File Generated Successfully");
        }

        private void GenerateExcelfilePipuploadStyles(WorksheetStyleCollection styles)
        {
            // -----------------------------------------------
            //  Default
            // -----------------------------------------------
            WorksheetStyle Default = styles.Add("Default");
            Default.Name = "Normal";
            Default.Font.FontName = "Arial";
            Default.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s57
            // -----------------------------------------------
            WorksheetStyle s57 = styles.Add("s57");
            s57.Name = "Hyperlink";
            s57.Font.Underline = UnderlineStyle.Single;
            s57.Font.FontName = "Arial";
            s57.Font.Color = "#0563C1";
            // -----------------------------------------------
            //  s58
            // -----------------------------------------------
            WorksheetStyle s58 = styles.Add("s58");
            s58.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s58.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s58.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s58.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s59
            // -----------------------------------------------
            WorksheetStyle s59 = styles.Add("s59");
            s59.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s59.Alignment.Vertical = StyleVerticalAlignment.Center;
            s59.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s59.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s59.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s59.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s60
            // -----------------------------------------------
            WorksheetStyle s60 = styles.Add("s60");
            s60.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s60.Alignment.Vertical = StyleVerticalAlignment.Center;
            // -----------------------------------------------
            //  s61
            // -----------------------------------------------
            WorksheetStyle s61 = styles.Add("s61");
            s61.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s61.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s62
            // -----------------------------------------------
            WorksheetStyle s62 = styles.Add("s62");
            s62.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s62.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s63
            // -----------------------------------------------
            WorksheetStyle s63 = styles.Add("s63");
            s63.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s63.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s63.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s63.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s63.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s63.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s64
            // -----------------------------------------------
            WorksheetStyle s64 = styles.Add("s64");
            s64.Font.FontName = "Arial";
            s64.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s64.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s64.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s64.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s65
            // -----------------------------------------------
            WorksheetStyle s65 = styles.Add("s65");
            s65.Font.Bold = true;
            s65.Font.FontName = "Calibri Light";
            s65.Font.Size = 16;
            s65.Interior.Color = "#DEEBF6";
            s65.Interior.Pattern = StyleInteriorPattern.Solid;
            s65.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s65.Alignment.Vertical = StyleVerticalAlignment.Center;
            // -----------------------------------------------
            //  s66
            // -----------------------------------------------
            WorksheetStyle s66 = styles.Add("s66");
            s66.Font.Bold = true;
            s66.Font.FontName = "Calibri Light";
            s66.Font.Size = 16;
            s66.Interior.Color = "#DEEBF6";
            s66.Interior.Pattern = StyleInteriorPattern.Solid;
            s66.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s66.Alignment.Vertical = StyleVerticalAlignment.Center;
            // -----------------------------------------------
            //  s67
            // -----------------------------------------------
            WorksheetStyle s67 = styles.Add("s67");
            s67.Interior.Color = "#DEEBF6";
            s67.Interior.Pattern = StyleInteriorPattern.Solid;
            // -----------------------------------------------
            //  s68
            // -----------------------------------------------
            WorksheetStyle s68 = styles.Add("s68");
            s68.Interior.Color = "#DEEBF6";
            s68.Interior.Pattern = StyleInteriorPattern.Solid;
            s68.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s68.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s69
            // -----------------------------------------------
            WorksheetStyle s69 = styles.Add("s69");
            s69.Font.Bold = true;
            s69.Font.FontName = "Calibri";
            s69.Font.Color = "#000000";
            s69.Interior.Color = "#FBE4D5";
            s69.Interior.Pattern = StyleInteriorPattern.Solid;
            s69.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s69.Alignment.Vertical = StyleVerticalAlignment.Center;
            s69.Alignment.WrapText = true;
            s69.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            // -----------------------------------------------
            //  s70
            // -----------------------------------------------
            WorksheetStyle s70 = styles.Add("s70");
            s70.Font.Bold = true;
            s70.Font.FontName = "Calibri";
            s70.Font.Color = "#000000";
            s70.Interior.Color = "#FBE4D5";
            s70.Interior.Pattern = StyleInteriorPattern.Solid;
            s70.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s70.Alignment.Vertical = StyleVerticalAlignment.Center;
            s70.Alignment.WrapText = true;
            s70.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            // -----------------------------------------------
            //  s71
            // -----------------------------------------------
            WorksheetStyle s71 = styles.Add("s71");
            s71.Font.Bold = true;
            s71.Font.FontName = "Calibri";
            s71.Font.Color = "#000000";
            s71.Interior.Color = "#FBE4D5";
            s71.Interior.Pattern = StyleInteriorPattern.Solid;
            s71.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            s71.Alignment.Vertical = StyleVerticalAlignment.Center;
            s71.Alignment.WrapText = true;
            s71.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            // -----------------------------------------------
            //  s72
            // -----------------------------------------------
            WorksheetStyle s72 = styles.Add("s72");
            s72.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s72.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s73
            // -----------------------------------------------
            WorksheetStyle s73 = styles.Add("s73");
            s73.Font.FontName = "Arial";
            s73.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s73.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s74
            // -----------------------------------------------
            WorksheetStyle s74 = styles.Add("s74");
            // -----------------------------------------------
            //  s75
            // -----------------------------------------------
            WorksheetStyle s75 = styles.Add("s75");
            s75.Interior.Color = "#DEEBF6";
            s75.Interior.Pattern = StyleInteriorPattern.Solid;
            s75.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s75.Alignment.Vertical = StyleVerticalAlignment.Center;
            // -----------------------------------------------
            //  s76
            // -----------------------------------------------
            WorksheetStyle s76 = styles.Add("s76");
            s76.Interior.Color = "#DEEBF6";
            s76.Interior.Pattern = StyleInteriorPattern.Solid;
            s76.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s76.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s77
            // -----------------------------------------------
            WorksheetStyle s77 = styles.Add("s77");
            s77.Font.Bold = true;
            s77.Font.FontName = "Calibri";
            s77.Font.Size = 11;
            s77.Font.Color = "#000000";
            s77.Interior.Color = "#FBE4D5";
            s77.Interior.Pattern = StyleInteriorPattern.Solid;
            s77.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s77.Alignment.Vertical = StyleVerticalAlignment.Center;
            s77.Alignment.WrapText = true;
            s77.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s77.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s77.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s77.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s77.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s78
            // -----------------------------------------------
            WorksheetStyle s78 = styles.Add("s78");
            s78.Font.FontName = "Calibri";
            s78.Font.Size = 11;
            // -----------------------------------------------
            //  s79
            // -----------------------------------------------
            WorksheetStyle s79 = styles.Add("s79");
            s79.Font.Bold = true;
            s79.Font.FontName = "Calibri";
            s79.Font.Size = 11;
            s79.Font.Color = "#000000";
            s79.Interior.Color = "#FBE4D5";
            s79.Interior.Pattern = StyleInteriorPattern.Solid;
            s79.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s79.Alignment.Vertical = StyleVerticalAlignment.Center;
            s79.Alignment.WrapText = true;
            s79.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s79.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s79.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s79.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s79.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s80
            // -----------------------------------------------
            WorksheetStyle s80 = styles.Add("s80");
            s80.Font.FontName = "Arial";
            s80.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s80.Alignment.Vertical = StyleVerticalAlignment.Center;
            s80.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s80.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s80.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s80.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s81
            // -----------------------------------------------
            WorksheetStyle s81 = styles.Add("s81");
            s81.Font.FontName = "Arial";
            s81.Font.Color = "#000000";
            s81.Interior.Color = "#FFFFFF";
            s81.Interior.Pattern = StyleInteriorPattern.Solid;
            s81.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s81.Alignment.Vertical = StyleVerticalAlignment.Center;
            s81.Alignment.WrapText = true;
            s81.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            // -----------------------------------------------
            //  s82
            // -----------------------------------------------
            WorksheetStyle s82 = styles.Add("s82");
            s82.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s82.Alignment.Vertical = StyleVerticalAlignment.Center;
            // -----------------------------------------------
            //  s83
            // -----------------------------------------------
            WorksheetStyle s83 = styles.Add("s83");
            s83.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s83.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s84
            // -----------------------------------------------
            WorksheetStyle s84 = styles.Add("s84");
            s84.Font.FontName = "Arial";
            s84.Font.Color = "#000000";
            s84.Interior.Color = "#FFFFFF";
            s84.Interior.Pattern = StyleInteriorPattern.Solid;
            s84.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s84.Alignment.Vertical = StyleVerticalAlignment.Center;
            s84.Alignment.WrapText = true;
            s84.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s84.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s84.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s84.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s84.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s85
            // -----------------------------------------------
            WorksheetStyle s85 = styles.Add("s85");
            s85.Parent = "s57";
            s85.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s85.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s85.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s85.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s86
            // -----------------------------------------------
            WorksheetStyle s86 = styles.Add("s86");
            s86.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s86.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s86.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s86.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s86.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s86.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            s86.NumberFormat = "Short Date";
            // -----------------------------------------------
            //  s87
            // -----------------------------------------------
            WorksheetStyle s87 = styles.Add("s87");
            s87.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s87.Alignment.Vertical = StyleVerticalAlignment.Center;
            s87.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s87.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s87.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s87.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            s87.NumberFormat = "Short Date";
        }

        private void GenerateExcelfilePipupload()
        {

            PdfName = "IMGS_UPLOADED_PIP_" + BaseForm.UserID.ToString() + "_" + DateTime.Now.ToString("yyyyMMdd") + "_" + DateTime.Now.ToString("HHmmss");

            Random_Filename = null;

            string strFileName = string.Empty;

            strFileName = PdfName;

            strFileName = strFileName + ".xls";

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



            string strselectImgAgency = string.Empty;
            if (BaseForm.BaseAgencyControlDetails.PIPSwitch == "I")
            {
                if (cmbAgy.Items.Count > 0)
                {
                    strselectImgAgency = ((ListItem)cmbAgy.SelectedItem).Value.ToString();
                }
                if (strselectImgAgency == "00")
                    strselectImgAgency = string.Empty;
            }

            DataTable dt = PIPDATA.GETPIPINTAKEDATABYDATE(BaseForm.BaseLeanDataBaseConnectionString, dtpFrmDte.Value.ToShortDateString(), dtpToDte.Value.ToShortDateString(), BaseForm.BaseAgencyControlDetails.AgyShortName, strselectImgAgency, "ADMNB005", string.Empty);

            // PIPAdmin..ADMNB005_GET(strselectImgAgency, dtpFrmDte.Value.ToString(), dtpToDte.Value.ToString(), "UPDCAPTAIN");






            Workbook book = new Workbook();

            this.GenerateExcelfilePipuploadStyles(book.Styles);

            Worksheet sheet; WorksheetCell cell;

            sheet = book.Worksheets.Add("Parameters");
            sheet.Table.DefaultRowHeight = 13.2F;
            sheet.Table.ExpandedColumnCount = 10;
            sheet.Table.ExpandedRowCount = 45;
            sheet.Table.FullColumns = 1;
            sheet.Table.FullRows = 1;
            // -----------------------------------------------
            WorksheetRow Row0 = sheet.Table.Rows.Add();
            Row0.Height = 21;

            cell = Row0.Cells.Add();
            cell.StyleID = "s65";
            cell = Row0.Cells.Add();
            cell.StyleID = "s66";
            cell = Row0.Cells.Add();
            cell.StyleID = "s66";
            cell = Row0.Cells.Add();
            cell.StyleID = "s67";
            cell = Row0.Cells.Add();
            cell.StyleID = "s67";
            cell = Row0.Cells.Add();
            cell.StyleID = "s67";
            cell = Row0.Cells.Add();
            cell.StyleID = "s67";
            cell = Row0.Cells.Add();
            cell.StyleID = "s67";
            cell = Row0.Cells.Add();
            cell.StyleID = "s68";
            cell = Row0.Cells.Add();
            cell.StyleID = "s67";
            // -----------------------------------------------
            WorksheetRow Row1 = sheet.Table.Rows.Add();
            Row1.Height = 18;
            Row1.AutoFitHeight = false;
            cell = Row1.Cells.Add();
            cell.StyleID = "s69";
            cell = Row1.Cells.Add();
            cell.StyleID = "s69";
            cell = Row1.Cells.Add();
            cell.StyleID = "s69";
            cell = Row1.Cells.Add();
            cell.StyleID = "s70";
            cell = Row1.Cells.Add();
            cell.StyleID = "s71";
            cell = Row1.Cells.Add();
            cell.StyleID = "s71";
            cell = Row1.Cells.Add();
            cell.StyleID = "s71";
            cell = Row1.Cells.Add();
            cell.StyleID = "s71";
            cell = Row1.Cells.Add();
            cell.StyleID = "s69";
            cell = Row1.Cells.Add();
            cell.StyleID = "s70";
            // -----------------------------------------------
            WorksheetRow Row2 = sheet.Table.Rows.Add();
            Row2.Height = 18;
            Row2.AutoFitHeight = false;
            cell = Row2.Cells.Add();
            cell.StyleID = "s72";
            cell = Row2.Cells.Add();
            cell.StyleID = "s73";
            cell = Row2.Cells.Add();
            cell.StyleID = "s72";
            cell = Row2.Cells.Add();
            cell.StyleID = "s72";
            cell = Row2.Cells.Add();
            cell.StyleID = "s72";
            cell = Row2.Cells.Add();
            cell.StyleID = "s74";
            cell = Row2.Cells.Add();
            cell.StyleID = "s74";
            cell = Row2.Cells.Add();
            cell.StyleID = "s74";
            cell = Row2.Cells.Add();
            cell.StyleID = "s72";
            cell = Row2.Cells.Add();
            cell.StyleID = "s74";



            sheet = book.Worksheets.Add("Details");
            sheet.Table.DefaultRowHeight = 13.2F;
            sheet.Table.DefaultColumnWidth = 50.4F;
            sheet.Table.FullColumns = 1;
            sheet.Table.FullRows = 1;
            WorksheetColumn column0 = sheet.Table.Columns.Add();
            column0.Width = 43;
            column0.StyleID = "s61";
            sheet.Table.Columns.Add(69);
            WorksheetColumn column2 = sheet.Table.Columns.Add();
            column2.Width = 77;
            column2.StyleID = "s61";
            WorksheetColumn column3 = sheet.Table.Columns.Add();
            column3.Width = 175;
            column3.StyleID = "s61";
            WorksheetColumn column4 = sheet.Table.Columns.Add();
            column4.Width = 112;
            column4.StyleID = "s61";
            WorksheetColumn column5 = sheet.Table.Columns.Add();
            column5.Width = 139;
            column5.StyleID = "s61";
            WorksheetColumn column6 = sheet.Table.Columns.Add();
            column6.Width = 47;
            column6.StyleID = "s61";
            sheet.Table.Columns.Add(142);
            sheet.Table.Columns.Add(198);
            sheet.Table.Columns.Add(63);
            WorksheetColumn column10 = sheet.Table.Columns.Add();
            column10.Width = 69;
            column10.StyleID = "s60";
            WorksheetColumn column11 = sheet.Table.Columns.Add();
            column11.Width = 112;
            column11.StyleID = "s60";
            WorksheetColumn column12 = sheet.Table.Columns.Add();
            column12.Width = 112;
            column12.StyleID = "s61";
            WorksheetColumn column13 = sheet.Table.Columns.Add();
            column13.Width = 85;
            column13.StyleID = "s62";
            WorksheetColumn column14 = sheet.Table.Columns.Add();
            column14.Width = 69;
            column14.StyleID = "s60";
            column14.Span = 1;
            WorksheetColumn column15 = sheet.Table.Columns.Add();
            column15.Index = 17;
            column15.Width = 112;
            sheet.Table.Columns.Add(166);
            sheet.Table.Columns.Add(69);
            sheet.Table.Columns.Add(274);
            // -----------------------------------------------
            Row0 = sheet.Table.Rows.Add();
            Row0.Height = 21;
            Row0.Cells.Add("Images Uploaded to PIP between " + dtpFrmDte.Value.ToShortDateString() + " - " + dtpToDte.Value.ToShortDateString() + "", DataType.String, "s65");

            cell = Row0.Cells.Add();
            cell.StyleID = "s67";
            cell = Row0.Cells.Add();
            cell.StyleID = "s66";
            cell = Row0.Cells.Add();
            cell.StyleID = "s66";
            cell = Row0.Cells.Add();
            cell.StyleID = "s66";
            cell = Row0.Cells.Add();
            cell.StyleID = "s68";
            cell = Row0.Cells.Add();
            cell.StyleID = "s68";
            cell = Row0.Cells.Add();
            cell.StyleID = "s67";
            cell = Row0.Cells.Add();
            cell.StyleID = "s67";
            cell = Row0.Cells.Add();
            cell.StyleID = "s67";
            cell = Row0.Cells.Add();
            cell.StyleID = "s75";
            cell = Row0.Cells.Add();
            cell.StyleID = "s75";
            cell = Row0.Cells.Add();
            cell.StyleID = "s68";
            cell = Row0.Cells.Add();
            cell.StyleID = "s76";
            cell = Row0.Cells.Add();
            cell.StyleID = "s75";
            cell = Row0.Cells.Add();
            cell.StyleID = "s75";
            cell = Row0.Cells.Add();
            cell.StyleID = "s67";
            cell = Row0.Cells.Add();
            cell.StyleID = "s67";
            cell = Row0.Cells.Add();
            cell.StyleID = "s67";
            cell = Row0.Cells.Add();
            cell.StyleID = "s67";
            cell = Row0.Cells.Add();
            cell.StyleID = "s67";
            // -----------------------------------------------
            Row1 = sheet.Table.Rows.Add();
            Row1.Height = 28;
            Row1.Cells.Add("Sno", DataType.String, "s79");
            Row1.Cells.Add("Uploaded On", DataType.String, "s77");
            Row1.Cells.Add("Conformation #", DataType.String, "s77");
            Row1.Cells.Add("Registered Email", DataType.String, "s77");
            Row1.Cells.Add("CAPTAIN APP#", DataType.String, "s77");
            Row1.Cells.Add("Member Name", DataType.String, "s79");
            Row1.Cells.Add("Security", DataType.String, "s79");
            Row1.Cells.Add("Image Type", DataType.String, "s77");
            Row1.Cells.Add("Image/document Name", DataType.String, "s77");
            Row1.Cells.Add("Verified Status", DataType.String, "s77");
            Row1.Cells.Add("Verified On", DataType.String, "s79");
            Row1.Cells.Add("Verified By", DataType.String, "s79");
            Row1.Cells.Add("Attested Fname", DataType.String, "s77");
            Row1.Cells.Add("Attested Lname", DataType.String, "s77");
            Row1.Cells.Add("Attested On", DataType.String, "s79");
            Row1.Cells.Add("Dragged to CAPTAIN", DataType.String, "s79");
            Row1.Cells.Add("Dragged By", DataType.String, "s77");
            Row1.Cells.Add("Dragged As", DataType.String, "s77");
            Row1.Cells.Add("Last Message Sent", DataType.String, "s77");
            Row1.Cells.Add("Remarks", DataType.String, "s77");
            Row1.Cells.Add("LINK Doc", DataType.String, "s77");
            // -----------------------------------------------



            // -----------------------------------------------

            int intseq = 0;
            string strstatus = string.Empty;
            foreach (DataRow dritem in dt.Rows)
            {
                strstatus = string.Empty;
                if (dritem["PIPDOCUPLD_VERIFIED_STAT"].ToString() == "C")
                {
                    strstatus = "Completed";
                }
                else if (dritem["PIPDOCUPLD_VERIFIED_STAT"].ToString() == "I")
                {
                    strstatus = "Incomplete";
                }
                intseq = intseq + 1;
                Row1 = sheet.Table.Rows.Add();
                Row1.Height = 18;
                Row1.AutoFitHeight = false;
                Row1.Cells.Add(intseq.ToString(), DataType.Number, "s59");
                Row1.Cells.Add(LookupDataAccess.Getdate(dritem["PIPDOCUPLD_DATE_ADD"].ToString()), DataType.String, "s58");
                Row1.Cells.Add(dritem["PIPREG_CONFNO"].ToString(), DataType.String, "s58");
                Row1.Cells.Add(dritem["PIPREG_EMAIL"].ToString(), DataType.String, "s58");
                Row1.Cells.Add(dritem["APPLICANT"].ToString(), DataType.String, "s80");
                Row1.Cells.Add(dritem["NAME"].ToString(), DataType.String, "s58");
                Row1.Cells.Add(dritem["PIPDOCUPLD_SECURITY"].ToString(), DataType.String, "s63");
                Row1.Cells.Add(dritem["IMAGE_DESC"].ToString(), DataType.String, "s64");
                Row1.Cells.Add(dritem["PIPDOCUPLD_DOCNAME"].ToString(), DataType.String, "s58");
                Row1.Cells.Add(strstatus.ToString(), DataType.String, "s58");
                Row1.Cells.Add(LookupDataAccess.Getdate(dritem["PIPDOCUPLD_DATE_VERIFIED"].ToString()), DataType.String, "s58");
                Row1.Cells.Add(dritem["PIPDOCUPLD_VERIFIED_BY"].ToString(), DataType.String, "s58");
                Row1.Cells.Add(dritem["PIPDOCUPLD_FNAME"].ToString(), DataType.String, "s58");
                Row1.Cells.Add(dritem["PIPDOCUPLD_LNAME"].ToString(), DataType.String, "s58");
                Row1.Cells.Add(LookupDataAccess.Getdate(dritem["PIPDOCUPLD_ATSTN_DATE"].ToString()), DataType.String, "s58");
                Row1.Cells.Add(LookupDataAccess.Getdate(dritem["PIPDOCUPLD_DATE_DRAGD"].ToString()), DataType.String, "s58");
                Row1.Cells.Add(dritem["PIPDOCUPLD_DRAGD_BY"].ToString(), DataType.String, "s58");
                Row1.Cells.Add(dritem["PIPDOCUPLD_DRAGD_AS"].ToString(), DataType.String, "s58");
                Row1.Cells.Add(LookupDataAccess.Getdate(dritem["PIPDOCUPLD_MAIL_ON"].ToString()), DataType.String, "s58");
                Row1.Cells.Add(dritem["PIPDOCUPLD_REMARKS"].ToString(), DataType.String, "s58");
                Row1.Cells.Add(dritem["PIPDOCUPLD_LINK_DOC"].ToString(), DataType.String, "s59");


                // -----------------------------------------------
            }
            FileStream stream = new FileStream(PdfName, FileMode.Create);

            book.Save(stream);
            stream.Close();
            AlertBox.Show(strFileName + " File Generated Successfully");
        }

        #endregion

        private void cmbReport_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbReport.Items.Count > 0)
            {
                lblFrmDte.Visible = false;
                lblToDte.Visible = false;
                dtpFrmDte.Visible = false;
                dtpToDte.Visible = false;
                pnlDteRange.Visible = false;

                if (((ListItem)cmbReport.SelectedItem).Value.ToString() == "2" || ((ListItem)cmbReport.SelectedItem).Value.ToString() == "3")
                {
                    pnlDteRange.Visible = true;
                    lblFrmDte.Visible = true;
                    lblToDte.Visible = true;
                    dtpFrmDte.Visible = true;
                    dtpToDte.Visible = true;
                }
            }
        }

        #region Added by Vikash on 09/28/2023 as a part of enhancement in "Brian work for DSS feed for week of sept 19.doc - 9/26/2023 - Image Types Report" point

        private void Pb_Search_Hie_Click(object sender, EventArgs e)
        {
            HierarchieSelection hierarchieSelectionForm = new HierarchieSelection(BaseForm, Current_Hierarchy_DB, "Master", "A", "D", "Reports", BaseForm.UserID);
            hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
            hierarchieSelectionForm.StartPosition = FormStartPosition.CenterScreen;
            hierarchieSelectionForm.ShowDialog();
        }

        string DepYear;
        bool DefHieExist = false;

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
                this.Txt_HieDesc.Size = new System.Drawing.Size(676, 25);

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
                this.Txt_HieDesc.Size = new System.Drawing.Size(676, 25);
        }

        private void GenerateExcelfileUploadCaptain_CT()
        {

            PdfName = "IMGS_UPLOADED_CAP_" + BaseForm.UserID.ToString() + "_" + DateTime.Now.ToString("yyyyMMdd") + "_" + DateTime.Now.ToString("HHmmss");

            Random_Filename = null;

            string strFileName = string.Empty;

            strFileName = PdfName;

            strFileName = strFileName + ".xls";

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
                CommonFunctions.MessageBoxDisplay("Error");
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

            DataSet ds = SPAdminDB.ADMNB005_GET(Current_Hierarchy.Substring(0, 2), dtpFrmDte.Value.ToString(), dtpToDte.Value.ToString(), "IMGTYPES", Current_Hierarchy.Substring(2, 2), Current_Hierarchy.Substring(4, 2), Program_Year);

            DataTable dt = ds.Tables[0];

            Workbook book = new Workbook();

            this.GenerateStylesUpload(book.Styles);
            #region styles
            Worksheet sheet;
            WorksheetCell cell;

            //// -----------------------------------------------
            WorksheetRow Row0;
            //// -----------------------------------------------
            WorksheetRow Row1;
            sheet = book.Worksheets.Add("Details");
            sheet.Table.DefaultRowHeight = 14.25F;
            sheet.Table.DefaultColumnWidth = 50.4F;
            sheet.Table.FullColumns = 1;
            sheet.Table.FullRows = 1;
            WorksheetColumn column0 = sheet.Table.Columns.Add();
            column0.Width = 43;
            column0.StyleID = "s77";
            WorksheetColumn column1 = sheet.Table.Columns.Add();
            column1.Width = 60;
            column1.StyleID = "s77";
            WorksheetColumn column2 = sheet.Table.Columns.Add();
            column2.Width = 110;
            column2.StyleID = "s77";
            WorksheetColumn column3 = sheet.Table.Columns.Add();
            column3.Width = 110;
            column3.StyleID = "s77";

            sheet.Table.Columns.Add(60);

            WorksheetColumn column7 = sheet.Table.Columns.Add();
            column7.Width = 180;//100;
            column7.StyleID = "s77";

            sheet.Table.Columns.Add(80);

            WorksheetColumn column8 = sheet.Table.Columns.Add();
            column8.Width = 110;//60;
            column8.StyleID = "s77";

            //WorksheetColumn column12 = sheet.Table.Columns.Add();
            //column12.Width = 70;
            //column12.StyleID = "s77";
            WorksheetColumn column13 = sheet.Table.Columns.Add();
            column13.Width = 61;
            column13.StyleID = "s77";
            sheet.Table.Columns.Add(150);
            sheet.Table.Columns.Add(185);
            sheet.Table.Columns.Add(100);
            WorksheetColumn column14 = sheet.Table.Columns.Add();
            column14.Width = 110;//75;
            column14.StyleID = "s77";
            WorksheetColumn column15 = sheet.Table.Columns.Add();
            column15.Width = 90;//85;
            column15.StyleID = "s77";
            WorksheetColumn column16 = sheet.Table.Columns.Add();
            column16.Width = 110;
            column16.StyleID = "s77";
            WorksheetColumn column17 = sheet.Table.Columns.Add();
            column17.Width = 75;//69;
            column17.StyleID = "s77";
            WorksheetColumn column18 = sheet.Table.Columns.Add();
            column18.Width = 110;//80;
            column18.StyleID = "s77";
            // -----------------------------------------------
            Row0 = sheet.Table.Rows.Add();
            Row0.Height = 23;
            Row0.AutoFitHeight = false;
            Row0.Cells.Add("Images Uploaded to CAPTAIN between " + dtpFrmDte.Value.ToShortDateString() + " - " + dtpToDte.Value.ToShortDateString() + "", DataType.String, "s83");

            cell = Row0.Cells.Add();
            cell.StyleID = "s84";
            cell = Row0.Cells.Add();
            cell.StyleID = "s84";
            cell = Row0.Cells.Add();
            cell.StyleID = "s81";
            cell = Row0.Cells.Add();
            cell.StyleID = "s80";
            cell = Row0.Cells.Add();
            cell.StyleID = "s80";
            cell = Row0.Cells.Add();
            cell.StyleID = "s80";
            cell = Row0.Cells.Add();
            cell.StyleID = "s81";
            cell = Row0.Cells.Add();
            cell.StyleID = "s96";
            cell = Row0.Cells.Add();
            cell.StyleID = "s81";
            cell = Row0.Cells.Add();
            cell.StyleID = "s81";
            cell = Row0.Cells.Add();
            cell.StyleID = "s80";
            cell = Row0.Cells.Add();
            cell.StyleID = "s81";
            cell = Row0.Cells.Add();
            cell.StyleID = "s80";
            cell = Row0.Cells.Add();
            cell.StyleID = "s81";

            // -----------------------------------------------
            Row1 = sheet.Table.Rows.Add();
            Row1.Height = 19;
            Row1.AutoFitHeight = false;
            Row1.Cells.Add("Sno", DataType.String, "s99");
            Row1.Cells.Add("Applicant #", DataType.String, "s100");
            Row1.Cells.Add("First Name", DataType.String, "s100");
            Row1.Cells.Add("Last Name", DataType.String, "s100");

            Row1.Cells.Add("DSS Inatke", DataType.String, "s100");

            Row1.Cells.Add("Status", DataType.String, "s102");

            Row1.Cells.Add("Date", DataType.String, "s100");

            Row1.Cells.Add("Worker", DataType.String, "s103");

            //Row1.Cells.Add("Fam Seq", DataType.String, "s101");
            Row1.Cells.Add("Security", DataType.String, "s99");
            Row1.Cells.Add("Image Type", DataType.String, "s102");
            Row1.Cells.Add("Image/document Name", DataType.String, "s103");
            Row1.Cells.Add("Uploaded By", DataType.String, "s103");
            Row1.Cells.Add("Uploaded On", DataType.String, "s104");

            Row1.Cells.Add("Changed By", DataType.String, "s104");
            Row1.Cells.Add("Changed On", DataType.String, "s104");
            Row1.Cells.Add("Deleted By", DataType.String, "s103");
            Row1.Cells.Add("Deleted On", DataType.String, "s104");
            // -----------------------------------------------
            #endregion

            List<CommonEntity> DeniedReason = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "S0085", Current_Hierarchy.Substring(0, 2), Current_Hierarchy.Substring(2, 2), Current_Hierarchy.Substring(4, 2), string.Empty);

            //string strfileAGY = BaseForm.BaseAgencyControlDetails.AgyShortName;

            int intseq = 0;
            foreach (DataRow dritem in dt.Rows)
            {
                intseq = intseq + 1;
                Row1 = sheet.Table.Rows.Add();
                Row1.Height = 16;
                Row1.AutoFitHeight = false;
                Row1.Cells.Add(intseq.ToString(), DataType.Number, "s92");
                Row1.Cells.Add(dritem["IMGLOG_APP"].ToString(), DataType.String, "s93");
                Row1.Cells.Add(dritem["SNP_NAME_IX_FI"].ToString(), DataType.String, "s93");
                Row1.Cells.Add(dritem["SNP_NAME_IX_LAST"].ToString(), DataType.String, "s93");

                //dtDSSXMLData = DSSXMLData.DSSXMLMID_GET(BaseForm.BaseDSSXMLDBConnString, strfileAGY, dritem["APPlNO"].ToString().Substring(0,2), dritem["APPlNO"].ToString().Substring(2, 2), dritem["APPlNO"].ToString().Substring(4, 2), dritem["APPlNO"].ToString().Substring(6, 4), dritem["APPlNO"].ToString().Substring(10, 8), "BYAPPNO");

                //if (dtDSSXMLData.Rows.Count > 0)
                //    Row1.Cells.Add("Yes", DataType.String, "s93");
                //else
                Row1.Cells.Add(dritem["DSS_INTAKE"].ToString(), DataType.String, "s93");

                List<CommonEntity> item = DeniedReason.Where(x => x.Code == dritem["LPB_DENIED"].ToString().Trim()).ToList();

                if (string.IsNullOrEmpty(dritem["MST_INTAKE_DATE"].ToString()))
                {
                    Row1.Cells.Add("", DataType.String, "s93");
                }
                else
                {
                    if (dritem["Col_Status"].ToString() == "Denied" && item.Count > 0 && !string.IsNullOrEmpty(dritem["LPB_DENIED"].ToString()))
                    {
                        Row1.Cells.Add(dritem["Col_Status"].ToString() + " - " + item[0].Desc, DataType.String, "s93");
                    }
                    else
                        Row1.Cells.Add(dritem["Col_Status"].ToString(), DataType.String, "s93");
                }


                if (dritem["Col_Status"].ToString() == "Awaiting Review")
                {
                    Row1.Cells.Add(dritem["MST_INTAKE_DATE"].ToString() == "" ? "" : Convert.ToDateTime(dritem["MST_INTAKE_DATE"]).ToString("MM/dd/yyyy"), DataType.String, "s93");
                    Row1.Cells.Add(dritem["MST_WORKER"].ToString(), DataType.String, "s93");
                }
                else if (dritem["Col_Status"].ToString() == "Incomplete")
                {
                    Row1.Cells.Add(dritem["INTK_LETTER_DATE"].ToString() == "" ? "" : Convert.ToDateTime(dritem["INTK_LETTER_DATE"]).ToString("MM/dd/yyyy"), DataType.String, "s93");
                    Row1.Cells.Add(dritem["INTK_WORKER"].ToString(), DataType.String, "s93");
                }
                else if (dritem["Col_Status"].ToString() == "Denied" && dritem["LPB_DENIED"].ToString() == "01")
                {
                    Row1.Cells.Add(dritem["INTK_LETTER_DATE"].ToString() == "" ? "" : Convert.ToDateTime(dritem["INTK_LETTER_DATE"]).ToString("MM/dd/yyyy"), DataType.String, "s93");

                    Row1.Cells.Add(dritem["INTK_WORKER"].ToString(), DataType.String, "s93");
                }
                else if (dritem["Col_Status"].ToString() == "Denied" && dritem["LPB_DENIED"].ToString() != "01")
                {
                    Row1.Cells.Add(dritem["LPB_DATE"].ToString() == "" ? "" : Convert.ToDateTime(dritem["LPB_DATE"]).ToString("MM/dd/yyyy"), DataType.String, "s93");

                    Row1.Cells.Add(dritem["LPB_WORKER"].ToString(), DataType.String, "s93");
                }
                else
                {
                    Row1.Cells.Add(dritem["LPB_DATE"].ToString() == "" ? "" : Convert.ToDateTime(dritem["LPB_DATE"]).ToString("MM/dd/yyyy"), DataType.String, "s93");
                    Row1.Cells.Add(dritem["LPB_WORKER"].ToString(), DataType.String, "s93");
                }

                /*if (dritem["Col_Status"].ToString() == "Awaiting Review")
                    Row1.Cells.Add(dritem["MST_WORKER"].ToString(), DataType.String, "s93");
                else if(dritem["Col_Status"].ToString() == "Incomplete")
                    Row1.Cells.Add(dritem["INTK_WORKER"].ToString(), DataType.String, "s93");
                else if (dritem["Col_Status"].ToString() == "98" && dritem["LPB_DENIED"].ToString() == "01")
                    Row1.Cells.Add(dritem["INTK_WORKER"].ToString(), DataType.String, "s93");
                else
                    Row1.Cells.Add(dritem["LPB_WORKER"].ToString(), DataType.String, "s93");*/

               // Row1.Cells.Add(dritem["IMGLOG_FAMILY_SEQ"].ToString() == string.Empty ? "House Hold" : dritem["IMGLOG_FAMILY_SEQ"].ToString(), DataType.String, "s92");

                Row1.Cells.Add(dritem["IMGLOG_SECURITY"].ToString(), DataType.String, "s79");
                Row1.Cells.Add(dritem["IMGTYPE"].ToString(), DataType.String, "s78");
                Row1.Cells.Add(dritem["IMGLOG_UPLoadAs"].ToString(), DataType.String, "s78");
                Row1.Cells.Add(dritem["IMGLOG_UPLOAD_BY"].ToString(), DataType.String, "s78");
                Row1.Cells.Add(dritem["IMGLOG_DATE_UPLOAD"].ToString(), DataType.String, "s78");
                //if (BaseForm.BaseAgencyControlDetails.PIPSwitch != "N" && BaseForm.BaseAgencyControlDetails.PIPSwitch != string.Empty)
                //{
                //    Row1.Cells.Add((dritem["IMGLOG_SCREEN"].ToString().ToUpper() == "PIP00000" ? "Yes" : string.Empty), DataType.String, "s79");
                //}
                Row1.Cells.Add(dritem["IMGLOG_LSTC_OPERATOR"].ToString(), DataType.String, "s78");
                Row1.Cells.Add(dritem["IMGLOG_DATE_LSTC"].ToString(), DataType.String, "s78");
                Row1.Cells.Add(dritem["IMGLOG_DELETED_BY"].ToString(), DataType.String, "s78");
                Row1.Cells.Add(dritem["IMGLOG_DATE_DELETED"].ToString(), DataType.String, "s78");


            }
            FileStream stream = new FileStream(PdfName, FileMode.Create);

            book.Save(stream);
            stream.Close();
            AlertBox.Show(strFileName + " File Generated Successfully");

        }

        #endregion
    }
}