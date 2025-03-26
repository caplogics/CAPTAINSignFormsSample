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
using Captain.Common.Views.Controls.Compatibility;
using System.Web.UI.WebControls;
using NPOI.SS.Formula.Functions;
using DevExpress.XtraSpreadsheet.API.Native.Implementation;
using Captain.Common.Interfaces;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class DIMSCOREREPORT : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        //private GridControl _intakeHierarchy = null;
        private bool boolChangeStatus = false;
        private CaptainModel _model = null;
        private List<HierarchyEntity> _selectedHierarchies = null;

        #endregion
        public DIMSCOREREPORT(BaseForm baseForm, PrivilegeEntity privileges)
        {
            InitializeComponent();

            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            txtfrom.Validator = TextBoxValidation.IntegerValidator;
            txtTo.Validator = TextBoxValidation.IntegerValidator;
            propPreassGroup = _model.SPAdminData.Browse_PreassGroups();
            DimensionCombofill();
            BaseForm = baseForm; Privileges = privileges;
            Agency = BaseForm.BaseAgency; Depart = BaseForm.BaseDept; Program = BaseForm.BaseProg;
            strYear = BaseForm.BaseYear;
            Set_Report_Hierarchy(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear);
            propReportPath = _model.lookupDataAccess.GetReportPath();
            this.Text = /*Privileges.Program + " - " + */Privileges.PrivilegeName.Trim();
            cmbPressGroup.Focus();

            DataSet ds1 = Captain.DatabaseLayer.CaseMst.GetCaseWorker(string.Empty, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
            if (ds1.Tables.Count > 0)
            {
                DataTable dt1 = ds1.Tables[0];
                dtWorker = ds1.Tables[0];
            }

            DataSet ds = Captain.DatabaseLayer.CaseMst.GetSiteByHIE(BaseForm.BaseAgency, string.Empty, string.Empty);
            if (ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                dtSite = ds.Tables[0];
            }
        }

        void DimensionCombofill()
        {
            preassessMasterEntity = _model.lookupDataAccess.GetDimension();
            foreach (CommonEntity preassesdata in preassessMasterEntity)
            {
                cmbdimension.Items.Add(new Captain.Common.Utilities.ListItem(preassesdata.Desc, preassesdata.Code));
            }
            cmbdimension.Items.Insert(0, new Captain.Common.Utilities.ListItem("ALL", "ALL"));
            cmbdimension.SelectedIndex = 0;


            List<RankCatgEntity> pressgroupcategorylist = propPreassGroup.FindAll(u => u.SubCode.Trim() == string.Empty);
            foreach (RankCatgEntity item in pressgroupcategorylist)
            {
                cmbPressGroup.Items.Add(new Captain.Common.Utilities.ListItem(item.Desc.ToString().Trim(), item.Code.ToString()));
            }
            cmbPressGroup.Items.Insert(0, new Captain.Common.Utilities.ListItem("ALL", "ALL"));
            cmbPressGroup.SelectedIndex = 0;

            this.cmbPressgcat.SelectedIndexChanged -= new System.EventHandler(this.cmbPressgcat_SelectedIndexChanged);
            cmbPressgcat.Items.Clear();
            cmbPressgcat.Items.Insert(0, new Captain.Common.Utilities.ListItem("ALL", "ALL"));
            cmbPressgcat.SelectedIndex = 0;
            this.cmbPressgcat.SelectedIndexChanged += new System.EventHandler(this.cmbPressgcat_SelectedIndexChanged);
            //RankCatgEntity preasscategory = pressgroupcategorylist.Find(u => (Convert.ToInt32(u.PointsLow) <= Convert.ToInt32(intTotDScore)) && (Convert.ToInt32(u.PointsHigh) >= Convert.ToInt32(intTotDScore)));
            //if (preasscategory != null)
            //{
            //}

        }
        string Agency = string.Empty, Depart = string.Empty, Program = string.Empty, strYear = string.Empty;
        #region properties

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public string Calling_ID { get; set; }

        public string Calling_UserID { get; set; }

        public string propReportPath { get; set; }

        List<CommonEntity> preassessMasterEntity { get; set; }
        List<RankCatgEntity> propPreassGroup { get; set; }
        #endregion

        string Current_Hierarchy = "******", Current_Hierarchy_DB = "**-**-**";
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
                this.Txt_HieDesc.Size = new System.Drawing.Size(800, 25);
                Agency = Agy; Depart = Dept; Program = Prog; //strYear = Year;
            }
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
                this.Txt_HieDesc.Size = new System.Drawing.Size(700, 25);
            else
                this.Txt_HieDesc.Size = new System.Drawing.Size(800, 25);
        }



        private void BtnGenPdf_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, false, propReportPath,"XLS");
                pdfListForm.FormClosed += new FormClosedEventHandler(On_SaveFormClosed_DevExpress);//(On_SaveFormClosed);
                pdfListForm.StartPosition= FormStartPosition.CenterScreen;
                pdfListForm.ShowDialog();
            }

        }

        private bool ValidateForm()
        {
            bool isValid = true;

            if (txtfrom.Text.Trim() == string.Empty)
            {
                _errorProvider.SetError(txtfrom, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblFrom.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtfrom, null);
            }

            if (txtTo.Text.Trim() == string.Empty)
            {
                _errorProvider.SetError(txtTo, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblTo.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(txtTo, null);
            }

            if ((txtfrom.Text.Trim() != string.Empty) && (txtTo.Text.Trim() != string.Empty))
            {
                if (Convert.ToDecimal(txtfrom.Text) > Convert.ToDecimal(txtTo.Text))
                {
                    _errorProvider.SetError(txtTo, "From Score may not be Greater than To Score");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtTo, null);
                }


            }
            if (rdoSelectApplicant.Checked)
            {
                if (txtApplicant.Text.Trim() == string.Empty)
                {
                    _errorProvider.SetError(txtApplicant, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblApplicant.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(txtApplicant, null);
                }

            }
            if (dtpFrmDate.Checked == false && dtpToDt.Checked == true)
            {
                _errorProvider.SetError(dtpFrmDate, "From Date is required");
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtpFrmDate, null);
            }
            if (dtpToDt.Checked == false && dtpFrmDate.Checked == true)
            {
                _errorProvider.SetError(dtpToDt, "To Date is required");
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtpToDt, null);
            }
            if (dtpFrmDate.Checked.Equals(true) && dtpToDt.Checked.Equals(true))
            {
                if (string.IsNullOrWhiteSpace(dtpFrmDate.Text))
                {
                    _errorProvider.SetError(dtpFrmDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "From Date".Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtpFrmDate, null);
                }
                if (string.IsNullOrWhiteSpace(dtpToDt.Text))
                {
                    _errorProvider.SetError(dtpToDt, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "To Date".Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtpToDt, null);
                }
            }
            if (dtpFrmDate.Checked.Equals(true) && dtpToDt.Checked.Equals(true))
            {
                if (!string.IsNullOrEmpty(dtpFrmDate.Text) && (!string.IsNullOrEmpty(dtpToDt.Text)))
                {
                    if (Convert.ToDateTime(dtpFrmDate.Text) > Convert.ToDateTime(dtpToDt.Text))
                    {
                        _errorProvider.SetError(dtpToDt, "'To Date' should be equal to or greater than 'From Date'");
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(dtpToDt, null);
                    }
                }
            }

            if (rdbAlrtCde_Sel.Checked)
            {
                if (SelectedAlertCodes == "")
                {
                    _errorProvider.SetError(rdbAlrtCde_Sel, "Please Select atleast one Alert Code");
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(rdbAlrtCde_Sel, null);
                }
            }

            return (isValid);
        }



        string Random_Filename;
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


                WorksheetRow excelrow;
                WorksheetCell cell;

                Workbook book = new Workbook();

                Worksheet sheet = book.Worksheets.Add("Parameters");

                #region Header Page
                WorksheetColumn columnHead = sheet.Table.Columns.Add();
                columnHead.Index = 2;
                columnHead.Width = 5;
                sheet.Table.Columns.Add(163);
                WorksheetColumn column2Head = sheet.Table.Columns.Add();
                column2Head.Width = 332;
                column2Head.StyleID = "s172";
                sheet.Table.Columns.Add(59);
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
                cell.Data.Text = "            Agency: " + Agency + " , Department : " + Depart + " , Program : " + Program;
                cell.MergeAcross = 2;
                // -----------------------------------------------
                WorksheetRow Row8 = sheet.Table.Rows.Add();
                Row8.Height = 14;
                Row8.AutoFitHeight = false;
                cell = Row8.Cells.Add();
                cell.StyleID = "s139";
                cell = Row8.Cells.Add();
                cell.StyleID = "s143";
                cell = Row8.Cells.Add();
                cell.StyleID = "m2611536909344";
                cell.Data.Type = DataType.String;
                cell.MergeAcross = 2;
                // -----------------------------------------------
                WorksheetRow Row9 = sheet.Table.Rows.Add();
                Row9.Height = 14;
                Row9.AutoFitHeight = false;
                cell = Row9.Cells.Add();
                cell.StyleID = "s139";
                cell = Row9.Cells.Add();
                cell.StyleID = "s143";
                cell = Row9.Cells.Add();
                cell.StyleID = "s139";
                cell = Row9.Cells.Add();
                cell.StyleID = "s170";
                cell = Row9.Cells.Add();
                cell.StyleID = "s145";
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
                WorksheetRow RowPreassG = sheet.Table.Rows.Add();
                RowPreassG.Height = 14;
                RowPreassG.AutoFitHeight = false;
                cell = RowPreassG.Cells.Add();
                cell.StyleID = "s139";
                cell = RowPreassG.Cells.Add();
                cell.StyleID = "s143";
                RowPreassG.Cells.Add("            Preass Group", DataType.String, "s144");
                RowPreassG.Cells.Add(" : " + ((Captain.Common.Utilities.ListItem)cmbPressGroup.SelectedItem).Text.ToString(), DataType.String, "s169");
                cell = RowPreassG.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                WorksheetRow RowPreassC = sheet.Table.Rows.Add();
                RowPreassC.Height = 14;
                RowPreassC.AutoFitHeight = false;
                cell = RowPreassC.Cells.Add();
                cell.StyleID = "s139";
                cell = RowPreassC.Cells.Add();
                cell.StyleID = "s143";
                RowPreassC.Cells.Add("            Preass Category", DataType.String, "s144");
                RowPreassC.Cells.Add(" : " + ((Captain.Common.Utilities.ListItem)cmbPressgcat.SelectedItem).Text.ToString(), DataType.String, "s169");
                cell = RowPreassC.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                WorksheetRow RowScores = sheet.Table.Rows.Add();
                RowScores.Height = 14;
                RowScores.AutoFitHeight = false;
                cell = RowScores.Cells.Add();
                cell.StyleID = "s139";
                cell = RowScores.Cells.Add();
                cell.StyleID = "s143";
                RowScores.Cells.Add("            Score Range", DataType.String, "s144");
                RowScores.Cells.Add(" : " + "From: " + txtfrom.Text + "    To: " + txtTo.Text, DataType.String, "s169");
                cell = RowScores.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                WorksheetRow ReportType = sheet.Table.Rows.Add();
                ReportType.Height = 14;
                ReportType.AutoFitHeight = false;
                cell = ReportType.Cells.Add();
                cell.StyleID = "s139";
                cell = ReportType.Cells.Add();
                cell.StyleID = "s143";
                ReportType.Cells.Add("            Report Type", DataType.String, "s144");
                ReportType.Cells.Add(" : " + (rdoall.Checked ? "All" : "Selected"), DataType.String, "s169");
                cell = ReportType.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                string selectedAlertCodes = SelectedAlertCodes;
                selectedAlertCodes = selectedAlertCodes.Replace("'", "");
                selectedAlertCodes = selectedAlertCodes.Replace(",", ", ");

                WorksheetRow alertCodes = sheet.Table.Rows.Add();
                alertCodes.Height = 14;
                alertCodes.AutoFitHeight = false;
                cell = alertCodes.Cells.Add();
                cell.StyleID = "s139";
                cell = alertCodes.Cells.Add();
                cell.StyleID = "s143";
                alertCodes.Cells.Add("            Alert Codes", DataType.String, "s144");
                alertCodes.Cells.Add(" : " + (rdbAlrtCde_All.Checked == true ? "All Alert Codes" : selectedAlertCodes), DataType.String, "s169");
                cell = alertCodes.Cells.Add();
                cell.StyleID = "s145";
                //-----------------------------------------------
                WorksheetRow AppNum = sheet.Table.Rows.Add();
                AppNum.Height = 14;
                AppNum.AutoFitHeight = false;
                cell = AppNum.Cells.Add();
                cell.StyleID = "s139";
                cell = AppNum.Cells.Add();
                cell.StyleID = "s143";
                AppNum.Cells.Add("            Applicant Number", DataType.String, "s144");
                AppNum.Cells.Add(": " + txtApplicant.Text, DataType.String, "s169");
                cell = AppNum.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                WorksheetRow RowDimension = sheet.Table.Rows.Add();
                RowDimension.Height = 14;
                RowDimension.AutoFitHeight = false;
                cell = RowDimension.Cells.Add();
                cell.StyleID = "s139";
                cell = RowDimension.Cells.Add();
                cell.StyleID = "s143";
                RowDimension.Cells.Add("            Dimension", DataType.String, "s144");
                RowDimension.Cells.Add(" : " + ((Captain.Common.Utilities.ListItem)cmbdimension.SelectedItem).Text.ToString(), DataType.String, "s169");
                cell = RowDimension.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                WorksheetRow Row12 = sheet.Table.Rows.Add();
                Row12.Height = 14;
                Row12.AutoFitHeight = false;
                cell = Row12.Cells.Add();
                cell.StyleID = "s139";
                cell = Row12.Cells.Add();
                cell.StyleID = "s143";
                Row12.Cells.Add("            Date Range", DataType.String, "s144");
                if (dtpFrmDate.Checked && dtpToDt.Checked)
                {
                    Row12.Cells.Add(" : From: " +
                                                CommonFunctions.ChangeDateFormat(Convert.ToDateTime(dtpFrmDate.Value).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat)
                                                + "    To: " +
                                                CommonFunctions.ChangeDateFormat(Convert.ToDateTime(dtpToDt.Value).ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat), DataType.String, "s169");

                }
                else
                    Row12.Cells.Add(" ",DataType.String,"s169");
                
                cell = Row12.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
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

                /*Worksheet*/ sheet = book.Worksheets.Add("Data");
                //sheet.Names.Add(new WorksheetNamedRange("_FilterDatabase", "=Data!R1C1:R"+(MatRepList.Count+1).ToString()+"C11", true));
                sheet.Table.DefaultRowHeight = 14.25F;

                sheet.Table.DefaultColumnWidth = 220.5F;
                sheet.Table.Columns.Add(100);
                sheet.Table.Columns.Add(100);
                sheet.Table.Columns.Add(100);
                sheet.Table.Columns.Add(100);
                sheet.Table.Columns.Add(100);
                sheet.Table.Columns.Add(200);              
               


                List<CommonEntity> preassessMasterEntitytemp = new List<CommonEntity>();
                if (((Captain.Common.Utilities.ListItem)cmbdimension.SelectedItem).Value.ToString() == "ALL")
                    preassessMasterEntitytemp = preassessMasterEntity.OrderBy(U => Convert.ToInt32(U.Code)).ToList();
                else
                    preassessMasterEntitytemp = preassessMasterEntity.FindAll(u => u.Code == ((Captain.Common.Utilities.ListItem)cmbdimension.SelectedItem).Value.ToString());
                int intcolumn = 7;
                foreach (CommonEntity preassesdata in preassessMasterEntitytemp)
                {
                    sheet.Table.Columns.Add(150); 
                }                
                sheet.Table.Columns.Add(150);
                //intcolumn = intcolumn + 1;

              
                sheet.Table.Columns.Add(150);
               // intcolumn = intcolumn + 1;

                //xlWorkSheet.ColumnWidth(intcolumn, 100);
                sheet.Table.Columns.Add(150);

               // int excelcolumn = 0;


                try
                {


                    excelrow = sheet.Table.Rows.Add();
                    
                    //excelcolumn = excelcolumn + 1;
                    //xlWorkSheet[excelcolumn, 1].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    //xlWorkSheet[excelcolumn, 1].Alignment = Alignment.Centered;
                    //xlWorkSheet.WriteCell(excelcolumn, 1, "AGENCY");

                    cell = excelrow.Cells.Add("AGENCY", DataType.String, "MainHeaderStyles");

                    //xlWorkSheet[excelcolumn, 2].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    //xlWorkSheet[excelcolumn, 2].Alignment = Alignment.Centered;
                    //xlWorkSheet.WriteCell(excelcolumn, 2, "DEPT");

                    cell = excelrow.Cells.Add("DEPT", DataType.String, "MainHeaderStyles");

                    //xlWorkSheet[excelcolumn, 3].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    //xlWorkSheet[excelcolumn, 3].Alignment = Alignment.Centered;
                    //xlWorkSheet.WriteCell(excelcolumn, 3, "PROGRAM");

                    cell = excelrow.Cells.Add("PROGRAM", DataType.String, "MainHeaderStyles");

                    //xlWorkSheet[excelcolumn, 4].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    //xlWorkSheet[excelcolumn, 4].Alignment = Alignment.Centered;
                    //xlWorkSheet.WriteCell(excelcolumn, 4, "YEAR");

                    cell = excelrow.Cells.Add("YEAR", DataType.String, "MainHeaderStyles");

                    //xlWorkSheet[excelcolumn, 5].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    //xlWorkSheet[excelcolumn, 5].Alignment = Alignment.Centered;
                    //xlWorkSheet.WriteCell(excelcolumn, 5, "Applicant No");

                    cell = excelrow.Cells.Add("Applicant No", DataType.String, "MainHeaderStyles");

                    //xlWorkSheet[excelcolumn, 6].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    //xlWorkSheet[excelcolumn, 6].Alignment = Alignment.Centered;
                    //xlWorkSheet.WriteCell(excelcolumn, 6, "Name");

                    cell = excelrow.Cells.Add("Name", DataType.String, "MainHeaderStyles");

                    intcolumn = 7;
                    foreach (CommonEntity preassesdata in preassessMasterEntitytemp)
                    {
                         //xlWorkSheet[excelcolumn, intcolumn].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                        //xlWorkSheet[excelcolumn, intcolumn].Alignment = Alignment.Centered;
                        //xlWorkSheet.WriteCell(excelcolumn, intcolumn, preassesdata.Desc);
                        //intcolumn = intcolumn + 1;

                        cell = excelrow.Cells.Add(preassesdata.Desc, DataType.String, "MainHeaderStyles");
                    }

                    //xlWorkSheet[excelcolumn, intcolumn].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    //xlWorkSheet[excelcolumn, intcolumn].Alignment = Alignment.Centered;
                    //xlWorkSheet.WriteCell(excelcolumn, intcolumn, "Applicant Total");

                    cell = excelrow.Cells.Add("Applicant Total", DataType.String, "MainHeaderStyles");

                    //intcolumn = intcolumn + 1;

                    //xlWorkSheet[excelcolumn, intcolumn].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    //xlWorkSheet[excelcolumn, intcolumn].Alignment = Alignment.Centered;
                    //xlWorkSheet.WriteCell(excelcolumn, intcolumn, "Category Name");

                    cell = excelrow.Cells.Add("Category Name", DataType.String, "MainHeaderStyles");

                    //intcolumn = intcolumn + 1;

                    //xlWorkSheet[excelcolumn, intcolumn].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    //xlWorkSheet[excelcolumn, intcolumn].Alignment = Alignment.Centered;
                    //xlWorkSheet.WriteCell(excelcolumn, intcolumn, "Group Name");

                    cell = excelrow.Cells.Add("Group Name", DataType.String, "MainHeaderStyles");

                    //intcolumn = intcolumn + 1;

                    //xlWorkSheet[excelcolumn, intcolumn].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    //xlWorkSheet[excelcolumn, intcolumn].Alignment = Alignment.Centered;
                    //xlWorkSheet.WriteCell(excelcolumn, intcolumn, "Last Update Date");

                    cell = excelrow.Cells.Add("Last Update Date", DataType.String, "MainHeaderStyles");


                    //excelcolumn = excelcolumn + 1;
                    string strApplicantNo = string.Empty;
                    if (rdoSelectApplicant.Checked)
                        strApplicantNo = txtApplicant.Text;

                    string strDimensioncode = string.Empty;
                    string strGroupcode = string.Empty;
                    string strFromdate = string.Empty;
                    string strTodate = string.Empty;

                    if (((Captain.Common.Utilities.ListItem)cmbPressGroup.SelectedItem).Value.ToString() != "ALL")
                        strGroupcode = ((Captain.Common.Utilities.ListItem)cmbPressGroup.SelectedItem).Value.ToString();

                    if (dtpFrmDate.Checked == true && dtpToDt.Checked == true)
                    {
                        strFromdate = dtpFrmDate.Value.ToShortDateString();
                        strTodate = dtpToDt.Value.ToShortDateString();
                    }

                    if (((Captain.Common.Utilities.ListItem)cmbdimension.SelectedItem).Value.ToString() != "ALL")
                        strDimensioncode = ((Captain.Common.Utilities.ListItem)cmbdimension.SelectedItem).Value.ToString();
                    DataSet dSCaseSnp = Captain.DatabaseLayer.CaseSnpData.GETDIMSCORE(Agency == "**" ? string.Empty : Agency, Depart == "**" ? string.Empty : Depart, Program == "**" ? string.Empty : Program, Program_Year.Trim(), strApplicantNo, "SNP", string.Empty, txtfrom.Text, txtTo.Text, strDimensioncode, strGroupcode, strFromdate, strTodate, string.Empty);

                    foreach (DataRow DRitem in dSCaseSnp.Tables[0].Rows)
                    {
                       // excelcolumn = excelcolumn + 1;
                        //xlWorkSheet.WriteCell(excelcolumn, 1, DRitem["MST_AGENCY"].ToString());
                        //xlWorkSheet.WriteCell(excelcolumn, 2, DRitem["MST_DEPT"].ToString());
                        //xlWorkSheet.WriteCell(excelcolumn, 3, DRitem["MST_PROGRAM"].ToString());
                        //xlWorkSheet.WriteCell(excelcolumn, 4, DRitem["MST_YEAR"].ToString());
                        //xlWorkSheet.WriteCell(excelcolumn, 5, DRitem["MST_APP_NO"].ToString());
                        //xlWorkSheet.WriteCell(excelcolumn, 6, LookupDataAccess.GetMemberName(DRitem["SNP_NAME_IX_FI"].ToString(), DRitem["SNP_NAME_IX_MI"].ToString(), DRitem["SNP_NAME_IX_LAST"].ToString(), Member_NameFormat));

                        excelrow = sheet.Table.Rows.Add();

                        cell = excelrow.Cells.Add(DRitem["MST_AGENCY"].ToString(), DataType.String, "NormalLeft");
                        cell = excelrow.Cells.Add(DRitem["MST_DEPT"].ToString(), DataType.String, "NormalLeft");
                        cell = excelrow.Cells.Add(DRitem["MST_PROGRAM"].ToString(), DataType.String, "NormalLeft");
                        cell = excelrow.Cells.Add(DRitem["MST_YEAR"].ToString(), DataType.String, "NormalLeft");
                        cell = excelrow.Cells.Add(DRitem["MST_APP_NO"].ToString(), DataType.String, "NormalLeft");
                        cell = excelrow.Cells.Add(LookupDataAccess.GetMemberName(DRitem["SNP_NAME_IX_FI"].ToString(), DRitem["SNP_NAME_IX_MI"].ToString(), DRitem["SNP_NAME_IX_LAST"].ToString(), Member_NameFormat), DataType.String, "NormalLeft");

                        if (((Captain.Common.Utilities.ListItem)cmbdimension.SelectedItem).Value.ToString() != "ALL")
                        {

                            if (dSCaseSnp.Tables[0].Columns.Contains("A1"))
                            {
                                cell = excelrow.Cells.Add(DRitem["A1"].ToString(), DataType.String, "NormalLeft");
                               // xlWorkSheet.WriteCell(excelcolumn, 7, DRitem["A1"].ToString());
                            }
                            if (dSCaseSnp.Tables[0].Columns.Contains("A2"))
                            {
                                cell = excelrow.Cells.Add(DRitem["A2"].ToString(), DataType.String, "NormalLeft");
                               // xlWorkSheet.WriteCell(excelcolumn, 7, DRitem["A2"].ToString());
                            }
                            if (dSCaseSnp.Tables[0].Columns.Contains("A3"))
                            {
                                cell = excelrow.Cells.Add(DRitem["A3"].ToString(), DataType.String, "NormalLeft");
                               // xlWorkSheet.WriteCell(excelcolumn, 7, DRitem["A3"].ToString());
                            }
                            if (dSCaseSnp.Tables[0].Columns.Contains("A4"))
                            {
                                cell = excelrow.Cells.Add(DRitem["A4"].ToString(), DataType.String, "NormalLeft");
                                //xlWorkSheet.WriteCell(excelcolumn, 7, DRitem["A4"].ToString());
                            }
                            if (dSCaseSnp.Tables[0].Columns.Contains("A5"))
                            {
                                cell = excelrow.Cells.Add(DRitem["A5"].ToString(), DataType.String, "NormalLeft");
                               // xlWorkSheet.WriteCell(excelcolumn, 7, DRitem["A5"].ToString());
                            }
                            if (dSCaseSnp.Tables[0].Columns.Contains("A6"))
                            {
                                cell = excelrow.Cells.Add(DRitem["A6"].ToString(), DataType.String, "NormalLeft");
                               // xlWorkSheet.WriteCell(excelcolumn, 7, DRitem["A6"].ToString());
                            }
                            if (dSCaseSnp.Tables[0].Columns.Contains("A7"))
                            {
                                cell = excelrow.Cells.Add(DRitem["A7"].ToString(), DataType.String, "NormalLeft");
                              //  xlWorkSheet.WriteCell(excelcolumn, 7, DRitem["A7"].ToString());
                            }
                            if (dSCaseSnp.Tables[0].Columns.Contains("A8"))
                            {
                                cell = excelrow.Cells.Add(DRitem["A8"].ToString(), DataType.String, "NormalLeft");
                               // xlWorkSheet.WriteCell(excelcolumn, 7, DRitem["A8"].ToString());
                            }
                            if (dSCaseSnp.Tables[0].Columns.Contains("A9"))
                            {
                                cell = excelrow.Cells.Add(DRitem["A9"].ToString(), DataType.String, "NormalLeft");
                               // xlWorkSheet.WriteCell(excelcolumn, 7, DRitem["A9"].ToString());
                            }
                            if (dSCaseSnp.Tables[0].Columns.Contains("A10"))
                            {
                                cell = excelrow.Cells.Add(DRitem["A10"].ToString(), DataType.String, "NormalLeft");
                               // xlWorkSheet.WriteCell(excelcolumn, 7, DRitem["A10"].ToString());
                            }
                            if (dSCaseSnp.Tables[0].Columns.Contains("A11"))
                            {
                                cell = excelrow.Cells.Add(DRitem["A11"].ToString(), DataType.String, "NormalLeft");
                               // xlWorkSheet.WriteCell(excelcolumn, 7, DRitem["A11"].ToString());
                            }
                            if (dSCaseSnp.Tables[0].Columns.Contains("A12"))
                            {
                                cell = excelrow.Cells.Add(DRitem["A12"].ToString(), DataType.String, "NormalLeft");
                               // xlWorkSheet.WriteCell(excelcolumn, 7, DRitem["A12"].ToString());
                            }
                            if (dSCaseSnp.Tables[0].Columns.Contains("A13"))
                            {
                                cell = excelrow.Cells.Add(DRitem["A13"].ToString(), DataType.String, "NormalLeft");
                               // xlWorkSheet.WriteCell(excelcolumn, 7, DRitem["A13"].ToString());
                            }
                            if (dSCaseSnp.Tables[0].Columns.Contains("A14"))
                            {
                                cell = excelrow.Cells.Add(DRitem["A14"].ToString(), DataType.String, "NormalLeft");
                              //  xlWorkSheet.WriteCell(excelcolumn, 7, DRitem["A14"].ToString());
                            }
                            if (dSCaseSnp.Tables[0].Columns.Contains("A15"))
                            {
                                cell = excelrow.Cells.Add(DRitem["A15"].ToString(), DataType.String, "NormalLeft");
                               // xlWorkSheet.WriteCell(excelcolumn, 7, DRitem["A15"].ToString());
                            }
                            if (dSCaseSnp.Tables[0].Columns.Contains("A16"))
                            {
                                cell = excelrow.Cells.Add(DRitem["A16"].ToString(), DataType.String, "NormalLeft");
                               // xlWorkSheet.WriteCell(excelcolumn, 7, DRitem["A16"].ToString());
                            }
                            if (dSCaseSnp.Tables[0].Columns.Contains("A17"))
                            {
                                cell = excelrow.Cells.Add(DRitem["A17"].ToString(), DataType.String, "NormalLeft");
                               // xlWorkSheet.WriteCell(excelcolumn, 7, DRitem["A17"].ToString());
                            }
                            if (dSCaseSnp.Tables[0].Columns.Contains("A18"))
                            {
                                cell = excelrow.Cells.Add(DRitem["A18"].ToString(), DataType.String, "NormalLeft");
                               // xlWorkSheet.WriteCell(excelcolumn, 7, DRitem["A18"].ToString());
                            }
                            if (dSCaseSnp.Tables[0].Columns.Contains("A19"))
                            {
                                cell = excelrow.Cells.Add(DRitem["A19"].ToString(), DataType.String, "NormalLeft");
                               // xlWorkSheet.WriteCell(excelcolumn, 7, DRitem["A19"].ToString());
                            }
                            if (dSCaseSnp.Tables[0].Columns.Contains("A20"))
                            {
                                cell = excelrow.Cells.Add(DRitem["A20"].ToString(), DataType.String, "NormalLeft");
                              //  xlWorkSheet.WriteCell(excelcolumn, 7, DRitem["A20"].ToString());
                            }
                            if (dSCaseSnp.Tables[0].Columns.Contains("A21"))
                            {
                                cell = excelrow.Cells.Add(DRitem["A21"].ToString(), DataType.String, "NormalLeft");
                               // xlWorkSheet.WriteCell(excelcolumn, 7, DRitem["A21"].ToString());
                            }
                            if (dSCaseSnp.Tables[0].Columns.Contains("A22"))
                            {
                                cell = excelrow.Cells.Add(DRitem["A22"].ToString(), DataType.String, "NormalLeft");
                               // xlWorkSheet.WriteCell(excelcolumn, 7, DRitem["A22"].ToString());
                            }
                            if (dSCaseSnp.Tables[0].Columns.Contains("A23"))
                            {
                                cell = excelrow.Cells.Add(DRitem["A23"].ToString(), DataType.String, "NormalLeft");
                                //xlWorkSheet.WriteCell(excelcolumn, 7, DRitem["A23"].ToString());
                            }
                            if (dSCaseSnp.Tables[0].Columns.Contains("A24"))
                            {
                                cell = excelrow.Cells.Add(DRitem["A24"].ToString(), DataType.String, "NormalLeft");
                               // xlWorkSheet.WriteCell(excelcolumn, 7, DRitem["A24"].ToString());
                            }
                            if (dSCaseSnp.Tables[0].Columns.Contains("A25"))
                            {
                                cell = excelrow.Cells.Add(DRitem["A25"].ToString(), DataType.String, "NormalLeft");
                              //  xlWorkSheet.WriteCell(excelcolumn, 7, DRitem["A25"].ToString());
                            }
                        }
                        else
                        {
                            foreach (DataColumn dc in dSCaseSnp.Tables[0].Columns)
                            {

                                if (dc.ColumnName.Equals("A1"))
                                {
                                   // xlWorkSheet.WriteCell(excelcolumn, 7, DRitem["A1"].ToString());
                                    cell = excelrow.Cells.Add(DRitem["A1"].ToString(), DataType.String, "NormalLeft");
                                }
                                if (dc.ColumnName.Equals("A2"))
                                {
                                   // xlWorkSheet.WriteCell(excelcolumn, 8, DRitem["A2"].ToString());
                                    cell = excelrow.Cells.Add(DRitem["A2"].ToString(), DataType.String, "NormalLeft");
                                }
                                if (dc.ColumnName.Equals("A3"))
                                {
                                    cell = excelrow.Cells.Add(DRitem["A3"].ToString(), DataType.String, "NormalLeft");
                                   // xlWorkSheet.WriteCell(excelcolumn, 9, DRitem["A3"].ToString());
                                }
                                if (dc.ColumnName.Equals("A4"))
                                {
                                    cell = excelrow.Cells.Add(DRitem["A4"].ToString(), DataType.String, "NormalLeft");
                                   // xlWorkSheet.WriteCell(excelcolumn, 10, DRitem["A4"].ToString());
                                }
                                if (dc.ColumnName.Equals("A5"))
                                {
                                    cell = excelrow.Cells.Add(DRitem["A5"].ToString(), DataType.String, "NormalLeft");
                                   // xlWorkSheet.WriteCell(excelcolumn, 11, DRitem["A5"].ToString());
                                }
                                if (dc.ColumnName.Equals("A6"))
                                {
                                    cell = excelrow.Cells.Add(DRitem["A6"].ToString(), DataType.String, "NormalLeft");
                                   // xlWorkSheet.WriteCell(excelcolumn, 12, DRitem["A6"].ToString());
                                }
                                if (dc.ColumnName.Equals("A7"))
                                {
                                    cell = excelrow.Cells.Add(DRitem["A7"].ToString(), DataType.String, "NormalLeft");
                                   // xlWorkSheet.WriteCell(excelcolumn, 13, DRitem["A7"].ToString());
                                }
                                if (dc.ColumnName.Equals("A8"))
                                {
                                    cell = excelrow.Cells.Add(DRitem["A8"].ToString(), DataType.String, "NormalLeft");
                                 //   xlWorkSheet.WriteCell(excelcolumn, 14, DRitem["A8"].ToString());
                                }
                                if (dc.ColumnName.Equals("A9"))
                                {
                                    cell = excelrow.Cells.Add(DRitem["A9"].ToString(), DataType.String, "NormalLeft"); 
                                   // xlWorkSheet.WriteCell(excelcolumn, 15, DRitem["A9"].ToString());
                                }
                                if (dc.ColumnName.Equals("A10"))
                                {
                                    cell = excelrow.Cells.Add(DRitem["A10"].ToString(), DataType.String, "NormalLeft");
                                   // xlWorkSheet.WriteCell(excelcolumn, 16, DRitem["A10"].ToString());
                                }
                                if (dc.ColumnName.Equals("A11"))
                                {
                                    cell = excelrow.Cells.Add(DRitem["A11"].ToString(), DataType.String, "NormalLeft");
                                  //  xlWorkSheet.WriteCell(excelcolumn, 17, DRitem["A11"].ToString());
                                }
                                if (dc.ColumnName.Equals("A12"))
                                {
                                    cell = excelrow.Cells.Add(DRitem["A12"].ToString(), DataType.String, "NormalLeft");
                                   // xlWorkSheet.WriteCell(excelcolumn, 18, DRitem["A12"].ToString());
                                }
                                if (dc.ColumnName.Equals("A13"))
                                {
                                    cell = excelrow.Cells.Add(DRitem["A13"].ToString(), DataType.String, "NormalLeft");
                                   // xlWorkSheet.WriteCell(excelcolumn, 19, DRitem["A13"].ToString());
                                }
                                if (dc.ColumnName.Equals("A14"))
                                {
                                    cell = excelrow.Cells.Add(DRitem["A14"].ToString(), DataType.String, "NormalLeft");
                                  //  xlWorkSheet.WriteCell(excelcolumn, 20, DRitem["A14"].ToString());
                                }
                                if (dc.ColumnName.Equals("A15"))
                                {
                                    cell = excelrow.Cells.Add(DRitem["A15"].ToString(), DataType.String, "NormalLeft");
                                  //  xlWorkSheet.WriteCell(excelcolumn, 21, DRitem["A15"].ToString());
                                }
                                if (dc.ColumnName.Equals("A16"))
                                {
                                    cell = excelrow.Cells.Add(DRitem["A16"].ToString(), DataType.String, "NormalLeft");
                                   // xlWorkSheet.WriteCell(excelcolumn, 22, DRitem["A16"].ToString());
                                }
                                if (dc.ColumnName.Equals("A17"))
                                {
                                    cell = excelrow.Cells.Add(DRitem["A17"].ToString(), DataType.String, "NormalLeft");
                                  //  xlWorkSheet.WriteCell(excelcolumn, 23, DRitem["A17"].ToString());
                                }
                                if (dc.ColumnName.Equals("A18"))
                                {
                                    cell = excelrow.Cells.Add(DRitem["A18"].ToString(), DataType.String, "NormalLeft");
                                   // xlWorkSheet.WriteCell(excelcolumn, 24, DRitem["A18"].ToString());
                                }
                                if (dc.ColumnName.Equals("A19"))
                                {
                                    cell = excelrow.Cells.Add(DRitem["A19"].ToString(), DataType.String, "NormalLeft");
                                  //  xlWorkSheet.WriteCell(excelcolumn, 25, DRitem["A19"].ToString());
                                }
                                if (dc.ColumnName.Equals("A20"))
                                {
                                    cell = excelrow.Cells.Add(DRitem["A20"].ToString(), DataType.String, "NormalLeft");
                                  //  xlWorkSheet.WriteCell(excelcolumn, 26, DRitem["A20"].ToString());
                                }
                                if (dc.ColumnName.Equals("A21"))
                                {
                                    cell = excelrow.Cells.Add(DRitem["A21"].ToString(), DataType.String, "NormalLeft");
                                   // xlWorkSheet.WriteCell(excelcolumn, 27, DRitem["A21"].ToString());
                                }
                                if (dc.ColumnName.Equals("A22"))
                                {
                                    cell = excelrow.Cells.Add(DRitem["A22"].ToString(), DataType.String, "NormalLeft");
                                    //xlWorkSheet.WriteCell(excelcolumn, 28, DRitem["A22"].ToString());
                                }
                                if (dc.ColumnName.Equals("A23"))
                                {
                                    cell = excelrow.Cells.Add(DRitem["A23"].ToString(), DataType.String, "NormalLeft");
                                  //  xlWorkSheet.WriteCell(excelcolumn, 29, DRitem["A23"].ToString());
                                }
                                if (dc.ColumnName.Equals("A24"))
                                {
                                    cell = excelrow.Cells.Add(DRitem["A24"].ToString(), DataType.String, "NormalLeft");
                                   // xlWorkSheet.WriteCell(excelcolumn, 30, DRitem["A24"].ToString());
                                }
                                if (dc.ColumnName.Equals("A25"))
                                {
                                    cell = excelrow.Cells.Add(DRitem["A25"].ToString(), DataType.String, "NormalLeft");
                                  //  xlWorkSheet.WriteCell(excelcolumn, 31, DRitem["A25"].ToString());
                                }
                            }

                            //if (dSCaseSnp.Tables[0].Columns[].ColumnName.Equals("A1"))
                            //    xlWorkSheet.WriteCell(excelcolumn, 7, DRitem["A1"].ToString());
                            //if (dSCaseSnp.Tables[0].Columns.Equals("A2"))
                            //    xlWorkSheet.WriteCell(excelcolumn, 8, DRitem["A2"].ToString());

                        }
                      //  xlWorkSheet.WriteCell(excelcolumn, (preassessMasterEntitytemp.Count + 7), DRitem["MST_PRESS_TOTAL"].ToString());

                        cell = excelrow.Cells.Add(DRitem["MST_PRESS_TOTAL"].ToString(), DataType.String, "NormalLeft");

                        if (DRitem["MST_PRESS_CAT"].ToString() != string.Empty)
                        {
                            RankCatgEntity preasscategory = propPreassGroup.Find(u => u.Code == DRitem["MST_PRESS_GRP"].ToString() && u.SubCode.Trim() == DRitem["MST_PRESS_CAT"].ToString());
                            if (preasscategory != null)
                            {
                                // xlWorkSheet.WriteCell(excelcolumn, (preassessMasterEntitytemp.Count + 8), preasscategory.Desc.ToString().Trim());
                                cell = excelrow.Cells.Add(preasscategory.Desc.ToString().Trim(), DataType.String, "NormalLeft");
                            }
                            else
                            {
                                cell = excelrow.Cells.Add();
                            }
                        }
                        else
                        {
                            cell = excelrow.Cells.Add();
                        }

                        RankCatgEntity preassgroup = propPreassGroup.Find(u => u.Code.Trim() == DRitem["MST_PRESS_GRP"].ToString() && u.SubCode.Trim() == string.Empty);
                        if (preassgroup != null)
                        {
                           // xlWorkSheet.WriteCell(excelcolumn, (preassessMasterEntitytemp.Count + 9), preassgroup.Desc.ToString().Trim());
                            cell = excelrow.Cells.Add(preassgroup.Desc.ToString().Trim(), DataType.String, "NormalLeft");
                        }
                        else
                        {
                            cell = excelrow.Cells.Add();
                        }

                        cell = excelrow.Cells.Add(LookupDataAccess.Getdate(DRitem["MST_DATE_LSTC_5"].ToString()).Trim(), DataType.String, "NormalLeft");
                      
                       // xlWorkSheet.WriteCell(excelcolumn, (preassessMasterEntitytemp.Count + 10), LookupDataAccess.Getdate(DRitem["MST_DATE_LSTC_5"].ToString()).Trim());

                        //if (((Captain.Common.Utilities.ListItem)cmbPressGroup1.SelectedItem).Value.ToString() != "ALL" && ((Captain.Common.Utilities.ListItem)cmbPressGroup1.SelectedItem).Value.ToString() == "ALL")
                        //{
                        //    List<RankCatgEntity> proppressgroupcategoryalllist = propPreassGroup.FindAll(u => u.Code.Trim() == ((Captain.Common.Utilities.ListItem)cmbPressGroup1.SelectedItem).Value.ToString() && u.SubCode.Trim() != string.Empty && u.PointsLow != string.Empty && u.PointsHigh != string.Empty);
                        //    if (DRitem["MST_PRESS_TOTAL"].ToString() != string.Empty)
                        //    {
                        //        RankCatgEntity preasscategory = proppressgroupcategoryalllist.Find(u => (Convert.ToInt32(u.PointsLow) <= Convert.ToInt32(DRitem["MST_PRESS_TOTAL"])) && (Convert.ToInt32(u.PointsHigh) >= Convert.ToInt32(DRitem["MST_PRESS_TOTAL"])));
                        //        if (preasscategory != null)
                        //        {
                        //            xlWorkSheet.WriteCell(excelcolumn, (preassessMasterEntitytemp.Count + 8), preasscategory.Desc.ToString().Trim());
                        //        }
                        //    }
                        //}
                        //else
                        //{
                        //    if (((Captain.Common.Utilities.ListItem)cmbPressGroup1.SelectedItem).Value.ToString() != "ALL")
                        //    {
                        //        xlWorkSheet.WriteCell(excelcolumn, (preassessMasterEntitytemp.Count + 8), ((Captain.Common.Utilities.ListItem)cmbPressGroup1.SelectedItem).Text.ToString().Trim());
                        //    }

                        //}

                    }

                    sheet =  book.Worksheets.Add("Summary");

                    sheet.Table.DefaultRowHeight = 15.25F;

                    sheet.Table.DefaultColumnWidth = 220.5F;                   
                    sheet.Table.Columns.Add(100);
                    sheet.Table.Columns.Add(200);
                    sheet.Table.Columns.Add(100);
                    sheet.Table.Columns.Add(100);
                    sheet.Table.Columns.Add(120);
                    

                   // excelcolumn = excelcolumn + 3;

                    excelrow = sheet.Table.Rows.Add();

                    //xlWorkSheet[excelcolumn, 1].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    //xlWorkSheet[excelcolumn, 1].Alignment = Alignment.Centered;
                    //xlWorkSheet.WriteCell(excelcolumn, 1, "Category Code");

                    cell = excelrow.Cells.Add("Category Code", DataType.String, "MainHeaderStyles");

                    //xlWorkSheet[excelcolumn, 2].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    //xlWorkSheet[excelcolumn, 2].Alignment = Alignment.Centered;
                    //xlWorkSheet.WriteCell(excelcolumn, 2, "Category Name");

                    cell = excelrow.Cells.Add("Category Name", DataType.String, "MainHeaderStyles");

                    //xlWorkSheet[excelcolumn, 3].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    //xlWorkSheet[excelcolumn, 3].Alignment = Alignment.Centered;
                    //xlWorkSheet.WriteCell(excelcolumn, 3, "Low Points");

                    cell = excelrow.Cells.Add("Low Points", DataType.String, "MainHeaderStyles");

                    //xlWorkSheet[excelcolumn, 4].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    //xlWorkSheet[excelcolumn, 4].Alignment = Alignment.Centered;
                    //xlWorkSheet.WriteCell(excelcolumn, 4, "High Points");

                    cell = excelrow.Cells.Add("High Points", DataType.String, "MainHeaderStyles");

                    //xlWorkSheet[excelcolumn, 5].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                    //xlWorkSheet[excelcolumn, 5].Alignment = Alignment.Centered;
                    //xlWorkSheet.WriteCell(excelcolumn, 5, "Applicant Counts");

                    cell = excelrow.Cells.Add("Applicant Counts", DataType.String, "MainHeaderStyles");

                    List<RankCatgEntity> PreassGroupList = propPreassGroup;
                    if (((Captain.Common.Utilities.ListItem)cmbPressGroup.SelectedItem).Value.ToString() != "ALL")
                        PreassGroupList = propPreassGroup.FindAll(u => u.Code == ((Captain.Common.Utilities.ListItem)cmbPressGroup.SelectedItem).Value.ToString());
                    int inttotalcout = 0;
                    int intsummarloop = 0;
                    foreach (RankCatgEntity item in PreassGroupList)
                    {
                       // excelcolumn = excelcolumn + 1;

                        excelrow = sheet.Table.Rows.Add();

                        if (item.SubCode == string.Empty)
                        {

                            if (intsummarloop > 0)
                            {
                                cell = excelrow.Cells.Add();
                                cell = excelrow.Cells.Add();
                                cell = excelrow.Cells.Add();
                                cell = excelrow.Cells.Add("Total", DataType.String, "MainHeaderStyles");
                                cell = excelrow.Cells.Add(inttotalcout.ToString(), DataType.String, "MainHeaderStyles");
                                excelrow = sheet.Table.Rows.Add();

                            }
                            cell = excelrow.Cells.Add(item.Desc, DataType.String, "Normalcenter");
                            cell.MergeAcross =4;
                           
                            intsummarloop = intsummarloop + 1;
                            inttotalcout = 0;
                            //xlWorkSheet[excelcolumn, 2].Font = new System.Drawing.Font("Tahoma", 10, System.Drawing.FontStyle.Bold);
                            //xlWorkSheet[excelcolumn, 2].Alignment = Alignment.Centered;
                            //xlWorkSheet.WriteCell(excelcolumn, 2, item.Desc);
                        }
                        else
                        {
                          //  xlWorkSheet.WriteCell(excelcolumn, 1, item.SubCode);
                            cell = excelrow.Cells.Add(item.SubCode, DataType.String, "NormalLeft");
                          //  xlWorkSheet.WriteCell(excelcolumn, 2, item.Desc);
                            cell = excelrow.Cells.Add(item.Desc, DataType.String, "NormalLeft");
                         //   xlWorkSheet.WriteCell(excelcolumn, 3, item.PointsLow);
                            cell = excelrow.Cells.Add(item.PointsLow, DataType.String, "NormalLeft");
                        //    xlWorkSheet.WriteCell(excelcolumn, 4, item.PointsHigh);
                            cell = excelrow.Cells.Add(item.PointsHigh, DataType.String, "NormalLeft");                          
                            DataRow[] results = dSCaseSnp.Tables[0].Select("MST_PRESS_GRP = '" + item.Code.Trim() + "' AND MST_PRESS_CAT = '" + item.SubCode.Trim() + "'");
                        //    xlWorkSheet.WriteCell(excelcolumn, 5, results.Length);
                            cell = excelrow.Cells.Add(results.Length.ToString(), DataType.String, "NormalLeft");
                            inttotalcout = inttotalcout + results.Length;
                        }

                    }
                    excelrow = sheet.Table.Rows.Add();
                    cell = excelrow.Cells.Add();
                    cell = excelrow.Cells.Add();
                    cell = excelrow.Cells.Add();
                    cell = excelrow.Cells.Add("Total", DataType.String, "MainHeaderStyles");
                    cell = excelrow.Cells.Add(inttotalcout.ToString(), DataType.String, "MainHeaderStyles");
                   

                    //List<CustomQuestionsEntity> customdimension = _model.CaseMstData.GETDIMSCORE(Agency, Depart, Program, Program_Year, strApplicantNo, "SNP", string.Empty, txtfrom.Text, txtTo.Text);
                    //var varApplicantlist = customdimension.GroupBy(u => u.ACTAPPNO).ToList();
                    //List<CustomQuestionsEntity> custdimesionApplicantData = null;
                    //foreach (var ApplicantData in varApplicantlist)
                    //{
                    //    custdimesionApplicantData = customdimension.FindAll(u => u.ACTAPPNO == ApplicantData.Key.ToString());
                    //    custdimesionApplicantData = custdimesionApplicantData.OrderBy(u => Convert.ToInt32(u.ACTCODE)).ToList();
                    //    intcolumn = 7;

                    //    int intpoints = 0;
                    //    foreach (CustomQuestionsEntity custApplicantData in custdimesionApplicantData)
                    //    {
                    //        if (intpoints == 0)
                    //        {
                    //            excelcolumn = excelcolumn + 1;
                    //            xlWorkSheet.WriteCell(excelcolumn, 1, custApplicantData.ACTAGENCY);
                    //            xlWorkSheet.WriteCell(excelcolumn, 2, custApplicantData.ACTDEPT);
                    //            xlWorkSheet.WriteCell(excelcolumn, 3, custApplicantData.ACTPROGRAM);
                    //            xlWorkSheet.WriteCell(excelcolumn, 4, custApplicantData.ACTYEAR);
                    //            xlWorkSheet.WriteCell(excelcolumn, 5, custApplicantData.ACTAPPNO);
                    //            xlWorkSheet.WriteCell(excelcolumn, 6, LookupDataAccess.GetMemberName(custApplicantData.FirstName, custApplicantData.MIName, custApplicantData.LastName, Member_NameFormat));
                    //            xlWorkSheet.WriteCell(excelcolumn, (intcolumn), custApplicantData.PRESPOINTS);
                    //            xlWorkSheet.WriteCell(excelcolumn, (preassessMasterEntity.Count + 7), custApplicantData.Appcount);
                    //            intpoints = intpoints + 1;
                    //            intcolumn = intcolumn + 1;
                    //        }
                    //        else
                    //        {
                    //            xlWorkSheet.WriteCell(excelcolumn, intcolumn , custApplicantData.PRESPOINTS);
                    //            intcolumn = intcolumn + 1;
                    //        }
                    //    }

                    //}
                    //FileStream stream = new FileStream(PdfName, FileMode.Create);

                    //xlWorkSheet.Save(stream);
                    //stream.Close();

                    FileStream stream1 = new FileStream( PdfName, FileMode.Create);
                    book.Save(stream1);
                    stream1.Close();
                    AlertBox.Show("Report Generated Successfully");

                }
                catch (Exception ex) { }

            }
        }

        private void BtnPdfPrev_Click(object sender, EventArgs e)
        {
            PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, true, propReportPath);
            pdfListForm.StartPosition = FormStartPosition.CenterScreen;
            pdfListForm.ShowDialog();
        }

        private void btnSaveParameters_Click(object sender, EventArgs e)
        {
            //if (ValidateForm())
            //{
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
            //}
        }
        private string Get_XML_Format_for_Report_Controls()
        {



            string strFundSource = string.Empty;
            string From = string.Empty;
            string To = string.Empty;
            string RType = string.Empty;
            string strApplicant = string.Empty;

            From = txtfrom.Text;
            To = txtTo.Text;
            if (rdoall.Checked)
                RType = "A";
            else if (rdoSelectApplicant.Checked)
                RType = "S";
            if (txtApplicant.Text != string.Empty)
                strApplicant = txtApplicant.Text;
            //string strFundingCodes = string.Empty;
            //if (rbFundSel.Checked == true)
            //{
            //    foreach (CommonEntity FundingCode in SelFundingList)
            //    {
            //        if (!strFundingCodes.Equals(string.Empty)) strFundingCodes += ",";
            //        strFundingCodes += FundingCode.Code;
            //    }
            //}
            string ChkFrmDte = dtpFrmDate.Checked == true ? "Y" : "N";
            string ChkToDte = dtpToDt.Checked == true ? "Y" : "N";
            StringBuilder str = new StringBuilder();
            if (dtpFrmDate.Checked == true && dtpToDt.Checked == true)
            {
                str.Append("<Rows>");
                str.Append("<Row AGENCY = \"" + Agency + "\" DEPT = \"" + Depart + "\" PROG = \"" + Program +
                                "\" YEAR = \"" + Program_Year + "\" From = \"" + From + "\" To = \"" + To + "\" RType = \"" + RType + "\" strApplicant = \"" + strApplicant +
                                "\" DIMCODE = \"" + ((Captain.Common.Utilities.ListItem)cmbdimension.SelectedItem).Value.ToString() +
                                "\" PREGRP = \"" + ((Captain.Common.Utilities.ListItem)cmbPressGroup.SelectedItem).Value.ToString() +
                                "\" PRECAT = \"" + ((Captain.Common.Utilities.ListItem)cmbPressgcat.SelectedItem).Value.ToString() +
                                "\" CFRMDTE = \"" + ChkFrmDte + "\" CTODATE = \"" + ChkToDte +
                                "\" FRMDTE = \"" + dtpFrmDate.Value.Date + "\" TODATE = \"" + dtpToDt.Value.Date + "\" ALERTCODES = \"" + (rdbAlrtCde_All.Checked == true ? "A" : "S") + "\" SELECTEDALERTCODES = \"" + SelectedAlertCodes +
                                "\" />");
                str.Append("</Rows>");
            }
            else
            {
                str.Append("<Rows>");
                str.Append("<Row AGENCY = \"" + Agency + "\" DEPT = \"" + Depart + "\" PROG = \"" + Program +
                                "\" YEAR = \"" + Program_Year + "\" From = \"" + From + "\" To = \"" + To + "\" RType = \"" + RType + "\" strApplicant = \"" + strApplicant +
                                "\" DIMCODE = \"" + ((Captain.Common.Utilities.ListItem)cmbdimension.SelectedItem).Value.ToString() +
                                "\" PREGRP = \"" + ((Captain.Common.Utilities.ListItem)cmbPressGroup.SelectedItem).Value.ToString() +
                                "\" PRECAT = \"" + ((Captain.Common.Utilities.ListItem)cmbPressgcat.SelectedItem).Value.ToString() +
                                "\" CFRMDTE = \"" + ChkFrmDte + "\" CTODATE = \"" + ChkToDte +
                                "\" FRMDTE = \"" + dtpFrmDate.Value.Date + "\" TODATE = \"" + dtpToDt.Value.Date + "\" ALERTCODES = \"" + (rdbAlrtCde_All.Checked == true ? "A" : "S") + "\" SELECTEDALERTCODES = \"" + SelectedAlertCodes +
                                "\" />");
                str.Append("</Rows>");
            }
            return str.ToString();
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
                DataColumnCollection columns = Tmp_Table.Columns;

                Set_Report_Hierarchy(dr["AGENCY"].ToString(), dr["DEPT"].ToString(), dr["PROG"].ToString(), dr["YEAR"].ToString());


                txtfrom.Text = dr["From"].ToString().Trim();
                txtTo.Text = dr["To"].ToString().Trim();
                if (dr["RType"].ToString().Trim() == "A")
                {
                    txtApplicant.Enabled = false;
                    btnBrowse.Enabled = false;
                    txtApplicant.Text = "";

                    rdoall.Checked = true;
                    rdoSelectApplicant.Checked = false;
                }
                else
                {
                    txtApplicant.Enabled = true;
                    btnBrowse.Enabled = true;
                    rdoall.Checked = false;
                    rdoSelectApplicant.Checked = true;
                    if (dr["strApplicant"].ToString().Trim() != "")
                    {
                        txtApplicant.Text = dr["strApplicant"].ToString().Trim();
                    }
                }
                CommonFunctions.SetComboBoxValue(cmbPressGroup, dr["PREGRP"].ToString().Trim());
                CommonFunctions.SetComboBoxValue(cmbPressgcat, dr["PRECAT"].ToString().Trim());
                CommonFunctions.SetComboBoxValue(cmbdimension, dr["DIMCODE"].ToString().Trim());
                dtpFrmDate.Checked = dr["CFRMDTE"].ToString() == "Y" ? true : false;
                dtpToDt.Checked = dr["CTODATE"].ToString() == "Y" ? true : false;
                dtpFrmDate.Value = Convert.ToDateTime(dr["FRMDTE"]);
                dtpToDt.Value = Convert.ToDateTime(dr["TODATE"]);

                if (columns.Contains("ALERTCODES"))
                {
                    if (dr["ALERTCODES"].ToString() == "A")
                        rdbAlrtCde_All.Checked = true;
                    else if (dr["ALERTCODES"].ToString() == "S")
                        rdbAlrtCde_Sel.Checked = true;
                }
                else
                    rdbAlrtCde_All.Checked = true;

                if (columns.Contains("SELECTEDALERTCODES"))
                {
                    if (dr["SELECTEDALERTCODES"].ToString() != "")
                    {
                        Get_Sel_AlertCodes(dr["SELECTEDALERTCODES"].ToString());
                    }
                }
            }
        }

        private string Get_Sel_AlertCodes(string alertCodes)
        {
            SelectedAlertCodes = alertCodes;

            return SelectedAlertCodes;
        }
        private void CmbYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program_Year = "    ";
            if (!(string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString())))
                Program_Year = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString();
        }

        private void Pb_Search_Hie_Click(object sender, EventArgs e)
        {
            //HierarchieSelectionFormNew hierarchieSelectionForm = new HierarchieSelectionFormNew(BaseForm, Current_Hierarchy_DB, "Master", string.Empty, "*", "Reports");
            //hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
            //hierarchieSelectionForm.StartPosition = FormStartPosition.CenterScreen;
            //hierarchieSelectionForm.ShowDialog();

            HierarchieSelection hierarchieSelectionForm = new HierarchieSelection(BaseForm, Current_Hierarchy_DB, "Master", string.Empty, "*", "Reports", BaseForm.UserID);
            hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
            hierarchieSelectionForm.StartPosition = FormStartPosition.CenterScreen;
            hierarchieSelectionForm.ShowDialog();
        }

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
                    Depart = hierarchy.Substring(2, 2);
                    Program = hierarchy.Substring(4, 2);


                    if (Agency == "**" || Depart == "**" || Program == "**")
                    {
                        rdoall.Checked = true;
                        rdoSelectApplicant_CheckedChanged(sender, e);
                        rdoSelectApplicant.Enabled = false;
                    }
                    else
                    {
                        rdoSelectApplicant.Enabled = true;
                    }

                }
            }
        }

        private void txtApplicant_Leave(object sender, EventArgs e)
        {
            if (txtApplicant.Text.Trim() != string.Empty)
            {
                txtApplicant.Text = SetLeadingZeros(txtApplicant.Text);
                List<CaseMstEntity> caseMstList = _model.CaseMstData.GetCaseMstadpyn(Agency, Depart, Program, Program_Year, txtApplicant.Text);
                if (caseMstList.Count == 0)
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
        private void rdoall_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void rdoSelectApplicant_CheckedChanged(object sender, EventArgs e)
        {
            _errorProvider.SetError(txtApplicant, null);
            if (rdoSelectApplicant.Checked == true)
            {
                txtApplicant.Enabled = true;
                btnBrowse.Enabled = true;
                lblApplicantReq.Visible = true;
            }
            else
            {
                txtApplicant.Clear();
                txtApplicant.Enabled = false;
                btnBrowse.Enabled = false;
                lblApplicantReq.Visible = false;

            }
        }

        private void rdbAlrtCde_All_Click(object sender, EventArgs e)
        {
            SelectedAlertCodes = string.Empty;
            _errorProvider.SetError(rdbAlrtCde_Sel, null);
        }

        private void rdbAlrtCde_Sel_Click(object sender, EventArgs e)
        {
            AlertCodeForm objform = new AlertCodeForm(BaseForm, Privileges, SelectedAlertCodes);
            objform.StartPosition = FormStartPosition.CenterScreen;
            objform.FormClosed += new FormClosedEventHandler(objform_FormClosed);
            objform.ShowDialog();
        }

        string SelectedAlertCodes = string.Empty;
        void objform_FormClosed(object sender, FormClosedEventArgs e)
        {
            AlertCodeForm form = sender as AlertCodeForm;
            if (form.DialogResult == DialogResult.OK)
            {
                SelectedAlertCodes = form.fillAlertCodesforReport();
            }
            else
                form.Close();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            BrowseApplicantForm BrowseApplcantForm = new BrowseApplicantForm(BaseForm, string.Empty, Privileges, Agency, Depart, Program, Program_Year);
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

        private void cmbPressgcat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((Captain.Common.Utilities.ListItem)cmbPressgcat.SelectedItem).Value.ToString() == "ALL")
            {
                txtfrom.Text = string.Empty;
                txtTo.Text = string.Empty;
                txtTo.Enabled = true;
                txtfrom.Enabled = true;
            }

            else
            {
                txtTo.Enabled = false;
                txtfrom.Enabled = false;
                RankCatgEntity preasscategory = propPreassGroup.Find(u => u.Code == ((Captain.Common.Utilities.ListItem)cmbPressGroup.SelectedItem).Value.ToString() && u.SubCode.Trim() == ((Captain.Common.Utilities.ListItem)cmbPressgcat.SelectedItem).Value.ToString());
                if (preasscategory != null)
                {
                    txtfrom.Text = preasscategory.PointsLow;
                    txtTo.Text = preasscategory.PointsHigh;
                }
            }
        }

        private void cmbPressGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbPressgcat.SelectedIndexChanged -= new EventHandler(cmbPressgcat_SelectedIndexChanged);
            if (((Captain.Common.Utilities.ListItem)cmbPressGroup.SelectedItem).Value.ToString() != "ALL")
            {
                cmbPressgcat.Items.Clear();
                List<RankCatgEntity> pressgroupcategorylist = propPreassGroup.FindAll(u => u.Code == ((Captain.Common.Utilities.ListItem)cmbPressGroup.SelectedItem).Value.ToString() && u.SubCode.Trim() != string.Empty && u.PointsLow != string.Empty && u.PointsHigh != string.Empty);
                foreach (RankCatgEntity item in pressgroupcategorylist)
                {
                    cmbPressgcat.Items.Add(new Captain.Common.Utilities.ListItem(item.Desc.ToString().Trim(), item.SubCode.ToString()));
                }
                cmbPressgcat.Items.Insert(0, new Captain.Common.Utilities.ListItem("ALL", "ALL"));
                cmbPressgcat.SelectedIndex = 0;
            }
            else
            {
                cmbPressgcat.Items.Clear();
                cmbPressgcat.Items.Insert(0, new Captain.Common.Utilities.ListItem("ALL", "ALL"));
                cmbPressgcat.SelectedIndex = 0;
            }
            cmbPressgcat_SelectedIndexChanged(sender, e);
            cmbPressgcat.SelectedIndexChanged += new EventHandler(cmbPressgcat_SelectedIndexChanged);
        }

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
            // -----------------------------------------------
            //  s17
            // -----------------------------------------------
            WorksheetStyle s17 = styles.Add("s17");
            s17.NumberFormat = "0%";
            // -----------------------------------------------
            //  s18
            // -----------------------------------------------
            WorksheetStyle s18 = styles.Add("s18");
            // -----------------------------------------------
            //  s19
            // -----------------------------------------------
            WorksheetStyle s19 = styles.Add("s19");
            s19.Font.FontName = "Arial";
            // -----------------------------------------------
            //  s20
            // -----------------------------------------------
            WorksheetStyle s20 = styles.Add("s20");
            s20.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s20.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s21
            // -----------------------------------------------
            WorksheetStyle s21 = styles.Add("s21");
            s21.Font.Bold = true;
            s21.Font.FontName = "Arial";
            s21.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s21.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s21.NumberFormat = "0%";
            // -----------------------------------------------
            //  s23
            // -----------------------------------------------
            WorksheetStyle s23 = styles.Add("s23");
            s23.Font.Bold = true;
            s23.Font.FontName = "Calibri";
            s23.Font.Size = 11;
            s23.Font.Color = "#000000";
            // -----------------------------------------------
            //  s24
            // -----------------------------------------------
            WorksheetStyle s24 = styles.Add("s24");
            s24.Interior.Color = "#D8D8D8";
            s24.Interior.Pattern = StyleInteriorPattern.Solid;
            // -----------------------------------------------
            //  s25
            // -----------------------------------------------
            WorksheetStyle s25 = styles.Add("s25");
            s25.Font.FontName = "Arial";
            s25.Interior.Color = "#D8D8D8";
            s25.Interior.Pattern = StyleInteriorPattern.Solid;
            // -----------------------------------------------
            //  s26
            // -----------------------------------------------
            WorksheetStyle s26 = styles.Add("s26");
            s26.Interior.Color = "#D8D8D8";
            s26.Interior.Pattern = StyleInteriorPattern.Solid;
            s26.NumberFormat = "0%";
            // -----------------------------------------------
            //  s27
            // -----------------------------------------------
            WorksheetStyle s27 = styles.Add("s27");
            s27.Interior.Color = "#D8D8D8";
            s27.Interior.Pattern = StyleInteriorPattern.Solid;
            s27.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s27.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s28
            // -----------------------------------------------
            WorksheetStyle s28 = styles.Add("s28");
            s28.Font.Bold = true;
            s28.Font.FontName = "Arial";
            s28.Interior.Color = "#D8D8D8";
            s28.Interior.Pattern = StyleInteriorPattern.Solid;
            s28.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s28.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s28.NumberFormat = "0%";
            // -----------------------------------------------
            //  s62
            // -----------------------------------------------
            WorksheetStyle s62 = styles.Add("s62");
            s62.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
            s62.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s62.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#ABABAB");
            s62.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            // -----------------------------------------------
            //  s63
            // -----------------------------------------------
            WorksheetStyle s63 = styles.Add("s63");
            s63.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s63.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            // -----------------------------------------------
            //  s64
            // -----------------------------------------------
            WorksheetStyle s64 = styles.Add("s64");
            s64.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Background");
            s64.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            // -----------------------------------------------
            //  s65
            // -----------------------------------------------
            WorksheetStyle s65 = styles.Add("s65");
            s65.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Background");
            s65.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#ABABAB");
            s65.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            // -----------------------------------------------
            //  s66
            // -----------------------------------------------
            WorksheetStyle s66 = styles.Add("s66");
            s66.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            // -----------------------------------------------
            //  s67
            // -----------------------------------------------
            WorksheetStyle s67 = styles.Add("s67");
            s67.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s67.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#ABABAB");
            s67.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            // -----------------------------------------------
            //  s68
            // -----------------------------------------------
            WorksheetStyle s68 = styles.Add("s68");
            s68.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s68.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s68.NumberFormat = "0%";
            // -----------------------------------------------
            //  s69
            // -----------------------------------------------
            WorksheetStyle s69 = styles.Add("s69");
            s69.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s69.NumberFormat = "0%";
            // -----------------------------------------------
            //  s70
            // -----------------------------------------------
            WorksheetStyle s70 = styles.Add("s70");
            s70.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s70.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#ABABAB");
            s70.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s70.NumberFormat = "0%";
            // -----------------------------------------------
            //  s71
            // -----------------------------------------------
            WorksheetStyle s71 = styles.Add("s71");
            s71.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            // -----------------------------------------------
            //  s72
            // -----------------------------------------------
            WorksheetStyle s72 = styles.Add("s72");
            s72.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s72.NumberFormat = "0%";
            // -----------------------------------------------
            //  s73
            // -----------------------------------------------
            WorksheetStyle s73 = styles.Add("s73");
            s73.NumberFormat = "0%";
            // -----------------------------------------------
            //  s74
            // -----------------------------------------------
            WorksheetStyle s74 = styles.Add("s74");
            s74.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s74.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#ABABAB");
            s74.NumberFormat = "0%";
            // -----------------------------------------------
            //  s75
            // -----------------------------------------------
            WorksheetStyle s75 = styles.Add("s75");
            s75.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
            s75.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s75.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            // -----------------------------------------------
            //  s76
            // -----------------------------------------------
            WorksheetStyle s76 = styles.Add("s76");
            s76.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
            s76.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s76.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s76.NumberFormat = "0%";
            // -----------------------------------------------
            //  s77
            // -----------------------------------------------
            WorksheetStyle s77 = styles.Add("s77");
            s77.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
            s77.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s77.NumberFormat = "0%";
            // -----------------------------------------------
            //  s78
            // -----------------------------------------------
            WorksheetStyle s78 = styles.Add("s78");
            s78.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
            s78.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s78.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#ABABAB");
            s78.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s78.NumberFormat = "0%";
            // -----------------------------------------------
            //  s79
            // -----------------------------------------------
            WorksheetStyle s79 = styles.Add("s79");
            s79.Font.Bold = true;
            s79.Font.FontName = "Arial";
            s79.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s79.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s81
            // -----------------------------------------------
            WorksheetStyle s81 = styles.Add("s81");
            s81.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s82
            // -----------------------------------------------
            WorksheetStyle s82 = styles.Add("s82");
            s82.Font.Bold = true;
            s82.Font.FontName = "Arial";
            s82.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s82.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s82.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
            s82.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#ABABAB");
            s82.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s82.NumberFormat = "0%";
            // -----------------------------------------------
            //  s84
            // -----------------------------------------------
            WorksheetStyle s84 = styles.Add("s84");
            s84.Font.Bold = true;
            s84.Font.FontName = "Arial";
            s84.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s84.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s84.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
            s84.NumberFormat = "0%";
            // -----------------------------------------------
            //  s86
            // -----------------------------------------------
            WorksheetStyle s86 = styles.Add("s86");
            s86.Font.Bold = true;
            s86.Font.FontName = "Arial";
            s86.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s86.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s86.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#ABABAB");
            s86.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s86.NumberFormat = "0%";
            // -----------------------------------------------
            //  s87
            // -----------------------------------------------
            WorksheetStyle s87 = styles.Add("s87");
            s87.Font.Bold = true;
            s87.Font.FontName = "Arial";
            s87.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s87.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s87.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#ABABAB");
            s87.NumberFormat = "0%";
            // -----------------------------------------------
            //  s90
            // -----------------------------------------------
            WorksheetStyle s90 = styles.Add("s90");
            s90.Font.Bold = true;
            s90.Font.FontName = "Arial";
            s90.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s90.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s90.NumberFormat = "0%";
            // -----------------------------------------------
            //  s92
            // -----------------------------------------------
            WorksheetStyle s92 = styles.Add("s92");
            s92.Font.Bold = true;
            s92.Font.Italic = true;
            s92.Font.FontName = "Arial";
            s92.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s92.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s92.NumberFormat = "0%";
            // -----------------------------------------------
            //  s93
            // -----------------------------------------------
            WorksheetStyle s93 = styles.Add("s93");
            s93.Font.Bold = true;
            s93.Font.Italic = true;
            s93.Font.FontName = "Arial";
            s93.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s93.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s94
            // -----------------------------------------------
            WorksheetStyle s94 = styles.Add("s94");
            s94.Font.Bold = true;
            s94.Font.FontName = "Arial";
            s94.Font.Color = "#000000";
            s94.Interior.Color = "#B0C4DE";
            s94.Interior.Pattern = StyleInteriorPattern.Solid;
            s94.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s94.Alignment.Vertical = StyleVerticalAlignment.Top;
            s94.Alignment.WrapText = true;
            s94.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s94.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            s94.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            s94.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            s94.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s95
            // -----------------------------------------------
            WorksheetStyle s95 = styles.Add("s95");
            s95.Font.FontName = "Arial";
            s95.Font.Color = "#000000";
            s95.Interior.Color = "#FFFFFF";
            s95.Interior.Pattern = StyleInteriorPattern.Solid;
            s95.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            s95.Alignment.Vertical = StyleVerticalAlignment.Top;
            s95.Alignment.WrapText = true;
            s95.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            // -----------------------------------------------
            //  s96
            // -----------------------------------------------
            WorksheetStyle s96 = styles.Add("s96");
            s96.Font.FontName = "Arial";
            s96.Font.Color = "#000000";
            s96.Interior.Color = "#FFFFFF";
            s96.Interior.Pattern = StyleInteriorPattern.Solid;
            s96.Alignment.Vertical = StyleVerticalAlignment.Top;
            s96.Alignment.WrapText = true;
            s96.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;

            // -----------------------------------------------
            //  s97
            // -----------------------------------------------
            WorksheetStyle s97 = styles.Add("s97");
            s97.Font.Bold = true;
            s97.Font.FontName = "Arial";
            s97.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s97.Alignment.Vertical = StyleVerticalAlignment.Center;
            s97.NumberFormat = "0%";
            // -----------------------------------------------
            //  s98
            // -----------------------------------------------
            WorksheetStyle s98 = styles.Add("s98");
            s98.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#A5A5A5");
            s98.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#A5A5A5");
            s98.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#A5A5A5");
            s98.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#A5A5A5");
            // -----------------------------------------------
            //  s99
            // -----------------------------------------------
            WorksheetStyle s99 = styles.Add("s99");
            s99.Font.Bold = true;
            s99.Font.FontName = "Arial";
            s99.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#A5A5A5");
            s99.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#A5A5A5");
            s99.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#A5A5A5");
            s99.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#A5A5A5");
            // -----------------------------------------------
            //  s100
            // -----------------------------------------------
            WorksheetStyle s100 = styles.Add("s100");
            s100.Font.Bold = true;
            s100.Font.FontName = "Arial";
            s100.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s100.Alignment.Vertical = StyleVerticalAlignment.Center;
            s100.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#A5A5A5");
            s100.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#A5A5A5");
            s100.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#A5A5A5");
            s100.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#A5A5A5");
            s100.NumberFormat = "0%";
            // -----------------------------------------------
            //  s101
            // -----------------------------------------------
            WorksheetStyle s101 = styles.Add("s101");
            s101.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#A5A5A5");
            s101.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#A5A5A5");
            s101.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#A5A5A5");
            s101.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#A5A5A5");
            s101.NumberFormat = "0%";
            // -----------------------------------------------
            //  s102
            // -----------------------------------------------
            WorksheetStyle s102 = styles.Add("s102");
            s102.Font.Bold = true;
            s102.Font.FontName = "Arial";
            s102.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s102.Alignment.Vertical = StyleVerticalAlignment.Center;
            s102.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#A5A5A5");
            s102.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#A5A5A5");
            s102.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#A5A5A5");
            s102.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#A5A5A5");
            s102.NumberFormat = "0%";
            // -----------------------------------------------
            //  s103
            // -----------------------------------------------
            WorksheetStyle s103 = styles.Add("s103");
            s103.Font.Bold = true;
            s103.Font.FontName = "Arial";
            s103.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s103.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s103.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#A5A5A5");
            s103.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#A5A5A5");
            s103.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#A5A5A5");
            s103.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#A5A5A5");
            s103.NumberFormat = "0%";
            // -----------------------------------------------
            //  s104
            // -----------------------------------------------
            WorksheetStyle s104 = styles.Add("s104");
            s104.Font.FontName = "Arial";
            // -----------------------------------------------
            //  s105
            // -----------------------------------------------
            WorksheetStyle s105 = styles.Add("s105");
            // -----------------------------------------------
            //  s106
            // -----------------------------------------------
            WorksheetStyle s106 = styles.Add("s106");
            s106.NumberFormat = "0%";
            // -----------------------------------------------
            //  s107
            // -----------------------------------------------
            WorksheetStyle s107 = styles.Add("s107");
            s107.Font.FontName = "Arial";
            // -----------------------------------------------
            //  s108
            // -----------------------------------------------
            WorksheetStyle s108 = styles.Add("s108");
            s108.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#A5A5A5");
            s108.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#A5A5A5");
            s108.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#A5A5A5");
            s108.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#A5A5A5");
            s108.NumberFormat = "0%";
        }

        List<CommonEntity> alertcodeList = new List<CommonEntity>();
        string alertcodeName = string.Empty;
        private void On_SaveFormClosed_DevExpress(object sender, FormClosedEventArgs e)
        {
            PdfListForm form = sender as PdfListForm;
            if (form.DialogResult == DialogResult.OK)
            {
                #region FileNameBuild
                /*************************************************************/
                Random_Filename = null;
                string xlFileName = form.GetFileName();
                xlFileName = propReportPath + BaseForm.UserID + "\\" + xlFileName;
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

                /*************************************************************/
                #endregion

                try
                {
                    int _RowIndex = 0;
                    int _ColumnIndex = 0;

                    using (DevExpress.Spreadsheet.Workbook wb = new DevExpress.Spreadsheet.Workbook())
                    {
                        DevExpress_Excel_Properties oDevExpress_Excel_Properties = new DevExpress_Excel_Properties();
                        oDevExpress_Excel_Properties.sxlbook = wb;
                        oDevExpress_Excel_Properties.sxlTitleFont = "calibri";
                        oDevExpress_Excel_Properties.sxlbodyFont = "calibri";

                        oDevExpress_Excel_Properties.getDevexpress_Excel_Properties();

                        PrintHeaderPage_Excel(wb, oDevExpress_Excel_Properties);

                        #region Excel Data Printing

                        string strApplicantNo = string.Empty;
                        if (rdoSelectApplicant.Checked)
                            strApplicantNo = txtApplicant.Text;

                        string alertcodes = "A";
                        if (rdbAlrtCde_Sel.Checked)
                            alertcodes = SelectedAlertCodes.Replace("'", "");

                        string strDimensioncode = string.Empty;
                        string strGroupcode = string.Empty;
                        string strFromdate = string.Empty;
                        string strTodate = string.Empty;

                        if (((Captain.Common.Utilities.ListItem)cmbPressGroup.SelectedItem).Value.ToString() != "ALL")
                            strGroupcode = ((Captain.Common.Utilities.ListItem)cmbPressGroup.SelectedItem).Value.ToString();

                        if (dtpFrmDate.Checked == true && dtpToDt.Checked == true)
                        {
                            strFromdate = dtpFrmDate.Value.ToShortDateString();
                            strTodate = dtpToDt.Value.ToShortDateString();
                        }

                        if (((Captain.Common.Utilities.ListItem)cmbdimension.SelectedItem).Value.ToString() != "ALL")
                            strDimensioncode = ((Captain.Common.Utilities.ListItem)cmbdimension.SelectedItem).Value.ToString();
                        DataSet dSCaseSnp = Captain.DatabaseLayer.CaseSnpData.GETDIMSCORE(Agency == "**" ? string.Empty : Agency, Depart == "**" ? string.Empty : Depart, Program == "**" ? string.Empty : Program, Program_Year.Trim(), strApplicantNo, "SNP", string.Empty, txtfrom.Text, txtTo.Text, strDimensioncode, strGroupcode, strFromdate, strTodate, alertcodes);


                        List<CommonEntity> listCodes = _model.lookupDataAccess.GetAlertCodes();
                        listCodes = filterByHIE(listCodes);

                        #region Data Sheet

                        DevExpress.Spreadsheet.Worksheet SheetDetails = wb.Worksheets.Add("Data");
                        SheetDetails.ActiveView.TabColor = ColorTranslator.FromHtml("#ADD8E6");

                        SheetDetails.ActiveView.ShowGridlines = false;
                        wb.Unit = DevExpress.Office.DocumentUnit.Point;

                        #region Column Widths

                        SheetDetails.Columns[0].Width = 60;//Agency
                        SheetDetails.Columns[1].Width = 80;//Department
                        SheetDetails.Columns[2].Width = 60;//Program
                        SheetDetails.Columns[3].Width = 60;//Year
                        SheetDetails.Columns[4].Width = 80;//Applicant No
                        SheetDetails.Columns[5].Width = 120;//Name

                        List<CommonEntity> preassessMasterEntitytemp = new List<CommonEntity>();
                        if (((Captain.Common.Utilities.ListItem)cmbdimension.SelectedItem).Value.ToString() == "ALL")
                            preassessMasterEntitytemp = preassessMasterEntity.OrderBy(U => Convert.ToInt32(U.Code)).ToList();
                        else
                            preassessMasterEntitytemp = preassessMasterEntity.FindAll(u => u.Code == ((Captain.Common.Utilities.ListItem)cmbdimension.SelectedItem).Value.ToString());

                        int intcolumn = 6;
                        foreach (CommonEntity preassesdata in preassessMasterEntitytemp)
                        {
                            SheetDetails.Columns[intcolumn].Width = 90;//Employment

                            intcolumn++;
                        }

                        //SheetDetails.Columns[7].Width = 80;//Education
                        //SheetDetails.Columns[8].Width = 80;//Training
                        //SheetDetails.Columns[9].Width = 80;//Housing
                        //SheetDetails.Columns[10].Width = 80;//Transportation
                        //SheetDetails.Columns[11].Width = 80;//Child Care
                        //SheetDetails.Columns[12].Width = 80;//Child Care Enrollment
                        //SheetDetails.Columns[13].Width = 80;//Elder care
                        //SheetDetails.Columns[14].Width = 80;//Eligible Meals
                        //SheetDetails.Columns[15].Width = 80;//DSS Programs
                        //SheetDetails.Columns[16].Width = 80;//Income
                        //SheetDetails.Columns[17].Width = 80;//Child Health Ins.
                        
                        SheetDetails.Columns[intcolumn].Width = 80;//Applicant Total
                        intcolumn++;
                        SheetDetails.Columns[intcolumn].Width = 100;//Category Name
                        intcolumn++;
                        SheetDetails.Columns[intcolumn].Width = 100;//Group Name
                        intcolumn++;
                        SheetDetails.Columns[intcolumn].Width = 80;//Last Update Date
                        intcolumn++;
                        SheetDetails.Columns[intcolumn].Width = 120;//Assigned Worker
                        intcolumn++;
                        SheetDetails.Columns[intcolumn].Width = 110;//Site
                        intcolumn++;
                        SheetDetails.Columns[intcolumn].Width = 80;//Alert Code(s)

                        #endregion

                        #region Column Headers

                        _ColumnIndex = 0;

                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = "Agency";
                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlBLHC;
                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Font.Size = 11;
                        _ColumnIndex++;

                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = "Department";
                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlBLHC;
                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Font.Size = 11;
                        _ColumnIndex++;

                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = "Program";
                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlBLHC;
                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Font.Size = 11;
                        _ColumnIndex++;

                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = "Year";
                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlBLHC;
                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Font.Size = 11;
                        //SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                        _ColumnIndex++;

                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = "Applicant No";
                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlBLHC;
                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Font.Size = 11;
                        _ColumnIndex++;

                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = "Name";
                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlBLHC;
                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Font.Size = 11;
                        _ColumnIndex++;

                        intcolumn = 6;

                        foreach (CommonEntity preassesdata in preassessMasterEntitytemp)
                        {
                            //_ColumnIndex = intcolumn;

                            SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = preassesdata.Desc;
                            SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlBLHC;
                            SheetDetails.Rows[_RowIndex][_ColumnIndex].Font.Size = 11;
                            _ColumnIndex++;

                            //intcolumn++;
                        }

                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = "Applicant Total";
                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlBLHC;
                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Font.Size = 11;
                        _ColumnIndex++;

                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = "Category Name";
                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlBLHC;
                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Font.Size = 11;
                        _ColumnIndex++;

                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = "Group Name";
                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlBLHC;
                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Font.Size = 11;
                        _ColumnIndex++;

                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = "Last Update Date";
                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlBLHC;
                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Font.Size = 11;
                        _ColumnIndex++;

                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = "Assigned Worker";
                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlBLHC;
                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Font.Size = 11;
                        _ColumnIndex++;

                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = "Site";
                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlBLHC;
                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Font.Size = 11;
                        _ColumnIndex++;

                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = "Alert Code(s)";
                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlBLHC;
                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Font.Size = 11;
                        _ColumnIndex++;

                        SheetDetails.FreezeRows(_RowIndex);

                        #endregion

                        foreach (DataRow DRitem in dSCaseSnp.Tables[0].Rows)
                        {
                            alertcodeList.Clear();
                            alertcodeName = string.Empty;
                            _RowIndex++;
                            _ColumnIndex = 0;

                            SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = DRitem["MST_AGENCY"].ToString();
                            SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                            SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                            _ColumnIndex++;

                            SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = DRitem["MST_DEPT"].ToString();
                            SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                            SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                            _ColumnIndex++;

                            SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = DRitem["MST_PROGRAM"].ToString();
                            SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                            SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                            _ColumnIndex++;

                            SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = DRitem["MST_YEAR"].ToString();
                            SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                            SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                            _ColumnIndex++;

                            SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = DRitem["MST_APP_NO"].ToString();
                            SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                            SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                            _ColumnIndex++;

                            SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = LookupDataAccess.GetMemberName(DRitem["SNP_NAME_IX_FI"].ToString(), DRitem["SNP_NAME_IX_MI"].ToString(), DRitem["SNP_NAME_IX_LAST"].ToString(), Member_NameFormat);
                            SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                            SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                            _ColumnIndex++;


                            if (((Captain.Common.Utilities.ListItem)cmbdimension.SelectedItem).Value.ToString() != "ALL")
                            {
                                if (dSCaseSnp.Tables[0].Columns.Contains("A1"))
                                {
                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = DRitem["A1"].ToString();

                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                    _ColumnIndex++;
                                }
                                if (dSCaseSnp.Tables[0].Columns.Contains("A2"))
                                {
                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = DRitem["A2"].ToString();

                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                    _ColumnIndex++;
                                }
                                if (dSCaseSnp.Tables[0].Columns.Contains("A3"))
                                {
                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = DRitem["A3"].ToString();

                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                    _ColumnIndex++;
                                }
                                if (dSCaseSnp.Tables[0].Columns.Contains("A4"))
                                {
                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = DRitem["A4"].ToString();

                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                    _ColumnIndex++;
                                }
                                if (dSCaseSnp.Tables[0].Columns.Contains("A5"))
                                {
                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = DRitem["A5"].ToString();

                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                    _ColumnIndex++;
                                }
                                if (dSCaseSnp.Tables[0].Columns.Contains("A6"))
                                {
                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = DRitem["A6"].ToString();

                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                    _ColumnIndex++;
                                }
                                if (dSCaseSnp.Tables[0].Columns.Contains("A7"))
                                {
                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = DRitem["A7"].ToString();

                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                    _ColumnIndex++;
                                }
                                if (dSCaseSnp.Tables[0].Columns.Contains("A8"))
                                {
                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = DRitem["A8"].ToString();

                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                    _ColumnIndex++;
                                }
                                if (dSCaseSnp.Tables[0].Columns.Contains("A9"))
                                {
                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = DRitem["A9"].ToString();

                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                    _ColumnIndex++;
                                }
                                if (dSCaseSnp.Tables[0].Columns.Contains("A10"))
                                {
                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = DRitem["A10"].ToString();

                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                    _ColumnIndex++;
                                }
                                if (dSCaseSnp.Tables[0].Columns.Contains("A11"))
                                {
                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = DRitem["A11"].ToString();

                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                    _ColumnIndex++;
                                }
                                if (dSCaseSnp.Tables[0].Columns.Contains("A12"))
                                {
                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = DRitem["A12"].ToString();

                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                    _ColumnIndex++;
                                }
                                if (dSCaseSnp.Tables[0].Columns.Contains("A13"))
                                {
                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = DRitem["A13"].ToString();

                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                    _ColumnIndex++;
                                }
                                if (dSCaseSnp.Tables[0].Columns.Contains("A14"))
                                {
                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = DRitem["A14"].ToString();

                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                    _ColumnIndex++;
                                }
                                if (dSCaseSnp.Tables[0].Columns.Contains("A15"))
                                {
                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = DRitem["A15"].ToString();

                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                    _ColumnIndex++;
                                }
                                if (dSCaseSnp.Tables[0].Columns.Contains("A16"))
                                {
                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = DRitem["A16"].ToString();

                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                    _ColumnIndex++;
                                }
                                if (dSCaseSnp.Tables[0].Columns.Contains("A17"))
                                {
                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = DRitem["A17"].ToString();

                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                    _ColumnIndex++;
                                }
                                if (dSCaseSnp.Tables[0].Columns.Contains("A18"))
                                {
                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = DRitem["A18"].ToString();

                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                    _ColumnIndex++;
                                }
                                if (dSCaseSnp.Tables[0].Columns.Contains("A19"))
                                {
                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = DRitem["A19"].ToString();
                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                    _ColumnIndex++;
                                }
                                if (dSCaseSnp.Tables[0].Columns.Contains("A20"))
                                {
                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = DRitem["A20"].ToString();

                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                    _ColumnIndex++;
                                }
                                if (dSCaseSnp.Tables[0].Columns.Contains("A21"))
                                {
                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = DRitem["A21"].ToString();

                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                    _ColumnIndex++;
                                }
                                if (dSCaseSnp.Tables[0].Columns.Contains("A22"))
                                {
                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = DRitem["A22"].ToString();

                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                    _ColumnIndex++;
                                }
                                if (dSCaseSnp.Tables[0].Columns.Contains("A23"))
                                {
                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = DRitem["A23"].ToString();

                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                    _ColumnIndex++;
                                }
                                if (dSCaseSnp.Tables[0].Columns.Contains("A24"))
                                {
                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = DRitem["A24"].ToString();
                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                    _ColumnIndex++;
                                }
                                if (dSCaseSnp.Tables[0].Columns.Contains("A25"))
                                {
                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = DRitem["A25"].ToString();


                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                    _ColumnIndex++;
                                }
                            }
                            else
                            {
                                foreach (DataColumn dc in dSCaseSnp.Tables[0].Columns)
                                {
                                    if (dc.ColumnName.Equals("A1"))
                                    {
                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = DRitem["A1"].ToString();
                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                        _ColumnIndex++;
                                    }
                                    if (dc.ColumnName.Equals("A2"))
                                    {
                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = DRitem["A2"].ToString();

                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                        _ColumnIndex++;
                                    }
                                    if (dc.ColumnName.Equals("A3"))
                                    {
                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = DRitem["A3"].ToString();

                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                        _ColumnIndex++;
                                    }
                                    if (dc.ColumnName.Equals("A4"))
                                    {
                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = DRitem["A4"].ToString();

                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                        _ColumnIndex++;
                                    }
                                    if (dc.ColumnName.Equals("A5"))
                                    {
                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = DRitem["A5"].ToString();

                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                        _ColumnIndex++;
                                    }
                                    if (dc.ColumnName.Equals("A6"))
                                    {
                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = DRitem["A6"].ToString();

                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                        _ColumnIndex++;
                                    }
                                    if (dc.ColumnName.Equals("A7"))
                                    {
                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = DRitem["A7"].ToString();

                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                        _ColumnIndex++;
                                    }
                                    if (dc.ColumnName.Equals("A8"))
                                    {
                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = DRitem["A8"].ToString();

                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                        _ColumnIndex++;
                                    }
                                    if (dc.ColumnName.Equals("A9"))
                                    {
                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = DRitem["A9"].ToString();

                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                        _ColumnIndex++;
                                    }
                                    if (dc.ColumnName.Equals("A10"))
                                    {
                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = DRitem["A10"].ToString();

                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                        _ColumnIndex++;
                                    }
                                    if (dc.ColumnName.Equals("A11"))
                                    {
                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = DRitem["A11"].ToString();

                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                        _ColumnIndex++;
                                    }
                                    if (dc.ColumnName.Equals("A12"))
                                    {
                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = DRitem["A12"].ToString();

                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                        _ColumnIndex++;
                                    }
                                    if (dc.ColumnName.Equals("A13"))
                                    {
                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = DRitem["A13"].ToString();

                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                        _ColumnIndex++;
                                    }
                                    if (dc.ColumnName.Equals("A14"))
                                    {
                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = DRitem["A14"].ToString();

                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                        _ColumnIndex++;
                                    }
                                    if (dc.ColumnName.Equals("A15"))
                                    {
                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = DRitem["A15"].ToString();

                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                        _ColumnIndex++;
                                    }
                                    if (dc.ColumnName.Equals("A16"))
                                    {
                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = DRitem["A16"].ToString();

                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                        _ColumnIndex++;
                                    }
                                    if (dc.ColumnName.Equals("A17"))
                                    {
                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = DRitem["A17"].ToString();

                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                        _ColumnIndex++;
                                    }
                                    if (dc.ColumnName.Equals("A18"))
                                    {
                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = DRitem["A18"].ToString();

                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                        _ColumnIndex++;
                                    }
                                    if (dc.ColumnName.Equals("A19"))
                                    {
                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = DRitem["A19"].ToString();
                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                        _ColumnIndex++;
                                    }
                                    if (dc.ColumnName.Equals("A20"))
                                    {
                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = DRitem["A20"].ToString();

                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                        _ColumnIndex++;
                                    }
                                    if (dc.ColumnName.Equals("A21"))
                                    {
                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = DRitem["A21"].ToString();

                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                        _ColumnIndex++;
                                    }
                                    if (dc.ColumnName.Equals("A22"))
                                    {
                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = DRitem["A22"].ToString();

                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                        _ColumnIndex++;
                                    }
                                    if (dc.ColumnName.Equals("A23"))
                                    {
                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = DRitem["A23"].ToString();

                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                        _ColumnIndex++;
                                    }
                                    if (dc.ColumnName.Equals("A24"))
                                    {
                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = DRitem["A24"].ToString();
                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                        _ColumnIndex++;
                                    }
                                    if (dc.ColumnName.Equals("A25"))
                                    {
                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = DRitem["A25"].ToString();

                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                        SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                        _ColumnIndex++;
                                    }
                                }
                            }

                            SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = DRitem["MST_PRESS_TOTAL"].ToString();
                            SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                            SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                            _ColumnIndex++;


                            if (DRitem["MST_PRESS_CAT"].ToString() != string.Empty)
                            {
                                RankCatgEntity preasscategory = propPreassGroup.Find(u => u.Code == DRitem["MST_PRESS_GRP"].ToString() && u.SubCode.Trim() == DRitem["MST_PRESS_CAT"].ToString());
                                if (preasscategory != null)
                                {
                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = preasscategory.Desc.ToString().Trim();
                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                    _ColumnIndex++;
                                }
                                else
                                {
                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = "";
                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                    SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                    _ColumnIndex++;
                                }
                            }
                            else
                            {
                                SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = "";
                                SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                _ColumnIndex++;
                            }

                            RankCatgEntity preassgroup = propPreassGroup.Find(u => u.Code.Trim() == DRitem["MST_PRESS_GRP"].ToString() && u.SubCode.Trim() == string.Empty);
                            if (preassgroup != null)
                            {
                                SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = preassgroup.Desc.ToString().Trim();
                                SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                _ColumnIndex++;
                            }
                            else
                            {
                                SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = "";
                                SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                _ColumnIndex++;
                            }

                            SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = LookupDataAccess.Getdate(DRitem["MST_DATE_LSTC_5"].ToString()).Trim();
                            SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                            SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                            _ColumnIndex++;

                            SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = DRitem["MST_INTAKE_WORKER"].ToString() == "" ? "" : GetWorkerName(DRitem["MST_INTAKE_WORKER"].ToString().Trim());
                            SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                            SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                            _ColumnIndex++;

                            SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = DRitem["MST_SITE"].ToString() == "" ? "" : GetSiteName(DRitem["MST_SITE"].ToString());
                            SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                            SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                            _ColumnIndex++;

                            alertcodeList = listCodes.FindAll(x => DRitem["MST_ALERT_CODES"].ToString().Contains(x.Code)).ToList();

                            foreach (CommonEntity entity in alertcodeList)
                            {
                                alertcodeName = alertcodeName + entity.Desc + ", ";
                            }
                            SheetDetails.Rows[_RowIndex][_ColumnIndex].Value = alertcodeName.Trim().TrimEnd(',');
                            SheetDetails.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                            SheetDetails.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                            _ColumnIndex++;
                        }

                        SheetDetails.IgnoredErrors.Add(SheetDetails.GetDataRange(), DevExpress.Spreadsheet.IgnoredErrorType.NumberAsText);

                        #endregion

                        #region Summary Shert

                        DevExpress.Spreadsheet.Worksheet SummarySheet = wb.Worksheets.Add("Summary");
                        SummarySheet.ActiveView.TabColor = ColorTranslator.FromHtml("#e3e6ad");

                        #region Column Widths

                        SummarySheet.Columns[0].Width = 90;//Category Code
                        SummarySheet.Columns[1].Width = 150;//Category Name
                        SummarySheet.Columns[2].Width = 80;//Low Points
                        SummarySheet.Columns[3].Width = 80;//High Points
                        SummarySheet.Columns[4].Width = 110;//Applicant Counts

                        #endregion

                        #region Column Headers

                        _RowIndex = 0;
                        _ColumnIndex = 0;

                        SummarySheet.Rows[_RowIndex][_ColumnIndex].Value = "Category Code";
                        SummarySheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlBCHC;
                        SummarySheet.Rows[_RowIndex][_ColumnIndex].Font.Size = 11;
                        _ColumnIndex++;

                        SummarySheet.Rows[_RowIndex][_ColumnIndex].Value = "Category Name";
                        SummarySheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlBLHC;
                        SummarySheet.Rows[_RowIndex][_ColumnIndex].Font.Size = 11;
                        _ColumnIndex++;

                        SummarySheet.Rows[_RowIndex][_ColumnIndex].Value = "Low Points";
                        SummarySheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlBCHC;
                        SummarySheet.Rows[_RowIndex][_ColumnIndex].Font.Size = 11;
                        _ColumnIndex++;

                        SummarySheet.Rows[_RowIndex][_ColumnIndex].Value = "High Points";
                        SummarySheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlBCHC;
                        SummarySheet.Rows[_RowIndex][_ColumnIndex].Font.Size = 11;
                        //SummarySheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                        _ColumnIndex++;

                        SummarySheet.Rows[_RowIndex][_ColumnIndex].Value = "Applicant Counts";
                        SummarySheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlBCHC;
                        SummarySheet.Rows[_RowIndex][_ColumnIndex].Font.Size = 11;
                        _ColumnIndex++;

                        #endregion

                        #region Printing

                        List<RankCatgEntity> PreassGroupList = propPreassGroup;
                        if (((Captain.Common.Utilities.ListItem)cmbPressGroup.SelectedItem).Value.ToString() != "ALL")
                            PreassGroupList = propPreassGroup.FindAll(u => u.Code == ((Captain.Common.Utilities.ListItem)cmbPressGroup.SelectedItem).Value.ToString());
                        int inttotalcout = 0;
                        int intsummarloop = 0;

                        foreach (RankCatgEntity item in PreassGroupList)
                        {
                            _RowIndex++;
                            _ColumnIndex = 0;

                            if (item.SubCode == string.Empty)
                            {

                                if (intsummarloop > 0)
                                {
                                    SummarySheet.Rows[_RowIndex][_ColumnIndex].Value = "";
                                    SummarySheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                    SummarySheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                    _ColumnIndex++;

                                    SummarySheet.Rows[_RowIndex][_ColumnIndex].Value = "";
                                    SummarySheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                    SummarySheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                    _ColumnIndex++;

                                    SummarySheet.Rows[_RowIndex][_ColumnIndex].Value = "";
                                    SummarySheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                    SummarySheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                    _ColumnIndex++;

                                    SummarySheet.Rows[_RowIndex][_ColumnIndex].Value = "Total";
                                    SummarySheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlBCHC;
                                    SummarySheet.Rows[_RowIndex][_ColumnIndex].Font.Size = 11;
                                    SummarySheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                    _ColumnIndex++;

                                    SummarySheet.Rows[_RowIndex][_ColumnIndex].Value = inttotalcout.ToString();
                                    SummarySheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlBCHC;
                                    SummarySheet.Rows[_RowIndex][_ColumnIndex].Font.Size = 11;
                                    SummarySheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                    _ColumnIndex++;

                                    _RowIndex++;
                                }
                                _ColumnIndex = 0;
                                SummarySheet.Rows[_RowIndex][_ColumnIndex].Value = item.Desc;
                                oDevExpress_Excel_Properties.xlRowsMerge(SummarySheet, _RowIndex, _ColumnIndex, 5,oDevExpress_Excel_Properties.gxlNCC_bo);
                                SummarySheet.Rows[_RowIndex][_ColumnIndex].Font.Size = 14;
                                SummarySheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                _ColumnIndex = _ColumnIndex + 5;

                                intsummarloop = intsummarloop + 1;
                                inttotalcout = 0;
                            }
                            else
                            {
                                SummarySheet.Rows[_RowIndex][_ColumnIndex].Value = item.SubCode;
                                SummarySheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                SummarySheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                _ColumnIndex++;

                                SummarySheet.Rows[_RowIndex][_ColumnIndex].Value = item.Desc;
                                SummarySheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBLC_Blue_Borders;
                                SummarySheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                _ColumnIndex++;

                                SummarySheet.Rows[_RowIndex][_ColumnIndex].Value = item.PointsLow;
                                SummarySheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                SummarySheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                _ColumnIndex++;

                                SummarySheet.Rows[_RowIndex][_ColumnIndex].Value = item.PointsHigh;
                                SummarySheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                SummarySheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                _ColumnIndex++;

                                DataRow[] results = dSCaseSnp.Tables[0].Select("MST_PRESS_GRP = '" + item.Code.Trim() + "' AND MST_PRESS_CAT = '" + item.SubCode.Trim() + "'");

                                SummarySheet.Rows[_RowIndex][_ColumnIndex].Value = results.Length.ToString();
                                SummarySheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                                SummarySheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                                _ColumnIndex++;

                                inttotalcout = inttotalcout + results.Length;
                            }
                        }

                        _RowIndex++;
                        _ColumnIndex = 0;
                        SummarySheet.Rows[_RowIndex][_ColumnIndex].Value = "";
                        SummarySheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SummarySheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                        _ColumnIndex++;

                        SummarySheet.Rows[_RowIndex][_ColumnIndex].Value = "";
                        SummarySheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SummarySheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                        _ColumnIndex++;

                        SummarySheet.Rows[_RowIndex][_ColumnIndex].Value = "";
                        SummarySheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlDBCC_Blue_Borders;
                        SummarySheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                        _ColumnIndex++;

                        SummarySheet.Rows[_RowIndex][_ColumnIndex].Value = "Total";
                        SummarySheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlBCHC;
                        SummarySheet.Rows[_RowIndex][_ColumnIndex].Font.Size = 11;
                        SummarySheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                        _ColumnIndex++;

                        SummarySheet.Rows[_RowIndex][_ColumnIndex].Value = inttotalcout.ToString();
                        SummarySheet.Rows[_RowIndex][_ColumnIndex].Style = oDevExpress_Excel_Properties.gxlBCHC;
                        SummarySheet.Rows[_RowIndex][_ColumnIndex].Font.Size = 11;
                        SummarySheet.Rows[_RowIndex][_ColumnIndex].RowHeight = 20;
                        _ColumnIndex++;

                        SummarySheet.IgnoredErrors.Add(SummarySheet.GetDataRange(), DevExpress.Spreadsheet.IgnoredErrorType.NumberAsText);

                        #endregion

                        #endregion

                        #endregion

                        wb.Sheets.ActiveSheet = wb.Worksheets[0];
                        wb.SaveDocument(xlFileName, DevExpress.Spreadsheet.DocumentFormat.OpenXml);

                        try
                        {
                            string localFilePath = xlFileName;

                            FileInfo fiDownload = new FileInfo(localFilePath);

                            string name = fiDownload.Name;
                            using (FileStream fileStream = fiDownload.OpenRead())
                            {
                                Application.Download(fileStream, name);
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
                catch (Exception ex) { };
            }
        }

        private void PrintHeaderPage_Excel(DevExpress.Spreadsheet.Workbook wb, DevExpress_Excel_Properties oDevExpress_Excel_Properties)
        {
            DevExpress.Spreadsheet.Worksheet paramSheet = wb.Worksheets[0];
            paramSheet.Name = "PARAMS";
            paramSheet.ActiveView.TabColor = Color.ForestGreen;
            paramSheet.ActiveView.ShowGridlines = false;
            wb.Unit = DevExpress.Office.DocumentUnit.Point;

            paramSheet.Columns[1].Width = 100;//80;
            paramSheet.Columns[2].Width = 200;
            paramSheet.Columns[3].Width = 50;
            paramSheet.Columns[4].Width = 100;
            paramSheet.Columns[5].Width = 200;

            int _Rowindex = 0;
            int _Columnindex = 0;

            string strAgy = Current_Hierarchy_DB.Split('-')[0];

            AgencyControlEntity BAgyControlDetails = _model.ZipCodeAndAgency.GetAgencyControlFile(strAgy);

            string ImgName = "";
            if (BaseForm.BaseAgencyControlDetails.AgyShortName == "NEOCAA")
            {
                ImgName = "NEOCAA_" + strAgy + "_LOGO.png";
            }
            else
                ImgName = BaseForm.BaseAgencyControlDetails.AgyShortName + "_00_LOGO.png";

            _Rowindex = 1;
            _Columnindex = 1;
            paramSheet.Rows[_Rowindex][_Columnindex].Value = BAgyControlDetails.AgyName;
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.xTitleCellstyle;
            oDevExpress_Excel_Properties.xlRowsMerge(paramSheet, _Rowindex, _Columnindex, 5);
            _Rowindex++;

            string imagesPath = "https://capsysdev.capsystems.com/images/PIPlogos/" + ImgName;
            DevExpress.Spreadsheet.SpreadsheetImageSource imgsrc = DevExpress.Spreadsheet.SpreadsheetImageSource.FromUri(imagesPath, wb);
            //DevExpress.Spreadsheet.Picture pic = paramSheet.Pictures.AddPicture(imgsrc, 200, 80, 630, 280);
            DevExpress.Spreadsheet.Picture pic = paramSheet.Pictures.AddPicture(imgsrc, 50, 0, 120, 70);


            AgencyControlEntity _agycntrldets = new AgencyControlEntity();
            _agycntrldets = BaseForm.BaseAgencyControlDetails;

            if (BaseForm.BaseAgencyControlDetails.AgyShortName == "NEOCAA")
                _agycntrldets = BAgyControlDetails;
            else
                _agycntrldets = BaseForm.BaseAgencyControlDetails;

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
            paramSheet.Rows[_Rowindex][_Columnindex].Value = Privileges.PrivilegeName;
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
                oDevExpress_Excel_Properties.xlRowsMerge(paramSheet, _Rowindex, _Columnindex, 5);
            }
            else
            {
                _Rowindex++;
                paramSheet.Rows[_Rowindex][_Columnindex].Value = Txt_HieDesc.Text.Trim();
                paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.xsubTitleintakeCellstyle;
                oDevExpress_Excel_Properties.xlRowsMerge(paramSheet, _Rowindex, _Columnindex, 5);
            }

            _Rowindex++;
            paramSheet.Rows[_Rowindex][_Columnindex].Value = "";
            oDevExpress_Excel_Properties.xlRowsMerge(paramSheet, _Rowindex, _Columnindex, 5);

            _Rowindex++;
            paramSheet.Rows[_Rowindex][_Columnindex].Value = lblPreassGrp.Text.Trim();
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.paramsCellStyle;
            _Columnindex++;

            paramSheet.Rows[_Rowindex][_Columnindex].Value = ((Captain.Common.Utilities.ListItem)cmbPressGroup.SelectedItem).Text.ToString().Trim();
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNLC;
            _Columnindex++;

            paramSheet.Rows[_Rowindex][_Columnindex].Value = " ";
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlEMPTC;
            _Columnindex++;

            paramSheet.Rows[_Rowindex][_Columnindex].Value = lblPreCatg.Text.Trim();
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.paramsCellStyle;
            _Columnindex++;

            paramSheet.Rows[_Rowindex][_Columnindex].Value = ((Captain.Common.Utilities.ListItem)cmbPressgcat.SelectedItem).Text.ToString().Trim();
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNLC;

            _Rowindex++;
            _Columnindex = 1;

            paramSheet.Rows[_Rowindex][_Columnindex].Value = "Score Range";
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.paramsCellStyle;
            _Columnindex++;

            paramSheet.Rows[_Rowindex][_Columnindex].Value = "From: " + txtfrom.Text.Trim() + "  To: " + txtTo.Text.Trim();
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNLC;
            _Columnindex++;

            paramSheet.Rows[_Rowindex][_Columnindex].Value = " ";
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlEMPTC;
            _Columnindex++;

            paramSheet.Rows[_Rowindex][_Columnindex].Value = lblRepType.Text.Trim();
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.paramsCellStyle;
            _Columnindex++;

            string repType = string.Empty;
            if (rdoall.Checked)
                repType = "All Applicants";
            else
                repType = "Selected Applicant: " + txtApplicant.Text.Trim();

            paramSheet.Rows[_Rowindex][_Columnindex].Value = repType;
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNLC;

            _Rowindex++;
            _Columnindex = 1;

            string alertcode = string.Empty;
            if (rdbAlrtCde_All.Checked)
                alertcode = lblAlertCodes.Text.Trim();
            else
                alertcode = "Selected Alert Code(s)";

            paramSheet.Rows[_Rowindex][_Columnindex].Value = alertcode;
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.paramsCellStyle;
            _Columnindex++;

            string selectedAlertCodes = SelectedAlertCodes;
            selectedAlertCodes = selectedAlertCodes.Replace("'", "");
            selectedAlertCodes = selectedAlertCodes.Replace(",", ", ");

            if (rdbAlrtCde_All.Checked)
                selectedAlertCodes = "All Alert Codes";
            else
                selectedAlertCodes = selectedAlertCodes;

            paramSheet.Rows[_Rowindex][_Columnindex].Value = selectedAlertCodes;
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNLC;
            _Columnindex++;

            paramSheet.Rows[_Rowindex][_Columnindex].Value = " ";
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlEMPTC;
            _Columnindex++;

            paramSheet.Rows[_Rowindex][_Columnindex].Value = lblDimension.Text.Trim();
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.paramsCellStyle;
            _Columnindex++;

            paramSheet.Rows[_Rowindex][_Columnindex].Value = ((Captain.Common.Utilities.ListItem)cmbdimension.SelectedItem).Text.ToString().Trim();
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNLC;
            _Columnindex++;

            _Rowindex++;
            _Columnindex = 1;

            paramSheet.Rows[_Rowindex][_Columnindex].Value = "Date Range";
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.paramsCellStyle;
            _Columnindex++;

            paramSheet.Rows[_Rowindex][_Columnindex].Value = "From: " + Convert.ToDateTime(dtpFrmDate.Value).ToShortDateString() + "  To: " + Convert.ToDateTime(dtpToDt.Value).ToShortDateString();
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNLC;
            _Columnindex++;

            paramSheet.Rows[_Rowindex][_Columnindex].Value = " ";
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlEMPTC;
            _Columnindex++;

            paramSheet.Rows[_Rowindex][_Columnindex].Value = " ";
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlEMPTC;
            _Columnindex++;

            paramSheet.Rows[_Rowindex][_Columnindex].Value = " ";
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlEMPTC;
            _Columnindex++;

            _Rowindex++;
            _Columnindex = 1;

            paramSheet.Rows[_Rowindex][_Columnindex].Value = "";
            _Columnindex = _Columnindex + 5;

            _Rowindex = _Rowindex + 3;
            _Columnindex = 5;
            paramSheet.Rows[_Rowindex][_Columnindex].Value = "Generated By: " + BaseForm.UserID;
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlGenerate_lr;

            _Rowindex++;
            paramSheet.Rows[_Rowindex][_Columnindex].Value = "Generated On: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");
            paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlGenerate_lr;

            paramSheet.IgnoredErrors.Add(paramSheet.GetDataRange(), DevExpress.Spreadsheet.IgnoredErrorType.NumberAsText);

        }

        DataTable dtWorker = new DataTable();
        public string GetWorkerName(string Worker)
        {
            string Caseworker = string.Empty;
            if (dtWorker.Rows.Count > 0)
            {
                foreach (DataRow dr in dtWorker.Rows)
                {
                    if (dr["PWH_CASEWORKER"].ToString().Trim() == Worker)
                    {
                        Caseworker = dr["NAME"].ToString().Trim();
                        break;
                    }
                }
            }
            return Caseworker;
        }

        DataTable dtSite = new DataTable();
        public string GetSiteName(string Site)
        {
            string Casesite = string.Empty;
            if (dtSite.Rows.Count > 0)
            {
                foreach (DataRow dr in dtSite.Rows)
                {
                    if (dr["SITE_NUMBER"].ToString().Trim() == Site.Trim())
                    {
                        Casesite = dr["SITE_NAME"].ToString().Trim();
                        break;
                    }
                }
            }
            return Casesite;
        }

        private List<CommonEntity> filterByHIE(List<CommonEntity> LookupValues)
        {
            string HIE = BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg;
            LookupValues = LookupValues.FindAll(u => u.ListHierarchy.Contains(HIE) || u.ListHierarchy.Contains(BaseForm.BaseAgency + BaseForm.BaseDept + "**") || u.ListHierarchy.Contains(BaseForm.BaseAgency + "****") || u.ListHierarchy.Contains("******"));
            return LookupValues;
        }
    }
}