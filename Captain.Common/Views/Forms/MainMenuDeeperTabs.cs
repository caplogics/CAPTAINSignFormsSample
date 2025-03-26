#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Collections;
//using Gizmox.WebGUI.Common;
using Wisej.Web;

using System.Web.Configuration;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Utilities;
using Captain.Common.Menus;
using System.Data.SqlClient;
using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
//using Gizmox.WebGUI.Common.Resources;
using Captain.Common.Views.UserControls.Base;
using Captain.Common.Exceptions;
using System.Diagnostics;
using Captain.Common.Views.Forms;
using System.IO;
using System.Linq;

using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Text.RegularExpressions;
//using Gizmox.WebGUI.Common.Gateways;
//using Gizmox.WebGUI.Common.Interfaces;
using System.Web;
using System.Runtime.InteropServices;
using Microsoft.Win32;


#endregion

namespace Captain.Common.Views.Forms
{
    public partial class MainMenuDeeperTabs : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        //private GridControl _intakeHierarchy = null;
        private bool boolChangeStatus = false;
        private CaptainModel _model = null;
        private List<HierarchyEntity> _selectedHierarchies = null;

        #endregion
        public MainMenuDeeperTabs(BaseForm baseform, string Agency, string Depart, string Program, string ProgramYear, string AppNo, string Selc, int SelCount)
        {
            InitializeComponent();

            _model = new CaptainModel();

            propAgencyControlDetails = _model.ZipCodeAndAgency.GetAgencyControlFile("00");
            propReportPath = _model.lookupDataAccess.GetReportPath();

            BaseForm = baseform;
            AGY = Agency; Dept = Depart; Prog = Program; Year = ProgramYear; App = AppNo;
            Selection = Selc; Count = SelCount;
            this.Text = "Client Inquiry Tabs";

            //Fill_Totals_Grid();
            AddTabs();
        }

        #region properties

        public BaseForm BaseForm { get; set; }


        public string AGY { get; set; }

        public string Dept { get; set; }
        public string Prog { get; set; }
        public string App { get; set; }
        public string Selection { get; set; }
        public int Count { get; set; }

        public PrivilegeEntity Priviliges { get; set; }


        public string Year { get; set; }


        public string AgencyName { get; set; }


        public string DeptName { get; set; }


        public string ProgramName { get; set; }


        public string ApplicationNo { get; set; }

        public char AddPriv { get; set; }

        public ProgramDefinitionEntity ProgramDefinition { get; set; }

        public AgencyControlEntity propAgencyControlDetails { get; set; }
        public string propReportPath { get; set; }
        List<Client_InqTotal_entity> Client_Tot_List = new List<Client_InqTotal_entity>();
        List<CaseMstEntity> CASEMSTList = new List<CaseMstEntity>();
        List<CaseSnpEntity> casesnpList = new List<CaseSnpEntity>();
        List<CaseIncomeEntity> incomelist = new List<CaseIncomeEntity>();
        List<CASESPMEntity> spmlist = new List<CASESPMEntity>();
        List<CASEACTEntity> actList = new List<CASEACTEntity>();
        List<CASEMSEntity> MSlist = new List<CASEMSEntity>();
        List<ACTREFSEntity> Actrefslist = new List<ACTREFSEntity>();
        List<CASECONTEntity> ContList = new List<CASECONTEntity>();
        List<MATASMTEntity> MATAList = new List<MATASMTEntity>();
        List<LIHEAPBEntity> LPBList = new List<LIHEAPBEntity>();
        List<PAYMNETEntity> PaymentList = new List<PAYMNETEntity>();
        List<EMSRESEntity> EMSRESList = new List<EMSRESEntity>();
        List<EMSCLCPMCEntity> EMSCLCList = new List<EMSCLCPMCEntity>();
        List<CASESP1Entity> SP1List = new List<CASESP1Entity>();
        List<CASEREFEntity> CASEREFREF_List = new List<CASEREFEntity>();
        List<MATDEFBMEntity> MATADEFBMentity = new List<MATDEFBMEntity>();
        List<EMSBDCEntity> propEmsbdc_List { get; set; }
        List<EMSCLAPMAEntity> propEMSCLAPMAList { get; set; }
        List<CommonEntity> propfundingsource { get; set; }
        CaseSnpEntity SelSnp = new CaseSnpEntity();
        DataTable dtSP1 = new DataTable(); DataSet dsSite = new DataSet();
        // Gizmox.WebGUI.Common.Resources.ResourceHandle Img_Blank = new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("Blank.JPG");
        // Gizmox.WebGUI.Common.Resources.ResourceHandle Img_Tick = new Gizmox.WebGUI.Common.Resources.ImageResourceHandle("tick.ico");
        private string strNameFormat = string.Empty;


        #endregion

        ProgramDefinitionEntity programEntity = new ProgramDefinitionEntity();
        private void Fill_Totals_Grid()
        {

            //string AGY = GvwAppHou.CurrentRow.Cells["Grd_Agy"].Value.ToString();
            //string Dept = GvwAppHou.CurrentRow.Cells["Grd_Dept"].Value.ToString();
            //string Prog = GvwAppHou.CurrentRow.Cells["Grd_Prog"].Value.ToString();
            //string Year = GvwAppHou.CurrentRow.Cells["Grd_Year"].Value.ToString();
            //string App = GvwAppHou.CurrentRow.Cells["Grd_App"].Value.ToString();

            //strAgency = AGY; strDept = Dept; strProgram = Prog; strYear = Year; strApp = App;

            string Acr_State = "";
            if (propAgencyControlDetails != null)
                Acr_State = propAgencyControlDetails.State;

            //Client_Tot_List = _model.AdhocData.Get_ClientInq_Totals(AGY, Dept, Prog, Year, App);
            programEntity = _model.HierarchyAndPrograms.GetCaseDepadp(AGY, Dept, Prog);

            DataSet ds = DatabaseLayer.SPAdminDB.Get_ClientInq_Totals(AGY, Dept, Prog, Year, App);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                        Client_Tot_List.Add(new Client_InqTotal_entity(row));
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[1].Rows)
                        CASEMSTList.Add(new CaseMstEntity(row));
                }
                if (ds.Tables[2].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[2].Rows)
                        casesnpList.Add(new CaseSnpEntity(row));
                }
                if (ds.Tables[3].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[3].Rows)
                        incomelist.Add(new CaseIncomeEntity(row, string.Empty));
                }
                if (ds.Tables[4].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[4].Rows)
                        spmlist.Add(new CASESPMEntity(row));
                }
                if (ds.Tables[5].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[5].Rows)
                        actList.Add(new CASEACTEntity(row));
                }
                if (ds.Tables[6].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[6].Rows)
                        MSlist.Add(new CASEMSEntity(row));
                }
                if (ds.Tables[7].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[7].Rows)
                        ContList.Add(new CASECONTEntity(row));
                }
                if (ds.Tables[8].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[8].Rows)
                        Actrefslist.Add(new ACTREFSEntity(row));
                }
                if (ds.Tables[9].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[9].Rows)
                        MATAList.Add(new MATASMTEntity(row, "Browse"));
                }
                if (ds.Tables[10].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[10].Rows)
                        LPBList.Add(new LIHEAPBEntity(row));
                }
                if (ds.Tables[11].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[11].Rows)
                        PaymentList.Add(new PAYMNETEntity(row));
                }

                if ((Acr_State == "TX"))
                {
                    if (ds.Tables[12].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[12].Rows)
                            EMSRESList.Add(new EMSRESEntity(row));
                    }
                    if (ds.Tables[13].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[13].Rows)
                            EMSCLCList.Add(new EMSCLCPMCEntity(row));
                    }

                    propEmsbdc_List = _model.EMSBDCData.GetEmsBdcAllData(AGY, Dept, Prog, Year, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                    propEMSCLAPMAList = _model.EMSBDCData.GetEmsclapmaAllData(AGY, Dept, Prog, Year, App, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

                }

                if (CASEMSTList.Count > 0 && casesnpList.Count > 0)
                    SelSnp = casesnpList.Find(u => u.App.Equals(CASEMSTList[0].ApplNo) && u.FamilySeq.Equals(CASEMSTList[0].FamilySeq));

            }

            //if (Client_Tot_List.Count > 0)
            //{
            //    TotalsGrid.Rows.Add(Img_Blank, "Intake", CASEMSTList.Count.ToString(), "0", "N");
            //    foreach (Client_InqTotal_entity ent in Client_Tot_List)
            //    {
            //        if (ent.Sno.Trim() == "9" || ent.Sno.Trim() == "10") //if (("9, 10").Contains(ent.Sno.Trim()))
            //        {
            //            if ((Acr_State == "TX"))
            //                TotalsGrid.Rows.Add(Img_Blank, ent.Total_Desc, ent.Total_Cnt, ent.Sno, "N");
            //        }
            //        else if (ent.Sno.Trim() == "1" || ent.Sno.Trim() == "2" || ent.Sno.Trim() == "3")
            //        {
            //            if ((Acr_State != "TX"))
            //                TotalsGrid.Rows.Add(Img_Blank, ent.Total_Desc, ent.Total_Cnt, ent.Sno, "N");
            //        }
            //        else
            //            TotalsGrid.Rows.Add(Img_Blank, ent.Total_Desc, ent.Total_Cnt, ent.Sno, "N");

            //        //if ((Acr_State == "TX" && ent.Sno.Contains("9, 10")))
            //        //    TotalsGrid.Rows.Add(ent.Total_Desc, ent.Total_Cnt);
            //    }
            //}


        }

        private void AddTabs()
        {
            if (!string.IsNullOrEmpty(Selection.Trim()))
            {
                string[] Seltab = Selection.Split(' ');
                if (Seltab != null)
                {
                    for (int i = 0; i < Seltab.Length; i++)
                    {
                        //if (Seltab[i] == "A")
                        //{
                        //    string title = "All" ;
                        //    TabPage myTabPage = new TabPage(title);
                        //    myTabPage.Tag = "A";
                        //    tabControl1.TabPages.Add(myTabPage);

                        //    On_SaveFormClosed(Selection);
                        //}
                        if (Seltab[i] == "I")
                        {
                            string title = "Client Intake";
                            TabPage myTabPage = new TabPage(title);
                            myTabPage.Tag = "I"; //myTabPage.Dock = Wisej.Web.DockStyle.Fill;
                            tabControl1.TabPages.Add(myTabPage);
                        }
                        if (Seltab[i] == "S")
                        {
                            string title = "Service Plans";
                            TabPage myTabPage = new TabPage(title);
                            myTabPage.Tag = "S"; //myTabPage.Dock = Wisej.Web.DockStyle.Fill;
                            tabControl1.TabPages.Add(myTabPage);
                        }
                        if (Seltab[i] == "C")
                        {
                            string title = "Contact Data";
                            TabPage myTabPage = new TabPage(title);
                            myTabPage.Tag = "C"; //myTabPage.Dock = Wisej.Web.DockStyle.Fill;
                            tabControl1.TabPages.Add(myTabPage);
                        }
                        if (Seltab[i] == "R")
                        {
                            string title = "REFER From/TO";
                            TabPage myTabPage = new TabPage(title);
                            myTabPage.Tag = "R"; //myTabPage.Dock = Wisej.Web.DockStyle.Fill;
                            tabControl1.TabPages.Add(myTabPage);
                        }
                        if (Seltab[i] == "M")
                        {
                            string title = "Full Assessment (Matrix)";
                            TabPage myTabPage = new TabPage(title);
                            myTabPage.Tag = "M"; //myTabPage.Dock = Wisej.Web.DockStyle.Fill;
                            tabControl1.TabPages.Add(myTabPage);
                        }
                        if (Seltab[i] == "L")
                        {
                            string title = "Fuel Assistance";
                            TabPage myTabPage = new TabPage(title);
                            myTabPage.Tag = "L"; //myTabPage.Dock = Wisej.Web.DockStyle.Fill;
                            tabControl1.TabPages.Add(myTabPage);
                        }
                        if (Seltab[i] == "E")
                        {
                            string title = "Emergency Services";
                            TabPage myTabPage = new TabPage(title);
                            myTabPage.Tag = "E"; //myTabPage.Dock = Wisej.Web.DockStyle.Fill;
                            tabControl1.TabPages.Add(myTabPage);
                        }

                    }
                    tabControl1.SelectedIndex = 0;
                    tabControl1_SelectedIndexChanged(tabControl1, new EventArgs());
                }
            }
        }

        PdfContentByte cb;
        int X_Pos, Y_Pos;
        string strFolderPath = string.Empty;
        string Random_Filename = null; string PdfName = "Pdf File";
        private void On_SaveFormClosed(string Selection)
        {
            Random_Filename = null;

            PdfName = Selection + BaseForm.BaseApplicationNo.ToString() + "Report";//form.GetFileName();
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

            Document document = new Document();
            document.SetPageSize(iTextSharp.text.PageSize.LETTER.Rotate());
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();
            BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            iTextSharp.text.Font helvetica = new iTextSharp.text.Font(bf_times, 12, 1);
            BaseFont bf_helv = helvetica.GetCalculatedBaseFont(false);
            iTextSharp.text.Font TimesUnderline = new iTextSharp.text.Font(bf_times, 9, 4);
            BaseFont bf_TimesUnderline = TimesUnderline.GetCalculatedBaseFont(true);

            iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 9);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 8);
            iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 8, 3);
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_times, 9, 1);
            iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 8, 2);
            iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 9, 4);
            cb = writer.DirectContent;


            string Acr_State = "";
            if (propAgencyControlDetails != null)
                Acr_State = propAgencyControlDetails.State;

            SP1List = new List<CASESP1Entity>();


            Fill_CaseWorker();
            Fill_Languages_List();
            Get_Vendor_Brose_List();
            dsSite = Captain.DatabaseLayer.CaseMst.GetSiteByHIE(AGY, string.Empty, string.Empty);
            CASEREFEntity Search_REF_Entity = new CASEREFEntity(true);
            CASEREFREF_List = _model.SPAdminData.Browse_CASEREF(Search_REF_Entity, "Browse");
            //SP1List = _model.SPAdminData.Browse_CASESP1(string.Empty, AGY, Dept, Prog);
            DataSet ds = Captain.DatabaseLayer.SPAdminDB.Browse_CASESP1(string.Empty, AGY, Dept, Prog);
            //DataTable dt = new DataTable();
            if (ds.Tables.Count > 0)
                dtSP1 = ds.Tables[0];

            propfundingsource = _model.lookupDataAccess.GetAgyFunds(); //CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, "00501", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg,Mode);

            propfundingsource = filterByHIE(propfundingsource, "View");
            Fill_CAMAST_List();
            Fill_MSMAST_List();

            //if(((Captain.Common.Utilities.ListItem)cmbSelect.SelectedItem).Value.ToString()=="A" || ((Captain.Common.Utilities.ListItem)cmbSelect.SelectedItem).Value.ToString()=="I")
            if (Selection.Contains("I") || Selection.Contains("A"))
                ClientIntakeForm(document, AGY, Dept, Prog, Year, App, cb, writer);

            if (Selection.Contains("S") || Selection.Contains("A")) //if (((Captain.Common.Utilities.ListItem)cmbSelect.SelectedItem).Value.ToString() == "A" || ((Captain.Common.Utilities.ListItem)cmbSelect.SelectedItem).Value.ToString() == "S")
            {
                if (spmlist.Count > 0)
                {
                    document.SetPageSize(iTextSharp.text.PageSize.A4);
                    if (Selection == "A")
                        document.NewPage();
                    CaseSpmPdf(document);
                }
            }

            if (Selection.Contains("C") || Selection.Contains("A"))//if (((Captain.Common.Utilities.ListItem)cmbSelect.SelectedItem).Value.ToString() == "A" || ((Captain.Common.Utilities.ListItem)cmbSelect.SelectedItem).Value.ToString() == "C")
            {
                if (ContList.Count > 0)
                {
                    document.SetPageSize(iTextSharp.text.PageSize.A4);
                    if (Selection == "A")
                        document.NewPage();
                    CaseCONTPdf(document);
                }
            }

            if (Selection.Contains("A") || Selection.Contains("R"))//if (((Captain.Common.Utilities.ListItem)cmbSelect.SelectedItem).Value.ToString() == "A" || ((Captain.Common.Utilities.ListItem)cmbSelect.SelectedItem).Value.ToString() == "R")
            {
                if (Actrefslist.Count > 0)
                {
                    document.SetPageSize(iTextSharp.text.PageSize.A4);
                    if (Selection == "A")
                        document.NewPage();
                    ACTREFSPdf(document);
                }
            }
            if (Selection.Contains("A") || Selection.Contains("M"))//if (((Captain.Common.Utilities.ListItem)cmbSelect.SelectedItem).Value.ToString() == "A" || ((Captain.Common.Utilities.ListItem)cmbSelect.SelectedItem).Value.ToString() == "M")
            {
                if (MATAList.Count > 0)
                {
                    document.SetPageSize(iTextSharp.text.PageSize.A4);
                    if (Selection == "A")
                        document.NewPage();
                    MATASMTPdf(document);
                }
            }


            if (Selection.Contains("A") || Selection.Contains("L"))//if (((Captain.Common.Utilities.ListItem)cmbSelect.SelectedItem).Value.ToString() == "A" || ((Captain.Common.Utilities.ListItem)cmbSelect.SelectedItem).Value.ToString() == "L")
            {
                if (LPBList.Count > 0)
                {
                    document.SetPageSize(iTextSharp.text.PageSize.A4);
                    if (Selection == "A")
                        document.NewPage();
                    LiheapList(document);
                }
            }

            if (Acr_State == "TX")
            {
                if (Selection.Contains("A") || Selection.Contains("E"))//if (((Captain.Common.Utilities.ListItem)cmbSelect.SelectedItem).Value.ToString() == "A" || ((Captain.Common.Utilities.ListItem)cmbSelect.SelectedItem).Value.ToString() == "E")
                {
                    if (EMSRESList.Count > 0)
                    {
                        document.SetPageSize(iTextSharp.text.PageSize.A4);
                        if (Selection == "A")
                            document.NewPage();
                        EMSRESPdf(document);
                    }
                }
            }

            if (Count == 0 && Selection != "A")
                document.Add(new Paragraph("No Records Found............................................... "));

            document.Close();
            fs.Close();
            fs.Dispose();

            //FrmViewer objfrm = new FrmViewer(PdfName);
            //objfrm.FormClosed += new Form.FormClosedEventHandler(On_Delete_PDF_File);
            //objfrm.ShowDialog();

            //frmViewer_Load();


        }

        private List<CommonEntity> filterByHIE(List<CommonEntity> LookupValues, string Mode)
        {
            string HIE = AGY + Dept + Prog;
            List<CommonEntity> _AgytabsFilter = new List<CommonEntity>();
            _AgytabsFilter = LookupValues;
            if (LookupValues.Count > 0)
            {

                if (Mode.ToUpper() == "ADD")
                {
                    _AgytabsFilter = _AgytabsFilter.FindAll(u => (u.ListHierarchy.Contains(HIE) || u.ListHierarchy.Contains(AGY + Dept + "**") || u.ListHierarchy.Contains(AGY + "****") || u.ListHierarchy.Contains("******")) && u.Active.ToString().ToUpper() == "Y").ToList();
                }
                else
                {
                    _AgytabsFilter = _AgytabsFilter.FindAll(u => u.ListHierarchy.Contains(HIE) || u.ListHierarchy.Contains(AGY + Dept + "**") || u.ListHierarchy.Contains(AGY + "****") || u.ListHierarchy.Contains("******")).ToList();
                }

                _AgytabsFilter = _AgytabsFilter.OrderByDescending(u => u.Active).ThenBy(u => u.Desc).ToList();
            }

            return _AgytabsFilter;
        }

        private void On_Delete_PDF_File(object sender, FormClosedEventArgs e)
        {
            System.IO.File.Delete(PdfName);
        }

        #region Casemanagement Application
        private void ClientIntakeForm(Document document, string AGY, string Dept, string Prog, string Year, string App, PdfContentByte cb, PdfWriter writer)
        {
            BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            iTextSharp.text.Font helvetica = new iTextSharp.text.Font(bf_times, 12, 1);
            BaseFont bf_helv = helvetica.GetCalculatedBaseFont(false);
            iTextSharp.text.Font TimesUnderline = new iTextSharp.text.Font(bf_times, 9, 4);
            BaseFont bf_TimesUnderline = TimesUnderline.GetCalculatedBaseFont(true);

            iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 9);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 8);
            iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 8, 3);
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_times, 9, 1);
            iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 8, 2);
            iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 9, 4);


            //Agency Control Table
            string Attention = string.Empty;
            DataSet ds = Captain.DatabaseLayer.ADMNB001DB.ADMNB001_Browse_AGCYCNTL("00", null, null, null, null, null, null);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
                Attention = ds.Tables[0].Rows[0]["ACR_03_ATTESTATION"].ToString().Trim();

            //Mst Details Table
            //DataSet dsCaseMST = DatabaseLayer.CaseSnpData.GetCaseMST(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo);
            //DataRow drCaseMST = dsCaseMST.Tables[0].Rows[0];

            ////Snp details Table
            //DataSet dsCaseSNP = DatabaseLayer.CaseSnpData.GetCaseSnpDetails(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, null);
            //if (dsCaseSNP.Tables.Count > 0)
            //    dtCaseSNP = dsCaseSNP.Tables[0];

            //Casesite Table
            List<CaseSiteEntity> SiteList = new List<CaseSiteEntity>();
            CaseSiteEntity Search_Site = new CaseSiteEntity(true);
            Search_Site.SiteAGENCY = AGY; Search_Site.SiteNUMBER = CASEMSTList[0].Site;
            Search_Site.SiteROOM = "0000";
            SiteList = _model.CaseMstData.Browse_CASESITE(Search_Site, "Browse");

            ////Services Table
            //List<CaseMSTSER> MSTSER_List = new List<CaseMSTSER>();
            //CaseMSTSER Search_MSTSER = new CaseMSTSER(true);
            //Search_MSTSER.Agency = BaseForm.BaseAgency; Search_MSTSER.Dept = BaseForm.BaseDept; Search_MSTSER.Program = BaseForm.BaseProg;
            //Search_MSTSER.Year = BaseForm.BaseYear; Search_MSTSER.AppNo = BaseForm.BaseApplicationNo;
            //MSTSER_List = _model.CaseMstData.Browse_MSTSER(Search_MSTSER, "Browse");

            ////AddCust Table
            //List<AddCustEntity> ADDCUST_List = new List<AddCustEntity>();
            //AddCustEntity Search_AddCust = new AddCustEntity(true);
            //Search_AddCust.ACTAGENCY = BaseForm.BaseAgency; Search_AddCust.ACTDEPT = BaseForm.BaseDept; Search_AddCust.ACTPROGRAM = BaseForm.BaseProg;
            //Search_AddCust.ACTYEAR = BaseForm.BaseYear; Search_AddCust.ACTAPPNO = BaseForm.BaseApplicationNo;
            //ADDCUST_List = _model.CaseMstData.Browse_ADDCUST(Search_AddCust, "Browse");

            ////CUSTFLDS Table for custom Questions
            //List<CustfldsEntity> custQues_List = new List<CustfldsEntity>();
            //CustfldsEntity Search_CustQues = new CustfldsEntity(true);
            //custQues_List = _model.SPAdminData.Browse_CUSTFLDS(Search_CustQues, "Browse");

            //List<CustRespEntity> custResp_List = new List<CustRespEntity>();
            //CustRespEntity Search_CustResp = new CustRespEntity(true);
            //Search_CustResp.ScrCode = "CASE2001";
            //custResp_List = _model.FieldControls.Browse_CUSTRESP(Search_CustResp, "Browse");

            //CaseHie Table
            DataSet dsCaseHie = DatabaseLayer.ADMNB001DB.ADMNB001_GetCashie("**-**-**", Dept, Prog);
            DataTable dtCaseHie = dsCaseHie.Tables[0];

            //Getting CaseWorker
            DataSet dsVerifier = DatabaseLayer.CaseMst.GetCaseWorker("I", AGY, Dept, Prog);
            DataTable dtVerifier = dsVerifier.Tables[0];

            ////CaseIncome Table
            //DataSet dsCaseIncome = DatabaseLayer.CaseMst.GetCASEINCOME(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo);
            //DataTable dtCaseIncome = dsCaseIncome.Tables[0];
            DataSet dsIncome = DatabaseLayer.Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.INCOMETYPES);

            DataSet dsCaseDiff = DatabaseLayer.CaseMst.GetCASEDiffadpya(AGY, Dept, Prog, Year, App);
            DataTable dtCasediff = dsCaseDiff.Tables[0];

            ////CHLDMST Table
            //ChldMstEntity chldMstDetails = _model.ChldMstData.GetChldMstDetails(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, string.Empty);

            ////CHLDEMER TABLE
            //List<ChldMstEMEMEntitty> chldEmemDetails = _model.ChldMstData.GetChldEmemList(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, string.Empty);
            //CaseCondEntitty caseconddet = _model.ChldMstData.GetCaseCondDetails(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, string.Empty);

            AGYTABSEntity searchAgytabs = new AGYTABSEntity(true);
            searchAgytabs.Tabs_Type = "S0060";  //List<AGYTABSEntity> TransportList = AgyTabs_List.FindAll(u => u.Tabs_Type.ToString().Trim().Equals("S0041"));
            List<AGYTABSEntity> AgyTabs_List = _model.AdhocData.Browse_AGYTABS(searchAgytabs);

            DataSet Relations = DatabaseLayer.AgyTab.GetAgyTabDetails(Consts.AgyTab.RELATIONSHIP);
            //DataTable dtrelation = Relations.Tables[0];
            List<CommonEntity> commonEntity = new List<CommonEntity>();
            if (Relations != null && Relations.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in Relations.Tables[0].Rows)
                    commonEntity.Add(new CommonEntity(dr["AGY_1"].ToString(), dr["Agy_8"].ToString(), dr["AGY_2"].ToString()));
            }

            CommonEntity MotherEntity = new CommonEntity(); List<CommonEntity> FatherEntity = new List<CommonEntity>();
            if (commonEntity.Count > 0)
            {
                MotherEntity = commonEntity.Find(u => u.Hierarchy.Equals("G1"));
                FatherEntity = commonEntity.FindAll(u => u.Hierarchy.Equals("G2"));
            }

            List<CommonEntity> lookInsuranceCategory = _model.lookupDataAccess.GetInsuranceCategory();

            DataSet dsFUND = DatabaseLayer.Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.CASEMNGMTFUNDSRC, "A");
            DataTable dtFUND = dsFUND.Tables[0];

            cb.BeginText();
            X_Pos = 400; Y_Pos = 580;
            cb.SetFontAndSize(bf_helv, 13);
            //cb.SetColorFill(BaseColor.BLUE.Darker());
            string Header_Desc = string.Empty; string Form_Selection = string.Empty;

            //if (Privileges.ModuleCode == "03")
            //{
            string ShortName = string.Empty;
            string AgencyName = string.Empty; string SerHie = "N";

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                ShortName = ds.Tables[0].Rows[0]["ACR_SHORT_NAME"].ToString().Trim();
                if (ds.Tables[0].Rows[0]["ACR_SERVINQ_CASEHIE"].ToString().Trim() == "1") SerHie = "Y"; else SerHie = "N";

            }


            if (dtCaseHie.Rows.Count > 0)
            {
                foreach (DataRow drCasehie in dtCaseHie.Rows)
                {
                    if (drCasehie["Code"].ToString().Trim() == AGY + Dept + Prog)
                    {
                        AgencyName = drCasehie["HIE_NAME"].ToString().Trim(); break;
                    }
                }

                cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, ShortName + " - INTAKE APPLICATION", X_Pos, Y_Pos - 25, 0);
                Header_Desc = ShortName + " - INTAKE APPLICATION";
                Form_Selection = AgencyName;//"Casemanagement Application";

                //cb.SetFontAndSize(bf_helv, 9);
                //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Applicant No: ", 30, Y_Pos - 40, 0);
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(BaseForm.BaseApplicationNo, Timesline), 30 + 72, Y_Pos-40, 0);

                cb.SetFontAndSize(bf_helv, 13);
                cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, Form_Selection, X_Pos, Y_Pos - 40, 0);
            }

            cb.SetFontAndSize(bf_helv, 9);
            //cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "Date Printed: ", 740, Y_Pos - 40, 0);
            //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(LookupDataAccess.Getdate(DateTime.Now.ToShortDateString()), Times), 780, Y_Pos - 40, 0);
            X_Pos = 30; Y_Pos -= 55;


            #region Comment block
            //}
            //else if (Privileges.ModuleCode == "02")
            //{
            //    string ShortName = string.Empty;
            //    if (dtCaseHie.Rows.Count > 0)
            //    {
            //        foreach (DataRow drCasehie in dtCaseHie.Rows)
            //        {
            //            if (drCasehie["Code"].ToString() == BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg)
            //            {
            //                ShortName = drCasehie["HIE_SHORT_NAME"].ToString().Trim(); break;
            //            }
            //        }
            //        cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, ShortName + " – EARLY CARE & EDUCATION ", X_Pos, Y_Pos - 25, 0);
            //        Header_Desc = ShortName + " – EARLY CARE & EDUCATION ";

            //            Form_Selection = "CASE MANAGEMENT APPLICATION";

            //        cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, Form_Selection, X_Pos, Y_Pos - 40, 0);

            //    }

            //    cb.SetFontAndSize(bf_helv, 9);
            //    X_Pos = 30; Y_Pos -= 50;
            //}
            //else //if (Privileges.ModuleCode == "05" )
            //{
            //    cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Application For Assistance", X_Pos, Y_Pos, 0);
            //    cb.SetFontAndSize(bf_helv, 9);
            //    cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "OFFICE USE ONLY ________________________________", 780, Y_Pos, 0);
            //    string SiteName = null, Site_city = null, Site_street = null, Site_state = null; string Site_zipcode = null;
            //    Header_Desc = "Application For Assistance";
            //    //To dispalying the Programme Address
            //    if (SiteList.Count > 0)
            //    {
            //        //drCaseSite = dsCaseSite.Tables[0].Rows[0];

            //        if (!string.IsNullOrEmpty(SiteList[0].SiteNAME.Trim()))
            //            SiteName = SiteList[0].SiteNAME.Trim();
            //        if (!string.IsNullOrEmpty(SiteList[0].SiteSTREET.Trim()))
            //            Site_street = "," + SiteList[0].SiteSTREET.Trim();
            //        if (!string.IsNullOrEmpty(SiteList[0].SiteCITY.Trim()))
            //            Site_city = "," + SiteList[0].SiteCITY.Trim();
            //        if (!string.IsNullOrEmpty(SiteList[0].SiteSTATE.Trim()))
            //            Site_state = "," + SiteList[0].SiteSTATE.Trim();
            //        if (SiteList[0].SiteZIP.Trim() != "0")
            //            Site_zipcode = "," + SiteList[0].SiteZIP.Trim();
            //    }
            //    cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, SiteName + Site_street + Site_city + Site_state + Site_zipcode, X_Pos, Y_Pos - 13, 0);
            //    cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "CABA# ___________________________________________", 780, Y_Pos - 13, 0);

            //    //To Print the Verifier Name on Application
            //    string Intake_Worker = null;
            //    if (dtVerifier.Rows.Count > 0)
            //    {
            //        foreach (DataRow drVerifier in dtVerifier.Rows)
            //        {
            //            if (drCaseMST["MST_INTAKE_WORKER"].ToString().Trim() == drVerifier["PWH_CASEWORKER"].ToString().Trim())
            //            {
            //                Intake_Worker = drVerifier["NAME"].ToString().Trim();
            //                break;
            //            }
            //        }
            //    }
            //    if (!string.IsNullOrEmpty(Intake_Worker))
            //        cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, Intake_Worker, X_Pos, Y_Pos - 26, 0);
            //    else
            //        cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "  ", X_Pos, Y_Pos - 26, 0);
            //    cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "Search Results ______________________________________", 780, Y_Pos - 26, 0);
            //    cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "___________________________________________________", 780, Y_Pos - 39, 0);
            //    cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "___________________________________________________", 780, Y_Pos - 52, 0);
            //    cb.SetFontAndSize(bf_helv, 9);
            //    X_Pos = 30; Y_Pos -= 72;
            //}

            //cb.SetFontAndSize(bf_helv, 9);
            //X_Pos = 30; Y_Pos -= 72;



            //cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "Date: ", 740, Y_Pos, 0);
            //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(LookupDataAccess.Getdate(DateTime.Now.ToShortDateString()), Timesline), 780, Y_Pos, 0);

            ////Y_Pos -= 13;
            #endregion

            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Applicant No: ", X_Pos, Y_Pos, 0);
            ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(App, Timesline), X_Pos + 72, Y_Pos, 0);

            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "Date Printed: ", 700, Y_Pos, 0);
            ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(DateTime.Now.ToString("g"), Timesline), 780, Y_Pos, 0);

            Y_Pos -= 15;
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Applicant Name   ", X_Pos, Y_Pos, 0);
            ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(BaseForm.BaseApplicationName, Timesline), X_Pos + 72, Y_Pos, 0);

            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "Application Date: ", 720, Y_Pos, 0);
            if (!string.IsNullOrEmpty(CASEMSTList[0].IntakeDate.Trim()))
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(LookupDataAccess.Getdate(CASEMSTList[0].IntakeDate.Trim()), Timesline), 780, Y_Pos, 0);
            else
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase("____________", Times), 780, Y_Pos, 0);

            //Y_Pos -= 13;
            //if (Privileges.ModuleCode == "05" || gvApp.CurrentRow.Cells["AppDet"].Value.ToString() == "Application for Assistance")
            //    cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Are you a citizen or naturalized Alien?", X_Pos, Y_Pos, 0);
            //else
            //    cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Do you have a disability and need an accommdation or special help to complete this application?", X_Pos, Y_Pos, 0);

            /************************************CheckBoxes****************************/
            iTextSharp.text.Rectangle rect = new iTextSharp.text.Rectangle(806, 40, 788, 60);
            PdfAppearance[] onOff = new PdfAppearance[2];
            onOff[0] = cb.CreateAppearance(20, 20);
            onOff[0].Rectangle(1, 20, 1, 20);
            onOff[0].Rectangle(18, 18, 1, 1);
            onOff[0].Stroke();
            onOff[1] = cb.CreateAppearance(20, 20);
            onOff[1].SetRGBColorFill(255, 128, 128);
            onOff[1].Rectangle(18, 18, 1, 1);
            onOff[1].FillStroke();
            onOff[1].MoveTo(1, 1);
            onOff[1].LineTo(19, 19);
            onOff[1].MoveTo(1, 19);
            onOff[1].LineTo(19, 1);

            RadioCheckField checkbox;
            PdfFormField SField;
            //if (Privileges.ModuleCode == "05" || gvApp.CurrentRow.Cells["AppDet"].Value.ToString() == "Application for Assistance")
            //{
            //    rect = new iTextSharp.text.Rectangle(190, Y_Pos + 8, 198, Y_Pos);
            //    //rect.Rotate();
            //    checkbox = new RadioCheckField(writer, rect, "Yes", "On");
            //    checkbox.BorderColor = new GrayColor(0.3f);
            //    checkbox.Rotation = 90;
            //    SField = checkbox.CheckField;
            //    writer.AddAnnotation(SField);
            //    ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Yes", Times), 200, Y_Pos, 0);
            //    rect = new iTextSharp.text.Rectangle(220, Y_Pos + 8, 228, Y_Pos);
            //    checkbox = new RadioCheckField(writer, rect, "No", "On");
            //    checkbox.BorderColor = new GrayColor(0.3f);
            //    checkbox.Rotation = 90;
            //    SField = checkbox.CheckField;
            //    writer.AddAnnotation(SField);
            //    ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("No", Times), 230, Y_Pos, 0);

            //    cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "If no, are you a qualified Alien??", 320, Y_Pos, 0);

            //    rect = new iTextSharp.text.Rectangle(450, Y_Pos + 8, 458, Y_Pos);
            //    //rect.Rotate();
            //    checkbox = new RadioCheckField(writer, rect, "SecondYes", "On");
            //    checkbox.BorderColor = new GrayColor(0.3f);
            //    checkbox.Rotation = 90;
            //    SField = checkbox.CheckField;
            //    writer.AddAnnotation(SField);
            //    ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Yes", Times), 460, Y_Pos, 0);
            //    rect = new iTextSharp.text.Rectangle(480, Y_Pos + 8, 488, Y_Pos);
            //    checkbox = new RadioCheckField(writer, rect, "SecondNo", "On");
            //    checkbox.BorderColor = new GrayColor(0.3f);
            //    checkbox.Rotation = 90;
            //    SField = checkbox.CheckField;
            //    writer.AddAnnotation(SField);
            //    ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("No", Times), 490, Y_Pos, 0);
            //}
            //else
            //{
            //    rect = new iTextSharp.text.Rectangle(390, Y_Pos + 8, 398, Y_Pos);
            //    //rect.Rotate();
            //    checkbox = new RadioCheckField(writer, rect, "SecondYes", "On");
            //    checkbox.BorderColor = new GrayColor(0.3f);
            //    checkbox.Rotation = 90;
            //    SField = checkbox.CheckField;
            //    writer.AddAnnotation(SField);
            //    ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("Yes", Times), 400, Y_Pos, 0);
            //    rect = new iTextSharp.text.Rectangle(420, Y_Pos + 8, 428, Y_Pos);
            //    checkbox = new RadioCheckField(writer, rect, "SecondNo", "On");
            //    checkbox.BorderColor = new GrayColor(0.3f);
            //    checkbox.Rotation = 90;
            //    SField = checkbox.CheckField;
            //    writer.AddAnnotation(SField);
            //    ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase("No", Times), 430, Y_Pos, 0);
            //}
            X_Pos = 30; Y_Pos -= 13;
            //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Applicant Name   ", X_Pos, Y_Pos, 0);
            //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(BaseForm.BaseApplicationName, Timesline), X_Pos + 72, Y_Pos, 0);
            //string Zip = string.Empty;
            //if (!string.IsNullOrEmpty(drCaseMST["MST_ZIP"].ToString().Trim()))
            //    Zip = "00000".Substring(0, 5 - drCaseMST["MST_ZIP"].ToString().Trim().Length) + drCaseMST["MST_ZIP"].ToString().Trim();
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Address ", X_Pos, Y_Pos, 0);
            string Apt = string.Empty; string Floor = string.Empty; string HN = string.Empty; string Suffix = string.Empty; string Street = string.Empty;
            string Zip = string.Empty;
            if (!string.IsNullOrEmpty(CASEMSTList[0].Apt.Trim()))
                Apt = ", Apt  " + CASEMSTList[0].Apt.Trim() + "   ";
            if (!string.IsNullOrEmpty(CASEMSTList[0].Flr.Trim()))
                Floor = "Flr  " + CASEMSTList[0].Flr.Trim() + "   ";
            if (!string.IsNullOrEmpty(CASEMSTList[0].Street.Trim()))
                Street = CASEMSTList[0].Street.Trim() + " ";
            if (!string.IsNullOrEmpty(CASEMSTList[0].Suffix.Trim()))
                Suffix = CASEMSTList[0].Suffix.Trim();
            if (!string.IsNullOrEmpty(CASEMSTList[0].Hn.Trim()))
                HN = CASEMSTList[0].Hn.Trim() + " ";
            if (!string.IsNullOrEmpty(CASEMSTList[0].Zip.Trim()) && CASEMSTList[0].Zip != "0")
                Zip = "00000".Substring(0, 5 - CASEMSTList[0].Zip.Trim().Length) + CASEMSTList[0].Zip.Trim();
            string Comma = string.Empty;
            if (!string.IsNullOrEmpty(CASEMSTList[0].Suffix.ToString().Trim()) && (!string.IsNullOrEmpty(CASEMSTList[0].Apt.ToString().Trim()) || !string.IsNullOrEmpty(CASEMSTList[0].Flr.ToString().Trim())))
                Comma = ", ";

            string Address = HN + Street + Suffix + Comma + Apt + Floor + ", " + CASEMSTList[0].City.Trim() + ", " + CASEMSTList[0].State.Trim() + " " + Zip;
            ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(Address, Timesline), X_Pos + 72, Y_Pos, 0);

            string Language = null;
            DataSet dsLang = DatabaseLayer.Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.LANGUAGECODES);
            DataTable dtLang = dsLang.Tables[0];
            foreach (DataRow drLang in dtLang.Rows)
            {
                if (CASEMSTList[0].Language.Trim() == drLang["Code"].ToString().Trim())
                {
                    Language = drLang["LookUpDesc"].ToString().Trim(); break;
                }
            }

            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Primary Language ", 400, Y_Pos, 0);//380 changed on 05/22/2017
            ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(Language, Timesline), 470, Y_Pos, 0);
            if (!string.IsNullOrEmpty(CASEMSTList[0].Site.Trim()))
            {
                DataSet dsSITE = DatabaseLayer.CaseMst.GetSITEDESC(AGY, CASEMSTList[0].Site);
                DataRow drSITE = dsSITE.Tables[0].Rows[0];
                string Site_Name = null;
                cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "Site : ", 640, Y_Pos, 0);
                Site_Name = drSITE["SiteName"].ToString().Trim();
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(Site_Name, Timesline), 650, Y_Pos, 0);
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(CASEMSTList[0].Site, Timesline), 650, Y_Pos, 0);
            }
            else
            {
                cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "Site : ", 640, Y_Pos, 0);
                cb.SetLineWidth(0.5f);
                //cb.SetLineCap(5);
                cb.MoveTo(650, Y_Pos - 2);
                cb.LineTo(705, Y_Pos - 2);
                cb.Stroke();
            }
            Y_Pos -= 13;
            //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Address ", X_Pos, Y_Pos, 0);
            //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(drCaseMST["MST_HN"].ToString().Trim() + " " + drCaseMST["MST_STREET"].ToString().Trim() + "," + drCaseMST["MST_CITY"].ToString().Trim() + "," + drCaseMST["MST_STATE"].ToString().Trim() + "," + drCaseMST["MST_ZIP"].ToString().Trim(), Timesline), X_Pos + 72, Y_Pos, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Mailing Address", X_Pos, Y_Pos, 0);
            string House_NO = null, Street1 = null, city = null, state = null, zip = null, DApt = null; string DSuffix = string.Empty;
            if (dtCasediff.Rows.Count > 0)
            {
                foreach (DataRow drCaseDiff in dtCasediff.Rows)
                {
                    if (!string.IsNullOrEmpty(drCaseDiff["DIFF_HN"].ToString().Trim()))
                        House_NO = drCaseDiff["DIFF_HN"].ToString().Trim() + " ";
                    if (!string.IsNullOrEmpty(drCaseDiff["DIFF_APT"].ToString().Trim()))
                        DApt = drCaseDiff["DIFF_APT"].ToString().Trim() + " ";
                    if (!string.IsNullOrEmpty(drCaseDiff["DIFF_SUFFIX"].ToString().Trim()))
                        DSuffix = " " + drCaseDiff["DIFF_SUFFIX"].ToString().Trim();
                    if (!string.IsNullOrEmpty(drCaseDiff["DIFF_STREET"].ToString().Trim()))
                        Street1 = drCaseDiff["DIFF_STREET"].ToString().Trim() + DSuffix + ",";
                    if (!string.IsNullOrEmpty(drCaseDiff["DIFF_CITY"].ToString().Trim()))
                        city = drCaseDiff["DIFF_CITY"].ToString().Trim() + ",";
                    if (!string.IsNullOrEmpty(drCaseDiff["DIFF_STATE"].ToString().Trim()))
                        state = drCaseDiff["DIFF_STATE"].ToString().Trim();
                    if (!string.IsNullOrEmpty(drCaseDiff["DIFF_ZIP"].ToString().Trim()))
                        zip = "00000".Substring(0, 5 - drCaseDiff["DIFF_ZIP"].ToString().Trim().Length) + drCaseDiff["DIFF_ZIP"].ToString().Trim();
                    if (zip == "00000") zip = ""; else zip = ", " + zip;
                }
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(House_NO + Street1 + DApt + city + state + zip, Timesline), X_Pos + 72, Y_Pos, 0);
            }
            else
            {
                //cb.SetLineWidth(0.5f);
                ////cb.SetLineCap(5);
                //cb.MoveTo(X_Pos + 72, Y_Pos);
                //cb.LineTo(210, Y_Pos);
                //cb.Stroke();
                //string Apt = string.Empty; string Floor = string.Empty; string HN = string.Empty; string Suffix = string.Empty; string Street = string.Empty;
                //string Zip = string.Empty;
                if (!string.IsNullOrEmpty(CASEMSTList[0].Apt.Trim()))
                    Apt = "Apt  " + CASEMSTList[0].Apt.Trim() + "   ";
                if (!string.IsNullOrEmpty(CASEMSTList[0].Flr.Trim()))
                    Floor = "Flr  " + CASEMSTList[0].Flr.Trim() + "   ";
                if (!string.IsNullOrEmpty(CASEMSTList[0].Street.Trim()))
                    Street = CASEMSTList[0].Street.Trim() + " ";
                if (!string.IsNullOrEmpty(CASEMSTList[0].Suffix.Trim()))
                    Suffix = CASEMSTList[0].Suffix.Trim() + ", ";
                if (!string.IsNullOrEmpty(CASEMSTList[0].Hn.Trim()))
                    HN = CASEMSTList[0].Hn.Trim() + " ";
                if (!string.IsNullOrEmpty(CASEMSTList[0].Zip.Trim()) && CASEMSTList[0].Zip != "0")
                    Zip = "00000".Substring(0, 5 - CASEMSTList[0].Zip.Trim().Length) + CASEMSTList[0].Zip.Trim();

                Address = HN + Street + Suffix + Apt + Floor + ", " + CASEMSTList[0].City.Trim() + ", " + CASEMSTList[0].State.Trim() + " " + Zip;

                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(Address, Timesline), X_Pos + 72, Y_Pos, 0);
            }


            if (!string.IsNullOrEmpty(CASEMSTList[0].Email.Trim()))
            {
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase("Email : ", Times), 400, Y_Pos, 0);//380 changed on 05/22/2017
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(CASEMSTList[0].Email.Trim(), Timesline), 430, Y_Pos, 0);
            }
            else
            {
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Email", 400, Y_Pos, 0);//380 changed on 05/22/2017
                cb.SetLineWidth(0.5f);
                //cb.SetLineCap(5);
                cb.MoveTo(430, Y_Pos);
                cb.LineTo(520, Y_Pos);
                cb.Stroke();
            }
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "Home Telephone ", 640, Y_Pos, 0);
            MaskedTextBox mskPhn = new MaskedTextBox();
            mskPhn.Mask = "(000)000-0000";
            mskPhn.Text = CASEMSTList[0].Area + CASEMSTList[0].Phone;   //"(" + drCaseMST["MST_AREA"].ToString() + ")" + drCaseMST["MST_PHONE"].ToString()
            ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(mskPhn.Text, Timesline), 650, Y_Pos, 0);
            Y_Pos -= 13;
            //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Mailing Address", X_Pos, Y_Pos, 0);
            //string House_NO = null, Street = null, city = null, state = null, zip = null;
            //if (dtCasediff.Rows.Count > 0)
            //{
            //    foreach (DataRow drCaseDiff in dtCasediff.Rows)
            //    {
            //        if (!string.IsNullOrEmpty(drCaseDiff["DIFF_HN"].ToString().Trim()))
            //            House_NO = drCaseDiff["DIFF_HN"].ToString().Trim() + " ";
            //        if (!string.IsNullOrEmpty(drCaseDiff["DIFF_STREET"].ToString().Trim()))
            //            Street = drCaseDiff["DIFF_STREET"].ToString().Trim() + ",";
            //        if (!string.IsNullOrEmpty(drCaseDiff["DIFF_STREET"].ToString().Trim()))
            //            city = drCaseDiff["DIFF_STREET"].ToString().Trim() + ",";
            //        if (!string.IsNullOrEmpty(drCaseDiff["DIFF_STATE"].ToString().Trim()))
            //            state = drCaseDiff["DIFF_STATE"].ToString().Trim() + ",";
            //        zip = drCaseDiff["DIFF_ZIP"].ToString().Trim();
            //    }
            //    ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(House_NO + Street + city + state + zip, Timesline), X_Pos + 72, Y_Pos, 0);
            //}
            //else
            //{
            //    //cb.SetLineWidth(0.5f);
            //    ////cb.SetLineCap(5);
            //    //cb.MoveTo(X_Pos + 72, Y_Pos);
            //    //cb.LineTo(210, Y_Pos);
            //    //cb.Stroke();
            //    ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(drCaseMST["MST_HN"].ToString().Trim() + " " + drCaseMST["MST_STREET"].ToString().Trim() + "," + drCaseMST["MST_CITY"].ToString().Trim() + "," + drCaseMST["MST_STATE"].ToString().Trim() + "," + drCaseMST["MST_ZIP"].ToString().Trim(), Timesline), X_Pos + 72, Y_Pos, 0);
            //}
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Message Number", 400, Y_Pos, 0);//380 changed on 05/22/2017
            if (!string.IsNullOrEmpty(CASEMSTList[0].MessagePhone.Trim()))
            {
                MaskedTextBox mskMessage = new MaskedTextBox();
                mskMessage.Mask = "(000)000-0000";
                mskMessage.Text = CASEMSTList[0].MessagePhone.Trim();
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(mskMessage.Text, Timesline), 470, Y_Pos, 0);
            }
            else
            {
                cb.SetLineWidth(0.5f);
                //cb.SetLineCap(5);
                cb.MoveTo(450, Y_Pos);
                cb.LineTo(505, Y_Pos);
                cb.Stroke();
            }

            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "Cell Number", 640, Y_Pos, 0);
            if (!string.IsNullOrEmpty(CASEMSTList[0].CellPhone.Trim()))
            {
                MaskedTextBox mskCell = new MaskedTextBox();
                mskCell.Mask = "(000)000-0000";
                mskCell.Text = CASEMSTList[0].CellPhone.Trim();
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(mskCell.Text, Timesline), 650, Y_Pos, 0);
            }
            else
            {
                cb.SetLineWidth(0.5f);
                //cb.SetLineCap(5);
                cb.MoveTo(650, Y_Pos - 3);
                cb.LineTo(705, Y_Pos - 3);
                cb.Stroke();
            }

            //Y_Pos -= 13;
            //cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "Message Number", 640, Y_Pos, 0);
            //if (!string.IsNullOrEmpty(drCaseMST["MST_MESSAGE_PHONE"].ToString().Trim()))
            //{
            //    MaskedTextBox mskMessage = new MaskedTextBox();
            //    mskMessage.Mask = "(000) 000-0000";
            //    mskMessage.Text = drCaseMST["MST_MESSAGE_PHONE"].ToString().Trim();
            //    ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(mskMessage.Text, Timesline), 650, Y_Pos, 0);
            //}
            //else
            //{
            //    cb.SetLineWidth(0.5f);
            //    //cb.SetLineCap(5);
            //    cb.MoveTo(650, Y_Pos - 3);
            //    cb.LineTo(705, Y_Pos - 3);
            //    cb.Stroke();
            //}

            Y_Pos -= 8;
            SetLine();
            Y_Pos -= 20;
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Housing Situation", X_Pos, Y_Pos, 0);
            DataSet dsHouseMain = Captain.DatabaseLayer.AgyTab.GetAgyTab(Consts.AgyTab.HOUSINGTYPES);
            string strMainData = dsHouseMain.Tables[0].Rows[0]["Agy_9"].ToString();
            string CodeColSubscript = string.Empty, DescColSubscript = string.Empty;
            if (!string.IsNullOrEmpty(dsHouseMain.Tables[0].Rows[0]["AGY_ACTIVE"].ToString().Trim()))
                CodeColSubscript = "AGY_" + dsHouseMain.Tables[0].Rows[0]["AGY_ACTIVE"].ToString().Trim();
            if (!string.IsNullOrEmpty(dsHouseMain.Tables[0].Rows[0]["AGY_DEFAULT"].ToString().Trim()))
                DescColSubscript = "AGY_" + dsHouseMain.Tables[0].Rows[0]["AGY_DEFAULT"].ToString().Trim();


            //DataSet dsHousing = DatabaseLayer.Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.HOUSINGTYPES);
            DataSet dsHousing = DatabaseLayer.AgyTab.GetAgyTabDetails(Consts.AgyTab.HOUSINGTYPES);
            DataTable dtHousing = dsHousing.Tables[0];
            PdfFormField Field;
            X_Pos = 140;
            string CheckTitle = string.Empty;
            foreach (DataRow drHousing in dtHousing.Rows)
            {
                if (drHousing["AGY_ACTIVE"].ToString() == "Y" ||
                    (drHousing["AGY_ACTIVE"].ToString() == "N" && !string.IsNullOrEmpty(CASEMSTList[0].Housing.Trim()) && CASEMSTList[0].Housing.Trim() == drHousing[CodeColSubscript].ToString().Trim()))
                {
                    rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 8, X_Pos + 8, Y_Pos);
                    checkbox = new RadioCheckField(writer, rect, drHousing[DescColSubscript].ToString().Trim(), "Yes");
                    checkbox.BorderColor = new GrayColor(0.3f);
                    checkbox.Rotation = 90;
                    checkbox.CheckType = RadioCheckField.TYPE_CHECK;
                    if (CASEMSTList[0].Housing.Trim().Trim() == drHousing[CodeColSubscript].ToString().Trim())
                        checkbox.Checked = true;
                    Field = checkbox.CheckField;
                    writer.AddAnnotation(Field);
                    ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(drHousing[DescColSubscript].ToString().Trim(), Times), X_Pos + 10, Y_Pos, 0);
                    X_Pos += 90;
                    if (X_Pos > 600)
                    {
                        X_Pos = 140;
                        Y_Pos -= 13;
                    }
                }
            }

            Y_Pos -= 25; X_Pos = 30;
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Family Type", X_Pos, Y_Pos, 0);
            X_Pos = 140;

            DataSet dsFamilyMain = Captain.DatabaseLayer.AgyTab.GetAgyTab(Consts.AgyTab.HOUSINGTYPES);
            strMainData = dsFamilyMain.Tables[0].Rows[0]["Agy_9"].ToString();
            CodeColSubscript = string.Empty; DescColSubscript = string.Empty;
            if (!string.IsNullOrEmpty(dsFamilyMain.Tables[0].Rows[0]["AGY_ACTIVE"].ToString().Trim()))
                CodeColSubscript = "AGY_" + dsFamilyMain.Tables[0].Rows[0]["AGY_ACTIVE"].ToString().Trim();
            if (!string.IsNullOrEmpty(dsFamilyMain.Tables[0].Rows[0]["AGY_DEFAULT"].ToString().Trim()))
                DescColSubscript = "AGY_" + dsFamilyMain.Tables[0].Rows[0]["AGY_DEFAULT"].ToString().Trim();

            //DataSet dsFamilyType = DatabaseLayer.Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.FAMILYTYPE);
            DataSet dsFamilyType = DatabaseLayer.AgyTab.GetAgyTabDetails(Consts.AgyTab.FAMILYTYPE);
            DataTable dtFamilyType = dsFamilyType.Tables[0];
            foreach (DataRow drFamilyType in dtFamilyType.Rows)
            {
                if (drFamilyType["AGY_ACTIVE"].ToString() == "Y" ||
                    (drFamilyType["AGY_ACTIVE"].ToString() == "N" && !string.IsNullOrEmpty(CASEMSTList[0].FamilyType.Trim()) && CASEMSTList[0].FamilyType.Trim() == drFamilyType[CodeColSubscript].ToString().Trim()))
                {
                    rect = new iTextSharp.text.Rectangle(X_Pos, Y_Pos + 8, X_Pos + 8, Y_Pos);
                    //checkbox = new RadioCheckField(writer, rect, "F" + drFamilyType["LookUpDesc"].ToString().Trim(), "On");
                    checkbox = new RadioCheckField(writer, rect, "F" + drFamilyType[DescColSubscript].ToString().Trim(), "Yes");
                    checkbox.BorderColor = new GrayColor(0.3f);
                    checkbox.Rotation = 90;
                    checkbox.CheckType = RadioCheckField.TYPE_CHECK;
                    if (CASEMSTList[0].FamilyType.Trim() == drFamilyType[CodeColSubscript].ToString().Trim())
                        checkbox.Checked = true;
                    Field = checkbox.CheckField;
                    writer.AddAnnotation(Field);
                    ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(drFamilyType[DescColSubscript].ToString().Trim(), Times), X_Pos + 10, Y_Pos, 0);
                    X_Pos += 130;
                    if (X_Pos > 600)
                    {
                        Y_Pos -= 13;
                        X_Pos = 140;
                    }

                }
            }

            Y_Pos -= 25; X_Pos = 30;
            int Count = casesnpList.Count;
            int Adults = 0, Child = 0, under5 = 0;
            foreach (CaseSnpEntity drsnp in casesnpList)
            {
                if (!string.IsNullOrEmpty(drsnp.Age))
                {
                    if (int.Parse(drsnp.Age) >= 18)
                        Adults++;
                    else
                        Child++;
                    if (int.Parse(drsnp.Age) < 5)
                        under5++;
                }
            }
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Total No of Household Members: ", X_Pos, Y_Pos, 0);
            ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(Count.ToString(), Timesline), X_Pos + 123, Y_Pos, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "No of Adults: ", 210, Y_Pos, 0);
            ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(Adults.ToString(), Timesline), 260, Y_Pos, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "No of Children: ", 310, Y_Pos, 0);
            ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(Child.ToString(), Timesline), 370, Y_Pos, 0);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "No of Children under 5: ", 420, Y_Pos, 0);
            ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(under5.ToString(), Timesline), 508, Y_Pos, 0);
            cb.EndText();

            //Temp table not displayed on the screen
            PdfPTable head = new PdfPTable(1);
            head.HorizontalAlignment = Element.ALIGN_CENTER;
            head.TotalWidth = 50f;
            PdfPCell headcell = new PdfPCell(new Phrase(""));
            headcell.HorizontalAlignment = Element.ALIGN_CENTER;
            headcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            head.AddCell(headcell);

            #region To Print SNP Details in the Table
            PdfPTable Snp_Table = new PdfPTable(13);
            Snp_Table.TotalWidth = 750f;
            Snp_Table.WidthPercentage = 100;
            Snp_Table.LockedWidth = true;
            float[] widths = new float[] { 55f, 45f, 25f, 25f, 10f, 13f, 30f, 28f, 42f, 18f, 18f, 22f, 35f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
            Snp_Table.SetWidths(widths);
            Snp_Table.HorizontalAlignment = Element.ALIGN_CENTER;
            Snp_Table.SpacingBefore = 270f;

            PdfPCell Header = new PdfPCell(new Phrase("HOUSEHOLD MEMBERS Listing yourself first, complete all spaces below for ALL persons living in the home.", TblFontBold));
            Header.Colspan = 15;
            Header.FixedHeight = 15f;
            Header.BackgroundColor = BaseColor.LIGHT_GRAY;
            Header.Border = iTextSharp.text.Rectangle.BOX;
            Snp_Table.AddCell(Header);

            PdfPCell row2 = new PdfPCell(new Phrase(""));
            row2.Colspan = 9;
            row2.FixedHeight = 15f;
            row2.Border = iTextSharp.text.Rectangle.BOX;
            Snp_Table.AddCell(row2);

            PdfPCell row2_Health = new PdfPCell(new Phrase("Health", TableFontBoldItalic));
            row2_Health.HorizontalAlignment = Element.ALIGN_CENTER;
            row2_Health.FixedHeight = 15f;
            row2_Health.Border = iTextSharp.text.Rectangle.BOX;
            Snp_Table.AddCell(row2_Health);

            PdfPCell row2_Space = new PdfPCell(new Phrase(""));
            row2_Space.Colspan = 3;
            row2_Space.FixedHeight = 15f;
            row2_Space.Border = iTextSharp.text.Rectangle.BOX;
            Snp_Table.AddCell(row2_Space);

            //PdfPCell row3 = new PdfPCell(new Phrase(""));
            //row3.Colspan = 3;
            //row3.FixedHeight = 15f;
            //row3.Border = iTextSharp.text.Rectangle.BOX;
            //Snp_Table.AddCell(row3);

            PdfPCell row3 = new PdfPCell(new Phrase(""));
            row3.Colspan = 2;
            row3.FixedHeight = 15f;
            row3.Border = iTextSharp.text.Rectangle.BOX;
            Snp_Table.AddCell(row3);

            PdfPCell row3_SSN = new PdfPCell(new Phrase("Social", TableFontBoldItalic));
            row3_SSN.HorizontalAlignment = Element.ALIGN_CENTER;
            row3_SSN.FixedHeight = 15f;
            row3_SSN.Border = iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.LEFT_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER;
            Snp_Table.AddCell(row3_SSN);

            PdfPCell row3_Birth = new PdfPCell(new Phrase("BirthDate", TableFontBoldItalic));
            row3_Birth.HorizontalAlignment = Element.ALIGN_CENTER;
            row3_Birth.FixedHeight = 15f;
            row3_Birth.Border = iTextSharp.text.Rectangle.BOX;
            Snp_Table.AddCell(row3_Birth);

            PdfPCell row3_Space = new PdfPCell(new Phrase(""));
            //row3_Space.Colspan = 2;
            row3_Space.FixedHeight = 15f;
            row3_Space.Border = iTextSharp.text.Rectangle.BOX;
            Snp_Table.AddCell(row3_Space);

            PdfPCell row3_Sex = new PdfPCell(new Phrase("Sex", TableFontBoldItalic));
            row3_Sex.HorizontalAlignment = Element.ALIGN_CENTER;
            row3_Sex.FixedHeight = 15f;
            row3_Sex.Border = iTextSharp.text.Rectangle.BOX;
            Snp_Table.AddCell(row3_Sex);

            PdfPCell row3_Space2 = new PdfPCell(new Phrase(""));
            row3_Space2.Colspan = 3;
            row3_Space2.HorizontalAlignment = Element.ALIGN_CENTER;
            row3_Space2.FixedHeight = 15f;
            row3_Space2.Border = iTextSharp.text.Rectangle.BOX;
            Snp_Table.AddCell(row3_Space2);

            PdfPCell row3_Insurance = new PdfPCell(new Phrase("Insurance ", TableFontBoldItalic));
            row3_Insurance.HorizontalAlignment = Element.ALIGN_CENTER;
            row3_Insurance.FixedHeight = 15f;
            row3_Insurance.Border = iTextSharp.text.Rectangle.BOX;
            Snp_Table.AddCell(row3_Insurance);

            PdfPCell row3_Veteran = new PdfPCell(new Phrase("Veteran", TableFontBoldItalic));
            row3_Veteran.HorizontalAlignment = Element.ALIGN_CENTER;
            row3_Veteran.FixedHeight = 15f;
            row3_Veteran.Border = iTextSharp.text.Rectangle.BOX;
            Snp_Table.AddCell(row3_Veteran);

            PdfPCell row3_Receive_FS = new PdfPCell(new Phrase("Receive FS", TableFontBoldItalic));
            row3_Receive_FS.HorizontalAlignment = Element.ALIGN_CENTER;
            row3_Receive_FS.FixedHeight = 15f;
            row3_Receive_FS.Border = iTextSharp.text.Rectangle.BOX;
            Snp_Table.AddCell(row3_Receive_FS);

            PdfPCell row3_Space3 = new PdfPCell(new Phrase("", TableFontBoldItalic));
            row3_Space3.HorizontalAlignment = Element.ALIGN_CENTER;
            row3_Space3.FixedHeight = 15f;
            row3_Space3.Border = iTextSharp.text.Rectangle.BOX;
            Snp_Table.AddCell(row3_Space3);

            string[] col = { "Name (last, first, MI)", "Relationship to Applicant", "Security", "mm/dd/yyyy", "Age", "M/F", "Ethnicity", "Race", "Education", "Y/N", "Y/N", "Y/N", "Disabled" };
            for (int i = 0; i < col.Length; ++i)
            {
                PdfPCell cell = new PdfPCell(new Phrase(col[i], TableFontBoldItalic));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.FixedHeight = 15f;
                if (i == 2) cell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.LEFT_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER;
                else cell.Border = iTextSharp.text.Rectangle.BOX;
                Snp_Table.AddCell(cell);
            }

            int Tbl_Count = 0; string FamSeq = string.Empty;
            List<CaseSnpEntity> snplist = new List<CaseSnpEntity>();
            foreach (CaseSnpEntity entity in casesnpList)
            {
                if (CASEMSTList[0].FamilySeq == entity.FamilySeq)
                {
                    FamSeq = entity.FamilySeq.Trim();
                    string ApplicantName = entity.NameixLast + " " + entity.NameixFi + " " + entity.NameixMi;//snpEntity.NameixFi.Trim() + " " + snpEntity.NameixLast.Trim();
                    PdfPCell Name = new PdfPCell(new Phrase(ApplicantName, TableFont));
                    Name.HorizontalAlignment = Element.ALIGN_LEFT;
                    Name.FixedHeight = 15f;
                    Name.Border = iTextSharp.text.Rectangle.BOX;
                    Snp_Table.AddCell(Name);

                    string Relation = null;
                    DataSet dsRelation = DatabaseLayer.Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.RELATIONSHIP);
                    DataTable dtRelation = dsRelation.Tables[0];
                    foreach (DataRow drRelation in dtRelation.Rows)
                    {
                        if (entity.MemberCode.Trim() == drRelation["Code"].ToString().Trim())
                        {
                            Relation = drRelation["LookUpDesc"].ToString().Trim(); break;
                        }
                    }
                    PdfPCell RelationShip = new PdfPCell(new Phrase(Relation, TableFont));
                    RelationShip.HorizontalAlignment = Element.ALIGN_LEFT;
                    RelationShip.FixedHeight = 15f;
                    RelationShip.Border = iTextSharp.text.Rectangle.BOX;
                    Snp_Table.AddCell(RelationShip);
                    string SSNNum = null;
                    if (!string.IsNullOrEmpty(entity.Ssno.Trim()))
                        //SSNNum = drCaseSNP["SNP_SSNO"].ToString().Trim();
                        SSNNum = "xxx" + "-" + "xx" + "-" + entity.Ssno.Trim().Substring(5, 4);
                    PdfPCell SSN = new PdfPCell(new Phrase(SSNNum, TableFont));
                    SSN.HorizontalAlignment = Element.ALIGN_CENTER;
                    SSN.FixedHeight = 15f;
                    SSN.Border = iTextSharp.text.Rectangle.BOX;
                    Snp_Table.AddCell(SSN);

                    string DOB = string.Empty;
                    if (!string.IsNullOrEmpty(entity.AltBdate))
                    {
                        DOB = CommonFunctions.ChangeDateFormat(entity.AltBdate.Trim(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);
                    }
                    PdfPCell BirthDate = new PdfPCell(new Phrase(LookupDataAccess.Getdate(entity.AltBdate.Trim()), TableFont));
                    BirthDate.HorizontalAlignment = Element.ALIGN_CENTER;
                    BirthDate.FixedHeight = 15f;
                    BirthDate.Border = iTextSharp.text.Rectangle.BOX;
                    Snp_Table.AddCell(BirthDate);

                    if (entity.Age.Trim() != "0")
                    {
                        PdfPCell Age = new PdfPCell(new Phrase(entity.Age.Trim(), TableFont));
                        Age.HorizontalAlignment = Element.ALIGN_CENTER;
                        Age.FixedHeight = 15f;
                        Age.Border = iTextSharp.text.Rectangle.BOX;
                        Snp_Table.AddCell(Age);
                    }
                    else
                    {
                        PdfPCell Age = new PdfPCell(new Phrase("", TableFont));
                        Age.HorizontalAlignment = Element.ALIGN_CENTER;
                        Age.FixedHeight = 15f;
                        Age.Border = iTextSharp.text.Rectangle.BOX;
                        Snp_Table.AddCell(Age);
                    }

                    PdfPCell Sex = new PdfPCell(new Phrase(entity.Sex.Trim(), TableFont));
                    Sex.HorizontalAlignment = Element.ALIGN_CENTER;
                    Sex.FixedHeight = 15f;
                    Sex.Border = iTextSharp.text.Rectangle.BOX;
                    Snp_Table.AddCell(Sex);

                    string Etinic = null;
                    DataSet dsEtinic = DatabaseLayer.Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.ETHNICODES);
                    DataTable dtEtinic = dsEtinic.Tables[0];
                    foreach (DataRow drEtinic in dtEtinic.Rows)
                    {
                        if (entity.Ethnic.Trim() == drEtinic["Code"].ToString().Trim())
                        {
                            Etinic = drEtinic["LookUpDesc"].ToString().Trim(); break;
                        }
                    }
                    PdfPCell Snp_Ethnic = new PdfPCell(new Phrase(Etinic, TableFont));
                    Snp_Ethnic.HorizontalAlignment = Element.ALIGN_LEFT;
                    Snp_Ethnic.FixedHeight = 15f;
                    Snp_Ethnic.Border = iTextSharp.text.Rectangle.BOX;
                    Snp_Table.AddCell(Snp_Ethnic);

                    string Race = null;
                    DataSet dsRace = DatabaseLayer.Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.RACE);
                    DataTable dtRace = dsRace.Tables[0];
                    foreach (DataRow drRace in dtRace.Rows)
                    {
                        if (entity.Race.Trim() == drRace["Code"].ToString().Trim())
                        {
                            Race = drRace["LookUpDesc"].ToString().Trim(); break;
                        }
                    }
                    PdfPCell Snp_Race = new PdfPCell(new Phrase(Race, TableFont));
                    Snp_Race.HorizontalAlignment = Element.ALIGN_LEFT;
                    Snp_Race.FixedHeight = 15f;
                    Snp_Race.Border = iTextSharp.text.Rectangle.BOX;
                    Snp_Table.AddCell(Snp_Race);

                    string Education = null;
                    DataSet dsEducation = DatabaseLayer.Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.EDUCATIONCODES);
                    DataTable dtEducation = dsEducation.Tables[0];
                    foreach (DataRow drEducation in dtEducation.Rows)
                    {
                        if (entity.Education.Trim() == drEducation["Code"].ToString().Trim())
                        {
                            Education = drEducation["LookUpDesc"].ToString().Trim(); break;
                        }
                    }
                    PdfPCell Snp_Education = new PdfPCell(new Phrase(Education, TableFont));
                    Snp_Education.HorizontalAlignment = Element.ALIGN_LEFT;
                    Snp_Education.FixedHeight = 15f;
                    Snp_Education.Border = iTextSharp.text.Rectangle.BOX;
                    Snp_Table.AddCell(Snp_Education);

                    PdfPCell Insurance = new PdfPCell(new Phrase(entity.HealthIns.Trim(), TableFont));
                    Insurance.HorizontalAlignment = Element.ALIGN_CENTER;
                    Insurance.FixedHeight = 15f;
                    Insurance.Border = iTextSharp.text.Rectangle.BOX;
                    Snp_Table.AddCell(Insurance);

                    PdfPCell Vetran = new PdfPCell(new Phrase(entity.Vet.Trim(), TableFont));
                    Vetran.HorizontalAlignment = Element.ALIGN_CENTER;
                    Vetran.FixedHeight = 15f;
                    Vetran.Border = iTextSharp.text.Rectangle.BOX;
                    Snp_Table.AddCell(Vetran);

                    PdfPCell FoodStamps1 = new PdfPCell(new Phrase(entity.FootStamps.Trim(), TableFont));
                    FoodStamps1.HorizontalAlignment = Element.ALIGN_CENTER;
                    FoodStamps1.FixedHeight = 15f;
                    FoodStamps1.Border = iTextSharp.text.Rectangle.BOX;
                    Snp_Table.AddCell(FoodStamps1);

                    string AGYDisable = null;
                    DataSet dsDisable = DatabaseLayer.Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.DISABLED);
                    DataTable dtDisable = dsDisable.Tables[0];
                    foreach (DataRow drDisable in dtDisable.Rows)
                    {
                        if (entity.Disable.Trim() == drDisable["Code"].ToString().Trim())
                            AGYDisable = drDisable["LookUpDesc"].ToString().Trim();
                    }
                    PdfPCell Disabled = new PdfPCell(new Phrase(AGYDisable, TableFont));
                    Disabled.HorizontalAlignment = Element.ALIGN_LEFT;
                    Disabled.FixedHeight = 15f;
                    Disabled.Border = iTextSharp.text.Rectangle.BOX;
                    Snp_Table.AddCell(Disabled);
                }
            }

            string MotherName = string.Empty; string FatherName = string.Empty;
            string M_Address = string.Empty; string F_Address = string.Empty;
            string M_Phone = string.Empty; string F_Phone = string.Empty;
            string M_FullPart = string.Empty; string F_FullPart = string.Empty;
            foreach (CaseSnpEntity drCaseSNP in casesnpList)
            {
                if (FamSeq != drCaseSNP.FamilySeq.Trim())
                {
                    if (MotherEntity != null)
                    {
                        if (drCaseSNP.MemberCode == MotherEntity.Code)
                        {
                            MotherName = drCaseSNP.EmployerName.Trim();
                            if (!string.IsNullOrEmpty(drCaseSNP.EmployerStreet.Trim()))
                                M_Address = drCaseSNP.EmployerStreet.Trim() + ",";
                            if (!string.IsNullOrEmpty(drCaseSNP.EmployerCity.Trim()))
                                M_Address = drCaseSNP.EmployerCity.Trim();
                            if (!string.IsNullOrEmpty(drCaseSNP.EmplPhone.Trim()))
                                M_Phone = drCaseSNP.EmplPhone.Trim();
                            if (drCaseSNP.FullTimeHours.Trim() != "0")
                                M_FullPart = "F";
                            else if (drCaseSNP.FullTimeHours.Trim() != "0")
                                M_FullPart = "P";
                        }
                    }

                    if (FatherEntity.Count > 0)
                    {
                        foreach (CommonEntity cm in FatherEntity)
                        {
                            if (cm.Code == drCaseSNP.MemberCode)
                            {
                                FatherName = drCaseSNP.EmployerName.Trim();
                                if (!string.IsNullOrEmpty(drCaseSNP.EmployerStreet))
                                    F_Address = drCaseSNP.EmployerStreet.Trim() + ",";
                                if (!string.IsNullOrEmpty(drCaseSNP.EmployerCity.Trim()))
                                    F_Address = drCaseSNP.EmployerCity.Trim();
                                if (!string.IsNullOrEmpty(drCaseSNP.EmplPhone.Trim()))
                                    F_Phone = drCaseSNP.EmplPhone.Trim();
                                if (drCaseSNP.FullTimeHours.Trim() != "0")
                                    F_FullPart = "F";
                                else if (drCaseSNP.FullTimeHours.Trim() != "0")
                                    F_FullPart = "P";
                                break;
                            }
                        }
                    }

                    string ApplicantName = drCaseSNP.NameixLast.Trim() + " " + drCaseSNP.NameixFi.Trim() + " " + drCaseSNP.NameixMi.Trim();//snpEntity.NameixFi.Trim() + " " + snpEntity.NameixLast.Trim();
                    PdfPCell Name = new PdfPCell(new Phrase(ApplicantName, TableFont));
                    Name.HorizontalAlignment = Element.ALIGN_LEFT;
                    Name.FixedHeight = 15f;
                    Name.Border = iTextSharp.text.Rectangle.BOX;
                    Snp_Table.AddCell(Name);

                    string Relation = null;
                    DataSet dsRelation = DatabaseLayer.Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.RELATIONSHIP);
                    DataTable dtRelation = dsRelation.Tables[0];
                    foreach (DataRow drRelation in dtRelation.Rows)
                    {
                        if (drCaseSNP.MemberCode.Trim() == drRelation["Code"].ToString().Trim())
                        {
                            Relation = drRelation["LookUpDesc"].ToString().Trim(); break;
                        }
                    }
                    PdfPCell RelationShip = new PdfPCell(new Phrase(Relation, TableFont));
                    RelationShip.HorizontalAlignment = Element.ALIGN_LEFT;
                    RelationShip.FixedHeight = 15f;
                    RelationShip.Border = iTextSharp.text.Rectangle.BOX;
                    Snp_Table.AddCell(RelationShip);
                    string SSNNum = null;
                    if (!string.IsNullOrEmpty(drCaseSNP.Ssno.Trim()))
                        SSNNum = "xxx" + "-" + "xx" + "-" + drCaseSNP.Ssno.Trim().Substring(5, 4);
                    PdfPCell SSN = new PdfPCell(new Phrase(SSNNum, TableFont));
                    SSN.HorizontalAlignment = Element.ALIGN_CENTER;
                    SSN.FixedHeight = 15f;
                    SSN.Border = iTextSharp.text.Rectangle.BOX;
                    Snp_Table.AddCell(SSN);

                    string DOB = string.Empty;
                    if (!string.IsNullOrEmpty(drCaseSNP.AltBdate))
                    {
                        DOB = CommonFunctions.ChangeDateFormat(drCaseSNP.AltBdate.Trim(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);
                    }
                    PdfPCell BirthDate = new PdfPCell(new Phrase(LookupDataAccess.Getdate(drCaseSNP.AltBdate.Trim()), TableFont));
                    BirthDate.HorizontalAlignment = Element.ALIGN_CENTER;
                    BirthDate.FixedHeight = 15f;
                    BirthDate.Border = iTextSharp.text.Rectangle.BOX;
                    Snp_Table.AddCell(BirthDate);

                    if (drCaseSNP.Age.Trim() != "0")
                    {
                        PdfPCell Age = new PdfPCell(new Phrase(drCaseSNP.Age.Trim(), TableFont));
                        Age.HorizontalAlignment = Element.ALIGN_CENTER;
                        Age.FixedHeight = 15f;
                        Age.Border = iTextSharp.text.Rectangle.BOX;
                        Snp_Table.AddCell(Age);
                    }
                    else
                    {
                        PdfPCell Age = new PdfPCell(new Phrase("", TableFont));
                        Age.HorizontalAlignment = Element.ALIGN_CENTER;
                        Age.FixedHeight = 15f;
                        Age.Border = iTextSharp.text.Rectangle.BOX;
                        Snp_Table.AddCell(Age);
                    }

                    PdfPCell Sex = new PdfPCell(new Phrase(drCaseSNP.Sex.Trim(), TableFont));
                    Sex.HorizontalAlignment = Element.ALIGN_CENTER;
                    Sex.FixedHeight = 15f;
                    Sex.Border = iTextSharp.text.Rectangle.BOX;
                    Snp_Table.AddCell(Sex);

                    string Etinic = null;
                    DataSet dsEtinic = DatabaseLayer.Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.ETHNICODES);
                    DataTable dtEtinic = dsEtinic.Tables[0];
                    foreach (DataRow drEtinic in dtEtinic.Rows)
                    {
                        if (drCaseSNP.Ethnic.Trim() == drEtinic["Code"].ToString().Trim())
                        {
                            Etinic = drEtinic["LookUpDesc"].ToString().Trim(); break;
                        }
                    }
                    PdfPCell Snp_Ethnic = new PdfPCell(new Phrase(Etinic, TableFont));
                    Snp_Ethnic.HorizontalAlignment = Element.ALIGN_LEFT;
                    Snp_Ethnic.FixedHeight = 15f;
                    Snp_Ethnic.Border = iTextSharp.text.Rectangle.BOX;
                    Snp_Table.AddCell(Snp_Ethnic);

                    string Race = null;
                    DataSet dsRace = DatabaseLayer.Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.RACE);
                    DataTable dtRace = dsRace.Tables[0];
                    foreach (DataRow drRace in dtRace.Rows)
                    {
                        if (drCaseSNP.Race.Trim() == drRace["Code"].ToString().Trim())
                        {
                            Race = drRace["LookUpDesc"].ToString().Trim(); break;
                        }
                    }
                    PdfPCell Snp_Race = new PdfPCell(new Phrase(Race, TableFont));
                    Snp_Race.HorizontalAlignment = Element.ALIGN_LEFT;
                    Snp_Race.FixedHeight = 15f;
                    Snp_Race.Border = iTextSharp.text.Rectangle.BOX;
                    Snp_Table.AddCell(Snp_Race);

                    string Education = null;
                    DataSet dsEducation = DatabaseLayer.Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.EDUCATIONCODES);
                    DataTable dtEducation = dsEducation.Tables[0];
                    foreach (DataRow drEducation in dtEducation.Rows)
                    {
                        if (drCaseSNP.Education.Trim() == drEducation["Code"].ToString().Trim())
                        {
                            Education = drEducation["LookUpDesc"].ToString().Trim(); break;
                        }
                    }
                    PdfPCell Snp_Education = new PdfPCell(new Phrase(Education, TableFont));
                    Snp_Education.HorizontalAlignment = Element.ALIGN_LEFT;
                    Snp_Education.FixedHeight = 15f;
                    Snp_Education.Border = iTextSharp.text.Rectangle.BOX;
                    Snp_Table.AddCell(Snp_Education);

                    PdfPCell Insurance = new PdfPCell(new Phrase(drCaseSNP.HealthIns.Trim(), TableFont));
                    Insurance.HorizontalAlignment = Element.ALIGN_CENTER;
                    Insurance.FixedHeight = 15f;
                    Insurance.Border = iTextSharp.text.Rectangle.BOX;
                    Snp_Table.AddCell(Insurance);

                    PdfPCell Vetran = new PdfPCell(new Phrase(drCaseSNP.Vet.Trim(), TableFont));
                    Vetran.HorizontalAlignment = Element.ALIGN_CENTER;
                    Vetran.FixedHeight = 15f;
                    Vetran.Border = iTextSharp.text.Rectangle.BOX;
                    Snp_Table.AddCell(Vetran);

                    PdfPCell FoodStamps1 = new PdfPCell(new Phrase(drCaseSNP.FootStamps.Trim(), TableFont));
                    FoodStamps1.HorizontalAlignment = Element.ALIGN_CENTER;
                    FoodStamps1.FixedHeight = 15f;
                    FoodStamps1.Border = iTextSharp.text.Rectangle.BOX;
                    Snp_Table.AddCell(FoodStamps1);

                    string AGYDisable = null;
                    DataSet dsDisable = DatabaseLayer.Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.DISABLED);
                    DataTable dtDisable = dsDisable.Tables[0];
                    foreach (DataRow drDisable in dtDisable.Rows)
                    {
                        if (drCaseSNP.Disable.Trim() == drDisable["Code"].ToString().Trim())
                            AGYDisable = drDisable["LookUpDesc"].ToString().Trim();
                    }
                    PdfPCell Disabled = new PdfPCell(new Phrase(AGYDisable, TableFont));
                    Disabled.HorizontalAlignment = Element.ALIGN_LEFT;
                    Disabled.FixedHeight = 15f;
                    Disabled.Border = iTextSharp.text.Rectangle.BOX;
                    Snp_Table.AddCell(Disabled);

                    Tbl_Count++;
                    if (Tbl_Count > 10)
                        break;
                }
            }
            int Len_Var = 130 - Tbl_Count * 13;
            for (int j = 0; j <= Len_Var; ++j)  //120
            {
                PdfPCell SpaceCell = new PdfPCell(new Phrase(" ", TableFont));
                SpaceCell.HorizontalAlignment = Element.ALIGN_CENTER;
                SpaceCell.FixedHeight = 15f;
                SpaceCell.Border = iTextSharp.text.Rectangle.BOX;
                Snp_Table.AddCell(SpaceCell);
            }

            document.Add(head);
            document.Add(Snp_Table);
            document.NewPage();

            #endregion End Of SNP details Table

            //cb.BeginText();
            //X_Pos = 400; Y_Pos = 580;
            //cb.SetFontAndSize(bf_helv, 13);
            //cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, Header_Desc, X_Pos, Y_Pos, 0);

            //cb.SetFontAndSize(bf_helv, 9);
            //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Applicant No: ", 30, Y_Pos - 15, 0);
            //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(BaseForm.BaseApplicationNo, Timesline), 30 + 72, Y_Pos - 15, 0);

            //cb.SetFontAndSize(bf_helv, 13);
            //cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, Form_Selection, X_Pos, Y_Pos - 15, 0);
            //cb.SetFontAndSize(bf_helv, 9);
            //cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "Date Printed: ", 740, Y_Pos - 15, 0);
            //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(LookupDataAccess.Getdate(DateTime.Now.ToShortDateString()), Times), 780, Y_Pos - 15, 0);

            //X_Pos = 30; Y_Pos -= 30;
            //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Applicant Name   ", X_Pos, Y_Pos, 0);
            //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(BaseForm.BaseApplicationName, Timesline), X_Pos + 72, Y_Pos, 0);

            //cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "Application Date: ", 740, Y_Pos, 0);
            //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(LookupDataAccess.Getdate(drCaseMST["MST_INTAKE_DATE"].ToString().Trim()), Timesline), 780, Y_Pos, 0);

            //cb.EndText();

            iTextSharp.text.Font HeaderFontBold = new iTextSharp.text.Font(bf_helv, 13);
            //start the Income details of a Family to the table

            //PdfPTable SpaceTable = new PdfPTable(1);
            //SpaceTable.TotalWidth = 750f;
            //SpaceTable.WidthPercentage = 100;
            //SpaceTable.LockedWidth = true;
            //float[] SpaceTablewidths = new float[] { 80f };
            //SpaceTable.SetWidths(SpaceTablewidths);
            //SpaceTable.HorizontalAlignment = Element.ALIGN_CENTER;
            //SpaceTable.SpacingAfter = 70f;

            #region Income Table

            PdfPTable IncomeTable = new PdfPTable(5);
            IncomeTable.TotalWidth = 750f;
            IncomeTable.WidthPercentage = 100;
            IncomeTable.LockedWidth = true;
            float[] Incomewidths = new float[] { 80f, 50f, 40f, 90f, 80f };
            IncomeTable.SetWidths(Incomewidths);
            IncomeTable.HorizontalAlignment = Element.ALIGN_CENTER;
            IncomeTable.SpacingBefore = 100f;

            PdfPCell IncomeCell = new PdfPCell(new Phrase(Header_Desc, HeaderFontBold));
            IncomeCell.Colspan = 5;
            IncomeCell.HorizontalAlignment = Element.ALIGN_CENTER;
            IncomeCell.FixedHeight = 15f;
            IncomeCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            IncomeTable.AddCell(IncomeCell);

            PdfPCell IncomeCell1 = new PdfPCell(new Phrase(Form_Selection, HeaderFontBold));
            IncomeCell1.Colspan = 5;
            IncomeCell1.HorizontalAlignment = Element.ALIGN_CENTER;
            IncomeCell1.FixedHeight = 15f;
            IncomeCell1.Border = iTextSharp.text.Rectangle.NO_BORDER;
            IncomeTable.AddCell(IncomeCell1);

            PdfPCell IncomeCell0 = new PdfPCell(new Phrase("Applicant No: " + BaseForm.BaseApplicationNo, TableFont));
            IncomeCell0.Colspan = 2;
            IncomeCell0.HorizontalAlignment = Element.ALIGN_LEFT;
            IncomeCell0.FixedHeight = 15f;
            IncomeCell0.Border = iTextSharp.text.Rectangle.NO_BORDER;
            IncomeTable.AddCell(IncomeCell0);

            PdfPCell IncomeCell2 = new PdfPCell(new Phrase("Date Printed: " + DateTime.Now.ToString("g"), TableFont));
            IncomeCell2.Colspan = 3;
            IncomeCell2.HorizontalAlignment = Element.ALIGN_RIGHT;
            IncomeCell2.FixedHeight = 15f;
            IncomeCell2.Border = iTextSharp.text.Rectangle.NO_BORDER;
            IncomeTable.AddCell(IncomeCell2);

            PdfPCell IncomeCell3 = new PdfPCell(new Phrase("Applicant Name   " + BaseForm.BaseApplicationName, TableFont));
            IncomeCell3.Colspan = 3;
            IncomeCell3.HorizontalAlignment = Element.ALIGN_LEFT;
            IncomeCell3.FixedHeight = 15f;
            IncomeCell3.Border = iTextSharp.text.Rectangle.NO_BORDER;
            IncomeTable.AddCell(IncomeCell3);

            if (!string.IsNullOrEmpty(CASEMSTList[0].IntakeDate.Trim()))
            {
                PdfPCell IncomeCell4 = new PdfPCell(new Phrase("Application Date: " + LookupDataAccess.Getdate(CASEMSTList[0].IntakeDate.Trim()), TableFont));
                IncomeCell4.Colspan = 2;
                IncomeCell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                IncomeCell4.FixedHeight = 15f;
                IncomeCell4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                IncomeTable.AddCell(IncomeCell4);
            }
            else
            {
                PdfPCell IncomeCell4 = new PdfPCell(new Phrase("Application Date: " + "____________", TableFont));
                IncomeCell4.Colspan = 2;
                IncomeCell4.HorizontalAlignment = Element.ALIGN_RIGHT;
                IncomeCell4.FixedHeight = 15f;
                IncomeCell4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                IncomeTable.AddCell(IncomeCell4);
            }
            PdfPCell IncomeHeader = new PdfPCell(new Phrase("Financial Information - As reported by Applicant", TblFontBold));
            IncomeHeader.Colspan = 5;
            IncomeHeader.FixedHeight = 15f;
            IncomeHeader.BackgroundColor = BaseColor.LIGHT_GRAY;
            IncomeHeader.Border = iTextSharp.text.Rectangle.BOX;
            IncomeTable.AddCell(IncomeHeader);
            string[] Incomecol = { "Household Member", "Amount", "Interval", "Income Sources", "How Verified?" };
            for (int p = 0; p < Incomecol.Length; ++p)
            {
                PdfPCell Incomecell = new PdfPCell(new Phrase(Incomecol[p], TableFontBoldItalic));
                Incomecell.HorizontalAlignment = Element.ALIGN_CENTER;
                Incomecell.FixedHeight = 15f;
                Incomecell.Border = iTextSharp.text.Rectangle.BOX;
                IncomeTable.AddCell(Incomecell);
            }

            //if (dsIncome.Tables.Count > 0)


            decimal Row_Prog_Income = 0; string interval = null, MemName = null, Income_Verifier = null;
            string IncomeDesc = null, privSeq = null;
            if (incomelist.Count > 0)
            {
                //DataView dv = dtCaseIncome.DefaultView;
                //dv.RowFilter = "INCOME_EXCLUDE = 'N'";
                //dv.Sort = "INCOME_FAMILY_SEQ ASC";
                //dtCaseIncome = dv.ToTable();
                dtIncome = dsIncome.Tables[0];
                foreach (CaseIncomeEntity drCaseIncome in incomelist)
                {
                    Income_Verifier = interval = IncomeDesc = string.Empty;
                    if (drCaseIncome.FamilySeq.Trim() != privSeq)
                    {
                        if (!string.IsNullOrEmpty(drCaseIncome.FamilySeq.Trim()))
                            MemName = Get_Member_Name(drCaseIncome.FamilySeq.Trim(), string.Empty);
                        PdfPCell House_HoldMem = new PdfPCell(new Phrase(MemName, TableFont));
                        House_HoldMem.Colspan = 5;
                        House_HoldMem.HorizontalAlignment = Element.ALIGN_LEFT;
                        House_HoldMem.FixedHeight = 15f;
                        House_HoldMem.Border = iTextSharp.text.Rectangle.BOX;
                        IncomeTable.AddCell(House_HoldMem);
                        privSeq = drCaseIncome.FamilySeq.Trim();
                    }
                    PdfPCell Income_Space = new PdfPCell(new Phrase("", TableFont));
                    Income_Space.Colspan = 1;
                    Income_Space.HorizontalAlignment = Element.ALIGN_CENTER;
                    Income_Space.FixedHeight = 15f;
                    Income_Space.Border = iTextSharp.text.Rectangle.BOX;
                    IncomeTable.AddCell(Income_Space);

                    if (!string.IsNullOrEmpty(drCaseIncome.ProgIncome.Trim()))
                        Row_Prog_Income = decimal.Parse(drCaseIncome.ProgIncome.Trim());
                    PdfPCell Amount = new PdfPCell(new Phrase(Row_Prog_Income.ToString(), TableFont));
                    Amount.HorizontalAlignment = Element.ALIGN_CENTER;
                    Amount.FixedHeight = 15f;
                    Amount.Border = iTextSharp.text.Rectangle.BOX;
                    IncomeTable.AddCell(Amount);

                    if (!string.IsNullOrEmpty(drCaseIncome.Interval.Trim()))
                        interval = LookupDataAccess.ShowIncomeInterval(drCaseIncome.Interval.Trim());
                    PdfPCell Freq = new PdfPCell(new Phrase(interval, TableFont));
                    Freq.HorizontalAlignment = Element.ALIGN_LEFT;
                    Freq.FixedHeight = 15f;
                    Freq.Border = iTextSharp.text.Rectangle.BOX;
                    IncomeTable.AddCell(Freq);

                    if (!string.IsNullOrEmpty(drCaseIncome.Type.Trim()))
                        IncomeDesc = Get_IncomeType_Desc(drCaseIncome.Type.Trim());

                    PdfPCell IncomeSource = new PdfPCell(new Phrase(IncomeDesc, TableFont));
                    IncomeSource.HorizontalAlignment = Element.ALIGN_CENTER;
                    IncomeSource.FixedHeight = 15f;
                    IncomeSource.Border = iTextSharp.text.Rectangle.BOX;
                    IncomeTable.AddCell(IncomeSource);

                    if (!string.IsNullOrEmpty(drCaseIncome.HowVerified.Trim()))
                        Income_Verifier = drCaseIncome.HowVerified.Trim();

                    PdfPCell IncomeVer = new PdfPCell(new Phrase(Income_Verifier, TableFont));
                    IncomeVer.HorizontalAlignment = Element.ALIGN_LEFT;
                    IncomeVer.FixedHeight = 15f;
                    IncomeVer.Border = iTextSharp.text.Rectangle.BOX;
                    IncomeTable.AddCell(IncomeVer);

                }
            }

            PdfPTable TotIncomeTable = new PdfPTable(8);
            TotIncomeTable.TotalWidth = 750f;
            TotIncomeTable.WidthPercentage = 100;
            TotIncomeTable.LockedWidth = true;
            float[] TotIncomeTablewidths = new float[] { 50f, 30f, 50f, 40f, 60f, 30f, 50f, 30f };
            TotIncomeTable.SetWidths(TotIncomeTablewidths);
            TotIncomeTable.HorizontalAlignment = Element.ALIGN_CENTER;
            //IncomeTable.SpacingBefore = 60f;

            PdfPCell Total_Space = new PdfPCell(new Phrase("", TableFont));
            Total_Space.Colspan = 8;
            Total_Space.HorizontalAlignment = Element.ALIGN_LEFT;
            Total_Space.FixedHeight = 15f;
            Total_Space.Border = iTextSharp.text.Rectangle.BOX;
            TotIncomeTable.AddCell(Total_Space);

            PdfPCell Total = new PdfPCell(new Phrase("Total Income", TableFont));
            Total.HorizontalAlignment = Element.ALIGN_LEFT;
            Total.FixedHeight = 15f;
            Total.Border = iTextSharp.text.Rectangle.BOX;
            TotIncomeTable.AddCell(Total);

            PdfPCell Total_Income = new PdfPCell(new Phrase(CASEMSTList[0].FamIncome.Trim(), TableFont));
            Total_Income.HorizontalAlignment = Element.ALIGN_CENTER;
            Total_Income.FixedHeight = 15f;
            Total_Income.Border = iTextSharp.text.Rectangle.BOX;
            TotIncomeTable.AddCell(Total_Income);

            PdfPCell Program = new PdfPCell(new Phrase("Program Income", TableFont));
            Program.HorizontalAlignment = Element.ALIGN_LEFT;
            Program.FixedHeight = 15f;
            Program.Border = iTextSharp.text.Rectangle.BOX;
            TotIncomeTable.AddCell(Program);

            PdfPCell Program_Income = new PdfPCell(new Phrase(CASEMSTList[0].ProgIncome.Trim(), TableFont));
            Program_Income.HorizontalAlignment = Element.ALIGN_CENTER;
            Program_Income.FixedHeight = 15f;
            Program_Income.Border = iTextSharp.text.Rectangle.BOX;
            TotIncomeTable.AddCell(Program_Income);

            PdfPCell Federal = new PdfPCell(new Phrase("% of Federal Poverty Level", TableFont));
            Federal.HorizontalAlignment = Element.ALIGN_LEFT;
            Federal.FixedHeight = 15f;
            Federal.Border = iTextSharp.text.Rectangle.BOX;
            TotIncomeTable.AddCell(Federal);

            PdfPCell Federal_Poverty = new PdfPCell(new Phrase(CASEMSTList[0].Poverty.Trim() + "%", TableFont));
            Federal_Poverty.HorizontalAlignment = Element.ALIGN_CENTER;
            Federal_Poverty.FixedHeight = 15f;
            Federal_Poverty.Border = iTextSharp.text.Rectangle.BOX;
            TotIncomeTable.AddCell(Federal_Poverty);

            PdfPCell HUD = new PdfPCell(new Phrase("HUD%", TableFont));
            HUD.HorizontalAlignment = Element.ALIGN_LEFT;
            HUD.FixedHeight = 15f;
            HUD.Border = iTextSharp.text.Rectangle.BOX;
            TotIncomeTable.AddCell(HUD);

            PdfPCell HUD_Value = new PdfPCell(new Phrase(CASEMSTList[0].Hud.Trim() + "%", TableFont));
            HUD_Value.HorizontalAlignment = Element.ALIGN_CENTER;
            HUD_Value.FixedHeight = 15f;
            HUD_Value.Border = iTextSharp.text.Rectangle.BOX;
            TotIncomeTable.AddCell(HUD_Value);

            document.Add(IncomeTable);
            document.Add(TotIncomeTable);

            #endregion

            #region Income Verification

            iTextSharp.text.Image _image_UnChecked = iTextSharp.text.Image.GetInstance(Consts.Icons.ico_checkbox);
            iTextSharp.text.Image _image_Checked = iTextSharp.text.Image.GetInstance(Consts.Icons.ico_Checked_checkbox);

            _image_UnChecked.ScalePercent(60f);
            _image_Checked.ScalePercent(60f);


            PdfPTable Income_Last = new PdfPTable(15);
            Income_Last.TotalWidth = 750f;
            Income_Last.WidthPercentage = 100;
            Income_Last.LockedWidth = true;
            float[] Income_Lastwidths = new float[] { 20f, 8f, 13f, 8f, 20f, 8f, 25f, 8f, 18f, 8f, 15f, 25f, 20f, 13f, 30f };
            Income_Last.SetWidths(Income_Lastwidths);
            Income_Last.HorizontalAlignment = Element.ALIGN_CENTER;
            Income_Last.SpacingBefore = 20f;

            PdfPCell Income_Verified = new PdfPCell(new Phrase("Income Verified", TableFontBoldItalic));
            Income_Verified.HorizontalAlignment = Element.ALIGN_LEFT;
            Income_Verified.FixedHeight = 15f;
            Income_Verified.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Income_Last.AddCell(Income_Verified);

            if (CASEMSTList[0].VerifyW2.Trim() == "Y")
            {
                PdfPCell W2Cheked = new PdfPCell(_image_Checked);
                W2Cheked.VerticalAlignment = Element.ALIGN_MIDDLE;
                W2Cheked.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                //W2Cheked.FixedHeight = 15f;
                W2Cheked.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Income_Last.AddCell(W2Cheked);
            }
            else
            {
                PdfPCell W2UnCheked = new PdfPCell(_image_UnChecked);
                W2UnCheked.VerticalAlignment = Element.ALIGN_MIDDLE;
                W2UnCheked.HorizontalAlignment = Element.ALIGN_RIGHT;
                //W2UnCheked.FixedHeight = 15f;
                W2UnCheked.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Income_Last.AddCell(W2UnCheked);
            }

            PdfPCell W2Text = new PdfPCell(new Phrase("W2", Times));
            W2Text.HorizontalAlignment = Element.ALIGN_LEFT;
            W2Text.FixedHeight = 15f;
            W2Text.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Income_Last.AddCell(W2Text);

            if (CASEMSTList[0].VerifyCheckStub.Trim() == "Y")
            {
                PdfPCell CHECK_STUB_Check = new PdfPCell(_image_Checked);
                CHECK_STUB_Check.VerticalAlignment = Element.ALIGN_MIDDLE;
                CHECK_STUB_Check.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                //W2Cheked.FixedHeight = 15f;
                CHECK_STUB_Check.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Income_Last.AddCell(CHECK_STUB_Check);
            }
            else
            {
                PdfPCell CHECK_STUB_UnCheked = new PdfPCell(_image_UnChecked);
                CHECK_STUB_UnCheked.VerticalAlignment = Element.ALIGN_MIDDLE;
                CHECK_STUB_UnCheked.HorizontalAlignment = Element.ALIGN_RIGHT;
                //W2UnCheked.FixedHeight = 15f;
                CHECK_STUB_UnCheked.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Income_Last.AddCell(CHECK_STUB_UnCheked);
            }

            PdfPCell CHECK_STUB = new PdfPCell(new Phrase("CHECK_STUB", Times));
            CHECK_STUB.HorizontalAlignment = Element.ALIGN_LEFT;
            CHECK_STUB.FixedHeight = 15f;
            CHECK_STUB.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Income_Last.AddCell(CHECK_STUB);

            if (CASEMSTList[0].VerifyLetter.Trim() == "Y")
            {
                PdfPCell LETTER_Check = new PdfPCell(_image_Checked);
                LETTER_Check.VerticalAlignment = Element.ALIGN_MIDDLE;
                LETTER_Check.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                //W2Cheked.FixedHeight = 15f;
                LETTER_Check.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Income_Last.AddCell(LETTER_Check);
            }
            else
            {
                PdfPCell LETTER_UnCheked = new PdfPCell(_image_UnChecked);
                LETTER_UnCheked.VerticalAlignment = Element.ALIGN_MIDDLE;
                LETTER_UnCheked.HorizontalAlignment = Element.ALIGN_RIGHT;
                //W2UnCheked.FixedHeight = 15f;
                LETTER_UnCheked.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Income_Last.AddCell(LETTER_UnCheked);
            }

            PdfPCell LETTER = new PdfPCell(new Phrase("Letter/Budget Sheet", Times));
            LETTER.HorizontalAlignment = Element.ALIGN_LEFT;
            LETTER.FixedHeight = 15f;
            LETTER.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Income_Last.AddCell(LETTER);

            if (CASEMSTList[0].VerifyTaxReturn.Trim() == "Y")
            {
                PdfPCell TAX_RETURN_Check = new PdfPCell(_image_Checked);
                TAX_RETURN_Check.VerticalAlignment = Element.ALIGN_MIDDLE;
                TAX_RETURN_Check.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                //W2Cheked.FixedHeight = 15f;
                TAX_RETURN_Check.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Income_Last.AddCell(TAX_RETURN_Check);
            }
            else
            {
                PdfPCell TAX_RETURN_UnCheked = new PdfPCell(_image_UnChecked);
                TAX_RETURN_UnCheked.VerticalAlignment = Element.ALIGN_MIDDLE;
                TAX_RETURN_UnCheked.HorizontalAlignment = Element.ALIGN_RIGHT;
                //W2UnCheked.FixedHeight = 15f;
                TAX_RETURN_UnCheked.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Income_Last.AddCell(TAX_RETURN_UnCheked);
            }

            PdfPCell TAX_RETURN = new PdfPCell(new Phrase("Tax Returns", Times));
            TAX_RETURN.HorizontalAlignment = Element.ALIGN_LEFT;
            TAX_RETURN.FixedHeight = 15f;
            TAX_RETURN.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Income_Last.AddCell(TAX_RETURN);

            if (CASEMSTList[0].VerifyOther.Trim() == "Y")
            {
                PdfPCell OTHER_Check = new PdfPCell(_image_Checked);
                OTHER_Check.VerticalAlignment = Element.ALIGN_MIDDLE;
                OTHER_Check.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                //W2Cheked.FixedHeight = 15f;
                OTHER_Check.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Income_Last.AddCell(OTHER_Check);
            }
            else
            {
                PdfPCell OTHER_UnCheked = new PdfPCell(_image_UnChecked);
                OTHER_UnCheked.VerticalAlignment = Element.ALIGN_MIDDLE;
                OTHER_UnCheked.HorizontalAlignment = Element.ALIGN_RIGHT;
                //W2UnCheked.FixedHeight = 15f;
                OTHER_UnCheked.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Income_Last.AddCell(OTHER_UnCheked);
            }

            PdfPCell OTHER = new PdfPCell(new Phrase("Other", Times));
            OTHER.HorizontalAlignment = Element.ALIGN_LEFT;
            OTHER.FixedHeight = 15f;
            OTHER.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Income_Last.AddCell(OTHER);

            PdfPCell Verification_Date = new PdfPCell(new Phrase("Verification Date:", Times));
            Verification_Date.HorizontalAlignment = Element.ALIGN_LEFT;
            Verification_Date.FixedHeight = 15f;
            Verification_Date.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Income_Last.AddCell(Verification_Date);

            if (!string.IsNullOrEmpty(CASEMSTList[0].EligDate.Trim().Trim()))
            {
                PdfPCell Verify_Date = new PdfPCell(new Phrase(LookupDataAccess.Getdate(CASEMSTList[0].EligDate.Trim().Trim()), Timesline));
                Verify_Date.HorizontalAlignment = Element.ALIGN_LEFT;
                Verify_Date.FixedHeight = 15f;
                Verify_Date.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Income_Last.AddCell(Verify_Date);
            }
            else
            {
                PdfPCell Verify_Date_Space = new PdfPCell(new Phrase("________________", Times));
                Verify_Date_Space.VerticalAlignment = PdfPCell.BOTTOM_BORDER;
                Verify_Date_Space.HorizontalAlignment = Element.ALIGN_LEFT;
                Verify_Date_Space.FixedHeight = 15f;
                Verify_Date_Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Income_Last.AddCell(Verify_Date_Space);
            }

            PdfPCell Verifier_Head = new PdfPCell(new Phrase("Verifier:", Times));
            Verifier_Head.HorizontalAlignment = Element.ALIGN_LEFT;
            Verifier_Head.FixedHeight = 15f;
            Verifier_Head.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Income_Last.AddCell(Verifier_Head);

            string CaseWorker = null;
            if (dtVerifier.Rows.Count > 0)
            {
                foreach (DataRow drVerifier in dtVerifier.Rows)
                {
                    if (CASEMSTList[0].Verifier.Trim().Trim() == drVerifier["PWH_CASEWORKER"].ToString().Trim())
                    {
                        CaseWorker = drVerifier["NAME"].ToString().Trim();
                        break;
                    }
                }
            }

            if (!string.IsNullOrEmpty(CaseWorker))
            {
                PdfPCell Verifier = new PdfPCell(new Phrase(CaseWorker, Timesline));
                Verifier.HorizontalAlignment = Element.ALIGN_LEFT;
                Verifier.FixedHeight = 15f;
                Verifier.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Income_Last.AddCell(Verifier);
            }
            else
            {
                PdfPCell Verifier_Space = new PdfPCell(new Phrase("_______________________________", Times));
                Verifier_Space.VerticalAlignment = PdfPCell.BOTTOM_BORDER;
                Verifier_Space.HorizontalAlignment = Element.ALIGN_LEFT;
                Verifier_Space.FixedHeight = 15f;
                Verifier_Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Income_Last.AddCell(Verifier_Space);
            }



            document.Add(Income_Last);


            #endregion

            #region LastIncome Table Not Used

            //PdfPTable Income_Last = new PdfPTable(15);
            //Income_Last.TotalWidth = 750f;
            //Income_Last.WidthPercentage = 100;
            //Income_Last.LockedWidth = true;
            //float[] Income_Lastwidths = new float[] { 20f, 8f, 13f, 8f, 20f, 8f, 25f, 8f, 18f, 8f, 15f, 25f, 20f, 13f, 30f };
            //Income_Last.SetWidths(Income_Lastwidths);
            //Income_Last.HorizontalAlignment = Element.ALIGN_CENTER;
            //Income_Last.SpacingBefore = 20f;


            //iTextSharp.text.Image _image_UnChecked = iTextSharp.text.Image.GetInstance(Context.Server.MapPath("~\\Resources\\Icons\\16X16\\CheckBoxUnchecked.JPG"));
            //iTextSharp.text.Image _image_Checked = iTextSharp.text.Image.GetInstance(Context.Server.MapPath("~\\Resources\\Icons\\16X16\\CheckBoxChecked.JPG"));

            //_image_UnChecked.ScalePercent(60f);
            //_image_Checked.ScalePercent(60f);


            //PdfPCell Income_Verified = new PdfPCell(new Phrase("Income Verified", TableFontBoldItalic));
            //Income_Verified.HorizontalAlignment = Element.ALIGN_LEFT;
            //Income_Verified.FixedHeight = 15f;
            //Income_Verified.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //Income_Last.AddCell(Income_Verified);

            //if (drCaseMST["MST_VERIFY_W2"].ToString().Trim() == "Y")
            //{
            //    PdfPCell W2Cheked = new PdfPCell(_image_Checked);
            //    W2Cheked.VerticalAlignment = Element.ALIGN_MIDDLE;
            //    W2Cheked.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
            //    //W2Cheked.FixedHeight = 15f;
            //    W2Cheked.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //    Income_Last.AddCell(W2Cheked);
            //}
            //else
            //{
            //    PdfPCell W2UnCheked = new PdfPCell(_image_UnChecked);
            //    W2UnCheked.VerticalAlignment = Element.ALIGN_MIDDLE;
            //    W2UnCheked.HorizontalAlignment = Element.ALIGN_RIGHT;
            //    //W2UnCheked.FixedHeight = 15f;
            //    W2UnCheked.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //    Income_Last.AddCell(W2UnCheked);
            //}

            //PdfPCell W2Text = new PdfPCell(new Phrase("W2", Times));
            //W2Text.HorizontalAlignment = Element.ALIGN_LEFT;
            //W2Text.FixedHeight = 15f;
            //W2Text.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //Income_Last.AddCell(W2Text);

            //if (drCaseMST["MST_VERIFY_CHECK_STUB"].ToString().Trim() == "Y")
            //{
            //    PdfPCell CHECK_STUB_Check = new PdfPCell(_image_Checked);
            //    CHECK_STUB_Check.VerticalAlignment = Element.ALIGN_MIDDLE;
            //    CHECK_STUB_Check.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
            //    //W2Cheked.FixedHeight = 15f;
            //    CHECK_STUB_Check.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //    Income_Last.AddCell(CHECK_STUB_Check);
            //}
            //else
            //{
            //    PdfPCell CHECK_STUB_UnCheked = new PdfPCell(_image_UnChecked);
            //    CHECK_STUB_UnCheked.VerticalAlignment = Element.ALIGN_MIDDLE;
            //    CHECK_STUB_UnCheked.HorizontalAlignment = Element.ALIGN_RIGHT;
            //    //W2UnCheked.FixedHeight = 15f;
            //    CHECK_STUB_UnCheked.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //    Income_Last.AddCell(CHECK_STUB_UnCheked);
            //}

            //PdfPCell CHECK_STUB = new PdfPCell(new Phrase("CHECK_STUB", Times));
            //CHECK_STUB.HorizontalAlignment = Element.ALIGN_LEFT;
            //CHECK_STUB.FixedHeight = 15f;
            //CHECK_STUB.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //Income_Last.AddCell(CHECK_STUB);

            //if (drCaseMST["MST_VERIFY_LETTER"].ToString().Trim() == "Y")
            //{
            //    PdfPCell LETTER_Check = new PdfPCell(_image_Checked);
            //    LETTER_Check.VerticalAlignment = Element.ALIGN_MIDDLE;
            //    LETTER_Check.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
            //    //W2Cheked.FixedHeight = 15f;
            //    LETTER_Check.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //    Income_Last.AddCell(LETTER_Check);
            //}
            //else
            //{
            //    PdfPCell LETTER_UnCheked = new PdfPCell(_image_UnChecked);
            //    LETTER_UnCheked.VerticalAlignment = Element.ALIGN_MIDDLE;
            //    LETTER_UnCheked.HorizontalAlignment = Element.ALIGN_RIGHT;
            //    //W2UnCheked.FixedHeight = 15f;
            //    LETTER_UnCheked.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //    Income_Last.AddCell(LETTER_UnCheked);
            //}

            //PdfPCell LETTER = new PdfPCell(new Phrase("Letter/Budget Sheet", Times));
            //LETTER.HorizontalAlignment = Element.ALIGN_LEFT;
            //LETTER.FixedHeight = 15f;
            //LETTER.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //Income_Last.AddCell(LETTER);

            //if (drCaseMST["MST_VERIFY_TAX_RETURN"].ToString().Trim() == "Y")
            //{
            //    PdfPCell TAX_RETURN_Check = new PdfPCell(_image_Checked);
            //    TAX_RETURN_Check.VerticalAlignment = Element.ALIGN_MIDDLE;
            //    TAX_RETURN_Check.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
            //    //W2Cheked.FixedHeight = 15f;
            //    TAX_RETURN_Check.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //    Income_Last.AddCell(TAX_RETURN_Check);
            //}
            //else
            //{
            //    PdfPCell TAX_RETURN_UnCheked = new PdfPCell(_image_UnChecked);
            //    TAX_RETURN_UnCheked.VerticalAlignment = Element.ALIGN_MIDDLE;
            //    TAX_RETURN_UnCheked.HorizontalAlignment = Element.ALIGN_RIGHT;
            //    //W2UnCheked.FixedHeight = 15f;
            //    TAX_RETURN_UnCheked.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //    Income_Last.AddCell(TAX_RETURN_UnCheked);
            //}

            //PdfPCell TAX_RETURN = new PdfPCell(new Phrase("Tax Returns", Times));
            //TAX_RETURN.HorizontalAlignment = Element.ALIGN_LEFT;
            //TAX_RETURN.FixedHeight = 15f;
            //TAX_RETURN.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //Income_Last.AddCell(TAX_RETURN);

            //if (drCaseMST["MST_VERIFY_OTHER"].ToString().Trim() == "Y")
            //{
            //    PdfPCell OTHER_Check = new PdfPCell(_image_Checked);
            //    OTHER_Check.VerticalAlignment = Element.ALIGN_MIDDLE;
            //    OTHER_Check.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
            //    //W2Cheked.FixedHeight = 15f;
            //    OTHER_Check.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //    Income_Last.AddCell(OTHER_Check);
            //}
            //else
            //{
            //    PdfPCell OTHER_UnCheked = new PdfPCell(_image_UnChecked);
            //    OTHER_UnCheked.VerticalAlignment = Element.ALIGN_MIDDLE;
            //    OTHER_UnCheked.HorizontalAlignment = Element.ALIGN_RIGHT;
            //    //W2UnCheked.FixedHeight = 15f;
            //    OTHER_UnCheked.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //    Income_Last.AddCell(OTHER_UnCheked);
            //}

            //PdfPCell OTHER = new PdfPCell(new Phrase("Other", Times));
            //OTHER.HorizontalAlignment = Element.ALIGN_LEFT;
            //OTHER.FixedHeight = 15f;
            //OTHER.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //Income_Last.AddCell(OTHER);

            //PdfPCell Verification_Date = new PdfPCell(new Phrase("Verification Date:", Times));
            //Verification_Date.HorizontalAlignment = Element.ALIGN_LEFT;
            //Verification_Date.FixedHeight = 15f;
            //Verification_Date.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //Income_Last.AddCell(Verification_Date);

            //if (!string.IsNullOrEmpty(drCaseMST["MST_ELIG_DATE"].ToString().Trim()))
            //{
            //    PdfPCell Verify_Date = new PdfPCell(new Phrase(drCaseMST["MST_ELIG_DATE"].ToString().Trim(), Timesline));
            //    Verify_Date.HorizontalAlignment = Element.ALIGN_LEFT;
            //    Verify_Date.FixedHeight = 15f;
            //    Verify_Date.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //    Income_Last.AddCell(Verify_Date);
            //}
            //else
            //{
            //    PdfPCell Verify_Date_Space = new PdfPCell(new Phrase("________________", Times));
            //    Verify_Date_Space.VerticalAlignment = PdfPCell.BOTTOM_BORDER;
            //    Verify_Date_Space.HorizontalAlignment = Element.ALIGN_LEFT;
            //    Verify_Date_Space.FixedHeight = 15f;
            //    Verify_Date_Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //    Income_Last.AddCell(Verify_Date_Space);
            //}

            //PdfPCell Verifier_Head = new PdfPCell(new Phrase("Verifier:", Times));
            //Verifier_Head.HorizontalAlignment = Element.ALIGN_LEFT;
            //Verifier_Head.FixedHeight = 15f;
            //Verifier_Head.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //Income_Last.AddCell(Verifier_Head);

            //string CaseWorker = null;
            //if (dtVerifier.Rows.Count > 0)
            //{
            //    foreach (DataRow drVerifier in dtVerifier.Rows)
            //    {
            //        if (drCaseMST["MST_VERIFIER"].ToString().Trim() == drVerifier["PWH_CASEWORKER"].ToString().Trim())
            //        {
            //            CaseWorker = drVerifier["NAME"].ToString().Trim();
            //            break;
            //        }
            //    }
            //}

            //if (!string.IsNullOrEmpty(CaseWorker))
            //{
            //    PdfPCell Verifier = new PdfPCell(new Phrase(CaseWorker, Timesline));
            //    Verifier.HorizontalAlignment = Element.ALIGN_LEFT;
            //    Verifier.FixedHeight = 15f;
            //    Verifier.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //    Income_Last.AddCell(Verifier);
            //}
            //else
            //{
            //    PdfPCell Verifier_Space = new PdfPCell(new Phrase("_______________________________", Times));
            //    Verifier_Space.VerticalAlignment = PdfPCell.BOTTOM_BORDER;
            //    Verifier_Space.HorizontalAlignment = Element.ALIGN_LEFT;
            //    Verifier_Space.FixedHeight = 15f;
            //    Verifier_Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //    Income_Last.AddCell(Verifier_Space);
            //}

            //if (ShortName != "UETHDA")
            //{
            //    PdfPCell V_Space = new PdfPCell(new Phrase("", Times));
            //    V_Space.VerticalAlignment = PdfPCell.BOTTOM_BORDER;
            //    V_Space.HorizontalAlignment = Element.ALIGN_LEFT;
            //    V_Space.Colspan = 15;
            //    V_Space.FixedHeight = 15f;
            //    V_Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //    Income_Last.AddCell(V_Space);

            //    PdfPCell P_Ver = new PdfPCell(new Phrase("Parent Verification ______________________________________________________________________________", Times));
            //    P_Ver.VerticalAlignment = PdfPCell.BOTTOM_BORDER;
            //    P_Ver.HorizontalAlignment = Element.ALIGN_LEFT;
            //    P_Ver.Colspan = 11;
            //    P_Ver.FixedHeight = 15f;
            //    P_Ver.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //    Income_Last.AddCell(P_Ver);

            //    PdfPCell P_Ver1 = new PdfPCell(new Phrase("Reverify Date: ________________", Times));
            //    P_Ver1.VerticalAlignment = PdfPCell.BOTTOM_BORDER;
            //    P_Ver1.HorizontalAlignment = Element.ALIGN_LEFT;
            //    P_Ver1.Colspan = 2;
            //    P_Ver1.FixedHeight = 15f;
            //    P_Ver1.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //    Income_Last.AddCell(P_Ver1);

            //    //PdfPCell Ver_Date_Space = new PdfPCell(new Phrase("________________", Times));
            //    //Ver_Date_Space.VerticalAlignment = PdfPCell.BOTTOM_BORDER;
            //    //Ver_Date_Space.HorizontalAlignment = Element.ALIGN_LEFT;
            //    //Ver_Date_Space.FixedHeight = 15f;
            //    //Ver_Date_Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //    //Income_Last.AddCell(Ver_Date_Space);

            //    PdfPCell P_Ver2 = new PdfPCell(new Phrase("Verifier ", Times));
            //    P_Ver2.VerticalAlignment = PdfPCell.BOTTOM_BORDER;
            //    P_Ver2.HorizontalAlignment = Element.ALIGN_LEFT;
            //    //P_Ver2.Colspan = 4;
            //    P_Ver2.FixedHeight = 15f;
            //    P_Ver2.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //    Income_Last.AddCell(P_Ver2);

            //    PdfPCell Ver_Space = new PdfPCell(new Phrase("_______________________________", Times));
            //    Ver_Space.VerticalAlignment = PdfPCell.BOTTOM_BORDER;
            //    Ver_Space.HorizontalAlignment = Element.ALIGN_LEFT;
            //    Ver_Space.FixedHeight = 15f;
            //    Ver_Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //    Income_Last.AddCell(Ver_Space);
            //}

            //End of Income Details

            #endregion

            //#region Start of Energy Calculations
            //PdfPTable Energy = new PdfPTable(3);
            //Energy.TotalWidth = 750f;
            //Energy.WidthPercentage = 100;
            //Energy.LockedWidth = true;
            //float[] Energy_widths = new float[] { 60, 60f, 60f };
            //Energy.SetWidths(Energy_widths);
            //Energy.HorizontalAlignment = Element.ALIGN_CENTER;
            //Energy.SpacingBefore = 20f;


            //if (Privileges.ModuleCode == "05" || gvApp.CurrentRow.Cells["AppDet"].Value.ToString() == "Application for Assistance" || DEPState == "TX")
            //{
            //    PdfPCell Compute = new PdfPCell(new Phrase("Compute Energy Burden", TblFontBold));
            //    Compute.HorizontalAlignment = Element.ALIGN_LEFT;
            //    Compute.FixedHeight = 15f;
            //    Compute.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //    Energy.AddCell(Compute);

            //    PdfPCell Compute_Space = new PdfPCell(new Phrase("", TableFontBoldItalic));
            //    Compute_Space.Colspan = 2;
            //    Compute_Space.HorizontalAlignment = Element.ALIGN_LEFT;
            //    Compute_Space.FixedHeight = 15f;
            //    Compute_Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //    Energy.AddCell(Compute_Space);

            //    decimal UtilityCost = 0; decimal EnergyBurden = 0;
            //    if (ADDCUST_List.Count > 0)
            //    {
            //        AddCustEntity Custdet = ADDCUST_List.Find(u => u.ACTCODE.Equals("C00012") && u.ACTAPPNO.Equals(BaseForm.BaseApplicationNo));
            //        if (Custdet != null)
            //        {
            //            if (!string.IsNullOrEmpty(Custdet.ACTNUMRESP.Trim()))
            //                UtilityCost = decimal.Parse(Custdet.ACTNUMRESP.Trim());
            //        }
            //    }

            //    PdfPCell Utility_Costs = new PdfPCell(new Phrase("Utility Costs: " + UtilityCost.ToString("0.00"), Times));
            //    Utility_Costs.HorizontalAlignment = Element.ALIGN_LEFT;
            //    Utility_Costs.FixedHeight = 15f;
            //    Utility_Costs.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //    Energy.AddCell(Utility_Costs);

            //    PdfPCell Annual_Income = new PdfPCell(new Phrase("Annual Income Total:" + drCaseMST["MST_FAM_INCOME"].ToString().Trim(), Times));
            //    Annual_Income.HorizontalAlignment = Element.ALIGN_CENTER;
            //    Annual_Income.FixedHeight = 15f;
            //    Annual_Income.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //    Energy.AddCell(Annual_Income);

            //    if (decimal.Parse(drCaseMST["MST_FAM_INCOME"].ToString().Trim()) > 0 && UtilityCost > 0)
            //        EnergyBurden = (UtilityCost / decimal.Parse(drCaseMST["MST_FAM_INCOME"].ToString().Trim())) * 100;

            //    PdfPCell Energy_Burden = new PdfPCell(new Phrase("Energy Burden: " + EnergyBurden.ToString("0.00") + "%", Times));
            //    Energy_Burden.HorizontalAlignment = Element.ALIGN_CENTER;
            //    Energy_Burden.FixedHeight = 15f;
            //    Energy_Burden.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //    Energy.AddCell(Energy_Burden);
            //}

            //#endregion


            //cb.BeginText();
            //X_Pos = 400; Y_Pos = 580;
            //cb.SetFontAndSize(bf_helv, 13);
            //cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, Header_Desc, X_Pos, Y_Pos, 0);
            //cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, Form_Selection, X_Pos, Y_Pos - 15, 0);
            //cb.SetFontAndSize(bf_helv, 9);
            //cb.EndText();

            //cb.BeginText();
            //X_Pos = 400; Y_Pos = 580;
            //cb.SetFontAndSize(bf_helv, 13);
            //cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, Header_Desc, X_Pos, Y_Pos, 0);

            //cb.SetFontAndSize(bf_helv, 9);
            //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Applicant No: ", 30, Y_Pos - 15, 0);
            //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(BaseForm.BaseApplicationNo, Timesline), 30 + 72, Y_Pos - 15, 0);

            //cb.SetFontAndSize(bf_helv, 13);
            //cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, Form_Selection, X_Pos, Y_Pos - 15, 0);
            //cb.SetFontAndSize(bf_helv, 9);
            //cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "Date Printed: ", 740, Y_Pos - 15, 0);
            //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(LookupDataAccess.Getdate(DateTime.Now.ToShortDateString()), Times), 780, Y_Pos - 15, 0);

            //X_Pos = 30; Y_Pos -= 30;
            //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "Applicant Name   ", X_Pos, Y_Pos, 0);
            //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(BaseForm.BaseApplicationName, Timesline), X_Pos + 72, Y_Pos, 0);

            //cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, "Application Date: ", 740, Y_Pos, 0);
            //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_RIGHT, new Phrase(LookupDataAccess.Getdate(drCaseMST["MST_INTAKE_DATE"].ToString().Trim()), Timesline), 780, Y_Pos, 0);

            //cb.EndText();

            //PdfPTable Spacetable = new PdfPTable(1);
            //Spacetable.HorizontalAlignment = Element.ALIGN_CENTER;
            //Spacetable.TotalWidth = 600f;

            //document.Add(Spacetable);



            //#endregion

            //#region Health Insurance Information

            //PdfPTable InsuranceTable = new PdfPTable(5);
            //InsuranceTable.TotalWidth = 750f;
            //InsuranceTable.WidthPercentage = 100;
            //InsuranceTable.LockedWidth = true;
            //float[] InsuranceTable_widths = new float[] { 25f, 30f, 45f, 60f, 25f };
            //InsuranceTable.SetWidths(InsuranceTable_widths);
            //InsuranceTable.HorizontalAlignment = Element.ALIGN_CENTER;
            //InsuranceTable.SpacingBefore = 10f;

            //PdfPCell INS_Head = new PdfPCell(new Phrase("HEALTH INSURANCE INFORMATION", TblFontBold));
            //INS_Head.Colspan = 5;
            //INS_Head.HorizontalAlignment = Element.ALIGN_LEFT;
            //INS_Head.FixedHeight = 15f;
            //INS_Head.Border = iTextSharp.text.Rectangle.BOX;
            //INS_Head.BorderWidth = 1f;
            //InsuranceTable.AddCell(INS_Head);

            //for (int i = 0; i < 4; i++)
            //{
            //    PdfPCell I1 = new PdfPCell(new Phrase("Name", TableFont));
            //    I1.HorizontalAlignment = Element.ALIGN_LEFT;
            //    I1.Colspan = 5;
            //    I1.FixedHeight = 15f;
            //    I1.BorderWidth = 1f;
            //    I1.Border = iTextSharp.text.Rectangle.BOX;
            //    InsuranceTable.AddCell(I1);

            //    if (chldMstDetails != null && i == 0)
            //    {
            //        PdfPCell I2 = new PdfPCell(new Phrase("Medical", TableFont));
            //        I2.HorizontalAlignment = Element.ALIGN_LEFT;
            //        I2.Rowspan = 2;
            //        I2.BorderWidth = 1f;
            //        I2.Border = iTextSharp.text.Rectangle.BOX;
            //        InsuranceTable.AddCell(I2);

            //        PdfPCell I3 = new PdfPCell(new Phrase("Insurance Plan", TableFont));
            //        I3.HorizontalAlignment = Element.ALIGN_LEFT;
            //        I3.BorderWidth = 1f;
            //        I3.Border = iTextSharp.text.Rectangle.BOX;
            //        InsuranceTable.AddCell(I3);

            //        PdfPCell I4 = new PdfPCell(new Phrase("Doctor Name", TableFont));
            //        I4.HorizontalAlignment = Element.ALIGN_LEFT;
            //        I4.BorderWidth = 1f;
            //        I4.Border = iTextSharp.text.Rectangle.BOX;
            //        InsuranceTable.AddCell(I4);

            //        PdfPCell I5 = new PdfPCell(new Phrase(chldMstDetails.DoctorAddress.Trim(), TableFont));
            //        I5.HorizontalAlignment = Element.ALIGN_LEFT;
            //        I5.BorderWidth = 1f;
            //        I5.Rowspan = 2;
            //        I5.Border = iTextSharp.text.Rectangle.BOX;
            //        InsuranceTable.AddCell(I5);

            //        PdfPCell I6 = new PdfPCell(new Phrase("Telephone", TableFont));
            //        I6.HorizontalAlignment = Element.ALIGN_LEFT;
            //        I6.BorderWidth = 1f;
            //        I6.Border = iTextSharp.text.Rectangle.BOX;
            //        InsuranceTable.AddCell(I6);

            //        if (!string.IsNullOrEmpty(chldMstDetails.MedPlan.Trim()))
            //        {
            //            PdfPCell I7 = new PdfPCell(new Phrase(chldMstDetails.MedPlan.Trim(), TableFont));
            //            I7.HorizontalAlignment = Element.ALIGN_LEFT;
            //            I7.BorderWidth = 1f;
            //            I7.Border = iTextSharp.text.Rectangle.BOX;
            //            InsuranceTable.AddCell(I7);
            //        }
            //        else
            //        {
            //            PdfPCell I7 = new PdfPCell(new Phrase("", TableFont));
            //            I7.HorizontalAlignment = Element.ALIGN_LEFT;
            //            I7.BorderWidth = 1f;
            //            I7.FixedHeight = 15f;
            //            I7.Border = iTextSharp.text.Rectangle.BOX;
            //            InsuranceTable.AddCell(I7);
            //        }

            //        if (!string.IsNullOrEmpty(chldMstDetails.DoctorName.Trim()))
            //        {
            //            PdfPCell I8 = new PdfPCell(new Phrase(chldMstDetails.DoctorName.Trim(), TableFont));
            //            I8.HorizontalAlignment = Element.ALIGN_LEFT;
            //            I8.BorderWidth = 1f;
            //            I8.Border = iTextSharp.text.Rectangle.BOX;
            //            InsuranceTable.AddCell(I8);
            //        }
            //        else
            //        {
            //            PdfPCell I8 = new PdfPCell(new Phrase("", TableFont));
            //            I8.HorizontalAlignment = Element.ALIGN_LEFT;
            //            I8.BorderWidth = 1f;
            //            I8.FixedHeight = 15f;
            //            I8.Border = iTextSharp.text.Rectangle.BOX;
            //            InsuranceTable.AddCell(I8);
            //        }

            //        if (!string.IsNullOrEmpty(chldMstDetails.DoctorPhone.Trim()))
            //        {
            //            PdfPCell I9 = new PdfPCell(new Phrase(chldMstDetails.DoctorPhone.Trim(), TableFont));
            //            I9.HorizontalAlignment = Element.ALIGN_LEFT;
            //            I9.BorderWidth = 1f;
            //            I9.Border = iTextSharp.text.Rectangle.BOX;
            //            InsuranceTable.AddCell(I9);
            //        }
            //        else
            //        {
            //            PdfPCell I9 = new PdfPCell(new Phrase("", TableFont));
            //            I9.HorizontalAlignment = Element.ALIGN_LEFT;
            //            I9.BorderWidth = 1f;
            //            I9.FixedHeight = 15f;
            //            I9.Border = iTextSharp.text.Rectangle.BOX;
            //            InsuranceTable.AddCell(I9);
            //        }

            //        PdfPCell D1 = new PdfPCell(new Phrase("Dental", TableFont));
            //        D1.HorizontalAlignment = Element.ALIGN_LEFT;
            //        D1.BorderWidth = 1f;
            //        D1.Rowspan = 2;
            //        D1.Border = iTextSharp.text.Rectangle.BOX;
            //        InsuranceTable.AddCell(D1);

            //        PdfPCell D2 = new PdfPCell(new Phrase("Insurance Plan", TableFont));
            //        D2.HorizontalAlignment = Element.ALIGN_LEFT;
            //        D2.BorderWidth = 1f;
            //        D2.Border = iTextSharp.text.Rectangle.BOX;
            //        InsuranceTable.AddCell(I3);

            //        PdfPCell D3 = new PdfPCell(new Phrase("Doctor Name", TableFont));
            //        D3.HorizontalAlignment = Element.ALIGN_LEFT;
            //        D3.BorderWidth = 1f;
            //        D3.Border = iTextSharp.text.Rectangle.BOX;
            //        InsuranceTable.AddCell(D3);

            //        PdfPCell D4 = new PdfPCell(new Phrase(chldMstDetails.DentistAddress.Trim(), TableFont));
            //        D4.HorizontalAlignment = Element.ALIGN_LEFT;
            //        D4.BorderWidth = 1f;
            //        D4.Rowspan = 2;
            //        D4.Border = iTextSharp.text.Rectangle.BOX;
            //        InsuranceTable.AddCell(D4);

            //        PdfPCell D5 = new PdfPCell(new Phrase("Telephone", TableFont));
            //        D5.HorizontalAlignment = Element.ALIGN_LEFT;
            //        D5.BorderWidth = 1f;
            //        D5.Border = iTextSharp.text.Rectangle.BOX;
            //        InsuranceTable.AddCell(D5);

            //        if (!string.IsNullOrEmpty(chldMstDetails.DentalPlan.Trim()))
            //        {
            //            PdfPCell D6 = new PdfPCell(new Phrase(chldMstDetails.DentalPlan.Trim(), TableFont));
            //            D6.HorizontalAlignment = Element.ALIGN_LEFT;
            //            D6.BorderWidth = 1f;
            //            D6.Border = iTextSharp.text.Rectangle.BOX;
            //            InsuranceTable.AddCell(D6);
            //        }
            //        else
            //        {
            //            PdfPCell D6 = new PdfPCell(new Phrase("", TableFont));
            //            D6.HorizontalAlignment = Element.ALIGN_LEFT;
            //            D6.BorderWidth = 1f;
            //            D6.FixedHeight = 15f;
            //            D6.Border = iTextSharp.text.Rectangle.BOX;
            //            InsuranceTable.AddCell(D6);
            //        }

            //        if (!string.IsNullOrEmpty(chldMstDetails.DentistName.Trim()))
            //        {
            //            PdfPCell D7 = new PdfPCell(new Phrase(chldMstDetails.DentistName.Trim(), TableFont));
            //            D7.HorizontalAlignment = Element.ALIGN_LEFT;
            //            D7.BorderWidth = 1f;
            //            D7.Border = iTextSharp.text.Rectangle.BOX;
            //            InsuranceTable.AddCell(D7);
            //        }
            //        else
            //        {
            //            PdfPCell D7 = new PdfPCell(new Phrase("", TableFont));
            //            D7.HorizontalAlignment = Element.ALIGN_LEFT;
            //            D7.BorderWidth = 1f;
            //            D7.FixedHeight = 15f;
            //            D7.Border = iTextSharp.text.Rectangle.BOX;
            //            InsuranceTable.AddCell(D7);
            //        }

            //        if (!string.IsNullOrEmpty(chldMstDetails.DentistPhone.Trim()))
            //        {
            //            PdfPCell D8 = new PdfPCell(new Phrase(chldMstDetails.DentistPhone.Trim(), TableFont));
            //            D8.HorizontalAlignment = Element.ALIGN_LEFT;
            //            D8.BorderWidth = 1f;
            //            D8.Border = iTextSharp.text.Rectangle.BOX;
            //            InsuranceTable.AddCell(D8);
            //        }
            //        else
            //        {
            //            PdfPCell D8 = new PdfPCell(new Phrase("", TableFont));
            //            D8.HorizontalAlignment = Element.ALIGN_LEFT;
            //            D8.BorderWidth = 1f;
            //            D8.FixedHeight = 15f;
            //            D8.Border = iTextSharp.text.Rectangle.BOX;
            //            InsuranceTable.AddCell(D8);
            //        }
            //    }
            //    else
            //    {
            //        PdfPCell I2 = new PdfPCell(new Phrase("Medical", TableFont));
            //        I2.HorizontalAlignment = Element.ALIGN_LEFT;
            //        I2.Rowspan = 2;
            //        I2.BorderWidth = 1f;
            //        I2.Border = iTextSharp.text.Rectangle.BOX;
            //        InsuranceTable.AddCell(I2);

            //        PdfPCell I3 = new PdfPCell(new Phrase("Insurance Plan", TableFont));
            //        I3.HorizontalAlignment = Element.ALIGN_LEFT;
            //        I3.BorderWidth = 1f;
            //        I3.Border = iTextSharp.text.Rectangle.BOX;
            //        InsuranceTable.AddCell(I3);

            //        PdfPCell I4 = new PdfPCell(new Phrase("Doctor Name", TableFont));
            //        I4.HorizontalAlignment = Element.ALIGN_LEFT;
            //        I4.BorderWidth = 1f;
            //        I4.Border = iTextSharp.text.Rectangle.BOX;
            //        InsuranceTable.AddCell(I4);

            //        PdfPCell I5 = new PdfPCell(new Phrase("", TableFont));
            //        I5.HorizontalAlignment = Element.ALIGN_LEFT;
            //        I5.BorderWidth = 1f;
            //        I5.Rowspan = 2;
            //        I5.Border = iTextSharp.text.Rectangle.BOX;
            //        InsuranceTable.AddCell(I5);

            //        PdfPCell I6 = new PdfPCell(new Phrase("Telephone", TableFont));
            //        I6.HorizontalAlignment = Element.ALIGN_LEFT;
            //        I6.BorderWidth = 1f;
            //        I6.Border = iTextSharp.text.Rectangle.BOX;
            //        InsuranceTable.AddCell(I6);

            //        PdfPCell I7 = new PdfPCell(new Phrase("", TableFont));
            //        I7.HorizontalAlignment = Element.ALIGN_LEFT;
            //        I7.BorderWidth = 1f;
            //        I7.FixedHeight = 15f;
            //        I7.Border = iTextSharp.text.Rectangle.BOX;
            //        InsuranceTable.AddCell(I7);

            //        PdfPCell I8 = new PdfPCell(new Phrase("", TableFont));
            //        I8.HorizontalAlignment = Element.ALIGN_LEFT;
            //        I8.BorderWidth = 1f;
            //        I8.FixedHeight = 15f;
            //        I8.Border = iTextSharp.text.Rectangle.BOX;
            //        InsuranceTable.AddCell(I8);

            //        PdfPCell I9 = new PdfPCell(new Phrase("", TableFont));
            //        I9.HorizontalAlignment = Element.ALIGN_LEFT;
            //        I9.BorderWidth = 1f;
            //        I9.FixedHeight = 15f;
            //        I9.Border = iTextSharp.text.Rectangle.BOX;
            //        InsuranceTable.AddCell(I9);


            //        PdfPCell D1 = new PdfPCell(new Phrase("Dental", TableFont));
            //        D1.HorizontalAlignment = Element.ALIGN_LEFT;
            //        D1.BorderWidth = 1f;
            //        D1.Rowspan = 2;
            //        D1.Border = iTextSharp.text.Rectangle.BOX;
            //        InsuranceTable.AddCell(D1);

            //        PdfPCell D2 = new PdfPCell(new Phrase("Insurance Plan", TableFont));
            //        D2.HorizontalAlignment = Element.ALIGN_LEFT;
            //        D2.BorderWidth = 1f;
            //        D2.Border = iTextSharp.text.Rectangle.BOX;
            //        InsuranceTable.AddCell(I3);

            //        PdfPCell D3 = new PdfPCell(new Phrase("Doctor Name", TableFont));
            //        D3.HorizontalAlignment = Element.ALIGN_LEFT;
            //        D3.BorderWidth = 1f;
            //        D3.Border = iTextSharp.text.Rectangle.BOX;
            //        InsuranceTable.AddCell(D3);

            //        PdfPCell D4 = new PdfPCell(new Phrase("", TableFont));
            //        D4.HorizontalAlignment = Element.ALIGN_LEFT;
            //        D4.BorderWidth = 1f;
            //        D4.Rowspan = 2;
            //        D4.Border = iTextSharp.text.Rectangle.BOX;
            //        InsuranceTable.AddCell(D4);

            //        PdfPCell D5 = new PdfPCell(new Phrase("Telephone", TableFont));
            //        D5.HorizontalAlignment = Element.ALIGN_LEFT;
            //        D5.BorderWidth = 1f;
            //        D5.Border = iTextSharp.text.Rectangle.BOX;
            //        InsuranceTable.AddCell(D5);

            //        PdfPCell D6 = new PdfPCell(new Phrase("", TableFont));
            //        D6.HorizontalAlignment = Element.ALIGN_LEFT;
            //        D6.BorderWidth = 1f;
            //        D6.FixedHeight = 15f;
            //        D6.Border = iTextSharp.text.Rectangle.BOX;
            //        InsuranceTable.AddCell(D6);

            //        PdfPCell D7 = new PdfPCell(new Phrase("", TableFont));
            //        D7.HorizontalAlignment = Element.ALIGN_LEFT;
            //        D7.BorderWidth = 1f;
            //        D7.FixedHeight = 15f;
            //        D7.Border = iTextSharp.text.Rectangle.BOX;
            //        InsuranceTable.AddCell(D7);

            //        PdfPCell D8 = new PdfPCell(new Phrase("", TableFont));
            //        D8.HorizontalAlignment = Element.ALIGN_LEFT;
            //        D8.BorderWidth = 1f;
            //        D8.FixedHeight = 15f;
            //        D8.Border = iTextSharp.text.Rectangle.BOX;
            //        InsuranceTable.AddCell(D8);
            //    }
            //}

            //document.Add(InsuranceTable);

            //#endregion

            //#region Mediacal data from chldmst

            ////PdfPTable MediaclTable = new PdfPTable(4);
            ////MediaclTable.TotalWidth = 750f;
            ////MediaclTable.WidthPercentage = 100;
            ////MediaclTable.LockedWidth = true;
            ////float[] MediaclTable_widths = new float[] { 55f, 50f, 40f, 50f };
            ////MediaclTable.SetWidths(MediaclTable_widths);
            ////MediaclTable.HorizontalAlignment = Element.ALIGN_CENTER;
            ////MediaclTable.SpacingBefore = 10f;
            ////if (chldMstDetails != null)
            ////{

            ////    PdfPCell Chld_Med_Plan = new PdfPCell(new Phrase("Child Medical Insurance Plan", Times));
            ////    Chld_Med_Plan.HorizontalAlignment = Element.ALIGN_LEFT;
            ////    Chld_Med_Plan.FixedHeight = 15f;
            ////    Chld_Med_Plan.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////    MediaclTable.AddCell(Chld_Med_Plan);

            ////    if (!string.IsNullOrEmpty(chldMstDetails.MedPlan.Trim()))
            ////    {
            ////        PdfPCell Chld_Med_Plan_Line = new PdfPCell(new Phrase(chldMstDetails.MedPlan.Trim(), Timesline));
            ////        Chld_Med_Plan_Line.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        Chld_Med_Plan_Line.FixedHeight = 15f;
            ////        Chld_Med_Plan_Line.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(Chld_Med_Plan_Line);
            ////    }
            ////    else
            ////    {
            ////        PdfPCell Chld_Med_Plan_Line = new PdfPCell(new Phrase("_____________________________________________", Times));
            ////        Chld_Med_Plan_Line.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        Chld_Med_Plan_Line.FixedHeight = 15f;
            ////        Chld_Med_Plan_Line.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(Chld_Med_Plan_Line);
            ////    }

            ////    PdfPCell Chld_Med_No = new PdfPCell(new Phrase("Medical Insurance Name", Times));
            ////    Chld_Med_No.HorizontalAlignment = Element.ALIGN_RIGHT;
            ////    //Emer_Space.Colspan = 2;
            ////    Chld_Med_No.FixedHeight = 15f;
            ////    Chld_Med_No.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////    MediaclTable.AddCell(Chld_Med_No);

            ////    if (!string.IsNullOrEmpty(chldMstDetails.MedInsurer.Trim()))
            ////    {
            ////        PdfPCell Chld_Med_No_Line = new PdfPCell(new Phrase(chldMstDetails.MedInsurer.Trim(), Timesline));
            ////        Chld_Med_No_Line.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        Chld_Med_No_Line.FixedHeight = 15f;
            ////        Chld_Med_No_Line.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(Chld_Med_No_Line);
            ////    }
            ////    else
            ////    {
            ////        PdfPCell Chld_Med_No_Line = new PdfPCell(new Phrase("_____________________", Times));
            ////        Chld_Med_No_Line.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        Chld_Med_No_Line.FixedHeight = 15f;
            ////        Chld_Med_No_Line.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(Chld_Med_No_Line);
            ////    }

            ////    PdfPCell Chld_Ins_Catg = new PdfPCell(new Phrase("Medical Insurance Category", Times));
            ////    Chld_Ins_Catg.HorizontalAlignment = Element.ALIGN_LEFT;
            ////    //Emer_Space.Colspan = 2;
            ////    Chld_Ins_Catg.FixedHeight = 15f;
            ////    Chld_Ins_Catg.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////    MediaclTable.AddCell(Chld_Ins_Catg);

            ////    if (!string.IsNullOrEmpty(chldMstDetails.InsCat.Trim()))
            ////    {
            ////        string INS_CATG_DESC=string.Empty;
            ////        if (lookInsuranceCategory.Count > 0)
            ////        {
            ////            foreach (CommonEntity Ins in lookInsuranceCategory)
            ////            {
            ////                if (chldMstDetails.InsCat.Trim() == Ins.Code.Trim())
            ////                {
            ////                    INS_CATG_DESC = Ins.Desc.Trim(); break;
            ////                }
            ////            }
            ////        }

            ////        PdfPCell Chld_Med_No_Line = new PdfPCell(new Phrase(INS_CATG_DESC, Timesline));
            ////        Chld_Med_No_Line.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        Chld_Med_No_Line.FixedHeight = 15f;
            ////        Chld_Med_No_Line.Colspan = 3;
            ////        Chld_Med_No_Line.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(Chld_Med_No_Line);
            ////    }
            ////    else
            ////    {
            ////        PdfPCell Chld_Med_No_Line = new PdfPCell(new Phrase("_____________________", Times));
            ////        Chld_Med_No_Line.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        Chld_Med_No_Line.Colspan = 3;
            ////        Chld_Med_No_Line.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(Chld_Med_No_Line);
            ////    }


            ////    PdfPCell Chld_Doctor = new PdfPCell(new Phrase("Child Doctor & Address", Times));
            ////    Chld_Doctor.HorizontalAlignment = Element.ALIGN_LEFT;
            ////    Chld_Doctor.FixedHeight = 15f;
            ////    Chld_Doctor.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////    MediaclTable.AddCell(Chld_Doctor);

            ////    if (!string.IsNullOrEmpty(chldMstDetails.DoctorName.Trim()))
            ////    {
            ////        PdfPCell Chld_Doctor_Line = new PdfPCell(new Phrase(chldMstDetails.DoctorName.Trim() + ", " + chldMstDetails.DoctorAddress, Timesline));
            ////        Chld_Doctor_Line.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        Chld_Doctor_Line.FixedHeight = 15f;
            ////        Chld_Doctor_Line.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(Chld_Doctor_Line);
            ////    }
            ////    else
            ////    {
            ////        PdfPCell Chld_Doctor_Line = new PdfPCell(new Phrase("_____________________________________________", Times));
            ////        Chld_Doctor_Line.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        Chld_Doctor_Line.FixedHeight = 15f;
            ////        Chld_Doctor_Line.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(Chld_Doctor_Line);
            ////    }

            ////    PdfPCell Chld_Doc_Phone = new PdfPCell(new Phrase("Phone#", Times));
            ////    Chld_Doc_Phone.HorizontalAlignment = Element.ALIGN_RIGHT;
            ////    //Emer_Space.Colspan = 2;
            ////    Chld_Doc_Phone.FixedHeight = 15f;
            ////    Chld_Doc_Phone.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////    MediaclTable.AddCell(Chld_Doc_Phone);

            ////    if (!string.IsNullOrEmpty(chldMstDetails.DoctorPhone.Trim()))
            ////    {
            ////        MaskedTextBox mskdocphn = new MaskedTextBox();
            ////        mskdocphn.Mask = "(000)000-0000";
            ////        mskdocphn.Text = chldMstDetails.DoctorPhone.Trim();
            ////        PdfPCell Chld_Doc_Phone_Line = new PdfPCell(new Phrase(mskdocphn.Text.Trim(), Timesline));
            ////        Chld_Doc_Phone_Line.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        Chld_Doc_Phone_Line.FixedHeight = 15f;
            ////        Chld_Doc_Phone_Line.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(Chld_Doc_Phone_Line);
            ////    }
            ////    else
            ////    {
            ////        PdfPCell Chld_Doc_Phone_Line = new PdfPCell(new Phrase("_____________________", Times));
            ////        Chld_Doc_Phone_Line.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        Chld_Doc_Phone_Line.FixedHeight = 15f;
            ////        Chld_Doc_Phone_Line.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(Chld_Doc_Phone_Line);
            ////    }


            ////    PdfPCell Emer_Space2 = new PdfPCell(new Phrase("", Times));
            ////    Emer_Space2.HorizontalAlignment = Element.ALIGN_LEFT;
            ////    Emer_Space2.Colspan = 4;
            ////    Emer_Space2.FixedHeight = 10f;
            ////    Emer_Space2.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////    MediaclTable.AddCell(Emer_Space2);

            ////    PdfPCell Chld_Dent_Plan = new PdfPCell(new Phrase("Child Dental Insurance Plan", Times));
            ////    Chld_Dent_Plan.HorizontalAlignment = Element.ALIGN_LEFT;
            ////    Chld_Dent_Plan.FixedHeight = 15f;
            ////    Chld_Dent_Plan.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////    MediaclTable.AddCell(Chld_Dent_Plan);

            ////    if (!string.IsNullOrEmpty(chldMstDetails.DentalPlan.Trim()))
            ////    {
            ////        PdfPCell Chld_Dent_Plan_Line = new PdfPCell(new Phrase(chldMstDetails.DentalPlan.Trim(), Timesline));
            ////        Chld_Dent_Plan_Line.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        Chld_Dent_Plan_Line.FixedHeight = 15f;
            ////        Chld_Dent_Plan_Line.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(Chld_Dent_Plan_Line);
            ////    }
            ////    else
            ////    {
            ////        PdfPCell Chld_Dent_Plan_Line = new PdfPCell(new Phrase("_____________________________________________", Times));
            ////        Chld_Dent_Plan_Line.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        Chld_Dent_Plan_Line.FixedHeight = 15f;
            ////        Chld_Dent_Plan_Line.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(Chld_Dent_Plan_Line);
            ////    }

            ////    PdfPCell Chld_Dent_No = new PdfPCell(new Phrase("Dental Insurance Name", Times));
            ////    Chld_Dent_No.HorizontalAlignment = Element.ALIGN_RIGHT;
            ////    //Emer_Space.Colspan = 2;
            ////    Chld_Dent_No.FixedHeight = 15f;
            ////    Chld_Dent_No.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////    MediaclTable.AddCell(Chld_Dent_No);

            ////    if (!string.IsNullOrEmpty(chldMstDetails.DentalInsurer.Trim()))
            ////    {
            ////        PdfPCell Chld_Dent_No_Line = new PdfPCell(new Phrase(chldMstDetails.DentalInsurer.Trim(), Timesline));
            ////        Chld_Dent_No_Line.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        Chld_Dent_No_Line.FixedHeight = 15f;
            ////        Chld_Dent_No_Line.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(Chld_Dent_No_Line);
            ////    }
            ////    else
            ////    {
            ////        PdfPCell Chld_Dent_No_Line = new PdfPCell(new Phrase("_____________________", Times));
            ////        Chld_Dent_No_Line.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        Chld_Dent_No_Line.FixedHeight = 15f;
            ////        Chld_Dent_No_Line.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(Chld_Dent_No_Line);
            ////    }


            ////    PdfPCell Chld_Dentist = new PdfPCell(new Phrase("Child Dentist & Address", Times));
            ////    Chld_Dentist.HorizontalAlignment = Element.ALIGN_LEFT;
            ////    Chld_Dentist.FixedHeight = 15f;
            ////    Chld_Dentist.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////    MediaclTable.AddCell(Chld_Dentist);
            ////    if (!string.IsNullOrEmpty(chldMstDetails.DentistName.Trim()))
            ////    {
            ////        PdfPCell Chld_Dentist_Line = new PdfPCell(new Phrase(chldMstDetails.DentistName.Trim() + ", " + chldMstDetails.DentistAddress, Timesline));
            ////        Chld_Dentist_Line.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        Chld_Dentist_Line.FixedHeight = 15f;
            ////        Chld_Dentist_Line.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(Chld_Dentist_Line);
            ////    }
            ////    else
            ////    {
            ////        PdfPCell Chld_Dentist_Line = new PdfPCell(new Phrase("_____________________________________________", Times));
            ////        Chld_Dentist_Line.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        Chld_Dentist_Line.FixedHeight = 15f;
            ////        Chld_Dentist_Line.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(Chld_Dentist_Line);
            ////    }

            ////    PdfPCell Chld_Dentist_Phone = new PdfPCell(new Phrase("Phone#", Times));
            ////    Chld_Dentist_Phone.HorizontalAlignment = Element.ALIGN_RIGHT;
            ////    //Emer_Space.Colspan = 2;
            ////    Chld_Dentist_Phone.FixedHeight = 15f;
            ////    Chld_Dentist_Phone.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////    MediaclTable.AddCell(Chld_Dentist_Phone);

            ////    if (!string.IsNullOrEmpty(chldMstDetails.DentistPhone.Trim()))
            ////    {
            ////        MaskedTextBox mskdocphn = new MaskedTextBox();
            ////        mskdocphn.Mask = "(000)000-0000";
            ////        mskdocphn.Text = chldMstDetails.DentistPhone.Trim();
            ////        PdfPCell Chld_Dentist_Phone_Line = new PdfPCell(new Phrase(mskdocphn.Text.Trim(), Timesline));
            ////        Chld_Dentist_Phone_Line.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        Chld_Dentist_Phone_Line.FixedHeight = 15f;
            ////        Chld_Dentist_Phone_Line.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(Chld_Dentist_Phone_Line);
            ////    }
            ////    else
            ////    {
            ////        PdfPCell Chld_Dentist_Phone_Line = new PdfPCell(new Phrase("_____________________", Times));
            ////        Chld_Dentist_Phone_Line.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        Chld_Dentist_Phone_Line.FixedHeight = 15f;
            ////        Chld_Dentist_Phone_Line.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(Chld_Dentist_Phone_Line);
            ////    }

            ////    PdfPCell Emer_Space3 = new PdfPCell(new Phrase("", Times));
            ////    Emer_Space3.HorizontalAlignment = Element.ALIGN_LEFT;
            ////    Emer_Space3.Colspan = 4;
            ////    Emer_Space3.FixedHeight = 10f;
            ////    Emer_Space3.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////    MediaclTable.AddCell(Emer_Space3);

            ////    if (caseconddet != null)
            ////    {
            ////        PdfPCell ALLERGIES = new PdfPCell(new Phrase("My Child has the following ALLERGIES", Times));
            ////        ALLERGIES.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        ALLERGIES.FixedHeight = 15f;
            ////        ALLERGIES.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(ALLERGIES);

            ////        if (!string.IsNullOrEmpty(caseconddet.Allergy.Trim()))
            ////        {
            ////            string Allergy_desc = caseconddet.Allergy.Replace("\r\n", " ");
            ////            PdfPCell ALLERGIESLine = new PdfPCell(new Phrase(Allergy_desc.Trim(), Timesline));
            ////            ALLERGIESLine.HorizontalAlignment = Element.ALIGN_LEFT;
            ////            ALLERGIESLine.Colspan = 2;
            ////            ALLERGIESLine.FixedHeight = 15f;
            ////            ALLERGIESLine.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////            MediaclTable.AddCell(ALLERGIESLine);
            ////        }
            ////        else
            ////        {
            ////            PdfPCell ALLERGIESLine = new PdfPCell(new Phrase("_____________________________________________", Times));
            ////            ALLERGIESLine.HorizontalAlignment = Element.ALIGN_LEFT;
            ////            ALLERGIESLine.Colspan = 2;
            ////            ALLERGIESLine.FixedHeight = 15f;
            ////            ALLERGIESLine.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////            MediaclTable.AddCell(ALLERGIESLine);
            ////        }
            ////        PdfPCell ALLERGIES_Space = new PdfPCell(new Phrase("", Times));
            ////        ALLERGIES_Space.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        //ALLERGIES_Space.Colspan = 2;
            ////        ALLERGIES_Space.FixedHeight = 15f;
            ////        ALLERGIES_Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(ALLERGIES_Space);

            ////        PdfPCell DISABILITY = new PdfPCell(new Phrase("Has been diagonosed with following DISABILITY", Times));
            ////        DISABILITY.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        DISABILITY.FixedHeight = 15f;
            ////        DISABILITY.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(DISABILITY);

            ////        if (!string.IsNullOrEmpty(chldMstDetails.Disability.Trim()))
            ////        {
            ////            PdfPCell DISABILITYLine = new PdfPCell(new Phrase(chldMstDetails.DisabilityType.Trim(), Timesline));
            ////            DISABILITYLine.HorizontalAlignment = Element.ALIGN_LEFT;
            ////            DISABILITYLine.FixedHeight = 15f;
            ////            DISABILITYLine.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////            MediaclTable.AddCell(DISABILITYLine);
            ////        }
            ////        else
            ////        {
            ////            PdfPCell DISABILITYLine = new PdfPCell(new Phrase("_____________________________________________", Times));
            ////            DISABILITYLine.HorizontalAlignment = Element.ALIGN_LEFT;
            ////            DISABILITYLine.FixedHeight = 15f;
            ////            DISABILITYLine.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////            MediaclTable.AddCell(DISABILITYLine);
            ////        }

            ////        PdfPCell DISABILITY_Date = new PdfPCell(new Phrase("Date", Times));
            ////        DISABILITY_Date.HorizontalAlignment = Element.ALIGN_RIGHT;
            ////        //DISABILITY_Space.Colspan = 2;
            ////        DISABILITY_Date.FixedHeight = 15f;
            ////        DISABILITY_Date.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(DISABILITY_Date);

            ////        if (!string.IsNullOrEmpty(chldMstDetails.DiagnosisDate.Trim()))
            ////        {
            ////            PdfPCell DISABILITY_Date_Line = new PdfPCell(new Phrase(LookupDataAccess.Getdate(chldMstDetails.DiagnosisDate.Trim()), Timesline));
            ////            DISABILITY_Date_Line.HorizontalAlignment = Element.ALIGN_LEFT;
            ////            DISABILITY_Date_Line.FixedHeight = 15f;
            ////            DISABILITY_Date_Line.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////            MediaclTable.AddCell(DISABILITY_Date_Line);
            ////        }
            ////        else
            ////        {
            ////            PdfPCell DISABILITY_Date_Line = new PdfPCell(new Phrase("_____________________", Times));
            ////            DISABILITY_Date_Line.HorizontalAlignment = Element.ALIGN_LEFT;
            ////            DISABILITY_Date_Line.FixedHeight = 15f;
            ////            DISABILITY_Date_Line.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////            MediaclTable.AddCell(DISABILITY_Date_Line);
            ////        }


            ////        PdfPCell MEDICATIONS = new PdfPCell(new Phrase("is taking following MEDICATIONS", Times));
            ////        MEDICATIONS.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        MEDICATIONS.FixedHeight = 15f;
            ////        MEDICATIONS.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(MEDICATIONS);

            ////        if (!string.IsNullOrEmpty(caseconddet.Medications.Trim()))
            ////        {
            ////            string Medications_desc = caseconddet.Medications.Replace("\r\n", " ");
            ////            PdfPCell MEDICATIONSLine = new PdfPCell(new Phrase(Medications_desc.Trim(), Timesline));
            ////            MEDICATIONSLine.HorizontalAlignment = Element.ALIGN_LEFT;
            ////            MEDICATIONSLine.Colspan = 2;
            ////            MEDICATIONSLine.FixedHeight = 15f;
            ////            MEDICATIONSLine.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////            MediaclTable.AddCell(MEDICATIONSLine);
            ////        }
            ////        else
            ////        {
            ////            PdfPCell MEDICATIONSLine = new PdfPCell(new Phrase("_____________________________________________", Times));
            ////            MEDICATIONSLine.HorizontalAlignment = Element.ALIGN_LEFT;
            ////            MEDICATIONSLine.Colspan = 2;
            ////            MEDICATIONSLine.FixedHeight = 15f;
            ////            MEDICATIONSLine.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////            MediaclTable.AddCell(MEDICATIONSLine);
            ////        }

            ////        PdfPCell MEDICATIONS_Space = new PdfPCell(new Phrase("", Times));
            ////        MEDICATIONS_Space.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        //MEDICATIONS_Space.Colspan = 2;
            ////        MEDICATIONS_Space.FixedHeight = 15f;
            ////        MEDICATIONS_Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(MEDICATIONS_Space);

            ////        PdfPCell MEDICAL = new PdfPCell(new Phrase("has the following MEDICAL CONDITIONS", Times));
            ////        MEDICAL.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        MEDICAL.FixedHeight = 15f;
            ////        MEDICAL.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(MEDICAL);

            ////        if (!string.IsNullOrEmpty(caseconddet.MedConds.Trim()))
            ////        {
            ////            string MedConds_desc = caseconddet.MedConds.Replace("\r\n", " ");
            ////            PdfPCell MEDICALLine = new PdfPCell(new Phrase(MedConds_desc.Trim(), Timesline));
            ////            MEDICALLine.HorizontalAlignment = Element.ALIGN_LEFT;
            ////            MEDICALLine.Colspan = 2;
            ////            MEDICALLine.FixedHeight = 15f;
            ////            MEDICALLine.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////            MediaclTable.AddCell(MEDICALLine);
            ////        }
            ////        else
            ////        {
            ////            PdfPCell MEDICALLine = new PdfPCell(new Phrase("_____________________________________________", Times));
            ////            MEDICALLine.HorizontalAlignment = Element.ALIGN_LEFT;
            ////            MEDICALLine.Colspan = 2;
            ////            MEDICALLine.FixedHeight = 15f;
            ////            MEDICALLine.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////            MediaclTable.AddCell(MEDICALLine);
            ////        }

            ////        PdfPCell MEDICAL_Space = new PdfPCell(new Phrase("", Times));
            ////        MEDICAL_Space.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        //MEDICAL_Space.Colspan = 2;
            ////        MEDICAL_Space.FixedHeight = 15f;
            ////        MEDICAL_Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(MEDICAL_Space);

            ////        PdfPCell Diet = new PdfPCell(new Phrase("DIETARY RESTRICTIONS", Times));
            ////        Diet.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        Diet.FixedHeight = 15f;
            ////        Diet.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(Diet);

            ////        if (!string.IsNullOrEmpty(caseconddet.DietRestrct.Trim()))
            ////        {
            ////            string MedConds_desc = caseconddet.DietRestrct.Replace("\r\n", " ");
            ////            PdfPCell MEDICALLine = new PdfPCell(new Phrase(MedConds_desc.Trim(), Timesline));
            ////            MEDICALLine.HorizontalAlignment = Element.ALIGN_LEFT;
            ////            MEDICALLine.Colspan = 2;
            ////            MEDICALLine.FixedHeight = 15f;
            ////            MEDICALLine.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////            MediaclTable.AddCell(MEDICALLine);
            ////        }
            ////        else
            ////        {
            ////            PdfPCell MEDICALLine = new PdfPCell(new Phrase("_____________________________________________", Times));
            ////            MEDICALLine.HorizontalAlignment = Element.ALIGN_LEFT;
            ////            MEDICALLine.Colspan = 2;
            ////            MEDICALLine.FixedHeight = 15f;
            ////            MEDICALLine.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////            MediaclTable.AddCell(MEDICALLine);
            ////        }

            ////        PdfPCell DIET_Space = new PdfPCell(new Phrase("", Times));
            ////        DIET_Space.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        //MEDICAL_Space.Colspan = 2;
            ////        DIET_Space.FixedHeight = 15f;
            ////        DIET_Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(DIET_Space);

            ////        PdfPCell House = new PdfPCell(new Phrase("HOUSEHOLD CONCERNS ", Times));
            ////        House.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        House.FixedHeight = 15f;
            ////        House.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(House);

            ////        if (!string.IsNullOrEmpty(caseconddet.HHConcerns.Trim()))
            ////        {
            ////            string MedConds_desc = caseconddet.HHConcerns.Replace("\r\n", " ");
            ////            PdfPCell MEDICALLine = new PdfPCell(new Phrase(MedConds_desc.Trim(), Timesline));
            ////            MEDICALLine.HorizontalAlignment = Element.ALIGN_LEFT;
            ////            MEDICALLine.Colspan = 2;
            ////            MEDICALLine.FixedHeight = 15f;
            ////            MEDICALLine.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////            MediaclTable.AddCell(MEDICALLine);
            ////        }
            ////        else
            ////        {
            ////            PdfPCell MEDICALLine = new PdfPCell(new Phrase("_____________________________________________", Times));
            ////            MEDICALLine.HorizontalAlignment = Element.ALIGN_LEFT;
            ////            MEDICALLine.Colspan = 2;
            ////            MEDICALLine.FixedHeight = 15f;
            ////            MEDICALLine.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////            MediaclTable.AddCell(MEDICALLine);
            ////        }

            ////        PdfPCell HH_Space = new PdfPCell(new Phrase("", Times));
            ////        HH_Space.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        //MEDICAL_Space.Colspan = 2;
            ////        HH_Space.FixedHeight = 15f;
            ////        HH_Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(HH_Space);

            ////        PdfPCell Devp = new PdfPCell(new Phrase("DEVELOPMENTAL CONCERNS ", Times));
            ////        Devp.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        Devp.FixedHeight = 15f;
            ////        Devp.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(Devp);

            ////        if (!string.IsNullOrEmpty(caseconddet.DevlConcerns.Trim()))
            ////        {
            ////            string MedConds_desc = caseconddet.DevlConcerns.Replace("\r\n", " ");
            ////            PdfPCell MEDICALLine = new PdfPCell(new Phrase(MedConds_desc.Trim(), Timesline));
            ////            MEDICALLine.HorizontalAlignment = Element.ALIGN_LEFT;
            ////            MEDICALLine.Colspan = 2;
            ////            MEDICALLine.FixedHeight = 15f;
            ////            MEDICALLine.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////            MediaclTable.AddCell(MEDICALLine);
            ////        }
            ////        else
            ////        {
            ////            PdfPCell MEDICALLine = new PdfPCell(new Phrase("_____________________________________________", Times));
            ////            MEDICALLine.HorizontalAlignment = Element.ALIGN_LEFT;
            ////            MEDICALLine.Colspan = 2;
            ////            MEDICALLine.FixedHeight = 15f;
            ////            MEDICALLine.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////            MediaclTable.AddCell(MEDICALLine);
            ////        }

            ////        PdfPCell DEVL_Space = new PdfPCell(new Phrase("", Times));
            ////        DEVL_Space.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        //MEDICAL_Space.Colspan = 2;
            ////        DEVL_Space.FixedHeight = 15f;
            ////        DEVL_Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(DEVL_Space);

            ////        PdfPCell AltFnd = new PdfPCell(new Phrase("Alternate Fund", Times));
            ////        AltFnd.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        AltFnd.FixedHeight = 15f;
            ////        AltFnd.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(AltFnd);

            ////        if (!string.IsNullOrEmpty(chldMstDetails.AltFundSrc.Trim()))
            ////        {
            ////            string AltFund_Desc = string.Empty;
            ////            foreach (DataRow drFUND in dtFUND.Rows)
            ////            {
            ////                if (chldMstDetails.AltFundSrc.ToString().Trim() == drFUND["Code"].ToString().Trim())
            ////                {
            ////                    AltFund_Desc = drFUND["LookUpDesc"].ToString().Trim(); break;
            ////                }
            ////            }

            ////            PdfPCell AltFnddesc = new PdfPCell(new Phrase(AltFund_Desc, Timesline));
            ////            AltFnddesc.HorizontalAlignment = Element.ALIGN_LEFT;
            ////            AltFnddesc.FixedHeight = 15f;
            ////            AltFnddesc.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////            MediaclTable.AddCell(AltFnddesc);
            ////        }
            ////        else
            ////        {
            ////            PdfPCell AltFnddesc = new PdfPCell(new Phrase("_____________________________________________", Times));
            ////            AltFnddesc.HorizontalAlignment = Element.ALIGN_LEFT;
            ////            AltFnddesc.FixedHeight = 15f;
            ////            AltFnddesc.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////            MediaclTable.AddCell(AltFnddesc);
            ////        }

            ////        PdfPCell Trans = new PdfPCell(new Phrase("Transport", Times));
            ////        Trans.HorizontalAlignment = Element.ALIGN_RIGHT;
            ////        Trans.FixedHeight = 15f;
            ////        Trans.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(Trans);

            ////        if (!string.IsNullOrEmpty(chldMstDetails.Transport.Trim()))
            ////        {
            ////            string Trans_Desc = string.Empty;
            ////            List<CommonEntity> lookClsTransport = _model.lookupDataAccess.GetCMBTransport();
            ////            foreach (CommonEntity agyEntity in lookClsTransport)
            ////            {
            ////                if (chldMstDetails.Transport.ToString().Trim() == agyEntity.Code.ToString().Trim())
            ////                {
            ////                    Trans_Desc = agyEntity.Desc.ToString().Trim(); break;
            ////                }
            ////            }

            ////            if (!string.IsNullOrEmpty(Trans_Desc.Trim()))
            ////            {
            ////                PdfPCell AltFnddesc = new PdfPCell(new Phrase(Trans_Desc, Timesline));
            ////                AltFnddesc.HorizontalAlignment = Element.ALIGN_LEFT;
            ////                AltFnddesc.FixedHeight = 15f;
            ////                AltFnddesc.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////                MediaclTable.AddCell(AltFnddesc);
            ////            }
            ////            else
            ////            {
            ////                PdfPCell AltFnddesc = new PdfPCell(new Phrase("_____________________", Times));
            ////                AltFnddesc.HorizontalAlignment = Element.ALIGN_LEFT;
            ////                AltFnddesc.FixedHeight = 15f;
            ////                AltFnddesc.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////                MediaclTable.AddCell(AltFnddesc);
            ////            }
            ////        }
            ////        else
            ////        {
            ////            PdfPCell AltFnddesc = new PdfPCell(new Phrase("_____________________", Times));
            ////            AltFnddesc.HorizontalAlignment = Element.ALIGN_LEFT;
            ////            AltFnddesc.FixedHeight = 15f;
            ////            AltFnddesc.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////            MediaclTable.AddCell(AltFnddesc);
            ////        }

            ////        PdfPCell Pick = new PdfPCell(new Phrase("Pickup", Times));
            ////        Pick.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        Pick.FixedHeight = 15f;
            ////        Pick.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(Pick);

            ////        if (!string.IsNullOrEmpty(chldMstDetails.PickOff.Trim()))
            ////        {
            ////            PdfPCell AltFnddesc = new PdfPCell(new Phrase(chldMstDetails.PickOff.Trim(), Timesline));
            ////            AltFnddesc.HorizontalAlignment = Element.ALIGN_LEFT;
            ////            AltFnddesc.FixedHeight = 15f;
            ////            AltFnddesc.Colspan = 3;
            ////            AltFnddesc.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////            MediaclTable.AddCell(AltFnddesc);
            ////        }
            ////        else
            ////        {
            ////            PdfPCell AltFnddesc = new PdfPCell(new Phrase("_____________________________________________", Times));
            ////            AltFnddesc.HorizontalAlignment = Element.ALIGN_LEFT;
            ////            AltFnddesc.FixedHeight = 15f;
            ////            AltFnddesc.Colspan = 3;
            ////            AltFnddesc.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////            MediaclTable.AddCell(AltFnddesc);
            ////        }

            ////        PdfPCell Drop = new PdfPCell(new Phrase("Dropoff", Times));
            ////        Drop.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        Drop.FixedHeight = 15f;
            ////        Drop.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(Drop);

            ////        if (!string.IsNullOrEmpty(chldMstDetails.DropOff.Trim()))
            ////        {
            ////            PdfPCell AltFnddesc = new PdfPCell(new Phrase(chldMstDetails.DropOff.Trim(), Timesline));
            ////            AltFnddesc.HorizontalAlignment = Element.ALIGN_LEFT;
            ////            AltFnddesc.FixedHeight = 15f;
            ////            AltFnddesc.Colspan = 3;
            ////            AltFnddesc.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////            MediaclTable.AddCell(AltFnddesc);
            ////        }
            ////        else
            ////        {
            ////            PdfPCell AltFnddesc = new PdfPCell(new Phrase("_____________________________________________", Times));
            ////            AltFnddesc.HorizontalAlignment = Element.ALIGN_LEFT;
            ////            AltFnddesc.FixedHeight = 15f;
            ////            AltFnddesc.Colspan = 3;
            ////            AltFnddesc.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////            MediaclTable.AddCell(AltFnddesc);
            ////        }

            ////        PdfPCell Last_Space = new PdfPCell(new Phrase("", Times));
            ////        Last_Space.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        Last_Space.Colspan = 4;
            ////        Last_Space.FixedHeight = 15f;
            ////        Last_Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(Last_Space);

            ////        PdfPCell Signature = new PdfPCell(new Phrase("Signature of Parent/Gurdian", Times));
            ////        Signature.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        //Signature.FixedHeight = 15f;
            ////        Signature.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(Signature);

            ////        PdfPCell SignatureLine = new PdfPCell(new Phrase("______________________________________", Times));
            ////        SignatureLine.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        //SignatureLine.FixedHeight = 15f;
            ////        SignatureLine.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(SignatureLine);

            ////        PdfPCell Signature_Date = new PdfPCell(new Phrase("Date", Times));
            ////        Signature_Date.HorizontalAlignment = Element.ALIGN_RIGHT;
            ////        //DISABILITY_Space.Colspan = 2;
            ////        //Signature_Date.FixedHeight = 15f;
            ////        Signature_Date.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(Signature_Date);

            ////        PdfPCell Signature_Date_Line = new PdfPCell(new Phrase("_____________________", Times));
            ////        Signature_Date_Line.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        //Signature_Date_Line.FixedHeight = 15f;
            ////        Signature_Date_Line.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(Signature_Date_Line);

            ////        PdfPCell Line_Last = new PdfPCell(new Phrase("", Times));
            ////        Line_Last.HorizontalAlignment = Element.ALIGN_CENTER;
            ////        Line_Last.Colspan = 6;
            ////        //Line_Last.FixedHeight = 15f;
            ////        Line_Last.BorderWidthBottom = 2f;
            ////        Line_Last.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            ////        MediaclTable.AddCell(Line_Last);
            ////    }
            ////    else
            ////    {
            ////        PdfPCell ALLERGIES = new PdfPCell(new Phrase("My Child has the following ALLERGIES", Times));
            ////        ALLERGIES.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        ALLERGIES.FixedHeight = 15f;
            ////        ALLERGIES.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(ALLERGIES);

            ////        PdfPCell ALLERGIESLine = new PdfPCell(new Phrase("_____________________________________________", Times));
            ////        ALLERGIESLine.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        ALLERGIESLine.FixedHeight = 15f;
            ////        ALLERGIESLine.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(ALLERGIESLine);

            ////        PdfPCell ALLERGIES_Space = new PdfPCell(new Phrase("", Times));
            ////        ALLERGIES_Space.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        ALLERGIES_Space.Colspan = 2;
            ////        ALLERGIES_Space.FixedHeight = 15f;
            ////        ALLERGIES_Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(ALLERGIES_Space);

            ////        PdfPCell DISABILITY = new PdfPCell(new Phrase("Has been diagonosed with following DISABILITY", Times));
            ////        DISABILITY.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        DISABILITY.FixedHeight = 15f;
            ////        DISABILITY.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(DISABILITY);

            ////        PdfPCell DISABILITYLine = new PdfPCell(new Phrase("_____________________________________________", Times));
            ////        DISABILITYLine.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        DISABILITYLine.FixedHeight = 15f;
            ////        DISABILITYLine.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(DISABILITYLine);

            ////        PdfPCell DISABILITY_Date = new PdfPCell(new Phrase("Date", Times));
            ////        DISABILITY_Date.HorizontalAlignment = Element.ALIGN_RIGHT;
            ////        //DISABILITY_Space.Colspan = 2;
            ////        DISABILITY_Date.FixedHeight = 15f;
            ////        DISABILITY_Date.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(DISABILITY_Date);

            ////        PdfPCell DISABILITY_Date_Line = new PdfPCell(new Phrase("_____________________", Times));
            ////        DISABILITY_Date_Line.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        DISABILITY_Date_Line.FixedHeight = 15f;
            ////        DISABILITY_Date_Line.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(DISABILITY_Date_Line);

            ////        PdfPCell MEDICATIONS = new PdfPCell(new Phrase("is taking following MEDICATIONS", Times));
            ////        MEDICATIONS.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        MEDICATIONS.FixedHeight = 15f;
            ////        MEDICATIONS.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(MEDICATIONS);

            ////        PdfPCell MEDICATIONSLine = new PdfPCell(new Phrase("_____________________________________________", Times));
            ////        MEDICATIONSLine.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        MEDICATIONSLine.FixedHeight = 15f;
            ////        MEDICATIONSLine.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(MEDICATIONSLine);

            ////        PdfPCell MEDICATIONS_Space = new PdfPCell(new Phrase("", Times));
            ////        MEDICATIONS_Space.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        MEDICATIONS_Space.Colspan = 2;
            ////        MEDICATIONS_Space.FixedHeight = 15f;
            ////        MEDICATIONS_Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(MEDICATIONS_Space);

            ////        PdfPCell MEDICAL = new PdfPCell(new Phrase("has the following MEDICAL CONDITIONS", Times));
            ////        MEDICAL.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        MEDICAL.FixedHeight = 15f;
            ////        MEDICAL.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(MEDICAL);

            ////        PdfPCell MEDICALLine = new PdfPCell(new Phrase("_____________________________________________", Times));
            ////        MEDICALLine.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        MEDICALLine.FixedHeight = 15f;
            ////        MEDICALLine.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(MEDICALLine);

            ////        PdfPCell MEDICAL_Space = new PdfPCell(new Phrase("", Times));
            ////        MEDICAL_Space.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        MEDICAL_Space.Colspan = 2;
            ////        MEDICAL_Space.FixedHeight = 15f;
            ////        MEDICAL_Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(MEDICAL_Space);

            ////        PdfPCell Diet = new PdfPCell(new Phrase("DIETARY RESTRICTIONS", Times));
            ////        Diet.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        Diet.FixedHeight = 15f;
            ////        Diet.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(Diet);

            ////        PdfPCell DIETLine = new PdfPCell(new Phrase("_____________________________________________", Times));
            ////        DIETLine.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        //DIETLine.Colspan = 2;
            ////        DIETLine.FixedHeight = 15f;
            ////        DIETLine.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(DIETLine);

            ////        PdfPCell DIET_Space = new PdfPCell(new Phrase("", Times));
            ////        DIET_Space.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        DIET_Space.Colspan = 2;
            ////        DIET_Space.FixedHeight = 15f;
            ////        DIET_Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(DIET_Space);

            ////        PdfPCell House = new PdfPCell(new Phrase("HOUSEHOLD CONCERNS ", Times));
            ////        House.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        House.FixedHeight = 15f;
            ////        House.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(House);

            ////        PdfPCell HOUSEHOLDLine = new PdfPCell(new Phrase("_____________________________________________", Times));
            ////        HOUSEHOLDLine.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        //HOUSEHOLDLine.Colspan = 2;
            ////        HOUSEHOLDLine.FixedHeight = 15f;
            ////        HOUSEHOLDLine.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(HOUSEHOLDLine);

            ////        PdfPCell HOUSEHOLD_Space = new PdfPCell(new Phrase("", Times));
            ////        HOUSEHOLD_Space.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        HOUSEHOLD_Space.Colspan = 2;
            ////        HOUSEHOLD_Space.FixedHeight = 15f;
            ////        HOUSEHOLD_Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(HOUSEHOLD_Space);

            ////        PdfPCell Devp = new PdfPCell(new Phrase("DEVELOPMENTAL CONCERNS ", Times));
            ////        Devp.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        Devp.FixedHeight = 15f;
            ////        Devp.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(Devp);

            ////        PdfPCell DevpLine = new PdfPCell(new Phrase("_____________________________________________", Times));
            ////        DevpLine.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        //DevpLine.Colspan = 2;
            ////        DevpLine.FixedHeight = 15f;
            ////        DevpLine.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(DevpLine);

            ////        PdfPCell Devp_Space = new PdfPCell(new Phrase("", Times));
            ////        Devp_Space.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        Devp_Space.Colspan = 2;
            ////        Devp_Space.FixedHeight = 15f;
            ////        Devp_Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(Devp_Space);

            ////        PdfPCell AltFnd = new PdfPCell(new Phrase("Alternate Fund", Times));
            ////        AltFnd.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        AltFnd.FixedHeight = 15f;
            ////        AltFnd.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(AltFnd);

            ////        PdfPCell AltFnddesc = new PdfPCell(new Phrase("_____________________________________________", Times));
            ////        AltFnddesc.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        AltFnddesc.FixedHeight = 15f;
            ////        AltFnddesc.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(AltFnddesc);

            ////        PdfPCell Trans = new PdfPCell(new Phrase("Transport", Times));
            ////        Trans.HorizontalAlignment = Element.ALIGN_RIGHT;
            ////        Trans.FixedHeight = 15f;
            ////        Trans.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(Trans);


            ////        PdfPCell Trans_Space = new PdfPCell(new Phrase("_____________________", Times));
            ////        Trans_Space.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        Trans_Space.FixedHeight = 15f;
            ////        Trans_Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(Trans_Space);

            ////        PdfPCell Pick = new PdfPCell(new Phrase("Pickup", Times));
            ////        Pick.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        Pick.FixedHeight = 15f;
            ////        Pick.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(Pick);


            ////        PdfPCell PickSpace = new PdfPCell(new Phrase("_____________________________________________", Times));
            ////        PickSpace.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        PickSpace.FixedHeight = 15f;
            ////        PickSpace.Colspan = 3;
            ////        PickSpace.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(PickSpace);

            ////        PdfPCell Drop = new PdfPCell(new Phrase("Dropoff", Times));
            ////        Drop.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        Drop.FixedHeight = 15f;
            ////        Drop.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(Drop);


            ////        PdfPCell dropSpace = new PdfPCell(new Phrase("_____________________________________________", Times));
            ////        dropSpace.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        dropSpace.FixedHeight = 15f;
            ////        dropSpace.Colspan = 3;
            ////        dropSpace.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(dropSpace);

            ////        PdfPCell Last_Space = new PdfPCell(new Phrase("", Times));
            ////        Last_Space.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        Last_Space.Colspan = 4;
            ////        Last_Space.FixedHeight = 15f;
            ////        Last_Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(Last_Space);

            ////        PdfPCell Signature = new PdfPCell(new Phrase("Signature of Parent/Gurdian", Times));
            ////        Signature.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        //Signature.FixedHeight = 15f;
            ////        Signature.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(Signature);

            ////        PdfPCell SignatureLine = new PdfPCell(new Phrase("______________________________________", Times));
            ////        SignatureLine.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        //SignatureLine.FixedHeight = 15f;
            ////        SignatureLine.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(SignatureLine);

            ////        PdfPCell Signature_Date = new PdfPCell(new Phrase("Date", Times));
            ////        Signature_Date.HorizontalAlignment = Element.ALIGN_RIGHT;
            ////        //DISABILITY_Space.Colspan = 2;
            ////        //Signature_Date.FixedHeight = 15f;
            ////        Signature_Date.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(Signature_Date);

            ////        PdfPCell Signature_Date_Line = new PdfPCell(new Phrase("_____________________", Times));
            ////        Signature_Date_Line.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        //Signature_Date_Line.FixedHeight = 15f;
            ////        Signature_Date_Line.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(Signature_Date_Line);

            ////        PdfPCell Line_Last = new PdfPCell(new Phrase("", Times));
            ////        Line_Last.HorizontalAlignment = Element.ALIGN_CENTER;
            ////        Line_Last.Colspan = 6;
            ////        //Line_Last.FixedHeight = 15f;
            ////        Line_Last.BorderWidthBottom = 2f;
            ////        Line_Last.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            ////        MediaclTable.AddCell(Line_Last);
            ////    }
            ////}
            ////else
            ////{
            ////    PdfPCell Chld_Med_Plan = new PdfPCell(new Phrase("Child Medical Insurance Plan", Times));
            ////    Chld_Med_Plan.HorizontalAlignment = Element.ALIGN_LEFT;
            ////    Chld_Med_Plan.FixedHeight = 15f;
            ////    Chld_Med_Plan.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////    MediaclTable.AddCell(Chld_Med_Plan);

            ////    PdfPCell Chld_Med_Plan_Line = new PdfPCell(new Phrase("_____________________________________________", Times));
            ////    Chld_Med_Plan_Line.HorizontalAlignment = Element.ALIGN_LEFT;
            ////    Chld_Med_Plan_Line.FixedHeight = 15f;
            ////    Chld_Med_Plan_Line.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////    MediaclTable.AddCell(Chld_Med_Plan_Line);

            ////    PdfPCell Chld_Med_No = new PdfPCell(new Phrase("Medical Insurance Name", Times));
            ////    Chld_Med_No.HorizontalAlignment = Element.ALIGN_RIGHT;
            ////    //Emer_Space.Colspan = 2;
            ////    Chld_Med_No.FixedHeight = 15f;
            ////    Chld_Med_No.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////    MediaclTable.AddCell(Chld_Med_No);


            ////    PdfPCell Chld_Med_No_Line = new PdfPCell(new Phrase("_____________________", Times));
            ////    Chld_Med_No_Line.HorizontalAlignment = Element.ALIGN_LEFT;
            ////    Chld_Med_No_Line.FixedHeight = 15f;
            ////    Chld_Med_No_Line.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////    MediaclTable.AddCell(Chld_Med_No_Line);

            ////    PdfPCell Chld_Ins_Catg = new PdfPCell(new Phrase("Medical Insurance Category", Times));
            ////    Chld_Ins_Catg.HorizontalAlignment = Element.ALIGN_RIGHT;
            ////    //Emer_Space.Colspan = 2;
            ////    Chld_Ins_Catg.FixedHeight = 15f;
            ////    Chld_Ins_Catg.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////    MediaclTable.AddCell(Chld_Ins_Catg);


            ////    PdfPCell Chld_Ins_Catg_Line = new PdfPCell(new Phrase("_____________________", Times));
            ////    Chld_Ins_Catg_Line.HorizontalAlignment = Element.ALIGN_LEFT;
            ////    Chld_Ins_Catg_Line.FixedHeight = 15f;
            ////    Chld_Ins_Catg_Line.Colspan = 3;
            ////    Chld_Ins_Catg_Line.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////    MediaclTable.AddCell(Chld_Ins_Catg_Line);

            ////    PdfPCell Chld_Doctor = new PdfPCell(new Phrase("Child Doctor & Address", Times));
            ////    Chld_Doctor.HorizontalAlignment = Element.ALIGN_LEFT;
            ////    Chld_Doctor.FixedHeight = 15f;
            ////    Chld_Doctor.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////    MediaclTable.AddCell(Chld_Doctor);

            ////    PdfPCell Chld_Doctor_Line = new PdfPCell(new Phrase("_____________________________________________", Times));
            ////    Chld_Doctor_Line.HorizontalAlignment = Element.ALIGN_LEFT;
            ////    Chld_Doctor_Line.FixedHeight = 15f;
            ////    Chld_Doctor_Line.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////    MediaclTable.AddCell(Chld_Doctor_Line);

            ////    PdfPCell Chld_Doc_Phone = new PdfPCell(new Phrase("Phone#", Times));
            ////    Chld_Doc_Phone.HorizontalAlignment = Element.ALIGN_RIGHT;
            ////    //Emer_Space.Colspan = 2;
            ////    Chld_Doc_Phone.FixedHeight = 15f;
            ////    Chld_Doc_Phone.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////    MediaclTable.AddCell(Chld_Doc_Phone);


            ////    PdfPCell Chld_Doc_Phone_Line = new PdfPCell(new Phrase("_____________________", Times));
            ////    Chld_Doc_Phone_Line.HorizontalAlignment = Element.ALIGN_LEFT;
            ////    Chld_Doc_Phone_Line.FixedHeight = 15f;
            ////    Chld_Doc_Phone_Line.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////    MediaclTable.AddCell(Chld_Doc_Phone_Line);

            ////    PdfPCell Emer_Space2 = new PdfPCell(new Phrase("", Times));
            ////    Emer_Space2.HorizontalAlignment = Element.ALIGN_LEFT;
            ////    Emer_Space2.Colspan = 4;
            ////    Emer_Space2.FixedHeight = 10f;
            ////    Emer_Space2.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////    MediaclTable.AddCell(Emer_Space2);

            ////    PdfPCell Chld_Dent_Plan = new PdfPCell(new Phrase("Child Dental Insurance Plan", Times));
            ////    Chld_Dent_Plan.HorizontalAlignment = Element.ALIGN_LEFT;
            ////    Chld_Dent_Plan.FixedHeight = 15f;
            ////    Chld_Dent_Plan.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////    MediaclTable.AddCell(Chld_Dent_Plan);

            ////    PdfPCell Chld_Dent_Plan_Line = new PdfPCell(new Phrase("_____________________________________________", Times));
            ////    Chld_Dent_Plan_Line.HorizontalAlignment = Element.ALIGN_LEFT;
            ////    Chld_Dent_Plan_Line.FixedHeight = 15f;
            ////    Chld_Dent_Plan_Line.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////    MediaclTable.AddCell(Chld_Dent_Plan_Line);

            ////    PdfPCell Chld_Dent_No = new PdfPCell(new Phrase("Dental Insurance Name", Times));
            ////    Chld_Dent_No.HorizontalAlignment = Element.ALIGN_RIGHT;
            ////    //Emer_Space.Colspan = 2;
            ////    Chld_Dent_No.FixedHeight = 15f;
            ////    Chld_Dent_No.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////    MediaclTable.AddCell(Chld_Dent_No);

            ////    PdfPCell Chld_Dent_No_Line = new PdfPCell(new Phrase("_____________________", Times));
            ////    Chld_Dent_No_Line.HorizontalAlignment = Element.ALIGN_LEFT;
            ////    Chld_Dent_No_Line.FixedHeight = 15f;
            ////    Chld_Dent_No_Line.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////    MediaclTable.AddCell(Chld_Dent_No_Line);

            ////    PdfPCell Chld_Dentist = new PdfPCell(new Phrase("Child Dentist & Address", Times));
            ////    Chld_Dentist.HorizontalAlignment = Element.ALIGN_LEFT;
            ////    Chld_Dentist.FixedHeight = 15f;
            ////    Chld_Dentist.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////    MediaclTable.AddCell(Chld_Dentist);

            ////    PdfPCell Chld_Dentist_Line = new PdfPCell(new Phrase("_____________________________________________", Times));
            ////    Chld_Dentist_Line.HorizontalAlignment = Element.ALIGN_LEFT;
            ////    Chld_Dentist_Line.FixedHeight = 15f;
            ////    Chld_Dentist_Line.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////    MediaclTable.AddCell(Chld_Dentist_Line);

            ////    PdfPCell Chld_Dentist_Phone = new PdfPCell(new Phrase("Phone#", Times));
            ////    Chld_Dentist_Phone.HorizontalAlignment = Element.ALIGN_RIGHT;
            ////    //Emer_Space.Colspan = 2;
            ////    Chld_Dentist_Phone.FixedHeight = 15f;
            ////    Chld_Dentist_Phone.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////    MediaclTable.AddCell(Chld_Dentist_Phone);

            ////    PdfPCell Chld_Dentist_Phone_Line = new PdfPCell(new Phrase("_____________________", Times));
            ////    Chld_Dentist_Phone_Line.HorizontalAlignment = Element.ALIGN_LEFT;
            ////    Chld_Dentist_Phone_Line.FixedHeight = 15f;
            ////    Chld_Dentist_Phone_Line.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////    MediaclTable.AddCell(Chld_Dentist_Phone_Line);

            ////    PdfPCell Emer_Space3 = new PdfPCell(new Phrase("", Times));
            ////    Emer_Space3.HorizontalAlignment = Element.ALIGN_LEFT;
            ////    Emer_Space3.Colspan = 4;
            ////    Emer_Space3.FixedHeight = 10f;
            ////    Emer_Space3.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////    MediaclTable.AddCell(Emer_Space3);

            ////    if (caseconddet != null)
            ////    {
            ////        PdfPCell ALLERGIES = new PdfPCell(new Phrase("My Child has the following ALLERGIES", Times));
            ////        ALLERGIES.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        ALLERGIES.FixedHeight = 15f;
            ////        ALLERGIES.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(ALLERGIES);

            ////        if (!string.IsNullOrEmpty(caseconddet.Allergy.Trim()))
            ////        {
            ////            string Allergy_desc = caseconddet.Allergy.Replace("\r\n", " ");
            ////            PdfPCell ALLERGIESLine = new PdfPCell(new Phrase(Allergy_desc.Trim(), Times));
            ////            ALLERGIESLine.HorizontalAlignment = Element.ALIGN_LEFT;
            ////            ALLERGIESLine.Colspan = 2;
            ////            ALLERGIESLine.FixedHeight = 15f;
            ////            ALLERGIESLine.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////            MediaclTable.AddCell(ALLERGIESLine);
            ////        }
            ////        else
            ////        {
            ////            PdfPCell ALLERGIESLine = new PdfPCell(new Phrase("_____________________________________________", Times));
            ////            ALLERGIESLine.HorizontalAlignment = Element.ALIGN_LEFT;
            ////            ALLERGIESLine.Colspan = 2;
            ////            ALLERGIESLine.FixedHeight = 15f;
            ////            ALLERGIESLine.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////            MediaclTable.AddCell(ALLERGIESLine);
            ////        }
            ////        PdfPCell ALLERGIES_Space = new PdfPCell(new Phrase("", Times));
            ////        ALLERGIES_Space.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        //ALLERGIES_Space.Colspan = 2;
            ////        ALLERGIES_Space.FixedHeight = 15f;
            ////        ALLERGIES_Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(ALLERGIES_Space);

            ////        PdfPCell DISABILITY = new PdfPCell(new Phrase("Has been diagonosed with following DISABILITY", Times));
            ////        DISABILITY.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        DISABILITY.FixedHeight = 15f;
            ////        DISABILITY.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(DISABILITY);

            ////        if (!string.IsNullOrEmpty(chldMstDetails.Disability.Trim()))
            ////        {
            ////            PdfPCell DISABILITYLine = new PdfPCell(new Phrase(chldMstDetails.DisabilityType.Trim(), Times));
            ////            DISABILITYLine.HorizontalAlignment = Element.ALIGN_LEFT;
            ////            DISABILITYLine.FixedHeight = 15f;
            ////            DISABILITYLine.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////            MediaclTable.AddCell(DISABILITYLine);
            ////        }
            ////        else
            ////        {
            ////            PdfPCell DISABILITYLine = new PdfPCell(new Phrase("_____________________________________________", Times));
            ////            DISABILITYLine.HorizontalAlignment = Element.ALIGN_LEFT;
            ////            DISABILITYLine.FixedHeight = 15f;
            ////            DISABILITYLine.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////            MediaclTable.AddCell(DISABILITYLine);
            ////        }

            ////        PdfPCell DISABILITY_Date = new PdfPCell(new Phrase("Date", Times));
            ////        DISABILITY_Date.HorizontalAlignment = Element.ALIGN_RIGHT;
            ////        //DISABILITY_Space.Colspan = 2;
            ////        DISABILITY_Date.FixedHeight = 15f;
            ////        DISABILITY_Date.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(DISABILITY_Date);

            ////        if (!string.IsNullOrEmpty(chldMstDetails.DiagnosisDate.Trim()))
            ////        {
            ////            PdfPCell DISABILITY_Date_Line = new PdfPCell(new Phrase(LookupDataAccess.Getdate(chldMstDetails.DiagnosisDate.Trim()), Times));
            ////            DISABILITY_Date_Line.HorizontalAlignment = Element.ALIGN_LEFT;
            ////            DISABILITY_Date_Line.FixedHeight = 15f;
            ////            DISABILITY_Date_Line.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////            MediaclTable.AddCell(DISABILITY_Date_Line);
            ////        }
            ////        else
            ////        {
            ////            PdfPCell DISABILITY_Date_Line = new PdfPCell(new Phrase("_____________________", Times));
            ////            DISABILITY_Date_Line.HorizontalAlignment = Element.ALIGN_LEFT;
            ////            DISABILITY_Date_Line.FixedHeight = 15f;
            ////            DISABILITY_Date_Line.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////            MediaclTable.AddCell(DISABILITY_Date_Line);
            ////        }


            ////        PdfPCell MEDICATIONS = new PdfPCell(new Phrase("is taking following MEDICATIONS", Times));
            ////        MEDICATIONS.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        MEDICATIONS.FixedHeight = 15f;
            ////        MEDICATIONS.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(MEDICATIONS);

            ////        if (!string.IsNullOrEmpty(caseconddet.Medications.Trim()))
            ////        {
            ////            string Medications_desc = caseconddet.Medications.Replace("\r\n", " ");
            ////            PdfPCell MEDICATIONSLine = new PdfPCell(new Phrase(Medications_desc.Trim(), Times));
            ////            MEDICATIONSLine.HorizontalAlignment = Element.ALIGN_LEFT;
            ////            MEDICATIONSLine.Colspan = 2;
            ////            MEDICATIONSLine.FixedHeight = 15f;
            ////            MEDICATIONSLine.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////            MediaclTable.AddCell(MEDICATIONSLine);
            ////        }
            ////        else
            ////        {
            ////            PdfPCell MEDICATIONSLine = new PdfPCell(new Phrase("_____________________________________________", Times));
            ////            MEDICATIONSLine.HorizontalAlignment = Element.ALIGN_LEFT;
            ////            MEDICATIONSLine.Colspan = 2;
            ////            MEDICATIONSLine.FixedHeight = 15f;
            ////            MEDICATIONSLine.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////            MediaclTable.AddCell(MEDICATIONSLine);
            ////        }

            ////        PdfPCell MEDICATIONS_Space = new PdfPCell(new Phrase("", Times));
            ////        MEDICATIONS_Space.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        //MEDICATIONS_Space.Colspan = 2;
            ////        MEDICATIONS_Space.FixedHeight = 15f;
            ////        MEDICATIONS_Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(MEDICATIONS_Space);

            ////        PdfPCell MEDICAL = new PdfPCell(new Phrase("has the following MEDICAL CONDITIONS", Times));
            ////        MEDICAL.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        MEDICAL.FixedHeight = 15f;
            ////        MEDICAL.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(MEDICAL);

            ////        if (!string.IsNullOrEmpty(caseconddet.MedConds.Trim()))
            ////        {
            ////            string MedConds_desc = caseconddet.MedConds.Replace("\r\n", " ");
            ////            PdfPCell MEDICALLine = new PdfPCell(new Phrase(MedConds_desc.Trim(), Times));
            ////            MEDICALLine.HorizontalAlignment = Element.ALIGN_LEFT;
            ////            MEDICALLine.Colspan = 2;
            ////            MEDICALLine.FixedHeight = 15f;
            ////            MEDICALLine.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////            MediaclTable.AddCell(MEDICALLine);
            ////        }
            ////        else
            ////        {
            ////            PdfPCell MEDICALLine = new PdfPCell(new Phrase("_____________________________________________", Times));
            ////            MEDICALLine.HorizontalAlignment = Element.ALIGN_LEFT;
            ////            MEDICALLine.Colspan = 2;
            ////            MEDICALLine.FixedHeight = 15f;
            ////            MEDICALLine.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////            MediaclTable.AddCell(MEDICALLine);
            ////        }

            ////        PdfPCell MEDICAL_Space = new PdfPCell(new Phrase("", Times));
            ////        MEDICAL_Space.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        //MEDICAL_Space.Colspan = 2;
            ////        MEDICAL_Space.FixedHeight = 15f;
            ////        MEDICAL_Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(MEDICAL_Space);

            ////        PdfPCell Diet = new PdfPCell(new Phrase("DIETARY RESTRICTIONS", Times));
            ////        Diet.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        Diet.FixedHeight = 15f;
            ////        Diet.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(Diet);

            ////        if (!string.IsNullOrEmpty(caseconddet.DietRestrct.Trim()))
            ////        {
            ////            string MedConds_desc = caseconddet.DietRestrct.Replace("\r\n", " ");
            ////            PdfPCell MEDICALLine = new PdfPCell(new Phrase(MedConds_desc.Trim(), Times));
            ////            MEDICALLine.HorizontalAlignment = Element.ALIGN_LEFT;
            ////            MEDICALLine.Colspan = 2;
            ////            MEDICALLine.FixedHeight = 15f;
            ////            MEDICALLine.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////            MediaclTable.AddCell(MEDICALLine);
            ////        }
            ////        else
            ////        {
            ////            PdfPCell MEDICALLine = new PdfPCell(new Phrase("_____________________________________________", Times));
            ////            MEDICALLine.HorizontalAlignment = Element.ALIGN_LEFT;
            ////            MEDICALLine.Colspan = 2;
            ////            MEDICALLine.FixedHeight = 15f;
            ////            MEDICALLine.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////            MediaclTable.AddCell(MEDICALLine);
            ////        }

            ////        PdfPCell DIET_Space = new PdfPCell(new Phrase("", Times));
            ////        DIET_Space.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        //MEDICAL_Space.Colspan = 2;
            ////        DIET_Space.FixedHeight = 15f;
            ////        DIET_Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(DIET_Space);

            ////        PdfPCell House = new PdfPCell(new Phrase("HOUSEHOLD CONCERNS ", Times));
            ////        House.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        House.FixedHeight = 15f;
            ////        House.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(House);

            ////        if (!string.IsNullOrEmpty(caseconddet.HHConcerns.Trim()))
            ////        {
            ////            string MedConds_desc = caseconddet.HHConcerns.Replace("\r\n", " ");
            ////            PdfPCell MEDICALLine = new PdfPCell(new Phrase(MedConds_desc.Trim(), Times));
            ////            MEDICALLine.HorizontalAlignment = Element.ALIGN_LEFT;
            ////            MEDICALLine.Colspan = 2;
            ////            MEDICALLine.FixedHeight = 15f;
            ////            MEDICALLine.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////            MediaclTable.AddCell(MEDICALLine);
            ////        }
            ////        else
            ////        {
            ////            PdfPCell MEDICALLine = new PdfPCell(new Phrase("_____________________________________________", Times));
            ////            MEDICALLine.HorizontalAlignment = Element.ALIGN_LEFT;
            ////            MEDICALLine.Colspan = 2;
            ////            MEDICALLine.FixedHeight = 15f;
            ////            MEDICALLine.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////            MediaclTable.AddCell(MEDICALLine);
            ////        }

            ////        PdfPCell HH_Space = new PdfPCell(new Phrase("", Times));
            ////        HH_Space.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        //MEDICAL_Space.Colspan = 2;
            ////        HH_Space.FixedHeight = 15f;
            ////        HH_Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(HH_Space);

            ////        PdfPCell Devp = new PdfPCell(new Phrase("DEVELOPMENTAL CONCERNS ", Times));
            ////        Devp.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        Devp.FixedHeight = 15f;
            ////        Devp.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(Devp);

            ////        if (!string.IsNullOrEmpty(caseconddet.DevlConcerns.Trim()))
            ////        {
            ////            string MedConds_desc = caseconddet.DevlConcerns.Replace("\r\n", " ");
            ////            PdfPCell MEDICALLine = new PdfPCell(new Phrase(MedConds_desc.Trim(), Times));
            ////            MEDICALLine.HorizontalAlignment = Element.ALIGN_LEFT;
            ////            MEDICALLine.Colspan = 2;
            ////            MEDICALLine.FixedHeight = 15f;
            ////            MEDICALLine.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////            MediaclTable.AddCell(MEDICALLine);
            ////        }
            ////        else
            ////        {
            ////            PdfPCell MEDICALLine = new PdfPCell(new Phrase("_____________________________________________", Times));
            ////            MEDICALLine.HorizontalAlignment = Element.ALIGN_LEFT;
            ////            MEDICALLine.Colspan = 2;
            ////            MEDICALLine.FixedHeight = 15f;
            ////            MEDICALLine.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////            MediaclTable.AddCell(MEDICALLine);
            ////        }

            ////        PdfPCell DEVL_Space = new PdfPCell(new Phrase("", Times));
            ////        DEVL_Space.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        //MEDICAL_Space.Colspan = 2;
            ////        DEVL_Space.FixedHeight = 15f;
            ////        DEVL_Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(DEVL_Space);

            ////        PdfPCell AltFnd = new PdfPCell(new Phrase("Alternate Fund", Times));
            ////        AltFnd.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        AltFnd.FixedHeight = 15f;
            ////        AltFnd.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(AltFnd);

            ////        if (!string.IsNullOrEmpty(chldMstDetails.AltFundSrc.Trim()))
            ////        {
            ////            string AltFund_Desc = string.Empty;
            ////            foreach (DataRow drFUND in dtFUND.Rows)
            ////            {
            ////                if (chldMstDetails.AltFundSrc.ToString().Trim() == drFUND["Code"].ToString().Trim())
            ////                {
            ////                    AltFund_Desc = drFUND["LookUpDesc"].ToString().Trim(); break;
            ////                }
            ////            }

            ////            PdfPCell AltFnddesc = new PdfPCell(new Phrase(AltFund_Desc, Timesline));
            ////            AltFnddesc.HorizontalAlignment = Element.ALIGN_LEFT;
            ////            AltFnddesc.FixedHeight = 15f;
            ////            AltFnddesc.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////            MediaclTable.AddCell(AltFnddesc);
            ////        }
            ////        else
            ////        {
            ////            PdfPCell AltFnddesc = new PdfPCell(new Phrase("_____________________________________________", Times));
            ////            AltFnddesc.HorizontalAlignment = Element.ALIGN_LEFT;
            ////            AltFnddesc.FixedHeight = 15f;
            ////            AltFnddesc.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////            MediaclTable.AddCell(AltFnddesc);
            ////        }

            ////        PdfPCell Trans = new PdfPCell(new Phrase("Transport", Times));
            ////        Trans.HorizontalAlignment = Element.ALIGN_RIGHT;
            ////        Trans.FixedHeight = 15f;
            ////        Trans.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(Trans);

            ////        if (!string.IsNullOrEmpty(chldMstDetails.Transport.Trim()))
            ////        {
            ////            string Trans_Desc = string.Empty;
            ////            List<CommonEntity> lookClsTransport = _model.lookupDataAccess.GetCMBTransport();
            ////            foreach (CommonEntity agyEntity in lookClsTransport)
            ////            {
            ////                if (chldMstDetails.Transport.ToString().Trim() == agyEntity.Code.ToString().Trim())
            ////                {
            ////                    Trans_Desc = agyEntity.Desc.ToString().Trim(); break;
            ////                }
            ////            }

            ////            if (!string.IsNullOrEmpty(Trans_Desc.Trim()))
            ////            {
            ////                PdfPCell AltFnddesc = new PdfPCell(new Phrase(Trans_Desc, Timesline));
            ////                AltFnddesc.HorizontalAlignment = Element.ALIGN_LEFT;
            ////                AltFnddesc.FixedHeight = 15f;
            ////                AltFnddesc.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////                MediaclTable.AddCell(AltFnddesc);
            ////            }
            ////            else
            ////            {
            ////                PdfPCell AltFnddesc = new PdfPCell(new Phrase("_____________________", Times));
            ////                AltFnddesc.HorizontalAlignment = Element.ALIGN_LEFT;
            ////                AltFnddesc.FixedHeight = 15f;
            ////                AltFnddesc.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////                MediaclTable.AddCell(AltFnddesc);
            ////            }
            ////        }
            ////        else
            ////        {
            ////            PdfPCell AltFnddesc = new PdfPCell(new Phrase("_____________________", Times));
            ////            AltFnddesc.HorizontalAlignment = Element.ALIGN_LEFT;
            ////            AltFnddesc.FixedHeight = 15f;
            ////            AltFnddesc.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////            MediaclTable.AddCell(AltFnddesc);
            ////        }

            ////        PdfPCell Pick = new PdfPCell(new Phrase("Pickup", Times));
            ////        Pick.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        Pick.FixedHeight = 15f;
            ////        Pick.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(Pick);

            ////        if (!string.IsNullOrEmpty(chldMstDetails.PickOff.Trim()))
            ////        {
            ////            PdfPCell AltFnddesc = new PdfPCell(new Phrase(chldMstDetails.PickOff.Trim(), Timesline));
            ////            AltFnddesc.HorizontalAlignment = Element.ALIGN_LEFT;
            ////            AltFnddesc.FixedHeight = 15f;
            ////            AltFnddesc.Colspan = 3;
            ////            AltFnddesc.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////            MediaclTable.AddCell(AltFnddesc);
            ////        }
            ////        else
            ////        {
            ////            PdfPCell AltFnddesc = new PdfPCell(new Phrase("_____________________________________________", Times));
            ////            AltFnddesc.HorizontalAlignment = Element.ALIGN_LEFT;
            ////            AltFnddesc.FixedHeight = 15f;
            ////            AltFnddesc.Colspan = 3;
            ////            AltFnddesc.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////            MediaclTable.AddCell(AltFnddesc);
            ////        }

            ////        PdfPCell Drop = new PdfPCell(new Phrase("Dropoff", Times));
            ////        Drop.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        Drop.FixedHeight = 15f;
            ////        Drop.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(Drop);

            ////        if (!string.IsNullOrEmpty(chldMstDetails.DropOff.Trim()))
            ////        {
            ////            PdfPCell AltFnddesc = new PdfPCell(new Phrase(chldMstDetails.DropOff.Trim(), Timesline));
            ////            AltFnddesc.HorizontalAlignment = Element.ALIGN_LEFT;
            ////            AltFnddesc.FixedHeight = 15f;
            ////            AltFnddesc.Colspan = 3;
            ////            AltFnddesc.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////            MediaclTable.AddCell(AltFnddesc);
            ////        }
            ////        else
            ////        {
            ////            PdfPCell AltFnddesc = new PdfPCell(new Phrase("_____________________________________________", Times));
            ////            AltFnddesc.HorizontalAlignment = Element.ALIGN_LEFT;
            ////            AltFnddesc.FixedHeight = 15f;
            ////            AltFnddesc.Colspan = 3;
            ////            AltFnddesc.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////            MediaclTable.AddCell(AltFnddesc);
            ////        }

            ////        PdfPCell Last_Space = new PdfPCell(new Phrase("", Times));
            ////        Last_Space.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        Last_Space.Colspan = 4;
            ////        Last_Space.FixedHeight = 15f;
            ////        Last_Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(Last_Space);

            ////        PdfPCell Signature = new PdfPCell(new Phrase("Signature of Parent/Gurdian", Times));
            ////        Signature.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        //Signature.FixedHeight = 15f;
            ////        Signature.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(Signature);

            ////        PdfPCell SignatureLine = new PdfPCell(new Phrase("______________________________________", Times));
            ////        SignatureLine.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        //SignatureLine.FixedHeight = 15f;
            ////        SignatureLine.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(SignatureLine);

            ////        PdfPCell Signature_Date = new PdfPCell(new Phrase("Date", Times));
            ////        Signature_Date.HorizontalAlignment = Element.ALIGN_RIGHT;
            ////        //DISABILITY_Space.Colspan = 2;
            ////        //Signature_Date.FixedHeight = 15f;
            ////        Signature_Date.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(Signature_Date);

            ////        PdfPCell Signature_Date_Line = new PdfPCell(new Phrase("_____________________", Times));
            ////        Signature_Date_Line.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        //Signature_Date_Line.FixedHeight = 15f;
            ////        Signature_Date_Line.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(Signature_Date_Line);

            ////        PdfPCell Line_Last = new PdfPCell(new Phrase("", Times));
            ////        Line_Last.HorizontalAlignment = Element.ALIGN_CENTER;
            ////        Line_Last.Colspan = 6;
            ////        //Line_Last.FixedHeight = 15f;
            ////        Line_Last.BorderWidthBottom = 2f;
            ////        Line_Last.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            ////        MediaclTable.AddCell(Line_Last);
            ////    }
            ////    else
            ////    {
            ////        PdfPCell ALLERGIES = new PdfPCell(new Phrase("My Child has the following ALLERGIES", Times));
            ////        ALLERGIES.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        ALLERGIES.FixedHeight = 15f;
            ////        ALLERGIES.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(ALLERGIES);

            ////        PdfPCell ALLERGIESLine = new PdfPCell(new Phrase("_____________________________________________", Times));
            ////        ALLERGIESLine.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        ALLERGIESLine.FixedHeight = 15f;
            ////        ALLERGIESLine.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(ALLERGIESLine);

            ////        PdfPCell ALLERGIES_Space = new PdfPCell(new Phrase("", Times));
            ////        ALLERGIES_Space.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        ALLERGIES_Space.Colspan = 2;
            ////        ALLERGIES_Space.FixedHeight = 15f;
            ////        ALLERGIES_Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(ALLERGIES_Space);

            ////        PdfPCell DISABILITY = new PdfPCell(new Phrase("Has been diagonosed with following DISABILITY", Times));
            ////        DISABILITY.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        DISABILITY.FixedHeight = 15f;
            ////        DISABILITY.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(DISABILITY);

            ////        PdfPCell DISABILITYLine = new PdfPCell(new Phrase("_____________________________________________", Times));
            ////        DISABILITYLine.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        DISABILITYLine.FixedHeight = 15f;
            ////        DISABILITYLine.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(DISABILITYLine);

            ////        PdfPCell DISABILITY_Date = new PdfPCell(new Phrase("Date", Times));
            ////        DISABILITY_Date.HorizontalAlignment = Element.ALIGN_RIGHT;
            ////        //DISABILITY_Space.Colspan = 2;
            ////        DISABILITY_Date.FixedHeight = 15f;
            ////        DISABILITY_Date.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(DISABILITY_Date);

            ////        PdfPCell DISABILITY_Date_Line = new PdfPCell(new Phrase("_____________________", Times));
            ////        DISABILITY_Date_Line.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        DISABILITY_Date_Line.FixedHeight = 15f;
            ////        DISABILITY_Date_Line.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(DISABILITY_Date_Line);

            ////        PdfPCell MEDICATIONS = new PdfPCell(new Phrase("is taking following MEDICATIONS", Times));
            ////        MEDICATIONS.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        MEDICATIONS.FixedHeight = 15f;
            ////        MEDICATIONS.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(MEDICATIONS);

            ////        PdfPCell MEDICATIONSLine = new PdfPCell(new Phrase("_____________________________________________", Times));
            ////        MEDICATIONSLine.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        MEDICATIONSLine.FixedHeight = 15f;
            ////        MEDICATIONSLine.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(MEDICATIONSLine);

            ////        PdfPCell MEDICATIONS_Space = new PdfPCell(new Phrase("", Times));
            ////        MEDICATIONS_Space.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        MEDICATIONS_Space.Colspan = 2;
            ////        MEDICATIONS_Space.FixedHeight = 15f;
            ////        MEDICATIONS_Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(MEDICATIONS_Space);

            ////        PdfPCell MEDICAL = new PdfPCell(new Phrase("has the following MEDICAL CONDITIONS", Times));
            ////        MEDICAL.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        MEDICAL.FixedHeight = 15f;
            ////        MEDICAL.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(MEDICAL);

            ////        PdfPCell MEDICALLine = new PdfPCell(new Phrase("_____________________________________________", Times));
            ////        MEDICALLine.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        MEDICALLine.FixedHeight = 15f;
            ////        MEDICALLine.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(MEDICALLine);

            ////        PdfPCell MEDICAL_Space = new PdfPCell(new Phrase("", Times));
            ////        MEDICAL_Space.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        MEDICAL_Space.Colspan = 2;
            ////        MEDICAL_Space.FixedHeight = 15f;
            ////        MEDICAL_Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(MEDICAL_Space);

            ////        PdfPCell Diet = new PdfPCell(new Phrase("DIETARY RESTRICTIONS", Times));
            ////        Diet.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        Diet.FixedHeight = 15f;
            ////        Diet.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(Diet);

            ////        PdfPCell DIETLine = new PdfPCell(new Phrase("_____________________________________________", Times));
            ////        DIETLine.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        //DIETLine.Colspan = 2;
            ////        DIETLine.FixedHeight = 15f;
            ////        DIETLine.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(DIETLine);

            ////        PdfPCell DIET_Space = new PdfPCell(new Phrase("", Times));
            ////        DIET_Space.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        DIET_Space.Colspan = 2;
            ////        DIET_Space.FixedHeight = 15f;
            ////        DIET_Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(DIET_Space);

            ////        PdfPCell House = new PdfPCell(new Phrase("HOUSEHOLD CONCERNS ", Times));
            ////        House.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        House.FixedHeight = 15f;
            ////        House.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(House);

            ////        PdfPCell HOUSEHOLDLine = new PdfPCell(new Phrase("_____________________________________________", Times));
            ////        HOUSEHOLDLine.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        //HOUSEHOLDLine.Colspan = 2;
            ////        HOUSEHOLDLine.FixedHeight = 15f;
            ////        HOUSEHOLDLine.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(HOUSEHOLDLine);

            ////        PdfPCell HOUSEHOLD_Space = new PdfPCell(new Phrase("", Times));
            ////        HOUSEHOLD_Space.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        HOUSEHOLD_Space.Colspan = 2;
            ////        HOUSEHOLD_Space.FixedHeight = 15f;
            ////        HOUSEHOLD_Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(HOUSEHOLD_Space);

            ////        PdfPCell Devp = new PdfPCell(new Phrase("DEVELOPMENTAL CONCERNS ", Times));
            ////        Devp.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        Devp.FixedHeight = 15f;
            ////        Devp.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(Devp);

            ////        PdfPCell DevpLine = new PdfPCell(new Phrase("_____________________________________________", Times));
            ////        DevpLine.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        //DevpLine.Colspan = 2;
            ////        DevpLine.FixedHeight = 15f;
            ////        DevpLine.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(DevpLine);

            ////        PdfPCell Devp_Space = new PdfPCell(new Phrase("", Times));
            ////        Devp_Space.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        Devp_Space.Colspan = 2;
            ////        Devp_Space.FixedHeight = 15f;
            ////        Devp_Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(Devp_Space);

            ////        PdfPCell AltFnd = new PdfPCell(new Phrase("Alternate Fund", Times));
            ////        AltFnd.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        AltFnd.FixedHeight = 15f;
            ////        AltFnd.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(AltFnd);

            ////        PdfPCell AltFnddesc = new PdfPCell(new Phrase("_____________________________________________", Times));
            ////        AltFnddesc.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        AltFnddesc.FixedHeight = 15f;
            ////        AltFnddesc.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(AltFnddesc);

            ////        PdfPCell Trans = new PdfPCell(new Phrase("Transport", Times));
            ////        Trans.HorizontalAlignment = Element.ALIGN_RIGHT;
            ////        Trans.FixedHeight = 15f;
            ////        Trans.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(Trans);


            ////        PdfPCell Trans_Space = new PdfPCell(new Phrase("_____________________", Times));
            ////        Trans_Space.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        Trans_Space.FixedHeight = 15f;
            ////        Trans_Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(Trans_Space);

            ////        PdfPCell Pick = new PdfPCell(new Phrase("Pickup", Times));
            ////        Pick.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        Pick.FixedHeight = 15f;
            ////        Pick.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(Pick);


            ////        PdfPCell PickSpace = new PdfPCell(new Phrase("_____________________________________________", Times));
            ////        PickSpace.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        PickSpace.FixedHeight = 15f;
            ////        PickSpace.Colspan = 3;
            ////        PickSpace.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(PickSpace);

            ////        PdfPCell Drop = new PdfPCell(new Phrase("Dropoff", Times));
            ////        Drop.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        Drop.FixedHeight = 15f;
            ////        Drop.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(Drop);


            ////        PdfPCell dropSpace = new PdfPCell(new Phrase("_____________________________________________", Times));
            ////        dropSpace.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        dropSpace.FixedHeight = 15f;
            ////        dropSpace.Colspan = 3;
            ////        dropSpace.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(dropSpace);

            ////        PdfPCell Last_Space = new PdfPCell(new Phrase("", Times));
            ////        Last_Space.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        Last_Space.Colspan = 4;
            ////        Last_Space.FixedHeight = 15f;
            ////        Last_Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(Last_Space);

            ////        PdfPCell Signature = new PdfPCell(new Phrase("Signature of Parent/Gurdian", Times));
            ////        Signature.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        //Signature.FixedHeight = 15f;
            ////        Signature.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(Signature);

            ////        PdfPCell SignatureLine = new PdfPCell(new Phrase("______________________________________", Times));
            ////        SignatureLine.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        //SignatureLine.FixedHeight = 15f;
            ////        SignatureLine.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(SignatureLine);

            ////        PdfPCell Signature_Date = new PdfPCell(new Phrase("Date", Times));
            ////        Signature_Date.HorizontalAlignment = Element.ALIGN_RIGHT;
            ////        //DISABILITY_Space.Colspan = 2;
            ////        //Signature_Date.FixedHeight = 15f;
            ////        Signature_Date.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(Signature_Date);

            ////        PdfPCell Signature_Date_Line = new PdfPCell(new Phrase("_____________________", Times));
            ////        Signature_Date_Line.HorizontalAlignment = Element.ALIGN_LEFT;
            ////        //Signature_Date_Line.FixedHeight = 15f;
            ////        Signature_Date_Line.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ////        MediaclTable.AddCell(Signature_Date_Line);

            ////        PdfPCell Line_Last = new PdfPCell(new Phrase("", Times));
            ////        Line_Last.HorizontalAlignment = Element.ALIGN_CENTER;
            ////        Line_Last.Colspan = 6;
            ////        //Line_Last.FixedHeight = 15f;
            ////        Line_Last.BorderWidthBottom = 2f;
            ////        Line_Last.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            ////        MediaclTable.AddCell(Line_Last);
            ////    }
            ////}

            ////}
            ////document.Add(MediaclTable);
            //#endregion
            //}



        }

        private void SetLine()
        {
            cb.SetLineWidth(2f);
            cb.SetLineCap(5);
            cb.MoveTo(X_Pos, Y_Pos);
            cb.LineTo(780, Y_Pos);
            cb.Stroke();
        }

        private string Get_Member_Name(string Mem_Seq, string NameFormat)
        {
            string Member_NAme = string.Empty;
            foreach (CaseSnpEntity drCaseSnp in casesnpList)
            {
                if (Mem_Seq == drCaseSnp.FamilySeq.Trim())
                {
                    if (NameFormat == "First")
                    {
                        Member_NAme = drCaseSnp.FamilySeq.Trim(); break;
                    }
                    else
                        Member_NAme = LookupDataAccess.GetMemberName(drCaseSnp.NameixFi.Trim(), drCaseSnp.NameixMi.Trim(), drCaseSnp.NameixLast.Trim(), "1") + "  "; break;
                }
            }

            return Member_NAme;
        }

        DataTable dtIncome = new DataTable();
        private string Get_IncomeType_Desc(string Type_Code)
        {
            string Income_Desc = string.Empty;
            foreach (DataRow drIncome in dtIncome.Rows)
            {
                if (Type_Code == drIncome["Code"].ToString().Trim())
                {
                    Income_Desc = drIncome["LookUpDesc"].ToString().Trim(); break;
                }
            }

            return Income_Desc;
        }

        #endregion

        private void CaseSpmPdf(Document document)
        {
            BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            iTextSharp.text.Font helvetica = new iTextSharp.text.Font(bf_times, 12, 1);
            BaseFont bf_helv = helvetica.GetCalculatedBaseFont(false);
            iTextSharp.text.Font TimesUnderline = new iTextSharp.text.Font(bf_times, 9, 4);
            BaseFont bf_TimesUnderline = TimesUnderline.GetCalculatedBaseFont(true);

            iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 9);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 8);
            iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 8, 3);
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_times, 9, 1);
            iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 8, 2);
            iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 9, 4);

            PdfPTable SPMTable = new PdfPTable(5);
            SPMTable.TotalWidth = 550f;
            SPMTable.WidthPercentage = 100;
            SPMTable.LockedWidth = true;
            float[] widths2 = new float[] { 15f, 70f, 40f, 35f, 20f };
            SPMTable.SetWidths(widths2);
            SPMTable.HorizontalAlignment = Element.ALIGN_CENTER;

            string ProgramName = string.Empty;
            if (programEntity != null)
                ProgramName = programEntity.AgencyName;

            PdfPCell H1 = new PdfPCell(new Phrase("Hierarchy:" + CASEMSTList[0].ApplAgency + "-" + CASEMSTList[0].ApplDept + "-" + CASEMSTList[0].ApplProgram + " " + CASEMSTList[0].ApplYr + "  " + ProgramName, TblFontBold));
            H1.HorizontalAlignment = Element.ALIGN_LEFT;
            H1.Colspan = 2;
            H1.FixedHeight = 15f;
            H1.Border = iTextSharp.text.Rectangle.NO_BORDER;
            SPMTable.AddCell(H1);

            PdfPCell H2 = new PdfPCell(new Phrase("App:" + CASEMSTList[0].ApplNo + "  " + LookupDataAccess.GetMemberName(SelSnp.NameixFi, SelSnp.NameixMi, SelSnp.NameixLast, BaseForm.BaseHierarchyCnFormat.ToString()), TblFontBold));
            H2.HorizontalAlignment = Element.ALIGN_RIGHT;
            H2.Colspan = 3;
            H2.FixedHeight = 15f;
            H2.Border = iTextSharp.text.Rectangle.NO_BORDER;
            SPMTable.AddCell(H2);

            PdfPCell S2 = new PdfPCell(new Phrase("", TblFontBold));
            S2.HorizontalAlignment = Element.ALIGN_CENTER;
            S2.Colspan = 5;
            S2.FixedHeight = 15f;
            S2.Border = iTextSharp.text.Rectangle.NO_BORDER;
            SPMTable.AddCell(S2);

            PdfPCell Header = new PdfPCell(new Phrase("Service Plans", TblFontBold));
            Header.HorizontalAlignment = Element.ALIGN_CENTER;
            Header.Colspan = 5;
            Header.FixedHeight = 15f;
            Header.Border = iTextSharp.text.Rectangle.BOX;
            SPMTable.AddCell(Header);

            string[] HeaderSeq = { "Code", "Description", "Site", "CaseWorker", "Start Date" };
            for (int i = 0; i < HeaderSeq.Length; ++i)
            {
                PdfPCell cell = new PdfPCell(new Phrase(HeaderSeq[i], TblFontBold));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                //cell.FixedHeight = 15f;
                cell.Border = iTextSharp.text.Rectangle.BOX;
                SPMTable.AddCell(cell);
            }

            bool first = false;
            foreach (CASESPMEntity Entity in spmlist)
            {
                if (first)
                {
                    PdfPCell Header1 = new PdfPCell(new Phrase("Service Plan", TblFontBold));
                    Header1.HorizontalAlignment = Element.ALIGN_CENTER;
                    Header1.Colspan = 5;
                    Header1.FixedHeight = 15f;
                    Header1.Border = iTextSharp.text.Rectangle.BOX;
                    SPMTable.AddCell(Header1);

                    string[] HeaderSeq1 = { "Code", "Description", "Site", "CaseWorker", "Start Date" };
                    for (int i = 0; i < HeaderSeq1.Length; ++i)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(HeaderSeq1[i], TblFontBold));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        //cell.FixedHeight = 15f;
                        cell.Border = iTextSharp.text.Rectangle.BOX;
                        SPMTable.AddCell(cell);
                    }
                }


                PdfPCell A1 = new PdfPCell(new Phrase(Entity.service_plan, TableFont));
                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                A1.Border = iTextSharp.text.Rectangle.BOX;
                SPMTable.AddCell(A1);

                PdfPCell A2 = new PdfPCell(new Phrase(Entity.Sp0_Desc, TableFont));
                A2.HorizontalAlignment = Element.ALIGN_LEFT;
                A2.Border = iTextSharp.text.Rectangle.BOX;
                SPMTable.AddCell(A2);

                PdfPCell A3 = new PdfPCell(new Phrase(Entity.Site_Desc, TableFont));
                A3.HorizontalAlignment = Element.ALIGN_LEFT;
                A3.Border = iTextSharp.text.Rectangle.BOX;
                SPMTable.AddCell(A3);

                PdfPCell A4 = new PdfPCell(new Phrase(Get_CaseWorker_DESC(Entity.caseworker), TableFont));
                A4.HorizontalAlignment = Element.ALIGN_LEFT;
                A4.Border = iTextSharp.text.Rectangle.BOX;
                SPMTable.AddCell(A4);

                PdfPCell A5 = new PdfPCell(new Phrase(LookupDataAccess.Getdate(Entity.startdate.Trim()), TableFont));
                A5.HorizontalAlignment = Element.ALIGN_LEFT;
                A5.Border = iTextSharp.text.Rectangle.BOX;
                SPMTable.AddCell(A5);


                if (SPMTable.Rows.Count > 0)
                {
                    document.Add(SPMTable);
                    SPMTable.DeleteBodyRows();
                }
                if (actList.Count > 0)
                {
                    SP_Programs_List = _model.lookupDataAccess.Get_SerPlan_Prog_List(BaseForm.UserProfile.UserID, Entity.service_plan, string.Empty);

                    List<CASEACTEntity> SelActList = actList.FindAll(u => u.Service_plan.Equals(Entity.service_plan) && u.SPM_Seq.Equals(Entity.Seq));
                    SelActList = SelActList.OrderBy(u => u.Service_plan).ThenBy(u => u.SPM_Seq).ThenBy(u => u.Branch).ThenBy(u => u.Group).ThenBy(u => u.ACT_Code).ToList();

                    if (SelActList.Count > 0) CASEACTPdf(document, SelActList);

                    List<CASEMSEntity> SelMSList = MSlist.FindAll(u => u.Service_plan.Equals(Entity.service_plan) && u.SPM_Seq.Equals(Entity.Seq));
                    SelMSList = SelMSList.OrderBy(u => u.Service_plan).ThenBy(u => u.SPM_Seq).ThenBy(u => u.Branch).ThenBy(u => u.Group).ThenBy(u => u.MS_Code).ToList();

                    if (SelMSList.Count > 0) CASEMSPdf(document, SelMSList);

                    if (SelActList.Count > 0 || SelMSList.Count > 0)
                    {
                        first = true;
                    }
                    else first = false;
                }

            }

            document.Add(SPMTable);

        }


        private void CASEACTPdf(Document document, List<CASEACTEntity> SelList)
        {
            BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            iTextSharp.text.Font helvetica = new iTextSharp.text.Font(bf_times, 12, 1);
            BaseFont bf_helv = helvetica.GetCalculatedBaseFont(false);
            iTextSharp.text.Font TimesUnderline = new iTextSharp.text.Font(bf_times, 9, 4);
            BaseFont bf_TimesUnderline = TimesUnderline.GetCalculatedBaseFont(true);

            iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 9);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 8);
            iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 8, 3);
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_times, 9, 1);
            iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 8, 2);
            iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 9, 4);

            PdfPTable ACTTable = new PdfPTable(6);
            ACTTable.TotalWidth = 550f;
            ACTTable.WidthPercentage = 100;
            ACTTable.LockedWidth = true;
            float[] widths2 = new float[] { 50f, 40f, 18f, 30f, 25f, 25f };
            ACTTable.SetWidths(widths2);
            ACTTable.HorizontalAlignment = Element.ALIGN_CENTER;


            PdfPCell Header = new PdfPCell(new Phrase("Services Activities", TblFontBold));
            Header.HorizontalAlignment = Element.ALIGN_LEFT;
            Header.Colspan = 6;
            Header.FixedHeight = 15f;
            Header.Border = iTextSharp.text.Rectangle.BOX;
            ACTTable.AddCell(Header);

            string[] HeaderSeq = { "Branch", "CA Description", "Date", "Site", "CaseWorker", "Program" };
            for (int i = 0; i < HeaderSeq.Length; ++i)
            {
                PdfPCell cell = new PdfPCell(new Phrase(HeaderSeq[i], TblFontBold));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                //cell.FixedHeight = 15f;
                cell.Border = iTextSharp.text.Rectangle.BOX;
                ACTTable.AddCell(cell);
            }

            foreach (CASEACTEntity Entity in SelList)
            {
                string BranchDesc = string.Empty;
                if (dtSP1.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtSP1.Rows)
                    {
                        switch (Entity.Branch)
                        {
                            case "P": if (dr["SP0_PBRANCH_CODE"].ToString() == Entity.Branch) BranchDesc = dr["SP0_PBRANCH_DESC"].ToString(); break;
                            case "1": if (dr["SP0_BRANCH1_CODE"].ToString() == Entity.Branch) BranchDesc = dr["SP0_BRANCH1_DESC"].ToString(); break;
                            case "2": if (dr["SP0_BRANCH2_CODE"].ToString() == Entity.Branch) BranchDesc = dr["SP0_BRANCH2_DESC"].ToString(); break;
                            case "3": if (dr["SP0_BRANCH3_CODE"].ToString() == Entity.Branch) BranchDesc = dr["SP0_BRANCH3_DESC"].ToString(); break;
                            case "4": if (dr["SP0_BRANCH4_CODE"].ToString() == Entity.Branch) BranchDesc = dr["SP0_BRANCH4_DESC"].ToString(); break;
                            case "5": if (dr["SP0_BRANCH5_CODE"].ToString() == Entity.Branch) BranchDesc = dr["SP0_BRANCH5_DESC"].ToString(); break;
                        }
                    }
                }

                PdfPCell A1 = new PdfPCell(new Phrase(BranchDesc, TableFont));
                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                A1.Border = iTextSharp.text.Rectangle.BOX;
                ACTTable.AddCell(A1);

                //PdfPCell A2 = new PdfPCell(new Phrase(Entity.Group, TableFont));
                //A2.HorizontalAlignment = Element.ALIGN_LEFT;
                //A2.Border = iTextSharp.text.Rectangle.BOX;
                //ACTTable.AddCell(A2);

                PdfPCell A3 = new PdfPCell(new Phrase(Get_CA_Desc(Entity.ACT_Code.Trim()), TableFont));
                A3.HorizontalAlignment = Element.ALIGN_LEFT;
                A3.Border = iTextSharp.text.Rectangle.BOX;
                ACTTable.AddCell(A3);

                PdfPCell A4 = new PdfPCell(new Phrase(LookupDataAccess.Getdate(Entity.ACT_Date.Trim()), TableFont));
                A4.HorizontalAlignment = Element.ALIGN_LEFT;
                A4.Border = iTextSharp.text.Rectangle.BOX;
                ACTTable.AddCell(A4);

                string SiteDesc = string.Empty;
                if (dsSite.Tables.Count > 0)
                {
                    DataTable Sites_Table = dsSite.Tables[0];
                    if (Sites_Table.Rows.Count > 0)
                    {
                        foreach (DataRow dr in Sites_Table.Rows)
                        {
                            if (Entity.Site.Trim() == dr["SITE_NUMBER"].ToString().Trim())
                            {
                                SiteDesc = dr["SITE_NAME"].ToString().Trim();
                                break;
                            }
                        }
                    }
                }

                PdfPCell A5 = new PdfPCell(new Phrase(SiteDesc, TableFont));
                A5.HorizontalAlignment = Element.ALIGN_LEFT;
                A5.Border = iTextSharp.text.Rectangle.BOX;
                ACTTable.AddCell(A5);

                PdfPCell A6 = new PdfPCell(new Phrase(Get_CaseWorker_DESC(Entity.Caseworker.Trim()), TableFont));
                A6.HorizontalAlignment = Element.ALIGN_LEFT;
                A6.Border = iTextSharp.text.Rectangle.BOX;
                ACTTable.AddCell(A6);

                PdfPCell A7 = new PdfPCell(new Phrase(Set_SP_Program_Text(Entity.Act_PROG.Trim()), TableFont));
                A7.HorizontalAlignment = Element.ALIGN_LEFT;
                A7.Border = iTextSharp.text.Rectangle.BOX;
                ACTTable.AddCell(A7);

            }

            document.Add(ACTTable);


        }

        private void CASEMSPdf(Document document, List<CASEMSEntity> SelList)
        {
            BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            iTextSharp.text.Font helvetica = new iTextSharp.text.Font(bf_times, 12, 1);
            BaseFont bf_helv = helvetica.GetCalculatedBaseFont(false);
            iTextSharp.text.Font TimesUnderline = new iTextSharp.text.Font(bf_times, 9, 4);
            BaseFont bf_TimesUnderline = TimesUnderline.GetCalculatedBaseFont(true);

            iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 9);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 8);
            iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 8, 3);
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_times, 9, 1);
            iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 8, 2);
            iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 9, 4);

            PdfPTable MSTable = new PdfPTable(6);
            MSTable.TotalWidth = 550f;
            MSTable.WidthPercentage = 100;
            MSTable.LockedWidth = true;
            float[] widths2 = new float[] { 50f, 40f, 18f, 30f, 25f, 25f };
            MSTable.SetWidths(widths2);
            MSTable.HorizontalAlignment = Element.ALIGN_CENTER;

            PdfPCell Header = new PdfPCell(new Phrase("Milestones/Outcomes", TblFontBold));
            Header.HorizontalAlignment = Element.ALIGN_LEFT;
            Header.Colspan = 6;
            Header.FixedHeight = 15f;
            Header.Border = iTextSharp.text.Rectangle.BOX;
            MSTable.AddCell(Header);

            string[] HeaderSeq = { "Branch", "MS Description", "Date", "Site", "CaseWorker", "Program" };
            for (int i = 0; i < HeaderSeq.Length; ++i)
            {
                PdfPCell cell = new PdfPCell(new Phrase(HeaderSeq[i], TblFontBold));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                //cell.FixedHeight = 15f;
                cell.Border = iTextSharp.text.Rectangle.BOX;
                MSTable.AddCell(cell);
            }

            foreach (CASEMSEntity Entity in SelList)
            {
                string BranchDesc = string.Empty;
                if (dtSP1.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtSP1.Rows)
                    {
                        switch (Entity.Branch)
                        {
                            case "P": if (dr["SP0_PBRANCH_CODE"].ToString() == Entity.Branch) BranchDesc = dr["SP0_PBRANCH_DESC"].ToString(); break;
                            case "1": if (dr["SP0_BRANCH1_CODE"].ToString() == Entity.Branch) BranchDesc = dr["SP0_BRANCH1_DESC"].ToString(); break;
                            case "2": if (dr["SP0_BRANCH2_CODE"].ToString() == Entity.Branch) BranchDesc = dr["SP0_BRANCH2_DESC"].ToString(); break;
                            case "3": if (dr["SP0_BRANCH3_CODE"].ToString() == Entity.Branch) BranchDesc = dr["SP0_BRANCH3_DESC"].ToString(); break;
                            case "4": if (dr["SP0_BRANCH4_CODE"].ToString() == Entity.Branch) BranchDesc = dr["SP0_BRANCH4_DESC"].ToString(); break;
                            case "5": if (dr["SP0_BRANCH5_CODE"].ToString() == Entity.Branch) BranchDesc = dr["SP0_BRANCH5_DESC"].ToString(); break;
                        }
                    }
                }

                PdfPCell A1 = new PdfPCell(new Phrase(BranchDesc, TableFont));
                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                A1.Border = iTextSharp.text.Rectangle.BOX;
                MSTable.AddCell(A1);

                //PdfPCell A2 = new PdfPCell(new Phrase(Entity.Group, TableFont));
                //A2.HorizontalAlignment = Element.ALIGN_LEFT;
                //A2.Border = iTextSharp.text.Rectangle.BOX;
                //MSTable.AddCell(A2);

                PdfPCell A3 = new PdfPCell(new Phrase(Get_MS_Desc(Entity.MS_Code.Trim()), TableFont));
                A3.HorizontalAlignment = Element.ALIGN_LEFT;
                A3.Border = iTextSharp.text.Rectangle.BOX;
                MSTable.AddCell(A3);

                PdfPCell A4 = new PdfPCell(new Phrase(LookupDataAccess.Getdate(Entity.Date.Trim()), TableFont));
                A4.HorizontalAlignment = Element.ALIGN_LEFT;
                A4.Border = iTextSharp.text.Rectangle.BOX;
                MSTable.AddCell(A4);

                string SiteDesc = string.Empty;
                if (dsSite.Tables.Count > 0)
                {
                    DataTable Sites_Table = dsSite.Tables[0];
                    if (Sites_Table.Rows.Count > 0)
                    {
                        foreach (DataRow dr in Sites_Table.Rows)
                        {
                            if (Entity.Site.Trim() == dr["SITE_NUMBER"].ToString().Trim())
                            {
                                SiteDesc = dr["SITE_NAME"].ToString().Trim();
                                break;
                            }
                        }
                    }
                }
                PdfPCell A5 = new PdfPCell(new Phrase(SiteDesc, TableFont));
                A5.HorizontalAlignment = Element.ALIGN_LEFT;
                A5.Border = iTextSharp.text.Rectangle.BOX;
                MSTable.AddCell(A5);

                PdfPCell A6 = new PdfPCell(new Phrase(Get_CaseWorker_DESC(Entity.CaseWorker.Trim()), TableFont));
                A6.HorizontalAlignment = Element.ALIGN_LEFT;
                A6.Border = iTextSharp.text.Rectangle.BOX;
                MSTable.AddCell(A6);

                PdfPCell A7 = new PdfPCell(new Phrase(Set_SP_Program_Text(Entity.Acty_PROG.Trim()), TableFont));
                A7.HorizontalAlignment = Element.ALIGN_LEFT;
                A7.Border = iTextSharp.text.Rectangle.BOX;
                MSTable.AddCell(A7);

            }

            document.Add(MSTable);


        }

        private void CaseCONTPdf(Document document)
        {
            BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            iTextSharp.text.Font helvetica = new iTextSharp.text.Font(bf_times, 12, 1);
            BaseFont bf_helv = helvetica.GetCalculatedBaseFont(false);
            iTextSharp.text.Font TimesUnderline = new iTextSharp.text.Font(bf_times, 9, 4);
            BaseFont bf_TimesUnderline = TimesUnderline.GetCalculatedBaseFont(true);

            iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 9);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 8);
            iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 8, 3);
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_times, 9, 1);
            iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 8, 2);
            iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 9, 4);

            BaseFont bf_Calibri = BaseFont.CreateFont("c:/windows/fonts/calibri.ttf", BaseFont.WINANSI, BaseFont.EMBEDDED);
            iTextSharp.text.Font fc1 = new iTextSharp.text.Font(bf_Calibri, 10, 1, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#6499c1")));

            string ProgramName = string.Empty;
            if (programEntity != null)
                ProgramName = programEntity.AgencyName;

            /******************************* Head  1 ********************************************/

            PdfPTable mainHeader = new PdfPTable(2);
            mainHeader.TotalWidth = 510f;
            mainHeader.WidthPercentage = 100;
            mainHeader.LockedWidth = true;
            float[] Headerwidths = new float[] { 50f, 17f };
            mainHeader.SetWidths(Headerwidths);
            mainHeader.HorizontalAlignment = Element.ALIGN_CENTER;
            mainHeader.SpacingAfter = 07f;

            PdfPCell AppName = new PdfPCell(new Phrase("Applicant Name: " + LookupDataAccess.GetMemberName(SelSnp.NameixFi, SelSnp.NameixMi, SelSnp.NameixLast, BaseForm.BaseHierarchyCnFormat.ToString()), fc1));
            AppName.HorizontalAlignment = Element.ALIGN_LEFT;
            AppName.Border = iTextSharp.text.Rectangle.NO_BORDER;
            mainHeader.AddCell(AppName);

            PdfPCell Application = new PdfPCell(new Phrase("App#: " + CASEMSTList[0].ApplNo, fc1));
            Application.HorizontalAlignment = Element.ALIGN_RIGHT;
            Application.Border = iTextSharp.text.Rectangle.NO_BORDER;
            mainHeader.AddCell(Application);

            PdfPCell space = new PdfPCell(new Phrase("", fc1));
            space.HorizontalAlignment = Element.ALIGN_LEFT;
            space.Border = iTextSharp.text.Rectangle.NO_BORDER;
            mainHeader.AddCell(space);

            PdfPCell Hierarchy = new PdfPCell(new Phrase("Intake Hierarchy: " + CASEMSTList[0].ApplAgency + "-" + CASEMSTList[0].ApplDept + "-" + CASEMSTList[0].ApplProgram + " " + CASEMSTList[0].ApplYr + "  " + ProgramName, fc1));
            Hierarchy.HorizontalAlignment = Element.ALIGN_RIGHT;
            Hierarchy.Border = iTextSharp.text.Rectangle.NO_BORDER;
            mainHeader.AddCell(Hierarchy);

            /***************************************************************************/

            PdfPTable ContTable = new PdfPTable(6);
            ContTable.TotalWidth = 550f;
            ContTable.WidthPercentage = 100;
            ContTable.LockedWidth = true;
            float[] widths2 = new float[] { 20f, 70f, 15f, 15f, 20f, 35f };
            ContTable.SetWidths(widths2);
            ContTable.HorizontalAlignment = Element.ALIGN_CENTER;

            //string ProgramName = string.Empty;
            //if (programEntity != null)
            //    ProgramName = programEntity.AgencyName;

            /*PdfPCell H1 = new PdfPCell(new Phrase("Hierarchy:" + CASEMSTList[0].ApplAgency + "-" + CASEMSTList[0].ApplDept + "-" + CASEMSTList[0].ApplProgram + " " + CASEMSTList[0].ApplYr + "  " + ProgramName, TblFontBold));
            H1.HorizontalAlignment = Element.ALIGN_LEFT;
            H1.Colspan = 2;
            H1.FixedHeight = 15f;
            H1.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ContTable.AddCell(H1);

            PdfPCell H2 = new PdfPCell(new Phrase("App:" + CASEMSTList[0].ApplNo + "  " + LookupDataAccess.GetMemberName(SelSnp.NameixFi, SelSnp.NameixMi, SelSnp.NameixLast, BaseForm.BaseHierarchyCnFormat.ToString()), TblFontBold));
            H2.HorizontalAlignment = Element.ALIGN_RIGHT;
            H2.Colspan = 4;
            H2.FixedHeight = 15f;
            H2.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ContTable.AddCell(H2);*/

            PdfPCell S2 = new PdfPCell(new Phrase("", TblFontBold));
            S2.HorizontalAlignment = Element.ALIGN_CENTER;
            S2.Colspan = 6;
            S2.FixedHeight = 15f;
            S2.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ContTable.AddCell(S2);

            PdfPCell Header = new PdfPCell(new Phrase("Contact Data", TblFontBold));
            Header.HorizontalAlignment = Element.ALIGN_CENTER;
            Header.Colspan = 6;
            Header.FixedHeight = 15f;
            Header.Border = iTextSharp.text.Rectangle.BOX;
            ContTable.AddCell(Header);

            string[] HeaderSeq = { "Date", "Contact Name", "Where", "Time", "Language", "Case Worker" };
            for (int i = 0; i < HeaderSeq.Length; ++i)
            {
                PdfPCell cell = new PdfPCell(new Phrase(HeaderSeq[i], TblFontBold));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                //cell.FixedHeight = 15f;
                cell.Border = iTextSharp.text.Rectangle.BOX;
                ContTable.AddCell(cell);
            }

            foreach (CASECONTEntity Entity in ContList)
            {
                PdfPCell A1 = new PdfPCell(new Phrase(LookupDataAccess.Getdate(Entity.Cont_Date.Trim()), TableFont));
                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                A1.Border = iTextSharp.text.Rectangle.BOX;
                ContTable.AddCell(A1);

                PdfPCell A2 = new PdfPCell(new Phrase(Entity.Contact_Name, TableFont));
                A2.HorizontalAlignment = Element.ALIGN_LEFT;
                A2.Border = iTextSharp.text.Rectangle.BOX;
                ContTable.AddCell(A2);

                PdfPCell A3 = new PdfPCell(new Phrase(Entity.How_Where, TableFont));
                A3.HorizontalAlignment = Element.ALIGN_LEFT;
                A3.Border = iTextSharp.text.Rectangle.BOX;
                ContTable.AddCell(A3);

                PdfPCell A4 = new PdfPCell(new Phrase(Entity.Duration, TableFont));
                A4.HorizontalAlignment = Element.ALIGN_LEFT;
                A4.Border = iTextSharp.text.Rectangle.BOX;
                ContTable.AddCell(A4);

                PdfPCell A5 = new PdfPCell(new Phrase(Get_Language_Desc(Entity.Language), TableFont));
                A5.HorizontalAlignment = Element.ALIGN_LEFT;
                A5.Border = iTextSharp.text.Rectangle.BOX;
                ContTable.AddCell(A5);

                PdfPCell A6 = new PdfPCell(new Phrase(Get_CaseWorker_DESC(Entity.CaseWorker), TableFont));
                A6.HorizontalAlignment = Element.ALIGN_LEFT;
                A6.Border = iTextSharp.text.Rectangle.BOX;
                ContTable.AddCell(A6);
            }

            document.Add(ContTable);

        }


        private void ACTREFSPdf(Document document)
        {
            BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            iTextSharp.text.Font helvetica = new iTextSharp.text.Font(bf_times, 12, 1);
            BaseFont bf_helv = helvetica.GetCalculatedBaseFont(false);
            iTextSharp.text.Font TimesUnderline = new iTextSharp.text.Font(bf_times, 9, 4);
            BaseFont bf_TimesUnderline = TimesUnderline.GetCalculatedBaseFont(true);

            iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 9);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 8);
            iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 8, 3);
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_times, 9, 1);
            iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 8, 2);
            iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 9, 4);

            PdfPTable ActrefTable = new PdfPTable(6);
            ActrefTable.TotalWidth = 550f;
            ActrefTable.WidthPercentage = 100;
            ActrefTable.LockedWidth = true;
            float[] widths2 = new float[] { 20f, 30f, 15f, 60f, 30f, 18f };
            ActrefTable.SetWidths(widths2);
            ActrefTable.HorizontalAlignment = Element.ALIGN_CENTER;

            string ProgramName = string.Empty;
            if (programEntity != null)
                ProgramName = programEntity.AgencyName;

            PdfPCell H1 = new PdfPCell(new Phrase("Hierarchy:" + CASEMSTList[0].ApplAgency + "-" + CASEMSTList[0].ApplDept + "-" + CASEMSTList[0].ApplProgram + " " + CASEMSTList[0].ApplYr + "  " + ProgramName, TblFontBold));
            H1.HorizontalAlignment = Element.ALIGN_LEFT;
            H1.Colspan = 3;
            H1.FixedHeight = 15f;
            H1.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ActrefTable.AddCell(H1);

            PdfPCell H2 = new PdfPCell(new Phrase("App:" + CASEMSTList[0].ApplNo + "  " + LookupDataAccess.GetMemberName(SelSnp.NameixFi, SelSnp.NameixMi, SelSnp.NameixLast, BaseForm.BaseHierarchyCnFormat.ToString()), TblFontBold));
            H2.HorizontalAlignment = Element.ALIGN_RIGHT;
            H2.Colspan = 3;
            H2.FixedHeight = 15f;
            H2.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ActrefTable.AddCell(H2);

            PdfPCell S2 = new PdfPCell(new Phrase("", TblFontBold));
            S2.HorizontalAlignment = Element.ALIGN_CENTER;
            S2.Colspan = 6;
            S2.FixedHeight = 15f;
            S2.Border = iTextSharp.text.Rectangle.NO_BORDER;
            ActrefTable.AddCell(S2);

            PdfPCell Header = new PdfPCell(new Phrase("REFER From/TO", TblFontBold));
            Header.HorizontalAlignment = Element.ALIGN_CENTER;
            Header.Colspan = 6;
            Header.FixedHeight = 15f;
            Header.Border = iTextSharp.text.Rectangle.BOX;
            ActrefTable.AddCell(Header);

            string[] HeaderSeq = { "Date", "Refer From/TO", "Code", "Agency Name", "City", "State" };
            for (int i = 0; i < HeaderSeq.Length; ++i)
            {
                PdfPCell cell = new PdfPCell(new Phrase(HeaderSeq[i], TblFontBold));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                //cell.FixedHeight = 15f;
                cell.Border = iTextSharp.text.Rectangle.BOX;
                ActrefTable.AddCell(cell);
            }

            bool Ref_Exists = false; string AgyName = string.Empty;
            foreach (ACTREFSEntity Entity in Actrefslist)
            {

                Ref_Exists = false;
                foreach (CASEREFEntity Entity1 in CASEREFREF_List)
                {
                    if (Entity1.Code == Entity.Code)
                    {
                        Ref_Exists = true;

                        PdfPCell A1 = new PdfPCell(new Phrase(LookupDataAccess.Getdate(Entity.Date.Trim()), TableFont));
                        A1.HorizontalAlignment = Element.ALIGN_LEFT;
                        A1.Border = iTextSharp.text.Rectangle.BOX;
                        ActrefTable.AddCell(A1);

                        PdfPCell A2 = new PdfPCell(new Phrase(Entity.Type == "T" ? "Referred To" : "Referred From", TableFont));
                        A2.HorizontalAlignment = Element.ALIGN_LEFT;
                        A2.Border = iTextSharp.text.Rectangle.BOX;
                        ActrefTable.AddCell(A2);

                        PdfPCell A3 = new PdfPCell(new Phrase(Entity1.Code, TableFont));
                        A3.HorizontalAlignment = Element.ALIGN_LEFT;
                        A3.Border = iTextSharp.text.Rectangle.BOX;
                        ActrefTable.AddCell(A3);

                        PdfPCell A4 = new PdfPCell(new Phrase(Entity1.Name1, TableFont));
                        A4.HorizontalAlignment = Element.ALIGN_LEFT;
                        A4.Border = iTextSharp.text.Rectangle.BOX;
                        ActrefTable.AddCell(A4);

                        PdfPCell A5 = new PdfPCell(new Phrase(Entity1.City, TableFont));
                        A5.HorizontalAlignment = Element.ALIGN_LEFT;
                        A5.Border = iTextSharp.text.Rectangle.BOX;
                        ActrefTable.AddCell(A5);

                        PdfPCell A6 = new PdfPCell(new Phrase(Entity1.State, TableFont));
                        A6.HorizontalAlignment = Element.ALIGN_LEFT;
                        A6.Border = iTextSharp.text.Rectangle.BOX;
                        ActrefTable.AddCell(A6);

                        break;
                    }
                }

                if (!Ref_Exists)
                {
                    PdfPCell A1 = new PdfPCell(new Phrase(LookupDataAccess.Getdate(Entity.Date.Trim()), TableFont));
                    A1.HorizontalAlignment = Element.ALIGN_LEFT;
                    A1.Border = iTextSharp.text.Rectangle.BOX;
                    ActrefTable.AddCell(A1);

                    PdfPCell A2 = new PdfPCell(new Phrase(Entity.Type == "T" ? "Referred To" : "Referred From", TableFont));
                    A2.HorizontalAlignment = Element.ALIGN_LEFT;
                    A2.Border = iTextSharp.text.Rectangle.BOX;
                    ActrefTable.AddCell(A2);

                    PdfPCell A3 = new PdfPCell(new Phrase(Entity.Code, TableFont));
                    A3.HorizontalAlignment = Element.ALIGN_LEFT;
                    A3.Border = iTextSharp.text.Rectangle.BOX;
                    ActrefTable.AddCell(A3);

                    PdfPCell A4 = new PdfPCell(new Phrase("Not Defined in 'CASEREF'", TableFont));
                    A4.HorizontalAlignment = Element.ALIGN_LEFT;
                    A4.Border = iTextSharp.text.Rectangle.BOX;
                    ActrefTable.AddCell(A4);

                    PdfPCell A5 = new PdfPCell(new Phrase("", TableFont));
                    A5.HorizontalAlignment = Element.ALIGN_LEFT;
                    A5.Border = iTextSharp.text.Rectangle.BOX;
                    ActrefTable.AddCell(A5);

                    PdfPCell A6 = new PdfPCell(new Phrase("", TableFont));
                    A6.HorizontalAlignment = Element.ALIGN_LEFT;
                    A6.Border = iTextSharp.text.Rectangle.BOX;
                    ActrefTable.AddCell(A6);
                }
            }

            document.Add(ActrefTable);

        }


        int Page_Height = 800, Page_Width = 580; int Rect_X = 100, Rect_Y = 640; string Change = string.Empty;
        int R_Length = 140, R_Breadth = 450; int R_XV = 0; float R_YV = 0f; int diff = 0;

        private void MATASMTPdf(Document document)//object sender, FormClosedEventArgs e)
        {

            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Application.MapPath("~\\Resources\\images\\Logo.JPG"));
            logo.BackgroundColor = BaseColor.WHITE;
            logo.ScalePercent(50f);
            document.Add(logo);

            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, false);

            iTextSharp.text.Font fc = new iTextSharp.text.Font(bfTimes, 12, 0);
            iTextSharp.text.Font fc1 = new iTextSharp.text.Font(bfTimes, 12, 0, BaseColor.RED);
            iTextSharp.text.Font fcGreen = new iTextSharp.text.Font(bfTimes, 12, 0, new BaseColor(0, 153, 0));
            iTextSharp.text.Font fcBold = new iTextSharp.text.Font(bfTimes, 12, 1);
            float X_Pos, Y_Pos;

            //PdfContentByte cb;
            //cb = writer.DirectContent;

            ////Casesum Entity list
            //List<CaseSumEntity> casesumdetails = new List<CaseSumEntity>();
            //casesumdetails = _model.CaseSumData.GetCaseSumDetails(AGY, Dept, Prog, Year, App, string.Empty);
            X_Pos = 60; Y_Pos = 680;
            bool FirstTime = false;
            var Matrix = MATAList.Select(u => u.MatCode).Distinct().ToList();

            if (Matrix.Count > 0)
            {
                foreach (var Matcd in Matrix)
                {
                    //MAtdef Entity list
                    string FirstAssDate = string.Empty, LastAssDate = string.Empty;
                    List<MATDEFEntity> matlist = new List<MATDEFEntity>();
                    MATDEFEntity MatEntity = new MATDEFEntity(true);
                    MatEntity.Mat_Code = Matcd;
                    matlist = _model.MatrixScalesData.Browse_MATDEF(MatEntity, "Browse");

                    // Services list
                    //List<CASEMSEntity> CaseMSlist = new List<CASEMSEntity>();
                    //CASEMSEntity MS_Search = new CASEMSEntity(true);
                    //MS_Search.Agency = BaseForm.BaseAgency; MS_Search.Dept = BaseForm.BaseDept;
                    //MS_Search.Program = BaseForm.BaseProg; MS_Search.Year = BaseForm.BaseYear;
                    //MS_Search.App_no = BaseForm.BaseApplicationNo;
                    //CaseMSlist = _model.SPAdminData.Browse_CASEMS(MS_Search, "Browse");

                    ////Outcomes List
                    //List<MSMASTEntity> MSList = new List<MSMASTEntity>();

                    bool FIRST_Ser = true;
                    string SP_Desc = string.Empty; string Out_Desc = string.Empty, CaseMS_Date = string.Empty, Goals = string.Empty;


                    //Benchmarks displayed
                    List<MATDEFBMEntity> MatBMlist = new List<MATDEFBMEntity>();
                    MATDEFBMEntity Search_Entity = new MATDEFBMEntity(true);
                    Search_Entity.MatCode = Matcd;
                    MatBMlist = _model.MatrixScalesData.Browse_MATDEFBM(Search_Entity, "Browse");
                    MatBMlist.Reverse();
                    string[] Bench = new string[6];


                    List<MATASMTEntity> Matasmtlist = new List<MATASMTEntity>();
                    Matasmtlist = MATAList.FindAll(u => u.ByPass.Equals("N") && u.MatCode.Equals(Matcd));
                    //MATASMTEntity Entity_matasmt = new MATASMTEntity(true);
                    //Entity_matasmt.Agency = AGY; Entity_matasmt.Dept = Dept;
                    //Entity_matasmt.Program = Prog; Entity_matasmt.Year = Year;
                    //Entity_matasmt.App = BaseForm.BaseApplicationNo; Entity_matasmt.Type = "S"; Entity_matasmt.ByPass = "N";
                    //Entity_matasmt.MatCode = Matcd;

                    MATDEFBMEntity Search1_Entity = new MATDEFBMEntity(true);
                    Search1_Entity.MatCode = Matcd;
                    MATADEFBMentity = _model.MatrixScalesData.Browse_MATDEFBM(Search_Entity, "Browse");


                    //Temp table not displayed on the screen
                    PdfPTable head = new PdfPTable(1);
                    head.HorizontalAlignment = Element.ALIGN_CENTER;
                    head.TotalWidth = 50f;
                    PdfPCell headcell = new PdfPCell(new Phrase(""));
                    headcell.HorizontalAlignment = Element.ALIGN_CENTER;
                    headcell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    head.AddCell(headcell);

                    if (!FirstTime)
                    {
                        cb.BeginText();
                        cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ROMAN).BaseFont, 12);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, AGY + "-" + Dept + "-" + Prog + " " + Year + " " + App + " " + LookupDataAccess.GetMemberName(SelSnp.NameixFi, SelSnp.NameixMi, SelSnp.NameixLast, BaseForm.BaseHierarchyCnFormat.ToString()), 550, 780, 0);

                        cb.EndText();
                    }
                    else { document.NewPage(); }
                    document.Add(head);
                    //if (casesumdetails.Count > 0)
                    //{
                    //    //table For the Referral programmes
                    //    PdfPTable Table_Programs = new PdfPTable(2);
                    //    Table_Programs.HorizontalAlignment = 1;
                    //    Table_Programs.TotalWidth = 300f;
                    //    Table_Programs.WidthPercentage = 100f;
                    //    Table_Programs.LockedWidth = true;
                    //    Table_Programs.SpacingBefore = 10f;

                    //    PdfPCell Benefits = new PdfPCell(new Phrase("Benefits Portal Results", fcBold));
                    //    Benefits.HorizontalAlignment = Element.ALIGN_CENTER;
                    //    Benefits.Colspan = 2;
                    //    Benefits.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    //    Table_Programs.AddCell(Benefits);

                    //    PdfPCell Programs_Header = new PdfPCell(new Phrase("Programs Referred to", fcBold));
                    //    Programs_Header.HorizontalAlignment = Element.ALIGN_CENTER;
                    //    Programs_Header.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    //    Table_Programs.AddCell(Programs_Header);

                    //    PdfPCell Programs_Header1 = new PdfPCell(new Phrase("Date of Referral", fcBold));
                    //    Programs_Header1.HorizontalAlignment = Element.ALIGN_CENTER;
                    //    Programs_Header1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    //    Table_Programs.AddCell(Programs_Header1);

                    //    foreach (CaseSumEntity Entity in casesumdetails)
                    //    {
                    //        if (Entity.CaseSumNotInterested == "False")
                    //        {
                    //            DataRow drHie;//=new DataRow();
                    //            DataSet dsHie = DatabaseLayer.AgyTab.GetHierarchyNames(Entity.CaseSumRefHierachy.Substring(0, 2), Entity.CaseSumRefHierachy.Substring(2, 2), Entity.CaseSumRefHierachy.Substring(4, 2));

                    //            if (dsHie.Tables.Count > 0)
                    //            {
                    //                if (dsHie.Tables[0].Rows.Count > 0)
                    //                {
                    //                    drHie = dsHie.Tables[0].Rows[0];
                    //                    PdfPCell Hier = new PdfPCell(new Phrase(drHie["HIE_NAME"].ToString(), fc));
                    //                    Hier.HorizontalAlignment = Element.ALIGN_LEFT;
                    //                    Hier.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    //                    Table_Programs.AddCell(Hier);


                    //                    PdfPCell ReferDate = new PdfPCell(new Phrase(LookupDataAccess.Getdate(Entity.CaseSumReferDate), fc));
                    //                    ReferDate.HorizontalAlignment = Element.ALIGN_CENTER;
                    //                    ReferDate.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    //                    Table_Programs.AddCell(ReferDate);
                    //                }
                    //            }
                    //        }
                    //    }
                    //    document.Add(Table_Programs);
                    //}

                    //Scales assessment displaying first and last dates
                    if (matlist.Count > 0)
                    {
                        PdfPTable Table_Scales = new PdfPTable(4);
                        Table_Scales.HorizontalAlignment = 1;
                        Table_Scales.TotalWidth = 550f;
                        float[] Scale_widths = new float[] { 130f, 60f, 60f, 70f };
                        Table_Scales.SetWidths(Scale_widths);
                        Table_Scales.WidthPercentage = 100f;
                        Table_Scales.LockedWidth = true;
                        Table_Scales.SpacingBefore = 20f;

                        string Matdesc = matlist.Find(u => u.Scale_Code.Equals("0")).Desc.ToString();

                        PdfPCell Mat1 = new PdfPCell(new Phrase("Matrix: " + Matdesc.ToString(), fcBold));
                        Mat1.HorizontalAlignment = Element.ALIGN_CENTER;
                        Mat1.Colspan = 4;
                        Mat1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        Table_Scales.AddCell(Mat1);

                        PdfPCell Life = new PdfPCell(new Phrase("Life Skills Assessments", fcBold));
                        Life.HorizontalAlignment = Element.ALIGN_CENTER;
                        Life.Colspan = 4;
                        Life.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        Table_Scales.AddCell(Life);

                        PdfPCell Table_Header = new PdfPCell(new Phrase("Scale", fcBold));
                        Table_Header.HorizontalAlignment = Element.ALIGN_LEFT;
                        Table_Header.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        Table_Scales.AddCell(Table_Header);

                        PdfPCell Table_Header1 = new PdfPCell(new Phrase("First Assessment", fcBold));
                        Table_Header1.HorizontalAlignment = Element.ALIGN_LEFT;
                        Table_Header1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        Table_Scales.AddCell(Table_Header1);

                        PdfPCell Table_Header2 = new PdfPCell(new Phrase("Last Assessment", fcBold));
                        Table_Header2.HorizontalAlignment = Element.ALIGN_LEFT;
                        Table_Header2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        Table_Scales.AddCell(Table_Header2);

                        PdfPCell Table_Header3 = new PdfPCell(new Phrase("Result", fcBold));
                        Table_Header3.HorizontalAlignment = Element.ALIGN_LEFT;
                        Table_Header3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        Table_Scales.AddCell(Table_Header3);
                        foreach (MATDEFEntity Entity in matlist)
                        {
                            if (Entity.Scale_Code != "0")
                            {
                                //Entity_matasmt.SclCode = Entity.Scale_Code.Trim();
                                string Scl_Name = Entity.Desc.Trim();
                                string CalcBm = Entity.Benchmark;
                                Matasmtlist = MATAList.FindAll(u => u.MatCode.Equals(Matcd) && u.SclCode.Equals(Entity.Scale_Code) && u.ByPass.Equals("N"));
                                if (Matasmtlist.Count > 0)
                                {
                                    string FPoints = string.Empty; string Lpoints = string.Empty; diff = 0; string FBench_Code = string.Empty; string LBench_Code = string.Empty;
                                    string Fbench = "N/A", Lbench = "N/A"; Change = "N/A";

                                    FirstAssDate = LookupDataAccess.Getdate(Matasmtlist[0].SSDate);
                                    if (!string.IsNullOrEmpty(FirstAssDate))
                                    {
                                        FPoints = Matasmtlist[0].Points;
                                        MATDEFBMEntity MatadefrowEntity = MATADEFBMentity.Find(u => Convert.ToInt32(u.Low) <= Convert.ToInt32(Matasmtlist[0].Points) && Convert.ToInt32(Matasmtlist[0].Points) <= Convert.ToInt32(u.High));
                                        if (MatadefrowEntity != null)
                                        {
                                            if (Matasmtlist[0].RespExcel.Trim() == "Y")
                                                Fbench = "No score";
                                            else
                                                Fbench = MatadefrowEntity.Desc.Trim();
                                            FBench_Code = MatadefrowEntity.Code.Trim();
                                        }
                                        else
                                            Fbench = "N/A";
                                    }

                                    LastAssDate = LookupDataAccess.Getdate(Matasmtlist[Matasmtlist.Count - 1].SSDate);
                                    if (FirstAssDate == LastAssDate)
                                        LastAssDate = string.Empty;
                                    if (!string.IsNullOrEmpty(LastAssDate))
                                    {
                                        Lpoints = Matasmtlist[Matasmtlist.Count - 1].Points;
                                        MATDEFBMEntity MatadefLastRowEntity = MATADEFBMentity.Find(u => Convert.ToInt32(u.Low) <= Convert.ToInt32(Matasmtlist[Matasmtlist.Count - 1].Points) && Convert.ToInt32(Matasmtlist[Matasmtlist.Count - 1].Points) <= Convert.ToInt32(u.High));
                                        if (MatadefLastRowEntity != null)
                                        {
                                            if (Matasmtlist[0].RespExcel.Trim() == "Y")
                                                Lbench = "No score";
                                            else
                                                Lbench = MatadefLastRowEntity.Desc.Trim();
                                            LBench_Code = MatadefLastRowEntity.Code.Trim();
                                        }
                                        else
                                            Lbench = "N/A";

                                        if (!string.IsNullOrEmpty(FPoints) && !string.IsNullOrEmpty(Lpoints))
                                        {
                                            diff = int.Parse(Lpoints) - int.Parse(FPoints);
                                        }
                                        else if (string.IsNullOrEmpty(FPoints) && !string.IsNullOrEmpty(Lpoints))
                                            diff = int.Parse(Lpoints);
                                        else if (!string.IsNullOrEmpty(FPoints) && string.IsNullOrEmpty(Lpoints))
                                            diff = int.Parse(FPoints);
                                        if (diff > 0)
                                            Change = "Improvement  " + " +" + diff;
                                        else if (diff < 0)
                                            Change = "Regressed      " + diff;
                                        else if (diff == 0)
                                            Change = "No Change      ";

                                        if (string.IsNullOrEmpty(FPoints) || string.IsNullOrEmpty(Lpoints))
                                            Change = "N/A";
                                    }
                                    PdfPCell Scale = new PdfPCell(new Phrase(Scl_Name, fc));
                                    Scale.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Scale.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    Table_Scales.AddCell(Scale);

                                    if (Fbench == "N/A" || Fbench == "No score")
                                    {
                                        PdfPCell FirstAss = new PdfPCell(new Phrase(Fbench, fc));
                                        FirstAss.HorizontalAlignment = Element.ALIGN_LEFT;
                                        FirstAss.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        Table_Scales.AddCell(FirstAss);
                                    }
                                    else if (!string.IsNullOrEmpty(FPoints))
                                    {
                                        PdfPCell FirstAss = new PdfPCell(new Phrase(Fbench + "(" + FPoints + ")", fc));
                                        FirstAss.HorizontalAlignment = Element.ALIGN_LEFT;
                                        FirstAss.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        Table_Scales.AddCell(FirstAss);
                                    }
                                    else
                                    {
                                        PdfPCell FirstAss = new PdfPCell(new Phrase(Fbench, fc));
                                        FirstAss.HorizontalAlignment = Element.ALIGN_LEFT;
                                        FirstAss.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        Table_Scales.AddCell(FirstAss);
                                    }

                                    if (Lbench == "N/A" || Lbench == "No score")
                                    {
                                        PdfPCell FirstAss = new PdfPCell(new Phrase(Lbench, fc));
                                        FirstAss.HorizontalAlignment = Element.ALIGN_LEFT;
                                        FirstAss.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        Table_Scales.AddCell(FirstAss);
                                    }
                                    else if (!string.IsNullOrEmpty(Lpoints))
                                    {
                                        PdfPCell Lastass = new PdfPCell(new Phrase(Lbench + "(" + Lpoints + ")", fc));
                                        Lastass.HorizontalAlignment = Element.ALIGN_LEFT;
                                        Lastass.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        Table_Scales.AddCell(Lastass);
                                    }
                                    else
                                    {
                                        PdfPCell Lastass = new PdfPCell(new Phrase(Lbench, fc));
                                        Lastass.HorizontalAlignment = Element.ALIGN_LEFT;
                                        Lastass.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        Table_Scales.AddCell(Lastass);
                                    }

                                    if (Change == "Improvement  " + " +" + diff)
                                    {
                                        PdfPCell Result = new PdfPCell(new Phrase(Change, fcGreen));
                                        Result.HorizontalAlignment = Element.ALIGN_LEFT;
                                        Result.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        Table_Scales.AddCell(Result);
                                    }
                                    else if (Change == "Regressed      " + diff)
                                    {
                                        PdfPCell Result = new PdfPCell(new Phrase(Change, fc1));
                                        Result.HorizontalAlignment = Element.ALIGN_LEFT;
                                        Result.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        Table_Scales.AddCell(Result);
                                    }
                                    else
                                    {
                                        PdfPCell Result = new PdfPCell(new Phrase(Change, fc));
                                        Result.HorizontalAlignment = Element.ALIGN_LEFT;
                                        Result.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        Table_Scales.AddCell(Result);
                                    }
                                }
                            }
                        }
                        document.Add(Table_Scales);
                    }

                    // Displaying Services for that program
                    //if (CaseMSlist.Count > 0)
                    //{
                    //    DataSet dsResults = DatabaseLayer.Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.RESULTS);
                    //    DataTable dtResults = dsResults.Tables[0];

                    //    PdfPTable Table_Services = new PdfPTable(3);
                    //    Table_Services.HorizontalAlignment = PdfContentByte.ALIGN_CENTER;
                    //    Table_Services.TotalWidth = 550f;
                    //    float[] widths = new float[] { 30f, 160f, 50f };
                    //    Table_Services.SetWidths(widths);
                    //    Table_Services.WidthPercentage = 100f;
                    //    Table_Services.LockedWidth = true;
                    //    Table_Services.SpacingBefore = 20f;

                    //    foreach (CASEMSEntity Entity in CaseMSlist)
                    //    {
                    //        Out_Desc = string.Empty; Goals = string.Empty;
                    //        CaseMS_Date = LookupDataAccess.Getdate(Entity.Date);
                    //        MSList = _model.SPAdminData.Browse_MSMAST(null, Entity.MS_Code, null, null, null);
                    //        if (MSList.Count > 0)
                    //        {
                    //            if (MSList[0].Type1 == "O")
                    //            {
                    //                Out_Desc = MSList[0].Desc.Trim();
                    //                foreach (DataRow dr in dtResults.Rows)
                    //                {
                    //                    if (Entity.Result.Trim() == dr["Code"].ToString().Trim())
                    //                    {
                    //                        Goals = dr["LookUpDesc"].ToString().Trim(); break;
                    //                    }
                    //                }
                    //            }
                    //        }

                    //        if (!string.IsNullOrEmpty(Out_Desc))
                    //        {
                    //            if (FIRST_Ser)
                    //            {
                    //                PdfPCell Header_ser = new PdfPCell(new Phrase("Services  Delivered since first visit to the agency", fcBold));
                    //                Header_ser.HorizontalAlignment = Element.ALIGN_CENTER;
                    //                Header_ser.Colspan = 4;
                    //                Header_ser.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    //                Table_Services.AddCell(Header_ser);

                    //                PdfPCell Table_Date = new PdfPCell(new Phrase("Date", fcBold));
                    //                Table_Date.HorizontalAlignment = Element.ALIGN_LEFT;
                    //                Table_Date.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    //                Table_Services.AddCell(Table_Date);

                    //                PdfPCell Table_Outcome = new PdfPCell(new Phrase("Outcome", fcBold));
                    //                Table_Outcome.HorizontalAlignment = Element.ALIGN_LEFT;
                    //                Table_Outcome.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    //                Table_Services.AddCell(Table_Outcome);

                    //                PdfPCell Table_Result = new PdfPCell(new Phrase("Result", fcBold));
                    //                Table_Result.HorizontalAlignment = Element.ALIGN_LEFT;
                    //                Table_Result.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    //                Table_Services.AddCell(Table_Result);

                    //                FIRST_Ser = false;
                    //            }
                    //            PdfPCell Scale = new PdfPCell(new Phrase(CaseMS_Date, fc));
                    //            Scale.HorizontalAlignment = Element.ALIGN_LEFT;
                    //            Scale.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    //            Table_Services.AddCell(Scale);

                    //            PdfPCell Data_2 = new PdfPCell(new Phrase(Out_Desc, fc));
                    //            Data_2.HorizontalAlignment = Element.ALIGN_LEFT;
                    //            Data_2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    //            Table_Services.AddCell(Data_2);

                    //            PdfPCell Data_3 = new PdfPCell(new Phrase(Goals, fc));
                    //            Data_3.HorizontalAlignment = Element.ALIGN_LEFT;
                    //            Data_3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    //            Table_Services.AddCell(Data_3);
                    //        }

                    //    }
                    //    document.Add(Table_Services);
                    //}

                    //draw the Graphs for each Scale
                    //if (matlist.Count > 0)
                    //{
                    //    bool First = true;
                    //    string FPoints = string.Empty; //string Lpoints = string.Empty;

                    //    foreach (MATDEFEntity Entity in matlist)
                    //    {
                    //        if (Entity.Scale_Code != "0")
                    //        {
                    //            string SclCode = Entity.Scale_Code.Trim();
                    //            string DESC = Entity.Desc.Trim();
                    //            //Matasmtlist = _model.MatrixScalesData.Browse_MATASMT(Entity_matasmt, "Browse");
                    //            Matasmtlist = MATAList.FindAll(u => u.MatCode.Equals(Matcd) && u.SclCode.Equals(Entity.Scale_Code) && u.ByPass.Equals("N"));
                    //            int Scl_Count = Matasmtlist.Count; string[] Dates = new string[Scl_Count];
                    //            if (Matasmtlist.Count > 0)
                    //            {
                    //                int j = 0; string[] Points = new string[Scl_Count]; string[] BenchMarks = new string[Scl_Count]; string[] Resp_Excel_Sw = new string[Scl_Count];
                    //                for (int c = 0; c < Scl_Count; c++)
                    //                    Dates[c] = Points[c] = BenchMarks[c] = Resp_Excel_Sw[c] = " ";
                    //                foreach (MATASMTEntity SMt in Matasmtlist)
                    //                {
                    //                    if (j < 11)
                    //                    {
                    //                        Dates[j] = LookupDataAccess.GetdatebyYear(SMt.SSDate);
                    //                        Points[j] = SMt.Points;
                    //                        Resp_Excel_Sw[j] = SMt.RespExcel.Trim();

                    //                        MATDEFBMEntity MatadefrowEntity = MATADEFBMentity.Find(u => Convert.ToInt32(u.Low) <= Convert.ToInt32(SMt.Points) && Convert.ToInt32(SMt.Points) <= Convert.ToInt32(u.High));
                    //                        if (MatadefrowEntity != null)
                    //                            BenchMarks[j] = MatadefrowEntity.Desc.Trim();
                    //                        j++;
                    //                    }
                    //                }

                    //                bool IsThere = false;
                    //                for (int i = 0; i < Points.Length; i++)
                    //                {
                    //                    if (Points[i] != " ")
                    //                    {
                    //                        string Fbench = string.Empty;
                    //                        MATDEFBMEntity MatadefrowEntity = MATADEFBMentity.Find(u => Convert.ToInt32(u.Low) <= Convert.ToInt32(Points[i]) && Convert.ToInt32(Points[i]) <= Convert.ToInt32(u.High));
                    //                        if (MatadefrowEntity != null)
                    //                        {
                    //                            IsThere = true; break;
                    //                        }
                    //                    }
                    //                }
                    //                if (IsThere)
                    //                {
                    //                    if (First)
                    //                    {
                    //                        document.NewPage();
                    //                        X_Pos = 60; Y_Pos = 760; R_XV = Rect_X; R_YV = Rect_Y;
                    //                        cb.BeginText();
                    //                        iTextSharp.text.Font Bold_Under = new iTextSharp.text.Font(bfTimes, 12, 5);
                    //                        Paragraph para = new Paragraph(PdfContentByte.ALIGN_CENTER, "Assessments  Completed", Bold_Under);
                    //                        para.Alignment = Element.ALIGN_CENTER;
                    //                        document.Add(para);
                    //                        cb.EndText();
                    //                        First = false;
                    //                    }
                    //                    int len = DESC.Length;
                    //                    X_Pos = R_XV + 20; Y_Pos = R_YV + 145;
                    //                    cb.BeginText();
                    //                    cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ROMAN).BaseFont, 12);
                    //                    cb.SetRGBColorFill(0, 10, 10);
                    //                    cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, DESC, 300, Y_Pos, 0);

                    //                    if (MatBMlist.Count > 0)
                    //                    {
                    //                        X_Pos = R_XV - 10; Y_Pos = R_YV + 20; int i = 0;
                    //                        foreach (MATDEFBMEntity BM_Entity in MatBMlist)
                    //                        {
                    //                            if (i < 6)
                    //                            {
                    //                                Bench[i] = BM_Entity.Desc.Trim();
                    //                                cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT, Bench[i], X_Pos, Y_Pos, 0);
                    //                                Y_Pos += 20; i++;
                    //                            }
                    //                        }
                    //                    }

                    //                    X_Pos = R_XV + 10; Y_Pos = R_YV - 10;
                    //                    for (int i = 0; i < Dates.Length; ++i)
                    //                    {
                    //                        cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ROMAN).BaseFont, 8);
                    //                        cb.ShowTextAligned(100, Dates[i], X_Pos, Y_Pos, 0);
                    //                        X_Pos += 40;
                    //                    }

                    //                    cb.RoundRectangle(R_XV, R_YV, R_Breadth, R_Length, 5f);
                    //                    cb.Stroke();

                    //                    //PdfShading axial = PdfShading.SimpleAxial(writer, 36, 716, 396,788, BaseColor.ORANGE, BaseColor.BLUE);
                    //                    ////cb.PaintShading(axial);
                    //                    //PdfShadingPattern shading = new PdfShadingPattern(axial);
                    //                    //cb.SetShadingFill(shading);

                    //                    if (Entity.Scale_Code.Trim() == SclCode.Trim())
                    //                    {
                    //                        int Y_Axis = 20; int X_Axis = 20; string date = null;
                    //                        for (int i = 0; i < Points.Length; i++)
                    //                        {
                    //                            if (Points[i] != " ")
                    //                            {
                    //                                string Fbench = string.Empty;
                    //                                MATDEFBMEntity MatadefrowEntity = MATADEFBMentity.Find(u => Convert.ToInt32(u.Low) <= Convert.ToInt32(Points[i]) && Convert.ToInt32(Points[i]) <= Convert.ToInt32(u.High));
                    //                                if (MatadefrowEntity != null)
                    //                                    Fbench = MatadefrowEntity.Desc.Trim();
                    //                                FPoints = Points[i];
                    //                                date = Dates[i];
                    //                                if (!string.IsNullOrEmpty(Fbench))
                    //                                {
                    //                                    int breadth = 0;
                    //                                    switch (i)
                    //                                    {
                    //                                        case 0: if (date == Dates[0]) breadth = R_XV + 13; break;
                    //                                        case 1: if (date == Dates[1]) breadth = R_XV + 53; break;
                    //                                        case 2: if (date == Dates[2]) breadth = R_XV + 93; break;
                    //                                        case 3: if (date == Dates[3]) breadth = R_XV + 133; break;
                    //                                        case 4: if (date == Dates[4]) breadth = R_XV + 173; break;
                    //                                        case 5: if (date == Dates[5]) breadth = R_XV + 213; break;
                    //                                        case 6: if (date == Dates[6]) breadth = R_XV + 253; break;
                    //                                        case 7: if (date == Dates[7]) breadth = R_XV + 293; break;
                    //                                        case 8: if (date == Dates[8]) breadth = R_XV + 333; break;
                    //                                        case 9: if (date == Dates[9]) breadth = R_XV + 373; break;
                    //                                        case 10: if (date == Dates[10]) breadth = R_XV + 413; break;
                    //                                    }

                    //                                    if ((Bench[0] == Fbench))
                    //                                    {
                    //                                        cb.SetColorFill(BaseColor.DARK_GRAY.Brighter());
                    //                                        cb.Rectangle(breadth + 2, R_YV + 0.5f, X_Axis - 1, Y_Axis + 1);
                    //                                        cb.Fill();
                    //                                        cb.SetColorFill(new BaseColor(0, 128, 128));
                    //                                        cb.Rectangle(breadth, R_YV + 0.5f, X_Axis, Y_Axis);        //InCrisis
                    //                                        cb.Fill();
                    //                                        //cb.ShowTextAligned(iTextSharp.text.Rectangle.ALIGN_CENTER, FPoints, breadth + 10, R_YV + Y_Axis + 4, 0);
                    //                                        cb.SetColorFill(BaseColor.WHITE);
                    //                                        if (Resp_Excel_Sw[i] == "Y")
                    //                                            cb.ShowTextAligned(iTextSharp.text.Rectangle.ALIGN_CENTER, "N/A", breadth + 10, R_YV + 11, 0);
                    //                                        else
                    //                                            cb.ShowTextAligned(iTextSharp.text.Rectangle.ALIGN_CENTER, FPoints, breadth + 10, R_YV + 11, 0);
                    //                                    }
                    //                                    if ((Bench[1] == Fbench))
                    //                                    {
                    //                                        cb.SetColorFill(BaseColor.LIGHT_GRAY.Darker());
                    //                                        cb.Rectangle(breadth + 2, R_YV + 0.5f, X_Axis - 1, Y_Axis + 21);
                    //                                        cb.Fill();
                    //                                        cb.SetColorFill(BaseColor.MAGENTA.Darker());
                    //                                        cb.Rectangle(breadth, R_YV + 0.5f, X_Axis, Y_Axis + 20);        //Vulnarble
                    //                                        cb.Fill(); cb.Stroke();
                    //                                        //cb.ShowTextAligned(iTextSharp.text.Rectangle.ALIGN_CENTER, FPoints, breadth + 10, R_YV + Y_Axis + 24, 0);
                    //                                        cb.SetColorFill(BaseColor.WHITE);
                    //                                        if (Resp_Excel_Sw[i] == "Y")
                    //                                            cb.ShowTextAligned(iTextSharp.text.Rectangle.ALIGN_CENTER, "N/A", breadth + 10, R_YV + 11, 0);
                    //                                        else
                    //                                            cb.ShowTextAligned(iTextSharp.text.Rectangle.ALIGN_CENTER, FPoints, breadth + 10, R_YV + Y_Axis + 11, 0);
                    //                                    }
                    //                                    else if ((Bench[2] == Fbench))
                    //                                    {
                    //                                        cb.SetColorFill(BaseColor.LIGHT_GRAY.Darker());
                    //                                        cb.Rectangle(breadth + 2, R_YV + 0.5f, X_Axis - 1, Y_Axis + 41);
                    //                                        cb.Fill();

                    //                                        cb.SetColorFill(new BaseColor(153, 51, 255));
                    //                                        cb.Rectangle(breadth, R_YV + 0.5f, X_Axis, Y_Axis + 40);        //Safe
                    //                                        cb.Fill(); cb.Stroke();
                    //                                        //cb.ShowTextAligned(iTextSharp.text.Rectangle.ALIGN_CENTER, FPoints, breadth + 10, R_YV + Y_Axis + 44, 0);
                    //                                        cb.SetColorFill(BaseColor.WHITE);
                    //                                        if (Resp_Excel_Sw[i] == "Y")
                    //                                            cb.ShowTextAligned(iTextSharp.text.Rectangle.ALIGN_CENTER, "N/A", breadth + 10, R_YV + 11, 0);
                    //                                        else
                    //                                            cb.ShowTextAligned(iTextSharp.text.Rectangle.ALIGN_CENTER, FPoints, breadth + 10, R_YV + Y_Axis + 31, 0);
                    //                                    }
                    //                                    else if ((Bench[3] == Fbench))
                    //                                    {
                    //                                        cb.SetColorFill(BaseColor.LIGHT_GRAY.Darker());
                    //                                        cb.Rectangle(breadth + 2, R_YV + 0.5f, X_Axis - 1, Y_Axis + 61);
                    //                                        cb.Fill();

                    //                                        cb.SetColorFill(new BaseColor(255, 140, 0));
                    //                                        cb.Rectangle(breadth, R_YV + 0.5f, X_Axis, Y_Axis + 60);        //Stable
                    //                                        cb.Fill(); cb.Stroke();
                    //                                        //cb.ShowTextAligned(iTextSharp.text.Rectangle.ALIGN_CENTER, FPoints, breadth + 10, R_YV + Y_Axis + 64, 0);
                    //                                        cb.SetColorFill(BaseColor.WHITE);
                    //                                        if (Resp_Excel_Sw[i] == "Y")
                    //                                            cb.ShowTextAligned(iTextSharp.text.Rectangle.ALIGN_CENTER, "N/A", breadth + 10, R_YV + 11, 0);
                    //                                        else
                    //                                            cb.ShowTextAligned(iTextSharp.text.Rectangle.ALIGN_CENTER, FPoints, breadth + 10, R_YV + Y_Axis + 51, 0);
                    //                                    }
                    //                                    else if ((Bench[4] == Fbench))
                    //                                    {
                    //                                        cb.SetColorFill(BaseColor.LIGHT_GRAY.Darker());
                    //                                        cb.Rectangle(breadth + 2, R_YV + 0.5f, X_Axis - 1, Y_Axis + 81);
                    //                                        cb.Fill();

                    //                                        cb.SetColorFill(new BaseColor(70, 130, 180));
                    //                                        cb.Rectangle(breadth, R_YV + 0.5f, X_Axis, Y_Axis + 80);        //Thriving
                    //                                        cb.Fill(); cb.Stroke();
                    //                                        //cb.ShowTextAligned(iTextSharp.text.Rectangle.ALIGN_CENTER, FPoints, breadth + 10, R_YV + Y_Axis + 84, 0);
                    //                                        cb.SetColorFill(BaseColor.WHITE);
                    //                                        if (Resp_Excel_Sw[i] == "Y")
                    //                                            cb.ShowTextAligned(iTextSharp.text.Rectangle.ALIGN_CENTER, "N/A", breadth + 10, R_YV + 11, 0);
                    //                                        else
                    //                                            cb.ShowTextAligned(iTextSharp.text.Rectangle.ALIGN_CENTER, FPoints, breadth + 10, R_YV + Y_Axis + 71, 0);
                    //                                    }
                    //                                    else if ((Bench[5] == Fbench))
                    //                                    {
                    //                                        cb.SetColorFill(BaseColor.LIGHT_GRAY.Darker());
                    //                                        cb.Rectangle(breadth + 2, R_YV + 0.5f, X_Axis - 1, Y_Axis + 101);
                    //                                        cb.Fill();

                    //                                        cb.SetColorFill(BaseColor.YELLOW.Darker());
                    //                                        cb.Rectangle(breadth, R_YV + 0.5f, X_Axis, Y_Axis + 100);        //InCrisis
                    //                                        cb.Fill(); cb.Stroke();
                    //                                        //cb.ShowTextAligned(iTextSharp.text.Rectangle.ALIGN_CENTER, FPoints, breadth + 10, R_YV + Y_Axis + 104, 0);
                    //                                        cb.SetColorFill(BaseColor.WHITE);
                    //                                        if (Resp_Excel_Sw[i] == "Y")
                    //                                            cb.ShowTextAligned(iTextSharp.text.Rectangle.ALIGN_CENTER, "N/A", breadth + 10, R_YV + 11, 0);
                    //                                        else
                    //                                            cb.ShowTextAligned(iTextSharp.text.Rectangle.ALIGN_CENTER, FPoints, breadth + 10, R_YV + Y_Axis + 91, 0);
                    //                                    }
                    //                                }
                    //                            }
                    //                        }
                    //                    }
                    //                    cb.EndText();
                    //                    float Rem_Width = Page_Width - (R_XV + R_Breadth);
                    //                    float Rem_Height = R_YV;
                    //                    if (Rem_Width > (R_Breadth + 30))
                    //                    {
                    //                        R_XV = R_XV + R_Breadth + 30;
                    //                    }
                    //                    else
                    //                    {
                    //                        Rem_Height = Page_Height - (Page_Height - R_YV);
                    //                        if (Rem_Height > (R_Length + 50))
                    //                        {
                    //                            R_XV = Rect_X; R_YV = R_YV - (R_Length + 50);
                    //                        }
                    //                        else
                    //                        {
                    //                            document.NewPage();
                    //                            R_XV = Rect_X; R_YV = Rect_Y;
                    //                        }
                    //                    }
                    //                }
                    //            }
                    //        }

                    //    }
                    //}
                }
            }

            //document.Close();
            //fs.Close();
            //fs.Dispose();
            //FrmViewer objfrm = new FrmViewer(PdfName);
            ////objfrm.FormClosed += new Form.FormClosedEventHandler(On_Delete_PDF_File);
            //objfrm.ShowDialog();
        }

        private void LiheapList(Document document)
        {
            BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            iTextSharp.text.Font helvetica = new iTextSharp.text.Font(bf_times, 12, 1);
            BaseFont bf_helv = helvetica.GetCalculatedBaseFont(false);
            iTextSharp.text.Font TimesUnderline = new iTextSharp.text.Font(bf_times, 9, 4);
            BaseFont bf_TimesUnderline = TimesUnderline.GetCalculatedBaseFont(true);

            iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 9);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 8);
            iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 8, 3);
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_times, 9, 1);
            iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 8, 2);
            iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 9, 4);

            PdfPTable LPBTable1 = new PdfPTable(2);
            LPBTable1.TotalWidth = 550f;
            LPBTable1.WidthPercentage = 100;
            LPBTable1.LockedWidth = true;
            float[] Lwidths = new float[] { 50f, 50f };
            LPBTable1.SetWidths(Lwidths);
            LPBTable1.HorizontalAlignment = Element.ALIGN_LEFT;
            LPBTable1.SpacingAfter = 10f;

            PdfPTable LPBTable = new PdfPTable(4);
            LPBTable.TotalWidth = 350f;
            LPBTable.WidthPercentage = 100;
            LPBTable.LockedWidth = true;
            float[] widths2 = new float[] { 15f, 25f, 30f, 30f };
            LPBTable.SetWidths(widths2);
            LPBTable.HorizontalAlignment = Element.ALIGN_LEFT;

            string ProgramName = string.Empty;
            if (programEntity != null)
                ProgramName = programEntity.AgencyName;

            PdfPCell H1 = new PdfPCell(new Phrase("Hierarchy:" + CASEMSTList[0].ApplAgency + "-" + CASEMSTList[0].ApplDept + "-" + CASEMSTList[0].ApplProgram + " " + CASEMSTList[0].ApplYr + "  " + ProgramName, TblFontBold));
            H1.HorizontalAlignment = Element.ALIGN_LEFT;
            //H1.Colspan = 3;
            H1.FixedHeight = 15f;
            H1.Border = iTextSharp.text.Rectangle.NO_BORDER;
            LPBTable1.AddCell(H1);

            PdfPCell H2 = new PdfPCell(new Phrase("App:" + CASEMSTList[0].ApplNo + "  " + LookupDataAccess.GetMemberName(SelSnp.NameixFi, SelSnp.NameixMi, SelSnp.NameixLast, BaseForm.BaseHierarchyCnFormat.ToString()), TblFontBold));
            H2.HorizontalAlignment = Element.ALIGN_RIGHT;
            //H2.Colspan = 3;
            H2.FixedHeight = 15f;
            H2.Border = iTextSharp.text.Rectangle.NO_BORDER;
            LPBTable1.AddCell(H2);

            PdfPCell S2 = new PdfPCell(new Phrase("", TblFontBold));
            S2.HorizontalAlignment = Element.ALIGN_CENTER;
            S2.Colspan = 2;
            S2.FixedHeight = 15f;
            S2.Border = iTextSharp.text.Rectangle.NO_BORDER;
            LPBTable1.AddCell(S2);

            document.Add(LPBTable1);

            PdfPCell Header = new PdfPCell(new Phrase("Fuel Assistance Benefit", TblFontBold));
            Header.HorizontalAlignment = Element.ALIGN_CENTER;
            Header.Colspan = 4;
            Header.FixedHeight = 15f;
            Header.Border = iTextSharp.text.Rectangle.BOX;
            LPBTable.AddCell(Header);

            string[] HeaderSeq = { "Type", "Date", "Award Amount", "Balance Amount" };
            for (int i = 0; i < HeaderSeq.Length; ++i)
            {
                PdfPCell cell = new PdfPCell(new Phrase(HeaderSeq[i], TblFontBold));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                //cell.FixedHeight = 15f;
                cell.Border = iTextSharp.text.Rectangle.BOX;
                LPBTable.AddCell(cell);
            }

            string Benefit_Date = " ";
            foreach (LIHEAPBEntity Enty in LPBList)
            {
                Benefit_Date = " ";
                if (!string.IsNullOrEmpty(Enty.Award_Date.Trim()))
                    Benefit_Date = LookupDataAccess.Getdate(Enty.Award_Date);

                if (!string.IsNullOrEmpty(Enty.Award_Type.Trim()))
                {
                    if (Enty.Award_Type.Substring(0, 1) != "C" && Enty.Award_Type.Substring(0, 1) != "S")
                    {
                        Enty.Remaining = Enty.Award_Amount;
                        if (!string.IsNullOrEmpty(Enty.Reduce_Elig))
                            Enty.Remaining = (decimal.Parse(Enty.Award_Amount) - decimal.Parse(Enty.Reduce_Elig)).ToString();

                        PdfPCell A1 = new PdfPCell(new Phrase(Enty.Award_Type, TableFont));
                        A1.HorizontalAlignment = Element.ALIGN_LEFT;
                        A1.Border = iTextSharp.text.Rectangle.BOX;
                        LPBTable.AddCell(A1);

                        PdfPCell A2 = new PdfPCell(new Phrase(Benefit_Date, TableFont));
                        A2.HorizontalAlignment = Element.ALIGN_LEFT;
                        A2.Border = iTextSharp.text.Rectangle.BOX;
                        LPBTable.AddCell(A2);

                        PdfPCell A3 = new PdfPCell(new Phrase(Enty.Award_Amount, TableFont));
                        A3.HorizontalAlignment = Element.ALIGN_RIGHT;
                        A3.Border = iTextSharp.text.Rectangle.BOX;
                        LPBTable.AddCell(A3);

                        PdfPCell A4 = new PdfPCell(new Phrase(Enty.Remaining, TableFont));
                        A4.HorizontalAlignment = Element.ALIGN_RIGHT;
                        A4.Border = iTextSharp.text.Rectangle.BOX;
                        LPBTable.AddCell(A4);

                        //rowIndex = Benefits_Grid.Rows.Add(Enty.Award_Type, Benefit_Date, Enty.Award_Amount, Enty.Remaining);

                    }
                    else
                    {
                        if (Enty.Award_Amount == "0")
                        {
                            PdfPCell A1 = new PdfPCell(new Phrase(Enty.Award_Type, TableFont));
                            A1.HorizontalAlignment = Element.ALIGN_LEFT;
                            A1.Border = iTextSharp.text.Rectangle.BOX;
                            LPBTable.AddCell(A1);

                            PdfPCell A2 = new PdfPCell(new Phrase(Benefit_Date, TableFont));
                            A2.HorizontalAlignment = Element.ALIGN_LEFT;
                            A2.Border = iTextSharp.text.Rectangle.BOX;
                            LPBTable.AddCell(A2);

                            PdfPCell A3 = new PdfPCell(new Phrase(Enty.Award_Amount, TableFont));
                            A3.HorizontalAlignment = Element.ALIGN_RIGHT;
                            A3.Border = iTextSharp.text.Rectangle.BOX;
                            LPBTable.AddCell(A3);

                            PdfPCell A4 = new PdfPCell(new Phrase("0.00", TableFont));
                            A4.HorizontalAlignment = Element.ALIGN_RIGHT;
                            A4.Border = iTextSharp.text.Rectangle.BOX;
                            LPBTable.AddCell(A4);
                        }
                        else
                        {
                            PdfPCell A1 = new PdfPCell(new Phrase(Enty.Award_Type, TableFont));
                            A1.HorizontalAlignment = Element.ALIGN_LEFT;
                            A1.Border = iTextSharp.text.Rectangle.BOX;
                            LPBTable.AddCell(A1);

                            PdfPCell A2 = new PdfPCell(new Phrase(Benefit_Date, TableFont));
                            A2.HorizontalAlignment = Element.ALIGN_LEFT;
                            A2.Border = iTextSharp.text.Rectangle.BOX;
                            LPBTable.AddCell(A2);

                            PdfPCell A3 = new PdfPCell(new Phrase(Enty.Award_Amount, TableFont));
                            A3.HorizontalAlignment = Element.ALIGN_RIGHT;
                            A3.Border = iTextSharp.text.Rectangle.BOX;
                            LPBTable.AddCell(A3);

                            PdfPCell A4 = new PdfPCell(new Phrase(Enty.Remaining, TableFont));
                            A4.HorizontalAlignment = Element.ALIGN_RIGHT;
                            A4.Border = iTextSharp.text.Rectangle.BOX;
                            LPBTable.AddCell(A4);
                        }
                    }
                }
                else
                {
                    if (Enty.Award_Amount == "0")
                    {
                        PdfPCell A1 = new PdfPCell(new Phrase(Enty.Award_Type, TableFont));
                        A1.HorizontalAlignment = Element.ALIGN_LEFT;
                        A1.Border = iTextSharp.text.Rectangle.BOX;
                        LPBTable.AddCell(A1);

                        PdfPCell A2 = new PdfPCell(new Phrase(Benefit_Date, TableFont));
                        A2.HorizontalAlignment = Element.ALIGN_LEFT;
                        A2.Border = iTextSharp.text.Rectangle.BOX;
                        LPBTable.AddCell(A2);

                        PdfPCell A3 = new PdfPCell(new Phrase(Enty.Award_Amount, TableFont));
                        A3.HorizontalAlignment = Element.ALIGN_RIGHT;
                        A3.Border = iTextSharp.text.Rectangle.BOX;
                        LPBTable.AddCell(A3);

                        PdfPCell A4 = new PdfPCell(new Phrase("0.00", TableFont));
                        A4.HorizontalAlignment = Element.ALIGN_RIGHT;
                        A4.Border = iTextSharp.text.Rectangle.BOX;
                        LPBTable.AddCell(A4);
                    }
                    else
                    {
                        PdfPCell A1 = new PdfPCell(new Phrase(Enty.Award_Type, TableFont));
                        A1.HorizontalAlignment = Element.ALIGN_LEFT;
                        A1.Border = iTextSharp.text.Rectangle.BOX;
                        LPBTable.AddCell(A1);

                        PdfPCell A2 = new PdfPCell(new Phrase(Benefit_Date, TableFont));
                        A2.HorizontalAlignment = Element.ALIGN_LEFT;
                        A2.Border = iTextSharp.text.Rectangle.BOX;
                        LPBTable.AddCell(A2);

                        PdfPCell A3 = new PdfPCell(new Phrase(Enty.Award_Amount, TableFont));
                        A3.HorizontalAlignment = Element.ALIGN_RIGHT;
                        A3.Border = iTextSharp.text.Rectangle.BOX;
                        LPBTable.AddCell(A3);

                        PdfPCell A4 = new PdfPCell(new Phrase(Enty.Remaining, TableFont));
                        A4.HorizontalAlignment = Element.ALIGN_RIGHT;
                        A4.Border = iTextSharp.text.Rectangle.BOX;
                        LPBTable.AddCell(A4);
                    }


                }
            }

            document.Add(LPBTable);

            if (PaymentList.Count > 0)
            {
                Heating_List = _model.lookupDataAccess.Get_HearingSources();

                PdfPTable PAYTable = new PdfPTable(8);
                PAYTable.TotalWidth = 550f;
                PAYTable.WidthPercentage = 100;
                PAYTable.LockedWidth = true;
                float[] widths = new float[] { 23f, 18f, 8f, 20f, 18f, 25f, 35f, 25f };
                PAYTable.SetWidths(widths);
                PAYTable.HorizontalAlignment = Element.ALIGN_LEFT;
                PAYTable.SpacingBefore = 25f;

                PdfPCell Header1 = new PdfPCell(new Phrase("Fuel Assistance Authorization/Invoices", TblFontBold));
                Header1.HorizontalAlignment = Element.ALIGN_CENTER;
                Header1.Colspan = 8;
                Header1.FixedHeight = 15f;
                Header1.Border = iTextSharp.text.Rectangle.BOX;
                PAYTable.AddCell(Header1);

                string[] HeaderSeq1 = { "Authorization#", "Date", "BS", "Award Amount", "Pay For", "Worker", "Vendor", "Invoice Amount" };
                for (int i = 0; i < HeaderSeq1.Length; ++i)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(HeaderSeq1[i], TblFontBold));
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //cell.FixedHeight = 15f;
                    cell.Border = iTextSharp.text.Rectangle.BOX;
                    PAYTable.AddCell(cell);
                }


                bool Invoice_Exists = false;
                string Authr_Date = " ", Invc_SW = "N", SeftyNet_level = string.Empty, Bundle_SW = "N", Check_SW = "N";
                decimal Balance_Amount = 0, Priv_Award_Balance = 0, Invc_Dol1_Dol2 = 0;
                foreach (PAYMNETEntity Enty in PaymentList)
                {
                    Invoice_Exists = false; Authr_Date = " "; Invc_SW = Bundle_SW = Check_SW = "N";
                    if (!string.IsNullOrEmpty(Enty.Invc_Date.Trim()))
                    {
                        Invoice_Exists = true;

                        if (!string.IsNullOrEmpty(Enty.Amount_Dol1))
                            Invc_Dol1_Dol2 = decimal.Parse(Enty.Amount_Dol1.Trim());

                        if (!string.IsNullOrEmpty(Enty.Amount_Dol2))
                            Invc_Dol1_Dol2 += decimal.Parse(Enty.Amount_Dol2.Trim());

                        if (!string.IsNullOrEmpty(Enty.Amount_Dol3))
                            Invc_Dol1_Dol2 += decimal.Parse(Enty.Amount_Dol3.Trim());


                        //if (decimal.Parse(Enty.Amount_Dol.Trim()) > Invc_Dol1_Dol2)
                        //    Alloc_Short_Fall = true;

                        if (!string.IsNullOrEmpty(Enty.Batch_NO.Trim()))
                            Bundle_SW = "Y";

                        if (!string.IsNullOrEmpty(Enty.Check_No.Trim()) && !string.IsNullOrEmpty(Enty.Check_Date.Trim()))
                            Check_SW = "Y";

                        Invc_SW = "Y";

                    }


                    if (!string.IsNullOrEmpty(Enty.Authr_Date.Trim()))
                        Authr_Date = LookupDataAccess.Getdate(Enty.Authr_Date);

                    double Authr_Amount = 0;
                    if (!string.IsNullOrEmpty(Enty.Authr_Amount1))
                        Authr_Amount += double.Parse(Enty.Authr_Amount1.Trim());
                    if (!string.IsNullOrEmpty(Enty.Authr_Amount2))
                        Authr_Amount += double.Parse(Enty.Authr_Amount2.Trim());
                    if (!string.IsNullOrEmpty(Enty.Authr_Amount3))
                        Authr_Amount += double.Parse(Enty.Authr_Amount3.Trim());

                    PdfPCell A1 = new PdfPCell(new Phrase(Enty.Authr_Number, TableFont));
                    A1.HorizontalAlignment = Element.ALIGN_LEFT;
                    A1.Border = iTextSharp.text.Rectangle.BOX;
                    PAYTable.AddCell(A1);

                    PdfPCell A2 = new PdfPCell(new Phrase(Authr_Date, TableFont));
                    A2.HorizontalAlignment = Element.ALIGN_LEFT;
                    A2.Border = iTextSharp.text.Rectangle.BOX;
                    PAYTable.AddCell(A2);

                    PdfPCell A3 = new PdfPCell(new Phrase(((Enty.BS == "1" || Enty.BS == "Y") ? "Yes" : "No"), TableFont));
                    A3.HorizontalAlignment = Element.ALIGN_RIGHT;
                    A3.Border = iTextSharp.text.Rectangle.BOX;
                    PAYTable.AddCell(A3);

                    PdfPCell A4 = new PdfPCell(new Phrase(Authr_Amount.ToString(), TableFont));
                    A4.HorizontalAlignment = Element.ALIGN_RIGHT;
                    A4.Border = iTextSharp.text.Rectangle.BOX;
                    PAYTable.AddCell(A4);

                    PdfPCell A5 = new PdfPCell(new Phrase(Get_HeatingSource_Desc(Enty.Payment_For), TableFont));
                    A5.HorizontalAlignment = Element.ALIGN_LEFT;
                    A5.Border = iTextSharp.text.Rectangle.BOX;
                    PAYTable.AddCell(A5);

                    PdfPCell A6 = new PdfPCell(new Phrase(Get_CaseWorker_DESC(Enty.Authr_Caseworker), TableFont));
                    A6.HorizontalAlignment = Element.ALIGN_LEFT;
                    A6.Border = iTextSharp.text.Rectangle.BOX;
                    PAYTable.AddCell(A6);

                    PdfPCell A7 = new PdfPCell(new Phrase(Get_Vendor_Desc(Enty.Vendor), TableFont));
                    A7.HorizontalAlignment = Element.ALIGN_LEFT;
                    A7.Border = iTextSharp.text.Rectangle.BOX;
                    PAYTable.AddCell(A7);

                    PdfPCell A8 = new PdfPCell(new Phrase(Invoice_Exists ? Enty.Amount_Dol : " ", TableFont));
                    A8.HorizontalAlignment = Element.ALIGN_LEFT;
                    A8.Border = iTextSharp.text.Rectangle.BOX;
                    PAYTable.AddCell(A8);
                    //rowIndex = Payments_Grid.Rows.Add(Enty.Authr_Number, Authr_Date, ((Enty.BS == "1" || Enty.BS == "Y") ? "Yes" : "No"), Authr_Amount.ToString(), Get_HeatingSource_Desc(Enty.Payment_For), Enty.Authr_Caseworker, Get_Vendor_Desc(Enty.Vendor), Invoice_Exists ? Img_Tick : Img_Blank, Invoice_Exists ? Enty.Amount_Dol : " ", Invc_SW, Enty.MOR_Differential, Enty.Payment_For, Enty.Batch_NO, (Alloc_Short_Fall ? "Y" : "N"), Bundle_SW, Check_SW, Enty.SDWDC);
                }

                document.Add(PAYTable);

            }

        }

        private void EMSRESPdf(Document document)
        {
            BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            iTextSharp.text.Font helvetica = new iTextSharp.text.Font(bf_times, 12, 1);
            BaseFont bf_helv = helvetica.GetCalculatedBaseFont(false);
            iTextSharp.text.Font TimesUnderline = new iTextSharp.text.Font(bf_times, 9, 4);
            BaseFont bf_TimesUnderline = TimesUnderline.GetCalculatedBaseFont(true);

            iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 9);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 8);
            iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 8, 3);
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_times, 9, 1);
            iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 8, 2);
            iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 9, 4);

            PdfPTable EMSRESTable = new PdfPTable(7);
            EMSRESTable.TotalWidth = 550f;
            EMSRESTable.WidthPercentage = 100;
            EMSRESTable.LockedWidth = true;
            float[] widths2 = new float[] { 25f, 35f, 40f, 20f, 18f, 18f, 25f };
            EMSRESTable.SetWidths(widths2);
            EMSRESTable.HorizontalAlignment = Element.ALIGN_CENTER;
            //EMSRESTable.HeaderRows = 2;

            string ProgramName = string.Empty;
            if (programEntity != null)
                ProgramName = programEntity.AgencyName;

            PdfPCell H1 = new PdfPCell(new Phrase("Hierarchy:" + CASEMSTList[0].ApplAgency + "-" + CASEMSTList[0].ApplDept + "-" + CASEMSTList[0].ApplProgram + " " + CASEMSTList[0].ApplYr + "  " + ProgramName, TblFontBold));
            H1.HorizontalAlignment = Element.ALIGN_LEFT;
            H1.Colspan = 3;
            H1.FixedHeight = 15f;
            H1.Border = iTextSharp.text.Rectangle.NO_BORDER;
            EMSRESTable.AddCell(H1);

            PdfPCell H2 = new PdfPCell(new Phrase("App:" + CASEMSTList[0].ApplNo + "  " + LookupDataAccess.GetMemberName(SelSnp.NameixFi, SelSnp.NameixMi, SelSnp.NameixLast, BaseForm.BaseHierarchyCnFormat.ToString()), TblFontBold));
            H2.HorizontalAlignment = Element.ALIGN_RIGHT;
            H2.Colspan = 4;
            H2.FixedHeight = 15f;
            H2.Border = iTextSharp.text.Rectangle.NO_BORDER;
            EMSRESTable.AddCell(H2);

            PdfPCell S2 = new PdfPCell(new Phrase("", TblFontBold));
            S2.HorizontalAlignment = Element.ALIGN_CENTER;
            S2.Colspan = 7;
            S2.FixedHeight = 15f;
            S2.Border = iTextSharp.text.Rectangle.NO_BORDER;
            EMSRESTable.AddCell(S2);

            PdfPCell Header = new PdfPCell(new Phrase("Emergency Services Resources", TblFontBold));
            Header.HorizontalAlignment = Element.ALIGN_CENTER;
            Header.Colspan = 7;
            Header.FixedHeight = 15f;
            Header.Border = iTextSharp.text.Rectangle.BOX;
            EMSRESTable.AddCell(Header);

            string[] HeaderSeq = { "Worker", "Fund Source", "Date Range", "Date", "Amount", "Balance", "Status" };
            for (int i = 0; i < HeaderSeq.Length; ++i)
            {
                PdfPCell cell = new PdfPCell(new Phrase(HeaderSeq[i], TblFontBold));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                //cell.FixedHeight = 15f;
                cell.Border = iTextSharp.text.Rectangle.BOX;
                EMSRESTable.AddCell(cell);
            }
            bool first = false;
            foreach (EMSRESEntity resdetails in EMSRESList)
            {
                if (first)
                {
                    PdfPCell S1 = new PdfPCell(new Phrase("", TblFontBold));
                    S1.HorizontalAlignment = Element.ALIGN_CENTER;
                    S1.Colspan = 7;
                    S1.FixedHeight = 15f;
                    S1.Border = iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    EMSRESTable.AddCell(S1);

                    PdfPCell Header2 = new PdfPCell(new Phrase("Emergency Services Resources", TblFontBold));
                    Header2.HorizontalAlignment = Element.ALIGN_CENTER;
                    Header2.Colspan = 7;
                    Header2.FixedHeight = 15f;
                    Header2.Border = iTextSharp.text.Rectangle.BOX;
                    EMSRESTable.AddCell(Header2);

                    string[] HeaderSeq3 = { "Worker", "Fund Source", "Date Range", "Date", "Amount", "Balance", "Status" };
                    for (int i = 0; i < HeaderSeq3.Length; ++i)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(HeaderSeq3[i], TblFontBold));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        //cell.FixedHeight = 15f;
                        cell.Border = iTextSharp.text.Rectangle.BOX;
                        EMSRESTable.AddCell(cell);
                    }
                }

                resdetails.EMS_Balance = "0.00";
                resdetails.EMS_AdultFlag = "N";
                if (propfundingsource.Count > 0)
                {
                    CommonEntity commonfunddesc = propfundingsource.Find(u => u.Code == resdetails.EMSRES_FUND);
                    if (commonfunddesc != null)
                    {
                        resdetails.EMS_FundDesc = commonfunddesc.Desc.ToString();
                        if (commonfunddesc.Extension != "N")
                        {
                            EMSBDCEntity emsbdcentity = propEmsbdc_List.Find(u => u.BDC_FUND == resdetails.EMSRES_FUND && (Convert.ToDateTime(u.BDC_START) <= Convert.ToDateTime(resdetails.EMSRES_DATE) && Convert.ToDateTime(u.BDC_END) >= Convert.ToDateTime(resdetails.EMSRES_DATE)));
                            if (emsbdcentity != null)
                            {
                                resdetails.EMS_DateRange = LookupDataAccess.Getdate(emsbdcentity.BDC_START) + "  " + LookupDataAccess.Getdate(emsbdcentity.BDC_END);
                                resdetails.EMS_Balance = emsbdcentity.BDC_BALANCE;
                                resdetails.EMS_AdultFlag = emsbdcentity.BDC_AUDIT_FLAG.ToUpper();
                                resdetails.EMS_AllowPosting = emsbdcentity.BDC_ALLOW_POSTING.ToUpper();
                                resdetails.EMS_Cost_Center = emsbdcentity.BDC_COST_CENTER;
                                resdetails.EMS_GL_Account = emsbdcentity.BDC_GL_ACCOUNT;
                                resdetails.EMS_Budget_year = emsbdcentity.BDC_BUDGET_YEAR;
                                resdetails.EMS_Budget = emsbdcentity.BDC_BUDGET;
                                resdetails.EMS_Start = emsbdcentity.BDC_START;
                                resdetails.EMS_End = emsbdcentity.BDC_END;
                                resdetails.EMS_BDC_DATELSTC = emsbdcentity.BDC_DATE_LSTC;
                                resdetails.EMS_BdcIntOrder = emsbdcentity.BDC_INT_ORDER;
                                resdetails.EMS_AccountType = emsbdcentity.BDC_ACCOUNT_TYPE;

                            }

                        }
                        else
                        {
                            resdetails.EMS_NonBudget = commonfunddesc.Code;
                        }
                    }
                }
                if (resdetails.EMSRES_AMOUNT == string.Empty)
                {
                    resdetails.EMS_Balance = "0.00";
                }
                else
                {
                    resdetails.EMS_Balance = resdetails.EMSRES_AMOUNT;
                }

                List<EMSCLAPMAEntity> emsclapmalist = propEMSCLAPMAList.FindAll(u => u.CLA_RES_FUND == resdetails.EMSRES_FUND && u.CLA_RES_SEQ == resdetails.EMSRES_SEQ && Convert.ToDateTime(u.CLA_RES_DATE) == Convert.ToDateTime(resdetails.EMSRES_DATE) && ((u.RES_O_AMOUNT != u.RES_N_AMOUNT) || (u.RES_O_CASEWORKER != u.RES_N_CASEWORKER)));

                if (emsclapmalist.Count > 0)
                    resdetails.EMS_Status = "*";

                if (resdetails.EMSRES_NPDATE != string.Empty)
                {
                    resdetails.EMS_Post = "Nopost";
                    resdetails.EMS_Balance = "0.00";
                }
                else
                {
                    List<EMSCLCPMCEntity> emsclcpmclist = EMSCLCList.FindAll(u => u.CLC_RES_FUND == resdetails.EMSRES_FUND && u.CLC_RES_SEQ == resdetails.EMSRES_SEQ && Convert.ToDateTime(u.CLC_RES_DATE).Date == Convert.ToDateTime(resdetails.EMSRES_DATE).Date);
                    if (emsclcpmclist.Count > 0)
                    {
                        decimal decamount = emsclcpmclist.Sum(u => Convert.ToDecimal(u.PMC_AMOUNT));
                        decamount = Convert.ToDecimal(resdetails.EMS_Balance) - decamount;
                        resdetails.EMS_Balance = decamount.ToString();
                    }
                }

                PdfPCell A1 = new PdfPCell(new Phrase(Get_CaseWorker_DESC(resdetails.EMSRES_CASEWORKER.Trim()), TableFont));
                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                A1.Border = iTextSharp.text.Rectangle.BOX;
                EMSRESTable.AddCell(A1);

                PdfPCell A2 = new PdfPCell(new Phrase(resdetails.EMS_FundDesc, TableFont));
                A2.HorizontalAlignment = Element.ALIGN_LEFT;
                A2.Border = iTextSharp.text.Rectangle.BOX;
                EMSRESTable.AddCell(A2);

                PdfPCell A3 = new PdfPCell(new Phrase(resdetails.EMS_DateRange, TableFont));
                A3.HorizontalAlignment = Element.ALIGN_LEFT;
                A3.Border = iTextSharp.text.Rectangle.BOX;
                EMSRESTable.AddCell(A3);

                PdfPCell A4 = new PdfPCell(new Phrase(LookupDataAccess.Getdate(resdetails.EMSRES_DATE.Trim()), TableFont));
                A4.HorizontalAlignment = Element.ALIGN_LEFT;
                A4.Border = iTextSharp.text.Rectangle.BOX;
                EMSRESTable.AddCell(A4);

                PdfPCell A5 = new PdfPCell(new Phrase(resdetails.EMSRES_AMOUNT, TableFont));
                A5.HorizontalAlignment = Element.ALIGN_LEFT;
                A5.Border = iTextSharp.text.Rectangle.BOX;
                EMSRESTable.AddCell(A5);

                PdfPCell A6 = new PdfPCell(new Phrase(resdetails.EMS_Balance, TableFont));
                A6.HorizontalAlignment = Element.ALIGN_LEFT;
                A6.Border = iTextSharp.text.Rectangle.BOX;
                EMSRESTable.AddCell(A6);

                PdfPCell A7 = new PdfPCell(new Phrase(resdetails.EMS_Post, TableFont));
                A7.HorizontalAlignment = Element.ALIGN_LEFT;
                A7.Border = iTextSharp.text.Rectangle.BOX;
                EMSRESTable.AddCell(A7);

                if (EMSRESTable.Rows.Count > 0)
                {
                    document.Add(EMSRESTable);
                    EMSRESTable.DeleteBodyRows();
                }
                if (EMSCLCList.Count > 0)
                {
                    List<EMSCLCPMCEntity> SelClcList = EMSCLCList.FindAll(u => u.CLC_RES_FUND == resdetails.EMSRES_FUND && u.CLC_RES_SEQ == resdetails.EMSRES_SEQ && Convert.ToDateTime(u.CLC_RES_DATE).Date == Convert.ToDateTime(resdetails.EMSRES_DATE).Date);
                    if (SelClcList.Count > 0)
                    {
                        PdfPTable EMSCLCTable = new PdfPTable(6);
                        EMSCLCTable.TotalWidth = 550f;
                        EMSCLCTable.WidthPercentage = 100;
                        EMSCLCTable.LockedWidth = true;
                        float[] widths = new float[] { 25f, 40f, 20f, 20f, 20f, 25f };
                        EMSCLCTable.SetWidths(widths);
                        EMSCLCTable.HorizontalAlignment = Element.ALIGN_CENTER;

                        PdfPCell Header1 = new PdfPCell(new Phrase("Emergency Services Invoices", TblFontBold));
                        Header1.HorizontalAlignment = Element.ALIGN_LEFT;
                        Header1.Colspan = 7;
                        Header1.FixedHeight = 15f;
                        Header1.Border = iTextSharp.text.Rectangle.BOX;
                        EMSCLCTable.AddCell(Header1);

                        string[] HeaderSeq1 = { "Worker", "Service", "Type", "Amount", "Date", "Worker" };
                        for (int i = 0; i < HeaderSeq1.Length; ++i)
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(HeaderSeq1[i], TblFontBold));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            //cell.FixedHeight = 15f;
                            cell.Border = iTextSharp.text.Rectangle.BOX;
                            EMSCLCTable.AddCell(cell);
                        }


                        string strCamsdesc = string.Empty;
                        string strPType = string.Empty;
                        string strInvamt = string.Empty;
                        string strInvDt = string.Empty;
                        string strInvwkr = string.Empty;
                        string strAdjust = string.Empty;

                        foreach (EMSCLCPMCEntity item in SelClcList)
                        {
                            strAdjust = string.Empty;
                            strCamsdesc = string.Empty;
                            strPType = string.Empty;
                            strInvamt = string.Empty;
                            strInvDt = string.Empty;
                            strInvwkr = string.Empty;
                            //List<EMSCLAPMAEntity> emsclapmalist = propEMSCLAPMAList.FindAll(u => u.CLA_RES_FUND == item.CLC_RES_FUND && u.CLA_RES_SEQ == item.CLC_RES_SEQ && u.CLA_CLC_SEQ == item.CLC_SEQ && Convert.ToDateTime(u.CLA_RES_DATE) == Convert.ToDateTime(emsResentity.EMSRES_DATE));
                            if (emsclapmalist.Count > 0)
                                strAdjust = "*";
                            if (item.PMC_TYPE.ToUpper().Equals("I"))
                            {
                                strPType = "Invoice";
                                strInvamt = item.PMC_INV_AMT.ToString();
                                strInvDt = LookupDataAccess.Getdate(item.PMC_INV_DATE);
                                strInvwkr = item.PMC_INV_WORKER;

                                // newly added 08/11/2015
                                if (item.PMC_INV_AMT.ToString().Trim() == string.Empty)
                                {
                                    strInvamt = string.Empty;
                                    strInvDt = string.Empty;
                                    strInvwkr = string.Empty;
                                }

                            }
                            else if (item.PMC_TYPE.ToUpper().Equals("A"))
                            {
                                strPType = "Authorization";
                                strInvamt = item.PMC_AUTH_AMT.ToString();
                                strInvDt = LookupDataAccess.Getdate(item.PMC_AUTH_DATE);
                                strInvwkr = item.PMC_AUTH_WORKER;

                                // newly added 08/11/2015
                                if (item.PMC_AUTH_AMT.ToString().Trim() == string.Empty && item.PMC_AUTH_LIQUIDATE.ToString() != "Y")
                                {
                                    strInvamt = string.Empty;
                                    strInvDt = string.Empty;
                                    strInvwkr = string.Empty;
                                }
                            }


                            if (CAMASTList.Count > 0)
                            {
                                CAMASTEntity camastentity = CAMASTList.Find(u => u.Code == item.CLC_S_SERVICE_CODE);
                                if (camastentity != null)
                                    strCamsdesc = camastentity.Desc;
                            }
                            //emsResentity.EMSRES_CASEWORKER

                            PdfPCell B1 = new PdfPCell(new Phrase(Get_CaseWorker_DESC(item.CLC_S_CASEWORKER), TableFont));
                            B1.HorizontalAlignment = Element.ALIGN_LEFT;
                            B1.Border = iTextSharp.text.Rectangle.BOX;
                            EMSCLCTable.AddCell(B1);

                            PdfPCell B2 = new PdfPCell(new Phrase(strCamsdesc, TableFont));
                            B2.HorizontalAlignment = Element.ALIGN_LEFT;
                            B2.Border = iTextSharp.text.Rectangle.BOX;
                            EMSCLCTable.AddCell(B2);

                            PdfPCell B3 = new PdfPCell(new Phrase(strPType, TableFont));
                            B3.HorizontalAlignment = Element.ALIGN_LEFT;
                            B3.Border = iTextSharp.text.Rectangle.BOX;
                            EMSCLCTable.AddCell(B3);

                            PdfPCell B4 = new PdfPCell(new Phrase(strInvamt, TableFont));
                            B4.HorizontalAlignment = Element.ALIGN_LEFT;
                            B4.Border = iTextSharp.text.Rectangle.BOX;
                            EMSCLCTable.AddCell(B4);

                            PdfPCell B5 = new PdfPCell(new Phrase(strInvDt, TableFont));
                            B5.HorizontalAlignment = Element.ALIGN_LEFT;
                            B5.Border = iTextSharp.text.Rectangle.BOX;
                            EMSCLCTable.AddCell(B5);

                            PdfPCell B6 = new PdfPCell(new Phrase(Get_CaseWorker_DESC(strInvwkr), TableFont));
                            B6.HorizontalAlignment = Element.ALIGN_LEFT;
                            B6.Border = iTextSharp.text.Rectangle.BOX;
                            EMSCLCTable.AddCell(B6);
                        }
                        document.Add(EMSCLCTable);
                        first = true;
                    }
                    //else first = false;

                }
            }

        }

        List<HierarchyEntity> SP_Programs_List = new List<HierarchyEntity>();
        private string Set_SP_Program_Text(string Prog_Code)
        {
            string Tmp_Hierarchy = "";
            string Sel_CAMS_Program = "";

            foreach (HierarchyEntity Ent in SP_Programs_List)
            {
                Tmp_Hierarchy = Ent.Agency.Trim() + Ent.Dept.Trim() + Ent.Prog.Trim();
                if (Prog_Code == Tmp_Hierarchy)
                {
                    //Sel_CAMS_Program = Tmp_Hierarchy + " - " + Ent.HirarchyName.Trim();
                    Sel_CAMS_Program = Ent.HirarchyName.Trim();
                    break;
                }
            }

            return Sel_CAMS_Program;
        }

        List<Captain.Common.Utilities.ListItem> CaseWorker_List = new List<Captain.Common.Utilities.ListItem>();
        private void Fill_CaseWorker()
        {

            //DataSet ds2 = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(MainMenuAgency, MainMenuDept, MainMenuProgram);
            DataSet ds2 = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(AGY, "**", "**");
            string strNameFormat = null, strCwFormat = null;
            if (ds2.Tables[0].Rows.Count > 0)
            {
                strNameFormat = ds2.Tables[0].Rows[0]["HIE_CN_FORMAT"].ToString();
                strCwFormat = ds2.Tables[0].Rows[0]["HIE_CW_FORMAT"].ToString();
            }

            DataSet ds1 = Captain.DatabaseLayer.CaseMst.GetCaseWorker(strCwFormat, AGY, Dept, Prog);
            if (ds1.Tables.Count > 0)
            {
                DataTable dt1 = ds1.Tables[0];
                if (dt1.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt1.Rows)
                        CaseWorker_List.Add(new Captain.Common.Utilities.ListItem(dr["NAME"].ToString().Trim(), dr["PWH_CASEWORKER"].ToString().Trim()));
                }
            }
        }

        private string Get_CaseWorker_DESC(string Worker_Code)
        {
            string DESC = null;
            foreach (Captain.Common.Utilities.ListItem List in CaseWorker_List)
            {
                if (List.Value.ToString().Trim() == Worker_Code.Trim())
                {
                    DESC = List.Text; break;
                }
            }

            return DESC;
        }

        List<CommonEntity> LanguagesList = new List<CommonEntity>();
        private void Fill_Languages_List()
        {
            LanguagesList = _model.lookupDataAccess.GetPrimaryLanguage();
        }

        private string Get_Language_Desc(string LAng_Code)
        {
            string Lang_Desc = null;
            foreach (CommonEntity Entity in LanguagesList)
            {
                if (Entity.Code == LAng_Code)
                {
                    Lang_Desc = Entity.Desc; break;
                }
            }

            return Lang_Desc;
        }

        List<CAMASTEntity> CAMASTList = new List<CAMASTEntity>();
        private void Fill_CAMAST_List()
        {
            CAMASTList = _model.SPAdminData.Browse_CAMAST(null, null, null, null);
        }

        private string Get_CA_Desc(string CACode)
        {
            string CA_Desc = null;
            foreach (CAMASTEntity Entity in CAMASTList)
            {
                if (Entity.Code == CACode)
                {
                    CA_Desc = Entity.Desc; break;
                }
            }

            return CA_Desc;
        }

        List<MSMASTEntity> MSMASTList = new List<MSMASTEntity>();
        private void Fill_MSMAST_List()
        {
            MSMASTList = _model.SPAdminData.Browse_MSMAST(null, null, null, null, null);
        }

        private string Get_MS_Desc(string MSCode)
        {
            string MS_Desc = null;
            foreach (MSMASTEntity Entity in MSMASTList)
            {
                if (Entity.Code == MSCode)
                {
                    MS_Desc = Entity.Desc; break;
                }
            }

            return MS_Desc;
        }



        List<CommonEntity> Heating_List = new List<CommonEntity>();
        private string Get_HeatingSource_Desc(string Heating_Source)
        {
            string Heating_Source_Desc = string.Empty;
            foreach (CommonEntity List in Heating_List)
            {
                if (List.Code == Heating_Source)
                {
                    Heating_Source_Desc = List.Desc;
                    break;
                }
            }
            return Heating_Source_Desc;
        }

        List<CASEVDDEntity> Vendor_Brose_List = new List<CASEVDDEntity>();
        private void Get_Vendor_Brose_List()
        {
            CASEVDDEntity Search_Entity = new CASEVDDEntity(true);
            Vendor_Brose_List = _model.SPAdminData.Browse_CASEVDD(Search_Entity, "Browse");

            if (BaseForm.BaseAgencyControlDetails.AgyVendor == "Y")
                Vendor_Brose_List = Vendor_Brose_List.FindAll(u => u.VDD_Agency == BaseForm.BaseAgency);
        }

        private string Get_Vendor_Desc(string Vendor)
        {
            string Vendor_Name = string.Empty;
            foreach (CASEVDDEntity Ent in Vendor_Brose_List)
            {
                if (Ent.Code == Vendor)
                {
                    Vendor_Name = Ent.Name;
                    break;
                }
            }
            return Vendor_Name;
        }

        private Wisej.Web.HtmlPanel htmlBox1;
        private void frmViewer_Load(string strFileName)
        {
            //#if VWG
            // Create HtmlBox for file viewing in browser
            this.htmlBox1 = new Wisej.Web.HtmlPanel();

            if (BaseForm.BaseAgencyControlDetails.ReportSwitch.ToUpper() == "Y")
            {
                //string strpath = GetSiteUrl();
                //strpath = strpath.Replace("///", "/");
                //strFileName = strFileName.Replace(" ", "%20");
                ////URI uri = new URI(string.replace(" ", "%20"));
                //strpath = strpath + "ViewPdfForm.aspx?Name=" + strFileName;


                //this.pdfViewer.PdfSource = strFileName;

                Wisej.Web.PdfViewer pdfViewer = new PdfViewer();
                // Add it to form
                pdfViewer.Dock = Wisej.Web.DockStyle.Fill;
                this.tabControl1.SelectedTab.Controls.Add(pdfViewer);

                pdfViewer.PdfSource = strFileName;
                //this.htmlBox1.Html = "<HTML><iframe src=" + strpath + " height=100% width=100%></iframe></HTML>";
                //this.htmlBox1.Update();
            }
            else
            {
                //this.htmlBox1.Anchor = ((Wisej.Web.AnchorStyles)((((Wisej.Web.AnchorStyles.Top | Wisej.Web.AnchorStyles.Bottom)
                //| Wisej.Web.AnchorStyles.Left)
                //| Wisej.Web.AnchorStyles.Right)));
                ////this.htmlBox1.ContentType = "text/html";
                //// this.htmlBox1.Expires = -1;
                //this.htmlBox1.Html = "";
                //this.htmlBox1.Location = new System.Drawing.Point(12, 4);
                //this.htmlBox1.Name = "htmlBox1";
                ////this.htmlBox1.Path = "";
                ////this.htmlBox1.Resource = null;
                //this.htmlBox1.Size = new System.Drawing.Size(750, 460);
                //this.htmlBox1.TabIndex = 1;
                ////this.htmlBox1.Url = "";
                //// Add it to form
                //this.htmlBox1.Dock = Wisej.Web.DockStyle.Fill;
                Wisej.Web.PdfViewer pdfViewer = new PdfViewer();
                // Add it to form
                pdfViewer.Dock = Wisej.Web.DockStyle.Fill;
                this.tabControl1.SelectedTab.Controls.Add(pdfViewer);
                //this.Controls.Add(this.htmlBox1);


                // Show the report

                //htmlBox1.Resource = new GatewayResourceHandle(new GatewayReference(this, "FileContent"));
             //   htmlBox1.Update();
            }
            //#endif
        }

        //void IGatewayComponent.ProcessRequest(IContext objContext, string strAction)
        //{
        //    // Try to get the gateway handler
        //    IGatewayHandler objGatewayHandler = ProcessGatewayRequest(objContext.HttpContext, strAction);

        //    if (objGatewayHandler != null)
        //    {
        //        objGatewayHandler.ProcessGatewayRequest(objContext, this);
        //    }
        //}

        //protected override IGatewayHandler ProcessGatewayRequest(HttpContext objHttpContext, string strAction)
        //{
        //    IGatewayHandler objGH = null;

        //    if (!String.IsNullOrEmpty(strAction) && strAction.Equals("FileContent"))
        //    {
        //        if (objHttpContext != null && objHttpContext.Response != null)
        //        {
        //            // Disable cache.
        //            objHttpContext.Response.Expires = -1;

        //            string strFileName = PdfName;
        //            string strContentType = getMimeFromFile(PdfName);

        //            if (strContentType == "unknown/unknown")
        //            {
        //                MessageBox.Show("Unknown MIME type for file " + PdfName);
        //                return null;
        //            }

        //            objHttpContext.Response.ContentType = strContentType;

        //            // Set Headers for the Pdf attachment
        //            objHttpContext.Response.AddHeader("Content-Disposition", string.Format("inline; filename={0};", Path.GetFileName(strFileName)));

        //            if (File.Exists(strFileName))
        //            {
        //                // Write Pdf body.
        //                objHttpContext.Response.WriteFile(strFileName);
        //            }
        //            //else
        //            //{
        //            //    MessageBox.Show("File does not exist: " + strFileName);
        //            //    // Create an html writer.
        //            //    SystemWeb.HtmlTextWriter objHtmlWriter = new SystemWeb.HtmlTextWriter(objHttpContext.Response.Output);

        //            //    if (objHtmlWriter != null)
        //            //    {
        //            //        // Write empty body.
        //            //        objHtmlWriter.Write("<p> File was not found or contained an invalid mime type - see application log </p> ");
        //            //        // Flush html writer.
        //            //        objHtmlWriter.Flush();
        //            //    }
        //            //}
        //        }
        //    }
        //    return objGH;
        //}

        // Get the MIME type from a file
        [DllImport(@"urlmon.dll", CharSet = CharSet.Auto)]
        private extern static System.UInt32 FindMimeFromData(
            System.UInt32 pBC,
            [MarshalAs(UnmanagedType.LPStr)] System.String pwzUrl,
            [MarshalAs(UnmanagedType.LPArray)] byte[] pBuffer,
            System.UInt32 cbSize,
            [MarshalAs(UnmanagedType.LPStr)] System.String pwzMimeProposed,
            System.UInt32 dwMimeFlags,
            out System.UInt32 ppwzMimeOut,
            System.UInt32 dwReserverd
        );

        public static string getMimeFromFile(string filename)
        {
            if (!File.Exists(filename))
            {
                //throw new FileNotFoundException(filename + " not found");
                MessageBox.Show("File not found " + filename);
                return "unknown/unknown";
            }

            byte[] buffer = new byte[256];
            using (FileStream fs = new FileStream(filename, FileMode.Open))
            {
                if (fs.Length >= 256)
                    fs.Read(buffer, 0, 256);
                else
                    fs.Read(buffer, 0, (int)fs.Length);
            }
            try
            {
                System.UInt32 mimetype;
                FindMimeFromData(0, null, buffer, 256, null, 0, out mimetype, 0);
                System.IntPtr mimeTypePtr = new IntPtr(mimetype);
                string mime = Marshal.PtrToStringUni(mimeTypePtr);
                Marshal.FreeCoTaskMem(mimeTypePtr);
                return mime;
            }
            catch (Exception e)
            {
                return "unknown/unknown";
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (tabControl1.SelectedTab.Tag.ToString() == "A")
            //    On_SaveFormClosed(Selection);
            if (tabControl1.SelectedTab.Tag.ToString() == "I")
            {
                PdfName = "I" + BaseForm.BaseApplicationNo + ".pdf"; //On_SaveFormClosed("I");
                PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
                frmViewer_Load(PdfName);
            }
            else if (tabControl1.SelectedTab.Tag.ToString() == "S")
            {
                PdfName = "S" + BaseForm.BaseApplicationNo + ".pdf";
                PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
                frmViewer_Load(PdfName);
                //On_SaveFormClosed("S");
            }
            else if (tabControl1.SelectedTab.Tag.ToString() == "C")
            {
                PdfName = "C" + BaseForm.BaseApplicationNo + ".pdf";
                PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
                frmViewer_Load(PdfName);
                //On_SaveFormClosed("C");
            }
            else if (tabControl1.SelectedTab.Tag.ToString() == "R")
            {
                PdfName = "R" + BaseForm.BaseApplicationNo + ".pdf";
                PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
                frmViewer_Load(PdfName);
                //On_SaveFormClosed("R");
            }
            else if (tabControl1.SelectedTab.Tag.ToString() == "M")
            {
                PdfName = "M" + BaseForm.BaseApplicationNo + ".pdf";
                PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
                frmViewer_Load(PdfName);
                //On_SaveFormClosed("M");
            }
            else if (tabControl1.SelectedTab.Tag.ToString() == "L")
            {
                PdfName = "L" + BaseForm.BaseApplicationNo + ".pdf";
                PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
                frmViewer_Load(PdfName);
                //On_SaveFormClosed("L");
            }
            else if (tabControl1.SelectedTab.Tag.ToString() == "E")
            {
                PdfName = "E" + BaseForm.BaseApplicationNo + ".pdf";
                PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
                frmViewer_Load(PdfName);
                //On_SaveFormClosed("E");
            }
        }

        public static string GetSiteUrl()
        {
            string url = string.Empty;
            //HttpRequest request =  HttpContext.Current.Request;

            //if (request.IsSecureConnection)
            if (Application.IsSecure)
                url = "https://";
            else
                url = "http://";

            return url + HttpContext.Current.Request.Url.Authority + "/" + HttpContext.Current.Request.ApplicationPath + "/";


        }


        public string HKLM_GetString(string path, string key)
        {
            try
            {
                RegistryKey rk = Registry.LocalMachine.OpenSubKey(path);
                if (rk == null) return "";
                return (string)rk.GetValue(key);
            }
            catch { return ""; }
        }

        public string FriendlyName()
        {
            string ProductName = HKLM_GetString(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ProductName");
            string CSDVersion = HKLM_GetString(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", "CSDVersion");
            if (ProductName != "")
            {
                return (ProductName.StartsWith("Microsoft") ? "" : "Microsoft ") + ProductName +
                            (CSDVersion != "" ? " " + CSDVersion : "");
            }
            return "";
        }

    }
}