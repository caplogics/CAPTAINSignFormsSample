/***********************************************************
 * Author : Kranthi
 * Date   : 12/29/2022
 * ********************************************************/
#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Captain.Common.Model.Objects;
using System.Data;
using System.Data.SqlClient;
using Captain.DatabaseLayer;
using NPOI.SS.Formula.Functions;
using DevExpress.CodeParser;
#endregion

namespace Captain.Common.Model.Data
{
    public class CrossWalkData
    {
        #region Properties
        public CaptainModel Model { get; set; }
        public string UserId { get; set; }
        #endregion

        public CrossWalkData()
        {

        }



        public bool INSUPDELCRSWALK(string CRSWK_ID, string CRSWK_AGY, string CRSWK_DEPT, string CRSWK_PROG, string CRSWK_YEAR, string CRSWK_CATEGORY,
            string CRSWK_CATEGORY_ID, string CRSWK_FILE_CODE, string CRSWK_XL_NAME, string CRSWK_RECS_COUNT, string CRSWK_JSON_NAME, string CRSWK_WORK_TYPE, string CRSWK_STATUS, string CRSWK_USER, string MODE)
        {
            bool boolStatus = false;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                if (CRSWK_ID != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CRSWK_ID", CRSWK_ID));

                if (CRSWK_AGY != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CRSWK_AGY", CRSWK_AGY));
                if (CRSWK_DEPT != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CRSWK_DEPT", CRSWK_DEPT));
                if (CRSWK_PROG != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CRSWK_PROG", CRSWK_PROG));
                if (CRSWK_YEAR != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CRSWK_YEAR", CRSWK_YEAR));
                if (CRSWK_CATEGORY != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CRSWK_CATEGORY", CRSWK_CATEGORY));

                if (CRSWK_CATEGORY_ID != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CRSWK_CATEGORY_ID", CRSWK_CATEGORY_ID));
                if (CRSWK_FILE_CODE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CRSWK_FILE_CODE", CRSWK_FILE_CODE));
                if (CRSWK_XL_NAME != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CRSWK_XL_NAME", CRSWK_XL_NAME));
                if (CRSWK_RECS_COUNT != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CRSWK_RECS_COUNT", CRSWK_RECS_COUNT));
                if (CRSWK_JSON_NAME != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CRSWK_JSON_NAME", CRSWK_JSON_NAME));
                if (CRSWK_WORK_TYPE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CRSWK_WORK_TYPE", CRSWK_WORK_TYPE));
                if (CRSWK_STATUS != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CRSWK_STATUS", CRSWK_STATUS));
                if (CRSWK_USER != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CRSWK_USER", CRSWK_USER));
                if (MODE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MODE", MODE));

                boolStatus = CrossWalkDB.iINSUPDELCRSWALK(sqlParamList);
            }
            catch (Exception ex)
            {
                boolStatus = false;
            }
            return boolStatus;
        }

        public bool INSUPDELCRSWKSETTINGS(string CRSWK_SET_ID
            , string CRSWK_CAT_ID
           , string CRSWK_SET_FUN_FOR
           , string CRSWK_SET_FUNCTION
           , string CRSWK_OPERATOR
            , string CRSWK_SET_HIE
           , string MODE)
        {
            bool boolStatus = false;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                if (CRSWK_SET_ID != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CRSWK_SET_ID", CRSWK_SET_ID));
                if (CRSWK_CAT_ID != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CRSWK_CAT_ID", CRSWK_CAT_ID));
                if (CRSWK_SET_FUN_FOR != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CRSWK_SET_FUN_FOR", CRSWK_SET_FUN_FOR));
                if (CRSWK_SET_FUNCTION != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CRSWK_SET_FUNCTION", CRSWK_SET_FUNCTION));
                if (CRSWK_OPERATOR != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CRSWK_OPERATOR", CRSWK_OPERATOR));
                if (CRSWK_SET_HIE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CRSWK_SET_HIE", CRSWK_SET_HIE));

                if (MODE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MODE", MODE));
                boolStatus = CrossWalkDB.iINSUPDELCRSWKSETTINGS(sqlParamList);
            }
            catch (Exception ex)
            {
                boolStatus = false;
            }
            return boolStatus;
        }


        public bool INSUPDEL_CRSWK_MIDKEY(DataTable _dt, string Agy, string Dept, string Prog, string Year, string CategoryID, string MODE)
        {
            bool boolStatus = false;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                if (_dt.Rows.Count > 0)
                {

                    sqlParamList.Add(new SqlParameter("@CRSWKMIDKEY_TYPE", _dt));
                }

                if (Agy != string.Empty)
                    sqlParamList.Add(new SqlParameter("@AGY", Agy));
                if (Dept != string.Empty)
                    sqlParamList.Add(new SqlParameter("@DEPT", Dept));
                if (Prog != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PROG", Prog));
                if (Year != string.Empty)
                    sqlParamList.Add(new SqlParameter("@YEAR", Year));
                if (CategoryID != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CATID", CategoryID));

                if (MODE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MODE", MODE));
                boolStatus = CrossWalkDB.iINSUPDEL_CRSWK_MIDKEY(sqlParamList);
            }
            catch (Exception ex)
            {
                boolStatus = false;
            }
            return boolStatus;
        }

        public bool INSUPDEL_CRSWK_CAPTAIN(string MKEY_CRSWK_CAT_ID, string MKEY_CRSWK_CODE, string _Agy, string _Dept, string _prog, string _year, string Mode, out string strResstatus)
        {
            bool boolStatus = false; string outValue = "";
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                if (MKEY_CRSWK_CAT_ID != "")
                    sqlParamList.Add(new SqlParameter("@MKEY_CRSWK_CAT_ID", MKEY_CRSWK_CAT_ID));

                if (MKEY_CRSWK_CODE != "")
                    sqlParamList.Add(new SqlParameter("@MKEY_CRSWK_CODE", MKEY_CRSWK_CODE));

                if (_Agy != "")
                    sqlParamList.Add(new SqlParameter("@_AGY", _Agy));
                if (_Dept != "")
                    sqlParamList.Add(new SqlParameter("@_DEPT", _Dept));
                if (_prog != "")
                    sqlParamList.Add(new SqlParameter("@_PRG", _prog));
                if (_year != "")
                    sqlParamList.Add(new SqlParameter("@_YEAR", _year));


                if (Mode != "")
                    sqlParamList.Add(new SqlParameter("@MODE", Mode));

                SqlParameter sqlOutSeq = new SqlParameter("@strResstatus", SqlDbType.VarChar, 50);
                sqlOutSeq.Direction = ParameterDirection.Output;
                sqlParamList.Add(sqlOutSeq);

                boolStatus = CrossWalkDB.iINSUPDEL_CRSWK_CAPTAIN(sqlParamList);
                outValue = sqlOutSeq.Value.ToString();
            }
            catch (Exception ex)
            {
                boolStatus = false;
            }
            strResstatus = outValue;
            return boolStatus;
        }
        public bool INSUPDEL_CRSWK_CAPTAIN(string ISAPPLICANT, string ISEARNER, string FName, string DOB, string MIDFamilyID, DataTable _dtCRSWK_MST_TYPE, DataTable _dtCRSWK_SNP_TYPE, DataTable _dtCRSWK_INCOME_TYPE, string Mode, out string strResstatus)
        {
            bool boolStatus = false; string outValue = "";
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                if (ISAPPLICANT != "")
                    sqlParamList.Add(new SqlParameter("@ISAPPLICANT", ISAPPLICANT));

                if (ISEARNER != "")
                    sqlParamList.Add(new SqlParameter("@ISEARNER", ISEARNER));

                if (FName != "")
                    sqlParamList.Add(new SqlParameter("@FName", FName));

                if (DOB != "")
                    sqlParamList.Add(new SqlParameter("@DOB", DOB));

                if (MIDFamilyID != "")
                    sqlParamList.Add(new SqlParameter("@MIDFAMILYID", MIDFamilyID));

                if (_dtCRSWK_MST_TYPE != null)
                {
                    if (_dtCRSWK_MST_TYPE.Rows.Count > 0)
                        sqlParamList.Add(new SqlParameter("@CRSWK_MST_TYPE", _dtCRSWK_MST_TYPE));
                }

                if (_dtCRSWK_SNP_TYPE != null)
                {
                    if (_dtCRSWK_SNP_TYPE.Rows.Count > 0)
                        sqlParamList.Add(new SqlParameter("@CRSWK_SNP_TYPE", _dtCRSWK_SNP_TYPE));
                }
                if (_dtCRSWK_INCOME_TYPE != null)
                {
                    if (_dtCRSWK_INCOME_TYPE.Rows.Count > 0)
                        sqlParamList.Add(new SqlParameter("@CRSWK_INCOME_TYPE", _dtCRSWK_INCOME_TYPE));
                }

                if (Mode != "")
                    sqlParamList.Add(new SqlParameter("@MODE", Mode));


                SqlParameter sqlOutSeq = new SqlParameter("@strResstatus", SqlDbType.VarChar, 50);
                sqlOutSeq.Direction = ParameterDirection.Output;
                sqlParamList.Add(sqlOutSeq);

                boolStatus = CrossWalkDB.iINSUPDEL_CRSWK_CAPTAIN(sqlParamList);
                outValue = sqlOutSeq.Value.ToString();
            }
            catch (Exception ex)
            {
                boolStatus = false;
            }
            strResstatus = outValue;
            return boolStatus;
        }


        public bool CRSWK_DEL_CAPRECS(string AGY, string DEPT, string PROG, string YEAR, string MKEY_CRSWK_CODE, string MKEY_CRSWK_CAT_ID, string MKEY_XL_FAM_ID,
        string MKEY_XL_CHILDPLUS_ID, string MKEY_XL_CHILDPLUS_ID1, string MKEY_XL_FNAME, string MKEY_XL_LNAME, string MKEY_XL_DOB, string OPERATOR, string SNP_APP, string SNP_FAMILY_SEQ, string MODE)
        {
            bool boolStatus = false; string outValue = "";
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                if (AGY != "")
                    sqlParamList.Add(new SqlParameter("@AGY", AGY));

                if (DEPT != "")
                    sqlParamList.Add(new SqlParameter("@DEPT", DEPT));

                if (PROG != "")
                    sqlParamList.Add(new SqlParameter("@PROG", PROG));

                if (YEAR != "")
                    sqlParamList.Add(new SqlParameter("@YEAR", YEAR));


                if (MKEY_CRSWK_CODE != "")
                    sqlParamList.Add(new SqlParameter("@MKEY_CRSWK_CODE", MKEY_CRSWK_CODE));

                if (MKEY_CRSWK_CAT_ID != "")
                    sqlParamList.Add(new SqlParameter("@MKEY_CRSWK_CAT_ID", MKEY_CRSWK_CAT_ID));

                if (MKEY_XL_FAM_ID != "")
                    sqlParamList.Add(new SqlParameter("@MKEY_XL_FAM_ID", MKEY_XL_FAM_ID));

                if (MKEY_XL_CHILDPLUS_ID != "")
                    sqlParamList.Add(new SqlParameter("@MKEY_XL_CHILDPLUS_ID", MKEY_XL_CHILDPLUS_ID));



                if (MKEY_XL_CHILDPLUS_ID1 != "")
                    sqlParamList.Add(new SqlParameter("@MKEY_XL_CHILDPLUS_ID1", MKEY_XL_CHILDPLUS_ID1));

                if (MKEY_XL_FNAME != "")
                    sqlParamList.Add(new SqlParameter("@MKEY_XL_FNAME", MKEY_XL_FNAME));

                if (MKEY_XL_LNAME != "")
                    sqlParamList.Add(new SqlParameter("@MKEY_XL_LNAME", MKEY_XL_LNAME));

                if (MKEY_XL_DOB != "")
                    sqlParamList.Add(new SqlParameter("@MKEY_XL_DOB", MKEY_XL_DOB));

                if (OPERATOR != "")
                    sqlParamList.Add(new SqlParameter("@OPERATOR", OPERATOR));
                if (SNP_APP != "")
                    sqlParamList.Add(new SqlParameter("@SNP_APP", SNP_APP));
                if (SNP_FAMILY_SEQ != "")
                    sqlParamList.Add(new SqlParameter("@SNP_FAMILY_SEQ", SNP_FAMILY_SEQ));


                if (MODE != "")
                    sqlParamList.Add(new SqlParameter("@MODE", MODE));

                boolStatus = CrossWalkDB.iCRSWK_DEL_CAPRECS(sqlParamList);
            }
            catch (Exception ex)
            {
                boolStatus = false;
            }
            return boolStatus;
        }


        public bool CRSWK_INSUPDELCASEVER(CaseVerEntity CaseVer, out string strMsg)
        {
            bool boolsuccess;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@VER_AGENCY", CaseVer.Agency));
                sqlParamList.Add(new SqlParameter("@VER_DEPT", CaseVer.Dept));
                sqlParamList.Add(new SqlParameter("@VER_PROGRAM", CaseVer.Program));
                if (CaseVer.Year != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@VER_YEAR", CaseVer.Year));
                }
                if (CaseVer.AppNo != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@VER_APP_NO", CaseVer.AppNo));
                }
                if (CaseVer.VerifyDate != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@VER_VERIFY_DATE", CaseVer.VerifyDate));
                }
                if (CaseVer.Verifier != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@VER_VERIFIER", CaseVer.Verifier));
                }
                if (CaseVer.ReverifyDate != string.Empty && CaseVer.ReverifyDate != null)
                {
                    sqlParamList.Add(new SqlParameter("@VER_REVERIFY_DATE", CaseVer.ReverifyDate));
                }
                if (CaseVer.VerOmb != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@VER_OMB", CaseVer.VerOmb));
                }
                if (CaseVer.VerHud != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@VER_HUD", CaseVer.VerHud));
                }
                if (CaseVer.VerSmi != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@VER_SMI", CaseVer.VerSmi));
                }
                if (CaseVer.VerCmi != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@VER_CMI", CaseVer.VerCmi));
                }
                if (CaseVer.CatElig != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@VER_CAT_ELIG", CaseVer.CatElig));
                }
                if (CaseVer.VerifyW2 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@VER_VERIFY_W2", CaseVer.VerifyW2));
                }
                if (CaseVer.VerifyCheckStub != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@VER_VERIFY_CHECK_STUB", CaseVer.VerifyCheckStub));
                }
                if (CaseVer.VerifyTaxReturn != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@VER_VERIFY_TAX_RETURN", CaseVer.VerifyTaxReturn));
                }
                if (CaseVer.VerifyLetter != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@VER_VERIFY_LETTER", CaseVer.VerifyLetter));
                }
                if (CaseVer.VerifyOther != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@VER_VERIFY_OTHER", CaseVer.VerifyOther));
                }
                if (CaseVer.VerifySelfDecl != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@VER_VERIFY_SELF_DECL", CaseVer.VerifySelfDecl));
                }
                if (CaseVer.IncomeAmount != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@VER_INCOME_AMOUNT", CaseVer.IncomeAmount));
                }
                if (CaseVer.NoInhh != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@VER_NO_INHH", CaseVer.NoInhh));
                }
                if (CaseVer.FundSource != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@VER_FUND_SOURCE", CaseVer.FundSource));
                }
                if (CaseVer.MealElig != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@VER_MEAL_ELIG", CaseVer.MealElig));
                }
                if (CaseVer.Classification != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@VER_CLASSIFICATION", CaseVer.Classification));
                }
                if (CaseVer.VerifyOthCMB != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@VER_VERIFY_OTH_CMB", CaseVer.VerifyOthCMB));
                }
                sqlParamList.Add(new SqlParameter("@VER_ADD_OPERATOR", CaseVer.AddOperator));
                sqlParamList.Add(new SqlParameter("@VER_LSTC_OPERATOR", CaseVer.LstcOperator));
                sqlParamList.Add(new SqlParameter("@Mode", CaseVer.Mode));
                sqlParamList.Add(new SqlParameter("@MstModify", CaseVer.MstModify));
                SqlParameter parameterMsg = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
                parameterMsg.Direction = ParameterDirection.Output;
                sqlParamList.Add(parameterMsg);
                //boolsuccess = CaseMst.InsertUpdateDelCASEVer(sqlParamList);
                boolsuccess = CrossWalkDB.iCRSWK_INSUPDELCASEVER(sqlParamList);
                //if (boolsuccess == false)
                //{
                //    CaseMst.InsertErrorLog("CaseVER", ErrorLogCaseVer(CaseVer), "Record not modified some error", CaseVer.LstcOperator);
                //}
                //if (CaseVer.Mode.ToUpper() == "DELETE")
                //{
                //    CaseMst.InsertErrorLog("CaseVERDelete", ErrorLogCaseVer(CaseVer), "Delete", CaseVer.LstcOperator);
                //}
                strMsg = parameterMsg.Value.ToString();
            }
            catch (Exception ex)
            {
                //CaseMst.InsertErrorLog("CaseVER", ErrorLogCaseVer(CaseVer), ex.Message, CaseVer.LstcOperator);
                strMsg = string.Empty;
                return false;
            }

            return boolsuccess;
        }


        

      

    }
}
