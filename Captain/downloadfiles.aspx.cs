using Captain.Common.Model.Data;
using Captain.Common.Views.Forms.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Captain
{
    public partial class downloadfiles : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string strTyp = Request.QueryString["typ"].ToString();
                if (strTyp == "SD")
                {
                    string Appfoldername = Request.QueryString["fld"].ToString();
                    string fileName = Request.QueryString["file"].ToString();
                    string HISID = Request.QueryString["hisid"].ToString();

                    AlienTXData _oAlienTXData = new AlienTXData();
                    bool _isSaveFlag = _oAlienTXData.INSUPDEL_DOCSIGNHIS(HISID, "", "", "", "", "", "", "", "", "", "", "", "", "", "","","","", "DOWNLD");

                    CaptainModel _model = new CaptainModel();
                    string propReportPath = _model.lookupDataAccess.GetServerReportPath();
                    string FilePath = propReportPath + "//SignForms//" + Appfoldername + "//" + fileName;

                    // string Qryval = Request.QueryString["val"].ToString();
                    FileInfo file = new FileInfo(FilePath);
                    if (file.Exists)
                    {
                        HttpContext.Current.Response.Clear();
                        HttpContext.Current.Response.ClearHeaders();
                        HttpContext.Current.Response.ClearContent();
                        HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + fileName + "");
                        HttpContext.Current.Response.AddHeader("Content-Type", "application/pdf");
                        HttpContext.Current.Response.ContentType = "application/vnd.pdf";
                        HttpContext.Current.Response.AddHeader("Content-Length", file.Length.ToString());
                        HttpContext.Current.Response.WriteFile(file.FullName);
                        HttpContext.Current.Response.End();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}