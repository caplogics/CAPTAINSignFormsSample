using Captain.DatabaseLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wisej.Web;

namespace Captain.Common.Model.Data
{
    public class DSSXMLData
    {
        #region Properties
        public CaptainModel Model { get; set; }
        public string UserId { get; set; }
        #endregion

        public DSSXMLData()
        {

        }

        static System.Data.DataTable dtAgencylst()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("AGY", typeof(string));
            dt.Columns.Add("CAPAGY", typeof(string));
            dt.Rows.Add("ACAA", "ACCESS");
            dt.Rows.Add("ACCESS", "ACCESS");
            dt.Rows.Add("ALLIANCE", "ALLIANCE");
            dt.Rows.Add("ACE", "ALLIANCE");
            dt.Rows.Add("CAANH", "CAANH");
            dt.Rows.Add("CRT", "CRT");
            dt.Rows.Add("HRA", "HRANB");
            dt.Rows.Add("NOI", "NOI");
            dt.Rows.Add("TEAM", "TEAM");
            return dt;
        }
        public static string getZIPfileAGY1(string CAPAgency)
        {
            string strRes = "";
            System.Data.DataTable dt = new System.Data.DataTable();
            dt = dtAgencylst();
            if (dt.Rows.Count > 0)
            {

                DataRow[] dr = dt.Select("CAPAGY='" + CAPAgency + "'");
                if (dr.Length > 0)
                    strRes = dr[0]["AGY"].ToString();
            }
            return strRes;
        }
        public static string getZIPfileAGY(string CAPAgency)
        {
            string strRes = "";
            System.Data.DataTable dt = new System.Data.DataTable();
            dt = dtAgencylst();
            if (dt.Rows.Count > 0)
            {

                DataRow[] dr = dt.Select("CAPAGY='" + CAPAgency + "'");
                if (dr.Length > 0)
                {
                    if (dr.Length > 0)
                        strRes = dr[0]["AGY"].ToString();
                    if (dr.Length > 1)
                    {
                        strRes = "";
                        foreach (DataRow drAgy in dr)
                        {
                            strRes += drAgy["AGY"].ToString() + ",";

                        }
                        strRes = strRes.TrimEnd(',');
                    }
                }
            }
            return strRes;
        }
        public static DataTable getZippedFiles(string strConnection, string strAgency, string frmDate, string toDate, string strStatus, string CTZ_APPID, string isAddDate, string Mode)
        {

            DataTable dt = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(strConnection);
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "CTZIPS_GET";
                    cmd.CommandTimeout = 200;
                    cmd.CommandType = CommandType.StoredProcedure;


                    if (strAgency != string.Empty)
                        cmd.Parameters.AddWithValue("@AGENCY", strAgency);
                    if (frmDate != string.Empty)
                        cmd.Parameters.AddWithValue("@FROMDATE", frmDate);
                    if (toDate != string.Empty)
                        cmd.Parameters.AddWithValue("@TODATE", toDate);

                    if (CTZ_APPID != string.Empty)
                        cmd.Parameters.AddWithValue("@CTZ_APPID", CTZ_APPID);

                    if (strStatus != string.Empty)
                        cmd.Parameters.AddWithValue("@CTZ_STATUS", strStatus);

                    if (isAddDate != string.Empty)
                        cmd.Parameters.AddWithValue("@ISADDDATE", isAddDate);

                    if (Mode != string.Empty)
                        cmd.Parameters.AddWithValue("@MODE", Mode);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    da.Fill(dt);


                }
                con.Close();

            }
            catch (Exception ex)
            {


            }
            return dt;

        }

        public static DataTable DSSXMLMID_GET(string strConnection, string frmDate, string toDate, string ApplicantID, string Agency, string Mode)
        {

            DataTable dt = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(strConnection);
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "DSSXMLMID_GET";
                    cmd.CommandTimeout = 200;
                    cmd.CommandType = CommandType.StoredProcedure;


                    if (ApplicantID != string.Empty)
                        cmd.Parameters.AddWithValue("@ApplicantID", ApplicantID);
                    if (frmDate != string.Empty)
                        cmd.Parameters.AddWithValue("@FROMDATE", frmDate);
                    if (toDate != string.Empty)
                        cmd.Parameters.AddWithValue("@TODATE", toDate);
                    if (Agency != string.Empty)
                        cmd.Parameters.AddWithValue("@Agency", Agency);
                    if (Mode != string.Empty)
                        cmd.Parameters.AddWithValue("@MODE", Mode);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    da.Fill(dt);


                }
                con.Close();

            }
            catch (Exception ex)
            {


            }
            return dt;

        }

        public static DataTable DSSXMLMID_GET(string strConnection, string AgyShortName, string Agy, string Dept, string Prog, string Year, string AppNo, string Mode)
        {

            DataTable dt = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(strConnection);
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "DSSXMLMID_GET";
                    cmd.CommandTimeout = 200;
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (AgyShortName != string.Empty)
                        cmd.Parameters.AddWithValue("@Agency", AgyShortName);
                    if (Agy != string.Empty)
                        cmd.Parameters.AddWithValue("@CAP_AGY", Agy);
                    if (Dept != string.Empty)
                        cmd.Parameters.AddWithValue("@CAP_DEPT", Dept);
                    if (Prog != string.Empty)
                        cmd.Parameters.AddWithValue("@CAP_PROG", Prog);
                    if (Year != string.Empty)
                        cmd.Parameters.AddWithValue("@CAP_YEAR", Year);
                    if (AppNo != string.Empty)
                        cmd.Parameters.AddWithValue("@CAP_APPNO", AppNo);
                    if (Mode != string.Empty)
                        cmd.Parameters.AddWithValue("@MODE", Mode);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    da.Fill(dt);


                }
                con.Close();

            }
            catch (Exception ex)
            {


            }
            return dt;

        }

        public static DataTable DSSXMLDOCS_GET(string strConnection, string DSSXML_DOC_APP_ID, string Mode)
        {

            DataTable dt = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(strConnection);
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "DSSXMLDOCS_GET";
                    cmd.CommandTimeout = 200;
                    cmd.CommandType = CommandType.StoredProcedure;


                    if (DSSXML_DOC_APP_ID != string.Empty)
                        cmd.Parameters.AddWithValue("@DSSXML_DOC_APP_ID", DSSXML_DOC_APP_ID);
                    if (Mode != string.Empty)
                        cmd.Parameters.AddWithValue("@MODE", Mode);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    da.Fill(dt);


                }
                con.Close();

            }
            catch (Exception ex)
            {


            }
            return dt;

        }

        public static DataTable FixDssXML(string strConnection, string Agency)
        {

            DataTable dt = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(strConnection);
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "select *,(replace(convert(varchar, CTZ_FILEDATE,101),'/','') +'_'+ replace(convert(varchar, CTZ_FILEDATE,108),':','')) FOLDERNAME from CTZIPS where CTZ_STATUS !='Z' and CTZ_AGENCY='" + Agency + "'";
                    cmd.CommandTimeout = 200;
                    cmd.CommandType = CommandType.Text;


                    //if (DSSXML_DOC_APP_ID != string.Empty)
                    //    cmd.Parameters.AddWithValue("@DSSXML_DOC_APP_ID", DSSXML_DOC_APP_ID);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    da.Fill(dt);


                }
                con.Close();

            }
            catch (Exception ex)
            {


            }
            return dt;

        }

        public static DataTable getREVFeed(string strConnection, string strAgency, string strDept, string strProgram, string strYear, string strApp_No, string strDate, string ShortName)
        {

            DataTable dt = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(strConnection);
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "CAPS_REVFEED_GET";
                    cmd.CommandTimeout = 200;
                    cmd.CommandType = CommandType.StoredProcedure;


                    if (strAgency != string.Empty)
                        cmd.Parameters.AddWithValue("@DRF_AGENCY", strAgency);
                    if (strDept != string.Empty)
                        cmd.Parameters.AddWithValue("@DRF_DEPT", strDept);
                    if (strProgram != string.Empty)
                        cmd.Parameters.AddWithValue("@DRF_PROGRAM", strProgram);

                    if (strYear != string.Empty)
                        cmd.Parameters.AddWithValue("@DRF_YEAR", strYear);

                    if (strApp_No != string.Empty)
                        cmd.Parameters.AddWithValue("@DRF_APPNO", strApp_No);
                    if (strDate != string.Empty)
                        cmd.Parameters.AddWithValue("@DRF_DATE", strDate);

                    if (ShortName != string.Empty)
                        cmd.Parameters.AddWithValue("@DRF_SHORTNAME", ShortName);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    da.Fill(dt);


                }
                con.Close();

            }
            catch (Exception ex)
            {


            }
            return dt;

        }

        public static DataTable DSSCNTPROC_GET(string strConnection, string DCNP_APPID, string DCNP_AGENCY, string Mode)
        {

            DataTable dt = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(strConnection);
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "INSDELGETDSSCNTPROC";
                    cmd.CommandTimeout = 200;
                    cmd.CommandType = CommandType.StoredProcedure;


                    if (DCNP_APPID != string.Empty)
                        cmd.Parameters.AddWithValue("@DCNP_APPID", DCNP_APPID);
                    if (DCNP_AGENCY != string.Empty)
                        cmd.Parameters.AddWithValue("@DCNP_AGENCY", DCNP_AGENCY);
                    if (Mode != string.Empty)
                        cmd.Parameters.AddWithValue("@MODE", Mode);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    da.Fill(dt);


                }
                con.Close();

            }
            catch (Exception ex)
            {


            }
            return dt;

        }
        //public static bool INSERTUPDATECTZIPS(string strConnection, string ID, string CTZ_XML_FILE, string CTZ_COMP_OPERATOR, string MODE)
        //{
        //    bool boolStatus = false;
        //    try
        //    {
        //        SqlConnection con = new SqlConnection(strConnection);
        //        con.Open();
        //        using (SqlCommand cmd = new SqlCommand())
        //        {
        //            cmd.Connection = con;
        //            cmd.CommandText = "INSERTUPDATECTZIPS";
        //            cmd.CommandTimeout = 200;
        //            cmd.CommandType = CommandType.StoredProcedure;

        //            if (ID != string.Empty)
        //                cmd.Parameters.AddWithValue("@ID", ID);

        //            if (CTZ_XML_FILE != string.Empty)
        //                cmd.Parameters.AddWithValue("@CTZ_XML_FILE", CTZ_XML_FILE);

        //            if (CTZ_COMP_OPERATOR != string.Empty)
        //                cmd.Parameters.AddWithValue("@CTZ_COMP_OPERATOR", CTZ_COMP_OPERATOR);
        //            if (MODE != string.Empty)
        //                cmd.Parameters.AddWithValue("@MODE", MODE);
        //            int i = cmd.ExecuteNonQuery();

        //            if (i > 0)
        //                boolStatus = true;

        //        }
        //        con.Close();

        //    }
        //    catch (Exception ex)
        //    {


        //    }
        //    return boolStatus;
        //}


        //Kranthi 09162024
        public static bool INSERTUPDATECTZIPS(string strConnection, string ID, string DBName, string CTZ_XML_FILE, string FileType, string CTZ_COMP_OPERATOR, string MODE)
        {
            bool boolStatus = false;
            try
            {
                SqlConnection con = new SqlConnection(strConnection);
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "INSERTUPDATECTZIPS";
                    cmd.CommandTimeout = 200;
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (ID != string.Empty)
                        cmd.Parameters.AddWithValue("@ID", ID);

                    if (DBName != string.Empty)
                        cmd.Parameters.AddWithValue("@CTZ_AGENCY", DBName);

                    if (CTZ_XML_FILE != string.Empty)
                        cmd.Parameters.AddWithValue("@CTZ_XML_FILE", CTZ_XML_FILE);

                    if (FileType != string.Empty)
                        cmd.Parameters.AddWithValue("@CTZ_FILE_TYPE", FileType);

                    if (CTZ_COMP_OPERATOR != string.Empty)
                        cmd.Parameters.AddWithValue("@CTZ_COMP_OPERATOR", CTZ_COMP_OPERATOR);
                    if (MODE != string.Empty)
                        cmd.Parameters.AddWithValue("@MODE", MODE);
                    int i = cmd.ExecuteNonQuery();

                    if (i > 0)
                        boolStatus = true;

                }
                con.Close();

            }
            catch (Exception ex)
            {


            }
            return boolStatus;
        }

        public static bool UpdateNoXMLStatus(string strConnection, string ID, string CTZ_AGENCY, string CTZ_APPID, string CTZ_COMP_OPERATOR, string MODE)
        {
            bool boolStatus = false;
            try
            {
                SqlConnection con = new SqlConnection(strConnection);
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "INSERTUPDATECTZIPS";
                    cmd.CommandTimeout = 200;
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (ID != string.Empty)
                        cmd.Parameters.AddWithValue("@ID", ID);

                    if (CTZ_AGENCY != string.Empty)
                        cmd.Parameters.AddWithValue("@CTZ_AGENCY", CTZ_AGENCY);

                    if (CTZ_APPID != string.Empty)
                        cmd.Parameters.AddWithValue("@CTZ_APPID", CTZ_APPID);

                    if (CTZ_COMP_OPERATOR != string.Empty)
                        cmd.Parameters.AddWithValue("@CTZ_COMP_OPERATOR", CTZ_COMP_OPERATOR);
                    if (MODE != string.Empty)
                        cmd.Parameters.AddWithValue("@Mode", MODE);
                    int i = cmd.ExecuteNonQuery();

                    if (i > 0)
                        boolStatus = true;

                }
                con.Close();

            }
            catch (Exception ex)
            {


            }
            return boolStatus;
        }

        public static string UPDATE_VERIFY_FIELDS(string strConnection, string APPID, string MEMID, string MODE, string VERIFYMODE, string VERIFYUSER)
        {
            string strOutResult = string.Empty;
            try
            {
                SqlConnection con = new SqlConnection(strConnection);
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "INSUPDEL_CAPS_TMS00141";
                    cmd.CommandTimeout = 200;
                    cmd.CommandType = CommandType.StoredProcedure;

                    

                    if (APPID != string.Empty)
                        cmd.Parameters.AddWithValue("@APPID", APPID);

                    if (MEMID != string.Empty)
                        cmd.Parameters.AddWithValue("@MEMID", MEMID);


                    if (MODE != string.Empty)
                        cmd.Parameters.AddWithValue("@MODE", MODE);

                    if (VERIFYMODE != string.Empty)
                        cmd.Parameters.AddWithValue("@VERIFYMODE", VERIFYMODE);

                    if (VERIFYUSER != string.Empty)
                        cmd.Parameters.AddWithValue("@VERIFYUSER", VERIFYUSER);

                    SqlParameter sqlmsg = new SqlParameter("@strRESULT", SqlDbType.VarChar, 150);
                    sqlmsg.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(sqlmsg);


                    int i = cmd.ExecuteNonQuery();

                    if (i > 0)
                    {
                        strOutResult = sqlmsg.Value.ToString();
                    }
                }
                con.Close();

            }
            catch (Exception ex)
            {
            }
            return strOutResult;
        }

        public static string INSUPDEL_DSSXML_CAPTAIN(string strConnection, string strAgy, string strDept, string strProg, string strYear, string APPID, string MEMID, DataTable XML_MST_TYPE, DataTable XML_SNP_TYPE,
                                                   DataTable XML_INCOME_TYPE, DataTable XML_DIFF_TYPE, DataTable XML_LLR_TYPE, DataTable XML_LPV_TYPE, DataTable XML_LPW_TYPE, string MODE)
        {
            string strOutResult = string.Empty;
            try
            {
                SqlConnection con = new SqlConnection(strConnection);
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "INSUPDEL_CAPS_TMS00141";
                    cmd.CommandTimeout = 200;
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (strAgy != string.Empty)
                        cmd.Parameters.AddWithValue("@AGY", strAgy);

                    if (strDept != string.Empty)
                        cmd.Parameters.AddWithValue("@DEPT", strDept);

                    if (strProg != string.Empty)
                        cmd.Parameters.AddWithValue("@PROG", strProg);

                    if (strYear != string.Empty)
                        cmd.Parameters.AddWithValue("@YEAR", strYear);

                    if (APPID != string.Empty)
                        cmd.Parameters.AddWithValue("@APPID", APPID);

                    if (MEMID != string.Empty)
                        cmd.Parameters.AddWithValue("@MEMID", MEMID);

                    if (XML_MST_TYPE != null)
                        cmd.Parameters.AddWithValue("@XML_MST_TYPE", XML_MST_TYPE);

                    if (XML_SNP_TYPE != null)
                        cmd.Parameters.AddWithValue("@XML_SNP_TYPE", XML_SNP_TYPE);

                    if (XML_INCOME_TYPE != null)
                        cmd.Parameters.AddWithValue("@XML_INCOME_TYPE", XML_INCOME_TYPE);

                    if (XML_DIFF_TYPE != null)
                        cmd.Parameters.AddWithValue("@XML_DIFF_TYPE", XML_DIFF_TYPE);

                    if (XML_LLR_TYPE != null)
                        cmd.Parameters.AddWithValue("@XML_LLR_TYPE", XML_LLR_TYPE);

                    if (XML_LPV_TYPE != null)
                        cmd.Parameters.AddWithValue("@XML_LPV_TYPE", XML_LPV_TYPE);

                    if (XML_LPW_TYPE != null)
                        cmd.Parameters.AddWithValue("@XML_LPW_TYPE", XML_LPW_TYPE);

                    if (MODE != string.Empty)
                        cmd.Parameters.AddWithValue("@MODE", MODE);

                    SqlParameter sqlmsg = new SqlParameter("@strRESULT", SqlDbType.VarChar, 150);
                    sqlmsg.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(sqlmsg);


                    int i = cmd.ExecuteNonQuery();

                    if (i > 0)
                    {
                        strOutResult = sqlmsg.Value.ToString();
                    }
                }
                con.Close();

            }
            catch (Exception ex)
            {
            }
            return strOutResult;
        }


        public static bool InsupDelMIDTable(string strConnection, string CTZ_ID, string DXM_APPID, string DXM_MEMID, string DXM_FNAME, string DXM_LNAME, string DXM_DOB, string DXM_SSN, string DXM_Relation, string DXM_CatElig, string DXM_AsstType, string DXM_Supplier, string DXM_IMGUPLD,
    string DXM_CAP_AGENCY, string DXM_CAP_DEPT, string DXM_CAP_PROG, string DXM_CAP_YEAR, string DXM_CAP_APPNO, string DXM_CAP_FAMSEQ, string LSTCOperator, string mode, string CaseWorker, string ApplType, string AGENCYNAME, string DXMBurden,
            string IsMinor, string ProgYear, string PNotify, string Version)
        {
            bool boolStatus = false;
            try
            {
                SqlConnection con = new SqlConnection(strConnection);
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "INSERTUPDATE_DSSXMLMID";
                    cmd.CommandTimeout = 200;
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (CTZ_ID != string.Empty)
                        cmd.Parameters.AddWithValue("@CTZ_ID", CTZ_ID);

                    if (DXM_APPID != string.Empty)
                        cmd.Parameters.AddWithValue("@DXM_APPID", DXM_APPID);

                    if (DXM_MEMID != string.Empty)
                        cmd.Parameters.AddWithValue("@DXM_MEMID", DXM_MEMID);

                    if (DXM_FNAME != string.Empty)
                        cmd.Parameters.AddWithValue("@DXM_FNAME", DXM_FNAME);

                    if (DXM_LNAME != string.Empty)
                        cmd.Parameters.AddWithValue("@DXM_LNAME", DXM_LNAME);

                    if (DXM_DOB != string.Empty)
                        cmd.Parameters.AddWithValue("@DXM_DOB", DXM_DOB);

                    if (DXM_SSN != string.Empty)
                        cmd.Parameters.AddWithValue("@DXM_SSN", DXM_SSN);

                    if (DXM_Relation != string.Empty)
                        cmd.Parameters.AddWithValue("@DXM_RELATION", DXM_Relation);

                    if (DXM_CatElig != string.Empty)
                        cmd.Parameters.AddWithValue("@DXM_CATELIG", DXM_CatElig);

                    if (DXM_AsstType != string.Empty)
                        cmd.Parameters.AddWithValue("@DXM_ASSTYPE", DXM_AsstType);

                    if (DXM_Supplier != string.Empty)
                        cmd.Parameters.AddWithValue("@DXM_SUPPLIER", DXM_Supplier);

                    if (DXM_IMGUPLD != string.Empty)
                        cmd.Parameters.AddWithValue("@DXM_IMGUPLD", DXM_IMGUPLD);

                    if (DXM_CAP_AGENCY != string.Empty)
                        cmd.Parameters.AddWithValue("@DXM_CAP_AGENCY", DXM_CAP_AGENCY);

                    if (DXM_CAP_DEPT != string.Empty)
                        cmd.Parameters.AddWithValue("@DXM_CAP_DEPT", DXM_CAP_DEPT);

                    if (DXM_CAP_PROG != string.Empty)
                        cmd.Parameters.AddWithValue("@DXM_CAP_PROG", DXM_CAP_PROG);

                    if (DXM_CAP_YEAR != string.Empty)
                        cmd.Parameters.AddWithValue("@DXM_CAP_YEAR", DXM_CAP_YEAR);

                    if (DXM_CAP_APPNO != string.Empty)
                        cmd.Parameters.AddWithValue("@DXM_CAP_APPNO", DXM_CAP_APPNO);

                    if (DXM_CAP_FAMSEQ != string.Empty)
                        cmd.Parameters.AddWithValue("@DXM_CAP_FAMSEQ", DXM_CAP_FAMSEQ);

                    if (LSTCOperator != string.Empty)
                        cmd.Parameters.AddWithValue("@DXM_LSTC_USER", LSTCOperator);

                    if (CaseWorker != string.Empty)
                        cmd.Parameters.AddWithValue("@CASEWORKER", CaseWorker);

                    if (ApplType != string.Empty)
                        cmd.Parameters.AddWithValue("@APPLTYPE", ApplType);

                    if (AGENCYNAME != string.Empty)
                        cmd.Parameters.AddWithValue("@AGYName", AGENCYNAME);

                    if (DXMBurden != string.Empty)
                        cmd.Parameters.AddWithValue("@DXM_BURDEN", DXMBurden);

                    if (IsMinor != string.Empty)
                        cmd.Parameters.AddWithValue("@DXM_ISMINOR", IsMinor);

                    if (ProgYear != string.Empty)
                        cmd.Parameters.AddWithValue("@DXM_PROGYEAR", ProgYear);

                    if (PNotify != string.Empty)
                        cmd.Parameters.AddWithValue("@DXM_PNOTIFY", PNotify);

                    if (Version != string.Empty)
                        cmd.Parameters.AddWithValue("@DXM_VERSION", Version);

                    if (mode != string.Empty)
                        cmd.Parameters.AddWithValue("@mode", mode);
                    int i = cmd.ExecuteNonQuery();

                    if (i > 0)
                        boolStatus = true;

                }
                con.Close();

            }
            catch (Exception ex)
            {


            }
            return boolStatus;
        }

        public static bool insupDelDXMLDocs(string strConnection, string DSSXML_DOC_APP_ID
           , string DSSXML_DOC_SECURITY
           , string DSSXML_DOC_TYPE
           , string DSSXML_DOC_OG_NAME
           , string DSSXML_DOC_FILE_DATE, string DSSXML_DOC_XML_TYPE, string DSSXML_DOC_NAME, string DSSXML_DOC_ZIP_ID, string DSSXML_DOC_SAVE_SW
           , string MODE)
        {
            bool boolStatus = false;
            try
            {
                SqlConnection con = new SqlConnection(strConnection);
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "DSSXMLDOCS_INSUPDEL";
                    cmd.CommandTimeout = 200;
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (DSSXML_DOC_APP_ID != string.Empty)
                        cmd.Parameters.AddWithValue("@DSSXML_DOC_APP_ID", DSSXML_DOC_APP_ID);

                    if (DSSXML_DOC_SECURITY != string.Empty)
                        cmd.Parameters.AddWithValue("@DSSXML_DOC_SECURITY", DSSXML_DOC_SECURITY);

                    if (DSSXML_DOC_TYPE != string.Empty)
                        cmd.Parameters.AddWithValue("@DSSXML_DOC_TYPE", DSSXML_DOC_TYPE);

                    if (DSSXML_DOC_OG_NAME != string.Empty)
                        cmd.Parameters.AddWithValue("@DSSXML_DOC_OG_NAME", DSSXML_DOC_OG_NAME);

                    if (DSSXML_DOC_FILE_DATE != string.Empty)
                        cmd.Parameters.AddWithValue("@DSSXML_DOC_FILE_DATE", DSSXML_DOC_FILE_DATE);

                    if (DSSXML_DOC_NAME != string.Empty)
                        cmd.Parameters.AddWithValue("@DSSXML_DOC_NAME", DSSXML_DOC_NAME);
                    if (DSSXML_DOC_XML_TYPE != string.Empty)
                        cmd.Parameters.AddWithValue("@DSSXML_DOC_XML_TYPE", DSSXML_DOC_XML_TYPE);

                    if (DSSXML_DOC_ZIP_ID != string.Empty)
                        cmd.Parameters.AddWithValue("@DSSXML_DOC_ZIP_ID", DSSXML_DOC_ZIP_ID);

                    //if (DSSXML_DOC_ZIP_ID != string.Empty)
                    //    cmd.Parameters.AddWithValue("@DSSXML_DOC_ZIP_ID", DSSXML_DOC_ZIP_ID);

                    if (DSSXML_DOC_SAVE_SW != string.Empty)
                        cmd.Parameters.AddWithValue("@DSSXML_DOC_SAVE_SW", DSSXML_DOC_SAVE_SW);

                    if (MODE != string.Empty)
                        cmd.Parameters.AddWithValue("@MODE", MODE);
                    int i = cmd.ExecuteNonQuery();

                    if (i > 0)
                        boolStatus = true;

                }
                con.Close();

            }
            catch (Exception ex)
            {


            }
            return boolStatus;
        }

        public static string INSUPDEL_DSSXML_LIHEAPV(string strConnection, string APPID, string VendCode, string strseq, string strPrimCode, string AcctNo, string payfor, string Fname, string LName, string BillType, string VendName,
                                                  string AddOperator, string lstcOperator, string MODE, string strbypass)
        {
            string strOutResult = string.Empty;
            try
            {
                SqlConnection con = new SqlConnection(strConnection);
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "CAP_DSSLIHEAPV_INSUPDEL";
                    cmd.CommandTimeout = 200;
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (APPID != string.Empty)
                        cmd.Parameters.AddWithValue("@DSSLPV_APPID", APPID);

                    if (VendCode != string.Empty)
                        cmd.Parameters.AddWithValue("@DSSLPV_VEND_CODE", VendCode);

                    if (strseq != string.Empty)
                        cmd.Parameters.AddWithValue("@DSSLPV_SEQ", strseq);

                    if (strPrimCode != string.Empty)
                        cmd.Parameters.AddWithValue("@DSSLPV_PRIM_CODE", strPrimCode);

                    if (AcctNo != string.Empty)
                        cmd.Parameters.AddWithValue("@DSSLPV_ACCT_NO", AcctNo);

                    if (payfor != string.Empty)
                        cmd.Parameters.AddWithValue("@DSSLPV_PAYMENT_FOR", payfor);

                    if (Fname != string.Empty)
                        cmd.Parameters.AddWithValue("@DSSLPV_BILL_FNAME", Fname);

                    if (LName != string.Empty)
                        cmd.Parameters.AddWithValue("@DSSLPV_BILL_LNAME", LName);

                    if (VendName != string.Empty)
                        cmd.Parameters.AddWithValue("@DSSLPV_VEND_NAME", VendName);

                    if (BillType != string.Empty)
                        cmd.Parameters.AddWithValue("@DSSLPV_BILLNAME_TYPE", BillType);

                    if (AddOperator != string.Empty)
                        cmd.Parameters.AddWithValue("@DSSLPV_ADD_OPERATOR", AddOperator);

                    if (lstcOperator != string.Empty)
                        cmd.Parameters.AddWithValue("@DSSLPV_LSTC_OPERATOR", lstcOperator);

                    if (strbypass != string.Empty)
                        cmd.Parameters.AddWithValue("@DSSLPV_BYPASS", strbypass);

                    if (MODE != string.Empty)
                        cmd.Parameters.AddWithValue("@MODE", MODE);

                    SqlParameter sqlmsg = new SqlParameter("@strRESULT", SqlDbType.VarChar, 150);
                    sqlmsg.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(sqlmsg);


                    int i = cmd.ExecuteNonQuery();

                    if (i > 0)
                    {
                        strOutResult = sqlmsg.Value.ToString();
                    }
                }
                con.Close();

            }
            catch (Exception ex)
            {
            }
            return strOutResult;
        }

        public static bool insupDelDSSREVFEED(string strConnection, string strAgency, string strDept, string strProg, string strYear, string strApp, string strDate, string ShortName, string LetterType, string AppId, string MemID,
           string Fname, string Lname, string FileDate, string strStatus, string strStatusDate, string Language, string CertType, string Benefit, string strSource, string Vendor, string VendAcct, string FileName, string strExt, string Base64,
           string printUser, string PrintDate, string SentDate, string UniqueClientID,string DReason,string Intk_Det, string MODE)
        {
            bool boolStatus = false;
            try
            {
                SqlConnection con = new SqlConnection(strConnection);
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "CAPS_REVFEED_INSUPDEL";
                    cmd.CommandTimeout = 200;
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (strAgency != string.Empty)
                        cmd.Parameters.AddWithValue("@DRF_AGENCY", strAgency);

                    if (strDept != string.Empty)
                        cmd.Parameters.AddWithValue("@DRF_DEPT", strDept);

                    if (strProg != string.Empty)
                        cmd.Parameters.AddWithValue("@DRF_PROGRAM", strProg);

                    if (strYear != string.Empty)
                        cmd.Parameters.AddWithValue("@DRF_YEAR", strYear);

                    if (strApp != string.Empty)
                        cmd.Parameters.AddWithValue("@DRF_APPNO", strApp);

                    if (strDate != string.Empty)
                        cmd.Parameters.AddWithValue("@DRF_DATE", strDate);
                    if (ShortName != string.Empty)
                        cmd.Parameters.AddWithValue("@DRF_SHORTNAME", ShortName);

                    if (LetterType != string.Empty)
                        cmd.Parameters.AddWithValue("@DRF_LETT_TYPE", LetterType);

                    if (AppId != string.Empty)
                        cmd.Parameters.AddWithValue("@DRF_APPID", AppId);

                    if (MemID != string.Empty)
                        cmd.Parameters.AddWithValue("@DRF_MEMID", MemID);

                    if (Fname != string.Empty)
                        cmd.Parameters.AddWithValue("@DRF_FNAME", Fname);

                    if (Lname != string.Empty)
                        cmd.Parameters.AddWithValue("@DRF_LNAME", Lname);

                    if (FileDate != string.Empty)
                        cmd.Parameters.AddWithValue("@DRF_FILE_DATE", FileDate);

                    if (strStatus != string.Empty)
                        cmd.Parameters.AddWithValue("@DRF_STATUS", strStatus);

                    if (strStatusDate != string.Empty)
                        cmd.Parameters.AddWithValue("@DRF_STATUS_DATE", strStatusDate);

                    if (Language != string.Empty)
                        cmd.Parameters.AddWithValue("@DRF_LANGUAGE", Language);

                    if (CertType != string.Empty)
                        cmd.Parameters.AddWithValue("@DRF_CERT_TYPE", CertType);
                    if (Benefit != string.Empty)
                        cmd.Parameters.AddWithValue("@DRF_BENEFIT", Benefit);

                    if (strSource != string.Empty)
                        cmd.Parameters.AddWithValue("@DRF_SOURCE", strSource);

                    if (Vendor != string.Empty)
                        cmd.Parameters.AddWithValue("@DRF_VENDOR", Vendor);

                    if (VendAcct != string.Empty)
                        cmd.Parameters.AddWithValue("@DRF_VEND_ACCT", VendAcct);

                    if (FileName != string.Empty)
                        cmd.Parameters.AddWithValue("@DRF_FILENAME", FileName);

                    if (strExt != string.Empty)
                        cmd.Parameters.AddWithValue("@DRF_FILE_EXT", strExt);
                    if (Base64 != string.Empty)
                        cmd.Parameters.AddWithValue("@DRF_BASE64", Base64);

                    if (printUser != string.Empty)
                        cmd.Parameters.AddWithValue("@DRF_PRINT_USER", printUser);

                    if (PrintDate != string.Empty)
                        cmd.Parameters.AddWithValue("@DRF_PRINT_DATE", PrintDate);

                    if (SentDate != string.Empty)
                        cmd.Parameters.AddWithValue("@DRF_SENT_DATE", SentDate);

                    if (UniqueClientID != string.Empty)
                        cmd.Parameters.AddWithValue("@DRF_UNIQUE_CLIENT_ID", UniqueClientID);

                    if (DReason != string.Empty)
                        cmd.Parameters.AddWithValue("@DRF_DENIED_REASON", DReason);

                    if (Intk_Det != string.Empty)
                        cmd.Parameters.AddWithValue("@DRF_INTK_REASON", Intk_Det);

                    if (MODE != string.Empty)
                        cmd.Parameters.AddWithValue("@Mode", MODE);
                    int i = cmd.ExecuteNonQuery();

                    if (i > 0)
                        boolStatus = true;

                }
                con.Close();

            }
            catch (Exception ex)
            {


            }
            return boolStatus;
        }


        public static bool INSDELDSSCNTPROC(string strConnection, string DCNP_APPID, string DCNP_AGENCY, string DCNP_REASON, string DCNP_WRKR, string DCNP_MOV_DATE, string MODE)
        {
            bool boolStatus = false;
            try
            {
                SqlConnection con = new SqlConnection(strConnection);
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "INSDELGETDSSCNTPROC";
                    cmd.CommandTimeout = 200;
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (DCNP_APPID != string.Empty)
                        cmd.Parameters.AddWithValue("@DCNP_APPID", DCNP_APPID);
                    if (DCNP_AGENCY != string.Empty)
                        cmd.Parameters.AddWithValue("@DCNP_AGENCY", DCNP_AGENCY);

                    if (DCNP_REASON != string.Empty)
                        cmd.Parameters.AddWithValue("@DCNP_REASON", DCNP_REASON);

                    if (DCNP_WRKR != string.Empty)
                        cmd.Parameters.AddWithValue("@DCNP_WRKR", DCNP_WRKR);

                    if (DCNP_MOV_DATE != string.Empty)
                        cmd.Parameters.AddWithValue("@DCNP_MOV_DATE", DCNP_MOV_DATE);

                    if (MODE != string.Empty)
                        cmd.Parameters.AddWithValue("@MODE", MODE);
                    int i = cmd.ExecuteNonQuery();

                    if (i > 0)
                        boolStatus = true;

                }
                con.Close();

            }
            catch (Exception ex)
            {


            }
            return boolStatus;
        }

        public static DataTable DSSXMLLIHEAPV_GET(string strConnection, string ApplicantID)
        {

            DataTable dt = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(strConnection);
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "CAP_DSSLIHEAPV_GET";
                    cmd.CommandTimeout = 200;
                    cmd.CommandType = CommandType.StoredProcedure;


                    if (ApplicantID != string.Empty)
                        cmd.Parameters.AddWithValue("@DSSLPV_APPID", ApplicantID);
                    //if (frmDate != string.Empty)
                    //    cmd.Parameters.AddWithValue("@FROMDATE", frmDate);
                    //if (toDate != string.Empty)
                    //    cmd.Parameters.AddWithValue("@TODATE", toDate);
                    //if (Agency != string.Empty)
                    //    cmd.Parameters.AddWithValue("@Agency", Agency);
                    //if (Mode != string.Empty)
                    //    cmd.Parameters.AddWithValue("@MODE", Mode);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    da.Fill(dt);


                }
                con.Close();

            }
            catch (Exception ex)
            {


            }
            return dt;

        }

        //Added by Vikash on 09/20/2023 for Wait Room Status Report
        public static DataTable GET_WAITROOMSTATUS(string strConnection, string Agency, string FromDate, string ToDate, string Status)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(strConnection);
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "GET_WAITROOMSTATUS";
                    cmd.CommandTimeout = 200;
                    cmd.CommandType = CommandType.StoredProcedure;


                    if (Agency != string.Empty)
                        cmd.Parameters.AddWithValue("@Agency", Agency);

                    if (FromDate != string.Empty)
                        cmd.Parameters.AddWithValue("@FromDate", FromDate);

                    if (ToDate != string.Empty)
                        cmd.Parameters.AddWithValue("@ToDate", ToDate);

                    if (Status != string.Empty)
                        cmd.Parameters.AddWithValue("@Status", Status);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    da.Fill(dt);


                }
                con.Close();

            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        public static DataTable XML_MST_TYPE()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("XML_MST_AGENCY", typeof(string));
            dt.Columns.Add("XML_MST_DEPT", typeof(string));
            dt.Columns.Add("XML_MST_PROGRAM", typeof(string));
            dt.Columns.Add("XML_MST_YEAR", typeof(string));
            dt.Columns.Add("XML_MST_APP_NO", typeof(string));
            dt.Columns.Add("XML_MST_FAMILY_SEQ", typeof(decimal));
            dt.Columns.Add("XML_MST_FAMILY_ID", typeof(string));
            dt.Columns.Add("XML_MST_CLIENT_ID", typeof(decimal));
            dt.Columns.Add("XML_MST_SSN", typeof(string));
            dt.Columns.Add("XML_MST_BIC", typeof(string));
            dt.Columns.Add("XML_MST_NICKNAME", typeof(string));
            dt.Columns.Add("XML_MST_ETHNIC_OTHER", typeof(string));
            dt.Columns.Add("XML_MST_STATE", typeof(string));
            dt.Columns.Add("XML_MST_CITY", typeof(string));
            dt.Columns.Add("XML_MST_STREET", typeof(string));
            dt.Columns.Add("XML_MST_SUFFIX", typeof(string));
            dt.Columns.Add("XML_MST_HN", typeof(string));
            dt.Columns.Add("XML_MST_DIRECTION", typeof(string));
            dt.Columns.Add("XML_MST_APT", typeof(string));
            dt.Columns.Add("XML_MST_FLR", typeof(string));
            dt.Columns.Add("XML_MST_ZIP", typeof(decimal));
            dt.Columns.Add("XML_MST_ZIPPLUS", typeof(decimal));
            dt.Columns.Add("XML_MST_PRECINCT", typeof(string));
            dt.Columns.Add("XML_MST_AREA", typeof(string));
            dt.Columns.Add("XML_MST_PHONE", typeof(string));
            dt.Columns.Add("XML_MST_NEXTYEAR", typeof(string));
            dt.Columns.Add("XML_MST_CLASSIFICATION", typeof(string));
            dt.Columns.Add("XML_MST_LANGUAGE", typeof(string));
            dt.Columns.Add("XML_MST_LANGUAGE_OT", typeof(string));
            dt.Columns.Add("XML_MST_INTAKE_WORKER", typeof(string));
            dt.Columns.Add("XML_MST_INTAKE_DATE", typeof(string));
            dt.Columns.Add("XML_MST_INITIAL_DATE", typeof(string));
            dt.Columns.Add("XML_MST_CASE_TYPE", typeof(string));
            dt.Columns.Add("XML_MST_HOUSING", typeof(string));
            dt.Columns.Add("XML_MST_FAMILY_TYPE", typeof(string));
            dt.Columns.Add("XML_MST_SITE", typeof(string));
            dt.Columns.Add("XML_MST_JUVENILE", typeof(string));
            dt.Columns.Add("XML_MST_SENIOR", typeof(string));
            dt.Columns.Add("XML_MST_SECRET", typeof(string));
            dt.Columns.Add("XML_MST_CASE_REVIEW_DATE", typeof(string));
            dt.Columns.Add("XML_MST_ALERT_CODES", typeof(string));
            dt.Columns.Add("XML_MST_PARENT_STATUS", typeof(string));
            dt.Columns.Add("XML_MST_INTAKE_HRS", typeof(string));
            dt.Columns.Add("XML_MST_INTAKE_MNS", typeof(string));
            dt.Columns.Add("XML_MST_INTAKE_SCS", typeof(string));
            dt.Columns.Add("XML_MST_FIN_HRS", typeof(decimal));
            dt.Columns.Add("XML_MST_FIN_MNS", typeof(decimal));
            dt.Columns.Add("XML_MST_FIN_SCS", typeof(decimal));
            dt.Columns.Add("XML_MST_SIM_HRS", typeof(decimal));
            dt.Columns.Add("XML_MST_SIM_MNS", typeof(decimal));
            dt.Columns.Add("XML_MST_SIM_SCS", typeof(decimal));
            dt.Columns.Add("XML_MST_MED_HRS", typeof(decimal));
            dt.Columns.Add("XML_MST_MED_MNS", typeof(decimal));
            dt.Columns.Add("XML_MST_MED_SCS", typeof(decimal));
            dt.Columns.Add("XML_MST_RANK1", typeof(decimal));
            dt.Columns.Add("XML_MST_RANK2", typeof(decimal));
            dt.Columns.Add("XML_MST_RANK3", typeof(decimal));
            dt.Columns.Add("XML_MST_RANK4", typeof(decimal));
            dt.Columns.Add("XML_MST_RANK5", typeof(decimal));
            dt.Columns.Add("XML_MST_RANK6", typeof(decimal));
            dt.Columns.Add("XML_MST_POSITION1", typeof(string));
            dt.Columns.Add("XML_MST_POSITION2", typeof(string));
            dt.Columns.Add("XML_MST_POSITION3", typeof(string));
            dt.Columns.Add("XML_MST_TOWNSHIP", typeof(string));
            dt.Columns.Add("XML_MST_INTAKE_TIME1", typeof(string));
            dt.Columns.Add("XML_MST_SSN_FLAG", typeof(string));
            dt.Columns.Add("XML_MST_STATE_CASE", typeof(string));
            dt.Columns.Add("XML_MST_VERIFIER", typeof(string));
            dt.Columns.Add("XML_MST_ELIG_DATE", typeof(string));
            dt.Columns.Add("XML_MST_CAT_ELIG", typeof(string));
            dt.Columns.Add("XML_MST_MEAL_ELIG", typeof(string));
            dt.Columns.Add("XML_MST_VERIFY_W2", typeof(string));
            dt.Columns.Add("XML_MST_VERIFY_CHECK_STUB", typeof(string));
            dt.Columns.Add("XML_MST_VERIFY_TAX_RETURN", typeof(string));
            dt.Columns.Add("XML_MST_VERIFY_LETTER", typeof(string));
            dt.Columns.Add("XML_MST_VERIFY_OTHER", typeof(string));
            dt.Columns.Add("XML_MST_REVERIFY_DATE", typeof(string));
            dt.Columns.Add("XML_MST_INCOME_TYPES", typeof(string));
            dt.Columns.Add("XML_MST_POVERTY", typeof(decimal));
            dt.Columns.Add("XML_MST_WAIT_LIST", typeof(string));
            dt.Columns.Add("XML_MST_ACTIVE_STATUS", typeof(string));
            dt.Columns.Add("XML_MST_TOTAL_RANK", typeof(decimal));
            dt.Columns.Add("XML_MST_NO_INHH", typeof(decimal));
            dt.Columns.Add("XML_MST_FAM_INCOME", typeof(decimal));
            dt.Columns.Add("XML_MST_NO_INPROG", typeof(decimal));
            dt.Columns.Add("XML_MST_PROG_INCOME", typeof(decimal));
            dt.Columns.Add("XML_MST_OUT_OF_SERVICE", typeof(string));
            dt.Columns.Add("XML_MST_HUD", typeof(decimal));
            dt.Columns.Add("XML_MST_SMI", typeof(decimal));
            dt.Columns.Add("XML_MST_CMI", typeof(decimal));
            dt.Columns.Add("XML_MST_COUNTY", typeof(string));
            dt.Columns.Add("XML_MST_ADDRESS_YEARS", typeof(decimal));
            dt.Columns.Add("XML_MST_MESSAGE_PHONE", typeof(string));
            dt.Columns.Add("XML_MST_CELL_PHONE", typeof(string));
            dt.Columns.Add("XML_MST_FAX_NUMBER", typeof(string));
            dt.Columns.Add("XML_MST_TTY_NUMBER", typeof(string));
            dt.Columns.Add("XML_MST_EMAIL", typeof(string));
            dt.Columns.Add("XML_MST_BEST_CONTACT", typeof(string));
            dt.Columns.Add("XML_MST_ABOUT_US", typeof(string));
            dt.Columns.Add("XML_MST_IMPORT_DATE", typeof(string));
            dt.Columns.Add("XML_MST_DATE_ADDED", typeof(string));
            dt.Columns.Add("XML_MST_EXP_CASEWORKER", typeof(string));
            dt.Columns.Add("XML_MST_EXP_RENT", typeof(decimal));
            dt.Columns.Add("XML_MST_EXP_WATER", typeof(decimal));
            dt.Columns.Add("XML_MST_EXP_ELECTRIC", typeof(decimal));
            dt.Columns.Add("XML_MST_EXP_HEAT", typeof(decimal));
            dt.Columns.Add("XML_MST_EXP_MISC", typeof(decimal));
            dt.Columns.Add("XML_MST_EXP_TOTAL", typeof(decimal));
            dt.Columns.Add("XML_MST_EXP_LIVEXPENSE", typeof(decimal));
            dt.Columns.Add("XML_MST_DEBT_CC", typeof(decimal));
            dt.Columns.Add("XML_MST_DEBT_LOANS", typeof(decimal));
            dt.Columns.Add("XML_MST_DEBT_MED", typeof(decimal));
            dt.Columns.Add("XML_MST_DEBT_OTH", typeof(decimal));
            dt.Columns.Add("XML_MST_DEBT_MISC", typeof(decimal));
            dt.Columns.Add("XML_MST_DEBT_TOTAL", typeof(decimal));
            dt.Columns.Add("XML_MST_ASET_PHY", typeof(decimal));
            dt.Columns.Add("XML_MST_ASET_LIQ", typeof(decimal));
            dt.Columns.Add("XML_MST_ASET_OTH", typeof(decimal));
            dt.Columns.Add("XML_MST_ASET_TOTAL", typeof(decimal));
            dt.Columns.Add("XML_MST_ASET_MISC", typeof(decimal));
            dt.Columns.Add("XML_MST_DEB_ASET_RATIO", typeof(decimal));
            dt.Columns.Add("XML_MST_DEB_INCM_RATIO", typeof(decimal));
            dt.Columns.Add("XML_MST_DWELLING", typeof(string));
            dt.Columns.Add("XML_MST_HEAT_INC_RENT", typeof(string));
            dt.Columns.Add("XML_MST_SOURCE", typeof(string));
            dt.Columns.Add("XML_MST_ROLLOVER", typeof(string));
            dt.Columns.Add("XML_MST_RISK_VALUE", typeof(decimal));
            dt.Columns.Add("XML_MST_SUBSHOUSE", typeof(string));
            dt.Columns.Add("XML_MST_SUBSTYPE", typeof(string));
            dt.Columns.Add("XML_MST_VER_FUND", typeof(string));
            dt.Columns.Add("XML_MST_OMB_SCREEN", typeof(string));
            dt.Columns.Add("XML_MST_CB_CASE_MANAGER", typeof(decimal));
            dt.Columns.Add("XML_MST_CASE_MANAGER", typeof(string));
            dt.Columns.Add("XML_MST_VERIFY_OTH_CMB", typeof(string));
            dt.Columns.Add("XML_MST_SIM_PRINT", typeof(string));
            dt.Columns.Add("XML_MST_SIM_PRINT_DATE", typeof(string));
            dt.Columns.Add("XML_MST_CB_FRAUD", typeof(decimal));
            dt.Columns.Add("XML_MST_FRAUD_DATE", typeof(string));
            dt.Columns.Add("XML_MST_PRESS_JOB", typeof(string));
            dt.Columns.Add("XML_MST_PRESS_HSD", typeof(string));
            dt.Columns.Add("XML_MST_PRESS_RW_ENG", typeof(string));
            dt.Columns.Add("XML_MST_PRESS_SKILLS", typeof(string));
            dt.Columns.Add("XML_MST_PRESS_HOUSING", typeof(string));
            dt.Columns.Add("XML_MST_PRESS_TRANSPORT", typeof(string));
            dt.Columns.Add("XML_MST_PRESS_CHLDCARE", typeof(string));
            dt.Columns.Add("XML_MST_PRESS_CCENRL", typeof(string));
            dt.Columns.Add("XML_MST_PRESS_ELDRCARE", typeof(string));
            dt.Columns.Add("XML_MST_PRESS_ECNEED", typeof(string));
            dt.Columns.Add("XML_MST_PRESS_CHINS", typeof(string));
            dt.Columns.Add("XML_MST_PRESS_AHINS", typeof(string));
            dt.Columns.Add("XML_MST_PRESS_CURR_DSS", typeof(string));
            dt.Columns.Add("XML_MST_PRESS_RECV_DSS", typeof(string));
            dt.Columns.Add("XML_MST_LPM_0001", typeof(string));
            dt.Columns.Add("XML_MST_LPM_0002", typeof(string));
            dt.Columns.Add("XML_MST_LPM_0003", typeof(string));
            dt.Columns.Add("XML_MST_LPM_0004", typeof(string));
            dt.Columns.Add("XML_MST_LPM_0005", typeof(string));
            dt.Columns.Add("XML_MST_LPM_0006", typeof(string));
            dt.Columns.Add("XML_MST_LPM_0007", typeof(string));
            dt.Columns.Add("XML_MST_LPM_0008", typeof(string));
            dt.Columns.Add("XML_MST_LPM_0009", typeof(string));
            dt.Columns.Add("XML_MST_LPM_0010", typeof(string));
            dt.Columns.Add("XML_MST_LPM_0011", typeof(string));
            dt.Columns.Add("XML_MST_DATE_ADD_1", typeof(string));
            dt.Columns.Add("XML_MST_ADD_OPERATOR_1", typeof(string));
            dt.Columns.Add("XML_MST_DATE_LSTC_1", typeof(string));
            dt.Columns.Add("XML_MST_LSTC_OPERATOR_1", typeof(string));
            dt.Columns.Add("XML_MST_TIMES_UPDATED_1", typeof(decimal));
            dt.Columns.Add("XML_MST_DATE_ADD_2", typeof(string));
            dt.Columns.Add("XML_MST_ADD_OPERATOR_2", typeof(string));
            dt.Columns.Add("XML_MST_DATE_LSTC_2", typeof(string));
            dt.Columns.Add("XML_MST_LSTC_OPERATOR_2", typeof(string));
            dt.Columns.Add("XML_MST_TIMES_UPDATED_2", typeof(decimal));
            dt.Columns.Add("XML_MST_DATE_ADD_3", typeof(string));
            dt.Columns.Add("XML_MST_ADD_OPERATOR_3", typeof(string));
            dt.Columns.Add("XML_MST_DATE_LSTC_3", typeof(string));
            dt.Columns.Add("XML_MST_LSTC_OPERATOR_3", typeof(string));
            dt.Columns.Add("XML_MST_TIMES_UPDATED_3", typeof(decimal));
            dt.Columns.Add("XML_MST_DATE_ADD_4", typeof(string));
            dt.Columns.Add("XML_MST_ADD_OPERATOR_4", typeof(string));
            dt.Columns.Add("XML_MST_DATE_LSTC_4", typeof(string));
            dt.Columns.Add("XML_MST_LSTC_OPERATOR_4", typeof(string));
            dt.Columns.Add("XML_MST_TIMES_UPDATED_4", typeof(decimal));
            dt.Columns.Add("XML_MST_PRESS_TOTAL", typeof(decimal));
            dt.Columns.Add("XML_MST_PRESS_CAT", typeof(string));
            dt.Columns.Add("XML_MST_PRESS_GRP", typeof(string));
            dt.Columns.Add("XML_MST_DATE_LSTC_5", typeof(string));
            dt.Columns.Add("XML_MST_LSTC_OPERATOR_5", typeof(string));
            dt.Columns.Add("XML_MST_NCASHBEN", typeof(string));
            dt.Columns.Add("XML_MST_APPLICANT_TYPE", typeof(string));
            dt.Columns.Add("XML_MST_APPLICANT_DATE", typeof(string));
            dt.Columns.Add("XML_MST_HOME_NA", typeof(string));
            dt.Columns.Add("XML_MST_CELL_NA", typeof(string));
            dt.Columns.Add("XML_MST_MESSAGE_NA", typeof(string));
            dt.Columns.Add("XML_MST_EMAIL_NA", typeof(string));
            dt.Columns.Add("XML_MST_LPM_0012", typeof(string));
            dt.Columns.Add("XML_MST_LPM_0013", typeof(string));
            dt.Columns.Add("XML_MST_LPM_0014", typeof(string));
            dt.Columns.Add("XML_MST_LPM_0015", typeof(string));
            dt.Columns.Add("XML_MST_LPM_0016", typeof(string));
            dt.Columns.Add("XML_MST_LPM_0017", typeof(string));
            dt.Columns.Add("XML_MST_CASE_COMPLETE_DATE", typeof(string));
            dt.Columns.Add("XML_MST_VERIFY_SELF_DECL", typeof(string));


            return dt;
        }
        public static DataTable XML_SNP_TYPE()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("XML_SNP_AGENCY", typeof(string));
            dt.Columns.Add("XML_SNP_DEPT", typeof(string));
            dt.Columns.Add("XML_SNP_PROGRAM", typeof(string));
            dt.Columns.Add("XML_SNP_YEAR", typeof(string));
            dt.Columns.Add("XML_SNP_APP", typeof(string));
            dt.Columns.Add("XML_SNP_FAMILY_SEQ", typeof(decimal));
            dt.Columns.Add("XML_SNP_MEMBER_CODE", typeof(string));
            dt.Columns.Add("XML_SNP_CLIENT_ID", typeof(decimal));
            dt.Columns.Add("XML_SNP_SSNO", typeof(string));
            dt.Columns.Add("XML_SNP_SS_BIC", typeof(string));
            dt.Columns.Add("XML_SNP_NAME_IX_LAST", typeof(string));
            dt.Columns.Add("XML_SNP_NAME_IX_FI", typeof(string));
            dt.Columns.Add("XML_SNP_NAME_IX_MI", typeof(string));
            dt.Columns.Add("XML_SNP_ALT_BDATE", typeof(string));
            dt.Columns.Add("XML_SNP_ALT_LNAME", typeof(string));
            dt.Columns.Add("XML_SNP_ALT_FI", typeof(string));
            dt.Columns.Add("XML_SNP_ALIAS", typeof(string));
            dt.Columns.Add("XML_SNP_STATUS", typeof(string));
            dt.Columns.Add("XML_SNP_SEX", typeof(string));
            dt.Columns.Add("XML_SNP_AGE", typeof(decimal));
            dt.Columns.Add("XML_SNP_ETHNIC", typeof(string));
            dt.Columns.Add("XML_SNP_EDUCATION", typeof(string));
            dt.Columns.Add("XML_SNP_INCOME_BASIS", typeof(string));
            dt.Columns.Add("XML_SNP_HEALTH_INS", typeof(string));
            dt.Columns.Add("XML_SNP_VET", typeof(string));
            dt.Columns.Add("XML_SNP_DISABLE", typeof(string));
            dt.Columns.Add("XML_SNP_FOOD_STAMPS", typeof(string));
            dt.Columns.Add("XML_SNP_FARMER", typeof(string));
            dt.Columns.Add("XML_SNP_APPL_DATE", typeof(string));
            dt.Columns.Add("XML_SNP_APPL_TIME", typeof(string));
            dt.Columns.Add("XML_SNP_AMPM", typeof(string));
            dt.Columns.Add("XML_SNP_INTAKE_DATE", typeof(string));
            dt.Columns.Add("XML_SNP_SITE", typeof(string));
            dt.Columns.Add("XML_SNP_TOT_INCOME", typeof(decimal));
            dt.Columns.Add("XML_SNP_PROG_INCOME", typeof(decimal));
            dt.Columns.Add("XML_SNP_CLAIM_SSNO", typeof(string));
            dt.Columns.Add("XML_SNP_CLAIM_SS_BIC", typeof(string));
            dt.Columns.Add("XML_SNP_WAGEM", typeof(string));
            dt.Columns.Add("XML_SNP_WIC", typeof(string));
            dt.Columns.Add("XML_SNP_STUDENT", typeof(string));
            dt.Columns.Add("XML_SNP_RESIDENT", typeof(string));
            dt.Columns.Add("XML_SNP_PREGNANT", typeof(string));
            dt.Columns.Add("XML_SNP_MARITAL_STATUS", typeof(string));
            dt.Columns.Add("XML_SNP_SCHOOL_DISTRICT", typeof(string));
            dt.Columns.Add("XML_SNP_ALIEN_REG_NO", typeof(string));
            dt.Columns.Add("XML_SNP_LEGAL_TO_WORK", typeof(string));
            dt.Columns.Add("XML_SNP_EXPIRE_WORK_DATE", typeof(string));
            dt.Columns.Add("XML_SNP_EMPLOYED", typeof(string));
            dt.Columns.Add("XML_SNP_LAST_WORK_DATE", typeof(string));
            dt.Columns.Add("XML_SNP_WORK_LIMIT", typeof(string));
            dt.Columns.Add("XML_SNP_EXPLAIN_WORK_LIMIT", typeof(string));
            dt.Columns.Add("XML_SNP_NUMBER_OF_C_JOBS", typeof(decimal));
            dt.Columns.Add("XML_SNP_NUMBER_OF_LV_JOBS", typeof(decimal));
            dt.Columns.Add("XML_SNP_FULL_TIME_HOURS", typeof(decimal));
            dt.Columns.Add("XML_SNP_PART_TIME_HOURS", typeof(decimal));
            dt.Columns.Add("XML_SNP_SEASONAL_EMPLOY", typeof(string));
            dt.Columns.Add("XML_SNP_1ST_SHIFT", typeof(string));
            dt.Columns.Add("XML_SNP_2ND_SHIFT", typeof(string));
            dt.Columns.Add("XML_SNP_3RD_SHIFT", typeof(string));
            dt.Columns.Add("XML_SNP_R_SHIFT", typeof(string));
            dt.Columns.Add("XML_SNP_EMPLOYER_NAME", typeof(string));
            dt.Columns.Add("XML_SNP_EMPLOYER_STREET", typeof(string));
            dt.Columns.Add("XML_SNP_EMPLOYER_CITY", typeof(string));
            dt.Columns.Add("XML_SNP_JOB_TITLE", typeof(string));
            dt.Columns.Add("XML_SNP_JOB_CATEGORY", typeof(string));
            dt.Columns.Add("XML_SNP_HOURLY_WAGE", typeof(decimal));
            dt.Columns.Add("XML_SNP_PAY_FREQUENCY", typeof(string));
            dt.Columns.Add("XML_SNP_HIRE_DATE", typeof(string));
            dt.Columns.Add("XML_SNP_TRANSERV", typeof(string));
            dt.Columns.Add("XML_SNP_RELITRAN", typeof(string));
            dt.Columns.Add("XML_SNP_DRVLIC", typeof(string));
            dt.Columns.Add("XML_SNP_RACE", typeof(string));
            dt.Columns.Add("XML_SNP_EMPL_PHONE", typeof(string));
            dt.Columns.Add("XML_SNP_EMPL_EXT", typeof(string));
            dt.Columns.Add("XML_SNP_DOB_NA", typeof(decimal));
            dt.Columns.Add("XML_SNP_SSN_REASON", typeof(string));
            dt.Columns.Add("XML_SNP_EXCLUDE", typeof(string));
            dt.Columns.Add("XML_SNP_BLIND", typeof(string));
            dt.Columns.Add("XML_SNP_ABLE_TO_WORK", typeof(string));
            dt.Columns.Add("XML_SNP_REC_MEDICARE", typeof(string));
            dt.Columns.Add("XML_SNP_PURCHASE_FOOD", typeof(string));
            dt.Columns.Add("XML_SNP_VEHICLE_VALUE", typeof(decimal));
            dt.Columns.Add("XML_SNP_OTHER_VEHICLE_VALUE", typeof(decimal));
            dt.Columns.Add("XML_SNP_OTHER_ASSET_VALUE", typeof(decimal));
            dt.Columns.Add("XML_SNP_DATE_LSTC", typeof(string));
            dt.Columns.Add("XML_SNP_LSTC_OPERATOR", typeof(string));
            dt.Columns.Add("XML_SNP_DATE_ADD", typeof(string));
            dt.Columns.Add("XML_SNP_ADD_OPERATOR", typeof(string));
            dt.Columns.Add("XML_SNP_MILITARY_STATUS", typeof(string));
            dt.Columns.Add("XML_SNP_HEALTH_CODES", typeof(string));
            dt.Columns.Add("XML_SNP_WORK_STAT", typeof(string));
            dt.Columns.Add("XML_SNP_NCASHBEN", typeof(string));
            dt.Columns.Add("XML_SNP_YOUTH", typeof(string));
            dt.Columns.Add("XML_SNP_SUFFIX", typeof(string));
            dt.Columns.Add("XML_SNP_HH_EXCLUDE", typeof(string));
            dt.Columns.Add("XML_SNP_FAMILY_SWITCH", typeof(string));
            dt.Columns.Add("XML_SNP_CLIENT_SWITCH", typeof(string));
            dt.Columns.Add("XML_SNP_SSN_SWITCH", typeof(string));


            return dt;
        }
        public static DataTable XML_INCOME_TYPE()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("XML_INCOME_AGENCY", typeof(string));
            dt.Columns.Add("XML_INCOME_DEPT", typeof(string));
            dt.Columns.Add("XML_INCOME_PROGRAM", typeof(string));
            dt.Columns.Add("XML_INCOME_YEAR", typeof(string));
            dt.Columns.Add("XML_INCOME_APP", typeof(string));
            dt.Columns.Add("XML_INCOME_FAMILY_SEQ", typeof(decimal));
            dt.Columns.Add("XML_INCOME_SEQ", typeof(decimal));
            dt.Columns.Add("XML_INCOME_EXCLUDE", typeof(string));
            dt.Columns.Add("XML_INCOME_TYPE", typeof(string));
            dt.Columns.Add("XML_INCOME_INTERVAL", typeof(string));
            dt.Columns.Add("XML_INCOME_VAL1", typeof(decimal));
            dt.Columns.Add("XML_INCOME_DATE1", typeof(string));
            dt.Columns.Add("XML_INCOME_VAL2", typeof(decimal));
            dt.Columns.Add("XML_INCOME_DATE2", typeof(string));
            dt.Columns.Add("XML_INCOME_VAL3", typeof(decimal));
            dt.Columns.Add("XML_INCOME_DATE3", typeof(string));
            dt.Columns.Add("XML_INCOME_VAL4", typeof(decimal));
            dt.Columns.Add("XML_INCOME_DATE4", typeof(string));
            dt.Columns.Add("XML_INCOME_VAL5", typeof(decimal));
            dt.Columns.Add("XML_INCOME_DATE5", typeof(string));
            dt.Columns.Add("XML_INCOME_FACTOR", typeof(decimal));
            dt.Columns.Add("XML_INCOME_SOURCE", typeof(string));
            dt.Columns.Add("XML_INCOME_VERIFIER", typeof(string));
            dt.Columns.Add("XML_INCOME_HOW_VERIFIED", typeof(string));
            dt.Columns.Add("XML_INCOME_TOT_INCOME", typeof(decimal));
            dt.Columns.Add("XML_INCOME_PROG_INCOME", typeof(decimal));
            dt.Columns.Add("XML_INCOME_LSTC_OPERATOR", typeof(string));
            dt.Columns.Add("XML_INCOME_DATE_LSTC", typeof(string));
            dt.Columns.Add("XML_INCOME_DATE_ADD", typeof(string));
            dt.Columns.Add("XML_INCOME_ADD_OPERATOR", typeof(string));
            dt.Columns.Add("XML_INCOME_HR_RATE1", typeof(decimal));
            dt.Columns.Add("XML_INCOME_HR_RATE2", typeof(decimal));
            dt.Columns.Add("XML_INCOME_HR_RATE3", typeof(decimal));
            dt.Columns.Add("XML_INCOME_HR_RATE4", typeof(decimal));
            dt.Columns.Add("XML_INCOME_HR_RATE5", typeof(decimal));
            dt.Columns.Add("XML_INCOME_AVG", typeof(decimal));


            return dt;
        }
        public static DataTable XML_DIFF_TYPE()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("XML_DIFF_AGENCY", typeof(string));
            dt.Columns.Add("XML_DIFF_DEPT", typeof(string));
            dt.Columns.Add("XML_DIFF_PROGRAM", typeof(string));
            dt.Columns.Add("XML_DIFF_YEAR", typeof(string));
            dt.Columns.Add("XML_DIFF_APP_NO", typeof(string));
            dt.Columns.Add("XML_DIFF_STATE", typeof(string));
            dt.Columns.Add("XML_DIFF_CITY", typeof(string));
            dt.Columns.Add("XML_DIFF_STREET", typeof(string));
            dt.Columns.Add("XML_DIFF_SUFFIX", typeof(string));
            dt.Columns.Add("XML_DIFF_HN", typeof(string));
            dt.Columns.Add("XML_DIFF_APT", typeof(string));
            dt.Columns.Add("XML_DIFF_FLR", typeof(string));
            dt.Columns.Add("XML_DIFF_ZIP", typeof(decimal));
            dt.Columns.Add("XML_DIFF_ZIPPLUS", typeof(decimal));
            dt.Columns.Add("XML_DIFF_DIRECTION", typeof(string));
            dt.Columns.Add("XML_DIFF_INCARE_FIRST", typeof(string));
            dt.Columns.Add("XML_DIFF_INCARE_LAST", typeof(string));
            dt.Columns.Add("XML_DIFF_COUNTY", typeof(string));
            dt.Columns.Add("XML_DIFF_PHONE", typeof(string));
            dt.Columns.Add("XML_DIFF_DATE_ADD", typeof(string));
            dt.Columns.Add("XML_DIFF_ADD_OPERATOR", typeof(string));
            dt.Columns.Add("XML_DIFF_DATE_LSTC", typeof(string));
            dt.Columns.Add("XML_DIFF_LSTC_OPERATOR", typeof(string));


            return dt;
        }
        public static DataTable XML_LLR_TYPE()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("XML_LLR_AGENCY", typeof(string));
            dt.Columns.Add("XML_LLR_DEPT", typeof(string));
            dt.Columns.Add("XML_LLR_PROGRAM", typeof(string));
            dt.Columns.Add("XML_LLR_YEAR", typeof(string));
            dt.Columns.Add("XML_LLR_APP_NO", typeof(string));
            dt.Columns.Add("XML_LLR_FIRST_NAME", typeof(string));
            dt.Columns.Add("XML_LLR_LAST_NAME", typeof(string));
            dt.Columns.Add("XML_LLR_STATE", typeof(string));
            dt.Columns.Add("XML_LLR_CITY", typeof(string));
            dt.Columns.Add("XML_LLR_STREET", typeof(string));
            dt.Columns.Add("XML_LLR_SUFFIX", typeof(string));
            dt.Columns.Add("XML_LLR_HN", typeof(string));
            dt.Columns.Add("XML_LLR_APT", typeof(string));
            dt.Columns.Add("XML_LLR_FLR", typeof(string));
            dt.Columns.Add("XML_LLR_ZIP", typeof(int));
            dt.Columns.Add("XML_LLR_ZIPPLUS", typeof(int));
            dt.Columns.Add("XML_LLR_DIRECTION", typeof(string));
            dt.Columns.Add("XML_LLR_COUNTY", typeof(string));
            dt.Columns.Add("XML_LLR_PHONE", typeof(string));
            dt.Columns.Add("XML_LLR_DATE_ADD", typeof(string));
            dt.Columns.Add("XML_LLR_ADD_OPERATOR", typeof(string));
            dt.Columns.Add("XML_LLR_DATE_LSTC", typeof(string));
            dt.Columns.Add("XML_LLR_LSTC_OPERATOR", typeof(string));

            return dt;
        }
        public static DataTable XML_LPV_TYPE()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("XML_LPV_AGENCY", typeof(string));
            dt.Columns.Add("XML_LPV_DEPT", typeof(string));
            dt.Columns.Add("XML_LPV_PROGRAM", typeof(string));
            dt.Columns.Add("XML_LPV_YEAR", typeof(string));
            dt.Columns.Add("XML_LPV_APP_NO", typeof(string));
            dt.Columns.Add("XML_LPV_SEQ", typeof(int));
            dt.Columns.Add("XML_LPV_VENDOR", typeof(string));
            dt.Columns.Add("XML_LPV_ACCOUNT_NO", typeof(string));
            dt.Columns.Add("XML_LPV_PRIMARY_CODE", typeof(string));
            dt.Columns.Add("XML_LPV_PAYMENT_FOR", typeof(string));
            dt.Columns.Add("XML_LPV_CYCLE", typeof(string));
            dt.Columns.Add("XML_LPV_DIVIDE_BILL", typeof(int));
            dt.Columns.Add("XML_LPV_BILL_LNAME", typeof(string));
            dt.Columns.Add("XML_LPV_BILL_FNAME", typeof(string));
            dt.Columns.Add("XML_LPV_MOR", typeof(string));
            dt.Columns.Add("XML_LPV_METER", typeof(string));
            dt.Columns.Add("XML_LPV_DATE_LSTC", typeof(string));
            dt.Columns.Add("XML_LPV_LSTC_OPERATOR", typeof(string));
            dt.Columns.Add("XML_LPV_DATE_ADD", typeof(string));
            dt.Columns.Add("XML_LPV_ADD_OPERATOR", typeof(string));
            dt.Columns.Add("XML_LPV_VERIFY_DATE", typeof(string));
            dt.Columns.Add("XML_LPV_VERIFIED_BY", typeof(string));
            dt.Columns.Add("XML_LPV_REVERIFY", typeof(string));
            dt.Columns.Add("XML_LPV_ACCOUNT_SWITCH", typeof(string));
            dt.Columns.Add("XML_LPV_FAILED_ACCOUNT_EDIT", typeof(string));
            dt.Columns.Add("XML_LPV_BILLNAME_TYPE", typeof(string));


            return dt;
        }
        public static DataTable XML_LPW_TYPE()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("XML_LPW_AGENCY", typeof(string));
            dt.Columns.Add("XML_LPW_DEPT", typeof(string));
            dt.Columns.Add("XML_LPW_PROG", typeof(string));
            dt.Columns.Add("XML_LPW_YEAR", typeof(string));
            dt.Columns.Add("XML_LPW_APP_NO", typeof(string));
            dt.Columns.Add("XML_LPW_SEQ", typeof(int));
            dt.Columns.Add("XML_LPW_BEN_DATE", typeof(string));
            dt.Columns.Add("XML_LPW_BEN_WORKER", typeof(string));
            dt.Columns.Add("XML_LPW_CERTIFIED_STATUS", typeof(string));
            dt.Columns.Add("XML_LPW_REASON_DENIED", typeof(string));
            dt.Columns.Add("XML_LPW_FAM_INCOME", typeof(decimal));
            dt.Columns.Add("XML_LPW_FAM_NO_INHH", typeof(decimal));
            dt.Columns.Add("XML_LPW_OMB", typeof(decimal));
            dt.Columns.Add("XML_LPW_SMI", typeof(decimal));
            dt.Columns.Add("XML_LPW_WAT_VENDOR", typeof(string));
            dt.Columns.Add("XML_LPW_WAT_ACCOUNT_NO", typeof(string));
            dt.Columns.Add("XML_LPW_WAT_BILLNAME_TYPE", typeof(string));
            dt.Columns.Add("XML_LPW_WAT_BILL_LNAME", typeof(string));
            dt.Columns.Add("XML_LPW_WAT_BILL_FNAME", typeof(string));
            dt.Columns.Add("XML_LPW_WAT_INVDATE", typeof(string));
            dt.Columns.Add("XML_LPW_WAT_FREQ_BILL", typeof(string));
            dt.Columns.Add("XML_LPW_WAT_ARREARS", typeof(decimal));
            dt.Columns.Add("XML_LPW_WAT_CURBILL_AMT", typeof(decimal));
            dt.Columns.Add("XML_LPW_WAT_FEES", typeof(decimal));
            dt.Columns.Add("XML_LPW_WAT_TOT", typeof(decimal));
            dt.Columns.Add("XML_LPW_WAT_AWARD", typeof(decimal));
            dt.Columns.Add("XML_LPW_WAT_PAID", typeof(decimal));
            dt.Columns.Add("XML_LPW_WAT_AWAITING_DOL", typeof(decimal));
            dt.Columns.Add("XML_LPW_WAT_BEN_TYPE", typeof(string));
            dt.Columns.Add("XML_LPW_WAT_LEVEL", typeof(string));
            dt.Columns.Add("XML_LPW_WAT_VER_STAT", typeof(string));
            dt.Columns.Add("XML_LPW_WAT_VERIFIED_WORKER", typeof(string));
            dt.Columns.Add("XML_LPW_WAT_VERIFIED_DATE", typeof(string));
            dt.Columns.Add("XML_LPW_WAT_VERIFIED_DATE_LSTC", typeof(string));
            dt.Columns.Add("XML_LPW_WAT_VERIFIED_LSTC_OPERATOR", typeof(string));
            dt.Columns.Add("XML_LPW_SEW_VENDOR", typeof(string));
            dt.Columns.Add("XML_LPW_SEW_ACCOUNT_NO", typeof(string));
            dt.Columns.Add("XML_LPW_SEW_BILLNAME_TYPE", typeof(string));
            dt.Columns.Add("XML_LPW_SEW_BILL_LNAME", typeof(string));
            dt.Columns.Add("XML_LPW_SEW_BILL_FNAME", typeof(string));
            dt.Columns.Add("XML_LPW_SEW_INVDATE", typeof(string));
            dt.Columns.Add("XML_LPW_SEW_FREQ_BILL", typeof(string));
            dt.Columns.Add("XML_LPW_SEW_ARREARS", typeof(decimal));
            dt.Columns.Add("XML_LPW_SEW_CURBILL_AMT", typeof(decimal));
            dt.Columns.Add("XML_LPW_SEW_FEES", typeof(decimal));
            dt.Columns.Add("XML_LPW_SEW_TOT", typeof(decimal));
            dt.Columns.Add("XML_LPW_SEW_AWARD", typeof(decimal));
            dt.Columns.Add("XML_LPW_SEW_PAID", typeof(decimal));
            dt.Columns.Add("XML_LPW_SEW_AWAITING_DOL", typeof(decimal));
            dt.Columns.Add("XML_LPW_SEW_BEN_TYPE", typeof(string));
            dt.Columns.Add("XML_LPW_SEW_LEVEL", typeof(string));
            dt.Columns.Add("XML_LPW_SEW_VER_STAT", typeof(string));
            dt.Columns.Add("XML_LPW_SEW_VERIFIED_WORKER", typeof(string));
            dt.Columns.Add("XML_LPW_SEW_VERIFIED_DATE", typeof(string));
            dt.Columns.Add("XML_LPW_SEW_VERIFIED_DATE_LSTC", typeof(string));
            dt.Columns.Add("XML_LPW_SEW_VERIFIED_LSTC_OPERATOR", typeof(string));
            dt.Columns.Add("XML_LPW_DATE_ADD", typeof(string));
            dt.Columns.Add("XML_LPW_ADD_OPERATOR", typeof(string));
            dt.Columns.Add("XML_LPW_DATE_LSTC", typeof(string));
            dt.Columns.Add("XML_LPW_LSTC_OPERATOR", typeof(string));

            return dt;
        }



        public static DataTable GetAPPMEMS_Search(string strConnection, string Agency, string Dept, string Prog, string Year, string SSN, string DOB, string FName)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(strConnection);
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "CAPS_APPMEMS_SEARCH";
                    cmd.CommandTimeout = 200;
                    cmd.CommandType = CommandType.StoredProcedure;


                    if (Agency != string.Empty)
                        cmd.Parameters.AddWithValue("@AGENCY", Agency);
                    if (Dept != string.Empty)
                        cmd.Parameters.AddWithValue("@DEPT", Dept);
                    if (Prog != string.Empty)
                        cmd.Parameters.AddWithValue("@PROGRAM", Prog);
                    if (Year != string.Empty)
                        cmd.Parameters.AddWithValue("@YEAR", Year);
                    if (SSN != string.Empty)
                        cmd.Parameters.AddWithValue("@SSN", SSN);
                    if (DOB != string.Empty)
                        cmd.Parameters.AddWithValue("@DOB", DOB);
                    if (FName != string.Empty)
                        cmd.Parameters.AddWithValue("@FNAME", FName);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    da.Fill(dt);


                }
                con.Close();

            }
            catch (Exception ex)
            {


            }
            return dt;
        }

        public static DataTable GetClearWorkKeys(string strConnection, string Agency, string AppNo, string AppID, string Mode)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(strConnection);
                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "CLEARDSSWaitRoomKeys";
                    cmd.CommandTimeout = 200;
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (AppNo != string.Empty)
                        cmd.Parameters.AddWithValue("@MSTKey", AppNo);
                    if (Agency != string.Empty)
                        cmd.Parameters.AddWithValue("@AGENCY", Agency);
                    if (AppID != string.Empty)
                        cmd.Parameters.AddWithValue("@APPID", AppID);
                    if (Mode != string.Empty)
                        cmd.Parameters.AddWithValue("@MODE", Mode);

                    //if (Year != string.Empty)
                    //    cmd.Parameters.AddWithValue("@YEAR", Year);
                    //if (SSN != string.Empty)
                    //    cmd.Parameters.AddWithValue("@SSN", SSN);
                    //if (DOB != string.Empty)
                    //    cmd.Parameters.AddWithValue("@DOB", DOB);
                    //if (FName != string.Empty)
                    //    cmd.Parameters.AddWithValue("@FNAME", FName);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    da.Fill(dt);


                }
                con.Close();

            }
            catch (Exception ex)
            {


            }
            return dt;
        }


    }
}

