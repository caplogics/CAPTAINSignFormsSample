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

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class HSSB0118_ReportForm : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        //private GridControl _intakeHierarchy = null;
        private bool boolChangeStatus = false;
        private CaptainModel _model = null;
        private List<HierarchyEntity> _selectedHierarchies = null;

        #endregion
        public HSSB0118_ReportForm(BaseForm baseForm, PrivilegeEntity privileges)
        {
            InitializeComponent();
            _model = new CaptainModel();
            BaseForm = baseForm;
            Privileges = privileges;
            Agency = BaseForm.BaseAgency; Depart = BaseForm.BaseDept; Program = BaseForm.BaseProg;
            strYear = BaseForm.BaseYear;
            Set_Report_Hierarchy(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear);
            propReportPath = _model.lookupDataAccess.GetReportPath();
            this.Text = /*Privileges.Program + " - " +*/ Privileges.PrivilegeName.Trim();
            FillSortByCombo();
            fillRouteCombo(Agency, Depart, Program, strYear);   //Added by Vikash on 06/16/2023 for Route No fill at load
            this.Size = new Size(752, 237);
            strFolderPath = Consts.Common.ReportFolderLocation + BaseForm.UserID + "\\";

        }
        string Agency = string.Empty, Depart = string.Empty, Program = string.Empty, strYear = string.Empty;
        #region properties

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public string Calling_ID { get; set; }

        public string Calling_UserID { get; set; }

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
                FillYearCombo(Agy, Dept, Prog, Year);
            else
            {
                this.Txt_HieDesc.Size = new System.Drawing.Size(675, 25);
                FillYearCombo(Agy, Dept, Prog, Year);
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
            this.cmbBus.SelectedIndexChanged -= new System.EventHandler(this.cmbBus_SelectedIndexChanged);
            fillBusCombo(Agency, Depart, Program, strYear);
            fillRouteCombo(Agency, Depart, Program, strYear);   //Added by Vikash on 06/16/2023 to fill combo with Hierarchy change
            this.cmbBus.SelectedIndexChanged += new System.EventHandler(this.cmbBus_SelectedIndexChanged);
            if (!string.IsNullOrEmpty(Program_Year.Trim()))
                this.Txt_HieDesc.Size = new System.Drawing.Size(595, 25);
            else
                this.Txt_HieDesc.Size = new System.Drawing.Size(675, 25);
        }

        private void Pb_Search_Hie_Click(object sender, EventArgs e)
        {
            //HierarchieSelectionForm hierarchieSelectionForm = new HierarchieSelectionForm(BaseForm, "Master", Current_Hierarchy_DB, string.Empty, "Reports");
            //hierarchieSelectionForm.FormClosed += new Form.FormClosedEventHandler(OnHierarchieFormClosed);
            //hierarchieSelectionForm.ShowDialog();

           /* HierarchieSelectionFormNew hierarchieSelectionForm = new HierarchieSelectionFormNew(BaseForm, Current_Hierarchy_DB, "Master", "", "D", "Reports");
            hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
            hierarchieSelectionForm.ShowDialog();*/

            HierarchieSelection hierarchieSelectionForm = new HierarchieSelection(BaseForm, Current_Hierarchy_DB, "Master", "", "D", "Reports", BaseForm.UserID);
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


        private void fillBusCombo(string Agy, string Dept, string Prog, string Year)
        {
            cmbBus.Items.Clear();
            List<ChldBMEntity> BusList = new List<ChldBMEntity>();
            ChldBMEntity SearchEntity = new ChldBMEntity(true);
            SearchEntity.ChldBMAgency = Agy;
            SearchEntity.chldBMDept = Dept;
            SearchEntity.ChldBMProg = Prog;
            BusList = _model.SPAdminData.Browse_ChldBM(SearchEntity, "Browse");
            string Tmp_Desc = string.Empty;
            cmbBus.Items.Add(new Captain.Common.Utilities.ListItem("********** " + "-" + "All", "**"));
            if (BusList.Count > 0)
            {
                foreach (ChldBMEntity Entity in BusList)
                {
                    Tmp_Desc = string.Empty;
                    Tmp_Desc = String.Format("{0,-11}", Entity.ChldBMNumber.ToString().Trim()) + String.Format("{0,8}", " - " + Entity.Desc.ToString().Trim());
                    cmbBus.Items.Add(new Captain.Common.Utilities.ListItem(Tmp_Desc, Entity.ChldBMNumber.ToString().Trim()));
                }
            }
            if (cmbBus.Items.Count > 0)
                cmbBus.SelectedIndex = 0;
        }

        private void fillRouteCombo(string Agy, string Dept, string Prog, string Year)
        {
            cmbRoute.Items.Clear();
            List<BusRTEntity> RouteList = new List<BusRTEntity>();
            BusRTEntity SearchEntity = new BusRTEntity(true);
            SearchEntity.BUSRT_AGENCY = Agy;
            SearchEntity.BUSRT_DEPT = Dept;
            SearchEntity.BUSRT_PROGRAM = Prog;
            SearchEntity.BUSRT_YEAR = Year; SearchEntity.BUSRT_NUMBER = ((Captain.Common.Utilities.ListItem)cmbBus.SelectedItem).Value.ToString().Trim();
            RouteList = _model.SPAdminData.Browse_ChldBUSR(SearchEntity, "Browse");
            string Tmp_Desc = string.Empty;
            cmbRoute.Items.Add(new Captain.Common.Utilities.ListItem("********** " + "-" + "All", "**"));
            if (RouteList.Count > 0)
            {
                //cmbRoute.Items.Add(new ListItem("********** " + "-" + "All", "**"));
                if (((Captain.Common.Utilities.ListItem)cmbBus.SelectedItem).Value.ToString().Trim() != "**")
                {
                    foreach (BusRTEntity Entity in RouteList)
                    {
                        Tmp_Desc = string.Empty;
                        Tmp_Desc = String.Format("{0,-11}", Entity.BUSRT_ROUTE.ToString().Trim()) + String.Format("{0,8}", " - " + Entity.BUSRT_AREA_SERVED.ToString().Trim());
                        cmbRoute.Items.Add(new Captain.Common.Utilities.ListItem(Tmp_Desc, Entity.BUSRT_ROUTE.ToString().Trim()));
                    }
                }
            }
            if (cmbRoute.Items.Count > 0)
                cmbRoute.SelectedIndex = 0;
        }

        private void FillSortByCombo()
        {
            cmbSort.Items.Clear();
            List<Captain.Common.Utilities.ListItem> listItem2 = new List<Captain.Common.Utilities.ListItem>();
            //listItem.Add(new ListItem("Form Title                                    -  SCR-CODE", SCR-CODE, Form Title, Custom_Filedls_Flag));
            listItem2.Add(new Captain.Common.Utilities.ListItem("Pickup", "01"));
            listItem2.Add(new Captain.Common.Utilities.ListItem("Dropoff", "02"));
            listItem2.Add(new Captain.Common.Utilities.ListItem("Child Name", "03"));
            listItem2.Add(new Captain.Common.Utilities.ListItem("Address", "04"));
            cmbSort.Items.AddRange(listItem2.ToArray());
            cmbSort.SelectedIndex = 0;
        }



        private void BtnGenPdf_Click(object sender, EventArgs e)
        {
            PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, false, propReportPath, "PDF");
            pdfListForm.FormClosed += new FormClosedEventHandler(On_SaveFormClosed);
            pdfListForm.StartPosition = FormStartPosition.CenterScreen;
            pdfListForm.ShowDialog();
        }

        private void BtnPdfPrev_Click(object sender, EventArgs e)
        {
            PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, true, propReportPath);
            pdfListForm.StartPosition = FormStartPosition.CenterScreen;
            pdfListForm.ShowDialog();
        }

        private void cmbBus_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillRouteCombo(Agency, Depart, Program, strYear);
        }



        PdfContentByte cb;
        int X_Pos, Y_Pos;
        string strFolderPath = string.Empty;
        string Random_Filename = null; string PdfName = "Pdf File";
        BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
        //int pageNumber = 1;
        //string PdfScreen = null, rnkCd = null, PrivrnkCd = null, Rankdesc = null;
        //string PrintText = null;
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
                iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 10, 3);
                iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_times, 9, 1);
                iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 8, 2);
                iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 9, 4);
                cb = writer.DirectContent;

                List<ChldBMEntity> BusList = new List<ChldBMEntity>();
                ChldBMEntity SearchEntity = new ChldBMEntity(true);
                SearchEntity.ChldBMAgency = Agency;
                SearchEntity.chldBMDept = Depart;
                SearchEntity.ChldBMProg = Program;
                if (((Captain.Common.Utilities.ListItem)cmbBus.SelectedItem).Value.ToString().Trim() != "**")
                    SearchEntity.ChldBMNumber = ((Captain.Common.Utilities.ListItem)cmbBus.SelectedItem).Value.ToString().Trim();
                BusList = _model.SPAdminData.Browse_ChldBM(SearchEntity, "Browse");

                List<BusRTEntity> RouteList = new List<BusRTEntity>();
                BusRTEntity Route_Search = new BusRTEntity(true);
                Route_Search.BUSRT_AGENCY = Agency;
                Route_Search.BUSRT_DEPT = Depart;
                Route_Search.BUSRT_PROGRAM = Program;
                if (((Captain.Common.Utilities.ListItem)cmbBus.SelectedItem).Value.ToString().Trim() != "**")
                    Route_Search.BUSRT_NUMBER = ((Captain.Common.Utilities.ListItem)cmbBus.SelectedItem).Value.ToString().Trim();
                if (CmbYear.Visible == true)
                    Route_Search.BUSRT_YEAR = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString().Trim();
                RouteList = _model.SPAdminData.Browse_ChldBUSR(Route_Search, "Browse");

                List<BUSCEntity> Applist = new List<BUSCEntity>();
                BUSCEntity Chld_Search = new BUSCEntity(true);
                Chld_Search.BUSC_AGENCY = Agency; Chld_Search.BUSC_DEPT = Depart; Chld_Search.BUSC_PROG = Program;
                if (CmbYear.Visible == true)
                    Chld_Search.BUSC_YEAR = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString().Trim();
                if (((Captain.Common.Utilities.ListItem)cmbBus.SelectedItem).Value.ToString().Trim() != "**")
                    Chld_Search.BUSC_NUMBER = ((Captain.Common.Utilities.ListItem)cmbBus.SelectedItem).Value.ToString().Trim();
                if (((Captain.Common.Utilities.ListItem)cmbRoute.SelectedItem).Value.ToString().Trim() != "**")
                    Chld_Search.BUSC_ROUTE = ((Captain.Common.Utilities.ListItem)cmbRoute.SelectedItem).Value.ToString().Trim();

                //Mst Details Table
                //DataSet dsCaseMST = DatabaseLayer.CaseSnpData.GetCaseMST(Agency, Depart, Program, strYear, BaseForm.BaseApplicationNo);
                //DataRow drCaseMST = dsCaseMST.Tables[0].Rows[0];

                //Snp details Table
                //DataSet dsCaseSNP = DatabaseLayer.CaseSnpData.GetCaseSnpDetails(Agency, Depart, Program, strYear, BaseForm.BaseApplicationNo, null);
                try
                {
                    PrintHeaderPage(document, writer);

                    
                    document.SetPageSize(iTextSharp.text.PageSize.LETTER.Rotate());
                    document.NewPage();

                    Y_Pos = 795;

                    PdfPTable BusList_Table = new PdfPTable(6);
                    BusList_Table.TotalWidth = 750f;
                    BusList_Table.WidthPercentage = 100;
                    BusList_Table.LockedWidth = true;
                    float[] widths = new float[] { 60f, 60f, 30f, 25f, 20f, 20f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                    BusList_Table.SetWidths(widths);
                    BusList_Table.HorizontalAlignment = Element.ALIGN_CENTER;


                    if (BusList.Count > 0)
                    {
                        foreach (ChldBMEntity Entity in BusList)
                        {

                            PdfPCell row1 = new PdfPCell(new Phrase("Bus#     " + Entity.ChldBMNumber.Trim() + " " + Entity.Desc.Trim() + " " + Entity.ChldBMYear.Trim() + " " + Entity.Make.Trim() + " " + Entity.ChldBM_Type.Trim() + " " + Entity.Location1.Trim(), Times));
                            row1.HorizontalAlignment = Element.ALIGN_LEFT;
                            row1.Colspan = 6;
                            row1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            BusList_Table.AddCell(row1);

                            PdfPCell row3 = new PdfPCell(new Phrase("Reg#:     " + Entity.Registration, Times));
                            row3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            row3.Colspan = 2;
                            row3.HorizontalAlignment = Element.ALIGN_LEFT;
                            BusList_Table.AddCell(row3);

                            PdfPCell row_Dt = new PdfPCell(new Phrase("Reg Exp :" + LookupDataAccess.Getdate(Entity.Registration_Date), Times));
                            row_Dt.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            row_Dt.HorizontalAlignment = Element.ALIGN_LEFT;
                            BusList_Table.AddCell(row_Dt);

                            if (Entity.OL == "O")
                            {
                                PdfPCell row4 = new PdfPCell(new Phrase("OWN", Times));
                                row4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                row4.Colspan = 3;
                                row4.HorizontalAlignment = Element.ALIGN_LEFT;
                                BusList_Table.AddCell(row4);
                            }
                            else
                            {
                                PdfPCell row4 = new PdfPCell(new Phrase("Lease", Times));
                                row4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                row4.HorizontalAlignment = Element.ALIGN_LEFT;
                                BusList_Table.AddCell(row4);

                                PdfPCell row5 = new PdfPCell(new Phrase(LookupDataAccess.Getdate(Entity.OL_Date), Times));
                                row5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                row5.HorizontalAlignment = Element.ALIGN_LEFT;
                                BusList_Table.AddCell(row5);

                                PdfPCell row6 = new PdfPCell(new Phrase(Entity.OL_ID, Times));
                                row6.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                row6.HorizontalAlignment = Element.ALIGN_LEFT;
                                BusList_Table.AddCell(row6);


                            }
                            int Row_Cnt = 1; int App_Cnt = 0;
                            if (RouteList.Count > 0)
                            {
                                PdfPCell Space = new PdfPCell(new Phrase("", Times));
                                Space.Border = iTextSharp.text.Rectangle.TOP_BORDER;
                                Space.HorizontalAlignment = Element.ALIGN_LEFT;
                                Space.Colspan = 6;
                                BusList_Table.AddCell(Space);

                                PdfPCell Space_1 = new PdfPCell(new Phrase("", Times));
                                Space_1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                Space_1.HorizontalAlignment = Element.ALIGN_LEFT;
                                Space_1.Colspan = 6;
                                BusList_Table.AddCell(Space_1);

                                int Route_Cnt = 1;
                                foreach (BusRTEntity REntity in RouteList)
                                {
                                    if (Entity.ChldBMNumber.Trim() == REntity.BUSRT_NUMBER.Trim())
                                    {
                                        PdfPCell Route1 = new PdfPCell(new Phrase("Route:    " + REntity.BUSRT_ROUTE.Trim() + " " + REntity.BUSRT_AREA_SERVED.Trim(), Times));
                                        Route1.HorizontalAlignment = Element.ALIGN_LEFT;
                                        Route1.Colspan = 6;
                                        Route1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        BusList_Table.AddCell(Route1);

                                        //PdfPCell Route2 = new PdfPCell(new Phrase(REntity.BUSRT_ROUTE.Trim() + " " + REntity.BUSRT_AREA_SERVED.Trim(), Times));
                                        //Route2.HorizontalAlignment = Element.ALIGN_LEFT;
                                        //Route2.Colspan = 5;
                                        //Route2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        //BusList_Table.AddCell(Route2);

                                        PdfPCell Route_Driver1 = new PdfPCell(new Phrase("Driver:   " + REntity.BUSRT_DRIVER_NAME.Trim(), Times));
                                        Route_Driver1.HorizontalAlignment = Element.ALIGN_LEFT;
                                        Route_Driver1.Colspan = 2;
                                        Route_Driver1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        BusList_Table.AddCell(Route_Driver1);

                                        PdfPCell Route_Driver2 = new PdfPCell(new Phrase("DOB:  " + LookupDataAccess.Getdate(REntity.BUSRT_DRIVER_DOB.Trim()), Times));
                                        Route_Driver2.HorizontalAlignment = Element.ALIGN_LEFT;
                                        Route_Driver2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        BusList_Table.AddCell(Route_Driver2);

                                        PdfPCell Route_Driver3 = new PdfPCell(new Phrase("Lic:  " + REntity.BUSRT_DRIVER_LIC.Trim(), Times));
                                        Route_Driver3.HorizontalAlignment = Element.ALIGN_LEFT;
                                        Route_Driver3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        BusList_Table.AddCell(Route_Driver3);

                                        string DriverTelNo = string.Empty; 
                                        MaskedTextBox mskdphn = new MaskedTextBox();
                                        mskdphn.Mask = "999-000-0000";
                                        mskdphn.Text = REntity.BUSRT_DRIVER_TEL.Trim();

                                        PdfPCell Route_Driver4 = new PdfPCell(new Phrase("Tel#:  " + mskdphn.Text, Times));
                                        Route_Driver4.HorizontalAlignment = Element.ALIGN_LEFT;
                                        Route_Driver4.Colspan = 2;
                                        Route_Driver4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        BusList_Table.AddCell(Route_Driver4);
                                        string Lic_Dt = string.Empty; string sevDt = string.Empty; string CDL_dt = string.Empty; string DPU_dt = string.Empty;
                                        if (string.IsNullOrEmpty(REntity.BUSRT_DRIVER_LIC_DATE.Trim()))
                                            Lic_Dt = "          ";
                                        else Lic_Dt = LookupDataAccess.Getdate(REntity.BUSRT_DRIVER_LIC_DATE);
                                        if (string.IsNullOrEmpty(REntity.BUSRT_DRIVER_LIC_7D_DATE.Trim())) sevDt = "          ";
                                        else sevDt = LookupDataAccess.Getdate(REntity.BUSRT_DRIVER_LIC_7D_DATE);
                                        if (string.IsNullOrEmpty(REntity.BUSRT_DRIVER_LIC_CLD_DATE.Trim())) CDL_dt = "          ";
                                        else CDL_dt = LookupDataAccess.Getdate(REntity.BUSRT_DRIVER_LIC_CLD_DATE);
                                        if (string.IsNullOrEmpty(REntity.BUSRT_DRIVER_LIC_DPU_DATE.Trim())) DPU_dt = "          ";
                                        else DPU_dt = LookupDataAccess.Getdate(REntity.BUSRT_DRIVER_LIC_DPU_DATE);

                                        PdfPCell Route_Dt1 = new PdfPCell(new Phrase("Lic Date: " + Lic_Dt + "  " + "       7D Date:  " + sevDt + "  " + "      CDL Date:  " + CDL_dt + "  " + "        DPU Date:  " + DPU_dt, Times));
                                        Route_Dt1.HorizontalAlignment = Element.ALIGN_LEFT;
                                        Route_Dt1.Colspan = 6;
                                        Route_Dt1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        BusList_Table.AddCell(Route_Dt1);

                                        string Pickup = string.Empty; string Arrive = string.Empty; string Leave = string.Empty;
                                        if (string.IsNullOrEmpty(REntity.BUSRT_PICKUP_STARTS.Trim())) Pickup = "          ";
                                        else Pickup = Convert.ToDateTime(REntity.BUSRT_PICKUP_STARTS.ToString().Trim()).ToString("t");
                                        if (string.IsNullOrEmpty(REntity.BUSRT_ARRIVE_SCHOOL.Trim())) Arrive = "          ";
                                        else Arrive = Convert.ToDateTime(REntity.BUSRT_ARRIVE_SCHOOL.Trim()).ToString("t");
                                        if (string.IsNullOrEmpty(REntity.BUSRT_LEAVES_SCHOOL.Trim())) Leave = "          ";
                                        else Leave = Convert.ToDateTime(REntity.BUSRT_LEAVES_SCHOOL.Trim()).ToString("t");

                                        PdfPCell Route_Time1 = new PdfPCell(new Phrase("Start Pickups:" + Pickup + "     Arrive School: " + Arrive + "     Leaves School: " + Leave, Times));
                                        Route_Time1.HorizontalAlignment = Element.ALIGN_LEFT;
                                        Route_Time1.Colspan = 6;
                                        Route_Time1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        BusList_Table.AddCell(Route_Time1);


                                        if (rbChild.Checked.Equals(true))
                                        {
                                            DataSet dsApp = DatabaseLayer.SPAdminDB.Get_Appdet(REntity.BUSRT_AGENCY, REntity.BUSRT_DEPT, REntity.BUSRT_PROGRAM, REntity.BUSRT_YEAR, REntity.BUSRT_NUMBER, REntity.BUSRT_ROUTE);
                                            DataTable dtApp = dsApp.Tables[0];
                                            App_Cnt = dtApp.Rows.Count;
                                            if (dtApp.Rows.Count > 0)
                                            {
                                                DataView dv = dtApp.DefaultView;
                                                string Sort_Value = string.Empty;
                                                if (((Captain.Common.Utilities.ListItem)cmbSort.SelectedItem).Value.ToString() == "01")
                                                { dv.Sort = "BUSC_PICKUP"; }
                                                else if (((Captain.Common.Utilities.ListItem)cmbSort.SelectedItem).Value.ToString() == "02") { dv.Sort = "BUSC_HOME"; }
                                                else if (((Captain.Common.Utilities.ListItem)cmbSort.SelectedItem).Value.ToString() == "03") { dv.Sort = "NAME"; }
                                                else if (((Captain.Common.Utilities.ListItem)cmbSort.SelectedItem).Value.ToString() == "04") { dv.Sort = "MST_CITY,MST_STREET,MST_SUFFIX,MST_HN,MST_APT,MST_FLR"; }
                                                dtApp = dv.ToTable();
                                                //DataView dv1 = new DataView(dtApp);
                                                //DataTable dtcount = dv1.ToTable(true, "MST_APP_NO");

                                                PdfPCell AppSpace1 = new PdfPCell(new Phrase("", Times));
                                                AppSpace1.HorizontalAlignment = Element.ALIGN_LEFT;
                                                AppSpace1.Colspan = 6;
                                                AppSpace1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                BusList_Table.AddCell(AppSpace1);

                                                PdfPCell App1 = new PdfPCell(new Phrase("Name", TableFontBoldItalic));
                                                App1.HorizontalAlignment = Element.ALIGN_LEFT;
                                                //App1.Border = iTextSharp.text.Rectangle.BOX;
                                                App1.Border = iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.LEFT_BORDER + iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                                BusList_Table.AddCell(App1);

                                                PdfPCell App2 = new PdfPCell(new Phrase("Address", TableFontBoldItalic));
                                                App2.HorizontalAlignment = Element.ALIGN_LEFT;
                                                App2.Border = iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.BOTTOM_BORDER;//iTextSharp.text.Rectangle.BOX;
                                                BusList_Table.AddCell(App2);
                                                PdfPCell App3 = new PdfPCell(new Phrase("Class", TableFontBoldItalic));
                                                App3.HorizontalAlignment = Element.ALIGN_LEFT;
                                                App3.Border = iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.BOTTOM_BORDER;//iTextSharp.text.Rectangle.BOX;
                                                BusList_Table.AddCell(App3);
                                                PdfPCell App4 = new PdfPCell(new Phrase("Tel#", TableFontBoldItalic));
                                                App4.HorizontalAlignment = Element.ALIGN_LEFT;
                                                App4.Border = iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.BOTTOM_BORDER;//iTextSharp.text.Rectangle.BOX;
                                                BusList_Table.AddCell(App4);
                                                PdfPCell App5 = new PdfPCell(new Phrase("Pickup", TableFontBoldItalic));
                                                App5.HorizontalAlignment = Element.ALIGN_LEFT;
                                                App5.Border = iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.BOTTOM_BORDER;//iTextSharp.text.Rectangle.BOX;
                                                BusList_Table.AddCell(App5);
                                                PdfPCell App6 = new PdfPCell(new Phrase("Dropoff", TableFontBoldItalic));
                                                App6.HorizontalAlignment = Element.ALIGN_LEFT;
                                                App6.Border = iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER + iTextSharp.text.Rectangle.BOTTOM_BORDER;//iTextSharp.text.Rectangle.BOX;
                                                BusList_Table.AddCell(App6);
                                                int App_Row_cnt = 1;
                                                //Row_Cnt = dtApp.Rows.Count;
                                                string PrivApp = string.Empty;
                                                foreach (DataRow drApp in dtApp.Rows)
                                                {
                                                    string Pickupinfo = string.Empty, DropofInfo = string.Empty; bool pd = false;
                                                    if (drApp["MST_APP_NO"].ToString().Trim() != PrivApp)
                                                    {
                                                        Pickupinfo = drApp["CHLDMST_PICKUP"].ToString().Trim();
                                                        DropofInfo = drApp["CHLDMST_DROPOFF"].ToString().Trim();
                                                        if (string.IsNullOrEmpty(Pickupinfo.Trim()) && string.IsNullOrEmpty(DropofInfo.Trim()))
                                                            pd = true;

                                                        PdfPCell App1_det = new PdfPCell(new Phrase(drApp["NAME"].ToString().Trim(), Times));
                                                        App1_det.HorizontalAlignment = Element.ALIGN_LEFT;
                                                        if (App_Row_cnt == dtApp.Rows.Count && chkbSkip.Checked.Equals(false) && pd)
                                                            App1_det.Border = iTextSharp.text.Rectangle.LEFT_BORDER + iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                                        else App1_det.Border = iTextSharp.text.Rectangle.LEFT_BORDER; //+ iTextSharp.text.Rectangle.RIGHT_BORDER;
                                                        BusList_Table.AddCell(App1_det);

                                                        string Apt = string.Empty; string Floor = string.Empty; string HN = string.Empty; string Suffix = string.Empty; string Street = string.Empty;
                                                        string Zip = string.Empty;
                                                        if (!string.IsNullOrEmpty(drApp["MST_APT"].ToString().Trim()))
                                                            Apt = "Apt  " + drApp["MST_APT"].ToString().Trim() + " ";
                                                        if (!string.IsNullOrEmpty(drApp["MST_FLR"].ToString().Trim()))
                                                            Floor = "Flr  " + drApp["MST_FLR"].ToString().Trim() + " ";
                                                        if (!string.IsNullOrEmpty(drApp["MST_STREET"].ToString().Trim()))
                                                            Street = drApp["MST_STREET"].ToString().Trim() + " ";
                                                        if (!string.IsNullOrEmpty(drApp["MST_SUFFIX"].ToString().Trim()))
                                                            Suffix = drApp["MST_SUFFIX"].ToString().Trim() + "  ";
                                                        if (!string.IsNullOrEmpty(drApp["MST_HN"].ToString().Trim()))
                                                            HN = drApp["MST_HN"].ToString().Trim() + " ";
                                                        //if (!string.IsNullOrEmpty(drApp["MST_ZIP"].ToString().Trim()) && drApp["MST_ZIP"].ToString() != "0")
                                                        //    Zip = "00000".Substring(0, 5 - drApp["MST_ZIP"].ToString().Trim().Length) + drApp["MST_ZIP"].ToString().Trim();

                                                        string Address = HN + Street + Suffix + Apt + Floor + drApp["MST_CITY"].ToString().Trim();// +", " + drApp["MST_STATE"].ToString().Trim();// +" " + Zip;

                                                        PdfPCell App2_det = new PdfPCell(new Phrase(Address, Times));
                                                        App2_det.HorizontalAlignment = Element.ALIGN_LEFT;
                                                        if (chkbSkip.Checked.Equals(false) && App_Row_cnt == dtApp.Rows.Count && pd)
                                                            App2_det.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                                        else App2_det.Border = iTextSharp.text.Rectangle.NO_BORDER;// +iTextSharp.text.Rectangle.LEFT_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER;
                                                        BusList_Table.AddCell(App2_det);

                                                        if (!string.IsNullOrEmpty(drApp["ENRL_SITE"].ToString().Trim() + drApp["ENRL_ROOM"].ToString().Trim() + drApp["ENRL_AMPM"].ToString().Trim()))
                                                        {
                                                            PdfPCell App3_det = new PdfPCell(new Phrase(drApp["ENRL_SITE"].ToString().Trim() + "/" + drApp["ENRL_ROOM"].ToString().Trim() + "/" + drApp["ENRL_AMPM"].ToString().Trim(), Times));
                                                            App3_det.HorizontalAlignment = Element.ALIGN_LEFT;
                                                            if (chkbSkip.Checked.Equals(false) && App_Row_cnt == dtApp.Rows.Count && pd)
                                                                App3_det.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                                            else App3_det.Border = iTextSharp.text.Rectangle.NO_BORDER;// +iTextSharp.text.Rectangle.LEFT_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER;
                                                            BusList_Table.AddCell(App3_det);
                                                        }
                                                        else
                                                        {
                                                            PdfPCell App3_det = new PdfPCell(new Phrase("", Times));
                                                            App3_det.HorizontalAlignment = Element.ALIGN_LEFT;
                                                            if (chkbSkip.Checked.Equals(false) && App_Row_cnt == dtApp.Rows.Count && pd)
                                                                App3_det.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                                            else App3_det.Border = iTextSharp.text.Rectangle.NO_BORDER;// +iTextSharp.text.Rectangle.LEFT_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER;
                                                            BusList_Table.AddCell(App3_det);
                                                        }

                                                        MaskedTextBox mskphn = new MaskedTextBox();
                                                        mskphn.Mask = "999-000-0000";
                                                        mskphn.Text = drApp["TEL#"].ToString().Trim();

                                                        PdfPCell App4_det = new PdfPCell(new Phrase(mskphn.Text, Times));
                                                        App4_det.HorizontalAlignment = Element.ALIGN_LEFT;
                                                        if (chkbSkip.Checked.Equals(false) && App_Row_cnt == dtApp.Rows.Count && pd)
                                                            App4_det.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                                        else App4_det.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        BusList_Table.AddCell(App4_det);
                                                        if (!string.IsNullOrEmpty(drApp["BUSC_PICKUP"].ToString().Trim()))
                                                        {
                                                            PdfPCell App5_det = new PdfPCell(new Phrase(Convert.ToDateTime(drApp["BUSC_PICKUP"].ToString().Trim()).ToString("t"), Times));
                                                            App5_det.HorizontalAlignment = Element.ALIGN_LEFT;
                                                            if (chkbSkip.Checked.Equals(false) && App_Row_cnt == dtApp.Rows.Count && pd)
                                                                App5_det.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                                            else App5_det.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                            BusList_Table.AddCell(App5_det);
                                                        }
                                                        else
                                                        {
                                                            PdfPCell App5_det = new PdfPCell(new Phrase(drApp["BUSC_PICKUP"].ToString().Trim(), Times));
                                                            App5_det.HorizontalAlignment = Element.ALIGN_LEFT;
                                                            if (chkbSkip.Checked.Equals(false) && App_Row_cnt == dtApp.Rows.Count && pd)
                                                                App5_det.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                                            else App5_det.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                            BusList_Table.AddCell(App5_det);
                                                        }

                                                        if (!string.IsNullOrEmpty(drApp["BUSC_HOME"].ToString().Trim()))
                                                        {
                                                            PdfPCell App6_det = new PdfPCell(new Phrase(Convert.ToDateTime(drApp["BUSC_HOME"].ToString().Trim()).ToString("t"), Times));
                                                            App6_det.HorizontalAlignment = Element.ALIGN_LEFT;
                                                            if (chkbSkip.Checked.Equals(false) && App_Row_cnt == dtApp.Rows.Count && pd)
                                                                App6_det.Border = iTextSharp.text.Rectangle.RIGHT_BORDER + iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                                            else App6_det.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                                            BusList_Table.AddCell(App6_det);
                                                        }
                                                        else
                                                        {
                                                            PdfPCell App6_det = new PdfPCell(new Phrase(drApp["BUSC_HOME"].ToString().Trim(), Times));
                                                            App6_det.HorizontalAlignment = Element.ALIGN_LEFT;
                                                            if (chkbSkip.Checked.Equals(false) && App_Row_cnt == dtApp.Rows.Count && pd)
                                                                App6_det.Border = iTextSharp.text.Rectangle.RIGHT_BORDER + iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                                            else App6_det.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                                            BusList_Table.AddCell(App6_det);
                                                        }

                                                        if (!pd)
                                                        {
                                                            if (!string.IsNullOrEmpty(Pickupinfo.Trim()))
                                                            {
                                                                PdfPCell S1 = new PdfPCell(new Phrase("", Times));
                                                                S1.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                S1.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                                                BusList_Table.AddCell(S1);

                                                                PdfPCell S2 = new PdfPCell(new Phrase("Pickup : " + Pickupinfo.Trim(), Times));
                                                                S2.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                S2.Colspan = 5;
                                                                S2.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                                                BusList_Table.AddCell(S2);

                                                                //PdfPCell S3 = new PdfPCell(new Phrase("" , Times));
                                                                //S3.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                //S3.Colspan = 4;
                                                                //S3.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                                                //BusList_Table.AddCell(S3);
                                                            }
                                                            if (!string.IsNullOrEmpty(DropofInfo.Trim()))
                                                            {
                                                                PdfPCell S1 = new PdfPCell(new Phrase("", Times));
                                                                S1.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                S1.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                                                BusList_Table.AddCell(S1);

                                                                PdfPCell S2 = new PdfPCell(new Phrase("Dropoff : " + DropofInfo.Trim(), Times));
                                                                S2.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                S2.Colspan = 5;
                                                                S2.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                                                BusList_Table.AddCell(S2);

                                                                //PdfPCell S3 = new PdfPCell(new Phrase("", Times));
                                                                //S3.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                //S3.Colspan = 4;
                                                                //S3.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                                                //BusList_Table.AddCell(S3);
                                                            }
                                                            if (chkbSkip.Checked.Equals(false) && App_Row_cnt == dtApp.Rows.Count)
                                                            {
                                                                PdfPCell S4 = new PdfPCell(new Phrase("", Times));
                                                                S4.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                S4.Colspan = 6;
                                                                S4.Border = iTextSharp.text.Rectangle.TOP_BORDER;
                                                                BusList_Table.AddCell(S4);
                                                            }
                                                        }

                                                        if (chkbSkip.Checked.Equals(true))
                                                        {
                                                            PdfPCell Skip_Row = new PdfPCell(new Phrase("", Times));
                                                            Skip_Row.HorizontalAlignment = Element.ALIGN_LEFT;
                                                            Skip_Row.FixedHeight = 16f;
                                                            Skip_Row.Colspan = 6;
                                                            if (App_Row_cnt == dtApp.Rows.Count)
                                                                Skip_Row.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.LEFT_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER;
                                                            else
                                                                Skip_Row.Border = iTextSharp.text.Rectangle.LEFT_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER;
                                                            BusList_Table.AddCell(Skip_Row);
                                                        }

                                                        Row_Cnt++; //App_Row_cnt++;
                                                        PrivApp = drApp["MST_APP_NO"].ToString().Trim();
                                                    }
                                                    App_Row_cnt++;
                                                }
                                            }
                                        }

                                        PdfPCell RouteSpace = new PdfPCell(new Phrase("", Times));
                                        RouteSpace.HorizontalAlignment = Element.ALIGN_LEFT;
                                        RouteSpace.Colspan = 6;
                                        RouteSpace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        BusList_Table.AddCell(RouteSpace);

                                        PdfPCell RouteSpace1 = new PdfPCell(new Phrase("", Times));
                                        RouteSpace1.HorizontalAlignment = Element.ALIGN_LEFT;
                                        RouteSpace1.Colspan = 6;
                                        RouteSpace1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        BusList_Table.AddCell(RouteSpace1);

                                        if (rbChild.Checked.Equals(true))
                                        {
                                            document.Add(BusList_Table);
                                            BusList_Table.DeleteBodyRows();
                                            document.NewPage();

                                            List<BusRTEntity> RouteList_Cnt = new List<BusRTEntity>();
                                            BusRTEntity Route_Search_Cnt = new BusRTEntity(true);
                                            Route_Search_Cnt.BUSRT_AGENCY = Entity.ChldBMAgency;
                                            Route_Search_Cnt.BUSRT_DEPT = Entity.chldBMDept;
                                            Route_Search_Cnt.BUSRT_PROGRAM = Entity.ChldBMProg;
                                            Route_Search_Cnt.BUSRT_NUMBER = Entity.ChldBMNumber;
                                            if (CmbYear.Visible == true)
                                                Route_Search_Cnt.BUSRT_YEAR = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString().Trim();
                                            RouteList_Cnt = _model.SPAdminData.Browse_ChldBUSR(Route_Search_Cnt, "Browse");

                                            if (RouteList_Cnt.Count > Route_Cnt)
                                            {
                                                PdfPCell row12 = new PdfPCell(new Phrase("Bus#     " + Entity.ChldBMNumber.Trim() + " " + Entity.Desc.Trim() + " " + Entity.ChldBMYear.Trim() + " " + Entity.Make.Trim() + " " + Entity.ChldBM_Type.Trim() + " " + Entity.Location1.Trim(), Times));
                                                row12.HorizontalAlignment = Element.ALIGN_LEFT;
                                                row12.Colspan = 6;
                                                row12.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                BusList_Table.AddCell(row12);

                                                PdfPCell row31 = new PdfPCell(new Phrase("Reg#:     " + Entity.Registration, Times));
                                                row31.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                row31.Colspan = 2;
                                                row31.HorizontalAlignment = Element.ALIGN_LEFT;
                                                BusList_Table.AddCell(row31);

                                                PdfPCell row_Dt2 = new PdfPCell(new Phrase("Reg Exp :" + LookupDataAccess.Getdate(Entity.Registration_Date), Times));
                                                row_Dt2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                row_Dt2.HorizontalAlignment = Element.ALIGN_LEFT;
                                                BusList_Table.AddCell(row_Dt2);

                                                if (Entity.OL == "O")
                                                {
                                                    PdfPCell row42 = new PdfPCell(new Phrase("OWN", Times));
                                                    row42.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                    row42.Colspan = 3;
                                                    row42.HorizontalAlignment = Element.ALIGN_LEFT;
                                                    BusList_Table.AddCell(row42);
                                                }
                                                else
                                                {
                                                    PdfPCell row42 = new PdfPCell(new Phrase("Lease", Times));
                                                    row42.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                    row42.HorizontalAlignment = Element.ALIGN_LEFT;
                                                    BusList_Table.AddCell(row42);

                                                    PdfPCell row52 = new PdfPCell(new Phrase(LookupDataAccess.Getdate(Entity.OL_Date), Times));
                                                    row52.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                    row52.HorizontalAlignment = Element.ALIGN_LEFT;
                                                    BusList_Table.AddCell(row52);

                                                    PdfPCell row62 = new PdfPCell(new Phrase(Entity.OL_ID, Times));
                                                    row62.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                    row62.HorizontalAlignment = Element.ALIGN_LEFT;
                                                    BusList_Table.AddCell(row62);

                                                }
                                                PdfPCell Space2 = new PdfPCell(new Phrase("", Times));
                                                Space2.Border = iTextSharp.text.Rectangle.TOP_BORDER;
                                                Space2.HorizontalAlignment = Element.ALIGN_LEFT;
                                                Space2.Colspan = 6;
                                                BusList_Table.AddCell(Space2);

                                                PdfPCell Space3 = new PdfPCell(new Phrase("", Times));
                                                Space3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                Space3.HorizontalAlignment = Element.ALIGN_LEFT;
                                                Space3.Colspan = 6;
                                                BusList_Table.AddCell(Space3);
                                            }
                                        }
                                        Route_Cnt++;
                                    }


                                }

                            }
                            if (rbMaster.Checked.Equals(true))
                            {
                                document.Add(BusList_Table);
                                BusList_Table.DeleteBodyRows();
                                document.NewPage();
                            }

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


        private void PrintHeaderPage(Document document, PdfWriter writer)
        {
            /*BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/calibrib.ttf", BaseFont.WINANSI, BaseFont.EMBEDDED);
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

            PdfPCell row3 = new PdfPCell(new Phrase("Selected Report Parameters", TblHeaderTitleFont));
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


            PdfPCell Hierarchy = new PdfPCell(new Phrase(Txt_HieDesc.Text.Trim() + "     " + Header_year, TableFont));
            Hierarchy.HorizontalAlignment = Element.ALIGN_LEFT;
            Hierarchy.Colspan = 2;
            Hierarchy.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Headertable.AddCell(Hierarchy);*/

            string Agy = /*"Agency : */"All"; string Dept = /*"Dept : */"All"; string Prg = /*"Program : */"All"; string Header_year = string.Empty;
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

            PdfPCell R1 = new PdfPCell(new Phrase("  " + "Report Type"/* + (rbMaster.Checked == true ? rbMaster.Text : rbChild.Text)*/, TableFont));
            R1.HorizontalAlignment = Element.ALIGN_LEFT;
            R1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R1.PaddingBottom = 5;
            Headertable.AddCell(R1);

            PdfPCell R11 = new PdfPCell(new Phrase((rbMaster.Checked == true ? rbMaster.Text : rbChild.Text), TableFont));
            R11.HorizontalAlignment = Element.ALIGN_LEFT;
            R11.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R11.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R11.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R11.PaddingBottom = 5;
            Headertable.AddCell(R11);

            string Bus =string.Empty; string Route=string.Empty;
            if (((Captain.Common.Utilities.ListItem)cmbBus.SelectedItem).Value.ToString().Trim() == "**")
                Bus = "All";
            else Bus = ((Captain.Common.Utilities.ListItem)cmbBus.SelectedItem).Value.ToString().Trim();

            PdfPCell R2 = new PdfPCell(new Phrase("  " + "Bus Number" /*+ Bus*/, TableFont));
            R2.HorizontalAlignment = Element.ALIGN_LEFT;
            R2.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R2.PaddingBottom = 5;
            Headertable.AddCell(R2);

            PdfPCell R22 = new PdfPCell(new Phrase(Bus, TableFont));
            R22.HorizontalAlignment = Element.ALIGN_LEFT;
            R22.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R22.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R22.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R22.PaddingBottom = 5;
            Headertable.AddCell(R22);

            if (((Captain.Common.Utilities.ListItem)cmbRoute.SelectedItem).Value.ToString().Trim() == "**")
                Route = "All";
            else Route = ((Captain.Common.Utilities.ListItem)cmbRoute.SelectedItem).Value.ToString().Trim();

            PdfPCell R3 = new PdfPCell(new Phrase("  " + "Route Number" /*+ Route*/, TableFont));
            R3.HorizontalAlignment = Element.ALIGN_LEFT;
            R3.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R3.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R3.PaddingBottom = 5;
            Headertable.AddCell(R3);

            PdfPCell R33 = new PdfPCell(new Phrase(Route, TableFont));
            R33.HorizontalAlignment = Element.ALIGN_LEFT;
            R33.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R33.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R33.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R33.PaddingBottom = 5;
            Headertable.AddCell(R33);

            if (cmbSort.Visible == true)
            {
                PdfPCell R4 = new PdfPCell(new Phrase("  "  + "Sort by" /*+ ((Captain.Common.Utilities.ListItem)cmbSort.SelectedItem).Text.ToString()*/, TableFont));
                R4.HorizontalAlignment = Element.ALIGN_LEFT;
                R4.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                R4.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                R4.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                R4.PaddingBottom = 5;
                Headertable.AddCell(R4);

                PdfPCell R44 = new PdfPCell(new Phrase(((Captain.Common.Utilities.ListItem)cmbSort.SelectedItem).Text.ToString(), TableFont));
                R44.HorizontalAlignment = Element.ALIGN_LEFT;
                R44.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                R44.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                R44.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                R44.PaddingBottom = 5;
                Headertable.AddCell(R44);

                PdfPCell R5 = new PdfPCell(new Phrase("  " + "Skip Line", TableFont));
                R5.HorizontalAlignment = Element.ALIGN_LEFT;
                R5.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                R5.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                R5.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                R5.PaddingBottom = 5;
                Headertable.AddCell(R5);

                if (chkbSkip.Checked == true)
                {
                    PdfPCell R55 = new PdfPCell(new Phrase("Yes", TableFont));
                    R55.HorizontalAlignment = Element.ALIGN_LEFT;
                    R55.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    R55.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    R55.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                    R55.PaddingBottom = 5;
                    Headertable.AddCell(R55);

                }
                else
                {
                    PdfPCell R55 = new PdfPCell(new Phrase("No", TableFont));
                    R55.HorizontalAlignment = Element.ALIGN_LEFT;
                    R55.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    R55.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    R55.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                    R55.PaddingBottom = 5;
                    Headertable.AddCell(R55);
                }
            }

            document.Add(Headertable);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase("Generated By : ", fnttimesRoman_Italic), 33, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.GetMemberName(BaseForm.UserProfile.FirstName.Trim(), BaseForm.UserProfile.MI.Trim(), BaseForm.UserProfile.LastName.Trim(), "3"), fnttimesRoman_Italic), 90, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase("Generated On : ", fnttimesRoman_Italic), 410, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase(DateTime.Now.ToString(), fnttimesRoman_Italic), 468, 40, 0);

            //cb.BeginText();

            //cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ROMAN).BaseFont, 12);
            //cb.ShowTextAligned(100, "Program Name:", 30, 725, 0);
            //cb.ShowTextAligned(100, Privileges.Program + " - " + Privileges.PrivilegeName, 110, 725, 0);
            //cb.ShowTextAligned(100, "Run By :" + Privileges.UserID, 30, 705, 0);
            //cb.ShowTextAligned(100, "Module Desc :" + GetModuleDesc(), 30, 685, 0);
            //cb.ShowTextAligned(100, "Started : " + DateTime.Now.ToString(), 30, 665, 0);
            //cb.ShowTextAligned(100, "Report  Selection Criterion", 40, 635, 0);
            //string Header_year = string.Empty;
            //if (CmbYear.Visible == true)
            //    Header_year = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString();
            //cb.ShowTextAligned(100, "Hierarchy : " + Agency + "-" + Depart + "-" + Program + "   " + Header_year, 120, 610, 0);
            //string Report = "Master";
            //if (rbMaster.Checked.Equals(true)) Report = "Master"; else Report = "Condensed with Child";
            //cb.ShowTextAligned(100, "Report Type :" + Report, 120, 590, 0);
            //cb.ShowTextAligned(100, "Bus Number :" + ((Captain.Common.Utilities.ListItem)cmbBus.SelectedItem).Value.ToString() + "          " + "Route Number :" + ((Captain.Common.Utilities.ListItem)cmbRoute.SelectedItem).Value.ToString(), 120, 570, 0);
            ////cb.ShowTextAligned(100, "Route Number :"+ ((Captain.Common.Utilities.ListItem)cmbRoute.SelectedItem).Value.ToString(), 120, 570, 0);
            //if (cmbSort.Visible == true)
            //{
            //    cb.ShowTextAligned(100, "Sort By :" + ((Captain.Common.Utilities.ListItem)cmbSort.SelectedItem).Text.ToString(), 120, 550, 0);
            //    if(chkbSkip.Checked==true)
            //        cb.ShowTextAligned(100, chkbSkip.Text.Trim(), 300, 550, 0);
            //}

            //cb.SetFontAndSize(bfTimes, 12);
            //Y_Pos = 480;    // Y =  350

            //cb.EndText();

            //cb.SetLineWidth(0.7f);
            //cb.MoveTo(30f, 638f);
            //cb.LineTo(40f, 638f);

            //cb.LineTo(30f, 638f);
            //cb.LineTo(30f, 530f);

            //cb.LineTo(30f, 530f);
            //cb.LineTo(530f, 530f);

            //cb.LineTo(530f, 638f);
            //cb.LineTo(530f, 530f);

            //cb.MoveTo(170f, 638f);
            //cb.LineTo(530f, 638f);

            //cb.Stroke();
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


        private void rbChild_CheckedChanged(object sender, EventArgs e)
        {
            if (rbChild.Checked.Equals(true))
            {
                chkbSkip.Enabled = true;
                cmbSort.Enabled = true;
                pnlSkip.Visible = pnlSortBy.Visible = true;
                this.Size = new Size(752,294);
            }
            else
            {
                chkbSkip.Checked = false;
                cmbSort.SelectedIndex = 0;
                this.Size = new Size(752, 237);
                pnlSkip.Visible = pnlSortBy.Visible = false;
            }
        }

        private void HSSB0118_ReportForm_ToolClick(object sender, ToolClickEventArgs e)
        {
            Application.Navigate(CommonFunctions.CreateZenHelps(Privileges.Program, 0, BaseForm.BusinessModuleID.ToString(),"",""), target: "_blank");
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
            //_errorProvider.SetError(rbHeight, null);
            //_errorProvider.SetError(rbWeight, null);
            //_errorProvider.SetError(rdoMultipleSites, null);
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
            string Busno = ((Captain.Common.Utilities.ListItem)cmbBus.SelectedItem).Value.ToString();
            string RouteNo = ((Captain.Common.Utilities.ListItem)cmbRoute.SelectedItem).Value.ToString();
            string strReport = rbMaster.Checked == true ? "M" : "C";

            string strSort = string.Empty; string Pick = "N";
            if (rbChild.Checked == true)
            {
                strSort = ((Captain.Common.Utilities.ListItem)cmbSort.SelectedItem).Value.ToString();
                if (chkbSkip.Checked == true) Pick = "Y";
                
            }
            
            StringBuilder str = new StringBuilder();
            str.Append("<Rows>");
            str.Append("<Row AGENCY = \"" + Agency + "\" DEPT = \"" + Depart + "\" PROG = \"" + Program +
                            "\" YEAR = \"" + Program_Year + "\" ReportType = \"" + strReport + "\" Bus = \"" + Busno + "\" Route = \"" + RouteNo + "\" Sortby = \"" + strSort +
                            "\" PickUp = \"" + Pick + "\"  />");
            str.Append("</Rows>");

            return str.ToString();
        }

        private void Set_Report_Controls(DataTable Tmp_Table)
        {
            if (Tmp_Table != null && Tmp_Table.Rows.Count > 0)
            {
                DataRow dr = Tmp_Table.Rows[0];

                Set_Report_Hierarchy(dr["AGENCY"].ToString(), dr["DEPT"].ToString(), dr["PROG"].ToString(), dr["YEAR"].ToString());

                if (dr["ReportType"].ToString() == "M")
                    rbMaster.Checked = true;
                else
                {
                    rbChild.Checked = true;
                    CommonFunctions.SetComboBoxValue(cmbSort, dr["Sortby"].ToString());
                    if (dr["PickUp"].ToString() == "Y")
                        chkbSkip.Checked = true;
                    else chkbSkip.Checked = false;
                }

                CommonFunctions.SetComboBoxValue(cmbBus, dr["Bus"].ToString());
                CommonFunctions.SetComboBoxValue(cmbRoute, dr["Route"].ToString());

                

            }
        }

    }
}