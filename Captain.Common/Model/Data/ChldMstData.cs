using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Captain.Common.Model.Objects;
using System.Data.SqlClient;
using Captain.DatabaseLayer;
using System.Data;
using Amazon.Runtime.Documents;

namespace Captain.Common.Model.Data
{
    public class ChldMstData
    {
        public ChldMstData(CaptainModel model)
        {
            Model = model;
        }

        #region Properties

        public CaptainModel Model { get; set; }

        public string UserId { get; set; }

        #endregion

        public bool InsertUpdateChldMst(ChldMstEntity ChldEntity)
        {
            bool boolSuccess = false;
            string strNewApplNo = string.Empty;
            string strNewClientId = string.Empty;
            string strNewFamilyId = string.Empty;
            string strNewSSNO = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@CHLDMST_AGENCY", ChldEntity.ChldMstAgency));
                sqlParamList.Add(new SqlParameter("@CHLDMST_DEPT", ChldEntity.ChldMstDept));
                sqlParamList.Add(new SqlParameter("@CHLDMST_PROGRAM", ChldEntity.ChldMstProgram));
                if (ChldEntity.ChldMstYr != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CHLDMST_YEAR", ChldEntity.ChldMstYr));
                sqlParamList.Add(new SqlParameter("@CHLDMST_APP_NO", ChldEntity.ApplNo));

                //if (ChldEntity.FamilySeq != string.Empty)
                //    sqlParamList.Add(new SqlParameter("@CHLDMST_FAMILY_SEQ", ChldEntity.FamilySeq));

                if (ChldEntity.FundSource != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CHLDMST_FUND_SOURCE", ChldEntity.FundSource));

                if (ChldEntity.AltFundSrc != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CHLDMST_ALT_FUND_SRC", ChldEntity.AltFundSrc));

                if (ChldEntity.ClassPrefer != string.Empty)//bit
                    sqlParamList.Add(new SqlParameter("@CHLDMST_CLASS_PREFER", ChldEntity.ClassPrefer));

                if (ChldEntity.Pob != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CHLDMST_POB", ChldEntity.Pob));

                if (ChldEntity.BirthCert != string.Empty)//bit
                    sqlParamList.Add(new SqlParameter("@CHLDMST_BIRTH_CERT", ChldEntity.BirthCert));

                if (ChldEntity.ChldReat != string.Empty)//bit
                    sqlParamList.Add(new SqlParameter("@CHLDMST_CHLD_REPEAT", ChldEntity.ChldReat));

                if (ChldEntity.NextYearPrep != string.Empty)//bit
                    sqlParamList.Add(new SqlParameter("@CHLDMST_NXTYEAR_PREP", ChldEntity.NextYearPrep));

                if (ChldEntity.PreClient != string.Empty)//bit
                    sqlParamList.Add(new SqlParameter("@CHLDMST_PRE_CLIENT", ChldEntity.PreClient));

                if (ChldEntity.Transport != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CHLDMST_TRANSPORT", ChldEntity.Transport));

                if (ChldEntity.MedSecurity != string.Empty)//bit
                    sqlParamList.Add(new SqlParameter("@CHLDMST_MED_SECURITY", ChldEntity.MedSecurity));

                if (ChldEntity.MedCoverage != string.Empty)//bit
                    sqlParamList.Add(new SqlParameter("@CHLDMST_MED_COVERAGE", ChldEntity.MedCoverage));

                if (ChldEntity.MedCoverType != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CHLDMST_MED_COVER_TYPE", ChldEntity.MedCoverType));

                if (ChldEntity.MedPlan != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CHLDMST_MED_PLAN", ChldEntity.MedPlan));

                if (ChldEntity.InsCat != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CHLDMST_INS_CAT", ChldEntity.InsCat));

                if (ChldEntity.MedInsurer != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CHLDMST_MED_INSURER", ChldEntity.MedInsurer));

                if (ChldEntity.DoctorName != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CHLDMST_DOCTOR_NAME", ChldEntity.DoctorName));

                if (ChldEntity.DoctorAddress != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CHLDMST_DOCTOR_ADDRESS", ChldEntity.DoctorAddress));

                if (ChldEntity.DoctorPhone != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CHLDMST_DOCTOR_PHONE", ChldEntity.DoctorPhone));

                if (ChldEntity.DentalCoverage != string.Empty)//bit
                    sqlParamList.Add(new SqlParameter("@CHLDMST_DENTAL_COVERAGE", ChldEntity.DentalCoverage));

                if (ChldEntity.DentalPlan != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CHLDMST_DENTAL_PLAN", ChldEntity.DentalPlan));

                if (ChldEntity.DentistName != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CHLDMST_DENTIST_NAME", ChldEntity.DentistName));

                if (ChldEntity.DentalInsurer != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CHLDMST_DENTAL_INSURER", ChldEntity.DentalInsurer));

                if (ChldEntity.DentistAddress != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CHLDMST_DENTIST_ADDRESS", ChldEntity.DentistAddress));

                if (ChldEntity.DentistPhone != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CHLDMST_DENTIST_PHONE", ChldEntity.DentistPhone));

                if (ChldEntity.Disability != string.Empty)//bit
                    sqlParamList.Add(new SqlParameter("@CHLDMST_DISABILITY", ChldEntity.Disability));

                if (ChldEntity.DisabilityType != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CHLDMST_DISABILITY_TYPE", ChldEntity.DisabilityType));

                if (ChldEntity.DiagnosisDate != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CHLDMST_DIAGNOSIS_DATE", ChldEntity.DiagnosisDate));

                if (ChldEntity.Allergies != string.Empty)//bit
                    sqlParamList.Add(new SqlParameter("@CHLDMST_ALLERGIES", ChldEntity.Allergies));

                if (ChldEntity.DietaryRes != string.Empty)//bit
                    sqlParamList.Add(new SqlParameter("@CHLDMST_DIETARY_RES", ChldEntity.DietaryRes));

                if (ChldEntity.Medications != string.Empty)//bit
                    sqlParamList.Add(new SqlParameter("@CHLDMST_MEDICATIONS", ChldEntity.Medications));

                if (ChldEntity.MedicalCond != string.Empty)//bit
                    sqlParamList.Add(new SqlParameter("@CHLDMST_MEDICAL_COND", ChldEntity.MedicalCond));

                if (ChldEntity.HhConcerns != string.Empty)//bit
                    sqlParamList.Add(new SqlParameter("@CHLDMST_HH_CONCERNS", ChldEntity.HhConcerns));

                if (ChldEntity.DevlpmntlConcern != string.Empty)//bit
                    sqlParamList.Add(new SqlParameter("@CHLDMST_DEVLPMNTL_CONCERN", ChldEntity.DevlpmntlConcern));

                if (ChldEntity.PickOff != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CHLDMST_PICKUP", ChldEntity.PickOff));

                if (ChldEntity.DropOff != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CHLDMST_DROPOFF", ChldEntity.DropOff));

                sqlParamList.Add(new SqlParameter("@CHLDMST_LSTC_OPERATOR", ChldEntity.LstcOperator));

                sqlParamList.Add(new SqlParameter("@CHLDMST_ADD_OPERATOR", ChldEntity.AddOperator));

                sqlParamList.Add(new SqlParameter("@Mode", ChldEntity.Mode));

                boolSuccess = ChldMst.InsertUpdateDelChldMst(sqlParamList);

            }
            catch (Exception ex)
            {

                boolSuccess = false;
            }

            return boolSuccess;
        }

        public ChldMstEntity GetChldMstDetails(string agency, string dep, string program, string year, string app, string seq)
        {
            ChldMstEntity ChldMstDetails = null;
            try
            {
                DataSet ChldMst = Captain.DatabaseLayer.ChldMst.GetChldMstDetails(agency, dep, program, year, app, seq);
                if (ChldMst != null && ChldMst.Tables[0].Rows.Count > 0)
                {
                    ChldMstDetails = new ChldMstEntity(ChldMst.Tables[0].Rows[0]);
                }
            }
            catch (Exception ex)
            {
                //
                return ChldMstDetails;
            }

            return ChldMstDetails;
        }


        public List<ChldMstEntity> GetChldMstListDetails(string agency, string dep, string program, string year, string app, string seq)
        {
            List<ChldMstEntity> ChldMstDetails = new List<ChldMstEntity>();
            try
            {
                DataSet ChldMst = Captain.DatabaseLayer.ChldMst.GetChldMstDetails(agency, dep, program, year, app, seq);
                if (ChldMst != null && ChldMst.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ChldMst.Tables[0].Rows)
                    {
                        ChldMstDetails.Add(new ChldMstEntity(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return ChldMstDetails;
            }

            return ChldMstDetails;
        }


        public List<ChldMstEntity> GetChldMstcase2530Report(string agency, string dep, string program, string year, string app, string seq, string strApplicantDetails)
        {
            List<ChldMstEntity> ChldMstDetails = new List<ChldMstEntity>();
            try
            {
                DataSet ChldMst = Captain.DatabaseLayer.ChldMst.GetChldMstcase2530Report(agency, dep, program, year, app, seq, strApplicantDetails);
                if (ChldMst != null && ChldMst.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ChldMst.Tables[0].Rows)
                    {
                        ChldMstDetails.Add(new ChldMstEntity(row, "case2530Report"));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return ChldMstDetails;
            }

            return ChldMstDetails;
        }



        public bool InsertUpdateChldEMEM(ChldMstEMEMEntitty ChldEMEMEntity)
        {
            bool boolSuccess = false;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@EMER_AGENCY", ChldEMEMEntity.ChldMstAgency));
                sqlParamList.Add(new SqlParameter("@EMER_DEPT", ChldEMEMEntity.ChldMstDept));
                sqlParamList.Add(new SqlParameter("@EMER_PROG", ChldEMEMEntity.ChldMstProgram));
                if (ChldEMEMEntity.ChldMstYr != string.Empty)
                    sqlParamList.Add(new SqlParameter("@EMER_YEAR", ChldEMEMEntity.ChldMstYr));
                sqlParamList.Add(new SqlParameter("@EMER_APP_NO", ChldEMEMEntity.ApplNo));

                if (ChldEMEMEntity.FamilySeq != string.Empty)
                    sqlParamList.Add(new SqlParameter("@EMER_SEQ", ChldEMEMEntity.FamilySeq));

                if (ChldEMEMEntity.EMERName != string.Empty)
                    sqlParamList.Add(new SqlParameter("@EMER_NAME", ChldEMEMEntity.EMERName));

                if (ChldEMEMEntity.EmerRel != string.Empty)
                    sqlParamList.Add(new SqlParameter("@EMER_REL", ChldEMEMEntity.EmerRel));

                if (ChldEMEMEntity.EmerAddress1 != string.Empty)//bit
                    sqlParamList.Add(new SqlParameter("@EMER_ADDR1", ChldEMEMEntity.EmerAddress1));

                if (ChldEMEMEntity.EmerAddress2 != string.Empty)
                    sqlParamList.Add(new SqlParameter("@EMER_ADDR2", ChldEMEMEntity.EmerAddress2));

                if (ChldEMEMEntity.EmerPhone1 != string.Empty)//bit
                    sqlParamList.Add(new SqlParameter("@EMER_PHONE1", ChldEMEMEntity.EmerPhone1));

                if (ChldEMEMEntity.EmerPhone2 != string.Empty)//bit
                    sqlParamList.Add(new SqlParameter("@EMER_PHONE2", ChldEMEMEntity.EmerPhone2));

                if (ChldEMEMEntity.Emode != string.Empty)
                    sqlParamList.Add(new SqlParameter("@Mode", ChldEMEMEntity.Emode));

                boolSuccess = ChldMst.InsertUpdateDelChldEmer(sqlParamList);

            }
            catch (Exception ex)
            {

                boolSuccess = false;
            }

            return boolSuccess;
        }


        public ChldMstEMEMEntitty GetChldEmemDetails(string agency, string dep, string program, string year, string app, string seq)
        {
            ChldMstEMEMEntitty ChldEmemDetails = null;
            try
            {
                DataSet ChldMst = Captain.DatabaseLayer.ChldMst.GetChldEmerDetails(agency, dep, program, year, app, seq);
                if (ChldMst != null && ChldMst.Tables[0].Rows.Count > 0)
                {
                    ChldEmemDetails = new ChldMstEMEMEntitty(ChldMst.Tables[0].Rows[0]);
                }
            }
            catch (Exception ex)
            {
                //
                return ChldEmemDetails;
            }

            return ChldEmemDetails;
        }


        public List<ChldMstEMEMEntitty> GetChldEmemList(string agency, string dep, string program, string year, string app, string seq)
        {
            List<ChldMstEMEMEntitty> ChldEmemDetails = new List<ChldMstEMEMEntitty>();
            try
            {
                DataSet ChldEMem = Captain.DatabaseLayer.ChldMst.GetChldEmerDetails(agency, dep, program, year, app, seq);
                if (ChldEMem != null && ChldEMem.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ChldEMem.Tables[0].Rows)
                    {
                        ChldEmemDetails.Add(new ChldMstEMEMEntitty(row));
                    }
                }
            }
            catch (Exception ex)
            {
                //
                return ChldEmemDetails;
            }

            return ChldEmemDetails;
        }

        public List<ChldMstEntity> GetChldMstDetailsList(string agency, string dep, string program, string year, string app, string seq)
        {
            List<ChldMstEntity> ChldMstDetails = new List<ChldMstEntity>();
            try
            {
                DataSet ChldMst = Captain.DatabaseLayer.ChldMst.GetChldMstDetails(agency, dep, program, year, app, seq);
                if (ChldMst != null && ChldMst.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ChldMst.Tables[0].Rows)
                    {
                        ChldMstDetails.Add(new ChldMstEntity(row));
                    }

                }
            }
            catch (Exception ex)
            {
                //
                return ChldMstDetails;
            }

            return ChldMstDetails;
        }


        public CaseCondEntitty GetCaseCondDetails(string agency, string dep, string program, string year, string app, string seq)
        {
            CaseCondEntitty ChldEmemDetails = null;
            try
            {
                DataSet CondDs = Captain.DatabaseLayer.ChldMst.GetCaseCondDetails(agency, dep, program, year, app, seq);
                if (CondDs != null && CondDs.Tables[0].Rows.Count > 0)
                {
                    ChldEmemDetails = new CaseCondEntitty(CondDs.Tables[0].Rows[0]);
                }
            }
            catch (Exception ex)
            {
                //
                return ChldEmemDetails;
            }

            return ChldEmemDetails;
        }


        public List<CaseCondEntitty> GetCaseCondDetailsList(string agency, string dep, string program, string year, string app, string seq)
        {
            List<CaseCondEntitty> ChldEmemDetails = new List<CaseCondEntitty>();
            try
            {
                DataSet ChldMst = Captain.DatabaseLayer.ChldMst.GetCaseCondDetails(agency, dep, program, year, app, seq);
                if (ChldMst != null && ChldMst.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ChldMst.Tables[0].Rows)
                    {
                        ChldEmemDetails.Add(new CaseCondEntitty(row));
                    }

                }
            }
            catch (Exception ex)
            {
                //
                return ChldEmemDetails;
            }

            return ChldEmemDetails;

        }


        public bool InsertUpdateDelCASECOND(CaseCondEntitty casecondEntity)
        {
            bool boolSuccess = false;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@CASECOND_AGENCY", casecondEntity.CaseCondAgency));
                sqlParamList.Add(new SqlParameter("@CASECOND_DEPT", casecondEntity.CaseCondDept));
                sqlParamList.Add(new SqlParameter("@CASECOND_PROG", casecondEntity.CaseCondProgram));
                if (casecondEntity.CaseCondYr != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CASECOND_YEAR", casecondEntity.CaseCondYr));
                sqlParamList.Add(new SqlParameter("@CASECOND_APP_NO", casecondEntity.ApplNo));

                if (casecondEntity.Allergy != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CASECOND_ALLERGY", casecondEntity.Allergy));

                if (casecondEntity.DietRestrct != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CASECOND_DIET_RESTRCT", casecondEntity.DietRestrct));

                if (casecondEntity.Medications != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CASECOND_MEDICATIONS", casecondEntity.Medications));

                if (casecondEntity.MedConds != string.Empty)//bit
                    sqlParamList.Add(new SqlParameter("@CASECOND_MED_CONDS", casecondEntity.MedConds));

                if (casecondEntity.HHConcerns != string.Empty)
                    sqlParamList.Add(new SqlParameter("@CASECOND_HH_CONCRNS", casecondEntity.HHConcerns));

                if (casecondEntity.DevlConcerns != string.Empty)//bit
                    sqlParamList.Add(new SqlParameter("@CASECOND_DEVL_CONCRNS", casecondEntity.DevlConcerns));
                if (casecondEntity.AddOperator != string.Empty)//bit
                    sqlParamList.Add(new SqlParameter("@CASECOND_ADD_OPERATOR", casecondEntity.AddOperator));
                if (casecondEntity.LstcOperator != string.Empty)//bit
                    sqlParamList.Add(new SqlParameter("@CASECOND_LSTC_OPERATOR", casecondEntity.LstcOperator));


                if (casecondEntity.Emode != string.Empty)
                    sqlParamList.Add(new SqlParameter("@Mode", casecondEntity.Emode));

                boolSuccess = ChldMst.InsertUpdateDelCASECOND(sqlParamList);

            }
            catch (Exception ex)
            {

                boolSuccess = false;
            }

            return boolSuccess;
        }


        public List<IMGUPLOGNEntity> GetImgUpLogList(string strAgency, string strDept, string strProg, string strYear, string app, string strFamseq, string screen,string strId)
        {
            List<IMGUPLOGNEntity> imgLogDetails = new List<IMGUPLOGNEntity>();
            try
            {
                DataSet ImgLogData = Captain.DatabaseLayer.ChldMst.GETIMGUPLOG(strAgency, strDept, strProg, strYear, app, strFamseq, screen,strId);
                if (ImgLogData != null && ImgLogData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ImgLogData.Tables[0].Rows)
                    {
                        imgLogDetails.Add(new IMGUPLOGNEntity(row));
                    }

                }
            }
            catch (Exception ex)
            {
                //
                return imgLogDetails;
            }

            return imgLogDetails;

        }

        public List<INVDOCLOGEntity> GetInvDocsLogList(string strAgency, string strDept, string strProg, string strYear, string app, string ServPlan, string SpmSeq, string strType,string strid)
        {
            List<INVDOCLOGEntity> imgLogDetails = new List<INVDOCLOGEntity>();
            try
            {
                DataSet ImgLogData = Captain.DatabaseLayer.ChldMst.GETInvDocLOG(strAgency, strDept, strProg, strYear, app, ServPlan, SpmSeq, strType, strid);
                if (ImgLogData != null && ImgLogData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ImgLogData.Tables[0].Rows)
                    {
                        imgLogDetails.Add(new INVDOCLOGEntity(row));
                    }

                }
            }
            catch (Exception ex)
            {
                //
                return imgLogDetails;
            }

            return imgLogDetails;

        }

        public List<PARTNERDOCSEntity> GetPartnerDocsList(string PartnerId)
        {
            List<PARTNERDOCSEntity> imgLogDetails = new List<PARTNERDOCSEntity>();
            try
            {
                DataSet ImgLogData = Captain.DatabaseLayer.ChldMst.GETPartnerDocs(PartnerId);
                if (ImgLogData != null && ImgLogData.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ImgLogData.Tables[0].Rows)
                    {
                        imgLogDetails.Add(new PARTNERDOCSEntity(row));
                    }

                }
            }
            catch (Exception ex)
            {
                //
                return imgLogDetails;
            }

            return imgLogDetails;

        }

        public bool InsertIMGUPLOG(IMGUPLOGNEntity imglogEntity)
        {
            bool boolSuccess = false;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                if (imglogEntity.IMGLOG_ID != string.Empty)
                    sqlParamList.Add(new SqlParameter("@IMGLOG_ID", imglogEntity.IMGLOG_ID));
                sqlParamList.Add(new SqlParameter("@IMGLOG_AGY", imglogEntity.IMGLOG_AGY));
                sqlParamList.Add(new SqlParameter("@IMGLOG_DEP", imglogEntity.IMGLOG_DEP));
                sqlParamList.Add(new SqlParameter("@IMGLOG_PROG", imglogEntity.IMGLOG_PROG));
                sqlParamList.Add(new SqlParameter("@IMGLOG_YEAR", imglogEntity.IMGLOG_YEAR));
                sqlParamList.Add(new SqlParameter("@IMGLOG_APP", imglogEntity.IMGLOG_APP));
                if (imglogEntity.IMGLOG_FAMILY_SEQ != string.Empty)
                    sqlParamList.Add(new SqlParameter("@IMGLOG_FAMILY_SEQ", imglogEntity.IMGLOG_FAMILY_SEQ));
                sqlParamList.Add(new SqlParameter("@IMGLOG_SCREEN", imglogEntity.IMGLOG_SCREEN));
                if (imglogEntity.IMGLOG_SECURITY != string.Empty)
                    sqlParamList.Add(new SqlParameter("@IMGLOG_SECURITY", imglogEntity.IMGLOG_SECURITY));
                if (imglogEntity.IMGLOG_TYPE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@IMGLOG_TYPE", imglogEntity.IMGLOG_TYPE));
                sqlParamList.Add(new SqlParameter("@IMGLOG_UPLoadAs", imglogEntity.IMGLOG_UPLoadAs));
                sqlParamList.Add(new SqlParameter("@IMGLOG_UPLOAD_BY", imglogEntity.IMGLOG_UPLOAD_BY));
                if (imglogEntity.IMGLOG_ORIG_FILENAME != string.Empty)
                    sqlParamList.Add(new SqlParameter("@IMGLOG_ORIG_FILENAME", imglogEntity.IMGLOG_ORIG_FILENAME));
                sqlParamList.Add(new SqlParameter("@Mode", imglogEntity.MODE));

                boolSuccess = ChldMst.InsertIMGUPNLOG(sqlParamList);

            }
            catch (Exception ex)
            {

                boolSuccess = false;
            }

            return boolSuccess;
        }


        public bool InsertIMGUPLOG(IMGUPLOGNEntity imglogEntity, out string strimglogid)
        {
            bool boolSuccess = false;
            string strInvoiceidout = string.Empty;
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                if (imglogEntity.IMGLOG_ID != string.Empty)
                    sqlParamList.Add(new SqlParameter("@IMGLOG_ID", imglogEntity.IMGLOG_ID));
                sqlParamList.Add(new SqlParameter("@IMGLOG_AGY", imglogEntity.IMGLOG_AGY));
                sqlParamList.Add(new SqlParameter("@IMGLOG_DEP", imglogEntity.IMGLOG_DEP));
                sqlParamList.Add(new SqlParameter("@IMGLOG_PROG", imglogEntity.IMGLOG_PROG));
                sqlParamList.Add(new SqlParameter("@IMGLOG_YEAR", imglogEntity.IMGLOG_YEAR));
                sqlParamList.Add(new SqlParameter("@IMGLOG_APP", imglogEntity.IMGLOG_APP));
                if (imglogEntity.IMGLOG_FAMILY_SEQ != string.Empty)
                    sqlParamList.Add(new SqlParameter("@IMGLOG_FAMILY_SEQ", imglogEntity.IMGLOG_FAMILY_SEQ));
                sqlParamList.Add(new SqlParameter("@IMGLOG_SCREEN", imglogEntity.IMGLOG_SCREEN));
                if (imglogEntity.IMGLOG_SECURITY != string.Empty)
                    sqlParamList.Add(new SqlParameter("@IMGLOG_SECURITY", imglogEntity.IMGLOG_SECURITY));
                if (imglogEntity.IMGLOG_TYPE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@IMGLOG_TYPE", imglogEntity.IMGLOG_TYPE));
                sqlParamList.Add(new SqlParameter("@IMGLOG_UPLoadAs", imglogEntity.IMGLOG_UPLoadAs));
                sqlParamList.Add(new SqlParameter("@IMGLOG_UPLOAD_BY", imglogEntity.IMGLOG_UPLOAD_BY));
                if (imglogEntity.IMGLOG_ORIG_FILENAME != string.Empty)
                    sqlParamList.Add(new SqlParameter("@IMGLOG_ORIG_FILENAME", imglogEntity.IMGLOG_ORIG_FILENAME));
                sqlParamList.Add(new SqlParameter("@Mode", imglogEntity.MODE));

                SqlParameter sqlInvoiceid = new SqlParameter("@IMGLOGIDOUT", SqlDbType.Int, 10);
                sqlInvoiceid.Value = imglogEntity.IMGLOG_ID;
                sqlInvoiceid.Direction = ParameterDirection.Output;
                sqlParamList.Add(sqlInvoiceid);


                boolSuccess = ChldMst.InsertIMGUPNLOG(sqlParamList);
                strInvoiceidout = sqlInvoiceid.Value.ToString();
            }
            catch (Exception ex)
            {

                boolSuccess = false;
            }
            strimglogid = strInvoiceidout.ToString();
            return boolSuccess;
        }

        public bool InsertINVDOCLOG(INVDOCLOGEntity imglogEntity)
        {
            bool boolSuccess = false;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                if (imglogEntity.INVDOC_ID != string.Empty)
                    sqlParamList.Add(new SqlParameter("@INVDOC_ID", imglogEntity.INVDOC_ID));
                sqlParamList.Add(new SqlParameter("@INVDOC_AGENCY", imglogEntity.INVDOC_AGENCY));
                sqlParamList.Add(new SqlParameter("@INVDOC_DEPT", imglogEntity.INVDOC_DEPT));
                sqlParamList.Add(new SqlParameter("@INVDOC_PROGRAM", imglogEntity.INVDOC_PROGRAM));
                sqlParamList.Add(new SqlParameter("@INVDOC_YEAR", imglogEntity.INVDOC_YEAR));
                sqlParamList.Add(new SqlParameter("@INVDOC_APP", imglogEntity.INVDOC_APP));
                if (imglogEntity.INVDOC_SERVICEPLAN != string.Empty)
                    sqlParamList.Add(new SqlParameter("@INVDOC_SERVICEPLAN", imglogEntity.INVDOC_SERVICEPLAN));
                sqlParamList.Add(new SqlParameter("@INVDOC_SPM_SEQ", imglogEntity.INVDOC_SPM_SEQ));
                if (imglogEntity.INVDOC_VEND_TYPE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@INVDOC_VEND_TYPE", imglogEntity.INVDOC_VEND_TYPE));
                if (imglogEntity.INVDOC_SEQ != string.Empty)
                    sqlParamList.Add(new SqlParameter("@INVDOC_SEQ", imglogEntity.INVDOC_SEQ));
                if (imglogEntity.INVDOC_VENDOR != string.Empty)
                    sqlParamList.Add(new SqlParameter("@INVDOC_VENDOR", imglogEntity.INVDOC_VENDOR));
                if (imglogEntity.INVDOC_SERVICE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@INVDOC_SERVICE", imglogEntity.INVDOC_SERVICE));
                sqlParamList.Add(new SqlParameter("@INVDOC_UPLOAD_AS", imglogEntity.INVDOC_UPLOAD_AS));
                sqlParamList.Add(new SqlParameter("@INVDOC_UPLOAD_BY", imglogEntity.INVDOC_UPLOAD_BY));
                if (imglogEntity.INVDOC_ORIG_NAME != string.Empty)
                    sqlParamList.Add(new SqlParameter("@INVDOC_ORIG_NAME", imglogEntity.INVDOC_ORIG_NAME));
                if (imglogEntity.INVDOC_ACTID != string.Empty)
                    sqlParamList.Add(new SqlParameter("@INVDOC_ACTID", imglogEntity.INVDOC_ACTID));
                sqlParamList.Add(new SqlParameter("@Mode", imglogEntity.MODE));

                boolSuccess = ChldMst.InsertINVDOCNLOG(sqlParamList);

            }
            catch (Exception ex)
            {

                boolSuccess = false;
            }

            return boolSuccess;
        }



        public bool BRAINIMGES_INSUPDEL(IMGUPLOGNEntity imglogEntity)
        {
            bool boolSuccess = false;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                if (imglogEntity.IMGLOG_ID != string.Empty)
                    sqlParamList.Add(new SqlParameter("@IMGLOG_ID", imglogEntity.IMGLOG_ID));
                sqlParamList.Add(new SqlParameter("@IMGLOG_AGY", imglogEntity.IMGLOG_AGY));
                sqlParamList.Add(new SqlParameter("@IMGLOG_DEP", imglogEntity.IMGLOG_DEP));
                sqlParamList.Add(new SqlParameter("@IMGLOG_PROG", imglogEntity.IMGLOG_PROG));
                sqlParamList.Add(new SqlParameter("@IMGLOG_YEAR", imglogEntity.IMGLOG_YEAR));
                sqlParamList.Add(new SqlParameter("@IMGLOG_APP", imglogEntity.IMGLOG_APP));
                if (imglogEntity.IMGLOG_FAMILY_SEQ != string.Empty)
                    sqlParamList.Add(new SqlParameter("@IMGLOG_FAMILY_SEQ", imglogEntity.IMGLOG_FAMILY_SEQ));
                sqlParamList.Add(new SqlParameter("@IMGLOG_SCREEN", imglogEntity.IMGLOG_SCREEN));
                if (imglogEntity.IMGLOG_SECURITY != string.Empty)
                    sqlParamList.Add(new SqlParameter("@IMGLOG_SECURITY", imglogEntity.IMGLOG_SECURITY));
                if (imglogEntity.IMGLOG_TYPE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@IMGLOG_TYPE", imglogEntity.IMGLOG_TYPE));
                sqlParamList.Add(new SqlParameter("@IMGLOG_UPLoadAs", imglogEntity.IMGLOG_UPLoadAs));
                sqlParamList.Add(new SqlParameter("@IMGLOG_DATE_UPLOAD", imglogEntity.IMGLOG_DATE_UPLOAD_Dt));
                sqlParamList.Add(new SqlParameter("@IMGLOG_UPLOAD_BY", imglogEntity.IMGLOG_UPLOAD_BY));
                if (imglogEntity.IMGLOG_ORIG_FILENAME != string.Empty)
                    sqlParamList.Add(new SqlParameter("@IMGLOG_ORIG_FILENAME", imglogEntity.IMGLOG_ORIG_FILENAME));
                sqlParamList.Add(new SqlParameter("@Mode", imglogEntity.MODE));
                boolSuccess = ChldMst.BRAINIMGES_INSUPDEL(sqlParamList);

            }
            catch (Exception ex)
            {

                boolSuccess = false;
            }

            return boolSuccess;
        }


        public bool InsertPARTNERDOCS(PARTNERDOCSEntity DocsEntity)
        {
            bool boolSuccess = false;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@PRED_ID", DocsEntity.PRED_ID));
                sqlParamList.Add(new SqlParameter("@PRED_OPERATN", DocsEntity.PRED_OPERATN));
                sqlParamList.Add(new SqlParameter("@PRED_FILENAME", DocsEntity.PRED_FILENAME));
                sqlParamList.Add(new SqlParameter("@PRED_DESC", DocsEntity.PRED_DESC));
                sqlParamList.Add(new SqlParameter("@PRED_REFER", DocsEntity.PRED_REFER));
                sqlParamList.Add(new SqlParameter("@PRED_ULOADAS", DocsEntity.PRED_ULOADAS));
                sqlParamList.Add(new SqlParameter("@PRED_USER", DocsEntity.PRED_USER));
                boolSuccess = ChldMst.InsertPartnerdocs(sqlParamList);

            }
            catch (Exception ex)
            {

                boolSuccess = false;
            }

            return boolSuccess;
        }


        public CASEBIOEntitty GetCaseBioDetails(string agency, string dep, string program, string year, string app)
        {
            CASEBIOEntitty CaseBioDetails = null;
            try
            {
                DataSet CaseBioDs = Captain.DatabaseLayer.ChldMst.GetCaseBioDetails(agency, dep, program, year, app);
                if (CaseBioDs != null && CaseBioDs.Tables[0].Rows.Count > 0)
                {
                    CaseBioDetails = new CASEBIOEntitty(CaseBioDs.Tables[0].Rows[0]);
                }
            }
            catch (Exception ex)
            {
                //
                return CaseBioDetails;
            }

            return CaseBioDetails;
        }




        public bool InsertUpdateDelCASEBIO(CASEBIOEntitty casebioEntity)
        {
            bool boolSuccess = false;

            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();

                sqlParamList.Add(new SqlParameter("@BIO_AGENCY", casebioEntity.BIO_AGENCY));
                sqlParamList.Add(new SqlParameter("@BIO_DEPT", casebioEntity.BIO_DEPT));
                sqlParamList.Add(new SqlParameter("@BIO_PROGRAM", casebioEntity.BIO_PROGRAM));
                if (casebioEntity.BIO_YEAR != string.Empty)
                    sqlParamList.Add(new SqlParameter("@BIO_YEAR", casebioEntity.BIO_YEAR));
                sqlParamList.Add(new SqlParameter("@BIO_APP_NO", casebioEntity.BIO_APP_NO));

                if (casebioEntity.BIO_MOT_FNAME != string.Empty)
                    sqlParamList.Add(new SqlParameter("@BIO_MOT_FNAME", casebioEntity.BIO_MOT_FNAME));

                if (casebioEntity.BIO_MOT_LNAME != string.Empty)
                    sqlParamList.Add(new SqlParameter("@BIO_MOT_LNAME", casebioEntity.BIO_MOT_LNAME));

                if (casebioEntity.BIO_MOT_STREET != string.Empty)
                    sqlParamList.Add(new SqlParameter("@BIO_MOT_STREET", casebioEntity.BIO_MOT_STREET));

                if (casebioEntity.BIO_MOT_CITY != string.Empty)
                    sqlParamList.Add(new SqlParameter("@BIO_MOT_CITY", casebioEntity.BIO_MOT_CITY));

                if (casebioEntity.BIO_MOT_STATE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@BIO_MOT_STATE", casebioEntity.BIO_MOT_STATE));

                if (casebioEntity.BIO_MOT_ZIP != string.Empty)
                    sqlParamList.Add(new SqlParameter("@BIO_MOT_ZIP", casebioEntity.BIO_MOT_ZIP));

                if (casebioEntity.BIO_MOT_ZIPPLUS != string.Empty)
                    sqlParamList.Add(new SqlParameter("@BIO_MOT_ZIPPLUS", casebioEntity.BIO_MOT_ZIPPLUS));

                if (casebioEntity.BIO_MOT_PHONE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@BIO_MOT_PHONE", casebioEntity.BIO_MOT_PHONE));

                if (casebioEntity.BIO_MOT_CELL != string.Empty)
                    sqlParamList.Add(new SqlParameter("@BIO_MOT_CELL", casebioEntity.BIO_MOT_CELL));

                if (casebioEntity.BIO_FAT_FNAME != string.Empty)
                    sqlParamList.Add(new SqlParameter("@BIO_FAT_FNAME", casebioEntity.BIO_FAT_FNAME));

                if (casebioEntity.BIO_FAT_LNAME != string.Empty)
                    sqlParamList.Add(new SqlParameter("@BIO_FAT_LNAME", casebioEntity.BIO_FAT_LNAME));

                if (casebioEntity.BIO_FAT_STREET != string.Empty)
                    sqlParamList.Add(new SqlParameter("@BIO_FAT_STREET", casebioEntity.BIO_FAT_STREET));

                if (casebioEntity.BIO_FAT_CITY != string.Empty)
                    sqlParamList.Add(new SqlParameter("@BIO_FAT_CITY", casebioEntity.BIO_FAT_CITY));

                if (casebioEntity.BIO_FAT_STATE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@BIO_FAT_STATE", casebioEntity.BIO_FAT_STATE));

                if (casebioEntity.BIO_FAT_ZIP != string.Empty)
                    sqlParamList.Add(new SqlParameter("@BIO_FAT_ZIP", casebioEntity.BIO_FAT_ZIP));

                if (casebioEntity.BIO_FAT_ZIPPLUS != string.Empty)
                    sqlParamList.Add(new SqlParameter("@BIO_FAT_ZIPPLUS", casebioEntity.BIO_FAT_ZIPPLUS));

                if (casebioEntity.BIO_FAT_PHONE != string.Empty)
                    sqlParamList.Add(new SqlParameter("@BIO_FAT_PHONE", casebioEntity.BIO_FAT_PHONE));

                if (casebioEntity.BIO_FAT_CELL != string.Empty)
                    sqlParamList.Add(new SqlParameter("@BIO_FAT_CELL", casebioEntity.BIO_FAT_CELL));

                if (casebioEntity.BIO_MOT_RELATION != string.Empty)
                    sqlParamList.Add(new SqlParameter("@BIO_MOT_RELATION", casebioEntity.BIO_MOT_RELATION));

                if (casebioEntity.BIO_FAT_RELATION != string.Empty)
                    sqlParamList.Add(new SqlParameter("@BIO_FAT_RELATION", casebioEntity.BIO_FAT_RELATION));

                if (casebioEntity.BIO_ADD_OPERATOR != string.Empty)
                    sqlParamList.Add(new SqlParameter("@BIO_ADD_OPERATOR", casebioEntity.BIO_ADD_OPERATOR));

                if (casebioEntity.BIO_LSTC_OPERATOR != string.Empty)
                    sqlParamList.Add(new SqlParameter("@BIO_LSTC_OPERATOR", casebioEntity.BIO_LSTC_OPERATOR));

                if (casebioEntity.Mode != string.Empty)
                    sqlParamList.Add(new SqlParameter("@Mode", casebioEntity.Mode));

                boolSuccess = ChldMst.InsertUpdateDelCASEBIO(sqlParamList);

            }
            catch (Exception ex)
            {

                boolSuccess = false;
            }

            return boolSuccess;
        }



    }
}
