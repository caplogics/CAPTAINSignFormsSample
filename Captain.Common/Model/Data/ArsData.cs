using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Captain.Common.Model.Objects;
using System.Data;
using System.Data.SqlClient;
using Captain.DatabaseLayer;

namespace Captain.Common.Model.Data
{
    public class ArsData
    {

        public ArsData(CaptainModel model)
        {
            Model = model;
        }

        #region Properties

        public CaptainModel Model { get; set; }

        public string UserId { get; set; }

        #endregion

        public List<ARSMASTEntity> Get_ARSMAST(string Agency, string Dept, string Prog, string Appno, string Type, string Source, string Invoice)
        {
            List<ARSMASTEntity> arsMastList = new List<ARSMASTEntity>();
            try
            {
                DataSet arsData = Captain.DatabaseLayer.ARSDB.Get_ARSMAST(Agency, Dept, Prog, Appno, Type, Source, Invoice);
                if (arsData != null && arsData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in arsData.Tables[0].Rows)
                    {
                        arsMastList.Add(new ARSMASTEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return arsMastList;
            }

            return arsMastList;
        }

        public List<ARSCUSTEntity> Browse_ARSCUST(string Agency, string Dept, string Prog, string Appno, string Type)
        {
            List<ARSCUSTEntity> arsMastList = new List<ARSCUSTEntity>();
            try
            {
                DataSet arsData = Captain.DatabaseLayer.ARSDB.Browse_ARSCUST(Agency, Dept, Prog, Appno, Type);
                if (arsData != null && arsData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in arsData.Tables[0].Rows)
                    {
                        arsMastList.Add(new ARSCUSTEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return arsMastList;
            }

            return arsMastList;
        }

        public List<ARSCUSTEntity> Get_ARSCUST(string Agency, string Dept, string Prog, string Appno, string Type, string strTable)
        {
            List<ARSCUSTEntity> arsMastList = new List<ARSCUSTEntity>();
            try
            {
                DataSet arsData = Captain.DatabaseLayer.ARSDB.Get_ARSCUST(Agency, Dept, Prog, Appno, Type);
                if (arsData != null && arsData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in arsData.Tables[0].Rows)
                    {
                        arsMastList.Add(new ARSCUSTEntity(row, strTable));
                    }
                }
            }
            catch (Exception ex)
            {
                return arsMastList;
            }

            return arsMastList;
        }


        public List<ARSCUSTEntity> Browse_ARSCUST(ARSCUSTEntity Entity, string Opretaion_Mode)
        {
            List<ARSCUSTEntity> CASESPMProfile = new List<ARSCUSTEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_ARSCUST_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_ARSCUST]");
                //DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_CASECONT(sqlParamList);

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASESPMProfile.Add(new ARSCUSTEntity(row));
                }
            }
            catch (Exception ex)
            { return CASESPMProfile; }

            return CASESPMProfile;
        }

        public List<SqlParameter> Prepare_ARSCUST_SqlParameters_List(ARSCUSTEntity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                if (Opretaion_Mode != "Browse")
                    sqlParamList.Add(new SqlParameter("@Row_Type", Entity.Rec_Type));

                sqlParamList.Add(new SqlParameter("@ARS_AGENCY", Entity.ARS_AGENCY));
                sqlParamList.Add(new SqlParameter("@ARS_DEPT", Entity.ARS_DEPT));
                sqlParamList.Add(new SqlParameter("@ARS_PROGRAM", Entity.ARS_PROGRAM));
                sqlParamList.Add(new SqlParameter("@ARS_APP_NO", Entity.ARS_APP_NO));
                sqlParamList.Add(new SqlParameter("@ARS_CUST_TYPE", Entity.ARS_CUST_TYPE));
                sqlParamList.Add(new SqlParameter("@ARS_NAME", Entity.ARS_NAME));
                sqlParamList.Add(new SqlParameter("@ARS_INDEXBY", Entity.ARS_INDEXBY));
                sqlParamList.Add(new SqlParameter("@ARS_NAME_2", Entity.ARS_NAME_2));
                sqlParamList.Add(new SqlParameter("@ARS_INDEXBY2", Entity.ARS_INDEXBY2));
                sqlParamList.Add(new SqlParameter("@ARS_STREET", Entity.ARS_STREET));
                sqlParamList.Add(new SqlParameter("@ARS_CITY", Entity.ARS_CITY));
                sqlParamList.Add(new SqlParameter("@ARS_STATE", Entity.ARS_STATE));
                sqlParamList.Add(new SqlParameter("@ARS_ZIP", Entity.ARS_ZIP));
                sqlParamList.Add(new SqlParameter("@ARS_ZIP_PLUS4", Entity.ARS_ZIP_PLUS4));
                sqlParamList.Add(new SqlParameter("@ARS_TELEPHONE", Entity.ARS_TELEPHONE));
                sqlParamList.Add(new SqlParameter("@ARS_ACTIVE", Entity.ARS_ACTIVE));
                sqlParamList.Add(new SqlParameter("@ARS_C_NAME", Entity.ARS_C_NAME));
                sqlParamList.Add(new SqlParameter("@ARS_C_TELEPHONE", Entity.ARS_C_TELEPHONE));
                sqlParamList.Add(new SqlParameter("@ARS_FREQ", Entity.ARS_FREQ));
                sqlParamList.Add(new SqlParameter("@ARS_TERMS", Entity.ARS_TERMS));
                sqlParamList.Add(new SqlParameter("@ARS_BILLTO", Entity.ARS_BILLTO));
                sqlParamList.Add(new SqlParameter("@ARS_CONTRACT", Entity.ARS_CONTRACT));
                sqlParamList.Add(new SqlParameter("@ARS_LOCATION", Entity.ARS_LOCATION));
                sqlParamList.Add(new SqlParameter("@ARS_PROGRAM_ACCOUNT", Entity.ARS_PROGRAM_ACCOUNT));
                sqlParamList.Add(new SqlParameter("@ARS_EXPENSE_CODE", Entity.ARS_EXPENSE_CODE));
                sqlParamList.Add(new SqlParameter("@ARS_CLASS_IDENTIFIER", Entity.ARS_CLASS_IDENTIFIER));
                sqlParamList.Add(new SqlParameter("@ARS_PRINT_INV", Entity.ARS_PRINT_INV));
                sqlParamList.Add(new SqlParameter("@ARS_PRINT_STMT", Entity.ARS_PRINT_STMT));
                sqlParamList.Add(new SqlParameter("@ARS_LSTC_OPERATOR", Entity.ARS_LSTC_OPERATOR));


                if (Opretaion_Mode == "Browse")
                {
                    sqlParamList.Add(new SqlParameter("@ARS_DATE_LSTC", Entity.ARS_DATE_LSTC));
                    sqlParamList.Add(new SqlParameter("@ARS_DATE_ADD", Entity.ARS_DATE_ADD));
                    sqlParamList.Add(new SqlParameter("@ARS_ADD_OPERATOR", Entity.ARS_ADD_OPERATOR));
                }
            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }

