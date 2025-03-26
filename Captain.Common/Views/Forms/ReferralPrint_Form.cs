/************************************************************************
 * Conversion On        :   11/28/2022
 * Converted By         :   Kranthi
 * Last Modification On :   11/28/2022
 * **********************************************************************/
#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Web.Configuration;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Utilities;
using Captain.Common.Menus;
using System.Data.SqlClient;
using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Views.UserControls.Base;
using Captain.Common.Exceptions;
using System.Diagnostics;
using Captain.Common.Views.Forms;
using System.Text.RegularExpressions;
using System.IO;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Drawing;
using Wisej.Web;
#endregion

namespace Captain.Common.Views.Forms
{
    public partial class ReferralPrint_Form : Form
    {

        #region private variables
        private ErrorProvider _errorProvider = null;
        private CaptainModel _model = null;

        #endregion

        public ReferralPrint_Form(BaseForm baseform, PrivilegeEntity priviliges, List<ACTREFSEntity> sel_REFS_List, string strdate, string strReferfromto)
        {
            InitializeComponent();

            _model = new CaptainModel();

            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            Referfromto = strReferfromto;
            Sel_REFS_List = null;
            ActRefsList = sel_REFS_List;
            ReferDate = strdate;
            //Mode = strMode;
            BaseForm = baseform;
            Privileges = priviliges;
            //FillCombo();
            //pnlAgyNameSearch.Visible = false;

            //this.Ref_Sel.Visible = true;
            this.Street.Visible = true;
            this.City.Visible = true;
            this.State.Visible = true;
            this.gvtNameIndex.Visible = false;
            this.Name2.Visible = true;

            this.Text = "CASE0006 - Agency Referrals Print";

            propReportPath = _model.lookupDataAccess.GetReportPath();
            lblDate.Text = "Date: " + LookupDataAccess.Getdate(ReferDate.Trim());

            //ActRefsList = ActRefsList.FindAll(u => u.Date.Equals(ReferDate) && u.Type.Equals(Referfromto));

            FillNameCombo();
            Fill_CaseWorker();

            Get_ReferrTo_Data();
            GetActReferData();

            ACTREFS_List= ACTREFS_List.FindAll(u => u.Date.Equals(ReferDate) && u.Type.Equals(Referfromto));

            userHierarchy = _model.UserProfileAccess.GetUserHierarchyByID(ACTREFS_List[0].Add_Operator.Trim());
            hierarchyEntity = new HierarchyEntity(); 
            foreach (HierarchyEntity Entity in userHierarchy)
            {
                if (Entity.Agency == BaseForm.BaseAgency && Entity.Dept == BaseForm.BaseDept && Entity.Prog == BaseForm.BaseProg)
                    hierarchyEntity = Entity;
                else if (Entity.Agency == BaseForm.BaseAgency && Entity.Dept == BaseForm.BaseDept && Entity.Prog == "**")
                    hierarchyEntity = Entity;
                else if (Entity.Agency == BaseForm.BaseAgency && Entity.Dept == "**" && Entity.Prog == "**")
                    hierarchyEntity = Entity;
                else if (Entity.Agency == "**" && Entity.Dept == "**" && Entity.Prog == "**")
                { hierarchyEntity = Entity; }
            }

            Fill_ReferrTo_Data();

        }

        #region properties

        public BaseForm BaseForm { get; set; }

        public string Form_Display_Mode { get; set; }

        public string AgencyCode { get; set; }

        public string CAMS_Desc { get; set; }

        public string Hierarchy { get; set; }

