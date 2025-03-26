#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Wisej.Web;
using System.Web;
using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;
using System.Web.UI;
using NPOI.SS.Formula.Functions;


#endregion

namespace Captain.Common.Views.Forms
{
    public partial class Admn0004UserForm : Form
    {
        CaptainModel model;
        public Admn0004UserForm(UserEntity userProfile, string strUserName)
        {
            InitializeComponent();
            model = new CaptainModel();
            UserEntity = userProfile;
            chkuserStatus.Visible = false;
            pnlsearch.Visible = false;
            txtSearch.Focus();
        }

        public Admn0004UserForm(string strUserName)
        {
            InitializeComponent();
            model = new CaptainModel();

            pblblock1.Visible = false;
            chkuserStatus.Checked = false;
            LoadUsers(false);
            
            if (gvwCustomer.Rows.Count > 0)
                btnLanghApplication.Visible = true;
        }
        public UserEntity UserEntity { get; set; }




        private void btnExit_Click(object sender, EventArgs e)
        {
            //Kranthi Commented on 09/26/2022:: doesn't find Context. session

            Application.Session.IsLoggedOn = false;
            Close();


            //Context.Session.IsLoggedOn = false;
            //HttpContext.Current.Session.Clear();
            //Context.Terminate(true);
            //Context.Redirect(Context.HostContext.HttpContext.Request.UrlReferrer.ToString());
        }

