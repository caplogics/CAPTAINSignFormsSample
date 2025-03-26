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
using NPOI.SS.Formula.Functions;

#endregion

namespace Captain.Common.Model.Objects
{
    /// <summary>
    /// Entity Object
    /// </summary>
    [Serializable]
    public class HierarchyEntity
    {
        #region Constructors

        public HierarchyEntity()
        {
            Code = string.Empty;
            UserID = string.Empty;
            CaseWorker = string.Empty;
            HirarchyType = string.Empty;
            LastName = string.Empty;
            FirstName = string.Empty;
            MI = string.Empty;
            Security = string.Empty;
            Agency = string.Empty;
            Dept = string.Empty;
            Prog = string.Empty;
            UsedFlag = string.Empty;
            InActiveFlag = string.Empty;
            DateLSTC = string.Empty;
            LSTCOperator = string.Empty;
            DateAdd = string.Empty;
            AddOperator = string.Empty;
            HirarchyName = string.Empty;
            ShortName = string.Empty;
            Intake = string.Empty;
            HIERepresentation = string.Empty;
            HIEProg = string.Empty;
            CNFormat = string.Empty;
            CWFormat = string.Empty;
            AgencyName = string.Empty;
            DeptName = string.Empty;
            PIPActive = string.Empty;
            Logo = string.Empty;

            SerAgency = string.Empty;
            SerDept = string.Empty;
            SerProg = string.Empty;

            IntakeCNT = string.Empty;
            ACTProgCNT = string.Empty;
            MSProgCNT = string.Empty;

        }

        public HierarchyEntity(DataRow userHierarchy)
        {
            if (userHierarchy != null)
            {
                DataRow row = userHierarchy;
                Code = row["CODE"].ToString().Trim();
                UserID = row["PWH_EMPLOYEE_NO"].ToString().Trim();
                HirarchyType = row["PWH_TYPE"].ToString().Trim();
                CaseWorker = row["PWH_CASEWORKER"].ToString().Trim();
                LastName = row["PWH_LAST_NAME"].ToString().Trim();
                FirstName = row["PWH_FIRST_NAME"].ToString().Trim();
                MI = row["PWH_MI_NAME"].ToString().Trim();
                Security = row["PWH_SECURITY"].ToString().Trim();
                Agency = row["PWH_AGENCY"].ToString().Trim();
                Dept = row["PWH_DEPT"].ToString().Trim();
                Prog = row["PWH_PROG"].ToString().Trim();
                UsedFlag = row["PWH_USED_FLAG"].ToString().Trim();
                InActiveFlag = row["PWH_INACTIVE"].ToString().Trim();
                DateLSTC = row["PWH_DATE_LSTC"].ToString().Trim();
                LSTCOperator = row["PWH_LSTC_OPERATOR"].ToString().Trim();
                DateAdd = row["PWH_DATE_ADD"].ToString().Trim();
                AddOperator = row["PWH_ADD_OPERATOR"].ToString().Trim();
                Sites = row["PWH_SITES"].ToString().Trim();
                HirarchyName = row["HIE_NAME"].ToString().Trim();
                // Logo= row["HIE_LOGO"].ToString().Trim();
            }
        }

        public HierarchyEntity(string code, string desc)
        {
            Code = code;
            ShortName = desc;
        }

