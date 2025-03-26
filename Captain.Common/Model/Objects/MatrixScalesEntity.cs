using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Captain.Common.Utilities;
using System.Web.UI.WebControls;
using System.Data;

namespace Captain.Common.Model.Objects
{
    public class MATDEFEntity
    {
        #region Constructors

        public MATDEFEntity()
        {

            Mat_Code = string.Empty;
            Scale_Code = string.Empty;
            Desc = string.Empty;
            Rationale = string.Empty;
            Sequence = string.Empty;
            Override = string.Empty;
            Benchmark = string.Empty;
            Active = string.Empty;
            Score = string.Empty;
            Show_BA = string.Empty;
            Date_option = string.Empty;
            Interval = string.Empty;
            Add_Date = string.Empty;
            Add_Operator = string.Empty;
            Lstc_Date = string.Empty;
            Lstc_Operator = string.Empty;
            Assessment_Type = string.Empty;
            OverlScor = string.Empty;
            SpecScor = string.Empty;
            GroupCode = string.Empty;
            Copy_Prassmnt = string.Empty;
            Prog_Method = string.Empty;
        }

        public MATDEFEntity(bool Initialize)
        {
            if (Initialize)
            {
                Mat_Code = null;
                Scale_Code = null;
                Desc = null;
                Rationale = null;
                Sequence = null;
                Override = null;
                Benchmark = null;
                Active = null;
                Score = null;
                Show_BA = null;
                Date_option = null;
                Interval = null;
                Add_Date = null;
                Add_Operator = null;
                Lstc_Date = null;
                Lstc_Operator = null;
                Assessment_Type = null;
                OverlScor = null;
                SpecScor = null;
                GroupCode = null;
                MatDefault = null;
                Copy_Prassmnt = null;
                Prog_Method = null;
            }
        }


        public MATDEFEntity(DataRow AgyTabControl)
        {
            if (AgyTabControl != null)
            {
                DataRow row = AgyTabControl;

                Rec_Type = "U";
                Rec_mode = "M";
                Mat_Code = row["MATDEF_MAT_CODE"].ToString();
                Scale_Code = row["MATDEF_SCL_CODE"].ToString();
                Desc = row["MATDEF_DESC"].ToString();
                Rationale = row["MATDEF_RATIONALE"].ToString();
                Sequence = row["MATDEF_SEQUENCE"].ToString();
                Override = row["MATDEF_SS_OVERIDE"].ToString();
                Benchmark = row["MATDEF_CALC_BMARK"].ToString();
                Active = row["MATDEF_ACTIVE"].ToString();
                Score = row["MATDEF_SCORE_SHEET"].ToString();
                Show_BA = row["MATDEF_SHOW_BA"].ToString();
                Date_option = row["MATDEF_SS_DATE_OPT"].ToString();
                Interval = row["MATDEF_INTERVAL"].ToString();
                Add_Date = row["MATDEF_DATE_ADD"].ToString();
                Add_Operator = row["MATDEF_ADD_OPERATOR"].ToString();
                Lstc_Date = row["MATDEF_DATE_LSTC"].ToString();
                Lstc_Operator = row["MATDEF_LSTC_OPERATOR"].ToString();
                Assessment_Type = row["MATDEF_SCL_ASSMT_TYPE"].ToString();
                OverlScor = row["MATDEF_OVRLSCOR"].ToString();
                SpecScor = row["MATDEF_SPECSCOR"].ToString();
                GroupCode = row["MATDEF_GRP_CODE"].ToString();
                MatDefault = row["MATDEF_DEFAULT"].ToString();
                Copy_Prassmnt = row["MATDEF_COPY_PRASSMNT"].ToString();
                Prog_Method = row["MATDEF_PROG_METHD"].ToString();
            }

        }

        public MATDEFEntity(MATDEFEntity Entity)
        {
            if (Entity != null)
            {
                Rec_Type = Entity.Rec_Type;
                Rec_mode = Entity.Rec_mode;
                Mat_Code = Entity.Mat_Code;
                Scale_Code = Entity.Scale_Code;
                Desc = Entity.Desc;
                Rationale = Entity.Rationale;
                Sequence = Entity.Sequence;
                Override = Entity.Override;
                Benchmark = Entity.Benchmark;
                Active = Entity.Active;
                //Type = Entity.Type;
                Score = Entity.Score;
                Show_BA = Entity.Show_BA;
                Date_option = Entity.Date_option;
                Interval = Entity.Interval;
                Add_Date = Entity.Add_Date;
                Add_Operator = Entity.Add_Operator;
                Lstc_Date = Entity.Lstc_Date;
                Lstc_Operator = Entity.Lstc_Operator;
                Assessment_Type = Entity.Assessment_Type;
                OverlScor = Entity.OverlScor;
                SpecScor  = Entity.SpecScor;
                GroupCode = Entity.GroupCode;
                MatDefault = Entity.MatDefault;
                Copy_Prassmnt = Entity.Copy_Prassmnt;
                Prog_Method = Entity.Prog_Method;

            }
        }

        #endregion

        #region Properties

        public string Rec_Type { get; set; }
        public string Rec_mode { get; set; }
        public string Mat_Code { get; set; }
        public string Scale_Code { get; set; }
        public string Desc { get; set; }
        public string Rationale { get; set; }
        public string Sequence { get; set; }
        public string Override { get; set; }
        public string Benchmark { get; set; }
        public string Active { get; set; }
        public string Score { get; set; }
        public string Show_BA { get; set; }
        public string Date_option { get; set; }
        public string Interval { get; set; }
        public string Add_Date { get; set; }
        public string Add_Operator { get; set; }
        public string Lstc_Date { get; set; }
        public string Lstc_Operator { get; set; }
        public string Assessment_Type { get; set; }
        public string OverlScor { get; set; }
        public string SpecScor { get; set; }
        public string GroupCode { get; set; }
        public string MatDefault { get; set; }
        public string Copy_Prassmnt { get; set; }
        public string Prog_Method { get; set; }

        #endregion

    }

    public class MATDEFBMEntity
    {
        #region Constructors

        public MATDEFBMEntity()
        {
            Rec_Type = string.Empty;
            MatCode = string.Empty;
            Code = string.Empty;
            Desc = string.Empty;
            Low = string.Empty;
            High = string.Empty;
            ScoreType = string.Empty;
            Overall_Low = string.Empty;
            Overall_High = string.Empty;
            Progress = string.Empty;
            Continuity = string.Empty;
            Regressed = string.Empty;
        }

        public MATDEFBMEntity(bool Initialize)
        {
            Rec_Type = null;
            MatCode = null;
            Code = null;
            Desc = null;
            Low = null;
            High = null;
            ScoreType = null;
            Overall_Low = null;
            Overall_High = null;
            Progress = null;
            Continuity = null;
            Regressed = null;
        }