public class XML_MST_TYPE
{
    public string XML_MST_AGENCY { get; set; }
    public string XML_MST_DEPT { get; set; }
    public string XML_MST_PROGRAM { get; set; }
    public string XML_MST_YEAR { get; set; }
    public string XML_MST_APP_NO { get; set; }
    public decimal XML_MST_FAMILY_SEQ { get; set; }
    public string XML_MST_FAMILY_ID { get; set; }
    public decimal XML_MST_CLIENT_ID { get; set; }
    public string XML_MST_SSN { get; set; }
    public string XML_MST_BIC { get; set; }
    public string XML_MST_NICKNAME { get; set; }
    public string XML_MST_ETHNIC_OTHER { get; set; }
    public string XML_MST_STATE { get; set; }
    public string XML_MST_CITY { get; set; }
    public string XML_MST_STREET { get; set; }
    public string XML_MST_SUFFIX { get; set; }
    public string XML_MST_HN { get; set; }
    public string XML_MST_DIRECTION { get; set; }
    public string XML_MST_APT { get; set; }
    public string XML_MST_FLR { get; set; }
    public decimal XML_MST_ZIP { get; set; }
    public decimal XML_MST_ZIPPLUS { get; set; }
    public string XML_MST_PRECINCT { get; set; }
    public string XML_MST_AREA { get; set; }
    public string XML_MST_PHONE { get; set; }
    public string XML_MST_NEXTYEAR { get; set; }
    public string XML_MST_CLASSIFICATION { get; set; }
    public string XML_MST_LANGUAGE { get; set; }
    public string XML_MST_LANGUAGE_OT { get; set; }
    public string XML_MST_INTAKE_WORKER { get; set; }
    public string XML_MST_INTAKE_DATE { get; set; }
    public string XML_MST_INITIAL_DATE { get; set; }
    public string XML_MST_CASE_TYPE { get; set; }
    public string XML_MST_HOUSING { get; set; }
    public string XML_MST_FAMILY_TYPE { get; set; }
    public string XML_MST_SITE { get; set; }
    public string XML_MST_JUVENILE { get; set; }
    public string XML_MST_SENIOR { get; set; }
    public string XML_MST_SECRET { get; set; }
    public string XML_MST_CASE_REVIEW_DATE { get; set; }
    public string XML_MST_ALERT_CODES { get; set; }
    public string XML_MST_PARENT_STATUS { get; set; }
    public string XML_MST_INTAKE_HRS { get; set; }
    public string XML_MST_INTAKE_MNS { get; set; }
    public string XML_MST_INTAKE_SCS { get; set; }
    public decimal XML_MST_FIN_HRS { get; set; }
    public decimal XML_MST_FIN_MNS { get; set; }
    public decimal XML_MST_FIN_SCS { get; set; }
    public decimal XML_MST_SIM_HRS { get; set; }
    public decimal XML_MST_SIM_MNS { get; set; }
    public decimal XML_MST_SIM_SCS { get; set; }
    public decimal XML_MST_MED_HRS { get; set; }
    public decimal XML_MST_MED_MNS { get; set; }
    public decimal XML_MST_MED_SCS { get; set; }
    public decimal XML_MST_RANK1 { get; set; }
    public decimal XML_MST_RANK2 { get; set; }
    public decimal XML_MST_RANK3 { get; set; }
    public decimal XML_MST_RANK4 { get; set; }
    public decimal XML_MST_RANK5 { get; set; }
    public decimal XML_MST_RANK6 { get; set; }
    public string XML_MST_POSITION1 { get; set; }
    public string XML_MST_POSITION2 { get; set; }
    public string XML_MST_POSITION3 { get; set; }
    public string XML_MST_TOWNSHIP { get; set; }
    public string XML_MST_INTAKE_TIME1 { get; set; }
    public string XML_MST_SSN_FLAG { get; set; }
    public string XML_MST_STATE_CASE { get; set; }
    public string XML_MST_VERIFIER { get; set; }
    public string XML_MST_ELIG_DATE { get; set; }
    public string XML_MST_CAT_ELIG { get; set; }
    public string XML_MST_MEAL_ELIG { get; set; }
    public string XML_MST_VERIFY_W2 { get; set; }
    public string XML_MST_VERIFY_CHECK_STUB { get; set; }
    public string XML_MST_VERIFY_TAX_RETURN { get; set; }
    public string XML_MST_VERIFY_LETTER { get; set; }
    public string XML_MST_VERIFY_OTHER { get; set; }
    public string XML_MST_REVERIFY_DATE { get; set; }
    public string XML_MST_INCOME_TYPES { get; set; }
    public decimal XML_MST_POVERTY { get; set; }
    public string XML_MST_WAIT_LIST { get; set; }
    public string XML_MST_ACTIVE_STATUS { get; set; }
    public decimal XML_MST_TOTAL_RANK { get; set; }
    public decimal XML_MST_NO_INHH { get; set; }
    public decimal XML_MST_FAM_INCOME { get; set; }
    public decimal XML_MST_NO_INPROG { get; set; }
    public decimal XML_MST_PROG_INCOME { get; set; }
    public string XML_MST_OUT_OF_SERVICE { get; set; }
    public decimal XML_MST_HUD { get; set; }
    public decimal XML_MST_SMI { get; set; }
    public decimal XML_MST_CMI { get; set; }
    public string XML_MST_COUNTY { get; set; }
    public decimal XML_MST_ADDRESS_YEARS { get; set; }
    public string XML_MST_MESSAGE_PHONE { get; set; }
    public string XML_MST_CELL_PHONE { get; set; }
    public string XML_MST_FAX_NUMBER { get; set; }
    public string XML_MST_TTY_NUMBER { get; set; }
    public string XML_MST_EMAIL { get; set; }
    public string XML_MST_BEST_CONTACT { get; set; }
    public string XML_MST_ABOUT_US { get; set; }
    public string XML_MST_IMPORT_DATE { get; set; }
    public string XML_MST_DATE_ADDED { get; set; }
    public string XML_MST_EXP_CASEWORKER { get; set; }
    public decimal XML_MST_EXP_RENT { get; set; }
    public decimal XML_MST_EXP_WATER { get; set; }
    public decimal XML_MST_EXP_ELECTRIC { get; set; }
    public decimal XML_MST_EXP_HEAT { get; set; }
    public decimal XML_MST_EXP_MISC { get; set; }
    public decimal XML_MST_EXP_TOTAL { get; set; }
    public decimal XML_MST_EXP_LIVEXPENSE { get; set; }
    public decimal XML_MST_DEBT_CC { get; set; }
    public decimal XML_MST_DEBT_LOANS { get; set; }
    public decimal XML_MST_DEBT_MED { get; set; }
    public decimal XML_MST_DEBT_OTH { get; set; }
    public decimal XML_MST_DEBT_MISC { get; set; }
    public decimal XML_MST_DEBT_TOTAL { get; set; }
    public decimal XML_MST_ASET_PHY { get; set; }
    public decimal XML_MST_ASET_LIQ { get; set; }
    public decimal XML_MST_ASET_OTH { get; set; }
    public decimal XML_MST_ASET_TOTAL { get; set; }
    public decimal XML_MST_ASET_MISC { get; set; }
    public decimal XML_MST_DEB_ASET_RATIO { get; set; }
    public decimal XML_MST_DEB_INCM_RATIO { get; set; }
    public string XML_MST_DWELLING { get; set; }
    public string XML_MST_HEAT_INC_RENT { get; set; }
    public string XML_MST_SOURCE { get; set; }
    public string XML_MST_ROLLOVER { get; set; }
    public decimal XML_MST_RISK_VALUE { get; set; }
    public string XML_MST_SUBSHOUSE { get; set; }
    public string XML_MST_SUBSTYPE { get; set; }
    public string XML_MST_VER_FUND { get; set; }
    public string XML_MST_OMB_SCREEN { get; set; }
    public decimal XML_MST_CB_CASE_MANAGER { get; set; }
    public string XML_MST_CASE_MANAGER { get; set; }
    public string XML_MST_VERIFY_OTH_CMB { get; set; }
    public string XML_MST_SIM_PRINT { get; set; }
    public string XML_MST_SIM_PRINT_DATE { get; set; }
    public decimal XML_MST_CB_FRAUD { get; set; }
    public string XML_MST_FRAUD_DATE { get; set; }
    public string XML_MST_PRESS_JOB { get; set; }
    public string XML_MST_PRESS_HSD { get; set; }
    public string XML_MST_PRESS_RW_ENG { get; set; }
    public string XML_MST_PRESS_SKILLS { get; set; }
    public string XML_MST_PRESS_HOUSING { get; set; }
    public string XML_MST_PRESS_TRANSPORT { get; set; }
    public string XML_MST_PRESS_CHLDCARE { get; set; }
    public string XML_MST_PRESS_CCENRL { get; set; }
    public string XML_MST_PRESS_ELDRCARE { get; set; }
    public string XML_MST_PRESS_ECNEED { get; set; }
    public string XML_MST_PRESS_CHINS { get; set; }
    public string XML_MST_PRESS_AHINS { get; set; }
    public string XML_MST_PRESS_CURR_DSS { get; set; }
    public string XML_MST_PRESS_RECV_DSS { get; set; }
    public string XML_MST_LPM_0001 { get; set; }
    public string XML_MST_LPM_0002 { get; set; }
    public string XML_MST_LPM_0003 { get; set; }
    public string XML_MST_LPM_0004 { get; set; }
    public string XML_MST_LPM_0005 { get; set; }
    public string XML_MST_LPM_0006 { get; set; }
    public string XML_MST_LPM_0007 { get; set; }
    public string XML_MST_LPM_0008 { get; set; }
    public string XML_MST_LPM_0009 { get; set; }
    public string XML_MST_LPM_0010 { get; set; }
    public string XML_MST_LPM_0011 { get; set; }
    public string XML_MST_DATE_ADD_1 { get; set; }
    public string XML_MST_ADD_OPERATOR_1 { get; set; }
    public string XML_MST_DATE_LSTC_1 { get; set; }
    public string XML_MST_LSTC_OPERATOR_1 { get; set; }
    public decimal XML_MST_TIMES_UPDATED_1 { get; set; }
    public string XML_MST_DATE_ADD_2 { get; set; }
    public string XML_MST_ADD_OPERATOR_2 { get; set; }
    public string XML_MST_DATE_LSTC_2 { get; set; }
    public string XML_MST_LSTC_OPERATOR_2 { get; set; }
    public decimal XML_MST_TIMES_UPDATED_2 { get; set; }
    public string XML_MST_DATE_ADD_3 { get; set; }
    public string XML_MST_ADD_OPERATOR_3 { get; set; }
    public string XML_MST_DATE_LSTC_3 { get; set; }
    public string XML_MST_LSTC_OPERATOR_3 { get; set; }
    public decimal XML_MST_TIMES_UPDATED_3 { get; set; }
    public string XML_MST_DATE_ADD_4 { get; set; }
    public string XML_MST_ADD_OPERATOR_4 { get; set; }
    public string XML_MST_DATE_LSTC_4 { get; set; }
    public string XML_MST_LSTC_OPERATOR_4 { get; set; }
    public decimal XML_MST_TIMES_UPDATED_4 { get; set; }
    public decimal XML_MST_PRESS_TOTAL { get; set; }
    public string XML_MST_PRESS_CAT { get; set; }
    public string XML_MST_PRESS_GRP { get; set; }
    public string XML_MST_DATE_LSTC_5 { get; set; }
    public string XML_MST_LSTC_OPERATOR_5 { get; set; }
    public string XML_MST_NCASHBEN { get; set; }
    public string XML_MST_APPLICANT_TYPE { get; set; }
    public string XML_MST_APPLICANT_DATE { get; set; }
    public string XML_MST_HOME_NA { get; set; }
    public string XML_MST_CELL_NA { get; set; }
    public string XML_MST_MESSAGE_NA { get; set; }
    public string XML_MST_EMAIL_NA { get; set; }
    public string XML_MST_LPM_0012 { get; set; }
    public string XML_MST_LPM_0013 { get; set; }
    public string XML_MST_LPM_0014 { get; set; }
    public string XML_MST_LPM_0015 { get; set; }
    public string XML_MST_LPM_0016 { get; set; }
    public string XML_MST_LPM_0017 { get; set; }
    public string XML_MST_CASE_COMPLETE_DATE { get; set; }
    public string XML_MST_VERIFY_SELF_DECL { get; set; }
    public string XML_MST_DSS_APPID { get; set; }
}