        public HierarchyEntity(DataRow userHierarchy, string tableName)
        {
            if (tableName == "PASSWORDHIEBYID")
            {
                DataRow row = userHierarchy;
                Code = row["CODE"].ToString().Trim();
                Agency = row["PWH_AGENCY"].ToString().Trim();
                Dept = row["PWH_DEPT"].ToString().Trim();
                Prog = row["PWH_PROG"].ToString().Trim();
            }
            else if (tableName == "S")
            {
                if (userHierarchy != null)
                {
                    DataRow row = userHierarchy;
                    Code = row["CODE"].ToString().Trim();
                    UserID = row["PWH_EMPLOYEE_NO"].ToString().Trim();
                    HirarchyType = row["PWH_TYPE"].ToString().Trim();
                    CaseWorker = row["PWH_CASEWORKER"].ToString().Trim();
                    LastName = row["PWH_LAST_NAME"].ToString().Trim();
                    FirstName = row["PWH_FIRST_NAME"].ToString().Trim();
                    MI = row["PWH_MI_NAME"].ToString().Trim();
                    Security = row["PWH_SECURITY"].ToString().Trim();
                    Agency = row["PWH_AGENCY"].ToString().Trim();
                    Dept = row["PWH_DEPT"].ToString().Trim();
                    Prog = row["PWH_PROG"].ToString().Trim();
                    UsedFlag = row["PWH_USED_FLAG"].ToString().Trim();
                    InActiveFlag = row["PWH_INACTIVE"].ToString().Trim();
                    DateLSTC = row["PWH_DATE_LSTC"].ToString().Trim();
                    LSTCOperator = row["PWH_LSTC_OPERATOR"].ToString().Trim();
                    DateAdd = row["PWH_DATE_ADD"].ToString().Trim();
                    AddOperator = row["PWH_ADD_OPERATOR"].ToString().Trim();
                    Sites = row["PWH_SITES"].ToString().Trim();
                    HirarchyName = row["HIE_NAME"].ToString().Trim();

                    SerAgency = row["PWSH_AGENCY"].ToString().Trim();
                    SerDept = row["PWSH_DEPT"].ToString().Trim();
                    SerProg = row["PWSH_PROG"].ToString().Trim();

                    // Logo= row["HIE_LOGO"].ToString().Trim();
                }
            }
            else if (tableName == "PASSWORDSERHIE")
            {
                if (userHierarchy != null)
                {
                    DataRow row = userHierarchy;
                    Code = row["CODE"].ToString().Trim();
                    UserID = row["PWSH_EMPLOYEE_NO"].ToString().Trim();
                    //HirarchyType = row["PWH_TYPE"].ToString().Trim();
                    CaseWorker = row["PWSH_CASEWORKER"].ToString().Trim();
                    LastName = row["PWSH_LAST_NAME"].ToString().Trim();
                    FirstName = row["PWSH_FIRST_NAME"].ToString().Trim();
                    MI = row["PWSH_MI_NAME"].ToString().Trim();
                    Security = row["PWSH_SECURITY"].ToString().Trim();
                    Agency = row["PWSH_AGENCY"].ToString().Trim();
                    Dept = row["PWSH_DEPT"].ToString().Trim();
                    Prog = row["PWSH_PROG"].ToString().Trim();
                    UsedFlag = row["PWSH_USED_FLAG"].ToString().Trim();
                    InActiveFlag = row["PWSH_INACTIVE"].ToString().Trim();
                    DateLSTC = row["PWSH_DATE_LSTC"].ToString().Trim();
                    LSTCOperator = row["PWSH_LSTC_OPERATOR"].ToString().Trim();
                    DateAdd = row["PWSH_DATE_ADD"].ToString().Trim();
                    AddOperator = row["PWSH_ADD_OPERATOR"].ToString().Trim();
                    Sites = row["PWSH_SITES"].ToString().Trim();
                    HirarchyName = row["HIE_NAME"].ToString().Trim();

                    SerAgency = row["PWSIH_AGENCY"].ToString().Trim();
                    SerDept = row["PWSIH_DEPT"].ToString().Trim();
                    SerProg = row["PWSIH_PROG"].ToString().Trim();

                    // Logo= row["HIE_LOGO"].ToString().Trim();
                }
            }
            else
            {
                if (userHierarchy != null)
                {
                    DataRow row = userHierarchy;
                    Code = row["CODE"].ToString().Trim();
                    Agency = row["HIE_AGENCY"].ToString().Trim();
                    Dept = row["HIE_DEPT"].ToString().Trim();
                    Prog = row["HIE_PROGRAM"].ToString().Trim();
                    HirarchyName = row["HIE_NAME"].ToString().Trim();
                    ShortName = row["HIE_SHORT_NAME"].ToString().Trim();
                    Intake = row["HIE_ALT_INTAKE"].ToString().Trim();
                    HIERepresentation = row["HIE_REPRSNTN"].ToString().Trim();
                    HIEProg = row["HIE_PROG"].ToString().Trim();
                    CNFormat = row["HIE_CN_FORMAT"].ToString().Trim();
                    CWFormat = row["HIE_CW_FORMAT"].ToString().Trim();
                    DateLSTC = row["HIE_DATE_LSTC"].ToString().Trim();
                    LSTCOperator = row["HIE_LSTC_OPERATOR"].ToString().Trim();
                    DateAdd = row["HIE_DATE_ADD"].ToString().Trim();
                    AddOperator = row["HIE_ADD_OPERATOR"].ToString().Trim();
                    Logo = row["HIE_LOGO"].ToString().Trim();


                    if (row.Table.Columns.Contains("HIE_SNAME"))
                    {
                        SpanishName = row["HIE_SNAME"].ToString().Trim();
                    }
                    else
                        SpanishName = string.Empty;

                    if (row.Table.Columns.Contains("HIE_SEND_PIP"))
                    {
                        SendtoPIP = row["HIE_SEND_PIP"].ToString().Trim();
                    }
                    else
                        SendtoPIP = string.Empty;

                    if (row.Table.Columns.Contains("HIE_PIP_ACTIVE"))
                    {
                        PIPActive = row["HIE_PIP_ACTIVE"].ToString().Trim();
                    }
                    else
                        PIPActive = string.Empty;





                    if (tableName.Equals("DEFAULT"))
                    {
                        AgencyName = row["AGENCYNAME"].ToString().Trim();
                        DeptName = row["DEPTNAME"].ToString().Trim();
                    }
                }
            }
        }

