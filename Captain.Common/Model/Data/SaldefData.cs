using Captain.Common.Model.Objects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Captain.Common.Model.Data
{
    public class SaldefData
    {
        public SaldefData(CaptainModel model)
        {
            Model = model;
        }

        #region Properties

        public CaptainModel Model { get; set; }

        public string UserId { get; set; }

        #endregion

        public List<SaldefEntity> Browse_SALDEF(SaldefEntity Entity, string Opretaion_Mode,string UserID,string AdminAgency)
        {
            List<SaldefEntity> CASESPMProfile = new List<SaldefEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_SALDEF_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet CASESPMData = Captain.DatabaseLayer.SALDB.Browse_SALDEF(sqlParamList,UserID, AdminAgency);

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASESPMProfile.Add(new SaldefEntity(row, string.Empty));
                }
            }
            catch (Exception ex)
            { return CASESPMProfile; }

            return CASESPMProfile;
        }

        public List<SqlParameter> Prepare_SALDEF_SqlParameters_List(SaldefEntity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                if (Opretaion_Mode != "Browse")
                    sqlParamList.Add(new SqlParameter("@Mode", Entity.Mode));

                sqlParamList.Add(new SqlParameter("@IP_ID", Entity.SALD_ID));
                sqlParamList.Add(new SqlParameter("@IP_TYPE", Entity.SALD_TYPE));
                sqlParamList.Add(new SqlParameter("@IP_HIE", Entity.SALD_HIE));
                sqlParamList.Add(new SqlParameter("@IP_NAME", Entity.SALD_NAME));
                sqlParamList.Add(new SqlParameter("@IP_SPS", Entity.SALD_SPS));
                sqlParamList.Add(new SqlParameter("@IP_SERVICES", Entity.SALD_SERVICES));
                sqlParamList.Add(new SqlParameter("@IP_ACTIVE", Entity.SALD_ACTIVE));

                sqlParamList.Add(new SqlParameter("@IP_ADD_OPERATOR", Entity.SALD_ADD_OPERATOR));
                sqlParamList.Add(new SqlParameter("@IP_LSTC_OPERATOR", Entity.SALD_LSTC_OPERATOR));

                if (Opretaion_Mode == "Browse")
                {
                    sqlParamList.Add(new SqlParameter("@IP_DATE_ADD", Entity.SALD_DATE_ADD));
                    sqlParamList.Add(new SqlParameter("@IP_DATE_LSTC", Entity.SALD_DATE_LSTC));
                }

            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }

        public List<SalquesEntity> Browse_SALQUES(SalquesEntity Entity, string Opretaion_Mode)
        {
            List<SalquesEntity> CASESPMProfile = new List<SalquesEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_SALQUES_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet CASESPMData = Captain.DatabaseLayer.SALDB.Browse_SALQUES(sqlParamList);

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASESPMProfile.Add(new SalquesEntity(row, string.Empty));
                }
            }
            catch (Exception ex)
            { return CASESPMProfile; }

            return CASESPMProfile;
        }

        public List<SqlParameter> Prepare_SALQUES_SqlParameters_List(SalquesEntity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                if (Opretaion_Mode != "Browse")
                    sqlParamList.Add(new SqlParameter("@Mode", Entity.Mode));

                sqlParamList.Add(new SqlParameter("@IP_ID", Entity.SALQ_ID));
                sqlParamList.Add(new SqlParameter("@IP_SALD_ID", Entity.SALQ_SALD_ID));
                sqlParamList.Add(new SqlParameter("@IP_GRP_CODE", Entity.SALQ_GRP_CODE));
                sqlParamList.Add(new SqlParameter("@IP_GRP_SEQ", Entity.SALQ_GRP_SEQ));
                sqlParamList.Add(new SqlParameter("@IP_SEQ", Entity.SALQ_SEQ));
                // sqlParamList.Add(new SqlParameter("@IP_CODE", Entity.SALQ_CODE));
                sqlParamList.Add(new SqlParameter("@IP_TYPE", Entity.SALQ_TYPE));

                sqlParamList.Add(new SqlParameter("@IP_DESC", Entity.SALQ_DESC));
                sqlParamList.Add(new SqlParameter("@IP_ADD_OPERATOR", Entity.SALQ_ADD_OPERATOR));
                sqlParamList.Add(new SqlParameter("@IP_LSTC_OPERATOR", Entity.SALQ_LSTC_OPERATOR));

                if (Opretaion_Mode == "Browse")
                {
                    sqlParamList.Add(new SqlParameter("@IP_DATE_ADD", Entity.SALQ_DATE_ADD));
                    sqlParamList.Add(new SqlParameter("@IP_DATE_LSTC", Entity.SALQ_DATE_LSTC));
                }

            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }

        public List<SalqrespEntity> Browse_SALQRESP(SalqrespEntity Entity, string Opretaion_Mode)
        {
            List<SalqrespEntity> CASESPMProfile = new List<SalqrespEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_SALQRESP_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet CASESPMData = Captain.DatabaseLayer.SALDB.Browse_SALQRESP(sqlParamList);

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASESPMProfile.Add(new SalqrespEntity(row, string.Empty));
                }
            }
            catch (Exception ex)
            { return CASESPMProfile; }

            return CASESPMProfile;
        }

        public List<SqlParameter> Prepare_SALQRESP_SqlParameters_List(SalqrespEntity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                if (Opretaion_Mode != "Browse")
                    sqlParamList.Add(new SqlParameter("@Mode", Entity.Mode));

                sqlParamList.Add(new SqlParameter("@IP_Q_ID", Entity.SALQR_Q_ID));
                sqlParamList.Add(new SqlParameter("@IP_CODE", Entity.SALQR_CODE));
                sqlParamList.Add(new SqlParameter("@IP_SEQ", Entity.SALQR_SEQ));
                sqlParamList.Add(new SqlParameter("@IP_DESC", Entity.SALQR_DESC));


                sqlParamList.Add(new SqlParameter("@IP_ADD_OPERATOR", Entity.SALQR_ADD_OPERATOR));
                sqlParamList.Add(new SqlParameter("@IP_LSTC_OPERATOR", Entity.SALQR_LSTC_OPERATOR));

                if (Opretaion_Mode == "Browse")
                {
                    sqlParamList.Add(new SqlParameter("@IP_DATE_ADD", Entity.SALQR_DATE_ADD));
                    sqlParamList.Add(new SqlParameter("@IP_DATE_LSTC", Entity.SALQR_DATE_LSTC));
                }

            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }

        public List<SaldefEntity> Browse_Saldef(string Agency, string Dept, string Prog, string Appno, string Type)
        {
            List<SaldefEntity> SaldefEntityList = new List<SaldefEntity>();
            try
            {
                DataSet arsData = Captain.DatabaseLayer.ARSDB.Browse_ARSCUST(Agency, Dept, Prog, Appno, Type);
                if (arsData != null && arsData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in arsData.Tables[0].Rows)
                    {
                        SaldefEntityList.Add(new SaldefEntity(row, string.Empty));
                    }
                }
            }
            catch (Exception ex)
            {
                return SaldefEntityList;
            }

            return SaldefEntityList;
        }

        public List<SALACTEntity> Browse_SALACT(SALACTEntity Entity, string Opretaion_Mode)
        {
            List<SALACTEntity> CASESPMProfile = new List<SALACTEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_SALACT_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet CASESPMData = Captain.DatabaseLayer.SALDB.Browse_SALACT(sqlParamList);

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASESPMProfile.Add(new SALACTEntity(row, string.Empty));
                }
            }
            catch (Exception ex)
            { return CASESPMProfile; }

            return CASESPMProfile;
        }

        public List<SqlParameter> Prepare_SALACT_SqlParameters_List(SALACTEntity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                if (Opretaion_Mode != "Browse")
                    sqlParamList.Add(new SqlParameter("@Mode", Entity.Mode));

                sqlParamList.Add(new SqlParameter("@IP_ID", Entity.SALACT_ID));
                sqlParamList.Add(new SqlParameter("@IP_SALID", Entity.SALACT_SALID));
                sqlParamList.Add(new SqlParameter("@IP_Q_ID", Entity.SALACT_Q_ID));
                sqlParamList.Add(new SqlParameter("@IP_TYPE", Entity.SALACT_TYPE));
                sqlParamList.Add(new SqlParameter("@IP_STATUS", Entity.SALACT_STATUS));
                sqlParamList.Add(new SqlParameter("@IP_LOCATION", Entity.SALACT_LOCATION));
                sqlParamList.Add(new SqlParameter("@IP_RECIPIENT", Entity.SALACT_RECIPIENT));
                sqlParamList.Add(new SqlParameter("@IP_ATTN", Entity.SALACT_ATTN));
                sqlParamList.Add(new SqlParameter("@IP_Q_TYPE", Entity.SALACT_Q_TYPE));
                sqlParamList.Add(new SqlParameter("@IP_SEQ", Entity.SALACT_SEQ));
                sqlParamList.Add(new SqlParameter("@IP_NUM_RESP", Entity.SALACT_NUM_RESP));
                sqlParamList.Add(new SqlParameter("@IP_DATE_RESP", Entity.SALACT_DATE_RESP));
                sqlParamList.Add(new SqlParameter("@IP_MULTI_RESP", Entity.SALACT_MULTI_RESP));


                sqlParamList.Add(new SqlParameter("@IP_ADD_OPERATOR", Entity.SALACT_ADD_OPERATOR));
                sqlParamList.Add(new SqlParameter("@IP_LSTC_OPERATOR", Entity.SALACT_LSTC_OPERATOR));

                if (Opretaion_Mode == "Browse")
                {
                    sqlParamList.Add(new SqlParameter("@IP_DATE_ADD", Entity.SALACT_DATE_ADD));
                    sqlParamList.Add(new SqlParameter("@IP_DATE_LSTC", Entity.SALACT_DATE_LSTC));
                }

            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }

        public List<CALCONTEntity> Browse_CALCONT(CALCONTEntity Entity, string Opretaion_Mode)
        {
            List<CALCONTEntity> CASESPMProfile = new List<CALCONTEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_CALCONT_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet CASESPMData = Captain.DatabaseLayer.SALDB.Browse_CALCONT(sqlParamList);

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASESPMProfile.Add(new CALCONTEntity(row, string.Empty));
                }
            }
            catch (Exception ex)
            { return CASESPMProfile; }

            return CASESPMProfile;
        }

        public List<SqlParameter> Prepare_CALCONT_SqlParameters_List(CALCONTEntity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                if (Opretaion_Mode != "Browse")
                    sqlParamList.Add(new SqlParameter("@Mode", Entity.Mode));

                sqlParamList.Add(new SqlParameter("@IP_ID", Entity.CALCONT_ID));
                sqlParamList.Add(new SqlParameter("@IP_SALID", Entity.CALCONT_SALID));
                sqlParamList.Add(new SqlParameter("@IP_Q_ID", Entity.CALCONT_Q_ID));
                sqlParamList.Add(new SqlParameter("@IP_Q_TYPE", Entity.CALCONT_Q_TYPE));
                sqlParamList.Add(new SqlParameter("@IP_SEQ", Entity.CALCONT_SEQ));
                sqlParamList.Add(new SqlParameter("@IP_NUM_RESP", Entity.CALCONT_NUM_RESP));
                sqlParamList.Add(new SqlParameter("@IP_DATE_RESP", Entity.CALCONT_DATE_RESP));
                sqlParamList.Add(new SqlParameter("@IP_MULTI_RESP", Entity.CALCONT_MULTI_RESP));


                sqlParamList.Add(new SqlParameter("@IP_ADD_OPERATOR", Entity.CALCONT_ADD_OPERATOR));
                sqlParamList.Add(new SqlParameter("@IP_LSTC_OPERATOR", Entity.CALCONT_LSTC_OPERATOR));

                if (Opretaion_Mode == "Browse")
                {
                    sqlParamList.Add(new SqlParameter("@IP_DATE_ADD", Entity.CALCONT_DATE_ADD));
                    sqlParamList.Add(new SqlParameter("@IP_DATE_LSTC", Entity.CALCONT_DATE_LSTC));
                }

            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }


        public bool CAP_SALDEF_INSUPDEL(SaldefEntity Entity, out string Msg, out string Sql_Reslut_Msg)
        {
            bool boolStatus = false;
            Sql_Reslut_Msg = "Success";
            Msg = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                if (Entity.SALD_ID != string.Empty)
                    sqlParamList.Add(new SqlParameter("@IP_ID", Entity.SALD_ID));
                sqlParamList.Add(new SqlParameter("@IP_TYPE", Entity.SALD_TYPE));
                sqlParamList.Add(new SqlParameter("@IP_HIE", Entity.SALD_HIE));
                sqlParamList.Add(new SqlParameter("@IP_NAME", Entity.SALD_NAME));
                sqlParamList.Add(new SqlParameter("@IP_SPS", Entity.SALD_SPS));
                sqlParamList.Add(new SqlParameter("@IP_SERVICES", Entity.SALD_SERVICES));
                sqlParamList.Add(new SqlParameter("@IP_ACTIVE", Entity.SALD_ACTIVE));

                sqlParamList.Add(new SqlParameter("@IP_ADD_OPERATOR", Entity.SALD_ADD_OPERATOR));
                sqlParamList.Add(new SqlParameter("@IP_LSTC_OPERATOR", Entity.SALD_LSTC_OPERATOR));
                sqlParamList.Add(new SqlParameter("@IP_BOILERPLATE", Entity.SALD_BOILERPLATE));
                sqlParamList.Add(new SqlParameter("@IP_SIGN_REQURED", Entity.SALD_SIGN_REQURED));

                if(!string.IsNullOrEmpty(Entity.SALD_5QUEST.Trim()))
                    sqlParamList.Add(new SqlParameter("@IP_5QUESTS", Entity.SALD_5QUEST));


                if (Entity.Mode != string.Empty)
                    sqlParamList.Add(new SqlParameter("@Mode", Entity.Mode));

                SqlParameter DeleteMsg = new SqlParameter("@msg", SqlDbType.VarChar, 50);
                DeleteMsg.Direction = ParameterDirection.Output;
                sqlParamList.Add(DeleteMsg);

                boolStatus = Captain.DatabaseLayer.SALDB.CAP_SALDEF_INSUPDEL(sqlParamList, out Sql_Reslut_Msg);

                Msg = DeleteMsg.Value.ToString();
            }

            catch (Exception ex)
            {
                boolStatus = false;
            }

            return boolStatus;
        }

        public bool CAP_SALQUES_INSUPDEL(SalquesEntity Entity, string strType, out string stroutQID)
        {
            bool boolStatus = false;
            string stroutputid = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();



                SqlParameter sqlQId = new SqlParameter("@IP_ID", SqlDbType.VarChar, 10);
                sqlQId.Value = Entity.SALQ_ID;
                sqlQId.Direction = ParameterDirection.InputOutput;
                sqlParamList.Add(sqlQId);

                if (Entity.SALQ_SALD_ID != string.Empty)
                    sqlParamList.Add(new SqlParameter("@IP_SALD_ID", Entity.SALQ_SALD_ID));
                if (Entity.SALQ_GRP_CODE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@IP_GRP_CODE", Entity.SALQ_GRP_CODE));
                if (Entity.SALQ_GRP_SEQ != string.Empty)
                    sqlParamList.Add(new SqlParameter("@IP_GRP_SEQ", Entity.SALQ_GRP_SEQ));
                if (Entity.SALQ_SEQ != string.Empty)
                    sqlParamList.Add(new SqlParameter("@IP_SEQ", Entity.SALQ_SEQ));
                //if (Entity.SALQ_CODE != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@IP_CODE", Entity.SALQ_CODE));
                if (Entity.SALQ_TYPE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@IP_TYPE", Entity.SALQ_TYPE));
                if (Entity.SALQ_DESC != string.Empty)
                    sqlParamList.Add(new SqlParameter("@IP_DESC", Entity.SALQ_DESC));
                if (Entity.SALQ_REQ != string.Empty)
                    sqlParamList.Add(new SqlParameter("@IP_REQ", Entity.SALQ_REQ));
                sqlParamList.Add(new SqlParameter("@IP_ADD_OPERATOR", Entity.SALQ_ADD_OPERATOR));
                sqlParamList.Add(new SqlParameter("@IP_LSTC_OPERATOR", Entity.SALQ_LSTC_OPERATOR));
                sqlParamList.Add(new SqlParameter("@Type", strType));


                if (Entity.Mode != string.Empty)
                    sqlParamList.Add(new SqlParameter("@Mode", Entity.Mode));

                boolStatus = Captain.DatabaseLayer.SALDB.CAP_SALQUES_INSUPDEL(sqlParamList);
                stroutputid = sqlQId.Value.ToString();
            }

            catch (Exception ex)
            {
                boolStatus = false;
            }
            stroutQID = stroutputid;
            return boolStatus;
        }


        public bool CAP_SALQRESP_INSUPDEL(SalqrespEntity Entity)
        {
            bool boolStatus = false;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();


                sqlParamList.Add(new SqlParameter("@IP_Q_ID", Entity.SALQR_Q_ID));
                sqlParamList.Add(new SqlParameter("@IP_CODE", Entity.SALQR_CODE));
                sqlParamList.Add(new SqlParameter("@IP_SEQ", Entity.SALQR_SEQ));
                sqlParamList.Add(new SqlParameter("@IP_DESC", Entity.SALQR_DESC));


                sqlParamList.Add(new SqlParameter("@IP_ADD_OPERATOR", Entity.SALQR_ADD_OPERATOR));
                sqlParamList.Add(new SqlParameter("@IP_LSTC_OPERATOR", Entity.SALQR_LSTC_OPERATOR));


                if (Entity.Mode != string.Empty)
                    sqlParamList.Add(new SqlParameter("@Mode", Entity.Mode));

                boolStatus = Captain.DatabaseLayer.SALDB.CAP_SALQRESP_INSUPDEL(sqlParamList);
            }

            catch (Exception ex)
            {
                boolStatus = false;
            }

            return boolStatus;
        }

        public bool CAP_SALACT_INSUPDEL(SALACTEntity Entity)
        {
            bool boolStatus = false;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();


                sqlParamList.Add(new SqlParameter("@IP_ID", Entity.SALACT_ID));
                sqlParamList.Add(new SqlParameter("@IP_SALID", Entity.SALACT_SALID));
                sqlParamList.Add(new SqlParameter("@IP_Q_ID", Entity.SALACT_Q_ID));
                sqlParamList.Add(new SqlParameter("@IP_TYPE", Entity.SALACT_TYPE));
                if (!string.IsNullOrEmpty(Entity.SALACT_STATUS.Trim()))
                    sqlParamList.Add(new SqlParameter("@IP_STATUS", Entity.SALACT_STATUS));
                if (!string.IsNullOrEmpty(Entity.SALACT_LOCATION.Trim()))
                    sqlParamList.Add(new SqlParameter("@IP_LOCATION", Entity.SALACT_LOCATION));
                if (!string.IsNullOrEmpty(Entity.SALACT_RECIPIENT.Trim()))
                    sqlParamList.Add(new SqlParameter("@IP_RECIPIENT", Entity.SALACT_RECIPIENT));
                if (!string.IsNullOrEmpty(Entity.SALACT_ATTN.Trim()))
                    sqlParamList.Add(new SqlParameter("@IP_ATTN", Entity.SALACT_ATTN));

                if (!string.IsNullOrEmpty(Entity.SALACT_TIME_IN.Trim()))
                    sqlParamList.Add(new SqlParameter("@IP_TIME_IN", Entity.SALACT_TIME_IN));
                if (!string.IsNullOrEmpty(Entity.SALACT_TIME_OUT.Trim()))
                    sqlParamList.Add(new SqlParameter("@IP_TIME_OUT", Entity.SALACT_TIME_OUT));
                if (!string.IsNullOrEmpty(Entity.SALACT_TIME_SPENT.Trim()))
                    sqlParamList.Add(new SqlParameter("@IP_TIME_SPENT", Entity.SALACT_TIME_SPENT));

                if (!string.IsNullOrEmpty(Entity.SALACT_Q_TYPE.Trim()))
                    sqlParamList.Add(new SqlParameter("@IP_Q_TYPE", Entity.SALACT_Q_TYPE));
                if (!string.IsNullOrEmpty(Entity.SALACT_SEQ.Trim()))
                    sqlParamList.Add(new SqlParameter("@IP_SEQ", Entity.SALACT_SEQ));
                if (!string.IsNullOrEmpty(Entity.SALACT_NUM_RESP.Trim()))
                    sqlParamList.Add(new SqlParameter("@IP_NUM_RESP", Entity.SALACT_NUM_RESP));
                if (!string.IsNullOrEmpty(Entity.SALACT_DATE_RESP.Trim()))
                    sqlParamList.Add(new SqlParameter("@IP_DATE_RESP", Entity.SALACT_DATE_RESP));
                if (!string.IsNullOrEmpty(Entity.SALACT_MULTI_RESP.Trim()))
                    sqlParamList.Add(new SqlParameter("@IP_MULTI_RESP", Entity.SALACT_MULTI_RESP));


                sqlParamList.Add(new SqlParameter("@IP_ADD_OPERATOR", Entity.SALACT_ADD_OPERATOR));
                sqlParamList.Add(new SqlParameter("@IP_LSTC_OPERATOR", Entity.SALACT_LSTC_OPERATOR));


                if (Entity.Mode != string.Empty)
                    sqlParamList.Add(new SqlParameter("@Mode", Entity.Mode));

                boolStatus = Captain.DatabaseLayer.SALDB.CAP_SALACT_INSUPDEL(sqlParamList);
            }

            catch (Exception ex)
            {
                boolStatus = false;
            }

            return boolStatus;
        }

        public bool CAP_CALCONT_INSUPDEL(CALCONTEntity Entity)
        {
            bool boolStatus = false;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();


                sqlParamList.Add(new SqlParameter("@IP_ID", Entity.CALCONT_ID));
                sqlParamList.Add(new SqlParameter("@IP_SALID", Entity.CALCONT_SALID));
                sqlParamList.Add(new SqlParameter("@IP_Q_ID", Entity.CALCONT_Q_ID));
                
                if (!string.IsNullOrEmpty(Entity.CALCONT_Q_TYPE.Trim()))
                    sqlParamList.Add(new SqlParameter("@IP_Q_TYPE", Entity.CALCONT_Q_TYPE));
                if (!string.IsNullOrEmpty(Entity.CALCONT_SEQ.Trim()))
                    sqlParamList.Add(new SqlParameter("@IP_SEQ", Entity.CALCONT_SEQ));
                if (!string.IsNullOrEmpty(Entity.CALCONT_NUM_RESP.Trim()))
                    sqlParamList.Add(new SqlParameter("@IP_NUM_RESP", Entity.CALCONT_NUM_RESP));
                if (!string.IsNullOrEmpty(Entity.CALCONT_DATE_RESP.Trim()))
                    sqlParamList.Add(new SqlParameter("@IP_DATE_RESP", Entity.CALCONT_DATE_RESP));
                if (!string.IsNullOrEmpty(Entity.CALCONT_MULTI_RESP.Trim()))
                    sqlParamList.Add(new SqlParameter("@IP_MULTI_RESP", Entity.CALCONT_MULTI_RESP));


                sqlParamList.Add(new SqlParameter("@IP_ADD_OPERATOR", Entity.CALCONT_ADD_OPERATOR));
                sqlParamList.Add(new SqlParameter("@IP_LSTC_OPERATOR", Entity.CALCONT_LSTC_OPERATOR));


                if (Entity.Mode != string.Empty)
                    sqlParamList.Add(new SqlParameter("@Mode", Entity.Mode));

                boolStatus = Captain.DatabaseLayer.SALDB.CAP_CALCONT_INSUPDEL(sqlParamList);
            }

            catch (Exception ex)
            {
                boolStatus = false;
            }

            return boolStatus;
        }

        public List<SALQLNKEntity> Browse_SALQLNK(string strQuestion, string strGroup, string strQlnk)
        {
            List<SALQLNKEntity> _salqlinkEntity = new List<SALQLNKEntity>();
            try
            {

                DataSet CASESPMData = Captain.DatabaseLayer.SALDB.Browse_SALQLNK(strQuestion, strGroup, strQlnk);

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        _salqlinkEntity.Add(new SALQLNKEntity(row, string.Empty));
                }
            }
            catch (Exception ex)
            { return _salqlinkEntity; }

            return _salqlinkEntity;
        }

        public bool CAP_SALQLNK_INSUPDEL(string strMode, string strQid, string strGroup, string strLnkQid, string strReq,  string strEnable, string strDisable, string strLstcoperator)
        {
            bool boolStatus = false;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@Mode", strMode));
                sqlParamList.Add(new SqlParameter("@IP_Q_ID", strQid));
                if (strGroup != string.Empty)
                    sqlParamList.Add(new SqlParameter("@IP_GROUP", strGroup));
                if (strLnkQid != string.Empty)
                    sqlParamList.Add(new SqlParameter("@IP_LINKQ", strLnkQid));
                if (strReq != string.Empty)
                    sqlParamList.Add(new SqlParameter("@IP_REQ", strReq));              
                if (strEnable != string.Empty)
                    sqlParamList.Add(new SqlParameter("@IP_ENABLE", strEnable));
                if (strDisable != string.Empty)
                    sqlParamList.Add(new SqlParameter("@IP_DISABLE", strDisable));
                if (strLstcoperator != string.Empty)
                    sqlParamList.Add(new SqlParameter("@IP_LSTC_OPERATOR", strLstcoperator));


                boolStatus = Captain.DatabaseLayer.SALDB.CAP_SALQLNK_INSUPDEL(sqlParamList);
            }

            catch (Exception ex)
            {
                boolStatus = false;
            }

            return boolStatus;
        }

    }
}