public class XML_SNP_TYPE
{
    public string XML_SNP_AGENCY { get; set; }
    public string XML_SNP_DEPT { get; set; }
    public string XML_SNP_PROGRAM { get; set; }
    public string XML_SNP_YEAR { get; set; }
    public string XML_SNP_APP { get; set; }
    public decimal XML_SNP_FAMILY_SEQ { get; set; }
    public string XML_SNP_MEMBER_CODE { get; set; }
    public decimal XML_SNP_CLIENT_ID { get; set; }
    public string XML_SNP_SSNO { get; set; }
    public string XML_SNP_SS_BIC { get; set; }
    public string XML_SNP_NAME_IX_LAST { get; set; }
    public string XML_SNP_NAME_IX_FI { get; set; }
    public string XML_SNP_NAME_IX_MI { get; set; }
    public string XML_SNP_ALT_BDATE { get; set; }
    public string XML_SNP_ALT_LNAME { get; set; }
    public string XML_SNP_ALT_FI { get; set; }
    public string XML_SNP_ALIAS { get; set; }
    public string XML_SNP_STATUS { get; set; }
    public string XML_SNP_SEX { get; set; }
    public decimal XML_SNP_AGE { get; set; }
    public string XML_SNP_ETHNIC { get; set; }
    public string XML_SNP_EDUCATION { get; set; }
    public string XML_SNP_INCOME_BASIS { get; set; }
    public string XML_SNP_HEALTH_INS { get; set; }
    public string XML_SNP_VET { get; set; }
    public string XML_SNP_DISABLE { get; set; }
    public string XML_SNP_FOOD_STAMPS { get; set; }
    public string XML_SNP_FARMER { get; set; }
    public string XML_SNP_APPL_DATE { get; set; }
    public string XML_SNP_APPL_TIME { get; set; }
    public string XML_SNP_AMPM { get; set; }
    public string XML_SNP_INTAKE_DATE { get; set; }
    public string XML_SNP_SITE { get; set; }
    public decimal XML_SNP_TOT_INCOME { get; set; }
    public decimal XML_SNP_PROG_INCOME { get; set; }
    public string XML_SNP_CLAIM_SSNO { get; set; }
    public string XML_SNP_CLAIM_SS_BIC { get; set; }
    public string XML_SNP_WAGEM { get; set; }
    public string XML_SNP_WIC { get; set; }
    public string XML_SNP_STUDENT { get; set; }
    public string XML_SNP_RESIDENT { get; set; }
    public string XML_SNP_PREGNANT { get; set; }
    public string XML_SNP_MARITAL_STATUS { get; set; }
    public string XML_SNP_SCHOOL_DISTRICT { get; set; }
    public string XML_SNP_ALIEN_REG_NO { get; set; }
    public string XML_SNP_LEGAL_TO_WORK { get; set; }
    public string XML_SNP_EXPIRE_WORK_DATE { get; set; }
    public string XML_SNP_EMPLOYED { get; set; }
    public string XML_SNP_LAST_WORK_DATE { get; set; }
    public string XML_SNP_WORK_LIMIT { get; set; }
    public string XML_SNP_EXPLAIN_WORK_LIMIT { get; set; }
    public decimal XML_SNP_NUMBER_OF_C_JOBS { get; set; }
    public decimal XML_SNP_NUMBER_OF_LV_JOBS { get; set; }
    public decimal XML_SNP_FULL_TIME_HOURS { get; set; }
    public decimal XML_SNP_PART_TIME_HOURS { get; set; }
    public string XML_SNP_SEASONAL_EMPLOY { get; set; }
    public string XML_SNP_1ST_SHIFT { get; set; }
    public string XML_SNP_2ND_SHIFT { get; set; }
    public string XML_SNP_3RD_SHIFT { get; set; }
    public string XML_SNP_R_SHIFT { get; set; }
    public string XML_SNP_EMPLOYER_NAME { get; set; }
    public string XML_SNP_EMPLOYER_STREET { get; set; }
    public string XML_SNP_EMPLOYER_CITY { get; set; }
    public string XML_SNP_JOB_TITLE { get; set; }
    public string XML_SNP_JOB_CATEGORY { get; set; }
    public decimal XML_SNP_HOURLY_WAGE { get; set; }
    public string XML_SNP_PAY_FREQUENCY { get; set; }
    public string XML_SNP_HIRE_DATE { get; set; }
    public string XML_SNP_TRANSERV { get; set; }
    public string XML_SNP_RELITRAN { get; set; }
    public string XML_SNP_DRVLIC { get; set; }
    public string XML_SNP_RACE { get; set; }
    public string XML_SNP_EMPL_PHONE { get; set; }
    public string XML_SNP_EMPL_EXT { get; set; }
    public decimal XML_SNP_DOB_NA { get; set; }
    public string XML_SNP_SSN_REASON { get; set; }
    public string XML_SNP_EXCLUDE { get; set; }
    public string XML_SNP_BLIND { get; set; }
    public string XML_SNP_ABLE_TO_WORK { get; set; }
    public string XML_SNP_REC_MEDICARE { get; set; }
    public string XML_SNP_PURCHASE_FOOD { get; set; }
    public decimal XML_SNP_VEHICLE_VALUE { get; set; }
    public decimal XML_SNP_OTHER_VEHICLE_VALUE { get; set; }
    public decimal XML_SNP_OTHER_ASSET_VALUE { get; set; }
    public string XML_SNP_DATE_LSTC { get; set; }
    public string XML_SNP_LSTC_OPERATOR { get; set; }
    public string XML_SNP_DATE_ADD { get; set; }
    public string XML_SNP_ADD_OPERATOR { get; set; }
    public string XML_SNP_MILITARY_STATUS { get; set; }
    public string XML_SNP_HEALTH_CODES { get; set; }
    public string XML_SNP_WORK_STAT { get; set; }
    public string XML_SNP_NCASHBEN { get; set; }
    public string XML_SNP_YOUTH { get; set; }
    public string XML_SNP_SUFFIX { get; set; }
    public string XML_SNP_HH_EXCLUDE { get; set; }
    public string XML_SNP_FAMILY_SWITCH { get; set; }
    public string XML_SNP_CLIENT_SWITCH { get; set; }
    public string XML_SNP_SSN_SWITCH { get; set; }

}