        public HierarchyEntity(DataRow userHierarchy, string tableName, string Type)
        {
            if (userHierarchy != null && Type != "frmHIEDEFSCRN")
            {
                DataRow row = userHierarchy;
                UserID = row["PWH_EMPLOYEE_NO"].ToString().Trim();
                CaseWorker = row["PWH_CASEWORKER"].ToString().Trim();
                Security = row["PWH_SECURITY"].ToString().Trim();
                InActiveFlag = row["PWH_INACTIVE"].ToString().Trim();
                HirarchyName = row["NAME"].ToString().Trim();
                if (tableName == "ADMNB002")
                {
                    Agency = row["PWH_AGENCY"].ToString().Trim();
                    Dept = row["PWH_DEPT"].ToString().Trim();
                    Prog = row["PWH_PROG"].ToString().Trim();
                }
            }

            if (Type == "frmHIEDEFSCRN")
            {
                DataRow row = userHierarchy;
                Code = row["CODE"].ToString().Trim();
                Agency = row["HIE_AGENCY"].ToString().Trim();
                Dept = row["HIE_DEPT"].ToString().Trim();
                Prog = row["HIE_PROGRAM"].ToString().Trim();
                HirarchyName = row["HIE_NAME"].ToString().Trim();
                ShortName = row["HIE_SHORT_NAME"].ToString().Trim();
                Intake = row["HIE_ALT_INTAKE"].ToString().Trim();
                HIERepresentation = row["HIE_REPRSNTN"].ToString().Trim();
                HIEProg = row["HIE_PROG"].ToString().Trim();
                CNFormat = row["HIE_CN_FORMAT"].ToString().Trim();
                CWFormat = row["HIE_CW_FORMAT"].ToString().Trim();
                DateLSTC = row["HIE_DATE_LSTC"].ToString().Trim();
                LSTCOperator = row["HIE_LSTC_OPERATOR"].ToString().Trim();
                DateAdd = row["HIE_DATE_ADD"].ToString().Trim();
                AddOperator = row["HIE_ADD_OPERATOR"].ToString().Trim();
                Logo = row["HIE_LOGO"].ToString().Trim();


                IntakeCNT = row["INTAKESCNT"].ToString().Trim();
                ACTProgCNT = row["ACTPROGCNT"].ToString().Trim();
                MSProgCNT = row["MSPROGCNT"].ToString().Trim();

                if (row.Table.Columns.Contains("HIE_SNAME"))
                {
                    SpanishName = row["HIE_SNAME"].ToString().Trim();
                }
                else
                    SpanishName = string.Empty;

                if (row.Table.Columns.Contains("HIE_SEND_PIP"))
                {
                    SendtoPIP = row["HIE_SEND_PIP"].ToString().Trim();
                }
                else
                    SendtoPIP = string.Empty;

                if (row.Table.Columns.Contains("HIE_PIP_ACTIVE"))
                {
                    PIPActive = row["HIE_PIP_ACTIVE"].ToString().Trim();
                }
                else
                    PIPActive = string.Empty;





                if (tableName.Equals("DEFAULT"))
                {
                    AgencyName = row["AGENCYNAME"].ToString().Trim();
                    DeptName = row["DEPTNAME"].ToString().Trim();
                }
            }
        }

