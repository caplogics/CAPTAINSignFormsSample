#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;


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


using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
#endregion

namespace Captain.Common.Views.Forms
{
    public partial class IncomeReportForm : Form
    {
        private CaptainModel _model = null;
        private ErrorProvider _errorProvider = null;
        private string strAgency = string.Empty;
        private string strDept = string.Empty;
        private string strProgram = string.Empty;
        private string strYear = string.Empty;
        private string strApplNo = string.Empty;
        private string strMode = string.Empty;
        private string strNameFormat = string.Empty;
        private string strVerfierFormat = string.Empty;
        public static string[] strkeys;

        public IncomeReportForm(string appKey,BaseForm baseForm,string HowVer) //string appKey
        {
            InitializeComponent();
            _model = new CaptainModel();
            BaseForm = baseForm;
            HowVerified = HowVer;
            AppKey = appKey;
            propReportPath = _model.lookupDataAccess.GetReportPath();
            //strFolderPath = "C:\\CapReports\\";      // Run at Local System
            strFolderPath = Consts.Common.ReportFolderLocation + baseForm.UserID + "\\";    // Run at Server
            propAgencyControlDetails = _model.ZipCodeAndAgency.GetAgencyControlFile("00");
            

             //BtnGenPdf_Click(BtnGenPdf, EventArgs.Empty);
            BtnGenPdf1_Click(BtnGenPdf, EventArgs.Empty);
             Close_Form();
        }

        public string AppKey { get; set; }

        public BaseForm BaseForm { get; set; }

        public string HowVerified { get; set; }

        public string propReportPath { get; set; }
        public AgencyControlEntity propAgencyControlDetails { get; set; }
        public AgencyControlEntity propAgencyControlDetailsBase { get; set; }
        public string propHourlyMode { get; set; }
        public DateTime MstIntakeStartDate { get; set; }
        public DateTime MstIntakeEndDate { get; set; }

        string strFolderPath = string.Empty;
        string PdfName = null;
        PdfContentByte cb;
        int X_Pos, Y_Pos;
        decimal Mst_Prog_Icn;
        string Mst_Poverty = null;
        string MST_InProg = null;
        string TmpDOB = null;
        string[] time = null;
        string IntakeDate;
        decimal Tot = 0, Grand_Total = 0,ems_30days_Income=0;

        //private void BtnGenPdf_Click(object sender, EventArgs e)
        //{
        //    string Random_Filename = null;
        //    PdfName = null;
        //    PdfName = AppKey.Substring(0, 10) + "_" + AppKey.Substring(10, 8);

        //    PdfName = propReportPath + BaseForm.UserID + "\\" + PdfName;
        //    try
        //    {
        //        if (!Directory.Exists(propReportPath + BaseForm.UserID.Trim()))
        //        { DirectoryInfo di = Directory.CreateDirectory(propReportPath + BaseForm.UserID.Trim()); }
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonFunctions.MessageBoxDisplay("Error");
        //    }

        //    try
        //    {
        //        string Tmpstr = PdfName + ".pdf";
        //        if (File.Exists(Tmpstr))
        //            File.Delete(Tmpstr);
        //    }
        //    catch (Exception ex)
        //    {
        //        int length = 8;
        //        string newFileName = System.Guid.NewGuid().ToString();
        //        newFileName = newFileName.Replace("-", string.Empty);

        //        Random_Filename = PdfName + "_" + newFileName.Substring(0, length) + ".pdf";
        //    }


        //    if (!string.IsNullOrEmpty(Random_Filename))
        //        PdfName = Random_Filename;
        //    else
        //        PdfName += ".pdf";

        //    System.IO.FileStream fs = new FileStream(PdfName, FileMode.Create);

        //    Document document;
        //    document = new Document();

        //    PdfWriter writer = PdfWriter.GetInstance(document, fs);
        //    document.Open();

        //    //Image _image = iTextSharp.text.Image.GetInstance(Application.MapPath("~\\Resources\\images\\Capsystems_WaterMark.bmp"));
        //    //_image.SetAbsolutePosition(160, 310);
        //    //_image.RotationDegrees = 45;
        //    //_image.Rotate();
        //    //PdfGState _state = new PdfGState()
        //    //{
        //    //    FillOpacity = 0.2F,
        //    //    StrokeOpacity = 0.2F
        //    //};
        //    //cb = writer.DirectContentUnder;
        //    //cb.SaveState();
        //    //cb.SetGState(_state);                               //WaterMark*******
        //    //cb.AddImage(_image);
        //    //cb.RestoreState();

        //    cb = writer.DirectContent;
        //    try
        //    {
        //        //DataSet dsAgcycntl = Captain.DatabaseLayer.ADMNB001DB.ADMNB001_Browse_AGCYCNTL("00", null, null, null, null, null, null);
        //        //string ShortName = string.Empty;
        //        //if (dsAgcycntl != null && dsAgcycntl.Tables[0].Rows.Count > 0)
        //        //    ShortName = dsAgcycntl.Tables[0].Rows[0]["ACR_SHORT_NAME"].ToString().Trim();

        //        DataSet dsIncType = DatabaseLayer.ADMNB001DB.ADMNB001_GetaAgyTabList("00004");
        //        DataTable dtIncType = dsIncType.Tables[0];



        //        DataSet CaseMST = Captain.DatabaseLayer.CaseSnpData.GetCaseMST(AppKey.Substring(0, 2), AppKey.Substring(2, 2), AppKey.Substring(4, 2), AppKey.Substring(6, 4), AppKey.Substring(10, 8));
        //        DataTable dtCaseMst = CaseMST.Tables[0];
        //        DataRow drCaseMst = dtCaseMst.Rows[0];

        //        string Fam_Sqe = drCaseMst["MST_FAMILY_SEQ"].ToString().Trim();
        //        IntakeDate = drCaseMst["MST_INTAKE_DATE"].ToString();
        //        Mst_Prog_Icn = decimal.Parse(drCaseMst["MST_PROG_INCOME"].ToString());
        //        Mst_Poverty = drCaseMst["MST_POVERTY"].ToString();
        //        time = Regex.Split(IntakeDate.ToString(), " ");
        //        IntakeDate = time[0];
        //        time = Regex.Split(IntakeDate.ToString(), " ");
        //        IntakeDate = time[0];


        //        PrintHeaderPage();
        //        //cb.BeginText();
        //        List<AgyTabEntity> lookUpIncomeTypes = _model.lookupDataAccess.GetIncomeTypes();
        //        string strHourlymode = "N";
        //        if (propAgencyControlDetails != null)
        //        {
        //            if (propAgencyControlDetails.IncMethods == "2")
        //                strHourlymode = "Y";
        //        }
        //        List<CommonEntity> commonEntity = _model.lookupDataAccess.GetIncomeInterval(strHourlymode);

        //        DataSet ds = DatabaseLayer.ADMNB002DB.GET_INCOME_REPORT(AppKey.Substring(0, 10), AppKey.Substring(10, 8), Fam_Sqe);
        //        DataTable dt = ds.Tables[0];
        //        DataRow dr;
        //        if (dt.Rows.Count > 0)
        //            dr = dt.Rows[0];

        //        if (propHourlyMode == "Y")
        //        {
        //            if (string.IsNullOrEmpty(drCaseMst["MST_INTAKE_DATE"].ToString()))// == "" || MstIntakeDate == null)
        //            {
        //                MstIntakeEndDate = DateTime.Now;
        //                MstIntakeStartDate = MstIntakeEndDate.AddDays(-30);

        //            }
        //            else
        //            {
        //                MstIntakeEndDate = Convert.ToDateTime(drCaseMst["MST_INTAKE_DATE"].ToString());
        //                MstIntakeStartDate = MstIntakeEndDate.AddDays(-30);
        //            }
        //        }

        //        cb.BeginText();

        //        int Count = 0;
        //        string Prev_fam_Seq = null, prev_income_Seq = null; string Priv_Income_Type = string.Empty;
        //        Y_Pos = 650; bool isfalse = true; string SORT_NEGATIVE = string.Empty;

        //        int i;
        //        if (dt.Rows.Count > 0)
        //        {
        //            DataView dv = new DataView(dt);
        //            //if(ShortName=="UETHDA")
        //            //    dv.Sort = "INCOME_FAMILY_SEQ,INCOME_INTERVAL,INCOME_TYPE";
        //            dv.Sort = "INCOME_FAMILY_SEQ,INCOME_TYPE,INCOME_INTERVAL";
        //            dt = dv.ToTable();
        //            DataTable IncomeTable = new DataTable();
        //            if (propHourlyMode == "Y")
        //            {
        //                foreach (DataRow dr1 in dt.Rows)
        //                {
        //                    DataRow drInc = IncomeTable.NewRow();
        //                    for (i = 1; i < 6; i++)
        //                    {
        //                        if (!string.IsNullOrEmpty(dr1["INCOME_DATE" + i.ToString()].ToString()))
        //                        {
        //                            if (Convert.ToDateTime(dr1["INCOME_DATE" + i.ToString()].ToString()) < MstIntakeStartDate || Convert.ToDateTime(dr1["INCOME_DATE" + i.ToString()].ToString()) < MstIntakeEndDate)
        //                            {
        //                                dr1["INCOME_DATE" + i.ToString()] = string.Empty;
        //                                dr1["INCOME_VAL" + i.ToString()] = "0.00";
        //                            }

        //                            drInc = dr1;
        //                        }


        //                    }
        //                }
        //            }
        //            else
        //            {
        //                IncomeTable = dt;
        //            }

        //            foreach (DataRow dr1 in IncomeTable.Rows)
        //            {
        //                if (Prev_fam_Seq != dr1["INCOME_FAMILY_SEQ"].ToString().Trim())
        //                    Priv_Income_Type = null;

        //                if (!isfalse)
        //                {
        //                    if (Priv_Income_Type != dr1["INCOME_TYPE"].ToString().Trim() || dt.Rows.Count == Count)
        //                    {
        //                        Grand_Total += Tot;
        //                        Y_Pos -= 6; X_Pos = 415;//X_Pos = 435;
        //                        cb.MoveTo(X_Pos, Y_Pos);
        //                        cb.LineTo(X_Pos + 50, Y_Pos);
        //                        cb.SetLineWidth(0.5f);
        //                        cb.Stroke();
        //                        Y_Pos -= 10; X_Pos = 415;// X_Pos = 435;
        //                        CheckBottomBorderReached(document, writer);
        //                        cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_BOLD).BaseFont, 11);
        //                        PrintRec("$ " + Tot.ToString(), 20);
        //                    }
        //                }

        //                TmpDOB = null; Count++;
        //                X_Pos = 50; Y_Pos -= 14;
        //                CheckBottomBorderReached(document, writer);
        //                if (Prev_fam_Seq != dr1["INCOME_FAMILY_SEQ"].ToString().Trim())
        //                {
        //                    if (Count > 1) { Y_Pos -= 10; CheckBottomBorderReached(document, writer); }
        //                    cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ITALIC).BaseFont, 12);
        //                    cb.SetColorFill(BaseColor.BLUE);
        //                    PrintRec(dr1["NAME"].ToString(), 150);
        //                    cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ROMAN).BaseFont, 11);
        //                    cb.SetCMYKColorFill(0, 0, 0, 255);
        //                    string strSsno = LookupDataAccess.GetCardNo(dr1["SNP_SSNO"].ToString(), "1", string.Empty, string.Empty);
        //                    strSsno = strSsno.Replace("n", "x");
        //                    PrintRec("SSN# : " + strSsno, 130);
        //                    TmpDOB = time[0] = null;
        //                    TmpDOB = dr1["SNP_ALT_BDATE"].ToString();
        //                    time = Regex.Split(TmpDOB.ToString(), " ");
        //                    TmpDOB = time[0];
        //                    PrintRec("DOB: " + TmpDOB, 80);
        //                    Prev_fam_Seq = dr1["INCOME_FAMILY_SEQ"].ToString().Trim();
        //                    prev_income_Seq = null; Priv_Income_Type = null; Tot = decimal.Parse("0.00");
        //                    Y_Pos -= 15;
        //                    CheckBottomBorderReached(document, writer);
        //                    isfalse = false;
        //                }
        //                X_Pos = 50; cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ROMAN).BaseFont, 11);
        //                if (Prev_fam_Seq + prev_income_Seq != dr1["INCOME_FAMILY_SEQ"].ToString().Trim() + dr1["INCOME_SEQ"].ToString().Trim())
        //                {
        //                    X_Pos = 50;
        //                    cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ROMAN).BaseFont, 11);
        //                    if (Priv_Income_Type != dr1["INCOME_TYPE"].ToString().Trim())
        //                    {
        //                        Y_Pos -= 15;
        //                        CheckBottomBorderReached(document, writer);
        //                        foreach (AgyTabEntity agyEntity in lookUpIncomeTypes)
        //                        {
        //                            if (dr1["INCOME_TYPE"].ToString().Trim() == agyEntity.agycode)
        //                            {
        //                                PrintRec(agyEntity.agydesc, 70);
        //                                if (dtIncType.Rows.Count > 0)
        //                                {
        //                                    foreach (DataRow drInc in dtIncType.Rows)
        //                                    {
        //                                        if (agyEntity.agycode == drInc["AGY_2"].ToString().Trim())
        //                                            SORT_NEGATIVE = drInc["AGY_1"].ToString();
        //                                    }
        //                                }
        //                                break;
        //                            }
        //                        }
        //                        Priv_Income_Type = dr1["INCOME_TYPE"].ToString().Trim();
        //                        Tot = decimal.Parse("0.00");
        //                    }
        //                    else
        //                    {
        //                        //Y_Pos -= 5;
        //                        //CheckBottomBorderReached(document, writer);
        //                        PrintRec("", 70);
        //                    }

        //                    PrintRec(dr1["INCOME_SOURCE"].ToString(), 140);
        //                    foreach (CommonEntity interval in commonEntity)
        //                    {
        //                        if (dr1["INCOME_INTERVAL"].ToString().Trim() == interval.Code)
        //                        {
        //                            if (Fam_Sqe == dr1["INCOME_FAMILY_SEQ"].ToString().Trim())
        //                                PrintRec(interval.Desc, 80);
        //                            else
        //                                PrintRec(interval.Desc, 80);
        //                            break;
        //                        }
        //                    }

        //                    //if (Fam_Sqe == dr1["INCOME_FAMILY_SEQ"].ToString().Trim()) // for Applicant only Dont Delete 
        //                    //    PrintRec(IntakeDate, 100);

        //                    prev_income_Seq = dr1["INCOME_SEQ"].ToString().Trim();
        //                    //PrintRec(dr1["INCOME_VERIFIER"].ToString(), 50);
        //                }
        //                bool isLoop = true;
        //                for (i = 1; i < 6; i++)
        //                {
        //                    string Tmp = null;
        //                    Tmp = dr1["INCOME_VAL" + i.ToString()].ToString();
        //                    if (!string.IsNullOrEmpty(dr1["INCOME_VAL" + i.ToString()].ToString()))
        //                    {
        //                        if ((float.Parse(dr1["INCOME_VAL" + i.ToString()].ToString()) > 0)) //|| (dr1["INCOME_FAMILY_SEQ"].ToString().Trim() == Fam_Sqe && dr1["INCOME_TYPE"].ToString().Trim() == "X"))
        //                        {
        //                            if (dr1["INCOME_EXCLUDE"].ToString().Trim() != "Y")
        //                            {
        //                                PrintRec(LookupDataAccess.Getdate(dr1["INCOME_DATE" + i.ToString()].ToString().Trim()), 75);
        //                                PrintRec("$ " + dr1["INCOME_VAL" + i.ToString()].ToString(), 50);
        //                                Tot += decimal.Parse(dr1["INCOME_VAL" + i.ToString()].ToString());
        //                                //if (dr1["INCOME_TYPE"].ToString().Trim() == "X")
        //                                //    i = 6;
        //                                CalculateEMS30dayIncome(dr1["INCOME_INTERVAL"].ToString().Trim(), decimal.Parse(dr1["INCOME_VAL" + i.ToString()].ToString()), SORT_NEGATIVE);
        //                            }
        //                            else
        //                            {
        //                                PrintRec("", 75);
        //                                //PrintRec(LookupDataAccess.Getdate(dr1["INCOME_DATE" + i.ToString()].ToString().Trim()), 75);
        //                                PrintRec("$  0.00   (exmp " + dr1["INCOME_VAL" + i.ToString()].ToString() + " )", 50);
        //                                if (i == 1)
        //                                {
        //                                    Y_Pos -= 14; //X_Pos = 50;
        //                                    CheckBottomBorderReached(document, writer); X_Pos = 50;
        //                                    cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ROMAN).BaseFont, 11);
        //                                    PrintRec("EXCLUDE", 100);
        //                                }
        //                                CalculateEMS30dayIncome(dr1["INCOME_INTERVAL"].ToString().Trim(), decimal.Parse("0.00"), SORT_NEGATIVE);
        //                            }

        //                            PrintRec(dr1["INCOME_HOW_VERIFIED"].ToString(), 60);
        //                            Y_Pos -= 14;
        //                            CheckBottomBorderReached(document, writer);
        //                            X_Pos = 340;//360
        //                            cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ROMAN).BaseFont, 11);
        //                        }
        //                        else if ((dr1["INCOME_FAMILY_SEQ"].ToString().Trim() == Fam_Sqe))
        //                        {
        //                            if (float.Parse(dr1["INCOME_VAL1"].ToString()) == 0 && float.Parse(dr1["INCOME_VAL2"].ToString()) == 0 && float.Parse(dr1["INCOME_VAL3"].ToString()) == 0 &&
        //                                float.Parse(dr1["INCOME_VAL4"].ToString()) == 0 && float.Parse(dr1["INCOME_VAL5"].ToString()) == 0)
        //                            {
        //                                PrintRec(LookupDataAccess.Getdate(dr1["INCOME_DATE" + i.ToString()].ToString().Trim()), 75);
        //                                PrintRec("$ " + dr1["INCOME_VAL" + i.ToString()].ToString(), 50);
        //                                Tot += decimal.Parse(dr1["INCOME_VAL" + i.ToString()].ToString());
        //                                PrintRec(dr1["INCOME_HOW_VERIFIED"].ToString(), 60);
        //                                i = 6;
        //                                Y_Pos -= 14; CheckBottomBorderReached(document, writer);
        //                                CalculateEMS30dayIncome(dr1["INCOME_INTERVAL"].ToString().Trim(), decimal.Parse(dr1["INCOME_VAL" + i.ToString()].ToString()), SORT_NEGATIVE);
        //                            }
        //                            else if (float.Parse(dr1["INCOME_VAL" + i.ToString()].ToString()) == 0)
        //                            {
        //                                if (dr1["INCOME_EXCLUDE"].ToString().Trim() != "Y")
        //                                {
        //                                    if (i > 1)
        //                                        i = 6;
        //                                    else
        //                                    {
        //                                        PrintRec(LookupDataAccess.Getdate(dr1["INCOME_DATE" + i.ToString()].ToString().Trim()), 75);
        //                                        PrintRec("$ " + dr1["INCOME_VAL" + i.ToString()].ToString(), 50);
        //                                        Tot += decimal.Parse(dr1["INCOME_VAL" + i.ToString()].ToString());
        //                                        PrintRec(dr1["INCOME_HOW_VERIFIED"].ToString(), 60);
        //                                        //if (dr1["INCOME_TYPE"].ToString().Trim() == "X")
        //                                        i = 6;
        //                                        Y_Pos -= 14; CheckBottomBorderReached(document, writer);
        //                                        CalculateEMS30dayIncome(dr1["INCOME_INTERVAL"].ToString().Trim(), decimal.Parse(dr1["INCOME_VAL" + i.ToString()].ToString()), SORT_NEGATIVE);
        //                                    }
        //                                }
        //                            }

        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (isLoop && i == 1)
        //                        {
        //                            PrintRec("", 75);
        //                            PrintRec("", 50);
        //                            PrintRec(dr1["INCOME_HOW_VERIFIED"].ToString(), 60);
        //                            if (dt.Rows.Count == Count)
        //                                i = 6;
        //                            Y_Pos -= 14; CheckBottomBorderReached(document, writer);
        //                            isLoop = false;
        //                        }
        //                    }
        //                }
        //                Y_Pos += 14; CheckBottomBorderReached(document, writer);
        //                if (Priv_Income_Type != dr1["INCOME_TYPE"].ToString().Trim() || dt.Rows.Count == Count)
        //                {
        //                    Grand_Total += Tot;
        //                    Y_Pos -= 6; X_Pos = 415;//X_Pos = 435;
        //                    cb.MoveTo(X_Pos, Y_Pos);
        //                    cb.LineTo(X_Pos + 50, Y_Pos);
        //                    cb.SetLineWidth(0.5f);
        //                    cb.Stroke();
        //                    Y_Pos -= 10; X_Pos = 415;// X_Pos = 435;
        //                    CheckBottomBorderReached(document, writer);
        //                    cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_BOLD).BaseFont, 11);
        //                    PrintRec("$ " + Tot.ToString(), 20);
        //                }

        //            }
        //        }
        //        else
        //        {
        //            CaseSnpEntity snpEntity = BaseForm.BaseCaseSnpEntity.Find(u => u.FamilySeq.Equals(Fam_Sqe));

        //            X_Pos = 50; Y_Pos -= 10;
        //            CheckBottomBorderReached(document, writer);
        //            cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ITALIC).BaseFont, 12);
        //            cb.SetColorFill(BaseColor.BLUE);
        //            PrintRec(BaseForm.BaseApplicationName.Trim(), 150);
        //            cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ROMAN).BaseFont, 11);
        //            cb.SetCMYKColorFill(0, 0, 0, 255);

        //            string strSsno = LookupDataAccess.GetCardNo(snpEntity.Ssno.Trim(), "1", string.Empty, string.Empty);
        //            strSsno = strSsno.Replace("n", "x");
        //            PrintRec("SSN# : " + strSsno, 130);
        //            TmpDOB = time[0] = null;
        //            TmpDOB = LookupDataAccess.Getdate(snpEntity.AltBdate);
        //            //time = Regex.Split(TmpDOB.ToString(), " ");
        //            //TmpDOB = time[0];
        //            PrintRec("DOB: " + TmpDOB, 80);
        //            //Prev_fam_Seq = dr1["INCOME_FAMILY_SEQ"].ToString().Trim();
        //            //prev_income_Seq = null;
        //            Y_Pos -= 15;
        //            CheckBottomBorderReached(document, writer);

