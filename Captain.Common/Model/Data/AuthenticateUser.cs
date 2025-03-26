using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using Captain.DatabaseLayer;
using Captain.Common.Model.Objects;

namespace Captain.Common.Model.Data
{
    public class AuthenticateUser
    {

        public AuthenticateUser(CaptainModel model)
        {
            Model = model;

        }

        #region Properties

        public CaptainModel Model { get; set; }

        public string UserId { get; set; }

        #endregion

        /// <summary>
        /// Authenticate user and store his personal  and preferences information. 
        /// </summary>
        /// <param name="userName">The user name to authenticate.</param>
        /// <param name="password">The password to the user name.</param>
        /// <returns>Returns a DataSet with the user's preferences,profiles.</returns>
        public UserEntity AuthenticateWithProfile(string userName, string password,string LoginType,out string strErrorMsg)
        {
            string strMsg = string.Empty;
            UserEntity userProfile = null;
            try
            {
                DataSet userData = UserAccess.AuthenticatePassword(userName, password, LoginType);
                if(userData != null && userData.Tables[0].Rows.Count > 0)
                {
                    userProfile = new UserEntity(userData.Tables[0]);
                }
            }
            catch (Exception ex)
            {
               strMsg =   ex.Message;
                //
             //   return userProfile;
            }

           
            strErrorMsg = strMsg;
            return userProfile;
        }

        /// <summary>
        /// Registers a user as logged in.
        /// </summary>
        /// <param name="userID">The user id to register.</param>
        /// <returns>Returns true if successfull.</returns>
        public bool RegisterLogin(string userID)
        {
            bool response = true;

            return response;
        }

        /// <summary>
        /// Removes a user registration
        /// </summary>
        /// <param name="userID">The user id to remove</param>
        /// <returns>Returns true if successfull.</returns>
        public bool RegisterLogout(string userID)
        {
            return true;
        }

    }
}