        public MATDEFBMEntity(DataRow MatDefBm)
        {
            if (MatDefBm != null)
            {
                Rec_Type = "U";
                Code = MatDefBm["MATDEFBM_CODE"].ToString();
                Low = MatDefBm["MATDEFBM_LOW"].ToString();
                High = MatDefBm["MATDEFBM_HIGH"].ToString();
                Desc = MatDefBm["MATDEFBM_DESC"].ToString();
                MatCode = MatDefBm["MATDEFBM_MAT_CODE"].ToString();
                ScoreType = MatDefBm["MATDEFBM_SCORE_TYPE"].ToString(); ;
                Overall_Low = MatDefBm["MATDEFBM_MAT_LOW"].ToString();
                Overall_High = MatDefBm["MATDEFBM_MAT_HIGH"].ToString();
                Progress = MatDefBm["MATDEFBM_PROGRESS"].ToString();
                Continuity = MatDefBm["MATDEFBM_CONTINUITY"].ToString();
                Regressed = MatDefBm["MATDEFBM_Regress"].ToString() ;
            }
        }

        public MATDEFBMEntity(MATDEFBMEntity Entity)
        {
            if (Entity != null)
            {
                Rec_Type = Entity.Rec_Type;
                Code = Entity.Code;
                Low = Entity.Low;
                High = Entity.High;
                Desc = Entity.Desc;
                MatCode = Entity.MatCode;
                ScoreType = Entity.ScoreType;
                Overall_Low = Entity.Overall_Low;
                Overall_High = Entity.Overall_High;
                Progress = Entity.Progress;
                Continuity = Entity.Continuity;
                Regressed = Entity.Regressed;

            }
        }

        #endregion

        #region Properties

        public string Rec_Type { get; set; }
        public string MatCode { get; set; }
        public string Code { get; set; }
        public string Desc { get; set; }
        public string Low { get; set; }
        public string High { get; set; }
        public string Mode { get; set; }
        public string Type { get; set; }
        public string ScoreType { get; set; }
        public string Overall_Low { get; set; }
        public string Overall_High { get; set; }
        public string Progress { get; set; }
        public string Continuity { get; set; }
        public string Regressed { get; set; }
        #endregion

    }

    public class MATREASNEntity
    {
        #region Constructors

        public MATREASNEntity()
        {
            Rec_Type = string.Empty;
            MatCode = string.Empty;
            Scl_Code = string.Empty;
            Code = string.Empty;
            PN = string.Empty;
            Desc = string.Empty;
            Add_Date = string.Empty;
            Add_Operator = string.Empty;
            Lstc_Date = string.Empty;
            Lstc_Operator = string.Empty;
        }

        public MATREASNEntity(bool Initialize)
        {
            Rec_Type = null;
            MatCode = null;
            Scl_Code = null;
            PN = null;
            Code = null;
            Desc = null;
            Add_Date = null;
            Add_Operator = null;
            Lstc_Date = null;
            Lstc_Operator = null;
        }

        public MATREASNEntity(DataRow MATReasn)
        {
            if (MATReasn != null)
            {
                Rec_Type = "U";
                Code = MATReasn["MATREASN_CODE"].ToString();
                PN = MATReasn["MATREASN_PN"].ToString();
                //High = MATReasn["MATDEFBM_HIGH"].ToString();
                Desc = MATReasn["MATREASN_DESC"].ToString();
                MatCode = MATReasn["MATREASN_MAT_CODE"].ToString();
                Scl_Code = MATReasn["MATREASN_SCL_CODE"].ToString();
                Add_Date = MATReasn["MATREASN_DATE_ADD"].ToString();
                Add_Operator = MATReasn["MATREASN_ADD_OPERATOR"].ToString();
                Lstc_Date = MATReasn["MATREASN_DATE_LSTC"].ToString();
                Lstc_Operator = MATReasn["MATREASN_LSTC_OPERATOR"].ToString();
            }
        }

        public MATREASNEntity(MATREASNEntity Entity)
        {
            if (Entity != null)
            {
                Rec_Type = Entity.Rec_Type;
                Code = Entity.Code;
                PN = Entity.PN;
                //High = Entity.High;
                Desc = Entity.Desc;
                MatCode = Entity.MatCode;
                Scl_Code = Entity.Scl_Code;
                Add_Date = Entity.Add_Date;
                Add_Operator = Entity.Add_Operator;
                Lstc_Date = Entity.Lstc_Date;
                Lstc_Operator = Entity.Lstc_Operator;
            }
        }

        #endregion

        #region Properties

        public string Rec_Type { get; set; }
        public string MatCode { get; set; }
        public string Scl_Code { get; set; }
        public string Code { get; set; }
        public string Desc { get; set; }
        public string PN { get; set; }
        //public string High { get; set; }
        public string Mode { get; set; }
        public string Type { get; set; }
        public string Add_Date { get; set; }
        public string Add_Operator { get; set; }
        public string Lstc_Date { get; set; }
        public string Lstc_Operator { get; set; }
        #endregion

    }

    public class MATOUTCEntity
    {
        #region Constructors

        public MATOUTCEntity()
        {

            Rec_Type = string.Empty;
            MatCode = string.Empty;
            SclCode = string.Empty;
            BmCode = string.Empty;
            Code = string.Empty;
            Desc = string.Empty;
            Points = string.Empty;
            Condition = string.Empty;
            AddDate = string.Empty;
            AddOperator = string.Empty;
            DateLstc = string.Empty;
            LstcOperator = string.Empty;
        }

        public MATOUTCEntity(bool Initialize)
        {

            Rec_Type = null;
            MatCode = null;
            SclCode = null;
            BmCode = null;
            Code = null;
            Desc = null;
            Points = null;
            Condition = null;
            AddDate = null;
            AddOperator = null;
            DateLstc = null;
            LstcOperator = null;
        }


        public MATOUTCEntity(DataRow Matout,string strTable)
        {
            if (Matout != null)
            {
                Rec_Type = "U";
                MatCode = Matout["MATOUTC_MAT_CODE"].ToString();
                SclCode = Matout["MATOUTC_SCL_CODE"].ToString();
                BmCode = Matout["MATOUTC_BM_CODE"].ToString();
                Code = Matout["MATOUTC_CODE"].ToString();
                Desc = Matout["MATOUTC_DESC"].ToString();
                Points = Matout["MATOUTC_POINTS"].ToString();
                Condition = Matout["MATOUTC_CONDITION"].ToString();
                AddDate = Matout["MATOUTC_DATE_ADD"].ToString();
                AddOperator = Matout["MATOUTC_ADD_OPERATOR"].ToString();
                DateLstc = Matout["MATOUTC_DATE_LSTC"].ToString();
                LstcOperator = Matout["MATOUTC_LSTC_OPERATOR"].ToString();
                if (strTable == "MATDEFBM")
                    BMDesc = Matout["MATDEFBM_DESC"].ToString();
            }
        }