        public List<SqlParameter> Prepare_ARSMAST_SqlParameters_List(ARSMASTEntity Entity, string Opretaion_Mode)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                if (Opretaion_Mode != "Browse")
                    sqlParamList.Add(new SqlParameter("@Row_Type", Entity.Rec_Type));

                sqlParamList.Add(new SqlParameter("@ARM_AGENCY", Entity.ARM_AGENCY));
                sqlParamList.Add(new SqlParameter("@ARM_DEPT", Entity.ARM_DEPT));
                sqlParamList.Add(new SqlParameter("@ARM_PROGRAM", Entity.ARM_PROGRAM));
                sqlParamList.Add(new SqlParameter("@ARM_APP_NO", Entity.ARM_APP_NO));
                sqlParamList.Add(new SqlParameter("@ARM_CUST_TYPE", Entity.ARM_CUST_TYPE));
                sqlParamList.Add(new SqlParameter("@ARM_SOURCE", Entity.ARM_SOURCE));
                sqlParamList.Add(new SqlParameter("@ARM_INVOICE", Entity.ARM_INVOICE));
                sqlParamList.Add(new SqlParameter("@ARM_STATUS", Entity.ARM_STATUS));
                sqlParamList.Add(new SqlParameter("@ARM_TYPE", Entity.ARM_TYPE));
                sqlParamList.Add(new SqlParameter("@ARM_INVOICE_DATE", Entity.ARM_INVOICE_DATE));
                sqlParamList.Add(new SqlParameter("@ARM_DUE_DATE", Entity.ARM_DUE_DATE));
                sqlParamList.Add(new SqlParameter("@ARM_COMMENTS", Entity.ARM_COMMENTS));
                sqlParamList.Add(new SqlParameter("@ARM_AUTO", Entity.ARM_AUTO));
                sqlParamList.Add(new SqlParameter("@ARM_BILLED", Entity.ARM_BILLED));
                sqlParamList.Add(new SqlParameter("@ARM_RECEIPT", Entity.ARM_RECEIPT));
                sqlParamList.Add(new SqlParameter("@ARM_ADJUSTMENTS", Entity.ARM_ADJUSTMENTS));
                sqlParamList.Add(new SqlParameter("@ARM_DISC_TAKEN", Entity.ARM_DISC_TAKEN));
                sqlParamList.Add(new SqlParameter("@ARM_BALANCE", Entity.ARM_BALANCE));
                sqlParamList.Add(new SqlParameter("@ARM_SITE", Entity.ARM_SITE));
                sqlParamList.Add(new SqlParameter("@ARM_ROOM", Entity.ARM_ROOM));
                sqlParamList.Add(new SqlParameter("@ARM_AMPM", Entity.ARM_AMPM));
                sqlParamList.Add(new SqlParameter("@ARM_FUNDING_SOURCE", Entity.ARM_FUNDING_SOURCE));
                sqlParamList.Add(new SqlParameter("@ARM_CLOSED_DATE", Entity.ARM_CLOSED_DATE));
                sqlParamList.Add(new SqlParameter("@ARM_LSTC_OPERATOR", Entity.ARM_LSTC_OPERATOR));
                sqlParamList.Add(new SqlParameter("@ARM_INVF_DATE", Entity.ARM_INVF_DATE));
                sqlParamList.Add(new SqlParameter("@ARM_INVT_DATE", Entity.ARM_INVT_DATE));


                if (Opretaion_Mode == "Browse")
                {
                    sqlParamList.Add(new SqlParameter("@ARM_DATE_LSTC", Entity.ARM_DATE_LSTC));
                    sqlParamList.Add(new SqlParameter("@ARM_DATE_ADD", Entity.ARM_DATE_ADD));
                    sqlParamList.Add(new SqlParameter("@ARM_ADD_OPERATOR", Entity.ARM_ADD_OPERATOR));
                }
            }
            catch (Exception ex)
            { return sqlParamList; }