public class XML_INC_TYPE
{
    public string XML_INCOME_AGENCY { get; set; }
    public string XML_INCOME_DEPT { get; set; }
    public string XML_INCOME_PROGRAM { get; set; }
    public string XML_INCOME_YEAR { get; set; }
    public string XML_INCOME_APP { get; set; }
    public decimal XML_INCOME_FAMILY_SEQ { get; set; }
    public decimal XML_INCOME_SEQ { get; set; }
    public string XML_INCOME_EXCLUDE { get; set; }
    public string XML_INCOME_TYPE { get; set; }
    public string XML_INCOME_INTERVAL { get; set; }
    public decimal XML_INCOME_VAL1 { get; set; }
    public string XML_INCOME_DATE1 { get; set; }
    public decimal XML_INCOME_VAL2 { get; set; }
    public string XML_INCOME_DATE2 { get; set; }
    public decimal XML_INCOME_VAL3 { get; set; }
    public string XML_INCOME_DATE3 { get; set; }
    public decimal XML_INCOME_VAL4 { get; set; }
    public string XML_INCOME_DATE4 { get; set; }
    public decimal XML_INCOME_VAL5 { get; set; }
    public string XML_INCOME_DATE5 { get; set; }
    public decimal XML_INCOME_FACTOR { get; set; }
    public string XML_INCOME_SOURCE { get; set; }
    public string XML_INCOME_VERIFIER { get; set; }
    public string XML_INCOME_HOW_VERIFIED { get; set; }
    public decimal XML_INCOME_TOT_INCOME { get; set; }
    public decimal XML_INCOME_PROG_INCOME { get; set; }
    public string XML_INCOME_LSTC_OPERATOR { get; set; }
    public string XML_INCOME_DATE_LSTC { get; set; }
    public string XML_INCOME_DATE_ADD { get; set; }
    public string XML_INCOME_ADD_OPERATOR { get; set; }
    public decimal XML_INCOME_HR_RATE1 { get; set; }
    public decimal XML_INCOME_HR_RATE2 { get; set; }
    public decimal XML_INCOME_HR_RATE3 { get; set; }
    public decimal XML_INCOME_HR_RATE4 { get; set; }
    public decimal XML_INCOME_HR_RATE5 { get; set; }
    public decimal XML_INCOME_AVG { get; set; }

}

