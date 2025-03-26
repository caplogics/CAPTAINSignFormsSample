using Captain.Common.Utilities;
using Captain.DatabaseLayer;
using Microsoft.ReportingServices.Diagnostics.Internal;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Captain
{
    public partial class optin : System.Web.UI.Page
    {
        static List<OPTinParams> _OPTinParams = new List<OPTinParams>();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    try
                    {
                        if (Request.QueryString["idparms"] != null)
                        {
                            string encryptCode = Request.QueryString["idparms"].ToString();
                            _OPTinParams = WM_DecryptUrl(encryptCode);
                            string _status = _OPTinParams[0].optStatus;
                            bool _flag = UpdateOPTDate(_OPTinParams[0].username, _OPTinParams[0].optStatus);
                            if (_flag)
                            {
                                imgStatus.ImageUrl = "~/Resources/Images/check-mark.png";
                                if (_status == "A")
                                    lblMessage.Text = "Text Messages activated successfully";
                                else
                                    lblMessage.Text = "Text Messages deactivated successfully";
                            }
                        }
                    }
                    catch
                    {

                    }
                }
            }
            catch
            {

            }
        }

        public static List<OPTinParams> WM_DecryptUrl(string ourl)
        {
            List<OPTinParams> result = new List<OPTinParams>();
            try
            {
                string _decrypturl = CommonFunctions.Decrypt(HttpUtility.UrlDecode(ourl));

                JavaScriptSerializer json = new JavaScriptSerializer();
                List<OPTinParams> objArray = json.Deserialize<List<OPTinParams>>(_decrypturl);

                result = objArray;

            }
            catch (Exception ex)
            {
                //result = ex.Message;
            }
            return result;
        }

        public bool UpdateOPTDate(string _strUserID, string _optStatus)
        {
            bool result = false;
            try
            {
                string conn = ConfigurationManager.ConnectionStrings["CMMSo"].ConnectionString;
                SqlConnection connection = new SqlConnection(conn);
                connection.Open();
                string sql = "";
                if (_optStatus == "A")
                    sql = "UPDATE PASSWORD SET PWR_OPT_IN=GETDATE() WHERE PWR_EMPLOYEE_NO=@USERID";
                else
                    sql = "UPDATE PASSWORD SET PWR_OPT_IN=NULL WHERE PWR_EMPLOYEE_NO=@USERID";
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@USERID", _strUserID);
                int _res = cmd.ExecuteNonQuery();
                connection.Close();
                if (_res > 0)
                    result = true;
            }
            catch (Exception ex)
            {

            }
            return result;
        }
    }
}