        public string propReportPath { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public string code { get; set; }

        public HierarchyEntity hierarchyEntity { get; set; }

        public List<HierarchyEntity> userHierarchy { get; set; }

        public List<ACTREFSEntity> Sel_REFS_List { get; set; }

        public List<ACTREFSEntity> ActRefsList { get; set; }

        public List<CASEREFSEntity> Sel_CASEREFS_List { get; set; }

        public List<CaseVDD1Entity> Sel_CASEVDD1_List { get; set; }

        public CASEMSEntity Pass_MS_Entity { get; set; }

        public string ReferDate { get; set; }

        public string Referfromto { get; set; }

        public string Mode { get; set; }

        public string FormName { get; set; }

        #endregion


        List<CASEREFEntity> CASEREFREF_List = new List<CASEREFEntity>();
        List<ACTREFSEntity> ACTREFS_List = new List<ACTREFSEntity>();
        CASEREFSEntity Search_REFS_Entity = new CASEREFSEntity(true);
        private void Get_ReferrTo_Data()
        {
            CASEREFEntity Search_REF_Entity = new CASEREFEntity(true);
            CASEREFREF_List = _model.SPAdminData.Browse_CASEREF(Search_REF_Entity, "Browse");
            GetActReferData();
        }

        public void GetActReferData()
        {
            ACTREFSEntity Search_ACTREF_Entity = new ACTREFSEntity(true);
            Search_ACTREF_Entity.Agency = BaseForm.BaseAgency;
            Search_ACTREF_Entity.Dept = BaseForm.BaseDept;
            Search_ACTREF_Entity.Program = BaseForm.BaseProg;
            Search_ACTREF_Entity.Year = BaseForm.BaseYear;
            Search_ACTREF_Entity.ApplNo = BaseForm.BaseApplicationNo;
            ACTREFS_List = _model.SPAdminData.Browse_ACTREFS(Search_ACTREF_Entity, "Browse");

        }

        private void FillNameCombo()
        {
            cmbName.Items.Clear();
            if (BaseForm.BaseCaseSnpEntity != null)
            {
                CaseSnpEntity casesnpApplicant = BaseForm.BaseCaseSnpEntity.Find(u => u.FamilySeq.Equals(BaseForm.BaseCaseMstListEntity[0].FamilySeq));

                List<CaseSnpEntity> caseSnpMembers = BaseForm.BaseCaseSnpEntity.FindAll(u => u.FamilySeq != BaseForm.BaseCaseMstListEntity[0].FamilySeq);
                List<Captain.Common.Utilities.ListItem> listItem = new List<Captain.Common.Utilities.ListItem>();
                if (casesnpApplicant != null)
                {
                    string name = LookupDataAccess.GetMemberName(casesnpApplicant.NameixFi, casesnpApplicant.NameixMi, casesnpApplicant.NameixLast, BaseForm.BaseHierarchyCnFormat);

                    listItem.Add(new Captain.Common.Utilities.ListItem(name, casesnpApplicant.FamilySeq));
                }

                foreach (CaseSnpEntity caseSnp in caseSnpMembers)
                {
                    string name = LookupDataAccess.GetMemberName(caseSnp.NameixFi, caseSnp.NameixMi, caseSnp.NameixLast, BaseForm.BaseHierarchyCnFormat);
                    listItem.Add(new Captain.Common.Utilities.ListItem(name, casesnpApplicant.FamilySeq));
                }

                cmbName.Items.AddRange(listItem.ToArray());
                cmbName.SelectedIndex = 0;
            }
        }

        string UserID = string.Empty;
        private void Fill_ReferrTo_Data()
        {
            bool Ref_Exists = false;
            string Active_Stat = "N";
            Ref_Grid.Rows.Clear();
            int rowIndex = 0;
            //foreach (CASEREFSEntity Entity in Sel_REFS_List)
            //{
            string strConnected = string.Empty;
            List<CommonEntity> commonconneted = BaseForm.BaseAgyTabsEntity.FindAll(u => u.AgyCode == "S0070").ToList();

            if (ACTREFS_List != null)
            {
                strConnected = string.Empty;
                foreach (ACTREFSEntity Entity in ACTREFS_List)
                {
                    Ref_Exists = false;
                    string strRefKey = Entity.Agency + Entity.Dept + Entity.Program + (Entity.Year.ToString().Trim() == string.Empty ? "    " : Entity.Year) + Entity.ApplNo + LookupDataAccess.Getdate(Entity.Date) + Entity.Code + Entity.Type;
                    foreach (CASEREFEntity Entity1 in CASEREFREF_List)
                    {
                        Active_Stat = Entity1.Active;
                        if (Entity1.Code == Entity.Code)
                        {
                            Ref_Exists = true;
                            if (commonconneted.Count > 0)
                            {
                                if (Entity.Connected.ToUpper().Trim() == "N" || Entity.Connected.ToUpper().Trim() == string.Empty)
                                    strConnected = string.Empty;
                                else
                                {
                                    CommonEntity connectddesc = commonconneted.Find(u => u.Code.ToUpper().Trim() == Entity.Connected.ToUpper().Trim());
                                    if (connectddesc != null)
                                    {
                                        strConnected = connectddesc.Desc;
                                    }
                                    else
                                    {
                                        strConnected = string.Empty;
                                    }

                                }

                            }

                            rowIndex = Ref_Grid.Rows.Add(Entity1.Code, Entity1.Name1.Trim(), Entity1.Name2.Trim(), Entity1.Street, Entity1.City, Entity1.State, Entity.NameIndex, Entity1.Active);

                            //rowIndex = Ref_Grid.Rows.Add(Convert.ToDateTime(Entity.Date).Date, Entity.Type == "T" ? "Referred To" : "Referred From", Entity.Type, Entity1.Code, Entity1.Name1.Trim(), Entity1.Name2.Trim(), Entity1.City, Entity1.State, Entity1.Active, strConnected, strRefKey);
                            Ref_Grid.Rows[rowIndex].Tag = Entity;
                            break;
                        }
                    }
                    //if (!Ref_Exists)
                    //{
                    //    if (commonconneted.Count > 0)
                    //    {
                    //        if (Entity.Connected.ToUpper().Trim() == "N" || Entity.Connected.ToUpper().Trim() == string.Empty)
                    //            strConnected = string.Empty;
                    //        else
                    //            strConnected = commonconneted.Find(u => u.Code.ToUpper().Trim() == Entity.Connected.ToUpper().Trim()).Desc;
                    //    }
                    //    rowIndex = Ref_Grid.Rows.Add(Convert.ToDateTime(Entity.Date).Date, Entity.Type == "T" ? "Referred To" : "Referred From", Entity.Type, Entity.Code, "Not Defined in 'CASEREF'", " ", " ", " ", " ", strConnected, strRefKey);
                    //    Ref_Grid.Rows[rowIndex].Tag = Entity;
                    //}

                    if (Active_Stat != "Y")
                        Ref_Grid.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.MediumVioletRed;  // Color.Red;

                    //set_Reffer_Tooltip(rowIndex, Entity);
                }
                
            }
            //}
        }

        List<Captain.Common.Utilities.ListItem> CaseWorker_List = new List<Captain.Common.Utilities.ListItem>();
        private void Fill_CaseWorker()
        {
            try
            {


                //DataSet ds2 = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(MainMenuAgency, MainMenuDept, MainMenuProgram);
                DataSet ds2 = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(BaseForm.BaseAgency, "**", "**");
                string strNameFormat = null, strCwFormat = null;
                if (ds2.Tables[0].Rows.Count > 0)
                {
                    strNameFormat = ds2.Tables[0].Rows[0]["HIE_CN_FORMAT"].ToString();
                    strCwFormat = ds2.Tables[0].Rows[0]["HIE_CW_FORMAT"].ToString();
                }

                DataSet ds1 = Captain.DatabaseLayer.CaseMst.GetCaseWorker(strCwFormat, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg);
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
            catch (Exception ex)
            {

            }
        }


        //Added by Sudheer on 05/26/2021 for CCA referral Letter
        string Agency = null;
        string strFolderPath = string.Empty;
        string Random_Filename = null; string PdfName = null;
        PdfContentByte cb;
        int X_Pos, Y_Pos;
        private void On_SaveForm_Closed()
        {
            Random_Filename = null;

            string ReaderName = string.Empty;

            ReaderName = propReportPath + "\\" + "CCA_REFERRAL_FORM.pdf";



            PdfName = "REFERRAL_FORM";//form.GetFileName();
            //PdfName = strFolderPath + PdfName;
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

            PdfReader Hreader = new PdfReader(ReaderName);

            PdfStamper Hstamper = new PdfStamper(Hreader, new FileStream(PdfName, FileMode.Create, FileAccess.Write));
            Hstamper.Writer.SetPageSize(PageSize.A4);
            PdfContentByte cb = Hstamper.GetOverContent(1);

            //AcroFields form = Hstamper.AcroFields;           
            //FileStream fs = new FileStream(PdfName, FileMode.Create);

            //Document document = new Document();
            //document.SetPageSize(iTextSharp.text.PageSize.A4);
            //PdfWriter writer = PdfWriter.GetInstance(document, fs);
            //document.Open();
            //cb = Writer.DirectContent;
            ////string Priv_Scr = null;
            ////document = new Document(iTextSharp.text.PageSize.A4.Rotate());
            BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            iTextSharp.text.Font helvetica = new iTextSharp.text.Font(bf_times, 12, 1);
            BaseFont bf_helv = helvetica.GetCalculatedBaseFont(false);
            iTextSharp.text.Font TimesUnderline = new iTextSharp.text.Font(bf_times, 9, 4);
            BaseFont bf_TimesUnderline = TimesUnderline.GetCalculatedBaseFont(true);

            iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 10);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 12);
            iTextSharp.text.Font TableFontBoldItalicUnderline = new iTextSharp.text.Font(bf_times, 11, 7, BaseColor.BLUE.Darker());
            iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 11, 3, BaseColor.BLUE.Darker());
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_times, 11, 1);
            iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 10, 2);
            iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 10, 4);
            iTextSharp.text.Font TblFontBoldColor = new iTextSharp.text.Font(bf_times, 16, 7, BaseColor.BLUE.Darker());

            iTextSharp.text.Image _image_UnChecked = iTextSharp.text.Image.GetInstance(Consts.Icons.ico_checkbox);
            iTextSharp.text.Image _image_Checked = iTextSharp.text.Image.GetInstance(Consts.Icons.ico_Checked_checkbox);

            _image_UnChecked.ScalePercent(60f);
            _image_Checked.ScalePercent(60f);

            try
            {
                X_Pos = 30; Y_Pos = 760;

                //cb.BeginText();
                //cb.SetFontAndSize(bf_helv, 16);
                ////cb.SetColorFill(BaseColor.BLUE.Darker());
                ////cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Head Start Eligibility Verification", 300, 800, 0);
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_CENTER, new Phrase("Head Start Eligibility Verification", TblFontBoldColor), 300, Y_Pos, 0);
                ////cb.SetColorFill(BaseColor.BLACK.Brighter());
                //cb.EndText();


                X_Pos = 187; Y_Pos -= 210;
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase("Customer/Primary Name: ", Times), X_Pos, Y_Pos, 0);
                //ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(BaseForm.BaseApplicationName, TableFont), X_Pos, Y_Pos, 0);
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(((Utilities.ListItem)cmbName.SelectedItem).Text.Trim(), TableFont), X_Pos, Y_Pos, 0);


                X_Pos = 187; Y_Pos -= 24;

                string Worker = hierarchyEntity.CaseWorker.Trim(); //BaseForm.BaseCaseMstListEntity[0].IntakeWorker.ToString().Trim();
                Worker = Get_CaseWorker_DESC(Worker);//Get_CaseWorker_DESC(BaseForm.BaseCaseMstListEntity[0].IntakeWorker.ToString().Trim());
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(Worker, TableFont), X_Pos, Y_Pos, 0);



                if (ACTREFS_List.Count > 0)
                {
                    X_Pos = 187; Y_Pos -= 24;
                    ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(LookupDataAccess.Getdate(ReferDate.Trim()), TableFont), X_Pos, Y_Pos, 0);

                    Y_Pos -= 65;

                    X_Pos = 70;
                    int Count = 1;
                    foreach (ACTREFSEntity Entity in ACTREFS_List)
                    {
                        foreach (CASEREFEntity Entity1 in CASEREFREF_List)
                        {
                            if (Entity.Code.Trim() == Entity1.Code.Trim())
                            {
                                Y_Pos -= 17; CheckBottomBorderReachedLetterHead(Hstamper);
                                MaskedTextBox mskPhn = new MaskedTextBox();
                                mskPhn.Mask = "(000)000-0000";
                                mskPhn.Text = Entity1.Telno.Trim();

                                string Phn = string.Empty;
                                if (!string.IsNullOrEmpty(Entity1.Telno.Trim())) Phn = mskPhn.Text;

                                string WebAddress = string.Empty;
                                if (!string.IsNullOrEmpty(Entity1.WebAddress.Trim())) WebAddress = Entity1.WebAddress.Trim();
                                string Coma = string.Empty;
                                if (!string.IsNullOrEmpty(Entity1.WebAddress.Trim())) Coma = ", ";

                                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(Count.ToString() + ". " + Entity1.Name1 + ", " + Entity1.Street.Trim() + " " + Entity1.City.Trim() + " " + Entity1.State + ", " + "00000".Substring(0, 5 - Entity1.Zip.Length) + Entity1.Zip.Trim() + "-" + "0000".Substring(0, 4 - Entity1.Zip_Plus.Length) + Entity1.Zip_Plus.Trim() + ", " + Phn+ Coma, Times), X_Pos, Y_Pos, 0);
                                Y_Pos  -= 15; CheckBottomBorderReachedLetterHead(Hstamper);
                                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase(WebAddress, Times), X_Pos, Y_Pos, 0);
                                Count++;
                            }
                        }
                    }
                }

                Y_Pos -= 40; CheckBottomBorderReachedLetterHead(Hstamper);
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase("CCA is an organization dedicated to providing strengths-based opportunities to help people", TableFont), X_Pos, Y_Pos, 0);
                Y_Pos -= 15; CheckBottomBorderReachedLetterHead(Hstamper);
                ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase("achieve economic, physical, and emotional security.", TableFont), X_Pos, Y_Pos, 0);



                //Y_Pos -= 520; X_Pos = 160;
                //ColumnText.ShowTextAligned(cb, Element.ALIGN_LEFT, new Phrase(Worker, TblFontBold), X_Pos, Y_Pos, 0);


            }
            catch (Exception ex) { /*document.Add(new Paragraph("Aborted due to Exception............................................... "));*/ }

            Hstamper.Close();

            if (BaseForm.BaseAgencyControlDetails.ReportSwitch.ToUpper() == "Y")
            {
                PdfViewerNewForm objfrm = new PdfViewerNewForm(PdfName);
                objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                objfrm.ShowDialog();
            }
            else
            {
                FrmViewer objfrm = new FrmViewer(PdfName);
                objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                objfrm.ShowDialog();
            }

        }

        private void CheckBottomBorderReachedLetterHead(PdfStamper Hstamper)
        {
            if (Y_Pos <= 20)
            {
                cb.EndText();
                Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                ////document.SetPageSize(new iTextSharp.text.Rectangle(iTextSharp.text.PageSize.A4.Width, iTextSharp.text.PageSize.A4.Height));

                document.NewPage();
                //pageNumber = Hstamper.PageNumber - 1;

                //cb.BeginText();

                //X_Pos = 50;
                //Y_Pos -= 5;

                //cb.EndText();

                Y_Pos = 760;
                X_Pos = 90;                                                           //modified

                cb.BeginText();

            }
        }


        private void On_Delete_PDF_File(object sender, FormClosedEventArgs e)
        {
            System.IO.File.Delete(PdfName);
        }

        private void btnGenPdf_Click(object sender, EventArgs e)
        {
            On_SaveForm_Closed();
        }

        private void btnPDFprev_Click(object sender, EventArgs e)
        {
            this.Close();
            //PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, true, propReportPath);
            //pdfListForm.ShowDialog();
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


    }
}