public class XML_DIFF_TYPE
{

    public string XML_DIFF_AGENCY { get; set; }
    public string XML_DIFF_DEPT { get; set; }
    public string XML_DIFF_PROGRAM { get; set; }
    public string XML_DIFF_YEAR { get; set; }
    public string XML_DIFF_APP_NO { get; set; }
    public string XML_DIFF_STATE { get; set; }
    public string XML_DIFF_CITY { get; set; }
    public string XML_DIFF_STREET { get; set; }
    public string XML_DIFF_SUFFIX { get; set; }
    public string XML_DIFF_HN { get; set; }
    public string XML_DIFF_APT { get; set; }
    public string XML_DIFF_FLR { get; set; }
    public decimal XML_DIFF_ZIP { get; set; }
    public decimal XML_DIFF_ZIPPLUS { get; set; }
    public string XML_DIFF_DIRECTION { get; set; }
    public string XML_DIFF_INCARE_FIRST { get; set; }
    public string XML_DIFF_INCARE_LAST { get; set; }
    public string XML_DIFF_COUNTY { get; set; }
    public string XML_DIFF_PHONE { get; set; }
    public string XML_DIFF_DATE_ADD { get; set; }
    public string XML_DIFF_ADD_OPERATOR { get; set; }
    public string XML_DIFF_DATE_LSTC { get; set; }
    public string XML_DIFF_LSTC_OPERATOR { get; set; }

}

