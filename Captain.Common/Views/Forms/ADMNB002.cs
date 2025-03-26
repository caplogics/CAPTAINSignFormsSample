#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
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
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using CarlosAg.ExcelXmlWriter;
using Wisej.Web;
#endregion

namespace Captain.Common.Views.Forms
{
    public partial class ADMNB002 : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        //private CustomFieldsControl _intakeHierarchy = null;
        private bool boolChangeStatus = false;
        private CaptainModel _model = null;
        private List<HierarchyEntity> _selectedHierarchies = null;

        #endregion

        public ADMNB002(BaseForm baseform, PrivilegeEntity privileges)
        {
            _model = new CaptainModel();
            BaseForm = baseform;
            Privileges = privileges;
            InitializeComponent();

            propReportPath = _model.lookupDataAccess.GetReportPath();

            this.Text = Privileges.PrivilegeName;
            PopulateDropdowns();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

            Agency = baseform.BaseAgency;  //"**";
            Dept = baseform.BaseDept; //"**";
            Prog = baseform.BaseProg; //"**";
        }

        #region properties

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileges { get; set; }
        public string propReportPath { get; set; }
        #endregion

        DirectoryInfo MyDir;

        string UserName = null;
        string ModuleCode = null;
        string PdfName = "Pdf File", PrintText = null;
        string Random_Filename = null;
        int X_Pos, Y_Pos;
        bool FirstRun = true;
        PdfContentByte cb;
        int Users;
        string pcnt;
        DataTable dt;
        int pageNumber = 1;
        BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false); //BaseFont.CP1252
        //BaseFont bfTimes = BaseFont.CreateFont("c:\\windows\\fonts\\BOOKOS.TTF", BaseFont.MACROMAN, false);
        //Font fc = new Font();

        int TmpCount_yeswanth = 0;


        private void PopulateDropdowns()
        {
            DataSet ds = Captain.DatabaseLayer.Lookups.GetModules();
            dt = ds.Tables[0];

            List<Captain.Common.Utilities.ListItem> listItem = new List<Captain.Common.Utilities.ListItem>();

            listItem.Add(new Captain.Common.Utilities.ListItem("All Applications", "**"));
            foreach (DataRow dr in dt.Rows)
            {
                listItem.Add(new Captain.Common.Utilities.ListItem(dr["APPL_DESCRIPTION"].ToString(), dr["APPL_CODE"].ToString()));
            }
            CmbApp.Items.AddRange(listItem.ToArray());
            CmbApp.SelectedIndex = 0;

            DataSet ds1 = Captain.DatabaseLayer.ADMNB002DB.GetUserNames();
            DataTable dt1 = ds1.Tables[0];
            listItem.Clear();
            listItem.Add(new Captain.Common.Utilities.ListItem("List of Screens & Reports", "00"));
            listItem.Add(new Captain.Common.Utilities.ListItem("All Users", "**"));
            foreach (DataRow dr in dt1.Rows)
            {
                listItem.Add(new Captain.Common.Utilities.ListItem(dr["PWR_EMPLOYEE_NO"].ToString(), dr["PWR_EMPLOYEE_NO"].ToString()));
            }
            CmbUser.Items.AddRange(listItem.ToArray());
            CmbUser.SelectedIndex = 1;



            /***************************************************************************************/
            /**************** Kranthi:: Added new Combobox for Reportsfor option  ******************/
            /***************************************************************************************/
            cmbReportfor.Items.Clear();
            List<comboboxlist> list = new List<comboboxlist>
                {
                    new comboboxlist{ id ="1", Description = "Screens Only"},
                    new comboboxlist{ id ="2", Description = "Reports Only"},
                    new comboboxlist{ id ="3", Description = "Both Screens and Reports"},
                };

            cmbReportfor.DataSource = list;
            cmbReportfor.DisplayMember = "Description";
            cmbReportfor.ValueMember = "id";
            cmbReportfor.SelectedIndex = 2;
            /***************************************************************************************/
            /***************************************************************************************/
        }


        private void BtnPdfPrev_Click(object sender, EventArgs e)
        {
            //PdfName = Context.Server.MapPath("~\\Resources\\Pdf\\" + TxtFileName.Text + ".pdf");
            //FrmViewer ADMNB001Form = new FrmViewer(PdfName);
            //ADMNB001Form.ShowDialog();
            PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, true, propReportPath);
            pdfListForm.StartPosition = FormStartPosition.CenterScreen;
            pdfListForm.ShowDialog();

        }

        private void BtnGenFile_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                string Format = "PDF";
                if (rdoUserAccess.Checked)
                    Format = "XLS";

                    PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, false, propReportPath, Format);
                if (rdoUserAccess.Checked)
                {
                    pdfListForm.FormClosed += new FormClosedEventHandler(On_ExcelForm_Closed);
                }
                else
                {
                    if (((Captain.Common.Utilities.ListItem)CmbUser.SelectedItem).Value.ToString().Trim() == "00")
                        pdfListForm.FormClosed += new FormClosedEventHandler(On_All_List_Pdf);
                    else
                        pdfListForm.FormClosed += new FormClosedEventHandler(On_SaveForm_Closed1);//OnsavePdf
                                                                                                  //pdfListForm.FormClosed += new Form.FormClosedEventHandler(OnsavePdf);//OnsavePdf
                }
                pdfListForm.StartPosition = FormStartPosition.CenterScreen;
                pdfListForm.ShowDialog();
            }
        }

        private bool ValidateForm()
        {
            bool isValid = true;
            if (rdoUserAccess.Checked)
            {
                if (string.IsNullOrEmpty(txtHierchy.Text.Trim()))
                {
                    _errorProvider.SetError(Pb_Search_Hie, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), LblApp.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(Pb_Search_Hie, null);
                }
            }
            return (isValid);
        }
        string getUserHierarchies(string strUserName, string strModule)
        {

            string strResult = "";
            DataSet dsModuleHie = Captain.DatabaseLayer.ADMNB002DB.ADMNB002_GetModuleHierarchies(strUserName.Trim(), strModule);
            DataTable dtModuleHie = dsModuleHie.Tables[0];
            int i = 0;
            bool MultLine = false;
            string Str = null;
            foreach (DataRow dr in dtModuleHie.Rows)
            {
                if (i == 6)
                {
                    //PrintRec(Str, 70);
                    //if (!MultLine)
                    //{
                    //    X_Pos = 370;
                    //    cb.SetColorFill(BaseColor.BLUE.Brighter());
                    //    PrintRec("Case Worker: ", 58);
                    //    cb.SetCMYKColorFill(0, 0, 0, 255);
                    //    PrintRec(Entity.CaseWoeker, 55);
                    //}
                    //Y_Pos -= 14;
                    //X_Pos = 100;
                    Str = null;
                    i = 0;
                    //CheckBottomBorderReached(document, writer);
                    MultLine = true;
                }
                if (i > 0)
                    Str += ", ";

                Str += dr["Hierarchy"].ToString();
                i++;
            }
            strResult = Str;
            return strResult;
        }
        private void On_SaveForm_Closed1(object sender, FormClosedEventArgs e)
        {
            PdfListForm form = sender as PdfListForm;
            if (form.DialogResult == DialogResult.OK)
            {
                X_Pos = 30; Y_Pos = 720;
                string ModuleDesc = null, PrvUser = null, PrvModule = null, PrvScr = null;
                CaptainModel model = new CaptainModel();

                DataTable dtDistUsers = new DataTable();
                DataTable dtUserScreens = new DataTable();
                DataSet dsData = model.ADMNB002Data.ADMNB002_GetDistinctUserId(UserName, ModuleCode);
                if (dsData.Tables.Count > 0)
                {
                    dtUserScreens = dsData.Tables[0];
                    dtDistUsers = dsData.Tables[1];
                }

                // List<EMPLFUNCEntity> Cust = model.ADMNB002Data.ADMNB002_GetScrPrivbyUserId(UserName, ModuleCode);
                List<BATCNTLEntity> BatcntlList = model.ADMNB002Data.ADMNB002_GetRepPrivbyUserId(UserName, ModuleCode);

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

                System.IO.FileStream fs = new FileStream(PdfName, FileMode.Create);

                Document document = new Document(PageSize.A4, 30f, 30f, 30f, 50f);
                PdfWriter writer = PdfWriter.GetInstance(document, fs);

                document.Open();
                cb = writer.DirectContent;
                try
                {
                    PrintHeaderPage(document, writer);
                    document.NewPage();

                    BaseFont bf_Calibri = BaseFont.CreateFont("c:/windows/fonts/calibri.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
                    //BaseFont bf_Wingdings2 = BaseFont.CreateFont("C:/Windows/Fonts/WINGDNG2.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
                    BaseFont bf_Wingdings2 = BaseFont.CreateFont(Application.MapPath("~\\Fonts\\WINGDNG2.TTF"), BaseFont.WINANSI, BaseFont.EMBEDDED);

                    iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_Calibri, 10);
                    iTextSharp.text.Font FontWingdings = new iTextSharp.text.Font(bf_Wingdings2, 8, iTextSharp.text.Font.BOLD, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#01a601")));

                    iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_Calibri, 8, iTextSharp.text.Font.NORMAL);
                    iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_Calibri, 8, 3);
                    iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_Calibri, 8, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
                    iTextSharp.text.Font TableFontSubHeader = new iTextSharp.text.Font(bf_Calibri, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                    iTextSharp.text.Font TblFontBold1 = new iTextSharp.text.Font(bf_Calibri, 8, 1);
                    iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_Calibri, 8, 2);
                    iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_Calibri, 9, 4);
                    iTextSharp.text.Font TblFontWhite = new iTextSharp.text.Font(bf_Calibri, 8, iTextSharp.text.Font.NORMAL, BaseColor.WHITE);




                    foreach (DataRow drDistUsers in dtDistUsers.Rows)
                    {
                        string _EMPNo = drDistUsers["EFR_EMPLOYEE_NO"].ToString();
                        string _ModuleCode = drDistUsers["EFR_MODULE_CODE"].ToString();
                        document.NewPage();
                        #region HeadDetails

                        PdfPTable tblHeaderData = new PdfPTable(6);
                        tblHeaderData.TotalWidth = 560f;
                        tblHeaderData.WidthPercentage = 100;
                        tblHeaderData.LockedWidth = true;
                        float[] widths = new float[] { 15f, 30f, 10f, 15f, 10f, 20f };
                        tblHeaderData.SetWidths(widths);
                        tblHeaderData.HorizontalAlignment = Element.ALIGN_CENTER;
                        //tblHeaderData.SpacingAfter = 1f;
                        tblHeaderData.HeaderRows = 1;

                        ModuleDesc = "";
                        if (drDistUsers["EFR_MODULE_CODE"].ToString() != "")
                        {
                            DataRow[] dr = dt.Select("APPL_CODE='" + drDistUsers["EFR_MODULE_CODE"].ToString() + "'");
                            if (dr.Length > 0)
                                ModuleDesc = dr[0]["APPL_DESCRIPTION"].ToString();
                        }


                        PdfPCell cell_Content_Title1 = new PdfPCell(new Phrase("  Module", TableFont));
                        cell_Content_Title1.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell_Content_Title1.Border = iTextSharp.text.Rectangle.BOX;
                        cell_Content_Title1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                        cell_Content_Title1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#b1c9df"));
                        cell_Content_Title1.FixedHeight = 15f;
                        tblHeaderData.AddCell(cell_Content_Title1);

                        PdfPCell cell_Content_Desc1 = new PdfPCell(new Phrase(ModuleDesc, TableFont));
                        cell_Content_Desc1.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell_Content_Desc1.Border = iTextSharp.text.Rectangle.BOX;
                        cell_Content_Desc1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                        cell_Content_Desc1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#c6d6e5"));
                        cell_Content_Desc1.PaddingBottom = 5;
                        tblHeaderData.AddCell(cell_Content_Desc1);

                        PdfPCell cell_Content_Title2 = new PdfPCell(new Phrase("  UserID", TableFont));
                        cell_Content_Title2.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell_Content_Title2.Border = iTextSharp.text.Rectangle.BOX;
                        cell_Content_Title2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                        cell_Content_Title2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#b1c9df"));
                        cell_Content_Title2.FixedHeight = 15f;
                        tblHeaderData.AddCell(cell_Content_Title2);

                        PdfPCell cell_Content_Desc2 = new PdfPCell(new Phrase(drDistUsers["EFR_EMPLOYEE_NO"].ToString(), TableFont));
                        cell_Content_Desc2.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell_Content_Desc2.Border = iTextSharp.text.Rectangle.BOX;
                        cell_Content_Desc2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                        cell_Content_Desc2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#c6d6e5"));
                        cell_Content_Desc2.PaddingBottom = 5;
                        tblHeaderData.AddCell(cell_Content_Desc2);

                        PdfPCell cell_Content_Title3 = new PdfPCell(new Phrase("  Name", TableFont));
                        cell_Content_Title3.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell_Content_Title3.Border = iTextSharp.text.Rectangle.BOX;
                        cell_Content_Title3.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                        cell_Content_Title3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#b1c9df"));
                        cell_Content_Title3.FixedHeight = 15f;
                        tblHeaderData.AddCell(cell_Content_Title3);

                        PdfPCell cell_Content_Desc3 = new PdfPCell(new Phrase(drDistUsers["Name"].ToString(), TableFont));
                        cell_Content_Desc3.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell_Content_Desc3.Border = iTextSharp.text.Rectangle.BOX;
                        cell_Content_Desc3.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                        cell_Content_Desc3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#c6d6e5"));
                        cell_Content_Desc3.PaddingBottom = 5;
                        tblHeaderData.AddCell(cell_Content_Desc3);


                        PdfPCell cell_Content_Title4 = new PdfPCell(new Phrase("  Hiearchy", TableFont));
                        cell_Content_Title4.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell_Content_Title4.Border = iTextSharp.text.Rectangle.BOX;
                        cell_Content_Title4.BorderColorBottom = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                        cell_Content_Title4.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                        cell_Content_Title4.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#b1c9df"));
                        cell_Content_Title4.FixedHeight = 15f;
                        tblHeaderData.AddCell(cell_Content_Title4);

                        string strHierarchy = getUserHierarchies(drDistUsers["EFR_EMPLOYEE_NO"].ToString(), drDistUsers["EFR_MODULE_CODE"].ToString());

                        PdfPCell cell_Content_Desc4 = new PdfPCell(new Phrase(strHierarchy, TableFont));
                        cell_Content_Desc4.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell_Content_Desc4.Border = iTextSharp.text.Rectangle.BOX;
                        cell_Content_Desc4.BorderColorBottom = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                        cell_Content_Desc4.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                        cell_Content_Desc4.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#c6d6e5"));
                        cell_Content_Desc4.PaddingBottom = 5;
                        cell_Content_Desc4.Colspan = 3;
                        tblHeaderData.AddCell(cell_Content_Desc4);


                        PdfPCell cell_Content_Title5 = new PdfPCell(new Phrase("  CaseWorker", TableFont));
                        cell_Content_Title5.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell_Content_Title5.Border = iTextSharp.text.Rectangle.BOX;
                        cell_Content_Title5.BorderColorBottom = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                        cell_Content_Title5.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                        cell_Content_Title5.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#b1c9df"));
                        cell_Content_Title5.FixedHeight = 15f;
                        tblHeaderData.AddCell(cell_Content_Title5);

                        PdfPCell cell_Content_Desc5 = new PdfPCell(new Phrase(drDistUsers["CaseWoeker"].ToString(), TableFont));
                        cell_Content_Desc5.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell_Content_Desc5.Border = iTextSharp.text.Rectangle.BOX;
                        cell_Content_Desc5.BorderColorBottom = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                        cell_Content_Desc5.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                        cell_Content_Desc5.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#c6d6e5"));
                        cell_Content_Desc5.FixedHeight = 15f;
                        tblHeaderData.AddCell(cell_Content_Desc5);


                        //PdfPCell cellempty = new PdfPCell(new Phrase("   ", TableFont));
                        //cellempty.HorizontalAlignment = Element.ALIGN_LEFT;
                        //cellempty.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                        //cellempty.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                        //cellempty.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                        //cellempty.Colspan = 6;
                        //cellempty.FixedHeight = 1f;
                        //tblHeaderData.AddCell(cellempty);

                        if (tblHeaderData.Rows.Count > 0)
                        {
                            document.Add(tblHeaderData);
                            tblHeaderData.DeleteBodyRows();
                        }

                        #endregion

                        if (dtUserScreens.Rows.Count > 0)
                        {

                            if (_EMPNo != "" && _ModuleCode != "")
                            {

                                PdfPTable tblScrnRpts = new PdfPTable(6);
                                tblScrnRpts.TotalWidth = 560f;
                                tblScrnRpts.WidthPercentage = 100;
                                tblScrnRpts.LockedWidth = true;
                                float[] widthSCRRPT = new float[] { 56f, 12f, 8f, 8f, 8f, 8f };
                                tblScrnRpts.SetWidths(widthSCRRPT);
                                tblScrnRpts.HorizontalAlignment = Element.ALIGN_CENTER;
                                //tblScrnRpts.HeaderRows = 1;
                                //tblScrnRpts.SpacingBefore = 15f;

                                DataRow[] drobjScreens = dtUserScreens.Select("EFR_EMPLOYEE_NO='" + _EMPNo + "' AND EFR_MODULE_CODE='" + _ModuleCode + "'");
                                List<BATCNTLEntity> drobjReports = BatcntlList.FindAll(x => x.ModuleCode == _ModuleCode && x.UserName == _EMPNo.Trim().ToString()).ToList();
                                if (cmbReportfor.SelectedIndex == 0 || cmbReportfor.SelectedIndex == 2)
                                {
                                    if (drobjScreens.Length > 0)
                                    {
                                        DataTable dtobjScreens = drobjScreens.CopyToDataTable();
                                        foreach (DataRow drCAPAPPL in dt.Rows)
                                        {
                                            DataRow[] drScreens = dtobjScreens.Select("EFR_MODULE_CODE='" + drCAPAPPL["APPL_CODE"].ToString() + "'");
                                            if (drScreens.Length > 0)
                                            {

                                                #region HeaderTitles
                                                PdfPCell Htitle1 = new PdfPCell(new Phrase(" Screen Description", TblFontBold));
                                                Htitle1.HorizontalAlignment = Element.ALIGN_LEFT;
                                                Htitle1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                                                Htitle1.BorderColorTop = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                                                Htitle1.BorderColor = BaseColor.WHITE;
                                                Htitle1.FixedHeight = 15f;
                                                tblScrnRpts.AddCell(Htitle1);

                                                PdfPCell Htitle2 = new PdfPCell(new Phrase("Hierarchy", TblFontBold));
                                                Htitle2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                Htitle2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                                                Htitle2.BorderColorTop = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                                                Htitle2.BorderColor = BaseColor.WHITE;
                                                Htitle2.FixedHeight = 15f;
                                                tblScrnRpts.AddCell(Htitle2);

                                                PdfPCell Htitle3 = new PdfPCell(new Phrase("View", TblFontBold));
                                                Htitle3.HorizontalAlignment = Element.ALIGN_CENTER;
                                                Htitle3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                                                Htitle3.BorderColorTop = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                                                Htitle3.BorderColor = BaseColor.WHITE;
                                                Htitle3.FixedHeight = 15f;
                                                tblScrnRpts.AddCell(Htitle3);


                                                PdfPCell Htitle4 = new PdfPCell(new Phrase("Add", TblFontBold));
                                                Htitle4.HorizontalAlignment = Element.ALIGN_CENTER;
                                                Htitle4.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                                                Htitle4.BorderColorTop = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                                                Htitle4.BorderColor = BaseColor.WHITE;
                                                Htitle4.FixedHeight = 15f;
                                                tblScrnRpts.AddCell(Htitle4);

                                                PdfPCell Htitle5 = new PdfPCell(new Phrase("Edit", TblFontBold));
                                                Htitle5.HorizontalAlignment = Element.ALIGN_CENTER;
                                                Htitle5.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                                                Htitle5.BorderColorTop = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                                                Htitle5.BorderColor = BaseColor.WHITE;
                                                Htitle5.FixedHeight = 15f;
                                                tblScrnRpts.AddCell(Htitle5);

                                                PdfPCell Htitle6 = new PdfPCell(new Phrase("Delete", TblFontBold));
                                                Htitle6.HorizontalAlignment = Element.ALIGN_CENTER;
                                                Htitle6.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                                                Htitle6.BorderColorTop = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                                                Htitle6.BorderColor = BaseColor.WHITE;
                                                Htitle6.FixedHeight = 15f;
                                                tblScrnRpts.AddCell(Htitle6);
                                                #endregion

                                                string PrvDesc = "";

                                                foreach (DataRow drSRCN in drScreens)
                                                {

                                                    if (drSRCN["EFR_DESCRIPTION"].ToString() != PrvDesc)
                                                    {
                                                        PdfPCell Cell1 = new PdfPCell(new Phrase(" " + drSRCN["EFR_DESCRIPTION"].ToString(), TableFont));
                                                        Cell1.HorizontalAlignment = Element.ALIGN_LEFT;
                                                        Cell1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                                        Cell1.BorderColor = BaseColor.WHITE;
                                                        Cell1.FixedHeight = 15f;
                                                        tblScrnRpts.AddCell(Cell1);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell Cell1 = new PdfPCell(new Phrase(" ", TableFont));
                                                        Cell1.HorizontalAlignment = Element.ALIGN_LEFT;
                                                        Cell1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                                        Cell1.BorderColor = BaseColor.WHITE;
                                                        Cell1.FixedHeight = 15f;
                                                        tblScrnRpts.AddCell(Cell1);
                                                    }


                                                    PrvDesc = drSRCN["EFR_DESCRIPTION"].ToString();

                                                    PdfPCell Cell2 = new PdfPCell(new Phrase(" " + drSRCN["EFR_HIERARCHY"].ToString(), TableFont));
                                                    Cell2.HorizontalAlignment = Element.ALIGN_CENTER;
                                                    Cell2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                                                    Cell2.BorderColor = BaseColor.WHITE;
                                                    Cell2.FixedHeight = 15f;
                                                    tblScrnRpts.AddCell(Cell2);


                                                    string _imgicon = "";

                                                    if (drSRCN["EFR_INQ_PRIV"].ToString() == "N")
                                                        _imgicon = "";
                                                    else if (drSRCN["EFR_INQ_PRIV"].ToString() == "Y")
                                                        _imgicon = "P";

                                                    PdfPCell Cell3 = new PdfPCell(new Phrase(_imgicon, FontWingdings));
                                                    //Cell3.AddElement(new Chunk(imgViewIcon, 13, 13));
                                                    Cell3.HorizontalAlignment = Element.ALIGN_CENTER;
                                                    Cell3.VerticalAlignment = Element.ALIGN_MIDDLE;
                                                    Cell3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                    Cell3.BorderColor = BaseColor.WHITE;
                                                    Cell3.FixedHeight = 15f;
                                                    tblScrnRpts.AddCell(Cell3);


                                                    if (drSRCN["EFR_ADD_PRIV"].ToString() == "N")
                                                        _imgicon = "";
                                                   else if (drSRCN["EFR_ADD_PRIV"].ToString() == "N")
                                                        _imgicon = "P";

                                                    PdfPCell Cell4 = new PdfPCell(new Phrase(_imgicon, FontWingdings));
                                                    Cell4.HorizontalAlignment = Element.ALIGN_CENTER;
                                                    Cell4.VerticalAlignment = Element.ALIGN_MIDDLE;
                                                    Cell4.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                    Cell4.BorderColor = BaseColor.WHITE;
                                                    Cell4.FixedHeight = 15f;
                                                    tblScrnRpts.AddCell(Cell4);


                                                    if (drSRCN["EFR_CHG_PRIV"].ToString() == "N")
                                                        _imgicon = "";
                                                    else if (drSRCN["EFR_CHG_PRIV"].ToString() == "Y")
                                                        _imgicon = "P";

                                                    PdfPCell Cell5 = new PdfPCell(new Phrase(_imgicon, FontWingdings));
                                                    Cell5.HorizontalAlignment = Element.ALIGN_CENTER;
                                                    Cell5.VerticalAlignment = Element.ALIGN_MIDDLE;
                                                    Cell5.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                    Cell5.BorderColor = BaseColor.WHITE;
                                                    Cell5.FixedHeight = 15f;
                                                    tblScrnRpts.AddCell(Cell5);

                                                    if (drSRCN["EFR_DEL_PRIV"].ToString() == "N")
                                                        _imgicon = "";
                                                    else if (drSRCN["EFR_DEL_PRIV"].ToString() == "Y")
                                                        _imgicon = "P";
                                                    PdfPCell Cell6 = new PdfPCell(new Phrase(_imgicon, FontWingdings));
                                                    Cell6.HorizontalAlignment = Element.ALIGN_CENTER;
                                                    Cell6.VerticalAlignment = Element.ALIGN_MIDDLE;
                                                    Cell6.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                    Cell6.BorderColor = BaseColor.WHITE;
                                                    Cell6.FixedHeight = 15f;
                                                    tblScrnRpts.AddCell(Cell6);
                                                }
                                            }



                                        }
                                    }
                                    if (tblScrnRpts.Rows.Count > 0)
                                    {

                                    }

                                }
                                if (cmbReportfor.SelectedIndex == 1 || cmbReportfor.SelectedIndex == 2)
                                {
                                    if (drobjReports.Count > 0)
                                    {
                                        foreach (DataRow drCAPAPPL in dt.Rows)
                                        {
                                            List<BATCNTLEntity> drReports = drobjReports.FindAll(x => x.ModuleCode == drCAPAPPL["APPL_CODE"].ToString()).ToList();
                                            if (drReports.Count > 0)
                                            {

                                                #region HeaderTitles
                                                PdfPCell Htitle1 = new PdfPCell(new Phrase(" Reports", TblFontBold));
                                                Htitle1.HorizontalAlignment = Element.ALIGN_LEFT;
                                                Htitle1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                                                Htitle1.BorderColor = BaseColor.WHITE;
                                                Htitle1.FixedHeight = 15f;
                                                Htitle1.Colspan = 6;
                                                tblScrnRpts.AddCell(Htitle1);
                                                #endregion

                                                foreach (BATCNTLEntity lstRPT in drReports)
                                                {
                                                    PdfPCell Cell1 = new PdfPCell(new Phrase(" " + lstRPT.RepName, TableFont));
                                                    Cell1.HorizontalAlignment = Element.ALIGN_LEFT;
                                                    Cell1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                                    Cell1.BorderColor = BaseColor.WHITE;
                                                    Cell1.FixedHeight = 15f;
                                                    Cell1.Colspan = 6;
                                                    tblScrnRpts.AddCell(Cell1);
                                                }
                                            }
                                        }
                                    }
                                }

                                PdfPCell cellEmpty2 = new PdfPCell(new Phrase(" ", TblFontBold));
                                cellEmpty2.HorizontalAlignment = Element.ALIGN_LEFT;
                                cellEmpty2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                                cellEmpty2.BorderColor = BaseColor.WHITE;
                                cellEmpty2.FixedHeight = 15f;
                                cellEmpty2.Colspan = 6;
                                tblScrnRpts.AddCell(cellEmpty2);

                                document.Add(tblScrnRpts);
                                tblScrnRpts.DeleteBodyRows();
                            }

                        }



                    }
                }

                catch (Exception ex) { document.Add(new Paragraph("Aborted due to Exception............................................... " )); }

                document.Close();
                fs.Close();
                fs.Dispose();
                AlertBox.Show("Report Generated Successfully.");

            }

        }
        private void On_SaveForm_Closed(object sender, FormClosedEventArgs e)
        {
            PdfListForm form = sender as PdfListForm;
            if (form.DialogResult == DialogResult.OK)
            {
                X_Pos = 30; Y_Pos = 720;
                string ModuleDesc = null, PrvUser = null, PrvModule = null, PrvScr = null;
                CaptainModel model = new CaptainModel();
                List<EMPLFUNCEntity> Cust = model.ADMNB002Data.ADMNB002_GetScrPrivbyUserId(UserName, ModuleCode);
                List<BATCNTLEntity> BatcntlList = model.ADMNB002Data.ADMNB002_GetRepPrivbyUserId(UserName, ModuleCode);


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

                System.IO.FileStream fs = new FileStream(PdfName, FileMode.Create);

                Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                PdfWriter writer = PdfWriter.GetInstance(document, fs);

                document.Open();

                ////Font fc = new Font(bfTimes);
                //Image logo = Image.GetInstance(Context.Server.MapPath("~\\Resources\\images\\Captain_Wheel.bmp"));
                //logo.BackgroundColor = BaseColor.WHITE;
                //logo.ScalePercent(30f);

                //Image logo1 = Image.GetInstance(Context.Server.MapPath("~\\Resources\\images\\CapSystems_Title.bmp"));
                //logo1.ScalePercent(50F);
                //logo1.SetAbsolutePosition(60, 783);
                //document.Add(logo);
                //document.Add(logo1);

                pageNumber = 1;
                cb = writer.DirectContent;

                try
                {
                    PrintHeaderPage(document, writer);  // For Report Summary

                    document.NewPage();
                    Y_Pos = 795;
                    Users = 0;
                    X_Pos = 80;
                    if (Cust.Count > 0)
                    {
                        cb.BeginText();

                        cb.SetFontAndSize(FontFactory.GetFont(FontFactory.COURIER_BOLDOBLIQUE).BaseFont, 10);
                        cb.SetColorFill(BaseColor.GRAY);
                        cb.ShowTextAligned(100, "User Tree Structure", 260, 820, 0);
                        cb.SetCMYKColorFill(0, 0, 0, 255);

                        cb.EndText();
                        FirstRun = true;
                        Y_Pos = 780;

                        foreach (EMPLFUNCEntity Entity in Cust)
                        {

                            cb.BeginText();

                            cb.SetFontAndSize(bfTimes, 10);
                            ModuleDesc = null;
                            foreach (DataRow dr in dt.Rows)
                            {
                                if (dr["APPL_CODE"].ToString() == Entity.Module)
                                {
                                    ModuleDesc = dr["APPL_DESCRIPTION"].ToString(); break;
                                }
                            }

                            X_Pos = 50;


                            PrintText = " ";
                            if (PrvUser != Entity.UserName.Trim())
                            {
                                if (BatcntlList.Count > 0 && PrvUser != null)
                                {
                                    bool TempFirstRun = true;
                                    foreach (BATCNTLEntity BatcntlEntity in BatcntlList)
                                    {
                                        if (PrvUser == BatcntlEntity.UserName.Trim() && BatcntlEntity.ModuleCode.Trim() == PrvModule)
                                        {
                                            if (TempFirstRun)
                                            {
                                                PrintRec("Reports", 160);
                                                Y_Pos -= 14;
                                                CheckBottomBorderReached(document, writer);
                                                TempFirstRun = false;
                                            }

                                            X_Pos = 80;
                                            PrintRec(BatcntlEntity.RepName, 160);
                                            Y_Pos -= 14;
                                            CheckBottomBorderReached(document, writer);
                                        }
                                    }
                                }

                                if (Users > 0)
                                {
                                    Y_Pos -= 5;
                                    CheckBottomBorderReached(document, writer);
                                }
                                PrintText = Entity.UserName;
                                PrvUser = Entity.UserName.Trim();
                                PrvModule = PrvScr = null;

                                Users++;

                            }
                            else if (PrvUser == Entity.UserName.Trim() && Entity.Module.Trim() != PrvModule)
                            {
                                if (BatcntlList.Count > 0 && PrvUser != null)
                                {
                                    bool TempFirstRun = true;
                                    foreach (BATCNTLEntity BatcntlEntity in BatcntlList)
                                    {
                                        if (PrvUser == BatcntlEntity.UserName.Trim() && BatcntlEntity.ModuleCode.Trim() == PrvModule)
                                        {
                                            if (TempFirstRun)
                                            {
                                                PrintRec("Reports", 160);
                                                Y_Pos -= 14;
                                                CheckBottomBorderReached(document, writer);
                                                TempFirstRun = false;
                                            }

                                            X_Pos = 80;
                                            PrintRec(BatcntlEntity.RepName, 160);
                                            Y_Pos -= 14;
                                            CheckBottomBorderReached(document, writer);
                                        }
                                    }
                                }

                                if (Users > 0)
                                {
                                    Y_Pos -= 5;
                                    CheckBottomBorderReached(document, writer);
                                }
                                PrintText = Entity.UserName;
                                PrvUser = Entity.UserName.Trim();
                                PrvModule = PrvScr = null;
                            }
                            cb.SetColorFill(BaseColor.BLUE.Brighter());
                            cb.SetCMYKColorFill(0, 0, 0, 255);
                            cb.SetFontAndSize(bfTimes, 10);            ///Sudheer
                            PrintText = " ";
                            if (PrvModule != Entity.Module.Trim())
                            {
                                X_Pos = 50;
                                Y_Pos -= 5;
                                //PrintText = ModuleDesc;
                                PrvScr = null;
                                PrvModule = Entity.Module.Trim();
                                cb.SetColorFill(BaseColor.BLUE.Brighter());
                                PrintRec("Module:  ", 40);
                                cb.SetCMYKColorFill(0, 0, 0, 255);
                                PrintRec(ModuleDesc, 210);

                                cb.SetColorFill(BaseColor.BLUE.Brighter());
                                PrintRec("User_ID: ", 45);
                                cb.SetCMYKColorFill(0, 0, 0, 255);
                                PrintRec(Entity.UserName.Trim(), 100);

                                cb.SetColorFill(BaseColor.BLUE.Brighter());
                                PrintRec("Name: ", 30);
                                cb.SetCMYKColorFill(0, 0, 0, 255);
                                PrintRec(Entity.Name.Trim(), 80);

                                Y_Pos -= 14;
                                CheckBottomBorderReached(document, writer);
                                X_Pos = 50;
                                cb.SetColorFill(BaseColor.BLUE.Brighter());
                                PrintRec("Hierarchies: ", 50);
                                cb.SetCMYKColorFill(0, 0, 0, 255);

                                DataSet dsModuleHie = Captain.DatabaseLayer.ADMNB002DB.ADMNB002_GetModuleHierarchies(Entity.UserName.Trim(), PrvModule);
                                DataTable dtModuleHie = dsModuleHie.Tables[0];
                                int i = 0;
                                bool MultLine = false;
                                string Str = null;
                                foreach (DataRow dr in dtModuleHie.Rows)
                                {
                                    if (i == 6)
                                    {
                                        PrintRec(Str, 70);
                                        if (!MultLine)
                                        {
                                            X_Pos = 370;
                                            cb.SetColorFill(BaseColor.BLUE.Brighter());
                                            PrintRec("Case Worker: ", 58);
                                            cb.SetCMYKColorFill(0, 0, 0, 255);
                                            PrintRec(Entity.CaseWoeker, 55);
                                        }
                                        Y_Pos -= 14;
                                        X_Pos = 100;
                                        Str = null;
                                        i = 0;
                                        CheckBottomBorderReached(document, writer);
                                        MultLine = true;
                                    }
                                    if (i > 0)
                                        Str += ", ";
                                    Str += dr["Hierarchy"].ToString();
                                    i++;
                                }
                                PrintRec(Str, 70);


                                if (!MultLine)
                                {
                                    X_Pos = 370;
                                    cb.SetColorFill(BaseColor.BLUE.Brighter());
                                    PrintRec("Case Worker: ", 58);
                                    cb.SetCMYKColorFill(0, 0, 0, 255);
                                    PrintRec(Entity.CaseWoeker, 55);
                                }


                                Y_Pos -= 19;
                                CheckBottomBorderReached(document, writer);


                                cb.SetColorFill(BaseColor.GRAY);
                                cb.ShowTextAligned(Element.ALIGN_RIGHT, "Privileges", 500, Y_Pos, 0);
                                Y_Pos -= 19;
                                CheckBottomBorderReached(document, writer);
                                X_Pos = 110;
                                PrintHeader();
                                X_Pos = 50;
                                Y_Pos += 5;
                                CheckBottomBorderReached(document, writer);
                                cb.EndText();
                                cb.SetLineWidth(0.7f);
                                cb.MoveTo(X_Pos, Y_Pos);                       //header line...
                                cb.LineTo(580, Y_Pos);
                                cb.Stroke();

                                cb.BeginText();
                                X_Pos = 80;
                                Y_Pos -= 15;
                                CheckBottomBorderReached(document, writer);
                                X_Pos = 50;

                            }

                            TmpCount_yeswanth++;

                            cb.SetFontAndSize(bfTimes, 10);
                            cb.SetCMYKColorFill(0, 0, 0, 255);
                            if (Entity.ScrDesc.Trim() != PrvScr)
                            {
                                PrvScr = Entity.ScrDesc.Trim();
                                PrintRec(Entity.ScrDesc.Trim(), 285);
                            }
                            else
                                PrintRec(" ", 285);
                            PrintRec(Entity.Hierarchy, 100);
                            PrintRec(Entity.Inq_Priv, 35);
                            PrintRec(Entity.Add_Priv, 35);
                            PrintRec(Entity.Chg_Priv, 38);
                            PrintRec(Entity.Del_Priv, 25);
                            Y_Pos -= 14;
                            CheckBottomBorderReached(document, writer);

                            cb.EndText();
                        }

                        cb.BeginText();
                        if (Y_Pos >= 20)
                        {
                            Y_Pos = 07;
                            X_Pos = 20;
                            cb.SetFontAndSize(bfTimes, 10);
                            cb.SetCMYKColorFill(0, 0, 0, 255);
                            PrintRec(DateTime.Now.ToLocalTime().ToString(), 130);

                            X_Pos = 555;
                            PrintRec("Page:", 23);
                            PrintRec(pageNumber.ToString(), 15);
                        }

                        cb.EndText();
                    }
                }
                catch (Exception ex) { document.Add(new Paragraph("Aborted due to Exception............................................... ")); }

                document.Close();
                fs.Close();
                fs.Dispose();

            }
        }

        private void On_ExcelForm_Closed(object sender, FormClosedEventArgs e)
        {
            PdfListForm form = sender as PdfListForm;
            if (form.DialogResult == DialogResult.OK)
            {
                X_Pos = 30; Y_Pos = 720;
                string ModuleDesc = null, PrvUser = null, PrvModule = null, PrvScr = null;
                CaptainModel model = new CaptainModel();
                List<EMPLFUNCEntity> Cust = model.ADMNB002Data.ADMNB002_GetScrPrivbyUserId(UserName, ModuleCode);
                List<BATCNTLEntity> BatcntlList = model.ADMNB002Data.ADMNB002_GetRepPrivbyUserId(UserName, ModuleCode);


                string PdfName = "Access File";
                PdfName = form.GetFileName();


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

                //System.IO.FileStream fs = new FileStream(PdfName, FileMode.Create);

                //Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                //PdfWriter writer = PdfWriter.GetInstance(document, fs);

                //document.Open();





                try
                {

                    Workbook book = new Workbook();


                    book.ExcelWorkbook.WindowHeight = 10005;
                    book.ExcelWorkbook.WindowWidth = 10005;
                    book.ExcelWorkbook.WindowTopX = 120;
                    book.ExcelWorkbook.WindowTopY = 135;
                    book.ExcelWorkbook.ProtectWindows = false;
                    book.ExcelWorkbook.ProtectStructure = false;

                    Worksheet sheet; WorksheetCell cell; WorksheetRow Row0;

                    this.GenerateStyles(book.Styles);

                    GenerateWorksheetParameters(book.Worksheets);

                    //sheet = book.Worksheets.Add("Hierarchy_Access_UserID");
                    //sheet.Table.DefaultRowHeight = 14.25F;

                    //sheet.Table.Columns.Add(30);
                    //sheet.Table.Columns.Add(80);
                    //sheet.Table.Columns.Add(100);


                    sheet = book.Worksheets.Add("Hierarchy_Access_UserID");
                    sheet.Options.GridLineColor = "#FFFFFF";
                    sheet.Table.DefaultRowHeight = 14.25F;
                    //sheet.Table.ExpandedColumnCount = 6;
                    //sheet.Table.ExpandedRowCount = 47;
                    //sheet.Table.FullColumns = 1;
                    //sheet.Table.FullRows = 1;
                    sheet.Table.Columns.Add(15);
                    sheet.Table.Columns.Add(80);
                    sheet.Table.Columns.Add(148);
                    // -----------------------------------------------
                    Row0 = sheet.Table.Rows.Add();
                    Row0.Height = 23;
                    Row0.AutoFitHeight = false;






                    // Row0 = sheet.Table.Rows.Add();


                    int excelcolumn = 0;

                    try
                    {


                        string programName = string.Empty;
                        string hierarchy = Agency + Dept + Prog;
                        if (!hierarchy.Equals(string.Empty)) { hierarchy = hierarchy.Replace("**", string.Empty); }

                        List<ProgramDefinitionEntity> programEntityList = _model.HierarchyAndPrograms.GetPrograms(programName, hierarchy, BaseForm.UserID, BaseForm.BaseAdminAgency);

                        string strUserId = string.Empty;
                        if (((Captain.Common.Utilities.ListItem)CmbUser.SelectedItem).Value.ToString().Trim() != "00" && ((Captain.Common.Utilities.ListItem)CmbUser.SelectedItem).Value.ToString().Trim() != "**")
                            strUserId = ((Captain.Common.Utilities.ListItem)CmbUser.SelectedItem).Value.ToString().Trim();


                        List<HierarchyEntity> Hieruserlist = _model.CaseMstData.CAPS_ADMNB002_GET(string.Empty, Agency, Dept, Prog, strUserId, "ADMNB002");

                        cell = Row0.Cells.Add();
                        cell.StyleID = "m169381600";
                        cell.Data.Type = DataType.String;
                        cell.Data.Text = "Hierarchy Access Report";
                        cell.Index = 2;
                        cell.MergeAcross = 1;
                        cell = Row0.Cells.Add();
                        cell.StyleID = "s63";
                        cell = Row0.Cells.Add();
                        cell.StyleID = "s63";
                        cell = Row0.Cells.Add();
                        cell.StyleID = "s63";
                        // -----------------------------------------------
                        WorksheetRow Row1 = sheet.Table.Rows.Add();
                        Row1.Height = 23;
                        Row1.AutoFitHeight = false;
                        cell = Row1.Cells.Add();
                        cell.StyleID = "s106";
                        cell.Data.Type = DataType.String;
                        cell.Data.Text = "Hierarchy";
                        cell.Index = 2;
                        Row1.Cells.Add("User ID", DataType.String, "s103");
                        cell = Row1.Cells.Add();
                        cell.StyleID = "s63";
                        cell = Row1.Cells.Add();
                        cell.StyleID = "s63";
                        cell = Row1.Cells.Add();
                        cell.StyleID = "s63";



                        if (programEntityList.Count > 0)
                        {



                            int intfirst = 0;
                            foreach (ProgramDefinitionEntity programEntity in programEntityList)
                            {


                                string code = programEntity.Agency + "-" + programEntity.Dept + "-" + programEntity.Prog;
                                intfirst = 0;
                                if (strUserId == string.Empty)
                                {
                                    //Row0 = sheet.Table.Rows.Add();
                                    //cell = Row0.Cells.Add("", DataType.String, "s65");
                                    //cell = Row0.Cells.Add(code, DataType.String, "s118");

                                    WorksheetRow Row4 = sheet.Table.Rows.Add();
                                    Row4.AutoFitHeight = false;
                                    cell = Row4.Cells.Add();
                                    cell.StyleID = "s65";
                                    cell.Data.Type = DataType.String;
                                    Row4.Cells.Add(code, DataType.String, "s119");

                                    bool boolexist = false;
                                    foreach (HierarchyEntity item in Hieruserlist)
                                    {
                                        boolexist = false;

                                        if ((programEntity.Agency + programEntity.Dept + programEntity.Prog) == (item.Agency + item.Dept + item.Prog))
                                            boolexist = true;
                                        if (item.Agency == "**")
                                            boolexist = true;
                                        if (item.Agency == programEntity.Agency && item.Dept == "**")
                                            boolexist = true;
                                        if (item.Agency == programEntity.Agency && item.Dept == programEntity.Dept && item.Prog == "**")
                                            boolexist = true;
                                        if (boolexist)
                                        {
                                            if (intfirst == 0)
                                            {
                                                cell = Row4.Cells.Add(item.UserID, DataType.String, "s118");

                                                intfirst = intfirst + 1;
                                            }
                                            else
                                            {
                                                WorksheetRow Row5 = sheet.Table.Rows.Add();
                                                Row5.AutoFitHeight = false;
                                                cell = Row5.Cells.Add();
                                                cell.StyleID = "s65";
                                                cell.Data.Type = DataType.String;
                                                Row5.Cells.Add("", DataType.String, "s119");
                                                Row5.Cells.Add(item.UserID, DataType.String, "s118");
                                                //Row0 = sheet.Table.Rows.Add();
                                                //cell = Row0.Cells.Add("", DataType.String, "s65");
                                                //cell = Row0.Cells.Add("", DataType.String, "s109");
                                                //cell = Row0.Cells.Add(item.UserID, DataType.String, "s118");
                                            }
                                        }


                                    }


                                    WorksheetRow Row2 = sheet.Table.Rows.Add();
                                    Row2.AutoFitHeight = false;
                                    cell = Row2.Cells.Add();
                                    cell.StyleID = "s65";
                                    cell.Data.Type = DataType.String;
                                    Row2.Cells.Add("", DataType.String, "s119");
                                    Row2.Cells.Add("", DataType.String, "s118");


                                }
                                else
                                {

                                    List<HierarchyEntity> hierchyuserlist = Hieruserlist.FindAll(u => u.UserID.Trim() == strUserId.Trim() && (u.Agency == programEntity.Agency || u.Agency == "**") && (u.Dept == programEntity.Dept || u.Dept == "**") && (u.Prog == programEntity.Prog || u.Prog == "**"));
                                    if (hierchyuserlist.Count > 0)
                                    {
                                        //Row0 = sheet.Table.Rows.Add();
                                        //cell = Row0.Cells.Add("", DataType.String, "s65");
                                        //cell = Row0.Cells.Add(code, DataType.String, "s109");
                                        //cell = Row0.Cells.Add(hierchyuserlist[0].UserID, DataType.String, "s118");

                                        WorksheetRow Row2 = sheet.Table.Rows.Add();
                                        Row2.AutoFitHeight = false;
                                        cell = Row2.Cells.Add();
                                        cell.StyleID = "s65";
                                        cell.Data.Type = DataType.String;
                                        Row2.Cells.Add(code, DataType.String, "s119");
                                        Row2.Cells.Add(hierchyuserlist[0].UserID, DataType.String, "s118");


                                        WorksheetRow Row6 = sheet.Table.Rows.Add();
                                        Row6.AutoFitHeight = false;
                                        cell = Row6.Cells.Add();
                                        cell.StyleID = "s65";
                                        cell.Data.Type = DataType.String;
                                        Row6.Cells.Add("", DataType.String, "s119");
                                        Row6.Cells.Add("", DataType.String, "s118");
                                    }


                                }


                            }

                            WorksheetRow Rowlast = sheet.Table.Rows.Add();
                            Rowlast.AutoFitHeight = false;
                            cell = Rowlast.Cells.Add();
                            cell.StyleID = "s65";
                            cell.Data.Type = DataType.String;
                            Rowlast.Cells.Add("", DataType.String, "s120");
                            Rowlast.Cells.Add("", DataType.String, "s120");

                        }

                        FileStream stream = new FileStream(PdfName, FileMode.Create);

                        book.Save(stream);
                        AlertBox.Show("Report Generated Successfully");
                        stream.Close();



                    }
                    catch (Exception ex) { }

                }
                catch (Exception ex) { }



            }
        }



        private void GenerateWorksheetParameters(WorksheetCollection sheets)
        {

            Worksheet sheet = sheets.Add("Parameters");
            sheet.Table.DefaultRowHeight = 15F;
            //sheet.Table.ExpandedColumnCount = 11;
            //sheet.Table.ExpandedRowCount = 18;
            sheet.Table.FullColumns = 1;
            sheet.Table.FullRows = 1;
            sheet.Options.GridLineColor = "#FFFFFF";
            WorksheetColumn column0 = sheet.Table.Columns.Add();
            column0.Index = 6;
            column0.Width = 14;
            // -----------------------------------------------
            WorksheetRow Row0 = sheet.Table.Rows.Add();
            Row0.Index = 3;
            WorksheetCell cell;
            cell = Row0.Cells.Add();
            cell.StyleID = "s1201";
            cell.Index = 2;
            cell = Row0.Cells.Add();
            cell.StyleID = "s120";
            cell = Row0.Cells.Add();
            cell.StyleID = "s120";
            cell = Row0.Cells.Add();
            cell.StyleID = "s120";
            cell = Row0.Cells.Add();
            cell.StyleID = "s120";
            cell = Row0.Cells.Add();
            cell.StyleID = "s120";
            cell = Row0.Cells.Add();
            cell.StyleID = "s120";
            cell = Row0.Cells.Add();
            cell.StyleID = "s120";
            cell = Row0.Cells.Add();
            cell.StyleID = "s120";
            cell = Row0.Cells.Add();
            cell.StyleID = "s108";
            // -----------------------------------------------
            WorksheetRow Row1 = sheet.Table.Rows.Add();
            Row1.Height = 21;
            cell = Row1.Cells.Add();
            cell.StyleID = "m44272968";
            cell.Data.Type = DataType.String;
            cell.Data.Text = Privileges.Program + " - " + Privileges.PrivilegeName;
            cell.Index = 2;
            cell.MergeAcross = 9;
            // -----------------------------------------------
            WorksheetRow Row2 = sheet.Table.Rows.Add();
            Row2.Height = 17;
            Row2.AutoFitHeight = false;
            cell = Row2.Cells.Add();
            cell.StyleID = "s200";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "Generated by : " + LookupDataAccess.GetMemberName(BaseForm.UserProfile.FirstName.Trim(), BaseForm.UserProfile.MI.Trim(), BaseForm.UserProfile.LastName.Trim(), "3");

            cell.Index = 2;
            cell.MergeAcross = 2;
            cell = Row2.Cells.Add();
            cell.StyleID = "s122";
            cell = Row2.Cells.Add();
            cell.StyleID = "s122";
            cell = Row2.Cells.Add();
            cell.StyleID = "s122";
            //cell = Row2.Cells.Add();
            //cell.StyleID = "s122";
            //cell = Row2.Cells.Add();
            //cell.StyleID = "s122";
            //cell = Row2.Cells.Add();
            //cell.StyleID = "s122";
            cell = Row2.Cells.Add();
            cell.StyleID = "m44273008";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "Generated Date : " + DateTime.Now.ToString();
            cell.MergeAcross = 3;


            //Worksheet sheet = sheets.Add("Parameters");
            //sheet.Table.DefaultRowHeight = 15F;

            //sheet.Table.ExpandedColumnCount = 11;
            //sheet.Table.ExpandedRowCount = 17;
            //sheet.Table.FullColumns = 1;
            //sheet.Options.GridLineColor = "#FFFFFF";
            //sheet.Table.FullRows = 1;
            //WorksheetColumn column0 = sheet.Table.Columns.Add();
            //column0.Index = 6;
            //column0.Width = 14;
            //// -----------------------------------------------
            //WorksheetRow Row0 = sheet.Table.Rows.Add();
            //Row0.Index = 3;
            //WorksheetCell cell;
            //cell = Row0.Cells.Add();
            //cell.StyleID = "s119";
            //cell.Index = 2;
            //cell = Row0.Cells.Add();
            //cell.StyleID = "s120";
            //cell = Row0.Cells.Add();
            //cell.StyleID = "s120";
            //cell = Row0.Cells.Add();
            //cell.StyleID = "s120";
            //cell = Row0.Cells.Add();
            //cell.StyleID = "s120";
            //cell = Row0.Cells.Add();
            //cell.StyleID = "s120";
            //cell = Row0.Cells.Add();
            //cell.StyleID = "s120";
            //cell = Row0.Cells.Add();
            //cell.StyleID = "s120";
            //cell = Row0.Cells.Add();
            //cell.StyleID = "s120";
            //cell = Row0.Cells.Add();
            //cell.StyleID = "s108";
            //// -----------------------------------------------
            //WorksheetRow Row1 = sheet.Table.Rows.Add();
            //Row1.Height = 21;
            //cell = Row1.Cells.Add();
            //cell.StyleID = "m44272968";
            //cell.Data.Type = DataType.String;
            //cell.Data.Text = Privileges.Program + " - " + Privileges.PrivilegeName;
            //cell.Index = 2;
            //cell.MergeAcross = 9;
            //// -----------------------------------------------
            //WorksheetRow Row2 = sheet.Table.Rows.Add();
            //Row2.Height = 17;
            //Row2.AutoFitHeight = false;
            //cell = Row2.Cells.Add();
            //cell.StyleID = "s200";
            //cell.Data.Type = DataType.String;
            //cell.Data.Text = "Run by : " + LookupDataAccess.GetMemberName(BaseForm.UserProfile.FirstName.Trim(), BaseForm.UserProfile.MI.Trim(), BaseForm.UserProfile.LastName.Trim(), "3");
            //cell.Index = 2;
            //cell.MergeAcross = 1;
            //cell = Row2.Cells.Add();
            //cell.StyleID = "s122";
            //cell = Row2.Cells.Add();
            //cell.StyleID = "s122";
            //cell = Row2.Cells.Add();
            //cell.StyleID = "s122";
            //cell = Row2.Cells.Add();
            //cell.StyleID = "s122";
            //cell = Row2.Cells.Add();
            //cell.StyleID = "s122";
            //cell = Row2.Cells.Add();
            //cell.StyleID = "s122";
            //cell = Row2.Cells.Add();
            //cell.StyleID = "m44273008";
            //cell.Data.Type = DataType.String;
            //cell.Data.Text = "Date : " + DateTime.Now.ToString();
            //cell.MergeAcross = 1;
            // -----------------------------------------------
            WorksheetRow Row3 = sheet.Table.Rows.Add();
            Row3.Height = 18;
            cell = Row3.Cells.Add();
            cell.StyleID = "m44272948";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "Report Parameters";
            cell.Index = 2;
            cell.MergeAcross = 9;
            // -----------------------------------------------
            WorksheetRow Row4 = sheet.Table.Rows.Add();
            cell = Row4.Cells.Add();
            cell.StyleID = "s121";
            cell.Index = 2;
            cell = Row4.Cells.Add();
            cell.StyleID = "s122";
            cell = Row4.Cells.Add();
            cell.StyleID = "s122";
            cell = Row4.Cells.Add();
            cell.StyleID = "s122";
            cell = Row4.Cells.Add();
            cell.StyleID = "s122";
            cell = Row4.Cells.Add();
            cell.StyleID = "s122";
            cell = Row4.Cells.Add();
            cell.StyleID = "s122";
            cell = Row4.Cells.Add();
            cell.StyleID = "s122";
            cell = Row4.Cells.Add();
            cell.StyleID = "s122";
            cell = Row4.Cells.Add();
            cell.StyleID = "s111";
            // -----------------------------------------------
            WorksheetRow Row5 = sheet.Table.Rows.Add();
            cell = Row5.Cells.Add();
            cell.StyleID = "s145";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "Report Type";
            cell.Index = 2;
            cell.MergeAcross = 3;

            cell = Row5.Cells.Add();
            cell.Data.Text = ":";
            cell.StyleID = "s147";

            cell = Row5.Cells.Add();
            cell.StyleID = "s151";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "User Access";
            cell.MergeAcross = 3;
            cell = Row5.Cells.Add();
            cell.StyleID = "s111";
            // -----------------------------------------------
            // -----------------------------------------------

            string strReportType = "";
            if (cmbReportfor.SelectedIndex == 0)
                strReportType = "Screens Only";
            if (cmbReportfor.SelectedIndex == 1)
                strReportType = "Reports Only";
            if (cmbReportfor.SelectedIndex == 2)
                strReportType = "Both Screens and Reports";

            WorksheetRow Row51 = sheet.Table.Rows.Add();
            cell = Row51.Cells.Add();
            cell.StyleID = "s145";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "Report For";
            cell.Index = 2;
            cell.MergeAcross = 3;

            cell = Row51.Cells.Add();
            cell.Data.Text = ":";
            cell.StyleID = "s147";

            cell = Row51.Cells.Add();
            cell.StyleID = "s151";
            cell.Data.Type = DataType.String;
            cell.Data.Text = strReportType;
            cell.MergeAcross = 3;

            cell = Row51.Cells.Add();
            cell.StyleID = "s111";
            // -----------------------------------------------
            WorksheetRow Row6 = sheet.Table.Rows.Add();
            cell = Row6.Cells.Add();
            cell.StyleID = "s145";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "User ID";
            cell.Index = 2;
            cell.MergeAcross = 3;

            cell = Row6.Cells.Add();
            cell.Data.Text = ":";
            cell.StyleID = "s122";

            cell = Row6.Cells.Add();
            cell.StyleID = "s151";
            cell.Data.Type = DataType.String;
            cell.Data.Text = ((Captain.Common.Utilities.ListItem)CmbUser.SelectedItem).Text.ToString();
            cell.MergeAcross = 2;
            cell = Row6.Cells.Add();
            cell.StyleID = "s122";
            cell = Row6.Cells.Add();
            cell.StyleID = "s111";
            // -----------------------------------------------
            WorksheetRow Row7 = sheet.Table.Rows.Add();
            cell = Row7.Cells.Add();
            cell.StyleID = "s121";
            cell.Index = 2;
            cell = Row7.Cells.Add();
            cell.StyleID = "s157";
            cell.Data.Type = DataType.String;
            cell.Data.Text = "Hierarchy";
            cell.MergeAcross = 2;
            cell = Row7.Cells.Add();
            cell.StyleID = "s122";
            cell.Data.Text = ":";

            Row7.Cells.Add(txtHierchy.Text, DataType.String, "s158");
            cell = Row7.Cells.Add();
            cell.StyleID = "s122";
            cell = Row7.Cells.Add();
            cell.StyleID = "s122";
            cell = Row7.Cells.Add();
            cell.StyleID = "s122";
            cell = Row7.Cells.Add();
            cell.StyleID = "s111";
            // -----------------------------------------------
            WorksheetRow Row8 = sheet.Table.Rows.Add();
            cell = Row8.Cells.Add();
            cell.StyleID = "s121";
            cell.Index = 2;
            cell = Row8.Cells.Add();
            cell.StyleID = "s122";
            cell = Row8.Cells.Add();
            cell.StyleID = "s122";
            cell = Row8.Cells.Add();
            cell.StyleID = "s122";
            cell = Row8.Cells.Add();
            cell.StyleID = "s122";
            cell = Row8.Cells.Add();
            cell.StyleID = "s122";
            cell = Row8.Cells.Add();
            cell.StyleID = "s122";
            cell = Row8.Cells.Add();
            cell.StyleID = "s122";
            cell = Row8.Cells.Add();
            cell.StyleID = "s122";
            cell = Row8.Cells.Add();
            cell.StyleID = "s111";
            // -----------------------------------------------
            WorksheetRow Row9 = sheet.Table.Rows.Add();
            cell = Row9.Cells.Add();
            cell.StyleID = "s121";
            cell.Index = 2;
            cell = Row9.Cells.Add();
            cell.StyleID = "s122";
            cell = Row9.Cells.Add();
            cell.StyleID = "s122";
            cell = Row9.Cells.Add();
            cell.StyleID = "s122";
            cell = Row9.Cells.Add();
            cell.StyleID = "s122";
            cell = Row9.Cells.Add();
            cell.StyleID = "s122";
            cell = Row9.Cells.Add();
            cell.StyleID = "s122";
            cell = Row9.Cells.Add();
            cell.StyleID = "s122";
            cell = Row9.Cells.Add();
            cell.StyleID = "s122";
            cell = Row9.Cells.Add();
            cell.StyleID = "s111";
            // -----------------------------------------------
            WorksheetRow Row10 = sheet.Table.Rows.Add();
            cell = Row10.Cells.Add();
            cell.StyleID = "s121";
            cell.Index = 2;
            cell = Row10.Cells.Add();
            cell.StyleID = "s122";
            cell = Row10.Cells.Add();
            cell.StyleID = "s122";
            cell = Row10.Cells.Add();
            cell.StyleID = "s122";
            cell = Row10.Cells.Add();
            cell.StyleID = "s122";
            cell = Row10.Cells.Add();
            cell.StyleID = "s122";
            cell = Row10.Cells.Add();
            cell.StyleID = "s122";
            cell = Row10.Cells.Add();
            cell.StyleID = "s122";
            cell = Row10.Cells.Add();
            cell.StyleID = "s122";
            cell = Row10.Cells.Add();
            cell.StyleID = "s111";
            // -----------------------------------------------
            WorksheetRow Row11 = sheet.Table.Rows.Add();
            cell = Row11.Cells.Add();
            cell.StyleID = "s121";
            cell.Index = 2;
            cell = Row11.Cells.Add();
            cell.StyleID = "s122";
            cell = Row11.Cells.Add();
            cell.StyleID = "s122";
            cell = Row11.Cells.Add();
            cell.StyleID = "s122";
            cell = Row11.Cells.Add();
            cell.StyleID = "s122";
            cell = Row11.Cells.Add();
            cell.StyleID = "s122";
            cell = Row11.Cells.Add();
            cell.StyleID = "s122";
            cell = Row11.Cells.Add();
            cell.StyleID = "s122";
            cell = Row11.Cells.Add();
            cell.StyleID = "s122";
            cell = Row11.Cells.Add();
            cell.StyleID = "s111";
            // -----------------------------------------------
            WorksheetRow Row12 = sheet.Table.Rows.Add();
            cell = Row12.Cells.Add();
            cell.StyleID = "s121";
            cell.Index = 2;
            cell = Row12.Cells.Add();
            cell.StyleID = "s122";
            cell = Row12.Cells.Add();
            cell.StyleID = "s122";
            cell = Row12.Cells.Add();
            cell.StyleID = "s122";
            cell = Row12.Cells.Add();
            cell.StyleID = "s122";
            cell = Row12.Cells.Add();
            cell.StyleID = "s122";
            cell = Row12.Cells.Add();
            cell.StyleID = "s122";
            cell = Row12.Cells.Add();
            cell.StyleID = "s122";
            cell = Row12.Cells.Add();
            cell.StyleID = "s122";
            cell = Row12.Cells.Add();
            cell.StyleID = "s111";
            // -----------------------------------------------
            WorksheetRow Row13 = sheet.Table.Rows.Add();
            cell = Row13.Cells.Add();
            cell.StyleID = "s121";
            cell.Index = 2;
            cell = Row13.Cells.Add();
            cell.StyleID = "s122";
            cell = Row13.Cells.Add();
            cell.StyleID = "s122";
            cell = Row13.Cells.Add();
            cell.StyleID = "s122";
            cell = Row13.Cells.Add();
            cell.StyleID = "s122";
            cell = Row13.Cells.Add();
            cell.StyleID = "s122";
            cell = Row13.Cells.Add();
            cell.StyleID = "s122";
            cell = Row13.Cells.Add();
            cell.StyleID = "s122";
            cell = Row13.Cells.Add();
            cell.StyleID = "s122";
            cell = Row13.Cells.Add();
            cell.StyleID = "s111";
            // -----------------------------------------------
            WorksheetRow Row14 = sheet.Table.Rows.Add();
            cell = Row14.Cells.Add();
            cell.StyleID = "s123";
            cell.Index = 2;
            cell = Row14.Cells.Add();
            cell.StyleID = "s124";
            cell = Row14.Cells.Add();
            cell.StyleID = "s124";
            cell = Row14.Cells.Add();
            cell.StyleID = "s124";
            cell = Row14.Cells.Add();
            cell.StyleID = "s124";
            cell = Row14.Cells.Add();
            cell.StyleID = "s124";
            cell = Row14.Cells.Add();
            cell.StyleID = "s124";
            cell = Row14.Cells.Add();
            cell.StyleID = "s124";
            cell = Row14.Cells.Add();
            cell.StyleID = "s124";
            cell = Row14.Cells.Add();
            cell.StyleID = "s113";
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
            //  s38
            // -----------------------------------------------
            WorksheetStyle s38 = styles.Add("s38");
            s38.Name = "Accent1";
            s38.Font.FontName = "Calibri";
            s38.Font.Size = 11;
            s38.Font.Color = "#FFFFFF";
            s38.Interior.Color = "#4F81BD";
            s38.Interior.Pattern = StyleInteriorPattern.Solid;
            // -----------------------------------------------
            //  m44272948
            // -----------------------------------------------
            WorksheetStyle m44272948 = styles.Add("m44272948");
            m44272948.Font.Bold = true;
            m44272948.Font.FontName = "Calibri";
            m44272948.Font.Size = 14;
            m44272948.Font.Color = "#0D0D0D";
            m44272948.Interior.Color = "#BFBFBF";
            m44272948.Interior.Pattern = StyleInteriorPattern.Solid;
            m44272948.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            m44272948.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            m44272948.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            m44272948.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  m44272968
            // -----------------------------------------------
            WorksheetStyle m44272968 = styles.Add("m44272968");
            m44272968.Font.Bold = true;
            m44272968.Font.FontName = "Calibri";
            m44272968.Font.Size = 16;
            m44272968.Font.Color = "#4F81BD";
            m44272968.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            m44272968.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            m44272968.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            m44272968.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  m44273008
            // -----------------------------------------------
            WorksheetStyle m44273008 = styles.Add("m44273008");
            m44273008.Font.FontName = "Calibri";
            m44273008.Font.Size = 8;
            m44273008.Font.Color = "#000000";
            m44273008.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            m44273008.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            m44273008.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  m169381600
            // -----------------------------------------------
            WorksheetStyle m169381600 = styles.Add("m169381600");
            m169381600.Parent = "s38";
            m169381600.Font.Bold = true;
            m169381600.Font.FontName = "Calibri";
            m169381600.Font.Size = 14;
            m169381600.Font.Color = "#0D0D0D";
            m169381600.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            m169381600.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            m169381600.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            m169381600.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            m169381600.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            m169381600.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s63
            // -----------------------------------------------
            WorksheetStyle s63 = styles.Add("s63");
            s63.Font.Bold = true;
            s63.Font.FontName = "Calibri";
            s63.Font.Size = 18;
            s63.Font.Color = "#000000";
            s63.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            s63.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s65
            // -----------------------------------------------
            WorksheetStyle s65 = styles.Add("s65");
            s65.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s65.Alignment.Vertical = StyleVerticalAlignment.Center;
            s65.NumberFormat = "@";
            // -----------------------------------------------
            //  s103
            // -----------------------------------------------
            WorksheetStyle s103 = styles.Add("s103");
            s103.Parent = "s38";
            s103.Font.Bold = true;
            s103.Font.FontName = "Calibri";
            s103.Font.Size = 11;
            s103.Font.Color = "#0D0D0D";
            s103.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s103.Alignment.Vertical = StyleVerticalAlignment.Center;
            s103.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s103.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s106
            // -----------------------------------------------
            WorksheetStyle s106 = styles.Add("s106");
            s106.Parent = "s38";
            s106.Font.Bold = true;
            s106.Font.FontName = "Calibri";
            s106.Font.Size = 11;
            s106.Font.Color = "#0D0D0D";
            s106.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s106.Alignment.Vertical = StyleVerticalAlignment.Center;
            s106.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s106.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s106.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s106.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s107
            // -----------------------------------------------
            WorksheetStyle s107 = styles.Add("s107");
            s107.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s107.Alignment.Vertical = StyleVerticalAlignment.Center;
            s107.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s107.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            s107.NumberFormat = "@";
            // -----------------------------------------------
            //  s108
            // -----------------------------------------------
            WorksheetStyle s108 = styles.Add("s108");
            s108.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s108.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s109
            // -----------------------------------------------
            WorksheetStyle s109 = styles.Add("s109");
            s109.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s109.Alignment.Vertical = StyleVerticalAlignment.Center;
            s109.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s109.NumberFormat = "@";
            // -----------------------------------------------
            //  s110
            // -----------------------------------------------
            WorksheetStyle s110 = styles.Add("s110");
            s110.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s110.Alignment.Vertical = StyleVerticalAlignment.Center;
            s110.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s110.NumberFormat = "@";
            // -----------------------------------------------
            //  s111
            // -----------------------------------------------
            WorksheetStyle s111 = styles.Add("s111");
            s111.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s112
            // -----------------------------------------------
            WorksheetStyle s112 = styles.Add("s112");
            s112.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s112.Alignment.Vertical = StyleVerticalAlignment.Center;
            s112.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s112.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            s112.NumberFormat = "@";
            // -----------------------------------------------
            //  s113
            // -----------------------------------------------
            WorksheetStyle s113 = styles.Add("s113");
            s113.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s113.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s117
            // -----------------------------------------------
            WorksheetStyle s117 = styles.Add("s117");
            s117.Font.FontName = "Calibri";
            s117.Font.Size = 11;
            s117.Font.Color = "#000000";
            s117.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s117.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s118
            // -----------------------------------------------
            WorksheetStyle s118 = styles.Add("s118");
            s118.Font.FontName = "Calibri";
            s118.Font.Size = 11;
            s118.Font.Color = "#000000";
            s118.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s119
            // -----------------------------------------------
            WorksheetStyle s119 = styles.Add("s119");
            s119.Font.FontName = "Calibri";
            s119.Font.Size = 11;
            s119.Font.Color = "#000000";
            s119.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            //s119.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s120
            // -----------------------------------------------
            WorksheetStyle s120 = styles.Add("s120");
            s120.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);

            WorksheetStyle s1201 = styles.Add("s1201");
            s1201.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1);
            s1201.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s121
            // -----------------------------------------------
            WorksheetStyle s121 = styles.Add("s121");
            s121.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s122
            // -----------------------------------------------
            WorksheetStyle s122 = styles.Add("s122");
            // -----------------------------------------------
            //  s123
            // -----------------------------------------------
            WorksheetStyle s123 = styles.Add("s123");
            s123.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            s123.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s124
            // -----------------------------------------------
            WorksheetStyle s124 = styles.Add("s124");
            s124.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s145
            // -----------------------------------------------
            WorksheetStyle s145 = styles.Add("s145");
            s145.Font.FontName = "Calibri";
            s145.Font.Size = 11;
            s145.Font.Color = "#000000";
            s145.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            s145.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s145.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
            // -----------------------------------------------
            //  s147
            // -----------------------------------------------
            WorksheetStyle s147 = styles.Add("s147");
            s147.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s151
            // -----------------------------------------------
            WorksheetStyle s151 = styles.Add("s151");
            s151.Font.FontName = "Calibri";
            s151.Font.Size = 11;
            s151.Font.Color = "#000000";
            s151.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s151.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s157
            // -----------------------------------------------
            WorksheetStyle s157 = styles.Add("s157");
            s157.Font.FontName = "Calibri";
            s157.Font.Size = 11;
            s157.Font.Color = "#000000";
            s157.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            s157.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s158
            // -----------------------------------------------
            WorksheetStyle s158 = styles.Add("s158");
            s158.Font.FontName = "Calibri";
            s158.Font.Size = 11;
            s158.Font.Color = "#000000";
            s158.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s158.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            // -----------------------------------------------
            //  s159
            // -----------------------------------------------
            WorksheetStyle s159 = styles.Add("s159");
            s159.Font.FontName = "Calibri";
            s159.Font.Size = 11;
            s159.Font.Color = "#000000";
            s159.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s159.Alignment.Vertical = StyleVerticalAlignment.Center;
            s159.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1);
            s159.NumberFormat = "@";
            // -----------------------------------------------
            //  s171
            // -----------------------------------------------
            WorksheetStyle s171 = styles.Add("s171");
            s171.Font.FontName = "Calibri";
            s171.Font.Size = 14;
            s171.Font.Color = "#000000";
            // -----------------------------------------------
            //  s200
            // -----------------------------------------------
            WorksheetStyle s200 = styles.Add("s200");
            s200.Font.FontName = "Calibri";
            s200.Font.Size = 8;
            s200.Font.Color = "#000000";
            s200.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            s200.Alignment.Vertical = StyleVerticalAlignment.Bottom;
            s200.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1);
        }



        private void On_All_List_Pdf1(object sender, FormClosedEventArgs e)
        {
            PdfListForm form = sender as PdfListForm;
            if (form.DialogResult == DialogResult.OK)
            {
                X_Pos = 30; Y_Pos = 720;
                string ModuleDesc = null, TypeModule = null, PrvModule = null, PrvScr = null;

                if (((Captain.Common.Utilities.ListItem)CmbApp.SelectedItem).Value.ToString().Trim() != "**")
                    TypeModule = ((Captain.Common.Utilities.ListItem)CmbApp.SelectedItem).Value.ToString().Trim();
                DataSet dsScreen = Captain.DatabaseLayer.ADMNB002DB.Browse_CAPFNCC(TypeModule);
                DataTable dtScreen = dsScreen.Tables[0];
                DataSet dsRep = Captain.DatabaseLayer.ADMNB002DB.Browse_CAPBATC(TypeModule);
                DataTable dtRep = dsRep.Tables[0];

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

                System.IO.FileStream fs = new FileStream(PdfName, FileMode.Create);

                Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                PdfWriter writer = PdfWriter.GetInstance(document, fs);

                document.Open();
                cb = writer.DirectContent;
                try
                {
                    PrintHeaderPage(document, writer);
                    document.NewPage();

                    BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
                    iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 10);
                    iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 10);
                    iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 9, 3);
                    iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_times, 12, 1);
                    iTextSharp.text.Font TblFontBold1 = new iTextSharp.text.Font(bf_times, 10, 1);
                    iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 8, 2);
                    iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 9, 4);
                    PdfPTable table = new PdfPTable(3);
                    table.TotalWidth = 560f;
                    table.WidthPercentage = 100;
                    table.LockedWidth = true;
                    float[] widths = new float[] { 18f, 20f, 150f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                    table.SetWidths(widths);
                    table.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.HeaderRows = 1;

                    string[] Header = { "Module", "Code", "Description" };
                    for (int i = 0; i < Header.Length; ++i)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(Header[i], TblFontBold));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.FixedHeight = 15f;
                        cell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                        table.AddCell(cell);

                    }

                    if (dtScreen.Rows.Count > 0)
                    {
                        bool First = true;
                        foreach (DataRow item in dtScreen.Rows)
                        {

                            if (PrvModule != item["CFC_MODULE_CODE"].ToString().Trim())
                            {
                                if (!First)
                                {
                                    if (dtRep.Rows.Count > 0)
                                    {
                                        bool RepFirst = true;

                                        foreach (DataRow drRep in dtRep.Rows)
                                        {
                                            if (drRep["CBC_MODULE_CODE"].ToString().Trim() == PrvModule)
                                            {
                                                if (RepFirst)
                                                {
                                                    PdfPCell Rep_Space = new PdfPCell(new Phrase("", TblFontBold1));
                                                    Rep_Space.HorizontalAlignment = Element.ALIGN_LEFT;
                                                    Rep_Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                    table.AddCell(Rep_Space);

                                                    PdfPCell Rep_Space2 = new PdfPCell(new Phrase("", TblFontBold1));
                                                    Rep_Space2.HorizontalAlignment = Element.ALIGN_LEFT;
                                                    Rep_Space2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                    table.AddCell(Rep_Space2);

                                                    PdfPCell Rep_TYpe = new PdfPCell(new Phrase("       Reports", TblFontBold1));
                                                    Rep_TYpe.HorizontalAlignment = Element.ALIGN_LEFT;
                                                    Rep_TYpe.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                    table.AddCell(Rep_TYpe);
                                                    RepFirst = false;
                                                }
                                                PdfPCell Rep_Space3 = new PdfPCell(new Phrase("", TableFont));
                                                Rep_Space3.HorizontalAlignment = Element.ALIGN_LEFT;
                                                Rep_Space3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                table.AddCell(Rep_Space3);

                                                PdfPCell Rep_Space1 = new PdfPCell(new Phrase(drRep["CBC_REPORT_CODE"].ToString().Trim(), TableFont));
                                                Rep_Space1.HorizontalAlignment = Element.ALIGN_LEFT;
                                                Rep_Space1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                table.AddCell(Rep_Space1);

                                                PdfPCell RepDesc = new PdfPCell(new Phrase("            " + drRep["CBC_REPORT_NAME"].ToString().Trim(), TableFont));
                                                RepDesc.HorizontalAlignment = Element.ALIGN_LEFT;
                                                RepDesc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                table.AddCell(RepDesc);
                                            }
                                        }
                                    }
                                    if (table.Rows.Count > 0)
                                    {
                                        document.Add(table);
                                        table.DeleteBodyRows();
                                        document.NewPage();
                                    }


                                }

                                ModuleDesc = null;
                                foreach (DataRow dr in dt.Rows)
                                {
                                    if (dr["APPL_CODE"].ToString() == item["CFC_MODULE_CODE"].ToString().Trim())
                                    {
                                        ModuleDesc = dr["APPL_DESCRIPTION"].ToString(); break;
                                    }
                                }

                                PdfPCell Module = new PdfPCell(new Phrase(item["CFC_MODULE_CODE"].ToString().Trim(), TblFontBold1));
                                Module.HorizontalAlignment = Element.ALIGN_CENTER;
                                Module.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(Module);

                                PdfPCell desc = new PdfPCell(new Phrase("", TblFontBold1));
                                desc.HorizontalAlignment = Element.ALIGN_LEFT;
                                desc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(desc);

                                PdfPCell Module1 = new PdfPCell(new Phrase(ModuleDesc, TblFontBold1));
                                Module1.HorizontalAlignment = Element.ALIGN_LEFT;
                                Module1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(Module1);

                                PdfPCell Module_sapce2 = new PdfPCell(new Phrase("", TableFont));
                                Module_sapce2.HorizontalAlignment = Element.ALIGN_LEFT;
                                Module_sapce2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(Module_sapce2);

                                PdfPCell Module_sapce = new PdfPCell(new Phrase(item["CFC_PROGNO"].ToString().Trim(), TableFont));
                                Module_sapce.HorizontalAlignment = Element.ALIGN_LEFT;
                                Module_sapce.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(Module_sapce);

                                PdfPCell Desc = new PdfPCell(new Phrase("       " + item["CFC_DESCRIPTION"].ToString().Trim(), TableFont));
                                Desc.HorizontalAlignment = Element.ALIGN_LEFT;
                                Desc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(Desc);

                                PrvModule = item["CFC_MODULE_CODE"].ToString().Trim(); First = false;
                            }
                            else
                            {
                                PdfPCell Module_sapce3 = new PdfPCell(new Phrase("", TableFont));
                                Module_sapce3.HorizontalAlignment = Element.ALIGN_LEFT;
                                Module_sapce3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(Module_sapce3);

                                PdfPCell Module_sapce1 = new PdfPCell(new Phrase(item["CFC_PROGNO"].ToString().Trim(), TableFont));
                                Module_sapce1.HorizontalAlignment = Element.ALIGN_LEFT;
                                Module_sapce1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(Module_sapce1);

                                PdfPCell Desc = new PdfPCell(new Phrase("       " + item["CFC_DESCRIPTION"].ToString().Trim(), TableFont));
                                Desc.HorizontalAlignment = Element.ALIGN_LEFT;
                                Desc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(Desc);
                            }
                        }

                        if (dtRep.Rows.Count > 0)
                        {
                            bool RepFirst = true;

                            foreach (DataRow drRep in dtRep.Rows)
                            {
                                if (drRep["CBC_MODULE_CODE"].ToString().Trim() == PrvModule)
                                {
                                    if (RepFirst)
                                    {
                                        PdfPCell Rep_Space = new PdfPCell(new Phrase("", TblFontBold1));
                                        Rep_Space.HorizontalAlignment = Element.ALIGN_LEFT;
                                        Rep_Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(Rep_Space);

                                        PdfPCell Rep_Space2 = new PdfPCell(new Phrase("", TblFontBold1));
                                        Rep_Space2.HorizontalAlignment = Element.ALIGN_LEFT;
                                        Rep_Space2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(Rep_Space2);

                                        PdfPCell Rep_TYpe = new PdfPCell(new Phrase("       Reports", TblFontBold1));
                                        Rep_TYpe.HorizontalAlignment = Element.ALIGN_LEFT;
                                        Rep_TYpe.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(Rep_TYpe);
                                        RepFirst = false;
                                    }

                                    PdfPCell Rep_Space3 = new PdfPCell(new Phrase("", TableFont));
                                    Rep_Space3.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Rep_Space3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(Rep_Space3);

                                    PdfPCell Rep_Space1 = new PdfPCell(new Phrase(drRep["CBC_REPORT_CODE"].ToString().Trim(), TableFont));
                                    Rep_Space1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Rep_Space1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(Rep_Space1);

                                    PdfPCell RepDesc = new PdfPCell(new Phrase("            " + drRep["CBC_REPORT_NAME"].ToString().Trim(), TableFont));
                                    RepDesc.HorizontalAlignment = Element.ALIGN_LEFT;
                                    RepDesc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(RepDesc);
                                }
                            }
                        }
                    }

                    if (table.Rows.Count > 0)
                        document.Add(table);

                }
                catch (Exception ex) { document.Add(new Paragraph("Aborted due to Exception............................................... ")); }

                document.Close();
                fs.Close();
                fs.Dispose();

            }
        }

        private void On_All_List_Pdf(object sender, FormClosedEventArgs e)
        {
            PdfListForm form = sender as PdfListForm;
            if (form.DialogResult == DialogResult.OK)
            {
                X_Pos = 30; Y_Pos = 720;
                string ModuleDesc = null, TypeModule = null, PrvModule = null, PrvScr = null;

                if (((Captain.Common.Utilities.ListItem)CmbApp.SelectedItem).Value.ToString().Trim() != "**")
                    TypeModule = ((Captain.Common.Utilities.ListItem)CmbApp.SelectedItem).Value.ToString().Trim();
                DataSet dsScreen = Captain.DatabaseLayer.ADMNB002DB.Browse_CAPFNCC(TypeModule);
                DataTable dtScreen = dsScreen.Tables[0];
                DataSet dsRep = Captain.DatabaseLayer.ADMNB002DB.Browse_CAPBATC(TypeModule);
                DataTable dtRep = dsRep.Tables[0];

                DataSet ds = Captain.DatabaseLayer.Lookups.GetModules();
                dt = ds.Tables[0];

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

                System.IO.FileStream fs = new FileStream(PdfName, FileMode.Create);

                Document document = new Document(PageSize.A4, 30f, 30f, 30f, 50f);
                PdfWriter writer = PdfWriter.GetInstance(document, fs);

                document.Open();
                cb = writer.DirectContent;
                try
                {
                    PrintHeaderPage(document, writer);
                    document.NewPage();

                    BaseFont bf_Calibri = BaseFont.CreateFont("c:/windows/fonts/calibri.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
                    iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_Calibri, 10);
                    iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_Calibri, 8, iTextSharp.text.Font.NORMAL);
                    iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_Calibri, 8, 3);
                    iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_Calibri, 8, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
                    iTextSharp.text.Font TableFontSubHeader = new iTextSharp.text.Font(bf_Calibri, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                    iTextSharp.text.Font TblFontBold1 = new iTextSharp.text.Font(bf_Calibri, 8, 1);
                    iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_Calibri, 8, 2);
                    iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_Calibri, 9, 4);
                    iTextSharp.text.Font Table_error_Font = new iTextSharp.text.Font(bf_Calibri, 8, iTextSharp.text.Font.NORMAL, BaseColor.RED);


                    PdfPTable table = new PdfPTable(3);
                    table.TotalWidth = 560f;
                    table.WidthPercentage = 100;
                    table.LockedWidth = true;
                    float[] widths = new float[] { 18f, 20f, 150f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                    table.SetWidths(widths);
                    table.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.HeaderRows = 1;

                    string[] Header = { "Module", "Code", "Description" };
                    for (int i = 0; i < Header.Length; ++i)
                    {
                        string textHeader = "";
                        if (i == 2 || i == 1)
                            textHeader = " " + Header[i];
                        else
                            textHeader = Header[i];

                        PdfPCell cell = new PdfPCell(new Phrase(textHeader, TblFontBold));
                        if (i == 2 || i == 1)
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        else
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;

                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell.UseAscender = true;
                        cell.FixedHeight = 15f;
                        cell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));    //Column Header Background
                        cell.BorderColor = BaseColor.WHITE;                                                              //cell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                        table.AddCell(cell);

                    }

                    if (CmbApp.SelectedIndex > 0) {
                        DataRow[] dtRows = dt.Select("APPL_CODE='" + TypeModule + "'");
                        if (dtRows.Length > 0) {
                            dt = new DataTable();
                            dt= dtRows.CopyToDataTable();
                        }
                    }

                    /*********** Only Screens Report :: Kranthi 10/14/2022 *****************************/
                    if (cmbReportfor.SelectedIndex == 0)
                    {
                        if (dtScreen.Rows.Count > 0)
                        {
                            foreach (DataRow drCAPAPPL in dt.Rows)
                            {
                                DataRow[] drScreens = dtScreen.Select("CFC_MODULE_CODE='" + drCAPAPPL["APPL_CODE"].ToString() + "'");
                                if (drScreens.Length > 0)
                                {
                                    document.NewPage();
                                    bool First = true;
                                    foreach (DataRow item in drScreens)
                                    {

                                        if (PrvModule != item["CFC_MODULE_CODE"].ToString().Trim())
                                        {
                                            ModuleDesc = drCAPAPPL["APPL_DESCRIPTION"].ToString();

                                            PdfPCell Module = new PdfPCell(new Phrase(item["CFC_MODULE_CODE"].ToString().Trim(), TableFontSubHeader));
                                            Module.HorizontalAlignment = Element.ALIGN_CENTER;
                                            Module.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#a5c8e1"));
                                            Module.BorderColor = BaseColor.WHITE;
                                            Module.FixedHeight = 15f;
                                            table.AddCell(Module);

                                            PdfPCell desc = new PdfPCell(new Phrase("", TableFontSubHeader));
                                            desc.HorizontalAlignment = Element.ALIGN_LEFT;
                                            desc.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#a5c8e1"));
                                            desc.BorderColor = BaseColor.WHITE;
                                            desc.FixedHeight = 15f;
                                            table.AddCell(desc);

                                            PdfPCell Module1 = new PdfPCell(new Phrase(" " + ModuleDesc + " - " + "Screens", TableFontSubHeader));
                                            Module1.HorizontalAlignment = Element.ALIGN_LEFT;
                                            Module1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#a5c8e1"));
                                            Module1.BorderColor = BaseColor.WHITE;
                                            Module1.FixedHeight = 15f;
                                            table.AddCell(Module1);


                                            PdfPCell Module_sapce2 = new PdfPCell(new Phrase("", TableFont));
                                            Module_sapce2.HorizontalAlignment = Element.ALIGN_LEFT;
                                            Module_sapce2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                            Module_sapce2.BorderColor = BaseColor.WHITE;
                                            Module_sapce2.FixedHeight = 15f;
                                            table.AddCell(Module_sapce2);

                                            PdfPCell Module_sapce = new PdfPCell(new Phrase(" " + item["CFC_PROGNO"].ToString().Trim(), TableFont));
                                            Module_sapce.HorizontalAlignment = Element.ALIGN_LEFT;
                                            Module_sapce.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                                            Module_sapce.BorderColor = BaseColor.WHITE;
                                            Module_sapce.FixedHeight = 15f;
                                            table.AddCell(Module_sapce);

                                            PdfPCell Desc = new PdfPCell(new Phrase(" " + item["CFC_DESCRIPTION"].ToString().Trim(), TableFont));
                                            Desc.HorizontalAlignment = Element.ALIGN_LEFT;
                                            Desc.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                            Desc.BorderColor = BaseColor.WHITE;
                                            Desc.FixedHeight = 15f;
                                            table.AddCell(Desc);

                                            PrvModule = item["CFC_MODULE_CODE"].ToString().Trim(); First = false;
                                        }
                                        else
                                        {
                                            PdfPCell Module_sapce3 = new PdfPCell(new Phrase("", TableFont));
                                            Module_sapce3.HorizontalAlignment = Element.ALIGN_LEFT;
                                            Module_sapce3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                            Module_sapce3.BorderColor = BaseColor.WHITE;
                                            Module_sapce3.FixedHeight = 15f;
                                            table.AddCell(Module_sapce3);

                                            PdfPCell Module_sapce1 = new PdfPCell(new Phrase(" " + item["CFC_PROGNO"].ToString().Trim(), TableFont));
                                            Module_sapce1.HorizontalAlignment = Element.ALIGN_LEFT;
                                            Module_sapce1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                                            Module_sapce1.BorderColor = BaseColor.WHITE;
                                            Module_sapce1.FixedHeight = 15f;
                                            table.AddCell(Module_sapce1);

                                            PdfPCell Desc = new PdfPCell(new Phrase(" " + item["CFC_DESCRIPTION"].ToString().Trim(), TableFont));
                                            Desc.HorizontalAlignment = Element.ALIGN_LEFT;
                                            Desc.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                            Desc.BorderColor = BaseColor.WHITE;
                                            Desc.FixedHeight = 15f;
                                            table.AddCell(Desc);
                                        }
                                    }
                                }
                                else if (drScreens.Length == 0)
                                {
                                    document.NewPage();
                                    PdfPCell Rep_Space = new PdfPCell(new Phrase(drCAPAPPL["APPL_CODE"].ToString().Trim(), TableFontSubHeader));
                                    Rep_Space.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Rep_Space.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#a5c8e1"));
                                    Rep_Space.BorderColor = BaseColor.WHITE;
                                    Rep_Space.FixedHeight = 15f;
                                    table.AddCell(Rep_Space);

                                    PdfPCell Rep_Space2 = new PdfPCell(new Phrase("", TableFontSubHeader));
                                    Rep_Space2.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Rep_Space2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#a5c8e1"));
                                    Rep_Space2.BorderColor = BaseColor.WHITE;
                                    Rep_Space2.FixedHeight = 15f;
                                    table.AddCell(Rep_Space2);


                                    ModuleDesc = drCAPAPPL["APPL_DESCRIPTION"].ToString().Trim();
                                    PdfPCell Rep_TYpe = new PdfPCell(new Phrase(" " + ModuleDesc + " - Screens", TableFontSubHeader));
                                    Rep_TYpe.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Rep_TYpe.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#a5c8e1"));
                                    Rep_TYpe.BorderColor = BaseColor.WHITE;
                                    Rep_TYpe.FixedHeight = 15f;
                                    table.AddCell(Rep_TYpe);



                                    PdfPCell Rep_Space3 = new PdfPCell(new Phrase("Records not found", Table_error_Font));
                                    Rep_Space3.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Rep_Space3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F7F7F7"));
                                    Rep_Space3.BorderColor = BaseColor.WHITE;

                                    Rep_Space3.FixedHeight = 15f;
                                    Rep_Space3.Colspan = 3;
                                    table.AddCell(Rep_Space3);

                                }

                                if (table.Rows.Count > 0)
                                {
                                    document.Add(table);
                                    table.DeleteBodyRows();
                                    document.NewPage();
                                }

                            }
                        }
                    }
                    /********************************************************************************/

                    /*********** Only Reports Report :: Kranthi 10/14/2022 *****************************/
                    if (cmbReportfor.SelectedIndex == 1)
                    {
                        if (dtRep.Rows.Count > 0)
                        {
                            bool RepFirst = true;

                            foreach (DataRow drCAPAPPL in dt.Rows)
                            {

                                DataRow[] drReports = dtRep.Select("CBC_MODULE_CODE='" + drCAPAPPL["APPL_CODE"].ToString() + "'");
                                if (drReports.Length > 0)
                                {
                                    document.NewPage();
                                    foreach (DataRow drRep in drReports)
                                    {
                                        if (drRep["CBC_MODULE_CODE"].ToString().Trim() != PrvModule)
                                        {
                                            RepFirst = true;
                                        }
                                        if (RepFirst)
                                        {
                                            PdfPCell Rep_Space = new PdfPCell(new Phrase(drRep["CBC_MODULE_CODE"].ToString().Trim(), TableFontSubHeader));
                                            Rep_Space.HorizontalAlignment = Element.ALIGN_LEFT;
                                            Rep_Space.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#a5c8e1"));
                                            Rep_Space.BorderColor = BaseColor.WHITE;
                                            Rep_Space.FixedHeight = 15f;
                                            table.AddCell(Rep_Space);

                                            PdfPCell Rep_Space2 = new PdfPCell(new Phrase("", TableFontSubHeader));
                                            Rep_Space2.HorizontalAlignment = Element.ALIGN_LEFT;
                                            Rep_Space2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#a5c8e1"));
                                            Rep_Space2.BorderColor = BaseColor.WHITE;
                                            Rep_Space2.FixedHeight = 15f;
                                            table.AddCell(Rep_Space2);

                                            ModuleDesc = drCAPAPPL["APPL_DESCRIPTION"].ToString().Trim();

                                            PdfPCell Rep_TYpe = new PdfPCell(new Phrase(" " + ModuleDesc + " - Reports", TableFontSubHeader));
                                            Rep_TYpe.HorizontalAlignment = Element.ALIGN_LEFT;
                                            Rep_TYpe.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#a5c8e1"));
                                            Rep_TYpe.BorderColor = BaseColor.WHITE;
                                            Rep_TYpe.FixedHeight = 15f;
                                            table.AddCell(Rep_TYpe);
                                            RepFirst = false;
                                        }

                                        PdfPCell Rep_Space3 = new PdfPCell(new Phrase("", TableFont));
                                        Rep_Space3.HorizontalAlignment = Element.ALIGN_LEFT;
                                        Rep_Space3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                        Rep_Space3.BorderColor = BaseColor.WHITE;
                                        Rep_Space3.FixedHeight = 15f;
                                        table.AddCell(Rep_Space3);

                                        PdfPCell Rep_Space1 = new PdfPCell(new Phrase(drRep["CBC_REPORT_CODE"].ToString().Trim(), TableFont));
                                        Rep_Space1.HorizontalAlignment = Element.ALIGN_LEFT;
                                        Rep_Space1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                                        Rep_Space1.BorderColor = BaseColor.WHITE;
                                        Rep_Space1.FixedHeight = 15f;
                                        table.AddCell(Rep_Space1);

                                        PdfPCell RepDesc = new PdfPCell(new Phrase(" " + drRep["CBC_REPORT_NAME"].ToString().Trim(), TableFont));
                                        RepDesc.HorizontalAlignment = Element.ALIGN_LEFT;
                                        RepDesc.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                        RepDesc.BorderColor = BaseColor.WHITE;
                                        RepDesc.FixedHeight = 15f;
                                        table.AddCell(RepDesc);

                                        PrvModule = drRep["CBC_MODULE_CODE"].ToString().Trim();
                                        // }
                                        //else
                                        //{

                                        //}
                                    }
                                }
                                else if (drReports.Length == 0)
                                {
                                    document.NewPage();

                                    PdfPCell Rep_Space = new PdfPCell(new Phrase(drCAPAPPL["APPL_CODE"].ToString().Trim(), TableFontSubHeader));
                                    Rep_Space.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Rep_Space.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#a5c8e1"));
                                    Rep_Space.BorderColor = BaseColor.WHITE;
                                    Rep_Space.FixedHeight = 15f;
                                    table.AddCell(Rep_Space);

                                    PdfPCell Rep_Space2 = new PdfPCell(new Phrase("", TableFontSubHeader));
                                    Rep_Space2.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Rep_Space2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#a5c8e1"));
                                    Rep_Space2.BorderColor = BaseColor.WHITE;
                                    Rep_Space2.FixedHeight = 15f;
                                    table.AddCell(Rep_Space2);

                                    ModuleDesc = drCAPAPPL["APPL_DESCRIPTION"].ToString().Trim();

                                    PdfPCell Rep_TYpe = new PdfPCell(new Phrase(" " + ModuleDesc + " - Reports", TableFontSubHeader));
                                    Rep_TYpe.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Rep_TYpe.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#a5c8e1"));
                                    Rep_TYpe.BorderColor = BaseColor.WHITE;
                                    Rep_TYpe.FixedHeight = 15f;
                                    table.AddCell(Rep_TYpe);


                                    PdfPCell Rep_Space3 = new PdfPCell(new Phrase("Records not found", Table_error_Font));
                                    Rep_Space3.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Rep_Space3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F7F7F7"));
                                    Rep_Space3.BorderColor = BaseColor.WHITE;

                                    Rep_Space3.FixedHeight = 15f;
                                    Rep_Space3.Colspan = 3;
                                    table.AddCell(Rep_Space3);

                                }

                                if (table.Rows.Count > 0)
                                {
                                    document.Add(table);
                                    table.DeleteBodyRows();
                                    document.NewPage();
                                }
                            }


                        }
                    }
                    /********************************************************************************/


                    /*********** Both Screens and Report :: Kranthi 10/14/2022 *****************************/
                    if (cmbReportfor.SelectedIndex == 2)
                    {

                        if (dtScreen.Rows.Count > 0)
                        {
                            foreach (DataRow drCAPAPPL in dt.Rows)
                            {
                                DataRow[] drScreens = dtScreen.Select("CFC_MODULE_CODE='" + drCAPAPPL["APPL_CODE"].ToString() + "'");
                                DataRow[] drReports = dtRep.Select("CBC_MODULE_CODE='" + drCAPAPPL["APPL_CODE"].ToString() + "'");
                                if (drScreens.Length > 0)
                                {
                                    
                                    bool First = true;
                                    foreach (DataRow item in drScreens)
                                    {

                                        if (PrvModule != item["CFC_MODULE_CODE"].ToString().Trim())
                                        {
                                            if (First)
                                            {
                                                if (drReports.Length > 0)
                                                {
                                                    bool RepFirst = true;

                                                    foreach (DataRow drRep in drReports)
                                                    {
                                                        if (drRep["CBC_MODULE_CODE"].ToString().Trim() == PrvModule)
                                                        {
                                                            if (RepFirst)
                                                            {
                                                                PdfPCell Rep_Space = new PdfPCell(new Phrase("", TableFontSubHeader));
                                                                Rep_Space.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                Rep_Space.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#a5c8e1"));
                                                                Rep_Space.BorderColor = BaseColor.WHITE;
                                                                Rep_Space.FixedHeight = 15f;
                                                                table.AddCell(Rep_Space);

                                                                PdfPCell Rep_Space2 = new PdfPCell(new Phrase("", TableFontSubHeader));
                                                                Rep_Space2.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                Rep_Space2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#a5c8e1"));
                                                                Rep_Space2.BorderColor = BaseColor.WHITE;
                                                                Rep_Space2.FixedHeight = 15f;
                                                                table.AddCell(Rep_Space2);

                                                                PdfPCell Rep_TYpe = new PdfPCell(new Phrase(" Reports", TableFontSubHeader));
                                                                Rep_TYpe.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                Rep_TYpe.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#a5c8e1"));
                                                                Rep_TYpe.BorderColor = BaseColor.WHITE;
                                                                Rep_TYpe.FixedHeight = 15f;
                                                                table.AddCell(Rep_TYpe);
                                                                RepFirst = false;
                                                            }
                                                            PdfPCell Rep_Space3 = new PdfPCell(new Phrase("", TableFont));
                                                            Rep_Space3.HorizontalAlignment = Element.ALIGN_LEFT;
                                                            Rep_Space3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                                            Rep_Space3.BorderColor = BaseColor.WHITE;
                                                            Rep_Space3.FixedHeight = 15f;
                                                            table.AddCell(Rep_Space3);

                                                            PdfPCell Rep_Space1 = new PdfPCell(new Phrase(drRep["CBC_REPORT_CODE"].ToString().Trim(), TableFont));
                                                            Rep_Space1.HorizontalAlignment = Element.ALIGN_LEFT;
                                                            Rep_Space1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                                                            Rep_Space1.BorderColor = BaseColor.WHITE;
                                                            Rep_Space1.FixedHeight = 15f;
                                                            table.AddCell(Rep_Space1);

                                                            PdfPCell RepDesc = new PdfPCell(new Phrase(" " + drRep["CBC_REPORT_NAME"].ToString().Trim(), TableFont));
                                                            RepDesc.HorizontalAlignment = Element.ALIGN_LEFT;
                                                            RepDesc.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                            RepDesc.BorderColor = BaseColor.WHITE;
                                                            RepDesc.FixedHeight = 15f;
                                                            table.AddCell(RepDesc);
                                                        }
                                                    }
                                                }
                                                if (table.Rows.Count > 0)
                                                {
                                                    document.Add(table);
                                                    table.DeleteBodyRows();
                                                    document.NewPage();
                                                }


                                            }

                                            ModuleDesc = drCAPAPPL["APPL_DESCRIPTION"].ToString().Trim();

                                            //Chunk Semibold = new Chunk(item["CFC_MODULE_CODE"].ToString().Trim(), TableFontSubHeader);
                                            //Semibold.SetTextRenderMode(
                                            //    PdfContentByte.TEXT_RENDER_MODE_FILL_STROKE, 0.1f,
                                            //    GrayColor.GRAYBLACK);

                                            PdfPCell Module = new PdfPCell(new Phrase(item["CFC_MODULE_CODE"].ToString().Trim(), TableFontSubHeader));
                                            //PdfPCell Module = new PdfPCell(new Phrase(Semibold));
                                            Module.HorizontalAlignment = Element.ALIGN_CENTER;
                                            Module.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#a5c8e1"));
                                            Module.BorderColor = BaseColor.WHITE;
                                            Module.FixedHeight = 15f;
                                            table.AddCell(Module);

                                            PdfPCell desc = new PdfPCell(new Phrase("", TableFontSubHeader));
                                            desc.HorizontalAlignment = Element.ALIGN_LEFT;
                                            desc.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#a5c8e1"));
                                            desc.BorderColor = BaseColor.WHITE;
                                            desc.FixedHeight = 15f;
                                            table.AddCell(desc);

                                            PdfPCell Module1 = new PdfPCell(new Phrase(" " + ModuleDesc, TableFontSubHeader));
                                            Module1.HorizontalAlignment = Element.ALIGN_LEFT;
                                            Module1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#a5c8e1"));
                                            Module1.BorderColor = BaseColor.WHITE;
                                            Module1.FixedHeight = 15f;
                                            table.AddCell(Module1);


                                            PdfPCell Module_sapce2 = new PdfPCell(new Phrase("", TableFont));
                                            Module_sapce2.HorizontalAlignment = Element.ALIGN_LEFT;
                                            Module_sapce2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                            Module_sapce2.BorderColor = BaseColor.WHITE;
                                            Module_sapce2.FixedHeight = 15f;
                                            table.AddCell(Module_sapce2);

                                            PdfPCell Module_sapce = new PdfPCell(new Phrase(" " + item["CFC_PROGNO"].ToString().Trim(), TableFont));
                                            Module_sapce.HorizontalAlignment = Element.ALIGN_LEFT;
                                            Module_sapce.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                                            Module_sapce.BorderColor = BaseColor.WHITE;
                                            Module_sapce.FixedHeight = 15f;
                                            table.AddCell(Module_sapce);

                                            PdfPCell Desc = new PdfPCell(new Phrase(" " + item["CFC_DESCRIPTION"].ToString().Trim(), TableFont));
                                            Desc.HorizontalAlignment = Element.ALIGN_LEFT;
                                            Desc.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                            Desc.BorderColor = BaseColor.WHITE;
                                            Desc.FixedHeight = 15f;
                                            table.AddCell(Desc);

                                            PrvModule = item["CFC_MODULE_CODE"].ToString().Trim(); First = false;
                                        }
                                        else
                                        {
                                            PdfPCell Module_sapce3 = new PdfPCell(new Phrase("", TableFont));
                                            Module_sapce3.HorizontalAlignment = Element.ALIGN_LEFT;
                                            Module_sapce3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                            Module_sapce3.BorderColor = BaseColor.WHITE;
                                            Module_sapce3.FixedHeight = 15f;
                                            table.AddCell(Module_sapce3);

                                            PdfPCell Module_sapce1 = new PdfPCell(new Phrase(" " + item["CFC_PROGNO"].ToString().Trim(), TableFont));
                                            Module_sapce1.HorizontalAlignment = Element.ALIGN_LEFT;
                                            Module_sapce1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                                            Module_sapce1.BorderColor = BaseColor.WHITE;
                                            Module_sapce1.FixedHeight = 15f;
                                            table.AddCell(Module_sapce1);

                                            PdfPCell Desc = new PdfPCell(new Phrase(" " + item["CFC_DESCRIPTION"].ToString().Trim(), TableFont));
                                            Desc.HorizontalAlignment = Element.ALIGN_LEFT;
                                            Desc.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                            Desc.BorderColor = BaseColor.WHITE;
                                            Desc.FixedHeight = 15f;
                                            table.AddCell(Desc);
                                        }
                                    }

                                    if (drReports.Length > 0)
                                    {
                                        bool RepFirst = true;

                                        foreach (DataRow drRep in drReports)
                                        {
                                            if (drRep["CBC_MODULE_CODE"].ToString().Trim() == PrvModule)
                                            {
                                                if (RepFirst)
                                                {
                                                    PdfPCell Rep_Space = new PdfPCell(new Phrase("", TableFontSubHeader));
                                                    Rep_Space.HorizontalAlignment = Element.ALIGN_LEFT;
                                                    Rep_Space.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#a5c8e1"));
                                                    Rep_Space.BorderColor = BaseColor.WHITE;
                                                    Rep_Space.FixedHeight = 15f;
                                                    table.AddCell(Rep_Space);

                                                    PdfPCell Rep_Space2 = new PdfPCell(new Phrase("", TableFontSubHeader));
                                                    Rep_Space2.HorizontalAlignment = Element.ALIGN_LEFT;
                                                    Rep_Space2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#a5c8e1"));
                                                    Rep_Space2.BorderColor = BaseColor.WHITE;
                                                    Rep_Space2.FixedHeight = 15f;
                                                    table.AddCell(Rep_Space2);

                                                    PdfPCell Rep_TYpe = new PdfPCell(new Phrase(" Reports", TableFontSubHeader));
                                                    Rep_TYpe.HorizontalAlignment = Element.ALIGN_LEFT;
                                                    Rep_TYpe.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#a5c8e1"));
                                                    Rep_TYpe.BorderColor = BaseColor.WHITE;
                                                    Rep_TYpe.FixedHeight = 15f;
                                                    table.AddCell(Rep_TYpe);
                                                    RepFirst = false;
                                                }

                                                PdfPCell Rep_Space3 = new PdfPCell(new Phrase("", TableFont));
                                                Rep_Space3.HorizontalAlignment = Element.ALIGN_LEFT;
                                                Rep_Space3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                                Rep_Space3.BorderColor = BaseColor.WHITE;
                                                Rep_Space3.FixedHeight = 15f;
                                                table.AddCell(Rep_Space3);

                                                PdfPCell Rep_Space1 = new PdfPCell(new Phrase(drRep["CBC_REPORT_CODE"].ToString().Trim(), TableFont));
                                                Rep_Space1.HorizontalAlignment = Element.ALIGN_LEFT;
                                                Rep_Space1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                                                Rep_Space1.BorderColor = BaseColor.WHITE;
                                                Rep_Space1.FixedHeight = 15f;
                                                table.AddCell(Rep_Space1);

                                                PdfPCell RepDesc = new PdfPCell(new Phrase(" " + drRep["CBC_REPORT_NAME"].ToString().Trim(), TableFont));
                                                RepDesc.HorizontalAlignment = Element.ALIGN_LEFT;
                                                RepDesc.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                RepDesc.BorderColor = BaseColor.WHITE;
                                                RepDesc.FixedHeight = 15f;
                                                table.AddCell(RepDesc);
                                            }
                                        }
                                    }
                                }
                                else if (drScreens.Length == 0)
                                {
                                    document.Add(table);
                                    table.DeleteBodyRows();
                                    document.NewPage();

                                    PdfPCell Rep_Space = new PdfPCell(new Phrase(drCAPAPPL["APPL_CODE"].ToString().Trim(), TableFontSubHeader));
                                    Rep_Space.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Rep_Space.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#a5c8e1"));
                                    Rep_Space.BorderColor = BaseColor.WHITE;
                                    Rep_Space.FixedHeight = 15f;
                                    table.AddCell(Rep_Space);

                                    PdfPCell Rep_Space2 = new PdfPCell(new Phrase("", TableFontSubHeader));
                                    Rep_Space2.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Rep_Space2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#a5c8e1"));
                                    Rep_Space2.BorderColor = BaseColor.WHITE;
                                    Rep_Space2.FixedHeight = 15f;
                                    table.AddCell(Rep_Space2);


                                    ModuleDesc = drCAPAPPL["APPL_DESCRIPTION"].ToString().Trim();
                                    PdfPCell Rep_TYpe = new PdfPCell(new Phrase(" " + ModuleDesc + " ", TableFontSubHeader));
                                    Rep_TYpe.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Rep_TYpe.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#a5c8e1"));
                                    Rep_TYpe.BorderColor = BaseColor.WHITE;
                                    Rep_TYpe.FixedHeight = 15f;
                                    table.AddCell(Rep_TYpe);



                                    PdfPCell Rep_Space3 = new PdfPCell(new Phrase("Records not found", Table_error_Font));
                                    Rep_Space3.HorizontalAlignment = Element.ALIGN_CENTER;
                                    Rep_Space3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F7F7F7"));
                                    Rep_Space3.BorderColor = BaseColor.WHITE;

                                    Rep_Space3.FixedHeight = 15f;
                                    Rep_Space3.Colspan = 3;
                                    table.AddCell(Rep_Space3);

                                }
                            }
                        }
                    }
                    /********************************************************************************/

                    if (table.Rows.Count > 0)
                        document.Add(table);


                }
                catch (Exception ex) { document.Add(new Paragraph("Aborted due to Exception............................................... ")); }

                document.Close();
                fs.Close();
                fs.Dispose();
                AlertBox.Show("Report Generated Successfully.");
            }
        }


        private void CheckBottomBorderReached(Document document, PdfWriter writer)
        {

            //Font _font = new Font(bfTimes, 45, 1, BaseColor.BLACK);
            //Image _image = iTextSharp.text.Image.GetInstance(Context.Server.MapPath("~\\Resources\\images\\Capsystems_WaterMark.bmp"));
            //_image.SetAbsolutePosition(160, 310);
            //////_image.SetAbsolutePosition(140, 260);

            //_image.RotationDegrees = 45;
            //_image.Rotate();
            //PdfGState _state = new PdfGState()
            //{
            //    FillOpacity = 0.2F,
            //    StrokeOpacity = 0.2F
            //};

            if (Y_Pos <= 20)
            {

                cb.EndText();
                cb.BeginText();

                Y_Pos = 07;
                X_Pos = 20;
                cb.SetFontAndSize(bfTimes, 10);
                cb.SetCMYKColorFill(0, 0, 0, 255);
                //cb.SetRGBColorFill(0, 0, 0);
                PrintRec(DateTime.Now.ToLocalTime().ToString(), 130);
                Y_Pos = 07;
                X_Pos = 555;
                PrintRec("Page:", 23);
                PrintRec(pageNumber.ToString(), 15);
                cb.EndText();


                //cb = writer.DirectContentUnder;
                //cb.SaveState();
                //cb.SetGState(_state);                               //WaterMark*******
                //cb.AddImage(_image);
                //cb.RestoreState();

                //cb.EndText();

                document.NewPage();
                pageNumber = writer.PageNumber - 1;

                cb.BeginText();
                cb.SetFontAndSize(FontFactory.GetFont(FontFactory.COURIER_OBLIQUE).BaseFont, 10);
                cb.SetColorFill(BaseColor.GRAY);
                cb.ShowTextAligned(100, "User Tree Structure", 260, 810, 0);
                cb.SetCMYKColorFill(0, 0, 0, 255);
                //cb.SetRGBColorFill(0, 0, 0);
                Y_Pos = 793;
                //PrintRec("T" + DateTime.Now.ToLocalTime(), 10);
                //DateTime.Now.ToLocalTime();
                //PrintRec(pageCount.ToString(), 15);
                //PrintRec(pcnt.ToString(), 15);
                // Users = 0;

                cb.SetFontAndSize(bfTimes, 10);

            }
        }



        private void PrintHeader()
        {

            //cb.BeginText();

            //  cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_BOLD).BaseFont, 10);
            cb.SetColorFill(BaseColor.GRAY);
            //PrintText = "User ID";
            //PrintHeaderRec(PrintText, 120);
            //PrintText = "Module";
            //PrintHeaderRec(PrintText, 150 );
            PrintText = "Screen Description";
            PrintHeaderRec(PrintText, 245);
            PrintText = "Hierarchy";
            PrintHeaderRec(PrintText, 80);
            PrintText = "View";
            PrintHeaderRec(PrintText, 40);
            PrintText = "Add";
            PrintHeaderRec(PrintText, 40);
            PrintText = "Change";
            PrintHeaderRec(PrintText, 40);
            PrintText = "Del";
            PrintHeaderRec(PrintText, 40);



            //if (FirstRun)
            //    Y_Pos = 760;
            //else
            // Y_Pos = 770;
            Y_Pos -= 10;
            //cb.EndText();

            //cb.SetLineWidth(0.7f);
            //    cb.MoveTo(45, 775);
            //    cb.LineTo(590, 775);
            //    cb.Stroke();
            //cb.SetRGBColorFill(0, 0, 0);      //ss
            //       cb.SetColorFill(BaseColor.BLACK);
            cb.SetCMYKColorFill(0, 0, 0, 255);
            cb.SetFontAndSize(bfTimes, 10);
            //cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ROMAN).BaseFont, 10);
        }

        private void Meth(int Users)
        {


        }
        private void PrintHeaderRec(string PrintText, int StrWidth)
        {
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, PrintText, X_Pos, Y_Pos, 0);

            //cb.Stroke();
            //cb.FillStroke();

            X_Pos += StrWidth;
        }

        private void PrintRec(string PrintText, int StrWidth)
        {
            cb.ShowTextAligned(800, PrintText, X_Pos, Y_Pos, 0);
            X_Pos += StrWidth;
            PrintText = null;
        }


        private void CmbUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            UserName = null;
            if (((Captain.Common.Utilities.ListItem)CmbUser.SelectedItem).Value.ToString() != "**")
                UserName = ((Captain.Common.Utilities.ListItem)CmbUser.SelectedItem).Value.ToString();
        }

        private void CmbApp_SelectedIndexChanged(object sender, EventArgs e)
        {
            ModuleCode = null;
            if (((Captain.Common.Utilities.ListItem)CmbApp.SelectedItem).Value.ToString() != "**")
                ModuleCode = ((Captain.Common.Utilities.ListItem)CmbApp.SelectedItem).Value.ToString();
        }


        private string AddLocation(int Loc)
        {
            string Temp = null;
            for (int i = 0; i < Loc; i++)
                Temp += " ";
            return Temp;
        }

        private void PrintHeaderPage(Document document, PdfWriter writer)
        {
            BaseFont bf_Calibri = BaseFont.CreateFont("c:/windows/fonts/calibri.ttf", BaseFont.WINANSI, BaseFont.EMBEDDED);
            BaseFont bf_TimesRomanI = BaseFont.CreateFont(BaseFont.TIMES_ITALIC, BaseFont.CP1250, false);
            BaseFont bf_Times = BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1250, false);
            iTextSharp.text.Font fc = new iTextSharp.text.Font(bf_Calibri, 10, 2);
            iTextSharp.text.Font fc1 = new iTextSharp.text.Font(bf_Times, 10, 2, BaseColor.BLUE);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_Calibri, 9);
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_Times, 13);
            iTextSharp.text.Font TblFont = new iTextSharp.text.Font(bf_Times, 11);
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
            float[] Headerwidths = new float[] { 12f, 88f };
            Headertable.SetWidths(Headerwidths);
            Headertable.HorizontalAlignment = Element.ALIGN_CENTER;

            //border trails

            PdfContentByte content = writer.DirectContent;
            iTextSharp.text.Rectangle rectangle = new iTextSharp.text.Rectangle(document.PageSize);
            rectangle.Left += document.LeftMargin;
            rectangle.Right -= document.RightMargin;
            rectangle.Top -= document.TopMargin;
            rectangle.Bottom += document.BottomMargin;
            content.SetColorStroke(BaseColor.DARK_GRAY);
            content.Rectangle(rectangle.Left, rectangle.Bottom, rectangle.Width, rectangle.Height);
            //content.SetLineWidth(6);
            content.Stroke();



            PdfPCell cellspace = new PdfPCell(new Phrase(""));
            cellspace.HorizontalAlignment = Element.ALIGN_CENTER;
            cellspace.Colspan = 2;
            cellspace.PaddingBottom = 10;
            cellspace.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Headertable.AddCell(cellspace);

            if (_strImageFolderPath != "")
            {
                iTextSharp.text.Image imgLogo = iTextSharp.text.Image.GetInstance(_strImageFolderPath);
                PdfPCell cellLogo = new PdfPCell(imgLogo);
                cellLogo.HorizontalAlignment = Element.ALIGN_CENTER;
                cellLogo.Colspan = 2;
                cellLogo.PaddingBottom = 5;
                cellLogo.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Headertable.AddCell(cellLogo);
            }

            PdfPCell cellRptTitle = new PdfPCell(new Phrase(Privileges.Program + " - " + Privileges.PrivilegeName, TblHeaderTitleFont));
            cellRptTitle.HorizontalAlignment = Element.ALIGN_CENTER;
            cellRptTitle.Colspan = 2;
            cellRptTitle.PaddingBottom = 15;
            cellRptTitle.Border = iTextSharp.text.Rectangle.NO_BORDER;
            Headertable.AddCell(cellRptTitle);


            PdfPCell cellRptHeader = new PdfPCell(new Phrase("Report Parameters", TblParamsHeaderFont));
            cellRptHeader.HorizontalAlignment = Element.ALIGN_CENTER;
            cellRptHeader.VerticalAlignment = PdfPCell.ALIGN_TOP;
            cellRptHeader.PaddingBottom = 5;
            cellRptHeader.MinimumHeight = 6;
            cellRptHeader.Colspan = 2;
            cellRptHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
            cellRptHeader.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#b8e9fb"));
            Headertable.AddCell(cellRptHeader);

            //string Agy = "Agency : All"; string Dept = "Dept : All"; string Prg = "Program : All"; string Header_year = string.Empty;
            //if (Agency != "**") Agy = "Agency : " + Agency;
            //if (Depart != "**") Dept = "Dept : " + Depart;
            //if (Program != "**") Prg = "Program : " + Program;
            //if (CmbYear.Visible == true)
            //    Header_year = "Year : " + ((Captain.Common.Utilities.ListItem)CmbYear.SelectedItem).Text.ToString();


            //PdfPCell Hierarchy = new PdfPCell(new Phrase(Agy + "  " + Dept + "  " + Prg + "  " + Header_year, TableFont));
            //Hierarchy.HorizontalAlignment = Element.ALIGN_LEFT;
            //Hierarchy.Colspan = 2;
            //Hierarchy.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //Headertable.AddCell(Hierarchy);

            PdfPCell cell_Content_Title1 = new PdfPCell(new Phrase("  " + LblUser.Text.Trim() + "", TableFont));
            cell_Content_Title1.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_Content_Title1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            cell_Content_Title1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            cell_Content_Title1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            cell_Content_Title1.PaddingBottom = 5;
            Headertable.AddCell(cell_Content_Title1);

            PdfPCell cell_Content_Desc1 = new PdfPCell(new Phrase(((Captain.Common.Utilities.ListItem)CmbUser.SelectedItem).Text.ToString().Trim(), TableFont));
            cell_Content_Desc1.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_Content_Desc1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            cell_Content_Desc1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            cell_Content_Desc1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            cell_Content_Desc1.PaddingBottom = 5;
            Headertable.AddCell(cell_Content_Desc1);


            PdfPCell cell_Content_Title2 = new PdfPCell(new Phrase("  " + "Application", TableFont));
            cell_Content_Title2.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_Content_Title2.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            cell_Content_Title2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            cell_Content_Title2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            cell_Content_Title2.PaddingBottom = 5;
            Headertable.AddCell(cell_Content_Title2);

            PdfPCell cell_Content_Desc2 = new PdfPCell(new Phrase(((Captain.Common.Utilities.ListItem)CmbApp.SelectedItem).Text.ToString().Trim(), TableFont));
            cell_Content_Desc2.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_Content_Desc2.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            cell_Content_Desc2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            cell_Content_Desc2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            cell_Content_Desc2.PaddingBottom = 5;
            Headertable.AddCell(cell_Content_Desc2);

            PdfPCell cell_Content_Title4 = new PdfPCell(new Phrase("  " + "Report Type", TableFont));
            cell_Content_Title4.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_Content_Title4.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            cell_Content_Title4.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            cell_Content_Title4.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            cell_Content_Title4.PaddingBottom = 5;
            Headertable.AddCell(cell_Content_Title4);


            string strReportType = "";
            if (rdoUserstructure.Checked)
                strReportType = "User Tree Structure";
            if (rdoUserAccess.Checked)
                strReportType = "User Access";

            PdfPCell cell_Content_Desc4 = new PdfPCell(new Phrase(strReportType, TableFont));
            cell_Content_Desc4.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_Content_Desc4.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            cell_Content_Desc4.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            cell_Content_Desc4.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            cell_Content_Desc4.PaddingBottom = 5;
            Headertable.AddCell(cell_Content_Desc4);


            PdfPCell cell_Content_Title3 = new PdfPCell(new Phrase("  " + "Report for", TableFont));
            cell_Content_Title3.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_Content_Title3.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            cell_Content_Title3.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            cell_Content_Title3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            cell_Content_Title3.PaddingBottom = 5;
            Headertable.AddCell(cell_Content_Title3);


            strReportType = "";
            if (cmbReportfor.SelectedIndex == 0)
                strReportType = "Screens Only";
            if (cmbReportfor.SelectedIndex == 1)
                strReportType = "Reports Only";
            if (cmbReportfor.SelectedIndex == 2)
                strReportType = "Both Screens and Reports";


            PdfPCell cell_Content_Desc3 = new PdfPCell(new Phrase(strReportType, TableFont));
            cell_Content_Desc3.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_Content_Desc3.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            cell_Content_Desc3.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            cell_Content_Desc3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            cell_Content_Desc3.PaddingBottom = 5;
            Headertable.AddCell(cell_Content_Desc3);



            document.Add(Headertable);

            //PdfPTable Footertable = new PdfPTable(2);
            //Footertable.TotalWidth = 550f;
            //Footertable.WidthPercentage = 100;
            //Footertable.LockedWidth = true;
            //float[] Footerwidths = new float[] { 80f, 25f };
            //Footertable.SetWidths(Footerwidths);
            //Footertable.HorizontalAlignment = Element.ALIGN_CENTER;

            //PdfPCell cellTitle1 = new PdfPCell(new Phrase("Generated By : ", TableFont));
            //cellTitle1.HorizontalAlignment = Element.ALIGN_RIGHT;
            //cellTitle1.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //cellTitle1.PaddingTop = 12;
            //Footertable.AddCell(cellTitle1);

            //PdfPCell cellDesc1 = new PdfPCell(new Phrase(LookupDataAccess.GetMemberName(BaseForm.UserProfile.FirstName.Trim(), BaseForm.UserProfile.MI.Trim(), BaseForm.UserProfile.LastName.Trim(), "3"), fnttimesRoman_Italic));
            //cellDesc1.HorizontalAlignment = Element.ALIGN_LEFT;
            //cellDesc1.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //cellDesc1.PaddingTop = 12;
            //Footertable.AddCell(cellDesc1);

            //PdfPCell cellTitle2 = new PdfPCell(new Phrase("Generated On : ", TableFont));
            //cellTitle2.HorizontalAlignment = Element.ALIGN_RIGHT;
            //cellTitle2.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //Footertable.AddCell(cellTitle2);

            //PdfPCell cellDesc2 = new PdfPCell(new Phrase(DateTime.Now.ToString(), fnttimesRoman_Italic));
            //cellDesc2.HorizontalAlignment = Element.ALIGN_LEFT;
            //cellDesc2.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //Footertable.AddCell(cellDesc2);

            //document.Add(Footertable);

            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase("Generated By : ", fnttimesRoman_Italic), 33, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.GetMemberName(BaseForm.UserProfile.FirstName.Trim(), BaseForm.UserProfile.MI.Trim(), BaseForm.UserProfile.LastName.Trim(), "3"), fnttimesRoman_Italic), 90, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase("Generated On : ", fnttimesRoman_Italic), 410, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase(DateTime.Now.ToString(), fnttimesRoman_Italic), 468, 40, 0);

        }
        private void rdoUserAccess_Click(object sender, EventArgs e)
        {
            if (rdoUserstructure.Checked)
            {

                LblApp.Text = "Application";
                Pb_Search_Hie.Visible = false;
                txtHierchy.Visible = false;
                CmbApp.Visible = true;
                lblHierequired.Visible = false;
                btnGenPdf.Text = "&Generate";
                CommonFunctions.SetComboBoxValue(CmbUser, "00");

            }
            else
            {

               // txtHierchy.Text = Agency + " - " + Dept + " - " + Prog;
                LblApp.Text = "Hierarchy";
                Pb_Search_Hie.Visible = true;
                txtHierchy.Visible = true;
                lblHierequired.Visible = true;
                CmbApp.Visible = false;
                btnGenPdf.Text = "&Generate";
                CommonFunctions.SetComboBoxValue(CmbUser, "**");

                //Current_Hierarchy_DB = Agency + "-" + Dept + "-" + Prog;
            }
        }

        private void Pb_Search_Hie_Click(object sender, EventArgs e)
        {
            //HierarchieSelectionFormNew hierarchieSelectionForm = new HierarchieSelectionFormNew(BaseForm, "", "Master", "A", "*", "Reports");
            HierarchieSelection hierarchieSelectionForm = new HierarchieSelection(BaseForm, strDefaultHIE, "Master", string.Empty, "*", "", BaseForm.UserID);
            hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
            hierarchieSelectionForm.StartPosition = FormStartPosition.CenterScreen;
            hierarchieSelectionForm.ShowDialog();
        }

        string strDefaultHIE=string.Empty;
        string Current_Hierarchy = "******", Current_Hierarchy_DB = "**-**-**";
        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Prog { get; set; }

        private void ADMNB002_ToolClick(object sender, ToolClickEventArgs e)
        {
            Application.Navigate(CommonFunctions.CreateZenHelps(Privileges.Program, 0, BaseForm.BusinessModuleID.ToString(), "", ""), target: "_blank");
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


                    Agency = hierarchy.Substring(0, 2);
                    Dept = hierarchy.Substring(2, 2);
                    Prog = hierarchy.Substring(4, 2);
                    txtHierchy.Text = hierarchy.Substring(0, 2) + "-" + hierarchy.Substring(2, 2) + "-" + hierarchy.Substring(4, 2);
                    strDefaultHIE= hierarchy.Substring(0, 2) + "-" + hierarchy.Substring(2, 2) + "-" + hierarchy.Substring(4, 2);
                    //Current_Hierarchy_DB = hierarchy.Substring(0, 2) + "-" + hierarchy.Substring(2, 2) + "-" + hierarchy.Substring(4, 2);
                }
            }
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

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "User Tree Structure");
        }

        //private void OnsavePdf(object sender, FormClosedEventArgs e)
        //{
        //    PdfListForm form = sender as PdfListForm;
        //    if (form.DialogResult == DialogResult.OK)
        //    {
        //        X_Pos = 30; Y_Pos = 720;
        //        string ModuleDesc = null, PrvUser = null, PrvModule = null, PrvScr = null;
        //        CaptainModel model = new CaptainModel();
        //        List<EMPLFUNCEntity> Cust = model.ADMNB002Data.ADMNB002_GetScrPrivbyUserId(UserName, ModuleCode);
        //        List<BATCNTLEntity> BatcntlList = model.ADMNB002Data.ADMNB002_GetRepPrivbyUserId(UserName, ModuleCode);


        //            string PdfName = "Pdf File";
        //            PdfName = form.GetFileName();


        //            PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
        //            try
        //            {
        //                if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
        //                { DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim()); }
        //            }
        //            catch (Exception ex)
        //            {
        //                CommonFunctions.MessageBoxDisplay("Error");
        //            }

        //            try
        //            {
        //                string Tmpstr = PdfName + ".pdf";
        //                if (File.Exists(Tmpstr))
        //                    File.Delete(Tmpstr);
        //            }
        //            catch (Exception ex)
        //            {
        //                int length = 8;
        //                string newFileName = System.Guid.NewGuid().ToString();
        //                newFileName = newFileName.Replace("-", string.Empty);

        //                Random_Filename = PdfName + newFileName.Substring(0, length) + ".pdf";
        //            }


        //            if (!string.IsNullOrEmpty(Random_Filename))
        //                PdfName = Random_Filename;
        //            else
        //                PdfName += ".pdf";

        //            System.IO.FileStream fs = new FileStream(PdfName, FileMode.Create);

        //            Document document = new Document(PageSize.A4, 25, 25, 30, 30);
        //            PdfWriter writer = PdfWriter.GetInstance(document, fs);

        //            document.Open();

        //            //Font fc = new Font(bfTimes);
        //            Image logo = Image.GetInstance(Context.Server.MapPath("~\\Resources\\images\\Captain_Wheel.bmp"));
        //            logo.BackgroundColor = BaseColor.WHITE;
        //            logo.ScalePercent(30f);

        //            Image logo1 = Image.GetInstance(Context.Server.MapPath("~\\Resources\\images\\CapSystems_Title.bmp"));
        //            logo1.ScalePercent(50F);
        //            logo1.SetAbsolutePosition(60, 783);
        //            document.Add(logo);
        //            document.Add(logo1);
        //            cb = writer.DirectContent;
        //            try
        //            {
        //                PrintHeaderPage();

        //                    document.NewPage();
        //                    Y_Pos = 795;
        //                    Users = 0;
        //                    X_Pos = 80;
        //                    cb.BeginText();

        //                    cb.SetFontAndSize(FontFactory.GetFont(FontFactory.COURIER_BOLDOBLIQUE).BaseFont, 10);
        //                    cb.SetColorFill(BaseColor.GRAY);
        //                    cb.ShowTextAligned(100, "User Tree Structure", 260, 820, 0);
        //                    cb.SetCMYKColorFill(0, 0, 0, 255);

        //                    cb.EndText();

        //                    BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
        //                    iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 10);
        //                    iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 8);
        //                    iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 9, 3);
        //                    iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(1, 9, 1);
        //                    iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 8, 2);
        //                    iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 9, 4);
        //                    PdfPTable table = new PdfPTable(11);
        //                    table.TotalWidth = 560f;
        //                    table.WidthPercentage = 100;
        //                    table.LockedWidth = true;
        //                    float[] widths = new float[] { 10f,  30f, 30f, 30f, 22f, 13f, 13f, 13f, 10f, 25f, 20f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
        //                    table.SetWidths(widths);
        //                    table.HorizontalAlignment = Element.ALIGN_CENTER;
        //                    if (Cust.Count > 0)
        //                    {
        //                        string[] Header = { "App",  "Description", "Function", "Batch Job", "User", "Privileges", "Name", "CC" };
        //                        for (int i = 0; i < Header.Length; ++i)
        //                        {
        //                            if (i == 5)
        //                            {
        //                                PdfPCell cell = new PdfPCell(new Phrase(Header[i], TblFontBold));
        //                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
        //                                cell.Colspan = 4;
        //                                cell.FixedHeight = 15f;
        //                                cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                table.AddCell(cell);
        //                            }
        //                            else
        //                            {
        //                                PdfPCell cell = new PdfPCell(new Phrase(Header[i], TblFontBold));
        //                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
        //                                cell.FixedHeight = 15f;
        //                                cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                table.AddCell(cell);
        //                            }
        //                        }

        //                        PdfPCell Space = new PdfPCell(new Phrase("", TblFontBold));
        //                        Space.HorizontalAlignment = Element.ALIGN_CENTER;
        //                        Space.Colspan = 5;
        //                        Space.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
        //                        table.AddCell(Space);

        //                        PdfPCell Addpriv = new PdfPCell(new Phrase("View", TblFontBold));
        //                        Addpriv.HorizontalAlignment = Element.ALIGN_CENTER;
        //                        Addpriv.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
        //                        table.AddCell(Addpriv);

        //                        PdfPCell Changepriv = new PdfPCell(new Phrase("Change", TblFontBold));
        //                        Changepriv.HorizontalAlignment = Element.ALIGN_CENTER;
        //                        Changepriv.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
        //                        table.AddCell(Changepriv);

        //                        PdfPCell Delpriv = new PdfPCell(new Phrase("Del", TblFontBold));
        //                        Delpriv.HorizontalAlignment = Element.ALIGN_CENTER;
        //                        Delpriv.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
        //                        table.AddCell(Delpriv);

        //                        PdfPCell viewpriv = new PdfPCell(new Phrase("Add", TblFontBold));
        //                        viewpriv.HorizontalAlignment = Element.ALIGN_CENTER;
        //                        viewpriv.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
        //                        table.AddCell(viewpriv);

        //                        PdfPCell Space1 = new PdfPCell(new Phrase("", TblFontBold));
        //                        Space1.HorizontalAlignment = Element.ALIGN_CENTER;
        //                        Space1.Colspan = 2;
        //                        Space1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
        //                        table.AddCell(Space1);

        //                        bool Report = false; string ScreenName = string.Empty;
        //                        string PrivUser = string.Empty, PrivModule = string.Empty; string MOdule_Desc = string.Empty;
        //                        foreach (EMPLFUNCEntity Entity in Cust)
        //                        {
        //                            if (Entity.UserName.Trim() != PrivUser && Entity.Module.Trim() != PrivModule)
        //                            {
        //                                if (Report == true)
        //                                {
        //                                    if (BatcntlList.Count > 0)
        //                                    {
        //                                        List<BATCNTLEntity> RepList=BatcntlList.FindAll(u=>u.UserName.Trim().Equals(PrivUser.Trim()) && u.ModuleCode.Trim().Equals(PrivModule.Trim()));
        //                                        if (RepList.Count > 0)
        //                                        {
        //                                            PdfPCell Space_Rep = new PdfPCell(new Phrase(" ", TableFont));
        //                                            Space_Rep.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                            Space_Rep.Colspan = 2;
        //                                            Space_Rep.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                            table.AddCell(Space_Rep);

        //                                            PdfPCell Space_Rep1 = new PdfPCell(new Phrase("Batch Reports", TableFont));
        //                                            Space_Rep1.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                            Space_Rep1.Colspan = 9;
        //                                            Space_Rep1.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                            table.AddCell(Space_Rep1);
        //                                            foreach (BATCNTLEntity item in RepList)
        //                                            {
        //                                                PdfPCell RepSpace = new PdfPCell(new Phrase(" ", TableFont));
        //                                                RepSpace.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                                RepSpace.Colspan = 3;
        //                                                RepSpace.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                                table.AddCell(RepSpace);

        //                                                PdfPCell RepName = new PdfPCell(new Phrase(item.RepName.Trim(), TableFont));
        //                                                RepName.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                                RepName.Colspan = 8;
        //                                                RepName.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                                table.AddCell(RepName);
        //                                            }
        //                                        }
        //                                    }

        //                                    PdfPCell RepSpace1 = new PdfPCell(new Phrase(" ", TableFont));
        //                                    RepSpace1.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                    RepSpace1.Colspan = 11;
        //                                    RepSpace1.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                    table.AddCell(RepSpace1);

        //                                    PdfPCell RepSpace2 = new PdfPCell(new Phrase(" ", TableFont));
        //                                    RepSpace2.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                    RepSpace2.Colspan = 11;
        //                                    RepSpace2.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
        //                                    table.AddCell(RepSpace2);
        //                                }

        //                                PdfPCell App = new PdfPCell(new Phrase(Entity.Module.Trim(), TableFont));
        //                                App.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                App.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                table.AddCell(App);

        //                                ModuleDesc = null;
        //                                foreach (DataRow dr in dt.Rows)
        //                                {
        //                                    if (dr["APPL_CODE"].ToString() == Entity.Module)
        //                                    {
        //                                        ModuleDesc = dr["APPL_DESCRIPTION"].ToString(); break;
        //                                    }
        //                                }

        //                                PdfPCell Desc = new PdfPCell(new Phrase(ModuleDesc, TableFont));
        //                                Desc.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                Desc.Colspan = 3;
        //                                Desc.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                table.AddCell(Desc);

        //                                PdfPCell User = new PdfPCell(new Phrase(Entity.UserName.Trim(), TableFont));
        //                                User.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                User.Colspan = 5;
        //                                User.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                table.AddCell(User);

        //                                PdfPCell Name = new PdfPCell(new Phrase(Entity.Name.Trim(), TableFont));
        //                                Name.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                Name.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                table.AddCell(Name);

        //                                PdfPCell CaseWorker = new PdfPCell(new Phrase(Entity.CaseWoeker.Trim(), TableFont));
        //                                CaseWorker.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                CaseWorker.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                table.AddCell(CaseWorker);

        //                                DataSet dsModuleHie = Captain.DatabaseLayer.ADMNB002DB.ADMNB002_GetModuleHierarchies(Entity.UserName.Trim(), Entity.Module.Trim());
        //                                DataTable dtModuleHie = dsModuleHie.Tables[0]; string Hierarchy = string.Empty;
        //                                foreach (DataRow drModuleHie in dtModuleHie.Rows)
        //                                {
        //                                    Hierarchy += drModuleHie["Hierarchy"].ToString().Trim() + "  ";
        //                                }
        //                                PdfPCell Hier = new PdfPCell(new Phrase("Hierarchy: " + Hierarchy, TableFont));
        //                                Hier.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                Hier.Colspan = 11;
        //                                Hier.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                table.AddCell(Hier);

        //                                PdfPCell ScrSpace = new PdfPCell(new Phrase(" ", TableFont));
        //                                ScrSpace.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                ScrSpace.Colspan = 2;
        //                                ScrSpace.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                table.AddCell(ScrSpace);

        //                                PdfPCell ScrName = new PdfPCell(new Phrase(Entity.ScrDesc.Trim(), TableFont));
        //                                ScrName.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                ScrName.Colspan = 3;
        //                                ScrName.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                table.AddCell(ScrName);

        //                                PdfPCell ScrAdd = new PdfPCell(new Phrase(Entity.Add_Priv.Trim(), TableFont));
        //                                ScrAdd.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                ScrAdd.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                table.AddCell(ScrAdd);

        //                                PdfPCell ScrEdit = new PdfPCell(new Phrase(Entity.Chg_Priv.Trim(), TableFont));
        //                                ScrEdit.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                ScrEdit.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                table.AddCell(ScrEdit);

        //                                PdfPCell ScrDel = new PdfPCell(new Phrase(Entity.Del_Priv.Trim(), TableFont));
        //                                ScrDel.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                ScrDel.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                table.AddCell(ScrDel);

        //                                PdfPCell ScrView = new PdfPCell(new Phrase(Entity.Inq_Priv.Trim(), TableFont));
        //                                ScrView.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                ScrView.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                table.AddCell(ScrView);

        //                                PdfPCell ScrSpace1 = new PdfPCell(new Phrase(" ", TableFont));
        //                                ScrSpace1.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                ScrSpace1.Colspan = 2;
        //                                ScrSpace1.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                table.AddCell(ScrSpace1);
        //                                PrivUser = Entity.UserName.Trim(); PrivModule = Entity.Module.Trim(); Report = true; ScreenName = Entity.ScrDesc.Trim();
        //                            }
        //                            else if(PrivUser==Entity.UserName.Trim() && Entity.Module.Trim()!=PrivModule)
        //                            {
        //                                if (Report == true)
        //                                {
        //                                    if (BatcntlList.Count > 0)
        //                                    {
        //                                        List<BATCNTLEntity> RepList = BatcntlList.FindAll(u => u.UserName.Trim().Equals(PrivUser.Trim()) && u.ModuleCode.Trim().Equals(PrivModule.Trim()));
        //                                        if (RepList.Count > 0)
        //                                        {
        //                                            PdfPCell Space_Rep = new PdfPCell(new Phrase(" ", TableFont));
        //                                            Space_Rep.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                            Space_Rep.Colspan = 2;
        //                                            Space_Rep.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                            table.AddCell(Space_Rep);

        //                                            PdfPCell Space_Rep1 = new PdfPCell(new Phrase("Batch Reports", TableFont));
        //                                            Space_Rep1.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                            Space_Rep1.Colspan = 9;
        //                                            Space_Rep1.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                            table.AddCell(Space_Rep1);
        //                                            foreach (BATCNTLEntity item in RepList)
        //                                            {
        //                                                PdfPCell RepSpace = new PdfPCell(new Phrase(" ", TableFont));
        //                                                RepSpace.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                                RepSpace.Colspan = 3;
        //                                                RepSpace.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                                table.AddCell(RepSpace);

        //                                                PdfPCell RepName = new PdfPCell(new Phrase(item.RepName.Trim(), TableFont));
        //                                                RepName.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                                RepName.Colspan = 8;
        //                                                RepName.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                                table.AddCell(RepName);
        //                                            }
        //                                        }
        //                                    }

        //                                    PdfPCell RepSpace1 = new PdfPCell(new Phrase(" ", TableFont));
        //                                    RepSpace1.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                    RepSpace1.Colspan = 11;
        //                                    RepSpace1.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                    table.AddCell(RepSpace1);

        //                                    PdfPCell RepSpace2 = new PdfPCell(new Phrase(" ", TableFont));
        //                                    RepSpace2.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                    RepSpace2.Colspan = 11;
        //                                    RepSpace2.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
        //                                    table.AddCell(RepSpace2);
        //                                }

        //                                PdfPCell App = new PdfPCell(new Phrase(Entity.Module.Trim(), TableFont));
        //                                App.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                App.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                table.AddCell(App);

        //                                ModuleDesc = null;
        //                                foreach (DataRow dr in dt.Rows)
        //                                {
        //                                    if (dr["APPL_CODE"].ToString() == Entity.Module)
        //                                    {
        //                                        ModuleDesc = dr["APPL_DESCRIPTION"].ToString(); break;
        //                                    }
        //                                }

        //                                PdfPCell Desc = new PdfPCell(new Phrase(ModuleDesc, TableFont));
        //                                Desc.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                Desc.Colspan = 3;
        //                                Desc.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                table.AddCell(Desc);

        //                                PdfPCell User = new PdfPCell(new Phrase(Entity.UserName.Trim(), TableFont));
        //                                User.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                User.Colspan = 5;
        //                                User.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                table.AddCell(User);

        //                                PdfPCell Name = new PdfPCell(new Phrase(Entity.Name.Trim(), TableFont));
        //                                Name.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                Name.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                table.AddCell(Name);

        //                                PdfPCell CaseWorker = new PdfPCell(new Phrase(Entity.CaseWoeker.Trim(), TableFont));
        //                                CaseWorker.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                CaseWorker.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                table.AddCell(CaseWorker);

        //                                DataSet dsModuleHie = Captain.DatabaseLayer.ADMNB002DB.ADMNB002_GetModuleHierarchies(Entity.UserName.Trim(), Entity.Module.Trim());
        //                                DataTable dtModuleHie = dsModuleHie.Tables[0]; string Hierarchy = string.Empty;
        //                                foreach (DataRow drModuleHie in dtModuleHie.Rows)
        //                                {
        //                                    Hierarchy += drModuleHie["Hierarchy"].ToString().Trim() + "  ";
        //                                }
        //                                PdfPCell Hier = new PdfPCell(new Phrase("Hierarchy: " + Hierarchy, TableFont));
        //                                Hier.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                Hier.Colspan = 11;
        //                                Hier.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                table.AddCell(Hier);

        //                                PdfPCell ScrSpace = new PdfPCell(new Phrase(" ", TableFont));
        //                                ScrSpace.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                ScrSpace.Colspan = 2;
        //                                ScrSpace.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                table.AddCell(ScrSpace);

        //                                PdfPCell ScrName = new PdfPCell(new Phrase(Entity.ScrDesc.Trim(), TableFont));
        //                                ScrName.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                ScrName.Colspan = 3;
        //                                ScrName.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                table.AddCell(ScrName);

        //                                PdfPCell ScrAdd = new PdfPCell(new Phrase(Entity.Add_Priv.Trim(), TableFont));
        //                                ScrAdd.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                ScrAdd.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                table.AddCell(ScrAdd);

        //                                PdfPCell ScrEdit = new PdfPCell(new Phrase(Entity.Chg_Priv.Trim(), TableFont));
        //                                ScrEdit.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                ScrEdit.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                table.AddCell(ScrEdit);

        //                                PdfPCell ScrDel = new PdfPCell(new Phrase(Entity.Del_Priv.Trim(), TableFont));
        //                                ScrDel.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                ScrDel.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                table.AddCell(ScrDel);

        //                                PdfPCell ScrView = new PdfPCell(new Phrase(Entity.Inq_Priv.Trim(), TableFont));
        //                                ScrView.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                ScrView.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                table.AddCell(ScrView);

        //                                PdfPCell ScrSpace1 = new PdfPCell(new Phrase(" ", TableFont));
        //                                ScrSpace1.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                ScrSpace1.Colspan = 2;
        //                                ScrSpace1.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                table.AddCell(ScrSpace1);
        //                                PrivUser = Entity.UserName.Trim(); PrivModule = Entity.Module.Trim(); Report = true;

        //                            }
        //                            else if (PrivUser != Entity.UserName.Trim() && PrivModule == Entity.Module.Trim())
        //                            {
        //                                if (Report == true)
        //                                {
        //                                    if (BatcntlList.Count > 0)
        //                                    {
        //                                        List<BATCNTLEntity> RepList = BatcntlList.FindAll(u => u.UserName.Trim().Equals(PrivUser.Trim()) && u.ModuleCode.Trim().Equals(PrivModule.Trim()));
        //                                        if (RepList.Count > 0)
        //                                        {
        //                                            PdfPCell Space_Rep = new PdfPCell(new Phrase(" ", TableFont));
        //                                            Space_Rep.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                            Space_Rep.Colspan = 2;
        //                                            Space_Rep.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                            table.AddCell(Space_Rep);

        //                                            PdfPCell Space_Rep1 = new PdfPCell(new Phrase("Batch Reports", TableFont));
        //                                            Space_Rep1.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                            Space_Rep1.Colspan = 9;
        //                                            Space_Rep1.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                            table.AddCell(Space_Rep1);
        //                                            foreach (BATCNTLEntity item in RepList)
        //                                            {
        //                                                PdfPCell RepSpace = new PdfPCell(new Phrase(" ", TableFont));
        //                                                RepSpace.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                                RepSpace.Colspan = 3;
        //                                                RepSpace.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                                table.AddCell(RepSpace);

        //                                                PdfPCell RepName = new PdfPCell(new Phrase(item.RepName.Trim(), TableFont));
        //                                                RepName.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                                RepName.Colspan = 8;
        //                                                RepName.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                                table.AddCell(RepName);
        //                                            }
        //                                        }
        //                                    }

        //                                    PdfPCell RepSpace1 = new PdfPCell(new Phrase(" ", TableFont));
        //                                    RepSpace1.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                    RepSpace1.Colspan = 11;
        //                                    RepSpace1.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                    table.AddCell(RepSpace1);

        //                                    PdfPCell RepSpace2 = new PdfPCell(new Phrase(" ", TableFont));
        //                                    RepSpace2.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                    RepSpace2.Colspan = 11;
        //                                    RepSpace2.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
        //                                    table.AddCell(RepSpace2);
        //                                }

        //                                PdfPCell App = new PdfPCell(new Phrase(Entity.Module.Trim(), TableFont));
        //                                App.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                App.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                table.AddCell(App);

        //                                ModuleDesc = null;
        //                                foreach (DataRow dr in dt.Rows)
        //                                {
        //                                    if (dr["APPL_CODE"].ToString() == Entity.Module)
        //                                    {
        //                                        ModuleDesc = dr["APPL_DESCRIPTION"].ToString(); break;
        //                                    }
        //                                }

        //                                PdfPCell Desc = new PdfPCell(new Phrase(ModuleDesc, TableFont));
        //                                Desc.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                Desc.Colspan = 3;
        //                                Desc.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                table.AddCell(Desc);

        //                                PdfPCell User = new PdfPCell(new Phrase(Entity.UserName.Trim(), TableFont));
        //                                User.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                User.Colspan = 5;
        //                                User.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                table.AddCell(User);

        //                                PdfPCell Name = new PdfPCell(new Phrase(Entity.Name.Trim(), TableFont));
        //                                Name.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                Name.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                table.AddCell(Name);

        //                                PdfPCell CaseWorker = new PdfPCell(new Phrase(Entity.CaseWoeker.Trim(), TableFont));
        //                                CaseWorker.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                CaseWorker.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                table.AddCell(CaseWorker);

        //                                DataSet dsModuleHie = Captain.DatabaseLayer.ADMNB002DB.ADMNB002_GetModuleHierarchies(Entity.UserName.Trim(), Entity.Module.Trim());
        //                                DataTable dtModuleHie = dsModuleHie.Tables[0]; string Hierarchy = string.Empty;
        //                                foreach (DataRow drModuleHie in dtModuleHie.Rows)
        //                                {
        //                                    Hierarchy += drModuleHie["Hierarchy"].ToString().Trim() + "  ";
        //                                }
        //                                PdfPCell Hier = new PdfPCell(new Phrase("Hierarchy: " + Hierarchy, TableFont));
        //                                Hier.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                Hier.Colspan = 11;
        //                                Hier.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                table.AddCell(Hier);

        //                                PdfPCell ScrSpace = new PdfPCell(new Phrase(" ", TableFont));
        //                                ScrSpace.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                ScrSpace.Colspan = 2;
        //                                ScrSpace.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                table.AddCell(ScrSpace);

        //                                PdfPCell ScrName = new PdfPCell(new Phrase(Entity.ScrDesc.Trim(), TableFont));
        //                                ScrName.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                ScrName.Colspan = 3;
        //                                ScrName.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                table.AddCell(ScrName);

        //                                PdfPCell ScrAdd = new PdfPCell(new Phrase(Entity.Add_Priv.Trim(), TableFont));
        //                                ScrAdd.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                ScrAdd.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                table.AddCell(ScrAdd);

        //                                PdfPCell ScrEdit = new PdfPCell(new Phrase(Entity.Chg_Priv.Trim(), TableFont));
        //                                ScrEdit.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                ScrEdit.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                table.AddCell(ScrEdit);

        //                                PdfPCell ScrDel = new PdfPCell(new Phrase(Entity.Del_Priv.Trim(), TableFont));
        //                                ScrDel.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                ScrDel.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                table.AddCell(ScrDel);

        //                                PdfPCell ScrView = new PdfPCell(new Phrase(Entity.Inq_Priv.Trim(), TableFont));
        //                                ScrView.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                ScrView.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                table.AddCell(ScrView);

        //                                PdfPCell ScrSpace1 = new PdfPCell(new Phrase(" ", TableFont));
        //                                ScrSpace1.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                ScrSpace1.Colspan = 2;
        //                                ScrSpace1.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                table.AddCell(ScrSpace1);
        //                                PrivUser = Entity.UserName.Trim(); PrivModule = Entity.Module.Trim(); Report = true;
        //                            }
        //                            else
        //                            {
        //                                if (Entity.ScrDesc.Trim() == ScreenName)
        //                                {
        //                                    PdfPCell ScrSpace = new PdfPCell(new Phrase(" ", TableFont));
        //                                    ScrSpace.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                    ScrSpace.Colspan = 2;
        //                                    ScrSpace.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                    table.AddCell(ScrSpace);

        //                                    PdfPCell ScrName = new PdfPCell(new Phrase(Entity.ScrDesc.Trim(), TableFont));
        //                                    ScrName.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                    ScrName.Colspan = 3;
        //                                    ScrName.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                    table.AddCell(ScrName);

        //                                    PdfPCell ScrAdd = new PdfPCell(new Phrase(Entity.Add_Priv.Trim(), TableFont));
        //                                    ScrAdd.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                    ScrAdd.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                    table.AddCell(ScrAdd);

        //                                    PdfPCell ScrEdit = new PdfPCell(new Phrase(Entity.Chg_Priv.Trim(), TableFont));
        //                                    ScrEdit.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                    ScrEdit.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                    table.AddCell(ScrEdit);

        //                                    PdfPCell ScrDel = new PdfPCell(new Phrase(Entity.Del_Priv.Trim(), TableFont));
        //                                    ScrDel.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                    ScrDel.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                    table.AddCell(ScrDel);

        //                                    PdfPCell ScrView = new PdfPCell(new Phrase(Entity.Inq_Priv.Trim(), TableFont));
        //                                    ScrView.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                    ScrView.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                    table.AddCell(ScrView);

        //                                    PdfPCell ScrSpace1 = new PdfPCell(new Phrase(" ", TableFont));
        //                                    ScrSpace1.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                    ScrSpace1.Colspan = 2;
        //                                    ScrSpace1.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                    table.AddCell(ScrSpace1);
        //                                }
        //                            }

        //                        }

        //                        if (BatcntlList.Count > 0)
        //                        {
        //                            List<BATCNTLEntity> RepList = BatcntlList.FindAll(u => u.UserName.Trim().Equals(PrivUser.Trim()) && u.ModuleCode.Trim().Equals(PrivModule.Trim()));
        //                            if (RepList.Count > 0)
        //                            {
        //                                PdfPCell ScrSpace = new PdfPCell(new Phrase(" ", TableFont));
        //                                ScrSpace.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                ScrSpace.Colspan = 2;
        //                                ScrSpace.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                table.AddCell(ScrSpace);

        //                                PdfPCell ScrName = new PdfPCell(new Phrase("Batch Reports", TableFont));
        //                                ScrName.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                ScrName.Colspan = 9;
        //                                ScrName.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                table.AddCell(ScrName);
        //                                foreach (BATCNTLEntity item in RepList)
        //                                {
        //                                    PdfPCell RepSpace = new PdfPCell(new Phrase(" ", TableFont));
        //                                    RepSpace.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                    RepSpace.Colspan = 3;
        //                                    RepSpace.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                    table.AddCell(RepSpace);

        //                                    PdfPCell RepName = new PdfPCell(new Phrase(item.RepName.Trim(), TableFont));
        //                                    RepName.HorizontalAlignment = Element.ALIGN_LEFT;
        //                                    RepName.Colspan = 8;
        //                                    RepName.Border = iTextSharp.text.Rectangle.NO_BORDER;
        //                                    table.AddCell(RepName);
        //                                }
        //                            }
        //                        }

        //                    }
        //                    if (table.Rows.Count > 0)
        //                        document.Add(table);
        //            }
        //            catch (Exception ex) { document.Add(new Paragraph("Aborted due to Exception............................................... ")); }

        //            document.Close();
        //            fs.Close();
        //            fs.Dispose();
        //    }

        //}


    }
}
class comboboxlist
{
    public string id { set; get; }
    public string Description { set; get; }
}