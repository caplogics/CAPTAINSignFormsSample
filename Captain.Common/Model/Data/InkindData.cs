using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Captain.DatabaseLayer;
using Captain.Common.Model.Objects;
using System.Data.SqlClient;
using System.Data;
using Captain.Common.Utilities;
using NPOI.SS.Formula.Functions;


namespace Captain.Common.Model.Data
{
    public class InkindData
    {

        public InkindData()
        {

        }

        #region Properties

        public CaptainModel Model { get; set; }

        #endregion


        public List<INKINDMSTEntity> Browse_INKINDMST(INKINDMSTEntity Entity, string Opretaion_Mode)
        {
            List<INKINDMSTEntity> CASESPMProfile = new List<INKINDMSTEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_INKINDMST_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_INKINDMST]");
                //DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_STAFFMST(sqlParamList);

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASESPMProfile.Add(new INKINDMSTEntity(row));
                }
            }
            catch (Exception ex)
            { return CASESPMProfile; }

            return CASESPMProfile;
        }

        public List<SqlParameter> Prepare_INKINDMST_SqlParameters_List(INKINDMSTEntity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                if (Opretaion_Mode != "Browse")
                    sqlParamList.Add(new SqlParameter("@Row_Type", Entity.Rec_Type));

                sqlParamList.Add(new SqlParameter("@IKM_AGENCY", Entity.Agency));
                sqlParamList.Add(new SqlParameter("@IKM_DEPT", Entity.Dept));
                sqlParamList.Add(new SqlParameter("@IKM_PROGRAM", Entity.Prog));
                sqlParamList.Add(new SqlParameter("@IKM_CODE", Entity.CODE));
                sqlParamList.Add(new SqlParameter("@IKM_TYPE", Entity.TYPE));


                sqlParamList.Add(new SqlParameter("@IKM_DONOR_NAME", Entity.DONOR_NAME));
                if (!string.IsNullOrEmpty(Entity.CHILD_FNAME))
                    sqlParamList.Add(new SqlParameter("@IKM_CHILD_FNAME", Entity.CHILD_FNAME.Trim()));
                if (!string.IsNullOrEmpty(Entity.CHILD_LNAME))
                    sqlParamList.Add(new SqlParameter("@IKM_CHILD_LNAME", Entity.CHILD_LNAME.Trim()));
                if (!string.IsNullOrEmpty(Entity.ADDRESS))
                    sqlParamList.Add(new SqlParameter("@IKM_ADDRESS", Entity.ADDRESS.Trim()));
                if (!string.IsNullOrEmpty(Entity.PHONE))
                    sqlParamList.Add(new SqlParameter("@IKM_PHONE", Entity.PHONE.Trim()));
                if (!string.IsNullOrEmpty(Entity.DOB))
                    sqlParamList.Add(new SqlParameter("@IKM_DOB", Entity.DOB.Trim()));
                if (!string.IsNullOrEmpty(Entity.SEX))
                    sqlParamList.Add(new SqlParameter("@IKM_SEX", Entity.SEX.Trim()));
                if (!string.IsNullOrEmpty(Entity.RACE))
                    sqlParamList.Add(new SqlParameter("@IKM_RACE", Entity.RACE.Trim()));
                if (!string.IsNullOrEmpty(Entity.ETHNICITY))
                    sqlParamList.Add(new SqlParameter("@IKM_ETHNICITY", Entity.ETHNICITY.Trim()));
                if (!string.IsNullOrEmpty(Entity.EDUCATION))
                    sqlParamList.Add(new SqlParameter("@IKM_EDUCATION", Entity.EDUCATION.Trim()));
                if (!string.IsNullOrEmpty(Entity.VETERAN))
                    sqlParamList.Add(new SqlParameter("@IKM_VETERAN", Entity.VETERAN.Trim()));
                if (!string.IsNullOrEmpty(Entity.DISABLED))
                    sqlParamList.Add(new SqlParameter("@IKM_DISABLED", Entity.DISABLED.Trim()));
                if (!string.IsNullOrEmpty(Entity.FARMER))
                    sqlParamList.Add(new SqlParameter("@IKM_FARMER", Entity.FARMER.Trim()));
                if (!string.IsNullOrEmpty(Entity.US_CITIZEN))
                    sqlParamList.Add(new SqlParameter("@IKM_US_CITIZEN", Entity.US_CITIZEN.Trim()));
                if (!string.IsNullOrEmpty(Entity.HS_PARENT))
                    sqlParamList.Add(new SqlParameter("@IKM_HS_PARENT", Entity.HS_PARENT.Trim()));

                sqlParamList.Add(new SqlParameter("@IKM_LSTC_OPERATOR", Entity.LSTC_OPERATOR));


                if (Opretaion_Mode == "Browse")
                {
                    sqlParamList.Add(new SqlParameter("@IKM_DATE_ADD", Entity.DATE_ADD));
                    sqlParamList.Add(new SqlParameter("@IKM_ADD_OPERATOR", Entity.ADD_OPERATOR));
                    sqlParamList.Add(new SqlParameter("@IKM_DATE_LSTC", Entity.DATE_LSTC));
                }
            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }


        public List<INKINDDTLEntity> Browse_INKINDDTL(INKINDDTLEntity Entity, string Opretaion_Mode)
        {
            List<INKINDDTLEntity> CASESPMProfile = new List<INKINDDTLEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_INKINDDTL_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_INKINDDTL]");
                //DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_STAFFMST(sqlParamList);

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASESPMProfile.Add(new INKINDDTLEntity(row));
                }
            }
            catch (Exception ex)
            { return CASESPMProfile; }

            return CASESPMProfile;
        }

        public List<SqlParameter> Prepare_INKINDDTL_SqlParameters_List(INKINDDTLEntity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                if (Opretaion_Mode != "Browse")
                    sqlParamList.Add(new SqlParameter("@Row_Type", Entity.Rec_Type));

                sqlParamList.Add(new SqlParameter("@IKD_AGENCY", Entity.Agency));
                sqlParamList.Add(new SqlParameter("@IKD_DEPT", Entity.Dept));
                sqlParamList.Add(new SqlParameter("@IKD_PROGRAM", Entity.Prog));
                sqlParamList.Add(new SqlParameter("@IKD_CODE", Entity.CODE));
                sqlParamList.Add(new SqlParameter("@IKD_SEQ", Entity.SEQ));


                sqlParamList.Add(new SqlParameter("@IKD_SERVICE_TYPE", Entity.SERVICE_TYPE));
                sqlParamList.Add(new SqlParameter("@IKD_SERVICE_DATE", Entity.SERVICE_DATE));
                sqlParamList.Add(new SqlParameter("@IKD_START_TIME", Entity.START_TIME));
                sqlParamList.Add(new SqlParameter("@IKD_END_TIME", Entity.END_TIME));
                if (!string.IsNullOrEmpty(Entity.SERVICE_TIME))
                    sqlParamList.Add(new SqlParameter("@IKD_SERVICE_TIME", Entity.SERVICE_TIME));
                if (!string.IsNullOrEmpty(Entity.TRANSPORT_TIME))
                    sqlParamList.Add(new SqlParameter("@IKD_TRANSPORT_TIME", Entity.TRANSPORT_TIME));
                if (!string.IsNullOrEmpty(Entity.MILES_DRIVEN))
                    sqlParamList.Add(new SqlParameter("@IKD_MILES_DRIVEN", Entity.MILES_DRIVEN));
                if (!string.IsNullOrEmpty(Entity.TOTAL_SERVICE))
                    sqlParamList.Add(new SqlParameter("@IKD_TOTAL_SERVICE", Entity.TOTAL_SERVICE));
                if (!string.IsNullOrEmpty(Entity.TOTAL_MILEAGE))
                    sqlParamList.Add(new SqlParameter("@IKD_TOTAL_MILEAGE", Entity.TOTAL_MILEAGE));
                if (!string.IsNullOrEmpty(Entity.TOTAL_INKIND))
                    sqlParamList.Add(new SqlParameter("@IKD_TOTAL_INKIND", Entity.TOTAL_INKIND));

                if (!string.IsNullOrEmpty(Entity.Site))
                    sqlParamList.Add(new SqlParameter("@IKD_SITE", Entity.Site));

                sqlParamList.Add(new SqlParameter("@IKD_LSTC_OPERATOR", Entity.LSTC_Operator));


                if (Opretaion_Mode == "Browse")
                {
                    sqlParamList.Add(new SqlParameter("@IKD_DATE_ADD", Entity.Date_ADD));
                    sqlParamList.Add(new SqlParameter("@IKD_ADD_OPERATOR", Entity.ADD_Operator));
                    sqlParamList.Add(new SqlParameter("@IKD_DATE_LSTC", Entity.Date_LSTC));
                }
            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }

        public bool UpdateINKINDMST(INKINDMSTEntity Entity, string Operation_Mode, out string Msg, out string Sql_Reslut_Msg)
        {
            bool boolsuccess = true;
            Sql_Reslut_Msg = "Success";
            Msg = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList = Prepare_INKINDMST_SqlParameters_List(Entity, Operation_Mode);

                SqlParameter DeleteMsg = new SqlParameter("@msg", SqlDbType.VarChar, 50);
                DeleteMsg.Direction = ParameterDirection.Output;
                sqlParamList.Add(DeleteMsg);

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.Update_INKINDMST", out Sql_Reslut_Msg);  //

                Msg = DeleteMsg.Value.ToString();
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        public bool UpdateINKINDDET(INKINDDTLEntity Entity, string Operation_Mode, out string Msg, out string Sql_Reslut_Msg)
        {
            bool boolsuccess = true;
            Sql_Reslut_Msg = "Success";
            Msg = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList = Prepare_INKINDDTL_SqlParameters_List(Entity, Operation_Mode);

                SqlParameter DeleteMsg = new SqlParameter("@msg", SqlDbType.VarChar, 50);
                DeleteMsg.Direction = ParameterDirection.Output;
                sqlParamList.Add(DeleteMsg);

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.Update_Sel_Table(sqlParamList, "dbo.Update_INKINDDTL", out Sql_Reslut_Msg);  //

                Msg = DeleteMsg.Value.ToString();
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }

        public List<STAFFPostEntity> GetStaffPostDetails(string agency, string dep, string program, string code, string category, string seq)
        {
            List<STAFFPostEntity> StaffPostDetails = new List<STAFFPostEntity>();
            try
            {
                DataSet SitePost = Captain.DatabaseLayer.CaseMst.GetStaffPostAllDetails(agency, dep, program, code, category, seq);
                if (SitePost != null && SitePost.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in SitePost.Tables[0].Rows)
                    {
                        StaffPostDetails.Add(new STAFFPostEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return StaffPostDetails;
            }

            return StaffPostDetails;
        }

        public List<SqlParameter> Insupdel_INKINDDTLSEntyPrep(INKINDDTLSEntity Entity, string Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                //if (Opretaion_Mode != "Browse")
                //    sqlParamList.Add(new SqlParameter("@Row_Type", Entity.Rec_Type));

                sqlParamList.Add(new SqlParameter("@IKDL_AGENCY", Entity.IKDL_AGENCY));
                sqlParamList.Add(new SqlParameter("@IKDL_DEPT", Entity.IKDL_DEPT));
                sqlParamList.Add(new SqlParameter("@IKDL_PROGRAM", Entity.IKDL_PROGRAM));
                sqlParamList.Add(new SqlParameter("@IKDL_CODE", Entity.IKDL_CODE));
                sqlParamList.Add(new SqlParameter("@IKDL_SEQ", Entity.IKDL_SEQ));


                sqlParamList.Add(new SqlParameter("@IKDL_SERVICE_TYPE", Entity.IKDL_SERVICE_TYPE));
                sqlParamList.Add(new SqlParameter("@IKDL_SERVICE_AREA", Entity.IKDL_SERVICE_AREA));
                sqlParamList.Add(new SqlParameter("@IKDL_SERVICE_DATE", Entity.IKDL_SERVICE_DATE));
                sqlParamList.Add(new SqlParameter("@IKDL_PROG", Entity.IKDL_PROG));

                if (!string.IsNullOrEmpty(Entity.IKDL_SITE))
                    sqlParamList.Add(new SqlParameter("@IKDL_SITE", Entity.IKDL_SITE));

                if (!string.IsNullOrEmpty(Entity.IKDL_ROOM))
                    sqlParamList.Add(new SqlParameter("@IKDL_ROOM", Entity.IKDL_ROOM));

                if (!string.IsNullOrEmpty(Entity.IKDL_AMPM))
                    sqlParamList.Add(new SqlParameter("@IKDL_AMPM", Entity.IKDL_AMPM));

                if (!string.IsNullOrEmpty(Entity.IKDL_FATHER))
                    sqlParamList.Add(new SqlParameter("@IKDL_FATHER", Entity.IKDL_FATHER));

                if (!string.IsNullOrEmpty(Entity.IKDL_CLASS_ROOM))
                    sqlParamList.Add(new SqlParameter("@IKDL_CLASS_ROOM", Entity.IKDL_CLASS_ROOM));

                if (!string.IsNullOrEmpty(Entity.IKDL_QUANTITY))
                    sqlParamList.Add(new SqlParameter("@IKDL_QUANTITY", Entity.IKDL_QUANTITY));

                if (!string.IsNullOrEmpty(Entity.IKDL_RATE))
                    sqlParamList.Add(new SqlParameter("@IKDL_RATE", Entity.IKDL_RATE));


                //if (!string.IsNullOrEmpty(Entity.IKDL_UOM))
                    sqlParamList.Add(new SqlParameter("@IKDL_UOM", Entity.IKDL_UOM));

                if (!string.IsNullOrEmpty(Entity.IKDL_AMOUNT))
                    sqlParamList.Add(new SqlParameter("@IKDL_AMOUNT", Entity.IKDL_AMOUNT));

                sqlParamList.Add(new SqlParameter("@IKDL_OPERATOR", Entity.IKDL_LSTC_OPERATOR));

                sqlParamList.Add(new SqlParameter("@MODE", Mode));

            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }
        public bool Insupdel_INKINDDTLS(INKINDDTLSEntity Entity, string Operation_Mode, out string Msg) //, out string Sql_Reslut_Msg
        {
            bool boolsuccess = true;
            // Sql_Reslut_Msg = "Success";
             Msg = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList = Insupdel_INKINDDTLSEntyPrep(Entity, Operation_Mode);

                SqlParameter DeleteMsg = new SqlParameter("@msg", SqlDbType.VarChar, 50);
                DeleteMsg.Direction = ParameterDirection.Output;
                sqlParamList.Add(DeleteMsg);

                boolsuccess = Captain.DatabaseLayer.SPAdminDB.InsUpdelINKINDDTLS(sqlParamList);  // Update_Sel_Table

                 Msg = DeleteMsg.Value.ToString();
            }
            catch (Exception ex)
            { return false; }

            return boolsuccess;
        }


        public List<INKINDDTLSEntity> Get_INKINDTLS(INKINDDTLSEntity Entity, string Opretaion_Mode)
        {
            List<INKINDDTLSEntity> CASESPMProfile = new List<INKINDDTLSEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                //sqlParamList = Insupdel_INKINDDTLSEntyPrep(Entity, Opretaion_Mode);


                if (Entity.IKDL_AGENCY != string.Empty)
                {
                    SqlParameter sqlp1 = new SqlParameter("@IKDL_AGENCY", Entity.IKDL_AGENCY);
                    sqlParamList.Add(sqlp1);
                   // dbCmd.Parameters.Add(sqlp1);
                }
                if (Entity.IKDL_CODE != string.Empty)
                {
                    SqlParameter sqlp2 = new SqlParameter("@IKDL_CODE", Entity.IKDL_CODE);
                    sqlParamList.Add(sqlp2);
                }
                if (Entity.IKDL_SEQ != string.Empty)
                {
                    SqlParameter sqlp3 = new SqlParameter("@IKDL_SEQ", Entity.IKDL_SEQ);
                    sqlParamList.Add(sqlp3);
                }
                if (Opretaion_Mode != string.Empty)
                {
                    SqlParameter sqlp5 = new SqlParameter("@MODE ", Opretaion_Mode);
                    sqlParamList.Add(sqlp5);
                }


                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.GetINKINDTLS(sqlParamList, "[dbo].[GET_INKINDDTLS]");
                //DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_STAFFMST(sqlParamList);

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASESPMProfile.Add(new INKINDDTLSEntity(row));
                }
            }
            catch (Exception ex)
            { return CASESPMProfile; }

            return CASESPMProfile;
        }




    }
}
