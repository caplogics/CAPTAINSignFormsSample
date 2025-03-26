using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Captain.DatabaseLayer;
using Captain.Common.Model.Objects;
using System.Data;

namespace Captain.Common.Model.Data
{
    public class NavigationData
    {

        public NavigationData(CaptainModel model)
            {
                Model = model;

            }

        #region Properties

        public CaptainModel Model { get; set; }

        public string UserId { get; set; }

        #endregion

        /// <summary>
        /// Get User screens information. 
        /// </summary>
        /// <param name="userID">The user ID to get user profile.</param>
        /// <returns>Returns a DataSet with the screens.</returns>
        public List<PrivilegeEntity> GetScreensByUserID(string moduleID, string userID, string mode, string hierarchy)
        {
            List<PrivilegeEntity> userPrivileges = new List<PrivilegeEntity>();
            try
            {
                DataSet userData = Lookups.GetScreensByUserID(moduleID, userID, mode, hierarchy);
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

        /// <summary>
        /// Get User screens information. 
        /// </summary>
        /// <param name="userID">The user ID to get user profile.</param>
        /// <returns>Returns a DataSet with the screens.</returns>
        public List<PrivilegeEntity> GetReportsByUserID(string moduleID, string userID, string mode)
        {
            List<PrivilegeEntity> reportPrivileges = new List<PrivilegeEntity>();
            try
            {
                DataSet userData = Lookups.GetReportsByUserID(moduleID, userID, mode);
                if (userData != null && userData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in userData.Tables[0].Rows)
                    {
                        reportPrivileges.Add(new PrivilegeEntity(row, "Reports"));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return reportPrivileges;
            }

            return reportPrivileges;
        }

        public DataSet AgencyPartner_Navigate(string Nav_Option, string Agencycode, string strMode)
        {
          return   Captain.DatabaseLayer.MainMenu.AgencyPartner_Navigate(Nav_Option,Agencycode, strMode);
        }

      

    }
}
