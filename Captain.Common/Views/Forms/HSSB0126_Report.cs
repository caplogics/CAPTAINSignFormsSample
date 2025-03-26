using Captain.Common.Interfaces;
using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;
using Captain.Common.Views.Forms.Base;
using DevExpress.Data.NetCompatibility.Extensions;
using DevExpress.PivotGrid.OLAP.SchemaEntities;
using DevExpress.XtraRichEdit.Model;
using NPOI.HSSF.Util;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Wisej.Web;

namespace Captain.Common.Views.Forms
{
    public partial class HSSB0126_Report : Form
    {
        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;

        public HSSB0126_Report(BaseForm baseform, PrivilegeEntity privilegeEntity)
        {
            InitializeComponent();

            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            _baseForm = baseform;
            _privilegeEntity = privilegeEntity;

            propReportPath = _model.lookupDataAccess.GetReportPath();
            Set_Report_Hierarchy(_baseForm.BaseAgency, _baseForm.BaseDept, _baseForm.BaseProg, _baseForm.BaseYear);
        }

        #region Properties

        public BaseForm _baseForm
        {
            get;
            set;
        }
        public PrivilegeEntity _privilegeEntity
        {
            get; set;
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
        public string propReportPath
        {
            get; set;
        }

        #endregion

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
                this.Txt_HieDesc.Size = new System.Drawing.Size(665, 25);
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
                this.Txt_HieDesc.Size = new System.Drawing.Size(560, 25);
            else
                this.Txt_HieDesc.Size = new System.Drawing.Size(665, 25);
        }

        #endregion

