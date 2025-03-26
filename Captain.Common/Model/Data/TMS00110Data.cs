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
    public class TMS00110Data
    {

        public TMS00110Data()
        {

        }

        #region Properties

        public CaptainModel Model { get; set; }

        #endregion


        public List<TmsApptEntity> TMS00110_Browse_TMSAPPT(string Hierarchy, string Location, string Date, string Slot, string Time, string AppName, string Ssn, string FirstName, string User)
        {
            List<TmsApptEntity> AgytabProfile = new List<TmsApptEntity>();
            try
            {
                string RowType = " ";
                DataSet AgytabData = TMS00110DB.Browse_TMSAPPT(Hierarchy, Location, Date, Time, Slot, Ssn, null, null, AppName, FirstName, null, null, null, null, null, null, null, null, User);
                if (AgytabData != null && AgytabData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in AgytabData.Tables[0].Rows)
                    {
                        AgytabProfile.Add(new TmsApptEntity(row, RowType));
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

        public List<TmsApptEntity> TMS00110_Browse_TMSAPPT_Report(string Hierarchy, string Location, string Date, string Slot, string Time, string AppName, string Ssn, string FirstName, string User)
        {
            List<TmsApptEntity> AgytabProfile = new List<TmsApptEntity>();
            try
            {
                string RowType = " ";
                DataSet AgytabData = TMS00110DB.Browse_TMSAPPT(Hierarchy, Location, Date, Time, Slot, Ssn, null, null, AppName, FirstName, null, null, null, null, null, null, null, null, User);


                if (AgytabData != null && AgytabData.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = AgytabData.Tables[0];
                    DataView dv = new DataView(dt);
                    dv.Sort = "TMSAPPT_AGENCY,TMSAPPT_DEPT,TMSAPPT_PROGRAM,TMSAPPT_YEAR,TMSAPPT_LOCATION,TMSAPPT_DATE,TMSAPPT_TIME,TMSAPPT_SLOT_NUMBER";
                    dt = dv.ToTable();

                    foreach (DataRow row in dt.Rows)
                    {
                        AgytabProfile.Add(new TmsApptEntity(row, RowType));
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

        public bool UpdateTMSAPPT(TmsApptEntity Entity, Char Row_Type)
        {
            bool boolsuccess;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@Row_Type", Row_Type));
                sqlParamList.Add(new SqlParameter("@TMSAPPT_AGENCY", Entity.Agency));
                sqlParamList.Add(new SqlParameter("@TMSAPPT_DEPT", Entity.Dept));
                sqlParamList.Add(new SqlParameter("@TMSAPPT_PROGRAM", Entity.Program));
                sqlParamList.Add(new SqlParameter("@TMSAPPT_YEAR", Entity.Year));

                sqlParamList.Add(new SqlParameter("@TMSAPPT_LOCATION", Entity.Location));
                sqlParamList.Add(new SqlParameter("@TMSAPPT_DATE", Entity.Date));
                sqlParamList.Add(new SqlParameter("@TMSAPPT_TIME", Entity.Time));
                sqlParamList.Add(new SqlParameter("@TMSAPPT_SLOT_NUMBER", Entity.SlotNumber));
                sqlParamList.Add(new SqlParameter("@TMSAPPT_SS_NUMBER", Entity.SsNumber));
                sqlParamList.Add(new SqlParameter("@TMSAPPT_TEMPLATE_DATE", Entity.TemplateDate));
                sqlParamList.Add(new SqlParameter("@TMSAPPT_TEMPLATE_TYPE", Entity.TemplateType));

                sqlParamList.Add(new SqlParameter("@TMSAPPT_RECORD_TYPE", Entity.RecordType));
                sqlParamList.Add(new SqlParameter("@TMSAPPT_NAME", Entity.Name));
                sqlParamList.Add(new SqlParameter("@TMSAPPT_FIRST_NAME", Entity.FirstName));
                //sqlParamList.Add(new SqlParameter("@TMSAPPT_TEL_AREA_CODE", Entity.TelAreaCode));


                sqlParamList.Add(new SqlParameter("@TMSAPPT_TEL_NUMBER", Entity.TelNumber));
                sqlParamList.Add(new SqlParameter("@TMSAPPT_STNO", Entity.StNo));
                sqlParamList.Add(new SqlParameter("@TMSAPPT_STREET", Entity.Street));
                sqlParamList.Add(new SqlParameter("@TMSAPPT_SUFFIX", Entity.Suffix));
                sqlParamList.Add(new SqlParameter("@TMSAPPT_APT", Entity.Apt));
                sqlParamList.Add(new SqlParameter("@TMSAPPT_FLOOR", Entity.Floor));

                sqlParamList.Add(new SqlParameter("@TMSAPPT_CITY", Entity.City));
                sqlParamList.Add(new SqlParameter("@TMSAPPT_STATE", Entity.State));
                sqlParamList.Add(new SqlParameter("@TMSAPPT_ZIP1", Entity.Zip1));
                sqlParamList.Add(new SqlParameter("@TMSAPPT_ZIP2", Entity.Zip2));
                sqlParamList.Add(new SqlParameter("@TMSAPPT_HEAT_SOURCE", Entity.HeatSource));
                sqlParamList.Add(new SqlParameter("@TMSAPPT_SOURCE_INCOME", Entity.SourceIncome));
                sqlParamList.Add(new SqlParameter("@TMSAPPT_CONTACT_PERSON", Entity.ContactPerson));

                sqlParamList.Add(new SqlParameter("@TMSAPPT_CONTACT_DATE", Entity.ContactDate));
                sqlParamList.Add(new SqlParameter("@TMSAPPT_SEX", Entity.Sex));
                sqlParamList.Add(new SqlParameter("@TMSAPPT_CELL_PROVIDER", Entity.CellProvider));
                //sqlParamList.Add(new SqlParameter("@TMSAPPT_CELL_AREA_CODE", Entity.CellAreaCode));
                sqlParamList.Add(new SqlParameter("@TMSAPPT_CELL_NUMBER", Entity.CellNumber));
                sqlParamList.Add(new SqlParameter("@TMSAPPT_CASE_WORKER", Entity.CaseWorker));

                sqlParamList.Add(new SqlParameter("@TMSAPPT_LSTC_OPERATOR", Entity.LstcOperation));
                sqlParamList.Add(new SqlParameter("@TMSAPPT_ADD_OPERATOR", Entity.AddOperator));
                sqlParamList.Add(new SqlParameter("@TMSAPPT_EDITIME", Entity.EditTime));
                sqlParamList.Add(new SqlParameter("@TMSAPPT_EDITBY", Entity.EditBy));
                sqlParamList.Add(new SqlParameter("@TMSAPPT_EMAIL", Entity.Email));


                boolsuccess = Captain.DatabaseLayer.TMS00110DB.UpdateTMSAPPT(sqlParamList);
                //boolsuccess = AgyTab.UpdateAGYTAB(sqlParamList);
            }
            catch (Exception ex)
            {
                //
                return false;
            }

            return boolsuccess;
        }

        public bool Delete_TMSAPPT(string Hierarchy, string Location, string date, string Time, string Slot, string SSN)
        {
            try
            {
                Captain.DatabaseLayer.TMS00110DB.DeleteTMSAPPT(Hierarchy, Location, date, Time, Slot, SSN);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }



        public bool InsertUpdateDelAPPTSCHED(APPTSCHEDULEEntity Entity)
        {
            bool boolsuccess;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@Mode", Entity.Mode));
                sqlParamList.Add(new SqlParameter("@APPTSCHD_AGY", Entity.Agency));
                sqlParamList.Add(new SqlParameter("@APPTSCHD_DEPT", Entity.Dept));
                sqlParamList.Add(new SqlParameter("@APPTSCHD_PROG", Entity.Program));
                sqlParamList.Add(new SqlParameter("@APPTSCHD_YEAR", Entity.Year));

                if (Entity.Site != string.Empty)
                    sqlParamList.Add(new SqlParameter("@APPTSCHD_SITE", Entity.Site));
                if (Entity.Date != string.Empty)
                    sqlParamList.Add(new SqlParameter("@APPTSCHD_DATE", Entity.Date));
                if (Entity.Time != string.Empty)
                    sqlParamList.Add(new SqlParameter("@APPTSCHD_TIME", Entity.Time));

                sqlParamList.Add(new SqlParameter("@APPTSCHD_SLOT", Entity.SlotNumber));
                sqlParamList.Add(new SqlParameter("@APPTSCHD_SSN", Entity.SsNumber));
                sqlParamList.Add(new SqlParameter("@APPTSCHD_TEMPL_ID", Entity.TemplateID));

                sqlParamList.Add(new SqlParameter("@APPTSCHD_TYPE", Entity.SchdType));
                sqlParamList.Add(new SqlParameter("@APPTSCHD_LNAME", Entity.LastName));
                sqlParamList.Add(new SqlParameter("@APPTSCHD_FNAME", Entity.FirstName));
                if(Entity.DOB !=string.Empty)
                    sqlParamList.Add(new SqlParameter("@APPTSCHD_DOB", Entity.DOB));
                sqlParamList.Add(new SqlParameter("@APPTSCHD_TEL", Entity.TelNumber));
                sqlParamList.Add(new SqlParameter("@APPTSCHD_HNO", Entity.HNo));
                sqlParamList.Add(new SqlParameter("@APPTSCHD_STREET", Entity.Street));
                sqlParamList.Add(new SqlParameter("@APPTSCHD_SUFFIX", Entity.Suffix));
                sqlParamList.Add(new SqlParameter("@APPTSCHD_APT", Entity.Apt));
                sqlParamList.Add(new SqlParameter("@APPTSCHD_FLOOR", Entity.Floor));
                sqlParamList.Add(new SqlParameter("@APPTSCHD_CITY", Entity.City));
                sqlParamList.Add(new SqlParameter("@APPTSCHD_STATE", Entity.State));
                if (Entity.Zip1 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@APPTSCHD_ZIP1", Entity.Zip1));
                if (Entity.Zip2 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@APPTSCHD_ZIP2", Entity.Zip2));
                sqlParamList.Add(new SqlParameter("@APPTSCHD_HEAT_SOURCE", Entity.HeatSource));
                sqlParamList.Add(new SqlParameter("@APPTSCHD_SOURCE_INCOME", Entity.SourceIncome));
                sqlParamList.Add(new SqlParameter("@APPTSCHD_CONTACT_PERSON", Entity.ContactPerson));
                sqlParamList.Add(new SqlParameter("@APPTSCHD_CONTACT_DATE", Entity.ContactDate));
                sqlParamList.Add(new SqlParameter("@APPTSCHD_SEX", Entity.Sex));
                sqlParamList.Add(new SqlParameter("@APPTSCHD_CELL_PROVIDER", Entity.CellProvider));
                sqlParamList.Add(new SqlParameter("@APPTSCHD_CELL_NUMBER", Entity.CellNumber));
                sqlParamList.Add(new SqlParameter("@APPTSCHD_CASE_WORKER", Entity.CaseWorker));
                if (Entity.EditTime != string.Empty)
                    sqlParamList.Add(new SqlParameter("@APPTSCHD_EDITIME", Entity.EditTime));
                sqlParamList.Add(new SqlParameter("@APPTSCHD_EDITBY", Entity.EditBy));
                sqlParamList.Add(new SqlParameter("@APPTSCHD_EMAIL", Entity.Email));
                sqlParamList.Add(new SqlParameter("@APPTSCHD_LSTC_OPERATOR", Entity.LstcOperation));
                sqlParamList.Add(new SqlParameter("@APPTSCHD_ADD_OPERATOR", Entity.AddOperator));
                sqlParamList.Add(new SqlParameter("@APPTSCHD_LANGUAGE", Entity.Language));

                if (Entity.Status != string.Empty)
                    sqlParamList.Add(new SqlParameter("@APPTSCHD_STATUS", Entity.Status));
                if (Entity.Client!= string.Empty)
                    sqlParamList.Add(new SqlParameter("@APPTSCHD_CLIENT", Entity.Client));
                if (Entity.Notes != string.Empty)
                    sqlParamList.Add(new SqlParameter("@APPTSCHD_NOTES", Entity.Notes));

                if (Entity.SlotType != string.Empty)
                    sqlParamList.Add(new SqlParameter("@APPTSCHD_SLOT_TYPE", Entity.SlotType));


                boolsuccess = Captain.DatabaseLayer.TMS00110DB.InsertUpdateDelAPPTSCHED(sqlParamList);

            }
            catch (Exception ex)
            {
                //
                return false;
            }

            return boolsuccess;
        }


        public bool InsertUpdateDelAPPTSCHDHIST(APPTSCHDHISTEntity Entity)
        {
            bool boolsuccess;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@Mode", Entity.Mode));
                sqlParamList.Add(new SqlParameter("@APPTSCHDHIST_AGY", Entity.Agency));
                sqlParamList.Add(new SqlParameter("@APPTSCHDHIST_DEPT", Entity.Dept));
                sqlParamList.Add(new SqlParameter("@APPTSCHDHIST_PROG", Entity.Program));
                sqlParamList.Add(new SqlParameter("@APPTSCHDHIST_YEAR", Entity.Year));

                if (Entity.Site != string.Empty)
                    sqlParamList.Add(new SqlParameter("@APPTSCHDHIST_SITE", Entity.Site));
                if (Entity.Date != string.Empty)
                    sqlParamList.Add(new SqlParameter("@APPTSCHDHIST_DATE", Entity.Date));
                if (Entity.Time != string.Empty)
                    sqlParamList.Add(new SqlParameter("@APPTSCHDHIST_TIME", Entity.Time));

                sqlParamList.Add(new SqlParameter("@APPTSCHDHIST_SLOT", Entity.SlotNumber));
                sqlParamList.Add(new SqlParameter("@APPTSCHDHIST_SEQ", Entity.Seq));
                sqlParamList.Add(new SqlParameter("@APPTSCHDHIST_SSN", Entity.SsNumber));
                sqlParamList.Add(new SqlParameter("@APPTSCHDHIST_TEMPL_ID", Entity.TemplateID));

                sqlParamList.Add(new SqlParameter("@APPTSCHDHIST_TYPE", Entity.SchdType));
                sqlParamList.Add(new SqlParameter("@APPTSCHDHIST_LNAME", Entity.LastName));
                sqlParamList.Add(new SqlParameter("@APPTSCHDHIST_FNAME", Entity.FirstName));
                if (Entity.DOB != string.Empty)
                    sqlParamList.Add(new SqlParameter("@APPTSCHDHIST_DOB", Entity.DOB));
                sqlParamList.Add(new SqlParameter("@APPTSCHDHIST_TEL", Entity.TelNumber));
                sqlParamList.Add(new SqlParameter("@APPTSCHDHIST_HNO", Entity.HNo));
                sqlParamList.Add(new SqlParameter("@APPTSCHDHIST_STREET", Entity.Street));
                sqlParamList.Add(new SqlParameter("@APPTSCHDHIST_SUFFIX", Entity.Suffix));
                sqlParamList.Add(new SqlParameter("@APPTSCHDHIST_APT", Entity.Apt));
                sqlParamList.Add(new SqlParameter("@APPTSCHDHIST_FLOOR", Entity.Floor));
                sqlParamList.Add(new SqlParameter("@APPTSCHDHIST_CITY", Entity.City));
                sqlParamList.Add(new SqlParameter("@APPTSCHDHIST_STATE", Entity.State));
                if (Entity.Zip1 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@APPTSCHDHIST_ZIP1", Entity.Zip1));
                if (Entity.Zip2 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@APPTSCHDHIST_ZIP2", Entity.Zip2));
                sqlParamList.Add(new SqlParameter("@APPTSCHDHIST_HEAT_SOURCE", Entity.HeatSource));
                sqlParamList.Add(new SqlParameter("@APPTSCHDHIST_SOURCE_INCOME", Entity.SourceIncome));
                sqlParamList.Add(new SqlParameter("@APPTSCHDHIST_CONTACT_PERSON", Entity.ContactPerson));
                sqlParamList.Add(new SqlParameter("@APPTSCHDHIST_CONTACT_DATE", Entity.ContactDate));
                sqlParamList.Add(new SqlParameter("@APPTSCHDHIST_SEX", Entity.Sex));
                sqlParamList.Add(new SqlParameter("@APPTSCHDHIST_CELL_PROVIDER", Entity.CellProvider));
                sqlParamList.Add(new SqlParameter("@APPTSCHDHIST_CELL_NUMBER", Entity.CellNumber));
                sqlParamList.Add(new SqlParameter("@APPTSCHDHIST_CASE_WORKER", Entity.CaseWorker));
                if (Entity.EditTime != string.Empty)
                    sqlParamList.Add(new SqlParameter("@APPTSCHDHIST_EDITIME", Entity.EditTime));
                sqlParamList.Add(new SqlParameter("@APPTSCHDHIST_EDITBY", Entity.EditBy));
                sqlParamList.Add(new SqlParameter("@APPTSCHDHIST_EMAIL", Entity.Email));
                sqlParamList.Add(new SqlParameter("@APPTSCHDHIST_LSTC_OPERATOR", Entity.LstcOperation));
                sqlParamList.Add(new SqlParameter("@APPTSCHDHIST_ADD_OPERATOR", Entity.AddOperator));

                if (Entity.Status != string.Empty)
                    sqlParamList.Add(new SqlParameter("@APPTSCHDHIST_STATUS", Entity.Status));
                if (Entity.Client != string.Empty)
                    sqlParamList.Add(new SqlParameter("@APPTSCHDHIST_CLIENT", Entity.Client));
                if (Entity.Notes != string.Empty)
                    sqlParamList.Add(new SqlParameter("@APPTSCHDHIST_NOTES", Entity.Notes));


                boolsuccess = Captain.DatabaseLayer.TMS00110DB.InsertUpdateDelAPPTSCHEDHIST(sqlParamList);

            }
            catch (Exception ex)
            {
                //
                return false;
            }

            return boolsuccess;
        }



        public bool Delete_NowSch_TMSAPPT(string Hierarchy, string Location, string date, string User)
        {
            try
            {
                Captain.DatabaseLayer.TMS00110DB.Delete_NowSch_TMSAPPT(Hierarchy, Location, date, User);
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }





        // TMS00120 


        //public List<TmsApptEntity>  Get_Template_APPT_ByDate(string Appt_Key, string From_Date, string TO_Date)
        //{
        //    List<TmsApptEntity> TmsApptProfile = new List<TmsApptEntity>();
        //    try
        //    {
        //        string RowType = " ";
        //        DataSet TmsApcnData = TMS00110DB.Get_Template_APPT_ByDate(Appt_Key, From_Date, TO_Date);
        //        if (TmsApptData != null && TmsApptData.Tables[0].Rows.Count > 0)
        //        {
        //            foreach (DataRow row in TmsApptData.Tables[0].Rows)
        //            {
        //                TmsApptProfile.Add(new TmsApptEntity(row, RowType));
        //            }
        //        }

        //        DataSet TmsApptData = TMS00110DB.Get_Template_APPT_ByDate(Appt_Key, From_Date, TO_Date);
        //        if (TmsApptData != null && TmsApptData.Tables[0].Rows.Count > 0)
        //        {
        //            foreach (DataRow row in TmsApptData.Tables[0].Rows)
        //            {
        //                TmsApptProfile.Add(new TmsApptEntity(row, RowType));
        //            }
        //        }




        //    }
        //    catch (Exception ex) {   return TmsApptProfile;  }

        //    return TmsApptProfile;
        //}


        // TMS00120 END




    }
}
