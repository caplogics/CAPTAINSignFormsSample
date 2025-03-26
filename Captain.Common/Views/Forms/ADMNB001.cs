#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
//using Excel = Microsoft.Office.Interop.Excel;
using Wisej.Web;
//using Gizmox.WebGUI.Common;
//using Wisej.Web;
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
//using System.Drawing;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using CarlosAg.ExcelXmlWriter;
#endregion

namespace Captain.Common.Views.Forms
{
    public partial class ADMNB001 : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private bool boolChangeStatus = false;
        private CaptainModel _model = null;
        private List<HierarchyEntity> _selectedHierarchies = null;


        #endregion
        public ADMNB001(BaseForm baseform, PrivilegeEntity privileges)
        {
            _model = new CaptainModel();
            BaseForm = baseform;
            Privileges = privileges;
            InitializeComponent();
            this.Size = new System.Drawing.Size(435, 228);//(516, 297);
            CmbRepType.Size = new System.Drawing.Size(300, 25);
            //this.pnlTypes.Size = new System.Drawing.Size(500,220);//(515, 200);
            propReportPath = _model.lookupDataAccess.GetReportPath();
            // LblHeader.Text = privileges.PrivilegeName;
            lblHie.Visible = true;
            this.CmbAgyTab.Size = new System.Drawing.Size(300, 25);
            // this.CmbAgyTab.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            //  this.HieFilter.Location = new System.Drawing.Point(331, 64);
            //  this.CmbAgyTab.Location = new System.Drawing.Point(123, 108);
            //this.LblAgyTab.Location = new System.Drawing.Point(21, 111);
            LblAgyTab.Text = "Application";
            lblHie.Text = "Agency Table";

            cmbModule.Visible = true;
            //   this.LblAgyTab.Location = new System.Drawing.Point(35, 111);
            this.lblHie.Location = new System.Drawing.Point(15, 8);
            this.cmbModule.Size = new System.Drawing.Size(300, 25);
            //this.cmbModule.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            //this.HieFilter.Location = new System.Drawing.Point(331, 64);
            this.cmbModule.Location = new System.Drawing.Point(110, 4);//(123, 136);
            this.lblHie.Size = new System.Drawing.Size(76, 16);
            //strFolderPath = Consts.Common.ReportFolderLocation + BaseForm.UserID + "\\";    // Run at Server

            FillAllCombos();

            PopulateDropDowns();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;

        }

        #region properties

        public BaseForm BaseForm { get; set; }

        public PrivilegeEntity Privileges { get; set; }

        public string propReportPath { get; set; }

        #endregion

        string strFolderPath = string.Empty;
        DirectoryInfo MyDir;

        string RepType = null;
        int X_Pos, Y_Pos; string PrintText = null;
        PdfContentByte cb;
        //BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, false);


        private void FillAllCombos()
        {
            CmbRepType.Items.Clear();
            List<RepListItem> listItem2 = new List<RepListItem>();
            listItem2.Add(new RepListItem("Agency Table List", "01"));
            //listItem2.Add(new RepListItem("Action Plan Associations", "02"));
            //listItem2.Add(new RepListItem("Case Demographics Associations", "03"));
            //listItem2.Add(new RepListItem("Performance Measures Associations", "04"));
            listItem2.Add(new RepListItem("Critical Activities List", "05"));
            listItem2.Add(new RepListItem("Outcomes List", "06"));
            //listItem2.Add(new RepListItem("Service Codes List", "07"));
            listItem2.Add(new RepListItem("ZIP Code", "08"));
            //listItem2.Add(new RepListItem("Hierarchy Report", "09"));
            listItem2.Add(new RepListItem("Field Control Maintenance", "10"));
            if (BaseForm.UserProfile.Security.Trim() == "P" || BaseForm.UserProfile.Security.Trim() == "B")
            {
                listItem2.Add(new RepListItem("Access Logs", "11"));
                //listItem2.Add(new RepListItem("Report Logs", "12"));
            }
            listItem2.Add(new RepListItem("RNG Case Demographics Associations", "13"));
            listItem2.Add(new RepListItem("RNG Performance Measures Associations", "14"));
            listItem2.Add(new RepListItem("RNG Service Associations", "15"));
            listItem2.Add(new RepListItem("Programs & Reports", "16"));

            CmbRepType.Items.AddRange(listItem2.ToArray());
            CmbRepType.SelectedIndex = 0;

            DataSet ds1 = Captain.DatabaseLayer.ADMNB002DB.GetUserNames();
            DataTable dt1 = ds1.Tables[0];
            List<Captain.Common.Utilities.ListItem> listItem = new List<Captain.Common.Utilities.ListItem>();
            listItem.Clear();
            listItem.Add(new Captain.Common.Utilities.ListItem("All Users", "**"));
            foreach (DataRow dr in dt1.Rows)
            {
                listItem.Add(new Captain.Common.Utilities.ListItem(dr["PWR_EMPLOYEE_NO"].ToString(), dr["PWR_EMPLOYEE_NO"].ToString()));
            }
            CmbUser.Items.AddRange(listItem.ToArray());
            CmbUser.SelectedIndex = 0;
            cmbRepUserId.Items.AddRange(listItem.ToArray());
            cmbRepUserId.SelectedIndex = 0;
        }

        private void PopulateDropDowns()
        {
            if (RepType == "01")
            {
                this.CmbAgyTab.SelectedIndexChanged -= new System.EventHandler(this.CmbAgyTab_SelectedIndexChanged);
                CmbAgyTab.Items.Clear();
                this.CmbAgyTab.SelectedIndexChanged += new System.EventHandler(this.CmbAgyTab_SelectedIndexChanged);
                DataSet dsMod = Captain.DatabaseLayer.Lookups.GetModules();
                if (dsMod.Tables.Count > 0)
                {
                    DataTable dtMod = dsMod.Tables[0];

                    List<Captain.Common.Utilities.ListItem> listItem1 = new List<Captain.Common.Utilities.ListItem>();

                    listItem1.Add(new Captain.Common.Utilities.ListItem("All Applications", "**"));
                    foreach (DataRow dr in dtMod.Rows)
                    {
                        listItem1.Add(new Captain.Common.Utilities.ListItem(dr["APPL_DESCRIPTION"].ToString(), dr["APPL_CODE"].ToString()));
                    }
                    CmbAgyTab.Items.AddRange(listItem1.ToArray());
                    CmbAgyTab.SelectedIndex = 0;
                }


                //DataSet ds = Captain.DatabaseLayer.AgyTab.GetAgencyTableByApp("**");
                //DataTable dt = ds.Tables[0];
                //DataView dv = new DataView(dt);
                //if (RblCaseMgmt.Checked == true)
                //    dv.Sort = "AGY_TYPE";
                //else dv.Sort = "AGY_DESC";
                //dt = dv.ToTable();
                //CmbAgyTab.Items.Clear();
                //List<RepListItem> listItem = new List<RepListItem>();
                //CmbAgyTab.Items.Insert(0, new RepListItem("All Tables", "*****"));
                //string Tmp_Desc = string.Empty;
                //foreach (DataRow dr in dt.Rows)
                //{
                //    Tmp_Desc = string.Empty;
                //    Tmp_Desc = String.Format("{0,-35}", dr["AGY_DESC"].ToString().Trim()) + String.Format("{0,8}", " - " + dr["AGY_TYPE"].ToString());
                //    CmbAgyTab.Items.Add(new RepListItem(Tmp_Desc, dr["AGY_TYPE"].ToString()));
                //}
                //CmbAgyTab.SelectedIndex = 0;




            }
            else if (RepType == "10")
            {

                FillScreenCombo();
            }
            else if (RepType == "04")
            {
                fillComboRefDate();
            }
            else if (RepType == "14")
            {
                fillRNGComboRefDate();
            }
            else if (RepType == "15")
            {
                fillRNGSERComboRefDate();
            }
            else if (RepType == "16")
            {
                DataSet ds = Captain.DatabaseLayer.Lookups.GetModules();
                DataTable dt = ds.Tables[0];
                CmbAgyTab.Items.Clear();
                List<Captain.Common.Utilities.ListItem> listItem = new List<Captain.Common.Utilities.ListItem>();

                listItem.Add(new Captain.Common.Utilities.ListItem("All Applications", "**"));
                foreach (DataRow dr in dt.Rows)
                {
                    listItem.Add(new Captain.Common.Utilities.ListItem(dr["APPL_DESCRIPTION"].ToString(), dr["APPL_CODE"].ToString()));
                }
                CmbAgyTab.Items.AddRange(listItem.ToArray());
                CmbAgyTab.SelectedIndex = 0;
                //fillRNGSERComboRefDate();
            }
        }

        private void FillScreenCombo()
        {
            this.CmbAgyTab.SelectedIndexChanged -= new System.EventHandler(this.CmbAgyTab_SelectedIndexChanged);
            CmbAgyTab.Items.Clear();
            this.CmbAgyTab.SelectedIndexChanged += new System.EventHandler(this.CmbAgyTab_SelectedIndexChanged);
            List<Captain.Common.Utilities.ListItem> listItem2 = new List<Captain.Common.Utilities.ListItem>();
            List<FLDSCRSEntity> FLDSCRS_List = new List<FLDSCRSEntity>();
            //listItem.Add(new ListItem("Form Title                                    -  SCR-CODE", SCR-CODE, Form Title, Custom_Filedls_Flag));
            //listItem2.Add(new Captain.Common.Utilities.ListItem("Client Intake                                    -  CASE2001", "CASE2001", "Client Intake", "Y"));
            //listItem2.Add(new Captain.Common.Utilities.ListItem("Income Entry                                   -  CASINCOM", "CASINCOM", "Income Entry", "N"));
            //listItem2.Add(new Captain.Common.Utilities.ListItem("Income Verification                          -  CASE2003", "CASE2003", "Income Verification", "N"));
            //listItem2.Add(new Captain.Common.Utilities.ListItem("Contact Posting                               -  CASE0006", "CASE0061", "Contact Posting", "Y"));
            //listItem2.Add(new Captain.Common.Utilities.ListItem("Critical Activity Posting                    -  CASE0006", "CASE0062", "Critical Activity Posting", "Y"));
            //listItem2.Add(new Captain.Common.Utilities.ListItem("Milestone Posting                             -  CASE0006", "CASE0063", "Milestone Posting", "N"));
            //listItem2.Add(new Captain.Common.Utilities.ListItem("Medical/Emergency                          -  CASE2330", "CASE2330", "Medical/Emergency", "N"));
            //listItem2.Add(new Captain.Common.Utilities.ListItem("Track Master Maintenance               -  HSS00133", "HSS00133", "Track Master Maintenance", "Y"));

            //listItem2.Add(new Captain.Common.Utilities.ListItem("Client Intake                                -  CASE2001", "CASE2001", " ", "Y"));
            //listItem2.Add(new Captain.Common.Utilities.ListItem("Income Entry                               -  CASINCOM", "CASINCOM", " ", "N"));
            //listItem2.Add(new Captain.Common.Utilities.ListItem("Income Verification                      -  CASE2003", "CASE2003", " ", "N"));
            //listItem2.Add(new Captain.Common.Utilities.ListItem("Contact Posting                           -  CASE0006", "CASE0061", " ", "Y"));
            //listItem2.Add(new Captain.Common.Utilities.ListItem("Critical Activity Posting                -  CASE0006", "CASE0062", " ", "Y"));
            //listItem2.Add(new Captain.Common.Utilities.ListItem("Milestone Posting                             -  CASE0006", "CASE0063", " ", "N"));
            FLDSCRSEntity Search_Entity = new FLDSCRSEntity(true);
            Search_Entity.Called_By = "CASE0008";
            FLDSCRS_List = _model.FieldControls.Browse_FLDSCRS(Search_Entity);

            string Tmp_Desc = string.Empty;

            foreach (FLDSCRSEntity Entity in FLDSCRS_List)
            {
                if ((Entity.Scr_Code == "CASE2001" && Entity.Scr_Sub_Code == "00") || Entity.Scr_Code != "CASE2001")
                {
                    Tmp_Desc = string.Empty;
                    Tmp_Desc = String.Format("{0,-30}", Entity.Scr_Desc) + String.Format("{0,8}", " - " + Entity.Scr_Code);
                    CmbAgyTab.Items.Add(new Captain.Common.Utilities.ListItem(Tmp_Desc, Entity.Scr_Code, Entity.Scr_Desc, Entity.Cust_Ques_SW));
                }
            }
            if (CmbAgyTab.Items.Count > 0)
                CmbAgyTab.SelectedIndex = 0;
            //CmbAgyTab.Items.AddRange(listItem2.ToArray());
            //CmbAgyTab.SelectedIndex = 0;
            //ScrCode = "CASE2001";
        }

        string TmpDOB = null, TmpRefTdate = null;
        private void fillComboRefDate()
        {
            DataSet dsCsb16 = DatabaseLayer.SPAdminDB.Get_CSB16();
            DataTable dtCsb16;
            dtCsb16 = dsCsb16.Tables[0];
            CmbAgyTab.Items.Clear();
            int Sel_Ind = 0; int Cnt = 0;
            List<RepListItem> listitem = new List<RepListItem>();

            foreach (DataRow drCsb16 in dtCsb16.Rows)
            {
                TmpDOB = LookupDataAccess.Getdate(drCsb16["C16DTR_REF_FDATE"].ToString());


                TmpRefTdate = LookupDataAccess.Getdate(drCsb16["C16DTR_REF_TDATE"].ToString());


                CmbAgyTab.Items.Add(new RepListItem(TmpDOB + "   -   " + TmpRefTdate, TmpDOB + "," + TmpRefTdate));
                //if (TmpDOB == Added_Edited_Date)
                //    Sel_Ind = Cnt;
                Cnt++;
            }
            if (dtCsb16.Rows.Count > 0)
                CmbAgyTab.SelectedIndex = Sel_Ind;
        }


        private void fillRNGComboRefDate()
        {
            //DataSet dsCsb16 = DatabaseLayer.SPAdminDB.Get_CSB16();
            //DataTable dtCsb16;
            //dtCsb16 = dsCsb16.Tables[0];
            CmbAgyTab.Items.Clear();
            int Sel_Ind = 0; int Cnt = 0;
            List<RepListItem> listitem = new List<RepListItem>();

            List<RCsb14GroupEntity> CODEList = _model.SPAdminData.Browse_RNGGrp(null, null, null, null, null, BaseForm.UserID, BaseForm.BaseAdminAgency);
            if (CODEList.Count > 0)
            {
                foreach (RCsb14GroupEntity GrpEnt in CODEList)
                {
                    if (string.IsNullOrWhiteSpace(GrpEnt.GrpCode.Trim()) && string.IsNullOrWhiteSpace(GrpEnt.TblCode.Trim()))
                    {
                        TmpDOB = LookupDataAccess.Getdate(GrpEnt.OFdate.ToString());


                        TmpRefTdate = LookupDataAccess.Getdate(GrpEnt.OTdate.ToString());


                        CmbAgyTab.Items.Add(new RepListItem(TmpDOB + "   -   " + TmpRefTdate, GrpEnt.Code, GrpEnt.GrpDesc.Trim(), GrpEnt.Agency.Trim()));
                        Cnt++;
                    }
                }
            }

            //foreach (DataRow drCsb16 in dtCsb16.Rows)
            //{
            //    TmpDOB = LookupDataAccess.Getdate(drCsb16["C16DTR_REF_FDATE"].ToString());


            //    TmpRefTdate = LookupDataAccess.Getdate(drCsb16["C16DTR_REF_TDATE"].ToString());


            //    CmbAgyTab.Items.Add(new RepListItem(TmpDOB + "   -   " + TmpRefTdate, TmpDOB + "," + TmpRefTdate));
            //    //if (TmpDOB == Added_Edited_Date)
            //    //    Sel_Ind = Cnt;
            //    Cnt++;
            //}
            if (CODEList.Count > 0)
                CmbAgyTab.SelectedIndex = Sel_Ind;
        }

        private void fillRNGSERComboRefDate()
        {
            //DataSet dsCsb16 = DatabaseLayer.SPAdminDB.Get_CSB16();
            //DataTable dtCsb16;
            //dtCsb16 = dsCsb16.Tables[0];
            CmbAgyTab.Items.Clear();
            int Sel_Ind = 0; int Cnt = 0;
            List<RepListItem> listitem = new List<RepListItem>();

            List<SRCsb14GroupEntity> CODEList = _model.SPAdminData.Browse_RNGSRGrp(null, null, null, null, null, BaseForm.UserID, BaseForm.BaseAdminAgency);
            if (CODEList.Count > 0)
            {
                foreach (SRCsb14GroupEntity GrpEnt in CODEList)
                {
                    if (string.IsNullOrWhiteSpace(GrpEnt.GrpCode.Trim()) && string.IsNullOrWhiteSpace(GrpEnt.TblCode.Trim()))
                    {
                        TmpDOB = LookupDataAccess.Getdate(GrpEnt.OFdate.ToString());


                        TmpRefTdate = LookupDataAccess.Getdate(GrpEnt.OTdate.ToString());


                        CmbAgyTab.Items.Add(new RepListItem(TmpDOB + "   -   " + TmpRefTdate, GrpEnt.Code, GrpEnt.GrpDesc.Trim(), GrpEnt.Agency.Trim()));
                        Cnt++;
                    }
                }
            }

            if (CODEList.Count > 0)
                CmbAgyTab.SelectedIndex = Sel_Ind;
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

        bool FirstRun = true;

        string PdfName = "Pdf File";
        string Random_Filename = null;
        int pageNumber = 1;
        private void BtnGenFile_Click(object sender, EventArgs e)
        {
            //X_Pos = 30; Y_Pos = 720;
            //ExcelAgencyTable();
            if (ValidateForm())
            {
                PdfListForm pdfListForm = new PdfListForm(BaseForm, Privileges, false, propReportPath,"PDF");
                pdfListForm.FormClosed += new FormClosedEventHandler(On_SaveForm_Closed);
                pdfListForm.StartPosition = FormStartPosition.CenterScreen;
                pdfListForm.ShowDialog();
            }
        }


        private bool ValidateForm()
        {
            bool isValid = true;
            if (RepType == "10")
            {
                if (string.IsNullOrEmpty(txtHie.Text.Trim()))
                {
                    _errorProvider.SetError(HieFilter, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), lblHie.Text.Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(HieFilter, null);
                }
            }
            if(dtRepDate.Checked == false)
            {
                _errorProvider.SetError(dtRepDate, "Please select 'Date'");
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtRepDate, null);
            }
            if (dtRepDate.Checked.Equals(true))
            {
                if (string.IsNullOrWhiteSpace(dtRepDate.Text))
                {
                    _errorProvider.SetError(dtRepDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "Date".Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtRepDate, null);
                }
            }
             if (dtselect.Checked == false)
            {
                _errorProvider.SetError(dtselect, "Please select 'From Date'");
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtselect, null);
            }
            if (dtSelectToDate.Checked == false)
            {
                _errorProvider.SetError(dtSelectToDate, "Please select 'To Date'");
                isValid = false;
            }
            else
            {
                _errorProvider.SetError(dtSelectToDate, null);
            }
            if (dtselect.Checked.Equals(true) && dtSelectToDate.Checked.Equals(true))
            {
                if (string.IsNullOrWhiteSpace(dtselect.Text))
                {
                    _errorProvider.SetError(dtselect, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "From Date".Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtselect, null);
                }
                if (string.IsNullOrWhiteSpace(dtSelectToDate.Text))
                {
                    _errorProvider.SetError(dtSelectToDate, string.Format(Consts.Messages.BlankIsRequired.GetMessage(), "To Date".Replace(Consts.Common.Colon, string.Empty)));
                    isValid = false;
                }
                else
                {
                    _errorProvider.SetError(dtSelectToDate, null);
                }
            }
            if (dtselect.Checked.Equals(true) && dtSelectToDate.Checked.Equals(true))
            {
                if (!string.IsNullOrEmpty(dtselect.Text) && (!string.IsNullOrEmpty(dtSelectToDate.Text)))
                {
                    if (Convert.ToDateTime(dtselect.Text) > Convert.ToDateTime(dtSelectToDate.Text))
                    {
                        _errorProvider.SetError(dtselect, "'From Date' should be less than or equal to 'End Date'".Replace(Consts.Common.Colon, string.Empty));
                        isValid = false;
                    }
                    else
                    {
                        _errorProvider.SetError(dtselect, null);
                    }
                }
            }

            return (isValid);
        }

        private void NotePadFormat(object sender, FormClosedEventArgs e)
        {
            PdfListForm form = sender as PdfListForm;
            if (form.DialogResult == DialogResult.OK)
            {
                string PdfName = "Pdf File";
                PdfName = form.GetFileName();
                PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
                //PdfName = strFolderPath + PdfName;

                try
                {
                    string Tmpstr = PdfName + ".txt";
                    if (File.Exists(Tmpstr))
                        File.Delete(Tmpstr);
                }
                catch (Exception ex)
                {
                    int length = 8;
                    string newFileName = System.Guid.NewGuid().ToString();
                    newFileName = newFileName.Replace("-", string.Empty);

                    Random_Filename = PdfName + newFileName.Substring(0, length) + ".txt";
                }


                if (!string.IsNullOrEmpty(Random_Filename))
                    PdfName = Random_Filename;
                else
                    PdfName += ".txt";

                FileStream fs = null;


                if (!File.Exists(PdfName))
                {
                    using (fs = File.Create(PdfName))
                    {

                    }
                    string data = string.Empty;
                    using (StreamWriter sw = new StreamWriter(PdfName))
                    {
                        sw.WriteLine("CA Code" + "         " + "Description");
                        sw.WriteLine("***********************************************************************");
                        sw.WriteLine("");

                        string SortCol = null;
                        if (RblEmrServ.Checked == true)
                            SortCol = "Description";

                        DataSet ds = DatabaseLayer.ADMNB001DB.ADMNB001_GetCamast(SortCol, null);
                        DataTable dt = ds.Tables[0];

                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                data = String.Format("{0,-10}", dr["CA_CODE"].ToString().Trim()) + String.Format("{0,8}", " - " + dr["CA_DESC"].ToString());
                                sw.WriteLine(data);

                            }
                        }
                    }

                }

            }
        }

        //private void ExcelFormats(object sender, FormClosedEventArgs e)
        //{

        //    PdfListForm form = sender as PdfListForm;
        //    if (form.DialogResult == DialogResult.OK)
        //    {
        //        string PdfName = "Pdf File";
        //        PdfName = form.GetFileName();
        //        //PdfName = strFolderPath + PdfName;
        //        PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
        //        try
        //        {
        //            if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
        //            { DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim()); }
        //        }
        //        catch (Exception ex)
        //        {
        //            CommonFunctions.MessageBoxDisplay("Error");
        //        }


        //        try
        //        {
        //            string Tmpstr = PdfName + ".xls";
        //            if (File.Exists(Tmpstr))
        //                File.Delete(Tmpstr);
        //        }
        //        catch (Exception ex)
        //        {
        //            int length = 8;
        //            string newFileName = System.Guid.NewGuid().ToString();
        //            newFileName = newFileName.Replace("-", string.Empty);

        //            Random_Filename = PdfName + newFileName.Substring(0, length) + ".xls";
        //        }


        //        if (!string.IsNullOrEmpty(Random_Filename))
        //            PdfName = Random_Filename;
        //        else
        //            PdfName += ".xls";

        //        switch (RepType)
        //        {
        //            case "01": ExcelAgencyTable(PdfName); break;
        //            case "02": break;
        //            case "03": ExcelCaseDemographics(PdfName); break;
        //            case "04": ExcelPerformanceMeasures(PdfName); break;
        //            case "05": ExcelCriticalActivities(PdfName); break;
        //            case "06": ExcelMilestonslist(PdfName); break;
        //            //case "07": ServiceCodeList(document, writer, RblCaseMgmt.Checked); break;
        //            case "08": ExcelZipCode(PdfName); break;
        //            //case "09": HierarchyReport(document, writer, RblCaseMgmt.Checked, strPublicCode); break;
        //            //case "10": FieldControl_Maintenance(document, writer, RblCaseMgmt.Checked, strPublicCode, ((Captain.Common.Utilities.ListItem)CmbAgyTab.SelectedItem).Value.ToString()); break;
        //        }
        //    }
        //}

        private void On_SaveForm_Closed(object sender, FormClosedEventArgs e)
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

                //PdfName = Context.Server.MapPath("~\\Resources\\Pdf\\" + TxtFileName.Text + ".pdf");
                //System.IO.FileStream fs = new FileStream(PdfName, FileMode.Create);

                //Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                Document document;
                document = new Document(PageSize.A4, 30f, 30f, 30f, 50f);
                //document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                document.Open();

                //Font fc = new Font(bfTimes);


                //Image logo = Image.GetInstance(Context.Server.MapPath("~\\Resources\\images\\Captain_Wheel.bmp"));
                //logo.BackgroundColor = BaseColor.WHITE;
                //logo.ScalePercent(30f);

                //Image logo1 = Image.GetInstance(Context.Server.MapPath("~\\Resources\\images\\CapSystems_Title.bmp"));
                //logo1.ScalePercent(50F);
                //logo1.SetAbsolutePosition(70, 779);
                //document.Add(logo);
                //document.Add(logo1);


                //Image _image = iTextSharp.text.Image.GetInstance(Context.Server.MapPath("~\\Resources\\images\\Capsystems_WaterMark.bmp"));
                //if ((RepType == "07" && RblEmrServ.Checked)  )
                //    _image.SetAbsolutePosition(200, 160);
                //else if (RepType == "10" || RepType == "04" || RepType == "03")
                //{
                //}
                //else
                //    _image.SetAbsolutePosition(160, 310);

                //_image.RotationDegrees = 45;
                //_image.Rotate();
                //PdfGState _state = new PdfGState()
                //{
                //    FillOpacity = 0.2F,
                //    StrokeOpacity = 0.2F
                //};

                cb = writer.DirectContent;
                PrintHeaderPage(document, writer);  // For Report Summary

                //document.NewPage();
                if ((RepType == "07" && RblEmrServ.Checked)) // RepType == "03" || || RepType == "04"
                {
                    document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                    document.NewPage();

                }
                else                ///if (RepType != "03" && RepType != "13")
                    document.NewPage();
                Y_Pos = 795;

                //if (RepType == "10" || RepType=="04")
                //{
                //}
                //else
                //{
                //    cb = writer.DirectContentUnder;
                //    cb.SaveState();
                //    cb.SetGState(_state);                               //WaterMark*******
                //    cb.AddImage(_image);
                //    cb.RestoreState();
                //}

                //Users = 0;
                X_Pos = 80;
                cb.BeginText();

                //if (RepType == "05" || RepType == "06" || RepType == "09" || RepType == "04")
                //{

                //}
                //else
                //{
                //    printHText();
                //}

                //      cb.SetColorFill(BaseColor.BLACK);
                cb.SetCMYKColorFill(0, 0, 0, 255);
                cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES).BaseFont, 12);
                //cb.SetFontAndSize(bfTimes, 12);
                cb.EndText();
                FirstRun = true;
                Y_Pos = 780;



                cb = writer.DirectContent;

                DataSet ds = DatabaseLayer.ADMNB001DB.ADMNB001_GetServices();
                DataTable dt = ds.Tables[0];
                //cb.BeginText();


                switch (RepType)
                {
                    case "01": AgencyTableList(document, writer); break;
                    case "02": break;
                    case "03": CaseDemoAssociation(document, writer); break;
                    case "04": PerformanceMeasures(document, writer); break;
                    case "05": CriticalActivitiesList(document, writer); break;
                    case "06": MilestonesList(document, writer); break;
                    case "07": ServiceCodeList(document, writer, RblCaseMgmt.Checked); break;
                    case "08": ZipCodeReport(document, writer); break;
                    case "09": HierarchyReport(document, writer, RblCaseMgmt.Checked, strPublicCode); break;
                    case "10": FieldControl_Maintenance(document, writer, RblCaseMgmt.Checked, strPublicCode, ((Captain.Common.Utilities.ListItem)CmbAgyTab.SelectedItem).Value.ToString()); break;
                    case "11": AccessLogList(document, writer); break;
                    case "12": ReportLogList(document, writer); break;
                    case "13": RNGCaseDemoAssociation(document, writer); break;
                    case "14": RNGPerformanceMeasures(document, writer); break;
                    case "15": RNGServices(document, writer); break;
                    case "16": PrintProgramsandReports(document, writer); break;

                }

                //if (RepType == "05" || RepType == "06" || RepType == "03" || RepType == "09" && RblEmrServ.Checked == false)
                //{
                //}
                //else if (RepType == "10" || RepType == "01" || RepType == "04")
                //{
                //}
                //else
                //{
                //    cb.BeginText();
                //    cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES).BaseFont, 12);
                //    //cb.SetFontAndSize(bfTimes, 12);

                //    if (Y_Pos >= 20)
                //    {
                //        Y_Pos = 07;
                //        X_Pos = 20;
                //        cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES).BaseFont, 12);
                //        //cb.SetFontAndSize(bfTimes, 10);
                //        //          cb.SetColorFill(BaseColor.BLACK);
                //        cb.SetCMYKColorFill(0, 0, 0, 255);
                //        PrintRec(DateTime.Now.ToLocalTime().ToString(), 130);

                //        if ((RepType == "07" && RblEmrServ.Checked)  || RepType == "04")
                //            X_Pos = 790;
                //        else
                //            X_Pos = 550;

                //        PrintRec("Page:", 28);
                //        PrintRec(pageNumber.ToString(), 15);
                //    }

                //    cb.EndText();
                //}
                document.Close();
                AlertBox.Show("Report Generated Successfully");
                fs.Close();
                fs.Dispose();
                pageNumber = 1;
            }
        }

        private void CmbApp_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void CmbRepType_SelectedIndexChanged(object sender, EventArgs e)
        {
            RepType = ((RepListItem)CmbRepType.SelectedItem).Value.ToString();
            pnlFormat.Visible = true;
            RblCaseMgmt.Checked = true;
            pnlAccess.Visible = false;
            pnlRepList.Visible = false;

            if (RepType == "01")
            {
                this.Size = new System.Drawing.Size(435, 228);
                this.CmbAgyTab.Size = new System.Drawing.Size(300, 25);
                CmbRepType.Size = new System.Drawing.Size(300, 25);
                //this.CmbAgyTab.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                //this.HieFilter.Location = new System.Drawing.Point(331, 64);
                this.CmbAgyTab.Location = new System.Drawing.Point(110, 1);
                //this.LblAgyTab.Location = new System.Drawing.Point(21, 111);
                LblAgyTab.Text = "Application";
                lblHie.Text = "Agency Table";

                cmbModule.Visible = true;
                //this.LblAgyTab.Location = new System.Drawing.Point(15, 5);
                //this.lblHie.Location = new System.Drawing.Point(15, 8);
                this.cmbModule.Size = new System.Drawing.Size(300, 25);
                //this.cmbModule.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                //this.HieFilter.Location = new System.Drawing.Point(331, 64);
                this.cmbModule.Location = new System.Drawing.Point(110, 4);//(123, 136);
                this.lblHie.Size = new System.Drawing.Size(76, 16);

                this.CmbAgyTab.Size = new System.Drawing.Size(300, 25);
                //this.CmbAgyTab.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                //this.pictureBox3.Location = new System.Drawing.Point(331, 64);
                //this.CmbAgyTab.Location = new System.Drawing.Point(110, 1);
                //this.LblAgyTab.Location = new System.Drawing.Point(21, 111);

                // this.LblFormat.Location = new System.Drawing.Point(57, 86);
                pnlAgyTbl.Visible = true;
                pnlFormat.Visible = true;
                LblFormat.Enabled = true;
                RblCaseMgmt.Enabled = true;
                RblEmrServ.Enabled = true;
                lblReqFormat.Visible = false;
                // LblAgyTab.Visible = true;//LblAgyTab.Text = "Agency Table";
                RblCaseMgmt.Text = "Code";
                RblCaseMgmt.Size = new System.Drawing.Size(60, 20);
                RblEmrServ.Text = "Description";
                RblEmrServ.Size = new System.Drawing.Size(98, 20);
                RblEmrServ.Location = new System.Drawing.Point(175, 3);
                LblFormat.Text = "Sort By";
                LblAgyTab.Visible = true; CmbAgyTab.Visible = true;
                LblAgyTab.Enabled = true;
                CmbAgyTab.Enabled = true;
                lblReqAgyTable.Visible = false;
                pnlAccessndRepList.Visible = false;
                pnlType.Visible = false;
                pnlHiendMod.Visible = true;
                HieFilter.Visible = false;
                rbTown.Visible = false;
                rbCounty.Visible = false;
                txtHie.Visible = false;
                lblHie.Visible = true;
                LblFormat.Visible = true;
                RblCaseMgmt.Visible = true;
                RblEmrServ.Visible = true;
                lblReqHie.Visible = false;



                PopulateDropDowns();
            }
            else if (RepType == "07")
            {
                LblFormat.Text = "Format";
                RblCaseMgmt.Text = "Case Management";
                RblEmrServ.Text = "Emergency Services"; // adding
                this.LblFormat.Location = new System.Drawing.Point(57, 86);
                LblFormat.Enabled = true;
                lblReqFormat.Visible = false;
                RblCaseMgmt.Enabled = true;
                RblEmrServ.Enabled = true;
                LblAgyTab.Enabled = false;
                CmbAgyTab.Visible = false;
                LblAgyTab.Visible = false;
                lblReqAgyTable.Visible = false;
                lblHie.Visible = false;
                txtHie.Visible = false;
                HieFilter.Visible = false;
                lblType.Visible = false;
                pnlType.Visible = false;
                lblReqHie.Visible = false;

                pnlFormat.Visible = true;
                LblFormat.Visible = true;
                RblCaseMgmt.Visible = true;
                RblEmrServ.Visible = true;
                rbCounty.Visible = false;
                rbTown.Visible = false;
                cmbModule.Visible = false;
            }
            else if (RepType == "08")
            {
                this.Size = new System.Drawing.Size(435, 162);
                CmbRepType.Size = new System.Drawing.Size(300, 25);
                RblCaseMgmt.Size = new System.Drawing.Size(80, 20);
                LblFormat.Text = "Sort By";
                RblCaseMgmt.Text = "ZIP Code";
                RblEmrServ.Text = "City";
                RblEmrServ.Size = new System.Drawing.Size(55, 20);
                RblEmrServ.Location = new System.Drawing.Point(193, 3);
                rbTown.Location = new System.Drawing.Point(253, 3);
                rbCounty.Location = new System.Drawing.Point(345, 3);
                //this.LblFormat.Location = new System.Drawing.Point(57, 86);
                lblReqFormat.Visible = false;
                LblFormat.Enabled = true;
                RblCaseMgmt.Enabled = true;
                RblEmrServ.Enabled = true;
                LblAgyTab.Enabled = false;
                CmbAgyTab.Visible = false;
                LblAgyTab.Visible = false;
                lblReqAgyTable.Visible = false;
                lblHie.Visible = false;
                txtHie.Visible = false;
                HieFilter.Visible = false;
                lblType.Visible = false;
                pnlType.Visible = false;
                lblReqHie.Visible = false;
                pnlAccessndRepList.Visible = false;
                pnlFormat.Visible = true;
                LblFormat.Visible = true;
                RblCaseMgmt.Visible = true;
                RblEmrServ.Visible = true;
                rbTown.Visible = true;
                rbCounty.Visible = true;
                cmbModule.Visible = false;
            }
            else if (RepType == "09")
            {
                LblFormat.Text = "Format";
                RblCaseMgmt.Text = "Hierarchy Summary";
                RblEmrServ.Text = "Program Details";
                this.LblFormat.Location = new System.Drawing.Point(57, 86);
                LblFormat.Enabled = true;
                HieFilter.Enabled = true;
                HieFilter.Visible = true;
                lblReqFormat.Visible = false;
                RblCaseMgmt.Enabled = true;
                RblEmrServ.Enabled = true;
                LblAgyTab.Enabled = false;
                CmbAgyTab.Visible = false;
                LblAgyTab.Visible = false;
                lblReqAgyTable.Visible = false;
                lblHie.Visible = false;
                txtHie.Visible = false;
                lblType.Visible = false;
                pnlType.Visible = false;
                lblReqHie.Visible = false;

                pnlFormat.Visible = true;
                LblFormat.Visible = true;
                RblCaseMgmt.Visible = true;
                RblEmrServ.Visible = true;
                rbTown.Visible = false;
                rbCounty.Visible = false;
                cmbModule.Visible = false;
            }
            else if (RepType == "05")
            {
                this.Size = new System.Drawing.Size(400, 164);
                CmbRepType.Size = new System.Drawing.Size(265, 25);
                RblCaseMgmt.Size = new System.Drawing.Size(60, 20);
                RblEmrServ.Size = new System.Drawing.Size(98, 20);
                RblEmrServ.Location = new System.Drawing.Point(175, 3);
                LblFormat.Text = "Sort By";
                RblCaseMgmt.Text = "Code";
                RblEmrServ.Text = "Description";
                this.LblFormat.Location = new System.Drawing.Point(15, 7);
                lblReqFormat.Visible = false;
                LblFormat.Enabled = true;
                RblCaseMgmt.Enabled = true;
                RblEmrServ.Enabled = true;
                LblAgyTab.Enabled = false;
                CmbAgyTab.Visible = false;
                LblAgyTab.Visible = false;
                lblReqAgyTable.Visible = false;
                lblHie.Visible = false;
                txtHie.Visible = false;
                HieFilter.Visible = false;
                lblType.Visible = false;
                pnlType.Visible = false;
                lblReqHie.Visible = false;
                pnlAccessndRepList.Visible = false;
                pnlFormat.Visible = true;
                LblFormat.Visible = true;
                RblCaseMgmt.Visible = true;
                RblEmrServ.Visible = true;
                rbTown.Visible = false;
                rbCounty.Visible = false;
                cmbModule.Visible = false;
            }
            else if (RepType == "06")
            {
                this.Size = new System.Drawing.Size(400, 164);
                CmbRepType.Size = new System.Drawing.Size(265, 25);
                RblCaseMgmt.Size = new System.Drawing.Size(60, 20);
                RblEmrServ.Size = new System.Drawing.Size(98, 20);
                RblEmrServ.Location = new System.Drawing.Point(175, 3);
                LblFormat.Text = "Sort By";
                RblCaseMgmt.Text = "Code";
                RblEmrServ.Text = "Description";
                this.LblFormat.Location = new System.Drawing.Point(15, 7);
                lblReqFormat.Visible = false;
                LblFormat.Enabled = true;
                RblCaseMgmt.Enabled = true;
                RblEmrServ.Enabled = true;
                LblAgyTab.Enabled = false;

                CmbAgyTab.Visible = false;

                LblAgyTab.Visible = false;

                lblReqAgyTable.Visible = false;
                lblHie.Visible = false;
                txtHie.Visible = false;
                HieFilter.Visible = false;
                lblType.Visible = false;
                pnlType.Visible = false;
                lblReqHie.Visible = false;
                pnlAccessndRepList.Visible = false;
                pnlFormat.Visible = true;
                LblFormat.Visible = true;
                RblCaseMgmt.Visible = true;
                RblEmrServ.Visible = true;
                rbTown.Visible = false;
                rbCounty.Visible = false;
                cmbModule.Visible = false;
            }
            else if (RepType == "10")
            {
                this.Size = new System.Drawing.Size(435, 256);
                RblCaseMgmt.Size = new System.Drawing.Size(60, 20);
                RblEmrServ.Size = new System.Drawing.Size(98, 20);
                RblEmrServ.Location = new System.Drawing.Point(175, 3);
                LblAgyTab.Enabled = true;
                CmbAgyTab.Enabled = true;
                //this.CmbAgyTab.Location = new System.Drawing.Point(123, 108);
                //this.LblAgyTab.Location = new System.Drawing.Point(21, 111);
                this.CmbRepType.Size = new System.Drawing.Size(300, 25);
                this.CmbAgyTab.Size = new System.Drawing.Size(300, 25);
                //this.LblFormat.Location = new System.Drawing.Point(57, 86);
                //this.CmbAgyTab.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                CmbAgyTab.Visible = true;
                LblFormat.Enabled = true;
                RblCaseMgmt.Enabled = true;
                RblEmrServ.Enabled = true;
                lblReqFormat.Visible = false;
                lblReqAgyTable.Visible = false;
                LblAgyTab.Visible = true;
                // this.HieFilter.Location = new System.Drawing.Point(205, 136);
                HieFilter.Visible = true;
                LblAgyTab.Text = "Screen Name";
                RblCaseMgmt.Text = "Code";
                RblEmrServ.Text = "Description";
                LblFormat.Text = "Sort By";
                lblHie.Visible = true;
                txtHie.Visible = true;
                lblHie.Text = "Hierarchy";
                // this.lblHie.Location = new System.Drawing.Point(44, 136);
                lblHie.Enabled = true;
                lblType.Visible = true;
                pnlType.Visible = true;
                lblReqHie.Visible = true;
                pnlAccessndRepList.Visible = false;
                pnlFormat.Visible = true;
                LblFormat.Visible = true;
                RblCaseMgmt.Visible = true;
                RblEmrServ.Visible = true;
                rbTown.Visible = false;
                rbCounty.Visible = false;
                cmbModule.Visible = false;
                pnlAgyTbl.Visible = true;
                pnlHiendMod.Visible = true;

                PopulateDropDowns();
            }
            else if (RepType == "04")
            {
                this.Size = new System.Drawing.Size(400, 167);
                LblAgyTab.Enabled = true;
                pnlAgyTbl.Visible = true;
                pnlFormat.Visible = false;
                CmbRepType.Size = new System.Drawing.Size(265, 25);
                this.CmbAgyTab.Size = new System.Drawing.Size(265, 25);
                this.CmbAgyTab.Location = new System.Drawing.Point(110, 1);
                //this.CmbAgyTab.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                CmbAgyTab.Visible = true;
                CmbAgyTab.Enabled = true;
                LblAgyTab.Text = "Date Range";
                this.LblAgyTab.Location = new System.Drawing.Point(15, 5);
                LblFormat.Enabled = false;
                RblCaseMgmt.Enabled = false;
                RblEmrServ.Enabled = false;
                LblFormat.Text = "Format";
                //RblCaseMgmt.Text = "Case Management";
                //RblEmrServ.Text = "Emergency Services";
                lblReqAgyTable.Visible = false;
                LblAgyTab.Visible = true;
                lblType.Visible = false;
                pnlType.Visible = false;
                lblReqHie.Visible = false;
                lblReqFormat.Visible = false;
                lblHie.Visible = false;
                txtHie.Visible = false;
                HieFilter.Visible = false;
                pnlAccessndRepList.Visible = false;
                LblFormat.Visible = false;
                RblCaseMgmt.Visible = false;
                RblEmrServ.Visible = false;
                rbTown.Visible = false;
                rbCounty.Visible = false;
                cmbModule.Visible = false;

                PopulateDropDowns();

            }
            else if (RepType == "14")
            {
                LblAgyTab.Enabled = true;
                pnlFormat.Visible = false;
                this.Size = new System.Drawing.Size(400, 167);
                CmbRepType.Size = new System.Drawing.Size(265, 25);
                this.CmbAgyTab.Size = new System.Drawing.Size(265, 25);
                this.CmbAgyTab.Location = new System.Drawing.Point(110, 1);
                //this.CmbAgyTab.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                CmbAgyTab.Visible = true;
                CmbAgyTab.Enabled = true;
                LblAgyTab.Text = "Date Range";
                this.LblAgyTab.Location = new System.Drawing.Point(15, 5);
                LblFormat.Enabled = false;
                RblCaseMgmt.Enabled = false;
                RblEmrServ.Enabled = false;
                LblFormat.Text = "Format";
                //RblCaseMgmt.Text = "Case Management";
                //RblEmrServ.Text = "Emergency Services";
                lblReqAgyTable.Visible = false;
                LblAgyTab.Visible = true;
                lblType.Visible = false;
                pnlType.Visible = false;
                lblReqHie.Visible = false;
                lblReqFormat.Visible = false;
                lblHie.Visible = false;
                txtHie.Visible = false;
                HieFilter.Visible = false;
                pnlAccessndRepList.Visible = false;
                pnlAgyTbl.Visible = true;
                LblFormat.Visible = false;
                RblCaseMgmt.Visible = false;
                RblEmrServ.Visible = false;
                rbTown.Visible = false;
                rbCounty.Visible = false;
                cmbModule.Visible = false;

                PopulateDropDowns();

            }
            else if (RepType == "15")
            {
                LblAgyTab.Enabled = true;
                pnlFormat.Visible = false;
                this.Size = new System.Drawing.Size(400, 167);
                CmbRepType.Size = new System.Drawing.Size(265, 25);
                this.CmbAgyTab.Size = new System.Drawing.Size(265, 25);
                this.CmbAgyTab.Location = new System.Drawing.Point(110, 1);
                // this.CmbAgyTab.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                CmbAgyTab.Visible = true;
                CmbAgyTab.Enabled = true;
                LblAgyTab.Text = "Date Range";
                this.LblAgyTab.Location = new System.Drawing.Point(15, 5);
                LblFormat.Enabled = false;
                RblCaseMgmt.Enabled = false;
                RblEmrServ.Enabled = false;
                LblFormat.Text = "Format";
                //RblCaseMgmt.Text = "Case Management";
                //RblEmrServ.Text = "Emergency Services";
                lblReqAgyTable.Visible = false;
                LblAgyTab.Visible = true;
                lblType.Visible = false;
                pnlType.Visible = false;
                lblReqHie.Visible = false;
                lblReqFormat.Visible = false;
                lblHie.Visible = false;
                txtHie.Visible = false;
                HieFilter.Visible = false;
                pnlAccessndRepList.Visible = false;
                pnlAgyTbl.Visible = true;
                LblFormat.Visible = false;
                RblCaseMgmt.Visible = false;
                RblEmrServ.Visible = false;
                rbTown.Visible = false;
                rbCounty.Visible = false;
                cmbModule.Visible = false;

                PopulateDropDowns();

            }
            else if (RepType == "11")
            {
                this.Size = new System.Drawing.Size(555, 202);
                CmbRepType.Size = new System.Drawing.Size(421, 25);
                CmbUser.Size = new System.Drawing.Size(421, 25);
                pnlAccessndRepList.Visible = true;
                pnlAccess.Visible = true;
                rdoAll.Checked = true;
                //pnlAccess.Location = new System.Drawing.Point(47, 90);
                LblAgyTab.Visible = false;
                CmbAgyTab.Visible = false;
                CmbAgyTab.Visible = false;
                pnlRepList.Visible = false;
                pnlFormat.Visible = false;
                pnlAgyTbl.Visible = false;
                pnlHiendMod.Visible = false;
                pnlType.Visible = false;
                // LblAgyTab.Text = "Date Range";
                // this.LblAgyTab.Location = new System.Drawing.Point(15, 5);
                // LblFormat.Visible = false;
                // RblCaseMgmt.Visible = false;
                // RblEmrServ.Visible = false;
                // LblFormat.Text = "Format";
                // RblCaseMgmt.Text = "Case Management";
                // RblEmrServ.Text = "Emergency Services";
                // lblReqAgyTable.Visible = false;
                // LblAgyTab.Visible = false;
                // lblType.Visible = false;
                // pnlType.Visible = false;
                // lblReqHie.Visible = false;
                // lblReqFormat.Visible = false;
                // lblHie.Visible = false;
                // txtHie.Visible = false;
                // HieFilter.Visible = false;

                // pnlFormat.Visible = false;
                // LblFormat.Visible = false;
                // RblCaseMgmt.Visible = false;
                // RblEmrServ.Visible = false;
                // rbTown.Visible = false;
                // rbCounty.Visible = false;
                // cmbModule.Visible = false;
                // PopulateDropDowns();

            }
            else if (RepType == "12")
            {
                this.Size = new System.Drawing.Size(420, 265);
                CmbRepType.Size = new System.Drawing.Size(284, 25);
                cmbRepUserId.Size = new System.Drawing.Size(284, 25);
                cmbRepModule.Size = new System.Drawing.Size(284, 25);
                cmbRepCode.Size = new System.Drawing.Size(284, 25);
                pnlAccessndRepList.Visible = true;
                pnlRepList.Visible = true;
                //pnlRepList.Location = new System.Drawing.Point(25, 82);
                pnlAccess.Visible = false;
                pnlFormat.Visible = false;
                pnlAgyTbl.Visible = false;
                pnlHiendMod.Visible = false;
                pnlType.Visible = false;
                //this.LblAgyTab.Location = new System.Drawing.Point(15, 1);
                //LblAgyTab.Visible = false;
                //CmbAgyTab.Visible = false;
                //CmbAgyTab.Visible = false;
                //LblAgyTab.Text = "Date Range";
                //LblFormat.Visible = false;
                //RblCaseMgmt.Visible = false;
                //RblEmrServ.Visible = false;
                //LblFormat.Text = "Format";
                //RblCaseMgmt.Text = "Case Management";
                //RblEmrServ.Text = "Emergency Services";
                //lblReqAgyTable.Visible = false;
                //LblAgyTab.Visible = false;
                //lblType.Visible = false;
                //pnlType.Visible = false;
                //lblReqHie.Visible = false;
                //lblReqFormat.Visible = false;
                //lblHie.Visible = false;
                //txtHie.Visible = false;
                //HieFilter.Visible = false;

                //pnlFormat.Visible = false;
                //LblFormat.Visible = false;
                //RblCaseMgmt.Visible = false;
                //RblEmrServ.Visible = false;
                //rbTown.Visible = false;
                //rbCounty.Visible = false;
                //cmbModule.Visible = false;
                // PopulateDropDowns();

            }
            else if (RepType == "16")
            {
                this.Size = new System.Drawing.Size(400, 197);
                CmbRepType.Size = new System.Drawing.Size(265, 25);
                this.CmbAgyTab.Size = new System.Drawing.Size(265, 25);
                RblCaseMgmt.Size = new System.Drawing.Size(85, 20);
                RblEmrServ.Size = new System.Drawing.Size(75, 20);
                RblEmrServ.Location = new System.Drawing.Point(193, 3);
                // this.CmbAgyTab.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                //this.HieFilter.Location = new System.Drawing.Point(331, 64);
                this.CmbAgyTab.Location = new System.Drawing.Point(110, 1);
                this.LblAgyTab.Location = new System.Drawing.Point(15, 5);
                this.LblFormat.Location = new System.Drawing.Point(15, 7);
                //this.LblFormat.TextAlign = System.Drawing.ContentAlignment.TopRight;
                pnlAgyTbl.Visible = true;
                pnlFormat.Visible = true;
                LblAgyTab.Enabled = true;
                CmbAgyTab.Enabled = true;
                CmbAgyTab.Visible = true;
                LblFormat.Enabled = true;
                RblCaseMgmt.Enabled = true;
                RblEmrServ.Enabled = true;
                lblReqFormat.Visible = false;
                lblReqAgyTable.Visible = false;
                LblAgyTab.Visible = true;
                HieFilter.Visible = false;
                LblAgyTab.Text = "Application";
                RblCaseMgmt.Text = "Programs";
                RblEmrServ.Text = "Reports";
                LblFormat.Text = "Type";
                lblHie.Visible = false;
                txtHie.Visible = false;
                lblType.Visible = false;
                pnlType.Visible = false;
                lblReqHie.Visible = false;
                LblFormat.Visible = true;
                RblCaseMgmt.Visible = true;
                RblEmrServ.Visible = true;
                rbTown.Visible = false;
                rbCounty.Visible = false;
                cmbModule.Visible = false;
                pnlAccessndRepList.Visible = false;
                PopulateDropDowns();
            }
            else
            {
                this.Size = new System.Drawing.Size(400, 137);
                CmbRepType.Size = new System.Drawing.Size(265, 25);
                pnlAccessndRepList.Visible = false;
                pnlFormat.Visible = false;
                pnlAgyTbl.Visible = false;
                pnlHiendMod.Visible = false;
                pnlType.Visible = false;
                pnlReportType.Visible = true;
                //LblAgyTab.Enabled = false;
                //CmbAgyTab.Visible = false;

                //LblFormat.Enabled = false;
                //RblCaseMgmt.Enabled = false;
                //RblEmrServ.Enabled = false;
                //LblFormat.Text = "Format";
                //RblCaseMgmt.Text = "Case Management";
                //RblEmrServ.Text = "Emergency Services";
                //lblReqAgyTable.Visible = false;
                //LblAgyTab.Visible = false;
                //lblType.Visible = false;
                //pnlType.Visible = false;
                //lblReqHie.Visible = false;
                //lblReqFormat.Visible = false;
                //lblHie.Visible = false;
                //txtHie.Visible = false;
                //HieFilter.Visible = false;

                //pnlFormat.Visible = false;
                //LblFormat.Visible = false;
                //RblCaseMgmt.Visible = false;
                //RblEmrServ.Visible = false;
                //rbTown.Visible = false;
                //rbCounty.Visible = false;
                //cmbModule.Visible = false;
            }
            // else
            //{
            //    //LblAgyTab.Enabled = false;
            //    //CmbAgyTab.Enabled = false;
            //    //LblSeq.Enabled = false;
            //    //CmbSeq.Enabled = false;
            //    LblFormat.Enabled = false;


            //    RblCaseMgmt.Enabled = false;
            //    RblEmrServ.Enabled = false;
            //}

        }

        private void PrintRec(string PrintText, int StrWidth)
        {

            cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES).BaseFont, 12);//cb.SetFontAndSize(bfTimes, 10);
            cb.ShowTextAligned(800, PrintText, X_Pos, Y_Pos, 0);
            X_Pos += StrWidth;
            PrintText = null;
        }

        private void PrintHeader()
        {
            Y_Pos = 790;
            X_Pos = 50;
            //70

            switch (RepType)
            {
                case "01": Print_Agytab_List_Header(); break;
                case "02": break;
                case "03": CaseDemoAssociation(); break;
                case "04": PerformanceMeasureHeader(); break;
                case "05": break;
                case "06": break;
                case "07": Print_Agytab_List_Header(RblCaseMgmt.Checked); break;
                case "08": ZipCodeHeader(); break;
                case "09": break;  //ProgramDetailsHeader();
            }
        }

        private void PrintHeaderRec(string PrintText, int StrWidth)
        {
            cb.SetFontAndSize(FontFactory.GetFont(FontFactory.COURIER_BOLDOBLIQUE).BaseFont, 12);
            cb.SetColorFill(BaseColor.GRAY);

            //Font ft = BaseColor.GRAY;
            cb.ShowTextAligned(100, PrintText, X_Pos, Y_Pos, 0);

            X_Pos += StrWidth;
            //     cb.SetColorFill(BaseColor.BLACK);
            cb.SetCMYKColorFill(0, 0, 0, 255);

        }

        private void CheckBottomBorderReached(Document document, PdfWriter writer)
        {

            //Font _font = new Font(bfTimes, 45, 1, BaseColor.BLACK);
            Image _image = iTextSharp.text.Image.GetInstance(Application.MapPath("~\\Resources\\images\\Capsystems_WaterMark.bmp"));
            _image.SetAbsolutePosition(160, 310);
            ////_image.SetAbsolutePosition(140, 260);

            _image.RotationDegrees = 45;
            _image.Rotate();
            PdfGState _state = new PdfGState()
            {
                FillOpacity = 0.3F,
                StrokeOpacity = 0.5F
            };



            //cb.EndText();

            //cb = writer.DirectContentUnder;
            //cb.SaveState();
            //cb.SetGState(_state);                               //WaterMark*******
            //cb.AddImage(_image);
            //cb.RestoreState();

            //cb.BeginText();
            if (Y_Pos <= 20)
            {

                cb.EndText();

                cb.BeginText();
                Y_Pos = 07;
                X_Pos = 20;
                cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES).BaseFont, 12);
                //cb.SetFontAndSize(bfTimes, 10);
                //       cb.SetColorFill(BaseColor.BLACK);
                cb.SetCMYKColorFill(0, 0, 0, 255);
                PrintRec(DateTime.Now.ToLocalTime().ToString(), 130);
                Y_Pos = 07;
                Y_Pos = 07;
                X_Pos = 550;
                PrintRec("Page:", 28);
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
                cb = writer.DirectContentUnder;
                cb.SaveState();
                cb.SetGState(_state);                               //WaterMark*******
                cb.AddImage(_image);
                cb.RestoreState();

                cb.BeginText();
                cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES).BaseFont, 12);
                //cb.SetFontAndSize(bfTimes, 10);
                cb.SetColorFill(BaseColor.LIGHT_GRAY);

                printHText();


                PrintHeader();

                //            cb.SetColorFill(BaseColor.BLACK);
                cb.SetCMYKColorFill(0, 0, 0, 230);
                X_Pos = 50;
                Y_Pos -= 5;
                //CheckBottomBorderReached(document, writer);
                cb.EndText();
                if (RepType == "06" || RepType == "09")
                {
                }
                else
                {
                    Print_Line();
                }
                Y_Pos = 770;
                X_Pos = 55;                                                           //modified
                //cb.SetFontAndSize(bfTimes, 10);
                cb.BeginText();
                //X_Pos = 80;

            }
        }

        //Begining of header Horizontal Line 

        private void Print_Line()
        {
            cb.SetLineWidth(0.7f);
            cb.MoveTo(X_Pos, Y_Pos);
            if ((RepType == "07" && RblEmrServ.Checked) || RepType == "03" || RepType == "04" || RepType == "13" || RepType == "14" || RepType == "15")
                cb.LineTo(800, Y_Pos);
            else
                cb.LineTo(585, Y_Pos);

            cb.Stroke();
        }

        //End of header Horizontal Line 



        //Beginibg of Report Printing Ligic For All Formats

        // Agency Table List Print Logic
        private void AgencyTableList(Document document, PdfWriter writer)
        {
            document.SetPageSize(new iTextSharp.text.Rectangle(iTextSharp.text.PageSize.A4.Width, iTextSharp.text.PageSize.A4.Height));
            try
            {
                DataTable dt = new DataTable();
                if (dtModules.Rows.Count > 0)
                {
                    DataSet ds = DatabaseLayer.ADMNB001DB.ADMNB001_GetaAgyTabList(((RepListItem)cmbModule.SelectedItem).Value.ToString());
                    dt = ds.Tables[0];
                }
                //if (dt.Rows.Count > 0)
                //{
                //    DataView dvMod = new DataView(dt);
                //    if (((Captain.Common.Utilities.ListItem)cmbModule.SelectedItem).Value.ToString() != "**")
                //    {
                //        dvMod.RowFilter = "AGY_8 LIKE '% " + ((Captain.Common.Utilities.ListItem)cmbModule.SelectedItem).Value.ToString() + "%'";
                //        dt = dvMod.ToTable();
                //    }
                //}

                //New Font Calibri applied to all data
                BaseFont bf_Calibri = BaseFont.CreateFont("c:/windows/fonts/calibri.ttf", BaseFont.WINANSI, BaseFont.EMBEDDED);
                iTextSharp.text.Font Calibri = new iTextSharp.text.Font(bf_Calibri, 10);
                iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_Calibri, 8);
                iTextSharp.text.Font SubHeadFont = new iTextSharp.text.Font(bf_Calibri, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_Calibri, 9, 3);
                iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_Calibri, 8, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
                iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_Calibri, 9, 2);
                iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_Calibri, 9, 4);

                //BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
                //iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 10);
                //iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 10);
                //iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 9, 3);
                //iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(1, 10, 1);
                //iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 9, 2);
                //iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 9, 4);

                //cb.BeginText();
                //cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES).BaseFont, 12);
                //cb.SetFontAndSize(bfTimes, 10);


                string Agy_Code_Cell = string.Empty;
                string Agy_Desc_Cell = string.Empty;
                string PrivAgyType = string.Empty;

                //PrintHeader();

                ////        cb.SetColorFill(BaseColor.BLACK);
                //cb.SetCMYKColorFill(0, 0, 0, 255);
                //X_Pos = 50;
                //Y_Pos -= 5;
                //CheckBottomBorderReached(document, writer);
                //cb.EndText();
                //Print_Line();

                //cb.BeginText();
                //cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES).BaseFont, 12);
                ////cb.SetFontAndSize(bfTimes, 12);
                ////        cb.SetColorFill(BaseColor.BLACK);
                //cb.SetCMYKColorFill(0, 0, 0, 255);
                ////cb.ShowTextAligned(Element.ALIGN_MIDDLE, "Agency Table List", 250, 820, 0);
                if (dt.Rows.Count > 0)
                {
                    PdfPTable table = new PdfPTable(7);
                    table.TotalWidth = 560f;
                    table.WidthPercentage = 100;
                    table.LockedWidth = true;
                    float[] widths = new float[] { 15f, 18f, 90f, 10f, 10f, 15f, 15f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                    table.SetWidths(widths);
                    table.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.HeaderRows = 1;

                    string[] Header = { "Table#", "Code", "Description", "Cell1", "Cell2", "Default", "Inactive" };
                    for (int i = 0; i < Header.Length; ++i)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(Header[i], TblFontBold));
                        if (i == 2 || i==0)
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        else
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.FixedHeight = 15f;
                        //cell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                        cell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));   // Column Header background
                        cell.BorderColor = BaseColor.WHITE;
                        table.AddCell(cell);
                    }

                    DataTable dtCodes = new DataTable();

                    dtCodes = dtModules;
                    DataView dvAgyCodes = new DataView(dtCodes);
                    if (((RepListItem)cmbModule.SelectedItem).Value.ToString() != "*****")
                    {
                        dvAgyCodes.RowFilter = "AGY_TYPE= '" + ((RepListItem)cmbModule.SelectedItem).Value.ToString() + "'";
                    }

                    //dvAgyCodes.RowFilter = "AGY_CODE='00000'";
                    if (RblEmrServ.Checked)
                        dvAgyCodes.Sort = "AGY_DESC";
                    else if (!RblEmrServ.Checked) dvAgyCodes.Sort = "AGY_TYPE";
                    dtCodes = dvAgyCodes.ToTable();

                    //}
                    //else
                    //{
                    //    DataView dvAgyCodes = new DataView(dt);
                    //    dvAgyCodes.RowFilter = "AGY_CODE='00000'";
                    //    if (RblEmrServ.Checked)
                    //        dvAgyCodes.Sort = "AGY_DESC";
                    //    else if (!RblEmrServ.Checked) dvAgyCodes.Sort = "AGY_CODE";
                    //    dtCodes = dvAgyCodes.ToTable();
                    //}


                    foreach (DataRow dr in dtCodes.Rows)
                    {

                        int number;
                        if (dr["AGY_CODE"].ToString() == "00000")
                        {

                            if (string.IsNullOrEmpty(dr["AGY_ACTIVE"].ToString()) || string.IsNullOrEmpty(dr["AGY_DEFAULT"].ToString()) ||
                              (!(int.TryParse(dr["AGY_ACTIVE"].ToString(), out number))) || (!(int.TryParse(dr["AGY_DEFAULT"].ToString(), out number))))
                            {
                                PrivAgyType = dr["AGY_TYPE"].ToString(); goto NextAgyType;
                            }
                        }
                        //X_Pos = 50;

                        //CheckBottomBorderReached(document, writer);   Test and Remove
                        if (dr["AGY_TYPE"].ToString() != PrivAgyType)
                        {
                            //Y_Pos -= 13;
                            //CheckBottomBorderReached(document, writer);
                            PdfPCell Agy_Type = new PdfPCell(new Phrase(dr["AGY_TYPE"].ToString().Trim(), SubHeadFont));
                            Agy_Type.HorizontalAlignment = Element.ALIGN_LEFT;
                            Agy_Type.Colspan = 2;
                            //Agy_Type.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            Agy_Type.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#a5c8e1"));       // Column Sub-Headings Background
                            Agy_Type.BorderColor = BaseColor.WHITE;
                            Agy_Type.FixedHeight = 15;
                            table.AddCell(Agy_Type);

                            PdfPCell Desc = new PdfPCell(new Phrase(dr["AGY_DESC"].ToString().Trim(), SubHeadFont));
                            Desc.HorizontalAlignment = Element.ALIGN_LEFT;
                            //Desc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            Desc.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#a5c8e1"));
                            Desc.BorderColor = BaseColor.WHITE;
                            Desc.FixedHeight = 15;
                            table.AddCell(Desc);

                            PdfPCell ACTIVE = new PdfPCell(new Phrase(dr["AGY_ACTIVE"].ToString().Trim(), SubHeadFont));
                            ACTIVE.HorizontalAlignment = Element.ALIGN_RIGHT;
                            //ACTIVE.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            ACTIVE.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#a5c8e1"));
                            ACTIVE.BorderColor = BaseColor.WHITE;
                            ACTIVE.FixedHeight = 15;
                            table.AddCell(ACTIVE);

                            PdfPCell DEFAULT = new PdfPCell(new Phrase(dr["AGY_DEFAULT"].ToString().Trim(), SubHeadFont));
                            DEFAULT.HorizontalAlignment = Element.ALIGN_RIGHT;
                            //DEFAULT.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            DEFAULT.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#a5c8e1"));
                            DEFAULT.BorderColor = BaseColor.WHITE;
                            DEFAULT.FixedHeight = 15;
                            table.AddCell(DEFAULT);

                            PdfPCell Space4 = new PdfPCell(new Phrase("", SubHeadFont));
                            Space4.HorizontalAlignment = Element.ALIGN_RIGHT;
                            Space4.Colspan = 2;
                            //Space4.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            Space4.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#a5c8e1"));
                            Space4.BorderColor = BaseColor.WHITE;
                            Space4.FixedHeight = 15;
                            table.AddCell(Space4);

                            //PrintRec(dr["AGY_TYPE"].ToString(), 100);
                            //PrintRec(dr["AGY_DESC"].ToString(), 210);
                            //PrintRec(dr["AGY_ACTIVE"].ToString(), 60);
                            //PrintRec(dr["AGY_DEFAULT"].ToString(), 30);

                            Agy_Code_Cell = "AGY_" + dr["AGY_ACTIVE"].ToString();
                            Agy_Desc_Cell = "AGY_" + dr["AGY_DEFAULT"].ToString();

                            //Y_Pos -= 13;
                            //X_Pos = 100;
                            //CheckBottomBorderReached(document, writer);

                            PrivAgyType = dr["AGY_TYPE"].ToString();
                            //                cb.SetColorFill(BaseColor.BLACK);
                            //cb.SetCMYKColorFill(0, 0, 0, 255);
                        }
                        DataView dv = new DataView(dt);
                        dv.RowFilter = "AGY_TYPE=" + "'" + PrivAgyType + "'";
                        DataTable dtDetail = dv.ToTable();

                        if (dtDetail.Rows.Count > 0)
                        {
                            foreach (DataRow dr1 in dtDetail.Rows)
                            {
                                if (dr1["AGY_TYPE"].ToString() == PrivAgyType && dr1["AGY_CODE"].ToString() != "00000")
                                {
                                    //X_Pos = 115;
                                    PdfPCell Space = new PdfPCell(new Phrase("", TableFont));
                                    Space.HorizontalAlignment = Element.ALIGN_LEFT;
                                    //Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    Space.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                    Space.BorderColor = BaseColor.WHITE;
                                    Space.FixedHeight = 15;
                                    table.AddCell(Space);

                                    if (!string.IsNullOrEmpty(Agy_Code_Cell.Trim()))
                                    {
                                        PdfPCell Code = new PdfPCell(new Phrase(dr1[Agy_Code_Cell].ToString().Trim(), TableFont));
                                        Code.HorizontalAlignment = Element.ALIGN_CENTER;
                                        //Code.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        Code.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));   //Column Left Rows Background
                                        Code.BorderColor = BaseColor.WHITE;
                                        Code.FixedHeight = 15f;
                                        table.AddCell(Code);
                                    }
                                    else
                                    {
                                        PdfPCell Code = new PdfPCell(new Phrase("", TableFont));
                                        Code.HorizontalAlignment = Element.ALIGN_CENTER;
                                        //Code.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        Code.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                        Code.BorderColor = BaseColor.WHITE;
                                        Code.FixedHeight = 15f;
                                        table.AddCell(Code);
                                    }

                                    if (!string.IsNullOrEmpty(Agy_Desc_Cell.Trim()))
                                    {
                                        PdfPCell Agy_Desc = new PdfPCell(new Phrase(dr1[Agy_Desc_Cell].ToString().Trim(), TableFont));
                                        Agy_Desc.HorizontalAlignment = Element.ALIGN_LEFT;
                                        //Agy_Desc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        Agy_Desc.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));       //Column Middle Rows Background
                                        Agy_Desc.BorderColor = BaseColor.WHITE;
                                        Agy_Desc.FixedHeight = 15f;
                                        table.AddCell(Agy_Desc);
                                    }
                                    else
                                    {
                                        PdfPCell Agy_Desc = new PdfPCell(new Phrase("", TableFont));
                                        Agy_Desc.HorizontalAlignment = Element.ALIGN_LEFT;
                                        //Agy_Desc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        Agy_Desc.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                                        Agy_Desc.BorderColor = BaseColor.WHITE;
                                        Agy_Desc.FixedHeight = 15f;
                                        table.AddCell(Agy_Desc);
                                    }

                                    PdfPCell Space1 = new PdfPCell(new Phrase("", TableFont));
                                    Space1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Space1.Colspan = 2;
                                    //Space1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    Space1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));     //Column Right Rows Background
                                    Space1.BorderColor = BaseColor.WHITE;
                                    Space1.FixedHeight = 15f;
                                    table.AddCell(Space1);

                                    if (dr1["AGY_DEFAULT"].ToString() == "Y")
                                    {
                                        PdfPCell Def = new PdfPCell(new Phrase("Y", TableFont));
                                        Def.HorizontalAlignment = Element.ALIGN_CENTER;
                                        //Def.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        Def.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                        Def.BorderColor = BaseColor.WHITE;
                                        Def.FixedHeight = 15f;
                                        table.AddCell(Def);
                                    }
                                    else
                                    {
                                        PdfPCell Def = new PdfPCell(new Phrase("", TableFont));
                                        Def.HorizontalAlignment = Element.ALIGN_LEFT;
                                        //Def.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        Def.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                        Def.BorderColor = BaseColor.WHITE;
                                        Def.FixedHeight = 15f;
                                        table.AddCell(Def);
                                    }

                                    if (dr1["AGY_ACTIVE"].ToString() != "Y")
                                    {
                                        PdfPCell Def = new PdfPCell(new Phrase("Y", TableFont));
                                        Def.HorizontalAlignment = Element.ALIGN_CENTER;
                                        //Def.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        Def.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                        Def.BorderColor = BaseColor.WHITE;
                                        Def.FixedHeight = 15f;
                                        table.AddCell(Def);
                                    }
                                    else
                                    {
                                        PdfPCell Def = new PdfPCell(new Phrase("", TableFont));
                                        Def.HorizontalAlignment = Element.ALIGN_LEFT;
                                        //Def.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        Def.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                        Def.BorderColor = BaseColor.WHITE;
                                        Def.FixedHeight = 15f;
                                        table.AddCell(Def);
                                    }

                                    //PrintRec(dr[Agy_Code_Cell].ToString(), 35);
                                    //PrintRec(dr[Agy_Desc_Cell].ToString(), 340);


                                    //if (dr["AGY_DEFAULT"].ToString() == "Y")
                                    //    PrintRec("Y", 70);
                                    //else
                                    //    PrintRec(" ", 70);

                                    //if (dr["AGY_ACTIVE"].ToString() != "Y")
                                    //    PrintRec("Y", 60);
                                    //else
                                    //    PrintRec(" ", 60);

                                    //Y_Pos -= 10;
                                    //CheckBottomBorderReached(document, writer);
                                }
                            }
                        }

                    NextAgyType: continue;
                    }
                    document.Add(table);
                }
                //cb.EndText();
            }
            catch (Exception ex) { document.Add(new Paragraph("Aborted due to Exception............................................... ")); }

            ExcelAgencyTable(((RepListItem)cmbModule.SelectedItem).Value.ToString());
        }
        //End of Agency Table List Print Logic

        private void ServiceCodeList(Document document, PdfWriter writer, bool Format)
        {
            document.SetPageSize(new iTextSharp.text.Rectangle(iTextSharp.text.PageSize.A4.Width, iTextSharp.text.PageSize.A4.Height));

            DataSet ds = DatabaseLayer.ADMNB001DB.ADMNB001_GetServices();
            DataTable dt = ds.Tables[0];

            if (Format)
            {
                document.SetPageSize(new iTextSharp.text.Rectangle(iTextSharp.text.PageSize.A4.Width, iTextSharp.text.PageSize.A4.Height));
                cb.BeginText();
                cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES).BaseFont, 12);
                //cb.SetFontAndSize(bfTimes, 10);
                try
                {
                    PrintHeader();

                    //           cb.SetColorFill(BaseColor.BLACK);
                    cb.SetCMYKColorFill(0, 0, 0, 255);
                    X_Pos = 50;
                    Y_Pos -= 5;

                    CheckBottomBorderReached(document, writer);
                    cb.EndText();
                    Print_Line();

                    cb.BeginText();
                    cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES).BaseFont, 12);
                    //cb.SetFontAndSize(bfTimes, 10);
                    cb.SetColorFill(BaseColor.GRAY);
                    //cb.ShowTextAligned(Element.ALIGN_MIDDLE, "Case Management Sevice Codes", 250, 820, 0);
                    //       cb.SetColorFill(BaseColor.BLACK);
                    cb.SetCMYKColorFill(0, 0, 0, 255);
                    foreach (DataRow dr in dt.Rows)
                    {
                        X_Pos = 70;
                        Y_Pos -= 10;
                        CheckBottomBorderReached(document, writer);
                        PrintRec(dr["CAC_SERVICE"].ToString(), 130);
                        PrintRec(dr["CAC_DESC"].ToString(), 150);
                    }
                    cb.EndText();
                }
                catch (Exception ex) { document.Add(new Paragraph("Aborted due to Exception............................................... ")); }
            }
            else
            {
                //document.SetPageSize(new iTextSharp.text.Rectangle(iTextSharp.text.PageSize.A4.Width, iTextSharp.text.PageSize.A4.Height)); 
                //document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                cb.BeginText();
                cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES).BaseFont, 12);
                //cb.SetFontAndSize(bfTimes, 10);

                try
                {
                    PrintHeader();

                    //        cb.SetColorFill(BaseColor.BLACK);
                    cb.SetCMYKColorFill(0, 0, 0, 255);
                    //X_Pos = 50;
                    //Y_Pos -= 5;
                    X_Pos = 50;
                    Y_Pos -= 5;
                    CheckBottomBorderReachedLandScape(document, writer);
                    cb.EndText();
                    Print_Line();

                    cb.BeginText();
                    cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES).BaseFont, 12);

                    //cb.SetFontAndSize(bfTimes, 10);
                    cb.SetColorFill(BaseColor.GRAY);
                    //cb.ShowTextAligned(Element.ALIGN_MIDDLE, "Emergency Services", 250, 580, 0);
                    //           cb.SetColorFill(BaseColor.BLACK);
                    cb.SetCMYKColorFill(0, 0, 0, 255);
                    foreach (DataRow dr in dt.Rows)
                    {
                        X_Pos = 60; Y_Pos -= 10;
                        CheckBottomBorderReachedLandScape(document, writer);
                        PrintRec(dr["CAC_SERVICE"].ToString(), 80);
                        PrintRec(dr["CAC_FUND"].ToString(), 40);
                        PrintRec(dr["CAC_AGENCY"].ToString() + dr["CAC_DEPT"].ToString() + dr["CAC_PROGRAM"].ToString(), 70);
                        PrintRec(dr["CAC_DESC"].ToString(), 190);
                        PrintRec(dr["CAC_APPLICATION"].ToString(), 40);
                        PrintRec(dr["CAC_ZERO"].ToString(), 60);
                        PrintRec(dr["CAC_VENDOR"].ToString(), 60);
                        PrintRec(dr["CAC_AUTHORIZATIONS"].ToString(), 50);
                        PrintRec(dr["CAC_MI"].ToString(), 50);
                        PrintRec(dr["CAC_OUTOFPOVERTY"].ToString(), 80);
                        PrintRec(dr["CAC_ACTIVE"].ToString(), 50);
                    }
                    cb.EndText();
                }
                catch (Exception ex) { document.Add(new Paragraph("Aborted due to Exception............................................... ")); }
            }
        }

        private void CaseDemoAssociation(Document document, PdfWriter writer)
        {
            //document = new Document(iTextSharp.text.PageSize.A4.Rotate());
            try
            {
                //DataSet ds = DatabaseLayer.ADMNB001DB.ADMNB001_GetCaseDemoAss();
                //DataTable dt = ds.Tables[0];
                DataSet ds = DatabaseLayer.ADMNB001DB.ADMNB001_GetCSBCAT();
                DataTable dt = ds.Tables[0];
                DataSet dsAssoc = DatabaseLayer.ADMNB001DB.ADMNB001_GetCSB4ASSOC();
                DataTable dtAssoc = dsAssoc.Tables[0];

                BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
                iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 10);
                iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 10);
                iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 9, 3);
                iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_times, 10, 1);
                iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 9, 2);
                iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 9, 4);
                iTextSharp.text.Font TimesHeader = new iTextSharp.text.Font(bf_times, 12, 1, BaseColor.BLUE);

                //cb.BeginText();
                //cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES).BaseFont, 12);
                ////cb.SetFontAndSize(bfTimes, 10);

                //PrintHeader();
                ////      cb.SetColorFill(BaseColor.BLACK);
                //cb.SetCMYKColorFill(0, 0, 0, 255);
                //X_Pos = 50;
                //Y_Pos -= 5;
                //CheckBottomBorderReachedLandScape(document, writer);
                //cb.EndText();
                //Print_Line();

                //cb.BeginText();
                //cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES).BaseFont, 12);
                ////cb.SetFontAndSize(bfTimes, 10);
                //cb.SetColorFill(BaseColor.GRAY);
                ////cb.ShowTextAligned(Element.ALIGN_MIDDLE, "Case Demo Association", 350, 580, 0);
                ////      cb.SetColorFill(BaseColor.BLACK);
                //cb.SetCMYKColorFill(0, 0, 0, 255);
                string PrivAgyDesc = null;
                string privTable = null;

                string AgyType = null, Temp_Agy_Codes = null, Temp_Code = null;
                if (dt.Rows.Count > 0)
                {
                    PdfPTable table = new PdfPTable(7);
                    table.TotalWidth = 560f;
                    table.WidthPercentage = 100;
                    table.LockedWidth = true;
                    float[] widths = new float[] { 33f, 60f, 14f, 11f, 10f, 8f, 8f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                    table.SetWidths(widths);
                    table.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.HeaderRows = 2;

                    PdfPCell HeaderDesc = new PdfPCell(new Phrase("Case Demographic Associations", TimesHeader));
                    HeaderDesc.HorizontalAlignment = Element.ALIGN_CENTER;
                    HeaderDesc.FixedHeight = 15f;
                    HeaderDesc.Colspan = 7;
                    HeaderDesc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    table.AddCell(HeaderDesc);


                    string[] Header = { "CSBG Category", "Description Codes Used", "Age From", "Age To", "Table#", "Code", "Desc" };
                    for (int i = 0; i < Header.Length; ++i)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(Header[i], TblFontBold));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.FixedHeight = 15f;
                        cell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                        table.AddCell(cell);
                    }

                    foreach (DataRow dr in dt.Rows)
                    {
                        //X_Pos = 60; Y_Pos -= 15;
                        //CheckBottomBorderReachedLandScape(document, writer);
                        if (PrivAgyDesc != dr["AGY_DESC"].ToString())
                        {
                            PdfPCell AGY_DESC = new PdfPCell(new Phrase(dr["AGY_DESC"].ToString().Trim(), TableFont));
                            AGY_DESC.HorizontalAlignment = Element.ALIGN_LEFT;
                            AGY_DESC.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            table.AddCell(AGY_DESC);
                            //PrintRec(dr["AGY_DESC"].ToString(), 150);
                            PrivAgyDesc = dr["AGY_DESC"].ToString();
                            AgyType = dr["CSB4CATG_AGY_TYPE"].ToString();
                        }
                        else
                        {
                            PdfPCell AGY_DESC = new PdfPCell(new Phrase("", TableFont));
                            AGY_DESC.HorizontalAlignment = Element.ALIGN_LEFT;
                            AGY_DESC.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            table.AddCell(AGY_DESC);
                            //PrintRec(" ", 150);
                        }
                        bool First = true;
                        DataSet dsAgyCodes = DatabaseLayer.Lookups.GetLookUpFromAGYTAB(AgyType);
                        DataTable dtAgyCodes = dsAgyCodes.Tables[0];
                        string AgycodesTemp = string.Empty; string NotAgycodes = string.Empty;
                        foreach (DataRow drAssoc in dtAssoc.Rows)
                        {

                            if (drAssoc["CSB4ASOC_CATG_CODE"].ToString() == dr["CSB4CATG_CODE"].ToString())
                            {
                                if (!First)
                                {
                                    PdfPCell AGY_DESC = new PdfPCell(new Phrase("", TableFont));
                                    AGY_DESC.HorizontalAlignment = Element.ALIGN_LEFT;
                                    AGY_DESC.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(AGY_DESC);
                                }

                                PdfPCell CSB4CATG_DESC = new PdfPCell(new Phrase(drAssoc["CSB4CATG_DESC"].ToString().Trim(), TableFont));
                                CSB4CATG_DESC.HorizontalAlignment = Element.ALIGN_LEFT;
                                CSB4CATG_DESC.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(CSB4CATG_DESC);

                                PdfPCell AgeFrom = new PdfPCell(new Phrase(drAssoc["CSB4ASOC_AGE_FROM"].ToString().Trim(), TableFont));
                                AgeFrom.HorizontalAlignment = Element.ALIGN_RIGHT;
                                AgeFrom.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(AgeFrom);

                                PdfPCell Ageto = new PdfPCell(new Phrase(drAssoc["CSB4ASOC_AGE_TO"].ToString().Trim(), TableFont));
                                Ageto.HorizontalAlignment = Element.ALIGN_RIGHT;
                                Ageto.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(Ageto);

                                //string SS = drAssoc["CSB4CATG_DESC"].ToString();
                                //string SD = drAssoc["CSB4ASOC_AGE_FROM"].ToString();
                                //string SA = drAssoc["CSB4ASOC_AGE_TO"].ToString();

                                //PrintRec(drAssoc["CSB4CATG_DESC"].ToString(), 260);
                                //PrintRec(drAssoc["CSB4ASOC_AGE_FROM"].ToString(), 60);
                                //PrintRec(drAssoc["CSB4ASOC_AGE_TO"].ToString(), 100);
                                Temp_Agy_Codes = drAssoc["CSB4ASOC_AGYTAB_CODES"].ToString();

                                //PrivAgyDesc = null;
                                if (privTable != dr["TableUsed"].ToString())
                                {
                                    PdfPCell TableUsed = new PdfPCell(new Phrase(dr["TableUsed"].ToString().Substring(0, 5).ToString(), TableFont));
                                    TableUsed.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    TableUsed.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(TableUsed);

                                    PdfPCell TableUsed1 = new PdfPCell(new Phrase(dr["TableUsed"].ToString().Substring(6, 1).ToString(), TableFont));
                                    TableUsed1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    TableUsed1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(TableUsed1);

                                    PdfPCell TableUsed2 = new PdfPCell(new Phrase(dr["TableUsed"].ToString().Substring(8, 1).ToString(), TableFont));
                                    TableUsed2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    TableUsed2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(TableUsed2);

                                    //PrintRec(dr["TableUsed"].ToString().Substring(0, 5).ToString(), 70);
                                    //PrintRec(dr["TableUsed"].ToString().Substring(6, 1).ToString(), 60);
                                    //PrintRec(dr["TableUsed"].ToString().Substring(8, 1).ToString(), 60);
                                    privTable = dr["TableUsed"].ToString();
                                }
                                else
                                {
                                    PdfPCell TableUsed = new PdfPCell(new Phrase("", TableFont));
                                    TableUsed.HorizontalAlignment = Element.ALIGN_LEFT;
                                    TableUsed.Colspan = 3;
                                    TableUsed.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(TableUsed);
                                    //PrintRec(" ", 150);
                                }
                                //Y_Pos -= 15;
                                //CheckBottomBorderReachedLandScape(document, writer); X_Pos = 215;

                                foreach (DataRow drAgyCodes in dtAgyCodes.Rows)
                                {
                                    //bool Code_Exists = false;
                                    bool Can_Continue = true;
                                    if (!string.IsNullOrEmpty(Temp_Agy_Codes) && Temp_Agy_Codes.Length >= 1)
                                    {
                                        for (int i = 0; Can_Continue;)
                                        {

                                            if (Temp_Agy_Codes.Substring(i, Temp_Agy_Codes.Length - i).Length < 5)
                                                Temp_Code = Temp_Agy_Codes.Substring(i, Temp_Agy_Codes.Length - i);
                                            else
                                                Temp_Code = Temp_Agy_Codes.Substring(i, 5);
                                            if (drAgyCodes["Code"].ToString().Trim() == Temp_Code.Trim())
                                            {
                                                if (!string.IsNullOrEmpty(AgycodesTemp.Trim()))
                                                {
                                                    if (AgycodesTemp.Contains(drAgyCodes["Code"].ToString().Trim()))
                                                    { }
                                                    else
                                                    {
                                                        if (NotAgycodes.Contains(drAgyCodes["Code"].ToString().Trim()))
                                                        {
                                                            NotAgycodes = NotAgycodes.Replace(drAgyCodes["Code"].ToString().Trim(), "");
                                                            AgycodesTemp += drAgyCodes["Code"].ToString().Trim() + " ";
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (NotAgycodes.Contains(drAgyCodes["Code"].ToString().Trim()))
                                                    {
                                                        NotAgycodes = NotAgycodes.Replace(drAgyCodes["Code"].ToString().Trim(), "");
                                                        AgycodesTemp += drAgyCodes["Code"].ToString().Trim() + " ";
                                                    }
                                                    else AgycodesTemp += drAgyCodes["Code"].ToString().Trim() + " ";
                                                }
                                                PdfPCell Space = new PdfPCell(new Phrase("", TableFont));
                                                Space.HorizontalAlignment = Element.ALIGN_LEFT;
                                                Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                table.AddCell(Space);

                                                PdfPCell Code = new PdfPCell(new Phrase("    " + drAgyCodes["Code"].ToString().Trim() + "  " + drAgyCodes["LookUpDesc"].ToString().Trim(), TableFont));
                                                Code.HorizontalAlignment = Element.ALIGN_LEFT;
                                                Code.Colspan = 6;
                                                Code.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                table.AddCell(Code);

                                                //string SG = drAgyCodes["Code"].ToString();
                                                //string SV = drAgyCodes["LookUpDesc"].ToString();
                                                //PrintRec(drAgyCodes["Code"].ToString(), 30);
                                                //PrintRec(drAgyCodes["LookUpDesc"].ToString(), 70);
                                                //Y_Pos -= 15;
                                                //CheckBottomBorderReachedLandScape(document, writer); X_Pos = 215;
                                            }
                                            else
                                            {
                                                //if (!string.IsNullOrEmpty(Temp_Code.Trim()))
                                                //{
                                                if (!string.IsNullOrEmpty(NotAgycodes.Trim()))
                                                {
                                                    if (NotAgycodes.Contains(drAgyCodes["Code"].ToString().Trim()))
                                                    { }
                                                    else
                                                    {
                                                        if (AgycodesTemp.Contains(drAgyCodes["Code"].ToString().Trim())) { }
                                                        else
                                                            NotAgycodes += drAgyCodes["Code"].ToString().Trim() + " ";
                                                    }
                                                }
                                                else
                                                {
                                                    if (AgycodesTemp.Contains(drAgyCodes["Code"].ToString().Trim())) { }
                                                    else
                                                        NotAgycodes += drAgyCodes["Code"].ToString().Trim() + " ";
                                                }
                                                //}
                                            }

                                            i += 5;
                                            if (i >= Temp_Agy_Codes.Length)
                                                Can_Continue = false;
                                        }
                                    }

                                }
                                //X_Pos = 60; Y_Pos -= 13;
                                //CheckBottomBorderReachedLandScape(document, writer);
                                PdfPCell Space3 = new PdfPCell(new Phrase("", TableFont));
                                Space3.HorizontalAlignment = Element.ALIGN_LEFT;
                                Space3.Colspan = 7;
                                Space3.FixedHeight = 15f;
                                Space3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(Space3);
                                //PrintRec(" ", 150);
                                First = false;
                            }

                        }

                        if (!string.IsNullOrEmpty(NotAgycodes.Trim()))
                        {
                            PdfPCell Space = new PdfPCell(new Phrase(" ", TableFont));
                            Space.HorizontalAlignment = Element.ALIGN_LEFT;
                            Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            table.AddCell(Space);

                            PdfPCell Code = new PdfPCell(new Phrase("Not Assoc  ", TableFont));
                            Code.HorizontalAlignment = Element.ALIGN_LEFT;
                            Code.Colspan = 6;
                            Code.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            table.AddCell(Code);
                            string[] tmpCd = NotAgycodes.Split(' ');
                            for (int i = 0; i < tmpCd.Length; i++)
                            {
                                if (!string.IsNullOrEmpty(tmpCd[i].ToString()))
                                {
                                    DataRow[] row = dtAgyCodes.Select("Code='" + tmpCd[i].ToString() + "'");
                                    if (row != null)
                                    {
                                        PdfPCell Space1 = new PdfPCell(new Phrase(" ", TableFont));
                                        Space1.HorizontalAlignment = Element.ALIGN_LEFT;
                                        Space1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(Space1);

                                        PdfPCell Code2 = new PdfPCell(new Phrase("    " + row[0]["Code"].ToString().Trim() + "  " + row[0]["LookUpDesc"].ToString().Trim(), TableFont));
                                        Code2.HorizontalAlignment = Element.ALIGN_LEFT;
                                        Code2.Colspan = 6;
                                        Code2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(Code2);
                                    }
                                }
                            }
                            PdfPCell Space3 = new PdfPCell(new Phrase("", TableFont));
                            Space3.HorizontalAlignment = Element.ALIGN_LEFT;
                            Space3.Colspan = 7;
                            Space3.FixedHeight = 15f;
                            Space3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            table.AddCell(Space3);
                            //PrintRec(" ", 150);
                            First = false;
                        }
                        else if (string.IsNullOrEmpty(AgycodesTemp.Trim()))
                        {
                            PdfPCell Space = new PdfPCell(new Phrase(" ", TableFont));
                            Space.HorizontalAlignment = Element.ALIGN_LEFT;
                            Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            table.AddCell(Space);

                            PdfPCell Code = new PdfPCell(new Phrase("Not Assoc  ", TableFont));
                            Code.HorizontalAlignment = Element.ALIGN_LEFT;
                            Code.Colspan = 6;
                            Code.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            table.AddCell(Code);
                            //string[] tmpCd = NotAgycodes.Split(' ');
                            if (dtAgyCodes.Rows.Count > 0)
                            {
                                foreach (DataRow row in dtAgyCodes.Rows)
                                {
                                    PdfPCell Space1 = new PdfPCell(new Phrase(" ", TableFont));
                                    Space1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Space1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(Space1);

                                    PdfPCell Code2 = new PdfPCell(new Phrase("    " + row["Code"].ToString().Trim() + "  " + row["LookUpDesc"].ToString().Trim(), TableFont));
                                    Code2.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Code2.Colspan = 6;
                                    Code2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(Code2);
                                }
                            }
                            PdfPCell Space3 = new PdfPCell(new Phrase("", TableFont));
                            Space3.HorizontalAlignment = Element.ALIGN_LEFT;
                            Space3.Colspan = 7;
                            Space3.FixedHeight = 15f;
                            Space3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            table.AddCell(Space3);
                            //PrintRec(" ", 150);
                            First = false;
                        }
                    }
                    document.Add(table);
                }

                //cb.EndText();
            }
            catch (Exception ex) { document.Add(new Paragraph("Aborted due to Exception............................................... ")); }
        }

        private void PerformanceMeasures(Document document, PdfWriter writer)
        {
            try
            {
                string[] RefWdate = null; string Refdate = null;
                string Fdate = null, Tdate = null;

                Refdate = ((RepListItem)CmbAgyTab.SelectedItem).Value.ToString();
                RefWdate = Refdate.Split(',');
                Fdate = RefWdate[0]; Tdate = RefWdate[1];

                string empty = " ";
                DataSet ds = DatabaseLayer.SPAdminDB.Get_CSB14GRP(Fdate, Tdate, null, empty, null);
                DataTable dt = ds.Tables[0];
                DataView dvgrp = new DataView(dt);
                dvgrp.Sort = "CSB14GRP_GROUP_CODE";//+string.Empty+"'";
                dt = dvgrp.ToTable();

                DataSet dsCsbRa = DatabaseLayer.SPAdminDB.Get_CSB14RA(Fdate, Tdate, null, null);
                DataTable dtCsbRa = dsCsbRa.Tables[0];

                BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
                iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 10);
                iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 10);
                iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 9, 3);
                iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_times, 10, 1);
                iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 9, 2);
                iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 9, 4);
                iTextSharp.text.Font TblFontBoldColor = new iTextSharp.text.Font(bf_times, 11, 1, BaseColor.BLUE);

                List<MSMASTEntity> MSList = _model.SPAdminData.Browse_MSMAST(null, null, null, null, null);
                if (dt.Rows.Count > 0)
                {
                    PdfPTable table = new PdfPTable(6);
                    table.TotalWidth = 540f;
                    table.WidthPercentage = 100;
                    table.LockedWidth = true;
                    float[] widths = new float[] { 15f, 13f, 75f, 6f, 12f, 8f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                    table.SetWidths(widths);
                    table.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.HeaderRows = 2;

                    PdfPCell Hdr = new PdfPCell(new Phrase("Performance Measures Association", TblFontBoldColor));
                    Hdr.HorizontalAlignment = Element.ALIGN_CENTER;
                    Hdr.FixedHeight = 15f;
                    Hdr.Colspan = 6;
                    Hdr.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    table.AddCell(Hdr);

                    PdfPCell ref_fdate = new PdfPCell(new Phrase("From " + LookupDataAccess.Getdate(Fdate) + "  To " + LookupDataAccess.Getdate(Tdate), TblFontBoldColor));
                    ref_fdate.HorizontalAlignment = Element.ALIGN_CENTER;
                    ref_fdate.Colspan = 6;
                    ref_fdate.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    table.AddCell(ref_fdate);

                    string UsedGoals = string.Empty;
                    string temp_code = null; string AgyGoal_code = null; int j = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        string Grp_List = null; string group = null;

                        PdfPCell Space_code = new PdfPCell(new Phrase("", TblFontBold));
                        Space_code.HorizontalAlignment = Element.ALIGN_LEFT;
                        Space_code.Colspan = 6;
                        Space_code.FixedHeight = 10f;
                        Space_code.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        table.AddCell(Space_code);

                        PdfPCell group_code = new PdfPCell(new Phrase("Group : " + dr["csb14grp_group_code"].ToString().Trim(), TblFontBold));
                        group_code.HorizontalAlignment = Element.ALIGN_LEFT;
                        group_code.Colspan = 2;
                        group_code.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER;
                        table.AddCell(group_code);

                        PdfPCell group_desc = new PdfPCell(new Phrase(dr["csb14grp_desc"].ToString().Trim(), TblFontBold));
                        group_desc.HorizontalAlignment = Element.ALIGN_LEFT;
                        //group_desc.Colspan = 2;
                        group_desc.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER;
                        table.AddCell(group_desc);

                        PdfPCell group_Space = new PdfPCell(new Phrase("", TableFont));
                        group_Space.HorizontalAlignment = Element.ALIGN_LEFT;
                        group_Space.Colspan = 3;
                        group_Space.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER;
                        table.AddCell(group_Space);


                        PdfPCell cell1 = new PdfPCell(new Phrase("Headers & Results", TblFontBold));
                        cell1.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell1.FixedHeight = 15f;
                        cell1.Colspan = 6;
                        cell1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        table.AddCell(cell1);

                        group = dr["csb14grp_group_code"].ToString();
                        string RefFdate = dr["csb14grp_ref_fdate"].ToString();
                        string RefTdate = dr["csb14grp_ref_tdate"].ToString();
                        if (!string.IsNullOrEmpty(dr["csb14grp_cnt_hdr1"].ToString()) && dr["csb14grp_cnt_incld1"].ToString() == "True")
                            Grp_List = "1";
                        if (!string.IsNullOrEmpty(dr["csb14grp_cnt_hdr2"].ToString()) && dr["csb14grp_cnt_incld2"].ToString() == "True")
                            Grp_List += "2";
                        if (!string.IsNullOrEmpty(dr["csb14grp_cnt_hdr3"].ToString()) && dr["csb14grp_cnt_incld3"].ToString() == "True")
                            Grp_List += "3";
                        if (!string.IsNullOrEmpty(dr["csb14grp_cnt_hdr4"].ToString()) && dr["csb14grp_cnt_incld4"].ToString() == "True")
                            Grp_List += "4";
                        if (!string.IsNullOrEmpty(dr["csb14grp_cnt_hdr5"].ToString()) && dr["csb14grp_cnt_incld5"].ToString() == "True")
                            Grp_List += "5";

                        for (int i = 0; i < Grp_List.Length; i++)
                        {
                            if (Grp_List.Substring(i, 1) == "1")
                            {
                                PdfPCell hdr1 = new PdfPCell(new Phrase(dr["csb14grp_cnt_hdr1"].ToString().Trim(), TableFont));
                                hdr1.HorizontalAlignment = Element.ALIGN_LEFT;
                                hdr1.Colspan = 6;
                                hdr1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(hdr1);
                            }
                            else if (Grp_List.Substring(i, 1) == "2")
                            {
                                PdfPCell hdr1 = new PdfPCell(new Phrase(dr["csb14grp_cnt_hdr2"].ToString().Trim(), TableFont));
                                hdr1.HorizontalAlignment = Element.ALIGN_LEFT;
                                hdr1.Colspan = 6;
                                hdr1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(hdr1);
                            }
                            else if (Grp_List.Substring(i, 1) == "3")
                            {
                                PdfPCell hdr1 = new PdfPCell(new Phrase(dr["csb14grp_cnt_hdr3"].ToString().Trim(), TableFont));
                                hdr1.HorizontalAlignment = Element.ALIGN_LEFT;
                                hdr1.Colspan = 6;
                                hdr1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(hdr1);
                            }
                            else if (Grp_List.Substring(i, 1) == "4")
                            {
                                PdfPCell hdr1 = new PdfPCell(new Phrase(dr["csb14grp_cnt_hdr4"].ToString().Trim(), TableFont));
                                hdr1.HorizontalAlignment = Element.ALIGN_LEFT;
                                hdr1.Colspan = 6;
                                hdr1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(hdr1);
                            }
                            else
                            {
                                PdfPCell hdr1 = new PdfPCell(new Phrase(dr["csb14grp_cnt_hdr5"].ToString().Trim(), TableFont));
                                hdr1.HorizontalAlignment = Element.ALIGN_LEFT;
                                hdr1.Colspan = 6;
                                hdr1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(hdr1);
                            }

                            foreach (DataRow drCsbRa in dtCsbRa.Rows)
                            {
                                if (Convert.ToDateTime(drCsbRa["CSB14RA_REF_FDATE"].ToString()) == Convert.ToDateTime(dr["csb14grp_ref_fdate"].ToString()) && Convert.ToDateTime(drCsbRa["CSB14RA_REF_TDATE"].ToString()) == Convert.ToDateTime(dr["csb14grp_ref_tdate"].ToString()) && drCsbRa["CSB14RA_GROUP_CODE"].ToString().Trim() == dr["csb14grp_group_code"].ToString().Trim())
                                {
                                    string Grp = Grp_List.Substring(i, 1);
                                    if (drCsbRa["CSB14RA_COUNT_CODE"].ToString() == Grp_List.Substring(i, 1))
                                    {
                                        PdfPCell Space1 = new PdfPCell(new Phrase("", TableFont));
                                        Space1.HorizontalAlignment = Element.ALIGN_LEFT;
                                        //Space1.Colspan = 2;
                                        Space1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(Space1);

                                        PdfPCell RESULT_CODE = new PdfPCell(new Phrase(drCsbRa["CSB14RA_RESULT_CODE"].ToString().Trim(), TableFont));
                                        RESULT_CODE.HorizontalAlignment = Element.ALIGN_LEFT;
                                        //RESULT_CODE.Colspan = 2;
                                        RESULT_CODE.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(RESULT_CODE);

                                        PdfPCell RESULT_Desc = new PdfPCell(new Phrase(drCsbRa["CSB14RA_DESC"].ToString().Trim(), TableFont));
                                        RESULT_Desc.HorizontalAlignment = Element.ALIGN_LEFT;
                                        RESULT_Desc.Colspan = 4;
                                        RESULT_Desc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table.AddCell(RESULT_Desc);
                                    }
                                }
                            }
                        }


                        string Goal = null; string goaldesc = null;
                        DataSet dsGrp = DatabaseLayer.SPAdminDB.Get_CSB14GRP(RefFdate, RefTdate, group, null, null);
                        DataTable dtGrp = dsGrp.Tables[0];
                        DataView dv = new DataView(dtGrp);
                        dv.Sort = "csb14grp_table_code";//+string.Empty+"'";
                        dtGrp = dv.ToTable();
                        if (dtGrp.Rows.Count > 1)
                        {
                            PdfPCell TbHeader = new PdfPCell(new Phrase("Table/Goals", TblFontBold));
                            TbHeader.HorizontalAlignment = Element.ALIGN_LEFT;
                            TbHeader.FixedHeight = 15f;
                            TbHeader.Colspan = 2;
                            TbHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            table.AddCell(TbHeader);

                            string[] Header = { "Description", "Ind", "Achieve", "CC" };
                            for (int i = 0; i < Header.Length; ++i)
                            {
                                PdfPCell cell = new PdfPCell(new Phrase(Header[i], TblFontBold));
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                cell.FixedHeight = 15f;
                                cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                table.AddCell(cell);
                            }

                        }
                        foreach (DataRow drGrp in dtGrp.Rows)
                        {
                            if (!string.IsNullOrWhiteSpace(drGrp["csb14grp_table_code"].ToString()))
                            {
                                temp_code = drGrp["csb14grp_goal_codes"].ToString();

                                if (!string.IsNullOrEmpty(drGrp["csb14grp_table_code"].ToString()))
                                {
                                    PdfPCell table_code = new PdfPCell(new Phrase(drGrp["csb14grp_table_code"].ToString().Trim(), TableFont));
                                    table_code.HorizontalAlignment = Element.ALIGN_LEFT;
                                    table_code.Colspan = 2;
                                    table_code.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(table_code);

                                    PdfPCell desc = new PdfPCell(new Phrase(drGrp["csb14grp_desc"].ToString().Trim(), TableFont));
                                    desc.HorizontalAlignment = Element.ALIGN_LEFT;
                                    desc.FixedHeight = 12f;
                                    desc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(desc);

                                    PdfPCell cnt_indicator = new PdfPCell(new Phrase(drGrp["csb14grp_cnt_indicator"].ToString().Trim(), TableFont));
                                    cnt_indicator.HorizontalAlignment = Element.ALIGN_CENTER;
                                    cnt_indicator.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(cnt_indicator);

                                    PdfPCell achieve = new PdfPCell(new Phrase(drGrp["csb14grp_expect_achieve"].ToString().Trim(), TableFont));
                                    achieve.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    achieve.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(achieve);

                                    PdfPCell calc_cost = new PdfPCell(new Phrase(drGrp["csb14grp_calc_cost"].ToString().Trim(), TableFont));
                                    calc_cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    calc_cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    table.AddCell(calc_cost);
                                    bool Can_Continue = true;
                                    if (!string.IsNullOrWhiteSpace(temp_code) && temp_code.Length >= 1)
                                    {
                                        for (j = 0; Can_Continue;)
                                        {
                                            if ((temp_code.Substring(j, temp_code.Length - j).Length) < 10)
                                                AgyGoal_code = temp_code.Substring(j, temp_code.Substring(j, temp_code.Length - j).Length);
                                            else
                                                AgyGoal_code = temp_code.Substring(j, 10);
                                            if (string.IsNullOrEmpty(UsedGoals.Trim()))
                                                UsedGoals = AgyGoal_code.Trim() + " ";
                                            else
                                            {
                                                if (UsedGoals.Contains(AgyGoal_code.ToString().Trim()))
                                                { }
                                                else
                                                    UsedGoals += AgyGoal_code.Trim() + " ";
                                            }

                                            foreach (MSMASTEntity Entity in MSList)
                                            {
                                                if (!string.IsNullOrWhiteSpace(AgyGoal_code))
                                                {
                                                    if (Entity.Code.ToString().Trim() == AgyGoal_code.ToString().Trim())
                                                    {
                                                        Goal = Entity.Code;
                                                        goaldesc = Entity.Desc;
                                                        //Entity.Sel_SW = true;
                                                        //Ststus_Exists = Entity.Sel_WS;
                                                        PdfPCell Space2 = new PdfPCell(new Phrase("", TableFont));
                                                        Space2.HorizontalAlignment = Element.ALIGN_LEFT;
                                                        //Space1.Colspan = 3;
                                                        Space2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(Space2);

                                                        PdfPCell stone_code = new PdfPCell(new Phrase(Entity.Code.Trim(), TableFont));
                                                        stone_code.HorizontalAlignment = Element.ALIGN_LEFT;
                                                        stone_code.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(stone_code);

                                                        PdfPCell stone_desc = new PdfPCell(new Phrase("   " + Entity.Desc.Trim(), TableFont));
                                                        stone_desc.HorizontalAlignment = Element.ALIGN_LEFT;
                                                        stone_desc.Colspan = 4;
                                                        stone_desc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        table.AddCell(stone_desc);
                                                        break;
                                                    }

                                                }
                                            }

                                            j += 10;
                                            if (j >= temp_code.Length)
                                                Can_Continue = false;
                                        }
                                    }

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

                    string NotUsedGoals = string.Empty;
                    foreach (MSMASTEntity Entity in MSList)
                    {
                        NotUsedGoals += Entity.Code.Trim() + " ";
                    }

                    if (!string.IsNullOrEmpty(UsedGoals.Trim()))
                    {

                        string[] tmpCd = UsedGoals.Split(' ');
                        for (int i = 0; i < tmpCd.Length; i++)
                        {
                            if (!string.IsNullOrEmpty(tmpCd[i].ToString()))
                            {

                                if (NotUsedGoals.Contains(tmpCd[i].ToString().Trim()))
                                    NotUsedGoals = NotUsedGoals.Replace(tmpCd[i].ToString().Trim(), "");

                                //foreach (MSMASTEntity Entity in MSList)
                                //{
                                //    if (Entity.Code.Trim() != tmpCd[i].ToString().Trim())
                                //    {
                                //        if (string.IsNullOrEmpty(NotUsedGoals.Trim()))
                                //        {
                                //            NotUsedGoals = Entity.Code.Trim() + " ";

                                //        }
                                //        else
                                //        {
                                //            if (NotUsedGoals.Contains(Entity.Code.Trim()))
                                //            {
                                //                //NotUsedGoals = NotUsedGoals.Replace(Entity.Code.Trim(), "");
                                //            }
                                //            else
                                //            {
                                //                NotUsedGoals += Entity.Code.Trim() + " ";
                                //            }
                                //        }
                                //    }
                                //    else
                                //    {
                                //        if(NotUsedGoals.Contains(tmpCd[i].ToString().Trim()))
                                //            NotUsedGoals = NotUsedGoals.Replace(Entity.Code.Trim(), "");
                                //    }
                                //}
                            }
                        }

                        if (!string.IsNullOrEmpty(NotUsedGoals.Trim()))
                        {
                            document.NewPage();

                            PdfPTable table1 = new PdfPTable(2);
                            table1.TotalWidth = 500f;
                            table1.WidthPercentage = 100;
                            table1.LockedWidth = true;
                            float[] widths1 = new float[] { 25f, 60f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                            table1.SetWidths(widths1);
                            table1.HorizontalAlignment = Element.ALIGN_CENTER;

                            PdfPCell Header = new PdfPCell(new Phrase("List of Unassociated Goals", TblFontBoldColor));
                            Header.HorizontalAlignment = Element.ALIGN_CENTER;
                            Header.FixedHeight = 15f;
                            Header.Colspan = 2;
                            Header.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            table1.AddCell(Header);

                            PdfPCell stone_head = new PdfPCell(new Phrase("Outcome Code", TblFontBold));
                            stone_head.HorizontalAlignment = Element.ALIGN_LEFT;
                            stone_head.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                            table1.AddCell(stone_head);

                            PdfPCell stone_descH = new PdfPCell(new Phrase("Description", TblFontBold));
                            stone_descH.HorizontalAlignment = Element.ALIGN_LEFT;
                            //stone_desc.Colspan = 4;
                            stone_descH.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                            table1.AddCell(stone_descH);

                            string[] tmpCd1 = NotUsedGoals.Split(' ');
                            Array.Sort(tmpCd1);
                            for (int i = 0; i < tmpCd1.Length; i++)
                            {
                                if (!string.IsNullOrEmpty(tmpCd1[i].ToString()))
                                {
                                    foreach (MSMASTEntity Entity in MSList)
                                    {
                                        if (Entity.Code.Trim() == tmpCd1[i].ToString().Trim())
                                        {
                                            PdfPCell stone_code = new PdfPCell(new Phrase(Entity.Code.Trim(), TableFont));
                                            stone_code.HorizontalAlignment = Element.ALIGN_LEFT;
                                            stone_code.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table1.AddCell(stone_code);

                                            PdfPCell stone_desc = new PdfPCell(new Phrase("   " + Entity.Desc.Trim(), TableFont));
                                            stone_desc.HorizontalAlignment = Element.ALIGN_LEFT;
                                            //stone_desc.Colspan = 4;
                                            stone_desc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            table1.AddCell(stone_desc);
                                        }
                                    }
                                }
                            }
                            document.Add(table1);
                        }

                    }
                    //document.Add(table);
                }
                //if (dt.Rows.Count > 0)
                //{
                //    PdfPTable table = new PdfPTable(8);
                //    table.TotalWidth = 560f;
                //    table.WidthPercentage = 100;
                //    table.LockedWidth = true;
                //    float[] widths = new float[] { 15f, 15f, 15f, 15f, 73f, 6f, 14f, 8f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                //    table.SetWidths(widths);
                //    table.HorizontalAlignment = Element.ALIGN_CENTER;
                //    table.HeaderRows = 1;

                //    string[] Header = { "F Date", "T Date", "Grp Code", "Tab Code", "Description", "Ind", "Achieve", "CC" };
                //    for (int i = 0; i < Header.Length; ++i)
                //    {
                //        PdfPCell cell = new PdfPCell(new Phrase(Header[i], TblFontBold));
                //        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                //        cell.FixedHeight = 15f;
                //        cell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                //        table.AddCell(cell);
                //    }

                //    foreach (DataRow dr in dt.Rows)
                //    {
                //        string Grp_List = null; string group = null;
                //        //X_Pos = 60; Y_Pos -= 15;
                //        //CheckBottomBorderReachedLandScape(document, writer);
                //        //cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_BOLDITALIC).BaseFont, 13);

                //        PdfPCell ref_fdate = new PdfPCell(new Phrase(LookupDataAccess.Getdate(dr["csb14grp_ref_fdate"].ToString().Trim()), TableFont));
                //        ref_fdate.HorizontalAlignment = Element.ALIGN_LEFT;
                //        ref_fdate.Border = iTextSharp.text.Rectangle.NO_BORDER;
                //        table.AddCell(ref_fdate);

                //        PdfPCell ref_tdate = new PdfPCell(new Phrase(LookupDataAccess.Getdate(dr["csb14grp_ref_tdate"].ToString().Trim()), TableFont));
                //        ref_tdate.HorizontalAlignment = Element.ALIGN_LEFT;
                //        ref_tdate.Border = iTextSharp.text.Rectangle.NO_BORDER;
                //        table.AddCell(ref_tdate);

                //        PdfPCell group_code = new PdfPCell(new Phrase(dr["csb14grp_group_code"].ToString().Trim(), TableFont));
                //        group_code.HorizontalAlignment = Element.ALIGN_LEFT;
                //        group_code.Colspan = 2;
                //        group_code.Border = iTextSharp.text.Rectangle.NO_BORDER;
                //        table.AddCell(group_code);

                //        PdfPCell group_desc = new PdfPCell(new Phrase(dr["csb14grp_desc"].ToString().Trim(), TableFont));
                //        group_desc.HorizontalAlignment = Element.ALIGN_LEFT;
                //        //group_desc.Colspan = 2;
                //        group_desc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                //        table.AddCell(group_desc);

                //        PdfPCell group_Space = new PdfPCell(new Phrase("", TableFont));
                //        group_Space.HorizontalAlignment = Element.ALIGN_LEFT;
                //        group_Space.Colspan = 3;
                //        group_Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
                //        table.AddCell(group_Space);

                //        //PrintRec(LookupDataAccess.Getdate(dr["csb14grp_ref_fdate"].ToString()), 70);
                //        //PrintRec(LookupDataAccess.Getdate(dr["csb14grp_ref_tdate"].ToString()), 70);
                //        //PrintRec(dr["csb14grp_group_code"].ToString(), 50);
                //        //PrintRec(dr["csb14grp_desc"].ToString(), 250);
                //        group = dr["csb14grp_group_code"].ToString();
                //        string RefFdate = dr["csb14grp_ref_fdate"].ToString();
                //        string RefTdate = dr["csb14grp_ref_tdate"].ToString();
                //        if (!string.IsNullOrEmpty(dr["csb14grp_cnt_hdr1"].ToString()) && dr["csb14grp_cnt_incld1"].ToString() == "True")
                //            Grp_List = "1";
                //        if (!string.IsNullOrEmpty(dr["csb14grp_cnt_hdr2"].ToString()) && dr["csb14grp_cnt_incld2"].ToString() == "True")
                //            Grp_List += "2";
                //        if (!string.IsNullOrEmpty(dr["csb14grp_cnt_hdr3"].ToString()) && dr["csb14grp_cnt_incld3"].ToString() == "True")
                //            Grp_List += "3";
                //        if (!string.IsNullOrEmpty(dr["csb14grp_cnt_hdr4"].ToString()) && dr["csb14grp_cnt_incld4"].ToString() == "True")
                //            Grp_List += "4";
                //        if (!string.IsNullOrEmpty(dr["csb14grp_cnt_hdr5"].ToString()) && dr["csb14grp_cnt_incld5"].ToString() == "True")
                //            Grp_List += "5";
                //        for (int i = 0; i < Grp_List.Length; i++)
                //        {
                //            //cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES).BaseFont, 12);
                //            //Y_Pos -= 14;
                //            //CheckBottomBorderReachedLandScape(document, writer);
                //            //X_Pos = 200;
                //            PdfPCell Space = new PdfPCell(new Phrase("", TableFont));
                //            Space.HorizontalAlignment = Element.ALIGN_LEFT;
                //            Space.Colspan = 2;
                //            Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
                //            table.AddCell(Space);

                //            if (Grp_List.Substring(i, 1) == "1")
                //            {
                //                PdfPCell hdr1 = new PdfPCell(new Phrase(dr["csb14grp_cnt_hdr1"].ToString().Trim(), TableFont));
                //                hdr1.HorizontalAlignment = Element.ALIGN_LEFT;
                //                hdr1.Colspan = 6;
                //                hdr1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                //                table.AddCell(hdr1);
                //                //PrintRec(dr["csb14grp_cnt_hdr1"].ToString(), 80);
                //            }
                //            else if (Grp_List.Substring(i, 1) == "2")
                //            {
                //                PdfPCell hdr1 = new PdfPCell(new Phrase(dr["csb14grp_cnt_hdr2"].ToString().Trim(), TableFont));
                //                hdr1.HorizontalAlignment = Element.ALIGN_LEFT;
                //                hdr1.Colspan = 6;
                //                hdr1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                //                table.AddCell(hdr1);
                //                //PrintRec(dr["csb14grp_cnt_hdr2"].ToString(), 80);
                //            }
                //            else if (Grp_List.Substring(i, 1) == "3")
                //            {
                //                PdfPCell hdr1 = new PdfPCell(new Phrase(dr["csb14grp_cnt_hdr3"].ToString().Trim(), TableFont));
                //                hdr1.HorizontalAlignment = Element.ALIGN_LEFT;
                //                hdr1.Colspan = 6;
                //                hdr1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                //                table.AddCell(hdr1);
                //                //PrintRec(dr["csb14grp_cnt_hdr3"].ToString(), 80);
                //            }
                //            else if (Grp_List.Substring(i, 1) == "4")
                //            {
                //                PdfPCell hdr1 = new PdfPCell(new Phrase(dr["csb14grp_cnt_hdr4"].ToString().Trim(), TableFont));
                //                hdr1.HorizontalAlignment = Element.ALIGN_LEFT;
                //                hdr1.Colspan = 6;
                //                hdr1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                //                table.AddCell(hdr1);
                //                //PrintRec(dr["csb14grp_cnt_hdr4"].ToString(), 80);
                //            }
                //            else
                //            {
                //                PdfPCell hdr1 = new PdfPCell(new Phrase(dr["csb14grp_cnt_hdr5"].ToString().Trim(), TableFont));
                //                hdr1.HorizontalAlignment = Element.ALIGN_LEFT;
                //                hdr1.Colspan = 6;
                //                hdr1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                //                table.AddCell(hdr1);
                //                //PrintRec(dr["csb14grp_cnt_hdr5"].ToString(), 80);
                //            }

                //            foreach (DataRow drCsbRa in dtCsbRa.Rows)
                //            {
                //                if (drCsbRa["CSB14RA_REF_FDATE"].ToString() == dr["csb14grp_ref_fdate"].ToString() && drCsbRa["CSB14RA_REF_TDATE"].ToString() == dr["csb14grp_ref_tdate"].ToString() && drCsbRa["CSB14RA_GROUP_CODE"].ToString() == dr["csb14grp_group_code"].ToString())
                //                {
                //                    string Grp = Grp_List.Substring(i, 1);
                //                    if (drCsbRa["CSB14RA_COUNT_CODE"].ToString() == Grp_List.Substring(i, 1))
                //                    {
                //                        //Y_Pos -= 14;
                //                        //CheckBottomBorderReachedLandScape(document, writer);
                //                        //X_Pos = 230;
                //                        PdfPCell Space1 = new PdfPCell(new Phrase("", TableFont));
                //                        Space1.HorizontalAlignment = Element.ALIGN_LEFT;
                //                        Space1.Colspan = 2;
                //                        Space1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                //                        table.AddCell(Space1);

                //                        PdfPCell RESULT_CODE = new PdfPCell(new Phrase(drCsbRa["CSB14RA_RESULT_CODE"].ToString().Trim(), TableFont));
                //                        RESULT_CODE.HorizontalAlignment = Element.ALIGN_CENTER;
                //                        RESULT_CODE.Colspan = 2;
                //                        RESULT_CODE.Border = iTextSharp.text.Rectangle.NO_BORDER;
                //                        table.AddCell(RESULT_CODE);

                //                        PdfPCell RESULT_Desc = new PdfPCell(new Phrase(drCsbRa["CSB14RA_DESC"].ToString().Trim(), TableFont));
                //                        RESULT_Desc.HorizontalAlignment = Element.ALIGN_LEFT;
                //                        RESULT_Desc.Colspan = 4;
                //                        RESULT_Desc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                //                        table.AddCell(RESULT_Desc);

                //                        //PrintRec(drCsbRa["CSB14RA_RESULT_CODE"].ToString(), 60);
                //                        //PrintRec(drCsbRa["CSB14RA_DESC"].ToString(), 90);
                //                    }
                //                }
                //            }
                //        }
                //        DataSet dsGrp = DatabaseLayer.SPAdminDB.Get_CSB14GRP(RefFdate, RefTdate, group, null, null);
                //        DataTable dtGrp = dsGrp.Tables[0];
                //        foreach (DataRow drGrp in dtGrp.Rows)
                //        {
                //            if (!string.IsNullOrWhiteSpace(drGrp["csb14grp_table_code"].ToString()))
                //            {
                //                //Y_Pos -= 14;
                //                //CheckBottomBorderReachedLandScape(document, writer);
                //                //X_Pos = 200;
                //                //PrintRec(LookupDataAccess.Getdate(drGrp["csb14grp_ref_fdate"].ToString()), 70);
                //                //PrintRec(LookupDataAccess.Getdate(drGrp["csb14grp_ref_tdate"].ToString()), 70);
                //                //PrintRec(drGrp["csb14grp_group_code"].ToString(), 40);
                //                if (!string.IsNullOrEmpty(drGrp["csb14grp_table_code"].ToString()))
                //                {
                //                    PdfPCell Space1 = new PdfPCell(new Phrase("", TableFont));
                //                    Space1.HorizontalAlignment = Element.ALIGN_LEFT;
                //                    Space1.Colspan = 3;
                //                    Space1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                //                    table.AddCell(Space1);

                //                    PdfPCell table_code = new PdfPCell(new Phrase(drGrp["csb14grp_table_code"].ToString().Trim(), TableFont));
                //                    table_code.HorizontalAlignment = Element.ALIGN_LEFT;
                //                    table_code.Border = iTextSharp.text.Rectangle.NO_BORDER;
                //                    table.AddCell(table_code);

                //                    PdfPCell desc = new PdfPCell(new Phrase(drGrp["csb14grp_desc"].ToString().Trim(), TableFont));
                //                    desc.HorizontalAlignment = Element.ALIGN_LEFT;
                //                    desc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                //                    table.AddCell(desc);

                //                    PdfPCell cnt_indicator = new PdfPCell(new Phrase(drGrp["csb14grp_cnt_indicator"].ToString().Trim(), TableFont));
                //                    cnt_indicator.HorizontalAlignment = Element.ALIGN_CENTER;
                //                    cnt_indicator.Border = iTextSharp.text.Rectangle.NO_BORDER;
                //                    table.AddCell(cnt_indicator);

                //                    PdfPCell achieve = new PdfPCell(new Phrase(drGrp["csb14grp_expect_achieve"].ToString().Trim(), TableFont));
                //                    achieve.HorizontalAlignment = Element.ALIGN_RIGHT;
                //                    achieve.Border = iTextSharp.text.Rectangle.NO_BORDER;
                //                    table.AddCell(achieve);

                //                    PdfPCell calc_cost = new PdfPCell(new Phrase(drGrp["csb14grp_calc_cost"].ToString().Trim(), TableFont));
                //                    calc_cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                //                    calc_cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                //                    table.AddCell(calc_cost);

                //                    //PrintRec(drGrp["csb14grp_table_code"].ToString(), 50);
                //                    //PrintRec(drGrp["csb14grp_desc"].ToString(), 460);
                //                    //PrintRec(drGrp["csb14grp_cnt_indicator"].ToString(), 30);
                //                    //PrintRec(drGrp["csb14grp_expect_achieve"].ToString(), 40);
                //                    //PrintRec(drGrp["csb14grp_calc_cost"].ToString(), 10);
                //                }
                //            }
                //        }

                //    }
                //    document.Add(table);
                //}

                //cb.EndText();
            }
            catch (Exception ex) { document.Add(new Paragraph("Aborted due to Exception............................................... ")); }
        }

        private void RNGCaseDemoAssociation(Document document, PdfWriter writer)
        {
            //document = new Document(iTextSharp.text.PageSize.A4.Rotate());
            try
            {
                string IncCodes = string.Empty, EMPCodes = string.Empty, NCSCodes = string.Empty;

                DataSet ds = DatabaseLayer.ADMNB001DB.ADMNB001_GetRNGCAT();
                DataTable dt = ds.Tables[0];
                DataSet dsAssoc = DatabaseLayer.ADMNB001DB.ADMNB001_GetRNG4ASSOC();
                DataTable dtAssoc = dsAssoc.Tables[0];

                DataSet dscategories = DatabaseLayer.SPAdminDB.Get_RNG4CATG();

                BaseFont bf_Calibri = BaseFont.CreateFont("c:/windows/fonts/calibri.ttf", BaseFont.WINANSI, BaseFont.EMBEDDED);
                iTextSharp.text.Font Calibri = new iTextSharp.text.Font(bf_Calibri, 10);
                iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_Calibri, 8);
                iTextSharp.text.Font SubHeadFont = new iTextSharp.text.Font(bf_Calibri, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_Calibri, 9, 3);
                iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_Calibri, 8, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
                iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_Calibri, 9, 2);
                iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_Calibri, 9, 4);
                iTextSharp.text.Font MainHeaderLine = new iTextSharp.text.Font(bf_Calibri, 9, 1, BaseColor.BLACK);


                //BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
                //iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 10);
                //iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 10);
                //iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 9, 3);
                //iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(1, 10, 1);
                //iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 9, 2);
                //iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 9, 4);
                //iTextSharp.text.Font MainHeaderBackColor = new iTextSharp.text.Font(bf_Calibri, 11, 1, BaseColor.BLUE);

                string PrivAgyDesc = null;
                string privTable = null;

                string AgyType = null, Temp_Agy_Codes = null, Temp_Code = null;
                if (dt.Rows.Count > 0)
                {
                    PdfPTable table = new PdfPTable(7);
                    table.TotalWidth = 560f;
                    table.WidthPercentage = 100;
                    table.LockedWidth = true;
                    float[] widths = new float[] { 33f, 60f, 14f, 11f, 10f, 8f, 8f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                    table.SetWidths(widths);
                    table.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.HeaderRows = 2;

                    PdfPCell Hdr = new PdfPCell(new Phrase("Demographics Associations", MainHeaderLine));
                    Hdr.HorizontalAlignment = Element.ALIGN_CENTER;
                    Hdr.FixedHeight = 15f;
                    Hdr.Colspan = 7;
                    Hdr.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Hdr.PaddingBottom = 5f;
                    table.AddCell(Hdr);

                    string[] Header = { "CSBG Category", "Description Codes Used", "Age From", "Age To", "Table#", "Code", "Desc" };
                    for (int i = 0; i < Header.Length; ++i)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(Header[i], TblFontBold));
                        if (i == 0 || i == 1)
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        else
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.FixedHeight = 15f;
                        //cell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                        cell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));   
                        cell.BorderColor = BaseColor.WHITE;
                        table.AddCell(cell);
                    }

                    int y = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (PrivAgyDesc != dr["RNG4CATG_DESC"].ToString())
                        {
                            if (y > 0)
                            {
                                PdfPCell Space3 = new PdfPCell(new Phrase("", TableFont));
                                Space3.HorizontalAlignment = Element.ALIGN_LEFT;
                                Space3.Colspan = 7;
                                Space3.FixedHeight = 10f;
                                Space3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                Space3.BorderColor = BaseColor.WHITE;
                                table.AddCell(Space3);
                            }
                            y++;

                            PdfPCell AGY_DESC = new PdfPCell(new Phrase(dr["RNG4CATG_DESC"].ToString().Trim(), TableFont));
                            AGY_DESC.HorizontalAlignment = Element.ALIGN_LEFT;
                            //AGY_DESC.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            AGY_DESC.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#a5c8e1"));
                            AGY_DESC.BorderColor = BaseColor.WHITE;
                            AGY_DESC.FixedHeight = 15;
                            table.AddCell(AGY_DESC);
                            PrivAgyDesc = dr["RNG4CATG_DESC"].ToString();
                            AgyType = dr["RNG4CATG_AGY_TYPE"].ToString();
                        }
                        else
                        {
                            PdfPCell AGY_DESC = new PdfPCell(new Phrase("", TableFont));
                            AGY_DESC.HorizontalAlignment = Element.ALIGN_LEFT;
                            //AGY_DESC.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            AGY_DESC.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                            AGY_DESC.BorderColor = BaseColor.WHITE;
                            AGY_DESC.FixedHeight = 15;
                            table.AddCell(AGY_DESC);
                        }
                        bool First = true;
                        DataSet dsAgyCodes = DatabaseLayer.Lookups.GetLookUpFromAGYTAB(AgyType);
                        DataTable dtAgyCodes = new DataTable();
                        List<AGYTABSEntity> AgyTabs_List = new List<AGYTABSEntity>();
                        if (dr["RNG4CATG_CODE"].ToString() == "N")
                        {
                            AGYTABSEntity searchAgytabs = new AGYTABSEntity(true);
                            searchAgytabs.Tabs_Type = AgyType;
                            AgyTabs_List = _model.AdhocData.Browse_AGYTABS(searchAgytabs);
                        }
                        else
                        {
                            if (dsAgyCodes.Tables.Count > 0) dtAgyCodes = dsAgyCodes.Tables[0];
                        }
                        string AgycodesTemp = string.Empty; string NotAgycodes = string.Empty;
                        if (dr["RNG4CATG_CODE"].ToString() != "I")
                        {
                            foreach (DataRow drAssoc in dtAssoc.Rows)
                            {
                                if (drAssoc["RNG4ASOC_CATG_CODE"].ToString() == dr["RNG4CATG_CODE"].ToString())
                                {
                                    if (!First)
                                    {
                                        PdfPCell AGY_DESC = new PdfPCell(new Phrase("", TableFont));
                                        AGY_DESC.HorizontalAlignment = Element.ALIGN_LEFT;
                                        //AGY_DESC.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        AGY_DESC.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                        AGY_DESC.BorderColor = BaseColor.WHITE;
                                        AGY_DESC.FixedHeight = 15f;
                                        table.AddCell(AGY_DESC);
                                    }

                                    PdfPCell CSB4CATG_DESC = new PdfPCell(new Phrase(drAssoc["RNG4CATG_DESC"].ToString().Trim(), TableFont));
                                    CSB4CATG_DESC.HorizontalAlignment = Element.ALIGN_LEFT;
                                    //CSB4CATG_DESC.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    CSB4CATG_DESC.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                    CSB4CATG_DESC.BorderColor = BaseColor.WHITE;
                                    CSB4CATG_DESC.FixedHeight = 15f;
                                    table.AddCell(CSB4CATG_DESC);

                                    PdfPCell AgeFrom = new PdfPCell(new Phrase(drAssoc["RNG4ASOC_AGE_FROM"].ToString().Trim(), TableFont));
                                    AgeFrom.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    //AgeFrom.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    AgeFrom.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                    AgeFrom.BorderColor = BaseColor.WHITE;
                                    AgeFrom.FixedHeight = 15f;
                                    table.AddCell(AgeFrom);

                                    PdfPCell Ageto = new PdfPCell(new Phrase(drAssoc["RNG4ASOC_AGE_TO"].ToString().Trim(), TableFont));
                                    Ageto.HorizontalAlignment = Element.ALIGN_RIGHT;
                                    //Ageto.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    Ageto.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                    Ageto.BorderColor = BaseColor.WHITE;
                                    Ageto.FixedHeight = 15f;
                                    table.AddCell(Ageto);

                                    Temp_Agy_Codes = drAssoc["RNG4ASOC_AGYTAB_CODES"].ToString();

                                    //PrivAgyDesc = null;
                                    if (privTable != dr["TableUsed"].ToString())
                                    {
                                        if (!string.IsNullOrEmpty(dr["TableUsed"].ToString().Trim()))
                                        {
                                            PdfPCell TableUsed = new PdfPCell(new Phrase(dr["TableUsed"].ToString().Substring(0, 5).ToString(), TableFont));
                                            TableUsed.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            //TableUsed.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            TableUsed.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                            TableUsed.BorderColor = BaseColor.WHITE;
                                            TableUsed.FixedHeight = 15f;
                                            table.AddCell(TableUsed);

                                            PdfPCell TableUsed1 = new PdfPCell(new Phrase(dr["TableUsed"].ToString().Substring(6, 1).ToString(), TableFont));
                                            TableUsed1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            //TableUsed1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            TableUsed1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                            TableUsed1.BorderColor = BaseColor.WHITE;
                                            TableUsed1.FixedHeight = 15f;
                                            table.AddCell(TableUsed1);

                                            PdfPCell TableUsed2 = new PdfPCell(new Phrase(dr["TableUsed"].ToString().Substring(8, 1).ToString(), TableFont));
                                            TableUsed2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                            //TableUsed2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            TableUsed2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                            TableUsed2.BorderColor = BaseColor.WHITE;
                                            TableUsed2.FixedHeight = 15f;
                                            table.AddCell(TableUsed2);

                                            privTable = dr["TableUsed"].ToString();
                                        }
                                        else
                                        {
                                            PdfPCell TableUsed = new PdfPCell(new Phrase("", TableFont));
                                            TableUsed.HorizontalAlignment = Element.ALIGN_LEFT;
                                            TableUsed.Colspan = 3;
                                            //TableUsed.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            TableUsed.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                            TableUsed.BorderColor = BaseColor.WHITE;
                                            TableUsed.FixedHeight = 15f;
                                            table.AddCell(TableUsed);
                                        }
                                    }
                                    else
                                    {
                                        PdfPCell TableUsed = new PdfPCell(new Phrase("", TableFont));
                                        TableUsed.HorizontalAlignment = Element.ALIGN_LEFT;
                                        TableUsed.Colspan = 3;
                                        //TableUsed.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        TableUsed.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                        TableUsed.BorderColor = BaseColor.WHITE;
                                        TableUsed.FixedHeight = 15f;
                                        table.AddCell(TableUsed);
                                    }

                                    if (dr["RNG4CATG_CODE"].ToString() == "N")
                                    {
                                        foreach (AGYTABSEntity drAgency in AgyTabs_List)
                                        {
                                            bool Can_Continue = true;
                                            if (!string.IsNullOrEmpty(Temp_Agy_Codes) && Temp_Agy_Codes.Length >= 1)
                                            {
                                                for (int i = 0; Can_Continue;)
                                                {

                                                    if (Temp_Agy_Codes.Substring(i, Temp_Agy_Codes.Length - i).Length < 5)
                                                        Temp_Code = Temp_Agy_Codes.Substring(i, Temp_Agy_Codes.Length - i);
                                                    else
                                                        Temp_Code = Temp_Agy_Codes.Substring(i, 5);
                                                    if (drAgency.Table_Code.ToString().Trim() == Temp_Code.Trim())
                                                    {
                                                        if (!string.IsNullOrEmpty(AgycodesTemp.Trim()))
                                                        {
                                                            if (AgycodesTemp.Contains(drAgency.Table_Code.ToString().Trim()))
                                                            { }
                                                            else
                                                            {
                                                                if (NotAgycodes.Contains(drAgency.Table_Code.ToString().Trim()))
                                                                {
                                                                    NotAgycodes = NotAgycodes.Replace(drAgency.Table_Code.ToString().Trim(), "");
                                                                    AgycodesTemp += drAgency.Table_Code.ToString().Trim() + " ";
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (NotAgycodes.Contains(drAgency.Table_Code.ToString().Trim()))
                                                            {
                                                                NotAgycodes = NotAgycodes.Replace(drAgency.Table_Code.ToString().Trim(), "");
                                                                AgycodesTemp += drAgency.Table_Code.ToString().Trim() + " ";
                                                            }
                                                            else AgycodesTemp += drAgency.Table_Code.ToString().Trim() + " ";
                                                        }
                                                        PdfPCell Space = new PdfPCell(new Phrase("", TableFont));
                                                        Space.HorizontalAlignment = Element.ALIGN_LEFT;
                                                        //Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        Space.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                                        Space.BorderColor = BaseColor.WHITE;
                                                        Space.FixedHeight = 15f;
                                                        table.AddCell(Space);

                                                        PdfPCell Code = new PdfPCell(new Phrase("    " + drAgency.Table_Code.ToString().Trim() + "  " + drAgency.Code_Desc.ToString().Trim(), TableFont));
                                                        Code.HorizontalAlignment = Element.ALIGN_LEFT;
                                                        Code.Colspan = 6;
                                                        //Code.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        Code.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                        Code.BorderColor = BaseColor.WHITE;
                                                        Code.FixedHeight = 15f;
                                                        table.AddCell(Code);

                                                    }
                                                    else
                                                    {
                                                        if (!string.IsNullOrEmpty(NotAgycodes.Trim()))
                                                        {
                                                            if (NotAgycodes.Contains(drAgency.Table_Code.ToString().Trim()))
                                                            { }
                                                            else
                                                            {
                                                                if (AgycodesTemp.Contains(drAgency.Table_Code.ToString().Trim())) { }
                                                                else
                                                                    NotAgycodes += drAgency.Table_Code.ToString().Trim() + " ";
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (AgycodesTemp.Contains(drAgency.Table_Code.ToString().Trim())) { }
                                                            else
                                                                NotAgycodes += drAgency.Table_Code.ToString().Trim() + " ";
                                                        }
                                                    }

                                                    i += 5;
                                                    if (i >= Temp_Agy_Codes.Length)
                                                        Can_Continue = false;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        foreach (DataRow drAgyCodes in dtAgyCodes.Rows)
                                        {
                                            //bool Code_Exists = false;
                                            bool Can_Continue = true;
                                            if (!string.IsNullOrEmpty(Temp_Agy_Codes) && Temp_Agy_Codes.Length >= 1)
                                            {
                                                for (int i = 0; Can_Continue;)
                                                {

                                                    if (Temp_Agy_Codes.Substring(i, Temp_Agy_Codes.Length - i).Length < 5)
                                                        Temp_Code = Temp_Agy_Codes.Substring(i, Temp_Agy_Codes.Length - i);
                                                    else
                                                        Temp_Code = Temp_Agy_Codes.Substring(i, 5);
                                                    if (drAgyCodes["Code"].ToString().Trim() == Temp_Code.Trim())
                                                    {
                                                        if (!string.IsNullOrEmpty(AgycodesTemp.Trim()))
                                                        {
                                                            if (AgycodesTemp.Contains(drAgyCodes["Code"].ToString().Trim()))
                                                            { }
                                                            else
                                                            {
                                                                if (NotAgycodes.Contains(drAgyCodes["Code"].ToString().Trim()))
                                                                {
                                                                    NotAgycodes = NotAgycodes.Replace(drAgyCodes["Code"].ToString().Trim(), "");
                                                                    AgycodesTemp += drAgyCodes["Code"].ToString().Trim() + " ";
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (NotAgycodes.Contains(drAgyCodes["Code"].ToString().Trim()))
                                                            {
                                                                NotAgycodes = NotAgycodes.Replace(drAgyCodes["Code"].ToString().Trim(), "");
                                                                AgycodesTemp += drAgyCodes["Code"].ToString().Trim() + " ";
                                                            }
                                                            else AgycodesTemp += drAgyCodes["Code"].ToString().Trim() + " ";
                                                        }
                                                        PdfPCell Space = new PdfPCell(new Phrase("", TableFont));
                                                        Space.HorizontalAlignment = Element.ALIGN_LEFT;
                                                        //Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        Space.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                                        Space.BorderColor = BaseColor.WHITE;
                                                        Space.FixedHeight = 15f;
                                                        table.AddCell(Space);

                                                        PdfPCell Code = new PdfPCell(new Phrase("    " + drAgyCodes["Code"].ToString().Trim() + "  " + drAgyCodes["LookUpDesc"].ToString().Trim(), TableFont));
                                                        Code.HorizontalAlignment = Element.ALIGN_LEFT;
                                                        Code.Colspan = 6;
                                                        //Code.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        Code.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                        Code.BorderColor = BaseColor.WHITE;
                                                        Code.FixedHeight = 15f;
                                                        table.AddCell(Code);

                                                    }
                                                    else
                                                    {
                                                        if (!string.IsNullOrEmpty(NotAgycodes.Trim()))
                                                        {
                                                            if (NotAgycodes.Contains(drAgyCodes["Code"].ToString().Trim()))
                                                            { }
                                                            else
                                                            {
                                                                if (AgycodesTemp.Contains(drAgyCodes["Code"].ToString().Trim())) { }
                                                                else
                                                                    NotAgycodes += drAgyCodes["Code"].ToString().Trim() + " ";
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (AgycodesTemp.Contains(drAgyCodes["Code"].ToString().Trim())) { }
                                                            else
                                                                NotAgycodes += drAgyCodes["Code"].ToString().Trim() + " ";
                                                        }
                                                    }

                                                    i += 5;
                                                    if (i >= Temp_Agy_Codes.Length)
                                                        Can_Continue = false;
                                                }
                                            }
                                        }
                                    }
                                    //PdfPCell Space3 = new PdfPCell(new Phrase("", TableFont));
                                    //Space3.HorizontalAlignment = Element.ALIGN_LEFT;
                                    //Space3.Colspan = 7;
                                    //Space3.FixedHeight = 15f;
                                    //Space3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                    //Space3.BorderColor = BaseColor.WHITE;
                                    //table.AddCell(Space3);
                                    First = false;
                                }

                            }
                        }
                        else
                        {
                            if (dr["RNG4CATG_CODE"].ToString() == "I")
                            {
                                DataTable SrcInc = dscategories.Tables[0];
                                DataView dv = new DataView(SrcInc);
                                dv.RowFilter = "RNG4CATG_CODE='I'AND RNG4CATG_DEM<>''";// AND RNG4CATG_DEM<>'a' AND RNG4CATG_DEM<>'g' AND RNG4CATG_DEM<>'i'";
                                SrcInc = dv.ToTable();

                                List<AGYTABSEntity> AgyTabs_Incomes = new List<AGYTABSEntity>();
                                List<AGYTABSEntity> AgyTabs_NCB = new List<AGYTABSEntity>();
                                AGYTABSEntity searchAgytabs = new AGYTABSEntity(true);
                                searchAgytabs.Tabs_Type = "00004";
                                AgyTabs_Incomes = _model.AdhocData.Browse_AGYTABS(searchAgytabs);

                                AGYTABSEntity searchAgyNCB = new AGYTABSEntity(true);
                                searchAgyNCB.Tabs_Type = "00037";
                                AgyTabs_NCB = _model.AdhocData.Browse_AGYTABS(searchAgyNCB);

                                foreach (DataRow drsrcinc in SrcInc.Rows)
                                {
                                    if (drsrcinc["RNG4CATG_DEM"].ToString().Trim() == "a" || drsrcinc["RNG4CATG_DEM"].ToString().Trim() == "g" || drsrcinc["RNG4CATG_DEM"].ToString().Trim() == "i")
                                    {
                                        DataTable dtsrcAsc = dtAssoc;
                                        DataView dvsrc = new DataView(dtsrcAsc);
                                        dvsrc.RowFilter = "RNG4ASOC_CATG_CODE='I' AND RNG4ASOC_DEMO_CODE='" + drsrcinc["RNG4CATG_DEM"].ToString().Trim() + "'";
                                        dtsrcAsc = dvsrc.ToTable();

                                        foreach (DataRow drAssoc in dtsrcAsc.Rows)
                                        {
                                            if (drAssoc["RNG4ASOC_CATG_CODE"].ToString() == dr["RNG4CATG_CODE"].ToString())
                                            {
                                                if (!First)
                                                {
                                                    PdfPCell AGY_DESC = new PdfPCell(new Phrase("", TableFont));
                                                    AGY_DESC.HorizontalAlignment = Element.ALIGN_LEFT;
                                                    //AGY_DESC.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                    AGY_DESC.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                                    AGY_DESC.BorderColor = BaseColor.WHITE;
                                                    AGY_DESC.FixedHeight = 15f;
                                                    table.AddCell(AGY_DESC);
                                                }

                                                PdfPCell CSB4CATG_DESC = new PdfPCell(new Phrase(drAssoc["RNG4CATG_DESC"].ToString().Trim(), TableFont));
                                                CSB4CATG_DESC.HorizontalAlignment = Element.ALIGN_LEFT;
                                                //CSB4CATG_DESC.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                CSB4CATG_DESC.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                                CSB4CATG_DESC.BorderColor = BaseColor.WHITE;
                                                CSB4CATG_DESC.FixedHeight = 15f;
                                                table.AddCell(CSB4CATG_DESC);

                                                PdfPCell AgeFrom = new PdfPCell(new Phrase(drAssoc["RNG4ASOC_AGE_FROM"].ToString().Trim(), TableFont));
                                                AgeFrom.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                //AgeFrom.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                AgeFrom.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                                AgeFrom.BorderColor = BaseColor.WHITE;
                                                AgeFrom.FixedHeight = 15f;
                                                table.AddCell(AgeFrom);

                                                PdfPCell Ageto = new PdfPCell(new Phrase(drAssoc["RNG4ASOC_AGE_TO"].ToString().Trim(), TableFont));
                                                Ageto.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                //Ageto.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                Ageto.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                                Ageto.BorderColor = BaseColor.WHITE;
                                                Ageto.FixedHeight = 15f;
                                                table.AddCell(Ageto);

                                                Temp_Agy_Codes = drAssoc["RNG4ASOC_AGYTAB_CODES"].ToString();

                                                //PrivAgyDesc = null;
                                                if (privTable != dr["TableUsed"].ToString())
                                                {
                                                    if (!string.IsNullOrEmpty(dr["TableUsed"].ToString().Trim()))
                                                    {
                                                        PdfPCell TableUsed = new PdfPCell(new Phrase(dr["TableUsed"].ToString().Substring(0, 5).ToString(), TableFont));
                                                        TableUsed.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        //TableUsed.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        TableUsed.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                                        TableUsed.BorderColor = BaseColor.WHITE;
                                                        TableUsed.FixedHeight = 15f;
                                                        table.AddCell(TableUsed);

                                                        PdfPCell TableUsed1 = new PdfPCell(new Phrase(dr["TableUsed"].ToString().Substring(6, 1).ToString(), TableFont));
                                                        TableUsed1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        //TableUsed1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        TableUsed1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                                        TableUsed1.BorderColor = BaseColor.WHITE;
                                                        TableUsed1.FixedHeight = 15f;
                                                        table.AddCell(TableUsed1);

                                                        PdfPCell TableUsed2 = new PdfPCell(new Phrase(dr["TableUsed"].ToString().Substring(8, 1).ToString(), TableFont));
                                                        TableUsed2.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                        //TableUsed2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        TableUsed2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                                        TableUsed2.BorderColor = BaseColor.WHITE;
                                                        TableUsed2.FixedHeight = 15f;
                                                        table.AddCell(TableUsed2);

                                                        privTable = dr["TableUsed"].ToString();
                                                    }
                                                    else
                                                    {
                                                        PdfPCell TableUsed = new PdfPCell(new Phrase("", TableFont));
                                                        TableUsed.HorizontalAlignment = Element.ALIGN_LEFT;
                                                        TableUsed.Colspan = 3;
                                                        //TableUsed.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                        TableUsed.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                                        TableUsed.BorderColor = BaseColor.WHITE;
                                                        TableUsed.FixedHeight = 15f;
                                                        table.AddCell(TableUsed);
                                                    }
                                                }
                                                else
                                                {
                                                    PdfPCell TableUsed = new PdfPCell(new Phrase("", TableFont));
                                                    TableUsed.HorizontalAlignment = Element.ALIGN_LEFT;
                                                    TableUsed.Colspan = 3;
                                                    //TableUsed.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                    TableUsed.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                                    TableUsed.BorderColor = BaseColor.WHITE;
                                                    TableUsed.FixedHeight = 15f;
                                                    table.AddCell(TableUsed);
                                                }

                                                foreach (DataRow drAgyCodes in dtAgyCodes.Rows)
                                                {
                                                    //bool Code_Exists = false;
                                                    bool Can_Continue = true;
                                                    if (!string.IsNullOrEmpty(Temp_Agy_Codes) && Temp_Agy_Codes.Length >= 1)
                                                    {
                                                        for (int i = 0; Can_Continue;)
                                                        {

                                                            if (Temp_Agy_Codes.Substring(i, Temp_Agy_Codes.Length - i).Length < 5)
                                                                Temp_Code = Temp_Agy_Codes.Substring(i, Temp_Agy_Codes.Length - i);
                                                            else
                                                                Temp_Code = Temp_Agy_Codes.Substring(i, 5);
                                                            if (drAgyCodes["Code"].ToString().Trim() == Temp_Code.Trim())
                                                            {
                                                                if (!string.IsNullOrEmpty(AgycodesTemp.Trim()))
                                                                {
                                                                    if (AgycodesTemp.Contains(drAgyCodes["Code"].ToString().Trim()))
                                                                    { }
                                                                    else
                                                                    {
                                                                        if (NotAgycodes.Contains(drAgyCodes["Code"].ToString().Trim()))
                                                                        {
                                                                            NotAgycodes = NotAgycodes.Replace(drAgyCodes["Code"].ToString().Trim(), "");
                                                                            AgycodesTemp += drAgyCodes["Code"].ToString().Trim() + " ";
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (NotAgycodes.Contains(drAgyCodes["Code"].ToString().Trim()))
                                                                    {
                                                                        NotAgycodes = NotAgycodes.Replace(drAgyCodes["Code"].ToString().Trim(), "");
                                                                        AgycodesTemp += drAgyCodes["Code"].ToString().Trim() + " ";
                                                                    }
                                                                    else AgycodesTemp += drAgyCodes["Code"].ToString().Trim() + " ";
                                                                }
                                                                PdfPCell Space = new PdfPCell(new Phrase("", TableFont));
                                                                Space.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                //Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                Space.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                                                Space.BorderColor = BaseColor.WHITE;
                                                                Space.FixedHeight = 15f;
                                                                table.AddCell(Space);

                                                                PdfPCell Code = new PdfPCell(new Phrase("    " + drAgyCodes["Code"].ToString().Trim() + "  " + drAgyCodes["LookUpDesc"].ToString().Trim(), TableFont));
                                                                Code.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                Code.Colspan = 6;
                                                                //Code.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                Code.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                                Code.BorderColor = BaseColor.WHITE;
                                                                Code.FixedHeight = 15f;
                                                                table.AddCell(Code);

                                                            }
                                                            else
                                                            {
                                                                if (!string.IsNullOrEmpty(NotAgycodes.Trim()))
                                                                {
                                                                    if (NotAgycodes.Contains(drAgyCodes["Code"].ToString().Trim()))
                                                                    { }
                                                                    else
                                                                    {
                                                                        if (AgycodesTemp.Contains(drAgyCodes["Code"].ToString().Trim())) { }
                                                                        else
                                                                            NotAgycodes += drAgyCodes["Code"].ToString().Trim() + " ";
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (AgycodesTemp.Contains(drAgyCodes["Code"].ToString().Trim())) { }
                                                                    else
                                                                        NotAgycodes += drAgyCodes["Code"].ToString().Trim() + " ";
                                                                }
                                                            }

                                                            i += 5;
                                                            if (i >= Temp_Agy_Codes.Length)
                                                                Can_Continue = false;
                                                        }
                                                    }
                                                }

                                                //PdfPCell Space3 = new PdfPCell(new Phrase("", TableFont));
                                                //Space3.HorizontalAlignment = Element.ALIGN_LEFT;
                                                //Space3.Colspan = 7;
                                                //Space3.FixedHeight = 15f;
                                                ////Space3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                //Space3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                //Space3.BorderColor = BaseColor.WHITE;
                                                //table.AddCell(Space3);
                                                First = false;

                                            }
                                        }
                                    }
                                    else
                                    {
                                        PdfPCell AGY_DESC = new PdfPCell(new Phrase("", TableFont));
                                        AGY_DESC.HorizontalAlignment = Element.ALIGN_LEFT;
                                        //AGY_DESC.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        AGY_DESC.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                        AGY_DESC.BorderColor = BaseColor.WHITE;
                                        AGY_DESC.FixedHeight = 15f;
                                        table.AddCell(AGY_DESC);

                                        PdfPCell CSB4CATG_DESC = new PdfPCell(new Phrase(drsrcinc["RNG4CATG_DESC"].ToString().Trim(), TableFont));
                                        CSB4CATG_DESC.HorizontalAlignment = Element.ALIGN_LEFT;
                                        //CSB4CATG_DESC.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        CSB4CATG_DESC.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                        CSB4CATG_DESC.BorderColor = BaseColor.WHITE;
                                        CSB4CATG_DESC.FixedHeight = 15;
                                        table.AddCell(CSB4CATG_DESC);

                                        PdfPCell AgeFrom = new PdfPCell(new Phrase("0", TableFont));
                                        AgeFrom.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        //AgeFrom.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        AgeFrom.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                        AgeFrom.BorderColor = BaseColor.WHITE;
                                        AgeFrom.FixedHeight = 15f;
                                        table.AddCell(AgeFrom);

                                        PdfPCell Ageto = new PdfPCell(new Phrase("999", TableFont));
                                        Ageto.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        //Ageto.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        Ageto.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                        Ageto.BorderColor = BaseColor.WHITE;
                                        Ageto.FixedHeight = 15f;
                                        table.AddCell(Ageto);

                                        PdfPCell TableUsed = new PdfPCell(new Phrase("", TableFont));
                                        TableUsed.HorizontalAlignment = Element.ALIGN_LEFT;
                                        TableUsed.Colspan = 3;
                                        //TableUsed.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        TableUsed.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                        TableUsed.BorderColor = BaseColor.WHITE;
                                        TableUsed.FixedHeight = 15f;
                                        table.AddCell(TableUsed);


                                        if (drsrcinc["RNG4CATG_DEM"].ToString().Trim() == "b" || drsrcinc["RNG4CATG_DEM"].ToString().Trim() == "c" || drsrcinc["RNG4CATG_DEM"].ToString().Trim() == "d")
                                        {
                                            if (!string.IsNullOrEmpty(EMPCodes.Trim()))
                                            {
                                                PdfPCell AGY_DESC1 = new PdfPCell(new Phrase("", TableFont));
                                                AGY_DESC1.HorizontalAlignment = Element.ALIGN_LEFT;
                                                //AGY_DESC1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                AGY_DESC1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                                AGY_DESC1.BorderColor = BaseColor.WHITE;
                                                AGY_DESC1.FixedHeight = 15f;
                                                table.AddCell(AGY_DESC1);

                                                PdfPCell Space = new PdfPCell(new Phrase("Income for Employment", MainHeaderLine));
                                                Space.HorizontalAlignment = Element.ALIGN_LEFT;
                                                Space.Colspan = 6;
                                                //Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                Space.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                                Space.BorderColor = BaseColor.WHITE;
                                                Space.FixedHeight = 15;
                                                table.AddCell(Space);



                                                string[] Temp_Codes = EMPCodes.Split(',');

                                                foreach (AGYTABSEntity drAgency in AgyTabs_Incomes)
                                                {

                                                    if (Temp_Codes != null)
                                                    {
                                                        for (int i = 0; i < Temp_Codes.Length; i++)
                                                        {
                                                            if (drAgency.Table_Code.ToString().Trim() == Temp_Codes[i].Trim())
                                                            {
                                                                PdfPCell Space1 = new PdfPCell(new Phrase("", TableFont));
                                                                Space1.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                Space1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                                                Space1.BorderColor = BaseColor.WHITE;
                                                                Space1.FixedHeight = 15f;
                                                                table.AddCell(Space1);

                                                                PdfPCell Code = new PdfPCell(new Phrase("    " + drAgency.Table_Code.ToString().Trim() + "  " + drAgency.Code_Desc.ToString().Trim(), TableFont));
                                                                Code.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                Code.Colspan = 6;
                                                                //Code.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                Code.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                                Code.BorderColor = BaseColor.WHITE;
                                                                Code.FixedHeight = 15f;
                                                                table.AddCell(Code);
                                                            }
                                                        }
                                                    }

                                                }

                                            }

                                        }

                                        if (drsrcinc["RNG4CATG_DEM"].ToString().Trim() == "b" || drsrcinc["RNG4CATG_DEM"].ToString().Trim() == "c" || drsrcinc["RNG4CATG_DEM"].ToString().Trim() == "e" || drsrcinc["RNG4CATG_DEM"].ToString().Trim() == "f")
                                        {
                                            if (!string.IsNullOrEmpty(IncCodes.Trim()))
                                            {
                                                if (drsrcinc["RNG4CATG_DEM"].ToString().Trim() != "e")
                                                {
                                                    PdfPCell AGY_DESC1 = new PdfPCell(new Phrase("", TableFont));
                                                    AGY_DESC1.HorizontalAlignment = Element.ALIGN_LEFT;
                                                    //AGY_DESC1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                    AGY_DESC1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                                    AGY_DESC1.BorderColor = BaseColor.WHITE;
                                                    AGY_DESC1.FixedHeight = 15f;
                                                    table.AddCell(AGY_DESC1);

                                                    PdfPCell Space = new PdfPCell(new Phrase("Other Income Sources", MainHeaderLine));
                                                    Space.HorizontalAlignment = Element.ALIGN_LEFT;
                                                    Space.Colspan = 6;
                                                    //Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                    Space.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                                    Space.BorderColor = BaseColor.WHITE;
                                                    Space.FixedHeight = 15;
                                                    table.AddCell(Space);
                                                }
                                                string[] Temp_Codes = IncCodes.Split(',');

                                                foreach (AGYTABSEntity drAgency in AgyTabs_Incomes)
                                                {

                                                    if (Temp_Codes != null)
                                                    {
                                                        for (int i = 0; i < Temp_Codes.Length; i++)
                                                        {
                                                            if (drAgency.Table_Code.ToString().Trim() == Temp_Codes[i].Trim())
                                                            {
                                                                PdfPCell Space = new PdfPCell(new Phrase("", TableFont));
                                                                Space.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                //Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                Space.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                                                Space.BorderColor = BaseColor.WHITE;
                                                                Space.FixedHeight = 15f;
                                                                table.AddCell(Space);

                                                                PdfPCell Code = new PdfPCell(new Phrase("    " + drAgency.Table_Code.ToString().Trim() + "  " + drAgency.Code_Desc.ToString().Trim(), TableFont));
                                                                Code.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                Code.Colspan = 6;
                                                                //Code.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                Code.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                                Code.BorderColor = BaseColor.WHITE;
                                                                Code.FixedHeight = 15f;
                                                                table.AddCell(Code);
                                                            }
                                                        }
                                                    }

                                                }

                                            }

                                        }

                                        if (drsrcinc["RNG4CATG_DEM"].ToString().Trim() == "d" || drsrcinc["RNG4CATG_DEM"].ToString().Trim() == "c" || drsrcinc["RNG4CATG_DEM"].ToString().Trim() == "h" || drsrcinc["RNG4CATG_DEM"].ToString().Trim() == "f")
                                        {
                                            if (!string.IsNullOrEmpty(NCSCodes.Trim()))
                                            {
                                                if (drsrcinc["RNG4CATG_DEM"].ToString().Trim() != "h")
                                                {
                                                    PdfPCell AGY_DESC1 = new PdfPCell(new Phrase("", TableFont));
                                                    AGY_DESC1.HorizontalAlignment = Element.ALIGN_LEFT;
                                                    //AGY_DESC1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                    AGY_DESC1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                                    AGY_DESC1.BorderColor = BaseColor.WHITE;
                                                    AGY_DESC1.FixedHeight = 15f;
                                                    table.AddCell(AGY_DESC1);

                                                    PdfPCell Space = new PdfPCell(new Phrase("Non-Cash Benefits", MainHeaderLine));
                                                    Space.HorizontalAlignment = Element.ALIGN_LEFT;
                                                    Space.Colspan = 6;
                                                    //Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                    Space.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                    Space.BorderColor = BaseColor.WHITE;
                                                    Space.FixedHeight = 15;
                                                    table.AddCell(Space);
                                                }

                                                string[] Temp_Codes = NCSCodes.Split(',');

                                                foreach (AGYTABSEntity drAgency in AgyTabs_NCB)
                                                {

                                                    if (Temp_Codes != null)
                                                    {
                                                        for (int i = 0; i < Temp_Codes.Length; i++)
                                                        {
                                                            if (drAgency.Table_Code.ToString().Trim() == Temp_Codes[i].Trim())
                                                            {
                                                                PdfPCell Space = new PdfPCell(new Phrase("", TableFont));
                                                                Space.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                //Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                Space.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                                                Space.BorderColor = BaseColor.WHITE;
                                                                Space.FixedHeight = 15f;
                                                                table.AddCell(Space);

                                                                PdfPCell Code = new PdfPCell(new Phrase("    " + drAgency.Table_Code.ToString().Trim() + "  " + drAgency.Code_Desc.ToString().Trim(), TableFont));
                                                                Code.HorizontalAlignment = Element.ALIGN_LEFT;
                                                                Code.Colspan = 6;
                                                                //Code.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                                Code.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                                                                Code.BorderColor = BaseColor.WHITE;
                                                                Code.FixedHeight = 15f;
                                                                table.AddCell(Code);
                                                            }
                                                        }
                                                    }

                                                }

                                            }
  
                                        }
                                        //PdfPCell Space3 = new PdfPCell(new Phrase("", TableFont));
                                        //Space3.HorizontalAlignment = Element.ALIGN_LEFT;
                                        //Space3.Colspan = 7;
                                        //Space3.FixedHeight = 15f;
                                        ////Space3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        //Space3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                        //Space3.BorderColor = BaseColor.WHITE;
                                        //table.AddCell(Space3);
                                    }

                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(NotAgycodes.Trim()))
                        {
                            PdfPCell Space = new PdfPCell(new Phrase("", TableFont));
                            Space.HorizontalAlignment = Element.ALIGN_LEFT;
                            //Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            Space.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                            Space.BorderColor = BaseColor.WHITE;
                            Space.FixedHeight = 15f;
                            table.AddCell(Space);

                            PdfPCell Code = new PdfPCell(new Phrase("Not Assoc  ", TableFont));
                            Code.HorizontalAlignment = Element.ALIGN_LEFT;
                            Code.Colspan = 6;
                            //Code.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            Code.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                            Code.BorderColor = BaseColor.WHITE;
                            Code.FixedHeight = 15f;
                            table.AddCell(Code);
                            string[] tmpCd = NotAgycodes.Split(' ');
                            for (int i = 0; i < tmpCd.Length; i++)
                            {
                                if (!string.IsNullOrEmpty(tmpCd[i].ToString()))
                                {
                                    if (dr["RNG4CATG_CODE"].ToString() == "N")
                                    {
                                        AGYTABSEntity row = AgyTabs_List.Find(u => u.Table_Code.ToString().Trim().Equals(tmpCd[i].ToString().Trim()));
                                        if (row != null)
                                        {
                                            PdfPCell Space1 = new PdfPCell(new Phrase(" ", TableFont));
                                            Space1.HorizontalAlignment = Element.ALIGN_LEFT;
                                            //Space1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            Space1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                            Space1.BorderColor = BaseColor.WHITE;
                                            Space1.FixedHeight = 15f;
                                            table.AddCell(Space1);

                                            PdfPCell Code2 = new PdfPCell(new Phrase("    " + row.Table_Code.ToString().Trim() + "  " + row.Code_Desc.ToString().Trim(), TableFont));
                                            Code2.HorizontalAlignment = Element.ALIGN_LEFT;
                                            Code2.Colspan = 6;
                                            //Code2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            Code2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                            Code2.BorderColor = BaseColor.WHITE;
                                            Code2.FixedHeight = 15f;
                                            table.AddCell(Code2);
                                        }
                                    }
                                    else
                                    {
                                        DataRow[] row = dtAgyCodes.Select("Code='" + tmpCd[i].ToString() + "'");
                                        if (row != null && row.Length > 0)
                                        {
                                            PdfPCell Space1 = new PdfPCell(new Phrase(" ", TableFont));
                                            Space1.HorizontalAlignment = Element.ALIGN_LEFT;
                                            //Space1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            Space1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                            Space1.BorderColor = BaseColor.WHITE;
                                            Space1.FixedHeight = 15f;
                                            table.AddCell(Space1);

                                            PdfPCell Code2 = new PdfPCell(new Phrase("    " + row[0]["Code"].ToString().Trim() + "  " + row[0]["LookUpDesc"].ToString().Trim(), TableFont));
                                            Code2.HorizontalAlignment = Element.ALIGN_LEFT;
                                            Code2.Colspan = 6;
                                            //Code2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            Code2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                            Code2.BorderColor = BaseColor.WHITE;
                                            Code2.FixedHeight = 15f;
                                            table.AddCell(Code2);
                                        }
                                    }
                                }
                            }

                            //PdfPCell Space3 = new PdfPCell(new Phrase("", TableFont));
                            //Space3.HorizontalAlignment = Element.ALIGN_LEFT;
                            //Space3.Colspan = 7;
                            //Space3.FixedHeight = 15f;
                            ////Space3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            //Space3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                            //Space3.BorderColor = BaseColor.WHITE;
                            //table.AddCell(Space3);
                            First = false;
                        }
                        else if (string.IsNullOrEmpty(AgycodesTemp.Trim()))
                        {
                            PdfPCell Space = new PdfPCell(new Phrase(" ", TableFont));
                            Space.HorizontalAlignment = Element.ALIGN_LEFT;
                            //Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            Space.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                            Space.BorderColor = BaseColor.WHITE;
                            Space.FixedHeight = 15f;
                            table.AddCell(Space);

                            PdfPCell Code = new PdfPCell(new Phrase("Not Assoc  ", TableFont));
                            Code.HorizontalAlignment = Element.ALIGN_LEFT;
                            Code.Colspan = 6;
                            //Code.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            Code.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                            Code.BorderColor = BaseColor.WHITE;
                            Code.FixedHeight = 15f;
                            table.AddCell(Code);
                            //string[] tmpCd = NotAgycodes.Split(' ');
                            if (dr["RNG4CATG_CODE"].ToString() == "N")
                            {
                                if (AgyTabs_List.Count > 0)
                                {
                                    foreach (AGYTABSEntity row in AgyTabs_List)
                                    {
                                        PdfPCell Space1 = new PdfPCell(new Phrase(" ", TableFont));
                                        Space1.HorizontalAlignment = Element.ALIGN_LEFT;
                                        //Space1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        Space1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                        Space1.BorderColor = BaseColor.WHITE;
                                        Space1.FixedHeight = 15f;
                                        table.AddCell(Space1);

                                        PdfPCell Code2 = new PdfPCell(new Phrase("    " + row.Table_Code.ToString().Trim() + "  " + row.Code_Desc.ToString().Trim(), TableFont));
                                        Code2.HorizontalAlignment = Element.ALIGN_LEFT;
                                        Code2.Colspan = 6;
                                        //Code2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        Code2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                        Code2.BorderColor = BaseColor.WHITE;
                                        Code2.FixedHeight = 15f;
                                        table.AddCell(Code2);
                                    }
                                }
                            }
                            else
                            {
                                if (dtAgyCodes.Rows.Count > 0)
                                {
                                    foreach (DataRow row in dtAgyCodes.Rows)
                                    {
                                        PdfPCell Space1 = new PdfPCell(new Phrase(" ", TableFont));
                                        Space1.HorizontalAlignment = Element.ALIGN_LEFT;
                                        //Space1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        Space1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                        Space1.BorderColor = BaseColor.WHITE;
                                        Space1.FixedHeight = 15f;
                                        table.AddCell(Space1);

                                        PdfPCell Code2 = new PdfPCell(new Phrase("    " + row["Code"].ToString().Trim() + "  " + row["LookUpDesc"].ToString().Trim(), TableFont));
                                        Code2.HorizontalAlignment = Element.ALIGN_LEFT;
                                        Code2.Colspan = 6;
                                        //Code2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        Code2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                        Code2.BorderColor = BaseColor.WHITE;
                                        Code2.FixedHeight = 15f;
                                        table.AddCell(Code2);
                                    }
                                }
                            }

                            PdfPCell Space3 = new PdfPCell(new Phrase("", TableFont));
                            Space3.HorizontalAlignment = Element.ALIGN_LEFT;
                            Space3.Colspan = 7;
                            Space3.FixedHeight = 15f;
                            //Space3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            Space3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                            Space3.BorderColor = BaseColor.WHITE;
                            table.AddCell(Space3);
                            //PrintRec(" ", 150);
                            First = false;
                        }
                    }
                    document.Add(table);
                }
            }
            catch (Exception ex) { document.Add(new Paragraph("Aborted due to Exception............................................... ")); }

        }

        private void RNGPerformanceMeasures(Document document, PdfWriter writer)
        {
            try
            {
                string empty = " ";
                DataSet ds = DatabaseLayer.SPAdminDB.Get_RNGGRP(((RepListItem)CmbAgyTab.SelectedItem).Value.ToString(), ((RepListItem)CmbAgyTab.SelectedItem).ValueDisplayCode.ToString(), null, null, null, BaseForm.UserID, BaseForm.BaseAdminAgency);
                DataTable dt = ds.Tables[0];
                DataView dvgrp = new DataView(dt);
                dvgrp.Sort = "RNGGRP_GROUP_CODE";//+string.Empty+"'";
                dt = dvgrp.ToTable();

                BaseFont bf_Calibri = BaseFont.CreateFont("c:/windows/fonts/calibri.ttf", BaseFont.WINANSI, BaseFont.EMBEDDED);
                iTextSharp.text.Font Calibri = new iTextSharp.text.Font(bf_Calibri, 10);
                iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_Calibri, 8);
                iTextSharp.text.Font SubHeadFont = new iTextSharp.text.Font(bf_Calibri, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_Calibri, 9, 3);
                iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_Calibri, 8, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
                iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_Calibri, 9, 2);
                iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_Calibri, 9, 4);
                iTextSharp.text.Font TblFontBoldColor = new iTextSharp.text.Font(bf_Calibri, 9, 1, BaseColor.BLACK);

                //BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
                //iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 10);
                //iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 10);
                //iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 9, 3);
                //iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(1, 10, 1);
                //iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 9, 2);
                //iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 9, 4);
                //iTextSharp.text.Font TblFontBoldColor = new iTextSharp.text.Font(bf_times, 11, 1, BaseColor.BLUE);

                //DataSet dsCsbRa = DatabaseLayer.SPAdminDB.Get_CSB14RA(gvDateRange.CurrentRow.Cells["From_Date"].Value.ToString(), gvDateRange.CurrentRow.Cells["To_Date"].Value.ToString(), null, null);
                //DataTable dtCsbRa = dsCsbRa.Tables[0];

                List<MSMASTEntity> MSList = _model.SPAdminData.Browse_MSMAST(null, null, null, null, null);

                if (dt.Rows.Count > 0)
                {
                    PdfPTable table = new PdfPTable(6);
                    table.TotalWidth = 540f;
                    table.WidthPercentage = 100;
                    table.LockedWidth = true;
                    float[] widths = new float[] { 15f, 13f, 75f, 6f, 12f, 8f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                    table.SetWidths(widths);
                    table.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.HeaderRows = 2;

                    PdfPCell Hdr = new PdfPCell(new Phrase("Performance Measures Association", TblFontBoldColor));
                    Hdr.HorizontalAlignment = Element.ALIGN_CENTER;
                    Hdr.FixedHeight = 15f;
                    Hdr.Colspan = 6;
                    Hdr.PaddingBottom = 5f;
                    Hdr.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    table.AddCell(Hdr);

                    PdfPCell ref_fdate = new PdfPCell(new Phrase(((RepListItem)CmbAgyTab.SelectedItem).ID.ToString(), TblFontBoldColor));
                    ref_fdate.HorizontalAlignment = Element.ALIGN_CENTER;
                    ref_fdate.Colspan = 6;
                    ref_fdate.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    ref_fdate.PaddingBottom = 10f;
                    table.AddCell(ref_fdate);


                    string UsedGoals = string.Empty;
                    string temp_code = null; string AgyGoal_code = null; int j = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        string Grp_List = null; string group = null;

                        if (!string.IsNullOrEmpty(dr["RNGGRP_group_code"].ToString().Trim()) && string.IsNullOrEmpty(dr["RNGGRP_TABLE_CODE"].ToString().Trim()))
                        {
                            //***Vikash Commented
                            //PdfPCell Space_code = new PdfPCell(new Phrase("", TblFontBold));
                            //Space_code.HorizontalAlignment = Element.ALIGN_LEFT;
                            //Space_code.Colspan = 6;
                            //Space_code.FixedHeight = 10f;
                            //Space_code.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            //table.AddCell(Space_code);

                            PdfPCell group_code = new PdfPCell(new Phrase("Group : " + dr["RNGGRP_group_code"].ToString().Trim(), TblFontBold));
                            group_code.HorizontalAlignment = Element.ALIGN_LEFT;
                            group_code.Colspan = 2;
                            //group_code.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER;
                            group_code.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                            group_code.BorderColor = BaseColor.WHITE;
                            //group_code.FixedHeight = 15f;
                            table.AddCell(group_code);

                            PdfPCell group_desc = new PdfPCell(new Phrase(dr["RNGGRP_desc"].ToString().Trim(), TblFontBold));
                            group_desc.HorizontalAlignment = Element.ALIGN_LEFT;
                            //group_desc.Colspan = 2;
                            //group_desc.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER;
                            group_desc.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                            group_desc.BorderColor = BaseColor.WHITE;
                            //group_desc.FixedHeight = 15f;
                            table.AddCell(group_desc);

                            PdfPCell group_Space = new PdfPCell(new Phrase("", TableFont));
                            group_Space.HorizontalAlignment = Element.ALIGN_LEFT;
                            group_Space.Colspan = 3;
                            //group_Space.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER;
                            group_Space.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                            group_Space.BorderColor = BaseColor.WHITE;
                           // group_Space.FixedHeight = 15f;
                            table.AddCell(group_Space);


                            //PdfPCell cell1 = new PdfPCell(new Phrase("Headers & Results", TblFontBold));
                            //cell1.HorizontalAlignment = Element.ALIGN_LEFT;
                            //cell1.FixedHeight = 15f;
                            //cell1.Colspan = 6;
                            //cell1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            //table.AddCell(cell1);

                            group = dr["RNGGRP_GROUP_CODE"].ToString();
                            string RefFdate = dr["RNGGRP_CODE"].ToString();
                            string RefTdate = dr["RNGGRP_AGENCY"].ToString();
                            //if (!string.IsNullOrEmpty(dr["RNGGRP_cnt_hdr1"].ToString()) && dr["RNGGRP_cnt_incld1"].ToString() == "True")
                            //    Grp_List = "1";
                            //if (!string.IsNullOrEmpty(dr["RNGGRP_cnt_hdr2"].ToString()) && dr["RNGGRP_cnt_incld2"].ToString() == "True")
                            //    Grp_List += "2";
                            //if (!string.IsNullOrEmpty(dr["RNGGRP_cnt_hdr3"].ToString()) && dr["RNGGRP_cnt_incld3"].ToString() == "True")
                            //    Grp_List += "3";
                            //if (!string.IsNullOrEmpty(dr["RNGGRP_cnt_hdr4"].ToString()) && dr["RNGGRP_cnt_incld4"].ToString() == "True")
                            //    Grp_List += "4";
                            //if (!string.IsNullOrEmpty(dr["RNGGRP_cnt_hdr5"].ToString()) && dr["RNGGRP_cnt_incld5"].ToString() == "True")
                            //    Grp_List += "5";

                            //for (int i = 0; i < Grp_List.Length; i++)
                            //{
                            //    if (Grp_List.Substring(i, 1) == "1")
                            //    {
                            //        PdfPCell hdr1 = new PdfPCell(new Phrase(dr["RNGGRP_cnt_hdr1"].ToString().Trim(), TableFont));
                            //        hdr1.HorizontalAlignment = Element.ALIGN_LEFT;
                            //        hdr1.Colspan = 6;
                            //        hdr1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            //        table.AddCell(hdr1);
                            //    }
                            //    else if (Grp_List.Substring(i, 1) == "2")
                            //    {
                            //        PdfPCell hdr1 = new PdfPCell(new Phrase(dr["RNGGRP_cnt_hdr2"].ToString().Trim(), TableFont));
                            //        hdr1.HorizontalAlignment = Element.ALIGN_LEFT;
                            //        hdr1.Colspan = 6;
                            //        hdr1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            //        table.AddCell(hdr1);
                            //    }
                            //    else if (Grp_List.Substring(i, 1) == "3")
                            //    {
                            //        PdfPCell hdr1 = new PdfPCell(new Phrase(dr["RNGGRP_cnt_hdr3"].ToString().Trim(), TableFont));
                            //        hdr1.HorizontalAlignment = Element.ALIGN_LEFT;
                            //        hdr1.Colspan = 6;
                            //        hdr1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            //        table.AddCell(hdr1);
                            //    }
                            //    else if (Grp_List.Substring(i, 1) == "4")
                            //    {
                            //        PdfPCell hdr1 = new PdfPCell(new Phrase(dr["RNGGRP_cnt_hdr4"].ToString().Trim(), TableFont));
                            //        hdr1.HorizontalAlignment = Element.ALIGN_LEFT;
                            //        hdr1.Colspan = 6;
                            //        hdr1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            //        table.AddCell(hdr1);
                            //    }
                            //    else
                            //    {
                            //        PdfPCell hdr1 = new PdfPCell(new Phrase(dr["RNGGRP_cnt_hdr5"].ToString().Trim(), TableFont));
                            //        hdr1.HorizontalAlignment = Element.ALIGN_LEFT;
                            //        hdr1.Colspan = 6;
                            //        hdr1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            //        table.AddCell(hdr1);
                            //    }

                            //    foreach (DataRow drCsbRa in dtCsbRa.Rows)
                            //    {
                            //        if (Convert.ToDateTime(drCsbRa["CSB14RA_REF_FDATE"].ToString()) == Convert.ToDateTime(dr["csb14grp_ref_fdate"].ToString()) && Convert.ToDateTime(drCsbRa["CSB14RA_REF_TDATE"].ToString()) == Convert.ToDateTime(dr["csb14grp_ref_tdate"].ToString()) && drCsbRa["CSB14RA_GROUP_CODE"].ToString().Trim() == dr["csb14grp_group_code"].ToString().Trim())
                            //        {
                            //            string Grp = Grp_List.Substring(i, 1);
                            //            if (drCsbRa["CSB14RA_COUNT_CODE"].ToString() == Grp_List.Substring(i, 1))
                            //            {
                            //                PdfPCell Space1 = new PdfPCell(new Phrase("", TableFont));
                            //                Space1.HorizontalAlignment = Element.ALIGN_LEFT;
                            //                //Space1.Colspan = 2;
                            //                Space1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            //                table.AddCell(Space1);

                            //                PdfPCell RESULT_CODE = new PdfPCell(new Phrase(drCsbRa["CSB14RA_RESULT_CODE"].ToString().Trim(), TableFont));
                            //                RESULT_CODE.HorizontalAlignment = Element.ALIGN_LEFT;
                            //                //RESULT_CODE.Colspan = 2;
                            //                RESULT_CODE.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            //                table.AddCell(RESULT_CODE);

                            //                PdfPCell RESULT_Desc = new PdfPCell(new Phrase(drCsbRa["CSB14RA_DESC"].ToString().Trim(), TableFont));
                            //                RESULT_Desc.HorizontalAlignment = Element.ALIGN_LEFT;
                            //                RESULT_Desc.Colspan = 4;
                            //                RESULT_Desc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            //                table.AddCell(RESULT_Desc);
                            //            }
                            //        }
                            //    }
                            //}


                            string Goal = null; string goaldesc = null;
                            DataSet dsGrp = DatabaseLayer.SPAdminDB.Get_RNGGRP(RefFdate, RefTdate, group, null, null, BaseForm.UserID, BaseForm.BaseAdminAgency);
                            DataTable dtGrp = dsGrp.Tables[0];
                            DataView dv = new DataView(dtGrp);
                            dv.Sort = "RNGGRP_TABLE_CODE";//+string.Empty+"'";
                            dtGrp = dv.ToTable();
                            if (dtGrp.Rows.Count > 1)
                            {
                                PdfPCell TbHeader = new PdfPCell(new Phrase("Table/Goals", SubHeadFont));
                                TbHeader.HorizontalAlignment = Element.ALIGN_LEFT;
                                //TbHeader.FixedHeight = 15f;
                                TbHeader.Colspan = 2;
                                //TbHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                TbHeader.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#a5c8e1"));
                                TbHeader.BorderColor = BaseColor.WHITE;
                                table.AddCell(TbHeader);

                                string[] Header = { "Description", "Ind", "Budget", "CC" };
                                for (int i = 0; i < Header.Length; ++i)
                                {
                                    PdfPCell cell = new PdfPCell(new Phrase(Header[i], SubHeadFont));
                                    if (i == 0)
                                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    else
                                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                    //cell.FixedHeight = 15f;
                                    //cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    cell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#a5c8e1"));
                                    cell.BorderColor = BaseColor.WHITE;
                                    table.AddCell(cell);
                                }

                            }
                            foreach (DataRow drGrp in dtGrp.Rows)
                            {
                                if (!string.IsNullOrWhiteSpace(drGrp["RNGGRP_TABLE_CODE"].ToString()))
                                {
                                    temp_code = drGrp["RNGGRP_goal_codes"].ToString();
                                    List<RNGGAEntity> GoalEntity = _model.SPAdminData.Browse_RNGGA(RefFdate, RefTdate, group, drGrp["RNGGRP_TABLE_CODE"].ToString(), null);

                                    if (!string.IsNullOrEmpty(drGrp["RNGGRP_table_code"].ToString()))
                                    {
                                        PdfPCell table_code = new PdfPCell(new Phrase(drGrp["RNGGRP_table_code"].ToString().Trim(), TableFont));
                                        table_code.HorizontalAlignment = Element.ALIGN_LEFT;
                                        table_code.Colspan = 2;
                                        //table_code.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table_code.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#b9d8ef"));
                                        table_code.BorderColor = BaseColor.WHITE;
                                       // table_code.FixedHeight = 15f;
                                        table.AddCell(table_code);

                                        PdfPCell desc = new PdfPCell(new Phrase(drGrp["RNGGRP_desc"].ToString().Trim(), TableFont));
                                        desc.HorizontalAlignment = Element.ALIGN_LEFT;
                                        desc.FixedHeight = 12f;
                                        //desc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        desc.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#b9d8ef"));
                                        desc.BorderColor = BaseColor.WHITE;
                                       // desc.FixedHeight = 15f;
                                        table.AddCell(desc);

                                        PdfPCell cnt_indicator = new PdfPCell(new Phrase(drGrp["RNGGRP_cnt_indicator"].ToString().Trim(), TableFont));
                                        cnt_indicator.HorizontalAlignment = Element.ALIGN_CENTER;
                                        //cnt_indicator.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        cnt_indicator.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#b9d8ef"));
                                        cnt_indicator.BorderColor = BaseColor.WHITE;
                                       // cnt_indicator.FixedHeight = 15f;
                                        table.AddCell(cnt_indicator);

                                        PdfPCell achieve = new PdfPCell(new Phrase(drGrp["RNGGRP_expect_achieve"].ToString().Trim(), TableFont));
                                        achieve.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        //achieve.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        achieve.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#b9d8ef"));
                                        achieve.BorderColor = BaseColor.WHITE;
                                       // achieve.FixedHeight = 15f;
                                        table.AddCell(achieve);

                                        PdfPCell calc_cost = new PdfPCell(new Phrase(drGrp["RNGGRP_calc_cost"].ToString().Trim(), TableFont));
                                        calc_cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        //calc_cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        calc_cost.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#b9d8ef"));
                                        calc_cost.BorderColor = BaseColor.WHITE;
                                       // calc_cost.FixedHeight = 15f;
                                        table.AddCell(calc_cost);

                                        if (GoalEntity.Count > 0)
                                        {
                                            foreach (RNGGAEntity GEntity in GoalEntity)
                                            {
                                                foreach (MSMASTEntity Entity in MSList)
                                                {
                                                    if (!string.IsNullOrWhiteSpace(GEntity.GoalCode.Trim()))
                                                    {
                                                        if (Entity.Code.ToString().Trim() == GEntity.GoalCode.Trim())
                                                        {
                                                            Goal = Entity.Code;
                                                            goaldesc = Entity.Desc;
                                                            UsedGoals += Entity.Code.Trim() + " ";
                                                            //Entity.Sel_SW = true;
                                                            //Ststus_Exists = Entity.Sel_WS;
                                                            PdfPCell Space2 = new PdfPCell(new Phrase("", TableFont));
                                                            Space2.HorizontalAlignment = Element.ALIGN_LEFT;
                                                            //Space1.Colspan = 3;
                                                            //Space2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                            Space2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                                            Space2.BorderColor = BaseColor.WHITE;
                                                           // Space2.FixedHeight = 15f;
                                                            table.AddCell(Space2);

                                                            PdfPCell stone_code = new PdfPCell(new Phrase(Entity.Code.Trim(), TableFont));
                                                            stone_code.HorizontalAlignment = Element.ALIGN_LEFT;
                                                            //stone_code.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                            stone_code.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                                            stone_code.BorderColor = BaseColor.WHITE;
                                                           // stone_code.FixedHeight = 15f;
                                                            table.AddCell(stone_code);

                                                            PdfPCell stone_desc = new PdfPCell(new Phrase("   " + Entity.Desc.Trim(), TableFont));
                                                            stone_desc.HorizontalAlignment = Element.ALIGN_LEFT;
                                                            stone_desc.Colspan = 2;
                                                            //stone_desc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                            stone_desc.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                            stone_desc.BorderColor = BaseColor.WHITE;
                                                            //stone_desc.FixedHeight = 15f;
                                                            table.AddCell(stone_desc);

                                                            PdfPCell achieve1 = new PdfPCell(new Phrase(GEntity.Budget.Trim(), TableFont));
                                                            achieve1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                            //achieve1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                            achieve1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                            achieve1.BorderColor = BaseColor.WHITE;
                                                           // achieve1.FixedHeight = 15f;
                                                            table.AddCell(achieve1);

                                                            PdfPCell Space3 = new PdfPCell(new Phrase("", TableFont));
                                                            Space3.HorizontalAlignment = Element.ALIGN_LEFT;
                                                            //Space3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                            Space3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                            Space3.BorderColor = BaseColor.WHITE;
                                                            //Space3.FixedHeight = 15f;
                                                            table.AddCell(Space3);

                                                            break;
                                                        }

                                                    }
                                                }
                                            }
                                        }


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

                    }

                    string NotUsedGoals = string.Empty;
                    foreach (MSMASTEntity Entity in MSList)
                    {
                        NotUsedGoals += Entity.Code.Trim() + " ";
                    }

                    if (!string.IsNullOrEmpty(UsedGoals.Trim()))
                    {

                        string[] tmpCd = UsedGoals.Split(' ');
                        for (int i = 0; i < tmpCd.Length; i++)
                        {
                            if (!string.IsNullOrEmpty(tmpCd[i].ToString()))
                            {

                                if (NotUsedGoals.Contains(tmpCd[i].ToString().Trim()))
                                    NotUsedGoals = NotUsedGoals.Replace(tmpCd[i].ToString().Trim(), "");

                                //foreach (MSMASTEntity Entity in MSList)
                                //{
                                //    if (Entity.Code.Trim() != tmpCd[i].ToString().Trim())
                                //    {
                                //        if (string.IsNullOrEmpty(NotUsedGoals.Trim()))
                                //        {
                                //            NotUsedGoals = Entity.Code.Trim() + " ";

                                //        }
                                //        else
                                //        {
                                //            if (NotUsedGoals.Contains(Entity.Code.Trim()))
                                //            {
                                //                //NotUsedGoals = NotUsedGoals.Replace(Entity.Code.Trim(), "");
                                //            }
                                //            else
                                //            {
                                //                NotUsedGoals += Entity.Code.Trim() + " ";
                                //            }
                                //        }
                                //    }
                                //    else
                                //    {
                                //        if(NotUsedGoals.Contains(tmpCd[i].ToString().Trim()))
                                //            NotUsedGoals = NotUsedGoals.Replace(Entity.Code.Trim(), "");
                                //    }
                                //}
                            }
                        }

                        if (!string.IsNullOrEmpty(NotUsedGoals.Trim()))
                        {
                            document.NewPage();

                            PdfPTable table1 = new PdfPTable(2);
                            table1.TotalWidth = 500f;
                            table1.WidthPercentage = 100;
                            table1.LockedWidth = true;
                            float[] widths1 = new float[] { 25f, 60f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                            table1.SetWidths(widths1);
                            table1.HorizontalAlignment = Element.ALIGN_CENTER;

                            PdfPCell Header = new PdfPCell(new Phrase("List of Unassociated Goals", TblFontBoldColor));
                            Header.HorizontalAlignment = Element.ALIGN_CENTER;
                            Header.FixedHeight = 15f;
                            Header.Colspan = 2;
                            //Header.PaddingBottom = 10f;
                            Header.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            table1.AddCell(Header);

                            PdfPCell stone_head = new PdfPCell(new Phrase("Outcome Code", TblFontBold));
                            stone_head.HorizontalAlignment = Element.ALIGN_LEFT;
                            //stone_head.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                            stone_head.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                            stone_head.BorderColor = BaseColor.WHITE;
                           // stone_head.FixedHeight = 15f;
                            table1.AddCell(stone_head);

                            PdfPCell stone_descH = new PdfPCell(new Phrase("Description", TblFontBold));
                            stone_descH.HorizontalAlignment = Element.ALIGN_LEFT;
                            //stone_desc.Colspan = 4;
                            //stone_descH.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                            stone_descH.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                            stone_descH.BorderColor = BaseColor.WHITE;
                           // stone_descH.FixedHeight = 15f;
                            table1.AddCell(stone_descH);

                            string[] tmpCd1 = NotUsedGoals.Split(' ');
                            Array.Sort(tmpCd1);
                            for (int i = 0; i < tmpCd1.Length; i++)
                            {
                                if (!string.IsNullOrEmpty(tmpCd1[i].ToString()))
                                {
                                    foreach (MSMASTEntity Entity in MSList)
                                    {
                                        if (Entity.Code.Trim() == tmpCd1[i].ToString().Trim())
                                        {
                                            PdfPCell stone_code = new PdfPCell(new Phrase(Entity.Code.Trim(), TableFont));
                                            stone_code.HorizontalAlignment = Element.ALIGN_LEFT;
                                            //stone_code.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            stone_code.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                            stone_code.BorderColor = BaseColor.WHITE;
                                           // stone_code.FixedHeight = 15f;
                                            table1.AddCell(stone_code);

                                            PdfPCell stone_desc = new PdfPCell(new Phrase("   " + Entity.Desc.Trim(), TableFont));
                                            stone_desc.HorizontalAlignment = Element.ALIGN_LEFT;
                                            //stone_desc.Colspan = 4;
                                            //stone_desc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            stone_desc.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                            stone_desc.BorderColor = BaseColor.WHITE;
                                           // stone_desc.FixedHeight = 15f;
                                            table1.AddCell(stone_desc);
                                            break;
                                        }
                                    }
                                }
                            }
                            document.Add(table1);
                        }

                    }
                    //document.Add(table);
                }

            }
            catch (Exception ex) { document.Add(new Paragraph("Aborted due to Exception............................................... ")); }

        }

        private void RNGServices(Document document, PdfWriter writer)
        {
            try
            {
                string empty = " ";
                DataSet ds = DatabaseLayer.SPAdminDB.Get_RNGSRGRP(((RepListItem)CmbAgyTab.SelectedItem).Value.ToString(), ((RepListItem)CmbAgyTab.SelectedItem).ValueDisplayCode.ToString(), null, null, null, BaseForm.UserID, BaseForm.BaseAdminAgency);
                DataTable dt = ds.Tables[0];
                DataView dvgrp = new DataView(dt);
                dvgrp.Sort = "RNGSRGRP_GROUP_CODE";//+string.Empty+"'";
                dt = dvgrp.ToTable();

                BaseFont bf_Calibri = BaseFont.CreateFont("c:/windows/fonts/calibri.ttf", BaseFont.WINANSI, BaseFont.EMBEDDED);
                iTextSharp.text.Font Calibri = new iTextSharp.text.Font(bf_Calibri, 10);
                iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_Calibri, 8);
                iTextSharp.text.Font SubHeadFont = new iTextSharp.text.Font(bf_Calibri, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_Calibri, 9, 3);
                iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_Calibri, 8, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
                iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_Calibri, 9, 2);
                iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_Calibri, 9, 4);
                iTextSharp.text.Font TblFontBoldColor = new iTextSharp.text.Font(bf_Calibri, 9, 1, BaseColor.BLACK);

                //BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
                //iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 10);
                //iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 10);
                //iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 9, 3);
                //iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(1, 10, 1);
                //iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 9, 2);
                //iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 9, 4);


                //DataSet dsCsbRa = DatabaseLayer.SPAdminDB.Get_CSB14RA(gvDateRange.CurrentRow.Cells["From_Date"].Value.ToString(), gvDateRange.CurrentRow.Cells["To_Date"].Value.ToString(), null, null);
                //DataTable dtCsbRa = dsCsbRa.Tables[0];

                List<CAMASTEntity> CAList = _model.SPAdminData.Browse_CAMAST(null, null, null, null);

                if (dt.Rows.Count > 0)
                {
                    PdfPTable table = new PdfPTable(6);
                    table.TotalWidth = 540f;
                    table.WidthPercentage = 100;
                    table.LockedWidth = true;
                    float[] widths = new float[] { 15f, 13f, 75f, 6f, 12f, 8f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                    table.SetWidths(widths);
                    table.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.HeaderRows = 2;

                    PdfPCell Hdr = new PdfPCell(new Phrase("Services Association", TblFontBoldColor));
                    Hdr.HorizontalAlignment = Element.ALIGN_CENTER;
                    Hdr.FixedHeight = 15f;
                    Hdr.Colspan = 6;
                    Hdr.PaddingBottom = 5f;
                    Hdr.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    table.AddCell(Hdr);

                    PdfPCell ref_fdate = new PdfPCell(new Phrase(((RepListItem)CmbAgyTab.SelectedItem).ID.ToString(), TblFontBoldColor));
                    ref_fdate.HorizontalAlignment = Element.ALIGN_CENTER;
                    ref_fdate.Colspan = 6;
                    ref_fdate.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    ref_fdate.PaddingBottom = 10f;
                    table.AddCell(ref_fdate);


                    string UsedGoals = string.Empty;
                    string temp_code = null; string AgyGoal_code = null; int j = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        string Grp_List = null; string group = null;

                        if (!string.IsNullOrEmpty(dr["RNGSRgrp_group_code"].ToString().Trim()) && string.IsNullOrEmpty(dr["RNGSRgrp_table_code"].ToString().Trim()))
                        {
                            //***Vikash Commented
                            //PdfPCell Space_code = new PdfPCell(new Phrase("", TblFontBold));
                            //Space_code.HorizontalAlignment = Element.ALIGN_LEFT;
                            //Space_code.Colspan = 6;
                            ////Space_code.FixedHeight = 5f;
                            //Space_code.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            //table.AddCell(Space_code);

                            PdfPCell group_code = new PdfPCell(new Phrase("Group : " + dr["RNGSRgrp_group_code"].ToString().Trim(), TblFontBold));
                            group_code.HorizontalAlignment = Element.ALIGN_LEFT;
                            group_code.Colspan = 2;
                            //group_code.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER;
                            group_code.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                            group_code.BorderColor = BaseColor.WHITE;
                            group_code.FixedHeight = 15f;                          
                            table.AddCell(group_code);

                            PdfPCell group_desc = new PdfPCell(new Phrase(dr["RNGSRgrp_desc"].ToString().Trim(), TblFontBold));
                            group_desc.HorizontalAlignment = Element.ALIGN_LEFT;
                            //group_desc.Colspan = 2;
                            //group_desc.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER;
                            group_desc.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                            group_desc.BorderColor = BaseColor.WHITE;
                            group_desc.FixedHeight = 15f;
                            table.AddCell(group_desc);

                            PdfPCell group_Space = new PdfPCell(new Phrase("", TableFont));
                            group_Space.HorizontalAlignment = Element.ALIGN_LEFT;
                            group_Space.Colspan = 3;
                            //group_Space.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER + iTextSharp.text.Rectangle.TOP_BORDER;
                            group_Space.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                            group_Space.BorderColor = BaseColor.WHITE;
                            group_Space.FixedHeight = 15f;
                            table.AddCell(group_Space);


                            //PdfPCell cell1 = new PdfPCell(new Phrase("Headers & Results", TblFontBold));
                            //cell1.HorizontalAlignment = Element.ALIGN_LEFT;
                            //cell1.FixedHeight = 15f;
                            //cell1.Colspan = 6;
                            //cell1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            //table.AddCell(cell1);

                            group = dr["RNGSRgrp_group_code"].ToString();
                            string RefFdate = dr["RNGSRGRP_CODE"].ToString();
                            string RefTdate = dr["RNGSRGRP_AGENCY"].ToString();
                            //if (!string.IsNullOrEmpty(dr["RNGGRP_cnt_hdr1"].ToString()) && dr["RNGGRP_cnt_incld1"].ToString() == "True")
                            //    Grp_List = "1";
                            //if (!string.IsNullOrEmpty(dr["RNGGRP_cnt_hdr2"].ToString()) && dr["RNGGRP_cnt_incld2"].ToString() == "True")
                            //    Grp_List += "2";
                            //if (!string.IsNullOrEmpty(dr["RNGGRP_cnt_hdr3"].ToString()) && dr["RNGGRP_cnt_incld3"].ToString() == "True")
                            //    Grp_List += "3";
                            //if (!string.IsNullOrEmpty(dr["RNGGRP_cnt_hdr4"].ToString()) && dr["RNGGRP_cnt_incld4"].ToString() == "True")
                            //    Grp_List += "4";
                            //if (!string.IsNullOrEmpty(dr["RNGGRP_cnt_hdr5"].ToString()) && dr["RNGGRP_cnt_incld5"].ToString() == "True")
                            //    Grp_List += "5";

                            //for (int i = 0; i < Grp_List.Length; i++)
                            //{
                            //    if (Grp_List.Substring(i, 1) == "1")
                            //    {
                            //        PdfPCell hdr1 = new PdfPCell(new Phrase(dr["RNGGRP_cnt_hdr1"].ToString().Trim(), TableFont));
                            //        hdr1.HorizontalAlignment = Element.ALIGN_LEFT;
                            //        hdr1.Colspan = 6;
                            //        hdr1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            //        table.AddCell(hdr1);
                            //    }
                            //    else if (Grp_List.Substring(i, 1) == "2")
                            //    {
                            //        PdfPCell hdr1 = new PdfPCell(new Phrase(dr["RNGGRP_cnt_hdr2"].ToString().Trim(), TableFont));
                            //        hdr1.HorizontalAlignment = Element.ALIGN_LEFT;
                            //        hdr1.Colspan = 6;
                            //        hdr1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            //        table.AddCell(hdr1);
                            //    }
                            //    else if (Grp_List.Substring(i, 1) == "3")
                            //    {
                            //        PdfPCell hdr1 = new PdfPCell(new Phrase(dr["RNGGRP_cnt_hdr3"].ToString().Trim(), TableFont));
                            //        hdr1.HorizontalAlignment = Element.ALIGN_LEFT;
                            //        hdr1.Colspan = 6;
                            //        hdr1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            //        table.AddCell(hdr1);
                            //    }
                            //    else if (Grp_List.Substring(i, 1) == "4")
                            //    {
                            //        PdfPCell hdr1 = new PdfPCell(new Phrase(dr["RNGGRP_cnt_hdr4"].ToString().Trim(), TableFont));
                            //        hdr1.HorizontalAlignment = Element.ALIGN_LEFT;
                            //        hdr1.Colspan = 6;
                            //        hdr1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            //        table.AddCell(hdr1);
                            //    }
                            //    else
                            //    {
                            //        PdfPCell hdr1 = new PdfPCell(new Phrase(dr["RNGGRP_cnt_hdr5"].ToString().Trim(), TableFont));
                            //        hdr1.HorizontalAlignment = Element.ALIGN_LEFT;
                            //        hdr1.Colspan = 6;
                            //        hdr1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            //        table.AddCell(hdr1);
                            //    }

                            //    foreach (DataRow drCsbRa in dtCsbRa.Rows)
                            //    {
                            //        if (Convert.ToDateTime(drCsbRa["CSB14RA_REF_FDATE"].ToString()) == Convert.ToDateTime(dr["csb14grp_ref_fdate"].ToString()) && Convert.ToDateTime(drCsbRa["CSB14RA_REF_TDATE"].ToString()) == Convert.ToDateTime(dr["csb14grp_ref_tdate"].ToString()) && drCsbRa["CSB14RA_GROUP_CODE"].ToString().Trim() == dr["csb14grp_group_code"].ToString().Trim())
                            //        {
                            //            string Grp = Grp_List.Substring(i, 1);
                            //            if (drCsbRa["CSB14RA_COUNT_CODE"].ToString() == Grp_List.Substring(i, 1))
                            //            {
                            //                PdfPCell Space1 = new PdfPCell(new Phrase("", TableFont));
                            //                Space1.HorizontalAlignment = Element.ALIGN_LEFT;
                            //                //Space1.Colspan = 2;
                            //                Space1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            //                table.AddCell(Space1);

                            //                PdfPCell RESULT_CODE = new PdfPCell(new Phrase(drCsbRa["CSB14RA_RESULT_CODE"].ToString().Trim(), TableFont));
                            //                RESULT_CODE.HorizontalAlignment = Element.ALIGN_LEFT;
                            //                //RESULT_CODE.Colspan = 2;
                            //                RESULT_CODE.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            //                table.AddCell(RESULT_CODE);

                            //                PdfPCell RESULT_Desc = new PdfPCell(new Phrase(drCsbRa["CSB14RA_DESC"].ToString().Trim(), TableFont));
                            //                RESULT_Desc.HorizontalAlignment = Element.ALIGN_LEFT;
                            //                RESULT_Desc.Colspan = 4;
                            //                RESULT_Desc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            //                table.AddCell(RESULT_Desc);
                            //            }
                            //        }
                            //    }
                            //}



                            string Goal = null; string goaldesc = null;
                            DataSet dsGrp = DatabaseLayer.SPAdminDB.Get_RNGSRGRP(RefFdate, RefTdate, group, null, null, BaseForm.UserID, BaseForm.BaseAdminAgency);
                            DataTable dtGrp = dsGrp.Tables[0];
                            DataView dv = new DataView(dtGrp);
                            dv.Sort = "RNGSRgrp_table_code";//+string.Empty+"'";
                            dtGrp = dv.ToTable();
                            if (dtGrp.Rows.Count > 1)
                            {
                                PdfPCell TbHeader = new PdfPCell(new Phrase("Table/Services", SubHeadFont));
                                TbHeader.HorizontalAlignment = Element.ALIGN_LEFT;
                                TbHeader.FixedHeight = 15f;
                                TbHeader.Colspan = 2;
                                //TbHeader.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                TbHeader.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#a5c8e1"));
                                TbHeader.BorderColor = BaseColor.WHITE;
                                table.AddCell(TbHeader);

                                string[] Header = { "Description", "Ind", "Budget", "CC" };
                                for (int i = 0; i < Header.Length; ++i)
                                {
                                    PdfPCell cell = new PdfPCell(new Phrase(Header[i], SubHeadFont));
                                    if(i==0)
                                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                                    else
                                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                    cell.FixedHeight = 15f;
                                    //cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    cell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#a5c8e1"));
                                    cell.BorderColor = BaseColor.WHITE;
                                    table.AddCell(cell);
                                }

                            }
                            foreach (DataRow drGrp in dtGrp.Rows)
                            {
                                if (!string.IsNullOrWhiteSpace(drGrp["RNGSRgrp_table_code"].ToString()))
                                {
                                    //temp_code = drGrp["RNGSRGRP_goal_codes"].ToString();
                                    List<RNGSRGAEntity> GoalEntity = _model.SPAdminData.Browse_RNGSRGA(RefFdate, RefTdate, group, drGrp["RNGSRgrp_table_code"].ToString(), null);

                                    if (!string.IsNullOrEmpty(drGrp["RNGSRgrp_table_code"].ToString()))
                                    {
                                        PdfPCell table_code = new PdfPCell(new Phrase(drGrp["RNGSRgrp_table_code"].ToString().Trim(), TableFont));
                                        table_code.HorizontalAlignment = Element.ALIGN_LEFT;
                                        table_code.Colspan = 2;
                                        //table_code.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        table_code.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                        table_code.BorderColor = BaseColor.WHITE;
                                        table_code.FixedHeight = 15f;
                                        table.AddCell(table_code);

                                        PdfPCell desc = new PdfPCell(new Phrase(drGrp["RNGSRgrp_desc"].ToString().Trim(), TableFont));
                                        desc.HorizontalAlignment = Element.ALIGN_LEFT;
                                        //desc.FixedHeight = 12f;
                                        //desc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        desc.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                        desc.BorderColor = BaseColor.WHITE;
                                        desc.FixedHeight = 15f;
                                        table.AddCell(desc);

                                        PdfPCell cnt_indicator = new PdfPCell(new Phrase(drGrp["RNGSRgrp_cnt_indicator"].ToString().Trim(), TableFont));
                                        cnt_indicator.HorizontalAlignment = Element.ALIGN_CENTER;
                                        //cnt_indicator.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        cnt_indicator.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                        cnt_indicator.BorderColor = BaseColor.WHITE;
                                        cnt_indicator.FixedHeight = 15f;
                                        table.AddCell(cnt_indicator);

                                        PdfPCell achieve = new PdfPCell(new Phrase(drGrp["RNGSRgrp_expect_achieve"].ToString().Trim(), TableFont));
                                        achieve.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        //achieve.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        achieve.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                        achieve.BorderColor = BaseColor.WHITE;
                                        achieve.FixedHeight = 15f;
                                        table.AddCell(achieve);

                                        PdfPCell calc_cost = new PdfPCell(new Phrase(drGrp["RNGSRgrp_calc_cost"].ToString().Trim(), TableFont));
                                        calc_cost.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        //calc_cost.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                        calc_cost.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));/*b9d8ef*/
                                        calc_cost.BorderColor = BaseColor.WHITE;
                                        calc_cost.FixedHeight = 15f;
                                        table.AddCell(calc_cost);

                                        if (GoalEntity.Count > 0)
                                        {
                                            foreach (RNGSRGAEntity GEntity in GoalEntity)
                                            {
                                                foreach (CAMASTEntity Entity in CAList)
                                                {
                                                    if (!string.IsNullOrWhiteSpace(GEntity.GoalCode.Trim()))
                                                    {
                                                        if (Entity.Code.ToString().Trim() == GEntity.GoalCode.Trim())
                                                        {
                                                            Goal = Entity.Code;
                                                            goaldesc = Entity.Desc;
                                                            UsedGoals += Entity.Code.Trim() + " ";
                                                            //Entity.Sel_SW = true;
                                                            //Ststus_Exists = Entity.Sel_WS;
                                                            PdfPCell Space2 = new PdfPCell(new Phrase("", TableFont));
                                                            Space2.HorizontalAlignment = Element.ALIGN_LEFT;
                                                            //Space1.Colspan = 3;
                                                            //Space2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                            Space2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                                            Space2.BorderColor = BaseColor.WHITE;
                                                            Space2.FixedHeight = 15f;
                                                            table.AddCell(Space2);

                                                            PdfPCell stone_code = new PdfPCell(new Phrase(Entity.Code.Trim(), TableFont));
                                                            stone_code.HorizontalAlignment = Element.ALIGN_LEFT;
                                                            //stone_code.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                            stone_code.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                                            stone_code.BorderColor = BaseColor.WHITE;
                                                            stone_code.FixedHeight = 15f;
                                                            table.AddCell(stone_code);

                                                            PdfPCell stone_desc = new PdfPCell(new Phrase("   " + Entity.Desc.Trim(), TableFont));
                                                            stone_desc.HorizontalAlignment = Element.ALIGN_LEFT;
                                                            stone_desc.Colspan = 2;
                                                            //stone_desc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                            stone_desc.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                            stone_desc.BorderColor = BaseColor.WHITE;
                                                            stone_desc.FixedHeight = 15f;
                                                            table.AddCell(stone_desc);

                                                            PdfPCell achieve1 = new PdfPCell(new Phrase(GEntity.Budget.Trim(), TableFont));
                                                            achieve1.HorizontalAlignment = Element.ALIGN_RIGHT;
                                                            //achieve1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                            achieve1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                            achieve1.BorderColor = BaseColor.WHITE;
                                                            achieve1.FixedHeight = 15f;
                                                            table.AddCell(achieve1);

                                                            PdfPCell Space3 = new PdfPCell(new Phrase("", TableFont));
                                                            Space3.HorizontalAlignment = Element.ALIGN_LEFT;
                                                            //Space3.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                                            Space3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                                            Space3.BorderColor = BaseColor.WHITE;
                                                            Space3.FixedHeight = 15f;
                                                            table.AddCell(Space3);

                                                            break;
                                                        }

                                                    }
                                                }
                                            }
                                        }


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

                    }

                    string NotUsedGoals = string.Empty;
                    foreach (CAMASTEntity Entity in CAList)
                    {
                        NotUsedGoals += Entity.Code.Trim() + " ";
                    }

                    if (!string.IsNullOrEmpty(UsedGoals.Trim()))
                    {

                        string[] tmpCd = UsedGoals.Split(' ');
                        for (int i = 0; i < tmpCd.Length; i++)
                        {
                            if (!string.IsNullOrEmpty(tmpCd[i].ToString()))
                            {

                                if (NotUsedGoals.Contains(tmpCd[i].ToString().Trim()))
                                    NotUsedGoals = NotUsedGoals.Replace(tmpCd[i].ToString().Trim(), "");

                                //foreach (MSMASTEntity Entity in MSList)
                                //{
                                //    if (Entity.Code.Trim() != tmpCd[i].ToString().Trim())
                                //    {
                                //        if (string.IsNullOrEmpty(NotUsedGoals.Trim()))
                                //        {
                                //            NotUsedGoals = Entity.Code.Trim() + " ";

                                //        }
                                //        else
                                //        {
                                //            if (NotUsedGoals.Contains(Entity.Code.Trim()))
                                //            {
                                //                //NotUsedGoals = NotUsedGoals.Replace(Entity.Code.Trim(), "");
                                //            }
                                //            else
                                //            {
                                //                NotUsedGoals += Entity.Code.Trim() + " ";
                                //            }
                                //        }
                                //    }
                                //    else
                                //    {
                                //        if(NotUsedGoals.Contains(tmpCd[i].ToString().Trim()))
                                //            NotUsedGoals = NotUsedGoals.Replace(Entity.Code.Trim(), "");
                                //    }
                                //}
                            }
                        }

                        if (!string.IsNullOrEmpty(NotUsedGoals.Trim()))
                        {
                            document.NewPage();

                            PdfPTable table1 = new PdfPTable(2);
                            table1.TotalWidth = 500f;
                            table1.WidthPercentage = 100;
                            table1.LockedWidth = true;
                            float[] widths1 = new float[] { 25f, 60f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                            table1.SetWidths(widths1);
                            table1.HorizontalAlignment = Element.ALIGN_CENTER;

                            PdfPCell Header = new PdfPCell(new Phrase("List of Unassociated Services", TblFontBoldColor));
                            Header.HorizontalAlignment = Element.ALIGN_CENTER;
                            Header.FixedHeight = 15f;
                            Header.Colspan = 2;
                            //Header.PaddingBottom = 10f;
                            Header.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            table1.AddCell(Header);

                            PdfPCell stone_head = new PdfPCell(new Phrase("CA Code", TblFontBold));
                            stone_head.HorizontalAlignment = Element.ALIGN_LEFT;
                            //stone_head.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                            stone_head.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));   
                            stone_head.BorderColor = BaseColor.WHITE;
                            stone_head.FixedHeight = 15f;
                            table1.AddCell(stone_head);

                            PdfPCell stone_descH = new PdfPCell(new Phrase("Description", TblFontBold));
                            stone_descH.HorizontalAlignment = Element.ALIGN_LEFT;
                            //stone_desc.Colspan = 4;
                            //stone_descH.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                            stone_descH.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                            stone_descH.BorderColor = BaseColor.WHITE;
                            stone_descH.FixedHeight = 15f;
                            table1.AddCell(stone_descH);

                            string[] tmpCd1 = NotUsedGoals.Split(' ');
                            Array.Sort(tmpCd1);
                            for (int i = 0; i < tmpCd1.Length; i++)
                            {
                                if (!string.IsNullOrEmpty(tmpCd1[i].ToString()))
                                {
                                    foreach (CAMASTEntity Entity in CAList)
                                    {
                                        if (Entity.Code.Trim() == tmpCd1[i].ToString().Trim())
                                        {
                                            PdfPCell stone_code = new PdfPCell(new Phrase(Entity.Code.Trim(), TableFont));
                                            stone_code.HorizontalAlignment = Element.ALIGN_LEFT;
                                            //stone_code.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            stone_code.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                                            stone_code.BorderColor = BaseColor.WHITE;
                                            stone_code.FixedHeight = 15f;
                                            table1.AddCell(stone_code);

                                            PdfPCell stone_desc = new PdfPCell(new Phrase("   " + Entity.Desc.Trim(), TableFont));
                                            stone_desc.HorizontalAlignment = Element.ALIGN_LEFT;
                                            //stone_desc.Colspan = 4;
                                            //stone_desc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                            stone_desc.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                            stone_desc.BorderColor = BaseColor.WHITE;
                                            stone_desc.FixedHeight = 15f;
                                            table1.AddCell(stone_desc);
                                            break;
                                        }
                                    }
                                }
                            }
                            document.Add(table1);
                        }

                    }
                    //document.Add(table);
                }

            }
            catch (Exception ex) { document.Add(new Paragraph("Aborted due to Exception............................................... ")); }


        }

        private void ZipCodeReport(Document document, PdfWriter writer)
        {
            string SortCol = null;
            if (RblEmrServ.Checked == true)
                SortCol = "City";
            //else if (rbCounty.Checked == true) SortCol = "";

            BaseFont bf_Calibri = BaseFont.CreateFont("c:/windows/fonts/calibri.ttf", BaseFont.WINANSI, BaseFont.EMBEDDED);
            iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_Calibri, 10);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_Calibri, 8);
            //iTextSharp.text.Font TableFont1 = new iTextSharp.text.Font(bf_times, 8, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
            iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_Calibri, 9, 3);
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_Calibri, 8, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
            iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_Calibri, 9, 2);
            iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_Calibri, 9, 4);


            //BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            //iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 10);
            //iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 10);
            //iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 9, 3);
            //iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(1, 10, 1);
            //iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 9, 2);
            //iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 9, 4);

            try
            {
                DataSet ds = DatabaseLayer.ADMNB001DB.ADMNB001_ZipcodeSearch(SortCol);
                DataTable dt = ds.Tables[0];

                DataView dv = new DataView(dt);
                if (rbTown.Checked == true)
                    dv.Sort = "ZCR_CITY_CODE,ZCR_CITY";
                else if (rbCounty.Checked == true)
                    dv.Sort = "ZCR_COUNTY,ZCR_CITY";
                dt = dv.ToTable();

                //cb.BeginText();
                //cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES).BaseFont, 12);
                ////cb.SetFontAndSize(bfTimes, 10);

                //PrintHeader();

                ////       cb.SetColorFill(BaseColor.BLACK);
                //cb.SetCMYKColorFill(0, 0, 0, 255);
                //X_Pos = 50;
                //Y_Pos -= 5;

                //CheckBottomBorderReached(document, writer);
                //cb.EndText();
                //Print_Line();

                //cb.BeginText();
                //cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES).BaseFont, 12);
                ////cb.SetFontAndSize(bfTimes, 10);
                //cb.SetColorFill(BaseColor.GRAY);
                ////cb.ShowTextAligned(Element.ALIGN_MIDDLE, "ZipCode Report", 250, 820, 0);
                ////           cb.SetColorFill(BaseColor.BLACK);
                //cb.SetCMYKColorFill(0, 0, 0, 255);

                if (dt.Rows.Count > 0)
                {
                    PdfPTable table = new PdfPTable(8);
                    table.TotalWidth = 550f;
                    table.WidthPercentage = 100;
                    table.LockedWidth = true;
                    float[] widths = new float[] { 15f, 30f, 8f, 15f, 15f, 12f, 8f, 22f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                    table.SetWidths(widths);
                    table.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.HeaderRows = 1;

                    string[] Header = { "ZIP Code", "City", "State", "County", "Township", "Month", "Day", "Year" };
                    for (int i = 0; i < Header.Length; ++i)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(Header[i], TblFontBold));
                        if (i == 2 || i == 5 || i == 6)
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        else
                            cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.FixedHeight = 15f;
                        //cell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                        cell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                        cell.BorderColor = BaseColor.WHITE;
                        table.AddCell(cell);
                    }

                    AGYTABSEntity searchAgytabs = new AGYTABSEntity(true);
                    searchAgytabs.Tabs_Type = "00525";  //List<AGYTABSEntity> TransportList = AgyTabs_List.FindAll(u => u.Tabs_Type.ToString().Trim().Equals("S0041"));
                    List<AGYTABSEntity> AgyTabs_List = _model.AdhocData.Browse_AGYTABS(searchAgytabs);

                    List<CommonEntity> TownshipList = _model.ZipCodeAndAgency.GetTownship();

                    foreach (DataRow dr in dt.Rows)
                    {
                        //X_Pos = 55;
                        //Y_Pos -= 14;
                        //CheckBottomBorderReached(document, writer);
                        string Zip = SetLeadingZeros(dr["ZCR_ZIP"].ToString().Trim());
                        string Zip_Plus = "0000".Substring(0, 4 - dr["ZCRPLUS_4"].ToString().Length) + dr["ZCRPLUS_4"].ToString().Trim();
                        PdfPCell Desc = new PdfPCell(new Phrase(Zip + "-" + Zip_Plus, TableFont));
                        Desc.HorizontalAlignment = Element.ALIGN_LEFT;
                        //Desc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        Desc.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                        Desc.BorderColor = BaseColor.WHITE;
                        Desc.FixedHeight = 15;
                        table.AddCell(Desc);

                        PdfPCell City = new PdfPCell(new Phrase(dr["ZCR_CITY"].ToString().Trim(), TableFont));
                        City.HorizontalAlignment = Element.ALIGN_LEFT;
                        //City.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        City.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                        City.BorderColor = BaseColor.WHITE;
                        City.PaddingBottom = 3;
                        table.AddCell(City);

                        PdfPCell State = new PdfPCell(new Phrase(dr["ZCR_STATE"].ToString().Trim(), TableFont));
                        State.HorizontalAlignment = Element.ALIGN_CENTER;
                        //State.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        State.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                        State.BorderColor = BaseColor.WHITE;
                        State.FixedHeight = 15;
                        table.AddCell(State);

                        if (AgyTabs_List.Count > 0)
                        {
                            string CountyDesc = string.Empty;
                            if (!string.IsNullOrEmpty(dr["ZCR_COUNTY"].ToString().Trim()))
                            {
                                AGYTABSEntity County = AgyTabs_List.Find(u => u.Table_Code.Equals(dr["ZCR_COUNTY"].ToString().Trim()));
                                if (County != null)
                                {
                                    //CountyDesc = AgyTabs_List.Find(u => u.Table_Code.Equals(dr["ZCR_COUNTY"].ToString().Trim())).Code_Desc.Trim();

                                    PdfPCell Intake = new PdfPCell(new Phrase(County.Code_Desc.Trim(), TableFont));
                                    Intake.HorizontalAlignment = Element.ALIGN_LEFT;
                                    //Intake.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    Intake.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                                    Intake.BorderColor = BaseColor.WHITE;
                                    Intake.FixedHeight = 15;
                                    table.AddCell(Intake);
                                }
                                else
                                {
                                    PdfPCell Intake = new PdfPCell(new Phrase("", TableFont));
                                    Intake.HorizontalAlignment = Element.ALIGN_LEFT;
                                    //Intake.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    Intake.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                                    Intake.BorderColor = BaseColor.WHITE;
                                    Intake.FixedHeight = 15;
                                    table.AddCell(Intake);
                                }
                            }
                            else
                            {
                                PdfPCell Intake = new PdfPCell(new Phrase("", TableFont));
                                Intake.HorizontalAlignment = Element.ALIGN_LEFT;
                                //Intake.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                Intake.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                                Intake.BorderColor = BaseColor.WHITE;
                                Intake.FixedHeight = 15;
                                table.AddCell(Intake);
                            }
                        }
                        else
                        {
                            PdfPCell Intake = new PdfPCell(new Phrase("", TableFont));
                            Intake.HorizontalAlignment = Element.ALIGN_LEFT;
                            //Intake.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            Intake.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                            Intake.BorderColor = BaseColor.WHITE;
                            Intake.FixedHeight = 15;
                            table.AddCell(Intake);
                        }

                        if (TownshipList.Count > 0)
                        {
                            string TownshipDesc = string.Empty;
                            if (!string.IsNullOrEmpty(dr["ZCR_CITY_CODE"].ToString().Trim()))
                            {
                                CommonEntity TownShip = TownshipList.Find(u => u.Code.Trim().Equals(dr["ZCR_CITY_CODE"].ToString().Trim()));
                                if (TownShip != null)
                                {
                                    //TownshipDesc = TownshipList.Find(u => u.Code.Trim().Equals(dr["ZCR_CITY_CODE"].ToString().Trim())).Desc.Trim();

                                    PdfPCell Town2 = new PdfPCell(new Phrase(TownShip.Desc.Trim(), TableFont));
                                    Town2.HorizontalAlignment = Element.ALIGN_LEFT;
                                    //Town2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    Town2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                                    Town2.BorderColor = BaseColor.WHITE;
                                    Town2.FixedHeight = 15;
                                    table.AddCell(Town2);
                                }
                                else
                                {
                                    PdfPCell Town2 = new PdfPCell(new Phrase("None", TableFont));
                                    Town2.HorizontalAlignment = Element.ALIGN_LEFT;
                                    //Town2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    Town2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                                    Town2.BorderColor = BaseColor.WHITE;
                                    Town2.FixedHeight = 15;
                                    table.AddCell(Town2);
                                }
                            }
                            else
                            {
                                PdfPCell Town2 = new PdfPCell(new Phrase("None", TableFont));
                                Town2.HorizontalAlignment = Element.ALIGN_LEFT;
                                //Town2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                Town2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                                Town2.BorderColor = BaseColor.WHITE;
                                Town2.FixedHeight = 15;
                                table.AddCell(Town2);
                            }
                        }
                        else
                        {
                            PdfPCell Town2 = new PdfPCell(new Phrase(dr["ZCR_CITY_CODE"].ToString().Trim(), TableFont));
                            Town2.HorizontalAlignment = Element.ALIGN_LEFT;
                            //Town2.Border = iTextSharp.text.Rectangle.NO_BORDER;
                            Town2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                            Town2.BorderColor = BaseColor.WHITE;
                            Town2.FixedHeight = 15;
                            table.AddCell(Town2);
                        }


                        PdfPCell Month = new PdfPCell(new Phrase(GetMonth(dr["ZCR_HSS_MO"].ToString()), TableFont));
                        Month.HorizontalAlignment = Element.ALIGN_CENTER;
                        //Month.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        Month.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                        Month.BorderColor = BaseColor.WHITE;
                        Month.FixedHeight = 15;
                        table.AddCell(Month);

                        PdfPCell Day = new PdfPCell(new Phrase(dr["ZCR_HSS_DAY"].ToString().Trim(), TableFont));
                        Day.HorizontalAlignment = Element.ALIGN_CENTER;
                        //Day.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        Day.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                        Day.BorderColor = BaseColor.WHITE;
                        Day.FixedHeight = 15;
                        table.AddCell(Day);

                        PdfPCell Year = new PdfPCell(new Phrase(GetYear(dr["ZCR_HSS_YEAR"].ToString()), TableFont));
                        Year.HorizontalAlignment = Element.ALIGN_LEFT;
                        //Year.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        Year.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                        Year.BorderColor = BaseColor.WHITE;
                        Year.FixedHeight = 15;
                        table.AddCell(Year);

                        //PrintRec(SetLeadingZeros(dr["ZCR_ZIP"].ToString()), 80);
                        ////PrintRec(dr["ZCRPLUS_4"].ToString(), 50);
                        //PrintRec(dr["ZCR_CITY"].ToString(), 140);
                        //PrintRec(dr["ZCR_STATE"].ToString(), 60);
                        //PrintRec(dr["ZCR_INTAKE_CODE"].ToString(), 50);
                        ////PrintRec(dr["ZCR_CITY_CODE"].ToString(), 85);

                        //PrintRec(GetMonth(dr["ZCR_HSS_MO"].ToString()), 60);
                        //PrintRec(dr["ZCR_HSS_DAY"].ToString(), 50);

                        //PrintRec(GetYear(dr["ZCR_HSS_YEAR"].ToString()), 80);

                    }
                    document.Add(table);
                }
                else
                {
                    cb.SetFontAndSize(bf_Calibri, 20);
                    cb.BeginText();
                    cb.SetColorFill(BaseColor.RED);
                    cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "No Records Found", 250, 400, 0);
                    cb.EndText();
                }
                //cb.EndText();
            }
            catch (Exception ex) { document.Add(new Paragraph("Aborted due to Exception............................................... ")); }
        }

        private string SetLeadingZeros(string TmpSeq)
        {
            int Seq_len = TmpSeq.Trim().Length;
            string TmpCode = null;
            TmpCode = TmpSeq.ToString().Trim();
            switch (Seq_len)
            {
                case 4: TmpCode = "0" + TmpCode; break;
                case 3: TmpCode = "00" + TmpCode; break;
                case 2: TmpCode = "000" + TmpCode; break;
                case 1: TmpCode = "0000" + TmpCode; break;
                    //default: MessageBox.Show("Table Code should not be blank", "CAP Systems", MessageBoxButtons.OK);  TxtCode.Focus();
                    //    break;
            }
            return (TmpCode);
        }

        private void MilestonesList(Document document, PdfWriter writer)
        {
            string SortCol = null;
            if (RblEmrServ.Checked == true)
                SortCol = "Description";

            BaseFont bf_Calibri = BaseFont.CreateFont("c:/windows/fonts/calibri.ttf", BaseFont.WINANSI, BaseFont.EMBEDDED);
            iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_Calibri, 10);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_Calibri, 8);
            //iTextSharp.text.Font TableFont1 = new iTextSharp.text.Font(bf_times, 8, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
            iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_Calibri, 9, 3);
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_Calibri, 8, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
            iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_Calibri, 9, 2);
            iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_Calibri, 9, 4);

            //BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            //iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 10);
            //iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 10);
            //iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 12, 3);
            //iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(1, 12, 1);
            //iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 9, 2);
            //iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 9, 4);

            try
            {
                DataSet ds = DatabaseLayer.ADMNB001DB.ADMNB001_GetMsmast(SortCol, null);
                DataTable dt = ds.Tables[0];
                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, false);
                Font fc = new iTextSharp.text.Font(bfTimes, 12, 2);


                BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, BaseFont.NOT_EMBEDDED);

                PdfPTable table = new PdfPTable(2);
                table.TotalWidth = 500f;
                table.WidthPercentage = 100;
                table.LockedWidth = true;
                float[] widths = new float[] { 10f, 50f };
                table.SetWidths(widths);
                table.HorizontalAlignment = Element.ALIGN_CENTER;
                table.SpacingBefore = 20f;
                table.SpacingAfter = 30f;
                Y_Pos = 550;
                PdfPCell cell2 = new PdfPCell(new Phrase("Code", TblFontBold));
                cell2.HorizontalAlignment = Element.ALIGN_LEFT;
                //cell2.Border = Rectangle.NO_BORDER;
                cell2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                cell2.BorderColor = BaseColor.WHITE;
                //cell2.BorderWidthBottom = 0.7f;
                cell2.FixedHeight = 15;
                table.AddCell(cell2);

                PdfPCell cell1 = new PdfPCell(new Phrase("Description", TblFontBold));
                cell1.HorizontalAlignment = Element.ALIGN_LEFT;
                //cell1.Border = Rectangle.NO_BORDER;
                cell1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                cell1.BorderColor = BaseColor.WHITE;
                //cell1.BorderWidthBottom = 0.7f;
                cell1.FixedHeight = 15;
                table.AddCell(cell1);
                table.HeaderRows = 1;
                //table.FooterRows = 5;

                if (dt.Rows.Count > 0)
                {
                    //int i = 1;
                    Font Cell_Font = new Font();
                    Cell_Font.SetColor(249, 21, 21);
                    foreach (DataRow dr in dt.Rows)
                    {
                        PdfPCell Service = new PdfPCell(new Phrase(dr["MS_CODE"].ToString(), TableFont));
                        Service.HorizontalAlignment = Element.ALIGN_LEFT;
                        //Service.Border = Rectangle.NO_BORDER;
                        Service.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                        Service.BorderColor = BaseColor.WHITE;
                        Service.FixedHeight = 15;
                        table.AddCell(Service);

                        PdfPCell cell = new PdfPCell(new Phrase(dr["MS_DESC"].ToString(), TableFont));
                        //cell.Border = Rectangle.NO_BORDER;
                        cell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                        cell.BorderColor = BaseColor.WHITE;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.FixedHeight = 15;
                        table.AddCell(cell);


                    }


                }
                else
                {
                    cb.SetFontAndSize(bf_Calibri, 20);
                    cb.BeginText();
                    cb.SetColorFill(BaseColor.RED);
                    cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "No Records Found", 250, 400, 0);
                    cb.EndText();
                }

                document.Add(table);
            }
            catch (Exception ex) { document.Add(new Paragraph("Aborted due to Exception............................................... ")); }

            //cb.BeginText();
            //cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ROMAN).BaseFont, 12);

            //PrintHeader();
            //cb.SetCMYKColorFill(0, 0, 0, 255);
            //X_Pos = 50;
            //Y_Pos -= 5;

            //CheckBottomBorderReached(document, writer);
            //cb.EndText();
            //Print_Line();

            //cb.BeginText();
            //cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ROMAN).BaseFont, 12);

            //cb.SetColorFill(BaseColor.GRAY);
            // cb.SetCMYKColorFill(0, 0, 0, 255);
            //foreach (DataRow dr in dt.Rows)
            //{
            //    X_Pos = 55;
            //    Y_Pos -= 14;
            //    CheckBottomBorderReached(document, writer);
            //    PrintRec(dr["MS_CODE"].ToString(), 60);
            //    PrintRec(dr["MS_DESC"].ToString(), 50);

            //}
            //cb.EndText();
        }

        private void CriticalActivitiesList(Document document, PdfWriter writer)
        {
            string SortCol = null;
            if (RblEmrServ.Checked == true)
                SortCol = "Description";

            BaseFont bf_Calibri = BaseFont.CreateFont("c:/windows/fonts/calibri.ttf", BaseFont.WINANSI, BaseFont.EMBEDDED);
            iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_Calibri, 10);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_Calibri, 8);
            //iTextSharp.text.Font TableFont1 = new iTextSharp.text.Font(bf_times, 8, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
            iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_Calibri, 9, 3);
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_Calibri, 8, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
            iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_Calibri, 9, 2);
            iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_Calibri, 9, 4);

            //BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            //iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 10);
            //iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 10);
            //iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 12, 3);
            //iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(1, 12, 1);
            //iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 9, 2);
            //iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 9, 4);

            try
            {
                DataSet ds = DatabaseLayer.ADMNB001DB.ADMNB001_GetCamast(SortCol, null);
                DataTable dt = ds.Tables[0];
                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, false);
                Font fc = new iTextSharp.text.Font(bfTimes, 12, 2);
                PdfTemplate template = writer.DirectContent.CreateTemplate(100, 100);

                BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, BaseFont.NOT_EMBEDDED);

                PdfPTable table = new PdfPTable(2);
                table.TotalWidth = 500f;
                table.WidthPercentage = 100;
                table.LockedWidth = true;
                float[] widths = new float[] { 10f, 50f };
                table.SetWidths(widths);
                table.HorizontalAlignment = Element.ALIGN_CENTER;
                table.SpacingBefore = 20f;
                table.SpacingAfter = 30f;
                Y_Pos = 550;

                PdfPCell cell2 = new PdfPCell(new Phrase("Code", TblFontBold));
                cell2.HorizontalAlignment = Element.ALIGN_LEFT;
                //cell2.Border = Rectangle.NO_BORDER;
                cell2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                cell2.BorderColor = BaseColor.WHITE;
                //cell2.BorderWidthBottom = 0.7f;
                cell2.FixedHeight = 15f;
                table.AddCell(cell2);

                PdfPCell cell1 = new PdfPCell(new Phrase("Description", TblFontBold));
                // cell.BackgroundColor = new BaseColor(190, 120, 204);
                cell1.HorizontalAlignment = Element.ALIGN_LEFT;
                //cell1.Border = Rectangle.NO_BORDER;
                cell1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                cell1.BorderColor = BaseColor.WHITE;
                cell1.FixedHeight = 15f;
                //cell1.BorderWidthBottom = 0.7f;
                cell1.FixedHeight = 15;
                table.AddCell(cell1);
                table.HeaderRows = 1;

                if (dt.Rows.Count > 0)
                {
                    Font Cell_Font = new Font();
                    Cell_Font.SetColor(249, 21, 21);
                    foreach (DataRow dr in dt.Rows)
                    {
                        PdfPCell Service = new PdfPCell(new Phrase(dr["CA_CODE"].ToString(), TableFont));
                        Service.HorizontalAlignment = Element.ALIGN_LEFT;
                        //Service.Border = Rectangle.NO_BORDER;
                        Service.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                        Service.BorderColor = BaseColor.WHITE;
                        Service.FixedHeight = 15f;
                        table.AddCell(Service);

                        PdfPCell cell = new PdfPCell(new Phrase(dr["CA_DESC"].ToString(), TableFont));
                        //cell.Border = Rectangle.NO_BORDER;
                        cell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                        cell.BorderColor = BaseColor.WHITE;
                        cell.FixedHeight = 15f;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.AddCell(cell);

                    }

                }
                else
                {
                    cb.SetFontAndSize(bf_Calibri, 20);
                    cb.BeginText();
                    cb.SetColorFill(BaseColor.RED);
                    cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "No Records Found", 250, 400, 0);
                    cb.EndText();
                }

                document.Add(table);
            }
            catch (Exception ex) { document.Add(new Paragraph("Aborted due to Exception............................................... ")); }
        }

        private void PrintProgramsandReports(Document document, PdfWriter writer)
        {
            document.SetPageSize(new iTextSharp.text.Rectangle(iTextSharp.text.PageSize.A4.Width, iTextSharp.text.PageSize.A4.Height));
            try
            {
                string Module = string.Empty;
                if (((Captain.Common.Utilities.ListItem)CmbAgyTab.SelectedItem).Value.ToString() != "**") Module = ((Captain.Common.Utilities.ListItem)CmbAgyTab.SelectedItem).Value.ToString();

                DataSet dsScreen = Captain.DatabaseLayer.ADMNB002DB.Browse_CAPFNCC(Module);
                DataTable dtScreen = dsScreen.Tables[0];
                DataSet dsRep = Captain.DatabaseLayer.ADMNB002DB.Browse_CAPBATC(Module);
                DataTable dtRep = dsRep.Tables[0];

                BaseFont bf_Calibri = BaseFont.CreateFont("c:/windows/fonts/calibri.ttf", BaseFont.WINANSI, BaseFont.EMBEDDED);
                iTextSharp.text.Font Calibri = new iTextSharp.text.Font(bf_Calibri, 10);
                iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_Calibri, 8);
                iTextSharp.text.Font SubHeadFont = new iTextSharp.text.Font(bf_Calibri, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_Calibri, 9, 3);
                iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_Calibri, 8, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
                iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_Calibri, 9, 2);
                iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_Calibri, 9, 4);

                //BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
                //iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 10);
                //iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 10);
                //iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 9, 3);
                //iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(1, 10, 1);
                //iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 9, 2);
                //iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 9, 4);

                if (RblCaseMgmt.Checked && dtScreen.Rows.Count > 0)
                {
                    PdfPTable table = new PdfPTable(3);
                    table.TotalWidth = 560f;
                    table.WidthPercentage = 100;
                    table.LockedWidth = true;
                    float[] widths = new float[] { 15f, 25f, 60f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                    table.SetWidths(widths);
                    table.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.HeaderRows = 1;

                    string[] Header = { "Module", "Program Code", "Description" };
                    for (int i = 0; i < Header.Length; ++i)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(Header[i], TblFontBold));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.FixedHeight = 15f;
                        //cell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                        cell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                        cell.BorderColor = BaseColor.WHITE;
                        table.AddCell(cell);
                    }

                    foreach (DataRow dr in dtScreen.Rows)
                    {
                        PdfPCell Agy_Type = new PdfPCell(new Phrase(dr["CFC_MODULE_CODE"].ToString().Trim(), TableFont));
                        Agy_Type.HorizontalAlignment = Element.ALIGN_LEFT;
                        //Agy_Type.Colspan = 2;
                        //Agy_Type.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        Agy_Type.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                        Agy_Type.BorderColor = BaseColor.WHITE;
                        Agy_Type.FixedHeight = 15;
                        table.AddCell(Agy_Type);

                        PdfPCell Desc = new PdfPCell(new Phrase(dr["CFC_PROGNO"].ToString().Trim(), TableFont));
                        Desc.HorizontalAlignment = Element.ALIGN_LEFT;
                        //Desc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        Desc.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                        Desc.BorderColor = BaseColor.WHITE;
                        Desc.FixedHeight = 15;
                        table.AddCell(Desc);

                        PdfPCell ACTIVE = new PdfPCell(new Phrase(dr["CFC_DESCRIPTION"].ToString().Trim(), TableFont));
                        ACTIVE.HorizontalAlignment = Element.ALIGN_LEFT;
                        //ACTIVE.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        ACTIVE.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                        ACTIVE.BorderColor = BaseColor.WHITE;
                        ACTIVE.FixedHeight = 15;
                        table.AddCell(ACTIVE);
                    }

                    document.Add(table);
                }
                else if (RblEmrServ.Checked && dtRep.Rows.Count > 0)
                {
                    PdfPTable table = new PdfPTable(3);
                    table.TotalWidth = 560f;
                    table.WidthPercentage = 100;
                    table.LockedWidth = true;
                    float[] widths = new float[] { 15f, 25f, 60f };// 30f, 25f, 18f, 18f, 20f, 25f, 30f, 20f, 25f, 18f, 18f, 22f };
                    table.SetWidths(widths);
                    table.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.HeaderRows = 1;

                    string[] Header = { "Module", "Report Code", "Report Name" };
                    for (int i = 0; i < Header.Length; ++i)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(Header[i], TblFontBold));
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.FixedHeight = 15f;
                        //cell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                        cell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                        cell.BorderColor = BaseColor.WHITE;
                        table.AddCell(cell);
                    }

                    foreach (DataRow dr in dtRep.Rows)
                    {
                        PdfPCell Agy_Type = new PdfPCell(new Phrase(dr["CBC_MODULE_CODE"].ToString().Trim(), TableFont));
                        Agy_Type.HorizontalAlignment = Element.ALIGN_LEFT;
                        //Agy_Type.Colspan = 2;
                        //Agy_Type.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        Agy_Type.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                        Agy_Type.BorderColor = BaseColor.WHITE;
                        Agy_Type.FixedHeight = 15;
                        table.AddCell(Agy_Type);

                        PdfPCell Desc = new PdfPCell(new Phrase(dr["CBC_REPORT_CODE"].ToString().Trim(), TableFont));
                        Desc.HorizontalAlignment = Element.ALIGN_LEFT;
                        //Desc.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        Desc.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                        Desc.BorderColor = BaseColor.WHITE;
                        Desc.FixedHeight = 15;
                        table.AddCell(Desc);

                        PdfPCell ACTIVE = new PdfPCell(new Phrase(dr["CBC_REPORT_NAME"].ToString().Trim(), TableFont));
                        ACTIVE.HorizontalAlignment = Element.ALIGN_LEFT;
                        //ACTIVE.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        ACTIVE.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                        ACTIVE.BorderColor = BaseColor.WHITE;
                        ACTIVE.FixedHeight = 15;
                        table.AddCell(ACTIVE);
                    }

                    document.Add(table);
                }
                else
                {
                    if (RblCaseMgmt.Checked)
                    {
                        document.Add(new Paragraph("No Programs to define this application. "));
                    }
                    else if (RblEmrServ.Checked)
                    {
                        document.Add(new Paragraph("No Reports to define this application. "));
                    }
                }

            }
            catch (Exception ex) { document.Add(new Paragraph("Aborted due to Exception............................................... ")); }
        }


        private void HierarchyReport(Document document, PdfWriter writer, bool Format, string strPublicCode)
        {

            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, false);
            Font fc = new iTextSharp.text.Font(bfTimes, 12);
            Font fb = new iTextSharp.text.Font(bfTimes, 12, 1);
            PdfTemplate template = writer.DirectContent.CreateTemplate(100, 100);

            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, BaseFont.NOT_EMBEDDED);

            if (Format)
            {
                //if (strPublicCode == "**-**-**")
                //strPublicCode = "  -  -  ";
                try
                {
                    DataSet ds = DatabaseLayer.ADMNB001DB.ADMNB001_GetCashie(strPublicCode, BaseForm.UserID, BaseForm.BaseAdminAgency);
                    DataTable dt = ds.Tables[0];
                    PdfPTable table = new PdfPTable(6);
                    table.TotalWidth = 500f;
                    table.WidthPercentage = 100;
                    table.LockedWidth = true;
                    float[] widths = new float[] { 25f, 20f, 20f, 60f, 60f, 60f };
                    table.SetWidths(widths);
                    table.HorizontalAlignment = Element.ALIGN_CENTER;
                    table.SpacingBefore = 20f;
                    table.SpacingAfter = 30f;
                    PdfPCell BlankCell = new PdfPCell(new Phrase(" "));
                    BlankCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    BlankCell.Border = Rectangle.NO_BORDER;
                    //int j;
                    //for (j = 1; j <= table.NumberOfColumns; j++)
                    //{
                    //    PdfPCell Page = new PdfPCell(new Phrase("Page: " + j));
                    //    Page.HorizontalAlignment = Element.ALIGN_LEFT;
                    //    //Page.Left = 5f;
                    //    Page.Colspan = 6;
                    //    Page.Border = Rectangle.NO_BORDER;
                    //    table.AddCell(Page);
                    //    table.HeaderRows = 1;

                    //}


                    string[] col = { "Agency Code", "Dept Code", "Prog Code", "Agency Name", "Department Name", "Program Name" };
                    for (int i = 0; i < col.Length; ++i)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(col[i]));
                        // cell.BackgroundColor = new BaseColor(190, 120, 204);
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.Border = Rectangle.NO_BORDER;
                        cell.BorderWidthBottom = 0.7f;
                        table.HeaderRows = 2;
                        table.AddCell(cell);

                    }

                    //PdfPCell Page = new PdfPCell(new Phrase("Page: "));
                    //Page.HorizontalAlignment = Element.ALIGN_LEFT;

                    //Page.Colspan = 6;
                    //Page.Border = Rectangle.NO_BORDER;
                    //table.FooterRows = 1;
                    //table.AddCell(Page);

                    if (dt.Rows.Count > 0)
                    {
                        //int i = 1;
                        Font Cell_Font = new Font();
                        Cell_Font.SetColor(249, 21, 21);
                        foreach (DataRow dr in dt.Rows)
                        {
                            PdfPCell Agency = new PdfPCell(new Phrase(dr["HIE_AGENCY"].ToString(), fc));
                            Agency.HorizontalAlignment = Element.ALIGN_CENTER;
                            Agency.Border = Rectangle.NO_BORDER;
                            table.AddCell(Agency);

                            PdfPCell Dept = new PdfPCell(new Phrase(dr["HIE_DEPT"].ToString(), fc));
                            Dept.HorizontalAlignment = Element.ALIGN_CENTER;
                            Dept.Border = Rectangle.NO_BORDER;
                            table.AddCell(Dept);

                            PdfPCell Prog = new PdfPCell(new Phrase(dr["HIE_PROGRAM"].ToString(), fc));
                            Prog.HorizontalAlignment = Element.ALIGN_CENTER;
                            Prog.Border = Rectangle.NO_BORDER;
                            table.AddCell(Prog);



                            if (!string.IsNullOrEmpty(dr["HIE_AGENCY"].ToString().Trim()) && string.IsNullOrEmpty(dr["HIE_DEPT"].ToString().Trim()) && string.IsNullOrEmpty(dr["HIE_PROGRAM"].ToString().Trim()))
                            {
                                PdfPCell AgencyName = new PdfPCell(new Phrase(dr["HIE_NAME"].ToString(), fc));
                                AgencyName.HorizontalAlignment = Element.ALIGN_LEFT;
                                AgencyName.Border = Rectangle.NO_BORDER;
                                table.AddCell(AgencyName);
                                table.AddCell(BlankCell);
                                table.AddCell(BlankCell);
                            }


                            else if ((!string.IsNullOrEmpty(dr["HIE_AGENCY"].ToString().Trim()) && !string.IsNullOrEmpty(dr["HIE_DEPT"].ToString().Trim()) && string.IsNullOrEmpty(dr["HIE_PROGRAM"].ToString().Trim())))
                            {
                                PdfPCell DeptName = new PdfPCell(new Phrase(dr["HIE_NAME"].ToString(), fc));
                                DeptName.Border = Rectangle.NO_BORDER;
                                DeptName.HorizontalAlignment = Element.ALIGN_LEFT;
                                table.AddCell(BlankCell);
                                table.AddCell(DeptName);
                                table.AddCell(BlankCell);
                            }


                            else if ((!string.IsNullOrEmpty(dr["HIE_AGENCY"].ToString().Trim()) && (!string.IsNullOrEmpty(dr["HIE_DEPT"].ToString().Trim())) && !string.IsNullOrEmpty(dr["HIE_PROGRAM"].ToString().Trim())))
                            {
                                PdfPCell ProgName = new PdfPCell(new Phrase(dr["HIE_NAME"].ToString(), fc));
                                ProgName.Border = Rectangle.NO_BORDER;
                                ProgName.HorizontalAlignment = Element.ALIGN_LEFT;
                                table.AddCell(BlankCell);
                                table.AddCell(BlankCell);
                                table.AddCell(ProgName);
                            }
                        }
                    }

                    document.Add(table);
                }
                catch (Exception ex) { document.Add(new Paragraph("Aborted due to Exception............................................... ")); }
            }

            else
            {
                document.SetPageSize(new iTextSharp.text.Rectangle(iTextSharp.text.PageSize.A4.Width, iTextSharp.text.PageSize.A4.Height));

                try
                {
                    string Agy, Dept, Prog = null;
                    if ((strPublicCode.Substring(0, 2) == "**") || strPublicCode.Substring(0, 2) == ("  ") || strPublicCode.Substring(0, 2) == null)
                    {
                        Agy = null;
                    }
                    else
                        Agy = strPublicCode.Substring(0, 2);

                    if ((strPublicCode.Substring(3, 2) == "**") || strPublicCode.Substring(3, 2) == ("  ") || strPublicCode.Substring(3, 2) == null)
                    {
                        Dept = null;
                    }
                    else
                        Dept = strPublicCode.Substring(3, 2);
                    if ((strPublicCode.Substring(6, 2) == "**") || strPublicCode.Substring(6, 2) == ("  ") || strPublicCode.Substring(6, 2) == null)
                    {
                        Prog = null;
                    }
                    else
                        Prog = strPublicCode.Substring(6, 2);
                    DataSet ds = DatabaseLayer.ADMNB001DB.ADMNB001_GetCasDep(Agy, Dept, Prog);
                    DataTable dt = ds.Tables[0];

                    Y_Pos = 780;

                    string Agency = null, Depart = null;
                    string Priv_Agency = null, Priv_Depart = null;
                    foreach (DataRow drr in dt.Rows)
                    {
                        cb.BeginText();
                        cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES).BaseFont, 12);

                        if (Priv_Agency != drr["DEP_AGENCY"].ToString())
                        {
                            DataSet dsAgy = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(drr["DEP_AGENCY"].ToString(), "**", "**");
                            Agency = (dsAgy.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim();
                        }

                        if (Priv_Depart != drr["DEP_DEPT"].ToString())
                        {
                            DataSet ds1 = Captain.DatabaseLayer.AgyTab.GetHierarchyNames(drr["DEP_AGENCY"].ToString(), drr["DEP_DEPT"].ToString(), "**");
                            Depart = (ds1.Tables[0].Rows[0]["HIE_NAME"].ToString()).Trim();
                        }

                        cb.ShowTextAligned(Element.ALIGN_LEFT, "Agency :" + drr["DEP_AGENCY"].ToString() + " " + Agency, 50, Y_Pos, 0);
                        cb.ShowTextAligned(Element.ALIGN_LEFT, "Department :" + drr["DEP_DEPT"].ToString() + " " + Depart, 350, Y_Pos, 0);


                        Y_Pos -= 20; CheckBottomBorderReached(document, writer);
                        cb.ShowTextAligned(Element.ALIGN_LEFT, "Program Code: " + drr["DEP_PROGRAM"], 50, Y_Pos, 0);
                        cb.ShowTextAligned(Element.ALIGN_LEFT, "Program Name: " + drr["DEP_AGCY"], 150, Y_Pos, 0);
                        cb.ShowTextAligned(Element.ALIGN_LEFT, "Short Name: " + drr["DEP_SHORT_NAME"], 400, Y_Pos, 0);
                        //PrintHeader();

                        cb.SetCMYKColorFill(0, 0, 0, 255);
                        X_Pos = 70;
                        Y_Pos -= 25;
                        CheckBottomBorderReached(document, writer);
                        cb.ShowTextAligned(Element.ALIGN_LEFT, "Director Information", X_Pos, Y_Pos, 0);
                        cb.ShowTextAligned(Element.ALIGN_LEFT, "Contact Information", X_Pos + 310, Y_Pos, 0);
                        cb.EndText();
                        X_Pos = 50;
                        Y_Pos -= 5;
                        CheckBottomBorderReached(document, writer);
                        Print_Line();

                        string TmpZip = drr["DEP_ZIP"].ToString();
                        string TmpZipPlus = drr["DEP_ZIP_PLUS"].ToString();

                        TmpZip = "00000".Substring(0, 5 - TmpZip.Length) + TmpZip;
                        TmpZipPlus = "0000".Substring(0, 4 - TmpZipPlus.Length) + TmpZipPlus;

                        cb.BeginText();
                        cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES).BaseFont, 12);
                        //X_Pos = 50;
                        Y_Pos -= 20;
                        CheckBottomBorderReached(document, writer);
                        cb.ShowTextAligned(Element.ALIGN_LEFT, "Name : ", 50, Y_Pos, 0);
                        cb.ShowTextAligned(Element.ALIGN_LEFT, drr["DEP_D_FN"].ToString().Trim() + drr["DEP_D_LN"].ToString().Trim(), 110, Y_Pos, 0);
                        Y_Pos -= 15;
                        CheckBottomBorderReached(document, writer);
                        cb.ShowTextAligned(Element.ALIGN_LEFT, "Address : ", 50, Y_Pos, 0);
                        cb.ShowTextAligned(Element.ALIGN_LEFT, drr["DEP_ADDR1"].ToString(), 110, Y_Pos, 0);
                        Y_Pos -= 15;
                        CheckBottomBorderReached(document, writer);
                        cb.ShowTextAligned(Element.ALIGN_LEFT, drr["DEP_CITY"].ToString().Trim() + " " + drr["DEP_STATE"].ToString().Trim() + " " + TmpZip + " " + TmpZipPlus, 110, Y_Pos, 0);
                        Y_Pos -= 15;
                        CheckBottomBorderReached(document, writer);
                        cb.ShowTextAligned(Element.ALIGN_LEFT, "Phone No: ", 50, Y_Pos, 0);
                        cb.ShowTextAligned(Element.ALIGN_LEFT, "(" + drr["DEP_PHONE"].ToString().Substring(0, 3) + ") " + drr["DEP_PHONE"].ToString().Substring(3, 3) + "-" + drr["DEP_PHONE"].ToString().Substring(6, 3), 110, Y_Pos, 0);
                        //cb.SetColorFill(BaseColor.GRAY);

                        if (drr["FILE_SWITCH"].ToString() == "CONTDEP")
                        {

                            Y_Pos = Y_Pos + 45;
                            cb.ShowTextAligned(Element.ALIGN_LEFT, "Name : ", 310, Y_Pos, 0);
                            cb.ShowTextAligned(Element.ALIGN_LEFT, drr["DEPCONT_FNAME"].ToString().Trim() + " " + drr["DEPCONT_LNAME"].ToString().Trim(), 380, Y_Pos, 0);

                            Y_Pos -= 15;
                            CheckBottomBorderReached(document, writer);
                            cb.ShowTextAligned(Element.ALIGN_LEFT, "Phone No: ", 310, Y_Pos, 0);
                            cb.ShowTextAligned(Element.ALIGN_LEFT, "(" + drr["DEPCONT_PHONE1"].ToString().Substring(0, 3) + ")" + drr["DEPCONT_PHONE1"].ToString().Substring(3, 3) + "-" + drr["DEPCONT_PHONE1"].ToString().Substring(6, 3), 380, Y_Pos, 0);

                            Y_Pos -= 15;
                            CheckBottomBorderReached(document, writer);
                            cb.ShowTextAligned(Element.ALIGN_LEFT, "Phone No: ", 310, Y_Pos, 0);
                            cb.ShowTextAligned(Element.ALIGN_LEFT, drr["DEPCONT_PHONE2"].ToString(), 380, Y_Pos, 0);

                            Y_Pos -= 15;
                            CheckBottomBorderReached(document, writer);
                            cb.ShowTextAligned(Element.ALIGN_LEFT, "Fax: ", 310, Y_Pos, 0);
                            cb.ShowTextAligned(Element.ALIGN_LEFT, drr["DEPCONT_FAX"].ToString(), 380, Y_Pos, 0);

                            Y_Pos -= 15;
                            CheckBottomBorderReached(document, writer);
                            cb.ShowTextAligned(Element.ALIGN_LEFT, "Email: ", 310, Y_Pos, 0);
                            cb.ShowTextAligned(Element.ALIGN_LEFT, drr["DEPCONT_EMAIL"].ToString(), 380, Y_Pos, 0);

                        }

                        // Header Print
                        X_Pos = 70;
                        Y_Pos -= 20;
                        CheckBottomBorderReached(document, writer);
                        cb.ShowTextAligned(Element.ALIGN_LEFT, "Threshold", X_Pos, Y_Pos, 0);
                        cb.ShowTextAligned(Element.ALIGN_LEFT, " Value", X_Pos, Y_Pos - 15, 0);
                        cb.ShowTextAligned(Element.ALIGN_LEFT, "Auto", X_Pos + 60, Y_Pos, 0);
                        cb.ShowTextAligned(Element.ALIGN_LEFT, "Refer", X_Pos + 60, Y_Pos - 15, 0);
                        cb.ShowTextAligned(Element.ALIGN_LEFT, "Edit", X_Pos + 100, Y_Pos, 0);
                        cb.ShowTextAligned(Element.ALIGN_LEFT, "Address", X_Pos + 100, Y_Pos - 15, 0);
                        cb.ShowTextAligned(Element.ALIGN_LEFT, "Year", X_Pos + 150, Y_Pos, 0);
                        cb.ShowTextAligned(Element.ALIGN_LEFT, "Generate", X_Pos + 190, Y_Pos, 0);
                        cb.ShowTextAligned(Element.ALIGN_LEFT, "App #", X_Pos + 190, Y_Pos - 15, 0);
                        cb.ShowTextAligned(Element.ALIGN_LEFT, "Next App", X_Pos + 240, Y_Pos, 0);
                        //cb.ShowTextAligned(Element.ALIGN_LEFT, "App", X_Pos + 250, Y_Pos - 15, 0);
                        cb.ShowTextAligned(Element.ALIGN_LEFT, "UOM", X_Pos + 310, Y_Pos, 0);
                        cb.ShowTextAligned(Element.ALIGN_LEFT, "Juvenile", X_Pos + 350, Y_Pos, 0);
                        cb.ShowTextAligned(Element.ALIGN_LEFT, "From", X_Pos + 350, Y_Pos - 15, 0);
                        cb.ShowTextAligned(Element.ALIGN_LEFT, "Age", X_Pos + 400, Y_Pos, 0);
                        cb.ShowTextAligned(Element.ALIGN_LEFT, "To", X_Pos + 400, Y_Pos - 15, 0);
                        cb.ShowTextAligned(Element.ALIGN_LEFT, "Senior", X_Pos + 430, Y_Pos, 0);
                        cb.ShowTextAligned(Element.ALIGN_LEFT, "From", X_Pos + 430, Y_Pos - 15, 0);
                        cb.ShowTextAligned(Element.ALIGN_LEFT, "Age", X_Pos + 470, Y_Pos, 0);
                        cb.ShowTextAligned(Element.ALIGN_LEFT, "To", X_Pos + 470, Y_Pos - 15, 0);
                        cb.EndText();

                        X_Pos = 70;
                        Y_Pos -= 20;
                        //CheckBottomBorderReached(document, writer);

                        Print_Line();

                        cb.BeginText();
                        cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES).BaseFont, 12);

                        Y_Pos -= 20;
                        CheckBottomBorderReached(document, writer);
                        cb.ShowTextAligned(Element.ALIGN_LEFT, drr["DEP_THRESHOLD"].ToString(), X_Pos + 10, Y_Pos, 0);
                        cb.ShowTextAligned(Element.ALIGN_LEFT, drr["DEP_AUTO_REFER"].ToString(), X_Pos + 70, Y_Pos, 0);
                        cb.ShowTextAligned(Element.ALIGN_LEFT, drr["DEP_ADDRESS_EDIT"].ToString(), X_Pos + 110, Y_Pos, 0);
                        cb.ShowTextAligned(Element.ALIGN_LEFT, drr["DEP_YEAR"].ToString(), X_Pos + 150, Y_Pos, 0);
                        cb.ShowTextAligned(Element.ALIGN_LEFT, drr["DEP_GENERATE_APPS"].ToString(), X_Pos + 200, Y_Pos, 0);
                        cb.ShowTextAligned(Element.ALIGN_LEFT, drr["DEP_APP_NO"].ToString(), X_Pos + 240, Y_Pos, 0);
                        cb.ShowTextAligned(Element.ALIGN_LEFT, drr["DEP_UNIT_CALC"].ToString(), X_Pos + 320, Y_Pos, 0);
                        cb.ShowTextAligned(Element.ALIGN_LEFT, drr["DEP_JUV_FROM_AGE"].ToString(), X_Pos + 360, Y_Pos, 0);
                        cb.ShowTextAligned(Element.ALIGN_LEFT, drr["DEP_JUV_TO_AGE"].ToString(), X_Pos + 410, Y_Pos, 0);
                        cb.ShowTextAligned(Element.ALIGN_LEFT, drr["DEP_SEN_TO_AGE"].ToString(), X_Pos + 440, Y_Pos, 0);
                        cb.ShowTextAligned(Element.ALIGN_LEFT, drr["DEP_SEN_TO_AGE"].ToString(), X_Pos + 480, Y_Pos, 0);
                        //cb.EndText();

                        //cb.BeginText();
                        cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES).BaseFont, 12);
                        X_Pos = 50;
                        Y_Pos -= 20;
                        CheckBottomBorderReached(document, writer);
                        cb.ShowTextAligned(Element.ALIGN_LEFT, "Excluded Income Types :", X_Pos, Y_Pos, 0);
                        cb.ShowTextAligned(Element.ALIGN_LEFT, drr["DEP_INCOME_ARRAY"].ToString(), X_Pos + 150, Y_Pos, 0);
                        Y_Pos -= 15;
                        CheckBottomBorderReached(document, writer);
                        cb.ShowTextAligned(Element.ALIGN_LEFT, "Excluded Relationships:", X_Pos, Y_Pos, 0);
                        cb.ShowTextAligned(Element.ALIGN_LEFT, drr["DEP_RELATION_ARRAY"].ToString(), X_Pos + 150, Y_Pos, 0);
                        cb.EndText();

                        Y_Pos -= 15;
                        Print_Line();

                        cb.BeginText();
                        cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES).BaseFont, 13);
                        X_Pos = 240;
                        Y_Pos -= 20;
                        CheckBottomBorderReached(document, writer);
                        cb.ShowTextAligned(Element.ALIGN_CENTER, "Poverty Guidelines", X_Pos, Y_Pos, 0);

                        X_Pos = 70;
                        Y_Pos -= 20;
                        cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES).BaseFont, 12);
                        CheckBottomBorderReached(document, writer);
                        cb.ShowTextAligned(Element.ALIGN_LEFT, "Fed OMB:  " + drr["DEP_FED_USED"].ToString(), X_Pos, Y_Pos, 0);
                        cb.ShowTextAligned(Element.ALIGN_LEFT, "CMI:      " + drr["DEP_CMI_USED"].ToString(), X_Pos + 250, Y_Pos, 0);
                        Y_Pos -= 15;
                        CheckBottomBorderReached(document, writer);
                        cb.ShowTextAligned(Element.ALIGN_LEFT, "HUD:         " + drr["DEP_HUD_USED"].ToString(), X_Pos, Y_Pos, 0);
                        cb.ShowTextAligned(Element.ALIGN_LEFT, "SMI:      " + drr["DEP_SMI_USED"].ToString(), X_Pos + 250, Y_Pos, 0);
                        cb.EndText();

                        Y_Pos -= 15;
                        Print_Line();

                        cb.BeginText();
                        cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES).BaseFont, 13);
                        X_Pos = 240;
                        Y_Pos -= 20;
                        CheckBottomBorderReached(document, writer);
                        cb.ShowTextAligned(Element.ALIGN_CENTER, "Eligibility Criteria", X_Pos, Y_Pos, 0);

                        X_Pos = 90;
                        Y_Pos -= 20;
                        cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES).BaseFont, 12);
                        CheckBottomBorderReached(document, writer);
                        cb.ShowTextAligned(Element.ALIGN_LEFT, "Include in SIM: " + drr["DEP_INCL_SIM"].ToString(), X_Pos, Y_Pos, 0);
                        cb.ShowTextAligned(Element.ALIGN_LEFT, "Calculate Eligibility: " + drr["DEP_CALC_ELIG"].ToString(), X_Pos + 230, Y_Pos, 0);
                        cb.EndText();

                        Y_Pos -= 50;
                        cb.BeginText();
                        cb.SetFontAndSize(bfTimes, 12);
                        CheckBottomBorderReached(document, writer);
                        cb.EndText();

                        Priv_Agency = drr["DEP_AGENCY"].ToString();
                        Priv_Depart = drr["DEP_DEPT"].ToString();
                    }
                }
                catch (Exception ex) { document.Add(new Paragraph("Aborted due to Exception............................................... ")); }
            }
        }


        private void FieldControl_Maintenance(Document document, PdfWriter writer, bool Format, string strPublicCode, string ScreenCode)
        {

            BaseFont bf_Calibri = BaseFont.CreateFont("c:/windows/fonts/calibri.ttf", BaseFont.WINANSI, BaseFont.EMBEDDED);
            //BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, false);
            Font fc = new iTextSharp.text.Font(bf_Calibri, 12);
            Font fb = new iTextSharp.text.Font(bf_Calibri, 12, 1);
            PdfTemplate template = writer.DirectContent.CreateTemplate(100, 100);

            string ScrName = ((Captain.Common.Utilities.ListItem)CmbAgyTab.SelectedItem).ID.ToString().Trim();

            try
            {
                if (ScreenCode == "**")
                {
                    DataSet dsScreens = DatabaseLayer.FieldControlsDB.Browse_FldCntl();
                    DataTable dtScreens = dsScreens.Tables[0];
                    DataView dvScreens = dtScreens.DefaultView;
                    dtScreens = dvScreens.ToTable(true, "FLD_SCR_CODE");
                    int rowCnt = 0;
                    //dtScreens.DefaultView.ToTable(true, "FLD_SCR_CODE");
                    if (dtScreens.Rows.Count > 0)
                    {
                        foreach (DataRow drScreens in dtScreens.Rows)
                        {
                            ScreenCode = drScreens["FLD_SCR_CODE"].ToString().Trim();
                            //Name_Screen(ScreenCode);
                            if (rbStandard.Checked == true)
                                GetStandardQuestions(document, writer, Format, ScreenCode);
                            else if (rbCustom.Checked == true)
                                GetCustomQuestions(document, writer, Format, ScreenCode);
                            else if (rbBoth.Checked == true)
                            {

                                GetStandardQuestions(document, writer, Format, ScreenCode);

                                //PdfPTable Table_Space = new PdfPTable(1);
                                //Table_Space.HorizontalAlignment = PdfContentByte.ALIGN_CENTER;
                                //Table_Space.TotalWidth = 500f;
                                //Table_Space.WidthPercentage = 100f;
                                //Table_Space.LockedWidth = true;
                                //Table_Space.SpacingBefore = 20f;

                                //PdfPCell Ques_Type = new PdfPCell(new Phrase("", fb));
                                //Ques_Type.HorizontalAlignment = Element.ALIGN_CENTER;
                                ////Ques_Type.Colspan = 2;
                                //Ques_Type.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                //Table_Space.AddCell(Ques_Type);
                                //document.Add(Table_Space);

                                GetCustomQuestions(document, writer, Format, ScreenCode);
                                DataTable dt = new DataTable();
                                //DataRow dr;
                                DataSet dsFld = DatabaseLayer.FieldControlsDB.GetFLDCNTLHIE(ScreenCode, HieCode, "FLDCNTL");
                                if (dsFld.Tables.Count > 0)
                                    dt = dsFld.Tables[0];

                                DataTable dtCustom = new DataTable();
                                DataSet dsCustom = DatabaseLayer.CaseSum.GetELIGCUSTOMQUES(HieCode);
                                if (dsCustom.Tables.Count > 0)
                                {
                                    dtCustom = dsCustom.Tables[0];
                                    DataView dv = dtCustom.DefaultView;
                                    dv.RowFilter = "FLDH_SCR_CODE='" + ScreenCode + "'";
                                    dtCustom = dv.ToTable();
                                }
                                if ((dtCustom.Rows.Count > 0) || (dt.Rows.Count > 0))
                                {
                                    document.NewPage();
                                    rowCnt++;
                                }
                            }
                        }
                    }
                    if (rowCnt == 0)
                    {
                        PdfContentByte cb = writer.DirectContent;
                        cb.BeginText();
                        cb.SetFontAndSize(bf_Calibri, 15);
                        cb.SetColorFill(BaseColor.RED);
                        cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Field Controls are not defined for this Hierarchy", 300, 400, 0);
                        cb.EndText();
                    }

                }
                else
                {

                    if (rbStandard.Checked == true)
                        GetStandardQuestions(document, writer, Format, ScreenCode);
                    else if (rbCustom.Checked == true)
                        GetCustomQuestions(document, writer, Format, ScreenCode);
                    else if (rbBoth.Checked == true)
                    {
                        //PdfPTable Table_StdQuesHead = new PdfPTable(1);
                        //Table_StdQuesHead.HorizontalAlignment = PdfContentByte.ALIGN_CENTER;
                        //Table_StdQuesHead.TotalWidth = 500f;
                        //Table_StdQuesHead.WidthPercentage = 100f;
                        //Table_StdQuesHead.LockedWidth = true;
                        ////Table_StdQuesHead.SpacingBefore = 20f;

                        //PdfPCell Ques_Type = new PdfPCell(new Phrase("Standard Questions", fb));
                        //Ques_Type.HorizontalAlignment = Element.ALIGN_CENTER;
                        ////Ques_Type.Colspan = 2;
                        //Ques_Type.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //Table_StdQuesHead.AddCell(Ques_Type);
                        //document.Add(Table_StdQuesHead);

                        GetStandardQuestions(document, writer, Format, ScreenCode);

                        DataTable dt = new DataTable();
                        //DataRow dr;
                        DataSet dsFld = DatabaseLayer.FieldControlsDB.GetFLDCNTLHIE(ScreenCode, HieCode, "FLDCNTL");
                        if (dsFld.Tables.Count > 0)
                            dt = dsFld.Tables[0];

                        DataTable dtCustom = new DataTable();
                        DataSet dsCustom = DatabaseLayer.CaseSum.GetELIGCUSTOMQUES(HieCode);
                        if (dsCustom.Tables.Count > 0)
                        {
                            dtCustom = dsCustom.Tables[0];
                            DataView dv = dtCustom.DefaultView;
                            dv.RowFilter = "FLDH_SCR_CODE='" + ScreenCode + "'";
                            dtCustom = dv.ToTable();
                        }
                        if (dtCustom.Rows.Count > 0 || dt.Rows.Count > 0)
                            document.NewPage();

                        //PdfPTable Table_CustomQuesHead = new PdfPTable(1);
                        //Table_CustomQuesHead.HorizontalAlignment = PdfContentByte.ALIGN_CENTER;
                        //Table_CustomQuesHead.TotalWidth = 500f;
                        //Table_CustomQuesHead.WidthPercentage = 100f;
                        //Table_CustomQuesHead.LockedWidth = true;
                        ////Table_StdQuesHead.SpacingBefore = 20f;

                        //PdfPCell CustQues_Type = new PdfPCell(new Phrase("Custom Questions", fb));
                        //CustQues_Type.HorizontalAlignment = Element.ALIGN_CENTER;
                        ////CustQues_Type.Colspan = 2;
                        //CustQues_Type.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        //Table_CustomQuesHead.AddCell(CustQues_Type);
                        //document.Add(Table_CustomQuesHead);
                        GetCustomQuestions(document, writer, Format, ScreenCode);
                    }
                }
            }
            catch (Exception ex) { document.Add(new Paragraph("Aborted due to Exception............................................... ")); }

        }

        private void AccessLogList(Document document, PdfWriter writer)
        {
            string SortCol = null;
            if (RblEmrServ.Checked == true)
                SortCol = "Description";

            BaseFont bf_Calibri = BaseFont.CreateFont("c:/windows/fonts/calibri.ttf", BaseFont.WINANSI, BaseFont.EMBEDDED);
            iTextSharp.text.Font Calibri = new iTextSharp.text.Font(bf_Calibri, 10);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_Calibri, 8);
            iTextSharp.text.Font SubHeadFont = new iTextSharp.text.Font(bf_Calibri, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_Calibri, 9, 3);
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_Calibri, 8, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
            iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_Calibri, 9, 2);
            iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_Calibri, 9, 4);

            //BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            //iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 10);
            //iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 10);
            //iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 12, 3);
            //iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(1, 12, 1);
            //iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 9, 2);
            //iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 9, 4);

            try
            {
                string strType = string.Empty;
                string strUsers = string.Empty;
                if (rdoAll.Checked)
                    strType = "ALL";

                if (((Captain.Common.Utilities.ListItem)CmbUser.SelectedItem).Value.ToString().Trim() != "**")
                    strUsers = ((Captain.Common.Utilities.ListItem)CmbUser.SelectedItem).Value.ToString();

                string startdate = string.Empty; string EndDate = string.Empty;
                startdate = CommonFunctions.ChangeDateFormat(dtselect.Value.ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);
                EndDate = CommonFunctions.ChangeDateFormat(dtSelectToDate.Value.ToShortDateString(), Consts.DateTimeFormats.DateSaveFormat, Consts.DateTimeFormats.DateDisplayFormat);

                DataSet ds = DatabaseLayer.ADMNB001DB.GETLOGUSERS(strUsers, startdate, EndDate, strType);
                DataTable dt = ds.Tables[0];
                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, false);
                Font fc = new iTextSharp.text.Font(bfTimes, 12, 2);
                PdfTemplate template = writer.DirectContent.CreateTemplate(100, 100);

                BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, BaseFont.NOT_EMBEDDED);

                PdfPTable table = new PdfPTable(4);
                table.TotalWidth = 500f;
                table.WidthPercentage = 100;
                table.LockedWidth = true;
                float[] widths = new float[] { 40f, 40f, 40f, 40f };
                table.SetWidths(widths);
                table.HorizontalAlignment = Element.ALIGN_CENTER;
                table.SpacingBefore = 20f;
                table.SpacingAfter = 30f;
                Y_Pos = 550;

                PdfPCell cell2 = new PdfPCell(new Phrase("USER ID", TblFontBold));
                cell2.HorizontalAlignment = Element.ALIGN_LEFT;
                //cell2.Border = Rectangle.NO_BORDER;
                //cell2.BorderWidthBottom = 0.7f;
                cell2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                cell2.BorderColor = BaseColor.WHITE;
                cell2.FixedHeight = 15f;
                table.AddCell(cell2);

                PdfPCell cell1 = new PdfPCell(new Phrase("LOG IN TIME", TblFontBold));
                // cell.BackgroundColor = new BaseColor(190, 120, 204);
                cell1.HorizontalAlignment = Element.ALIGN_LEFT;
                //cell1.Border = Rectangle.NO_BORDER;
                //cell1.BorderWidthBottom = 0.7f;
                cell1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                cell1.BorderColor = BaseColor.WHITE;
                cell1.FixedHeight = 15f;
                table.AddCell(cell1);

                PdfPCell cell3 = new PdfPCell(new Phrase("LOG OUT TIME", TblFontBold));
                cell3.HorizontalAlignment = Element.ALIGN_LEFT;
                //cell3.Border = Rectangle.NO_BORDER;
                //cell3.BorderWidthBottom = 0.7f;
                cell3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                cell3.BorderColor = BaseColor.WHITE;
                cell3.FixedHeight = 15f;
                table.AddCell(cell3);

                PdfPCell cell4 = new PdfPCell(new Phrase("IP ADDRESS", TblFontBold));
                cell4.HorizontalAlignment = Element.ALIGN_LEFT;
                //cell4.Border = Rectangle.NO_BORDER;
                //cell4.BorderWidthBottom = 0.7f;
                cell4.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                cell4.BorderColor = BaseColor.WHITE;
                cell4.FixedHeight = 15f;
                table.AddCell(cell4);

                table.HeaderRows = 1;

                if (dt.Rows.Count > 0)
                {
                    Font Cell_Font = new Font();
                    Cell_Font.SetColor(249, 21, 21);
                    foreach (DataRow dr in dt.Rows)
                    {
                        PdfPCell Service = new PdfPCell(new Phrase(dr["LOG_USERID"].ToString(), TableFont));
                        Service.HorizontalAlignment = Element.ALIGN_LEFT;
                        //Service.Border = Rectangle.NO_BORDER;
                        Service.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                        Service.BorderColor = BaseColor.WHITE;
                        Service.FixedHeight = 15;
                        table.AddCell(Service);

                        PdfPCell cell = new PdfPCell(new Phrase(dr["LOG_INTIME"].ToString(), TableFont));
                        //cell.Border = Rectangle.NO_BORDER;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                        cell.BorderColor = BaseColor.WHITE;
                        cell.FixedHeight = 15;
                        table.AddCell(cell);

                        PdfPCell pdfout = new PdfPCell(new Phrase(dr["LOG_OUTTIME"].ToString(), TableFont));
                        //pdfout.Border = Rectangle.NO_BORDER;
                        pdfout.HorizontalAlignment = Element.ALIGN_LEFT;
                        pdfout.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#ebf4fa"));
                        pdfout.BorderColor = BaseColor.WHITE;
                        pdfout.FixedHeight = 15;
                        table.AddCell(pdfout);

                        PdfPCell pdfIP = new PdfPCell(new Phrase(dr["LOG_IP"].ToString(), TableFont));
                        //pdfIP.Border = Rectangle.NO_BORDER;
                        pdfIP.HorizontalAlignment = Element.ALIGN_LEFT;
                        pdfIP.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));   
                        pdfIP.BorderColor = BaseColor.WHITE;
                        pdfIP.FixedHeight = 15;
                        table.AddCell(pdfIP);

                    }

                }
                else
                {
                    cb.SetFontAndSize(bf_Calibri, 20);
                    cb.BeginText();
                    cb.SetColorFill(BaseColor.RED);
                    cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "No Records Found", 250, 400, 0);
                    cb.EndText();
                }

                document.Add(table);
            }
            catch (Exception ex) { document.Add(new Paragraph("Aborted due to Exception............................................... ")); }
        }

        private void ReportLogList(Document document, PdfWriter writer)
        {

            BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/TIMES.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
            iTextSharp.text.Font Times = new iTextSharp.text.Font(bf_times, 10);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_times, 10);
            iTextSharp.text.Font TableFontbig = new iTextSharp.text.Font(bf_times, 12);
            iTextSharp.text.Font TableFontBoldItalic = new iTextSharp.text.Font(bf_times, 12, 3);
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_times, 12, 1);
            iTextSharp.text.Font TblFontItalic = new iTextSharp.text.Font(bf_times, 9, 2);
            iTextSharp.text.Font Timesline = new iTextSharp.text.Font(bf_times, 9, 4);

            try
            {
                string strdateType = string.Empty;
                string strUsers = string.Empty;
                string strModulecode = string.Empty;
                string strRepCode = string.Empty;
                if (rdoRepAll.Checked)
                    strdateType = string.Empty;
                else
                    strdateType = dtRepDate.Value.ToShortDateString();

                if (((Captain.Common.Utilities.ListItem)CmbUser.SelectedItem).Value.ToString().Trim() != "**")
                    strUsers = ((Captain.Common.Utilities.ListItem)CmbUser.SelectedItem).Value.ToString();


                List<ReportLogEntity> reportentity = _model.AdhocData.GetReportLog(strRepCode, strUsers, strModulecode, strdateType);
                BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, false);
                Font fc = new iTextSharp.text.Font(bfTimes, 12, 2);
                PdfTemplate template = writer.DirectContent.CreateTemplate(100, 100);

                BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.WINANSI, BaseFont.NOT_EMBEDDED);

                PdfPTable table = new PdfPTable(5);
                table.TotalWidth = 500f;
                table.WidthPercentage = 100;
                table.LockedWidth = true;
                float[] widths = new float[] { 30f, 30f, 30f, 30f, 30f };
                table.SetWidths(widths);
                table.HorizontalAlignment = Element.ALIGN_CENTER;
                table.SpacingBefore = 20f;
                table.SpacingAfter = 30f;
                Y_Pos = 550;

                PdfPCell cell2 = new PdfPCell(new Phrase("USER ID", TblFontBold));
                cell2.HorizontalAlignment = Element.ALIGN_CENTER;
                cell2.Border = Rectangle.NO_BORDER;
                cell2.BorderWidthBottom = 0.7f;
                table.AddCell(cell2);

                PdfPCell cell1 = new PdfPCell(new Phrase("Report Code", TblFontBold));
                // cell.BackgroundColor = new BaseColor(190, 120, 204);
                cell1.HorizontalAlignment = Element.ALIGN_CENTER;
                cell1.Border = Rectangle.NO_BORDER;
                cell1.BorderWidthBottom = 0.7f;
                table.AddCell(cell1);

                PdfPCell cell3 = new PdfPCell(new Phrase("ModuleCode", TblFontBold));
                cell3.HorizontalAlignment = Element.ALIGN_CENTER;
                cell3.Border = Rectangle.NO_BORDER;
                cell3.BorderWidthBottom = 0.7f;
                table.AddCell(cell3);

                PdfPCell cell4 = new PdfPCell(new Phrase("Date", TblFontBold));
                cell4.HorizontalAlignment = Element.ALIGN_CENTER;
                cell4.Border = Rectangle.NO_BORDER;
                cell4.BorderWidthBottom = 0.7f;
                table.AddCell(cell4);

                PdfPCell cell5 = new PdfPCell(new Phrase("File Name", TblFontBold));
                cell5.HorizontalAlignment = Element.ALIGN_CENTER;
                cell5.Border = Rectangle.NO_BORDER;
                cell5.BorderWidthBottom = 0.7f;
                table.AddCell(cell5);

                //   table.HeaderRows = 1;

                if (reportentity.Count > 0)
                {
                    Font Cell_Font = new Font();
                    Cell_Font.SetColor(249, 21, 21);
                    foreach (ReportLogEntity dr in reportentity)
                    {
                        PdfPCell Service = new PdfPCell(new Phrase(dr.REP_EMP_CODE.ToString(), TableFont));
                        Service.HorizontalAlignment = Element.ALIGN_LEFT;
                        Service.Border = Rectangle.NO_BORDER;
                        table.AddCell(Service);

                        PdfPCell cell = new PdfPCell(new Phrase(dr.REP_PROG_NAME.ToString(), TableFont));
                        cell.Border = Rectangle.NO_BORDER;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.AddCell(cell);

                        PdfPCell pdfout = new PdfPCell(new Phrase(dr.REP_MODULE_CODE.ToString(), TableFont));
                        pdfout.Border = Rectangle.NO_BORDER;
                        pdfout.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.AddCell(pdfout);

                        PdfPCell pdfIP = new PdfPCell(new Phrase(dr.REP_DATE_ADD.ToString(), TableFont));
                        pdfIP.Border = Rectangle.NO_BORDER;
                        pdfIP.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.AddCell(pdfIP);

                        PdfPCell pdfName = new PdfPCell(new Phrase(dr.REP_FILE_NAME.ToString(), TableFont));
                        pdfName.Border = Rectangle.NO_BORDER;
                        pdfName.HorizontalAlignment = Element.ALIGN_LEFT;
                        table.AddCell(pdfName);


                        PdfPCell pdfparalist = new PdfPCell(new Phrase("", TableFont));
                        pdfparalist.Border = Rectangle.NO_BORDER;
                        pdfparalist.HorizontalAlignment = Element.ALIGN_LEFT;
                        pdfparalist.Colspan = 5;
                        table.AddCell(pdfparalist);

                        PdfPCell pdfparalist1 = new PdfPCell(new Phrase("Selected Report Parameters", TableFontbig));
                        pdfparalist1.Border = Rectangle.NO_BORDER;
                        pdfparalist1.HorizontalAlignment = Element.ALIGN_CENTER;
                        pdfparalist1.Colspan = 5;
                        table.AddCell(pdfparalist1);

                        PdfPCell pdfparalist2 = new PdfPCell(new Phrase("", TableFont));
                        pdfparalist2.Border = Rectangle.NO_BORDER;
                        pdfparalist2.HorizontalAlignment = Element.ALIGN_LEFT;
                        pdfparalist2.Colspan = 5;
                        table.AddCell(pdfparalist2);


                        if (dr.Rep_Table.Rows.Count > 0)
                        {
                            foreach (DataRow item in dr.Rep_Table.Rows)
                            {
                                PdfPCell pdfData = new PdfPCell(new Phrase(item[0].ToString() + "  ", TableFont));
                                pdfData.HorizontalAlignment = Element.ALIGN_LEFT;
                                pdfData.Border = Rectangle.NO_BORDER;
                                //pdfData.Colspan = 5;
                                table.AddCell(pdfData);
                            }


                        }

                        PdfPCell pdfspace = new PdfPCell(new Phrase("", TableFont));
                        pdfspace.Border = Rectangle.NO_BORDER;
                        pdfspace.HorizontalAlignment = Element.ALIGN_LEFT;
                        pdfspace.Colspan = 5;
                        table.AddCell(pdfspace);

                        PdfPCell pdfborder = new PdfPCell(new Phrase("", TableFont));
                        pdfborder.Border = Rectangle.BOTTOM_BORDER;
                        pdfborder.HorizontalAlignment = Element.ALIGN_LEFT;
                        pdfborder.Colspan = 5;
                        table.AddCell(pdfborder);

                        PdfPCell pdfspace1 = new PdfPCell(new Phrase("", TableFont));
                        pdfspace1.Border = Rectangle.NO_BORDER;
                        pdfspace1.HorizontalAlignment = Element.ALIGN_LEFT;
                        pdfspace1.Colspan = 5;
                        table.AddCell(pdfspace1);

                    }

                }
                else
                {
                    cb.SetFontAndSize(bf_times, 20);
                    cb.BeginText();
                    cb.SetColorFill(BaseColor.RED);
                    cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "No Records Found", 250, 400, 0);
                    cb.EndText();
                }

                document.Add(table);
            }
            catch (Exception ex) { document.Add(new Paragraph("Aborted due to Exception............................................... ")); }
        }


        private void GetStandardQuestions(Document document, PdfWriter writer, bool Formattype, string ScrCode)
        {
            //iTextSharp.text.Image _image_UnChecked = iTextSharp.text.Image.GetInstance(Application.MapPath("~\\Resources\\Icons\\16X16\\CheckBoxUnchecked.JPG"));
            //iTextSharp.text.Image _image_Checked = iTextSharp.text.Image.GetInstance(Application.MapPath("~\\Resources\\Icons\\16X16\\CheckBoxChecked.JPG"));
            //_image_UnChecked.ScalePercent(80f);
            //_image_Checked.ScalePercent(80f);
            //BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, false);
            //Font fc = new iTextSharp.text.Font(bf_Calibri, 12);
            //Font fb = new iTextSharp.text.Font(bf_Calibri, 12, 1);

            BaseFont bf_Calibri = BaseFont.CreateFont("c:/windows/fonts/calibri.ttf", BaseFont.WINANSI, BaseFont.EMBEDDED);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_Calibri, 8);
            iTextSharp.text.Font SubHeadFont = new iTextSharp.text.Font(bf_Calibri, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_Calibri, 8, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
            iTextSharp.text.Font TblFontBoldColor = new iTextSharp.text.Font(bf_Calibri, 9, 1, BaseColor.BLACK);
            BaseFont bf_Wingdings2 = BaseFont.CreateFont(Application.MapPath("~\\Fonts\\WINGDNG2.TTF"), BaseFont.WINANSI, BaseFont.EMBEDDED);
            iTextSharp.text.Font FontWingdings = new iTextSharp.text.Font(bf_Wingdings2, 8, iTextSharp.text.Font.BOLD, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#01a601")));

            DataTable dt = new DataTable();
            //DataRow dr;
            DataSet dsFld = DatabaseLayer.FieldControlsDB.GetFLDCNTLHIE(ScrCode, HieCode, "FLDCNTL");
            if (dsFld.Tables.Count > 0)
                dt = dsFld.Tables[0];
            if (Formattype)
            {
                //if (dt.Rows.Count > 0)
                //{
                //    DataView dv = dt.DefaultView;
                //    dv.Sort = "FLDH_CODE,FLDH_ENABLED,FLDH_REQUIRED,FLDH_SHARED";
                //    dt = dv.ToTable();
                //}
            }
            else
            {
                if (dt.Rows.Count > 0)
                {
                    DataView dv = dt.DefaultView;
                    dv.Sort = "STF_DESC";
                    dt = dv.ToTable();
                }
            }

            if (dt.Rows.Count > 0)
            {
                string Hierarchy = dt.Rows[0]["FLDH_SCR_HIE"].ToString().Trim();
                Hierarchy = Hierarchy.Substring(0, 2).Trim() + "-" + Hierarchy.Substring(2, 2).Trim() + "-" + Hierarchy.Substring(4, 2).Trim();

                PdfPTable Table_StdQuesHead = new PdfPTable(1);
                Table_StdQuesHead.HorizontalAlignment = PdfContentByte.ALIGN_CENTER;
                Table_StdQuesHead.TotalWidth = 500f;
                Table_StdQuesHead.WidthPercentage = 100f;
                Table_StdQuesHead.LockedWidth = true;
                //Table_StdQuesHead.SpacingBefore = 20f;
                Screen_Name = Name_Screen(ScrCode);

                PdfPCell Ques_Type = new PdfPCell(new Phrase("Screen: " + Screen_Name, TblFontBoldColor));
                Ques_Type.HorizontalAlignment = Element.ALIGN_CENTER;
                //Ques_Type.Colspan = 2;
                Ques_Type.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Table_StdQuesHead.AddCell(Ques_Type);
                document.Add(Table_StdQuesHead);

                PdfPTable Table_StdQues = new PdfPTable(4);
                Table_StdQues.HorizontalAlignment = PdfContentByte.ALIGN_CENTER;
                Table_StdQues.TotalWidth = 550f;
                float[] widths = new float[] { 80f, 20f, 20f, 20f };
                Table_StdQues.SetWidths(widths);
                Table_StdQues.WidthPercentage = 100f;
                Table_StdQues.LockedWidth = true;
                Table_StdQues.SpacingBefore = 20f;

                if (Hierarchy != strPublicCode)
                {
                    PdfPCell Special = new PdfPCell(new Phrase("", TblFontBoldColor));
                    Special.HorizontalAlignment = Element.ALIGN_LEFT;
                    //Special.Colspan = 2;
                    Special.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Table_StdQues.AddCell(Special);

                    PdfPCell Special1 = new PdfPCell(new Phrase("Applicable Hierarchy: " + Hierarchy, TblFontBoldColor));
                    Special1.HorizontalAlignment = Element.ALIGN_LEFT;
                    Special1.Colspan = 3;
                    Special1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Table_StdQues.AddCell(Special1);

                    PdfPCell ScreenNAme = new PdfPCell(new Phrase("Standard Questions", TblFontBoldColor));
                    ScreenNAme.HorizontalAlignment = Element.ALIGN_LEFT;
                    //ScreenNAme.Colspan = 2;
                    ScreenNAme.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Table_StdQues.AddCell(ScreenNAme);

                    PdfPCell Hier = new PdfPCell(new Phrase("Hierarchy: " + strPublicCode + " (Not Defined)", TblFontBoldColor));
                    Hier.HorizontalAlignment = Element.ALIGN_LEFT;
                    Hier.Colspan = 3;
                    Hier.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Table_StdQues.AddCell(Hier);
                }
                else
                {
                    PdfPCell ScreenNAme = new PdfPCell(new Phrase("Standard Questions", TblFontBoldColor));
                    ScreenNAme.HorizontalAlignment = Element.ALIGN_LEFT;
                    ScreenNAme.Colspan = 2;
                    ScreenNAme.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Table_StdQues.AddCell(ScreenNAme);

                    PdfPCell Hier = new PdfPCell(new Phrase("Hierarchy: " + strPublicCode, TblFontBoldColor));
                    Hier.HorizontalAlignment = Element.ALIGN_LEFT;
                    Hier.Colspan = 2;
                    Hier.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Table_StdQues.AddCell(Hier);
                }

                PdfPCell Space = new PdfPCell(new Phrase("", TblFontBold));
                Space.HorizontalAlignment = Element.ALIGN_LEFT;
                Space.Colspan = 4;
                Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Table_StdQues.AddCell(Space);

                PdfPCell Header_Desc = new PdfPCell(new Phrase("Description", TblFontBold));
                Header_Desc.HorizontalAlignment = Element.ALIGN_LEFT;
                //Header_Desc.Colspan = 4;
                //Header_Desc.Border = iTextSharp.text.Rectangle.BOX;
                Header_Desc.FixedHeight = 15f;
                Header_Desc.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));   
                Header_Desc.BorderColor = BaseColor.WHITE;
                Table_StdQues.AddCell(Header_Desc);

                PdfPCell Header_Enabled = new PdfPCell(new Phrase("Enabled", TblFontBold));
                Header_Enabled.HorizontalAlignment = Element.ALIGN_CENTER;
                //Header_Enabled.Colspan = 4;
                //Header_Enabled.Border = iTextSharp.text.Rectangle.BOX;
                Header_Enabled.FixedHeight = 15f;
                Header_Enabled.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                Header_Enabled.BorderColor = BaseColor.WHITE;
                Table_StdQues.AddCell(Header_Enabled);

                PdfPCell Header_Required = new PdfPCell(new Phrase("Required", TblFontBold));
                Header_Required.HorizontalAlignment = Element.ALIGN_CENTER;
                //Header_Required.Colspan = 4;
                //Header_Required.Border = iTextSharp.text.Rectangle.BOX;
                Header_Required.FixedHeight = 15f;
                Header_Required.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                Header_Required.BorderColor = BaseColor.WHITE;
                Table_StdQues.AddCell(Header_Required);

                PdfPCell Header_Shared = new PdfPCell(new Phrase("Shared", TblFontBold));
                Header_Shared.HorizontalAlignment = Element.ALIGN_CENTER;
                //Header_Shared.Colspan = 4;
                //Header_Shared.Border = iTextSharp.text.Rectangle.BOX;
                Header_Shared.FixedHeight = 15f;
                Header_Shared.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                Header_Shared.BorderColor = BaseColor.WHITE;
                Table_StdQues.AddCell(Header_Shared);

                foreach (DataRow dr in dt.Rows)
                {
                    PdfPCell QuesDesc = new PdfPCell(new Phrase(dr["STF_DESC"].ToString().Trim(), TableFont));
                    QuesDesc.HorizontalAlignment = Element.ALIGN_LEFT;
                    //QuesDesc.Border = iTextSharp.text.Rectangle.BOX;
                    QuesDesc.FixedHeight = 15f;
                    QuesDesc.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                    QuesDesc.BorderColor = BaseColor.WHITE;
                    Table_StdQues.AddCell(QuesDesc);


                    if (dr["FLDH_ENABLED"].ToString().Trim() == "Y")
                    {
                        string _imgicon = "P";
                        PdfPCell Enabled = new PdfPCell(new Phrase(_imgicon, FontWingdings));
                        Enabled.HorizontalAlignment = Element.ALIGN_CENTER;
                        Enabled.VerticalAlignment = Element.ALIGN_MIDDLE;
                        //Enabled.Border = iTextSharp.text.Rectangle.BOX;
                        Enabled.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                        Enabled.BorderColor = BaseColor.WHITE;
                        Enabled.FixedHeight = 15f;
                        Table_StdQues.AddCell(Enabled);
                    }
                    else
                    {
                        string _imgicon = "";
                        PdfPCell Enabled = new PdfPCell(new Phrase(_imgicon, FontWingdings));
                        Enabled.HorizontalAlignment = Element.ALIGN_CENTER;
                        Enabled.VerticalAlignment = Element.ALIGN_MIDDLE;
                        //Enabled.Border = iTextSharp.text.Rectangle.BOX;
                        Enabled.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                        Enabled.BorderColor = BaseColor.WHITE;
                        Enabled.FixedHeight = 15f;
                        Table_StdQues.AddCell(Enabled);
                    }

                    if (dr["FLDH_REQUIRED"].ToString().Trim() == "Y")
                    {
                        string _imgicon = "P";
                        PdfPCell Required = new PdfPCell(new Phrase(_imgicon, FontWingdings));
                        Required.HorizontalAlignment = Element.ALIGN_CENTER;
                        Required.VerticalAlignment = Element.ALIGN_MIDDLE;
                        //Required.Border = iTextSharp.text.Rectangle.BOX;
                        Required.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                        Required.BorderColor = BaseColor.WHITE;
                        Required.FixedHeight = 15f;
                        Table_StdQues.AddCell(Required);
                    }
                    else
                    {
                        string _imgicon = "";
                        PdfPCell Required = new PdfPCell(new Phrase(_imgicon, FontWingdings));
                        Required.HorizontalAlignment = Element.ALIGN_CENTER;
                        Required.VerticalAlignment = Element.ALIGN_MIDDLE;
                        //Required.Border = iTextSharp.text.Rectangle.BOX;
                        Required.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                        Required.BorderColor = BaseColor.WHITE;
                        Required.FixedHeight = 15f;
                        Table_StdQues.AddCell(Required);
                    }

                    if (dr["FLDH_SHARED"].ToString().Trim() == "Y")
                    {
                        string _imgicon = "P";
                        PdfPCell Shared = new PdfPCell(new Phrase(_imgicon, FontWingdings));
                        Shared.HorizontalAlignment = Element.ALIGN_CENTER;
                        Shared.VerticalAlignment = Element.ALIGN_MIDDLE;
                        //Shared.Border = iTextSharp.text.Rectangle.BOX;
                        Shared.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                        Shared.BorderColor = BaseColor.WHITE;
                        Shared.FixedHeight = 15f;
                        Table_StdQues.AddCell(Shared);
                    }
                    else
                    {
                        string _imgicon = "";
                        PdfPCell Shared = new PdfPCell(new Phrase(_imgicon, FontWingdings));
                        Shared.HorizontalAlignment = Element.ALIGN_CENTER;
                        Shared.VerticalAlignment = Element.ALIGN_MIDDLE;
                        //Shared.Border = iTextSharp.text.Rectangle.BOX;
                        Shared.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                        Shared.BorderColor = BaseColor.WHITE;
                        Shared.FixedHeight = 15f;
                        Table_StdQues.AddCell(Shared);
                    }
                }
                document.Add(Table_StdQues);
            }
            else
            {
                if (((Captain.Common.Utilities.ListItem)CmbAgyTab.SelectedItem).Value.ToString().Trim() != "**")
                {
                    PdfContentByte cb = writer.DirectContent;
                    cb.BeginText();
                    cb.SetFontAndSize(bf_Calibri, 15);
                    cb.SetColorFill(BaseColor.RED);
                    cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Field Controls are not defined for this Hierarchy", 300, 400, 0);
                    cb.EndText();
                }
            }
        }

        private void GetCustomQuestions(Document document, PdfWriter writer, bool formatType, string ScrCode)
        {
            //iTextSharp.text.Image _image_UnChecked = iTextSharp.text.Image.GetInstance(Application.MapPath("~\\Resources\\Icons\\16X16\\CheckBoxUnchecked.JPG"));
            //iTextSharp.text.Image _image_Checked = iTextSharp.text.Image.GetInstance(Application.MapPath("~\\Resources\\Icons\\16X16\\CheckBoxChecked.JPG"));
            //_image_UnChecked.ScalePercent(80f);
            //_image_Checked.ScalePercent(80f);

            //BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1250, false);
            //Font fc = new iTextSharp.text.Font(bfTimes, 12);
            //Font fb = new iTextSharp.text.Font(bfTimes, 12, 1);
            BaseFont bf_Calibri = BaseFont.CreateFont("c:/windows/fonts/calibri.ttf", BaseFont.WINANSI, BaseFont.EMBEDDED);
            iTextSharp.text.Font TableFont = new iTextSharp.text.Font(bf_Calibri, 8);
            iTextSharp.text.Font SubHeadFont = new iTextSharp.text.Font(bf_Calibri, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            iTextSharp.text.Font TblFontBold = new iTextSharp.text.Font(bf_Calibri, 8, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
            iTextSharp.text.Font TblFontBoldColor = new iTextSharp.text.Font(bf_Calibri, 9, 1, BaseColor.BLACK);
            BaseFont bf_Wingdings2 = BaseFont.CreateFont(Application.MapPath("~\\Fonts\\WINGDNG2.TTF"), BaseFont.WINANSI, BaseFont.EMBEDDED);
            iTextSharp.text.Font FontWingdings = new iTextSharp.text.Font(bf_Wingdings2, 8, iTextSharp.text.Font.BOLD, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#01a601")));

            List<CustRespEntity> custresplist = new List<CustRespEntity>();
            CustRespEntity Search_Custresp = new CustRespEntity(true);
            DataTable dtCustom = new DataTable();
            DataSet dsCustom = DatabaseLayer.CaseSum.GetELIGCUSTOMQUES(HieCode);
            if (dsCustom.Tables.Count > 0)
                dtCustom = dsCustom.Tables[0];
            if (formatType)
            {
                if (dtCustom.Rows.Count > 0)
                {
                    DataView dv = dtCustom.DefaultView;
                    dv.Sort = "FLDH_CODE";
                    dv.RowFilter = "FLDH_SCR_CODE='" + ScrCode + "'";
                    dtCustom = dv.ToTable();
                    //dtCustom.DefaultView.RowFilter = "FLDH_SCR_CODE='"+ScrCode+"'";
                    //DataView Newdv = dtCustom.DefaultView;
                    //DataTable newdtCustom = Newdv.ToTable();
                }
            }
            else
            {
                if (dtCustom.Rows.Count > 0)
                {
                    DataView dv = dtCustom.DefaultView;
                    dv.RowFilter = "FLDH_SCR_CODE='" + ScrCode + "'";
                    dv.Sort = "CUST_DESC";
                    dtCustom = dv.ToTable();
                    //dtCustom.DefaultView.RowFilter = "FLDH_SCR_CODE='" + ScrCode + "'";
                    //dtCustom.DefaultView.ToTable();
                }
            }

            if (dtCustom.Rows.Count > 0)
            {
                string Hierarchy = dtCustom.Rows[0]["FLDH_SCR_HIE"].ToString().Trim();
                Hierarchy = Hierarchy.Substring(0, 2).Trim() + "-" + Hierarchy.Substring(2, 2).Trim() + "-" + Hierarchy.Substring(4, 2).Trim();

                PdfPTable Table_CustomQuesHead = new PdfPTable(1);
                Table_CustomQuesHead.HorizontalAlignment = PdfContentByte.ALIGN_CENTER;
                Table_CustomQuesHead.TotalWidth = 500f;
                Table_CustomQuesHead.WidthPercentage = 100f;
                Table_CustomQuesHead.LockedWidth = true;
                //Table_CustomQuesHead.SpacingBefore = 20f;
                Screen_Name = Name_Screen(ScrCode);

                PdfPCell CustQues_Type = new PdfPCell(new Phrase("Screen: " + Screen_Name, TblFontBoldColor));
                CustQues_Type.HorizontalAlignment = Element.ALIGN_CENTER;
                //CustQues_Type.Colspan = 2;
                CustQues_Type.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Table_CustomQuesHead.AddCell(CustQues_Type);
                document.Add(Table_CustomQuesHead);

                PdfPTable Table_CustQues = new PdfPTable(8);
                Table_CustQues.HorizontalAlignment = PdfContentByte.ALIGN_CENTER;
                Table_CustQues.TotalWidth = 550f;
                float[] widths = new float[] { 30f, 15f, 45f, 30f, 20f, 18f, 30f, 35f };
                Table_CustQues.SetWidths(widths);
                Table_CustQues.WidthPercentage = 100f;
                Table_CustQues.LockedWidth = true;
                Table_CustQues.SpacingBefore = 20f;

                if (Hierarchy != strPublicCode)
                {
                    PdfPCell Special = new PdfPCell(new Phrase("", TblFontBoldColor));
                    Special.HorizontalAlignment = Element.ALIGN_LEFT;
                    Special.Colspan = 5;
                    Special.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Table_CustQues.AddCell(Special);

                    PdfPCell Special1 = new PdfPCell(new Phrase("Applicable Hierarchy: " + Hierarchy, TblFontBoldColor));
                    Special1.HorizontalAlignment = Element.ALIGN_LEFT;
                    Special1.Colspan = 3;
                    Special1.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Table_CustQues.AddCell(Special1);

                    PdfPCell ScreenNAme = new PdfPCell(new Phrase("Custom Questions", TblFontBoldColor));
                    ScreenNAme.HorizontalAlignment = Element.ALIGN_LEFT;
                    ScreenNAme.Colspan = 5;
                    ScreenNAme.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Table_CustQues.AddCell(ScreenNAme);

                    PdfPCell Hier = new PdfPCell(new Phrase("Hierarchy: " + strPublicCode + " (Not Defined)", TblFontBoldColor));
                    Hier.HorizontalAlignment = Element.ALIGN_LEFT;
                    Hier.Colspan = 3;
                    Hier.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Table_CustQues.AddCell(Hier);
                }
                else
                {
                    PdfPCell ScreenNAme = new PdfPCell(new Phrase("Custom Questions", TblFontBoldColor));
                    ScreenNAme.HorizontalAlignment = Element.ALIGN_LEFT;
                    ScreenNAme.Colspan = 5;
                    ScreenNAme.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Table_CustQues.AddCell(ScreenNAme);

                    PdfPCell Hier = new PdfPCell(new Phrase("Hierarchy: " + strPublicCode, TblFontBoldColor));
                    Hier.HorizontalAlignment = Element.ALIGN_LEFT;
                    Hier.Colspan = 3;
                    Hier.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    Table_CustQues.AddCell(Hier);
                }

                PdfPCell Space = new PdfPCell(new Phrase("", TblFontBold));
                Space.HorizontalAlignment = Element.ALIGN_LEFT;
                Space.Colspan = 8;
                Space.Border = iTextSharp.text.Rectangle.NO_BORDER;
                Table_CustQues.AddCell(Space);

                PdfPCell Header_Desc = new PdfPCell(new Phrase("Description", TblFontBold));
                Header_Desc.HorizontalAlignment = Element.ALIGN_LEFT;
                Header_Desc.Colspan = 4;
                //Header_Desc.Border = iTextSharp.text.Rectangle.BOX;
                Header_Desc.FixedHeight = 15f;
                Header_Desc.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                Header_Desc.BorderColor = BaseColor.WHITE;
                Table_CustQues.AddCell(Header_Desc);

                //PdfPCell Header_Enabled = new PdfPCell(new Phrase("Enabled", fb));
                //Header_Enabled.HorizontalAlignment = Element.ALIGN_CENTER;
                ////Header_Enabled.Colspan = 4;
                //Header_Enabled.Border = iTextSharp.text.Rectangle.BOX;
                //Table_CustQues.AddCell(Header_Enabled);

                PdfPCell Header_Required = new PdfPCell(new Phrase("Required", TblFontBold));
                Header_Required.HorizontalAlignment = Element.ALIGN_CENTER;
                //Header_Required.Colspan = 4;
                //Header_Required.Border = iTextSharp.text.Rectangle.BOX;
                Header_Required.FixedHeight = 15f;
                Header_Required.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                Header_Required.BorderColor = BaseColor.WHITE;
                Table_CustQues.AddCell(Header_Required);

                PdfPCell Header_Shared = new PdfPCell(new Phrase("Shared", TblFontBold));
                Header_Shared.HorizontalAlignment = Element.ALIGN_CENTER;
                //Header_Shared.Colspan = 4;
                //Header_Shared.Border = iTextSharp.text.Rectangle.BOX;
                Header_Shared.FixedHeight = 15f;
                Header_Shared.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                Header_Shared.BorderColor = BaseColor.WHITE;
                Table_CustQues.AddCell(Header_Shared);

                PdfPCell Header_Access = new PdfPCell(new Phrase("Member Access", TblFontBold));
                Header_Access.HorizontalAlignment = Element.ALIGN_CENTER;
                //Header_Shared.Colspan = 4;
                //Header_Access.Border = iTextSharp.text.Rectangle.BOX;
                Header_Access.FixedHeight = 15f;
                Header_Access.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                Header_Access.BorderColor = BaseColor.WHITE;
                Table_CustQues.AddCell(Header_Access);

                PdfPCell Header_Response = new PdfPCell(new Phrase("Response Type", TblFontBold));
                Header_Response.HorizontalAlignment = Element.ALIGN_CENTER;
                //Header_Shared.Colspan = 4;
                //Header_Response.Border = iTextSharp.text.Rectangle.BOX;
                Header_Response.FixedHeight = 15f;
                Header_Response.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#0b4775"));
                Header_Response.BorderColor = BaseColor.WHITE;
                Table_CustQues.AddCell(Header_Response);


                foreach (DataRow dr in dtCustom.Rows)
                {
                    if (ScrCode.Trim() == dr["FLDH_SCR_CODE"].ToString().Trim())
                    {
                        PdfPCell QuesDesc = new PdfPCell(new Phrase(dr["CUST_DESC"].ToString().Trim(), TableFont));
                        QuesDesc.HorizontalAlignment = Element.ALIGN_LEFT;
                        QuesDesc.Colspan = 4;
                        //QuesDesc.Border = iTextSharp.text.Rectangle.BOX;
                        QuesDesc.FixedHeight = 15f;
                        QuesDesc.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                        QuesDesc.BorderColor = BaseColor.WHITE;
                        Table_CustQues.AddCell(QuesDesc);

                        //if (dr["FLDH_ENABLED"].ToString().Trim() == "Y")
                        //{
                        //    PdfPCell Enabled = new PdfPCell(_image_Checked);
                        //    Enabled.HorizontalAlignment = Element.ALIGN_CENTER;
                        //    Enabled.VerticalAlignment = Element.ALIGN_MIDDLE;
                        //    Enabled.Border = iTextSharp.text.Rectangle.BOX;
                        //    Table_CustQues.AddCell(Enabled);
                        //}
                        //else
                        //{
                        //    PdfPCell Enabled = new PdfPCell(_image_UnChecked);
                        //    Enabled.HorizontalAlignment = Element.ALIGN_CENTER;
                        //    Enabled.VerticalAlignment = Element.ALIGN_MIDDLE;
                        //    Enabled.Border = iTextSharp.text.Rectangle.BOX;
                        //    Table_CustQues.AddCell(Enabled);
                        //}

                        if (dr["FLDH_REQUIRED"].ToString().Trim() == "Y")
                        {
                            string _imgicon = "P";
                            PdfPCell Required = new PdfPCell(new Phrase(_imgicon, FontWingdings));
                            Required.HorizontalAlignment = Element.ALIGN_CENTER;
                            Required.VerticalAlignment = Element.ALIGN_MIDDLE;
                            //Required.Border = iTextSharp.text.Rectangle.BOX;
                            Required.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                            Required.BorderColor = BaseColor.WHITE;
                            Required.FixedHeight = 15f;
                            Table_CustQues.AddCell(Required);
                        }
                        else
                        {
                            string _imgicon = "";
                            PdfPCell Required = new PdfPCell(new Phrase(_imgicon, FontWingdings));
                            Required.HorizontalAlignment = Element.ALIGN_CENTER;
                            Required.VerticalAlignment = Element.ALIGN_MIDDLE;
                            //Required.Border = iTextSharp.text.Rectangle.BOX;
                            Required.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                            Required.BorderColor = BaseColor.WHITE;
                            Required.FixedHeight = 15f;
                            Table_CustQues.AddCell(Required);
                        }

                        if (dr["FLDH_SHARED"].ToString().Trim() == "Y")
                        {
                            string _imgicon = "P";
                            PdfPCell Shared = new PdfPCell(new Phrase(_imgicon, FontWingdings));
                            Shared.HorizontalAlignment = Element.ALIGN_CENTER;
                            Shared.VerticalAlignment = Element.ALIGN_MIDDLE;
                            //Shared.Border = iTextSharp.text.Rectangle.BOX;
                            Shared.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                            Shared.BorderColor = BaseColor.WHITE;
                            Shared.FixedHeight = 15f;
                            Table_CustQues.AddCell(Shared);
                        }
                        else
                        {
                            string _imgicon = "";
                            PdfPCell Shared = new PdfPCell(new Phrase(_imgicon, FontWingdings));
                            Shared.HorizontalAlignment = Element.ALIGN_CENTER;
                            Shared.VerticalAlignment = Element.ALIGN_MIDDLE;
                            //Shared.Border = iTextSharp.text.Rectangle.BOX;
                            Shared.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                            Shared.BorderColor = BaseColor.WHITE;
                            Shared.FixedHeight = 15f;
                            Table_CustQues.AddCell(Shared);
                        }

                        Access_Member(dr["CUST_MEM_ACCESS"].ToString().Trim());
                        PdfPCell MemAccess = new PdfPCell(new Phrase(Member_Access.Trim(), TableFont));
                        MemAccess.HorizontalAlignment = Element.ALIGN_LEFT;
                        //MemAccess.Border = iTextSharp.text.Rectangle.BOX;
                        MemAccess.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                        MemAccess.BorderColor = BaseColor.WHITE;
                        MemAccess.FixedHeight = 15f;
                        Table_CustQues.AddCell(MemAccess);

                        ResponseType(dr["CUST_RESP_TYPE"].ToString().Trim());
                        PdfPCell Response = new PdfPCell(new Phrase(Resp_Desc.Trim(), TableFont));
                        Response.HorizontalAlignment = Element.ALIGN_LEFT;
                        //Response.Border = iTextSharp.text.Rectangle.BOX;
                        Response.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#dceaf4"));
                        Response.BorderColor = BaseColor.WHITE;
                        Response.FixedHeight = 15f;
                        Table_CustQues.AddCell(Response);

                        if (dr["CUST_RESP_TYPE"].ToString().Trim() == "D")
                        {
                            Search_Custresp.ResoCode = dr["FLDH_CODE"].ToString().Trim();
                            custresplist = _model.FieldControls.Browse_CUSTRESP(Search_Custresp, "Browse");
                            if (custresplist.Count > 0)
                            {
                                foreach (CustRespEntity Entity in custresplist)
                                {
                                    PdfPCell sPACE_rESP = new PdfPCell(new Phrase(" ", TableFont));
                                    sPACE_rESP.HorizontalAlignment = Element.ALIGN_LEFT;
                                    //Header_Desc.Colspan = 4;
                                    //sPACE_rESP.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
                                    sPACE_rESP.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                    sPACE_rESP.BorderColor = BaseColor.WHITE;
                                    sPACE_rESP.FixedHeight = 15f;
                                    Table_CustQues.AddCell(sPACE_rESP);

                                    PdfPCell Resp_Cd = new PdfPCell(new Phrase(Entity.DescCode, TableFont));
                                    Resp_Cd.HorizontalAlignment = Element.ALIGN_LEFT;
                                    //Header_Enabled.Colspan = 4;
                                    //Resp_Cd.Border = iTextSharp.text.Rectangle.NO_BORDER;
                                    Resp_Cd.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                    Resp_Cd.BorderColor = BaseColor.WHITE;
                                    Resp_Cd.FixedHeight = 15f;
                                    Table_CustQues.AddCell(Resp_Cd);

                                    PdfPCell Resp_DEsc = new PdfPCell(new Phrase(Entity.RespDesc, TableFont));
                                    Resp_DEsc.HorizontalAlignment = Element.ALIGN_LEFT;
                                    Resp_DEsc.Colspan = 6;
                                    //Resp_DEsc.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
                                    Resp_DEsc.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                                    Resp_DEsc.BorderColor = BaseColor.WHITE;
                                    Resp_DEsc.FixedHeight = 15f;
                                    Table_CustQues.AddCell(Resp_DEsc);
                                }

                            }

                        }
                    }
                }

                PdfPCell Space_resp1 = new PdfPCell(new Phrase(" ", TableFont));
                Space_resp1.HorizontalAlignment = Element.ALIGN_CENTER;
                Space_resp1.Colspan = 8;
                //Space_resp1.Border = iTextSharp.text.Rectangle.TOP_BORDER;
                Space_resp1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#f2f9fd"));
                Space_resp1.BorderColor = BaseColor.WHITE;
                Space_resp1.FixedHeight = 15f;
                Table_CustQues.AddCell(Space_resp1);

                document.Add(Table_CustQues);
            }
            else
            {
                if (((Captain.Common.Utilities.ListItem)CmbAgyTab.SelectedItem).Value.ToString().Trim() != "**")
                {
                    PdfContentByte cb = writer.DirectContent;
                    cb.BeginText();
                    cb.SetFontAndSize(bf_Calibri, 15);
                    cb.SetColorFill(BaseColor.RED);
                    cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "Custom Questions are not defined for this Hierarchy", 300, 450, 0);
                    cb.EndText();
                }
            }
        }

        //End of Report Printing Ligic For All Formats
        string Resp_Desc = string.Empty;
        private string ResponseType(string Resp_Type)
        {
            if (Resp_Type == "D")
                Resp_Desc = "DropDown";
            else if (Resp_Type == "N")
                Resp_Desc = "Numeric";
            else if (Resp_Type == "T")
                Resp_Desc = "Date";
            else if (Resp_Type == "X")
                Resp_Desc = "Text";
            else if (Resp_Type == "C")
                Resp_Desc = "Check Box";

            return Resp_Desc;
        }
        string Member_Access = string.Empty;
        public string Access_Member(string Mem_access)
        {
            if (Mem_access == "A")
                Member_Access = "Applicant";
            else if (Mem_access == "H")
                Member_Access = "Houshold";
            else if (Mem_access == "*")
                Member_Access = "All Members";
            else if (Mem_access == "1")
                Member_Access = "Any One Member";

            return Member_Access;
        }

        string Screen_Name = string.Empty;
        public string Name_Screen(string screen)
        {
            if (screen == "CASE2001")
                Screen_Name = "Client Intake";
            else if (screen == "CASINCOM")
                Screen_Name = "Income Entry";
            else if (screen == "CASE2003")
                Screen_Name = "Income Verification";
            else if (screen == "CASE0061")
                Screen_Name = "Contact Posting";
            else if (screen == "CASE0062")
                Screen_Name = "Crititcal Activity Posting";
            else if (screen == "CASE0063")
                Screen_Name = "Milestone Posting";
            return Screen_Name;
        }

        // Begining of Header for All Report Formats

        private void Print_Agytab_List_Header()
        {
            //PrintText = "Table#";
            //PrintHeaderRec(PrintText, 55);
            //PrintText = "Code";
            //PrintHeaderRec(PrintText, 35);
            //PrintText = "Description";
            //PrintHeaderRec(PrintText, 200);
            //PrintText = "Cell1";
            //PrintHeaderRec(PrintText, 60);
            //PrintText = "Cell2";
            //PrintHeaderRec(PrintText, 55);
            //PrintText = "Default";
            //PrintHeaderRec(PrintText, 65);
            //PrintText = "Inactive";
            //PrintHeaderRec(PrintText, 30);
        }

        private void Print_Agytab_List_Header(bool Format)
        {
            //X_Pos = 50;    //690

            X_Pos = 50;
            if (Format)
            {  //Casemanagement Header
                PrintText = "SERVICE#";
                PrintHeaderRec(PrintText, 150);
                PrintText = "D E S C R I P T I O N";
                PrintHeaderRec(PrintText, 200);

            }
            else
            {
                Y_Pos = 550;
                //Energy Assistance Header
                PrintText = "SERVICE#";
                PrintHeaderRec(PrintText, 70);
                PrintText = "FUND";
                PrintHeaderRec(PrintText, 50);
                PrintText = "HIERARCHY";
                PrintHeaderRec(PrintText, 100);
                PrintText = "DESCRIPTION";
                PrintHeaderRec(PrintText, 150);
                PrintText = "APPLICN";
                PrintHeaderRec(PrintText, 60);
                PrintText = "ZERO";
                PrintHeaderRec(PrintText, 40);
                PrintText = "VENDOR";
                PrintHeaderRec(PrintText, 70);
                PrintText = "AUTHORIZATIONS";
                PrintHeaderRec(PrintText, 120);
                PrintText = "MI";
                PrintHeaderRec(PrintText, 30);
                PrintText = "OOP";
                PrintHeaderRec(PrintText, 30);
                PrintText = "ACTIVE";
                PrintHeaderRec(PrintText, 30);
            }
        }

        private void CaseDemoAssociation()
        {
            Y_Pos = 550; X_Pos = 60;
            PrintText = "CSBG Category";
            PrintHeaderRec(PrintText, 150);
            PrintText = "Description Codes Used";
            PrintHeaderRec(PrintText, 250);
            PrintText = "Age From";
            PrintHeaderRec(PrintText, 70);
            PrintText = "Age To";
            PrintHeaderRec(PrintText, 90);
            PrintText = "Table#";
            PrintHeaderRec(PrintText, 70);
            PrintText = "Code";
            PrintHeaderRec(PrintText, 60);
            PrintText = "Desc";
            PrintHeaderRec(PrintText, 60);
        }

        private void PerformanceMeasureHeader()
        {
            Y_Pos = 550; X_Pos = 60;
            PrintText = "F Date";
            PrintHeaderRec(PrintText, 60);
            PrintText = "T Date";
            PrintHeaderRec(PrintText, 60);
            PrintText = "Grp Code";
            PrintHeaderRec(PrintText, 60);
            PrintText = "Tab Code";
            PrintHeaderRec(PrintText, 100);
            PrintText = "Description";
            PrintHeaderRec(PrintText, 350);
            PrintText = "Ind";
            PrintHeaderRec(PrintText, 35);
            PrintText = "Achieve";
            PrintHeaderRec(PrintText, 60);
            PrintText = "CC";
            PrintHeaderRec(PrintText, 30);
        }
        private void PerformMeasuresAssc()
        {
            Y_Pos = 550; X_Pos = 60;
            PrintText = "F Date";
            PrintHeaderRec(PrintText, 100);
            PrintText = "T Date";
            PrintHeaderRec(PrintText, 100);
            PrintText = "Grp Code";
            PrintHeaderRec(PrintText, 30);
            PrintText = "Tab Code";
            PrintHeaderRec(PrintText, 90);
            PrintText = "Description";
            PrintHeaderRec(PrintText, 250);
            PrintText = "Ind";
            PrintHeaderRec(PrintText, 40);
            PrintText = "Achieve";
            PrintHeaderRec(PrintText, 40);
            PrintText = "CC";
            PrintHeaderRec(PrintText, 40);
        }

        private void ZipCodeHeader()
        {
            X_Pos = 50;
            PrintText = "ZIPCode";
            PrintHeaderRec(PrintText, 90);
            //PrintText = "Plus";
            //PrintHeaderRec(PrintText, 50);
            PrintText = "City";
            PrintHeaderRec(PrintText, 130);
            PrintText = "State";
            PrintHeaderRec(PrintText, 50);
            PrintText = "IntakeCd";
            PrintHeaderRec(PrintText, 70);
            //PrintText = "City Code";
            //PrintHeaderRec(PrintText, 90);
            //PrintText = "Print Opt";
            //PrintHeaderRec(PrintText, 80);
            PrintText = "Month";
            PrintHeaderRec(PrintText, 50);
            PrintText = "Date";
            PrintHeaderRec(PrintText, 75);
            PrintText = "Year";
            PrintHeaderRec(PrintText, 40);
        }

        // End of Header for All Report Formats

        private void CheckBottomBorderReachedLandScape(Document document, PdfWriter writer)
        {

            //Font _font = new Font(bfTimes, 45, 1, BaseColor.BLACK);
            Image _image = iTextSharp.text.Image.GetInstance(Application.MapPath("~\\Resources\\images\\Capsystems_WaterMark.bmp"));
            _image.SetAbsolutePosition(200, 160);
            ////_image.SetAbsolutePosition(140, 260);

            _image.RotationDegrees = 30;
            _image.Rotate();
            PdfGState _state = new PdfGState()
            {
                FillOpacity = 0.2F,
                StrokeOpacity = 0.2F
            };
            //cb.EndText();


            if (Y_Pos <= 20)
            {
                //cb.BeginText();

                cb.EndText();

                //cb = writer.DirectContentUnder;
                //cb.SaveState();
                //cb.SetGState(_state);                               //WaterMark*******
                //cb.AddImage(_image);
                //cb.RestoreState();

                cb.BeginText();
                Y_Pos = 07;
                X_Pos = 20;
                cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES).BaseFont, 12);
                //cb.SetFontAndSize(bfTimes, 10);
                //       cb.SetColorFill(BaseColor.BLACK);
                cb.SetCMYKColorFill(0, 0, 0, 255);
                PrintRec(DateTime.Now.ToLocalTime().ToString(), 130);
                Y_Pos = 07;
                X_Pos = 800;
                PrintRec("Page:", 28);
                PrintRec(pageNumber.ToString(), 15);

                cb.EndText();

                //cb = writer.DirectContentUnder;
                //cb.SaveState();
                //cb.SetGState(_state);                               //WaterMark*******
                //cb.AddImage(_image);
                //cb.RestoreState();

                //cb.EndText();
                document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                document.NewPage();
                pageNumber = writer.PageNumber - 1;
                cb = writer.DirectContentUnder;
                cb.SaveState();
                cb.SetGState(_state);                               //WaterMark*******
                cb.AddImage(_image);
                cb.RestoreState();

                cb.BeginText();
                cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES).BaseFont, 12);
                //cb.SetFontAndSize(bfTimes, 10);
                cb.SetColorFill(BaseColor.LIGHT_GRAY);
                printHText();


                PrintHeader();

                //            cb.SetColorFill(BaseColor.BLACK);

                X_Pos = 50;
                Y_Pos -= 5;
                //CheckBottomBorderReached(document, writer);
                cb.EndText();
                Print_Line();
                Y_Pos = 520;
                X_Pos = 60;
                //cb.SetFontAndSize(bfTimes, 10);
                cb.BeginText();
                //X_Pos = 80;

            }
        }

        //Report name repeated on Each Page
        private void printHText()
        {
            cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES).BaseFont, 12);
            cb.SetColorFill(BaseColor.GRAY);
            cb.ShowTextAligned(Element.ALIGN_MIDDLE, "ADMNB001", 520, 820, 0);
            //switch (RepType)
            //{
            //    case "01": cb.ShowTextAligned(Element.ALIGN_MIDDLE, "ADMNB001", 250, 820, 0); break;
            //    case "02": break;
            //    case "03": cb.ShowTextAligned(Element.ALIGN_MIDDLE, "Case Demo Association", 400, 580, 0); break;
            //    case "04": cb.ShowTextAligned(Element.ALIGN_MIDDLE, "Performance Measures Associations ", 400, 580, 0); break;
            //    case "05": break;
            //    case "06": break;
            //    case "07": Print_Services_List_HText(RblCaseMgmt.Checked); break;
            //    case "08": cb.ShowTextAligned(Element.ALIGN_MIDDLE, "ZipCode_Report ", 250, 820, 0); break; 
            //}

        }


        private string GetMonth(String month)
        {
            string month_name = null;
            switch (month)
            {
                case "01":
                case "1": month_name = "January"; break;
                case "02":
                case "2": month_name = "February"; break;
                case "03":
                case "3": month_name = "March"; break;
                case "04":
                case "4": month_name = "April"; break;
                case "05":
                case "5": month_name = "May"; break;
                case "06":
                case "6": month_name = "June"; break;
                case "07":
                case "7": month_name = "July"; break;
                case "08":
                case "8": month_name = "August"; break;
                case "09":
                case "9": month_name = "September"; break;
                case "10": month_name = "October"; break;
                case "11": month_name = "November"; break;
                case "12": month_name = "December"; break;
            }
            return month_name;
        }

        private string GetYear(string year)
        {
            String Year_name = null;
            switch (year)
            {
                case "01": Year_name = "This Program Year"; break;
                case "02": Year_name = "Next Year"; break;
            }
            return Year_name;
        }


        //void setUploadImage(string FileName)
        //{
        //    if (FileName != "")
        //    {

        //        _strLogoName = FileName.ToString();
        //        _strImageFolderPath = _model.lookupDataAccess.GetReportPath() + "\\AgencyLogos\\";

        //        FileInfo info = new FileInfo(_strImageFolderPath + _strLogoName);

        //        if (info.Exists)
        //        {
        //            byte[] buff = System.IO.File.ReadAllBytes(_strImageFolderPath + _strLogoName);
        //            System.IO.MemoryStream ms = new System.IO.MemoryStream(buff);
        //            _ostreamLogo = ms;
        //            pictureBox.BackColor = Color.White;
        //            pictureBox.Image = GetImageFromStream(ms);
        //        }
        //        else
        //            pictureBox.Image = null;
        //    }
        //}
        private void PrintHeaderPage(Document document, PdfWriter writer)
        {
            // BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/calibrib.ttf", BaseFont.WINANSI, BaseFont.EMBEDDED);
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
            Headertable.TotalWidth = 510f;  //550f;
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
            content.SetColorStroke(BaseColor.BLACK);
            content.Rectangle(rectangle.Left, rectangle.Bottom, rectangle.Width, rectangle.Height);
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
                cellLogo.Padding = 5;
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

            PdfPCell cell_Content_Title1 = new PdfPCell(new Phrase("  " + LblRepType.Text.Trim() + "", TableFont));
            cell_Content_Title1.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_Content_Title1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            cell_Content_Title1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            cell_Content_Title1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
            cell_Content_Title1.PaddingBottom = 5;
            Headertable.AddCell(cell_Content_Title1);

            PdfPCell cell_Content_Desc1 = new PdfPCell(new Phrase(CmbRepType.Text.Trim(), TableFont));
            cell_Content_Desc1.HorizontalAlignment = Element.ALIGN_LEFT;
            cell_Content_Desc1.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
            cell_Content_Desc1.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
            cell_Content_Desc1.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
            cell_Content_Desc1.PaddingBottom = 5;
            Headertable.AddCell(cell_Content_Desc1);

            //PdfPCell R2 = new PdfPCell(new Phrase("Sequence :" + ((Captain.Common.Utilities.ListItem)cmbSeq.SelectedItem).Text.ToString(), TableFont));
            //R2.HorizontalAlignment = Element.ALIGN_LEFT;
            //R2.Colspan = 2;
            //R2.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //Headertable.AddCell(R2);

            //PdfPCell R3 = new PdfPCell(new Phrase("Enroll Status :" + ((Captain.Common.Utilities.ListItem)cmbEntlStat.SelectedItem).Text.ToString(), TableFont));
            //R3.HorizontalAlignment = Element.ALIGN_LEFT;
            //R3.Colspan = 2;
            //R3.Border = iTextSharp.text.Rectangle.NO_BORDER;
            //Headertable.AddCell(R3);

            string TmpStr = null;
            switch (RepType)
            {
                case "01":
                    TmpStr = "Code";
                    if (RblEmrServ.Checked)
                        TmpStr = "Description";

                    PdfPCell cell_Content_Title2 = new PdfPCell(new Phrase("  " + "Sort By", TableFont));
                    cell_Content_Title2.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell_Content_Title2.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    cell_Content_Title2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    cell_Content_Title2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                    cell_Content_Title2.PaddingBottom = 5;
                    Headertable.AddCell(cell_Content_Title2);

                    PdfPCell cell_Content_Desc2 = new PdfPCell(new Phrase(TmpStr, TableFont));
                    cell_Content_Desc2.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell_Content_Desc2.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    cell_Content_Desc2.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    cell_Content_Desc2.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                    cell_Content_Desc2.PaddingBottom = 5;
                    Headertable.AddCell(cell_Content_Desc2);


                    PdfPCell cell_Content_Title3 = new PdfPCell(new Phrase("  " + "Agency Table", TableFont));
                    cell_Content_Title3.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell_Content_Title3.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    cell_Content_Title3.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    cell_Content_Title3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                    cell_Content_Title3.PaddingBottom = 5;
                    Headertable.AddCell(cell_Content_Title3);

                    PdfPCell cell_Content_Desc3 = new PdfPCell(new Phrase(CmbAgyTab.Text.Trim(), TableFont));
                    cell_Content_Desc3.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell_Content_Desc3.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    cell_Content_Desc3.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    cell_Content_Desc3.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                    cell_Content_Desc3.PaddingBottom = 5;
                    Headertable.AddCell(cell_Content_Desc3);


                    PdfPCell cell_Content_Title4 = new PdfPCell(new Phrase("  " + "Application", TableFont));
                    cell_Content_Title4.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell_Content_Title4.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    cell_Content_Title4.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    cell_Content_Title4.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                    cell_Content_Title4.PaddingBottom = 5;
                    Headertable.AddCell(cell_Content_Title4);

                    PdfPCell cell_Content_Desc4 = new PdfPCell(new Phrase(cmbModule.Text.Trim(), TableFont));
                    cell_Content_Desc4.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell_Content_Desc4.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    cell_Content_Desc4.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    cell_Content_Desc4.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                    cell_Content_Desc4.PaddingBottom = 5;
                    Headertable.AddCell(cell_Content_Desc4);

                    break;
                //case "02": cb.ShowTextAligned(100, "Report Type :" + CmbRepType.Text, 120, 610, 0); break;
                //case "03": cb.ShowTextAligned(100, "Report Type :" + CmbRepType.Text, 120, 610, 0); break;
                case "04":
                    PdfPCell cell_Content_Title5 = new PdfPCell(new Phrase("  " + "Date Range", TableFont));
                    cell_Content_Title5.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell_Content_Title5.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    cell_Content_Title5.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    cell_Content_Title5.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                    cell_Content_Title5.PaddingBottom = 5;
                    Headertable.AddCell(cell_Content_Title5);

                    PdfPCell cell_Content_Desc5 = new PdfPCell(new Phrase(CmbAgyTab.Text.Trim(), TableFont));
                    cell_Content_Desc5.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell_Content_Desc5.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    cell_Content_Desc5.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    cell_Content_Desc5.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                    cell_Content_Desc5.PaddingBottom = 5;
                    Headertable.AddCell(cell_Content_Desc5);

                    break;
                //cb.ShowTextAligned(100, "Date Range : " + CmbAgyTab.Text, 120, 565, 0); 
                case "05":
                    TmpStr = "Code";
                    if (RblEmrServ.Checked)
                        TmpStr = "Description";

                    PdfPCell cell_Content_Title6 = new PdfPCell(new Phrase("  " + "Sort By", TableFont));
                    cell_Content_Title6.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell_Content_Title6.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    cell_Content_Title6.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    cell_Content_Title6.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                    cell_Content_Title6.PaddingBottom = 5;
                    Headertable.AddCell(cell_Content_Title6);

                    PdfPCell cell_Content_Desc6 = new PdfPCell(new Phrase(TmpStr, TableFont));
                    cell_Content_Desc6.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell_Content_Desc6.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    cell_Content_Desc6.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    cell_Content_Desc6.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                    cell_Content_Desc6.PaddingBottom = 5;
                    Headertable.AddCell(cell_Content_Desc6);

                    break;
                //cb.ShowTextAligned(100, "Sort By :" + TmpStr, 120, 565, 0); 
                case "06":
                    TmpStr = "Code";
                    if (RblEmrServ.Checked)
                        TmpStr = "Description";

                    PdfPCell cell_Content_Title7 = new PdfPCell(new Phrase("  " + "Sort By", TableFont));
                    cell_Content_Title7.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell_Content_Title7.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    cell_Content_Title7.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    cell_Content_Title7.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                    cell_Content_Title7.PaddingBottom = 5;
                    Headertable.AddCell(cell_Content_Title7);

                    PdfPCell cell_Content_Desc7 = new PdfPCell(new Phrase(TmpStr, TableFont));
                    cell_Content_Desc7.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell_Content_Desc7.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    cell_Content_Desc7.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    cell_Content_Desc7.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                    cell_Content_Desc7.PaddingBottom = 5;
                    Headertable.AddCell(cell_Content_Desc7);

                    break;
                case "07":
                    TmpStr = "Case Managemnt";
                    if (RblEmrServ.Checked)
                        TmpStr = "Energy Services";

                    PdfPCell cell_Content_Title8 = new PdfPCell(new Phrase("  " + "Format", TableFont));
                    cell_Content_Title8.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell_Content_Title8.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    cell_Content_Title8.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    cell_Content_Title8.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                    cell_Content_Title8.PaddingBottom = 5;
                    Headertable.AddCell(cell_Content_Title8);

                    PdfPCell cell_Content_Desc8 = new PdfPCell(new Phrase(TmpStr, TableFont));
                    cell_Content_Desc8.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell_Content_Desc8.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    cell_Content_Desc8.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    cell_Content_Desc8.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                    cell_Content_Desc8.PaddingBottom = 5;
                    Headertable.AddCell(cell_Content_Desc8);

                    break;
                //cb.ShowTextAligned(100, "Format :" + TmpStr, 120, 565, 0); break;
                case "08":
                    TmpStr = "ZIP Code";
                    if (RblEmrServ.Checked)
                        TmpStr = "City";
                    else if (rbTown.Checked) TmpStr = "Towship"; else if (rbCounty.Checked) TmpStr = "County";

                    PdfPCell cell_Content_Title9 = new PdfPCell(new Phrase("  " + "Sort By", TableFont));
                    cell_Content_Title9.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell_Content_Title9.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    cell_Content_Title9.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    cell_Content_Title9.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                    cell_Content_Title9.PaddingBottom = 5;
                    Headertable.AddCell(cell_Content_Title9);

                    PdfPCell cell_Content_Desc9 = new PdfPCell(new Phrase(TmpStr, TableFont));
                    cell_Content_Desc9.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell_Content_Desc9.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    cell_Content_Desc9.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    cell_Content_Desc9.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                    cell_Content_Desc9.PaddingBottom = 5;
                    Headertable.AddCell(cell_Content_Desc9);

                    break;
                //cb.ShowTextAligned(100, "Sort By :" + TmpStr, 120, 565, 0); break;
                case "09":
                    TmpStr = "Hierarchy Summary"; if (RblEmrServ.Checked)
                        TmpStr = "Program Details";

                    PdfPCell cell_Content_Title10 = new PdfPCell(new Phrase("  " + "Sort By", TableFont));
                    cell_Content_Title10.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell_Content_Title10.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    cell_Content_Title10.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    cell_Content_Title10.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                    cell_Content_Title10.PaddingBottom = 5;
                    Headertable.AddCell(cell_Content_Title10);

                    PdfPCell cell_Content_Desc10 = new PdfPCell(new Phrase(TmpStr, TableFont));
                    cell_Content_Desc10.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell_Content_Desc10.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    cell_Content_Desc10.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    cell_Content_Desc10.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                    cell_Content_Desc10.PaddingBottom = 5;
                    Headertable.AddCell(cell_Content_Desc10);

                    break;
                //cb.ShowTextAligned(100, "Sort By :" + TmpStr, 120, 565, 0); break;
                case "10":
                    TmpStr = "Code"; if (RblEmrServ.Checked)
                        TmpStr = "Description";

                    PdfPCell cell_Content_Title11 = new PdfPCell(new Phrase("  " + "Sort By", TableFont));
                    cell_Content_Title11.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell_Content_Title11.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    cell_Content_Title11.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    cell_Content_Title11.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                    cell_Content_Title11.PaddingBottom = 5;
                    Headertable.AddCell(cell_Content_Title11);

                    PdfPCell cell_Content_Desc11 = new PdfPCell(new Phrase(TmpStr, TableFont));
                    cell_Content_Desc11.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell_Content_Desc11.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    cell_Content_Desc11.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    cell_Content_Desc11.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                    cell_Content_Desc11.PaddingBottom = 5;
                    Headertable.AddCell(cell_Content_Desc11);

                    PdfPCell cell_Content_Title12 = new PdfPCell(new Phrase("  " + "Screen Name", TableFont));
                    cell_Content_Title12.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell_Content_Title12.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    cell_Content_Title12.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    cell_Content_Title12.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                    cell_Content_Title12.PaddingBottom = 5;
                    Headertable.AddCell(cell_Content_Title12);

                    PdfPCell cell_Content_Desc12 = new PdfPCell(new Phrase(((Captain.Common.Utilities.ListItem)CmbAgyTab.SelectedItem).ID.ToString().Trim(), TableFont));
                    cell_Content_Desc12.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell_Content_Desc12.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    cell_Content_Desc12.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    cell_Content_Desc12.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                    cell_Content_Desc12.PaddingBottom = 5;
                    Headertable.AddCell(cell_Content_Desc12);

                    PdfPCell cell_Content_Title13 = new PdfPCell(new Phrase("  " + "Hierarchy ", TableFont));
                    cell_Content_Title13.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell_Content_Title13.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    cell_Content_Title13.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    cell_Content_Title13.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                    cell_Content_Title13.PaddingBottom = 5;
                    Headertable.AddCell(cell_Content_Title13);

                    PdfPCell cell_Content_Desc13 = new PdfPCell(new Phrase(txtHie.Text, TableFont));
                    cell_Content_Desc13.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell_Content_Desc13.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    cell_Content_Desc13.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    cell_Content_Desc13.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                    cell_Content_Desc13.PaddingBottom = 5;
                    Headertable.AddCell(cell_Content_Desc13);


                    string QuesType = string.Empty; string QuesDesc = string.Empty;
                    if (rbBoth.Checked == true) { QuesType = lblType.Text.Trim(); QuesDesc = rbBoth.Text.Trim(); }
                    else if (rbCustom.Checked == true) { QuesType = lblType.Text.Trim(); QuesDesc = rbCustom.Text.Trim(); }
                    else if (rbStandard.Checked == true) { QuesType = lblType.Text.Trim(); QuesDesc = rbStandard.Text.Trim(); }

                    if (lblType.Visible == true)
                    {
                        PdfPCell cell_Content_Title14 = new PdfPCell(new Phrase("  " + QuesType, TableFont));
                        cell_Content_Title14.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell_Content_Title14.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                        cell_Content_Title14.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                        cell_Content_Title14.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                        cell_Content_Title14.PaddingBottom = 5;
                        Headertable.AddCell(cell_Content_Title14);

                        PdfPCell cell_Content_Desc14 = new PdfPCell(new Phrase(QuesDesc, TableFont));
                        cell_Content_Desc14.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell_Content_Desc14.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                        cell_Content_Desc14.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                        cell_Content_Desc14.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                        cell_Content_Desc14.PaddingBottom = 5;
                        Headertable.AddCell(cell_Content_Desc14);
                    }

                    //cb.ShowTextAligned(100, "Sort By :" + TmpStr, 120, 565, 0);
                    //cb.ShowTextAligned(100, "Screen Name :" + ((Captain.Common.Utilities.ListItem)CmbAgyTab.SelectedItem).ID.ToString().Trim(), 120, 545, 0);
                    //cb.ShowTextAligned(100, "Hierarchy:" + txtHie.Text, 300, 565, 0);
                    break;

                case "11":
                    string strLabelType = "Type";
                    string strLabelName = "All";
                    if (rdoSeletedt.Checked)
                    {
                        strLabelType = "Date Range";
                        strLabelName = "From: " + dtselect.Text + "    " +"To: " + dtSelectToDate.Text;
                    }
                    PdfPCell cell_Content_Title17 = new PdfPCell(new Phrase("  " + strLabelType, TableFont));
                    cell_Content_Title17.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell_Content_Title17.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    cell_Content_Title17.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    cell_Content_Title17.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                    cell_Content_Title17.PaddingBottom = 5;
                    Headertable.AddCell(cell_Content_Title17);

                    PdfPCell cell_Content_Desc17 = new PdfPCell(new Phrase(strLabelName, TableFont));
                    cell_Content_Desc17.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell_Content_Desc17.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    cell_Content_Desc17.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    cell_Content_Desc17.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                    cell_Content_Desc17.PaddingBottom = 5;
                    Headertable.AddCell(cell_Content_Desc17);


                    PdfPCell cell_Content_Title18 = new PdfPCell(new Phrase("  " + "UserID", TableFont));
                    cell_Content_Title18.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell_Content_Title18.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    cell_Content_Title18.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    cell_Content_Title18.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                    cell_Content_Title18.PaddingBottom = 5;
                    Headertable.AddCell(cell_Content_Title18);

                    string strUsers = "All Users";
                    if (CmbUser.SelectedIndex > 0)
                    {
                        strUsers = CmbUser.Text.ToString();
                    }
                    PdfPCell cell_Content_Desc18 = new PdfPCell(new Phrase(strUsers, TableFont));
                    cell_Content_Desc18.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell_Content_Desc18.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    cell_Content_Desc18.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    cell_Content_Desc18.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                    cell_Content_Desc18.PaddingBottom = 5;
                    Headertable.AddCell(cell_Content_Desc18);

                    break;

                case "12":
                    string strLabelType19 = "Type";
                    string strLabelName19 = "All";
                    if (rdoRepDate.Checked)
                    {
                        strLabelType19 = "Selected Date";
                        strLabelName19 = dtRepDate.Text;
                    }
                    PdfPCell cell_Content_Title19 = new PdfPCell(new Phrase("  " + strLabelType19, TableFont));
                    cell_Content_Title19.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell_Content_Title19.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    cell_Content_Title19.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    cell_Content_Title19.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                    cell_Content_Title19.PaddingBottom = 5;
                    Headertable.AddCell(cell_Content_Title19);

                    PdfPCell cell_Content_Desc19 = new PdfPCell(new Phrase(strLabelName19, TableFont));
                    cell_Content_Desc19.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell_Content_Desc19.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    cell_Content_Desc19.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    cell_Content_Desc19.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                    cell_Content_Desc19.PaddingBottom = 5;
                    Headertable.AddCell(cell_Content_Desc19);


                    PdfPCell cell_Content_Title20 = new PdfPCell(new Phrase("  " + "UserID", TableFont));
                    cell_Content_Title20.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell_Content_Title20.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    cell_Content_Title20.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    cell_Content_Title20.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                    cell_Content_Title20.PaddingBottom = 5;
                    Headertable.AddCell(cell_Content_Title20);

                    string strUsers20 = "All Users";
                    if (cmbRepUserId.SelectedIndex > -1)
                    {
                        strUsers20 = cmbRepUserId.Text.ToString();
                    }
                    PdfPCell cell_Content_Desc20 = new PdfPCell(new Phrase(strUsers20, TableFont));
                    cell_Content_Desc20.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell_Content_Desc20.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    cell_Content_Desc20.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    cell_Content_Desc20.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                    cell_Content_Desc20.PaddingBottom = 5;
                    Headertable.AddCell(cell_Content_Desc20);

                    PdfPCell cell_Content_Title21 = new PdfPCell(new Phrase("  " + "Module", TableFont));
                    cell_Content_Title21.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell_Content_Title21.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    cell_Content_Title21.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    cell_Content_Title21.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                    cell_Content_Title21.PaddingBottom = 5;
                    Headertable.AddCell(cell_Content_Title21);

                    string strModule21 = "-";
                    if (cmbRepModule.SelectedIndex > 0)
                    {
                        strModule21 = cmbRepModule.Text.ToString();
                    }
                    PdfPCell cell_Content_Desc21 = new PdfPCell(new Phrase(strModule21, TableFont));
                    cell_Content_Desc21.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell_Content_Desc21.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    cell_Content_Desc21.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    cell_Content_Desc21.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                    cell_Content_Desc21.PaddingBottom = 5;
                    Headertable.AddCell(cell_Content_Desc21);

                    PdfPCell cell_Content_Title22 = new PdfPCell(new Phrase("  " + "Report Code", TableFont));
                    cell_Content_Title22.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell_Content_Title22.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    cell_Content_Title22.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    cell_Content_Title22.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                    cell_Content_Title22.PaddingBottom = 5;
                    Headertable.AddCell(cell_Content_Title22);

                    string strReportCode22 = "-";
                    if (cmbRepCode.SelectedIndex > -1)
                    {
                        strReportCode22 = cmbRepCode.Text.ToString();
                    }
                    PdfPCell cell_Content_Desc22 = new PdfPCell(new Phrase(strReportCode22, TableFont));
                    cell_Content_Desc22.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell_Content_Desc22.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    cell_Content_Desc22.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    cell_Content_Desc22.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                    cell_Content_Desc22.PaddingBottom = 5;
                    Headertable.AddCell(cell_Content_Desc22);

                    break;

                case "14":

                    PdfPCell cell_Content_Title23 = new PdfPCell(new Phrase("  " + "Date Range", TableFont));
                    cell_Content_Title23.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell_Content_Title23.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    cell_Content_Title23.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    cell_Content_Title23.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                    cell_Content_Title23.PaddingBottom = 5;
                    Headertable.AddCell(cell_Content_Title23);

                    PdfPCell cell_Content_Desc23 = new PdfPCell(new Phrase(CmbAgyTab.Text.ToString(), TableFont));
                    cell_Content_Desc23.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell_Content_Desc23.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    cell_Content_Desc23.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    cell_Content_Desc23.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                    cell_Content_Desc23.PaddingBottom = 5;
                    Headertable.AddCell(cell_Content_Desc23);
                    break;
                case "15":

                    PdfPCell cell_Content_Title24 = new PdfPCell(new Phrase("  " + "Date Range", TableFont));
                    cell_Content_Title24.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell_Content_Title24.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    cell_Content_Title24.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    cell_Content_Title24.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                    cell_Content_Title24.PaddingBottom = 5;
                    Headertable.AddCell(cell_Content_Title24);

                    PdfPCell cell_Content_Desc24 = new PdfPCell(new Phrase(CmbAgyTab.Text.ToString(), TableFont));
                    cell_Content_Desc24.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell_Content_Desc24.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    cell_Content_Desc24.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    cell_Content_Desc24.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                    cell_Content_Desc24.PaddingBottom = 5;
                    Headertable.AddCell(cell_Content_Desc24);
                    break;
                case "16":
                    TmpStr = "Programs"; if (RblEmrServ.Checked)
                        TmpStr = "Reports";
                    Headerwidths = new float[] { 15f, 88f };
                    Headertable.SetWidths(Headerwidths);
                    Headertable.HorizontalAlignment = Element.ALIGN_CENTER;

                    PdfPCell cell_Content_Title15 = new PdfPCell(new Phrase("  " + "Program/Report ", TableFont));
                    cell_Content_Title15.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell_Content_Title15.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    cell_Content_Title15.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    cell_Content_Title15.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                    cell_Content_Title15.PaddingBottom = 5;
                    Headertable.AddCell(cell_Content_Title15);

                    PdfPCell cell_Content_Desc15 = new PdfPCell(new Phrase(TmpStr, TableFont));
                    cell_Content_Desc15.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell_Content_Desc15.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    cell_Content_Desc15.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    cell_Content_Desc15.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                    cell_Content_Desc15.PaddingBottom = 5;
                    Headertable.AddCell(cell_Content_Desc15);


                    PdfPCell cell_Content_Title16 = new PdfPCell(new Phrase("  " + "Application ", TableFont));
                    cell_Content_Title16.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell_Content_Title16.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    cell_Content_Title16.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    cell_Content_Title16.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#F2F2F2"));
                    cell_Content_Title16.PaddingBottom = 5;
                    Headertable.AddCell(cell_Content_Title16);

                    PdfPCell cell_Content_Desc16 = new PdfPCell(new Phrase(CmbAgyTab.Text.Trim(), TableFont));
                    cell_Content_Desc16.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell_Content_Desc16.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;
                    cell_Content_Desc16.BorderColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF"));
                    cell_Content_Desc16.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#fbfbfb"));
                    cell_Content_Desc16.PaddingBottom = 5;
                    Headertable.AddCell(cell_Content_Desc16);

                    break;
            }
            document.Add(Headertable);

            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase("Generated By : ", fnttimesRoman_Italic), 33, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase(LookupDataAccess.GetMemberName(BaseForm.UserProfile.FirstName.Trim(), BaseForm.UserProfile.MI.Trim(), BaseForm.UserProfile.LastName.Trim(), "3"), fnttimesRoman_Italic), 90, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase("Generated On : ", fnttimesRoman_Italic), 410, 40, 0);
            ColumnText.ShowTextAligned(writer.DirectContent, Element.ALIGN_LEFT, new Phrase(DateTime.Now.ToString(), fnttimesRoman_Italic), 468, 40, 0);

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

        private void PrintRelatrdSelections()
        {
            string TmpStr = null;
            switch (RepType)
            {
                case "01":
                    TmpStr = "Code"; if (RblEmrServ.Checked)
                        TmpStr = "Description";
                    cb.ShowTextAligned(100, "Sort By :" + TmpStr, 120, 565, 0); break;
                //cb.ShowTextAligned(100, "Sequnce : " + CmbSeq.Text, 120, 545, 0); break;
                //case "02": cb.ShowTextAligned(100, "Report Type :" + CmbRepType.Text, 120, 610, 0); break;
                //case "03": cb.ShowTextAligned(100, "Report Type :" + CmbRepType.Text, 120, 610, 0); break;
                case "04": cb.ShowTextAligned(100, "Date Range : " + CmbAgyTab.Text, 120, 565, 0); break;
                case "05":
                    TmpStr = "Code"; if (RblEmrServ.Checked)
                        TmpStr = "Description";
                    cb.ShowTextAligned(100, "Sort By :" + TmpStr, 120, 565, 0); break;
                case "06":
                    TmpStr = "Code"; if (RblEmrServ.Checked)
                        TmpStr = "Description";
                    cb.ShowTextAligned(100, "Sort By :" + TmpStr, 120, 565, 0); break;
                case "07":
                    TmpStr = "Case Managemnt"; if (RblEmrServ.Checked)
                        TmpStr = "Energy Services";
                    cb.ShowTextAligned(100, "Format :" + TmpStr, 120, 565, 0); break;
                case "08":
                    TmpStr = "ZipCode"; if (RblEmrServ.Checked)
                        TmpStr = "City";
                    cb.ShowTextAligned(100, "Sort By :" + TmpStr, 120, 565, 0); break;
                case "09":
                    TmpStr = "Hierarchy Summary"; if (RblEmrServ.Checked)
                        TmpStr = "Program Details";
                    cb.ShowTextAligned(100, "Sort By :" + TmpStr, 120, 565, 0); break;
                case "10":
                    TmpStr = "Code"; if (RblEmrServ.Checked)
                        TmpStr = "Description";
                    cb.ShowTextAligned(100, "Sort By :" + TmpStr, 120, 565, 0);
                    cb.ShowTextAligned(100, "Screen Name :" + ((Captain.Common.Utilities.ListItem)CmbAgyTab.SelectedItem).ID.ToString().Trim(), 120, 545, 0);
                    cb.ShowTextAligned(100, "Hierarchy:" + txtHie.Text, 300, 565, 0); break;
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //  Help.ShowHelp(this, Context.Server.MapPath("~\\Resources\\HelpFiles\\Captain_Help.chm"), HelpNavigator.KeywordIndex, "Master Table Lists");
        }



        private void pictureBox3_Click(object sender, EventArgs e)
        {
            //HierarchieSelectionForm hierarchieSelectionForm = new HierarchieSelectionForm(BaseForm, "Master");
            //hierarchieSelectionForm.FormClosed += new Form.FormClosedEventHandler(OnHierarchieFormClosed);
            //hierarchieSelectionForm.ShowDialog();

            //HierarchieSelectionFormNew hierarchieSelectionForm = new HierarchieSelectionFormNew(BaseForm, string.Empty, "Master", string.Empty, "*", "Reports");
            //hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
            //hierarchieSelectionForm.StartPosition = FormStartPosition.CenterScreen;
            //hierarchieSelectionForm.ShowDialog();

            HierarchieSelection hierarchieSelectionForm = new HierarchieSelection(BaseForm, string.Empty, "Master", string.Empty, "*", "", BaseForm.UserID);
            hierarchieSelectionForm.FormClosed += new FormClosedEventHandler(OnHierarchieFormClosed);
            hierarchieSelectionForm.StartPosition = FormStartPosition.CenterScreen;
            hierarchieSelectionForm.ShowDialog();
        }

        string strPublicCode = "**-**-**";
        string HieCode = string.Empty;
        private void OnHierarchieFormClosed(object sender, FormClosedEventArgs e)
        {
            HierarchieSelection form = sender as HierarchieSelection;

            if (form.DialogResult == DialogResult.OK)
            {
                List<HierarchyEntity> selectedHierarchies = form.SelectedHierarchies;
                string hierarchy = string.Empty;
                foreach (HierarchyEntity row in selectedHierarchies)
                {
                    if (row.Agency == "")
                        row.Agency = "**";
                    if (row.Dept == "")
                        row.Dept = "**";
                    if (row.Prog == "")
                        row.Prog = "**";
                    hierarchy = row.Agency + row.Dept + row.Prog;
                    strPublicCode = row.Code;
                }
                if (RepType == "10")
                {
                    txtHie.Text = strPublicCode;
                    HieCode = hierarchy;
                }
            }
        }

        //private void ExcelAgencyTable(string FileName)
        //{

        //    string data = null;
        //    int i = 0;
        //    int j = 0;
        //    string PrivAgyType = null;
        //    string Agy_Code_Cell = null;
        //    string Agy_Desc_Cell = null;

        //    Excel.Application xlApp;
        //    Excel.Workbook xlWorkBook;
        //    Excel.Worksheet xlWorkSheet;
        //    Excel.Range chartRange;
        //    object misValue = System.Reflection.Missing.Value;

        //    xlApp = new Excel.Application();
        //    xlWorkBook = xlApp.Workbooks.Add(misValue);
        //    xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

        //    DataSet ds = DatabaseLayer.ADMNB001DB.ADMNB001_GetaAgyTabList(((RepListItem)CmbAgyTab.SelectedItem).Value.ToString());
        //    DataTable dt = ds.Tables[0];


        //    xlWorkSheet.Cells[2, 2] = "Table#";
        //    xlWorkSheet.Cells[2, 3] = "Code";
        //    xlWorkSheet.Cells[2, 4] = "Description";
        //    xlWorkSheet.Cells[2, 5] = "Cell1";
        //    xlWorkSheet.Cells[2, 6] = "Cell2";
        //    xlWorkSheet.Cells[2, 7] = "Default";
        //    xlWorkSheet.Cells[2, 8] = "Inactive";

        //    xlWorkSheet.Cells[2, 2].EntireColumn.ColumnWidth = "7";
        //    xlWorkSheet.Cells[2, 3].EntireColumn.ColumnWidth = "8";
        //    xlWorkSheet.Cells[2, 4].EntireColumn.ColumnWidth = "60";
        //    xlWorkSheet.Cells[2, 5].EntireColumn.ColumnWidth = "5";
        //    xlWorkSheet.Cells[2, 6].EntireColumn.ColumnWidth = "5";
        //    xlWorkSheet.Cells[2, 7].EntireColumn.ColumnWidth = "8";
        //    xlWorkSheet.Cells[2, 8].EntireColumn.ColumnWidth = "7";


        //    for (i = 0; i <= dt.Rows.Count - 1; i++)
        //    {
        //        //for (j = 0; j <= dt.Columns.Count - 1; j++)
        //        //{
        //        int number;
        //        if (dt.Rows[i]["AGY_CODE"].ToString() == "00000")
        //        {
        //            if (string.IsNullOrEmpty(dt.Rows[i]["AGY_ACTIVE"].ToString()) || string.IsNullOrEmpty(dt.Rows[i]["AGY_DEFAULT"].ToString()) ||
        //          (!(int.TryParse(dt.Rows[i]["AGY_ACTIVE"].ToString(), out number))) || (!(int.TryParse(dt.Rows[i]["AGY_DEFAULT"].ToString(), out number))))
        //            {
        //                //data = dt.Rows[i].ItemArray[j].ToString(); goto NextAgyType;
        //                PrivAgyType = dt.Rows[i]["AGY_TYPE"].ToString(); goto NextAgyType;
        //            }
        //        }
        //        if (dt.Rows[i]["AGY_TYPE"].ToString() != PrivAgyType)
        //        {
        //            data = dt.Rows[i]["AGY_TYPE"].ToString();
        //            xlWorkSheet.Cells[i + 3, 2] = data;
        //            data = dt.Rows[i]["AGY_DESC"].ToString();
        //            xlWorkSheet.Cells[i + 3, 4] = data;
        //            //xlWorkSheet.Cells[i + 3, 4].EntireColumn.ColumnWidth = "60";
        //            data = dt.Rows[i]["AGY_ACTIVE"].ToString();
        //            xlWorkSheet.Cells[i + 3, 8] = data;
        //            data = dt.Rows[i]["AGY_DEFAULT"].ToString();
        //            xlWorkSheet.Cells[i + 3, 7] = data;

        //            Agy_Code_Cell = "AGY_" + dt.Rows[i]["AGY_ACTIVE"].ToString();
        //            Agy_Desc_Cell = "AGY_" + dt.Rows[i]["AGY_DEFAULT"].ToString();

        //            PrivAgyType = dt.Rows[i]["AGY_TYPE"].ToString();

        //        }

        //        if (dt.Rows[i]["AGY_TYPE"].ToString() == PrivAgyType && dt.Rows[i]["AGY_CODE"].ToString() != "00000")
        //        {
        //            data = dt.Rows[i][Agy_Code_Cell].ToString();

        //            xlWorkSheet.Cells[i + 3, 3] = data;
        //            data = dt.Rows[i][Agy_Desc_Cell].ToString();
        //            xlWorkSheet.Cells[i + 3, 4] = data;



        //            if (dt.Rows[i]["AGY_DEFAULT"].ToString() == "Y")
        //            {
        //                data = "Y";
        //                xlWorkSheet.Cells[i + 3, 7] = data;
        //            }
        //            else
        //            {
        //                data = " ";
        //                xlWorkSheet.Cells[i + 3, 7] = data;
        //            }

        //            if (dt.Rows[i]["AGY_ACTIVE"].ToString() != "Y")
        //            {
        //                data = "Y";
        //                xlWorkSheet.Cells[i + 3, 8] = data;
        //            }
        //            else
        //            {
        //                data = " ";
        //                xlWorkSheet.Cells[i + 3, 8] = data;
        //            }


        //        }

        //    NextAgyType: continue;

        //        //xlWorkSheet.Cells[i + 1, j + 1] = data;
        //        //}
        //    }

        //    xlWorkSheet.get_Range("a1", "h1").Merge(false);

        //    chartRange = xlWorkSheet.get_Range("a1", "h1");
        //    chartRange.FormulaR1C1 = "Agency Table List";
        //    chartRange.Font.Bold = true;
        //    chartRange.Font.Color = System.Drawing.Color.Blue;
        //    chartRange.Font.Size = 15;
        //    chartRange.HorizontalAlignment = 7;
        //    //chartRange.VerticalAlignment = 1;

        //    //chartRange = xlWorkSheet.get_Range("d2", "c2");


        //    chartRange = xlWorkSheet.get_Range("b2", "i2");
        //    chartRange.Font.Bold = true;
        //    //string PdfName = "Pdf File";
        //    //PdfName = PdfName + ".xls";

        //    xlWorkBook.SaveAs(FileName, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
        //    xlWorkBook.Close(true, misValue, misValue);
        //    xlApp.Quit();

        //    releaseObject(xlWorkSheet);
        //    releaseObject(xlWorkBook);
        //    releaseObject(xlApp);

        //    MessageBox.Show("AgencyTable.xls file created , you can find the file " + PdfName);

        //}

        //private void ExcelCaseDemographics(string FileName)
        //{
        //    try
        //    {
        //        string data = null;
        //        int i = 0;
        //        int j = 0;
        //        string PrivAgyDesc = null;
        //        string privTable = null;


        //        Excel.Application xlApp;
        //        Excel.Workbook xlWorkBook;
        //        Excel.Worksheet xlWorkSheet;
        //        Excel.Range chartRange;
        //        object misValue = System.Reflection.Missing.Value;

        //        xlApp = new Excel.Application();
        //        xlWorkBook = xlApp.Workbooks.Add(misValue);
        //        xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

        //        DataSet ds = DatabaseLayer.ADMNB001DB.ADMNB001_GetCSBCAT();
        //        DataTable dt = ds.Tables[0];
        //        DataSet dsAssoc = DatabaseLayer.ADMNB001DB.ADMNB001_GetCSB4ASSOC();
        //        DataTable dtAssoc = dsAssoc.Tables[0];


        //        xlWorkSheet.Cells[2, 2] = "CSBG Category#";
        //        xlWorkSheet.Cells[2, 3] = "Description Codes Used";
        //        xlWorkSheet.Cells[2, 4] = "Age From";
        //        xlWorkSheet.Cells[2, 5] = "Age To";
        //        xlWorkSheet.Cells[2, 6] = "Table#";
        //        xlWorkSheet.Cells[2, 7] = "Code";
        //        xlWorkSheet.Cells[2, 8] = "Desc";

        //        xlWorkSheet.Cells[2, 1].EntireColumn.ColumnWidth = "2";
        //        xlWorkSheet.Cells[2, 2].EntireColumn.ColumnWidth = "15";
        //        xlWorkSheet.Cells[2, 3].EntireColumn.ColumnWidth = "30";
        //        xlWorkSheet.Cells[2, 4].EntireColumn.ColumnWidth = "8";
        //        xlWorkSheet.Cells[2, 5].EntireColumn.ColumnWidth = "7";
        //        xlWorkSheet.Cells[2, 6].EntireColumn.ColumnWidth = "6";
        //        xlWorkSheet.Cells[2, 7].EntireColumn.ColumnWidth = "5";
        //        xlWorkSheet.Cells[2, 8].EntireColumn.ColumnWidth = "5";

        //        //xlWorkSheet.Cells[3,6].EntireColumn.IndentLevel = "1";
        //        int c = 0;
        //        string AgyType = null, Temp_Agy_Codes = null, Temp_Code = null;
        //        for (i = 0; i <= dt.Rows.Count - 1; i++)
        //        {
        //            if (PrivAgyDesc != dt.Rows[i]["AGY_DESC"].ToString())
        //            {
        //                data = dt.Rows[i]["AGY_DESC"].ToString();
        //                xlWorkSheet.Cells[c + 3, 2] = data;
        //                PrivAgyDesc = dt.Rows[i]["AGY_DESC"].ToString();
        //                AgyType = dt.Rows[i]["CSB4CATG_AGY_TYPE"].ToString();
        //            }
        //            else
        //                xlWorkSheet.Cells[i + 3, 2] = " ";
        //            for (j = 0; j <= dtAssoc.Rows.Count - 1; j++)
        //            {
        //                if (dtAssoc.Rows[j]["CSB4ASOC_CATG_CODE"].ToString() == dt.Rows[i]["CSB4CATG_CODE"].ToString())
        //                {
        //                    data = dtAssoc.Rows[j]["CSB4CATG_DESC"].ToString();
        //                    xlWorkSheet.Cells[c + 3, 3] = data;

        //                    data = dtAssoc.Rows[j]["CSB4ASOC_AGE_FROM"].ToString();
        //                    xlWorkSheet.Cells[c + 3, 4] = data;

        //                    data = dtAssoc.Rows[j]["CSB4ASOC_AGE_TO"].ToString();
        //                    xlWorkSheet.Cells[c + 3, 5] = data;

        //                    Temp_Agy_Codes = dtAssoc.Rows[j]["CSB4ASOC_AGYTAB_CODES"].ToString();
        //                    PrivAgyDesc = null;
        //                    if (privTable != dt.Rows[i]["TableUsed"].ToString())
        //                    {
        //                        data = dt.Rows[i]["TableUsed"].ToString().Substring(0, 5).ToString();
        //                        xlWorkSheet.Cells[c + 3, 6] = data;

        //                        data = dt.Rows[i]["TableUsed"].ToString().Substring(6, 1).ToString();
        //                        xlWorkSheet.Cells[c + 3, 7] = data;

        //                        data = dt.Rows[i]["TableUsed"].ToString().Substring(8, 1).ToString();
        //                        xlWorkSheet.Cells[c + 3, 8] = data;

        //                    }
        //                    else
        //                    {
        //                        xlWorkSheet.Cells[c + 3, 6] = " ";
        //                        xlWorkSheet.Cells[c + 3, 7] = " ";
        //                        xlWorkSheet.Cells[c + 3, 8] = " ";
        //                    }
        //                    c++;
        //                    DataSet dsAgyCodes = DatabaseLayer.Lookups.GetLookUpFromAGYTAB(AgyType);
        //                    DataTable dtAgyCodes = dsAgyCodes.Tables[0];
        //                    foreach (DataRow drAgyCodes in dtAgyCodes.Rows)
        //                    {
        //                        //bool Code_Exists = false;
        //                        bool Can_Continue = true;
        //                        if (!string.IsNullOrEmpty(Temp_Agy_Codes) && Temp_Agy_Codes.Length >= 5)
        //                        {
        //                            for (int k = 0; Can_Continue; )
        //                            {
        //                                if (Temp_Agy_Codes.Length >= 5)
        //                                {
        //                                    if (Temp_Agy_Codes.Substring(k, Temp_Agy_Codes.Length - k).Length < 5)
        //                                        Temp_Code = Temp_Agy_Codes.Substring(k, Temp_Agy_Codes.Length - k);
        //                                    else
        //                                        Temp_Code = Temp_Agy_Codes.Substring(k, 5);
        //                                    if (drAgyCodes["Code"].ToString().Trim() == Temp_Code.Trim())
        //                                    {
        //                                        data = "    " + drAgyCodes["Code"].ToString() + "   " + drAgyCodes["LookUpDesc"].ToString();
        //                                        xlWorkSheet.Cells[c + 3, 3] = data;
        //                                        c++;
        //                                    }

        //                                }

        //                                k += 5;
        //                                if (k >= Temp_Agy_Codes.Length)
        //                                    Can_Continue = false;
        //                            }
        //                        }
        //                    }
        //                    c++;
        //                }
        //            }
        //        }

        //        xlWorkSheet.get_Range("a1", "h1").Merge(false);
        //        //xlWorkSheet.get_Range("f3", "f" + c).IndentLevel = 2;

        //        chartRange = xlWorkSheet.get_Range("a1", "h1");
        //        chartRange.FormulaR1C1 = "Case Demographics Associations";
        //        chartRange.Font.Bold = true;
        //        chartRange.Font.Color = System.Drawing.Color.Blue;
        //        chartRange.Font.Size = 15;
        //        chartRange.HorizontalAlignment = 7;

        //        //chartRange.VerticalAlignment = 1;

        //        //chartRange = xlWorkSheet.get_Range("d2", "c2");


        //        chartRange = xlWorkSheet.get_Range("b2", "i2");
        //        chartRange.Font.Bold = true;

        //        //PdfName = PdfName + ".xls";

        //        xlWorkBook.SaveAs(FileName, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
        //        xlWorkBook.Close(true, misValue, misValue);
        //        xlApp.Quit();

        //        releaseObject(xlWorkSheet);
        //        releaseObject(xlWorkBook);
        //        releaseObject(xlApp);

        //        //MessageBox.Show("Excel file created , you can find the file " + PdfName);
        //    }
        //    catch (Exception ex) { }
        //}

        //private void ExcelPerformanceMeasures(string FileName)
        //{
        //    try
        //    {
        //        string data = null;
        //        int i = 0;
        //        int j = 0;
        //        string PrivAgyDesc = null;
        //        string privTable = null;


        //        Excel.Application xlApp;
        //        Excel.Workbook xlWorkBook;
        //        Excel.Worksheet xlWorkSheet;
        //        Excel.Range chartRange;
        //        object misValue = System.Reflection.Missing.Value;

        //        xlApp = new Excel.Application();
        //        xlWorkBook = xlApp.Workbooks.Add(misValue);
        //        xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);


        //        xlWorkSheet.Cells[2, 2] = "Fdate";
        //        xlWorkSheet.Cells[2, 3] = "Tdate";
        //        xlWorkSheet.Cells[2, 4] = "Grp Cd";
        //        xlWorkSheet.Cells[2, 5] = "Tbl Cd";
        //        xlWorkSheet.Cells[2, 6] = "Description";
        //        xlWorkSheet.Cells[2, 7] = "Ind";
        //        xlWorkSheet.Cells[2, 8] = "Acheive";
        //        xlWorkSheet.Cells[2, 9] = "CC";

        //        xlWorkSheet.Cells[2, 1].EntireColumn.ColumnWidth = "2";
        //        xlWorkSheet.Cells[2, 2].EntireColumn.ColumnWidth = "10";
        //        xlWorkSheet.Cells[2, 3].EntireColumn.ColumnWidth = "10";
        //        xlWorkSheet.Cells[2, 4].EntireColumn.ColumnWidth = "7";
        //        xlWorkSheet.Cells[2, 5].EntireColumn.ColumnWidth = "7";
        //        xlWorkSheet.Cells[2, 6].EntireColumn.ColumnWidth = "70";
        //        xlWorkSheet.Cells[2, 7].EntireColumn.ColumnWidth = "5";
        //        xlWorkSheet.Cells[2, 8].EntireColumn.ColumnWidth = "7";
        //        xlWorkSheet.Cells[2, 9].EntireColumn.ColumnWidth = "4";

        //        //xlWorkSheet.Cells[3,6].EntireColumn.IndentLevel = "1";
        //        int c = 0; string empty = " ";
        //        DataSet ds = DatabaseLayer.SPAdminDB.Get_CSB14GRP(null, null, null, empty, null);
        //        DataTable dt = ds.Tables[0];
        //        DataSet dsCsbRa = DatabaseLayer.SPAdminDB.Get_CSB14RA(null, null, null, null);
        //        DataTable dtCsbRa = dsCsbRa.Tables[0];

        //        for (i = 0; i < dt.Rows.Count - 1; i++)
        //        {
        //            string Grp_List = null; string group = null;
        //            //X_Pos = 60; Y_Pos -= 15;
        //            data = LookupDataAccess.Getdate(dt.Rows[i]["csb14grp_ref_fdate"].ToString());
        //            xlWorkSheet.Cells[c + 3, 2] = data;

        //            data = LookupDataAccess.Getdate(dt.Rows[i]["csb14grp_ref_tdate"].ToString());
        //            xlWorkSheet.Cells[c + 3, 3] = data;

        //            data = dt.Rows[i]["csb14grp_group_code"].ToString();
        //            xlWorkSheet.Cells[c + 3, 4] = data;

        //            data = dt.Rows[i]["csb14grp_desc"].ToString();
        //            xlWorkSheet.Cells[c + 3, 6] = data;

        //            group = dt.Rows[i]["csb14grp_group_code"].ToString();
        //            string RefFdate = dt.Rows[i]["csb14grp_ref_fdate"].ToString();
        //            string RefTdate = dt.Rows[i]["csb14grp_ref_tdate"].ToString();
        //            if (!string.IsNullOrEmpty(dt.Rows[i]["csb14grp_cnt_hdr1"].ToString()) && dt.Rows[i]["csb14grp_cnt_incld1"].ToString() == "True")
        //                Grp_List = "1";
        //            if (!string.IsNullOrEmpty(dt.Rows[i]["csb14grp_cnt_hdr2"].ToString()) && dt.Rows[i]["csb14grp_cnt_incld2"].ToString() == "True")
        //                Grp_List += "2";
        //            if (!string.IsNullOrEmpty(dt.Rows[i]["csb14grp_cnt_hdr3"].ToString()) && dt.Rows[i]["csb14grp_cnt_incld3"].ToString() == "True")
        //                Grp_List += "3";
        //            if (!string.IsNullOrEmpty(dt.Rows[i]["csb14grp_cnt_hdr4"].ToString()) && dt.Rows[i]["csb14grp_cnt_incld4"].ToString() == "True")
        //                Grp_List += "4";
        //            if (!string.IsNullOrEmpty(dt.Rows[i]["csb14grp_cnt_hdr5"].ToString()) && dt.Rows[i]["csb14grp_cnt_incld5"].ToString() == "True")
        //                Grp_List += "5";
        //            for (int k = 0; i < Grp_List.Length; i++)
        //            {
        //                c++;//Y_Pos -= 14;
        //                if (Grp_List.Substring(i, 1) == "1")
        //                {
        //                    data = "    " + dt.Rows[i]["csb14grp_cnt_hdr1"].ToString();
        //                    xlWorkSheet.Cells[c + 3, 6] = data;
        //                }
        //                else if (Grp_List.Substring(i, 1) == "2")
        //                {
        //                    data = "    " + dt.Rows[i]["csb14grp_cnt_hdr2"].ToString();
        //                    xlWorkSheet.Cells[c + 3, 6] = data;
        //                }
        //                else if (Grp_List.Substring(i, 1) == "3")
        //                {
        //                    data = "    " + dt.Rows[i]["csb14grp_cnt_hdr3"].ToString();
        //                    xlWorkSheet.Cells[c + 3, 6] = data;
        //                }
        //                else if (Grp_List.Substring(i, 1) == "4")
        //                {
        //                    data = "    " + dt.Rows[i]["csb14grp_cnt_hdr4"].ToString();
        //                    xlWorkSheet.Cells[c + 3, 6] = data;
        //                }
        //                else
        //                {
        //                    data = "    " + dt.Rows[i]["csb14grp_cnt_hdr5"].ToString();
        //                    xlWorkSheet.Cells[c + 3, 6] = data;
        //                }

        //                foreach (DataRow drCsbRa in dtCsbRa.Rows)
        //                {
        //                    if (drCsbRa["CSB14RA_REF_FDATE"].ToString() == dt.Rows[i]["csb14grp_ref_fdate"].ToString() && drCsbRa["CSB14RA_REF_TDATE"].ToString() == dt.Rows[i]["csb14grp_ref_tdate"].ToString() && drCsbRa["CSB14RA_GROUP_CODE"].ToString() == dt.Rows[i]["csb14grp_group_code"].ToString())
        //                    {
        //                        string Grp = Grp_List.Substring(i, 1);
        //                        if (drCsbRa["CSB14RA_COUNT_CODE"].ToString() == Grp_List.Substring(i, 1))
        //                        {
        //                            c++;//Y_Pos -= 14;
        //                            data = "        " + drCsbRa["CSB14RA_RESULT_CODE"].ToString().Trim() + "      " + drCsbRa["CSB14RA_DESC"].ToString().Trim();
        //                            xlWorkSheet.Cells[c + 3, 6] = data;
        //                        }
        //                    }
        //                }
        //            }
        //            DataSet dsGrp = DatabaseLayer.SPAdminDB.Get_CSB14GRP(RefFdate, RefTdate, group, null, null);
        //            DataTable dtGrp = dsGrp.Tables[0];
        //            foreach (DataRow drGrp in dtGrp.Rows)
        //            {
        //                if (!string.IsNullOrWhiteSpace(drGrp["csb14grp_table_code"].ToString()))
        //                {
        //                    c++;//Y_Pos -= 14;
        //                    if (!string.IsNullOrEmpty(drGrp["csb14grp_table_code"].ToString()))
        //                    {
        //                        data = drGrp["csb14grp_table_code"].ToString();
        //                        xlWorkSheet.Cells[c + 3, 5] = data;

        //                        data = drGrp["csb14grp_desc"].ToString();
        //                        xlWorkSheet.Cells[c + 3, 6] = data;

        //                        data = drGrp["csb14grp_cnt_indicator"].ToString();
        //                        xlWorkSheet.Cells[c + 3, 7] = data;

        //                        data = drGrp["csb14grp_expect_achieve"].ToString();
        //                        xlWorkSheet.Cells[c + 3, 8] = data;

        //                        data = drGrp["csb14grp_calc_cost"].ToString();
        //                        xlWorkSheet.Cells[c + 3, 9] = data;

        //                    }
        //                }
        //            }

        //        }

        //        xlWorkSheet.get_Range("a1", "i1").Merge(false);
        //        //xlWorkSheet.get_Range("f3", "f" + c).IndentLevel = 2;

        //        chartRange = xlWorkSheet.get_Range("a1", "i1");
        //        chartRange.FormulaR1C1 = "Performance Measures Associations";
        //        chartRange.Font.Bold = true;
        //        chartRange.Font.Color = System.Drawing.Color.Blue;
        //        chartRange.Font.Size = 15;
        //        chartRange.HorizontalAlignment = 7;
        //        //chartRange.VerticalAlignment = 1;
        //        //chartRange = xlWorkSheet.get_Range("d2", "c2");

        //        chartRange = xlWorkSheet.get_Range("b2", "i2");
        //        chartRange.Font.Bold = true;

        //        //PdfName = PdfName + ".xls";

        //        xlWorkBook.SaveAs(FileName, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
        //        xlWorkBook.Close(true, misValue, misValue);
        //        xlApp.Quit();

        //        releaseObject(xlWorkSheet);
        //        releaseObject(xlWorkBook);
        //        releaseObject(xlApp);

        //        //MessageBox.Show("Excel file created , you can find the file " + PdfName);
        //    }
        //    catch (Exception ex) { }
        //}

        //private void ExcelCriticalActivities(string FileName)
        //{
        //    try
        //    {
        //        string data = null;
        //        int i = 0;
        //        int j = 0;
        //        string PrivAgyDesc = null;
        //        string privTable = null;


        //        Excel.Application xlApp;
        //        Excel.Workbook xlWorkBook;
        //        Excel.Worksheet xlWorkSheet;
        //        Excel.Range chartRange;
        //        object misValue = System.Reflection.Missing.Value;

        //        xlApp = new Excel.Application();
        //        xlWorkBook = xlApp.Workbooks.Add(misValue);
        //        xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);


        //        xlWorkSheet.Cells[2, 2] = "Code";
        //        xlWorkSheet.Cells[2, 3] = "Description";


        //        xlWorkSheet.Cells[2, 1].EntireColumn.ColumnWidth = "2";
        //        xlWorkSheet.Cells[2, 2].EntireColumn.ColumnWidth = "10";
        //        xlWorkSheet.Cells[2, 3].EntireColumn.ColumnWidth = "80";


        //        //xlWorkSheet.Cells[3,6].EntireColumn.IndentLevel = "1";
        //        int c = 0; string empty = " ";
        //        string SortCol = null;
        //        if (RblEmrServ.Checked == true)
        //            SortCol = "Description";

        //        DataSet ds = DatabaseLayer.ADMNB001DB.ADMNB001_GetCamast(SortCol, null);
        //        DataTable dt = ds.Tables[0];

        //        if (dt.Rows.Count > 0)
        //        {
        //            for (i = 0; i <= dt.Rows.Count - 1; i++)
        //            {
        //                data = dt.Rows[i]["CA_CODE"].ToString();
        //                xlWorkSheet.Cells[c + 3, 2] = data;

        //                data = dt.Rows[i]["CA_DESC"].ToString();
        //                xlWorkSheet.Cells[c + 3, 3] = data;

        //                c++;
        //            }
        //        }

        //        xlWorkSheet.get_Range("a1", "c1").Merge(false);
        //        //xlWorkSheet.get_Range("f3", "f" + c).IndentLevel = 2;

        //        chartRange = xlWorkSheet.get_Range("a1", "c1");
        //        chartRange.FormulaR1C1 = "Critical Activities List";
        //        chartRange.Font.Bold = true;
        //        chartRange.Font.Color = System.Drawing.Color.Blue;
        //        chartRange.Font.Size = 15;
        //        chartRange.HorizontalAlignment = 7;
        //        //chartRange.VerticalAlignment = 1;
        //        //chartRange = xlWorkSheet.get_Range("d2", "c2");

        //        chartRange = xlWorkSheet.get_Range("b2", "c2");
        //        chartRange.Font.Bold = true;

        //        //PdfName = PdfName + ".xls";

        //        xlWorkBook.SaveAs(FileName, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
        //        xlWorkBook.Close(true, misValue, misValue);
        //        xlApp.Quit();

        //        releaseObject(xlWorkSheet);
        //        releaseObject(xlWorkBook);
        //        releaseObject(xlApp);

        //        //MessageBox.Show("Excel file created , you can find the file " + PdfName);
        //    }
        //    catch (Exception ex) { }
        //}

        //private void ExcelMilestonslist(string FileName)
        //{
        //    try
        //    {
        //        string data = null;
        //        int i = 0;
        //        int j = 0;

        //        Excel.Application xlApp;
        //        Excel.Workbook xlWorkBook;
        //        Excel.Worksheet xlWorkSheet;
        //        Excel.Range chartRange;
        //        object misValue = System.Reflection.Missing.Value;

        //        xlApp = new Excel.Application();
        //        xlWorkBook = xlApp.Workbooks.Add(misValue);
        //        xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

        //        xlWorkSheet.Cells[2, 2] = "Code";
        //        xlWorkSheet.Cells[2, 3] = "Description";

        //        xlWorkSheet.Cells[2, 1].EntireColumn.ColumnWidth = "2";
        //        xlWorkSheet.Cells[2, 2].EntireColumn.ColumnWidth = "10";
        //        xlWorkSheet.Cells[2, 3].EntireColumn.ColumnWidth = "80";

        //        int c = 0;
        //        string SortCol = null;
        //        if (RblEmrServ.Checked == true)
        //            SortCol = "Description";

        //        DataSet ds = DatabaseLayer.ADMNB001DB.ADMNB001_GetMsmast(SortCol, null);
        //        DataTable dt = ds.Tables[0];

        //        if (dt.Rows.Count > 0)
        //        {
        //            for (i = 0; i <= dt.Rows.Count - 1; i++)
        //            {
        //                data = dt.Rows[i]["MS_CODE"].ToString();
        //                xlWorkSheet.Cells[c + 3, 2] = data;

        //                data = dt.Rows[i]["MS_DESC"].ToString();
        //                xlWorkSheet.Cells[c + 3, 3] = data;

        //                c++;
        //            }
        //        }

        //        xlWorkSheet.get_Range("a1", "c1").Merge(false);
        //        //xlWorkSheet.get_Range("f3", "f" + c).IndentLevel = 2;

        //        chartRange = xlWorkSheet.get_Range("a1", "c1");
        //        chartRange.FormulaR1C1 = "Milestones List";
        //        chartRange.Font.Bold = true;
        //        chartRange.Font.Color = System.Drawing.Color.Blue;
        //        chartRange.Font.Size = 15;
        //        chartRange.HorizontalAlignment = 7;
        //        //chartRange.VerticalAlignment = 1;
        //        //chartRange = xlWorkSheet.get_Range("d2", "c2");

        //        chartRange = xlWorkSheet.get_Range("b2", "c2");
        //        chartRange.Font.Bold = true;

        //        //PdfName = PdfName + ".xls";

        //        xlWorkBook.SaveAs(FileName, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
        //        xlWorkBook.Close(true, misValue, misValue);
        //        xlApp.Quit();

        //        releaseObject(xlWorkSheet);
        //        releaseObject(xlWorkBook);
        //        releaseObject(xlApp);

        //        //MessageBox.Show("Excel file created , you can find the file " + PdfName);
        //    }
        //    catch (Exception ex) { }
        //}

        //private void ExcelZipCode(string FileName)
        //{
        //    try
        //    {
        //        string data = null;
        //        int i = 0;
        //        int j = 0;

        //        Excel.Application xlApp;
        //        Excel.Workbook xlWorkBook;
        //        Excel.Worksheet xlWorkSheet;
        //        Excel.Range chartRange;
        //        object misValue = System.Reflection.Missing.Value;

        //        xlApp = new Excel.Application();
        //        xlWorkBook = xlApp.Workbooks.Add(misValue);
        //        xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

        //        xlWorkSheet.Cells[2, 2] = "Zipcode";
        //        xlWorkSheet.Cells[2, 3] = "City";
        //        xlWorkSheet.Cells[2, 4] = "State";
        //        xlWorkSheet.Cells[2, 5] = "Intake Cd";
        //        xlWorkSheet.Cells[2, 6] = "Month";
        //        xlWorkSheet.Cells[2, 7] = "Date";
        //        xlWorkSheet.Cells[2, 8] = "Year";

        //        xlWorkSheet.Cells[2, 1].EntireColumn.ColumnWidth = "2";
        //        xlWorkSheet.Cells[2, 2].EntireColumn.ColumnWidth = "10";
        //        xlWorkSheet.Cells[2, 3].EntireColumn.ColumnWidth = "30";
        //        xlWorkSheet.Cells[2, 4].EntireColumn.ColumnWidth = "8";
        //        xlWorkSheet.Cells[2, 5].EntireColumn.ColumnWidth = "10";
        //        xlWorkSheet.Cells[2, 6].EntireColumn.ColumnWidth = "12";
        //        xlWorkSheet.Cells[2, 7].EntireColumn.ColumnWidth = "8";
        //        xlWorkSheet.Cells[2, 8].EntireColumn.ColumnWidth = "30";

        //        int c = 0;
        //        string SortCol = null;
        //        if (RblEmrServ.Checked == true)
        //            SortCol = "City";
        //        DataSet ds = DatabaseLayer.ADMNB001DB.ADMNB001_ZipcodeSearch(SortCol);
        //        DataTable dt = ds.Tables[0];
        //        if (dt.Rows.Count > 0)
        //        {
        //            for (i = 0; i <= dt.Rows.Count - 1; i++)
        //            {
        //                data = dt.Rows[i]["ZCR_ZIP"].ToString();
        //                xlWorkSheet.Cells[c + 3, 2] = data;

        //                data = dt.Rows[i]["ZCR_CITY"].ToString();
        //                xlWorkSheet.Cells[c + 3, 3] = data;

        //                data = dt.Rows[i]["ZCR_STATE"].ToString();
        //                xlWorkSheet.Cells[c + 3, 4] = data;

        //                data = dt.Rows[i]["ZCR_INTAKE_CODE"].ToString();
        //                xlWorkSheet.Cells[c + 3, 5] = data;

        //                data = GetMonth(dt.Rows[i]["ZCR_HSS_MO"].ToString());
        //                xlWorkSheet.Cells[c + 3, 6] = data;

        //                data = dt.Rows[i]["ZCR_HSS_DAY"].ToString();
        //                xlWorkSheet.Cells[c + 3, 7] = data;

        //                data = GetYear(dt.Rows[i]["ZCR_HSS_YEAR"].ToString());
        //                xlWorkSheet.Cells[c + 3, 8] = data;

        //                c++;
        //            }
        //        }


        //        xlWorkSheet.get_Range("a1", "h1").Merge(false);
        //        //xlWorkSheet.get_Range("f3", "f" + c).IndentLevel = 2;

        //        chartRange = xlWorkSheet.get_Range("a1", "h1");
        //        chartRange.FormulaR1C1 = "Zipcodes";
        //        chartRange.Font.Bold = true;
        //        chartRange.Font.Color = System.Drawing.Color.Blue;
        //        chartRange.Font.Size = 15;
        //        chartRange.HorizontalAlignment = 7;
        //        //chartRange.VerticalAlignment = 1;
        //        //chartRange = xlWorkSheet.get_Range("d2", "c2");

        //        chartRange = xlWorkSheet.get_Range("b2", "h2");
        //        chartRange.Font.Bold = true;

        //        //PdfName = PdfName + ".xls";

        //        xlWorkBook.SaveAs(FileName, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
        //        xlWorkBook.Close(true, misValue, misValue);
        //        xlApp.Quit();

        //        releaseObject(xlWorkSheet);
        //        releaseObject(xlWorkBook);
        //        releaseObject(xlApp);

        //        //MessageBox.Show("Excel file created , you can find the file " + PdfName);
        //    }
        //    catch (Exception ex) { }
        //}

        private void ExcelAgencyTable(string FileName)
        {
            Random_Filename = null;
            PdfName = "Pdf File";
            PdfName = "AgencyTable_" + "Report";

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

            Workbook book = new Workbook();

            this.GenerateStyles(book.Styles);

            Worksheet sheet = book.Worksheets.Add("Sheet1");
            sheet.Table.DefaultRowHeight = 14.25F;

            DataSet ds = DatabaseLayer.ADMNB001DB.ADMNB001_GetaAgyTabList(FileName);
            DataTable dt = ds.Tables[0];

            sheet.Table.Columns.Add(120);
            sheet.Table.Columns.Add(250);
            //sheet.Table.Columns.Add(250);
            //sheet.Table.Columns.Add(52);
            //sheet.Table.Columns.Add(75);
            //sheet.Table.Columns.Add(75);

            string Agy_Code_Cell = string.Empty;
            string Agy_Desc_Cell = string.Empty;
            string PrivAgyType = string.Empty;
            string HeaderDesc = string.Empty;

            string Code_cell = string.Empty;
            string Desc_Cell = string.Empty;

            bool First = true;

            if (dt.Rows.Count > 0)
            {
                DataTable dtCodes = new DataTable();

                dtCodes = dtModules;
                DataView dvAgyCodes = new DataView(dtCodes);
                if (((RepListItem)cmbModule.SelectedItem).Value.ToString() != "*****")
                {
                    dvAgyCodes.RowFilter = "AGY_TYPE= '" + ((RepListItem)cmbModule.SelectedItem).Value.ToString() + "'";
                }

                //dvAgyCodes.RowFilter = "AGY_CODE='00000'";
                if (RblEmrServ.Checked)
                    dvAgyCodes.Sort = "AGY_DESC";
                else if (!RblEmrServ.Checked) dvAgyCodes.Sort = "AGY_TYPE";
                dtCodes = dvAgyCodes.ToTable();

                WorksheetRow Row0 = sheet.Table.Rows.Add();

                WorksheetCell cell;

                foreach (DataRow dr in dtCodes.Rows)
                {

                    int number;
                    if (dr["AGY_CODE"].ToString() == "00000")
                    {

                        if (string.IsNullOrEmpty(dr["AGY_ACTIVE"].ToString()) || string.IsNullOrEmpty(dr["AGY_DEFAULT"].ToString()) ||
                          (!(int.TryParse(dr["AGY_ACTIVE"].ToString(), out number))) || (!(int.TryParse(dr["AGY_DEFAULT"].ToString(), out number))))
                        {
                            PrivAgyType = dr["AGY_TYPE"].ToString(); goto NextAgyType;
                        }
                    }

                    if (dr["AGY_TYPE"].ToString() != PrivAgyType)
                    {
                        HeaderDesc = dr["AGY_DESC"].ToString().Trim();

                        Agy_Code_Cell = "AGY_" + dr["AGY_ACTIVE"].ToString();
                        Agy_Desc_Cell = "AGY_" + dr["AGY_DEFAULT"].ToString();

                        Code_cell = dr["AGY_ACTIVE"].ToString();
                        Desc_Cell = dr["AGY_DEFAULT"].ToString();

                        if (!First)
                        {
                            Row0 = sheet.Table.Rows.Add();
                            cell = Row0.Cells.Add("", DataType.String, "s95");
                            cell = Row0.Cells.Add("", DataType.String, "s95");
                        }


                        Row0 = sheet.Table.Rows.Add();
                        cell = Row0.Cells.Add(HeaderDesc.Trim(), DataType.String, "s96");
                        cell.MergeAcross = 1;

                        First = false; PrivAgyType = dr["AGY_TYPE"].ToString();
                    }

                    DataView dv = new DataView(dt);
                    dv.RowFilter = "AGY_TYPE=" + "'" + PrivAgyType + "'";
                    DataTable dtDetail = dv.ToTable();



                    if (dtDetail.Rows.Count > 0)
                    {
                        Row0 = sheet.Table.Rows.Add();

                        cell = Row0.Cells.Add("Code (Cell " + Code_cell + ")", DataType.String, "s96");
                        cell = Row0.Cells.Add("Description (Cell " + Desc_Cell + ")", DataType.String, "s96");

                        foreach (DataRow dr1 in dtDetail.Rows)
                        {
                            Row0 = sheet.Table.Rows.Add();

                            if (dr1["AGY_TYPE"].ToString() == PrivAgyType && dr1["AGY_CODE"].ToString() != "00000")
                            {
                                if (!string.IsNullOrEmpty(Agy_Code_Cell.Trim()))
                                    cell = Row0.Cells.Add(dr1[Agy_Code_Cell].ToString().Trim(), DataType.String, "s95C");
                                else
                                    cell = Row0.Cells.Add("", DataType.String, "s95C");

                                if (!string.IsNullOrEmpty(Agy_Desc_Cell.Trim()))
                                    cell = Row0.Cells.Add(dr1[Agy_Desc_Cell].ToString().Trim(), DataType.String, "s95");
                                else
                                    cell = Row0.Cells.Add("", DataType.String, "s95");


                            }
                        }

                    }
                NextAgyType: continue;
                }
            }

            FileStream stream = new FileStream(PdfName, FileMode.Create);

            book.Save(stream);
            stream.Close();

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
        }



        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

        private void RblEmrServ_CheckedChanged(object sender, EventArgs e)
        {
            if (RepType == "01")
                PopulateDropDowns();
        }

        DataTable dtModules;
        private void CmbAgyTab_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RepType == "01")
            {
                string module = ((Captain.Common.Utilities.ListItem)CmbAgyTab.SelectedItem).Value.ToString();
                DataSet ds = Captain.DatabaseLayer.AgyTab.GetAgencyTableByApp(module);
                cmbModule.Items.Clear();
                if (ds.Tables.Count > 0)
                {
                    dtModules = ds.Tables[0];
                    Fill_AgyType_Combo();
                }
            }

            if (RepType == "10")
            {
                if (((Captain.Common.Utilities.ListItem)CmbAgyTab.SelectedItem).ValueDisplayCode == "N")
                {
                    this.Size = new System.Drawing.Size(435, 226);
                    rbStandard.Checked = true;
                    rbStandard.Visible = false; rbBoth.Visible = false; rbCustom.Visible = false;
                    lblType.Visible = false;
                }
                else
                {
                    this.Size = new System.Drawing.Size(435, 256);
                    //rbStandard.Checked = true;
                    rbStandard.Visible = true; rbBoth.Visible = true; rbCustom.Visible = true;
                    lblType.Visible = true;
                }
            }
        }

        private void Fill_AgyType_Combo()
        {
            if (dtModules.Rows.Count > 0)
            {
                DataRow[] foundRows;

                //if (RblCaseMgmt.Checked == true)
                //    dv.Sort = "AGY_TYPE";
                //else dv.Sort = "AGY_DESC";

                if (RblCaseMgmt.Checked)
                    foundRows = dtModules.Select(null, "AGY_TYPE ASC");
                else
                    foundRows = dtModules.Select(null, "AGY_DESC ASC");

                //Loading_Complete = boolChangeStatus = ValReqBlankItems = false;
                //Previous_Row_Cnt = 0;
                //gvwHierarchy.Rows.Clear();
                //cmbAgencyTable.Items.Clear();
                string Tmp_Desc = string.Empty;

                cmbModule.Items.Insert(0, new RepListItem("All Tables", "*****"));

                foreach (DataRow dr in foundRows)
                {
                    Tmp_Desc = string.Empty;
                    Tmp_Desc = String.Format("{0,-35}", dr["AGY_DESC"].ToString().Trim()) + String.Format("{0,8}", " - " + dr["AGY_TYPE"].ToString());
                    cmbModule.Items.Add(new RepListItem(Tmp_Desc, dr["AGY_TYPE"].ToString()));
                }
                //foreach (DataRow dr in foundRows)
                //{
                //    Tmp_Desc = string.Empty;
                //    Tmp_Desc = String.Format("{0,-50}", dr["AGY_DESC"].ToString().Trim()) + String.Format("{0,8}", " - " + dr["AGY_TYPE"].ToString());

                //    cmbModule.Items.Add(new RepListItem(Tmp_Desc, dr["AGY_TYPE"].ToString()));
                //}
                if (cmbModule.Items.Count > 0)
                    cmbModule.SelectedIndex = 0;
                //OnAgencyTableSelectedIndexChanged(cmbAgencyTable, new EventArgs());
            }
        }

        private void rdoAll_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoAll.Checked == true)
            {
                dtselect.Visible = false;
                dtSelectToDate.Visible = false;
                lblTodate.Visible = false;
                this.Size = new System.Drawing.Size(555, 202);
                CmbRepType.Size = new System.Drawing.Size(421, 25);
                CmbUser.Size = new System.Drawing.Size(421, 25);
            }
            else
            {
                dtselect.Visible = true;
                dtSelectToDate.Visible = true;
                lblTodate.Visible = true;
                this.Size = new System.Drawing.Size(595, 202);
                CmbRepType.Size = new System.Drawing.Size(463, 25);
                CmbUser.Size = new System.Drawing.Size(463, 25);
            }
        }

        private void rdoRepAll_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoRepAll.Checked == true)
                dtRepDate.Visible = false;
            else
                dtRepDate.Visible = true;
        }

        private void ADMNB001_ToolClick(object sender, ToolClickEventArgs e)
        {
            Application.Navigate(CommonFunctions.CreateZenHelps(Privileges.Program, 0, BaseForm.BusinessModuleID.ToString(), "", ""), target: "_blank");
        }

    }
}