        public HierarchyEntity(DataRow row, char SerPlan)
        {
            if (SerPlan == 'S')
            {
                Code = row["Hierarchy"].ToString().Trim();
                HirarchyName = row["Hie_Name"].ToString().Trim();
                Agency = row["Agency"].ToString().Trim();
                Dept = row["Dept"].ToString().Trim();
                Prog = row["Prog"].ToString().Trim();
            }
        }


        #endregion

        #region Properties

        public string UserID { get; set; }
        public string Mode { get; set; }
        public string Code { get; set; }
        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Prog { get; set; }

        public string HirarchyType { get; set; }
        public string HirarchyName { get; set; }
        public string AgencyName { get; set; }
        public string DeptName { get; set; }
        public string CaseWorker { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MI { get; set; }
        public string Security { get; set; }
        public string UsedFlag { get; set; }
        public string InActiveFlag { get; set; }
        public string ShortName { get; set; }
        public string Intake { get; set; }
        public string HIERepresentation { get; set; }
        public string HIEProg { get; set; }
        public string CNFormat { get; set; }
        public string CWFormat { get; set; }
        public string DateLSTC { get; set; }
        public string LSTCOperator { get; set; }
        public string DateAdd { get; set; }
        public string AddOperator { get; set; }
        public string Sites { get; set; }
        public string SpanishName { get; set; }
        public string SendtoPIP { get; set; }
        public string PIPActive { get; set; }

        public string Logo { get; set; }

        public string SerAgency { get; set; }
        public string SerDept { get; set; }
        public string SerProg { get; set; }

        public string IntakeCNT { get; set; }
        public string ACTProgCNT { get; set; }
        public string MSProgCNT { get; set; }


        #endregion


    }

    public class CLINQHIEEntity
    {
        #region Constructors

        public CLINQHIEEntity()
        {
            Code = string.Empty;
            UserID = string.Empty;
            Agency = string.Empty;
            Dept = string.Empty;
            Prog = string.Empty;
            CLINQPdf = string.Empty;
            CLINQCNotes = string.Empty;
            //LastName = string.Empty;
            //FirstName = string.Empty;
            //MI = string.Empty;
            //Security = string.Empty;

            UsedFlag = string.Empty;
            //InActiveFlag = string.Empty;
            DateLSTC = string.Empty;
            LSTCOperator = string.Empty;
            DateAdd = string.Empty;
            AddOperator = string.Empty;
            HirarchyName = string.Empty;
            //ShortName = string.Empty;
            //Intake = string.Empty;
            //HIERepresentation = string.Empty;
            //HIEProg = string.Empty;
            //CNFormat = string.Empty;
            //CWFormat = string.Empty;
            //AgencyName = string.Empty;
            //DeptName = string.Empty;
        }