        public MATOUTCEntity(MATOUTCEntity Entity)
        {
            if (Entity != null)
            {
                Rec_Type = Entity.Rec_Type;
                Code = Entity.Code;
                SclCode = Entity.SclCode;
                BmCode = Entity.BmCode;
                Desc = Entity.Desc;
                MatCode = Entity.MatCode;
                Points = Entity.Points;
                Condition = Entity.Condition;
                AddDate = Entity.AddDate;
                AddOperator = Entity.AddOperator;
                DateLstc = Entity.DateLstc;
                LstcOperator = Entity.LstcOperator;
            }
        }

        #endregion

        #region Properties

        public string Rec_Type { get; set; }
        public string MatCode { get; set; }
        public string SclCode { get; set; }
        public string BmCode { get; set; }
        public string Code { get; set; }
        public string Desc { get; set; }
        public string Points { get; set; }
        public string Condition { get; set; }
        public string AddDate { get; set; }
        public string AddOperator { get; set; }
        public string DateLstc { get; set; }
        public string LstcOperator { get; set; }
        public string Mode { get; set; }
        public string Type { get; set; }
        public string BMDesc { get; set; }
        #endregion

    }


    public class MATOUTREntity
    {
        #region Constructors

        public MATOUTREntity()
        {

            MatCode = string.Empty;
            SclCode = string.Empty;
            BmCode = string.Empty;
            Code = string.Empty;
            Question = string.Empty;
            RespCode = string.Empty;           
            AddDate = string.Empty;
            AddOperator = string.Empty;
            DateLstc = string.Empty;
            LstcOperator = string.Empty;
        }

        public MATOUTREntity(bool Initialize)
        {
            MatCode = null;
            SclCode = null;
            BmCode = null;
            Code = null;
            Question = null;
            RespCode = null;
            AddDate = null;
            AddOperator = null;
            DateLstc = null;
            LstcOperator = null;
        }

        public MATOUTREntity(DataRow Matout, string strTable)
        {
            if (Matout != null)
            {
                MatCode = Matout["MATOUTR_MAT_CODE"].ToString();
                SclCode = Matout["MATOUTR_SCL_CODE"].ToString();
                BmCode = Matout["MATOUTR_BM_CODE"].ToString();
                Code = Matout["MATOUTR_CODE"].ToString();
                Question = Matout["MATOUTR_QUESTION"].ToString();
                RespCode = Matout["MATOUTR_RESP_CODE"].ToString();
                AddDate = Matout["MATOUTR_DATE_ADD"].ToString();
                AddOperator = Matout["MATOUTR_ADD_OPERATOR"].ToString();
                DateLstc = Matout["MATOUTR_DATE_LSTC"].ToString();
                LstcOperator = Matout["MATOUTR_LSTC_OPERATOR"].ToString();
            }
        }

        public MATOUTREntity(MATOUTREntity Entity)
        {
            if (Entity != null)
            {
                Code = Entity.Code;
                SclCode = Entity.SclCode;
                BmCode = Entity.BmCode;
                Question = Entity.Question;
                MatCode = Entity.MatCode;
                RespCode = Entity.RespCode;              
                AddDate = Entity.AddDate;
                AddOperator = Entity.AddOperator;
                DateLstc = Entity.DateLstc;
                LstcOperator = Entity.LstcOperator;
            }
        }

        #endregion

        #region Properties

        public string MatCode { get; set; }
        public string SclCode { get; set; }
        public string BmCode { get; set; }
        public string Code { get; set; }
        public string Question { get; set; }
        public string RespCode { get; set; }     
        public string AddDate { get; set; }
        public string AddOperator { get; set; }
        public string DateLstc { get; set; }
        public string LstcOperator { get; set; }
        public string Mode { get; set; }
        public string Type { get; set; }
        #endregion

    }

    public class MATQUESTEntity
    {
        #region Constructors

        public MATQUESTEntity()
        {
            Rec_Type = string.Empty;
            MatCode = string.Empty;
            SclCode = string.Empty;
            Group = string.Empty;
            Code = string.Empty;
            Seq = string.Empty;
            Desc = string.Empty;
            Type = string.Empty;
            SsOverride = string.Empty;
            AddDate = string.Empty;
            AddOperator = string.Empty;
            DateLstc = string.Empty;
            LstcOperator = string.Empty;
            QuesNumericType = string.Empty;
            QuesActive = string.Empty;
        }

        public MATQUESTEntity(bool Initialize)
        {
            Rec_Type = null;
            MatCode = null;
            SclCode = null;
            Group = null;
            Code = null;
            Seq = null;
            Desc = null;
            Type = null;
            SsOverride = null;
            AddDate = null;
            AddOperator = null;
            DateLstc = null;
            LstcOperator = null;
            QuesNumericType = null;
            QuesActive = null;
        }

        public MATQUESTEntity(DataRow Ques,string strTable)
        {
            if (Ques != null)
            {
                Rec_Type = "U";
                MatCode = Ques["MATQUEST_MAT_CODE"].ToString();
                SclCode = Ques["MATQUEST_SCL_CODE"].ToString();
                Group = Ques["MATQUEST_GROUP"].ToString();
                Code = Ques["MATQUEST_CODE"].ToString();
                Seq = Ques["MATQUEST_SEQ"].ToString();
                Desc = Ques["MATQUEST_DESC"].ToString();
                Type = Ques["MATQUEST_TYPE"].ToString();
                SsOverride = Ques["MATQUEST_SS_OVERRIDE"].ToString();
                AddDate = Ques["MATQUEST_DATE_ADD"].ToString();
                AddOperator = Ques["MATQUEST_ADD_OPERATOR"].ToString();
                DateLstc = Ques["MATQUEST_DATE_LSTC"].ToString();
                LstcOperator = Ques["MATQUEST_LSTC_OPERATOR"].ToString();
                QuesNumericType = Ques["MATQUEST_NTYPE"].ToString();
                QuesActive = Ques["MATQUEST_ACTIVE"].ToString();
                if (strTable == "MATOUTR")
                {
                    ResponceCode = Ques["MATOUTR_RESP_CODE"].ToString();
                    ResponceDesc = Ques["RESPONCE"].ToString();
                    //ResponceExclude = Ques["Exclude"].ToString();
                }
            }

        }

