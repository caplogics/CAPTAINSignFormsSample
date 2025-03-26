using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Captain.Common.Model.Objects;
using System.Data.SqlClient;
using Captain.DatabaseLayer;
using System.Data;

namespace Captain.Common.Model.Data
{
  public class MainMenuData
    {
      public MainMenuData()
        {

      }

        #region Properties

        public CaptainModel Model { get; set; }

        public string UserId { get; set; }

        #endregion

        public List<MainMenuEntity> GetMainMenuSearch(string SearchCat, string SearchFor, string CaseType, string CaseWRK, string App, string Lname, string Fname, string Ssn,
                                             string HNo, string Street, string City, string State, string Phone, string Alias, string ScanApp, string Hierarchy, string DOB, string UserID,string HomePhone)
        {
            List<MainMenuEntity> MainmenuDetails = new List<MainMenuEntity>();
            try
            {
                DataSet MainMenu_Table = Captain.DatabaseLayer.MainMenu.MainMenuSearch(SearchCat, SearchFor, CaseType, CaseWRK, App, Lname, Fname, Ssn, HNo, Street, City, State, Phone, Alias, ScanApp, Hierarchy, DOB, UserID,string.Empty,string.Empty, HomePhone);
                if (MainMenu_Table != null && MainMenu_Table.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in MainMenu_Table.Tables[0].Rows)
                        MainmenuDetails.Add(new MainMenuEntity(row));
                }
            }
            catch (Exception ex)
            {
                return MainmenuDetails;
            }
            return MainmenuDetails;
        }

        
      
    }
}
