using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Captain.Common.Model.Objects;
using Captain.DatabaseLayer;

namespace Captain.Common.Model.Data
{
    public class TmsApcnData
    {

        public TmsApcnData(CaptainModel model)
        {
            Model = model;
        }

        #region Properties

        public CaptainModel Model { get; set; }

        public string UserId { get; set; }

        #endregion

        public List<CommonEntity> GetTimesSettings()
        {
            List<CommonEntity> incomeInterval = new List<CommonEntity>();
            incomeInterval.Add(new CommonEntity("5", "5"));
            incomeInterval.Add(new CommonEntity("10", "10"));
            incomeInterval.Add(new CommonEntity("15", "15"));
            incomeInterval.Add(new CommonEntity("20", "20"));
            incomeInterval.Add(new CommonEntity("25", "25"));
            incomeInterval.Add(new CommonEntity("30", "30"));
            incomeInterval.Add(new CommonEntity("35", "35"));
            incomeInterval.Add(new CommonEntity("40", "40"));
            incomeInterval.Add(new CommonEntity("45", "45"));
            incomeInterval.Add(new CommonEntity("50", "50"));
            incomeInterval.Add(new CommonEntity("55", "55"));
            incomeInterval.Add(new CommonEntity("60", "60"));
            return incomeInterval;
        }

