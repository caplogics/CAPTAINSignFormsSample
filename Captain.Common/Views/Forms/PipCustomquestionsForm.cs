#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Wisej.Web;
using System.Data.SqlClient;
using Captain.Common.Views.Forms.Base;
using Captain.Common.Model.Data;
using Captain.Common.Model.Objects;
using Captain.Common.Utilities;

#endregion

namespace Captain.Common.Views.Forms
{
    public partial class PipCustomquestionsForm : Form
    {
        #region private variables

        private ErrorProvider _errorProvider = null;
        private bool boolChangeStatus = false;
        private CaptainModel _model = null;


        #endregion
        public PipCustomquestionsForm(BaseForm baseForm, string strMode, string strAgency, string strDept, string strProgram, string strYear, string strApplNo, string strleanUserId, string strServices, string strFormType)
        {
            InitializeComponent();
            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            BaseForm = baseForm;
            CaseSnpEntity snpdata = new CaseSnpEntity();
            snpdata.Agency = strAgency;
            snpdata.Dept = strDept;
            snpdata.Program = strProgram;
            snpdata.Year = strYear;
            snpdata.App = strApplNo;
            this.Size = new Size(this.Width, 360);
            if (strMode == string.Empty)
            {
                LblHeader.Text = "Services Details";
                pnlServices.Location = new Point(0, 40);
                pnlQuestions.Visible = false;
                pnlServices.Visible = true;
                string strType = string.Empty;
                if (BaseForm.BaseAgencyControlDetails.ServinqCaseHie == "0")
                {
                    strType = "CAMAST";
                }
                else if (BaseForm.BaseAgencyControlDetails.ServinqCaseHie == "1")
                {
                    strType = "CASEHIE";
                }
                else if (BaseForm.BaseAgencyControlDetails.ServinqCaseHie == "2")
                {
                    strType = "CASESER";
                }

                DataSet serviceLDS = Captain.DatabaseLayer.CaseMst.GetSelectServicesByHIE(string.Empty, BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo);
                DataTable serviceLDT = serviceLDS.Tables[0];
                if (strFormType != string.Empty)
                {
                    this.Text = "Update Client Intake Data";
                    gvtLeanServices.HeaderText = "Services Inquired in Recent Intake";
                    gvtIntakeServices.HeaderText = "Services Inquired in Current Intake";

                    DataSet serviceSaveLDS = Captain.DatabaseLayer.CaseMst.GetSelectServicesByHIE("SAVE", BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo);
                    DataTable serviceSaveLDT = serviceSaveLDS.Tables[0];

                    DataSet serviceSaveDS = Captain.DatabaseLayer.CaseMst.GetSelectServicesByHIE("SAVE", strAgency, strDept, strProgram, strYear, strApplNo);
                    DataTable serviceSaveDT = serviceSaveDS.Tables[0];

                    foreach (DataRow dr in serviceLDT.Rows)
                    {
                        for (int i = 0; i < serviceSaveLDT.Rows.Count; i++)
                        {
                            if (Convert.ToString(dr["INQ_CODE"].ToString().Trim()) == serviceSaveLDT.Rows[i]["INQ_CODE"].ToString().Trim())
                            {
                                gvwIntakeServices.Rows.Add(dr["INQ_DESC"].ToString(), dr["INQ_DESC"].ToString());
                            }
                        }

                        for (int i = 0; i < serviceSaveDT.Rows.Count; i++)
                        {
                            if (Convert.ToString(dr["INQ_CODE"].ToString().Trim()) == serviceSaveDT.Rows[i]["INQ_CODE"].ToString().Trim())
                            {
                                gvwLeanServices.Rows.Add(dr["INQ_DESC"].ToString(), dr["INQ_DESC"].ToString());
                            }
                        }

                    }


                }
                else
                {
                    string strAgy = "00";
                    if (BaseForm.BaseAgencyControlDetails.PIPSwitch == "I")
                        strAgy = BaseForm.BaseAgency;
                    DataTable dtservice = PIPDATA.GETPIPSERVICES(BaseForm.BaseLeanDataBaseConnectionString, strType, BaseForm.BaseAgencyControlDetails.AgyShortName, strAgy);
                    foreach (DataRow drservices in dtservice.Rows)
                    {
                        if (strServices.Contains(drservices["CODE"].ToString()))
                            gvwLeanServices.Rows.Add(drservices["DESCRIP"].ToString(), drservices["CODE"].ToString());
                    }


                    DataSet serviceSaveDS = Captain.DatabaseLayer.CaseMst.GetSelectServicesByHIE("SAVE", strAgency, strDept, strProgram, strYear, strApplNo);
                    DataTable serviceSaveDT = serviceSaveDS.Tables[0];
                    foreach (DataRow dr in serviceLDT.Rows)
                    {
                        for (int i = 0; i < serviceSaveDT.Rows.Count; i++)
                        {
                            if (Convert.ToString(dr["INQ_CODE"].ToString().Trim()) == serviceSaveDT.Rows[i]["INQ_CODE"].ToString().Trim())
                            {
                                gvwIntakeServices.Rows.Add(dr["INQ_DESC"].ToString(), dr["INQ_DESC"].ToString());
                            }
                        }


                    }
                }





            }

            else
            {
                pnlServices.Visible = false;

                if (strFormType != string.Empty)
                {
                    this.Text = "Update Client Intake Data";

                    //List<CustomQuestionsEntity> custQuestions = _model.FieldControls.GetCustomQuestions("CASE2001", "A", HIE, seq, questionType, questionAccess);

                }
                else
                {
                    List<CustomQuestionsEntity> custResponses = _model.CaseMstData.GetCustomQuestionAnswers(snpdata);

                    List<CustomQuestionsEntity> lencustdata = PIPDATA.GETPIPADDCUSTList(BaseForm.BaseLeanDataBaseConnectionString, string.Empty, BaseForm.BaseAgencyControlDetails.AgyShortName, string.Empty, strleanUserId, string.Empty, "ADDCUSTRESP");
                    string strAgy = "00";
                    if (BaseForm.BaseAgencyControlDetails.PIPSwitch == "I")
                        strAgy = BaseForm.BaseAgency;

                    DataTable dttable = PIPDATA.GETPIPCUSTORRESPORADDCUST(BaseForm.BaseLeanDataBaseConnectionString, string.Empty, BaseForm.BaseAgencyControlDetails.AgyShortName, strAgy, string.Empty, string.Empty, "CUSTQUEST");

                    //DataTable dttable = GETLEANCUSTQUES(string.Empty, BaseForm.BaseAgencyControlDetails.AgyShortName, "FLDS");
                    foreach (DataRow dtitem in dttable.Rows)
                    {
                        CustomQuestionsEntity response = custResponses.Find(u => u.ACTCODE == dtitem["PCUST_CODE"].ToString() && u.ACTSNPFAMILYSEQ == "9999999");
                        List<CustRespEntity> custReponseEntity = _model.FieldControls.GetCustomResponses("CASE2001", dtitem["PCUST_CODE"].ToString());
                        string strOldValue = string.Empty;
                        string strNewValue = string.Empty;

                        if (dtitem["PCUST_RESP_TYPE"].ToString().Trim().Equals("D"))
                        {
                            if (response != null)
                            {

                                string code = response.ACTMULTRESP.Trim();
                                CustRespEntity custRespEntity = custReponseEntity.Find(u => u.DescCode.Trim().Equals(code));
                                if (custRespEntity != null)
                                {
                                    strOldValue = custRespEntity.RespDesc;
                                }
                            }
                            CustomQuestionsEntity leanresponse = lencustdata.Find(u => u.ACTCODE == dtitem["PCUST_CODE"].ToString());
                            if (leanresponse != null)
                            {
                                CustRespEntity custRespEntity = custReponseEntity.Find(u => u.DescCode.Trim().Equals(leanresponse.ACTMULTRESP.Trim()));
                                if (custRespEntity != null)
                                {
                                    strNewValue = custRespEntity.RespDesc;

                                }
                            }
                        }
                        else if (dtitem["PCUST_RESP_TYPE"].ToString().Trim().Equals("C"))
                        {
                            string custQuestionResp = string.Empty;
                            string custQuestionResp2 = string.Empty;
                            if (response != null)
                            {

                                string response1 = response.ACTMULTRESP.Trim();
                                if (!string.IsNullOrEmpty(response1))
                                {
                                    string[] arrResponse = null;
                                    if (response1.IndexOf(',') > 0)
                                    {
                                        arrResponse = response1.Split(',');
                                    }
                                    else if (!response1.Equals(string.Empty))
                                    {
                                        arrResponse = new string[] { response1 };
                                    }
                                    foreach (string stringitem in arrResponse)
                                    {
                                        CustRespEntity custRespEntity = custReponseEntity.Find(u => u.DescCode.Trim().Equals(stringitem));
                                        if (custRespEntity != null)
                                        {
                                            custQuestionResp += custRespEntity.RespDesc + ", ";
                                            //custQuestionCode += custResp.ACTMULTRESP.ToString() + " ";
                                        }
                                    }
                                }
                                if (custQuestionResp.Length > 1)
                                {
                                    custQuestionResp = custQuestionResp.Trim();
                                    if ((custQuestionResp.Substring(custQuestionResp.Length - 1)) == ",")
                                    {
                                        custQuestionResp = custQuestionResp.Remove(custQuestionResp.Length - 1, 1);
                                    }
                                }


                            }
                            strOldValue = custQuestionResp;
                            // custQuestionResp = response[0].ACTALPHARESP;



                            CustomQuestionsEntity leanresponse = lencustdata.Find(u => u.ACTCODE == dtitem["PCUST_CODE"].ToString());
                            if (leanresponse != null)
                            {

                                string response1 = leanresponse.ACTMULTRESP==null?string.Empty: leanresponse.ACTMULTRESP.ToString();
                                if (!string.IsNullOrEmpty(response1))
                                {
                                    string[] arrResponse = null;
                                    if (response1.IndexOf(',') > 0)
                                    {
                                        arrResponse = response1.Split(',');
                                    }
                                    else if (!response1.Equals(string.Empty))
                                    {
                                        arrResponse = new string[] { response1 };
                                    }
                                    foreach (string stringitem in arrResponse)
                                    {
                                        CustRespEntity custRespEntity = custReponseEntity.Find(u => u.DescCode.Trim().Equals(stringitem));
                                        if (custRespEntity != null)
                                        {
                                            custQuestionResp2 += custRespEntity.RespDesc + ", ";
                                            //custQuestionCode += custResp.ACTMULTRESP.ToString() + " ";
                                        }
                                    }
                                }
                                if (custQuestionResp2.Length > 1)
                                {
                                    custQuestionResp2 = custQuestionResp2.Trim();
                                    if ((custQuestionResp2.Substring(custQuestionResp2.Length - 1)) == ",")
                                    {
                                        custQuestionResp2 = custQuestionResp2.Remove(custQuestionResp2.Length - 1, 1);
                                    }
                                }


                            }
                            strNewValue = custQuestionResp2;

                        }
                        else if (dtitem["PCUST_RESP_TYPE"].ToString().Trim().Equals("N"))
                        {
                            if (response != null)
                            {
                                strOldValue = response.ACTNUMRESP.ToString();
                            }
                            CustomQuestionsEntity leanresponse = lencustdata.Find(u => u.ACTCODE == dtitem["PCUST_CODE"].ToString());
                            if (leanresponse != null)
                            {

                                strNewValue = leanresponse.ACTNUMRESP.ToString();
                            }


                        }
                        else if (dtitem["PCUST_RESP_TYPE"].ToString().Trim().Equals("T"))
                        {
                            if (response != null)
                            {
                                strOldValue = response.ACTDATERESP.ToString();
                            }
                            CustomQuestionsEntity leanresponse = lencustdata.Find(u => u.ACTCODE == dtitem["PCUST_CODE"].ToString());
                            if (leanresponse != null)
                            {

                                strNewValue = leanresponse.ACTDATERESP.ToString();
                            }

                        }
                        else
                        {
                            if (response != null)
                            {
                                strOldValue = response.ACTALPHARESP.ToString();
                            }
                            CustomQuestionsEntity leanresponse = lencustdata.Find(u => u.ACTCODE == dtitem["PCUST_CODE"].ToString());
                            if (leanresponse != null)
                            {

                                strNewValue = leanresponse.ACTALPHARESP.ToString();
                            }
                        }

                        gvwSubdetails.Rows.Add(dtitem["PCUST_DESC"].ToString(), strOldValue, strNewValue);
                    }
                }
            }
        }