        public MATQUESTEntity(MATQUESTEntity Entity)
        {
            if (Entity != null)
            {
                Rec_Type = Entity.Rec_Type;
                Code = Entity.Code;
                SclCode = Entity.SclCode;
                Group = Entity.Group;
                Seq = Entity.Seq;
                MatCode = Entity.MatCode;
                Desc = Entity.Desc;
                Type = Entity.Type;
                SsOverride = Entity.SsOverride;
                AddDate = Entity.AddDate;
                AddOperator = Entity.AddOperator;
                DateLstc = Entity.DateLstc;
                LstcOperator = Entity.LstcOperator;
                QuesActive = Entity.QuesActive;
            }
        }

        #endregion

        #region Properties

        public string Rec_Type { get; set; }
        public string MatCode { get; set; }
        public string SclCode { get; set; }
        public string Group { get; set; }
        public string Code { get; set; }
        public string Seq { get; set; }
        public string Desc { get; set; }
        public string Type { get; set; }
        public string SsOverride { get; set; }
        public string AddDate { get; set; }
        public string AddOperator { get; set; }
        public string DateLstc { get; set; }
        public string LstcOperator { get; set; }
        public string Mode { get; set; }
        public string SqlType { get; set; }
        public string ResponceCode { get; set; }
        public string ResponceDesc { get; set; }
        public string ResponceExclude { get; set; }
        public string QuesNumericType { get; set; }
        public string QuesActive { get; set; }
        #endregion

    }

    public class MATQUESREntity
    {
        #region Constructors

        public MATQUESREntity()
        {
            Rec_Type = string.Empty;
            MatCode = string.Empty;
            SclCode = string.Empty;
            Group = string.Empty;
            Code = string.Empty;
            //Seq = string.Empty;
            RespDesc = string.Empty;          
            RespCode = string.Empty;           
            Points = string.Empty;
            Exclude = string.Empty;          
        }

        public MATQUESREntity(bool Initialize)
        {
            Rec_Type = null;
            MatCode = null;
            Code = null;
            SclCode = null;
            Group = null;
            //Seq = null;
            MatCode = null;
            RespDesc = null;
            RespCode = null;
            Points = null;
            Exclude = null;
        }

        public MATQUESREntity(DataRow QuesResp)
        {
            if (QuesResp != null)
            {
                Rec_Type = "U";
                MatCode = QuesResp["MATQUESR_MAT_CODE"].ToString();
                SclCode = QuesResp["MATQUESR_SCL_CODE"].ToString();
                Group = QuesResp["MATQUESR_GROUP"].ToString();
                Code = QuesResp["MATQUESR_CODE"].ToString();
                //Seq = QuesResp["MATQUESR_SEQ"].ToString();
                RespDesc = QuesResp["MATQUESR_RESP_DESC"].ToString().Trim();
                RespCode = QuesResp["MATQUESR_RESP_CODE"].ToString();
                Points = QuesResp["MATQUESR_POINTS"].ToString();
                Exclude = QuesResp["MATQUESR_EXCLUDE"].ToString();              
            }
        }

        public MATQUESREntity(MATQUESREntity Entity)
        {
            if (Entity != null)
            {
                Rec_Type = Entity.Rec_Type;
                MatCode = Entity.MatCode;
                Code = Entity.Code;
                SclCode = Entity.SclCode;
                Group = Entity.Group;
                //Seq = Entity.Seq;
                MatCode = Entity.MatCode;
                RespDesc = Entity.RespDesc;               
                RespCode = Entity.RespCode;               
                Points = Entity.Points;
                Exclude = Entity.Exclude;
            }
        }

        #endregion

        #region Properties
        
        public string Rec_Type { get; set; }
        public string MatCode { get; set; }
        public string SclCode { get; set; }
        public string Group { get; set; }
        public string Code { get; set; }
        public string Seq { get; set; }
        public string RespDesc { get; set; }      
        public string RespCode { get; set; }       
        public string Points { get; set; }
        public string Exclude { get; set; }      
        public string Mode { get; set; }
        public string SqlType { get; set; }
        #endregion

    }


    public class MATSGRPEntity
    {
        #region Constructors

        public MATSGRPEntity()
        {
            Rec_Type = string.Empty;
            MatCode = string.Empty;
            SclCode = string.Empty;
            Group = string.Empty;
            Desc = string.Empty;
            GrpType = string.Empty;
            Seq = string.Empty;
        }

        public MATSGRPEntity(bool Initialize)
        {
            Rec_Type = null;
            MatCode = null;
            SclCode = null;
            Group = null;
            Desc = null;
            GrpType = null;
            Seq = null;
        }

        public MATSGRPEntity(DataRow Grp)
        {
            if (Grp != null)
            {
                Rec_Type = "U";
                MatCode = Grp["MATSGRP_MAT_CODE"].ToString();
                SclCode = Grp["MATSGRP_SCL_CODE"].ToString();
                Group = Grp["MATSGRP_CODE"].ToString();
                Desc = Grp["MATSGRP_DESC"].ToString();
                GrpType = Grp["MATSGRP_TYPE"].ToString();
                Seq = Grp["MATSGRP_SEQ"].ToString();
            }
        }

        public MATSGRPEntity(MATSGRPEntity Entity)
        {
            if (Entity != null)
            {
                Rec_Type = Entity.Rec_Type;
                MatCode = Entity.MatCode;
                SclCode = Entity.SclCode;
                Group = Entity. Group;
                Desc = Entity.Desc;
                GrpType = Entity.GrpType;
                Seq = Entity.Seq;
            }
        }

        #endregion

        #region Properties

        public string Rec_Type { get; set; }
        public string MatCode { get; set; }
        public string SclCode { get; set; }
        public string Group { get; set; }
        public string Desc { get; set; }
        public string GrpType { get; set; }
        public string Seq { get; set; }

        #endregion

    }

    public class MATSHIEEntity
    {
        #region Constructors

        public MATSHIEEntity()
        {
            Rec_Type = string.Empty;
            MatCode = string.Empty;
            SclCode = string.Empty;
            Agency = string.Empty;
            Dept = string.Empty;
            Prog = string.Empty;
            //Seq = string.Empty;
        }

        public MATSHIEEntity(bool Initialize)
        {
            Rec_Type = null;
            MatCode = null;
            SclCode = null;
            Agency = null;
            Dept = null;
            Prog = null;
            //Seq = null;
        }

        public MATSHIEEntity(DataRow Hie)
        {
            if (Hie != null)
            {
                Rec_Type = "U";
                MatCode = Hie["MATSHIE_MAT_CODE"].ToString();
                SclCode = Hie["MATSHIE_SCL_CODE"].ToString();
                Agency = Hie["MATSHIE_AGENCY"].ToString();
                Dept = Hie["MATSHIE_DEPT"].ToString();
                Prog = Hie["MATSHIE_PROG"].ToString();
                //Seq = Grp["MATSGRP_SEQ"].ToString();
            }
        }