        public List<TmsApcnEntity> GetTmsApcn()
        {
            List<TmsApcnEntity> TmsApcnProfile = new List<TmsApcnEntity>();
            try
            {
                DataSet TmsApcnData = TmsApcn.GetTMSAPCN();
                if (TmsApcnData != null && TmsApcnData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in TmsApcnData.Tables[0].Rows)
                    {
                        TmsApcnProfile.Add(new TmsApcnEntity(row, string.Empty));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return TmsApcnProfile;
            }

            return TmsApcnProfile;
        }

        public List<TmsApcnEntity> GetTMSAPCNadpyldt(string Agency, string Dept, string Program, string Year, string Location, string Date, string Type)
        {
            List<TmsApcnEntity> TmsApcnProfile = new List<TmsApcnEntity>();
            try
            {
                DataSet TmsApcnData = TmsApcn.GetTMSAPCNadpyldt(Agency, Dept, Program, Year, Location, Date, Type);
                if (TmsApcnData != null && TmsApcnData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in TmsApcnData.Tables[0].Rows)
                    {
                        TmsApcnProfile.Add(new TmsApcnEntity(row, string.Empty));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return TmsApcnProfile;
            }

            return TmsApcnProfile;
        }

        public List<APPTEMPLATESEntity> GetAPPTEMPLATESadpyldt(string Agency, string Dept, string Program, string Year, string Location, string FDate,string TDate, string Type)
        {
            List<APPTEMPLATESEntity> TmsApcnProfile = new List<APPTEMPLATESEntity>();
            try
            {
                DataSet TmsApcnData = TmsApcn.GetAPPTEMPLATESadpyldt(Agency, Dept, Program, Year, Location, FDate,TDate, Type);
                if (TmsApcnData != null && TmsApcnData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in TmsApcnData.Tables[0].Rows)
                    {
                        TmsApcnProfile.Add(new APPTEMPLATESEntity(row, string.Empty));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return TmsApcnProfile;
            }

            return TmsApcnProfile;
        }

        public List<APPTEMPLATESEntity> GetAPPTEMPLATESadpysitedates(string Agency, string Dept, string Program, string Year, string Location, string FDate, string TDate, string Type,string strEntityType)
        {
            List<APPTEMPLATESEntity> TmsApcnProfile = new List<APPTEMPLATESEntity>();
            try
            {
                DataSet TmsApcnData = TmsApcn.GetAPPTEMPLATESadpysitedates(Agency, Dept, Program, Year, Location, FDate, TDate, Type);
                if (TmsApcnData != null && TmsApcnData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in TmsApcnData.Tables[0].Rows)
                    {
                        TmsApcnProfile.Add(new APPTEMPLATESEntity(row, strEntityType));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return TmsApcnProfile;
            }

            return TmsApcnProfile;
        }


        public List<TmsApcnEntity> GetCASESITEadpy(string Agency, string Dept, string Program, string Year,string FilterType)
        {
            List<TmsApcnEntity> TmsApcnProfile = new List<TmsApcnEntity>();
            try
            {
                DataSet TmsApcnData = TmsApcn.GetCASESITEadpy(Agency, Dept, Program, Year, FilterType);
                if (TmsApcnData != null && TmsApcnData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in TmsApcnData.Tables[0].Rows)
                    {
                        TmsApcnProfile.Add(new TmsApcnEntity(row, "SITE"));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return TmsApcnProfile;
            }

            return TmsApcnProfile;
        }

        public List<TmsApptEntity> GetTmsAppt()
        {
            List<TmsApptEntity> TmsApptProfile = new List<TmsApptEntity>();
            try
            {
                DataSet TMSApptData = TmsApcn.GetTMSAPPT();
                if (TMSApptData != null && TMSApptData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in TMSApptData.Tables[0].Rows)
                    {
                        TmsApptProfile.Add(new TmsApptEntity(row, string.Empty));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return TmsApptProfile;
            }

            return TmsApptProfile;
        }

        public List<TmsApptEntity> GetTMSAPPTadpyldt(string Agency, string Dept, string Program, string Year, string Location, string Date, string Time, string TemDate,string TemType)
        {
            List<TmsApptEntity> TmsApptProfile = new List<TmsApptEntity>();
            try
            {
                DataSet TmsApptData = TmsApcn.GetTMSAPPTadpyldt(Agency, Dept, Program, Year, Location, Date, Time, TemDate,TemType);
                if (TmsApptData != null && TmsApptData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in TmsApptData.Tables[0].Rows)
                    {
                        TmsApptProfile.Add(new TmsApptEntity(row, string.Empty));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return TmsApptProfile;
            }

            return TmsApptProfile;
        }

        public List<TMSAPCNSLTEntity> GetTMSAPCNSLTadpyldt(string Agency, string Dept, string Program, string Year, string Location, string Date, string Type)
        {
            List<TMSAPCNSLTEntity> TmsApcnsltProfile = new List<TMSAPCNSLTEntity>();
            try
            {
                DataSet TmsApcnData = TmsApcn.GetTMSAPCNSLTadpyldt(Agency, Dept, Program, Year, Location, Date, Type);
                if (TmsApcnData != null && TmsApcnData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in TmsApcnData.Tables[0].Rows)
                    {
                        TmsApcnsltProfile.Add(new TMSAPCNSLTEntity(row, string.Empty));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return TmsApcnsltProfile;
            }

            return TmsApcnsltProfile;
        }


        public bool InsertUpdateTMSAPCN(TmsApcnEntity TmsApcnp)
        {
            bool boolsuccess;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@TMSAPCN_AGENCY", TmsApcnp.Agency));
                sqlParamList.Add(new SqlParameter("@TMSAPCN_DEPT", TmsApcnp.Dept));
                sqlParamList.Add(new SqlParameter("@TMSAPCN_PROGRAM", TmsApcnp.Program));
                if (TmsApcnp.Year != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSAPCN_YEAR", TmsApcnp.Year));
                }
                if (TmsApcnp.Location != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSAPCN_LOCATION", TmsApcnp.Location));
                }
                sqlParamList.Add(new SqlParameter("@TMSAPCN_DATE", TmsApcnp.Date));
                sqlParamList.Add(new SqlParameter("@TMSAPCN_TYPE", TmsApcnp.Type));
                if (TmsApcnp.SlotsPerPeriod != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSAPCN_SLOTS_PER_PERIOD", TmsApcnp.SlotsPerPeriod));
                }
                if (TmsApcnp.Mins != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSAPCN_MINS", TmsApcnp.Mins));
                }
                if (TmsApcnp.TemplateAvailble != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSAPCN_TEMPLATE_AVAILABLE", TmsApcnp.TemplateAvailble));
                }
                if (TmsApcnp.DayTable != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSAPCN_DAY_TABLE", TmsApcnp.DayTable));
                }
                if (TmsApcnp.PeriodTable != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSAPCN_PERIOD_TABLE", TmsApcnp.PeriodTable));
                }

                sqlParamList.Add(new SqlParameter("@TMSAPCN_LSTC_OPERATOR", TmsApcnp.LstcOperation));
                sqlParamList.Add(new SqlParameter("@TMSAPCN_ADD_OPERATOR", TmsApcnp.AddOperator));
                sqlParamList.Add(new SqlParameter("@Mode", TmsApcnp.Mode));
                boolsuccess = TmsApcn.InsertUpdateTMSAPCN(sqlParamList);

            }
            catch (Exception ex)
            {
                //
                return false;
            }

            return boolsuccess;
        }

        public bool InsertUpdateTMSAPCNSLT(TMSAPCNSLTEntity TmsApcnsl)
        {
            bool boolsuccess;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@TMSAPCNSLT_AGENCY", TmsApcnsl.Agency));
                sqlParamList.Add(new SqlParameter("@TMSAPCNSLT_DEPT", TmsApcnsl.Dept));
                sqlParamList.Add(new SqlParameter("@TMSAPCNSLT_PROGRAM", TmsApcnsl.Program));
                if (TmsApcnsl.Year != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSAPCNSLT_YEAR", TmsApcnsl.Year));
                }
                if (TmsApcnsl.Location != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSAPCNSLT_LOCATION", TmsApcnsl.Location));
                }
                sqlParamList.Add(new SqlParameter("@TMSAPCNSLT_DATE", TmsApcnsl.Date));
                if (TmsApcnsl.Type != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSAPCNSLT_TYPE", TmsApcnsl.Type));
                }
                if (TmsApcnsl.Time != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSAPCNSLT_TIME", TmsApcnsl.Time));
                }
                if (TmsApcnsl.Opcl != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSAPCNSLT_OPCL", TmsApcnsl.Opcl));
                }
                sqlParamList.Add(new SqlParameter("@Mode", TmsApcnsl.Mode));
                boolsuccess = TmsApcn.InsertUpdateTMSAPCNSLT(sqlParamList);

            }
            catch (Exception ex)
            {
                //
                return false;
            }

            return boolsuccess;
        }

        public bool InsertUpdateTMSAPPT(TmsApptEntity TmsAppt)
        {
            bool boolsuccess;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@TMSAPPT_AGENCY", TmsAppt.Agency));
                sqlParamList.Add(new SqlParameter("@TMSAPPT_DEPT", TmsAppt.Dept));
                sqlParamList.Add(new SqlParameter("@TMSAPPT_PROGRAM", TmsAppt.Program));
                if (TmsAppt.Year != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSAPPT_YEAR", TmsAppt.Year));
                }
                sqlParamList.Add(new SqlParameter("@TMSAPPT_LOCATION", TmsAppt.Location));
                sqlParamList.Add(new SqlParameter("@TMSAPPT_DATE", TmsAppt.Date));
                if (TmsAppt.Time != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSAPPT_TIME", TmsAppt.Time));
                }
                if (TmsAppt.SlotNumber != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSAPPT_SLOT_NUMBER", TmsAppt.SlotNumber));
                }
                if (TmsAppt.SsNumber != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSAPPT_SS_NUMBER", TmsAppt.SsNumber));
                }
                if (TmsAppt.TemplateDate != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSAPPT_TEMPLATE_DATE", TmsAppt.TemplateDate));
                }
                if (TmsAppt.TemplateType != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSAPPT_TEMPLATE_TYPE", TmsAppt.TemplateType));
                }
                if (TmsAppt.RecordType != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSAPPT_RECORD_TYPE", TmsAppt.RecordType));
                }
                if (TmsAppt.Name != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSAPPT_NAME", TmsAppt.Name));
                }
                if (TmsAppt.FirstName != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSAPPT_FIRST_NAME", TmsAppt.FirstName));
                }
                //if (TmsAppt.TelAreaCode != string.Empty)
                //{
                //    sqlParamList.Add(new SqlParameter("@TMSAPPT_TEL_AREA_CODE", TmsAppt.TelAreaCode));
                //}
                if (TmsAppt.TelNumber != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSAPPT_TEL_NUMBER", TmsAppt.TelNumber));
                }
                if (TmsAppt.StNo != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSAPPT_STNO", TmsAppt.StNo));
                }
                if (TmsAppt.Street != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSAPPT_STREET", TmsAppt.Street));
                }
                if (TmsAppt.Suffix != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSAPPT_SUFFIX", TmsAppt.Suffix));
                }
                if (TmsAppt.Apt != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSAPPT_APT", TmsAppt.Apt));
                }
                if (TmsAppt.Floor != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSAPPT_FLOOR", TmsAppt.Floor));
                }
                if (TmsAppt.City != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSAPPT_CITY", TmsAppt.City));
                }
                if (TmsAppt.State != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSAPPT_STATE", TmsAppt.State));
                }
                if (TmsAppt.Zip1 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSAPPT_ZIP1", TmsAppt.Zip1));
                }
                if (TmsAppt.Zip2 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSAPPT_ZIP2", TmsAppt.Zip2));
                }
                if (TmsAppt.HeatSource != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSAPPT_HEAT_SOURCE", TmsAppt.HeatSource));
                }
                if (TmsAppt.SourceIncome != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSAPPT_SOURCE_INCOME", TmsAppt.SourceIncome));
                }
                if (TmsAppt.ContactPerson != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSAPPT_CONTACT_PERSON", TmsAppt.ContactPerson));
                }
                if (TmsAppt.ContactDate != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSAPPT_CONTACT_DATE", TmsAppt.ContactDate));
                }
                if (TmsAppt.Sex != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSAPPT_SEX", TmsAppt.Sex));
                }
                if (TmsAppt.CellProvider != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSAPPT_CELL_PROVIDER", TmsAppt.CellProvider));
                }
                //if (TmsAppt.CellAreaCode != string.Empty)
                //{
                //    sqlParamList.Add(new SqlParameter("@TMSAPPT_CELL_AREA_CODE", TmsAppt.CellAreaCode));
                //}
                if (TmsAppt.CellNumber != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSAPPT_CELL_NUMBER", TmsAppt.CellNumber));
                }
                if (TmsAppt.CaseWorker != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSAPPT_CASE_WORKER", TmsAppt.CaseWorker));
                }

                sqlParamList.Add(new SqlParameter("@TMSAPPT_LSTC_OPERATOR", TmsAppt.LstcOperation));
                sqlParamList.Add(new SqlParameter("@TMSAPPT_ADD_OPERATOR", TmsAppt.AddOperator));
                sqlParamList.Add(new SqlParameter("@Mode", TmsAppt.Mode));
                boolsuccess = CaseMst.InsertCASEINCOME(sqlParamList);

            }
            catch (Exception ex)
            {
                //
                return false;
            }

            return boolsuccess;
        }

        public bool InsertUpdateAPPTEMPLATES(APPTEMPLATESEntity TmsApcnp)
        {
            bool boolsuccess;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@APPTEMPL_AGY", TmsApcnp.Agency));
                sqlParamList.Add(new SqlParameter("@APPTEMPL_DEPT", TmsApcnp.Dept));
                sqlParamList.Add(new SqlParameter("@APPTEMPL_PROG", TmsApcnp.Program));
                if (TmsApcnp.Year != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@APPTEMPL_YEAR", TmsApcnp.Year));
                }
                if (TmsApcnp.Location != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@APPTEMPL_SITE", TmsApcnp.Location));
                }
                sqlParamList.Add(new SqlParameter("@APPTEMPL_FDATE", TmsApcnp.FDate));
                sqlParamList.Add(new SqlParameter("@APPTEMPL_TDATE", TmsApcnp.TDate));
                sqlParamList.Add(new SqlParameter("@APPTEMPL_ID", TmsApcnp.Type));
                if (TmsApcnp.SlotsPerPeriod != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@APPTEMPL_SLOTS", TmsApcnp.SlotsPerPeriod));
                }
                if (TmsApcnp.Mins != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@APPTEMPL_MINS", TmsApcnp.Mins));
                }
                if (TmsApcnp.TemplateAvailble != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@APPTEMPL_OPEN", TmsApcnp.TemplateAvailble));
                }
                if (TmsApcnp.DayTable != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@APPTEMPL_DAYS", TmsApcnp.DayTable));
                }
                if (TmsApcnp.PeriodTable != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@APPTEMPL_PERIOD_TABLE", TmsApcnp.PeriodTable));
                }

                sqlParamList.Add(new SqlParameter("@APPTEMPL_LSTC_OPERATOR", TmsApcnp.LstcOperation));
                sqlParamList.Add(new SqlParameter("@APPTEMPL_ADD_OPERATOR", TmsApcnp.AddOperator));
                sqlParamList.Add(new SqlParameter("@Mode", TmsApcnp.Mode));
                boolsuccess = TmsApcn.InsertUpdateAPPTEMPLATES(sqlParamList);

            }
            catch (Exception ex)
            {
                //
                return false;
            }

            return boolsuccess;
        }


        public List<CaseNotesEntity> GetCaseNotesScreenFieldName(string ScreeName, string FieldName)
        {
            List<CaseNotesEntity> CaseNotesProfile = new List<CaseNotesEntity>();
            try
            {
                DataSet TmsApptData = TmsApcn.GetCaseNotesScreenFieldName(ScreeName, FieldName);
                if (TmsApptData != null && TmsApptData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in TmsApptData.Tables[0].Rows)
                    {
                        CaseNotesProfile.Add(new CaseNotesEntity(row, string.Empty));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return CaseNotesProfile;
            }

            return CaseNotesProfile;
        }

        public List<CaseNotesEntity> GetCaseNotesWaitList(string FieldName)
        {
            List<CaseNotesEntity> CaseNotesProfile = new List<CaseNotesEntity>();
            try
            {
                DataSet TmsApptData = TmsApcn.GetCaseNotesWaitingList(FieldName);
                if (TmsApptData != null && TmsApptData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in TmsApptData.Tables[0].Rows)
                    {
                        CaseNotesProfile.Add(new CaseNotesEntity(row, string.Empty));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return CaseNotesProfile;
            }

            return CaseNotesProfile;
        }

        public List<CaseNotesEntity> GETCASENOTESKeyFields(string ScreeName, string FieldName)
        {
            List<CaseNotesEntity> CaseNotesProfile = new List<CaseNotesEntity>();
            try
            {
                DataSet TmsApptData = TmsApcn.GETCASENOTESKeyFields(ScreeName, FieldName);
                if (TmsApptData != null && TmsApptData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in TmsApptData.Tables[0].Rows)
                    {
                        CaseNotesProfile.Add(new CaseNotesEntity(row, "KEYFIELD"));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return CaseNotesProfile;
            }

            return CaseNotesProfile;
        }


        

        public bool InsertUpdateCaseNotes(CaseNotesEntity  caseNotes)
        {
            bool boolsuccess;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@CASENOTES_SCREEN", caseNotes.ScreenName));
                sqlParamList.Add(new SqlParameter("@CASENOTES_KEYFIELD", caseNotes.FieldName));
                sqlParamList.Add(new SqlParameter("@CASENOTES_APP_NO", caseNotes.AppliCationNo));
                sqlParamList.Add(new SqlParameter("@CASENOTES_LSTC_OPERATOR", caseNotes.LstcOperation));
                sqlParamList.Add(new SqlParameter("@CASENOTES_ADD_OPERATOR", caseNotes.AddOperator));
                sqlParamList.Add(new SqlParameter("@CASENOTES_DATA", caseNotes.Data));
                sqlParamList.Add(new SqlParameter("@Mode", caseNotes.Mode));
                boolsuccess = TmsApcn.InsertUpdateDelCaseNotes(sqlParamList);

            }
            catch (Exception ex)
            {
                //
                return false;
            }

            return boolsuccess;
        }

        public bool InsertPROCHIST(string PRH_SCR_NAME,string PRH_KEYFIELD,string PRH_USER_ID,string PRH_Short_Notes)
        {
            bool boolsuccess;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@PRH_SCR_NAME", PRH_SCR_NAME));
                sqlParamList.Add(new SqlParameter("@PRH_KEYFIELD", PRH_KEYFIELD));
                sqlParamList.Add(new SqlParameter("@PRH_USER_ID", PRH_USER_ID));
                sqlParamList.Add(new SqlParameter("@PRH_Short_Notes", PRH_Short_Notes));
                boolsuccess = TmsApcn.InsertPROCHIST(sqlParamList);

            }
            catch (Exception ex)
            {
                //
                return false;
            }

            return boolsuccess;
        }
        public List<CaseNotesEntity> BrowseCaseNotes(string Screen_Name, string Field)//string Appno,
        {
            List<CaseNotesEntity> CaseNotesProfile = new List<CaseNotesEntity>();
            try
            {
                DataSet CASENOTE = TmsApcn.BrowseCase_Notes(Screen_Name, Field);// Appno,
                if (CASENOTE != null && CASENOTE.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASENOTE.Tables[0].Rows)
                    {
                        CaseNotesProfile.Add(new CaseNotesEntity(row, string.Empty));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return CaseNotesProfile;
            }

            return CaseNotesProfile;
        }


        public bool DeleteTMSAPCN(TmsApcnEntity TmsApcne)
        {
            bool boolsuccess;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@TMSAPCN_AGENCY", TmsApcne.Agency));
                sqlParamList.Add(new SqlParameter("@TMSAPCN_DEPT", TmsApcne.Dept));
                sqlParamList.Add(new SqlParameter("@TMSAPCN_PROGRAM", TmsApcne.Program));
                if (TmsApcne.Year != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSAPCN_YEAR", TmsApcne.Year));
                }
                if (TmsApcne.Location != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSAPCN_LOCATION", TmsApcne.Location));
                }
                sqlParamList.Add(new SqlParameter("@TMSAPCN_DATE", TmsApcne.Date));
                if (TmsApcne.Type != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSAPCN_TYPE", TmsApcne.Type));
                }

                boolsuccess = TmsApcn.DeleteTMSAPCN(sqlParamList);

            }
            catch (Exception ex)
            {
                //
                return false;
            }

            return boolsuccess;
        }

        public bool DeleteTMSAPPT(TmsApptEntity TmsAppt)
        {
            bool boolsuccess;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@TMSAPPT_AGENCY", TmsAppt.Agency));
                sqlParamList.Add(new SqlParameter("@TMSAPPT_DEPT", TmsAppt.Dept));
                sqlParamList.Add(new SqlParameter("@TMSAPPT_PROGRAM", TmsAppt.Program));
                if (TmsAppt.Year != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSAPPT_YEAR", TmsAppt.Year));
                }
                if (TmsAppt.Location != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSAPPT_LOCATION", TmsAppt.Location));
                }
                sqlParamList.Add(new SqlParameter("@TMSAPPT_DATE", TmsAppt.Date));
                if (TmsAppt.Time != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSAPPT_TIME", TmsAppt.Time));
                }
                if (TmsAppt.SlotNumber != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSAPPT_SLOT_NUMBER", TmsAppt.SlotNumber));
                }


                boolsuccess = TmsApcn.DeleteTMSAPPT(sqlParamList);

            }
            catch (Exception ex)
            {
                //
                return false;
            }

            return boolsuccess;
        }

        public string TmsApcnKeyCheck(TmsApcnEntity TmsApcnp)
        {
            string strmsg = "";
            try
            {
                
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@TMSAPCN_AGENCY", TmsApcnp.Agency));
                sqlParamList.Add(new SqlParameter("@TMSAPCN_DEPT", TmsApcnp.Dept));
                sqlParamList.Add(new SqlParameter("@TMSAPCN_PROGRAM", TmsApcnp.Program));
                if (TmsApcnp.Year != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSAPCN_YEAR", TmsApcnp.Year));
                }
                if (TmsApcnp.Location != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSAPCN_LOCATION", TmsApcnp.Location));
                }
                if (TmsApcnp.Date != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSAPCN_DATE", TmsApcnp.Date));
                }
                if (TmsApcnp.Type != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@TMSAPCN_Type", TmsApcnp.Type));
                }
                sqlParamList.Add(new SqlParameter("@Mode", TmsApcnp.Mode));
                SqlParameter parameterMsg = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
                parameterMsg.Direction = ParameterDirection.Output;
                sqlParamList.Add(parameterMsg);
                Captain.DatabaseLayer.TmsApcn.TmsApcnkeyCheck(sqlParamList);
                strmsg = parameterMsg.Value.ToString() ;
            }
            catch (Exception ex)
            {
                //
                return "";
            }

            return strmsg;
        }

        public string APPTTEMPLKeyCheck(APPTEMPLATESEntity TmsApcnp)
        {
            string strmsg = "";
            try
            {

                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@APPTEMPL_AGY", TmsApcnp.Agency));
                sqlParamList.Add(new SqlParameter("@APPTEMPL_DEPT", TmsApcnp.Dept));
                sqlParamList.Add(new SqlParameter("@APPTEMPL_PROGRAM", TmsApcnp.Program));
                if (TmsApcnp.Year != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@APPTEMPL_YEAR", TmsApcnp.Year));
                }
                if (TmsApcnp.Location != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@APPTEMPL_SITE", TmsApcnp.Location));
                }
                if (TmsApcnp.FDate != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@APPTEMPL_DATE", TmsApcnp.FDate));
                }
                if (TmsApcnp.Type != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@APPTEMPL_Type", TmsApcnp.Type));
                }
                sqlParamList.Add(new SqlParameter("@Mode", TmsApcnp.Mode));
                SqlParameter parameterMsg = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
                parameterMsg.Direction = ParameterDirection.Output;
                sqlParamList.Add(parameterMsg);
                Captain.DatabaseLayer.TmsApcn.APPTTEMPLCheck(sqlParamList);
                strmsg = parameterMsg.Value.ToString();
            }
            catch (Exception ex)
            {
                //
                return "";
            }

            return strmsg;
        }


        public List<EMPLFUNCEntity> GetCaseNotesDesc(string UserId, string ModuleCode, string Hierarchy, string CaseNotesFieldName)
        {
            List<EMPLFUNCEntity> EMPLFUNCprof = new List<EMPLFUNCEntity>();
            try
            {
                DataSet EMPLFUNCData = TmsApcn.GetCaseNotesDesc(UserId, ModuleCode, Hierarchy, CaseNotesFieldName);
                if (EMPLFUNCData != null && EMPLFUNCData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in EMPLFUNCData.Tables[0].Rows)
                    {
                        EMPLFUNCprof.Add(new EMPLFUNCEntity(row,string.Empty));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return EMPLFUNCprof;
            }

            return EMPLFUNCprof;
        }

        public List<TMSB0001Entity> GetTMSB0001(string Agency, string Dept, string Program, string Year, string startdate, string EndDate)
        {
            List<TMSB0001Entity> TmsApcnProfile = new List<TMSB0001Entity>();
            try
            {
                DataSet TmsApcnData = TmsApcn.TMSB0001_Report(Agency, Dept, Program, Year, startdate, EndDate);
                if (TmsApcnData != null && TmsApcnData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in TmsApcnData.Tables[0].Rows)
                    {
                        TmsApcnProfile.Add(new TMSB0001Entity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return TmsApcnProfile;
            }

            return TmsApcnProfile;
        }

        public List<TMSB4007ENTITY> GetTMSB4007_Det(string Agency, string Dept, string Program, string Year, string btnresult)
        {
            List<TMSB4007ENTITY> Tmsb4007Profile = new List<TMSB4007ENTITY>();
            try
            {
                DataSet TmsApcnData = TmsApcn.TMSB4007(Agency, Dept, Program, Year, btnresult);
                if (TmsApcnData != null && TmsApcnData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in TmsApcnData.Tables[0].Rows)
                    {
                        Tmsb4007Profile.Add(new TMSB4007ENTITY(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return Tmsb4007Profile;
            }
            return Tmsb4007Profile;
        }


        public List<TMSB0005ENTITY> GetTMSB0005(string Agency, string Dept, string Program, string Year)
        {
            List<TMSB0005ENTITY> TmsApcnProfile = new List<TMSB0005ENTITY>();
            try
            {
                DataSet TmsApcnData = TMSAllDB.GETTMSB4005(Agency, Dept, Program, Year);
                if (TmsApcnData != null && TmsApcnData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in TmsApcnData.Tables[0].Rows)
                    {
                        TmsApcnProfile.Add(new TMSB0005ENTITY(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return TmsApcnProfile;
            }

            return TmsApcnProfile;
        }

        public List<SqlParameter> Prepare_Notice_SqlParameters_List(NotesEntity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                if (Opretaion_Mode != "Browse")
                    sqlParamList.Add(new SqlParameter("@Row_Type", Entity.Rec_Type));

                sqlParamList.Add(new SqlParameter("@VDN_AGENCY", Entity.VDN_AGENCY));
                sqlParamList.Add(new SqlParameter("@VDN_DEPT", Entity.VDN_DEPT));
                sqlParamList.Add(new SqlParameter("@VDN_PROGRAM", Entity.VDN_PROGRAM));
                sqlParamList.Add(new SqlParameter("@VDN_YEAR", Entity.VDN_YEAR));
                sqlParamList.Add(new SqlParameter("@VDN_APP_NO", Entity.VDN_APP_NO));


                sqlParamList.Add(new SqlParameter("@VDN_BY_PROG", Entity.VDN_BY_PROG));
                //if (!string.IsNullOrEmpty(Entity.VDN_TYPE.Trim()))
                    sqlParamList.Add(new SqlParameter("@VDN_TYPE", Entity.VDN_TYPE));
                //if (!string.IsNullOrEmpty(Entity.VDN_VENDOR.Trim()))
                    sqlParamList.Add(new SqlParameter("@VDN_VENDOR", Entity.VDN_VENDOR));
                if (!string.IsNullOrEmpty(Entity.VDN_SEQUENCE.Trim()))
                    sqlParamList.Add(new SqlParameter("@VDN_SEQUENCE", Entity.VDN_SEQUENCE));
                if (!string.IsNullOrEmpty(Entity.VDN_DATE_REPORTED.Trim()))
                    sqlParamList.Add(new SqlParameter("@VDN_DATE_REPORTED", Entity.VDN_DATE_REPORTED));
                if (!string.IsNullOrEmpty(Entity.VDN_DATE_SEQUENCE.Trim()))
                    sqlParamList.Add(new SqlParameter("@VDN_DATE_SEQUENCE", Entity.VDN_DATE_SEQUENCE));
                if (!string.IsNullOrEmpty(Entity.VDN_RERUN.Trim()))
                    sqlParamList.Add(new SqlParameter("@VDN_RERUN", Entity.VDN_RERUN));
                if (!string.IsNullOrEmpty(Entity.VDN_CERT_STATUS.Trim()))
                    sqlParamList.Add(new SqlParameter("@VDN_CERT_STATUS", Entity.VDN_CERT_STATUS));
                if (!string.IsNullOrEmpty(Entity.VDN_PAGE_NUMBER.Trim()))
                    sqlParamList.Add(new SqlParameter("@VDN_PAGE_NUMBER", Entity.VDN_PAGE_NUMBER));
                if (!string.IsNullOrEmpty(Entity.VDN_TMSB5511_CATEGORY.Trim()))
                    sqlParamList.Add(new SqlParameter("@VDN_TMSB5511_CATEGORY", Entity.VDN_TMSB5511_CATEGORY));
                if (!string.IsNullOrEmpty(Entity.VDN_FILE_FORMAT.Trim()))
                    sqlParamList.Add(new SqlParameter("@VDN_FILE_FORMAT", Entity.VDN_FILE_FORMAT));
                if (!string.IsNullOrEmpty(Entity.PMR_AUTH_NUM.Trim()))
                    sqlParamList.Add(new SqlParameter("@PMR_AUTH_NUM", Entity.PMR_AUTH_NUM));
               
                sqlParamList.Add(new SqlParameter("@VDN_LSTC_OPERATOR", Entity.VDN_LSTC_OPERATOR));


                if (Opretaion_Mode == "Browse")
                {
                    sqlParamList.Add(new SqlParameter("@VDN_DATE_ADD", Entity.VDN_DATE_ADD));
                    sqlParamList.Add(new SqlParameter("@VDN_ADD_OPERATOR", Entity.VDN_ADD_OPERATOR));
                    sqlParamList.Add(new SqlParameter("@VDN_DATE_LSTC", Entity.VDN_DATE_LSTC));
                }
            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }



        public bool UpdateNOTICE(NotesEntity Entity, string Operation_Mode, out string Sql_Reslut_Msg)
        {
            bool boolsuccess = true;
            Sql_Reslut_Msg = "Success";
            //Msg = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList = Prepare_Notice_SqlParameters_List(Entity, Operation_Mode);

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.UPDATE_NOTICE", out Sql_Reslut_Msg);  
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        public List<NotesEntity> GetReRunDates(string Agency, string Dept, string Program, string Year,string FormType,string Vendor)
        {
            List<NotesEntity> TmsApcnProfile = new List<NotesEntity>();
            try
            {
                DataSet TmsApcnData = TMSAllDB.GetReRunDates(Agency, Dept, Program, Year, FormType,Vendor);
                if (TmsApcnData != null && TmsApcnData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in TmsApcnData.Tables[0].Rows)
                    {
                        TmsApcnProfile.Add(new NotesEntity(row,string.Empty));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return TmsApcnProfile;
            }

            return TmsApcnProfile;
        }

        public List<NotesEntity> GetReRunDatesForTMSBCHCT(string Agency, string Dept, string Program, string Year, string FormType, string Vendor)
        {
            List<NotesEntity> TmsApcnProfile = new List<NotesEntity>();
            try
            {
                DataSet TmsApcnData = TMSAllDB.GetReRunDates(Agency, Dept, Program, Year, FormType, Vendor);
                if (TmsApcnData != null && TmsApcnData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in TmsApcnData.Tables[0].Rows)
                    {
                        TmsApcnProfile.Add(new NotesEntity(row, "TMSBCHCT"));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return TmsApcnProfile;
            }

            return TmsApcnProfile;
        }


        /* public List<TMSB0006Entity> TMSB0006_Report(string Agency, string Dept, string Program, string Year, string Frmdate,string Todt,string Sort)  //Vikash commented on 04/20/2023 for Certification/Recert Report 
         {
             List<TMSB0006Entity> TmsApcnProfile = new List<TMSB0006Entity>();
             try
             {
                 DataSet TmsApcnData = TmsApcn.TMSB4006(Agency, Dept, Program, Year, Frmdate,Todt,string.Empty,string.Empty);


                 if (TmsApcnData != null && TmsApcnData.Tables[0].Rows.Count > 0)
                 {
                     DataTable dt = TmsApcnData.Tables[0];
                     DataView dv = new DataView(dt);
                     if (Sort == "D")
                     {
                         dv.Sort = "MST_INTAKE_DATE";
                         dt = dv.ToTable();
                     }
                     else
                     {
                         dv.Sort = "SNP_NAME_IX_LAST";
                         dt = dv.ToTable();
                     }

                     foreach (DataRow row in dt.Rows)
                     {
                         TmsApcnProfile.Add(new TMSB0006Entity(row));
                     }
                 }
             }
             catch (Exception ex)
             {
                 //
                 return TmsApcnProfile;
             }

             return TmsApcnProfile;
         }*/

        public List<TMSB0011_nondelverEntity> TMSB00011_Report(string Agency, string Dept, string Program, string Year, string Frmdate, string Todt, string UserName,string Sort)
        {
            List<TMSB0011_nondelverEntity> TmsApcnProfile = new List<TMSB0011_nondelverEntity>();
            try
            {
                DataSet TmsApcnData = TmsApcn.TMSB4011_NONDeliver(Agency, Dept, Program, Year, Frmdate, Todt, UserName);


                if (TmsApcnData != null && TmsApcnData.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = TmsApcnData.Tables[0];
                    DataView dv = new DataView(dt);
                    if (Sort == "A")
                    {
                        dv.Sort = "LPB_APP_NO,SNP_NAME_IX_LAST,SNP_NAME_IX_FI";
                        dt = dv.ToTable();
                    }
                    else if(Sort=="C")
                    {
                        dv.Sort = "SNP_NAME_IX_LAST,SNP_NAME_IX_FI";
                        dt = dv.ToTable();
                    }
                    else if (Sort == "V")
                    {
                        dv.Sort = "PMR_VENDOR,SNP_NAME_IX_LAST,SNP_NAME_IX_FI";
                        dt = dv.ToTable();
                    }

                    foreach (DataRow row in dt.Rows)
                    {
                        TmsApcnProfile.Add(new TMSB0011_nondelverEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return TmsApcnProfile;
            }

            return TmsApcnProfile;
        }

        //public List<TMSB0011_Delivers> TMSB00011_DelReport(string Agency, string Dept, string Program, string Year, string Frmdate, string Todt, string UserName, string Sort,string Vendor,string Auth)
        //{
        //    List<TMSB0011_Delivers> TmsApcnProfile = new List<TMSB0011_Delivers>();
        //    try
        //    {
        //        DataSet TmsApcnData = TmsApcn.TMSB4011_Deliver(Agency, Dept, Program, Year, Frmdate, Todt, UserName, Vendor, Auth);


        //        if (TmsApcnData != null && TmsApcnData.Tables[0].Rows.Count > 0)
        //        {
        //            DataTable dt = TmsApcnData.Tables[0];
        //            DataView dv = new DataView(dt);
        //            if (Sort == "A")
        //            {
        //                dv.Sort = "LPB_APP_NO,SNP_NAME_IX_LAST,SNP_NAME_IX_FI";
        //                dt = dv.ToTable();
        //            }
        //            else if (Sort == "C")
        //            {
        //                dv.Sort = "SNP_NAME_IX_LAST,SNP_NAME_IX_FI";
        //                dt = dv.ToTable();
        //            }
        //            else if (Sort == "V")
        //            {
        //                dv.Sort = "PMR_VENDOR,SNP_NAME_IX_LAST,SNP_NAME_IX_FI";
        //                dt = dv.ToTable();
        //            }

        //            foreach (DataRow row in dt.Rows)
        //            {
        //                TmsApcnProfile.Add(new TMSB0011_Delivers(row));
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //
        //        return TmsApcnProfile;
        //    }

        //    return TmsApcnProfile;
        //}

        public List<APPTSCHEDULEEntity> GetAPPTSCHEDULEsitedates(string Agency, string Dept, string Program, string Year, string Site, string Date, string strMonth,string strYear, string Type,string strMainType)
        {
            List<APPTSCHEDULEEntity> Apptscheduledata = new List<APPTSCHEDULEEntity>();
            try
            {
                DataSet TmsApptscheduleData = TmsApcn.GetAPPTScheduleSitesdates(Agency, Dept, Program, Year, Site, Date,strMonth ,strYear, Type);
                if (TmsApptscheduleData != null && TmsApptscheduleData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in TmsApptscheduleData.Tables[0].Rows)
                    {
                        Apptscheduledata.Add(new APPTSCHEDULEEntity(row, strMainType));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return Apptscheduledata;
            }

            return Apptscheduledata;
        }

        public List<APPTSCHEDULEEntity> GetAPPTSCHEDULEBrowse(string Agency, string Dept, string Program, string Year, string Site, string Date,string strTime,string strSlot,string strSSN, string strUser, string strLName,string strFName,string strTel,string strDOB, string Type)
        {
            List<APPTSCHEDULEEntity> Apptscheduledata = new List<APPTSCHEDULEEntity>();
            try
            {
                DataSet TmsApptscheduleData = TmsApcn.GetAPPTScheduleBrowse(Agency, Dept, Program, Year, Site, Date,strTime,strSlot,strSSN , strUser,strLName,strFName,strTel,strDOB, Type);
                if (TmsApptscheduleData != null && TmsApptscheduleData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in TmsApptscheduleData.Tables[0].Rows)
                    {
                        Apptscheduledata.Add(new APPTSCHEDULEEntity(row, string.Empty));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return Apptscheduledata;
            }

            return Apptscheduledata;
        }


        public List<APPTSCHDHISTEntity> GetAPPTSCHDHISTBrowse(string Agency, string Dept, string Program, string Year, string Site, string Date, string strTime, string strSlot, string strSSN, string strUser, string strLName, string strFName, string strTel, string strDOB, string Type)
        {
            List<APPTSCHDHISTEntity> Apptscheduledata = new List<APPTSCHDHISTEntity>();
            try
            {
                DataSet TmsApptscheduleData = TmsApcn.GetAPPTSchdHistBrowse(Agency, Dept, Program, Year, Site, Date, strTime, strSlot, strSSN, strUser, strLName, strFName, strTel, strDOB, Type);
                if (TmsApptscheduleData != null && TmsApptscheduleData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in TmsApptscheduleData.Tables[0].Rows)
                    {
                        Apptscheduledata.Add(new APPTSCHDHISTEntity(row, string.Empty));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return Apptscheduledata;
            }

            return Apptscheduledata;
        }

        public bool InsertUpdateDelAPPTREASN(APPTREASNEntity TmsApcnp)
        {
            bool boolsuccess;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@APTRSN_AGY", TmsApcnp.APTRSN_AGY));
                sqlParamList.Add(new SqlParameter("@APTRSN_DEPT", TmsApcnp.APTRSN_DEPT));
                sqlParamList.Add(new SqlParameter("@APTRSN_PROG", TmsApcnp.APTRSN_PROG));
                if (TmsApcnp.APTRSN_YEAR != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@APTRSN_YEAR", TmsApcnp.APTRSN_YEAR));
                }
                if (TmsApcnp.APTRSN_SITE != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@APTRSN_SITE", TmsApcnp.APTRSN_SITE));
                }
                sqlParamList.Add(new SqlParameter("@APTRSN_DATE", TmsApcnp.APTRSN_DATE));
                sqlParamList.Add(new SqlParameter("@APTRSN_REASON", TmsApcnp.APTRSN_REASON));
               
                sqlParamList.Add(new SqlParameter("@APTRSN_LSTC_OPERATOR", TmsApcnp.APTRSN_LSTC_OPERATOR));
                sqlParamList.Add(new SqlParameter("@APTRSN_ADD_OPERATOR", TmsApcnp.APTRSN_ADD_OPERATOR));
                sqlParamList.Add(new SqlParameter("@Mode", TmsApcnp.Mode));
                boolsuccess = TmsApcn.InsertUpdateAPPTREASN(sqlParamList);

            }
            catch (Exception ex)
            {
                //
                return false;
            }

            return boolsuccess;
        }

        public List<APPTREASNEntity> GETAPPTREASONS(string Agency, string Dept, string Program, string strYear, string Site, string Date)
        {
            List<APPTREASNEntity> APPTREASNdetails = new List<APPTREASNEntity>();

            try
            {

                DataSet APPTREASONDS = Captain.DatabaseLayer.TmsApcn.GETAPPTREASONS(Agency, Dept, Program, strYear, Site,Date);

                if (APPTREASONDS != null && APPTREASONDS.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in APPTREASONDS.Tables[0].Rows)
                        APPTREASNdetails.Add(new APPTREASNEntity(row));
                }
            }
            catch (Exception ex)
            { return APPTREASNdetails; }

            return APPTREASNdetails;
        }

    }
}
