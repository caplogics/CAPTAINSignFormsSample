using Captain.Common.Model.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Captain.Common.Model.Data
{
    public class PIPDATA
    {


        public static DataTable GetPIPIntakeSearchData(string strConnection, string struserid, string strLcode, string strEmail, string strMode, string strDBName, string strAgency)
        {
            DataTable dt = new DataTable();
            try
            {

                SqlConnection con = new SqlConnection(strConnection);

                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandTimeout = 200;
                    cmd.CommandText = "PIPINTAKE_SEARCH";
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (struserid != string.Empty)
                        cmd.Parameters.AddWithValue("@PIP_REG_ID", struserid);
                    cmd.Parameters.AddWithValue("@PIP_CONFNO", strLcode);

                    if (strEmail != string.Empty)
                        cmd.Parameters.AddWithValue("@PIP_EMAIL", strEmail);
                    cmd.Parameters.AddWithValue("@AGENCY", strDBName);
                    if (strAgency != string.Empty)
                        cmd.Parameters.AddWithValue("@AGY", strAgency);
                    cmd.Parameters.AddWithValue("@Mode", strMode);
                    cmd.Parameters.Add("@Msg", SqlDbType.VarChar, 10);
                    cmd.Parameters["@Msg"].Direction = ParameterDirection.Output;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    da.Fill(dt);


                }
                con.Close();

            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.Message);

            }
            return dt;

        }

        public static DataTable GETPIPINTAKE(string strConnection, string strRegid, string strPIPID, string strconfumn, string strMode, string strDBName, string strAgency)
        {
            DataTable dt = new DataTable();
            try
            {

                SqlConnection con = new SqlConnection(strConnection);

                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandTimeout = 200;
                    cmd.CommandText = "PIPINTAKE_GET";
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (strRegid != string.Empty)
                        cmd.Parameters.AddWithValue("@PIP_REG_ID", strRegid);
                    if (strPIPID != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPID", strPIPID);
                    if (strconfumn != string.Empty)
                        cmd.Parameters.AddWithValue("@PIP_CONFNO", strconfumn);


                    if (strAgency != string.Empty)
                        cmd.Parameters.AddWithValue("@PIP_AGY", strAgency);


                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    da.Fill(dt);


                }
                con.Close();

            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.Message);

            }
            return dt;

        }




        public static DataTable GETPIPCUSTORRESPORADDCUST(string strConnection, string strRespCode, string strAgency, string strAgy, string REGID, string strCustseq, string Mode)
        {

            DataTable dt = new DataTable();
            try
            {

                SqlConnection con = new SqlConnection(strConnection);

                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "PIPCUST_N_RESPS_GET";
                    cmd.CommandTimeout = 200;
                    cmd.CommandType = CommandType.StoredProcedure;


                    if (strRespCode != string.Empty)
                        cmd.Parameters.AddWithValue("@RESPCODE", strRespCode);
                    if (strAgency != string.Empty)
                        cmd.Parameters.AddWithValue("@AGENCY", strAgency);
                    if (strAgy != string.Empty)
                        cmd.Parameters.AddWithValue("@AGY", strAgy);
                    if (REGID != string.Empty)
                        cmd.Parameters.AddWithValue("@REGID", REGID);
                    if (strCustseq != string.Empty)
                        cmd.Parameters.AddWithValue("@ADDCUST_SEQ", strCustseq);

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

        public static List<CustomQuestionsEntity> GETPIPADDCUSTList(string strConnection, string strRespCode, string strAgency, string strAgy, string REGID, string strCustseq, string Mode)
        {
            List<CustomQuestionsEntity> pipCustomQuestions = new List<CustomQuestionsEntity>();


            DataTable dt = new DataTable();
            try
            {

                SqlConnection con = new SqlConnection(strConnection);

                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "PIPCUST_N_RESPS_GET";
                    cmd.CommandTimeout = 200;
                    cmd.CommandType = CommandType.StoredProcedure;


                    if (strRespCode != string.Empty)
                        cmd.Parameters.AddWithValue("@RESPCODE", strRespCode);
                    if (strAgency != string.Empty)
                        cmd.Parameters.AddWithValue("@AGENCY", strAgency);
                    if (strAgy != string.Empty)
                        cmd.Parameters.AddWithValue("@AGY", strAgy);
                    if (REGID != string.Empty)
                        cmd.Parameters.AddWithValue("@REGID", REGID);
                    if (strCustseq != string.Empty)
                        cmd.Parameters.AddWithValue("@ADDCUST_SEQ", strCustseq);

                    if (Mode != string.Empty)
                        cmd.Parameters.AddWithValue("@MODE", Mode);


                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    da.Fill(dt);

                    foreach (DataRow dtitem in dt.Rows)
                    {
                        pipCustomQuestions.Add(new CustomQuestionsEntity(dtitem, "PIPQUESTIONS"));
                    }

                }
                con.Close();

            }
            catch (Exception ex)
            {


            }
            return pipCustomQuestions;

        }

        public static DataTable GETPIPSERVICES(string strConnection, string strServiceoption, string strShortName, string strAgy)
        {

            DataTable dt = new DataTable();
            try
            {

                SqlConnection con = new SqlConnection(strConnection);

                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "PIPSERVICES_GET";
                    cmd.CommandTimeout = 200;
                    cmd.CommandType = CommandType.StoredProcedure;


                    if (strServiceoption != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPSER_TYPE", strServiceoption);
                    if (strShortName != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPSER_AGENCY", strShortName);
                    if (strAgy != string.Empty)
                        cmd.Parameters.AddWithValue("@PSH_AGY", strAgy);
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

        public static string CheckorDeletePIPIntakeData(string strConnection, string strRegId, string strLcode, string strEmail, string strMode, string strDBName, string strAgy)
        {
            string strLocstatus = string.Empty;
            try
            {


                SqlConnection con = new SqlConnection(strConnection);

                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandTimeout = 200;
                    cmd.CommandText = "PIPINTAKE_SEARCH";
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (strRegId != string.Empty)
                        cmd.Parameters.AddWithValue("@PIP_REG_ID", strRegId);
                    if (strLcode != string.Empty)
                        cmd.Parameters.AddWithValue("@PIP_CONFNO", strLcode);
                    if (strEmail != string.Empty)
                        cmd.Parameters.AddWithValue("@PIP_EMAIL", strEmail);
                    cmd.Parameters.AddWithValue("@AGENCY", strDBName);
                    cmd.Parameters.AddWithValue("@AGY", strAgy);
                    cmd.Parameters.AddWithValue("@Mode", strMode);
                    cmd.Parameters.Add("@Msg", SqlDbType.VarChar, 100);
                    cmd.Parameters["@Msg"].Direction = ParameterDirection.Output;

                    int i = cmd.ExecuteNonQuery();

                    // Start getting the RetrunValue and Output from Stored Procedure
                    strLocstatus = cmd.Parameters["@Msg"].Value.ToString();
                    if (strLocstatus == string.Empty)
                    {
                        if (i > 0)
                        {
                            strLocstatus = "Success";
                        }
                    }
                    //if (IntReturn == 1)
                    //{
                    //    outputParam = cmd.Parameters["@OutputParam"].Value;
                    //    inoutParam = cmd.Parameters["@InputOutputParam"].Value;
                    //}

                }
                con.Close();
            }
            catch (Exception ex)
            {

                //MessageBox.Show(ex.Message);
            }
            return strLocstatus;
        }

        public static string DELNotverfRegUSERS(string strConnection, string strRegId, string UserID, string strMode)
        {
            string strLocstatus = string.Empty;
            try
            {


                SqlConnection con = new SqlConnection(strConnection);

                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandTimeout = 200;
                    cmd.CommandText = "PIPINTAKE_SEARCH";
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (strRegId != string.Empty)
                        cmd.Parameters.AddWithValue("@PIP_REG_ID", strRegId);
                    if (UserID != string.Empty)
                        cmd.Parameters.AddWithValue("@USERID", UserID);
                    cmd.Parameters.AddWithValue("@Mode", strMode);
                    cmd.Parameters.Add("@Msg", SqlDbType.VarChar, 100);
                    cmd.Parameters["@Msg"].Direction = ParameterDirection.Output;

                    int i = cmd.ExecuteNonQuery();

                    // Start getting the RetrunValue and Output from Stored Procedure
                    strLocstatus = cmd.Parameters["@Msg"].Value.ToString();
                    if (strLocstatus == string.Empty)
                    {
                        if (i > 0)
                        {
                            strLocstatus = "Success";
                        }
                    }
                    //if (IntReturn == 1)
                    //{
                    //    outputParam = cmd.Parameters["@OutputParam"].Value;
                    //    inoutParam = cmd.Parameters["@InputOutputParam"].Value;
                    //}

                }
                con.Close();
            }
            catch (Exception ex)
            {

                //MessageBox.Show(ex.Message);
            }
            return strLocstatus;
        }
        public static List<PIPDocEntity> GetPIPDOCUPLOADS(string strConnection, string strPIPID, string strAGY, string strAgency, string strDOCTYPS, string strREGID, string Mode)
        {
            List<PIPDocEntity> pipdocmentdata = new List<PIPDocEntity>();


            DataTable dt = new DataTable();
            try
            {

                SqlConnection con = new SqlConnection(strConnection);

                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "PIPDOCUPLOADS_GET";
                    cmd.CommandTimeout = 200;
                    cmd.CommandType = CommandType.StoredProcedure;


                    if (strPIPID != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPDOCUPLD_PIP_ID", strPIPID);
                    if (strAGY != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPDOCUPLD_AGY", strAGY);
                    if (strAgency != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPDOCUPLD_AGENCY", strAgency);
                    if (strDOCTYPS != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPDOCUPLD_DOCTYPE", strDOCTYPS);


                    if (strREGID != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPDOCUPLD_REG_ID", strREGID);


                    if (Mode != string.Empty)
                        cmd.Parameters.AddWithValue("@MODE", Mode);


                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    da.Fill(dt);

                    foreach (DataRow dtitem in dt.Rows)
                    {
                        pipdocmentdata.Add(new Objects.PIPDocEntity(dtitem));
                    }

                }
                con.Close();

            }
            catch (Exception ex)
            {


            }
            return pipdocmentdata;

        }

        public static DataTable GetPIPDOCUPLOADS(string strConnection, string strPIPID, string strAGY, string strAgency, string strDOCTYPS, string strREGID, string strFromdate, string strTodate, string strDateType, string Mode)
        {



            DataTable dt = new DataTable();
            try
            {

                SqlConnection con = new SqlConnection(strConnection);

                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "PIPDOCUPLOADS_GET";
                    cmd.CommandTimeout = 200;
                    cmd.CommandType = CommandType.StoredProcedure;


                    if (strPIPID != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPDOCUPLD_PIP_ID", strPIPID);
                    if (strAGY != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPDOCUPLD_AGY", strAGY);
                    if (strAgency != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPDOCUPLD_AGENCY", strAgency);
                    if (strDOCTYPS != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPDOCUPLD_DOCTYPE", strDOCTYPS);
                    if (strFromdate != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPDOCFROM_DATE", strFromdate);
                    if (strTodate != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPDOCTO_DATE", strTodate);

                    if (strDateType != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPDATETYPE", strDateType);

                    if (strREGID != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPDOCUPLD_REG_ID", strREGID);

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



        public static int InsertUpdatePipDoc(string strConnection, string strPIPDOCID, string strVerCode, string strVerfierBy, string strRemarks, string strMode)
        {
            int i = 0;
            try
            {


                SqlConnection con = new SqlConnection(strConnection);

                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandTimeout = 200;
                    cmd.CommandText = "PIPDOCUPLOADS_INSUPDEL";
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (strPIPDOCID != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPDOCUPLD_ID", strPIPDOCID);
                    if (strVerCode != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPDOCUPLD_VERIFIED_STAT", strVerCode);
                    if (strVerfierBy != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPDOCUPLD_VERIFIED_BY", strVerfierBy);
                    if (strRemarks != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPDOCUPLD_REMARKS", strRemarks);
                    cmd.Parameters.AddWithValue("@Mode", strMode);

                    i = cmd.ExecuteNonQuery();



                }
                con.Close();
            }
            catch (Exception ex)
            {

                //MessageBox.Show(ex.Message);
            }
            return i;
        }

        public static int InsertUpdatePipDocVer(string strConnection, string strPIPVERID, string strPIPDOCID, string strVerCode, string strVerfierBy, string strRemarks, string strMode, string strPIPID, string strREGID)
        {
            int i = 0;
            try
            {


                SqlConnection con = new SqlConnection(strConnection);

                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandTimeout = 200;
                    cmd.CommandText = "PIPDOCVER_INSUPDDEL";
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (strPIPVERID != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPDOCVER_ID", strPIPVERID);
                    if (strREGID != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPDOCVER_REG_ID", strREGID);
                    if (strPIPID != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPDOCVER_PIP_ID", strPIPID);
                    if (strPIPDOCID != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPDOCVER_DOC_ID", strPIPDOCID);
                    if (strVerCode != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPDOCVER_STATUS", strVerCode);
                    if (strVerfierBy != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPDOCVER_BY", strVerfierBy);
                    if (strRemarks != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPDOCVER_REMARKS", strRemarks);
                    cmd.Parameters.AddWithValue("@Mode", strMode);

                    i = cmd.ExecuteNonQuery();



                }
                con.Close();
            }
            catch (Exception ex)
            {

                //MessageBox.Show(ex.Message);
            }
            return i;
        }


        public static DataTable GETPIPREG(string strConnection, string strPassword, string strEmail, string strRegid, string strAgency, string strAgy, string strActive, string strMode)
        {

            DataTable dt = new DataTable();
            try
            {

                SqlConnection con = new SqlConnection(strConnection);

                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "PIPREG_GET";
                    cmd.CommandTimeout = 200;
                    cmd.CommandType = CommandType.StoredProcedure;


                    if (strPassword != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPREG_PASSWORD", strPassword);
                    if (strEmail != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPREG_EMAIL", strEmail);
                    if (strRegid != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPREG_ID", strRegid);
                    if (strAgency != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPREG_AGENCY", strAgency);
                    if (strAgy != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPREG_AGY", strAgy);
                    if (strActive != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPREG_ACTIVE", strActive);
                    if (strMode != string.Empty)
                        cmd.Parameters.AddWithValue("@MODE", strMode);
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

        public static DataTable PIPMAILS_GET(string strConnection, string strAgency, string strAgy, string strPurpose, string strMode)
        {

            DataTable dt = new DataTable();
            try
            {

                SqlConnection con = new SqlConnection(strConnection);

                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "PIPMAILS_GET";
                    cmd.CommandTimeout = 200;
                    cmd.CommandType = CommandType.StoredProcedure;



                    if (strAgency != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPMAIL_AGENCY", strAgency);
                    if (strAgy != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPMAIL_AGY", strAgy);
                    if (strPurpose != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPMAIL_PURPOSE", strPurpose);
                    if (strMode != string.Empty)
                        cmd.Parameters.AddWithValue("@MODE", strMode);
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

        public static DataTable PIPMAILCONFIG_GET(string strConnection, string strType)
        {

            DataTable dt = new DataTable();
            try
            {

                SqlConnection con = new SqlConnection(strConnection);

                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {

                    cmd.Connection = con;
                    cmd.CommandText = "SELECT * FROM PIPMAILCONFIG WHERE MAILCONFIG_TYPE = '" + strType + "'";
                    cmd.CommandTimeout = 200;
                    cmd.CommandType = CommandType.Text;

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


        public static string[] PIPAGENCY_GET(string strConnection, string Agency, string Agy)
        {
            DataTable dt = new DataTable();
            string[] DBName = new string[9];
            try
            {
                SqlConnection con = new SqlConnection(strConnection);

                con.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "SELECT *  FROM PIPAGENCY WHERE PIPAGY_AGENCY = '" + Agency + "' AND PIPAGY_AGY = '" + Agy + "'";
                    cmd.CommandTimeout = 200;
                    cmd.CommandType = CommandType.Text;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        DBName[0] = dt.Rows[0]["PIPAGY_AGENCY"].ToString();
                        DBName[1] = dt.Rows[0]["PIPAGY_AGY"].ToString();
                        DBName[2] = dt.Rows[0]["PIPAGY_DESC"].ToString();
                        DBName[3] = dt.Rows[0]["PIPAGY_CSS_STYLE"].ToString();
                        DBName[4] = dt.Rows[0]["PIPAGY_STATUS"].ToString();
                        DBName[5] = dt.Rows[0]["PIPAGY_PNL1_CSS_STYLE"].ToString();

                        DBName[6] = dt.Rows[0]["PIPAGY_PRIMARY_CSS_STYLE"].ToString();
                        DBName[7] = dt.Rows[0]["PIPAGY_ACCOUNT_CSS_STYLE"].ToString();
                        DBName[8] = dt.Rows[0]["PIPAGY_SHORT"].ToString();
                    }
                }

            }
            catch (Exception ex)
            {


            }
            return DBName;
        }


        public static int InsertUpdatePIPEmailHistory(string strConnection, string strMailid, string strRegid, string strPIPDOCID, string strPIPDOCVERID, string strEmailSendBy, string strMode, string strContentMessage)
        {
            int i = 0;
            try
            {


                SqlConnection con = new SqlConnection(strConnection);

                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandTimeout = 200;
                    cmd.CommandText = "PIPEMAILHIST_INSUPDEL";
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (strMailid != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPMAILH_ID", strMailid);

                    if (strRegid != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPMAILH_REG_ID", strRegid);
                    if (strPIPDOCID != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPMAILH_UPD_ID", strPIPDOCID);
                    if (strPIPDOCVERID != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPMAILH_VER_ID", strPIPDOCVERID);
                    if (strContentMessage != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPMAILH_MISC_MSG", strContentMessage);
                    if (strEmailSendBy != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPMAILH_SENT_BY", strEmailSendBy);
                    if (strMode != string.Empty)
                        cmd.Parameters.AddWithValue("@Mode", strMode);


                    i = cmd.ExecuteNonQuery();



                }
                con.Close();
            }
            catch (Exception ex)
            {

                //MessageBox.Show(ex.Message);
            }
            return i;
        }

        public static DataTable PIPEMAILHIST(string strConnection, string strRegid)
        {

            DataTable dt = new DataTable();
            try
            {

                SqlConnection con = new SqlConnection(strConnection);

                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "SELECT * FROM PIPEMAILHIST WHERE PIPMAILH_REG_ID = " + strRegid;
                    cmd.CommandTimeout = 200;
                    cmd.CommandType = CommandType.Text;

                    //if (strAgency != string.Empty)
                    //    cmd.Parameters.AddWithValue("@PIPMAIL_AGENCY", strAgency);
                    //if (strAgy != string.Empty)
                    //    cmd.Parameters.AddWithValue("@PIPMAIL_AGY", strAgy);
                    //if (strPurpose != string.Empty)
                    //    cmd.Parameters.AddWithValue("@PIPMAIL_PURPOSE", strPurpose);
                    //if (strMode != string.Empty)
                    //    cmd.Parameters.AddWithValue("@MODE", strMode);
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

        public static int InsertUpdatePIPCAPLNK(string strConnection, string strAgency, string strDept, string strProg, string strYear, string strApplNo, string strFamseq, string strPIPAGENCY, string strPIPAGy, string strPIPREGID, string strPIPID, string strDraggedBy, string strLnkTYPE, string strMode)
        {
            int i = 0;
            try
            {


                SqlConnection con = new SqlConnection(strConnection);

                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandTimeout = 200;
                    cmd.CommandText = "PIPCAPLNK_INSUPDEL";
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (strAgency != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPCAPLNK_AGENCY", strAgency);
                    if (strDept != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPCAPLNK_DEPT", strDept);
                    if (strProg != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPCAPLNK_PROGRAM", strProg);
                    if (strYear != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPCAPLNK_YEAR", strYear);
                    if (strApplNo != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPCAPLNK_APP", strApplNo);
                    if (strFamseq != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPCAPLNK_FAMILY_SEQ", strFamseq);
                    if (strPIPAGENCY != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPCAPLNK_PIP_AGENCY", strPIPAGENCY);
                    if (strPIPAGy != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPCAPLNK_PIP_AGY", strPIPAGy);
                    if (strPIPREGID != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPCAPLNK_REGID", strPIPREGID);
                    if (strPIPID != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPCAPLNK_PIP_ID", strPIPID);
                    if (strDraggedBy != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPCAPLNK_DRAGGED_BY", strDraggedBy);
                    if (strLnkTYPE != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPCAPLNK_TYPE", strLnkTYPE);
                    if (strMode != string.Empty)
                        cmd.Parameters.AddWithValue("@Mode", strMode);


                    i = cmd.ExecuteNonQuery();



                }
                con.Close();
            }
            catch (Exception ex)
            {

                //MessageBox.Show(ex.Message);
            }
            return i;
        }

        public static List<PIPCAPLNK> GetPIPCAPLNK(string strConnection, string strAgency, string strDept, string strProg, string strYear, string strApplNo, string strAgyshorName, string strPIPAgy, string strRegId)
        {
            List<PIPCAPLNK> pipcaplnkdata = new List<PIPCAPLNK>();


            DataTable dt = new DataTable();
            try
            {

                SqlConnection con = new SqlConnection(strConnection);

                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "PIPCAPLNK_GET";
                    cmd.CommandTimeout = 200;
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (strAgency != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPCAPLNK_AGENCY", strAgency);
                    if (strDept != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPCAPLNK_DEPT", strDept);
                    if (strProg != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPCAPLNK_PROGRAM", strProg);
                    if (strYear != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPCAPLNK_YEAR", strYear);
                    if (strApplNo != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPCAPLNK_APP", strApplNo);
                    if (strAgyshorName != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPCAPLNK_PIP_AGENCY", strAgyshorName);
                    if (strPIPAgy != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPCAPLNK_PIP_AGY", strPIPAgy);
                    if (strRegId != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPCAPLNK_REGID", strRegId);


                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    da.Fill(dt);

                    foreach (DataRow dtitem in dt.Rows)
                    {
                        pipcaplnkdata.Add(new Objects.PIPCAPLNK(dtitem));
                    }

                }
                con.Close();

            }
            catch (Exception ex)
            {


            }
            return pipcaplnkdata;

        }

        public static List<PIPDocVerEntity> GETPIPDOCVER(string strConnection, string strPIPUPDVERID, string strPIPUPDID, string strMode)
        {
            List<PIPDocVerEntity> pipdocverdata = new List<PIPDocVerEntity>();


            DataTable dt = new DataTable();
            try
            {

                SqlConnection con = new SqlConnection(strConnection);

                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "PIPDOCVER_GET";
                    cmd.CommandTimeout = 200;
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (strPIPUPDVERID != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPDOCVER_ID", strPIPUPDVERID);
                    if (strPIPUPDID != string.Empty)
                        cmd.Parameters.AddWithValue("@PIPDOCVER_DOC_ID", strPIPUPDID);
                    if (strMode != string.Empty)
                        cmd.Parameters.AddWithValue("@Mode", strMode);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    da.Fill(dt);

                    foreach (DataRow dtitem in dt.Rows)
                    {
                        pipdocverdata.Add(new Objects.PIPDocVerEntity(dtitem));
                    }

                }
                con.Close();

            }
            catch (Exception ex)
            {


            }
            return pipdocverdata;

        }

        public static DataTable GETPIPINTAKEDATABYDATE(string strconn, string FromDate, string Todate, string Agency, string AGY, string strMode, string strProcess)
        {

            DataTable dt = new DataTable();
            try
            {

                SqlConnection con = new SqlConnection(strconn);

                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "PIPINTAKE_GETBYDATE";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 6000;

                    if (FromDate != string.Empty)
                        cmd.Parameters.AddWithValue("@FROMDATE", FromDate);
                    if (Todate != string.Empty)
                        cmd.Parameters.AddWithValue("@TODATE", Todate);
                    if (Agency != string.Empty)
                        cmd.Parameters.AddWithValue("@Agency", Agency);

                    if (AGY != string.Empty)
                        cmd.Parameters.AddWithValue("@Agy", AGY);

                    if (strMode != string.Empty)
                        cmd.Parameters.AddWithValue("@Mode", strMode);

                    if (strProcess != string.Empty)
                        cmd.Parameters.AddWithValue("@Process", strProcess);

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


        public static int InsertUpdatePipCustomQuestions(string strConnection, string strAgency, string strCustCode, string strActive, string strDesc, string strSeq, string strRespcode, string strLstcoperator, string strMode, string strectype)
        {
            int i = 0;
            try
            {


                SqlConnection con = new SqlConnection(strConnection);

                con.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandTimeout = 200;
                    cmd.CommandText = "PIPCUSTQST_INSUPDEL";
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (strAgency != string.Empty)
                        cmd.Parameters.AddWithValue("@PCUST_AGENCY", strAgency);
                    if (strCustCode != string.Empty)
                        cmd.Parameters.AddWithValue("@PCUST_CODE", strCustCode);
                    if (strActive != string.Empty)
                        cmd.Parameters.AddWithValue("@PCUST_ACTIVE", strActive);
                    if (strDesc != string.Empty)
                        cmd.Parameters.AddWithValue("@PCUST_DESC", strDesc);
                    if (strSeq != string.Empty)
                        cmd.Parameters.AddWithValue("@PRSP_SEQ", strSeq);
                    if (strRespcode != string.Empty)
                        cmd.Parameters.AddWithValue("@PRSP_RESP_CODE", strRespcode);
                    if (strLstcoperator != string.Empty)
                        cmd.Parameters.AddWithValue("@PRSP_LSTC_OPERATOR", strLstcoperator);
                    cmd.Parameters.AddWithValue("@Mode", strMode);
                    if (strectype != string.Empty)
                        cmd.Parameters.AddWithValue("@RecType", strectype);
                    i = cmd.ExecuteNonQuery();



                }
                con.Close();
            }
            catch (Exception ex)
            {

                //MessageBox.Show(ex.Message);
            }
            return i;
        }



    }

    public class NetworkShareAccesser : IDisposable
    {
        private string _remoteUncName;
        private string _remoteComputerName;

        public string RemoteComputerName
        {
            get
            {
                return this._remoteComputerName;
            }
            set
            {
                this._remoteComputerName = value;
                this._remoteUncName = @"\\" + this._remoteComputerName;
            }
        }

        public string UserName
        {
            get;
            set;
        }
        public string Password
        {
            get;
            set;
        }

        #region Consts

        private const int RESOURCE_CONNECTED = 0x00000001;
        private const int RESOURCE_GLOBALNET = 0x00000002;
        private const int RESOURCE_REMEMBERED = 0x00000003;

        private const int RESOURCETYPE_ANY = 0x00000000;
        private const int RESOURCETYPE_DISK = 0x00000001;
        private const int RESOURCETYPE_PRINT = 0x00000002;

        private const int RESOURCEDISPLAYTYPE_GENERIC = 0x00000000;
        private const int RESOURCEDISPLAYTYPE_DOMAIN = 0x00000001;
        private const int RESOURCEDISPLAYTYPE_SERVER = 0x00000002;
        private const int RESOURCEDISPLAYTYPE_SHARE = 0x00000003;
        private const int RESOURCEDISPLAYTYPE_FILE = 0x00000004;
        private const int RESOURCEDISPLAYTYPE_GROUP = 0x00000005;

        private const int RESOURCEUSAGE_CONNECTABLE = 0x00000001;
        private const int RESOURCEUSAGE_CONTAINER = 0x00000002;


        private const int CONNECT_INTERACTIVE = 0x00000008;
        private const int CONNECT_PROMPT = 0x00000010;
        private const int CONNECT_REDIRECT = 0x00000080;
        private const int CONNECT_UPDATE_PROFILE = 0x00000001;
        private const int CONNECT_COMMANDLINE = 0x00000800;
        private const int CONNECT_CMD_SAVECRED = 0x00001000;

        private const int CONNECT_LOCALDRIVE = 0x00000100;

        #endregion

        #region Errors

        private const int NO_ERROR = 0;

        private const int ERROR_ACCESS_DENIED = 5;
        private const int ERROR_ALREADY_ASSIGNED = 85;
        private const int ERROR_BAD_DEVICE = 1200;
        private const int ERROR_BAD_NET_NAME = 67;
        private const int ERROR_BAD_PROVIDER = 1204;
        private const int ERROR_CANCELLED = 1223;
        private const int ERROR_EXTENDED_ERROR = 1208;
        private const int ERROR_INVALID_ADDRESS = 487;
        private const int ERROR_INVALID_PARAMETER = 87;
        private const int ERROR_INVALID_PASSWORD = 1216;
        private const int ERROR_MORE_DATA = 234;
        private const int ERROR_NO_MORE_ITEMS = 259;
        private const int ERROR_NO_NET_OR_BAD_PATH = 1203;
        private const int ERROR_NO_NETWORK = 1222;

        private const int ERROR_BAD_PROFILE = 1206;
        private const int ERROR_CANNOT_OPEN_PROFILE = 1205;
        private const int ERROR_DEVICE_IN_USE = 2404;
        private const int ERROR_NOT_CONNECTED = 2250;
        private const int ERROR_OPEN_FILES = 2401;

        #endregion

        #region PInvoke Signatures

        [DllImport("Mpr.dll")]
        private static extern int WNetUseConnection(
            IntPtr hwndOwner,
            NETRESOURCE lpNetResource,
            string lpPassword,
            string lpUserID,
            int dwFlags,
            string lpAccessName,
            string lpBufferSize,
            string lpResult
            );

        [DllImport("Mpr.dll")]
        private static extern int WNetCancelConnection2(
            string lpName,
            int dwFlags,
            bool fForce
            );

        [StructLayout(LayoutKind.Sequential)]
        private class NETRESOURCE
        {
            public int dwScope = 0;
            public int dwType = 0;
            public int dwDisplayType = 0;
            public int dwUsage = 0;
            public string lpLocalName = "";
            public string lpRemoteName = "";
            public string lpComment = "";
            public string lpProvider = "";
        }

        #endregion

        /// <summary>
        /// Creates a NetworkShareAccesser for the given computer name. The user will be promted to enter credentials
        /// </summary>
        /// <param name="remoteComputerName"></param>
        /// <returns></returns>
        public static NetworkShareAccesser Access(string remoteComputerName)
        {
            return new NetworkShareAccesser(remoteComputerName);
        }

        /// <summary>
        /// Creates a NetworkShareAccesser for the given computer name using the given domain/computer name, username and password
        /// </summary>
        /// <param name="remoteComputerName"></param>
        /// <param name="domainOrComuterName"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public static NetworkShareAccesser Access(string remoteComputerName, string domainOrComuterName, string userName, string password)
        {
            return new NetworkShareAccesser(remoteComputerName,
                                            domainOrComuterName + @"\" + userName,
                                            password);
        }

        /// <summary>
        /// Creates a NetworkShareAccesser for the given computer name using the given username (format: domainOrComputername\Username) and password
        /// </summary>
        /// <param name="remoteComputerName"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public static NetworkShareAccesser Access(string remoteComputerName, string userName, string password)
        {
            return new NetworkShareAccesser(remoteComputerName,
                                            userName,
                                            password);
        }

        private NetworkShareAccesser(string remoteComputerName)
        {
            RemoteComputerName = remoteComputerName;

            this.ConnectToShare(this._remoteUncName, null, null, true);
        }

        private NetworkShareAccesser(string remoteComputerName, string userName, string password)
        {
            RemoteComputerName = remoteComputerName;
            UserName = userName;
            Password = password;

            this.ConnectToShare(this._remoteUncName, this.UserName, this.Password, false);
        }

        private void ConnectToShare(string remoteUnc, string username, string password, bool promptUser)
        {
            NETRESOURCE nr = new NETRESOURCE
            {
                dwType = RESOURCETYPE_DISK,
                lpRemoteName = remoteUnc
            };

            int result;
            if (promptUser)
            {
                result = WNetUseConnection(IntPtr.Zero, nr, "", "", CONNECT_INTERACTIVE | CONNECT_PROMPT, null, null, null);
            }
            else
            {
                result = WNetUseConnection(IntPtr.Zero, nr, password, username, 0, null, null, null);
            }

            if (result != NO_ERROR)
            {
                throw new Win32Exception(result);
            }
        }

        private void DisconnectFromShare(string remoteUnc)
        {
            int result = WNetCancelConnection2(remoteUnc, CONNECT_UPDATE_PROFILE, false);
            if (result != NO_ERROR)
            {
                throw new Win32Exception(result);
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            this.DisconnectFromShare(this._remoteUncName);
        }
    }

}
