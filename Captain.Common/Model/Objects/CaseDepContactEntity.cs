using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Captain.Common.Model.Objects
{
    /// <summary>
    /// Entity Object
    /// </summary>
    [Serializable]
    public class DepContactEntity 
    {
        #region Constructors

        public DepContactEntity()
        {
        Agency = string.Empty;
        Dept = string.Empty;
        Program = string.Empty;
        SEQ = string.Empty;
        FirstName = string.Empty;
        MI = string.Empty;
        LastName = string.Empty;
        StaffCode = string.Empty;
        Phone1 = string.Empty;
        Phone2 = string.Empty;
        Fax = string.Empty;
        Email = string.Empty;
        DepDateLstc = string.Empty;
        DepLstcOperator = string.Empty;
        DepDateAdd = string.Empty;
        DepAddOperator = string.Empty;
           
         
        }

        public DepContactEntity(DataRow DEP)
        {
            if (DEP != null)
            {
                DataRow row = DEP;
                Agency = row["DEPCONT_AGENCY"].ToString();
                Dept = row["DEPCONT_DEPT"].ToString();
                Program = row["DEPCONT_PROG"].ToString();
                SEQ = row["DEPCONT_SEQ"].ToString();
                FirstName = row["DEPCONT_FNAME"].ToString();
                MI = row["DEPCONT_MNAME"].ToString();
                LastName = row["DEPCONT_LNAME"].ToString();
                StaffCode = row["DEPCONT_STAFF_CODE"].ToString();
                Phone1 = row["DEPCONT_PHONE1"].ToString();
                Phone2 = row["DEPCONT_PHONE2"].ToString();
                Fax = row["DEPCONT_FAX"].ToString();
                Email = row["DEPCONT_EMAIL"].ToString();
                DepDateLstc = row["DEPCONT_DATE_LSTC"].ToString();
                DepLstcOperator = row["DEPCONT_LSTC_OPERATOR"].ToString();
                DepDateAdd = row["DEPCONT_DATE_ADD"].ToString();
                DepAddOperator = row["DEPCONT_ADD_OPERATOR"].ToString();          
            }
        }

        #endregion

        #region Properties

        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Program { get; set; }
        public string SEQ { get; set; }
        public string FirstName { get; set; }
        public string MI { get; set; }
        public string LastName { get; set; }
        public string StaffCode { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string DepDateLstc { get; set; }
        public string DepLstcOperator { get; set; }
        public string DepDateAdd { get; set; }
        public string DepAddOperator { get; set; }
        public string Mode { get; set; }
       

        #endregion

   
    }

    [Serializable]
    public class DepEnrollHierachiesEntity
    {
        #region Constructors

        public DepEnrollHierachiesEntity()
        {
            Agency = string.Empty;
            Dept = string.Empty;
            Program = string.Empty;
            Hierachies = string.Empty;
            Nofoslots = string.Empty;
            StartDate = string.Empty;
            Enddate = string.Empty;           
            DepDateLstc = string.Empty;
            DepLstcOperator = string.Empty;
            DepDateAdd = string.Empty;
            DepAddOperator = string.Empty;         

        }

        public DepEnrollHierachiesEntity(DataRow DEP)
        {
            if (DEP != null)
            {
                DataRow row = DEP;
                Agency = row["DEPNRLHIE_AGENCY"].ToString();
                Dept = row["DEPNRLHIE_DEPT"].ToString();
                Program = row["DEPNRLHIE_PROG"].ToString();              
                Hierachies = row["DEPNRLHIE_HIE"].ToString();
                Nofoslots = row["DEPNRLHIE_FUND_SLOTS"].ToString();
                StartDate = row["DEPNRLHIE_START_DATE"].ToString();
                Enddate = row["DEPNRLHIE_END_DATE"].ToString();
                DepDateLstc = row["DEPNRLHIE_DATE_LSTC"].ToString();
                DepLstcOperator = row["DEPNRLHIE_LSTC_OPERATOR"].ToString();
                DepDateAdd = row["DEPNRLHIE_DATE_ADD"].ToString();
                DepAddOperator = row["DEPNRLHIE_ADD_OPERATOR"].ToString();
                Mode = "Update";
                DepEnrollCode = string.Empty;
                DepEnrollDesc = string.Empty;
            }
        }

        #endregion

        #region Properties

        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Program { get; set; }
      //  public string Seq { get; set; }
        public string Hierachies { get; set; }
        public string Nofoslots { get; set; }
        public string StartDate { get; set; }
        public string Enddate { get; set; }      
        public string DepDateLstc { get; set; }
        public string DepLstcOperator { get; set; }
        public string DepDateAdd { get; set; }
        public string DepAddOperator { get; set; }
        public string DepEnrollCode { get; set; }
        public string DepEnrollDesc { get; set; }
        public string Mode { get; set; }


        #endregion


    }

}