        private bool ValidateReport()
        {
            bool isValid = true;

            _errorProvider.SetError(dtpFromDte, null);
            _errorProvider.SetError(dtpToDte, null);
            _errorProvider.SetError(rdbFundSel, null);
            _errorProvider.SetError(rdbSiteSel, null);
            _errorProvider.SetError(rdbServTypeSel, null);

            if (dtpFromDte.Checked == false)
            {
                _errorProvider.SetError(dtpFromDte, string.Format("From Date is required"));
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtpFromDte, null);
            }
            if (dtpToDte.Checked == false)
            {
                _errorProvider.SetError(dtpToDte, string.Format("To Date is required"));
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
                    _errorProvider.SetError(dtpFromDte, string.Format("From Date is required"));
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
                    _errorProvider.SetError(dtpToDte, string.Format("To Date is required"));
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
                    _errorProvider.SetError(dtpFromDte, string.Format("From Date is required"));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtpFromDte, null);
                }
                if (string.IsNullOrWhiteSpace(dtpToDte.Text))
                {
                    _errorProvider.SetError(dtpToDte, string.Format("To Date is required"));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtpToDte, null);
                }
            }

            if (dtpFromDte.Checked && dtpToDte.Checked)
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

            if (dtpFromDte.Checked && dtpToDte.Checked)
            {
                if (!string.IsNullOrWhiteSpace(dtpFromDte.Text) && !string.IsNullOrWhiteSpace(dtpToDte.Text))
                {
                    TimeSpan diffMonths = Convert.ToDateTime(dtpToDte.Text) - Convert.ToDateTime(dtpFromDte.Text);

                    if (diffMonths.TotalDays > 365)
                    {
                        _errorProvider.SetError(dtpToDte, "The Date Range should be within 1 Year");
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(dtpToDte, null);
                    }
                }
            }

            if (rdbFundSel.Checked)
            {
                if (SelFundingList.Count == 0)
                {
                    _errorProvider.SetError(rdbFundSel, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblFund.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                    _errorProvider.SetError(rdbFundSel, null);
            }

            if (rdbSiteSel.Checked)
            {
                if (selSites.Count == 0)
                {
                    _errorProvider.SetError(rdbSiteSel, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblSite.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                    _errorProvider.SetError(rdbSiteSel, null);
            }

            if (rdbServTypeSel.Checked)
            {
                if (selServTypes.Count == 0)
                {
                    _errorProvider.SetError(rdbServTypeSel, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblServType.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                    _errorProvider.SetError(rdbServTypeSel, null);
            }

            return isValid;
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (ValidateReport())
            {
                PdfListForm pdfListForm = new PdfListForm(_baseForm, _privilegeEntity, false, propReportPath, "XLSX");
                pdfListForm.FormClosed += new FormClosedEventHandler(On_SaveFormClosed);
                pdfListForm.StartPosition = FormStartPosition.CenterScreen;
                pdfListForm.ShowDialog();
            }
        }
        string Random_Filename = null;
        private void On_SaveFormClosed(object sender, FormClosedEventArgs e)
        {
            PdfListForm form = sender as PdfListForm;
            if (form.DialogResult == DialogResult.OK)
            {
                #region FileNameBuild

                Random_Filename = null;
                string xlFileName = form.GetFileName();
                xlFileName = propReportPath + _baseForm.UserID + "\\" + xlFileName;
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

                int Cnt = 0;
                string Appno = string.Empty;

                try
                {
                    using (DevExpress.Spreadsheet.Workbook wb = new DevExpress.Spreadsheet.Workbook())
                    {
                        DevExpress_Excel_Properties oDevExpress_Excel_Properties = new DevExpress_Excel_Properties();
                        oDevExpress_Excel_Properties.sxlbook = wb;
                        oDevExpress_Excel_Properties.sxlTitleFont = "calibri";
                        oDevExpress_Excel_Properties.sxlbodyFont = "calibri";

                        oDevExpress_Excel_Properties.getDevexpress_Excel_Properties();

                        #region Parameters Page

                        DevExpress.Spreadsheet.Worksheet paramSheet = wb.Worksheets[0];
                        paramSheet.Name = "Params";
                        paramSheet.ActiveView.TabColor = Color.ForestGreen;
                        paramSheet.ActiveView.ShowGridlines = false;
                        wb.Unit = DevExpress.Office.DocumentUnit.Point;

                        paramSheet.Columns[1].Width = 100;//80;
                        paramSheet.Columns[2].Width = 200;
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
                        DevExpress.Spreadsheet.Picture pic = paramSheet.Pictures.AddPicture(imgsrc, 50, 0, 150, 70);


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
                        paramSheet.Rows[_Rowindex][_Columnindex].Value = "Date Range";
                        paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.paramsCellStyle;
                        _Columnindex++;

                        paramSheet.Rows[_Rowindex][_Columnindex].Value = "Start Date: " + dtpFromDte.Text + "  " + "End Date: " + dtpToDte.Text;
                        oDevExpress_Excel_Properties.xlRowsMerge(paramSheet, _Rowindex, _Columnindex, 4, oDevExpress_Excel_Properties.gxlNLC);
                        _Columnindex = _Columnindex + 4;

                        _Rowindex++;
                        _Columnindex = 1;
                        paramSheet.Rows[_Rowindex][_Columnindex].Value = rdbFundAll.Checked ? lblFund.Text.Trim() : "Selected Funding Sources";
                        paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.paramsCellStyle;
                        _Columnindex++;

                        string fundNames = string.Empty;

                        foreach (CommonEntity FundingCode in SelFundingList)
                        {
                            if (!fundNames.Equals(string.Empty))
                                fundNames += ",";
                            fundNames += FundingCode.Desc;
                        }

                        paramSheet.Rows[_Rowindex][_Columnindex].Value = rdbFundAll.Checked ? "All Funding Sources" : fundNames.Trim().TrimEnd(',');
                        oDevExpress_Excel_Properties.xlRowsMerge(paramSheet, _Rowindex, _Columnindex, 4, oDevExpress_Excel_Properties.gxlNLC);
                        _Columnindex = _Columnindex + 4;

                        _Rowindex++;
                        _Columnindex = 1;
                        paramSheet.Rows[_Rowindex][_Columnindex].Value = rdbSiteAll.Checked ? lblSite.Text.Trim() : "Selected Sites";
                        paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.paramsCellStyle;
                        _Columnindex++;

                        string siteNames = string.Empty;

                        foreach (CaseSiteEntity SiteCode in selSites)
                        {
                            if (!siteNames.Equals(string.Empty))
                                siteNames += ",";
                            siteNames += SiteCode.SiteNAME;
                        }

                        paramSheet.Rows[_Rowindex][_Columnindex].Value = rdbSiteAll.Checked ? "All Sites" : siteNames.Trim().TrimEnd(',');
                        oDevExpress_Excel_Properties.xlRowsMerge(paramSheet, _Rowindex, _Columnindex, 4, oDevExpress_Excel_Properties.gxlNLC);
                        _Columnindex = _Columnindex + 4;

                        _Rowindex++;
                        _Columnindex = 1;
                        paramSheet.Rows[_Rowindex][_Columnindex].Value = rdbServTypeAll.Checked ? lblServType.Text.Trim() : "Selected Service Types";
                        paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.paramsCellStyle;
                        _Columnindex++;

                        string servTypeNames = string.Empty;

                        foreach (CommonEntity servTypeCode in selServTypes)
                        {
                            if (!servTypeNames.Equals(string.Empty))
                                servTypeNames += ",";
                            servTypeNames += servTypeCode.Desc;
                        }

                        paramSheet.Rows[_Rowindex][_Columnindex].Value = rdbServTypeAll.Checked ? "All Service Types" : servTypeNames.Trim().TrimEnd(',');
                        oDevExpress_Excel_Properties.xlRowsMerge(paramSheet, _Rowindex, _Columnindex, 4, oDevExpress_Excel_Properties.gxlNLC);
                        _Columnindex = _Columnindex + 4;

                        _Rowindex++;
                        _Columnindex = 1;
                        paramSheet.Rows[_Rowindex][_Columnindex].Value = "";
                        _Columnindex = _Columnindex + 5;

                        _Rowindex = _Rowindex + 3;
                        _Columnindex = 5;
                        paramSheet.Rows[_Rowindex][_Columnindex].Value = "Generated By: " + _baseForm.UserID;
                        paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlGenerate_lr;

                        _Rowindex++;
                        paramSheet.Rows[_Rowindex][_Columnindex].Value = "Generated On: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");
                        paramSheet.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlGenerate_lr;

                        // Determine the last used row and column
                        int lastRow = paramSheet.GetUsedRange().BottomRowIndex;
                        int lastColumn = paramSheet.GetUsedRange().RightColumnIndex;

                        // Autofit all columns from the first to the last used column
                        for (int i = 0; i <= lastColumn; i++)
                        {
                            paramSheet.Columns[i].AutoFit();
                        }

                        // Autofit all rows from the first to the last used row
                        for (int i = 0; i <= lastRow; i++)
                        {
                            paramSheet.Rows[i].AutoFit();
                        }

                        paramSheet.IgnoredErrors.Add(paramSheet.GetDataRange(), DevExpress.Spreadsheet.IgnoredErrorType.NumberAsText);

                        #endregion

                        #region Get Data

                        string Year = string.Empty;
                        if (CmbYear.Visible == true)
                            Year = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString().Trim();

                        string funds = string.Empty;

                        if (SelFundingList.Count > 0)
                        {
                            funds = string.Empty;
                            foreach (CommonEntity entity in SelFundingList)
                            {
                                funds += entity.Code + ",";
                            }
                        }
                        else
                            funds = "A";

                        string sites = string.Empty;

                        if (selSites.Count > 0)
                        {
                            sites = string.Empty;
                            foreach (CaseSiteEntity entity in selSites)
                            {
                                sites += entity.SiteNUMBER + ",";
                            }
                        }
                        else
                            sites = "A";

                        string servTypes = string.Empty;

                        if (selServTypes.Count > 0)
                        {
                            servTypes = string.Empty;
                            foreach (CommonEntity entity in selServTypes)
                            {
                                servTypes += entity.Code + ",";
                            }
                        }
                        else
                            servTypes = "A";

                        DataSet dsInkind = DatabaseLayer.SPAdminDB.HSSB0126_Report(Current_Hierarchy_DB.Substring(0, 2), Current_Hierarchy_DB.Substring(2, 2), Current_Hierarchy_DB.Substring(4, 2), Year, dtpFromDte.Text, dtpToDte.Text, funds.Trim().TrimEnd(','), sites.Trim().TrimEnd(','), servTypes.Trim().TrimEnd(','));

                        DataTable dtInkind = dsInkind.Tables[0];

                        #endregion

                        if (dtInkind.Rows.Count > 0)
                        {
                            #region Excel Data

                            DevExpress.Spreadsheet.Worksheet SheetDetails = wb.Worksheets.Add("In_Kind Details");

                            SheetDetails.ActiveView.TabColor = ColorTranslator.FromHtml("#ADD8E6");

                            SheetDetails.ActiveView.ShowGridlines = false;
                            wb.Unit = DevExpress.Office.DocumentUnit.Point;

                            #region Column Widths

                            _Rowindex = 0;
                            _Columnindex = 0;

                            SheetDetails.Columns[0].Width = 120;//Service Type

                            //Months Columns Loop based on given date range
                            DateTime FromDate = Convert.ToDateTime(dtpFromDte.Text);
                            DateTime ToDate = Convert.ToDateTime(dtpToDte.Text);
                            int NoMonths = GetMonthsBetween(FromDate, ToDate) + 1;

                            _Columnindex = 1;

                            for (int xx = 0; xx < NoMonths; xx++)
                            {
                                SheetDetails.Columns[_Columnindex].Width = 75;
                                _Columnindex++;
                            }

                            SheetDetails.Columns[_Columnindex].Width = 80;//Total

                            #endregion

                            #region Column Headers

                            _Rowindex = _Columnindex = 0;

                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Service Type";
                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlBLHC;
                            SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 11;
                            _Columnindex++;

                            DateTime ct = Convert.ToDateTime(dtpFromDte.Text);
                            for (int x = 0; x < NoMonths; x++)
                            {
                                if (x > 0)
                                    ct = ct.AddMonths(1);

                                string MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(Convert.ToInt16(ct.Month)) + "-" + ct.Year.ToString();
                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = MonthName;
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlBCHC;
                                SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 11;
                                _Columnindex++;
                            }

                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Total";
                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlBRHC;
                            SheetDetails.Rows[_Rowindex][_Columnindex].Font.Size = 11;

                            #endregion

                            DataTable dtreorder = dtInkind;

                            if (dtreorder.Columns.Contains("JAN"))
                            {
                                dtreorder.Columns["JAN"].SetOrdinal(0);
                            }

                            #region Data Printing Loop

                            _Rowindex = 0;
                            _Columnindex = 0;

                            int introwindex = 0;

                            foreach (DataRow drInkind in dtInkind.Rows)
                            {
                                _Rowindex++;
                                _Columnindex = 0;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = drInkind["ServiceType"].ToString();
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNLC;
                                _Columnindex++;

                                //int NoMonth = GetMonthsBetween(FromDate, ToDate) + 1;
                                ct = Convert.ToDateTime(dtpFromDte.Text);

                                for (int x = 0; x < NoMonths; x++)
                                {
                                    if (x > 0)
                                        ct = ct.AddMonths(1);

                                    string MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(Convert.ToInt16(ct.Month)); //+ "-" + ct.Year.ToString();

                                    //if (dtInkind.Columns.Contains((MonthName.ToUpper())))
                                    //{
                                    if (dtInkind.Columns.Contains("JAN"))
                                    {
                                        if (dtInkind.Columns["JAN"].ColumnName.Equals(MonthName.ToUpper()))
                                        {
                                            introwindex++;

                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = drInkind["JAN"].ToString() == "" ? 0 : Convert.ToDecimal(drInkind["JAN"].ToString());
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNRC;
                                            SheetDetails.Rows[_Rowindex][_Columnindex].NumberFormat = "$#,##0.00";
                                            _Columnindex++;
                                        }

                                    }

                                    if (dtInkind.Columns.Contains("FEB"))
                                    {
                                        if (dtInkind.Columns["FEB"].ColumnName.Equals(MonthName.ToUpper()))
                                        {
                                            introwindex++;

                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = drInkind["FEB"].ToString() == "" ? 0 : Convert.ToDecimal(drInkind["FEB"].ToString());
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNRC;
                                            SheetDetails.Rows[_Rowindex][_Columnindex].NumberFormat = "$#,##0.00";
                                            _Columnindex++;
                                        }

                                    }

                                    if (dtInkind.Columns.Contains("MAR"))
                                    {
                                        if (dtInkind.Columns["MAR"].ColumnName.Equals(MonthName.ToUpper()))
                                        {
                                            introwindex++;

                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = drInkind["MAR"].ToString() == "" ? 0 : Convert.ToDecimal(drInkind["MAR"].ToString());
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNRC;
                                            SheetDetails.Rows[_Rowindex][_Columnindex].NumberFormat = "$#,##0.00";
                                            _Columnindex++;
                                        }

                                    }

                                    if (dtInkind.Columns.Contains("APR"))
                                    {
                                        if (dtInkind.Columns["APR"].ColumnName.Equals(MonthName.ToUpper()))
                                        {
                                            introwindex++;

                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = drInkind["APR"].ToString() == "" ? 0 : Convert.ToDecimal(drInkind["APR"].ToString());
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNRC;
                                            SheetDetails.Rows[_Rowindex][_Columnindex].NumberFormat = "$#,##0.00";
                                            _Columnindex++;
                                        }

                                    }

                                    if (dtInkind.Columns.Contains("MAY"))
                                    {
                                        if (dtInkind.Columns["MAY"].ColumnName.Equals(MonthName.ToUpper()))
                                        {
                                            introwindex++;

                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = drInkind["MAY"].ToString() == "" ? 0 : Convert.ToDecimal(drInkind["MAY"].ToString());
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNRC;
                                            SheetDetails.Rows[_Rowindex][_Columnindex].NumberFormat = "$#,##0.00";
                                            _Columnindex++;
                                        }

                                    }

                                    if (dtInkind.Columns.Contains("JUN"))
                                    {
                                        if (dtInkind.Columns["JUN"].ColumnName.Equals(MonthName.ToUpper()))
                                        {
                                            introwindex++;

                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = drInkind["JUN"].ToString() == "" ? 0 : Convert.ToDecimal(drInkind["JUN"].ToString());
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNRC;
                                            SheetDetails.Rows[_Rowindex][_Columnindex].NumberFormat = "$#,##0.00";
                                            _Columnindex++;
                                        }

                                    }

                                    if (dtInkind.Columns.Contains("JUL"))
                                    {
                                        if (dtInkind.Columns["JUL"].ColumnName.Equals(MonthName.ToUpper()))
                                        {
                                            introwindex++;

                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = drInkind["JUL"].ToString() == "" ? 0 : Convert.ToDecimal(drInkind["JUL"].ToString());
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNRC;
                                            SheetDetails.Rows[_Rowindex][_Columnindex].NumberFormat = "$#,##0.00";
                                            _Columnindex++;
                                        }

                                    }

                                    if (dtInkind.Columns.Contains("AUG"))
                                    {
                                        if (dtInkind.Columns["AUG"].ColumnName.Equals(MonthName.ToUpper()))
                                        {
                                            introwindex++;

                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = drInkind["AUG"].ToString() == "" ? 0 : Convert.ToDecimal(drInkind["AUG"].ToString());
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNRC;
                                            SheetDetails.Rows[_Rowindex][_Columnindex].NumberFormat = "$#,##0.00";
                                            _Columnindex++;
                                        }

                                    }

                                    if (dtInkind.Columns.Contains("SEP"))
                                    {
                                        if (dtInkind.Columns["SEP"].ColumnName.Equals(MonthName.ToUpper()))
                                        {
                                            introwindex++;

                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = drInkind["SEP"].ToString() == "" ? 0 : Convert.ToDecimal(drInkind["SEP"].ToString());
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNRC;
                                            SheetDetails.Rows[_Rowindex][_Columnindex].NumberFormat = "$#,##0.00";
                                            _Columnindex++;
                                        }

                                    }

                                    if (dtInkind.Columns.Contains("OCT"))
                                    {
                                        if (dtInkind.Columns["OCT"].ColumnName.Equals(MonthName.ToUpper()))
                                        {
                                            introwindex++;

                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = drInkind["OCT"].ToString() == "" ? 0 : Convert.ToDecimal(drInkind["OCT"].ToString());
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNRC;
                                            SheetDetails.Rows[_Rowindex][_Columnindex].NumberFormat = "$#,##0.00";
                                            _Columnindex++;
                                        }

                                    }

                                    if (dtInkind.Columns.Contains("NOV"))
                                    {
                                        if (dtInkind.Columns["NOV"].ColumnName.Equals(MonthName.ToUpper()))
                                        {
                                            introwindex++;

                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = drInkind["NOV"].ToString() == "" ? 0 : Convert.ToDecimal(drInkind["NOV"].ToString());
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNRC;
                                            SheetDetails.Rows[_Rowindex][_Columnindex].NumberFormat = "$#,##0.00";
                                            _Columnindex++;
                                        }

                                    }

                                    if (dtInkind.Columns.Contains("DEC"))
                                    {
                                        if (dtInkind.Columns["DEC"].ColumnName.Equals(MonthName.ToUpper()))
                                        {
                                            introwindex++;

                                            SheetDetails.Rows[_Rowindex][_Columnindex].Value = drInkind["DEC"].ToString() == "" ? 0 : Convert.ToDecimal(drInkind["DEC"].ToString());
                                            SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNRC;
                                            SheetDetails.Rows[_Rowindex][_Columnindex].NumberFormat = "$#,##0.00";
                                            _Columnindex++;
                                        }

                                    }
                                    //}
                                }


                                /*SheetDetails.Rows[_Rowindex][_Columnindex].Value = drInkind["ServiceType"].ToString();
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNLC;
                                _Columnindex++;

                                if (dtInkind.Columns.Contains("JAN"))
                                {
                                    introwindex++;

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = drInkind["JAN"].ToString() == "" ? 0 : Convert.ToDecimal(drInkind["JAN"].ToString());
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNRC;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].NumberFormat = "$#,##0.00";
                                    _Columnindex++;
                                }

                                if (dtInkind.Columns.Contains("FEB"))
                                {
                                    introwindex++;

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = drInkind["FEB"].ToString() == "" ? 0 : Convert.ToDecimal(drInkind["FEB"].ToString());
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNRC;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].NumberFormat = "$#,##0.00";
                                    _Columnindex++;
                                }

                                if (dtInkind.Columns.Contains("MAR"))
                                {
                                    introwindex++;

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = drInkind["MAR"].ToString() == "" ? 0 : Convert.ToDecimal(drInkind["MAR"].ToString());
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNRC;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].NumberFormat = "$#,##0.00";
                                    _Columnindex++;
                                }

                                if (dtInkind.Columns.Contains("APR"))
                                {
                                    introwindex++;

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = drInkind["APR"].ToString() == "" ? 0 : Convert.ToDecimal(drInkind["APR"].ToString());
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNRC;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].NumberFormat = "$#,##0.00";
                                    _Columnindex++;
                                }

                                if (dtInkind.Columns.Contains("MAY"))
                                {
                                    introwindex++;

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = drInkind["MAY"].ToString() == "" ? 0 : Convert.ToDecimal(drInkind["MAY"].ToString());
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNRC;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].NumberFormat = "$#,##0.00";
                                    _Columnindex++;
                                }

                                if (dtInkind.Columns.Contains("JUN"))
                                {
                                    introwindex++;

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = drInkind["JUN"].ToString() == "" ? 0 : Convert.ToDecimal(drInkind["JUN"].ToString());
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNRC;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].NumberFormat = "$#,##0.00";
                                    _Columnindex++;
                                }

                                if (dtInkind.Columns.Contains("JUL"))
                                {
                                    introwindex++;

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = drInkind["JUL"].ToString() == "" ? 0 : Convert.ToDecimal(drInkind["JUL"].ToString());
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNRC;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].NumberFormat = "$#,##0.00";
                                    _Columnindex++;
                                }

                                if (dtInkind.Columns.Contains("AUG"))
                                {
                                    introwindex++;

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = drInkind["AUG"].ToString() == "" ? 0 : Convert.ToDecimal(drInkind["AUG"].ToString());
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNRC;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].NumberFormat = "$#,##0.00";
                                    _Columnindex++;
                                }

                                if (dtInkind.Columns.Contains("SEP"))
                                {
                                    introwindex++;

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = drInkind["SEP"].ToString() == "" ? 0 : Convert.ToDecimal(drInkind["SEP"].ToString());
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNRC;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].NumberFormat = "$#,##0.00";
                                    _Columnindex++;
                                }

                                if (dtInkind.Columns.Contains("OCT"))
                                {
                                    introwindex++;

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = drInkind["OCT"].ToString() == "" ? 0 : Convert.ToDecimal(drInkind["OCT"].ToString());
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNRC;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].NumberFormat = "$#,##0.00";
                                    _Columnindex++;
                                }

                                if (dtInkind.Columns.Contains("NOV"))
                                {
                                    introwindex++;

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = drInkind["NOV"].ToString() == "" ? 0 : Convert.ToDecimal(drInkind["NOV"].ToString());
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNRC;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].NumberFormat = "$#,##0.00";
                                    _Columnindex++;
                                }

                                if (dtInkind.Columns.Contains("DEC"))
                                {
                                    introwindex++;

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Value = (drInkind["DEC"].ToString() == "" ? 0 : Convert.ToDecimal(drInkind["DEC"].ToString()));
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNRC;
                                    SheetDetails.Rows[_Rowindex][_Columnindex].NumberFormat = "$#,##0.00";
                                    _Columnindex++;
                                }*/

                                string columnLetter = GetExcelColumnName(_Columnindex);

                                string rangeAddress = $"{"B"}{_Rowindex + 1}:{columnLetter}{_Rowindex + 1}";

                                SheetDetails.Rows[_Rowindex][_Columnindex].Formula = $"=SUM({rangeAddress})";
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNRC_bo;
                                HideFormulaAfterCalculation(SheetDetails, _Rowindex, _Columnindex);
                                _Columnindex++;
                            }

                            if (introwindex > 0)
                            {
                                _Rowindex++;
                                _Columnindex = 0;

                                SheetDetails.Rows[_Rowindex][_Columnindex].Value = "Report Totals";
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNLC_bo;
                                _Columnindex++;
                                string columnLetter = string.Empty;
                                string rangeAddress = string.Empty;

                                //JAN
                                if (dtInkind.Columns.Contains("JAN"))
                                {
                                    columnLetter = GetExcelColumnName(_Columnindex + 1);
                                    rangeAddress = $"{columnLetter}{2}:{columnLetter}{_Rowindex}";

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Formula = $"=SUM({rangeAddress})";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNRC_bo;
                                    HideFormulaAfterCalculation(SheetDetails, _Rowindex, _Columnindex);
                                    _Columnindex++;
                                }

                                //FEB
                                if (dtInkind.Columns.Contains("FEB"))
                                {
                                    columnLetter = GetExcelColumnName(_Columnindex + 1);
                                    rangeAddress = $"{columnLetter}{2}:{columnLetter}{_Rowindex}";

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Formula = $"=SUM({rangeAddress})";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNRC_bo;
                                    HideFormulaAfterCalculation(SheetDetails, _Rowindex, _Columnindex);
                                    _Columnindex++;
                                }
                                //MAR
                                if (dtInkind.Columns.Contains("MAR"))
                                {
                                    columnLetter = GetExcelColumnName(_Columnindex + 1);
                                    rangeAddress = $"{columnLetter}{2}:{columnLetter}{_Rowindex}";

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Formula = $"=SUM({rangeAddress})";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNRC_bo;
                                    HideFormulaAfterCalculation(SheetDetails, _Rowindex, _Columnindex);
                                    _Columnindex++;
                                }
                                //APR
                                if (dtInkind.Columns.Contains("APR"))
                                {
                                    columnLetter = GetExcelColumnName(_Columnindex + 1);
                                    rangeAddress = $"{columnLetter}{2}:{columnLetter}{_Rowindex}";

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Formula = $"=SUM({rangeAddress})";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNRC_bo;
                                    HideFormulaAfterCalculation(SheetDetails, _Rowindex, _Columnindex);
                                    _Columnindex++;
                                }
                                //MAY
                                if (dtInkind.Columns.Contains("MAY"))
                                {
                                    columnLetter = GetExcelColumnName(_Columnindex + 1);
                                    rangeAddress = $"{columnLetter}{2}:{columnLetter}{_Rowindex}";

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Formula = $"=SUM({rangeAddress})";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNRC_bo;
                                    HideFormulaAfterCalculation(SheetDetails, _Rowindex, _Columnindex);
                                    _Columnindex++;
                                }
                                //JUN
                                if (dtInkind.Columns.Contains("JUN"))
                                {
                                    columnLetter = GetExcelColumnName(_Columnindex + 1);
                                    rangeAddress = $"{columnLetter}{2}:{columnLetter}{_Rowindex}";

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Formula = $"=SUM({rangeAddress})";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNRC_bo;
                                    HideFormulaAfterCalculation(SheetDetails, _Rowindex, _Columnindex);
                                    _Columnindex++;
                                }
                                //JUL
                                if (dtInkind.Columns.Contains("JUL"))
                                {
                                    columnLetter = GetExcelColumnName(_Columnindex + 1);
                                    rangeAddress = $"{columnLetter}{2}:{columnLetter}{_Rowindex}";

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Formula = $"=SUM({rangeAddress})";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNRC_bo;
                                    HideFormulaAfterCalculation(SheetDetails, _Rowindex, _Columnindex);
                                    _Columnindex++;
                                }
                                //AUG
                                if (dtInkind.Columns.Contains("AUG"))
                                {
                                    columnLetter = GetExcelColumnName(_Columnindex + 1);
                                    rangeAddress = $"{columnLetter}{2}:{columnLetter}{_Rowindex}";

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Formula = $"=SUM({rangeAddress})";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNRC_bo;
                                    HideFormulaAfterCalculation(SheetDetails, _Rowindex, _Columnindex);
                                    _Columnindex++;
                                }
                                //SEP
                                if (dtInkind.Columns.Contains("SEP"))
                                {
                                    columnLetter = GetExcelColumnName(_Columnindex + 1);
                                    rangeAddress = $"{columnLetter}{2}:{columnLetter}{_Rowindex}";

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Formula = $"=SUM({rangeAddress})";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNRC_bo;
                                    HideFormulaAfterCalculation(SheetDetails, _Rowindex, _Columnindex);
                                    _Columnindex++;
                                }
                                //OCT

                                if (dtInkind.Columns.Contains("OCT"))
                                {
                                    columnLetter = GetExcelColumnName(_Columnindex + 1);
                                    rangeAddress = $"{columnLetter}{2}:{columnLetter}{_Rowindex}";

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Formula = $"=SUM({rangeAddress})";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNRC_bo;
                                    HideFormulaAfterCalculation(SheetDetails, _Rowindex, _Columnindex);
                                    _Columnindex++;
                                }
                                //NOV
                                if (dtInkind.Columns.Contains("NOV"))
                                {
                                    columnLetter = GetExcelColumnName(_Columnindex + 1);
                                    rangeAddress = $"{columnLetter}{2}:{columnLetter}{_Rowindex}";

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Formula = $"=SUM({rangeAddress})";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNRC_bo;
                                    HideFormulaAfterCalculation(SheetDetails, _Rowindex, _Columnindex);
                                    _Columnindex++;
                                }

                                //DEC
                                if (dtInkind.Columns.Contains("DEC"))
                                {
                                    columnLetter = GetExcelColumnName(_Columnindex + 1);
                                    rangeAddress = $"{columnLetter}{2}:{columnLetter}{_Rowindex}";

                                    SheetDetails.Rows[_Rowindex][_Columnindex].Formula = $"=SUM({rangeAddress})";
                                    SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNRC_bo;
                                    HideFormulaAfterCalculation(SheetDetails, _Rowindex, _Columnindex);
                                    _Columnindex++;
                                }
                                //Sub-Total
                                columnLetter = GetExcelColumnName(_Columnindex + 1);
                                rangeAddress = $"{columnLetter}{2}:{columnLetter}{_Rowindex}";

                                SheetDetails.Rows[_Rowindex][_Columnindex].Formula = $"=SUM({rangeAddress})";
                                SheetDetails.Rows[_Rowindex][_Columnindex].Style = oDevExpress_Excel_Properties.gxlNRC_bo;
                                HideFormulaAfterCalculation(SheetDetails, _Rowindex, _Columnindex);
                                _Columnindex++;
                            }

                            #endregion

                            #endregion

                            SheetDetails.IgnoredErrors.Add(SheetDetails.GetDataRange(), DevExpress.Spreadsheet.IgnoredErrorType.NumberAsText);
                        }

                        #region File Saving and Downloading

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

                                AlertBox.Show("Report Generated Successfully");
                            }
                        }
                        catch (Exception ex)
                        {

                        }

                        #endregion
                    }
                }
                catch (Exception ex) { }

            }
        }
     
        public int GetMonthsBetween(DateTime from, DateTime to)
        {
            if (from > to)
                return GetMonthsBetween(to, from);

            var monthDiff = Math.Abs((to.Year * 12 + (to.Month - 1)) - (from.Year * 12 + (from.Month - 1)));

            if (from.AddMonths(monthDiff) > to || to.Day < from.Day)
            {
                return monthDiff - 1;
            }
            else
            {
                return monthDiff;
            }
        }
        public static string GetExcelColumnName(int columnIndex)
        {
            string columnName = string.Empty;
            while (columnIndex > 0)
            {
                int remainder = (columnIndex - 1) % 26;
                columnName = (char)(65 + remainder) + columnName; // 'A' is ASCII 65
                columnIndex = (columnIndex - 1) / 26;
            }
            return columnName;
        }
        private void HideFormulaAfterCalculation(DevExpress.Spreadsheet.Worksheet sheetDetails, int Rowindex, int Columnindex)
        {
            sheetDetails.Calculate();
            var calculatedValue = sheetDetails[Rowindex, Columnindex].Value;

            // Set the value directly to the cell and remove the formula
            sheetDetails[Rowindex, Columnindex].Value = calculatedValue; // Now it only contains the value
            sheetDetails.Rows[Rowindex][Columnindex].NumberFormat = "$#,##0.00";
            sheetDetails[Rowindex, Columnindex].Formula = null; // Clear the formula
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            PdfListForm pdfListForm = new PdfListForm(_baseForm, _privilegeEntity, true, propReportPath);
            pdfListForm.StartPosition = FormStartPosition.CenterScreen;
            pdfListForm.ShowDialog();
        }

        #region Save and Get Parameters

        private void btnGetParams_Click(object sender, EventArgs e)
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
        private void btnSaveParams_Click(object sender, EventArgs e)
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
        private string Get_XML_Format_for_Report_Controls()
        {
            string FromDate = string.Empty, ToDate = string.Empty, Fund = string.Empty, Site = string.Empty, ServType = string.Empty;
            string strFundingCodes = string.Empty;
            string strsiteRoomNames = string.Empty;
            string strsiteServTypes = string.Empty;

            FromDate = dtpFromDte.Text;
            ToDate = dtpToDte.Text;

            if (rdbFundAll.Checked)
            {
                Fund = "A";
            }
            else
            {
                if (rdbFundSel.Checked == true)
                {
                    foreach (CommonEntity FundingCode in SelFundingList)
                    {
                        if (!strFundingCodes.Equals(string.Empty))
                            strFundingCodes += ",";
                        strFundingCodes += FundingCode.Code;
                    }
                }
                Fund = "S";
            }

            if (rdbSiteAll.Checked)
            {
                Site = "A";
            }
            else
            {
                if (rdbSiteSel.Checked == true)
                {
                    foreach (CaseSiteEntity siteRoom in selSites)
                    {
                        if (!strsiteRoomNames.Equals(string.Empty))
                            strsiteRoomNames += ",";
                        strsiteRoomNames += siteRoom.SiteNUMBER + siteRoom.SiteROOM + siteRoom.SiteAM_PM;
                    }
                }
                Site = "S";
            }

            if (rdbServTypeAll.Checked)
            {
                ServType = "A";
            }
            else
            {
                if (rdbServTypeSel.Checked == true)
                {
                    foreach (CommonEntity ServTypeCode in selServTypes)
                    {
                        if (!strsiteServTypes.Equals(string.Empty))
                            strsiteServTypes += ",";
                        strsiteServTypes += ServTypeCode.Code;
                    }
                }
                ServType = "S";
            }

            string year = string.Empty;
            if (!string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString()))
                year = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString();

            StringBuilder str = new StringBuilder();
            str.Append("<Rows>");
            str.Append("<Row AGENCY = \"" + Current_Hierarchy.Substring(0, 2) + "\" DEPT = \"" + Current_Hierarchy.Substring(2, 2) + "\" PROG = \"" + Current_Hierarchy.Substring(4, 2) +
                            "\" YEAR = \"" + year + "\" FROMDATE = \"" + FromDate + "\" TODATE = \"" + ToDate + "\" FUND = \"" + Fund +"\" FUNDCODE = \"" + strFundingCodes + "\" SITE = \"" + Site + "\" SITES = \"" + strsiteRoomNames + "\" SERVTYPE = \"" + ServType + "\" SERVTYPES = \"" + strsiteServTypes + "\" />");

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

                if (dr["FUND"].ToString() == "A")
                    rdbFundAll.Checked = true;
                else
                {
                    List<CommonEntity> lookUpfundingResource = new List<CommonEntity>();
                    lookUpfundingResource = _model.lookupDataAccess.GetAgyTabRecordsByCodefilterFunds(Consts.AgyTab.CASEMNGMTFUNDSRC, "H");

                    if (dr["FUND"].ToString() == "S")
                        rdbFundSel.Checked = true;

                    SelFundingList.Clear();
                    string strFunds = dr["FUNDCODE"].ToString();
                    List<string> siteList = new List<string>();
                    if (strFunds != null)
                    {
                        string[] FundCodes = strFunds.Split(',');
                        for (int i = 0; i < FundCodes.Length; i++)
                        {
                            CommonEntity fundDetails = lookUpfundingResource.Find(u => u.Code == FundCodes.GetValue(i).ToString());
                            SelFundingList.Add(fundDetails);
                        }
                    }
                    SelFundingList = SelFundingList;
                }

                if (dr["SITE"].ToString() == "A")
                    rdbSiteAll.Checked = true;
                else
                {
                    if (dr["SITE"].ToString() == "S")
                        rdbSiteSel.Checked = true;

                    List<CaseSiteEntity> sitelist = new List<CaseSiteEntity>();
                    CaseSiteEntity Search_Site = new CaseSiteEntity(true);
                    Search_Site.SiteAGENCY = Current_Hierarchy.Substring(0, 2);
                    Search_Site.SiteDEPT = Current_Hierarchy.Substring(2, 2);
                    Search_Site.SitePROG = Current_Hierarchy.Substring(4, 2);
                    if (Program_Year != "All")
                        Search_Site.SiteYEAR = Program_Year.Trim();
                    sitelist = _model.CaseMstData.Browse_CASESITE(Search_Site, "Browse");
                    sitelist = sitelist.FindAll(u => u.SiteROOM.Trim() != "0000");
                    selSites.Clear();
                    string strSites = dr["SITES"].ToString();
                    List<string> siteList = new List<string>();
                    if (strSites != null)
                    {
                        string[] sitesRooms = strSites.Split(',');
                        for (int i = 0; i < sitesRooms.Length; i++)
                        {
                            CaseSiteEntity siteDetails = sitelist.Find(u => u.SiteNUMBER + u.SiteROOM + u.SiteAM_PM == sitesRooms.GetValue(i).ToString());
                            selSites.Add(siteDetails);
                        }
                    }
                    selSites = selSites;
                }

                if (dr["SERVTYPE"].ToString() == "A")
                    rdbServTypeAll.Checked = true;
                else
                {
                    List<CommonEntity> Ethnicity = CommonFunctions.AgyTabsFilterCode(_baseForm.BaseAgyTabsEntity, "05558", Current_Hierarchy.Substring(0, 2), Current_Hierarchy.Substring(2, 2), Current_Hierarchy.Substring(4, 2), string.Empty);

                    if (dr["SERVTYPE"].ToString() == "S")
                        rdbServTypeSel.Checked = true;

                    selServTypes.Clear();
                    string strServTypes = dr["SERVTYPES"].ToString();
                    List<string> siteList = new List<string>();
                    if (strServTypes != null)
                    {
                        string[] ServTypeCodes = strServTypes.Split(',');
                        for (int i = 0; i < ServTypeCodes.Length; i++)
                        {
                            CommonEntity ServTypeDetails = Ethnicity.Find(u => u.Code == ServTypeCodes.GetValue(i).ToString());
                            selServTypes.Add(ServTypeDetails);
                        }
                    }
                    selServTypes = selServTypes;
                }
            }
        }

        #endregion

        #region Radio button Click Events

        private void rdbFundAll_Click(object sender, EventArgs e)
        {
            SelFundingList.Clear();
            _errorProvider.SetError(rdbFundSel, null);
        }

        private void rdbFundSel_Click(object sender, EventArgs e)
        {
            HSSB2111FundForm FundingForm = new HSSB2111FundForm(_baseForm, SelFundingList, _privilegeEntity, 1);
            FundingForm.FormClosed += new FormClosedEventHandler(FundingForm_FormClosed);
            FundingForm.StartPosition = FormStartPosition.CenterScreen;
            FundingForm.ShowDialog();
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
        private void rdbSiteAll_Click(object sender, EventArgs e)
        {
            _errorProvider.SetError(rdbSiteSel, null);
            selSites.Clear();
        }

        private void rdbSiteSel_Click(object sender, EventArgs e)
        {
            string year = string.Empty;
            if(!string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString()))
                    year = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString();

            Site_SelectionForm SiteSelection = new Site_SelectionForm(_baseForm, "Room", selSites, "Report", Current_Hierarchy.Substring(0, 2), Current_Hierarchy.Substring(2, 2), Current_Hierarchy.Substring(4, 2), year, _privilegeEntity);
            SiteSelection.FormClosed += new FormClosedEventHandler(SelectedSiteFormClosed);
            SiteSelection.StartPosition = FormStartPosition.CenterScreen;
            SiteSelection.ShowDialog();

            //SelectZipSiteCountyForm siteForm = new SelectZipSiteCountyForm(_baseForm, _privilegeEntity, Current_Hierarchy.Substring(0, 2), Current_Hierarchy.Substring(2, 2),Current_Hierarchy.Substring(4, 2), ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString(), selSites);
            //siteForm.FormClosed += new FormClosedEventHandler(SelectedSiteFormClosed);
            //siteForm.StartPosition = FormStartPosition.CenterScreen;
            //siteForm.ShowDialog();
        }

        List<CaseSiteEntity> selSites = new List<CaseSiteEntity>();
        private void SelectedSiteFormClosed(object sender, FormClosedEventArgs e)
        {
            Site_SelectionForm form = sender as Site_SelectionForm;
            if (form.DialogResult == DialogResult.OK)
            {
                selSites = form.GetSelected_Sites();

                //if (Sel_REFS_List.Count > 0)
                //{
                //    CommonFunctions.SetComboBoxValue(cmbEnrlStatus, "E");
                //    cmbEnrlStatus.Enabled = false;
                //}
            }

            //SelectZipSiteCountyForm form = sender as SelectZipSiteCountyForm;
            //if (form.DialogResult == DialogResult.OK)
            //{
            //    selSites = form.GetSelectedSites();
            //}
        }

        private void rdbServTypeAll_Click(object sender, EventArgs e)
        {
            _errorProvider.SetError(rdbServTypeSel, null);
            selServTypes.Clear();
        }

        private void rdbServTypeSel_Click(object sender, EventArgs e)
        {
            SelectZipSiteCountyForm siteForm = new SelectZipSiteCountyForm(_baseForm, _privilegeEntity, Current_Hierarchy.Substring(0, 2), Current_Hierarchy.Substring(2, 2), Current_Hierarchy.Substring(4, 2), ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString(), selServTypes);
            siteForm.FormClosed += new FormClosedEventHandler(SelServTypeFormClosed);
            siteForm.StartPosition = FormStartPosition.CenterScreen;
            siteForm.ShowDialog();
        }

        List<CommonEntity> selServTypes = new List<CommonEntity>();
        private void SelServTypeFormClosed(object sender, FormClosedEventArgs e)
        {
            SelectZipSiteCountyForm form = sender as SelectZipSiteCountyForm;
            if (form.DialogResult == DialogResult.OK)
            {
                selServTypes = form.GetSelectedServTypes();
            }
        }

        #endregion
    }
}
