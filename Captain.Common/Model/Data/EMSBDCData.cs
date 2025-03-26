using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Captain.Common.Model.Objects;
using System.Data.SqlClient;
using Captain.DatabaseLayer;
using System.Data;

namespace Captain.Common.Model.Data
{
    public class EMSBDCData
    {
        public EMSBDCData(CaptainModel model)
        {
            Model = model;
        }

        #region Properties

        public CaptainModel Model { get; set; }

        #endregion

        public bool InsertUpdateDelEmsbdc(EMSBDCEntity EmsbdcEntity, out string strMsg)
        {
            bool boolstatus = false;
            string strerrormsg = string.Empty;
            try
            {

                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@BDC_AGENCY", EmsbdcEntity.BDC_AGENCY));
                sqlParamList.Add(new SqlParameter("@BDC_DEPT", EmsbdcEntity.BDC_DEPT));
                sqlParamList.Add(new SqlParameter("@BDC_PROGRAM", EmsbdcEntity.BDC_PROGRAM));
                if (EmsbdcEntity.BDC_YEAR.Trim() != string.Empty)
                    sqlParamList.Add(new SqlParameter("@BDC_YEAR", EmsbdcEntity.BDC_YEAR));
                sqlParamList.Add(new SqlParameter("@BDC_COST_CENTER", EmsbdcEntity.BDC_COST_CENTER));
                if (EmsbdcEntity.BDC_GL_ACCOUNT != string.Empty) sqlParamList.Add(new SqlParameter("@BDC_GL_ACCOUNT", EmsbdcEntity.BDC_GL_ACCOUNT));

                if (EmsbdcEntity.BDC_BUDGET_YEAR != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@BDC_BUDGET_YEAR", EmsbdcEntity.BDC_BUDGET_YEAR));
                }
                if (EmsbdcEntity.BDC_DESCRIPTION != string.Empty)
                    sqlParamList.Add(new SqlParameter("@BDC_DESCRIPTION", EmsbdcEntity.BDC_DESCRIPTION));


                if (EmsbdcEntity.BDC_FUND != string.Empty)
                    sqlParamList.Add(new SqlParameter("@BDC_FUND", EmsbdcEntity.BDC_FUND));
                if (EmsbdcEntity.BDC_BUDGET != string.Empty)
                    sqlParamList.Add(new SqlParameter("@BDC_BUDGET", EmsbdcEntity.BDC_BUDGET));
                if (EmsbdcEntity.BDC_START != string.Empty)
                    sqlParamList.Add(new SqlParameter("@BDC_START", EmsbdcEntity.BDC_START));
                if (EmsbdcEntity.BDC_END != string.Empty)
                    sqlParamList.Add(new SqlParameter("@BDC_END", EmsbdcEntity.BDC_END));
                if (EmsbdcEntity.BDC_ACCOUNT_TYPE != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@BDC_ACCOUNT_TYPE", EmsbdcEntity.BDC_ACCOUNT_TYPE));
                }
                if (EmsbdcEntity.BDC_INT_ORDER != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@BDC_INT_ORDER", EmsbdcEntity.BDC_INT_ORDER));
                }
                if (EmsbdcEntity.BDC_AUDIT_FLAG != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@BDC_AUDIT_FLAG", EmsbdcEntity.BDC_AUDIT_FLAG));
                }
                if (EmsbdcEntity.BDC_ALLOW_POSTING != string.Empty)
                    sqlParamList.Add(new SqlParameter("@BDC_ALLOW_POSTING", EmsbdcEntity.BDC_ALLOW_POSTING));
                if (EmsbdcEntity.BDC_SWEEP_DAYS != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@BDC_SWEEP_DAYS", EmsbdcEntity.BDC_SWEEP_DAYS));
                }

                if (EmsbdcEntity.BDC_BALANCE != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@BDC_BALANCE", EmsbdcEntity.BDC_BALANCE));
                }

                if (EmsbdcEntity.BDC_BAL_DATE != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@BDC_BAL_DATE", EmsbdcEntity.BDC_BAL_DATE));
                }

                if (EmsbdcEntity.BDC_TOT_INV != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@BDC_TOT_INV", EmsbdcEntity.BDC_TOT_INV));
                }

                if (EmsbdcEntity.BDC_TOT_COMMIT != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@BDC_TOT_COMMIT", EmsbdcEntity.BDC_TOT_COMMIT));
                }
                sqlParamList.Add(new SqlParameter("@BDC_ADD_OPERATOR", EmsbdcEntity.BDC_ADD_OPERATOR));
                sqlParamList.Add(new SqlParameter("@BDC_LSTC_OPERATOR", EmsbdcEntity.BDC_LSTC_OPERATOR));
                if (EmsbdcEntity.BDC_CONTR_NUM != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@BDC_CONTR_NUM", EmsbdcEntity.BDC_CONTR_NUM));
                }
                sqlParamList.Add(new SqlParameter("@BDC_ALLOW_ZERO", EmsbdcEntity.BDC_ALLOW_ZERO));
                sqlParamList.Add(new SqlParameter("@BDC_INV_UPLD", EmsbdcEntity.BDC_INV_UPLD));
                sqlParamList.Add(new SqlParameter("@Mode", EmsbdcEntity.Mode));
                SqlParameter sqlsMsg = new SqlParameter("@Msg", SqlDbType.VarChar, 10);
                sqlsMsg.Value = string.Empty;
                sqlsMsg.Direction = ParameterDirection.Output;
                sqlParamList.Add(sqlsMsg);
                boolstatus = EMSBDCDB.InsertUpdateDelEmsbdc(sqlParamList);
                strerrormsg = sqlsMsg.Value.ToString();
            }

            catch (Exception ex)
            {
                boolstatus = false;
            }
            strMsg = strerrormsg;
            return boolstatus;
        }

        public List<EMSBDCEntity> GetEmsBdcAllData(string agency, string dep, string program, string year, string costcenter, string GlAccount, string BudgetYear, string Desc, string Fund, string IntOrder, string AccountType)
        {
            List<EMSBDCEntity> Emsbdcdata = new List<EMSBDCEntity>();
            try
            {
                DataSet emsbdcds = Captain.DatabaseLayer.EMSBDCDB.GetEmsBdcData(agency, dep, program, year, costcenter, GlAccount, BudgetYear, Desc, Fund, IntOrder, AccountType);
                if (emsbdcds != null && emsbdcds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in emsbdcds.Tables[0].Rows)
                    {
                        Emsbdcdata.Add(new EMSBDCEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return Emsbdcdata;
            }

            return Emsbdcdata;
        }

        public List<EMSBDCEntity> GetEmsB0014Report(string agency, string dep, string program, string year, string Fund, string Fromdate, string Todate)
        {
            List<EMSBDCEntity> Emsbdcdata = new List<EMSBDCEntity>();
            try
            {
                DataSet emsbdcds = Captain.DatabaseLayer.EMSBDCDB.GetEmsB3014Report(agency, dep, program, year, Fund, Fromdate, Todate);
                if (emsbdcds != null && emsbdcds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in emsbdcds.Tables[0].Rows)
                    {
                        Emsbdcdata.Add(new EMSBDCEntity(row, "EMSB0014"));
                    }
                }
            }
            catch (Exception ex)
            {
                return Emsbdcdata;
            }

            return Emsbdcdata;
        }

        public List<EMSBDCEntity> GetEmsBdcFundCheck(string agency, string dep, string program, string year, string Fund, string startDate, string EndDate, string NewstartDate, string NewEndDate, string strType)
        {
            List<EMSBDCEntity> Emsbdcdata = new List<EMSBDCEntity>();
            try
            {
                DataSet emsbdcds = Captain.DatabaseLayer.EMSBDCDB.GetEmsBdcFundCheck(agency, dep, program, year, Fund, startDate, EndDate, NewstartDate, NewEndDate, strType);
                if (emsbdcds != null && emsbdcds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in emsbdcds.Tables[0].Rows)
                    {
                        Emsbdcdata.Add(new EMSBDCEntity(row, strType));
                    }
                }
            }
            catch (Exception ex)
            {
                return Emsbdcdata;
            }

            return Emsbdcdata;
        }

        public List<EMSBDCEntity> GetEMSBDCCalAmount(string agency, string dep, string program, string year, string Fund, string startDate, string EndDate, string strType, string strBudgetYear, string strBudget)
        {
            List<EMSBDCEntity> Emsbdcdata = new List<EMSBDCEntity>();
            try
            {
                DataSet emsbdcds = Captain.DatabaseLayer.EMSBDCDB.GetEMSBDCCalAmount(agency, dep, program, year, Fund, startDate, EndDate, strType, strBudgetYear, strBudget);
                if (emsbdcds != null && emsbdcds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in emsbdcds.Tables[0].Rows)
                    {
                        Emsbdcdata.Add(new EMSBDCEntity(row, strType));
                    }
                }
            }
            catch (Exception ex)
            {
                return Emsbdcdata;
            }

            return Emsbdcdata;
        }



        public EMSBDCEntity GetEmsBdcData(string agency, string dep, string program, string year, string costcenter, string GlAccount, string BudgetYear, string Desc, string Fund, string IntOrder, string Accountype)
        {
            EMSBDCEntity emsbdcDetail = null;
            try
            {
                DataSet emsbdcds = Captain.DatabaseLayer.EMSBDCDB.GetEmsBdcData(agency, dep, program, year, costcenter, GlAccount, BudgetYear, Desc, Fund, IntOrder, Accountype);
                if (emsbdcds != null && emsbdcds.Tables[0].Rows.Count > 0)
                {
                    emsbdcDetail = new EMSBDCEntity(emsbdcds.Tables[0].Rows[0]);
                }
            }
            catch (Exception ex)
            {
                //
                return emsbdcDetail;
            }

            return emsbdcDetail;
        }