        public MATSHIEEntity(MATSHIEEntity Entity)
        {
            if (Entity != null)
            {
                Rec_Type = Entity.Rec_Type;
                MatCode = Entity.MatCode;
                SclCode = Entity.SclCode;
                Agency = Entity.Agency;
                Dept = Entity.Dept;
                Prog = Entity.Prog;
                //Seq = Entity.Seq;
            }
        }

        #endregion

        #region Properties

        public string Rec_Type { get; set; }
        public string MatCode { get; set; }
        public string SclCode { get; set; }
        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Prog { get; set; }
        //public string Seq { get; set; }

        #endregion

    }

    public class MATDEFDTEntity
    {
        #region Constructors

        public MATDEFDTEntity()
        {
            Rec_Type = string.Empty;
            MatCode = string.Empty;
            MatDate = string.Empty;
            //Agency = string.Empty;
            //Dept = string.Empty;
            //Prog = string.Empty;
            //Seq = string.Empty;
        }

        public MATDEFDTEntity(bool Initialize)
        {
            Rec_Type = null;
            MatCode = null;
            MatDate = null;
            //Agency = null;
            //Dept = null;
            //Prog = null;
            //Seq = null;
        }

        public MATDEFDTEntity(DataRow Hie)
        {
            if (Hie != null)
            {
                Rec_Type = "U";
                MatCode = Hie["MATDEFDT_MAT_CODE"].ToString();
                MatDate = Hie["MATDEFDT_DATE"].ToString();
                //Agency = Hie["MATSHIE_AGENCY"].ToString();
                //Dept = Hie["MATSHIE_DEPT"].ToString();
                //Prog = Hie["MATSHIE_PROG"].ToString();
                //Seq = Grp["MATSGRP_SEQ"].ToString();
            }
        }

        public MATDEFDTEntity(MATDEFDTEntity Entity)
        {
            if (Entity != null)
            {
                Rec_Type = Entity.Rec_Type;
                MatCode = Entity.MatCode;
                MatDate = Entity.MatDate;
                //Agency = Entity.Agency;
                //Dept = Entity.Dept;
                //Prog = Entity.Prog;
                //Seq = Entity.Seq;
            }
        }

        #endregion

        #region Properties

        public string Rec_Type { get; set; }
        public string MatCode { get; set; }
        public string MatDate { get; set; }
        //public string Agency { get; set; }
        //public string Dept { get; set; }
        //public string Prog { get; set; }
        //public string Seq { get; set; }

        #endregion

    }



    public class MATAPDTSEntity
    {
        #region Constructors

        public MATAPDTSEntity()
        {
            Agency = string.Empty;
            Dept = string.Empty;
            Program = string.Empty;
            Year = string.Empty;
            App = string.Empty;
            MatCode = string.Empty;
            SSDate = string.Empty;
            SSworker = string.Empty;
            Mode = string.Empty;
            SqlType = string.Empty;
            AddDate = string.Empty;
            AddOperator = string.Empty;
            DateLstc = string.Empty;
            LstcOperator = string.Empty;
            Assessment_Type = string.Empty;
            TotalScore = string.Empty;
            PartialScore = string.Empty;
            FOllowON = string.Empty;
            FollowCDate = string.Empty;
        }

        public MATAPDTSEntity(bool Initialize)
        {
            Agency = null;
            Dept = null;
            Program = null;
            Year = null;
            App = null;
            MatCode = null;
            SSDate = null;
            SSworker = null;
            Mode = null;
            SqlType = null;
            AddDate = null;
            AddOperator = null;
            DateLstc = null;
            LstcOperator = null;
            Assessment_Type = null;
            TotalScore = null;
            PartialScore = null;
            FOllowON = null;
            FollowCDate = null;
        }

        public MATAPDTSEntity(DataRow Ques, string strTable)
        {
            if (Ques != null)
            {
                if (strTable == "MATDEFMATAPDTS")
                {
                    SclcodeDesc = Ques["MATDEF_DESC"].ToString();
                    SclCode = Ques["MATDEF_SCL_CODE"].ToString();
                    Sequency = Ques["MATDEF_SEQUENCE"].ToString();
                    ScoreSheet = Ques["MATDEF_SCORE_SHEET"].ToString();
                    CalcBmark = Ques["MATDEF_CALC_BMARK"].ToString();
                    SSOverRide = Ques["MATDEF_SS_OVERIDE"].ToString();
                    Agency = Ques["MATAPDTS_AGENCY"].ToString(); ;
                    Dept = Ques["MATAPDTS_DEPT"].ToString(); ;
                    Program = Ques["MATAPDTS_PROGRAM"].ToString();
                    Year = Ques["MATAPDTS_YEAR"].ToString(); ;
                    App = Ques["MATAPDTS_APP"].ToString(); ;
                    MatCode = Ques["MATAPDTS_MAT_CODE"].ToString(); ;
                    SSDate = Ques["MATAPDTS_SS_DATE"].ToString();
                    CalcBmark = Ques["MATDEF_CALC_BMARK"].ToString();
                    SSOverRide = Ques["MATDEF_SS_OVERIDE"].ToString();
                    AddDate = Ques["MATDEF_DATE_ADD"].ToString();
                    AddOperator = Ques["MATDEF_ADD_OPERATOR"].ToString();
                    DateLstc = Ques["MATDEF_DATE_LSTC"].ToString();
                    LstcOperator = Ques["MATDEF_LSTC_OPERATOR"].ToString();
                    Assessment_Type = Ques["MATDEF_SCL_ASSMT_TYPE"].ToString();

                }
                else if (strTable == "MATDEFMATDEFDT")
                {
                    SclcodeDesc = Ques["MATDEF_DESC"].ToString();
                    SclCode = Ques["MATDEF_SCL_CODE"].ToString();
                    Sequency = Ques["MATDEF_SEQUENCE"].ToString();
                    ScoreSheet = Ques["MATDEF_SCORE_SHEET"].ToString();
                    CalcBmark = Ques["MATDEF_CALC_BMARK"].ToString();
                    SSOverRide = Ques["MATDEF_SS_OVERIDE"].ToString();
                    Agency = string.Empty;
                    Dept = string.Empty;
                    Program = string.Empty;
                    Year = string.Empty;
                    App = string.Empty;
                    MatCode = Ques["MATDEFDT_MAT_CODE"].ToString(); ;
                    SSDate = Ques["MATDEFDT_DATE"].ToString();
                    CalcBmark = Ques["MATDEF_CALC_BMARK"].ToString();
                    SSOverRide = Ques["MATDEF_SS_OVERIDE"].ToString();
                    AddDate = Ques["MATDEF_DATE_ADD"].ToString();
                    AddOperator = Ques["MATDEF_ADD_OPERATOR"].ToString();
                    DateLstc = Ques["MATDEF_DATE_LSTC"].ToString();
                    LstcOperator = Ques["MATDEF_LSTC_OPERATOR"].ToString();
                    Assessment_Type = Ques["MATDEF_SCL_ASSMT_TYPE"].ToString();

                }
                else
                {
                    Agency = Ques["MATAPDTS_AGENCY"].ToString(); ;
                    Dept = Ques["MATAPDTS_DEPT"].ToString(); ;
                    Program = Ques["MATAPDTS_PROGRAM"].ToString(); ;
                    Year = Ques["MATAPDTS_YEAR"].ToString(); ;
                    App = Ques["MATAPDTS_APP"].ToString(); ;
                    MatCode = Ques["MATAPDTS_MAT_CODE"].ToString(); ;
                    SSDate = Ques["MATAPDTS_SS_DATE"].ToString(); ;
                    SSworker = Ques["MATAPDTS_SS_WORKER"].ToString(); ;
                    AddDate = Ques["MATAPDTS_DATE_ADD"].ToString(); ;
                    AddOperator = Ques["MATAPDTS_ADD_OPERATOR"].ToString(); ;
                    DateLstc = Ques["MATAPDTS_DATE_LSTC"].ToString(); ;
                    LstcOperator = Ques["MATAPDTS_LSTC_OPERATOR"].ToString(); ;
                    Name = Ques["Name"].ToString();
                    TotalScore = Ques["MATAPDTS_TOT_SCORE"].ToString();
                    PartialScore = Ques["MATAPDTS_PAR_SCORE"].ToString();
                    FOllowON = Ques["MATAPDTS_FUP_DATE"].ToString();
                    FollowCDate = Ques["MATAPDTS_COMP_DATE"].ToString();

                }

            }

        }

