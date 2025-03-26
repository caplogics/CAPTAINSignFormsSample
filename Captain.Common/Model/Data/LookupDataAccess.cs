using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Captain.DatabaseLayer;
using Captain.Common.Model.Objects;
using System.Data;
using Captain.Common.Utilities;
using System.Data.SqlClient;
using System.Globalization;
using Microsoft.Win32;
using Wisej.Web;
using DevExpress.CodeParser;

namespace Captain.Common.Model.Data
{
    public class LookupDataAccess
    {

        public LookupDataAccess(CaptainModel model)
        {
            Model = model;

        }

        #region Properties

        public CaptainModel Model { get; set; }

        public string UserId { get; set; }

        #endregion

        /// <summary>
        /// Get User screens information. 
        /// </summary>
        /// <param name="userID">The user ID to get user profile.</param>
        /// <returns>Returns a DataSet with the screens.</returns>
        public List<HierarchyEntity> GetCaseHierarchy(string Intake)
        {
            List<HierarchyEntity> caseHierarchyEntity = new List<HierarchyEntity>();
            try
            {
                DataSet hierarchyData = Lookups.GetHierarchies("DEFAULT");
                if (hierarchyData != null && hierarchyData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in hierarchyData.Tables[0].Rows)
                    {
                        caseHierarchyEntity.Add(new HierarchyEntity(row, "DEFAULT"));
                    }
                }
            }
            catch (Exception ex)
            {
                return caseHierarchyEntity;
            }

            return caseHierarchyEntity;
        }

        /// <summary>
        /// Get User screens information. 
        /// </summary>
        /// <param name="userID">The user ID to get user profile.</param>
        /// <returns>Returns a DataSet with the screens.</returns>
        public List<HierarchyEntity> GetCaseHierarchy(string FilterBy, string Agency, string Dept, string UserId, string AdminAgency)
        {
            List<HierarchyEntity> caseHierarchyEntity = new List<HierarchyEntity>();
            try
            {
                DataSet hierarchyData = Lookups.GetHierarchies(FilterBy, Agency, Dept, string.Empty, UserId, AdminAgency);
                if (hierarchyData != null && hierarchyData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in hierarchyData.Tables[0].Rows)
                    {
                        caseHierarchyEntity.Add(new HierarchyEntity(row, "CASEHIE"));
                    }
                }
            }
            catch (Exception ex)
            {
                return caseHierarchyEntity;
            }

