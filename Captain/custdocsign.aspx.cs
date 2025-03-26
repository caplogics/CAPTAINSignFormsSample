using Captain.Common.Model.Data;
using Captain.Common.Utilities;
using Captain.Common.Views.Forms.Base;
using Captain.DatabaseLayer;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;
using Twilio.Http;
using Wisej.Web;

namespace Captain
{
    public partial class custdocsign : System.Web.UI.Page
    {
        public static string strDBConnection = "";
        public static string QueryString = "";
        static List<SignParams> _signParams = new List<SignParams>();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //string encryptCode1 = "7ttQQuNmHW381WRErspvYRnixt6qWXoNi1FnVSZzwxfllDmuLWZlMNAVjhjUzfl0aRZZl3QLypqy0SMQzuTBpLIQtAAKCQqFpb77iJbMkV1h5K7RuXFH0Bai2k9mA6MYM%2byo%2fgzmfM62WCqPRoBgKB76cga34YbL7GjIQGiwlsPtL3StvispoC8jWisEJoqELXL1iV2XoUdxTlv278FlFp%2f06KHGkomyODYRlZV%2bOSLbAGaEjHWtzyEks%2bOUcH2n9dnsbFy7OzZqEUXqXpAaHWJBLvzwFBZ2JJzjeNlmVj1We8YjEnp%2fEqPVGm99OY1Ml6WaMGyg9ltDDOE0Xx5o1DHVABq9VL2lt2XBinwdgKWLdanZRYML3uhjQF1crjTN7VlwMx8HSRXp2bCF2Nd2uQQzrdDWCPe8DmmJVY18xoDH705RNO2TGvJUbTZZfDUNQEuYxqdQK%2bhF5n8W3Ztm0GVbep37z%2fuTLNtrBpdlXqj3nQb36nvzRaC9CcoVk3CUtBbRILVNDoxyZyFLzuyIoPAdRF0rq0t7%2foLteWeskVKcGQW%2brZJsljhoz6MUkbw4HEw8ckyiq1HgsBLIDHVgT8RcENoSdZYV6RxUqVEPD8Y%3d";
                //_signParams = WM_DecryptUrl(encryptCode1);
                if (!IsPostBack)
                {
                    try
                    {

                        if (Request.QueryString["idparms"] != null)
                        {
                            string encryptCode = Request.QueryString["idparms"].ToString();

                            

                            //byte[] strBytes = WM_bytesDecryptUrl(encryptCode);
                            //string strParamsCode = Encoding.ASCII.GetString(strBytes);
                            //_signParams = WM_DecryptUrl(strParamsCode);

                            _signParams = WM_DecryptUrl(encryptCode);

                            //    if (AlienTXDB.CHECKSIGNDOCS(_signParams[0].baseAgency, _signParams[0].baseDept, _signParams[0].baseProg,
                            //_signParams[0].baseYear, _signParams[0].baseApplicationNo, "CHKSIGNDOCS"))
                            if (checkfrmValidation())
                            {
                                string strAppno = _signParams[0].HIEAppno; //Request.QueryString["appno"].ToString();
                                string strDBName = _signParams[0].baseAgyShortName; //Request.QueryString["dbname"].ToString();

                                strDBConnection = System.Configuration.ConfigurationManager.ConnectionStrings["CMMSo"].ConnectionString.ToString();
                                //Session["Appno"]=strAppno;
                                //Session["DBShortName"] = strDBName;
                                hdnAppno.Value = strAppno;
                                hdnDBShortname.Value = strDBName;

                                string AgencyCode = "00";

                                string logopath = "http://capsysdev.capsystems.com/images/PIPlogos/" + strDBName.ToUpper() + "_" + AgencyCode.ToString() + "_LOGO.png";
                                mainlogo.ImageUrl = logopath;
                                //mainlogo.Width = 300;

                            }
                            else
                            {
                                Response.Redirect("~/error.aspx");
                            }
                        }
                        else
                        {
                            //Response.Redirect("~/index.aspx?Agy=APC&typ=Rep");
                            Page.Response.Redirect(Page.Request.Url.ToString(), true);
                        }
                    }
                    catch (NullReferenceException ex)
                    {
                        Response.Redirect("~/error.aspx");
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                //lblappTitle.Text = msg;
                Response.Redirect("~/error.aspx");
            }
        }

