using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Captain.DatabaseLayer;
using Captain.Common.Model.Objects;
using System.Data.SqlClient;
using System.Data;


namespace Captain.Common.Model.Data
{
    public class Agytab
    {

        public Agytab()
        {

        }

        #region Properties

        public CaptainModel Model { get; set; }

        #endregion

        public List<AgyTabEntity> GetSelAgyChildDetails(string SelAgyType, string selChildCode)
        {
            List<AgyTabEntity> AgytabProfile = new List<AgyTabEntity>();
            try
            {
                DataSet AgytabData = AgyTab.GetSelAgyChildDetails(SelAgyType, selChildCode);
                if (AgytabData != null && AgytabData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in AgytabData.Tables[0].Rows)
                    {
                        AgytabProfile.Add(new AgyTabEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return AgytabProfile;
            }

            return AgytabProfile;
        }

        public List<AgyTabEntity> GetAgyTab(string SelAgyType)
        {
            List<AgyTabEntity> AgytabProfile = new List<AgyTabEntity>();
            try
            {
                DataSet AgytabData = AgyTab.GetAgyTab(SelAgyType);
                if (AgytabData != null && AgytabData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in AgytabData.Tables[0].Rows)
                    {
                        AgytabProfile.Add(new AgyTabEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return AgytabProfile;
            }

            return AgytabProfile;
        }

        public List<AgyTabEntity> GetAgencyTableCodes(string agencyType)
        {
            List<AgyTabEntity> AgytabProfile = new List<AgyTabEntity>();
            try
            {
                DataSet AgytabData = AgyTab.GetAgyTabDetails(agencyType);
                if (AgytabData != null && AgytabData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in AgytabData.Tables[0].Rows)
                    {
                        AgytabProfile.Add(new AgyTabEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return AgytabProfile;
            }

            return AgytabProfile;
        }

        public bool UpdateAGYTAB(AgyTabEntity AgyTab)
        {
            bool boolsuccess;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();  
                if(AgyTab.agyRowtype!=string.Empty)
                    sqlParamList.Add(new SqlParameter("@Row_Type", AgyTab.agyRowtype)); 
                if(AgyTab.agytype!=string.Empty)
                sqlParamList.Add(new SqlParameter("@AGY_TYPE", AgyTab.agytype));
                if (AgyTab.agycode != string.Empty)
                sqlParamList.Add(new SqlParameter("@AGY_CODE", AgyTab.agycode));

                if (AgyTab.agy1 != string.Empty)
                sqlParamList.Add(new SqlParameter("@AGY_1", AgyTab.agy1));
                if (AgyTab.agy2 != string.Empty)
                sqlParamList.Add(new SqlParameter("@AGY_2", AgyTab.agy2));
                if (AgyTab.agy3 != string.Empty)
                sqlParamList.Add(new SqlParameter("@AGY_3", AgyTab.agy3));
                if (AgyTab.agy4 != string.Empty)
                sqlParamList.Add(new SqlParameter("@AGY_4", AgyTab.agy4));
                if (AgyTab.agy5 != string.Empty)
                sqlParamList.Add(new SqlParameter("@AGY_5", AgyTab.agy5));
                if (AgyTab.agy6 != string.Empty)
                sqlParamList.Add(new SqlParameter("@AGY_6", AgyTab.agy6));
                if (AgyTab.agy7 != string.Empty)
                sqlParamList.Add(new SqlParameter("@AGY_7", AgyTab.agy7));
                if (AgyTab.agy8 != string.Empty)
                sqlParamList.Add(new SqlParameter("@AGY_8", AgyTab.agy8));
                if (AgyTab.agy9 != string.Empty)
                sqlParamList.Add(new SqlParameter("@AGY_9", AgyTab.agy9));


                //sqlParamList.Add(new SqlParameter("@AGY_A1", SqlDbType.Decimal, 11, AgyTab.agya1));
                //sqlParamList.Add(new SqlParameter("@AGY_A2", SqlDbType.Decimal, 11, AgyTab.agya2));
                //sqlParamList.Add(new SqlParameter("@AGY_A3", SqlDbType.Decimal, 11, AgyTab.agya3));
                //sqlParamList.Add(new SqlParameter("@AGY_A4", SqlDbType.Decimal, 11, AgyTab.agya4));

                if (AgyTab.agya1 != string.Empty)
                sqlParamList.Add(new SqlParameter("@AGY_A1", AgyTab.agya1));
                if (AgyTab.agya2 != string.Empty)
                sqlParamList.Add(new SqlParameter("@AGY_A2", AgyTab.agya2));
                if (AgyTab.agya3 != string.Empty)
                sqlParamList.Add(new SqlParameter("@AGY_A3", AgyTab.agya3));
                if (AgyTab.agya4 != string.Empty)
                sqlParamList.Add(new SqlParameter("@AGY_A4", AgyTab.agya4));

                if (AgyTab.agydesc != string.Empty)
                sqlParamList.Add(new SqlParameter("@AGY_DESC", AgyTab.agydesc));
                if (AgyTab.agyactive != string.Empty)
                sqlParamList.Add(new SqlParameter("@AGY_ACTIVE", AgyTab.agyactive));
                if (AgyTab.agydefault != string.Empty)
                sqlParamList.Add(new SqlParameter("@AGY_DEFAULT", AgyTab.agydefault));
                if (AgyTab.agyhierarchy != string.Empty)
                sqlParamList.Add(new SqlParameter("@AGY_HIERARCHY", AgyTab.agyhierarchy));
               
                sqlParamList.Add(new SqlParameter("@AGY_LSTC_OPERATOR", AgyTab.agylstcoperator));               
                sqlParamList.Add(new SqlParameter("@AGY_ADD_OPERATOR", AgyTab.agyaddoperator));
                boolsuccess = Captain.DatabaseLayer.AgyTab.UpdateAGYTAB(sqlParamList);
                //boolsuccess = AgyTab.UpdateAGYTAB(sqlParamList);
            }
            catch (Exception ex)
            {
                //
                return false;
            }

            return boolsuccess;
        }
      

        public bool DeleteAGYTAB(string type, string code)
        {
            try
            {
                Captain.DatabaseLayer.AgyTab.DeleteAGYTAB(type, code);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public bool InsertUpdateAGYTABS(string strRowType,string strType,string strCode,string strDesc,string strActive,string strDefault,string strHie,string strSDesc)
        {
            bool boolsuccess;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                if (strRowType != string.Empty)
                    sqlParamList.Add(new SqlParameter("@Row_Type", strRowType));
                if (strType != string.Empty)
                    sqlParamList.Add(new SqlParameter("@AGYS_TYPE", strType));
                if (strCode != string.Empty)
                    sqlParamList.Add(new SqlParameter("@AGYS_CODE", strCode));

                if (strDesc != string.Empty)
                    sqlParamList.Add(new SqlParameter("@AGYS_DESC", strDesc));
                if (strActive != string.Empty)
                    sqlParamList.Add(new SqlParameter("@AGYS_ACTIVE", strActive));
                if (strDefault != string.Empty)
                    sqlParamList.Add(new SqlParameter("@AGYS_DEFAULT", strDefault));
                if (strHie != string.Empty)
                    sqlParamList.Add(new SqlParameter("@AGYS_HIERARCHY", strHie));
                if (strSDesc != string.Empty)
                    sqlParamList.Add(new SqlParameter("@AGYS_SDESC", strSDesc));
                boolsuccess = Captain.DatabaseLayer.AgyTab.InsertUpdateAGYTABS(sqlParamList);
               
            }
            catch (Exception ex)
            {
                //
                return false;
            }

            return boolsuccess;
        }

        public bool DeleteAGYTABSingle(string type, string code,string seq)
        {
            try
            {
                Captain.DatabaseLayer.AgyTab.DeleteAGYTABSingle(type, code,seq);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }


        public List<CommonEntity> GetSystemDBS(string strType,string strDB)
        {
            List<CommonEntity> commonEntity = new List<CommonEntity>();
            try
            {
                DataSet CapAppl = Captain.DatabaseLayer.AgyTab.GetSystemDBS(strType,strDB);
                if (CapAppl != null && CapAppl.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CapAppl.Tables[0].Rows)
                    {
                        commonEntity.Add(new CommonEntity(row["CODE"].ToString(), row["VALUE"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                return commonEntity;
            }

            return commonEntity;
        }

        public bool InsertAgyTabDB(string FromDB, string ToDb, string AgyCode, string strType, string UserId)
        {
            bool boolsuccess;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                if (FromDB != string.Empty)
                    sqlParamList.Add(new SqlParameter("@FromDB", FromDB));
                if (ToDb != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ToDB", ToDb));
                if (AgyCode != string.Empty)
                    sqlParamList.Add(new SqlParameter("@AgyCode", AgyCode));

                if (strType != string.Empty)
                    sqlParamList.Add(new SqlParameter("@Type", strType));
                if (UserId != string.Empty)
                    sqlParamList.Add(new SqlParameter("@UserId", UserId));
                boolsuccess = Captain.DatabaseLayer.AgyTab.InsertAgyTabDB(sqlParamList);
                //boolsuccess = AgyTab.UpdateAGYTAB(sqlParamList);
            }
            catch (Exception ex)
            {
                //
                return false;
            }

            return boolsuccess;
        }

        public List<CTXMLTAGSEntity> GetCTXMLTAGSData()
        {
            List<CTXMLTAGSEntity> CTXMLTAGSdata = new List<CTXMLTAGSEntity>();
            try
            {
                DataSet ctxmlData = AgyTab.GetCTXMLTAGSData();
                if (ctxmlData != null && ctxmlData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ctxmlData.Tables[0].Rows)
                    {
                        CTXMLTAGSdata.Add(new CTXMLTAGSEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return CTXMLTAGSdata;
            }

            return CTXMLTAGSdata;
        }

        public List<CTXMLASOCEntity> GetCTXMLASOCData(string strCatg,string strScatg)
        {
            List<CTXMLASOCEntity> CTXMLASOCdata = new List<CTXMLASOCEntity>();
            try
            {
                DataSet ctXMLdata = AgyTab.GetCTXMLASOCData(strCatg,strScatg);
                if (ctXMLdata != null && ctXMLdata.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ctXMLdata.Tables[0].Rows)
                    {
                        CTXMLASOCdata.Add(new CTXMLASOCEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return CTXMLASOCdata;
            }

            return CTXMLASOCdata;
        }

        /// <summary>
        /// Get User Profile information. 
        /// </summary>
        /// <param name="userID">The Zipcode ID to get ZipCode.</param>
        /// <returns>Returns a DataSet with the ZipCode's profiles.</returns>
        public bool InsertUpdateCTXMLASOC(CTXMLASOCEntity ctcmlascoEntity)
        {
            bool boolsuccess;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@CTXMLASOC_CATG", ctcmlascoEntity.CTXMLASOC_CATG));
                sqlParamList.Add(new SqlParameter("@CTXMLASOC_SCATG", ctcmlascoEntity.CTXMLASOC_SCATG));
                if (ctcmlascoEntity.CTXMLASOC_DESC != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@CTXMLASOC_DESC", ctcmlascoEntity.CTXMLASOC_DESC));
                }
                if (ctcmlascoEntity.CTXMLASOC_DEFAULT != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@CTXMLASOC_DEFAULT", ctcmlascoEntity.CTXMLASOC_DEFAULT));
                }
                if (ctcmlascoEntity.CTXMLASOC_CODES != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@CTXMLASOC_CODES", ctcmlascoEntity.CTXMLASOC_CODES));
                }
                if (ctcmlascoEntity.CTXMLASOC_LSTC_OPERATOR != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@CTXMLASOC_LSTC_OPERATOR", ctcmlascoEntity.CTXMLASOC_LSTC_OPERATOR));
                }
              boolsuccess = AgyTab.InsertUpdateCTXMLASOC(sqlParamList);
            }
            catch (Exception ex)
            {
                //
                return false;
            }

            return boolsuccess;
        }


        public  List<CASESPMEntity> GETCASESPMLIST(string strAgency, string strDept, string strProgram, string strYear, string strFromDt, string strToDt, string strCumType,string strFilterType,string strFilterApp)
        {
            List<CASESPMEntity> CASESPMProfile = new List<CASESPMEntity>();

            try
            {
                DataSet CASESPMData = AgyTab.GETXMLTMSB2022SERVICEDATA(strAgency, strDept, strProgram, strYear, strFromDt, strToDt, strCumType, strFilterType, strFilterApp);

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASESPMProfile.Add(new CASESPMEntity(row,string.Empty));
                }
            }
            catch (Exception ex)
            { return CASESPMProfile; }

            return CASESPMProfile;
        }

        public List<CASEACTEntity> GETCASEACTLIST(string strAgency, string strDept, string strProgram, string strYear, string strFromDt, string strToDt, string strCumType, string strFilterType, string strFilterApp)
        {
            List<CASEACTEntity> CASEACTProfile = new List<CASEACTEntity>();

            try
            {
                DataSet CASEACtData = AgyTab.GETXMLTMSB2022SERVICEDATA(strAgency, strDept, strProgram, strYear, strFromDt, strToDt, strCumType, strFilterType, strFilterApp);

                if (CASEACtData != null && CASEACtData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASEACtData.Tables[0].Rows)
                        CASEACTProfile.Add(new CASEACTEntity(row, string.Empty, string.Empty));
                }
            }
            catch (Exception ex)
            { return CASEACTProfile; }

            return CASEACTProfile;
        }


        public List<CASEMSEntity> GETCASEMSLIST(string strAgency, string strDept, string strProgram, string strYear, string strFromDt, string strToDt, string strCumType, string strFilterType, string strFilterApp)
        {
            List<CASEMSEntity> CaseActprofile = new List<CASEMSEntity>();

            try
            {
                DataSet CASEActData = AgyTab.GETXMLTMSB2022SERVICEDATA(strAgency, strDept, strProgram, strYear, strFromDt, strToDt, strCumType, strFilterType, strFilterApp);

                if (CASEActData != null && CASEActData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASEActData.Tables[0].Rows)
                        CaseActprofile.Add(new CASEMSEntity(row,string.Empty,string.Empty));
                }
            }
            catch (Exception ex)
            { return CaseActprofile; }

            return CaseActprofile;
        }


        public List<CASECONTEntity> GETCASECONTLIST(string strAgency, string strDept, string strProgram, string strYear, string strFromDt, string strToDt, string strCumType, string strFilterApp)
        {
            List<CASECONTEntity> Casecontprofile = new List<CASECONTEntity>();

            try
            {
                DataSet CASEContData = AgyTab.GETXMLTMSB2022SERVICEDATA(strAgency, strDept, strProgram, strYear, strFromDt, strToDt, strCumType, "CASECONT", strFilterApp);

                if (CASEContData != null && CASEContData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASEContData.Tables[0].Rows)
                        Casecontprofile.Add(new CASECONTEntity(row,string.Empty));
                }
            }
            catch (Exception ex)
            { return Casecontprofile; }

            return Casecontprofile;
        }

                
        public  List<MATASMTEntity> GETXMLDATABRIDGEMATASMTList(string strAgency, string strDept, string strProgram, string strYear, string strFromDt, string strToDt, string strCumType, string strFilterApp)
        {
            List<MATASMTEntity> TemplateDetails = new List<MATASMTEntity>();
            try
            {
                DataSet Matadata = AgyTab.GETXMLTMSB2022SERVICEDATA(strAgency, strDept, strProgram, strYear, strFromDt, strToDt, strCumType, "MATASMT", strFilterApp);
                if (Matadata != null && Matadata.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in Matadata.Tables[0].Rows)
                    {
                        TemplateDetails.Add(new MATASMTEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return TemplateDetails;
            }

            return TemplateDetails;
        }


        public List<CaseIncomeEntity> GETCASEIncomeLIST(string strAgency, string strDept, string strProgram, string strYear, string strFromDt, string strToDt, string strCumType, string strFilterApp)
        {
            List<CaseIncomeEntity> CASEIncomeProfile = new List<CaseIncomeEntity>();

            try
            {
                DataSet CASEIncomeData = AgyTab.GETXMLTMSB2022SERVICEDATA(strAgency, strDept, strProgram, strYear, strFromDt, strToDt, strCumType, "CASEINCOME", strFilterApp);

                if (CASEIncomeData != null && CASEIncomeData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASEIncomeData.Tables[0].Rows)
                        CASEIncomeProfile.Add(new CaseIncomeEntity(row, string.Empty));
                }
            }
            catch (Exception ex)
            { return CASEIncomeProfile; }

            return CASEIncomeProfile;
        }

        public List<CaseMstEntity> GETCASEMSTLIST(string strAgency, string strDept, string strProgram, string strYear, string strFromDt, string strToDt, string strCumType,string strFilterApp)
        {
            List<CaseMstEntity> CASEMSTProfile = new List<CaseMstEntity>();

            try
            {
                DataSet CASEMSTData = AgyTab.GETXMLTMSB2022SERVICEDATA(strAgency, strDept, strProgram, strYear, strFromDt, strToDt, strCumType, "CASEMST", strFilterApp);

                if (CASEMSTData != null && CASEMSTData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASEMSTData.Tables[0].Rows)
                        CASEMSTProfile.Add(new CaseMstEntity(row, "TMSB2022"));
                }
            }
            catch (Exception ex)
            { return CASEMSTProfile; }

            return CASEMSTProfile;
        }

        public List<CaseSnpEntity> GETCASESNPLIST(string strAgency, string strDept, string strProgram, string strYear, string strFromDt, string strToDt, string strCumType, string strFilterApp)
        {
            List<CaseSnpEntity> CASESnpProfile = new List<CaseSnpEntity>();

            try
            {
                DataSet CASEsnpData = AgyTab.GETXMLTMSB2022SERVICEDATA(strAgency, strDept, strProgram, strYear, strFromDt, strToDt, strCumType, "CASESNP", strFilterApp);

                if (CASEsnpData != null && CASEsnpData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASEsnpData.Tables[0].Rows)
                        CASESnpProfile.Add(new CaseSnpEntity(row, "TMSB2022"));
                }
            }
            catch (Exception ex)
            { return CASESnpProfile; }

            return CASESnpProfile;
        }







    }
}