        public MATAPDTSEntity(MATAPDTSEntity Entity)
        {
            if (Entity != null)
            {
                Agency = Entity.Agency;
                Dept = Entity.Dept;
                Program = Entity.Program;
                Year = Entity.Year;
                App = Entity.App;
                MatCode = Entity.MatCode;
                SSworker = Entity.SSworker;
                SSDate = Entity.SSDate;
                AddDate = Entity.AddDate;
                AddOperator = Entity.AddOperator;
                DateLstc = Entity.DateLstc;
                LstcOperator = Entity.LstcOperator;
                TotalScore = Entity.TotalScore.ToString();
                PartialScore = Entity.PartialScore.ToString();
                FOllowON = Entity.FOllowON;
                FollowCDate = Entity.FollowCDate;
            }
        }

        #endregion

        #region Properties

        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Program { get; set; }
        public string Year { get; set; }
        public string App { get; set; }
        public string MatCode { get; set; }
        public string SSDate { get; set; }
        public string SSworker { get; set; }
        public string AddDate { get; set; }
        public string AddOperator { get; set; }
        public string DateLstc { get; set; }
        public string LstcOperator { get; set; }
        public string Mode { get; set; }
        public string SqlType { get; set; }
        public string Rec_Type { get; set; }
        public string Name { get; set; }
        public string SclcodeDesc { get; set; }
        public string Sequency { get; set; }
        public string SclCode { get; set; }
        public string ScoreSheet { get; set; }
        public string CalcBmark { get; set; }
        public string SSOverRide { get; set; }
        public string Assessment_Type { get; set; }
        public string TotalScore { get; set; }
        public string PartialScore { get; set; }
        public string FOllowON { get; set; }
        public string FollowCDate { get; set; }
        #endregion

    }
    public class MATASMTEntity
    {
        #region Constructors

        public MATASMTEntity()
        {
            Agency = string.Empty;
            Dept = string.Empty;
            Program = string.Empty;
            Year = string.Empty;
            App = string.Empty;
            MatCode = string.Empty;
            SSDate = string.Empty;
            SclCode = string.Empty;
            Type = string.Empty;
            SSDate = string.Empty;
            QuesCode = string.Empty;
            RespCode = string.Empty;
            RespDesc = string.Empty;
            Points = string.Empty;
            Change = string.Empty;
            Pn = string.Empty;
            SixReasons = string.Empty;
            ByPass = string.Empty;
            Mode = string.Empty;
            SqlType = string.Empty;
            AddDate = string.Empty;
            AddOperator = string.Empty;
            DateLstc = string.Empty;
            LstcOperator = string.Empty;
            RespExcel = string.Empty;
            Mat_BM_Code = string.Empty;
            FamSeq = string.Empty;
        }

        public MATASMTEntity(bool Initialize)
        {
            Agency = null;
            Dept = null;
            Program = null;
            Year = null;
            App = null;
            MatCode = null;
            SSDate = null;
            SclCode = null;
            Type = null;
            SSDate = null;
            QuesCode = null;
            RespCode = null;
            RespDesc = null;
            Points = null;
            Change = null;
            Pn = null;
            SixReasons = null;
            ByPass = null;
            Mode = null;
            SqlType = null;
            AddDate = null;
            AddOperator = null;
            DateLstc = null;
            LstcOperator = null;
            RespExcel = null;
            Mat_BM_Code = null;
            FamSeq = null;
        }

        public MATASMTEntity(DataRow ASMT, string strTable)
        {
            if (ASMT != null)
            {
                Agency = ASMT["MATASMT_AGENCY"].ToString();
                Dept = ASMT["MATASMT_DEPT"].ToString();
                Program = ASMT["MATASMT_PROGRAM"].ToString();
                Year = ASMT["MATASMT_YEAR"].ToString();
                App = ASMT["MATASMT_APP"].ToString();
                MatCode = ASMT["MATASMT_MAT_CODE"].ToString();
                SclCode = ASMT["MATASMT_SCL_CODE"].ToString();
                Type = ASMT["MATASMT_TYPE"].ToString();
                SSDate = ASMT["MATASMT_SS_DATE"].ToString();
                QuesCode = ASMT["MATASMT_QUES_CODE"].ToString();
                RespCode = ASMT["MATASMT_RESP_CODE"].ToString();
                RespDesc = ASMT["MATASMT_RESP_DESC"].ToString();
                Points = ASMT["MATASMT_POINTS"].ToString();
                Change = ASMT["MATASMT_CHANGE"].ToString();
                Pn = ASMT["MATASMT_PN"].ToString();
                SixReasons = ASMT["MATASMT_SIX_REASONS"].ToString();
                ByPass = ASMT["MATASMT_BYPASS"].ToString();
                AddDate = ASMT["MATASMT_DATE_ADD"].ToString();
                AddOperator = ASMT["MATASMT_ADD_OPERATOR"].ToString();
                DateLstc = ASMT["MATASMT_DATE_LSTC"].ToString();
                LstcOperator = ASMT["MATASMT_LSTC_OPERATOR"].ToString();
                RespExcel = ASMT["MATASMT_RESP_EXCEL"].ToString();
                Mat_BM_Code = ASMT["MATASMT_BM_CODE"].ToString();
                FamSeq = ASMT["MATASMT_FAM_SEQ"].ToString();
                PointsSwitch = ASMT["MATASMT_POINT_SWITCH"].ToString();
                if (strTable != "Browse")
                    ScrDesc = ASMT["ScrDesc"].ToString();
            }

        }

