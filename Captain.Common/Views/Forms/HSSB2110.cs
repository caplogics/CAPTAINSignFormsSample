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
using System.Globalization;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class HSSB2110 : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        //private GridControl _intakeHierarchy = null;
        private bool boolChangeStatus = false;
        private CaptainModel _model = null;
        private List<HierarchyEntity> _selectedHierarchies = null;

        #endregion
        public HSSB2110(BaseForm baseForm, PrivilegeEntity privileges)
        {
            InitializeComponent();
            _model = new CaptainModel();
            BaseForm = baseForm;
            Privileges = privileges;
            Agency = BaseForm.BaseAgency; Depart = BaseForm.BaseDept; Program = BaseForm.BaseProg;
            strYear = BaseForm.BaseYear;
            if(!string.IsNullOrEmpty(BaseForm.BaseYear))
                Set_Report_Hierarchy(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear);
           
            propReportPath = _model.lookupDataAccess.GetReportPath();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            //strFolderPath = Consts.Common.ReportFolderLocation + BaseForm.UserID + "\\";    // Run at Server
            txtEndMnth.Validator = TextBoxValidation.IntegerValidator;
            txtEndYear.Validator = TextBoxValidation.IntegerValidator;
            txtStrtMnth.Validator = TextBoxValidation.IntegerValidator;
            txtStrtYear.Validator = TextBoxValidation.IntegerValidator;
            this.Text =/* Privileges.Program + " - " +*/ Privileges.PrivilegeName.Trim();
            //FillActive_Inactive();
            FormLoad();
            
        }
        string Agency = string.Empty, Depart = string.Empty, Program = string.Empty, strYear = string.Empty;
        #region properties

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public string propReportPath { get; set; }

        public List<CaseSiteEntity> propCaseSiteEntity { get; set; }
        

        //List<ProgramDefinitionEntity> Prog_Entity { get; set; }

        public bool IsSaveValid { get; set; }

        #endregion                      

        private void FillActive_Inactive()
        {
            cmbActive.Items.Clear();

            List<Captain.Common.Utilities.ListItem> listItem = new List<Captain.Common.Utilities.ListItem>();
            listItem.Add(new Captain.Common.Utilities.ListItem("Active", "A"));
            listItem.Add(new Captain.Common.Utilities.ListItem("Inactive", "I"));
            listItem.Add(new Captain.Common.Utilities.ListItem("Both", "B"));

            cmbActive.Items.AddRange(listItem.ToArray());
            cmbActive.SelectedIndex = 2;
        }

        private void FormLoad()
        {
            if (rbClass.Checked.Equals(true))
            {
                this.Size = new System.Drawing.Size(750, 205);
                //this.panel5.Location = new System.Drawing.Point(0, 95);
                pnlSite.Visible = false;
            }
            else
            {
                this.Size = new System.Drawing.Size(750, 271);
               // this.panel5.Location = new System.Drawing.Point(0, 145);
                pnlSite.Visible = true;

                //Prog_Entity = _model.HierarchyAndPrograms.GetCaseDep();
                
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
            {
                FillYearCombo(Agy, Dept, Prog, Year);
                rbSelectedSite.Enabled = true;
            }
            else
            {
                Agency = Agy; Depart = Dept; Program = Prog; strYear = "    ";
                rbSelectedSite.Enabled = false;
                //Fill_SP_Grid(); FillProgramsCombo();
                this.Txt_HieDesc.Size = new System.Drawing.Size(660, 25);
            }
        }

        string DepYear; int Startmonth = 0; int endMonth = 0;
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
            ProgramDefinitionEntity programDefinitionList = _model.HierarchyAndPrograms.GetCaseDepadp(Agency.Trim(), Depart.Trim(), Program.Trim());
            if (!string.IsNullOrEmpty(programDefinitionList.StartMonth.Trim()) && !string.IsNullOrEmpty(programDefinitionList.EndMonth.Trim()))
            {
                Startmonth = int.Parse(programDefinitionList.StartMonth.Trim()); endMonth = int.Parse(programDefinitionList.EndMonth.Trim());
            }
            if(rbSchedule.Checked.Equals(true))
            {
                if ((!string.IsNullOrEmpty(programDefinitionList.StartMonth.Trim()) && !string.IsNullOrEmpty(programDefinitionList.EndMonth.Trim())) || (Startmonth!=0 && endMonth!=0) )
                {
                    if (programDefinitionList.StartMonth.Trim() == "0")
                        txtStrtMnth.Text = "";
                    else
                        txtStrtMnth.Text = programDefinitionList.StartMonth.Trim();
                    if (programDefinitionList.EndMonth.Trim() == "0")
                        txtEndMnth.Text = "";
                    else
                        txtEndMnth.Text = programDefinitionList.EndMonth.Trim();
                    
                    if (Startmonth != 0 && endMonth != 0)
                    {
                        if (programDefinitionList.StartMonth.Trim() != "")
                        {
                            if (int.Parse(programDefinitionList.StartMonth.Trim()) < 12)
                                txtStrtYear.Text = programDefinitionList.DepYear.Trim();
                            else txtStrtYear.Text = (int.Parse(programDefinitionList.DepYear.Trim()) + 1).ToString();


                            if (int.Parse(programDefinitionList.EndMonth.Trim()) > int.Parse(programDefinitionList.StartMonth.Trim()))
                                txtEndYear.Text = programDefinitionList.DepYear.Trim();
                            else txtEndYear.Text = (int.Parse(programDefinitionList.DepYear.Trim()) + 1).ToString();
                        }
                    }
                    else
                    {
                        txtStrtYear.Text = "";
                        txtEndYear.Text = "";
                    }
                }
            }
            //Fill_SP_Grid(); FillProgramsCombo();
            if (!string.IsNullOrEmpty(Program_Year.Trim()))
                this.Txt_HieDesc.Size = new System.Drawing.Size(580, 25);
            else
                this.Txt_HieDesc.Size = new System.Drawing.Size(660, 25);
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

        private void CmbYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program_Year = "    ";
            if (!(string.IsNullOrEmpty(((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString())))
                Program_Year = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString();
        }

        private void Pb_Search_Hie_Click(object sender, EventArgs e)
        {
            /*HierarchieSelectionFormNew hierarchieSelectionForm = new HierarchieSelectionFormNew(BaseForm, Current_Hierarchy_DB, "Master", "", "D", "Reports");
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

        PdfContentByte cb;
        int X_Pos, Y_Pos;
        string strFolderPath = string.Empty;
        string Random_Filename = null; string PdfName = null;
        BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
        private void On_SaveForm_Closed(object sender, EventArgs e)
        {
            PdfListForm form = sender as PdfListForm;
            if (form.DialogResult == DialogResult.OK)
            {
                string PdfName = "Pdf File";
                PdfName = form.GetFileName();
                //PdfName = strFolderPath + PdfName;
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

                Document document = new Document(PageSize.A4, 30f, 30f, 30f, 50f);
                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                document.Open();
                BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, false);
                BaseFont bf_Times = BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1250, false);
                iTextSharp.text.Font fc = new iTextSharp.text.Font(bfTimes, 10, 2);
                iTextSharp.text.Font fc1 = new iTextSharp.text.Font(bf_Times, 10, 2, BaseColor.BLUE);
                iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bfTimes, 10);
                iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_Times, 10);
                cb = writer.DirectContent;

                PrintHeaderPage(document, writer);


                List<ReportSiteEntity> Site_list = new List<ReportSiteEntity>();


                //CaseSiteEntity Search_Entity = new CaseSiteEntity(true);
                //Search_Entity.SiteAGENCY = Agency; Search_Entity.SiteDEPT = Depart;
                //Search_Entity.SitePROG = Program; Search_Entity.SiteYEAR = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString().Trim();
                string Year_Value=string.Empty;
                if(CmbYear.Visible==true)
                    Year_Value=((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString().Trim();
                else Year_Value = null;
                Site_list = _model.CaseMstData.HSSB2110_Report(Agency, Depart, Program, Year_Value);
                //if (((Captain.Common.Utilities.ListItem)cmbActive.SelectedItem).Value.ToString().Trim() == "A")
                //    Site_list = Site_list.FindAll(u => u.SITEACTIVE.Equals("Y"));
                //else if (((Captain.Common.Utilities.ListItem)cmbActive.SelectedItem).Value.ToString().Trim() == "I")
                //    Site_list = Site_list.FindAll(u => u.SITEACTIVE.Equals("N"));
                if (BaseForm.BaseAgencyControlDetails.SiteSecurity == "1")
                {
                    List<HierarchyEntity> userHierarchy = _model.UserProfileAccess.GetUserHierarchyByID(BaseForm.UserID);
                    HierarchyEntity hierarchyEntity = new HierarchyEntity();
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
                            { hierarchyEntity = null; }
                        }
                    }

                    if (hierarchyEntity != null)
                    {
                        if (hierarchyEntity.Sites.Length > 0)
                        {
                            string[] Sites = hierarchyEntity.Sites.Split(',');
                            List<ReportSiteEntity> Selsites = new List<ReportSiteEntity>();
                            for (int i = 0; i < Sites.Length; i++)
                            {
                                if (!string.IsNullOrEmpty(Sites[i].ToString().Trim()))
                                {
                                    foreach (ReportSiteEntity casesite in Site_list) //Site_List)//ListcaseSiteEntity)
                                    {
                                        if (Sites[i].ToString() == casesite.SiteNUMBER && casesite.SiteROOM != "0000")
                                        {
                                            Selsites.Add((casesite));
                                            //break;
                                        }
                                        // Sel_Site_Codes += "'" + casesite.SiteNUMBER + "' ,";
                                    }
                                }
                            }
                            Site_list = Selsites;
                            //strsiteRoomNames = hierarchyEntity.Sites;
                        }
                    }
                }



                if (rbCondensed.Checked.Equals(true))
                {
                    document.NewPage();

                    PdfPTable table = new PdfPTable(6);
                    table.TotalWidth = 550f;
                    table.WidthPercentage = 100;
                    table.LockedWidth = true;
                    float[] widths = new float[] { 15f, 15f, 20f, 60f, 30f, 100f };
                    table.SetWidths(widths);
                    table.HorizontalAlignment = Element.ALIGN_CENTER;



                    PdfPCell Headercell1 = new PdfPCell(new Phrase("Site", TblFontBold));
                    Headercell1.HorizontalAlignment = Element.ALIGN_CENTER;
                    Headercell1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Headercell1.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(Headercell1);

                    PdfPCell Headercell2 = new PdfPCell(new Phrase("Room", TblFontBold));
                    Headercell2.HorizontalAlignment = Element.ALIGN_CENTER;
                    Headercell2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Headercell2.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(Headercell2);

                    PdfPCell Headercell3 = new PdfPCell(new Phrase("Am/Pm", TblFontBold));
                    Headercell3.HorizontalAlignment = Element.ALIGN_CENTER;
                    Headercell3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Headercell3.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(Headercell3);

                    PdfPCell Headercell4 = new PdfPCell(new Phrase("Name", TblFontBold));
                    Headercell4.HorizontalAlignment = Element.ALIGN_CENTER;
                    Headercell4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Headercell4.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(Headercell4);

                    PdfPCell Headercell5 = new PdfPCell(new Phrase("Telephone", TblFontBold));
                    Headercell5.HorizontalAlignment = Element.ALIGN_CENTER;
                    Headercell5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Headercell5.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(Headercell5);

                    PdfPCell Headercell6 = new PdfPCell(new Phrase("Address", TblFontBold));
                    Headercell6.HorizontalAlignment = Element.ALIGN_CENTER;
                    Headercell6.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Headercell6.BackgroundColor = BaseColor.LIGHT_GRAY;
                    table.AddCell(Headercell6);


                    if (Site_list.Count > 0)
                    {
                        foreach (ReportSiteEntity Entity in Site_list)
                        {
                            string Stie_AMPM = string.Empty;
                            if (Entity.SiteROOM != "0000")
                            {
                                PdfPCell Site = new PdfPCell(new Phrase(Entity.SiteNUMBER.Trim(), TableFont));
                                Site.HorizontalAlignment = Element.ALIGN_LEFT;
                                Site.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(Site);

                                PdfPCell Room = new PdfPCell(new Phrase(Entity.SiteROOM.Trim(), TableFont));
                                Room.HorizontalAlignment = Element.ALIGN_LEFT;
                                Room.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(Room);

                                if (Entity.SiteAM_PM == "A")
                                    Stie_AMPM = "AM";
                                else if (Entity.SiteAM_PM == "P")
                                    Stie_AMPM = "PM";
                                else if (Entity.SiteAM_PM == "F")
                                    Stie_AMPM = "FULL";
                                else if (Entity.SiteAM_PM == "E")
                                    Stie_AMPM = "EXTD";
                                PdfPCell AM = new PdfPCell(new Phrase(Stie_AMPM, TableFont));
                                AM.HorizontalAlignment = Element.ALIGN_LEFT;
                                AM.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(AM);

                                PdfPCell Name = new PdfPCell(new Phrase(Entity.SiteNAME.Trim(), TableFont));
                                Name.HorizontalAlignment = Element.ALIGN_LEFT;
                                Name.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(Name);

                                //string Phone_Num = string.Empty;
                                MaskedTextBox mskPhn = new MaskedTextBox();
                                mskPhn.Mask = "(999) 000-0000";
                                if (!string.IsNullOrEmpty(Entity.SitePHONE.Trim()))
                                {
                                    mskPhn.Text = Entity.SitePHONE.Trim();
                                }
                                PdfPCell Phone = new PdfPCell(new Phrase(mskPhn.Text.Trim(), TableFont));
                                Phone.HorizontalAlignment = Element.ALIGN_LEFT;
                                Phone.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(Phone);

                                PdfPCell Address = new PdfPCell(new Phrase(Entity.SiteSTREET.Trim() + " " + Entity.SiteCITY.Trim() + " " + Entity.SiteSTATE.Trim() + " " + Entity.SiteZIP, TableFont));
                                Address.HorizontalAlignment = Element.ALIGN_LEFT;
                                Address.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(Address);
                            }
                        }
                    }

                    document.Add(table);
                }
                else if(rbFull.Checked.Equals(true))
                {
                    document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                    document.NewPage();

                    PdfPTable table = new PdfPTable(10);
                    table.TotalWidth = 700f;
                    table.WidthPercentage = 100;
                    table.LockedWidth = true;
                    float[] widths = new float[] { 20f, 15f, 20f, 80f, 50f, 30f, 30f, 30f, 30f,55f };
                    table.SetWidths(widths);
                    table.HorizontalAlignment = Element.ALIGN_CENTER;

                    DataSet dsLang = DatabaseLayer.Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.LANGUAGECODES);
                    DataTable dtLang = dsLang.Tables[0];

                    DataSet dsFund = DatabaseLayer.Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.CASEMNGMTFUNDSRC, "A");
                    DataTable dtFund = dsFund.Tables[0];

                    if (Site_list.Count > 0)
                    {
                        foreach (ReportSiteEntity REntity in Site_list)
                        {
                            string Stie_AMPM = string.Empty;
                            if (REntity.SiteROOM != "0000")
                            {
                                PdfPCell Site = new PdfPCell(new Phrase(REntity.SiteNUMBER.Trim(), TableFont));
                                Site.HorizontalAlignment = Element.ALIGN_LEFT;
                                Site.Border = iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.LEFT_BORDER;
                                table.AddCell(Site);

                                PdfPCell room = new PdfPCell(new Phrase(REntity.SiteROOM.Trim(), TableFont));
                                room.HorizontalAlignment = Element.ALIGN_LEFT;
                                room.Border = iTextSharp.text.Rectangle.TOP_BORDER;
                                table.AddCell(room);

                                if (REntity.SiteAM_PM == "A")
                                    Stie_AMPM = "AM";
                                else if (REntity.SiteAM_PM == "P")
                                    Stie_AMPM = "PM";
                                else if (REntity.SiteAM_PM == "F")
                                    Stie_AMPM = "FULL";
                                else if (REntity.SiteAM_PM == "E")
                                    Stie_AMPM = "EXTD";
                                PdfPCell AMPM = new PdfPCell(new Phrase(Stie_AMPM.Trim(), TableFont));
                                AMPM.HorizontalAlignment = Element.ALIGN_LEFT;
                                AMPM.Border = iTextSharp.text.Rectangle.TOP_BORDER;
                                table.AddCell(AMPM);

                                PdfPCell Name = new PdfPCell(new Phrase(REntity.SiteNAME.Trim(), TableFont));
                                Name.HorizontalAlignment = Element.ALIGN_LEFT;
                                Name.Border = iTextSharp.text.Rectangle.TOP_BORDER;
                                table.AddCell(Name);

                                PdfPCell Cat = new PdfPCell(new Phrase("Capacity  "+REntity.SITE_FUNDED_SLOTS.Trim(), TableFont));
                                Cat.HorizontalAlignment = Element.ALIGN_LEFT;
                                Cat.Border = iTextSharp.text.Rectangle.TOP_BORDER;
                                table.AddCell(Cat);

                                PdfPCell Stdt = new PdfPCell(new Phrase("Start Date ", TableFont));
                                Stdt.HorizontalAlignment = Element.ALIGN_LEFT;
                                Stdt.Border = iTextSharp.text.Rectangle.TOP_BORDER;
                                table.AddCell(Stdt);

                                PdfPCell Stdt1 = new PdfPCell(new Phrase(LookupDataAccess.Getdate(REntity.SiteCLASS_START.Trim()), TableFont));
                                Stdt1.HorizontalAlignment = Element.ALIGN_LEFT;
                                Stdt1.Border = iTextSharp.text.Rectangle.TOP_BORDER;
                                table.AddCell(Stdt1);

                                PdfPCell StTime = new PdfPCell(new Phrase("Start Time ", TableFont));
                                StTime.HorizontalAlignment = Element.ALIGN_LEFT;
                                StTime.Border = iTextSharp.text.Rectangle.TOP_BORDER;
                                table.AddCell(StTime);

                                PdfPCell StTime1 = new PdfPCell(new Phrase(LookupDataAccess.GetTime(REntity.SiteSTART_TIME.Trim()), TableFont));
                                StTime1.HorizontalAlignment = Element.ALIGN_LEFT;
                                StTime1.Border = iTextSharp.text.Rectangle.TOP_BORDER;
                                table.AddCell(StTime1);

                                //string Phone_No = string.Empty;
                                //if (REntity.SitePHONE.Trim().Length == 10)
                                //    Phone_No = "(" + REntity.SitePHONE.Trim().Substring(0, 3) + ")" + " " + REntity.SitePHONE.Trim().Substring(3, 3) + "-" + REntity.SitePHONE.Trim().Substring(6, 4);
                                //else
                                //    Phone_No = REntity.SitePHONE.Trim();
                                MaskedTextBox mskPhn = new MaskedTextBox();
                                mskPhn.Mask = "(999) 000-0000";
                                if (!string.IsNullOrEmpty(REntity.SitePHONE.Trim()))
                                    mskPhn.Text = REntity.SitePHONE.Trim();

                                PdfPCell Phone = new PdfPCell(new Phrase("Phone " + mskPhn.Text.Trim(), TableFont));
                                Phone.HorizontalAlignment = Element.ALIGN_LEFT;
                                Phone.Border = iTextSharp.text.Rectangle.TOP_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER;
                                table.AddCell(Phone);

                                PdfPCell Space = new PdfPCell(new Phrase("", TableFont));
                                Space.HorizontalAlignment = Element.ALIGN_LEFT;
                                Space.Colspan = 3;
                                Space.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                table.AddCell(Space);

                                PdfPCell Street = new PdfPCell(new Phrase(REntity.SiteSTREET.Trim(), TableFont));
                                Street.HorizontalAlignment = Element.ALIGN_LEFT;
                                Street.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(Street);

                                PdfPCell Trans = new PdfPCell(new Phrase("Trans Area " +REntity.SiteTRAN_AREA.Trim(), TableFont));
                                Trans.HorizontalAlignment = Element.ALIGN_LEFT;
                                Trans.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(Trans);

                                PdfPCell EndDt = new PdfPCell(new Phrase("End Date ", TableFont));
                                EndDt.HorizontalAlignment = Element.ALIGN_LEFT;
                                EndDt.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(EndDt);

                                PdfPCell EndDt1 = new PdfPCell(new Phrase(LookupDataAccess.Getdate(REntity.SiteCLASS_END.Trim()), TableFont));
                                EndDt1.HorizontalAlignment = Element.ALIGN_LEFT;
                                EndDt1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(EndDt1);

                                PdfPCell EndTime = new PdfPCell(new Phrase("End Time ", TableFont));
                                EndTime.HorizontalAlignment = Element.ALIGN_LEFT;
                                EndTime.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(EndTime);

                                PdfPCell EndTime1 = new PdfPCell(new Phrase(LookupDataAccess.GetTime(REntity.SiteEND_TIME.Trim()), TableFont));
                                EndTime1.HorizontalAlignment = Element.ALIGN_LEFT;
                                EndTime1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(EndTime1);

                                //if (REntity.SiteFAX.Trim().Length == 10)
                                //    Phone_No = "(" + REntity.SiteFAX.Trim().Substring(0, 3) + ")" + " " + REntity.SiteFAX.Trim().Substring(3, 3) + "-" + REntity.SiteFAX.Trim().Substring(6, 4);
                                //else
                                //    Phone_No = REntity.SiteFAX.Trim();
                                MaskedTextBox mskPhn1 = new MaskedTextBox();
                                mskPhn1.Mask = "(999) 000-0000";
                                if (!string.IsNullOrEmpty(REntity.SiteFAX.Trim()))
                                    mskPhn1.Text = REntity.SiteFAX.Trim();

                                PdfPCell Fax = new PdfPCell(new Phrase("Fax " + mskPhn1.Text.Trim(), TableFont));
                                Fax.HorizontalAlignment = Element.ALIGN_LEFT;
                                Fax.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                table.AddCell(Fax);

                                PdfPCell Space1 = new PdfPCell(new Phrase("", TableFont));
                                Space1.HorizontalAlignment = Element.ALIGN_LEFT;
                                Space1.Colspan = 3;
                                Space1.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                table.AddCell(Space1);

                                PdfPCell City = new PdfPCell(new Phrase(REntity.SiteCITY.Trim()+" "+REntity.SiteSTATE.Trim()+" "+REntity.SiteZIP.Trim(), TableFont));
                                City.HorizontalAlignment = Element.ALIGN_LEFT;
                                City.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(City);

                                PdfPCell Lang = new PdfPCell(new Phrase("Language in Class", TableFont));
                                Lang.HorizontalAlignment = Element.ALIGN_LEFT;
                                //Lang.Colspan = 2;
                                Lang.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(Lang);
                                string Language = string.Empty;
                                if(REntity.SiteLANGUAGE.Trim()=="06")
                                    Language = REntity.SiteLANGUAGE_OTHER.Trim();
                                else if (dtLang.Rows.Count > 0)
                                {
                                    foreach (DataRow drLang in dtLang.Rows)
                                    {
                                        if (REntity.SiteLANGUAGE.Trim() == drLang["Code"].ToString().Trim())
                                        {
                                            Language = drLang["LookUpDesc"].ToString().Trim();
                                            break;
                                        }
                                    }
                                }

                                PdfPCell Lang1 = new PdfPCell(new Phrase(Language.Trim(), TableFont));
                                Lang1.HorizontalAlignment = Element.ALIGN_LEFT;
                                Lang1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(Lang1);

                                PdfPCell Space2 = new PdfPCell(new Phrase("", TableFont));
                                Space2.HorizontalAlignment = Element.ALIGN_LEFT;
                                Space2.Colspan = 3;
                                Space2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(Space2);

                                //if (REntity.SiteOTHER_PHONE.Trim().Length == 10)
                                //    Phone_No = "(" + REntity.SiteOTHER_PHONE.Trim().Substring(0, 3) + ")" + " " + REntity.SiteOTHER_PHONE.Trim().Substring(3, 3) + "-" + REntity.SiteOTHER_PHONE.Trim().Substring(6, 4);
                                //else
                                //    Phone_No = REntity.SiteOTHER_PHONE.Trim();
                                MaskedTextBox mskPhnOther = new MaskedTextBox();
                                mskPhnOther.Mask = "(999) 000-0000";
                                if (!string.IsNullOrEmpty(REntity.SiteOTHER_PHONE.Trim()))
                                    mskPhnOther.Text = REntity.SiteOTHER_PHONE.Trim();
                                PdfPCell Other = new PdfPCell(new Phrase("Other " + mskPhnOther.Text.Trim(), TableFont));
                                Other.HorizontalAlignment = Element.ALIGN_LEFT;
                                Other.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                table.AddCell(Other);


                                PdfPCell Space_Row = new PdfPCell(new Phrase("", TableFont));
                                Space_Row.HorizontalAlignment = Element.ALIGN_LEFT;
                                Space_Row.Colspan = 10;
                                Space_Row.Border = iTextSharp.text.Rectangle.LEFT_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER;
                                table.AddCell(Space_Row);

                                string Fund = string.Empty;
                                if (!string.IsNullOrEmpty(REntity.SiteFUND1.Trim()))
                                {
                                    foreach (DataRow drFund in dtFund.Rows)
                                    {
                                        if (REntity.SiteFUND1.Trim() == drFund["Code"].ToString().Trim())
                                        {
                                            Fund += drFund["LookUpDesc"].ToString().Trim()+",";
                                            break;
                                        }
                                    }
                                }
                                else if (!string.IsNullOrEmpty(REntity.SiteFUND2.Trim()))
                                {
                                    foreach (DataRow drFund in dtFund.Rows)
                                    {
                                        if (REntity.SiteFUND2.Trim() == drFund["Code"].ToString().Trim())
                                        {
                                            Fund += drFund["LookUpDesc"].ToString().Trim() + ",";
                                            break;
                                        }
                                    }
                                }
                                else if (!string.IsNullOrEmpty(REntity.SiteFUND3.Trim())) 
                                {
                                    foreach (DataRow drFund in dtFund.Rows)
                                    {
                                        if (REntity.SiteFUND3.Trim() == drFund["Code"].ToString().Trim())
                                        {
                                            Fund += drFund["LookUpDesc"].ToString().Trim() + ",";
                                            break;
                                        }
                                    }
                                }
                                else if (!string.IsNullOrEmpty(REntity.SiteFUND4.Trim()))
                                {
                                    foreach (DataRow drFund in dtFund.Rows)
                                    {
                                        if (REntity.SiteFUND4.Trim() == drFund["Code"].ToString().Trim())
                                        {
                                            Fund += drFund["LookUpDesc"].ToString().Trim() + ",";
                                            break;
                                        }
                                    }
                                }
                                else if (!string.IsNullOrEmpty(REntity.SiteFUND5.Trim()))
                                {
                                    foreach (DataRow drFund in dtFund.Rows)
                                    {
                                        if (REntity.SiteFUND5.Trim() == drFund["Code"].ToString().Trim())
                                        {
                                            Fund += drFund["LookUpDesc"].ToString().Trim();
                                            break;
                                        }
                                    }
                                }

                                if (!string.IsNullOrEmpty(Fund))
                                {
                                    PdfPCell FundRow = new PdfPCell(new Phrase("Funds", TableFont));
                                    FundRow.HorizontalAlignment = Element.ALIGN_LEFT;
                                    FundRow.Colspan = 2;
                                    FundRow.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                    table.AddCell(FundRow);

                                    PdfPCell Funddesc = new PdfPCell(new Phrase(Fund.Trim(), TableFont));
                                    Funddesc.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Funddesc.Colspan = 8;
                                    Funddesc.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                    table.AddCell(Funddesc);
                                }
                                
                                string Mon = string.Empty, Tue = string.Empty, Wed = string.Empty, Thu = string.Empty, Fri = string.Empty, Sat = string.Empty, Sun = string.Empty;
                                string meals = REntity.SiteMEAL_AREA.Trim();
                                for (int i = 0; i < meals.Length; i++)
                                {
                                    if (meals.Substring(i, 1).ToString().Trim() == "Y")
                                    {
                                        switch (i)
                                        {
                                            case 0: Mon += "B"; break;
                                            case 1: Tue += "B"; break;
                                            case 2: Wed += "B"; break;
                                            case 3: Thu += "B"; break;
                                            case 4: Fri += "B"; break;
                                            case 5: Sat += "B"; break;
                                            case 6: Sun += "B"; break;
                                            case 7: Mon += "A"; break;
                                            case 8: Tue += "A"; break;
                                            case 9: Wed += "A"; break;
                                            case 10: Thu += "A"; break;
                                            case 11: Fri += "A"; break;
                                            case 12: Sat += "A"; break;
                                            case 13: Sun += "A"; break;
                                            case 14: Mon += "L"; break;
                                            case 15: Tue += "L"; break;
                                            case 16: Wed += "L"; break;
                                            case 17: Thu += "L"; break;
                                            case 18: Fri += "L"; break;
                                            case 19: Sat += "L"; break;
                                            case 20: Sun += "L"; break;
                                            case 21: Mon += "P"; break;
                                            case 22: Tue += "P"; break;
                                            case 23: Wed += "P"; break;
                                            case 24: Thu += "P"; break;
                                            case 25: Fri += "P"; break;
                                            case 26: Sat += "P"; break;
                                            case 27: Sun += "P"; break;
                                            case 28: Mon += "S"; break;
                                            case 29: Tue += "S"; break;
                                            case 30: Wed += "S"; break;
                                            case 31: Thu += "S"; break;
                                            case 32: Fri += "S"; break;
                                            case 33: Sat += "S"; break;
                                            case 34: Sun += "S"; break;
                                        }
                                        //if (i == 1 || i == 8 || i == 15 || i == 22 || i == 29)
                                        //{
                                        //    if (i == 1) Mon += "B";
                                        //    else if (i == 8) Mon += "A";
                                        //    else if (i == 15) Mon += "L";
                                        //    else if (i == 22) Mon += "P";
                                        //    else if (i == 29) Mon += "S";
                                        //}
                                        //else if (i == 2 || i == 9 || i == 16 || i == 23 || i == 30)
                                        //{
                                        //    if (i == 2) Tue += "B";
                                        //    else if (i == 9) Tue += "A";
                                        //    else if (i == 16) Tue += "L";
                                        //    else if (i == 23) Tue += "P";
                                        //    else if (i == 30) Tue += "S";
                                        //}
                                        //else if (i == 3 || i == 10 || i == 17 || i == 24 || i == 31)
                                        //{
                                        //    if (i == 3) Wed += "B";
                                        //    else if (i == 10) Wed += "A";
                                        //    else if (i == 17) Wed += "L";
                                        //    else if (i == 24) Wed += "P";
                                        //    else if (i == 31) Wed += "S";
                                        //}
                                        //else if (i == 4 || i == 11 || i == 18 || i == 25 || i == 32)
                                        //{
                                        //    if (i == 4) Wed += "B";
                                        //    else if (i == 11) Wed += "A";
                                        //    else if (i == 18) Wed += "L";
                                        //    else if (i == 25) Wed += "P";
                                        //    else if (i == 32) Wed += "S";
                                        //}

                                    }
                                }

                                PdfPCell Space_Row1 = new PdfPCell(new Phrase("", TableFont));
                                Space_Row1.HorizontalAlignment = Element.ALIGN_LEFT;
                                Space_Row1.Colspan = 10;
                                Space_Row1.Border = iTextSharp.text.Rectangle.LEFT_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER;
                                table.AddCell(Space_Row1);

                                PdfPCell Meals = new PdfPCell(new Phrase("Meals:", TableFont));
                                Meals.HorizontalAlignment = Element.ALIGN_LEFT;
                                Meals.Colspan = 3;
                                Meals.Border = iTextSharp.text.Rectangle.LEFT_BORDER ;
                                table.AddCell(Meals);

                                if (string.IsNullOrEmpty(Mon.Trim())) Mon = "None";
                                if (string.IsNullOrEmpty(Tue.Trim())) Tue = "None"; if (string.IsNullOrEmpty(Wed.Trim())) Wed = "None";
                                if (string.IsNullOrEmpty(Thu.Trim())) Thu = "None"; if (string.IsNullOrEmpty(Fri.Trim())) Fri = "None";
                                if (string.IsNullOrEmpty(Sat.Trim())) Sat = "None"; if (string.IsNullOrEmpty(Sun.Trim())) Sun = "None";

                                PdfPCell Weeks = new PdfPCell(new Phrase("Mon: " + Mon.Trim() + "       " + "Tue: " + Tue.Trim() + "       " + "Wed: " + Wed.Trim() + "       " + "Thu: " + Thu.Trim() + "       " + "Fri: " + Fri.Trim() + "       " + "Sat: " + Sat.Trim() + "       " + "Sun: " + Sun.Trim(), TableFont));
                                Weeks.HorizontalAlignment = Element.ALIGN_LEFT;
                                Weeks.Colspan = 7;
                                Weeks.Border = iTextSharp.text.Rectangle.RIGHT_BORDER ;
                                table.AddCell(Weeks);

                                PdfPCell Empty = new PdfPCell(new Phrase("", TableFont));
                                Empty.HorizontalAlignment = Element.ALIGN_LEFT;
                                Empty.Colspan = 10;
                                Empty.FixedHeight = 10f;
                                Empty.Border = iTextSharp.text.Rectangle.LEFT_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER + iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                table.AddCell(Empty);

                                PdfPCell Empty1 = new PdfPCell(new Phrase("", TableFont));
                                Empty1.HorizontalAlignment = Element.ALIGN_LEFT;
                                Empty1.Colspan = 10;
                                Empty1.FixedHeight = 20f;
                                Empty1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(Empty1);

                            }
                        }
                    }

                    document.Add(table);
                }
                document.Close();
                fs.Close();
                fs.Dispose();
                AlertBox.Show("Report Generated Successfully");
            }
        }

        private void btnGenPDF_Click(object sender, EventArgs e)
        {
            if ((Agency != "**" || Depart != "**" || Program != "**") && CmbYear.Visible == false)
                AlertBox.Show("This Agency/Program having no Year, so unable to generate Report", MessageBoxIcon.Warning);
            else
            {
                if (rbSchedule.Checked.Equals(true))
                {
                    if (ValidateForm())
                    {
                        if (rbSelectedSite.Checked.Equals(true))
                        {
                            if (Sel_REFS_List.Count > 0)
                            {
                                PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, false, propReportPath,"PDF");
                                pdfListForm.FormClosed += new FormClosedEventHandler(On_SaveForm_Schdule);
                                pdfListForm.StartPosition = FormStartPosition.CenterScreen;
                                pdfListForm.ShowDialog();
                            }
                            else
                            {
                                AlertBox.Show("You must select at least One site or choose All sites", MessageBoxIcon.Warning);
                            }
                        }
                        else
                        {
                            PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, false, propReportPath, "PDF");
                            pdfListForm.FormClosed += new FormClosedEventHandler(On_SaveForm_Schdule);
                            pdfListForm.StartPosition = FormStartPosition.CenterScreen;
                            pdfListForm.ShowDialog();
                        }
                    }
                }
                if (rbClass.Checked.Equals(true))
                {
                    PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, false, propReportPath, "PDF");
                    pdfListForm.FormClosed += new FormClosedEventHandler(On_SaveForm_Closed);
                    pdfListForm.StartPosition = FormStartPosition.CenterScreen;
                    pdfListForm.ShowDialog();
                }
            }
            //if (CmbYear.Visible == false)
            //    MessageBox.Show("This Agency/Program having no Year so unable to generate Report", "CAP Systems");
        }

        private bool ValidateForm()
        {
            bool isValid = true;
            if (string.IsNullOrEmpty(txtStrtMnth.Text.Trim()) || string.IsNullOrWhiteSpace(txtStrtMnth.Text.Trim()))
            {
                _errorProvider.SetError(txtStrtMnth, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblStrtMonth.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else if (int.Parse(txtStrtMnth.Text.Trim()) < 1)
            {
                _errorProvider.SetError(txtStrtMnth, "Enter Correct Month value".Replace(Consts.Common.Colon, string.Empty));
                isValid = false;
            }else
                _errorProvider.SetError(txtStrtMnth, null);

            if (string.IsNullOrEmpty(txtEndMnth.Text.Trim()) || string.IsNullOrWhiteSpace(txtEndMnth.Text.Trim()))
            {
                _errorProvider.SetError(txtEndMnth, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblEndMonth.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else if (int.Parse(txtEndMnth.Text.Trim()) < 1)
            {
                _errorProvider.SetError(txtEndMnth, "Enter Correct Month value".Replace(Consts.Common.Colon, string.Empty));
                isValid = false;
            }
            else
                _errorProvider.SetError(txtEndMnth, null);

            if (string.IsNullOrEmpty(txtStrtYear.Text.Trim()) || string.IsNullOrWhiteSpace(txtStrtYear.Text.Trim()))
            {
                _errorProvider.SetError(txtStrtYear, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblStartYear.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(txtStrtYear, null);

            if (string.IsNullOrEmpty(txtEndYear.Text.Trim()) || string.IsNullOrWhiteSpace(txtEndYear.Text.Trim()))
            {
                _errorProvider.SetError(txtEndYear, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblEndYear.Text.Replace(Consts.Common.Colon, string.Empty)));
                isValid = false;
            }
            else
                _errorProvider.SetError(txtEndYear, null);

            if (!string.IsNullOrEmpty(txtEndYear.Text.Trim()) && !string.IsNullOrEmpty(txtStrtYear.Text.Trim()))
            {
                if (int.Parse(txtStrtYear.Text.Trim()) > int.Parse(txtEndYear.Text.Trim()))
                {
                    _errorProvider.SetError(txtEndYear, "End Month must be Greater than Start Month, fill the Values correctly".Replace(Consts.Common.Colon, string.Empty));
                    isValid = false;
                }
                else
                    _errorProvider.SetError(txtEndYear, null);
            }
            if (!string.IsNullOrEmpty(txtEndYear.Text.Trim()) || !string.IsNullOrEmpty(txtStrtYear.Text.Trim()))
            {
                if (txtEndYear.Text.Trim() == txtStrtYear.Text.Trim())
                {
                    if (string.IsNullOrEmpty(txtStrtMnth.Text.Trim()) || !string.IsNullOrEmpty(txtEndMnth.Text.Trim()))
                    {
                        if (int.Parse(txtStrtMnth.Text.Trim()) > 0 && int.Parse(txtEndMnth.Text.Trim()) > 0)
                        {
                            if (int.Parse(txtStrtMnth.Text.Trim()) > int.Parse(txtEndMnth.Text.Trim()))
                            {
                                _errorProvider.SetError(txtEndMnth, "End Month must be Greater than Start Month".Replace(Consts.Common.Colon, string.Empty));
                                isValid = false;
                            }
                            else
                                _errorProvider.SetError(txtEndMnth, null);
                        }
                    }
                }
            }
           
            IsSaveValid = isValid;
            return (isValid);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, true, propReportPath);
            pdfListForm.StartPosition = FormStartPosition.CenterScreen;
            pdfListForm.ShowDialog();
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

            PdfPCell Hierarchy = new PdfPCell(new Phrase("  " + "Hierarchy", TableFont));
            Hierarchy.HorizontalAlignment = Element.ALIGN_LEFT;
            Hierarchy.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            Hierarchy.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            Hierarchy.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            Hierarchy.PaddingBottom = 5;
            Headertable.AddCell(Hierarchy);

            if (CmbYear.Visible == true)
            {
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
                PdfPCell Hierarchy1 = new PdfPCell(new Phrase(Txt_HieDesc.Text.Trim(), TableFont));
                Hierarchy1.HorizontalAlignment = Element.ALIGN_LEFT;
                Hierarchy1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                Hierarchy1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                Hierarchy1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                Hierarchy1.PaddingBottom = 5;
                Headertable.AddCell(Hierarchy1);
            }

            //string Report = string.Empty; string TypeRep = string.Empty;
            //if (rbClass.Checked.Equals(true)) Report = "Site Class"; else Report = "Site Schedule";
            PdfPCell R1 = new PdfPCell(new Phrase("  " + "Report" /*+ (rbClass.Checked == true ? rbClass.Text : rbSchedule.Text)*/, TableFont));
            R1.HorizontalAlignment = Element.ALIGN_LEFT;
            R1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R1.PaddingBottom = 5;
            Headertable.AddCell(R1);

            PdfPCell R11 = new PdfPCell(new Phrase((rbClass.Checked == true ? rbClass.Text : rbSchedule.Text), TableFont));
            R11.HorizontalAlignment = Element.ALIGN_LEFT;
            R11.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R11.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R11.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R11.PaddingBottom = 5;
            Headertable.AddCell(R11);

            //if (rbFull.Checked.Equals(true)) TypeRep = "Full"; else TypeRep = "Condensed";
            PdfPCell R2 = new PdfPCell(new Phrase("  " + "Type of Report" /*+ (rbFull.Checked == true ? rbFull.Text : rbCondensed.Text)*/ , TableFont));
            R2.HorizontalAlignment = Element.ALIGN_LEFT;
            R2.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            R2.PaddingBottom = 5;
            Headertable.AddCell(R2);

            PdfPCell R22 = new PdfPCell(new Phrase((rbFull.Checked == true ? rbFull.Text : rbCondensed.Text), TableFont));
            R22.HorizontalAlignment = Element.ALIGN_LEFT;
            R22.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            R22.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            R22.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            R22.PaddingBottom = 5;
            Headertable.AddCell(R22);

            if (rbSchedule.Checked == true)
            {
                string Site = string.Empty;
                if (rbAll.Checked == true) Site = "All Sites";
                else
                {
                    string Selsites = string.Empty;
                    if (Sel_REFS_List.Count > 0)
                    {
                        foreach (CaseSiteEntity Entity in Sel_REFS_List)
                        {
                            Selsites += Entity.SiteNUMBER + "/" + Entity.SiteROOM + "/" + Entity.SiteAM_PM + "  ";
                        }
                    }
                    Site = rbSelectedSite.Text.Trim() + " ( " + Selsites + " )";
                }
                PdfPCell R3 = new PdfPCell(new Phrase("  " + "Site", TableFont));
                R3.HorizontalAlignment = Element.ALIGN_LEFT;
                R3.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                R3.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                R3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                R3.PaddingBottom = 5;
                Headertable.AddCell(R3);

                PdfPCell R33 = new PdfPCell(new Phrase(Site, TableFont));
                R33.HorizontalAlignment = Element.ALIGN_LEFT;
                R33.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                R33.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                R33.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                R33.PaddingBottom = 5;
                Headertable.AddCell(R33);

                PdfPCell R4 = new PdfPCell(new Phrase("", TableFont));
                R4.HorizontalAlignment = Element.ALIGN_LEFT;
                R4.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                R4.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                R4.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                R4.PaddingBottom = 5;
                Headertable.AddCell(R4);

                PdfPCell R44 = new PdfPCell(new Phrase("From Month: " + txtStrtMnth.Text.Trim() + "   Year: " + txtStrtYear.Text.Trim() + "      " + "To Month: " + txtEndMnth.Text.Trim() + "   Year: " + txtEndYear.Text.Trim(), TableFont));
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
            //cb.BeginText();

            //cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ROMAN).BaseFont, 12);
            ////cb.SetRGBColorFill(00, 00, 00);      //ss
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
            //string Report = string.Empty; string TypeRep = string.Empty;
            //if (rbClass.Checked.Equals(true)) Report = "Site Class"; else Report = "Site Schedule";
            //if (rbFull.Checked.Equals(true)) TypeRep = "Full"; else TypeRep = "Condensed";
            //cb.ShowTextAligned(100, "Report : " + Report, 120, 590, 0);
            //cb.ShowTextAligned(100, "Report Type :"+TypeRep, 120, 570, 0);
            //if(rbSchedule.Checked==true)
            //    cb.ShowTextAligned(100, "From Month :" + txtStrtMnth.Text.Trim() + "  Year: " + txtStrtYear.Text.Trim() + "      " + "To Month :" + txtEndMnth.Text.Trim() + "    Year: " + txtEndYear.Text.Trim(), 120, 550, 0);

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

        private void rbSchedule_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSchedule.Checked.Equals(true))
            {
                this.Size = new System.Drawing.Size(750, 271);
                //this.panel5.Location = new System.Drawing.Point(0, 145);
                pnlSite.Visible = true;
                if (Agency != "**" || Depart != "**" || Program != "**")
                {
                    ProgramDefinitionEntity programDefinitionList = _model.HierarchyAndPrograms.GetCaseDepadp(Agency.Trim(), Depart.Trim(), Program.Trim());
                    if (!string.IsNullOrEmpty(programDefinitionList.StartMonth.Trim()) && !string.IsNullOrEmpty(programDefinitionList.StartMonth.Trim()))
                    {
                        Startmonth = int.Parse(programDefinitionList.StartMonth.Trim()); endMonth = int.Parse(programDefinitionList.EndMonth.Trim());
                    }
                    if (rbSchedule.Checked.Equals(true))
                    {
                        if ((!string.IsNullOrEmpty(programDefinitionList.StartMonth.Trim()) && !string.IsNullOrEmpty(programDefinitionList.EndMonth.Trim())) || (Startmonth != 0 && endMonth != 0))
                        {
                            if (programDefinitionList.StartMonth.Trim() == "0")
                                txtStrtMnth.Text = "";
                            else
                                txtStrtMnth.Text = programDefinitionList.StartMonth.Trim();
                            if (programDefinitionList.EndMonth.Trim() == "0")
                                txtEndMnth.Text = "";
                            else
                                txtEndMnth.Text = programDefinitionList.EndMonth.Trim();
                            if (Startmonth != 0 && endMonth != 0)
                            {
                                if (programDefinitionList.StartMonth.Trim() != "")
                                { 
                                    if (int.Parse(programDefinitionList.StartMonth.Trim()) < 12)
                                        txtStrtYear.Text = programDefinitionList.DepYear.Trim();
                                    else txtStrtYear.Text = (int.Parse(programDefinitionList.DepYear.Trim()) + 1).ToString();


                                    if (int.Parse(programDefinitionList.EndMonth.Trim()) > int.Parse(programDefinitionList.StartMonth.Trim()))
                                        txtEndYear.Text = programDefinitionList.DepYear.Trim();
                                    else txtEndYear.Text = (int.Parse(programDefinitionList.DepYear.Trim()) + 1).ToString();
                                }
                            }
                            else
                            {
                                txtStrtYear.Text = "";
                                txtEndYear.Text = "";
                            }
                        }
                    }
                }
            }
            else
            {
                _errorProvider.SetError(txtEndMnth, null);
                _errorProvider.SetError(txtStrtMnth, null);
                _errorProvider.SetError(txtStrtYear, null);
                _errorProvider.SetError(txtEndYear, null);
                rbAll.Checked = true;
                Sel_REFS_List.Clear();
                this.Size = new System.Drawing.Size(750, 205);
                //this.panel5.Location = new System.Drawing.Point(0, 95);
                pnlSite.Visible = false;   
            }
        }

        private void On_SaveForm_Schdule(object sender, EventArgs e)
        {
            PdfListForm form = sender as PdfListForm;
            if (form.DialogResult == DialogResult.OK)
            {
                string PdfName = "Pdf File";
                PdfName = form.GetFileName();
                //PdfName = strFolderPath + PdfName;
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

                Document document = new Document(PageSize.A4, 30f, 30f, 30f, 50f);
                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                document.Open();
                BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, false);
                BaseFont bf_Times = BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1250, false);
                iTextSharp.text.Font fc = new iTextSharp.text.Font(bfTimes, 10, 2);
                iTextSharp.text.Font fc1 = new iTextSharp.text.Font(bf_Times, 10, 2, BaseColor.BLUE);
                iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bfTimes, 10);
                iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_Times, 10);
                cb = writer.DirectContent;

                PrintHeaderPage(document, writer);
                GetHeadstartTemplate_Values();
                //List<SiteScheduleEntity> Selected_SchduleList = new List<SiteScheduleEntity>();
                List<SiteScheduleEntity> SchduleList = new List<SiteScheduleEntity>();
                List<SiteScheduleEntity> MasterSchd = new List<SiteScheduleEntity>();
                SiteScheduleEntity site_Search = new SiteScheduleEntity(true);
                site_Search.ATTM_AGENCY = Agency; site_Search.ATTM_DEPT = Depart;
                site_Search.ATTM_PROG = Program; 
                if(CmbYear.Visible==true)
                    site_Search.ATTM_YEAR = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString().Trim();
                if(Agency!="**")
                    MasterSchd = _model.SPAdminData.Browse_CHILDATTM(site_Search, "Browse");
                List<ReportSiteEntity> Site_list = new List<ReportSiteEntity>();

                CaseSiteEntity Search_Entity = new CaseSiteEntity(true);
                Search_Entity.SiteAGENCY = Agency; Search_Entity.SiteDEPT = Depart;
                Search_Entity.SitePROG = Program;
                string Year_value=string.Empty;
                if (CmbYear.Visible == true)
                    Year_value = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString().Trim();
                else Year_value = null;
                Site_list = _model.CaseMstData.HSSB2110_Report(Agency, Depart, Program, Year_value);

                if (BaseForm.BaseAgencyControlDetails.SiteSecurity == "1")
                {
                    List<HierarchyEntity> userHierarchy = _model.UserProfileAccess.GetUserHierarchyByID(BaseForm.UserID);
                    HierarchyEntity hierarchyEntity = new HierarchyEntity();
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
                            { hierarchyEntity = null; }
                        }
                    }

                    if (hierarchyEntity != null)
                    {
                        if (hierarchyEntity.Sites.Length > 0)
                        {
                            string[] Sites = hierarchyEntity.Sites.Split(',');
                            List<ReportSiteEntity> Selsites = new List<ReportSiteEntity>();
                            for (int i = 0; i < Sites.Length; i++)
                            {
                                if (!string.IsNullOrEmpty(Sites[i].ToString().Trim()))
                                {
                                    foreach (ReportSiteEntity casesite in Site_list) //Site_List)//ListcaseSiteEntity)
                                    {
                                        if (Sites[i].ToString() == casesite.SiteNUMBER && casesite.SiteROOM != "0000")
                                        {
                                            Selsites.Add((casesite));
                                            //break;
                                        }
                                        // Sel_Site_Codes += "'" + casesite.SiteNUMBER + "' ,";
                                    }
                                }
                            }
                            Site_list = Selsites;
                            //strsiteRoomNames = hierarchyEntity.Sites;
                        }
                    }
                }

                //if (((Captain.Common.Utilities.ListItem)cmbActive.SelectedItem).Value.ToString().Trim() == "A")
                //    Site_list = Site_list.FindAll(u => u.SITEACTIVE.Equals("Y"));
                //else if (((Captain.Common.Utilities.ListItem)cmbActive.SelectedItem).Value.ToString().Trim() == "I")
                //    Site_list = Site_list.FindAll(u => u.SITEACTIVE.Equals("N"));

                NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;

                if (rbAll.Checked.Equals(true))
                {
                    if (rbCondensed.Checked.Equals(true))
                    {
                        document.NewPage();

                        PdfPTable table = new PdfPTable(7);
                        table.TotalWidth = 550f;
                        table.WidthPercentage = 100;
                        table.LockedWidth = true;
                        float[] widths = new float[] { 20f, 15f, 20f, 60f, 34f, 88f, 28f };
                        table.SetWidths(widths);
                        table.HorizontalAlignment = Element.ALIGN_CENTER;

                        string[] header1 = { "Site", "Room", "AM/PM", "Name", "Telephone", "Address", "Funded Days" };
                        for (int head_Rows = 0; head_Rows < header1.Length; head_Rows++)
                        {
                            PdfPCell day = new PdfPCell(new Phrase(header1[head_Rows], TblFontBold));
                            day.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            day.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            day.BackgroundColor = BaseColor.LIGHT_GRAY;
                            table.AddCell(day);
                        }

                        string Priv_Site = null; string Priv_SName = string.Empty; //bool First = false;
                        //int Total = 0; 

                        int fundCnt = 0; string PrivSite = string.Empty; int TotFund = 0; bool FundFirst = true;
                        List<CommonEntity> Fundedlist = new List<CommonEntity>();
                        string PrivSitenum = string.Empty;
                        string PrivSiteName = string.Empty;
                        int SiteCnt = 0;
                        if (Site_list.Count > 0)
                        {
                            int FundedDays = 0;
                            string Site_main = string.Empty, Site_main_Name = string.Empty; //int FundDays = 0;
                            foreach (ReportSiteEntity Entity in Site_list)
                            {
                                FundedDays = 0; SiteCnt++;
                                string Stie_AMPM = string.Empty;
                                if (Entity.SiteROOM != "0000")
                                {
                                    PdfPCell Site = new PdfPCell(new Phrase(Entity.SiteNUMBER.Trim(), TableFont));
                                    Site.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Site.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(Site);

                                    PdfPCell Room = new PdfPCell(new Phrase(Entity.SiteROOM.Trim(), TableFont));
                                    Room.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Room.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(Room);

                                    if (Entity.SiteAM_PM == "A")
                                        Stie_AMPM = "AM";
                                    else if (Entity.SiteAM_PM == "P")
                                        Stie_AMPM = "PM";
                                    else if (Entity.SiteAM_PM == "F")
                                        Stie_AMPM = "FULL";
                                    else if (Entity.SiteAM_PM == "E")
                                        Stie_AMPM = "EXTD";
                                    PdfPCell AM = new PdfPCell(new Phrase(Stie_AMPM, TableFont));
                                    AM.HorizontalAlignment = Element.ALIGN_LEFT;
                                    AM.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(AM);

                                    PdfPCell Name = new PdfPCell(new Phrase(Entity.SiteNAME.Trim(), TableFont));
                                    Name.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Name.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(Name);

                                    MaskedTextBox mskPhn = new MaskedTextBox();
                                    mskPhn.Mask = "(999) 000-0000";
                                    if (!string.IsNullOrEmpty(Entity.SitePHONE.Trim()))
                                        mskPhn.Text = Entity.SitePHONE.Trim();

                                    PdfPCell Phone = new PdfPCell(new Phrase(mskPhn.Text.Trim(), TableFont));
                                    Phone.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Phone.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(Phone);

                                    PdfPCell Address = new PdfPCell(new Phrase(Entity.SiteSTREET.Trim() + " " + Entity.SiteCITY.Trim() + " " + Entity.SiteSTATE.Trim() + " " + Entity.SiteZIP, TableFont));
                                    Address.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Address.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(Address);
                                    int Count = 0; int Cnt = 0;

                                    site_Search.ATTM_AGENCY = Entity.SiteAGENCY.Trim(); site_Search.ATTM_DEPT = Entity.SiteDEPT.Trim();
                                    site_Search.ATTM_PROG = Entity.SitePROG.Trim(); site_Search.ATTM_YEAR = Entity.SiteYEAR.Trim();
                                    site_Search.ATTM_SITE = Entity.SiteNUMBER.Trim(); //site_Search.ATTM_ROOM = REntity.SiteROOM.Trim();
                                    //site_Search.ATTM_AMPM = REntity.SiteAM_PM.Trim();
                                    SchduleList = _model.SPAdminData.Browse_CHILDATTM(site_Search, "Browse");
                                    if (SchduleList.Count > 0)
                                    {
                                        int length = 0;
                                        if (txtEndYear.Text.Trim() == txtStrtYear.Text.Trim())
                                            length = int.Parse(txtEndMnth.Text.Trim()) - int.Parse(txtStrtMnth.Text.Trim()) + 1;
                                        else
                                        {
                                            int temp = 0;
                                            temp = 12 - int.Parse(txtStrtMnth.Text.Trim()) + 1;
                                            length = int.Parse(txtEndMnth.Text.Trim()) + temp;
                                        }
                                        int j = int.Parse(txtStrtMnth.Text.Trim()); int StrtYear = int.Parse(txtStrtYear.Text.Trim());
                                        SiteScheduleEntity Selected_SchduleList = new SiteScheduleEntity();
                                        for (int i = 1; i <= length; i++)
                                        {
                                            Selected_SchduleList = SchduleList.Find(u => u.ATTM_ROOM.Trim().Equals(Entity.SiteROOM.Trim()) && u.ATTM_AMPM.Trim().Equals(Entity.SiteAM_PM.Trim()) && u.ATTM_MONTH.Trim().Equals(j.ToString()) && u.ATTM_FUND.Trim().Equals("") && u.ATTM_CALENDER_YEAR.Equals(StrtYear.ToString()));
                                            if (Selected_SchduleList != null)
                                            {
                                            }
                                            else
                                            {
                                                Selected_SchduleList = SchduleList.Find(u => u.ATTM_ROOM.Trim().Equals("") && u.ATTM_AMPM.Trim().Equals("") && u.ATTM_MONTH.Trim().Equals(j.ToString()) && u.ATTM_FUND.Trim().Equals("") && u.ATTM_CALENDER_YEAR.Equals(StrtYear.ToString()));
                                                if (Selected_SchduleList != null)
                                                {
                                                }
                                                else
                                                {
                                                    SiteScheduleEntity Site_MonthSearch = new SiteScheduleEntity(true);
                                                    List<SiteScheduleEntity> Sel_MonthSite = new List<SiteScheduleEntity>();
                                                    Site_MonthSearch.ATTM_AGENCY = Entity.SiteAGENCY.Trim(); Site_MonthSearch.ATTM_DEPT = Entity.SiteDEPT.Trim();
                                                    Site_MonthSearch.ATTM_PROG = Entity.SitePROG.Trim(); Site_MonthSearch.ATTM_YEAR = Entity.SiteYEAR.Trim();
                                                    Site_MonthSearch.ATTM_MONTH = j.ToString(); Site_MonthSearch.ATTM_CALENDER_YEAR = StrtYear.ToString();//site_Search.ATTM_SITE = "    "; site_Search.ATTM_ROOM = "    ";
                                                    Sel_MonthSite = _model.SPAdminData.Browse_CHILDATTM(Site_MonthSearch, "Browse");
                                                    //MasterSchd = _model.SPAdminData.Browse_CHILDATTM(site_Search, "Browse");
                                                    //Selected_SchduleList = MasterSchd.Find(u => u.ATTM_SITE.Trim().Equals("") && u.ATTM_ROOM.Trim().Equals("") && u.ATTM_MONTH.Trim().Equals(j.ToString()));
                                                    Selected_SchduleList = Sel_MonthSite.Find(u => u.ATTM_SITE.Trim().Equals("") && u.ATTM_ROOM.Trim().Equals("") && u.ATTM_FUND.Trim().Equals(""));
                                                    if (Selected_SchduleList != null)
                                                    {
                                                    }
                                                }
                                            }

                                            if (Selected_SchduleList != null)
                                            {
                                                List<ChildATTMSEntity> Month_List = new List<ChildATTMSEntity>();
                                                ChildATTMSEntity Search_Month = new ChildATTMSEntity(true);
                                                Search_Month.ATTMS_ID = Selected_SchduleList.ATTM_ID.Trim();
                                                Month_List = _model.SPAdminData.Browse_CHILDATTMS(Search_Month, "Browse");
                                                if (Month_List.Count > 0)
                                                {

                                                    DateTime firstday = new DateTime(Convert.ToInt32(Selected_SchduleList.ATTM_CALENDER_YEAR.Trim()), Convert.ToInt32(Selected_SchduleList.ATTM_MONTH.Trim()), 1);
                                                    DateTime end_Month = new DateTime(Convert.ToInt32(Selected_SchduleList.ATTM_CALENDER_YEAR.Trim()), Convert.ToInt32(Selected_SchduleList.ATTM_MONTH.Trim()), DateTime.DaysInMonth(Convert.ToInt32(Selected_SchduleList.ATTM_CALENDER_YEAR.Trim()), Convert.ToInt32(Selected_SchduleList.ATTM_MONTH.Trim())));
                                                    string Strtday = firstday.DayOfWeek.ToString();
                                                    string Endday = end_Month.DayOfWeek.ToString();

                                                    foreach (ChildATTMSEntity AEntity in Month_List)
                                                    {
                                                        string Status = string.Empty;
                                                        string Stat_Extn = string.Empty;
                                                        if (Template_List.Count > 0)
                                                        {
                                                            foreach (Headstart_Template HEntity in Template_List)
                                                            {
                                                                if (HEntity.Code_Desc_Tag.Trim() == AEntity.ATTMS_STATUS.Trim())
                                                                {
                                                                    Status = HEntity.Code_Desc.Trim();
                                                                    if (Entity.SiteNUMBER.Trim() != PrivSite)
                                                                    {

                                                                        if (!FundFirst)
                                                                        {
                                                                            Fundedlist.Add(new CommonEntity(PrivSitenum, PrivSiteName, fundCnt.ToString()));
                                                                            TotFund = TotFund + fundCnt;
                                                                            fundCnt = 0;
                                                                        }
                                                                        FundFirst = false;
                                                                        PrivSite = Entity.SiteNUMBER.Trim();
                                                                        PrivSitenum = Entity.SiteNUMBER.Trim();
                                                                        PrivSiteName = Entity.SiteNAME.Trim();
                                                                    }
                                                                    //if (AEntity.ATTMS_STATUS.Trim() == "O")
                                                                    //    fundCnt++;
                                                                    if (!string.IsNullOrEmpty(HEntity.Agy_2.Trim()))
                                                                    {
                                                                        Stat_Extn = " (" + HEntity.Agy_2.Trim() + ")";
                                                                        fundCnt++; FundedDays++;
                                                                    }
                                                                    break;
                                                                }
                                                            }

                                                        }
                                                    }
                                                }
                                            }
                                            j++;
                                            if (j > 12) { j = 1; StrtYear = StrtYear + 1; }
                                        }

                                        PdfPCell Funds = new PdfPCell(new Phrase(FundedDays.ToString(), TableFont));
                                        Funds.HorizontalAlignment = Element.ALIGN_CENTER;
                                        Funds.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(Funds);
                                        //document.NewPage();
                                    }
                                    else
                                    {
                                        int length = 0;
                                        if (txtEndYear.Text.Trim() == txtStrtYear.Text.Trim())
                                            length = int.Parse(txtEndMnth.Text.Trim()) - int.Parse(txtStrtMnth.Text.Trim()) + 1;
                                        else
                                        {
                                            int temp = 0;
                                            temp = 12 - int.Parse(txtStrtMnth.Text.Trim()) + 1;
                                            length = int.Parse(txtEndMnth.Text.Trim()) + temp;
                                        }
                                        int j = int.Parse(txtStrtMnth.Text.Trim()); int StrtYear = int.Parse(txtStrtYear.Text.Trim());
                                        SiteScheduleEntity Selected_SchduleList = new SiteScheduleEntity();
                                        for (int i = 1; i <= length; i++)
                                        {
                                            SiteScheduleEntity Site_MonthSearch = new SiteScheduleEntity(true);
                                            List<SiteScheduleEntity> Sel_MonthSite = new List<SiteScheduleEntity>();
                                            Site_MonthSearch.ATTM_AGENCY = Entity.SiteAGENCY.Trim(); Site_MonthSearch.ATTM_DEPT = Entity.SiteDEPT.Trim();
                                            Site_MonthSearch.ATTM_PROG = Entity.SitePROG.Trim(); Site_MonthSearch.ATTM_YEAR = Entity.SiteYEAR.Trim();
                                            Site_MonthSearch.ATTM_MONTH = j.ToString(); Site_MonthSearch.ATTM_CALENDER_YEAR = StrtYear.ToString(); //site_Search.ATTM_SITE = "    "; site_Search.ATTM_ROOM = "    ";
                                            Sel_MonthSite = _model.SPAdminData.Browse_CHILDATTM(Site_MonthSearch, "Browse");
                                            Selected_SchduleList = Sel_MonthSite.Find(u => u.ATTM_SITE.Trim().Equals("") && u.ATTM_ROOM.Trim().Equals("") && u.ATTM_FUND.Trim().Equals(""));

                                            if (Selected_SchduleList != null)
                                            {
                                                List<ChildATTMSEntity> Month_List = new List<ChildATTMSEntity>();
                                                ChildATTMSEntity Search_Month = new ChildATTMSEntity(true);
                                                Search_Month.ATTMS_ID = Selected_SchduleList.ATTM_ID.Trim();
                                                Month_List = _model.SPAdminData.Browse_CHILDATTMS(Search_Month, "Browse");
                                                if (Month_List.Count > 0)
                                                {

                                                    DateTime firstday = new DateTime(Convert.ToInt32(Selected_SchduleList.ATTM_CALENDER_YEAR.Trim()), Convert.ToInt32(Selected_SchduleList.ATTM_MONTH.Trim()), 1);
                                                    DateTime end_Month = new DateTime(Convert.ToInt32(Selected_SchduleList.ATTM_CALENDER_YEAR.Trim()), Convert.ToInt32(Selected_SchduleList.ATTM_MONTH.Trim()), DateTime.DaysInMonth(Convert.ToInt32(Selected_SchduleList.ATTM_CALENDER_YEAR.Trim()), Convert.ToInt32(Selected_SchduleList.ATTM_MONTH.Trim())));
                                                    string Strtday = firstday.DayOfWeek.ToString();
                                                    string Endday = end_Month.DayOfWeek.ToString();

                                                    foreach (ChildATTMSEntity AEntity in Month_List)
                                                    {
                                                        string Status = string.Empty;
                                                        string Stat_Extn = string.Empty;
                                                        if (Template_List.Count > 0)
                                                        {
                                                            foreach (Headstart_Template HEntity in Template_List)
                                                            {
                                                                if (HEntity.Code_Desc_Tag.Trim() == AEntity.ATTMS_STATUS.Trim())
                                                                {
                                                                    Status = HEntity.Code_Desc.Trim();
                                                                    if (Entity.SiteNUMBER.Trim() != PrivSite)
                                                                    {

                                                                        if (!FundFirst)
                                                                        {
                                                                            Fundedlist.Add(new CommonEntity(PrivSitenum, PrivSiteName, fundCnt.ToString()));
                                                                            TotFund = TotFund + fundCnt;
                                                                            fundCnt = 0;
                                                                        }
                                                                        FundFirst = false;
                                                                        PrivSite = Entity.SiteNUMBER.Trim();
                                                                        PrivSitenum = Entity.SiteNUMBER.Trim();
                                                                        PrivSiteName = Entity.SiteNAME.Trim();
                                                                    }
                                                                    //if (AEntity.ATTMS_STATUS.Trim() == "O")
                                                                    //    fundCnt++;
                                                                    if (!string.IsNullOrEmpty(HEntity.Agy_2.Trim()))
                                                                    {
                                                                        Stat_Extn = " (" + HEntity.Agy_2.Trim() + ")";
                                                                        fundCnt++; FundedDays++;
                                                                    }
                                                                    break;
                                                                }
                                                            }

                                                        }
                                                    }
                                                }
                                            }

                                            j++;
                                            if (j > 12) { j = 1; StrtYear = StrtYear + 1; }
                                       
                                        }
                                        PdfPCell Funds = new PdfPCell(new Phrase(FundedDays.ToString(), TableFont));
                                        Funds.HorizontalAlignment = Element.ALIGN_CENTER;
                                        Funds.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(Funds);
                                }

                                    if (Site_list.Count == SiteCnt)
                                    {
                                        Fundedlist.Add(new CommonEntity(Entity.SiteNUMBER.Trim(), Entity.SiteNAME.Trim(), fundCnt.ToString()));
                                        TotFund = TotFund + fundCnt;
                                    }
                                }
                            }
                            if (table.Rows.Count > 0)
                            {
                                document.Add(table);
                                //table.DeleteBodyRows();
                            }

                            document.NewPage();
                            PdfPTable Funding = new PdfPTable(3);
                            Funding.TotalWidth = 400f;
                            Funding.WidthPercentage = 100;
                            Funding.LockedWidth = true;
                            float[] Funding_widths = new float[] { 12f, 60f, 15f };
                            Funding.SetWidths(Funding_widths);
                            Funding.HorizontalAlignment = Element.ALIGN_CENTER;

                            string[] header = { "Site", "Site Name", "Funded Days" };
                            for (int header_Rows = 0; header_Rows < header.Length; header_Rows++)
                            {
                                PdfPCell day = new PdfPCell(new Phrase(header[header_Rows], TblFontBold));
                                day.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                day.BackgroundColor = BaseColor.GRAY;
                                day.Border = iTextSharp.text.Rectangle.BOX;
                                Funding.AddCell(day);
                            }

                            foreach (CommonEntity FEntity in Fundedlist)
                            {
                                PdfPCell Site = new PdfPCell(new Phrase(FEntity.Code.Trim(), TableFont));
                                Site.HorizontalAlignment = Element.ALIGN_LEFT;
                                Site.Border = iTextSharp.text.Rectangle.BOX;
                                Funding.AddCell(Site);

                                PdfPCell Site_Desc = new PdfPCell(new Phrase(FEntity.Desc.Trim(), TableFont));
                                Site_Desc.HorizontalAlignment = Element.ALIGN_LEFT;
                                Site_Desc.Border = iTextSharp.text.Rectangle.BOX;
                                Funding.AddCell(Site_Desc);

                                string FundingDays = Convert.ToInt32(FEntity.Hierarchy.Trim()).ToString("N2", CultureInfo.InvariantCulture);
                                string SFund = FundingDays.Substring(0, (FundingDays.Length - 3));
                                PdfPCell FundDayas = new PdfPCell(new Phrase(SFund, TableFont));
                                FundDayas.HorizontalAlignment = Element.ALIGN_RIGHT;
                                FundDayas.Border = iTextSharp.text.Rectangle.BOX;
                                Funding.AddCell(FundDayas);
                            }
                            PdfPCell Tot_desc = new PdfPCell(new Phrase("Total Funded Days", TableFont));
                            Tot_desc.HorizontalAlignment = Element.ALIGN_RIGHT;
                            Tot_desc.Colspan = 2;
                            Tot_desc.Border = iTextSharp.text.Rectangle.BOX;
                            Funding.AddCell(Tot_desc);

                            string TotFundingDays = Convert.ToInt32(TotFund.ToString().Trim()).ToString("N2", CultureInfo.InvariantCulture);
                            string STotFund = TotFundingDays.Substring(0, (TotFundingDays.Length - 3));
                            PdfPCell Tot = new PdfPCell(new Phrase(STotFund, TableFont));
                            Tot.HorizontalAlignment = Element.ALIGN_RIGHT;
                            Tot.Border = iTextSharp.text.Rectangle.BOX;
                            Funding.AddCell(Tot);
                            document.Add(Funding);
                        }
                    }
                    else if (rbFull.Checked.Equals(true))
                    {
                        document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                        document.NewPage();

                        PdfPTable table = new PdfPTable(10);
                        table.TotalWidth = 700f;
                        table.WidthPercentage = 100;
                        table.LockedWidth = true;
                        float[] widths = new float[] { 20f, 15f, 20f, 80f, 50f, 30f, 30f, 30f, 30f, 55f };
                        table.SetWidths(widths);
                        table.HorizontalAlignment = Element.ALIGN_CENTER;

                        PdfPTable Schd_table = new PdfPTable(7);
                        Schd_table.TotalWidth = 700f;
                        Schd_table.WidthPercentage = 100;
                        Schd_table.LockedWidth = true;
                        float[] Schd_table_widths = new float[] { 80f, 80f, 80f, 80f, 80f, 80f, 80f };
                        Schd_table.SetWidths(Schd_table_widths);
                        Schd_table.HorizontalAlignment = Element.ALIGN_CENTER;

                        DataSet dsLang = DatabaseLayer.Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.LANGUAGECODES);
                        DataTable dtLang = dsLang.Tables[0];

                        DataSet dsFund = DatabaseLayer.Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.CASEMNGMTFUNDSRC, "A");
                        DataTable dtFund = dsFund.Tables[0];
                        int fundCnt = 0; string PrivSite = string.Empty; int TotFund = 0; bool FundFirst = true;
                        List<CommonEntity> Fundedlist = new List<CommonEntity>();
                        string PrivSitenum = string.Empty;
                        string PrivSiteName = string.Empty;
                        int SiteCnt = 0;
                        if (Site_list.Count > 0)
                        {
                            foreach (ReportSiteEntity REntity in Site_list)
                            {
                                SiteCnt++;
                                string Stie_AMPM = string.Empty;
                                if (REntity.SiteROOM != "0000")
                                {
                                    PdfPCell Site = new PdfPCell(new Phrase(REntity.SiteNUMBER.Trim(), TableFont));
                                    Site.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Site.Border = iTextSharp.text.Rectangle.TOP_BORDER; //+ iTextSharp.text.Rectangle.LEFT_BORDER;
                                    table.AddCell(Site);

                                    PdfPCell room = new PdfPCell(new Phrase(REntity.SiteROOM.Trim(), TableFont));
                                    room.HorizontalAlignment = Element.ALIGN_LEFT;
                                    room.Border = iTextSharp.text.Rectangle.TOP_BORDER;
                                    table.AddCell(room);

                                    if (REntity.SiteAM_PM == "A")
                                        Stie_AMPM = "AM";
                                    else if (REntity.SiteAM_PM == "P")
                                        Stie_AMPM = "PM";
                                    else if (REntity.SiteAM_PM == "F")
                                        Stie_AMPM = "FULL";
                                    else if (REntity.SiteAM_PM == "E")
                                        Stie_AMPM = "EXTD";
                                    PdfPCell AMPM = new PdfPCell(new Phrase(Stie_AMPM.Trim(), TableFont));
                                    AMPM.HorizontalAlignment = Element.ALIGN_LEFT;
                                    AMPM.Border = iTextSharp.text.Rectangle.TOP_BORDER;
                                    table.AddCell(AMPM);

                                    PdfPCell Name = new PdfPCell(new Phrase(REntity.SiteNAME.Trim(), TableFont));
                                    Name.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Name.Border = iTextSharp.text.Rectangle.TOP_BORDER;
                                    table.AddCell(Name);

                                    PdfPCell Cat = new PdfPCell(new Phrase("Capacity  " + REntity.SITE_FUNDED_SLOTS.Trim(), TableFont));
                                    Cat.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Cat.Border = iTextSharp.text.Rectangle.TOP_BORDER;
                                    table.AddCell(Cat);

                                    PdfPCell Stdt = new PdfPCell(new Phrase("Start Date ", TableFont));
                                    Stdt.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Stdt.Border = iTextSharp.text.Rectangle.TOP_BORDER;
                                    table.AddCell(Stdt);

                                    PdfPCell Stdt1 = new PdfPCell(new Phrase(LookupDataAccess.Getdate(REntity.SiteCLASS_START.Trim()), TableFont));
                                    Stdt1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Stdt1.Border = iTextSharp.text.Rectangle.TOP_BORDER;
                                    table.AddCell(Stdt1);

                                    PdfPCell StTime = new PdfPCell(new Phrase("Start Time ", TableFont));
                                    StTime.HorizontalAlignment = Element.ALIGN_LEFT;
                                    StTime.Border = iTextSharp.text.Rectangle.TOP_BORDER;
                                    table.AddCell(StTime);

                                    PdfPCell StTime1 = new PdfPCell(new Phrase(LookupDataAccess.GetTime(REntity.SiteSTART_TIME.Trim()), TableFont));
                                    StTime1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    StTime1.Border = iTextSharp.text.Rectangle.TOP_BORDER;
                                    table.AddCell(StTime1);


                                    MaskedTextBox mskPhn = new MaskedTextBox();
                                    mskPhn.Mask = "(999) 000-0000";
                                    if (!string.IsNullOrEmpty(REntity.SitePHONE.Trim()))
                                        mskPhn.Text = REntity.SitePHONE.Trim();
                                    PdfPCell Phone = new PdfPCell(new Phrase("Phone " + mskPhn.Text.Trim(), TableFont));
                                    Phone.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Phone.Border = iTextSharp.text.Rectangle.TOP_BORDER; //+ iTextSharp.text.Rectangle.RIGHT_BORDER;
                                    table.AddCell(Phone);

                                    PdfPCell Space = new PdfPCell(new Phrase("", TableFont));
                                    Space.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Space.Colspan = 3;
                                    Space.Border = iTextSharp.text.Rectangle.NO_BORDER; //iTextSharp.text.Rectangle.LEFT_BORDER;
                                    table.AddCell(Space);

                                    PdfPCell Street = new PdfPCell(new Phrase(REntity.SiteSTREET.Trim(), TableFont));
                                    Street.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Street.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(Street);

                                    PdfPCell Trans = new PdfPCell(new Phrase("Trans Area " + REntity.SiteTRAN_AREA.Trim(), TableFont));
                                    Trans.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Trans.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(Trans);

                                    PdfPCell EndDt = new PdfPCell(new Phrase("End Date ", TableFont));
                                    EndDt.HorizontalAlignment = Element.ALIGN_LEFT;
                                    EndDt.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(EndDt);

                                    PdfPCell EndDt1 = new PdfPCell(new Phrase(LookupDataAccess.Getdate(REntity.SiteCLASS_END.Trim()), TableFont));
                                    EndDt1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    EndDt1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(EndDt1);

                                    PdfPCell EndTime = new PdfPCell(new Phrase("End Time ", TableFont));
                                    EndTime.HorizontalAlignment = Element.ALIGN_LEFT;
                                    EndTime.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(EndTime);

                                    PdfPCell EndTime1 = new PdfPCell(new Phrase(LookupDataAccess.GetTime(REntity.SiteEND_TIME.Trim()), TableFont));
                                    EndTime1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    EndTime1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(EndTime1);

                                    MaskedTextBox mskFax = new MaskedTextBox();
                                    mskFax.Mask = "(999) 000-0000";
                                    if (!string.IsNullOrEmpty(REntity.SiteFAX.Trim()))
                                        mskFax.Text = REntity.SiteFAX.Trim();

                                    PdfPCell Fax = new PdfPCell(new Phrase("Fax " + mskFax.Text.Trim(), TableFont));
                                    Fax.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Fax.Border = iTextSharp.text.Rectangle.NO_BORDER; //iTextSharp.text.Rectangle.RIGHT_BORDER;
                                    table.AddCell(Fax);

                                    PdfPCell Space1 = new PdfPCell(new Phrase("", TableFont));
                                    Space1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Space1.Colspan = 3;
                                    Space1.Border = iTextSharp.text.Rectangle.NO_BORDER; //iTextSharp.text.Rectangle.LEFT_BORDER;
                                    table.AddCell(Space1);

                                    PdfPCell City = new PdfPCell(new Phrase(REntity.SiteCITY.Trim() + " " + REntity.SiteSTATE.Trim() + " " + REntity.SiteZIP.Trim(), TableFont));
                                    City.HorizontalAlignment = Element.ALIGN_LEFT;
                                    City.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(City);

                                    PdfPCell Lang = new PdfPCell(new Phrase("Language in Class", TableFont));
                                    Lang.HorizontalAlignment = Element.ALIGN_LEFT;
                                    //Lang.Colspan = 2;
                                    Lang.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(Lang);
                                    string Language = string.Empty;
                                    if (REntity.SiteLANGUAGE.Trim() == "06")
                                        Language = REntity.SiteLANGUAGE_OTHER.Trim();
                                    else if (dtLang.Rows.Count > 0)
                                    {
                                        foreach (DataRow drLang in dtLang.Rows)
                                        {
                                            if (REntity.SiteLANGUAGE.Trim() == drLang["Code"].ToString().Trim())
                                            {
                                                Language = drLang["LookUpDesc"].ToString().Trim();
                                                break;
                                            }
                                        }
                                    }

                                    PdfPCell Lang1 = new PdfPCell(new Phrase(Language.Trim(), TableFont));
                                    Lang1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Lang1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(Lang1);

                                    PdfPCell Space2 = new PdfPCell(new Phrase("", TableFont));
                                    Space2.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Space2.Colspan = 3;
                                    Space2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(Space2);

                                    MaskedTextBox mskOther = new MaskedTextBox();
                                    mskOther.Mask = "(999) 000-0000";
                                    if (!string.IsNullOrEmpty(REntity.SiteOTHER_PHONE.Trim()))
                                        mskOther.Text = REntity.SiteOTHER_PHONE.Trim();

                                    PdfPCell Other = new PdfPCell(new Phrase("Other " + mskOther.Text.Trim(), TableFont));
                                    Other.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Other.Border = iTextSharp.text.Rectangle.NO_BORDER; //iTextSharp.text.Rectangle.RIGHT_BORDER;
                                    table.AddCell(Other);

                                    PdfPCell Space_Row = new PdfPCell(new Phrase("", TableFont));
                                    Space_Row.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Space_Row.Colspan = 10;
                                    Space_Row.Border = iTextSharp.text.Rectangle.NO_BORDER; //iTextSharp.text.Rectangle.LEFT_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER;
                                    table.AddCell(Space_Row);

                                    string Mon = string.Empty, Tue = string.Empty, Wed = string.Empty, Thu = string.Empty, Fri = string.Empty, Sat = string.Empty, Sun = string.Empty;
                                    string meals = REntity.SiteMEAL_AREA.Trim();
                                    for (int i = 0; i < meals.Length; i++)
                                    {
                                        if (meals.Substring(i, 1).ToString().Trim() == "Y")
                                        {
                                            switch (i)
                                            {
                                                case 0: Mon += "B"; break;
                                                case 1: Tue += "B"; break;
                                                case 2: Wed += "B"; break;
                                                case 3: Thu += "B"; break;
                                                case 4: Fri += "B"; break;
                                                case 5: Sat += "B"; break;
                                                case 6: Sun += "B"; break;
                                                case 7: Mon += "A"; break;
                                                case 8: Tue += "A"; break;
                                                case 9: Wed += "A"; break;
                                                case 10: Thu += "A"; break;
                                                case 11: Fri += "A"; break;
                                                case 12: Sat += "A"; break;
                                                case 13: Sun += "A"; break;
                                                case 14: Mon += "L"; break;
                                                case 15: Tue += "L"; break;
                                                case 16: Wed += "L"; break;
                                                case 17: Thu += "L"; break;
                                                case 18: Fri += "L"; break;
                                                case 19: Sat += "L"; break;
                                                case 20: Sun += "L"; break;
                                                case 21: Mon += "P"; break;
                                                case 22: Tue += "P"; break;
                                                case 23: Wed += "P"; break;
                                                case 24: Thu += "P"; break;
                                                case 25: Fri += "P"; break;
                                                case 26: Sat += "P"; break;
                                                case 27: Sun += "P"; break;
                                                case 28: Mon += "S"; break;
                                                case 29: Tue += "S"; break;
                                                case 30: Wed += "S"; break;
                                                case 31: Thu += "S"; break;
                                                case 32: Fri += "S"; break;
                                                case 33: Sat += "S"; break;
                                                case 34: Sun += "S"; break;
                                            }
                                        }
                                    }

                                    PdfPCell Space_Row1 = new PdfPCell(new Phrase("", TableFont));
                                    Space_Row1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Space_Row1.Colspan = 10;
                                    Space_Row1.Border = iTextSharp.text.Rectangle.NO_BORDER; //iTextSharp.text.Rectangle.LEFT_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER;
                                    table.AddCell(Space_Row1);

                                    PdfPCell Meals = new PdfPCell(new Phrase("Meals: ", TableFont));
                                    Meals.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Meals.Colspan = 3;
                                    Meals.Border = iTextSharp.text.Rectangle.NO_BORDER; //iTextSharp.text.Rectangle.LEFT_BORDER;
                                    table.AddCell(Meals);

                                    if (string.IsNullOrEmpty(Mon.Trim())) Mon = "None";
                                    if (string.IsNullOrEmpty(Tue.Trim())) Tue = "None"; if (string.IsNullOrEmpty(Wed.Trim())) Wed = "None";
                                    if (string.IsNullOrEmpty(Thu.Trim())) Thu = "None"; if (string.IsNullOrEmpty(Fri.Trim())) Fri = "None";
                                    if (string.IsNullOrEmpty(Sat.Trim())) Sat = "None"; if (string.IsNullOrEmpty(Sun.Trim())) Sun = "None";

                                    PdfPCell Weeks = new PdfPCell(new Phrase("Mon: " + Mon.Trim() + "       " + "Tue: " + Tue.Trim() + "       " + "Wed: " + Wed.Trim() + "       " + "Thu: " + Thu.Trim() + "       " + "Fri: " + Fri.Trim() + "       " + "Sat: " + Sat.Trim() + "       " + "Sun: " + Sun.Trim(), TableFont));
                                    Weeks.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Weeks.Colspan = 7;
                                    Weeks.Border = iTextSharp.text.Rectangle.NO_BORDER;//iTextSharp.text.Rectangle.RIGHT_BORDER;
                                    table.AddCell(Weeks);

                                    PdfPCell Empty = new PdfPCell(new Phrase("", TableFont));
                                    Empty.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Empty.Colspan = 10;
                                    Empty.FixedHeight = 10f;
                                    Empty.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;// iTextSharp.text.Rectangle.LEFT_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER + iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                    table.AddCell(Empty);

                                    site_Search.ATTM_AGENCY = REntity.SiteAGENCY.Trim(); site_Search.ATTM_DEPT = REntity.SiteDEPT.Trim();
                                    site_Search.ATTM_PROG = REntity.SitePROG.Trim(); site_Search.ATTM_YEAR = REntity.SiteYEAR.Trim();
                                    site_Search.ATTM_SITE = REntity.SiteNUMBER.Trim(); //site_Search.ATTM_ROOM = REntity.SiteROOM.Trim();
                                    //site_Search.ATTM_AMPM = REntity.SiteAM_PM.Trim();
                                    SchduleList = _model.SPAdminData.Browse_CHILDATTM(site_Search, "Browse");
                                    if (SchduleList.Count > 0)
                                    {
                                        int length = 0;
                                        if (txtEndYear.Text.Trim() == txtStrtYear.Text.Trim())
                                            length = int.Parse(txtEndMnth.Text.Trim()) - int.Parse(txtStrtMnth.Text.Trim()) + 1;
                                        else
                                        {
                                            int temp = 0;
                                            temp = 12 - int.Parse(txtStrtMnth.Text.Trim()) + 1;
                                            length = int.Parse(txtEndMnth.Text.Trim()) + temp;
                                        }
                                        int j = int.Parse(txtStrtMnth.Text.Trim()); int StrtYear = int.Parse(txtStrtYear.Text.Trim());
                                        List<SiteScheduleEntity> Selected_SchduleList = new List<SiteScheduleEntity>();
                                        for (int i = 1; i <= length; i++)
                                        {
                                            Selected_SchduleList = SchduleList.FindAll(u => u.ATTM_ROOM.Trim().Equals(REntity.SiteROOM.Trim()) && u.ATTM_AMPM.Trim().Equals(REntity.SiteAM_PM.Trim()) && u.ATTM_MONTH.Trim().Equals(j.ToString()) && u.ATTM_FUND.Trim().Equals("") && u.ATTM_CALENDER_YEAR.Equals(StrtYear.ToString()));
                                            if (Selected_SchduleList.Count>0)
                                            {
                                                Selected_SchduleList = SchduleList.FindAll(u => u.ATTM_ROOM.Trim().Equals(REntity.SiteROOM.Trim()) && u.ATTM_AMPM.Trim().Equals(REntity.SiteAM_PM.Trim()) && u.ATTM_MONTH.Trim().Equals(j.ToString()) && u.ATTM_CALENDER_YEAR.Equals(StrtYear.ToString()));
                                            }
                                            else
                                            {
                                                Selected_SchduleList = SchduleList.FindAll(u => u.ATTM_ROOM.Trim().Equals("") && u.ATTM_AMPM.Trim().Equals("") && u.ATTM_MONTH.Trim().Equals(j.ToString()) && u.ATTM_FUND.Trim().Equals("") && u.ATTM_CALENDER_YEAR.Equals(StrtYear.ToString()));
                                                if (Selected_SchduleList.Count > 0)
                                                {
                                                    Selected_SchduleList = SchduleList.FindAll(u => u.ATTM_ROOM.Trim().Equals("") && u.ATTM_AMPM.Trim().Equals("") && u.ATTM_MONTH.Trim().Equals(j.ToString()) && u.ATTM_CALENDER_YEAR.Equals(StrtYear.ToString()));
                                                }
                                                else
                                                {
                                                    SiteScheduleEntity Site_MonthSearch = new SiteScheduleEntity(true);
                                                    List<SiteScheduleEntity> Sel_MonthSite = new List<SiteScheduleEntity>();
                                                    Site_MonthSearch.ATTM_AGENCY = REntity.SiteAGENCY.Trim(); Site_MonthSearch.ATTM_DEPT = REntity.SiteDEPT.Trim();
                                                    Site_MonthSearch.ATTM_PROG = REntity.SitePROG.Trim(); Site_MonthSearch.ATTM_YEAR = REntity.SiteYEAR.Trim();
                                                    Site_MonthSearch.ATTM_MONTH = j.ToString(); Site_MonthSearch.ATTM_CALENDER_YEAR = StrtYear.ToString(); //site_Search.ATTM_SITE = "    "; site_Search.ATTM_ROOM = "    ";
                                                    Sel_MonthSite = _model.SPAdminData.Browse_CHILDATTM(Site_MonthSearch, "Browse");
                                                    //MasterSchd = _model.SPAdminData.Browse_CHILDATTM(site_Search, "Browse");
                                                    //Selected_SchduleList = MasterSchd.Find(u => u.ATTM_SITE.Trim().Equals("") && u.ATTM_ROOM.Trim().Equals("") && u.ATTM_MONTH.Trim().Equals(j.ToString()));
                                                    Selected_SchduleList = Sel_MonthSite.FindAll(u => u.ATTM_SITE.Trim().Equals("") && u.ATTM_ROOM.Trim().Equals(""));
                                                    //if (Selected_SchduleList.Count > 0)
                                                    //{
                                                    //}
                                                }
                                            }
                                            if (Selected_SchduleList.Count > 0)
                                            {
                                                SiteScheduleEntity FundList = Selected_SchduleList.Find(u => u.ATTM_FUND.Trim().Equals(""));
                                                Selected_SchduleList = Selected_SchduleList.OrderBy(u => u.ATTM_FUND).ToList();
                                                bool First = true; string Second = string.Empty;
                                                if (FundList != null)
                                                {
                                                    foreach (SiteScheduleEntity SEntity in Selected_SchduleList)
                                                    {
                                                        List<ChildATTMSEntity> Month_List = new List<ChildATTMSEntity>();
                                                        ChildATTMSEntity Search_Month = new ChildATTMSEntity(true);
                                                        Search_Month.ATTMS_ID = SEntity.ATTM_ID.Trim();
                                                        Month_List = _model.SPAdminData.Browse_CHILDATTMS(Search_Month, "Browse");
                                                        if (Month_List.Count > 0)
                                                        {
                                                            if (First)
                                                            {
                                                                PdfPCell Month = new PdfPCell(new Phrase(GetMonth(SEntity.ATTM_MONTH.Trim()) + " " + SEntity.ATTM_CALENDER_YEAR.Trim(), TblFontBold));
                                                                Month.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                Month.Colspan = 7;
                                                                Month.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                Schd_table.AddCell(Month);
                                                                First = false;
                                                            }
                                                            else
                                                            {
                                                                string Fund = string.Empty;
                                                                if (!string.IsNullOrEmpty(SEntity.ATTM_FUND.Trim()))
                                                                {
                                                                    foreach (DataRow drFund in dtFund.Rows)
                                                                    {
                                                                        if (SEntity.ATTM_FUND.Trim() == drFund["Code"].ToString().Trim())
                                                                        {
                                                                            Fund += drFund["LookUpDesc"].ToString().Trim();
                                                                            break;
                                                                        }
                                                                    }
                                                                }
                                                                Second = "Except ";

                                                                PdfPCell Month = new PdfPCell(new Phrase("Except " + Fund, TblFontBold));
                                                                Month.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                Month.Colspan = 7;
                                                                Month.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                Schd_table.AddCell(Month);
                                                            }



                                                            string[] Weekdays = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

                                                            for (int Week = 0; Week < Weekdays.Length; Week++)
                                                            {
                                                                PdfPCell day = new PdfPCell(new Phrase(Weekdays[Week], TableFont));
                                                                day.HorizontalAlignment = Element.ALIGN_CENTER;
                                                                day.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                Schd_table.AddCell(day);
                                                            }
                                                            DateTime firstday = new DateTime(Convert.ToInt32(SEntity.ATTM_CALENDER_YEAR.Trim()), Convert.ToInt32(SEntity.ATTM_MONTH.Trim()), 1);
                                                            DateTime end_Month = new DateTime(Convert.ToInt32(SEntity.ATTM_CALENDER_YEAR.Trim()), Convert.ToInt32(SEntity.ATTM_MONTH.Trim()), DateTime.DaysInMonth(Convert.ToInt32(SEntity.ATTM_CALENDER_YEAR.Trim()), Convert.ToInt32(SEntity.ATTM_MONTH.Trim())));
                                                            string Strtday = firstday.DayOfWeek.ToString();
                                                            string Endday = end_Month.DayOfWeek.ToString();

                                                            switch (Strtday)
                                                            {
                                                                case "Monday": break;
                                                                case "Tuesday": PdfPCell space = new PdfPCell(new Phrase("", TableFont));
                                                                    space.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                    space.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                    Schd_table.AddCell(space); break;
                                                                case "Wednesday": PdfPCell Wspace = new PdfPCell(new Phrase("", TableFont));
                                                                    Wspace.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                    Wspace.Colspan = 2;
                                                                    Wspace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                    Schd_table.AddCell(Wspace);
                                                                    break;
                                                                case "Thursday": PdfPCell Tspace = new PdfPCell(new Phrase("", TableFont));
                                                                    Tspace.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                    Tspace.Colspan = 3;
                                                                    Tspace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                    Schd_table.AddCell(Tspace); break;
                                                                case "Friday": PdfPCell Fspace = new PdfPCell(new Phrase("", TableFont));
                                                                    Fspace.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                    Fspace.Colspan = 4;
                                                                    Fspace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                    Schd_table.AddCell(Fspace);
                                                                    break;
                                                                case "Saturday": PdfPCell Satspace = new PdfPCell(new Phrase("", TableFont));
                                                                    Satspace.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                    Satspace.Colspan = 5;
                                                                    Satspace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                    Schd_table.AddCell(Satspace);
                                                                    break;
                                                                case "Sunday": PdfPCell Sunspace = new PdfPCell(new Phrase("", TableFont));
                                                                    Sunspace.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                    Sunspace.Colspan = 6;
                                                                    Sunspace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                    Schd_table.AddCell(Sunspace);
                                                                    break;
                                                            }
                                                            foreach (ChildATTMSEntity AEntity in Month_List)
                                                            {
                                                                string Status = string.Empty;
                                                                string Stat_Extn = string.Empty;
                                                                if (Template_List.Count > 0)
                                                                {
                                                                    foreach (Headstart_Template Entity in Template_List)
                                                                    {
                                                                        if (Entity.Code_Desc_Tag.Trim() == AEntity.ATTMS_STATUS.Trim())
                                                                        {
                                                                            Status = Entity.Code_Desc.Trim();
                                                                            if (REntity.SiteNUMBER.Trim() != PrivSite)
                                                                            {

                                                                                if (!FundFirst)
                                                                                {
                                                                                    Fundedlist.Add(new CommonEntity(PrivSitenum, PrivSiteName, fundCnt.ToString()));
                                                                                    TotFund = TotFund + fundCnt;
                                                                                    fundCnt = 0;
                                                                                }
                                                                                FundFirst = false;
                                                                                PrivSite = REntity.SiteNUMBER.Trim();
                                                                                PrivSitenum = REntity.SiteNUMBER.Trim();
                                                                                PrivSiteName = REntity.SiteNAME.Trim();
                                                                            }
                                                                            //if (AEntity.ATTMS_STATUS.Trim() == "O")
                                                                            //    fundCnt++;
                                                                            if (string.IsNullOrEmpty(Second.Trim()))
                                                                            {
                                                                                if (!string.IsNullOrEmpty(Entity.Agy_2.Trim()))
                                                                                {
                                                                                    Stat_Extn = " (" + Entity.Agy_2.Trim() + ")";
                                                                                    fundCnt++;
                                                                                }
                                                                            }
                                                                            break;
                                                                           
                                                                        }
                                                                    }

                                                                }

                                                                PdfPCell Day_Desc = new PdfPCell(new Phrase(AEntity.ATTMS_DAY.Trim() + "  " + Status.Trim() + Stat_Extn, TableFont));
                                                                Day_Desc.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                Day_Desc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                Schd_table.AddCell(Day_Desc);

                                                            }

                                                            switch (Endday)
                                                            {
                                                                case "Sunday": break;
                                                                case "Saturday": PdfPCell space = new PdfPCell(new Phrase("", TableFont));
                                                                    space.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                    space.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                    Schd_table.AddCell(space); break;
                                                                case "Friday": PdfPCell Wspace = new PdfPCell(new Phrase("", TableFont));
                                                                    Wspace.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                    Wspace.Colspan = 2;
                                                                    Wspace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                    Schd_table.AddCell(Wspace);
                                                                    break;
                                                                case "Thursday": PdfPCell Tspace = new PdfPCell(new Phrase("", TableFont));
                                                                    Tspace.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                    Tspace.Colspan = 3;
                                                                    Tspace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                    Schd_table.AddCell(Tspace); break;
                                                                case "Wednesday": PdfPCell Fspace = new PdfPCell(new Phrase("", TableFont));
                                                                    Fspace.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                    Fspace.Colspan = 4;
                                                                    Fspace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                    Schd_table.AddCell(Fspace);
                                                                    break;
                                                                case "Tuesday": PdfPCell Satspace = new PdfPCell(new Phrase("", TableFont));
                                                                    Satspace.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                    Satspace.Colspan = 5;
                                                                    Satspace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                    Schd_table.AddCell(Satspace);
                                                                    break;
                                                                case "Monday": PdfPCell Sunspace = new PdfPCell(new Phrase("", TableFont));
                                                                    Sunspace.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                    Sunspace.Colspan = 6;
                                                                    Sunspace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                    Schd_table.AddCell(Sunspace);
                                                                    break;
                                                            }

                                                        }

                                                        PdfPCell line = new PdfPCell(new Phrase("", TableFont));
                                                        line.HorizontalAlignment = Element.ALIGN_LEFT;
                                                        line.Colspan = 7;
                                                        line.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        Schd_table.AddCell(line);
                                                    }
                                                }
                                            }
                                            j++;
                                            if (j > 12) { j = 1; StrtYear = StrtYear + 1; }
                                        }


                                        PdfPCell line1 = new PdfPCell(new Phrase("", TableFont));
                                        line1.HorizontalAlignment = Element.ALIGN_LEFT;
                                        line1.Colspan = 7;
                                        line1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                        Schd_table.AddCell(line1);

                                        if (table.Rows.Count > 0)
                                        {
                                            document.Add(table);
                                            table.DeleteBodyRows();
                                        }
                                        if (Schd_table.Rows.Count > 0)
                                        {
                                            document.Add(Schd_table);
                                            Schd_table.DeleteBodyRows();
                                        }
                                        document.NewPage();
                                        //PdfPCell line = new PdfPCell(new Phrase("", TableFont));
                                        //line.HorizontalAlignment = Element.ALIGN_LEFT;
                                        //line.Colspan = 7;
                                        //line.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                        //Schd_table.AddCell(line);
                                    }
                                    else
                                    {
                                        int length = 0;
                                        if (txtEndYear.Text.Trim() == txtStrtYear.Text.Trim())
                                            length = int.Parse(txtEndMnth.Text.Trim()) - int.Parse(txtStrtMnth.Text.Trim()) + 1;
                                        else
                                        {
                                            int temp = 0;
                                            temp = 12 - int.Parse(txtStrtMnth.Text.Trim()) + 1;
                                            length = int.Parse(txtEndMnth.Text.Trim()) + temp;
                                        }
                                        int j = int.Parse(txtStrtMnth.Text.Trim());
                                        int StrtYear = int.Parse(txtStrtYear.Text.Trim());
                                        List<SiteScheduleEntity> Selected_SchduleList = new List<SiteScheduleEntity>();
                                        for (int i = 1; i <= length; i++)
                                        {
                                                    SiteScheduleEntity Site_MonthSearch = new SiteScheduleEntity(true);
                                                    List<SiteScheduleEntity> Sel_MonthSite = new List<SiteScheduleEntity>();
                                                    Site_MonthSearch.ATTM_AGENCY = REntity.SiteAGENCY.Trim(); Site_MonthSearch.ATTM_DEPT = REntity.SiteDEPT.Trim();
                                                    Site_MonthSearch.ATTM_PROG = REntity.SitePROG.Trim(); Site_MonthSearch.ATTM_YEAR = REntity.SiteYEAR.Trim();
                                                    Site_MonthSearch.ATTM_MONTH = j.ToString(); Site_MonthSearch.ATTM_CALENDER_YEAR = StrtYear.ToString();
                                                    //site_Search.ATTM_SITE = "    "; site_Search.ATTM_ROOM = "    ";
                                                    Sel_MonthSite = _model.SPAdminData.Browse_CHILDATTM(Site_MonthSearch, "Browse");
                                                    //MasterSchd = _model.SPAdminData.Browse_CHILDATTM(site_Search, "Browse");
                                                    //Selected_SchduleList = MasterSchd.Find(u => u.ATTM_SITE.Trim().Equals("") && u.ATTM_ROOM.Trim().Equals("") && u.ATTM_MONTH.Trim().Equals(j.ToString()));
                                                    Selected_SchduleList = Sel_MonthSite.FindAll(u => u.ATTM_SITE.Trim().Equals("") && u.ATTM_ROOM.Trim().Equals(""));
                                                    //if (Selected_SchduleList != null)
                                                    //{
                                                    //}


                                                    if (Selected_SchduleList.Count>0)
                                                    {
                                                        SiteScheduleEntity FundList = Selected_SchduleList.Find(u => u.ATTM_FUND.Trim().Equals(""));
                                                        Selected_SchduleList = Selected_SchduleList.OrderBy(u => u.ATTM_FUND).ToList();
                                                        bool First = true; string Second = string.Empty;
                                                        if (FundList != null)
                                                        {
                                                            foreach (SiteScheduleEntity Mast in Selected_SchduleList)
                                                            {
                                                                List<ChildATTMSEntity> Month_List = new List<ChildATTMSEntity>();
                                                                ChildATTMSEntity Search_Month = new ChildATTMSEntity(true);
                                                                Search_Month.ATTMS_ID = Mast.ATTM_ID.Trim();
                                                                Month_List = _model.SPAdminData.Browse_CHILDATTMS(Search_Month, "Browse");
                                                                if (Month_List.Count > 0)
                                                                {
                                                                    if (First)
                                                                    {
                                                                        PdfPCell Month = new PdfPCell(new Phrase(GetMonth(Mast.ATTM_MONTH.Trim()) + " " + Mast.ATTM_CALENDER_YEAR.Trim(), TblFontBold));
                                                                        Month.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                        Month.Colspan = 7;
                                                                        Month.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                        Schd_table.AddCell(Month);
                                                                        First = false;
                                                                    }
                                                                    else
                                                                    {
                                                                        string Fund = string.Empty;
                                                                        if (!string.IsNullOrEmpty(Mast.ATTM_FUND.Trim()))
                                                                        {
                                                                            foreach (DataRow drFund in dtFund.Rows)
                                                                            {
                                                                                if (Mast.ATTM_FUND.Trim() == drFund["Code"].ToString().Trim())
                                                                                {
                                                                                    Fund += drFund["LookUpDesc"].ToString().Trim();
                                                                                    break;
                                                                                }
                                                                            }
                                                                        }
                                                                        Second = "Except ";

                                                                        PdfPCell Month = new PdfPCell(new Phrase("Except " + Fund, TblFontBold));
                                                                        Month.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                        Month.Colspan = 7;
                                                                        Month.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                        Schd_table.AddCell(Month);
                                                                    }

                                                                    string[] Weekdays = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

                                                                    for (int Week = 0; Week < Weekdays.Length; Week++)
                                                                    {
                                                                        PdfPCell day = new PdfPCell(new Phrase(Weekdays[Week], TableFont));
                                                                        day.HorizontalAlignment = Element.ALIGN_CENTER;
                                                                        day.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                        Schd_table.AddCell(day);
                                                                    }
                                                                    DateTime firstday = new DateTime(Convert.ToInt32(Mast.ATTM_CALENDER_YEAR.Trim()), Convert.ToInt32(Mast.ATTM_MONTH.Trim()), 1);
                                                                    DateTime end_Month = new DateTime(Convert.ToInt32(Mast.ATTM_CALENDER_YEAR.Trim()), Convert.ToInt32(Mast.ATTM_MONTH.Trim()), DateTime.DaysInMonth(Convert.ToInt32(Mast.ATTM_CALENDER_YEAR.Trim()), Convert.ToInt32(Mast.ATTM_MONTH.Trim())));
                                                                    string Strtday = firstday.DayOfWeek.ToString();
                                                                    string Endday = end_Month.DayOfWeek.ToString();

                                                                    switch (Strtday)
                                                                    {
                                                                        case "Monday": break;
                                                                        case "Tuesday": PdfPCell space = new PdfPCell(new Phrase("", TableFont));
                                                                            space.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                            space.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                            Schd_table.AddCell(space); break;
                                                                        case "Wednesday": PdfPCell Wspace = new PdfPCell(new Phrase("", TableFont));
                                                                            Wspace.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                            Wspace.Colspan = 2;
                                                                            Wspace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                            Schd_table.AddCell(Wspace);
                                                                            break;
                                                                        case "Thursday": PdfPCell Tspace = new PdfPCell(new Phrase("", TableFont));
                                                                            Tspace.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                            Tspace.Colspan = 3;
                                                                            Tspace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                            Schd_table.AddCell(Tspace); break;
                                                                        case "Friday": PdfPCell Fspace = new PdfPCell(new Phrase("", TableFont));
                                                                            Fspace.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                            Fspace.Colspan = 4;
                                                                            Fspace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                            Schd_table.AddCell(Fspace);
                                                                            break;
                                                                        case "Saturday": PdfPCell Satspace = new PdfPCell(new Phrase("", TableFont));
                                                                            Satspace.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                            Satspace.Colspan = 5;
                                                                            Satspace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                            Schd_table.AddCell(Satspace);
                                                                            break;
                                                                        case "Sunday": PdfPCell Sunspace = new PdfPCell(new Phrase("", TableFont));
                                                                            Sunspace.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                            Sunspace.Colspan = 6;
                                                                            Sunspace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                            Schd_table.AddCell(Sunspace);
                                                                            break;
                                                                    }
                                                                    foreach (ChildATTMSEntity AEntity in Month_List)
                                                                    {
                                                                        string Status = string.Empty;
                                                                        string Stat_Extn = string.Empty;
                                                                        if (Template_List.Count > 0)
                                                                        {
                                                                            foreach (Headstart_Template Entity in Template_List)
                                                                            {
                                                                                if (Entity.Code_Desc_Tag.Trim() == AEntity.ATTMS_STATUS.Trim())
                                                                                {
                                                                                    Status = Entity.Code_Desc.Trim();
                                                                                    if (REntity.SiteNUMBER.Trim() != PrivSite)
                                                                                    {

                                                                                        if (!FundFirst)
                                                                                        {
                                                                                            Fundedlist.Add(new CommonEntity(PrivSitenum, PrivSiteName, fundCnt.ToString()));
                                                                                            TotFund = TotFund + fundCnt;
                                                                                            fundCnt = 0;
                                                                                        }
                                                                                        FundFirst = false;
                                                                                        PrivSite = REntity.SiteNUMBER.Trim();
                                                                                        PrivSitenum = REntity.SiteNUMBER.Trim();
                                                                                        PrivSiteName = REntity.SiteNAME.Trim();
                                                                                    }
                                                                                    //if (AEntity.ATTMS_STATUS.Trim() == "O")
                                                                                    //    fundCnt++;
                                                                                    if (string.IsNullOrEmpty(Second.Trim()))
                                                                                    {
                                                                                        if (!string.IsNullOrEmpty(Entity.Agy_2.Trim()))
                                                                                        {
                                                                                            Stat_Extn = " (" + Entity.Agy_2.Trim() + ")";
                                                                                            fundCnt++;
                                                                                        }
                                                                                    }
                                                                                    break;

                                                                                }
                                                                            }

                                                                        }

                                                                        PdfPCell Day_Desc = new PdfPCell(new Phrase(AEntity.ATTMS_DAY.Trim() + "  " + Status.Trim() + Stat_Extn, TableFont));
                                                                        Day_Desc.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                        Day_Desc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                        Schd_table.AddCell(Day_Desc);

                                                                    }

                                                                    switch (Endday)
                                                                    {
                                                                        case "Sunday": break;
                                                                        case "Saturday": PdfPCell space = new PdfPCell(new Phrase("", TableFont));
                                                                            space.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                            space.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                            Schd_table.AddCell(space); break;
                                                                        case "Friday": PdfPCell Wspace = new PdfPCell(new Phrase("", TableFont));
                                                                            Wspace.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                            Wspace.Colspan = 2;
                                                                            Wspace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                            Schd_table.AddCell(Wspace);
                                                                            break;
                                                                        case "Thursday": PdfPCell Tspace = new PdfPCell(new Phrase("", TableFont));
                                                                            Tspace.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                            Tspace.Colspan = 3;
                                                                            Tspace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                            Schd_table.AddCell(Tspace); break;
                                                                        case "Wednesday": PdfPCell Fspace = new PdfPCell(new Phrase("", TableFont));
                                                                            Fspace.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                            Fspace.Colspan = 4;
                                                                            Fspace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                            Schd_table.AddCell(Fspace);
                                                                            break;
                                                                        case "Tuesday": PdfPCell Satspace = new PdfPCell(new Phrase("", TableFont));
                                                                            Satspace.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                            Satspace.Colspan = 5;
                                                                            Satspace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                            Schd_table.AddCell(Satspace);
                                                                            break;
                                                                        case "Monday": PdfPCell Sunspace = new PdfPCell(new Phrase("", TableFont));
                                                                            Sunspace.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                            Sunspace.Colspan = 6;
                                                                            Sunspace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                            Schd_table.AddCell(Sunspace);
                                                                            break;
                                                                    }

                                                                }

                                                                PdfPCell line = new PdfPCell(new Phrase("", TableFont));
                                                                line.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                line.Colspan = 7;
                                                                line.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                Schd_table.AddCell(line);
                                                            }
                                                        }
                                                    }
                                            j++;
                                            if (j > 12) { j = 1; StrtYear=StrtYear+1;}
                                        }


                                        PdfPCell line1 = new PdfPCell(new Phrase("", TableFont));
                                        line1.HorizontalAlignment = Element.ALIGN_LEFT;
                                        line1.Colspan = 7;
                                        line1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                        Schd_table.AddCell(line1);

                                        if (table.Rows.Count > 0)
                                        {
                                            document.Add(table);
                                            table.DeleteBodyRows();
                                        }
                                        if (Schd_table.Rows.Count > 0)
                                        {
                                            document.Add(Schd_table);
                                            Schd_table.DeleteBodyRows();
                                        }
                                        document.NewPage();
                                    }
                                    if (Site_list.Count == SiteCnt)
                                    {
                                        Fundedlist.Add(new CommonEntity(REntity.SiteNUMBER.Trim(), REntity.SiteNAME.Trim(), fundCnt.ToString()));
                                        TotFund = TotFund + fundCnt;
                                    }
                                }

                            }
                            document.NewPage();
                            PdfPTable Funding = new PdfPTable(3);
                            Funding.TotalWidth = 400f;
                            Funding.WidthPercentage = 100;
                            Funding.LockedWidth = true;
                            float[] Funding_widths = new float[] { 12f, 60f, 15f };
                            Funding.SetWidths(Funding_widths);
                            Funding.HorizontalAlignment = Element.ALIGN_CENTER;

                            string[] header = { "Site", "Site Name", "Funded Days" };
                            for (int header_Rows = 0; header_Rows < header.Length; header_Rows++)
                            {
                                PdfPCell day = new PdfPCell(new Phrase(header[header_Rows], TblFontBold));
                                day.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                day.BackgroundColor = BaseColor.GRAY;
                                day.Border = iTextSharp.text.Rectangle.BOX;
                                Funding.AddCell(day);
                            }

                            foreach (CommonEntity FEntity in Fundedlist)
                            {
                                PdfPCell Site = new PdfPCell(new Phrase(FEntity.Code.Trim(), TableFont));
                                Site.HorizontalAlignment = Element.ALIGN_LEFT;
                                Site.Border = iTextSharp.text.Rectangle.BOX;
                                Funding.AddCell(Site);

                                PdfPCell Site_Desc = new PdfPCell(new Phrase(FEntity.Desc.Trim(), TableFont));
                                Site_Desc.HorizontalAlignment = Element.ALIGN_LEFT;
                                Site_Desc.Border = iTextSharp.text.Rectangle.BOX;
                                Funding.AddCell(Site_Desc);

                                string FundingDays = Convert.ToInt32(FEntity.Hierarchy.Trim()).ToString("N2", CultureInfo.InvariantCulture);
                                string SFund = FundingDays.Substring(0, (FundingDays.Length - 3));
                                PdfPCell FundDayas = new PdfPCell(new Phrase(SFund, TableFont));
                                FundDayas.HorizontalAlignment = Element.ALIGN_RIGHT;
                                FundDayas.Border = iTextSharp.text.Rectangle.BOX;
                                Funding.AddCell(FundDayas);
                            }
                            PdfPCell Tot_desc = new PdfPCell(new Phrase("Total Funded Days", TableFont));
                            Tot_desc.HorizontalAlignment = Element.ALIGN_RIGHT;
                            Tot_desc.Colspan = 2;
                            Tot_desc.Border = iTextSharp.text.Rectangle.BOX;
                            Funding.AddCell(Tot_desc);

                            string TotFundingDays = Convert.ToInt32(TotFund.ToString().Trim()).ToString("N2", CultureInfo.InvariantCulture);
                            string STotFund = TotFundingDays.Substring(0, (TotFundingDays.Length - 3));
                            PdfPCell Tot = new PdfPCell(new Phrase(STotFund, TableFont));
                            Tot.HorizontalAlignment = Element.ALIGN_RIGHT;
                            Tot.Border = iTextSharp.text.Rectangle.BOX;
                            Funding.AddCell(Tot);
                            document.Add(Funding);
                        }
                    }
                }
                else if (rbSelectedSite.Checked.Equals(true))
                {
                    if (rbCondensed.Checked.Equals(true))
                    {
                        document.NewPage();

                        PdfPTable table = new PdfPTable(7);
                        table.TotalWidth = 550f;
                        table.WidthPercentage = 100;
                        table.LockedWidth = true;
                        float[] widths = new float[] { 20f, 15f, 20f, 60f, 34f, 88f, 28f };
                        table.SetWidths(widths);
                        table.HorizontalAlignment = Element.ALIGN_CENTER;

                        string[] header1 = { "Site", "Room", "AM/PM", "Name", "Telephone", "Address", "Funded Days" };
                        for (int head_Rows = 0; head_Rows < header1.Length; head_Rows++)
                        {
                            PdfPCell day = new PdfPCell(new Phrase(header1[head_Rows], TblFontBold));
                            day.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            day.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            day.BackgroundColor = BaseColor.LIGHT_GRAY;
                            table.AddCell(day);
                        }

                        string Priv_SName = string.Empty;
                        int fundCnt = 0; string PrivSite = string.Empty; int TotFund = 0; bool FundFirst = true;
                        List<CommonEntity> Fundedlist = new List<CommonEntity>();
                        string PrivSitenum = string.Empty;
                        string PrivSiteName = string.Empty;
                        int SiteCnt = 0;
                        if (Site_list.Count > 0)
                        {
                            int FundedDays = 0;
                            string Site_main = string.Empty, Site_main_Name = string.Empty; //int FundDays = 0;
                            foreach (CaseSiteEntity SelEntity in Sel_REFS_List)
                            {
                                SiteCnt++;
                                ReportSiteEntity sel_site_Row = Site_list.Find(u => u.SiteNUMBER.Trim().Equals(SelEntity.SiteNUMBER.Trim()) && u.SiteROOM.Trim().Equals(SelEntity.SiteROOM.Trim()) && u.SiteAM_PM.Trim().Equals(SelEntity.SiteAM_PM.Trim()));
                                if (sel_site_Row != null)
                                {
                                    FundedDays = 0;
                                    string Stie_AMPM = string.Empty;

                                    PdfPCell Site = new PdfPCell(new Phrase(sel_site_Row.SiteNUMBER.Trim(), TableFont));
                                    Site.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Site.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(Site);

                                    PdfPCell Room = new PdfPCell(new Phrase(sel_site_Row.SiteROOM.Trim(), TableFont));
                                    Room.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Room.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(Room);

                                    if (sel_site_Row.SiteAM_PM == "A")
                                        Stie_AMPM = "AM";
                                    else if (sel_site_Row.SiteAM_PM == "P")
                                        Stie_AMPM = "PM";
                                    else if (sel_site_Row.SiteAM_PM == "F")
                                        Stie_AMPM = "FULL";
                                    else if (sel_site_Row.SiteAM_PM == "E")
                                        Stie_AMPM = "EXTD";
                                    PdfPCell AM = new PdfPCell(new Phrase(Stie_AMPM, TableFont));
                                    AM.HorizontalAlignment = Element.ALIGN_LEFT;
                                    AM.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(AM);

                                    PdfPCell Name = new PdfPCell(new Phrase(sel_site_Row.SiteNAME.Trim(), TableFont));
                                    Name.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Name.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(Name);

                                    MaskedTextBox mskPhn = new MaskedTextBox();
                                    mskPhn.Mask = "(999) 000-0000";
                                    if (!string.IsNullOrEmpty(sel_site_Row.SitePHONE.Trim()))
                                        mskPhn.Text = sel_site_Row.SitePHONE.Trim();

                                    PdfPCell Phone = new PdfPCell(new Phrase(mskPhn.Text.Trim(), TableFont));
                                    Phone.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Phone.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(Phone);

                                    PdfPCell Address = new PdfPCell(new Phrase(sel_site_Row.SiteSTREET.Trim() + " " + sel_site_Row.SiteCITY.Trim() + " " + sel_site_Row.SiteSTATE.Trim() + " " + sel_site_Row.SiteZIP, TableFont));
                                    Address.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Address.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(Address);

                                    site_Search.ATTM_AGENCY = sel_site_Row.SiteAGENCY.Trim(); site_Search.ATTM_DEPT = sel_site_Row.SiteDEPT.Trim();
                                    site_Search.ATTM_PROG = sel_site_Row.SitePROG.Trim(); site_Search.ATTM_YEAR = sel_site_Row.SiteYEAR.Trim();
                                    site_Search.ATTM_SITE = sel_site_Row.SiteNUMBER.Trim(); //site_Search.ATTM_ROOM = REntity.SiteROOM.Trim();
                                    //site_Search.ATTM_AMPM = REntity.SiteAM_PM.Trim();
                                    SchduleList = _model.SPAdminData.Browse_CHILDATTM(site_Search, "Browse");
                                    if (SchduleList.Count > 0)
                                    {
                                        int length = 0;
                                        if (txtEndYear.Text.Trim() == txtStrtYear.Text.Trim())
                                            length = int.Parse(txtEndMnth.Text.Trim()) - int.Parse(txtStrtMnth.Text.Trim()) + 1;
                                        else
                                        {
                                            int temp = 0;
                                            temp = 12 - int.Parse(txtStrtMnth.Text.Trim()) + 1;
                                            length = int.Parse(txtEndMnth.Text.Trim()) + temp;
                                        }
                                        int j = int.Parse(txtStrtMnth.Text.Trim()); int StrtYear = int.Parse(txtStrtYear.Text.Trim());
                                        SiteScheduleEntity Selected_SchduleList = new SiteScheduleEntity();
                                        for (int i = 1; i <= length; i++)
                                        {
                                            Selected_SchduleList = SchduleList.Find(u => u.ATTM_ROOM.Trim().Equals(sel_site_Row.SiteROOM.Trim()) && u.ATTM_AMPM.Trim().Equals(sel_site_Row.SiteAM_PM.Trim()) && u.ATTM_MONTH.Trim().Equals(j.ToString()) && u.ATTM_FUND.Trim().Equals("") && u.ATTM_CALENDER_YEAR.Equals(StrtYear.ToString()));
                                            if (Selected_SchduleList != null)
                                            {
                                            }
                                            else
                                            {
                                                Selected_SchduleList = SchduleList.Find(u => u.ATTM_ROOM.Trim().Equals("") && u.ATTM_AMPM.Trim().Equals("") && u.ATTM_MONTH.Trim().Equals(j.ToString()) && u.ATTM_FUND.Trim().Equals("") && u.ATTM_CALENDER_YEAR.Equals(StrtYear.ToString()));
                                                if (Selected_SchduleList != null)
                                                {
                                                }
                                                else
                                                {
                                                    SiteScheduleEntity Site_MonthSearch = new SiteScheduleEntity(true);
                                                    List<SiteScheduleEntity> Sel_MonthSite = new List<SiteScheduleEntity>();
                                                    Site_MonthSearch.ATTM_AGENCY = sel_site_Row.SiteAGENCY.Trim(); Site_MonthSearch.ATTM_DEPT = sel_site_Row.SiteDEPT.Trim();
                                                    Site_MonthSearch.ATTM_PROG = sel_site_Row.SitePROG.Trim(); Site_MonthSearch.ATTM_YEAR = sel_site_Row.SiteYEAR.Trim();
                                                    Site_MonthSearch.ATTM_MONTH = j.ToString(); Site_MonthSearch.ATTM_CALENDER_YEAR = StrtYear.ToString(); //site_Search.ATTM_SITE = "    "; site_Search.ATTM_ROOM = "    ";
                                                    Sel_MonthSite = _model.SPAdminData.Browse_CHILDATTM(Site_MonthSearch, "Browse");
                                                    //MasterSchd = _model.SPAdminData.Browse_CHILDATTM(site_Search, "Browse");
                                                    //Selected_SchduleList = MasterSchd.Find(u => u.ATTM_SITE.Trim().Equals("") && u.ATTM_ROOM.Trim().Equals("") && u.ATTM_MONTH.Trim().Equals(j.ToString()));
                                                    Selected_SchduleList = Sel_MonthSite.Find(u => u.ATTM_SITE.Trim().Equals("") && u.ATTM_ROOM.Trim().Equals("") && u.ATTM_FUND.Trim().Equals(""));
                                                    if (Selected_SchduleList != null)
                                                    {
                                                    }
                                                }
                                            }

                                            if (Selected_SchduleList != null)
                                            {
                                                List<ChildATTMSEntity> Month_List = new List<ChildATTMSEntity>();
                                                ChildATTMSEntity Search_Month = new ChildATTMSEntity(true);
                                                Search_Month.ATTMS_ID = Selected_SchduleList.ATTM_ID.Trim();
                                                Month_List = _model.SPAdminData.Browse_CHILDATTMS(Search_Month, "Browse");
                                                if (Month_List.Count > 0)
                                                {

                                                    DateTime firstday = new DateTime(Convert.ToInt32(Selected_SchduleList.ATTM_CALENDER_YEAR.Trim()), Convert.ToInt32(Selected_SchduleList.ATTM_MONTH.Trim()), 1);
                                                    DateTime end_Month = new DateTime(Convert.ToInt32(Selected_SchduleList.ATTM_CALENDER_YEAR.Trim()), Convert.ToInt32(Selected_SchduleList.ATTM_MONTH.Trim()), DateTime.DaysInMonth(Convert.ToInt32(Selected_SchduleList.ATTM_CALENDER_YEAR.Trim()), Convert.ToInt32(Selected_SchduleList.ATTM_MONTH.Trim())));
                                                    string Strtday = firstday.DayOfWeek.ToString();
                                                    string Endday = end_Month.DayOfWeek.ToString();

                                                    foreach (ChildATTMSEntity AEntity in Month_List)
                                                    {
                                                        string Status = string.Empty;
                                                        string Stat_Extn = string.Empty;
                                                        if (Template_List.Count > 0)
                                                        {
                                                            foreach (Headstart_Template HEntity in Template_List)
                                                            {
                                                                if (HEntity.Code_Desc_Tag.Trim() == AEntity.ATTMS_STATUS.Trim())
                                                                {
                                                                    Status = HEntity.Code_Desc.Trim();
                                                                    if (sel_site_Row.SiteNUMBER.Trim() != PrivSite)
                                                                    {

                                                                        if (!FundFirst)
                                                                        {
                                                                            Fundedlist.Add(new CommonEntity(PrivSitenum, PrivSiteName, fundCnt.ToString()));
                                                                            TotFund = TotFund + fundCnt;
                                                                            fundCnt = 0;
                                                                        }
                                                                        FundFirst = false;
                                                                        PrivSite = sel_site_Row.SiteNUMBER.Trim();
                                                                        PrivSitenum = sel_site_Row.SiteNUMBER.Trim();
                                                                        PrivSiteName = sel_site_Row.SiteNAME.Trim();
                                                                    }
                                                                    //if (AEntity.ATTMS_STATUS.Trim() == "O")
                                                                    //    fundCnt++;
                                                                    if (!string.IsNullOrEmpty(HEntity.Agy_2.Trim()))
                                                                    {
                                                                        Stat_Extn = " (" + HEntity.Agy_2.Trim() + ")";
                                                                        fundCnt++; FundedDays++;
                                                                    }
                                                                    break;
                                                                }
                                                            }

                                                        }
                                                    }
                                                }
                                            }
                                            j++;
                                            if (j > 12) { j = 1; StrtYear = StrtYear + 1; }
                                        }

                                        PdfPCell Funds = new PdfPCell(new Phrase(FundedDays.ToString(), TableFont));
                                        Funds.HorizontalAlignment = Element.ALIGN_CENTER;
                                        Funds.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(Funds);
                                        //document.NewPage();
                                    }
                                    else
                                    {
                                        int length = 0;
                                        if (txtEndYear.Text.Trim() == txtStrtYear.Text.Trim())
                                            length = int.Parse(txtEndMnth.Text.Trim()) - int.Parse(txtStrtMnth.Text.Trim()) + 1;
                                        else
                                        {
                                            int temp = 0;
                                            temp = 12 - int.Parse(txtStrtMnth.Text.Trim()) + 1;
                                            length = int.Parse(txtEndMnth.Text.Trim()) + temp;
                                        }
                                        int j = int.Parse(txtStrtMnth.Text.Trim()); int StrtYear = int.Parse(txtStrtYear.Text.Trim());
                                        SiteScheduleEntity Selected_SchduleList = new SiteScheduleEntity();
                                        for (int i = 1; i <= length; i++)
                                        {
                                            SiteScheduleEntity Site_MonthSearch = new SiteScheduleEntity(true);
                                            List<SiteScheduleEntity> Sel_MonthSite = new List<SiteScheduleEntity>();
                                            Site_MonthSearch.ATTM_AGENCY = sel_site_Row.SiteAGENCY.Trim(); Site_MonthSearch.ATTM_DEPT = sel_site_Row.SiteDEPT.Trim();
                                            Site_MonthSearch.ATTM_PROG = sel_site_Row.SitePROG.Trim(); Site_MonthSearch.ATTM_YEAR = sel_site_Row.SiteYEAR.Trim();
                                            Site_MonthSearch.ATTM_MONTH = j.ToString(); Site_MonthSearch.ATTM_CALENDER_YEAR = StrtYear.ToString(); //site_Search.ATTM_SITE = "    "; site_Search.ATTM_ROOM = "    ";
                                            Sel_MonthSite = _model.SPAdminData.Browse_CHILDATTM(Site_MonthSearch, "Browse");
                                            Selected_SchduleList = Sel_MonthSite.Find(u => u.ATTM_SITE.Trim().Equals("") && u.ATTM_ROOM.Trim().Equals("") && u.ATTM_FUND.Trim().Equals(""));

                                            if (Selected_SchduleList != null)
                                            {
                                                List<ChildATTMSEntity> Month_List = new List<ChildATTMSEntity>();
                                                ChildATTMSEntity Search_Month = new ChildATTMSEntity(true);
                                                Search_Month.ATTMS_ID = Selected_SchduleList.ATTM_ID.Trim();
                                                Month_List = _model.SPAdminData.Browse_CHILDATTMS(Search_Month, "Browse");
                                                if (Month_List.Count > 0)
                                                {

                                                    DateTime firstday = new DateTime(Convert.ToInt32(Selected_SchduleList.ATTM_CALENDER_YEAR.Trim()), Convert.ToInt32(Selected_SchduleList.ATTM_MONTH.Trim()), 1);
                                                    DateTime end_Month = new DateTime(Convert.ToInt32(Selected_SchduleList.ATTM_CALENDER_YEAR.Trim()), Convert.ToInt32(Selected_SchduleList.ATTM_MONTH.Trim()), DateTime.DaysInMonth(Convert.ToInt32(Selected_SchduleList.ATTM_CALENDER_YEAR.Trim()), Convert.ToInt32(Selected_SchduleList.ATTM_MONTH.Trim())));
                                                    string Strtday = firstday.DayOfWeek.ToString();
                                                    string Endday = end_Month.DayOfWeek.ToString();

                                                    foreach (ChildATTMSEntity AEntity in Month_List)
                                                    {
                                                        string Status = string.Empty;
                                                        string Stat_Extn = string.Empty;
                                                        if (Template_List.Count > 0)
                                                        {
                                                            foreach (Headstart_Template HEntity in Template_List)
                                                            {
                                                                if (HEntity.Code_Desc_Tag.Trim() == AEntity.ATTMS_STATUS.Trim())
                                                                {
                                                                    Status = HEntity.Code_Desc.Trim();
                                                                    if (sel_site_Row.SiteNUMBER.Trim() != PrivSite)
                                                                    {

                                                                        if (!FundFirst)
                                                                        {
                                                                            Fundedlist.Add(new CommonEntity(PrivSitenum, PrivSiteName, fundCnt.ToString()));
                                                                            TotFund = TotFund + fundCnt;
                                                                            fundCnt = 0;
                                                                        }
                                                                        FundFirst = false;
                                                                        PrivSite = sel_site_Row.SiteNUMBER.Trim();
                                                                        PrivSitenum = sel_site_Row.SiteNUMBER.Trim();
                                                                        PrivSiteName = sel_site_Row.SiteNAME.Trim();
                                                                    }
                                                                    //if (AEntity.ATTMS_STATUS.Trim() == "O")
                                                                    //    fundCnt++;
                                                                    if (!string.IsNullOrEmpty(HEntity.Agy_2.Trim()))
                                                                    {
                                                                        Stat_Extn = " (" + HEntity.Agy_2.Trim() + ")";
                                                                        fundCnt++; FundedDays++;
                                                                    }
                                                                    break;
                                                                }
                                                            }

                                                        }
                                                    }
                                                }
                                            }

                                            j++;
                                            if (j > 12) { j = 1; StrtYear = StrtYear + 1; }

                                        }
                                        PdfPCell Funds = new PdfPCell(new Phrase(FundedDays.ToString(), TableFont));
                                        Funds.HorizontalAlignment = Element.ALIGN_CENTER;
                                        Funds.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(Funds);
                                    }

                                    if (Sel_REFS_List.Count == SiteCnt)
                                    {
                                        Fundedlist.Add(new CommonEntity(sel_site_Row.SiteNUMBER.Trim(), sel_site_Row.SiteNAME.Trim(), fundCnt.ToString()));
                                        TotFund = TotFund + fundCnt;
                                    }
                                }

                            }
                            if (table.Rows.Count > 0)
                            {
                                document.Add(table);
                                //table.DeleteBodyRows();
                            }

                            document.NewPage();
                            PdfPTable Funding = new PdfPTable(3);
                            Funding.TotalWidth = 400f;
                            Funding.WidthPercentage = 100;
                            Funding.LockedWidth = true;
                            float[] Funding_widths = new float[] { 12f, 60f, 15f };
                            Funding.SetWidths(Funding_widths);
                            Funding.HorizontalAlignment = Element.ALIGN_CENTER;

                            string[] header = { "Site", "Site Name", "Funded Days" };
                            for (int header_Rows = 0; header_Rows < header.Length; header_Rows++)
                            {
                                PdfPCell day = new PdfPCell(new Phrase(header[header_Rows], TblFontBold));
                                day.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                day.BackgroundColor = BaseColor.GRAY;
                                day.Border = iTextSharp.text.Rectangle.BOX;
                                Funding.AddCell(day);
                            }

                            foreach (CommonEntity FEntity in Fundedlist)
                            {
                                PdfPCell Site = new PdfPCell(new Phrase(FEntity.Code.Trim(), TableFont));
                                Site.HorizontalAlignment = Element.ALIGN_LEFT;
                                Site.Border = iTextSharp.text.Rectangle.BOX;
                                Funding.AddCell(Site);

                                PdfPCell Site_Desc = new PdfPCell(new Phrase(FEntity.Desc.Trim(), TableFont));
                                Site_Desc.HorizontalAlignment = Element.ALIGN_LEFT;
                                Site_Desc.Border = iTextSharp.text.Rectangle.BOX;
                                Funding.AddCell(Site_Desc);

                                string FundingDays = Convert.ToInt32(FEntity.Hierarchy.Trim()).ToString("N2", CultureInfo.InvariantCulture);
                                string SFund = FundingDays.Substring(0, (FundingDays.Length - 3));
                                PdfPCell FundDayas = new PdfPCell(new Phrase(SFund, TableFont));
                                FundDayas.HorizontalAlignment = Element.ALIGN_RIGHT;
                                FundDayas.Border = iTextSharp.text.Rectangle.BOX;
                                Funding.AddCell(FundDayas);
                            }
                            PdfPCell Tot_desc = new PdfPCell(new Phrase("Total Funded Days", TableFont));
                            Tot_desc.HorizontalAlignment = Element.ALIGN_RIGHT;
                            Tot_desc.Colspan = 2;
                            Tot_desc.Border = iTextSharp.text.Rectangle.BOX;
                            Funding.AddCell(Tot_desc);

                            string TotFundingDays = Convert.ToInt32(TotFund.ToString().Trim()).ToString("N2", CultureInfo.InvariantCulture);
                            string STotFund = TotFundingDays.Substring(0, (TotFundingDays.Length - 3));
                            PdfPCell Tot = new PdfPCell(new Phrase(STotFund, TableFont));
                            Tot.HorizontalAlignment = Element.ALIGN_RIGHT;
                            Tot.Border = iTextSharp.text.Rectangle.BOX;
                            Funding.AddCell(Tot);
                            document.Add(Funding);
                        }
                    }
                    else if (rbFull.Checked.Equals(true))
                    {
                        document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                        document.NewPage();

                        PdfPTable table = new PdfPTable(10);
                        table.TotalWidth = 700f;
                        table.WidthPercentage = 100;
                        table.LockedWidth = true;
                        float[] widths = new float[] { 20f, 15f, 20f, 80f, 50f, 30f, 30f, 30f, 30f, 55f };
                        table.SetWidths(widths);
                        table.HorizontalAlignment = Element.ALIGN_CENTER;

                        PdfPTable Schd_table = new PdfPTable(7);
                        Schd_table.TotalWidth = 700f;
                        Schd_table.WidthPercentage = 100;
                        Schd_table.LockedWidth = true;
                        float[] Schd_table_widths = new float[] { 80f, 80f, 80f, 80f, 80f, 80f, 80f };
                        Schd_table.SetWidths(Schd_table_widths);
                        Schd_table.HorizontalAlignment = Element.ALIGN_CENTER;

                        DataSet dsLang = DatabaseLayer.Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.LANGUAGECODES);
                        DataTable dtLang = dsLang.Tables[0];

                        DataSet dsFund = DatabaseLayer.Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.CASEMNGMTFUNDSRC, "A");
                        DataTable dtFund = dsFund.Tables[0];

                        int fundCnt = 0; string PrivSite = string.Empty; int TotFund = 0; bool FundFirst = true;
                        List<CommonEntity> Fundedlist = new List<CommonEntity>();
                        string PrivSitenum = string.Empty;
                        string PrivSiteName = string.Empty;
                        int SiteCnt = 0;
                        if (Site_list.Count > 0)
                        {
                            foreach (CaseSiteEntity SelEntity in Sel_REFS_List)
                            {
                                SiteCnt++;
                                foreach (ReportSiteEntity REntity in Site_list)
                                {
                                    string Stie_AMPM = string.Empty;
                                    if (REntity.SiteROOM.Trim() == SelEntity.SiteROOM.Trim() && REntity.SiteNUMBER.Trim() == SelEntity.SiteNUMBER.Trim())
                                    {
                                        PdfPCell Site = new PdfPCell(new Phrase(REntity.SiteNUMBER.Trim(), TableFont));
                                        Site.HorizontalAlignment = Element.ALIGN_LEFT;
                                        Site.Border = iTextSharp.text.Rectangle.TOP_BORDER; //+ iTextSharp.text.Rectangle.LEFT_BORDER;
                                        table.AddCell(Site);

                                        PdfPCell room = new PdfPCell(new Phrase(REntity.SiteROOM.Trim(), TableFont));
                                        room.HorizontalAlignment = Element.ALIGN_LEFT;
                                        room.Border = iTextSharp.text.Rectangle.TOP_BORDER;
                                        table.AddCell(room);

                                        if (REntity.SiteAM_PM == "A")
                                            Stie_AMPM = "AM";
                                        else if (REntity.SiteAM_PM == "P")
                                            Stie_AMPM = "PM";
                                        else if (REntity.SiteAM_PM == "F")
                                            Stie_AMPM = "FULL";
                                        else if (REntity.SiteAM_PM == "E")
                                            Stie_AMPM = "EXTD";
                                        PdfPCell AMPM = new PdfPCell(new Phrase(Stie_AMPM.Trim(), TableFont));
                                        AMPM.HorizontalAlignment = Element.ALIGN_LEFT;
                                        AMPM.Border = iTextSharp.text.Rectangle.TOP_BORDER;
                                        table.AddCell(AMPM);

                                        PdfPCell Name = new PdfPCell(new Phrase(REntity.SiteNAME.Trim(), TableFont));
                                        Name.HorizontalAlignment = Element.ALIGN_LEFT;
                                        Name.Border = iTextSharp.text.Rectangle.TOP_BORDER;
                                        table.AddCell(Name);

                                        PdfPCell Cat = new PdfPCell(new Phrase("Capacity  " + REntity.SITE_FUNDED_SLOTS.Trim(), TableFont));
                                        Cat.HorizontalAlignment = Element.ALIGN_LEFT;
                                        Cat.Border = iTextSharp.text.Rectangle.TOP_BORDER;
                                        table.AddCell(Cat);

                                        PdfPCell Stdt = new PdfPCell(new Phrase("Start Date ", TableFont));
                                        Stdt.HorizontalAlignment = Element.ALIGN_LEFT;
                                        Stdt.Border = iTextSharp.text.Rectangle.TOP_BORDER;
                                        table.AddCell(Stdt);

                                        PdfPCell Stdt1 = new PdfPCell(new Phrase(LookupDataAccess.Getdate(REntity.SiteCLASS_START.Trim()), TableFont));
                                        Stdt1.HorizontalAlignment = Element.ALIGN_LEFT;
                                        Stdt1.Border = iTextSharp.text.Rectangle.TOP_BORDER;
                                        table.AddCell(Stdt1);

                                        PdfPCell StTime = new PdfPCell(new Phrase("Start Time ", TableFont));
                                        StTime.HorizontalAlignment = Element.ALIGN_LEFT;
                                        StTime.Border = iTextSharp.text.Rectangle.TOP_BORDER;
                                        table.AddCell(StTime);

                                        PdfPCell StTime1 = new PdfPCell(new Phrase(LookupDataAccess.GetTime(REntity.SiteSTART_TIME.Trim()), TableFont));
                                        StTime1.HorizontalAlignment = Element.ALIGN_LEFT;
                                        StTime1.Border = iTextSharp.text.Rectangle.TOP_BORDER;
                                        table.AddCell(StTime1);

                                        //string Phone_No = string.Empty;
                                        //if (REntity.SitePHONE.Trim().Length == 10)
                                        //    Phone_No = "(" + REntity.SitePHONE.Trim().Substring(0, 3) + ")" + " " + REntity.SitePHONE.Trim().Substring(3, 3) + "-" + REntity.SitePHONE.Trim().Substring(6, 4);
                                        //else
                                        //    Phone_No = REntity.SitePHONE.Trim();
                                        MaskedTextBox mskPhn = new MaskedTextBox();
                                        mskPhn.Mask = "(999) 000-0000";
                                        if (!string.IsNullOrEmpty(REntity.SitePHONE.Trim()))
                                            mskPhn.Text = REntity.SitePHONE.Trim();
                                        PdfPCell Phone = new PdfPCell(new Phrase("Phone " + mskPhn.Text.Trim(), TableFont));
                                        Phone.HorizontalAlignment = Element.ALIGN_LEFT;
                                        Phone.Border = iTextSharp.text.Rectangle.TOP_BORDER; //+ iTextSharp.text.Rectangle.RIGHT_BORDER;
                                        table.AddCell(Phone);

                                        PdfPCell Space = new PdfPCell(new Phrase("", TableFont));
                                        Space.HorizontalAlignment = Element.ALIGN_LEFT;
                                        Space.Colspan = 3;
                                        Space.Border = iTextSharp.text.Rectangle.NO_BORDER; //iTextSharp.text.Rectangle.LEFT_BORDER;
                                        table.AddCell(Space);

                                        PdfPCell Street = new PdfPCell(new Phrase(REntity.SiteSTREET.Trim(), TableFont));
                                        Street.HorizontalAlignment = Element.ALIGN_LEFT;
                                        Street.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(Street);

                                        PdfPCell Trans = new PdfPCell(new Phrase("Trans Area " + REntity.SiteTRAN_AREA.Trim(), TableFont));
                                        Trans.HorizontalAlignment = Element.ALIGN_LEFT;
                                        Trans.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(Trans);

                                        PdfPCell EndDt = new PdfPCell(new Phrase("End Date ", TableFont));
                                        EndDt.HorizontalAlignment = Element.ALIGN_LEFT;
                                        EndDt.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(EndDt);

                                        PdfPCell EndDt1 = new PdfPCell(new Phrase(LookupDataAccess.Getdate(REntity.SiteCLASS_END.Trim()), TableFont));
                                        EndDt1.HorizontalAlignment = Element.ALIGN_LEFT;
                                        EndDt1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(EndDt1);

                                        PdfPCell EndTime = new PdfPCell(new Phrase("End Time ", TableFont));
                                        EndTime.HorizontalAlignment = Element.ALIGN_LEFT;
                                        EndTime.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(EndTime);

                                        PdfPCell EndTime1 = new PdfPCell(new Phrase(LookupDataAccess.GetTime(REntity.SiteEND_TIME.Trim()), TableFont));
                                        EndTime1.HorizontalAlignment = Element.ALIGN_LEFT;
                                        EndTime1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(EndTime1);

                                        //if (REntity.SiteFAX.Trim().Length == 10)
                                        //    Phone_No = "(" + REntity.SiteFAX.Trim().Substring(0, 3) + ")" + " " + REntity.SiteFAX.Trim().Substring(3, 3) + "-" + REntity.SiteFAX.Trim().Substring(6, 4);
                                        //else
                                        //    Phone_No = REntity.SiteFAX.Trim();
                                        MaskedTextBox mskFax = new MaskedTextBox();
                                        mskFax.Mask = "(999) 000-0000";
                                        if (!string.IsNullOrEmpty(REntity.SiteFAX.Trim()))
                                            mskFax.Text = REntity.SiteFAX.Trim();
                                        PdfPCell Fax = new PdfPCell(new Phrase("Fax " + mskFax.Text.Trim(), TableFont));
                                        Fax.HorizontalAlignment = Element.ALIGN_LEFT;
                                        Fax.Border = iTextSharp.text.Rectangle.NO_BORDER; //iTextSharp.text.Rectangle.RIGHT_BORDER;
                                        table.AddCell(Fax);

                                        PdfPCell Space1 = new PdfPCell(new Phrase("", TableFont));
                                        Space1.HorizontalAlignment = Element.ALIGN_LEFT;
                                        Space1.Colspan = 3;
                                        Space1.Border = iTextSharp.text.Rectangle.NO_BORDER; //iTextSharp.text.Rectangle.LEFT_BORDER;
                                        table.AddCell(Space1);

                                        PdfPCell City = new PdfPCell(new Phrase(REntity.SiteCITY.Trim() + " " + REntity.SiteSTATE.Trim() + " " + REntity.SiteZIP.Trim(), TableFont));
                                        City.HorizontalAlignment = Element.ALIGN_LEFT;
                                        City.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(City);

                                        PdfPCell Lang = new PdfPCell(new Phrase("Language in Class", TableFont));
                                        Lang.HorizontalAlignment = Element.ALIGN_LEFT;
                                        //Lang.Colspan = 2;
                                        Lang.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(Lang);
                                        string Language = string.Empty;
                                        if (REntity.SiteLANGUAGE.Trim() == "06")
                                            Language = REntity.SiteLANGUAGE_OTHER.Trim();
                                        else if (dtLang.Rows.Count > 0)
                                        {
                                            foreach (DataRow drLang in dtLang.Rows)
                                            {
                                                if (REntity.SiteLANGUAGE.Trim() == drLang["Code"].ToString().Trim())
                                                {
                                                    Language = drLang["LookUpDesc"].ToString().Trim();
                                                    break;
                                                }
                                            }
                                        }

                                        PdfPCell Lang1 = new PdfPCell(new Phrase(Language.Trim(), TableFont));
                                        Lang1.HorizontalAlignment = Element.ALIGN_LEFT;
                                        Lang1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(Lang1);

                                        PdfPCell Space2 = new PdfPCell(new Phrase("", TableFont));
                                        Space2.HorizontalAlignment = Element.ALIGN_LEFT;
                                        Space2.Colspan = 3;
                                        Space2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(Space2);

                                        //if (REntity.SiteOTHER_PHONE.Trim().Length == 10)
                                        //    Phone_No = "(" + REntity.SiteOTHER_PHONE.Trim().Substring(0, 3) + ")" + " " + REntity.SiteOTHER_PHONE.Trim().Substring(3, 3) + "-" + REntity.SiteOTHER_PHONE.Trim().Substring(6, 4);
                                        //else
                                        //    Phone_No = REntity.SiteOTHER_PHONE.Trim();
                                        MaskedTextBox mskOther = new MaskedTextBox();
                                        mskOther.Mask = "(999) 000-0000";
                                        if (!string.IsNullOrEmpty(REntity.SiteOTHER_PHONE.Trim()))
                                            mskOther.Text = REntity.SiteOTHER_PHONE.Trim();
                                        PdfPCell Other = new PdfPCell(new Phrase("Other " + mskOther.Text.Trim(), TableFont));
                                        Other.HorizontalAlignment = Element.ALIGN_LEFT;
                                        Other.Border = iTextSharp.text.Rectangle.NO_BORDER; //iTextSharp.text.Rectangle.RIGHT_BORDER;
                                        table.AddCell(Other);


                                        PdfPCell Space_Row = new PdfPCell(new Phrase("", TableFont));
                                        Space_Row.HorizontalAlignment = Element.ALIGN_LEFT;
                                        Space_Row.Colspan = 10;
                                        Space_Row.Border = iTextSharp.text.Rectangle.NO_BORDER; //iTextSharp.text.Rectangle.LEFT_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER;
                                        table.AddCell(Space_Row);


                                        string Mon = string.Empty, Tue = string.Empty, Wed = string.Empty, Thu = string.Empty, Fri = string.Empty, Sat = string.Empty, Sun = string.Empty;
                                        string meals = REntity.SiteMEAL_AREA.Trim();
                                        for (int i = 0; i < meals.Length; i++)
                                        {
                                            if (meals.Substring(i, 1).ToString().Trim() == "Y")
                                            {
                                                switch (i)
                                                {
                                                    case 0: Mon += "B"; break;
                                                    case 1: Tue += "B"; break;
                                                    case 2: Wed += "B"; break;
                                                    case 3: Thu += "B"; break;
                                                    case 4: Fri += "B"; break;
                                                    case 5: Sat += "B"; break;
                                                    case 6: Sun += "B"; break;
                                                    case 7: Mon += "A"; break;
                                                    case 8: Tue += "A"; break;
                                                    case 9: Wed += "A"; break;
                                                    case 10: Thu += "A"; break;
                                                    case 11: Fri += "A"; break;
                                                    case 12: Sat += "A"; break;
                                                    case 13: Sun += "A"; break;
                                                    case 14: Mon += "L"; break;
                                                    case 15: Tue += "L"; break;
                                                    case 16: Wed += "L"; break;
                                                    case 17: Thu += "L"; break;
                                                    case 18: Fri += "L"; break;
                                                    case 19: Sat += "L"; break;
                                                    case 20: Sun += "L"; break;
                                                    case 21: Mon += "P"; break;
                                                    case 22: Tue += "P"; break;
                                                    case 23: Wed += "P"; break;
                                                    case 24: Thu += "P"; break;
                                                    case 25: Fri += "P"; break;
                                                    case 26: Sat += "P"; break;
                                                    case 27: Sun += "P"; break;
                                                    case 28: Mon += "S"; break;
                                                    case 29: Tue += "S"; break;
                                                    case 30: Wed += "S"; break;
                                                    case 31: Thu += "S"; break;
                                                    case 32: Fri += "S"; break;
                                                    case 33: Sat += "S"; break;
                                                    case 34: Sun += "S"; break;
                                                }
                                            }
                                        }

                                        PdfPCell Space_Row1 = new PdfPCell(new Phrase("", TableFont));
                                        Space_Row1.HorizontalAlignment = Element.ALIGN_LEFT;
                                        Space_Row1.Colspan = 10;
                                        Space_Row1.Border = iTextSharp.text.Rectangle.NO_BORDER; //iTextSharp.text.Rectangle.LEFT_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER;
                                        table.AddCell(Space_Row1);

                                        PdfPCell Meals = new PdfPCell(new Phrase("Meals: ", TableFont));
                                        Meals.HorizontalAlignment = Element.ALIGN_LEFT;
                                        Meals.Colspan = 3;
                                        Meals.Border = iTextSharp.text.Rectangle.NO_BORDER; //iTextSharp.text.Rectangle.LEFT_BORDER;
                                        table.AddCell(Meals);

                                        if (string.IsNullOrEmpty(Mon.Trim())) Mon = "None";
                                        if (string.IsNullOrEmpty(Tue.Trim())) Tue = "None"; if (string.IsNullOrEmpty(Wed.Trim())) Wed = "None";
                                        if (string.IsNullOrEmpty(Thu.Trim())) Thu = "None"; if (string.IsNullOrEmpty(Fri.Trim())) Fri = "None";
                                        if (string.IsNullOrEmpty(Sat.Trim())) Sat = "None"; if (string.IsNullOrEmpty(Sun.Trim())) Sun = "None";

                                        PdfPCell Weeks = new PdfPCell(new Phrase("Mon: " + Mon.Trim() + "       " + "Tue: " + Tue.Trim() + "       " + "Wed: " + Wed.Trim() + "       " + "Thu: " + Thu.Trim() + "       " + "Fri: " + Fri.Trim() + "       " + "Sat: " + Sat.Trim() + "       " + "Sun: " + Sun.Trim(), TableFont));
                                        Weeks.HorizontalAlignment = Element.ALIGN_LEFT;
                                        Weeks.Colspan = 7;
                                        Weeks.Border = iTextSharp.text.Rectangle.NO_BORDER;//iTextSharp.text.Rectangle.RIGHT_BORDER;
                                        table.AddCell(Weeks);

                                        PdfPCell Empty = new PdfPCell(new Phrase("", TableFont));
                                        Empty.HorizontalAlignment = Element.ALIGN_LEFT;
                                        Empty.Colspan = 10;
                                        Empty.FixedHeight = 10f;
                                        Empty.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;// iTextSharp.text.Rectangle.LEFT_BORDER + iTextSharp.text.Rectangle.RIGHT_BORDER + iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                        table.AddCell(Empty);

                                        site_Search.ATTM_AGENCY = REntity.SiteAGENCY.Trim(); site_Search.ATTM_DEPT = REntity.SiteDEPT.Trim();
                                        site_Search.ATTM_PROG = REntity.SitePROG.Trim(); site_Search.ATTM_YEAR = REntity.SiteYEAR.Trim();
                                        site_Search.ATTM_SITE = REntity.SiteNUMBER.Trim(); //site_Search.ATTM_ROOM = REntity.SiteROOM.Trim();
                                        //site_Search.ATTM_AMPM = REntity.SiteAM_PM.Trim();
                                        SchduleList = _model.SPAdminData.Browse_CHILDATTM(site_Search, "Browse");
                                        if (SchduleList.Count > 0)
                                        {
                                            int length = 0;
                                            if (txtEndYear.Text.Trim() == txtStrtYear.Text.Trim())
                                                length = int.Parse(txtEndMnth.Text.Trim()) - int.Parse(txtStrtMnth.Text.Trim()) + 1;
                                            else
                                            {
                                                int temp = 0;
                                                temp = 12 - int.Parse(txtStrtMnth.Text.Trim()) + 1;
                                                length = int.Parse(txtEndMnth.Text.Trim()) + temp;
                                            }
                                            int j = int.Parse(txtStrtMnth.Text.Trim()); int StrtYear = int.Parse(txtStrtYear.Text.Trim());
                                            List<SiteScheduleEntity> Selected_SchduleList = new List<SiteScheduleEntity>();
                                            for (int i = 1; i <= length; i++)
                                            {
                                                Selected_SchduleList = SchduleList.FindAll(u => u.ATTM_ROOM.Trim().Equals(REntity.SiteROOM.Trim()) && u.ATTM_AMPM.Trim().Equals(REntity.SiteAM_PM.Trim()) && u.ATTM_MONTH.Trim().Equals(j.ToString()) && u.ATTM_FUND.Trim().Equals("") && u.ATTM_CALENDER_YEAR.Equals(StrtYear.ToString()));
                                                if (Selected_SchduleList.Count > 0)
                                                {
                                                    Selected_SchduleList = SchduleList.FindAll(u => u.ATTM_ROOM.Trim().Equals(REntity.SiteROOM.Trim()) && u.ATTM_AMPM.Trim().Equals(REntity.SiteAM_PM.Trim()) && u.ATTM_MONTH.Trim().Equals(j.ToString()) && u.ATTM_CALENDER_YEAR.Equals(StrtYear.ToString()));
                                                }
                                                else
                                                {
                                                    Selected_SchduleList = SchduleList.FindAll(u => u.ATTM_ROOM.Trim().Equals("") && u.ATTM_AMPM.Trim().Equals("") && u.ATTM_MONTH.Trim().Equals(j.ToString()) && u.ATTM_FUND.Trim().Equals("") && u.ATTM_CALENDER_YEAR.Equals(StrtYear.ToString()));
                                                    if (Selected_SchduleList.Count > 0)
                                                    {
                                                        Selected_SchduleList = SchduleList.FindAll(u => u.ATTM_ROOM.Trim().Equals("") && u.ATTM_AMPM.Trim().Equals("") && u.ATTM_MONTH.Trim().Equals(j.ToString()) && u.ATTM_CALENDER_YEAR.Equals(StrtYear.ToString()));
                                                    }
                                                    else
                                                    {
                                                        SiteScheduleEntity Site_MonthSearch = new SiteScheduleEntity(true);
                                                        List<SiteScheduleEntity> Sel_MonthSite = new List<SiteScheduleEntity>();
                                                        Site_MonthSearch.ATTM_AGENCY = REntity.SiteAGENCY.Trim(); Site_MonthSearch.ATTM_DEPT = REntity.SiteDEPT.Trim();
                                                        Site_MonthSearch.ATTM_PROG = REntity.SitePROG.Trim(); Site_MonthSearch.ATTM_YEAR = REntity.SiteYEAR.Trim();
                                                        Site_MonthSearch.ATTM_MONTH = j.ToString(); Site_MonthSearch.ATTM_CALENDER_YEAR = StrtYear.ToString();  //site_Search.ATTM_SITE = "    "; site_Search.ATTM_ROOM = "    ";
                                                        Sel_MonthSite = _model.SPAdminData.Browse_CHILDATTM(Site_MonthSearch, "Browse");
                                                        //MasterSchd = _model.SPAdminData.Browse_CHILDATTM(site_Search, "Browse");
                                                        //Selected_SchduleList = MasterSchd.Find(u => u.ATTM_SITE.Trim().Equals("") && u.ATTM_ROOM.Trim().Equals("") && u.ATTM_MONTH.Trim().Equals(j.ToString()));
                                                        Selected_SchduleList = Sel_MonthSite.FindAll(u => u.ATTM_SITE.Trim().Equals("") && u.ATTM_ROOM.Trim().Equals(""));
                                                        //if (Selected_SchduleList.Count > 0)
                                                        //{
                                                        //}
                                                    }
                                                }
                                                if (Selected_SchduleList.Count > 0)
                                                {
                                                    SiteScheduleEntity FundList = Selected_SchduleList.Find(u => u.ATTM_FUND.Trim().Equals(""));
                                                    Selected_SchduleList = Selected_SchduleList.OrderBy(u => u.ATTM_FUND).ToList();
                                                    bool First = true; string Second = string.Empty;
                                                    if (FundList != null)
                                                    {
                                                        foreach (SiteScheduleEntity SEntity in Selected_SchduleList)
                                                        {
                                                            List<ChildATTMSEntity> Month_List = new List<ChildATTMSEntity>();
                                                            ChildATTMSEntity Search_Month = new ChildATTMSEntity(true);
                                                            Search_Month.ATTMS_ID = SEntity.ATTM_ID.Trim();
                                                            Month_List = _model.SPAdminData.Browse_CHILDATTMS(Search_Month, "Browse");
                                                            if (Month_List.Count > 0)
                                                            {
                                                                if (First)
                                                                {
                                                                    PdfPCell Month = new PdfPCell(new Phrase(GetMonth(SEntity.ATTM_MONTH.Trim()) + " " + SEntity.ATTM_CALENDER_YEAR.Trim(), TblFontBold));
                                                                    Month.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                    Month.Colspan = 7;
                                                                    Month.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                    Schd_table.AddCell(Month);
                                                                    First = false;
                                                                }
                                                                else
                                                                {
                                                                    string Fund = string.Empty;
                                                                    if (!string.IsNullOrEmpty(SEntity.ATTM_FUND.Trim()))
                                                                    {
                                                                        foreach (DataRow drFund in dtFund.Rows)
                                                                        {
                                                                            if (SEntity.ATTM_FUND.Trim() == drFund["Code"].ToString().Trim())
                                                                            {
                                                                                Fund += drFund["LookUpDesc"].ToString().Trim();
                                                                                break;
                                                                            }
                                                                        }
                                                                    }
                                                                    Second = "Except ";

                                                                    PdfPCell Month = new PdfPCell(new Phrase("Except " + Fund, TblFontBold));
                                                                    Month.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                    Month.Colspan = 7;
                                                                    Month.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                    Schd_table.AddCell(Month);
                                                                }



                                                                string[] Weekdays = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

                                                                for (int Week = 0; Week < Weekdays.Length; Week++)
                                                                {
                                                                    PdfPCell day = new PdfPCell(new Phrase(Weekdays[Week], TableFont));
                                                                    day.HorizontalAlignment = Element.ALIGN_CENTER;
                                                                    day.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                    Schd_table.AddCell(day);
                                                                }
                                                                DateTime firstday = new DateTime(Convert.ToInt32(SEntity.ATTM_CALENDER_YEAR.Trim()), Convert.ToInt32(SEntity.ATTM_MONTH.Trim()), 1);
                                                                DateTime end_Month = new DateTime(Convert.ToInt32(SEntity.ATTM_CALENDER_YEAR.Trim()), Convert.ToInt32(SEntity.ATTM_MONTH.Trim()), DateTime.DaysInMonth(Convert.ToInt32(SEntity.ATTM_CALENDER_YEAR.Trim()), Convert.ToInt32(SEntity.ATTM_MONTH.Trim())));
                                                                string Strtday = firstday.DayOfWeek.ToString();
                                                                string Endday = end_Month.DayOfWeek.ToString();

                                                                switch (Strtday)
                                                                {
                                                                    case "Monday": break;
                                                                    case "Tuesday": PdfPCell space = new PdfPCell(new Phrase("", TableFont));
                                                                        space.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                        space.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                        Schd_table.AddCell(space); break;
                                                                    case "Wednesday": PdfPCell Wspace = new PdfPCell(new Phrase("", TableFont));
                                                                        Wspace.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                        Wspace.Colspan = 2;
                                                                        Wspace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                        Schd_table.AddCell(Wspace);
                                                                        break;
                                                                    case "Thursday": PdfPCell Tspace = new PdfPCell(new Phrase("", TableFont));
                                                                        Tspace.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                        Tspace.Colspan = 3;
                                                                        Tspace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                        Schd_table.AddCell(Tspace); break;
                                                                    case "Friday": PdfPCell Fspace = new PdfPCell(new Phrase("", TableFont));
                                                                        Fspace.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                        Fspace.Colspan = 4;
                                                                        Fspace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                        Schd_table.AddCell(Fspace);
                                                                        break;
                                                                    case "Saturday": PdfPCell Satspace = new PdfPCell(new Phrase("", TableFont));
                                                                        Satspace.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                        Satspace.Colspan = 5;
                                                                        Satspace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                        Schd_table.AddCell(Satspace);
                                                                        break;
                                                                    case "Sunday": PdfPCell Sunspace = new PdfPCell(new Phrase("", TableFont));
                                                                        Sunspace.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                        Sunspace.Colspan = 6;
                                                                        Sunspace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                        Schd_table.AddCell(Sunspace);
                                                                        break;
                                                                }
                                                                foreach (ChildATTMSEntity AEntity in Month_List)
                                                                {
                                                                    string Status = string.Empty;
                                                                    string Stat_Extn = string.Empty;
                                                                    if (Template_List.Count > 0)
                                                                    {
                                                                        foreach (Headstart_Template Entity in Template_List)
                                                                        {
                                                                            if (Entity.Code_Desc_Tag.Trim() == AEntity.ATTMS_STATUS.Trim())
                                                                            {
                                                                                Status = Entity.Code_Desc.Trim();
                                                                                if (REntity.SiteNUMBER.Trim() != PrivSite)
                                                                                {

                                                                                    if (!FundFirst)
                                                                                    {
                                                                                        Fundedlist.Add(new CommonEntity(PrivSitenum, PrivSiteName, fundCnt.ToString()));
                                                                                        TotFund = TotFund + fundCnt;
                                                                                        fundCnt = 0;
                                                                                    }
                                                                                    FundFirst = false;
                                                                                    PrivSite = REntity.SiteNUMBER.Trim();
                                                                                    PrivSitenum = REntity.SiteNUMBER.Trim();
                                                                                    PrivSiteName = REntity.SiteNAME.Trim();
                                                                                }
                                                                                //if (AEntity.ATTMS_STATUS.Trim() == "O")
                                                                                //    fundCnt++;
                                                                                if (string.IsNullOrEmpty(Second.Trim()))
                                                                                {
                                                                                    if (!string.IsNullOrEmpty(Entity.Agy_2.Trim()))
                                                                                    {
                                                                                        Stat_Extn = " (" + Entity.Agy_2.Trim() + ")";
                                                                                        fundCnt++;
                                                                                    }
                                                                                }
                                                                                break;

                                                                            }
                                                                        }

                                                                    }

                                                                    PdfPCell Day_Desc = new PdfPCell(new Phrase(AEntity.ATTMS_DAY.Trim() + "  " + Status.Trim() + Stat_Extn, TableFont));
                                                                    Day_Desc.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                    Day_Desc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                    Schd_table.AddCell(Day_Desc);

                                                                }

                                                                switch (Endday)
                                                                {
                                                                    case "Sunday": break;
                                                                    case "Saturday": PdfPCell space = new PdfPCell(new Phrase("", TableFont));
                                                                        space.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                        space.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                        Schd_table.AddCell(space); break;
                                                                    case "Friday": PdfPCell Wspace = new PdfPCell(new Phrase("", TableFont));
                                                                        Wspace.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                        Wspace.Colspan = 2;
                                                                        Wspace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                        Schd_table.AddCell(Wspace);
                                                                        break;
                                                                    case "Thursday": PdfPCell Tspace = new PdfPCell(new Phrase("", TableFont));
                                                                        Tspace.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                        Tspace.Colspan = 3;
                                                                        Tspace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                        Schd_table.AddCell(Tspace); break;
                                                                    case "Wednesday": PdfPCell Fspace = new PdfPCell(new Phrase("", TableFont));
                                                                        Fspace.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                        Fspace.Colspan = 4;
                                                                        Fspace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                        Schd_table.AddCell(Fspace);
                                                                        break;
                                                                    case "Tuesday": PdfPCell Satspace = new PdfPCell(new Phrase("", TableFont));
                                                                        Satspace.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                        Satspace.Colspan = 5;
                                                                        Satspace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                        Schd_table.AddCell(Satspace);
                                                                        break;
                                                                    case "Monday": PdfPCell Sunspace = new PdfPCell(new Phrase("", TableFont));
                                                                        Sunspace.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                        Sunspace.Colspan = 6;
                                                                        Sunspace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                        Schd_table.AddCell(Sunspace);
                                                                        break;
                                                                }

                                                            }

                                                            PdfPCell line = new PdfPCell(new Phrase("", TableFont));
                                                            line.HorizontalAlignment = Element.ALIGN_LEFT;
                                                            line.Colspan = 7;
                                                            line.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                            Schd_table.AddCell(line);
                                                        }
                                                    }
                                                }
                                                j++;
                                                if (j > 12) { j = 1; StrtYear = StrtYear + 1; }
                                            }


                                            PdfPCell line1 = new PdfPCell(new Phrase("", TableFont));
                                            line1.HorizontalAlignment = Element.ALIGN_LEFT;
                                            line1.Colspan = 7;
                                            line1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                            Schd_table.AddCell(line1);

                                            if (table.Rows.Count > 0)
                                            {
                                                document.Add(table);
                                                table.DeleteBodyRows();
                                            }
                                            if (Schd_table.Rows.Count > 0)
                                            {
                                                document.Add(Schd_table);
                                                Schd_table.DeleteBodyRows();
                                            }
                                            document.NewPage();
                                            //PdfPCell line = new PdfPCell(new Phrase("", TableFont));
                                            //line.HorizontalAlignment = Element.ALIGN_LEFT;
                                            //line.Colspan = 7;
                                            //line.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                            //Schd_table.AddCell(line);
                                        }
                                        else
                                        {
                                            int length = 0;
                                            if (txtEndYear.Text.Trim() == txtStrtYear.Text.Trim())
                                                length = int.Parse(txtEndMnth.Text.Trim()) - int.Parse(txtStrtMnth.Text.Trim()) + 1;
                                            else
                                            {
                                                int temp = 0;
                                                temp = 12 - int.Parse(txtStrtMnth.Text.Trim()) + 1;
                                                length = int.Parse(txtEndMnth.Text.Trim()) + temp;
                                            }
                                            int j = int.Parse(txtStrtMnth.Text.Trim()); int StrtYear = int.Parse(txtStrtYear.Text.Trim());
                                            List<SiteScheduleEntity> Selected_SchduleList = new List<SiteScheduleEntity>();
                                            for (int i = 1; i <= length; i++)
                                            {
                                                SiteScheduleEntity Site_MonthSearch = new SiteScheduleEntity(true);
                                                List<SiteScheduleEntity> Sel_MonthSite = new List<SiteScheduleEntity>();
                                                Site_MonthSearch.ATTM_AGENCY = REntity.SiteAGENCY.Trim(); Site_MonthSearch.ATTM_DEPT = REntity.SiteDEPT.Trim();
                                                Site_MonthSearch.ATTM_PROG = REntity.SitePROG.Trim(); Site_MonthSearch.ATTM_YEAR = REntity.SiteYEAR.Trim();
                                                Site_MonthSearch.ATTM_MONTH = j.ToString(); Site_MonthSearch.ATTM_CALENDER_YEAR = StrtYear.ToString(); //site_Search.ATTM_SITE = "    "; site_Search.ATTM_ROOM = "    ";
                                                Sel_MonthSite = _model.SPAdminData.Browse_CHILDATTM(Site_MonthSearch, "Browse");
                                                //MasterSchd = _model.SPAdminData.Browse_CHILDATTM(site_Search, "Browse");
                                                //Selected_SchduleList = MasterSchd.Find(u => u.ATTM_SITE.Trim().Equals("") && u.ATTM_ROOM.Trim().Equals("") && u.ATTM_MONTH.Trim().Equals(j.ToString()));
                                                Selected_SchduleList = Sel_MonthSite.FindAll(u => u.ATTM_SITE.Trim().Equals("") && u.ATTM_ROOM.Trim().Equals(""));
                                                //if (Selected_SchduleList != null)
                                                //{
                                                //}


                                                if (Selected_SchduleList.Count > 0)
                                                {
                                                    SiteScheduleEntity FundList = Selected_SchduleList.Find(u => u.ATTM_FUND.Trim().Equals(""));
                                                    Selected_SchduleList = Selected_SchduleList.OrderBy(u => u.ATTM_FUND).ToList();
                                                    bool First = true; string Second = string.Empty;
                                                    if (FundList != null)
                                                    {
                                                        foreach (SiteScheduleEntity Mast in Selected_SchduleList)
                                                        {
                                                            List<ChildATTMSEntity> Month_List = new List<ChildATTMSEntity>();
                                                            ChildATTMSEntity Search_Month = new ChildATTMSEntity(true);
                                                            Search_Month.ATTMS_ID = Mast.ATTM_ID.Trim();
                                                            Month_List = _model.SPAdminData.Browse_CHILDATTMS(Search_Month, "Browse");
                                                            if (Month_List.Count > 0)
                                                            {
                                                                if (First)
                                                                {
                                                                    PdfPCell Month = new PdfPCell(new Phrase(GetMonth(Mast.ATTM_MONTH.Trim()) + " " + Mast.ATTM_CALENDER_YEAR.Trim(), TblFontBold));
                                                                    Month.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                    Month.Colspan = 7;
                                                                    Month.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                    Schd_table.AddCell(Month);
                                                                    First = false;
                                                                }
                                                                else
                                                                {
                                                                    string Fund = string.Empty;
                                                                    if (!string.IsNullOrEmpty(Mast.ATTM_FUND.Trim()))
                                                                    {
                                                                        foreach (DataRow drFund in dtFund.Rows)
                                                                        {
                                                                            if (Mast.ATTM_FUND.Trim() == drFund["Code"].ToString().Trim())
                                                                            {
                                                                                Fund += drFund["LookUpDesc"].ToString().Trim();
                                                                                break;
                                                                            }
                                                                        }
                                                                    }
                                                                    Second = "Except ";

                                                                    PdfPCell Month = new PdfPCell(new Phrase("Except " + Fund, TblFontBold));
                                                                    Month.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                    Month.Colspan = 7;
                                                                    Month.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                    Schd_table.AddCell(Month);
                                                                }

                                                                string[] Weekdays = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

                                                                for (int Week = 0; Week < Weekdays.Length; Week++)
                                                                {
                                                                    PdfPCell day = new PdfPCell(new Phrase(Weekdays[Week], TableFont));
                                                                    day.HorizontalAlignment = Element.ALIGN_CENTER;
                                                                    day.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                    Schd_table.AddCell(day);
                                                                }
                                                                DateTime firstday = new DateTime(Convert.ToInt32(Mast.ATTM_CALENDER_YEAR.Trim()), Convert.ToInt32(Mast.ATTM_MONTH.Trim()), 1);
                                                                DateTime end_Month = new DateTime(Convert.ToInt32(Mast.ATTM_CALENDER_YEAR.Trim()), Convert.ToInt32(Mast.ATTM_MONTH.Trim()), DateTime.DaysInMonth(Convert.ToInt32(Mast.ATTM_CALENDER_YEAR.Trim()), Convert.ToInt32(Mast.ATTM_MONTH.Trim())));
                                                                string Strtday = firstday.DayOfWeek.ToString();
                                                                string Endday = end_Month.DayOfWeek.ToString();

                                                                switch (Strtday)
                                                                {
                                                                    case "Monday": break;
                                                                    case "Tuesday": PdfPCell space = new PdfPCell(new Phrase("", TableFont));
                                                                        space.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                        space.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                        Schd_table.AddCell(space); break;
                                                                    case "Wednesday": PdfPCell Wspace = new PdfPCell(new Phrase("", TableFont));
                                                                        Wspace.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                        Wspace.Colspan = 2;
                                                                        Wspace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                        Schd_table.AddCell(Wspace);
                                                                        break;
                                                                    case "Thursday": PdfPCell Tspace = new PdfPCell(new Phrase("", TableFont));
                                                                        Tspace.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                        Tspace.Colspan = 3;
                                                                        Tspace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                        Schd_table.AddCell(Tspace); break;
                                                                    case "Friday": PdfPCell Fspace = new PdfPCell(new Phrase("", TableFont));
                                                                        Fspace.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                        Fspace.Colspan = 4;
                                                                        Fspace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                        Schd_table.AddCell(Fspace);
                                                                        break;
                                                                    case "Saturday": PdfPCell Satspace = new PdfPCell(new Phrase("", TableFont));
                                                                        Satspace.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                        Satspace.Colspan = 5;
                                                                        Satspace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                        Schd_table.AddCell(Satspace);
                                                                        break;
                                                                    case "Sunday": PdfPCell Sunspace = new PdfPCell(new Phrase("", TableFont));
                                                                        Sunspace.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                        Sunspace.Colspan = 6;
                                                                        Sunspace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                        Schd_table.AddCell(Sunspace);
                                                                        break;
                                                                }
                                                                foreach (ChildATTMSEntity AEntity in Month_List)
                                                                {
                                                                    string Status = string.Empty;
                                                                    string Stat_Extn = string.Empty;
                                                                    if (Template_List.Count > 0)
                                                                    {
                                                                        foreach (Headstart_Template Entity in Template_List)
                                                                        {
                                                                            if (Entity.Code_Desc_Tag.Trim() == AEntity.ATTMS_STATUS.Trim())
                                                                            {
                                                                                Status = Entity.Code_Desc.Trim();
                                                                                if (REntity.SiteNUMBER.Trim() != PrivSite)
                                                                                {

                                                                                    if (!FundFirst)
                                                                                    {
                                                                                        Fundedlist.Add(new CommonEntity(PrivSitenum, PrivSiteName, fundCnt.ToString()));
                                                                                        TotFund = TotFund + fundCnt;
                                                                                        fundCnt = 0;
                                                                                    }
                                                                                    FundFirst = false;
                                                                                    PrivSite = REntity.SiteNUMBER.Trim();
                                                                                    PrivSitenum = REntity.SiteNUMBER.Trim();
                                                                                    PrivSiteName = REntity.SiteNAME.Trim();
                                                                                }
                                                                                //if (AEntity.ATTMS_STATUS.Trim() == "O")
                                                                                //    fundCnt++;
                                                                                if (string.IsNullOrEmpty(Second.Trim()))
                                                                                {
                                                                                    if (!string.IsNullOrEmpty(Entity.Agy_2.Trim()))
                                                                                    {
                                                                                        Stat_Extn = " (" + Entity.Agy_2.Trim() + ")";
                                                                                        fundCnt++;
                                                                                    }
                                                                                }
                                                                                break;

                                                                            }
                                                                        }

                                                                    }

                                                                    PdfPCell Day_Desc = new PdfPCell(new Phrase(AEntity.ATTMS_DAY.Trim() + "  " + Status.Trim() + Stat_Extn, TableFont));
                                                                    Day_Desc.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                    Day_Desc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                    Schd_table.AddCell(Day_Desc);

                                                                }

                                                                switch (Endday)
                                                                {
                                                                    case "Sunday": break;
                                                                    case "Saturday": PdfPCell space = new PdfPCell(new Phrase("", TableFont));
                                                                        space.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                        space.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                        Schd_table.AddCell(space); break;
                                                                    case "Friday": PdfPCell Wspace = new PdfPCell(new Phrase("", TableFont));
                                                                        Wspace.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                        Wspace.Colspan = 2;
                                                                        Wspace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                        Schd_table.AddCell(Wspace);
                                                                        break;
                                                                    case "Thursday": PdfPCell Tspace = new PdfPCell(new Phrase("", TableFont));
                                                                        Tspace.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                        Tspace.Colspan = 3;
                                                                        Tspace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                        Schd_table.AddCell(Tspace); break;
                                                                    case "Wednesday": PdfPCell Fspace = new PdfPCell(new Phrase("", TableFont));
                                                                        Fspace.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                        Fspace.Colspan = 4;
                                                                        Fspace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                        Schd_table.AddCell(Fspace);
                                                                        break;
                                                                    case "Tuesday": PdfPCell Satspace = new PdfPCell(new Phrase("", TableFont));
                                                                        Satspace.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                        Satspace.Colspan = 5;
                                                                        Satspace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                        Schd_table.AddCell(Satspace);
                                                                        break;
                                                                    case "Monday": PdfPCell Sunspace = new PdfPCell(new Phrase("", TableFont));
                                                                        Sunspace.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                        Sunspace.Colspan = 6;
                                                                        Sunspace.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                        Schd_table.AddCell(Sunspace);
                                                                        break;
                                                                }

                                                            }

                                                            PdfPCell line = new PdfPCell(new Phrase("", TableFont));
                                                            line.HorizontalAlignment = Element.ALIGN_LEFT;
                                                            line.Colspan = 7;
                                                            line.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                            Schd_table.AddCell(line);
                                                        }
                                                    }
                                                }
                                                j++;
                                                if (j > 12) { j = 1; StrtYear = StrtYear + 1; }
                                            }


                                            PdfPCell line1 = new PdfPCell(new Phrase("", TableFont));
                                            line1.HorizontalAlignment = Element.ALIGN_LEFT;
                                            line1.Colspan = 7;
                                            line1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                                            Schd_table.AddCell(line1);

                                            if (table.Rows.Count > 0)
                                            {
                                                document.Add(table);
                                                table.DeleteBodyRows();
                                            }
                                            if (Schd_table.Rows.Count > 0)
                                            {
                                                document.Add(Schd_table);
                                                Schd_table.DeleteBodyRows();
                                            }
                                            document.NewPage();
                                        }
                                        if (Sel_REFS_List.Count == SiteCnt)
                                        {
                                            Fundedlist.Add(new CommonEntity(REntity.SiteNUMBER.Trim(), REntity.SiteNAME.Trim(), fundCnt.ToString()));
                                            TotFund = TotFund + fundCnt;
                                        }
                                    }

                                }
                                document.NewPage();
                                PdfPTable Funding = new PdfPTable(3);
                                Funding.TotalWidth = 400f;
                                Funding.WidthPercentage = 100;
                                Funding.LockedWidth = true;
                                float[] Funding_widths = new float[] { 12f, 60f, 15f };
                                Funding.SetWidths(Funding_widths);
                                Funding.HorizontalAlignment = Element.ALIGN_CENTER;

                                string[] header = { "Site", "Site Name", "Funded Days" };
                                for (int header_Rows = 0; header_Rows < header.Length; header_Rows++)
                                {
                                    PdfPCell day = new PdfPCell(new Phrase(header[header_Rows], TblFontBold));
                                    day.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                                    day.BackgroundColor = BaseColor.GRAY;
                                    day.Border = iTextSharp.text.Rectangle.BOX;
                                    Funding.AddCell(day);
                                }

                                foreach (CommonEntity FEntity in Fundedlist)
                                {
                                    PdfPCell Site = new PdfPCell(new Phrase(FEntity.Code.Trim(), TableFont));
                                    Site.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Site.Border = iTextSharp.text.Rectangle.BOX;
                                    Funding.AddCell(Site);

                                    PdfPCell Site_Desc = new PdfPCell(new Phrase(FEntity.Desc.Trim(), TableFont));
                                    Site_Desc.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Site_Desc.Border = iTextSharp.text.Rectangle.BOX;
                                    Funding.AddCell(Site_Desc);

                                    string FundingDays = Convert.ToInt32(FEntity.Hierarchy.Trim()).ToString("N2", CultureInfo.InvariantCulture);
                                    string SFund = FundingDays.Substring(0, (FundingDays.Length - 3));
                                    PdfPCell FundDayas = new PdfPCell(new Phrase(SFund, TableFont));
                                    FundDayas.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    FundDayas.Border = iTextSharp.text.Rectangle.BOX;
                                    Funding.AddCell(FundDayas);
                                }
                                PdfPCell Tot_desc = new PdfPCell(new Phrase("Total Funded Days", TableFont));
                                Tot_desc.HorizontalAlignment = Element.ALIGN_RIGHT;
                                Tot_desc.Colspan = 2;
                                Tot_desc.Border = iTextSharp.text.Rectangle.BOX;
                                Funding.AddCell(Tot_desc);

                                string TotFundingDays = Convert.ToInt32(TotFund.ToString().Trim()).ToString("N2", CultureInfo.InvariantCulture);
                                string STotFund = TotFundingDays.Substring(0, (TotFundingDays.Length - 3));
                                PdfPCell Tot = new PdfPCell(new Phrase(STotFund, TableFont));
                                Tot.HorizontalAlignment = Element.ALIGN_RIGHT;
                                Tot.Border = iTextSharp.text.Rectangle.BOX;
                                Funding.AddCell(Tot);
                                document.Add(Funding);
                            }
                        }
                    }
                }
                

                    //document.Add(table);
                    //document.Add(Schd_table);
            
                document.Close();
                fs.Close();
                fs.Dispose();
                AlertBox.Show("Report Generated Successfully");
        }
    }


        private string GetMonth(String month)
        {
            string month_name = null;
            switch (month)
            {
                case "1": month_name = "January"; break;
                case "2": month_name = "February"; break;
                case "3": month_name = "March"; break;
                case "4": month_name = "April"; break;
                case "5": month_name = "May"; break;
                case "6": month_name = "June"; break;
                case "7": month_name = "July"; break;
                case "8": month_name = "August"; break;
                case "9": month_name = "September"; break;
                case "10": month_name = "October"; break;
                case "11": month_name = "November"; break;
                case "12": month_name = "December"; break;
            }
            return month_name;
        }

        List<Headstart_Template> Template_List = new List<Headstart_Template>();
        private void GetHeadstartTemplate_Values()
        {
            Template_List = _model.CaseMstData.GetHeadstartTemplate(Consts.AgyTab.HSCALENDARDAYSTATS, "00000");
        }

        List<CaseSiteEntity> Sel_REFS_List = new List<CaseSiteEntity>();
        private void On_Room_Select_Closed(object sender, FormClosedEventArgs e)
        {
            Site_SelectionForm form = sender as Site_SelectionForm;
            if (form.DialogResult == DialogResult.OK)
            {
                Sel_REFS_List = form.GetSelected_Sites();
            }
        }

        private void rbSelectedSite_Click(object sender, EventArgs e)
        {
            if (rbSelectedSite.Checked == true)
            {
                Site_SelectionForm SiteSelection = new Site_SelectionForm(BaseForm, "Room", Sel_REFS_List, "Report", Agency.Trim(), Depart.Trim(), Program.Trim(), Program_Year, Privileges);
                SiteSelection.FormClosed += new FormClosedEventHandler(On_Room_Select_Closed);
                SiteSelection.StartPosition = FormStartPosition.CenterScreen;
                SiteSelection.ShowDialog();
            }
        }

        private void txtStrtMnth_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtStrtMnth.Text))
            {
                if (int.Parse(txtStrtMnth.Text) >=Startmonth )
                    txtStrtYear.Text=((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString() ;
                else
                {
                    int Temp_Year = int.Parse(((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString().Trim());
                    txtStrtYear.Text = (Temp_Year + 1).ToString();
                }

                if (int.Parse(txtStrtMnth.Text.Trim()) > 12)
                    txtStrtMnth.Text = "12";
                else if(int.Parse(txtStrtMnth.Text.Trim()) <1)
                    txtStrtMnth.Text = "01";
                
            }
        }

        private void txtEndMnth_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtEndMnth.Text))
            {
                if (int.Parse(txtEndMnth.Text) >= Startmonth)
                    txtEndYear.Text = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString();
                else
                {
                    int Temp_Year = int.Parse(((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString().Trim());
                    txtEndYear.Text = (Temp_Year + 1).ToString();
                }

                if (int.Parse(txtEndMnth.Text.Trim()) > 12)
                    txtEndMnth.Text = "12";
                else if (int.Parse(txtEndMnth.Text.Trim()) < 1)
                    txtEndMnth.Text = "01";
            }
        }

        private void txtStrtYear_TextChanged(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(txtStrtYear.Text.Trim()))
            {
                if (int.Parse(txtStrtYear.Text.Trim()) < 1900)
                {
                    AlertBox.Show("Invalid Start year", MessageBoxIcon.Warning);
                    txtStrtYear.Text="";
                }
                else if (int.Parse(txtStrtYear.Text.Trim()) > 2099)
                {
                    AlertBox.Show("Invalid Start year", MessageBoxIcon.Warning);
                    txtStrtYear.Text = "";
                }
            }
        }

        private void txtEndYear_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtEndYear.Text.Trim()))
            {
                if (int.Parse(txtEndYear.Text.Trim()) < 1900)
                {
                    AlertBox.Show("Invalid End year", MessageBoxIcon.Warning);
                    txtEndYear.Text = "";
                }
                else if (int.Parse(txtEndYear.Text.Trim()) > 2099)
                {
                    AlertBox.Show("Invalid End year", MessageBoxIcon.Warning);
                    txtEndYear.Text = "";
                }
            }
        }

        private void HSSB2110_ToolClick(object sender, ToolClickEventArgs e)
        {
            Application.Navigate(CommonFunctions.BuildHelpURLS(Privileges.Program, 0, BaseForm.BusinessModuleID.ToString()), target: "_blank");
        }

        private void rbAll_Click(object sender, EventArgs e)
        {
            Sel_REFS_List.Clear();
        }

        private void Pb_Help_Click(object sender, EventArgs e)
        {
            //Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "HSSB2110");
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

        private void btnGetParameters_Click(object sender, EventArgs e)
        {
            _errorProvider.SetError(txtEndMnth, null); _errorProvider.SetError(txtEndYear, null);
            _errorProvider.SetError(txtStrtMnth, null); _errorProvider.SetError(txtStrtYear, null);
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
            //string Active = ((Captain.Common.Utilities.ListItem)cmbActive.SelectedItem).Value.ToString();
            //string RepSeq = ((Captain.Common.Utilities.ListItem)CmbRepSeq.SelectedItem).Value.ToString();
            //string EnrlStat = ((Captain.Common.Utilities.ListItem)cmbEnrlStatus.SelectedItem).Value.ToString();
            string strSite = string.Empty;
            //string strTaskAll = rbAllTasks.Checked == true ? "1" : "2";
            string strShowTask = string.Empty;
            string RepType = string.Empty;
            string SMonth = string.Empty; string SYear = string.Empty;
            string EMonth = string.Empty; string EYear = string.Empty;


            if (rbClass.Checked == true)
            {
                strShowTask = "C";
                RepType = rbAll.Checked == true ? "A" : "B";
            }
            else if (rbSchedule.Checked == true)
            {
                strShowTask = "S";
                RepType = rbFull.Checked == true ? "F" : "B";
                strSite = rbAll.Checked == true ? "A" : "M";
                SMonth = txtStrtMnth.Text; SYear = txtStrtYear.Text;
                EMonth = txtEndMnth.Text; EYear = txtEndYear.Text;
            }
            
            string strsiteRoomNames = string.Empty;
            if (rbSelectedSite.Checked == true)
            {
                foreach (CaseSiteEntity siteRoom in Sel_REFS_List)
                {
                    if (!strsiteRoomNames.Equals(string.Empty)) strsiteRoomNames += ",";
                    strsiteRoomNames += siteRoom.SiteNUMBER + siteRoom.SiteROOM + siteRoom.SiteAM_PM;
                }
            }

            
            StringBuilder str = new StringBuilder();
            str.Append("<Rows>");
            str.Append("<Row AGENCY = \"" + Agency + "\" DEPT = \"" + Depart + "\" PROG = \"" + Program +
                            "\" YEAR = \"" + Program_Year + "\" Site = \"" + strSite + "\"  ReportType = \"" + RepType + "\" StrtMonth = \"" + SMonth + 
                            "\" StrtYear = \"" + SYear + "\" EndMonth = \"" + EMonth +
                            "\" EndYear = \"" + EYear +  "\" ShowTask = \"" + strShowTask +
                            "\" SiteNames = \"" + strsiteRoomNames +  "\" />");
            str.Append("</Rows>");

            return str.ToString();
        }

        private void Set_Report_Controls(DataTable Tmp_Table)
        {
            if (Tmp_Table != null && Tmp_Table.Rows.Count > 0)
            {
                DataRow dr = Tmp_Table.Rows[0];

                Set_Report_Hierarchy(dr["AGENCY"].ToString(), dr["DEPT"].ToString(), dr["PROG"].ToString(), dr["YEAR"].ToString());

                if (dr["ShowTask"].ToString() == "C")
                {
                    rbClass.Checked = true;
                    if (dr["ReportType"].ToString() == "F") rbFull.Checked = true; else rbCondensed.Checked = true;
                }
                else
                {
                    rbSchedule.Checked = true;
                    if (dr["ReportType"].ToString() == "F") rbFull.Checked = true; else rbCondensed.Checked = true;
                    if (dr["Site"].ToString() == "A")
                        rbAll.Checked = true;
                    else
                    {
                        rbSelectedSite.Checked = true;
                        CaseSiteEntity Search_Entity = new CaseSiteEntity(true);
                        Search_Entity.SiteAGENCY = Agency; Search_Entity.SiteDEPT = Depart;
                        Search_Entity.SitePROG = Program; Search_Entity.SiteYEAR = ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString().Trim();
                        propCaseSiteEntity = _model.CaseMstData.Browse_CASESITE(Search_Entity, "Browse");
                        propCaseSiteEntity = propCaseSiteEntity.FindAll(u => u.SiteROOM.Trim() != "0000");
                        Sel_REFS_List.Clear();
                        string strSites = dr["SiteNames"].ToString();
                        List<string> siteList = new List<string>();
                        if (strSites != null)
                        {
                            string[] sitesRooms = strSites.Split(',');
                            for (int i = 0; i < sitesRooms.Length; i++)
                            {
                                CaseSiteEntity siteDetails = propCaseSiteEntity.Find(u => u.SiteNUMBER + u.SiteROOM + u.SiteAM_PM == sitesRooms.GetValue(i).ToString());
                                Sel_REFS_List.Add(siteDetails);
                            }
                        }
                    }

                    txtEndMnth.Text = dr["EndMonth"].ToString();
                    txtEndYear.Text = dr["EndYear"].ToString();
                    txtStrtMnth.Text = dr["StrtMonth"].ToString();
                    txtStrtYear.Text = dr["StrtYear"].ToString();
                }
                Sel_REFS_List = Sel_REFS_List;
            }
        }

    }
}