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

    public class HierarchyAndPrograms
    {

        public HierarchyAndPrograms(CaptainModel model)
        {
            Model = model;
        }

        #region Properties

        public CaptainModel Model { get; set; }

        public string UserId { get; set; }

        #endregion

        /// <summary>
        /// Get Hierarchy information. 
        /// </summary>
        /// <param name="userID">The user ID to get user profile.</param>
        /// <returns>Returns a DataSet with the screens.</returns>
        public List<HierarchyEntity> GetCaseHierarchy(string FilterBy, string Agency, string Dept)
        {
            List<HierarchyEntity> caseHierarchyEntity = new List<HierarchyEntity>();
            try
            {
                DataSet hierarchyData = Lookups.GetHierarchies(FilterBy, Agency, Dept, string.Empty, string.Empty,string.Empty);
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
        public List<ProgramDefinitionEntity> GetPrograms(string programName, string hierarchy)
        {
            List<ProgramDefinitionEntity> programEntity = new List<ProgramDefinitionEntity>();
            try
            {
                DataSet programData = Captain.DatabaseLayer.HierarchyPlusProgram.GetProgramsSearch(programName, hierarchy);
                if (programData != null && programData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in programData.Tables[0].Rows)
                    {
                        programEntity.Add(new ProgramDefinitionEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return programEntity;
            }

            return programEntity;
        }
        /// <summary>
        /// Get User screens information. 
        /// </summary>
        /// <param name="userID">The user ID to get user profile.</param>
        /// <returns>Returns a DataSet with the screens.</returns>
        public HierarchyEntity GetCaseHierarchy(string FilterBy, string Agency, string Dept, string Program, string UserId,string AdminAgency)
        {
            HierarchyEntity caseHierarchyEntity = null;
            try
            {
                DataSet hierarchyData = Lookups.GetHierarchies(FilterBy, Agency, Dept, Program, UserId,AdminAgency);
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


        /// <summary>
        /// Get User screens information. 
        /// </summary>
        /// <param name="userID">The user ID to get user profile.</param>
        /// <returns>Returns a DataSet with the screens.</returns>
        public HierarchyEntity GetCaseHierarchyName(string Agency, string Dept, string Program)
        {
            HierarchyEntity caseHierarchyEntity = null;
            try
            {
                DataSet hierarchyData = AgyTab.GetHierarchyNames(Agency, Dept, Program);
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

        /// <summary>
        /// INSERT AND UPDATE HIERARCHY information. 
        /// </summary>
        public bool InsertUpdateHierarchy(HierarchyEntity hierarchyEntity)
        {
            bool boolsuccess;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@HIE_AGENCY", hierarchyEntity.Agency));
                sqlParamList.Add(new SqlParameter("@HIE_DEPT", hierarchyEntity.Dept));

                sqlParamList.Add(new SqlParameter("@HIE_PROGRAM", hierarchyEntity.Prog));
                sqlParamList.Add(new SqlParameter("@HIE_SHORT_NAME", hierarchyEntity.ShortName));
                sqlParamList.Add(new SqlParameter("@HIE_ALT_INTAKE", hierarchyEntity.Intake));

                if (hierarchyEntity.HIERepresentation != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@HIE_REPRSNTN", hierarchyEntity.HIERepresentation));
                }

                sqlParamList.Add(new SqlParameter("@HIE_NAME", hierarchyEntity.HirarchyName));
                sqlParamList.Add(new SqlParameter("@HIE_PROG", hierarchyEntity.HIEProg));
                if (hierarchyEntity.CNFormat != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@HIE_CN_FORMAT", hierarchyEntity.CNFormat));
                }
                if (hierarchyEntity.CWFormat != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@HIE_CW_FORMAT", hierarchyEntity.CWFormat));
                }

                sqlParamList.Add(new SqlParameter("@HIE_LOGO", hierarchyEntity.Logo));

                sqlParamList.Add(new SqlParameter("@HIE_DATE_ADD", hierarchyEntity.DateAdd));
                sqlParamList.Add(new SqlParameter("@HIE_ADD_OPERATOR", hierarchyEntity.AddOperator));
                sqlParamList.Add(new SqlParameter("@HIE_DATE_LSTC", hierarchyEntity.DateLSTC));
                sqlParamList.Add(new SqlParameter("@HIE_LSTC_OPERATOR", hierarchyEntity.LSTCOperator));
                sqlParamList.Add(new SqlParameter("@MODE", hierarchyEntity.Mode));

                boolsuccess = HierarchyPlusProgram.InsertUpdateCASEHIE(sqlParamList);

            }
            catch (Exception ex)
            {
                //
                return false;
            }

            return boolsuccess;
        }

        /// <summary>
        /// Get Program information. 
        /// </summary>        
        /// <returns>Returns a DataSet with the screens.</returns>
        public List<ProgramDefinitionEntity> GetPrograms(string programName, string hierarchy,string UserID,string AdminAgency)
        {
            List<ProgramDefinitionEntity> programEntity = new List<ProgramDefinitionEntity>();
            try
            {
                DataSet programData = Captain.DatabaseLayer.HierarchyPlusProgram.GetProgramsSearch(programName, hierarchy,UserID, AdminAgency);
                if (programData != null && programData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in programData.Tables[0].Rows)
                    {
                        programEntity.Add(new ProgramDefinitionEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return programEntity;
            }

            return programEntity;
        }


        /// <summary>
        /// DELETE  HIERARCHY information. 
        /// </summary>
        public string DeleteHierarchy(HierarchyEntity hierarchyEntity)
        {

            string strMsg = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@HIE_AGENCY", hierarchyEntity.Agency));
                if (!hierarchyEntity.Dept.Trim().Equals(string.Empty))
                {
                    sqlParamList.Add(new SqlParameter("@HIE_DEPT", hierarchyEntity.Dept));
                }
                if (!hierarchyEntity.Prog.Trim().Equals(string.Empty))
                {
                    sqlParamList.Add(new SqlParameter("@HIE_PROGRAM", hierarchyEntity.Prog));
                }
                SqlParameter sqlmsg = new SqlParameter("@Msg", SqlDbType.VarChar, 10);
                sqlmsg.Direction = ParameterDirection.Output;
                sqlParamList.Add(sqlmsg);
                HierarchyPlusProgram.DeleteCASEHIE(sqlParamList);
                strMsg = sqlmsg.Value.ToString();
            }
            catch (Exception ex)
            {
                return strMsg;
            }

            return strMsg;
        }

        public List<ProgramDefinitionEntity> GetCaseGdl()
        {
            List<ProgramDefinitionEntity> CaseDepProfile = new List<ProgramDefinitionEntity>();
            try
            {
                DataSet CaseDepData = HierarchyPlusProgram.GetCASEDEP();
                if (CaseDepData != null && CaseDepData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseDepData.Tables[0].Rows)
                    {
                        CaseDepProfile.Add(new ProgramDefinitionEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return CaseDepProfile;
            }

            return CaseDepProfile;
        }

        public ProgramDefinitionEntity GetCaseDepadp(string Agency, string Dept, string Program)
        {
            ProgramDefinitionEntity programEntity = null;
            try
            {
                DataSet CaseDepData = HierarchyPlusProgram.GetCASEDEPadp(Agency, Dept, Program);
                if (CaseDepData != null && CaseDepData.Tables[0].Rows.Count > 0)
                {
                    programEntity = new ProgramDefinitionEntity(CaseDepData.Tables[0].Rows[0]);
                }
            }
            catch (Exception ex)
            {
                //
                return programEntity;
            }

            return programEntity;
        }

        public List<ProgramDefinitionEntity> GetCaseDep()
        {
            List<ProgramDefinitionEntity> programEntity = new List<ProgramDefinitionEntity>();
            try
            {
                DataSet CaseDepData = HierarchyPlusProgram.GetCASEDEP();
                if (CaseDepData != null && CaseDepData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseDepData.Tables[0].Rows)
                    {
                        programEntity.Add(new ProgramDefinitionEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return programEntity;
            }

            return programEntity;
        }

        public List<DepContactEntity> GetCASEDEPContacts(DepContactEntity programEntity)
        {
            List<DepContactEntity> CaseDepProfile = new List<DepContactEntity>();
            try
            {
                DataSet CaseDepData = HierarchyPlusProgram.GetCASEDEPContacts(programEntity.Agency, programEntity.Dept, programEntity.Program);
                if (CaseDepData != null && CaseDepData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseDepData.Tables[0].Rows)
                    {
                        CaseDepProfile.Add(new DepContactEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return CaseDepProfile;
            }

            return CaseDepProfile;
        }

        /// <summary>
        /// Get User Profile information. 
        /// </summary>
        /// <param name="userID">The CaseDep ID to get CaseDep.</param>
        /// <returns>Returns a DataSet with the CaseDep's profiles.</returns>
        public bool InsertCaseDep(ProgramDefinitionEntity programEntity)
        {
            bool boolsuccess;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@DEP_AGENCY", programEntity.Agency));
                sqlParamList.Add(new SqlParameter("@DEP_DEPT", programEntity.Dept));
                sqlParamList.Add(new SqlParameter("@DEP_PROGRAM", programEntity.Prog));

                if (programEntity.ProgramName != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_PROG", programEntity.ProgramName));
                }

                if (programEntity.ShortName != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_SHORT_NAME", programEntity.ShortName));
                }
                if (programEntity.DepAGCY != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_AGCY", programEntity.DepAGCY));
                }
                if (programEntity.Address1 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_ADDR1", programEntity.Address1));
                }
                if (programEntity.Address2 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_ADDR2", programEntity.Address2));
                }
                if (programEntity.Address3 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_ADDR3", programEntity.Address3));
                }
                if (programEntity.City != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_CITY", programEntity.City));
                }
                if (programEntity.Phone != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_PHONE", programEntity.Phone));
                }
                if (programEntity.DepFax != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_FAX", programEntity.DepFax));
                }
                if (programEntity.State != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_STATE", programEntity.State));
                }
                if (programEntity.Zip != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_ZIP", programEntity.Zip));
                }
                if (programEntity.ZipPlus != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_ZIP_PLUS", programEntity.ZipPlus));
                }
                if (programEntity.DepProg != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_PROG", programEntity.DepProg));
                }
                if (programEntity.DepYear != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_YEAR", programEntity.DepYear));
                }
                if (programEntity.DepFirstName != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_D_FN", programEntity.DepFirstName));
                }
                if (programEntity.DepMI != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_D_MI", programEntity.DepMI));
                }
                if (programEntity.DepLastName != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_D_LN", programEntity.DepLastName));
                }
                if (programEntity.DepGNO != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_G_NO", programEntity.DepGNO));
                }
                if (programEntity.DepDI != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_DI", programEntity.DepDI));
                }
                if (programEntity.DepAppNo != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_APP_NO", programEntity.DepAppNo));
                }
                if (programEntity.DepGenerateApps != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_GENERATE_APPS", programEntity.DepGenerateApps));
                }
                if (programEntity.DepAddressEdit != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_ADDRESS_EDIT", programEntity.DepAddressEdit));
                }
                if (programEntity.DepThreshold != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_THRESHOLD", programEntity.DepThreshold));
                }
                if (programEntity.DepAutoRefer != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_AUTO_REFER", programEntity.DepAutoRefer));
                }
                if (programEntity.DepJUVFromAge != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_JUV_FROM_AGE", programEntity.DepJUVFromAge));
                }
                if (programEntity.DepJUVToAge != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_JUV_TO_AGE", programEntity.DepJUVToAge));
                }
                if (programEntity.DepSENFromAge != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_SEN_FROM_AGE", programEntity.DepSENFromAge));
                }
                if (programEntity.DepSENToAge != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_SEN_TO_AGE", programEntity.DepSENToAge));
                }
                if (programEntity.DepFEDUsed != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_FED_USED", programEntity.DepFEDUsed));
                }
                if (programEntity.DepCMIUsed != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_CMI_USED", programEntity.DepCMIUsed));
                }
                if (programEntity.DepSMIUsed != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_SMI_USED", programEntity.DepSMIUsed));
                }
                if (programEntity.DepHUDUsed != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_HUD_USED", programEntity.DepHUDUsed));
                }
                if (programEntity.DepINCLSIM != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_INCL_SIM", programEntity.DepINCLSIM));
                }
                if (programEntity.DepIncomeTypes != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_INCOME_ARRAY", programEntity.DepIncomeTypes));
                }
                if (programEntity.DepReleataionTypes != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_RELATION_ARRAY", programEntity.DepReleataionTypes));
                }
                if (programEntity.DepCalcEligibility != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_CALC_ELIG", programEntity.DepCalcEligibility));
                }
                if (programEntity.DepUnitCalc != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_UNIT_CALC", programEntity.DepUnitCalc));
                }
                if (programEntity.Account != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@CNTL_ACCOUNT", programEntity.Account));
                }
                if (programEntity.Free1 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@CNTL_FREE_1", programEntity.Free1));
                }
                if (programEntity.Free2 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@CNTL_FREE_2", programEntity.Free2));
                }
                if (programEntity.Free3 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@CNTL_FREE_3", programEntity.Free3));
                }
                if (programEntity.Free4 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@CNTL_FREE_4", programEntity.Free4));
                }
                if (programEntity.Free5 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@CNTL_FREE_5", programEntity.Free5));
                }
                if (programEntity.Free6 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@CNTL_FREE_6", programEntity.Free6));
                }
                if (programEntity.Reduced1 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@CNTL_REDUCED_1", programEntity.Reduced1));
                }
                if (programEntity.Reduced2 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@CNTL_REDUCED_2", programEntity.Reduced2));
                }
                if (programEntity.Reduced3 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@CNTL_REDUCED_3", programEntity.Reduced3));
                }
                if (programEntity.Reduced4 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@CNTL_REDUCED_4", programEntity.Reduced4));
                }
                if (programEntity.Reduced5 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@CNTL_REDUCED_5", programEntity.Reduced5));
                }
                if (programEntity.Reduced6 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@CNTL_REDUCED_6", programEntity.Reduced6));
                }
                if (programEntity.Paid1 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@CNTL_PAID_1", programEntity.Paid1));
                }
                if (programEntity.Paid2 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@CNTL_PAID_2", programEntity.Paid2));
                }
                if (programEntity.Paid3 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@CNTL_PAID_3", programEntity.Paid3));
                }
                if (programEntity.Paid4 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@CNTL_PAID_4", programEntity.Paid4));
                }
                if (programEntity.Paid5 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@CNTL_PAID_5", programEntity.Paid5));
                }
                if (programEntity.Paid6 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@CNTL_PAID_6", programEntity.Paid6));
                }
                if (programEntity.Type != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@CNTL_TYPE", programEntity.Type));
                }
                if (programEntity.Source != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@CNTL_SOURCE", programEntity.Source));
                }
                if (programEntity.HSS != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@CASEDEP_HSS", programEntity.HSS));
                }
                if (programEntity.PRODUPSSN != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_PRO_DUP_SSN", programEntity.PRODUPSSN));
                }
                if (programEntity.StartMonth != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@CASEDEP_STARTMONTH", programEntity.StartMonth));
                }
                if (programEntity.EndMonth != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@CASEDEP_ENDMONTH", programEntity.EndMonth));
                }
                if (programEntity.CaseTypeEdit != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_CASETYPE_EDIT", programEntity.CaseTypeEdit));
                }
                if (programEntity.SelectedClient != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_SELECTED_CLIENT", programEntity.SelectedClient));
                }
                if (programEntity.IncomeVerMsg != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_INC_VER_MSG", programEntity.IncomeVerMsg));
                }
                if (programEntity.IncomeTypeOnly != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_INCOME_TYPE_ONLY", programEntity.IncomeTypeOnly));
                }
                if (programEntity.ProDupMEM != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_PRO_DUP_MEM", programEntity.ProDupMEM));
                }
                if (programEntity.IncomeWeek != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_INCOME_WEEK", programEntity.IncomeWeek));
                }
                if (programEntity.ZipSearch != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_ZIP_SRCH", programEntity.ZipSearch));
                }
                if (programEntity.AutoInActivation != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_AUTO_INACTIVATION", programEntity.AutoInActivation));
                }
                if (programEntity.SSNReasonFlag != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_SSN_REASON_FLAG", programEntity.SSNReasonFlag));
                }
                if (programEntity.SIMPointsMethod != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_SIM_POINTS_METHOD", programEntity.SIMPointsMethod));
                }
                if (programEntity.IntakeEdit != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_INTAKE_EDIT", programEntity.IntakeEdit));
                }
                if (programEntity.IntakeFDate != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_INTAKE_FDATE", programEntity.IntakeFDate));
                }
                if (programEntity.IntakeTDate != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_INTAKE_TDATE", programEntity.IntakeTDate));
                }
                if (programEntity.LoadProgramQuestions != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_LOAD_PROGRAM_QUESTIONS", programEntity.LoadProgramQuestions));
                }
                if (programEntity.SecretProgram != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_SECRET_PROGRAM", programEntity.SecretProgram));
                }
                if (programEntity.FuelCheckNo != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_FUEL_CHECK_NO", programEntity.FuelCheckNo));
                }
                if (programEntity.FuelPayNo != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_FUEL_PAY_NO", programEntity.FuelPayNo));
                }
                if (programEntity.FuelCheckDate != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_FUEL_CHECK_DATE", programEntity.FuelCheckDate));
                }
                if (programEntity.FuelVoucherNo != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_FUEL_VOUCH_NO", programEntity.FuelVoucherNo));
                }
                if (programEntity.IncomeDateValidate != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_INCOME_DATE_VALIDATE", programEntity.IncomeDateValidate));
                }
                if (programEntity.DepIncExcIntUsed1 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_INC_EXC_INT_USED_1", programEntity.DepIncExcIntUsed1));
                }
                if (programEntity.DepIncExcIntUsed2 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_INC_EXC_INT_USED_2", programEntity.DepIncExcIntUsed2));
                }
                if (programEntity.DepIncExcIntUsed3 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_INC_EXC_INT_USED_3", programEntity.DepIncExcIntUsed3));
                }
                if (programEntity.DepIncExcIntDefault1 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_INC_EXC_INT_DEFLT_1", programEntity.DepIncExcIntDefault1));
                }
                if (programEntity.DepIncExcIntDefault2 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_INC_EXC_INT_DEFLT_2", programEntity.DepIncExcIntDefault2));
                }
                if (programEntity.DepIncExcIntDefault3 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_INC_EXC_INT_DEFLT_3", programEntity.DepIncExcIntDefault3));
                }
                if (programEntity.DepIncExcIntFactr1 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_INC_EXC_INT_FACTR_1", programEntity.DepIncExcIntFactr1));
                }
                if (programEntity.DepIncExcIntFactr2 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_INC_EXC_INT_FACTR_2", programEntity.DepIncExcIntFactr2));
                }
                if (programEntity.DepIncExcIntFactr3 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_INC_EXC_INT_FACTR_3", programEntity.DepIncExcIntFactr3));
                }
                if (programEntity.DepIncludeIncVer != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_INCLD_INCVER", programEntity.DepIncludeIncVer));
                }
                if (programEntity.DepHsAgeMethod != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_HS_AGE_METHOD", programEntity.DepHsAgeMethod));
                }

                sqlParamList.Add(new SqlParameter("@DEP_INTAKE_PROG", programEntity.DepIntakeProg));

                if (programEntity.DepAttendanceTimes != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_ATT_TIMES", programEntity.DepAttendanceTimes));
                }
                if (programEntity.DepSsnAutoAssign != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_SSN_AUTOASSIGN", programEntity.DepSsnAutoAssign));
                }

                if (programEntity.DepSerpostPAYCAT != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_SERPOST_PAYCAT", programEntity.DepSerpostPAYCAT));
                }

                if (programEntity.PRODUPAPP_BY != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_PRO_DUP_APP_BY", programEntity.PRODUPAPP_BY));
                }
                if (programEntity.PRODUPMEM_BY != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_PRO_DUP_MEM_BY", programEntity.PRODUPMEM_BY));
                }

                if (programEntity.emailID != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEP_EMAIL", programEntity.emailID));
                }
                //if (programEntity.Search_Cntrl != string.Empty)
                //{
                //    sqlParamList.Add(new SqlParameter("@DEP_SEARCH_CNTL", programEntity.Search_Cntrl));
                //}


                sqlParamList.Add(new SqlParameter("@DEP_LSTC_OPERATOR", programEntity.LSTCOperator));
                sqlParamList.Add(new SqlParameter("@DEP_ADD_OPERATOR", programEntity.AddOperator));
                sqlParamList.Add(new SqlParameter("@Mode", programEntity.Mode));
                boolsuccess = HierarchyPlusProgram.InsertCASEDEP(sqlParamList);

            }
            catch (Exception ex)
            {
                //
                return false;
            }

            return boolsuccess;
        }

        /// <summary>
        /// Get User Profile information. 
        /// </summary>
        /// <param name="userID">The CaseDep ID to get CaseDep.</param>
        /// <returns>Returns a DataSet with the CaseDep's profiles.</returns>
        public bool InsertDepContact(DepContactEntity contactEntity)
        {
            bool boolsuccess;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@DEPCONT_AGENCY", contactEntity.Agency));
                sqlParamList.Add(new SqlParameter("@DEPCONT_DEPT", contactEntity.Dept));
                sqlParamList.Add(new SqlParameter("@DEPCONT_PROG", contactEntity.Program));

                if (contactEntity.SEQ != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEPCONT_SEQ", contactEntity.SEQ));
                }
                if (contactEntity.FirstName != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEPCONT_FNAME", contactEntity.FirstName));
                }
                if (contactEntity.LastName != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEPCONT_LNAME", contactEntity.LastName));
                }
                if (contactEntity.MI != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEPCONT_MNAME", contactEntity.MI));
                }
                if (contactEntity.StaffCode != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEPCONT_STAFF_CODE", contactEntity.StaffCode));
                }
                if (contactEntity.Phone1 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEPCONT_PHONE1", contactEntity.Phone1.Trim()));
                }
                if (contactEntity.Phone2 != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEPCONT_PHONE2", contactEntity.Phone2));
                }
                if (contactEntity.Fax != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEPCONT_FAX", contactEntity.Fax));
                }
                if (contactEntity.Email != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEPCONT_EMAIL", contactEntity.Email));
                }

                sqlParamList.Add(new SqlParameter("@DEPCONT_LSTC_OPERATOR", contactEntity.DepLstcOperator));
                sqlParamList.Add(new SqlParameter("@DEPCONT_ADD_OPERATOR", contactEntity.DepAddOperator));
                sqlParamList.Add(new SqlParameter("@Mode", contactEntity.Mode));
                boolsuccess = HierarchyPlusProgram.InsertDEPCONT(sqlParamList);

            }
            catch (Exception ex)
            {
                //
                return false;
            }

            return boolsuccess;
        }

        /// <summary>
        /// DELETE  HIERARCHY information. 
        /// </summary>
        public string DeleteCaseDep(ProgramDefinitionEntity programEntity)
        {
            string strMsg = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@AGENCY", programEntity.Agency));
                sqlParamList.Add(new SqlParameter("@DEPT", programEntity.Dept));
                sqlParamList.Add(new SqlParameter("@PROGRAM", programEntity.Prog));

                SqlParameter sqlmsg = new SqlParameter("@Msg", SqlDbType.VarChar, 10);
                sqlmsg.Direction = ParameterDirection.Output;
                sqlParamList.Add(sqlmsg);
                HierarchyPlusProgram.DeleteCASEDEP(sqlParamList);
                strMsg = sqlmsg.Value.ToString();
            }
            catch (Exception ex)
            {
                return strMsg;
            }

            return strMsg;
        }


        /// <summary>
        /// DELETE  HIERARCHY information. 
        /// </summary>
        public bool DeleteDepContact(DepContactEntity contactEntity)
        {
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@AGENCY", contactEntity.Agency));
                sqlParamList.Add(new SqlParameter("@DEPT", contactEntity.Dept));
                sqlParamList.Add(new SqlParameter("@PROGRAM", contactEntity.Program));
                sqlParamList.Add(new SqlParameter("@SEQ", contactEntity.SEQ));

                HierarchyPlusProgram.DeleteDEPCONT(sqlParamList);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }


        /// <summary>
        /// Insert update delete Casedep enrolls. 
        /// </summary>
        /// <param name="userID">The CaseDep ID to get CaseDep.</param>
        /// <returns>Returns a DataSet with the CaseDep's profiles.</returns>
        public bool InsertCaseDepEnrollHierachies(DepEnrollHierachiesEntity EnrollEntity)
        {
            bool boolsuccess;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                sqlParamList.Add(new SqlParameter("@DEPNRLHIE_AGENCY", EnrollEntity.Agency));
                sqlParamList.Add(new SqlParameter("@DEPNRLHIE_DEPT", EnrollEntity.Dept));
                sqlParamList.Add(new SqlParameter("@DEPNRLHIE_PROG", EnrollEntity.Program));

                if (EnrollEntity.Hierachies != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEPNRLHIE_HIE", EnrollEntity.Hierachies));
                }
                if (EnrollEntity.Nofoslots != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEPNRLHIE_FUND_SLOTS", EnrollEntity.Nofoslots));
                }
                if (EnrollEntity.StartDate != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEPNRLHIE_START_DATE", EnrollEntity.StartDate));
                }
                if (EnrollEntity.Enddate != string.Empty)
                {
                    sqlParamList.Add(new SqlParameter("@DEPNRLHIE_END_DATE", EnrollEntity.Enddate));
                }


                sqlParamList.Add(new SqlParameter("@DEPNRLHIE_LSTC_OPERATOR", EnrollEntity.DepLstcOperator));
                sqlParamList.Add(new SqlParameter("@DEPNRLHIE_ADD_OPERATOR", EnrollEntity.DepAddOperator));
                sqlParamList.Add(new SqlParameter("@Mode", EnrollEntity.Mode));
                boolsuccess = HierarchyPlusProgram.InsertCASEDEPEnrollHierachies(sqlParamList);

            }
            catch (Exception ex)
            {
                //
                return false;
            }

            return boolsuccess;
        }


        /// <summary>
        /// Get Hierarchy information. 
        /// </summary>
        /// <param name="userID">The user ID to get user profile.</param>
        /// <returns>Returns a DataSet with the screens.</returns>
        public List<DepEnrollHierachiesEntity> GetDepEntollHierachies(string Agency, string Dept, string Program, string Hierachy)
        {
            List<DepEnrollHierachiesEntity> DepEnrollEntity = new List<DepEnrollHierachiesEntity>();
            try
            {
                DataSet hierarchyData = Lookups.GetDepEntollHierachies(Agency, Dept, Program, Hierachy);
                if (hierarchyData != null && hierarchyData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in hierarchyData.Tables[0].Rows)
                    {
                        DepEnrollEntity.Add(new DepEnrollHierachiesEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                return DepEnrollEntity;
            }

            return DepEnrollEntity;
        }




        /// <summary>
        /// Get Hierarchy information. 
        /// </summary>
        /// <param name="userID">The user ID to get user profile.</param>
        /// <returns>Returns a DataSet with the screens.</returns>
        public List<HierarchyEntity> GetCaseHierarchyDepartment(string Agency, string Dept, string Program, string Year, string strApplicationNO, string Type)
        {
            List<HierarchyEntity> caseHierarchyEntity = new List<HierarchyEntity>();
            try
            {
                DataSet hierarchyData = Lookups.GetHierarchiesDepartment(Agency, Dept, Program, Year, strApplicationNO, Type);
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



        public List<HierarchyEntity> GetCaseHierachyAllData(string Agency, string Dept, string Program)
        {
            List<HierarchyEntity> CaseHierarchyEntityProfile = new List<HierarchyEntity>();
            try
            {
                DataSet CaseHierarchyEntityData = ADMNB001DB.ADMNB001_GetCashie(Agency, Dept, Program);
                if (CaseHierarchyEntityData != null && CaseHierarchyEntityData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in CaseHierarchyEntityData.Tables[0].Rows)
                    {
                        CaseHierarchyEntityProfile.Add(new HierarchyEntity(row, "CASEHIE"));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return CaseHierarchyEntityProfile;
            }

            return CaseHierarchyEntityProfile;
        }


    }
}