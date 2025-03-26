using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Captain.DatabaseLayer;
using Captain.Common.Model.Objects;
using System.Data.SqlClient;
using System.Data;
using Captain.Common.Utilities;
using System.ComponentModel;


namespace Captain.Common.Model.Data
{
    public class MatrixScalesData
    {

        public MatrixScalesData()
        {

        }

        #region Properties

        public CaptainModel Model { get; set; }

        #endregion


        //    **************************************************************************************************************************   Yeswanth
        public List<MATDEFEntity> Browse_MATDEF(MATDEFEntity Entity, string Opretaion_Mode)
        {
            List<MATDEFEntity> CASESPMProfile = new List<MATDEFEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_MATDEF_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet CASESPMData = Captain.DatabaseLayer.MatrixDB.Browse_Selected_Table(sqlParamList, "MATDEF");

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASESPMProfile.Add(new MATDEFEntity(row));
                }
            }
            catch (Exception ex)
            { return CASESPMProfile; }

            return CASESPMProfile;
        }


        public List<SqlParameter> Prepare_MATDEF_SqlParameters_List(MATDEFEntity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                if (Opretaion_Mode != "Browse")
                {
                    sqlParamList.Add(new SqlParameter("@Row_Type", Entity.Rec_Type));
                    sqlParamList.Add(new SqlParameter("@Rec_mode", Entity.Rec_mode));

                }
                sqlParamList.Add(new SqlParameter("@MATDEF_MAT_CODE", Entity.Mat_Code));
                if (!string.IsNullOrEmpty(Entity.Scale_Code))
                    sqlParamList.Add(new SqlParameter("@MATDEF_SCL_CODE", int.Parse(Entity.Scale_Code)));
                sqlParamList.Add(new SqlParameter("@MATDEF_DESC", Entity.Desc));
                if (!string.IsNullOrEmpty(Entity.Rationale))
                    sqlParamList.Add(new SqlParameter("@MATDEF_RATIONALE", Entity.Rationale));
                if (!string.IsNullOrEmpty(Entity.Sequence))
                    sqlParamList.Add(new SqlParameter("@MATDEF_SEQUENCE", Entity.Sequence));
                if (!string.IsNullOrEmpty(Entity.Override))
                    sqlParamList.Add(new SqlParameter("@MATDEF_SS_OVERIDE", Entity.Override));
                if (!string.IsNullOrEmpty(Entity.Benchmark))
                    sqlParamList.Add(new SqlParameter("@MATDEF_CALC_BMARK", Entity.Benchmark));
                sqlParamList.Add(new SqlParameter("@MATDEF_ACTIVE", Entity.Active));
                sqlParamList.Add(new SqlParameter("@MATDEF_SCORE_SHEET", Entity.Score));
                sqlParamList.Add(new SqlParameter("@MATDEF_SHOW_BA", Entity.Show_BA));

                if (!string.IsNullOrEmpty(Entity.Date_option))
                    sqlParamList.Add(new SqlParameter("@MATDEF_SS_DATE_OPT", Entity.Date_option));

                sqlParamList.Add(new SqlParameter("@MATDEF_INTERVAL", Entity.Interval));
                sqlParamList.Add(new SqlParameter("@MATDEF_LSTC_OPERATOR", Entity.Lstc_Operator));
                if (!string.IsNullOrEmpty(Entity.Assessment_Type))
                    sqlParamList.Add(new SqlParameter("@MATDEF_SCL_ASSMT_TYPE", Entity.Assessment_Type));
                if (!string.IsNullOrEmpty(Entity.OverlScor))
                    sqlParamList.Add(new SqlParameter("@MATDEF_OVRLSCOR", Entity.OverlScor));
                if (!string.IsNullOrEmpty(Entity.SpecScor))
                    sqlParamList.Add(new SqlParameter("@MATDEF_SPECSCOR", Entity.SpecScor));
                if (!string.IsNullOrEmpty(Entity.GroupCode))
                    sqlParamList.Add(new SqlParameter("@MATDEF_GRP_CODE", Entity.GroupCode));

                if (!string.IsNullOrEmpty(Entity.Copy_Prassmnt))
                    sqlParamList.Add(new SqlParameter("@MATDEF_COPY_PRASSMNT", Entity.Copy_Prassmnt));

                if (!string.IsNullOrEmpty(Entity.Prog_Method))
                    sqlParamList.Add(new SqlParameter("@MATDEF_PROG_METHD", Entity.Prog_Method));


                if (Opretaion_Mode == "Browse")
                {
                    sqlParamList.Add(new SqlParameter("@MATDEF_DATE_ADD", Entity.Add_Date));
                    sqlParamList.Add(new SqlParameter("@MATDEF_ADD_OPERATOR", Entity.Add_Operator));
                    sqlParamList.Add(new SqlParameter("@MATDEF_DATE_LSTC", Entity.Lstc_Date));
                }
            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }


        public List<MATASMTEntity> Browse_MATASMT(MATASMTEntity Entity, string Opretaion_Mode)
        {
            List<MATASMTEntity> MATSGRPProfile = new List<MATASMTEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_MATASMT_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet MATSGRPData = Captain.DatabaseLayer.MatrixDB.Browse_Selected_Table(sqlParamList, "MATASMT");

                if (MATSGRPData != null && MATSGRPData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in MATSGRPData.Tables[0].Rows)
                        MATSGRPProfile.Add(new MATASMTEntity(row, "Browse"));
                }
            }
            catch (Exception ex)
            { return MATSGRPProfile; }

            return MATSGRPProfile;
        }

        public List<SqlParameter> Prepare_MATASMT_SqlParameters_List(MATASMTEntity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                //if (Opretaion_Mode != "Browse")
                //    sqlParamList.Add(new SqlParameter("@Row_Type", Entity.Rec_Type));

                sqlParamList.Add(new SqlParameter("@MATASMT_AGENCY", Entity.Agency));
                sqlParamList.Add(new SqlParameter("@MATASMT_DEPT", Entity.Dept));
                sqlParamList.Add(new SqlParameter("@MATASMT_PROGRAM", Entity.Program));
                if(!string.IsNullOrEmpty(Entity.Year.Trim()))
                    sqlParamList.Add(new SqlParameter("@MATASMT_YEAR", Entity.Year));
                sqlParamList.Add(new SqlParameter("@MATASMT_APP", Entity.App));
                sqlParamList.Add(new SqlParameter("@MATASMT_MAT_CODE", Entity.MatCode));
                sqlParamList.Add(new SqlParameter("@MATASMT_SCL_CODE", Entity.SclCode));
                sqlParamList.Add(new SqlParameter("@MATASMT_TYPE", Entity.Type));
                sqlParamList.Add(new SqlParameter("@MATASMT_SS_DATE", Entity.SSDate));
                sqlParamList.Add(new SqlParameter("@MATASMT_QUES_CODE", Entity.QuesCode));
                sqlParamList.Add(new SqlParameter("@MATASMT_RESP_CODE", Entity.RespCode));
                sqlParamList.Add(new SqlParameter("@MATASMT_RESP_DESC", Entity.RespDesc));
                sqlParamList.Add(new SqlParameter("@MATASMT_POINTS", Entity.Points));
                sqlParamList.Add(new SqlParameter("@MATASMT_CHANGE", Entity.Change));
                sqlParamList.Add(new SqlParameter("@MATASMT_PN", Entity.Pn));
                sqlParamList.Add(new SqlParameter("@MATASMT_SIX_REASONS", Entity.SixReasons));
                sqlParamList.Add(new SqlParameter("@MATASMT_BYPASS", Entity.ByPass));

            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }




        public List<MATSGRPEntity> Browse_MATSGRP(MATSGRPEntity Entity, string Opretaion_Mode)
        {
            List<MATSGRPEntity> MATSGRPProfile = new List<MATSGRPEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_MATSGRP_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet MATSGRPData = Captain.DatabaseLayer.MatrixDB.Browse_Selected_Table(sqlParamList, "MATSGRP");

                if (MATSGRPData != null && MATSGRPData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in MATSGRPData.Tables[0].Rows)
                        MATSGRPProfile.Add(new MATSGRPEntity(row));
                }
            }
            catch (Exception ex)
            { return MATSGRPProfile; }

            return MATSGRPProfile;
        }




        public List<SqlParameter> Prepare_MATSGRP_SqlParameters_List(MATSGRPEntity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                if (Opretaion_Mode != "Browse")
                    sqlParamList.Add(new SqlParameter("@Row_Type", Entity.Rec_Type));

                sqlParamList.Add(new SqlParameter("@MATSGRP_MAT_CODE", Entity.MatCode));
                sqlParamList.Add(new SqlParameter("@MATSGRP_SCL_CODE", Entity.SclCode));
                sqlParamList.Add(new SqlParameter("@MATSGRP_CODE", Entity.Group));
                sqlParamList.Add(new SqlParameter("@MATSGRP_DESC", Entity.Desc));
                sqlParamList.Add(new SqlParameter("@MATSGRP_TYPE", Entity.GrpType));
                sqlParamList.Add(new SqlParameter("@MATSGRP_SEQ", Entity.Seq));
            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }

        public List<MATQUESTEntity> Browse_MATQUEST(MATQUESTEntity Entity, string Opretaion_Mode)
        {
            List<MATQUESTEntity> MATQUESTProfile = new List<MATQUESTEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_MATQUEST_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet MATQUESTData = Captain.DatabaseLayer.MatrixDB.Browse_Selected_Table(sqlParamList, "MATQUEST");

                if (MATQUESTData != null && MATQUESTData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in MATQUESTData.Tables[0].Rows)
                        MATQUESTProfile.Add(new MATQUESTEntity(row, string.Empty));
                }
            }
            catch (Exception ex)
            { return MATQUESTProfile; }

            return MATQUESTProfile;
        }


        public List<SqlParameter> Prepare_MATQUEST_SqlParameters_List(MATQUESTEntity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                if (Opretaion_Mode != "Browse")
                    sqlParamList.Add(new SqlParameter("@Row_Type", Entity.Rec_Type));

                sqlParamList.Add(new SqlParameter("@MATQUEST_MAT_CODE", Entity.MatCode));
                sqlParamList.Add(new SqlParameter("@MATQUEST_SCL_CODE", Entity.SclCode));
                sqlParamList.Add(new SqlParameter("@MATQUEST_GROUP", Entity.Group));
                sqlParamList.Add(new SqlParameter("@MATQUEST_CODE", Entity.Code));
                sqlParamList.Add(new SqlParameter("@MATQUEST_SEQ", Entity.Seq));
                sqlParamList.Add(new SqlParameter("@MATQUEST_DESC", Entity.Desc));
                sqlParamList.Add(new SqlParameter("@MATQUEST_TYPE", Entity.Type));
                sqlParamList.Add(new SqlParameter("@MATQUEST_SS_OVERRIDE", Entity.SsOverride));

                sqlParamList.Add(new SqlParameter("@MATQUEST_LSTC_OPERATOR", Entity.LstcOperator));
                if(Entity.QuesNumericType!=string.Empty)
                    sqlParamList.Add(new SqlParameter("@MATQUEST_NTYPE", Entity.QuesNumericType));
                

                if (Opretaion_Mode == "Browse")
                {
                    sqlParamList.Add(new SqlParameter("@MATQUEST_DATE_ADD", Entity.AddDate));
                    sqlParamList.Add(new SqlParameter("@MATQUEST_ADD_OPERATOR", Entity.AddOperator));
                    sqlParamList.Add(new SqlParameter("@MATQUEST_DATE_LSTC", Entity.DateLstc));
                }
            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }

        public List<MATQUESREntity> Browse_MATQUESR(MATQUESREntity Entity, string Opretaion_Mode)
        {
            List<MATQUESREntity> MATQUESRProfile = new List<MATQUESREntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_MATQUESR_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet MATQUESRData = Captain.DatabaseLayer.MatrixDB.Browse_Selected_Table(sqlParamList, "MATQUESR");

                if (MATQUESRData != null && MATQUESRData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in MATQUESRData.Tables[0].Rows)
                        MATQUESRProfile.Add(new MATQUESREntity(row));
                }
            }
            catch (Exception ex)
            { return MATQUESRProfile; }

            return MATQUESRProfile;
        }


        public List<SqlParameter> Prepare_MATQUESR_SqlParameters_List(MATQUESREntity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                if (Opretaion_Mode != "Browse")
                    sqlParamList.Add(new SqlParameter("@Row_Type", Entity.Rec_Type));

                sqlParamList.Add(new SqlParameter("@MATQUESR_MAT_CODE", Entity.MatCode));
                sqlParamList.Add(new SqlParameter("@MATQUESR_SCL_CODE", Entity.SclCode));
                sqlParamList.Add(new SqlParameter("@MATQUESR_GROUP", Entity.Group));
                sqlParamList.Add(new SqlParameter("@MATQUESR_CODE", Entity.Code));
                //sqlParamList.Add(new SqlParameter("@MATQUESR_SEQ", Entity.Seq));
                sqlParamList.Add(new SqlParameter("@MATQUESR_RESP_CODE", Entity.RespCode));
                sqlParamList.Add(new SqlParameter("@MATQUESR_RESP_DESC", Entity.RespDesc));
                sqlParamList.Add(new SqlParameter("@MATQUESR_POINTS", Entity.Points));
                sqlParamList.Add(new SqlParameter("@MATQUESR_EXCLUDE", Entity.Exclude));
            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }

        public List<MATOUTREntity> Browse_MATOUTR(MATOUTREntity Entity, string Opretaion_Mode)
        {
            List<MATOUTREntity> MATOUTRProfile = new List<MATOUTREntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_MATOUTR_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet MATOUTCData = Captain.DatabaseLayer.MatrixDB.Browse_Selected_Table(sqlParamList, "MATOUTR");

                if (MATOUTCData != null && MATOUTCData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in MATOUTCData.Tables[0].Rows)
                        MATOUTRProfile.Add(new MATOUTREntity(row, null));
                }
            }
            catch (Exception ex)
            { return MATOUTRProfile; }

            return MATOUTRProfile;
        }

        public List<SqlParameter> Prepare_MATOUTR_SqlParameters_List(MATOUTREntity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                //if (Opretaion_Mode != "Browse")
                //sqlParamList.Add(new SqlParameter("@Row_Type", Entity.Rec_Type));

                sqlParamList.Add(new SqlParameter("@MATOUTR_MAT_CODE", Entity.MatCode));
                sqlParamList.Add(new SqlParameter("@MATOUTR_SCL_CODE", Entity.SclCode));
                sqlParamList.Add(new SqlParameter("@MATOUTR_BM_CODE", Entity.BmCode));
                sqlParamList.Add(new SqlParameter("@MATOUTR_CODE", Entity.Code));
                sqlParamList.Add(new SqlParameter("@MATOUTR_QUESTION", Entity.Question));
                sqlParamList.Add(new SqlParameter("@MATOUTR_RESP_CODE", Entity.RespCode));
                //sqlParamList.Add(new SqlParameter("@MATOUTC_CONDITION", Entity.Condition));
                //sqlParamList.Add(new SqlParameter("@MATOUTC_LSTC_OPERATOR", Entity.LstcOperator));

                if (Opretaion_Mode == "Browse")
                {
                    sqlParamList.Add(new SqlParameter("@MATOUTR_DATE_ADD", Entity.AddDate));
                    sqlParamList.Add(new SqlParameter("@MATOUTR_ADD_OPERATOR", Entity.AddOperator));
                    sqlParamList.Add(new SqlParameter("@MATOUTR_DATE_LSTC", Entity.DateLstc));
                }

            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }

        public List<SqlParameter> Prepare_MATDEFDT_SqlParameters_List(MATDEFDTEntity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                if (Opretaion_Mode != "Browse")
                    sqlParamList.Add(new SqlParameter("@Row_Type", Entity.Rec_Type));

                sqlParamList.Add(new SqlParameter("@MATDEFDT_MAT_CODE", Entity.MatCode));
                sqlParamList.Add(new SqlParameter("@MATDEFDT_DATE", Entity.MatDate));


                //if (Opretaion_Mode == "Browse")
                //{
                //    sqlParamList.Add(new SqlParameter("@MATOUTR_DATE_ADD", Entity.AddDate));
                //    sqlParamList.Add(new SqlParameter("@MATOUTR_ADD_OPERATOR", Entity.AddOperator));
                //    sqlParamList.Add(new SqlParameter("@MATOUTR_DATE_LSTC", Entity.DateLstc));
                //}

            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }

        public List<MATDEFDTEntity> Browse_MATDEFDT(MATDEFDTEntity Entity, string Opretaion_Mode)
        {
            List<MATDEFDTEntity> MATOUTCProfile = new List<MATDEFDTEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_MATDEFDT_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet MATOUTCData = Captain.DatabaseLayer.MatrixDB.Browse_Selected_Table(sqlParamList, "MATDEFDT");

                if (MATOUTCData != null && MATOUTCData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in MATOUTCData.Tables[0].Rows)
                        MATOUTCProfile.Add(new MATDEFDTEntity(row));
                }
            }
            catch (Exception ex)
            { return MATOUTCProfile; }

            return MATOUTCProfile;
        }


        public List<MATOUTCEntity> Browse_MATOUTC(MATOUTCEntity Entity, string Opretaion_Mode)
        {
            List<MATOUTCEntity> MATOUTCProfile = new List<MATOUTCEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_MATOUTC_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet MATOUTCData = Captain.DatabaseLayer.MatrixDB.Browse_Selected_Table(sqlParamList, "MATOUTC");

                if (MATOUTCData != null && MATOUTCData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in MATOUTCData.Tables[0].Rows)
                        MATOUTCProfile.Add(new MATOUTCEntity(row, null));
                }
            }
            catch (Exception ex)
            { return MATOUTCProfile; }

            return MATOUTCProfile;
        }

        public bool UpdateMATDEFDT(MATDEFDTEntity Entity, string Operation_Mode, out string Msg)
        {
            bool boolsuccess;
            Msg = Consts.Common.DB_Exception;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList = Prepare_MATDEFDT_SqlParameters_List(Entity, Operation_Mode);

                //SqlParameter DeleteMsg = new SqlParameter("@msg", SqlDbType.VarChar, 50);
                //DeleteMsg.Direction = ParameterDirection.Output;
                //sqlParamList.Add(DeleteMsg);

                boolsuccess = Captain.DatabaseLayer.MatrixDB.Update_Sel_Table(sqlParamList, "MATDEFDT", out Msg);

                //Msg = DeleteMsg.Value.ToString();
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }


        public List<SqlParameter> Prepare_MATOUTC_SqlParameters_List(MATOUTCEntity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                if (Opretaion_Mode != "Browse")
                    sqlParamList.Add(new SqlParameter("@Row_Type", Entity.Rec_Type));

                sqlParamList.Add(new SqlParameter("@MATOUTC_MAT_CODE", Entity.MatCode));
                sqlParamList.Add(new SqlParameter("@MATOUTC_SCL_CODE", Entity.SclCode));
                sqlParamList.Add(new SqlParameter("@MATOUTC_BM_CODE", Entity.BmCode));
                sqlParamList.Add(new SqlParameter("@MATOUTC_CODE", Entity.Code));
                sqlParamList.Add(new SqlParameter("@MATOUTC_DESC", Entity.Desc));
                sqlParamList.Add(new SqlParameter("@MATOUTC_POINTS", Entity.Points));
                sqlParamList.Add(new SqlParameter("@MATOUTC_CONDITION", Entity.Condition));
                sqlParamList.Add(new SqlParameter("@MATOUTC_LSTC_OPERATOR", Entity.LstcOperator));

                if (Opretaion_Mode == "Browse")
                {
                    sqlParamList.Add(new SqlParameter("@MATOUTC_DATE_ADD", Entity.AddDate));
                    sqlParamList.Add(new SqlParameter("@MATOUTC_ADD_OPERATOR", Entity.AddOperator));
                    sqlParamList.Add(new SqlParameter("@MATOUTC_DATE_LSTC", Entity.DateLstc));
                }

            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }

        public bool UpdateMATOUTC(MATOUTCEntity Entity, string Operation_Mode, out string Msg)
        {
            bool boolsuccess;
            Msg = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList = Prepare_MATOUTC_SqlParameters_List(Entity, Operation_Mode);

                SqlParameter DeleteMsg = new SqlParameter("@msg", SqlDbType.VarChar, 50);
                DeleteMsg.Direction = ParameterDirection.Output;
                sqlParamList.Add(DeleteMsg);

                boolsuccess = Captain.DatabaseLayer.MatrixDB.Update_Sel_Table(sqlParamList, "MATOUTC");

                Msg = DeleteMsg.Value.ToString();
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        public List<MATDEFBMEntity> Browse_MATDEFBM(MATDEFBMEntity Entity, string Opretaion_Mode)
        {
            List<MATDEFBMEntity> MATDEFBMProfile = new List<MATDEFBMEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_MATDEFBM_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet MATDEFBMData = Captain.DatabaseLayer.MatrixDB.Browse_Selected_Table(sqlParamList, "MATDEFBM");

                if (MATDEFBMData != null && MATDEFBMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in MATDEFBMData.Tables[0].Rows)
                        MATDEFBMProfile.Add(new MATDEFBMEntity(row));
                }
            }
            catch (Exception ex)
            { return MATDEFBMProfile; }

            return MATDEFBMProfile;
        }


        public List<SqlParameter> Prepare_MATDEFBM_SqlParameters_List(MATDEFBMEntity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                if (Opretaion_Mode != "Browse")
                {
                    sqlParamList.Add(new SqlParameter("@Row_Type", Entity.Rec_Type));
                    if (Entity.ScoreType != string.Empty)
                        sqlParamList.Add(new SqlParameter("@MATDEFBM_SCORE_TYPE", Entity.ScoreType));
                }

                sqlParamList.Add(new SqlParameter("@MATDEFBM_MAT_CODE", Entity.MatCode));
                sqlParamList.Add(new SqlParameter("@MATDEFBM_CODE", Entity.Code));
                sqlParamList.Add(new SqlParameter("@MATDEFBM_DESC", Entity.Desc));
                sqlParamList.Add(new SqlParameter("@MATDEFBM_LOW", Entity.Low));
                sqlParamList.Add(new SqlParameter("@MATDEFBM_HIGH", Entity.High));

                sqlParamList.Add(new SqlParameter("@MATDEFBM_MAT_LOW", Entity.Overall_Low));
                sqlParamList.Add(new SqlParameter("@MATDEFBM_MAT_HIGH", Entity.Overall_High));

                sqlParamList.Add(new SqlParameter("@MATDEFBM_PROGRESS", Entity.Progress));
                sqlParamList.Add(new SqlParameter("@MATDEFBM_CONTINUITY", Entity.Continuity));




            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }


        public bool UpdateMATDEFBM(MATDEFBMEntity Entity, string Operation_Mode, out string Msg)
        {
            bool boolsuccess;
            Msg = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList = Prepare_MATDEFBM_SqlParameters_List(Entity, Operation_Mode);

                SqlParameter DeleteMsg = new SqlParameter("@msg", SqlDbType.VarChar, 50);
                DeleteMsg.Direction = ParameterDirection.Output;
                sqlParamList.Add(DeleteMsg);

                boolsuccess = Captain.DatabaseLayer.MatrixDB.Update_Sel_Table(sqlParamList, "MATDEFBM");
                Msg = DeleteMsg.Value.ToString();
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        public List<MATREASNEntity> Browse_MATREASN(MATREASNEntity Entity, string Opretaion_Mode)
        {
            List<MATREASNEntity> MATREASNProfile = new List<MATREASNEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_MATREASN_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet MATREASNData = Captain.DatabaseLayer.MatrixDB.Browse_Selected_Table(sqlParamList, "MATREASN");

                if (MATREASNData != null && MATREASNData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in MATREASNData.Tables[0].Rows)
                        MATREASNProfile.Add(new MATREASNEntity(row));
                }
            }
            catch (Exception ex)
            { return MATREASNProfile; }

            return MATREASNProfile;
        }

        public bool UpdateMATREASN(MATREASNEntity Entity, string Operation_Mode, out string Msg)
        {
            bool boolsuccess;
            Msg = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList = Prepare_MATREASN_SqlParameters_List(Entity, Operation_Mode);

                SqlParameter DeleteMsg = new SqlParameter("@msg", SqlDbType.VarChar, 50);
                DeleteMsg.Direction = ParameterDirection.Output;
                sqlParamList.Add(DeleteMsg);

                boolsuccess = Captain.DatabaseLayer.MatrixDB.Update_Sel_Table(sqlParamList, "MATREASN");
                Msg = DeleteMsg.Value.ToString();
            }
            catch (Exception ex)
            { return false; }
            return boolsuccess;
        }

        public List<SqlParameter> Prepare_MATREASN_SqlParameters_List(MATREASNEntity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                if (Opretaion_Mode != "Browse")
                    sqlParamList.Add(new SqlParameter("@Row_Type", Entity.Rec_Type));

                sqlParamList.Add(new SqlParameter("@MATREASN_MAT_CODE", Entity.MatCode));
                sqlParamList.Add(new SqlParameter("@MATREASN_SCL_CODE", Entity.Scl_Code));
                sqlParamList.Add(new SqlParameter("@MATREASN_CODE", Entity.Code));
                sqlParamList.Add(new SqlParameter("@MATREASN_PN", Entity.PN));
                sqlParamList.Add(new SqlParameter("@MATREASN_DESC", Entity.Desc));
                sqlParamList.Add(new SqlParameter("@MATREASN_LSTC_OPERATOR", Entity.Lstc_Operator));

                if (Opretaion_Mode == "Browse")
                {
                    sqlParamList.Add(new SqlParameter("@MATREASN_DATE_ADD", Entity.Add_Date));
                    sqlParamList.Add(new SqlParameter("@MATREASN_ADD_OPERATOR", Entity.Add_Operator));
                    sqlParamList.Add(new SqlParameter("@MATREASN_DATE_LSTC", Entity.Lstc_Date));
                }

            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }

        public bool UpdateMATSGRP(MATSGRPEntity Entity, string Operation_Mode, out string Msg)
        {
            bool boolsuccess;
            Msg = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList = Prepare_MATSGRP_SqlParameters_List(Entity, Operation_Mode);

                SqlParameter DeleteMsg = new SqlParameter("@msg", SqlDbType.VarChar, 50);
                DeleteMsg.Direction = ParameterDirection.Output;
                sqlParamList.Add(DeleteMsg);

                boolsuccess = Captain.DatabaseLayer.MatrixDB.Update_Sel_Table(sqlParamList, "MATSGRP");
                Msg = DeleteMsg.Value.ToString();

            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        public bool UpdateMATQUESR(MATQUESREntity Entity, string Operation_Mode, out string msg)
        {
            bool boolsuccess;
            msg = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList = Prepare_MATQUESR_SqlParameters_List(Entity, Operation_Mode);

                SqlParameter DeleteMsg = new SqlParameter("@msg", SqlDbType.VarChar, 50);
                DeleteMsg.Direction = ParameterDirection.Output;
                sqlParamList.Add(DeleteMsg);

                boolsuccess = Captain.DatabaseLayer.MatrixDB.Update_Sel_Table(sqlParamList, "MATQUESR");
                msg = DeleteMsg.Value.ToString();
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        public bool UpdateMATQUEST(MATQUESTEntity Entity, string Operation_Mode, out string msg)
        {
            bool boolsuccess;
            msg = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList = Prepare_MATQUEST_SqlParameters_List(Entity, Operation_Mode);

                SqlParameter DeleteMsg = new SqlParameter("@msg", SqlDbType.VarChar, 50);
                DeleteMsg.Direction = ParameterDirection.Output;
                sqlParamList.Add(DeleteMsg);

                boolsuccess = Captain.DatabaseLayer.MatrixDB.Update_Sel_Table(sqlParamList, "MATQUEST");
                msg = DeleteMsg.Value.ToString();
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        public bool InsertUpdateMatDef(MATDEFEntity Entity, string Operation_Mode, out int New_Scale_Code)
        {
            bool boolsuccess;
            New_Scale_Code = 0;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList = Prepare_MATDEF_SqlParameters_List(Entity, Operation_Mode);

                


                SqlParameter parameterMsg = new SqlParameter("@MAT_New_ID", SqlDbType.Int, 5);
                parameterMsg.Direction = ParameterDirection.Output;
                sqlParamList.Add(parameterMsg);

                //SqlParameter DeleteMsg = new SqlParameter("@msg", SqlDbType.VarChar, 50);
                //DeleteMsg.Direction = ParameterDirection.Output;
                //sqlParamList.Add(DeleteMsg);

                boolsuccess = Captain.DatabaseLayer.MatrixDB.Update_Sel_Table(sqlParamList, "MATDEF");
                New_Scale_Code = int.Parse(parameterMsg.Value.ToString());

            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        //public List<MATDEFDTEntity> Browse_MatMatdefdt()
        //{
        //    List<MATDEFDTEntity> MatsdefdtProfile = new List<MATDEFDTEntity>();
        //    try
        //    {
        //        DataSet CASESP1Data = MatrixDB.Browse_MatDefDt();
        //        if (CASESP1Data != null && CASESP1Data.Tables[0].Rows.Count > 0)
        //        {
        //            foreach (DataRow row in CASESP1Data.Tables[0].Rows)
        //                MatsdefdtProfile.Add(new MATDEFDTEntity(row));
        //        }
        //    }
        //    catch (Exception ex)
        //    { return MatsdefdtProfile; }

        //    return MatsdefdtProfile;
        //}



        public List<MATSHIEEntity> Browse_MatsHie(string Mat_Code, string Scl_Code)
        {
            List<MATSHIEEntity> MatsHieProfile = new List<MATSHIEEntity>();
            try
            {
                DataSet CASESP1Data = MatrixDB.Browse_MatsHie(Mat_Code, Scl_Code);
                if (CASESP1Data != null && CASESP1Data.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESP1Data.Tables[0].Rows)
                        MatsHieProfile.Add(new MATSHIEEntity(row));
                }
            }
            catch (Exception ex)
            { return MatsHieProfile; }

            return MatsHieProfile;
        }


        public bool UpdateMatsHie(MATSHIEEntity Entity)
        {
            bool boolsuccess;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@Row_Type", Entity.Rec_Type));
                sqlParamList.Add(new SqlParameter("@MATSHIE_MAT_CODE", Entity.MatCode));
                sqlParamList.Add(new SqlParameter("@MATSHIE_SCL_CODE", Entity.SclCode));
                sqlParamList.Add(new SqlParameter("@MATSHIE_AGENCY", Entity.Agency));
                sqlParamList.Add(new SqlParameter("@MATSHIE_DEPT", Entity.Dept));
                sqlParamList.Add(new SqlParameter("@MATSHIE_PROG", Entity.Prog));
                //sqlParamList.Add(new SqlParameter("@sp1_lstc_operator", Entity.lstcOperator));

                boolsuccess = Captain.DatabaseLayer.MatrixDB.UpdateMatsHie(sqlParamList);
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        //public bool UpdateMatsDefdt(MATDEFDTEntity Entity)
        //{
        //    bool boolsuccess;

        //    try
        //    {
        //        List<SqlParameter> sqlParamList = new List<SqlParameter>();
        //        sqlParamList.Add(new SqlParameter("@Row_Type", Entity.Rec_Type));
        //        sqlParamList.Add(new SqlParameter("@MATSHIE_MAT_CODE", Entity.MatCode));
        //        sqlParamList.Add(new SqlParameter("@MATSHIE_SCL_CODE", Entity.MatDate));
        //        //sqlParamList.Add(new SqlParameter("@MATSHIE_AGENCY", Entity.Agency));
        //        //sqlParamList.Add(new SqlParameter("@MATSHIE_DEPT", Entity.Dept));
        //        //sqlParamList.Add(new SqlParameter("@MATSHIE_PROG", Entity.Prog));
        //        //sqlParamList.Add(new SqlParameter("@sp1_lstc_operator", Entity.lstcOperator));

        //        boolsuccess = Captain.DatabaseLayer.MatrixDB.UpdateMatsDefDt(sqlParamList);
        //    }
        //    catch (Exception ex)
        //    { return false; }

        //    return boolsuccess;
        //}

        //public bool Delete_CASESP2(string SP_Code, string Branch_Code, int Orig_Grp, string Type1, string CamCd)
        //{
        //    try
        //    {
        //        Captain.DatabaseLayer.SPAdminDB.Delete_CASESP2(int.Parse(SP_Code), Branch_Code, Orig_Grp, Type1, CamCd);
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }

        //    return true;
        //}

        //    **************************************************************************************************************************   Yeswanth





        public List<MATDEFEntity> Get_Matdef_MatCode(string MatCode)
        {
            List<MATDEFEntity> CASESPMProfile = new List<MATDEFEntity>();

            try
            {

                DataSet CASESPMData = Captain.DatabaseLayer.MatrixDB.Get_Matdef_MatCode(MatCode);

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASESPMProfile.Add(new MATDEFEntity(row));
                }
            }
            catch (Exception ex)
            { return CASESPMProfile; }

            return CASESPMProfile;
        }

        public List<MATOUTCEntity> GetMATOUTCMatCode(string MatCode, string Scrcode, string strTable)
        {
            List<MATOUTCEntity> matoutcProfile = new List<MATOUTCEntity>();

            try
            {

                DataSet MatoutData = Captain.DatabaseLayer.MatrixDB.GETMATOUTCMatCode(MatCode, Scrcode);

                if (MatoutData != null && MatoutData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in MatoutData.Tables[0].Rows)
                        matoutcProfile.Add(new MATOUTCEntity(row, strTable));
                }
            }
            catch (Exception ex)
            { return matoutcProfile; }

            return matoutcProfile;
        }


        public List<MATQUESTEntity> GETMATQUESTQuestions(string MatCode, string Scrcode, string strGroupcode, string strBMcode, string strOutComecode, string strType, string strTable)
        {
            List<MATQUESTEntity> matquestProfile = new List<MATQUESTEntity>();

            try
            {

                DataSet MatquestData = Captain.DatabaseLayer.MatrixDB.GETMATQUESTQuestions(MatCode, Scrcode, strGroupcode, strBMcode, strOutComecode, strType);

                if (MatquestData != null && MatquestData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in MatquestData.Tables[0].Rows)
                        matquestProfile.Add(new MATQUESTEntity(row, strTable));
                }
            }
            catch (Exception ex)
            { return matquestProfile; }

            return matquestProfile;
        }




        public List<MATQUESREntity> GETMATQUESR(string MatCode, string Scrcode, string strGroupcode, string strQuestioncode, string strTable)
        {
            List<MATQUESREntity> matResponceProfile = new List<MATQUESREntity>();

            try
            {

                DataSet MatResponceData = Captain.DatabaseLayer.MatrixDB.GETMATQUESR(MatCode, Scrcode, strGroupcode, strQuestioncode);

                if (MatResponceData != null && MatResponceData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in MatResponceData.Tables[0].Rows)
                        matResponceProfile.Add(new MATQUESREntity(row));
                }
            }
            catch (Exception ex)
            { return matResponceProfile; }

            return matResponceProfile;
        }


        public bool InsertUpdateDelMatOutr(MATOUTREntity OutrEntity)
        {
            bool boolSuccess = false;
            string strNewApplNo = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@MATOUTR_MAT_CODE", OutrEntity.MatCode));
                sqlParamList.Add(new SqlParameter("@MATOUTR_SCL_CODE", OutrEntity.SclCode));
                sqlParamList.Add(new SqlParameter("@MATOUTR_BM_CODE", OutrEntity.BmCode));
                sqlParamList.Add(new SqlParameter("@MATOUTR_CODE", OutrEntity.Code));
                sqlParamList.Add(new SqlParameter("@MATOUTR_QUESTION", OutrEntity.Question));

                sqlParamList.Add(new SqlParameter("@MATOUTR_RESP_CODE", OutrEntity.RespCode));

                if (OutrEntity.AddOperator != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MATOUTR_ADD_OPERATOR", OutrEntity.AddOperator));

                if (OutrEntity.LstcOperator != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MATOUTR_LSTC_OPERATOR", OutrEntity.LstcOperator));

                sqlParamList.Add(new SqlParameter("@Mode", OutrEntity.Mode));



                boolSuccess = MatrixDB.InsertUpdateDelMatOutr(sqlParamList);

            }
            catch (Exception ex)
            {

                boolSuccess = false;
            }


            // strApplNo = strNewApplNo;
            return boolSuccess;
        }


        public List<MATQUESREntity> GETMatasmt(string MatCode, string Scrcode, string strGroupCode, string strQuestioncode, string strTable)
        {
            List<MATQUESREntity> matResponceProfile = new List<MATQUESREntity>();

            try
            {

                DataSet MatResponceData = Captain.DatabaseLayer.MatrixDB.GETMATQUESR(MatCode, Scrcode, strGroupCode, strQuestioncode);

                if (MatResponceData != null && MatResponceData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in MatResponceData.Tables[0].Rows)
                        matResponceProfile.Add(new MATQUESREntity(row));
                }
            }
            catch (Exception ex)
            { return matResponceProfile; }

            return matResponceProfile;
        }

        public List<MATAPDTSEntity> GETMatapdts(string Agency, string Dept, string Program, string Year, string AppNo, string MatCode, string ssDate, string hienameformat)
        {
            List<MATAPDTSEntity> MATAPDTSProfile = new List<MATAPDTSEntity>();

            try
            {

                DataSet MATAPDTSData = Captain.DatabaseLayer.MatrixDB.GETMatapdts(Agency, Dept, Program, Year, AppNo, MatCode, ssDate, hienameformat);

                if (MATAPDTSData != null && MATAPDTSData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in MATAPDTSData.Tables[0].Rows)
                        MATAPDTSProfile.Add(new MATAPDTSEntity(row, string.Empty));
                }
            }
            catch (Exception ex)
            { return MATAPDTSProfile; }

            return MATAPDTSProfile;
        }

        public List<MATAPDTSEntity> GETMatDefapdts(string Agency, string Dept, string Program, string Year, string AppNo, string MatCode, string ssDate, string strType)
        {
            List<MATAPDTSEntity> MATAPDTSProfile = new List<MATAPDTSEntity>();

            try
            {

                DataSet MATAPDTSData = Captain.DatabaseLayer.MatrixDB.GETMatDefapdts(Agency, Dept, Program, Year, AppNo, MatCode, ssDate, strType);

                if (MATAPDTSData != null && MATAPDTSData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in MATAPDTSData.Tables[0].Rows)
                        MATAPDTSProfile.Add(new MATAPDTSEntity(row, "MATDEFMATAPDTS"));
                }
            }
            catch (Exception ex)
            { return MATAPDTSProfile; }

            return MATAPDTSProfile;
        }


        public List<MATAPDTSEntity> GETMatDefDtdates(string Agency, string Dept, string Program, string Year, string AppNo, string MatCode, string ssDate, string strType)
        {
            List<MATAPDTSEntity> MATAPDTSProfile = new List<MATAPDTSEntity>();

            try
            {

                DataSet MATAPDTSData = Captain.DatabaseLayer.MatrixDB.GETMatDefapdts(Agency, Dept, Program, Year, AppNo, MatCode, ssDate, strType);

                if (MATAPDTSData != null && MATAPDTSData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in MATAPDTSData.Tables[0].Rows)
                        MATAPDTSProfile.Add(new MATAPDTSEntity(row, "MATDEFMATDEFDT"));
                }
            }
            catch (Exception ex)
            { return MATAPDTSProfile; }

            return MATAPDTSProfile;
        }




        public List<MATASMTEntity> GETMatasmt(string Agency, string Dept, string Program, string Year, string AppNo, string MatCode, string strSclcode, string strType, string ssDate, string quescode, string Famseq)
        {
            List<MATASMTEntity> MATASMTProfile = new List<MATASMTEntity>();

            try
            {

                DataSet MATASMTData = Captain.DatabaseLayer.MatrixDB.GETMatasmt(Agency, Dept, Program, Year, AppNo, MatCode, strSclcode, strType, ssDate, quescode, Famseq);

                if (MATASMTData != null && MATASMTData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in MATASMTData.Tables[0].Rows)
                        MATASMTProfile.Add(new MATASMTEntity(row, string.Empty));
                }
            }
            catch (Exception ex)
            { return MATASMTProfile; }

            return MATASMTProfile;
        }


        public bool InsertUpdateDelMatasmt(MATASMTEntity MATASMTEntity)
        {
            bool boolSuccess = false;
            string strNewApplNo = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@MATASMT_AGENCY", MATASMTEntity.Agency));
                sqlParamList.Add(new SqlParameter("@MATASMT_DEPT", MATASMTEntity.Dept));
                sqlParamList.Add(new SqlParameter("@MATASMT_PROGRAM", MATASMTEntity.Program));
                sqlParamList.Add(new SqlParameter("@MATASMT_YEAR", MATASMTEntity.Year));
                sqlParamList.Add(new SqlParameter("@MATASMT_APP", MATASMTEntity.App));
                if (MATASMTEntity.FamSeq != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MATASMT_FAM_SEQ", MATASMTEntity.FamSeq));
                if (MATASMTEntity.MatCode != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MATASMT_MAT_CODE", MATASMTEntity.MatCode));

                if (MATASMTEntity.SclCode != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MATASMT_SCL_CODE", MATASMTEntity.SclCode));
                if (MATASMTEntity.Type != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MATASMT_TYPE", MATASMTEntity.Type));
                if (MATASMTEntity.SSDate != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MATASMT_SS_DATE", MATASMTEntity.SSDate));
                if (MATASMTEntity.QuesCode != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MATASMT_QUES_CODE", MATASMTEntity.QuesCode));
                if (MATASMTEntity.RespCode != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MATASMT_RESP_CODE", MATASMTEntity.RespCode));
                if (MATASMTEntity.RespDesc != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MATASMT_RESP_DESC", MATASMTEntity.RespDesc));
                if (MATASMTEntity.Points != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MATASMT_POINTS", MATASMTEntity.Points));
                if (MATASMTEntity.Change != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MATASMT_CHANGE", MATASMTEntity.Change));
                if (MATASMTEntity.Pn != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MATASMT_PN", MATASMTEntity.Pn));
                if (MATASMTEntity.SixReasons != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MATASMT_SIX_REASONS", MATASMTEntity.SixReasons));
                if (MATASMTEntity.ByPass != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MATASMT_BYPASS", MATASMTEntity.ByPass));
                if (MATASMTEntity.AddOperator != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MATASMT_ADD_OPERATOR", MATASMTEntity.AddOperator));

                if (MATASMTEntity.LstcOperator != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MATASMT_LSTC_OPERATOR", MATASMTEntity.LstcOperator));

                if (MATASMTEntity.RespExcel != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MATASMT_RESP_EXCEL", MATASMTEntity.RespExcel));
                if (MATASMTEntity.Mat_BM_Code != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MATASMT_BM_CODE", MATASMTEntity.Mat_BM_Code));
                if (MATASMTEntity.PointsSwitch != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MATASMT_POINT_SWITCH", MATASMTEntity.PointsSwitch));

                sqlParamList.Add(new SqlParameter("@Mode", MATASMTEntity.Mode));



                boolSuccess = MatrixDB.InsertUpdateDelMatasmt(sqlParamList);

            }
            catch (Exception ex)
            {

                boolSuccess = false;
            }


            // strApplNo = strNewApplNo;
            return boolSuccess;
        }


        public bool InsertUpdateDelMatapdts(MATAPDTSEntity MATAPDTSEntity, out string strMsg)
        {
            bool boolSuccess = false;
            string strNewApplNo = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@MATAPDTS_AGENCY", MATAPDTSEntity.Agency));
                sqlParamList.Add(new SqlParameter("@MATAPDTS_DEPT", MATAPDTSEntity.Dept));
                sqlParamList.Add(new SqlParameter("@MATAPDTS_PROGRAM", MATAPDTSEntity.Program));
                if (MATAPDTSEntity.Year != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MATAPDTS_YEAR", MATAPDTSEntity.Year));
                sqlParamList.Add(new SqlParameter("@MATAPDTS_APP", MATAPDTSEntity.App));
                if (MATAPDTSEntity.MatCode != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MATAPDTS_MAT_CODE", MATAPDTSEntity.MatCode));
                if (MATAPDTSEntity.SSDate != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MATAPDTS_SS_DATE", MATAPDTSEntity.SSDate));
                if (MATAPDTSEntity.SSworker != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MATAPDTS_SS_WORKER", MATAPDTSEntity.SSworker));
                if (MATAPDTSEntity.AddOperator != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MATAPDTS_ADD_OPERATOR", MATAPDTSEntity.AddOperator));
                if (MATAPDTSEntity.LstcOperator != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MATAPDTS_LSTC_OPERATOR", MATAPDTSEntity.LstcOperator));
                sqlParamList.Add(new SqlParameter("@Mode", MATAPDTSEntity.Mode));

                if (MATAPDTSEntity.TotalScore != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MATAPDTS_TOT_SCORE", MATAPDTSEntity.TotalScore));
                if (MATAPDTSEntity.PartialScore != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MATAPDTS_PAR_SCORE", MATAPDTSEntity.PartialScore));

                if (MATAPDTSEntity.FOllowON != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MATAPDTS_FUP_DATE", MATAPDTSEntity.FOllowON));  //Added by Vikash 12/30/2022 for Followup issue
                if (MATAPDTSEntity.FollowCDate != string.Empty)
                    sqlParamList.Add(new SqlParameter("@MATAPDTS_COMP_DATE", MATAPDTSEntity.FollowCDate));  //Added by Vikash 12/30/2022 for Followup issue

                SqlParameter parameterMsg = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
                parameterMsg.Direction = ParameterDirection.Output;
                sqlParamList.Add(parameterMsg);
                boolSuccess = MatrixDB.InsertUpdateDelMatapdts(sqlParamList);
                strMsg = parameterMsg.Value.ToString();

            }
            catch (Exception ex)
            {
                strMsg = string.Empty;
                boolSuccess = false;
            }


            // strApplNo = strNewApplNo;
            return boolSuccess;
        }

        public bool INSERTUPDATEDELSCLGRPS(MATGroupEntity matGroupEntity)
        {
            bool boolSuccess = false;
            string strNewApplNo = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@SCLGRP_MAT_CODE", matGroupEntity.MatCode));
                sqlParamList.Add(new SqlParameter("@SCLGRP_GRP_CODE", matGroupEntity.Code));
                sqlParamList.Add(new SqlParameter("@SCLGRP_GRP_DESC", matGroupEntity.Desc));

                if (matGroupEntity.ShortName != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SCLGRP_GRP_SHORTNAME", matGroupEntity.ShortName));

                if (matGroupEntity.Seq != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SCLGRP_GRP_SEQ", matGroupEntity.Seq));

                if (matGroupEntity.Add_Operator != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SCLGRP_ADD_OPERATOR", matGroupEntity.Add_Operator));

                if (matGroupEntity.Lstc_Operator != string.Empty)
                    sqlParamList.Add(new SqlParameter("@SCLGRP_LSTC_OPERATOR", matGroupEntity.Lstc_Operator));

                sqlParamList.Add(new SqlParameter("@TYPE", matGroupEntity.Type));


                boolSuccess = MatrixDB.INSERTUPDATEDELSCLGRPS(sqlParamList);

            }
            catch (Exception ex)
            {

                boolSuccess = false;
            }


            // strApplNo = strNewApplNo;
            return boolSuccess;
        }


        public List<MATGroupEntity> GetSCLGRPS(string strMatcode, string strCode)
        {
            List<MATGroupEntity> MATGroupProfile = new List<MATGroupEntity>();
            try
            {
                DataSet MATREASNData = Captain.DatabaseLayer.MatrixDB.GetSCLGRPS(strMatcode, strCode);
                if (MATREASNData != null && MATREASNData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in MATREASNData.Tables[0].Rows)
                        MATGroupProfile.Add(new MATGroupEntity(row));
                }
            }
            catch (Exception ex)
            { return MATGroupProfile; }

            return MATGroupProfile;
        }



    }
}
