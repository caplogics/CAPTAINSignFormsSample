/**********************************************************************************************************
 * Class Name   : HierarchyEntity
 * Author       : 
 * Created Date : 
 * Version      : 
 * Description  : Entity object to extend ObjectUsersType.
 **********************************************************************************************************/

#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Captain.Common.Utilities;
using System.Web.UI.WebControls;
using System.Data;

#endregion

namespace Captain.Common.Model.Objects
{
    /// <summary>
    /// Entity Object
    /// </summary>
    [Serializable]
    public class CaseHierarchyEntity
    {
        #region Constructors

        public CaseHierarchyEntity()
        {
             Code = string.Empty;
             Agency   = string.Empty;
             Dept   = string.Empty;
             Prog   = string.Empty;
             ShortName = string.Empty;
             AltIntake   = string.Empty;
             Reprsntn   = string.Empty;
             HierarchyName   = string.Empty;
             HierarchyProg   = string.Empty;
             CN_Format   = string.Empty;
             CW_Format   = string.Empty;
             DateLSTC = string.Empty;
             LSTCOperator   = string.Empty;
             DateAdd   = string.Empty;
             AddOperator = string.Empty;             
             Mode  = string.Empty;
        }

        public CaseHierarchyEntity(DataRow userHierarchy)
        {
            if (userHierarchy != null)
            {
                DataRow row = userHierarchy;
                Code    = row["CODE"].ToString();
                Agency  = row["HIE_AGENCY"].ToString();
                Dept    = row["HIE_DEPT"].ToString();
                Prog    = row["HIE_PROGRAM"].ToString();
                HierarchyName = row["HIE_NAME"].ToString();
                DateLSTC     = row["HIE_DATE_LSTC"].ToString();
                LSTCOperator = row["HIE_LSTC_OPERATOR"].ToString();
                DateAdd      = row["HIE_DATE_ADD"].ToString();
                AddOperator  = row["HIE_ADD_OPERATOR"].ToString();
            }
        }
        
        #endregion

        #region Properties

        public string Mode { get; set; }
        public string Code { get; set; }
        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Prog { get; set; }
        public string ShortName { get; set; }
        public string AltIntake { get; set; }
        public string Reprsntn { get; set; }
        public string HierarchyName { get; set; }
        public string HierarchyProg { get; set; }
        public string CN_Format { get; set; }
        public string CW_Format { get; set; }
        public string DateLSTC { get; set; }
        public string LSTCOperator { get; set; }
        public string DateAdd { get; set; }
        public string AddOperator { get; set; }

        #endregion


    }
}