            return sqlParamList;
        }

        public List<ARSMASTEntity> Browse_ARSMAST(ARSMASTEntity Entity, string Opretaion_Mode)
        {
            List<ARSMASTEntity> CASESPMProfile = new List<ARSMASTEntity>();
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            try
            {
                sqlParamList = Prepare_ARSMAST_SqlParameters_List(Entity, Opretaion_Mode);
                DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Browse_ARSMAST]");
                //DataSet CASESPMData = Captain.DatabaseLayer.SPAdminDB.Browse_CASECONT(sqlParamList);

                if (CASESPMData != null && CASESPMData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CASESPMData.Tables[0].Rows)
                        CASESPMProfile.Add(new ARSMASTEntity(row));
                }
            }
            catch (Exception ex)
            { return CASESPMProfile; }

            return CASESPMProfile;
        }

        public List<ARSMASTEntity> Get_ARSMASTCHECK(string Agency, string Dept, string Prog, string Appno, string Type)
        {
            List<ARSMASTEntity> artMastList = new List<ARSMASTEntity>();
            try
            {
                DataSet arsData = Captain.DatabaseLayer.ARSDB.ARSMASTCHECK(Agency, Dept, Prog, Appno, Type);
                if (arsData != null && arsData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in arsData.Tables[0].Rows)
                    {
                        artMastList.Add(new ARSMASTEntity(row,string.Empty));
                    }
                }
            }
            catch (Exception ex)
            {
                return artMastList;
            }

            return artMastList;
        }

        public List<ARSMASTEntity> Get_ARS00125(string Agency, string Dept, string Prog, string Appno, string Type)
        {
            List<ARSMASTEntity> artMastList = new List<ARSMASTEntity>();
            try
            {
                DataSet arsData = Captain.DatabaseLayer.ARSDB.GetARS00125(Agency, Dept, Prog, Appno, Type);
                if (arsData != null && arsData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in arsData.Tables[0].Rows)
                    {
                        artMastList.Add(new ARSMASTEntity(row, "ARS00125"));
                    }
                }
            }
            catch (Exception ex)
            {
                return artMastList;
            }

            return artMastList;
        }

        public List<ARSTRANEntity> Get_ARSTRAN(string Agency, string Dept, string Prog, string Appno, string Type, string Source, string Invoice, string Entry, string Seq)
        {
            List<ARSTRANEntity> artMastList = new List<ARSTRANEntity>();
            try
            {
                DataSet arsData = Captain.DatabaseLayer.ARSDB.Get_ARSTRAN(Agency, Dept, Prog, Appno, Type, Source, Invoice, Entry, Seq);
                if (arsData != null && arsData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in arsData.Tables[0].Rows)
                    {
                        artMastList.Add(new ARSTRANEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return artMastList;
            }

            return artMastList;
        }

        public bool InsertUpdateDelARSCUST(ARSCUSTEntity ArscustEntity)
        {
            bool boolStatus = false;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@ARS_AGENCY", ArscustEntity.ARS_AGENCY));
                sqlParamList.Add(new SqlParameter("@ARS_DEPT", ArscustEntity.ARS_DEPT));
                sqlParamList.Add(new SqlParameter("@ARS_PROGRAM", ArscustEntity.ARS_PROGRAM));

                sqlParamList.Add(new SqlParameter("@ARS_APP_NO", ArscustEntity.ARS_APP_NO));

                if (ArscustEntity.ARS_CUST_TYPE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARS_CUST_TYPE", ArscustEntity.ARS_CUST_TYPE));


                //if (ArscustEntity.ARS_NAME != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@ARS_NAME", ArscustEntity.ARS_NAME));
                if (ArscustEntity.ARS_INDEXBY != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARS_INDEXBY", ArscustEntity.ARS_INDEXBY));
                if (ArscustEntity.ARS_NAME != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARS_NAME", ArscustEntity.ARS_NAME));

                if (ArscustEntity.ARS_INDEXBY2 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@ARS_INDEXBY2", ArscustEntity.ARS_INDEXBY2));
                }
                //if (ArscustEntity.ARS_STREET != string.Empty)
                //{
                //    sqlParamList.Add(new SqlParameter("@ARS_STREET", ArscustEntity.ARS_STREET));
                //}

                //if (ArscustEntity.ARS_CITY != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@ARS_CITY", ArscustEntity.ARS_CITY));
                //if (ArscustEntity.ARS_STATE != string.Empty)
                //{
                //    sqlParamList.Add(new SqlParameter("@ARS_STATE", ArscustEntity.ARS_STATE));
                //}

                //if (ArscustEntity.ARS_ZIP != string.Empty)
                //{
                //    sqlParamList.Add(new SqlParameter("@ARS_ZIP", ArscustEntity.ARS_ZIP));
                //}

                //if (ArscustEntity.ARS_ZIP_PLUS4 != string.Empty)
                //{
                //    sqlParamList.Add(new SqlParameter("@ARS_ZIP_PLUS4", ArscustEntity.ARS_ZIP_PLUS4));
                //}

                //if (ArscustEntity.ARS_TELEPHONE != string.Empty)
                //{
                //    sqlParamList.Add(new SqlParameter("@ARS_TELEPHONE", ArscustEntity.ARS_TELEPHONE));
                //}

                if (ArscustEntity.ARS_ACTIVE != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@ARS_ACTIVE", ArscustEntity.ARS_ACTIVE));
                }

                //if (ArscustEntity.ARS_C_NAME != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@ARS_C_NAME", ArscustEntity.ARS_C_NAME));


                //if (ArscustEntity.ARS_C_TELEPHONE != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@ARS_C_TELEPHONE", ArscustEntity.ARS_C_TELEPHONE));


                //if (ArscustEntity.ARS_FREQ != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@ARS_FREQ", ArscustEntity.ARS_FREQ));

                if (ArscustEntity.ARS_TERMS != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARS_TERMS", ArscustEntity.ARS_TERMS));


                if (ArscustEntity.ARS_BILLTO != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARS_BILLTO", ArscustEntity.ARS_BILLTO));

                if (ArscustEntity.ARS_CONTRACT != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARS_CONTRACT", ArscustEntity.ARS_CONTRACT));
                if (ArscustEntity.ARS_LOCATION != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@ARS_LOCATION", ArscustEntity.ARS_LOCATION));
                }

                if (ArscustEntity.ARS_PROGRAM_ACCOUNT != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARS_PROGRAM_ACCOUNT", ArscustEntity.ARS_PROGRAM_ACCOUNT));

                if (ArscustEntity.ARS_EXPENSE_CODE != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@ARS_EXPENSE_CODE", ArscustEntity.ARS_EXPENSE_CODE));
                }

                if (ArscustEntity.ARS_CLASS_IDENTIFIER != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@ARS_CLASS_IDENTIFIER", ArscustEntity.ARS_CLASS_IDENTIFIER));
                }

                if (ArscustEntity.ARS_PRINT_INV != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@ARS_PRINT_INV", ArscustEntity.ARS_PRINT_INV));
                }
                if (ArscustEntity.ARS_PRINT_STMT != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARS_PRINT_STMT", ArscustEntity.ARS_PRINT_STMT));

                if (ArscustEntity.ARS_ADD_OPERATOR != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@ARS_ADD_OPERATOR", ArscustEntity.ARS_ADD_OPERATOR));
                }
                if (ArscustEntity.ARS_LSTC_OPERATOR != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARS_LSTC_OPERATOR", ArscustEntity.ARS_LSTC_OPERATOR));

                if (ArscustEntity.Mode != string.Empty)
                    sqlParamList.Add(new SqlParameter("@Mode", ArscustEntity.Mode));

                boolStatus = ARSDB.InsertUpdateDelARSCUST(sqlParamList);
            }

            catch (Exception ex)
            {
                boolStatus = false;
            }

            return boolStatus;
        }

        public bool InsertUpdateDelARSMAST(ARSMASTEntity ArsMastEntity,out string Msg)
        {
            bool boolStatus = false;
            string strMsg = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                if (ArsMastEntity.ARM_AGENCY != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARM_AGENCY", ArsMastEntity.ARM_AGENCY));
                if (ArsMastEntity.ARM_DEPT != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARM_DEPT", ArsMastEntity.ARM_DEPT));
                if (ArsMastEntity.ARM_PROGRAM != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARM_PROGRAM", ArsMastEntity.ARM_PROGRAM));
                if (ArsMastEntity.ARM_APP_NO != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARM_APP_NO", ArsMastEntity.ARM_APP_NO));
                if (ArsMastEntity.ARM_CUST_TYPE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARM_CUST_TYPE", ArsMastEntity.ARM_CUST_TYPE));
                if (ArsMastEntity.ARM_SOURCE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARM_SOURCE", ArsMastEntity.ARM_SOURCE));
                if (ArsMastEntity.ARM_INVOICE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARM_INVOICE", ArsMastEntity.ARM_INVOICE));
                if (ArsMastEntity.ARM_STATUS != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARM_STATUS", ArsMastEntity.ARM_STATUS));
                if (ArsMastEntity.ARM_TYPE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARM_TYPE", ArsMastEntity.ARM_TYPE));
                if (ArsMastEntity.ARM_INVOICE_DATE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARM_INVOICE_DATE", ArsMastEntity.ARM_INVOICE_DATE));
                if (ArsMastEntity.ARM_DUE_DATE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARM_DUE_DATE", ArsMastEntity.ARM_DUE_DATE));
                if (ArsMastEntity.ARM_COMMENTS != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARM_COMMENTS", ArsMastEntity.ARM_COMMENTS));
                if (ArsMastEntity.ARM_AUTO != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARM_AUTO", ArsMastEntity.ARM_AUTO));
                if (ArsMastEntity.ARM_BILLED != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARM_BILLED", ArsMastEntity.ARM_BILLED));
                if (ArsMastEntity.ARM_RECEIPT != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARM_RECEIPT", ArsMastEntity.ARM_RECEIPT));
                if (ArsMastEntity.ARM_ADJUSTMENTS != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARM_ADJUSTMENTS", ArsMastEntity.ARM_ADJUSTMENTS));
                if (ArsMastEntity.ARM_DISC_TAKEN != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARM_DISC_TAKEN", ArsMastEntity.ARM_DISC_TAKEN));
                if (ArsMastEntity.ARM_BALANCE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARM_BALANCE", ArsMastEntity.ARM_BALANCE));
                if (ArsMastEntity.ARM_SITE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARM_SITE", ArsMastEntity.ARM_SITE));
                if (ArsMastEntity.ARM_ROOM != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARM_ROOM", ArsMastEntity.ARM_ROOM));
                if (ArsMastEntity.ARM_AMPM != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARM_AMPM", ArsMastEntity.ARM_AMPM));
                if (ArsMastEntity.ARM_FUNDING_SOURCE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARM_FUNDING_SOURCE", ArsMastEntity.ARM_FUNDING_SOURCE));
                if (ArsMastEntity.ARM_CLOSED_DATE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARM_CLOSED_DATE", ArsMastEntity.ARM_CLOSED_DATE));

                if (ArsMastEntity.ARM_LSTC_OPERATOR != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARM_LSTC_OPERATOR", ArsMastEntity.ARM_LSTC_OPERATOR));
                if (ArsMastEntity.ARM_ADD_OPERATOR != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARM_ADD_OPERATOR", ArsMastEntity.ARM_ADD_OPERATOR));

                if (ArsMastEntity.ARM_INVF_DATE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARM_INVF_DATE", ArsMastEntity.ARM_INVF_DATE));

                if (ArsMastEntity.ARM_INVT_DATE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARM_INVT_DATE", ArsMastEntity.ARM_INVT_DATE));

                sqlParamList.Add(new SqlParameter("@Mode", ArsMastEntity.Mode));
                SqlParameter parameterMsg = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
                parameterMsg.Direction = ParameterDirection.Output;
                sqlParamList.Add(parameterMsg);

                boolStatus =  ARSDB.InsertUpdateDelARSMAST(sqlParamList);
                Msg = parameterMsg.Value.ToString();

            }
            catch (Exception ex)
            {
                Msg = string.Empty;
                return false;               
            }

            return boolStatus;
        }


        public bool InsertUpdateDeleteARSTRAN(ARSTRANEntity ArsTranEntity)
        {
            bool boolStatus = false;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                if (ArsTranEntity.art_agency != string.Empty)
                    sqlParamList.Add(new SqlParameter("@art_agency", ArsTranEntity.art_agency));
                if (ArsTranEntity.art_dept != string.Empty)
                    sqlParamList.Add(new SqlParameter("@art_dept", ArsTranEntity.art_dept));
                if (ArsTranEntity.art_program != string.Empty)
                    sqlParamList.Add(new SqlParameter("@art_program", ArsTranEntity.art_program));
                if (ArsTranEntity.art_app_no != string.Empty)
                    sqlParamList.Add(new SqlParameter("@art_app_no", ArsTranEntity.art_app_no));
                if (ArsTranEntity.art_Cust_type != string.Empty)
                    sqlParamList.Add(new SqlParameter("@art_Cust_type", ArsTranEntity.art_Cust_type));
                if (ArsTranEntity.art_source != string.Empty)
                    sqlParamList.Add(new SqlParameter("@art_source", ArsTranEntity.art_source));
                if (ArsTranEntity.art_invoice != string.Empty)
                    sqlParamList.Add(new SqlParameter("@art_invoice", ArsTranEntity.art_invoice));
                if (ArsTranEntity.art_Item != string.Empty)
                    sqlParamList.Add(new SqlParameter("@art_Item", ArsTranEntity.art_Item));

                if (ArsTranEntity.art_seq != string.Empty)
                    sqlParamList.Add(new SqlParameter("@art_seq", ArsTranEntity.art_seq));

                if (ArsTranEntity.art_date != string.Empty)
                    sqlParamList.Add(new SqlParameter("@art_date", ArsTranEntity.art_date));

                if (ArsTranEntity.art_contract != string.Empty)
                    sqlParamList.Add(new SqlParameter("@art_contract", ArsTranEntity.art_contract));

                if (ArsTranEntity.art_location != string.Empty)
                    sqlParamList.Add(new SqlParameter("@art_location", ArsTranEntity.art_location));

                if (ArsTranEntity.art_program_account != string.Empty)
                    sqlParamList.Add(new SqlParameter("@art_program_account", ArsTranEntity.art_program_account));

                if (ArsTranEntity.art_expense_code != string.Empty)
                    sqlParamList.Add(new SqlParameter("@art_expense_code", ArsTranEntity.art_expense_code));

                if (ArsTranEntity.art_class_identifier != string.Empty)
                    sqlParamList.Add(new SqlParameter("@art_class_identifier", ArsTranEntity.art_class_identifier));

                if (ArsTranEntity.art_inv_amt != string.Empty)
                    sqlParamList.Add(new SqlParameter("@art_inv_amt", ArsTranEntity.art_inv_amt));

                if (ArsTranEntity.art_open_amt != string.Empty)
                    sqlParamList.Add(new SqlParameter("@art_open_amt", ArsTranEntity.art_open_amt));

                if (ArsTranEntity.art_closed_amt != string.Empty)
                    sqlParamList.Add(new SqlParameter("@art_closed_amt", ArsTranEntity.art_closed_amt));

                if (ArsTranEntity.art_disc_taken != string.Empty)
                    sqlParamList.Add(new SqlParameter("@art_disc_taken", ArsTranEntity.art_disc_taken));

                if (ArsTranEntity.art_description != string.Empty)
                    sqlParamList.Add(new SqlParameter("@art_description", ArsTranEntity.art_description));

                if (ArsTranEntity.art_pmt_source != string.Empty)
                    sqlParamList.Add(new SqlParameter("@art_pmt_source", ArsTranEntity.art_pmt_source));

                if (ArsTranEntity.art_check != string.Empty)
                    sqlParamList.Add(new SqlParameter("@art_check", ArsTranEntity.art_check));

                if (ArsTranEntity.art1_entry != string.Empty)
                    sqlParamList.Add(new SqlParameter("@art1_entry", ArsTranEntity.art1_entry));

                if (ArsTranEntity.art_pmt_type != string.Empty)
                    sqlParamList.Add(new SqlParameter("@art_pmt_type", ArsTranEntity.art_pmt_type));

                if (ArsTranEntity.art1_type != string.Empty)
                    sqlParamList.Add(new SqlParameter("@art_type", ArsTranEntity.art1_type));

                if (ArsTranEntity.art_bill_rate != string.Empty)
                    sqlParamList.Add(new SqlParameter("@art_bill_rate", ArsTranEntity.art_bill_rate));

                if (ArsTranEntity.art_units != string.Empty)
                    sqlParamList.Add(new SqlParameter("@art_units", ArsTranEntity.art_units));

                if (ArsTranEntity.art_lstc_operator != string.Empty)
                    sqlParamList.Add(new SqlParameter("@art_lstc_operator", ArsTranEntity.art_lstc_operator));
                if (ArsTranEntity.art_add_operator != string.Empty)
                    sqlParamList.Add(new SqlParameter("@art_add_operator", ArsTranEntity.art_add_operator));
                sqlParamList.Add(new SqlParameter("@Mode", ArsTranEntity.Mode));
             boolStatus =   ARSDB.InsertUpdateDeleteARSTRAN(sqlParamList);

            }
            catch (Exception ex)
            {
                boolStatus = false;
            }

            return boolStatus;
        }

        public bool UpdateARSNUMB(UpdateARSNUMBEntity arsnumbertity)
        {
            bool boolStatus = false;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@arsnumb_code", arsnumbertity.arsnumb_code));
                sqlParamList.Add(new SqlParameter("@arsnumb_no", arsnumbertity.arsnumb_no));
                sqlParamList.Add(new SqlParameter("@ATTN_AGENCY", arsnumbertity.ATTN_AGENCY));
                sqlParamList.Add(new SqlParameter("@ATTN_DEPT", arsnumbertity.ATTN_DEPT));
                sqlParamList.Add(new SqlParameter("@ATTN_PROGRAM", arsnumbertity.ATTN_PROG));
                sqlParamList.Add(new SqlParameter("@ATTN_YEAR", arsnumbertity.ATTN_YEAR));
                sqlParamList.Add(new SqlParameter("@ATTN_APP_NO", arsnumbertity.ATTN_APP_NO));
                sqlParamList.Add(new SqlParameter("@ATTN_SITE", arsnumbertity.ATTN_SITE));
                sqlParamList.Add(new SqlParameter("@ATTN_ROOM", arsnumbertity.ATTN_ROOM));
                sqlParamList.Add(new SqlParameter("@ATTN_AMPM", arsnumbertity.ATTN_AMPM));
                sqlParamList.Add(new SqlParameter("@ATTN_FUNDING_SOURCE", arsnumbertity.ATTN_FUNDING_SOURCE));
                sqlParamList.Add(new SqlParameter("@ATTN_PARENT_INVOICE", arsnumbertity.ATTN_PARENT_INVOICE));
                sqlParamList.Add(new SqlParameter("@FromDate", arsnumbertity.FromDate));
                sqlParamList.Add(new SqlParameter("@ToDate", arsnumbertity.ToDate));
                sqlParamList.Add(new SqlParameter("@ATTN_LSTC_OPERATOR", arsnumbertity.ATTN_LSTC_OPERATOR));
                sqlParamList.Add(new SqlParameter("@Report", arsnumbertity.Report));
                
                boolStatus = ARSDB.UpdateARSNUMB(sqlParamList);
            }

            catch (Exception ex)
            {
                boolStatus = false;
            }

            return boolStatus;
        }



        public List<ARSCHECKEntity> Get_ARSCHECK(string Agency, string Dept, string Prog, string Appno, string Type, string Source, string Check)
        {
            List<ARSCHECKEntity> artCheckList = new List<ARSCHECKEntity>();
            try
            {
                DataSet arsData = Captain.DatabaseLayer.ARSDB.Get_ARSCHECK(Agency, Dept, Prog, Appno, Type, Source, Check);
                if (arsData != null && arsData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in arsData.Tables[0].Rows)
                    {
                        artCheckList.Add(new ARSCHECKEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return artCheckList;
            }

            return artCheckList;
        }


        public bool InsertUpdateDelARSCheck(ARSCHECKEntity ArsCheckEntity, out string Msg)
        {
            bool boolStatus = false;
            string strMsg = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                if (ArsCheckEntity.ARC_AGENCY != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARC_AGENCY", ArsCheckEntity.ARC_AGENCY));
                if (ArsCheckEntity.ARC_DEPT != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARC_DEPT", ArsCheckEntity.ARC_DEPT));
                if (ArsCheckEntity.ARC_PROGRAM != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARC_PROGRAM", ArsCheckEntity.ARC_PROGRAM));
                if (ArsCheckEntity.ARC_APP_NO != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARC_APP_NO", ArsCheckEntity.ARC_APP_NO));
                if (ArsCheckEntity.ARC_CUST_TYPE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARC_CUST_TYPE", ArsCheckEntity.ARC_CUST_TYPE));
                if (ArsCheckEntity.ARC_SOURCE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARC_SOURCE", ArsCheckEntity.ARC_SOURCE));
                if (ArsCheckEntity.ARC_CHECK != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARC_CHECK", ArsCheckEntity.ARC_CHECK));
                if (ArsCheckEntity.ARC_STATUS != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARC_STATUS", ArsCheckEntity.ARC_STATUS));
                if (ArsCheckEntity.ARC_TYPE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARC_TYPE", ArsCheckEntity.ARC_TYPE));
                if (ArsCheckEntity.ARC_CHECK_DATE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARC_CHECK_DATE", ArsCheckEntity.ARC_CHECK_DATE));
                if (ArsCheckEntity.ARC_AUTO != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARC_AUTO", ArsCheckEntity.ARC_AUTO));               

               
                if (ArsCheckEntity.ARC_DUE_DATE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARC_DUE_DATE", ArsCheckEntity.ARC_DUE_DATE));
                if (ArsCheckEntity.ARC_COMMENTS != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARC_COMMENTS", ArsCheckEntity.ARC_COMMENTS));
               
                if (ArsCheckEntity.ARC_BILLED != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARC_BILLED", ArsCheckEntity.ARC_BILLED));
                if (ArsCheckEntity.ARC_RECEIPT != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARC_RECEIPT", ArsCheckEntity.ARC_RECEIPT));
                if (ArsCheckEntity.ARC_ADJUSTMENTS != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARC_ADJUSTMENTS", ArsCheckEntity.ARC_ADJUSTMENTS));
                if (ArsCheckEntity.ARC_DISC_TAKEN != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARC_DISC_TAKEN", ArsCheckEntity.ARC_DISC_TAKEN));
                if (ArsCheckEntity.ARC_BALANCE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARC_BALANCE", ArsCheckEntity.ARC_BALANCE));              
                if (ArsCheckEntity.ARC_CLOSED_DATE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARC_CLOSED_DATE", ArsCheckEntity.ARC_CLOSED_DATE));

                if (ArsCheckEntity.ARC_LSTC_OPERATOR != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARC_LSTC_OPERATOR", ArsCheckEntity.ARC_LSTC_OPERATOR));
                if (ArsCheckEntity.ARC_ADD_OPERATOR != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARC_ADD_OPERATOR", ArsCheckEntity.ARC_ADD_OPERATOR));
                sqlParamList.Add(new SqlParameter("@Mode", ArsCheckEntity.Mode));
                SqlParameter parameterMsg = new SqlParameter("@Msg", SqlDbType.VarChar, 100);
                parameterMsg.Direction = ParameterDirection.Output;
                sqlParamList.Add(parameterMsg);

                boolStatus = ARSDB.InsertUpdateDelARSCHECK(sqlParamList);
                Msg = parameterMsg.Value.ToString();

            }
            catch (Exception ex)
            {
                Msg = string.Empty;
                return false;
            }

            return boolStatus;
        }



        public bool InsertUpdateDeleteARSChkDetails(ARSCHKDETEntity ArsChkdetEntity)
        {
            bool boolStatus = false;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                if (ArsChkdetEntity.ARD_Agency != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARD_agency", ArsChkdetEntity.ARD_Agency));
                if (ArsChkdetEntity.ARD_Dept != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARD_dept", ArsChkdetEntity.ARD_Dept));
                if (ArsChkdetEntity.ARD_Program != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARD_program", ArsChkdetEntity.ARD_Program));
                if (ArsChkdetEntity.ARD_App_no != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARD_app_no", ArsChkdetEntity.ARD_App_no));
                if (ArsChkdetEntity.ARD_Cust_type != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARD_Cust_type", ArsChkdetEntity.ARD_Cust_type));
                if (ArsChkdetEntity.ARD_Source != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARD_source", ArsChkdetEntity.ARD_Source));
                if (ArsChkdetEntity.ARD_Invoice != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARD_invoice", ArsChkdetEntity.ARD_Invoice));
                if (ArsChkdetEntity.ARD_Item != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARD_Item", ArsChkdetEntity.ARD_Item));

                if (ArsChkdetEntity.ARD_Seq != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARD_seq", ArsChkdetEntity.ARD_Seq));

                if (ArsChkdetEntity.ARD_Date != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARD_date", ArsChkdetEntity.ARD_Date));

                if (ArsChkdetEntity.ARD_Contract != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARD_contract", ArsChkdetEntity.ARD_Contract));

                if (ArsChkdetEntity.ARD_Location != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARD_location", ArsChkdetEntity.ARD_Location));

                if (ArsChkdetEntity.ARD_Program_account != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARD_program_account", ArsChkdetEntity.ARD_Program_account));

                if (ArsChkdetEntity.ARD_Expense_code != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARD_expense_code", ArsChkdetEntity.ARD_Expense_code));

                if (ArsChkdetEntity.ARD_Class_identifier != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARD_class_identifier", ArsChkdetEntity.ARD_Class_identifier));

                if (ArsChkdetEntity.ARD_Inv_amt != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARD_inv_amt", ArsChkdetEntity.ARD_Inv_amt));

                if (ArsChkdetEntity.ARD_Open_amt != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARD_open_amt", ArsChkdetEntity.ARD_Open_amt));

                if (ArsChkdetEntity.ARD_Closed_amt != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARD_closed_amt", ArsChkdetEntity.ARD_Closed_amt));

                if (ArsChkdetEntity.ARD_Disc_taken != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARD_disc_taken", ArsChkdetEntity.ARD_Disc_taken));

                if (ArsChkdetEntity.ARD_Description != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARD_description", ArsChkdetEntity.ARD_Description));

                if (ArsChkdetEntity.ARD_Pmt_source != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARD_pmt_source", ArsChkdetEntity.ARD_Pmt_source));

                if (ArsChkdetEntity.ARD_Check != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARD_check", ArsChkdetEntity.ARD_Check));

                if (ArsChkdetEntity.ARD1_ITEM != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARD1_ITEM", ArsChkdetEntity.ARD1_ITEM));

                if (ArsChkdetEntity.ARD_Pmt_type != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARD_pmt_type", ArsChkdetEntity.ARD_Pmt_type));

                if (ArsChkdetEntity.ARD1_Type != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARD_type", ArsChkdetEntity.ARD1_Type));

                if (ArsChkdetEntity.ARD_Bill_rate != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARD_bill_rate", ArsChkdetEntity.ARD_Bill_rate));
                
                if (ArsChkdetEntity.ARD_Units != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARD_units", ArsChkdetEntity.ARD_Units));

                if (ArsChkdetEntity.ARD_Lstc_operator != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARD_lstc_operator", ArsChkdetEntity.ARD_Lstc_operator));
                if (ArsChkdetEntity.ARD_Add_operator != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARD_add_operator", ArsChkdetEntity.ARD_Add_operator));
                sqlParamList.Add(new SqlParameter("@Mode", ArsChkdetEntity.Mode));
                boolStatus = ARSDB.InsertUpdateDelARSCHKDET(sqlParamList);

            }
            catch (Exception ex)
            {
                boolStatus = false;
            }

            return boolStatus;
        }

        public bool UpdateARSCHKDET(ARSCHKDETEntity ArsChkdetEntity)
        {
            bool boolStatus = false;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                if (ArsChkdetEntity.ARD_Agency != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARD_agency", ArsChkdetEntity.ARD_Agency));
                if (ArsChkdetEntity.ARD_Dept != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARD_dept", ArsChkdetEntity.ARD_Dept));
                if (ArsChkdetEntity.ARD_Program != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARD_program", ArsChkdetEntity.ARD_Program));
                if (ArsChkdetEntity.ARD_App_no != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARD_app_no", ArsChkdetEntity.ARD_App_no));
                if (ArsChkdetEntity.ARD_Cust_type != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARD_Cust_type", ArsChkdetEntity.ARD_Cust_type));
                if (ArsChkdetEntity.ARD_Source != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARD_source", ArsChkdetEntity.ARD_Source));

                if (ArsChkdetEntity.ARD_Check != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARD_check", ArsChkdetEntity.ARD_Check));


                if (ArsChkdetEntity.ARD_Date != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARD_CHECKDATE", ArsChkdetEntity.ARD_Date));

            
                if (ArsChkdetEntity.ARD_Inv_amt != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARD_AMOUNT", ArsChkdetEntity.ARD_Inv_amt));

                if (ArsChkdetEntity.ARD_Auto != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARD_AUTO", ArsChkdetEntity.ARD_Auto));

                

                if (ArsChkdetEntity.ARD_XmlDetails != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CheckDetails", ArsChkdetEntity.ARD_XmlDetails));

                if (ArsChkdetEntity.ARD_Lstc_operator != string.Empty)
                    sqlParamList.Add(new SqlParameter("@ARD_lstc_operator", ArsChkdetEntity.ARD_Lstc_operator));
               
                sqlParamList.Add(new SqlParameter("@Mode", ArsChkdetEntity.Mode));
                boolStatus = ARSDB.UpdateARSCHKDET(sqlParamList);

            }
            catch (Exception ex)
            {
                boolStatus = false;
            }

            return boolStatus;
        }



        public List<ARSCHKDETEntity> Get_ARSCHKDETDetails(string Agency, string Dept, string Prog, string Appno, string Type, string Source, string Invoice, string Entry, string Seq,string Check)
        {
            List<ARSCHKDETEntity> artChkDetList = new List<ARSCHKDETEntity>();
            try
            {
                DataSet arsData = Captain.DatabaseLayer.ARSDB.Get_ARSChkDetDetails(Agency, Dept, Prog, Appno, Type, Source, Invoice, Entry, Seq, Check);
                if (arsData != null && arsData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in arsData.Tables[0].Rows)
                    {
                        artChkDetList.Add(new ARSCHKDETEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return artChkDetList;
            }

            return artChkDetList;
        }

      


        

    }
}
