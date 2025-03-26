using Captain.Common.Interfaces;
using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Views.Forms.Base;
using DevExpress.DataProcessing.InMemoryDataProcessor;
using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using Wisej.Web;
using Captain.Common.Utilities;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using DevExpress.DataAccess.Native.Sql;
using Syncfusion.XlsIO.Implementation.XmlSerialization;
using DevExpress.CodeParser;
using CarlosAg.ExcelXmlWriter;
using DevExpress.Utils.Win.Hook;
using OfficeOpenXml;
using DevExpress.DataAccess.UI.Wizard.Views;
using DevExpress.UIAutomation;
using Syncfusion.Calculate;
using DevExpress.XtraSpreadsheet.API.Native.Implementation;
using System.Data.SqlClient;
using NPOI.SS.Formula.Functions;
using System.Net.PeerToPeer;
using System.Globalization;
using NPOI.POIFS.Properties;


namespace Captain.Common.Views.Forms
{
    public partial class HSSB1026_PIRCounting_Form : Form
    {
        #region Private Variables
        private CaptainModel _model = null;
        #endregion

        public HSSB1026_PIRCounting_Form(BaseForm baseForm, PrivilegeEntity privileges)
        {
            InitializeComponent();

            _baseform = baseForm;
            _priviliges = privileges;
            _model = new CaptainModel();
            Agency = _baseform.BaseAgency;
            Depart = _baseform.BaseDept;
            Program = _baseform.BaseProg;
            Program_Year = _baseform.BaseYear;
            Set_Report_Hierarchy(_baseform.BaseAgency, _baseform.BaseDept, _baseform.BaseProg, _baseform.BaseYear);
            propReportPath = _model.lookupDataAccess.GetReportPath();

            Fill_Section_Fund_Combo();
            Fill_Sites_Combo();

            btnSaveRepCounts.Visible = false;

            //**chkbHitNoOnly.Checked = true;

            //fillQuestionsGrid();
        }

        #region Properties

        public BaseForm _baseform
        {
            get; set;
        }

        public PrivilegeEntity _priviliges
        {
            get; set;
        }
        public DataTable dtQues
        {
            get; set;
        }
        public DataTable dtCounts
        {
            get; set;
        }
        public string propReportPath
        {
            get; set;
        }

        #endregion

        #region Hierarchy Code
        private void Pb_Search_Hie_Click(object sender, EventArgs e)
        {
            HierarchieSelection hierarchieSelectionForm = new HierarchieSelection(_baseform, Current_Hierarchy_DB, "Master", "", "A", "Reports", _baseform.UserID);
            hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
            hierarchieSelectionForm.StartPosition = FormStartPosition.CenterScreen;
            hierarchieSelectionForm.ShowDialog();
        }

        string Agency = string.Empty, Depart = string.Empty, Program = string.Empty, strYear = string.Empty;
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
                this.Txt_HieDesc.Size = new System.Drawing.Size(870, 25);
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
                this.Txt_HieDesc.Size = new System.Drawing.Size(790, 25);
            else
                this.Txt_HieDesc.Size = new System.Drawing.Size(870, 25);
        }

        #endregion

        private void Fill_Section_Fund_Combo()
        {
            Cmb_Section.SelectedIndexChanged -= new EventHandler(Cmb_Section_SelectedIndexChanged);
            Cmb_Section.Items.Clear();
            List<Captain.Common.Utilities.ListItem> listItem = new List<Captain.Common.Utilities.ListItem>();
            listItem.Add(new Captain.Common.Utilities.ListItem("Section - A", "A"));
            listItem.Add(new Captain.Common.Utilities.ListItem("Section - B", "B"));
            listItem.Add(new Captain.Common.Utilities.ListItem("Section - C", "C"));
            Cmb_Section.Items.AddRange(listItem.ToArray());
            Cmb_Section.SelectedIndex = 1;


            //cmbFunds.Items.Insert(0, new Captain.Common.Utilities.ListItem("All", "9"));
            //cmbFunds.Items.Insert(1, new Captain.Common.Utilities.ListItem("HS", "H"));
            //cmbFunds.Items.Insert(2, new Captain.Common.Utilities.ListItem("HS 2", "2"));
            //cmbFunds.Items.Insert(3, new Captain.Common.Utilities.ListItem("EHS", "E"));
            //cmbFunds.Items.Insert(4, new Captain.Common.Utilities.ListItem("EHSCCP", "S"));
            //if (((Captain.Common.Utilities.ListItem)Cmb_Section.SelectedItem).Value.ToString() == "B")
            //    cmbFunds.SelectedIndex = 0;
            //else
            //{
            //    cmbFunds.SelectedIndex = 1;
            //}

            Cmb_Section.SelectedIndexChanged += new EventHandler(Cmb_Section_SelectedIndexChanged);
            Cmb_Section_SelectedIndexChanged(Cmb_Section, EventArgs.Empty);
        }

        private void fillFundsCombo()
        {
            cmbFunds.SelectedIndexChanged -= new EventHandler(cmbFunds_SelectedIndexChanged);
            cmbFunds.Items.Clear();

            if (((Captain.Common.Utilities.ListItem)Cmb_Section.SelectedItem).Value.ToString() == "B")
            {
                cmbFunds.Items.Insert(0, new Captain.Common.Utilities.ListItem("All", "9"));
                cmbFunds.Items.Insert(1, new Captain.Common.Utilities.ListItem("HS", "H"));
                cmbFunds.Items.Insert(2, new Captain.Common.Utilities.ListItem("HS 2", "2"));
                cmbFunds.Items.Insert(3, new Captain.Common.Utilities.ListItem("EHS", "E"));
                cmbFunds.Items.Insert(4, new Captain.Common.Utilities.ListItem("EHSCCP", "S"));
            }
            else
            {
                cmbFunds.Items.Insert(0, new Captain.Common.Utilities.ListItem("HS", "H"));
                cmbFunds.Items.Insert(1, new Captain.Common.Utilities.ListItem("HS 2", "2"));
                cmbFunds.Items.Insert(2, new Captain.Common.Utilities.ListItem("EHS", "E"));
                cmbFunds.Items.Insert(3, new Captain.Common.Utilities.ListItem("EHSCCP", "S"));
            }
            
            //if (((Captain.Common.Utilities.ListItem)Cmb_Section.SelectedItem).Value.ToString() == "B")
                cmbFunds.SelectedIndex = 0;
            cmbFunds.SelectedIndexChanged += new EventHandler(cmbFunds_SelectedIndexChanged);
            //cmbFunds_SelectedIndexChanged(cmbFunds, EventArgs.Empty);
        }

        private void Fill_Sites_Combo()
        {
            Cmb_Site.Items.Clear();
            Cmb_Site.ColorMember = "FavoriteColor";

            List<Captain.Common.Utilities.ListItem> listItem = new List<Captain.Common.Utilities.ListItem>();

            CaseSiteEntity Search_Entity = new CaseSiteEntity(true);
            List<CaseSiteEntity> HSS_Site_List = new List<CaseSiteEntity>();
            List<Captain.Common.Utilities.ListItem> HSS_Site_Headers_List = new List<Captain.Common.Utilities.ListItem>();
            HSS_Site_Headers_List.Add(new Captain.Common.Utilities.ListItem("All Sites", "****"));

            Search_Entity.SiteAGENCY = Agency;
            Search_Entity.SiteDEPT = Depart;
            Search_Entity.SitePROG = Program;
            Search_Entity.SiteYEAR = Program_Year;
            HSS_Site_List = _model.CaseMstData.Browse_CASESITE(Search_Entity, "Browse");

            if (HSS_Site_List.Count > 0 && _baseform.BaseAgencyControlDetails.SiteSecurity == "1")
            {
                List<HierarchyEntity> userHierarchy = _model.UserProfileAccess.GetUserHierarchyByID(_baseform.UserID);
                HierarchyEntity hierarchyEntity = new HierarchyEntity();
                List<CaseSiteEntity> selsites = new List<CaseSiteEntity>();
                foreach (HierarchyEntity Entity in userHierarchy)
                {
                    if (Entity.InActiveFlag == "N")
                    {
                        if (Entity.Agency == Agency && Entity.Dept == Depart && Entity.Prog == Program)
                            hierarchyEntity = Entity;
                        else if (Entity.Agency == Agency && Entity.Dept == Depart && Entity.Prog == "**")
                            hierarchyEntity = Entity;
                        else if (Entity.Agency == Agency && Entity.Dept == "**" && Entity.Prog == "**")
                            hierarchyEntity = Entity;
                        else if (Entity.Agency == "**" && Entity.Dept == "**" && Entity.Prog == "**")
                        {
                            hierarchyEntity = null;
                        }
                    }
                }

                if (hierarchyEntity != null)
                {
                    List<CaseSiteEntity> SelSite_List = new List<CaseSiteEntity>();

                    if (hierarchyEntity.Sites.Length > 0)
                    {
                        string[] Sites = hierarchyEntity.Sites.Split(',');

                        for (int i = 0; i < Sites.Length; i++)
                        {
                            if (!string.IsNullOrEmpty(Sites[i].ToString().Trim()))
                            {
                                foreach (CaseSiteEntity casesite in HSS_Site_List)
                                {
                                    if (Sites[i].ToString() == casesite.SiteNUMBER)
                                    {
                                        SelSite_List.Add(casesite);
                                    }
                                }
                            }
                        }
                        HSS_Site_List = SelSite_List;
                    }

                }
            }

            HSS_Site_List = HSS_Site_List.OrderByDescending(u => u.SiteACTIVE).ToList();
            if (HSS_Site_List.Count > 0)
            {
                foreach (CaseSiteEntity Entity in HSS_Site_List)
                {
                    if (Entity.SiteROOM == "0000" && string.IsNullOrEmpty(Entity.SiteAM_PM.Trim()))
                        HSS_Site_Headers_List.Add(new Captain.Common.Utilities.ListItem(Entity.SiteNAME, Entity.SiteNUMBER, Entity.SiteACTIVE, Entity.SiteACTIVE.Equals("Y") ? Color.Black : Color.Red));
                }
            }
            Cmb_Site.Items.AddRange(HSS_Site_Headers_List.ToArray());
            Cmb_Site.SelectedIndex = 0;
        }

        private void Cmb_Section_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillFundsCombo();


            cmbFunds_SelectedIndexChanged(sender, e);
        }

        private void cmbFunds_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillQuestionsGrid();

            if (dtCounts.Rows.Count > 0)
            {
                DataView dv = new DataView(dtCounts);
                dv.RowFilter = "PIRCOUNT_Q_FUND='" + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Value.ToString() + "'";

                dtCounts = dv.ToTable();

                txtCountsGeneratedOn.Text = "Counts Saved On: " + (dtCounts.Rows[0]["PIRCOUNT_DATE_LSTC"].ToString() == "" ? "" : Convert.ToDateTime(dtCounts.Rows[0]["PIRCOUNT_DATE_LSTC"].ToString()).ToString("MM/dd/yyyy hh:mm:ss tt")) + ", by " + dtCounts.Rows[0]["PIRCOUNT_LSTC_OPERATOR"].ToString();
            }
            else
                txtCountsGeneratedOn.Text = string.Empty;

            btnSaveRepCounts.Visible = false;
        }

        #region PIR Counts PDF

        private void btnPIRCounts_Click(object sender, EventArgs e)
        {
            string Sel_site = string.Empty;
            if (((Captain.Common.Utilities.ListItem)Cmb_Site.SelectedItem).Value.ToString() != "****")
                Sel_site = ((Captain.Common.Utilities.ListItem)Cmb_Site.SelectedItem).Value.ToString();

            DataSet dsPIRCounts = Captain.DatabaseLayer.Lookups.Browse_PIR_Audit_Report(Agency, Depart, Program, Program_Year, ((Captain.Common.Utilities.ListItem)Cmb_Section.SelectedItem).Value.ToString(), ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Value.ToString(), Sel_site, "", "", _baseform.UserID, "N", "N");

            DataTable dtPIR_Counts = new DataTable();
            if(((Captain.Common.Utilities.ListItem)Cmb_Section.SelectedItem).Value.ToString()=="B")
                dtPIR_Counts = dsPIRCounts.Tables[1];
            else
                dtPIR_Counts = dsPIRCounts.Tables[2];

            GeneratePIRCounts(dtPIR_Counts);

            btnSaveRepCounts.Visible = true;
        }

        private void GeneratePIRCounts(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                foreach (DataGridViewRow dgv in dgvQuestions.Rows)
                {
                    if (dgv.Cells["gvisHeader"].Value.ToString() != "H")
                    {
                        DataRow[] dr = dt.Select("QuesID = '" + dgv.Cells["gvQID"].Value.ToString() + "'");

                        if (dr != null)
                        {
                            if (dgv.Cells["gvQID"].Value.ToString() == "54" || dgv.Cells["gvQID"].Value.ToString() == "55" || dgv.Cells["gvQID"].Value.ToString() == "56" || dgv.Cells["gvQID"].Value.ToString() == "57" || dgv.Cells["gvQID"].Value.ToString() == "58" || dgv.Cells["gvQID"].Value.ToString() == "59" || dgv.Cells["gvQID"].Value.ToString() == "60" || dgv.Cells["gvQID"].Value.ToString() == "61" || dgv.Cells["gvQID"].Value.ToString() == "62" || dgv.Cells["gvQID"].Value.ToString() == "63" || dgv.Cells["gvQID"].Value.ToString() == "64")
                            {
                                if (dgv.Cells["Column1"].Value.ToString() != "0" && dgv.Cells["Column1"].Value.ToString().Trim() != "")
                                    dgv.Cells["gvHeader1"].Value = dr[0][5].ToString() == "" ? "$0.00" : "$" + Decimal.Parse(dr[0][5].ToString()).ToString("N", new CultureInfo("en-US"));
                                if (dgv.Cells["Column2"].Value.ToString() != "0" && dgv.Cells["Column2"].Value.ToString().Trim() != "")
                                    dgv.Cells["gvHeader2"].Value = dr[0][6].ToString() == "" ? "$0.00" : "$" + Decimal.Parse(dr[0][6].ToString()).ToString("N", new CultureInfo("en-US"));
                                else
                                    dgv.Cells["gvHeader2"].Value = "";

                                if ((dgv.Cells["gvHeader1"].Value.ToString() == "$0.00" && dgv.Cells["gvHeader2"].Value.ToString() == "$0.00") || (dgv.Cells["gvHeader1"].Value.ToString() == "$0.00" && dgv.Cells["gvHeader2"].Value.ToString() == "") || (dgv.Cells["gvHeader1"].Value.ToString() == "" && dgv.Cells["gvHeader2"].Value.ToString() == ""))
                                {
                                    dgv.Cells["gvSelect"].Style.Padding = new Padding(dgv.Cells["gvSelect"].OwningColumn.Width, 0, 0, 0);
                                }
                                else
                                {
                                    dgv.Cells["gvSelect"].Style.Padding = new Padding(5, 5, 5, 5);
                                }
                            }
                            else
                            {
                                if (dgv.Cells["Column1"].Value.ToString() != "0" && dgv.Cells["Column1"].Value.ToString().Trim() != "")
                                    dgv.Cells["gvHeader1"].Value = dr[0][5].ToString() == "" ? 0 : Convert.ToInt32(Convert.ToDecimal(dr[0][5].ToString()));
                                if (dgv.Cells["Column2"].Value.ToString() != "0" && dgv.Cells["Column2"].Value.ToString().Trim() != "")
                                    dgv.Cells["gvHeader2"].Value = dr[0][6].ToString() == "" ? 0 : (Convert.ToInt32(Convert.ToDecimal(dr[0][6].ToString())));
                                else
                                    dgv.Cells["gvHeader2"].Value = "";

                                if ((dgv.Cells["gvHeader1"].Value.ToString() == "0" && dgv.Cells["gvHeader2"].Value.ToString() == "0") || (dgv.Cells["gvHeader1"].Value.ToString() == "0" && dgv.Cells["gvHeader2"].Value.ToString() == "") || (dgv.Cells["gvHeader1"].Value.ToString() == "" && dgv.Cells["gvHeader2"].Value.ToString() == ""))
                                {
                                    dgv.Cells["gvSelect"].Style.Padding = new Padding(dgv.Cells["gvSelect"].OwningColumn.Width, 0, 0, 0);
                                }
                                else
                                {
                                    dgv.Cells["gvSelect"].Style.Padding = new Padding(5, 5, 5, 5);
                                }
                            }
                        }
                    }
                }
            }
        }

        string PdfName;
        string Random_Filename = null;
        PdfContentByte cb;
        private void btnPIRReport_Click(object sender, EventArgs e)
        {
            PIR_Counts_PDF();
            //**PDFNew();
            /*PdfName = "PIR_Counts_Report";

            PdfName = propReportPath + _baseform.UserID + "\\" + PdfName;

            try
            {
                if (!Directory.Exists(propReportPath + _baseform.UserID.Trim()))
                {
                    DirectoryInfo di = Directory.CreateDirectory(propReportPath + _baseform.UserID.Trim());
                }
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

            Document document = new Document(PageSize.A4, 30f, 30f, 30f, 50f);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();
            BaseFont bf_calibri = BaseFont.CreateFont("c:/windows/fonts/Calibri.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);

            BaseFont bf_Times = BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1250, false);
            cb = writer.DirectContent;

            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, false);
            BaseFont bfTimesBold = BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1250, false);
            iTextSharp.text.Font fc = new iTextSharp.text.Font(bfTimes, 12, 2);
            iTextSharp.text.Font fc1 = new iTextSharp.text.Font(bfTimes, 8, 2, BaseColor.BLUE);
            iTextSharp.text.Font fcRed = new iTextSharp.text.Font(bf_calibri, 10, 1, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#806000")));

            iTextSharp.text.Font Times = new iTextSharp.text.Font(bfTimes, 7);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_calibri, 8);
            iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bfTimes, 8, 3);
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(1, 7, 1);
            iTextSharp.text.Font TblFontHeadBold = new iTextSharp.text.Font(1, 12, 1);
            iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bfTimes, 8, 2);
            iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bfTimes, 9, 4);

            DataSet dsPIRCounts = Captain.DatabaseLayer.Lookups.Browse_PIR_Audit_Report(Agency, Depart, Program, Program_Year, ((Captain.Common.Utilities.ListItem)Cmb_Section.SelectedItem).Value.ToString(), ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Value.ToString(), ((Captain.Common.Utilities.ListItem)Cmb_Site.SelectedItem).Value.ToString(), "", "", _baseform.UserID);

            DataTable dtPIR_Counts = dsPIRCounts.Tables[0];

            HeaderPage(document, writer);

            try
            {
                document.NewPage();

                PdfPTable table = new PdfPTable(3);
                table.WidthPercentage = 100;
                table.LockedWidth = true;
                table.TotalWidth = 500f;
                float[] widths = new float[] { 300f, 75f, 75f };
                table.SetWidths(widths);
                table.HorizontalAlignment = Element.ALIGN_CENTER;

                string isPrevHeader1 = string.Empty, isPrevHeader2 = string.Empty;

                foreach (DataGridViewRow dr in dgvQuestions.Rows)
                {
                    if (dr.Cells["gvisHeader"].Value.ToString() != "H")
                    {

                        if (dr.Cells["Column0"].Value.ToString() == "000")
                        {
                            PdfPCell Quesdec = new PdfPCell(new Phrase(" " + dr["gvQuesDesc"].Value.ToString().Trim(), TblFontBold));
                            Quesdec.HorizontalAlignment = Element.ALIGN_LEFT;
                            Quesdec.VerticalAlignment = Element.ALIGN_MIDDLE;
                            Quesdec.Border = iTextSharp.text.Rectangle.BOX;
                            Quesdec.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                            table.AddCell(Quesdec);
                        }
                        else
                        {
                            PdfPCell Quesdec = new PdfPCell(new Phrase("   " + dr["gvQuesDesc"].Value.ToString().Trim(), TableFont));
                            Quesdec.HorizontalAlignment = Element.ALIGN_LEFT;
                            Quesdec.VerticalAlignment = Element.ALIGN_MIDDLE;
                            Quesdec.Border = iTextSharp.text.Rectangle.BOX;
                            Quesdec.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                            table.AddCell(Quesdec);
                        }

                        if (dr.Cells["gvHeader1"].Value.ToString() != "" && dr.Cells["gvHeader2"].Value.ToString() != "")
                        {
                            PdfPCell col1Count = new PdfPCell(new Phrase(" " + dr["gvHeader1"].Value.ToString().Trim(), TableFont));
                            col1Count.HorizontalAlignment = Element.ALIGN_CENTER;
                            col1Count.VerticalAlignment = Element.ALIGN_MIDDLE;
                            col1Count.Border = iTextSharp.text.Rectangle.BOX;
                            col1Count.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                            table.AddCell(col1Count);

                            PdfPCell col2Count = new PdfPCell(new Phrase(" " + dr["gvHeader2"].Value.ToString().Trim(), TableFont));
                            col2Count.HorizontalAlignment = Element.ALIGN_CENTER;
                            col2Count.VerticalAlignment = Element.ALIGN_MIDDLE;
                            col2Count.Border = iTextSharp.text.Rectangle.BOX;
                            col2Count.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                            table.AddCell(col2Count);
                        }
                        else if (dr.Cells["gvHeader1"].Value.ToString() != "" && dr.Cells["gvHeader2"].Value.ToString() == "")
                        {
                            PdfPCell col1Count = new PdfPCell(new Phrase(" " + dr["gvHeader1"].Value.ToString().Trim(), TableFont));
                            col1Count.HorizontalAlignment = Element.ALIGN_CENTER;
                            col1Count.VerticalAlignment = Element.ALIGN_MIDDLE;
                            col1Count.Border = iTextSharp.text.Rectangle.BOX;
                            col1Count.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                            col1Count.Colspan = 2;
                            table.AddCell(col1Count);
                        }

                        else if (dr.Cells["gvHeader2"].Value.ToString() != "" && dr.Cells["gvHeader1"].Value.ToString() == "")
                        {
                            PdfPCell col2Count = new PdfPCell(new Phrase(" " + dr["gvHeader2"].Value.ToString().Trim(), TableFont));
                            col2Count.HorizontalAlignment = Element.ALIGN_CENTER;
                            col2Count.VerticalAlignment = Element.ALIGN_MIDDLE;
                            col2Count.Border = iTextSharp.text.Rectangle.BOX;
                            col2Count.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                            col2Count.Colspan = 2;
                            table.AddCell(col2Count);
                        }
                        else
                        {
                            if ((dr.Cells["Column1"].Value.ToString() == "0" || dr.Cells["Column1"].Value.ToString().Trim() == "") && (dr.Cells["Column2"].Value.ToString() == "0" || dr.Cells["Column2"].Value.ToString().Trim() == ""))
                            {
                                PdfPCell col1Count = new PdfPCell(new Phrase("", TableFont));
                                col1Count.HorizontalAlignment = Element.ALIGN_CENTER;
                                col1Count.VerticalAlignment = Element.ALIGN_MIDDLE;
                                col1Count.Border = iTextSharp.text.Rectangle.BOX;
                                col1Count.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                col1Count.Colspan = 2;
                                table.AddCell(col1Count);
                            }
                            else
                            {
                                PdfPCell col1Count = new PdfPCell(new Phrase("", TableFont));
                                col1Count.HorizontalAlignment = Element.ALIGN_CENTER;
                                col1Count.VerticalAlignment = Element.ALIGN_MIDDLE;
                                col1Count.Border = iTextSharp.text.Rectangle.BOX;
                                col1Count.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                table.AddCell(col1Count);

                                PdfPCell col2Count = new PdfPCell(new Phrase("", TableFont));
                                col2Count.HorizontalAlignment = Element.ALIGN_CENTER;
                                col2Count.VerticalAlignment = Element.ALIGN_MIDDLE;
                                col2Count.Border = iTextSharp.text.Rectangle.BOX;
                                col2Count.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                table.AddCell(col2Count);
                            }
                        }
                    }
                    else
                    {
                        if (isPrevHeader1 != dr.Cells["gvHeader1"].Value.ToString() || isPrevHeader2 != dr.Cells["gvHeader2"].Value.ToString())
                        {
                            if (dr.Cells["Column0"].Value.ToString() == "")
                            {
                                PdfPCell QuesSpaces = new PdfPCell(new Phrase("", TableFont));
                                QuesSpaces.HorizontalAlignment = Element.ALIGN_LEFT;
                                QuesSpaces.VerticalAlignment = Element.ALIGN_MIDDLE;
                                QuesSpaces.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                QuesSpaces.Colspan = 3;
                                QuesSpaces.FixedHeight = 15;
                                table.AddCell(QuesSpaces);
                            }

                            if (string.IsNullOrEmpty(dr["gvQuesDesc"].Value.ToString().Trim()))
                            {
                                PdfPCell Descspace = new PdfPCell(new Phrase(" " + dr["gvQuesDesc"].Value.ToString().Trim(), TableFont));
                                Descspace.HorizontalAlignment = Element.ALIGN_LEFT;
                                Descspace.VerticalAlignment = Element.ALIGN_MIDDLE;
                                Descspace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(Descspace);
                            }
                            else
                            {
                                PdfPCell Descspace;
                                if (dr.Cells["Column0"].Value.ToString() == "000")
                                    Descspace = new PdfPCell(new Phrase(" " + dr["gvQuesDesc"].Value.ToString().Trim(), TblFontBold));
                                else
                                    Descspace = new PdfPCell(new Phrase("   " + dr["gvQuesDesc"].Value.ToString().Trim(), TableFont));
                                Descspace.HorizontalAlignment = Element.ALIGN_LEFT;
                                Descspace.VerticalAlignment = Element.ALIGN_MIDDLE;
                                //Descspace.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fafdff"));
                                Descspace.Border = iTextSharp.text.Rectangle.BOX;
                                Descspace.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                table.AddCell(Descspace);
                            }
                            if (dr.Cells["gvHeader1"].Value.ToString() == "" || dr.Cells["gvHeader2"].Value.ToString() == "")
                            {
                                if (dr.Cells["gvHeader1"].Value.ToString() != "")
                                {
                                    //if (string.IsNullOrEmpty(dr["gvQuesDesc"].Value.ToString().Trim()))
                                    //{
                                    //    PdfPCell Descspace = new PdfPCell(new Phrase(" " + dr["gvQuesDesc"].Value.ToString().Trim(), TableFont));
                                    //    Descspace.HorizontalAlignment = Element.ALIGN_LEFT;
                                    //    Descspace.VerticalAlignment = Element.ALIGN_MIDDLE;
                                    //    Descspace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //    Descspace.Colspan = 2;
                                    //    table.AddCell(Descspace);
                                    //}
                                    //else
                                    //{
                                    //    PdfPCell Descspace;
                                    //    if (dr.Cells["Column0"].Value.ToString() == "000")
                                    //        Descspace = new PdfPCell(new Phrase(" " + dr["gvQuesDesc"].Value.ToString().Trim(), TblFontBold));
                                    //    else
                                    //        Descspace = new PdfPCell(new Phrase("   " + dr["gvQuesDesc"].Value.ToString().Trim(), TableFont));
                                    //    Descspace.HorizontalAlignment = Element.ALIGN_LEFT;
                                    //    Descspace.VerticalAlignment = Element.ALIGN_MIDDLE;
                                    //    Descspace.Border = iTextSharp.text.Rectangle.BOX;
                                    //    Descspace.Colspan = 2;
                                    //    Descspace.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                    //    table.AddCell(Descspace);
                                    //}

                                    PdfPCell col1Header = new PdfPCell(new Phrase(" " + dr["gvHeader1"].Value.ToString().Trim(), TblFontBold));
                                    col1Header.HorizontalAlignment = Element.ALIGN_CENTER;
                                    col1Header.VerticalAlignment = Element.ALIGN_MIDDLE;
                                    col1Header.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                    col1Header.Border = iTextSharp.text.Rectangle.BOX;
                                    col1Header.Colspan = 2;
                                    col1Header.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                    table.AddCell(col1Header);
                                }

                                if (dr.Cells["gvHeader2"].Value.ToString() != "")
                                {
                                    //if (string.IsNullOrEmpty(dr["gvQuesDesc"].Value.ToString().Trim()))
                                    //{
                                    //    PdfPCell Descspace = new PdfPCell(new Phrase(" " + dr["gvQuesDesc"].Value.ToString().Trim(), TableFont));
                                    //    Descspace.HorizontalAlignment = Element.ALIGN_LEFT;
                                    //    Descspace.VerticalAlignment = Element.ALIGN_MIDDLE;
                                    //    Descspace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    //    Descspace.Colspan = 2;
                                    //    table.AddCell(Descspace);
                                    //}
                                    //else
                                    //{
                                    //    PdfPCell Descspace;
                                    //    if (dr.Cells["Column0"].Value.ToString() == "000")
                                    //        Descspace = new PdfPCell(new Phrase(" " + dr["gvQuesDesc"].Value.ToString().Trim(), TblFontBold));
                                    //    else
                                    //        Descspace = new PdfPCell(new Phrase("   " + dr["gvQuesDesc"].Value.ToString().Trim(), TableFont));
                                    //    Descspace.HorizontalAlignment = Element.ALIGN_LEFT;
                                    //    Descspace.VerticalAlignment = Element.ALIGN_MIDDLE;
                                    //    Descspace.Border = iTextSharp.text.Rectangle.BOX;
                                    //    Descspace.Colspan = 2;
                                    //    Descspace.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                    //    table.AddCell(Descspace);
                                    //}

                                    PdfPCell col2Header = new PdfPCell(new Phrase(" " + dr["gvHeader2"].Value.ToString().Trim(), TblFontBold));
                                    col2Header.HorizontalAlignment = Element.ALIGN_CENTER;
                                    col2Header.VerticalAlignment = Element.ALIGN_MIDDLE;
                                    col2Header.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                    col2Header.Border = iTextSharp.text.Rectangle.BOX;
                                    col2Header.Colspan = 2;
                                    col2Header.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                    table.AddCell(col2Header);
                                }
                            }
                            else
                            {
                                PdfPCell col1Header = new PdfPCell(new Phrase(" " + dr["gvHeader1"].Value.ToString().Trim(), TblFontBold));
                                col1Header.HorizontalAlignment = Element.ALIGN_CENTER;
                                col1Header.VerticalAlignment = Element.ALIGN_MIDDLE;
                                col1Header.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                col1Header.Border = iTextSharp.text.Rectangle.BOX;
                                col1Header.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                table.AddCell(col1Header);

                                PdfPCell col2Header = new PdfPCell(new Phrase(" " + dr["gvHeader2"].Value.ToString().Trim(), TblFontBold));
                                col2Header.HorizontalAlignment = Element.ALIGN_CENTER;
                                col2Header.VerticalAlignment = Element.ALIGN_MIDDLE;
                                col2Header.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                col2Header.Border = iTextSharp.text.Rectangle.BOX;
                                col2Header.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                table.AddCell(col2Header);
                            }
                        }
                        isPrevHeader1 = dr.Cells["gvHeader1"].Value.ToString();
                        isPrevHeader2 = dr.Cells["gvHeader2"].Value.ToString();
                    }
                }

                if (table.Rows.Count > 0)
                {
                    document.Add(table);
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;

                document.Add(new Paragraph("Aborted due to Exception!!"));
            }

            document.Close();
            fs.Close();
            fs.Dispose();
            AlertBox.Show("Report Generated Successfully");

            PdfViewerNewForm objfrm = new PdfViewerNewForm(PdfName);
            objfrm.StartPosition = FormStartPosition.CenterScreen;
            objfrm.ShowDialog();*/
        }

        private void PIR_Counts_PDF()
        {
            PdfName = "PIR_Counts_Report";

            PdfName = propReportPath + _baseform.UserID + "\\" + PdfName;

            try
            {
                if (!Directory.Exists(propReportPath + _baseform.UserID.Trim()))
                {
                    DirectoryInfo di = Directory.CreateDirectory(propReportPath + _baseform.UserID.Trim());
                }
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

            Document document = new Document(PageSize.A4, 30f, 30f, 30f, 50f);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();
            BaseFont bf_calibri = BaseFont.CreateFont("c:/windows/fonts/Calibri.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);

            BaseFont bf_Times = BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1250, false);
            cb = writer.DirectContent;

            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, false);
            BaseFont bfTimesBold = BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1250, false);
            iTextSharp.text.Font fc = new iTextSharp.text.Font(bfTimes, 12, 2);
            iTextSharp.text.Font fc1 = new iTextSharp.text.Font(bfTimes, 8, 2, BaseColor.BLUE);
            iTextSharp.text.Font fcRed = new iTextSharp.text.Font(bf_calibri, 10, 1, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#806000")));

            iTextSharp.text.Font Times = new iTextSharp.text.Font(bfTimes, 7);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_calibri, 8);
            iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bfTimes, 8, 3);
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bfTimes, 7, 1);
            iTextSharp.text.Font TblFontHeadBold = new iTextSharp.text.Font(bfTimes, 12, 1);
            iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bfTimes, 8, 2);
            iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bfTimes, 9, 4);

            //HeaderPage(document, writer);

            PrintHeaderPage_New_Format(document, writer);

            try
            {
                document.NewPage();

                PdfPTable table = new PdfPTable(3);
                table.WidthPercentage = 100;
                table.LockedWidth = true;
                table.TotalWidth = 500f;
                float[] widths = new float[] { 300f, 75f, 75f };
                table.SetWidths(widths);
                table.HorizontalAlignment = Element.ALIGN_CENTER;

                string isPrevHeader1 = string.Empty, isPrevHeader2 = string.Empty;
                string isPrevBold = string.Empty;

                foreach (DataGridViewRow dr in dgvQuestions.Rows)
                {
                    if (dr.Cells["gvisHeader"].Value.ToString() != "H")
                    {
                        if (dr.Cells["gvHeader1"].Value.ToString() != "" && dr.Cells["gvHeader2"].Value.ToString() != "")
                        {
                            if (dr.Cells["Column0"].Value.ToString() == "000")
                            {
                                PdfPCell Quesdec = new PdfPCell(new Phrase(" " + dr["gvQuesDesc"].Value.ToString().Trim(), TblFontBold));
                                Quesdec.HorizontalAlignment = Element.ALIGN_LEFT;
                                Quesdec.VerticalAlignment = Element.ALIGN_MIDDLE;
                                Quesdec.Border = iTextSharp.text.Rectangle.BOX;
                                Quesdec.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                table.AddCell(Quesdec);

                                PdfPCell col1Count = new PdfPCell(new Phrase(" " + dr["gvHeader1"].Value.ToString().Trim(), TableFont));
                                col1Count.HorizontalAlignment = Element.ALIGN_CENTER;
                                col1Count.VerticalAlignment = Element.ALIGN_MIDDLE;
                                col1Count.Border = iTextSharp.text.Rectangle.BOX;
                                col1Count.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                table.AddCell(col1Count);

                                PdfPCell col2Count = new PdfPCell(new Phrase(" " + dr["gvHeader2"].Value.ToString().Trim(), TableFont));
                                col2Count.HorizontalAlignment = Element.ALIGN_CENTER;
                                col2Count.VerticalAlignment = Element.ALIGN_MIDDLE;
                                col2Count.Border = iTextSharp.text.Rectangle.BOX;
                                col2Count.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                table.AddCell(col2Count);
                            }
                            else
                            {
                                if (dr["gvQID"].Value.ToString() == "6" || dr["gvQID"].Value.ToString() == "25" || dr["gvQID"].Value.ToString() == "104")
                                {
                                    PdfPCell Quesdec = new PdfPCell(new Phrase("   " + dr["gvQuesDesc"].Value.ToString().Trim(), TableFont));
                                    Quesdec.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Quesdec.VerticalAlignment = Element.ALIGN_MIDDLE;
                                    Quesdec.Border = iTextSharp.text.Rectangle.BOX;
                                    Quesdec.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                    if (dr["gvQID"].Value.ToString() != "104")
                                        Quesdec.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fafafa"));
                                    Quesdec.Colspan = 3;
                                    table.AddCell(Quesdec);
                                }
                                else
                                {
                                    PdfPCell Quesdec = new PdfPCell(new Phrase("   " + dr["gvQuesDesc"].Value.ToString().Trim(), TableFont));
                                    Quesdec.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Quesdec.VerticalAlignment = Element.ALIGN_MIDDLE;
                                    Quesdec.Border = iTextSharp.text.Rectangle.BOX;
                                    Quesdec.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                    table.AddCell(Quesdec);

                                    PdfPCell col1Count = new PdfPCell(new Phrase(" " + dr["gvHeader1"].Value.ToString().Trim(), TableFont));
                                    col1Count.HorizontalAlignment = Element.ALIGN_CENTER;
                                    col1Count.VerticalAlignment = Element.ALIGN_MIDDLE;
                                    col1Count.Border = iTextSharp.text.Rectangle.BOX;
                                    col1Count.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                    table.AddCell(col1Count);

                                    PdfPCell col2Count = new PdfPCell(new Phrase(" " + dr["gvHeader2"].Value.ToString().Trim(), TableFont));
                                    col2Count.HorizontalAlignment = Element.ALIGN_CENTER;
                                    col2Count.VerticalAlignment = Element.ALIGN_MIDDLE;
                                    col2Count.Border = iTextSharp.text.Rectangle.BOX;
                                    col2Count.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                    table.AddCell(col2Count);
                                }
                            }

                        }

                        else if ((dr.Cells["gvHeader1"].Value.ToString() != "" || dr.Cells["gvHeader1"].Value.ToString() != "0") && dr.Cells["gvHeader2"].Value.ToString() == "")
                        {
                            if (dr.Cells["Column0"].Value.ToString() == "000")
                            {
                                PdfPCell Quesdec = new PdfPCell(new Phrase(" " + dr["gvQuesDesc"].Value.ToString().Trim(), TblFontBold));
                                Quesdec.HorizontalAlignment = Element.ALIGN_LEFT;
                                Quesdec.VerticalAlignment = Element.ALIGN_MIDDLE;
                                Quesdec.Border = iTextSharp.text.Rectangle.BOX;
                                Quesdec.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                Quesdec.Colspan = 2;
                                table.AddCell(Quesdec);

                                PdfPCell col1Count = new PdfPCell(new Phrase(" " + dr["gvHeader1"].Value.ToString().Trim(), TableFont));
                                col1Count.HorizontalAlignment = Element.ALIGN_CENTER;
                                col1Count.VerticalAlignment = Element.ALIGN_MIDDLE;
                                col1Count.Border = iTextSharp.text.Rectangle.BOX;
                                col1Count.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                //col1Count.Colspan = 2;
                                table.AddCell(col1Count);
                            }
                            else
                            {
                                if (dr["gvQID"].Value.ToString() == "6" || dr["gvQID"].Value.ToString() == "25" || dr["gvQID"].Value.ToString() == "104")
                                {
                                    PdfPCell Quesdec = new PdfPCell(new Phrase("   " + dr["gvQuesDesc"].Value.ToString().Trim(), TableFont));
                                    Quesdec.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Quesdec.VerticalAlignment = Element.ALIGN_MIDDLE;
                                    Quesdec.Border = iTextSharp.text.Rectangle.BOX;
                                    Quesdec.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                    if(dr["gvQID"].Value.ToString() != "104")
                                        Quesdec.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fafafa"));
                                    Quesdec.Colspan = 3;
                                    table.AddCell(Quesdec);
                                }
                                else
                                {
                                    PdfPCell Quesdec = new PdfPCell(new Phrase("   " + dr["gvQuesDesc"].Value.ToString().Trim(), TableFont));
                                    Quesdec.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Quesdec.VerticalAlignment = Element.ALIGN_MIDDLE;
                                    Quesdec.Border = iTextSharp.text.Rectangle.BOX;
                                    Quesdec.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                    Quesdec.Colspan = 2;
                                    table.AddCell(Quesdec);

                                    PdfPCell col1Count = new PdfPCell(new Phrase(" " + dr["gvHeader1"].Value.ToString().Trim(), TableFont));
                                    col1Count.HorizontalAlignment = Element.ALIGN_CENTER;
                                    col1Count.VerticalAlignment = Element.ALIGN_MIDDLE;
                                    col1Count.Border = iTextSharp.text.Rectangle.BOX;
                                    col1Count.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                    table.AddCell(col1Count);
                                }
                            }
                        }

                        else if ((dr.Cells["gvHeader2"].Value.ToString() != "" || dr.Cells["gvHeader2"].Value.ToString() != "0") && dr.Cells["gvHeader1"].Value.ToString() == "")
                        {
                            if (dr.Cells["Column0"].Value.ToString() == "000")
                            {
                                PdfPCell Quesdec = new PdfPCell(new Phrase(" " + dr["gvQuesDesc"].Value.ToString().Trim(), TblFontBold));
                                Quesdec.HorizontalAlignment = Element.ALIGN_LEFT;
                                Quesdec.VerticalAlignment = Element.ALIGN_MIDDLE;
                                Quesdec.Border = iTextSharp.text.Rectangle.BOX;
                                Quesdec.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                Quesdec.Colspan = 2;
                                table.AddCell(Quesdec);

                                PdfPCell col2Count = new PdfPCell(new Phrase(" " + dr["gvHeader2"].Value.ToString().Trim(), TableFont));
                                col2Count.HorizontalAlignment = Element.ALIGN_CENTER;
                                col2Count.VerticalAlignment = Element.ALIGN_MIDDLE;
                                col2Count.Border = iTextSharp.text.Rectangle.BOX;
                                col2Count.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                //col2Count.Colspan = 2;
                                table.AddCell(col2Count);
                            }
                            else
                            {
                                if (dr["gvQID"].Value.ToString() == "6" || dr["gvQID"].Value.ToString() == "25" || dr["gvQID"].Value.ToString() == "104")
                                {
                                    PdfPCell Quesdec = new PdfPCell(new Phrase("   " + dr["gvQuesDesc"].Value.ToString().Trim(), TableFont));
                                    Quesdec.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Quesdec.VerticalAlignment = Element.ALIGN_MIDDLE;
                                    Quesdec.Border = iTextSharp.text.Rectangle.BOX;
                                    Quesdec.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                    if (dr["gvQID"].Value.ToString() != "104")
                                        Quesdec.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fafafa"));
                                    Quesdec.Colspan = 3;
                                    table.AddCell(Quesdec);
                                }
                                else
                                {
                                    PdfPCell Quesdec = new PdfPCell(new Phrase("   " + dr["gvQuesDesc"].Value.ToString().Trim(), TableFont));
                                    Quesdec.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Quesdec.VerticalAlignment = Element.ALIGN_MIDDLE;
                                    Quesdec.Border = iTextSharp.text.Rectangle.BOX;
                                    Quesdec.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                    Quesdec.Colspan = 2;
                                    table.AddCell(Quesdec);

                                    PdfPCell col2Count = new PdfPCell(new Phrase(" " + dr["gvHeader2"].Value.ToString().Trim(), TableFont));
                                    col2Count.HorizontalAlignment = Element.ALIGN_CENTER;
                                    col2Count.VerticalAlignment = Element.ALIGN_MIDDLE;
                                    col2Count.Border = iTextSharp.text.Rectangle.BOX;
                                    col2Count.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                    table.AddCell(col2Count);
                                }
                            }
                        }

                        else
                        {
                            if (dr.Cells["Column0"].Value.ToString() == "000")
                            {
                                if ((dr.Cells["Column1"].Value.ToString() == "0" || dr.Cells["Column1"].Value.ToString().Trim() == "") && (dr.Cells["Column2"].Value.ToString() == "0" || dr.Cells["Column2"].Value.ToString().Trim() == ""))
                                {
                                    PdfPCell Quesdec = new PdfPCell(new Phrase(" " + dr["gvQuesDesc"].Value.ToString().Trim(), TblFontBold));
                                    Quesdec.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Quesdec.VerticalAlignment = Element.ALIGN_MIDDLE;
                                    Quesdec.Border = iTextSharp.text.Rectangle.BOX;
                                    Quesdec.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                    Quesdec.Colspan = 2;
                                    table.AddCell(Quesdec);

                                    PdfPCell col1Count = new PdfPCell(new Phrase("", TableFont));
                                    col1Count.HorizontalAlignment = Element.ALIGN_CENTER;
                                    col1Count.VerticalAlignment = Element.ALIGN_MIDDLE;
                                    col1Count.Border = iTextSharp.text.Rectangle.BOX;
                                    col1Count.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                    //col1Count.Colspan = 2;
                                    table.AddCell(col1Count);
                                }
                                else
                                {
                                    PdfPCell Quesdec = new PdfPCell(new Phrase(" " + dr["gvQuesDesc"].Value.ToString().Trim(), TblFontBold));
                                    Quesdec.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Quesdec.VerticalAlignment = Element.ALIGN_MIDDLE;
                                    Quesdec.Border = iTextSharp.text.Rectangle.BOX;
                                    Quesdec.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                    table.AddCell(Quesdec);

                                    PdfPCell col1Count = new PdfPCell(new Phrase("", TableFont));
                                    col1Count.HorizontalAlignment = Element.ALIGN_CENTER;
                                    col1Count.VerticalAlignment = Element.ALIGN_MIDDLE;
                                    col1Count.Border = iTextSharp.text.Rectangle.BOX;
                                    col1Count.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                    table.AddCell(col1Count);

                                    PdfPCell col2Count = new PdfPCell(new Phrase("", TableFont));
                                    col2Count.HorizontalAlignment = Element.ALIGN_CENTER;
                                    col2Count.VerticalAlignment = Element.ALIGN_MIDDLE;
                                    col2Count.Border = iTextSharp.text.Rectangle.BOX;
                                    col2Count.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                    table.AddCell(col2Count);
                                }
                            }
                            else
                            {
                                if ((dr.Cells["Column1"].Value.ToString() == "0" || dr.Cells["Column1"].Value.ToString().Trim() == "") && (dr.Cells["Column2"].Value.ToString() == "0" || dr.Cells["Column2"].Value.ToString().Trim() == ""))
                                {
                                    if (dr["gvQID"].Value.ToString() == "104")
                                    {
                                        PdfPCell Quesdec = new PdfPCell(new Phrase("     " + dr["gvQuesDesc"].Value.ToString().Trim(), TableFont));
                                        Quesdec.HorizontalAlignment = Element.ALIGN_LEFT;
                                        Quesdec.VerticalAlignment = Element.ALIGN_MIDDLE;
                                        Quesdec.Border = iTextSharp.text.Rectangle.BOX;
                                        Quesdec.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                        Quesdec.Colspan = 3;
                                        table.AddCell(Quesdec);
                                    }
                                    else
                                    {
                                        PdfPCell Quesdec = new PdfPCell(new Phrase("   " + dr["gvQuesDesc"].Value.ToString().Trim(), TableFont));
                                        Quesdec.HorizontalAlignment = Element.ALIGN_LEFT;
                                        Quesdec.VerticalAlignment = Element.ALIGN_MIDDLE;
                                        Quesdec.Border = iTextSharp.text.Rectangle.BOX;
                                        Quesdec.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                        Quesdec.Colspan = 2;
                                        table.AddCell(Quesdec);

                                        PdfPCell col1Count = new PdfPCell(new Phrase("", TableFont));
                                        col1Count.HorizontalAlignment = Element.ALIGN_CENTER;
                                        col1Count.VerticalAlignment = Element.ALIGN_MIDDLE;
                                        col1Count.Border = iTextSharp.text.Rectangle.BOX;
                                        col1Count.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                        table.AddCell(col1Count);
                                    }
                                }
                                else
                                {
                                    PdfPCell Quesdec = new PdfPCell(new Phrase("   " + dr["gvQuesDesc"].Value.ToString().Trim(), TableFont));
                                    Quesdec.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Quesdec.VerticalAlignment = Element.ALIGN_MIDDLE;
                                    Quesdec.Border = iTextSharp.text.Rectangle.BOX;
                                    Quesdec.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                    table.AddCell(Quesdec);

                                    PdfPCell col1Count = new PdfPCell(new Phrase("", TableFont));
                                    col1Count.HorizontalAlignment = Element.ALIGN_CENTER;
                                    col1Count.VerticalAlignment = Element.ALIGN_MIDDLE;
                                    col1Count.Border = iTextSharp.text.Rectangle.BOX;
                                    col1Count.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                    table.AddCell(col1Count);

                                    PdfPCell col2Count = new PdfPCell(new Phrase("", TableFont));
                                    col2Count.HorizontalAlignment = Element.ALIGN_CENTER;
                                    col2Count.VerticalAlignment = Element.ALIGN_MIDDLE;
                                    col2Count.Border = iTextSharp.text.Rectangle.BOX;
                                    col2Count.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                    table.AddCell(col2Count);
                                }
                            }
                        }
                    }
                    else
                    {
                        if ((isPrevHeader1 != dr.Cells["gvHeader1"].Value.ToString() || isPrevHeader2 != dr.Cells["gvHeader2"].Value.ToString() || isPrevBold != "Y" || isPrevBold != ""))
                        {
                            if (dr.Cells["Column0"].Value.ToString() == "" || dr.Cells["Column0"].Value.ToString() == "000")
                            {
                                PdfPCell QuesSpaces = new PdfPCell(new Phrase("", TableFont));
                                QuesSpaces.HorizontalAlignment = Element.ALIGN_LEFT;
                                QuesSpaces.VerticalAlignment = Element.ALIGN_MIDDLE;
                                QuesSpaces.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                QuesSpaces.Colspan = 3;
                                QuesSpaces.FixedHeight = 15;
                                table.AddCell(QuesSpaces);
                            }

                            if (dr.Cells["gvHeader1"].Value.ToString() != "" && dr.Cells["gvHeader2"].Value.ToString() != "")
                            {
                                if (string.IsNullOrEmpty(dr["gvQuesDesc"].Value.ToString().Trim()))
                                {
                                    PdfPCell Descspace = new PdfPCell(new Phrase(" " + dr["gvQuesDesc"].Value.ToString().Trim(), TableFont));
                                    Descspace.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Descspace.VerticalAlignment = Element.ALIGN_MIDDLE;
                                    Descspace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(Descspace);
                                }
                                else
                                {
                                    PdfPCell Descspace;
                                    if (dr.Cells["Column0"].Value.ToString() == "000")
                                        Descspace = new PdfPCell(new Phrase(" " + dr["gvQuesDesc"].Value.ToString().Trim(), TblFontBold));
                                    else
                                        Descspace = new PdfPCell(new Phrase("   " + dr["gvQuesDesc"].Value.ToString().Trim(), TableFont));
                                    Descspace.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Descspace.VerticalAlignment = Element.ALIGN_MIDDLE;
                                    Descspace.Border = iTextSharp.text.Rectangle.BOX;
                                    Descspace.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                    table.AddCell(Descspace);
                                }

                                PdfPCell col1Header = new PdfPCell(new Phrase(" " + dr["gvHeader1"].Value.ToString().Trim(), TblFontBold));
                                col1Header.HorizontalAlignment = Element.ALIGN_CENTER;
                                col1Header.VerticalAlignment = Element.ALIGN_MIDDLE;
                                col1Header.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                col1Header.Border = iTextSharp.text.Rectangle.BOX;
                                col1Header.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                table.AddCell(col1Header);

                                PdfPCell col2Header = new PdfPCell(new Phrase(" " + dr["gvHeader2"].Value.ToString().Trim(), TblFontBold));
                                col2Header.HorizontalAlignment = Element.ALIGN_CENTER;
                                col2Header.VerticalAlignment = Element.ALIGN_MIDDLE;
                                col2Header.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                col2Header.Border = iTextSharp.text.Rectangle.BOX;
                                col2Header.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                table.AddCell(col2Header);
                            }
                            else if (dr.Cells["gvHeader1"].Value.ToString() != "" && dr.Cells["gvHeader2"].Value.ToString() == "")
                            {
                                if (string.IsNullOrEmpty(dr["gvQuesDesc"].Value.ToString().Trim()))
                                {
                                    PdfPCell Descspace = new PdfPCell(new Phrase(" " + dr["gvQuesDesc"].Value.ToString().Trim(), TableFont));
                                    Descspace.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Descspace.VerticalAlignment = Element.ALIGN_MIDDLE;
                                    Descspace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    Descspace.Colspan = 2;
                                    table.AddCell(Descspace);
                                }
                                else
                                {
                                    PdfPCell Descspace;
                                    if (dr.Cells["Column0"].Value.ToString() == "000")
                                        Descspace = new PdfPCell(new Phrase(" " + dr["gvQuesDesc"].Value.ToString().Trim(), TblFontBold));
                                    else
                                        Descspace = new PdfPCell(new Phrase("   " + dr["gvQuesDesc"].Value.ToString().Trim(), TableFont));
                                    Descspace.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Descspace.VerticalAlignment = Element.ALIGN_MIDDLE;
                                    Descspace.Border = iTextSharp.text.Rectangle.BOX;
                                    Descspace.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                    Descspace.Colspan = 2;
                                    table.AddCell(Descspace);
                                }

                                PdfPCell col1Header = new PdfPCell(new Phrase(" " + dr["gvHeader1"].Value.ToString().Trim(), TblFontBold));
                                col1Header.HorizontalAlignment = Element.ALIGN_CENTER;
                                col1Header.VerticalAlignment = Element.ALIGN_MIDDLE;
                                col1Header.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                col1Header.Border = iTextSharp.text.Rectangle.BOX;
                                //col1Header.Colspan = 2;
                                col1Header.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                table.AddCell(col1Header);
                            }
                            else if (dr.Cells["gvHeader2"].Value.ToString() != "" && dr.Cells["gvHeader1"].Value.ToString() == "")
                            {
                                if (string.IsNullOrEmpty(dr["gvQuesDesc"].Value.ToString().Trim()))
                                {
                                    PdfPCell Descspace = new PdfPCell(new Phrase(" " + dr["gvQuesDesc"].Value.ToString().Trim(), TableFont));
                                    Descspace.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Descspace.VerticalAlignment = Element.ALIGN_MIDDLE;
                                    Descspace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    Descspace.Colspan = 2;
                                    table.AddCell(Descspace);
                                }
                                else
                                {
                                    PdfPCell Descspace;
                                    if (dr.Cells["Column0"].Value.ToString() == "000")
                                        Descspace = new PdfPCell(new Phrase(" " + dr["gvQuesDesc"].Value.ToString().Trim(), TblFontBold));
                                    else
                                        Descspace = new PdfPCell(new Phrase("   " + dr["gvQuesDesc"].Value.ToString().Trim(), TableFont));
                                    Descspace.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Descspace.VerticalAlignment = Element.ALIGN_MIDDLE;
                                    Descspace.Border = iTextSharp.text.Rectangle.BOX;
                                    Descspace.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                    Descspace.Colspan = 2;
                                    table.AddCell(Descspace);
                                }

                                PdfPCell col2Header = new PdfPCell(new Phrase(" " + dr["gvHeader2"].Value.ToString().Trim(), TblFontBold));
                                col2Header.HorizontalAlignment = Element.ALIGN_CENTER;
                                col2Header.VerticalAlignment = Element.ALIGN_MIDDLE;
                                col2Header.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                col2Header.Border = iTextSharp.text.Rectangle.BOX;
                                //col2Header.Colspan = 2;
                                col2Header.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                table.AddCell(col2Header);
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(dr["gvQuesDesc"].Value.ToString().Trim()))
                                {
                                    PdfPCell Descspace = new PdfPCell(new Phrase(" " + dr["gvQuesDesc"].Value.ToString().Trim(), TableFont));
                                    Descspace.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Descspace.VerticalAlignment = Element.ALIGN_MIDDLE;
                                    Descspace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(Descspace);
                                }
                                else
                                {
                                    PdfPCell Descspace;
                                    if (dr.Cells["Column0"].Value.ToString() == "000")
                                        Descspace = new PdfPCell(new Phrase(" " + dr["gvQuesDesc"].Value.ToString().Trim(), TblFontBold));
                                    else
                                        Descspace = new PdfPCell(new Phrase("   " + dr["gvQuesDesc"].Value.ToString().Trim(), TableFont));
                                    Descspace.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Descspace.VerticalAlignment = Element.ALIGN_MIDDLE;
                                    Descspace.Border = iTextSharp.text.Rectangle.BOX;
                                    Descspace.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                    table.AddCell(Descspace);
                                }

                                PdfPCell col1Header = new PdfPCell(new Phrase("", TblFontBold));
                                col1Header.HorizontalAlignment = Element.ALIGN_CENTER;
                                col1Header.VerticalAlignment = Element.ALIGN_MIDDLE;
                                col1Header.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                col1Header.Border = iTextSharp.text.Rectangle.BOX;
                                col1Header.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                table.AddCell(col1Header);

                                PdfPCell col2Header = new PdfPCell(new Phrase("", TblFontBold));
                                col2Header.HorizontalAlignment = Element.ALIGN_CENTER;
                                col2Header.VerticalAlignment = Element.ALIGN_MIDDLE;
                                col2Header.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                col2Header.Border = iTextSharp.text.Rectangle.BOX;
                                col2Header.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#e8e8e8"));
                                table.AddCell(col2Header);
                            }
                        }

                        isPrevHeader1 = dr.Cells["gvHeader1"].Value.ToString();
                        isPrevHeader2 = dr.Cells["gvHeader2"].Value.ToString();

                        isPrevBold = dr.Cells["Column3"].Value.ToString();
                    }
                }

                if (table.Rows.Count > 0)
                {
                    document.Add(table);
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;

                document.Add(new Paragraph("Aborted due to Exception!!"));
            }

            document.Close();
            fs.Close();
            fs.Dispose();
            AlertBox.Show("Report Generated Successfully");

            PdfViewerNewForm objfrm = new PdfViewerNewForm(PdfName);
            objfrm.StartPosition = FormStartPosition.CenterScreen;
            objfrm.ShowDialog();
        }

        private void HeaderPage(Document document, PdfWriter writer)
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

            HierarchyEntity hierarchyDetails = _model.HierarchyAndPrograms.GetCaseHierarchy("AGENCY", _baseform.BaseAdminAgency, string.Empty, string.Empty, string.Empty, string.Empty);
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
            float[] Headerwidths = new float[] { 25f, 100f };
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

            PdfPCell row1 = new PdfPCell(new Phrase(_priviliges.Program + " - " + _priviliges.PrivilegeName, TblHeaderTitleFont));
            row1.HorizontalAlignment = Element.ALIGN_CENTER;
            row1.Colspan = 2;
            row1.PaddingBottom = 15;
            row1.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Headertable.AddCell(row1);

            PdfPCell row3 = new PdfPCell(new Phrase("Selected Report Parameters", TblParamsHeaderFont));
            row3.HorizontalAlignment = Element.ALIGN_CENTER;
            row3.VerticalAlignment = PdfPCell.ALIGN_TOP;
            row3.PaddingBottom = 5;
            row3.MinimumHeight = 6;
            row3.Colspan = 2;
            row3.Border = iTextSharp.text.Rectangle.NO_BORDER;
            row3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#b8e9fb"));
            Headertable.AddCell(row3);

            string Agy = "All";
            string Dept = "All";
            string Prg = "All";
            string Header_year = string.Empty;
            if (Agency != "**")
                Agy = Agency;
            if (Depart != "**")
                Dept = Depart;
            if (Program != "**")
                Prg = Program;
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

            PdfPCell cell_Content_Title2 = new PdfPCell(new Phrase("  " + lblSection.Text.Trim(), TableFont));
            cell_Content_Title2.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_Content_Title2.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            cell_Content_Title2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            cell_Content_Title2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            cell_Content_Title2.PaddingBottom = 5;
            Headertable.AddCell(cell_Content_Title2);

            PdfPCell cell_Content_Desc2 = new PdfPCell(new Phrase(((Captain.Common.Utilities.ListItem)Cmb_Section.SelectedItem).Text.ToString(), TableFont));
            cell_Content_Desc2.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_Content_Desc2.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            cell_Content_Desc2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            cell_Content_Desc2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            cell_Content_Desc2.PaddingBottom = 5;
            Headertable.AddCell(cell_Content_Desc2);

            PdfPCell cell_Content_Title3 = new PdfPCell(new Phrase("  " + lblFund.Text.Trim(), TableFont));
            cell_Content_Title3.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_Content_Title3.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            cell_Content_Title3.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            cell_Content_Title3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            cell_Content_Title3.PaddingBottom = 5;
            Headertable.AddCell(cell_Content_Title3);

            PdfPCell cell_Content_Desc3 = new PdfPCell(new Phrase(((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString(), TableFont));
            cell_Content_Desc3.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_Content_Desc3.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            cell_Content_Desc3.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            cell_Content_Desc3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            cell_Content_Desc3.PaddingBottom = 5;
            Headertable.AddCell(cell_Content_Desc3);

            PdfPCell cell_Content_Title4 = new PdfPCell(new Phrase("  " + lblSite.Text.Trim(), TableFont));
            cell_Content_Title4.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_Content_Title4.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            cell_Content_Title4.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            cell_Content_Title4.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            cell_Content_Title4.PaddingBottom = 5;
            Headertable.AddCell(cell_Content_Title4);

            PdfPCell cell_Content_Desc4 = new PdfPCell(new Phrase(((Captain.Common.Utilities.ListItem)Cmb_Site.SelectedItem).Text.ToString(), TableFont));
            cell_Content_Desc4.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_Content_Desc4.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            cell_Content_Desc4.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            cell_Content_Desc4.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            cell_Content_Desc4.PaddingBottom = 5;
            Headertable.AddCell(cell_Content_Desc4);

            document.Add(Headertable);

            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase("Generated By : ", fnttimesRoman_Italic), 33, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.GetMemberName(_baseform.UserProfile.FirstName.Trim(), _baseform.UserProfile.MI.Trim(), _baseform.UserProfile.LastName.Trim(), "3"), fnttimesRoman_Italic), 90, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase("Generated On : ", fnttimesRoman_Italic), 410, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase(DateTime.Now.ToString(), fnttimesRoman_Italic), 468, 40, 0);
        }

        private void PrintHeaderPage_New_Format(Document document, PdfWriter writer)
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

            //Row 1
            PdfPCell CH1 = new PdfPCell(new Phrase(lblSection.Text.Trim(), paramsCellStyle));
            CH1.HorizontalAlignment = Element.ALIGN_LEFT;
            CH1.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CH1.Border = iTextSharp.text.Rectangle.BOX;
            CH1.PaddingBottom = 5;
            CH1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEF5FC"));
            Headertable.AddCell(CH1);

            PdfPCell CB1 = new PdfPCell(new Phrase(((Captain.Common.Utilities.ListItem)Cmb_Section.SelectedItem).Text.ToString(), paramsCellStyle));
            CB1.HorizontalAlignment = Element.ALIGN_LEFT;
            CB1.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CB1.Border = iTextSharp.text.Rectangle.BOX;
            CB1.Colspan = 4;
            CB1.PaddingBottom = 5;
            CB1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            Headertable.AddCell(CB1);

            //Row 2
            PdfPCell CH1_2 = new PdfPCell(new Phrase(lblFund.Text.Trim(), paramsCellStyle));
            CH1_2.HorizontalAlignment = Element.ALIGN_LEFT;
            CH1_2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CH1_2.Border = iTextSharp.text.Rectangle.BOX;
            CH1_2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEF5FC"));
            Headertable.AddCell(CH1_2);

            PdfPCell CB1_2 = new PdfPCell(new Phrase(((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString(), paramsCellStyle));
            CB1_2.HorizontalAlignment = Element.ALIGN_LEFT;
            CB1_2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CB1_2.Border = iTextSharp.text.Rectangle.BOX;
            CB1_2.Colspan = 4;
            CB1_2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            Headertable.AddCell(CB1_2);

            //Row 3
            CH1_2 = new PdfPCell(new Phrase(lblSite.Text.Trim(), paramsCellStyle));
            CH1_2.HorizontalAlignment = Element.ALIGN_LEFT;
            CH1_2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CH1_2.Border = iTextSharp.text.Rectangle.BOX;
            CH1_2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#EEF5FC"));
            Headertable.AddCell(CH1_2);

            CB1_2 = new PdfPCell(new Phrase(((Captain.Common.Utilities.ListItem)Cmb_Site.SelectedItem).Text.ToString(), paramsCellStyle));
            CB1_2.HorizontalAlignment = Element.ALIGN_LEFT;
            CB1_2.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            CB1_2.Colspan = 4;
            CB1_2.Border = iTextSharp.text.Rectangle.BOX;
            CB1_2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            Headertable.AddCell(CB1_2);

            document.Add(Headertable);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase("Generated By: ", fnttimesRoman_Italic), 33, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.GetMemberName(_baseform.UserProfile.FirstName.Trim(), _baseform.UserProfile.MI.Trim(), _baseform.UserProfile.LastName.Trim(), "3"), fnttimesRoman_Italic), 90, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase("Generated On: ", fnttimesRoman_Italic), 410, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase(DateTime.Now.ToString(), fnttimesRoman_Italic), 468, 40, 0);
        }

        #endregion

        #region Audit Report

        private void btnAuditReport_Click(object sender, EventArgs e)
        {
            bool anyCheckboxChecked = dgvQuestions.Rows.Cast<DataGridViewRow>().Any(row => Convert.ToBoolean(row.Cells["gvSelect"].Value) == true);

            if (anyCheckboxChecked)
            {
                MessageBox.Show("Do you want to generate 'Hits On Records Only'?", Consts.Common.ApplicationCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, onclose: Onclose);
            }
            else
            {
                AlertBox.Show("Please Select any Question", MessageBoxIcon.Warning);
            }

        }

        bool Hit_On_Records = true;
        private void Onclose(DialogResult dialogResult)
        {

            DataSet dsAudit;
            string Sel_site = string.Empty;
            if (((Captain.Common.Utilities.ListItem)Cmb_Site.SelectedItem).Value.ToString() != "****")
                Sel_site = ((Captain.Common.Utilities.ListItem)Cmb_Site.SelectedItem).Value.ToString();

            string QuesID = Selected_Questions();

            if (dialogResult == DialogResult.Yes)
            {
                dsAudit = Captain.DatabaseLayer.Lookups.Browse_PIR_Audit_Report(Agency, Depart, Program, Program_Year, ((Captain.Common.Utilities.ListItem)Cmb_Section.SelectedItem).Value.ToString(), ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Value.ToString(), Sel_site, QuesID, "", _baseform.UserID, "Y", "Y");

                Hit_On_Records = true;
            }
            else
            {
                dsAudit = Captain.DatabaseLayer.Lookups.Browse_PIR_Audit_Report(Agency, Depart, Program, Program_Year, ((Captain.Common.Utilities.ListItem)Cmb_Section.SelectedItem).Value.ToString(), ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Value.ToString(), Sel_site, QuesID, "", _baseform.UserID, "Y", "N");

                Hit_On_Records = false;
            }

            if (((Captain.Common.Utilities.ListItem)Cmb_Section.SelectedItem).Value.ToString() != "A" || ((Captain.Common.Utilities.ListItem)Cmb_Section.SelectedItem).Value.ToString() != "C")
            {
                DataSet dsConditions = Captain.DatabaseLayer.Lookups.GET_QUES_AUDIT_CONDITIONS(Agency, Depart, Program, Program_Year, ((Captain.Common.Utilities.ListItem)Cmb_Section.SelectedItem).Value.ToString(), ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Value.ToString(), Sel_site, QuesID, dgvQuestions.SelectedRows[0].Cells["gvType"].Value.ToString());

                Genrate_Audit_Report_DevExp_New(dsAudit, dsConditions);
            }
            else
                Genrate_Audit_Report_DevExp_New(dsAudit, null);

        }

        private void Generate_Audit_Report(DataSet dsAudit)
        {
            string PdfName = "PIR Audit" + "_" + DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");

            PdfName = propReportPath + _baseform.UserID + "\\" + PdfName;
            try
            {
                if (!Directory.Exists(propReportPath + _baseform.UserID.Trim()))
                {
                    DirectoryInfo di = Directory.CreateDirectory(propReportPath + _baseform.UserID.Trim());
                }
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

            Workbook book = new Workbook();
            Worksheet sheet;
            sheet = book.Worksheets.Add("Params");
            WorksheetCell cell = new WorksheetCell();

            this.GenerateStyles(book.Styles);

            #region Styles

            WorksheetStyle mainstyle = book.Styles.Add("MainHeaderStyles");
            mainstyle.Font.FontName = "Calibri";
            mainstyle.Font.Size = 11;
            mainstyle.Font.Bold = true;
            mainstyle.Font.Color = "#2c2c2c";
            mainstyle.Interior.Color = "#d2e1ef";
            mainstyle.Interior.Pattern = StyleInteriorPattern.Solid;
            mainstyle.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            mainstyle.Alignment.Vertical = StyleVerticalAlignment.Center;
            mainstyle.Alignment.WrapText = true;

            WorksheetStyle mainstyleCenter = book.Styles.Add("MainHeaderStylesC");
            mainstyleCenter.Font.FontName = "Calibri";
            mainstyleCenter.Font.Size = 11;
            mainstyleCenter.Font.Bold = true;
            mainstyleCenter.Font.Color = "#2c2c2c";
            mainstyleCenter.Interior.Color = "#d2e1ef";
            mainstyleCenter.Interior.Pattern = StyleInteriorPattern.Solid;
            mainstyleCenter.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            mainstyleCenter.Alignment.Vertical = StyleVerticalAlignment.Center;
            mainstyleCenter.Alignment.WrapText = true;

            WorksheetStyle mainstyleRight = book.Styles.Add("MainHeaderStylesR");
            mainstyleRight.Font.FontName = "Calibri";
            mainstyleRight.Font.Size = 11;
            mainstyleRight.Font.Bold = true;
            mainstyleRight.Font.Color = "#2c2c2c";
            mainstyleRight.Interior.Color = "#d2e1ef";
            mainstyleRight.Interior.Pattern = StyleInteriorPattern.Solid;
            mainstyleRight.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            mainstyleRight.Alignment.Vertical = StyleVerticalAlignment.Center;
            mainstyleRight.Alignment.WrapText = true;

            WorksheetStyle mainstyle2 = book.Styles.Add("MainHeaderStyles2");
            mainstyle2.Font.FontName = "Calibri";
            mainstyle2.Font.Size = 12;
            mainstyle2.Font.Bold = true;
            mainstyle2.Font.Color = "#FFFFFF";
            mainstyle2.Interior.Color = "#0070c0";
            mainstyle2.Interior.Pattern = StyleInteriorPattern.Solid;
            mainstyle2.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            mainstyle2.Alignment.Vertical = StyleVerticalAlignment.Center;

            WorksheetStyle style1 = book.Styles.Add("Normal");
            style1.Font.FontName = "Calibri";
            style1.Font.Size = 10;
            style1.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            style1.Alignment.Vertical = StyleVerticalAlignment.Center;

            WorksheetStyle stylecenter = book.Styles.Add("Normalcenter");
            stylecenter.Font.FontName = "Calibri";
            stylecenter.Font.Size = 10;
            stylecenter.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            stylecenter.Alignment.Vertical = StyleVerticalAlignment.Center;

            WorksheetStyle style3 = book.Styles.Add("NormalLeft");
            style3.Font.FontName = "Calibri";
            style3.Font.Size = 10;
            style3.Interior.Color = "#f2f2f2";
            style3.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            style3.Alignment.Vertical = StyleVerticalAlignment.Center;
            style3.Alignment.WrapText = true;

            WorksheetStyle style4 = book.Styles.Add("NormalLeftRed");
            style4.Font.FontName = "Calibri";
            style4.Font.Size = 10;
            style4.Interior.Color = "#f2f2f2";
            style4.Font.Color = "#f00808";
            style4.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            style4.Alignment.Vertical = StyleVerticalAlignment.Center;

            WorksheetStyle style5 = book.Styles.Add("NormalRight");
            style5.Font.FontName = "Calibri";
            style5.Font.Size = 10;
            style5.Interior.Color = "#f2f2f2";
            style5.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            style5.Alignment.Vertical = StyleVerticalAlignment.Center;

            WorksheetStyle style55 = book.Styles.Add("NormalRightServTot");
            style55.Font.FontName = "Calibri";
            style55.Font.Size = 10;
            style55.Font.Bold = true;
            style55.Interior.Color = "#ffe8bd";
            style55.Interior.Pattern = StyleInteriorPattern.Solid;
            style55.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            style55.Alignment.Vertical = StyleVerticalAlignment.Center;

            WorksheetStyle style555 = book.Styles.Add("NormalRightGrpTot");
            style555.Font.FontName = "Calibri";
            style555.Font.Size = 10;
            style555.Font.Bold = true;
            style555.Interior.Color = "#e0ebd8";
            style555.Interior.Pattern = StyleInteriorPattern.Solid;
            style555.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            style555.Alignment.Vertical = StyleVerticalAlignment.Center;

            WorksheetStyle style6 = book.Styles.Add("NormalRightRed");
            style6.Font.FontName = "Calibri";
            style6.Font.Size = 10;
            style6.Interior.Color = "#f2f2f2";
            style6.Font.Color = "#f00808";
            style6.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            style6.Alignment.Vertical = StyleVerticalAlignment.Center;

            #endregion

            PrintHeaderPage(sheet, cell);

            DataSet ds = dsAudit;
            DataTable dtCounts = new DataTable();
            if (ds.Tables.Count > 3)
                dtCounts = ds.Tables[3];
            else
                dtCounts = ds.Tables[2];

            if (dtCounts.Rows.Count > 0)
            {
                if (dtCounts.Rows[0]["ColHead1"].ToString() != "" && dtCounts.Rows[0]["ColHead1"].ToString() != "0.00")
                {
                    DataTable dtCol1Audit = ds.Tables[1];

                    if (dtCol1Audit.Rows.Count > 0)
                        Generate_Column1_Audit(book, dtCol1Audit);
                }
                if (dtCounts.Rows[0]["ColHead2"].ToString() != "" && dtCounts.Rows[0]["ColHead2"].ToString() != "0.00")
                {
                    DataTable dtCol2Audit = ds.Tables[2];

                    if (dtCol2Audit.Rows.Count > 0)
                        Generate_Column2_Audit(book, dtCol2Audit);
                }

                FileStream stream = new FileStream(PdfName, FileMode.Create);

                book.Save(stream);
                stream.Close();

                FileInfo fiDownload = new FileInfo(PdfName);
                string name = fiDownload.Name;
                using (FileStream fileStream = fiDownload.OpenRead())
                {
                    Application.Download(fileStream, name);
                    AlertBox.Show("PIR Audit Report Downloaded Successfully");
                }
            }
        }

        private void PrintHeaderPage(Worksheet sheet, WorksheetCell cell)
        {
            #region Header Page

            WorksheetColumn column0 = sheet.Table.Columns.Add();
            column0.Index = 2;
            column0.Width = 5;
            sheet.Table.Columns.Add(162);
            WorksheetColumn column2 = sheet.Table.Columns.Add();
            column2.Width = 332;
            column2.StyleID = "s123";
            sheet.Table.Columns.Add(59);
            // -----------------------------------------------
            WorksheetRow Row0 = sheet.Table.Rows.Add();
            cell = Row0.Cells.Add();
            cell.StyleID = "s107";
            cell = Row0.Cells.Add();
            cell.StyleID = "s107";
            cell = Row0.Cells.Add();
            cell.StyleID = "s107";
            cell = Row0.Cells.Add();
            cell.StyleID = "s108";
            cell = Row0.Cells.Add();
            cell.StyleID = "s107";
            // -----------------------------------------------
            WorksheetRow Row1 = sheet.Table.Rows.Add();
            Row1.Height = 14;
            Row1.AutoFitHeight = false;
            cell = Row1.Cells.Add();
            cell.StyleID = "s107";
            cell = Row1.Cells.Add();
            cell.StyleID = "s109";
            cell = Row1.Cells.Add();
            cell.StyleID = "s110";
            cell = Row1.Cells.Add();
            cell.StyleID = "s111";
            cell = Row1.Cells.Add();
            cell.StyleID = "s112";
            // -----------------------------------------------
            WorksheetRow Row2 = sheet.Table.Rows.Add();
            Row2.Height = 14;
            Row2.AutoFitHeight = false;
            cell = Row2.Cells.Add();
            cell.StyleID = "s107";
            cell = Row2.Cells.Add();
            cell.StyleID = "s113";
            cell = Row2.Cells.Add();
            cell.StyleID = "s114";
            cell.Data.Type = DataType.String;
            cell.MergeAcross = 2;
            // -----------------------------------------------
            WorksheetRow Row3 = sheet.Table.Rows.Add();
            Row3.Height = 18;
            Row3.AutoFitHeight = false;
            cell = Row3.Cells.Add();
            cell.StyleID = "s107";
            cell = Row3.Cells.Add();
            cell.StyleID = "s113";
            cell = Row3.Cells.Add();
            cell.StyleID = "s115";
            cell.Data.Type = DataType.String;
            cell.Data.Text = _priviliges.Program + " - " + _priviliges.PrivilegeName;
            cell.MergeAcross = 2;
            // -----------------------------------------------
            WorksheetRow Row4 = sheet.Table.Rows.Add();
            Row4.Height = 14;
            Row4.AutoFitHeight = false;
            cell = Row4.Cells.Add();
            cell.StyleID = "s107";
            cell = Row4.Cells.Add();
            cell.StyleID = "s113";
            cell = Row4.Cells.Add();
            cell.StyleID = "s107";
            cell = Row4.Cells.Add();
            cell.StyleID = "s108";
            cell = Row4.Cells.Add();
            cell.StyleID = "s116";
            // -----------------------------------------------
            WorksheetRow Row6 = sheet.Table.Rows.Add();
            Row6.Height = 14;
            Row6.AutoFitHeight = false;
            cell = Row6.Cells.Add();
            cell.StyleID = "s107";
            cell = Row6.Cells.Add();
            cell.StyleID = "s113";
            cell = Row6.Cells.Add();
            cell.StyleID = "s114";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "Selected Report Parameters";
            cell.MergeAcross = 2;
            // -----------------------------------------------
            WorksheetRow Row7 = sheet.Table.Rows.Add();
            Row7.Height = 14;
            Row7.AutoFitHeight = false;
            cell = Row7.Cells.Add();
            cell.StyleID = "s107";
            cell = Row7.Cells.Add();
            cell.StyleID = "s113";
            cell = Row7.Cells.Add();
            cell.StyleID = "s107";
            cell = Row7.Cells.Add();
            cell.StyleID = "s108";
            cell = Row7.Cells.Add();
            cell.StyleID = "s116";
            // -----------------------------------------------
            WorksheetRow Row8 = sheet.Table.Rows.Add();
            Row8.Height = 14;
            Row8.AutoFitHeight = false;
            cell = Row8.Cells.Add();
            cell.StyleID = "s107";
            cell = Row8.Cells.Add();
            cell.StyleID = "s113";
            cell = Row8.Cells.Add();
            cell.StyleID = "s114";
            cell.Data.Type = DataType.String;
            cell.MergeAcross = 2;
            // -----------------------------------------------
            WorksheetRow Row10 = sheet.Table.Rows.Add();
            Row10.Height = 14;
            Row10.AutoFitHeight = false;
            cell = Row10.Cells.Add();
            cell.StyleID = "s107";
            cell = Row10.Cells.Add();
            cell.StyleID = "s113";
            cell = Row10.Cells.Add();
            cell.StyleID = "s114";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "            Agency: " + Agency + " , Department : " + Depart + " , Program : " + Program;
            cell.MergeAcross = 2;
            // -----------------------------------------------
            WorksheetRow Row11 = sheet.Table.Rows.Add();
            Row11.Height = 14;
            Row11.AutoFitHeight = false;
            cell = Row11.Cells.Add();
            cell.StyleID = "s107";
            cell = Row11.Cells.Add();
            cell.StyleID = "s113";
            cell = Row11.Cells.Add();
            cell.StyleID = "s114";
            cell.Data.Type = DataType.String;
            cell.MergeAcross = 2;
            // -----------------------------------------------
            WorksheetRow Row12 = sheet.Table.Rows.Add();
            Row12.Height = 14;
            Row12.AutoFitHeight = false;
            cell = Row12.Cells.Add();
            cell.StyleID = "s107";
            cell = Row12.Cells.Add();
            cell.StyleID = "s113";
            cell = Row12.Cells.Add();
            cell.StyleID = "s107";
            cell = Row12.Cells.Add();
            cell.StyleID = "s108";
            cell = Row12.Cells.Add();
            cell.StyleID = "s116";
            // -----------------------------------------------
            WorksheetRow Row13 = sheet.Table.Rows.Add();
            Row13.Height = 14;
            Row13.AutoFitHeight = false;
            cell = Row13.Cells.Add();
            cell.StyleID = "s107";
            cell = Row13.Cells.Add();
            cell.StyleID = "s113";
            cell = Row13.Cells.Add();
            cell.StyleID = "s114";
            cell.Data.Type = DataType.String;
            cell.MergeAcross = 2;
            // -----------------------------------------------
            WorksheetRow Row15 = sheet.Table.Rows.Add();
            Row15.Height = 14;
            Row15.AutoFitHeight = false;
            cell = Row15.Cells.Add();
            cell.StyleID = "s107";
            cell = Row15.Cells.Add();
            cell.StyleID = "s113";
            Row15.Cells.Add("            " + lblSection.Text.Trim(), DataType.String, "s117");
            Row15.Cells.Add(" : " + ((Captain.Common.Utilities.ListItem)Cmb_Section.SelectedItem).Text.ToString().Trim(), DataType.String, "s119");
            cell = Row15.Cells.Add();
            cell.StyleID = "s116";
            // -----------------------------------------------

            WorksheetRow Row17 = sheet.Table.Rows.Add();
            Row17.Height = 14;
            Row17.AutoFitHeight = false;
            cell = Row17.Cells.Add();
            cell.StyleID = "s107";
            cell = Row17.Cells.Add();
            cell.StyleID = "s113";
            Row17.Cells.Add("            " + lblFund.Text.Trim(), DataType.String, "s117");
            Row17.Cells.Add(" : " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString().Trim(), DataType.String, "s119");
            cell = Row17.Cells.Add();
            cell.StyleID = "s116";
            // -----------------------------------------------

            WorksheetRow Row19 = sheet.Table.Rows.Add();
            Row19.Height = 14;
            Row19.AutoFitHeight = false;
            cell = Row19.Cells.Add();
            cell.StyleID = "s107";
            cell = Row19.Cells.Add();
            cell.StyleID = "s113";
            Row19.Cells.Add("            " + lblSite.Text.Trim(), DataType.String, "s117");
            Row19.Cells.Add(" : " + ((Captain.Common.Utilities.ListItem)Cmb_Site.SelectedItem).Text.ToString().Trim(), DataType.String, "s119");
            cell = Row19.Cells.Add();
            cell.StyleID = "s116";
            // -----------------------------------------------
            WorksheetRow Row21 = sheet.Table.Rows.Add();
            Row21.Height = 12;
            Row21.AutoFitHeight = false;
            cell = Row21.Cells.Add();
            cell.StyleID = "s107";
            cell = Row21.Cells.Add();
            cell.StyleID = "s113";
            cell = Row21.Cells.Add();
            cell.StyleID = "s114";
            cell.Data.Type = DataType.String;
            cell.MergeAcross = 2;
            // -----------------------------------------------
            WorksheetRow Row233 = sheet.Table.Rows.Add();
            Row233.Height = 12;
            Row233.AutoFitHeight = false;
            cell = Row233.Cells.Add();
            cell.StyleID = "s107";
            cell = Row233.Cells.Add();
            cell.StyleID = "s113";
            cell = Row233.Cells.Add();
            cell.StyleID = "s107";
            cell = Row233.Cells.Add();
            cell.StyleID = "s108";
            cell = Row233.Cells.Add();
            cell.StyleID = "s116";
            // -----------------------------------------------
            WorksheetRow Row23 = sheet.Table.Rows.Add();
            Row23.Height = 12;
            Row23.AutoFitHeight = false;
            cell = Row23.Cells.Add();
            cell.StyleID = "s107";
            cell = Row23.Cells.Add();
            cell.StyleID = "s113";
            cell = Row23.Cells.Add();
            cell.StyleID = "s107";
            cell = Row23.Cells.Add();
            cell.StyleID = "s108";
            cell = Row23.Cells.Add();
            cell.StyleID = "s116";
            // -----------------------------------------------
            WorksheetRow Row24 = sheet.Table.Rows.Add();
            Row24.Height = 12;
            Row24.AutoFitHeight = false;
            cell = Row24.Cells.Add();
            cell.StyleID = "s107";
            cell = Row24.Cells.Add();
            cell.StyleID = "s113";
            cell = Row24.Cells.Add();
            cell.StyleID = "s114";
            cell.Data.Type = DataType.String;
            cell.MergeAcross = 2;
            // -----------------------------------------------
            WorksheetRow Row25 = sheet.Table.Rows.Add();
            Row25.Height = 12;
            Row25.AutoFitHeight = false;
            cell = Row25.Cells.Add();
            cell.StyleID = "s107";
            cell = Row25.Cells.Add();
            cell.StyleID = "s113";
            cell = Row25.Cells.Add();
            cell.StyleID = "s107";
            cell = Row25.Cells.Add();
            cell.StyleID = "s108";
            cell = Row25.Cells.Add();
            cell.StyleID = "s116";
            // -----------------------------------------------
            WorksheetRow Row26 = sheet.Table.Rows.Add();
            Row26.Height = 12;
            Row26.AutoFitHeight = false;
            cell = Row26.Cells.Add();
            cell.StyleID = "s107";
            cell = Row26.Cells.Add();
            cell.StyleID = "s113";
            cell = Row26.Cells.Add();
            cell.StyleID = "s114";
            cell.Data.Type = DataType.String;
            cell.MergeAcross = 2;
            // -----------------------------------------------
            WorksheetRow Row27 = sheet.Table.Rows.Add();
            cell = Row27.Cells.Add();
            cell.StyleID = "s107";
            cell = Row27.Cells.Add();
            cell.StyleID = "s113";
            cell = Row27.Cells.Add();
            cell.StyleID = "s107";
            cell = Row27.Cells.Add();
            cell.StyleID = "s108";
            cell = Row27.Cells.Add();
            cell.StyleID = "s116";
            // -----------------------------------------------
            WorksheetRow Row28 = sheet.Table.Rows.Add();
            cell = Row28.Cells.Add();
            cell.StyleID = "s107";
            cell = Row28.Cells.Add();
            cell.StyleID = "s120";
            cell = Row28.Cells.Add();
            cell.StyleID = "m169114912";
            cell.Data.Type = DataType.String;
            cell.MergeAcross = 2;
            // -----------------------------------------------
            //  Options
            // -----------------------------------------------
            sheet.Options.ProtectObjects = false;
            sheet.Options.ProtectScenarios = false;
            sheet.Options.PageSetup.Header.Margin = 0.3F;
            sheet.Options.PageSetup.Footer.Margin = 0.3F;
            sheet.Options.PageSetup.PageMargins.Bottom = 0.75F;
            sheet.Options.PageSetup.PageMargins.Left = 0.7F;
            sheet.Options.PageSetup.PageMargins.Right = 0.7F;
            sheet.Options.PageSetup.PageMargins.Top = 0.75F;

            #endregion
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
            //  s106
            // -----------------------------------------------
            WorksheetStyle s106 = styles.Add("s106");
            s106.Name = "Normal 3";
            s106.Font.FontName = "Calibri";
            s106.Font.Size = 11;
            s106.Font.Color = "#000000";
            s106.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  m169114912
            // -----------------------------------------------
            WorksheetStyle m169114912 = styles.Add("m169114912");
            m169114912.Parent = "s106";
            m169114912.Font.FontName = "Arial";
            m169114912.Font.Color = "#800080";
            m169114912.Interior.Color = "#FFFFFF";
            m169114912.Interior.Pattern = StyleInteriorPattern.Solid;
            m169114912.Alignment.Vertical = StyleVerticalAlignment.Top;
            m169114912.Alignment.WrapText = true;
            m169114912.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            m169114912.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            m169114912.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  m191119360
            // -----------------------------------------------
            WorksheetStyle m191119360 = styles.Add("m191119360");
            m191119360.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            m191119360.Alignment.Vertical = StyleVerticalAlignment.Center;
            m191119360.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            m191119360.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            m191119360.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            m191119360.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  m191119460
            // -----------------------------------------------
            WorksheetStyle m191119460 = styles.Add("m191119460");
            m191119460.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            m191119460.Alignment.Vertical = StyleVerticalAlignment.Center;
            m191119460.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            m191119460.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            m191119460.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            m191119460.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  m191119480
            // -----------------------------------------------
            WorksheetStyle m191119480 = styles.Add("m191119480");
            m191119480.Font.FontName = "Calibri";
            m191119480.Font.Size = 11;
            m191119480.Font.Color = "#000000";
            m191119480.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            m191119480.Alignment.Vertical = StyleVerticalAlignment.Center;
            m191119480.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            m191119480.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            m191119480.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            m191119480.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  m137315584
            // -----------------------------------------------
            WorksheetStyle m137315584 = styles.Add("m137315584");
            m137315584.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            m137315584.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            m137315584.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            m137315584.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  m137315764
            // -----------------------------------------------
            WorksheetStyle m137315764 = styles.Add("m137315764");
            m137315764.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            m137315764.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            m137315764.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            m137315764.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  m137315784
            // -----------------------------------------------
            WorksheetStyle m137315784 = styles.Add("m137315784");
            m137315784.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            m137315784.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            m137315784.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            m137315784.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  m137315804
            // -----------------------------------------------
            WorksheetStyle m137315804 = styles.Add("m137315804");
            m137315804.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            m137315804.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            m137315804.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            m137315804.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  m137315824
            // -----------------------------------------------
            WorksheetStyle m137315824 = styles.Add("m137315824");
            m137315824.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            m137315824.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            m137315824.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            m137315824.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s16
            // -----------------------------------------------
            WorksheetStyle s16 = styles.Add("s16");
            s16.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s16.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s17
            // -----------------------------------------------
            WorksheetStyle s17 = styles.Add("s17");
            s17.Font.Bold = true;
            s17.Font.FontName = "Calibri";
            s17.Font.Size = 11;
            s17.Font.Color = "#000000";
            // -----------------------------------------------
            //  s18
            // -----------------------------------------------
            WorksheetStyle s18 = styles.Add("s18");
            s18.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s19
            // -----------------------------------------------
            WorksheetStyle s19 = styles.Add("s19");
            s19.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s19.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s19.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s19.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s20
            // -----------------------------------------------
            WorksheetStyle s20 = styles.Add("s20");
            s20.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s20.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s20.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s20.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s20.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);


            // -----------------------------------------------
            WorksheetStyle s2012 = styles.Add("s2012");
            s2012.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            s2012.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s2012.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s2012.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s2012.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s2012.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s21
            // -----------------------------------------------
            WorksheetStyle s21 = styles.Add("s21");
            // -----------------------------------------------
            //  s22
            // -----------------------------------------------
            WorksheetStyle s22 = styles.Add("s22");
            s22.Interior.Color = "#F4B084";
            s22.Interior.Pattern = StyleInteriorPattern.Solid;
            // -----------------------------------------------
            //  s23
            // -----------------------------------------------
            WorksheetStyle s23 = styles.Add("s23");
            s23.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 2);
            // -----------------------------------------------
            //  s24
            // -----------------------------------------------
            WorksheetStyle s24 = styles.Add("s24");
            s24.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s24.Alignment.WrapText = true;
            // -----------------------------------------------
            //  s25
            // -----------------------------------------------
            WorksheetStyle s25 = styles.Add("s25");
            s25.Font.FontName = "Calibri";
            s25.Font.Size = 11;
            s25.Font.Color = "#203764";
            // -----------------------------------------------
            //  s26
            // -----------------------------------------------
            WorksheetStyle s26 = styles.Add("s26");
            s26.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s26.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s26.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s26.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s27
            // -----------------------------------------------
            WorksheetStyle s27 = styles.Add("s27");
            s27.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s28
            // -----------------------------------------------
            WorksheetStyle s28 = styles.Add("s28");
            // -----------------------------------------------
            //  s29
            // -----------------------------------------------
            WorksheetStyle s29 = styles.Add("s29");
            s29.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s29.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s30
            // -----------------------------------------------
            WorksheetStyle s30 = styles.Add("s30");
            s30.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s30.Alignment.Vertical = StyleVerticalAlignment.Center;
            s30.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s30.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s30.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s30.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s31
            // -----------------------------------------------
            WorksheetStyle s31 = styles.Add("s31");
            s31.Font.FontName = "Calibri";
            s31.Font.Size = 8;
            s31.Font.Color = "#000000";
            // -----------------------------------------------
            //  s32
            // -----------------------------------------------
            WorksheetStyle s32 = styles.Add("s32");
            s32.Alignment.Vertical = StyleVerticalAlignment.Center;
            // -----------------------------------------------
            //  s33
            // -----------------------------------------------
            WorksheetStyle s33 = styles.Add("s33");
            s33.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            s33.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s47
            // -----------------------------------------------
            WorksheetStyle s47 = styles.Add("s47");
            s47.Font.Bold = true;
            s47.Font.FontName = "Calibri";
            s47.Font.Size = 12;
            s47.Font.Color = "#000000";
            s47.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s47.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s48
            // -----------------------------------------------
            WorksheetStyle s48 = styles.Add("s48");
            s48.Font.Bold = true;
            s48.Font.FontName = "Calibri";
            s48.Font.Size = 11;
            s48.Font.Color = "#000000";
            s48.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s48.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s50
            // -----------------------------------------------
            WorksheetStyle s50 = styles.Add("s50");
            s50.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s50.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s50.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s50.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s50.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s50.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s501
            // -----------------------------------------------
            WorksheetStyle s501 = styles.Add("s501");
            s501.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s501.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s501.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s501.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s501.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s501.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s51
            // -----------------------------------------------
            WorksheetStyle s51 = styles.Add("s51");
            s51.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s51.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s55
            // -----------------------------------------------
            WorksheetStyle s55 = styles.Add("s55");
            s55.Font.FontName = "Calibri";
            s55.Font.Size = 11;
            s55.Font.Color = "#203764";
            s55.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s55.Alignment.Vertical = StyleVerticalAlignment.Center;
            s55.Alignment.WrapText = true;
            // -----------------------------------------------
            //  s107
            // -----------------------------------------------
            WorksheetStyle s107 = styles.Add("s107");
            s107.Parent = "s106";
            s107.Font.FontName = "Calibri";
            s107.Font.Size = 11;
            s107.Interior.Color = "#FFFFFF";
            s107.Interior.Pattern = StyleInteriorPattern.Solid;
            s107.Alignment.Vertical = StyleVerticalAlignment.Top;
            s107.Alignment.WrapText = true;
            // -----------------------------------------------
            //  s108
            // -----------------------------------------------
            WorksheetStyle s108 = styles.Add("s108");
            s108.Parent = "s106";
            s108.Font.FontName = "Calibri";
            s108.Font.Size = 11;
            s108.Interior.Color = "#FFFFFF";
            s108.Interior.Pattern = StyleInteriorPattern.Solid;
            s108.Alignment.Vertical = StyleVerticalAlignment.Top;
            // -----------------------------------------------
            //  s109
            // -----------------------------------------------
            WorksheetStyle s109 = styles.Add("s109");
            s109.Parent = "s106";
            s109.Font.FontName = "Calibri";
            s109.Font.Size = 11;
            s109.Interior.Color = "#FFFFFF";
            s109.Interior.Pattern = StyleInteriorPattern.Solid;
            s109.Alignment.Vertical = StyleVerticalAlignment.Top;
            s109.Alignment.WrapText = true;
            s109.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            s109.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s109C
            // -----------------------------------------------
            WorksheetStyle s109C = styles.Add("s109C");
            s109C.Parent = "s106";
            s109C.Font.FontName = "Calibri";
            s109C.Font.Size = 11;
            s109C.Interior.Color = "#FFFFFF";
            s109C.Interior.Pattern = StyleInteriorPattern.Solid;
            s109C.Alignment.Vertical = StyleVerticalAlignment.Top;
            s109C.Alignment.WrapText = true;
            s109C.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s110
            // -----------------------------------------------
            WorksheetStyle s110 = styles.Add("s110");
            s110.Parent = "s106";
            s110.Font.FontName = "Calibri";
            s110.Font.Size = 11;
            s110.Interior.Color = "#FFFFFF";
            s110.Interior.Pattern = StyleInteriorPattern.Solid;
            s110.Alignment.Vertical = StyleVerticalAlignment.Top;
            s110.Alignment.WrapText = true;
            s110.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s111
            // -----------------------------------------------
            WorksheetStyle s111 = styles.Add("s111");
            s111.Parent = "s106";
            s111.Font.FontName = "Calibri";
            s111.Font.Size = 11;
            s111.Interior.Color = "#FFFFFF";
            s111.Interior.Pattern = StyleInteriorPattern.Solid;
            s111.Alignment.Vertical = StyleVerticalAlignment.Top;
            s111.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s111P
            // -----------------------------------------------
            WorksheetStyle s111P = styles.Add("s111P");
            s111P.Parent = "s106";
            s111P.Font.FontName = "Calibri";
            s111P.Font.Size = 11;
            s111P.Interior.Color = "#FFFFFF";
            s111P.Font.Color = "#000000";
            s111P.Interior.Pattern = StyleInteriorPattern.Solid;
            s111P.Alignment.Vertical = StyleVerticalAlignment.Top;
            // -----------------------------------------------
            //  s112
            // -----------------------------------------------
            WorksheetStyle s112 = styles.Add("s112");
            s112.Parent = "s106";
            s112.Font.FontName = "Calibri";
            s112.Font.Size = 11;
            s112.Interior.Color = "#FFFFFF";
            s112.Interior.Pattern = StyleInteriorPattern.Solid;
            s112.Alignment.Vertical = StyleVerticalAlignment.Top;
            s112.Alignment.WrapText = true;
            s112.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            s112.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s112C
            // -----------------------------------------------
            WorksheetStyle s112C = styles.Add("s112C");
            s112C.Parent = "s106";
            s112C.Font.FontName = "Calibri";
            s112C.Font.Size = 11;
            s112C.Font.Color = "#800080";
            s112C.Interior.Color = "#FFFFFF";
            s112C.Interior.Pattern = StyleInteriorPattern.Solid;
            s112C.Alignment.Vertical = StyleVerticalAlignment.Top;
            s112C.Alignment.WrapText = true;
            s112C.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            s112C.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s113
            // -----------------------------------------------
            WorksheetStyle s113 = styles.Add("s113");
            s113.Parent = "s106";
            s113.Font.FontName = "Calibri";
            s113.Font.Size = 11;
            s113.Interior.Color = "#FFFFFF";
            s113.Interior.Pattern = StyleInteriorPattern.Solid;
            s113.Alignment.Vertical = StyleVerticalAlignment.Top;
            s113.Alignment.WrapText = true;
            s113.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s113C
            // -----------------------------------------------
            WorksheetStyle s113C = styles.Add("s113C");
            s113C.Parent = "s106";
            s113C.Font.FontName = "Calibri";
            s113C.Font.Size = 11;
            s113C.Interior.Color = "#FFFFFF";
            s113C.Interior.Pattern = StyleInteriorPattern.Solid;
            s113C.Alignment.Vertical = StyleVerticalAlignment.Top;
            s113C.Alignment.WrapText = true;
            s113C.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            s113C.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s114
            // -----------------------------------------------
            WorksheetStyle s114 = styles.Add("s114");
            s114.Parent = "s106";
            s114.Font.FontName = "Arial";
            s114.Font.Color = "#800080";
            s114.Interior.Color = "#FFFFFF";
            s114.Interior.Pattern = StyleInteriorPattern.Solid;
            s114.Alignment.Vertical = StyleVerticalAlignment.Top;
            s114.Alignment.WrapText = true;
            s114.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s114.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s114P
            // -----------------------------------------------
            WorksheetStyle s114P = styles.Add("s114P");
            s114P.Parent = "s106";
            s114P.Font.FontName = "Arial";
            s114P.Font.Color = "#000000";
            s114P.Interior.Color = "#FFFFFF";
            s114P.Interior.Pattern = StyleInteriorPattern.Solid;
            s114P.Alignment.Vertical = StyleVerticalAlignment.Top;
            s114P.Alignment.WrapText = true;
            s114P.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s114P.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s115
            // -----------------------------------------------
            WorksheetStyle s115 = styles.Add("s115");
            s115.Parent = "s106";
            s115.Font.Bold = true;
            s115.Font.FontName = "Arial";
            s115.Font.Size = 14;
            s115.Font.Color = "#800080";
            s115.Interior.Color = "#FFFFFF";
            s115.Interior.Pattern = StyleInteriorPattern.Solid;
            s115.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s115.Alignment.Vertical = StyleVerticalAlignment.Top;
            s115.Alignment.WrapText = true;
            s115.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s115.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s116
            // -----------------------------------------------
            WorksheetStyle s116 = styles.Add("s116");
            s116.Parent = "s106";
            s116.Font.FontName = "Calibri";
            s116.Font.Size = 11;
            s116.Interior.Color = "#FFFFFF";
            s116.Interior.Pattern = StyleInteriorPattern.Solid;
            s116.Alignment.Vertical = StyleVerticalAlignment.Top;
            s116.Alignment.WrapText = true;
            s116.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s117
            // -----------------------------------------------
            WorksheetStyle s117 = styles.Add("s117");
            s117.Parent = "s106";
            s117.Font.FontName = "Arial";
            s117.Font.Color = "#800080";
            s117.Interior.Color = "#FFFFFF";
            s117.Interior.Pattern = StyleInteriorPattern.Solid;
            s117.Alignment.Vertical = StyleVerticalAlignment.Top;
            s117.Alignment.WrapText = true;
            s117.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            // -----------------------------------------------
            //  s118
            // -----------------------------------------------
            WorksheetStyle s118 = styles.Add("s118");
            s118.Parent = "s106";
            s118.Font.FontName = "Arial";
            s118.Font.Color = "#800080";
            s118.Interior.Color = "#FFFFFF";
            s118.Interior.Pattern = StyleInteriorPattern.Solid;
            s118.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            s118.Alignment.Vertical = StyleVerticalAlignment.Top;
            s118.Alignment.WrapText = true;
            s118.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            s118.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s119
            // -----------------------------------------------
            WorksheetStyle s119 = styles.Add("s119");
            s119.Parent = "s106";
            s119.Font.FontName = "Arial";
            s119.Font.Color = "#800080";
            s119.Interior.Color = "#FFFFFF";
            s119.Interior.Pattern = StyleInteriorPattern.Solid;
            s119.Alignment.Vertical = StyleVerticalAlignment.Top;
            s119.Alignment.ReadingOrder = StyleReadingOrder.LeftToRight;
            // -----------------------------------------------
            //  s120
            // -----------------------------------------------
            WorksheetStyle s120 = styles.Add("s120");
            s120.Parent = "s106";
            s120.Font.FontName = "Calibri";
            s120.Font.Size = 11;
            s120.Interior.Color = "#FFFFFF";
            s120.Interior.Pattern = StyleInteriorPattern.Solid;
            s120.Alignment.Vertical = StyleVerticalAlignment.Top;
            s120.Alignment.WrapText = true;
            s120.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "#000000");
            s120.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "#000000");
            // -----------------------------------------------
            //  s123
            // -----------------------------------------------
            WorksheetStyle s123 = styles.Add("s123");
            s123.Alignment.Vertical = StyleVerticalAlignment.Bottom;
        }

        private void Generate_Column1_Audit(Workbook book, DataTable dtCol1Audit)
        {
            try
            {

                WorksheetRow excelrowSpace;
                WorksheetRow excelrow1;
                WorksheetRow excelrow2;
                WorksheetCell cell;
                Worksheet sheet = book.Worksheets.Add("Header_1 " + "Audit");

                //sheet.Table.DefaultRowHeight = 14.25F;

                sheet.Table.DefaultColumnWidth = 220.5F;
                sheet.Table.Columns.Add(15);
                sheet.Table.Columns.Add(40);
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(100);
                sheet.Table.Columns.Add(100);
                sheet.Table.Columns.Add(40);
                sheet.Table.Columns.Add(150);
                sheet.Table.Columns.Add(100);
                sheet.Table.Columns.Add(170);
                sheet.Table.Columns.Add(200);
                sheet.Table.Columns.Add(100);

                sheet.Table.Columns.Add(200);
                sheet.Table.Columns.Add(100);
                sheet.Table.Columns.Add(100);
                sheet.Table.Columns.Add(100);
                sheet.Table.Columns.Add(100);
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(80);

                sheet.Table.Columns.Add(100);
                sheet.Table.Columns.Add(60);
                sheet.Table.Columns.Add(60);
                sheet.Table.Columns.Add(60);
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(100);
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(120);

                excelrowSpace = sheet.Table.Rows.Add();
                cell = excelrowSpace.Cells.Add("", DataType.String, "NormalLeft");
                cell.MergeAcross = 30;

                excelrow1 = sheet.Table.Rows.Add();
                excelrow1.Height = 30;
                cell = excelrow1.Cells.Add("", DataType.String, "NormalLeft");
                cell = excelrow1.Cells.Add("Hit No", DataType.String, "MainHeaderStylesC");
                cell = excelrow1.Cells.Add("Employee No", DataType.String, "MainHeaderStyles");
                cell = excelrow1.Cells.Add("First Name", DataType.String, "MainHeaderStyles");
                cell = excelrow1.Cells.Add("Last Name", DataType.String, "MainHeaderStyles");
                cell = excelrow1.Cells.Add("Active", DataType.String, "MainHeaderStyles");
                cell = excelrow1.Cells.Add("Ethnicity", DataType.String, "MainHeaderStyles");
                cell = excelrow1.Cells.Add("Race", DataType.String, "MainHeaderStyles");
                cell = excelrow1.Cells.Add("Education Completed", DataType.String, "MainHeaderStyles");
                cell = excelrow1.Cells.Add("Education Progress", DataType.String, "MainHeaderStyles");
                cell = excelrow1.Cells.Add("Date Terminated", DataType.String, "MainHeaderStyles");
                cell = excelrow1.Cells.Add("Reason For Termination", DataType.String, "MainHeaderStyles");
                cell = excelrow1.Cells.Add("Primary Language", DataType.String, "MainHeaderStyles");
                cell = excelrow1.Cells.Add("Language 1", DataType.String, "MainHeaderStyles");
                cell = excelrow1.Cells.Add("Language 2", DataType.String, "MainHeaderStyles");
                cell = excelrow1.Cells.Add("Language 3", DataType.String, "MainHeaderStyles");
                cell = excelrow1.Cells.Add("Work For HS", DataType.String, "MainHeaderStylesC");
                cell = excelrow1.Cells.Add("Work For EHS", DataType.String, "MainHeaderStylesC");
                cell = excelrow1.Cells.Add("Work For Non HS", DataType.String, "MainHeaderStylesC");
                cell = excelrow1.Cells.Add("Work For Volunteer", DataType.String, "MainHeaderStylesC");
                cell = excelrow1.Cells.Add("Work For Contracted", DataType.String, "MainHeaderStylesC");
                cell = excelrow1.Cells.Add("Positional Category", DataType.String, "MainHeaderStyles");
                cell = excelrow1.Cells.Add("Hourly Rate", DataType.String, "MainHeaderStylesR");
                cell = excelrow1.Cells.Add("Std Hours", DataType.String, "MainHeaderStylesR");
                cell = excelrow1.Cells.Add("Salary", DataType.String, "MainHeaderStylesR");
                cell = excelrow1.Cells.Add("Weeks Worked", DataType.String, "MainHeaderStylesR");
                cell = excelrow1.Cells.Add("Head Start Parent", DataType.String, "MainHeaderStylesC");
                cell = excelrow1.Cells.Add("Daycare Program Parent", DataType.String, "MainHeaderStylesC");
                cell = excelrow1.Cells.Add("Early Headestart Parent", DataType.String, "MainHeaderStylesC");
                cell = excelrow1.Cells.Add("Position Filled after 3 Months", DataType.String, "MainHeaderStylesC");
                cell = excelrow1.Cells.Add("This Individual replaced another staff member", DataType.String, "MainHeaderStylesC");

                foreach (DataRow dr in dtCol1Audit.Rows)
                {
                    excelrow2 = sheet.Table.Rows.Add();

                    cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                    cell = excelrow2.Cells.Add(dr["HIT_NUM"].ToString().Trim() == "" ? "" : dr["HIT_NUM"].ToString().Trim(), DataType.String, "Normalcenter");

                    cell = excelrow2.Cells.Add(dr["STF_CODE"].ToString().Trim() == "" ? "" : dr["STF_CODE"].ToString().Trim(), DataType.String, "NormalLeft");

                    cell = excelrow2.Cells.Add(dr["STF_NAME_FI"].ToString().Trim() == "" ? "" : dr["STF_NAME_FI"].ToString().Trim(), DataType.String, "NormalLeft");

                    cell = excelrow2.Cells.Add(dr["STF_NAME_LAST"].ToString().Trim() == "" ? "" : dr["STF_NAME_LAST"].ToString().Trim(), DataType.String, "NormalLeft");

                    cell = excelrow2.Cells.Add(dr["STF_ACTIVE"].ToString().Trim() == "" ? "" : dr["STF_ACTIVE"].ToString().Trim(), DataType.String, "Normalcenter");

                    cell = excelrow2.Cells.Add(dr["ETHNICITY"].ToString().Trim() == "" ? "" : dr["ETHNICITY"].ToString().Trim(), DataType.String, "NormalLeft");

                    cell = excelrow2.Cells.Add(dr["Race"].ToString().Trim() == "" ? "" : dr["Race"].ToString().Trim(), DataType.String, "NormalLeft");

                    cell = excelrow2.Cells.Add((dr["Education"].ToString().Trim() == "" ? "" : dr["Education"].ToString().Trim()), DataType.String, "NormalLeft");

                    cell = excelrow2.Cells.Add(dr["EDUCATION_PROGRESS"].ToString().Trim() == "" ? "" : dr["EDUCATION_PROGRESS"].ToString().Trim(), DataType.String, "NormalLeft");

                    cell = excelrow2.Cells.Add(dr["STF_DATE_TERMINATED"].ToString().Trim() == "" ? "" : Convert.ToDateTime(dr["STF_DATE_TERMINATED"]).ToString("MM/dd/yyyy").ToString().Trim(), DataType.String, "NormalLeft");

                    cell = excelrow2.Cells.Add(dr["RES_TERMINATION"].ToString().Trim() == "" ? "" : dr["RES_TERMINATION"].ToString().Trim(), DataType.String, "NormalLeft");

                    cell = excelrow2.Cells.Add(dr["PRIM_LANGUAGE"].ToString().Trim() == "" ? "" : dr["PRIM_LANGUAGE"].ToString().Trim(), DataType.String, "NormalLeft");

                    cell = excelrow2.Cells.Add(dr["LANGUAGE1"].ToString().Trim() == "" ? "" : dr["LANGUAGE1"].ToString().Trim(), DataType.String, "NormalLeft");

                    cell = excelrow2.Cells.Add(dr["LANGUAGE2"].ToString().Trim() == "" ? "" : dr["LANGUAGE2"].ToString().Trim(), DataType.String, "NormalLeft");

                    cell = excelrow2.Cells.Add(dr["LANGUAGE3"].ToString().Trim() == "" ? "" : dr["LANGUAGE3"].ToString(), DataType.String, "NormalLeft");

                    cell = excelrow2.Cells.Add((dr["STF_WORKFOR_HS"].ToString().Trim() == "" ? "" : dr["STF_WORKFOR_HS"].ToString().Trim()), DataType.String, "Normalcenter");

                    cell = excelrow2.Cells.Add(dr["STF_WORKFOR_EHS"].ToString().Trim() == "" ? "" : dr["STF_WORKFOR_EHS"].ToString().Trim(), DataType.String, "Normalcenter");

                    cell = excelrow2.Cells.Add(dr["STF_WORKFOR_NONHS"].ToString().Trim() == "" ? "" : dr["STF_WORKFOR_NONHS"].ToString().Trim(), DataType.String, "Normalcenter");

                    cell = excelrow2.Cells.Add(dr["STF_WORKFOR_VOL"].ToString().Trim() == "" ? "" : dr["STF_WORKFOR_VOL"].ToString().Trim(), DataType.String, "Normalcenter");

                    cell = excelrow2.Cells.Add(dr["STF_WORKFOR_CONT"].ToString().Trim() == "" ? "" : dr["STF_WORKFOR_CONT"].ToString().Trim(), DataType.String, "Normalcenter");

                    cell = excelrow2.Cells.Add(dr["POS_CATG"].ToString().Trim() == "" ? "" : dr["POS_CATG"].ToString().Trim(), DataType.String, "NormalLeft");

                    cell = excelrow2.Cells.Add(dr["STF_BASE_RATE"].ToString().Trim() == "" ? "" : dr["STF_BASE_RATE"].ToString().Trim(), DataType.String, "NormalRight");

                    cell = excelrow2.Cells.Add(dr["STF_HRS_WORKED_PW"].ToString().Trim() == "" ? "" : dr["STF_HRS_WORKED_PW"].ToString(), DataType.String, "NormalRight");

                    cell = excelrow2.Cells.Add((dr["STF_SALARY"].ToString().Trim() == "" ? "" : dr["STF_SALARY"].ToString().Trim()), DataType.String, "NormalRight");

                    cell = excelrow2.Cells.Add(dr["STF_WEEKS_WORKED"].ToString().Trim() == "" ? "" : dr["STF_WEEKS_WORKED"].ToString().Trim(), DataType.String, "NormalRight");

                    cell = excelrow2.Cells.Add(dr["HS_PARENT"].ToString().Trim() == "" ? "" : dr["HS_PARENT"].ToString().Trim(), DataType.String, "Normalcenter");

                    cell = excelrow2.Cells.Add(dr["DAYCARE_PARENT"].ToString().Trim() == "" ? "" : dr["DAYCARE_PARENT"].ToString().Trim(), DataType.String, "Normalcenter");

                    cell = excelrow2.Cells.Add(dr["EHS_PARESNT"].ToString().Trim() == "" ? "" : dr["EHS_PARESNT"].ToString().Trim(), DataType.String, "Normalcenter");

                    cell = excelrow2.Cells.Add(dr["POS_FILLED"].ToString().Trim() == "" ? "" : dr["POS_FILLED"].ToString().Trim(), DataType.String, "Normalcenter");

                    cell = excelrow2.Cells.Add(dr["REPLACE_SM"].ToString().Trim() == "" ? "" : dr["REPLACE_SM"].ToString().Trim(), DataType.String, "Normalcenter");
                }
            }
            catch (Exception ex) { }

        }

        private void Generate_Column2_Audit(Workbook book, DataTable dtCol2Audit)
        {
            try
            {

                WorksheetRow excelrowSpace;
                WorksheetRow excelrow1;
                WorksheetRow excelrow2;
                WorksheetCell cell;
                Worksheet sheet = book.Worksheets.Add("Header_2 " + "Audit");

                //sheet.Table.DefaultRowHeight = 14.25F;

                sheet.Table.DefaultColumnWidth = 220.5F;
                sheet.Table.Columns.Add(15);
                sheet.Table.Columns.Add(40);
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(100);
                sheet.Table.Columns.Add(100);
                sheet.Table.Columns.Add(40);
                sheet.Table.Columns.Add(150);
                sheet.Table.Columns.Add(100);
                sheet.Table.Columns.Add(170);
                sheet.Table.Columns.Add(200);
                sheet.Table.Columns.Add(100);

                sheet.Table.Columns.Add(200);
                sheet.Table.Columns.Add(100);
                sheet.Table.Columns.Add(100);
                sheet.Table.Columns.Add(100);
                sheet.Table.Columns.Add(100);
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(80);

                sheet.Table.Columns.Add(100);
                sheet.Table.Columns.Add(60);
                sheet.Table.Columns.Add(60);
                sheet.Table.Columns.Add(60);
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(100);
                sheet.Table.Columns.Add(80);
                sheet.Table.Columns.Add(120);

                excelrowSpace = sheet.Table.Rows.Add();
                cell = excelrowSpace.Cells.Add("", DataType.String, "NormalLeft");
                cell.MergeAcross = 30;

                excelrow1 = sheet.Table.Rows.Add();
                excelrow1.Height = 30;
                cell = excelrow1.Cells.Add("", DataType.String, "NormalLeft");
                cell = excelrow1.Cells.Add("Hit No", DataType.String, "MainHeaderStylesC");
                cell = excelrow1.Cells.Add("Employee No", DataType.String, "MainHeaderStyles");
                cell = excelrow1.Cells.Add("First Name", DataType.String, "MainHeaderStyles");
                cell = excelrow1.Cells.Add("Last Name", DataType.String, "MainHeaderStyles");
                cell = excelrow1.Cells.Add("Active", DataType.String, "MainHeaderStyles");
                cell = excelrow1.Cells.Add("Ethnicity", DataType.String, "MainHeaderStyles");
                cell = excelrow1.Cells.Add("Race", DataType.String, "MainHeaderStyles");
                cell = excelrow1.Cells.Add("Education Completed", DataType.String, "MainHeaderStyles");
                cell = excelrow1.Cells.Add("Education Progress", DataType.String, "MainHeaderStyles");
                cell = excelrow1.Cells.Add("Date Terminated", DataType.String, "MainHeaderStyles");
                cell = excelrow1.Cells.Add("Reason For Termination", DataType.String, "MainHeaderStyles");
                cell = excelrow1.Cells.Add("Primary Language", DataType.String, "MainHeaderStyles");
                cell = excelrow1.Cells.Add("Language 1", DataType.String, "MainHeaderStyles");
                cell = excelrow1.Cells.Add("Language 2", DataType.String, "MainHeaderStyles");
                cell = excelrow1.Cells.Add("Language 3", DataType.String, "MainHeaderStyles");
                cell = excelrow1.Cells.Add("Work For HS", DataType.String, "MainHeaderStylesC");
                cell = excelrow1.Cells.Add("Work For EHS", DataType.String, "MainHeaderStylesC");
                cell = excelrow1.Cells.Add("Work For Non HS", DataType.String, "MainHeaderStylesC");
                cell = excelrow1.Cells.Add("Work For Volunteer", DataType.String, "MainHeaderStylesC");
                cell = excelrow1.Cells.Add("Work For Contracted", DataType.String, "MainHeaderStylesC");
                cell = excelrow1.Cells.Add("Positional Category", DataType.String, "MainHeaderStyles");
                cell = excelrow1.Cells.Add("Hourly Rate", DataType.String, "MainHeaderStylesR");
                cell = excelrow1.Cells.Add("Std Hours", DataType.String, "MainHeaderStylesR");
                cell = excelrow1.Cells.Add("Salary", DataType.String, "MainHeaderStylesR");
                cell = excelrow1.Cells.Add("Weeks Worked", DataType.String, "MainHeaderStylesR");
                cell = excelrow1.Cells.Add("Head Start Parent", DataType.String, "MainHeaderStylesC");
                cell = excelrow1.Cells.Add("Daycare Program Parent ", DataType.String, "MainHeaderStylesC");
                cell = excelrow1.Cells.Add("Early Headestart Parent", DataType.String, "MainHeaderStylesC");
                cell = excelrow1.Cells.Add("Position Filled after 3 Months", DataType.String, "MainHeaderStylesC");
                cell = excelrow1.Cells.Add("This Individual replaced another staff member", DataType.String, "MainHeaderStylesC");

                foreach (DataRow dr in dtCol2Audit.Rows)
                {
                    excelrow2 = sheet.Table.Rows.Add();

                    cell = excelrow2.Cells.Add("", DataType.String, "NormalLeft");

                    cell = excelrow2.Cells.Add(dr["HIT_NUM"].ToString().Trim() == "" ? "" : dr["HIT_NUM"].ToString().Trim(), DataType.String, "Normalcenter");

                    cell = excelrow2.Cells.Add(dr["STF_CODE"].ToString().Trim() == "" ? "" : dr["STF_CODE"].ToString().Trim(), DataType.String, "NormalLeft");

                    cell = excelrow2.Cells.Add(dr["STF_NAME_FI"].ToString().Trim() == "" ? "" : dr["STF_NAME_FI"].ToString().Trim(), DataType.String, "NormalLeft");

                    cell = excelrow2.Cells.Add(dr["STF_NAME_LAST"].ToString().Trim() == "" ? "" : dr["STF_NAME_LAST"].ToString().Trim(), DataType.String, "NormalLeft");

                    cell = excelrow2.Cells.Add(dr["STF_ACTIVE"].ToString().Trim() == "" ? "" : dr["STF_ACTIVE"].ToString().Trim(), DataType.String, "Normalcenter");

                    cell = excelrow2.Cells.Add(dr["ETHNICITY"].ToString().Trim() == "" ? "" : dr["ETHNICITY"].ToString().Trim(), DataType.String, "NormalLeft");

                    cell = excelrow2.Cells.Add(dr["Race"].ToString().Trim() == "" ? "" : dr["Race"].ToString().Trim(), DataType.String, "NormalLeft");

                    cell = excelrow2.Cells.Add((dr["Education"].ToString().Trim() == "" ? "" : dr["Education"].ToString().Trim()), DataType.String, "NormalLeft");

                    cell = excelrow2.Cells.Add(dr["EDUCATION_PROGRESS"].ToString().Trim() == "" ? "" : dr["EDUCATION_PROGRESS"].ToString().Trim(), DataType.String, "NormalLeft");

                    cell = excelrow2.Cells.Add(dr["STF_DATE_TERMINATED"].ToString().Trim() == "" ? "" : Convert.ToDateTime(dr["STF_DATE_TERMINATED"]).ToString("MM/dd/yyyy").ToString().Trim(), DataType.String, "NormalLeft");

                    cell = excelrow2.Cells.Add(dr["RES_TERMINATION"].ToString().Trim() == "" ? "" : dr["RES_TERMINATION"].ToString().Trim(), DataType.String, "NormalLeft");

                    cell = excelrow2.Cells.Add(dr["PRIM_LANGUAGE"].ToString().Trim() == "" ? "" : dr["PRIM_LANGUAGE"].ToString().Trim(), DataType.String, "NormalLeft");

                    cell = excelrow2.Cells.Add(dr["LANGUAGE1"].ToString().Trim() == "" ? "" : dr["LANGUAGE1"].ToString().Trim(), DataType.String, "NormalLeft");

                    cell = excelrow2.Cells.Add(dr["LANGUAGE2"].ToString().Trim() == "" ? "" : dr["LANGUAGE2"].ToString().Trim(), DataType.String, "NormalLeft");

                    cell = excelrow2.Cells.Add(dr["LANGUAGE3"].ToString().Trim() == "" ? "" : dr["LANGUAGE3"].ToString(), DataType.String, "NormalLeft");

                    cell = excelrow2.Cells.Add((dr["STF_WORKFOR_HS"].ToString().Trim() == "" ? "" : dr["STF_WORKFOR_HS"].ToString().Trim()), DataType.String, "Normalcenter");

                    cell = excelrow2.Cells.Add(dr["STF_WORKFOR_EHS"].ToString().Trim() == "" ? "" : dr["STF_WORKFOR_EHS"].ToString().Trim(), DataType.String, "Normalcenter");

                    cell = excelrow2.Cells.Add(dr["STF_WORKFOR_NONHS"].ToString().Trim() == "" ? "" : dr["STF_WORKFOR_NONHS"].ToString().Trim(), DataType.String, "Normalcenter");

                    cell = excelrow2.Cells.Add(dr["STF_WORKFOR_VOL"].ToString().Trim() == "" ? "" : dr["STF_WORKFOR_VOL"].ToString().Trim(), DataType.String, "Normalcenter");

                    cell = excelrow2.Cells.Add(dr["STF_WORKFOR_CONT"].ToString().Trim() == "" ? "" : dr["STF_WORKFOR_CONT"].ToString().Trim(), DataType.String, "Normalcenter");

                    cell = excelrow2.Cells.Add(dr["POS_CATG"].ToString().Trim() == "" ? "" : dr["POS_CATG"].ToString().Trim(), DataType.String, "NormalLeft");

                    cell = excelrow2.Cells.Add(dr["STF_BASE_RATE"].ToString().Trim() == "" ? "" : dr["STF_BASE_RATE"].ToString().Trim(), DataType.String, "NormalRight");

                    cell = excelrow2.Cells.Add(dr["STF_HRS_WORKED_PW"].ToString().Trim() == "" ? "" : dr["STF_HRS_WORKED_PW"].ToString(), DataType.String, "NormalRight");

                    cell = excelrow2.Cells.Add((dr["STF_SALARY"].ToString().Trim() == "" ? "" : dr["STF_SALARY"].ToString().Trim()), DataType.String, "NormalRight");

                    cell = excelrow2.Cells.Add(dr["STF_WEEKS_WORKED"].ToString().Trim() == "" ? "" : dr["STF_WEEKS_WORKED"].ToString().Trim(), DataType.String, "NormalRight");

                    cell = excelrow2.Cells.Add(dr["HS_PARENT"].ToString().Trim() == "" ? "" : dr["HS_PARENT"].ToString().Trim(), DataType.String, "Normalcenter");

                    cell = excelrow2.Cells.Add(dr["DAYCARE_PARENT"].ToString().Trim() == "" ? "" : dr["DAYCARE_PARENT"].ToString().Trim(), DataType.String, "Normalcenter");

                    cell = excelrow2.Cells.Add(dr["EHS_PARESNT"].ToString().Trim() == "" ? "" : dr["EHS_PARESNT"].ToString().Trim(), DataType.String, "Normalcenter");

                    cell = excelrow2.Cells.Add(dr["POS_FILLED"].ToString().Trim() == "" ? "" : dr["POS_FILLED"].ToString().Trim(), DataType.String, "Normalcenter");

                    cell = excelrow2.Cells.Add(dr["REPLACE_SM"].ToString().Trim() == "" ? "" : dr["REPLACE_SM"].ToString().Trim(), DataType.String, "Normalcenter");
                }
            }
            catch (Exception ex) { }
        }


        #endregion

        bool AudiT_Question_Selected = false;
        private void dgvQuestions_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvQuestions.Rows.Count > 0)
            {
                int ColIdx = 0;
                int RowIdx = 0;
                ColIdx = dgvQuestions.CurrentCell.ColumnIndex;
                RowIdx = dgvQuestions.CurrentCell.RowIndex;

                AudiT_Question_Selected = false;
                if (e.ColumnIndex == 0 && e.RowIndex != -1)
                {
                    if (dgvQuestions.CurrentRow.Cells["gvSelect"].Value.ToString() == true.ToString())
                        AudiT_Question_Selected = true;
                }

                //foreach (DataGridViewRow dr in dgvQuestions.Rows)
                //{
                //    if (dr.Cells["gvSelect"].Value.ToString() == true.ToString())
                //    {
                //        if (dr.Cells["gvQID"].Value.ToString() != dgvQuestions.CurrentRow.Cells["gvQID"].Value.ToString())
                //            dr.Cells["gvSelect"].Value = false;
                //    }

                //}
            }
        }

        private void btnSaveRepCounts_Click(object sender, EventArgs e)
        {
            PIRCOUNT2Entity count2_entity = new PIRCOUNT2Entity();
            int i = 0;
            foreach (DataGridViewRow dr in dgvQuestions.Rows)
            {
                if (dr.Cells["gvisHeader"].Value.ToString() != "H")
                {
                    count2_entity.Agency = Current_Hierarchy.Substring(0, 2);
                    count2_entity.Dept = Current_Hierarchy.Substring(2, 2);
                    count2_entity.Prog = Current_Hierarchy.Substring(4, 2);
                    count2_entity.Year = (CmbYear.Visible ? ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString() : "    ");
                    count2_entity.Site = ((Captain.Common.Utilities.ListItem)Cmb_Site.SelectedItem).Value.ToString();
                    count2_entity.Ques_Fund = ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Value.ToString();

                    if (dr.Cells["gvHeader1"].Value.ToString() != "")
                    {
                        count2_entity.Column1 = dr.Cells["gvHeader1"].Value.ToString().Replace("$","").Replace(",","");
                        count2_entity.Ques_ID = dr.Cells["gvQID"].Value.ToString();
                    }
                    else if (dr.Cells["gvHeader1"].Value.ToString() == "" )
                    {
                        count2_entity.Column1 = "";
                        count2_entity.Ques_ID = dr.Cells["gvQID"].Value.ToString();
                    }
                    else
                    {
                        count2_entity.Column1 = "0";
                        count2_entity.Ques_ID = dr.Cells["gvQID"].Value.ToString();
                    }

                    if (dr.Cells["gvHeader2"].Value.ToString() != "")
                    {
                        count2_entity.Column2 = dr.Cells["gvHeader2"].Value.ToString().Replace("$", "").Replace(",", "");
                        count2_entity.Ques_ID = dr.Cells["gvQID"].Value.ToString();
                    }
                    else if (dr.Cells["gvHeader2"].Value.ToString() == "")
                    {
                        count2_entity.Column2 = "";
                        count2_entity.Ques_ID = dr.Cells["gvQID"].Value.ToString();
                    }
                    else
                    {
                        count2_entity.Column2 = "0";
                        count2_entity.Ques_ID = dr.Cells["gvQID"].Value.ToString();
                    }

                    count2_entity.Agy_Flag = string.Empty;
                    count2_entity.Agy_Code = string.Empty;
                    count2_entity.Add_Operator = _baseform.UserID;
                    count2_entity.Lstc_Operator = _baseform.UserID;

                    if (_model.PIRData.UpdatePIRCOUNT2(count2_entity))
                    {
                        i++;
                    }
                }
            }
            if (i > 0)
            {
                fillQuestionsGrid();
                AlertBox.Show("PIR Counts Saved Successfully");
            }
            else
                AlertBox.Show("Failed to Update", MessageBoxIcon.Warning);

        }

        string Program_Year;
        private void fillQuestionsGrid()
        {
            int rowcnt = 0;
            int rowindex = 0;
            if (ValidateReport())
            {
                try
                {
                    DataSet ds = Captain.DatabaseLayer.Lookups.BrowsePIRQues2(Program_Year);
                    dtQues = ds.Tables[0];

                    DataSet dscounts = Captain.DatabaseLayer.Lookups.BrowsePIRCOUNTS(Program_Year);
                    dtCounts = dscounts.Tables[0];

                    DataView dvCounts = new DataView(dtCounts);
                    
                    if (dtCounts.Rows.Count > 0)
                    {
                        if (((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Value.ToString() == "9")
                            dvCounts.RowFilter = "PIRCOUNT_Q_FUND ='9'";
                        if (((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Value.ToString() == "H")
                            dvCounts.RowFilter = "PIRCOUNT_Q_FUND ='H'";
                        if (((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Value.ToString() == "2")
                            dvCounts.RowFilter = "PIRCOUNT_Q_FUND ='2'";
                        if (((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Value.ToString() == "E")
                            dvCounts.RowFilter = "PIRCOUNT_Q_FUND ='E'";
                        if (((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Value.ToString() == "S")
                            dvCounts.RowFilter = "PIRCOUNT_Q_FUND ='S'";
                    }
                    dtCounts = dvCounts.ToTable();

                    DataRow[] drs = dtQues.Select("PIRQUEST_SECTION = '" + ((Captain.Common.Utilities.ListItem)Cmb_Section.SelectedItem).Value.ToString() + "'");
                    List<PIRQUEST2Entity> pirQuestlist = new List<PIRQUEST2Entity>();
                    pirQuestlist.Clear();
                    foreach (DataRow item in drs)
                    {
                        pirQuestlist.Add(new PIRQUEST2Entity(item, string.Empty));

                    }
                    if (pirQuestlist.Count > 0)
                    {
                        if (((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Value.ToString() == "H")
                            pirQuestlist = pirQuestlist.FindAll(u => u.PIR_Fund_Type.Contains("9") || u.PIR_Fund_Type.Contains("H"));
                        if (((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Value.ToString() == "2")
                            pirQuestlist = pirQuestlist.FindAll(u => u.PIR_Fund_Type.Contains("9") || u.PIR_Fund_Type.Contains("2"));
                        if (((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Value.ToString() == "E")
                            pirQuestlist = pirQuestlist.FindAll(u => u.PIR_Fund_Type.Contains("9") || u.PIR_Fund_Type.Contains("E"));
                        if (((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Value.ToString() == "S")
                            pirQuestlist = pirQuestlist.FindAll(u => u.PIR_Fund_Type.Contains("9") || u.PIR_Fund_Type.Contains("S"));

                        pirQuestlist = pirQuestlist.FindAll(u => u.PIR_Active == "A");
                        //pirQuestlist = pirQuestlist.OrderBy(u => u.PIR_Section).ThenBy(u => u.PIR_Ques_Code).ThenBy(u => u.PIR_Ques_SCode).ThenBy(u => u.PIR_Ques_Section).ThenBy(u => Convert.ToInt32(u.PIR_Ques_Position)).ToList();
                        pirQuestlist = pirQuestlist.OrderBy(u => Convert.ToInt32(u.PIR_Ques_ID)).ToList();
                    }

                    dgvQuestions.Rows.Clear();

                    if (pirQuestlist.Count > 0)
                    {
                        string isPrevHeader1 = string.Empty, isPrevHeader2 = string.Empty;

                        foreach (PIRQUEST2Entity item in pirQuestlist)
                        {
                            string question_Type = string.Empty;
                            if (item.PIR_Ques_Bold.ToString().Trim() == "Y")
                            {
                                if (item.PIR_Ques_ID != "6" && item.PIR_Ques_ID != "25")
                                {
                                    isPrevHeader1 = string.Empty;
                                    isPrevHeader2 = string.Empty;
                                }
                            }

                            if (item.PIR_Ques_Section.ToString().Trim() == "0")
                                question_Type = "";
                            else
                            {
                                question_Type = item.PIR_Ques_Type.ToString().Trim();
                                if (!string.IsNullOrEmpty(question_Type.Trim()))
                                {
                                    question_Type = GetQuestionTypeDesc(question_Type);
                                }
                            }

                            if (item.PIR_Ques_Col_Head_Top == "Y" && ((item.PIR_Ques_Col1 != "0" || item.PIR_Ques_Col1 != "") || (item.PIR_Ques_Col2 != "0" || item.PIR_Ques_Col2 != "")))
                            {
                                if (isPrevHeader1 != item.PIR_Ques_Col1 || isPrevHeader2 != item.PIR_Ques_Col2)
                                {
                                    if (!string.IsNullOrEmpty(item.Col1_Desc) && !string.IsNullOrEmpty(item.Col2_Desc))
                                        rowindex = dgvQuestions.Rows.Add(false, "", "", item.Col1_Desc == "" ? "" : "(1) " + item.Col1_Desc, item.Col2_Desc == "" ? "" : "(2) " + item.Col2_Desc, "", "H", "", "", "", "", item.PIR_Ques_Code, item.PIR_Ques_Unique_ID);
                                    if (!string.IsNullOrEmpty(item.Col1_Desc) && string.IsNullOrEmpty(item.Col2_Desc))
                                        rowindex = dgvQuestions.Rows.Add(false, "", "", item.Col1_Desc == "" ? "" : item.Col1_Desc, "", "", "H", "", "", "", "", item.PIR_Ques_Code, item.PIR_Ques_Unique_ID);
                                    if (!string.IsNullOrEmpty(item.Col2_Desc) && string.IsNullOrEmpty(item.Col1_Desc))
                                        rowindex = dgvQuestions.Rows.Add(false, "", "", "", item.Col2_Desc == "" ? "" : item.Col2_Desc, "", "H", "", "", "", "", item.PIR_Ques_Code, item.PIR_Ques_Unique_ID);

                                    dgvQuestions.Rows[rowindex].Cells["gvSelect"].Style.Padding = new Padding(dgvQuestions.Rows[rowindex].Cells["gvSelect"].OwningColumn.Width, 0, 0, 0);

                                    dgvQuestions.Rows[rowindex].DefaultCellStyle.Font = new System.Drawing.Font("Calibri", 10.5F, System.Drawing.FontStyle.Regular);
                                    dgvQuestions.Rows[rowindex].Cells["gvHeader1"].Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#ECE8DD");//"#fff8eb");
                                    dgvQuestions.Rows[rowindex].Cells["gvHeader2"].Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#ECE8DD");
                                    //("#fff8eb");

                                    dgvQuestions.Rows[rowindex].Cells["gvQuesDesc"].Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
                                    dgvQuestions.Rows[rowindex].Cells["gvType"].Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");

                                    dgvQuestions.Rows[rowindex].Cells["gvSelect"].Style.CssStyle = "border-bottom: 0.5px Solid #e6e6e6;";
                                    dgvQuestions.Rows[rowindex].Cells["gvQuesDesc"].Style.CssStyle = "border-bottom: 0.5px Solid #e6e6e6;";
                                    dgvQuestions.Rows[rowindex].Cells["gvType"].Style.CssStyle = "border-bottom: 0.5px Solid #e6e6e6;";
                                }


                                isPrevHeader1 = item.PIR_Ques_Col1;
                                isPrevHeader2 = item.PIR_Ques_Col2;

                                rowindex = dgvQuestions.Rows.Add(false, item.PIR_Ques_Type, item.PIR_Ques_Desc, "", "", item.PIR_Ques_ID, "", item.PIR_Ques_SCode, item.PIR_Ques_Col1, item.PIR_Ques_Col2, item.PIR_Ques_Bold, item.PIR_Ques_Code, item.PIR_Ques_Unique_ID);

                                dgvQuestions.Rows[rowindex].Cells["gvSelect"].Style.CssStyle = "border-bottom: 0.5px Solid #e6e6e6;";
                                dgvQuestions.Rows[rowindex].Cells["gvQuesDesc"].Style.CssStyle = "border-bottom: 0.5px Solid #e6e6e6;";
                                dgvQuestions.Rows[rowindex].Cells["gvType"].Style.CssStyle = "border-bottom: 0.5px Solid #e6e6e6;";
                            }
                            else if (item.PIR_Ques_Col_Head_Top != "Y")
                            {
                                if (isPrevHeader1 != item.PIR_Ques_Col1 || isPrevHeader2 != item.PIR_Ques_Col2)
                                {
                                    if (!string.IsNullOrEmpty(item.Col1_Desc) && !string.IsNullOrEmpty(item.Col2_Desc))
                                        rowindex = dgvQuestions.Rows.Add(false, item.PIR_Ques_Type, item.PIR_Ques_Desc, item.Col1_Desc == "" ? "" : "(1) " + item.Col1_Desc, item.Col2_Desc == "" ? "" : "(2) " + item.Col2_Desc, item.PIR_Ques_ID, "H", item.PIR_Ques_SCode, item.PIR_Ques_Col1, item.PIR_Ques_Col2, item.PIR_Ques_Bold, item.PIR_Ques_Code, item.PIR_Ques_Unique_ID);
                                    if (!string.IsNullOrEmpty(item.Col1_Desc) && string.IsNullOrEmpty(item.Col2_Desc))
                                        rowindex = dgvQuestions.Rows.Add(false, item.PIR_Ques_Type, item.PIR_Ques_Desc, item.Col1_Desc == "" ? "" : item.Col1_Desc, "", item.PIR_Ques_ID, "H", item.PIR_Ques_SCode, item.PIR_Ques_Col1, item.PIR_Ques_Col2, item.PIR_Ques_Bold, item.PIR_Ques_Code, item.PIR_Ques_Unique_ID);
                                    if (!string.IsNullOrEmpty(item.Col2_Desc) && string.IsNullOrEmpty(item.Col1_Desc))
                                        rowindex = dgvQuestions.Rows.Add(false, item.PIR_Ques_Type, item.PIR_Ques_Desc, "", item.Col2_Desc == "" ? "" : item.Col2_Desc, item.PIR_Ques_ID, "H", item.PIR_Ques_SCode, item.PIR_Ques_Col1, item.PIR_Ques_Col2, item.PIR_Ques_Bold, item.PIR_Ques_Code, item.PIR_Ques_Unique_ID);
                                    if (string.IsNullOrEmpty(item.Col1_Desc) && string.IsNullOrEmpty(item.Col2_Desc))
                                        rowindex = dgvQuestions.Rows.Add(false, item.PIR_Ques_Type, item.PIR_Ques_Desc, item.Col1_Desc == "" ? "" : "(1) " + item.Col1_Desc, item.Col2_Desc == "" ? "" : "(2) " + item.Col2_Desc, item.PIR_Ques_ID, "", item.PIR_Ques_SCode, item.PIR_Ques_Col1, item.PIR_Ques_Col2, item.PIR_Ques_Bold, item.PIR_Ques_Code, item.PIR_Ques_Unique_ID);

                                    dgvQuestions.Rows[rowindex].Cells["gvSelect"].Style.CssStyle = "border-bottom: 0.5px Solid #e6e6e6;";
                                    dgvQuestions.Rows[rowindex].Cells["gvQuesDesc"].Style.CssStyle = "border-bottom: 0.5px Solid #e6e6e6;";
                                    dgvQuestions.Rows[rowindex].Cells["gvType"].Style.CssStyle = "border-bottom: 0.5px Solid #e6e6e6;";

                                    dgvQuestions.Rows[rowindex].Cells["gvSelect"].Style.Padding = new Padding(dgvQuestions.Rows[rowindex].Cells["gvSelect"].OwningColumn.Width, 0, 0, 0);
                                }
                                else
                                {
                                    if (item.PIR_Ques_ID != "6" && item.PIR_Ques_ID != "25")
                                    {
                                        rowindex = dgvQuestions.Rows.Add(false, item.PIR_Ques_Type, item.PIR_Ques_Desc, "", "", item.PIR_Ques_ID, "", item.PIR_Ques_SCode, item.PIR_Ques_Col1, item.PIR_Ques_Col2, item.PIR_Ques_Bold, item.PIR_Ques_Code, item.PIR_Ques_Unique_ID);
                                    }
                                    else
                                    {
                                        rowindex = dgvQuestions.Rows.Add(false, item.PIR_Ques_Type, item.PIR_Ques_Desc, "", "", item.PIR_Ques_ID, "", item.PIR_Ques_SCode, "", "", item.PIR_Ques_Bold, item.PIR_Ques_Code, item.PIR_Ques_Unique_ID);
                                    }
                                    dgvQuestions.Rows[rowindex].Cells["gvSelect"].Style.CssStyle = "border-bottom: 0.5px Solid #e6e6e6;";
                                    dgvQuestions.Rows[rowindex].Cells["gvQuesDesc"].Style.CssStyle = "border-bottom: 0.5px Solid #e6e6e6;";
                                    dgvQuestions.Rows[rowindex].Cells["gvType"].Style.CssStyle = "border-bottom: 0.5px Solid #e6e6e6;";
                                }

                                isPrevHeader1 = item.PIR_Ques_Col1;
                                isPrevHeader2 = item.PIR_Ques_Col2;


                            }

                            dgvQuestions.Rows[rowindex].Tag = item;
                            if (item.PIR_Ques_Bold.ToString().Trim() == "Y")
                            {
                                if ((item.PIR_Ques_Col1 == "" && item.PIR_Ques_Col2 == "") || (item.PIR_Ques_Col1 == "0" && item.PIR_Ques_Col2 == "0"))
                                {
                                    dgvQuestions.Rows[rowindex].DefaultCellStyle.Font = new System.Drawing.Font("Calibri", 10.5F, System.Drawing.FontStyle.Bold);
                                    dgvQuestions.Rows[rowindex].DefaultCellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#F8F4EA");

                                    dgvQuestions.Rows[rowindex].Cells["gvSelect"].Style.CssStyle = "border-bottom: 0.5px Solid #e6e6e6;";
                                    dgvQuestions.Rows[rowindex].Cells["gvQuesDesc"].Style.CssStyle = "border-bottom: 0.5px Solid #e6e6e6;";
                                    dgvQuestions.Rows[rowindex].Cells["gvType"].Style.CssStyle = "border-bottom: 0.5px Solid #e6e6e6;";

                                    dgvQuestions.Rows[rowindex].Cells["gvSelect"].Style.Padding = new Padding(dgvQuestions.Rows[rowindex].Cells["gvSelect"].OwningColumn.Width, 0, 0, 0);
                                }
                                else
                                {
                                    dgvQuestions.Rows[rowindex].DefaultCellStyle.Font = new System.Drawing.Font("Calibri", 10.5F, System.Drawing.FontStyle.Bold);
                                    dgvQuestions.Rows[rowindex].DefaultCellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#F8F4EA");

                                    dgvQuestions.Rows[rowindex].Cells["gvSelect"].Style.CssStyle = "border-bottom: 0.5px Solid #e6e6e6;";
                                    dgvQuestions.Rows[rowindex].Cells["gvQuesDesc"].Style.CssStyle = "border-bottom: 0.5px Solid #e6e6e6;";
                                    dgvQuestions.Rows[rowindex].Cells["gvType"].Style.CssStyle = "border-bottom: 0.5px Solid #e6e6e6;";
                                }
                            }
                            else
                            {
                                if ((item.PIR_Ques_Col1 == "" && item.PIR_Ques_Col2 == "") || (item.PIR_Ques_Col1 == "0" && item.PIR_Ques_Col2 == "") || (item.PIR_Ques_Col1 == "0" && item.PIR_Ques_Col2 == "0"))
                                {
                                    dgvQuestions.Rows[rowindex].DefaultCellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
                                    dgvQuestions.Rows[rowindex].Cells["gvSelect"].Style.Padding = new Padding(dgvQuestions.Rows[rowindex].Cells["gvSelect"].OwningColumn.Width, 0, 0, 0);
                                }
                                else
                                {
                                    dgvQuestions.Rows[rowindex].DefaultCellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffffff");
                                }
                            }
                            rowcnt++;
                        }

                        foreach (DataGridViewRow dgv in dgvQuestions.Rows)
                        {
                            if (dgv.Cells["gvisHeader"].Value.ToString() != "H")
                            {
                                DataRow[] dr = dtCounts.Select("PIRCOUNT_Q_ID = '" + dgv.Cells["gvQID"].Value.ToString() + "'");

                                if (dr != null && dr.Length > 0)
                                {
                                    if (dgv.Cells["gvQID"].Value.ToString() == dr[0][6].ToString())
                                    {
                                        if (dgv.Cells["gvQID"].Value.ToString() == "54" || dgv.Cells["gvQID"].Value.ToString() == "55" || dgv.Cells["gvQID"].Value.ToString() == "56" || dgv.Cells["gvQID"].Value.ToString() == "57" || dgv.Cells["gvQID"].Value.ToString() == "58" || dgv.Cells["gvQID"].Value.ToString() == "59" || dgv.Cells["gvQID"].Value.ToString() == "60" || dgv.Cells["gvQID"].Value.ToString() == "61" || dgv.Cells["gvQID"].Value.ToString() == "62" || dgv.Cells["gvQID"].Value.ToString() == "63" || dgv.Cells["gvQID"].Value.ToString() == "64")
                                        {
                                            if (dgv.Cells["Column1"].Value.ToString() == "0" || dgv.Cells["Column2"].Value.ToString() == "0")
                                            {
                                                if (dgv.Cells["Column1"].Value.ToString() == "0")
                                                    dgv.Cells["gvHeader1"].Value = "";
                                                else
                                                    dgv.Cells["gvHeader1"].Value = dr[0][7].ToString() == "" ? "$0.00" : "$" + Decimal.Parse(dr[0][7].ToString()).ToString("N", new CultureInfo("en-US"));
                                                if (dgv.Cells["Column2"].Value.ToString() == "0")
                                                    dgv.Cells["gvHeader2"].Value = "";
                                                else
                                                    dgv.Cells["gvHeader2"].Value = dr[0][8].ToString() == "" ? "$0.00" : "$" + Decimal.Parse(dr[0][8].ToString()).ToString("N", new CultureInfo("en-US"));
                                            }
                                            else
                                            {
                                                dgv.Cells["gvHeader1"].Value = dr[0][7].ToString() == "" ? "$0.00" : "$" + Decimal.Parse(dr[0][7].ToString()).ToString("N", new CultureInfo("en-US"));
                                                dgv.Cells["gvHeader2"].Value = dr[0][8].ToString() == "" ? "$0.00" : "$" + Decimal.Parse(dr[0][8].ToString()).ToString("N", new CultureInfo("en-US"));
                                            }

                                            if ((dgv.Cells["gvHeader1"].Value.ToString() == "$0.00" && dgv.Cells["gvHeader2"].Value.ToString() == "$0.00") || (dgv.Cells["gvHeader1"].Value.ToString() == "$0.00" && dgv.Cells["gvHeader2"].Value.ToString() == "") || (dgv.Cells["gvHeader1"].Value.ToString() == "" && dgv.Cells["gvHeader2"].Value.ToString() == ""))
                                            {
                                                dgv.Cells["gvSelect"].Style.Padding = new Padding(dgv.Cells["gvSelect"].OwningColumn.Width, 0, 0, 0);
                                            }
                                        }
                                        else
                                        {
                                            if (dgv.Cells["Column1"].Value.ToString() == "0" || dgv.Cells["Column2"].Value.ToString() == "0" || dgv.Cells["Column1"].Value.ToString() == "" || dgv.Cells["Column2"].Value.ToString() == "")
                                            {
                                                if (dgv.Cells["Column1"].Value.ToString() == "0" || dgv.Cells["Column1"].Value.ToString() == "")
                                                    dgv.Cells["gvHeader1"].Value = "";
                                                else
                                                    dgv.Cells["gvHeader1"].Value = dr[0][7].ToString() == "" ? 0 : Convert.ToInt32(Convert.ToDecimal(dr[0][7].ToString()));

                                                if (dgv.Cells["Column2"].Value.ToString() == "0" || dgv.Cells["Column2"].Value.ToString() == "")
                                                    dgv.Cells["gvHeader2"].Value = "";
                                                else
                                                    dgv.Cells["gvHeader2"].Value = dr[0][8].ToString() == "" ? 0 : Convert.ToInt32(Convert.ToDecimal(dr[0][8].ToString()));
                                            }
                                            else
                                            {
                                                dgv.Cells["gvHeader1"].Value = dr[0][7].ToString() == "" ? 0 : Convert.ToInt32(Convert.ToDecimal(dr[0][7].ToString()));
                                                dgv.Cells["gvHeader2"].Value = dr[0][8].ToString() == "" ? 0 : Convert.ToInt32(Convert.ToDecimal(dr[0][8].ToString()));
                                            }

                                            if ((dgv.Cells["gvHeader1"].Value.ToString() == "0" && dgv.Cells["gvHeader2"].Value.ToString() == "0") || (dgv.Cells["gvHeader1"].Value.ToString() == "0" && dgv.Cells["gvHeader2"].Value.ToString() == "") || (dgv.Cells["gvHeader1"].Value.ToString() == "" && dgv.Cells["gvHeader2"].Value.ToString() == ""))
                                            {
                                                dgv.Cells["gvSelect"].Style.Padding = new Padding(dgv.Cells["gvSelect"].OwningColumn.Width, 0, 0, 0);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (rowcnt > 0)
                            dgvQuestions.CurrentCell = dgvQuestions.Rows[0].Cells[1];

                    }
                }
                catch (Exception ex) { string error = ex.Message; }
            }
        }

        private string GetQuestionTypeDesc(string question_Type)
        {
            string QuesTypeDesc = string.Empty;
            if (!string.IsNullOrEmpty(question_Type.Trim()))
            {
                switch (question_Type)
                {
                    case "F":
                        QuesTypeDesc = "Staff Master";
                        break;
                    case "H":
                        QuesTypeDesc = "Hardcoded";
                        break;
                    case "T":
                        QuesTypeDesc = "Child Tracking";
                        break;
                    case "I":
                        QuesTypeDesc = "Client Intake";
                        break;

                }
            }
            return QuesTypeDesc;
        }

        private bool ValidateReport()
        {
            bool isValid = true;


            return isValid;
        }

        #region Vikash added on 05/01/2024 for using DevExpress to generate Excel
        int rowIndex = 0; int colIndex = 0;
        private void Genrate_Audit_Report_DevExp(DataSet dsAudit)
        {
            #region PDF Name

            string PdfName = "PIR Audit" + "_" + DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");

            PdfName = propReportPath + _baseform.UserID + "\\" + PdfName;
            try
            {
                if (!Directory.Exists(propReportPath + _baseform.UserID.Trim()))
                {
                    DirectoryInfo di = Directory.CreateDirectory(propReportPath + _baseform.UserID.Trim());
                }
            }
            catch (Exception ex)
            {
                AlertBox.Show("Error", MessageBoxIcon.Error);
            }

            try
            {
                string Tmpstr = PdfName + ".xlsx";
                if (File.Exists(Tmpstr))
                    File.Delete(Tmpstr);
            }
            catch (Exception ex)
            {
                int length = 8;
                string newFileName = System.Guid.NewGuid().ToString();
                newFileName = newFileName.Replace("-", string.Empty);

                Random_Filename = PdfName + newFileName.Substring(0, length) + ".xlsx";
            }

            if (!string.IsNullOrEmpty(Random_Filename))
                PdfName = Random_Filename;
            else
                PdfName += ".xlsx";

            #endregion

            #region Excel Generation

            try
            {
                using (DevExpress.Spreadsheet.Workbook wb = new DevExpress.Spreadsheet.Workbook())
                {
                    DevExpress_Excel_Properties styles = new DevExpress_Excel_Properties();
                    styles.sxlbook = wb;
                    styles.sxlTitleFont = "Trebuchet MS";
                    styles.sxlbodyFont = "calibri";

                    styles.getDevexpress_Excel_Properties();

                    #region Custom Styles

                    DevExpress.Spreadsheet.Style xTitleCellstyle = styles.xfnCELL_STYLE(wb, styles.sxlTitleFont, 20, "#002060", true, "#ffffff", "center", 1, 1, 1, 1, 1, "Thin", "#F8F9D0", "");
                    DevExpress.Spreadsheet.Style xTitleCellstyle2 = styles.xfnCELL_STYLE(wb, styles.sxlbodyFont, 14, "#002060", true, "#CFE6F9", "center", 1, 1, 1, 1, 1, "Thin", "#FFFFFF", "");

                    DevExpress.Spreadsheet.Style col1Desc_style = styles.xfnCELL_STYLE(wb, styles.sxlTitleFont, 8, "#000000", true, "#ECE8DD", "center", 1, 1, 1, 1, 1, "Thin", "#806000", "");

                    DevExpress.Spreadsheet.Style col2Desc_style = styles.xfnCELL_STYLE(wb, styles.sxlTitleFont, 8, "#000000", true, "#F8F4EA", "center", 1, 1, 1, 1, 1, "Thin", "#806000", "");

                    #endregion

                    DataSet ds = dsAudit;

                    #region Parameters Page
                    if (((Captain.Common.Utilities.ListItem)Cmb_Section.SelectedItem).Value.ToString() == "C")
                    {
                        DataTable dtSecC_Ques = new DataTable();
                        dtSecC_Ques = ds.Tables[0];

                        ParametersPage(wb, styles, xTitleCellstyle, xTitleCellstyle2, dtSecC_Ques);
                    }
                    else if (((Captain.Common.Utilities.ListItem)Cmb_Section.SelectedItem).Value.ToString() == "B")
                    {
                        DataTable dtSecB_Ques = new DataTable();
                        dtSecB_Ques = ds.Tables[0];

                        ParametersPage(wb, styles, xTitleCellstyle, xTitleCellstyle2, dtSecB_Ques);
                    }
                    else
                    {

                    }

                    #endregion

                    #region Excel Data

                    //Get_Multiple_Question_Audit_Table();

                    wb.Unit = DevExpress.Office.DocumentUnit.Point;
                    string col1_Desc = string.Empty;
                    string col2_Desc = string.Empty;

                    DataTable dtColHeads = new DataTable();

                    DataSet dsColHeads = Captain.DatabaseLayer.Lookups.BrowsePIRQues2(Program_Year);
                    dtColHeads = dsColHeads.Tables[0];

                    if (ds.Tables.Count > 0)
                    {
                        if (((Captain.Common.Utilities.ListItem)Cmb_Section.SelectedItem).Value.ToString() == "B")
                        {
                            DataTable dtCounts = new DataTable();
                            if (ds.Tables.Count == 4)
                                dtCounts = ds.Tables[3];
                            if (ds.Tables.Count == 5)
                                dtCounts = ds.Tables[4];
                            if (ds.Tables.Count == 6)
                                dtCounts = ds.Tables[5];

                            if (dtCounts.Rows.Count > 0)
                            {
                                foreach (DataRow drQuesID in dtCounts.Rows)
                                {
                                    if (drQuesID["QuesID"].ToString() == "55" || drQuesID["QuesID"].ToString() == "56" || drQuesID["QuesID"].ToString() == "57" || drQuesID["QuesID"].ToString() == "58" || drQuesID["QuesID"].ToString() == "59" || drQuesID["QuesID"].ToString() == "60" || drQuesID["QuesID"].ToString() == "61" || drQuesID["QuesID"].ToString() == "62")
                                    {

                                        if (drQuesID["ColHead1"].ToString() != "")
                                        {
                                            DataTable dtCol1Audit = ds.Tables[1];
                                            if (dtCol1Audit.Rows.Count > 0)
                                            {
                                                DevExpress.Spreadsheet.Worksheet Col1Sheet = wb.Worksheets.Add("Header_1 " + "Audit" + drQuesID["QuesID"].ToString());
                                                Col1Sheet.ActiveView.ShowGridlines = false;
                                                rowIndex = 1;
                                                colIndex = 1;

                                                Col1Sheet.Columns[1].Width = 40;
                                                Col1Sheet.Columns[2].Width = 80;
                                                Col1Sheet.Columns[3].Width = 100;
                                                Col1Sheet.Columns[4].Width = 100;
                                                Col1Sheet.Columns[5].Width = 40;
                                                Col1Sheet.Columns[6].Width = 150;
                                                Col1Sheet.Columns[7].Width = 100;
                                                Col1Sheet.Columns[8].Width = 170;
                                                Col1Sheet.Columns[9].Width = 200;
                                                Col1Sheet.Columns[10].Width = 100;
                                                Col1Sheet.Columns[11].Width = 200;
                                                Col1Sheet.Columns[12].Width = 100;
                                                Col1Sheet.Columns[13].Width = 100;

                                                Col1Sheet.Columns[14].Width = 100;
                                                Col1Sheet.Columns[15].Width = 100;
                                                Col1Sheet.Columns[16].Width = 80;
                                                Col1Sheet.Columns[17].Width = 80;
                                                Col1Sheet.Columns[18].Width = 80;
                                                Col1Sheet.Columns[19].Width = 80;
                                                Col1Sheet.Columns[20].Width = 80;
                                                Col1Sheet.Columns[21].Width = 100;
                                                Col1Sheet.Columns[22].Width = 60;
                                                Col1Sheet.Columns[23].Width = 60;
                                                Col1Sheet.Columns[24].Width = 60;
                                                Col1Sheet.Columns[25].Width = 80;
                                                Col1Sheet.Columns[26].Width = 80;
                                                Col1Sheet.Columns[27].Width = 80;
                                                Col1Sheet.Columns[28].Width = 100;
                                                Col1Sheet.Columns[29].Width = 80;
                                                Col1Sheet.Columns[30].Width = 120;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = "Hit No";
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = "Employee No";
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = "First Name";
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = "Last Name";
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = "Active";
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = "Ethnicity";
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = "Race";
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = "Education Completed";
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = "Education Progress";
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = "Date Terminated";
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = "Reason For Termination";
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = "Primary Language";
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = "Language 1";
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = "Language 2";
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = "Language 3";
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = "Work For HS";
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = "Work For EHS";
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = "Work For Non HS";
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = "Work For Volunteer";
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = "Work For Contracted";
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = "Positional Category";
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = "Hourly Rate";
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBRHC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = "Std Hours";
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBRHC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = "Salary";
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBRHC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = "Weeks Worked";
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBRHC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = "Head Start Parent";
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = "Daycare Program Parent";
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = "Early Headestart Parent";
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = "Position Filled after 3 Months";
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = "This Individual replaced another staff member";
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;

                                                foreach (DataRow dr in dtCol1Audit.Rows)
                                                {
                                                    rowIndex++;

                                                    colIndex = 1;
                                                    if (!string.IsNullOrEmpty(dr["HIT_NUM"].ToString().Trim()))
                                                    {
                                                        Col1Sheet.Rows[rowIndex][colIndex].Value = Convert.ToInt32(dr["HIT_NUM"].ToString().Trim());
                                                    }
                                                    Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNumb_NCC;
                                                    colIndex++;

                                                    Col1Sheet.Rows[rowIndex][colIndex].Value = dr["STF_CODE"].ToString().Trim() == "" ? "" : dr["STF_CODE"].ToString().Trim();
                                                    Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                    colIndex++;

                                                    Col1Sheet.Rows[rowIndex][colIndex].Value = dr["STF_NAME_FI"].ToString().Trim() == "" ? "" : dr["STF_NAME_FI"].ToString().Trim();
                                                    Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                    colIndex++;

                                                    Col1Sheet.Rows[rowIndex][colIndex].Value = dr["STF_NAME_LAST"].ToString().Trim() == "" ? "" : dr["STF_NAME_LAST"].ToString().Trim();
                                                    Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                    colIndex++;

                                                    Col1Sheet.Rows[rowIndex][colIndex].Value = dr["STF_ACTIVE"].ToString().Trim() == "" ? "" : dr["STF_ACTIVE"].ToString().Trim();
                                                    Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                    colIndex++;

                                                    Col1Sheet.Rows[rowIndex][colIndex].Value = dr["ETHNICITY"].ToString().Trim() == "" ? "" : dr["ETHNICITY"].ToString().Trim();
                                                    Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                    colIndex++;

                                                    Col1Sheet.Rows[rowIndex][colIndex].Value = dr["Race"].ToString().Trim() == "" ? "" : dr["Race"].ToString().Trim().Trim();
                                                    Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                    colIndex++;

                                                    Col1Sheet.Rows[rowIndex][colIndex].Value = (dr["Education"].ToString().Trim() == "" ? "" : dr["Education"].ToString().Trim());
                                                    Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                    colIndex++;

                                                    Col1Sheet.Rows[rowIndex][colIndex].Value = dr["EDUCATION_PROGRESS"].ToString().Trim() == "" ? "" : dr["EDUCATION_PROGRESS"].ToString().Trim().Trim();
                                                    Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                    colIndex++;

                                                    Col1Sheet.Rows[rowIndex][colIndex].Value = dr["STF_DATE_TERMINATED"].ToString().Trim() == "" ? "" : Convert.ToDateTime(dr["STF_DATE_TERMINATED"]).ToString("MM/dd/yyyy").ToString().Trim();
                                                    Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                    colIndex++;

                                                    Col1Sheet.Rows[rowIndex][colIndex].Value = dr["RES_TERMINATION"].ToString().Trim() == "" ? "" : dr["RES_TERMINATION"].ToString().Trim();
                                                    Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                    colIndex++;

                                                    Col1Sheet.Rows[rowIndex][colIndex].Value = dr["PRIM_LANGUAGE"].ToString().Trim() == "" ? "" : dr["PRIM_LANGUAGE"].ToString().Trim();
                                                    Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                    colIndex++;

                                                    Col1Sheet.Rows[rowIndex][colIndex].Value = dr["LANGUAGE1"].ToString().Trim() == "" ? "" : dr["LANGUAGE1"].ToString().Trim();
                                                    Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                    colIndex++;

                                                    Col1Sheet.Rows[rowIndex][colIndex].Value = dr["LANGUAGE2"].ToString().Trim() == "" ? "" : dr["LANGUAGE2"].ToString().Trim();
                                                    Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                    colIndex++;

                                                    Col1Sheet.Rows[rowIndex][colIndex].Value = dr["LANGUAGE3"].ToString().Trim() == "" ? "" : dr["LANGUAGE3"].ToString().Trim();
                                                    Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                    colIndex++;

                                                    Col1Sheet.Rows[rowIndex][colIndex].Value = dr["STF_WORKFOR_HS"].ToString().Trim() == "" ? "" : dr["STF_WORKFOR_HS"].ToString().Trim();
                                                    Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                    colIndex++;

                                                    Col1Sheet.Rows[rowIndex][colIndex].Value = dr["STF_WORKFOR_EHS"].ToString().Trim() == "" ? "" : dr["STF_WORKFOR_EHS"].ToString().Trim();
                                                    Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                    colIndex++;

                                                    Col1Sheet.Rows[rowIndex][colIndex].Value = dr["STF_WORKFOR_NONHS"].ToString().Trim() == "" ? "" : dr["STF_WORKFOR_NONHS"].ToString().Trim();
                                                    Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                    colIndex++;

                                                    Col1Sheet.Rows[rowIndex][colIndex].Value = dr["STF_WORKFOR_VOL"].ToString().Trim() == "" ? "" : dr["STF_WORKFOR_VOL"].ToString().Trim();
                                                    Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                    colIndex++;

                                                    Col1Sheet.Rows[rowIndex][colIndex].Value = dr["STF_WORKFOR_CONT"].ToString().Trim() == "" ? "" : dr["STF_WORKFOR_CONT"].ToString().Trim();
                                                    Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                    colIndex++;

                                                    Col1Sheet.Rows[rowIndex][colIndex].Value = dr["POS_CATG"].ToString().Trim() == "" ? "" : dr["POS_CATG"].ToString().Trim();
                                                    Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                    colIndex++;

                                                    Col1Sheet.Rows[rowIndex][colIndex].Value = dr["STF_BASE_RATE"].ToString().Trim() == "" ? 0 : Convert
                                                    .ToDecimal(dr["STF_BASE_RATE"].ToString().Trim());
                                                    Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlDeci_NRC;
                                                    colIndex++;

                                                    Col1Sheet.Rows[rowIndex][colIndex].Value = dr["STF_HRS_WORKED_PW"].ToString().Trim() == "" ? 0 : Convert
                                                        .ToDecimal(dr["STF_HRS_WORKED_PW"].ToString());
                                                    Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlDeci_NRC;
                                                    colIndex++;

                                                    Col1Sheet.Rows[rowIndex][colIndex].Value = dr["STF_SALARY"].ToString().Trim() == "" ? 0 : Convert
                                                        .ToDecimal(dr["STF_SALARY"].ToString().Trim());
                                                    Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlDeci_NRC;
                                                    colIndex++;

                                                    Col1Sheet.Rows[rowIndex][colIndex].Value = dr["STF_WEEKS_WORKED"].ToString().Trim() == "" ? 0 : Convert
                                                        .ToInt32(dr["STF_WEEKS_WORKED"].ToString().Trim());
                                                    Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNumb_NRC;
                                                    colIndex++;

                                                    Col1Sheet.Rows[rowIndex][colIndex].Value = dr["HS_PARENT"].ToString().Trim() == "" ? "" : dr["HS_PARENT"].ToString().Trim();
                                                    Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                    colIndex++;

                                                    Col1Sheet.Rows[rowIndex][colIndex].Value = dr["DAYCARE_PARENT"].ToString().Trim() == "" ? "" : dr["DAYCARE_PARENT"].ToString().Trim();
                                                    Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                    colIndex++;

                                                    Col1Sheet.Rows[rowIndex][colIndex].Value = dr["EHS_PARESNT"].ToString().Trim() == "" ? "" : dr["EHS_PARESNT"].ToString().Trim();
                                                    Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                    colIndex++;

                                                    Col1Sheet.Rows[rowIndex][colIndex].Value = dr["POS_FILLED"].ToString().Trim() == "" ? "" : dr["POS_FILLED"].ToString().Trim();
                                                    Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                    colIndex++;

                                                    Col1Sheet.Rows[rowIndex][colIndex].Value = dr["REPLACE_SM"].ToString().Trim() == "" ? "" : dr["REPLACE_SM"].ToString().Trim();
                                                    Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                }
                                            }
                                        }
                                        if (drQuesID["ColHead2"].ToString() != "" && dgvQuestions.SelectedRows[0].Cells["gvHeader2"].Value.ToString() != "")
                                        {
                                            DataTable dtCol2Audit = ds.Tables[2];

                                            if (dtCol2Audit.Rows.Count > 0)
                                            {
                                                DevExpress.Spreadsheet.Worksheet Col2Sheet = wb.Worksheets.Add("Header_2 " + "Audit" + drQuesID["QuesID"].ToString());
                                                Col2Sheet.ActiveView.ShowGridlines = false;
                                                rowIndex = 1;
                                                colIndex = 1;

                                                Col2Sheet.Columns[1].Width = 40;
                                                Col2Sheet.Columns[2].Width = 80;
                                                Col2Sheet.Columns[3].Width = 100;
                                                Col2Sheet.Columns[4].Width = 100;
                                                Col2Sheet.Columns[5].Width = 40;
                                                Col2Sheet.Columns[6].Width = 150;
                                                Col2Sheet.Columns[7].Width = 100;
                                                Col2Sheet.Columns[8].Width = 170;
                                                Col2Sheet.Columns[9].Width = 200;
                                                Col2Sheet.Columns[10].Width = 100;
                                                Col2Sheet.Columns[11].Width = 200;
                                                Col2Sheet.Columns[12].Width = 100;
                                                Col2Sheet.Columns[13].Width = 100;

                                                Col2Sheet.Columns[14].Width = 100;
                                                Col2Sheet.Columns[15].Width = 100;
                                                Col2Sheet.Columns[16].Width = 80;
                                                Col2Sheet.Columns[17].Width = 80;
                                                Col2Sheet.Columns[18].Width = 80;
                                                Col2Sheet.Columns[19].Width = 80;
                                                Col2Sheet.Columns[20].Width = 80;
                                                Col2Sheet.Columns[21].Width = 100;
                                                Col2Sheet.Columns[22].Width = 60;
                                                Col2Sheet.Columns[23].Width = 60;
                                                Col2Sheet.Columns[24].Width = 60;
                                                Col2Sheet.Columns[25].Width = 80;
                                                Col2Sheet.Columns[26].Width = 80;
                                                Col2Sheet.Columns[27].Width = 80;
                                                Col2Sheet.Columns[28].Width = 100;
                                                Col2Sheet.Columns[29].Width = 80;
                                                Col2Sheet.Columns[30].Width = 120;

                                                Col2Sheet.Rows[rowIndex][colIndex].Value = "Hit No";
                                                Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                                colIndex++;

                                                Col2Sheet.Rows[rowIndex][colIndex].Value = "Employee No";
                                                Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                colIndex++;

                                                Col2Sheet.Rows[rowIndex][colIndex].Value = "First Name";
                                                Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                colIndex++;

                                                Col2Sheet.Rows[rowIndex][colIndex].Value = "Last Name";
                                                Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                colIndex++;

                                                Col2Sheet.Rows[rowIndex][colIndex].Value = "Active";
                                                Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                                colIndex++;

                                                Col2Sheet.Rows[rowIndex][colIndex].Value = "Ethnicity";
                                                Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                colIndex++;

                                                Col2Sheet.Rows[rowIndex][colIndex].Value = "Race";
                                                Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                colIndex++;

                                                Col2Sheet.Rows[rowIndex][colIndex].Value = "Education Completed";
                                                Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                colIndex++;

                                                Col2Sheet.Rows[rowIndex][colIndex].Value = "Education Progress";
                                                Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                colIndex++;

                                                Col2Sheet.Rows[rowIndex][colIndex].Value = "Date Terminated";
                                                Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                colIndex++;

                                                Col2Sheet.Rows[rowIndex][colIndex].Value = "Reason For Termination";
                                                Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                colIndex++;

                                                Col2Sheet.Rows[rowIndex][colIndex].Value = "Primary Language";
                                                Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                colIndex++;

                                                Col2Sheet.Rows[rowIndex][colIndex].Value = "Language 1";
                                                Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                colIndex++;

                                                Col2Sheet.Rows[rowIndex][colIndex].Value = "Language 2";
                                                Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                colIndex++;

                                                Col2Sheet.Rows[rowIndex][colIndex].Value = "Language 3";
                                                Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                colIndex++;

                                                Col2Sheet.Rows[rowIndex][colIndex].Value = "Work For HS";
                                                Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                                colIndex++;

                                                Col2Sheet.Rows[rowIndex][colIndex].Value = "Work For EHS";
                                                Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                                colIndex++;

                                                Col2Sheet.Rows[rowIndex][colIndex].Value = "Work For Non HS";
                                                Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                                colIndex++;

                                                Col2Sheet.Rows[rowIndex][colIndex].Value = "Work For Volunteer";
                                                Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                                colIndex++;

                                                Col2Sheet.Rows[rowIndex][colIndex].Value = "Work For Contracted";
                                                Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                                colIndex++;

                                                Col2Sheet.Rows[rowIndex][colIndex].Value = "Positional Category";
                                                Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                colIndex++;

                                                Col2Sheet.Rows[rowIndex][colIndex].Value = "Hourly Rate";
                                                Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBRHC;
                                                colIndex++;

                                                Col2Sheet.Rows[rowIndex][colIndex].Value = "Std Hours";
                                                Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBRHC;
                                                colIndex++;

                                                Col2Sheet.Rows[rowIndex][colIndex].Value = "Salary";
                                                Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBRHC;
                                                colIndex++;

                                                Col2Sheet.Rows[rowIndex][colIndex].Value = "Weeks Worked";
                                                Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBRHC;
                                                colIndex++;

                                                Col2Sheet.Rows[rowIndex][colIndex].Value = "Head Start Parent";
                                                Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                                colIndex++;

                                                Col2Sheet.Rows[rowIndex][colIndex].Value = "Daycare Program Parent";
                                                Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                                colIndex++;

                                                Col2Sheet.Rows[rowIndex][colIndex].Value = "Early Headestart Parent";
                                                Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                                colIndex++;

                                                Col2Sheet.Rows[rowIndex][colIndex].Value = "Position Filled after 3 Months";
                                                Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                                colIndex++;

                                                Col2Sheet.Rows[rowIndex][colIndex].Value = "This Individual replaced another staff member";
                                                Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;

                                                foreach (DataRow dr in dtCol2Audit.Rows)
                                                {
                                                    rowIndex++;

                                                    colIndex = 1;
                                                    if (!string.IsNullOrEmpty(dr["HIT_NUM"].ToString().Trim()))
                                                        Col2Sheet.Rows[rowIndex][colIndex].Value = Convert.ToInt32(dr["HIT_NUM"].ToString().Trim());
                                                    Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNumb_NCC;
                                                    colIndex++;

                                                    Col2Sheet.Rows[rowIndex][colIndex].Value = dr["STF_CODE"].ToString().Trim() == "" ? "" : dr["STF_CODE"].ToString().Trim();
                                                    Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                    colIndex++;

                                                    Col2Sheet.Rows[rowIndex][colIndex].Value = dr["STF_NAME_FI"].ToString().Trim() == "" ? "" : dr["STF_NAME_FI"].ToString().Trim();
                                                    Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                    colIndex++;

                                                    Col2Sheet.Rows[rowIndex][colIndex].Value = dr["STF_NAME_LAST"].ToString().Trim() == "" ? "" : dr["STF_NAME_LAST"].ToString().Trim();
                                                    Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                    colIndex++;

                                                    Col2Sheet.Rows[rowIndex][colIndex].Value = dr["STF_ACTIVE"].ToString().Trim() == "" ? "" : dr["STF_ACTIVE"].ToString().Trim();
                                                    Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                    colIndex++;

                                                    Col2Sheet.Rows[rowIndex][colIndex].Value = dr["ETHNICITY"].ToString().Trim() == "" ? "" : dr["ETHNICITY"].ToString().Trim();
                                                    Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                    colIndex++;

                                                    Col2Sheet.Rows[rowIndex][colIndex].Value = dr["Race"].ToString().Trim() == "" ? "" : dr["Race"].ToString().Trim().Trim();
                                                    Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                    colIndex++;

                                                    Col2Sheet.Rows[rowIndex][colIndex].Value = (dr["Education"].ToString().Trim() == "" ? "" : dr["Education"].ToString().Trim());
                                                    Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                    colIndex++;

                                                    Col2Sheet.Rows[rowIndex][colIndex].Value = dr["EDUCATION_PROGRESS"].ToString().Trim() == "" ? "" : dr["EDUCATION_PROGRESS"].ToString().Trim().Trim();
                                                    Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                    colIndex++;

                                                    Col2Sheet.Rows[rowIndex][colIndex].Value = dr["STF_DATE_TERMINATED"].ToString().Trim() == "" ? "" : Convert.ToDateTime(dr["STF_DATE_TERMINATED"]).ToString("MM/dd/yyyy").ToString().Trim();
                                                    Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                    colIndex++;

                                                    Col2Sheet.Rows[rowIndex][colIndex].Value = dr["RES_TERMINATION"].ToString().Trim() == "" ? "" : dr["RES_TERMINATION"].ToString().Trim();
                                                    Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                    colIndex++;

                                                    Col2Sheet.Rows[rowIndex][colIndex].Value = dr["PRIM_LANGUAGE"].ToString().Trim() == "" ? "" : dr["PRIM_LANGUAGE"].ToString().Trim();
                                                    Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                    colIndex++;

                                                    Col2Sheet.Rows[rowIndex][colIndex].Value = dr["LANGUAGE1"].ToString().Trim() == "" ? "" : dr["LANGUAGE1"].ToString().Trim();
                                                    Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                    colIndex++;

                                                    Col2Sheet.Rows[rowIndex][colIndex].Value = dr["LANGUAGE2"].ToString().Trim() == "" ? "" : dr["LANGUAGE2"].ToString().Trim();
                                                    Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                    colIndex++;

                                                    Col2Sheet.Rows[rowIndex][colIndex].Value = dr["LANGUAGE3"].ToString().Trim() == "" ? "" : dr["LANGUAGE3"].ToString().Trim();
                                                    Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                    colIndex++;

                                                    Col2Sheet.Rows[rowIndex][colIndex].Value = dr["STF_WORKFOR_HS"].ToString().Trim() == "" ? "" : dr["STF_WORKFOR_HS"].ToString().Trim();
                                                    Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                    colIndex++;

                                                    Col2Sheet.Rows[rowIndex][colIndex].Value = dr["STF_WORKFOR_EHS"].ToString().Trim() == "" ? "" : dr["STF_WORKFOR_EHS"].ToString().Trim();
                                                    Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                    colIndex++;

                                                    Col2Sheet.Rows[rowIndex][colIndex].Value = dr["STF_WORKFOR_NONHS"].ToString().Trim() == "" ? "" : dr["STF_WORKFOR_NONHS"].ToString().Trim();
                                                    Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                    colIndex++;

                                                    Col2Sheet.Rows[rowIndex][colIndex].Value = dr["STF_WORKFOR_VOL"].ToString().Trim() == "" ? "" : dr["STF_WORKFOR_VOL"].ToString().Trim();
                                                    Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                    colIndex++;

                                                    Col2Sheet.Rows[rowIndex][colIndex].Value = dr["STF_WORKFOR_CONT"].ToString().Trim() == "" ? "" : dr["STF_WORKFOR_CONT"].ToString().Trim();
                                                    Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                    colIndex++;

                                                    Col2Sheet.Rows[rowIndex][colIndex].Value = dr["POS_CATG"].ToString().Trim() == "" ? "" : dr["POS_CATG"].ToString().Trim();
                                                    Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                    colIndex++;

                                                    Col2Sheet.Rows[rowIndex][colIndex].Value = dr["STF_BASE_RATE"].ToString().Trim() == "" ? 0 : Convert
                                                        .ToDecimal(dr["STF_BASE_RATE"].ToString().Trim());
                                                    Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlDeci_NRC;
                                                    colIndex++;

                                                    Col2Sheet.Rows[rowIndex][colIndex].Value = dr["STF_HRS_WORKED_PW"].ToString().Trim() == "" ? 0 : Convert
                                                        .ToDecimal(dr["STF_HRS_WORKED_PW"].ToString());
                                                    Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlDeci_NRC;
                                                    colIndex++;

                                                    Col2Sheet.Rows[rowIndex][colIndex].Value = dr["STF_SALARY"].ToString().Trim() == "" ? 0 : Convert
                                                        .ToDecimal(dr["STF_SALARY"].ToString().Trim());
                                                    Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlDeci_NRC;
                                                    colIndex++;

                                                    Col2Sheet.Rows[rowIndex][colIndex].Value = dr["STF_WEEKS_WORKED"].ToString().Trim() == "" ? 0 : Convert
                                                        .ToInt32(dr["STF_WEEKS_WORKED"].ToString().Trim());
                                                    Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNumb_NRC;
                                                    colIndex++;

                                                    Col2Sheet.Rows[rowIndex][colIndex].Value = dr["HS_PARENT"].ToString().Trim() == "" ? "" : dr["HS_PARENT"].ToString().Trim();
                                                    Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                    colIndex++;

                                                    Col2Sheet.Rows[rowIndex][colIndex].Value = dr["DAYCARE_PARENT"].ToString().Trim() == "" ? "" : dr["DAYCARE_PARENT"].ToString().Trim();
                                                    Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                    colIndex++;

                                                    Col2Sheet.Rows[rowIndex][colIndex].Value = dr["EHS_PARESNT"].ToString().Trim() == "" ? "" : dr["EHS_PARESNT"].ToString().Trim();
                                                    Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                    colIndex++;

                                                    Col2Sheet.Rows[rowIndex][colIndex].Value = dr["POS_FILLED"].ToString().Trim() == "" ? "" : dr["POS_FILLED"].ToString().Trim();
                                                    Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                    colIndex++;

                                                    Col2Sheet.Rows[rowIndex][colIndex].Value = dr["REPLACE_SM"].ToString().Trim() == "" ? "" : dr["REPLACE_SM"].ToString().Trim();
                                                    Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (drQuesID["ColHead1"].ToString() != "" && drQuesID["ColHead1"].ToString() != "0.00")
                                        {
                                            DataTable dtCol1Audit = ds.Tables[1];
                                            if (dtCol1Audit.Rows.Count > 0)
                                            {
                                                DevExpress.Spreadsheet.Worksheet Col1Sheet = wb.Worksheets.Add("Header_1 " + "Audit" + drQuesID["QuesID"].ToString());
                                                Col1Sheet.ActiveView.ShowGridlines = false;
                                                rowIndex = 1;
                                                colIndex = 1;

                                                Col1Sheet.Columns[1].Width = 40;
                                                Col1Sheet.Columns[2].Width = 80;
                                                Col1Sheet.Columns[3].Width = 100;
                                                Col1Sheet.Columns[4].Width = 100;
                                                Col1Sheet.Columns[5].Width = 40;
                                                Col1Sheet.Columns[6].Width = 150;
                                                Col1Sheet.Columns[7].Width = 100;
                                                Col1Sheet.Columns[8].Width = 170;
                                                Col1Sheet.Columns[9].Width = 200;
                                                Col1Sheet.Columns[10].Width = 100;
                                                Col1Sheet.Columns[11].Width = 200;
                                                Col1Sheet.Columns[12].Width = 100;
                                                Col1Sheet.Columns[13].Width = 100;

                                                Col1Sheet.Columns[14].Width = 100;
                                                Col1Sheet.Columns[15].Width = 100;
                                                Col1Sheet.Columns[16].Width = 80;
                                                Col1Sheet.Columns[17].Width = 80;
                                                Col1Sheet.Columns[18].Width = 80;
                                                Col1Sheet.Columns[19].Width = 80;
                                                Col1Sheet.Columns[20].Width = 80;
                                                Col1Sheet.Columns[21].Width = 100;
                                                Col1Sheet.Columns[22].Width = 60;
                                                Col1Sheet.Columns[23].Width = 60;
                                                Col1Sheet.Columns[24].Width = 60;
                                                Col1Sheet.Columns[25].Width = 80;
                                                Col1Sheet.Columns[26].Width = 80;
                                                Col1Sheet.Columns[27].Width = 80;
                                                Col1Sheet.Columns[28].Width = 100;
                                                Col1Sheet.Columns[29].Width = 80;
                                                Col1Sheet.Columns[30].Width = 120;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = "Hit No";
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = "Employee No";
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = "First Name";
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = "Last Name";
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = "Active";
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = "Ethnicity";
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = "Race";
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = "Education Completed";
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = "Education Progress";
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = "Date Terminated";
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = "Reason For Termination";
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = "Primary Language";
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = "Language 1";
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = "Language 2";
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = "Language 3";
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = "Work For HS";
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = "Work For EHS";
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = "Work For Non HS";
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = "Work For Volunteer";
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = "Work For Contracted";
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = "Positional Category";
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = "Hourly Rate";
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBRHC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = "Std Hours";
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBRHC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = "Salary";
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBRHC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = "Weeks Worked";
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBRHC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = "Head Start Parent";
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = "Daycare Program Parent";
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = "Early Headestart Parent";
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = "Position Filled after 3 Months";
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = "This Individual replaced another staff member";
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;

                                                foreach (DataRow dr in dtCol1Audit.Rows)
                                                {
                                                    rowIndex++;

                                                    colIndex = 1;
                                                    if (!string.IsNullOrEmpty(dr["HIT_NUM"].ToString().Trim()))
                                                    {
                                                        Col1Sheet.Rows[rowIndex][colIndex].Value = Convert.ToInt32(dr["HIT_NUM"].ToString().Trim());
                                                    }
                                                    Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNumb_NCC;
                                                    colIndex++;

                                                    Col1Sheet.Rows[rowIndex][colIndex].Value = dr["STF_CODE"].ToString().Trim() == "" ? "" : dr["STF_CODE"].ToString().Trim();
                                                    Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                    colIndex++;

                                                    Col1Sheet.Rows[rowIndex][colIndex].Value = dr["STF_NAME_FI"].ToString().Trim() == "" ? "" : dr["STF_NAME_FI"].ToString().Trim();
                                                    Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                    colIndex++;

                                                    Col1Sheet.Rows[rowIndex][colIndex].Value = dr["STF_NAME_LAST"].ToString().Trim() == "" ? "" : dr["STF_NAME_LAST"].ToString().Trim();
                                                    Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                    colIndex++;

                                                    Col1Sheet.Rows[rowIndex][colIndex].Value = dr["STF_ACTIVE"].ToString().Trim() == "" ? "" : dr["STF_ACTIVE"].ToString().Trim();
                                                    Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                    colIndex++;

                                                    Col1Sheet.Rows[rowIndex][colIndex].Value = dr["ETHNICITY"].ToString().Trim() == "" ? "" : dr["ETHNICITY"].ToString().Trim();
                                                    Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                    colIndex++;

                                                    Col1Sheet.Rows[rowIndex][colIndex].Value = dr["Race"].ToString().Trim() == "" ? "" : dr["Race"].ToString().Trim().Trim();
                                                    Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                    colIndex++;

                                                    Col1Sheet.Rows[rowIndex][colIndex].Value = (dr["Education"].ToString().Trim() == "" ? "" : dr["Education"].ToString().Trim());
                                                    Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                    colIndex++;

                                                    Col1Sheet.Rows[rowIndex][colIndex].Value = dr["EDUCATION_PROGRESS"].ToString().Trim() == "" ? "" : dr["EDUCATION_PROGRESS"].ToString().Trim().Trim();
                                                    Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                    colIndex++;

                                                    Col1Sheet.Rows[rowIndex][colIndex].Value = dr["STF_DATE_TERMINATED"].ToString().Trim() == "" ? "" : Convert.ToDateTime(dr["STF_DATE_TERMINATED"]).ToString("MM/dd/yyyy").ToString().Trim();
                                                    Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                    colIndex++;

                                                    Col1Sheet.Rows[rowIndex][colIndex].Value = dr["RES_TERMINATION"].ToString().Trim() == "" ? "" : dr["RES_TERMINATION"].ToString().Trim();
                                                    Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                    colIndex++;

                                                    Col1Sheet.Rows[rowIndex][colIndex].Value = dr["PRIM_LANGUAGE"].ToString().Trim() == "" ? "" : dr["PRIM_LANGUAGE"].ToString().Trim();
                                                    Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                    colIndex++;

                                                    Col1Sheet.Rows[rowIndex][colIndex].Value = dr["LANGUAGE1"].ToString().Trim() == "" ? "" : dr["LANGUAGE1"].ToString().Trim();
                                                    Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                    colIndex++;

                                                    Col1Sheet.Rows[rowIndex][colIndex].Value = dr["LANGUAGE2"].ToString().Trim() == "" ? "" : dr["LANGUAGE2"].ToString().Trim();
                                                    Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                    colIndex++;

                                                    Col1Sheet.Rows[rowIndex][colIndex].Value = dr["LANGUAGE3"].ToString().Trim() == "" ? "" : dr["LANGUAGE3"].ToString().Trim();
                                                    Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                    colIndex++;

                                                    Col1Sheet.Rows[rowIndex][colIndex].Value = dr["STF_WORKFOR_HS"].ToString().Trim() == "" ? "" : dr["STF_WORKFOR_HS"].ToString().Trim();
                                                    Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                    colIndex++;

                                                    Col1Sheet.Rows[rowIndex][colIndex].Value = dr["STF_WORKFOR_EHS"].ToString().Trim() == "" ? "" : dr["STF_WORKFOR_EHS"].ToString().Trim();
                                                    Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                    colIndex++;

                                                    Col1Sheet.Rows[rowIndex][colIndex].Value = dr["STF_WORKFOR_NONHS"].ToString().Trim() == "" ? "" : dr["STF_WORKFOR_NONHS"].ToString().Trim();
                                                    Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                    colIndex++;

                                                    Col1Sheet.Rows[rowIndex][colIndex].Value = dr["STF_WORKFOR_VOL"].ToString().Trim() == "" ? "" : dr["STF_WORKFOR_VOL"].ToString().Trim();
                                                    Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                    colIndex++;

                                                    Col1Sheet.Rows[rowIndex][colIndex].Value = dr["STF_WORKFOR_CONT"].ToString().Trim() == "" ? "" : dr["STF_WORKFOR_CONT"].ToString().Trim();
                                                    Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                    colIndex++;

                                                    Col1Sheet.Rows[rowIndex][colIndex].Value = dr["POS_CATG"].ToString().Trim() == "" ? "" : dr["POS_CATG"].ToString().Trim();
                                                    Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                    colIndex++;

                                                    Col1Sheet.Rows[rowIndex][colIndex].Value = dr["STF_BASE_RATE"].ToString().Trim() == "" ? 0 : Convert
                                                    .ToDecimal(dr["STF_BASE_RATE"].ToString().Trim());
                                                    Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlDeci_NRC;
                                                    colIndex++;

                                                    Col1Sheet.Rows[rowIndex][colIndex].Value = dr["STF_HRS_WORKED_PW"].ToString().Trim() == "" ? 0 : Convert
                                                        .ToDecimal(dr["STF_HRS_WORKED_PW"].ToString());
                                                    Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlDeci_NRC;
                                                    colIndex++;

                                                    Col1Sheet.Rows[rowIndex][colIndex].Value = dr["STF_SALARY"].ToString().Trim() == "" ? 0 : Convert
                                                        .ToDecimal(dr["STF_SALARY"].ToString().Trim());
                                                    Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlDeci_NRC;
                                                    colIndex++;

                                                    Col1Sheet.Rows[rowIndex][colIndex].Value = dr["STF_WEEKS_WORKED"].ToString().Trim() == "" ? 0 : Convert
                                                        .ToInt32(dr["STF_WEEKS_WORKED"].ToString().Trim());
                                                    Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNumb_NRC;
                                                    colIndex++;

                                                    Col1Sheet.Rows[rowIndex][colIndex].Value = dr["HS_PARENT"].ToString().Trim() == "" ? "" : dr["HS_PARENT"].ToString().Trim();
                                                    Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                    colIndex++;

                                                    Col1Sheet.Rows[rowIndex][colIndex].Value = dr["DAYCARE_PARENT"].ToString().Trim() == "" ? "" : dr["DAYCARE_PARENT"].ToString().Trim();
                                                    Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                    colIndex++;

                                                    Col1Sheet.Rows[rowIndex][colIndex].Value = dr["EHS_PARESNT"].ToString().Trim() == "" ? "" : dr["EHS_PARESNT"].ToString().Trim();
                                                    Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                    colIndex++;

                                                    Col1Sheet.Rows[rowIndex][colIndex].Value = dr["POS_FILLED"].ToString().Trim() == "" ? "" : dr["POS_FILLED"].ToString().Trim();
                                                    Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                    colIndex++;

                                                    Col1Sheet.Rows[rowIndex][colIndex].Value = dr["REPLACE_SM"].ToString().Trim() == "" ? "" : dr["REPLACE_SM"].ToString().Trim();
                                                    Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                }
                                            }
                                        }
                                        if (drQuesID["ColHead2"].ToString() != "" && drQuesID["ColHead2"].ToString() != "0.00")
                                        {
                                            DataTable dtCol2Audit = ds.Tables[2];

                                            if (dtCol2Audit.Rows.Count > 0)
                                            {
                                                DevExpress.Spreadsheet.Worksheet Col2Sheet = wb.Worksheets.Add("Header_2 " + "Audit" + drQuesID["QuesID"].ToString());
                                                Col2Sheet.ActiveView.ShowGridlines = false;
                                                rowIndex = 1;
                                                colIndex = 1;

                                                Col2Sheet.Columns[1].Width = 40;
                                                Col2Sheet.Columns[2].Width = 80;
                                                Col2Sheet.Columns[3].Width = 100;
                                                Col2Sheet.Columns[4].Width = 100;
                                                Col2Sheet.Columns[5].Width = 40;
                                                Col2Sheet.Columns[6].Width = 150;
                                                Col2Sheet.Columns[7].Width = 100;
                                                Col2Sheet.Columns[8].Width = 170;
                                                Col2Sheet.Columns[9].Width = 200;
                                                Col2Sheet.Columns[10].Width = 100;
                                                Col2Sheet.Columns[11].Width = 200;
                                                Col2Sheet.Columns[12].Width = 100;
                                                Col2Sheet.Columns[13].Width = 100;

                                                Col2Sheet.Columns[14].Width = 100;
                                                Col2Sheet.Columns[15].Width = 100;
                                                Col2Sheet.Columns[16].Width = 80;
                                                Col2Sheet.Columns[17].Width = 80;
                                                Col2Sheet.Columns[18].Width = 80;
                                                Col2Sheet.Columns[19].Width = 80;
                                                Col2Sheet.Columns[20].Width = 80;
                                                Col2Sheet.Columns[21].Width = 100;
                                                Col2Sheet.Columns[22].Width = 60;
                                                Col2Sheet.Columns[23].Width = 60;
                                                Col2Sheet.Columns[24].Width = 60;
                                                Col2Sheet.Columns[25].Width = 80;
                                                Col2Sheet.Columns[26].Width = 80;
                                                Col2Sheet.Columns[27].Width = 80;
                                                Col2Sheet.Columns[28].Width = 100;
                                                Col2Sheet.Columns[29].Width = 80;
                                                Col2Sheet.Columns[30].Width = 120;

                                                Col2Sheet.Rows[rowIndex][colIndex].Value = "Hit No";
                                                Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                                colIndex++;

                                                Col2Sheet.Rows[rowIndex][colIndex].Value = "Employee No";
                                                Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                colIndex++;

                                                Col2Sheet.Rows[rowIndex][colIndex].Value = "First Name";
                                                Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                colIndex++;

                                                Col2Sheet.Rows[rowIndex][colIndex].Value = "Last Name";
                                                Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                colIndex++;

                                                Col2Sheet.Rows[rowIndex][colIndex].Value = "Active";
                                                Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                                colIndex++;

                                                Col2Sheet.Rows[rowIndex][colIndex].Value = "Ethnicity";
                                                Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                colIndex++;

                                                Col2Sheet.Rows[rowIndex][colIndex].Value = "Race";
                                                Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                colIndex++;

                                                Col2Sheet.Rows[rowIndex][colIndex].Value = "Education Completed";
                                                Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                colIndex++;

                                                Col2Sheet.Rows[rowIndex][colIndex].Value = "Education Progress";
                                                Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                colIndex++;

                                                Col2Sheet.Rows[rowIndex][colIndex].Value = "Date Terminated";
                                                Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                colIndex++;

                                                Col2Sheet.Rows[rowIndex][colIndex].Value = "Reason For Termination";
                                                Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                colIndex++;

                                                Col2Sheet.Rows[rowIndex][colIndex].Value = "Primary Language";
                                                Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                colIndex++;

                                                Col2Sheet.Rows[rowIndex][colIndex].Value = "Language 1";
                                                Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                colIndex++;

                                                Col2Sheet.Rows[rowIndex][colIndex].Value = "Language 2";
                                                Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                colIndex++;

                                                Col2Sheet.Rows[rowIndex][colIndex].Value = "Language 3";
                                                Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                colIndex++;

                                                Col2Sheet.Rows[rowIndex][colIndex].Value = "Work For HS";
                                                Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                                colIndex++;

                                                Col2Sheet.Rows[rowIndex][colIndex].Value = "Work For EHS";
                                                Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                                colIndex++;

                                                Col2Sheet.Rows[rowIndex][colIndex].Value = "Work For Non HS";
                                                Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                                colIndex++;

                                                Col2Sheet.Rows[rowIndex][colIndex].Value = "Work For Volunteer";
                                                Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                                colIndex++;

                                                Col2Sheet.Rows[rowIndex][colIndex].Value = "Work For Contracted";
                                                Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                                colIndex++;

                                                Col2Sheet.Rows[rowIndex][colIndex].Value = "Positional Category";
                                                Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                colIndex++;

                                                Col2Sheet.Rows[rowIndex][colIndex].Value = "Hourly Rate";
                                                Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBRHC;
                                                colIndex++;

                                                Col2Sheet.Rows[rowIndex][colIndex].Value = "Std Hours";
                                                Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBRHC;
                                                colIndex++;

                                                Col2Sheet.Rows[rowIndex][colIndex].Value = "Salary";
                                                Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBRHC;
                                                colIndex++;

                                                Col2Sheet.Rows[rowIndex][colIndex].Value = "Weeks Worked";
                                                Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBRHC;
                                                colIndex++;

                                                Col2Sheet.Rows[rowIndex][colIndex].Value = "Head Start Parent";
                                                Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                                colIndex++;

                                                Col2Sheet.Rows[rowIndex][colIndex].Value = "Daycare Program Parent";
                                                Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                                colIndex++;

                                                Col2Sheet.Rows[rowIndex][colIndex].Value = "Early Headestart Parent";
                                                Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                                colIndex++;

                                                Col2Sheet.Rows[rowIndex][colIndex].Value = "Position Filled after 3 Months";
                                                Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                                colIndex++;

                                                Col2Sheet.Rows[rowIndex][colIndex].Value = "This Individual replaced another staff member";
                                                Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;

                                                foreach (DataRow dr in dtCol2Audit.Rows)
                                                {
                                                    rowIndex++;

                                                    colIndex = 1;
                                                    if (!string.IsNullOrEmpty(dr["HIT_NUM"].ToString().Trim()))
                                                        Col2Sheet.Rows[rowIndex][colIndex].Value = Convert.ToInt32(dr["HIT_NUM"].ToString().Trim());
                                                    Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNumb_NCC;
                                                    colIndex++;

                                                    Col2Sheet.Rows[rowIndex][colIndex].Value = dr["STF_CODE"].ToString().Trim() == "" ? "" : dr["STF_CODE"].ToString().Trim();
                                                    Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                    colIndex++;

                                                    Col2Sheet.Rows[rowIndex][colIndex].Value = dr["STF_NAME_FI"].ToString().Trim() == "" ? "" : dr["STF_NAME_FI"].ToString().Trim();
                                                    Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                    colIndex++;

                                                    Col2Sheet.Rows[rowIndex][colIndex].Value = dr["STF_NAME_LAST"].ToString().Trim() == "" ? "" : dr["STF_NAME_LAST"].ToString().Trim();
                                                    Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                    colIndex++;

                                                    Col2Sheet.Rows[rowIndex][colIndex].Value = dr["STF_ACTIVE"].ToString().Trim() == "" ? "" : dr["STF_ACTIVE"].ToString().Trim();
                                                    Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                    colIndex++;

                                                    Col2Sheet.Rows[rowIndex][colIndex].Value = dr["ETHNICITY"].ToString().Trim() == "" ? "" : dr["ETHNICITY"].ToString().Trim();
                                                    Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                    colIndex++;

                                                    Col2Sheet.Rows[rowIndex][colIndex].Value = dr["Race"].ToString().Trim() == "" ? "" : dr["Race"].ToString().Trim().Trim();
                                                    Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                    colIndex++;

                                                    Col2Sheet.Rows[rowIndex][colIndex].Value = (dr["Education"].ToString().Trim() == "" ? "" : dr["Education"].ToString().Trim());
                                                    Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                    colIndex++;

                                                    Col2Sheet.Rows[rowIndex][colIndex].Value = dr["EDUCATION_PROGRESS"].ToString().Trim() == "" ? "" : dr["EDUCATION_PROGRESS"].ToString().Trim().Trim();
                                                    Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                    colIndex++;

                                                    Col2Sheet.Rows[rowIndex][colIndex].Value = dr["STF_DATE_TERMINATED"].ToString().Trim() == "" ? "" : Convert.ToDateTime(dr["STF_DATE_TERMINATED"]).ToString("MM/dd/yyyy").ToString().Trim();
                                                    Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                    colIndex++;

                                                    Col2Sheet.Rows[rowIndex][colIndex].Value = dr["RES_TERMINATION"].ToString().Trim() == "" ? "" : dr["RES_TERMINATION"].ToString().Trim();
                                                    Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                    colIndex++;

                                                    Col2Sheet.Rows[rowIndex][colIndex].Value = dr["PRIM_LANGUAGE"].ToString().Trim() == "" ? "" : dr["PRIM_LANGUAGE"].ToString().Trim();
                                                    Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                    colIndex++;

                                                    Col2Sheet.Rows[rowIndex][colIndex].Value = dr["LANGUAGE1"].ToString().Trim() == "" ? "" : dr["LANGUAGE1"].ToString().Trim();
                                                    Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                    colIndex++;

                                                    Col2Sheet.Rows[rowIndex][colIndex].Value = dr["LANGUAGE2"].ToString().Trim() == "" ? "" : dr["LANGUAGE2"].ToString().Trim();
                                                    Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                    colIndex++;

                                                    Col2Sheet.Rows[rowIndex][colIndex].Value = dr["LANGUAGE3"].ToString().Trim() == "" ? "" : dr["LANGUAGE3"].ToString().Trim();
                                                    Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                    colIndex++;

                                                    Col2Sheet.Rows[rowIndex][colIndex].Value = dr["STF_WORKFOR_HS"].ToString().Trim() == "" ? "" : dr["STF_WORKFOR_HS"].ToString().Trim();
                                                    Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                    colIndex++;

                                                    Col2Sheet.Rows[rowIndex][colIndex].Value = dr["STF_WORKFOR_EHS"].ToString().Trim() == "" ? "" : dr["STF_WORKFOR_EHS"].ToString().Trim();
                                                    Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                    colIndex++;

                                                    Col2Sheet.Rows[rowIndex][colIndex].Value = dr["STF_WORKFOR_NONHS"].ToString().Trim() == "" ? "" : dr["STF_WORKFOR_NONHS"].ToString().Trim();
                                                    Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                    colIndex++;

                                                    Col2Sheet.Rows[rowIndex][colIndex].Value = dr["STF_WORKFOR_VOL"].ToString().Trim() == "" ? "" : dr["STF_WORKFOR_VOL"].ToString().Trim();
                                                    Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                    colIndex++;

                                                    Col2Sheet.Rows[rowIndex][colIndex].Value = dr["STF_WORKFOR_CONT"].ToString().Trim() == "" ? "" : dr["STF_WORKFOR_CONT"].ToString().Trim();
                                                    Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                    colIndex++;

                                                    Col2Sheet.Rows[rowIndex][colIndex].Value = dr["POS_CATG"].ToString().Trim() == "" ? "" : dr["POS_CATG"].ToString().Trim();
                                                    Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                    colIndex++;

                                                    Col2Sheet.Rows[rowIndex][colIndex].Value = dr["STF_BASE_RATE"].ToString().Trim() == "" ? 0 : Convert
                                                        .ToDecimal(dr["STF_BASE_RATE"].ToString().Trim());
                                                    Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlDeci_NRC;
                                                    colIndex++;

                                                    Col2Sheet.Rows[rowIndex][colIndex].Value = dr["STF_HRS_WORKED_PW"].ToString().Trim() == "" ? 0 : Convert
                                                        .ToDecimal(dr["STF_HRS_WORKED_PW"].ToString());
                                                    Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlDeci_NRC;
                                                    colIndex++;

                                                    Col2Sheet.Rows[rowIndex][colIndex].Value = dr["STF_SALARY"].ToString().Trim() == "" ? 0 : Convert
                                                        .ToDecimal(dr["STF_SALARY"].ToString().Trim());
                                                    Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlDeci_NRC;
                                                    colIndex++;

                                                    Col2Sheet.Rows[rowIndex][colIndex].Value = dr["STF_WEEKS_WORKED"].ToString().Trim() == "" ? 0 : Convert
                                                        .ToInt32(dr["STF_WEEKS_WORKED"].ToString().Trim());
                                                    Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNumb_NRC;
                                                    colIndex++;

                                                    Col2Sheet.Rows[rowIndex][colIndex].Value = dr["HS_PARENT"].ToString().Trim() == "" ? "" : dr["HS_PARENT"].ToString().Trim();
                                                    Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                    colIndex++;

                                                    Col2Sheet.Rows[rowIndex][colIndex].Value = dr["DAYCARE_PARENT"].ToString().Trim() == "" ? "" : dr["DAYCARE_PARENT"].ToString().Trim();
                                                    Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                    colIndex++;

                                                    Col2Sheet.Rows[rowIndex][colIndex].Value = dr["EHS_PARESNT"].ToString().Trim() == "" ? "" : dr["EHS_PARESNT"].ToString().Trim();
                                                    Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                    colIndex++;

                                                    Col2Sheet.Rows[rowIndex][colIndex].Value = dr["POS_FILLED"].ToString().Trim() == "" ? "" : dr["POS_FILLED"].ToString().Trim();
                                                    Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                    colIndex++;

                                                    Col2Sheet.Rows[rowIndex][colIndex].Value = dr["REPLACE_SM"].ToString().Trim() == "" ? "" : dr["REPLACE_SM"].ToString().Trim();
                                                    Col2Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else if (((Captain.Common.Utilities.ListItem)Cmb_Section.SelectedItem).Value.ToString() == "C")
                        {
                            DataTable dtSecC_Audit = new DataTable();
                            DataTable dtSecC_Counts = new DataTable();

                            dtSecC_Counts = ds.Tables[2];

                            if (dtSecC_Counts.Rows.Count > 0)
                            {
                                if (dtSecC_Counts.Rows[0]["ColHead1"].ToString() != "" && dtSecC_Counts.Rows[0]["ColHead1"].ToString() != "0.00")
                                {
                                    dtSecC_Audit = ds.Tables[1];

                                    if (dtSecC_Audit.Rows.Count > 0)
                                    {
                                        DevExpress.Spreadsheet.Worksheet sheetAudit = wb.Worksheets.Add("Audit");
                                        sheetAudit.ActiveView.ShowGridlines = false;
                                        rowIndex = 1;
                                        colIndex = 1;

                                        sheetAudit.Columns[1].Width = 100;
                                        sheetAudit.Columns[2].Width = 80;
                                        sheetAudit.Columns[3].Width = 100;
                                        sheetAudit.Columns[4].Width = 150;
                                        sheetAudit.Columns[5].Width = 80;
                                        sheetAudit.Columns[6].Width = 100;

                                        sheetAudit.Rows[rowIndex][colIndex].Value = "Family ID";
                                        sheetAudit.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                        colIndex++;

                                        sheetAudit.Rows[rowIndex][colIndex].Value = "Client ID";
                                        sheetAudit.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                        colIndex++;

                                        sheetAudit.Rows[rowIndex][colIndex].Value = "App#";
                                        sheetAudit.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                        colIndex++;

                                        sheetAudit.Rows[rowIndex][colIndex].Value = "Applicant Name";
                                        sheetAudit.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                        colIndex++;

                                        sheetAudit.Rows[rowIndex][colIndex].Value = "1st Attn";
                                        sheetAudit.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                        colIndex++;

                                        sheetAudit.Rows[rowIndex][colIndex].Value = "Site/Room";
                                        sheetAudit.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                        colIndex++;

                                        DataRow[] drColHeads = dtColHeads.Select("PIRQUEST_SECTION ='C' AND PIRQUEST_COLHEAD1= '" + dgvQuestions.SelectedRows[0].Cells["Column1"].Value.ToString() + "' AND PIRQUEST_COLHEAD2 = '" + dgvQuestions.SelectedRows[0].Cells["Column2"].Value.ToString() + "'");

                                        if (drColHeads.Length > 0)
                                        {
                                            col1_Desc = drColHeads[0]["COL1_DESC"].ToString();
                                            col2_Desc = drColHeads[0]["COL2_DESC"].ToString();
                                        }

                                        if (col1_Desc != "" && col2_Desc != "")
                                        {
                                            sheetAudit.Columns[7].Width = 30;
                                            sheetAudit.Columns[8].Width = 200;

                                            sheetAudit.Columns[9].Width = 30;
                                            sheetAudit.Columns[10].Width = 200;

                                            rowIndex = 0;
                                            colIndex = 0;

                                            sheetAudit.Rows[rowIndex][colIndex].Value = "";
                                            xlRowsMerge(sheetAudit, rowIndex, colIndex, 7);
                                            colIndex = colIndex + 7;

                                            sheetAudit.Rows[rowIndex][colIndex].Value = col1_Desc.Trim();
                                            sheetAudit.Rows[rowIndex][colIndex].Style = col1Desc_style;
                                            xlRowsMerge(sheetAudit, rowIndex, colIndex, 2);
                                            colIndex = colIndex + 2;

                                            sheetAudit.Rows[rowIndex][colIndex].Value = col2_Desc.Trim();
                                            sheetAudit.Rows[rowIndex][colIndex].Style = col2Desc_style;
                                            xlRowsMerge(sheetAudit, rowIndex, colIndex, 2);

                                            rowIndex = 1;
                                            colIndex = 7;

                                            sheetAudit.Rows[rowIndex][colIndex].Value = "Hit No";
                                            sheetAudit.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                            colIndex++;

                                            sheetAudit.Rows[rowIndex][colIndex].Value = "Counting Logic Results";
                                            sheetAudit.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                            colIndex++;

                                            sheetAudit.Rows[rowIndex][colIndex].Value = "Hit No";
                                            sheetAudit.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                            colIndex++;

                                            sheetAudit.Rows[rowIndex][colIndex].Value = "Counting Logic Results";
                                            sheetAudit.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;

                                            foreach (DataRow dr in dtSecC_Audit.Rows)
                                            {
                                                rowIndex++;
                                                colIndex = 1;

                                                sheetAudit.Rows[rowIndex][colIndex].Value = dr["App_Fam_ID"].ToString().Trim();
                                                sheetAudit.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                colIndex++;

                                                sheetAudit.Rows[rowIndex][colIndex].Value = dr["App_Client_ID"].ToString().Trim();
                                                sheetAudit.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                colIndex++;

                                                sheetAudit.Rows[rowIndex][colIndex].Value = dr["App_App"].ToString().Trim();
                                                sheetAudit.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                colIndex++;

                                                sheetAudit.Rows[rowIndex][colIndex].Value = dr["App_Namee"].ToString().Trim();
                                                sheetAudit.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                colIndex++;

                                                sheetAudit.Rows[rowIndex][colIndex].Value = Convert.ToDateTime(dr["App_Attn_Date"]).ToString("MM/dd/yyyy");
                                                sheetAudit.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                colIndex++;

                                                sheetAudit.Rows[rowIndex][colIndex].Value =
                                                (dr["App_Site"].ToString().Trim() == "" ? "" : dr["App_Site"].ToString().Trim()) + (dr["App_Room"].ToString().Trim() == "" ? "" : dr["App_Room"].ToString().Trim()) + (dr["App_AMPM"].ToString().Trim() == "" ? "" : dr["App_AMPM"].ToString().Trim());
                                                sheetAudit.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                colIndex++;

                                                if (!string.IsNullOrEmpty(dr["App_Hit_Num"].ToString().Trim()))
                                                {
                                                    sheetAudit.Rows[rowIndex][colIndex].Value = Convert.ToInt32(dr["App_Hit_Num"].ToString().Trim());
                                                }
                                                sheetAudit.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                colIndex++;

                                                sheetAudit.Rows[rowIndex][colIndex].Value = dr["App_Logic_Results"].ToString().Trim();
                                                sheetAudit.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                colIndex++;

                                                if (!string.IsNullOrEmpty(dr["App_Hit_Num2"].ToString().Trim()))
                                                {
                                                    sheetAudit.Rows[rowIndex][colIndex].Value = Convert.ToInt32(dr["App_Hit_Num2"].ToString().Trim());
                                                }
                                                sheetAudit.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                colIndex++;

                                                sheetAudit.Rows[rowIndex][colIndex].Value = dr["App_Logic_Results2"].ToString().Trim();
                                                sheetAudit.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                            }
                                        }
                                        else
                                        {
                                            sheetAudit.Columns[7].Width = 30;
                                            sheetAudit.Columns[8].Width = 200;

                                            rowIndex = 0;
                                            colIndex = 0;

                                            sheetAudit.Rows[rowIndex][colIndex].Value = "";
                                            xlRowsMerge(sheetAudit, rowIndex, colIndex, 7);
                                            colIndex = colIndex + 7;

                                            sheetAudit.Rows[rowIndex][colIndex].Value = col1_Desc.Trim();
                                            sheetAudit.Rows[rowIndex][colIndex].Style = col1Desc_style;
                                            xlRowsMerge(sheetAudit, rowIndex, colIndex, 2);

                                            rowIndex = 1;
                                            colIndex = 7;

                                            sheetAudit.Rows[rowIndex][colIndex].Value = "Hit No";
                                            sheetAudit.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                            colIndex++;

                                            sheetAudit.Rows[rowIndex][colIndex].Value = "Counting Logic Results";
                                            sheetAudit.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;

                                            foreach (DataRow dr in dtSecC_Audit.Rows)
                                            {
                                                rowIndex++;
                                                colIndex = 1;

                                                sheetAudit.Rows[rowIndex][colIndex].Value = dr["App_Fam_ID"].ToString().Trim();
                                                sheetAudit.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                colIndex++;

                                                sheetAudit.Rows[rowIndex][colIndex].Value = dr["App_Client_ID"].ToString().Trim();
                                                sheetAudit.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                colIndex++;

                                                sheetAudit.Rows[rowIndex][colIndex].Value = dr["App_App"].ToString().Trim();
                                                sheetAudit.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                colIndex++;

                                                sheetAudit.Rows[rowIndex][colIndex].Value = dr["App_Namee"].ToString().Trim();
                                                sheetAudit.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                colIndex++;

                                                sheetAudit.Rows[rowIndex][colIndex].Value = Convert.ToDateTime(dr["App_Attn_Date"]).ToString("MM/dd/yyyy");
                                                sheetAudit.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                colIndex++;

                                                sheetAudit.Rows[rowIndex][colIndex].Value = (dr["App_Site"].ToString().Trim() == "" ? "" : dr["App_Site"].ToString().Trim()) + (dr["App_Room"].ToString().Trim() == "" ? "" : dr["App_Room"].ToString().Trim()) + (dr["App_AMPM"].ToString().Trim() == "" ? "" : dr["App_AMPM"].ToString().Trim());
                                                sheetAudit.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                colIndex++;

                                                if (!string.IsNullOrEmpty(dr["App_Hit_Num"].ToString().Trim()))
                                                {
                                                    sheetAudit.Rows[rowIndex][colIndex].Value = Convert.ToInt32(dr["App_Hit_Num"].ToString().Trim());
                                                }
                                                sheetAudit.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                colIndex++;

                                                sheetAudit.Rows[rowIndex][colIndex].Value = dr["App_Logic_Results"].ToString().Trim();
                                                sheetAudit.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                colIndex++;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    AlertBox.Show("No Records found", MessageBoxIcon.Warning);
                                }
                            }
                        }
                        else
                        {

                        }
                    }

                    #endregion
                    wb.Sheets.ActiveSheet = wb.Worksheets[0];
                    wb.SaveDocument(PdfName, DevExpress.Spreadsheet.DocumentFormat.OpenXml);

                    try
                    {
                        string localFilePath = PdfName;

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
            catch (Exception ex)
            {
                string errormessage = ex.Message;
            }
            #endregion
        }
        string Audit_Ques_Desc = string.Empty; string Audit_HeadQues_desc = string.Empty;
        private void ParametersPage(DevExpress.Spreadsheet.Workbook _wb, DevExpress_Excel_Properties _styles, DevExpress.Spreadsheet.Style Title1_style, DevExpress.Spreadsheet.Style Title2_style, DataTable dtQues)
        {
            DevExpress.Spreadsheet.Worksheet paramSheet = _wb.Worksheets[0];

            DevExpress.Spreadsheet.Style xsubTitleintakeCellstyle = _styles.xfnCELL_STYLE(_wb, _styles.sxlbodyFont, 10, "#002060", true, "#F9F9F9", "center", 1, 1, 1, 1, 1, "Thin", "#F8F9D0", "");
            DevExpress.Spreadsheet.Style gxlGenerate_cr = _styles.xfnCELL_STYLE(_wb, _styles.sxlTitleFont, 11, "#0070C0", false, "#FFFFFF", "right", 0, 1, 1, 1, 1, "Thin", "#d3e6f5", "");
            DevExpress.Spreadsheet.Style gxlGenerate_lr = _styles.xfnCELL_STYLE(_wb, _styles.sxlTitleFont, 10, "#0070C0", false, "#FFFFFF", "left", 0, 1, 1, 1, 1, "Thin", "#d3e6f5", "");
            DevExpress.Spreadsheet.Style reportNameStyle = _styles.xfnCELL_STYLE(_wb, _styles.sxlTitleFont, 13, "#002060", true, "#FFFFFF", "center", 1, 1, 1, 1, 1, "Thin", "#FFFFFF", "");

            paramSheet.Name = "Params";
            paramSheet.ActiveView.ShowGridlines = false;

            paramSheet.Columns[4].Width = 200;
            paramSheet.Columns[5].Width = 300;
            paramSheet.Columns[6].Width = 200;
            paramSheet.Columns[7].Width = 500;
            paramSheet.Columns[8].Width = 200;
            paramSheet.Columns[9].Width = 200;
            paramSheet.Columns[10].Width = 200;
            paramSheet.Columns[11].Width = 200;
            paramSheet.Columns[12].Width = 600;

            string strAgy = Current_Hierarchy_DB.Split('-')[0];

            AgencyControlEntity BAgyControlDetails = _model.ZipCodeAndAgency.GetAgencyControlFile(strAgy);

            string ImgName = "";
            if (_baseform.BaseAgencyControlDetails.AgyShortName == "NEOCAA")
            {
                ImgName = "NEOCAA_" + strAgy + "_LOGO.png";
            }
            else
                ImgName = _baseform.BaseAgencyControlDetails.AgyShortName + "_00_LOGO.png";

            rowIndex = 1;
            colIndex = 4;
            paramSheet.Rows[rowIndex][colIndex].Value = BAgyControlDetails.AgyName;
            paramSheet.Rows[rowIndex][colIndex].Style = Title1_style;
            xlRowsMerge(paramSheet, rowIndex, colIndex, 9);
            rowIndex++;

            string imagesPath = "https://capsysdev.capsystems.com/images/PIPlogos/" + ImgName;
            DevExpress.Spreadsheet.SpreadsheetImageSource imgsrc = DevExpress.Spreadsheet.SpreadsheetImageSource.FromUri(imagesPath, _wb);
            DevExpress.Spreadsheet.Picture pic = paramSheet.Pictures.AddPicture(imgsrc, 800, 80, 630, 280);

            AgencyControlEntity _agycntrldets = new AgencyControlEntity();
            _agycntrldets = _baseform.BaseAgencyControlDetails;

            if (_baseform.BaseAgencyControlDetails.AgyShortName == "NEOCAA")
                _agycntrldets = BAgyControlDetails;
            else
                _agycntrldets = _baseform.BaseAgencyControlDetails;

            string street = _agycntrldets.Street == "" ? "" : (_agycntrldets.Street + ", ");
            string city = _agycntrldets.City == "" ? "" : (_agycntrldets.City + ", ");
            string state = _agycntrldets.State == "" ? "" : (_agycntrldets.State + ", ");
            string zip1 = _agycntrldets.Zip1 == "" ? "" : _agycntrldets.Zip1.PadLeft(5,'0');

            string strAddress = street + city + state + zip1;
            paramSheet.Rows[rowIndex][colIndex].Value = strAddress;
            paramSheet.Rows[rowIndex][colIndex].Style = _styles.gxlEMPTC;
            xlRowsMerge(paramSheet, rowIndex, colIndex, 9);

            rowIndex++;
            paramSheet.Rows[rowIndex][colIndex].Value = "";
            xlRowsMerge(paramSheet, rowIndex, colIndex, 9);

            rowIndex++;
            paramSheet.Rows[rowIndex][colIndex].Value = _priviliges.PrivilegeName;
            paramSheet.Rows[rowIndex][colIndex].Style = reportNameStyle;
            xlRowsMerge(paramSheet, rowIndex, colIndex, 9);

            rowIndex++;
            paramSheet.Rows[rowIndex][colIndex].Value = "Report Parameters";
            paramSheet.Rows[rowIndex][colIndex].Style = Title2_style;
            xlRowsMerge(paramSheet,rowIndex,colIndex, 9);

            string Agy = "All";
            string Dept = "All";
            string Prg = "All";
            string Header_year = string.Empty;
            if (Current_Hierarchy.Substring(0, 2) != "**")
                Agy = Current_Hierarchy.Substring(0, 2);
            if (Current_Hierarchy.Substring(2, 2) != "**")
                Dept = Current_Hierarchy.Substring(2, 2);
            if (Current_Hierarchy.Substring(4, 2) != "**")
                Prg = Current_Hierarchy.Substring(4, 2);
            if (CmbYear.Visible == true)
                Header_year = "Year: " + ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString();

            if (CmbYear.Visible == true)
            {
                rowIndex++;
                paramSheet.Rows[rowIndex][colIndex].Value = Txt_HieDesc.Text.Trim() + "   " + Header_year;
                paramSheet.Rows[rowIndex][colIndex].Style = xsubTitleintakeCellstyle;
                xlRowsMerge(paramSheet, rowIndex, colIndex, 9);
            }
            else
            {
                rowIndex++;
                paramSheet.Rows[rowIndex][colIndex].Value = Txt_HieDesc.Text.Trim();
                paramSheet.Rows[rowIndex][colIndex].Style = xsubTitleintakeCellstyle;
                xlRowsMerge(paramSheet, rowIndex, colIndex, 9);
            }


            rowIndex++;
            paramSheet.Rows[rowIndex][colIndex].Value = "";
            xlRowsMerge(paramSheet, rowIndex, colIndex, 9);

            rowIndex++;
            paramSheet.Rows[rowIndex][colIndex].Value = lblSection.Text.Trim();
            xlRowsMerge(paramSheet, rowIndex, colIndex, 2, _styles.gxlNLHC);
            colIndex = colIndex + 2;

            paramSheet.Rows[rowIndex][colIndex].Value = ((Captain.Common.Utilities.ListItem)Cmb_Section.SelectedItem).Text.ToString().Trim();
            xlRowsMerge(paramSheet, rowIndex, colIndex, 2, _styles.gxlNLC);
            colIndex = colIndex + 2;

            rowIndex++;
            colIndex = 4;
            paramSheet.Rows[rowIndex][colIndex].Value = lblFund.Text.Trim();
            xlRowsMerge(paramSheet, rowIndex, colIndex, 2, _styles.gxlNLHC);
            colIndex = colIndex + 2;

            paramSheet.Rows[rowIndex][colIndex].Value = ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString().Trim();
            xlRowsMerge(paramSheet, rowIndex, colIndex, 2, _styles.gxlNLC);
            colIndex = colIndex + 2;

            rowIndex++;
            colIndex = 4;
            paramSheet.Rows[rowIndex][colIndex].Value = lblSite.Text.Trim();
            xlRowsMerge(paramSheet, rowIndex, colIndex, 2, _styles.gxlNLHC);
            colIndex = colIndex + 2;

            paramSheet.Rows[rowIndex][colIndex].Value = ((Captain.Common.Utilities.ListItem)Cmb_Site.SelectedItem).Text.ToString().Trim();
            xlRowsMerge(paramSheet, rowIndex, colIndex, 2, _styles.gxlNLC);
            colIndex = colIndex + 2;

            if (chkbHitNoOnly.Checked)
            {
                rowIndex++;
                colIndex = 4;
                paramSheet.Rows[rowIndex][colIndex].Value = chkbHitNoOnly.Text.Trim();
                xlRowsMerge(paramSheet, rowIndex, colIndex, 2, _styles.gxlNLHC);
                colIndex = colIndex + 2;

                paramSheet.Rows[rowIndex][colIndex].Value = "Yes";
                xlRowsMerge(paramSheet, rowIndex, colIndex, 2, _styles.gxlNLC);
                colIndex = colIndex + 2;
            }

            rowIndex++;
            colIndex = 4;
            paramSheet.Rows[rowIndex][colIndex].Value = "";
            colIndex = colIndex + 9;

            string Audit_Ques_ID = string.Empty, Audit_Ques_Seq = string.Empty, Audit_Ques_Typr = null, Audit_Ques_Scode = null, Audit_Ques_code = null;
            ;
            Audit_Ques_Desc = string.Empty;
            Audit_HeadQues_desc = string.Empty;

            foreach (DataGridViewRow dr in dgvQuestions.Rows)
            {
                if (dr.Cells["gvSelect"].Value.ToString() == true.ToString())
                {
                    Audit_Ques_Desc = dr.Cells["gvQuesDesc"].Value.ToString();
                    Audit_Ques_ID = dr.Cells["gvQID"].Value.ToString();
                    Audit_Ques_code = dr.Cells["Column4"].Value.ToString();

                    foreach (DataGridViewRow drHead in dgvQuestions.Rows)
                    {
                        if (dr.Cells["Column5"].Value.ToString() == drHead.Cells["Column5"].Value.ToString() && drHead.Cells["Column0"].Value.ToString() == "000" && drHead.Cells["Column4"].Value.ToString() == Audit_Ques_code)
                        {
                            Audit_HeadQues_desc = drHead.Cells["gvQuesDesc"].Value.ToString();
                            break;
                        }
                    }

                    break;
                }
            }
            int Rect_Total_Rows = 3;
            string Tmp_Sel_TextQ = string.Empty;
            string Tmp_Sel_Text = string.Empty;
            string Text_to_Print = string.Empty;

            for (int i = 0; i < Rect_Total_Rows; i++)
            {
                switch (i)
                {
                    case 1:
                            Tmp_Sel_TextQ = Audit_HeadQues_desc;
                        break;
                    case 2:
                        if (dgvQuestions.SelectedRows[0].Cells["Column0"].Value.ToString() != "000")
                        {
                            Tmp_Sel_TextQ = "Question: " + Audit_Ques_Desc;
                        }
                        else
                        {
                            Tmp_Sel_TextQ = "";
                        }
                        break;
                    default:
                        Tmp_Sel_TextQ = "  ";
                        break;
                }
                Text_to_Print = Text_to_Print + "    " + Tmp_Sel_TextQ;
            }

            rowIndex++;
            colIndex = 4;
            paramSheet.Rows[rowIndex][colIndex].Value = Text_to_Print.ToString().Trim();
            paramSheet.Rows[rowIndex][colIndex].RowHeight = 130;
            paramSheet.Rows[rowIndex][colIndex].Alignment.Vertical = DevExpress.Spreadsheet.SpreadsheetVerticalAlignment.Top;
            xlRowsMerge(paramSheet, rowIndex, colIndex, 9, _styles.gxlNLC);
            colIndex = colIndex + 9;

            rowIndex = rowIndex + 3;
            colIndex = 10;//11;
            paramSheet.Rows[rowIndex][colIndex].Value = "Generated By: " + _baseform.UserID;
            paramSheet.Rows[rowIndex][colIndex].Style = gxlGenerate_lr;
            xlRowsMerge(paramSheet, rowIndex, colIndex, 3);
            rowIndex++;
            paramSheet.Rows[rowIndex][colIndex].Value = "Generated On: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");
            paramSheet.Rows[rowIndex][colIndex].Style = gxlGenerate_lr;
            xlRowsMerge(paramSheet, rowIndex, colIndex, 3);
        }

        private void xlRowsMerge(DevExpress.Spreadsheet.Worksheet worksheet, int Rowindex, int ColumnIndex, int numberOfColumns)
        {
            DevExpress.Spreadsheet.CellRange range = worksheet.Range.FromLTRB(ColumnIndex, Rowindex, ColumnIndex + numberOfColumns - 1, Rowindex);
            worksheet.MergeCells(range);
            //range.AutoFitRows();
        }
        private void xlRowsMerge(DevExpress.Spreadsheet.Worksheet worksheet, int Rowindex, int ColumnIndex, int numberOfColumns, DevExpress.Spreadsheet.Style _cellStyle)
        {
            DevExpress.Spreadsheet.CellRange range = worksheet.Range.FromLTRB(ColumnIndex, Rowindex, ColumnIndex + numberOfColumns - 1, Rowindex);
            worksheet.MergeCells(range);
            range.Style = _cellStyle;
            //range.AutoFitRows();
        }
        private void xlColsMerge(DevExpress.Spreadsheet.Worksheet worksheet, int startRowindex, int ColumnIndex, int endRowIndex)
        {
            DevExpress.Spreadsheet.CellRange mergeRange = worksheet.Range.FromLTRB(ColumnIndex, startRowindex, ColumnIndex, endRowIndex - 1);
            worksheet.MergeCells(mergeRange);
        }

        private void HSSB1026_PIRCounting_Form_ToolClick(object sender, ToolClickEventArgs e)
        {
            Application.Navigate(CommonFunctions.CreateZenHelps(_priviliges.Program, 0, _baseform.BusinessModuleID.ToString(), "", ""), target: "_blank");
        }

        public string Selected_Questions()
        {
            string selectedQuestions = string.Empty;
            if (_priviliges.ViewPriv.Equals("true"))
            {
                foreach (DataGridViewRow gvRows in dgvQuestions.Rows)
                {
                    if (gvRows.Cells["gvSelect"].Value != null && Convert.ToBoolean(gvRows.Cells["gvSelect"].Value) == true)
                    {
                        if (!selectedQuestions.Equals(string.Empty))
                        {
                            selectedQuestions +=  gvRows.Cells["gvQID"].Value + ",";
                        }
                        else
                        {
                            selectedQuestions +=  gvRows.Cells["gvQID"].Value + ",";
                        }
                    }
                }
            }
            selectedQuestions = selectedQuestions.TrimEnd(',');
            return selectedQuestions;
        }

        private void Genrate_Audit_Report_DevExp_New(DataSet dsAudit, DataSet dsConditions)
        {
            #region PDF Name

            string PdfName = "PIR Audit" + "_" + DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");

            PdfName = propReportPath + _baseform.UserID + "\\" + PdfName;
            try
            {
                if (!Directory.Exists(propReportPath + _baseform.UserID.Trim()))
                {
                    DirectoryInfo di = Directory.CreateDirectory(propReportPath + _baseform.UserID.Trim());
                }
            }
            catch (Exception ex)
            {
                AlertBox.Show("Error", MessageBoxIcon.Error);
            }

            try
            {
                string Tmpstr = PdfName + ".xlsx";
                if (File.Exists(Tmpstr))
                    File.Delete(Tmpstr);
            }
            catch (Exception ex)
            {
                int length = 8;
                string newFileName = System.Guid.NewGuid().ToString();
                newFileName = newFileName.Replace("-", string.Empty);

                Random_Filename = PdfName + newFileName.Substring(0, length) + ".xlsx";
            }

            if (!string.IsNullOrEmpty(Random_Filename))
                PdfName = Random_Filename;
            else
                PdfName += ".xlsx";

            #endregion

            #region Excel Generation

            try
            {
                using (DevExpress.Spreadsheet.Workbook wb = new DevExpress.Spreadsheet.Workbook())
                {
                    DevExpress_Excel_Properties styles = new DevExpress_Excel_Properties();
                    styles.sxlbook = wb;
                    styles.sxlTitleFont = "Trebuchet MS";
                    styles.sxlbodyFont = "calibri";

                    styles.getDevexpress_Excel_Properties();

                    #region Custom Styles

                    DevExpress.Spreadsheet.Style xTitleCellstyle = styles.xfnCELL_STYLE(wb, styles.sxlTitleFont, 20, "#002060", true, "#ffffff", "center", 1, 1, 1, 1, 1, "Thin", "#F8F9D0", "");
                    DevExpress.Spreadsheet.Style xTitleCellstyle2 = styles.xfnCELL_STYLE(wb, styles.sxlbodyFont, 14, "#002060", true, "#CFE6F9", "center", 1, 1, 1, 1, 1, "Thin", "#FFFFFF", "");

                    DevExpress.Spreadsheet.Style col1Desc_style = styles.xfnCELL_STYLE(wb, styles.sxlTitleFont, 8, "#000000", true, "#ECE8DD", "center", 1, 1, 1, 1, 1, "Thin", "#806000", "");

                    DevExpress.Spreadsheet.Style col2Desc_style = styles.xfnCELL_STYLE(wb, styles.sxlTitleFont, 8, "#000000", true, "#F8F4EA", "center", 1, 1, 1, 1, 1, "Thin", "#806000", "");

                    DevExpress.Spreadsheet.Style QuesHeader_background = styles.xfnCELL_STYLE(wb, "calibri", 10, "#000000", true, "#f0f5f5", "left", 1, 1, 1, 1, 1, "Thin", "#000000", "");//b7e099

                    DevExpress.Spreadsheet.Style ColHeaders_background = styles.xfnCELL_STYLE(wb, "calibri", 9, "#000000", true, "#fcfcf6", "left", 1, 1, 1, 1, 1, "Thin", "#000000", "");//F5F5DC fcfcf6 f9f9eb

                    #endregion

                    DataSet ds = dsAudit;
                    DataTable dtColHeads = new DataTable();
                    DataSet dsColHeads = Captain.DatabaseLayer.Lookups.BrowsePIRQues2(Program_Year);
                    dtColHeads = dsColHeads.Tables[0];

                    DataSet dsAssn2 = Captain.DatabaseLayer.ChldAttnDB.GetPirassnData2(Agency, Depart, Program, Program_Year);
                    DataTable dtAssn2 = dsAssn2.Tables[0];

                    DataSet dsCond = dsConditions;

                    #region Parameters Page

                    if (((Captain.Common.Utilities.ListItem)Cmb_Section.SelectedItem).Value.ToString() == "C" || ((Captain.Common.Utilities.ListItem)Cmb_Section.SelectedItem).Value.ToString() == "A")
                    {
                        DataTable dtSecC_Ques = new DataTable();
                        dtSecC_Ques = ds.Tables[0];

                        ParametersPage_New(wb, styles, xTitleCellstyle, xTitleCellstyle2, dtSecC_Ques, dtColHeads);
                    }
                    else if (((Captain.Common.Utilities.ListItem)Cmb_Section.SelectedItem).Value.ToString() == "B")
                    {
                        DataTable dtSecB_Ques = new DataTable();
                        dtSecB_Ques = ds.Tables[0];

                        ParametersPage_New(wb, styles, xTitleCellstyle, xTitleCellstyle2, dtSecB_Ques, dtColHeads);
                    }

                    #endregion

                    #region Excel Data

                    wb.Unit = DevExpress.Office.DocumentUnit.Point;
                    string col1_Desc = string.Empty;
                    string col2_Desc = string.Empty;

                    List<string> lstSheetNams = new List<string>();
                    if (ds.Tables.Count > 0)
                    {
                        if (((Captain.Common.Utilities.ListItem)Cmb_Section.SelectedItem).Value.ToString() == "B")
                        {
                            int tablecount = 0;

                            foreach (DataRow drLastTable in ds.Tables[ds.Tables.Count - 1].Rows)
                            {
                                for (int x = 1; x < 3; x++)
                                {
                                    if (drLastTable["ColHead" + x].ToString() != "" && drLastTable["ColHead" + x].ToString() != "0.00")
                                    {
                                        tablecount++;
                                        DataTable dtColAudit = ds.Tables[tablecount];

                                        if (dtColAudit.Rows.Count > 0)
                                        {
                                            string Tmp_Sel_TextQ = string.Empty;
                                            string Text_to_Print = string.Empty;
                                            string sheetName = string.Empty;

                                            if (drLastTable["SubQues"].ToString() == "000")
                                            {
                                                Text_to_Print = drLastTable["Ques_Desc"].ToString().Trim();
                                                sheetName = drLastTable["Ques_Desc"].ToString().Trim().Substring(0, 5);
                                            }
                                            else
                                            {
                                                DataRow[] dr = ds.Tables[ds.Tables.Count - 1].Select("SubQues = '000' AND Ques_Code = '" + drLastTable["Ques_Code"].ToString() + "'");

                                                if (dr.Length > 0)
                                                {
                                                    Text_to_Print = dr[0]["Ques_Desc"].ToString().Trim() + "   ---->   Question: " + drLastTable["Ques_Desc"].ToString().Trim();
                                                    sheetName = dr[0]["Ques_Desc"].ToString().Trim().Substring(0, 5) + " (" + drLastTable["Ques_Desc"].ToString().Trim().Substring(0, 5) + ")";
                                                }
                                                else
                                                {
                                                    DataRow[] drColHeads = dtColHeads.Select("PIRQUEST_SECTION ='B' AND PIRQUEST_SQUE_CODE = '000' AND PIRQUEST_QUE_CODE= '" + drLastTable["Ques_Code"].ToString() + "'");

                                                    if (drColHeads.Length > 0)
                                                        Text_to_Print = drColHeads[0]["PIRQUEST_QUE_DESC"].ToString() + "   ---->   Question: "+ drLastTable["Ques_Desc"].ToString().Trim();

                                                    sheetName = drColHeads[0]["PIRQUEST_QUE_DESC"].ToString().Substring(0, 5) +"_"+ drLastTable["Ques_Desc"].ToString().Trim().Substring(0, 5);
                                                    var match = lstSheetNams.FirstOrDefault(u => u.Equals(sheetName));
                                                    if (match != null)
                                                        sheetName = sheetName + (lstSheetNams.Count + 1);
                                                }
                                            }

                                            lstSheetNams.Add(sheetName);

                                            DevExpress.Spreadsheet.Worksheet Col1Sheet = wb.Worksheets.Add(sheetName + "_Header_" + x);

                                            Col1Sheet.ActiveView.ShowGridlines = false;

                                            //DevExpress.Spreadsheet.Style QuesHeader_background = styles.xfnCELL_STYLE(wb, "calibri", 10, "#000000", true, "#f0f5f5", "left", 1, 1, 1, 1, 1, "Thin", "#000000", "");//b7e099
                                            rowIndex = 0;
                                            colIndex = 1;
                                            Col1Sheet.Rows[rowIndex][colIndex].Value = Text_to_Print;
                                            Col1Sheet.Rows[rowIndex][colIndex].RowHeight = 80;
                                            xlRowsMerge(Col1Sheet, rowIndex, colIndex, 32, QuesHeader_background);

                                            //DevExpress.Spreadsheet.Style ColHeaders_background = styles.xfnCELL_STYLE(wb, "calibri", 9, "#000000", true, "#fcfcf6", "left", 1, 1, 1, 1, 1, "Thin", "#000000", "");//F5F5DC fcfcf6 f9f9eb
                                            rowIndex = 1;
                                            colIndex = 1;
                                            Col1Sheet.Rows[rowIndex][colIndex].Value = drLastTable["ColId" + x].ToString().Trim();
                                            xlRowsMerge(Col1Sheet, rowIndex, colIndex, 32, ColHeaders_background);

                                            rowIndex = 2;
                                            colIndex = 1;

                                            Col1Sheet.Columns[1].Width = 40;
                                            Col1Sheet.Columns[2].Width = 80;
                                            Col1Sheet.Columns[3].Width = 100;
                                            Col1Sheet.Columns[4].Width = 100;
                                            Col1Sheet.Columns[5].Width = 40;
                                            Col1Sheet.Columns[6].Width = 150;
                                            Col1Sheet.Columns[7].Width = 100;
                                            Col1Sheet.Columns[8].Width = 170;
                                            Col1Sheet.Columns[9].Width = 200;
                                            Col1Sheet.Columns[10].Width = 100;
                                            Col1Sheet.Columns[11].Width = 200;
                                            Col1Sheet.Columns[12].Width = 100;
                                            Col1Sheet.Columns[13].Width = 100;

                                            Col1Sheet.Columns[14].Width = 100;
                                            Col1Sheet.Columns[15].Width = 100;
                                            Col1Sheet.Columns[16].Width = 80;
                                            Col1Sheet.Columns[17].Width = 80;
                                            Col1Sheet.Columns[18].Width = 80;
                                            Col1Sheet.Columns[19].Width = 80;
                                            Col1Sheet.Columns[20].Width = 80;
                                            Col1Sheet.Columns[21].Width = 80;
                                            Col1Sheet.Columns[22].Width = 80;
                                            Col1Sheet.Columns[23].Width = 100;
                                            Col1Sheet.Columns[24].Width = 60;
                                            Col1Sheet.Columns[25].Width = 60;
                                            Col1Sheet.Columns[26].Width = 60;
                                            Col1Sheet.Columns[27].Width = 80;
                                            Col1Sheet.Columns[28].Width = 80;
                                            Col1Sheet.Columns[29].Width = 80;
                                            Col1Sheet.Columns[30].Width = 100;
                                            Col1Sheet.Columns[31].Width = 80;
                                            Col1Sheet.Columns[32].Width = 120;

                                            #region Column Headers
                                            Col1Sheet.Rows[rowIndex][colIndex].Value = "Hit No";
                                            Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                            colIndex++;

                                            Col1Sheet.Rows[rowIndex][colIndex].Value = "Employee No";
                                            Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                            colIndex++;

                                            Col1Sheet.Rows[rowIndex][colIndex].Value = "First Name";
                                            Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                            colIndex++;

                                            Col1Sheet.Rows[rowIndex][colIndex].Value = "Last Name";
                                            Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                            colIndex++;

                                            Col1Sheet.Rows[rowIndex][colIndex].Value = "Active";
                                            Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                            colIndex++;

                                            Col1Sheet.Rows[rowIndex][colIndex].Value = "Ethnicity";
                                            Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                            colIndex++;

                                            Col1Sheet.Rows[rowIndex][colIndex].Value = "Race";
                                            Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                            colIndex++;

                                            Col1Sheet.Rows[rowIndex][colIndex].Value = "Education Completed";
                                            Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                            colIndex++;

                                            Col1Sheet.Rows[rowIndex][colIndex].Value = "Education Progress";
                                            Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                            colIndex++;

                                            Col1Sheet.Rows[rowIndex][colIndex].Value = "Date Terminated";
                                            Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                            colIndex++;

                                            Col1Sheet.Rows[rowIndex][colIndex].Value = "Reason For Termination";
                                            Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                            colIndex++;

                                            Col1Sheet.Rows[rowIndex][colIndex].Value = "Primary Language";
                                            Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                            colIndex++;

                                            Col1Sheet.Rows[rowIndex][colIndex].Value = "Language 1";
                                            Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                            colIndex++;

                                            Col1Sheet.Rows[rowIndex][colIndex].Value = "Language 2";
                                            Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                            colIndex++;

                                            Col1Sheet.Rows[rowIndex][colIndex].Value = "Language 3";
                                            Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                            colIndex++;

                                            Col1Sheet.Rows[rowIndex][colIndex].Value = "Work For HS";
                                            Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                            colIndex++;

                                            Col1Sheet.Rows[rowIndex][colIndex].Value = "Work For HS 2";
                                            Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                            colIndex++;

                                            Col1Sheet.Rows[rowIndex][colIndex].Value = "Work For EHS";
                                            Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                            colIndex++;

                                            Col1Sheet.Rows[rowIndex][colIndex].Value = "Work For EHSCCP";
                                            Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                            colIndex++;

                                            Col1Sheet.Rows[rowIndex][colIndex].Value = "Work For Non HS";
                                            Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                            colIndex++;

                                            Col1Sheet.Rows[rowIndex][colIndex].Value = "Work For Volunteer";
                                            Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                            colIndex++;

                                            Col1Sheet.Rows[rowIndex][colIndex].Value = "Work For Contracted";
                                            Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                            colIndex++;

                                            Col1Sheet.Rows[rowIndex][colIndex].Value = "Positional Category";
                                            Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                            colIndex++;

                                            Col1Sheet.Rows[rowIndex][colIndex].Value = "Hourly Rate";
                                            Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBRHC;
                                            colIndex++;

                                            Col1Sheet.Rows[rowIndex][colIndex].Value = "Std Hours";
                                            Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBRHC;
                                            colIndex++;

                                            Col1Sheet.Rows[rowIndex][colIndex].Value = "Salary";
                                            Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBRHC;
                                            colIndex++;

                                            Col1Sheet.Rows[rowIndex][colIndex].Value = "Weeks Worked";
                                            Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBRHC;
                                            colIndex++;

                                            Col1Sheet.Rows[rowIndex][colIndex].Value = "Head Start Parent";
                                            Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                            colIndex++;

                                            Col1Sheet.Rows[rowIndex][colIndex].Value = "Daycare Program Parent";
                                            Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                            colIndex++;

                                            Col1Sheet.Rows[rowIndex][colIndex].Value = "Early Headestart Parent";
                                            Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                            colIndex++;

                                            Col1Sheet.Rows[rowIndex][colIndex].Value = "Position Filled after 3 Months";
                                            Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                            colIndex++;

                                            Col1Sheet.Rows[rowIndex][colIndex].Value = "This Individual replaced another staff member";
                                            Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                            Col1Sheet.FreezeRows(rowIndex);
                                            #endregion

                                            #region Counts Loop
                                            foreach (DataRow dr in dtColAudit.Rows)
                                            {
                                                rowIndex++;

                                                colIndex = 1;
                                                if (!string.IsNullOrEmpty(dr["HIT_NUM"].ToString().Trim()))
                                                {
                                                    Col1Sheet.Rows[rowIndex][colIndex].Value = Convert.ToInt32(dr["HIT_NUM"].ToString().Trim());
                                                }
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNumb_NCC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = dr["STF_CODE"].ToString().Trim() == "" ? "" : dr["STF_CODE"].ToString().Trim();
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = dr["STF_NAME_FI"].ToString().Trim() == "" ? "" : dr["STF_NAME_FI"].ToString().Trim();
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = dr["STF_NAME_LAST"].ToString().Trim() == "" ? "" : dr["STF_NAME_LAST"].ToString().Trim();
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = dr["STF_ACTIVE"].ToString().Trim() == "" ? "" : dr["STF_ACTIVE"].ToString().Trim();
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = dr["ETHNICITY"].ToString().Trim() == "" ? "" : dr["ETHNICITY"].ToString().Trim();
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = dr["Race"].ToString().Trim() == "" ? "" : dr["Race"].ToString().Trim().Trim();
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = (dr["Education"].ToString().Trim() == "" ? "" : dr["Education"].ToString().Trim());
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = dr["EDUCATION_PROGRESS"].ToString().Trim() == "" ? "" : dr["EDUCATION_PROGRESS"].ToString().Trim().Trim();
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = dr["STF_DATE_TERMINATED"].ToString().Trim() == "" ? "" : Convert.ToDateTime(dr["STF_DATE_TERMINATED"]).ToString("MM/dd/yyyy").ToString().Trim();
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = dr["RES_TERMINATION"].ToString().Trim() == "" ? "" : dr["RES_TERMINATION"].ToString().Trim();
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = dr["PRIM_LANGUAGE"].ToString().Trim() == "" ? "" : dr["PRIM_LANGUAGE"].ToString().Trim();
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = dr["LANGUAGE1"].ToString().Trim() == "" ? "" : dr["LANGUAGE1"].ToString().Trim();
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = dr["LANGUAGE2"].ToString().Trim() == "" ? "" : dr["LANGUAGE2"].ToString().Trim();
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = dr["LANGUAGE3"].ToString().Trim() == "" ? "" : dr["LANGUAGE3"].ToString().Trim();
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = dr["STF_WORKFOR_HS"].ToString().Trim() == "" ? "" : dr["STF_WORKFOR_HS"].ToString().Trim();
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = dr["STF_WORKFOR_HS2"].ToString().Trim() == "" ? "" : dr["STF_WORKFOR_HS2"].ToString().Trim();
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = dr["STF_WORKFOR_EHS"].ToString().Trim() == "" ? "" : dr["STF_WORKFOR_EHS"].ToString().Trim();
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = dr["STF_WORKFOR_EHSCCP"].ToString().Trim() == "" ? "" : dr["STF_WORKFOR_EHSCCP"].ToString().Trim();
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = dr["STF_WORKFOR_NONHS"].ToString().Trim() == "" ? "" : dr["STF_WORKFOR_NONHS"].ToString().Trim();
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = dr["STF_WORKFOR_VOL"].ToString().Trim() == "" ? "" : dr["STF_WORKFOR_VOL"].ToString().Trim();
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = dr["STF_WORKFOR_CONT"].ToString().Trim() == "" ? "" : dr["STF_WORKFOR_CONT"].ToString().Trim();
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = dr["POS_CATG"].ToString().Trim() == "" ? "" : dr["POS_CATG"].ToString().Trim();
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = dr["STF_BASE_RATE"].ToString().Trim() == "" ? 0 : Convert
                                                .ToDecimal(dr["STF_BASE_RATE"].ToString().Trim());
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlDeci_NRC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = dr["STF_HRS_WORKED_PW"].ToString().Trim() == "" ? 0 : Convert
                                                    .ToDecimal(dr["STF_HRS_WORKED_PW"].ToString());
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlDeci_NRC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = dr["STF_SALARY"].ToString().Trim() == "" ? 0 : Convert
                                                    .ToDecimal(dr["STF_SALARY"].ToString().Trim());
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlDeci_NRC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = dr["STF_WEEKS_WORKED"].ToString().Trim() == "" ? 0 : Convert
                                                    .ToInt32(dr["STF_WEEKS_WORKED"].ToString().Trim());
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNumb_NRC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = dr["HS_PARENT"].ToString().Trim() == "" ? "" : dr["HS_PARENT"].ToString().Trim();
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = dr["DAYCARE_PARENT"].ToString().Trim() == "" ? "" : dr["DAYCARE_PARENT"].ToString().Trim();
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = dr["EHS_PARESNT"].ToString().Trim() == "" ? "" : dr["EHS_PARESNT"].ToString().Trim();
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = dr["POS_FILLED"].ToString().Trim() == "" ? "" : dr["POS_FILLED"].ToString().Trim();
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                colIndex++;

                                                Col1Sheet.Rows[rowIndex][colIndex].Value = dr["REPLACE_SM"].ToString().Trim() == "" ? "" : dr["REPLACE_SM"].ToString().Trim();
                                                Col1Sheet.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                            }
                                            #endregion

                                            #region Counting Rules

                                            if (((Captain.Common.Utilities.ListItem)Cmb_Section.SelectedItem).Value.ToString() == "B")
                                            {
                                                string LogicDesc = string.Empty;

                                                LogicDesc = getRule(drLastTable["QuesID"].ToString(), ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Value.ToString(), x.ToString());
                                                if(!string.IsNullOrEmpty(LogicDesc.Trim()))
                                                {
                                                    rowIndex = rowIndex + 3;
                                                    colIndex = 1;

                                                    Col1Sheet.Rows[rowIndex][colIndex].Value = "Rules:   " + LogicDesc;
                                                    Col1Sheet.Rows[rowIndex][colIndex].RowHeight = 80;//30;
                                                    xlRowsMerge(Col1Sheet, rowIndex, colIndex, 32, ColHeaders_background);
                                                }

                                            }
                                            else
                                            {

                                                DataRow[] drColLogic = dtColHeads.Select("PIRQUEST_QID = '" + drLastTable["QuesID"] + "'");

                                                if (drColLogic.Length > 0)
                                                {
                                                    rowIndex = rowIndex + 3;
                                                    colIndex = 1;

                                                    Col1Sheet.Rows[rowIndex][colIndex].Value = "Rules:   " + drColLogic[0]["PIRQUEST_COL" + x + "_LOGIC"].ToString().Trim();
                                                    Col1Sheet.Rows[rowIndex][colIndex].RowHeight = 80;//30;
                                                    xlRowsMerge(Col1Sheet, rowIndex, colIndex, 32, ColHeaders_background);
                                                }
                                            }
                                            #endregion
                                        }
                                    }
                                }
                            }
                        }
                        else if (((Captain.Common.Utilities.ListItem)Cmb_Section.SelectedItem).Value.ToString() == "C"|| ((Captain.Common.Utilities.ListItem)Cmb_Section.SelectedItem).Value.ToString() == "A")
                        {
                            DataTable dtSecC_Counts = new DataTable();

                            if (ds.Tables.Count > 0)
                            {
                                dtSecC_Counts = ds.Tables[ds.Tables.Count - 1];

                                if (dtSecC_Counts.Rows.Count > 0)
                                {
                                    string prev_QID = string.Empty;

                                    foreach (DataRow dr in dtSecC_Counts.Rows)
                                    {
                                        DataRow[] drAssn = dtAssn2.Select("PIRASSN_Q_ID = '" + dr["QuesID"] + "'");

                                        string sheetName = string.Empty;
                                        string Text_to_Print = string.Empty;
                                        DataRow[] drColHeads;

                                        if (dr["SubQues"].ToString() == "000")
                                        {
                                            Text_to_Print = dr["Ques_Desc"].ToString().Trim();
                                            sheetName = dr["Ques_Desc"].ToString().Trim().Substring(0, 5);
                                        }
                                        else
                                        {
                                            DataRow[] dr1 = ds.Tables[ds.Tables.Count - 1].Select("SubQues = '000' AND Ques_Code = '" + dr["Ques_Code"].ToString() + "'");

                                            if (dr1.Length > 0)
                                            {
                                                Text_to_Print = dr1[0]["Ques_Desc"].ToString().Trim() + "   ---->   Question: " + dr["Ques_Desc"].ToString().Trim();
                                                sheetName = dr1[0]["Ques_Desc"].ToString().Trim().Substring(0, 5) + " (" + dr["Ques_Desc"].ToString().Trim().Substring(0, 5) + ")";
                                            }
                                            else
                                            {
                                                if (((Captain.Common.Utilities.ListItem)Cmb_Section.SelectedItem).Value.ToString() == "C")
                                                    drColHeads = dtColHeads.Select("PIRQUEST_SECTION ='C' AND PIRQUEST_SQUE_CODE = '000' AND PIRQUEST_QUE_CODE= '" + dr["Ques_Code"].ToString() + "'");
                                                else
                                                    drColHeads = dtColHeads.Select("PIRQUEST_SECTION ='A' AND PIRQUEST_SQUE_CODE = '000' AND PIRQUEST_QUE_CODE= '" + dr["Ques_Code"].ToString() + "'");

                                                if (drColHeads.Length > 0)
                                                    Text_to_Print = drColHeads[0]["PIRQUEST_QUE_DESC"].ToString() + "   ---->   Question: " + dr["Ques_Desc"].ToString().Trim();

                                                sheetName = drColHeads[0]["PIRQUEST_QUE_DESC"].ToString().Substring(0, 5) + "_" + dr["Ques_Desc"].ToString().Trim().Substring(0, 5);
                                                var match = lstSheetNams.FirstOrDefault(u => u.Equals(sheetName));
                                                if (match != null)
                                                    sheetName = sheetName + (lstSheetNams.Count + 1);
                                            }
                                        }

                                        lstSheetNams.Add(sheetName);

                                        if (dr["ColHead1"].ToString() != "" || dr["ColHead1"].ToString() != "0.00")
                                        {
                                            DataRow[] drAudit1 = ds.Tables[1].Select("App_Ques_ID = '" + dr["QuesID"].ToString() + "'");

                                            if (drAudit1.Length > 0)
                                            {
                                                DataTable dtSecC_Audit = drAudit1.CopyToDataTable();

                                                if (prev_QID != dr["QuesID"].ToString().Trim())
                                                {
                                                    DevExpress.Spreadsheet.Worksheet sheetAudit = wb.Worksheets.Add(sheetName);
                                                    sheetAudit.ActiveView.ShowGridlines = false;

                                                    rowIndex = 0; colIndex = 1;
                                                    ////DevExpress.Spreadsheet.Style QuesHeader_background = styles.xfnCELL_STYLE(wb, "calibri", 10, "#000000", true, "#f0f5f5", "left", 1, 1, 1, 1, 1, "Thin", "#000000", "");
                                                    rowIndex = 0;
                                                    colIndex = 1;
                                                    sheetAudit.Rows[rowIndex][colIndex].Value = Text_to_Print;
                                                    sheetAudit.Rows[rowIndex][colIndex].RowHeight = 30;
                                                    if (dr["ColId2"].ToString().Trim() != "")
                                                    {
                                                        xlRowsMerge(sheetAudit, rowIndex, colIndex, 10, QuesHeader_background);
                                                    }
                                                    else
                                                    {
                                                        xlRowsMerge(sheetAudit, rowIndex, colIndex, 8, QuesHeader_background);
                                                    }

                                                    rowIndex = 2;
                                                    colIndex = 1;

                                                    sheetAudit.Columns[1].Width = 100;
                                                    sheetAudit.Columns[2].Width = 80;
                                                    sheetAudit.Columns[3].Width = 100;
                                                    sheetAudit.Columns[4].Width = 150;
                                                    sheetAudit.Columns[5].Width = 80;
                                                    sheetAudit.Columns[6].Width = 100;

                                                    sheetAudit.Rows[rowIndex][colIndex].Value = "Family ID";
                                                    sheetAudit.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                    colIndex++;

                                                    sheetAudit.Rows[rowIndex][colIndex].Value = "Client ID";
                                                    sheetAudit.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                    colIndex++;

                                                    sheetAudit.Rows[rowIndex][colIndex].Value = "App#";
                                                    sheetAudit.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                    colIndex++;

                                                    sheetAudit.Rows[rowIndex][colIndex].Value = "Applicant Name";
                                                    sheetAudit.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                    colIndex++;

                                                    sheetAudit.Rows[rowIndex][colIndex].Value = "1st Attn";
                                                    sheetAudit.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                    colIndex++;

                                                    sheetAudit.Rows[rowIndex][colIndex].Value = "Site/Room";
                                                    sheetAudit.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                    colIndex++;
                                                    sheetAudit.FreezeRows(rowIndex);

                                                    col1_Desc = dr["ColId1"].ToString().ToString();
                                                    col2_Desc = dr["ColId2"].ToString().ToString();

                                                    if (col1_Desc != "" && col2_Desc != "")
                                                    {
                                                        sheetAudit.Columns[7].Width = 30;
                                                        sheetAudit.Columns[8].Width = 200;

                                                        sheetAudit.Columns[9].Width = 30;
                                                        sheetAudit.Columns[10].Width = 200;

                                                        rowIndex = 1;
                                                        colIndex = 1;

                                                        sheetAudit.Rows[rowIndex][colIndex].Value = "";
                                                        xlRowsMerge(sheetAudit, rowIndex, colIndex, 6);
                                                        colIndex = colIndex + 6;

                                                        sheetAudit.Rows[rowIndex][colIndex].Value = col1_Desc.Trim();
                                                        sheetAudit.Rows[rowIndex][colIndex].Style = col1Desc_style;
                                                        xlRowsMerge(sheetAudit, rowIndex, colIndex, 2);
                                                        colIndex = colIndex + 2;

                                                        sheetAudit.Rows[rowIndex][colIndex].Value = col2_Desc.Trim();
                                                        sheetAudit.Rows[rowIndex][colIndex].Style = col2Desc_style;
                                                        xlRowsMerge(sheetAudit, rowIndex, colIndex, 2);

                                                        rowIndex = 2;
                                                        colIndex = 7;

                                                        sheetAudit.Rows[rowIndex][colIndex].Value = "Hit No";
                                                        sheetAudit.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                                        colIndex++;

                                                        sheetAudit.Rows[rowIndex][colIndex].Value = "Counting Logic Results";
                                                        sheetAudit.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                        colIndex++;

                                                        sheetAudit.Rows[rowIndex][colIndex].Value = "Hit No";
                                                        sheetAudit.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                                        colIndex++;

                                                        sheetAudit.Rows[rowIndex][colIndex].Value = "Counting Logic Results";
                                                        sheetAudit.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                        sheetAudit.FreezeRows(rowIndex);

                                                        foreach (DataRow drAudit in dtSecC_Audit.Rows)
                                                        {
                                                            rowIndex++;
                                                            colIndex = 1;

                                                            sheetAudit.Rows[rowIndex][colIndex].Value = drAudit["App_Fam_ID"].ToString().Trim();
                                                            sheetAudit.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                            colIndex++;

                                                            sheetAudit.Rows[rowIndex][colIndex].Value = drAudit["App_Client_ID"].ToString().Trim();
                                                            sheetAudit.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                            colIndex++;

                                                            sheetAudit.Rows[rowIndex][colIndex].Value = drAudit["App_App"].ToString().Trim();
                                                            sheetAudit.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                            colIndex++;

                                                            sheetAudit.Rows[rowIndex][colIndex].Value = drAudit["App_Namee"].ToString().Trim();
                                                            sheetAudit.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                            colIndex++;

                                                            sheetAudit.Rows[rowIndex][colIndex].Value = Convert.ToDateTime(drAudit["App_Attn_Date"]).ToString("MM/dd/yyyy");
                                                            sheetAudit.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                            colIndex++;

                                                            sheetAudit.Rows[rowIndex][colIndex].Value =
                                                            (drAudit["App_Site"].ToString().Trim() == "" ? "" : drAudit["App_Site"].ToString().Trim()) + (drAudit["App_Room"].ToString().Trim() == "" ? "" : drAudit["App_Room"].ToString().Trim()) + (drAudit["App_AMPM"].ToString().Trim() == "" ? "" : drAudit["App_AMPM"].ToString().Trim());
                                                            sheetAudit.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                            colIndex++;

                                                            if (!string.IsNullOrEmpty(drAudit["App_Hit_Num"].ToString().Trim()))
                                                            {
                                                                sheetAudit.Rows[rowIndex][colIndex].Value = Convert.ToInt32(drAudit["App_Hit_Num"].ToString().Trim());
                                                            }
                                                            sheetAudit.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                            colIndex++;

                                                            sheetAudit.Rows[rowIndex][colIndex].Value = drAudit["App_Logic_Results"].ToString().Trim();
                                                            sheetAudit.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                            colIndex++;

                                                            if (!string.IsNullOrEmpty(drAudit["App_Hit_Num2"].ToString().Trim()))
                                                            {
                                                                sheetAudit.Rows[rowIndex][colIndex].Value = Convert.ToInt32(drAudit["App_Hit_Num2"].ToString().Trim());
                                                            }
                                                            sheetAudit.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                            colIndex++;

                                                            sheetAudit.Rows[rowIndex][colIndex].Value = drAudit["App_Logic_Results2"].ToString().Trim();
                                                            sheetAudit.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        sheetAudit.Columns[7].Width = 30;
                                                        sheetAudit.Columns[8].Width = 200;

                                                        rowIndex = 1;
                                                        colIndex = 1;

                                                        sheetAudit.Rows[rowIndex][colIndex].Value = "";
                                                        xlRowsMerge(sheetAudit, rowIndex, colIndex, 6);
                                                        colIndex = colIndex + 6;

                                                        sheetAudit.Rows[rowIndex][colIndex].Value = col1_Desc.Trim();
                                                        sheetAudit.Rows[rowIndex][colIndex].Style = col1Desc_style;
                                                        xlRowsMerge(sheetAudit, rowIndex, colIndex, 2);

                                                        rowIndex = 2;
                                                        colIndex = 7;

                                                        sheetAudit.Rows[rowIndex][colIndex].Value = "Hit No";
                                                        sheetAudit.Rows[rowIndex][colIndex].Style = styles.gxlBCHC;
                                                        colIndex++;

                                                        sheetAudit.Rows[rowIndex][colIndex].Value = "Counting Logic Results";
                                                        sheetAudit.Rows[rowIndex][colIndex].Style = styles.gxlBLHC;
                                                        sheetAudit.FreezeRows(rowIndex);

                                                        foreach (DataRow drAudit in dtSecC_Audit.Rows)
                                                        {
                                                            rowIndex++;
                                                            colIndex = 1;

                                                            sheetAudit.Rows[rowIndex][colIndex].Value = drAudit["App_Fam_ID"].ToString().Trim();
                                                            sheetAudit.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                            colIndex++;

                                                            sheetAudit.Rows[rowIndex][colIndex].Value = drAudit["App_Client_ID"].ToString().Trim();
                                                            sheetAudit.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                            colIndex++;

                                                            sheetAudit.Rows[rowIndex][colIndex].Value = drAudit["App_App"].ToString().Trim();
                                                            sheetAudit.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                            colIndex++;

                                                            sheetAudit.Rows[rowIndex][colIndex].Value = drAudit["App_Namee"].ToString().Trim();
                                                            sheetAudit.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                            colIndex++;

                                                            sheetAudit.Rows[rowIndex][colIndex].Value = Convert.ToDateTime(drAudit["App_Attn_Date"]).ToString("MM/dd/yyyy");
                                                            sheetAudit.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                            colIndex++;

                                                            sheetAudit.Rows[rowIndex][colIndex].Value = (drAudit["App_Site"].ToString().Trim() == "" ? "" : drAudit["App_Site"].ToString().Trim()) + (drAudit["App_Room"].ToString().Trim() == "" ? "" : drAudit["App_Room"].ToString().Trim()) + (drAudit["App_AMPM"].ToString().Trim() == "" ? "" : drAudit["App_AMPM"].ToString().Trim());
                                                            sheetAudit.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                            colIndex++;

                                                            if (!string.IsNullOrEmpty(drAudit["App_Hit_Num"].ToString().Trim()))
                                                            {
                                                                sheetAudit.Rows[rowIndex][colIndex].Value = Convert.ToInt32(drAudit["App_Hit_Num"].ToString().Trim());
                                                            }
                                                            sheetAudit.Rows[rowIndex][colIndex].Style = styles.gxlNCC;
                                                            colIndex++;

                                                            sheetAudit.Rows[rowIndex][colIndex].Value = drAudit["App_Logic_Results"].ToString().Trim();
                                                            sheetAudit.Rows[rowIndex][colIndex].Style = styles.gxlNLC;
                                                            colIndex++;
                                                        }
                                                    }

                                                    #region Associations
                                                    //DevExpress.Spreadsheet.Style ColHeaders_background = styles.xfnCELL_STYLE(wb, "calibri", 9, "#000000", true, "#fcfcf6", "left", 1, 1, 1, 1, 1, "Thin", "#000000", "");
                                                    if (dr["Ques_Type"].ToString() == "H")
                                                    {
                                                        if (dtColHeads.Rows.Count > 0)
                                                        {
                                                            DataRow[] drColLogic = dtColHeads.Select("PIRQUEST_QID = '" + dr["QuesID"] + "'");
                                                            if (drColLogic.Length > 0)
                                                            {
                                                                rowIndex = rowIndex + 3;
                                                                colIndex = 1;

                                                                if (!string.IsNullOrEmpty(drColLogic[0]["PIRQUEST_COL1_LOGIC"].ToString().Trim()))
                                                                {
                                                                    sheetAudit.Rows[rowIndex][colIndex].Value = "Rules:   " + drColLogic[0]["PIRQUEST_COL1_LOGIC"].ToString().Trim();
                                                                    sheetAudit.Rows[rowIndex][colIndex].RowHeight = 80;
                                                                    if (dr["ColId2"].ToString().Trim() != "")
                                                                    {
                                                                        xlRowsMerge(sheetAudit, rowIndex, colIndex, 10, ColHeaders_background);
                                                                    }
                                                                    else
                                                                    {
                                                                        xlRowsMerge(sheetAudit, rowIndex, colIndex, 8, ColHeaders_background);
                                                                    }
                                                                }
                                                                if (!string.IsNullOrEmpty(drColLogic[0]["PIRQUEST_COL2_LOGIC"].ToString().Trim()))
                                                                {
                                                                    rowIndex++;
                                                                    sheetAudit.Rows[rowIndex][colIndex].Value = "Rules:   " + drColLogic[0]["PIRQUEST_COL2_LOGIC"].ToString().Trim();
                                                                    sheetAudit.Rows[rowIndex][colIndex].RowHeight = 80;
                                                                    if (dr["ColId2"].ToString().Trim() != "")
                                                                    {
                                                                        xlRowsMerge(sheetAudit, rowIndex, colIndex, 10, ColHeaders_background);
                                                                    }
                                                                    else
                                                                    {
                                                                        xlRowsMerge(sheetAudit, rowIndex, colIndex, 8, ColHeaders_background);
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (dsCond != null && dsCond.Tables.Count > 0)
                                                        {
                                                            DataTable dtCond = dsCond.Tables[0];

                                                            DataRow[] drCond = dtCond.Select("Ques_ID = '" + dr["QuesID"] + "'");

                                                            if (drCond.Length > 0)
                                                            {
                                                                dtCond = new DataTable();
                                                                dtCond = drCond.CopyToDataTable();

                                                                colIndex = 1;
                                                                rowIndex = rowIndex + 3;

                                                                string Task_Desc = string.Empty, Task_Condition = string.Empty, Task_Conjunction = string.Empty;
                                                                string Tmp_Sel_TextQ = string.Empty;
                                                                string Tmp_Sel_Text = string.Empty;

                                                                for (int i = 0; i < dtCond.Rows.Count; i++)
                                                                {
                                                                    for (int j = 0; j < 2; j++)
                                                                    {
                                                                        rowIndex++;
                                                                        Task_Desc = Task_Condition = Task_Conjunction = Tmp_Sel_Text = string.Empty;
                                                                        if (i < (dtCond.Rows.Count))
                                                                        {
                                                                            Task_Desc = dtCond.Rows[i]["Task"].ToString();
                                                                            Task_Condition = dtCond.Rows[i]["Condition"].ToString();
                                                                            if (i < (dtCond.Rows.Count - 1))
                                                                                Task_Conjunction = dtCond.Rows[i]["Conj"].ToString();

                                                                            switch (j)
                                                                            {
                                                                                case 0:
                                                                                    Tmp_Sel_Text = "Task = " + Task_Condition;
                                                                                    break;
                                                                                case 1:
                                                                                    Tmp_Sel_Text = "          " + Task_Conjunction;
                                                                                    break;
                                                                                default:
                                                                                    Tmp_Sel_Text = "  ";
                                                                                    break;
                                                                            }
                                                                        }

                                                                        if (Tmp_Sel_Text.Contains("Task = "))
                                                                        {
                                                                            sheetAudit.Rows[rowIndex][colIndex].Value = Tmp_Sel_Text;
                                                                            sheetAudit.Rows[rowIndex][colIndex].RowHeight = 30;
                                                                            if (dr["ColId2"].ToString().Trim() != "")
                                                                            {
                                                                                xlRowsMerge(sheetAudit, rowIndex, colIndex, 10, QuesHeader_background);
                                                                            }
                                                                            else
                                                                            {
                                                                                xlRowsMerge(sheetAudit, rowIndex, colIndex, 8, QuesHeader_background);
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            sheetAudit.Rows[rowIndex][colIndex].Value = Tmp_Sel_Text;
                                                                            sheetAudit.Rows[rowIndex][colIndex].RowHeight = 15;
                                                                            if (dr["ColId2"].ToString().Trim() != "")
                                                                            {
                                                                                xlRowsMerge(sheetAudit, rowIndex, colIndex, 10, styles.gxlNLC);
                                                                            }
                                                                            else
                                                                            {
                                                                                xlRowsMerge(sheetAudit, rowIndex, colIndex, 8, styles.gxlNLC);
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }

                                                    #endregion
                                                }
                                            }
                                        }
                                        prev_QID = dr["QuesID"].ToString().Trim();
                                    }
                                }
                            }
                        }
                    }

                    #endregion

                    wb.Sheets.ActiveSheet = wb.Worksheets[0];
                    wb.SaveDocument(PdfName, DevExpress.Spreadsheet.DocumentFormat.OpenXml);

                    try
                    {
                        string localFilePath = PdfName;

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
                }
            }
            catch (Exception ex)
            {
                string errormessage = ex.Message;
            }
            #endregion
        }

        private void ParametersPage_New(DevExpress.Spreadsheet.Workbook _wb, DevExpress_Excel_Properties _styles, DevExpress.Spreadsheet.Style Title1_style, DevExpress.Spreadsheet.Style Title2_style, DataTable dtQues, DataTable dtcolHeads)
        {
            DevExpress.Spreadsheet.Worksheet paramSheet = _wb.Worksheets[0];

            DevExpress.Spreadsheet.Style xsubTitleintakeCellstyle = _styles.xfnCELL_STYLE(_wb, _styles.sxlbodyFont, 10, "#002060", true, "#F9F9F9", "center", 1, 1, 1, 1, 1, "Thin", "#F8F9D0", "");
            DevExpress.Spreadsheet.Style gxlGenerate_cr = _styles.xfnCELL_STYLE(_wb, _styles.sxlTitleFont, 11, "#0070C0", false, "#FFFFFF", "right", 0, 1, 1, 1, 1, "Thin", "#d3e6f5", "");
            DevExpress.Spreadsheet.Style gxlGenerate_lr = _styles.xfnCELL_STYLE(_wb, _styles.sxlTitleFont, 10, "#0070C0", false, "#FFFFFF", "left", 0, 1, 1, 1, 1, "Thin", "#d3e6f5", "");
            DevExpress.Spreadsheet.Style reportNameStyle = _styles.xfnCELL_STYLE(_wb, _styles.sxlTitleFont, 13, "#002060", true, "#FFFFFF", "center", 1, 1, 1, 1, 1, "Thin", "#FFFFFF", "");

            DataTable dtQuesHeaders = dtQues;
            DataTable dtColHeaders = dtcolHeads;

            paramSheet.Name = "Params";
            paramSheet.ActiveView.ShowGridlines = false;
            _wb.Unit = DevExpress.Office.DocumentUnit.Point;
            //paramSheet.Columns[1].Width = 200;
            //paramSheet.Columns[2].Width = 300;
            //paramSheet.Columns[3].Width = 200;
            //paramSheet.Columns[4].Width = 500;
            //paramSheet.Columns[5].Width = 200;
            //paramSheet.Columns[6].Width = 200;
            //paramSheet.Columns[7].Width = 200;
            //paramSheet.Columns[8].Width = 200;
            //paramSheet.Columns[9].Width = 600;

            paramSheet.Columns[1].Width = 80;
            paramSheet.Columns[2].Width = 200;
            paramSheet.Columns[3].Width = 50;
            paramSheet.Columns[4].Width = 80;
            paramSheet.Columns[5].Width = 200;

            //_wb.Unit = DevExpress.Office.DocumentUnit.Point;
            string strAgy = Current_Hierarchy_DB.Split('-')[0];

            AgencyControlEntity BAgyControlDetails = _model.ZipCodeAndAgency.GetAgencyControlFile(strAgy);

            string ImgName = "";
            if (_baseform.BaseAgencyControlDetails.AgyShortName == "NEOCAA")
            {
                ImgName = "NEOCAA_" + strAgy + "_LOGO.png";
            }
            else
                ImgName = _baseform.BaseAgencyControlDetails.AgyShortName + "_00_LOGO.png";

            rowIndex = 1;
            colIndex = 1;
            paramSheet.Rows[rowIndex][colIndex].Value = BAgyControlDetails.AgyName;
            paramSheet.Rows[rowIndex][colIndex].Style = Title1_style;
            //xlRowsMerge(paramSheet, rowIndex, colIndex, 9);
            xlRowsMerge(paramSheet, rowIndex, colIndex, 5);
            rowIndex++;

            string imagesPath = "https://capsysdev.capsystems.com/images/PIPlogos/" + ImgName;
            DevExpress.Spreadsheet.SpreadsheetImageSource imgsrc = DevExpress.Spreadsheet.SpreadsheetImageSource.FromUri(imagesPath, _wb);
            //DevExpress.Spreadsheet.Picture pic = paramSheet.Pictures.AddPicture(imgsrc, 200, 80, 630, 280);
            DevExpress.Spreadsheet.Picture pic = paramSheet.Pictures.AddPicture(imgsrc, 50, 10, 120, 80);

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
            paramSheet.Rows[rowIndex][colIndex].Value = strAddress;
            paramSheet.Rows[rowIndex][colIndex].Style = _styles.gxlEMPTC;
            xlRowsMerge(paramSheet, rowIndex, colIndex, 5);

            rowIndex++;
            paramSheet.Rows[rowIndex][colIndex].Value = "";
            xlRowsMerge(paramSheet, rowIndex, colIndex, 5);

            rowIndex++;
            paramSheet.Rows[rowIndex][colIndex].Value = _priviliges.PrivilegeName;
            paramSheet.Rows[rowIndex][colIndex].Style = reportNameStyle;
            xlRowsMerge(paramSheet, rowIndex, colIndex, 5);

            rowIndex++;
            paramSheet.Rows[rowIndex][colIndex].Value = "Report Parameters";
            paramSheet.Rows[rowIndex][colIndex].Style = Title2_style;
            xlRowsMerge(paramSheet, rowIndex, colIndex, 5);

            string Agy = "All";
            string Dept = "All";
            string Prg = "All";
            string Header_year = string.Empty;
            if (Current_Hierarchy.Substring(0, 2) != "**")
                Agy = Current_Hierarchy.Substring(0, 2);
            if (Current_Hierarchy.Substring(2, 2) != "**")
                Dept = Current_Hierarchy.Substring(2, 2);
            if (Current_Hierarchy.Substring(4, 2) != "**")
                Prg = Current_Hierarchy.Substring(4, 2);
            if (CmbYear.Visible == true)
                Header_year = "Year: " + ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString();

            if (CmbYear.Visible == true)
            {
                rowIndex++;
                paramSheet.Rows[rowIndex][colIndex].Value = Txt_HieDesc.Text.Trim() + "   " + Header_year;
                paramSheet.Rows[rowIndex][colIndex].Style = xsubTitleintakeCellstyle;
                xlRowsMerge(paramSheet, rowIndex, colIndex, 5);
            }
            else
            {
                rowIndex++;
                paramSheet.Rows[rowIndex][colIndex].Value = Txt_HieDesc.Text.Trim();
                paramSheet.Rows[rowIndex][colIndex].Style = xsubTitleintakeCellstyle;
                xlRowsMerge(paramSheet, rowIndex, colIndex, 5);
            }


            rowIndex++;
            paramSheet.Rows[rowIndex][colIndex].Value = "";
            xlRowsMerge(paramSheet, rowIndex, colIndex, 5);

            rowIndex++;
            paramSheet.Rows[rowIndex][colIndex].Value = lblSection.Text.Trim();
            paramSheet.Rows[rowIndex][colIndex].Style = _styles.gxlNLHC;
            //xlRowsMerge(paramSheet, rowIndex, colIndex, 2, _styles.gxlNLHC);
            colIndex++;// = colIndex + 2;

            paramSheet.Rows[rowIndex][colIndex].Value = ((Captain.Common.Utilities.ListItem)Cmb_Section.SelectedItem).Text.ToString().Trim();
            paramSheet.Rows[rowIndex][colIndex].Style = _styles.gxlNLC;
            //xlRowsMerge(paramSheet, rowIndex, colIndex, 2, _styles.gxlNLC);
            colIndex++;// = colIndex + 2;

            rowIndex++;
            colIndex = 1;
            paramSheet.Rows[rowIndex][colIndex].Value = lblFund.Text.Trim();
            paramSheet.Rows[rowIndex][colIndex].Style = _styles.gxlNLHC;
            //xlRowsMerge(paramSheet, rowIndex, colIndex, 2, _styles.gxlNLHC);
            colIndex++;// = colIndex + 2;

            paramSheet.Rows[rowIndex][colIndex].Value = ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString().Trim();
            paramSheet.Rows[rowIndex][colIndex].Style = _styles.gxlNLC;
            //xlRowsMerge(paramSheet, rowIndex, colIndex, 2, _styles.gxlNLC);
            colIndex++;// = colIndex + 2;

            rowIndex++;
            colIndex = 1;
            paramSheet.Rows[rowIndex][colIndex].Value = lblSite.Text.Trim();
            paramSheet.Rows[rowIndex][colIndex].Style = _styles.gxlNLHC;
            //xlRowsMerge(paramSheet, rowIndex, colIndex, 2, _styles.gxlNLHC);
            colIndex++;// colIndex + 2;

            paramSheet.Rows[rowIndex][colIndex].Value = ((Captain.Common.Utilities.ListItem)Cmb_Site.SelectedItem).Text.ToString().Trim();
            paramSheet.Rows[rowIndex][colIndex].Style = _styles.gxlNLC;
            //xlRowsMerge(paramSheet, rowIndex, colIndex, 2, _styles.gxlNLC);
            colIndex++;// colIndex + 2;

            if (Hit_On_Records) //(chkbHitNoOnly.Checked)
            {
                rowIndex++;
                colIndex = 1;
                paramSheet.Rows[rowIndex][colIndex].Value = "Hits On Records Only";
                paramSheet.Rows[rowIndex][colIndex].Style = _styles.gxlNLHC;
                //xlRowsMerge(paramSheet, rowIndex, colIndex, 2, _styles.gxlNLHC);
                colIndex++;// colIndex + 2;

                paramSheet.Rows[rowIndex][colIndex].Value = "Yes";
                paramSheet.Rows[rowIndex][colIndex].Style = _styles.gxlNLC;
                //xlRowsMerge(paramSheet, rowIndex, colIndex, 2, _styles.gxlNLC);
                colIndex++;// colIndex + 2;
            }

            rowIndex++;
            colIndex = 1;
            paramSheet.Rows[rowIndex][colIndex].Value = "";
            colIndex = colIndex + 5;

            string Text_to_Print = string.Empty;

            DataView dvQuesHeaders = new DataView(dtQuesHeaders);
            dvQuesHeaders.Sort = "Ques_ID";
            dtQuesHeaders = dvQuesHeaders.ToTable();

            foreach (DataRow dr in dtQuesHeaders.Rows)
            {
                if (dr["SQUES_Cd"].ToString().Trim() != "000")
                {
                    DataRow[] drQues = dtColHeaders.Select("PIRQUEST_SQUE_CODE = '000' AND PIRQUEST_QUE_CODE = '"+ dr["Ques_Code"] +"'");

                    if (drQues.Length > 0)
                    {
                        Text_to_Print = drQues[0]["PIRQUEST_QUE_DESC"].ToString().Trim() + "  ---->  Question: " + dr["Ques_Desc"].ToString().Trim();
                    }
                }
                else
                {
                    Text_to_Print = dr["Ques_Desc"].ToString().Trim();
                }

                rowIndex++;
                colIndex = 1;
                paramSheet.Rows[rowIndex][colIndex].Value = Text_to_Print;
                if (((Captain.Common.Utilities.ListItem)Cmb_Section.SelectedItem).Value.ToString() == "B")
                    paramSheet.Rows[rowIndex][colIndex].RowHeight = 80;//330;//80;
                else
                    paramSheet.Rows[rowIndex][colIndex].RowHeight = 35;//130;
                paramSheet.Rows[rowIndex][colIndex].Alignment.Vertical = DevExpress.Spreadsheet.SpreadsheetVerticalAlignment.Top;
                xlRowsMerge(paramSheet, rowIndex, colIndex, 5, _styles.gxlNLC);
                colIndex = colIndex + 5;
            }

            rowIndex = rowIndex + 3;
            colIndex = 5;//8;
            paramSheet.Rows[rowIndex][colIndex].Value = "Generated By: " + _baseform.UserID;
            paramSheet.Rows[rowIndex][colIndex].Style = gxlGenerate_lr;
            //xlRowsMerge(paramSheet, rowIndex, colIndex, 2);
            rowIndex++;
            paramSheet.Rows[rowIndex][colIndex].Value = "Generated On: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");
            paramSheet.Rows[rowIndex][colIndex].Style = gxlGenerate_lr;
            //xlRowsMerge(paramSheet, rowIndex, colIndex, 2);
        }

        List<Captain.Common.Utilities.ListItem> GetAssociations()
        {

            List<Captain.Common.Utilities.ListItem> listpipimageTypes = new List<Captain.Common.Utilities.ListItem>();
            try
            {
                string strselectImgAgency = "00";

                SqlConnection con = new SqlConnection(_baseform.BaseLeanDataBaseConnectionString);

                con.Open();

                using (SqlCommand cmd = new SqlCommand("SELECT * FROM PIRASSN2 WHERE PIPIMGT_AGENCY ='" + _baseform.BaseAgencyControlDetails.AgyShortName + "' AND PIPIMGT_AGY ='" + strselectImgAgency + "'", con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drrow in ds.Tables[0].Rows)
                        {
                            listpipimageTypes.Add(new Captain.Common.Utilities.ListItem(drrow["PIPIMGT_ID"].ToString(), drrow["PIPIMGT_IMG_TYPE"].ToString(), drrow["PIPIMGT_DESCRIPTION"].ToString(), drrow["PIPIMGT_AMH"].ToString(), drrow["PIPIMGT_IncTypes"].ToString(), drrow["PIPIMGT_AGENCY"].ToString(), drrow["PIPIMGT_REQUIRED"].ToString(), string.Empty));
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

        #endregion

        public string getRule(string QID,string Fund,string Col)
        {
            string RuleDesc = string.Empty;

            switch(QID)
            {
                case "1":   
                    if(Col=="1")
                    {
                        if(Fund=="9")
                        {
                            RuleDesc = "If ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is checked)  AND (Work For Contracted checkbox is Unchecked) AND (Work For Volunteer checkbox is Unchecked)";//AND (Positional Category = “Classroom Teacher”  OR “Assistant Teacher”  OR “Home Visitor” OR “Family Child Care Provider” OR “Child Development Specialist”)
                        }
                        else
                        {
                            RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Work For Contracted checkbox is Unchecked) AND (Work For Volunteer checkbox is Unchecked)";// AND (Positional Category = “Classroom Teacher”  OR “Assistant Teacher”  OR “Home Visitor” OR “Family Child Care Provider” OR “Child Development Specialist”)";
                        }

                    }
                    else if(Col=="2") 
                    {
                        RuleDesc = "IF (Work For Contracted checkbox is checked) AND (Active checkbox is checked) AND (Work For Volunteer checkbox is Unchecked)";// AND (Positional Category = “Classroom Teacher”  OR “Assistant Teacher”  OR “Home Visitor” OR “Family Child Care Provider” OR “Child Development Specialist”)";
                    }
                    break;
                case "2":
                    if (Col == "1")
                    {
                        if (Fund == "9")
                        {
                            RuleDesc = "If ((Work For HS checkbox is checked) OR (Work For checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is checked) AND (Work For Contracted checkbox is Unchecked) AND (Work For Volunteer checkbox is Unchecked) AND (Head Start Parent checkbox OR Early Head Start Parent checkbox is checked)";// AND (Positional Category = “Classroom Teacher”  OR “Assistant Teacher”  OR “Home Visitor” OR “Family Child Care Provider”)";
                        }
                        else
                        {
                            RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Work For Contracted checkbox is Unchecked) AND (Work For Volunteer checkbox is Unchecked) AND (Head Start Parent checkbox OR Early Head Start Parent checkbox is checked)";// AND (Positional Category = “Classroom Teacher”  OR “Assistant Teacher”  OR “Home Visitor” OR “Family Child Care Provider”)";
                        }

                    }
                    else if (Col == "2")
                    {
                        RuleDesc = "IF (Work For Contracted checkbox is checked) AND (Active checkbox is checked) AND (Head Start Parent checkbox OR Early Head Start Parent checkbox is checked) AND (Work For Volunteer checkbox is Unchecked)";// AND (Positional Category = “Classroom Teacher”  OR “Assistant Teacher”  OR “Home Visitor” OR “Family Child Care Provider”)";
                    }
                    break;
                case "3":
                    RuleDesc = "IF (Work For Volunteer checkbox is checked) AND (Active checkbox is checked)";
                    break;
                case "4":
                    RuleDesc = "IF (Work For Volunteer checkbox is checked) AND (Active checkbox is checked)";
                    break;
                case "5":
                    if (Col == "1")
                    {
                        if (Fund == "9")
                        {
                            RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked))  AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher”)";
                        }
                        else
                        {
                            RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher”)";
                        }

                    }
                    else if (Col == "2")
                    {
                        if (Fund == "9")
                        {
                            RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked))  AND (Active checkbox is checked) AND (Positional Category = “Assistant Teacher”)";
                        }
                        else
                        {
                            RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Assistant Teacher”)";
                        }
                    }
                    break;
                case "7":
                    if (Col == "1")
                    {
                        if (Fund == "9")
                        {
                            RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked))  AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher”) AND (Education Completed = “ECE - Graduate Degree” OR “ECE - Master’s Degree”)";
                        }
                        else
                        {
                            RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher”) AND (Education Completed = “ECE - Graduate Degree” OR “ECE - Master’s Degree”)";
                        }

                    }
                    else if (Col == "2")
                    {
                        if (Fund == "9")
                        {
                            RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked))  AND (Active checkbox is checked) AND (Positional Category = “Assistant Teacher”) AND (Education Completed = “ECE - Graduate Degree” OR “ECE - Master’s Degree”)";
                        }
                        else
                        {
                            RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Assistant Teacher”) AND (Education Completed = “ECE - Graduate Degree” OR “ECE - Master’s Degree”)";
                        }
                    }
                    break;
                case "10":
                    if (Col == "1")
                    {
                        if (Fund == "9")
                        {
                            RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked)) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher”) AND (Education Completed = “ECE - Baccalaureate Degree”)";
                        }
                        else
                        {
                            RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher”) AND (Education Completed = “ECE - Baccalaureate Degree”)";
                        }

                    }
                    else if (Col == "2")
                    {
                        if (Fund == "9")
                        {
                            RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked))  AND (Active checkbox is checked) AND (Positional Category = “Assistant Teacher”) AND (Education Completed = “ECE - Baccalaureate Degree”)";
                        }
                        else
                        {
                            RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Assistant Teacher”) AND (Education Completed = “ECE - Baccalaureate Degree”)";
                        }
                    }
                    break;
                case "14":
                    if (Col == "1")
                    {
                        if (Fund == "9")
                        {
                            RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked)) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher”) AND (Education Completed = “ECE - Associate Degree”)";
                        }
                        else
                        {
                            RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher”) AND (Education Completed = “ECE - Associate Degree”)";
                        }

                    }
                    else if (Col == "2")
                    {
                        if (Fund == "9")
                        {
                            RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked))  AND (Active checkbox is checked) AND (Positional Category = “Assistant Teacher”) AND (Education Completed = “ECE - Associate Degree”)";
                        }
                        else
                        {
                            RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Assistant Teacher”) AND (Education Completed = “ECE - Associate Degree”)";
                        }
                    }
                    break;
                case "17":
                    if (Col == "1")
                    {
                        if (Fund == "9")
                        {
                            RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked)) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher”) AND (Education Completed = “CDA Child Development Associate”)";
                        }
                        else
                        {
                            RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher”) AND (Education Completed = “CDA Child Development Associate”)";
                        }

                    }
                    else if (Col == "2")
                    {
                        if (Fund == "9")
                        {
                            RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked))  AND (Active checkbox is checked) AND (Positional Category = “Assistant Teacher”) AND (Education Completed = “CDA Child Development Associate”)";
                        }
                        else
                        {
                            RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Assistant Teacher”) AND (Education Completed = “CDA Child Development Associate”)";
                        }
                    }
                    break;
                case "19":
                    if (Col == "1")
                    {
                        if (Fund == "9")
                        {
                            RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked)) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher”) AND (Education Completed NOT = “ECE - Graduate Degree” AND “ECE - Master’s Degree” AND “ECE - Baccalaureate Degree” AND “ECE - Associate Degree” AND “CDA Child Development Associate”)";
                        }
                        else
                        {
                            RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher”) AND (Education Completed NOT = “ECE - Graduate Degree” AND “ECE - Master’s Degree” AND “ECE - Baccalaureate Degree” AND “ECE - Associate Degree” AND “CDA Child Development Associate”)";
                        }

                    }
                    else if (Col == "2")
                    {
                        if (Fund == "9")
                        {
                            RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked))  AND (Active checkbox is checked) AND (Positional Category = “Assistant Teacher”) AND (Education Completed NOT = “ECE - Graduate Degree” AND “ECE - Master’s Degree” AND “ECE - Baccalaureate Degree” AND “ECE - Associate Degree” AND “CDA Child Development Associate”)";
                        }
                        else
                        {
                            RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Assistant Teacher”) AND (Education Completed NOT = “ECE - Graduate Degree” AND “ECE - Master’s Degree” AND “ECE - Baccalaureate Degree” AND “ECE - Associate Degree” AND “CDA Child Development Associate”)";
                        }
                    }
                    break;
                case "20":
                    RuleDesc = "System calculates as Sum of {B.3.c(1) through B.3.e(1)}";
                    break;
                case "21":
                    if (Fund == "9")
                    {
                        RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked)) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher”) AND (Education Progress = “Enrolled in Graduate degree in ECE” OR “Enrolled in Baccalaureate degree in ECE”)";
                    }
                    else
                    {
                        RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher”) AND (Education Progress = “Enrolled in Graduate degree in ECE” OR “Enrolled in Baccalaureate degree in ECE”)";
                    }
                    break;
                case "22":
                    RuleDesc = "System calculates as B.3.e(2)";
                    break;
                case "23":
                    if (Fund == "9")
                    {
                        RuleDesc = "IF ((Work For HS checkbox is checked)  OR (Work For HS2 checkbox is checked)) AND (Active checkbox is checked) AND (Positional Category = “Assistant Teacher”) AND (Education Progress = “Enrolled in Graduate degree in ECE” OR “Enrolled in Baccalaureate degree in ECE” OR “Enrolled in related degree in ECE” OR “Enrolled in CDA Training”)";
                    }
                    else
                    {
                        RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Assistant Teacher”) AND (Education Progress = “Enrolled in Graduate degree in ECE” OR “Enrolled in Baccalaureate degree in ECE” OR “Enrolled in related degree in ECE” OR “Enrolled in CDA Training”)";
                    }
                    break;
                case "24":
                    if (Fund == "9")
                    {
                        RuleDesc = "IF ((Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher”)";
                    }
                    else
                    {
                        RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher”)";
                    }
                    break;
                case "26":
                    if (Fund == "9")
                    {
                        RuleDesc = "IF ((Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher”) AND (Education Completed = “ECE - Graduate Degree” OR “ECE - Master’s Degree”)";
                    }
                    else
                    {
                        RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Positional Category = “Classroom Teacher”)  AND (Education Completed = “ECE - Graduate Degree” OR “ECE - Master’s Degree”)";
                    }
                    break;
                case "29":
                    if (Fund == "9")
                    {
                        RuleDesc = "IF ((Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked))  AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher”) AND (Education Completed = “ECE - Baccalaureate Degree”)";
                    }
                    else
                    {
                        RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Positional Category = “Classroom Teacher”) AND (Education Completed = “ECE - Baccalaureate Degree”)";
                    }
                    break;
                case "32":
                    if (Fund == "9")
                    {
                        RuleDesc = "IF ((Work For EHS checkbox is checked)  OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher”) AND (Education Completed = “ECE - Associate Degree”)";
                    }
                    else
                    {
                        RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Positional Category = “Classroom Teacher”) AND (Education Completed = “ECE - Associate Degree”)";
                    }
                    break;
                case "35":
                    if (Fund == "9")
                    {
                        RuleDesc = "IF ((Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher”) AND (Education Completed = “CDA Child Development Associate”)";
                    }
                    else
                    {
                        RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Positional Category = “Classroom Teacher”) AND (Education Completed = “CDA Child Development Associate”)";
                    }
                    break;
                case "37":
                    if (Fund == "9")
                    {
                        RuleDesc = "IF ((Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher”) AND (Education Completed NOT = “ECE - Graduate Degree” AND “ECE - Master’s Degree” AND “ECE - Baccalaureate Degree” AND “ECE - Associate Degree” AND “CDA Child Development Associate”)";
                    }
                    else
                    {
                        RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Positional Category = “Classroom Teacher”) AND (Education Completed NOT = “ECE - Graduate Degree” AND “ECE - Master’s Degree” AND “ECE - Baccalaureate Degree” AND “ECE - Associate Degree” AND “CDA Child Development Associate”)";
                    }
                    break;
                case "38":
                    RuleDesc = "System calculates as B.6.e";
                    break;
                case "39":
                    if (Fund == "9")
                    {
                        RuleDesc = "IF ((Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher”) AND (Education Progress = “Enrolled in Graduate degree in ECE” OR “Enrolled in Baccalaureate degree in ECE” OR “Enrolled in related degree in ECE” OR “Enrolled in CDA Training”)";
                    }
                    else
                    {
                        RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Positional Category = “Classroom Teacher”) AND (Education Completed NOT = “ECE - Graduate Degree” AND “ECE - Master’s Degree” AND “ECE - Baccalaureate Degree” AND “ECE - Associate Degree” AND “CDA Child Development Associate”)";
                    }
                    break;
                case "40":
                    if (Fund == "9")
                    {
                        RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is checked) AND (Positional Category = “Home Visitor”)";
                    }
                    else
                    {
                        RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Home Visitor”)"; 
                    }
                    break;
                case "41":
                    if (Fund == "9")
                    {
                        RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is checked) AND (Positional Category = “Home Visitor”) AND (Education Completed = “ECE - Graduate Degree” OR “ECE - Master’s Degree” OR “ECE - Baccalaureate Degree” OR “ECE - Associate Degree” OR “CDA Child Development Associate”)";
                    }
                    else
                    {
                        RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Home Visitor”) AND (Education Completed = “ECE - Graduate Degree” OR “ECE - Master’s Degree” OR “ECE - Baccalaureate Degree” OR “ECE - Associate Degree” OR “CDA Child Development Associate”)";
                    }
                    break;
                case "42":
                    if (Fund == "9")
                    {
                        RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is checked) AND (Positional Category = “Home Visitor”) AND (Education Completed = “ECE - Graduate Degree” OR “ECE - Master’s Degree” OR “ECE - Baccalaureate Degree”)";
                    }
                    else
                    {
                        RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Home Visitor”) AND (Education Completed = “ECE - Graduate Degree” OR “ECE - Master’s Degree” OR “ECE - Baccalaureate Degree”)";
                    }
                    break;
                case "43":
                    if (Fund == "9")
                    {
                        RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is checked) AND (Positional Category = “Home Visitor”) AND (Education Completed NOT = “ECE - Graduate Degree” AND “ECE - Master’s Degree” AND “ECE - Baccalaureate Degree” AND “ECE - Associate Degree” AND “CDA Child Development Associate”)";
                    }
                    else
                    {
                        RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Home Visitor”) AND (Education Completed NOT = “ECE - Graduate Degree” AND “ECE - Master’s Degree” AND “ECE - Baccalaureate Degree” AND “ECE - Associate Degree” AND “CDA Child Development Associate”)";
                    }
                    break;
                case "44":
                    if (Fund == "9")
                    {
                        RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is checked) AND (Positional Category = “Home Visitor”) AND (Education Progress = “Enrolled in Graduate degree in ECE” OR “Enrolled in Baccalaureate degree in ECE” OR “Enrolled in CDA Training”)";
                    }
                    else
                    {
                        RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Home Visitor”) AND (Education Completed NOT = “ECE - Graduate Degree” AND “ECE - Master’s Degree” AND “ECE - Baccalaureate Degree” AND “ECE - Associate Degree” AND “CDA Child Development Associate”)";
                    }
                    break;
                case "45":
                    if (Fund == "9")
                    {
                        RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is checked) AND (Positional Category = “Family Child Care Provider”)";
                    }
                    else
                    {
                        RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Family Child Care Provider”)";
                    }
                    break;
                case "46":
                    if (Fund == "9")
                    {
                        RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is checked) AND (Positional Category = “Family Child Care Provider”) AND (Education Completed = “ECE - Graduate Degree” OR “ECE - Master’s Degree” OR “ECE - Baccalaureate Degree” OR “ECE - Associate Degree” OR “CDA Child Development Associate”)";
                    }
                    else
                    {
                        RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Family Child Care Provider”) AND (Education Completed = “ECE - Graduate Degree” OR “ECE - Master’s Degree” OR “ECE - Baccalaureate Degree” OR “ECE - Associate Degree” OR “CDA Child Development Associate”)";
                    }
                    break;
                case "47":
                    if (Fund == "9")
                    {
                        RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is checked) AND (Positional Category = “Family Child Care Provider”) AND (Education Completed = “ECE - Graduate Degree” OR “ECE - Master’s Degree” OR “ECE - Baccalaureate Degree”)";
                    }
                    else
                    {
                        RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Family Child Care Provider”) AND (Education Completed = “ECE - Graduate Degree” OR “ECE - Master’s Degree” OR “ECE - Baccalaureate Degree”)";
                    }
                    break;
                case "48":
                    if (Fund == "9")
                    {
                        RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is checked) AND (Positional Category = “Family Child Care Provider”) AND (Education Completed NOT = “ECE - Graduate Degree” AND “ECE - Master’s Degree” AND “ECE - Baccalaureate Degree” AND “ECE - Associate Degree” AND “CDA Child Development Associate”)";
                    }
                    else
                    {
                        RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Family Child Care Provider”) AND (Education Completed NOT = “ECE - Graduate Degree” AND “ECE - Master’s Degree” AND “ECE - Baccalaureate Degree” AND “ECE - Associate Degree” AND “CDA Child Development Associate”)";
                    }
                    break;
                case "49":
                    if (Fund == "9")
                    {
                        RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is checked) AND (Positional Category = “Family Child Care Provider”) AND (Education Progress = “Enrolled in Graduate degree in ECE” OR “Enrolled in Baccalaureate degree in ECE” OR “Enrolled in CDA Training”)";
                    }
                    else
                    {
                        RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Family Child Care Provider”) AND (Education Progress = “Enrolled in Graduate degree in ECE” OR “Enrolled in Baccalaureate degree in ECE” OR “Enrolled in CDA Training”)";
                    }
                    break;
                case "50":
                    if (Fund == "9")
                    {
                        RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is checked) AND (Positional Category = “Child Development Specialist”)";
                    }
                    else
                    {
                        RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Child Development Specialist”)";
                    }
                    break;
                case "51":
                    if (Fund == "9")
                    {
                        RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is checked) AND (Positional Category = “Child Development Specialist”) AND (Education Completed = “ECE - Baccalaureate Degree” OR “CDA Child Development Associate”)";
                    }
                    else
                    {
                        RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Child Development Specialist”) AND (Education Completed = “ECE - Baccalaureate Degree” OR “CDA Child Development Associate”)";
                    }
                    break;
                case "52":
                    if (Fund == "9")
                    {
                        RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is checked) AND (Positional Category = “Child Development Specialist”) AND (Education Completed NOT = “ECE - Baccalaureate Degree” AND “CDA Child Development Associate”)";
                    }
                    else
                    {
                        RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Child Development Specialist”) AND (Education Completed NOT = “ECE - Baccalaureate Degree” AND “CDA Child Development Associate”)";
                    }
                    break;
                case "53":
                    if (Fund == "9")
                    {
                        RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is checked) AND (Positional Category = “Child Development Specialist”) AND (Education Progress = “Enrolled in Baccalaureate degree in ECE” OR “Enrolled in CDA Training”)";
                    }
                    else
                    {
                        RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Child Development Specialist”) AND (Education Progress = “Enrolled in Baccalaureate degree in ECE” OR “Enrolled in CDA Training”)";
                    }
                    break;
                case "55":
                    if (Fund == "9")
                    {
                        RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”) AND (Education Completed = “ECE - Graduate Degree” OR “ECE - Master’s Degree”)" +
                            "Individual calculation:  Total Salary = Total Salary + Salary" +
                            "Total Teachers = Total Teachers + 1 " +
                            
                            "Final average calculation: Average Annual Salary = Total Salary / Total Teachers";
                    }
                    else
                    {
                        RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”) AND (Education Completed = “ECE - Graduate Degree” OR “ECE - Master’s Degree”)" +
                            "Individual calculation:  Total Salary = Total Salary + Salary  " +
                            "Total Teachers = Total Teachers + 1  "+                            
                            
                            "Final average calculation: Average Annual Salary = Total Salary / Total Teachers";
                    }
                    break;
                case "56":
                    if (Fund == "9")
                    {
                        RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”) AND (Education Completed = “ECE - Baccalaureate Degree”)" +
                            "Individual calculation:  Total Salary = Total Salary + Salary" +
                            "Total Teachers = Total Teachers + 1 " +
                            
                            "Final average calculation: Average Annual Salary = Total Salary / Total Teachers";
                    }
                    else
                    {
                        RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”) AND (Education Completed = “ECE - Baccalaureate Degree”)" +
                            "Individual calculation:  Total Salary = Total Salary + Salary + " +
                            "Total Teachers = Total Teachers + 1  " +
                            
                            "Final average calculation: Average Annual Salary = Total Salary / Total Teachers";
                    }
                    break;
                case "57":
                    if (Fund == "9")
                    {
                        RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”) AND (Education Completed = “ECE - Associate Degree”)" +
                            "Individual calculation:  Total Salary = Total Salary + Salary" +
                            "Total Teachers = Total Teachers + 1 " +
                            
                            "Final average calculation: Average Annual Salary = Total Salary / Total Teachers";
                    }
                    else
                    {
                        RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”)  AND (Education Completed = “ECE - Associate Degree”)" +
                            "Individual calculation:  Total Salary = Total Salary + Salary + " +
                            "Total Teachers = Total Teachers + 1  " +
                            
                            "Final average calculation: Average Annual Salary = Total Salary / Total Teachers";
                    }
                    break;
                case "58":
                    if (Fund == "9")
                    {
                        RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”) AND (Education Completed = “CDA Child Development Associate”)" +
                            "Individual calculation:  Total Salary = Total Salary + Salary" +
                            "Total Teachers = Total Teachers + 1 " +
                            
                            "Final average calculation: Average Annual Salary = Total Salary / Total Teachers";
                    }
                    else
                    {
                        RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”)  AND (Education Completed = “CDA Child Development Associate”)" +
                            "Individual calculation:  Total Salary = Total Salary + Salary + " +
                            "Total Teachers = Total Teachers + 1  " +
                            
                            "Final average calculation: Average Annual Salary = Total Salary / Total Teachers";
                    }
                    break;
                case "59":
                    if (Fund == "9")
                    {
                        RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”) AND (Education Completed NOT = “ECE - Graduate Degree” AND “ECE - Master’s Degree” AND “ECE - Baccalaureate Degree” AND “ECE - Associate Degree” AND “CDA Child Development Associate”)" +
                            "Individual calculation:  Total Salary = Total Salary + Salary" +
                            "Total Teachers = Total Teachers + 1 " +
                            
                            "Final average calculation: Average Annual Salary = Total Salary / Total Teachers";
                    }
                    else
                    {
                        RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”)  AND (Education Completed NOT = “ECE - Graduate Degree” AND “ECE - Master’s Degree” AND “ECE - Baccalaureate Degree” AND “ECE - Associate Degree” AND “CDA Child Development Associate”)" +
                            "Individual calculation:  Total Salary = Total Salary + Salary + " +
                            "Total Teachers = Total Teachers + 1  " +
                            
                            "Final average calculation: Average Annual Salary = Total Salary / Total Teachers";
                    }
                    break;
                case "61":
                    if (Col == "1")
                    {
                        if (Fund == "9")
                        {
                            RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher”)" +
                                "Individual calculation:  Total Salary = Total Salary + Salary" +
                                "Total Teachers = Total Teachers + 1 " +
                                
                                "Final average calculation: Average Annual Salary = Total Salary / Total Teachers";
                        }
                        else
                        {
                            RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher”)" +
                                "Individual calculation:  Total Salary = Total Salary + Salary + " +
                                "Total Teachers = Total Teachers + 1  " +
                                
                                "Final average calculation: Average Annual Salary = Total Salary / Total Teachers";
                        }
                    }
                    else if (Col == "2")
                    {
                        if (Fund == "9")
                        {
                            RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher”)" +
                                "Individual calculation: Total Horuly Rate = Total Hourly Rate +Hourly Rate" +
                                     "Total Teachers = Total Teachers + 1" +

                                "Final average calculation: Average Hourly Rate = Total Hourly Rate / Total Teachers";
                        }
                        else
                        {
                            RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher”)" +
                                "Individual calculation: Total Horuly Rate = Total Hourly Rate +Hourly Rate" +
                                     "Total Teachers = Total Teachers + 1" +

                                "Final average calculation: Average Hourly Rate = Total Hourly Rate / Total Teachers";
                        }
                    }
                    
                    break;
                case "62":
                    if (Col == "1")
                    {
                        if (Fund == "9")
                        {
                            RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is checked) AND (Positional Category = “Assistant Teacher”)" +
                                "Individual calculation:  Total Salary = Total Salary + Salary" +
                                "Total Teachers = Total Teachers + 1 " +

                                "Final average calculation: Average Annual Salary = Total Salary / Total Teachers";
                        }
                        else
                        {
                            RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Assistant Teacher”)" +
                                "Individual calculation:  Total Salary = Total Salary + Salary + " +
                                "Total Teachers = Total Teachers + 1  " +

                                "Final average calculation: Average Annual Salary = Total Salary / Total Teachers";
                        }
                    }
                    else if (Col == "2")
                    {
                        if (Fund == "9")
                        {
                            RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is checked) AND (Positional Category = “Assistant Teacher”)" +
                                "Individual calculation: Total Horuly Rate = Total Hourly Rate +Hourly Rate" +
                                     "Total Teachers = Total Teachers + 1" +

                                "Final average calculation: Average Hourly Rate = Total Hourly Rate / Total Teachers";
                        }
                        else
                        {
                            RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Assistant Teacher”)" +
                                "Individual calculation: Total Horuly Rate = Total Hourly Rate +Hourly Rate" +
                                     "Total Teachers = Total Teachers + 1" +

                                "Final average calculation: Average Hourly Rate = Total Hourly Rate / Total Teachers";
                        }
                    }

                    break;
                case "63":
                    if (Col == "1")
                    {
                        if (Fund == "9")
                        {
                            RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is checked) AND (Positional Category = “Home Visitor”)" +
                                "Individual calculation:  Total Salary = Total Salary + Salary" +
                                "Total Teachers = Total Teachers + 1 " +

                                "Final average calculation: Average Annual Salary = Total Salary / Total Teachers";
                        }
                        else
                        {
                            RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Home Visitor”)" +
                                "Individual calculation:  Total Salary = Total Salary + Salary + " +
                                "Total Teachers = Total Teachers + 1  " +

                                "Final average calculation: Average Annual Salary = Total Salary / Total Teachers";
                        }
                    }
                    else if (Col == "2")
                    {
                        if (Fund == "9")
                        {
                            RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is checked) AND (Positional Category = “Home Visitor”)" +
                                "Individual calculation: Total Horuly Rate = Total Hourly Rate +Hourly Rate" +
                                     "Total Teachers = Total Teachers + 1" +

                                "Final average calculation: Average Hourly Rate = Total Hourly Rate / Total Teachers";
                        }
                        else
                        {
                            RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Home Visitor”)" +
                                "Individual calculation: Total Horuly Rate = Total Hourly Rate +Hourly Rate" +
                                     "Total Teachers = Total Teachers + 1" +

                                "Final average calculation: Average Hourly Rate = Total Hourly Rate / Total Teachers";
                        }
                    }

                    break;
                case "64":
                    if (Col == "1")
                    {
                        if (Fund == "9")
                        {
                            RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is checked) AND (Positional Category = “Family Child Care Provider”)" +
                                "Individual calculation:  Total Salary = Total Salary + Salary" +
                                "Total Teachers = Total Teachers + 1 " +

                                "Final average calculation: Average Annual Salary = Total Salary / Total Teachers";
                        }
                        else
                        {
                            RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Family Child Care Provider”)" +
                                "Individual calculation:  Total Salary = Total Salary + Salary + " +
                                "Total Teachers = Total Teachers + 1  " +

                                "Final average calculation: Average Annual Salary = Total Salary / Total Teachers";
                        }
                    }
                    else if (Col == "2")
                    {
                        if (Fund == "9")
                        {
                            RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is checked) AND (Positional Category = “Family Child Care Provider”)" +
                                "Individual calculation: Total Horuly Rate = Total Hourly Rate +Hourly Rate" +
                                     "Total Teachers = Total Teachers + 1" +

                                "Final average calculation: Average Hourly Rate = Total Hourly Rate / Total Teachers";
                        }
                        else
                        {
                            RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Family Child Care Provider”)" +
                                "Individual calculation: Total Horuly Rate = Total Hourly Rate +Hourly Rate" +
                                     "Total Teachers = Total Teachers + 1" +

                                "Final average calculation: Average Hourly Rate = Total Hourly Rate / Total Teachers";
                        }
                    }

                    break;
                case "66":
                    if (Col == "1")
                    {
                        if (Fund == "9")
                        {
                            RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”  OR “Home Visitor” OR “Family Child Care Provider”) AND (Race = “AMERICAN INDIAN/ALASKA NATIVE”) AND (Ethnicity = “HISPANIC OR LATINO ORIGIN”)";
                        }
                        else
                        {
                            RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”  OR “Home Visitor” OR “Family Child Care Provider”) AND (Race = “AMERICAN INDIAN/ALASKA NATIVE”) AND (Ethnicity = “HISPANIC OR LATINO ORIGIN”)";
                        }

                    }
                    else if (Col == "2")
                    {
                        if (Fund == "9")
                        {
                            RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked))  AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”  OR “Home Visitor” OR “Family Child Care Provider”) AND (Race = “AMERICAN INDIAN/ALASKA NATIVE”) AND (Ethnicity = “NON-HISPANIC OR NON-LATINO ORIGIN”)";
                        }
                        else
                        {
                            RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”  OR “Home Visitor” OR “Family Child Care Provider”) AND (Race = “AMERICAN INDIAN/ALASKA NATIVE”) AND (Ethnicity = “NON-HISPANIC OR NON-LATINO ORIGIN”)";
                        }
                    }
                    break;
                case "67":
                    if (Col == "1")
                    {
                        if (Fund == "9")
                        {
                            RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”  OR “Home Visitor” OR “Family Child Care Provider”) AND (Race = “ASIAN”) AND (Ethnicity = “HISPANIC OR LATINO ORIGIN”)";
                        }
                        else
                        {
                            RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”  OR “Home Visitor” OR “Family Child Care Provider”) AND (Race = “ASIAN”) AND (Ethnicity = “HISPANIC OR LATINO ORIGIN”)";
                        }

                    }
                    else if (Col == "2")
                    {
                        if (Fund == "9")
                        {
                            RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked))  AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”  OR “Home Visitor” OR “Family Child Care Provider”) AND (Race = “ASIAN”) AND (Ethnicity = “NON-HISPANIC OR NON-LATINO ORIGIN”)";
                        }
                        else
                        {
                            RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”  OR “Home Visitor” OR “Family Child Care Provider”) AND (Race = “ASIAN”) AND (Ethnicity = “NON-HISPANIC OR NON-LATINO ORIGIN”)";
                        }
                    }
                    break;
                case "68":
                    if (Col == "1")
                    {
                        if (Fund == "9")
                        {
                            RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”  OR “Home Visitor” OR “Family Child Care Provider”) AND (Race = “BLACK/AFRICAN AMERICAN”) AND (Ethnicity = “HISPANIC OR LATINO ORIGIN”)";
                        }
                        else
                        {
                            RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”  OR “Home Visitor” OR “Family Child Care Provider”) AND (Race = “BLACK/AFRICAN AMERICAN”) AND (Ethnicity = “HISPANIC OR LATINO ORIGIN”)";
                        }

                    }
                    else if (Col == "2")
                    {
                        if (Fund == "9")
                        {
                            RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked))  AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”  OR “Home Visitor” OR “Family Child Care Provider”) AND (Race = “BLACK/AFRICAN AMERICAN”) AND (Ethnicity = “NON-HISPANIC OR NON-LATINO ORIGIN”)";
                        }
                        else
                        {
                            RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”  OR “Home Visitor” OR “Family Child Care Provider”) AND (Race = “BLACK/AFRICAN AMERICAN”) AND (Ethnicity = “NON-HISPANIC OR NON-LATINO ORIGIN”)";
                        }
                    }
                    break;
                case "69":
                    if (Col == "1")
                    {
                        if (Fund == "9")
                        {
                            RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”  OR “Home Visitor” OR “Family Child Care Provider”) AND (Race = “NATIVE HAWAIIAN OR OTHER PACIFIC ISLANDER”) AND (Ethnicity = “HISPANIC OR LATINO ORIGIN”)";
                        }
                        else
                        {
                            RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”  OR “Home Visitor” OR “Family Child Care Provider”) AND (Race = “NATIVE HAWAIIAN OR OTHER PACIFIC ISLANDER”) AND (Ethnicity = “HISPANIC OR LATINO ORIGIN”)";
                        }

                    }
                    else if (Col == "2")
                    {
                        if (Fund == "9")
                        {
                            RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked))  AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”  OR “Home Visitor” OR “Family Child Care Provider”) AND (Race = “NATIVE HAWAIIAN OR OTHER PACIFIC ISLANDER”) AND (Ethnicity = “NON-HISPANIC OR NON-LATINO ORIGIN”)";
                        }
                        else
                        {
                            RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”  OR “Home Visitor” OR “Family Child Care Provider”) AND (Race = “NATIVE HAWAIIAN OR OTHER PACIFIC ISLANDER”) AND (Ethnicity = “NON-HISPANIC OR NON-LATINO ORIGIN”)";
                        }
                    }
                    break;
                case "70":
                    if (Col == "1")
                    {
                        if (Fund == "9")
                        {
                            RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”  OR “Home Visitor” OR “Family Child Care Provider”) AND (Race = “CAUCASIAN (WHITE)”) AND (Ethnicity = “HISPANIC OR LATINO ORIGIN”)";
                        }
                        else
                        {
                            RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”  OR “Home Visitor” OR “Family Child Care Provider”) AND (Race = “CAUCASIAN (WHITE)”) AND (Ethnicity = “HISPANIC OR LATINO ORIGIN”)";
                        }

                    }
                    else if (Col == "2")
                    {
                        if (Fund == "9")
                        {
                            RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked))  AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”  OR “Home Visitor” OR “Family Child Care Provider”) AND (Race = “CAUCASIAN (WHITE)”) AND (Ethnicity = “NON-HISPANIC OR NON-LATINO ORIGIN”)";
                        }
                        else
                        {
                            RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”  OR “Home Visitor” OR “Family Child Care Provider”) AND (Race = “CAUCASIAN (WHITE)”) AND (Ethnicity = “NON-HISPANIC OR NON-LATINO ORIGIN”)";
                        }
                    }
                    break;
                case "71":
                    if (Col == "1")
                    {
                        if (Fund == "9")
                        {
                            RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”  OR “Home Visitor” OR “Family Child Care Provider”) AND (Race = “BIRACIAL/MULTI-RACE”) AND (Ethnicity = “HISPANIC OR LATINO ORIGIN”)";
                        }
                        else
                        {
                            RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”  OR “Home Visitor” OR “Family Child Care Provider”) AND (Race = “BIRACIAL/MULTI-RACE”) AND (Ethnicity = “HISPANIC OR LATINO ORIGIN”)";
                        }

                    }
                    else if (Col == "2")
                    {
                        if (Fund == "9")
                        {
                            RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked))  AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”  OR “Home Visitor” OR “Family Child Care Provider”) AND (Race = “BIRACIAL/MULTI-RACE”) AND (Ethnicity = “NON-HISPANIC OR NON-LATINO ORIGIN”)";
                        }
                        else
                        {
                            RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”  OR “Home Visitor” OR “Family Child Care Provider”) AND (Race = “BIRACIAL/MULTI-RACE”) AND (Ethnicity = “NON-HISPANIC OR NON-LATINO ORIGIN”)";
                        }
                    }
                    break;
                case "72":
                    if (Col == "1")
                    {
                        if (Fund == "9")
                        {
                            RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”  OR “Home Visitor” OR “Family Child Care Provider”) AND (Race = “OTHER”) AND (Ethnicity = “HISPANIC OR LATINO ORIGIN”)";
                        }
                        else
                        {
                            RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”  OR “Home Visitor” OR “Family Child Care Provider”) AND (Race = “OTHER”) AND (Ethnicity = “HISPANIC OR LATINO ORIGIN”)";
                        }

                    }
                    else if (Col == "2")
                    {
                        if (Fund == "9")
                        {
                            RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked))  AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”  OR “Home Visitor” OR “Family Child Care Provider”) AND (Race = “OTHER”) AND (Ethnicity = “NON-HISPANIC OR NON-LATINO ORIGIN”)";
                        }
                        else
                        {
                            RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”  OR “Home Visitor” OR “Family Child Care Provider”) AND (Race = “OTHER”) AND (Ethnicity = “NON-HISPANIC OR NON-LATINO ORIGIN”)";
                        }
                    }
                    break;
                case "74":
                    if (Fund == "9")
                    {
                        RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”  OR “Home Visitor” OR “Family Child Care Provider”) AND ((Race = “UNSPECIFIED”) OR (Ethnicity = “Unspecified”))";
                    }
                    else
                    {
                        RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”  OR “Home Visitor” OR “Family Child Care Provider”) AND ((Race = “UNSPECIFIED”) OR (Ethnicity = “Unspecified”))";
                    }

                    break;
                case "76":
                    if (Fund == "9")
                    {
                        RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”  OR “Home Visitor” OR “Family Child Care Provider”) AND ((Primary Language NOT = “ENGLISH” OR “None”) OR (Language1 NOT = “ENGLISH” OR “None”) OR (Language2 NOT = “ENGLISH” OR “None”) OR (Language3 NOT = “ENGLISH” OR “None”))";
                    }
                    else
                    {
                        RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”  OR “Home Visitor” OR “Family Child Care Provider”) AND ((Primary Language NOT = “ENGLISH” OR “None”) OR (Language1 NOT = “ENGLISH” OR “None”) OR (Language2 NOT = “ENGLISH” OR “None”) OR (Language3 NOT = “ENGLISH” OR “None”))";
                    }
                    break;
                case "77":
                    if (Fund == "9")
                    {
                        RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”  OR “Home Visitor” OR “Family Child Care Provider”) AND (((Primary Language NOT = “ENGLISH” OR “None”) AND (Language1 NOT = “ENGLISH” OR “None”))  AND (Language2 NOT = “ENGLISH” OR “None”))  AND (Language3 NOT = “ENGLISH” OR “None”)))";
                    }
                    else
                    {
                        RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”  OR “Home Visitor” OR “Family Child Care Provider”) AND ((Primary Language NOT = “ENGLISH” OR “None”) OR (Language1 NOT = “ENGLISH” OR “None”) OR (Language2 NOT = “ENGLISH” OR “None”) OR (Language3 NOT = “ENGLISH” OR “None”))";
                    }
                    break;
                case "79":
                    if (Fund == "9")
                    {
                        RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”  OR “Home Visitor” OR “Family Child Care Provider”) AND ((Primary Language = “SPANISH”) OR (Language1 = “SPANISH”) OR (Language2 = “SPANISH”) OR (Language3 = “SPANISH”))";
                    }
                    else
                    {
                        RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”  OR “Home Visitor” OR “Family Child Care Provider”) AND ((Primary Language = “SPANISH”) OR (Language1 = “SPANISH”) OR (Language2 = “SPANISH”) OR (Language3 = “SPANISH”))";
                    }
                    break;
                case "80":
                    if (Fund == "9")
                    {
                        RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”  OR “Home Visitor” OR “Family Child Care Provider”) AND ((Primary Language = “Native Central American, South American, and Mexican”) OR (Language1 = “Native Central American, South American, and Mexican”) OR (Language2 = “Native Central American, South American, and Mexican”) OR (Language3 = “Native Central American, South American, and Mexican”))";
                    }
                    else
                    {
                        RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”  OR “Home Visitor” OR “Family Child Care Provider”) AND ((Primary Language = “Native Central American, South American, and Mexican”) OR (Language1 = “Native Central American, South American, and Mexican”) OR (Language2 = “Native Central American, South American, and Mexican”) OR (Language3 = “Native Central American, South American, and Mexican”))";
                    }
                    break;
                case "81":
                    if (Fund == "9")
                    {
                        RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”  OR “Home Visitor” OR “Family Child Care Provider”) AND ((Primary Language = “CARRIBEAN”) OR (Language1 = “CARRIBEAN”) OR (Language2 = “CARRIBEAN”) OR (Language3 = “CARRIBEAN”))";
                    }
                    else
                    {
                        RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”  OR “Home Visitor” OR “Family Child Care Provider”) AND ((Primary Language = “CARRIBEAN”) OR (Language1 = “CARRIBEAN”) OR (Language2 = “CARRIBEAN”) OR (Language3 = “CARRIBEAN”))";
                    }
                    break;
                case "82":
                    if (Fund == "9")
                    {
                        RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”  OR “Home Visitor” OR “Family Child Care Provider”) AND ((Primary Language = “Middle Eastern & South Asian”) OR (Language1 = “Middle Eastern & South Asian”) OR (Language2 = “Middle Eastern & South Asian”) OR (Language3 = “Middle Eastern & South Asian”))";
                    }
                    else
                    {
                        RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”  OR “Home Visitor” OR “Family Child Care Provider”) AND ((Primary Language = “Middle Eastern & South Asian”) OR (Language1 = “Middle Eastern & South Asian”) OR (Language2 = “Middle Eastern & South Asian”) OR (Language3 = “Middle Eastern & South Asian”))";
                    }
                    break;
                case "83":
                    if (Fund == "9")
                    {
                        RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”  OR “Home Visitor” OR “Family Child Care Provider”) AND ((Primary Language = “East Asian”) OR (Language1 = “East Asian”) OR (Language2 = “East Asian”) OR (Language3 = “East Asian”))";
                    }
                    else
                    {
                        RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”  OR “Home Visitor” OR “Family Child Care Provider”) AND ((Primary Language = “East Asian”) OR (Language1 = “East Asian”) OR (Language2 = “East Asian”) OR (Language3 = “East Asian”))";
                    }
                    break;
                case "84":
                    if (Fund == "9")
                    {
                        RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”  OR “Home Visitor” OR “Family Child Care Provider”) AND ((Primary Language = “Native North American/Alaska Native”) OR (Language1 = “Native North American/Alaska Native”) OR (Language2 = “Native North American/Alaska Native”) OR (Language3 = “Native North American/Alaska Native”))";
                    }
                    else
                    {
                        RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”  OR “Home Visitor” OR “Family Child Care Provider”) AND ((Primary Language = “Native North American/Alaska Native”) OR (Language1 = “Native North American/Alaska Native”) OR (Language2 = “Native North American/Alaska Native”) OR (Language3 = “Native North American/Alaska Native”))";
                    }
                    break;
                case "85":
                    if (Fund == "9")
                    {
                        RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”  OR “Home Visitor” OR “Family Child Care Provider”) AND ((Primary Language = “Pacific Island”) OR (Language1 = “Pacific Island”) OR (Language2 = “Pacific Island”) OR (Language3 = “Pacific Island”))";
                    }
                    else
                    {
                        RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”  OR “Home Visitor” OR “Family Child Care Provider”) AND ((Primary Language = “Pacific Island”) OR (Language1 = “Pacific Island”) OR (Language2 = “Pacific Island”) OR (Language3 = “Pacific Island”))";
                    }
                    break;
                case "86":
                    if (Fund == "9")
                    {
                        RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is checked) AND ((Primary Language = “European & Slavic”) OR (Language1 = “European & Slavic”) OR (Language2 = “European & Slavic”) OR (Language3 = “European & Slavic”))";
                    }
                    else
                    {
                        RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND ((Primary Language = “European & Slavic”) OR (Language1 = “European & Slavic”) OR (Language2 = “European & Slavic”) OR (Language3 = “European & Slavic”))";
                    }
                    break;
                case "87":
                    if (Fund == "9")
                    {
                        RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is checked) AND ((Primary Language = “AFRICAN”) OR (Language1 = “AFRICAN”) OR (Language2 = “AFRICAN”) OR (Language3 = “AFRICAN”))";
                    }
                    else
                    {
                        RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND ((Primary Language = “AFRICAN”) OR (Language1 = “AFRICAN”) OR (Language2 = “AFRICAN”) OR (Language3 = “AFRICAN”))";
                    }
                    break;
                case "88":
                    if (Fund == "9")
                    {
                        RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is checked) AND ((Primary Language = “American Sign Language”) OR (Language1 = “American Sign Language”) OR (Language2 = “American Sign Language”) OR (Language3 = “American Sign Language”))";
                    }
                    else
                    {
                        RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND ((Primary Language = “American Sign Language”) OR (Language1 = “American Sign Language”) OR (Language2 = “American Sign Language”) OR (Language3 = “American Sign Language”))";
                    }
                    break;
                case "89":
                    if (Fund == "9")
                    {
                        RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is checked) AND ((Primary Language = “OTHER”) OR (Language1 = “OTHER”) OR (Language2 = “OTHER”) OR (Language3 = “OTHER”))";
                    }
                    else
                    {
                        RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND ((Primary Language = “OTHER”) OR (Language1 = “OTHER”) OR (Language2 = “OTHER”) OR (Language3 = “OTHER”))";
                    }
                    break;
                case "91":
                    if (Fund == "9")
                    {
                        RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is checked) AND ((Primary Language = “Unspecified”) AND (Language1 = “Unspecified”) AND (Language2 = “Unspecified”) AND (Language3 = “Unspecified”))";
                    }
                    else
                    {
                        RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is checked) AND ((Primary Language = “Unspecified”) AND (Language1 = “Unspecified”) AND (Language2 = “Unspecified”) AND (Language3 = “Unspecified”))";
                    }
                    break;
                case "92":
                    if(Col=="1")
                    {
                        if (Fund == "9")
                        {
                            RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is unchecked) AND (Date Terminated = “Valid Date”)";
                        }
                        else
                        {
                            RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is unchecked) AND (Date Terminated = “Valid Date”)";
                        }
                    }
                    else
                    {
                        RuleDesc = "IF (Work For Contracted checkbox is checked) AND (Active checkbox is unchecked) AND (Date Terminated = “Valid Date”)";
                    }
                    
                    break;
                case "93":
                    if (Col == "1")
                    {
                        if (Fund == "9")
                        {
                            RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is unchecked) AND (Date Terminated = “Valid Date”)  AND (Replaced by another staff member checkbox is checked)";
                        }
                        else
                        {
                            RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is unchecked) AND (Date Terminated = “Valid Date”)  AND (Replaced by another staff member checkbox is checked)";
                        }
                    }
                    else
                    {
                        RuleDesc = "IF (Work For Contracted checkbox is checked) AND (Active checkbox is unchecked) AND (Date Terminated = “Valid Date”)  AND (Replaced by another staff member checkbox is checked)";
                    }

                    break;
                case "94":
                    if (Fund == "9")
                    {
                        RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is unchecked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”  OR “Home Visitor” OR “Family Child Care Provider”) AND (Date Terminated = “Valid Date”)";
                    }
                    else
                    {
                        RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is unchecked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”  OR “Home Visitor” OR “Family Child Care Provider”) AND (Date Terminated = “Valid Date”)";
                    }
                    break;
                case "95":
                    if (Fund == "9")
                    {
                        RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is “Unchecked”) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”  OR “Home Visitor” OR “Family Child Care Provider”) AND (Date Terminated = “Valid Date”) AND (Replaced by another staff member checkbox is checked)";
                    }
                    else
                    {
                        RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is unchecked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher”  OR “Home Visitor” OR “Family Child Care Provider”) AND (Date Terminated = “Valid Date”) AND (Replaced by another staff member checkbox is checked)";
                    }
                    break;
                case "97":
                    if (Fund == "9")
                    {
                        RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is “Unchecked”) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher” ) AND  (Date Terminated = “Valid Date”)";
                    }
                    else
                    {
                        RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is unchecked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher” ) AND  (Date Terminated = “Valid Date”)";
                    }
                    break;
                case "99":
                    if (Fund == "9")
                    {
                        RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is “Unchecked”) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher” OR “Home Visitor” OR “Family Child Care Provider”) AND (Date Terminated = “Valid Date”) AND (Reason for termination = “Higher Compensation”)";
                    }
                    else
                    {
                        RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is unchecked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher” OR “Home Visitor” OR “Family Child Care Provider”) AND (Date Terminated = “Valid Date”) AND (Reason for termination = “Higher Compensation”)";
                    }
                    break;
                case "101":
                    if (Fund == "9")
                    {
                        RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is “Unchecked”) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher” OR “Home Visitor” OR “Family Child Care Provider”) AND (Date Terminated = “Valid Date”) AND (Reason for termination = “Retirement” OR “Relocation”)";
                    }
                    else
                    {
                        RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is unchecked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher” OR “Home Visitor” OR “Family Child Care Provider”) AND (Date Terminated = “Valid Date”) AND (Reason for termination = “Retirement” OR “Relocation”)";
                    }
                    break;
                case "102":
                    if (Fund == "9")
                    {
                        RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is “Unchecked”) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher” OR “Home Visitor” OR “Family Child Care Provider”) AND (Date Terminated = “Valid Date”) AND (Reason for termination = “Position Eliminated” OR “Released from Employment” OR “Staff Reduction”)";
                    }
                    else
                    {
                        RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is unchecked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher” OR “Home Visitor” OR “Family Child Care Provider”) AND (Date Terminated = “Valid Date”) AND (Reason for termination = “Position Eliminated” OR “Released from Employment” OR “Staff Reduction”)";
                    }
                    break;
                case "103":
                    if (Fund == "9")
                    {
                        RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is “Unchecked”) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher” OR “Home Visitor” OR “Family Child Care Provider”) AND (Date Terminated = “Valid Date”) AND (Reason for termination = “Other”)";
                    }
                    else
                    {
                        RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is unchecked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher” OR “Home Visitor” OR “Family Child Care Provider”) AND (Date Terminated = “Valid Date”) AND (Reason for termination = “Other”)";
                    }
                    break;
                case "105":
                    if (Fund == "9")
                    {
                        RuleDesc = "IF ((Work For HS checkbox is checked) OR (Work For HS2 checkbox is checked) OR (Work For EHS checkbox is checked) OR (Work For EHSCCP checkbox is checked)) AND (Active checkbox is “Unchecked”) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher” OR “Home Visitor” OR “Family Child Care Provider”) AND (Date Terminated = “Valid Date”) AND (Position Filled after 3 months is unchecked)";
                    }
                    else
                    {
                        RuleDesc = "If (Work For " + ((Captain.Common.Utilities.ListItem)cmbFunds.SelectedItem).Text.ToString() + " checkbox is checked) AND (Active checkbox is unchecked) AND (Positional Category = “Classroom Teacher” OR “Assistant Teacher” OR “Home Visitor” OR “Family Child Care Provider”) AND (Date Terminated = “Valid Date”) AND (Position Filled after 3 months is unchecked)";
                    }
                    break;
            }



            return RuleDesc;
        }



    }
}