            return caseHierarchyEntity;
        }

        public List<CommonEntity> GeAGYTABForCrosswalk(string strFilterCode)
        {
            List<CommonEntity> commonEntity = new List<CommonEntity>();
            try
            {

                DataSet AgyTabData = Lookups.GetLookUpFromAGYTAB(strFilterCode);
                if (AgyTabData != null && AgyTabData.Tables[0].Rows.Count > 0)
                {
                    AgyTabData.Tables[0].Rows.Add("<ignore>", "-", "", "", "");
                    DataRow[] _drRows = AgyTabData.Tables[0].Select(null, "LookUpDesc ASC");
                    foreach (DataRow row in _drRows)
                    {
                        commonEntity.Add(new CommonEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return commonEntity;
            }

            return commonEntity;
        }
        public List<CustRespEntity> GetCustRespForCrossWalk(string ScrCode, string custCode)
        {
            List<CustRespEntity> custEntity = new List<CustRespEntity>();
            try
            {
                DataSet TableData = FieldControlsDB.GetQuestionResponses(ScrCode, custCode);
                if (TableData != null && TableData.Tables[0].Rows.Count > 0)
                {
                    TableData.Tables[0].Rows.Add(custCode, TableData.Tables[0].Rows.Count + 1, "<ignore>", "-", "CASE2001", DateTime.Now.ToString(), "SYSTEM", DateTime.Now.ToString(), "SYSTEM", null, "A", "", "", "");
                    DataRow[] _drRows = TableData.Tables[0].Select(null, "RSP_DESC ASC");
                    foreach (DataRow row in _drRows)
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
        public List<HierarchyEntity> GetCaseHIERPT(string FilterBy, string Agency, string Dept, string UserId, string AdminAgency, string frmScrn)
        {
            List<HierarchyEntity> caseHierarchyEntity = new List<HierarchyEntity>();
            try
            {
                DataSet hierarchyData = Lookups.GetHierarchies(FilterBy, Agency, Dept, string.Empty, UserId, AdminAgency);
                if (hierarchyData != null && hierarchyData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in hierarchyData.Tables[0].Rows)
                    {
                        caseHierarchyEntity.Add(new HierarchyEntity(row, "CASEHIE", frmScrn));
                    }
                }
            }
            catch (Exception ex)
            {
                return caseHierarchyEntity;
            }

            return caseHierarchyEntity;
        }
        /// <summary>
        /// Get User screens information. 
        /// </summary>
        /// <param name="userID">The user ID to get user profile.</param>
        /// <returns>Returns a DataSet with the screens.</returns>
        public HierarchyEntity GetCaseHierarchy(string FilterBy, string Agency, string Dept, string Program, string UserId, string AdminAgency)
        {
            HierarchyEntity caseHierarchyEntity = null;
            try
            {
                DataSet hierarchyData = Lookups.GetHierarchies(FilterBy, Agency, Dept, Program, UserId, AdminAgency);
                if (hierarchyData != null && hierarchyData.Tables[0].Rows.Count > 0)
                {
                    caseHierarchyEntity = new HierarchyEntity(hierarchyData.Tables[0].Rows[0], "CASEHIE");
                }
            }
            catch (Exception ex)
            {
                return caseHierarchyEntity;
            }

            return caseHierarchyEntity;
        }

        public List<HierarchyEntity> GetAgencyRepresentation()
        {
            List<HierarchyEntity> caseHierarchyEntity = new List<HierarchyEntity>();
            caseHierarchyEntity.Add(new HierarchyEntity("01", "Agency"));
            caseHierarchyEntity.Add(new HierarchyEntity("02", "Agency Department"));
            caseHierarchyEntity.Add(new HierarchyEntity("03", "Agency Program"));
            caseHierarchyEntity.Add(new HierarchyEntity("04", "Agency Department Program"));
            caseHierarchyEntity.Add(new HierarchyEntity("05", "Agency Program Department"));
            caseHierarchyEntity.Add(new HierarchyEntity("06", "Department"));
            caseHierarchyEntity.Add(new HierarchyEntity("07", "Department Agency"));
            caseHierarchyEntity.Add(new HierarchyEntity("08", "Department Program"));
            caseHierarchyEntity.Add(new HierarchyEntity("09", "Department Program Agency"));
            caseHierarchyEntity.Add(new HierarchyEntity("10", "Department Agency Program"));
            caseHierarchyEntity.Add(new HierarchyEntity("11", "Program"));
            caseHierarchyEntity.Add(new HierarchyEntity("12", "Program Agency"));
            caseHierarchyEntity.Add(new HierarchyEntity("13", "Program Department"));
            caseHierarchyEntity.Add(new HierarchyEntity("14", "Program Agency Department"));
            caseHierarchyEntity.Add(new HierarchyEntity("15", "Program Department Agency"));

            return caseHierarchyEntity;
        }

        public List<HierarchyEntity> GetClientNameFormat()
        {
            List<HierarchyEntity> caseHierarchyEntity = new List<HierarchyEntity>();
            caseHierarchyEntity.Add(new HierarchyEntity("1", "First Name MI. Last Name"));
            caseHierarchyEntity.Add(new HierarchyEntity("2", "Last Name, First Name MI"));

            return caseHierarchyEntity;
        }

        public List<HierarchyEntity> GetCaseWorkerFormat()
        {
            List<HierarchyEntity> caseHierarchyEntity = new List<HierarchyEntity>();
            caseHierarchyEntity.Add(new HierarchyEntity("1", "First Name Last Name"));
            caseHierarchyEntity.Add(new HierarchyEntity("2", "Last Name, First Name"));

            return caseHierarchyEntity;
        }

        public List<CommonEntity> GetMonths()
        {
            List<CommonEntity> months = new List<CommonEntity>();
            months.Add(new CommonEntity("1", "January"));
            months.Add(new CommonEntity("2", "February"));
            months.Add(new CommonEntity("3", "March"));
            months.Add(new CommonEntity("4", "April"));
            months.Add(new CommonEntity("5", "May"));
            months.Add(new CommonEntity("6", "June"));
            months.Add(new CommonEntity("7", "July"));
            months.Add(new CommonEntity("8", "August"));
            months.Add(new CommonEntity("9", "September"));
            months.Add(new CommonEntity("10", "October"));
            months.Add(new CommonEntity("11", "November"));
            months.Add(new CommonEntity("12", "December"));
            return months;
        }

        public static string GetMonthName(string Month)
        {
            string MonName = string.Empty;

            switch (Month)
            {
                case "JAN": MonName = "January"; break;
                case "FEB": MonName = "February"; break;
                case "MAR": MonName = "March"; break;
                case "APR": MonName = "April"; break;
                case "MAY": MonName = "May"; break;
                case "JUN": MonName = "June"; break;
                case "JUL": MonName = "July"; break;
                case "AUG": MonName = "August"; break;
                case "SEP": MonName = "September"; break;
                case "OCT": MonName = "October"; break;
                case "NOV": MonName = "November"; break;
                case "DEC": MonName = "December"; break;
            }

            return MonName;
        }

        public static string GetMonth(string Month)
        {
            string MonName = string.Empty;

            switch (Month)
            {
                case "01":
                case "1": MonName = "January"; break;
                case "02":
                case "2": MonName = "February"; break;
                case "03":
                case "3": MonName = "March"; break;
                case "04":
                case "4": MonName = "April"; break;
                case "05":
                case "5": MonName = "May"; break;
                case "06":
                case "6": MonName = "June"; break;
                case "07":
                case "7": MonName = "July"; break;
                case "08":
                case "8": MonName = "August"; break;
                case "09":
                case "9": MonName = "September"; break;
                case "10": MonName = "October"; break;
                case "11": MonName = "November"; break;
                case "12": MonName = "December"; break;
            }

            return MonName;
        }

        public static string GetFixedLengthString(string input, int length)
        {
            string result = string.Empty;

            if (string.IsNullOrEmpty(input))
            {
                result = new string(' ', length);
            }
            else if (input.Length > length)
            {
                result = input.Substring(0, length);
            }
            else
            {
                result = input.PadRight(length);
            }

            return result;
        }

        public List<CommonEntity> GetDefaultQuestions()
        {
            List<CommonEntity> questions = new List<CommonEntity>();
            questions.Add(new CommonEntity("A", "Show All Questions"));
            questions.Add(new CommonEntity("P", "Show Questions For This Program"));
            //questions.Add(new CommonEntity("E", "Eligibility Criteria Specific"));
            //questions.Add(new CommonEntity("T", "Not Eligibility Criteria Specific"));

            return questions;
        }

        /// <summary>
        /// Get User screens information. 
        /// </summary>
        /// <param name="userID">The user ID to get user profile.</param>
        /// <returns>Returns a DataSet with the screens.</returns>
        public List<HierarchyEntity> GetHierarchyByUserID(string userID, string pwhtype, string strType)
        {
            List<HierarchyEntity> hierarchyEntity = new List<HierarchyEntity>();
            try
            {
                DataSet hierarchyData = Lookups.GetHierarchiesByUserID(userID, pwhtype, strType);
                if (hierarchyData != null && hierarchyData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in hierarchyData.Tables[0].Rows)
                    {
                        if (strType == "S")
                            hierarchyEntity.Add(new HierarchyEntity(row, strType));
                        else
                            hierarchyEntity.Add(new HierarchyEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return hierarchyEntity;
            }

            return hierarchyEntity;
        }

        public List<HierarchyEntity> GetserHierarchyByUserID(string userID, string pwhtype, string strType, string Agy, string Dept, string Prog)
        {
            List<HierarchyEntity> hierarchyEntity = new List<HierarchyEntity>();
            try
            {
                DataSet hierarchyData = Lookups.GetSerHierarchiesByUserID(userID, pwhtype, strType, Agy, Dept, Prog);
                if (hierarchyData != null && hierarchyData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in hierarchyData.Tables[0].Rows)
                    {
                        if (strType == "S")
                            hierarchyEntity.Add(new HierarchyEntity(row, strType));
                        else
                            hierarchyEntity.Add(new HierarchyEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return hierarchyEntity;
            }

            return hierarchyEntity;
        }

        public List<PASSWORDSERENTITY> GetPasswordSerEntity(string userID, string strType)
        {
            List<PASSWORDSERENTITY> hierarchyEntity = new List<PASSWORDSERENTITY>();
            try
            {
                DataSet hierarchyData = Lookups.GetPasswordSerUserID(userID);
                if (hierarchyData != null && hierarchyData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in hierarchyData.Tables[0].Rows)
                    {
                        hierarchyEntity.Add(new PASSWORDSERENTITY(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return hierarchyEntity;
            }

            return hierarchyEntity;
        }


        public List<HierarchyEntity> Get_SerPlan_Prog_List(string userID, string SerPlan, string StrType)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            List<HierarchyEntity> hierarchyEntity = new List<HierarchyEntity>();
            try
            {
                sqlParamList.Add(new SqlParameter("@UserName", userID));
                sqlParamList.Add(new SqlParameter("@SerPlan_Code", SerPlan));

                if (!string.IsNullOrEmpty(StrType.Trim()))
                    sqlParamList.Add(new SqlParameter("@PWH_TYPE", StrType));

                DataSet SerPla_Prog = Captain.DatabaseLayer.SPAdminDB.Browse_Selected_Table(sqlParamList, "[dbo].[Get_SerPlan_Programs]");

                if (SerPla_Prog != null && SerPla_Prog.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in SerPla_Prog.Tables[0].Rows)
                        hierarchyEntity.Add(new HierarchyEntity(row, 'S'));
                }
            }
            catch (Exception ex)
            {
                return hierarchyEntity;
            }

            return hierarchyEntity;
        }

        /// <summary>
        /// Get User screens information. 
        /// </summary>
        /// <param name="userID">The user ID to get user profile.</param>
        /// <returns>Returns a DataSet with the screens.</returns>
        public List<HierarchyEntity> GetPasswordHieByUserID(string userID)
        {
            List<HierarchyEntity> hierarchyEntity = new List<HierarchyEntity>();
            try
            {
                DataSet hierarchyData = Lookups.GetPasswordHieByUserID(userID);
                if (hierarchyData != null && hierarchyData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in hierarchyData.Tables[0].Rows)
                    {
                        hierarchyEntity.Add(new HierarchyEntity(row, "PASSWORDHIEBYID"));
                    }
                }
            }
            catch (Exception ex)
            {
                return hierarchyEntity;
            }

            return hierarchyEntity;
        }

        ///// <summary>
        ///// Get Relationship screens information. 
        ///// </summary>
        ///// <param name="userID">The user ID to get user profile.</param>
        ///// <returns>Returns a DataSet with the screens.</returns>
        //public List<AgyTabEntity> GetRelationship()
        //{
        //    List<AgyTabEntity> lookUpEntity = new List<AgyTabEntity>();
        //    try
        //    {
        //        DataSet hierarchyData = Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.RELATIONSHIP);
        //        if (hierarchyData != null && hierarchyData.Tables[0].Rows.Count > 0)
        //        {
        //            foreach (DataRow row in hierarchyData.Tables[0].Rows)
        //            {
        //                lookUpEntity.Add(new AgyTabEntity(row, string.Empty));
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return lookUpEntity;
        //    }

        //    return lookUpEntity;
        //}

        /// <summary>
        /// Get Invoice Source screens information. 
        /// </summary>
        /// <param name="userID">The user ID to get user profile.</param>
        /// <returns>Returns a DataSet with the screens.</returns>
        public List<AgyTabEntity> GetInvoiceSource()
        {
            List<AgyTabEntity> lookUpEntity = new List<AgyTabEntity>();
            try
            {
                DataSet hierarchyData = Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.ARINVOICESOURCES);
                if (hierarchyData != null && hierarchyData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in hierarchyData.Tables[0].Rows)
                    {
                        lookUpEntity.Add(new AgyTabEntity(row, string.Empty));
                    }
                }
            }
            catch (Exception ex)
            {
                return lookUpEntity;
            }

            return lookUpEntity;
        }

        /// <summary>
        /// Get Invoice Source screens information. 
        /// </summary>
        /// <param name="userID">The user ID to get user profile.</param>
        /// <returns>Returns a DataSet with the screens.</returns>
        public List<AgyTabEntity> GetCustomerTypes()
        {
            List<AgyTabEntity> lookUpEntity = new List<AgyTabEntity>();
            try
            {
                DataSet hierarchyData = Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.ARCUSTOMERTYPES);
                if (hierarchyData != null && hierarchyData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in hierarchyData.Tables[0].Rows)
                    {
                        lookUpEntity.Add(new AgyTabEntity(row, string.Empty));
                    }
                }
            }
            catch (Exception ex)
            {
                return lookUpEntity;
            }

            return lookUpEntity;
        }

        /// <summary>
        /// Get Relationship screens information. 
        /// </summary>
        /// <param name="userID">The user ID to get user profile.</param>
        /// <returns>Returns a DataSet with the screens.</returns>
        public List<AgyTabEntity> GetIncomeTypes()
        {
            List<AgyTabEntity> lookUpEntity = new List<AgyTabEntity>();
            try
            {
                DataSet lookUpData = Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.INCOMETYPES);
                if (lookUpData != null && lookUpData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in lookUpData.Tables[0].Rows)
                    {
                        lookUpEntity.Add(new AgyTabEntity(row, string.Empty));
                    }
                }
            }
            catch (Exception ex)
            {
                return lookUpEntity;
            }

            return lookUpEntity;
        }

        /// <summary>
        /// Get Relationship screens information. 
        /// </summary>
        /// <param name="userID">The user ID to get user profile.</param>
        /// <returns>Returns a DataSet with the screens.</returns>
        public List<CommonEntity> GetIncomeTypesDeduction()
        {

            List<CommonEntity> lookUpEntity = new List<CommonEntity>();
            try
            {
                DataSet lookUpData = Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.INCOMETYPES);
                if (lookUpData != null && lookUpData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in lookUpData.Tables[0].Rows)
                    {
                        lookUpEntity.Add(new CommonEntity(row["Code"].ToString(), row["LookUpDesc"].ToString(), row["Hierarchy"].ToString(), row["EXT"].ToString(), row["AGY_DEFAULT"].ToString(), row["Active"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                return lookUpEntity;
            }

            return lookUpEntity;
            //List<AgyTabEntity> lookUpEntity = new List<AgyTabEntity>();
            //try
            //{
            //    DataSet lookUpData = Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.INCOMETYPES);
            //    if (lookUpData != null && lookUpData.Tables[0].Rows.Count > 0)
            //    {
            //        foreach (DataRow row in lookUpData.Tables[0].Rows)
            //        {
            //            lookUpEntity.Add(new AgyTabEntity(row, "00004"));
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    return lookUpEntity;
            //}

            //return lookUpEntity;
        }

        /// <summary>
        /// Get Image Types information. 
        /// <returns>Returns a DataSet with the screens.</returns>
        public List<CommonEntity> GetImageTypes()
        {
            List<CommonEntity> lookUpEntity = new List<CommonEntity>();
            try
            {
                DataSet lookUpData = Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.IMAGETYPES);
                if (lookUpData != null && lookUpData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in lookUpData.Tables[0].Rows)
                    {
                        lookUpEntity.Add(new CommonEntity(row["Code"].ToString(), row["LookUpDesc"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                return lookUpEntity;
            }

            return lookUpEntity;
        }

        /// <summary>
        /// Get Image Types information. 
        /// <returns>Returns a DataSet with the screens.</returns>
        public List<CommonEntity> GetImageNameConvention()
        {
            List<CommonEntity> lookUpEntity = new List<CommonEntity>();
            try
            {
                DataSet lookUpData = Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.IMAGENAMECONVENTION);
                if (lookUpData != null && lookUpData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in lookUpData.Tables[0].Rows)
                    {
                        lookUpEntity.Add(new CommonEntity(row["Code"].ToString(), row["LookUpDesc"].ToString(), row["Hierarchy"].ToString(), row["EXT"].ToString(), string.Empty, string.Empty));
                    }
                }
            }
            catch (Exception ex)
            {
                return lookUpEntity;
            }

            return lookUpEntity;
        }




        /// <summary>
        /// Get Alert Codes screens information. 
        /// </summary>
        /// <param name="userID">The user ID to get user profile.</param>
        /// <returns>Returns a DataSet with the screens.</returns>
        public List<CommonEntity> GetAlertCodes()
        {
            List<CommonEntity> lookUpEntity = new List<CommonEntity>();
            try
            {
                DataSet lookUpData = Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.ALERTCODES);
                if (lookUpData != null && lookUpData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in lookUpData.Tables[0].Rows)
                    {
                        lookUpEntity.Add(new CommonEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return lookUpEntity;
            }

            return lookUpEntity;
        }

        public List<CommonEntity> GetCaseType()
        {
            List<CommonEntity> commonEntity = new List<CommonEntity>();
            try
            {

                DataSet CaseTypeData = Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.CASETYPES);
                if (CaseTypeData != null && CaseTypeData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseTypeData.Tables[0].Rows)
                    {
                        commonEntity.Add(new CommonEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return commonEntity;
            }

            return commonEntity;
        }

        public List<CommonEntity> GetTownship()
        {
            List<CommonEntity> commonEntity = new List<CommonEntity>();
            try
            {
                DataSet TownshipData = Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.CITYTOWNTABLE);
                if (TownshipData != null && TownshipData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in TownshipData.Tables[0].Rows)
                    {
                        commonEntity.Add(new CommonEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return commonEntity;
            }

            return commonEntity;
        }

        public List<CommonEntity> GetCountry()
        {
            List<CommonEntity> commonEntity = new List<CommonEntity>();
            try
            {
                DataSet CountryData = Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.COUNTY);
                if (CountryData != null && CountryData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CountryData.Tables[0].Rows)
                    {
                        commonEntity.Add(new CommonEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return commonEntity;
            }

            return commonEntity;
        }

        public List<CommonEntity> GetHousing()
        {
            List<CommonEntity> commonEntity = new List<CommonEntity>();
            try
            {
                DataSet HousingData = Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.HOUSINGTYPES);
                if (HousingData != null && HousingData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in HousingData.Tables[0].Rows)
                    {
                        commonEntity.Add(new CommonEntity(row["Code"].ToString(), row["LookUpDesc"].ToString(), row["Hierarchy"].ToString(), row["EXT"].ToString(), row["AGY_DEFAULT"].ToString(), row["Active"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                return commonEntity;
            }

            return commonEntity;
        }

        public List<CommonEntity> GetRelationship()
        {
            List<CommonEntity> commonEntity = new List<CommonEntity>();
            try
            {
                DataSet hierarchyData = Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.RELATIONSHIP);
                if (hierarchyData != null && hierarchyData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in hierarchyData.Tables[0].Rows)
                    {
                        commonEntity.Add(new CommonEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return commonEntity;
            }

            return commonEntity;
        }

        public List<CommonEntity> GetMeal()
        {
            List<CommonEntity> commonEntity = new List<CommonEntity>();
            try
            {
                DataSet hierarchyData = Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.ARTERMS);
                if (hierarchyData != null && hierarchyData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in hierarchyData.Tables[0].Rows)
                    {
                        commonEntity.Add(new CommonEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return commonEntity;
            }

            return commonEntity;
        }

        public List<CommonEntity> GetCategrcalEligiblity()
        {
            List<CommonEntity> commonEntity = new List<CommonEntity>();
            try
            {
                DataSet hierarchyData = Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.CATEGRCALELIGIBLTY);
                if (hierarchyData != null && hierarchyData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in hierarchyData.Tables[0].Rows)
                    {
                        commonEntity.Add(new CommonEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return commonEntity;
            }

            return commonEntity;
        }

        public List<CommonEntity> GetFundingSources()
        {
            List<CommonEntity> commonEntity = new List<CommonEntity>();
            try
            {
                DataSet hierarchyData = Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.FUNDINGSOURCES);
                if (hierarchyData != null && hierarchyData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in hierarchyData.Tables[0].Rows)
                    {
                        commonEntity.Add(new CommonEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return commonEntity;
            }

            return commonEntity;
        }

        public List<CommonEntity> GetEthnicity()
        {
            List<CommonEntity> commonEntity = new List<CommonEntity>();
            try
            {
                DataSet hierarchyData = Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.ETHNICODES);
                if (hierarchyData != null && hierarchyData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in hierarchyData.Tables[0].Rows)
                    {
                        commonEntity.Add(new CommonEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return commonEntity;
            }

            return commonEntity;
        }

        public List<CommonEntity> GetClassfication()
        {
            List<CommonEntity> commonEntity = new List<CommonEntity>();
            try
            {
                DataSet hierarchyData = Lookups.GetAgyTabs("S0013", string.Empty, string.Empty);
                if (hierarchyData != null && hierarchyData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in hierarchyData.Tables[0].Rows)
                    {
                        commonEntity.Add(new CommonEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return commonEntity;
            }

            return commonEntity;
        }

        public List<CommonEntity> GetRace()
        {
            List<CommonEntity> commonEntity = new List<CommonEntity>();
            try
            {
                DataSet hierarchyData = Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.RACE);
                if (hierarchyData != null && hierarchyData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in hierarchyData.Tables[0].Rows)
                    {
                        commonEntity.Add(new CommonEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return commonEntity;
            }

            return commonEntity;
        }

        public List<CommonEntity> GetEducation()
        {
            List<CommonEntity> commonEntity = new List<CommonEntity>();
            try
            {
                DataSet hierarchyData = Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.EDUCATIONCODES);
                if (hierarchyData != null && hierarchyData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in hierarchyData.Tables[0].Rows)
                    {
                        commonEntity.Add(new CommonEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return commonEntity;
            }

            return commonEntity;
        }

        public List<CommonEntity> GetSchool()
        {
            List<CommonEntity> commonEntity = new List<CommonEntity>();
            try
            {
                DataSet hierarchyData = Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.SCHOOLDISTRICTS);
                if (hierarchyData != null && hierarchyData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in hierarchyData.Tables[0].Rows)
                    {
                        commonEntity.Add(new CommonEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return commonEntity;
            }

            return commonEntity;
        }

        public List<CommonEntity> GetGender()
        {
            List<CommonEntity> commonEntity = new List<CommonEntity>();
            try
            {
                DataSet hierarchyData = Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.GENDER);
                if (hierarchyData != null && hierarchyData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in hierarchyData.Tables[0].Rows)
                    {
                        commonEntity.Add(new CommonEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return commonEntity;
            }

            return commonEntity;
        }

        public List<CommonEntity> GetAreyoupregnant()
        {
            List<CommonEntity> commonEntity = new List<CommonEntity>();
            try
            {
                DataSet hierarchyData = Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.PREGNANT);
                if (hierarchyData != null && hierarchyData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in hierarchyData.Tables[0].Rows)
                    {
                        commonEntity.Add(new CommonEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return commonEntity;
            }

            return commonEntity;
        }

        public List<CommonEntity> GetMaritalStatus()
        {
            List<CommonEntity> commonEntity = new List<CommonEntity>();
            try
            {
                DataSet hierarchyData = Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.MARITALSTATUS);
                if (hierarchyData != null && hierarchyData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in hierarchyData.Tables[0].Rows)
                    {
                        commonEntity.Add(new CommonEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return commonEntity;
            }

            return commonEntity;
        }

        public List<CommonEntity> GetHealthInsurance()
        {
            List<CommonEntity> commonEntity = new List<CommonEntity>();
            try
            {
                DataSet hierarchyData = Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.HEALTHINSURANCE);
                if (hierarchyData != null && hierarchyData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in hierarchyData.Tables[0].Rows)
                    {
                        commonEntity.Add(new CommonEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return commonEntity;
            }

            return commonEntity;
        }

        public List<CommonEntity> GetVeteranCode()
        {
            List<CommonEntity> commonEntity = new List<CommonEntity>();
            try
            {
                DataSet hierarchyData = Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.VETERAN);
                if (hierarchyData != null && hierarchyData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in hierarchyData.Tables[0].Rows)
                    {
                        commonEntity.Add(new CommonEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return commonEntity;
            }

            return commonEntity;
        }

        public List<CommonEntity> GetFoodStamps()
        {
            List<CommonEntity> commonEntity = new List<CommonEntity>();
            try
            {
                DataSet hierarchyData = Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.FOODSTAMPS);
                if (hierarchyData != null && hierarchyData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in hierarchyData.Tables[0].Rows)
                    {
                        commonEntity.Add(new CommonEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return commonEntity;
            }

            return commonEntity;
        }

        public List<CommonEntity> GetWIC()
        {
            List<CommonEntity> commonEntity = new List<CommonEntity>();
            try
            {
                DataSet hierarchyData = Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.WIC);
                if (hierarchyData != null && hierarchyData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in hierarchyData.Tables[0].Rows)
                    {
                        commonEntity.Add(new CommonEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return commonEntity;
            }

            return commonEntity;
        }

        public List<CommonEntity> GetFarmer()
        {
            List<CommonEntity> commonEntity = new List<CommonEntity>();
            try
            {
                DataSet hierarchyData = Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.FARMER);
                if (hierarchyData != null && hierarchyData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in hierarchyData.Tables[0].Rows)
                    {
                        commonEntity.Add(new CommonEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return commonEntity;
            }

            return commonEntity;
        }

        public List<CommonEntity> GetDisabled()
        {
            List<CommonEntity> commonEntity = new List<CommonEntity>();
            try
            {
                DataSet hierarchyData = Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.DISABLED);
                if (hierarchyData != null && hierarchyData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in hierarchyData.Tables[0].Rows)
                    {
                        commonEntity.Add(new CommonEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return commonEntity;
            }

            return commonEntity;
        }

        public List<CommonEntity> GetDriving()
        {
            List<CommonEntity> commonEntity = new List<CommonEntity>();
            try
            {
                DataSet hierarchyData = Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.DRIVERLICENSE);
                if (hierarchyData != null && hierarchyData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in hierarchyData.Tables[0].Rows)
                    {
                        commonEntity.Add(new CommonEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return commonEntity;
            }

            return commonEntity;
        }

        public List<CommonEntity> GetReliableTrans()
        {
            List<CommonEntity> commonEntity = new List<CommonEntity>();
            try
            {
                DataSet hierarchyData = Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.RELIABLETRANSPORTATION);
                if (hierarchyData != null && hierarchyData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in hierarchyData.Tables[0].Rows)
                    {
                        commonEntity.Add(new CommonEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return commonEntity;
            }

            return commonEntity;
        }

        public List<CommonEntity> GetResident()
        {
            List<CommonEntity> commonEntity = new List<CommonEntity>();
            try
            {
                DataSet hierarchyData = Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.RESIDENTCODES);
                if (hierarchyData != null && hierarchyData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in hierarchyData.Tables[0].Rows)
                    {
                        commonEntity.Add(new CommonEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return commonEntity;
            }

            return commonEntity;
        }

        public List<CommonEntity> GetPrimaryLanguage()
        {
            List<CommonEntity> commonEntity = new List<CommonEntity>();
            try
            {
                DataSet PrimaryLanguageData = Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.LANGUAGECODES);
                if (PrimaryLanguageData != null && PrimaryLanguageData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in PrimaryLanguageData.Tables[0].Rows)
                    {
                        commonEntity.Add(new CommonEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return commonEntity;
            }

            return commonEntity;
        }

        public List<CommonEntity> GetSecondaryLanguage()
        {
            List<CommonEntity> commonEntity = new List<CommonEntity>();
            try
            {

                DataSet SecondaryLanguageData = Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.LANGUAGECODES);
                if (SecondaryLanguageData != null && SecondaryLanguageData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in SecondaryLanguageData.Tables[0].Rows)
                    {
                        commonEntity.Add(new CommonEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return commonEntity;
            }

            return commonEntity;
        }

        public List<CommonEntity> GetReasonfor()
        {
            List<CommonEntity> commonEntity = new List<CommonEntity>();
            try
            {
                DataSet PrimaryLanguageData = Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.Reasonfor);
                if (PrimaryLanguageData != null && PrimaryLanguageData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in PrimaryLanguageData.Tables[0].Rows)
                    {
                        commonEntity.Add(new CommonEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return commonEntity;
            }

            return commonEntity;
        }

        public List<CommonEntity> GetCategory()
        {
            List<CommonEntity> commonEntity = new List<CommonEntity>();
            try
            {
                DataSet PrimaryLanguageData = Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.Category);
                if (PrimaryLanguageData != null && PrimaryLanguageData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in PrimaryLanguageData.Tables[0].Rows)
                    {
                        commonEntity.Add(new CommonEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return commonEntity;
            }

            return commonEntity;
        }

        public List<CommonEntity> GetFamilyType()
        {
            List<CommonEntity> commonEntity = new List<CommonEntity>();
            try
            {

                DataSet FamilyTypeData = Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.FAMILYTYPE);
                if (FamilyTypeData != null && FamilyTypeData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in FamilyTypeData.Tables[0].Rows)
                    {
                        commonEntity.Add(new CommonEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return commonEntity;
            }

            return commonEntity;
        }

        public List<CommonEntity> Getcontactyou()
        {
            List<CommonEntity> commonEntity = new List<CommonEntity>();
            try
            {
                DataSet contactyouData = Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.BESTWAYTOCONTACT);
                if (contactyouData != null && contactyouData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in contactyouData.Tables[0].Rows)
                    {
                        commonEntity.Add(new CommonEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return commonEntity;
            }

            return commonEntity;
        }

        public List<CommonEntity> GetaboutUs()
        {
            List<CommonEntity> commonEntity = new List<CommonEntity>();
            try
            {
                DataSet aboutUsData = Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.HOWDIDYOUHEARABOUTTHEPROGRAM);
                if (aboutUsData != null && aboutUsData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in aboutUsData.Tables[0].Rows)
                    {
                        commonEntity.Add(new CommonEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return commonEntity;
            }

            return commonEntity;
        }

        public List<CommonEntity> Get_HearingSources()
        {
            List<CommonEntity> commonEntity = new List<CommonEntity>();
            try
            {
                DataSet hierarchyData = Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.HEATSOURCE);
                if (hierarchyData != null && hierarchyData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in hierarchyData.Tables[0].Rows)
                    {
                        commonEntity.Add(new CommonEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return commonEntity;
            }

            return commonEntity;
        }

        public List<CommonEntity> Get_CellProvider()
        {
            List<CommonEntity> commonEntity = new List<CommonEntity>();
            try
            {
                DataSet hierarchyData = Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.CELLPHONEPROVIDR);
                if (hierarchyData != null && hierarchyData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in hierarchyData.Tables[0].Rows)
                    {
                        commonEntity.Add(new CommonEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return commonEntity;
            }

            return commonEntity;
        }


        /// <summary>
        /// Get Image Types information. 
        /// <returns>Returns a DataSet with the screens.</returns>
        public List<CommonEntity> GetResults()
        {
            List<CommonEntity> lookUpEntity = new List<CommonEntity>();
            try
            {
                DataSet lookUpData = Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.RESULTS);
                if (lookUpData != null && lookUpData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in lookUpData.Tables[0].Rows)
                    {
                        lookUpEntity.Add(new CommonEntity(row["Code"].ToString(), row["LookUpDesc"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                return lookUpEntity;
            }

            return lookUpEntity;
        }

        /// <summary>
        /// Get Image Types information. 
        /// <returns>Returns a DataSet with the screens.</returns>
        public List<CommonEntity> GetLegalIssues()
        {
            List<CommonEntity> lookUpEntity = new List<CommonEntity>();
            try
            {
                DataSet lookUpData = Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.LEGALISSUES);
                if (lookUpData != null && lookUpData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in lookUpData.Tables[0].Rows)
                    {
                        lookUpEntity.Add(new CommonEntity(row["Code"].ToString(), row["LookUpDesc"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                return lookUpEntity;
            }

            return lookUpEntity;
        }


        public List<CaseServicesEntity> GetSelectServices()
        {
            List<CaseServicesEntity> CaseSnpProfile = new List<CaseServicesEntity>();
            try
            {
                DataSet CaseSnp = Lookups.GetSelectServices();
                if (CaseSnp != null && CaseSnp.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseSnp.Tables[0].Rows)
                    {
                        CaseSnpProfile.Add(new CaseServicesEntity(row));
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

        public List<CommonEntity> GetIncomeInterval(string strHourMode, string strswitch)
        {
            List<CommonEntity> incomeInterval = new List<CommonEntity>();
            if (strswitch != "TX")
            {
                incomeInterval.Add(new CommonEntity("", "    "));
                incomeInterval.Add(new CommonEntity("E", "-----None-----"));
                incomeInterval.Add(new CommonEntity("O", "Other"));
                if (strHourMode == "Y")
                {
                    incomeInterval.Add(new CommonEntity("H", "Hourly"));
                }
                incomeInterval.Add(new CommonEntity("W", "Weekly"));
                incomeInterval.Add(new CommonEntity("B", "BiWeekly"));
                incomeInterval.Add(new CommonEntity("S", "Semi Monthly"));
                incomeInterval.Add(new CommonEntity("M", "Monthly"));
                incomeInterval.Add(new CommonEntity("N", "Semi Annual"));
                incomeInterval.Add(new CommonEntity("A", "Annual"));
                if (strHourMode != "Y")
                {
                    incomeInterval.Add(new CommonEntity("Q", "Quarterly"));
                }

            }
            else
            {
                incomeInterval.Add(new CommonEntity("", "    "));
                incomeInterval.Add(new CommonEntity("E", "-----None-----"));
                incomeInterval.Add(new CommonEntity("A", "Annual"));
                incomeInterval.Add(new CommonEntity("B", "BiWeekly"));

                if (strHourMode == "Y")
                {
                    incomeInterval.Add(new CommonEntity("H", "Hourly"));
                }
                incomeInterval.Add(new CommonEntity("M", "Monthly"));
                if (strHourMode != "Y")
                {
                    incomeInterval.Add(new CommonEntity("Q", "Quarterly"));
                }
                incomeInterval.Add(new CommonEntity("N", "Semi Annual"));
                incomeInterval.Add(new CommonEntity("S", "Semi Monthly"));
                incomeInterval.Add(new CommonEntity("W", "Weekly"));
                incomeInterval.Add(new CommonEntity("O", "Other"));
            }




            return incomeInterval;
        }

        public static string ShowIncomeInterval(string Type)
        {
            string strType = "";
            switch (Type)
            {
                case "E":
                    strType = "None";
                    break;
                case "O":
                    strType = "Other";
                    break;
                case "W":
                    strType = "Weekly";
                    break;
                case "B":
                    strType = "BiWeekly";
                    break;
                case "S":
                    strType = "Semi Monthly";
                    break;
                case "M":
                    strType = "Monthly";
                    break;
                case "N":
                    strType = "Semi Annual";
                    break;
                case "A":
                    strType = "Annual";
                    break;
                case "Q":
                    strType = "Quarterly";
                    break;
                case "3":
                    strType = "30 Days";
                    break;
                case "6":
                    strType = "60 Days";
                    break;
                case "9":
                    strType = "90 Days";
                    break;
            }
            return strType;

        }

        public static string GetMemberName(string strFirstName, string strMiddleName, string strLastName, string strType)
        {
            if (strType == "1")
            {
                if (string.IsNullOrEmpty(strMiddleName.Trim()))
                    return strFirstName.Trim() + " " + strLastName.Trim();
                else
                    return strFirstName.Trim() + " " + strMiddleName.Trim() + ". " + strLastName.Trim();
            }
            else if (strType == "3")
            {
                return strLastName.Trim() + "  " + strFirstName.Trim() + "  " + strMiddleName.Trim();
            }
            else if (strType == "4")
            {
                return strLastName.Trim() + "  " + strFirstName.Trim();
            }
            else if (strType == "5")
            {
                return strLastName.Trim() + "  " + strMiddleName.Trim() + "  " + strFirstName.Trim();
            }
            else
            {
                return strLastName.Trim() + ", " + strFirstName.Trim() + " " + strMiddleName.Trim();
            }
        }

        public static string Getdate(string strDate)
        {
            if (strDate != string.Empty)
            {
                return String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(strDate));
            }
            else
            {
                return string.Empty;
            }
        }
        public static string GetdateMM_DD_YYY(string strDate)
        {
            if (strDate != string.Empty)
            {
                return String.Format("{0:MM-dd-yyyy}", Convert.ToDateTime(strDate));
            }
            else
            {
                return string.Empty;
            }
        }

        public static string Getdate1(string strDate)
        {
            if (strDate != string.Empty)
            {
                return String.Format("{0:MM-dd-yyyy}", Convert.ToDateTime(strDate));
            }
            else
            {
                return string.Empty;
            }
        }

        public static string GetFormatdate(string strDate)
        {
            if (strDate.Length > 10)
                strDate = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(strDate)); //strDate.Substring(0, 10);

            string strLDate = strDate;
            if (strDate != string.Empty)
            {
                strDate = strDate.Replace("_", "").Trim();
                strDate = strDate.Replace(" ", "").Trim();
                strLDate = strDate;
                if (strDate.Length == 8)
                {
                    if (!strDate.Contains("/"))
                    {
                        strLDate = strDate.Substring(0, 2) + "/" + strDate.Substring(2, 2) + "/" + strDate.Substring(4, 4);
                    }

                }
                if (strDate.Length == 6)
                {
                    string[] strarrdate = strDate.Split('/');
                    if (strarrdate.Length == 3)
                    {
                        if ((Convert.ToInt16(strarrdate[2]) < 99) && (Convert.ToInt16(strarrdate[2]) > 60))
                        {
                            strLDate = strarrdate[0] + "/" + strarrdate[1] + "/19" + strarrdate[2];
                        }
                        else
                        {
                            strLDate = strarrdate[0] + "/" + strarrdate[1] + "/20" + strarrdate[2];
                        }
                    }
                    else
                    {

                        if ((Convert.ToInt16(strDate.Substring(4, 2)) < 99) && (Convert.ToInt16(strDate.Substring(4, 2)) > 60))
                        {
                            strLDate = strDate.Substring(0, 2) + "/" + strDate.Substring(2, 2) + "/19" + strDate.Substring(4, 2);
                        }
                        else
                        {
                            strLDate = strDate.Substring(0, 2) + "/" + strDate.Substring(2, 2) + "/20" + strDate.Substring(4, 2);
                        }
                    }
                }

            }
            return strLDate;
        }

        public static string Getdate2(string strDate)
        {
            if (strDate != string.Empty)
            {
                return String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(strDate));
            }
            else
            {
                return string.Empty;
            }
        }


        public static string GetdateYYYYMMDD(string strDate)
        {
            if (strDate != string.Empty)
            {
                return String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(strDate));
            }
            else
            {
                return string.Empty;
            }
        }

        public static string GetdateYYYYMMDD1(string strDate)
        {
            if (strDate != string.Empty)
            {
                return String.Format("{0:yyyyMMdd}", Convert.ToDateTime(strDate));
            }
            else
            {
                return string.Empty;
            }
        }

        public static string GetdatebyYear(string strDate)
        {
            if (strDate != string.Empty)
            {
                return String.Format("{0:MM/dd/yy}", Convert.ToDateTime(strDate));
            }
            else
            {
                return string.Empty;
            }
        }

        public static string GetdateYYMMDD(string strDate)
        {
            if (strDate != string.Empty)
            {
                return String.Format("{0:yyMMdd}", Convert.ToDateTime(strDate));
            }
            else
            {
                return string.Empty;
            }
        }

        public static string GetdateDDMMYY(string strDate)
        {
            if (strDate != string.Empty)
            {
                return String.Format("{0:ddMMyy}", Convert.ToDateTime(strDate));
            }
            else
            {
                return string.Empty;
            }
        }


        public static string GetLetterDate(string strDate)
        {
            if (strDate != string.Empty)
            {
                return String.Format("{0:MMMM dd, yyyy}", Convert.ToDateTime(strDate));
            }
            else
            {
                return string.Empty;
            }
        }

        public static string GetMonthNameDate(string strDate)
        {
            if (strDate != string.Empty)
            {
                return String.Format("{0:dd MMMM yyyy}", Convert.ToDateTime(strDate));
            }
            else
            {
                return string.Empty;
            }
        }

        public static string GetMonthDate(string strDate)
        {
            if (strDate != string.Empty)
            {
                return String.Format("{0:dd MMMM}", Convert.ToDateTime(strDate));
            }
            else
            {
                return string.Empty;
            }
        }

        public static string GetMonthandDate(string strDate)
        {
            if (strDate != string.Empty)
            {
                return String.Format("{0:MMMM dd}", Convert.ToDateTime(strDate));
            }
            else
            {
                return string.Empty;
            }
        }

        public static string GetTime(string strTime)
        {
            if (strTime != string.Empty)
            {
                return String.Format("{0:hh:mm tt}", Convert.ToDateTime(strTime));
            }
            else
            {
                return string.Empty;
            }
        }

        public static string GetPercentageTwodecimals(string strPercentage)
        {
            if (strPercentage != string.Empty)
            {
                return String.Format("{0:0.##}", Convert.ToDecimal(strPercentage));
            }
            else
            {
                return string.Empty;
            }
        }

        public static string GetAmountSep(string strAmount)
        {
            if (strAmount != string.Empty)
            {
                string AMount = Convert.ToDecimal(strAmount.Trim()).ToString("N", CultureInfo.InvariantCulture);
                return AMount;
            }
            else
            {
                return string.Empty;
            }
        }

        // GetCardNo function formats the SS# 
        public static string GetCardNo(string strCardNo, string strType, string strProgramReason, string strReasoncode)
        {
            if ((strCardNo != "000000000") || (strCardNo == "000000000") && (strProgramReason != "Y"))
            {
                if (strType == "1")
                {
                    if (strCardNo.Length > 5)
                    {
                        strCardNo = strCardNo.Substring(5);
                    }
                    return "nnn-nn-" + strCardNo;
                }
                else
                {
                    if (strCardNo.Length > 5)
                    {
                        strCardNo = strCardNo.Substring(5);
                    }
                    return "nnnnn" + strCardNo;

                }
            }
            else
            {
                AGYTABSEntity searchAgytabs = new AGYTABSEntity(true);
                searchAgytabs.Tabs_Type = "S0010";
                searchAgytabs.Table_Code = strReasoncode;
                AdhocData AgyTabs = new AdhocData();
                List<AGYTABSEntity> AgyTabs_List = AgyTabs.Browse_AGYTABS(searchAgytabs);
                if (AgyTabs_List.Count > 0)
                    return AgyTabs_List[0].Code_Desc.ToString();
                else
                {
                    if (strCardNo.Length > 5)
                    {
                        strCardNo = strCardNo.Substring(5);
                    }
                    return "nnn-nn-" + strCardNo;
                }
            }
        }

        public static string GetLookUpCode(string strType, string strcode)
        {
            DataSet lookUpData = Lookups.GetLookUpCode(strType, strcode);
            if (lookUpData != null && lookUpData.Tables[0].Rows.Count > 0)
            {
                return lookUpData.Tables[0].Rows[0]["LookUpDesc"].ToString();
            }
            else
                return string.Empty;
        }

        public static string GetPhoneSsnNoFormat(string strNumber)
        {
            string strFormatNumber = string.Empty;
            if (strNumber.Length == 10)
            {
                strFormatNumber = strNumber.Substring(0, 3) + "-" + strNumber.Substring(3, 3) + "-" + strNumber.Substring(6, 4);
            }
            else if (strNumber.Length == 9)
            {
                strFormatNumber = strNumber.Substring(0, 3) + "-" + strNumber.Substring(3, 2) + "-" + strNumber.Substring(5, 4);
            }
            return strFormatNumber;
        }


        public static string GetPhoneFormat(string strNumber)
        {
            string strFormatNumber = string.Empty;
            if (strNumber.Length == 10)
            {
                strFormatNumber = "(" + strNumber.Substring(0, 3) + ") " + strNumber.Substring(3, 3) + "-" + strNumber.Substring(6, 4);
            }
            return strFormatNumber;
        }
        public static string GetPhoneFormat2(string strNumber)
        {
            string strFormatNumber = string.Empty;
            if (strNumber.Length == 10)
            {
                strFormatNumber = strNumber.Substring(0, 3) + "-" + strNumber.Substring(3, 3) + "-" + strNumber.Substring(6, 4);
            }
            return strFormatNumber;
        }

        public string CheckDefaultHierachy(string strAgency, string strDept, string Program, string PWH_EMPLOYEE_NO)
        {
            string strsqlMsg = string.Empty;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                if (strAgency != string.Empty && strAgency != null)
                {
                    sqlParamList.Add(new SqlParameter("@AGENCY", strAgency));
                }
                if (strDept != string.Empty && strDept != null)
                {
                    sqlParamList.Add(new SqlParameter("@DEPT", strDept));
                }
                if (Program != string.Empty && Program != null)
                {
                    sqlParamList.Add(new SqlParameter("@PROGRAM", Program));
                }
                sqlParamList.Add(new SqlParameter("@PWH_EMPLOYEE_NO", PWH_EMPLOYEE_NO));

                SqlParameter parameterMsg = new SqlParameter("@KeyExists", SqlDbType.VarChar, 100);
                parameterMsg.Direction = ParameterDirection.Output;
                sqlParamList.Add(parameterMsg);
                UserAccess.CheckDefaultHierachy(sqlParamList);
                strsqlMsg = parameterMsg.Value.ToString();

            }
            catch (Exception ex)
            {
                //
                return strsqlMsg;
            }

            return strsqlMsg;
        }

        public string GetHierachyDescription(string Type, string Agency, string Dept, string Prog)
        {
            return UserAccess.GetHierachyDesc(Type, Agency, Dept, Prog);
        }

        public List<CommonEntity> GetReportTables()
        {
            List<CommonEntity> tables = new List<CommonEntity>();
            tables.Add(new CommonEntity("CASEMST", "Client Intake (Below member grid) and Income Verification"));
            tables.Add(new CommonEntity("CASESNP", "Client Intake (Applicant and member data)"));
            tables.Add(new CommonEntity("ADDCUST", "Custom Questions"));
            tables.Add(new CommonEntity("LIHEAPP", "Incomplete Letters"));
            tables.Add(new CommonEntity("LIHEAPB", "Basic Benefit"));
            tables.Add(new CommonEntity("LIHEAPV", "Primary Vendor Details"));
            tables.Add(new CommonEntity("PAYMENT", "Payment Details"));
            return tables;
        }

        public List<CommonEntity> GetReferldenialreasn()
        {
            List<CommonEntity> commonEntity = new List<CommonEntity>();
            try
            {
                DataSet hierarchyData = Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.REFERLDENIALREASN);
                if (hierarchyData != null && hierarchyData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in hierarchyData.Tables[0].Rows)
                    {
                        commonEntity.Add(new CommonEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return commonEntity;
            }

            return commonEntity;
        }

        public List<CommonEntity> GetREFERRALSTATUS()
        {
            List<CommonEntity> lookUpEntity = new List<CommonEntity>();
            try
            {
                DataSet lookUpData = Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.REFERRALSTATUS);
                if (lookUpData != null && lookUpData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in lookUpData.Tables[0].Rows)
                    {
                        lookUpEntity.Add(new CommonEntity(row["Code"].ToString(), row["LookUpDesc"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                return lookUpEntity;
            }

            return lookUpEntity;
        }

        public List<CommonEntity> GetMemAccess(string strH)
        {
            List<CommonEntity> MemAccess = new List<CommonEntity>();
            if (strH.Equals("CASEMST") || strH.Equals("H"))
            {
                MemAccess.Add(new CommonEntity("H", "Household"));
            }
            else
            {
                MemAccess.Add(new CommonEntity("*", "All Members"));
                MemAccess.Add(new CommonEntity("A", "Applicant"));
                MemAccess.Add(new CommonEntity("1", "Any One Member"));
            }

            return MemAccess;
        }

        public List<CommonEntity> GetConjunctions()
        {
            List<CommonEntity> Conjunctions = new List<CommonEntity>();
            Conjunctions.Add(new CommonEntity("A", "And"));
            Conjunctions.Add(new CommonEntity("O", "Or"));
            return Conjunctions;
        }


        public List<CommonEntity> GetLookkupFronAGYTAB(string strFilterCode)
        {
            List<CommonEntity> commonEntity = new List<CommonEntity>();
            try
            {

                DataSet AgyTabData = Lookups.GetLookUpFromAGYTAB(strFilterCode);
                if (AgyTabData != null && AgyTabData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in AgyTabData.Tables[0].Rows)
                    {
                        commonEntity.Add(new CommonEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return commonEntity;
            }

            return commonEntity;
        }

        public List<CommonEntity> GetJobTitle()
        {
            List<CommonEntity> commonEntity = new List<CommonEntity>();
            try
            {
                DataSet hierarchyData = Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.JOBTITLE);
                if (hierarchyData != null && hierarchyData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in hierarchyData.Tables[0].Rows)
                    {
                        commonEntity.Add(new CommonEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return commonEntity;
            }

            return commonEntity;
        }

        public List<CommonEntity> GetJobCategory()
        {
            List<CommonEntity> commonEntity = new List<CommonEntity>();
            try
            {
                DataSet hierarchyData = Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.JOBCATEGORY);
                if (hierarchyData != null && hierarchyData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in hierarchyData.Tables[0].Rows)
                    {
                        commonEntity.Add(new CommonEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return commonEntity;
            }

            return commonEntity;
        }


        public List<CommonEntity> GetInsuranceCategory()
        {
            List<CommonEntity> commonEntity = new List<CommonEntity>();
            try
            {
                DataSet hierarchyData = Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.STATEMEDICALCATEGRS);
                if (hierarchyData != null && hierarchyData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in hierarchyData.Tables[0].Rows)
                    {
                        commonEntity.Add(new CommonEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return commonEntity;
            }

            return commonEntity;
        }

        public List<CommonEntity> GetPositionCode()
        {
            List<CommonEntity> commonEntity = new List<CommonEntity>();
            try
            {
                DataSet hierarchyData = Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.POSITIONCODS);
                if (hierarchyData != null && hierarchyData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in hierarchyData.Tables[0].Rows)
                    {
                        commonEntity.Add(new CommonEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return commonEntity;
            }

            return commonEntity;
        }


        public List<CommonEntity> GetCmbClsPrefer()
        {
            List<CommonEntity> months = new List<CommonEntity>();
            months.Add(new CommonEntity("N", "NONE"));
            months.Add(new CommonEntity("A", "AM"));
            months.Add(new CommonEntity("P", "PM"));
            months.Add(new CommonEntity("F", "FULL DAY"));
            months.Add(new CommonEntity("E", "EXTENDED"));
            return months;
        }

        public List<CommonEntity> GetCMBTransport(string strCode)
        {
            List<CommonEntity> months = new List<CommonEntity>();
            months.Add(new CommonEntity("N", "NONE"));
            months.Add(new CommonEntity("A", "Agency"));
            months.Add(new CommonEntity("P", "Parent/Guardian"));
            if (strCode.ToUpper() == "COI")
            {
                months.Add(new CommonEntity("J", "JPS"));
                months.Add(new CommonEntity("D", "Dunkirk"));
                months.Add(new CommonEntity("F", "Foster"));
            }
            return months;
        }

        public List<CommonEntity> GetCmbRelations()
        {
            List<CommonEntity> relations = new List<CommonEntity>();
            relations.Add(new CommonEntity("N", "NONE"));
            relations.Add(new CommonEntity("R", "Relative"));
            relations.Add(new CommonEntity("P", "Parent"));
            relations.Add(new CommonEntity("G", "Guardian"));
            relations.Add(new CommonEntity("F", "Friend"));
            return relations;
        }

        public List<CommonEntity> GetAgyTabRecordsByCode(string strCode)
        {
            List<CommonEntity> commonEntity = new List<CommonEntity>();
            try
            {
                DataSet PrimaryLanguageData = Lookups.GetLookUpFromAGYTAB(strCode);
                if (PrimaryLanguageData != null && PrimaryLanguageData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in PrimaryLanguageData.Tables[0].Rows)
                    {
                        commonEntity.Add(new CommonEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return commonEntity;
            }

            return commonEntity;
        }

        public List<CommonEntity> GetAgyTabRecordsByCodefilter(string strCode, string strfilter)
        {
            List<CommonEntity> commonEntity = new List<CommonEntity>();
            try
            {
                DataSet PrimaryLanguageData = Lookups.GetLookUpFromAGYTAB(strCode, strfilter);
                if (PrimaryLanguageData != null && PrimaryLanguageData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in PrimaryLanguageData.Tables[0].Rows)
                    {
                        commonEntity.Add(new CommonEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return commonEntity;
            }

            return commonEntity;
        }

        public List<CommonEntity> GetAgyTabRecordsByCodefilterFunds(string strCode, string strfilter)
        {
            List<CommonEntity> commonEntity = new List<CommonEntity>();
            try
            {
                DataSet PrimaryLanguageData = Lookups.GetLookUpFromAGYTAB(strCode, strfilter);
                if (PrimaryLanguageData != null && PrimaryLanguageData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in PrimaryLanguageData.Tables[0].Rows)
                    {
                        commonEntity.Add(new CommonEntity(row, "Funds"));
                    }
                }
            }
            catch (Exception ex)
            {
                return commonEntity;
            }

            return commonEntity;
        }

        public string getServerPath()
        {
            string strReportPath = Consts.Common.ServerLocation;
            if (Consts.Common.ServerLocation == @"\\SYSTEM-1\" || Consts.Common.ServerLocation == @"\\CLSYS-1\" || Consts.Common.ServerLocation == @"\\CLSYS-2\" || Consts.Common.ServerLocation == @"\\CLSYS-5\|| Consts.Common.ServerLocation == @""\\KRANTHI\" || Consts.Common.ServerLocation == @"\\\\KRANTHI\\")
                strReportPath = Application.MapPath(strReportPath);

            return strReportPath;
        }

        public string GetReportPath()
        {
            string strReportPath = string.Empty;

            DataSet AgencyData = Captain.DatabaseLayer.ZipCodePlusAgency.GetAgencyControlDetails("00");
            if (AgencyData != null && AgencyData.Tables[0].Rows.Count > 0)
            {
                strReportPath = Consts.Common.ServerLocation + AgencyData.Tables[0].Rows[0]["ACR_PATH"].ToString().Trim();
                if (Consts.Common.ServerLocation == @"\\SYSTEM-1\" || Consts.Common.ServerLocation == @"\\\\SYSTEM-1\\" || Consts.Common.ServerLocation == @"\\CLSYS-1\" || Consts.Common.ServerLocation == @"\\CLSYS-2\" || Consts.Common.ServerLocation == @"\\CLSYS-5\"|| Consts.Common.ServerLocation == @"\\KRANTHI\" || Consts.Common.ServerLocation == @"\\\\KRANTHI\\" || Consts.Common.ServerLocation == @"\\CAPLOGICS\" || Consts.Common.ServerLocation == @"\\\\CAPLOGICS\\")
                {
                    strReportPath = "C:\\" + strReportPath; // Application.MapPath(strReportPath);
                }
            }

            return strReportPath;
        }
        public string GetServerReportPath()
        {
            string strReportPath = string.Empty;

            DataSet AgencyData = Captain.DatabaseLayer.ZipCodePlusAgency.GetAgencyControlDetails("00");
            if (AgencyData != null && AgencyData.Tables[0].Rows.Count > 0)
            {
                strReportPath = Consts.Common.ServerLocation + AgencyData.Tables[0].Rows[0]["ACR_PATH"].ToString().Trim();
                if (Consts.Common.ServerLocation == @"\\SYSTEM-1\" || Consts.Common.ServerLocation == @"\\\\SYSTEM-1\\" || Consts.Common.ServerLocation == @"\\CLSYS-1\" || Consts.Common.ServerLocation == @"\\CLSYS-2\" || Consts.Common.ServerLocation == @"\\CLSYS-5\"|| Consts.Common.ServerLocation == @"\\KRANTHI\"|| Consts.Common.ServerLocation == @"\\\\KRANTHI\\")
                {
                    strReportPath = "C:\\" + strReportPath;
                }
            }

            return strReportPath;
        }
        public string GetReportDirectPath()
        {
            string strReportPath = string.Empty;

            DataSet AgencyData = Captain.DatabaseLayer.ZipCodePlusAgency.GetAgencyControlDetails("00");
            if (AgencyData != null && AgencyData.Tables[0].Rows.Count > 0)
            {
                strReportPath = AgencyData.Tables[0].Rows[0]["ACR_PATH"].ToString().Trim();
            }

            return strReportPath;
        }
        public string checkPathLrS(string strPath)
        {
            string strReportPath = string.Empty;
            try
            {
                if (Consts.Common.ServerLocation == @"\\SYSTEM-1\" || Consts.Common.ServerLocation == @"\\CLSYS-1\" || Consts.Common.ServerLocation == @"\\CLSYS-2\"|| Consts.Common.ServerLocation == @"\\KRANTHI\"|| Consts.Common.ServerLocation == @"\\\\KRANTHI\\")
                    strReportPath = Application.MapPath(strPath);
                else
                    strReportPath = strPath;
            }
            catch { }

            return strReportPath;
        }

        public List<CommonEntity> GetAttandanceReason()
        {
            List<CommonEntity> incomeInterval = new List<CommonEntity>();
            incomeInterval.Add(new CommonEntity(" ", " "));
            incomeInterval.Add(new CommonEntity("1", "Arriving Late"));
            incomeInterval.Add(new CommonEntity("2", "Leaving Early"));
            incomeInterval.Add(new CommonEntity("3", "Doctor/Dentist"));
            incomeInterval.Add(new CommonEntity("4", "Other"));
            incomeInterval.Add(new CommonEntity("5", "Shared Placement"));
            return incomeInterval;
        }
        public List<CommonEntity> GetAttandanceFillempty()
        {
            List<CommonEntity> incomeInterval = new List<CommonEntity>();
            incomeInterval.Add(new CommonEntity("", "    "));
            return incomeInterval;
        }


        public List<CommonEntity> GetChildAge(string agency, string dept, string program, string year, string ApplNo, string Dob, string Type)
        {
            List<CommonEntity> commonEntity = new List<CommonEntity>();
            try
            {
                DataSet ChildAgeYear = Lookups.GetChildAge(agency, dept, program, year, ApplNo, Dob, Type);
                if (ChildAgeYear != null && ChildAgeYear.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ChildAgeYear.Tables[0].Rows)
                    {
                        commonEntity.Add(new CommonEntity(row["Years"].ToString(), row["Months"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                return commonEntity;
            }

            return commonEntity;
        }

        public int GetAgeCalculationMonths(DateTime startDate, DateTime EndDate)
        {
            return (((EndDate.Year - startDate.Year) * 12) + (EndDate.Month - startDate.Month));
        }

        public DateTime GetEndDateAgeCalculation(string Type, CaseMstEntity caseMst, List<ZipCodeEntity> propzipCodeEntity)
        {
            DateTime EndDate = DateTime.Now.Date;
            if (Type == "T")
            {
                EndDate = DateTime.Now.Date;
            }
            else if (Type == "I")
            {
                EndDate = Convert.ToDateTime(caseMst.IntakeDate);
            }
            else if (Type == "K")
            {
                string strDate = DateTime.Now.Date.ToShortDateString();
                string strYear;
                ZipCodeEntity zipentity = propzipCodeEntity.Find(u => u.Zcrzip.Trim().Equals(caseMst.Zip.Trim()));
                if (zipentity != null)
                {
                    if (zipentity.Zcrhssyear.Trim() == "2")
                    {
                        strYear = DateTime.Now.AddYears(1).Year.ToString();
                    }
                    else
                    {
                        strYear = DateTime.Now.Year.ToString();
                    }
                    strDate = zipentity.Zcrhssmo + "/" + zipentity.Zcrhssday + "/" + strYear;
                }
                EndDate = Convert.ToDateTime(strDate);
            }
            return EndDate;
        }

        public List<CommonEntity> GetSequence2530()
        {
            List<CommonEntity> sequenceDetails = new List<CommonEntity>();
            //sequenceDetails.Add(new CommonEntity("0", " "));
            sequenceDetails.Add(new CommonEntity("1", "Name"));
            sequenceDetails.Add(new CommonEntity("2", "Address"));
            sequenceDetails.Add(new CommonEntity("3", "RankValue(Ascending)"));
            sequenceDetails.Add(new CommonEntity("4", "RankValue(Descending)"));
            sequenceDetails.Add(new CommonEntity("5", "Date(Ascending)"));
            sequenceDetails.Add(new CommonEntity("6", "Date(Descending)"));
            sequenceDetails.Add(new CommonEntity("7", "Hierachy"));
            return sequenceDetails;
        }

        public List<CommonEntity> GetHssb2108FormActiveDetails()
        {
            List<CommonEntity> activeDetails = new List<CommonEntity>();
            activeDetails.Add(new CommonEntity("Y", "Active"));
            activeDetails.Add(new CommonEntity("N", "Inactive"));
            activeDetails.Add(new CommonEntity("B", "Both"));
            return activeDetails;
        }

        public List<CommonEntity> GetHssb2103FormTaskDetails()
        {
            List<CommonEntity> taskDetails = new List<CommonEntity>();
            taskDetails.Add(new CommonEntity("1", "10 Tasks (Condensed Report)"));
            taskDetails.Add(new CommonEntity("2", "6 Tasks (More Data)"));
            return taskDetails;
        }

        public List<CommonEntity> GetHssb2114FormFundings()
        {
            List<CommonEntity> taskDetails = new List<CommonEntity>();
            taskDetails.Add(new CommonEntity("H", "Head Start"));
            taskDetails.Add(new CommonEntity("D", "Day Care"));
            taskDetails.Add(new CommonEntity("A", "All Funding Sources"));
            return taskDetails;
        }

        public List<CommonEntity> GetHssb00133TaskConfiguration()
        {
            List<CommonEntity> taskConfiguration = new List<CommonEntity>();
            taskConfiguration.Add(new CommonEntity("QU", "Question"));
            taskConfiguration.Add(new CommonEntity("CA", "Casenotes"));
            taskConfiguration.Add(new CommonEntity("SB", "SBCB"));
            taskConfiguration.Add(new CommonEntity("AD", "Addressed"));
            taskConfiguration.Add(new CommonEntity("CO", "Completed"));
            taskConfiguration.Add(new CommonEntity("FO", "Followup"));
            taskConfiguration.Add(new CommonEntity("FC", "Followup Completed"));
            taskConfiguration.Add(new CommonEntity("DI", "Diagnosed"));
            taskConfiguration.Add(new CommonEntity("SP", "Special Service Referral"));
            taskConfiguration.Add(new CommonEntity("AG", "Agency Referral"));
            taskConfiguration.Add(new CommonEntity("FD", "Fund"));
            return taskConfiguration;
        }

        public List<CommonEntity> GetHss02001Sequences()
        {
            List<CommonEntity> hss02001Sequence = new List<CommonEntity>();
            hss02001Sequence.Add(new CommonEntity("A", "Application#"));
            hss02001Sequence.Add(new CommonEntity("C", "Client Name"));
            hss02001Sequence.Add(new CommonEntity("S", "Site"));
            hss02001Sequence.Add(new CommonEntity("F", "Fund"));
            hss02001Sequence.Add(new CommonEntity("L", "Language"));
            return hss02001Sequence;
        }


        public List<CommonEntity> GetHss02001PreEnrolled()
        {
            List<CommonEntity> hss02001status = new List<CommonEntity>();
            hss02001status.Add(new CommonEntity("P", "Pre-Enrolled"));
            hss02001status.Add(new CommonEntity("N", "Not Pre-Enrolled"));
            hss02001status.Add(new CommonEntity("E", "Enrolled"));
            hss02001status.Add(new CommonEntity("W", "Not Enrolled/Withdrawn"));
            return hss02001status;
        }


        public List<CommonEntity> GetHSS20001Menus()
        {
            List<CommonEntity> hss02001status = new List<CommonEntity>();
            hss02001status.Add(new CommonEntity("A", "Add Staff Member Association"));
            hss02001status.Add(new CommonEntity("R", "Remove Staff Member Association"));
            hss02001status.Add(new CommonEntity("All", "Add Staff Member To All"));
            hss02001status.Add(new CommonEntity("RAll", "Remove Staff Member From All"));
            return hss02001status;
        }


        public List<CommonEntity> GetAgyTabs(string agytype, string agycode, string agydesc)
        {
            List<CommonEntity> commonEntity = new List<CommonEntity>();
            try
            {
                DataSet AgytabsData = Lookups.GetAgyTabs(agytype, agycode, agydesc);
                if (AgytabsData != null && AgytabsData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in AgytabsData.Tables[0].Rows)
                    {
                        commonEntity.Add(new CommonEntity(row, "AGYTABS"));
                    }
                }
            }
            catch (Exception ex)
            {
                return commonEntity;
            }

            return commonEntity;
        }

        public List<CommonEntity> GetAgyTabspanish(string agytype, string agycode, string agydesc)
        {
            List<CommonEntity> commonEntity = new List<CommonEntity>();
            try
            {
                DataSet AgytabsData = Lookups.GetAgyTabs(agytype, agycode, agydesc);
                if (AgytabsData != null && AgytabsData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in AgytabsData.Tables[0].Rows)
                    {
                        commonEntity.Add(new CommonEntity(row, "SPANISH"));
                    }
                }
            }
            catch (Exception ex)
            {
                return commonEntity;
            }

            return commonEntity;
        }

        public List<CommonEntity> GetAgyTabsHeaders(string agytype, string agycode, string agydesc, string tableType)
        {
            List<CommonEntity> commonEntity = new List<CommonEntity>();
            try
            {
                DataSet AgytabsData = Lookups.GetAgyTabs(agytype, agycode, agydesc);
                if (AgytabsData != null && AgytabsData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in AgytabsData.Tables[0].Rows)
                    {
                        commonEntity.Add(new CommonEntity(row, tableType));
                    }
                }
            }
            catch (Exception ex)
            {
                return commonEntity;
            }

            return commonEntity;
        }

        public List<CommonEntity> GetReportSelection()
        {
            List<CommonEntity> selection = new List<CommonEntity>();
            selection.Add(new CommonEntity("A", "All Clients"));
            selection.Add(new CommonEntity("B", "All Clients (Select Vendors)"));
            selection.Add(new CommonEntity("C", "Eligible Clients"));
            selection.Add(new CommonEntity("D", "Eligible Clients (Select Vendors)"));
            return selection;
        }



        public List<CommonEntity> GetCapAppl()
        {
            List<CommonEntity> commonEntity = new List<CommonEntity>();
            try
            {
                DataSet CapAppl = Lookups.GetCapAppl();
                if (CapAppl != null && CapAppl.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CapAppl.Tables[0].Rows)
                    {
                        commonEntity.Add(new CommonEntity(row["APPL_CODE"].ToString(), row["APPL_DESCRIPTION"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                return commonEntity;
            }

            return commonEntity;
        }

        /// <summary>
        /// Get Funds Types information. 
        /// <returns>Returns a DataSet with the screens.</returns>
        public List<CommonEntity> GetAgyFunds()
        {
            List<CommonEntity> lookUpEntity = new List<CommonEntity>();
            try
            {
                DataSet lookUpData = Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.FUNDCODS);
                if (lookUpData != null && lookUpData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in lookUpData.Tables[0].Rows)
                    {
                        lookUpEntity.Add(new CommonEntity(row["Code"].ToString(), row["LookUpDesc"].ToString(), row["Hierarchy"].ToString(), row["EXT"].ToString(), row["AGY_DEFAULT"].ToString(), row["Active"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                return lookUpEntity;
            }

            return lookUpEntity;
        }

        /// <summary>
        /// Get Decision Types information. 
        /// <returns>Returns a DataSet with the screens.</returns>
        public List<CommonEntity> GetAgyDecision()
        {
            List<CommonEntity> lookUpEntity = new List<CommonEntity>();
            try
            {
                DataSet lookUpData = Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.DecisionType);
                if (lookUpData != null && lookUpData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in lookUpData.Tables[0].Rows)
                    {
                        lookUpEntity.Add(new CommonEntity(row["Code"].ToString(), row["LookUpDesc"].ToString(), row["Hierarchy"].ToString(), row["EXT"].ToString(), row["AGY_DEFAULT"].ToString(), row["Active"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                return lookUpEntity;
            }

            return lookUpEntity;
        }


        /// <summary>
        /// Get Funds Types  information Filter Based on screens . 
        /// <returns>Returns a DataSet with the screens.</returns>
        public List<CommonEntity> GetAgyFundsFilter(string strFilterType, string strDisplayFunds)
        {
            List<CommonEntity> lookUpEntity = new List<CommonEntity>();
            try
            {
                DataSet lookUpData = Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.FUNDINGSOURCES, strFilterType);
                if (lookUpData != null && lookUpData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in lookUpData.Tables[0].Rows)
                    {
                        lookUpEntity.Add(new CommonEntity(row["Code"].ToString(), row["LookUpDesc"].ToString(), row["Hierarchy"].ToString(), row["EXT"].ToString(), row["AGY_DEFAULT"].ToString(), row["Active"].ToString()));
                    }

                    if (strDisplayFunds == "Y")
                    {
                        lookUpEntity = lookUpEntity.FindAll(u => u.Extension.ToString() == strDisplayFunds);
                    }
                    else if (strDisplayFunds == "N")
                    {
                        lookUpEntity = lookUpEntity.FindAll(u => u.Extension.ToString() == strDisplayFunds);
                    }
                }
            }
            catch (Exception ex)
            {
                return lookUpEntity;
            }

            return lookUpEntity;
        }


        /// <summary>
        /// Get Funds Types information. 
        /// <returns>Returns a DataSet with the screens.</returns>
        public List<CommonEntity> GetCategrcalEligiblityNew()
        {
            List<CommonEntity> lookUpEntity = new List<CommonEntity>();
            try
            {
                DataSet lookUpData = Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.CATEGRCALELIGIBLTY);
                if (lookUpData != null && lookUpData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in lookUpData.Tables[0].Rows)
                    {
                        lookUpEntity.Add(new CommonEntity(row["Code"].ToString(), row["LookUpDesc"].ToString(), row["Hierarchy"].ToString(), row["EXT"].ToString(), row["AGY_DEFAULT"].ToString(), row["Active"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                return lookUpEntity;
            }

            return lookUpEntity;
        }


        public static string decimal2value(string strnum)
        {
            string decdata = strnum;
            int length = decdata.Length;
            if (decdata.Contains("."))
            {
                int sublenth = decdata.IndexOf('.');
                if (length > sublenth + 3)
                    decdata = decdata.Substring(0, sublenth + 3);
            }
            return decdata;
        }

        /// <summary>
        /// Get Funds Types information. 
        /// <returns>Returns a DataSet with the screens.</returns>
        public List<CommonEntity> GetDimension()
        {
            List<CommonEntity> lookUpEntity = new List<CommonEntity>();
            try
            {
                DataSet lookUpData = Lookups.GetLookUpFromAGYTAB("07001");
                if (lookUpData != null && lookUpData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in lookUpData.Tables[0].Rows)
                    {
                        lookUpEntity.Add(new CommonEntity(row["Code"].ToString(), row["LookUpDesc"].ToString(), row["Hierarchy"].ToString(), row["EXT"].ToString(), row["AGY_DEFAULT"].ToString(), row["Active"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                return lookUpEntity;
            }

            return lookUpEntity;
        }


        public List<CommonEntity> GETRDLCALLDATA(string strUserId, string strstartDate, string strEndDate, string strtbtype, string strType)
        {
            List<CommonEntity> lookUpEntity = new List<CommonEntity>();
            try
            {
                DataSet lookUpData = DashBoardDB.GETRDLCALLDATA(strUserId, strstartDate, strEndDate, strtbtype.ToString(), "USERTYPE"); ;
                if (lookUpData != null && lookUpData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in lookUpData.Tables[0].Rows)
                    {
                        lookUpEntity.Add(new CommonEntity(row["UserId"].ToString(), row["TBType"].ToString(), string.Empty, row["TotalCount"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                return lookUpEntity;
            }

            return lookUpEntity;
        }

        public static string HKLM_GetString(string path, string key)
        {
            try
            {
                RegistryKey rk = Registry.LocalMachine.OpenSubKey(path);
                if (rk == null) return "";
                return (string)rk.GetValue(key);
            }
            catch { return ""; }
        }

        public static string FriendlyName()
        {
            string ProductName = HKLM_GetString(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ProductName");
            string CSDVersion = HKLM_GetString(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", "CSDVersion");
            if (ProductName != "")
            {
                return (ProductName.StartsWith("Microsoft") ? "" : "Microsoft ") + ProductName +
                            (CSDVersion != "" ? " " + CSDVersion : "");
            }
            return "";
        }

        public static string GetResponceCode(string Responce)
        {
            string strResponce = "";
            switch (Responce)
            {
                case "A":
                case "a":
                    strResponce = "1";
                    break;
                case "B":
                case "b":
                    strResponce = "2";
                    break;
                case "C":
                case "c":
                    strResponce = "3";
                    break;
                case "D":
                case "d":
                    strResponce = "4";
                    break;
                case "E":
                case "e":
                    strResponce = "5";
                    break;
                case "F":
                case "f":
                    strResponce = "6";
                    break;
                case "G":
                case "g":
                    strResponce = "7";
                    break;
                case "H":
                case "h":
                    strResponce = "8";
                    break;
                case "I":
                case "i":
                    strResponce = "9";
                    break;
                case "J":
                case "j":
                    strResponce = "10";
                    break;
                case "K":
                case "k":
                    strResponce = "11";
                    break;
                case "L":
                case "l":
                    strResponce = "12";
                    break;
                case "M":
                case "m":
                    strResponce = "13";
                    break;
                case "N":
                case "n":
                    strResponce = "2";
                    break;
                case "O":
                case "o":
                    strResponce = "15";
                    break;
                case "P":
                case "p":
                    strResponce = "16";
                    break;
                case "Q":
                case "q":
                    strResponce = "17";
                    break;
                case "R":
                case "r":
                    strResponce = "18";
                    break;
                case "S":
                case "s":
                    strResponce = "19";
                    break;
                case "T":
                case "t":
                    strResponce = "20";
                    break;
                case "U":
                case "u":
                    strResponce = "21";
                    break;
                case "V":
                case "v":
                    strResponce = "22";
                    break;
                case "W":
                case "w":
                    strResponce = "23";
                    break;
                case "X":
                case "x":
                    strResponce = "24";
                    break;
                case "Y":
                case "y":
                    strResponce = "1";
                    break;
                case "Z":
                case "z":
                    strResponce = "26";
                    break;
                default: strResponce = Responce; break;
            }
            return strResponce;

        }
        public List<ListItem> GetAllCaseworker()
        {
            List<ListItem> listWorker = new List<Utilities.ListItem>();
            DataSet cwDataSet = Captain.DatabaseLayer.CaseMst.GetAllCaseWorkers(string.Empty);
            if (cwDataSet != null && cwDataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in cwDataSet.Tables[0].Rows)
                    listWorker.Add(new Captain.Common.Utilities.ListItem(dr["NAME"].ToString().Trim(), dr["PWH_CASEWORKER"].ToString().Trim()));

            }
            return listWorker;
        }

        public List<CommonEntity> GetAgyFamilyTypes()
        {
            List<CommonEntity> lookUpEntity = new List<CommonEntity>();
            try
            {
                DataSet lookUpData = Lookups.GetLookUpFromAGYTAB(Consts.AgyTab.FAMILYTYPE);
                if (lookUpData != null && lookUpData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in lookUpData.Tables[0].Rows)
                    {
                        lookUpEntity.Add(new CommonEntity(row["Code"].ToString(), row["LookUpDesc"].ToString(), row["Hierarchy"].ToString(), row["EXT"].ToString(), row["AGY_DEFAULT"].ToString(), row["Active"].ToString(), row["Cell2"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                return lookUpEntity;
            }

            return lookUpEntity;
        }

        /// <summary>
        /// Get Funds Types information. 
        /// <returns>Returns a DataSet with the screens.</returns>
        public List<CommonEntity> GetPaymentCategoryServicee()
        {
            List<CommonEntity> lookUpEntity = new List<CommonEntity>();
            try
            {
                DataSet lookUpData = Lookups.GetLookUpFromAGYTAB("00201");
                if (lookUpData != null && lookUpData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in lookUpData.Tables[0].Rows)
                    {
                        lookUpEntity.Add(new CommonEntity(row["Code"].ToString(), row["LookUpDesc"].ToString(), row["Hierarchy"].ToString(), row["EXT"].ToString(), row["AGY_DEFAULT"].ToString(), row["Active"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                return lookUpEntity;
            }

            return lookUpEntity;
        }


        public static void Hide_ScreenPrivilages_checkboxes(PrivilegeEntity privilegeEntity, DataGridView gvwControl, int rowIndex)
        {

            if (privilegeEntity.Program.Equals("ADMN0005") && privilegeEntity.ModuleCode.Equals("01"))
            {
                gvwControl.Rows[rowIndex].Cells["Delete"].Style.Padding = new Padding(gvwControl.Rows[rowIndex].Cells["Delete"].OwningColumn.Width, 0, 0, 0);
            }
            if (privilegeEntity.Program.Equals("SCHAUDIT"))
            {
                gvwControl.Rows[rowIndex].Cells["Add"].Style.Padding = new Padding(gvwControl.Rows[rowIndex].Cells["Add"].OwningColumn.Width, 0, 0, 0);
                gvwControl.Rows[rowIndex].Cells["Change"].Style.Padding = new Padding(gvwControl.Rows[rowIndex].Cells["Change"].OwningColumn.Width, 0, 0, 0);
                gvwControl.Rows[rowIndex].Cells["Delete"].Style.Padding = new Padding(gvwControl.Rows[rowIndex].Cells["Delete"].OwningColumn.Width, 0, 0, 0);
            }
            if (privilegeEntity.Program.Equals("MAT00001"))
            {
                gvwControl.Rows[rowIndex].Cells["Add"].Style.Padding = new Padding(gvwControl.Rows[rowIndex].Cells["Add"].OwningColumn.Width, 0, 0, 0);
                gvwControl.Rows[rowIndex].Cells["Change"].Style.Padding = new Padding(gvwControl.Rows[rowIndex].Cells["Change"].OwningColumn.Width, 0, 0, 0);
                gvwControl.Rows[rowIndex].Cells["Delete"].Style.Padding = new Padding(gvwControl.Rows[rowIndex].Cells["Delete"].OwningColumn.Width, 0, 0, 0);
            }
            if (privilegeEntity.Program.Equals("TRIGBULK"))
            {
                gvwControl.Rows[rowIndex].Cells["Add"].Style.Padding = new Padding(gvwControl.Rows[rowIndex].Cells["Add"].OwningColumn.Width, 0, 0, 0);
                gvwControl.Rows[rowIndex].Cells["Change"].Style.Padding = new Padding(gvwControl.Rows[rowIndex].Cells["Change"].OwningColumn.Width, 0, 0, 0);
                gvwControl.Rows[rowIndex].Cells["Delete"].Style.Padding = new Padding(gvwControl.Rows[rowIndex].Cells["Delete"].OwningColumn.Width, 0, 0, 0);
            }
            if (privilegeEntity.Program.Equals("TMSTRIGG"))
            {
                gvwControl.Rows[rowIndex].Cells["Add"].Style.Padding = new Padding(gvwControl.Rows[rowIndex].Cells["Add"].OwningColumn.Width, 0, 0, 0);
                gvwControl.Rows[rowIndex].Cells["Change"].Style.Padding = new Padding(gvwControl.Rows[rowIndex].Cells["Change"].OwningColumn.Width, 0, 0, 0);
                gvwControl.Rows[rowIndex].Cells["Delete"].Style.Padding = new Padding(gvwControl.Rows[rowIndex].Cells["Delete"].OwningColumn.Width, 0, 0, 0);
            }
            if (privilegeEntity.Program.Equals("CASE9006"))
            {
                gvwControl.Rows[rowIndex].Cells["Change"].Style.Padding = new Padding(gvwControl.Rows[rowIndex].Cells["Change"].OwningColumn.Width, 0, 0, 0);
                gvwControl.Rows[rowIndex].Cells["Delete"].Style.Padding = new Padding(gvwControl.Rows[rowIndex].Cells["Delete"].OwningColumn.Width, 0, 0, 0);
            }
            if (privilegeEntity.Program.Equals("CASE9016"))
            {
                gvwControl.Rows[rowIndex].Cells["Delete"].Style.Padding = new Padding(gvwControl.Rows[rowIndex].Cells["Delete"].OwningColumn.Width, 0, 0, 0);
            }
            if (privilegeEntity.Program.Equals("TMS00085"))
            {
                gvwControl.Rows[rowIndex].Cells["Add"].Style.Padding = new Padding(gvwControl.Rows[rowIndex].Cells["Add"].OwningColumn.Width, 0, 0, 0);
                gvwControl.Rows[rowIndex].Cells["Change"].Style.Padding = new Padding(gvwControl.Rows[rowIndex].Cells["Change"].OwningColumn.Width, 0, 0, 0);
                gvwControl.Rows[rowIndex].Cells["Delete"].Style.Padding = new Padding(gvwControl.Rows[rowIndex].Cells["Delete"].OwningColumn.Width, 0, 0, 0);
            }
        }

        public static string GetBenefitTypeDesc(string strCode)
        {
            string strDesc = string.Empty;
            switch (strCode)
            {
                case "1":
                    strDesc = "Household Crisis Component (HCC)";
                    break;
                case "2":
                    strDesc = "Utility Assistance Component(UAC)";
                    break;
                case "3":
                    strDesc = "LIHEAP Weatherization";
                    break;
                case "4":
                    strDesc = "DOE Weatherization";
                    break;
                case "6":
                    strDesc = "LIHWAP ARP";
                    break;
                case "7":
                    strDesc = "LIHWAP CAA";
                    break;
            }
            return strDesc;
        }
    }
}