        //            Y_Pos -= 25;
        //            CheckBottomBorderReached(document, writer); X_Pos = 50;
        //            cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ROMAN).BaseFont, 11);
        //            PrintRec("NONE", 70);
        //            PrintRec("NONE", 160);
        //            PrintRec("", 60); //80
        //            PrintRec(LookupDataAccess.Getdate(BaseForm.BaseCaseMstListEntity[0].IntakeDate.Trim()), 75); //100
        //            PrintRec("$ " + "0.00", 50);

        //        }


        //        Y_Pos -= 20; X_Pos = 405; //X_Pos = 425;
        //        CheckBottomBorderReached(document, writer);
        //        X_Pos = 405;
        //        cb.MoveTo(X_Pos, Y_Pos + 10);
        //        cb.LineTo(X_Pos + 70, Y_Pos + 10);
        //        cb.LineTo(X_Pos + 70, Y_Pos - 13);
        //        cb.LineTo(X_Pos, Y_Pos - 13);
        //        cb.SetLineWidth(1f);
        //        cb.SetColorStroke(BaseColor.GRAY.Brighter());
        //        cb.Stroke();
        //        Y_Pos -= 3;
        //        CheckBottomBorderReached(document, writer);
        //        cb.SetColorFill(BaseColor.GREEN.Darker());
        //        cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_BOLDITALIC).BaseFont, 12);
        //        if (Grand_Total == 0)
        //        {
        //            PrintRec("$ " + "0.00", 20);
        //            Mst_Poverty = "0";
        //        }
        //        else
        //            PrintRec("$ " + Grand_Total.ToString(), 20);
        //        X_Pos = 200;
        //        cb.SetCMYKColorFill(0, 0, 0, 255);
        //        //cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ITALIC).BaseFont, 11);
        //        PrintRec("Grand Total(all income recorded) = ", 10);

        //        cb.EndText();

        //        cb.BeginText();

        //        PrintFooterBox(document, writer);

        //        //cb.SetColorStroke(BaseColor.DARK_GRAY);
        //        cb.SetLineWidth(4.0f);
        //        cb.SetGrayStroke(0.7f);
        //        cb.MoveTo(15f, 25f);
        //        cb.LineTo(590f, 25f);
        //        cb.Stroke();

        //        cb.BeginText();
        //        Y_Pos = 10;
        //        X_Pos = 20;
        //        cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES).BaseFont, 10);
        //        cb.SetCMYKColorFill(255, 255, 0, 99);
        //        //cb.SetCMYKColorFill(0, 0, 0, 255);
        //        PrintRec(DateTime.Now.ToLocalTime().ToString(), 130);

        //        X_Pos = 250;
        //        //cb.SetCMYKColorFill(0, 0, 0, 255);
        //        PrintRec("USER-ID=" + BaseForm.UserID.ToString().Trim(), 150);

        //        //Y_Pos = 10;
        //        X_Pos = 550;
        //        PrintRec("Page:", 28);
        //        PrintRec(pageNumber.ToString(), 15);
        //        cb.SetCMYKColorFill(0, 0, 0, 255);
        //        cb.EndText();



        //    }
        //    catch (Exception ex) { document.Add(new Paragraph("Aborted due to Exception............................................... ")); }

        //    document.Close();
        //    fs.Close();
        //    fs.Dispose();


        //    //FrmViewer objfrm = new FrmViewer(PdfName);
        //    //objfrm.ShowDialog();

        //}

        private void BtnGenPdf_Click(object sender, EventArgs e)
        {
            string Random_Filename = null;
            PdfName = null;
            PdfName = AppKey.Substring(0, 10) + "_" + AppKey.Substring(10, 8);

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

                Random_Filename = PdfName + "_" + newFileName.Substring(0, length) + ".pdf";
            }


            if (!string.IsNullOrEmpty(Random_Filename))
                PdfName = Random_Filename;
            else
                PdfName += ".pdf";

            System.IO.FileStream fs = new FileStream(PdfName, FileMode.Create);

            Document document;
            document = new Document();

            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();

            //Image _image = iTextSharp.text.Image.GetInstance(Application.MapPath("~\\Resources\\images\\Capsystems_WaterMark.bmp"));
            //_image.SetAbsolutePosition(160, 310);
            //_image.RotationDegrees = 45;
            //_image.Rotate();
            //PdfGState _state = new PdfGState()
            //{
            //    FillOpacity = 0.2F,
            //    StrokeOpacity = 0.2F
            //};
            //cb = writer.DirectContentUnder;
            //cb.SaveState();
            //cb.SetGState(_state);                               //WaterMark*******
            //cb.AddImage(_image);
            //cb.RestoreState();

