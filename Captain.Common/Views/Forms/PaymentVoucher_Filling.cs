/************************************************************************
 * Conversion On            :   11/25/2022
 * Converted By             :   Kranthi
 * Latest Modification On   :   11/25/2022
 * **********************************************************************/
#region Using
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
using iTextSharp.text.pdf;
using System.IO;
using iTextSharp.text;
using Wisej.Web;
using Captain.Common.Views.Controls.Compatibility;
#endregion


namespace Captain.Common.Views.Forms
{
    public partial class PaymentVoucher_Filling : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        //private GridControl _intakeHierarchy = null;
        private bool boolChangeStatus = false;
        private CaptainModel _model = null;
        private List<HierarchyEntity> _selectedHierarchies = null;

        #endregion

        public PaymentVoucher_Filling(BaseForm baseform, DataGridView Cams, PrivilegeEntity privileges, string SPCode, string SeqSpm)
        {
            InitializeComponent();
            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;


            BaseForm = baseform;
            Privileges = privileges;
            CAMSGrid = Cams;
            ServicePlan_Code = SPCode;
            SPM_Seq = SeqSpm;

            propReportPath = _model.lookupDataAccess.GetReportPath();
            txtManual.Validator = TextBoxValidation.IntegerValidator;
            
            GetCaseACtEntity();
            FillCAMSGrid();
            //GetAgencyDetails();
        }

        #region properties

        public BaseForm BaseForm { get; set; }

        public DataGridView CAMSGrid { get; set; }

        public string CAMS_FLG { get; set; }

        public DataSet dsAgency { get; set; }

        public DataTable dtAgency { get; set; }

        public string propReportPath { get; set; }

        public string CAMS_Desc { get; set; }

        public string ServicePlan_Code { get; set; }

        public string SPM_Seq { get; set; }

        public string MST_Site { get; set; }

        public string Mode { get; set; }

        public string SPM_Site { get; set; }

        public string MST_Intakeworker { get; set; }