        public static string closelogin = "N";
        private void btnLanghApplication_Click(object sender, EventArgs e)
        {
            closelogin = "N";
            foreach (DataGridViewRow gvrows in gvwCustomer.Rows)
            {
                if (gvrows.Selected)
                {
                    string strErrorMsg = string.Empty;
                    UserEntity userProfile = model.AuthenticateUser.AuthenticateWithProfile(gvrows.Cells["UserName"].Value.ToString().Trim(), gvrows.Cells["Password"].Value.ToString().Trim(), "******", out strErrorMsg);

                    if ((!userProfile.UserID.Equals("@InCorrect@1UID@2PSW")) && (int.Parse(userProfile.UnSuccessful) < 5)) // &&  (!userProfile.UserID.Equals("@InActiveUserId")) || userProfile != null) 
                    {

                        bool isLoginRegistered = model.AuthenticateUser.RegisterLogin(userProfile.UserID);
                        if (txtPassword.Text != "******")
                        {
                            Captain<string>.Session[Consts.SessionVariables.FullName] = userProfile.FirstName.Trim() + Consts.Common.Space + (userProfile.MI.Trim().Equals(string.Empty) ? string.Empty : userProfile.MI + Consts.Common.Space) + userProfile.LastName.Trim();
                            Captain<string>.Session[Consts.SessionVariables.UserID] = userProfile.UserID.Trim();
                            Captain<string>.Session[Consts.SessionVariables.UserName] = userProfile.UserName;

                            Captain<string>.Session[Consts.SessionVariables.LostLogin_Status] = " Successful";
                            Captain<string>.Session[Consts.SessionVariables.LostLogin_Date] = userProfile.LastSuccessful;
                            if (int.Parse(userProfile.UnSuccessful) > 0)
                            {
                                Captain<string>.Session[Consts.SessionVariables.LostLogin_Status] = " unsuccessful";
                                Captain<string>.Session[Consts.SessionVariables.LostLogin_Date] = userProfile.LastUnSuccessful;
                            }

                            string strHelpURL = chkZendeskURL();
                            if (strHelpURL == "")
                            {
                                Captain<UserEntity>.Session[Consts.SessionVariables.UserProfile] = userProfile;
                                Application.Session.IsLoggedOn = true;

                                closelogin = "Y";

                                DialogResult = DialogResult.Yes;
                                Close();
                            }
                            else
                            {
                                Application.Navigate(strHelpURL);
                            }



                            //Application.MainPage = new MasterPage();
                            // MasterPage oMasterPage = new MasterPage();
                            //Application.MainPage = new MasterPage.MasterPage();

                            //Context.Session.IsLoggedOn = true;

                            //strFolderPath = "C:\\CapReports\\" + userName.Trim() + "\\";           // Run at Local System
                            //strFolderPath = Consts.Common.ReportFolderLocation + userName.Trim() + "\\";  // Run at Server
                            //MyDir = new DirectoryInfo(strFolderPath);
                            //if (MyDir.Exists == false)
                            //    MyDir.Create();
                        }
                        else
                        {
                            Admn0004UserForm objAdmn0004 = new Admn0004UserForm(userProfile, gvrows.Cells["UserName"].Value.ToString().Trim());
                            objAdmn0004.ShowDialog();
                        }
                    }
                    else if (userProfile.UserID.Equals("@NoHierarchy"))
                    {
                        MessageBox.Show("Sorry! you have access on no hierarchy", Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    }
                    else
                    {
                        if (userProfile.UserID.Equals("@InCorrect@1UID@2PSW") &&
                            (userProfile.Password.Equals("PASSWORD") || int.Parse(userProfile.UnSuccessful) > 0))
                        {
                            MessageBox.Show("Your Account is Blocked. Please Contact System administrator", Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        }
                    }


                }
            }
        }
        private string chkZendeskURL()
        {
            string _Res = "";

            if (Application.Uri.Query.Contains("return_to"))
            {
                string zendeskUrl = Application.QueryString["return_to"].ToString();
                _Res = CommonFunctions.ZendeskLogin(zendeskUrl);
            }
            return _Res;
        }
        private void btnViewUsers_Click(object sender, EventArgs e)
        {
            if (UserEntity.Password.ToUpper().Equals(txtPassword.Text.ToUpper()))
            {
                //DataSet ds = Captain.DatabaseLayer.UserAccess.UserSearch(null, null, null, null, null, null);
                //DataTable dt = ds.Tables[0];
                //foreach (DataRow dr in dt.Rows)
                //{
                //    if (dr["PWR_EMPLOYEE_NO"].ToString().ToUpper() != "JAKE" && dr["PWR_EMPLOYEE_NO"].ToString().ToUpper() != "CAPLOGICS")
                //    {
                //        int rowIndex = gvwCustomer.Rows.Add(dr["PWR_EMPLOYEE_NO"].ToString(), dr["PWR_PASSWORD"]);
                //        gvwCustomer.Rows[rowIndex].Tag = dr;
                //        if (dr["PWR_INACTIVE_FLAG"].ToString().Trim() == "Y")
                //            gvwCustomer.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
                //    }
                //    //  CommonFunctions.setTooltip(rowIndex, dr);

                //}
                chkuserStatus.Visible = true; pnlsearch.Visible = true; txtSearch.Focus();
                LoadUsers(chkuserStatus.Checked);
            }
            else
            {
                MessageBox.Show("Invalid User ID/Password. Please Contact System administrator", Consts.Common.ApplicationCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            if (gvwCustomer.Rows.Count > 0)
                btnLanghApplication.Visible = true;
        }

        private void chkuserStatus_CheckedChanged(object sender, EventArgs e)
        {
            LoadUsers(chkuserStatus.Checked);
        }

        void LoadUsers(bool isInactive)
        {
            gvwCustomer.Rows.Clear();
            DataSet ds = Captain.DatabaseLayer.UserAccess.UserSearch(null, null, null, null, null, null);
            DataTable dt = new DataTable();
            DataRow[] drFilt;
            if (isInactive)
                drFilt = ds.Tables[0].Select("PWR_INACTIVE_FLAG='Y'");   // AND PWR_SECURITY <>'B'    // INACTIVE users filter
            else
                drFilt = ds.Tables[0].Select("PWR_INACTIVE_FLAG='N' ");  //AND PWR_SECURITY <> 'B'     // ACTIVE users filter

            if (drFilt.Length > 0)
                dt = drFilt.CopyToDataTable();

            /************* SORT BY Employeeno ****************/
            DataView dv = dt.DefaultView;
            dv.Sort = "PWR_EMPLOYEE_NO";
            dt =dv.ToTable();
            /*****************************/

            foreach (DataRow dr in dt.Rows)
            {
                if (dr["PWR_EMPLOYEE_NO"].ToString().ToUpper() != "JAKE" && dr["PWR_EMPLOYEE_NO"].ToString().ToUpper() != "CAPLOGICS")
                {
                    int rowIndex = gvwCustomer.Rows.Add(dr["PWR_EMPLOYEE_NO"].ToString(), dr["PWR_PASSWORD"], dr["PWR_NAME_IX_FIRST"], dr["PWR_NAME_IX_LAST"]);
                    gvwCustomer.Rows[rowIndex].Tag = dr;
                    if (dr["PWR_INACTIVE_FLAG"].ToString().Trim() == "Y")
                        gvwCustomer.Rows[rowIndex].DefaultCellStyle.ForeColor = System.Drawing.Color.Red;
                }
            }


        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            searchGrid();
        }
        void searchGrid()
        {
            if (txtSearch.Text != "")
            {
                var allRows = gvwCustomer.Rows.Cast<DataGridViewRow>().ToList();
                allRows.ForEach(r => r.Visible = false);

                var filteredRows = gvwCustomer.Rows.Cast<DataGridViewRow>()
           .Where(row => row.Cells[0].Value.ToString().Contains(txtSearch.Text.ToUpper()) || row.Cells[2].Value.ToString().ToUpper().Contains(txtSearch.Text.ToUpper()) ||
           row.Cells[3].Value.ToString().ToUpper().Contains(txtSearch.Text.ToUpper()))
           .ToList();

                filteredRows.ForEach(r => r.Visible = true);
            }
            else
            {
                var allRows = gvwCustomer.Rows.Cast<DataGridViewRow>().ToList();
                allRows.ForEach(r => r.Visible = true);
            }
        }
    }
}