using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace Captain
{
    public partial class ViewPdfForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string strname = Request.QueryString["Name"].ToString();
                if (!IsPostBack)
                {
                    byte[] buffer = null;
                    buffer = File.ReadAllBytes(strname);
                    if (strname.ToUpper().Contains(".PDF"))
                    {
                        HttpContext.Current.Response.ContentType = "application/pdf";
                    }
                    else if (strname.ToUpper().Contains(".TXT"))
                    {
                        HttpContext.Current.Response.ContentType = "text/plain";
                    }
                    else if (strname.ToUpper().Contains(".DOC"))
                    {
                        HttpContext.Current.Response.ContentType = "Application/vnd.ms-word";
                    }
                    else if(strname.ToUpper().Contains(".JPG"))
                    {
                        HttpContext.Current.Response.ContentType = "image/jpeg";
                    }
                    else if (strname.ToUpper().Contains(".JPEG"))
                    {
                        HttpContext.Current.Response.ContentType = "image/jpeg";
                    }
                    else if (strname.ToUpper().Contains(".PNG"))
                    {
                        HttpContext.Current.Response.ContentType = "image/png";
                    }
                    else if (strname.ToUpper().Contains(".BMP"))
                    {
                        HttpContext.Current.Response.ContentType = "image/bmp";
                    }
                    else
                    {
                        HttpContext.Current.Response.ContentType = "Application/unknown";
                    }
                    HttpContext.Current.Response.OutputStream.Write(buffer, 0, buffer.Length);
                    HttpContext.Current.Response.End();
                }
            }
            catch (Exception ex)
            {

                Response.Write(ex.Message);
            }
          
        }

    }
}