        public PipCustomquestionsForm(BaseForm baseForm, string strMode, string strAgency, string strDept, string strProgram, string strYear, string strApplNo, string strleanUserId, string strServices, string strFormType, LeanIntakeEntity dritem)
        {
            InitializeComponent();
            _model = new CaptainModel();
            _errorProvider = new ErrorProvider(this);
            _errorProvider.BlinkRate = 3;
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _errorProvider.Icon = null;
            BaseForm = baseForm;
            CaseSnpEntity snpdata = new CaseSnpEntity();
            snpdata.Agency = strAgency;
            snpdata.Dept = strDept;
            snpdata.Program = strProgram;
            snpdata.Year = strYear;
            snpdata.App = strApplNo;
            this.Size = new Size(739, 306);
            if (strMode == "INCOMETYPES")
            {
                LblHeader.Text = "Income Types Compare";
                pnlServices.Location = new Point(0, 40);
                pnlQuestions.Visible = false;
                pnlServices.Visible = true;
                string strType = string.Empty;
                if (strFormType != string.Empty)
                {
                    this.Text = "Update Client Intake Data";
                    gvtLeanServices.HeaderText = "Income Types in Recent Intake";
                    gvtIntakeServices.HeaderText = "Income Types in Current Intake";
                    if (dritem != null)
                    {
                        CaseSnpEntity Basesnpdata = BaseForm.BaseCaseSnpEntity.Find(u => u.NameixFi.Trim().ToUpper().FirstOrDefault() == dritem.PIP_FNAME.Trim().ToUpper().FirstOrDefault() && LookupDataAccess.Getdate(u.AltBdate) == LookupDataAccess.Getdate(dritem.PIP_DOB));
                        if (Basesnpdata == null)
                            Basesnpdata = BaseForm.BaseCaseSnpEntity.Find(u => u.NameixFi.Trim().ToUpper() == dritem.PIP_FNAME.Trim().ToUpper() && u.NameixLast.Trim().ToUpper() == dritem.PIP_LNAME.Trim().ToUpper());

                        List<CaseIncomeEntity> _propCurrentIncomelist = _model.CaseMstData.GetCaseIncomeadpynf(BaseForm.BaseAgency, BaseForm.BaseDept, BaseForm.BaseProg, BaseForm.BaseYear, BaseForm.BaseApplicationNo, Basesnpdata.FamilySeq);

                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.INCOMETYPES, "**", "**", "**", string.Empty);

                        //var varincomelist = _propCurrentIncomelist.Distinct(u=>u.Type)
                        foreach (CaseIncomeEntity cincomeitem in _propCurrentIncomelist)
                        {

                            CommonEntity comOldvalue = listAgytabls.Find(u => u.Code.Trim() == cincomeitem.Type.Trim());
                            if (comOldvalue != null)
                            {
                                gvwIntakeServices.Rows.Add(comOldvalue.Desc, comOldvalue.Code);
                            }
                        }

                        // string[] strIncomeTypes = dritem.PIP_INCOME_TYPES.ToString().Trim().Split(',');
                        List<CaseIncomeEntity> _propRecentIncomelist = _model.CaseMstData.GetCaseIncomeadpynf(strAgency, strDept, strProgram, strYear, strApplNo, dritem.PIP_SEQ);


                        foreach (CaseIncomeEntity rincomeitem in _propRecentIncomelist)
                        {
                            CommonEntity comNewvalue = listAgytabls.Find(u => u.Code == rincomeitem.Type.Trim());
                            if (comNewvalue != null)
                            {
                                gvwLeanServices.Rows.Add(comNewvalue.Desc, comNewvalue.Code);
                            }
                        }
                    }
                }
                else
                {


                }

            }
            else if(strMode == "HEALTH INS CODE")
            {
                LblHeader.Text = "Health Insurances Compare";
                pnlServices.Location = new Point(0, 40);
                pnlQuestions.Visible = false;
                pnlServices.Visible = true;
                string strType = string.Empty;
                if (strFormType != string.Empty)
                {
                    this.Text = "Update Client Intake Data";
                    gvtLeanServices.HeaderText = "Health Insurances in Recent Intake";
                    gvtIntakeServices.HeaderText = "Health Insurances in Current Intake";
                    if (dritem != null)
                    {
                        CaseSnpEntity Basesnpdata = BaseForm.BaseCaseSnpEntity.Find(u => u.NameixFi.Trim().ToUpper().FirstOrDefault() == dritem.PIP_FNAME.Trim().ToUpper().FirstOrDefault() && LookupDataAccess.Getdate(u.AltBdate) == LookupDataAccess.Getdate(dritem.PIP_DOB));
                        if (Basesnpdata == null)
                            Basesnpdata = BaseForm.BaseCaseSnpEntity.Find(u => u.NameixFi.Trim().ToUpper() == dritem.PIP_FNAME.Trim().ToUpper() && u.NameixLast.Trim().ToUpper() == dritem.PIP_LNAME.Trim().ToUpper());

                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.HealthCodes, "**", "**", "**", string.Empty);



                        string[] strNCashben = Basesnpdata.Health_Codes.ToString().Trim().Split(',');
                        foreach (string Ncashitem in strNCashben)
                        {

                            CommonEntity comOldvalue = listAgytabls.Find(u => u.Code.Trim() == Ncashitem.Trim());
                            if (comOldvalue != null)
                            {
                                gvwIntakeServices.Rows.Add(comOldvalue.Desc, comOldvalue.Code);
                            }
                        }

                        string[] strPIPNCashben = dritem.PIP_HEALTH_CODES.ToString().Trim().Split(',');
                        foreach (string rincomeitem in strPIPNCashben)
                        {
                            CommonEntity comNewvalue = listAgytabls.Find(u => u.Code.Trim() == rincomeitem.Trim());
                            if (comNewvalue != null)
                            {
                                gvwLeanServices.Rows.Add(comNewvalue.Desc, comNewvalue.Code);
                            }
                        }

                    }
                }
                else
                {
                    this.Text = "Update Intake From PIP";
                    gvtLeanServices.HeaderText = "Health Insurances on PIP";
                    gvtIntakeServices.HeaderText = "Health Insurances in Captain Intake";
                    if (dritem != null)
                    {
                        List<CaseSnpEntity> casesnplist = _model.CaseMstData.GetCaseSnpadpyn(strAgency, strDept, strProgram, strYear, strApplNo);
                        CaseSnpEntity Basesnpdata = casesnplist.Find(u => u.NameixFi.Trim().ToUpper().FirstOrDefault() == dritem.PIP_FNAME.Trim().ToUpper().FirstOrDefault() && LookupDataAccess.Getdate(u.AltBdate) == LookupDataAccess.Getdate(dritem.PIP_DOB));
                        if (Basesnpdata == null)
                            Basesnpdata = casesnplist.Find(u => u.NameixFi.Trim().ToUpper() == dritem.PIP_FNAME.Trim().ToUpper() && u.NameixLast.Trim().ToUpper() == dritem.PIP_LNAME.Trim().ToUpper());

                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.HealthCodes, "**", "**", "**", string.Empty);



                        string[] strNCashben = Basesnpdata.Health_Codes.ToString().Trim().Split(',');
                        foreach (string Ncashitem in strNCashben)
                        {

                            CommonEntity comOldvalue = listAgytabls.Find(u => u.Code.Trim() == Ncashitem.Trim());
                            if (comOldvalue != null)
                            {
                                gvwIntakeServices.Rows.Add(comOldvalue.Desc, comOldvalue.Code);
                            }
                        }

                        string[] strPIPNCashben = dritem.PIP_HEALTH_CODES.ToString().Trim().Split(',');
                        foreach (string rincomeitem in strPIPNCashben)
                        {
                            CommonEntity comNewvalue = listAgytabls.Find(u => u.Code.Trim() == rincomeitem.Trim());
                            if (comNewvalue != null)
                            {
                                gvwLeanServices.Rows.Add(comNewvalue.Desc, comNewvalue.Code);
                            }
                        }

                    }
                }

            }

            else
            {
                LblHeader.Text = "Non Cash benefits Compare";
                pnlServices.Location = new Point(0, 40);
                pnlQuestions.Visible = false;
                pnlServices.Visible = true;
                string strType = string.Empty;
                if (strFormType != string.Empty)
                {
                    this.Text = "Update Client Intake Data";
                    gvtLeanServices.HeaderText = "Non Cash benefits in Recent Intake";
                    gvtIntakeServices.HeaderText = "Non Cash benefits in Current Intake";
                    if (dritem != null)
                    {
                        CaseSnpEntity Basesnpdata = BaseForm.BaseCaseSnpEntity.Find(u => u.NameixFi.Trim().ToUpper().FirstOrDefault() == dritem.PIP_FNAME.Trim().ToUpper().FirstOrDefault() && LookupDataAccess.Getdate(u.AltBdate) == LookupDataAccess.Getdate(dritem.PIP_DOB));
                        if (Basesnpdata == null)
                            Basesnpdata = BaseForm.BaseCaseSnpEntity.Find(u => u.NameixFi.Trim().ToUpper() == dritem.PIP_FNAME.Trim().ToUpper() && u.NameixLast.Trim().ToUpper() == dritem.PIP_LNAME.Trim().ToUpper());

                         List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.NonCashBenefits, "**", "**", "**", string.Empty);


                       
                        string[] strNCashben = Basesnpdata.NonCashBenefits.ToString().Trim().Split(',');
                        foreach (string Ncashitem in strNCashben)
                        {

                            CommonEntity comOldvalue = listAgytabls.Find(u => u.Code.Trim() == Ncashitem.Trim());
                            if (comOldvalue != null)
                            {
                                gvwIntakeServices.Rows.Add(comOldvalue.Desc, comOldvalue.Code);
                            }
                        }
                                              
                        string[] strPIPNCashben = dritem.PIP_NCASHBEN.ToString().Trim().Split(',');
                        foreach (string rincomeitem in strPIPNCashben)
                        {
                            CommonEntity comNewvalue = listAgytabls.Find(u => u.Code.Trim() == rincomeitem.Trim());
                            if (comNewvalue != null)
                            {
                                gvwLeanServices.Rows.Add(comNewvalue.Desc, comNewvalue.Code);
                            }
                        }

                    }
                }
                else
                {
                    this.Text = "Update Intake From PIP";
                    gvtLeanServices.HeaderText = "Non Cash benefits on PIP";
                    gvtIntakeServices.HeaderText = "Non Cash benefits in Captain Intake";
                    if (dritem != null)
                    {
                        List<CaseSnpEntity> casesnplist = _model.CaseMstData.GetCaseSnpadpyn(strAgency,strDept,strProgram,strYear,strApplNo);
                        CaseSnpEntity Basesnpdata = casesnplist.Find(u => u.NameixFi.Trim().ToUpper().FirstOrDefault() == dritem.PIP_FNAME.Trim().ToUpper().FirstOrDefault() && LookupDataAccess.Getdate(u.AltBdate) == LookupDataAccess.Getdate(dritem.PIP_DOB));
                        if (Basesnpdata == null)
                            Basesnpdata = casesnplist.Find(u => u.NameixFi.Trim().ToUpper() == dritem.PIP_FNAME.Trim().ToUpper() && u.NameixLast.Trim().ToUpper() == dritem.PIP_LNAME.Trim().ToUpper());

                        List<CommonEntity> listAgytabls = CommonFunctions.AgyTabsFilterCode(BaseForm.BaseAgyTabsEntity, Consts.AgyTab.NonCashBenefits, "**", "**", "**", string.Empty);



                        string[] strNCashben = Basesnpdata.NonCashBenefits.ToString().Trim().Split(',');
                        foreach (string Ncashitem in strNCashben)
                        {

                            CommonEntity comOldvalue = listAgytabls.Find(u => u.Code.Trim() == Ncashitem.Trim());
                            if (comOldvalue != null)
                            {
                                gvwIntakeServices.Rows.Add(comOldvalue.Desc, comOldvalue.Code);
                            }
                        }

                        string[] strPIPNCashben = dritem.PIP_NCASHBEN.ToString().Trim().Split(',');
                        foreach (string rincomeitem in strPIPNCashben)
                        {
                            CommonEntity comNewvalue = listAgytabls.Find(u => u.Code.Trim() == rincomeitem.Trim());
                            if (comNewvalue != null)
                            {
                                gvwLeanServices.Rows.Add(comNewvalue.Desc, comNewvalue.Code);
                            }
                        }

                    }
                }

            }


        }





        public BaseForm BaseForm { get; set; }


        private void Hepl_Click(object sender, EventArgs e)
        {

        }

        private void gvwIntakeServices_Click(object sender, EventArgs e)
        {

        }
    }
}