        public CLINQHIEEntity(DataRow userHierarchy)
        {
            if (userHierarchy != null)
            {
                DataRow row = userHierarchy;
                Code = row["CODE"].ToString().Trim();
                UserID = row["CLINQ_USER_ID"].ToString().Trim();
                Agency = row["CLINQ_AGENCY"].ToString().Trim();
                Dept = row["CLINQ_DEPT"].ToString().Trim();
                Prog = row["CLINQ_PROGRAM"].ToString().Trim();
                CLINQPdf = row["CLINQ_PDF"].ToString().Trim();
                CLINQCNotes = row["CLINQ_CNOTES"].ToString().Trim();
                //LastName = row["PWH_LAST_NAME"].ToString().Trim();
                //FirstName = row["PWH_FIRST_NAME"].ToString().Trim();
                //MI = row["PWH_MI_NAME"].ToString().Trim();
                //Security = row["PWH_SECURITY"].ToString().Trim();

                UsedFlag = row["CLINQ_USED_FLAG"].ToString().Trim();
                //InActiveFlag = row["PWH_INACTIVE"].ToString().Trim();
                DateLSTC = row["CLINQ_DATE_LSTC"].ToString().Trim();
                LSTCOperator = row["CLINQ_LSTC_OPERATOR"].ToString().Trim();
                DateAdd = row["CLINQ_DATE_ADD"].ToString().Trim();
                AddOperator = row["CLINQ_ADD_OPERATOR"].ToString().Trim();
                HirarchyName = row["HIE_NAME"].ToString().Trim();
            }
        }



        #endregion

        #region Properties

        public string UserID { get; set; }
        public string Mode { get; set; }
        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Prog { get; set; }
        public string CLINQPdf { get; set; }
        public string CLINQCNotes { get; set; }
        public string HirarchyName { get; set; }
        public string Code { get; set; }
        //public string CaseWorker { get; set; }
        //public string LastName { get; set; }
        //public string FirstName { get; set; }
        //public string MI { get; set; }
        //public string Security { get; set; }
        public string UsedFlag { get; set; }
        //public string InActiveFlag { get; set; }
        //public string ShortName { get; set; }
        //public string Intake { get; set; }
        //public string HIERepresentation { get; set; }
        //public string HIEProg { get; set; }
        //public string CNFormat { get; set; }
        //public string CWFormat { get; set; }
        public string DateLSTC { get; set; }
        public string LSTCOperator { get; set; }
        public string DateAdd { get; set; }
        public string AddOperator { get; set; }

        #endregion


    }

    public class PASSWORDSERENTITY
    {
        #region Constructors

        public PASSWORDSERENTITY()
        {
            Code = string.Empty;
            UserID = string.Empty;
            //PWSH_EMPLOYEE_NO = string.Empty;
            PWSIH_AGENCY = string.Empty;
            PWSIH_DEPT = string.Empty;
            PWSIH_PROG = string.Empty;
            PWSH_AGENCY = string.Empty;
            PWSH_DEPT = string.Empty;
            PWSH_PROG = string.Empty;
            PWSH_LAST_NAME = string.Empty;
            PWSH_FIRST_NAME = string.Empty;
            PWSH_MI_NAME = string.Empty;
            PWSH_CASEWORKER = string.Empty;
            PWSH_SECURITY = string.Empty;
            PWSH_USED_FLAG = string.Empty;
            PWSH_INACTIVE = string.Empty;
            PWSH_DATE_LSTC = string.Empty;
            PWSH_LSTC_OPERATOR = string.Empty;
            PWSH_DATE_ADD = string.Empty;
            PWSH_ADD_OPERATOR = string.Empty;
            PWSH_SITES = string.Empty;
            ShortName = string.Empty;
            Intake = string.Empty;
            HIERepresentation = string.Empty;
            HIEProg = string.Empty;
            CNFormat = string.Empty;
            CWFormat = string.Empty;
            //AgencyName = string.Empty;
            //DeptName = string.Empty;
            //PIPActive = string.Empty;
            Logo = string.Empty;
        }

