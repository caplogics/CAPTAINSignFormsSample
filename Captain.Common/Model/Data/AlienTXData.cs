/***********************************************************
 * Author : Kranthi
 * Date   : 02/14/2024
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
    public class AlienTXData
    {
        #region Properties
        public CaptainModel Model { get; set; }
        public string UserId { get; set; }
        #endregion

        public AlienTXData()
        {

        }



        public bool INSUPDEL_ALIENVER(string ALN_VER_ID, string ALN_VER_AGY, string ALN_VER_DEPT, string ALN_VER_PROG, string ALN_VER_YEAR, string ALN_VER_APPNO,
        string ALN_VER_FAM_SEQ, string ALN_VER_CITIZEN, string ALN_VER_IDENT, string ALN_VER_OPERATOR, string MODE)
        {
            bool boolStatus = false;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                if (ALN_VER_ID != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ALN_VER_ID", ALN_VER_ID));

                if (ALN_VER_AGY != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ALN_VER_AGY", ALN_VER_AGY));
                if (ALN_VER_DEPT != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ALN_VER_DEPT", ALN_VER_DEPT));
                if (ALN_VER_PROG != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ALN_VER_PROG", ALN_VER_PROG));
                //if (ALN_VER_YEAR != string.Empty)
                sqlParamList.Add(new SqlParameter("@ALN_VER_YEAR", ALN_VER_YEAR));

                if (ALN_VER_APPNO != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ALN_VER_APPNO", ALN_VER_APPNO));

                if (ALN_VER_FAM_SEQ != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ALN_VER_FAM_SEQ", ALN_VER_FAM_SEQ));
                if (ALN_VER_CITIZEN != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ALN_VER_CITIZEN", ALN_VER_CITIZEN));

                if (ALN_VER_IDENT != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ALN_VER_IDENT", ALN_VER_IDENT));

                if (ALN_VER_OPERATOR != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ALN_VER_OPERATOR", ALN_VER_OPERATOR));

                if (MODE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MODE", MODE));

                boolStatus = AlienTXDB.iINSUPDELALIENVER(sqlParamList);
            }
            catch (Exception ex)
            {
                boolStatus = false;
            }
            return boolStatus;
        }

        public bool INSUPDEL_DOCSIGNHIS(string DCSN_HIS_ID, string DCSN_HIS_AGY, string DCSN_HIS_DEPT, string DCSN_HIS_PROG, string DCSN_HIS_YEAR, string DCSN_HIS_APPNO
, string DCSN_HIS_DOC_CODE, string DCSN_Email, string DCSN_HIS_SIGN_REQ, string DCSN_HIS_SIGND_ON, string DCSN_MOVD_CAP, 
            string DCSN_HIS_LSTC_OPERATOR,string DCSN_HIS_DOC_SEQ, string DCSN_HIS_DOC_NAME, string DCSN_PRINT_FOR, string isemailSend, 
            string emailStatus, string emailstatusDate, string MODE)
        {
            bool boolStatus = false;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                if (DCSN_HIS_ID != string.Empty)
                    sqlParamList.Add(new SqlParameter("@DCSN_HIS_ID", DCSN_HIS_ID));

                if (DCSN_HIS_AGY != string.Empty)
                    sqlParamList.Add(new SqlParameter("@DCSN_HIS_AGY", DCSN_HIS_AGY));
                if (DCSN_HIS_DEPT != string.Empty)
                    sqlParamList.Add(new SqlParameter("@DCSN_HIS_DEPT", DCSN_HIS_DEPT));
                if (DCSN_HIS_PROG != string.Empty)
                    sqlParamList.Add(new SqlParameter("@DCSN_HIS_PROG", DCSN_HIS_PROG));
                //if (ALN_VER_YEAR != string.Empty)
                sqlParamList.Add(new SqlParameter("@DCSN_HIS_YEAR", DCSN_HIS_YEAR));

                if (DCSN_HIS_APPNO != string.Empty)
                    sqlParamList.Add(new SqlParameter("@DCSN_HIS_APPNO", DCSN_HIS_APPNO));

                if (DCSN_HIS_DOC_CODE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@DCSN_HIS_DOC_CODE", DCSN_HIS_DOC_CODE));
                if (DCSN_Email != string.Empty)
                    sqlParamList.Add(new SqlParameter("@DCSN_Email", DCSN_Email));

                if (DCSN_HIS_SIGN_REQ != string.Empty)
                    sqlParamList.Add(new SqlParameter("@DCSN_HIS_SIGN_REQ", DCSN_HIS_SIGN_REQ));

                if (DCSN_HIS_SIGND_ON != string.Empty)
                    sqlParamList.Add(new SqlParameter("@DCSN_HIS_SIGND_ON", DCSN_HIS_SIGND_ON));
                if (DCSN_MOVD_CAP != string.Empty)
                    sqlParamList.Add(new SqlParameter("@DCSN_MOVD_CAP", DCSN_MOVD_CAP));

                if (DCSN_HIS_LSTC_OPERATOR != string.Empty)
                    sqlParamList.Add(new SqlParameter("@DCSN_HIS_LSTC_OPERATOR", DCSN_HIS_LSTC_OPERATOR));

                if (DCSN_HIS_DOC_SEQ != string.Empty)
                    sqlParamList.Add(new SqlParameter("@DCSN_HIS_DOC_SEQ", DCSN_HIS_DOC_SEQ));

                if (DCSN_HIS_DOC_NAME != string.Empty)
                    sqlParamList.Add(new SqlParameter("@DCSN_HIS_DOC_NAME", DCSN_HIS_DOC_NAME));

                if (DCSN_PRINT_FOR != string.Empty)
                    sqlParamList.Add(new SqlParameter("@DCSN_PRINT_FOR", DCSN_PRINT_FOR));

                if (isemailSend != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ISSENDEMAIL", isemailSend));

                if (emailStatus != string.Empty)
                    sqlParamList.Add(new SqlParameter("@DCSN_EMAIL_STATUS", emailStatus));
                if (emailstatusDate != string.Empty)
                    sqlParamList.Add(new SqlParameter("@DCSN_EMAIL_STATUS_DATE", emailstatusDate));

                if (MODE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MODE", MODE));

                boolStatus = AlienTXDB.iINSUPDEL_DOCSIGNHIS(sqlParamList);
            }
            catch (Exception ex)
            {
                boolStatus = false;
            }
            return boolStatus;
        }

        //public bool INSUPDEL_CRSWK_CAPTAIN(string ISAPPLICANT, string ISEARNER, string FName, string DOB, string MIDFamilyID, DataTable _dtCRSWK_MST_TYPE, DataTable _dtCRSWK_SNP_TYPE, DataTable _dtCRSWK_INCOME_TYPE, string Mode, out string strResstatus)
        //{
        //    bool boolStatus = false; string outValue = "";
        //    try
        //    {
        //        List<SqlParameter> sqlParamList = new List<SqlParameter>();

        //        if (ISAPPLICANT != "")
        //            sqlParamList.Add(new SqlParameter("@ISAPPLICANT", ISAPPLICANT));

        //        if (ISEARNER != "")
        //            sqlParamList.Add(new SqlParameter("@ISEARNER", ISEARNER));

        //        if (FName != "")
        //            sqlParamList.Add(new SqlParameter("@FName", FName));

        //        if (DOB != "")
        //            sqlParamList.Add(new SqlParameter("@DOB", DOB));

        //        if (MIDFamilyID != "")
        //            sqlParamList.Add(new SqlParameter("@MIDFAMILYID", MIDFamilyID));

        //        if (_dtCRSWK_MST_TYPE != null)
        //        {
        //            if (_dtCRSWK_MST_TYPE.Rows.Count > 0)
        //                sqlParamList.Add(new SqlParameter("@CRSWK_MST_TYPE", _dtCRSWK_MST_TYPE));
        //        }

        //        if (_dtCRSWK_SNP_TYPE != null)
        //        {
        //            if (_dtCRSWK_SNP_TYPE.Rows.Count > 0)
        //                sqlParamList.Add(new SqlParameter("@CRSWK_SNP_TYPE", _dtCRSWK_SNP_TYPE));
        //        }
        //        if (_dtCRSWK_INCOME_TYPE != null)
        //        {
        //            if (_dtCRSWK_INCOME_TYPE.Rows.Count > 0)
        //                sqlParamList.Add(new SqlParameter("@CRSWK_INCOME_TYPE", _dtCRSWK_INCOME_TYPE));
        //        }

        //        if (Mode != "")
        //            sqlParamList.Add(new SqlParameter("@MODE", Mode));


        //        SqlParameter sqlOutSeq = new SqlParameter("@strResstatus", SqlDbType.VarChar, 50);
        //        sqlOutSeq.Direction = ParameterDirection.Output;
        //        sqlParamList.Add(sqlOutSeq);

        //        boolStatus = CrossWalkDB.iINSUPDEL_CRSWK_CAPTAIN(sqlParamList);
        //        outValue = sqlOutSeq.Value.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        boolStatus = false;
        //    }
        //    strResstatus = outValue;
        //    return boolStatus;
        //}





    }
}