        public string Sp_Start_Date { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public CASESP0Entity SP_Header_Rec { get; set; }

        #endregion

        private void FillCAMSGrid()
        {
            //List<CASEACTEntity> CaseactList=_model.

            SP_CAMS_Grid1.Rows.Clear();
            if (CAMSGrid.Rows.Count > 0)
            {
                int rowIndex = 0;
                foreach (DataGridViewRow dr in CAMSGrid.Rows)
                {
                    if (dr.Cells["SP2_Type"].Value.ToString().Trim() == "CA" && (!string.IsNullOrEmpty(dr.Cells["SP2_Comp_Date"].Value.ToString().Trim())))
                    {
                        string VoucherNo = string.Empty; string VendorNum = string.Empty;
                        if (SP_Activity_Details.Count > 0)
                        {
                            CASEACTEntity SelCaseact=SP_Activity_Details.Find(u=>u.ACT_ID.Equals(dr.Cells["SP2_CAMS_ID"].Value.ToString().Trim()));
                            if (SelCaseact != null)
                            {
                                if (!string.IsNullOrEmpty(SelCaseact.VOUCHNO.Trim()))
                                {
                                    if (Convert.ToInt32(SelCaseact.VOUCHNO.Trim()) > 0)
                                        VoucherNo = SelCaseact.VOUCHNO.Trim();
                                }
                                VendorNum = SelCaseact.Vendor_No.Trim();
                            }
                        }

                        rowIndex = SP_CAMS_Grid1.Rows.Add(false, dr.Cells["SP2_Desc"].Value.ToString().Trim(), dr.Cells["SP2_Comp_Date"].Value.ToString().Trim(), dr.Cells["SP2_Follow_Date"].Value.ToString().Trim(), dr.Cells["SP2_Type"].Value.ToString().Trim(), dr.Cells["SP2_CAMS_Code"].Value.ToString().Trim(), dr.Cells["SP2_Operation"].Value.ToString().Trim(), dr.Cells["SP2_CAMS_Branch"].Value.ToString().Trim(),
                            dr.Cells["SP2_Group"].Value.ToString().Trim(), dr.Cells["SP2_Notes_Sw"].Value.ToString().Trim(), dr.Cells["SP2_Notes_Key"].Value.ToString().Trim(), dr.Cells["SP2_CA_Seq"].Value.ToString().Trim(), dr.Cells["SP2_Dup_Desc"].Value.ToString().Trim(), dr.Cells["SP2_Year"].Value.ToString().Trim(), dr.Cells["SP2_CAMS_Active_Stat"].Value.ToString().Trim(), dr.Cells["SP2_CAMS_Id"].Value.ToString().Trim(),
                            dr.Cells["SP2_Curr_Grp"].Value.ToString().Trim(), dr.Cells["SP2_CAMS_Branch"].Value.ToString().Trim(), dr.Cells["SP2_Template"].Value.ToString().Trim(),VendorNum, VoucherNo);

                        rowIndex++;
                    }
                }
            }
        }

        private void GetCaseACtEntity()
        {
            CASEACTEntity Search_Enty = new CASEACTEntity(true);
            Search_Enty.Agency = BaseForm.BaseAgency;
            Search_Enty.Dept = BaseForm.BaseDept;
            Search_Enty.Program = BaseForm.BaseProg;
            Search_Enty.Year = BaseForm.BaseYear;                             // Year will be always Four-Spaces in CASEACT
            Search_Enty.App_no = BaseForm.BaseApplicationNo;
            Search_Enty.SPM_Seq = SPM_Seq; // Added By Yeswanth on 11/22/2013
            Search_Enty.Service_plan = ServicePlan_Code;

            SP_Activity_Details = _model.SPAdminData.Browse_CASEACT(Search_Enty, "Browse");
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        List<CASEACTEntity> SP_Activity_Details = new List<CASEACTEntity>();
        List<CASEACTEntity> Vendor_Activity_Details = new List<CASEACTEntity>();
        string Vendor = string.Empty;
        List<CAVoucherEntity> VoucherList = new List<CAVoucherEntity>();
        List<VoucherEntity> paymentVoucherList = new List<VoucherEntity>();
        List<CAVACNTFORMATEntity> CAVACNTFORMATList = new List<CAVACNTFORMATEntity>();
        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (SP_CAMS_Grid1.Rows.Count > 0)
            {
                if (Isvalidate())
                {
                    //CASEACTEntity Search_Enty = new CASEACTEntity(true);
                    //Search_Enty.Agency = BaseForm.BaseAgency;
                    //Search_Enty.Dept = BaseForm.BaseDept;
                    //Search_Enty.Program = BaseForm.BaseProg;
                    //Search_Enty.Year = BaseForm.BaseYear;                             // Year will be always Four-Spaces in CASEACT
                    //Search_Enty.App_no = BaseForm.BaseApplicationNo;
                    //Search_Enty.SPM_Seq = SPM_Seq; // Added By Yeswanth on 11/22/2013
                    //Search_Enty.Service_plan = ServicePlan_Code;

                    //SP_Activity_Details = new List<CASEACTEntity>();
                    //SP_Activity_Details = _model.SPAdminData.Browse_CASEACT(Search_Enty, "Browse");
                    CASEACTEntity VendAct = new CASEACTEntity();

                    //CAVoucherEntity Search_Entity = new CAVoucherEntity(true);

                    //VoucherList = _model.SPAdminData.Browse_CAVoucher(Search_Entity, "Browse");
                    
                    paymentVoucherList = _model.SPAdminData.GetPaymentVouchers(BaseForm.BaseAgency,string.Empty,string.Empty);

                    CAVACNTFORMATEntity Search_Entity = new CAVACNTFORMATEntity(true);
                    Search_Entity.CAVACCF_AGENCY = BaseForm.BaseAgency;
                    CAVACNTFORMATList = _model.SPAdminData.Browse_CAVACNTFORMAT(Search_Entity, "Browse");
                    CAVACNTFORMATList = CAVACNTFORMATList.OrderBy(u => u.CAVACCF_SEQ).ToList();

                    string FundV = string.Empty,ProgV=string.Empty,CAV=string.Empty;
                    if (SP_Activity_Details.Count > 0)
                    {
                        foreach (DataGridViewRow dr in SP_CAMS_Grid1.Rows)
                        {
                            //FundV = string.Empty; ProgV = string.Empty; CAV = string.Empty;
                            if (dr.Cells["Chk"].Value.ToString() == true.ToString())
                            {
                                VendAct = SP_Activity_Details.Find(u => u.Service_plan.Trim().Equals(ServicePlan_Code) && u.ACT_Code.Trim().Equals(dr.Cells["SP2_CAMS_Code"].Value.ToString().Trim()) && u.Group.Trim().Equals(dr.Cells["SP2_Group"].Value.ToString().Trim()) && u.Branch.Trim().Equals(dr.Cells["SP2_CAMS_Branch"].Value.ToString().Trim()) && u.ACT_Seq.Trim().Equals(dr.Cells["SP2_CA_Seq"].Value.ToString().Trim()));
                                if (VendAct != null)
                                {
                                    Vendor += VendAct.Vendor_No + VendAct.VOUCHNO + ",";
                                    //CAVoucherEntity FundVouch=VoucherList.Find(u=>u.Code.Equals(VendAct.Fund1.Trim()));
                                    //CAVoucherEntity ProgramVouch=VoucherList.Find(u=>u.Code.Equals(VendAct.Act_PROG.Trim()));
                                    //CAVoucherEntity CACode = VoucherList.Find(u => u.Code.Equals(VendAct.ACT_Code.Trim()));

                                    //if (FundVouch != null) FundV = FundVouch.VCode.Trim() + "-";
                                    //if (ProgramVouch != null) ProgV = ProgramVouch.VCode.Trim() + "-";
                                    //if (CACode != null) CAV = CACode.VCode.Trim();

                                    //if (string.IsNullOrEmpty(ProgV.Trim()) && string.IsNullOrEmpty(CAV.Trim())) FundV = FundV.Replace("-", "");
                                    //if (string.IsNullOrEmpty(CAV.Trim())) ProgV = ProgV.Replace("-", "");

                                    //VendAct.VOUCHNO = FundV + ProgV + CAV;

                                    //Vendor += VendAct.Vendor_No + VendAct.VOUCHNO + ",";
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(Vendor))
                        {
                            string VendTest = Vendor;
                            VendTest = VendTest.Replace(",", "");
                            if (!string.IsNullOrEmpty(VendTest.Trim()))
                                On_PaymentVoucher();
                            else
                            {
                                MessageBox.Show("Vendor doesn't Exist");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Vendor doesn't Exist");
                        }
                    }
                    else
                        MessageBox.Show("Select atleast one CA", "CAP Systems");
                }
                
            }
        }

        private bool Isvalidate()
        {
            bool isValid = false;
            DataSet AgencyData = Captain.DatabaseLayer.ZipCodePlusAgency.GetAgencyControlDetails(BaseForm.BaseAgency);
            int nxt_vouch_no = 0;
            if (AgencyData != null && AgencyData.Tables[0].Rows.Count > 0)
            {
                if(!string.IsNullOrEmpty(AgencyData.Tables[0].Rows[0]["ACR_NXT_VOUCHNO"].ToString().Trim()))
                    nxt_vouch_no = int.Parse(AgencyData.Tables[0].Rows[0]["ACR_NXT_VOUCHNO"].ToString());
            }
            if (SP_CAMS_Grid1.Rows.Count > 0)
            {
                foreach (DataGridViewRow dr in SP_CAMS_Grid1.Rows)
                {
                    if (dr.Cells["Chk"].Value.ToString() == true.ToString())
                    {
                        isValid = true; break;
                    }
                }
            }
            

            if (txtManual.Visible == true)
            {
                if (string.IsNullOrEmpty(txtManual.Text.Trim()))
                {
                    _errorProvider.SetError(txtManual, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Voucher Number"));
                    isValid = false;
                }
                else
                {
                    if (int.Parse(txtManual.Text.ToString().Trim()) > nxt_vouch_no)
                    {
                        _errorProvider.SetError(txtManual, string.Format("Not greater than this Number" + nxt_vouch_no.ToString()));
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(txtManual, null);
                        isValid = true;
                    }
                }
            }

            return isValid;
        }

        List<CASEVDDEntity> CaseVddlist = new List<CASEVDDEntity>();
        private void Get_Vendor_List()
        {
            CASEVDDEntity Search_Entity = new CASEVDDEntity(true);
            CaseVddlist = _model.SPAdminData.Browse_CASEVDD(Search_Entity, "Browse");
        }

        #region Payment Voucher

        PdfContentByte cb;
        int X_Pos, Y_Pos;
        string strFolderPath = string.Empty;
        string Random_Filename = null; string PdfName = "Pdf File";
        private void On_PaymentVoucher()
        {
            Random_Filename = null;

            PdfName = "PAYMENT VOUCHER";//form.GetFileName();
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

            //Document document = new Document();
            Document document = new Document(PageSize.LETTER, 10, 10, 20, 20);
            document.SetPageSize(iTextSharp.text.PageSize.LETTER.Rotate());
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();
            BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/Calibri.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            //BaseFont bf_timesBold = BaseFont.CreateFont("c:/windows/fonts/TIMESBD.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            iTextSharp.text.Font helvetica = new iTextSharp.text.Font(bf_times, 12, 1);
            BaseFont bf_helv = helvetica.GetCalculatedBaseFont(false);
            iTextSharp.text.Font TimesUnderline = new iTextSharp.text.Font(bf_times, 10, 4);
            BaseFont bf_TimesUnderline = TimesUnderline.GetCalculatedBaseFont(true);

            iTextSharp.text.Font CalibriFont = new iTextSharp.text.Font(bf_times, 11);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 8);
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_times, 11, 1);
            iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 8, 3);
            iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 11, 2);
            iTextSharp.text.Font TblFont10 = new iTextSharp.text.Font(bf_times, 10, 1);
            iTextSharp.text.Font TblBFont8 = new iTextSharp.text.Font(bf_times, 8, 1);

            //iTextSharp.text.Font TblFontSmall = new iTextSharp.text.Font(bf_times, 9);

            //iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 11, 4);
            //iTextSharp.text.Font TimesBoldline = new iTextSharp.text.Font(bf_times, 11, 5);
            //iTextSharp.text.Font TimesBoldlineHead = new iTextSharp.text.Font(bf_times, 13, 5);
            //iTextSharp.text.Font TimesBold = new iTextSharp.text.Font(bf_times, 15, 1);
            cb = writer.DirectContent;

            //List<HierarchyEntity> caseHierarchy = _model.lookupDataAccess.GetCaseHierarchy("AGENCY", string.Empty, string.Empty, string.Empty);
            Get_Vendor_List();
            GetAgencyDetails();

            try
            {
                
                PdfPTable FTable = new PdfPTable(3);
                FTable.TotalWidth = 750f;
                FTable.WidthPercentage = 100;
                FTable.LockedWidth = true;
                float[] Headertablewidths = new float[] { 40f, 60f, 70f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                FTable.SetWidths(Headertablewidths);
                FTable.HorizontalAlignment = Element.ALIGN_CENTER;

                PdfPTable STable = new PdfPTable(6);
                STable.TotalWidth = 750f;
                STable.WidthPercentage = 100;
                STable.LockedWidth = true;
                float[] STablewidths = new float[] { 40f, 90f, 20f, 30f, 20f, 25f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                STable.SetWidths(STablewidths);
                STable.HorizontalAlignment = Element.ALIGN_CENTER;
                STable.SpacingBefore = 5f;

                string[] Vendors = Vendor.Split(',');
                Array.Sort(Vendors);

                

                string[] VendorBase = Vendors.Distinct().ToArray();//Vendor.Split(',');
                //string[] VendorBase = Vendors.ToArray();
                if (VendorBase.Length > 0)
                {
                    
                    string Agcycntl_SW = "N"; string PrivVendor = string.Empty;
                    for (int i = 0; i < VendorBase.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(VendorBase[i].Trim()))
                        {
                            StringBuilder str = new StringBuilder();
                            str.Append("<CASEACT>"); Agcycntl_SW = "N";

                            Vendor_Activity_Details = SP_Activity_Details.FindAll(u => u.Service_plan.Trim().Equals(ServicePlan_Code) && u.Vendor_No.Trim().Equals(VendorBase[i].Substring(0, 10).Trim()));
                            string Voucher = string.Empty;
                            if (VendorBase[i].Length > 10)
                            {
                                Voucher = VendorBase[i].Substring(10, (VendorBase[i].Length - 10)).ToString();
                                Vendor_Activity_Details = Vendor_Activity_Details.FindAll(u => u.VOUCHNO.Trim().Equals(Voucher.Trim()));
                            }
                            else
                            {
                                Vendor_Activity_Details = Vendor_Activity_Details.FindAll(u => u.VOUCHNO.Trim().Equals(string.Empty));
                            }
                            if (Vendor_Activity_Details.Count > 0)
                            {
                                string VendName = string.Empty; string VendAddress = string.Empty; string Vendcity = string.Empty;// string VendPhone = string.Empty;
                                MaskedTextBox mskvendphn = new MaskedTextBox();
                                mskvendphn.Mask = "(000)000-0000";
                                if (!string.IsNullOrEmpty(Vendor_Activity_Details[0].Vendor_No.Trim()))
                                {
                                    CASEVDDEntity selvend = CaseVddlist.Find(u => u.Code.Trim().Equals(Vendor_Activity_Details[0].Vendor_No.Trim()));
                                    if (selvend != null)
                                    {
                                        VendName = selvend.Name.Trim();
                                        VendAddress = selvend.Addr1.Trim();
                                        Vendcity = selvend.City.Trim() + ", " + selvend.State.Trim() + "  " + selvend.Zip.Trim();
                                        if (!string.IsNullOrEmpty(selvend.Phone.Trim()))
                                            mskvendphn.Text = selvend.Phone.Trim();
                                    }

                                }

                                #region For Office Use Only
                                /* --------------------For Office Use Only----------------------*/
                                PdfPTable F1Table = new PdfPTable(3);
                                F1Table.WidthPercentage = 100;
                                float[] F1widths = new float[] { 40f, 30f, 40f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                                F1Table.SetWidths(F1widths);
                                F1Table.HorizontalAlignment = Element.ALIGN_LEFT;

                                PdfPCell A1 = new PdfPCell(new Phrase("FOR " + dtAgency.Rows[0]["ACR_SHORT_NAME"].ToString() + " Office Use Only", CalibriFont));
                                A1.Colspan = 3;
                                A1.HorizontalAlignment = Element.ALIGN_LEFT;
                                A1.Border = iTextSharp.text.Rectangle.BOX;
                                F1Table.AddCell(A1);

                                PdfPCell A2 = new PdfPCell(new Phrase("NSC Initials", TableFont));
                                A2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                A2.Border = iTextSharp.text.Rectangle.BOX;
                                F1Table.AddCell(A2);

                                PdfPCell A3 = new PdfPCell(new Phrase("CO Initials", TableFont));
                                A3.HorizontalAlignment = Element.ALIGN_LEFT;
                                A3.Border = iTextSharp.text.Rectangle.BOX;
                                F1Table.AddCell(A3);

                                PdfPCell A4 = new PdfPCell(new Phrase("Accounting Initials", TableFont));
                                A4.HorizontalAlignment = Element.ALIGN_CENTER;
                                A4.Border = iTextSharp.text.Rectangle.BOX;
                                F1Table.AddCell(A4);

                                PdfPCell A5 = new PdfPCell(new Phrase("Entered on MR", TableFont));
                                A5.HorizontalAlignment = Element.ALIGN_LEFT;
                                A5.Border = iTextSharp.text.Rectangle.BOX;
                                F1Table.AddCell(A5);

                                PdfPCell A6 = new PdfPCell(new Phrase("Date Rc'd CO", TableFont));
                                A6.HorizontalAlignment = Element.ALIGN_LEFT;
                                A6.Border = iTextSharp.text.Rectangle.BOX;
                                F1Table.AddCell(A6);

                                PdfPCell A7 = new PdfPCell(new Phrase("Voucher #", TableFont));
                                A7.HorizontalAlignment = Element.ALIGN_LEFT;
                                A7.Border = iTextSharp.text.Rectangle.BOX;
                                F1Table.AddCell(A7);

                                PdfPCell A8 = new PdfPCell(new Phrase("", TableFont));
                                A8.HorizontalAlignment = Element.ALIGN_LEFT;
                                A8.Border = iTextSharp.text.Rectangle.BOX;
                                F1Table.AddCell(A8);

                                PdfPCell A9 = new PdfPCell(new Phrase("Verified", TableFont));
                                A9.HorizontalAlignment = Element.ALIGN_LEFT;
                                A9.Border = iTextSharp.text.Rectangle.BOX;
                                F1Table.AddCell(A9);

                                PdfPCell A10 = new PdfPCell(new Phrase("Vendor #", TableFont));
                                A10.HorizontalAlignment = Element.ALIGN_LEFT;
                                A10.Border = iTextSharp.text.Rectangle.BOX;
                                F1Table.AddCell(A10);

                                PdfPCell A11 = new PdfPCell(new Phrase("", TableFont));
                                A11.HorizontalAlignment = Element.ALIGN_LEFT;
                                A11.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                F1Table.AddCell(A11);

                                PdfPCell A12 = new PdfPCell(new Phrase("DP in CO", TableFont));
                                A12.HorizontalAlignment = Element.ALIGN_LEFT;
                                A12.Border = iTextSharp.text.Rectangle.BOX;
                                F1Table.AddCell(A12);

                                PdfPCell A13 = new PdfPCell(new Phrase("Check & Date", TableFont));
                                A13.HorizontalAlignment = Element.ALIGN_LEFT;
                                A13.Border = iTextSharp.text.Rectangle.BOX;
                                F1Table.AddCell(A13);

                                PdfPCell A14 = new PdfPCell(new Phrase("", TableFont));
                                A14.HorizontalAlignment = Element.ALIGN_LEFT;
                                A14.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                F1Table.AddCell(A14);

                                PdfPCell A15 = new PdfPCell(new Phrase("DC on MR", TableFont));
                                A15.HorizontalAlignment = Element.ALIGN_LEFT;
                                A15.Border = iTextSharp.text.Rectangle.BOX;
                                F1Table.AddCell(A15);

                                PdfPCell A16 = new PdfPCell(new Phrase("Entered by", TableFont));
                                A16.HorizontalAlignment = Element.ALIGN_LEFT;
                                A16.Border = iTextSharp.text.Rectangle.BOX;
                                F1Table.AddCell(A16);

                                #endregion


                                #region Address
                                PdfPTable F2Table = new PdfPTable(1);
                                F2Table.WidthPercentage = 100;
                                float[] F2widths = new float[] { 70f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                                F2Table.SetWidths(F2widths);
                                F2Table.HorizontalAlignment = Element.ALIGN_LEFT;

                                if (!string.IsNullOrEmpty(Vendor_Activity_Details[0].VOUCHNO.Trim()))
                                {
                                    if (Convert.ToInt32(Vendor_Activity_Details[0].VOUCHNO.Trim()) > 0)
                                    {

                                        PdfPCell B1 = new PdfPCell(new Phrase("PAYMENT VOUCHER # - " + Vendor_Activity_Details[0].VOUCHNO.Trim(), CalibriFont));
                                        B1.HorizontalAlignment = Element.ALIGN_CENTER;
                                        B1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        F2Table.AddCell(B1);
                                    }
                                    else if (txtManual.Visible == true)
                                    {
                                        PdfPCell B1 = new PdfPCell(new Phrase("PAYMENT VOUCHER # - " + txtManual.Text.Trim(), CalibriFont));
                                        B1.HorizontalAlignment = Element.ALIGN_CENTER;
                                        B1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        F2Table.AddCell(B1);
                                        Agcycntl_SW = "Y";
                                    }
                                    else
                                    {
                                        PdfPCell B1 = new PdfPCell(new Phrase("PAYMENT VOUCHER # - " + dtAgency.Rows[0]["ACR_NXT_VOUCHNO"].ToString(), CalibriFont));
                                        B1.HorizontalAlignment = Element.ALIGN_CENTER;
                                        B1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        F2Table.AddCell(B1);
                                        Agcycntl_SW = "Y";
                                    }
                                        
                                }
                                else if (txtManual.Visible == true)
                                {
                                    PdfPCell B1 = new PdfPCell(new Phrase("PAYMENT VOUCHER # - " + txtManual.Text.Trim(), CalibriFont));
                                    B1.HorizontalAlignment = Element.ALIGN_CENTER;
                                    B1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    F2Table.AddCell(B1);
                                    Agcycntl_SW = "Y";
                                }
                                else
                                {
                                    PdfPCell B1 = new PdfPCell(new Phrase("PAYMENT VOUCHER # - " + dtAgency.Rows[0]["ACR_NXT_VOUCHNO"].ToString(), CalibriFont));
                                    B1.HorizontalAlignment = Element.ALIGN_CENTER;
                                    B1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    F2Table.AddCell(B1);
                                    Agcycntl_SW = "Y";
                                }

                                string Address = string.Empty, City = string.Empty, telphn = string.Empty, Fax = string.Empty;
                                string strAgencyName = string.Empty;
                                MaskedTextBox mskPhn = new MaskedTextBox();
                                MaskedTextBox mskFax = new MaskedTextBox();

                                strAgencyName = _model.lookupDataAccess.GetHierachyDescription("1", BaseForm.BaseAgency, string.Empty, string.Empty);
                                Address = dtAgency.Rows[0]["ACR_STREET"].ToString().Trim();
                                City = dtAgency.Rows[0]["ACR_CITY"].ToString().Trim() + ", " + dtAgency.Rows[0]["ACR_STATE"].ToString() + "  " + dtAgency.Rows[0]["ACR_ZIP1"].ToString();
                                telphn = dtAgency.Rows[0]["ACR_MAIN_PHONE"].ToString().Trim();

                                if (!string.IsNullOrEmpty(telphn.Trim()))
                                {
                                    mskPhn.Mask = "(999) 000-0000";
                                    mskPhn.Text = telphn.Trim();
                                }
                                Fax = dtAgency.Rows[0]["ACR_FAX_NUMBER"].ToString().Trim();

                                if (!string.IsNullOrEmpty(Fax.Trim()))
                                {
                                    mskFax.Mask = "(999) 000-0000";
                                    mskFax.Text = Fax.Trim();
                                }

                                PdfPCell B2 = new PdfPCell(new Phrase(strAgencyName, CalibriFont));
                                B2.HorizontalAlignment = Element.ALIGN_LEFT;
                                B2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                F2Table.AddCell(B2);

                                PdfPCell B3 = new PdfPCell(new Phrase(Address + ", " + City, CalibriFont));
                                B3.HorizontalAlignment = Element.ALIGN_LEFT;
                                B3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                F2Table.AddCell(B3);

                                PdfPCell B4 = new PdfPCell(new Phrase(mskPhn.Text, CalibriFont));
                                B4.HorizontalAlignment = Element.ALIGN_LEFT;
                                B4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                F2Table.AddCell(B4);

                                PdfPCell B5 = new PdfPCell(new Phrase("Tax Exemption # " + dtAgency.Rows[0]["ACR_CA_TAX_EXMNO"].ToString().Trim(), CalibriFont));
                                B5.HorizontalAlignment = Element.ALIGN_CENTER;
                                B5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                F2Table.AddCell(B5);

                                #endregion

                                #region Particular column image

                                PdfPTable ImgTable = new PdfPTable(1);
                                ImgTable.WidthPercentage = 100;
                                float[] ImgTablewidths = new float[] { 20f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                                ImgTable.SetWidths(ImgTablewidths);
                                ImgTable.HorizontalAlignment = Element.ALIGN_CENTER;

                                iTextSharp.text.Image _image = iTextSharp.text.Image.GetInstance(Consts.Icons.ico_lg_Righ_Arrow);
                                //iTextSharp.text.Image _image1 = iTextSharp.text.Image.GetInstance(Context.Server.MapPath("~\\Resources\\images\\arrow-left-icon.png"));
                                //_image.SetAbsolutePosition(160, 310);

                                PdfPCell Space1 = new PdfPCell(new Phrase("", TableFont));
                                Space1.HorizontalAlignment = Element.ALIGN_CENTER;
                                //HF1.FixedHeight = 50f;
                                Space1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                ImgTable.AddCell(Space1);

                                PdfPCell I1 = new PdfPCell(_image);
                                I1.HorizontalAlignment = Element.ALIGN_CENTER;
                                I1.FixedHeight = 30f;
                                I1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                ImgTable.AddCell(I1);

                                PdfPCell I2 = new PdfPCell(new Phrase("Deliver Articles to or Render Services For", TableFont));
                                I2.HorizontalAlignment = Element.ALIGN_CENTER;
                                I2.Rowspan = 5;
                                I2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                ImgTable.AddCell(I2);

                                PdfPCell Space5 = new PdfPCell(new Phrase("", TableFont));
                                Space5.HorizontalAlignment = Element.ALIGN_CENTER;
                                //HF1.FixedHeight = 50f;
                                Space5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                ImgTable.AddCell(Space5);

                                PdfPCell I3 = new PdfPCell(_image);
                                I3.HorizontalAlignment = Element.ALIGN_CENTER;
                                I3.FixedHeight = 30f;
                                I3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                ImgTable.AddCell(I3);


                                #endregion

                                #region details Table

                                PdfPTable F3Table = new PdfPTable(3);
                                F3Table.WidthPercentage = 100;
                                float[] F3widths = new float[] { 40f, 15f, 40f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                                F3Table.SetWidths(F3widths);
                                F3Table.HorizontalAlignment = Element.ALIGN_LEFT;

                                //string VendName = string.Empty; string VendAddress = string.Empty; string Vendcity = string.Empty;// string VendPhone = string.Empty;
                                //MaskedTextBox mskvendphn = new MaskedTextBox();
                                //mskvendphn.Mask = "(000)000-0000";
                                //if (!string.IsNullOrEmpty(Txt_VendNo.Text.Trim()))
                                //{
                                //    CASEVDDEntity selvend = CaseVddlist.Find(u => u.Code.Trim().Equals(Txt_VendNo.Text.Trim()));
                                //    if (selvend != null)
                                //    {
                                //        VendName = selvend.Name.Trim();
                                //        VendAddress = selvend.Addr1.Trim();
                                //        Vendcity = selvend.City.Trim() + ", " + selvend.State.Trim() + "  " + selvend.Zip.Trim();
                                //        if (!string.IsNullOrEmpty(selvend.Phone.Trim()))
                                //            mskvendphn.Text = selvend.Phone.Trim();
                                //    }

                                //}

                                #region Vendor Name

                                PdfPTable VendNametable = new PdfPTable(1);
                                VendNametable.WidthPercentage = 100;
                                float[] VendNametablewidths = new float[] { 40f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                                VendNametable.SetWidths(VendNametablewidths);
                                VendNametable.HorizontalAlignment = Element.ALIGN_CENTER;

                                PdfPCell K1 = new PdfPCell(new Phrase("Vendor/Service Provider", TableFont));
                                K1.HorizontalAlignment = Element.ALIGN_LEFT;
                                K1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                VendNametable.AddCell(K1);

                                PdfPCell K2 = new PdfPCell(new Phrase(VendName, TableFont));
                                K2.HorizontalAlignment = Element.ALIGN_LEFT;
                                K2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                VendNametable.AddCell(K2);

                                #endregion


                                PdfPCell C1 = new PdfPCell(VendNametable);
                                //C1.HorizontalAlignment = Element.ALIGN_LEFT;
                                C1.FixedHeight = 32f;
                                C1.Border = iTextSharp.text.Rectangle.BOX;
                                F3Table.AddCell(C1);

                                PdfPCell C2 = new PdfPCell(ImgTable);
                                C2.Padding = 0f;
                                C2.Rowspan = 4;
                                C2.Border = iTextSharp.text.Rectangle.BOX;
                                F3Table.AddCell(C2);

                                #region Consumer Name

                                PdfPTable Nametable = new PdfPTable(1);
                                Nametable.WidthPercentage = 100;
                                float[] Nametablewidths = new float[] { 40f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                                Nametable.SetWidths(Nametablewidths);
                                Nametable.HorizontalAlignment = Element.ALIGN_CENTER;

                                PdfPCell J1 = new PdfPCell(new Phrase("Consumer Name", TableFont));
                                J1.HorizontalAlignment = Element.ALIGN_LEFT;
                                J1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                Nametable.AddCell(J1);

                                PdfPCell J2 = new PdfPCell(new Phrase(BaseForm.BaseApplicationName.Trim(), TableFont));
                                J2.HorizontalAlignment = Element.ALIGN_LEFT;
                                J2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                Nametable.AddCell(J2);

                                #endregion

                                PdfPCell C3 = new PdfPCell(Nametable);//new PdfPCell(new Phrase("Consumer Name", TableFont));
                                //C3.HorizontalAlignment = Element.ALIGN_LEFT;
                                C3.FixedHeight = 32f;
                                C3.Border = iTextSharp.text.Rectangle.BOX;
                                F3Table.AddCell(C3);

                                #region Vendor Address

                                PdfPTable VendNametable1 = new PdfPTable(1);
                                VendNametable1.WidthPercentage = 100;
                                float[] VendNametable1widths = new float[] { 40f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                                VendNametable1.SetWidths(VendNametable1widths);
                                VendNametable1.HorizontalAlignment = Element.ALIGN_CENTER;

                                PdfPCell K3 = new PdfPCell(new Phrase("Mailing Address", TableFont));
                                K3.HorizontalAlignment = Element.ALIGN_LEFT;
                                K3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                VendNametable1.AddCell(K3);

                                //string Apt = string.Empty; if (!string.IsNullOrEmpty(BaseForm.BaseCaseMstListEntity[0].Apt.Trim().Trim())) Apt = ", Apt: " + BaseForm.BaseCaseMstListEntity[0].Apt.Trim().Trim();
                                //string Flr = string.Empty; if (!string.IsNullOrEmpty(BaseForm.BaseCaseMstListEntity[0].Flr.Trim().Trim())) Apt = ", Flr: " + BaseForm.BaseCaseMstListEntity[0].Flr.Trim().Trim();

                                PdfPCell K4 = new PdfPCell(new Phrase(VendAddress, TableFont));
                                K4.HorizontalAlignment = Element.ALIGN_LEFT;
                                K4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                VendNametable1.AddCell(K4);

                                #endregion


                                PdfPCell C4 = new PdfPCell(VendNametable1);
                                //C4.HorizontalAlignment = Element.ALIGN_LEFT;
                                C4.FixedHeight = 30f;
                                C4.Border = iTextSharp.text.Rectangle.BOX;
                                F3Table.AddCell(C4);

                                #region Consumer Address

                                PdfPTable Nametable1 = new PdfPTable(1);
                                Nametable1.WidthPercentage = 100;
                                float[] Nametable1widths = new float[] { 40f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                                Nametable1.SetWidths(Nametable1widths);
                                Nametable1.HorizontalAlignment = Element.ALIGN_CENTER;

                                PdfPCell J3 = new PdfPCell(new Phrase("Address", TableFont));
                                J3.HorizontalAlignment = Element.ALIGN_LEFT;
                                J3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                Nametable1.AddCell(J3);

                                string Apt = string.Empty; if (!string.IsNullOrEmpty(BaseForm.BaseCaseMstListEntity[0].Apt.Trim().Trim())) Apt = ", Apt: " + BaseForm.BaseCaseMstListEntity[0].Apt.Trim().Trim();
                                string Flr = string.Empty; if (!string.IsNullOrEmpty(BaseForm.BaseCaseMstListEntity[0].Flr.Trim().Trim())) Apt = ", Flr: " + BaseForm.BaseCaseMstListEntity[0].Flr.Trim().Trim();

                                PdfPCell J4 = new PdfPCell(new Phrase(BaseForm.BaseCaseMstListEntity[0].Hn.Trim().Trim() + " " + BaseForm.BaseCaseMstListEntity[0].Street.Trim() + " " + BaseForm.BaseCaseMstListEntity[0].Suffix.Trim() + Apt + Flr, TableFont));
                                J4.HorizontalAlignment = Element.ALIGN_LEFT;
                                J4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                Nametable1.AddCell(J4);

                                #endregion

                                PdfPCell C5 = new PdfPCell(Nametable1);
                                //C5.HorizontalAlignment = Element.ALIGN_LEFT;
                                C5.FixedHeight = 32f;
                                C5.Border = iTextSharp.text.Rectangle.BOX;
                                F3Table.AddCell(C5);

                                #region Vendor City

                                PdfPTable VendNametable2 = new PdfPTable(1);
                                VendNametable2.WidthPercentage = 100;
                                float[] VendNametable2widths = new float[] { 40f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                                VendNametable2.SetWidths(VendNametable2widths);
                                VendNametable2.HorizontalAlignment = Element.ALIGN_CENTER;

                                PdfPCell K5 = new PdfPCell(new Phrase("City, State, Zip", TableFont));
                                K5.HorizontalAlignment = Element.ALIGN_LEFT;
                                K5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                VendNametable2.AddCell(K5);

                                PdfPCell K6 = new PdfPCell(new Phrase(Vendcity, TableFont));
                                K6.HorizontalAlignment = Element.ALIGN_LEFT;
                                K6.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                VendNametable2.AddCell(K6);

                                #endregion

                                PdfPCell C6 = new PdfPCell(VendNametable2);
                                //C6.HorizontalAlignment = Element.ALIGN_LEFT;
                                C6.FixedHeight = 32f;
                                C6.Border = iTextSharp.text.Rectangle.BOX;
                                F3Table.AddCell(C6);

                                #region Consumer City

                                PdfPTable Nametable2 = new PdfPTable(1);
                                Nametable2.WidthPercentage = 100;
                                float[] Nametable2widths = new float[] { 40f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                                Nametable2.SetWidths(Nametable2widths);
                                Nametable2.HorizontalAlignment = Element.ALIGN_CENTER;

                                PdfPCell J5 = new PdfPCell(new Phrase("City, State, Zip", TableFont));
                                J5.HorizontalAlignment = Element.ALIGN_LEFT;
                                J5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                Nametable2.AddCell(J5);

                                PdfPCell J6 = new PdfPCell(new Phrase(BaseForm.BaseCaseMstListEntity[0].City.Trim().Trim() + ", " + BaseForm.BaseCaseMstListEntity[0].State.Trim() + " " + "00000".Substring(0, 5 - BaseForm.BaseCaseMstListEntity[0].Zip.Trim().Length) + BaseForm.BaseCaseMstListEntity[0].Zip.Trim(), TableFont));
                                J6.HorizontalAlignment = Element.ALIGN_LEFT;
                                J6.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                Nametable2.AddCell(J6);

                                #endregion

                                PdfPCell C7 = new PdfPCell(Nametable2);
                                //C7.HorizontalAlignment = Element.ALIGN_LEFT;
                                C7.FixedHeight = 32f;
                                C7.Border = iTextSharp.text.Rectangle.BOX;
                                F3Table.AddCell(C7);

                                #region Vendor Phone

                                PdfPTable VendNametable3 = new PdfPTable(1);
                                VendNametable3.WidthPercentage = 100;
                                float[] VendNametable3widths = new float[] { 40f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                                VendNametable3.SetWidths(VendNametable3widths);
                                VendNametable3.HorizontalAlignment = Element.ALIGN_CENTER;

                                PdfPCell K7 = new PdfPCell(new Phrase("Phone", TableFont));
                                K7.HorizontalAlignment = Element.ALIGN_LEFT;
                                K7.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                VendNametable3.AddCell(K7);

                                PdfPCell K8 = new PdfPCell(new Phrase(mskvendphn.Text.Trim(), TableFont));
                                K8.HorizontalAlignment = Element.ALIGN_LEFT;
                                K8.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                VendNametable3.AddCell(K8);

                                #endregion

                                PdfPCell C8 = new PdfPCell(VendNametable3);
                                //C8.HorizontalAlignment = Element.ALIGN_LEFT;
                                C8.FixedHeight = 32f;
                                C8.Border = iTextSharp.text.Rectangle.BOX;
                                F3Table.AddCell(C8);

                                #region Consumer Account && Phn

                                PdfPTable Nametable3 = new PdfPTable(2);
                                Nametable3.WidthPercentage = 100;
                                float[] Nametable3widths = new float[] { 28f, 12f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                                Nametable3.SetWidths(Nametable3widths);
                                Nametable3.HorizontalAlignment = Element.ALIGN_CENTER;

                                PdfPCell J7 = new PdfPCell(new Phrase("Consumer Account Number", TableFont));
                                J7.HorizontalAlignment = Element.ALIGN_LEFT;
                                J7.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                Nametable3.AddCell(J7);

                                PdfPCell J8 = new PdfPCell(new Phrase("Phone", TableFont));
                                J8.HorizontalAlignment = Element.ALIGN_LEFT;
                                J8.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                Nametable3.AddCell(J8);

                                PdfPCell J9 = new PdfPCell(new Phrase("", TableFont));
                                J9.HorizontalAlignment = Element.ALIGN_LEFT;
                                J9.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                Nametable3.AddCell(J9);

                                MaskedTextBox mskphn = new MaskedTextBox();
                                mskphn.Mask = "(000)000-0000";
                                mskphn.Text = BaseForm.BaseCaseMstListEntity[0].Area.Trim().Trim() + BaseForm.BaseCaseMstListEntity[0].Phone.Trim().Trim();

                                PdfPCell J10 = new PdfPCell(new Phrase(mskphn.Text, TableFont));
                                J10.HorizontalAlignment = Element.ALIGN_LEFT;
                                J10.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                Nametable3.AddCell(J10);

                                #endregion

                                PdfPCell C9 = new PdfPCell(Nametable3);
                                //C9.HorizontalAlignment = Element.ALIGN_LEFT;
                                C9.FixedHeight = 32f;
                                C9.Border = iTextSharp.text.Rectangle.BOX;
                                F3Table.AddCell(C9);

                                #endregion

                                #region Paragraph Table

                                PdfPTable PTable = new PdfPTable(1);
                                PTable.WidthPercentage = 100;
                                float[] PTablewidths = new float[] { 20f };
                                PTable.SetWidths(PTablewidths);
                                PTable.HorizontalAlignment = Element.ALIGN_CENTER;

                                PdfPCell D1 = new PdfPCell(new Phrase("CERTIFICATION REGARDING", TableFont));
                                D1.HorizontalAlignment = Element.ALIGN_CENTER;
                                D1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                PTable.AddCell(D1);

                                PdfPCell D2 = new PdfPCell(new Phrase("(Debarment, Suspension, Ineligibility and Voluntary Exclusion)", TableFont));
                                D2.HorizontalAlignment = Element.ALIGN_CENTER;
                                D2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                PTable.AddCell(D2);

                                PdfPCell D3 = new PdfPCell(new Phrase("This certification is required by the requisitions implementing executive order 13549, debarment and suspension, CFR 1036 Appendix B.  The prospective lower tier participant certifies by submission of this proposal, that neither it nor its principals is presently debarred, suspended, proposed for debarment, declared ineligible, or voluntarily excluded from participation in this transaction by any federal department or agency.  Where the prospective lower tier participant is unable to certify any of the statements in this certification, such prospective participant shall attach an explanation to this proposal.", TableFont));
                                D3.HorizontalAlignment = Element.ALIGN_JUSTIFIED_ALL;
                                D3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                PTable.AddCell(D3);

                                PdfPCell D4 = new PdfPCell(new Phrase("Signature _________________________________________ Date __________", TableFont));
                                D4.HorizontalAlignment = Element.ALIGN_LEFT;
                                D4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                PTable.AddCell(D4);

                                PdfPCell Space2 = new PdfPCell(new Phrase("", TableFont));
                                Space2.HorizontalAlignment = Element.ALIGN_CENTER;
                                Space2.FixedHeight = 5f;
                                Space2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                PTable.AddCell(Space2);

                                PdfPCell D5 = new PdfPCell(new Phrase("Vendor Federal Tax ID", TableFont));
                                D5.HorizontalAlignment = Element.ALIGN_LEFT;
                                D5.Border = iTextSharp.text.Rectangle.BOX;
                                PTable.AddCell(D5);

                                #endregion

                                iTextSharp.text.Image _Logo = iTextSharp.text.Image.GetInstance(Application.MapPath("~\\Resources\\images\\CommunityAction1.jpg"));

                                //_Logo.ScalePercent(50f);
                                _Logo.ScalePercent(40f);

                                PdfPCell HF1 = new PdfPCell(_Logo);
                                HF1.HorizontalAlignment = Element.ALIGN_LEFT;
                                HF1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                FTable.AddCell(HF1);

                                PdfPCell HF2 = new PdfPCell(F2Table);
                                HF2.Padding = 0f;
                                HF2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                FTable.AddCell(HF2);

                                PdfPCell HF3 = new PdfPCell(F1Table);
                                HF3.Padding = 0f;
                                HF3.Border = iTextSharp.text.Rectangle.BOX;
                                FTable.AddCell(HF3);

                                PdfPCell Space = new PdfPCell(new Phrase("", TableFont));
                                Space.HorizontalAlignment = Element.ALIGN_LEFT;
                                Space.Colspan = 3;
                                Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                FTable.AddCell(Space);

                                PdfPCell HF4 = new PdfPCell(F3Table);
                                HF4.Padding = 0f;
                                HF4.Colspan = 2;
                                HF4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                FTable.AddCell(HF4);

                                PdfPCell HF5 = new PdfPCell(PTable);
                                HF5.Padding = 0f;
                                HF5.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                FTable.AddCell(HF5);

                                document.Add(FTable);

                                PdfPCell E1 = new PdfPCell(new Phrase("The information on this form is for " + dtAgency.Rows[0]["ACR_SHORT_NAME"].ToString() + " purposes only and is strictly confidential.  This is a legal document.", TblFontItalic));
                                E1.HorizontalAlignment = Element.ALIGN_LEFT;
                                E1.Colspan = 6;
                                E1.Border = iTextSharp.text.Rectangle.BOX;
                                STable.AddCell(E1);

                                #region First Row

                                PdfPCell R1 = new PdfPCell(new Phrase("Services", TblFontBold));
                                R1.HorizontalAlignment = Element.ALIGN_LEFT;
                                R1.Border = iTextSharp.text.Rectangle.BOX;
                                STable.AddCell(R1);

                                PdfPCell E3 = new PdfPCell(new Phrase("By signing this voucher I " + VendName.Trim() + " guarantee that the above named consumer and those family members listed on the rental/mortgage agreement will be allowed to dwell in the above named property for one month (no less than 30 days) as a result of this assistance.  Further, I state that I am the authorized owner, manager (landlord) and/or mortgage holder of said property.  ", TableFont));
                                E3.HorizontalAlignment = Element.ALIGN_LEFT;
                                E3.Rowspan = 4;
                                E3.Border = iTextSharp.text.Rectangle.BOX;
                                STable.AddCell(E3);

                                PdfPCell E4 = new PdfPCell(new Phrase("FUND", TblFontBold));
                                E4.HorizontalAlignment = Element.ALIGN_CENTER;
                                E4.Border = iTextSharp.text.Rectangle.BOX;
                                STable.AddCell(E4);

                                PdfPCell E5 = new PdfPCell(new Phrase("Program Code", TblFontBold));
                                E5.HorizontalAlignment = Element.ALIGN_CENTER;
                                E5.Border = iTextSharp.text.Rectangle.BOX;
                                STable.AddCell(E5);

                                PdfPCell E6 = new PdfPCell(new Phrase("County", TblFontBold));
                                E6.HorizontalAlignment = Element.ALIGN_CENTER;
                                E6.Border = iTextSharp.text.Rectangle.BOX;
                                STable.AddCell(E6);

                                PdfPCell E7 = new PdfPCell(new Phrase("Amt of Assistance", TblFont10));
                                E7.HorizontalAlignment = Element.ALIGN_CENTER;
                                E7.Border = iTextSharp.text.Rectangle.BOX;
                                STable.AddCell(E7);

                                #endregion

                                iTextSharp.text.Image _image_UnChecked = iTextSharp.text.Image.GetInstance(Consts.Icons.ico_checkbox);
                                _image_UnChecked.ScalePercent(60f);


                                #region Second Row

                                if (SP_CAMS_Grid1.Rows.Count > 0)
                                {
                                    CASEACTEntity CA_Pass_Entity = new CASEACTEntity();
                                    int Count = 1;
                                    foreach (DataGridViewRow dr in SP_CAMS_Grid1.Rows)
                                    {
                                        if (dr.Cells["Chk"].Value.ToString() == true.ToString())
                                        {
                                            if (SP_Activity_Details.Count > 0)
                                            {
                                                List<CASEACTEntity> selCaseactRecs = SP_Activity_Details.FindAll(u => u.Service_plan.Trim().Equals(ServicePlan_Code) && u.ACT_Code.Trim().Equals(dr.Cells["SP2_CAMS_Code"].Value.ToString().Trim()) && u.Group.Trim().Equals(dr.Cells["SP2_Group"].Value.ToString().Trim()) && u.Branch.Trim().Equals(dr.Cells["SP2_CAMS_Branch"].Value.ToString().Trim()) && u.ACT_Seq.Trim().Equals(dr.Cells["SP2_CA_Seq"].Value.ToString().Trim()) && u.Vendor_No.Trim().Equals(VendorBase[i].Substring(0,10).ToString().Trim()));
                                                
                                                if (selCaseactRecs != null)
                                                {
                                                    if (string.IsNullOrEmpty(Voucher.Trim()))
                                                    {
                                                        CA_Pass_Entity = selCaseactRecs.Find(u => u.VOUCHNO.Trim().Equals(string.Empty));
                                                    }
                                                    else
                                                    {
                                                        CA_Pass_Entity = selCaseactRecs.Find(u => u.VOUCHNO.Trim().Equals(Voucher.Trim()));
                                                    }
                                                }
                                                //CA_Pass_Entity = SP_Activity_Details.Find(u => u.Service_plan.Trim().Equals(ServicePlan_Code) && u.ACT_Code.Trim().Equals(dr.Cells["SP2_CAMS_Code"].Value.ToString().Trim()) && u.Group.Trim().Equals(dr.Cells["SP2_Group"].Value.ToString().Trim()) && u.Branch.Trim().Equals(dr.Cells["SP2_CAMS_Branch"].Value.ToString().Trim()) && u.ACT_Seq.Trim().Equals(dr.Cells["SP2_CA_Seq"].Value.ToString().Trim()) && u.Vendor_No.Trim().Equals(VendorBase[i].ToString().Trim()));
                                                if (CA_Pass_Entity != null)
                                                {
                                                    PdfPCell R2 = new PdfPCell(new Phrase(dr.Cells["SP2_Dup_Desc"].Value.ToString().Trim(), TblBFont8));
                                                    R2.HorizontalAlignment = Element.ALIGN_LEFT;
                                                    //SS1.FixedHeight = 15f;
                                                    R2.Border = iTextSharp.text.Rectangle.BOX;
                                                    STable.AddCell(R2);

                                                    if (Count > 3)
                                                    {
                                                        PdfPCell E11 = new PdfPCell(new Phrase("", TableFont));
                                                        E11.HorizontalAlignment = Element.ALIGN_LEFT;
                                                        E11.Border = iTextSharp.text.Rectangle.BOX;
                                                        STable.AddCell(E11);
                                                    }

                                                    PdfPCell SS2 = new PdfPCell(new Phrase(CA_Pass_Entity.Fund1.Trim(), TblBFont8));
                                                    SS2.HorizontalAlignment = Element.ALIGN_LEFT;
                                                    //SS2.FixedHeight = 15f;
                                                    SS2.Border = iTextSharp.text.Rectangle.BOX;
                                                    STable.AddCell(SS2);

                                                    string FundV = string.Empty, ProgV = string.Empty, CAV = string.Empty;
                                                    //List<CAVoucherEntity> FndVouchers=VoucherList.FindAll(u => u.Code.Equals(CA_Pass_Entity.Fund1.Trim()) && u.Type.Equals("FUND"));
                                                    List<VoucherEntity> FndVouchers = paymentVoucherList.FindAll(u => u.CAVASSOC_Code.Equals(CA_Pass_Entity.Fund1.Trim()) && u.CAVASSOC_Type.Equals("FUND") && Convert.ToDateTime(u.CAVD_FDate.Trim()) < Convert.ToDateTime(CA_Pass_Entity.ACT_Date.Trim()) && Convert.ToDateTime(u.CAVD_TDate.Trim()) > Convert.ToDateTime(CA_Pass_Entity.ACT_Date.Trim()));

                                                    List<VoucherEntity> ProgramVouchers = paymentVoucherList.FindAll(u => u.CAVASSOC_Code.Equals(CA_Pass_Entity.Act_PROG.Trim()) && u.CAVASSOC_Type.Equals("PROG") && Convert.ToDateTime(u.CAVD_FDate.Trim()) < Convert.ToDateTime(CA_Pass_Entity.ACT_Date.Trim()) && Convert.ToDateTime(u.CAVD_TDate.Trim()) > Convert.ToDateTime(CA_Pass_Entity.ACT_Date.Trim()));

                                                    List<VoucherEntity> CACodeVouchers = paymentVoucherList.FindAll(u => u.CAVASSOC_Code.Equals(CA_Pass_Entity.ACT_Code.Trim()) && u.CAVASSOC_Type.Equals("CA") && Convert.ToDateTime(u.CAVD_FDate.Trim()) < Convert.ToDateTime(CA_Pass_Entity.ACT_Date.Trim()) && Convert.ToDateTime(u.CAVD_TDate.Trim()) > Convert.ToDateTime(CA_Pass_Entity.ACT_Date.Trim()));

                                                    //CAVoucherEntity FundVouch = VoucherList.Find(u => u.Code.Equals(CA_Pass_Entity.Fund1.Trim()));
                                                    //CAVoucherEntity ProgramVouch = VoucherList.Find(u => u.Code.Equals(CA_Pass_Entity.Act_PROG.Trim()));
                                                    //CAVoucherEntity CACode = VoucherList.Find(u => u.Code.Equals(CA_Pass_Entity.ACT_Code.Trim()));

                                                    if (FndVouchers.Count > 0)
                                                        FundV = FndVouchers[0].CAVASSOC_vCode.Trim() ;
                                                    if(ProgramVouchers.Count>0)
                                                        ProgV = ProgramVouchers[0].CAVASSOC_vCode.Trim() ;
                                                    if (CACodeVouchers.Count > 0)
                                                        CAV = CACodeVouchers[0].CAVASSOC_vCode.Trim();


                                                    //if (FundVouch != null)
                                                    //{
                                                    //    if (CA_Pass_Entity.Fund1.Contains("CSB"))
                                                    //    {
                                                    //        if (FndVouchers.Count > 0)
                                                    //            FundVouch = FndVouchers.Find(u => u.Code.Equals(CA_Pass_Entity.Fund1.Trim()));
                                                    //        if(FundVouch==null) 
                                                    //            FundVouch = FndVouchers.Find(u => Convert.ToDateTime(u.FromDate.Trim()) < Convert.ToDateTime(CA_Pass_Entity.ACT_Date.Trim()) && Convert.ToDateTime(u.ToDate.Trim()) > Convert.ToDateTime(CA_Pass_Entity.ACT_Date.Trim()));
                                                    //    }
                                                    //    else if (CA_Pass_Entity.Fund1.Contains("EFSP"))
                                                    //    {
                                                    //        ProgramVouch = null;
                                                    //    }
                                                    //    if (FundVouch != null)
                                                    //        FundV = FundVouch.VCode.Trim() + "-";
                                                    //    //else if (Convert.ToDateTime(CA_Pass_Entity.ACT_Date.Trim()).Year < 2016) FundV = "207";
                                                    //}
                                                    ////else if (Convert.ToDateTime(CA_Pass_Entity.ACT_Date.Trim()).Year < 2016) FundV = "207";
                                                    //if (ProgramVouch != null) ProgV = ProgramVouch.VCode.Trim() + "-";
                                                    //if (CACode != null) CAV = CACode.VCode.Trim();
                                                    string ProgramCode1 = string.Empty;

                                                    if (CAVACNTFORMATList.Count>0)
                                                    {
                                                        string[] VoucherCode = new string[CAVACNTFORMATList.Count];
                                                        foreach(CAVACNTFORMATEntity AcntEntity in CAVACNTFORMATList)
                                                        {
                                                            switch(AcntEntity.CAVACCF_Type)
                                                            {
                                                                case "CA":  VoucherCode[int.Parse(AcntEntity.CAVACCF_SEQ)-1] = CAV.ToString(); break;
                                                                case "FUND": VoucherCode[int.Parse(AcntEntity.CAVACCF_SEQ)-1] = FundV.ToString(); break;
                                                                case "PROG": VoucherCode[int.Parse(AcntEntity.CAVACCF_SEQ)-1] = ProgV.ToString(); break;
                                                            }
                                                        }

                                                        if(VoucherCode.Length>0)
                                                        {
                                                            for(int z=0;z<VoucherCode.Length;z++)
                                                            {
                                                                if (!string.IsNullOrEmpty(VoucherCode[z].ToString().Trim()))
                                                                    ProgramCode1 += VoucherCode[z].ToString().Trim() + "-";
                                                            }
                                                        }

                                                        if (!string.IsNullOrEmpty(ProgramCode1.ToString().Trim()))
                                                            ProgramCode1 = ProgramCode1.Substring(0, ProgramCode1.Length - 1);

                                                    }

                                                    

                                                    //if (string.IsNullOrEmpty(ProgV.Trim()) && string.IsNullOrEmpty(CAV.Trim())) FundV = FundV.Replace("-", "");
                                                    //if (string.IsNullOrEmpty(CAV.Trim())) ProgV = ProgV.Replace("-", "");

                                                    //string ProgramCode = FundV + ProgV + CAV;
                                                    string ProgramCode = ProgramCode1;

                                                    PdfPCell SS3 = new PdfPCell(new Phrase(ProgramCode.Trim(), TblBFont8));
                                                    SS3.HorizontalAlignment = Element.ALIGN_LEFT;
                                                    //SS2.FixedHeight = 15f;
                                                    SS3.Border = iTextSharp.text.Rectangle.BOX;
                                                    STable.AddCell(SS3);

                                                    if (!string.IsNullOrEmpty(BaseForm.BaseCaseMstListEntity[0].County.Trim()))
                                                    {
                                                        string CountyDesc = string.Empty;
                                                        List<CommonEntity> Country = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.COUNTY, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, string.Empty); //_model.lookupDataAccess.GetCountry();
                                                        foreach (CommonEntity country in Country)
                                                        {
                                                            if (BaseForm.BaseCaseMstListEntity[0].County.Trim() == country.Code.Trim())
                                                            {
                                                                CountyDesc = country.Desc.Trim();
                                                                break;
                                                            }
                                                        }

                                                        PdfPCell SS4 = new PdfPCell(new Phrase(CountyDesc, TblBFont8));
                                                        SS4.HorizontalAlignment = Element.ALIGN_LEFT;
                                                        //SS2.FixedHeight = 15f;
                                                        SS4.Border = iTextSharp.text.Rectangle.BOX;
                                                        STable.AddCell(SS4);
                                                    }
                                                    else
                                                    {
                                                        PdfPCell SS4 = new PdfPCell(new Phrase("", TblBFont8));
                                                        SS4.HorizontalAlignment = Element.ALIGN_LEFT;
                                                        SS4.FixedHeight = 15f;
                                                        SS4.Border = iTextSharp.text.Rectangle.BOX;
                                                        STable.AddCell(SS4);
                                                    }

                                                    PdfPCell SS5 = new PdfPCell(new Phrase(CA_Pass_Entity.Cost.Trim(), TblBFont8));
                                                    SS5.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                    //SS2.FixedHeight = 15f;
                                                    SS5.Border = iTextSharp.text.Rectangle.BOX;
                                                    STable.AddCell(SS5);

                                                    Count++;

                                                    if (string.IsNullOrEmpty(dr.Cells["gvtVoucherNo"].Value.ToString().Trim()))
                                                        str.Append("<Details ACT_ID = \"" + CA_Pass_Entity.ACT_ID + "\" />");




                                                }
                                            }

                                            if (Count >= 14) break;
                                        }
                                    }

                                    if (Count < 5)
                                    {
                                        for (int j = Count; j < 4; j++)
                                        {
                                            PdfPCell E12 = new PdfPCell(new Phrase("", TableFont));
                                            E12.FixedHeight = 15f;
                                            E12.HorizontalAlignment = Element.ALIGN_LEFT;
                                            E12.Border = iTextSharp.text.Rectangle.BOX;
                                            STable.AddCell(E12);

                                            SpaceCells(STable, TableFont);
                                        }
                                        Count = 4;
                                    }

                                    if (Count < 14)
                                    {
                                        for (int k = Count; k < 14; k++)
                                        {
                                            PdfPCell E11 = new PdfPCell(new Phrase("", TableFont));
                                            E11.FixedHeight = 15f;
                                            E11.HorizontalAlignment = Element.ALIGN_LEFT;
                                            E11.Border = iTextSharp.text.Rectangle.BOX;
                                            STable.AddCell(E11);

                                            PdfPCell E12 = new PdfPCell(new Phrase("", TableFont));
                                            E12.FixedHeight = 15f;
                                            E12.HorizontalAlignment = Element.ALIGN_LEFT;
                                            E12.Border = iTextSharp.text.Rectangle.BOX;
                                            STable.AddCell(E12);

                                            SpaceCells(STable, TableFont);
                                        }
                                    }


                                }
                                #endregion

                                #region authorization

                                PdfPTable LTable = new PdfPTable(3);
                                LTable.WidthPercentage = 100;
                                float[] LTablewidths = new float[] { 50f, 50f, 50f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                                LTable.SetWidths(LTablewidths);
                                LTable.HorizontalAlignment = Element.ALIGN_LEFT;

                                PdfPCell H1 = new PdfPCell(new Phrase("Authorized by                         Date          ", TableFont));
                                H1.HorizontalAlignment = Element.ALIGN_LEFT;
                                H1.FixedHeight = 25f;
                                H1.Border = iTextSharp.text.Rectangle.BOX;
                                LTable.AddCell(H1);

                                PdfPCell H2 = new PdfPCell(new Phrase("Consumer Signature                            Date          ", TableFont));
                                H2.HorizontalAlignment = Element.ALIGN_LEFT;
                                H2.FixedHeight = 25f;
                                H2.Border = iTextSharp.text.Rectangle.BOX;
                                LTable.AddCell(H2);

                                PdfPCell H3 = new PdfPCell(new Phrase("Program Director                          Date          ", TableFont));
                                H3.HorizontalAlignment = Element.ALIGN_LEFT;
                                H3.FixedHeight = 25f;
                                H3.Border = iTextSharp.text.Rectangle.BOX;
                                LTable.AddCell(H3);

                                #endregion

                                PdfPCell E31 = new PdfPCell(LTable);
                                E31.Padding = 0f;
                                E31.Colspan = 6;
                                E31.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                STable.AddCell(E31);

                                PdfPCell E32 = new PdfPCell(new Phrase("I understand that if the above mentioned services are not rendered or breached in any way, the monies paid by " + dtAgency.Rows[0]["ACR_SHORT_NAME"].ToString() + " are to be returned immediately to " + dtAgency.Rows[0]["ACR_SHORT_NAME"].ToString() + ".  Failure to do so or adhere to the conditions of this agreement will likely result in legal action.  I(we) certify that the services listed above have been performed for the client or articles received as authorized on this order and that payment therefore is due. ", TableFont));
                                E32.HorizontalAlignment = Element.ALIGN_LEFT;
                                E32.Colspan = 6;
                                E32.Border = iTextSharp.text.Rectangle.BOX;
                                STable.AddCell(E32);

                                #region Vendor

                                PdfPTable L1Table = new PdfPTable(3);
                                L1Table.WidthPercentage = 100;
                                float[] LTable1widths = new float[] { 50f, 50f, 50f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                                L1Table.SetWidths(LTable1widths);
                                L1Table.HorizontalAlignment = Element.ALIGN_LEFT;

                                PdfPCell H4 = new PdfPCell(new Phrase("Vendor/Service Provider                           Date          ", TableFont));
                                H4.HorizontalAlignment = Element.ALIGN_LEFT;
                                H4.FixedHeight = 25f;
                                H4.Border = iTextSharp.text.Rectangle.BOX;
                                L1Table.AddCell(H4);

                                PdfPCell H5 = new PdfPCell(new Phrase("Vendor/Service Provider Address (if different from above)        ", TableFont));
                                H5.HorizontalAlignment = Element.ALIGN_LEFT;
                                H5.FixedHeight = 25f;
                                H5.Border = iTextSharp.text.Rectangle.BOX;
                                L1Table.AddCell(H5);

                                PdfPCell H6 = new PdfPCell(new Phrase("Finance Director                          Date          ", TableFont));
                                H6.HorizontalAlignment = Element.ALIGN_LEFT;
                                H6.FixedHeight = 25f;
                                H6.Border = iTextSharp.text.Rectangle.BOX;
                                L1Table.AddCell(H6);

                                #endregion

                                PdfPCell E33 = new PdfPCell(L1Table);
                                E33.Padding = 0f;
                                E33.Colspan = 6;
                                E33.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                STable.AddCell(E33);

                                PdfPCell E34 = new PdfPCell(new Phrase("Submit voucher for payment within five (5) days to " + dtAgency.Rows[0]["ACR_SHORT_NAME"].ToString() + " at the address listed above.  An attached itemized receipt of services/items is required.", TableFont));
                                E34.HorizontalAlignment = Element.ALIGN_CENTER;
                                E34.Colspan = 6;
                                E34.Border = iTextSharp.text.Rectangle.BOX;
                                STable.AddCell(E34);

                                PdfPCell E35 = new PdfPCell(new Phrase("Service Provider should receive payment within twenty (20) days.", TableFont));
                                E35.HorizontalAlignment = Element.ALIGN_CENTER;
                                E35.Colspan = 6;
                                E35.Border = iTextSharp.text.Rectangle.BOX;
                                STable.AddCell(E35);

                                document.Add(STable);
                                document.NewPage();
                                if (STable.Rows.Count > 0)
                                {
                                    FTable.DeleteBodyRows();
                                    STable.DeleteBodyRows();
                                }



                            }
                            //}

                            str.Append("</CASEACT>");
                            if (Agcycntl_SW == "Y")
                            {
                                //DatabaseLayer.SPAdminDB.UpdateVouchers(BaseForm.BaseAgency, Vendor_Activity_Details[0].ACT_ID.Trim(), "CA", Vendor_Activity_Details[0].VOUCHNO.Trim());
                                DatabaseLayer.SPAdminDB.UpdateVouchers(BaseForm.BaseAgency, str.ToString(),txtManual.Visible==true?"M":"A",txtManual.Text.Trim(), "CA");
                                GetAgencyDetails();
                               

                            }
                        }
                    }

                    txtManual.Visible = false;

                }
            }
            catch (Exception ex) { document.Add(new Paragraph("Aborted due to Exception............................................... ")); }
            document.Close();
            fs.Close();
            fs.Dispose();

            if (BaseForm.BaseAgencyControlDetails.ReportSwitch.ToUpper() == "Y")
            {
                PdfViewerNewForm objfrm = new PdfViewerNewForm(PdfName);
                objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                objfrm.StartPosition=FormStartPosition.CenterScreen;
                objfrm.ShowDialog();
            }
            else
            {
                FrmViewer objfrm = new FrmViewer(PdfName);
                objfrm.FormClosed += new FormClosedEventHandler(On_Delete_PDF_File);
                objfrm.StartPosition = FormStartPosition.CenterScreen;
                objfrm.ShowDialog();
            }
        }

        private void SpaceCells(PdfPTable STable, iTextSharp.text.Font TableFont)
        {
            PrintSpaceCell(STable, 1, TableFont);
            PrintSpaceCell(STable, 1, TableFont);
            PrintSpaceCell(STable, 1, TableFont);
            PrintSpaceCell(STable, 1, TableFont);
        }

        private void On_Delete_PDF_File(object sender, FormClosedEventArgs e)
        {
            System.IO.File.Delete(PdfName);
            GetCaseACtEntity();
            FillCAMSGrid();
            GetAgencyDetails();
            Vendor = string.Empty;
            
        }

        private void PrintSpaceCell(PdfPTable table, int Spacesnum, iTextSharp.text.Font TableFont)
        {
            if (Spacesnum == 1)
            {
                PdfPCell S2 = new PdfPCell(new Phrase("", TableFont));
                S2.HorizontalAlignment = Element.ALIGN_LEFT;
                S2.Border = iTextSharp.text.Rectangle.BOX;
                table.AddCell(S2);
            }
            else if (Spacesnum == 2)
            {
                PdfPCell S2 = new PdfPCell(new Phrase("", TableFont));
                S2.HorizontalAlignment = Element.ALIGN_LEFT;
                S2.Colspan = 2;
                S2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                table.AddCell(S2);
            }
            else if (Spacesnum == 3)
            {
                PdfPCell S2 = new PdfPCell(new Phrase("", TableFont));
                S2.HorizontalAlignment = Element.ALIGN_LEFT;
                S2.Colspan = 3;
                S2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                table.AddCell(S2);
            }
            else if (Spacesnum == 4)
            {
                PdfPCell S2 = new PdfPCell(new Phrase("", TableFont));
                S2.HorizontalAlignment = Element.ALIGN_LEFT;
                S2.Colspan = 4;
                S2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                table.AddCell(S2);
            }
            else if (Spacesnum == 6)
            {
                PdfPCell S2 = new PdfPCell(new Phrase("", TableFont));
                S2.HorizontalAlignment = Element.ALIGN_LEFT;
                S2.Colspan = 6;
                S2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                table.AddCell(S2);
            }
            else if (Spacesnum == 7)
            {
                PdfPCell S2 = new PdfPCell(new Phrase("", TableFont));
                S2.HorizontalAlignment = Element.ALIGN_LEFT;
                S2.Colspan = 7;
                S2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                table.AddCell(S2);
            }
            else if (Spacesnum == 10)
            {
                PdfPCell S2 = new PdfPCell(new Phrase("", TableFont));
                S2.HorizontalAlignment = Element.ALIGN_LEFT;
                S2.Colspan = 10;
                S2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                table.AddCell(S2);
            }
            else if (Spacesnum == 15)
            {
                PdfPCell S2 = new PdfPCell(new Phrase("", TableFont));
                S2.HorizontalAlignment = Element.ALIGN_LEFT;
                S2.Colspan = 15;
                S2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                table.AddCell(S2);
            }
            else if (Spacesnum == 12)
            {
                PdfPCell S2 = new PdfPCell(new Phrase("", TableFont));
                S2.HorizontalAlignment = Element.ALIGN_LEFT;
                S2.Colspan = 12;
                S2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                table.AddCell(S2);
            }
        }

        private void GetAgencyDetails()
        {
            dsAgency = DatabaseLayer.ADMNB001DB.ADMNB001_Browse_AGCYCNTL(BaseForm.BaseAgency, null, null, null, null, null, null);
            dtAgency = dsAgency.Tables[0];
        }

        #endregion

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in SP_CAMS_Grid1.Rows)
            {
                if (item.Cells["Chk"].ReadOnly == false)
                    item.Cells["Chk"].Value = true;
            }
        }

        private void btnUnSelect_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in SP_CAMS_Grid1.Rows)
            {
                item.Cells["Chk"].Value = false;
            }
        }

        private void btnManual_Click(object sender, EventArgs e)
        {
            bool IsFollow = false;
            if (SP_CAMS_Grid1.Rows.Count > 0)
            {
                foreach (DataGridViewRow dr in SP_CAMS_Grid1.Rows)
                {
                    if (dr.Cells["Chk"].Value.ToString() == true.ToString() && string.IsNullOrEmpty(dr.Cells["gvtVoucherNo"].Value.ToString()))
                    {
                        txtManual.Visible = true; btnClear.Visible = true; IsFollow = true; break;
                    }
                }

                if (!IsFollow)
                {
                    txtManual.Visible = false;
                    txtManual.Text = string.Empty;
                    btnClear.Visible = false;
                }
            }
            
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtManual.Visible = false;
            txtManual.Text = string.Empty;
            btnClear.Visible = false;
        }

    }
}