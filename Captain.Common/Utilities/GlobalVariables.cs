/************************************************************************************
 * Class Name    : GlobalVariables
 * Author        : 
 * Created Date  : 
 * Version       : 1.0
 * Description   : This class has Global Variables declarations
 *************************************************************************************/

using Captain.Common.Views.UserControls.Base;
using System.Collections.Generic;
namespace Captain.Common.Utilities
{
    public static class GlobalVariables
    {       
        public static int CacheExpiration = 60;
        public static int SystemCacheExpiration = 24;

        public static string Password = "";
        public static string FileName = "";
        public static string PasswordMessage = "";
        public static bool IsMasterPassword = false;
        public static string FormTitle = "";
        public static string FormName = "";
        public static bool FormIsSaveEnabled = false;
    }
}