        public PASSWORDSERENTITY(DataRow userHierarchy)
        {
            if (userHierarchy != null)
            {
                DataRow row = userHierarchy;
                Code = row["CODE"].ToString().Trim();
                UserID = row["PWSH_EMPLOYEE_NO"].ToString().Trim();
                //HirarchyType = row["PWH_TYPE"].ToString().Trim();
                PWSIH_AGENCY = row["PWSH_EMPLOYEE_NO"].ToString().Trim();
                PWSIH_DEPT = row["PWSH_EMPLOYEE_NO"].ToString().Trim();
                PWSIH_PROG = row["PWSH_EMPLOYEE_NO"].ToString().Trim();
                PWSH_AGENCY = row["PWSH_EMPLOYEE_NO"].ToString().Trim();
                PWSH_DEPT = row["PWSH_EMPLOYEE_NO"].ToString().Trim();
                PWSH_PROG = row["PWSH_EMPLOYEE_NO"].ToString().Trim();
                PWSH_LAST_NAME = row["PWSH_EMPLOYEE_NO"].ToString().Trim();
                PWSH_FIRST_NAME = row["PWSH_EMPLOYEE_NO"].ToString().Trim();
                PWSH_MI_NAME = row["PWSH_EMPLOYEE_NO"].ToString().Trim();
                PWSH_CASEWORKER = row["PWSH_EMPLOYEE_NO"].ToString().Trim();
                PWSH_SECURITY = row["PWSH_EMPLOYEE_NO"].ToString().Trim();
                PWSH_USED_FLAG = row["PWSH_EMPLOYEE_NO"].ToString().Trim();
                PWSH_INACTIVE = row["PWSH_EMPLOYEE_NO"].ToString().Trim();
                PWSH_DATE_LSTC = row["PWSH_EMPLOYEE_NO"].ToString().Trim();
                PWSH_LSTC_OPERATOR = row["PWSH_EMPLOYEE_NO"].ToString().Trim();
                PWSH_DATE_ADD = row["PWSH_EMPLOYEE_NO"].ToString().Trim();
                PWSH_ADD_OPERATOR = row["PWSH_EMPLOYEE_NO"].ToString().Trim();
                PWSH_SITES = row["PWSH_EMPLOYEE_NO"].ToString().Trim();
                // Logo= row["HIE_LOGO"].ToString().Trim();
            }
        }

        public PASSWORDSERENTITY(string code, string desc)
        {
            Code = code;
            ShortName = desc;
        }

        //public PASSWORDSERENTITY(DataRow userHierarchy, string tableName)
        //{
        //    if (tableName == "PASSWORDHIEBYID")
        //    {
        //        DataRow row = userHierarchy;
        //        Code = row["CODE"].ToString().Trim();
        //        Agency = row["PWH_AGENCY"].ToString().Trim();
        //        Dept = row["PWH_DEPT"].ToString().Trim();
        //        Prog = row["PWH_PROG"].ToString().Trim();
        //    }
        //    else
        //    {
        //        if (userHierarchy != null)
        //        {
        //            DataRow row = userHierarchy;
        //            Code = row["CODE"].ToString().Trim();
        //            Agency = row["HIE_AGENCY"].ToString().Trim();
        //            Dept = row["HIE_DEPT"].ToString().Trim();
        //            Prog = row["HIE_PROGRAM"].ToString().Trim();
        //            HirarchyName = row["HIE_NAME"].ToString().Trim();
        //            ShortName = row["HIE_SHORT_NAME"].ToString().Trim();
        //            Intake = row["HIE_ALT_INTAKE"].ToString().Trim();
        //            HIERepresentation = row["HIE_REPRSNTN"].ToString().Trim();
        //            HIEProg = row["HIE_PROG"].ToString().Trim();
        //            CNFormat = row["HIE_CN_FORMAT"].ToString().Trim();
        //            CWFormat = row["HIE_CW_FORMAT"].ToString().Trim();
        //            DateLSTC = row["HIE_DATE_LSTC"].ToString().Trim();
        //            LSTCOperator = row["HIE_LSTC_OPERATOR"].ToString().Trim();
        //            DateAdd = row["HIE_DATE_ADD"].ToString().Trim();
        //            AddOperator = row["HIE_ADD_OPERATOR"].ToString().Trim();
        //            Logo = row["HIE_LOGO"].ToString().Trim();