        public MATASMTEntity(MATASMTEntity Entity)
        {
            if (Entity != null)
            {
                Agency = Entity.Agency;
                Dept = Entity.Dept;
                Program = Entity.Program;
                Year = Entity.Year;
                App = Entity.App;
                MatCode = Entity.MatCode;
                SclCode = Entity.SclCode;
                Type = Entity.Type;
                SSDate = Entity.SSDate;
                QuesCode = Entity.QuesCode;
                RespCode = Entity.RespCode;
                RespDesc = Entity.RespDesc;
                Points = Entity.Points;
                Change = Entity.Change;
                Pn = Entity.Pn;
                SixReasons = Entity.SixReasons;
                ByPass = Entity.ByPass;
                SSDate = Entity.SSDate;
                AddDate = Entity.AddDate;
                AddOperator = Entity.AddOperator;
                DateLstc = Entity.DateLstc;
                LstcOperator = Entity.LstcOperator;
                ScrDesc = Entity.ScrDesc;
                RespExcel = Entity.RespExcel;
                FamSeq = Entity.FamSeq;
                PointsSwitch = Entity.PointsSwitch;
            }
        }

        public MATASMTEntity(DataRow datasmt)
        {
            if (datasmt != null)
            {
                DataRow row = datasmt;
                Agency = row["MATASMT_AGENCY"].ToString().Trim();
                Dept = row["MATASMT_DEPT"].ToString().Trim();
                Program = row["MATASMT_PROGRAM"].ToString().Trim();
                Year = row["MATASMT_YEAR"].ToString().Trim();
                App = row["MATASMT_APP"].ToString().Trim();
                //MATDEF_SEQUENCE = row["MATDEF_SEQUENCE"].ToString().Trim();
                SSDate = row["MATASMT_SS_DATE"].ToString().Trim();
                Points = row["MATASMT_POINTS"].ToString().Trim();
                MatCode = row["MATASMT_MAT_CODE"].ToString().Trim();
                SclCode = row["MATASMT_SCL_CODE"].ToString().Trim();
                Type = row["MATASMT_TYPE"].ToString().Trim();
                QuesCode = row["MATASMT_QUES_CODE"].ToString().Trim();
                AddDate = row["MATASMT_DATE_ADD"].ToString().Trim();
            }
        }

        public MATASMTEntity(string strScale, string strSeq, string strBenchMark, string strScore, string strAssedate, string strStatus, string strStatusValue, string strScale1, string strSeq1, string strScaleCode, string strFamilySeq, string strBenchmarkcode, string strCalcBMark, string strCalcBmarkStatus, string strCalcBSwitch, string strRespExcel, string strAssessmentType, string strPointswitch, string strCurrentYear)
        {
            Agency = string.Empty;
            Dept = string.Empty;
            Program = string.Empty;
            Year = strCurrentYear;
            App = string.Empty;
            MatCode = string.Empty;
            SSDate = string.Empty;
            SclCode = string.Empty;
            Type = string.Empty;
            SSDate = string.Empty;
            QuesCode = string.Empty;
            RespCode = string.Empty;
            RespDesc = string.Empty;
            Points = string.Empty;
            Change = string.Empty;
            Pn = string.Empty;
            SixReasons = string.Empty;
            ByPass = string.Empty;
            Mode = string.Empty;
            SqlType = string.Empty;
            AddDate = string.Empty;
            AddOperator = string.Empty;
            DateLstc = string.Empty;
            LstcOperator = string.Empty;
            RespExcel = string.Empty;
            Mat_BM_Code = string.Empty;
            FamSeq = string.Empty;
            BenchMarkScoreType = string.Empty;
            propScale = strScale;
            propSeq = strSeq;
            propBenchMark = strBenchMark;
            propScore = strScore;
            propAssedate = strAssedate;
            propStatus = strStatus;
            propStatusValue = strStatusValue;
            propScale1 = strScale1;
            propSeq1 = strSeq1;
            propScaleCode = strScaleCode;
            propFamilySeq = strFamilySeq;
            propBenchmarkcode = strBenchmarkcode;
            propCalcBMark = strCalcBMark;
            propCalcBmarkStatus = strCalcBmarkStatus;
            propCalcBSwitch = strCalcBSwitch;
            propRespExcel = strRespExcel;
            propAssessmentType = strAssessmentType;
            PointsSwitch = strPointswitch;
        }


        #endregion

        #region Properties

        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Program { get; set; }
        public string Year { get; set; }
        public string App { get; set; }
        public string MatCode { get; set; }
        public string SclCode { get; set; }
        public string Type { get; set; }
        public string SSDate { get; set; }
        public string QuesCode { get; set; }
        public string RespCode { get; set; }
        public string RespDesc { get; set; }
        public string Points { get; set; }
        public string Change { get; set; }
        public string Pn { get; set; }
        public string SixReasons { get; set; }
        public string ByPass { get; set; }
        public string AddDate { get; set; }
        public string AddOperator { get; set; }
        public string DateLstc { get; set; }
        public string LstcOperator { get; set; }
        public string Mode { get; set; }
        public string SqlType { get; set; }
        public string Rec_Type { get; set; }
        public string ScrDesc { get; set; }
        public string RespExcel { get; set; }
        public string Mat_BM_Code { get; set; }
        public string FamSeq { get; set; }
        public string propScale { get; set; }
        public string propSeq { get; set; }
        public string propBenchMark { get; set; }
        public string propScore { get; set; }
        public string propAssedate { get; set; }
        public string propStatus { get; set; }
        public string propStatusValue { get; set; }
        public string propScale1 { get; set; }
        public string propSeq1 { get; set; }
        public string propScaleCode { get; set; }
        public string propFamilySeq { get; set; }
        public string propBenchmarkcode { get; set; }
        public string propCalcBMark { get; set; }
        public string propCalcBmarkStatus { get; set; }
        public string propCalcBSwitch { get; set; }
        public string propRespExcel { get; set; }
        public string propAssessmentType { get; set; }
        public string PointsSwitch { get; set; }
        public string BenchMarkScoreType { get; set; }
        #endregion

    }


    public class MATB0003Entity
    {
        #region Constructors

        public MATB0003Entity()
        {
            App = string.Empty;
            MST_FAMILY_ID = string.Empty;
            Name = string.Empty;
            SNP_SEX = string.Empty;
            ETHNIC = string.Empty;
            RACE = string.Empty;
            MatDesc = string.Empty;
            SclcodeDesc = string.Empty;
            MATASMT_RESP_DESC = string.Empty;
            Points = string.Empty;
            SNP_ALT_BDATE = string.Empty;
            Site = string.Empty;
            SCALE = string.Empty;
            SSDate=string.Empty;
            Assessment_Type=string.Empty;
        }

