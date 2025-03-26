#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using Wisej.Web;
using Captain.DatabaseLayer;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Views.UserControls.Base;
using Captain.Common.Model.Objects;
using Captain.Common.Model.Data;
using Captain.Common.Utilities;
using Captain.Common.Views.Forms;
using Captain.Common.Exceptions;
using System.Diagnostics;
using System.IO;
using CarlosAg.ExcelXmlWriter;
using static NPOI.HSSF.Util.HSSFColor;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class RIXMLADT_Form : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;
        //private bool boolChangeStatus = false;

        public int strIndex = 0;
        public int strCrIndex = 0;
        public int strPageIndex = 1;

        #endregion
        public RIXMLADT_Form(BaseForm baseform, PrivilegeEntity privilegeEntity)
        {
            InitializeComponent();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            _model = new CaptainModel();

            BaseForm = baseform;
            Privileges = privilegeEntity;
            this.Text = "Scheduler Audit Report Parameters";

            propReportPath = _model.lookupDataAccess.GetReportPath();
        }

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public string propReportPath { get; set; }

        List<TRIGSummaryEntity> SummaryMergeList = new List<TRIGSummaryEntity>();
        private void btnSearch_Click(object sender, EventArgs e)
        {

            //if (SummaryMergeList.Count > 0)
            //{

            if (ValidateForm())
            {
                PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, false, propReportPath, "XLS");
                pdfListForm.FormClosed += new FormClosedEventHandler(On_SaveExcelForm_Closed);
                pdfListForm.StartPosition = FormStartPosition.CenterScreen;
                pdfListForm.ShowDialog();
            }
            //}
            ////On_SaveExcelForm_Closed1(SummaryMergeList);
            //else
            //    CommonFunctions.MessageBoxDisplay("No Records found");
        }

        private bool ValidateForm()
        {
            bool isValid = true;

            if (dtpFromDate.Checked == false && dtToDate.Checked == false)
            {
                _errorProvider.SetError(dtToDate, string.Format("Please Select From and To Dates".Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtToDate, null);
            }

            if (dtpFromDate.Checked == true || dtToDate.Checked == true)
            {
                if (dtpFromDate.Checked == false && dtToDate.Checked == true)
                {
                    _errorProvider.SetError(dtpFromDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "From Date".Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtpFromDate, null);
                }
            }

            if (dtpFromDate.Checked == true || dtToDate.Checked == true)
            {
                if (dtToDate.Checked == false && dtpFromDate.Checked == true)
                {
                    _errorProvider.SetError(dtToDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "To Date".Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtToDate, null);
                }
            }

            if (dtpFromDate.Checked.Equals(true) && dtToDate.Checked.Equals(true))
                {
                if (string.IsNullOrWhiteSpace(dtpFromDate.Text))
                {
                    _errorProvider.SetError(dtpFromDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "From Date".Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtpFromDate, null);
                }
                if (string.IsNullOrWhiteSpace(dtToDate.Text))
                {
                    _errorProvider.SetError(dtToDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "To Date".Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtToDate, null);
                }

                if (!string.IsNullOrEmpty(dtpFromDate.Text) && (!string.IsNullOrEmpty(dtToDate.Text)))
                {
                    if (Convert.ToDateTime(dtpFromDate.Text) > Convert.ToDateTime(dtToDate.Text))
                    {
                        _errorProvider.SetError(dtToDate, "'To Date' should be equal or greater than 'From Date'");
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(dtToDate, null);
                    }
                }
            }
            return (isValid);
        }

        string Random_Filename = null; string PdfName = null;
        private void On_SaveExcelForm_Closed(object sender, FormClosedEventArgs e)
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
                   AlertBox.Show("Error",MessageBoxIcon.Error);
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

                this.GenerateStyles(book.Styles);
                this.GenerateWorksheetParameters(book.Worksheets);

                SummaryMergeList.Clear();
                if (rbTrigger.Checked)
                {
                    List<TRIGSummaryEntity> SummaryList = _model.SPAdminData.Browse_TrigSummary(string.Empty, dtpFromDate.Value.ToShortDateString(), dtToDate.Value.ToShortDateString());

                    SummaryList = SummaryList.FindAll(u => (Convert.ToDateTime(dtpFromDate.Value.ToShortDateString()) <= Convert.ToDateTime(u.Trig_Date.ToString())) && (Convert.ToDateTime(dtToDate.Value.ToShortDateString()) >= Convert.ToDateTime(u.Trig_Date)));
                    if (SummaryList.Count > 0)
                    {
                        foreach (TRIGSummaryEntity Entity in SummaryList)
                        {
                            SummaryMergeList.Add(new TRIGSummaryEntity(Entity.Trig_Code, LookupDataAccess.Getdate(Entity.Trig_Date.Trim()), Entity.Trig_Start_Time, Entity.Trig_Time, Entity.Trig_Date_Seq, Entity.Trig_User, "APP = " + Entity.AppCnt, "SPM =" + Entity.SPMCnt, "CA =" + Entity.ACTCnt, "MS =" + Entity.MSCnt, "Trigger"));
                        }
                    }
                }
                else if (rbRIAXML.Checked)
                {
                    DataTable dt = new DataTable();
                    dt = CaseMst.get_XMLDCRS(dtpFromDate.Value.ToShortDateString(), dtToDate.Value.ToShortDateString(), string.Empty, string.Empty, string.Empty).Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        //index = gvSummary.Rows.Add("RIAA XML", "Date", "", "", "Applicants", "Members", "Income Recs", "", "");
                        //gvSummary.Rows[index].DefaultCellStyle.ForeColor = System.Drawing.Color.Blue;
                        string strDesc = "Riaa xml";
                        if (BaseForm.BaseAgencyControlDetails.AgyShortName == "CAPR")
                        {
                            strDesc = "ServTraq EAP Feed";
                        }
                        foreach (DataRow item in dt.Rows)
                        {
                            SummaryMergeList.Add(new TRIGSummaryEntity(strDesc, LookupDataAccess.Getdate(item["oDate"].ToString()), "", "", "", "", "APP =" + item["MT"].ToString(), "Member =" + item["ST"].ToString(), "Income =" + item["IT"].ToString(), "", "RIAA XML"));
                        }

                    }
                }

                if (SummaryMergeList.Count > 0)
                {
                    SummaryMergeList = SummaryMergeList.OrderByDescending(u => Convert.ToDateTime(u.Trig_Date)).ThenByDescending(u => u.Trig_Type).ThenBy(u => u.Trig_Code).ThenBy(u => u.Trig_Date_Seq).ToList();

                    Worksheet sheet1 = book.Worksheets.Add("Data");
                    sheet1.Table.DefaultRowHeight = 14.25F;

                    sheet1.Table.Columns.Add(150);
                    sheet1.Table.Columns.Add(120);
                    sheet1.Table.Columns.Add(80);
                    sheet1.Table.Columns.Add(80);
                    sheet1.Table.Columns.Add(50);
                    sheet1.Table.Columns.Add(120);
                    sheet1.Table.Columns.Add(120);
                    sheet1.Table.Columns.Add(120);
                    sheet1.Table.Columns.Add(120);
                    sheet1.Table.Columns.Add(120);

                    WorksheetRow Row = sheet1.Table.Rows.Add();

                    Row.Cells.Add("Type", DataType.String, "s96");
                    Row.Cells.Add("Date", DataType.String, "s96");
                    if (rbTrigger.Checked)
                    {
                        Row.Cells.Add("Start Time", DataType.String, "s96");
                        Row.Cells.Add("End Time", DataType.String, "s96");
                        Row.Cells.Add("Seq", DataType.String, "s96");
                        Row.Cells.Add("User", DataType.String, "s96");
                        Row.Cells.Add("App Count", DataType.String, "s96");
                        Row.Cells.Add("SPM Count", DataType.String, "s96");
                        Row.Cells.Add("Act Count", DataType.String, "s96");
                        Row.Cells.Add("MS Count", DataType.String, "s96");
                    }
                    else
                    {
                        //Row.Cells.Add("", DataType.String, "s96");
                        //Row.Cells.Add("", DataType.String, "s96");
                        //Row.Cells.Add("", DataType.String, "s96");
                        //Row.Cells.Add("", DataType.String, "s96");
                        //Row.Cells.Add("User", DataType.String, "s96");
                        Row.Cells.Add("App Count", DataType.String, "s96");
                        Row.Cells.Add("Member Count", DataType.String, "s96");
                        Row.Cells.Add("Income Count", DataType.String, "s96");
                        //Row.Cells.Add("", DataType.String, "s96");
                    }

                    foreach (TRIGSummaryEntity MergeEntity in SummaryMergeList)
                    {
                        Row = sheet1.Table.Rows.Add();

                        Row.Cells.Add(MergeEntity.Trig_Code, DataType.String, "s95");
                        Row.Cells.Add(LookupDataAccess.Getdate(MergeEntity.Trig_Date.Trim()), DataType.String, "s95");
                        if (rbTrigger.Checked)
                        {
                            Row.Cells.Add(MergeEntity.Trig_Start_Time, DataType.String, "s95");
                            Row.Cells.Add(MergeEntity.Trig_Time, DataType.String, "s95");
                            Row.Cells.Add(MergeEntity.Trig_Date_Seq, DataType.String, "s95");
                            Row.Cells.Add(MergeEntity.Trig_User, DataType.String, "s95");
                            Row.Cells.Add(MergeEntity.AppCnt, DataType.String, "s95");
                            Row.Cells.Add(MergeEntity.SPMCnt, DataType.String, "s95");
                            Row.Cells.Add(MergeEntity.ACTCnt, DataType.String, "s95");
                            Row.Cells.Add(MergeEntity.MSCnt, DataType.String, "s95");
                        }
                        else if(rbRIAXML.Checked)
                        {
                            Row.Cells.Add(MergeEntity.AppCnt, DataType.String, "s95");
                            Row.Cells.Add(MergeEntity.SPMCnt, DataType.String, "s95");
                            Row.Cells.Add(MergeEntity.ACTCnt, DataType.String, "s95");
                        }

                    }
                }

                FileStream stream = new FileStream(PdfName, FileMode.Create);

                book.Save(stream);
                stream.Close();
                AlertBox.Show("Report Generated Successfully");
                //FileDownloadGateway downloadGateway = new FileDownloadGateway();
                //downloadGateway.Filename = "SchedulerAudit_Report.xls";

                //// downloadGateway.Version = file.Version;

                //downloadGateway.SetContentType(DownloadContentType.OctetStream);

                //downloadGateway.StartFileDownload(new ContainerControl(), PdfName);
            }

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
            s95.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s95.Alignment.Vertical = StyleVerticalAlignment.Center;
            s95.Alignment.WrapText = true;
            s95.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s95.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            s95.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            s95.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            s95.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");

            // -----------------------------------------------
            //  s95C
            // -----------------------------------------------
            WorksheetStyle s95C = styles.Add("s95C");
            s95C.Font.FontName = "Arial";
            s95C.Font.Color = "#000000";
            s95C.Interior.Color = "#FFFFFF";
            s95C.Interior.Pattern = StyleInteriorPattern.Solid;
            s95C.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s95C.Alignment.Vertical = StyleVerticalAlignment.Center;
            s95C.Alignment.WrapText = true;
            s95C.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s95C.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            s95C.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            s95C.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            s95C.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s95B
            // -----------------------------------------------
            WorksheetStyle s95B = styles.Add("s95B");
            s95B.Font.FontName = "Arial";
            s95B.Font.Bold = true;
            s95B.Font.Color = "#0000FF";
            s95B.Interior.Color = "#FFFFFF";
            s95B.Interior.Pattern = StyleInteriorPattern.Solid;
            s95B.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s95B.Alignment.Vertical = StyleVerticalAlignment.Top;
            s95B.Alignment.WrapText = true;
            s95B.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            //  s95R
            // -----------------------------------------------
            WorksheetStyle s95R = styles.Add("s95R");
            s95R.Font.FontName = "Arial";
            //s95R.Font.Bold = true;
            s95R.Font.Color = "#FF0000";
            s95R.Interior.Color = "#FFFFFF";
            s95R.Interior.Pattern = StyleInteriorPattern.Solid;
            s95R.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s95R.Alignment.Vertical = StyleVerticalAlignment.Top;
            s95R.Alignment.WrapText = true;
            s95R.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;

            // -----------------------------------------------
            //  s96
            // -----------------------------------------------
            WorksheetStyle s96 = styles.Add("s96");
            s96.Font.FontName = "Arial";
            s96.Font.Color = "#000000";
            s96.Interior.Color = "#FFFFFF";
            s96.Font.Bold = true;
            s96.Font.Bold = true;
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
            // -----------------------------------------------
            //  s172
            // -----------------------------------------------
            WorksheetStyle s172H = styles.Add("s172H");
            s172H.Alignment.Vertical = StyleVerticalAlignment.Bottom;
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
            //  s171
            // -----------------------------------------------
            WorksheetStyle s171H = styles.Add("s171H");
            s171H.Parent = "s137";
            s171H.Font.FontName = "Calibri";
            s171H.Font.Size = 11;
            s171H.Interior.Color = "#FFFFFF";
            s171H.Interior.Pattern = StyleInteriorPattern.Solid;
            s171H.Alignment.Vertical = StyleVerticalAlignment.Top;
            s171H.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
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
            //  s170
            // -----------------------------------------------
            WorksheetStyle s170H = styles.Add("s170H");
            s170H.Parent = "s137";
            s170H.Font.FontName = "Calibri";
            s170H.Font.Size = 11;
            s170H.Interior.Color = "#FFFFFF";
            s170H.Interior.Pattern = StyleInteriorPattern.Solid;
            s170H.Alignment.Vertical = StyleVerticalAlignment.Top;
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
        }

        private void GenerateWorksheetParameters(WorksheetCollection sheets)
        {

            Worksheet sheet = sheets.Add("Parameters");
            sheet.Table.DefaultRowHeight = 14.4F;

            WorksheetColumn columnHead = sheet.Table.Columns.Add();
            columnHead.Index = 2;
            columnHead.Width = 5;
            sheet.Table.Columns.Add(163);
            WorksheetColumn column2Head = sheet.Table.Columns.Add();
            column2Head.Width = 332;
            column2Head.StyleID = "s172H";
            sheet.Table.Columns.Add(59);
            // -----------------------------------------------
            WorksheetRow RowHead = sheet.Table.Rows.Add();
            WorksheetCell cell;
            cell = RowHead.Cells.Add();
            cell.StyleID = "s139";
            cell = RowHead.Cells.Add();
            cell.StyleID = "s139";
            cell = RowHead.Cells.Add();
            cell.StyleID = "s139";
            cell = RowHead.Cells.Add();
            cell.StyleID = "s170H";
            cell = RowHead.Cells.Add();
            cell.StyleID = "s139";
            // -----------------------------------------------
            WorksheetRow Row1Head = sheet.Table.Rows.Add();
            Row1Head.Height = 12;
            Row1Head.AutoFitHeight = false;
            cell = Row1Head.Cells.Add();
            cell.StyleID = "s139";
            cell = Row1Head.Cells.Add();
            cell.StyleID = "s140";
            cell = Row1Head.Cells.Add();
            cell.StyleID = "s141";
            cell = Row1Head.Cells.Add();
            cell.StyleID = "s171H";
            cell = Row1Head.Cells.Add();
            cell.StyleID = "s142";
            // ----------------------------------------------
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
            WorksheetRow Row22Head = sheet.Table.Rows.Add();
            Row2Head.Height = 12;
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
            Row3Head.Height = 12;
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
            Row4Head.Height = 12;
            Row4Head.AutoFitHeight = false;
            cell = Row4Head.Cells.Add();
            cell.StyleID = "s139";
            cell = Row4Head.Cells.Add();
            cell.StyleID = "s143";
            cell = Row4Head.Cells.Add();
            cell.StyleID = "s139";
            cell = Row4Head.Cells.Add();
            cell.StyleID = "s170H";
            cell = Row4Head.Cells.Add();
            cell.StyleID = "s145";
            // -----------------------------------------------
            WorksheetRow Row5Head = sheet.Table.Rows.Add();
            Row5Head.Height = 12;
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
            WorksheetRow RowDHead = sheet.Table.Rows.Add();
            RowDHead.Height = 12;
            RowDHead.AutoFitHeight = false;
            cell = RowDHead.Cells.Add();
            cell.StyleID = "s139";
            cell = RowDHead.Cells.Add();
            cell.StyleID = "s143";
            cell = RowDHead.Cells.Add();
            cell.StyleID = "s139";
            cell = RowDHead.Cells.Add();
            cell.StyleID = "s170H";
            cell = RowDHead.Cells.Add();
            cell.StyleID = "s145";
            // -----------------------------------------------
            WorksheetRow RowdHead = sheet.Table.Rows.Add();
            RowdHead.Height = 12;
            RowdHead.AutoFitHeight = false;
            cell = RowdHead.Cells.Add();
            cell.StyleID = "s139";
            cell = RowdHead.Cells.Add();
            cell.StyleID = "s143";
            cell = RowdHead.Cells.Add();
            cell.StyleID = "m2611536909324";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "            Date Range From: " + dtpFromDate.Text + "   To: " + dtToDate.Text;
            cell.MergeAcross = 2;
            // -----------------------------------------------
            WorksheetRow Row6Head = sheet.Table.Rows.Add();
            Row6Head.Height = 12;
            Row6Head.AutoFitHeight = false;
            cell = Row6Head.Cells.Add();
            cell.StyleID = "s139";
            cell = Row6Head.Cells.Add();
            cell.StyleID = "s143";
            cell = Row6Head.Cells.Add();
            cell.StyleID = "s139";
            cell = Row6Head.Cells.Add();
            cell.StyleID = "s170H";
            cell = Row6Head.Cells.Add();
            cell.StyleID = "s145";
            // -----------------------------------------------
            string RepType = string.Empty;

            if (rbTrigger.Checked == true)
                RepType = rbTrigger.Text;
            else
                RepType = rbRIAXML.Text;

            WorksheetRow Row77Head = sheet.Table.Rows.Add();
            Row77Head.Height = 12;
            Row77Head.AutoFitHeight = false;
            cell = Row77Head.Cells.Add();
            cell.StyleID = "s139";
            cell = Row77Head.Cells.Add();
            cell.StyleID = "s143";
            cell = Row77Head.Cells.Add();
            cell.StyleID = "m2611536909324";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "            Type: " + RepType;
            cell.MergeAcross = 2;
            // -----------------------------------------------
            WorksheetRow Row8 = sheet.Table.Rows.Add();
            Row8.Height = 12;
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
            WorksheetRow Row24 = sheet.Table.Rows.Add();
            Row24.Height = 12;
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
            Row25.Height = 12;
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
            Row26Head.Height = 12;
            Row26Head.AutoFitHeight = false;
            cell = Row26Head.Cells.Add();
            cell.StyleID = "s139";
            cell = Row26Head.Cells.Add();
            cell.StyleID = "s143";
            cell = Row26Head.Cells.Add();
            cell.StyleID = "s139";
            cell = Row26Head.Cells.Add();
            cell.StyleID = "s170H";
            cell = Row26Head.Cells.Add();
            cell.StyleID = "s145";
            // -----------------------------------------------
            WorksheetRow Row27Head = sheet.Table.Rows.Add();
            Row27Head.Height = 12;
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
            Row28.Height = 12;
            Row28.AutoFitHeight = false;
            cell = Row28.Cells.Add();
            cell.StyleID = "s139";
            cell = Row28.Cells.Add();
            cell.StyleID = "s143";
            cell = Row28.Cells.Add();
            cell.StyleID = "s139";
            cell = Row28.Cells.Add();
            cell.StyleID = "s170H";
            cell = Row28.Cells.Add();
            cell.StyleID = "s145";
            // -----------------------------------------------
            WorksheetRow Row29 = sheet.Table.Rows.Add();
            Row29.Height = 12;
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
            Row30.Height = 12;
            Row30.AutoFitHeight = false;
            cell = Row30.Cells.Add();
            cell.StyleID = "s139";
            cell = Row30.Cells.Add();
            cell.StyleID = "s143";
            cell = Row30.Cells.Add();
            cell.StyleID = "s139";
            cell = Row30.Cells.Add();
            cell.StyleID = "s170H";
            cell = Row30.Cells.Add();
            cell.StyleID = "s145";
            // -----------------------------------------------
            WorksheetRow Row31 = sheet.Table.Rows.Add();
            Row31.Height = 12;
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
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, true, propReportPath);
            pdfListForm.StartPosition = FormStartPosition.CenterScreen;
            pdfListForm.ShowDialog();
        }
    }
}