public class XML_LLR_TYPE
{
    public string XML_LLR_AGENCY { get; set; }
    public string XML_LLR_DEPT { get; set; }
    public string XML_LLR_PROGRAM { get; set; }
    public string XML_LLR_YEAR { get; set; }
    public string XML_LLR_APP_NO { get; set; }
    public string XML_LLR_FIRST_NAME { get; set; }
    public string XML_LLR_LAST_NAME { get; set; }
    public string XML_LLR_STATE { get; set; }
    public string XML_LLR_CITY { get; set; }
    public string XML_LLR_STREET { get; set; }
    public string XML_LLR_SUFFIX { get; set; }
    public string XML_LLR_HN { get; set; }
    public string XML_LLR_APT { get; set; }
    public string XML_LLR_FLR { get; set; }
    public int XML_LLR_ZIP { get; set; }
    public int XML_LLR_ZIPPLUS { get; set; }
    public string XML_LLR_DIRECTION { get; set; }
    public string XML_LLR_COUNTY { get; set; }
    public string XML_LLR_PHONE { get; set; }
    public string XML_LLR_DATE_ADD { get; set; }
    public string XML_LLR_ADD_OPERATOR { get; set; }
    public string XML_LLR_DATE_LSTC { get; set; }
    public string XML_LLR_LSTC_OPERATOR { get; set; }

}