        public MATB0003Entity(bool Initialize)
        {
            App = null;
            MST_FAMILY_ID = null;
            Name = null;
            SNP_SEX = null;
            ETHNIC = null;
            RACE = null;
            MatDesc = null;
            SclcodeDesc = null;
            MATASMT_RESP_DESC = null;
            Points = null;
            SNP_ALT_BDATE = null;
            Site = null;
            SCALE = null;
            SSDate = null;
            Assessment_Type = null;
        }

        public MATB0003Entity(DataRow Ques, string strTable)
        {
            if (Ques != null)
            {
                App = Ques["MST_APP_NO"].ToString();
                MST_FAMILY_ID = Ques["MST_FAMILY_ID"].ToString();
                Name = Ques["NAME"].ToString();
                SNP_SEX = Ques["SNP_SEX"].ToString();
                ETHNIC = Ques["ETHNIC"].ToString();
                RACE = Ques["RACE"].ToString();
                MatDesc = Ques["MATRIX"].ToString();
                SclcodeDesc = Ques["SCALE"].ToString(); ;
                MATASMT_RESP_DESC = Ques["MATASMT_RESP_DESC"].ToString(); ;
                Points = Ques["MATASMT_POINTS"].ToString();
                SNP_ALT_BDATE = Ques["SNP_ALT_BDATE"].ToString(); ;
                Site = Ques["MST_SITE"].ToString(); ;
                SCALE = Ques["MATASMT_SCL_CODE"].ToString();
                SSDate = Ques["MATASMT_SS_DATE"].ToString();
                    //CalcBmark = Ques["MATDEF_CALC_BMARK"].ToString();
                    //SSOverRide = Ques["MATDEF_SS_OVERIDE"].ToString();
                    //AddDate = Ques["MATDEF_DATE_ADD"].ToString();
                    //AddOperator = Ques["MATDEF_ADD_OPERATOR"].ToString();
                    //DateLstc = Ques["MATDEF_DATE_LSTC"].ToString();
                    //LstcOperator = Ques["MATDEF_LSTC_OPERATOR"].ToString();
                Assessment_Type = Ques["Assesment"].ToString();
            }

        }

        //public MATB0003Entity(MATB0003Entity Entity)
        //{
        //    if (Entity != null)
        //    {
        //        MST_FAMILY_ID = Entity.MST_FAMILY_ID;
        //        Dept = Entity.Dept;
        //        Program = Entity.Program;
        //        Year = Entity.Year;
        //        App = Entity.App;
        //        MatCode = Entity.MatCode;
        //        SSworker = Entity.SSworker;
        //        SSDate = Entity.SSDate;
        //        AddDate = Entity.AddDate;
        //        AddOperator = Entity.AddOperator;
        //        DateLstc = Entity.DateLstc;
        //        LstcOperator = Entity.LstcOperator;
        //    }
        //}

        #endregion

        #region Properties

        public string Agency { get; set; }
        public string Dept { get; set; }
        public string Program { get; set; }
        public string Year { get; set; }
        public string App { get; set; }
        public string MatCode { get; set; }
        public string MatDesc { get; set; }
        public string SSDate { get; set; }
        public string SSworker { get; set; }
        public string MST_FAMILY_ID { get; set; }
        public string SNP_SEX { get; set; }
        public string ETHNIC { get; set; }
        public string Mode { get; set; }
        public string RACE { get; set; }
        public string SCALE { get; set; }
        public string Name { get; set; }
        public string SclcodeDesc { get; set; }
        public string MATASMT_RESP_DESC { get; set; }
        public string SclCode { get; set; }
        public string Points { get; set; }
        public string SNP_ALT_BDATE { get; set; }
        public string Site { get; set; }
        public string Assessment_Type { get; set; }
        #endregion

    }

    public class MATGroupEntity
    {
        #region Constructors

        public MATGroupEntity()
        {
            Rec_Type = string.Empty;
            MatCode = string.Empty;
            Code = string.Empty;
           
            Desc = string.Empty;
            Add_Date = string.Empty;
            Add_Operator = string.Empty;
            Lstc_Date = string.Empty;
            Lstc_Operator = string.Empty;
            ShortName = string.Empty;
            Seq = string.Empty;
        }

        public MATGroupEntity(bool Initialize)
        {
            Rec_Type = null;
            MatCode = null;           
            Code = null;
            Desc = null;
            Add_Date = null;
            Add_Operator = null;
            Lstc_Date = null;
            Lstc_Operator = null;
            ShortName = null;
            Seq = null;
        }

        public MATGroupEntity(DataRow MATReasn)
        {
            if (MATReasn != null)
            {
                Rec_Type = "U";
                Code = MATReasn["SCLGRP_GRP_CODE"].ToString();
                Desc = MATReasn["SCLGRP_GRP_DESC"].ToString();
                MatCode = MATReasn["SCLGRP_MAT_CODE"].ToString();
                Add_Date = MATReasn["SCLGRP_DATE_ADD"].ToString();
                Add_Operator = MATReasn["SCLGRP_ADD_OPERATOR"].ToString();
                Lstc_Date = MATReasn["SCLGRP_DATE_LSTC"].ToString();
                Lstc_Operator = MATReasn["SCLGRP_LSTC_OPERATOR"].ToString();
                ShortName = MATReasn["SCLGRP_GRP_SHORTNAME"].ToString();
                Seq = MATReasn["SCLGRP_GRP_SEQ"].ToString();
            }
        }

        public MATGroupEntity(MATGroupEntity Entity)
        {
            if (Entity != null)
            {
                Rec_Type = Entity.Rec_Type;
                Code = Entity.Code;             
                Desc = Entity.Desc;
                MatCode = Entity.MatCode;                
                Add_Date = Entity.Add_Date;
                Add_Operator = Entity.Add_Operator;
                Lstc_Date = Entity.Lstc_Date;
                Lstc_Operator = Entity.Lstc_Operator;
                ShortName  = Entity.ShortName;
                Seq = Entity.Seq;  
            }
        }

        #endregion

        #region Properties

        public string Rec_Type { get; set; }
        public string MatCode { get; set; }       
        public string Code { get; set; }
        public string Desc { get; set; }       
        public string Mode { get; set; }
        public string Type { get; set; }
        public string Add_Date { get; set; }
        public string Add_Operator { get; set; }
        public string Lstc_Date { get; set; }
        public string Lstc_Operator { get; set; }
        public string ShortName { get; set; }
        public string Seq { get; set; }       
        #endregion
    }

}
