using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using Captain.DatabaseLayer;
using Captain.Common.Model.Objects;
using System.Data.SqlClient;
using NPOI.OpenXmlFormats.Shared;
using System.Configuration;

namespace Captain.Common.Model.Data
{
    public class UserProfileAccess
    {

        public UserProfileAccess(CaptainModel model)
        {
            Model = model;

        }

        #region Properties

        public CaptainModel Model { get; set; }

        public string UserId { get; set; }

        #endregion

        /// <summary>
        /// Get User Profile information. 
        /// </summary>
        /// <param name="userID">The user ID to get user profile.</param>
        /// <returns>Returns a DataSet with the user's profiles.</returns>
        public UserEntity GetUserProfileByID(string userID)
        {
            UserEntity userProfile = null;
            try
            {
                DataSet userData = UserAccess.GetUserProfileByID(userID, "PASSWORD", string.Empty);
                if (userData != null && userData.Tables[0].Rows.Count > 0)
                {
                    userProfile = new UserEntity(userData.Tables[0]);
                }
            }
            catch (Exception ex)
            {
                //
                return userProfile;
            }

            return userProfile;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Caseworker"></param>
        /// <returns></returns>
        public UserEntity Checkcaseworker(string Caseworker)
        {
            UserEntity userProfile = null;
            try
            {
                DataSet userData = UserAccess.CheckCaseworkerByID(Caseworker);
                if (userData != null && userData.Tables[0].Rows.Count > 0)
                {
                    userProfile = new UserEntity(userData.Tables[0]);
                }
            }
            catch (Exception ex)
            {
                //
                return userProfile;
            }

            return userProfile;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Caseworker"></param>
        /// <returns></returns>
        public List<UserEntity> CheckcaseworkerEdit(string Caseworker)
        {
            List<UserEntity> userProfile = new List<UserEntity>();
            try
            {
                DataSet userData = UserAccess.CheckCaseworkerByID(Caseworker);
                if (userData != null && userData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in userData.Tables[0].Rows)
                    {
                        userProfile.Add(new UserEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return userProfile;
            }

            return userProfile;
        }

        /// <summary>
        /// Get Template User Profile information. 
        /// </summary>            
        /// <returns>Returns a DataSet with the user's profiles.</returns>
        public List<UserEntity> GetTemplateUsers(string Agency)
        {
            List<UserEntity> userProfile = new List<UserEntity>();
            try
            {
                DataSet userData = UserAccess.GetUserProfileByID(string.Empty, "TEMPLATEUSER", Agency);
                if (userData != null && userData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in userData.Tables[0].Rows)
                    {
                        userProfile.Add(new UserEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return userProfile;
            }

            return userProfile;
        }

        /// <summary>
        /// Get Template User Profile information. 
        /// </summary>            
        /// <returns>Returns a DataSet with the user's profiles.</returns>
        public List<UserEntity> GetUsersList()
        {
            List<UserEntity> userProfile = new List<UserEntity>();
            try
            {
                DataSet userData = UserAccess.UserSearch(null, null, null, "N", string.Empty, string.Empty);
                if (userData != null && userData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in userData.Tables[0].Rows)
                    {
                        userProfile.Add(new UserEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return userProfile;
            }

            return userProfile;
        }

        /// <summary>
        /// Get User Profile information. 
        /// </summary>
        /// <param name="userID">The user ID to get user profile.</param>
        /// <returns>Returns a DataSet with the user's profiles.</returns>
        public List<HierarchyEntity> GetUserHierarchyByID(string userID)
        {
            List<HierarchyEntity> userProfile = new List<HierarchyEntity>();
            try
            {
                DataSet userData = UserAccess.GetUserProfileByID(userID, "PASSWORDHIE", string.Empty);
                if (userData != null && userData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in userData.Tables[0].Rows)
                    {
                        userProfile.Add(new HierarchyEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return userProfile;
            }

            return userProfile;
        }


        public List<HierarchyEntity> GetUserServiceHierarchyByID(string userID)
        {
            List<HierarchyEntity> userProfile = new List<HierarchyEntity>();
            try
            {
                DataSet userData = UserAccess.GetUserProfileByID(userID, "PASSWORDSERHIE", string.Empty);
                if (userData != null && userData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in userData.Tables[0].Rows)
                    {
                        userProfile.Add(new HierarchyEntity(row, "PASSWORDSERHIE"));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return userProfile;
            }

            return userProfile;
        }

        public List<CLINQHIEEntity> GetClentInqByID(string userID)
        {
            List<CLINQHIEEntity> userProfile = new List<CLINQHIEEntity>();
            try
            {
                DataSet userData = UserAccess.GetClientUserByID(userID);
                if (userData != null && userData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in userData.Tables[0].Rows)
                    {
                        userProfile.Add(new CLINQHIEEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return userProfile;
            }

            return userProfile;
        }

        /// <summary>
        /// Get User Profile information. 
        /// </summary>
        /// <param name="userID">The user ID to get user profile.</param>
        /// <returns>Returns a DataSet with the user's profiles.</returns>
        public List<PrivilegeEntity> GetUserPrivilegesByID(string userID, string privType, string Hierachy)
        {
            List<PrivilegeEntity> userProfile = new List<PrivilegeEntity>();
            string tableName = string.Empty;
            try
            {
                if (privType.Equals("Screen"))
                {
                    tableName = "EMPLFUNC";
                }
                else
                {
                    tableName = "BATCNTL";
                }
                DataSet userData = UserAccess.GetUserProfileByID(userID, tableName, Hierachy);
                if (userData != null && userData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in userData.Tables[0].Rows)
                    {
                        userProfile.Add(new PrivilegeEntity(row, privType));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return userProfile;
            }

            return userProfile;
        }

        /// <summary>
        /// Get User screens information. 
        /// </summary>
        /// <param name="userID">The user ID to get user profile.</param>
        /// <returns>Returns a DataSet with the screens.</returns>
        public List<PrivilegeEntity> GetScreensByUserID(string moduleID, string userID, string hierarchy)
        {
            List<PrivilegeEntity> userPrivileges = new List<PrivilegeEntity>();
            try
            {
                DataSet userData = Lookups.GetScreensByUserID(moduleID, userID, "View", hierarchy);
                if (userData != null && userData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in userData.Tables[0].Rows)
                    {
                        userPrivileges.Add(new PrivilegeEntity(row, "Screen"));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return userPrivileges;
            }

            return userPrivileges;
        }


        public List<MenuBranchEntity> GetMenuBranches()
        {
            List<MenuBranchEntity> menubranchEntity = new List<MenuBranchEntity>();
            try
            {
                DataSet userData = Lookups.GetMenuBranches();
                if (userData != null && userData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in userData.Tables[0].Rows)
                    {
                        menubranchEntity.Add(new MenuBranchEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return menubranchEntity;
            }

            return menubranchEntity;
        }




        /// <summary>
        /// Get User screens information. 
        /// </summary>
        /// <param name="userID">The user ID to get user profile.</param>
        /// <returns>Returns a DataSet with the screens.</returns>
        public List<PrivilegeEntity> GetReportsByUserID(string moduleID, string userID)
        {
            List<PrivilegeEntity> userPrivileges = new List<PrivilegeEntity>();
            try
            {
                DataSet userData = Lookups.GetReportsByUserID(moduleID, userID, "View");
                if (userData != null && userData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in userData.Tables[0].Rows)
                    {
                        userPrivileges.Add(new PrivilegeEntity(row, "Reports"));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return userPrivileges;
            }

            return userPrivileges;
        }

        /// <summary>
        /// Get User screens information. 
        /// </summary>
        /// <param name="userID">The user ID to get user profile.</param>
        /// <returns>Returns a DataSet with the screens.</returns>
        public List<PrivilegeEntity> GetUserReportMaintenanceByserID(string moduleID, string userID)
        {
            List<PrivilegeEntity> userPrivileges = new List<PrivilegeEntity>();
            try
            {
                DataSet userData = Lookups.GetUserReportMaintenanceByUserID(moduleID, userID, "View");
                if (userData != null && userData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in userData.Tables[0].Rows)
                    {
                        userPrivileges.Add(new PrivilegeEntity(row, "UserReportMaintenance"));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return userPrivileges;
            }

            return userPrivileges;
        }


        /// <summary>
        /// Get User screens information. 
        /// </summary>
        /// <param name="userID">The user ID to get user profile.</param>
        /// <returns>Returns a DataSet with the screens.</returns>
        public List<PrivilegeEntity> GetApplicationsByUserID(string userID, string HIE)
        {
            List<PrivilegeEntity> userPrivileges = new List<PrivilegeEntity>();
            try
            {
                DataSet userData = UserAccess.GetApplicationsByUserID(userID, HIE);
                if (userData != null && userData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in userData.Tables[0].Rows)
                    {
                        userPrivileges.Add(new PrivilegeEntity(row, "Module"));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return userPrivileges;
            }

            return userPrivileges;
        }

        public string UpdatePassword(string UserId, string password, string Newpassword)
        {
            // UserEntity userProfile = null;
            string strmsg = "";
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@PWR_EMPLOYEE_NO", UserId));
                sqlParamList.Add(new SqlParameter("@PWR_PASSWORD", password));
                sqlParamList.Add(new SqlParameter("@NewPassword ", Newpassword));
                SqlParameter parameterMsg = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
                parameterMsg.Direction = ParameterDirection.Output;
                sqlParamList.Add(parameterMsg);
                Captain.DatabaseLayer.UserAccess.UpdatePASSWORD(sqlParamList);
                strmsg = sqlParamList[3].SqlValue.ToString();
                //   return sqlParamList[].Value.ToString();
            }
            catch (Exception ex)
            {
                //
                return strmsg;
            }

            return strmsg;
        }

        public string InsertUpdateLogUsers(string UserId, string sessionId, string IP, string strMode, string strLogId)
        {
            // UserEntity userProfile = null;
            string strmsg = "";
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@LOG_SESSIONID", sessionId));
                sqlParamList.Add(new SqlParameter("@LOG_USERID", UserId));
                sqlParamList.Add(new SqlParameter("@LOG_IP", IP));
                sqlParamList.Add(new SqlParameter("@Mode", strMode));
                SqlParameter parameterMsg = new SqlParameter("@LOG_ID", SqlDbType.VarChar, 100);
                parameterMsg.Direction = ParameterDirection.InputOutput;
                parameterMsg.Value = strLogId;
                sqlParamList.Add(parameterMsg);
                if (Captain.DatabaseLayer.UserAccess.InsertUpdateLogUsers(sqlParamList))
                    strmsg = sqlParamList[4].SqlValue.ToString();

            }
            catch (Exception ex)
            {

                return strmsg;
            }

            return strmsg;
        }

        public DataTable GetEMailSetting(string strType)
        {
            DataTable dt = null;
            try
            {
                DataSet ds = UserAccess.GetEMailSetting(strType);
                if (ds.Tables[0].Rows.Count > 0)
                    dt = ds.Tables[0];

            }
            catch (Exception ex)
            {
                //
                return dt;
            }

            return dt;
        }

        public DataTable FetchLoginUser(string Username, string email, string Mobile, string Mode)
        {
            DataTable dt = null;
            try
            {
                DataSet ds = UserAccess.FetchLoginUser(Username, email, Mobile, Mode);
                if (ds.Tables[0].Rows.Count > 0)
                    dt = ds.Tables[0];
            }
            catch (Exception ex)
            {
                //
                return dt;
            }

            return dt;
        }



        public bool checkUseremailMobile(string Username, string Mode)
        {
            bool _flag = false;
            DataTable dt = null;
            try
            {
                DataSet ds = UserAccess.FetchLoginUser(Username, "", "", Mode);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        int val = Convert.ToInt32(dt.Rows[0][0].ToString());
                        if (val > 0)
                            _flag = true;
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return _flag;
            }

            return _flag;
        }

        public bool UpdateForcePassword(string UserID)
        {
            bool _flag = false;
            try
            {
                string conn = ConfigurationManager.ConnectionStrings["CMMSo"].ConnectionString;
                SqlConnection connection = new SqlConnection(conn);
                connection.Open();
                string sql = "UPDATE PASSWORD SET PWR_UNSUCCESSFUL='1', PWR_SUCCESSFUL='0' WHERE PWR_EMPLOYEE_NO=@USERID";
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@USERID", UserID);
                cmd.ExecuteNonQuery();
                connection.Close();
                _flag = true;
            }
            catch(Exception ex)
            {
                return false;
            }

            return _flag;
        }
    }
}
