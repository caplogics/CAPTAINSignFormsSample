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
using DevExpress.CodeParser;
using DevExpress.XtraPrinting.Export;
using NPOI.SS.UserModel;
using System.Xml.Linq;
using DevExpress.XtraExport.Implementation;
using DevExpress.PivotGrid.OLAP.Mdx;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class LPMQ0001_Report : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private bool boolChangeStatus = false;
        private CaptainModel _model = null;
        private List<HierarchyEntity> _selectedHierarchies = null;

        #endregion

        public LPMQ0001_Report(BaseForm baseForm, PrivilegeEntity privileges)
        {
            InitializeComponent();

            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            BaseForm = baseForm;
            Privileges = privileges;
            Agency = BaseForm.BaseAgency; Depart = BaseForm.BaseDept; Program = BaseForm.BaseProg;
            strYear = BaseForm.BaseYear;

            Set_Report_Hierarchy(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear);

            propReportPath = _model.lookupDataAccess.GetReportPath();
            this.Text =/* Privileges.Program + " - " +*/ Privileges.PrivilegeName.Trim();
        }

        string Agency = string.Empty, Depart = string.Empty, Program = string.Empty, strYear = string.Empty;
        #region properties

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileges { get; set; }
        public List<CaseHierarchyEntity> propCaseHieEntity { get; set; }
        public List<CaseSiteEntity> propCaseAllSiteEntity { get; set; }
        public List<CaseMstEntity> propcasemstAlllist { get; set; }

        public string propReportPath { get; set; }


        #endregion

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
            {
                FillYearCombo(Agy, Dept, Prog, Year);
                //rdoMultipleSites.Enabled = true;
            }
            else
            {
                this.Txt_HieDesc.Size = new System.Drawing.Size(740, 25);
                //rdoMultipleSites.Enabled = false;
            }
        }

        string DepYear;
        bool DefHieExist = false;
        private void FillYearCombo(string Agy, string Dept, string Prog, string Year)
        {
            //CmbYear.Items.Clear();
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
                this.Txt_HieDesc.Size = new System.Drawing.Size(660, 25);
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

        
        private void btnGeneratePdf_Click(object sender, EventArgs e)
        {
            PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, false, propReportPath, "PDF");
            if (rbDetail.Checked)
            {
                if(rbLPMQ.Checked)
                    pdfListForm.FormClosed += new FormClosedEventHandler(On_SaveFormClosed1);
                else
                    pdfListForm.FormClosed += new FormClosedEventHandler(On_LIHEAPW_Detail);
            }
            else
            {
                if (rbLPMQ.Checked)
                    pdfListForm.FormClosed += new FormClosedEventHandler(On_SaveFormClosed);
                else
                    pdfListForm.FormClosed += new FormClosedEventHandler(On_LIHEAPW_Summary);
            }
            pdfListForm.StartPosition = FormStartPosition.CenterScreen;
            pdfListForm.ShowDialog();
        }

        private void btnPdfPreview_Click(object sender, EventArgs e)
        {
            PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, true, propReportPath);
            pdfListForm.StartPosition = FormStartPosition.CenterScreen;
            pdfListForm.ShowDialog();
        }

        private void btnSaveParameters_Click(object sender, EventArgs e)
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

        private string Get_XML_Format_for_Report_Controls()
        {
            string Rep1 = string.Empty, Rep = string.Empty; string HeatSrc = string.Empty;

            if (rbLPMQ.Checked == true) 
            { 
                Rep1 = "L";
                //pnlHSource.Visible = true;
            } 
            else if (rbLIHWAP.Checked == true) 
            {
                Rep1 = "L1";
                //pnlHSource.Visible = false;
            }

            if (rbSummary.Checked == true) Rep = "S"; else if (rbDetail.Checked == true) Rep = "D"; //else if (rbBoth.Checked == true) Rep = "B";
            if (rbDeliverables.Checked == true) HeatSrc = "D"; else if (rbUtilities.Checked == true) HeatSrc = "U"; else if (rbHeatBoth.Checked == true) HeatSrc = "B";


            //string Auth = rbYes.Checked == true ? "Y" : "N";
            //string ChkPay = string.Empty; string Summary = string.Empty;
            //if (chkbPaymnets.Checked == true) ChkPay = "Y"; else ChkPay = "N";
            //if (chkbSummary.Checked == true) Summary = "Y"; else Summary = "N";

            //string Vendor = string.Empty;
            //string rbvendor = string.Empty;
            //string VendorName = string.Empty;
            //if (rbDNo.Checked == true)
            //{
            //    rbvendor = rbAll.Checked == true ? "A" : "S";
            //    Vendor = Vendor_Code; VendorName = txtVendorName.Text.Trim();
            //}


            string Year = string.Empty;
            if (CmbYear.Visible == true)
                Year = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString();

            string Excel = "N";
            if (chkbExcel.Checked)
                Excel = "Y";

            StringBuilder str = new StringBuilder();
            str.Append("<Rows>");
            str.Append("<Row AGENCY = \"" + Current_Hierarchy_DB.Substring(0,2) + "\" DEPT = \"" + Current_Hierarchy_DB.Substring(3,2) + "\" PROG = \"" + Current_Hierarchy_DB.Substring(6,2) +
            "\" YEAR = \"" + Year + 
                            "\" Rep1 = \"" + Rep1 +
                            "\" Sortby = \"" + Rep + "\" HeatSource = \"" + HeatSrc + "\" EXCEL = \"" + Excel + "\" />");

            str.Append("</Rows>");

            return str.ToString();
        }

        private void Set_Report_Controls(DataTable Tmp_Table)
        {
            if (Tmp_Table != null && Tmp_Table.Rows.Count > 0)
            {
                DataRow dr = Tmp_Table.Rows[0];

                Set_Report_Hierarchy(dr["AGENCY"].ToString(), dr["DEPT"].ToString(), dr["PROG"].ToString(), dr["YEAR"].ToString());

                if (dr["Rep1"].ToString().Trim() == "L") 
                { 
                    rbLPMQ.Checked = true;
                    pnlHSource.Visible = true;
                }
                else if (dr["Rep1"].ToString().Trim() == "L1") 
                {
                    rbLIHWAP.Checked = true;
                    pnlHSource.Visible = false;
                }

                if (dr["Sortby"].ToString().Trim() == "S") rbSummary.Checked = true;
                else if (dr["Sortby"].ToString().Trim() == "D") rbDetail.Checked = true;
                //else if (dr["Sortby"].ToString().Trim() == "B") rbBoth.Checked = true;

                if (dr["HeatSource"].ToString().Trim() == "D") rbDeliverables.Checked = true;
                else if (dr["HeatSource"].ToString().Trim() == "U") rbUtilities.Checked = true;
                else if (dr["HeatSource"].ToString().Trim() == "B") rbHeatBoth.Checked = true;

                chkbExcel.Checked = false;
                if (dr["EXCEL"].ToString() == "Y")
                    chkbExcel.Checked = true;
                //CommonFunctions.SetComboBoxValue(cmbSelectClient, dr["SelectClient"].ToString());
                //CommonFunctions.SetComboBoxValue(cmbCounty, dr["county"].ToString());

            }
        }

        private void Pb_Search_Hie_Click(object sender, EventArgs e)
        {
           /* HierarchieSelectionFormNew hierarchieSelectionForm = new HierarchieSelectionFormNew(BaseForm, Current_Hierarchy_DB, "Master", "A", "D", "Reports");
            hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
            hierarchieSelectionForm.StartPosition = FormStartPosition.CenterScreen;
            hierarchieSelectionForm.ShowDialog();*/

            HierarchieSelection hierarchieSelectionForm = new HierarchieSelection(BaseForm, Current_Hierarchy_DB, "Master", "A", "D", "Reports", BaseForm.UserID);
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

                }
            }
        }


        PdfContentByte cb;
        string strFolderPath = string.Empty;
        string Random_Filename = null; 
        string PdfName = "Pdf File";
        BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
        int Y_Pos;
        BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);

        private void rbLIHWAP_Click(object sender, EventArgs e)
        {
            if(rbLIHWAP.Checked)
            {
                pnlHSource.Visible = false;
                lblHeatSource.Visible = false;
            }
            else
            {
                pnlHSource.Visible = true;
                lblHeatSource.Visible = true;
            }
        }

        private void On_SaveFormClosed1(object sender, FormClosedEventArgs e)
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
                iTextSharp.text.Font HeaderTblFontBold = new iTextSharp.text.Font(bf_times, 8, 1);
                iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 8, 2);
                iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 9, 4);
                cb = writer.DirectContent;

                if (Agency == "**") Agency = null; if (Depart == "**") Depart = null; if (Program == "**") Program = null;
                string Year = string.Empty; string AppNo = string.Empty; string HeatSource = string.Empty; string Seq = string.Empty;
                if (CmbYear.Visible == true)
                    Year = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString().Trim();

                if (rbDeliverables.Checked) HeatSource = "B1"; else if (rbUtilities.Checked) HeatSource = "U1";

                List<LIHPMQuesEntity> lihpmQues = _model.ZipCodeAndAgency.GetLIHPMQuesData(string.Empty, Year);

                var LihmpquesDesc = lihpmQues.Select(u => u.LPMQ_DESC.Trim()).Distinct().ToList();

                List<LPMQEntity> LPMList = _model.CaseMstData.GetLPMQ0001(Agency, Depart, Program, Year, HeatSource);
                
                List<CommonEntity> lihpResp = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTabs.LIHPMQUES, Agency, Depart, Program, "View");
                try
                {
                    PrintHeaderPage(document, writer);
                    document.NewPage();

                    if (lihpmQues.Count > 0)
                    {
                        PdfPTable table = new PdfPTable(7);
                        table.TotalWidth = 500f;
                        table.WidthPercentage = 100;
                        table.LockedWidth = true;
                        float[] widths = new float[] { 8f, 20f, 45f, 13f, 13f, 20f,15f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                        table.SetWidths(widths);
                        table.HorizontalAlignment = Element.ALIGN_CENTER;
                        //table.HeaderRows = 1;

                        string[] HeaderSeq4 = { "S.No", "Question", "Yes", "No", "Not Applicable","Total" };
                        for (int i = 0; i < HeaderSeq4.Length; ++i)
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(HeaderSeq4[i], TblFontBold));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            if (i == 1) cell.Colspan = 2;
                            cell.Border = iTextSharp.text.Rectangle.BOX;
                            table.AddCell(cell);
                        }
                        int j = 1; //string prevDesc = string.Empty; string PrevCode = string.Empty;
                        int GYes = 0, GNo = 0, GNA = 0,GT=0;
                        //foreach (var item in LihmpquesDesc)
                        //{
                        //    List<LIHPMQuesEntity> sellihpmQues = lihpmQues.FindAll(u => u.LPMQ_DESC.Trim().Equals(item));
                            string QType = string.Empty;
                        //    if (sellihpmQues.Count == 1) QType = sellihpmQues[0].LPMQ_QTYPE;

                            if (rbHeatBoth.Checked && (Year == "2016" || Year == "2017" || Year == "2018")) lihpmQues = lihpmQues.OrderBy(u => u.LPMQ_QTYPE.ToString()).ThenBy(u => u.LPMQ_CODE.ToString()).ToList();

                        bool Isfirst = true;
                        LIHPMQuesEntity Entity = new LIHPMQuesEntity();//sellihpmQues[0]; 
                        foreach (LIHPMQuesEntity Selentity in lihpmQues)
                        {
                            
                            if (Year == "2015")
                            {
                                Selentity.LPMQ_QTYPE = string.Empty;
                                if (rbDeliverables.Checked) { if (int.Parse(Selentity.LPMQ_CODE) > 6) Entity = null; else Entity = Selentity; }
                                else if (rbUtilities.Checked) { if (Selentity.LPMQ_CODE == "0005" || Selentity.LPMQ_CODE == "0006") Entity = null; else Entity = Selentity; }
                                else if (rbHeatBoth.Checked) Entity = Selentity;
                            }
                            else if(!string.IsNullOrEmpty(Year.Trim()))//else if (Year == "2016" || Year == "2017" || Year == "2018") commented by sudheer on 01/10/2020
                            {
                                if (rbDeliverables.Checked) { if (Selentity.LPMQ_QTYPE != "N") Entity = Selentity; else Entity = null; }
                                else if (rbUtilities.Checked) { if (Selentity.LPMQ_QTYPE != "Y") Entity = Selentity; else Entity = null; }
                                else if (rbHeatBoth.Checked) Entity = Selentity;
                            }

                            if (!Isfirst)
                            {
                                if (QType != Entity.LPMQ_QTYPE)
                                {
                                    if (table.Rows.Count > 0)
                                    {
                                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        P9.Border = iTextSharp.text.Rectangle.BOX;
                                        table.AddCell(P9);

                                        PdfPCell P11 = new PdfPCell(new Phrase("Grand Total", TableFont));
                                        P11.HorizontalAlignment = Element.ALIGN_CENTER;
                                        P11.Colspan = 2;
                                        P11.Border = iTextSharp.text.Rectangle.BOX;
                                        table.AddCell(P11);

                                        PdfPCell P12 = new PdfPCell(new Phrase(GYes.ToString(), TableFont));
                                        P12.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        P12.Border = iTextSharp.text.Rectangle.BOX;
                                        table.AddCell(P12);

                                        PdfPCell P13 = new PdfPCell(new Phrase(GNo.ToString(), TableFont));
                                        P13.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        P13.Border = iTextSharp.text.Rectangle.BOX;
                                        table.AddCell(P13);

                                        PdfPCell P14 = new PdfPCell(new Phrase(GNA.ToString(), TableFont));
                                        P14.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        P14.Border = iTextSharp.text.Rectangle.BOX;
                                        table.AddCell(P14);

                                        PdfPCell P15 = new PdfPCell(new Phrase(GT.ToString(), TableFont));
                                        P15.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        P15.Border = iTextSharp.text.Rectangle.BOX;
                                        table.AddCell(P15);

                                        document.Add(table);

                                        table.DeleteBodyRows();


                                        PdfPCell P1 = new PdfPCell(new Phrase("", TblFontBold));
                                        P1.HorizontalAlignment = Element.ALIGN_CENTER;
                                        P1.Colspan = 7;
                                        P1.FixedHeight = 25f;
                                        P1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(P1);

                                        string[] HeaderSeq = { "S.No", "Question", "Yes", "No", "Not Applicable", "Total" };
                                        for (int i = 0; i < HeaderSeq.Length; ++i)
                                        {
                                            PdfPCell cell = new PdfPCell(new Phrase(HeaderSeq[i], TblFontBold));
                                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                            if (i == 1) cell.Colspan = 2;
                                            cell.Border = iTextSharp.text.Rectangle.BOX;
                                            table.AddCell(cell);
                                        }

                                        GYes = 0; GNo = 0; GNA = 0; GT = 0; Isfirst = true;

                                    }
                                }
                            }

                            if ((!string.IsNullOrEmpty(Year.Trim())) && rbHeatBoth.Checked && Isfirst==true) //(Year == "2016" || Year == "2017" || Year == "2018")
                            {
                                string Desc=string.Empty;
                                if(Entity.LPMQ_QTYPE=="N")Desc="Utilities";else Desc="Deliverables";
                                PdfPCell P1 = new PdfPCell(new Phrase(Desc, TblFontBold));
                                P1.HorizontalAlignment = Element.ALIGN_CENTER;
                                P1.Colspan = 7;
                                P1.Border = iTextSharp.text.Rectangle.BOX;
                                table.AddCell(P1);
                                QType = Entity.LPMQ_QTYPE; Isfirst = false;
                            }

                            if (Entity != null)
                            {
                                //foreach (LIHPMQuesEntity Entity in sellihpmQues)
                                //{
                                //if (rbHeatBoth.Checked == false)
                                //{
                                    PdfPCell P1 = new PdfPCell(new Phrase(int.Parse(Entity.LPMQ_CODE).ToString(), TableFont));
                                    P1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    P1.Border = iTextSharp.text.Rectangle.BOX;
                                    table.AddCell(P1);
                                //}
                                //else
                                //{
                                //    PdfPCell P1 = new PdfPCell(new Phrase(j.ToString(), TableFont));
                                //    P1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //    P1.Border = iTextSharp.text.Rectangle.BOX;
                                //    table.AddCell(P1);
                                //}


                                PdfPCell P2 = new PdfPCell(new Phrase(Entity.LPMQ_DESC.Trim(), TableFont));
                                P2.HorizontalAlignment = Element.ALIGN_LEFT;
                                P2.Colspan = 2;
                                P2.Border = iTextSharp.text.Rectangle.BOX;
                                table.AddCell(P2);

                                List<LPMQEntity> YesCount = new List<LPMQEntity>();
                                List<LPMQEntity> NoCount = new List<LPMQEntity>(); List<LPMQEntity> NACount = new List<LPMQEntity>();
                                List<LPMQEntity> SelList = new List<LPMQEntity>();
                                //if (Year == "2016")
                                //{
                                    switch (Entity.LPMQ_CODE)
                                    {
                                        case "0001": if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0001.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0001.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0001.Equals("Y"));
                                            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0001.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0001.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0001.Equals("N"));
                                            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0001.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0001.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0001.Equals("U"));
                                            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0001 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0001 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0001 != ""); break;
                                        case "0002": if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0002.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0002.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0002.Equals("Y"));
                                            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0002.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0002.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0002.Equals("N"));
                                            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0002.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0002.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0002.Equals("U"));
                                            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0002 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0002 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0002 != ""); break;
                                        case "0003": if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0003.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0003.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0003.Equals("Y"));
                                            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0003.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0003.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0003.Equals("N"));
                                            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0003.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0003.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0003.Equals("U"));
                                            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0003 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0003 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0003 != ""); break;
                                        case "0004": if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0004.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0004.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0004.Equals("Y"));
                                            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0004.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0004.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0004.Equals("N"));
                                            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0004.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0004.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0004.Equals("U"));
                                            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0004 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0004 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0004 != ""); break;
                                        case "0005": if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0005.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0005.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0005.Equals("Y"));
                                            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0005.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0005.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0005.Equals("N"));
                                            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0005.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0005.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0005.Equals("U"));
                                            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0005 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0005 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0005 != ""); break;
                                        case "0006": if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0006.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0006.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0006.Equals("Y"));
                                            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0006.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0006.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0006.Equals("N"));
                                            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0006.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0006.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0006.Equals("U"));
                                            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0006 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0006 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0006 != ""); break;
                                        case "0007": if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0007.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0007.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0007.Equals("Y"));
                                            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0007.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0007.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0007.Equals("N"));
                                            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0007.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0007.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0007.Equals("U"));
                                            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0007 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0007 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0007 != ""); break;
                                        case "0008": if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0008.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0008.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0008.Equals("Y"));
                                            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0008.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0008.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0008.Equals("N"));
                                            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0008.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0008.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0008.Equals("U"));
                                            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0008 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0008 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0008 != ""); break;
                                        case "0009": if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0009.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0009.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0009.Equals("Y"));
                                            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0009.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0009.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0009.Equals("N"));
                                            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0009.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0009.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0009.Equals("U"));
                                            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0009 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0009 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0009 != ""); break;
                                        case "0010": if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0010.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0010.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0010.Equals("Y"));
                                            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0010.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0010.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0010.Equals("N"));
                                            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0010.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0010.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0010.Equals("U"));
                                            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0010 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0010 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0010 != ""); break;
                                        case "0011": if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0011.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0011.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0011.Equals("Y"));
                                            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0011.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0011.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0011.Equals("N"));
                                            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0011.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0011.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0011.Equals("U"));
                                            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0011 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0011 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0011 != ""); break;
                                    case "0012":
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0012.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0012.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0012.Equals("Y"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0012.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0012.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0012.Equals("N"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0012.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0012.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0012.Equals("U"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0012 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0012 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0012 != ""); break;
                                    case "0013":
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0013.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0013.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0013.Equals("Y"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0013.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0013.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0013.Equals("N"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0013.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0013.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0013.Equals("U"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0013 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0013 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0013 != ""); break;
                                    case "0014":
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0014.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0014.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0014.Equals("Y"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0014.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0014.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0014.Equals("N"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0014.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0014.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0014.Equals("U"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0014 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0014 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0014 != ""); break;
                                    case "0015":
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0015.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0015.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0015.Equals("Y"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0015.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0015.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0015.Equals("N"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0015.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0015.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0015.Equals("U"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0015 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0015 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0015 != ""); break;
                                    case "0016":
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0016.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0016.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0016.Equals("Y"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0016.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0016.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0016.Equals("N"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0016.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0016.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0016.Equals("U"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0016 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0016 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0016 != ""); break;
                                    case "0017":
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0017.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0017.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0017.Equals("Y"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0017.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0017.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0017.Equals("N"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0017.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0017.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0017.Equals("U"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0017 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0017 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0017 != ""); break;
                                }
                                //}
                                //else if (Year == "2015")
                                //{
                                //    switch (Entity.LPMQ_CODE)
                                //    {
                                //        case "0001": if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0001.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0001.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0001.Equals("Y"));
                                //            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0001.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0001.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0001.Equals("N"));
                                //            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0001.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0001.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0001.Equals("U"));
                                //            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0001 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0001 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0001 != ""); break;
                                //        case "0002": if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0002.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0002.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0002.Equals("Y"));
                                //            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0002.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0002.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0002.Equals("N"));
                                //            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0002.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0002.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0002.Equals("U"));
                                //            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0002 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0002 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0002 != ""); break;
                                //        case "0003": if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0003.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0003.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0003.Equals("Y"));
                                //            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0003.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0003.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0003.Equals("N"));
                                //            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0003.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0003.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0003.Equals("U"));
                                //            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0003 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0003 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0003 != ""); break;
                                //        case "0004": if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0004.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0004.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0004.Equals("Y"));
                                //            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0004.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0004.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0004.Equals("N"));
                                //            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0004.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0004.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0004.Equals("U"));
                                //            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0004 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0004 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0004 != ""); break;
                                //        case "0005": if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0005.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0005.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0005.Equals("Y"));
                                //            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0005.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0005.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0005.Equals("N"));
                                //            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0005.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0005.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0005.Equals("U"));
                                //            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0005 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0005 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0005 != ""); break;
                                //        case "0006": if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0006.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0006.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0006.Equals("Y"));
                                //            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0006.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0006.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0006.Equals("N"));
                                //            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0006.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0006.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0006.Equals("U"));
                                //            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0006 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0006 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0006 != ""); break;
                                //        case "0007": if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0007.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0007.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0007.Equals("Y"));
                                //            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0007.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0007.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0007.Equals("N"));
                                //            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0007.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0007.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0007.Equals("U"));
                                //            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0007 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0007 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0007 != ""); break;
                                //        case "0008": if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0008.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0008.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0008.Equals("Y"));
                                //            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0008.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0008.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0008.Equals("N"));
                                //            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0008.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0008.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0008.Equals("U"));
                                //            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0008 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0008 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0008 != ""); break;
                                //        case "0009": if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0009.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0009.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0009.Equals("Y"));
                                //            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0009.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0009.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0009.Equals("N"));
                                //            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0009.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0009.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0009.Equals("U"));
                                //            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0009 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0009 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0009 != ""); break;
                                //        case "0010": if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0010.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0010.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0010.Equals("Y"));
                                //            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0010.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0010.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0010.Equals("N"));
                                //            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0010.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0010.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0010.Equals("U"));
                                //            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0010 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0010 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0010 != ""); break;
                                //        case "0011": if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0011.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0011.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0011.Equals("Y"));
                                //            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0011.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0011.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0011.Equals("N"));
                                //            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0011.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0011.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0011.Equals("U"));
                                //            if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0011 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0011 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0011 != ""); break;
                                //    }
                                //}



                                PdfPCell P3 = new PdfPCell(new Phrase(YesCount.Count.ToString(), TableFont));
                                P3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                P3.Border = iTextSharp.text.Rectangle.BOX;
                                table.AddCell(P3);

                                PdfPCell P4 = new PdfPCell(new Phrase(NoCount.Count.ToString(), TableFont));
                                P4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                P4.Border = iTextSharp.text.Rectangle.BOX;
                                table.AddCell(P4);

                                PdfPCell P5 = new PdfPCell(new Phrase(NACount.Count.ToString(), TableFont));
                                P5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                P5.Border = iTextSharp.text.Rectangle.BOX;
                                table.AddCell(P5);

                                PdfPCell P10 = new PdfPCell(new Phrase((YesCount.Count + NoCount.Count + NACount.Count).ToString(), TableFont));
                                P10.HorizontalAlignment = Element.ALIGN_RIGHT;
                                P10.Border = iTextSharp.text.Rectangle.BOX;
                                table.AddCell(P10);

                                GYes += YesCount.Count; GNo += NoCount.Count; GNA += NACount.Count; GT += YesCount.Count + NoCount.Count + NACount.Count;

                                if (rbDetail.Checked)
                                {
                                    if (SelList.Count > 0)
                                    {
                                        foreach (LPMQEntity LEntity in SelList)
                                        {
                                            PdfPCell P6 = new PdfPCell(new Phrase("", TableFont));
                                            P6.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            P6.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                            table.AddCell(P6);

                                            PdfPCell P7 = new PdfPCell(new Phrase(LEntity.ApplNo.Trim(), TableFont));
                                            P7.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            P7.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(P7);

                                            PdfPCell P8 = new PdfPCell(new Phrase(LookupDataAccess.GetMemberName(LEntity.NameixFi, LEntity.NameixMi, LEntity.NameixLast, BaseForm.BaseHierarchyCnFormat.ToString()), TableFont));
                                            P8.HorizontalAlignment = Element.ALIGN_LEFT;
                                            P8.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table.AddCell(P8);

                                            switch (Entity.LPMQ_CODE)
                                            {
                                                case "0001": if (LEntity.LPM_0001 == ("Y"))
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0001.Trim(), TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    if (LEntity.LPM_0001 == ("N"))
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0001.Trim(), TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    if (LEntity.LPM_0001 == ("U"))
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0001.Trim(), TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    PdfPCell P20 = new PdfPCell(new Phrase("", TableFont));
                                                    P20.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                    P20.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                                    table.AddCell(P20);
                                                    break;
                                                case "0002": if (LEntity.LPM_0002 == ("Y"))
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0002.Trim(), TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    if (LEntity.LPM_0002 == ("N"))
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0002.Trim(), TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    if (LEntity.LPM_0002 == ("U"))
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0002.Trim(), TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    PdfPCell P21 = new PdfPCell(new Phrase("", TableFont));
                                                    P21.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                    P21.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                                    table.AddCell(P21);
                                                    break;
                                                case "0003": if (LEntity.LPM_0003 == ("Y"))
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0003.Trim(), TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    if (LEntity.LPM_0003 == ("N"))
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0003.Trim(), TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    if (LEntity.LPM_0003 == ("U"))
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0003.Trim(), TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    PdfPCell P22 = new PdfPCell(new Phrase("", TableFont));
                                                    P22.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                    P22.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                                    table.AddCell(P22);
                                                    break;
                                                case "0004": if (LEntity.LPM_0004 == ("Y"))
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0004.Trim(), TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    if (LEntity.LPM_0004 == ("N"))
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0004.Trim(), TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    if (LEntity.LPM_0004 == ("U"))
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0004.Trim(), TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    PdfPCell P23 = new PdfPCell(new Phrase("", TableFont));
                                                    P23.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                    P23.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                                    table.AddCell(P23);
                                                    break;
                                                case "0005": if (LEntity.LPM_0005 == ("Y"))
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0005.Trim(), TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    if (LEntity.LPM_0005 == ("N"))
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0005.Trim(), TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    if (LEntity.LPM_0005 == ("U"))
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0005.Trim(), TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    PdfPCell P24 = new PdfPCell(new Phrase("", TableFont));
                                                    P24.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                    P24.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                                    table.AddCell(P24);
                                                    break;
                                                case "0006": if (LEntity.LPM_0006 == ("Y"))
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0006.Trim(), TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    if (LEntity.LPM_0006 == ("N"))
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0006.Trim(), TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    if (LEntity.LPM_0006 == ("U"))
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0006.Trim(), TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    PdfPCell P25 = new PdfPCell(new Phrase("", TableFont));
                                                    P25.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                    P25.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                                    table.AddCell(P25);
                                                    break;
                                                case "0007": if (LEntity.LPM_0007 == ("Y"))
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0007.Trim(), TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    if (LEntity.LPM_0007 == ("N"))
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0007.Trim(), TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    if (LEntity.LPM_0007 == ("U"))
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0007.Trim(), TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    PdfPCell P26 = new PdfPCell(new Phrase("", TableFont));
                                                    P26.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                    P26.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                                    table.AddCell(P26);
                                                    break;
                                                case "0008": if (LEntity.LPM_0008 == ("Y"))
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0008.Trim(), TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    if (LEntity.LPM_0008 == ("N"))
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0008.Trim(), TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    if (LEntity.LPM_0008 == ("U"))
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0008.Trim(), TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    PdfPCell P27 = new PdfPCell(new Phrase("", TableFont));
                                                    P27.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                    P27.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                                    table.AddCell(P27);
                                                    break;
                                                case "0009": if (LEntity.LPM_0009 == ("Y"))
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0009.Trim(), TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    if (LEntity.LPM_0009 == ("N"))
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0009.Trim(), TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    if (LEntity.LPM_0009 == ("U"))
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0009.Trim(), TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    PdfPCell P28 = new PdfPCell(new Phrase("", TableFont));
                                                    P28.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                    P28.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                                    table.AddCell(P28);
                                                    break;
                                                case "0010": if (LEntity.LPM_0010 == ("Y"))
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0010.Trim(), TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    if (LEntity.LPM_0010 == ("N"))
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0010.Trim(), TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    if (LEntity.LPM_0010 == ("U"))
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0010.Trim(), TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    PdfPCell P29 = new PdfPCell(new Phrase("", TableFont));
                                                    P29.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                    P29.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                                    table.AddCell(P29);
                                                    break;
                                                case "0011": if (LEntity.LPM_0011 == ("Y"))
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0011.Trim(), TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    if (LEntity.LPM_0011 == ("N"))
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0011.Trim(), TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    if (LEntity.LPM_0011 == ("U"))
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0011.Trim(), TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    PdfPCell P30 = new PdfPCell(new Phrase("", TableFont));
                                                    P30.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                    P30.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                                    table.AddCell(P30);
                                                    break;
                                                case "0012":
                                                    if (LEntity.LPM_0012 == ("Y"))
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0012.Trim(), TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    if (LEntity.LPM_0012 == ("N"))
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0012.Trim(), TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    if (LEntity.LPM_0012 == ("U"))
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0012.Trim(), TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    PdfPCell P31 = new PdfPCell(new Phrase("", TableFont));
                                                    P31.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                    P31.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                                    table.AddCell(P31);
                                                    break;
                                                case "0013":
                                                    if (LEntity.LPM_0013 == ("Y"))
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0013.Trim(), TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    if (LEntity.LPM_0013 == ("N"))
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0013.Trim(), TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    if (LEntity.LPM_0013 == ("U"))
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0013.Trim(), TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    PdfPCell P32 = new PdfPCell(new Phrase("", TableFont));
                                                    P32.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                    P32.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                                    table.AddCell(P32);
                                                    break;
                                                case "0014":
                                                    if (LEntity.LPM_0014 == ("Y"))
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0014.Trim(), TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    if (LEntity.LPM_0014 == ("N"))
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0014.Trim(), TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    if (LEntity.LPM_0014 == ("U"))
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0014.Trim(), TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    PdfPCell P33 = new PdfPCell(new Phrase("", TableFont));
                                                    P33.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                    P33.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                                    table.AddCell(P33);
                                                    break;
                                                case "0015":
                                                    if (LEntity.LPM_0015 == ("Y"))
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0015.Trim(), TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    if (LEntity.LPM_0015 == ("N"))
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0015.Trim(), TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    if (LEntity.LPM_0015 == ("U"))
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0015.Trim(), TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    PdfPCell P34 = new PdfPCell(new Phrase("", TableFont));
                                                    P34.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                    P34.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                                    table.AddCell(P34);
                                                    break;
                                                case "0016":
                                                    if (LEntity.LPM_0016 == ("Y"))
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0016.Trim(), TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    if (LEntity.LPM_0016 == ("N"))
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0016.Trim(), TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    if (LEntity.LPM_0016 == ("U"))
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0016.Trim(), TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    PdfPCell P35 = new PdfPCell(new Phrase("", TableFont));
                                                    P35.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                    P35.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                                    table.AddCell(P35);
                                                    break;
                                                case "0017":
                                                    if (LEntity.LPM_0017 == ("Y"))
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0017.Trim(), TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    if (LEntity.LPM_0017 == ("N"))
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0017.Trim(), TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    if (LEntity.LPM_0017 == ("U"))
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0017.Trim(), TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(P9);
                                                    }
                                                    PdfPCell P36 = new PdfPCell(new Phrase("", TableFont));
                                                    P36.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                    P36.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                                    table.AddCell(P36);
                                                    break;
                                            }


                                        }
                                    }
                                }

                                j++;
                            }

                        }
                            

                        //}

                        if (table.Rows.Count > 0)
                        {
                            PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                            P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                            P9.Border = iTextSharp.text.Rectangle.BOX;
                            table.AddCell(P9);

                            PdfPCell P11 = new PdfPCell(new Phrase("Grand Total", TableFont));
                            P11.HorizontalAlignment = Element.ALIGN_CENTER;
                            P11.Colspan = 2;
                            P11.Border = iTextSharp.text.Rectangle.BOX;
                            table.AddCell(P11);

                            PdfPCell P12 = new PdfPCell(new Phrase(GYes.ToString(), TableFont));
                            P12.HorizontalAlignment = Element.ALIGN_RIGHT;
                            P12.Border = iTextSharp.text.Rectangle.BOX;
                            table.AddCell(P12);

                            PdfPCell P13 = new PdfPCell(new Phrase(GNo.ToString(), TableFont));
                            P13.HorizontalAlignment = Element.ALIGN_RIGHT;
                            P13.Border = iTextSharp.text.Rectangle.BOX;
                            table.AddCell(P13);

                            PdfPCell P14 = new PdfPCell(new Phrase(GNA.ToString(), TableFont));
                            P14.HorizontalAlignment = Element.ALIGN_RIGHT;
                            P14.Border = iTextSharp.text.Rectangle.BOX;
                            table.AddCell(P14);

                            PdfPCell P15 = new PdfPCell(new Phrase(GT.ToString(), TableFont));
                            P15.HorizontalAlignment = Element.ALIGN_RIGHT;
                            P15.Border = iTextSharp.text.Rectangle.BOX;
                            table.AddCell(P15);

                            document.Add(table);
                        }

                        #region Without Distinct Description
                        //foreach (LIHPMQuesEntity Entity in lihpmQues)
                        //{
                        //        PdfPCell P1 = new PdfPCell(new Phrase(j.ToString(), TableFont));
                        //        P1.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //        P1.Border = iTextSharp.text.Rectangle.BOX;
                        //        table.AddCell(P1);


                        //        PdfPCell P2 = new PdfPCell(new Phrase(Entity.LPMQ_DESC.Trim(), TableFont));
                        //        P2.HorizontalAlignment = Element.ALIGN_LEFT;
                        //        P2.Colspan = 2;
                        //        P2.Border = iTextSharp.text.Rectangle.BOX;
                        //        table.AddCell(P2);

                        //        List<LPMQEntity> YesCount = new List<LPMQEntity>();
                        //        List<LPMQEntity> NoCount = new List<LPMQEntity>(); List<LPMQEntity> NACount = new List<LPMQEntity>();
                        //        List<LPMQEntity> SelList = new List<LPMQEntity>();
                        //        switch (Entity.LPMQ_CODE)
                        //        {
                        //            case "0001":  YesCount = LPMList.FindAll(u => u.LPM_0001.Equals("Y"));
                        //                NoCount = LPMList.FindAll(u => u.LPM_0001.Equals("N"));
                        //                NACount = LPMList.FindAll(u => u.LPM_0001.Equals("U"));
                        //                SelList = LPMList.FindAll(u => u.LPM_0001 != ""); break;
                        //            case "0002": YesCount = LPMList.FindAll(u => u.LPM_0002.Equals("Y"));
                        //                NoCount = LPMList.FindAll(u => u.LPM_0002.Equals("N"));
                        //                NACount = LPMList.FindAll(u => u.LPM_0002.Equals("U"));
                        //                SelList = LPMList.FindAll(u => u.LPM_0002 != ""); break;
                        //            case "0003": YesCount = LPMList.FindAll(u => u.LPM_0003.Equals("Y"));
                        //                NoCount = LPMList.FindAll(u => u.LPM_0003.Equals("N"));
                        //                NACount = LPMList.FindAll(u => u.LPM_0003.Equals("U"));
                        //                SelList = LPMList.FindAll(u => u.LPM_0003 != ""); break;
                        //            case "0004": YesCount = LPMList.FindAll(u => u.LPM_0004.Equals("Y"));
                        //                NoCount = LPMList.FindAll(u => u.LPM_0004.Equals("N"));
                        //                NACount = LPMList.FindAll(u => u.LPM_0004.Equals("U"));
                        //                SelList = LPMList.FindAll(u => u.LPM_0004 != ""); break;
                        //            case "0005": YesCount = LPMList.FindAll(u => u.LPM_0005.Equals("Y"));
                        //                NoCount = LPMList.FindAll(u => u.LPM_0005.Equals("N"));
                        //                NACount = LPMList.FindAll(u => u.LPM_0005.Equals("U"));
                        //                SelList = LPMList.FindAll(u => u.LPM_0005 != ""); break;
                        //            case "0006": YesCount = LPMList.FindAll(u => u.LPM_0006.Equals("Y"));
                        //                NoCount = LPMList.FindAll(u => u.LPM_0006.Equals("N"));
                        //                NACount = LPMList.FindAll(u => u.LPM_0006.Equals("U"));
                        //                SelList = LPMList.FindAll(u => u.LPM_0006 != ""); break;
                        //            case "0007": YesCount = LPMList.FindAll(u => u.LPM_0007.Equals("Y"));
                        //                NoCount = LPMList.FindAll(u => u.LPM_0007.Equals("N"));
                        //                NACount = LPMList.FindAll(u => u.LPM_0007.Equals("U"));
                        //                SelList = LPMList.FindAll(u => u.LPM_0007 != ""); break;
                        //            case "0008": YesCount = LPMList.FindAll(u => u.LPM_0008.Equals("Y"));
                        //                NoCount = LPMList.FindAll(u => u.LPM_0008.Equals("N"));
                        //                NACount = LPMList.FindAll(u => u.LPM_0008.Equals("U"));
                        //                SelList = LPMList.FindAll(u => u.LPM_0008 != ""); break;
                        //            case "0009": YesCount = LPMList.FindAll(u => u.LPM_0009.Equals("Y"));
                        //                NoCount = LPMList.FindAll(u => u.LPM_0009.Equals("N"));
                        //                NACount = LPMList.FindAll(u => u.LPM_0009.Equals("U"));
                        //                SelList = LPMList.FindAll(u => u.LPM_0009 != ""); break;
                        //            case "0010": YesCount = LPMList.FindAll(u => u.LPM_0010.Equals("Y"));
                        //                NoCount = LPMList.FindAll(u => u.LPM_0010.Equals("N"));
                        //                NACount = LPMList.FindAll(u => u.LPM_0010.Equals("U"));
                        //                SelList = LPMList.FindAll(u => u.LPM_0010 != ""); break;
                        //            case "0011": YesCount = LPMList.FindAll(u => u.LPM_0011.Equals("Y"));
                        //                NoCount = LPMList.FindAll(u => u.LPM_0011.Equals("N"));
                        //                NACount = LPMList.FindAll(u => u.LPM_0011.Equals("U"));
                        //                SelList = LPMList.FindAll(u => u.LPM_0011 != ""); break;
                        //        }


                        //        if (rbSummary.Checked || rbBoth.Checked)
                        //        {
                        //            PdfPCell P3 = new PdfPCell(new Phrase(YesCount.Count.ToString(), TableFont));
                        //            P3.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //            P3.Border = iTextSharp.text.Rectangle.BOX;
                        //            table.AddCell(P3);

                        //            PdfPCell P4 = new PdfPCell(new Phrase(NoCount.Count.ToString(), TableFont));
                        //            P4.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //            P4.Border = iTextSharp.text.Rectangle.BOX;
                        //            table.AddCell(P4);

                        //            PdfPCell P5 = new PdfPCell(new Phrase(NACount.Count.ToString(), TableFont));
                        //            P5.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //            P5.Border = iTextSharp.text.Rectangle.BOX;
                        //            table.AddCell(P5);
                        //        }
                        //        else
                        //        {
                        //            PdfPCell P3 = new PdfPCell(new Phrase("", TableFont));
                        //            P3.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //            P3.Border = iTextSharp.text.Rectangle.BOX;
                        //            table.AddCell(P3);

                        //            PdfPCell P4 = new PdfPCell(new Phrase("", TableFont));
                        //            P4.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //            P4.Border = iTextSharp.text.Rectangle.BOX;
                        //            table.AddCell(P4);

                        //            PdfPCell P5 = new PdfPCell(new Phrase("", TableFont));
                        //            P5.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //            P5.Border = iTextSharp.text.Rectangle.BOX;
                        //            table.AddCell(P5);
                        //        }

                        //        if (rbDetail.Checked || rbBoth.Checked)
                        //        {
                        //            if (SelList.Count > 0)
                        //            {
                        //                foreach (LPMQEntity LEntity in SelList)
                        //                {
                        //                    PdfPCell P6 = new PdfPCell(new Phrase("", TableFont));
                        //                    P6.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                    P6.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                    table.AddCell(P6);

                        //                    PdfPCell P7 = new PdfPCell(new Phrase(LEntity.ApplNo.Trim(), TableFont));
                        //                    P7.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                    P7.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                    table.AddCell(P7);

                        //                    PdfPCell P8 = new PdfPCell(new Phrase(LookupDataAccess.GetMemberName(LEntity.NameixFi, LEntity.NameixMi, LEntity.NameixLast, BaseForm.BaseHierarchyCnFormat.ToString()), TableFont));
                        //                    P8.HorizontalAlignment = Element.ALIGN_LEFT;
                        //                    P8.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                    table.AddCell(P8);

                        //                    switch (Entity.LPMQ_CODE)
                        //                    {
                        //                        case "0001": if (LEntity.LPM_0001 == ("Y"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0001.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            if (LEntity.LPM_0001 == ("N"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0001.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            if (LEntity.LPM_0001 == ("U"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0001.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            } break;
                        //                        case "0002": if (LEntity.LPM_0002 == ("Y"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0002.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            if (LEntity.LPM_0002 == ("N"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0002.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            if (LEntity.LPM_0002 == ("U"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0002.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            } break;
                        //                        case "0003": if (LEntity.LPM_0003 == ("Y"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0003.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            if (LEntity.LPM_0003 == ("N"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0003.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            if (LEntity.LPM_0003 == ("U"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0003.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            } break;
                        //                        case "0004": if (LEntity.LPM_0004 == ("Y"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0004.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            if (LEntity.LPM_0004 == ("N"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0004.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            if (LEntity.LPM_0004 == ("U"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0004.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            } break;
                        //                        case "0005": if (LEntity.LPM_0005 == ("Y"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0005.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            if (LEntity.LPM_0005 == ("N"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0005.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            if (LEntity.LPM_0005 == ("U"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0005.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            } break;
                        //                        case "0006": if (LEntity.LPM_0006 == ("Y"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0006.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            if (LEntity.LPM_0006 == ("N"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0006.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            if (LEntity.LPM_0006 == ("U"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0006.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            } break;
                        //                        case "0007": if (LEntity.LPM_0007 == ("Y"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0007.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            if (LEntity.LPM_0007 == ("N"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0007.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            if (LEntity.LPM_0007 == ("U"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0007.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            } break;
                        //                        case "0008": if (LEntity.LPM_0008 == ("Y"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0008.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            if (LEntity.LPM_0008 == ("N"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0008.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            if (LEntity.LPM_0008 == ("U"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0008.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            } break;
                        //                        case "0009": if (LEntity.LPM_0009 == ("Y"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0009.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            if (LEntity.LPM_0009 == ("N"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0009.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            if (LEntity.LPM_0009 == ("U"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0009.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            } break;
                        //                        case "0010": if (LEntity.LPM_0010 == ("Y"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0010.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            if (LEntity.LPM_0010 == ("N"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0010.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            if (LEntity.LPM_0010 == ("U"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0010.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            } break;
                        //                        case "0011": if (LEntity.LPM_0011 == ("Y"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0011.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            if (LEntity.LPM_0011 == ("N"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0011.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            if (LEntity.LPM_0011 == ("U"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0011.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            } break;
                        //                    }


                        //                }
                        //            }
                        //        }

                        //        prevDesc = Entity.LPMQ_DESC.Trim(); PrevCode = Entity.LPMQ_CODE.Trim();
                        //        j++;
                            
                        //}*/

#endregion

                        if (chkbExcel.Checked)
                        {

                            OnExcel_Report1(lihpmQues, LPMList, lihpResp, PdfName, Year);
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
                iTextSharp.text.Font HeaderTblFontBold = new iTextSharp.text.Font(bf_times, 8, 1);
                iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 8, 2);
                iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 9, 4);
                cb = writer.DirectContent;

                if (Agency == "**") Agency = null; if (Depart == "**") Depart = null; if (Program == "**") Program = null;
                string Year = string.Empty; string AppNo = string.Empty; string HeatSource = string.Empty; string Seq = string.Empty;
                if (CmbYear.Visible == true)
                    Year = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString().Trim();

                if (rbDeliverables.Checked) HeatSource = "B1"; else if (rbUtilities.Checked) HeatSource = "U1";

                List<LIHPMQuesEntity> lihpmQues = _model.ZipCodeAndAgency.GetLIHPMQuesData(string.Empty, Year);

                List<CommonEntity> PrimarySourceHeat = _model.lookupDataAccess.GetAgyTabRecordsByCode(Consts.AgyTab.HEATSOURCE);

                var LihmpquesDesc = lihpmQues.Select(u => u.LPMQ_DESC.Trim()).Distinct().ToList();

                List<LPMQEntity> LPMList = _model.CaseMstData.GetLPMQ0001(Agency, Depart, Program, Year, HeatSource);

                List<CommonEntity> lihpResp = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTabs.LIHPMQUES, Agency, Depart, Program, "View");
                try
                {
                    PrintHeaderPage(document, writer);
                    document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                    document.NewPage();
                    string LQType = string.Empty;
                    PdfPTable table;
                    if (lihpmQues.Count > 0)
                    {
                        
                        int j = 1; //string prevDesc = string.Empty; string PrevCode = string.Empty;
                        int GYes = 0, GNo = 0, GNA = 0, GT = 0;
                        //foreach (var item in LihmpquesDesc)
                        //{
                        //    List<LIHPMQuesEntity> sellihpmQues = lihpmQues.FindAll(u => u.LPMQ_DESC.Trim().Equals(item));
                        string QType = string.Empty;
                        //    if (sellihpmQues.Count == 1) QType = sellihpmQues[0].LPMQ_QTYPE;

                        if (!string.IsNullOrEmpty(Year.Trim())) //Year == "2016" || Year == "2017" || Year == "2018"  commented by sudheer on 01/10/2020
                        {
                            if (rbDeliverables.Checked) { lihpmQues = lihpmQues.FindAll(u => u.LPMQ_QTYPE == "Y"); PrimarySourceHeat = PrimarySourceHeat.FindAll(u => u.Code != "02" && u.Code != "04"); }
                            else if (rbUtilities.Checked) { lihpmQues = lihpmQues.FindAll(u => u.LPMQ_QTYPE == "N"); PrimarySourceHeat = PrimarySourceHeat.FindAll(u => u.Code.Equals("02") || u.Code.Equals("04")); PrimarySourceHeat = PrimarySourceHeat.OrderBy(u => u.Code).ToList(); }

                            //Commented by Sudheer on 08/14/24 based on the IHCDA document.
                            //if (rbDeliverables.Checked) { lihpmQues = lihpmQues.FindAll(u => u.LPMQ_QTYPE != "N"); PrimarySourceHeat = PrimarySourceHeat.FindAll(u => u.Code != "02" && u.Code != "04"); }
                            //else if (rbUtilities.Checked) { lihpmQues = lihpmQues.FindAll(u => u.LPMQ_QTYPE != "Y"); PrimarySourceHeat = PrimarySourceHeat.FindAll(u => u.Code.Equals("02") || u.Code.Equals("04")); PrimarySourceHeat = PrimarySourceHeat.OrderBy(u => u.Code).ToList(); }
                        }

                        if (rbHeatBoth.Checked && (!string.IsNullOrEmpty(Year.Trim()))) //Year == "2016" || Year == "2017" || Year == "2018" commented by sudheer on 01/10/2020
                            lihpmQues = lihpmQues.OrderBy(u => u.LPMQ_QTYPE.ToString()).ThenBy(u => u.LPMQ_CODE.ToString()).ToList();

                        List<LPMQEntity> SLPMList = LPMList.FindAll(u => u.Mst_Heating_Source != ""); //LPMList.OrderBy(u => u.Mst_Heating_Source.Trim()).ToList();
                        SLPMList = SLPMList.OrderBy(u => u.Mst_Heating_Source.Trim()).ToList();
                        var LPMS_Count = SLPMList.Select(u => u.Mst_Heating_Source.Trim()).Distinct().ToList();
                        
                        if (rbDeliverables.Checked)
                        {
                            float[] widths;
                            int ColumnsCount = 7 + LPMS_Count.Count;
                            table = new PdfPTable(ColumnsCount);
                            table.TotalWidth = 700f;
                            table.WidthPercentage = 100;
                            table.LockedWidth = true;
                            switch (LPMS_Count.Count)
                            {
                                case 1: widths = new float[] { 8f, 20f, 45f, 15f, 13f, 13f, 20f, 15f }; table.SetWidths(widths); break;
                                case 2: widths = new float[] { 8f, 20f, 45f, 15f, 15f, 13f, 13f, 20f, 15f }; table.SetWidths(widths); break;
                                case 3: widths = new float[] { 8f, 20f, 45f, 15f, 15f, 15f, 13f, 13f, 20f, 15f }; table.SetWidths(widths); break;
                                case 4: widths = new float[] { 8f, 20f, 45f, 15f, 15f, 15f, 15f, 13f, 13f, 20f, 15f }; table.SetWidths(widths); break;
                                case 5: widths = new float[] { 8f, 20f, 45f, 15f, 15f, 15f, 15f, 15f, 13f, 13f, 20f, 15f }; table.SetWidths(widths); break;
                                case 6: widths = new float[] { 8f, 20f, 45f, 15f, 15f, 15f, 15f, 15f, 15f, 13f, 13f, 20f, 15f }; table.SetWidths(widths); break;
                                case 7: widths = new float[] { 8f, 20f, 45f, 15f, 15f, 15f, 15f, 15f, 15f, 15f, 13f, 13f, 20f, 15f }; table.SetWidths(widths); break;
                                case 8: widths = new float[] { 8f, 20f, 45f, 15f, 15f, 15f, 15f, 15f, 15f, 15f, 15f, 13f, 13f, 20f, 15f }; table.SetWidths(widths); break;
                                case 9: widths = new float[] { 8f, 20f, 45f, 15f, 15f, 15f, 15f, 15f, 15f, 15f, 15f, 15f, 13f, 13f, 20f, 15f }; table.SetWidths(widths); break;
                                case 10: widths = new float[] { 8f, 20f, 45f, 15f, 15f, 15f, 15f, 15f, 15f, 15f, 15f, 15f, 15f, 13f, 13f, 20f, 15f }; table.SetWidths(widths); break;
                            }
                            //if(LPMS_Count.Count==5)
                            //    widths = new float[] { 8f, 20f, 45f, 15f, 15f, 15f, 15f, 15f, 13f, 13f, 20f, 15f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };

                            table.HorizontalAlignment = Element.ALIGN_CENTER;
                            //table.HeaderRows = 1;

                            string[] HeaderSeq4 = { "S.No", "Question" };
                            for (int i = 0; i < HeaderSeq4.Length; ++i)
                            {
                                PdfPCell cell = new PdfPCell(new Phrase(HeaderSeq4[i], TblFontBold));
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                if (i == 1) cell.Colspan = 2;
                                cell.Border = iTextSharp.text.Rectangle.BOX;
                                table.AddCell(cell);
                            }

                            if (LPMS_Count.Count > 0)
                            {
                                
                                foreach (var item in LPMS_Count)
                                {
                                    string Desc = PrimarySourceHeat.Find(u => u.Code.Equals(item)).Desc.Trim();

                                    PdfPCell cell = new PdfPCell(new Phrase(Desc, TblFontBold));
                                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                    //if (i == 1) cell.Colspan = 2;
                                    cell.Border = iTextSharp.text.Rectangle.BOX;
                                    table.AddCell(cell);
                                }
                            }

                            string[] HeaderSeq = { "Yes", "No", "Not Applicable", "Total" };
                            for (int i = 0; i < HeaderSeq.Length; ++i)
                            {
                                PdfPCell cell = new PdfPCell(new Phrase(HeaderSeq[i], TblFontBold));
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                //if (i == 1) cell.Colspan = 2;
                                cell.Border = iTextSharp.text.Rectangle.BOX;
                                table.AddCell(cell);
                            }
                        }
                        else
                        {
                            table = new PdfPTable(9);
                            table.TotalWidth = 700f;
                            table.WidthPercentage = 100;
                            table.LockedWidth = true;
                            float[] widths = new float[] { 8f, 20f, 45f, 20f, 20f, 13f, 13f, 20f, 15f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                            table.SetWidths(widths);
                            table.HorizontalAlignment = Element.ALIGN_CENTER;

                            string[] HeaderSeq = { "S.No", "Question", "Electric", "Natural Gas", "Yes", "No", "Not Applicable", "Total" };
                            for (int i = 0; i < HeaderSeq.Length; ++i)
                            {
                                PdfPCell cell = new PdfPCell(new Phrase(HeaderSeq[i], TblFontBold));
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                if (i == 1) cell.Colspan = 2;
                                cell.Border = iTextSharp.text.Rectangle.BOX;
                                table.AddCell(cell);
                            }
                        }


                        bool Isfirst = true; int[] SourceCount = new int[10];
                        LIHPMQuesEntity Entity = new LIHPMQuesEntity();//sellihpmQues[0]; 
                         List<CommonEntity> CCountEntity = new List<CommonEntity>();
                        foreach (LIHPMQuesEntity Selentity in lihpmQues)
                        {
                            LQType = Entity.LPMQ_QTYPE;
                            if (Year == "2015")
                            {
                                Selentity.LPMQ_QTYPE = string.Empty;
                                if (rbDeliverables.Checked) { if (int.Parse(Selentity.LPMQ_CODE) > 6) Entity = null; else Entity = Selentity; }
                                else if (rbUtilities.Checked) { if (Selentity.LPMQ_CODE == "0005" || Selentity.LPMQ_CODE == "0006") Entity = null; else Entity = Selentity; }
                                else if (rbHeatBoth.Checked) Entity = Selentity;
                            }
                            else if(!string.IsNullOrEmpty(Year.Trim()))   //else if (Year == "2016" || Year == "2017" || Year == "2018") commented by sudheer on 01/10/2020
                            {
                                Entity = Selentity;
                                //if (rbDeliverables.Checked) { if (Selentity.LPMQ_QTYPE != "N") Entity = Selentity; else Entity = null; }
                                //else if (rbUtilities.Checked) { if (Selentity.LPMQ_QTYPE != "Y") Entity = Selentity; else Entity = null; }
                                //else if (rbHeatBoth.Checked) Entity = Selentity;
                            }
                            
                            if (!Isfirst)
                            {
                                if (QType != Entity.LPMQ_QTYPE)
                                {
                                    if (table.Rows.Count > 0)
                                    {
                                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        P9.Border = iTextSharp.text.Rectangle.BOX;
                                        table.AddCell(P9);

                                        PdfPCell P11 = new PdfPCell(new Phrase("Grand Total", TableFont));
                                        P11.HorizontalAlignment = Element.ALIGN_CENTER;
                                        P11.Colspan = 2;
                                        P11.Border = iTextSharp.text.Rectangle.BOX;
                                        table.AddCell(P11);

                                        if (QType == "N")
                                        {
                                            List<CommonEntity> SelCCountEntity = CCountEntity.FindAll(u => u.Code.Equals("04"));
                                            int Count = SelCCountEntity.Sum(u => Convert.ToInt32(u.Hierarchy.Trim()));
                                            PdfPCell P01 = new PdfPCell(new Phrase(Count.ToString(), TableFont));
                                            P01.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            P01.Border = iTextSharp.text.Rectangle.BOX;
                                            table.AddCell(P01);

                                            SelCCountEntity = CCountEntity.FindAll(u => u.Code.Equals("02"));
                                            Count = SelCCountEntity.Sum(u => Convert.ToInt32(u.Hierarchy.Trim()));
                                            PdfPCell P02 = new PdfPCell(new Phrase(Count.ToString(), TableFont));
                                            P02.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            P02.Border = iTextSharp.text.Rectangle.BOX;
                                            table.AddCell(P02);
                                        }
                                        else
                                        {
                                            if (LPMS_Count.Count > 0)
                                            {
                                                foreach (var item in LPMS_Count)
                                                {
                                                    List<CommonEntity> SelCCountEntity = CCountEntity.FindAll(u => u.Code.Equals(item));
                                                    int Count = SelCCountEntity.Sum(u => Convert.ToInt32(u.Hierarchy.Trim()));

                                                    PdfPCell P01 = new PdfPCell(new Phrase(Count.ToString(), TableFont));
                                                    P01.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                    P01.Border = iTextSharp.text.Rectangle.BOX;
                                                    table.AddCell(P01);
                                                }
                                            }
                                        }

                                        PdfPCell P12 = new PdfPCell(new Phrase(GYes.ToString(), TableFont));
                                        P12.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        P12.Border = iTextSharp.text.Rectangle.BOX;
                                        table.AddCell(P12);

                                        PdfPCell P13 = new PdfPCell(new Phrase(GNo.ToString(), TableFont));
                                        P13.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        P13.Border = iTextSharp.text.Rectangle.BOX;
                                        table.AddCell(P13);

                                        PdfPCell P14 = new PdfPCell(new Phrase(GNA.ToString(), TableFont));
                                        P14.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        P14.Border = iTextSharp.text.Rectangle.BOX;
                                        table.AddCell(P14);

                                        PdfPCell P15 = new PdfPCell(new Phrase(GT.ToString(), TableFont));
                                        P15.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        P15.Border = iTextSharp.text.Rectangle.BOX;
                                        table.AddCell(P15);

                                        document.Add(table);

                                        table.DeleteBodyRows();

                                        
                                        PdfPCell P1 = new PdfPCell(new Phrase("", TblFontBold));
                                        P1.HorizontalAlignment = Element.ALIGN_CENTER;
                                        P1.Colspan = 7;
                                        P1.FixedHeight = 25f;
                                        P1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(P1);

                                        document.NewPage();

                                        
                                        float[] widths;
                                        if (Entity.LPMQ_QTYPE == "Y")
                                        {
                                            List<LPMQEntity> SELPMList = LPMList.FindAll(u => u.Mst_Heating_Source != "" && u.Mst_Heating_Source!="02" && u.Mst_Heating_Source!="04"); //LPMList.OrderBy(u => u.Mst_Heating_Source.Trim()).ToList();
                                            SELPMList = SELPMList.OrderBy(u => u.Mst_Heating_Source.Trim()).ToList();
                                            LPMS_Count = SELPMList.Select(u => u.Mst_Heating_Source.Trim()).Distinct().ToList();
                                        }
                                        //var LPMS_Count = LPMList.Select(u => u.Mst_Heating_Source.Trim()).Distinct().ToList();
                                        
                                        int ColumnsCount = 7 + LPMS_Count.Count;
                                        table = new PdfPTable(ColumnsCount);
                                        table.TotalWidth = 700f;
                                        table.WidthPercentage = 100;
                                        table.LockedWidth = true;
                                        switch (LPMS_Count.Count)
                                        {
                                            case 1: widths = new float[] { 8f, 20f, 45f, 15f, 13f, 13f, 20f, 15f }; table.SetWidths(widths); break;
                                            case 2: widths = new float[] { 8f, 20f, 45f, 15f, 15f, 13f, 13f, 20f, 15f }; table.SetWidths(widths); break;
                                            case 3: widths = new float[] { 8f, 20f, 45f, 15f, 15f, 15f, 13f, 13f, 20f, 15f }; table.SetWidths(widths); break;
                                            case 4: widths = new float[] { 8f, 20f, 45f, 15f, 15f, 15f, 15f, 13f, 13f, 20f, 15f }; table.SetWidths(widths); break;
                                            case 5: widths = new float[] { 8f, 20f, 45f, 15f, 15f, 15f, 15f, 15f, 13f, 13f, 20f, 15f }; table.SetWidths(widths); break;
                                            case 6: widths = new float[] { 8f, 20f, 45f, 15f, 15f, 15f, 15f, 15f, 15f, 13f, 13f, 20f, 15f }; table.SetWidths(widths); break;
                                            case 7: widths = new float[] { 8f, 20f, 45f, 15f, 15f, 15f, 15f, 15f, 15f, 15f, 13f, 13f, 20f, 15f }; table.SetWidths(widths); break;
                                            case 8: widths = new float[] { 8f, 20f, 45f, 15f, 15f, 15f, 15f, 15f, 15f, 15f, 15f, 13f, 13f, 20f, 15f }; table.SetWidths(widths); break;
                                            case 9: widths = new float[] { 8f, 20f, 45f, 15f, 15f, 15f, 15f, 15f, 15f, 15f, 15f, 15f, 13f, 13f, 20f, 15f }; table.SetWidths(widths); break;
                                            case 10: widths = new float[] { 8f, 20f, 45f, 15f, 15f, 15f, 15f, 15f, 15f, 15f, 15f, 15f, 15f, 13f, 13f, 20f, 15f }; table.SetWidths(widths); break;
                                        }
                                        //if(LPMS_Count.Count==5)
                                        //    widths = new float[] { 8f, 20f, 45f, 15f, 15f, 15f, 15f, 15f, 13f, 13f, 20f, 15f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };

                                        table.HorizontalAlignment = Element.ALIGN_CENTER;
                                        //table.HeaderRows = 1;

                                        string[] HeaderSeq4 = { "S.No", "Question" };
                                        for (int i = 0; i < HeaderSeq4.Length; ++i)
                                        {
                                            PdfPCell cell = new PdfPCell(new Phrase(HeaderSeq4[i], TblFontBold));
                                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                            if (i == 1) cell.Colspan = 2;
                                            cell.Border = iTextSharp.text.Rectangle.BOX;
                                            table.AddCell(cell);
                                        }

                                        if (LPMS_Count.Count > 0)
                                        {
                                            foreach (var item in LPMS_Count)
                                            {
                                                string Desc = PrimarySourceHeat.Find(u => u.Code.Equals(item)).Desc.Trim();

                                                PdfPCell cell = new PdfPCell(new Phrase(Desc, TblFontBold));
                                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                                //if (i == 1) cell.Colspan = 2;
                                                cell.Border = iTextSharp.text.Rectangle.BOX;
                                                table.AddCell(cell);
                                            }
                                        }

                                        string[] HeaderSeq = { "Yes", "No", "Not Applicable", "Total" };
                                        for (int i = 0; i < HeaderSeq.Length; ++i)
                                        {
                                            PdfPCell cell = new PdfPCell(new Phrase(HeaderSeq[i], TblFontBold));
                                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                            //if (i == 1) cell.Colspan = 2;
                                            cell.Border = iTextSharp.text.Rectangle.BOX;
                                            table.AddCell(cell);
                                        }

                                        //string[] HeaderSeq = { "S.No", "Question", "Yes", "No", "Not Applicable", "Total" };
                                        //for (int i = 0; i < HeaderSeq.Length; ++i)
                                        //{
                                        //    PdfPCell cell = new PdfPCell(new Phrase(HeaderSeq[i], TblFontBold));
                                        //    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                        //    if (i == 1) cell.Colspan = 2;
                                        //    cell.Border = iTextSharp.text.Rectangle.BOX;
                                        //    table.AddCell(cell);
                                        //}

                                        GYes = 0; GNo = 0; GNA = 0; GT = 0; Isfirst = true;
                                        CCountEntity = new List<CommonEntity>();

                                    }
                                }
                            }

                            if ((!string.IsNullOrEmpty(Year.Trim())) && rbHeatBoth.Checked && Isfirst == true) //Year == "2016" || Year == "2017" || Year == "2018" commented by sudheer on 01/10/2020
                            {
                                string Desc = string.Empty;
                                if (Entity.LPMQ_QTYPE == "N") Desc = "Utilities"; else Desc = "Deliverables";
                                PdfPCell P1 = new PdfPCell(new Phrase(Desc, TblFontBold));
                                P1.HorizontalAlignment = Element.ALIGN_CENTER;
                                if (Entity.LPMQ_QTYPE == "N") P1.Colspan = 9;
                                else P1.Colspan = 7+LPMS_Count.Count;
                                P1.Border = iTextSharp.text.Rectangle.BOX;
                                table.AddCell(P1);
                                QType = Entity.LPMQ_QTYPE; Isfirst = false;
                            }

                            if (Entity != null)
                            {
                                //foreach (LIHPMQuesEntity Entity in sellihpmQues)
                                //{
                                //if (rbHeatBoth.Checked == false)
                                //{
                                PdfPCell P1 = new PdfPCell(new Phrase(int.Parse(Entity.LPMQ_CODE).ToString(), TableFont));
                                P1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                P1.Border = iTextSharp.text.Rectangle.BOX;
                                table.AddCell(P1);
                                //}
                                //else
                                //{
                                //    PdfPCell P1 = new PdfPCell(new Phrase(j.ToString(), TableFont));
                                //    P1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //    P1.Border = iTextSharp.text.Rectangle.BOX;
                                //    table.AddCell(P1);
                                //}


                                PdfPCell P2 = new PdfPCell(new Phrase(Entity.LPMQ_DESC.Trim(), TableFont));
                                P2.HorizontalAlignment = Element.ALIGN_LEFT;
                                P2.Colspan = 2;
                                P2.Border = iTextSharp.text.Rectangle.BOX;
                                table.AddCell(P2);
                               
                                List<LPMQEntity> YesCount = new List<LPMQEntity>();
                                List<LPMQEntity> NoCount = new List<LPMQEntity>(); List<LPMQEntity> NACount = new List<LPMQEntity>();
                                List<LPMQEntity> SelList = new List<LPMQEntity>();
                                //if (Year == "2016")
                                //{
                                switch (Entity.LPMQ_CODE)
                                {
                                    case "0001": if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0001.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0001.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0001.Equals("Y"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0001.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0001.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0001.Equals("N"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0001.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0001.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0001.Equals("U"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0001 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0001 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0001 != "");
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim()))
                                        {
                                            if (Entity.LPMQ_QTYPE == "N")
                                            {
                                                int Count = LPMList.FindAll(u => u.LPM_0001 != "" && (u.Mst_Heating_Source == "04")).Count;
                                                CCountEntity.Add(new CommonEntity("04", "Electric", Count.ToString(), Entity.LPMQ_CODE));

                                                Count = LPMList.FindAll(u => u.LPM_0001 != "" && (u.Mst_Heating_Source == "02")).Count;
                                                CCountEntity.Add(new CommonEntity("02", "Natural Gas", Count.ToString(), Entity.LPMQ_CODE));
                                            }
                                            else
                                            {
                                                foreach (var item in LPMS_Count)
                                                {
                                                    foreach (CommonEntity Agytab in PrimarySourceHeat)
                                                    {
                                                        if (item == Agytab.Code)
                                                        {
                                                            int Count = LPMList.FindAll(u => u.LPM_0001 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04" && u.Mst_Heating_Source.Equals(Agytab.Code.Trim()))).Count;

                                                            CCountEntity.Add(new CommonEntity(Agytab.Code, Agytab.Desc, Count.ToString(), Entity.LPMQ_CODE));
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    case "0002": if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0002.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0002.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0002.Equals("Y"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0002.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0002.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0002.Equals("N"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0002.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0002.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0002.Equals("U"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0002 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0002 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0002 != ""); 
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim()))
                                        {
                                            if (Entity.LPMQ_QTYPE == "N")
                                            {
                                                int Count = LPMList.FindAll(u => u.LPM_0002 != "" && (u.Mst_Heating_Source == "04")).Count;
                                                CCountEntity.Add(new CommonEntity("04", "Electric", Count.ToString(), Entity.LPMQ_CODE));

                                                Count = LPMList.FindAll(u => u.LPM_0002 != "" && (u.Mst_Heating_Source == "02")).Count;
                                                CCountEntity.Add(new CommonEntity("02", "Natural Gas", Count.ToString(), Entity.LPMQ_CODE));
                                            }
                                            else
                                            {
                                                foreach (var item in LPMS_Count)
                                                {
                                                    foreach (CommonEntity Agytab in PrimarySourceHeat)
                                                    {
                                                        if (item == Agytab.Code)
                                                        {
                                                            int Count = LPMList.FindAll(u => u.LPM_0002 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04" && u.Mst_Heating_Source.Equals(Agytab.Code.Trim()))).Count;

                                                            CCountEntity.Add(new CommonEntity(Agytab.Code, Agytab.Desc, Count.ToString(), Entity.LPMQ_CODE));
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }break;
                                    case "0003": if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0003.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0003.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0003.Equals("Y"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0003.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0003.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0003.Equals("N"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0003.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0003.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0003.Equals("U"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0003 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0003 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0003 != ""); 
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim()))
                                        {
                                            if (Entity.LPMQ_QTYPE == "N")
                                            {
                                                int Count = LPMList.FindAll(u => u.LPM_0003 != "" && (u.Mst_Heating_Source == "04")).Count;
                                                CCountEntity.Add(new CommonEntity("04", "Electric", Count.ToString(), Entity.LPMQ_CODE));

                                                Count = LPMList.FindAll(u => u.LPM_0003 != "" && (u.Mst_Heating_Source == "02")).Count;
                                                CCountEntity.Add(new CommonEntity("02", "Natural Gas", Count.ToString(), Entity.LPMQ_CODE));
                                            }
                                            else
                                            {
                                                foreach (var item in LPMS_Count)
                                                {
                                                    foreach (CommonEntity Agytab in PrimarySourceHeat)
                                                    {
                                                        if (item == Agytab.Code)
                                                        {
                                                            int Count = LPMList.FindAll(u => u.LPM_0003 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04" && u.Mst_Heating_Source.Equals(Agytab.Code.Trim()))).Count;

                                                            CCountEntity.Add(new CommonEntity(Agytab.Code, Agytab.Desc, Count.ToString(), Entity.LPMQ_CODE));
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }break;
                                    case "0004": if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0004.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0004.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0004.Equals("Y"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0004.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0004.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0004.Equals("N"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0004.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0004.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0004.Equals("U"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0004 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0004 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0004 != ""); 
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim()))
                                        {
                                            if (Entity.LPMQ_QTYPE == "N")
                                            {
                                                int Count = LPMList.FindAll(u => u.LPM_0004 != "" && (u.Mst_Heating_Source == "04")).Count;
                                                CCountEntity.Add(new CommonEntity("04", "Electric", Count.ToString(), Entity.LPMQ_CODE));

                                                Count = LPMList.FindAll(u => u.LPM_0004 != "" && (u.Mst_Heating_Source == "02")).Count;
                                                CCountEntity.Add(new CommonEntity("02", "Natural Gas", Count.ToString(), Entity.LPMQ_CODE));
                                            }
                                            else
                                            {
                                                foreach (var item in LPMS_Count)
                                                {
                                                    foreach (CommonEntity Agytab in PrimarySourceHeat)
                                                    {
                                                        if (item == Agytab.Code)
                                                        {
                                                            int Count = LPMList.FindAll(u => u.LPM_0004 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04" && u.Mst_Heating_Source.Equals(Agytab.Code.Trim()))).Count;

                                                            CCountEntity.Add(new CommonEntity(Agytab.Code, Agytab.Desc, Count.ToString(), Entity.LPMQ_CODE));
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }break;
                                    case "0005": if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0005.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0005.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0005.Equals("Y"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0005.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0005.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0005.Equals("N"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0005.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0005.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0005.Equals("U"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0005 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0005 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0005 != ""); 
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim()))
                                        {
                                            if (Entity.LPMQ_QTYPE == "N")
                                            {
                                                int Count = LPMList.FindAll(u => u.LPM_0005 != "" && (u.Mst_Heating_Source == "04")).Count;
                                                CCountEntity.Add(new CommonEntity("04", "Electric", Count.ToString(), Entity.LPMQ_CODE));

                                                Count = LPMList.FindAll(u => u.LPM_0005 != "" && (u.Mst_Heating_Source == "02")).Count;
                                                CCountEntity.Add(new CommonEntity("02", "Natural Gas", Count.ToString(), Entity.LPMQ_CODE));
                                            }
                                            else
                                            {
                                                foreach (var item in LPMS_Count)
                                                {
                                                    foreach (CommonEntity Agytab in PrimarySourceHeat)
                                                    {
                                                        if (item == Agytab.Code)
                                                        {
                                                            int Count = LPMList.FindAll(u => u.LPM_0005 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04" && u.Mst_Heating_Source.Equals(Agytab.Code.Trim()))).Count;

                                                            CCountEntity.Add(new CommonEntity(Agytab.Code, Agytab.Desc, Count.ToString(), Entity.LPMQ_CODE));
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }break;
                                    case "0006": if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0006.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0006.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0006.Equals("Y"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0006.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0006.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0006.Equals("N"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0006.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0006.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0006.Equals("U"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0006 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0006 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0006 != ""); 
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim()))
                                        {
                                            if (Entity.LPMQ_QTYPE == "N")
                                            {
                                                int Count = LPMList.FindAll(u => u.LPM_0006 != "" && (u.Mst_Heating_Source == "04")).Count;
                                                CCountEntity.Add(new CommonEntity("04", "Electric", Count.ToString(), Entity.LPMQ_CODE));

                                                Count = LPMList.FindAll(u => u.LPM_0006 != "" && (u.Mst_Heating_Source == "02")).Count;
                                                CCountEntity.Add(new CommonEntity("02", "Natural Gas", Count.ToString(), Entity.LPMQ_CODE));
                                            }
                                            else
                                            {
                                                foreach (var item in LPMS_Count)
                                                {
                                                    foreach (CommonEntity Agytab in PrimarySourceHeat)
                                                    {
                                                        if (item == Agytab.Code)
                                                        {
                                                            int Count = LPMList.FindAll(u => u.LPM_0006 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04" && u.Mst_Heating_Source.Equals(Agytab.Code.Trim()))).Count;

                                                            CCountEntity.Add(new CommonEntity(Agytab.Code, Agytab.Desc, Count.ToString(), Entity.LPMQ_CODE));
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }break;
                                    case "0007": if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0007.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0007.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0007.Equals("Y"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0007.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0007.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0007.Equals("N"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0007.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0007.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0007.Equals("U"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0007 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0007 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0007 != ""); 
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim()))
                                        {
                                            if (Entity.LPMQ_QTYPE == "N")
                                            {
                                                int Count = LPMList.FindAll(u => u.LPM_0007 != "" && (u.Mst_Heating_Source == "04")).Count;
                                                CCountEntity.Add(new CommonEntity("04", "Electric", Count.ToString(), Entity.LPMQ_CODE));

                                                Count = LPMList.FindAll(u => u.LPM_0007 != "" && (u.Mst_Heating_Source == "02")).Count;
                                                CCountEntity.Add(new CommonEntity("02", "Natural Gas", Count.ToString(), Entity.LPMQ_CODE));
                                            }
                                            else
                                            {
                                                foreach (var item in LPMS_Count)
                                                {
                                                    foreach (CommonEntity Agytab in PrimarySourceHeat)
                                                    {
                                                        if (item == Agytab.Code)
                                                        {
                                                            int Count = LPMList.FindAll(u => u.LPM_0007 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04" && u.Mst_Heating_Source.Equals(Agytab.Code.Trim()))).Count;

                                                            CCountEntity.Add(new CommonEntity(Agytab.Code, Agytab.Desc, Count.ToString(), Entity.LPMQ_CODE));
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }break;
                                    case "0008": if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0008.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0008.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0008.Equals("Y"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0008.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0008.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0008.Equals("N"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0008.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0008.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0008.Equals("U"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0008 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0008 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0008 != ""); 
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim()))
                                        {
                                            if (Entity.LPMQ_QTYPE == "N")
                                            {
                                                int Count = LPMList.FindAll(u => u.LPM_0008 != "" && (u.Mst_Heating_Source == "04")).Count;
                                                CCountEntity.Add(new CommonEntity("04", "Electric", Count.ToString(), Entity.LPMQ_CODE));

                                                Count = LPMList.FindAll(u => u.LPM_0008 != "" && (u.Mst_Heating_Source == "02")).Count;
                                                CCountEntity.Add(new CommonEntity("02", "Natural Gas", Count.ToString(), Entity.LPMQ_CODE));
                                            }
                                            else
                                            {
                                                foreach (var item in LPMS_Count)
                                                {
                                                    foreach (CommonEntity Agytab in PrimarySourceHeat)
                                                    {
                                                        if (item == Agytab.Code)
                                                        {
                                                            int Count = LPMList.FindAll(u => u.LPM_0008 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04" && u.Mst_Heating_Source.Equals(Agytab.Code.Trim()))).Count;

                                                            CCountEntity.Add(new CommonEntity(Agytab.Code, Agytab.Desc, Count.ToString(), Entity.LPMQ_CODE));
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }break;
                                    case "0009": if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0009.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0009.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0009.Equals("Y"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0009.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0009.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0009.Equals("N"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0009.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0009.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0009.Equals("U"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0009 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0009 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0009 != ""); 
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim()))
                                        {
                                            if (Entity.LPMQ_QTYPE == "N")
                                            {
                                                int Count = LPMList.FindAll(u => u.LPM_0009 != "" && (u.Mst_Heating_Source == "04")).Count;
                                                CCountEntity.Add(new CommonEntity("04", "Electric", Count.ToString(), Entity.LPMQ_CODE));

                                                Count = LPMList.FindAll(u => u.LPM_0009 != "" && (u.Mst_Heating_Source == "02")).Count;
                                                CCountEntity.Add(new CommonEntity("02", "Natural Gas", Count.ToString(), Entity.LPMQ_CODE));
                                            }
                                            else
                                            {
                                                foreach (var item in LPMS_Count)
                                                {
                                                    foreach (CommonEntity Agytab in PrimarySourceHeat)
                                                    {
                                                        if (item == Agytab.Code)
                                                        {
                                                            int Count = LPMList.FindAll(u => u.LPM_0009 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04" && u.Mst_Heating_Source.Equals(Agytab.Code.Trim()))).Count;

                                                            CCountEntity.Add(new CommonEntity(Agytab.Code, Agytab.Desc, Count.ToString(), Entity.LPMQ_CODE));
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }break;
                                    case "0010": if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0010.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0010.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0010.Equals("Y"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0010.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0010.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0010.Equals("N"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0010.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0010.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0010.Equals("U"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0010 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0010 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0010 != ""); 
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim()))
                                        {
                                            if (Entity.LPMQ_QTYPE == "N")
                                            {
                                                int Count = LPMList.FindAll(u => u.LPM_0010 != "" && (u.Mst_Heating_Source == "04")).Count;
                                                CCountEntity.Add(new CommonEntity("04", "Electric", Count.ToString(), Entity.LPMQ_CODE));

                                                Count = LPMList.FindAll(u => u.LPM_0010 != "" && (u.Mst_Heating_Source == "02")).Count;
                                                CCountEntity.Add(new CommonEntity("02", "Natural Gas", Count.ToString(), Entity.LPMQ_CODE));
                                            }
                                            else
                                            {
                                                foreach (var item in LPMS_Count)
                                                {
                                                    foreach (CommonEntity Agytab in PrimarySourceHeat)
                                                    {
                                                        if (item == Agytab.Code)
                                                        {
                                                            int Count = LPMList.FindAll(u => u.LPM_0010 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04" && u.Mst_Heating_Source.Equals(Agytab.Code.Trim()))).Count;

                                                            CCountEntity.Add(new CommonEntity(Agytab.Code, Agytab.Desc, Count.ToString(), Entity.LPMQ_CODE));
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }break;
                                    case "0011": if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0011.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0011.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0011.Equals("Y"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0011.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0011.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0011.Equals("N"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0011.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0011.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0011.Equals("U"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0011 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0011 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0011 != ""); 
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim()))
                                        {
                                            if (Entity.LPMQ_QTYPE == "N")
                                            {
                                                int Count = LPMList.FindAll(u => u.LPM_0011 != "" && (u.Mst_Heating_Source == "04")).Count;
                                                CCountEntity.Add(new CommonEntity("04", "Electric", Count.ToString(), Entity.LPMQ_CODE));

                                                Count = LPMList.FindAll(u => u.LPM_0011 != "" && (u.Mst_Heating_Source == "02")).Count;
                                                CCountEntity.Add(new CommonEntity("02", "Natural Gas", Count.ToString(), Entity.LPMQ_CODE));
                                            }
                                            else
                                            {
                                                foreach (var item in LPMS_Count)
                                                {
                                                    foreach (CommonEntity Agytab in PrimarySourceHeat)
                                                    {
                                                        if (item == Agytab.Code)
                                                        {
                                                            int Count = LPMList.FindAll(u => u.LPM_0011 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04" && u.Mst_Heating_Source.Equals(Agytab.Code.Trim()))).Count;

                                                            CCountEntity.Add(new CommonEntity(Agytab.Code, Agytab.Desc, Count.ToString(), Entity.LPMQ_CODE));
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }break;
                                    case "0012":
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0012.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0012.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0012.Equals("Y"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0012.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0012.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0012.Equals("N"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0012.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0012.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0012.Equals("U"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0012 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0012 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0012 != "");
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim()))
                                        {
                                            if (Entity.LPMQ_QTYPE == "N")
                                            {
                                                int Count = LPMList.FindAll(u => u.LPM_0012 != "" && (u.Mst_Heating_Source == "04")).Count;
                                                CCountEntity.Add(new CommonEntity("04", "Electric", Count.ToString(), Entity.LPMQ_CODE));

                                                Count = LPMList.FindAll(u => u.LPM_0012 != "" && (u.Mst_Heating_Source == "02")).Count;
                                                CCountEntity.Add(new CommonEntity("02", "Natural Gas", Count.ToString(), Entity.LPMQ_CODE));
                                            }
                                            else
                                            {
                                                foreach (var item in LPMS_Count)
                                                {
                                                    foreach (CommonEntity Agytab in PrimarySourceHeat)
                                                    {
                                                        if (item == Agytab.Code)
                                                        {
                                                            int Count = LPMList.FindAll(u => u.LPM_0012 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04" && u.Mst_Heating_Source.Equals(Agytab.Code.Trim()))).Count;

                                                            CCountEntity.Add(new CommonEntity(Agytab.Code, Agytab.Desc, Count.ToString(), Entity.LPMQ_CODE));
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    case "0013":
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0013.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0013.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0013.Equals("Y"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0013.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0013.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0013.Equals("N"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0013.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0013.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0013.Equals("U"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0013 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0013 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0013 != "");
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim()))
                                        {
                                            if (Entity.LPMQ_QTYPE == "N")
                                            {
                                                int Count = LPMList.FindAll(u => u.LPM_0013 != "" && (u.Mst_Heating_Source == "04")).Count;
                                                CCountEntity.Add(new CommonEntity("04", "Electric", Count.ToString(), Entity.LPMQ_CODE));

                                                Count = LPMList.FindAll(u => u.LPM_0013 != "" && (u.Mst_Heating_Source == "02")).Count;
                                                CCountEntity.Add(new CommonEntity("02", "Natural Gas", Count.ToString(), Entity.LPMQ_CODE));
                                            }
                                            else
                                            {
                                                foreach (var item in LPMS_Count)
                                                {
                                                    foreach (CommonEntity Agytab in PrimarySourceHeat)
                                                    {
                                                        if (item == Agytab.Code)
                                                        {
                                                            int Count = LPMList.FindAll(u => u.LPM_0013 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04" && u.Mst_Heating_Source.Equals(Agytab.Code.Trim()))).Count;

                                                            CCountEntity.Add(new CommonEntity(Agytab.Code, Agytab.Desc, Count.ToString(), Entity.LPMQ_CODE));
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    case "0014":
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0014.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0014.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0014.Equals("Y"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0014.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0014.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0014.Equals("N"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0014.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0014.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0014.Equals("U"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0014 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0014 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0014 != "");
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim()))
                                        {
                                            if (Entity.LPMQ_QTYPE == "N")
                                            {
                                                int Count = LPMList.FindAll(u => u.LPM_0014 != "" && (u.Mst_Heating_Source == "04")).Count;
                                                CCountEntity.Add(new CommonEntity("04", "Electric", Count.ToString(), Entity.LPMQ_CODE));

                                                Count = LPMList.FindAll(u => u.LPM_0014 != "" && (u.Mst_Heating_Source == "02")).Count;
                                                CCountEntity.Add(new CommonEntity("02", "Natural Gas", Count.ToString(), Entity.LPMQ_CODE));
                                            }
                                            else
                                            {
                                                foreach (var item in LPMS_Count)
                                                {
                                                    foreach (CommonEntity Agytab in PrimarySourceHeat)
                                                    {
                                                        if (item == Agytab.Code)
                                                        {
                                                            int Count = LPMList.FindAll(u => u.LPM_0014 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04" && u.Mst_Heating_Source.Equals(Agytab.Code.Trim()))).Count;

                                                            CCountEntity.Add(new CommonEntity(Agytab.Code, Agytab.Desc, Count.ToString(), Entity.LPMQ_CODE));
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    case "0015":
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0015.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0015.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0015.Equals("Y"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0015.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0015.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0015.Equals("N"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0015.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0015.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0015.Equals("U"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0015 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0015 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0015 != "");
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim()))
                                        {
                                            if (Entity.LPMQ_QTYPE == "N")
                                            {
                                                int Count = LPMList.FindAll(u => u.LPM_0015 != "" && (u.Mst_Heating_Source == "04")).Count;
                                                CCountEntity.Add(new CommonEntity("04", "Electric", Count.ToString(), Entity.LPMQ_CODE));

                                                Count = LPMList.FindAll(u => u.LPM_0015 != "" && (u.Mst_Heating_Source == "02")).Count;
                                                CCountEntity.Add(new CommonEntity("02", "Natural Gas", Count.ToString(), Entity.LPMQ_CODE));
                                            }
                                            else
                                            {
                                                foreach (var item in LPMS_Count)
                                                {
                                                    foreach (CommonEntity Agytab in PrimarySourceHeat)
                                                    {
                                                        if (item == Agytab.Code)
                                                        {
                                                            int Count = LPMList.FindAll(u => u.LPM_0015 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04" && u.Mst_Heating_Source.Equals(Agytab.Code.Trim()))).Count;

                                                            CCountEntity.Add(new CommonEntity(Agytab.Code, Agytab.Desc, Count.ToString(), Entity.LPMQ_CODE));
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    case "0016":
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0016.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0016.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0016.Equals("Y"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0016.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0016.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0016.Equals("N"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0016.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0016.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0016.Equals("U"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0016 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0016 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0016 != "");
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim()))
                                        {
                                            if (Entity.LPMQ_QTYPE == "N")
                                            {
                                                int Count = LPMList.FindAll(u => u.LPM_0016 != "" && (u.Mst_Heating_Source == "04")).Count;
                                                CCountEntity.Add(new CommonEntity("04", "Electric", Count.ToString(), Entity.LPMQ_CODE));

                                                Count = LPMList.FindAll(u => u.LPM_0016 != "" && (u.Mst_Heating_Source == "02")).Count;
                                                CCountEntity.Add(new CommonEntity("02", "Natural Gas", Count.ToString(), Entity.LPMQ_CODE));
                                            }
                                            else
                                            {
                                                foreach (var item in LPMS_Count)
                                                {
                                                    foreach (CommonEntity Agytab in PrimarySourceHeat)
                                                    {
                                                        if (item == Agytab.Code)
                                                        {
                                                            int Count = LPMList.FindAll(u => u.LPM_0016 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04" && u.Mst_Heating_Source.Equals(Agytab.Code.Trim()))).Count;

                                                            CCountEntity.Add(new CommonEntity(Agytab.Code, Agytab.Desc, Count.ToString(), Entity.LPMQ_CODE));
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    case "0017":
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0017.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0017.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0017.Equals("Y"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0017.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0017.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0017.Equals("N"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0017.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0017.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0017.Equals("U"));
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0017 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0017 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0017 != "");
                                        if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim()))
                                        {
                                            if (Entity.LPMQ_QTYPE == "N")
                                            {
                                                int Count = LPMList.FindAll(u => u.LPM_0017 != "" && (u.Mst_Heating_Source == "04")).Count;
                                                CCountEntity.Add(new CommonEntity("04", "Electric", Count.ToString(), Entity.LPMQ_CODE));

                                                Count = LPMList.FindAll(u => u.LPM_0017 != "" && (u.Mst_Heating_Source == "02")).Count;
                                                CCountEntity.Add(new CommonEntity("02", "Natural Gas", Count.ToString(), Entity.LPMQ_CODE));
                                            }
                                            else
                                            {
                                                foreach (var item in LPMS_Count)
                                                {
                                                    foreach (CommonEntity Agytab in PrimarySourceHeat)
                                                    {
                                                        if (item == Agytab.Code)
                                                        {
                                                            int Count = LPMList.FindAll(u => u.LPM_0017 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04" && u.Mst_Heating_Source.Equals(Agytab.Code.Trim()))).Count;

                                                            CCountEntity.Add(new CommonEntity(Agytab.Code, Agytab.Desc, Count.ToString(), Entity.LPMQ_CODE));
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        break;

                                }

                                if (Entity.LPMQ_QTYPE == "N")
                                {
                                    if (CCountEntity.Count > 0)
                                    {
                                        foreach (CommonEntity CEntity in CCountEntity)
                                        {
                                            if (CEntity.Code.ToString() == "04" && Entity.LPMQ_CODE==CEntity.Extension)
                                            {
                                                PdfPCell P00 = new PdfPCell(new Phrase(CEntity.Hierarchy.ToString(), TableFont));
                                                P00.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                P00.Border = iTextSharp.text.Rectangle.BOX;
                                                table.AddCell(P00);
                                            }
                                            else if (CEntity.Code.ToString() == "02" && Entity.LPMQ_CODE == CEntity.Extension)
                                            {
                                                PdfPCell P00 = new PdfPCell(new Phrase(CEntity.Hierarchy.ToString(), TableFont));
                                                P00.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                P00.Border = iTextSharp.text.Rectangle.BOX;
                                                table.AddCell(P00);
                                            }
                                        }

                                        
                                    }
                                    
                                }
                                else 
                                {
                                    if (CCountEntity.Count > 0)
                                    {
                                        List<CommonEntity> SelCCountEntity = CCountEntity.FindAll(u => u.Extension.Equals(Entity.LPMQ_CODE.Trim()));
                                        SelCCountEntity = SelCCountEntity.OrderBy(u => u.Code).ToList();
                                        foreach (CommonEntity CEntity in SelCCountEntity)
                                        {
                                            PdfPCell P00 = new PdfPCell(new Phrase(CEntity.Hierarchy.ToString(), TableFont));
                                            P00.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            P00.Border = iTextSharp.text.Rectangle.BOX;
                                            table.AddCell(P00);

                                        }
                                    }
                                }

                                PdfPCell P3 = new PdfPCell(new Phrase(YesCount.Count.ToString(), TableFont));
                                P3.HorizontalAlignment = Element.ALIGN_RIGHT;
                                P3.Border = iTextSharp.text.Rectangle.BOX;
                                table.AddCell(P3);

                                PdfPCell P4 = new PdfPCell(new Phrase(NoCount.Count.ToString(), TableFont));
                                P4.HorizontalAlignment = Element.ALIGN_RIGHT;
                                P4.Border = iTextSharp.text.Rectangle.BOX;
                                table.AddCell(P4);

                                PdfPCell P5 = new PdfPCell(new Phrase(NACount.Count.ToString(), TableFont));
                                P5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                P5.Border = iTextSharp.text.Rectangle.BOX;
                                table.AddCell(P5);

                                PdfPCell P10 = new PdfPCell(new Phrase((YesCount.Count + NoCount.Count + NACount.Count).ToString(), TableFont));
                                P10.HorizontalAlignment = Element.ALIGN_RIGHT;
                                P10.Border = iTextSharp.text.Rectangle.BOX;
                                table.AddCell(P10);

                                GYes += YesCount.Count; GNo += NoCount.Count; GNA += NACount.Count; GT += YesCount.Count + NoCount.Count + NACount.Count;

                                //if (rbDetail.Checked)
                                //{
                                //    if (SelList.Count > 0)
                                //    {
                                //        foreach (LPMQEntity LEntity in SelList)
                                //        {
                                //            PdfPCell P6 = new PdfPCell(new Phrase("", TableFont));
                                //            P6.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //            P6.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                //            table.AddCell(P6);

                                //            PdfPCell P7 = new PdfPCell(new Phrase(LEntity.ApplNo.Trim(), TableFont));
                                //            P7.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //            P7.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //            table.AddCell(P7);

                                //            PdfPCell P8 = new PdfPCell(new Phrase(LookupDataAccess.GetMemberName(LEntity.NameixFi, LEntity.NameixMi, LEntity.NameixLast, BaseForm.BaseHierarchyCnFormat.ToString()), TableFont));
                                //            P8.HorizontalAlignment = Element.ALIGN_LEFT;
                                //            P8.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //            table.AddCell(P8);

                                //            switch (Entity.LPMQ_CODE)
                                //            {
                                //                case "0001": if (LEntity.LPM_0001 == ("Y"))
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0001.Trim(), TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    else
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    if (LEntity.LPM_0001 == ("N"))
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0001.Trim(), TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    else
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    if (LEntity.LPM_0001 == ("U"))
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0001.Trim(), TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    else
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    PdfPCell P20 = new PdfPCell(new Phrase("", TableFont));
                                //                    P20.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                    P20.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                //                    table.AddCell(P20);
                                //                    break;
                                //                case "0002": if (LEntity.LPM_0002 == ("Y"))
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0002.Trim(), TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    else
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    if (LEntity.LPM_0002 == ("N"))
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0002.Trim(), TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    else
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    if (LEntity.LPM_0002 == ("U"))
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0002.Trim(), TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    else
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    PdfPCell P21 = new PdfPCell(new Phrase("", TableFont));
                                //                    P21.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                    P21.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                //                    table.AddCell(P21);
                                //                    break;
                                //                case "0003": if (LEntity.LPM_0003 == ("Y"))
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0003.Trim(), TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    else
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    if (LEntity.LPM_0003 == ("N"))
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0003.Trim(), TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    else
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    if (LEntity.LPM_0003 == ("U"))
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0003.Trim(), TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    else
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    PdfPCell P22 = new PdfPCell(new Phrase("", TableFont));
                                //                    P22.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                    P22.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                //                    table.AddCell(P22);
                                //                    break;
                                //                case "0004": if (LEntity.LPM_0004 == ("Y"))
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0004.Trim(), TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    else
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    if (LEntity.LPM_0004 == ("N"))
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0004.Trim(), TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    else
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    if (LEntity.LPM_0004 == ("U"))
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0004.Trim(), TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    else
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    PdfPCell P23 = new PdfPCell(new Phrase("", TableFont));
                                //                    P23.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                    P23.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                //                    table.AddCell(P23);
                                //                    break;
                                //                case "0005": if (LEntity.LPM_0005 == ("Y"))
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0005.Trim(), TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    else
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    if (LEntity.LPM_0005 == ("N"))
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0005.Trim(), TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    else
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    if (LEntity.LPM_0005 == ("U"))
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0005.Trim(), TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    else
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    PdfPCell P24 = new PdfPCell(new Phrase("", TableFont));
                                //                    P24.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                    P24.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                //                    table.AddCell(P24);
                                //                    break;
                                //                case "0006": if (LEntity.LPM_0006 == ("Y"))
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0006.Trim(), TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    else
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    if (LEntity.LPM_0006 == ("N"))
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0006.Trim(), TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    else
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    if (LEntity.LPM_0006 == ("U"))
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0006.Trim(), TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    else
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    PdfPCell P25 = new PdfPCell(new Phrase("", TableFont));
                                //                    P25.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                    P25.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                //                    table.AddCell(P25);
                                //                    break;
                                //                case "0007": if (LEntity.LPM_0007 == ("Y"))
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0007.Trim(), TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    else
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    if (LEntity.LPM_0007 == ("N"))
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0007.Trim(), TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    else
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    if (LEntity.LPM_0007 == ("U"))
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0007.Trim(), TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    else
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    PdfPCell P26 = new PdfPCell(new Phrase("", TableFont));
                                //                    P26.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                    P26.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                //                    table.AddCell(P26);
                                //                    break;
                                //                case "0008": if (LEntity.LPM_0008 == ("Y"))
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0008.Trim(), TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    else
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    if (LEntity.LPM_0008 == ("N"))
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0008.Trim(), TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    else
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    if (LEntity.LPM_0008 == ("U"))
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0008.Trim(), TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    else
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    PdfPCell P27 = new PdfPCell(new Phrase("", TableFont));
                                //                    P27.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                    P27.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                //                    table.AddCell(P27);
                                //                    break;
                                //                case "0009": if (LEntity.LPM_0009 == ("Y"))
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0009.Trim(), TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    else
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    if (LEntity.LPM_0009 == ("N"))
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0009.Trim(), TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    else
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    if (LEntity.LPM_0009 == ("U"))
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0009.Trim(), TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    else
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    PdfPCell P28 = new PdfPCell(new Phrase("", TableFont));
                                //                    P28.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                    P28.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                //                    table.AddCell(P28);
                                //                    break;
                                //                case "0010": if (LEntity.LPM_0010 == ("Y"))
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0010.Trim(), TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    else
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    if (LEntity.LPM_0010 == ("N"))
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0010.Trim(), TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    else
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    if (LEntity.LPM_0010 == ("U"))
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0010.Trim(), TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    else
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    PdfPCell P29 = new PdfPCell(new Phrase("", TableFont));
                                //                    P29.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                    P29.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                //                    table.AddCell(P29);
                                //                    break;
                                //                case "0011": if (LEntity.LPM_0011 == ("Y"))
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0011.Trim(), TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    else
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    if (LEntity.LPM_0011 == ("N"))
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0011.Trim(), TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    else
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    if (LEntity.LPM_0011 == ("U"))
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0011.Trim(), TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    else
                                //                    {
                                //                        PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                                //                        P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                        P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //                        table.AddCell(P9);
                                //                    }
                                //                    PdfPCell P30 = new PdfPCell(new Phrase("", TableFont));
                                //                    P30.HorizontalAlignment = Element.ALIGN_RIGHT;
                                //                    P30.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                //                    table.AddCell(P30);
                                //                    break;
                                //            }


                                //        }
                                //    }
                                //}

                                j++;
                            }

                        }


                        //}

                        if (table.Rows.Count > 0)
                        {
                            PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                            P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                            P9.Border = iTextSharp.text.Rectangle.BOX;
                            table.AddCell(P9);

                            PdfPCell P11 = new PdfPCell(new Phrase("Grand Total", TableFont));
                            P11.HorizontalAlignment = Element.ALIGN_CENTER;
                            P11.Colspan = 2;
                            P11.Border = iTextSharp.text.Rectangle.BOX;
                            table.AddCell(P11);

                            if (LQType == "N")
                            {
                                List<CommonEntity> SelCCountEntity = CCountEntity.FindAll(u => u.Code.Equals("04"));
                                int Count = SelCCountEntity.Sum(u => Convert.ToInt32(u.Hierarchy.Trim()));
                                PdfPCell P01 = new PdfPCell(new Phrase(Count.ToString(), TableFont));
                                P01.HorizontalAlignment = Element.ALIGN_RIGHT;
                                P01.Border = iTextSharp.text.Rectangle.BOX;
                                table.AddCell(P01);

                                SelCCountEntity = CCountEntity.FindAll(u => u.Code.Equals("02"));
                                Count = SelCCountEntity.Sum(u => Convert.ToInt32(u.Hierarchy.Trim()));
                                PdfPCell P02 = new PdfPCell(new Phrase(Count.ToString(), TableFont));
                                P02.HorizontalAlignment = Element.ALIGN_RIGHT;
                                P02.Border = iTextSharp.text.Rectangle.BOX;
                                table.AddCell(P02);
                            }
                            else
                            {
                                if (LPMS_Count.Count > 0)
                                {
                                    foreach (var item in LPMS_Count)
                                    {
                                        List<CommonEntity> SelCCountEntity = CCountEntity.FindAll(u => u.Code.Equals(item));
                                        int Count = SelCCountEntity.Sum(u => Convert.ToInt32(u.Hierarchy.Trim()));

                                        PdfPCell P01 = new PdfPCell(new Phrase(Count.ToString(), TableFont));
                                        P01.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        P01.Border = iTextSharp.text.Rectangle.BOX;
                                        table.AddCell(P01);
                                    }
                                }
                            }

                            PdfPCell P12 = new PdfPCell(new Phrase(GYes.ToString(), TableFont));
                            P12.HorizontalAlignment = Element.ALIGN_RIGHT;
                            P12.Border = iTextSharp.text.Rectangle.BOX;
                            table.AddCell(P12);

                            PdfPCell P13 = new PdfPCell(new Phrase(GNo.ToString(), TableFont));
                            P13.HorizontalAlignment = Element.ALIGN_RIGHT;
                            P13.Border = iTextSharp.text.Rectangle.BOX;
                            table.AddCell(P13);

                            PdfPCell P14 = new PdfPCell(new Phrase(GNA.ToString(), TableFont));
                            P14.HorizontalAlignment = Element.ALIGN_RIGHT;
                            P14.Border = iTextSharp.text.Rectangle.BOX;
                            table.AddCell(P14);

                            PdfPCell P15 = new PdfPCell(new Phrase(GT.ToString(), TableFont));
                            P15.HorizontalAlignment = Element.ALIGN_RIGHT;
                            P15.Border = iTextSharp.text.Rectangle.BOX;
                            table.AddCell(P15);

                            document.Add(table);
                        }

                        #region Without Distinct Description
                        //foreach (LIHPMQuesEntity Entity in lihpmQues)
                        //{
                        //        PdfPCell P1 = new PdfPCell(new Phrase(j.ToString(), TableFont));
                        //        P1.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //        P1.Border = iTextSharp.text.Rectangle.BOX;
                        //        table.AddCell(P1);


                        //        PdfPCell P2 = new PdfPCell(new Phrase(Entity.LPMQ_DESC.Trim(), TableFont));
                        //        P2.HorizontalAlignment = Element.ALIGN_LEFT;
                        //        P2.Colspan = 2;
                        //        P2.Border = iTextSharp.text.Rectangle.BOX;
                        //        table.AddCell(P2);

                        //        List<LPMQEntity> YesCount = new List<LPMQEntity>();
                        //        List<LPMQEntity> NoCount = new List<LPMQEntity>(); List<LPMQEntity> NACount = new List<LPMQEntity>();
                        //        List<LPMQEntity> SelList = new List<LPMQEntity>();
                        //        switch (Entity.LPMQ_CODE)
                        //        {
                        //            case "0001":  YesCount = LPMList.FindAll(u => u.LPM_0001.Equals("Y"));
                        //                NoCount = LPMList.FindAll(u => u.LPM_0001.Equals("N"));
                        //                NACount = LPMList.FindAll(u => u.LPM_0001.Equals("U"));
                        //                SelList = LPMList.FindAll(u => u.LPM_0001 != ""); break;
                        //            case "0002": YesCount = LPMList.FindAll(u => u.LPM_0002.Equals("Y"));
                        //                NoCount = LPMList.FindAll(u => u.LPM_0002.Equals("N"));
                        //                NACount = LPMList.FindAll(u => u.LPM_0002.Equals("U"));
                        //                SelList = LPMList.FindAll(u => u.LPM_0002 != ""); break;
                        //            case "0003": YesCount = LPMList.FindAll(u => u.LPM_0003.Equals("Y"));
                        //                NoCount = LPMList.FindAll(u => u.LPM_0003.Equals("N"));
                        //                NACount = LPMList.FindAll(u => u.LPM_0003.Equals("U"));
                        //                SelList = LPMList.FindAll(u => u.LPM_0003 != ""); break;
                        //            case "0004": YesCount = LPMList.FindAll(u => u.LPM_0004.Equals("Y"));
                        //                NoCount = LPMList.FindAll(u => u.LPM_0004.Equals("N"));
                        //                NACount = LPMList.FindAll(u => u.LPM_0004.Equals("U"));
                        //                SelList = LPMList.FindAll(u => u.LPM_0004 != ""); break;
                        //            case "0005": YesCount = LPMList.FindAll(u => u.LPM_0005.Equals("Y"));
                        //                NoCount = LPMList.FindAll(u => u.LPM_0005.Equals("N"));
                        //                NACount = LPMList.FindAll(u => u.LPM_0005.Equals("U"));
                        //                SelList = LPMList.FindAll(u => u.LPM_0005 != ""); break;
                        //            case "0006": YesCount = LPMList.FindAll(u => u.LPM_0006.Equals("Y"));
                        //                NoCount = LPMList.FindAll(u => u.LPM_0006.Equals("N"));
                        //                NACount = LPMList.FindAll(u => u.LPM_0006.Equals("U"));
                        //                SelList = LPMList.FindAll(u => u.LPM_0006 != ""); break;
                        //            case "0007": YesCount = LPMList.FindAll(u => u.LPM_0007.Equals("Y"));
                        //                NoCount = LPMList.FindAll(u => u.LPM_0007.Equals("N"));
                        //                NACount = LPMList.FindAll(u => u.LPM_0007.Equals("U"));
                        //                SelList = LPMList.FindAll(u => u.LPM_0007 != ""); break;
                        //            case "0008": YesCount = LPMList.FindAll(u => u.LPM_0008.Equals("Y"));
                        //                NoCount = LPMList.FindAll(u => u.LPM_0008.Equals("N"));
                        //                NACount = LPMList.FindAll(u => u.LPM_0008.Equals("U"));
                        //                SelList = LPMList.FindAll(u => u.LPM_0008 != ""); break;
                        //            case "0009": YesCount = LPMList.FindAll(u => u.LPM_0009.Equals("Y"));
                        //                NoCount = LPMList.FindAll(u => u.LPM_0009.Equals("N"));
                        //                NACount = LPMList.FindAll(u => u.LPM_0009.Equals("U"));
                        //                SelList = LPMList.FindAll(u => u.LPM_0009 != ""); break;
                        //            case "0010": YesCount = LPMList.FindAll(u => u.LPM_0010.Equals("Y"));
                        //                NoCount = LPMList.FindAll(u => u.LPM_0010.Equals("N"));
                        //                NACount = LPMList.FindAll(u => u.LPM_0010.Equals("U"));
                        //                SelList = LPMList.FindAll(u => u.LPM_0010 != ""); break;
                        //            case "0011": YesCount = LPMList.FindAll(u => u.LPM_0011.Equals("Y"));
                        //                NoCount = LPMList.FindAll(u => u.LPM_0011.Equals("N"));
                        //                NACount = LPMList.FindAll(u => u.LPM_0011.Equals("U"));
                        //                SelList = LPMList.FindAll(u => u.LPM_0011 != ""); break;
                        //        }


                        //        if (rbSummary.Checked || rbBoth.Checked)
                        //        {
                        //            PdfPCell P3 = new PdfPCell(new Phrase(YesCount.Count.ToString(), TableFont));
                        //            P3.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //            P3.Border = iTextSharp.text.Rectangle.BOX;
                        //            table.AddCell(P3);

                        //            PdfPCell P4 = new PdfPCell(new Phrase(NoCount.Count.ToString(), TableFont));
                        //            P4.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //            P4.Border = iTextSharp.text.Rectangle.BOX;
                        //            table.AddCell(P4);

                        //            PdfPCell P5 = new PdfPCell(new Phrase(NACount.Count.ToString(), TableFont));
                        //            P5.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //            P5.Border = iTextSharp.text.Rectangle.BOX;
                        //            table.AddCell(P5);
                        //        }
                        //        else
                        //        {
                        //            PdfPCell P3 = new PdfPCell(new Phrase("", TableFont));
                        //            P3.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //            P3.Border = iTextSharp.text.Rectangle.BOX;
                        //            table.AddCell(P3);

                        //            PdfPCell P4 = new PdfPCell(new Phrase("", TableFont));
                        //            P4.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //            P4.Border = iTextSharp.text.Rectangle.BOX;
                        //            table.AddCell(P4);

                        //            PdfPCell P5 = new PdfPCell(new Phrase("", TableFont));
                        //            P5.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //            P5.Border = iTextSharp.text.Rectangle.BOX;
                        //            table.AddCell(P5);
                        //        }

                        //        if (rbDetail.Checked || rbBoth.Checked)
                        //        {
                        //            if (SelList.Count > 0)
                        //            {
                        //                foreach (LPMQEntity LEntity in SelList)
                        //                {
                        //                    PdfPCell P6 = new PdfPCell(new Phrase("", TableFont));
                        //                    P6.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                    P6.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                    table.AddCell(P6);

                        //                    PdfPCell P7 = new PdfPCell(new Phrase(LEntity.ApplNo.Trim(), TableFont));
                        //                    P7.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                    P7.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                    table.AddCell(P7);

                        //                    PdfPCell P8 = new PdfPCell(new Phrase(LookupDataAccess.GetMemberName(LEntity.NameixFi, LEntity.NameixMi, LEntity.NameixLast, BaseForm.BaseHierarchyCnFormat.ToString()), TableFont));
                        //                    P8.HorizontalAlignment = Element.ALIGN_LEFT;
                        //                    P8.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                    table.AddCell(P8);

                        //                    switch (Entity.LPMQ_CODE)
                        //                    {
                        //                        case "0001": if (LEntity.LPM_0001 == ("Y"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0001.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            if (LEntity.LPM_0001 == ("N"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0001.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            if (LEntity.LPM_0001 == ("U"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0001.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            } break;
                        //                        case "0002": if (LEntity.LPM_0002 == ("Y"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0002.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            if (LEntity.LPM_0002 == ("N"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0002.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            if (LEntity.LPM_0002 == ("U"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0002.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            } break;
                        //                        case "0003": if (LEntity.LPM_0003 == ("Y"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0003.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            if (LEntity.LPM_0003 == ("N"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0003.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            if (LEntity.LPM_0003 == ("U"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0003.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            } break;
                        //                        case "0004": if (LEntity.LPM_0004 == ("Y"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0004.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            if (LEntity.LPM_0004 == ("N"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0004.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            if (LEntity.LPM_0004 == ("U"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0004.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            } break;
                        //                        case "0005": if (LEntity.LPM_0005 == ("Y"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0005.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            if (LEntity.LPM_0005 == ("N"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0005.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            if (LEntity.LPM_0005 == ("U"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0005.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            } break;
                        //                        case "0006": if (LEntity.LPM_0006 == ("Y"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0006.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            if (LEntity.LPM_0006 == ("N"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0006.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            if (LEntity.LPM_0006 == ("U"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0006.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            } break;
                        //                        case "0007": if (LEntity.LPM_0007 == ("Y"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0007.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            if (LEntity.LPM_0007 == ("N"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0007.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            if (LEntity.LPM_0007 == ("U"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0007.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            } break;
                        //                        case "0008": if (LEntity.LPM_0008 == ("Y"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0008.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            if (LEntity.LPM_0008 == ("N"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0008.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            if (LEntity.LPM_0008 == ("U"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0008.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            } break;
                        //                        case "0009": if (LEntity.LPM_0009 == ("Y"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0009.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            if (LEntity.LPM_0009 == ("N"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0009.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            if (LEntity.LPM_0009 == ("U"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0009.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            } break;
                        //                        case "0010": if (LEntity.LPM_0010 == ("Y"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0010.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            if (LEntity.LPM_0010 == ("N"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0010.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            if (LEntity.LPM_0010 == ("U"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0010.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            } break;
                        //                        case "0011": if (LEntity.LPM_0011 == ("Y"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0011.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            if (LEntity.LPM_0011 == ("N"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0011.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            if (LEntity.LPM_0011 == ("U"))
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase(LEntity.LPM_0011.Trim(), TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            }
                        //                            else
                        //                            {
                        //                                PdfPCell P9 = new PdfPCell(new Phrase("", TableFont));
                        //                                P9.HorizontalAlignment = Element.ALIGN_RIGHT;
                        //                                P9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //                                table.AddCell(P9);
                        //                            } break;
                        //                    }


                        //                }
                        //            }
                        //        }

                        //        prevDesc = Entity.LPMQ_DESC.Trim(); PrevCode = Entity.LPMQ_CODE.Trim();
                        //        j++;

                        //}

                        #endregion

                        if (chkbExcel.Checked)
                        {

                            OnExcel_Report(lihpmQues, LPMList, lihpResp, PdfName, Year);
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
        private void GenerateStyles(WorksheetStyleCollection styles)
        {
            try
            {
                // -----------------------------------------------
                //  Default
                // -----------------------------------------------
                WorksheetStyle Default = styles.Add("Normal_1");
                Default.Name = "Normal";
                Default.Font.FontName = "Times New Roman";
                Default.Font.Color = "#000000";
                Default.Alignment.Vertical = StyleVerticalAlignment.Bottom;
                // -----------------------------------------------
                //  s18
                // -----------------------------------------------
                WorksheetStyle s18 = styles.Add("s18");
                s18.Name = "Normal 2";
                s18.Font.FontName = "Calibri";
                s18.Font.Size = 11;
                s18.Font.Color = "#000000";
                s18.Alignment.Vertical = StyleVerticalAlignment.Bottom;
                // -----------------------------------------------
                //  m198552084
                // -----------------------------------------------
                WorksheetStyle m198552084 = styles.Add("m198552084");
                m198552084.Font.Italic = true;
                m198552084.Font.FontName = "Calibri";
                m198552084.Font.Size = 11;
                m198552084.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                m198552084.Alignment.Vertical = StyleVerticalAlignment.Top;
                m198552084.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                m198552084.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                m198552084.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                m198552084.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  m198551552
                // -----------------------------------------------
                WorksheetStyle m198551552 = styles.Add("m198551552");
                m198551552.Font.FontName = "Calibri";
                m198551552.Font.Size = 12;
                m198551552.Font.Color = "#000000";
                m198551552.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                m198551552.Alignment.Vertical = StyleVerticalAlignment.Top;
                m198551552.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                m198551552.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                m198551552.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                m198551552.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  m198551592
                // -----------------------------------------------
                WorksheetStyle m198551592 = styles.Add("m198551592");
                m198551592.Font.Bold = true;
                m198551592.Font.FontName = "Calibri";
                m198551592.Font.Size = 12;
                m198551592.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                m198551592.Alignment.Vertical = StyleVerticalAlignment.Top;
                m198551592.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                m198551592.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                m198551592.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                m198551592.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  m198551612
                // -----------------------------------------------
                WorksheetStyle m198551612 = styles.Add("m198551612");
                m198551612.Font.FontName = "Calibri";
                m198551612.Font.Size = 12;
                m198551612.Interior.Color = "#BFBFBF";
                m198551612.Interior.Pattern = StyleInteriorPattern.Solid;
                m198551612.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                m198551612.Alignment.Vertical = StyleVerticalAlignment.Top;
                m198551612.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                m198551612.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                m198551612.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                m198551612.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  m198551692
                // -----------------------------------------------
                WorksheetStyle m198551692 = styles.Add("m198551692");
                m198551692.Font.FontName = "Calibri";
                m198551692.Font.Size = 12;
                m198551692.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                m198551692.Alignment.Vertical = StyleVerticalAlignment.Top;
                m198551692.Alignment.WrapText = true;
                m198551692.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                m198551692.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                m198551692.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                m198551692.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  m198551712
                // -----------------------------------------------
                WorksheetStyle m198551712 = styles.Add("m198551712");
                m198551712.Font.FontName = "Calibri";
                m198551712.Font.Size = 12;
                m198551712.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                m198551712.Alignment.Vertical = StyleVerticalAlignment.Top;
                m198551712.Alignment.WrapText = true;
                m198551712.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                m198551712.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                m198551712.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                m198551712.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  m198551732
                // -----------------------------------------------
                WorksheetStyle m198551732 = styles.Add("m198551732");
                m198551732.Font.Bold = true;
                m198551732.Font.FontName = "Calibri";
                m198551732.Font.Size = 12;
                m198551732.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                m198551732.Alignment.Vertical = StyleVerticalAlignment.Top;
                m198551732.Alignment.WrapText = true;
                m198551732.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                m198551732.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                m198551732.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                m198551732.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  m198551752
                // -----------------------------------------------
                WorksheetStyle m198551752 = styles.Add("m198551752");
                m198551752.Font.Bold = true;
                m198551752.Font.FontName = "Calibri";
                m198551752.Font.Size = 12;
                m198551752.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                m198551752.Alignment.Vertical = StyleVerticalAlignment.Top;
                m198551752.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                m198551752.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                m198551752.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                m198551752.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  m198551772
                // -----------------------------------------------
                WorksheetStyle m198551772 = styles.Add("m198551772");
                m198551772.Font.FontName = "Calibri";
                m198551772.Font.Size = 12;
                m198551772.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                m198551772.Alignment.Vertical = StyleVerticalAlignment.Top;
                m198551772.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                m198551772.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                m198551772.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                m198551772.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  m198551792
                // -----------------------------------------------
                WorksheetStyle m198551792 = styles.Add("m198551792");
                m198551792.Font.Bold = true;
                m198551792.Font.FontName = "Calibri";
                m198551792.Font.Size = 12;
                m198551792.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                m198551792.Alignment.Vertical = StyleVerticalAlignment.Top;
                m198551792.Alignment.WrapText = true;
                m198551792.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                m198551792.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                m198551792.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                m198551792.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  m198551812
                // -----------------------------------------------
                WorksheetStyle m198551812 = styles.Add("m198551812");
                m198551812.Font.Bold = true;
                m198551812.Font.FontName = "Calibri";
                m198551812.Font.Size = 12;
                m198551812.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                m198551812.Alignment.Vertical = StyleVerticalAlignment.Top;
                m198551812.Alignment.WrapText = true;
                m198551812.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                m198551812.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                m198551812.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                m198551812.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  m198552012
                // -----------------------------------------------
                WorksheetStyle m198552012 = styles.Add("m198552012");
                m198552012.Font.FontName = "Calibri";
                m198552012.Font.Size = 12;
                m198552012.Font.Color = "#000000";
                m198552012.Interior.Color = "#BFBFBF";
                m198552012.Interior.Pattern = StyleInteriorPattern.Solid;
                m198552012.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                m198552012.Alignment.Vertical = StyleVerticalAlignment.Top;
                m198552012.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                m198552012.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                m198552012.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                m198552012.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  m198551080
                // -----------------------------------------------
                WorksheetStyle m198551080 = styles.Add("m198551080");
                m198551080.Font.FontName = "Calibri";
                m198551080.Font.Size = 12;
                m198551080.Interior.Color = "#BFBFBF";
                m198551080.Interior.Pattern = StyleInteriorPattern.Solid;
                m198551080.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                m198551080.Alignment.Vertical = StyleVerticalAlignment.Top;
                m198551080.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                m198551080.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                m198551080.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                m198551080.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  m198551300
                // -----------------------------------------------
                WorksheetStyle m198551300 = styles.Add("m198551300");
                m198551300.Font.Bold = true;
                m198551300.Font.FontName = "Calibri";
                m198551300.Font.Size = 12;
                m198551300.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                m198551300.Alignment.Vertical = StyleVerticalAlignment.Top;
                m198551300.Alignment.WrapText = true;
                m198551300.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                m198551300.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                m198551300.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                m198551300.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  m198551320
                // -----------------------------------------------
                WorksheetStyle m198551320 = styles.Add("m198551320");
                m198551320.Font.Bold = true;
                m198551320.Font.FontName = "Calibri";
                m198551320.Font.Size = 12;
                m198551320.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                m198551320.Alignment.Vertical = StyleVerticalAlignment.Top;
                m198551320.Alignment.WrapText = true;
                m198551320.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                m198551320.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                m198551320.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                m198551320.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  m198551340
                // -----------------------------------------------
                WorksheetStyle m198551340 = styles.Add("m198551340");
                m198551340.Font.Bold = true;
                m198551340.Font.FontName = "Calibri";
                m198551340.Font.Size = 12;
                m198551340.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                m198551340.Alignment.Vertical = StyleVerticalAlignment.Top;
                m198551340.Alignment.WrapText = true;
                m198551340.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                m198551340.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                m198551340.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                m198551340.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  m198551360
                // -----------------------------------------------
                WorksheetStyle m198551360 = styles.Add("m198551360");
                m198551360.Font.Bold = true;
                m198551360.Font.FontName = "Calibri";
                m198551360.Font.Size = 12;
                m198551360.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                m198551360.Alignment.Vertical = StyleVerticalAlignment.Top;
                m198551360.Alignment.WrapText = true;
                m198551360.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                m198551360.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                m198551360.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                m198551360.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  m198551380
                // -----------------------------------------------
                WorksheetStyle m198551380 = styles.Add("m198551380");
                m198551380.Font.FontName = "Calibri";
                m198551380.Font.Size = 12;
                m198551380.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                m198551380.Alignment.Vertical = StyleVerticalAlignment.Top;
                m198551380.Alignment.WrapText = true;
                m198551380.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                m198551380.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                m198551380.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                m198551380.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  m198551400
                // -----------------------------------------------
                WorksheetStyle m198551400 = styles.Add("m198551400");
                m198551400.Font.FontName = "Calibri";
                m198551400.Font.Size = 12;
                m198551400.Interior.Color = "#BFBFBF";
                m198551400.Interior.Pattern = StyleInteriorPattern.Solid;
                m198551400.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                m198551400.Alignment.Vertical = StyleVerticalAlignment.Top;
                m198551400.Alignment.WrapText = true;
                m198551400.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                m198551400.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                m198551400.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                m198551400.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  m198551420
                // -----------------------------------------------
                WorksheetStyle m198551420 = styles.Add("m198551420");
                m198551420.Font.FontName = "Calibri";
                m198551420.Font.Size = 12;
                m198551420.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                m198551420.Alignment.Vertical = StyleVerticalAlignment.Top;
                m198551420.Alignment.WrapText = true;
                m198551420.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                m198551420.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                m198551420.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                m198551420.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  m198551460
                // -----------------------------------------------
                WorksheetStyle m198551460 = styles.Add("m198551460");
                m198551460.Font.Bold = true;
                m198551460.Font.FontName = "Calibri";
                m198551460.Font.Size = 13;
                m198551460.Interior.Color = "#BFBFBF";
                m198551460.Interior.Pattern = StyleInteriorPattern.Solid;
                m198551460.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                m198551460.Alignment.Vertical = StyleVerticalAlignment.Center;
                m198551460.Alignment.WrapText = true;
                m198551460.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                m198551460.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                m198551460.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                m198551460.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  m198551500
                // -----------------------------------------------
                WorksheetStyle m198551500 = styles.Add("m198551500");
                m198551500.Font.FontName = "Calibri";
                m198551500.Font.Size = 12;
                m198551500.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                m198551500.Alignment.Vertical = StyleVerticalAlignment.Top;
                m198551500.Alignment.WrapText = true;
                m198551500.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                m198551500.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                m198551500.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  m198550588
                // -----------------------------------------------
                WorksheetStyle m198550588 = styles.Add("m198550588");
                m198550588.Font.Bold = true;
                m198550588.Font.FontName = "Calibri";
                m198550588.Font.Size = 12;
                m198550588.Font.Color = "#000000";
                m198550588.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                m198550588.Alignment.Vertical = StyleVerticalAlignment.Center;
                m198550588.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                m198550588.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                m198550588.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  m198550608
                // -----------------------------------------------
                WorksheetStyle m198550608 = styles.Add("m198550608");
                m198550608.Font.FontName = "Calibri";
                m198550608.Font.Size = 12;
                m198550608.Font.Color = "#000000";
                m198550608.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                m198550608.Alignment.Vertical = StyleVerticalAlignment.Top;
                m198550608.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                m198550608.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  m198550628
                // -----------------------------------------------
                WorksheetStyle m198550628 = styles.Add("m198550628");
                m198550628.Font.FontName = "Calibri";
                m198550628.Font.Size = 12;
                m198550628.Font.Color = "#000000";
                m198550628.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                m198550628.Alignment.Vertical = StyleVerticalAlignment.Top;
                m198550628.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                m198550628.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  m198550648
                // -----------------------------------------------
                WorksheetStyle m198550648 = styles.Add("m198550648");
                m198550648.Font.FontName = "Calibri";
                m198550648.Font.Size = 12;
                m198550648.Font.Color = "#000000";
                m198550648.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                m198550648.Alignment.Vertical = StyleVerticalAlignment.Center;
                m198550648.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                m198550648.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                m198550648.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  m198550748
                // -----------------------------------------------
                WorksheetStyle m198550748 = styles.Add("m198550748");
                m198550748.Font.Bold = true;
                m198550748.Font.FontName = "Calibri";
                m198550748.Font.Size = 12;
                m198550748.Interior.Color = "#BFBFBF";
                m198550748.Interior.Pattern = StyleInteriorPattern.Solid;
                m198550748.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                m198550748.Alignment.Vertical = StyleVerticalAlignment.Top;
                m198550748.Alignment.WrapText = true;
                m198550748.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                m198550748.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                m198550748.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  m173988904
                // -----------------------------------------------
                WorksheetStyle m173988904 = styles.Add("m173988904");
                m173988904.Font.Bold = true;
                m173988904.Font.FontName = "Calibri";
                m173988904.Font.Size = 12;
                m173988904.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                m173988904.Alignment.Vertical = StyleVerticalAlignment.Center;
                m173988904.Alignment.WrapText = true;
                m173988904.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                m173988904.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                m173988904.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                m173988904.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  m173988984
                // -----------------------------------------------
                WorksheetStyle m173988984 = styles.Add("m173988984");
                m173988984.Font.Bold = true;
                m173988984.Font.FontName = "Calibri";
                m173988984.Font.Size = 12;
                m173988984.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                m173988984.Alignment.Vertical = StyleVerticalAlignment.Top;
                m173988984.Alignment.WrapText = true;
                m173988984.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                m173988984.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                m173988984.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                m173988984.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  m173989004
                // -----------------------------------------------
                WorksheetStyle m173989004 = styles.Add("m173989004");
                m173989004.Font.FontName = "Calibri";
                m173989004.Font.Size = 12;
                m173989004.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                m173989004.Alignment.Vertical = StyleVerticalAlignment.Top;
                m173989004.Alignment.WrapText = true;
                m173989004.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                m173989004.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                m173989004.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                m173989004.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  m173989024
                // -----------------------------------------------
                WorksheetStyle m173989024 = styles.Add("m173989024");
                m173989024.Font.FontName = "Calibri";
                m173989024.Font.Size = 12;
                m173989024.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                m173989024.Alignment.Vertical = StyleVerticalAlignment.Top;
                m173989024.Alignment.WrapText = true;
                m173989024.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                m173989024.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                m173989024.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                m173989024.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  m173989044
                // -----------------------------------------------
                WorksheetStyle m173989044 = styles.Add("m173989044");
                m173989044.Font.FontName = "Calibri";
                m173989044.Font.Size = 12;
                m173989044.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                m173989044.Alignment.Vertical = StyleVerticalAlignment.Top;
                m173989044.Alignment.WrapText = true;
                m173989044.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                m173989044.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                m173989044.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                m173989044.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  m173989064
                // -----------------------------------------------
                WorksheetStyle m173989064 = styles.Add("m173989064");
                m173989064.Font.FontName = "Calibri";
                m173989064.Font.Size = 12;
                m173989064.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                m173989064.Alignment.Vertical = StyleVerticalAlignment.Top;
                m173989064.Alignment.WrapText = true;
                m173989064.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                m173989064.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                m173989064.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                m173989064.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  m179113188
                // -----------------------------------------------
                WorksheetStyle m179113188 = styles.Add("m179113188");
                m179113188.Font.Bold = true;
                m179113188.Font.FontName = "Calibri";
                m179113188.Font.Size = 20;
                m179113188.Interior.Color = "#FFFFFF";
                m179113188.Interior.Pattern = StyleInteriorPattern.Solid;
                m179113188.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                m179113188.Alignment.Vertical = StyleVerticalAlignment.Center;
                m179113188.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                m179113188.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s19
                // -----------------------------------------------
                WorksheetStyle s19 = styles.Add("s19");
                s19.Font.FontName = "Calibri";
                s19.Font.Size = 9;
                s19.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                s19.Alignment.Vertical = StyleVerticalAlignment.Center;
                // -----------------------------------------------
                //  s20
                // -----------------------------------------------
                WorksheetStyle s20 = styles.Add("s20");
                s20.Font.FontName = "Calibri";
                s20.Font.Size = 9;
                s20.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                s20.Alignment.Vertical = StyleVerticalAlignment.Center;
                s20.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s21
                // -----------------------------------------------
                WorksheetStyle s21 = styles.Add("s21");
                s21.Font.Bold = true;
                s21.Font.Italic = true;
                s21.Font.FontName = "Calibri";
                s21.Font.Size = 11;
                s21.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s21.Alignment.Vertical = StyleVerticalAlignment.Center;
                s21.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s21.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s21.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s21.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                s21.NumberFormat = "#,##0;\\-#,##0";
                // -----------------------------------------------
                //  s22
                // -----------------------------------------------
                WorksheetStyle s22 = styles.Add("s22");
                s22.Font.Bold = true;
                s22.Font.FontName = "Calibri";
                s22.Font.Size = 11;
                s22.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s22.Alignment.Vertical = StyleVerticalAlignment.Center;
                s22.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s22.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s22.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s22.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s23
                // -----------------------------------------------
                WorksheetStyle s23 = styles.Add("s23");
                s23.Font.FontName = "Calibri";
                s23.Font.Size = 12;
                s23.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                s23.Alignment.Vertical = StyleVerticalAlignment.Center;
                s23.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s23.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s23.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s23.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s24
                // -----------------------------------------------
                WorksheetStyle s24 = styles.Add("s24");
                s24.Font.FontName = "Calibri";
                s24.Font.Size = 11;
                s24.Interior.Color = "#D9D9D9";
                s24.Interior.Pattern = StyleInteriorPattern.Solid;
                s24.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s24.Alignment.Vertical = StyleVerticalAlignment.Center;
                s24.NumberFormat = "#,##0;\\-#,##0";
                // -----------------------------------------------
                //  s25
                // -----------------------------------------------
                WorksheetStyle s25 = styles.Add("s25");
                s25.Font.Bold = true;
                s25.Font.FontName = "Calibri";
                s25.Font.Size = 18;
                s25.Font.Color = "#C00000";
                s25.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                s25.Alignment.Vertical = StyleVerticalAlignment.Top;
                s25.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s26
                // -----------------------------------------------
                WorksheetStyle s26 = styles.Add("s26");
                s26.Font.FontName = "Calibri";
                s26.Font.Size = 11;
                s26.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s26.Alignment.Vertical = StyleVerticalAlignment.Center;
                s26.NumberFormat = "#,##0;\\-#,##0";
                // -----------------------------------------------
                //  s27
                // -----------------------------------------------
                WorksheetStyle s27 = styles.Add("s27");
                s27.Font.Bold = true;
                s27.Font.FontName = "Calibri";
                s27.Font.Size = 14;
                s27.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                s27.Alignment.Vertical = StyleVerticalAlignment.Top;
                s27.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s27.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s27.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s28
                // -----------------------------------------------
                WorksheetStyle s28 = styles.Add("s28");
                s28.Font.Bold = true;
                s28.Font.FontName = "Calibri";
                s28.Font.Size = 18;
                s28.Font.Color = "#C00000";
                s28.Alignment.Vertical = StyleVerticalAlignment.Top;
                // -----------------------------------------------
                //  s29
                // -----------------------------------------------
                WorksheetStyle s29 = styles.Add("s29");
                s29.Font.FontName = "Calibri";
                s29.Font.Size = 11;
                s29.Interior.Color = "#FFFFFF";
                s29.Interior.Pattern = StyleInteriorPattern.Solid;
                s29.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                s29.Alignment.Vertical = StyleVerticalAlignment.Center;
                s29.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s29.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s29.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s29.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s30
                // -----------------------------------------------
                WorksheetStyle s30 = styles.Add("s30");
                s30.Font.FontName = "Calibri";
                s30.Font.Color = "#000000";
                s30.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                s30.Alignment.Vertical = StyleVerticalAlignment.Top;
                // -----------------------------------------------
                //  s31
                // -----------------------------------------------
                WorksheetStyle s31 = styles.Add("s31");
                s31.Font.FontName = "Calibri";
                s31.Font.Color = "#000000";
                // -----------------------------------------------
                //  s32
                // -----------------------------------------------
                WorksheetStyle s32 = styles.Add("s32");
                s32.Font.FontName = "Calibri";
                s32.Font.Color = "#000000";
                s32.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                s32.Alignment.Vertical = StyleVerticalAlignment.Top;
                s32.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#BFBFBF");
                // -----------------------------------------------
                //  s33
                // -----------------------------------------------
                WorksheetStyle s33 = styles.Add("s33");
                s33.Font.Bold = true;
                s33.Font.FontName = "Calibri";
                s33.Font.Color = "#000000";
                s33.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                s33.Alignment.Vertical = StyleVerticalAlignment.Top;
                // -----------------------------------------------
                //  s34
                // -----------------------------------------------
                WorksheetStyle s34 = styles.Add("s34");
                s34.Font.Bold = true;
                s34.Font.FontName = "Calibri";
                s34.Font.Size = 12;
                s34.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                s34.Alignment.Vertical = StyleVerticalAlignment.Top;
                s34.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s35
                // -----------------------------------------------
                WorksheetStyle s35 = styles.Add("s35");
                s35.Font.FontName = "Calibri";
                s35.Font.Size = 12;
                s35.Font.Color = "#000000";
                s35.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                s35.Alignment.Vertical = StyleVerticalAlignment.Top;
                // -----------------------------------------------
                //  s36
                // -----------------------------------------------
                WorksheetStyle s36 = styles.Add("s36");
                s36.Font.FontName = "Calibri";
                s36.Font.Size = 12;
                s36.Font.Color = "#000000";
                s36.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                s36.Alignment.Vertical = StyleVerticalAlignment.Top;
                s36.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s37
                // -----------------------------------------------
                WorksheetStyle s37 = styles.Add("s37");
                s37.Font.FontName = "Calibri";
                s37.Font.Size = 12;
                s37.Font.Color = "#000000";
                s37.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                s37.Alignment.Vertical = StyleVerticalAlignment.Top;
                // -----------------------------------------------
                //  s40
                // -----------------------------------------------
                WorksheetStyle s40 = styles.Add("s40");
                s40.Font.FontName = "Calibri";
                s40.Font.Size = 12;
                s40.Font.Color = "#000000";
                s40.Interior.Color = "#FFFFFF";
                s40.Interior.Pattern = StyleInteriorPattern.Solid;
                s40.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s40.Alignment.Vertical = StyleVerticalAlignment.Top;
                s40.Alignment.WrapText = true;
                s40.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s40.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s40.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s40.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s70
                // -----------------------------------------------
                WorksheetStyle s70 = styles.Add("s70");
                s70.Font.Bold = true;
                s70.Font.FontName = "Calibri";
                s70.Font.Size = 12;
                s70.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                s70.Alignment.Vertical = StyleVerticalAlignment.Top;
                s70.Alignment.WrapText = true;
                s70.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s70.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s70.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s70.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s71
                // -----------------------------------------------
                WorksheetStyle s71 = styles.Add("s71");
                s71.Font.FontName = "Calibri";
                s71.Font.Color = "#000000";
                s71.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                s71.Alignment.Vertical = StyleVerticalAlignment.Top;
                s71.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#BFBFBF");
                // -----------------------------------------------
                //  s72
                // -----------------------------------------------
                WorksheetStyle s72 = styles.Add("s72");
                s72.Font.FontName = "Calibri";
                s72.Font.Color = "#000000";
                s72.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                s72.Alignment.Vertical = StyleVerticalAlignment.Top;
                // -----------------------------------------------
                //  s73
                // -----------------------------------------------
                WorksheetStyle s73 = styles.Add("s73");
                s73.Font.Bold = true;
                s73.Font.FontName = "Calibri";
                s73.Font.Size = 13;
                s73.Interior.Color = "#D9D9D9";
                s73.Interior.Pattern = StyleInteriorPattern.Solid;
                s73.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s73.Alignment.Vertical = StyleVerticalAlignment.Center;
                s73.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s74
                // -----------------------------------------------
                WorksheetStyle s74 = styles.Add("s74");
                s74.Font.Bold = true;
                s74.Font.FontName = "Calibri";
                s74.Font.Size = 11;
                s74.Interior.Color = "#D9D9D9";
                s74.Interior.Pattern = StyleInteriorPattern.Solid;
                s74.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s74.Alignment.Vertical = StyleVerticalAlignment.Center;
                // -----------------------------------------------
                //  s75
                // -----------------------------------------------
                WorksheetStyle s75 = styles.Add("s75");
                s75.Font.FontName = "Calibri";
                s75.Font.Size = 11;
                s75.Interior.Color = "#D9D9D9";
                s75.Interior.Pattern = StyleInteriorPattern.Solid;
                s75.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s75.Alignment.Vertical = StyleVerticalAlignment.Center;
                s75.NumberFormat = "#,##0";
                // -----------------------------------------------
                //  s76
                // -----------------------------------------------
                WorksheetStyle s76 = styles.Add("s76");
                s76.Font.FontName = "Calibri";
                s76.Font.Size = 11;
                s76.Interior.Color = "#D9D9D9";
                s76.Interior.Pattern = StyleInteriorPattern.Solid;
                s76.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s76.Alignment.Vertical = StyleVerticalAlignment.Center;
                s76.NumberFormat = "#,##0";
                // -----------------------------------------------
                //  s77
                // -----------------------------------------------
                WorksheetStyle s77 = styles.Add("s77");
                s77.Font.FontName = "Calibri";
                s77.Font.Size = 11;
                s77.Interior.Color = "#BFBFBF";
                s77.Interior.Pattern = StyleInteriorPattern.Solid;
                s77.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s77.Alignment.Vertical = StyleVerticalAlignment.Center;
                s77.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s77.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s77.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s77.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                s77.NumberFormat = "#,##0";
                // -----------------------------------------------
                //  s81
                // -----------------------------------------------
                WorksheetStyle s81 = styles.Add("s81");
                s81.Font.Bold = true;
                s81.Font.FontName = "Calibri";
                s81.Font.Size = 14;
                s81.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                s81.Alignment.Vertical = StyleVerticalAlignment.Center;
                s81.Alignment.WrapText = true;
                s81.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s88
                // -----------------------------------------------
                WorksheetStyle s88 = styles.Add("s88");
                s88.Font.Bold = true;
                s88.Font.FontName = "Calibri";
                s88.Font.Size = 12;
                s88.Font.Color = "#000000";
                s88.Interior.Color = "#BFBFBF";
                s88.Interior.Pattern = StyleInteriorPattern.Solid;
                s88.Alignment.Vertical = StyleVerticalAlignment.Top;
                s88.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s89
                // -----------------------------------------------
                WorksheetStyle s89 = styles.Add("s89");
                s89.Font.Bold = true;
                s89.Font.FontName = "Calibri";
                s89.Font.Size = 12;
                s89.Font.Color = "#000000";
                s89.Interior.Color = "#BFBFBF";
                s89.Interior.Pattern = StyleInteriorPattern.Solid;
                s89.Alignment.Vertical = StyleVerticalAlignment.Top;
                // -----------------------------------------------
                //  s90
                // -----------------------------------------------
                WorksheetStyle s90 = styles.Add("s90");
                s90.Font.FontName = "Calibri";
                s90.Font.Size = 12;
                s90.Font.Color = "#000000";
                s90.Interior.Color = "#BFBFBF";
                s90.Interior.Pattern = StyleInteriorPattern.Solid;
                s90.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                s90.Alignment.Vertical = StyleVerticalAlignment.Top;
                s90.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s91
                // -----------------------------------------------
                WorksheetStyle s91 = styles.Add("s91");
                s91.Font.Bold = true;
                s91.Font.FontName = "Calibri";
                s91.Font.Size = 12;
                s91.Interior.Color = "#BFBFBF";
                s91.Interior.Pattern = StyleInteriorPattern.Solid;
                s91.Alignment.Vertical = StyleVerticalAlignment.Top;
                s91.Alignment.WrapText = true;
                s91.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s92
                // -----------------------------------------------
                WorksheetStyle s92 = styles.Add("s92");
                s92.Font.Bold = true;
                s92.Font.FontName = "Calibri";
                s92.Font.Size = 12;
                s92.Interior.Color = "#BFBFBF";
                s92.Interior.Pattern = StyleInteriorPattern.Solid;
                s92.Alignment.Vertical = StyleVerticalAlignment.Top;
                s92.Alignment.WrapText = true;
                // -----------------------------------------------
                //  s93
                // -----------------------------------------------
                WorksheetStyle s93 = styles.Add("s93");
                s93.Font.Bold = true;
                s93.Font.FontName = "Calibri";
                s93.Font.Size = 12;
                s93.Interior.Color = "#BFBFBF";
                s93.Interior.Pattern = StyleInteriorPattern.Solid;
                s93.Alignment.Vertical = StyleVerticalAlignment.Top;
                s93.Alignment.WrapText = true;
                s93.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s93.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s94
                // -----------------------------------------------
                WorksheetStyle s94 = styles.Add("s94");
                s94.Font.Bold = true;
                s94.Font.FontName = "Calibri";
                s94.Font.Size = 12;
                s94.Interior.Color = "#BFBFBF";
                s94.Interior.Pattern = StyleInteriorPattern.Solid;
                s94.Alignment.Vertical = StyleVerticalAlignment.Top;
                s94.Alignment.WrapText = true;
                s94.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s95
                // -----------------------------------------------
                WorksheetStyle s95 = styles.Add("s95");
                s95.Font.FontName = "Calibri";
                s95.Font.Size = 12;
                s95.Font.Color = "#000000";
                s95.Interior.Color = "#BFBFBF";
                s95.Interior.Pattern = StyleInteriorPattern.Solid;
                s95.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                s95.Alignment.Vertical = StyleVerticalAlignment.Top;
                s95.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s95.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s96
                // -----------------------------------------------
                WorksheetStyle s96 = styles.Add("s96");
                s96.Font.FontName = "Calibri";
                s96.Font.Size = 12;
                s96.Font.Color = "#000000";
                s96.Interior.Color = "#BFBFBF";
                s96.Interior.Pattern = StyleInteriorPattern.Solid;
                s96.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                s96.Alignment.Vertical = StyleVerticalAlignment.Top;
                s96.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s96.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s96.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s153
                // -----------------------------------------------
                WorksheetStyle s153 = styles.Add("s153");
                s153.Font.FontName = "Calibri";
                s153.Font.Size = 12;
                s153.Interior.Color = "#FF0000";
                s153.Interior.Pattern = StyleInteriorPattern.Solid;
                s153.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                s153.Alignment.Vertical = StyleVerticalAlignment.Center;
                s153.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s153.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s153.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                WorksheetStyleBorder s153Border3 = s153.Borders.Add();
                s153Border3.Position = StylePosition.Top;
                s153Border3.LineStyle = LineStyleOption.Continuous;
                // -----------------------------------------------
                //  s154
                // -----------------------------------------------
                WorksheetStyle s154 = styles.Add("s154");
                s154.Font.Bold = true;
                s154.Font.FontName = "Calibri";
                s154.Font.Size = 11;
                s154.Interior.Color = "#FF0000";
                s154.Interior.Pattern = StyleInteriorPattern.Solid;
                s154.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s154.Alignment.Vertical = StyleVerticalAlignment.Center;
                s154.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s154.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s154.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s154.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                s154.NumberFormat = "#,##0;\\-#,##0";
                // -----------------------------------------------
                //  s155
                // -----------------------------------------------
                WorksheetStyle s155 = styles.Add("s155");
                s155.Font.Bold = true;
                s155.Font.FontName = "Calibri";
                s155.Font.Size = 12;
                s155.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                s155.Alignment.Vertical = StyleVerticalAlignment.Top;
                s155.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s155.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s155.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s155.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s156
                // -----------------------------------------------
                WorksheetStyle s156 = styles.Add("s156");
                s156.Font.Bold = true;
                s156.Font.FontName = "Calibri";
                s156.Font.Size = 12;
                s156.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                s156.Alignment.Vertical = StyleVerticalAlignment.Top;
                s156.Alignment.WrapText = true;
                s156.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s156.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s156.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s156.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s157
                // -----------------------------------------------
                WorksheetStyle s157 = styles.Add("s157");
                s157.Font.FontName = "Calibri";
                s157.Font.Size = 12;
                s157.Font.Color = "#000000";
                s157.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s157.Alignment.Vertical = StyleVerticalAlignment.Top;
                s157.Alignment.WrapText = true;
                s157.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s157.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s157.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s157.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s158
                // -----------------------------------------------
                WorksheetStyle s158 = styles.Add("s158");
                s158.Font.FontName = "Calibri";
                s158.Font.Size = 12;
                s158.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                s158.Alignment.Vertical = StyleVerticalAlignment.Top;
                s158.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s158.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s158.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s158.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s159
                // -----------------------------------------------
                WorksheetStyle s159 = styles.Add("s159");
                s159.Font.FontName = "Calibri";
                s159.Font.Size = 12;
                s159.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                s159.Alignment.Vertical = StyleVerticalAlignment.Top;
                s159.Alignment.WrapText = true;
                s159.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s159.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s159.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s159.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                WorksheetStyle s159C = styles.Add("s159C");
                s159C.Font.FontName = "Calibri";
                s159C.Font.Size = 12;
                s159C.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s159C.Alignment.Vertical = StyleVerticalAlignment.Top;
                s159C.Alignment.WrapText = true;
                s159C.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s159C.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s159C.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s159C.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


                WorksheetStyle s159R = styles.Add("s159R");
                s159R.Font.FontName = "Calibri";
                s159R.Font.Size = 12;
                s159R.Alignment.Horizontal = StyleHorizontalAlignment.Right;
                s159R.Alignment.Vertical = StyleVerticalAlignment.Top;
                s159R.Alignment.WrapText = true;
                s159R.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s159R.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s159R.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s159R.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s160
                // -----------------------------------------------
                WorksheetStyle s160 = styles.Add("s160");
                s160.Font.Bold = true;
                s160.Font.FontName = "Calibri";
                s160.Font.Size = 11;
                s160.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s160.Alignment.Vertical = StyleVerticalAlignment.Center;
                s160.Alignment.WrapText = true;
                s160.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s160.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s160.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s160.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s161
                // -----------------------------------------------
                WorksheetStyle s161 = styles.Add("s161");
                s161.Font.Bold = true;
                s161.Font.FontName = "Calibri";
                s161.Font.Size = 11;
                s161.Font.Color = "#000000";
                s161.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s161.Alignment.Vertical = StyleVerticalAlignment.Center;
                s161.Alignment.WrapText = true;
                s161.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s161.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s161.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s161.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s162
                // -----------------------------------------------
                WorksheetStyle s162 = styles.Add("s162");
                s162.Font.Bold = true;
                s162.Font.FontName = "Calibri";
                s162.Font.Size = 11;
                s162.Font.Color = "#C00000";
                s162.Interior.Color = "#BFBFBF";
                s162.Interior.Pattern = StyleInteriorPattern.Solid;
                s162.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s162.Alignment.Vertical = StyleVerticalAlignment.Center;
                s162.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s162.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s162.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s162.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                s162.NumberFormat = "#,##0";
                // -----------------------------------------------
                //  s163
                // -----------------------------------------------
                WorksheetStyle s163 = styles.Add("s163");
                s163.Font.FontName = "Calibri";
                s163.Font.Size = 12;
                s163.Interior.Color = "#C00000";
                s163.Interior.Pattern = StyleInteriorPattern.Solid;
                s163.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                s163.Alignment.Vertical = StyleVerticalAlignment.Center;
                s163.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s163.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s163.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                WorksheetStyleBorder s163Border3 = s163.Borders.Add();
                s163Border3.Position = StylePosition.Top;
                s163Border3.LineStyle = LineStyleOption.Continuous;
                // -----------------------------------------------
                //  s164
                // -----------------------------------------------
                WorksheetStyle s164 = styles.Add("s164");
                s164.Font.Bold = true;
                s164.Font.FontName = "Calibri";
                s164.Font.Size = 11;
                s164.Interior.Color = "#C00000";
                s164.Interior.Pattern = StyleInteriorPattern.Solid;
                s164.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s164.Alignment.Vertical = StyleVerticalAlignment.Center;
                s164.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s164.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s164.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s164.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                s164.NumberFormat = "#,##0;\\-#,##0";
                // -----------------------------------------------
                //  s165
                // -----------------------------------------------
                WorksheetStyle s165 = styles.Add("s165");
                s165.Font.FontName = "Calibri";
                s165.Font.Size = 11;
                s165.Interior.Color = "#C00000";
                s165.Interior.Pattern = StyleInteriorPattern.Solid;
                s165.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s165.Alignment.Vertical = StyleVerticalAlignment.Center;
                s165.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s165.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s165.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s165.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                s165.NumberFormat = "#,##0";
                // -----------------------------------------------
                //  s166
                // -----------------------------------------------
                WorksheetStyle s166 = styles.Add("s166");
                s166.Font.Bold = true;
                s166.Font.FontName = "Calibri";
                s166.Font.Size = 11;
                s166.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s166.Alignment.Vertical = StyleVerticalAlignment.Center;
                s166.Alignment.WrapText = true;
                s166.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s166.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s166.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s166.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s167
                // -----------------------------------------------
                WorksheetStyle s167 = styles.Add("s167");
                s167.Font.FontName = "Calibri";
                s167.Font.Size = 11;
                s167.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s167.Alignment.Vertical = StyleVerticalAlignment.Center;
                s167.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s167.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s167.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s167.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                s167.NumberFormat = "#,##0";
                // -----------------------------------------------
                //  s168
                // -----------------------------------------------
                WorksheetStyle s168 = styles.Add("s168");
                s168.Font.FontName = "Calibri";
                s168.Font.Size = 11;
                s168.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s168.Alignment.Vertical = StyleVerticalAlignment.Center;
                s168.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s168.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s168.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s168.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                s168.NumberFormat = "#,##0;\\-#,##0";
                // -----------------------------------------------
                //  s169
                // -----------------------------------------------
                WorksheetStyle s169 = styles.Add("s169");
                s169.Font.FontName = "Calibri";
                s169.Font.Size = 12;
                s169.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                s169.Alignment.Vertical = StyleVerticalAlignment.Center;
                s169.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s169.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s169.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s169.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s170
                // -----------------------------------------------
                WorksheetStyle s170 = styles.Add("s170");
                s170.Font.FontName = "Calibri";
                s170.Font.Size = 11;
                s170.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s170.Alignment.Vertical = StyleVerticalAlignment.Center;
                s170.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s170.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s170.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s170.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                s170.NumberFormat = "#,##0";
                // -----------------------------------------------
                //  s171
                // -----------------------------------------------
                WorksheetStyle s171 = styles.Add("s171");
                s171.Font.FontName = "Calibri";
                s171.Font.Size = 11;
                s171.Font.Color = "#000000";
                s171.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s171.Alignment.Vertical = StyleVerticalAlignment.Center;
                s171.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s171.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s171.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s171.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                s171.NumberFormat = "#,##0";
                // -----------------------------------------------
                //  s172
                // -----------------------------------------------
                WorksheetStyle s172 = styles.Add("s172");
                s172.Font.FontName = "Calibri";
                s172.Font.Size = 11;
                s172.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                s172.Alignment.Vertical = StyleVerticalAlignment.Center;
                s172.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s172.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s172.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s172.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s173
                // -----------------------------------------------
                WorksheetStyle s173 = styles.Add("s173");
                s173.Font.FontName = "Calibri";
                s173.Font.Size = 11;
                s173.Font.Color = "#000000";
                s173.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s173.Alignment.Vertical = StyleVerticalAlignment.Center;
                s173.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s173.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s173.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s173.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                s173.NumberFormat = "#,##0;\\-#,##0";
                // -----------------------------------------------
                //  s174
                // -----------------------------------------------
                WorksheetStyle s174 = styles.Add("s174");
                s174.Font.FontName = "Calibri";
                s174.Font.Size = 12;
                s174.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                s174.Alignment.Vertical = StyleVerticalAlignment.Center;
                s174.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s174.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s174.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                WorksheetStyleBorder s174Border3 = s174.Borders.Add();
                s174Border3.Position = StylePosition.Top;
                s174Border3.LineStyle = LineStyleOption.Continuous;


                WorksheetStyle s1741 = styles.Add("s1741");
                s1741.Font.FontName = "Calibri";
                s1741.Font.Size = 12;
                s1741.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s1741.Alignment.Vertical = StyleVerticalAlignment.Center;
                s1741.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s1741.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s1741.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                WorksheetStyleBorder s174Border31 = s1741.Borders.Add();
                s174Border31.Position = StylePosition.Top;
                s174Border31.LineStyle = LineStyleOption.Continuous;
                // -----------------------------------------------
                //  s175
                // -----------------------------------------------
                WorksheetStyle s175 = styles.Add("s175");
                s175.Font.Bold = true;
                s175.Font.FontName = "Calibri";
                s175.Font.Size = 11;
                s175.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s175.Alignment.Vertical = StyleVerticalAlignment.Center;
                s175.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s175.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s175.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s175.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                s175.NumberFormat = "#,##0;\\-#,##0";
                // -----------------------------------------------
                //  s176
                // -----------------------------------------------
                WorksheetStyle s176 = styles.Add("s176");
                s176.Font.FontName = "Calibri";
                s176.Font.Size = 12;
                s176.Font.Color = "#000000";
                s176.Interior.Color = "#FFFFFF";
                s176.Interior.Pattern = StyleInteriorPattern.Solid;
                s176.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                s176.Alignment.Vertical = StyleVerticalAlignment.Center;
                s176.Alignment.WrapText = true;
                s176.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s176.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s176.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s176.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s177
                // -----------------------------------------------
                WorksheetStyle s177 = styles.Add("s177");
                s177.Font.Bold = true;
                s177.Font.FontName = "Calibri";
                s177.Font.Size = 12;
                s177.Font.Color = "#000000";
                s177.Interior.Color = "#BFBFBF";
                s177.Interior.Pattern = StyleInteriorPattern.Solid;
                s177.Alignment.Vertical = StyleVerticalAlignment.Center;
                s177.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s177.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s178
                // -----------------------------------------------
                WorksheetStyle s178 = styles.Add("s178");
                s178.Font.Bold = true;
                s178.Font.FontName = "Calibri";
                s178.Font.Size = 12;
                s178.Font.Color = "#000000";
                s178.Interior.Color = "#BFBFBF";
                s178.Interior.Pattern = StyleInteriorPattern.Solid;
                s178.Alignment.Vertical = StyleVerticalAlignment.Center;
                s178.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s179
                // -----------------------------------------------
                WorksheetStyle s179 = styles.Add("s179");
                s179.Font.FontName = "Calibri";
                s179.Font.Size = 12;
                s179.Font.Color = "#000000";
                s179.Interior.Color = "#BFBFBF";
                s179.Interior.Pattern = StyleInteriorPattern.Solid;
                s179.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                s179.Alignment.Vertical = StyleVerticalAlignment.Center;
                s179.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s179.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s180
                // -----------------------------------------------
                WorksheetStyle s180 = styles.Add("s180");
                s180.Font.FontName = "Calibri";
                s180.Font.Size = 12;
                s180.Font.Color = "#000000";
                s180.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                s180.Alignment.Vertical = StyleVerticalAlignment.Center;
                // -----------------------------------------------
                //  s181
                // -----------------------------------------------
                WorksheetStyle s181 = styles.Add("s181");
                s181.Font.Bold = true;
                s181.Font.FontName = "Calibri";
                s181.Font.Size = 12;
                s181.Interior.Color = "#FFFFFF";
                s181.Interior.Pattern = StyleInteriorPattern.Solid;
                s181.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                s181.Alignment.Vertical = StyleVerticalAlignment.Center;
                s181.Alignment.WrapText = true;
                s181.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s181.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s181.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s181.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s182
                // -----------------------------------------------
                WorksheetStyle s182 = styles.Add("s182");
                s182.Font.Bold = true;
                s182.Font.FontName = "Calibri";
                s182.Font.Size = 12;
                s182.Font.Color = "#000000";
                s182.Interior.Color = "#BFBFBF";
                s182.Interior.Pattern = StyleInteriorPattern.Solid;
                s182.Alignment.Vertical = StyleVerticalAlignment.Center;
                s182.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s182.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s183
                // -----------------------------------------------
                WorksheetStyle s183 = styles.Add("s183");
                s183.Font.Bold = true;
                s183.Font.FontName = "Calibri";
                s183.Font.Size = 12;
                s183.Font.Color = "#000000";
                s183.Interior.Color = "#BFBFBF";
                s183.Interior.Pattern = StyleInteriorPattern.Solid;
                s183.Alignment.Vertical = StyleVerticalAlignment.Center;
                s183.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s184
                // -----------------------------------------------
                WorksheetStyle s184 = styles.Add("s184");
                s184.Font.FontName = "Calibri";
                s184.Font.Size = 12;
                s184.Font.Color = "#000000";
                s184.Interior.Color = "#BFBFBF";
                s184.Interior.Pattern = StyleInteriorPattern.Solid;
                s184.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                s184.Alignment.Vertical = StyleVerticalAlignment.Center;
                s184.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s184.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s185
                // -----------------------------------------------
                WorksheetStyle s185 = styles.Add("s185");
                s185.Font.Bold = true;
                s185.Font.FontName = "Calibri";
                s185.Font.Size = 12;
                s185.Interior.Color = "#BFBFBF";
                s185.Interior.Pattern = StyleInteriorPattern.Solid;
                s185.Alignment.Vertical = StyleVerticalAlignment.Center;
                s185.Alignment.WrapText = true;
                s185.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s185.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s186
                // -----------------------------------------------
                WorksheetStyle s186 = styles.Add("s186");
                s186.Font.Bold = true;
                s186.Font.FontName = "Calibri";
                s186.Font.Size = 12;
                s186.Interior.Color = "#BFBFBF";
                s186.Interior.Pattern = StyleInteriorPattern.Solid;
                s186.Alignment.Vertical = StyleVerticalAlignment.Center;
                s186.Alignment.WrapText = true;
                s186.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s187
                // -----------------------------------------------
                WorksheetStyle s187 = styles.Add("s187");
                s187.Font.FontName = "Calibri";
                s187.Font.Size = 12;
                s187.Font.Color = "#000000";
                s187.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                s187.Alignment.Vertical = StyleVerticalAlignment.Center;
                // -----------------------------------------------
                //  s188
                // -----------------------------------------------
                WorksheetStyle s188 = styles.Add("s188");
                s188.Font.Bold = true;
                s188.Font.FontName = "Calibri";
                s188.Font.Size = 12;
                s188.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                s188.Alignment.Vertical = StyleVerticalAlignment.Center;
                s188.Alignment.WrapText = true;
                s188.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s188.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s188.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s188.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s189
                // -----------------------------------------------
                WorksheetStyle s189 = styles.Add("s189");
                s189.Font.Bold = true;
                s189.Font.FontName = "Calibri";
                s189.Font.Size = 12;
                s189.Font.Color = "#000000";
                s189.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                s189.Alignment.Vertical = StyleVerticalAlignment.Center;
                s189.Alignment.WrapText = true;
                s189.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s189.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s189.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s189.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s190
                // -----------------------------------------------
                WorksheetStyle s190 = styles.Add("s190");
                s190.Font.FontName = "Calibri";
                s190.Font.Size = 12;
                s190.Font.Color = "#000000";
                s190.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                s190.Alignment.Vertical = StyleVerticalAlignment.Center;
                s190.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s190.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s190.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s380
                // -----------------------------------------------
                WorksheetStyle s380 = styles.Add("s380");
                s380.Font.Bold = true;
                s380.Font.FontName = "Calibri";
                s380.Font.Size = 12;
                s380.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                s380.Alignment.Vertical = StyleVerticalAlignment.Center;
                s380.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s380.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s380.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s380.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s457
                // -----------------------------------------------
                WorksheetStyle s457 = styles.Add("s457");
                s457.Font.Bold = true;
                s457.Font.FontName = "Calibri";
                s457.Font.Size = 12;
                s457.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s457.Alignment.Vertical = StyleVerticalAlignment.Top;
                s457.Alignment.WrapText = true;
                s457.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s457.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s457.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s457.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s467
                // -----------------------------------------------
                WorksheetStyle s467 = styles.Add("s467");
                s467.Font.Bold = true;
                s467.Font.FontName = "Calibri";
                s467.Font.Size = 12;
                s467.Interior.Color = "#BFBFBF";
                s467.Interior.Pattern = StyleInteriorPattern.Solid;
                s467.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s467.Alignment.Vertical = StyleVerticalAlignment.Top;
                s467.Alignment.WrapText = true;
                s467.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s467.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s467.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s467.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s468
                // -----------------------------------------------
                WorksheetStyle s468 = styles.Add("s468");
                s468.Font.Bold = true;
                s468.Font.FontName = "Calibri";
                s468.Font.Size = 12;
                s468.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                s468.Alignment.Vertical = StyleVerticalAlignment.Top;
                s468.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s468.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s468.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s468.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s469
                // -----------------------------------------------
                WorksheetStyle s469 = styles.Add("s469");
                s469.Font.FontName = "Calibri";
                s469.Font.Size = 11;
                s469.Interior.Color = "#BFBFBF";
                s469.Interior.Pattern = StyleInteriorPattern.Solid;
                s469.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s469.Alignment.Vertical = StyleVerticalAlignment.Top;
                s469.Alignment.WrapText = true;
                s469.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s469.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s469.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s469.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s483
                // -----------------------------------------------
                WorksheetStyle s483 = styles.Add("s483");
                s483.Font.FontName = "Calibri";
                s483.Font.Size = 12;
                s483.Interior.Color = "#FF0000";
                s483.Interior.Pattern = StyleInteriorPattern.Solid;
                s483.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                s483.Alignment.Vertical = StyleVerticalAlignment.Top;
                s483.Alignment.WrapText = true;
                s483.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s483.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s483.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s483.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s484
                // -----------------------------------------------
                WorksheetStyle s484 = styles.Add("s484");
                s484.Font.Italic = true;
                s484.Font.FontName = "Calibri";
                s484.Font.Size = 11;
                s484.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                s484.Alignment.Vertical = StyleVerticalAlignment.Top;
                s484.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s484.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s484.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s484.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s488
                // -----------------------------------------------
                WorksheetStyle s488 = styles.Add("s488");
                s488.Font.Bold = true;
                s488.Font.FontName = "Calibri";
                s488.Font.Size = 8;
                s488.Interior.Color = "#FFFFFF";
                s488.Interior.Pattern = StyleInteriorPattern.Solid;
                s488.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                s488.Alignment.Vertical = StyleVerticalAlignment.Top;
                s488.Alignment.WrapText = true;
                s488.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s488.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s488.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s488.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s490
                // -----------------------------------------------
                WorksheetStyle s490 = styles.Add("s490");
                s490.Font.Bold = true;
                s490.Font.FontName = "Calibri";
                s490.Font.Size = 12;
                s490.Font.Color = "#000000";
                s490.Alignment.Vertical = StyleVerticalAlignment.Top;
                s490.Alignment.WrapText = true;
                s490.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s490.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s490.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s490.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s491
                // -----------------------------------------------
                WorksheetStyle s491 = styles.Add("s491");
                s491.Font.FontName = "Calibri";
                s491.Font.Size = 12;
                s491.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s491.Alignment.Vertical = StyleVerticalAlignment.Center;
                s491.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s491.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s491.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s491.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s492
                // -----------------------------------------------
                WorksheetStyle s492 = styles.Add("s492");
                s492.Font.Bold = true;
                s492.Font.FontName = "Calibri";
                s492.Font.Size = 12;
                s492.Font.Color = "#C00000";
                s492.Interior.Color = "#FFC000";
                s492.Interior.Pattern = StyleInteriorPattern.Solid;
                s492.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                s492.Alignment.Vertical = StyleVerticalAlignment.Top;
                s492.Alignment.WrapText = true;
                s492.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s492.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s492.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s492.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s493
                // -----------------------------------------------
                WorksheetStyle s493 = styles.Add("s493");
                s493.Font.Bold = true;
                s493.Font.FontName = "Calibri";
                s493.Font.Size = 12;
                s493.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                s493.Alignment.Vertical = StyleVerticalAlignment.Center;
                s493.Alignment.WrapText = true;
                s493.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s493.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s493.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s493.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s517
                // -----------------------------------------------
                WorksheetStyle s517 = styles.Add("s517");
                s517.Font.Bold = true;
                s517.Font.FontName = "Calibri";
                s517.Font.Size = 12;
                s517.Interior.Color = "#FF0000";
                s517.Interior.Pattern = StyleInteriorPattern.Solid;
                s517.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                s517.Alignment.Vertical = StyleVerticalAlignment.Top;
                s517.Alignment.WrapText = true;
                s517.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s517.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s517.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s517.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s546
                // -----------------------------------------------
                WorksheetStyle s546 = styles.Add("s546");
                s546.Font.Bold = true;
                s546.Font.FontName = "Calibri";
                s546.Font.Size = 18;
                s546.Interior.Color = "#FFFFFF";
                s546.Interior.Pattern = StyleInteriorPattern.Solid;
                s546.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s546.Alignment.Vertical = StyleVerticalAlignment.Top;
                s546.Alignment.WrapText = true;
                s546.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s546.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s546.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s546.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s548
                // -----------------------------------------------
                WorksheetStyle s548 = styles.Add("s548");
                s548.Font.FontName = "Calibri";
                s548.Font.Size = 12;
                s548.Interior.Color = "#BFBFBF";
                s548.Interior.Pattern = StyleInteriorPattern.Solid;
                s548.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s548.Alignment.Vertical = StyleVerticalAlignment.Top;
                s548.Alignment.WrapText = true;
                s548.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s548.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s548.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s548.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s550
                // -----------------------------------------------
                WorksheetStyle s550 = styles.Add("s550");
                s550.Font.FontName = "Calibri";
                s550.Font.Size = 12;
                s550.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s550.Alignment.Vertical = StyleVerticalAlignment.Top;
                s550.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s550.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s550.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s550.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s551
                // -----------------------------------------------
                WorksheetStyle s551 = styles.Add("s551");
                s551.Font.FontName = "Calibri";
                s551.Font.Size = 12;
                s551.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s551.Alignment.Vertical = StyleVerticalAlignment.Top;
                s551.Alignment.WrapText = true;
                s551.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s551.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s551.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s551.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s552
                // -----------------------------------------------
                WorksheetStyle s552 = styles.Add("s552");
                s552.Font.Bold = true;
                s552.Font.FontName = "Calibri";
                s552.Font.Size = 12;
                s552.Interior.Color = "#FFFFFF";
                s552.Interior.Pattern = StyleInteriorPattern.Solid;
                s552.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s552.Alignment.Vertical = StyleVerticalAlignment.Top;
                s552.Alignment.WrapText = true;
                s552.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s552.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s552.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s552.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s553
                // -----------------------------------------------
                WorksheetStyle s553 = styles.Add("s553");
                s553.Font.FontName = "Calibri";
                s553.Font.Size = 12;
                s553.Font.Color = "#000000";
                s553.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s553.Alignment.Vertical = StyleVerticalAlignment.Top;
                s553.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s553.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s553.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s553.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s554
                // -----------------------------------------------
                WorksheetStyle s554 = styles.Add("s554");
                s554.Font.Bold = true;
                s554.Font.FontName = "Calibri";
                s554.Font.Size = 12;
                s554.Font.Color = "#000000";
                s554.Interior.Color = "#BFBFBF";
                s554.Interior.Pattern = StyleInteriorPattern.Solid;
                s554.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s554.Alignment.Vertical = StyleVerticalAlignment.Top;
                s554.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s554.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s554.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s554.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s556
                // -----------------------------------------------
                WorksheetStyle s556 = styles.Add("s556");
                s556.Font.FontName = "Calibri";
                s556.Font.Size = 12;
                s556.Font.Color = "#C00000";
                s556.Interior.Color = "#D9D9D9";
                s556.Interior.Pattern = StyleInteriorPattern.Solid;
                s556.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s556.Alignment.Vertical = StyleVerticalAlignment.Center;
                s556.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s556.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s556.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s558
                // -----------------------------------------------
                WorksheetStyle s558 = styles.Add("s558");
                s558.Font.Bold = true;
                s558.Font.FontName = "Calibri";
                s558.Font.Size = 13;
                s558.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s558.Alignment.Vertical = StyleVerticalAlignment.Center;
                s558.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s558.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s558.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s560
                // -----------------------------------------------
                WorksheetStyle s560 = styles.Add("s560");
                s560.Font.Bold = true;
                s560.Font.FontName = "Calibri";
                s560.Font.Size = 18;
                s560.Font.Color = "#C00000";
                s560.Interior.Color = "#D9D9D9";
                s560.Interior.Pattern = StyleInteriorPattern.Solid;
                s560.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s560.Alignment.Vertical = StyleVerticalAlignment.Top;
                s560.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s560.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s562
                // -----------------------------------------------
                WorksheetStyle s562 = styles.Add("s562");
                s562.Font.FontName = "Calibri";
                s562.Font.Color = "#C00000";
                s562.Interior.Color = "#D9D9D9";
                s562.Interior.Pattern = StyleInteriorPattern.Solid;
                s562.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                s562.Alignment.Vertical = StyleVerticalAlignment.Top;
                // -----------------------------------------------
                //  s563
                // -----------------------------------------------
                WorksheetStyle s563 = styles.Add("s563");
                s563.Font.Bold = true;
                s563.Font.FontName = "Calibri";
                s563.Font.Size = 18;
                s563.Font.Color = "#C00000";
                s563.Interior.Color = "#D9D9D9";
                s563.Interior.Pattern = StyleInteriorPattern.Solid;
                s563.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s563.Alignment.Vertical = StyleVerticalAlignment.Center;
                s563.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s563.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s565
                // -----------------------------------------------
                WorksheetStyle s565 = styles.Add("s565");
                s565.Parent = "s18";
                s565.Font.FontName = "Calibri";
                s565.Font.Size = 12;
                s565.Interior.Color = "#D9D9D9";
                s565.Interior.Pattern = StyleInteriorPattern.Solid;
                s565.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s565.Alignment.Vertical = StyleVerticalAlignment.Center;
                s565.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s565.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
                // -----------------------------------------------
                //  s567
                // -----------------------------------------------
                WorksheetStyle s567 = styles.Add("s567");
                s567.Font.Bold = true;
                s567.Font.FontName = "Calibri";
                s567.Font.Size = 13;
                s567.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                s567.Alignment.Vertical = StyleVerticalAlignment.Center;
                s567.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
                s567.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
                s567.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
                s567.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);




                // added styles


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
                WorksheetStyle s169H = styles.Add("s169H");
                s169H.Parent = "s137";
                s169H.Font.FontName = "Arial";
                s169H.Font.Color = "#9400D3";
                s169H.Interior.Color = "#FFFFFF";
                s169H.Interior.Pattern = StyleInteriorPattern.Solid;
                s169H.Alignment.Vertical = StyleVerticalAlignment.Top;
                s169H.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
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
                //  s172
                // -----------------------------------------------
                WorksheetStyle s172H = styles.Add("s172H");
                s172H.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            }
            catch(Exception e) { }

        }

        private void GenerateWorksheetParameters(WorksheetCollection book)
        {
            try
            {
                Worksheet Psheet = book.Add("Parameters");
                Psheet.Table.DefaultRowHeight = 14.4F;

                WorksheetColumn columnHead = Psheet.Table.Columns.Add();
                columnHead.Index = 2;
                columnHead.Width = 5;
                Psheet.Table.Columns.Add(163);
                WorksheetColumn column2Head = Psheet.Table.Columns.Add();
                column2Head.Width = 332;
                column2Head.StyleID = "s172H";
                Psheet.Table.Columns.Add(59);
                // -----------------------------------------------
                WorksheetRow RowHead = Psheet.Table.Rows.Add();
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
                WorksheetRow Row1Head = Psheet.Table.Rows.Add();
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
                // -----------------------------------------------
                WorksheetRow Row2Head = Psheet.Table.Rows.Add();
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
                WorksheetRow Row7Head = Psheet.Table.Rows.Add();
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
                WorksheetRow Row101 = Psheet.Table.Rows.Add();
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
                WorksheetRow Row88Head = Psheet.Table.Rows.Add();
                Row88Head.Height = 14;
                Row88Head.AutoFitHeight = false;
                cell = Row88Head.Cells.Add();
                cell.StyleID = "s139";
                cell = Row88Head.Cells.Add();
                cell.StyleID = "s143";
                cell = Row88Head.Cells.Add();
                cell.StyleID = "s139";
                cell = Row88Head.Cells.Add();
                cell.StyleID = "s170H";
                cell = Row88Head.Cells.Add();
                cell.StyleID = "s145";
                // -----------------------------------------------
                WorksheetRow Row3Head = Psheet.Table.Rows.Add();
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
                WorksheetRow Row4Head = Psheet.Table.Rows.Add();
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
                WorksheetRow Row5Head = Psheet.Table.Rows.Add();
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
                WorksheetRow Row6Head = Psheet.Table.Rows.Add();
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
                string Header_year = string.Empty;
                if (CmbYear.Visible == true)
                    Header_year = "Year : " + ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString();

                WorksheetRow Row78Head = Psheet.Table.Rows.Add();
                Row78Head.Height = 12;
                Row78Head.AutoFitHeight = false;
                cell = Row78Head.Cells.Add();
                cell.StyleID = "s139";
                cell = Row78Head.Cells.Add();
                cell.StyleID = "s143";
                cell = Row78Head.Cells.Add();
                cell.StyleID = "m2611536909324";
                cell.Data.Type = DataType.String;
                cell.Data.Text = Txt_HieDesc.Text.Trim() + "     " + Header_year;
                cell.MergeAcross = 2;
                // -----------------------------------------------

                string Process = string.Empty;
                if (rbSummary.Checked == true) Process = rbSummary.Text.Trim();
                else if (rbDetail.Checked == true) Process = rbDetail.Text.Trim();

                string Report = string.Empty;
                if (rbLPMQ.Checked == true) Report = rbLPMQ.Text.Trim();
                else if (rbLIHWAP.Checked == true) Report = rbLIHWAP.Text.Trim();

                WorksheetRow Row8 = Psheet.Table.Rows.Add();
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

                Row7Head = Psheet.Table.Rows.Add();
                Row7Head.Height = 12;
                Row7Head.AutoFitHeight = false;
                cell = Row7Head.Cells.Add();
                cell.StyleID = "s139";
                cell = Row7Head.Cells.Add();
                cell.StyleID = "s143";
                cell = Row7Head.Cells.Add();
                cell.StyleID = "m2611536909324";
                cell.Data.Type = DataType.String;
                cell.Data.Text = "            Report : " + Report.Trim();
                cell.MergeAcross = 2;


                Row8 = Psheet.Table.Rows.Add();
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

                Row7Head = Psheet.Table.Rows.Add();
                Row7Head.Height = 12;
                Row7Head.AutoFitHeight = false;
                cell = Row7Head.Cells.Add();
                cell.StyleID = "s139";
                cell = Row7Head.Cells.Add();
                cell.StyleID = "s143";
                cell = Row7Head.Cells.Add();
                cell.StyleID = "m2611536909324";
                cell.Data.Type = DataType.String;
                cell.Data.Text = "            Report Type : " + Process.Trim();
                cell.MergeAcross = 2;


                Row8 = Psheet.Table.Rows.Add();
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

                string Heatsrc = string.Empty;
                if (rbDeliverables.Checked == true) Heatsrc = rbDeliverables.Text.Trim();
                else if (rbUtilities.Checked == true) Heatsrc = rbUtilities.Text.Trim();
                else if (rbHeatBoth.Checked == true) Heatsrc = rbHeatBoth.Text.Trim();
                if (rbLPMQ.Checked)
                {
                    Row7Head = Psheet.Table.Rows.Add();
                    Row7Head.Height = 12;
                    Row7Head.AutoFitHeight = false;
                    cell = Row7Head.Cells.Add();
                    cell.StyleID = "s139";
                    cell = Row7Head.Cells.Add();
                    cell.StyleID = "s143";
                    cell = Row7Head.Cells.Add();
                    cell.StyleID = "m2611536909324";
                    cell.Data.Type = DataType.String;
                    cell.Data.Text = "            " + "Heat Source" + " : " + Heatsrc.Trim();
                    cell.MergeAcross = 2;
                }
                Row8 = Psheet.Table.Rows.Add();
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
                WorksheetRow Row24 = Psheet.Table.Rows.Add();
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
                WorksheetRow Row25 = Psheet.Table.Rows.Add();
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
                WorksheetRow Row26Head = Psheet.Table.Rows.Add();
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
                WorksheetRow Row27Head = Psheet.Table.Rows.Add();
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
                WorksheetRow Row28 = Psheet.Table.Rows.Add();
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
                WorksheetRow Row29 = Psheet.Table.Rows.Add();
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
                WorksheetRow Row30 = Psheet.Table.Rows.Add();
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
                WorksheetRow Row31 = Psheet.Table.Rows.Add();
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
                Psheet.Options.Selected = true;
                Psheet.Options.ProtectObjects = false;
                Psheet.Options.ProtectScenarios = false;
                Psheet.Options.PageSetup.Header.Margin = 0.3F;
                Psheet.Options.PageSetup.Footer.Margin = 0.3F;
                Psheet.Options.PageSetup.PageMargins.Bottom = 0.75F;
                Psheet.Options.PageSetup.PageMargins.Left = 0.7F;
                Psheet.Options.PageSetup.PageMargins.Right = 0.7F;
                Psheet.Options.PageSetup.PageMargins.Top = 0.75F;
            }
            catch (Exception e) { }
        }

        private void OnExcel_Report1(List<LIHPMQuesEntity> lihpmQues, List<LPMQEntity> LPMList, List<CommonEntity> lihpResp, string pdfname, string Year)
        {
            //string propReportPath = _model.lookupDataAccess.GetReportPath(BaseForm.BaseAgency);
            //string PdfName = "Pdf File";
            PdfName = pdfname.Trim().Remove(pdfname.Trim().Length - 4);
            //PdfName = strFolderPath + PdfName;
            PdfName = PdfName + ".xls";

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
                string Tmpstr = PdfName;
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
                PdfName = PdfName;



            string data = null;
            Workbook book = new Workbook();

            this.GenerateStyles(book.Styles);
            GenerateWorksheetParameters(book.Worksheets);

            Worksheet sheet = book.Worksheets.Add("Data");
            if (rbSummary.Checked)
            {
                sheet.Table.Columns.Add(new WorksheetColumn(50));
                sheet.Table.Columns.Add(new WorksheetColumn(300));
                sheet.Table.Columns.Add(new WorksheetColumn(70));
                sheet.Table.Columns.Add(new WorksheetColumn(70));
                sheet.Table.Columns.Add(new WorksheetColumn(80));
                sheet.Table.Columns.Add(new WorksheetColumn(80));
            }
            else
            {
                sheet.Table.Columns.Add(new WorksheetColumn(50));
                sheet.Table.Columns.Add(new WorksheetColumn(70));
                sheet.Table.Columns.Add(new WorksheetColumn(300));
                sheet.Table.Columns.Add(new WorksheetColumn(70));
                sheet.Table.Columns.Add(new WorksheetColumn(70));
                sheet.Table.Columns.Add(new WorksheetColumn(80));
                sheet.Table.Columns.Add(new WorksheetColumn(80));
            }

            WorksheetStyle style = book.Styles.Add("HeaderStyle");
            style.Font.FontName = "Tahoma";
            style.Font.Size = 12;
            style.Font.Bold = true;
            style.Alignment.Horizontal = StyleHorizontalAlignment.Center;

            WorksheetStyle style1 = book.Styles.Add("Normal");
            style1.Font.FontName = "Tahoma";
            style1.Font.Size = 10;
            style1.Alignment.Horizontal = StyleHorizontalAlignment.Right;


            style = book.Styles.Add("Default");
            style.Font.FontName = "Tahoma";
            style.Font.Size = 10;

            try
            {

                var LihmpquesDesc = lihpmQues.Select(u => u.LPMQ_DESC.Trim()).Distinct().ToList();

                WorksheetRow row = sheet.Table.Rows.Add();

                WorksheetCell cell = row.Cells.Add("LIHEAP Performance Report", DataType.String, "HeaderStyle");
                //row.Cells.Add(new WorksheetCell("Matrix: " + ((ListItem)Cmb_Matrix.SelectedItem).Text.Trim(), "HeaderStyle"));
                cell.MergeAcross = 5;

                row = sheet.Table.Rows.Add();
                string[] HeaderSeq4 = { "S.No", "Question", "Yes", "No", "Not Applicable","Total"};
                for (int i = 0; i < HeaderSeq4.Length; ++i)
                {
                    if (i == 1 && rbSummary.Checked==false)
                    {
                        WorksheetCell cellH = row.Cells.Add("Question", DataType.String, "HeaderStyle");
                        cellH.MergeAcross = 1;
                    }
                    else
                        row.Cells.Add(new WorksheetCell(HeaderSeq4[i], "HeaderStyle"));
                }

                int j = 1;
                int GYes = 0, GNo = 0, GNA = 0, GT = 0;
                //foreach (var item in LihmpquesDesc)
                //{
                //    List<LIHPMQuesEntity> sellihpmQues = lihpmQues.FindAll(u => u.LPMQ_DESC.Trim().Equals(item));
                string QType = string.Empty;
                //    if (sellihpmQues.Count == 1) QType = sellihpmQues[0].LPMQ_QTYPE;

                    bool Isfirst = true;
                    LIHPMQuesEntity Entity = new LIHPMQuesEntity();//sellihpmQues[0];
                    foreach (LIHPMQuesEntity Selentity in lihpmQues)
                    {
                        



                        if (Year == "2015")
                        {
                            Selentity.LPMQ_QTYPE = string.Empty;
                            if (rbDeliverables.Checked) { if (int.Parse(Selentity.LPMQ_CODE) > 6) Entity = null; else Entity = Selentity; }
                            else if (rbUtilities.Checked) { if (Selentity.LPMQ_CODE == "0005" || Selentity.LPMQ_CODE == "0006") Entity = null; else Entity = Selentity; }
                            else if (rbHeatBoth.Checked) Entity = Selentity;
                        }
                        else if (!string.IsNullOrEmpty(Year.Trim())) //Year == "2016" || Year == "2017" || Year == "2018"
                    {
                            if (rbDeliverables.Checked) { if (Selentity.LPMQ_QTYPE != "N") Entity = Selentity; else Entity = null; }
                            else if (rbUtilities.Checked) { if (Selentity.LPMQ_QTYPE != "Y") Entity = Selentity; else Entity = null; }
                            else if (rbHeatBoth.Checked) Entity = Selentity;
                        }

                        if (!Isfirst)
                        {
                            if (QType != Entity.LPMQ_QTYPE)
                            {
                                row.Cells.Add("");
                                if (rbSummary.Checked)
                                    row.Cells.Add("Grand Total");
                                else
                                {
                                    WorksheetCell cellR = row.Cells.Add("Grand Total", DataType.String, "Default");
                                    cellR.MergeAcross = 1;
                                }
                                row.Cells.Add(GYes.ToString());
                                row.Cells.Add(GNo.ToString());
                                row.Cells.Add(GNA.ToString());
                                row.Cells.Add(GT.ToString());

                                GYes = 0; GNo = 0; GNA = 0; GT = 0; Isfirst = true;


                                row = sheet.Table.Rows.Add();
                                row = sheet.Table.Rows.Add();
                                row = sheet.Table.Rows.Add();

                                string[] HeaderSeq = { "S.No", "Question", "Yes", "No", "Not Applicable", "Total" };
                                for (int i = 0; i < HeaderSeq.Length; ++i)
                                {
                                    if (i == 1 && rbSummary.Checked == false)
                                    {
                                        WorksheetCell cellH = row.Cells.Add("Question", DataType.String, "HeaderStyle");
                                        cellH.MergeAcross = 1;
                                    }
                                    else
                                        row.Cells.Add(new WorksheetCell(HeaderSeq[i], "HeaderStyle"));
                                }


                            }
                        }

                    if ((!string.IsNullOrEmpty(Year.Trim())) && rbHeatBoth.Checked && Isfirst == true) //Year == "2016" || Year == "2017" || Year == "2018" commented by sudheer on 01/10/2020
                    {

                        string Desc = string.Empty;
                        if (Entity.LPMQ_QTYPE == "N") Desc = "Utilities"; else Desc = "Deliverables";

                        WorksheetCell cellR = row.Cells.Add(Desc, DataType.String, "HeaderStyle");
                        cellR.MergeAcross = 5;
                        QType = Entity.LPMQ_QTYPE; Isfirst = false;
                        row = sheet.Table.Rows.Add();
                    }

                    if (Entity != null)
                    {
                        row = sheet.Table.Rows.Add();

                        row.Cells.Add(int.Parse(Entity.LPMQ_CODE).ToString());
                        if (rbSummary.Checked)
                            row.Cells.Add(Entity.LPMQ_DESC.Trim());
                        else
                        {
                            WorksheetCell cellR = row.Cells.Add(Entity.LPMQ_DESC.Trim(), DataType.String, "Default");
                            cellR.MergeAcross = 1;
                        }


                        List<LPMQEntity> YesCount = new List<LPMQEntity>();
                        List<LPMQEntity> NoCount = new List<LPMQEntity>(); List<LPMQEntity> NACount = new List<LPMQEntity>();
                        List<LPMQEntity> SelList = new List<LPMQEntity>();
                        switch (Entity.LPMQ_CODE)
                        {
                            case "0001":
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0001.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0001.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0001.Equals("Y"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0001.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0001.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0001.Equals("N"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0001.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0001.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0001.Equals("U"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0001 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0001 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0001 != ""); break;
                            case "0002":
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0002.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0002.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0002.Equals("Y"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0002.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0002.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0002.Equals("N"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0002.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0002.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0002.Equals("U"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0002 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0002 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0002 != ""); break;
                            case "0003":
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0003.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0003.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0003.Equals("Y"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0003.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0003.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0003.Equals("N"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0003.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0003.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0003.Equals("U"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0003 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0003 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0003 != ""); break;
                            case "0004":
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0004.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0004.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0004.Equals("Y"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0004.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0004.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0004.Equals("N"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0004.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0004.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0004.Equals("U"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0004 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0004 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0004 != ""); break;
                            case "0005":
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0005.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0005.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0005.Equals("Y"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0005.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0005.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0005.Equals("N"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0005.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0005.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0005.Equals("U"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0005 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0005 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0005 != ""); break;
                            case "0006":
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0006.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0006.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0006.Equals("Y"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0006.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0006.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0006.Equals("N"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0006.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0006.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0006.Equals("U"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0006 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0006 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0006 != ""); break;
                            case "0007":
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0007.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0007.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0007.Equals("Y"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0007.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0007.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0007.Equals("N"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0007.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0007.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0007.Equals("U"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0007 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0007 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0007 != ""); break;
                            case "0008":
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0008.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0008.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0008.Equals("Y"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0008.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0008.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0008.Equals("N"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0008.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0008.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0008.Equals("U"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0008 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0008 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0008 != ""); break;
                            case "0009":
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0009.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0009.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0009.Equals("Y"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0009.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0009.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0009.Equals("N"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0009.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0009.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0009.Equals("U"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0009 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0009 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0009 != ""); break;
                            case "0010":
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0010.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0010.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0010.Equals("Y"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0010.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0010.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0010.Equals("N"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0010.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0010.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0010.Equals("U"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0010 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0010 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0010 != ""); break;
                            case "0011":
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0011.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0011.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0011.Equals("Y"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0011.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0011.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0011.Equals("N"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0011.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0011.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0011.Equals("U"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0011 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0011 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0011 != ""); break;
                            case "0012":
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0012.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0012.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0012.Equals("Y"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0012.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0012.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0012.Equals("N"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0012.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0012.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0012.Equals("U"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0012 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0012 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0012 != ""); break;
                            case "0013":
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0013.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0013.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0013.Equals("Y"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0013.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0013.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0013.Equals("N"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0013.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0013.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0013.Equals("U"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0013 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0013 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0011 != ""); break;
                            case "0014":
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0014.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0014.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0014.Equals("Y"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0014.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0014.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0014.Equals("N"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0014.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0014.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0014.Equals("U"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0014 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0014 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0014 != ""); break;
                            case "0015":
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0015.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0015.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0015.Equals("Y"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0015.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0015.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0015.Equals("N"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0015.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0015.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0015.Equals("U"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0015 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0015 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0015 != ""); break;
                            case "0016":
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0016.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0016.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0016.Equals("Y"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0016.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0016.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0016.Equals("N"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0016.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0016.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0016.Equals("U"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0016 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0016 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0016 != ""); break;
                            case "0017":
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0017.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0017.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0017.Equals("Y"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0017.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0017.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0017.Equals("N"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0017.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0017.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0017.Equals("U"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0017 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0017 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0017 != ""); break;
                        }



                        row.Cells.Add(YesCount.Count.ToString());
                        row.Cells.Add(NoCount.Count.ToString());
                        row.Cells.Add(NACount.Count.ToString());
                        row.Cells.Add((YesCount.Count + NoCount.Count + NACount.Count).ToString());

                        GYes += YesCount.Count; GNo += NoCount.Count; GNA += NACount.Count; GT += YesCount.Count + NoCount.Count + NACount.Count;

                        if (rbDetail.Checked)
                        {
                            if (SelList.Count > 0)
                            {
                                foreach (LPMQEntity LEntity in SelList)
                                {
                                    row = sheet.Table.Rows.Add();
                                    row.Cells.Add("");
                                    row.Cells.Add(LEntity.ApplNo.Trim());

                                    row.Cells.Add(LookupDataAccess.GetMemberName(LEntity.NameixFi, LEntity.NameixMi, LEntity.NameixLast, BaseForm.BaseHierarchyCnFormat.ToString()));

                                    switch (Entity.LPMQ_CODE)
                                    {
                                        case "0001":
                                            if (LEntity.LPM_0001 == ("Y")) row.Cells.Add(LEntity.LPM_0001.Trim()); else row.Cells.Add("");
                                            if (LEntity.LPM_0001 == ("N")) row.Cells.Add(LEntity.LPM_0001.Trim()); else row.Cells.Add("");
                                            if (LEntity.LPM_0001 == ("U")) row.Cells.Add(LEntity.LPM_0001.Trim()); else row.Cells.Add(""); break;
                                        case "0002":
                                            if (LEntity.LPM_0002 == ("Y")) row.Cells.Add(LEntity.LPM_0002.Trim()); else row.Cells.Add("");
                                            if (LEntity.LPM_0002 == ("N")) row.Cells.Add(LEntity.LPM_0002.Trim()); else row.Cells.Add("");
                                            if (LEntity.LPM_0002 == ("U")) row.Cells.Add(LEntity.LPM_0002.Trim()); else row.Cells.Add(""); break;
                                        case "0003":
                                            if (LEntity.LPM_0003 == ("Y")) row.Cells.Add(LEntity.LPM_0003.Trim()); else row.Cells.Add("");
                                            if (LEntity.LPM_0003 == ("N")) row.Cells.Add(LEntity.LPM_0003.Trim()); else row.Cells.Add("");
                                            if (LEntity.LPM_0003 == ("U")) row.Cells.Add(LEntity.LPM_0003.Trim()); else row.Cells.Add(""); break;
                                        case "0004":
                                            if (LEntity.LPM_0004 == ("Y")) row.Cells.Add(LEntity.LPM_0004.Trim()); else row.Cells.Add("");
                                            if (LEntity.LPM_0004 == ("N")) row.Cells.Add(LEntity.LPM_0004.Trim()); else row.Cells.Add("");
                                            if (LEntity.LPM_0004 == ("U")) row.Cells.Add(LEntity.LPM_0004.Trim()); else row.Cells.Add(""); break;
                                        case "0005":
                                            if (LEntity.LPM_0005 == ("Y")) row.Cells.Add(LEntity.LPM_0005.Trim()); else row.Cells.Add("");
                                            if (LEntity.LPM_0005 == ("N")) row.Cells.Add(LEntity.LPM_0005.Trim()); else row.Cells.Add("");
                                            if (LEntity.LPM_0005 == ("U")) row.Cells.Add(LEntity.LPM_0005.Trim()); else row.Cells.Add(""); break;
                                        case "0006":
                                            if (LEntity.LPM_0006 == ("Y")) row.Cells.Add(LEntity.LPM_0006.Trim()); else row.Cells.Add("");
                                            if (LEntity.LPM_0006 == ("N")) row.Cells.Add(LEntity.LPM_0006.Trim()); else row.Cells.Add("");
                                            if (LEntity.LPM_0006 == ("U")) row.Cells.Add(LEntity.LPM_0006.Trim()); else row.Cells.Add(""); break;
                                        case "0007":
                                            if (LEntity.LPM_0007 == ("Y")) row.Cells.Add(LEntity.LPM_0007.Trim()); else row.Cells.Add("");
                                            if (LEntity.LPM_0007 == ("N")) row.Cells.Add(LEntity.LPM_0007.Trim()); else row.Cells.Add("");
                                            if (LEntity.LPM_0007 == ("U")) row.Cells.Add(LEntity.LPM_0007.Trim()); else row.Cells.Add(""); break;
                                        case "0008":
                                            if (LEntity.LPM_0008 == ("Y")) row.Cells.Add(LEntity.LPM_0008.Trim()); else row.Cells.Add("");
                                            if (LEntity.LPM_0008 == ("N")) row.Cells.Add(LEntity.LPM_0008.Trim()); else row.Cells.Add("");
                                            if (LEntity.LPM_0008 == ("U")) row.Cells.Add(LEntity.LPM_0008.Trim()); else row.Cells.Add(""); break;
                                        case "0009":
                                            if (LEntity.LPM_0009 == ("Y")) row.Cells.Add(LEntity.LPM_0009.Trim()); else row.Cells.Add("");
                                            if (LEntity.LPM_0009 == ("N")) row.Cells.Add(LEntity.LPM_0009.Trim()); else row.Cells.Add("");
                                            if (LEntity.LPM_0009 == ("U")) row.Cells.Add(LEntity.LPM_0009.Trim()); else row.Cells.Add(""); break;
                                        case "0010":
                                            if (LEntity.LPM_0010 == ("Y")) row.Cells.Add(LEntity.LPM_0010.Trim()); else row.Cells.Add("");
                                            if (LEntity.LPM_0010 == ("N")) row.Cells.Add(LEntity.LPM_0010.Trim()); else row.Cells.Add("");
                                            if (LEntity.LPM_0010 == ("U")) row.Cells.Add(LEntity.LPM_0010.Trim()); else row.Cells.Add(""); break;
                                        case "0011":
                                            if (LEntity.LPM_0011 == ("Y")) row.Cells.Add(LEntity.LPM_0011.Trim()); else row.Cells.Add("");
                                            if (LEntity.LPM_0011 == ("N")) row.Cells.Add(LEntity.LPM_0011.Trim()); else row.Cells.Add("");
                                            if (LEntity.LPM_0011 == ("U")) row.Cells.Add(LEntity.LPM_0011.Trim()); else row.Cells.Add(""); break;
                                        case "0012":
                                            if (LEntity.LPM_0012 == ("Y")) row.Cells.Add(LEntity.LPM_0011.Trim()); else row.Cells.Add("");
                                            if (LEntity.LPM_0012 == ("N")) row.Cells.Add(LEntity.LPM_0011.Trim()); else row.Cells.Add("");
                                            if (LEntity.LPM_0012 == ("U")) row.Cells.Add(LEntity.LPM_0011.Trim()); else row.Cells.Add(""); break;
                                        case "0013":
                                            if (LEntity.LPM_0013 == ("Y")) row.Cells.Add(LEntity.LPM_0011.Trim()); else row.Cells.Add("");
                                            if (LEntity.LPM_0013 == ("N")) row.Cells.Add(LEntity.LPM_0011.Trim()); else row.Cells.Add("");
                                            if (LEntity.LPM_0013 == ("U")) row.Cells.Add(LEntity.LPM_0011.Trim()); else row.Cells.Add(""); break;
                                        case "0014":
                                            if (LEntity.LPM_0014 == ("Y")) row.Cells.Add(LEntity.LPM_0011.Trim()); else row.Cells.Add("");
                                            if (LEntity.LPM_0014 == ("N")) row.Cells.Add(LEntity.LPM_0011.Trim()); else row.Cells.Add("");
                                            if (LEntity.LPM_0014 == ("U")) row.Cells.Add(LEntity.LPM_0011.Trim()); else row.Cells.Add(""); break;
                                        case "0015":
                                            if (LEntity.LPM_0015 == ("Y")) row.Cells.Add(LEntity.LPM_0011.Trim()); else row.Cells.Add("");
                                            if (LEntity.LPM_0015 == ("N")) row.Cells.Add(LEntity.LPM_0011.Trim()); else row.Cells.Add("");
                                            if (LEntity.LPM_0015 == ("U")) row.Cells.Add(LEntity.LPM_0011.Trim()); else row.Cells.Add(""); break;
                                        case "0016":
                                            if (LEntity.LPM_0016 == ("Y")) row.Cells.Add(LEntity.LPM_0011.Trim()); else row.Cells.Add("");
                                            if (LEntity.LPM_0016 == ("N")) row.Cells.Add(LEntity.LPM_0011.Trim()); else row.Cells.Add("");
                                            if (LEntity.LPM_0016 == ("U")) row.Cells.Add(LEntity.LPM_0011.Trim()); else row.Cells.Add(""); break;
                                        case "0017":
                                            if (LEntity.LPM_0017 == ("Y")) row.Cells.Add(LEntity.LPM_0011.Trim()); else row.Cells.Add("");
                                            if (LEntity.LPM_0017 == ("N")) row.Cells.Add(LEntity.LPM_0011.Trim()); else row.Cells.Add("");
                                            if (LEntity.LPM_0017 == ("U")) row.Cells.Add(LEntity.LPM_0011.Trim()); else row.Cells.Add(""); break;
                                    }
                                }
                            }
                        }

                        j++;
                    }
                }

                row = sheet.Table.Rows.Add();
                row.Cells.Add("");
                if (rbSummary.Checked)
                    row.Cells.Add("Grand Total");
                else
                {
                    WorksheetCell cellR = row.Cells.Add("Grand Total", DataType.String, "Default");
                    cellR.MergeAcross = 1;
                }
                row.Cells.Add(GYes.ToString());
                row.Cells.Add(GNo.ToString());
                row.Cells.Add(GNA.ToString());
                row.Cells.Add(GT.ToString());

                FileStream stream = new FileStream(PdfName, FileMode.Create);

                book.Save(stream);
                stream.Close();
            }
            catch (Exception ex) { }


        }

        private void OnExcel_Report(List<LIHPMQuesEntity> lihpmQues, List<LPMQEntity> LPMList, List<CommonEntity> lihpResp, string pdfname, string Year)
        {
            //string propReportPath = _model.lookupDataAccess.GetReportPath(BaseForm.BaseAgency);
            //string PdfName = "Pdf File";
            PdfName = pdfname.Trim().Remove(pdfname.Trim().Length - 4);
            //PdfName = strFolderPath + PdfName;
            PdfName = PdfName + ".xls";

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
                string Tmpstr = PdfName;
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
                PdfName = PdfName;



            string data = null;
            Workbook book = new Workbook();

            this.GenerateStyles(book.Styles);
            GenerateWorksheetParameters(book.Worksheets);

            Worksheet sheet = book.Worksheets.Add("Data");
            //if (rbSummary.Checked)
            //{
            //    sheet.Table.Columns.Add(new WorksheetColumn(50));
            //    sheet.Table.Columns.Add(new WorksheetColumn(300));
            //    sheet.Table.Columns.Add(new WorksheetColumn(70));
            //    sheet.Table.Columns.Add(new WorksheetColumn(70));
            //    sheet.Table.Columns.Add(new WorksheetColumn(80));
            //    sheet.Table.Columns.Add(new WorksheetColumn(80));
            //}
            //else
            //{
            //    sheet.Table.Columns.Add(new WorksheetColumn(50));
            //    sheet.Table.Columns.Add(new WorksheetColumn(70));
            //    sheet.Table.Columns.Add(new WorksheetColumn(300));
            //    sheet.Table.Columns.Add(new WorksheetColumn(70));
            //    sheet.Table.Columns.Add(new WorksheetColumn(70));
            //    sheet.Table.Columns.Add(new WorksheetColumn(80));
            //    sheet.Table.Columns.Add(new WorksheetColumn(80));
            //}

            WorksheetStyle style = book.Styles.Add("HeaderStyle");
            style.Font.FontName = "Tahoma";
            style.Font.Size = 12;
            style.Font.Bold = true;
            style.Alignment.Horizontal = StyleHorizontalAlignment.Center;

            WorksheetStyle style1 = book.Styles.Add("Normal");
            style1.Font.FontName = "Tahoma";
            style1.Font.Size = 10;
            style1.Alignment.Horizontal = StyleHorizontalAlignment.Right;


            style = book.Styles.Add("Default");
            style.Font.FontName = "Tahoma";
            style.Font.Size = 10;

            try
            {

                List<LPMQEntity> SLPMList = LPMList.FindAll(u => u.Mst_Heating_Source != ""); //LPMList.OrderBy(u => u.Mst_Heating_Source.Trim()).ToList();
                SLPMList = SLPMList.OrderBy(u => u.Mst_Heating_Source.Trim()).ToList();
                var LPMS_Count = SLPMList.Select(u => u.Mst_Heating_Source.Trim()).Distinct().ToList();

                if (rbDeliverables.Checked || rbHeatBoth.Checked)
                {
                    sheet.Table.Columns.Add(new WorksheetColumn(50));
                    sheet.Table.Columns.Add(new WorksheetColumn(300));
                    switch (LPMS_Count.Count)
                    {
                        case 1: for (int i = 0; i < 3; i++) { sheet.Table.Columns.Add(new WorksheetColumn(70)); } break;
                        case 2: for (int i = 0; i < 6; i++) { sheet.Table.Columns.Add(new WorksheetColumn(70)); } break;
                        case 3: for (int i = 0; i < 9; i++) { sheet.Table.Columns.Add(new WorksheetColumn(70)); } break;
                        case 4: for (int i = 0; i < 12; i++) { sheet.Table.Columns.Add(new WorksheetColumn(70)); } break;
                        case 5: for (int i = 0; i < 15; i++) { sheet.Table.Columns.Add(new WorksheetColumn(70)); } break;
                        case 6: for (int i = 0; i < 18; i++) { sheet.Table.Columns.Add(new WorksheetColumn(70)); } break;
                        case 7: for (int i = 0; i < 21; i++) { sheet.Table.Columns.Add(new WorksheetColumn(70)); } break;
                        case 8: for (int i = 0; i < 24; i++) { sheet.Table.Columns.Add(new WorksheetColumn(70)); } break;
                        case 9: for (int i = 0; i < 27; i++) { sheet.Table.Columns.Add(new WorksheetColumn(70)); } break;
                        case 10: for (int i = 0; i < 30; i++) { sheet.Table.Columns.Add(new WorksheetColumn(70)); } break;
                        case 11: for (int i = 0; i < 33; i++) { sheet.Table.Columns.Add(new WorksheetColumn(70)); } break;
                        case 12: for (int i = 0; i < 36; i++) { sheet.Table.Columns.Add(new WorksheetColumn(70)); } break;
                    }
                    sheet.Table.Columns.Add(new WorksheetColumn(70));
                    sheet.Table.Columns.Add(new WorksheetColumn(70));
                    sheet.Table.Columns.Add(new WorksheetColumn(80));
                    sheet.Table.Columns.Add(new WorksheetColumn(80));
                }
                else
                {
                    //sheet.Table.Columns.Add(new WorksheetColumn(50));
                    sheet.Table.Columns.Add(new WorksheetColumn(50));
                    sheet.Table.Columns.Add(new WorksheetColumn(300));
                    sheet.Table.Columns.Add(new WorksheetColumn(70));
                    sheet.Table.Columns.Add(new WorksheetColumn(70));
                    sheet.Table.Columns.Add(new WorksheetColumn(70));
                    sheet.Table.Columns.Add(new WorksheetColumn(70));
                    sheet.Table.Columns.Add(new WorksheetColumn(70));
                    sheet.Table.Columns.Add(new WorksheetColumn(70));
                    sheet.Table.Columns.Add(new WorksheetColumn(70));
                    sheet.Table.Columns.Add(new WorksheetColumn(70));
                    sheet.Table.Columns.Add(new WorksheetColumn(80));
                    sheet.Table.Columns.Add(new WorksheetColumn(80));
                }
                        

                var LihmpquesDesc = lihpmQues.Select(u => u.LPMQ_DESC.Trim()).Distinct().ToList();

                List<CommonEntity> PrimarySourceHeat = _model.lookupDataAccess.GetAgyTabRecordsByCode(Consts.AgyTab.HEATSOURCE);
                if (!string.IsNullOrEmpty(Year.Trim())) //Year == "2016" || Year == "2017" || Year == "2018" commented by sudheer on 01/01/2020
                {
                    if (rbDeliverables.Checked) { lihpmQues = lihpmQues.FindAll(u => u.LPMQ_QTYPE != "N"); PrimarySourceHeat = PrimarySourceHeat.FindAll(u => u.Code != "02" && u.Code != "04"); }
                    else if (rbUtilities.Checked) { lihpmQues = lihpmQues.FindAll(u => u.LPMQ_QTYPE != "Y"); PrimarySourceHeat = PrimarySourceHeat.FindAll(u => u.Code.Equals("02") || u.Code.Equals("04")); PrimarySourceHeat = PrimarySourceHeat.OrderBy(u => u.Code).ToList(); }
                }

                WorksheetRow row = sheet.Table.Rows.Add();

                WorksheetCell cell = row.Cells.Add("LIHEAP Performance Report", DataType.String, "HeaderStyle");
                //row.Cells.Add(new WorksheetCell("Matrix: " + ((ListItem)Cmb_Matrix.SelectedItem).Text.Trim(), "HeaderStyle"));
                cell.MergeAcross = 11;

                row = sheet.Table.Rows.Add();
                if (rbDeliverables.Checked)
                {
                    string[] HeaderSeq4 = { "S.No", "Question" };
                    for (int i = 0; i < HeaderSeq4.Length; ++i)
                    {
                        if (i == 1 && rbSummary.Checked == false)
                        {
                            WorksheetCell cellH = row.Cells.Add("Question", DataType.String, "HeaderStyle");
                            cellH.MergeAcross = 1;
                        }
                        else
                        {
                          WorksheetCell cellH= row.Cells.Add(HeaderSeq4[i], DataType.String, "HeaderStyle");
                          cellH.MergeDown = 1;
                        }
                    }

                    if (LPMS_Count.Count > 0)
                    {

                        foreach (var item in LPMS_Count)
                        {
                            string Desc = PrimarySourceHeat.Find(u => u.Code.Equals(item)).Desc.Trim();

                            WorksheetCell c1= row.Cells.Add(Desc, DataType.String, "HeaderStyle");
                            c1.MergeAcross = 2;
                        }
                    }

                    string[] HeaderSeq = { "Yes", "No", "Not Applicable", "Total" };
                    for (int i = 0; i < HeaderSeq.Length; ++i)
                    {
                        WorksheetCell cellH = row.Cells.Add(HeaderSeq[i], DataType.String, "HeaderStyle");
                        cellH.MergeDown = 1;
                    }

                    if (LPMS_Count.Count > 0)
                    {
                        int i = 0;
                        row = sheet.Table.Rows.Add();
                        foreach (var item in LPMS_Count)
                        {
                            //string Desc = PrimarySourceHeat.Find(u => u.Code.Equals(item)).Desc.Trim();

                            WorksheetCell c1 = row.Cells.Add("Yes", DataType.String, "HeaderStyle");
                            if (i == 0)
                                c1.Index = 3;
                            row.Cells.Add("No", DataType.String, "HeaderStyle");
                            row.Cells.Add("NA", DataType.String, "HeaderStyle");

                            i++;
                        }
                    }

                }
                else
                {
                    string[] HeaderSeq4 = { "S.No", "Question", "Electric", "Natural Gas", "Yes", "No", "Not Applicable", "Total" };
                    for (int i = 0; i < HeaderSeq4.Length; ++i)
                    {
                        if (i == 1 && rbSummary.Checked == false)
                        {
                            WorksheetCell cellH = row.Cells.Add("Question", DataType.String, "HeaderStyle");
                            cellH.MergeAcross = 1;
                        }
                        else
                        {
                            if (HeaderSeq4[i] != "Electric" && HeaderSeq4[i] != "Natural Gas")
                            {
                                WorksheetCell cellH = row.Cells.Add(HeaderSeq4[i], DataType.String, "HeaderStyle");
                                cellH.MergeDown = 1;
                            }
                            else
                            {
                                WorksheetCell cellH = row.Cells.Add(HeaderSeq4[i], DataType.String, "HeaderStyle");
                                cellH.MergeAcross = 2;
                            }
                        }
                    }

                    row = sheet.Table.Rows.Add();
                    cell= row.Cells.Add("Yes", DataType.String, "HeaderStyle");
                    cell.Index = 3;
                    row.Cells.Add("No", DataType.String, "HeaderStyle");
                    row.Cells.Add("NA", DataType.String, "HeaderStyle");
                    row.Cells.Add("Yes", DataType.String, "HeaderStyle");
                    row.Cells.Add("No", DataType.String, "HeaderStyle");
                    row.Cells.Add("NA", DataType.String, "HeaderStyle");

                   
                }

                int j = 1;
                int GYes = 0, GNo = 0, GNA = 0, GT = 0;
                //foreach (var item in LihmpquesDesc)
                //{
                //    List<LIHPMQuesEntity> sellihpmQues = lihpmQues.FindAll(u => u.LPMQ_DESC.Trim().Equals(item));
                string QType = string.Empty; string LQType = string.Empty;
                //    if (sellihpmQues.Count == 1) QType = sellihpmQues[0].LPMQ_QTYPE;

                bool Isfirst = true; List<CommonEntity> CCountEntity = new List<CommonEntity>();
                LIHPMQuesEntity Entity = new LIHPMQuesEntity();//sellihpmQues[0];
                foreach (LIHPMQuesEntity Selentity in lihpmQues)
                {
                    LQType = Selentity.LPMQ_QTYPE;

                    if (Year == "2015")
                    {
                        Selentity.LPMQ_QTYPE = string.Empty;
                        if (rbDeliverables.Checked) { if (int.Parse(Selentity.LPMQ_CODE) > 6) Entity = null; else Entity = Selentity; }
                        else if (rbUtilities.Checked) { if (Selentity.LPMQ_CODE == "0005" || Selentity.LPMQ_CODE == "0006") Entity = null; else Entity = Selentity; }
                        else if (rbHeatBoth.Checked) Entity = Selentity;
                    }
                    else if (!string.IsNullOrEmpty(Year.Trim())) //Year == "2016" || Year == "2017" || Year == "2018" commented by sudheer on 01/01/2020
                    {
                        if (rbDeliverables.Checked) { if (Selentity.LPMQ_QTYPE != "N") Entity = Selentity; else Entity = null; }
                        else if (rbUtilities.Checked) { if (Selentity.LPMQ_QTYPE != "Y") Entity = Selentity; else Entity = null; }
                        else if (rbHeatBoth.Checked) Entity = Selentity;
                    }

                    if (!Isfirst)
                    {
                        if (QType != Entity.LPMQ_QTYPE)
                        {
                            row = sheet.Table.Rows.Add();
                            row.Cells.Add("");
                            if (rbSummary.Checked)
                                row.Cells.Add("Grand Total");
                            else
                            {
                                WorksheetCell cellR = row.Cells.Add("Grand Total", DataType.String, "Default");
                                cellR.MergeAcross = 1;
                            }

                            if (QType == "N")
                            {
                                List<CommonEntity> SelCCountEntity = CCountEntity.FindAll(u => u.Code.Equals("04"));
                                //int Count = SelCCountEntity.Sum(u => Convert.ToInt32(u.Hierarchy.Trim()));
                                int QYesCount = SelCCountEntity.Sum(u => Convert.ToInt32(u.Default.Trim()));
                                int QNoCount = SelCCountEntity.Sum(u => Convert.ToInt32(u.Active.Trim()));
                                int QNACount = SelCCountEntity.Sum(u => Convert.ToInt32(u.AgyCode.Trim()));

                                //row.Cells.Add(Count.ToString());
                                row.Cells.Add(QYesCount.ToString(), DataType.String, "Normal");
                                row.Cells.Add(QNoCount.ToString(), DataType.String, "Normal");
                                row.Cells.Add(QNACount.ToString(), DataType.String, "Normal");

                                SelCCountEntity = CCountEntity.FindAll(u => u.Code.Equals("02"));
                                //Count = SelCCountEntity.Sum(u => Convert.ToInt32(u.Hierarchy.Trim()));
                                QYesCount = SelCCountEntity.Sum(u => Convert.ToInt32(u.Default.Trim()));
                                QNoCount = SelCCountEntity.Sum(u => Convert.ToInt32(u.Active.Trim()));
                                QNACount = SelCCountEntity.Sum(u => Convert.ToInt32(u.AgyCode.Trim()));

                                //row.Cells.Add(Count.ToString());
                                row.Cells.Add(QYesCount.ToString(), DataType.String, "Normal");
                                row.Cells.Add(QNoCount.ToString(), DataType.String, "Normal");
                                row.Cells.Add(QNACount.ToString(), DataType.String, "Normal");
                            }
                            else
                            {
                                if (LPMS_Count.Count > 0)
                                {
                                    foreach (var item in LPMS_Count)
                                    {
                                        List<CommonEntity> SelCCountEntity = CCountEntity.FindAll(u => u.Code.Equals(item));
                                        //int Count = SelCCountEntity.Sum(u => Convert.ToInt32(u.Hierarchy.Trim()));
                                        int QYesCount = SelCCountEntity.Sum(u => Convert.ToInt32(u.Default.Trim()));
                                        int QNoCount = SelCCountEntity.Sum(u => Convert.ToInt32(u.Active.Trim()));
                                        int QNACount = SelCCountEntity.Sum(u => Convert.ToInt32(u.AgyCode.Trim()));

                                        //row.Cells.Add(Count.ToString());
                                        row.Cells.Add(QYesCount.ToString(), DataType.String, "Normal");
                                        row.Cells.Add(QNoCount.ToString(), DataType.String, "Normal");
                                        row.Cells.Add(QNACount.ToString(), DataType.String, "Normal");
                                    }
                                }
                            }

                            row.Cells.Add(GYes.ToString(), DataType.String, "Normal");
                            row.Cells.Add(GNo.ToString(), DataType.String, "Normal");
                            row.Cells.Add(GNA.ToString(), DataType.String, "Normal");
                            row.Cells.Add(GT.ToString(), DataType.String, "Normal");

                            GYes = 0; GNo = 0; GNA = 0; GT = 0; Isfirst = true;
                            CCountEntity = new List<CommonEntity>();

                            row = sheet.Table.Rows.Add();
                            row = sheet.Table.Rows.Add();
                            row = sheet.Table.Rows.Add();
                            row = sheet.Table.Rows.Add();

                            if (Entity.LPMQ_QTYPE == "Y")
                            {
                                List<LPMQEntity> SELPMList = LPMList.FindAll(u => u.Mst_Heating_Source != "" && u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04"); //LPMList.OrderBy(u => u.Mst_Heating_Source.Trim()).ToList();
                                SELPMList = SELPMList.OrderBy(u => u.Mst_Heating_Source.Trim()).ToList();
                                LPMS_Count = SELPMList.Select(u => u.Mst_Heating_Source.Trim()).Distinct().ToList();
                            }

                            string[] HeaderSeq4 = { "S.No", "Question" };
                            for (int i = 0; i < HeaderSeq4.Length; ++i)
                            {
                                if (i == 1 && rbSummary.Checked == false)
                                {
                                    WorksheetCell cellH = row.Cells.Add("Question", DataType.String, "HeaderStyle");
                                    cellH.MergeAcross = 1;
                                }
                                else
                                {
                                    WorksheetCell cellH = row.Cells.Add(HeaderSeq4[i], DataType.String, "HeaderStyle");
                                    cellH.MergeDown = 1;
                                }
                            }

                            if (LPMS_Count.Count > 0)
                            {

                                foreach (var item in LPMS_Count)
                                {
                                    string Desc = PrimarySourceHeat.Find(u => u.Code.Equals(item)).Desc.Trim();

                                    WorksheetCell c1 = row.Cells.Add(Desc, DataType.String, "HeaderStyle");
                                    c1.MergeAcross = 2;
                                }
                            }

                            string[] HeaderSeq = { "Yes", "No", "Not Applicable", "Total" };
                            for (int i = 0; i < HeaderSeq.Length; ++i)
                            {
                                WorksheetCell cellH = row.Cells.Add(HeaderSeq[i], DataType.String, "HeaderStyle");
                                cellH.MergeDown = 1;
                            }

                            if (LPMS_Count.Count > 0)
                            {
                                int i = 0;
                                row = sheet.Table.Rows.Add();
                                foreach (var item in LPMS_Count)
                                {
                                    //string Desc = PrimarySourceHeat.Find(u => u.Code.Equals(item)).Desc.Trim();

                                    WorksheetCell c1 = row.Cells.Add("Yes", DataType.String, "HeaderStyle");
                                    if (i == 0)
                                        c1.Index = 3;
                                    row.Cells.Add("No", DataType.String, "HeaderStyle");
                                    row.Cells.Add("NA", DataType.String, "HeaderStyle");

                                    i++;
                                }
                            }


                        }
                    }

                    if ((!string.IsNullOrEmpty(Year.Trim())) && rbHeatBoth.Checked && Isfirst == true) //Year == "2016" || Year == "2017" || Year == "2018"
                    {

                        string Desc = string.Empty;
                        if (Entity.LPMQ_QTYPE == "N") Desc = "Utilities"; else Desc = "Deliverables";

                        row = sheet.Table.Rows.Add();

                        WorksheetCell cellR = row.Cells.Add(Desc, DataType.String, "HeaderStyle");
                        cellR.MergeAcross = 11;
                        QType = Entity.LPMQ_QTYPE; Isfirst = false;
                        row = sheet.Table.Rows.Add();
                    }

                    if (Entity != null)
                    {
                        row = sheet.Table.Rows.Add();

                        row.Cells.Add(int.Parse(Entity.LPMQ_CODE).ToString());
                        if (rbSummary.Checked)
                            row.Cells.Add(Entity.LPMQ_DESC.Trim());
                        else
                        {
                            WorksheetCell cellR = row.Cells.Add(Entity.LPMQ_DESC.Trim(), DataType.String, "Default");
                            cellR.MergeAcross = 1;
                        }


                        List<LPMQEntity> YesCount = new List<LPMQEntity>();
                        List<LPMQEntity> NoCount = new List<LPMQEntity>(); List<LPMQEntity> NACount = new List<LPMQEntity>();
                        List<LPMQEntity> SelList = new List<LPMQEntity>();
                        switch (Entity.LPMQ_CODE)
                        {
                            case "0001": if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0001.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0001.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0001.Equals("Y"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0001.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0001.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0001.Equals("N"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0001.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0001.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0001.Equals("U"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0001 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0001 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0001 != "");
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim()))
                                {
                                    if (Entity.LPMQ_QTYPE == "N")
                                    {
                                        int Count = LPMList.FindAll(u => u.LPM_0001 != "" && (u.Mst_Heating_Source == "04")).Count;
                                        int QYesCount = LPMList.FindAll(u => u.LPM_0001 == "Y" && (u.Mst_Heating_Source == "04")).Count;
                                        int QNoCount = LPMList.FindAll(u => u.LPM_0001 == "N" && (u.Mst_Heating_Source == "04")).Count;
                                        int QNACount = LPMList.FindAll(u => u.LPM_0001 == "U" && (u.Mst_Heating_Source == "04")).Count;
                                        CCountEntity.Add(new CommonEntity("04", "Electric", Count.ToString(), Entity.LPMQ_CODE, QYesCount.ToString(), QNoCount.ToString(), QNACount.ToString()));

                                        Count = LPMList.FindAll(u => u.LPM_0001 != "" && (u.Mst_Heating_Source == "02")).Count;
                                        QYesCount = LPMList.FindAll(u => u.LPM_0001 == "Y" && (u.Mst_Heating_Source == "02")).Count;
                                        QNoCount = LPMList.FindAll(u => u.LPM_0001 == "N" && (u.Mst_Heating_Source == "02")).Count;
                                        QNACount = LPMList.FindAll(u => u.LPM_0001 == "U" && (u.Mst_Heating_Source == "02")).Count;
                                        CCountEntity.Add(new CommonEntity("02", "Natural Gas", Count.ToString(), Entity.LPMQ_CODE, QYesCount.ToString(), QNoCount.ToString(), QNACount.ToString()));
                                    }
                                    else
                                    {
                                        foreach (var item in LPMS_Count)
                                        {
                                            foreach (CommonEntity Agytab in PrimarySourceHeat)
                                            {
                                                if (item == Agytab.Code)
                                                {
                                                    int Count = LPMList.FindAll(u => u.LPM_0001 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04" && u.Mst_Heating_Source.Equals(Agytab.Code.Trim()))).Count;
                                                    int QYesCount = LPMList.FindAll(u => u.LPM_0001 == "Y" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04" && u.Mst_Heating_Source.Equals(Agytab.Code.Trim()))).Count;
                                                    int QNoCount = LPMList.FindAll(u => u.LPM_0001 == "N" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04" && u.Mst_Heating_Source.Equals(Agytab.Code.Trim()))).Count;
                                                    int QNACount = LPMList.FindAll(u => u.LPM_0001 == "U" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04" && u.Mst_Heating_Source.Equals(Agytab.Code.Trim()))).Count;
                                                    CCountEntity.Add(new CommonEntity(Agytab.Code, Agytab.Desc, Count.ToString(), Entity.LPMQ_CODE, QYesCount.ToString(), QNoCount.ToString(), QNACount.ToString()));
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                } break;
                            case "0002": if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0002.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0002.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0002.Equals("Y"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0002.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0002.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0002.Equals("N"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0002.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0002.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0002.Equals("U"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0002 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0002 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0002 != "");
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim()))
                                {
                                    if (Entity.LPMQ_QTYPE == "N")
                                    {
                                        int Count = LPMList.FindAll(u => u.LPM_0002 != "" && (u.Mst_Heating_Source == "04")).Count;
                                        int QYesCount = LPMList.FindAll(u => u.LPM_0002 == "Y" && (u.Mst_Heating_Source == "04")).Count;
                                        int QNoCount = LPMList.FindAll(u => u.LPM_0002 == "N" && (u.Mst_Heating_Source == "04")).Count;
                                        int QNACount = LPMList.FindAll(u => u.LPM_0002 == "U" && (u.Mst_Heating_Source == "04")).Count;
                                        CCountEntity.Add(new CommonEntity("04", "Electric", Count.ToString(), Entity.LPMQ_CODE, QYesCount.ToString(), QNoCount.ToString(), QNACount.ToString()));

                                        Count = LPMList.FindAll(u => u.LPM_0002 != "" && (u.Mst_Heating_Source == "02")).Count;
                                        QYesCount = LPMList.FindAll(u => u.LPM_0002 == "Y" && (u.Mst_Heating_Source == "02")).Count;
                                        QNoCount = LPMList.FindAll(u => u.LPM_0002 == "N" && (u.Mst_Heating_Source == "02")).Count;
                                        QNACount = LPMList.FindAll(u => u.LPM_0002 == "U" && (u.Mst_Heating_Source == "02")).Count;
                                        CCountEntity.Add(new CommonEntity("02", "Natural Gas", Count.ToString(), Entity.LPMQ_CODE, QYesCount.ToString(), QNoCount.ToString(), QNACount.ToString()));
                                    }
                                    else
                                    {
                                        foreach (var item in LPMS_Count)
                                        {
                                            foreach (CommonEntity Agytab in PrimarySourceHeat)
                                            {
                                                if (item == Agytab.Code)
                                                {
                                                    int Count = LPMList.FindAll(u => u.LPM_0002 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04" && u.Mst_Heating_Source.Equals(Agytab.Code.Trim()))).Count;
                                                    int QYesCount = LPMList.FindAll(u => u.LPM_0002 == "Y" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04" && u.Mst_Heating_Source.Equals(Agytab.Code.Trim()))).Count;
                                                    int QNoCount = LPMList.FindAll(u => u.LPM_0002 == "N" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04" && u.Mst_Heating_Source.Equals(Agytab.Code.Trim()))).Count;
                                                    int QNACount = LPMList.FindAll(u => u.LPM_0002 == "U" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04" && u.Mst_Heating_Source.Equals(Agytab.Code.Trim()))).Count;
                                                    CCountEntity.Add(new CommonEntity(Agytab.Code, Agytab.Desc, Count.ToString(), Entity.LPMQ_CODE, QYesCount.ToString(), QNoCount.ToString(), QNACount.ToString()));
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                } break;
                            case "0003": if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0003.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0003.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0003.Equals("Y"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0003.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0003.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0003.Equals("N"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0003.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0003.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0003.Equals("U"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0003 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0003 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0003 != "");
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim()))
                                {
                                    if (Entity.LPMQ_QTYPE == "N")
                                    {
                                        int Count = LPMList.FindAll(u => u.LPM_0003 != "" && (u.Mst_Heating_Source == "04")).Count;
                                        int QYesCount = LPMList.FindAll(u => u.LPM_0003 == "Y" && (u.Mst_Heating_Source == "04")).Count;
                                        int QNoCount = LPMList.FindAll(u => u.LPM_0003 == "N" && (u.Mst_Heating_Source == "04")).Count;
                                        int QNACount = LPMList.FindAll(u => u.LPM_0003 == "U" && (u.Mst_Heating_Source == "04")).Count;
                                        CCountEntity.Add(new CommonEntity("04", "Electric", Count.ToString(), Entity.LPMQ_CODE, QYesCount.ToString(), QNoCount.ToString(), QNACount.ToString()));

                                        Count = LPMList.FindAll(u => u.LPM_0003 != "" && (u.Mst_Heating_Source == "02")).Count;
                                        QYesCount = LPMList.FindAll(u => u.LPM_0003 == "Y" && (u.Mst_Heating_Source == "02")).Count;
                                        QNoCount = LPMList.FindAll(u => u.LPM_0003 == "N" && (u.Mst_Heating_Source == "02")).Count;
                                        QNACount = LPMList.FindAll(u => u.LPM_0003 == "U" && (u.Mst_Heating_Source == "02")).Count;
                                        CCountEntity.Add(new CommonEntity("02", "Natural Gas", Count.ToString(), Entity.LPMQ_CODE, QYesCount.ToString(), QNoCount.ToString(), QNACount.ToString()));
                                    }
                                    else
                                    {
                                        foreach (var item in LPMS_Count)
                                        {
                                            foreach (CommonEntity Agytab in PrimarySourceHeat)
                                            {
                                                if (item == Agytab.Code)
                                                {
                                                    int Count = LPMList.FindAll(u => u.LPM_0003 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04" && u.Mst_Heating_Source.Equals(Agytab.Code.Trim()))).Count;
                                                    int QYesCount = LPMList.FindAll(u => u.LPM_0003 == "Y" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04" && u.Mst_Heating_Source.Equals(Agytab.Code.Trim()))).Count;
                                                    int QNoCount = LPMList.FindAll(u => u.LPM_0003 == "N" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04" && u.Mst_Heating_Source.Equals(Agytab.Code.Trim()))).Count;
                                                    int QNACount = LPMList.FindAll(u => u.LPM_0003 == "U" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04" && u.Mst_Heating_Source.Equals(Agytab.Code.Trim()))).Count;
                                                    CCountEntity.Add(new CommonEntity(Agytab.Code, Agytab.Desc, Count.ToString(), Entity.LPMQ_CODE, QYesCount.ToString(), QNoCount.ToString(), QNACount.ToString()));
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                } break;
                            case "0004": if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0004.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0004.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0004.Equals("Y"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0004.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0004.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0004.Equals("N"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0004.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0004.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0004.Equals("U"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0004 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0004 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0004 != "");
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim()))
                                {
                                    if (Entity.LPMQ_QTYPE == "N")
                                    {
                                        int Count = LPMList.FindAll(u => u.LPM_0004 != "" && (u.Mst_Heating_Source == "04")).Count;
                                        int QYesCount = LPMList.FindAll(u => u.LPM_0004 == "Y" && (u.Mst_Heating_Source == "04")).Count;
                                        int QNoCount = LPMList.FindAll(u => u.LPM_0004 == "N" && (u.Mst_Heating_Source == "04")).Count;
                                        int QNACount = LPMList.FindAll(u => u.LPM_0004 == "U" && (u.Mst_Heating_Source == "04")).Count;
                                        CCountEntity.Add(new CommonEntity("04", "Electric", Count.ToString(), Entity.LPMQ_CODE, QYesCount.ToString(), QNoCount.ToString(), QNACount.ToString()));

                                        Count = LPMList.FindAll(u => u.LPM_0004 != "" && (u.Mst_Heating_Source == "02")).Count;
                                        QYesCount = LPMList.FindAll(u => u.LPM_0004 == "Y" && (u.Mst_Heating_Source == "02")).Count;
                                        QNoCount = LPMList.FindAll(u => u.LPM_0004 == "N" && (u.Mst_Heating_Source == "02")).Count;
                                        QNACount = LPMList.FindAll(u => u.LPM_0004 == "U" && (u.Mst_Heating_Source == "02")).Count;
                                        CCountEntity.Add(new CommonEntity("02", "Natural Gas", Count.ToString(), Entity.LPMQ_CODE, QYesCount.ToString(), QNoCount.ToString(), QNACount.ToString()));
                                    }
                                    else
                                    {
                                        foreach (var item in LPMS_Count)
                                        {
                                            foreach (CommonEntity Agytab in PrimarySourceHeat)
                                            {
                                                if (item == Agytab.Code)
                                                {
                                                    int Count = LPMList.FindAll(u => u.LPM_0004 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04" && u.Mst_Heating_Source.Equals(Agytab.Code.Trim()))).Count;
                                                    int QYesCount = LPMList.FindAll(u => u.LPM_0004 == "Y" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04" && u.Mst_Heating_Source.Equals(Agytab.Code.Trim()))).Count;
                                                    int QNoCount = LPMList.FindAll(u => u.LPM_0004 == "N" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04" && u.Mst_Heating_Source.Equals(Agytab.Code.Trim()))).Count;
                                                    int QNACount = LPMList.FindAll(u => u.LPM_0004 == "U" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04" && u.Mst_Heating_Source.Equals(Agytab.Code.Trim()))).Count;
                                                    CCountEntity.Add(new CommonEntity(Agytab.Code, Agytab.Desc, Count.ToString(), Entity.LPMQ_CODE, QYesCount.ToString(), QNoCount.ToString(), QNACount.ToString()));
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                } break;
                            case "0005": if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0005.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0005.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0005.Equals("Y"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0005.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0005.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0005.Equals("N"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0005.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0005.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0005.Equals("U"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0005 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0005 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0005 != "");
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim()))
                                {
                                    if (Entity.LPMQ_QTYPE == "N")
                                    {
                                        int Count = LPMList.FindAll(u => u.LPM_0005 != "" && (u.Mst_Heating_Source == "04")).Count;
                                        int QYesCount = LPMList.FindAll(u => u.LPM_0005 == "Y" && (u.Mst_Heating_Source == "04")).Count;
                                        int QNoCount = LPMList.FindAll(u => u.LPM_0005 == "N" && (u.Mst_Heating_Source == "04")).Count;
                                        int QNACount = LPMList.FindAll(u => u.LPM_0005 == "U" && (u.Mst_Heating_Source == "04")).Count;
                                        CCountEntity.Add(new CommonEntity("04", "Electric", Count.ToString(), Entity.LPMQ_CODE, QYesCount.ToString(), QNoCount.ToString(), QNACount.ToString()));

                                        Count = LPMList.FindAll(u => u.LPM_0005 != "" && (u.Mst_Heating_Source == "02")).Count;
                                        QYesCount = LPMList.FindAll(u => u.LPM_0005 == "Y" && (u.Mst_Heating_Source == "02")).Count;
                                        QNoCount = LPMList.FindAll(u => u.LPM_0005 == "N" && (u.Mst_Heating_Source == "02")).Count;
                                        QNACount = LPMList.FindAll(u => u.LPM_0005 == "U" && (u.Mst_Heating_Source == "02")).Count;
                                        CCountEntity.Add(new CommonEntity("02", "Natural Gas", Count.ToString(), Entity.LPMQ_CODE, QYesCount.ToString(), QNoCount.ToString(), QNACount.ToString()));
                                    }
                                    else
                                    {
                                        foreach (var item in LPMS_Count)
                                        {
                                            foreach (CommonEntity Agytab in PrimarySourceHeat)
                                            {
                                                if (item == Agytab.Code)
                                                {
                                                    int Count = LPMList.FindAll(u => u.LPM_0005 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04" && u.Mst_Heating_Source.Equals(Agytab.Code.Trim()))).Count;
                                                    int QYesCount = LPMList.FindAll(u => u.LPM_0005 == "Y" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04" && u.Mst_Heating_Source.Equals(Agytab.Code.Trim()))).Count;
                                                    int QNoCount = LPMList.FindAll(u => u.LPM_0005 == "N" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04" && u.Mst_Heating_Source.Equals(Agytab.Code.Trim()))).Count;
                                                    int QNACount = LPMList.FindAll(u => u.LPM_0005 == "U" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04" && u.Mst_Heating_Source.Equals(Agytab.Code.Trim()))).Count;
                                                    CCountEntity.Add(new CommonEntity(Agytab.Code, Agytab.Desc, Count.ToString(), Entity.LPMQ_CODE, QYesCount.ToString(), QNoCount.ToString(), QNACount.ToString()));
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                } break;
                            case "0006": if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0006.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0006.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0006.Equals("Y"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0006.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0006.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0006.Equals("N"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0006.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0006.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0006.Equals("U"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0006 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0006 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0006 != "");
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim()))
                                {
                                    if (Entity.LPMQ_QTYPE == "N")
                                    {
                                        int Count = LPMList.FindAll(u => u.LPM_0006 != "" && (u.Mst_Heating_Source == "04")).Count;
                                        int QYesCount = LPMList.FindAll(u => u.LPM_0006 == "Y" && (u.Mst_Heating_Source == "04")).Count;
                                        int QNoCount = LPMList.FindAll(u => u.LPM_0006 == "N" && (u.Mst_Heating_Source == "04")).Count;
                                        int QNACount = LPMList.FindAll(u => u.LPM_0006 == "U" && (u.Mst_Heating_Source == "04")).Count;
                                        CCountEntity.Add(new CommonEntity("04", "Electric", Count.ToString(), Entity.LPMQ_CODE, QYesCount.ToString(), QNoCount.ToString(), QNACount.ToString()));

                                        Count = LPMList.FindAll(u => u.LPM_0006 != "" && (u.Mst_Heating_Source == "02")).Count;
                                        QYesCount = LPMList.FindAll(u => u.LPM_0006 == "Y" && (u.Mst_Heating_Source == "02")).Count;
                                        QNoCount = LPMList.FindAll(u => u.LPM_0006 == "N" && (u.Mst_Heating_Source == "02")).Count;
                                        QNACount = LPMList.FindAll(u => u.LPM_0006 == "U" && (u.Mst_Heating_Source == "02")).Count;
                                        CCountEntity.Add(new CommonEntity("02", "Natural Gas", Count.ToString(), Entity.LPMQ_CODE, QYesCount.ToString(), QNoCount.ToString(), QNACount.ToString()));
                                    }
                                    else
                                    {
                                        foreach (var item in LPMS_Count)
                                        {
                                            foreach (CommonEntity Agytab in PrimarySourceHeat)
                                            {
                                                if (item == Agytab.Code)
                                                {
                                                    int Count = LPMList.FindAll(u => u.LPM_0006 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04" && u.Mst_Heating_Source.Equals(Agytab.Code.Trim()))).Count;
                                                    int QYesCount = LPMList.FindAll(u => u.LPM_0006 == "Y" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04" && u.Mst_Heating_Source.Equals(Agytab.Code.Trim()))).Count;
                                                    int QNoCount = LPMList.FindAll(u => u.LPM_0006 == "N" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04" && u.Mst_Heating_Source.Equals(Agytab.Code.Trim()))).Count;
                                                    int QNACount = LPMList.FindAll(u => u.LPM_0006 == "U" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04" && u.Mst_Heating_Source.Equals(Agytab.Code.Trim()))).Count;
                                                    CCountEntity.Add(new CommonEntity(Agytab.Code, Agytab.Desc, Count.ToString(), Entity.LPMQ_CODE, QYesCount.ToString(), QNoCount.ToString(), QNACount.ToString()));
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                } break;
                            case "0007": if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0007.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0007.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0007.Equals("Y"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0007.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0007.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0007.Equals("N"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0007.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0007.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0007.Equals("U"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0007 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0007 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0007 != "");
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim()))
                                {
                                    if (Entity.LPMQ_QTYPE == "N")
                                    {
                                        int Count = LPMList.FindAll(u => u.LPM_0007 != "" && (u.Mst_Heating_Source == "04")).Count;
                                        int QYesCount = LPMList.FindAll(u => u.LPM_0007 == "Y" && (u.Mst_Heating_Source == "04")).Count;
                                        int QNoCount = LPMList.FindAll(u => u.LPM_0007 == "N" && (u.Mst_Heating_Source == "04")).Count;
                                        int QNACount = LPMList.FindAll(u => u.LPM_0007 == "U" && (u.Mst_Heating_Source == "04")).Count;
                                        CCountEntity.Add(new CommonEntity("04", "Electric", Count.ToString(), Entity.LPMQ_CODE, QYesCount.ToString(), QNoCount.ToString(), QNACount.ToString()));

                                        Count = LPMList.FindAll(u => u.LPM_0007 != "" && (u.Mst_Heating_Source == "02")).Count;
                                        QYesCount = LPMList.FindAll(u => u.LPM_0007 == "Y" && (u.Mst_Heating_Source == "02")).Count;
                                        QNoCount = LPMList.FindAll(u => u.LPM_0007 == "N" && (u.Mst_Heating_Source == "02")).Count;
                                        QNACount = LPMList.FindAll(u => u.LPM_0007 == "U" && (u.Mst_Heating_Source == "02")).Count;
                                        CCountEntity.Add(new CommonEntity("02", "Natural Gas", Count.ToString(), Entity.LPMQ_CODE, QYesCount.ToString(), QNoCount.ToString(), QNACount.ToString()));
                                    }
                                    else
                                    {
                                        foreach (var item in LPMS_Count)
                                        {
                                            foreach (CommonEntity Agytab in PrimarySourceHeat)
                                            {
                                                if (item == Agytab.Code)
                                                {
                                                    int Count = LPMList.FindAll(u => u.LPM_0007 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04" && u.Mst_Heating_Source.Equals(Agytab.Code.Trim()))).Count;
                                                    int QYesCount = LPMList.FindAll(u => u.LPM_0007 == "Y" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04" && u.Mst_Heating_Source.Equals(Agytab.Code.Trim()))).Count;
                                                    int QNoCount = LPMList.FindAll(u => u.LPM_0007 == "N" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04" && u.Mst_Heating_Source.Equals(Agytab.Code.Trim()))).Count;
                                                    int QNACount = LPMList.FindAll(u => u.LPM_0007 == "U" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04" && u.Mst_Heating_Source.Equals(Agytab.Code.Trim()))).Count;
                                                    CCountEntity.Add(new CommonEntity(Agytab.Code, Agytab.Desc, Count.ToString(), Entity.LPMQ_CODE, QYesCount.ToString(), QNoCount.ToString(), QNACount.ToString()));
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                } break;
                            case "0008": if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0008.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0008.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0008.Equals("Y"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0008.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0008.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0008.Equals("N"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0008.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0008.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0008.Equals("U"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0008 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0008 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0008 != "");
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim()))
                                {
                                    if (Entity.LPMQ_QTYPE == "N")
                                    {
                                        int Count = LPMList.FindAll(u => u.LPM_0008 != "" && (u.Mst_Heating_Source == "04")).Count;
                                        int QYesCount = LPMList.FindAll(u => u.LPM_0008 == "Y" && (u.Mst_Heating_Source == "04")).Count;
                                        int QNoCount = LPMList.FindAll(u => u.LPM_0008 == "N" && (u.Mst_Heating_Source == "04")).Count;
                                        int QNACount = LPMList.FindAll(u => u.LPM_0008 == "U" && (u.Mst_Heating_Source == "04")).Count;
                                        CCountEntity.Add(new CommonEntity("04", "Electric", Count.ToString(), Entity.LPMQ_CODE, QYesCount.ToString(), QNoCount.ToString(), QNACount.ToString()));

                                        Count = LPMList.FindAll(u => u.LPM_0008 != "" && (u.Mst_Heating_Source == "02")).Count;
                                        QYesCount = LPMList.FindAll(u => u.LPM_0008 == "Y" && (u.Mst_Heating_Source == "02")).Count;
                                        QNoCount = LPMList.FindAll(u => u.LPM_0008 == "N" && (u.Mst_Heating_Source == "02")).Count;
                                        QNACount = LPMList.FindAll(u => u.LPM_0008 == "U" && (u.Mst_Heating_Source == "02")).Count;
                                        CCountEntity.Add(new CommonEntity("02", "Natural Gas", Count.ToString(), Entity.LPMQ_CODE, QYesCount.ToString(), QNoCount.ToString(), QNACount.ToString()));
                                    }
                                    else
                                    {
                                        foreach (var item in LPMS_Count)
                                        {
                                            foreach (CommonEntity Agytab in PrimarySourceHeat)
                                            {
                                                if (item == Agytab.Code)
                                                {
                                                    int Count = LPMList.FindAll(u => u.LPM_0008 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04" && u.Mst_Heating_Source.Equals(Agytab.Code.Trim()))).Count;
                                                    int QYesCount = LPMList.FindAll(u => u.LPM_0008 == "Y" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04" && u.Mst_Heating_Source.Equals(Agytab.Code.Trim()))).Count;
                                                    int QNoCount = LPMList.FindAll(u => u.LPM_0008 == "N" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04" && u.Mst_Heating_Source.Equals(Agytab.Code.Trim()))).Count;
                                                    int QNACount = LPMList.FindAll(u => u.LPM_0008 == "U" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04" && u.Mst_Heating_Source.Equals(Agytab.Code.Trim()))).Count;
                                                    CCountEntity.Add(new CommonEntity(Agytab.Code, Agytab.Desc, Count.ToString(), Entity.LPMQ_CODE, QYesCount.ToString(), QNoCount.ToString(), QNACount.ToString()));
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                } break;
                            case "0009": if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0009.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0009.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0009.Equals("Y"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0009.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0009.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0009.Equals("N"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0009.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0009.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0009.Equals("U"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0009 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0009 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0009 != "");
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim()))
                                {
                                    if (Entity.LPMQ_QTYPE == "N")
                                    {
                                        int Count = LPMList.FindAll(u => u.LPM_0009 != "" && (u.Mst_Heating_Source == "04")).Count;
                                        int QYesCount = LPMList.FindAll(u => u.LPM_0009 == "Y" && (u.Mst_Heating_Source == "04")).Count;
                                        int QNoCount = LPMList.FindAll(u => u.LPM_0009 == "N" && (u.Mst_Heating_Source == "04")).Count;
                                        int QNACount = LPMList.FindAll(u => u.LPM_0009 == "U" && (u.Mst_Heating_Source == "04")).Count;
                                        CCountEntity.Add(new CommonEntity("04", "Electric", Count.ToString(), Entity.LPMQ_CODE, QYesCount.ToString(), QNoCount.ToString(), QNACount.ToString()));

                                        Count = LPMList.FindAll(u => u.LPM_0009 != "" && (u.Mst_Heating_Source == "02")).Count;
                                        QYesCount = LPMList.FindAll(u => u.LPM_0009 == "Y" && (u.Mst_Heating_Source == "02")).Count;
                                        QNoCount = LPMList.FindAll(u => u.LPM_0009 == "N" && (u.Mst_Heating_Source == "02")).Count;
                                        QNACount = LPMList.FindAll(u => u.LPM_0009 == "U" && (u.Mst_Heating_Source == "02")).Count;
                                        CCountEntity.Add(new CommonEntity("02", "Natural Gas", Count.ToString(), Entity.LPMQ_CODE, QYesCount.ToString(), QNoCount.ToString(), QNACount.ToString()));
                                    }
                                    else
                                    {
                                        foreach (var item in LPMS_Count)
                                        {
                                            foreach (CommonEntity Agytab in PrimarySourceHeat)
                                            {
                                                if (item == Agytab.Code)
                                                {
                                                    int Count = LPMList.FindAll(u => u.LPM_0009 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04" && u.Mst_Heating_Source.Equals(Agytab.Code.Trim()))).Count;
                                                    int QYesCount = LPMList.FindAll(u => u.LPM_0009 == "Y" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04" && u.Mst_Heating_Source.Equals(Agytab.Code.Trim()))).Count;
                                                    int QNoCount = LPMList.FindAll(u => u.LPM_0009 == "N" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04" && u.Mst_Heating_Source.Equals(Agytab.Code.Trim()))).Count;
                                                    int QNACount = LPMList.FindAll(u => u.LPM_0009 == "U" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04" && u.Mst_Heating_Source.Equals(Agytab.Code.Trim()))).Count;
                                                    CCountEntity.Add(new CommonEntity(Agytab.Code, Agytab.Desc, Count.ToString(), Entity.LPMQ_CODE, QYesCount.ToString(), QNoCount.ToString(), QNACount.ToString()));
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                } break;
                            case "0010": if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0010.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0010.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0010.Equals("Y"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0010.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0010.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0010.Equals("N"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0010.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0010.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0010.Equals("U"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0010 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0010 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0010 != "");
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim()))
                                {
                                    if (Entity.LPMQ_QTYPE == "N")
                                    {
                                        int Count = LPMList.FindAll(u => u.LPM_0010 != "" && (u.Mst_Heating_Source == "04")).Count;
                                        int QYesCount = LPMList.FindAll(u => u.LPM_0010 == "Y" && (u.Mst_Heating_Source == "04")).Count;
                                        int QNoCount = LPMList.FindAll(u => u.LPM_0010 == "N" && (u.Mst_Heating_Source == "04")).Count;
                                        int QNACount = LPMList.FindAll(u => u.LPM_0010 == "U" && (u.Mst_Heating_Source == "04")).Count;
                                        CCountEntity.Add(new CommonEntity("04", "Electric", Count.ToString(), Entity.LPMQ_CODE, QYesCount.ToString(), QNoCount.ToString(), QNACount.ToString()));

                                        Count = LPMList.FindAll(u => u.LPM_0010 != "" && (u.Mst_Heating_Source == "02")).Count;
                                        QYesCount = LPMList.FindAll(u => u.LPM_0010 == "Y" && (u.Mst_Heating_Source == "02")).Count;
                                        QNoCount = LPMList.FindAll(u => u.LPM_0010 == "N" && (u.Mst_Heating_Source == "02")).Count;
                                        QNACount = LPMList.FindAll(u => u.LPM_0010 == "U" && (u.Mst_Heating_Source == "02")).Count;
                                        CCountEntity.Add(new CommonEntity("02", "Natural Gas", Count.ToString(), Entity.LPMQ_CODE, QYesCount.ToString(), QNoCount.ToString(), QNACount.ToString()));
                                    }
                                    else
                                    {
                                        foreach (var item in LPMS_Count)
                                        {
                                            foreach (CommonEntity Agytab in PrimarySourceHeat)
                                            {
                                                if (item == Agytab.Code)
                                                {
                                                    int Count = LPMList.FindAll(u => u.LPM_0010 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04" && u.Mst_Heating_Source.Equals(Agytab.Code.Trim()))).Count;
                                                    int QYesCount = LPMList.FindAll(u => u.LPM_0010 == "Y" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04" && u.Mst_Heating_Source.Equals(Agytab.Code.Trim()))).Count;
                                                    int QNoCount = LPMList.FindAll(u => u.LPM_0010 == "N" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04" && u.Mst_Heating_Source.Equals(Agytab.Code.Trim()))).Count;
                                                    int QNACount = LPMList.FindAll(u => u.LPM_0010 == "U" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04" && u.Mst_Heating_Source.Equals(Agytab.Code.Trim()))).Count;
                                                    CCountEntity.Add(new CommonEntity(Agytab.Code, Agytab.Desc, Count.ToString(), Entity.LPMQ_CODE, QYesCount.ToString(), QNoCount.ToString(), QNACount.ToString()));
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                } break;
                            case "0011": if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") YesCount = LPMList.FindAll(u => u.LPM_0011.Equals("Y") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else YesCount = LPMList.FindAll(u => u.LPM_0011.Equals("Y") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else YesCount = LPMList.FindAll(u => u.LPM_0011.Equals("Y"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NoCount = LPMList.FindAll(u => u.LPM_0011.Equals("N") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NoCount = LPMList.FindAll(u => u.LPM_0011.Equals("N") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NoCount = LPMList.FindAll(u => u.LPM_0011.Equals("N"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") NACount = LPMList.FindAll(u => u.LPM_0011.Equals("U") && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else NACount = LPMList.FindAll(u => u.LPM_0011.Equals("U") && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else NACount = LPMList.FindAll(u => u.LPM_0011.Equals("U"));
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim())) { if (Entity.LPMQ_QTYPE == "N") SelList = LPMList.FindAll(u => u.LPM_0011 != "" && (u.Mst_Heating_Source == "02" || u.Mst_Heating_Source == "04")); else SelList = LPMList.FindAll(u => u.LPM_0011 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04")); } else SelList = LPMList.FindAll(u => u.LPM_0011 != "");
                                if (!string.IsNullOrEmpty(Entity.LPMQ_QTYPE.Trim()))
                                {
                                    if (Entity.LPMQ_QTYPE == "N")
                                    {
                                        int Count = LPMList.FindAll(u => u.LPM_0011 != "" && (u.Mst_Heating_Source == "04")).Count;
                                        int QYesCount = LPMList.FindAll(u => u.LPM_0011 == "Y" && (u.Mst_Heating_Source == "04")).Count;
                                        int QNoCount = LPMList.FindAll(u => u.LPM_0011 == "N" && (u.Mst_Heating_Source == "04")).Count;
                                        int QNACount = LPMList.FindAll(u => u.LPM_0011 == "U" && (u.Mst_Heating_Source == "04")).Count;
                                        CCountEntity.Add(new CommonEntity("04", "Electric", Count.ToString(), Entity.LPMQ_CODE, QYesCount.ToString(), QNoCount.ToString(), QNACount.ToString()));

                                        Count = LPMList.FindAll(u => u.LPM_0011 != "" && (u.Mst_Heating_Source == "02")).Count;
                                        QYesCount = LPMList.FindAll(u => u.LPM_0011 == "Y" && (u.Mst_Heating_Source == "02")).Count;
                                        QNoCount = LPMList.FindAll(u => u.LPM_0011 == "N" && (u.Mst_Heating_Source == "02")).Count;
                                        QNACount = LPMList.FindAll(u => u.LPM_0011 == "U" && (u.Mst_Heating_Source == "02")).Count;
                                        CCountEntity.Add(new CommonEntity("02", "Natural Gas", Count.ToString(), Entity.LPMQ_CODE, QYesCount.ToString(), QNoCount.ToString(), QNACount.ToString()));
                                    }
                                    else
                                    {
                                        foreach (var item in LPMS_Count)
                                        {
                                            foreach (CommonEntity Agytab in PrimarySourceHeat)
                                            {
                                                if (item == Agytab.Code)
                                                {
                                                    int Count = LPMList.FindAll(u => u.LPM_0011 != "" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04" && u.Mst_Heating_Source.Equals(Agytab.Code.Trim()))).Count;
                                                    int QYesCount = LPMList.FindAll(u => u.LPM_0011 == "Y" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04" && u.Mst_Heating_Source.Equals(Agytab.Code.Trim()))).Count;
                                                    int QNoCount = LPMList.FindAll(u => u.LPM_0011 == "N" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04" && u.Mst_Heating_Source.Equals(Agytab.Code.Trim()))).Count;
                                                    int QNACount = LPMList.FindAll(u => u.LPM_0011 == "U" && (u.Mst_Heating_Source != "02" && u.Mst_Heating_Source != "04" && u.Mst_Heating_Source.Equals(Agytab.Code.Trim()))).Count;
                                                    CCountEntity.Add(new CommonEntity(Agytab.Code, Agytab.Desc, Count.ToString(), Entity.LPMQ_CODE, QYesCount.ToString(), QNoCount.ToString(), QNACount.ToString()));
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                } break;
                        }

                        if (Entity.LPMQ_QTYPE == "N")
                        {
                            if (CCountEntity.Count > 0)
                            {
                                foreach (CommonEntity CEntity in CCountEntity)
                                {
                                    if (CEntity.Code.ToString() == "04" && Entity.LPMQ_CODE == CEntity.Extension)
                                    {
                                        //row.Cells.Add(CEntity.Hierarchy.ToString());
                                        row.Cells.Add(CEntity.Default.ToString(), DataType.String, "Normal");
                                        row.Cells.Add(CEntity.Active.ToString(), DataType.String, "Normal");
                                        row.Cells.Add(CEntity.AgyCode.ToString(), DataType.String, "Normal");

                                    }
                                    else if (CEntity.Code.ToString() == "02" && Entity.LPMQ_CODE == CEntity.Extension)
                                    {
                                        //row.Cells.Add(CEntity.Hierarchy.ToString());
                                        row.Cells.Add(CEntity.Default.ToString(), DataType.String, "Normal");
                                        row.Cells.Add(CEntity.Active.ToString(), DataType.String, "Normal");
                                        row.Cells.Add(CEntity.AgyCode.ToString(), DataType.String, "Normal");
                                    }
                                }


                            }

                        }
                        else
                        {
                            if (CCountEntity.Count > 0)
                            {
                                List<CommonEntity> SelCCountEntity = CCountEntity.FindAll(u => u.Extension.Equals(Entity.LPMQ_CODE.Trim()));
                                SelCCountEntity = SelCCountEntity.OrderBy(u => u.Code).ToList();
                                foreach (CommonEntity CEntity in SelCCountEntity)
                                {
                                    //row.Cells.Add(CEntity.Hierarchy.ToString());
                                    row.Cells.Add(CEntity.Default.ToString(), DataType.String, "Normal");
                                    row.Cells.Add(CEntity.Active.ToString(), DataType.String, "Normal");
                                    row.Cells.Add(CEntity.AgyCode.ToString(), DataType.String, "Normal");

                                }
                            }
                        }


                        row.Cells.Add(YesCount.Count.ToString(), DataType.String, "Normal");
                        row.Cells.Add(NoCount.Count.ToString(), DataType.String, "Normal");
                        row.Cells.Add(NACount.Count.ToString(), DataType.String, "Normal");
                        row.Cells.Add((YesCount.Count + NoCount.Count + NACount.Count).ToString(), DataType.String, "Normal");

                        GYes += YesCount.Count; GNo += NoCount.Count; GNA += NACount.Count; GT += YesCount.Count + NoCount.Count + NACount.Count;

                        //if (rbDetail.Checked)
                        //{
                        //    if (SelList.Count > 0)
                        //    {
                        //        foreach (LPMQEntity LEntity in SelList)
                        //        {
                        //            row = sheet.Table.Rows.Add();
                        //            row.Cells.Add("");
                        //            row.Cells.Add(LEntity.ApplNo.Trim());

                        //            row.Cells.Add(LookupDataAccess.GetMemberName(LEntity.NameixFi, LEntity.NameixMi, LEntity.NameixLast, BaseForm.BaseHierarchyCnFormat.ToString()));

                        //            switch (Entity.LPMQ_CODE)
                        //            {
                        //                case "0001": if (LEntity.LPM_0001 == ("Y")) row.Cells.Add(LEntity.LPM_0001.Trim()); else row.Cells.Add("");
                        //                    if (LEntity.LPM_0001 == ("N")) row.Cells.Add(LEntity.LPM_0001.Trim()); else row.Cells.Add("");
                        //                    if (LEntity.LPM_0001 == ("U")) row.Cells.Add(LEntity.LPM_0001.Trim()); else row.Cells.Add(""); break;
                        //                case "0002": if (LEntity.LPM_0002 == ("Y")) row.Cells.Add(LEntity.LPM_0002.Trim()); else row.Cells.Add("");
                        //                    if (LEntity.LPM_0002 == ("N")) row.Cells.Add(LEntity.LPM_0002.Trim()); else row.Cells.Add("");
                        //                    if (LEntity.LPM_0002 == ("U")) row.Cells.Add(LEntity.LPM_0002.Trim()); else row.Cells.Add(""); break;
                        //                case "0003": if (LEntity.LPM_0003 == ("Y")) row.Cells.Add(LEntity.LPM_0003.Trim()); else row.Cells.Add("");
                        //                    if (LEntity.LPM_0003 == ("N")) row.Cells.Add(LEntity.LPM_0003.Trim()); else row.Cells.Add("");
                        //                    if (LEntity.LPM_0003 == ("U")) row.Cells.Add(LEntity.LPM_0003.Trim()); else row.Cells.Add(""); break;
                        //                case "0004": if (LEntity.LPM_0004 == ("Y")) row.Cells.Add(LEntity.LPM_0004.Trim()); else row.Cells.Add("");
                        //                    if (LEntity.LPM_0004 == ("N")) row.Cells.Add(LEntity.LPM_0004.Trim()); else row.Cells.Add("");
                        //                    if (LEntity.LPM_0004 == ("U")) row.Cells.Add(LEntity.LPM_0004.Trim()); else row.Cells.Add(""); break;
                        //                case "0005": if (LEntity.LPM_0005 == ("Y")) row.Cells.Add(LEntity.LPM_0005.Trim()); else row.Cells.Add("");
                        //                    if (LEntity.LPM_0005 == ("N")) row.Cells.Add(LEntity.LPM_0005.Trim()); else row.Cells.Add("");
                        //                    if (LEntity.LPM_0005 == ("U")) row.Cells.Add(LEntity.LPM_0005.Trim()); else row.Cells.Add(""); break;
                        //                case "0006": if (LEntity.LPM_0006 == ("Y")) row.Cells.Add(LEntity.LPM_0006.Trim()); else row.Cells.Add("");
                        //                    if (LEntity.LPM_0006 == ("N")) row.Cells.Add(LEntity.LPM_0006.Trim()); else row.Cells.Add("");
                        //                    if (LEntity.LPM_0006 == ("U")) row.Cells.Add(LEntity.LPM_0006.Trim()); else row.Cells.Add(""); break;
                        //                case "0007": if (LEntity.LPM_0007 == ("Y")) row.Cells.Add(LEntity.LPM_0007.Trim()); else row.Cells.Add("");
                        //                    if (LEntity.LPM_0007 == ("N")) row.Cells.Add(LEntity.LPM_0007.Trim()); else row.Cells.Add("");
                        //                    if (LEntity.LPM_0007 == ("U")) row.Cells.Add(LEntity.LPM_0007.Trim()); else row.Cells.Add(""); break;
                        //                case "0008": if (LEntity.LPM_0008 == ("Y")) row.Cells.Add(LEntity.LPM_0008.Trim()); else row.Cells.Add("");
                        //                    if (LEntity.LPM_0008 == ("N")) row.Cells.Add(LEntity.LPM_0008.Trim()); else row.Cells.Add("");
                        //                    if (LEntity.LPM_0008 == ("U")) row.Cells.Add(LEntity.LPM_0008.Trim()); else row.Cells.Add(""); break;
                        //                case "0009": if (LEntity.LPM_0009 == ("Y")) row.Cells.Add(LEntity.LPM_0009.Trim()); else row.Cells.Add("");
                        //                    if (LEntity.LPM_0009 == ("N")) row.Cells.Add(LEntity.LPM_0009.Trim()); else row.Cells.Add("");
                        //                    if (LEntity.LPM_0009 == ("U")) row.Cells.Add(LEntity.LPM_0009.Trim()); else row.Cells.Add(""); break;
                        //                case "0010": if (LEntity.LPM_0010 == ("Y")) row.Cells.Add(LEntity.LPM_0010.Trim()); else row.Cells.Add("");
                        //                    if (LEntity.LPM_0010 == ("N")) row.Cells.Add(LEntity.LPM_0010.Trim()); else row.Cells.Add("");
                        //                    if (LEntity.LPM_0010 == ("U")) row.Cells.Add(LEntity.LPM_0010.Trim()); else row.Cells.Add(""); break;
                        //                case "0011": if (LEntity.LPM_0011 == ("Y")) row.Cells.Add(LEntity.LPM_0011.Trim()); else row.Cells.Add("");
                        //                    if (LEntity.LPM_0011 == ("N")) row.Cells.Add(LEntity.LPM_0011.Trim()); else row.Cells.Add("");
                        //                    if (LEntity.LPM_0011 == ("U")) row.Cells.Add(LEntity.LPM_0011.Trim()); else row.Cells.Add(""); break;
                        //            }
                        //        }
                        //    }
                        //}

                        j++;
                    }
                }

                row = sheet.Table.Rows.Add();
                row.Cells.Add("");
                if (rbSummary.Checked)
                    row.Cells.Add("Grand Total");
                else
                {
                    WorksheetCell cellR = row.Cells.Add("Grand Total", DataType.String, "Default");
                    cellR.MergeAcross = 1;
                }
                if (LQType == "N")
                {

                    List<CommonEntity> SelCCountEntity = CCountEntity.FindAll(u => u.Code.Equals("04"));
                    int Count = SelCCountEntity.Sum(u => Convert.ToInt32(u.Hierarchy.Trim()));
                    int QYes = SelCCountEntity.Sum(u => Convert.ToInt32(u.Default.Trim()));
                    int QNo = SelCCountEntity.Sum(u => Convert.ToInt32(u.Active.Trim()));
                    int QNA = SelCCountEntity.Sum(u => Convert.ToInt32(u.AgyCode.Trim()));
                    //row.Cells.Add(Count.ToString());
                    row.Cells.Add(QYes.ToString(), DataType.String, "Normal");
                    row.Cells.Add(QNo.ToString(), DataType.String, "Normal");
                    row.Cells.Add(QNA.ToString(), DataType.String, "Normal");


                    SelCCountEntity = CCountEntity.FindAll(u => u.Code.Equals("02"));
                    Count = SelCCountEntity.Sum(u => Convert.ToInt32(u.Hierarchy.Trim()));
                    QYes = SelCCountEntity.Sum(u => Convert.ToInt32(u.Default.Trim()));
                    QNo = SelCCountEntity.Sum(u => Convert.ToInt32(u.Active.Trim()));
                    QNA = SelCCountEntity.Sum(u => Convert.ToInt32(u.AgyCode.Trim()));
                    //row.Cells.Add(Count.ToString());
                    row.Cells.Add(QYes.ToString(), DataType.String, "Normal");
                    row.Cells.Add(QNo.ToString(), DataType.String, "Normal");
                    row.Cells.Add(QNA.ToString(), DataType.String, "Normal");
                }
                else
                {
                    if (LPMS_Count.Count > 0)
                    {
                        foreach (var item in LPMS_Count)
                        {
                            List<CommonEntity> SelCCountEntity = CCountEntity.FindAll(u => u.Code.Equals(item));
                            int Count = SelCCountEntity.Sum(u => Convert.ToInt32(u.Hierarchy.Trim()));
                            int QYes = SelCCountEntity.Sum(u => Convert.ToInt32(u.Default.Trim()));
                            int QNo = SelCCountEntity.Sum(u => Convert.ToInt32(u.Active.Trim()));
                            int QNA = SelCCountEntity.Sum(u => Convert.ToInt32(u.AgyCode.Trim()));
                            //row.Cells.Add(Count.ToString());
                            row.Cells.Add(QYes.ToString(), DataType.String, "Normal");
                            row.Cells.Add(QNo.ToString(), DataType.String, "Normal");
                            row.Cells.Add(QNA.ToString(), DataType.String, "Normal");
                        }
                    }
                }

                row.Cells.Add(GYes.ToString(), DataType.String, "Normal");
                row.Cells.Add(GNo.ToString(), DataType.String, "Normal");
                row.Cells.Add(GNA.ToString(), DataType.String, "Normal");
                row.Cells.Add(GT.ToString(), DataType.String, "Normal");

                FileStream stream = new FileStream(PdfName, FileMode.Create);

                book.Save(stream);
                stream.Close();
            }
            catch (Exception ex) { }


        }
        string Header_year = string.Empty;
        private void PrintHeaderPage(Document document, PdfWriter writer)
        {
            /* BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/calibrib.ttf", BaseFont.WINANSI, BaseFont.EMBEDDED);
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
            float[] Headerwidths = new float[] { 20f, 110f };
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

            PdfPCell row1 = new PdfPCell(new Phrase(Privileges.Program + " - " + Privileges.PrivilegeName, TblHeaderTitleFont));
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

            PdfPCell row3 = new PdfPCell(new Phrase("Selected Report Parameters", TblParamsHeaderFont));
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

            string Agy = /*"Agency : */"All"; string Dept = /*"Dept : */"All"; string Prg = /*"Program : */"All"; //string Header_year = string.Empty;
            if (Agency != "**") Agy = /*"Agency : " +*/ Agency;
            if (Depart != "**") Dept = /*"Dept : " + */Depart;
            if (Program != "**") Prg = /*"Program : " +*/ Program;
            if (CmbYear.Visible == true)
                Header_year = "Year: " + ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString();

            if (CmbYear.Visible == true)
            {
                PdfPCell Hierarchy = new PdfPCell(new Phrase("  " + "Hierarchy", TableFont));
                Hierarchy.HorizontalAlignment = Element.ALIGN_LEFT;
                Hierarchy.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                Hierarchy.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                Hierarchy.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                Hierarchy.PaddingBottom = 5;
                Headertable.AddCell(Hierarchy);

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
                PdfPCell Hierarchy = new PdfPCell(new Phrase("  " + "Hierarchy", TableFont));
                Hierarchy.HorizontalAlignment = Element.ALIGN_LEFT;
                Hierarchy.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                Hierarchy.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                Hierarchy.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                Hierarchy.PaddingBottom = 5;
                Headertable.AddCell(Hierarchy);

                PdfPCell Hierarchy1 = new PdfPCell(new Phrase(Txt_HieDesc.Text.Trim(), TableFont));
                Hierarchy1.HorizontalAlignment = Element.ALIGN_LEFT;
                Hierarchy1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                Hierarchy1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                Hierarchy1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                Hierarchy1.PaddingBottom = 5;
                Headertable.AddCell(Hierarchy1);
            }

            string Process = string.Empty;
            if (rbSummary.Checked == true) Process = rbSummary.Text.Trim();
            else if (rbDetail.Checked == true) Process = rbDetail.Text.Trim();
            //else if (rbBoth.Checked == true) Process = rbBoth.Text.Trim();

            string Report = string.Empty;
            if (rbLPMQ.Checked == true) Report = rbLPMQ.Text.Trim();
            else if (rbLIHWAP.Checked == true) Report = rbLIHWAP.Text.Trim();

            PdfPCell R5 = new PdfPCell(new Phrase("  " + "Report" /*+ Report*/, TableFont));
            R5.HorizontalAlignment = Element.ALIGN_LEFT;
            R5.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R5.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R5.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R5.PaddingBottom = 5;
            Headertable.AddCell(R5);

            PdfPCell R55 = new PdfPCell(new Phrase(Report, TableFont));
            R55.HorizontalAlignment = Element.ALIGN_LEFT;
            R55.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R55.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R55.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R55.PaddingBottom = 5;
            Headertable.AddCell(R55);

            PdfPCell R3 = new PdfPCell(new Phrase("  " + "Report Type" /*+ Process.Trim()*/, TableFont));
            R3.HorizontalAlignment = Element.ALIGN_LEFT;
            R3.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R3.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R3.PaddingBottom = 5;
            Headertable.AddCell(R3);

            PdfPCell R33 = new PdfPCell(new Phrase(Process.Trim(), TableFont));
            R33.HorizontalAlignment = Element.ALIGN_LEFT;
            R33.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R33.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R33.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R33.PaddingBottom = 5;
            Headertable.AddCell(R33);

            string Heatsrc = string.Empty;
            if (rbDeliverables.Checked == true) Heatsrc = rbDeliverables.Text.Trim();
            else if (rbUtilities.Checked == true) Heatsrc = rbUtilities.Text.Trim();
            else if (rbHeatBoth.Checked == true) Heatsrc = rbHeatBoth.Text.Trim();

            if (rbLPMQ.Checked)
            { 
                PdfPCell R4 = new PdfPCell(new Phrase("  " + "Heat Source" /*+ Heatsrc.Trim()*/, TableFont));
                R4.HorizontalAlignment = Element.ALIGN_LEFT;
                R4.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                R4.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                R4.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                R4.PaddingBottom = 5;
                Headertable.AddCell(R4);

                PdfPCell R44 = new PdfPCell(new Phrase(Heatsrc.Trim(), TableFont));
                R44.HorizontalAlignment = Element.ALIGN_LEFT;
                R44.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                R44.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                R44.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                R44.PaddingBottom = 5;
                Headertable.AddCell(R44);
            }

            document.Add(Headertable);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase("Generated By : ", fnttimesRoman_Italic), 33, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.GetMemberName(BaseForm.UserProfile.FirstName.Trim(), BaseForm.UserProfile.MI.Trim(), BaseForm.UserProfile.LastName.Trim(), "3"), fnttimesRoman_Italic), 90, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase("Generated On : ", fnttimesRoman_Italic), 410, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase(DateTime.Now.ToString(), fnttimesRoman_Italic), 468, 40, 0);
        }

        private void On_LIHEAPW_Summary(object sender, FormClosedEventArgs e)
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
                iTextSharp.text.Font HeaderTblFontBold = new iTextSharp.text.Font(bf_times, 8, 1);
                iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 8, 2);
                iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 9, 4);
                cb = writer.DirectContent;

                if (Agency == "**") Agency = null; if (Depart == "**") Depart = null; if (Program == "**") Program = null;
                string Year = string.Empty; string AppNo = string.Empty; string HeatSource = string.Empty; string Seq = string.Empty;
                if (CmbYear.Visible == true)
                    Year = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString().Trim();

                if (rbDeliverables.Checked) HeatSource = "B1"; else if (rbUtilities.Checked) HeatSource = "U1";

                //List<LIHPMQuesEntity> lihpmQues = _model.ZipCodeAndAgency.GetLIHPMQuesData(string.Empty, Year);

                //var LihmpquesDesc = lihpmQues.Select(u => u.LPMQ_DESC.Trim()).Distinct().ToList();

                List<LPMQEntity> LPMList = _model.CaseMstData.GetLPMQ0001(Agency, Depart, Program, Year, string.Empty);

                //List<CommonEntity> lihpResp = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTabs.LIHPMQUES, Agency, Depart, Program, "View");
                try
                {
                    PrintHeaderPage(document, writer);
                    document.NewPage();

                    if (LPMList.Count > 0)
                    {
                        PdfPTable table = new PdfPTable(7);
                        table.TotalWidth = 550f;
                        table.WidthPercentage = 100;
                        table.LockedWidth = true;
                        float[] widths = new float[] {15f, 35f, 44f, 18f, 27f, 23f, 23f};// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                        table.SetWidths(widths);
                        table.HorizontalAlignment = Element.ALIGN_CENTER;
                        table.HeaderRows = 1;

                        string[] HeaderSeq4 = {"App #", "Name", "Address", "Phone", "E-Mail", "Priority Level - Drinking Water", "Priority Level - Sewer" };
                        for (int i = 0; i < HeaderSeq4.Length; ++i)
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(HeaderSeq4[i], TblFontBold));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.Border = iTextSharp.text.Rectangle.BOX;
                            table.AddCell(cell);
                        }

                        if(LPMList.Count>0)
                        {
                            bool IsPrint = false;
                            foreach(LPMQEntity LEntity in LPMList)
                            {
                                IsPrint = false;

                                if (LEntity.LPM_0012.Trim() == "Y" || LEntity.LPM_0015.Trim() == "Y")
                                    IsPrint = true;

                                if(IsPrint)
                                {
                                    PdfPCell L0 = new PdfPCell(new Phrase(LEntity.ApplNo.Trim(), TableFont));
                                    L0.HorizontalAlignment = Element.ALIGN_LEFT;
                                    L0.Border = iTextSharp.text.Rectangle.BOX;
                                    table.AddCell(L0);

                                    PdfPCell L1 = new PdfPCell(new Phrase(LEntity.NameixFi.Trim()+" "+LEntity.NameixLast.Trim(), TableFont));
                                    L1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    L1.Border = iTextSharp.text.Rectangle.BOX;
                                    table.AddCell(L1);

                                    string Address = string.Empty;
                                    string HN = string.Empty; string Street = string.Empty; string Aprt = string.Empty; string Floor = string.Empty;
                                    if (!string.IsNullOrEmpty(LEntity.Hn.Trim())) HN = LEntity.Hn.Trim() + " ";
                                    if (!string.IsNullOrEmpty(LEntity.Street.Trim())) Street = LEntity.Street.Trim() + " " + LEntity.Mst_Suffix.Trim();
                                    if (!string.IsNullOrEmpty(LEntity.Mst_Apt.Trim())) Aprt = " APT:" + LEntity.Mst_Apt.Trim();
                                    if (!string.IsNullOrEmpty(LEntity.MsT_Floor.Trim())) Floor = " FLR:" + LEntity.MsT_Floor.Trim();

                                    Address = HN + Street + Aprt + Floor + ", "; string Tmp_Zip = "00000";
                                    Tmp_Zip = LEntity.MsT_Zip.Trim();
                                    Tmp_Zip = "00000".Substring(0, 5 - Tmp_Zip.Length) + Tmp_Zip;

                                    Address= (Address + LEntity.City.Trim() + " " + LEntity.State.Trim() + "  " + Tmp_Zip);

                                    PdfPCell L2 = new PdfPCell(new Phrase(Address, TableFont));
                                    L2.HorizontalAlignment = Element.ALIGN_LEFT;
                                    L2.Border = iTextSharp.text.Rectangle.BOX;
                                    table.AddCell(L2);

                                    MaskedTextBox mskPhn = new MaskedTextBox();
                                    mskPhn.Mask = "(999) 000-0000";
                                    if (!string.IsNullOrEmpty(LEntity.Area.Trim()+LEntity.Phone.Trim()))
                                        mskPhn.Text = LEntity.Area.Trim() + LEntity.Phone.Trim();

                                    PdfPCell L3 = new PdfPCell(new Phrase(mskPhn.Text.Trim(), TableFont));
                                    L3.HorizontalAlignment = Element.ALIGN_LEFT;
                                    L3.Border = iTextSharp.text.Rectangle.BOX;
                                    table.AddCell(L3);

                                    PdfPCell L4 = new PdfPCell(new Phrase(LEntity.Mst_Email.Trim(), TableFont));
                                    L4.HorizontalAlignment = Element.ALIGN_LEFT;
                                    L4.Border = iTextSharp.text.Rectangle.BOX;
                                    table.AddCell(L4);

                                    string DWPL = string.Empty;
                                    if (LEntity.LPM_0012.Trim() == "Y" && LEntity.LPM_0013.Trim() == "Y" && LEntity.LPM_0014.Trim() == "Y") DWPL = "1";
                                    else if (LEntity.LPM_0012.Trim() == "Y" && LEntity.LPM_0013.Trim() == "Y" && (LEntity.LPM_0014.Trim() == "N" || LEntity.LPM_0014.Trim() == "")) DWPL = "2";
                                    else if (LEntity.LPM_0012.Trim() == "Y" && (LEntity.LPM_0013.Trim() == "N" || LEntity.LPM_0013.Trim() == "") && (LEntity.LPM_0014.Trim() == "N" || LEntity.LPM_0014.Trim() == "")) DWPL = "3";

                                    PdfPCell L5 = new PdfPCell(new Phrase(DWPL, TableFont));
                                    L5.HorizontalAlignment = Element.ALIGN_CENTER;
                                    L5.VerticalAlignment= Element.ALIGN_BOTTOM;
                                    L5.Border = iTextSharp.text.Rectangle.BOX;
                                    table.AddCell(L5);

                                    string SWPL = string.Empty;
                                    if (LEntity.LPM_0015.Trim() == "Y" && LEntity.LPM_0016.Trim() == "Y" && LEntity.LPM_0017.Trim() == "Y") SWPL = "1";
                                    else if (LEntity.LPM_0015.Trim() == "Y" && LEntity.LPM_0016.Trim() == "Y" && (LEntity.LPM_0017.Trim() == "N" || LEntity.LPM_0017.Trim() == "")) SWPL = "2";
                                    else if (LEntity.LPM_0015.Trim() == "Y" && (LEntity.LPM_0016.Trim() == "N" || LEntity.LPM_0016.Trim() =="") && (LEntity.LPM_0017.Trim() == "N" || LEntity.LPM_0017.Trim() =="")) SWPL = "3";

                                    PdfPCell L6 = new PdfPCell(new Phrase(SWPL, TableFont));
                                    L6.HorizontalAlignment = Element.ALIGN_CENTER;
                                    L6.VerticalAlignment = Element.ALIGN_BOTTOM;
                                    L6.Border = iTextSharp.text.Rectangle.BOX;
                                    table.AddCell(L6);

                                }

                            }
                        }

                        

                        if (table.Rows.Count > 0)
                        {
                            document.Add(table);
                        }

                        

                        if (chkbExcel.Checked)
                        {

                            OnExcel_LIHWAP_Summary(LPMList, PdfName, Year);
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

        private void OnExcel_LIHWAP_Summary(List<LPMQEntity> LPMList, string pdfname, string Year)
        {
            //string propReportPath = _model.lookupDataAccess.GetReportPath(BaseForm.BaseAgency);
            //string PdfName = "Pdf File";
            PdfName = pdfname.Trim().Remove(pdfname.Trim().Length - 4);
            //PdfName = strFolderPath + PdfName;
            PdfName = PdfName + ".xls";

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
                string Tmpstr = PdfName;
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
                PdfName = PdfName;



            string data = null;
            Workbook book = new Workbook();

            this.GenerateStyles(book.Styles);
            GenerateWorksheetParameters(book.Worksheets);

            Worksheet sheet = book.Worksheets.Add("Data");
            sheet.Table.Columns.Add(new WorksheetColumn(120));
            sheet.Table.Columns.Add(new WorksheetColumn(170));
            sheet.Table.Columns.Add(new WorksheetColumn(250));
            sheet.Table.Columns.Add(new WorksheetColumn(85));
            sheet.Table.Columns.Add(new WorksheetColumn(170));
            sheet.Table.Columns.Add(new WorksheetColumn(220));
            sheet.Table.Columns.Add(new WorksheetColumn(150));
            

            WorksheetStyle style = book.Styles.Add("HeaderStyle");
            style.Font.FontName = "Tahoma";
            style.Font.Size = 12;
            style.Font.Bold = true;
            style.Alignment.Horizontal = StyleHorizontalAlignment.Center;

            WorksheetStyle style1 = book.Styles.Add("Normal");
            style1.Font.FontName = "Tahoma";
            style1.Font.Size = 10;
            style1.Alignment.Horizontal = StyleHorizontalAlignment.Right;

            WorksheetStyle Center = book.Styles.Add("Center");
            Center.Font.FontName = "Tahoma";
            Center.Font.Size = 10;
            Center.Alignment.Horizontal = StyleHorizontalAlignment.Center;


            style = book.Styles.Add("Default");
            style.Font.FontName = "Tahoma";
            style.Font.Size = 10;

            try
            {

                

                WorksheetRow row = sheet.Table.Rows.Add();

                WorksheetCell cell = row.Cells.Add("LIHWAP Report", DataType.String, "HeaderStyle");
                //row.Cells.Add(new WorksheetCell("Matrix: " + ((ListItem)Cmb_Matrix.SelectedItem).Text.Trim(), "HeaderStyle"));
                cell.MergeAcross = 6;

                row = sheet.Table.Rows.Add();
                string[] HeaderSeq4 = { "App #","Name", "Address", "Phone", "Email", "Priority Level - Drinking Water", "Priority Level - Sewer" };
                for (int i = 0; i < HeaderSeq4.Length; ++i)
                {
                    row.Cells.Add(new WorksheetCell(HeaderSeq4[i], "HeaderStyle"));
                }


                if(LPMList.Count>0)
                {
                    bool IsPrint = false;
                    foreach (LPMQEntity LEntity in LPMList)
                    {
                        IsPrint = false;

                        if (LEntity.LPM_0012.Trim() == "Y" || LEntity.LPM_0015.Trim() == "Y")
                            IsPrint = true;

                        if (IsPrint)
                        {
                            row = sheet.Table.Rows.Add();
                            row.Height = 20;

                            row.Cells.Add(LEntity.ApplNo.Trim());

                            row.Cells.Add(LEntity.NameixFi.Trim() + " " + LEntity.NameixLast.Trim());
                            
                           
                            string Address = string.Empty;
                            string HN = string.Empty; string Street = string.Empty; string Aprt = string.Empty; string Floor = string.Empty;
                            if (!string.IsNullOrEmpty(LEntity.Hn.Trim())) HN = LEntity.Hn.Trim() + " ";
                            if (!string.IsNullOrEmpty(LEntity.Street.Trim())) Street = LEntity.Street.Trim() + " " + LEntity.Mst_Suffix.Trim();
                            if (!string.IsNullOrEmpty(LEntity.Mst_Apt.Trim())) Aprt = " APT:" + LEntity.Mst_Apt.Trim();
                            if (!string.IsNullOrEmpty(LEntity.MsT_Floor.Trim())) Floor = " FLR:" + LEntity.MsT_Floor.Trim();

                            Address = HN + Street + Aprt + Floor + ", "; string Tmp_Zip = "00000";
                            Tmp_Zip = LEntity.MsT_Zip.Trim();
                            Tmp_Zip = "00000".Substring(0, 5 - Tmp_Zip.Length) + Tmp_Zip;

                            Address = (Address + LEntity.City.Trim() + " " + LEntity.State.Trim() + "  " + Tmp_Zip);

                            row.Cells.Add(Address);

                            MaskedTextBox mskPhn = new MaskedTextBox();
                            mskPhn.Mask = "(999) 000-0000";
                            if (!string.IsNullOrEmpty(LEntity.Area.Trim() + LEntity.Phone.Trim()))
                                mskPhn.Text = LEntity.Area.Trim() + LEntity.Phone.Trim();

                            row.Cells.Add(mskPhn.Text.Trim());

                            row.Cells.Add(LEntity.Mst_Email.Trim());

                            string DWPL = string.Empty;
                            if (LEntity.LPM_0012.Trim() == "Y" && LEntity.LPM_0013.Trim() == "Y" && LEntity.LPM_0014.Trim() == "Y") DWPL = "1";
                            else if (LEntity.LPM_0012.Trim() == "Y" && LEntity.LPM_0013.Trim() == "Y" && (LEntity.LPM_0014.Trim() == "N" || LEntity.LPM_0014.Trim() == "")) DWPL = "2";
                            else if (LEntity.LPM_0012.Trim() == "Y" && (LEntity.LPM_0013.Trim() == "N" || LEntity.LPM_0013.Trim() == "") && (LEntity.LPM_0014.Trim() == "N" || LEntity.LPM_0014.Trim() == "")) DWPL = "3";


                            if (string.IsNullOrEmpty(DWPL.Trim()))
                                row.Cells.Add(DWPL, DataType.String, "Center");
                            else
                                row.Cells.Add(DWPL, DataType.Number, "Center");

                            string SWPL = string.Empty;
                            if (LEntity.LPM_0015.Trim() == "Y" && LEntity.LPM_0016.Trim() == "Y" && LEntity.LPM_0017.Trim() == "Y") SWPL = "1";
                            else if (LEntity.LPM_0015.Trim() == "Y" && LEntity.LPM_0016.Trim() == "Y" && (LEntity.LPM_0017.Trim() == "N" || LEntity.LPM_0017.Trim() == "")) SWPL = "2";
                            else if (LEntity.LPM_0015.Trim() == "Y" && (LEntity.LPM_0016.Trim() == "N" || LEntity.LPM_0016.Trim() == "") && (LEntity.LPM_0017.Trim() == "N" || LEntity.LPM_0017.Trim() == "")) SWPL = "3";

                            if (string.IsNullOrEmpty(SWPL.Trim()))
                                row.Cells.Add(SWPL,DataType.String, "Center");
                            else
                                row.Cells.Add(SWPL, DataType.Number, "Center");
                        }

                    }
                }

                
                FileStream stream = new FileStream(PdfName, FileMode.Create);

                book.Save(stream);
                stream.Close();
            }
            catch (Exception ex) { }


        }


        private void On_LIHEAPW_Detail(object sender, FormClosedEventArgs e)
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
                iTextSharp.text.Font HeaderTblFontBold = new iTextSharp.text.Font(bf_times, 8, 1);
                iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 8, 2);
                iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 9, 4);
                cb = writer.DirectContent;

                if (Agency == "**") Agency = null; if (Depart == "**") Depart = null; if (Program == "**") Program = null;
                string Year = string.Empty; string AppNo = string.Empty; string HeatSource = string.Empty; string Seq = string.Empty;
                if (CmbYear.Visible == true)
                    Year = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString().Trim();

                if (rbDeliverables.Checked) HeatSource = "B1"; else if (rbUtilities.Checked) HeatSource = "U1";

                List<LIHPMQuesEntity> lihpmQues = _model.ZipCodeAndAgency.GetLIHPMQuesData(string.Empty, Year);

                var LihmpquesDesc = lihpmQues.Select(u => u.LPMQ_DESC.Trim()).Distinct().ToList();

                List<LPMQEntity> LPMList = _model.CaseMstData.GetLPMQ0001(Agency, Depart, Program, Year, string.Empty);

                List<CommonEntity> lihpResp = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTabs.LIHPMQUES, Agency, Depart, Program, "View");
                try
                {
                    PrintHeaderPage(document, writer);
                    document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                    document.NewPage();

                    if (lihpmQues.Count > 0)
                    {
                        PdfPTable table = new PdfPTable(11);
                        table.TotalWidth = 790f;
                        table.WidthPercentage = 100;
                        table.LockedWidth = true;
                        float[] widths = new float[] {15f, 35f, 40f, 25f, 35f, 25f, 25f, 25f, 20f, 22f, 25f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                        table.SetWidths(widths);
                        table.HorizontalAlignment = Element.ALIGN_CENTER;
                        table.HeaderRows = 1;

                        string[] HeaderSeq4 = {"App#", "Name", "Address", "Phone", "E-Mail", "Pay for Drinking Water - Y/N", "Drinking Water Past Due  - Y/N", "Drinking Water Disconnected -  Y/N", "Pay for Sewer - Y/N", "Sewer Past Due  - Y/N", "Sewer Disconnected -  Y/N" };
                        for (int i = 0; i < HeaderSeq4.Length; ++i)
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(HeaderSeq4[i], TblFontBold));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.Border = iTextSharp.text.Rectangle.BOX;
                            table.AddCell(cell);
                        }

                        if (LPMList.Count > 0)
                        {
                            bool IsPrint = false;
                            foreach (LPMQEntity LEntity in LPMList)
                            {
                                IsPrint = false;

                                if (LEntity.LPM_0012.Trim() == "Y" || LEntity.LPM_0015.Trim() == "Y")
                                    IsPrint = true;

                                if (IsPrint)
                                {
                                    PdfPCell L0 = new PdfPCell(new Phrase(LEntity.ApplNo.Trim(), TableFont));
                                    L0.HorizontalAlignment = Element.ALIGN_LEFT;
                                    L0.Border = iTextSharp.text.Rectangle.BOX;
                                    table.AddCell(L0);

                                    PdfPCell L1 = new PdfPCell(new Phrase(LEntity.NameixFi.Trim() + " " + LEntity.NameixLast.Trim(), TableFont));
                                    L1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    L1.Border = iTextSharp.text.Rectangle.BOX;
                                    table.AddCell(L1);

                                    string Address = string.Empty;
                                    string HN = string.Empty; string Street = string.Empty; string Aprt = string.Empty; string Floor = string.Empty;
                                    if (!string.IsNullOrEmpty(LEntity.Hn.Trim())) HN = LEntity.Hn.Trim() + " ";
                                    if (!string.IsNullOrEmpty(LEntity.Street.Trim())) Street = LEntity.Street.Trim() + " " + LEntity.Mst_Suffix.Trim();
                                    if (!string.IsNullOrEmpty(LEntity.Mst_Apt.Trim())) Aprt = " APT:" + LEntity.Mst_Apt.Trim();
                                    if (!string.IsNullOrEmpty(LEntity.MsT_Floor.Trim())) Floor = " FLR:" + LEntity.MsT_Floor.Trim();

                                    Address = HN + Street + Aprt + Floor + ", "; string Tmp_Zip = "00000";
                                    Tmp_Zip = LEntity.MsT_Zip.Trim();
                                    Tmp_Zip = "00000".Substring(0, 5 - Tmp_Zip.Length) + Tmp_Zip;

                                    Address = (Address + LEntity.City.Trim() + " " + LEntity.State.Trim() + "  " + Tmp_Zip);

                                    PdfPCell L2 = new PdfPCell(new Phrase(Address, TableFont));
                                    L2.HorizontalAlignment = Element.ALIGN_LEFT;
                                    L2.Border = iTextSharp.text.Rectangle.BOX;
                                    table.AddCell(L2);

                                    MaskedTextBox mskPhn = new MaskedTextBox();
                                    mskPhn.Mask = "(999) 000-0000";
                                    if (!string.IsNullOrEmpty(LEntity.Area.Trim() + LEntity.Phone.Trim()))
                                        mskPhn.Text = LEntity.Area.Trim() + LEntity.Phone.Trim();

                                    PdfPCell L3 = new PdfPCell(new Phrase(mskPhn.Text.Trim(), TableFont));
                                    L3.HorizontalAlignment = Element.ALIGN_LEFT;
                                    L3.Border = iTextSharp.text.Rectangle.BOX;
                                    table.AddCell(L3);

                                    PdfPCell L4 = new PdfPCell(new Phrase(LEntity.Mst_Email.Trim(), TableFont));
                                    L4.HorizontalAlignment = Element.ALIGN_LEFT;
                                    L4.Border = iTextSharp.text.Rectangle.BOX;
                                    table.AddCell(L4);

                                    //string DWPL = string.Empty;
                                    //if (LEntity.LPM_0012.Trim() == "Y" && LEntity.LPM_0013.Trim() == "Y" && LEntity.LPM_0014.Trim() == "Y") DWPL = "1";
                                    //else if (LEntity.LPM_0012.Trim() == "Y" && LEntity.LPM_0013.Trim() == "Y" && LEntity.LPM_0014.Trim() == "N") DWPL = "2";
                                    //else if (LEntity.LPM_0012.Trim() == "Y" && LEntity.LPM_0013.Trim() == "N" && LEntity.LPM_0014.Trim() == "N") DWPL = "3";

                                    PdfPCell L5 = new PdfPCell(new Phrase(LEntity.LPM_0012.Trim(), TableFont));
                                    L5.HorizontalAlignment = Element.ALIGN_CENTER;
                                    L5.VerticalAlignment = Element.ALIGN_BOTTOM;
                                    L5.Border = iTextSharp.text.Rectangle.BOX;
                                    table.AddCell(L5);

                                    //string SWPL = string.Empty;
                                    //if (LEntity.LPM_0015.Trim() == "Y" && LEntity.LPM_0016.Trim() == "Y" && LEntity.LPM_0017.Trim() == "Y") SWPL = "1";
                                    //else if (LEntity.LPM_0015.Trim() == "Y" && LEntity.LPM_0016.Trim() == "Y" && LEntity.LPM_0017.Trim() == "N") SWPL = "2";
                                    //else if (LEntity.LPM_0015.Trim() == "Y" && LEntity.LPM_0016.Trim() == "N" && LEntity.LPM_0017.Trim() == "N") SWPL = "3";

                                    PdfPCell L6 = new PdfPCell(new Phrase(LEntity.LPM_0013.Trim(), TableFont));
                                    L6.HorizontalAlignment = Element.ALIGN_CENTER;
                                    L6.VerticalAlignment = Element.ALIGN_BOTTOM;
                                    L6.Border = iTextSharp.text.Rectangle.BOX;
                                    table.AddCell(L6);

                                    PdfPCell L7 = new PdfPCell(new Phrase(LEntity.LPM_0014.Trim(), TableFont));
                                    L7.HorizontalAlignment = Element.ALIGN_CENTER;
                                    L7.VerticalAlignment = Element.ALIGN_BOTTOM;
                                    L7.Border = iTextSharp.text.Rectangle.BOX;
                                    table.AddCell(L7);

                                    PdfPCell L8 = new PdfPCell(new Phrase(LEntity.LPM_0015.Trim(), TableFont));
                                    L8.HorizontalAlignment = Element.ALIGN_CENTER;
                                    L8.VerticalAlignment = Element.ALIGN_BOTTOM;
                                    L8.Border = iTextSharp.text.Rectangle.BOX;
                                    table.AddCell(L8);

                                    PdfPCell L9 = new PdfPCell(new Phrase(LEntity.LPM_0016.Trim(), TableFont));
                                    L9.HorizontalAlignment = Element.ALIGN_CENTER;
                                    L9.VerticalAlignment = Element.ALIGN_BOTTOM;
                                    L9.Border = iTextSharp.text.Rectangle.BOX;
                                    table.AddCell(L9);

                                    PdfPCell L10 = new PdfPCell(new Phrase(LEntity.LPM_0017.Trim(), TableFont));
                                    L10.HorizontalAlignment = Element.ALIGN_CENTER;
                                    L10.VerticalAlignment = Element.ALIGN_BOTTOM;
                                    L10.Border = iTextSharp.text.Rectangle.BOX;
                                    table.AddCell(L10);

                                }

                            }
                        }



                        if (table.Rows.Count > 0)
                        {
                            document.Add(table);
                        }



                        if (chkbExcel.Checked)
                        {

                            OnExcel_LIHWAP_Detail(LPMList, PdfName, Year);
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

        private void OnExcel_LIHWAP_Detail(List<LPMQEntity> LPMList, string pdfname, string Year)
        {
            //string propReportPath = _model.lookupDataAccess.GetReportPath(BaseForm.BaseAgency);
            //string PdfName = "Pdf File";
            PdfName = pdfname.Trim().Remove(pdfname.Trim().Length - 4);
            //PdfName = strFolderPath + PdfName;
            PdfName = PdfName + ".xls";

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
                string Tmpstr = PdfName;
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
                PdfName = PdfName;



            string data = null;
            Workbook book = new Workbook();

            this.GenerateStyles(book.Styles);
            GenerateWorksheetParameters(book.Worksheets);

            Worksheet sheet = book.Worksheets.Add("Data");
            if (rbSummary.Checked)
            {
                sheet.Table.Columns.Add(new WorksheetColumn(150));
                sheet.Table.Columns.Add(new WorksheetColumn(180));
                sheet.Table.Columns.Add(new WorksheetColumn(70));
                sheet.Table.Columns.Add(new WorksheetColumn(90));
                sheet.Table.Columns.Add(new WorksheetColumn(70));
                sheet.Table.Columns.Add(new WorksheetColumn(70));
            }
            else
            {
                sheet.Table.Columns.Add(new WorksheetColumn(120));
                sheet.Table.Columns.Add(new WorksheetColumn(150));
                sheet.Table.Columns.Add(new WorksheetColumn(250));
                sheet.Table.Columns.Add(new WorksheetColumn(85));
                sheet.Table.Columns.Add(new WorksheetColumn(140));
                sheet.Table.Columns.Add(new WorksheetColumn(200));
                sheet.Table.Columns.Add(new WorksheetColumn(200));
                sheet.Table.Columns.Add(new WorksheetColumn(200));
                sheet.Table.Columns.Add(new WorksheetColumn(180));
                sheet.Table.Columns.Add(new WorksheetColumn(200));
                sheet.Table.Columns.Add(new WorksheetColumn(200));
            }

            WorksheetStyle style = book.Styles.Add("HeaderStyle");
            style.Font.FontName = "Tahoma";
            style.Font.Size = 12;
            style.Font.Bold = true;
            style.Alignment.Horizontal = StyleHorizontalAlignment.Center;

            WorksheetStyle style1 = book.Styles.Add("Normal");
            style1.Font.FontName = "Tahoma";
            style1.Font.Size = 10;
            style1.Alignment.Horizontal = StyleHorizontalAlignment.Right;

            WorksheetStyle Center = book.Styles.Add("Center");
            Center.Font.FontName = "Tahoma";
            Center.Font.Size = 10;
            Center.Alignment.Horizontal = StyleHorizontalAlignment.Center;


            style = book.Styles.Add("Default");
            style.Font.FontName = "Tahoma";
            style.Font.Size = 10;

            try
            {



                WorksheetRow row = sheet.Table.Rows.Add();

                WorksheetCell cell = row.Cells.Add("LIHWAP Report", DataType.String, "HeaderStyle");
                //row.Cells.Add(new WorksheetCell("Matrix: " + ((ListItem)Cmb_Matrix.SelectedItem).Text.Trim(), "HeaderStyle"));
                cell.MergeAcross = 10;

                row = sheet.Table.Rows.Add();
                string[] HeaderSeq4 = { "App#", "Name", "Address", "Phone", "Email", "Pay for Drinking Water - Y/N", "Drinking Water Past Due  - Y/N", "Drinking Water Disconnected -  Y/N", "Pay for Sewer - Y/N", "Sewer Past Due  - Y/N", "Sewer Disconnected -  Y/N" };
                for (int i = 0; i < HeaderSeq4.Length; ++i)
                {
                    row.Cells.Add(new WorksheetCell(HeaderSeq4[i], "HeaderStyle"));
                }


                if (LPMList.Count > 0)
                {
                    bool IsPrint = false;
                    foreach (LPMQEntity LEntity in LPMList)
                    {
                        IsPrint = false;

                        if (LEntity.LPM_0012.Trim() == "Y" || LEntity.LPM_0015.Trim() == "Y")
                            IsPrint = true;

                        if (IsPrint)
                        {
                            row = sheet.Table.Rows.Add();
                            row.Height = 20;

                            row.Cells.Add(LEntity.ApplNo.Trim());

                            row.Cells.Add(LEntity.NameixFi.Trim() + " " + LEntity.NameixLast.Trim());


                            string Address = string.Empty;
                            string HN = string.Empty; string Street = string.Empty; string Aprt = string.Empty; string Floor = string.Empty;
                            if (!string.IsNullOrEmpty(LEntity.Hn.Trim())) HN = LEntity.Hn.Trim() + " ";
                            if (!string.IsNullOrEmpty(LEntity.Street.Trim())) Street = LEntity.Street.Trim() + " " + LEntity.Mst_Suffix.Trim();
                            if (!string.IsNullOrEmpty(LEntity.Mst_Apt.Trim())) Aprt = " APT:" + LEntity.Mst_Apt.Trim();
                            if (!string.IsNullOrEmpty(LEntity.MsT_Floor.Trim())) Floor = " FLR:" + LEntity.MsT_Floor.Trim();

                            Address = HN + Street + Aprt + Floor + ", "; string Tmp_Zip = "00000";
                            Tmp_Zip = LEntity.MsT_Zip.Trim();
                            Tmp_Zip = "00000".Substring(0, 5 - Tmp_Zip.Length) + Tmp_Zip;

                            Address = (Address + LEntity.City.Trim() + " " + LEntity.State.Trim() + "  " + Tmp_Zip);

                            row.Cells.Add(Address);

                            MaskedTextBox mskPhn = new MaskedTextBox();
                            mskPhn.Mask = "(999) 000-0000";
                            if (!string.IsNullOrEmpty(LEntity.Area.Trim() + LEntity.Phone.Trim()))
                                mskPhn.Text = LEntity.Area.Trim() + LEntity.Phone.Trim();

                            row.Cells.Add(mskPhn.Text.Trim());

                            row.Cells.Add(LEntity.Mst_Email.Trim());

                            //string DWPL = string.Empty;
                            //if (LEntity.LPM_0012.Trim() == "Y" && LEntity.LPM_0013.Trim() == "Y" && LEntity.LPM_0014.Trim() == "Y") DWPL = "1";
                            //else if (LEntity.LPM_0012.Trim() == "Y" && LEntity.LPM_0013.Trim() == "Y" && LEntity.LPM_0014.Trim() == "N") DWPL = "2";
                            //else if (LEntity.LPM_0012.Trim() == "Y" && LEntity.LPM_0013.Trim() == "N" && LEntity.LPM_0014.Trim() == "N") DWPL = "3";

                            row.Cells.Add(LEntity.LPM_0012.Trim(), DataType.String, "Center");

                            //string SWPL = string.Empty;
                            //if (LEntity.LPM_0015.Trim() == "Y" && LEntity.LPM_0016.Trim() == "Y" && LEntity.LPM_0017.Trim() == "Y") SWPL = "1";
                            //else if (LEntity.LPM_0015.Trim() == "Y" && LEntity.LPM_0016.Trim() == "Y" && LEntity.LPM_0017.Trim() == "N") SWPL = "2";
                            //else if (LEntity.LPM_0015.Trim() == "Y" && LEntity.LPM_0016.Trim() == "N" && LEntity.LPM_0017.Trim() == "N") SWPL = "3";

                            row.Cells.Add(LEntity.LPM_0013.Trim(), DataType.String, "Center");
                            row.Cells.Add(LEntity.LPM_0014.Trim(), DataType.String, "Center");

                            row.Cells.Add(LEntity.LPM_0015.Trim(), DataType.String, "Center");
                            row.Cells.Add(LEntity.LPM_0016.Trim(), DataType.String, "Center");
                            row.Cells.Add(LEntity.LPM_0017.Trim(),DataType.String, "Center");

                        }

                    }
                }


                FileStream stream = new FileStream(PdfName, FileMode.Create);

                book.Save(stream);
                stream.Close();
            }
            catch (Exception ex) { }


        }



    }
}