public class XML_LPV_TYPE
{

    public string XML_LPV_AGENCY { get; set; }
    public string XML_LPV_DEPT { get; set; }
    public string XML_LPV_PROGRAM { get; set; }
    public string XML_LPV_YEAR { get; set; }
    public string XML_LPV_APP_NO { get; set; }
    public int XML_LPV_SEQ { get; set; }
    public string XML_LPV_VENDOR { get; set; }
    public string XML_LPV_ACCOUNT_NO { get; set; }
    public string XML_LPV_PRIMARY_CODE { get; set; }
    public string XML_LPV_PAYMENT_FOR { get; set; }
    public string XML_LPV_CYCLE { get; set; }
    public int XML_LPV_DIVIDE_BILL { get; set; }
    public string XML_LPV_BILL_LNAME { get; set; }
    public string XML_LPV_BILL_FNAME { get; set; }
    public string XML_LPV_MOR { get; set; }
    public string XML_LPV_METER { get; set; }
    public string XML_LPV_DATE_LSTC { get; set; }
    public string XML_LPV_LSTC_OPERATOR { get; set; }
    public string XML_LPV_DATE_ADD { get; set; }
    public string XML_LPV_ADD_OPERATOR { get; set; }
    public string XML_LPV_VERIFY_DATE { get; set; }
    public string XML_LPV_VERIFIED_BY { get; set; }
    public string XML_LPV_REVERIFY { get; set; }
    public string XML_LPV_ACCOUNT_SWITCH { get; set; }
    public string XML_LPV_FAILED_ACCOUNT_EDIT { get; set; }
    public string XML_LPV_BILLNAME_TYPE { get; set; }

}