            cb = writer.DirectContent;
            try
            {
                //DataSet dsAgcycntl = Captain.DatabaseLayer.ADMNB001DB.ADMNB001_Browse_AGCYCNTL("00", null, null, null, null, null, null);
                //string ShortName = string.Empty;
                //if (dsAgcycntl != null && dsAgcycntl.Tables[0].Rows.Count > 0)
                //    ShortName = dsAgcycntl.Tables[0].Rows[0]["ACR_SHORT_NAME"].ToString().Trim();

                DataSet dsIncType = DatabaseLayer.ADMNB001DB.ADMNB001_GetaAgyTabList("00004");
                DataTable dtIncType = dsIncType.Tables[0];



                DataSet CaseMST = Captain.DatabaseLayer.CaseSnpData.GetCaseMST(AppKey.Substring(0, 2), AppKey.Substring(2, 2), AppKey.Substring(4, 2), AppKey.Substring(6, 4), AppKey.Substring(10, 8));
                DataTable dtCaseMst = CaseMST.Tables[0];
                DataRow drCaseMst = dtCaseMst.Rows[0];

                CaseVerEntity caseVerList = _model.CaseMstData.GetCaseveradpynd(AppKey.Substring(0, 2), AppKey.Substring(2, 2), AppKey.Substring(4, 2), AppKey.Substring(6, 4), AppKey.Substring(10, 8), string.Empty, string.Empty);

                string Fam_Sqe = drCaseMst["MST_FAMILY_SEQ"].ToString().Trim();
                IntakeDate = drCaseMst["MST_INTAKE_DATE"].ToString();
                Mst_Prog_Icn = decimal.Parse(drCaseMst["MST_PROG_INCOME"].ToString());
                Mst_Poverty = drCaseMst["MST_POVERTY"].ToString();
                if(!string.IsNullOrEmpty(Mst_Poverty.Trim()))
                {
                    if (BaseForm.BaseAgencyControlDetails.ACR_POV_WITH_DEC == "N") Mst_Poverty = Math.Abs((int) decimal.Parse(Mst_Poverty)).ToString();
                }
                MST_InProg= drCaseMst["MST_NO_INPROG"].ToString();
                time = Regex.Split(IntakeDate.ToString(), " ");
                IntakeDate = time[0];
                time = Regex.Split(IntakeDate.ToString(), " ");
                IntakeDate = time[0];


                PrintHeaderPage();
                //cb.BeginText();
                List<AgyTabEntity> lookUpIncomeTypes = _model.lookupDataAccess.GetIncomeTypes();
                string strHourlymode = "N"; propHourlyMode = "N";
                if (propAgencyControlDetails != null)
                {
                    if (propAgencyControlDetails.IncMethods == "2")
                    {
                        strHourlymode = "Y";
                        propHourlyMode = "Y";
                    }
                }
                List<CommonEntity> commonEntity = _model.lookupDataAccess.GetIncomeInterval(strHourlymode, propAgencyControlDetails.State);
                commonEntity.Add(new CommonEntity("3", "30 Days"));

                List<CaseIncomeEntity> caseIncomeList = _model.CaseMstData.GetCaseIncomeadpynf(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, string.Empty);
                DataSet ds = DatabaseLayer.ADMNB002DB.GET_INCOME_REPORT(AppKey.Substring(0, 10), AppKey.Substring(10, 8), Fam_Sqe);
                DataTable dt = ds.Tables[0];
                DataRow dr;
                if (dt.Rows.Count > 0)
                    dr = dt.Rows[0];

                if (strHourlymode == "Y")
                {
                    if (string.IsNullOrEmpty(drCaseMst["MST_INTAKE_DATE"].ToString()))// == "" || MstIntakeDate == null)
                    {
                        MstIntakeEndDate = DateTime.Now;
                        MstIntakeStartDate = MstIntakeEndDate.AddDays(-29);

                    }
                    else
                    {
                        if (BaseForm.BaseAgencyControlDetails.AgyShortName == "CACOST")
                        {
                            MstIntakeEndDate = Convert.ToDateTime(drCaseMst["MST_INTAKE_DATE"].ToString()).AddDays(-1);
                            MstIntakeStartDate = MstIntakeEndDate.AddDays(-29);
                        }
                        else
                        {
                            MstIntakeEndDate = Convert.ToDateTime(drCaseMst["MST_INTAKE_DATE"].ToString());//.AddDays(-1);
                            MstIntakeStartDate = MstIntakeEndDate.AddDays(-29);
                        }

                        //MstIntakeEndDate = Convert.ToDateTime(drCaseMst["MST_INTAKE_DATE"].ToString());
                        //MstIntakeStartDate = MstIntakeEndDate.AddDays(-29);
                    }
                }



                int Count = 0;
                string Prev_fam_Seq = null, prev_income_Seq = null; string Priv_Income_Type = string.Empty;
                Y_Pos = 650; bool isfalse = true; string SORT_NEGATIVE = string.Empty;

                int i;
                if (dt.Rows.Count > 0)
                {
                    DataView dv = new DataView(dt);
                    dv.Sort = "INCOME_FAMILY_SEQ,INCOME_TYPE,INCOME_INTERVAL";
                    dt = dv.ToTable();
                    DataTable IncomeTable = CreateTable();
                    if (strHourlymode == "Y")
                    {
                        Income_Check_For_CABA(dt, IncomeTable, dtIncType, commonEntity);
                    }
                    else
                    {
                        Income_Check_For_OTHER_CUSTOMERS(dt, IncomeTable, dtIncType, commonEntity);
                    }
                    cb.BeginText();
                    DataView dv1 = new DataView(IncomeTable);
                    dv1.Sort = "SORT_FAMILY_SEQ,SORT_INCOME_TYPE,SORT_INCOME_SOURCE,SORT_INCOME_DATE "; //SORT_INCOME_SEQ,
                    IncomeTable = dv1.ToTable();
                    if (IncomeTable.Rows.Count > 0)
                    {

                        string PrivInteval = string.Empty, PrivIncSrc = string.Empty; string PrivNeg = string.Empty, IncTypeSw = string.Empty;
                        foreach (DataRow dr1 in IncomeTable.Rows)
                        {
                            if (Prev_fam_Seq != dr1["SORT_FAMILY_SEQ"].ToString().Trim())
                                Priv_Income_Type = null;

                            if (!isfalse)
                            {
                                if (Priv_Income_Type != dr1["SORT_INCOME_TYPE"].ToString().Trim() || IncomeTable.Rows.Count == Count)
                                {
                                    Grand_Total += Tot;
                                    Y_Pos -= 6; X_Pos = 415;//X_Pos = 435;
                                    cb.MoveTo(X_Pos, Y_Pos);
                                    cb.LineTo(X_Pos + 50, Y_Pos);
                                    cb.SetLineWidth(0.5f);
                                    cb.Stroke();
                                    Y_Pos -= 10; X_Pos = 415;// X_Pos = 435;
                                    CheckBottomBorderReached(document, writer);
                                    cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_BOLD).BaseFont, 11);
                                    if (PrivNeg == "N")
                                    {
                                        X_Pos = 410; PrintRec("( ", 5);
                                        if (Tot < 0) Tot = Math.Abs(Tot);
                                        PrintRec("$ " + Tot.ToString() + " )", 20);
                                    }
                                    else
                                        PrintRec("$ " + Tot.ToString(), 20);
                                    PrivNeg = dr1["SORT_NEGATIVE"].ToString();
                                }
                            }

                            TmpDOB = null; Count++;
                            X_Pos = 50; Y_Pos -= 14;
                            CheckBottomBorderReached(document, writer);
                            if (Prev_fam_Seq != dr1["SORT_FAMILY_SEQ"].ToString().Trim())
                            {
                                if (Count > 1) { Y_Pos -= 10; CheckBottomBorderReached(document, writer); }
                                cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ITALIC).BaseFont, 11);
                                cb.SetColorFill(BaseColor.BLUE);
                                PrintRec(dr1["SORT_MEMBER_NAME"].ToString(), 150);
                                cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ROMAN).BaseFont, 10);
                                cb.SetCMYKColorFill(0, 0, 0, 255);
                                string strSsno = LookupDataAccess.GetCardNo(dr1["SORT_MEMBER_SSN"].ToString(), "1", string.Empty, string.Empty);
                                strSsno = strSsno.Replace("n", "x");
                                PrintRec("SSN# : " + strSsno, 130);
                                TmpDOB = time[0] = null;
                                TmpDOB = dr1["SORT_MEMBER_DOB"].ToString();
                                time = Regex.Split(TmpDOB.ToString(), " ");
                                TmpDOB = time[0];
                                PrintRec("DOB: " + TmpDOB, 80);
                                Prev_fam_Seq = dr1["SORT_FAMILY_SEQ"].ToString().Trim();
                                prev_income_Seq = null; Priv_Income_Type = null; Tot = decimal.Parse("0.00");
                                Y_Pos -= 15;
                                CheckBottomBorderReached(document, writer);
                                isfalse = false;
                            }
                            X_Pos = 50; cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ROMAN).BaseFont, 10);
                            X_Pos = 50;
                            cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ROMAN).BaseFont, 10);
                            if (Priv_Income_Type != dr1["SORT_INCOME_TYPE"].ToString().Trim())
                            {
                                Y_Pos -= 15;
                                CheckBottomBorderReached(document, writer);
                                PrintRec(dr1["SORT_INCOME_TYPE_LIT"].ToString(), 70);
                                Priv_Income_Type = dr1["SORT_INCOME_TYPE"].ToString().Trim();
                                PrivInteval = string.Empty; PrivIncSrc = string.Empty;
                                PrivNeg = dr1["SORT_NEGATIVE"].ToString();
                                Tot = decimal.Parse("0.00");
                                IncTypeSw = "Y";
                            }
                            else
                            {
                                PrintRec("", 70);
                                IncTypeSw = "N";
                            }
                            if (PrivIncSrc != dr1["SORT_INCOME_SOURCE"].ToString())
                            {
                                PrintRec(dr1["SORT_INCOME_SOURCE"].ToString(), 140);
                                PrivIncSrc = dr1["SORT_INCOME_SOURCE"].ToString();
                            }
                            else
                            {
                                PrintRec("", 140);
                            }

                            if (PrivInteval != dr1["SORT_INTERVAL"].ToString())
                            {
                                PrintRec(dr1["SORT_INTERVAL"].ToString(), 80);
                                PrivInteval = dr1["SORT_INTERVAL"].ToString();
                            }
                            else
                            {
                                PrintRec("", 80);
                            }

                            if (dr1["SORT_EXCLUDE_INCOME"].ToString().Trim() != "Y")
                            {
                                PrintRec(LookupDataAccess.Getdate(dr1["SORT_INCOME_DATE"].ToString().Trim()), 75);

                                if (dr1["SORT_NEGATIVE"].ToString() == "N")
                                {
                                    X_Pos -= 5;
                                    PrintRec("( $ " + dr1["SORT_INCOME_VAL"].ToString() + " )", 50);
                                    Tot -= decimal.Parse(dr1["SORT_INCOME_VAL"].ToString());
                                }
                                else
                                {
                                    PrintRec("$ " + dr1["SORT_INCOME_VAL"].ToString(), 50);
                                    Tot += decimal.Parse(dr1["SORT_INCOME_VAL"].ToString());
                                }

                                PrintRec(dr1["SORT_INCOME_VER"].ToString(), 60);
                            }
                            else
                            {
                                PrintRec("", 75);
                                PrintRec("$ " + dr1["SORT_INCOME_VAL"].ToString(), 50);
                                PrintRec(dr1["SORT_INCOME_VER"].ToString(), 60);
                                Y_Pos -= 14; //X_Pos = 50;
                                if (IncTypeSw == "Y")
                                {
                                    CheckBottomBorderReached(document, writer); X_Pos = 50;
                                    cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_BOLD).BaseFont, 10);
                                    PrintRec("EXCLUDE", 100);
                                }
                            }
                            CalculateEMS30dayIncome(dr1["SORT_INTERVAL"].ToString().Trim(), decimal.Parse(dr1["SORT_INCOME_VAL"].ToString()), dr1["SORT_NEGATIVE"].ToString());

                            Y_Pos -= 14;
                            CheckBottomBorderReached(document, writer);
                            X_Pos = 340;//360
                            cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ROMAN).BaseFont, 10);

                            Y_Pos += 14; CheckBottomBorderReached(document, writer);
                            if (Priv_Income_Type != dr1["SORT_INCOME_TYPE"].ToString().Trim() || IncomeTable.Rows.Count == Count)
                            {
                                Grand_Total += Tot;
                                Y_Pos -= 6; X_Pos = 415;//X_Pos = 435;
                                cb.MoveTo(X_Pos, Y_Pos);
                                cb.LineTo(X_Pos + 50, Y_Pos);
                                cb.SetLineWidth(0.5f);
                                cb.Stroke();
                                Y_Pos -= 10; X_Pos = 415;// X_Pos = 435;
                                CheckBottomBorderReached(document, writer);
                                cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_BOLD).BaseFont, 10);
                                if (PrivNeg == "N")
                                {
                                    X_Pos = 410; PrintRec("( ", 5);
                                    if (Tot < 0) Tot = Math.Abs(Tot);
                                    PrintRec("$ " + Tot.ToString() + " )", 20);
                                }
                                else
                                    PrintRec("$ " + Tot.ToString(), 20);
                                PrivNeg = dr1["SORT_NEGATIVE"].ToString();
                            }
                        }
                    }
                    else
                    {
                        CaseSnpEntity snpEntity = BaseForm.BaseCaseSnpEntity.Find(u => u.FamilySeq.Equals(Fam_Sqe));

                        X_Pos = 50; Y_Pos -= 10;
                        CheckBottomBorderReached(document, writer);
                        cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ITALIC).BaseFont, 11);
                        cb.SetColorFill(BaseColor.BLUE);
                        PrintRec(BaseForm.BaseApplicationName.Trim(), 150);
                        cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ROMAN).BaseFont, 10);
                        cb.SetCMYKColorFill(0, 0, 0, 255);

                        string strSsno = LookupDataAccess.GetCardNo(snpEntity.Ssno.Trim(), "1", string.Empty, string.Empty);
                        strSsno = strSsno.Replace("n", "x");
                        PrintRec("SSN# : " + strSsno, 130);
                        TmpDOB = time[0] = null;
                        TmpDOB = LookupDataAccess.Getdate(snpEntity.AltBdate);
                        //time = Regex.Split(TmpDOB.ToString(), " ");
                        //TmpDOB = time[0];
                        PrintRec("DOB: " + TmpDOB, 80);
                        //Prev_fam_Seq = dr1["INCOME_FAMILY_SEQ"].ToString().Trim();
                        //prev_income_Seq = null;
                        Y_Pos -= 15;
                        CheckBottomBorderReached(document, writer);

                        Y_Pos -= 25;
                        CheckBottomBorderReached(document, writer); X_Pos = 50;
                        cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ROMAN).BaseFont, 10);
                        PrintRec("NONE", 70);
                        PrintRec("NONE", 160);
                        PrintRec("", 60); //80
                        PrintRec(LookupDataAccess.Getdate(BaseForm.BaseCaseMstListEntity[0].IntakeDate.Trim()), 75); //100
                        PrintRec("$ " + "0.00", 50);
                        PrintRec("", 60);

                        Y_Pos -= 6; X_Pos = 415;//X_Pos = 435;
                        cb.MoveTo(X_Pos, Y_Pos);
                        cb.LineTo(X_Pos + 50, Y_Pos);
                        cb.SetLineWidth(0.5f);
                        cb.Stroke();
                        Y_Pos -= 10; X_Pos = 415;// X_Pos = 435;
                        CheckBottomBorderReached(document, writer);
                        cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_BOLD).BaseFont, 10);
                        PrintRec("$ " + "0.00", 20);

                    }


                    //Y_Pos -= 20; X_Pos = 405; //X_Pos = 425;
                    //CheckBottomBorderReached(document, writer);
                    //X_Pos = 405;
                    //cb.MoveTo(X_Pos, Y_Pos + 10);
                    //cb.LineTo(X_Pos + 70, Y_Pos + 10);
                    //cb.LineTo(X_Pos + 70, Y_Pos - 13);
                    //cb.LineTo(X_Pos, Y_Pos - 13);
                    //cb.SetLineWidth(1f);
                    //cb.SetColorStroke(BaseColor.GRAY.Brighter());
                    //cb.Stroke();
                    //Y_Pos -= 3;
                    //CheckBottomBorderReached(document, writer);
                    //cb.SetColorFill(BaseColor.GREEN.Darker());
                    //cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_BOLDITALIC).BaseFont, 12);
                    //if (Grand_Total == 0)
                    //{
                    //    PrintRec("$ " + "0.00", 20);
                    //    Mst_Poverty = "0";
                    //}
                    //else
                    //{
                    //    if (Grand_Total < 0)
                    //    {
                    //        X_Pos -= 5; Grand_Total = Math.Abs(Grand_Total);
                    //        PrintRec("( $ " + Grand_Total.ToString() + " )", 20);
                    //    }
                    //    else
                    //        PrintRec("$ " + Grand_Total.ToString(), 20);
                    //}
                    //X_Pos = 200;
                    //cb.SetCMYKColorFill(0, 0, 0, 255);
                    ////cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ITALIC).BaseFont, 11);
                    //PrintRec("Grand Total(all income recorded) = ", 10);

                    //cb.EndText();

                    //cb.BeginText();

                    //PrintFooterBox(document, writer);

                    ////cb.SetColorStroke(BaseColor.DARK_GRAY);
                    //cb.SetLineWidth(4.0f);
                    //cb.SetGrayStroke(0.7f);
                    //cb.MoveTo(15f, 25f);
                    //cb.LineTo(590f, 25f);
                    //cb.Stroke();

                    //cb.BeginText();
                    //Y_Pos = 10;
                    //X_Pos = 20;
                    //cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES).BaseFont, 10);
                    //cb.SetCMYKColorFill(255, 255, 0, 99);
                    ////cb.SetCMYKColorFill(0, 0, 0, 255);
                    //PrintRec(DateTime.Now.ToLocalTime().ToString(), 130);

                    //X_Pos = 250;
                    ////cb.SetCMYKColorFill(0, 0, 0, 255);
                    //PrintRec("USER-ID=" + BaseForm.UserID.ToString().Trim(), 150);

                    ////Y_Pos = 10;
                    //X_Pos = 550;
                    //PrintRec("Page:", 28);
                    //PrintRec(pageNumber.ToString(), 15);
                    //cb.SetCMYKColorFill(0, 0, 0, 255);
                    //cb.EndText();

                }
                else
                {
                    cb.BeginText();

                    //CaseSnpEntity snpEntity = BaseForm.BaseCaseSnpEntity.Find(u => u.FamilySeq.Equals(Fam_Sqe));
                    if (BaseForm.BaseCaseSnpEntity.Count > 0)
                    {
                        X_Pos = 50; Y_Pos -= 10;
                        foreach (CaseSnpEntity snpEntity in BaseForm.BaseCaseSnpEntity)
                        {
                            Count++;
                            if (Count > 1) { Y_Pos -= 10; CheckBottomBorderReached(document, writer); X_Pos = 50; }
                            CheckBottomBorderReached(document, writer);
                            cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ITALIC).BaseFont, 11);
                            cb.SetColorFill(BaseColor.BLUE);
                            PrintRec(LookupDataAccess.GetMemberName(snpEntity.NameixFi, snpEntity.NameixMi, snpEntity.NameixLast, BaseForm.BaseHierarchyCnFormat), 150);
                            cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ROMAN).BaseFont, 10);
                            cb.SetCMYKColorFill(0, 0, 0, 255);

                            string strSsno = LookupDataAccess.GetCardNo(snpEntity.Ssno.Trim(), "1", string.Empty, string.Empty);
                            strSsno = strSsno.Replace("n", "x");
                            PrintRec("SSN# : " + strSsno, 130);
                            TmpDOB = time[0] = null;
                            TmpDOB = LookupDataAccess.Getdate(snpEntity.AltBdate);
                            //time = Regex.Split(TmpDOB.ToString(), " ");
                            //TmpDOB = time[0];
                            PrintRec("DOB: " + TmpDOB, 80);
                            //Prev_fam_Seq = dr1["INCOME_FAMILY_SEQ"].ToString().Trim();
                            //prev_income_Seq = null;
                            Y_Pos -= 15;
                            CheckBottomBorderReached(document, writer);

                            Y_Pos -= 25;
                            CheckBottomBorderReached(document, writer); X_Pos = 50;
                            cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ROMAN).BaseFont, 10);
                            PrintRec("NONE", 70);
                            PrintRec("NONE", 140);
                            PrintRec("", 80); //80
                            PrintRec(LookupDataAccess.Getdate(BaseForm.BaseCaseMstListEntity[0].IntakeDate.Trim()), 75); //100
                            PrintRec("$ " + "0.00", 50);
                            PrintRec("", 60);


                            Y_Pos -= 6; X_Pos = 415;//X_Pos = 435;
                            cb.MoveTo(X_Pos, Y_Pos);
                            cb.LineTo(X_Pos + 50, Y_Pos);
                            cb.SetLineWidth(0.5f);
                            cb.Stroke();
                            Y_Pos -= 10; X_Pos = 415;// X_Pos = 435;
                            CheckBottomBorderReached(document, writer);
                            cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_BOLD).BaseFont, 10);
                            PrintRec("$ " + "0.00", 20);
                            Y_Pos -= 14;

                        }
                    }




                }

                Y_Pos -= 20; X_Pos = 405; //X_Pos = 425;
                CheckBottomBorderReached(document, writer);
                X_Pos = 405;
                cb.MoveTo(X_Pos, Y_Pos + 10);
                cb.LineTo(X_Pos + 70, Y_Pos + 10);
                cb.LineTo(X_Pos + 70, Y_Pos - 13);
                cb.LineTo(X_Pos, Y_Pos - 13);
                cb.SetLineWidth(1f);
                cb.SetColorStroke(BaseColor.GRAY.Brighter());
                cb.Stroke();
                Y_Pos -= 3;
                CheckBottomBorderReached(document, writer);
                cb.SetColorFill(BaseColor.GREEN.Darker());
                cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_BOLDITALIC).BaseFont, 12);
                if (Grand_Total == 0)
                {
                    PrintRec("$ " + "0.00", 20);
                    //Mst_Poverty = "0";
                }
                else
                {
                    if (Grand_Total < 0)
                    {
                        X_Pos -= 5; Grand_Total = Math.Abs(Grand_Total);
                        PrintRec("( $ " + Grand_Total.ToString() + " )", 20);
                    }
                    else
                        PrintRec("$ " + Grand_Total.ToString(), 20);
                }
                X_Pos = 200;
                cb.SetCMYKColorFill(0, 0, 0, 255);
                //cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ITALIC).BaseFont, 11);
                PrintRec("Grand Total(all income recorded) = ", 10);

                cb.EndText();

                cb.BeginText();

                PrintFooterBox(document, writer, caseVerList);

                //cb.SetColorStroke(BaseColor.DARK_GRAY);
                cb.SetLineWidth(4.0f);
                cb.SetGrayStroke(0.7f);
                cb.MoveTo(15f, 25f);
                cb.LineTo(590f, 25f);
                cb.Stroke();

                cb.BeginText();
                Y_Pos = 10;
                X_Pos = 20;
                cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES).BaseFont, 10);
                cb.SetCMYKColorFill(255, 255, 0, 99);
                //cb.SetCMYKColorFill(0, 0, 0, 255);
                PrintRec(DateTime.Now.ToLocalTime().ToString(), 130);

                X_Pos = 250;
                //cb.SetCMYKColorFill(0, 0, 0, 255);
                PrintRec("USER-ID=" + BaseForm.UserID.ToString().Trim(), 150);

                //Y_Pos = 10;
                X_Pos = 550;
                PrintRec("Page:", 28);
                PrintRec(pageNumber.ToString(), 15);
                cb.SetCMYKColorFill(0, 0, 0, 255);
                cb.EndText();

            }
            catch (Exception ex) { document.Add(new Paragraph("Aborted due to Exception............................................... ")); }

            document.Close();
            fs.Close();
            fs.Dispose();


            //FrmViewer objfrm = new FrmViewer(PdfName);
            //objfrm.ShowDialog();

        }

        private void BtnGenPdf1_Click(object sender, EventArgs e)
        {
            string Random_Filename = null;
            PdfName = null;
            PdfName = AppKey.Substring(0, 10) + "_" + AppKey.Substring(10, 8);

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

                Random_Filename = PdfName + "_" + newFileName.Substring(0, length) + ".pdf";
            }


            if (!string.IsNullOrEmpty(Random_Filename))
                PdfName = Random_Filename;
            else
                PdfName += ".pdf";

            System.IO.FileStream fs = new FileStream(PdfName, FileMode.Create);

            Document document;
            document = new Document();

            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();

            //byte[] imgsrc = 

            // result is a Bitmap binary image
            //iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance()

            //Image _image = iTextSharp.text.Image.GetInstance(Application.MapPath("~\\Resources\\images\\Capsystems_WaterMark.bmp"));
            //_image.SetAbsolutePosition(160, 310);
            //_image.RotationDegrees = 45;
            //_image.Rotate();
            //PdfGState _state = new PdfGState()
            //{
            //    FillOpacity = 0.2F,
            //    StrokeOpacity = 0.2F
            //};
            //cb = writer.DirectContentUnder;
            //cb.SaveState();
            //cb.SetGState(_state);                               //WaterMark*******
            //cb.AddImage(_image);
            //cb.RestoreState();

            cb = writer.DirectContent;
            try
            {
                //DataSet dsAgcycntl = Captain.DatabaseLayer.ADMNB001DB.ADMNB001_Browse_AGCYCNTL("00", null, null, null, null, null, null);
                //string ShortName = string.Empty;
                //if (dsAgcycntl != null && dsAgcycntl.Tables[0].Rows.Count > 0)
                //    ShortName = dsAgcycntl.Tables[0].Rows[0]["ACR_SHORT_NAME"].ToString().Trim();

                DataSet dsIncType = DatabaseLayer.ADMNB001DB.ADMNB001_GetaAgyTabList("00004");
                DataTable dtIncType = dsIncType.Tables[0];
                string strYear = "    ";
                if (!string.IsNullOrEmpty(BaseForm.BaseYear))
                {
                    strYear = BaseForm.BaseYear;
                }


                //DataSet dsimages= DatabaseLayer.ADMNB001DB.GetBinaryImages(string.Empty,BaseForm.BaseAgency.ToString()+BaseForm.BaseDept.ToString()+BaseForm.BaseProg.ToString()+ strYear + BaseForm.BaseApplicationNo.ToString());

                ////string input = dsimages.Tables[0].Rows[0]["ImgData"].ToString().Trim();
                //if (dsimages.Tables[0].Rows.Count > 0)
                //{
                //    byte[] array = (byte[])dsimages.Tables[0].Rows[0]["ImgData"]; //Encoding.Unicode.GetBytes(input);


                //    //string inputBytes = Convert.ToBase64String(array);
                //    ////iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(inputBytes);
                //    //byte[] imageBytes = Convert.FromBase64String(inputBytes);
                //    //iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(imageBytes);
                //    ////image.SetAbsolutePosition(60, 60);

                //    //System.Drawing.Image imgs= byteArrayToImage(array);
                //    iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(array);
                //    image.ScaleToFit(100f, 60f);
                //    image.SetAbsolutePosition(130, 80);
                //    cb.AddImage(image);
                //}

              

                DataSet CaseMST = Captain.DatabaseLayer.CaseSnpData.GetCaseMST(AppKey.Substring(0, 2), AppKey.Substring(2, 2), AppKey.Substring(4, 2), AppKey.Substring(6, 4), AppKey.Substring(10, 8));
                DataTable dtCaseMst = CaseMST.Tables[0];
                DataRow drCaseMst = dtCaseMst.Rows[0];

                CaseVerEntity caseVerList = _model.CaseMstData.GetCaseveradpynd(AppKey.Substring(0, 2), AppKey.Substring(2, 2), AppKey.Substring(4, 2), AppKey.Substring(6, 4), AppKey.Substring(10, 8), string.Empty, string.Empty);

                string Fam_Sqe = drCaseMst["MST_FAMILY_SEQ"].ToString().Trim();
                IntakeDate = drCaseMst["MST_INTAKE_DATE"].ToString();
                Mst_Prog_Icn = decimal.Parse(drCaseMst["MST_PROG_INCOME"].ToString());
                Mst_Poverty = drCaseMst["MST_POVERTY"].ToString();

                if (!string.IsNullOrEmpty(Mst_Poverty.Trim()))
                {
                    if (BaseForm.BaseAgencyControlDetails.ACR_POV_WITH_DEC == "N") Mst_Poverty = Math.Abs((int)decimal.Parse(Mst_Poverty)).ToString();
                }
                MST_InProg = drCaseMst["MST_NO_INPROG"].ToString();
                time = Regex.Split(IntakeDate.ToString(), " ");
                IntakeDate = time[0];
                time = Regex.Split(IntakeDate.ToString(), " ");
                IntakeDate = time[0];


                PrintHeaderPage();
                //cb.BeginText();
                List<AgyTabEntity> lookUpIncomeTypes = _model.lookupDataAccess.GetIncomeTypes();
                string strHourlymode = "N"; propHourlyMode = "N";
                if (propAgencyControlDetails != null)
                {
                    if (propAgencyControlDetails.IncMethods == "2")
                    {
                        strHourlymode = "Y";
                        propHourlyMode = "Y";
                    }
                }
                List<CommonEntity> commonEntity = _model.lookupDataAccess.GetIncomeInterval(strHourlymode, propAgencyControlDetails.State);
                commonEntity.Add(new CommonEntity("3", "30 Days"));

                List<CaseIncomeEntity> caseIncomeList = _model.CaseMstData.GetCaseIncomeadpynf(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, string.Empty);
                //DataSet ds = DatabaseLayer.ADMNB002DB.GET_INCOME_REPORT(AppKey.Substring(0, 10), AppKey.Substring(10, 8), Fam_Sqe);
                //DataTable dt = ds.Tables[0];
                //DataRow dr;
                //if (dt.Rows.Count > 0)
                //    dr = dt.Rows[0];

                if (strHourlymode == "Y")
                {
                    if (string.IsNullOrEmpty(drCaseMst["MST_INTAKE_DATE"].ToString()))// == "" || MstIntakeDate == null)
                    {
                        MstIntakeEndDate = DateTime.Now;
                        MstIntakeStartDate = MstIntakeEndDate.AddDays(-29);

                    }
                    else
                    {
                        if (BaseForm.BaseAgencyControlDetails.AgyShortName == "CACOST")
                        {
                            MstIntakeEndDate = Convert.ToDateTime(drCaseMst["MST_INTAKE_DATE"].ToString()).AddDays(-1);
                            MstIntakeStartDate = MstIntakeEndDate.AddDays(-29);
                        }
                        else
                        {
                            MstIntakeEndDate = Convert.ToDateTime(drCaseMst["MST_INTAKE_DATE"].ToString());//.AddDays(-1);
                            MstIntakeStartDate = MstIntakeEndDate.AddDays(-29);
                        }

                        //MstIntakeEndDate = Convert.ToDateTime(drCaseMst["MST_INTAKE_DATE"].ToString());
                        //MstIntakeStartDate = MstIntakeEndDate.AddDays(-29);
                    }
                }



                int Count = 0;
                string Prev_fam_Seq = null, prev_income_Seq = null; string Priv_Income_Type = string.Empty;
                Y_Pos = 650; bool isfalse = true; string SORT_NEGATIVE = string.Empty;

                int i;
                if (BaseForm.BaseCaseSnpEntity.Count > 0)
                {
                    //DataView dv = new DataView(dt);
                    //dv.Sort = "INCOME_FAMILY_SEQ,INCOME_TYPE,INCOME_INTERVAL";
                    //dt = dv.ToTable();
                    DataTable IncomeTable = CreateTable();
                    if (strHourlymode == "Y")
                    {
                        Income_Check_For_CABA1(caseIncomeList, IncomeTable, dtIncType, commonEntity);
                    }
                    else
                    {
                        Income_Check_For_OTHER_CUSTOMERS1(caseIncomeList, IncomeTable, dtIncType, commonEntity);
                    }
                    cb.BeginText();
                    DataView dv1 = new DataView(IncomeTable);
                    dv1.Sort = "SORT_FAMILY_SEQ ASC,SORT_INCOME_TYPE ASC,SORT_INCOME_SOURCE ASC,SORT_INCOME_SEQ ASC,SORT_INCOME_DATE ASC "; //SORT_INCOME_SOURCE,SORT_INCOME_SEQ
                    IncomeTable = dv1.ToTable();
                    if (IncomeTable.Rows.Count > 0)
                    {

                        string PrivInteval = string.Empty, PrivIncSrc = string.Empty; string PrivNeg = string.Empty, IncTypeSw = string.Empty;
                        foreach (DataRow dr1 in IncomeTable.Rows)
                        {
                            if (Prev_fam_Seq != dr1["SORT_FAMILY_SEQ"].ToString().Trim())
                                Priv_Income_Type = null;

                            if (!isfalse)
                            {
                                if (Priv_Income_Type != dr1["SORT_INCOME_TYPE"].ToString().Trim() || IncomeTable.Rows.Count == Count)
                                {
                                    Grand_Total += Tot;
                                    Y_Pos -= 6; X_Pos = 415;//X_Pos = 435;
                                    cb.EndText();
                                    cb.MoveTo(X_Pos, Y_Pos);
                                    cb.LineTo(X_Pos + 50, Y_Pos);
                                    cb.SetLineWidth(0.5f);
                                    cb.Stroke();
                                    cb.BeginText();
                                    Y_Pos -= 10; X_Pos = 415;// X_Pos = 435;
                                    CheckBottomBorderReached(document, writer);
                                    cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_BOLD).BaseFont, 11);
                                    if (PrivNeg == "N")
                                    {
                                        X_Pos = 410; PrintRec("( ", 5);
                                        if (Tot < 0) Tot = Math.Abs(Tot);
                                        PrintRec("$ " + Tot.ToString() + " )", 20);
                                    }
                                    else
                                        PrintRec("$ " + Tot.ToString(), 20);
                                    PrivNeg = dr1["SORT_NEGATIVE"].ToString();
                                }
                            }

                            TmpDOB = null; Count++;
                            X_Pos = 50; Y_Pos -= 14;
                            CheckBottomBorderReached(document, writer);
                            if (Prev_fam_Seq != dr1["SORT_FAMILY_SEQ"].ToString().Trim())
                            {
                                if (Count > 1) { Y_Pos -= 10; CheckBottomBorderReached(document, writer); }
                                cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ITALIC).BaseFont, 10);
                                cb.SetColorFill(BaseColor.BLUE);
                                PrintRec(dr1["SORT_MEMBER_NAME"].ToString(), 200);//150
                                cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ROMAN).BaseFont, 10);
                                cb.SetCMYKColorFill(0, 0, 0, 255);
                                string strSsno = LookupDataAccess.GetCardNo(dr1["SORT_MEMBER_SSN"].ToString(), "1", string.Empty, string.Empty);
                                strSsno = strSsno.Replace("n", "x");
                                PrintRec("SSN# : " + strSsno, 110);//130
                                TmpDOB = time[0] = null;
                                TmpDOB = dr1["SORT_MEMBER_DOB"].ToString();
                                time = Regex.Split(TmpDOB.ToString(), " ");
                                TmpDOB = time[0];
                                PrintRec("DOB: " + TmpDOB, 80);
                                Prev_fam_Seq = dr1["SORT_FAMILY_SEQ"].ToString().Trim();
                                prev_income_Seq = null; Priv_Income_Type = null; Tot = decimal.Parse("0.00");
                                Y_Pos -= 15;
                                CheckBottomBorderReached(document, writer);
                                isfalse = false;
                            }
                            X_Pos = 50; cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ROMAN).BaseFont, 10);
                            X_Pos = 50;
                            cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ROMAN).BaseFont, 10);
                            if (Priv_Income_Type != dr1["SORT_INCOME_TYPE"].ToString().Trim())
                            {
                                Y_Pos -= 15;
                                CheckBottomBorderReached(document, writer);
                                cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ROMAN).BaseFont, 10);
                                PrintRec(dr1["SORT_INCOME_TYPE_LIT"].ToString(), 105); //70
                                Priv_Income_Type = dr1["SORT_INCOME_TYPE"].ToString().Trim();
                                PrivInteval = string.Empty; PrivIncSrc = string.Empty;
                                PrivNeg = dr1["SORT_NEGATIVE"].ToString();
                                Tot = decimal.Parse("0.00");
                                IncTypeSw = "Y";
                            }
                            else
                            {
                                PrintRec("", 70);
                                IncTypeSw = "N";
                            }
                            if (PrivIncSrc != dr1["SORT_INCOME_SOURCE"].ToString())
                            {
                                PrintRec(dr1["SORT_INCOME_SOURCE"].ToString(), 120); //140
                                PrivIncSrc = dr1["SORT_INCOME_SOURCE"].ToString();
                            }
                            else
                            {
                                PrintRec("", 140);
                            }

                            if (PrivInteval != dr1["SORT_INTERVAL"].ToString())
                            {
                                PrintRec(dr1["SORT_INTERVAL"].ToString(), 65); //80
                                PrivInteval = dr1["SORT_INTERVAL"].ToString();
                            }
                            else
                            {
                                PrintRec("", 80);
                            }

                            if (dr1["SORT_EXCLUDE_INCOME"].ToString().Trim() != "Y")
                            {
                                PrintRec(LookupDataAccess.Getdate(dr1["SORT_INCOME_DATE"].ToString().Trim()), 75);

                                if (dr1["SORT_NEGATIVE"].ToString() == "N")
                                {
                                    X_Pos -= 5;
                                    PrintRec("( $ " + dr1["SORT_INCOME_VAL"].ToString() + " )", 50);
                                    if(!string.IsNullOrEmpty(dr1["SORT_INCOME_VAL"].ToString().Trim()))
                                        Tot -= decimal.Parse(dr1["SORT_INCOME_VAL"].ToString());
                                }
                                else
                                {
                                    PrintRec("$ " + dr1["SORT_INCOME_VAL"].ToString(), 50);
                                    if (!string.IsNullOrEmpty(dr1["SORT_INCOME_VAL"].ToString().Trim()))
                                        Tot += decimal.Parse(dr1["SORT_INCOME_VAL"].ToString());
                                }

                                PrintRec(dr1["SORT_INCOME_VER"].ToString(), 60);
                            }
                            else
                            {
                                PrintRec("", 75);
                                PrintRec("$ " + dr1["SORT_INCOME_VAL"].ToString(), 50);
                                PrintRec(dr1["SORT_INCOME_VER"].ToString(), 60);
                                Y_Pos -= 14; //X_Pos = 50;
                                if (IncTypeSw == "Y")
                                {
                                    CheckBottomBorderReached(document, writer); X_Pos = 50;
                                    cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_BOLD).BaseFont, 10);
                                    PrintRec("EXCLUDE", 100);
                                }
                            }
                            if (!string.IsNullOrEmpty(dr1["SORT_INCOME_VAL"].ToString().Trim()))
                                CalculateEMS30dayIncome(dr1["SORT_INTERVAL"].ToString().Trim(), decimal.Parse(dr1["SORT_INCOME_VAL"].ToString()), dr1["SORT_NEGATIVE"].ToString());

                            Y_Pos -= 14;
                            CheckBottomBorderReached(document, writer);
                            X_Pos = 340;//360
                            cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ROMAN).BaseFont, 10);

                            Y_Pos += 14; CheckBottomBorderReached(document, writer);
                            if (Priv_Income_Type != dr1["SORT_INCOME_TYPE"].ToString().Trim() || IncomeTable.Rows.Count == Count)
                            {
                                Grand_Total += Tot;
                                Y_Pos -= 6; X_Pos = 415;//X_Pos = 435;
                                cb.EndText();
                                cb.MoveTo(X_Pos, Y_Pos);
                                cb.LineTo(X_Pos + 50, Y_Pos);
                                cb.SetLineWidth(0.5f);
                                cb.Stroke();
                                cb.BeginText();
                                Y_Pos -= 10; X_Pos = 415;// X_Pos = 435;
                                CheckBottomBorderReached(document, writer);
                                cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_BOLD).BaseFont, 10);
                                if (PrivNeg == "N")
                                {
                                    X_Pos = 410; PrintRec("( ", 5);
                                    if (Tot < 0) Tot = Math.Abs(Tot);
                                    PrintRec("$ " + Tot.ToString() + " )", 20);
                                }
                                else
                                    PrintRec("$ " + Tot.ToString(), 20);
                                PrivNeg = dr1["SORT_NEGATIVE"].ToString();
                            }
                        }
                    }
                    else
                    {
                        CaseSnpEntity snpEntity = BaseForm.BaseCaseSnpEntity.Find(u => u.FamilySeq.Equals(Fam_Sqe));

                        X_Pos = 50; Y_Pos -= 10;
                        CheckBottomBorderReached(document, writer);
                        cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ITALIC).BaseFont, 11);
                        cb.SetColorFill(BaseColor.BLUE);
                        PrintRec(BaseForm.BaseApplicationName.Trim(), 150);
                        cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ROMAN).BaseFont, 10);
                        cb.SetCMYKColorFill(0, 0, 0, 255);

                        string strSsno = LookupDataAccess.GetCardNo(snpEntity.Ssno.Trim(), "1", string.Empty, string.Empty);
                        strSsno = strSsno.Replace("n", "x");
                        PrintRec("SSN# : " + strSsno, 130);
                        TmpDOB = time[0] = null;
                        TmpDOB = LookupDataAccess.Getdate(snpEntity.AltBdate);
                        //time = Regex.Split(TmpDOB.ToString(), " ");
                        //TmpDOB = time[0];
                        PrintRec("DOB: " + TmpDOB, 80);
                        //Prev_fam_Seq = dr1["INCOME_FAMILY_SEQ"].ToString().Trim();
                        //prev_income_Seq = null;
                        Y_Pos -= 15;
                        CheckBottomBorderReached(document, writer);

                        Y_Pos -= 25;
                        CheckBottomBorderReached(document, writer); X_Pos = 50;
                        cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ROMAN).BaseFont, 10);
                        PrintRec("NONE", 70);
                        PrintRec("NONE", 160);
                        PrintRec("", 60); //80
                        PrintRec(LookupDataAccess.Getdate(BaseForm.BaseCaseMstListEntity[0].IntakeDate.Trim()), 75); //100
                        PrintRec("$ " + "0.00", 50);
                        PrintRec("", 60);

                        Y_Pos -= 6; X_Pos = 415;//X_Pos = 435;
                        cb.EndText();
                        cb.MoveTo(X_Pos, Y_Pos);
                        cb.LineTo(X_Pos + 50, Y_Pos);
                        cb.SetLineWidth(0.5f);
                        cb.Stroke();
                        cb.BeginText();
                        Y_Pos -= 10; X_Pos = 415;// X_Pos = 435;
                        CheckBottomBorderReached(document, writer);
                        cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_BOLD).BaseFont, 10);
                        PrintRec("$ " + "0.00", 20);

                    }


                    //Y_Pos -= 20; X_Pos = 405; //X_Pos = 425;
                    //CheckBottomBorderReached(document, writer);
                    //X_Pos = 405;
                    //cb.MoveTo(X_Pos, Y_Pos + 10);
                    //cb.LineTo(X_Pos + 70, Y_Pos + 10);
                    //cb.LineTo(X_Pos + 70, Y_Pos - 13);
                    //cb.LineTo(X_Pos, Y_Pos - 13);
                    //cb.SetLineWidth(1f);
                    //cb.SetColorStroke(BaseColor.GRAY.Brighter());
                    //cb.Stroke();
                    //Y_Pos -= 3;
                    //CheckBottomBorderReached(document, writer);
                    //cb.SetColorFill(BaseColor.GREEN.Darker());
                    //cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_BOLDITALIC).BaseFont, 12);
                    //if (Grand_Total == 0)
                    //{
                    //    PrintRec("$ " + "0.00", 20);
                    //    Mst_Poverty = "0";
                    //}
                    //else
                    //{
                    //    if (Grand_Total < 0)
                    //    {
                    //        X_Pos -= 5; Grand_Total = Math.Abs(Grand_Total);
                    //        PrintRec("( $ " + Grand_Total.ToString() + " )", 20);
                    //    }
                    //    else
                    //        PrintRec("$ " + Grand_Total.ToString(), 20);
                    //}
                    //X_Pos = 200;
                    //cb.SetCMYKColorFill(0, 0, 0, 255);
                    ////cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ITALIC).BaseFont, 11);
                    //PrintRec("Grand Total(all income recorded) = ", 10);

                    //cb.EndText();

                    //cb.BeginText();

                    //PrintFooterBox(document, writer);

                    ////cb.SetColorStroke(BaseColor.DARK_GRAY);
                    //cb.SetLineWidth(4.0f);
                    //cb.SetGrayStroke(0.7f);
                    //cb.MoveTo(15f, 25f);
                    //cb.LineTo(590f, 25f);
                    //cb.Stroke();

                    //cb.BeginText();
                    //Y_Pos = 10;
                    //X_Pos = 20;
                    //cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES).BaseFont, 10);
                    //cb.SetCMYKColorFill(255, 255, 0, 99);
                    ////cb.SetCMYKColorFill(0, 0, 0, 255);
                    //PrintRec(DateTime.Now.ToLocalTime().ToString(), 130);

                    //X_Pos = 250;
                    ////cb.SetCMYKColorFill(0, 0, 0, 255);
                    //PrintRec("USER-ID=" + BaseForm.UserID.ToString().Trim(), 150);

                    ////Y_Pos = 10;
                    //X_Pos = 550;
                    //PrintRec("Page:", 28);
                    //PrintRec(pageNumber.ToString(), 15);
                    //cb.SetCMYKColorFill(0, 0, 0, 255);
                    //cb.EndText();

                }
                else
                {
                    cb.BeginText();

                    //CaseSnpEntity snpEntity = BaseForm.BaseCaseSnpEntity.Find(u => u.FamilySeq.Equals(Fam_Sqe));
                    if (BaseForm.BaseCaseSnpEntity.Count > 0)
                    {
                        X_Pos = 50; Y_Pos -= 10;
                        foreach (CaseSnpEntity snpEntity in BaseForm.BaseCaseSnpEntity)
                        {
                            Count++;
                            if (Count > 1) { Y_Pos -= 10; CheckBottomBorderReached(document, writer); X_Pos = 50; }
                            CheckBottomBorderReached(document, writer);
                            cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ITALIC).BaseFont, 10);
                            cb.SetColorFill(BaseColor.BLUE);
                            PrintRec(LookupDataAccess.GetMemberName(snpEntity.NameixFi, snpEntity.NameixMi, snpEntity.NameixLast, BaseForm.BaseHierarchyCnFormat), 150);
                            cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ROMAN).BaseFont, 10);
                            cb.SetCMYKColorFill(0, 0, 0, 255);

                            string strSsno = LookupDataAccess.GetCardNo(snpEntity.Ssno.Trim(), "1", string.Empty, string.Empty);
                            strSsno = strSsno.Replace("n", "x");
                            PrintRec("SSN# : " + strSsno, 110);
                            TmpDOB = time[0] = null;
                            TmpDOB = LookupDataAccess.Getdate(snpEntity.AltBdate);
                            //time = Regex.Split(TmpDOB.ToString(), " ");
                            //TmpDOB = time[0];
                            PrintRec("DOB: " + TmpDOB, 80);
                            //Prev_fam_Seq = dr1["INCOME_FAMILY_SEQ"].ToString().Trim();
                            //prev_income_Seq = null;
                            Y_Pos -= 15;
                            CheckBottomBorderReached(document, writer);

                            Y_Pos -= 25;
                            CheckBottomBorderReached(document, writer); X_Pos = 50;
                            cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ROMAN).BaseFont, 10);
                            PrintRec("NONE", 70);
                            PrintRec("NONE", 140);
                            PrintRec("", 80); //80
                            PrintRec(LookupDataAccess.Getdate(BaseForm.BaseCaseMstListEntity[0].IntakeDate.Trim()), 75); //100
                            PrintRec("$ " + "0.00", 50);
                            PrintRec("", 60);


                            Y_Pos -= 6; X_Pos = 415;//X_Pos = 435;
                            cb.MoveTo(X_Pos, Y_Pos);
                            cb.LineTo(X_Pos + 50, Y_Pos);
                            cb.SetLineWidth(0.5f);
                            cb.Stroke();
                            Y_Pos -= 10; X_Pos = 415;// X_Pos = 435;
                            CheckBottomBorderReached(document, writer);
                            cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_BOLD).BaseFont, 10);
                            PrintRec("$ " + "0.00", 20);
                            Y_Pos -= 14;

                        }
                    }




                }

                Y_Pos -= 20; X_Pos = 405; //X_Pos = 425;
                CheckBottomBorderReached(document, writer);
                X_Pos = 405;
                cb.EndText();
                cb.MoveTo(X_Pos, Y_Pos + 10);
                cb.LineTo(X_Pos + 70, Y_Pos + 10);
                cb.LineTo(X_Pos + 70, Y_Pos - 13);
                cb.LineTo(X_Pos, Y_Pos - 13);
                cb.SetLineWidth(1f);
                cb.SetColorStroke(BaseColor.GRAY.Brighter());
                cb.Stroke();
                cb.BeginText();
                Y_Pos -= 3;
                CheckBottomBorderReached(document, writer);
                cb.SetColorFill(BaseColor.GREEN.Darker());
                cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_BOLDITALIC).BaseFont, 12);
                if (Grand_Total == 0)
                {
                    PrintRec("$ " + "0.00", 20);
                    //Mst_Poverty = "0";
                }
                else
                {
                    if (Grand_Total < 0)
                    {
                        X_Pos -= 5; Grand_Total = Math.Abs(Grand_Total);
                        PrintRec("( $ " + Grand_Total.ToString() + " )", 20);
                    }
                    else
                        PrintRec("$ " + Grand_Total.ToString(), 20);
                }
                X_Pos = 200;
                cb.SetCMYKColorFill(0, 0, 0, 255);
                //cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ITALIC).BaseFont, 11);
                PrintRec("Grand Total(all income recorded) = ", 10);

                //Y_Pos -= 20; X_Pos = 50;
                //CheckBottomBorderReached(document, writer);
                //PrintRec("How Verified: " + HowVerified.Trim(), 200);

                cb.EndText();

                cb.BeginText();

                PrintFooterBox(document, writer, caseVerList);

                

                //cb.SetColorStroke(BaseColor.DARK_GRAY);
                cb.SetLineWidth(4.0f);
                cb.SetGrayStroke(0.7f);
                cb.MoveTo(15f, 25f);
                cb.LineTo(590f, 25f);
                cb.Stroke();

                cb.BeginText();
                
                if(BaseForm.BaseAgencyControlDetails.AgyShortName=="NCCAA")
                    cb.ShowTextAligned(Element.ALIGN_LEFT, "ES Signature ___________________________", 55, 80, 0);
                else
                    cb.ShowTextAligned(Element.ALIGN_LEFT, "Client Signature ___________________________", 55, 80, 0);

                Y_Pos = 10;
                X_Pos = 20;
                cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES).BaseFont, 10);
                cb.SetCMYKColorFill(255, 255, 0, 99);
                //cb.SetCMYKColorFill(0, 0, 0, 255);
                PrintRec(DateTime.Now.ToLocalTime().ToString(), 130);

                X_Pos = 250;
                //cb.SetCMYKColorFill(0, 0, 0, 255);
                PrintRec("USER-ID=" + BaseForm.UserID.ToString().Trim(), 150);

                //Y_Pos = 10;
                X_Pos = 550;
                PrintRec("Page:", 28);
                PrintRec(pageNumber.ToString(), 15);
                cb.SetCMYKColorFill(0, 0, 0, 255);
                cb.EndText();

            }
            catch (Exception ex) { document.Add(new Paragraph("Aborted due to Exception............................................... ")); }

            document.Close();
            fs.Close();
            fs.Dispose();


            //FrmViewer objfrm = new FrmViewer(PdfName);
            //objfrm.ShowDialog();

        }

        public System.Drawing.Image byteArrayToImage(byte[] byteArrayIn)
        {
            using (MemoryStream mStream = new MemoryStream(byteArrayIn))
            {
                return System.Drawing.Image.FromStream(mStream);
            }
        }

        private void PrintRec(string PrintText, int StrWidth)
        {
            //cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_ROMAN).BaseFont, 11);//cb.SetFontAndSize(bfTimes, 10);
            cb.ShowTextAligned(800, PrintText, X_Pos, Y_Pos, 0);
            X_Pos += StrWidth;
            PrintText = null;
        }

        private void CalculateEMS30dayIncome(string Interval, decimal Income,string SortNeg)
        {
            if (propHourlyMode == "Y")
            {
                if (Interval == "Annual")
                {
                    if (Income != 0)
                    {
                        if (SortNeg == "N") ems_30days_Income = ems_30days_Income - (Income / 12);
                        else  ems_30days_Income = ems_30days_Income + (Income / 12);
                    }
                }
                else
                {
                    if (SortNeg == "N") ems_30days_Income = ems_30days_Income - Income ;
                    else ems_30days_Income = ems_30days_Income + Income ;
                }
            }
        }


        private void PrintHeaderPage()
        {
            string Snp_App = string.Empty;
            //DataSet ds = DatabaseLayer.ADMNB002DB.GET_INCOME_REPORT(AppKey.Substring(0, 10), AppKey.Substring(10, 8),BaseForm.BaseCaseMstListEntity[0].FamilySeq);
            //DataTable dt = ds.Tables[0];
            //DataRow dr;
            Snp_App = BaseForm.BaseApplicationNo;
            //if (dt.Rows.Count > 0)
            //{
            //    dr = dt.Rows[0];
            //    Snp_App = dr["SNP_APP"].ToString();
            //}
            DataSet dsCaseHie = DatabaseLayer.ADMNB001DB.ADMNB001_GetCashie("**-**-**",BaseForm.UserID,BaseForm.BaseAdminAgency);
            DataTable dtCaseHie = dsCaseHie.Tables[0];

            string AgencyName = string.Empty;
            if (dtCaseHie.Rows.Count > 0)
            {
                foreach (DataRow drCasehie in dtCaseHie.Rows)
                {
                    if (drCasehie["Code"].ToString().Trim() == BaseForm.BaseAgency + BaseForm.BaseDept + BaseForm.BaseProg)
                    {
                        AgencyName = drCasehie["HIE_NAME"].ToString().Trim(); break;
                    }
                }
            }

            cb.BeginText();
           
            
            cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_BOLDITALIC).BaseFont, 20);
            cb.SetColorFill(BaseColor.BLUE.Darker());

            cb.ShowTextAligned(Element.ALIGN_LEFT, "Income Documentation - detail", 50, 775, 0);
            cb.EndText();
            cb.SetLineWidth(1.5f);

            Rectangle rect = new Rectangle(50, 760, 580, 720);
            rect.BorderWidthTop = 3;
            rect.BorderWidthBottom = 1;
            rect.BorderWidthRight = 3;
            rect.BorderWidthLeft = 1;
            cb.Rectangle(rect);

            Rectangle rectheader = new Rectangle(50, 680, 580, 660);
            rectheader.BackgroundColor = new CMYKColor(0, 0, 0, 64);
            cb.Rectangle(rectheader);

            cb.BeginText();
            cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_BOLD).BaseFont, 11);
            cb.ShowTextAligned(Element.ALIGN_LEFT,BaseForm.BaseApplicationName.Trim(), 60, 740, 0);
            cb.ShowTextAligned(Element.ALIGN_LEFT, "Application " + Snp_App, 60, 725, 0);

            cb.ShowTextAligned(Element.ALIGN_LEFT, AgencyName, 250, 725, 0);

            //cb.ShowTextAligned(Element.ALIGN_LEFT, "Application Date " + IntakeDate, 370, 725, 0);
            cb.ShowTextAligned(Element.ALIGN_LEFT, "Application Date " + IntakeDate, 435, 725, 0);
            cb.SetColorFill(BaseColor.BLUE.Darker());
            Y_Pos = 667; X_Pos = 50;
            PrintRec("Income Type", 100);
            PrintRec("Source", 130);
            PrintRec("Interval", 60);
            PrintRec("Date", 75);
            PrintRec("Amount ", 60);
            PrintRec("Verfication ", 60);
            cb.EndText();
        }

        private void PrintFooterBox(Document document, PdfWriter writer, CaseVerEntity VerEntity)
        {
            Y_Pos -= 40;
            if (Y_Pos < 100)
                Y_Pos = 19;

            CheckBottomBorderReached(document, writer);
            cb.EndText();
            
            Rectangle rectFooter = new Rectangle(50, Y_Pos, 320, Y_Pos-80);
            rectFooter.BorderWidthTop = 2;
            rectFooter.BorderWidthBottom = 2;
            rectFooter.BorderWidthRight = 2;
            rectFooter.BorderWidthLeft = 2;
            cb.Rectangle(rectFooter);
            cb.BeginText();
            cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES_BOLDITALIC).BaseFont, 12);
            X_Pos = 55; Y_Pos -= 10;
            cb.SetColorFill(BaseColor.BLUE);
            PrintRec("Income Level(30day basis)", 60);
            X_Pos = 65; Y_Pos -= 15;
            cb.SetCMYKColorFill(0, 0, 0, 255);
            cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES).BaseFont, 12);
            string thrgh = string.Empty;
            if (propHourlyMode == "Y") thrgh = LookupDataAccess.Getdate(MstIntakeStartDate.ToShortDateString()) + " thru " + LookupDataAccess.Getdate(MstIntakeEndDate.ToShortDateString()); else thrgh = " // thru // ";
            PrintRec(thrgh, 60);
            X_Pos = 230;   //Grand_Total
            //modify the decimal Values
            decimal Tmp_30Days_Amount = decimal.Parse("0.00");
            if (Mst_Prog_Icn > 0)
            {
                Tmp_30Days_Amount = decimal.Parse((Mst_Prog_Icn / 12).ToString());

                Tmp_30Days_Amount = decimal.Round(Tmp_30Days_Amount, 2);//30 days Income:
            }
            
            cb.ShowTextAligned(Element.ALIGN_RIGHT, "30 days Income:", X_Pos, Y_Pos - 15, 0);
            int Z_pos = Y_Pos-15;
            if (propHourlyMode == "Y")
            {
                if (ems_30days_Income != 0) ems_30days_Income = decimal.Round(ems_30days_Income, 2);

                if (ems_30days_Income < 0) ems_30days_Income = Math.Abs(ems_30days_Income);
                cb.ShowTextAligned(Element.ALIGN_RIGHT, "$" + ems_30days_Income.ToString(), X_Pos + 80, Y_Pos - 15, 0);
            }
            else
            {
                cb.ShowTextAligned(Element.ALIGN_RIGHT, "$" + Tmp_30Days_Amount.ToString(), X_Pos + 80, Y_Pos - 15, 0);
            }

            cb.ShowTextAligned(Element.ALIGN_RIGHT, "Annualized:", X_Pos, Y_Pos - 30, 0);
            if (Mst_Prog_Icn > 0)
            {
                cb.ShowTextAligned(Element.ALIGN_RIGHT, "$" + Mst_Prog_Icn.ToString(), X_Pos + 80, Y_Pos - 30, 0);
                cb.ShowTextAligned(Element.ALIGN_RIGHT, "Poverty level based on above:", X_Pos, Y_Pos - 45, 0);
                if(!string.IsNullOrEmpty(Mst_Poverty.Trim()))
                    cb.ShowTextAligned(Element.ALIGN_RIGHT, Mst_Poverty + "%", X_Pos + 80, Y_Pos - 45, 0);
                else
                    cb.ShowTextAligned(Element.ALIGN_RIGHT, "", X_Pos + 80, Y_Pos - 45, 0);
            }
            else
            {
                cb.ShowTextAligned(Element.ALIGN_RIGHT, "$" + "0.00", X_Pos + 80, Y_Pos - 30, 0);
                cb.ShowTextAligned(Element.ALIGN_RIGHT, "Poverty level based on above:", X_Pos, Y_Pos - 45, 0);
                cb.ShowTextAligned(Element.ALIGN_RIGHT, "0" + "%", X_Pos + 80, Y_Pos - 45, 0);
            }

            if (VerEntity != null)
                cb.ShowTextAligned(Element.ALIGN_RIGHT, "Last Income Verification Date : " + LookupDataAccess.Getdate(VerEntity.VerifyDate.Trim()), X_Pos + 310, Z_pos, 0);

            cb.ShowTextAligned(Element.ALIGN_RIGHT, "# in Program: "+ MST_InProg.ToString(), X_Pos, Y_Pos - 100, 0);
            //cb.ShowTextAligned(Element.ALIGN_RIGHT, MST_InProg.ToString(), X_Pos + 80, Y_Pos - 80, 0);

            //cb.ShowTextAligned(Element.ALIGN_RIGHT, "Client Signature ________________________________________________", 55, Y_Pos - 120, 0);

            cb.EndText();
        }

        int pageNumber = 1;
        private void CheckBottomBorderReached(Document document, PdfWriter writer)
        {
            if (Y_Pos <= 30)
            {
                cb.EndText();
                //cb.BeginText();
                //Y_Pos = 10;
                //X_Pos = 20;
                //cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES).BaseFont, 12);
                //cb.SetCMYKColorFill(0, 0, 0, 255);
                //PrintRec(DateTime.Now.ToLocalTime().ToString(), 130);
                //X_Pos = 250;
                //cb.SetCMYKColorFill(0, 0, 0, 255);
                //PrintRec("USER-ID=" + BaseForm.UserID.ToString().Trim(), 150);
                ////Y_Pos = 07;
                //X_Pos = 550;
                //PrintRec("Page:", 28);
                //PrintRec(pageNumber.ToString(), 15);
                //cb.EndText();

                cb.SetLineWidth(4.0f);
                cb.SetGrayStroke(0.7f);
                cb.MoveTo(15f, 25f);
                cb.LineTo(590f, 25f);
                cb.Stroke();

                cb.BeginText();
                Y_Pos = 10;
                X_Pos = 20;
                cb.SetFontAndSize(FontFactory.GetFont(FontFactory.TIMES).BaseFont, 10);
                cb.SetCMYKColorFill(255, 255, 0, 99);
                //cb.SetCMYKColorFill(0, 0, 0, 255);
                PrintRec(DateTime.Now.ToLocalTime().ToString(), 130);

                X_Pos = 250;
                //cb.SetCMYKColorFill(0, 0, 0, 255);
                PrintRec("USER-ID=" + BaseForm.UserID.ToString().Trim(), 150);

                //Y_Pos = 10;
                X_Pos = 550;
                PrintRec("Page:", 28);
                PrintRec(pageNumber.ToString(), 15);
                cb.SetCMYKColorFill(0, 0, 0, 255);
                cb.EndText();

                document.NewPage();
                pageNumber = pageNumber + 1;
                //pageNumber = writer.PageNumber - 1;

                //Image _image = iTextSharp.text.Image.GetInstance(Application.MapPath("~\\Resources\\images\\Capsystems_WaterMark.bmp"));
                //_image.SetAbsolutePosition(160, 310);
                //_image.RotationDegrees = 45;
                //_image.Rotate();
                //PdfGState _state = new PdfGState()
                //{
                //    FillOpacity = 0.2F,
                //    StrokeOpacity = 0.2F
                //};
                //cb = writer.DirectContentUnder;
                //cb.SaveState();
                //cb.SetGState(_state);                               //WaterMark*******
                //cb.AddImage(_image);
                //cb.RestoreState();


                cb.BeginText();
                
                X_Pos = 50;
                Y_Pos -= 5;
            
                cb.EndText();
               
                Y_Pos = 770;
                X_Pos = 55;                                                           //modified
         
                cb.BeginText();
           
            }
        }

        public string Get_Pdf_Path()
        {
            return PdfName;
        }


        private void Close_Form()
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private DataTable CreateTable()
        {
            try
            {
                DataTable table = new DataTable();

                // Declare DataColumn and DataRow variables.
                DataColumn column;

                // Create new DataColumn, set DataType, ColumnName
                // and add to DataTable.    
                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "SORT_APP_NO";
                table.Columns.Add(column);

                // Create second column.
                column = new DataColumn();
                column.DataType = Type.GetType("System.String");
                column.ColumnName = "SORT_FAMILY_SEQ";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "SORT_MEMBER_NAME";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "SORT_MEMBER_SSN";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "SORT_MEMBER_DOB";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "SORT_INCOME_TYPE";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "SORT_INCOME_TYPE_LIT";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "SORT_INCOME_SOURCE";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.DateTime");
                column.ColumnName = "SORT_INCOME_DATE";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "SORT_INCOME_VAL";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "SORT_INCOME_ACT_VAL";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "SORT_INCOME_VER";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "SORT_INCOME_SW";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "SORT_EXCLUDE_INCOME";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "PRT_SORT_INCOME_SOURCE";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "PRT_SORT_INCOME_VER";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "SORT_INCOME_SEQ";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "SORT_INTERVAL";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "PRT_SORT_INTERVAL";
                table.Columns.Add(column);

                column = new DataColumn();
                column.DataType = System.Type.GetType("System.String");
                column.ColumnName = "SORT_NEGATIVE";
                table.Columns.Add(column);

                return table;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        string Income_Sw = "N";
        private void Income_Check_For_CABA(DataTable dt, DataTable IncomeTable,DataTable IncTypes,List<CommonEntity> CommonEmtity)
        {
            string Sort_SW = "N";
            BaseForm.BaseCaseSnpEntity = BaseForm.BaseCaseSnpEntity.OrderBy(u => u.FamilySeq).ToList();
            foreach (CaseSnpEntity Entity in BaseForm.BaseCaseSnpEntity)
            {
                DataTable dtIncome = new DataTable();
                DataView dv = new DataView(dt);
                dv.RowFilter = "SNP_APP = '" + Entity.App.Trim() + "' AND INCOME_FAMILY_SEQ = '" + Entity.FamilySeq + "'";
                dtIncome = dv.ToTable();

                if (dtIncome.Rows.Count > 0)
                {
                    foreach (DataRow dr1 in dtIncome.Rows)
                    {
                        DataRow drInc = IncomeTable.NewRow();
                        FillTableValues(dr1, drInc, IncTypes, CommonEmtity);
                        

                        if (!string.IsNullOrEmpty(dr1["INCOME_DATE1"].ToString()))
                        {
                            if (Convert.ToDateTime(dr1["INCOME_DATE1"].ToString()) < MstIntakeStartDate || Convert.ToDateTime(dr1["INCOME_DATE1"].ToString()) > MstIntakeEndDate)
                            {
                            }
                            else
                            {
                                drInc.SetField("SORT_INCOME_DATE", dr1["INCOME_DATE1"].ToString());
                                if (dr1["INCOME_EXCLUDE"].ToString().Trim() == "Y")
                                {
                                    drInc.SetField("SORT_INCOME_VAL", "0.00");
                                    drInc.SetField("SORT_INCOME_ACT_VAL", "0.00");
                                    drInc.SetField("SORT_INCOME_VER", dr1["INCOME_HOW_VERIFIED"].ToString() + " (exmp " + dr1["INCOME_VAL1"].ToString() + ")");
                                }
                                else
                                {
                                    drInc.SetField("SORT_INCOME_VAL", dr1["INCOME_VAL1"].ToString());
                                    drInc.SetField("SORT_INCOME_ACT_VAL", dr1["INCOME_VAL1"].ToString());
                                    drInc.SetField("SORT_INCOME_VER", dr1["INCOME_HOW_VERIFIED"].ToString());
                                }
                                drInc.SetField("SORT_INCOME_SW", "Y");
                                Sort_SW = "Y";
                                IncomeTable.Rows.Add(drInc);
                                drInc = IncomeTable.NewRow();
                                FillTableValues(dr1, drInc, IncTypes, CommonEmtity);
                            }
                        }

                        if (!string.IsNullOrEmpty(dr1["INCOME_DATE2"].ToString()))
                        {
                            if (Convert.ToDateTime(dr1["INCOME_DATE2"].ToString()) < MstIntakeStartDate || Convert.ToDateTime(dr1["INCOME_DATE2"].ToString()) > MstIntakeEndDate)
                            {
                            }
                            else
                            {
                                drInc.SetField("SORT_INCOME_DATE", dr1["INCOME_DATE2"].ToString());
                                if (dr1["INCOME_EXCLUDE"].ToString().Trim() == "Y")
                                {
                                    drInc.SetField("SORT_INCOME_VAL", "0.00");
                                    drInc.SetField("SORT_INCOME_ACT_VAL", "0.00");
                                    drInc.SetField("SORT_INCOME_VER", dr1["INCOME_HOW_VERIFIED"].ToString() + " (exmp " + dr1["INCOME_VAL2"].ToString() + ")");
                                }
                                else
                                {
                                    drInc.SetField("SORT_INCOME_VAL", dr1["INCOME_VAL2"].ToString());
                                    drInc.SetField("SORT_INCOME_ACT_VAL", dr1["INCOME_VAL2"].ToString());
                                    drInc.SetField("SORT_INCOME_VER", dr1["INCOME_HOW_VERIFIED"].ToString());
                                }
                                drInc.SetField("SORT_INCOME_SW", "Y");
                                Sort_SW = "Y";
                                IncomeTable.Rows.Add(drInc);
                                drInc = IncomeTable.NewRow();
                                FillTableValues(dr1, drInc, IncTypes, CommonEmtity);
                            }
                        }

                        if (!string.IsNullOrEmpty(dr1["INCOME_DATE3"].ToString()))
                        {
                            if (Convert.ToDateTime(dr1["INCOME_DATE3"].ToString()) < MstIntakeStartDate || Convert.ToDateTime(dr1["INCOME_DATE3"].ToString()) > MstIntakeEndDate)
                            {
                            }
                            else
                            {
                                drInc.SetField("SORT_INCOME_DATE", dr1["INCOME_DATE3"].ToString());
                                if (dr1["INCOME_EXCLUDE"].ToString().Trim() == "Y")
                                {
                                    drInc.SetField("SORT_INCOME_VAL", "0.00");
                                    drInc.SetField("SORT_INCOME_ACT_VAL", "0.00");
                                    drInc.SetField("SORT_INCOME_VER", dr1["INCOME_HOW_VERIFIED"].ToString() + " (exmp " + dr1["INCOME_VAL3"].ToString() + ")");
                                }
                                else
                                {
                                    drInc.SetField("SORT_INCOME_VAL", dr1["INCOME_VAL3"].ToString());
                                    drInc.SetField("SORT_INCOME_ACT_VAL", dr1["INCOME_VAL3"].ToString());
                                    drInc.SetField("SORT_INCOME_VER", dr1["INCOME_HOW_VERIFIED"].ToString());
                                }
                                drInc.SetField("SORT_INCOME_SW", "Y");
                                Sort_SW = "Y";
                                IncomeTable.Rows.Add(drInc);
                                drInc = IncomeTable.NewRow();
                                FillTableValues(dr1, drInc, IncTypes, CommonEmtity);
                            }
                        }

                        if (!string.IsNullOrEmpty(dr1["INCOME_DATE4"].ToString()))
                        {
                            if (Convert.ToDateTime(dr1["INCOME_DATE4"].ToString()) < MstIntakeStartDate || Convert.ToDateTime(dr1["INCOME_DATE4"].ToString()) > MstIntakeEndDate)
                            {
                            }
                            else
                            {
                                drInc.SetField("SORT_INCOME_DATE", dr1["INCOME_DATE4"].ToString());
                                if (dr1["INCOME_EXCLUDE"].ToString().Trim() == "Y")
                                {
                                    drInc.SetField("SORT_INCOME_VAL", "0.00");
                                    drInc.SetField("SORT_INCOME_ACT_VAL", "0.00");
                                    drInc.SetField("SORT_INCOME_VER", dr1["INCOME_HOW_VERIFIED"].ToString() + " (exmp " + dr1["INCOME_VAL4"].ToString() + ")");
                                }
                                else
                                {
                                    drInc.SetField("SORT_INCOME_VAL", dr1["INCOME_VAL4"].ToString());
                                    drInc.SetField("SORT_INCOME_ACT_VAL", dr1["INCOME_VAL4"].ToString());
                                    drInc.SetField("SORT_INCOME_VER", dr1["INCOME_HOW_VERIFIED"].ToString());
                                }
                                drInc.SetField("SORT_INCOME_SW", "Y");
                                Sort_SW = "Y";
                                IncomeTable.Rows.Add(drInc);
                                drInc = IncomeTable.NewRow();
                                FillTableValues(dr1, drInc, IncTypes, CommonEmtity);
                            }
                        }

                        if (!string.IsNullOrEmpty(dr1["INCOME_DATE5"].ToString()))
                        {
                            if (Convert.ToDateTime(dr1["INCOME_DATE5"].ToString()) < MstIntakeStartDate || Convert.ToDateTime(dr1["INCOME_DATE5"].ToString()) > MstIntakeEndDate)
                            {
                            }
                            else
                            {
                                drInc.SetField("SORT_INCOME_DATE", dr1["INCOME_DATE5"].ToString());
                                if (dr1["INCOME_EXCLUDE"].ToString().Trim() == "Y")
                                {
                                    drInc.SetField("SORT_INCOME_VAL", "0.00");
                                    drInc.SetField("SORT_INCOME_ACT_VAL", "0.00");
                                    drInc.SetField("SORT_INCOME_VER", dr1["INCOME_HOW_VERIFIED"].ToString() + " (exmp " + dr1["INCOME_VAL5"].ToString() + ")");
                                }
                                else
                                {
                                    drInc.SetField("SORT_INCOME_VAL", dr1["INCOME_VAL5"].ToString());
                                    drInc.SetField("SORT_INCOME_ACT_VAL", dr1["INCOME_VAL5"].ToString());
                                    drInc.SetField("SORT_INCOME_VER", dr1["INCOME_HOW_VERIFIED"].ToString());
                                }
                                drInc.SetField("SORT_INCOME_SW", "Y");
                                Sort_SW = "Y";
                                IncomeTable.Rows.Add(drInc);
                                drInc = IncomeTable.NewRow();
                                FillTableValues(dr1, drInc, IncTypes, CommonEmtity);
                            }
                        }

                        if (Sort_SW == "N")
                        {
                            drInc.SetField("SORT_INCOME_VAL", "0.00");
                            drInc.SetField("SORT_INCOME_ACT_VAL", "0.00");
                            drInc.SetField("SORT_INCOME_SEQ", "1");
                            drInc.SetField("SORT_INCOME_SW", "Y");
                            Sort_SW = "Y";
                            IncomeTable.Rows.Add(drInc);
                            //drInc = IncomeTable.NewRow();
                            //FillTableValues(dr1, drInc, IncTypes, CommonEmtity);
                        }

                        //for (i = 1; i < 6; i++)
                        //{
                        //    if (!string.IsNullOrEmpty(dr1["INCOME_DATE" + i.ToString()].ToString()))
                        //    {
                        //        if (Convert.ToDateTime(dr1["INCOME_DATE" + i.ToString()].ToString()) < MstIntakeStartDate || Convert.ToDateTime(dr1["INCOME_DATE" + i.ToString()].ToString()) < MstIntakeEndDate)
                        //        {
                        //            dr1["INCOME_DATE" + i.ToString()] = string.Empty;
                        //            dr1["INCOME_VAL" + i.ToString()] = "0.00";
                        //        }

                        //        drInc = dr1;
                        //    }


                        //}
                    }

                }
                else
                {
                    DataRow drInc = IncomeTable.NewRow();
                    FillTableValuesNoIncome(Entity, drInc);
                    IncomeTable.Rows.Add(drInc);
                }
            }
            

            #region Previous Code

            //foreach (DataRow dr1 in dt.Rows)
            //{
            //    DataRow drInc = IncomeTable.NewRow();
            //    FillTableValues(dr1, drInc, IncTypes, CommonEmtity);
            //    string Sort_SW = "N";

            //    if (!string.IsNullOrEmpty(dr1["INCOME_DATE1"].ToString()))
            //    {
            //        if (Convert.ToDateTime(dr1["INCOME_DATE1"].ToString()) < MstIntakeStartDate || Convert.ToDateTime(dr1["INCOME_DATE1"].ToString()) > MstIntakeEndDate)
            //        {
            //        }
            //        else
            //        {
            //            drInc.SetField("SORT_INCOME_DATE", dr1["INCOME_DATE1"].ToString());
            //            if (dr1["INCOME_EXCLUDE"].ToString().Trim() == "Y")
            //            {
            //                drInc.SetField("SORT_INCOME_VAL", "0.00");
            //                drInc.SetField("SORT_INCOME_ACT_VAL", "0.00");
            //                drInc.SetField("SORT_INCOME_VER", dr1["INCOME_HOW_VERIFIED"].ToString() + " (exmp " + dr1["INCOME_VAL1"].ToString() + ")");
            //            }
            //            else
            //            {
            //                drInc.SetField("SORT_INCOME_VAL", dr1["INCOME_VAL1"].ToString());
            //                drInc.SetField("SORT_INCOME_ACT_VAL", dr1["INCOME_VAL1"].ToString());
            //                drInc.SetField("SORT_INCOME_VER", dr1["INCOME_HOW_VERIFIED"].ToString());
            //            }
            //            drInc.SetField("SORT_INCOME_SW", "Y");
            //            Sort_SW = "Y";
            //            IncomeTable.Rows.Add(drInc);
            //            drInc = IncomeTable.NewRow();
            //            FillTableValues(dr1, drInc, IncTypes, CommonEmtity);
            //        }
            //    }

            //    if (!string.IsNullOrEmpty(dr1["INCOME_DATE2"].ToString()))
            //    {
            //        if (Convert.ToDateTime(dr1["INCOME_DATE2"].ToString()) < MstIntakeStartDate || Convert.ToDateTime(dr1["INCOME_DATE2"].ToString()) > MstIntakeEndDate)
            //        {
            //        }
            //        else
            //        {
            //            drInc.SetField("SORT_INCOME_DATE", dr1["INCOME_DATE2"].ToString());
            //            if (dr1["INCOME_EXCLUDE"].ToString().Trim() == "Y")
            //            {
            //                drInc.SetField("SORT_INCOME_VAL", "0.00");
            //                drInc.SetField("SORT_INCOME_ACT_VAL", "0.00");
            //                drInc.SetField("SORT_INCOME_VER", dr1["INCOME_HOW_VERIFIED"].ToString() + " (exmp " + dr1["INCOME_VAL2"].ToString() + ")");
            //            }
            //            else
            //            {
            //                drInc.SetField("SORT_INCOME_VAL", dr1["INCOME_VAL2"].ToString());
            //                drInc.SetField("SORT_INCOME_ACT_VAL", dr1["INCOME_VAL2"].ToString());
            //                drInc.SetField("SORT_INCOME_VER", dr1["INCOME_HOW_VERIFIED"].ToString());
            //            }
            //            drInc.SetField("SORT_INCOME_SW", "Y");
            //            Sort_SW = "Y";
            //            IncomeTable.Rows.Add(drInc);
            //            drInc = IncomeTable.NewRow();
            //            FillTableValues(dr1, drInc, IncTypes, CommonEmtity);
            //        }
            //    }

            //    if (!string.IsNullOrEmpty(dr1["INCOME_DATE3"].ToString()))
            //    {
            //        if (Convert.ToDateTime(dr1["INCOME_DATE3"].ToString()) < MstIntakeStartDate || Convert.ToDateTime(dr1["INCOME_DATE3"].ToString()) > MstIntakeEndDate)
            //        {
            //        }
            //        else
            //        {
            //            drInc.SetField("SORT_INCOME_DATE", dr1["INCOME_DATE3"].ToString());
            //            if (dr1["INCOME_EXCLUDE"].ToString().Trim() == "Y")
            //            {
            //                drInc.SetField("SORT_INCOME_VAL", "0.00");
            //                drInc.SetField("SORT_INCOME_ACT_VAL", "0.00");
            //                drInc.SetField("SORT_INCOME_VER", dr1["INCOME_HOW_VERIFIED"].ToString() + " (exmp " + dr1["INCOME_VAL3"].ToString() + ")");
            //            }
            //            else
            //            {
            //                drInc.SetField("SORT_INCOME_VAL", dr1["INCOME_VAL3"].ToString());
            //                drInc.SetField("SORT_INCOME_ACT_VAL", dr1["INCOME_VAL3"].ToString());
            //                drInc.SetField("SORT_INCOME_VER", dr1["INCOME_HOW_VERIFIED"].ToString());
            //            }
            //            drInc.SetField("SORT_INCOME_SW", "Y");
            //            Sort_SW = "Y";
            //            IncomeTable.Rows.Add(drInc);
            //            drInc = IncomeTable.NewRow();
            //            FillTableValues(dr1, drInc, IncTypes, CommonEmtity);
            //        }
            //    }

            //    if (!string.IsNullOrEmpty(dr1["INCOME_DATE4"].ToString()))
            //    {
            //        if (Convert.ToDateTime(dr1["INCOME_DATE4"].ToString()) < MstIntakeStartDate || Convert.ToDateTime(dr1["INCOME_DATE4"].ToString()) > MstIntakeEndDate)
            //        {
            //        }
            //        else
            //        {
            //            drInc.SetField("SORT_INCOME_DATE", dr1["INCOME_DATE4"].ToString());
            //            if (dr1["INCOME_EXCLUDE"].ToString().Trim() == "Y")
            //            {
            //                drInc.SetField("SORT_INCOME_VAL", "0.00");
            //                drInc.SetField("SORT_INCOME_ACT_VAL", "0.00");
            //                drInc.SetField("SORT_INCOME_VER", dr1["INCOME_HOW_VERIFIED"].ToString() + " (exmp " + dr1["INCOME_VAL4"].ToString() + ")");
            //            }
            //            else
            //            {
            //                drInc.SetField("SORT_INCOME_VAL", dr1["INCOME_VAL4"].ToString());
            //                drInc.SetField("SORT_INCOME_ACT_VAL", dr1["INCOME_VAL4"].ToString());
            //                drInc.SetField("SORT_INCOME_VER", dr1["INCOME_HOW_VERIFIED"].ToString());
            //            }
            //            drInc.SetField("SORT_INCOME_SW", "Y");
            //            Sort_SW = "Y";
            //            IncomeTable.Rows.Add(drInc);
            //            drInc = IncomeTable.NewRow();
            //            FillTableValues(dr1, drInc, IncTypes, CommonEmtity);
            //        }
            //    }

            //    if (!string.IsNullOrEmpty(dr1["INCOME_DATE5"].ToString()))
            //    {
            //        if (Convert.ToDateTime(dr1["INCOME_DATE5"].ToString()) < MstIntakeStartDate || Convert.ToDateTime(dr1["INCOME_DATE5"].ToString()) > MstIntakeEndDate)
            //        {
            //        }
            //        else
            //        {
            //            drInc.SetField("SORT_INCOME_DATE", dr1["INCOME_DATE5"].ToString());
            //            if (dr1["INCOME_EXCLUDE"].ToString().Trim() == "Y")
            //            {
            //                drInc.SetField("SORT_INCOME_VAL", "0.00");
            //                drInc.SetField("SORT_INCOME_ACT_VAL", "0.00");
            //                drInc.SetField("SORT_INCOME_VER", dr1["INCOME_HOW_VERIFIED"].ToString() + " (exmp " + dr1["INCOME_VAL5"].ToString() + ")");
            //            }
            //            else
            //            {
            //                drInc.SetField("SORT_INCOME_VAL", dr1["INCOME_VAL5"].ToString());
            //                drInc.SetField("SORT_INCOME_ACT_VAL", dr1["INCOME_VAL5"].ToString());
            //                drInc.SetField("SORT_INCOME_VER", dr1["INCOME_HOW_VERIFIED"].ToString());
            //            }
            //            drInc.SetField("SORT_INCOME_SW", "Y");
            //            Sort_SW = "Y";
            //            IncomeTable.Rows.Add(drInc);
            //            drInc = IncomeTable.NewRow();
            //            FillTableValues(dr1, drInc, IncTypes, CommonEmtity);
            //        }
            //    }

            //    if (Sort_SW == "N")
            //    {
            //        drInc.SetField("SORT_INCOME_VAL", "0.00");
            //        drInc.SetField("SORT_INCOME_ACT_VAL", "0.00");
            //        drInc.SetField("SORT_INCOME_SEQ", "1");
            //        drInc.SetField("SORT_INCOME_SW", "Y");
            //        Sort_SW = "Y";
            //        IncomeTable.Rows.Add(drInc);
            //        //drInc = IncomeTable.NewRow();
            //        //FillTableValues(dr1, drInc, IncTypes, CommonEmtity);
            //    }

            //    //for (i = 1; i < 6; i++)
            //    //{
            //    //    if (!string.IsNullOrEmpty(dr1["INCOME_DATE" + i.ToString()].ToString()))
            //    //    {
            //    //        if (Convert.ToDateTime(dr1["INCOME_DATE" + i.ToString()].ToString()) < MstIntakeStartDate || Convert.ToDateTime(dr1["INCOME_DATE" + i.ToString()].ToString()) < MstIntakeEndDate)
            //    //        {
            //    //            dr1["INCOME_DATE" + i.ToString()] = string.Empty;
            //    //            dr1["INCOME_VAL" + i.ToString()] = "0.00";
            //    //        }

            //    //        drInc = dr1;
            //    //    }


            //    //}
            //}
            #endregion

        }

        private void Income_Check_For_CABA1(List<CaseIncomeEntity> caseIncomeList, DataTable IncomeTable, DataTable IncTypes, List<CommonEntity> CommonEmtity)
        {
            string Sort_SW = "N";
            BaseForm.BaseCaseSnpEntity = BaseForm.BaseCaseSnpEntity.OrderBy(u => u.FamilySeq).ToList();
            foreach (CaseSnpEntity Entity in BaseForm.BaseCaseSnpEntity)
            {
                List<CaseIncomeEntity> SelIncomeRecs = caseIncomeList.FindAll(u => u.App.Equals(Entity.App) && u.FamilySeq.Equals(Entity.FamilySeq));
                Sort_SW = "N";
                if (SelIncomeRecs.Count > 0)
                {
                    foreach (CaseIncomeEntity dr1 in SelIncomeRecs)
                    {
                        DataRow drInc = IncomeTable.NewRow();
                        FillTableValues1(dr1, drInc, IncTypes, CommonEmtity,Entity);


                        if (!string.IsNullOrEmpty(dr1.Date1.Trim()))
                        {
                            if (Convert.ToDateTime(dr1.Date1.Trim()) < MstIntakeStartDate || Convert.ToDateTime(dr1.Date1.Trim()) > MstIntakeEndDate)
                            {
                            }
                            else
                            {
                                drInc.SetField("SORT_INCOME_DATE", dr1.Date1.Trim());
                                if (dr1.Exclude.Trim() == "Y")
                                {
                                    drInc.SetField("SORT_INCOME_VAL", "0.00");
                                    drInc.SetField("SORT_INCOME_ACT_VAL", "0.00");
                                    drInc.SetField("SORT_INCOME_VER", dr1.HowVerified + " (exmp " + dr1.Val1.ToString() + ")");
                                }
                                else
                                {
                                    drInc.SetField("SORT_INCOME_VAL", dr1.Val1.ToString());
                                    drInc.SetField("SORT_INCOME_ACT_VAL", dr1.Val1.ToString());
                                    drInc.SetField("SORT_INCOME_VER", dr1.HowVerified.ToString());
                                }
                                drInc.SetField("SORT_INCOME_SW", "Y");
                                Sort_SW = "Y";
                                IncomeTable.Rows.Add(drInc);
                                drInc = IncomeTable.NewRow();
                                FillTableValues1(dr1, drInc, IncTypes, CommonEmtity,Entity);
                            }
                        }

                        if (!string.IsNullOrEmpty(dr1.Date2.Trim()))
                        {
                            if (Convert.ToDateTime(dr1.Date2.Trim()) < MstIntakeStartDate || Convert.ToDateTime(dr1.Date2.Trim()) > MstIntakeEndDate)
                            {
                            }
                            else
                            {
                                drInc.SetField("SORT_INCOME_DATE", dr1.Date2.Trim());
                                if (dr1.Exclude.ToString().Trim() == "Y")
                                {
                                    drInc.SetField("SORT_INCOME_VAL", "0.00");
                                    drInc.SetField("SORT_INCOME_ACT_VAL", "0.00");
                                    drInc.SetField("SORT_INCOME_VER", dr1.HowVerified.ToString() + " (exmp " + dr1.Val2.ToString() + ")");
                                }
                                else
                                {
                                    drInc.SetField("SORT_INCOME_VAL", dr1.Val2.ToString());
                                    drInc.SetField("SORT_INCOME_ACT_VAL", dr1.Val2.ToString());
                                    drInc.SetField("SORT_INCOME_VER", dr1.HowVerified.ToString());
                                }
                                drInc.SetField("SORT_INCOME_SW", "Y");
                                Sort_SW = "Y";
                                IncomeTable.Rows.Add(drInc);
                                drInc = IncomeTable.NewRow();
                                FillTableValues1(dr1, drInc, IncTypes, CommonEmtity,Entity);
                            }
                        }

                        if (!string.IsNullOrEmpty(dr1.Date3.Trim()))
                        {
                            if (Convert.ToDateTime(dr1.Date3.Trim()) < MstIntakeStartDate || Convert.ToDateTime(dr1.Date3.Trim()) > MstIntakeEndDate)
                            {
                            }
                            else
                            {
                                drInc.SetField("SORT_INCOME_DATE", dr1.Date3.Trim());
                                if (dr1.Exclude.ToString().Trim() == "Y")
                                {
                                    drInc.SetField("SORT_INCOME_VAL", "0.00");
                                    drInc.SetField("SORT_INCOME_ACT_VAL", "0.00");
                                    drInc.SetField("SORT_INCOME_VER", dr1.HowVerified.ToString() + " (exmp " + dr1.Val3.ToString() + ")");
                                }
                                else
                                {
                                    drInc.SetField("SORT_INCOME_VAL", dr1.Val3.ToString());
                                    drInc.SetField("SORT_INCOME_ACT_VAL", dr1.Val3.ToString());
                                    drInc.SetField("SORT_INCOME_VER", dr1.HowVerified.ToString());
                                }
                                drInc.SetField("SORT_INCOME_SW", "Y");
                                Sort_SW = "Y";
                                IncomeTable.Rows.Add(drInc);
                                drInc = IncomeTable.NewRow();
                                FillTableValues1(dr1, drInc, IncTypes, CommonEmtity,Entity);
                            }
                        }

                        if (!string.IsNullOrEmpty(dr1.Date4.Trim()))
                        {
                            if (Convert.ToDateTime(dr1.Date4.Trim()) < MstIntakeStartDate || Convert.ToDateTime(dr1.Date4.Trim()) > MstIntakeEndDate)
                            {
                            }
                            else
                            {
                                drInc.SetField("SORT_INCOME_DATE", dr1.Date4.Trim());
                                if (dr1.Exclude.ToString().Trim() == "Y")
                                {
                                    drInc.SetField("SORT_INCOME_VAL", "0.00");
                                    drInc.SetField("SORT_INCOME_ACT_VAL", "0.00");
                                    drInc.SetField("SORT_INCOME_VER", dr1.HowVerified + " (exmp " + dr1.Val4.ToString() + ")");
                                }
                                else
                                {
                                    drInc.SetField("SORT_INCOME_VAL", dr1.Val4.ToString());
                                    drInc.SetField("SORT_INCOME_ACT_VAL", dr1.Val4.ToString());
                                    drInc.SetField("SORT_INCOME_VER", dr1.HowVerified.ToString());
                                }
                                drInc.SetField("SORT_INCOME_SW", "Y");
                                Sort_SW = "Y";
                                IncomeTable.Rows.Add(drInc);
                                drInc = IncomeTable.NewRow();
                                FillTableValues1(dr1, drInc, IncTypes, CommonEmtity,Entity);
                            }
                        }

                        if (!string.IsNullOrEmpty(dr1.Date5.Trim()))
                        {
                            if (Convert.ToDateTime(dr1.Date5.Trim()) < MstIntakeStartDate || Convert.ToDateTime(dr1.Date5.Trim()) > MstIntakeEndDate)
                            {
                            }
                            else
                            {
                                drInc.SetField("SORT_INCOME_DATE", dr1.Date5.Trim());
                                if (dr1.Exclude.ToString().Trim() == "Y")
                                {
                                    drInc.SetField("SORT_INCOME_VAL", "0.00");
                                    drInc.SetField("SORT_INCOME_ACT_VAL", "0.00");
                                    drInc.SetField("SORT_INCOME_VER", dr1.HowVerified.ToString() + " (exmp " + dr1.Val5.ToString() + ")");
                                }
                                else
                                {
                                    drInc.SetField("SORT_INCOME_VAL", dr1.Val5.ToString());
                                    drInc.SetField("SORT_INCOME_ACT_VAL", dr1.Val5.ToString());
                                    drInc.SetField("SORT_INCOME_VER", dr1.HowVerified.ToString());
                                }
                                drInc.SetField("SORT_INCOME_SW", "Y");
                                Sort_SW = "Y";
                                IncomeTable.Rows.Add(drInc);
                                drInc = IncomeTable.NewRow();
                                FillTableValues1(dr1, drInc, IncTypes, CommonEmtity,Entity);
                            }
                        }

                        if (Sort_SW == "N")
                        {
                            drInc.SetField("SORT_INCOME_VAL", "0.00");
                            drInc.SetField("SORT_INCOME_ACT_VAL", "0.00");
                            drInc.SetField("SORT_INCOME_SEQ", "1");
                            drInc.SetField("SORT_INCOME_SW", "Y");
                            drInc.SetField("SORT_INCOME_VER", dr1.HowVerified.ToString());
                            Sort_SW = "Y";
                            IncomeTable.Rows.Add(drInc);
                            //drInc = IncomeTable.NewRow();
                            //FillTableValues(dr1, drInc, IncTypes, CommonEmtity);
                        }

                        //for (i = 1; i < 6; i++)
                        //{
                        //    if (!string.IsNullOrEmpty(dr1["INCOME_DATE" + i.ToString()].ToString()))
                        //    {
                        //        if (Convert.ToDateTime(dr1["INCOME_DATE" + i.ToString()].ToString()) < MstIntakeStartDate || Convert.ToDateTime(dr1["INCOME_DATE" + i.ToString()].ToString()) < MstIntakeEndDate)
                        //        {
                        //            dr1["INCOME_DATE" + i.ToString()] = string.Empty;
                        //            dr1["INCOME_VAL" + i.ToString()] = "0.00";
                        //        }

                        //        drInc = dr1;
                        //    }


                        //}
                    }

                }
                else
                {
                    DataRow drInc = IncomeTable.NewRow();
                    FillTableValuesNoIncome(Entity, drInc);
                    IncomeTable.Rows.Add(drInc);
                }
            }


            #region Previous Code

            //foreach (DataRow dr1 in dt.Rows)
            //{
            //    DataRow drInc = IncomeTable.NewRow();
            //    FillTableValues(dr1, drInc, IncTypes, CommonEmtity);
            //    string Sort_SW = "N";

            //    if (!string.IsNullOrEmpty(dr1["INCOME_DATE1"].ToString()))
            //    {
            //        if (Convert.ToDateTime(dr1["INCOME_DATE1"].ToString()) < MstIntakeStartDate || Convert.ToDateTime(dr1["INCOME_DATE1"].ToString()) > MstIntakeEndDate)
            //        {
            //        }
            //        else
            //        {
            //            drInc.SetField("SORT_INCOME_DATE", dr1["INCOME_DATE1"].ToString());
            //            if (dr1["INCOME_EXCLUDE"].ToString().Trim() == "Y")
            //            {
            //                drInc.SetField("SORT_INCOME_VAL", "0.00");
            //                drInc.SetField("SORT_INCOME_ACT_VAL", "0.00");
            //                drInc.SetField("SORT_INCOME_VER", dr1["INCOME_HOW_VERIFIED"].ToString() + " (exmp " + dr1["INCOME_VAL1"].ToString() + ")");
            //            }
            //            else
            //            {
            //                drInc.SetField("SORT_INCOME_VAL", dr1["INCOME_VAL1"].ToString());
            //                drInc.SetField("SORT_INCOME_ACT_VAL", dr1["INCOME_VAL1"].ToString());
            //                drInc.SetField("SORT_INCOME_VER", dr1["INCOME_HOW_VERIFIED"].ToString());
            //            }
            //            drInc.SetField("SORT_INCOME_SW", "Y");
            //            Sort_SW = "Y";
            //            IncomeTable.Rows.Add(drInc);
            //            drInc = IncomeTable.NewRow();
            //            FillTableValues(dr1, drInc, IncTypes, CommonEmtity);
            //        }
            //    }

            //    if (!string.IsNullOrEmpty(dr1["INCOME_DATE2"].ToString()))
            //    {
            //        if (Convert.ToDateTime(dr1["INCOME_DATE2"].ToString()) < MstIntakeStartDate || Convert.ToDateTime(dr1["INCOME_DATE2"].ToString()) > MstIntakeEndDate)
            //        {
            //        }
            //        else
            //        {
            //            drInc.SetField("SORT_INCOME_DATE", dr1["INCOME_DATE2"].ToString());
            //            if (dr1["INCOME_EXCLUDE"].ToString().Trim() == "Y")
            //            {
            //                drInc.SetField("SORT_INCOME_VAL", "0.00");
            //                drInc.SetField("SORT_INCOME_ACT_VAL", "0.00");
            //                drInc.SetField("SORT_INCOME_VER", dr1["INCOME_HOW_VERIFIED"].ToString() + " (exmp " + dr1["INCOME_VAL2"].ToString() + ")");
            //            }
            //            else
            //            {
            //                drInc.SetField("SORT_INCOME_VAL", dr1["INCOME_VAL2"].ToString());
            //                drInc.SetField("SORT_INCOME_ACT_VAL", dr1["INCOME_VAL2"].ToString());
            //                drInc.SetField("SORT_INCOME_VER", dr1["INCOME_HOW_VERIFIED"].ToString());
            //            }
            //            drInc.SetField("SORT_INCOME_SW", "Y");
            //            Sort_SW = "Y";
            //            IncomeTable.Rows.Add(drInc);
            //            drInc = IncomeTable.NewRow();
            //            FillTableValues(dr1, drInc, IncTypes, CommonEmtity);
            //        }
            //    }

            //    if (!string.IsNullOrEmpty(dr1["INCOME_DATE3"].ToString()))
            //    {
            //        if (Convert.ToDateTime(dr1["INCOME_DATE3"].ToString()) < MstIntakeStartDate || Convert.ToDateTime(dr1["INCOME_DATE3"].ToString()) > MstIntakeEndDate)
            //        {
            //        }
            //        else
            //        {
            //            drInc.SetField("SORT_INCOME_DATE", dr1["INCOME_DATE3"].ToString());
            //            if (dr1["INCOME_EXCLUDE"].ToString().Trim() == "Y")
            //            {
            //                drInc.SetField("SORT_INCOME_VAL", "0.00");
            //                drInc.SetField("SORT_INCOME_ACT_VAL", "0.00");
            //                drInc.SetField("SORT_INCOME_VER", dr1["INCOME_HOW_VERIFIED"].ToString() + " (exmp " + dr1["INCOME_VAL3"].ToString() + ")");
            //            }
            //            else
            //            {
            //                drInc.SetField("SORT_INCOME_VAL", dr1["INCOME_VAL3"].ToString());
            //                drInc.SetField("SORT_INCOME_ACT_VAL", dr1["INCOME_VAL3"].ToString());
            //                drInc.SetField("SORT_INCOME_VER", dr1["INCOME_HOW_VERIFIED"].ToString());
            //            }
            //            drInc.SetField("SORT_INCOME_SW", "Y");
            //            Sort_SW = "Y";
            //            IncomeTable.Rows.Add(drInc);
            //            drInc = IncomeTable.NewRow();
            //            FillTableValues(dr1, drInc, IncTypes, CommonEmtity);
            //        }
            //    }

            //    if (!string.IsNullOrEmpty(dr1["INCOME_DATE4"].ToString()))
            //    {
            //        if (Convert.ToDateTime(dr1["INCOME_DATE4"].ToString()) < MstIntakeStartDate || Convert.ToDateTime(dr1["INCOME_DATE4"].ToString()) > MstIntakeEndDate)
            //        {
            //        }
            //        else
            //        {
            //            drInc.SetField("SORT_INCOME_DATE", dr1["INCOME_DATE4"].ToString());
            //            if (dr1["INCOME_EXCLUDE"].ToString().Trim() == "Y")
            //            {
            //                drInc.SetField("SORT_INCOME_VAL", "0.00");
            //                drInc.SetField("SORT_INCOME_ACT_VAL", "0.00");
            //                drInc.SetField("SORT_INCOME_VER", dr1["INCOME_HOW_VERIFIED"].ToString() + " (exmp " + dr1["INCOME_VAL4"].ToString() + ")");
            //            }
            //            else
            //            {
            //                drInc.SetField("SORT_INCOME_VAL", dr1["INCOME_VAL4"].ToString());
            //                drInc.SetField("SORT_INCOME_ACT_VAL", dr1["INCOME_VAL4"].ToString());
            //                drInc.SetField("SORT_INCOME_VER", dr1["INCOME_HOW_VERIFIED"].ToString());
            //            }
            //            drInc.SetField("SORT_INCOME_SW", "Y");
            //            Sort_SW = "Y";
            //            IncomeTable.Rows.Add(drInc);
            //            drInc = IncomeTable.NewRow();
            //            FillTableValues(dr1, drInc, IncTypes, CommonEmtity);
            //        }
            //    }

            //    if (!string.IsNullOrEmpty(dr1["INCOME_DATE5"].ToString()))
            //    {
            //        if (Convert.ToDateTime(dr1["INCOME_DATE5"].ToString()) < MstIntakeStartDate || Convert.ToDateTime(dr1["INCOME_DATE5"].ToString()) > MstIntakeEndDate)
            //        {
            //        }
            //        else
            //        {
            //            drInc.SetField("SORT_INCOME_DATE", dr1["INCOME_DATE5"].ToString());
            //            if (dr1["INCOME_EXCLUDE"].ToString().Trim() == "Y")
            //            {
            //                drInc.SetField("SORT_INCOME_VAL", "0.00");
            //                drInc.SetField("SORT_INCOME_ACT_VAL", "0.00");
            //                drInc.SetField("SORT_INCOME_VER", dr1["INCOME_HOW_VERIFIED"].ToString() + " (exmp " + dr1["INCOME_VAL5"].ToString() + ")");
            //            }
            //            else
            //            {
            //                drInc.SetField("SORT_INCOME_VAL", dr1["INCOME_VAL5"].ToString());
            //                drInc.SetField("SORT_INCOME_ACT_VAL", dr1["INCOME_VAL5"].ToString());
            //                drInc.SetField("SORT_INCOME_VER", dr1["INCOME_HOW_VERIFIED"].ToString());
            //            }
            //            drInc.SetField("SORT_INCOME_SW", "Y");
            //            Sort_SW = "Y";
            //            IncomeTable.Rows.Add(drInc);
            //            drInc = IncomeTable.NewRow();
            //            FillTableValues(dr1, drInc, IncTypes, CommonEmtity);
            //        }
            //    }

            //    if (Sort_SW == "N")
            //    {
            //        drInc.SetField("SORT_INCOME_VAL", "0.00");
            //        drInc.SetField("SORT_INCOME_ACT_VAL", "0.00");
            //        drInc.SetField("SORT_INCOME_SEQ", "1");
            //        drInc.SetField("SORT_INCOME_SW", "Y");
            //        Sort_SW = "Y";
            //        IncomeTable.Rows.Add(drInc);
            //        //drInc = IncomeTable.NewRow();
            //        //FillTableValues(dr1, drInc, IncTypes, CommonEmtity);
            //    }

            //    //for (i = 1; i < 6; i++)
            //    //{
            //    //    if (!string.IsNullOrEmpty(dr1["INCOME_DATE" + i.ToString()].ToString()))
            //    //    {
            //    //        if (Convert.ToDateTime(dr1["INCOME_DATE" + i.ToString()].ToString()) < MstIntakeStartDate || Convert.ToDateTime(dr1["INCOME_DATE" + i.ToString()].ToString()) < MstIntakeEndDate)
            //    //        {
            //    //            dr1["INCOME_DATE" + i.ToString()] = string.Empty;
            //    //            dr1["INCOME_VAL" + i.ToString()] = "0.00";
            //    //        }

            //    //        drInc = dr1;
            //    //    }


            //    //}
            //}
            #endregion

        }

        private void Income_Check_For_OTHER_CUSTOMERS(DataTable dt, DataTable IncomeTable, DataTable IncTypes, List<CommonEntity> CommonEmtity)
        {
            string Sort_SW = "N";
            BaseForm.BaseCaseSnpEntity = BaseForm.BaseCaseSnpEntity.OrderBy(u => u.FamilySeq).ToList();
            foreach (CaseSnpEntity Entity in BaseForm.BaseCaseSnpEntity)
            {
                DataTable dtIncome = new DataTable();
                DataView dv = new DataView(dt);
                dv.RowFilter = "SNP_APP = '" + Entity.App.Trim() + "' AND INCOME_FAMILY_SEQ = '" + Entity.FamilySeq + "'";
                dtIncome = dv.ToTable();

                if (dtIncome.Rows.Count > 0)
                {
                    foreach (DataRow dr1 in dtIncome.Rows)
                    {
                        DataRow drInc = IncomeTable.NewRow();
                        FillTableValues(dr1, drInc, IncTypes, CommonEmtity);
                        

                        if (!string.IsNullOrEmpty(dr1["INCOME_DATE1"].ToString()) || (!string.IsNullOrEmpty(dr1["INCOME_VAL1"].ToString())))
                        {
                            if (!string.IsNullOrEmpty(dr1["INCOME_DATE1"].ToString()) || float.Parse(dr1["INCOME_VAL1"].ToString()) > 0)
                            {
                                if(!string.IsNullOrEmpty(dr1["INCOME_DATE1"].ToString().Trim()))
                                    drInc.SetField("SORT_INCOME_DATE", dr1["INCOME_DATE1"].ToString());
                                
                                if (dr1["INCOME_EXCLUDE"].ToString().Trim() == "Y")
                                {
                                    drInc.SetField("SORT_INCOME_VAL", "0.00");
                                    drInc.SetField("SORT_INCOME_ACT_VAL", "0.00");
                                    drInc.SetField("SORT_INCOME_VER", dr1["INCOME_HOW_VERIFIED"].ToString() + " (exmp " + dr1["INCOME_VAL1"].ToString() + ")");
                                }
                                else
                                {

                                    if (!string.IsNullOrEmpty(dr1["INCOME_VAL1"].ToString()))
                                    {
                                        drInc.SetField("SORT_INCOME_VAL", dr1["INCOME_VAL1"].ToString());
                                        drInc.SetField("SORT_INCOME_ACT_VAL", decimal.Parse(dr1["INCOME_VAL1"].ToString()) * decimal.Parse(dr1["INCOME_FACTOR"].ToString()));
                                    }
                                    else
                                    {
                                        drInc.SetField("SORT_INCOME_VAL", "0.00");
                                        drInc.SetField("SORT_INCOME_ACT_VAL", "0.00");
                                    }
                                    //drInc.SetField("SORT_INCOME_ACT_VAL", decimal.Parse(dr1["INCOME_VAL1"].ToString()) * decimal.Parse(dr1["INCOME_FACTOR"].ToString()));
                                    drInc.SetField("SORT_INCOME_VER", dr1["INCOME_HOW_VERIFIED"].ToString());
                                }
                                drInc.SetField("SORT_INCOME_SW", "Y");
                                Sort_SW = "Y";
                                IncomeTable.Rows.Add(drInc);
                                drInc = IncomeTable.NewRow();
                                FillTableValues(dr1, drInc, IncTypes, CommonEmtity);
                            }
                        }

                        if (!string.IsNullOrEmpty(dr1["INCOME_DATE2"].ToString()) || (!string.IsNullOrEmpty(dr1["INCOME_VAL2"].ToString())))
                        {
                            if (!string.IsNullOrEmpty(dr1["INCOME_DATE2"].ToString()) || float.Parse(dr1["INCOME_VAL2"].ToString()) > 0)
                            {
                                if (!string.IsNullOrEmpty(dr1["INCOME_DATE2"].ToString().Trim()))
                                    drInc.SetField("SORT_INCOME_DATE", dr1["INCOME_DATE2"].ToString());
                                if (dr1["INCOME_EXCLUDE"].ToString().Trim() == "Y")
                                {
                                    drInc.SetField("SORT_INCOME_VAL", "0.00");
                                    drInc.SetField("SORT_INCOME_ACT_VAL", "0.00");
                                    drInc.SetField("SORT_INCOME_VER", dr1["INCOME_HOW_VERIFIED"].ToString() + " (exmp " + dr1["INCOME_VAL2"].ToString() + ")");
                                }
                                else
                                {

                                    if (!string.IsNullOrEmpty(dr1["INCOME_VAL2"].ToString()))
                                    {
                                        drInc.SetField("SORT_INCOME_VAL", dr1["INCOME_VAL2"].ToString());
                                        drInc.SetField("SORT_INCOME_ACT_VAL", decimal.Parse(dr1["INCOME_VAL2"].ToString()) * decimal.Parse(dr1["INCOME_FACTOR"].ToString()));
                                    }
                                    else
                                    {
                                        drInc.SetField("SORT_INCOME_VAL", "0.00");
                                        drInc.SetField("SORT_INCOME_ACT_VAL", "0.00");
                                    }
                                    drInc.SetField("SORT_INCOME_VER", dr1["INCOME_HOW_VERIFIED"].ToString());
                                }
                                drInc.SetField("SORT_INCOME_SW", "Y");
                                Sort_SW = "Y";
                                IncomeTable.Rows.Add(drInc);
                                drInc = IncomeTable.NewRow();
                                FillTableValues(dr1, drInc, IncTypes, CommonEmtity);
                            }
                        }

                        if (!string.IsNullOrEmpty(dr1["INCOME_DATE3"].ToString()) || (!string.IsNullOrEmpty(dr1["INCOME_VAL3"].ToString())))
                        {
                            if (!string.IsNullOrEmpty(dr1["INCOME_DATE3"].ToString()) || float.Parse(dr1["INCOME_VAL3"].ToString()) > 0)
                            {
                                if (!string.IsNullOrEmpty(dr1["INCOME_DATE3"].ToString().Trim()))
                                    drInc.SetField("SORT_INCOME_DATE", dr1["INCOME_DATE3"].ToString());
                                if (dr1["INCOME_EXCLUDE"].ToString().Trim() == "Y")
                                {
                                    drInc.SetField("SORT_INCOME_VAL", "0.00");
                                    drInc.SetField("SORT_INCOME_ACT_VAL", "0.00");
                                    drInc.SetField("SORT_INCOME_VER", dr1["INCOME_HOW_VERIFIED"].ToString() + " (exmp " + dr1["INCOME_VAL3"].ToString() + ")");
                                }
                                else
                                {

                                    if (!string.IsNullOrEmpty(dr1["INCOME_VAL3"].ToString()))
                                    {
                                        drInc.SetField("SORT_INCOME_VAL", dr1["INCOME_VAL3"].ToString());
                                        drInc.SetField("SORT_INCOME_ACT_VAL", decimal.Parse(dr1["INCOME_VAL3"].ToString()) * decimal.Parse(dr1["INCOME_FACTOR"].ToString()));
                                    }
                                    else
                                    {
                                        drInc.SetField("SORT_INCOME_VAL", "0.00");
                                        drInc.SetField("SORT_INCOME_ACT_VAL", "0.00");
                                    }
                                    //drInc.SetField("SORT_INCOME_ACT_VAL", decimal.Parse(dr1["INCOME_VAL3"].ToString()) * decimal.Parse(dr1["INCOME_FACTOR"].ToString()));
                                    drInc.SetField("SORT_INCOME_VER", dr1["INCOME_HOW_VERIFIED"].ToString());
                                }
                                drInc.SetField("SORT_INCOME_SW", "Y");
                                Sort_SW = "Y";
                                IncomeTable.Rows.Add(drInc);
                                drInc = IncomeTable.NewRow();
                                FillTableValues(dr1, drInc, IncTypes, CommonEmtity);
                            }
                        }

                        if (!string.IsNullOrEmpty(dr1["INCOME_DATE4"].ToString()) || (!string.IsNullOrEmpty(dr1["INCOME_VAL4"].ToString())))
                        {
                            if (!string.IsNullOrEmpty(dr1["INCOME_DATE4"].ToString()) || float.Parse(dr1["INCOME_VAL4"].ToString()) > 0)
                            {
                                if (!string.IsNullOrEmpty(dr1["INCOME_DATE4"].ToString().Trim()))
                                    drInc.SetField("SORT_INCOME_DATE", dr1["INCOME_DATE4"].ToString());
                                if (dr1["INCOME_EXCLUDE"].ToString().Trim() == "Y")
                                {
                                    drInc.SetField("SORT_INCOME_VAL", "0.00");
                                    drInc.SetField("SORT_INCOME_ACT_VAL", "0.00");
                                    drInc.SetField("SORT_INCOME_VER", dr1["INCOME_HOW_VERIFIED"].ToString() + " (exmp " + dr1["INCOME_VAL4"].ToString() + ")");
                                }
                                else
                                {

                                    if (!string.IsNullOrEmpty(dr1["INCOME_VAL4"].ToString()))
                                    {
                                        drInc.SetField("SORT_INCOME_VAL", dr1["INCOME_VAL4"].ToString());
                                        drInc.SetField("SORT_INCOME_ACT_VAL", decimal.Parse(dr1["INCOME_VAL4"].ToString()) * decimal.Parse(dr1["INCOME_FACTOR"].ToString()));
                                    }
                                    else
                                    {
                                        drInc.SetField("SORT_INCOME_VAL", "0.00");
                                        drInc.SetField("SORT_INCOME_ACT_VAL", "0.00");
                                    }
                                    //drInc.SetField("SORT_INCOME_ACT_VAL", decimal.Parse(dr1["INCOME_VAL4"].ToString()) * decimal.Parse(dr1["INCOME_FACTOR"].ToString()));
                                    drInc.SetField("SORT_INCOME_VER", dr1["INCOME_HOW_VERIFIED"].ToString());
                                }
                                drInc.SetField("SORT_INCOME_SW", "Y");
                                Sort_SW = "Y";
                                IncomeTable.Rows.Add(drInc);
                                drInc = IncomeTable.NewRow();
                                FillTableValues(dr1, drInc, IncTypes, CommonEmtity);
                            }
                        }

                        if (!string.IsNullOrEmpty(dr1["INCOME_DATE5"].ToString()) || (!string.IsNullOrEmpty(dr1["INCOME_VAL5"].ToString())))
                        {
                            if (!string.IsNullOrEmpty(dr1["INCOME_DATE5"].ToString()) || float.Parse(dr1["INCOME_VAL5"].ToString()) > 0)
                            {
                                if (!string.IsNullOrEmpty(dr1["INCOME_DATE5"].ToString().Trim()))
                                    drInc.SetField("SORT_INCOME_DATE", dr1["INCOME_DATE5"].ToString());
                                if (dr1["INCOME_EXCLUDE"].ToString().Trim() == "Y")
                                {
                                    drInc.SetField("SORT_INCOME_VAL", "0.00");
                                    drInc.SetField("SORT_INCOME_ACT_VAL", "0.00");
                                    drInc.SetField("SORT_INCOME_VER", dr1["INCOME_HOW_VERIFIED"].ToString() + " (exmp " + dr1["INCOME_VAL5"].ToString() + ")");
                                }
                                else
                                {

                                    if (!string.IsNullOrEmpty(dr1["INCOME_VAL5"].ToString()))
                                    {
                                        drInc.SetField("SORT_INCOME_VAL", dr1["INCOME_VAL5"].ToString());
                                        drInc.SetField("SORT_INCOME_ACT_VAL", decimal.Parse(dr1["INCOME_VAL5"].ToString()) * decimal.Parse(dr1["INCOME_FACTOR"].ToString()));
                                    }
                                    else
                                    {
                                        drInc.SetField("SORT_INCOME_VAL", "0.00");
                                        drInc.SetField("SORT_INCOME_ACT_VAL", "0.00");
                                    }
                                    //drInc.SetField("SORT_INCOME_ACT_VAL", decimal.Parse(dr1["INCOME_VAL5"].ToString()) * decimal.Parse(dr1["INCOME_FACTOR"].ToString()));
                                    drInc.SetField("SORT_INCOME_VER", dr1["INCOME_HOW_VERIFIED"].ToString());
                                }
                                drInc.SetField("SORT_INCOME_SW", "Y");
                                Sort_SW = "Y";
                                IncomeTable.Rows.Add(drInc);
                                drInc = IncomeTable.NewRow();
                                FillTableValues(dr1, drInc, IncTypes, CommonEmtity);
                            }
                        }

                        if (Sort_SW == "N")
                        {
                            drInc.SetField("SORT_INCOME_VAL", "0.00");
                            drInc.SetField("SORT_INCOME_ACT_VAL", "0.00");
                            drInc.SetField("SORT_INCOME_SEQ", "1");
                            drInc.SetField("SORT_INCOME_SW", "Y");
                            Sort_SW = "Y";
                            IncomeTable.Rows.Add(drInc);
                            //drInc = IncomeTable.NewRow();
                            //FillTableValues(dr1, drInc, IncTypes, CommonEmtity);
                        }
                    }
                }
                else
                {
                    DataRow drInc = IncomeTable.NewRow();
                    FillTableValuesNoIncome(Entity, drInc);
                    IncomeTable.Rows.Add(drInc);
                }

            }

            #region Previous Code
            //foreach (DataRow dr1 in dt.Rows)
            //{
            //    DataRow drInc = IncomeTable.NewRow();
            //    FillTableValues(dr1, drInc, IncTypes, CommonEmtity);
            //    string Sort_SW = "N";

            //    if (!string.IsNullOrEmpty(dr1["INCOME_DATE1"].ToString()) || (!string.IsNullOrEmpty(dr1["INCOME_VAL1"].ToString()) ))
            //    {
            //        if (!string.IsNullOrEmpty(dr1["INCOME_DATE1"].ToString()) || float.Parse(dr1["INCOME_VAL1"].ToString()) > 0)
            //        {
            //            drInc.SetField("SORT_INCOME_DATE", dr1["INCOME_DATE1"].ToString());
            //            if (dr1["INCOME_EXCLUDE"].ToString().Trim() == "Y")
            //            {
            //                drInc.SetField("SORT_INCOME_VAL", "0.00");
            //                drInc.SetField("SORT_INCOME_ACT_VAL", "0.00");
            //                drInc.SetField("SORT_INCOME_VER", dr1["INCOME_HOW_VERIFIED"].ToString() + " (exmp " + dr1["INCOME_VAL1"].ToString() + ")");
            //            }
            //            else
            //            {
                            
            //                if (!string.IsNullOrEmpty(dr1["INCOME_VAL1"].ToString()))
            //                {
            //                    drInc.SetField("SORT_INCOME_VAL", dr1["INCOME_VAL1"].ToString());
            //                    drInc.SetField("SORT_INCOME_ACT_VAL", decimal.Parse(dr1["INCOME_VAL1"].ToString()) * decimal.Parse(dr1["INCOME_FACTOR"].ToString()));
            //                }
            //                else
            //                {
            //                    drInc.SetField("SORT_INCOME_VAL", "0.00");
            //                    drInc.SetField("SORT_INCOME_ACT_VAL", "0.00");
            //                }
            //                //drInc.SetField("SORT_INCOME_ACT_VAL", decimal.Parse(dr1["INCOME_VAL1"].ToString()) * decimal.Parse(dr1["INCOME_FACTOR"].ToString()));
            //                drInc.SetField("SORT_INCOME_VER", dr1["INCOME_HOW_VERIFIED"].ToString());
            //            }
            //            drInc.SetField("SORT_INCOME_SW", "Y");
            //            Sort_SW = "Y";
            //            IncomeTable.Rows.Add(drInc);
            //            drInc = IncomeTable.NewRow();
            //            FillTableValues(dr1, drInc, IncTypes, CommonEmtity);
            //        }
            //    }

            //    if (!string.IsNullOrEmpty(dr1["INCOME_DATE2"].ToString()) || (!string.IsNullOrEmpty(dr1["INCOME_VAL2"].ToString())))
            //    {
            //        if (!string.IsNullOrEmpty(dr1["INCOME_DATE2"].ToString()) || float.Parse(dr1["INCOME_VAL2"].ToString()) > 0)
            //        {
            //            drInc.SetField("SORT_INCOME_DATE", dr1["INCOME_DATE2"].ToString());
            //            if (dr1["INCOME_EXCLUDE"].ToString().Trim() == "Y")
            //            {
            //                drInc.SetField("SORT_INCOME_VAL", "0.00");
            //                drInc.SetField("SORT_INCOME_ACT_VAL", "0.00");
            //                drInc.SetField("SORT_INCOME_VER", dr1["INCOME_HOW_VERIFIED"].ToString() + " (exmp " + dr1["INCOME_VAL2"].ToString() + ")");
            //            }
            //            else
            //            {

            //                if (!string.IsNullOrEmpty(dr1["INCOME_VAL2"].ToString()))
            //                {
            //                    drInc.SetField("SORT_INCOME_VAL", dr1["INCOME_VAL2"].ToString());
            //                    drInc.SetField("SORT_INCOME_ACT_VAL", decimal.Parse(dr1["INCOME_VAL2"].ToString()) * decimal.Parse(dr1["INCOME_FACTOR"].ToString()));
            //                }
            //                else
            //                {
            //                    drInc.SetField("SORT_INCOME_VAL", "0.00");
            //                    drInc.SetField("SORT_INCOME_ACT_VAL", "0.00");
            //                }
            //                drInc.SetField("SORT_INCOME_VER", dr1["INCOME_HOW_VERIFIED"].ToString());
            //            }
            //            drInc.SetField("SORT_INCOME_SW", "Y");
            //            Sort_SW = "Y";
            //            IncomeTable.Rows.Add(drInc);
            //            drInc = IncomeTable.NewRow();
            //            FillTableValues(dr1, drInc, IncTypes, CommonEmtity);
            //        }
            //    }

            //    if (!string.IsNullOrEmpty(dr1["INCOME_DATE3"].ToString()) || (!string.IsNullOrEmpty(dr1["INCOME_VAL3"].ToString())))
            //    {
            //        if (!string.IsNullOrEmpty(dr1["INCOME_DATE3"].ToString()) || float.Parse(dr1["INCOME_VAL3"].ToString()) > 0)
            //        {
            //            drInc.SetField("SORT_INCOME_DATE", dr1["INCOME_DATE3"].ToString());
            //            if (dr1["INCOME_EXCLUDE"].ToString().Trim() == "Y")
            //            {
            //                drInc.SetField("SORT_INCOME_VAL", "0.00");
            //                drInc.SetField("SORT_INCOME_ACT_VAL", "0.00");
            //                drInc.SetField("SORT_INCOME_VER", dr1["INCOME_HOW_VERIFIED"].ToString() + " (exmp " + dr1["INCOME_VAL3"].ToString() + ")");
            //            }
            //            else
            //            {

            //                if (!string.IsNullOrEmpty(dr1["INCOME_VAL3"].ToString()))
            //                {
            //                    drInc.SetField("SORT_INCOME_VAL", dr1["INCOME_VAL3"].ToString());
            //                    drInc.SetField("SORT_INCOME_ACT_VAL", decimal.Parse(dr1["INCOME_VAL3"].ToString()) * decimal.Parse(dr1["INCOME_FACTOR"].ToString()));
            //                }
            //                else
            //                {
            //                    drInc.SetField("SORT_INCOME_VAL", "0.00");
            //                    drInc.SetField("SORT_INCOME_ACT_VAL", "0.00");
            //                }
            //                //drInc.SetField("SORT_INCOME_ACT_VAL", decimal.Parse(dr1["INCOME_VAL3"].ToString()) * decimal.Parse(dr1["INCOME_FACTOR"].ToString()));
            //                drInc.SetField("SORT_INCOME_VER", dr1["INCOME_HOW_VERIFIED"].ToString());
            //            }
            //            drInc.SetField("SORT_INCOME_SW", "Y");
            //            Sort_SW = "Y";
            //            IncomeTable.Rows.Add(drInc);
            //            drInc = IncomeTable.NewRow();
            //            FillTableValues(dr1, drInc, IncTypes, CommonEmtity);
            //        }
            //    }

            //    if (!string.IsNullOrEmpty(dr1["INCOME_DATE4"].ToString()) || (!string.IsNullOrEmpty(dr1["INCOME_VAL4"].ToString())))
            //    {
            //        if (!string.IsNullOrEmpty(dr1["INCOME_DATE4"].ToString()) || float.Parse(dr1["INCOME_VAL4"].ToString()) > 0)
            //        {
            //            drInc.SetField("SORT_INCOME_DATE", dr1["INCOME_DATE4"].ToString());
            //            if (dr1["INCOME_EXCLUDE"].ToString().Trim() == "Y")
            //            {
            //                drInc.SetField("SORT_INCOME_VAL", "0.00");
            //                drInc.SetField("SORT_INCOME_ACT_VAL", "0.00");
            //                drInc.SetField("SORT_INCOME_VER", dr1["INCOME_HOW_VERIFIED"].ToString() + " (exmp " + dr1["INCOME_VAL4"].ToString() + ")");
            //            }
            //            else
            //            {
                            
            //                if (!string.IsNullOrEmpty(dr1["INCOME_VAL4"].ToString()))
            //                {
            //                    drInc.SetField("SORT_INCOME_VAL", dr1["INCOME_VAL4"].ToString());
            //                    drInc.SetField("SORT_INCOME_ACT_VAL", decimal.Parse(dr1["INCOME_VAL4"].ToString()) * decimal.Parse(dr1["INCOME_FACTOR"].ToString()));
            //                }
            //                else
            //                {
            //                    drInc.SetField("SORT_INCOME_VAL", "0.00");
            //                    drInc.SetField("SORT_INCOME_ACT_VAL", "0.00");
            //                }
            //                //drInc.SetField("SORT_INCOME_ACT_VAL", decimal.Parse(dr1["INCOME_VAL4"].ToString()) * decimal.Parse(dr1["INCOME_FACTOR"].ToString()));
            //                drInc.SetField("SORT_INCOME_VER", dr1["INCOME_HOW_VERIFIED"].ToString());
            //            }
            //            drInc.SetField("SORT_INCOME_SW", "Y");
            //            Sort_SW = "Y";
            //            IncomeTable.Rows.Add(drInc);
            //            drInc = IncomeTable.NewRow();
            //            FillTableValues(dr1, drInc, IncTypes, CommonEmtity);
            //        }
            //    }

            //    if (!string.IsNullOrEmpty(dr1["INCOME_DATE5"].ToString()) || (!string.IsNullOrEmpty(dr1["INCOME_VAL5"].ToString())))
            //    {
            //        if (!string.IsNullOrEmpty(dr1["INCOME_DATE5"].ToString()) || float.Parse(dr1["INCOME_VAL5"].ToString()) > 0)
            //        {
            //            drInc.SetField("SORT_INCOME_DATE", dr1["INCOME_DATE5"].ToString());
            //            if (dr1["INCOME_EXCLUDE"].ToString().Trim() == "Y")
            //            {
            //                drInc.SetField("SORT_INCOME_VAL", "0.00");
            //                drInc.SetField("SORT_INCOME_ACT_VAL", "0.00");
            //                drInc.SetField("SORT_INCOME_VER", dr1["INCOME_HOW_VERIFIED"].ToString() + " (exmp " + dr1["INCOME_VAL5"].ToString() + ")");
            //            }
            //            else
            //            {

            //                if (!string.IsNullOrEmpty(dr1["INCOME_VAL5"].ToString()))
            //                {
            //                    drInc.SetField("SORT_INCOME_VAL", dr1["INCOME_VAL5"].ToString());
            //                    drInc.SetField("SORT_INCOME_ACT_VAL", decimal.Parse(dr1["INCOME_VAL5"].ToString()) * decimal.Parse(dr1["INCOME_FACTOR"].ToString()));
            //                }
            //                else
            //                {
            //                    drInc.SetField("SORT_INCOME_VAL", "0.00");
            //                    drInc.SetField("SORT_INCOME_ACT_VAL", "0.00");
            //                }
            //                //drInc.SetField("SORT_INCOME_ACT_VAL", decimal.Parse(dr1["INCOME_VAL5"].ToString()) * decimal.Parse(dr1["INCOME_FACTOR"].ToString()));
            //                drInc.SetField("SORT_INCOME_VER", dr1["INCOME_HOW_VERIFIED"].ToString());
            //            }
            //            drInc.SetField("SORT_INCOME_SW", "Y");
            //            Sort_SW = "Y";
            //            IncomeTable.Rows.Add(drInc);
            //            drInc = IncomeTable.NewRow();
            //            FillTableValues(dr1, drInc, IncTypes, CommonEmtity);
            //        }
            //    }

            //    if (Sort_SW == "N")
            //    {
            //        drInc.SetField("SORT_INCOME_VAL", "0.00");
            //        drInc.SetField("SORT_INCOME_ACT_VAL", "0.00");
            //        drInc.SetField("SORT_INCOME_SEQ", "1");
            //        drInc.SetField("SORT_INCOME_SW", "Y");
            //        Sort_SW = "Y";
            //        IncomeTable.Rows.Add(drInc);
            //        //drInc = IncomeTable.NewRow();
            //        //FillTableValues(dr1, drInc, IncTypes, CommonEmtity);
            //    }
            //}
            #endregion
        }

        private void Income_Check_For_OTHER_CUSTOMERS1(List<CaseIncomeEntity> caseIncomeList, DataTable IncomeTable, DataTable IncTypes, List<CommonEntity> CommonEmtity)
        {
            string Sort_SW = "N";
            BaseForm.BaseCaseSnpEntity = BaseForm.BaseCaseSnpEntity.OrderBy(u => u.FamilySeq).ToList();
            foreach (CaseSnpEntity Entity in BaseForm.BaseCaseSnpEntity)
            {
                if (Entity.Status == "A")
                {
                    List<CaseIncomeEntity> SelIncomeRecs = caseIncomeList.FindAll(u => u.App.Equals(Entity.App) && u.FamilySeq.Equals(Entity.FamilySeq));

                    if (SelIncomeRecs.Count > 0)
                    {
                        foreach (CaseIncomeEntity dr1 in SelIncomeRecs)
                        {
                            DataRow drInc = IncomeTable.NewRow();
                            FillTableValues1(dr1, drInc, IncTypes, CommonEmtity, Entity);


                            if (!string.IsNullOrEmpty(dr1.Date1.Trim()) || (!string.IsNullOrEmpty(dr1.Val1.Trim())))
                            {
                                if (!string.IsNullOrEmpty(dr1.Date1.Trim()) || float.Parse(dr1.Val1.Trim()) > 0)
                                {
                                    if (!string.IsNullOrEmpty(dr1.Date1.Trim()))
                                        drInc.SetField("SORT_INCOME_DATE", dr1.Date1.Trim());
                                    if (dr1.Exclude.Trim() == "Y")
                                    {
                                        drInc.SetField("SORT_INCOME_VAL", "0.00");
                                        drInc.SetField("SORT_INCOME_ACT_VAL", "0.00");
                                        drInc.SetField("SORT_INCOME_VER", dr1.HowVerified + " (exmp " + dr1.Val1.Trim() + ")");
                                    }
                                    else
                                    {

                                        if (!string.IsNullOrEmpty(dr1.Val1.Trim()))
                                        {
                                            drInc.SetField("SORT_INCOME_VAL", dr1.Val1.Trim());
                                            drInc.SetField("SORT_INCOME_ACT_VAL", decimal.Parse(dr1.Val1.Trim()) * decimal.Parse(dr1.Factor));
                                        }
                                        else
                                        {
                                            drInc.SetField("SORT_INCOME_VAL", "0.00");
                                            drInc.SetField("SORT_INCOME_ACT_VAL", "0.00");
                                        }
                                        //drInc.SetField("SORT_INCOME_ACT_VAL", decimal.Parse(dr1["INCOME_VAL1"].ToString()) * decimal.Parse(dr1["INCOME_FACTOR"].ToString()));
                                        drInc.SetField("SORT_INCOME_VER", dr1.HowVerified);
                                    }
                                    drInc.SetField("SORT_INCOME_SW", "Y");
                                    Sort_SW = "Y";
                                    IncomeTable.Rows.Add(drInc);
                                    drInc = IncomeTable.NewRow();
                                    FillTableValues1(dr1, drInc, IncTypes, CommonEmtity, Entity);
                                }
                            }

                            if (!string.IsNullOrEmpty(dr1.Date2.Trim()) || (!string.IsNullOrEmpty(dr1.Val2.Trim())))
                            {
                                if (!string.IsNullOrEmpty(dr1.Date2.Trim()) || float.Parse(dr1.Val2.Trim()) > 0)
                                {
                                    if (!string.IsNullOrEmpty(dr1.Date2.Trim()))
                                        drInc.SetField("SORT_INCOME_DATE", dr1.Date2.Trim());
                                    if (dr1.Exclude.Trim() == "Y")
                                    {
                                        drInc.SetField("SORT_INCOME_VAL", "0.00");
                                        drInc.SetField("SORT_INCOME_ACT_VAL", "0.00");
                                        drInc.SetField("SORT_INCOME_VER", dr1.HowVerified + " (exmp " + dr1.Val2.Trim() + ")");
                                    }
                                    else
                                    {

                                        if (!string.IsNullOrEmpty(dr1.Val2.Trim()))
                                        {
                                            drInc.SetField("SORT_INCOME_VAL", dr1.Val2.Trim());
                                            drInc.SetField("SORT_INCOME_ACT_VAL", decimal.Parse(dr1.Val2.Trim()) * decimal.Parse(dr1.Factor));
                                        }
                                        else
                                        {
                                            drInc.SetField("SORT_INCOME_VAL", "0.00");
                                            drInc.SetField("SORT_INCOME_ACT_VAL", "0.00");
                                        }
                                        drInc.SetField("SORT_INCOME_VER", dr1.HowVerified);
                                    }
                                    drInc.SetField("SORT_INCOME_SW", "Y");
                                    Sort_SW = "Y";
                                    IncomeTable.Rows.Add(drInc);
                                    drInc = IncomeTable.NewRow();
                                    FillTableValues1(dr1, drInc, IncTypes, CommonEmtity, Entity);
                                }
                            }

                            if (!string.IsNullOrEmpty(dr1.Date3.Trim()) || (!string.IsNullOrEmpty(dr1.Val3.Trim())))
                            {
                                if (!string.IsNullOrEmpty(dr1.Date3.Trim()) || float.Parse(dr1.Val3.Trim()) > 0)
                                {
                                    if (!string.IsNullOrEmpty(dr1.Date3.Trim()))
                                        drInc.SetField("SORT_INCOME_DATE", dr1.Date3.Trim());
                                    if (dr1.Exclude.Trim() == "Y")
                                    {
                                        drInc.SetField("SORT_INCOME_VAL", "0.00");
                                        drInc.SetField("SORT_INCOME_ACT_VAL", "0.00");
                                        drInc.SetField("SORT_INCOME_VER", dr1.HowVerified + " (exmp " + dr1.Val3.Trim() + ")");
                                    }
                                    else
                                    {

                                        if (!string.IsNullOrEmpty(dr1.Val3.Trim()))
                                        {
                                            drInc.SetField("SORT_INCOME_VAL", dr1.Val3.Trim());
                                            drInc.SetField("SORT_INCOME_ACT_VAL", decimal.Parse(dr1.Val3.Trim()) * decimal.Parse(dr1.Factor));
                                        }
                                        else
                                        {
                                            drInc.SetField("SORT_INCOME_VAL", "0.00");
                                            drInc.SetField("SORT_INCOME_ACT_VAL", "0.00");
                                        }
                                        //drInc.SetField("SORT_INCOME_ACT_VAL", decimal.Parse(dr1["INCOME_VAL3"].ToString()) * decimal.Parse(dr1["INCOME_FACTOR"].ToString()));
                                        drInc.SetField("SORT_INCOME_VER", dr1.HowVerified);
                                    }
                                    drInc.SetField("SORT_INCOME_SW", "Y");
                                    Sort_SW = "Y";
                                    IncomeTable.Rows.Add(drInc);
                                    drInc = IncomeTable.NewRow();
                                    FillTableValues1(dr1, drInc, IncTypes, CommonEmtity, Entity);
                                }
                            }

                            if (!string.IsNullOrEmpty(dr1.Date4.Trim()) || (!string.IsNullOrEmpty(dr1.Val4.Trim())))
                            {
                                if (!string.IsNullOrEmpty(dr1.Date4.Trim()) || float.Parse(dr1.Val4.Trim()) > 0)
                                {
                                    if (!string.IsNullOrEmpty(dr1.Date4.Trim()))
                                        drInc.SetField("SORT_INCOME_DATE", dr1.Date4.Trim());
                                    if (dr1.Exclude.Trim() == "Y")
                                    {
                                        drInc.SetField("SORT_INCOME_VAL", "0.00");
                                        drInc.SetField("SORT_INCOME_ACT_VAL", "0.00");
                                        drInc.SetField("SORT_INCOME_VER", dr1.HowVerified + " (exmp " + dr1.Val4.Trim() + ")");
                                    }
                                    else
                                    {

                                        if (!string.IsNullOrEmpty(dr1.Val4.Trim()))
                                        {
                                            drInc.SetField("SORT_INCOME_VAL", dr1.Val4.Trim());
                                            drInc.SetField("SORT_INCOME_ACT_VAL", decimal.Parse(dr1.Val4.Trim()) * decimal.Parse(dr1.Factor));
                                        }
                                        else
                                        {
                                            drInc.SetField("SORT_INCOME_VAL", "0.00");
                                            drInc.SetField("SORT_INCOME_ACT_VAL", "0.00");
                                        }
                                        //drInc.SetField("SORT_INCOME_ACT_VAL", decimal.Parse(dr1["INCOME_VAL4"].ToString()) * decimal.Parse(dr1["INCOME_FACTOR"].ToString()));
                                        drInc.SetField("SORT_INCOME_VER", dr1.HowVerified);
                                    }
                                    drInc.SetField("SORT_INCOME_SW", "Y");
                                    Sort_SW = "Y";
                                    IncomeTable.Rows.Add(drInc);
                                    drInc = IncomeTable.NewRow();
                                    FillTableValues1(dr1, drInc, IncTypes, CommonEmtity, Entity);
                                }
                            }

                            if (!string.IsNullOrEmpty(dr1.Date5.Trim()) || (!string.IsNullOrEmpty(dr1.Val5.Trim())))
                            {
                                if (!string.IsNullOrEmpty(dr1.Date5.Trim()) || float.Parse(dr1.Val5.Trim()) > 0)
                                {
                                    if (!string.IsNullOrEmpty(dr1.Date5.Trim()))
                                        drInc.SetField("SORT_INCOME_DATE", dr1.Date5.Trim());
                                    if (dr1.Exclude.Trim() == "Y")
                                    {
                                        drInc.SetField("SORT_INCOME_VAL", "0.00");
                                        drInc.SetField("SORT_INCOME_ACT_VAL", "0.00");
                                        drInc.SetField("SORT_INCOME_VER", dr1.HowVerified + " (exmp " + dr1.Val5.Trim() + ")");
                                    }
                                    else
                                    {

                                        if (!string.IsNullOrEmpty(dr1.Val5.Trim()))
                                        {
                                            drInc.SetField("SORT_INCOME_VAL", dr1.Val5.Trim());
                                            drInc.SetField("SORT_INCOME_ACT_VAL", decimal.Parse(dr1.Val5.Trim()) * decimal.Parse(dr1.Factor));
                                        }
                                        else
                                        {
                                            drInc.SetField("SORT_INCOME_VAL", "0.00");
                                            drInc.SetField("SORT_INCOME_ACT_VAL", "0.00");
                                        }
                                        //drInc.SetField("SORT_INCOME_ACT_VAL", decimal.Parse(dr1["INCOME_VAL5"].ToString()) * decimal.Parse(dr1["INCOME_FACTOR"].ToString()));
                                        drInc.SetField("SORT_INCOME_VER", dr1.HowVerified);
                                    }
                                    drInc.SetField("SORT_INCOME_SW", "Y");
                                    Sort_SW = "Y";
                                    IncomeTable.Rows.Add(drInc);
                                    drInc = IncomeTable.NewRow();
                                    FillTableValues1(dr1, drInc, IncTypes, CommonEmtity, Entity);
                                }
                            }

                            if (string.IsNullOrEmpty(dr1.Val1.Trim()) && string.IsNullOrEmpty(dr1.Val2.Trim()) && string.IsNullOrEmpty(dr1.Val3.Trim()) && string.IsNullOrEmpty(dr1.Val4.Trim()) && (string.IsNullOrEmpty(dr1.Val5.Trim())))
                            {
                                drInc.SetField("SORT_INCOME_VAL", "0.00");
                                drInc.SetField("SORT_INCOME_ACT_VAL", "0.00");
                                drInc.SetField("SORT_INCOME_SEQ", "1");
                                drInc.SetField("SORT_INCOME_SW", "Y");
                                drInc.SetField("SORT_INCOME_VER", dr1.HowVerified);
                                Sort_SW = "Y";
                                IncomeTable.Rows.Add(drInc);
                            }

                            if (Sort_SW == "N")
                            {
                                drInc.SetField("SORT_INCOME_VAL", "0.00");
                                drInc.SetField("SORT_INCOME_ACT_VAL", "0.00");
                                drInc.SetField("SORT_INCOME_SEQ", "1");
                                drInc.SetField("SORT_INCOME_SW", "Y");
                                drInc.SetField("SORT_INCOME_VER", dr1.HowVerified);
                                Sort_SW = "Y";
                                IncomeTable.Rows.Add(drInc);
                                //drInc = IncomeTable.NewRow();
                                //FillTableValues(dr1, drInc, IncTypes, CommonEmtity);
                            }

                        }
                    }
                    else
                    {
                        //if (Entity.Status.Trim() == "A")
                        //{
                        //    DataRow drInc = IncomeTable.NewRow();
                        //    FillTableValuesNoIncome(Entity, drInc);
                        //    IncomeTable.Rows.Add(drInc);
                        //}
                    }
                }

            }

           
        }

        private void FillTableValues(DataRow drentity, DataRow dr, DataTable IncTypeTable, List<CommonEntity> commonEntity)
        {
            dr.SetField("SORT_APP_NO", drentity["SNP_APP"].ToString());
            dr.SetField("SORT_FAMILY_SEQ", drentity["INCOME_FAMILY_SEQ"].ToString());
            dr.SetField("SORT_MEMBER_NAME", LookupDataAccess.GetMemberName(drentity["FName"].ToString(), drentity["MName"].ToString(), drentity["LName"].ToString(), BaseForm.BaseHierarchyCnFormat));
            dr.SetField("SORT_MEMBER_SSN", drentity["SNP_SSNO"].ToString());
            dr.SetField("SORT_MEMBER_DOB", drentity["SNP_ALT_BDATE"].ToString());
            dr.SetField("SORT_INCOME_TYPE", drentity["INCOME_TYPE"].ToString());
            if (IncTypeTable.Rows.Count > 0)
            {
                foreach (DataRow drInc in IncTypeTable.Rows)
                {
                    if (drentity["INCOME_TYPE"].ToString() == drInc["AGY_2"].ToString().Trim())
                    {
                        dr.SetField("SORT_INCOME_TYPE_LIT", drInc["AGY_8"].ToString());
                        dr.SetField("SORT_NEGATIVE", drInc["AGY_1"].ToString());
                        //SORT_NEGATIVE = drInc["AGY_1"].ToString();
                        break;
                    }
                }
            }

            dr.SetField("SORT_INCOME_SOURCE", drentity["INCOME_SOURCE"].ToString());
            dr.SetField("PRT_SORT_INCOME_SOURCE", drentity["INCOME_SOURCE"].ToString());

            foreach (CommonEntity interval in commonEntity)
            {
                if (drentity["INCOME_INTERVAL"].ToString().Trim() == interval.Code)
                {
                    if (BaseForm.BaseCaseMstListEntity[0].FamilySeq == drentity["INCOME_FAMILY_SEQ"].ToString().Trim())
                        dr.SetField("SORT_INTERVAL", interval.Desc);
                    else
                        dr.SetField("SORT_INTERVAL", interval.Desc);
                    break;
                }
            }
            dr.SetField("SORT_EXCLUDE_INCOME", drentity["INCOME_EXCLUDE"].ToString());

            //dr.SetField("FName", drentity["SNP_NAME_IX_FI"].ToString());
            //dr.SetField("LName", drentity["SNP_NAME_IX_LAST"].ToString());
            //dr.SetField("MName", drentity["SNP_NAME_IX_MI"].ToString());
            //dr.SetField("EMSRES_Amount", drentity["EMSRES_AMOUNT"].ToString());
            //dr.SetField("PMC_Amount", drentity["PMC_AMOUNT"].ToString());
            //dr.SetField("PMC_Count", "0");
            //dr.SetField("Preset_amount", "0.00");
            ////dr.SetField("Bal_Amount", drentity["PMC_AMOUNT"].ToString());
            //dr.SetField("CLC_S_CGN", drentity["CLC_S_CGN"].ToString());
            //dr.SetField("Decission", drentity["CLC_S_DECISION"].ToString());
            //dr.SetField("Decission_Date", drentity["CLC_S_DECSN_DATE"].ToString());
            //dr.SetField("PMC_Date", drentity["PMC_DATE"].ToString());
            //dr.SetField("Case_type", drentity["MST_CASE_TYPE"].ToString());
            //dr.SetField("Vendor", drentity["CLC_S_VENDOR"].ToString());
            //dr.SetField("Add_Operator", drentity["CLC_ADD_OPERATOR"].ToString());
        }

        private void FillTableValues1(CaseIncomeEntity drentity, DataRow dr, DataTable IncTypeTable, List<CommonEntity> commonEntity,CaseSnpEntity Entity)
        {
            dr.SetField("SORT_APP_NO", Entity.App.ToString());
            dr.SetField("SORT_FAMILY_SEQ", Entity.FamilySeq);
            dr.SetField("SORT_MEMBER_NAME", LookupDataAccess.GetMemberName(Entity.NameixFi.Trim(), Entity.NameixMi.Trim(), Entity.NameixLast.Trim(), BaseForm.BaseHierarchyCnFormat));
            dr.SetField("SORT_MEMBER_SSN", Entity.Ssno);
            dr.SetField("SORT_MEMBER_DOB", Entity.AltBdate);
            dr.SetField("SORT_INCOME_TYPE", drentity.Type);
            if (IncTypeTable.Rows.Count > 0)
            {
                foreach (DataRow drInc in IncTypeTable.Rows)
                {
                    if (drentity.Type == drInc["AGY_2"].ToString().Trim())
                    {
                        dr.SetField("SORT_INCOME_TYPE_LIT", drInc["AGY_8"].ToString());
                        dr.SetField("SORT_NEGATIVE", drInc["AGY_1"].ToString());
                        //SORT_NEGATIVE = drInc["AGY_1"].ToString();
                        break;
                    }
                }
            }

            dr.SetField("SORT_INCOME_SOURCE", drentity.Source);
            dr.SetField("PRT_SORT_INCOME_SOURCE", drentity.Source);

            foreach (CommonEntity interval in commonEntity)
            {
                if (drentity.Interval.Trim() == interval.Code)
                {
                    if (BaseForm.BaseCaseMstListEntity[0].FamilySeq == drentity.FamilySeq.Trim())
                        dr.SetField("SORT_INTERVAL", interval.Desc);
                    else
                        dr.SetField("SORT_INTERVAL", interval.Desc);
                    break;
                }
            }
            dr.SetField("SORT_EXCLUDE_INCOME", drentity.Exclude);

            //dr.SetField("FName", drentity["SNP_NAME_IX_FI"].ToString());
            //dr.SetField("LName", drentity["SNP_NAME_IX_LAST"].ToString());
            //dr.SetField("MName", drentity["SNP_NAME_IX_MI"].ToString());
            //dr.SetField("EMSRES_Amount", drentity["EMSRES_AMOUNT"].ToString());
            //dr.SetField("PMC_Amount", drentity["PMC_AMOUNT"].ToString());
            //dr.SetField("PMC_Count", "0");
            //dr.SetField("Preset_amount", "0.00");
            ////dr.SetField("Bal_Amount", drentity["PMC_AMOUNT"].ToString());
            //dr.SetField("CLC_S_CGN", drentity["CLC_S_CGN"].ToString());
            //dr.SetField("Decission", drentity["CLC_S_DECISION"].ToString());
            //dr.SetField("Decission_Date", drentity["CLC_S_DECSN_DATE"].ToString());
            //dr.SetField("PMC_Date", drentity["PMC_DATE"].ToString());
            //dr.SetField("Case_type", drentity["MST_CASE_TYPE"].ToString());
            //dr.SetField("Vendor", drentity["CLC_S_VENDOR"].ToString());
            //dr.SetField("Add_Operator", drentity["CLC_ADD_OPERATOR"].ToString());
        }

        private void FillTableValuesNoIncome(CaseSnpEntity Entity, DataRow dr)
        {
            dr.SetField("SORT_APP_NO", Entity.App);
            dr.SetField("SORT_FAMILY_SEQ", Entity.FamilySeq);
            dr.SetField("SORT_MEMBER_NAME", LookupDataAccess.GetMemberName(Entity.NameixFi.Trim(), Entity.NameixMi.Trim(), Entity.NameixLast.Trim(), BaseForm.BaseHierarchyCnFormat));
            dr.SetField("SORT_MEMBER_SSN", Entity.Ssno);
            dr.SetField("SORT_MEMBER_DOB", Entity.AltBdate);
            dr.SetField("SORT_INCOME_TYPE", "N");
            dr.SetField("SORT_INCOME_TYPE_LIT", "NONE");
            dr.SetField("SORT_NEGATIVE", "");
            
            dr.SetField("SORT_INCOME_SOURCE", "NONE");
            dr.SetField("PRT_SORT_INCOME_SOURCE", "NONE");
            dr.SetField("SORT_INTERVAL", "");

            dr.SetField("SORT_EXCLUDE_INCOME", "");

            dr.SetField("SORT_INCOME_VAL", "0.00");
            dr.SetField("SORT_INCOME_ACT_VAL", "0.00");
            dr.SetField("SORT_INCOME_SEQ", "1");
            dr.SetField("SORT_INCOME_SW", "Y");

            //dr.SetField("FName", drentity["SNP_NAME_IX_FI"].ToString());
            //dr.SetField("LName", drentity["SNP_NAME_IX_LAST"].ToString());
            //dr.SetField("MName", drentity["SNP_NAME_IX_MI"].ToString());
            //dr.SetField("EMSRES_Amount", drentity["EMSRES_AMOUNT"].ToString());
            //dr.SetField("PMC_Amount", drentity["PMC_AMOUNT"].ToString());
            //dr.SetField("PMC_Count", "0");
            //dr.SetField("Preset_amount", "0.00");
            ////dr.SetField("Bal_Amount", drentity["PMC_AMOUNT"].ToString());
            //dr.SetField("CLC_S_CGN", drentity["CLC_S_CGN"].ToString());
            //dr.SetField("Decission", drentity["CLC_S_DECISION"].ToString());
            //dr.SetField("Decission_Date", drentity["CLC_S_DECSN_DATE"].ToString());
            //dr.SetField("PMC_Date", drentity["PMC_DATE"].ToString());
            //dr.SetField("Case_type", drentity["MST_CASE_TYPE"].ToString());
            //dr.SetField("Vendor", drentity["CLC_S_VENDOR"].ToString());
            //dr.SetField("Add_Operator", drentity["CLC_ADD_OPERATOR"].ToString());
        }

    }
}