        //            if (row.Table.Columns.Contains("HIE_SNAME"))
        //            {
        //                SpanishName = row["HIE_SNAME"].ToString().Trim();
        //            }
        //            else
        //                SpanishName = string.Empty;

        //            if (row.Table.Columns.Contains("HIE_SEND_PIP"))
        //            {
        //                SendtoPIP = row["HIE_SEND_PIP"].ToString().Trim();
        //            }
        //            else
        //                SendtoPIP = string.Empty;

        //            if (row.Table.Columns.Contains("HIE_PIP_ACTIVE"))
        //            {
        //                PIPActive = row["HIE_PIP_ACTIVE"].ToString().Trim();
        //            }
        //            else
        //                PIPActive = string.Empty;





        //            if (tableName.Equals("DEFAULT"))
        //            {
        //                AgencyName = row["AGENCYNAME"].ToString().Trim();
        //                DeptName = row["DEPTNAME"].ToString().Trim();
        //            }
        //        }
        //    }
        //}

        //public PASSWORDSERENTITY(DataRow userHierarchy, string tableName, string Type)
        //{
        //    if (userHierarchy != null)
        //    {
        //        DataRow row = userHierarchy;
        //        UserID = row["PWSH_EMPLOYEE_NO"].ToString().Trim();
        //        CaseWorker = row["PWSH_CASEWORKER"].ToString().Trim();
        //        Security = row["PWSH_SECURITY"].ToString().Trim();
        //        InActiveFlag = row["PWSH_INACTIVE"].ToString().Trim();
        //        HirarchyName = row["NAME"].ToString().Trim();
        //        if (tableName == "ADMNB002")
        //        {
        //            Agency = row["PWH_AGENCY"].ToString().Trim();
        //            Dept = row["PWH_DEPT"].ToString().Trim();
        //            Prog = row["PWH_PROG"].ToString().Trim();
        //        }
        //    }
        //}

        //public PASSWORDSERENTITY(DataRow row, char SerPlan)
        //{
        //    if (SerPlan == 'S')
        //    {
        //        Code = row["Hierarchy"].ToString().Trim();
        //        HirarchyName = row["Hie_Name"].ToString().Trim();
        //        Agency = row["Agency"].ToString().Trim();
        //        Dept = row["Dept"].ToString().Trim();
        //        Prog = row["Prog"].ToString().Trim();
        //    }
        //}


        #endregion

        #region Properties

        public string UserID { get; set; }
        public string Mode { get; set; }
        public string Code { get; set; }
        public string PWSIH_AGENCY { get; set; }
        public string PWSIH_DEPT { get; set; }
        public string PWSIH_PROG { get; set; }
        public string PWSH_AGENCY { get; set; }
        public string PWSH_DEPT { get; set; }
        public string PWSH_PROG { get; set; }
        public string PWSH_LAST_NAME { get; set; }
        public string PWSH_FIRST_NAME { get; set; }
        public string PWSH_MI_NAME { get; set; }
        public string PWSH_CASEWORKER { get; set; }
        //public string MI { get; set; }
        public string PWSH_SECURITY { get; set; }
        public string PWSH_USED_FLAG { get; set; }
        public string PWSH_INACTIVE { get; set; }
        public string ShortName { get; set; }
        public string Intake { get; set; }
        public string HIERepresentation { get; set; }
        public string HIEProg { get; set; }
        public string CNFormat { get; set; }
        public string CWFormat { get; set; }
        public string PWSH_DATE_LSTC { get; set; }
        public string PWSH_LSTC_OPERATOR { get; set; }
        public string PWSH_DATE_ADD { get; set; }
        public string PWSH_ADD_OPERATOR { get; set; }
        public string PWSH_SITES { get; set; }
        //public string SpanishName { get; set; }
        //public string SendtoPIP { get; set; }
        //public string PIPActive { get; set; }

        public string Logo { get; set; }
        #endregion


    }

}