public class XML_LPW_TYPE
{
    public string XML_LPW_AGENCY { get; set; }
    public string XML_LPW_DEPT { get; set; }
    public string XML_LPW_PROG { get; set; }
    public string XML_LPW_YEAR { get; set; }
    public string XML_LPW_APP_NO { get; set; }
    public int XML_LPW_SEQ { get; set; }
    public string XML_LPW_BEN_DATE { get; set; }
    public string XML_LPW_BEN_WORKER { get; set; }
    public string XML_LPW_CERTIFIED_STATUS { get; set; }
    public string XML_LPW_REASON_DENIED { get; set; }
    public decimal XML_LPW_FAM_INCOME { get; set; }
    public decimal XML_LPW_FAM_NO_INHH { get; set; }
    public decimal XML_LPW_OMB { get; set; }
    public decimal XML_LPW_SMI { get; set; }
    public string XML_LPW_WAT_VENDOR { get; set; }
    public string XML_LPW_WAT_ACCOUNT_NO { get; set; }
    public string XML_LPW_WAT_BILLNAME_TYPE { get; set; }
    public string XML_LPW_WAT_BILL_LNAME { get; set; }
    public string XML_LPW_WAT_BILL_FNAME { get; set; }
    public string XML_LPW_WAT_INVDATE { get; set; }
    public string XML_LPW_WAT_FREQ_BILL { get; set; }
    public decimal XML_LPW_WAT_ARREARS { get; set; }
    public decimal XML_LPW_WAT_CURBILL_AMT { get; set; }
    public decimal XML_LPW_WAT_FEES { get; set; }
    public decimal XML_LPW_WAT_TOT { get; set; }
    public decimal XML_LPW_WAT_AWARD { get; set; }
    public decimal XML_LPW_WAT_PAID { get; set; }
    public decimal XML_LPW_WAT_AWAITING_DOL { get; set; }
    public string XML_LPW_WAT_BEN_TYPE { get; set; }
    public string XML_LPW_WAT_LEVEL { get; set; }
    public string XML_LPW_WAT_VER_STAT { get; set; }
    public string XML_LPW_WAT_VERIFIED_WORKER { get; set; }
    public string XML_LPW_WAT_VERIFIED_DATE { get; set; }
    public string XML_LPW_WAT_VERIFIED_DATE_LSTC { get; set; }
    public string XML_LPW_WAT_VERIFIED_LSTC_OPERATOR { get; set; }
    public string XML_LPW_SEW_VENDOR { get; set; }
    public string XML_LPW_SEW_ACCOUNT_NO { get; set; }
    public string XML_LPW_SEW_BILLNAME_TYPE { get; set; }
    public string XML_LPW_SEW_BILL_LNAME { get; set; }
    public string XML_LPW_SEW_BILL_FNAME { get; set; }
    public string XML_LPW_SEW_INVDATE { get; set; }
    public string XML_LPW_SEW_FREQ_BILL { get; set; }
    public decimal XML_LPW_SEW_ARREARS { get; set; }
    public decimal XML_LPW_SEW_CURBILL_AMT { get; set; }
    public decimal XML_LPW_SEW_FEES { get; set; }
    public decimal XML_LPW_SEW_TOT { get; set; }
    public decimal XML_LPW_SEW_AWARD { get; set; }
    public decimal XML_LPW_SEW_PAID { get; set; }
    public decimal XML_LPW_SEW_AWAITING_DOL { get; set; }
    public string XML_LPW_SEW_BEN_TYPE { get; set; }
    public string XML_LPW_SEW_LEVEL { get; set; }
    public string XML_LPW_SEW_VER_STAT { get; set; }
    public string XML_LPW_SEW_VERIFIED_WORKER { get; set; }
    public string XML_LPW_SEW_VERIFIED_DATE { get; set; }
    public string XML_LPW_SEW_VERIFIED_DATE_LSTC { get; set; }
    public string XML_LPW_SEW_VERIFIED_LSTC_OPERATOR { get; set; }
    public string XML_LPW_DATE_ADD { get; set; }
    public string XML_LPW_ADD_OPERATOR { get; set; }
    public string XML_LPW_DATE_LSTC { get; set; }
    public string XML_LPW_LSTC_OPERATOR { get; set; }

}


public class XML_IMGUPLOG_TYPE
{
    public int XML_IMGLOG_ID { get; set; }
    public string XML_IMGLOG_AGY { get; set; }
    public string XML_IMGLOG_DEP { get; set; }
    public string XML_IMGLOG_PROG { get; set; }
    public string XML_IMGLOG_YEAR { get; set; }
    public string XML_IMGLOG_APP { get; set; }
    public int XML_IMGLOG_FAMILY_SEQ { get; set; }
    public string XML_IMGLOG_SCREEN { get; set; }
    public string XML_IMGLOG_SECURITY { get; set; }
    public string XML_IMGLOG_TYPE { get; set; }
    public string XML_IMGLOG_UPLoadAs { get; set; }
    public string XML_IMGLOG_UPLOAD_BY { get; set; }
    public string XML_IMGLOG_DATE_UPLOAD { get; set; }
    public string XML_IMGLOG_DELETED_BY { get; set; }
    public string XML_IMGLOG_DATE_DELETED { get; set; }
    public string XML_IMGLOG_DELETED_FROM { get; set; }
    public string XML_IMGLOG_ORIG_FILENAME { get; set; }
    public string XML_IMGLOG_LSTC_OPERATOR { get; set; }
    public string XML_IMGLOG_DATE_LSTC { get; set; }

}

public class DSSLIHEAPEntity
{
    #region Constructors

    public DSSLIHEAPEntity()
    {
        DSSLPV_APPID = string.Empty;
        DSSLPV_SEQ = string.Empty;
        DSSLPV_VEND_CODE = string.Empty;
        DSSLPV_ACCT_NO = string.Empty;
        DSSLPV_PRIM_CODE = string.Empty;
        DSSLPV_PAYMENT_FOR = string.Empty;
        DSSLPV_BILL_LNAME = string.Empty;
        DSSLPV_BILL_FNAME = string.Empty;
        DSSLPV_VEND_NAME = string.Empty;
        DSSLPV_DATE_LSTC = string.Empty;
        DSSLPV_LSTC_OPERATOR = string.Empty;
        DSSLPV_DATE_ADD = string.Empty;
        DSSLPV_ADD_OPERATOR = string.Empty;

        DSSLPV_BILLNAME_TYPE = string.Empty;
        DSSLPV_BYPASS = string.Empty;
        DSSLPV_FAILED_ACCOUNT_EDIT = string.Empty;
    }

    public DSSLIHEAPEntity(DataRow row)
    {
        if (row != null)
        {
            DSSLPV_APPID = row["DSSLPV_APPID"].ToString();
            DSSLPV_SEQ = row["DSSLPV_SEQ"].ToString();
            DSSLPV_VEND_CODE = row["DSSLPV_VEND_CODE"].ToString();
            DSSLPV_ACCT_NO = row["DSSLPV_ACCT_NO"].ToString();
            DSSLPV_PRIM_CODE = row["DSSLPV_PRIM_CODE"].ToString();
            DSSLPV_PAYMENT_FOR = row["DSSLPV_PAYMENT_FOR"].ToString();
            DSSLPV_BILL_LNAME = row["DSSLPV_BILL_LNAME"].ToString();
            DSSLPV_BILL_FNAME = row["DSSLPV_BILL_FNAME"].ToString();
            DSSLPV_VEND_NAME = row["DSSLPV_VEND_NAME"].ToString();
            DSSLPV_DATE_LSTC = row["DSSLPV_DATE_LSTC"].ToString();
            DSSLPV_LSTC_OPERATOR = row["DSSLPV_LSTC_OPERATOR"].ToString();
            DSSLPV_DATE_ADD = row["DSSLPV_DATE_ADD"].ToString();
            DSSLPV_ADD_OPERATOR = row["DSSLPV_ADD_OPERATOR"].ToString();

            DSSLPV_BILLNAME_TYPE = row["DSSLPV_BILLNAME_TYPE"].ToString();

            DSSLPV_BYPASS = row["DSSLPV_BYPASS"].ToString();

        }

    }



    #endregion

    #region Properties

    public string DSSLPV_APPID { get; set; }

    public string DSSLPV_SEQ { get; set; }
    public string DSSLPV_VEND_CODE { get; set; }
    public string DSSLPV_ACCT_NO { get; set; }
    public string DSSLPV_PRIM_CODE { get; set; }
    public string DSSLPV_PAYMENT_FOR { get; set; }
    public string DSSLPV_VEND_NAME { get; set; }

    public string DSSLPV_BILL_LNAME { get; set; }
    public string DSSLPV_BILL_FNAME { get; set; }

    public string DSSLPV_DATE_LSTC { get; set; }
    public string DSSLPV_LSTC_OPERATOR { get; set; }
    public string DSSLPV_DATE_ADD { get; set; }
    public string DSSLPV_ADD_OPERATOR { get; set; }
    public string DSSLPV_BYPASS { get; set; }
    public string Mode { get; set; }

    public string DSSLPV_BILLNAME_TYPE { get; set; }

    public string DSSLPV_FAILED_ACCOUNT_EDIT { get; set; }

    #endregion

}