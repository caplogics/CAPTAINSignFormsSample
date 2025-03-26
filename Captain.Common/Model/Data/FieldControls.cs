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
    public class FieldControls
    {

        public FieldControls()
        {

        }

        #region Properties

        public CaptainModel Model { get; set; }

        #endregion

        public List<CustfldsEntity> GetCUSTFLDSByScrCode(string ScrCode, string From_Table, string Hie)
        {
            List<CustfldsEntity> CustProfile = new List<CustfldsEntity>();
            try
            {
                DataSet CustfldData = FieldControlsDB.GetCUSTFLDSByScrCode(ScrCode, From_Table, Hie);
                if (CustfldData != null && CustfldData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CustfldData.Tables[0].Rows)
                    {
                        CustProfile.Add(new CustfldsEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return CustProfile;
            }

            return CustProfile;
        }

        public List<CustfldsEntity> GetCUSTFLDSByScrCodeContact(string ScrCode, string From_Table, string Hie)
        {
            List<CustfldsEntity> CustProfile = new List<CustfldsEntity>();
            try
            {
                DataSet CustfldData = FieldControlsDB.GetCUSTFLDSByScrCode(ScrCode, From_Table, Hie);
                if (CustfldData != null && CustfldData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CustfldData.Tables[0].Rows)
                    {
                        CustProfile.Add(new CustfldsEntity(row, string.Empty));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return CustProfile;
            }

            return CustProfile;
        }

        public List<CustfldsEntity> GetSelCustDetails(string RecKey)
        {
            List<CustfldsEntity> CustProfile = new List<CustfldsEntity>();
            try
            {
                DataSet CustfldData = FieldControlsDB.GetSelCustDetails(RecKey);
                if (CustfldData != null && CustfldData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CustfldData.Tables[0].Rows)
                    {
                        CustProfile.Add(new CustfldsEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return CustProfile;
            }

            return CustProfile;
        }

        public List<FLDCNTLEntity> GetFLDCNTLByScrCode(string RecKey, string Hierarchy)
        {
            List<FLDCNTLEntity> FLDCNTLProfile = new List<FLDCNTLEntity>();
            try
            {
                DataSet FLDCNTLData = FieldControlsDB.GetFLDCNTLByScrCode(RecKey, Hierarchy);
                if (FLDCNTLData != null && FLDCNTLData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in FLDCNTLData.Tables[0].Rows)
                    {
                        FLDCNTLProfile.Add(new FLDCNTLEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return FLDCNTLProfile;
            }

            return FLDCNTLProfile;
        }

        public List<FLDDESCHieEntity> GetStatCustDescByScrCodeHIE(string Called_By, string ScrCode, string Hierarchy)
        {
            List<FLDDESCHieEntity> FLDCNTLProfile = new List<FLDDESCHieEntity>();
            try
            {
                DataSet FLDCNTLData = FieldControlsDB.GetStatCustDescByScrCodeHIE(Called_By, ScrCode, Hierarchy);
                if (FLDCNTLData != null && FLDCNTLData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in FLDCNTLData.Tables[0].Rows)
                    {
                        FLDCNTLProfile.Add(new FLDDESCHieEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return FLDCNTLProfile;
            }

            return FLDCNTLProfile;
        }

        public List<CustRespEntity> GetCustRespByCustCode(string CustCode, string strType)
        {
            List<CustRespEntity> FLDCNTLProfile = new List<CustRespEntity>();
            try
            {
                DataSet FLDCNTLData = FieldControlsDB.GetCustRespByCustCode(CustCode, strType);
                if (FLDCNTLData != null && FLDCNTLData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in FLDCNTLData.Tables[0].Rows)
                    {
                        FLDCNTLProfile.Add(new CustRespEntity(row, strType));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return FLDCNTLProfile;
            }

            return FLDCNTLProfile;
        }

        public List<CustRespEntity> GetCustRespByCustCode(string CustCode)
        {
            List<CustRespEntity> FLDCNTLProfile = new List<CustRespEntity>();
            try
            {
                DataSet FLDCNTLData = FieldControlsDB.GetCustRespByCustCode(CustCode, string.Empty);
                if (FLDCNTLData != null && FLDCNTLData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in FLDCNTLData.Tables[0].Rows)
                    {
                        FLDCNTLProfile.Add(new CustRespEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return FLDCNTLProfile;
            }

            return FLDCNTLProfile;
        }

        public List<CustRespEntity> GetCustRespByScrCode(string CustCode)
        {
            List<CustRespEntity> FLDCNTLProfile = new List<CustRespEntity>();
            try
            {
                DataSet FLDCNTLData = FieldControlsDB.GetCustRespByScrCode(CustCode);
                if (FLDCNTLData != null && FLDCNTLData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in FLDCNTLData.Tables[0].Rows)
                    {
                        FLDCNTLProfile.Add(new CustRespEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return FLDCNTLProfile;
            }

            return FLDCNTLProfile;
        }



        public List<FLDCNTLHIEAddEntity> GetStatCustDescByScrCode(string Called_By, string ScrCode)
        {
            List<FLDCNTLHIEAddEntity> FLDCNTLProfile = new List<FLDCNTLHIEAddEntity>();
            try
            {
                DataSet TableData = FieldControlsDB.GetStatCustDescByScrCode(Called_By, ScrCode);
                if (TableData != null && TableData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in TableData.Tables[0].Rows)
                    {
                        FLDCNTLProfile.Add(new FLDCNTLHIEAddEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return FLDCNTLProfile;
            }

            return FLDCNTLProfile;
        }

        public List<FldcntlHieEntity> GetFLDCNTLHIE(string ScrCode, string HIE, string Type)
        {
            List<FldcntlHieEntity> FLDCNTLEntity = new List<FldcntlHieEntity>();
            try
            {
                DataSet TableData = FieldControlsDB.GetFLDCNTLHIE(ScrCode, HIE, Type);
                if (TableData != null && TableData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in TableData.Tables[0].Rows)
                    {
                        FLDCNTLEntity.Add(new FldcntlHieEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return FLDCNTLEntity;
            }

            return FLDCNTLEntity;
        }

        public List<FldcntlHieEntity> GetFLDCNTLHIE(string ScrCode, string HIE, string Type, string strReqType)
        {
            List<FldcntlHieEntity> FLDCNTLEntity = new List<FldcntlHieEntity>();
            try
            {
                DataSet TableData = FieldControlsDB.GetFLDCNTLHIE(ScrCode, HIE, Type, strReqType);
                if (TableData != null && TableData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in TableData.Tables[0].Rows)
                    {
                        FLDCNTLEntity.Add(new FldcntlHieEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return FLDCNTLEntity;
            }

            return FLDCNTLEntity;
        }

        public List<CommonEntity> GetCASEELIGHIE(string ScrCode, string HIE, string Type)
        {
            List<CommonEntity> CASEELIGEntity = new List<CommonEntity>();
            try
            {
                DataSet TableData = FieldControlsDB.GetFLDCNTLHIE(ScrCode, HIE, Type);
                if (TableData != null && TableData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in TableData.Tables[0].Rows)
                    {
                        CASEELIGEntity.Add(new CommonEntity(row["ELIG_QUES_CODE"].ToString(), string.Empty));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return CASEELIGEntity;
            }

            return CASEELIGEntity;
        }

        public List<CustomQuestionsEntity> GetCustomQuestions(string ScrCode, string memAccess, string HIE, string seq, string type, string questionAccess)
        {
            List<CustomQuestionsEntity> custEntity = new List<CustomQuestionsEntity>();
            try
            {
                DataSet TableData = FieldControlsDB.GetCustomQuestions(ScrCode, memAccess, HIE, seq, type, questionAccess);
                if (TableData != null && TableData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in TableData.Tables[0].Rows)
                    {
                        custEntity.Add(new CustomQuestionsEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return custEntity;
            }

            return custEntity;
        }

        public List<CustomQuestionsEntity> GetPreassesQuestions(string ScrCode, string memAccess, string HIE, string seq, string type, string questionAccess)
        {
            List<CustomQuestionsEntity> custEntity = new List<CustomQuestionsEntity>();
            try
            {
                DataSet TableData = FieldControlsDB.GetCustomQuestions(ScrCode, memAccess, HIE, seq, type, questionAccess);
                if (TableData != null && TableData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in TableData.Tables[0].Rows)
                    {
                        custEntity.Add(new CustomQuestionsEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return custEntity;
            }

            return custEntity;
        }

        public List<CustRespEntity> GetCustomResponses(string ScrCode, string custCode)
        {
            List<CustRespEntity> custEntity = new List<CustRespEntity>();
            try
            {
                DataSet TableData = FieldControlsDB.GetQuestionResponses(ScrCode, custCode);
                if (TableData != null && TableData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in TableData.Tables[0].Rows)
                    {
                        custEntity.Add(new CustRespEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return custEntity;
            }

            return custEntity;
        }

        public bool UpdateCUSTFLDS(CustfldsEntity Cust, out string New_CUST_Code_Code)
        {
            bool boolsuccess;
            New_CUST_Code_Code = "NewCustCode";

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@Row_Type", Cust.UpdateType));
                sqlParamList.Add(new SqlParameter("@CUST_SCR_CODE", Cust.ScrCode));
                sqlParamList.Add(new SqlParameter("@CUST_CODE", Cust.CustCode));

                sqlParamList.Add(new SqlParameter("@CUST_DESC", Cust.CustDesc));
                sqlParamList.Add(new SqlParameter("@CUST_RESP_TYPE", Cust.RespType));
                if (Cust.Sub_Screen != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CUST_SUB_SCR", Cust.Sub_Screen));
                if (Cust.MemAccess != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CUST_MEM_ACCESS", Cust.MemAccess));
                sqlParamList.Add(new SqlParameter("@CUST_EQUAL", float.Parse(Cust.Equalto)));
                sqlParamList.Add(new SqlParameter("@CUST_GREATER", float.Parse(Cust.Greater)));
                sqlParamList.Add(new SqlParameter("@CUST_LESSTHAN", float.Parse(Cust.Less)));
                sqlParamList.Add(new SqlParameter("@CUST_ALPHA", Cust.Alpha));
                sqlParamList.Add(new SqlParameter("@CUST_OTHER", Cust.Other));

                sqlParamList.Add(new SqlParameter("@CUST_ABBR_QUESTION", Cust.Question));
                sqlParamList.Add(new SqlParameter("@CUST_ALLOW_FDATE", Cust.FutureDate));
                sqlParamList.Add(new SqlParameter("@CUST_ADD_OPERATOR", Cust.AddOpr));
                sqlParamList.Add(new SqlParameter("@CUST_LSTC_OPERATOR", Cust.ChdOpr));
                if (Cust.CustSeq != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CUST_SEQ", int.Parse(Cust.CustSeq)));

                sqlParamList.Add(new SqlParameter("@CUST_ACTIVE", Cust.Active));

                SqlParameter parameterMsg = new SqlParameter("@New_CUST_Code", SqlDbType.VarChar, 6);
                parameterMsg.Direction = ParameterDirection.Output;
                sqlParamList.Add(parameterMsg);

                boolsuccess = Captain.DatabaseLayer.FieldControlsDB.UpdateCustFlds(sqlParamList);
                New_CUST_Code_Code = parameterMsg.Value.ToString();
            }
            catch (Exception ex)
            {
                //
                return false;
            }

            return boolsuccess;
        }

        public bool UpdateCUSTRESP(List<CustRespEntity> RespList, string New_CUST_Code_Code)
        {
            bool boolsuccess = false;

            try
            {
                foreach (CustRespEntity Entity in RespList)
                {
                    if (Entity.Changed == "Y" && !(string.IsNullOrEmpty(Entity.DescCode)) && !(string.IsNullOrEmpty(Entity.RespDesc)))
                    {
                        List<SqlParameter> sqlParamList = new List<SqlParameter>();
                        sqlParamList.Add(new SqlParameter("@Row_Type", Entity.RecType));
                        sqlParamList.Add(new SqlParameter("@RSP_SCR_CODE", Entity.ScrCode));
                        sqlParamList.Add(new SqlParameter("@RSP_CUST_CODE", New_CUST_Code_Code));
                        sqlParamList.Add(new SqlParameter("@RSP_SEQ", int.Parse(Entity.RespSeq)));
                        sqlParamList.Add(new SqlParameter("@RSP_DESC", Entity.RespDesc));
                        sqlParamList.Add(new SqlParameter("@RSP_RESP_CODE", Entity.DescCode));
                        sqlParamList.Add(new SqlParameter("@RSP_ADD_OPERATOR", Entity.AddOpr));
                        sqlParamList.Add(new SqlParameter("@RSP_LSTC_OPERATOR", Entity.ChgOpr));
                        if (Entity.Points != string.Empty)
                            sqlParamList.Add(new SqlParameter("@RSP_POINTS", Entity.Points));
                        if (Entity.RespDefault != string.Empty)
                            sqlParamList.Add(new SqlParameter("@RSP_DEFAULT", Entity.RespDefault));

                        boolsuccess = Captain.DatabaseLayer.FieldControlsDB.UpdateCUSTRESP(sqlParamList);
                    }
                }

            }
            catch (Exception ex)
            {
                //
                return false;
            }

            return boolsuccess;
        }


        public bool UpdateCUSTRESPSingle(string strRecType, string strScrCode, string New_CUST_Code_Code, string strSeq, string strDesc, string strRespCode, string strAddoperator, string strlstcopertor, string strPoints)
        {
            bool boolsuccess = false;

            try
            {

                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@Row_Type", strRecType));
                sqlParamList.Add(new SqlParameter("@RSP_SCR_CODE", strScrCode));
                sqlParamList.Add(new SqlParameter("@RSP_CUST_CODE", New_CUST_Code_Code));
                sqlParamList.Add(new SqlParameter("@RSP_SEQ", int.Parse(strSeq)));
                sqlParamList.Add(new SqlParameter("@RSP_DESC", strDesc));
                if (strRespCode != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RSP_RESP_CODE", strRespCode));
                sqlParamList.Add(new SqlParameter("@RSP_ADD_OPERATOR", strAddoperator));
                sqlParamList.Add(new SqlParameter("@RSP_LSTC_OPERATOR", strlstcopertor));
                if (strPoints != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RSP_POINTS", strPoints));


                boolsuccess = Captain.DatabaseLayer.FieldControlsDB.UpdateCUSTRESP(sqlParamList);


            }
            catch (Exception ex)
            {
                //
                return false;
            }

            return boolsuccess;
        }

        public bool UpdateCUSTRESPSingle(string strRecType, string strScrCode, string New_CUST_Code_Code, string strSeq, string strDesc, string strRespCode, string strAddoperator, string strlstcopertor, string strPoints, string strRespNCode)
        {
            bool boolsuccess = false;

            try
            {

                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@Row_Type", strRecType));
                sqlParamList.Add(new SqlParameter("@RSP_SCR_CODE", strScrCode));
                sqlParamList.Add(new SqlParameter("@RSP_CUST_CODE", New_CUST_Code_Code));
                sqlParamList.Add(new SqlParameter("@RSP_SEQ", int.Parse(strSeq)));
                sqlParamList.Add(new SqlParameter("@RSP_DESC", strDesc));
                if (strRespCode != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RSP_RESP_CODE", strRespCode));
                sqlParamList.Add(new SqlParameter("@RSP_ADD_OPERATOR", strAddoperator));
                sqlParamList.Add(new SqlParameter("@RSP_LSTC_OPERATOR", strlstcopertor));
                if (strPoints != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RSP_POINTS", strPoints));

                if (strRespNCode != string.Empty)
                    sqlParamList.Add(new SqlParameter("@RSP_RESP_NCODE", strRespNCode));



                boolsuccess = Captain.DatabaseLayer.FieldControlsDB.UpdateCUSTRESP(sqlParamList);


            }
            catch (Exception ex)
            {
                //
                return false;
            }

            return boolsuccess;
        }


        public bool UpdateFLDCNTLHIE(List<FLDDESCHieEntity> Entity, string UserName, char Valid)
        {
            bool boolsuccess = false;

            try
            {
                foreach (FLDDESCHieEntity Fldhie in Entity)
                {
                    if (Fldhie.Changed == "Y")
                    {
                        if (Fldhie.RecType == "U" || Fldhie.RecType == "I")
                        {
                            List<SqlParameter> sqlParamList = new List<SqlParameter>();
                            sqlParamList.Add(new SqlParameter("@Row_Type", Fldhie.RecType));
                            sqlParamList.Add(new SqlParameter("@FLDH_SCR_CODE", Fldhie.ScrCode));
                            sqlParamList.Add(new SqlParameter("@FLDH_SCR_HIE", Fldhie.ScrHie));
                            sqlParamList.Add(new SqlParameter("@FLDH_CODE", Fldhie.FldCode));
                            sqlParamList.Add(new SqlParameter("@FLDH_ACTIVE", Fldhie.Active));
                            sqlParamList.Add(new SqlParameter("@FLDH_ENABLED", Fldhie.Enab));
                            sqlParamList.Add(new SqlParameter("@FLDH_REQUIRED", Fldhie.Req));
                            sqlParamList.Add(new SqlParameter("@FLDH_SHARED", Fldhie.Shared));
                            sqlParamList.Add(new SqlParameter("@UserName", UserName));
                            sqlParamList.Add(new SqlParameter("@ValidateScr", Valid));

                            boolsuccess = Captain.DatabaseLayer.FieldControlsDB.UpdateFLDCNTLHIE(sqlParamList);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return false;
            }

            return boolsuccess;
        }

        public bool InsertFLDCNTLHIE(List<FLDCNTLHIEAddEntity> Entity, string Hierarchy, string UserName, char Valid)
        {
            bool boolsuccess = false;

            try
            {
                foreach (FLDCNTLHIEAddEntity Fldhie in Entity)
                {
                    if (Fldhie.Changed == "Y")
                    {
                        List<SqlParameter> sqlParamList = new List<SqlParameter>();
                        sqlParamList.Add(new SqlParameter("@Row_Type", Fldhie.RecType));
                        sqlParamList.Add(new SqlParameter("@FLDH_SCR_CODE", Fldhie.ScrCode));
                        sqlParamList.Add(new SqlParameter("@FLDH_SCR_HIE", Hierarchy));
                        sqlParamList.Add(new SqlParameter("@FLDH_CODE", Fldhie.FldCode));
                        sqlParamList.Add(new SqlParameter("@FLDH_ACTIVE", Fldhie.Active));
                        sqlParamList.Add(new SqlParameter("@FLDH_ENABLED", Fldhie.Enab));
                        sqlParamList.Add(new SqlParameter("@FLDH_REQUIRED", Fldhie.Req));
                        sqlParamList.Add(new SqlParameter("@FLDH_SHARED", Fldhie.Shared));
                        sqlParamList.Add(new SqlParameter("@UserName", UserName));
                        sqlParamList.Add(new SqlParameter("@ValidateScr", Valid));

                        boolsuccess = Captain.DatabaseLayer.FieldControlsDB.UpdateFLDCNTLHIE(sqlParamList);
                    }
                }
            }
            catch (Exception ex)
            { return false; }
            return boolsuccess;
        }

        public bool InsertInactiveFLDCNTLHIE(string ScrCode, string Hierarchy, string[,] InactiveArr, string UserName, char Valid)
        {
            bool boolsuccess = false;

            try
            {
                for (int i = 0; i < (InactiveArr.Length / 4); i++)
                {
                    if (InactiveArr[i, 0] == "Y" || InactiveArr[i, 1] == "Y" || InactiveArr[i, 2] == "Y")
                    {
                        List<SqlParameter> sqlParamList = new List<SqlParameter>();
                        sqlParamList.Add(new SqlParameter("@Row_Type", "I"));
                        sqlParamList.Add(new SqlParameter("@FLDH_SCR_CODE", ScrCode));
                        sqlParamList.Add(new SqlParameter("@FLDH_SCR_HIE", Hierarchy));
                        sqlParamList.Add(new SqlParameter("@FLDH_CODE", InactiveArr[i, 3]));
                        sqlParamList.Add(new SqlParameter("@FLDH_ACTIVE", "A"));
                        sqlParamList.Add(new SqlParameter("@FLDH_ENABLED", InactiveArr[i, 0]));
                        sqlParamList.Add(new SqlParameter("@FLDH_REQUIRED", InactiveArr[i, 1]));
                        sqlParamList.Add(new SqlParameter("@FLDH_SHARED", InactiveArr[i, 2]));
                        sqlParamList.Add(new SqlParameter("@UserName", UserName));
                        sqlParamList.Add(new SqlParameter("@ValidateScr", Valid));

                        boolsuccess = Captain.DatabaseLayer.FieldControlsDB.UpdateFLDCNTLHIE(sqlParamList);
                    }
                }
            }
            catch (Exception ex)
            { return false; }
            return boolsuccess;
        }

        public List<CustRespEntity> Browse_CUSTRESP(CustRespEntity Entity, string Opretaion_Mode)
        {
            List<CustRespEntity> CASESPMProfile = new List<CustRespEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_CUSTRESP_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet CASESPMData = Captain.DatabaseLayer.FieldControlsDB.Browse_CUSTRESP(sqlParamList);

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASESPMProfile.Add(new CustRespEntity(row));
                }
            }
            catch (Exception ex)
            { return CASESPMProfile; }

            return CASESPMProfile;
        }

        public List<SqlParameter> Prepare_CUSTRESP_SqlParameters_List(CustRespEntity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                if (Opretaion_Mode != "Browse")
                    sqlParamList.Add(new SqlParameter("@Row_Type", Entity.RecType));


                sqlParamList.Add(new SqlParameter("@RSP_CUST_CODE", Entity.ResoCode));
                sqlParamList.Add(new SqlParameter("@RSP_SEQ", Entity.RespSeq));
                sqlParamList.Add(new SqlParameter("@RSP_DESC", Entity.RespDesc));
                sqlParamList.Add(new SqlParameter("@RSP_RESP_CODE", Entity.DescCode));
                sqlParamList.Add(new SqlParameter("@RSP_SCR_CODE", Entity.ScrCode));
                sqlParamList.Add(new SqlParameter("@RSP_LSTC_OPERATOR", Entity.ChgOpr));

                if (Opretaion_Mode == "Browse")
                {
                    sqlParamList.Add(new SqlParameter("@RSP_DATE_ADD", Entity.AddDate));
                    sqlParamList.Add(new SqlParameter("@RSP_ADD_OPERATOR", Entity.AddOpr));
                    sqlParamList.Add(new SqlParameter("@RSP_DATE_LSTC", Entity.Changed));
                }
            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }

        public List<FLDSCRSEntity> Browse_FLDSCRS(FLDSCRSEntity Entity)
        {
            List<FLDSCRSEntity> FLDSCRSProfile = new List<FLDSCRSEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_FLDSCRS_SqlParameters_List(Entity);
                DataSet FLDSCRSData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_FLDSCRS]");
                if (FLDSCRSData != null && FLDSCRSData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in FLDSCRSData.Tables[0].Rows)
                        FLDSCRSProfile.Add(new FLDSCRSEntity(row));
                }
            }
            catch (Exception ex)
            { return FLDSCRSProfile; }

            return FLDSCRSProfile;
        }

        public List<SqlParameter> Prepare_FLDSCRS_SqlParameters_List(FLDSCRSEntity Entity)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList.Add(new SqlParameter("@FLDSCRS_CLDBY", Entity.Called_By));
                sqlParamList.Add(new SqlParameter("@FLDSCRS_CODE", Entity.Scr_Code));
                sqlParamList.Add(new SqlParameter("@FLDSCRS_SUB", Entity.Scr_Sub_Code));
                sqlParamList.Add(new SqlParameter("@FLDSCRS_DESC", Entity.Scr_Desc));
            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }

        public bool UpdateFLDCNTL(string Type, string ScrCode, string ScrHie, string Valid, string Operator)
        {
            bool boolsuccess;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@Row_Type", Type));
                sqlParamList.Add(new SqlParameter("@FLD_SCR_CODE", ScrCode));
                sqlParamList.Add(new SqlParameter("@FLD_SCR_HIE", ScrHie));
                sqlParamList.Add(new SqlParameter("@FLD_SCR_VALID", Valid));
                sqlParamList.Add(new SqlParameter("@FLD_ADD_OPERATOR", Operator));

                boolsuccess = Captain.DatabaseLayer.FieldControlsDB.UpdateFLDCNTL(sqlParamList);
            }
            catch (Exception ex) { return false; }

            return boolsuccess;
        }

        public bool DeleteFLDCNTLHIE(string Scr_Code, string Scr_Hierarchy)
        {
            try
            {
                Captain.DatabaseLayer.FieldControlsDB.DeleteFLDCNTLHIE(Scr_Code, Scr_Hierarchy);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public bool DeleteCUSTFLDS(string Key, string strScreenName)
        {
            bool boolsucess = true;
            try
            {
                boolsucess = Captain.DatabaseLayer.FieldControlsDB.DeleteCUSTFLDS(Key, strScreenName);
            }
            catch (Exception ex)
            {
                boolsucess = false;
            }

            return boolsucess;
        }

        public List<PreassessQuesEntity> GetPreassessData(string Type)
        {
            List<PreassessQuesEntity> PreassessEntity = new List<PreassessQuesEntity>();
            try
            {
                DataSet TableData = FieldControlsDB.GetPreassessData(Type);
                if (TableData != null && TableData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in TableData.Tables[0].Rows)
                    {
                        PreassessEntity.Add(new PreassessQuesEntity(row, Type));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return PreassessEntity;
            }

            return PreassessEntity;
        }

        public bool InsertUpdatePrechild(string QId, string Did, string DQid, string QResult, string QSort, string strEnable, string strDisable, string strMode)
        {
            bool boolsuccess;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                if (QId != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PRECHILD_QID", QId));
                if (Did != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PRECHILD_DID", Did));
                if (DQid != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PRECHILD_DQID", DQid));
                if (QResult != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PRECHILD_REQ", QResult));
                if (QSort != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PRECHILD_SORT", QSort));
                if (strEnable != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PRECHILD_ENABLE", strEnable));
                if (strDisable != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PRECHILD_DISABLE", strDisable));
                sqlParamList.Add(new SqlParameter("@Mode", strMode));

                boolsuccess = Captain.DatabaseLayer.FieldControlsDB.InsertUpdatePrechild(sqlParamList);
            }
            catch (Exception ex) { return false; }

            return boolsuccess;
        }

        public List<AddCustEntity> GETCUSTRESPAPPCOUNT(string strCode, string strType, string strRespCode)
        {
            List<AddCustEntity> MSTSERProfile = new List<AddCustEntity>();

            try
            {

                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.GETCUSTRESPAPPCOUNT(strCode, strType, strRespCode);


                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        MSTSERProfile.Add(new AddCustEntity(row, strType));
                }
            }
            catch (Exception ex)
            { return MSTSERProfile; }

            return MSTSERProfile;
        }

        public List<ScafldsEntity> GETSCAFLDSDATA(string scrCode, string fldCode, string scrHie)
        {
            List<ScafldsEntity> oScafldsEntity = new List<ScafldsEntity>();

            try
            {
                DataSet TableData = FieldControlsDB.GETSCAFLDSDATA(scrCode, fldCode, scrHie, "SCAFLDS");
                if (TableData != null && TableData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in TableData.Tables[0].Rows)
                    {
                        oScafldsEntity.Add(new ScafldsEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return oScafldsEntity;
            }

            return oScafldsEntity;
        }
        public List<ScaFldsHieEntity> GETSCAFLDSHIEDATA(string scrCode, string fldCode, string scrHie)
        {
            List<ScaFldsHieEntity> oScaFldsHieEntity = new List<ScaFldsHieEntity>();

            try
            {
                DataSet TableData = FieldControlsDB.GETSCAFLDSDATA(scrCode, fldCode, scrHie, string.Empty);
                if (TableData != null && TableData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in TableData.Tables[0].Rows)
                    {
                        oScaFldsHieEntity.Add(new ScaFldsHieEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return oScaFldsHieEntity;
            }

            return oScaFldsHieEntity;
        }

        public bool InsertUpdateSCAFLDSHIE(ScaFldsHieEntity ScaFldsHie)
        {
            bool boolsuccess = false;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@Row_Type", ScaFldsHie.RecType));
                sqlParamList.Add(new SqlParameter("@SCAH_SCR_CODE", ScaFldsHie.ScrCode));
                sqlParamList.Add(new SqlParameter("@SCAH_SCR_HIE", ScaFldsHie.ScrHie));
                sqlParamList.Add(new SqlParameter("@SCAH_CODE", ScaFldsHie.ScahCode));
                sqlParamList.Add(new SqlParameter("@SCAH_ACTIVE", ScaFldsHie.Active));
                sqlParamList.Add(new SqlParameter("@SCAH_SEL", ScaFldsHie.Sel));
                sqlParamList.Add(new SqlParameter("@SCAH_ADD_OPERATOR", ScaFldsHie.AddOperator));
                sqlParamList.Add(new SqlParameter("@SCAH_LSTC_OPERATOR", ScaFldsHie.LstcOperator));
                sqlParamList.Add(new SqlParameter("@SCAH_MSG", ScaFldsHie.Msg));

                boolsuccess = Captain.DatabaseLayer.FieldControlsDB.InsertUpdateSCAFLDSDATA(sqlParamList);
            }
            catch (Exception ex)
            { return false; }
            return boolsuccess;
        }

        public List<PMTFLDCNTLHEntity> GETPMTFLDCNTLH(string scrCode, string fldCode, string scrHie, string strMode)
        {
            List<PMTFLDCNTLHEntity> FLDCNTLEntity = new List<PMTFLDCNTLHEntity>();
            try
            {
                DataSet TableData = FieldControlsDB.GETPMTSTDFLDS(scrCode, fldCode, scrHie, strMode);
                if (TableData != null && TableData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in TableData.Tables[0].Rows)
                    {
                        FLDCNTLEntity.Add(new PMTFLDCNTLHEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return FLDCNTLEntity;
            }

            return FLDCNTLEntity;
        }

        //Added by Sudheer 05/21/2021 included the SP,Branch,Group and CACode
        public List<PMTFLDCNTLHEntity> GETPMTFLDCNTLHSP(string scrCode, string fldCode, string scrHie, string SPcode, string Branch, string Group, string CAcode, string strMode)
        {
            List<PMTFLDCNTLHEntity> FLDCNTLEntity = new List<PMTFLDCNTLHEntity>();
            try
            {
                DataSet TableData = FieldControlsDB.GETPMTSTDFLDSWITHSP(scrCode, fldCode, scrHie, SPcode, Branch, Group, CAcode, strMode);
                if (TableData != null && TableData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in TableData.Tables[0].Rows)
                    {
                        FLDCNTLEntity.Add(new PMTFLDCNTLHEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return FLDCNTLEntity;
            }

            return FLDCNTLEntity;
        }

        public List<PMTSTDFLDSEntity> GETPMTSTDFLDS(string scrCode, string fldCode, string scrHie, string strMode)
        {
            List<PMTSTDFLDSEntity> PMTSTDFLDSEntity = new List<PMTSTDFLDSEntity>();
            try
            {
                DataSet TableData = FieldControlsDB.GETPMTSTDFLDS(scrCode, fldCode, scrHie, strMode);
                if (TableData != null && TableData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in TableData.Tables[0].Rows)
                    {
                        PMTSTDFLDSEntity.Add(new PMTSTDFLDSEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return PMTSTDFLDSEntity;
            }

            return PMTSTDFLDSEntity;
        }

        public bool InsertUpdateDelPMTFLDCNTLH(PMTFLDCNTLHEntity pmtfldhie)
        {
            bool boolsuccess = false;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@PMFLDH_SCR_CODE", pmtfldhie.PMFLDH_SCR_CODE));
                sqlParamList.Add(new SqlParameter("@PMFLDH_SCR_HIE", pmtfldhie.PMFLDH_SCR_HIE));
                if (pmtfldhie.PMFLDH_SP != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PMFLDH_SP", pmtfldhie.PMFLDH_SP));
                if (pmtfldhie.PMFLDH_BRANCH != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PMFLDH_BRANCH", pmtfldhie.PMFLDH_BRANCH));
                if (pmtfldhie.PMFLDH_CURR_GRP != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PMFLDH_CURR_GRP", pmtfldhie.PMFLDH_CURR_GRP));
                if (pmtfldhie.PMFLDH_CA_CODE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PMFLDH_CA_CODE", pmtfldhie.PMFLDH_CA_CODE));
                if (pmtfldhie.PMFLDH_CODE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@PMFLDH_CODE", pmtfldhie.PMFLDH_CODE));
                sqlParamList.Add(new SqlParameter("@PMFLDH_ACTIVE", pmtfldhie.PMFLDH_ACTIVE));
                sqlParamList.Add(new SqlParameter("@PMFLDH_ENABLED", pmtfldhie.PMFLDH_ENABLED));
                sqlParamList.Add(new SqlParameter("@PMFLDH_REQUIRED", pmtfldhie.PMFLDH_REQUIRED));
                sqlParamList.Add(new SqlParameter("@PMFLDH_CATG", pmtfldhie.PMFLDH_CATG));
                sqlParamList.Add(new SqlParameter("@Mode", pmtfldhie.Mode));
                boolsuccess = Captain.DatabaseLayer.FieldControlsDB.InsertUpdateDelPMTFLDCNTLH(sqlParamList);
            }
            catch (Exception ex)
            { return false; }
            return boolsuccess;
        }

        public bool CAPS_AGCYPARTQUES_INSUPDEL(AGCYPARTQUESEntity Agypartquesdata, out string New_CUST_Code_Code)
        {
            bool boolsuccess;
            New_CUST_Code_Code = "NewCustCode";

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@Mode", Agypartquesdata.Mode));
                if (Agypartquesdata.AGYQ_CODE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@AGYQ_CODE", Agypartquesdata.AGYQ_CODE));
                sqlParamList.Add(new SqlParameter("@AGYQ_ACTIVE", Agypartquesdata.AGYQ_ACTIVE));
                sqlParamList.Add(new SqlParameter("@AGYQ_REQ", Agypartquesdata.AGYQ_REQ));
                sqlParamList.Add(new SqlParameter("@AGYQ_DESC", Agypartquesdata.AGYQ_DESC));
                sqlParamList.Add(new SqlParameter("@AGYQ_RESP_TYPE", Agypartquesdata.AGYQ_RESP_TYPE));

                sqlParamList.Add(new SqlParameter("@AGYQ_ADD_OPERATOR", Agypartquesdata.AGYQ_ADD_OPERATOR));
                sqlParamList.Add(new SqlParameter("@AGYQ_LSTC_OPERATOR", Agypartquesdata.AGYQ_LSTC_OPERATOR));

                SqlParameter parameterMsg = new SqlParameter("@New_AGYQ_CODE", SqlDbType.Int);
                parameterMsg.Direction = ParameterDirection.Output;
                sqlParamList.Add(parameterMsg);

                boolsuccess = Captain.DatabaseLayer.FieldControlsDB.CAPS_AGCYPARTQUES_INSUPDEL(sqlParamList);
                New_CUST_Code_Code = parameterMsg.Value.ToString();
            }
            catch (Exception ex)
            {
                //
                return false;
            }

            return boolsuccess;
        }

        public List<AGCYPARTQUESEntity> CAPS_AGCYQUESRESP_GET(string Quescode, string Mode)
        {
            List<AGCYPARTQUESEntity> AGCYPARTQUESEntity = new List<AGCYPARTQUESEntity>();
            try
            {
                DataSet TableData = FieldControlsDB.CAPS_AGCYQUESRESP_GET(Quescode, Mode);
                if (TableData != null && TableData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in TableData.Tables[0].Rows)
                    {
                        AGCYPARTQUESEntity.Add(new AGCYPARTQUESEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return AGCYPARTQUESEntity;
            }

            return AGCYPARTQUESEntity;
        }

        public bool CAPS_AGCYSERQASSOC_INSUPDEL(string strSercode, string strQuescode, string strSeq, string stroperator, string strMode)
        {
            bool boolsuccess = false;

            try
            {

                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                if (strSercode.Trim() != string.Empty)
                    sqlParamList.Add(new SqlParameter("@AGYSERQ_SERCODE", strSercode));
                if (strQuescode.Trim() != string.Empty)
                    sqlParamList.Add(new SqlParameter("@AGYSERQ_QUESCODE", strQuescode));
                if (strSeq.Trim() != string.Empty)
                    sqlParamList.Add(new SqlParameter("@AGYSERQ_SEQ", strSeq));
                if (stroperator.Trim() != string.Empty)
                    sqlParamList.Add(new SqlParameter("@AGYSERQ_ADD_OPERATOR", stroperator));
                if (strMode.Trim() != string.Empty)
                    sqlParamList.Add(new SqlParameter("@Mode", strMode));


                boolsuccess = Captain.DatabaseLayer.FieldControlsDB.CAPS_AGCYSERQASSOC_INSUPDEL(sqlParamList);


            }
            catch (Exception ex)
            {
                //
                return false;
            }

            return boolsuccess;
        }

        public List<AGCYSERQASSOCEntity> CAPS_AGCYSERQASSOC_GET(string strServicecode, string Quescode, string Mode)
        {
            List<AGCYSERQASSOCEntity> AGCYSERQASSOCEntity = new List<AGCYSERQASSOCEntity>();
            try
            {
                DataSet TableData = FieldControlsDB.CAPS_AGCYSERQASSOC_GET(strServicecode, Quescode, Mode);
                if (TableData != null && TableData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in TableData.Tables[0].Rows)
                    {
                        AGCYSERQASSOCEntity.Add(new AGCYSERQASSOCEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return AGCYSERQASSOCEntity;
            }

            return AGCYSERQASSOCEntity;
        }

        public List<CustRespEntity> GetAGCYQuesRespByCode(string Quescode)
        {
            List<CustRespEntity> FLDCNTLProfile = new List<CustRespEntity>();
            try
            {
                DataSet FLDCNTLData = FieldControlsDB.CAPS_AGCYQUESRESP_GET(Quescode, string.Empty);
                ;
                if (FLDCNTLData != null && FLDCNTLData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in FLDCNTLData.Tables[0].Rows)
                    {
                        FLDCNTLProfile.Add(new CustRespEntity(row, string.Empty, string.Empty));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return FLDCNTLProfile;
            }

            return FLDCNTLProfile;
        }

        public bool UpdateAGCYQUESRESP(List<CustRespEntity> RespList, string New_Ques_Code)
        {
            bool boolsuccess = false;

            try
            {
                foreach (CustRespEntity Entity in RespList)
                {
                    if (Entity.Changed == "Y" && !(string.IsNullOrEmpty(Entity.DescCode)) && !(string.IsNullOrEmpty(Entity.RespDesc)))
                    {
                        List<SqlParameter> sqlParamList = new List<SqlParameter>();
                        sqlParamList.Add(new SqlParameter("@Row_Type", Entity.RecType));
                        sqlParamList.Add(new SqlParameter("@AGYQ_RESP_QCODE", New_Ques_Code));
                        sqlParamList.Add(new SqlParameter("@AGYQ_RESP_SEQ", int.Parse(Entity.RespSeq)));
                        sqlParamList.Add(new SqlParameter("@AGYQ_RESP_DESC", Entity.RespDesc));
                        sqlParamList.Add(new SqlParameter("@AGYQ_RESP_CODE", Entity.DescCode));
                        sqlParamList.Add(new SqlParameter("@AGYQ_RESP_ADD_OPERATOR", Entity.AddOpr));
                        sqlParamList.Add(new SqlParameter("@AGYQ_RESP_LSTC_OPERATOR", Entity.ChgOpr));

                        boolsuccess = Captain.DatabaseLayer.FieldControlsDB.CAPS_AGCYQUESRESP_INSUPDEL(sqlParamList);
                    }
                }

            }
            catch (Exception ex)
            {
                //
                return false;
            }

            return boolsuccess;
        }
    }
}
