using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Services;

namespace Captain
{
    /// <summary>
    /// Summary description for UploadService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class UploadService : System.Web.Services.WebService
    {

        public UploadService()
        {
            //Uncomment the following line if using designed components
            //InitializeComponent();
        }

        [WebMethod]
        public void UploadFiles()
        {
            //Create the Directory.
            string path = HttpContext.Current.Server.MapPath("~/Uploads/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            //Fetch the File.
            HttpPostedFile postedFile = HttpContext.Current.Request.Files[0];

            //Fetch the File Name.
            string fileName = HttpContext.Current.Request.Form["fileName"] + Path.GetExtension(postedFile.FileName);

            //Save the File.
            postedFile.SaveAs(path + fileName);

            //Send OK Response to Client.
            HttpContext.Current.Response.StatusCode = (int)HttpStatusCode.OK;
            HttpContext.Current.Response.Write(fileName);
            HttpContext.Current.Response.Flush();
        }
    }
}