        static bool checkfrmValidation()
        {
            bool _flag = true;
            DataTable dt = AlienTXDB.GET_DOCSIGNHIS(_signParams[0].baseAgency, _signParams[0].baseDept, _signParams[0].baseProg,
                       _signParams[0].baseYear, _signParams[0].baseApplicationNo, "", "", "N", "GET");

            if (dt.Rows.Count > 0)
            {
                DataRow[] drSign = dt.Select("DCSN_HIS_SIGN_REQ='Y' AND DCSN_HIS_SIGND_ON IS NOT NULL AND DCSN_HIS_DOWNLOAD IS NOT NULL");
                DataRow[] drdownload = dt.Select("DCSN_HIS_SIGN_REQ='N' AND DCSN_HIS_DOWNLOAD IS NOT NULL");

                int cntDocs = drSign.Length + drdownload.Length;
                if (dt.Rows.Count == cntDocs)
                    _flag = false;
            }


            return _flag;
        }

        [WebMethod]
        public static string WM_getAllFilesfromFolder(string Appno)
        {
            string Result = string.Empty;
            try
            {
                CaptainModel _model = new CaptainModel();
                string propReportPath = _model.lookupDataAccess.GetServerReportPath();

                string strImageFolderName = propReportPath + "//SignForms//" + Appno + "//"; //HttpContext.Current.Server.MapPath("~/custdocsignpdfs/" + Appno + "/");
                string[] filePaths = Directory.GetFiles(strImageFolderName, "*.pdf", SearchOption.AllDirectories);

                if (filePaths.Length > 0)
                    Result = JsonConvert.SerializeObject(filePaths);
                else
                    Result = JsonConvert.SerializeObject(null);
            }
            catch (Exception)
            {
                throw;
            }
            return Result;
        }
        [WebMethod]
        public static string WM_getAllFilesfromDB(string Appno)
        {
            string Result = string.Empty;
            try
            {
                CaptainModel _model = new CaptainModel();
                string propReportPath = _model.lookupDataAccess.GetServerReportPath();

                string strImageFolderName = propReportPath + "//SignForms//" + Appno + "//"; //HttpContext.Current.Server.MapPath("~/custdocsignpdfs/" + Appno + "/");
                // string[] filePaths = Directory.GetFiles(strImageFolderName, "*.pdf", SearchOption.AllDirectories);

                DataTable dt = AlienTXDB.GET_DOCSIGNHIS(_signParams[0].baseAgency, _signParams[0].baseDept, _signParams[0].baseProg,
                        _signParams[0].baseYear, _signParams[0].baseApplicationNo, "", "", "N", "GET");

                DataTable dtRes = new DataTable();
                if (dt.Rows.Count > 0)
                {
                    DataRow[] dr = dt.Select("DCSN_PRINT_FOR='S'");
                    if (dr.Length > 0)
                    {
                        DataTable dtR = dr.CopyToDataTable();
                        dtR.DefaultView.Sort = "DCSN_HIS_ADD_DATE DESC";
                        dtR = dtR.DefaultView.ToTable();

                        dtRes = dtR;
                    }
                }
                //string[] filePaths = new string[] { };
                //List<string> strlst = new List<string>();
                //if (dt.Rows.Count > 0)
                //{
                //    int x = 0;
                //    foreach(DataRow dr in dt.Rows)
                //    {
                //        strlst.Add(dr["DCSN_HIS_DOC_NAME"].ToString());
                //    }
                //   filePaths = strlst.ToArray();
                //}


                if (dtRes.Rows.Count > 0)
                    Result = JsonConvert.SerializeObject(dtRes);
                else
                    Result = JsonConvert.SerializeObject(null);
            }
            catch (Exception)
            {
                throw;
            }
            return Result;
        }
        [WebMethod]
        public static string WM_getPath(string filepath, string fileName)
        {
            string strRootPath = "";
            try
            {
                CaptainModel _model = new CaptainModel();
                string propReportPath = _model.lookupDataAccess.GetServerReportPath();
                if (filepath != "")
                {
                    string path = propReportPath + filepath;
                    path = path.Replace("/", "\\");
                    path = path.Replace("///", "/");
                    string sitepath = HttpContext.Current.Request.Url.ToString();
                    string[] siteArray = sitepath.Split(new char[] { '/' });
                    string SiteHomeFolder = siteArray[3];
                    if (SiteHomeFolder == "custdocsign.aspx")
                        strRootPath = @"/ViewPdfForm.aspx?Name=" + path;
                    else
                        strRootPath = @"/" + SiteHomeFolder + "/ViewPdfForm.aspx?Name=" + path;
                }
                else
                    strRootPath = propReportPath;

            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return strRootPath;
        }
        //public static string GetSiteUrl()
        //{
        //    string url = string.Empty;
        //    url = Application.Url;
        //    return url;

        //}

        [WebMethod]
        public static string WM_VerifySIGN(string oImage)
        {
            string result = "fail";
            try
            {
                string strImage = oImage;
                strImage = strImage.Replace("data:image/png;base64,", "");
                byte[] bytes = Convert.FromBase64String(strImage);

                if (bytes.Length > 5218)
                {
                    result = "success";
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }

        [WebMethod]
        public static string WM_TakeSIGN(string HISID, string Appno, string oImage, string odocPath, string odocCode, string docMode, string docName)
        {
            string SignDoc = "";
            string result = string.Empty;
            try
            {
                string strImage = oImage;
                strImage = strImage.Replace("data:image/png;base64,", "");
                byte[] bytes = Convert.FromBase64String(strImage);

                if (bytes.Length > 5218)
                {
                    CaptainModel _model = new CaptainModel();
                    string propReportPath = _model.lookupDataAccess.GetServerReportPath();
                    odocPath = propReportPath + odocPath; //HttpContext.Current.Server.MapPath(odocPath);


                    if (bytes.Length > 5218)
                    {
                        string folderPath = propReportPath + "//SignForms//Signatures//"; //HttpContext.Current.Server.MapPath("~/custdocsignpdfs/Signatures/");  //WebConfigurationManager.AppSettings["SIGNPATH"];  //Create a Folder in your Root directory on your solution.
                        if (!Directory.Exists(folderPath))
                            Directory.CreateDirectory(folderPath);

                        int count = Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories).Length;
                        string fileName = Appno + ".jpg";
                        string imagePath = folderPath + fileName;

                        File.WriteAllBytes(imagePath, bytes);
                        SignDoc = fileName;

                        if (docMode == "SD")
                        {

                            //PrintSignonDoc(odocPath, imagePath, 100, 120);
                            //string encryptCode = HttpContext.Current.Request.QueryString["idparms"].ToString();
                            //_signParams = WM_DecryptUrl(encryptCode);

                            SignPDFGeneration oSignPDFGeneration = new SignPDFGeneration(_signParams[0].baseUSERID, _signParams[0].baseAgency, _signParams[0].baseDept, _signParams[0].baseProg,
                            _signParams[0].baseYear, _signParams[0].baseApplicationNo, _signParams[0].baseApplicationName, _signParams[0].baseAgyShortName.ToUpper(), false, imagePath,
                            "SD", odocCode, "", docName.Split('.')[0], ""); //SD - Sign on Document
                            oSignPDFGeneration.PrintAlienFormPDF();
                            /**************************************************************/
                            /**************************************************************/

                            AlienTXData _oAlienTXData = new AlienTXData();
                            bool _isSaveFlag = _oAlienTXData.INSUPDEL_DOCSIGNHIS(HISID, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "","","", "SIGNED");
                            if (_isSaveFlag)
                                result = "success";

                            /**************************************************************/
                            /**************************************************************/

                          

                            //SignPDFGeneration oSignPDFGeneration = new SignPDFGeneration(_signParams[0].baseprivileges, _signParams[0].baseAgyTabsEntity, _signParams[0].baseCaseMstListEntity,
                            //    _signParams[0].baseCaseSnpEntity, _signParams[0].baseAgencyControlDetails, _signParams[0].baseUserProfile, _signParams[0].baseApplicationNo, _signParams[0].baseApplicationName,
                            //    _signParams[0].baseUSERID, _signParams[0].baseAgency, _signParams[0].baseDept, _signParams[0].baseProg, _signParams[0].baseYear, _signParams[0].baseAgyShortName, false, imagePath, "SD"); //SD - Sign on Document

                            //oSignPDFGeneration.PrintAlienFormPDF();

                            //if (odocPath.Contains("Client intake and Alien form"))
                            //{
                            //    PrintSignonDoc(odocPath, imagePath, 100, 120);
                            //}
                            //else if (odocPath.Contains("Client intake"))
                            //{
                            //    PrintSignonDoc(odocPath, imagePath, 100, 120);
                            //}
                            //else if (odocPath.Contains("TX_Alien_Form"))
                            //{
                            //    PrintSignonDoc(odocPath, imagePath, 45, 110);
                            //}
                        }
                    }
                    else
                    {
                        result = "fail";
                    }

                    //if (objCapsystems.APRSIGN_INSUPDEL(SignatureID, ReqID, SignDate, SignName, SignDoc, HttpContext.Current.Session["UserID"].ToString(), Mode))
                    //{

                    //}
                }
                else
                {
                    result = "fail";
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }

        public static void PrintSignonDoc(string pdfpath, string signImage, int Xaxis, int Yaxis)
        {
            try
            {
                BaseFont bf_times = BaseFont.CreateFont("c:/windows/fonts/Calibri.TTF", BaseFont.WINANSI, BaseFont.EMBEDDED);
                iTextSharp.text.Font TblFontBoldColor = new iTextSharp.text.Font(bf_times, 12, 1, BaseColor.BLACK);

                PdfReader Hreader = new PdfReader(System.IO.File.ReadAllBytes(pdfpath));
                PdfStamper Hstamper = new PdfStamper(Hreader, new FileStream(pdfpath, FileMode.Create, FileAccess.Write));
                Hstamper.Writer.SetPageSize(PageSize.A4);
                int pageNumber = 1;
                PdfContentByte cb = Hstamper.GetOverContent(pageNumber);

                // ColumnText.ShowTextAligned(cb, PdfContentByte.ALIGN_LEFT, new Phrase("Kranthi", TblFontBoldColor), Xaxis, Yaxis, 0);
                Stream inputImageStream = new FileStream(signImage, FileMode.Open, FileAccess.Read, FileShare.Read);
                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(inputImageStream);
                image.SetAbsolutePosition(Xaxis, Yaxis);
                image.ScaleToFit(95, 80); // Adjust these values as needed
                cb.AddImage(image);


                Hstamper.Close();
            }
            catch (Exception ex)
            {

            }
        }

        [WebMethod]
        public static string WM_getDocsHistory(string Appno)
        {
            string Result = string.Empty;
            try
            {
                DataTable dt = AlienTXDB.GET_DOCSIGNHIS(_signParams[0].baseAgency, _signParams[0].baseDept, _signParams[0].baseProg,
                        _signParams[0].baseYear, _signParams[0].baseApplicationNo, "", "", "N", "GET");

                DataTable dtRes = new DataTable();
                if (dt.Rows.Count > 0)
                {
                    DataRow[] dr = dt.Select("DCSN_PRINT_FOR='S'");
                    if (dr.Length > 0)
                    {
                        dtRes = dr.CopyToDataTable();
                    }
                }
                if (dtRes.Rows.Count > 0)
                    Result = JsonConvert.SerializeObject(dtRes);
                else
                    Result = JsonConvert.SerializeObject(null);
            }
            catch (Exception)
            {
                throw;
            }
            return Result;
        }
        public static List<SignParams> WM_DecryptUrl(string ourl)
        {
            List<SignParams> result = new List<SignParams>();
            try
            {
                //ourl = ourl.Remove(0, 1);
                // ourl = ourl.Remove(ourl.Length - 1);

                string _decrypturl = AlienTXDB.Decrypt(HttpUtility.UrlDecode(ourl));

                JavaScriptSerializer json = new JavaScriptSerializer();
                List<SignParams> objArray = json.Deserialize<List<SignParams>>(_decrypturl);

                result = objArray;

            }
            catch (Exception ex)
            {
                //result = ex.Message;
            }
            return result;
        }
        public static byte[] WM_bytesDecryptUrl(string ourl)
        {
            byte[] result = null;
            try
            {
                //ourl = ourl.Remove(0, 1);
                // ourl = ourl.Remove(ourl.Length - 1);

                string _decrypturl = AlienTXDB.Decrypt(HttpUtility.UrlDecode(ourl));

                JavaScriptSerializer json = new JavaScriptSerializer();
                byte[] objArray = json.Deserialize<byte[]>(_decrypturl);

                result = objArray;

            }
            catch (Exception ex)
            {
                //result = ex.Message;
            }
            return result;
        }
        protected void rbtnSigndoc_CheckedChanged(object sender, EventArgs e)
        {
            //rbtnSigndoc.Checked = true;
            //rbtnSign.Checked = false;

        }

        protected void rbtnSign_CheckedChanged(object sender, EventArgs e)
        {
            //rbtnSigndoc.Checked = false;
            //rbtnSign.Checked = true;
        }

        protected void rbtnSigndoc_CheckedChanged1(object sender, EventArgs e)
        {

        }
    }
}