        public List<EMSBDAEntity> GetEmsBdaAllData(string agency, string dep, string program, string year, string costcenter, string GlAccount, string BudgetYear, string seq)
        {
            List<EMSBDAEntity> Emsbdadata = new List<EMSBDAEntity>();
            try
            {
                DataSet emsbdads = Captain.DatabaseLayer.EMSBDCDB.GetEmsBdaData(agency, dep, program, year, costcenter, GlAccount, BudgetYear, seq);
                if (emsbdads != null && emsbdads.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in emsbdads.Tables[0].Rows)
                    {
                        Emsbdadata.Add(new EMSBDAEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return Emsbdadata;
            }

            return Emsbdadata;
        }


        public bool InsertUpdateDelEmsbda(EMSBDAEntity EmsbdaEntity)
        {
            bool boolstatus = false;
            try
            {

                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@BDA_AGENCY", EmsbdaEntity.BDA_AGENCY));
                sqlParamList.Add(new SqlParameter("@BDA_DEPT", EmsbdaEntity.BDA_DEPT));
                sqlParamList.Add(new SqlParameter("@BDA_PROGRAM", EmsbdaEntity.BDA_PROGRAM));
                if (EmsbdaEntity.BDA_YEAR != string.Empty)
                    sqlParamList.Add(new SqlParameter("@BDA_YEAR", EmsbdaEntity.BDA_YEAR));
                sqlParamList.Add(new SqlParameter("@BDA_COST_CENTER", EmsbdaEntity.BDA_COST_CENTER));
                if (EmsbdaEntity.BDA_GL_ACCOUNT != string.Empty) sqlParamList.Add(new SqlParameter("@BDA_GL_ACCOUNT", EmsbdaEntity.BDA_GL_ACCOUNT));

                if (EmsbdaEntity.BDA_BUDGET_YEAR != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@BDA_BUDGET_YEAR", EmsbdaEntity.BDA_BUDGET_YEAR));
                }
                if (EmsbdaEntity.BDA_OLD_DESC != string.Empty)
                    sqlParamList.Add(new SqlParameter("@BDA_OLD_DESC", EmsbdaEntity.BDA_OLD_DESC));
                if (EmsbdaEntity.BDA_NEW_DESC != string.Empty)
                    sqlParamList.Add(new SqlParameter("@BDA_NEW_DESC", EmsbdaEntity.BDA_NEW_DESC));


                if (EmsbdaEntity.BDA_OLD_FUND != string.Empty)
                    sqlParamList.Add(new SqlParameter("@BDA_OLD_FUND", EmsbdaEntity.BDA_OLD_FUND));
                if (EmsbdaEntity.BDA_NEW_FUND != string.Empty)
                    sqlParamList.Add(new SqlParameter("@BDA_NEW_FUND", EmsbdaEntity.BDA_NEW_FUND));

                if (EmsbdaEntity.BDA_OLD_BUDGET != string.Empty)
                    sqlParamList.Add(new SqlParameter("@BDA_OLD_BUDGET", EmsbdaEntity.BDA_OLD_BUDGET));
                if (EmsbdaEntity.BDA_NEW_BUDGET != string.Empty)
                    sqlParamList.Add(new SqlParameter("@BDA_NEW_BUDGET", EmsbdaEntity.BDA_NEW_BUDGET));

                if (EmsbdaEntity.BDA_OLD_START != string.Empty)
                    sqlParamList.Add(new SqlParameter("@BDA_OLD_START", EmsbdaEntity.BDA_OLD_START));
                if (EmsbdaEntity.BDA_NEW_START != string.Empty)
                    sqlParamList.Add(new SqlParameter("@BDA_NEW_START", EmsbdaEntity.BDA_NEW_START));

                if (EmsbdaEntity.BDA_OLD_END != string.Empty)
                    sqlParamList.Add(new SqlParameter("@BDA_OLD_END", EmsbdaEntity.BDA_OLD_END));
                if (EmsbdaEntity.BDA_NEW_END != string.Empty)
                    sqlParamList.Add(new SqlParameter("@BDA_NEW_END", EmsbdaEntity.BDA_NEW_END));

                if (EmsbdaEntity.BDA_OLD_ACCT_TYPE != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@BDA_OLD_ACCT_TYPE", EmsbdaEntity.BDA_OLD_ACCT_TYPE));
                }
                if (EmsbdaEntity.BDA_NEW_ACCT_TYPE != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@BDA_NEW_ACCT_TYPE", EmsbdaEntity.BDA_NEW_ACCT_TYPE));
                }
                if (EmsbdaEntity.BDA_OLD_INT_ORDER != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@BDA_OLD_INT_ORDER", EmsbdaEntity.BDA_OLD_INT_ORDER));
                }
                if (EmsbdaEntity.BDA_NEW_INT_ORDER != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@BDA_NEW_INT_ORDER", EmsbdaEntity.BDA_NEW_INT_ORDER));
                }
                if (EmsbdaEntity.BDA_OLD_AUDIT_FLAG != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@BDA_OLD_AUDIT_FLAG", EmsbdaEntity.BDA_OLD_AUDIT_FLAG));
                }
                if (EmsbdaEntity.BDA_NEW_AUDIT_FLAG != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@BDA_NEW_AUDIT_FLAG", EmsbdaEntity.BDA_NEW_AUDIT_FLAG));
                }
                if (EmsbdaEntity.BDA_OLD_ALLOW_POST != string.Empty)
                    sqlParamList.Add(new SqlParameter("@BDA_OLD_ALLOW_POST", EmsbdaEntity.BDA_OLD_ALLOW_POST));

                if (EmsbdaEntity.BDA_NEW_ALLOW_POST != string.Empty)
                    sqlParamList.Add(new SqlParameter("@BDA_NEW_ALLOW_POST", EmsbdaEntity.BDA_NEW_ALLOW_POST));

                if (EmsbdaEntity.BDA_OLD_SWEEP_DAYS != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@BDA_OLD_SWEEP_DAYS", EmsbdaEntity.BDA_OLD_SWEEP_DAYS));
                }
                if (EmsbdaEntity.BDA_NEW_SWEEP_DAYS != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@BDA_NEW_SWEEP_DAYS", EmsbdaEntity.BDA_NEW_SWEEP_DAYS));
                }

                if (EmsbdaEntity.BDA_REASON_DESC != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@BDA_REASON_DESC", EmsbdaEntity.BDA_REASON_DESC));
                }

                if (EmsbdaEntity.BDA_REASON_CODE != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@BDA_REASON_CODE", EmsbdaEntity.BDA_REASON_CODE));
                }

                if (EmsbdaEntity.BDA_DATE_CHANGD != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@BDA_DATE_CHANGD", EmsbdaEntity.BDA_DATE_CHANGD));
                }

                if (EmsbdaEntity.BDA_COMNT1 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@BDA_COMNT1", EmsbdaEntity.BDA_COMNT1));
                }
                if (EmsbdaEntity.BDA_COMNT2 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@BDA_COMNT2", EmsbdaEntity.BDA_COMNT2));
                }
                if (EmsbdaEntity.BDA_OLD_ALLOW_ZERO != string.Empty)
                    sqlParamList.Add(new SqlParameter("@BDA_OLD_ALLOW_ZERO", EmsbdaEntity.BDA_OLD_ALLOW_ZERO));

                if (EmsbdaEntity.BDA_NEW_ALLOW_ZERO != string.Empty)
                    sqlParamList.Add(new SqlParameter("@BDA_NEW_ALLOW_ZERO", EmsbdaEntity.BDA_NEW_ALLOW_ZERO));

                sqlParamList.Add(new SqlParameter("@BDA_DATE_ADD", EmsbdaEntity.BDA_DATE_ADD));
                sqlParamList.Add(new SqlParameter("@BDA_ADD_OPERATOR", EmsbdaEntity.BDA_ADD_OPERATOR));
                sqlParamList.Add(new SqlParameter("@Mode", EmsbdaEntity.Mode));
                boolstatus = EMSBDCDB.InsertUpdateDelEmsbda(sqlParamList);
            }

            catch (Exception ex)
            {
                return false;
            }

            return boolstatus;
        }


        public EMSCLCPMCEntity GetEmsclcpmcData(string agency, string dep, string program, string year, string App, string Fund, string Fundseq, string ServiceSeq, string strDate)
        {
            EMSCLCPMCEntity emsclcpmDetail = null;
            try
            {
                DataSet emsclcpmds = Captain.DatabaseLayer.EMSBDCDB.GetEmsclcpmcData(agency, dep, program, year, App, Fund, Fundseq, ServiceSeq, strDate);
                if (emsclcpmds != null && emsclcpmds.Tables[0].Rows.Count > 0)
                {
                    emsclcpmDetail = new EMSCLCPMCEntity(emsclcpmds.Tables[0].Rows[0]);
                }
            }
            catch (Exception ex)
            {
                //
                return emsclcpmDetail;
            }

            return emsclcpmDetail;
        }

        public List<EMSCLCPMCEntity> GetEmsclcpmcAllData(string agency, string dep, string program, string year, string App, string Fund, string Fundseq, string ServiceSeq, string strDate)
        {
            List<EMSCLCPMCEntity> Emsclcpmcdata = new List<EMSCLCPMCEntity>();
            try
            {
                DataSet emsclcpmcds = Captain.DatabaseLayer.EMSBDCDB.GetEmsclcpmcData(agency, dep, program, year, App, Fund, Fundseq, ServiceSeq, strDate);
                if (emsclcpmcds != null && emsclcpmcds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in emsclcpmcds.Tables[0].Rows)
                    {
                        Emsclcpmcdata.Add(new EMSCLCPMCEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return Emsclcpmcdata;
            }

            return Emsclcpmcdata;
        }

        public List<EMSCLCPMCEntity> GetEmsclcpmcAllData0026(string agency, string dep, string program, string year, string Fund)
        {
            List<EMSCLCPMCEntity> Emsclcpmcdata = new List<EMSCLCPMCEntity>();
            try
            {
                DataSet emsclcpmcds = Captain.DatabaseLayer.EMSBDCDB.GetEmsb0026_ClcData(agency, dep, program, year, Fund);
                if (emsclcpmcds != null && emsclcpmcds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in emsclcpmcds.Tables[0].Rows)
                    {
                        Emsclcpmcdata.Add(new EMSCLCPMCEntity(row, "EMSB0026", string.Empty));
                    }
                }
            }
            catch (Exception ex)
            {
                return Emsclcpmcdata;
            }

            return Emsclcpmcdata;
        }


        public List<EMSCLCPMCEntity> GetEMSCLCPMC00030Details(string agency, string dep, string program, string year, string App, string Fund, string servicecode, string Site, string strDate, string strDateH, string strCLC_RES_SEQ, string strCLC_SEQ, string strCLC_RES_DATE, string strType,string strVendorL, string strVendorH)
        {
            List<EMSCLCPMCEntity> Emsclcpmcdata = new List<EMSCLCPMCEntity>();
            try
            {
                DataSet emsclcpmcds = Captain.DatabaseLayer.EMSBDCDB.GetEMSCLCPMC00030Details(agency, dep, program, year, App, Fund, servicecode, Site, strDate, strDateH, strCLC_RES_SEQ, strCLC_SEQ, strCLC_RES_DATE, strType,strVendorL,strVendorH);
                if (emsclcpmcds != null && emsclcpmcds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in emsclcpmcds.Tables[0].Rows)
                    {
                        Emsclcpmcdata.Add(new EMSCLCPMCEntity(row, "EMS00030"));
                    }
                }
            }
            catch (Exception ex)
            {
                return Emsclcpmcdata;
            }

            return Emsclcpmcdata;
        }



        public bool InsertUpdateDelEmsclcpmc(EMSCLCPMCEntity EmsclcpmcEntity, out string strSId)
        {
            bool boolstatus = false;
            string strNewSid = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                if (EmsclcpmcEntity.CLC_AGENCY != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_AGENCY", EmsclcpmcEntity.CLC_AGENCY));
                if (EmsclcpmcEntity.CLC_DEPT != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_DEPT", EmsclcpmcEntity.CLC_DEPT));
                if (EmsclcpmcEntity.CLC_PROGRAM != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_PROGRAM", EmsclcpmcEntity.CLC_PROGRAM));
                if (EmsclcpmcEntity.CLC_YEAR != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_YEAR", EmsclcpmcEntity.CLC_YEAR));
                if (EmsclcpmcEntity.CLC_APP != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_APP", EmsclcpmcEntity.CLC_APP));
                if (EmsclcpmcEntity.CLC_RES_FUND != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_RES_FUND", EmsclcpmcEntity.CLC_RES_FUND));
                if (EmsclcpmcEntity.CLC_RES_SEQ != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_RES_SEQ", EmsclcpmcEntity.CLC_RES_SEQ));
                if (EmsclcpmcEntity.CLC_SEQ != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_SEQ", EmsclcpmcEntity.CLC_SEQ));
                if (EmsclcpmcEntity.CLC_RES_DATE != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_RES_DATE", EmsclcpmcEntity.CLC_RES_DATE));
                if (EmsclcpmcEntity.CLC_S_HEX_NO != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_S_HEX_NO", EmsclcpmcEntity.CLC_S_HEX_NO));
                if (EmsclcpmcEntity.CLC_S_OBO != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_S_OBO", EmsclcpmcEntity.CLC_S_OBO));
                if (EmsclcpmcEntity.CLC_S_CGN != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_S_CGN", EmsclcpmcEntity.CLC_S_CGN));
                if (EmsclcpmcEntity.CLC_S_CASEWORKER != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_S_CASEWORKER", EmsclcpmcEntity.CLC_S_CASEWORKER));
                if (EmsclcpmcEntity.CLC_S_SERVICE_CODE != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_S_SERVICE_CODE", EmsclcpmcEntity.CLC_S_SERVICE_CODE));
                if (EmsclcpmcEntity.CLC_S_VENDOR != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_S_VENDOR", EmsclcpmcEntity.CLC_S_VENDOR));
                if (EmsclcpmcEntity.CLC_S_ACCT != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_S_ACCT", EmsclcpmcEntity.CLC_S_ACCT));
                if (EmsclcpmcEntity.CLC_S_BIL_LNAME != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_S_BIL_LNAME", EmsclcpmcEntity.CLC_S_BIL_LNAME));
                if (EmsclcpmcEntity.CLC_S_BIL_FNAME != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_S_BIL_FNAME", EmsclcpmcEntity.CLC_S_BIL_FNAME));
                if (EmsclcpmcEntity.CLC_S_DECISION != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_S_DECISION", EmsclcpmcEntity.CLC_S_DECISION));
                if (EmsclcpmcEntity.CLC_S_DECSN_DATE != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_S_DECSN_DATE", EmsclcpmcEntity.CLC_S_DECSN_DATE));
                if (EmsclcpmcEntity.CLC_S_APPEAL != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_S_APPEAL", EmsclcpmcEntity.CLC_S_APPEAL));
                if (EmsclcpmcEntity.CLC_S_VOUCHER != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_S_VOUCHER", EmsclcpmcEntity.CLC_S_VOUCHER));
                if (EmsclcpmcEntity.CLC_S_FOL_DATE != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_S_FOL_DATE", EmsclcpmcEntity.CLC_S_FOL_DATE));
                if (EmsclcpmcEntity.CLC_S_FOLC_DATE != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_S_FOLC_DATE", EmsclcpmcEntity.CLC_S_FOLC_DATE));
                if (EmsclcpmcEntity.CLC_S_BEN_START != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_S_BEN_START", EmsclcpmcEntity.CLC_S_BEN_START));
                if (EmsclcpmcEntity.CLC_S_BEN_END != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_S_BEN_END", EmsclcpmcEntity.CLC_S_BEN_END));
                if (EmsclcpmcEntity.CLC_S_COST_CENTER != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_S_COST_CENTER", EmsclcpmcEntity.CLC_S_COST_CENTER));
                if (EmsclcpmcEntity.CLC_S_GL_ACCOUNT != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_S_GL_ACCOUNT", EmsclcpmcEntity.CLC_S_GL_ACCOUNT));
                if (EmsclcpmcEntity.CLC_S_COUNTY_YEAR != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_S_COUNTY_YEAR", EmsclcpmcEntity.CLC_S_COUNTY_YEAR));
                if (EmsclcpmcEntity.CLC_S_TEMP_AWARD != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_S_TEMP_AWARD", EmsclcpmcEntity.CLC_S_TEMP_AWARD));
                if (EmsclcpmcEntity.CLC_TMP_NPUSER != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_TMP_NPUSER", EmsclcpmcEntity.CLC_TMP_NPUSER));
                if (EmsclcpmcEntity.CLC_TMP_NPDATE != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_TMP_NPDATE", EmsclcpmcEntity.CLC_TMP_NPDATE));
                if (EmsclcpmcEntity.PMC_PAY_KEY != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_PAY_KEY", EmsclcpmcEntity.PMC_PAY_KEY));
                if (EmsclcpmcEntity.PMC_TYPE != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_TYPE", EmsclcpmcEntity.PMC_TYPE));
                if (EmsclcpmcEntity.PMC_CASEWORKER != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_CASEWORKER", EmsclcpmcEntity.PMC_CASEWORKER));
                if (EmsclcpmcEntity.PMC_DATE != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_DATE", EmsclcpmcEntity.PMC_DATE));
                if (EmsclcpmcEntity.PMC_AMOUNT != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_AMOUNT", EmsclcpmcEntity.PMC_AMOUNT));
                if (EmsclcpmcEntity.PMC_CHECK_DATE != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_CHECK_DATE", EmsclcpmcEntity.PMC_CHECK_DATE));
                if (EmsclcpmcEntity.PMC_CHECK_NO != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_CHECK_NO", EmsclcpmcEntity.PMC_CHECK_NO));
                if (EmsclcpmcEntity.PMC_CLOSE_LVL1_USER != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_CLOSE_LVL1_USER", EmsclcpmcEntity.PMC_CLOSE_LVL1_USER));
                if (EmsclcpmcEntity.PMC_AGENCY1 != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_AGENCY1", EmsclcpmcEntity.PMC_AGENCY1));
                if (EmsclcpmcEntity.PMC_DEPT1 != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_DEPT1", EmsclcpmcEntity.PMC_DEPT1));
                if (EmsclcpmcEntity.PMC_PROGRAM1 != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_PROGRAM1", EmsclcpmcEntity.PMC_PROGRAM1));
                if (EmsclcpmcEntity.PMC_YEAR1 != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_YEAR1", EmsclcpmcEntity.PMC_YEAR1));
                if (EmsclcpmcEntity.PMC_CLOSE_LVL1_DATE != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_CLOSE_LVL1_DATE", EmsclcpmcEntity.PMC_CLOSE_LVL1_DATE));
                if (EmsclcpmcEntity.PMC_CLOSE_LVL2_USER != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_CLOSE_LVL2_USER", EmsclcpmcEntity.PMC_CLOSE_LVL2_USER));
                if (EmsclcpmcEntity.PMC_AGENCY2 != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_AGENCY2", EmsclcpmcEntity.PMC_AGENCY2));
                if (EmsclcpmcEntity.PMC_DEPT2 != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_DEPT2", EmsclcpmcEntity.PMC_DEPT2));
                if (EmsclcpmcEntity.PMC_PROGRAM2 != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_PROGRAM2", EmsclcpmcEntity.PMC_PROGRAM2));
                if (EmsclcpmcEntity.PMC_YEAR2 != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_YEAR2", EmsclcpmcEntity.PMC_YEAR2));
                if (EmsclcpmcEntity.PMC_CLOSE_LVL2_DATE != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_CLOSE_LVL2_DATE", EmsclcpmcEntity.PMC_CLOSE_LVL2_DATE));
                if (EmsclcpmcEntity.PMC_AUTH_FOOD_VOUCHER != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_AUTH_FOOD_VOUCHER", EmsclcpmcEntity.PMC_AUTH_FOOD_VOUCHER));
                if (EmsclcpmcEntity.PMC_AUTH_LIQUIDATE != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_AUTH_LIQUIDATE", EmsclcpmcEntity.PMC_AUTH_LIQUIDATE));
                if (EmsclcpmcEntity.PMC_AUTH_AMT != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_AUTH_AMT", EmsclcpmcEntity.PMC_AUTH_AMT));
                if (EmsclcpmcEntity.PMC_AUTH_WORKER != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_AUTH_WORKER", EmsclcpmcEntity.PMC_AUTH_WORKER));
                if (EmsclcpmcEntity.PMC_AUTH_DATE != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_AUTH_DATE", EmsclcpmcEntity.PMC_AUTH_DATE));
                if (EmsclcpmcEntity.PMC_AUTH_AGENCY != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_AUTH_AGENCY", EmsclcpmcEntity.PMC_AUTH_AGENCY));
                if (EmsclcpmcEntity.PMC_AUTH_DEPT != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_AUTH_DEPT", EmsclcpmcEntity.PMC_AUTH_DEPT));
                if (EmsclcpmcEntity.PMC_AUTH_PROGRAM != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_AUTH_PROGRAM", EmsclcpmcEntity.PMC_AUTH_PROGRAM));
                if (EmsclcpmcEntity.PMC_AUTH_YEAR != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_AUTH_YEAR", EmsclcpmcEntity.PMC_AUTH_YEAR));
                if (EmsclcpmcEntity.PMC_AUTH_LVL_DATE != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_AUTH_LVL_DATE", EmsclcpmcEntity.PMC_AUTH_LVL_DATE));
                if (EmsclcpmcEntity.PMC_AUTH_LVL_USER != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_AUTH_LVL_USER", EmsclcpmcEntity.PMC_AUTH_LVL_USER));
                if (EmsclcpmcEntity.PMC_INV_AMT != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_INV_AMT", EmsclcpmcEntity.PMC_INV_AMT));
                if (EmsclcpmcEntity.PMC_INV_BILL_AMT != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_INV_BILL_AMT", EmsclcpmcEntity.PMC_INV_BILL_AMT));
                if (EmsclcpmcEntity.PMC_INV_BILL_DATE != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_INV_BILL_DATE", EmsclcpmcEntity.PMC_INV_BILL_DATE));
                if (EmsclcpmcEntity.PMC_INV_VENDOR_RATING != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_INV_VENDOR_RATING", EmsclcpmcEntity.PMC_INV_VENDOR_RATING));
                if (EmsclcpmcEntity.PMC_INV_WORKER != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_INV_WORKER", EmsclcpmcEntity.PMC_INV_WORKER));
                if (EmsclcpmcEntity.PMC_INV_DATE != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_INV_DATE", EmsclcpmcEntity.PMC_INV_DATE));
                if (EmsclcpmcEntity.PMC_DEPT_REJECT_DATE != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_DEPT_REJECT_DATE", EmsclcpmcEntity.PMC_DEPT_REJECT_DATE));
                if (EmsclcpmcEntity.PMC_REJECT_CODE != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_REJECT_CODE", EmsclcpmcEntity.PMC_REJECT_CODE));
                if (EmsclcpmcEntity.PMC_FILE_NAME != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_FILE_NAME", EmsclcpmcEntity.PMC_FILE_NAME));
                if (EmsclcpmcEntity.PMC_REJECT1_CODE != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_REJECT1_CODE", EmsclcpmcEntity.PMC_REJECT1_CODE));
                if (EmsclcpmcEntity.PMC_REJECT2_CODE != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_REJECT2_CODE", EmsclcpmcEntity.PMC_REJECT2_CODE));
                if (EmsclcpmcEntity.PMC_REJECT3_CODE != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_REJECT3_CODE", EmsclcpmcEntity.PMC_REJECT3_CODE));
                if (EmsclcpmcEntity.PMC_REJECT4_CODE != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_REJECT4_CODE", EmsclcpmcEntity.PMC_REJECT4_CODE));
                if (EmsclcpmcEntity.PMC_REJECT5_CODE != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_REJECT5_CODE", EmsclcpmcEntity.PMC_REJECT5_CODE));
                if (EmsclcpmcEntity.PMC_LIQUID_AMOUNT != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_LIQUID_AMOUNT", EmsclcpmcEntity.PMC_LIQUID_AMOUNT));
                if (EmsclcpmcEntity.CLC_DATE_LSTC != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_DATE_LSTC", EmsclcpmcEntity.CLC_DATE_LSTC));
                if (EmsclcpmcEntity.CLC_LSTC_OPERATOR != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_LSTC_OPERATOR", EmsclcpmcEntity.CLC_LSTC_OPERATOR));
                if (EmsclcpmcEntity.CLC_DATE_ADD != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_DATE_ADD", EmsclcpmcEntity.CLC_DATE_ADD));
                if (EmsclcpmcEntity.CLC_ADD_OPERATOR != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_ADD_OPERATOR", EmsclcpmcEntity.CLC_ADD_OPERATOR));
                if (EmsclcpmcEntity.CLC_LOCK_BY != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_LOCK_BY", EmsclcpmcEntity.CLC_LOCK_BY));
                if (EmsclcpmcEntity.PMC_PAID_TYPE != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_PAID_TYPE", EmsclcpmcEntity.PMC_PAID_TYPE));
                if (EmsclcpmcEntity.CLC_INVLOG_ID != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_INVLOG_ID", EmsclcpmcEntity.CLC_INVLOG_ID));

                
                sqlParamList.Add(new SqlParameter("@Mode", EmsclcpmcEntity.Mode));
                SqlParameter sqlsId = new SqlParameter("@CLC_S_ID", SqlDbType.VarChar, 100);
                sqlsId.Value = EmsclcpmcEntity.CLC_S_ID;
                sqlsId.Direction = ParameterDirection.Output;
                sqlParamList.Add(sqlsId);
                boolstatus = EMSBDCDB.InsertUpdateDelEmsclcpmc(sqlParamList);
                strNewSid = sqlsId.Value.ToString();
            }

            catch (Exception ex)
            {
                boolstatus = false;
            }
            strSId = strNewSid;
            return boolstatus;
        }


        public bool InsertEmsclcpmcLOG(EMSCLCPMCEntity EmsclcpmcEntity,string ScreenCode, string NewValue,  out string strSId)
        {
            bool boolstatus = false;
            string strNewSid = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                if (EmsclcpmcEntity.CLC_AGENCY != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_AGENCY", EmsclcpmcEntity.CLC_AGENCY));
                if (EmsclcpmcEntity.CLC_DEPT != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_DEPT", EmsclcpmcEntity.CLC_DEPT));
                if (EmsclcpmcEntity.CLC_PROGRAM != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_PROGRAM", EmsclcpmcEntity.CLC_PROGRAM));
                if (EmsclcpmcEntity.CLC_YEAR != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_YEAR", EmsclcpmcEntity.CLC_YEAR));
                if (EmsclcpmcEntity.CLC_APP != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_APP", EmsclcpmcEntity.CLC_APP));
                if (EmsclcpmcEntity.CLC_RES_FUND != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_RES_FUND", EmsclcpmcEntity.CLC_RES_FUND));
                if (EmsclcpmcEntity.CLC_RES_SEQ != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_RES_SEQ", EmsclcpmcEntity.CLC_RES_SEQ));
                if (EmsclcpmcEntity.CLC_SEQ != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_SEQ", EmsclcpmcEntity.CLC_SEQ));
                if (EmsclcpmcEntity.CLC_RES_DATE != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_RES_DATE", EmsclcpmcEntity.CLC_RES_DATE));
                if (EmsclcpmcEntity.CLC_S_HEX_NO != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_S_HEX_NO", EmsclcpmcEntity.CLC_S_HEX_NO));
                if (EmsclcpmcEntity.CLC_S_SERVICE_CODE != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_S_SERVICE_CODE", EmsclcpmcEntity.CLC_S_SERVICE_CODE));
                if (EmsclcpmcEntity.CLC_S_VENDOR != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_S_VENDOR", EmsclcpmcEntity.CLC_S_VENDOR));
                if (EmsclcpmcEntity.CLC_S_ACCT != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_S_ACCT", EmsclcpmcEntity.CLC_S_ACCT));
                if (EmsclcpmcEntity.CLC_S_DECISION != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_S_DECISION", EmsclcpmcEntity.CLC_S_DECISION));
                if (EmsclcpmcEntity.CLC_S_DECSN_DATE != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_S_DECSN_DATE", EmsclcpmcEntity.CLC_S_DECSN_DATE));
                if (EmsclcpmcEntity.CLC_S_COST_CENTER != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_S_COST_CENTER", EmsclcpmcEntity.CLC_S_COST_CENTER));
                if (EmsclcpmcEntity.CLC_S_GL_ACCOUNT != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_S_GL_ACCOUNT", EmsclcpmcEntity.CLC_S_GL_ACCOUNT));
                if (EmsclcpmcEntity.CLC_S_COUNTY_YEAR != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_S_COUNTY_YEAR", EmsclcpmcEntity.CLC_S_COUNTY_YEAR));
                if (EmsclcpmcEntity.CLC_TMP_NPUSER != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_TMP_NPUSER", EmsclcpmcEntity.CLC_TMP_NPUSER));
                if (EmsclcpmcEntity.CLC_TMP_NPDATE != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_TMP_NPDATE", EmsclcpmcEntity.CLC_TMP_NPDATE));
                if (EmsclcpmcEntity.PMC_TYPE != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_TYPE", EmsclcpmcEntity.PMC_TYPE));
                if (EmsclcpmcEntity.PMC_DATE != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_DATE", EmsclcpmcEntity.PMC_DATE));
                if (EmsclcpmcEntity.PMC_AMOUNT != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_AMOUNT", EmsclcpmcEntity.PMC_AMOUNT));
                if (EmsclcpmcEntity.PMC_CHECK_DATE != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_CHECK_DATE", EmsclcpmcEntity.PMC_CHECK_DATE));
                if (EmsclcpmcEntity.PMC_CHECK_NO != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_CHECK_NO", EmsclcpmcEntity.PMC_CHECK_NO));
                if (EmsclcpmcEntity.PMC_CLOSE_LVL1_USER != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_CLOSE_LVL1_USER", EmsclcpmcEntity.PMC_CLOSE_LVL1_USER));
                if (EmsclcpmcEntity.PMC_CLOSE_LVL1_DATE != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_CLOSE_LVL1_DATE", EmsclcpmcEntity.PMC_CLOSE_LVL1_DATE));
                if (EmsclcpmcEntity.PMC_CLOSE_LVL2_USER != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_CLOSE_LVL2_USER", EmsclcpmcEntity.PMC_CLOSE_LVL2_USER));
                if (EmsclcpmcEntity.PMC_CLOSE_LVL2_DATE != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_CLOSE_LVL2_DATE", EmsclcpmcEntity.PMC_CLOSE_LVL2_DATE));
                if (EmsclcpmcEntity.PMC_INV_AMT != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_INV_AMT", EmsclcpmcEntity.PMC_INV_AMT));
                if (EmsclcpmcEntity.PMC_INV_DATE != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_INV_DATE", EmsclcpmcEntity.PMC_INV_DATE));
                if (EmsclcpmcEntity.PMC_FILE_NAME != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_FILE_NAME", EmsclcpmcEntity.PMC_FILE_NAME));
                if (EmsclcpmcEntity.CLC_DATE_LSTC != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_DATE_LSTC", EmsclcpmcEntity.CLC_DATE_LSTC));
                if (EmsclcpmcEntity.CLC_LSTC_OPERATOR != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_LSTC_OPERATOR", EmsclcpmcEntity.CLC_LSTC_OPERATOR));
                if (EmsclcpmcEntity.CLC_DATE_ADD != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_DATE_ADD", EmsclcpmcEntity.CLC_DATE_ADD));
                if (EmsclcpmcEntity.CLC_ADD_OPERATOR != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_ADD_OPERATOR", EmsclcpmcEntity.CLC_ADD_OPERATOR));
                //if (EmsclcpmcEntity.PMC_PAID_TYPE != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_PAID_TYPE", EmsclcpmcEntity.PMC_PAID_TYPE));
                if(!string.IsNullOrEmpty(ScreenCode.Trim())) sqlParamList.Add(new SqlParameter("@CLC_SCREEN_CODE", ScreenCode));

                if (!string.IsNullOrEmpty(NewValue.Trim())) sqlParamList.Add(new SqlParameter("@CLC_NEW_VALUE", NewValue));

                //sqlParamList.Add(new SqlParameter("@Mode", EmsclcpmcEntity.Mode));
                SqlParameter sqlsId = new SqlParameter("@MSG", SqlDbType.VarChar, 100);
                sqlsId.Value = EmsclcpmcEntity.CLC_S_ID;
                sqlsId.Direction = ParameterDirection.Output;
                sqlParamList.Add(sqlsId);
                boolstatus = EMSBDCDB.InsertEmsclcpmcLOG(sqlParamList);
                strNewSid = sqlsId.Value.ToString();
            }

            catch (Exception ex)
            {
                boolstatus = false;
            }
            strSId = strNewSid;
            return boolstatus;
        }

        public bool InsertEmsclcpmcLOG(EMSCLCPMCEntity EmsclcpmcEntity, string ScreenCode, string NewValue, string strNewLVLuser1, string strNewLVLDate1, string strNewLVLuser2, string strNewLVLDate2, string strUpdType, out string strSId)
        {
            bool boolstatus = false;
            string strNewSid = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                if (EmsclcpmcEntity.CLC_AGENCY != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_AGENCY", EmsclcpmcEntity.CLC_AGENCY));
                if (EmsclcpmcEntity.CLC_DEPT != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_DEPT", EmsclcpmcEntity.CLC_DEPT));
                if (EmsclcpmcEntity.CLC_PROGRAM != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_PROGRAM", EmsclcpmcEntity.CLC_PROGRAM));
                if (EmsclcpmcEntity.CLC_YEAR != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_YEAR", EmsclcpmcEntity.CLC_YEAR));
                if (EmsclcpmcEntity.CLC_APP != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_APP", EmsclcpmcEntity.CLC_APP));
                if (EmsclcpmcEntity.CLC_RES_FUND != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_RES_FUND", EmsclcpmcEntity.CLC_RES_FUND));
                if (EmsclcpmcEntity.CLC_RES_SEQ != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_RES_SEQ", EmsclcpmcEntity.CLC_RES_SEQ));
                if (EmsclcpmcEntity.CLC_SEQ != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_SEQ", EmsclcpmcEntity.CLC_SEQ));
                if (EmsclcpmcEntity.CLC_RES_DATE != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_RES_DATE", EmsclcpmcEntity.CLC_RES_DATE));
                if (EmsclcpmcEntity.CLC_S_HEX_NO != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_S_HEX_NO", EmsclcpmcEntity.CLC_S_HEX_NO));
                if (EmsclcpmcEntity.CLC_S_SERVICE_CODE != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_S_SERVICE_CODE", EmsclcpmcEntity.CLC_S_SERVICE_CODE));
                if (EmsclcpmcEntity.CLC_S_VENDOR != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_S_VENDOR", EmsclcpmcEntity.CLC_S_VENDOR));
                if (EmsclcpmcEntity.CLC_S_ACCT != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_S_ACCT", EmsclcpmcEntity.CLC_S_ACCT));
                if (EmsclcpmcEntity.CLC_S_DECISION != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_S_DECISION", EmsclcpmcEntity.CLC_S_DECISION));
                if (EmsclcpmcEntity.CLC_S_DECSN_DATE != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_S_DECSN_DATE", EmsclcpmcEntity.CLC_S_DECSN_DATE));
                if (EmsclcpmcEntity.CLC_S_COST_CENTER != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_S_COST_CENTER", EmsclcpmcEntity.CLC_S_COST_CENTER));
                if (EmsclcpmcEntity.CLC_S_GL_ACCOUNT != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_S_GL_ACCOUNT", EmsclcpmcEntity.CLC_S_GL_ACCOUNT));
                if (EmsclcpmcEntity.CLC_S_COUNTY_YEAR != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_S_COUNTY_YEAR", EmsclcpmcEntity.CLC_S_COUNTY_YEAR));
                if (EmsclcpmcEntity.CLC_TMP_NPUSER != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_TMP_NPUSER", EmsclcpmcEntity.CLC_TMP_NPUSER));
                if (EmsclcpmcEntity.CLC_TMP_NPDATE != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_TMP_NPDATE", EmsclcpmcEntity.CLC_TMP_NPDATE));
                if (EmsclcpmcEntity.PMC_TYPE != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_TYPE", EmsclcpmcEntity.PMC_TYPE));
                if (EmsclcpmcEntity.PMC_DATE != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_DATE", EmsclcpmcEntity.PMC_DATE));
                if (EmsclcpmcEntity.PMC_AMOUNT != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_AMOUNT", EmsclcpmcEntity.PMC_AMOUNT));
                if (EmsclcpmcEntity.PMC_CHECK_DATE != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_CHECK_DATE", EmsclcpmcEntity.PMC_CHECK_DATE));
                if (EmsclcpmcEntity.PMC_CHECK_NO != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_CHECK_NO", EmsclcpmcEntity.PMC_CHECK_NO));
                if (EmsclcpmcEntity.PMC_CLOSE_LVL1_USER != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_CLOSE_LVL1_USER", EmsclcpmcEntity.PMC_CLOSE_LVL1_USER));
                if (EmsclcpmcEntity.PMC_CLOSE_LVL1_DATE != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_CLOSE_LVL1_DATE", EmsclcpmcEntity.PMC_CLOSE_LVL1_DATE));
                if (EmsclcpmcEntity.PMC_CLOSE_LVL2_USER != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_CLOSE_LVL2_USER", EmsclcpmcEntity.PMC_CLOSE_LVL2_USER));
                if (EmsclcpmcEntity.PMC_CLOSE_LVL2_DATE != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_CLOSE_LVL2_DATE", EmsclcpmcEntity.PMC_CLOSE_LVL2_DATE));
                if (EmsclcpmcEntity.PMC_INV_AMT != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_INV_AMT", EmsclcpmcEntity.PMC_INV_AMT));
                if (EmsclcpmcEntity.PMC_INV_DATE != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_INV_DATE", EmsclcpmcEntity.PMC_INV_DATE));
                if (EmsclcpmcEntity.PMC_FILE_NAME != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_FILE_NAME", EmsclcpmcEntity.PMC_FILE_NAME));
                if (EmsclcpmcEntity.CLC_DATE_LSTC != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_DATE_LSTC", EmsclcpmcEntity.CLC_DATE_LSTC));
                if (EmsclcpmcEntity.CLC_LSTC_OPERATOR != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_LSTC_OPERATOR", EmsclcpmcEntity.CLC_LSTC_OPERATOR));
                if (EmsclcpmcEntity.CLC_DATE_ADD != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_DATE_ADD", EmsclcpmcEntity.CLC_DATE_ADD));
                if (EmsclcpmcEntity.CLC_ADD_OPERATOR != string.Empty) sqlParamList.Add(new SqlParameter("@CLC_ADD_OPERATOR", EmsclcpmcEntity.CLC_ADD_OPERATOR));
                //if (EmsclcpmcEntity.PMC_PAID_TYPE != string.Empty) sqlParamList.Add(new SqlParameter("@PMC_PAID_TYPE", EmsclcpmcEntity.PMC_PAID_TYPE));
                if (!string.IsNullOrEmpty(ScreenCode.Trim())) sqlParamList.Add(new SqlParameter("@CLC_SCREEN_CODE", ScreenCode));

                if (!string.IsNullOrEmpty(NewValue.Trim())) sqlParamList.Add(new SqlParameter("@CLC_NEW_VALUE", NewValue));

                if (!string.IsNullOrEmpty(strNewLVLuser1.Trim())) sqlParamList.Add(new SqlParameter("@PMC_NCLOSE_LVL1_USER", strNewLVLuser1));
                if (!string.IsNullOrEmpty(strNewLVLDate1.Trim())) sqlParamList.Add(new SqlParameter("@PMC_NCLOSE_LVL1_DATE", strNewLVLDate1));
                if (!string.IsNullOrEmpty(strNewLVLuser2.Trim())) sqlParamList.Add(new SqlParameter("@PMC_NCLOSE_LVL2_USER", strNewLVLuser2));
                if (!string.IsNullOrEmpty(strNewLVLDate2.Trim())) sqlParamList.Add(new SqlParameter("@PMC_NCLOSE_LVL2_DATE", strNewLVLDate2));
                if (!string.IsNullOrEmpty(strUpdType.Trim())) sqlParamList.Add(new SqlParameter("@PMC_UPD_TYPE", strUpdType));


                //sqlParamList.Add(new SqlParameter("@Mode", EmsclcpmcEntity.Mode));
                SqlParameter sqlsId = new SqlParameter("@MSG", SqlDbType.VarChar, 100);
                sqlsId.Value = EmsclcpmcEntity.CLC_S_ID;
                sqlsId.Direction = ParameterDirection.Output;
                sqlParamList.Add(sqlsId);
                boolstatus = EMSBDCDB.InsertEmsclcpmcLOG(sqlParamList);
                strNewSid = sqlsId.Value.ToString();
            }

            catch (Exception ex)
            {
                boolstatus = false;
            }
            strSId = strNewSid;
            return boolstatus;
        }


        public List<EMSRESEntity> GetEmsresAllData(string agency, string dep, string program, string year, string App, string Fund, string Fundseq, string Date, string strType)
        {
            List<EMSRESEntity> Emsresdata = new List<EMSRESEntity>();
            try
            {
                DataSet emsresds = Captain.DatabaseLayer.EMSBDCDB.GetEmsResData(agency, dep, program, year, App, Fund, Fundseq, Date, strType);
                if (emsresds != null && emsresds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in emsresds.Tables[0].Rows)
                    {
                        Emsresdata.Add(new EMSRESEntity(row, strType));
                    }
                }
            }
            catch (Exception ex)
            {
                return Emsresdata;
            }

            return Emsresdata;
        }


        public EMSRESEntity GetEmsresData(string agency, string dep, string program, string year, string App, string Fund, string Fundseq, string Date, string strType)
        {
            EMSRESEntity emsResDetail = null;
            try
            {
                DataSet emsresds = Captain.DatabaseLayer.EMSBDCDB.GetEmsResData(agency, dep, program, year, App, Fund, Fundseq, Date, strType);
                if (emsresds != null && emsresds.Tables[0].Rows.Count > 0)
                {
                    emsResDetail = new EMSRESEntity(emsresds.Tables[0].Rows[0]);
                }
            }
            catch (Exception ex)
            {
                //
                return emsResDetail;
            }

            return emsResDetail;
        }


        public bool InsertUpdateDelEmsRes(EMSRESEntity EmsResEntity, out string strOutMst)
        {
            bool boolstatus = false;
            string strMsg = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                if (EmsResEntity.EMSRES_AGENCY != string.Empty) sqlParamList.Add(new SqlParameter("EMSRES_AGENCY", EmsResEntity.EMSRES_AGENCY));
                if (EmsResEntity.EMSRES_DEPT != string.Empty) sqlParamList.Add(new SqlParameter("EMSRES_DEPT", EmsResEntity.EMSRES_DEPT));
                if (EmsResEntity.EMSRES_PROGRAM != string.Empty) sqlParamList.Add(new SqlParameter("EMSRES_PROGRAM", EmsResEntity.EMSRES_PROGRAM));
                if (EmsResEntity.EMSRES_YEAR != string.Empty) sqlParamList.Add(new SqlParameter("EMSRES_YEAR", EmsResEntity.EMSRES_YEAR));
                if (EmsResEntity.EMSRES_APP != string.Empty) sqlParamList.Add(new SqlParameter("EMSRES_APP", EmsResEntity.EMSRES_APP));
                if (EmsResEntity.EMSRES_FUND != string.Empty) sqlParamList.Add(new SqlParameter("EMSRES_FUND", EmsResEntity.EMSRES_FUND));
                if (EmsResEntity.EMSRES_SEQ != string.Empty) sqlParamList.Add(new SqlParameter("EMSRES_SEQ", EmsResEntity.EMSRES_SEQ));
                if (EmsResEntity.EMSRES_DATE != string.Empty) sqlParamList.Add(new SqlParameter("EMSRES_DATE", EmsResEntity.EMSRES_DATE));
                if (EmsResEntity.EMSRES_CASEWORKER != string.Empty) sqlParamList.Add(new SqlParameter("EMSRES_CASEWORKER", EmsResEntity.EMSRES_CASEWORKER));
                if (EmsResEntity.EMSRES_AMOUNT != string.Empty) sqlParamList.Add(new SqlParameter("EMSRES_AMOUNT", EmsResEntity.EMSRES_AMOUNT));
                if (EmsResEntity.EMSRES_BALANCE != string.Empty) sqlParamList.Add(new SqlParameter("EMSRES_BALANCE", EmsResEntity.EMSRES_BALANCE)); // 07/21/2018 added murali
                if (EmsResEntity.EMSRES_NPUSER != string.Empty) sqlParamList.Add(new SqlParameter("EMSRES_NPUSER", EmsResEntity.EMSRES_NPUSER));
                if (EmsResEntity.EMSRES_NPDATE != string.Empty) sqlParamList.Add(new SqlParameter("EMSRES_NPDATE", EmsResEntity.EMSRES_NPDATE));
                if (EmsResEntity.EMSRES_TYPE != string.Empty) sqlParamList.Add(new SqlParameter("EMSRES_TYPE", EmsResEntity.EMSRES_TYPE)); // 03/29/2021 added murali
                if (EmsResEntity.EMSRES_DATE_LSTC != string.Empty) sqlParamList.Add(new SqlParameter("EMSRES_DATE_LSTC", EmsResEntity.EMSRES_DATE_LSTC));
                if (EmsResEntity.EMSRES_LSTC_OPERATOR != string.Empty) sqlParamList.Add(new SqlParameter("EMSRES_LSTC_OPERATOR", EmsResEntity.EMSRES_LSTC_OPERATOR));
                if (EmsResEntity.EMSRES_DATE_ADD != string.Empty) sqlParamList.Add(new SqlParameter("EMSRES_DATE_ADD", EmsResEntity.EMSRES_DATE_ADD));
                if (EmsResEntity.EMSRES_ADD_OPERATOR != string.Empty) sqlParamList.Add(new SqlParameter("EMSRES_ADD_OPERATOR", EmsResEntity.EMSRES_ADD_OPERATOR));
                sqlParamList.Add(new SqlParameter("@Mode", EmsResEntity.Mode));
                SqlParameter sqlsId = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
                sqlsId.Value = strMsg;
                sqlsId.Direction = ParameterDirection.Output;
                sqlParamList.Add(sqlsId);
                boolstatus = EMSBDCDB.InsertUpdateDelEmsres(sqlParamList);
                strMsg = sqlsId.Value.ToString();

            }

            catch (Exception ex)
            {
                boolstatus = false;
            }
            strOutMst = strMsg;
            return boolstatus;
        }

        public bool InsertUpdateDelEmsRes0050(EMSRESEntity EmsResEntity,string PaidAmount,string Bal,string ResDate,string Seq,string TarFund, out string strOutMst)
        {
            bool boolstatus = false;
            string strMsg = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                if (EmsResEntity.EMSRES_AGENCY != string.Empty) sqlParamList.Add(new SqlParameter("EMSRES_AGENCY", EmsResEntity.EMSRES_AGENCY));
                if (EmsResEntity.EMSRES_DEPT != string.Empty) sqlParamList.Add(new SqlParameter("EMSRES_DEPT", EmsResEntity.EMSRES_DEPT));
                if (EmsResEntity.EMSRES_PROGRAM != string.Empty) sqlParamList.Add(new SqlParameter("EMSRES_PROGRAM", EmsResEntity.EMSRES_PROGRAM));
                if (EmsResEntity.EMSRES_YEAR != string.Empty) sqlParamList.Add(new SqlParameter("EMSRES_YEAR", EmsResEntity.EMSRES_YEAR));
                if (EmsResEntity.EMSRES_APP != string.Empty) sqlParamList.Add(new SqlParameter("EMSRES_APP", EmsResEntity.EMSRES_APP));
                if (EmsResEntity.EMSRES_FUND != string.Empty) sqlParamList.Add(new SqlParameter("EMSRES_FUND", EmsResEntity.EMSRES_FUND));
                if (EmsResEntity.EMSRES_SEQ != string.Empty) sqlParamList.Add(new SqlParameter("EMSRES_SEQ", EmsResEntity.EMSRES_SEQ));
                if (EmsResEntity.EMSRES_DATE != string.Empty) sqlParamList.Add(new SqlParameter("EMSRES_DATE", EmsResEntity.EMSRES_DATE));
                if (EmsResEntity.EMSRES_CASEWORKER != string.Empty) sqlParamList.Add(new SqlParameter("EMSRES_CASEWORKER", EmsResEntity.EMSRES_CASEWORKER));
                if (EmsResEntity.EMSRES_AMOUNT != string.Empty) sqlParamList.Add(new SqlParameter("EMSRES_AMOUNT", EmsResEntity.EMSRES_AMOUNT));
                if (EmsResEntity.EMSRES_BALANCE != string.Empty) sqlParamList.Add(new SqlParameter("EMSRES_BALANCE", EmsResEntity.EMSRES_BALANCE)); // 07/21/2018 added murali
                if (EmsResEntity.EMSRES_NPUSER != string.Empty) sqlParamList.Add(new SqlParameter("EMSRES_NPUSER", EmsResEntity.EMSRES_NPUSER));
                if (EmsResEntity.EMSRES_NPDATE != string.Empty) sqlParamList.Add(new SqlParameter("EMSRES_NPDATE", EmsResEntity.EMSRES_NPDATE));
                if (EmsResEntity.EMSRES_DATE_LSTC != string.Empty) sqlParamList.Add(new SqlParameter("EMSRES_DATE_LSTC", EmsResEntity.EMSRES_DATE_LSTC));
                if (EmsResEntity.EMSRES_LSTC_OPERATOR != string.Empty) sqlParamList.Add(new SqlParameter("EMSRES_LSTC_OPERATOR", EmsResEntity.EMSRES_LSTC_OPERATOR));
                if (EmsResEntity.EMSRES_DATE_ADD != string.Empty) sqlParamList.Add(new SqlParameter("EMSRES_DATE_ADD", EmsResEntity.EMSRES_DATE_ADD));
                if (EmsResEntity.EMSRES_ADD_OPERATOR != string.Empty) sqlParamList.Add(new SqlParameter("EMSRES_ADD_OPERATOR", EmsResEntity.EMSRES_ADD_OPERATOR));
                if (PaidAmount != string.Empty) sqlParamList.Add(new SqlParameter("@PAID_AMOUNT", PaidAmount));
                if (Bal != string.Empty) sqlParamList.Add(new SqlParameter("@BALANCE", Bal));
                if (ResDate != string.Empty) sqlParamList.Add(new SqlParameter("@TARGET_RES_DATE", ResDate));
                if (TarFund != string.Empty) sqlParamList.Add(new SqlParameter("@TARGET_FUND", TarFund));

                if (Seq != string.Empty) sqlParamList.Add(new SqlParameter("@TARRES_SEQ", Seq));
                sqlParamList.Add(new SqlParameter("@Mode", EmsResEntity.Mode));
                SqlParameter sqlsId = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
                sqlsId.Value = strMsg;
                sqlsId.Direction = ParameterDirection.Output;
                sqlParamList.Add(sqlsId);
                boolstatus = EMSBDCDB.InsertUpdateDelEmsres0050(sqlParamList);
                strMsg = sqlsId.Value.ToString();

            }

            catch (Exception ex)
            {
                boolstatus = false;
            }
            strOutMst = strMsg;
            return boolstatus;
        }


        public List<EMSBCOEntity> GetEmsbudcaroverData(string agency, string dep, string program, string year, string App, string Fund, string Fundseq, string Date, string strType)
        {
            List<EMSBCOEntity> Emsresdata = new List<EMSBCOEntity>();
            try
            {
                DataSet emsresds = Captain.DatabaseLayer.EMSBDCDB.GetEMSBUDCARYOVER(agency, dep, program, year, App, Fund, Fundseq, Date, strType);
                if (emsresds != null && emsresds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in emsresds.Tables[0].Rows)
                    {
                        Emsresdata.Add(new EMSBCOEntity(row, strType));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return Emsresdata;
            }

            return Emsresdata;
        }

        public bool InsertUpdateDelEmsBUDCAROVER(EMSBCOEntity EmsResEntity, out string strOutMst)
        {
            bool boolstatus = false;
            string strMsg = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                if (EmsResEntity.AGENCY != string.Empty) sqlParamList.Add(new SqlParameter("@EMSBCO_AGENCY", EmsResEntity.AGENCY));
                if (EmsResEntity.DEPT!= string.Empty) sqlParamList.Add(new SqlParameter("@EMSBCO_DEPT", EmsResEntity.DEPT));
                if (EmsResEntity.PROGRAM!= string.Empty) sqlParamList.Add(new SqlParameter("@EMSBCO_PROGRAM", EmsResEntity.PROGRAM));
                if (EmsResEntity.YEAR!= string.Empty) sqlParamList.Add(new SqlParameter("@EMSBCO_YEAR", EmsResEntity.YEAR));
                if (EmsResEntity.APP != string.Empty) sqlParamList.Add(new SqlParameter("@EMSBCO_APP", EmsResEntity.APP));
                if (EmsResEntity.FUND!= string.Empty) sqlParamList.Add(new SqlParameter("@EMSBCO_FUND", EmsResEntity.FUND));
                if (EmsResEntity.SEQ!= string.Empty) sqlParamList.Add(new SqlParameter("@EMSBCO_SEQ", EmsResEntity.SEQ));
                if (EmsResEntity.DATE != string.Empty) sqlParamList.Add(new SqlParameter("@EMSBCO_DATE", EmsResEntity.DATE));
                if (EmsResEntity.EMSBCO_CASEWORKER != string.Empty) sqlParamList.Add(new SqlParameter("@EMSBCO_CASEWORKER", EmsResEntity.EMSBCO_CASEWORKER));
                if (EmsResEntity.EMSBCO_AMOUNT != string.Empty) sqlParamList.Add(new SqlParameter("@EMSBCO_AMOUNT", EmsResEntity.EMSBCO_AMOUNT));
                if (EmsResEntity.EMSBCO_BALANCE != string.Empty) sqlParamList.Add(new SqlParameter("@EMSBCO_BALANCE", EmsResEntity.EMSBCO_BALANCE)); 
                if (EmsResEntity.EMSBCO_NPUSER != string.Empty) sqlParamList.Add(new SqlParameter("@EMSBCO_NPUSER", EmsResEntity.EMSBCO_NPUSER));
                if (EmsResEntity.EMSBCO_NPDATE != string.Empty) sqlParamList.Add(new SqlParameter("@EMSBCO_NPDATE", EmsResEntity.EMSBCO_NPDATE));
                if (EmsResEntity.EMSBCO_PAID_AMT != string.Empty) sqlParamList.Add(new SqlParameter("@EMSBCO_PAID_AMT", EmsResEntity.EMSBCO_PAID_AMT));
                if (EmsResEntity.EMS_Start!= string.Empty) sqlParamList.Add(new SqlParameter("@EMSBCO_BDC_START", EmsResEntity.EMS_Start));
                if (EmsResEntity.EMS_End != string.Empty) sqlParamList.Add(new SqlParameter("@EMSBCO_BDC_END", EmsResEntity.EMS_End));
                if (EmsResEntity.EMSBCO_TAR_FUND != string.Empty) sqlParamList.Add(new SqlParameter("@EMSBCO_TAR_FUND", EmsResEntity.EMSBCO_TAR_FUND));
                if (EmsResEntity.EMSBCO_TAR_START != string.Empty) sqlParamList.Add(new SqlParameter("@EMSBCO_TAR_START", EmsResEntity.EMSBCO_TAR_START));
                if (EmsResEntity.EMSBCO_TAR_END!= string.Empty) sqlParamList.Add(new SqlParameter("@EMSBCO_TAR_END", EmsResEntity.EMSBCO_TAR_END));
                //if (EmsResEntity.EMSBCO_DATE_LSTC != string.Empty) sqlParamList.Add(new SqlParameter("EMSRES_DATE_LSTC", EmsResEntity.EMSBCO_DATE_LSTC));
                if (EmsResEntity.EMSBCO_LSTC_OPERATOR != string.Empty) sqlParamList.Add(new SqlParameter("@EMSBCO_LSTC_OPERATOR", EmsResEntity.EMSBCO_ADD_OPERATOR));
                //if (EmsResEntity.EMSBCO_DATE_ADD != string.Empty) sqlParamList.Add(new SqlParameter("EMSRES_DATE_ADD", EmsResEntity.EMSBCO_DATE_ADD));
                if (EmsResEntity.EMSBCO_ADD_OPERATOR != string.Empty) sqlParamList.Add(new SqlParameter("@EMSBCO_ADD_OPERATOR", EmsResEntity.EMSBCO_ADD_OPERATOR));
                sqlParamList.Add(new SqlParameter("@Mode", EmsResEntity.Mode));
                SqlParameter sqlsId = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
                sqlsId.Value = strMsg;
                sqlsId.Direction = ParameterDirection.Output;
                sqlParamList.Add(sqlsId);
                boolstatus = EMSBDCDB.InsertUpdateDelEmsBUDCAROVER(sqlParamList);
                strMsg = sqlsId.Value.ToString();

            }

            catch (Exception ex)
            {
                boolstatus = false;
            }
            strOutMst = strMsg;
            return boolstatus;
        }

        public List<EMSSPEntity> GetEmsspAllData(string Fund, string Type, string Code)
        {
            List<EMSSPEntity> Emsspdata = new List<EMSSPEntity>();
            try
            {
                DataSet emsclcpmcds = Captain.DatabaseLayer.EMSBDCDB.GetEmsspData(Fund, Type, Code);
                if (emsclcpmcds != null && emsclcpmcds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in emsclcpmcds.Tables[0].Rows)
                    {
                        Emsspdata.Add(new EMSSPEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return Emsspdata;
            }

            return Emsspdata;
        }


        public bool InsertUpdateDelEmssp(EMSSPEntity EmsspEntity)
        {
            bool boolstatus = false;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                if (EmsspEntity.EMSSP_FUND != string.Empty) sqlParamList.Add(new SqlParameter("@EMSSP_FUND", EmsspEntity.EMSSP_FUND));
                if (EmsspEntity.EMSSP_TYPE != string.Empty) sqlParamList.Add(new SqlParameter("@EMSSP_TYPE", EmsspEntity.EMSSP_TYPE));
                if (EmsspEntity.EMSSP_Xml != string.Empty) sqlParamList.Add(new SqlParameter("@EMSSP_Xml", EmsspEntity.EMSSP_Xml));

                //if (EmsspEntity.CLC_YEAR != string.Empty) sqlParamList.Add(new SqlParameter("CLC_YEAR", EmsspEntity.CLC_YEAR));
                //if (EmsspEntity.CLC_APP != string.Empty) sqlParamList.Add(new SqlParameter("CLC_APP", EmsspEntity.CLC_APP));
                //if (EmsspEntity.CLC_FUND != string.Empty) sqlParamList.Add(new SqlParameter("CLC_FUND", EmsspEntity.CLC_FUND));
                //if (EmsspEntity.CLC_FUND_SEQ != string.Empty) sqlParamList.Add(new SqlParameter("CLC_FUND_SEQ", EmsspEntity.CLC_FUND_SEQ));
                //if (EmsspEntity.CLC_SERVICE_SEQ != string.Empty) sqlParamList.Add(new SqlParameter("CLC_SERVICE_SEQ", EmsspEntity.CLC_SERVICE_SEQ));
                //if (EmsspEntity.CLC_S_RES_DATE != string.Empty) sqlParamList.Add(new SqlParameter("CLC_S_RES_DATE", EmsspEntity.CLC_S_RES_DATE));
                sqlParamList.Add(new SqlParameter("@Mode", EmsspEntity.Mode));
                boolstatus = EMSBDCDB.InsertUpdateDelEmssp(sqlParamList);
            }

            catch (Exception ex)
            {
                return false;
            }

            return boolstatus;
        }

        public EMSCLAPMAEntity GetEmsclapmaData(string agency, string dep, string program, string year, string App, string Fund, string Fundseq, string ServiceSeq, string strSeq, string strresdate)
        {
            EMSCLAPMAEntity emsclapmaDetail = null;
            try
            {
                DataSet emsclapmads = Captain.DatabaseLayer.EMSBDCDB.GetEmsclapmaData(agency, dep, program, year, App, Fund, Fundseq, ServiceSeq, strSeq, strresdate);
                if (emsclapmads != null && emsclapmads.Tables[0].Rows.Count > 0)
                {
                    emsclapmaDetail = new EMSCLAPMAEntity(emsclapmads.Tables[0].Rows[0]);
                }
            }
            catch (Exception ex)
            {
                //
                return emsclapmaDetail;
            }

            return emsclapmaDetail;
        }

        public List<EMSCLAPMAEntity> GetEmsclapmaAllData(string agency, string dep, string program, string year, string App, string Fund, string Fundseq, string ServiceSeq, string strSeq, string strresdate)
        {
            List<EMSCLAPMAEntity> Emsclcapmadata = new List<EMSCLAPMAEntity>();
            try
            {
                DataSet emsclcapmads = Captain.DatabaseLayer.EMSBDCDB.GetEmsclapmaData(agency, dep, program, year, App, Fund, Fundseq, ServiceSeq, strSeq, strresdate);
                if (emsclcapmads != null && emsclcapmads.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in emsclcapmads.Tables[0].Rows)
                    {
                        Emsclcapmadata.Add(new EMSCLAPMAEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return Emsclcapmadata;
            }

            return Emsclcapmadata;
        }


        public bool InsertUpdateDelEmsclapma(EMSCLAPMAEntity EmsclapmaEntity)
        {
            bool boolstatus = false;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                if (EmsclapmaEntity.CLA_AGENCY != string.Empty) sqlParamList.Add(new SqlParameter("@CLA_AGENCY", EmsclapmaEntity.CLA_AGENCY));
                if (EmsclapmaEntity.CLA_DEPT != string.Empty) sqlParamList.Add(new SqlParameter("@CLA_DEPT", EmsclapmaEntity.CLA_DEPT));
                if (EmsclapmaEntity.CLA_PROGRAM != string.Empty) sqlParamList.Add(new SqlParameter("@CLA_PROGRAM", EmsclapmaEntity.CLA_PROGRAM));
                if (EmsclapmaEntity.CLA_YEAR != string.Empty) sqlParamList.Add(new SqlParameter("@CLA_YEAR", EmsclapmaEntity.CLA_YEAR));
                if (EmsclapmaEntity.CLA_APP != string.Empty) sqlParamList.Add(new SqlParameter("@CLA_APP", EmsclapmaEntity.CLA_APP));
                if (EmsclapmaEntity.CLA_RES_FUND != string.Empty) sqlParamList.Add(new SqlParameter("@CLA_RES_FUND", EmsclapmaEntity.CLA_RES_FUND));
                if (EmsclapmaEntity.CLA_RES_SEQ != string.Empty) sqlParamList.Add(new SqlParameter("@CLA_RES_SEQ", EmsclapmaEntity.CLA_RES_SEQ));
                if (EmsclapmaEntity.CLA_RES_DATE != string.Empty) sqlParamList.Add(new SqlParameter("@CLA_RES_DATE", EmsclapmaEntity.CLA_RES_DATE));
                if (EmsclapmaEntity.CLA_CLC_SEQ != string.Empty) sqlParamList.Add(new SqlParameter("@CLA_CLC_SEQ", EmsclapmaEntity.CLA_CLC_SEQ));
                if (EmsclapmaEntity.CLA_SEQ != string.Empty) sqlParamList.Add(new SqlParameter("@CLA_SEQ", EmsclapmaEntity.CLA_SEQ));
                if (EmsclapmaEntity.CLA_REASON != string.Empty) sqlParamList.Add(new SqlParameter("@CLA_REASON", EmsclapmaEntity.CLA_REASON));
                if (EmsclapmaEntity.CLA_ADJ_DATE != string.Empty) sqlParamList.Add(new SqlParameter("@CLA_ADJ_DATE", EmsclapmaEntity.CLA_ADJ_DATE));
                if (EmsclapmaEntity.CLA_O_CASEWORKER != string.Empty) sqlParamList.Add(new SqlParameter("@CLA_O_CASEWORKER", EmsclapmaEntity.CLA_O_CASEWORKER));
                if (EmsclapmaEntity.CLA_O_SERVICE_CODE != string.Empty) sqlParamList.Add(new SqlParameter("@CLA_O_SERVICE_CODE", EmsclapmaEntity.CLA_O_SERVICE_CODE));
                if (EmsclapmaEntity.CLA_O_VENDOR != string.Empty) sqlParamList.Add(new SqlParameter("@CLA_O_VENDOR", EmsclapmaEntity.CLA_O_VENDOR));
                if (EmsclapmaEntity.CLA_O_ACCT != string.Empty) sqlParamList.Add(new SqlParameter("@CLA_O_ACCT", EmsclapmaEntity.CLA_O_ACCT));
                if (EmsclapmaEntity.CLA_O_BIL_LNAME != string.Empty) sqlParamList.Add(new SqlParameter("@CLA_O_BIL_LNAME", EmsclapmaEntity.CLA_O_BIL_LNAME));
                if (EmsclapmaEntity.CLA_O_BIL_FNAME != string.Empty) sqlParamList.Add(new SqlParameter("@CLA_O_BIL_FNAME", EmsclapmaEntity.CLA_O_BIL_FNAME));
                if (EmsclapmaEntity.CLA_O_DECISION != string.Empty) sqlParamList.Add(new SqlParameter("@CLA_O_DECISION", EmsclapmaEntity.CLA_O_DECISION));
                if (EmsclapmaEntity.CLA_O_DECISIONDATE != string.Empty) sqlParamList.Add(new SqlParameter("@CLA_O_DECISIONDATE", EmsclapmaEntity.CLA_O_DECISIONDATE));
                if (EmsclapmaEntity.CLA_O_APPEAL != string.Empty) sqlParamList.Add(new SqlParameter("@CLA_O_APPEAL", EmsclapmaEntity.CLA_O_APPEAL));
                if (EmsclapmaEntity.CLA_O_VOUCHER != string.Empty) sqlParamList.Add(new SqlParameter("@CLA_O_VOUCHER", EmsclapmaEntity.CLA_O_VOUCHER));
                if (EmsclapmaEntity.CLA_O_FOLL_DATE != string.Empty) sqlParamList.Add(new SqlParameter("@CLA_O_FOLL_DATE", EmsclapmaEntity.CLA_O_FOLL_DATE));
                if (EmsclapmaEntity.CLA_O_FOLL_CDATE != string.Empty) sqlParamList.Add(new SqlParameter("@CLA_O_FOLL_CDATE", EmsclapmaEntity.CLA_O_FOLL_CDATE));
                if (EmsclapmaEntity.CLA_O_BENPERD_START != string.Empty) sqlParamList.Add(new SqlParameter("@CLA_O_BENPERD_START", EmsclapmaEntity.CLA_O_BENPERD_START));
                if (EmsclapmaEntity.CLA_O_BENPERD_END != string.Empty) sqlParamList.Add(new SqlParameter("@CLA_O_BENPERD_END", EmsclapmaEntity.CLA_O_BENPERD_END));
                if (EmsclapmaEntity.CLA_N_CASEWORKER != string.Empty) sqlParamList.Add(new SqlParameter("@CLA_N_CASEWORKER", EmsclapmaEntity.CLA_N_CASEWORKER));
                if (EmsclapmaEntity.CLA_N_SERVICE_CODE != string.Empty) sqlParamList.Add(new SqlParameter("@CLA_N_SERVICE_CODE", EmsclapmaEntity.CLA_N_SERVICE_CODE));
                if (EmsclapmaEntity.CLA_N_VENDOR != string.Empty) sqlParamList.Add(new SqlParameter("@CLA_N_VENDOR", EmsclapmaEntity.CLA_N_VENDOR));
                if (EmsclapmaEntity.CLA_N_ACCT != string.Empty) sqlParamList.Add(new SqlParameter("@CLA_N_ACCT", EmsclapmaEntity.CLA_N_ACCT));
                if (EmsclapmaEntity.CLA_N_BIL_LNAME != string.Empty) sqlParamList.Add(new SqlParameter("@CLA_N_BIL_LNAME", EmsclapmaEntity.CLA_N_BIL_LNAME));
                if (EmsclapmaEntity.CLA_N_BIL_FNAME != string.Empty) sqlParamList.Add(new SqlParameter("@CLA_N_BIL_FNAME", EmsclapmaEntity.CLA_N_BIL_FNAME));
                if (EmsclapmaEntity.CLA_N_DECISION != string.Empty) sqlParamList.Add(new SqlParameter("@CLA_N_DECISION", EmsclapmaEntity.CLA_N_DECISION));
                if (EmsclapmaEntity.CLA_N_DECISIONDATE != string.Empty) sqlParamList.Add(new SqlParameter("@CLA_N_DECISIONDATE", EmsclapmaEntity.CLA_N_DECISIONDATE));
                if (EmsclapmaEntity.CLA_N_APPEAL != string.Empty) sqlParamList.Add(new SqlParameter("@CLA_N_APPEAL", EmsclapmaEntity.CLA_N_APPEAL));
                if (EmsclapmaEntity.CLA_N_VOUCHER != string.Empty) sqlParamList.Add(new SqlParameter("@CLA_N_VOUCHER", EmsclapmaEntity.CLA_N_VOUCHER));
                if (EmsclapmaEntity.CLA_N_FOLL_DATE != string.Empty) sqlParamList.Add(new SqlParameter("@CLA_N_FOLL_DATE", EmsclapmaEntity.CLA_N_FOLL_DATE));
                if (EmsclapmaEntity.CLA_N_FOLL_CDATE != string.Empty) sqlParamList.Add(new SqlParameter("@CLA_N_FOLL_CDATE", EmsclapmaEntity.CLA_N_FOLL_CDATE));
                if (EmsclapmaEntity.CLA_N_BENPERD_START != string.Empty) sqlParamList.Add(new SqlParameter("@CLA_N_BENPERD_START", EmsclapmaEntity.CLA_N_BENPERD_START));
                if (EmsclapmaEntity.CLA_N_BENPERD_END != string.Empty) sqlParamList.Add(new SqlParameter("@CLA_N_BENPERD_END", EmsclapmaEntity.CLA_N_BENPERD_END));
                if (EmsclapmaEntity.PMA_O_TYPE != string.Empty) sqlParamList.Add(new SqlParameter("@PMA_O_TYPE", EmsclapmaEntity.PMA_O_TYPE));
                if (EmsclapmaEntity.PMA_O_AMOUNT != string.Empty) sqlParamList.Add(new SqlParameter("@PMA_O_AMOUNT", EmsclapmaEntity.PMA_O_AMOUNT));
                if (EmsclapmaEntity.PMA_O_AUTH_FOOD_VOUCHER != string.Empty) sqlParamList.Add(new SqlParameter("@PMA_O_AUTH_FOOD_VOUCHER", EmsclapmaEntity.PMA_O_AUTH_FOOD_VOUCHER));
                if (EmsclapmaEntity.PMA_O_AUTH_LIQUIDATE != string.Empty) sqlParamList.Add(new SqlParameter("@PMA_O_AUTH_LIQUIDATE", EmsclapmaEntity.PMA_O_AUTH_LIQUIDATE));
                if (EmsclapmaEntity.PMA_O_AUTH_AMOUNT != string.Empty) sqlParamList.Add(new SqlParameter("@PMA_O_AUTH_AMOUNT", EmsclapmaEntity.PMA_O_AUTH_AMOUNT));
                if (EmsclapmaEntity.PMA_O_AUTH_WORKER != string.Empty) sqlParamList.Add(new SqlParameter("@PMA_O_AUTH_WORKER", EmsclapmaEntity.PMA_O_AUTH_WORKER));
                if (EmsclapmaEntity.PMA_O_AUTH_DATE != string.Empty) sqlParamList.Add(new SqlParameter("@PMA_O_AUTH_DATE", EmsclapmaEntity.PMA_O_AUTH_DATE));
                if (EmsclapmaEntity.PMA_O_INV_AMOUNT != string.Empty) sqlParamList.Add(new SqlParameter("@PMA_O_INV_AMOUNT", EmsclapmaEntity.PMA_O_INV_AMOUNT));
                if (EmsclapmaEntity.PMA_O_INV_BILL_AMOUNT != string.Empty) sqlParamList.Add(new SqlParameter("@PMA_O_INV_BILL_AMOUNT", EmsclapmaEntity.PMA_O_INV_BILL_AMOUNT));
                if (EmsclapmaEntity.PMA_O_INV_BILL_DATE != string.Empty) sqlParamList.Add(new SqlParameter("@PMA_O_INV_BILL_DATE", EmsclapmaEntity.PMA_O_INV_BILL_DATE));
                if (EmsclapmaEntity.PMA_O_INV_VENDOR_RATING != string.Empty) sqlParamList.Add(new SqlParameter("@PMA_O_INV_VENDOR_RATING", EmsclapmaEntity.PMA_O_INV_VENDOR_RATING));
                if (EmsclapmaEntity.PMA_O_INV_WORKER != string.Empty) sqlParamList.Add(new SqlParameter("@PMA_O_INV_WORKER", EmsclapmaEntity.PMA_O_INV_WORKER));
                if (EmsclapmaEntity.PMA_O_INV_DATE != string.Empty) sqlParamList.Add(new SqlParameter("@PMA_O_INV_DATE", EmsclapmaEntity.PMA_O_INV_DATE));
                if (EmsclapmaEntity.PMA_O_CHECK_DATE != string.Empty) sqlParamList.Add(new SqlParameter("@PMA_O_CHECK_DATE", EmsclapmaEntity.PMA_O_CHECK_DATE));
                if (EmsclapmaEntity.PMA_O_CHECK_NO != string.Empty) sqlParamList.Add(new SqlParameter("@PMA_O_CHECK_NO", EmsclapmaEntity.PMA_O_CHECK_NO));
                if (EmsclapmaEntity.PMA_O_FILE_NAME != string.Empty) sqlParamList.Add(new SqlParameter("@PMA_O_FILE_NAME", EmsclapmaEntity.PMA_O_FILE_NAME));
                if (EmsclapmaEntity.PMA_N_TYPE != string.Empty) sqlParamList.Add(new SqlParameter("@PMA_N_TYPE", EmsclapmaEntity.PMA_N_TYPE));
                if (EmsclapmaEntity.PMA_N_AMOUNT != string.Empty) sqlParamList.Add(new SqlParameter("@PMA_N_AMOUNT", EmsclapmaEntity.PMA_N_AMOUNT));
                if (EmsclapmaEntity.PMA_N_AUTH_FOOD_VOUCHER != string.Empty) sqlParamList.Add(new SqlParameter("@PMA_N_AUTH_FOOD_VOUCHER", EmsclapmaEntity.PMA_N_AUTH_FOOD_VOUCHER));
                if (EmsclapmaEntity.PMA_N_AUTH_LIQUIDATE != string.Empty) sqlParamList.Add(new SqlParameter("@PMA_N_AUTH_LIQUIDATE", EmsclapmaEntity.PMA_N_AUTH_LIQUIDATE));
                if (EmsclapmaEntity.PMA_N_AUTH_AMOUNT != string.Empty) sqlParamList.Add(new SqlParameter("@PMA_N_AUTH_AMOUNT", EmsclapmaEntity.PMA_N_AUTH_AMOUNT));
                if (EmsclapmaEntity.PMA_N_AUTH_WORKER != string.Empty) sqlParamList.Add(new SqlParameter("@PMA_N_AUTH_WORKER", EmsclapmaEntity.PMA_N_AUTH_WORKER));
                if (EmsclapmaEntity.PMA_N_AUTH_DATE != string.Empty) sqlParamList.Add(new SqlParameter("@PMA_N_AUTH_DATE", EmsclapmaEntity.PMA_N_AUTH_DATE));
                if (EmsclapmaEntity.PMA_N_INV_AMOUNT != string.Empty) sqlParamList.Add(new SqlParameter("@PMA_N_INV_AMOUNT", EmsclapmaEntity.PMA_N_INV_AMOUNT));
                if (EmsclapmaEntity.PMA_N_INV_BILL_AMOUNT != string.Empty) sqlParamList.Add(new SqlParameter("@PMA_N_INV_BILL_AMOUNT", EmsclapmaEntity.PMA_N_INV_BILL_AMOUNT));
                if (EmsclapmaEntity.PMA_N_INV_BILL_DATE != string.Empty) sqlParamList.Add(new SqlParameter("@PMA_N_INV_BILL_DATE", EmsclapmaEntity.PMA_N_INV_BILL_DATE));
                if (EmsclapmaEntity.PMA_N_INV_VENDOR_RATING != string.Empty) sqlParamList.Add(new SqlParameter("@PMA_N_INV_VENDOR_RATING", EmsclapmaEntity.PMA_N_INV_VENDOR_RATING));
                if (EmsclapmaEntity.PMA_N_INV_WORKER != string.Empty) sqlParamList.Add(new SqlParameter("@PMA_N_INV_WORKER", EmsclapmaEntity.PMA_N_INV_WORKER));
                if (EmsclapmaEntity.PMA_N_INV_DATE != string.Empty) sqlParamList.Add(new SqlParameter("@PMA_N_INV_DATE", EmsclapmaEntity.PMA_N_INV_DATE));
                if (EmsclapmaEntity.PMA_N_CHECK_DATE != string.Empty) sqlParamList.Add(new SqlParameter("@PMA_N_CHECK_DATE", EmsclapmaEntity.PMA_N_CHECK_DATE));
                if (EmsclapmaEntity.PMA_N_CHECK_NO != string.Empty) sqlParamList.Add(new SqlParameter("@PMA_N_CHECK_NO", EmsclapmaEntity.PMA_N_CHECK_NO));
                if (EmsclapmaEntity.PMA_N_FILE_NAME != string.Empty) sqlParamList.Add(new SqlParameter("@PMA_N_FILE_NAME", EmsclapmaEntity.PMA_N_FILE_NAME));
                if (EmsclapmaEntity.RES_O_CASEWORKER != string.Empty) sqlParamList.Add(new SqlParameter("@RES_O_CASEWORKER", EmsclapmaEntity.RES_O_CASEWORKER));
                if (EmsclapmaEntity.RES_O_AMOUNT != string.Empty) sqlParamList.Add(new SqlParameter("@RES_O_AMOUNT", EmsclapmaEntity.RES_O_AMOUNT));
                if (EmsclapmaEntity.RES_N_CASEWORKER != string.Empty) sqlParamList.Add(new SqlParameter("@RES_N_CASEWORKER", EmsclapmaEntity.RES_N_CASEWORKER));
                if (EmsclapmaEntity.RES_N_AMOUNT != string.Empty) sqlParamList.Add(new SqlParameter("@RES_N_AMOUNT", EmsclapmaEntity.RES_N_AMOUNT));
                // if (EmsclapmaEntity.CLA_DATE_ADD != string.Empty) sqlParamList.Add(new SqlParameter("CLA_DATE_ADD", EmsclapmaEntity.CLA_DATE_ADD));
                if (EmsclapmaEntity.CLA_ADD_OPERATOR != string.Empty) sqlParamList.Add(new SqlParameter("@CLA_ADD_OPERATOR", EmsclapmaEntity.CLA_ADD_OPERATOR));
                sqlParamList.Add(new SqlParameter("@Mode", EmsclapmaEntity.Mode));
                boolstatus = EMSBDCDB.InsertUpdateDelEmsclapma(sqlParamList);
            }

            catch (Exception ex)
            {
                return false;
            }

            return boolstatus;
        }


        public List<EMSOBOEntity> GetEmsOboData(string id, string seq)
        {
            List<EMSOBOEntity> Emsobodata = new List<EMSOBOEntity>();
            try
            {
                DataSet emsobods = Captain.DatabaseLayer.EMSBDCDB.GetEmsOboData(id, seq);
                if (emsobods != null && emsobods.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in emsobods.Tables[0].Rows)
                    {
                        Emsobodata.Add(new EMSOBOEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return Emsobodata;
            }

            return Emsobodata;
        }


        public bool InsertUpdateDelEmsobo(EMSOBOEntity EmsoboEntity)
        {
            bool boolstatus = false;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                if (EmsoboEntity.EMSOBO_ID != string.Empty) sqlParamList.Add(new SqlParameter("@EMSOBO_ID", EmsoboEntity.EMSOBO_ID));
                if (EmsoboEntity.EMSOBO_SEQ != string.Empty) sqlParamList.Add(new SqlParameter("@EMSOBO_SEQ", EmsoboEntity.EMSOBO_SEQ));
                if (EmsoboEntity.EMSOBO_CLIENT_ID != string.Empty) sqlParamList.Add(new SqlParameter("@EMSOBO_CLIENT_ID", EmsoboEntity.EMSOBO_CLIENT_ID));
                if (EmsoboEntity.EMSOBO_FAM_SEQ != string.Empty) sqlParamList.Add(new SqlParameter("@EMSOBO_FAM_SEQ", EmsoboEntity.EMSOBO_FAM_SEQ));
                sqlParamList.Add(new SqlParameter("@Mode", EmsoboEntity.Mode));
                boolstatus = EMSBDCDB.InsertUpdateDelEmsobo(sqlParamList);
            }

            catch (Exception ex)
            {
                return false;
            }

            return boolstatus;
        }

        public List<EMSB0003Entity> GetEMSB_Reports(string agency, string dep, string program, string year, string Site, string caseworker, string Fund, string FormType)
        {
            List<EMSB0003Entity> Emsresdata = new List<EMSB0003Entity>();
            try
            {
                DataSet emsresds = Captain.DatabaseLayer.EMSBDCDB.GetEMSBReports(agency, dep, program, year, Site, caseworker, Fund, FormType);

                if (emsresds != null && emsresds.Tables[0].Rows.Count > 0)
                {
                    if (FormType == "EMSB0018")
                    {
                        foreach (DataRow row in emsresds.Tables[0].Rows)
                        {
                            Emsresdata.Add(new EMSB0003Entity(row, string.Empty));
                        }
                    }
                    else
                    {
                        foreach (DataRow row in emsresds.Tables[0].Rows)
                        {
                            Emsresdata.Add(new EMSB0003Entity(row));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Emsresdata;
            }

            return Emsresdata;
        }

        public List<CaseSnpEntity> GetEMSB0008_presets(string Agency, string Dept, string Program, string Year, string Site, string Worker, string strTable)
        {
            List<CaseSnpEntity> CaseSnpProfile = new List<CaseSnpEntity>();
            try
            {
                DataSet CaseSnpData = EMSBDCDB.GetEMSB0008_Presets(Agency, Dept, Program, Year, Site, Worker);
                if (CaseSnpData != null && CaseSnpData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseSnpData.Tables[0].Rows)
                    {
                        CaseSnpProfile.Add(new CaseSnpEntity(row, strTable));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return CaseSnpProfile;
            }

            return CaseSnpProfile;
        }

        public List<EMSB0012Entity> GetEMSB0012(string agency, string dep, string program, string year, string Site, string caseworker, string Fund)
        {
            List<EMSB0012Entity> Emsresdata = new List<EMSB0012Entity>();
            try
            {
                DataSet emsresds = Captain.DatabaseLayer.EMSBDCDB.GetEMSB0012(agency, dep, program, year, Site, caseworker, Fund);
                if (emsresds != null && emsresds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in emsresds.Tables[0].Rows)
                    {
                        Emsresdata.Add(new EMSB0012Entity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return Emsresdata;
            }

            return Emsresdata;
        }

        //public List<EMSB0011Entity> GetEMSB0011_PaidInvoices(string agency, string dep, string program, string year, string CaseType, string Site, string caseworker, string Fund)
        //{
        //    List<EMSB0011Entity> Emsresdata = new List<EMSB0011Entity>();
        //    try
        //    {
        //        DataSet emsresds = Captain.DatabaseLayer.EMSBDCDB.GetEMSB0011(agency, dep, program, year,CaseType, Site, caseworker, Fund);
        //        if (emsresds != null && emsresds.Tables[0].Rows.Count > 0)
        //        {
        //            foreach (DataRow row in emsresds.Tables[0].Rows)
        //            {
        //                Emsresdata.Add(new EMSB0011Entity(row));
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Emsresdata;
        //    }

        //    return Emsresdata;
        //}

        //public List<EMSB0011Entity> GetEMSB0011_PaidInvoices_Summary(string agency, string dep, string program, string year, string CaseType, string Site, string caseworker, string Fund)
        //{
        //    List<EMSB0011Entity> Emsresdata = new List<EMSB0011Entity>();
        //    try
        //    {
        //        DataSet emsresds = Captain.DatabaseLayer.EMSBDCDB.GetEMSB0011(agency, dep, program, year, CaseType, Site, caseworker, Fund);
        //        if (emsresds != null && emsresds.Tables[1].Rows.Count > 0)
        //        {
        //            foreach (DataRow row in emsresds.Tables[1].Rows)
        //            {
        //                Emsresdata.Add(new EMSB0011Entity(row,string.Empty));
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Emsresdata;
        //    }

        //    return Emsresdata;
        //}

        public List<EMSB0024Entity> GetEMSB0024_Report(string agency, string dep, string program, string year, string Site, string AdjCd, string Caseworker, string Fund, string UserId, string Service, string CostCentre, string Acc, string CountyYear, string Vendor, string Acc_type)
        {
            List<EMSB0024Entity> Emsresdata = new List<EMSB0024Entity>();
            try
            {
                DataSet emsresds = Captain.DatabaseLayer.EMSBDCDB.GetEMSB0024(agency, dep, program, year, Site, AdjCd, Caseworker, Fund, UserId, Service, CostCentre, Acc, CountyYear, Vendor, Acc_type);
                if (emsresds != null && emsresds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in emsresds.Tables[0].Rows)
                    {
                        Emsresdata.Add(new EMSB0024Entity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return Emsresdata;
            }

            return Emsresdata;
        }


        public List<EMSB0026Entity> GetEMSB0026_Report(string agency, string dep, string program, string year, string Fund, string Site, string Caseworker, string Worker_sw, string SweepInterval, string Sweepdays)
        {
            List<EMSB0026Entity> Emsresdata = new List<EMSB0026Entity>();
            try
            {
                DataSet emsresds = Captain.DatabaseLayer.EMSBDCDB.GetEMSB0026(agency, dep, program, year, Fund, Site, Caseworker, Worker_sw, SweepInterval, Sweepdays);
                if (emsresds != null && emsresds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in emsresds.Tables[0].Rows)
                    {
                        Emsresdata.Add(new EMSB0026Entity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return Emsresdata;
            }

            return Emsresdata;
        }

        public List<EMSB0026Entity> GetEMS00050_Data(string agency, string dep, string program, string year, string Fund, string startdate, string Enddate)
        {
            List<EMSB0026Entity> Emsresdata = new List<EMSB0026Entity>();
            try
            {
                DataSet emsresds = Captain.DatabaseLayer.EMSBDCDB.GetEMS00050(agency, dep, program, year, Fund, startdate,Enddate);
                if (emsresds != null && emsresds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in emsresds.Tables[0].Rows)
                    {
                        Emsresdata.Add(new EMSB0026Entity(row,string.Empty,string.Empty));
                    }
                }
            }
            catch (Exception ex)
            {
                return Emsresdata;
            }

            return Emsresdata;
        }


        public List<EMSCLCPMCEntity> GetEMSB0025(string agency, string dep, string program, string year)
        {
            List<EMSCLCPMCEntity> Emsresdata = new List<EMSCLCPMCEntity>();
            try
            {
                DataSet emsresds = Captain.DatabaseLayer.EMSBDCDB.GetEMSB0025(agency, dep, program, year);
                if (emsresds != null && emsresds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in emsresds.Tables[0].Rows)
                    {
                        Emsresdata.Add(new EMSCLCPMCEntity(row, "EMSB0025", string.Empty));
                    }
                }
            }
            catch (Exception ex)
            {
                return Emsresdata;
            }

            return Emsresdata;
        }

        public List<EMSCLCPMCEntity> GetEMSB0023(string agency, string dep, string program, string year,string Applicants)
        {
            List<EMSCLCPMCEntity> Emsresdata = new List<EMSCLCPMCEntity>();
            try
            {
                DataSet emsresds = Captain.DatabaseLayer.EMSBDCDB.GetEMSB0023(agency, dep, program, year, Applicants);
                if (emsresds != null && emsresds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in emsresds.Tables[0].Rows)
                    {
                        Emsresdata.Add(new EMSCLCPMCEntity(row, "EMSB0023", string.Empty));
                    }
                }
            }
            catch (Exception ex)
            {
                return Emsresdata;
            }

            return Emsresdata;
        }

        public bool UpdateCASEVOT(CASEVOTEntity Entity)
        {
            bool boolsuccess = true;
            //Sql_Reslut_Msg = "Success";
            //Msg = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList = Prepare_CASEVOT_SqlParameters_List(Entity);

                //SqlParameter DeleteMsg = new SqlParameter("@msg", SqlDbType.VarChar, 50);
                //DeleteMsg.Direction = ParameterDirection.Output;
                //sqlParamList.Add(DeleteMsg);

                //boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.UPDATE_CASEVOT", out Sql_Reslut_Msg);  //

                boolsuccess = EMSBDCDB.UpdateCASEVOT(sqlParamList);

                //Msg = DeleteMsg.Value.ToString();
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        public List<SqlParameter> Prepare_CASEVOT_SqlParameters_List(CASEVOTEntity Entity)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {

                if (!string.IsNullOrEmpty(Entity.City))
                    sqlParamList.Add(new SqlParameter("@CASEVOT_CITY", Entity.City));

                if (!string.IsNullOrEmpty(Entity.Street))
                    sqlParamList.Add(new SqlParameter("@CASEVOT_STREET", Entity.Street));
                if (!string.IsNullOrEmpty(Entity.Suffix))
                    sqlParamList.Add(new SqlParameter("@CASEVOT_SUFFIX", Entity.Suffix));
                if (!string.IsNullOrEmpty(Entity.Block))
                    sqlParamList.Add(new SqlParameter("@CASEVOT_BLOCK", Entity.Block));
                if (!string.IsNullOrEmpty(Entity.Precinct))
                    sqlParamList.Add(new SqlParameter("@CASEVOT_PRECINCT", Entity.Precinct));

                if (!string.IsNullOrEmpty(Entity.Direction))
                    sqlParamList.Add(new SqlParameter("@CASEVOT_DIRECTION", Entity.Direction));
                sqlParamList.Add(new SqlParameter("@CASEVOT_ZIP", Entity.Zip));
                if (!string.IsNullOrEmpty(Entity.EO))
                    sqlParamList.Add(new SqlParameter("@CASEVOT_EO", Entity.EO));


            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }

        public List<CASEVOTEntity> GETCASEVOT(string strCity, string strStreet, string strSuffix)
        {
            List<CASEVOTEntity> CaseVotdata = new List<CASEVOTEntity>();
            try
            {
                DataSet casevotds = Captain.DatabaseLayer.EMSBDCDB.GETCASEVOT(strCity, strStreet, strSuffix);
                if (casevotds != null && casevotds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in casevotds.Tables[0].Rows)
                    {
                        CaseVotdata.Add(new CASEVOTEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return CaseVotdata;
            }

            return CaseVotdata;
        }

        //public List<EMSB0028Entity> GetEMSB0028Report(string agency, string dep, string program, string year, string Fund, string Site, string Worker, string Fromdate, string Todate, string BudRefdate)
        //{
        //    List<EMSB0028Entity> Emsresdata = new List<EMSB0028Entity>();
        //    try
        //    {
        //        DataSet emsresds = Captain.DatabaseLayer.EMSBDCDB.GetEMSB0028(agency, dep, program, year,Fund, Site, Worker, Fromdate,Todate,BudRefdate);
        //        if (emsresds != null && emsresds.Tables[0].Rows.Count > 0)
        //        {
        //            foreach (DataRow row in emsresds.Tables[0].Rows)
        //            {
        //                Emsresdata.Add(new EMSB0028Entity(row));
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Emsresdata;
        //    }

        //    return Emsresdata;
        //}



        public bool InsertEMSLOCKDATA(string agency, string dep, string program, string year, string App, string Fund, string Fundseq, string ServiceSeq, string strDate,  string strBdccostcenter, string strbdcGlAccount, string strBudgetYear, string strIntOrder, string strAccountType, string strBdcstart, string strBdcEnd, string strType, string strLockType,string strLockBy,string strLockScreen)
        {
            string strOutMsg;
            bool boolstatus = true;
            try
            {
               
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                if (agency != string.Empty)
                {
                    SqlParameter empnoParm = new SqlParameter("@CLC_AGENCY", agency);
                    sqlParamList.Add(empnoParm);
                }
                if (dep != string.Empty)
                {
                    SqlParameter empnoParm = new SqlParameter("@CLC_DEPT", dep);
                    sqlParamList.Add(empnoParm);
                }
                if (program != string.Empty)
                {
                    SqlParameter empnoParm = new SqlParameter("@CLC_PROGRAM", program);
                    sqlParamList.Add(empnoParm);
                }

                SqlParameter clcyear = new SqlParameter("@CLC_YEAR ", year);
                sqlParamList.Add(clcyear);

                if (App != string.Empty)
                {
                    SqlParameter empnoParm = new SqlParameter("@CLC_APP ", App);
                    sqlParamList.Add(empnoParm);
                }
                if (Fund != string.Empty)
                {
                    SqlParameter empnoParm = new SqlParameter("@CLC_RES_FUND", Fund);
                    sqlParamList.Add(empnoParm);
                }
                if (Fundseq != string.Empty)
                {
                    SqlParameter empnoParm = new SqlParameter("@CLC_RES_SEQ", Fundseq);
                    sqlParamList.Add(empnoParm);
                }
                if (ServiceSeq != string.Empty)
                {
                    SqlParameter empnoParm = new SqlParameter("@CLC_SEQ", ServiceSeq);
                    sqlParamList.Add(empnoParm);
                }
                if (strDate != string.Empty)
                {
                    SqlParameter empnoParm = new SqlParameter("@CLC_RES_DATE", strDate);
                    sqlParamList.Add(empnoParm);
                }                
                if (strBdccostcenter != string.Empty)
                {
                    SqlParameter empnoParm = new SqlParameter("@BDC_COST_CENTER", strBdccostcenter);
                    sqlParamList.Add(empnoParm);
                }
                if (strbdcGlAccount != string.Empty)
                {
                    SqlParameter empnoParm = new SqlParameter("@BDC_GL_ACCOUNT", strbdcGlAccount);
                    sqlParamList.Add(empnoParm);
                }
                if (strBudgetYear != string.Empty)
                {
                    SqlParameter empnoParm = new SqlParameter("@BDC_BUDGET_YEAR", strBudgetYear);
                    sqlParamList.Add(empnoParm);
                }
                if (strIntOrder != string.Empty)
                {
                    SqlParameter empnoParm = new SqlParameter("@BDC_INT_ORDER", strIntOrder);
                    sqlParamList.Add(empnoParm);
                }
                if (strAccountType != string.Empty)
                {
                    SqlParameter empnoParm = new SqlParameter("@BDC_ACCOUNT_TYPE", strAccountType);
                    sqlParamList.Add(empnoParm);
                }
                if (strBdcstart != string.Empty)
                {
                    SqlParameter empnoParm = new SqlParameter("@BDC_START", strBdcstart);
                    sqlParamList.Add(empnoParm);
                }
                if (strBdcEnd != string.Empty)
                {
                    SqlParameter empnoParm = new SqlParameter("@BDC_END", strBdcEnd);
                    sqlParamList.Add(empnoParm);
                }

                if (strType != string.Empty)
                {
                    SqlParameter empnoParm = new SqlParameter("@Type", strType);
                    sqlParamList.Add(empnoParm);
                }
                if (strLockType != string.Empty)
                {
                    SqlParameter empnoParm = new SqlParameter("@LockType", strLockType);
                    sqlParamList.Add(empnoParm);
                }
                 if (strLockBy != string.Empty)
                {
                    SqlParameter empnoParm = new SqlParameter("@LOCK_BY", strLockBy);
                    sqlParamList.Add(empnoParm);
                }
                if (strLockScreen != string.Empty)
                {
                    SqlParameter empnoParm = new SqlParameter("@LOCK_SCREEN", strLockScreen);
                    sqlParamList.Add(empnoParm);
                }                
                
                SqlParameter sqloutMsg = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
                sqloutMsg.Value = string.Empty;
                sqloutMsg.Direction = ParameterDirection.Output;
                sqlParamList.Add(sqloutMsg);

                boolstatus = EMSBDCDB.InsertEMSLOCKDATA(sqlParamList);
                strOutMsg = sqloutMsg.Value.ToString();
            }
            catch (Exception ex)
            {
                return false;
            }
            return boolstatus;
        }

        public bool UPDATEEMSLOCKED(string strType, string strTableName, string struserid, string strHierchy, string strKeydata, string Fund, string strResDate, string strResseq, string strClcSeq)
        {
            
            bool boolstatus = true;
            try
            {

                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                if (strType != string.Empty)
                {
                    SqlParameter empnoParm = new SqlParameter("@Type", strType);
                    sqlParamList.Add(empnoParm);
                }
                if (strTableName != string.Empty)
                {
                    SqlParameter empnoParm = new SqlParameter("@TableName", strTableName);
                    sqlParamList.Add(empnoParm);
                }
                if (struserid != string.Empty)
                {
                    SqlParameter empnoParm = new SqlParameter("@Userid", struserid);
                    sqlParamList.Add(empnoParm);
                }

                if (strHierchy != string.Empty)
                {
                    SqlParameter clcyear = new SqlParameter("@Hierachy", strHierchy);
                    sqlParamList.Add(clcyear);
                }

                if (strKeydata != string.Empty)
                {
                    SqlParameter empnoParm = new SqlParameter("@keyData", strKeydata);
                    sqlParamList.Add(empnoParm);
                }
                if (Fund != string.Empty)
                {
                    SqlParameter empnoParm = new SqlParameter("@FUND", Fund);
                    sqlParamList.Add(empnoParm);
                }
                if (strResDate != string.Empty)
                {
                    SqlParameter empnoParm = new SqlParameter("@ResDate", strResDate);
                    sqlParamList.Add(empnoParm);
                }
                if (strResseq != string.Empty)
                {
                    SqlParameter empnoParm = new SqlParameter("@ResSeq", strResseq);
                    sqlParamList.Add(empnoParm);
                }
                if (strClcSeq != string.Empty)
                {
                    SqlParameter empnoParm = new SqlParameter("@ClcSeq", strClcSeq);
                    sqlParamList.Add(empnoParm);
                }

                boolstatus = EMSBDCDB.UpdateEMSLOCK(sqlParamList);
               
            }
            catch (Exception ex)
            {
                return false;
            }
            return boolstatus;
        }


        public bool InsertUpdateDelINVLOG(InvoiceLogEntity invlogEntity,out string strinvoiceid)
        {
            bool boolSuccess = false;
            string strInvoiceidout = string.Empty;
            try
            {
               
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                if (invlogEntity.INVLOG_ID != string.Empty)
                    sqlParamList.Add(new SqlParameter("@INVLOG_ID", invlogEntity.INVLOG_ID));
                sqlParamList.Add(new SqlParameter("@INVLOG_AGENCY", invlogEntity.INVLOG_AGENCY));
                sqlParamList.Add(new SqlParameter("@INVLOG_DEPT", invlogEntity.INVLOG_DEPT));
                sqlParamList.Add(new SqlParameter("@INVLOG_PROGRAM", invlogEntity.INVLOG_PROGRAM));
                sqlParamList.Add(new SqlParameter("@INVLOG_APP", invlogEntity.INVLOG_APP));
                sqlParamList.Add(new SqlParameter("@INVLOG_ORIG_NAME", invlogEntity.INVLOG_ORIG_NAME));
                if (invlogEntity.INVLOG_UPLOAD_BY != string.Empty)
                    sqlParamList.Add(new SqlParameter("@INVLOG_UPLOAD_BY", invlogEntity.INVLOG_UPLOAD_BY));
                sqlParamList.Add(new SqlParameter("@INVLOG_UPLOAD_AS", invlogEntity.INVLOG_UPLOAD_AS));
                if (invlogEntity.INVLOG_LSTC_OPERATOR != string.Empty)
                    sqlParamList.Add(new SqlParameter("@INVLOG_LSTC_OPERATOR", invlogEntity.INVLOG_LSTC_OPERATOR));
                if (invlogEntity.INVLOG_DELETED_BY != string.Empty)
                    sqlParamList.Add(new SqlParameter("@INVLOG_DELETED_BY", invlogEntity.INVLOG_DELETED_BY));
                sqlParamList.Add(new SqlParameter("@Mode", invlogEntity.Mode));

                
                SqlParameter sqlInvoiceid = new SqlParameter("@INVLOGIDOUT", SqlDbType.Int, 10);
                sqlInvoiceid.Value = invlogEntity.INVLOG_ID;
                sqlInvoiceid.Direction = ParameterDirection.Output;
                sqlParamList.Add(sqlInvoiceid);

                boolSuccess = EMSBDCDB.InsertUpdateDelInvoiceLog(sqlParamList);
                strInvoiceidout = sqlInvoiceid.Value.ToString();

            }
            catch (Exception ex)
            {

                boolSuccess = false;
            }

            strinvoiceid = strInvoiceidout.ToString();
            return boolSuccess;
        }


        public List<InvoiceLogEntity> INVOICE_LOG_GET(string strLogId,string strAgeny,string strDept,string strProg,string stryear,string strApp)
        {
            List<InvoiceLogEntity> invlogdata = new List<InvoiceLogEntity>();
            try
            {
                DataSet casevotds = Captain.DatabaseLayer.EMSBDCDB.INVOICE_LOG_GET(strLogId, strAgeny,  strDept,  strProg,  stryear,  strApp);
                if (casevotds != null && casevotds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in casevotds.Tables[0].Rows)
                    {
                        invlogdata.Add(new InvoiceLogEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return invlogdata;
            }

            return invlogdata;
        }

        
    }
}
