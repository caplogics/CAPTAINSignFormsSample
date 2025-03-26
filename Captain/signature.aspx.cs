using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Captain
{
    public partial class signature : System.Web.UI.Page
    {
        SqlConnection connection = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string Appno = Request.QueryString["id"].ToString();
                string ActID= Request.QueryString["actid"].ToString();
                string USERID = Request.QueryString["userid"].ToString();
                

                SetImage(Appno,ActID);
            }
        }

        void SetImage(string Appno,string ACTID)
        {
            //Appno = Appno.Replace("YYYY", "    ");
            string conn = ConfigurationManager.ConnectionStrings["CMMSo"].ConnectionString;
            //SqlDataAdapter myAdapter1 = new SqlDataAdapter("Select [ImgData] FROM [dbo].[tbl_Signature] where filename='" + Appno + "' ", conn);
            SqlDataAdapter myAdapter1 = new SqlDataAdapter("Select [SALSIGN_IMG] FROM [dbo].[SALSIGN] where SALSIGN_SAL_ID='" + Appno + "' AND SALSIGN_ACT_ID='"+ACTID+"'", conn);
            DataTable dt = new DataTable();
            myAdapter1.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                byte[] bytes = (byte[])dt.Rows[0]["SALSIGN_IMG"];
                string imgString = Convert.ToBase64String(bytes);
                imgSignView.Src = String.Format("data:image/Bmp;base64,{0}\"", imgString);
            }
        }


        protected void btnSaveImage_Click(object sender, EventArgs e)
        {
            try
            {
                string Appno = Request.QueryString["id"].ToString();
                string ACTID= Request.QueryString["actid"].ToString();
                string USERID = Request.QueryString["userid"].ToString();
                string FileType = Request.QueryString["filetype"].ToString();
                string strImage = hdnImg.Value; strImage = strImage.Replace("data:image/png;base64,", "");
                byte[] bytes = Convert.FromBase64String(strImage);

                if (bytes.Length > 5218)
                {
                    // string folderPath = Server.MapPath("~/Images/");  //Create a Folder in your Root directory on your solution.
                    //  int count = Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories).Length;
                    //string fileName = Appno;// "signature" + (count + 1) + ".jpg";
                                            // string imagePath = folderPath + fileName;

                    //System.Drawing.Image image;
                    //using (MemoryStream ms = new MemoryStream(bytes))
                    //{
                    //    image = System.Drawing.Image.FromStream(ms);
                    //}

                    // image.Save(imagePath, System.Drawing.Imaging.ImageFormat.Jpeg);

                    //  ClientScript.RegisterClientScriptBlock(this.GetType(), "Close", "close(); ", true);
                    //Page.ClientScript.RegisterOnSubmitStatement(typeof(Page), "closePage", "window.onunload = CloseWindow();");

                    string conn = ConfigurationManager.ConnectionStrings["CMMSo"].ConnectionString;
                    connection = new SqlConnection(conn);

                    connection.Open();
                    string sql = "IF EXISTS(SELECT 1 FROM SALSIGN WHERE [SALSIGN_SAL_ID]=@SALSIGN_SAL_ID AND [SALSIGN_ACT_ID]=@SALSIGN_ACT_ID) BEGIN";
                    sql += " UPDATE SALSIGN SET SALSIGN_IMG = @SALSIGN_IMG,SALSIGN_FileType=@SALSIGN_FileType,SALSIGN_LSTC_OPERATOR=@SALSIGN_LSTC_OPERATOR,SALSIGN_DATE_LSTC=GETDATE() WHERE [SALSIGN_SAL_ID]=@SALSIGN_SAL_ID AND [SALSIGN_ACT_ID]=@SALSIGN_ACT_ID";
                    sql += " END";
                    sql += " ELSE BEGIN";
                    sql += " INSERT INTO SALSIGN(SALSIGN_SAL_ID,SALSIGN_ACT_ID,SALSIGN_IMG,SALSIGN_FileType,SALSIGN_DATE_ADD,SALSIGN_ADD_OPERATOR,SALSIGN_DATE_LSTC,SALSIGN_LSTC_OPERATOR) VALUES(@SALSIGN_SAL_ID,@SALSIGN_ACT_ID,@SALSIGN_IMG,@SALSIGN_FileType,GETDATE(),@SALSIGN_ADD_OPERATOR,GETDATE(),@SALSIGN_LSTC_OPERATOR)";
                    sql += " END";
                    SqlCommand cmd = new SqlCommand(sql, connection);
                    //fileName = fileName.Replace("YYYY", "    ");
                    cmd.Parameters.AddWithValue("@SALSIGN_SAL_ID", Appno);
                    cmd.Parameters.AddWithValue("@SALSIGN_ACT_ID", ACTID);
                    cmd.Parameters.AddWithValue("@SALSIGN_IMG", bytes);
                    cmd.Parameters.AddWithValue("@SALSIGN_FileType", FileType);
                    cmd.Parameters.AddWithValue("@SALSIGN_ADD_OPERATOR", USERID);
                    cmd.Parameters.AddWithValue("@SALSIGN_LSTC_OPERATOR", USERID);

                    cmd.ExecuteNonQuery();
                    connection.Close();

                    //  int id = Convert.ToInt32(cmd.ExecuteScalar());
                    // lblResult.Text = String.Format("Employee ID is {0}", id);
                    divclear.Visible = false;
                    btnSaveImage.Visible = false;
                    divCanvas.Visible = false;
                    dvSignNote.Visible = false;
                    lblmsg.Text = "Signature Saved Successfully!!";
                    lblmsg.ForeColor = System.Drawing.Color.Green;
                    SetImage(Appno,ACTID);
                    //string jScript = "<script>open(location, '_self').close(); window.close();</script>";
                    //ClientScript.RegisterClientScriptBlock(this.GetType(), "keyClientBlock", jScript);

                }
                else
                {
                    lblmsg.Text = "*Please provide a signature first.";
                }
            }
            catch (Exception ex)
            {
                string exMsg = ex.Message;
                lblmsg.Text = ex.Message;
            }
        }
    }
}